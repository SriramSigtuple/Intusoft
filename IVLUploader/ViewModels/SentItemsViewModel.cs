using BaseViewModel;
using Cloud_Models.Models;
using IntuUploader;
using Newtonsoft.Json;
using NLog;
using System;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows;
using System.Windows.Input;

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

        int retryCount = 0;
        private static SentItemsViewModel _sentItemsViewModel;
        /// <summary>
        /// Constructor
        /// </summary>
        private SentItemsViewModel()
        {
            logger.Info("");

            //SetValue = new RelayCommand(param=> SetValueMethod(param));
            logger.Info("");

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
            FileInfo[] sentItemsDirFileInfoArr = new DirectoryInfo(GlobalMethods.GetDirPath(DirectoryEnum.SentItemsDir)).GetFiles();
            FileInfo[] readDirFileInfoArr = new DirectoryInfo(GlobalMethods.GetDirPath(DirectoryEnum.ReadDir)).GetFiles();

            foreach (var item in sentItemsDirFileInfoArr)
            {
                {

                    GetFileFromActiveDir(item);
                }
            }
            logger.Info("");

        }

        private void GetFileFromActiveDir(FileInfo activeDirFileInfos)
        {
            logger.Info("");

            if (activeFileCloudVM == null || activeFileCloudVM.ActiveFnf == null)
            {
                try
                {
                   if( File.Exists(activeDirFileInfos.FullName))
                    {
                        StreamReader st = new StreamReader(activeDirFileInfos.FullName);
                        var json = st.ReadToEnd();
                        st.Close();
                        st.Dispose();
                        CloudModel activeFileCloudModel = JsonConvert.DeserializeObject<CloudModel>(json);
                        activeFileCloudVM = new CloudViewModel(activeFileCloudModel);
                        activeFileCloudVM.ActiveFnf = activeDirFileInfos;

                        activeFileCloudVM.StartAnalsysisFlow();
                    }
                   
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
                finally
                {
                   activeFileCloudVM.StartAnalsysisFlow();

                }

            }
            logger.Info("");

        }

        public void StartStopSentItemsTimer(bool isStart)
        {
            if(isStart)
            SentItemsStatusCheckTimer = new System.Threading.Timer(SentItemsStatusCheckTimerCallback, null, 0, (int)(GlobalVariables.UploaderSettings.SentItemsTimerInterval * 1000));
            else
             SentItemsStatusCheckTimer = new System.Threading.Timer(SentItemsStatusCheckTimerCallback, null, 0,Timeout.Infinite);

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
