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
using System.Threading;
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
        CloudViewModel activeFileCloudVM;

        int retryCount = 0;
        private static OutboxViewModel _outboxViewModel;
        /// <summary>
        /// Constructor
        /// </summary>
        private OutboxViewModel()
        {
            logger.Info("");

            //OutboxFileChecker = new Timer();
            //OutboxFileChecker.Interval = timerTick;
            //OutboxFileChecker.Elapsed += OutboxFileChecker_Elapsed;
            //OutboxFileChecker.Start();// = true;
            //OutBoxTimerCallback(new object());
            //SetValue = new RelayCommand(param=> SetValueMethod(param));
            logger.Info("");


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
            logger.Info("");

            if (_outboxViewModel == null)
                _outboxViewModel = new OutboxViewModel();
            logger.Info("");

            return _outboxViewModel;
        }

        /// <summary>
        /// Method to get Files from outbox to active directory
        /// </summary>
        /// <param name="state"></param>
        private void OutBoxTimerCallback(object state)
        {
            logger.Info("");

            FileInfo[] outboxDirFileInfoArr = new DirectoryInfo(GlobalMethods.GetDirPath(DirectoryEnum.OutboxDir)).GetFiles();


            FileInfo[] activeDirFileInfoArr = new DirectoryInfo(GlobalMethods.GetDirPath(DirectoryEnum.ActiveDir)).GetFiles();
            if (!(activeDirFileInfoArr.Any()) && outboxDirFileInfoArr.Any())
            {
                logger.Info(JsonConvert.SerializeObject(activeDirFileInfoArr, Formatting.Indented));

                try
                {
                    if(File.Exists(outboxDirFileInfoArr[0].FullName))
                    {
                        outboxDirFileInfoArr[0].MoveTo(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.ActiveDir), outboxDirFileInfoArr[0].Name));
                        GetFileFromActiveDir(DirectoryEnum.ActiveDir);
                    }
                   

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
              
            }
            else if (activeFileCloudVM == null)
            {
                GetFileFromActiveDir(DirectoryEnum.ActiveDir);
            }
            else if (activeFileCloudVM.ActiveFnf == null)
                GetFileFromActiveDir(DirectoryEnum.ActiveDir);

            logger.Info("");

        }

        private async void GetFileFromActiveDir(DirectoryEnum directoryEnum)
        {
            logger.Info("");

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
                        if (activeFileCloudVM == null)
                        {
                            Console.WriteLine("Get file in active vm is null");

                            UpdateActiveCloudVM(activeDirFileInfos[0]);

                        }
                        else if (activeFileCloudVM.ActiveFnf == null)
                        {
                            Console.WriteLine("Get file in active vm finf is null");

                            UpdateActiveCloudVM(activeDirFileInfos[0]);

                        }
                        else if ((activeFileCloudVM.ActiveFnf.FullName != activeDirFileInfos[0].FullName))
                        {
                            Console.WriteLine("Get file in active vm finf is not equal");

                            UpdateActiveCloudVM(activeDirFileInfos[0]);
                        }
                    }
                }
                }
                catch (Exception ex)
                {
                    logger.Info(ex);
                    

                }
                finally
                {
                    //GetFileFromActiveDir(directoryEnum);

                }

            }

            logger.Info("");

        }

        private async void UpdateActiveCloudVM(FileInfo fileInfo)
        {
            StreamReader st = new StreamReader(fileInfo.FullName);
            var json = await st.ReadToEndAsync();
            st.Close();
            st.Dispose();
            CloudModel activeFileCloudModel = JsonConvert.DeserializeObject<CloudModel>(json);
            activeFileCloudVM = new CloudViewModel(activeFileCloudModel);
            activeFileCloudVM.ActiveFnf = fileInfo;
            activeFileCloudVM.StartAnalsysisFlow();
        }
        public void StartStopSentItemsTimer(bool isStart)
        {
            if (isStart)
                OutboxFileChecker = new System.Threading.Timer(OutBoxTimerCallback, null, 0, (int)(GlobalVariables.UploaderSettings.OutboxTimerInterval * 1000));
            else
                OutboxFileChecker = new System.Threading.Timer(OutBoxTimerCallback, null, 0, Timeout.Infinite);

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
