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
        public CloudViewModel activeFileCloudVM;
        int fileIndx = 0;
        int retryCount = 0;
        private static SentItemsViewModel _sentItemsViewModel;
        FileInfo[] sentItemsDirFileInfoArr;
        /// <summary>
        /// Constructor
        /// </summary>
        private SentItemsViewModel()
        {
            
            if (activeFileCloudVM == null)
            {
                activeFileCloudVM = new CloudViewModel();
                activeFileCloudVM.startStopEvent += ActiveFileCloudVM_startStopEvent;

            }
            SentItemsStatusCheckTimer = new Timer((int)(GlobalVariables.UploaderSettings.SentItemsTimerInterval * 1000));
            SentItemsStatusCheckTimer.Elapsed += SentItemsStatusCheckTimer_Elapsed;

            sentItemsDirFileInfoArr = new DirectoryInfo(GlobalMethods.GetDirPath(DirectoryEnum.SentItemsDir)).GetFiles("*.json");
            foreach (var item in sentItemsDirFileInfoArr)
            {
                if(!File.Exists(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.SentItemsDir), item.Name.Split('.')[0] + "_pending")))
                {
                    StreamWriter st1 = new StreamWriter(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.SentItemsDir), item.Name.Split('.')[0] + "_pending"), false);
                    st1.Flush();
                    st1.Close();
                    st1.Dispose();
                }
               
            }

            //SetValue = new RelayCommand(param=> SetValueMethod(param));
            

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
            

            if (_sentItemsViewModel == null)
                _sentItemsViewModel = new SentItemsViewModel();
            
            return _sentItemsViewModel;
        }

        /// <summary>
        /// Method to get Files from SentItems directory
        /// </summary>
        /// <param name="state"></param>
        private void SentItemsStatusCheckTimerCallback(object state)
        {
            
            if (fileIndx == 0)
            {
                sentItemsDirFileInfoArr = new DirectoryInfo(GlobalMethods.GetDirPath(DirectoryEnum.SentItemsDir)).GetFiles("*.json");

            }

            for (int i = fileIndx; i < sentItemsDirFileInfoArr.Length; i++, fileIndx++)
                {
                if (File.Exists(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.SentItemsDir), sentItemsDirFileInfoArr[i].Name.Split('.')[0] + "_pending")))
                {
                    GetFileFromActiveDir(sentItemsDirFileInfoArr[fileIndx]);
                }
                    

                }
                if (fileIndx == sentItemsDirFileInfoArr.Length)
                    fileIndx = 0;
            

        }

        public void CreateMissingPendingFiles()
        {
            FileInfo[] activeDirFileInfoArr = new DirectoryInfo(GlobalMethods.GetDirPath(DirectoryEnum.ActiveDir)).GetFiles("*.json");
            //the below code is to write a pending file if any existing file in active directory has no pending file.
            if (activeDirFileInfoArr.Any())
            {
                if (!File.Exists(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.SentItemsDir), activeDirFileInfoArr[0].Name.Split('.')[0] + "_pending")))
                {
                    StreamWriter st1 = new StreamWriter(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.SentItemsDir), activeDirFileInfoArr[0].Name.Split('.')[0] + "_pending"), false);
                    st1.Flush();
                    st1.Close();
                    st1.Dispose();
                }
            }
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
             
                try
                {
                   if( File.Exists(activeDirFileInfo.FullName))
                    {
                             if (!activeFileCloudVM.isBusy)
                            {
                                //Console.WriteLine("Get sent items file in active vm finf is null");

                                UpdateActiveCloudVM(activeDirFileInfo);

                            }
                    }
                   
                }
                catch (Exception ex)
                {
                    logger.Info(ex);
                    activeFileCloudVM.isBusy = false;
                    CreateMissingPendingFiles();
                }

            

        }

        private  void UpdateActiveCloudVM(FileInfo fileInfo)
        {
            try
            {
                if (File.Exists(Path.Combine(fileInfo.Directory.FullName, fileInfo.Name.Split('.')[0] + "_pending")))
                {

                    StartStopSentItemsTimer(false);
                    activeFileCloudVM.isBusy = true;

                    StreamReader st = new StreamReader(fileInfo.FullName);
                    var json = st.ReadToEnd();
                    st.Close();
                    st.Dispose();
                    //Console.WriteLine("sent items {0}", fileInfo.Name);
                    File.Delete(Path.Combine(fileInfo.Directory.FullName, fileInfo.Name.Split('.')[0] + "_pending"));

                    CloudModel activeFileCloudModel = JsonConvert.DeserializeObject<CloudModel>(json);


                    activeFileCloudVM.SetCloudModel(activeFileCloudModel);

                    activeFileCloudVM.ActiveFnf = fileInfo;
                    activeFileCloudVM.StartAnalsysisFlow();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                activeFileCloudVM.isBusy = false;
                CreateMissingPendingFiles();
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
                SentItemsStatusCheckTimer.Start();
                SentItemsStatusCheckTimerCallback(new object());

            }
               
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
