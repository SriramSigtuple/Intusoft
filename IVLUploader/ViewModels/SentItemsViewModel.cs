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
        private static Logger logger = LogManager.GetCurrentClassLogger();

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
            logger.Info("SentItems VM Constructor, Analysis Type {0}", analysisType.ToString("g"));

            AnalysisType = analysisType;
            activeDirPath = GlobalMethods.GetDirPath(DirectoryEnum.ActiveDir, AnalysisType);
            sentItemsDirPath = GlobalMethods.GetDirPath(DirectoryEnum.SentItemsDir, AnalysisType);
            if (activeFileCloudVM == null)
            {
                activeFileCloudVM = new CloudViewModel();
                activeFileCloudVM.startStopEvent += ActiveFileCloudVM_startStopEvent;
                activeFileCloudVM.AnalysisType = AnalysisType;
                activeFileCloudVM.CreatePendingFilesEvent += ActiveFileCloudVM_CreatePendingFilesEvent;
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
            if(sentItemsDirFileInfoArr.Any())
            activeFileCloudVM.ActiveFnf = sentItemsDirFileInfoArr.First();

            //SetValue = new RelayCommand(param=> SetValueMethod(param));
            

        }

        private void ActiveFileCloudVM_CreatePendingFilesEvent(AnalysisType _analysisType)
        {
            if(analysisType == _analysisType)
            CreateMissingPendingFiles();
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

            logger.Info("SentItems VM Timer, Analysis Type {0}", analysisType.ToString("g"));

                sentItemsDirFileInfoArr = new DirectoryInfo(sentItemsDirPath).GetFiles("*.json");

            fileIndx = 0;
            if(!activeFileCloudVM.IsBusy && sentItemsDirFileInfoArr.Any())
            {
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
                if (!File.Exists(Path.Combine(sentItemsDirPath, activeDirFileInfoArr[0].Name.Split('.')[0] + "_pending")))
                {
                    StreamWriter st1 = new StreamWriter(Path.Combine(sentItemsDirPath, activeDirFileInfoArr[0].Name.Split('.')[0] + "_pending"), false);
                    st1.Flush();
                    st1.Close();
                    st1.Dispose();
                }
            }
            StartStopSentItemsTimer(!activeFileCloudVM.IsBusy);
        }

        private void RecursiveMethod()
        {
            if (fileIndx < sentItemsDirFileInfoArr.Length)
            {
                
                if(!activeFileCloudVM.IsBusy)
                {
                    GetFileFromActiveDir(sentItemsDirFileInfoArr[fileIndx]);

                }
                fileIndx++;

                RecursiveMethod();

            }
            else
                fileIndx = 0;
        }
        private  void GetFileFromActiveDir(FileInfo activeDirFileInfo)
        {
             
                try
                {
                   if( File.Exists(activeDirFileInfo.FullName))
                    {
                             if (!activeFileCloudVM.IsBusy)
                            {
                        logger.Info($"file Name ={activeDirFileInfo.Name}");

                        //Console.WriteLine("Get sent items file in active vm finf is null");

                        UpdateActiveCloudVM(activeDirFileInfo);

                            }
                    }
                   
                }
                catch (Exception ex)
                {
                    logger.Info(ex);
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
                    StartStopSentItemsTimer(!activeFileCloudVM.IsBusy);
                    StreamReader st = new StreamReader(fileInfo.FullName);
                    var json = st.ReadToEnd();
                    st.Close();
                    st.Dispose();
                    //Console.WriteLine("sent items {0}", fileInfo.Name);
                    File.Delete(Path.Combine(fileInfo.Directory.FullName, fileInfo.Name.Split('.')[0] + "_pending"));

                    CloudModel activeFileCloudModel = JsonConvert.DeserializeObject<CloudModel>(json);

                    activeFileCloudVM.ActiveFnf = fileInfo;

                    activeFileCloudVM.SetCloudModel(activeFileCloudModel);

                    activeFileCloudVM.StartAnalsysisFlow();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                activeFileCloudVM.IsBusy = false;
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
