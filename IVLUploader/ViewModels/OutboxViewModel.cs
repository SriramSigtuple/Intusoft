using BaseViewModel;
using Cloud_Models.Models;
using IntuUploader;
using Newtonsoft.Json;
using NLog;
using System;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Input;
using System.Timers;
using System.Windows;

namespace IVLUploader.ViewModels
{
    /// <summary>
    /// Class which implements the check for internet connection by pinging to 8.8.8.8 of google
    /// </summary>
    public class OutboxViewModel : ViewBaseModel
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        Timer OutboxFileChecker;
        int timeout = 10000;// TODO : to be configured
        int timerTick = 40000;// TODO : to be configured
        public CloudViewModel activeFileCloudVM;

        int retryCount = 0;
        private static OutboxViewModel _outboxViewModel;
        /// <summary>
        /// Constructor
        /// </summary>
        private OutboxViewModel()
        {
            

            if (activeFileCloudVM == null)
            {
                activeFileCloudVM = new CloudViewModel();
                activeFileCloudVM.startStopEvent += ActiveFileCloudVM_startStopEvent;

            }
            OutboxFileChecker = new Timer((int)(GlobalVariables.UploaderSettings.OutboxTimerInterval * 1000));// new System.Threading.Timer(OutBoxTimerCallback, null, 0, (int)(GlobalVariables.UploaderSettings.OutboxTimerInterval * 1000));
            OutboxFileChecker.Elapsed += OutboxFileChecker_Elapsed;
            FileInfo[] activeDirFileInfoArr = new DirectoryInfo(GlobalMethods.GetDirPath(DirectoryEnum.ActiveDir)).GetFiles("*.json");
            //the below code is to write a pending file if any existing file in active directory has no pending file.
            if (activeDirFileInfoArr.Any())
            {
                if (!File.Exists(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.ActiveDir), activeDirFileInfoArr[0].Name.Split('.')[0] + "_pending")))
                {
                    StreamWriter st1 = new StreamWriter(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.ActiveDir), activeDirFileInfoArr[0].Name.Split('.')[0] + "_pending"), false);
                    st1.Flush();
                    st1.Close();
                    st1.Dispose();
                }
            }
            
            


        }

        private void OutboxFileChecker_Elapsed(object sender, ElapsedEventArgs e)
        {
            OutBoxTimerCallback(new object());
        }

        //private void OutboxFileChecker_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    OutBoxTimerCallback(new object());
        //}

        /// <summary>
        /// Implementing singleton pattern in order to handle to values across the module
        /// </summary>
        /// <returns></returns>
        public static OutboxViewModel GetInstance()
        {
            

            if (_outboxViewModel == null)
                _outboxViewModel = new OutboxViewModel();
            

            return _outboxViewModel;
        }

        /// <summary>
        /// Method to get Files from outbox to active directory
        /// </summary>
        /// <param name="state"></param>
        private void OutBoxTimerCallback(object state)
        {
            

            FileInfo[] outboxDirFileInfoArr = new DirectoryInfo(GlobalMethods.GetDirPath(DirectoryEnum.OutboxDir)).GetFiles("*.json");


            FileInfo[] activeDirFileInfoArr = new DirectoryInfo(GlobalMethods.GetDirPath(DirectoryEnum.ActiveDir)).GetFiles("*.json");
            if (!(activeDirFileInfoArr.Any()) && outboxDirFileInfoArr.Any())
            {
                logger.Info(JsonConvert.SerializeObject(activeDirFileInfoArr, Formatting.Indented));

                try
                {
                    //the below code is to write a pending file in the active directory if any file in active file directory.
                    if(File.Exists(outboxDirFileInfoArr[0].FullName))
                    {
                        outboxDirFileInfoArr[0].MoveTo(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.ActiveDir), outboxDirFileInfoArr[0].Name));
                       StreamWriter st1 = new StreamWriter(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.ActiveDir), outboxDirFileInfoArr[0].Name.Split('.')[0] + "_pending"), false);
                        st1.Flush();
                        st1.Close();
                        st1.Dispose();
                        GetFileFromActiveDir(DirectoryEnum.ActiveDir);
                    }
                   

                }
                catch (Exception ex)
                {
                    logger.Info(ex);

                }

            }
            else if (activeFileCloudVM == null)
            {
                GetFileFromActiveDir(DirectoryEnum.ActiveDir);
            }
            else if (!activeFileCloudVM.isBusy)// == null)
                GetFileFromActiveDir(DirectoryEnum.ActiveDir);

            

        }

        /// <summary>
        /// To update the active cloud view model.
        /// </summary>
        /// <param name="directoryEnum"></param>
        private async void GetFileFromActiveDir(DirectoryEnum directoryEnum)
        {
            

             FileInfo[] activeDirFileInfos = new DirectoryInfo(GlobalMethods.GetDirPath(directoryEnum)).GetFiles();
            bool filesPresent = activeDirFileInfos.Any();
            {
                try
                {
                    if (filesPresent)
                    {
                        logger.Info(JsonConvert.SerializeObject(activeDirFileInfos[0], Formatting.Indented));
                        if (File.Exists(activeDirFileInfos[0].FullName))
                        {

                            if (!activeFileCloudVM.isBusy)
                            {
                                logger.Info("Get file in active vm finf is busy");

                                UpdateActiveCloudVM(activeDirFileInfos[0]);

                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Info(ex);
                    activeFileCloudVM.isBusy = false;

                }

            }

            

        }

        /// <summary>
        /// To write the current view model status and get the next cloud model
        /// </summary>
        /// <param name="fileInfo"></param>
        private void UpdateActiveCloudVM(FileInfo fileInfo)
        {
            if(File.Exists(Path.Combine(fileInfo.Directory.FullName, fileInfo.Name.Split('.')[0] + "_pending")))
            {
                activeFileCloudVM.isBusy = true;
                StartStopSentItemsTimer(false);

                StreamReader st = new StreamReader(fileInfo.FullName);
                var json = st.ReadToEnd();
                st.Close();
                st.Dispose();
                logger.Info("active Dir filename {0}", fileInfo.Name);

                File.Delete(Path.Combine(fileInfo.Directory.FullName, fileInfo.Name.Split('.')[0] + "_pending"));
                CloudModel activeFileCloudModel = JsonConvert.DeserializeObject<CloudModel>(json);
               
                activeFileCloudVM.SetCloudModel(activeFileCloudModel);
                activeFileCloudVM.ActiveFnf = fileInfo;


                activeFileCloudVM.StartAnalsysisFlow();

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
                OutboxFileChecker.Start();
                OutBoxTimerCallback(new object());

            }

            else
                OutboxFileChecker.Stop();

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
