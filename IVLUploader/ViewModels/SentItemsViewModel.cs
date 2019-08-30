using BaseViewModel;
using Cloud_Models.Models;
using IntuUploader;
using Newtonsoft.Json;
using NLog;
using System;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
//using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Timers;

namespace IVLUploader.ViewModels
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
        CloudViewModel activeFileCloudVM;
        int fileIndx = 0;
        int retryCount = 0;
        private static SentItemsViewModel _sentItemsViewModel;
        FileInfo[] sentItemsDirFileInfoArr;
        /// <summary>
        /// Constructor
        /// </summary>
        private SentItemsViewModel()
        {
            logger.Info("");
            if (activeFileCloudVM == null)
            {
                activeFileCloudVM = new CloudViewModel();
                activeFileCloudVM.startStopEvent += ActiveFileCloudVM_startStopEvent;

            }
            SentItemsStatusCheckTimer = new Timer((int)(GlobalVariables.UploaderSettings.SentItemsTimerInterval * 1000));
            SentItemsStatusCheckTimer.Elapsed += SentItemsStatusCheckTimer_Elapsed;
            //SetValue = new RelayCommand(param=> SetValueMethod(param));
            logger.Info("");

        }

        private void SentItemsStatusCheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SentItemsStatusCheckTimerCallback(new object());
        }

        /// <summary>
        /// Implementing singleton pattern in order to handle to values across the module
        /// </summary>
        /// <returns></returns>
        public static SentItemsViewModel GetInstance()
        {
            logger.Info("");

            if (_sentItemsViewModel == null)
                _sentItemsViewModel = new SentItemsViewModel();
            logger.Info("");
            return _sentItemsViewModel;
        }

        /// <summary>
        /// Method to get Files from SentItems directory
        /// </summary>
        /// <param name="state"></param>
        private void SentItemsStatusCheckTimerCallback(object state)
        {
            logger.Info("");
            if (fileIndx == 0)
            {
                sentItemsDirFileInfoArr = new DirectoryInfo(GlobalMethods.GetDirPath(DirectoryEnum.SentItemsDir)).GetFiles();

            }
            for (fileIndx = 0; fileIndx < sentItemsDirFileInfoArr.Length; fileIndx++)
            {
                GetFileFromActiveDir(sentItemsDirFileInfoArr[fileIndx]);

            }
            if (fileIndx == sentItemsDirFileInfoArr.Length)
                fileIndx = 0;
            //RecursiveMethod();
            logger.Info("");

        }

        private void RecursiveMethod()
        {
            if (fileIndx < sentItemsDirFileInfoArr.Length)
            {
                
                if(!activeFileCloudVM.isBusy)
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
            logger.Info("");

            {
                try
                {
                   if( File.Exists(activeDirFileInfo.FullName))
                    {
                        if (File.Exists(activeDirFileInfo.FullName))
                        {
                            if (activeFileCloudVM == null)
                            {
                                //Console.WriteLine("Get sent items file in active vm is null");

                                UpdateActiveCloudVM(activeDirFileInfo);


                            }
                            else if (!activeFileCloudVM.isBusy)
                            {
                                //Console.WriteLine("Get sent items file in active vm finf is null");

                                UpdateActiveCloudVM(activeDirFileInfo);

                            }
                            //else if ((activeFileCloudVM.ActiveFnf.FullName != activeDirFileInfo.FullName))
                            //{
                            //    Console.WriteLine("Get sent items file in active vm finf is not equal");

                            //    UpdateActiveCloudVM(activeDirFileInfo);
                            //}
                        }
                    }
                   
                }
                catch (Exception ex)
                {
                    logger.Info(ex);
                    activeFileCloudVM.isBusy = false;
                    
                }
                finally
                {
                    //GetFileFromActiveDir(activeDirFileInfo);

                }

            }
            logger.Info("");

        }

        private  void UpdateActiveCloudVM(FileInfo fileInfo)
        {
            StartStopSentItemsTimer(false);
            StreamReader st = new StreamReader(fileInfo.FullName);
            var json =  st.ReadToEnd();
            st.Close();
            st.Dispose();
            //Console.WriteLine("sent items {0}", fileInfo.Name);

            CloudModel activeFileCloudModel = JsonConvert.DeserializeObject<CloudModel>(json);
            

            activeFileCloudVM.SetCloudModel(activeFileCloudModel);

            activeFileCloudVM.ActiveFnf = fileInfo;
            activeFileCloudVM.isBusy = true;
            activeFileCloudVM.StartAnalsysisFlow();
        }
        private void ActiveFileCloudVM_startStopEvent(bool isStart)
        {
            StartStopSentItemsTimer(isStart);
        }
        public void StartStopSentItemsTimer(bool isStart)
        {
            if (isStart)
                SentItemsStatusCheckTimer.Start();
            //SentItemsStatusCheckTimer = new System.Threading.Timer(SentItemsStatusCheckTimerCallback, null, 0, (int)(GlobalVariables.UploaderSettings.SentItemsTimerInterval * 1000));
            else
            {
                SentItemsStatusCheckTimer.Stop();
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
