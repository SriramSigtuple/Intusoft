﻿using BaseViewModel;
using Cloud_Models.Models;
using Newtonsoft.Json;
using NLog;
using System;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Threading;
namespace IntuUploader.ViewModels
{
    /// <summary>
    /// Class which implements the check for internet connection by pinging to 8.8.8.8 of google
    /// </summary>
    public class OutboxViewModel : ViewBaseModel
    {
        static Logger exceptionLog = LogManager.GetLogger("ExceptionLogger");
        private  Logger _logger;// = LogManager.GetLogger("Logger");

        System.Threading.Timer OutboxFileChecker;
        int timeout = 10000;// TODO : to be configured
        int timerTick = 40000;// TODO : to be configured
        public CloudViewModel activeFileCloudVM;

        int retryCount = 0;
        private static OutboxViewModel _outboxViewModel;

        private string activeDirPath = string.Empty;
        private string outboxDirPath = string.Empty;
        /// <summary>
        /// Constructor
        /// </summary>
        public OutboxViewModel(AnalysisType analysisType)
        {
            AnalysisType = analysisType;
            if (AnalysisType == AnalysisType.Fundus)
                _logger = LogManager.GetLogger("FundusLogger");
            else
                _logger = LogManager.GetLogger("QILogger");
            _logger.Info("OutboxVM Constructor, Analysis Type {0}", analysisType.ToString("g"));

            activeDirPath = GlobalMethods.GetDirPath(DirectoryEnum.ActiveDir, AnalysisType);
            outboxDirPath = GlobalMethods.GetDirPath(DirectoryEnum.OutboxDir, AnalysisType);
            if (activeFileCloudVM == null)
            {
                activeFileCloudVM = new CloudViewModel(AnalysisType);
                activeFileCloudVM.startStopEvent += ActiveFileCloudVM_startStopEvent;
                activeFileCloudVM.AnalysisType = AnalysisType;
                activeFileCloudVM.CreatePendingFilesEvent += ActiveFileCloudVM_CreatePendingFilesEvent;
            }
            OutboxFileChecker = new System.Threading.Timer(OutBoxTimerCallback, null, -1, (int)(GlobalVariables.UploaderSettings.OutboxTimerInterval * 1000));// new Timer((int)(GlobalVariables.UploaderSettings.OutboxTimerInterval * 1000));// 
            //OutboxFileChecker.Elapsed += OutboxFileChecker_Elapsed;
            FileInfo[] activeDirFileInfoArr = new DirectoryInfo(activeDirPath).GetFiles("*.json");
            //the below code is to write a pending file if any existing file in active directory has no pending file.
            if (activeDirFileInfoArr.Any())
            {
                if (!File.Exists(Path.Combine(activeDirPath, activeDirFileInfoArr[0].Name.Split('.')[0] + "_pending")))
                {
                    StreamWriter st1 = new StreamWriter(Path.Combine(activeDirPath, activeDirFileInfoArr[0].Name.Split('.')[0] + "_pending"), false);
                    st1.Flush();
                    st1.Close();
                    st1.Dispose();
                }
            }
            
            


        }

        private void ActiveFileCloudVM_CreatePendingFilesEvent(AnalysisType _analysisType)
        {
            if (analysisType == _analysisType)
                CreateMissingPendingFiles();
        }

        private AnalysisType analysisType;

        public AnalysisType AnalysisType
        {
            get { return analysisType; }
            set
            {
                analysisType = value;
                OnPropertyChanged("AnalysisType");
            }
        }

        //private void OutboxFileChecker_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    OutBoxTimerCallback(new object());
        //}

        //private void OutboxFileChecker_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    OutBoxTimerCallback(new object());
        //}

        ///// <summary>
        ///// Implementing singleton pattern in order to handle to values across the module
        ///// </summary>
        ///// <returns></returns>
        //public static OutboxViewModel GetInstance()
        //{
            

        //    if (_outboxViewModel == null)
        //        _outboxViewModel = new OutboxViewModel();
            

        //    return _outboxViewModel;
        //}

        /// <summary>
        /// Method to get Files from outbox to active directory
        /// </summary>
        /// <param name="state"></param>
        private void OutBoxTimerCallback(object state)
        {
            _logger.Info("OutBox VM Timer, Analysis Type {0}", analysisType.ToString("g"));


            FileInfo[] outboxDirFileInfoArr = new DirectoryInfo(outboxDirPath).GetFiles("*.json");


            FileInfo[] activeDirFileInfoArr = new DirectoryInfo(activeDirPath).GetFiles("*.json");
            if (!activeFileCloudVM.IsBusy)
            {
                if (!(activeDirFileInfoArr.Any()) && outboxDirFileInfoArr.Any() && !activeFileCloudVM.IsBusy)
                {
                    _logger.Info(JsonConvert.SerializeObject(activeDirFileInfoArr, Formatting.Indented));
                    StartStopSentItemsTimer(false);

                    try
                    {
                        //the below code is to write a pending file in the active directory if any file in active file directory.
                        if (File.Exists(outboxDirFileInfoArr[0].FullName))
                        {
                            outboxDirFileInfoArr[0].MoveTo(Path.Combine(activeDirPath, outboxDirFileInfoArr[0].Name));
                            StreamWriter st1 = new StreamWriter(Path.Combine(activeDirPath, outboxDirFileInfoArr[0].Name.Split('.')[0] + "_pending"), false);
                            st1.Flush();
                            st1.Close();
                            st1.Dispose();
                            GetFileFromActiveDir(DirectoryEnum.ActiveDir);
                        }
                        else
                            StartStopSentItemsTimer(true);



                    }
                    catch (Exception ex)
                    {
                        _logger.Info(ex);
                        exceptionLog.Error(Common.Exception2StringConverter.GetInstance().ConvertException2String(ex));
                        StartStopSentItemsTimer(true);

                    }

                }
                else if (activeFileCloudVM == null)
                {
                    GetFileFromActiveDir(DirectoryEnum.ActiveDir);
                }
                else if (!activeFileCloudVM.IsBusy)// == null)
                    GetFileFromActiveDir(DirectoryEnum.ActiveDir);

            }

        }

        public void CreateMissingPendingFiles()
        {
            FileInfo[] activeDirFileInfoArr = new DirectoryInfo(activeDirPath).GetFiles("*.json");
            //the below code is to write a pending file if any existing file in active directory has no pending file.
            if (activeDirFileInfoArr.Any())
            {
                if (!File.Exists(Path.Combine(activeDirPath, activeDirFileInfoArr[0].Name.Split('.')[0] + "_pending")))
                {
                    StreamWriter st1 = new StreamWriter(Path.Combine(activeDirPath, activeDirFileInfoArr[0].Name.Split('.')[0] + "_pending"), false);
                    st1.Flush();
                    st1.Close();
                    st1.Dispose();
                }
            }

        }

        /// <summary>
        /// To update the active cloud view model.
        /// </summary>
        /// <param name="directoryEnum"></param>
        private void GetFileFromActiveDir(DirectoryEnum directoryEnum)
        {
            try
            {

                FileInfo[] activeDirFileInfos = new DirectoryInfo(GlobalMethods.GetDirPath(directoryEnum, AnalysisType)).GetFiles("*.json");
                bool filesPresent = activeDirFileInfos.Any();
                {

                    if (filesPresent)
                    {
                        _logger.Info(JsonConvert.SerializeObject(activeDirFileInfos[0], Formatting.Indented));
                        if (File.Exists(activeDirFileInfos[0].FullName))
                        {

                            if (!activeFileCloudVM.IsBusy)
                            {
                                _logger.Info("Get file in active vm finf is busy");
                                UpdateActiveCloudVM(activeDirFileInfos[0]);

                            }
                            else
                                StartStopSentItemsTimer(true);


                        }
                        else
                            StartStopSentItemsTimer(true);

                    }
                  else
                        StartStopSentItemsTimer(true);

                }


            }

            catch (Exception ex)
            {
                _logger.Info(ex);
                exceptionLog.Error(Common.Exception2StringConverter.GetInstance().ConvertException2String(ex));

                activeFileCloudVM.IsBusy = false;
            }

        }

        /// <summary>
        /// To write the current view model status and get the next cloud model
        /// </summary>
        /// <param name="fileInfo"></param>
        private void UpdateActiveCloudVM(FileInfo fileInfo)
        {
            try
            {
                if (File.Exists(Path.Combine(fileInfo.Directory.FullName, fileInfo.Name.Split('.')[0] + "_pending")))
                {
                    activeFileCloudVM.IsBusy = true;

                    StreamReader sr = new StreamReader(fileInfo.FullName);
                    var json = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                    _logger.Info("active Dir filename {0}", fileInfo.Name);

                    File.Delete(Path.Combine(fileInfo.Directory.FullName, fileInfo.Name.Split('.')[0] + "_pending"));
                    CloudModel activeFileCloudModel = JsonConvert.DeserializeObject<CloudModel>(json);

                    activeFileCloudVM.SetCloudModel(activeFileCloudModel);
                    activeFileCloudVM.ActiveFnf = fileInfo;


                    activeFileCloudVM.StartAnalsysisFlow();
                    var filePath = string.Empty;

                    if (activeFileCloudVM.IsMove2NextDir)
                    {
                        filePath = Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.SentItemsDir, AnalysisType), activeFileCloudVM.ActiveFnf.Name);
                    }
                    else
                        filePath = activeFileCloudVM.ActiveFnf.FullName;

                    using (StreamWriter st = new StreamWriter(filePath))
                    {
                        st.Write( JsonConvert.SerializeObject(activeFileCloudVM.ActiveCloudModel, Formatting.Indented));
                        st.Flush();
                        st.Close();
                        st.Dispose();
                        File.Delete(activeFileCloudVM.ActiveFnf.FullName);

                    }
                    activeFileCloudVM.IsBusy = false;
                    StartStopSentItemsTimer(true);

                }
                else
                    StartStopSentItemsTimer(true);

            }
            catch (Exception ex)
            {

                _logger.Error(ex);
                exceptionLog.Error(Common.Exception2StringConverter.GetInstance().ConvertException2String(ex));
                StartStopSentItemsTimer(true);

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
                OutboxFileChecker.Change(0, (int)(GlobalVariables.UploaderSettings.OutboxTimerInterval * 1000));
                //OutboxFileChecker = new System.Threading.Timer(OutBoxTimerCallback, null, 0, (int)(GlobalVariables.UploaderSettings.OutboxTimerInterval * 1000));// new Timer((int)(GlobalVariables.UploaderSettings.OutboxTimerInterval * 1000));// 

                //OutboxFileChecker.Start();
                //OutBoxTimerCallback(new object());

            }

            else
            {
                OutboxFileChecker.Change(-1, -1);// (int)(GlobalVariables.UploaderSettings.OutboxTimerInterval * 1000));

                //OutboxFileChecker = new System.Threading.Timer(OutBoxTimerCallback, null, 0, );// (int)(GlobalVariables.UploaderSettings.OutboxTimerInterval * 1000));// new Timer((int)(GlobalVariables.UploaderSettings.OutboxTimerInterval * 1000));// 

                //OutboxFileChecker.Stop();

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
