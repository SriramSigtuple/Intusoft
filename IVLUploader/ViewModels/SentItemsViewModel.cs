using BaseViewModel;
using Cloud_Models.Models;
using Newtonsoft.Json;
using NLog;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Input;

namespace IntuUploader.ViewModels
{
    /// <summary>
    /// Class which implements the check for internet connection by pinging to 8.8.8.8 of google
    /// </summary>
    public class SentItemsViewModel : ViewBaseModel
    {
        private  Logger logger;// = LogManager.GetLogger("Logger");
        static Logger exceptionLog = LogManager.GetLogger("ExceptionLogger");

        Timer SentItemsStatusCheckTimer;
        int timeout = 10000;// TODO : to be configured
        int timerTick = 100000;// TODO : to be configured
        public CloudViewModel activeFileCloudVM;
        int fileIndx = 0;
        int retryCount = 0;
        private static SentItemsViewModel _sentItemsViewModel;
        FileInfo[] sentItemsDirFileInfoArr;
        private AnalysisType analysisType;

        public AnalysisType AnalysisType
        {
            get { return analysisType; }
            set { analysisType = value;
                OnPropertyChanged("AnalysisType");
            }
        }

        private string activeDirPath = string.Empty;
        private string sentItemsDirPath = string.Empty;
        /// <summary>
        /// Constructor
        /// </summary>
        public SentItemsViewModel(AnalysisType analysisType)
        {
            AnalysisType = analysisType;

            if (AnalysisType == AnalysisType.Fundus)
                logger = LogManager.GetLogger("FundusLogger");
            else
                logger = LogManager.GetLogger("QILogger");
            logger.Info("SentItems VM Constructor, Analysis Type {0}", analysisType.ToString("g"));

            

            activeDirPath = GlobalMethods.GetDirPath(DirectoryEnum.ActiveDir, AnalysisType);
            sentItemsDirPath = GlobalMethods.GetDirPath(DirectoryEnum.SentItemsDir, AnalysisType);
            if (activeFileCloudVM == null)
            {
                activeFileCloudVM = new CloudViewModel(AnalysisType);
                activeFileCloudVM.startStopEvent += ActiveFileCloudVM_startStopEvent;
                activeFileCloudVM.CreatePendingFilesEvent += ActiveFileCloudVM_CreatePendingFilesEvent;
                activeFileCloudVM.Write_R_Move_File_Event += ActiveFileCloudVM_Write_R_Move_File_Event;
            }
            SentItemsStatusCheckTimer = new System.Threading.Timer(SentItemsStatusCheckTimerCallback, null, -1, (int)(GlobalVariables.UploaderSettings.OutboxTimerInterval * 1000));// new Timer((int)(GlobalVariables.UploaderSettings.OutboxTimerInterval * 1000));// 

            //SentItemsStatusCheckTimer = new Timer((int)(GlobalVariables.UploaderSettings.SentItemsTimerInterval * 1000));
            //SentItemsStatusCheckTimer.Elapsed += SentItemsStatusCheckTimer_Elapsed;

            sentItemsDirFileInfoArr = new DirectoryInfo(sentItemsDirPath).GetFiles("*.json");
            foreach (var item in sentItemsDirFileInfoArr)
            {
                if(!File.Exists(Path.Combine(sentItemsDirPath, item.Name.Split('.')[0] + "_pending")))
                {
                    StreamWriter st1 = new StreamWriter(Path.Combine(sentItemsDirPath, item.Name.Split('.')[0] + "_pending"), false);
                    st1.Flush();
                    st1.Close();
                    st1.Dispose();
                }
               
            }
            //if(sentItemsDirFileInfoArr.Any())
            //activeFileCloudVM.ActiveFnf = sentItemsDirFileInfoArr.First();

            //SetValue = new RelayCommand(param=> SetValueMethod(param));
            

        }

        private void ActiveFileCloudVM_Write_R_Move_File_Event(AnalysisType analysisType)
        {
            var filePath = string.Empty;
            File.Delete(activeFileCloudVM.ActiveFnf.FullName);

            if (activeFileCloudVM.IsMove2NextDir)
            {
                filePath = Path.Combine(GlobalMethods.GetDirPath(activeFileCloudVM.nextDirectory, AnalysisType), activeFileCloudVM.ActiveFnf.Name);
                
            }
            else
                filePath = activeFileCloudVM.ActiveFnf.FullName;

            using (StreamWriter st = new StreamWriter(filePath))
            {
                st.Write(JsonConvert.SerializeObject(activeFileCloudVM.ActiveCloudModel, Formatting.Indented));
                st.Flush();
                st.Close();
                st.Dispose();

            }
            activeFileCloudVM.IsBusy = false;
        }

        private void ActiveFileCloudVM_CreatePendingFilesEvent(AnalysisType _analysisType)
        {
            if (analysisType == _analysisType)
                CreateMissingPendingFiles();
            //StartStopSentItemsTimer(true);
        }

        //private void SentItemsStatusCheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    SentItemsStatusCheckTimerCallback(new object());
        //}

        ///// <summary>
        ///// Implementing singleton pattern in order to handle to values across the module
        ///// </summary>
        ///// <returns></returns>
        //public static SentItemsViewModel GetInstance()
        //{


        //    if (_sentItemsViewModel == null)
        //        _sentItemsViewModel = new SentItemsViewModel();

        //    return _sentItemsViewModel;
        //}

        /// <summary>
        /// Method to get Files from SentItems directory
        /// </summary>
        /// <param name="state"></param>
        private void SentItemsStatusCheckTimerCallback(object state)
        {

            CreateMissingPendingFiles();
            logger.Info("SentItems VM Timer, Analysis Type {0}", analysisType.ToString("g"));

                sentItemsDirFileInfoArr = new DirectoryInfo(sentItemsDirPath).GetFiles("*.json");

            fileIndx = 0;

            if (!activeFileCloudVM.IsBusy && sentItemsDirFileInfoArr.Any())
            {
                StartStopSentItemsTimer(false);

                if (activeFileCloudVM.ActiveFnf != null)
                {
                    if (sentItemsDirFileInfoArr.Where(x => x.FullName == activeFileCloudVM.ActiveFnf.FullName).ToList().Any())
                    {
                        var indx = sentItemsDirFileInfoArr.ToList().FindIndex(x => x.FullName == activeFileCloudVM.ActiveFnf.FullName);

                        if (indx < sentItemsDirFileInfoArr.Length - 1)
                            fileIndx = indx + 1;
                    }
                }
               
                logger.Info($"{analysisType.ToString("g")}  {fileIndx} {sentItemsDirFileInfoArr[fileIndx]}");
                // if(activeFileCloudVM.ActiveFnf == null || !activeFileCloudVM.IsBusy)
                GetFileFromActiveDir(sentItemsDirFileInfoArr[fileIndx]);

            }
            //else
            //    StartStopSentItemsTimer(true);



            //for (int i = fileIndx; i < sentItemsDirFileInfoArr.Length; i++, fileIndx++)
            //    {
            //    if (File.Exists(Path.Combine(sentItemsDirPath, sentItemsDirFileInfoArr[i].Name.Split('.')[0] + "_pending")))
            //    {
            //        GetFileFromActiveDir(sentItemsDirFileInfoArr[fileIndx]);
            //    }


            //    }
            //    if (fileIndx == sentItemsDirFileInfoArr.Length)
            //        fileIndx = 0;


        }

        public void CreateMissingPendingFiles()
        {

            FileInfo[] activeDirFileInfoArr = new DirectoryInfo(sentItemsDirPath).GetFiles("*.json");
            //the below code is to write a pending file if any existing file in active directory has no pending file.
            if (activeDirFileInfoArr.Any())
            {
                foreach (var item in activeDirFileInfoArr)
                {
                    if (!File.Exists(Path.Combine(sentItemsDirPath, item.Name.Split('.')[0] + "_pending")))
                    {
                        StreamWriter st1 = new StreamWriter(Path.Combine(sentItemsDirPath, item.Name.Split('.')[0] + "_pending"), false);
                        st1.Flush();
                        st1.Close();
                        st1.Dispose();
                    }
                }
               
            }
        }

        private  void GetFileFromActiveDir(FileInfo activeDirFileInfo)
        {
             
                try
                {
                if (File.Exists(activeDirFileInfo.FullName))
                {
                    if (!activeFileCloudVM.IsBusy)
                    {
                        logger.Info($"file Name ={activeDirFileInfo.Name}");

                        //Console.WriteLine("Get sent items file in active vm finf is null");

                        UpdateActiveCloudVM(activeDirFileInfo);

                    }
                    //else
                    //    StartStopSentItemsTimer(true);

                }
                else
                    StartStopSentItemsTimer(true);



            }
            catch (Exception ex)
                {
                    logger.Info(ex);
                exceptionLog.Error(Common.Exception2StringConverter.GetInstance().ConvertException2String(ex));

                activeFileCloudVM.IsBusy = false;
                }

            

        }

        private  void UpdateActiveCloudVM(FileInfo fileInfo)
        {
            try
            {
                
                if (File.Exists(Path.Combine(fileInfo.Directory.FullName, fileInfo.Name.Split('.')[0] + "_pending")))
                {

                    //StartStopSentItemsTimer(false);
                    activeFileCloudVM.IsBusy = true;
                    activeFileCloudVM.ActiveFnf = fileInfo;

                    var json = string.Empty;

                    using (StreamReader sr = new StreamReader(fileInfo.FullName))
                    {
                        json = sr.ReadToEnd();
                        sr.Close();
                        sr.Dispose();
                    }//st = new StreamReader(fileInfo.FullName);

                    //var json = st.ReadToEnd();
                    //st.Close();
                    //st.Dispose();
                    //GC.Collect();
                    //Console.WriteLine("sent items {0}", fileInfo.Name);
                    File.Delete(Path.Combine(fileInfo.Directory.FullName, fileInfo.Name.Split('.')[0] + "_pending"));

                    CloudModel activeFileCloudModel = JsonConvert.DeserializeObject<CloudModel>(json);


                    activeFileCloudVM.SetCloudModel(activeFileCloudModel);
                    activeFileCloudVM.StartAnalysisFlow();
                   
                    //activeFileCloudVM.IsBusy = false;

                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                exceptionLog.Error(Common.Exception2StringConverter.GetInstance().ConvertException2String(ex));
                activeFileCloudVM.IsBusy = false;

            }
            finally
            {
                StartStopSentItemsTimer(true);
            }


        }
        private void ActiveFileCloudVM_startStopEvent(bool isStart)
        {
            StartStopSentItemsTimer(isStart);
        }
        public void StartStopSentItemsTimer(bool isStart)
        {
            if (isStart)
            {
                //SentItemsStatusCheckTimer.Start();
                //SentItemsStatusCheckTimerCallback(new object());
                SentItemsStatusCheckTimer.Change(0, (int)(GlobalVariables.UploaderSettings.SentItemsTimerInterval * 1000));
            }
               
            //SentItemsStatusCheckTimer = new System.Threading.Timer(SentItemsStatusCheckTimerCallback, null, 0, (int)(GlobalVariables.UploaderSettings.SentItemsTimerInterval * 1000));
            else
            {
                SentItemsStatusCheckTimer.Change(-1,-1);

                //SentItemsStatusCheckTimer.Stop();
                //SentItemsStatusCheckTimer = new System.Threading.Timer(SentItemsStatusCheckTimerCallback, null, 0, Timeout.Infinite);
            }

        }

        public ICommand SetValue
        {
            get;
            set;
        }


        public void SetValueMethod(object param)
        {
            //this.FileUploadStatus = 100;
        }

    }
}
