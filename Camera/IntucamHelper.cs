using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using INTUSOFT.EventHandler;
using Common;
using NLog;
using NLog.Config;
using NLog.Targets;
using System.Xml.Serialization;
using System.Threading;
using System.ComponentModel;
using INTUSOFT.Configuration;
using System.Windows.Forms;

namespace INTUSOFT.Imaging
{
    public class IntucamHelper
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(Camera));
        //private static readonly Logger log = LogManager.GetLogger("INTUSOFT.Imaging");
        //private static readonly Logger captureLog = LogManager.GetLogger("INTUSOFT.Imaging.CaptureLog");
        private static readonly Logger CaptureSettingsLog = LogManager.GetLogger("INTUSOFT.Imaging.CaptureSettingsLog");
        private static readonly Logger Exception_Log = LogManager.GetLogger("ExceptionLog");

        private Thread cameraWindowThread;
        
        int maxExposureIndex = 43;
        string ConfigFileName = "IVLConfig.xml";
        bool isCameraSettingsSet = false;
        //private static readonly ILog captureLog = LogManager.GetLogger("Capture");

       public string FrameRateLabelText;
       public string CameraConnectionLabelText;
       public string CameraDisconnectionLabelText;
       public string LiveExposureLabelText;
       public string LiveGainLabelText;
       public string CaptureExposureLabelText;
       public string CaptureGainLabelText;
       public string ComportOpenLabelText;
       public string ComportCloseLabelText;
       public string PowerOnLabelText;
       public string PowerOffLabelText;

       public string MRNValue = string.Empty;
       public string VisitDate = string.Empty;

       internal System.Timers.Timer standByTimer;
       private bool isLogWritingCompleted = false;
       int StandByTimeOut = 3;
        const int oneMinInMilliseconds = 60000;
       public bool IsLogWritingCompleted
       {
           get 
           { 
               return IVLCamVariables.logClass.isLogWritingCompleted; 
           }
           set 
           { 
               isLogWritingCompleted = value; 
           }
       }


       public bool AppLaunched
       {
           get { return IVLCamVariables.isAppLaunched; }
           set { IVLCamVariables.isAppLaunched = value; }
       }
       private bool isAssemblySoftware;

       public bool IsAssemblySoftware
       {
           get { return isAssemblySoftware; }
           set { isAssemblySoftware = value; }
       }
        public static List<Args> capture_log;
        
        public delegate void EnableControls(bool isEnabled);
        public event EnableControls _EnableControls;


        System.Timers.Timer captureTimer;
        double captureTimeout = 15000;

        public delegate void CompleteCaptureDelegate();
        public event CompleteCaptureDelegate _CompleteCaptureDelegate;
        public System.Windows.Forms.ToolStripStatusLabel FrameRate_lbl;
        public System.Windows.Forms.ToolStripStatusLabel comPort_lbl;
        public System.Windows.Forms.ToolStripStatusLabel ExposureStatus_lbl;
        public System.Windows.Forms.ToolStripStatusLabel gainStatus_lbl;
        public System.Windows.Forms.ToolStripStatusLabel PowerStatus_lbl;
        public System.Windows.Forms.ToolStripStatusLabel CameraStatus_lbl;
        public System.Windows.Forms.ToolStripStatusLabel tintStatus_lbl;
        private bool processedImagePathFromConfig = true;

        public bool ProcessedImagePathFromConfig
        {
            get { return processedImagePathFromConfig; }
            set { processedImagePathFromConfig = value; }
        }
        public PictureBox Pbx
        {
            set { ivl_Camera.pbx = value; }
        }
        public PictureBox MaskOverlayPbx
        {
            set { ivl_Camera.maskOverlayPbx = value; }
        }

        private PictureBox rightArrowPbx;

        public PictureBox RightArrowPbx
        {
            get { return rightArrowPbx; }
            set { rightArrowPbx = value; }
        }

        private PictureBox leftArrowPbx;

        public PictureBox LeftArrowPbx
        {
            get { return leftArrowPbx; }
            set { leftArrowPbx = value; }
        }

        private PictureBox leftDiaptorPbx;

        public PictureBox LeftDiaptorPbx
        {
            get { return leftDiaptorPbx; }
            set { leftDiaptorPbx = value; }
        }
        private PictureBox rightDiaptorPbx;

        public PictureBox RightDiaptorPbx
        {
            get { return rightDiaptorPbx; }
            set { rightDiaptorPbx = value; }
        }
        #region Variables and Constants
        static IntucamHelper camHelper;
        BackgroundWorker bg;
       internal Camera ivl_Camera;
        System.Timers.Timer motorMotionTimer = new System.Timers.Timer();// Timer to maintain motor event timeout

        bool isDirCreated = false;
        public bool isValueChanged = false;// To maintain return value indicationg success/failure of each function.

        public uint frameCnt = 0;
        private EventArgs e = null;

        public delegate void UpdateStatusBar(string a, EventArgs e);

        public delegate void UpdateAnalogvalDigitalval(int val);

        public static event UpdateAnalogvalDigitalval _updateAnalogvalDigitalVal;

        public event UpdateStatusBar statusBarUpdate;

        public CameraPropertiesHelper camPropsHelper;

        //private System.Timers.Timer CameraPowerStatusTimer;
        private System.Timers.Timer FrameRateTimer;
        public bool isCameraOpen = false;
        CameraWindow cameraWindow;



        private XmlSerializer serializer1, serializer2, serializer3,serializer4;

        /// <summary>
        /// State variable used to maintain the state when the mode is being changed
        /// </summary>
        public bool isResetMode;

        public bool IsMotorMovementDone
        {
            get { return     IVLCamVariables.IsMotorMoved; }
            set { IVLCamVariables.IsMotorMoved = value; }
        }

        #region Board Configurable variables

        public IntPtr CameraHandle = IntPtr.Zero;
        //public System.Windows.Forms.PictureBox picBox = null;

        #endregion




        int prevCnt, FrameCnt = 0;
        ImageSaveHelper imageSaveHelper;// object of imagesavehelper to save captured images
        #endregion

        private bool isCapturing = false;

        public bool IsCapturing
        {
            get
            {

                return isCapturing = IVLCamVariables.isCapturing;
            }
        }
        private bool isChangeMode = false;

        public bool IsChangeMode
        {
            get { return isChangeMode; }
            set { isChangeMode = value; }
        }
        private bool isResuming = false;

        public bool IsResuming
        {
            get { return isResuming = IVLCamVariables.isResuming; }
        }
        public bool CameraIsLive
        {
            get { return IVLCamVariables.isLive; }
        }

        public bool IsCaptureFailure
        {
            get { return IVLCamVariables.isCaptureFailure; }
        }
        public Led LEDSource
        {
            get { return IVLCamVariables.ledSource; }
            set
            {
                IVLCamVariables.ledSource = value;
                switch (IVLCamVariables.ledSource)
                {
                    case Led.IR:
                        {
                            camPropsHelper.IRLightOnOff(true);
                            IVLCamVariables.liveLedCode = LedCode.IR;

                            if (IVLCamVariables.isBlueFilteredMoved)
                            {
                                IVLCamVariables.BoardHelper.GreenFilterMove(IVLCamVariables._Settings.BoardSettings.GreenFilterPos);
                                IVLCamVariables.isBlueFilteredMoved = false;
                            }

                            break;
                        }
                    case Led.Flash:
                        {
                            camPropsHelper.WhiteLightOnOff(true);
                            IVLCamVariables.liveLedCode = LedCode.Flash;
                            if (IVLCamVariables.isBlueFilteredMoved)
                            {
                                IVLCamVariables.BoardHelper.GreenFilterMove(IVLCamVariables._Settings.BoardSettings.GreenFilterPos);
                                IVLCamVariables.isBlueFilteredMoved = false;
                            }
                            break;
                        }
                    case Led.Blue:
                        {
                            camPropsHelper.BlueLightOnOff(true);
                            IVLCamVariables.liveLedCode = LedCode.Blue;
                            if (!IVLCamVariables.isBlueFilteredMoved)
                            {
                                camPropsHelper.ServoMove(camPropsHelper._Settings.BoardSettings.BlueFilterPos);
                                IVLCamVariables.isBlueFilteredMoved = true;
                            }
                            break;
                        }

                }
            }
        }

        private IntucamHelper()
        {
            captureTimer = new System.Timers.Timer(captureTimeout);
            captureTimer.Elapsed += captureTimer_Elapsed;

            standByTimer = new System.Timers.Timer(StandByTimeOut * oneMinInMilliseconds);
            standByTimer.Elapsed += standByTimer_Elapsed;
            this.FrameRate_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.comPort_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.ExposureStatus_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.gainStatus_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.PowerStatus_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.CameraStatus_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.tintStatus_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            //this.totalFrameCount_Lbl = new System.Windows.Forms.ToolStripStatusLabel();

            _CompleteCaptureDelegate = new CompleteCaptureDelegate(CompleteCaptureSequence);
            IVLCamVariables.logClass = LogClass.GetInstance();
             //IVLCamVariables.logClass.CaptureLogList = new List<string>();
            IVLCamVariables._Settings = CameraModuleSettings.GetInstance();
            capture_log = new List<Args>();
            IVLCamVariables.CameraLogList = new List<Args>();
            imageSaveHelper = ImageSaveHelper.GetInstance();
            IVLCamVariables._eventHandler = IVLEventHandler.getInstance();
            IVLCamVariables.exceptionConvertor = Exception2StringConverter.GetInstance();
            ivl_Camera = Camera.createCamera();
            camPropsHelper = CameraPropertiesHelper.GetInstance();

            IVLCamVariables.BoardHelper = IntucamBoardCommHelper.GetInstance();
            IVLCamVariables.isCameraConnected = Devices.OtherDevices;
            IVLCamVariables.isPowerConnected = Devices.OtherDevices;
            //IVLConfig.fileName = ConfigFileName;
            ConfigVariables.GetCurrentSettings();

            //IVLCamVariables._eventHandler.Register(IVLCamVariables._eventHandler.triggerRecieved, new NotificationHandler(TriggerRecieved));
            #region Board events
            IVLCamVariables.BoardHelper.triggerRecieved += IntucamBoardCommHelper_triggerRecieved;
            #endregion

            motorMotionTimer.Elapsed += motorMotionTimer_Elapsed;
            //CameraPowerStatusTimer = new System.Timers.Timer();
            //CameraPowerStatusTimer.Interval = 2000;
            //CameraPowerStatusTimer.Elapsed += CameraPowerStatusTimer_Elapsed;

            FrameRateTimer = new System.Timers.Timer();
            FrameRateTimer.Elapsed += FrameRateTimer_Elapsed;
            FrameRateTimer.Interval = 3000;


            camPropsHelper.FFATimer = new System.Timers.Timer();
            camPropsHelper.FFATimer.Elapsed += FFATimer_Elapsed;
            camPropsHelper.FFATimer.Interval = 1000;

            IVLCamVariables._Settings.BoardSettings.MotorSteps = 25;// Setting the default value of motor steps to 25 which is mean't for 45

            serializer4 = new XmlSerializer(IVLCamVariables._Settings.MotorOffSetSettings.GetType());
            serializer3 = new XmlSerializer(IVLCamVariables._Settings.BoardSettings.GetType());
            serializer2 = new XmlSerializer(IVLCamVariables._Settings.PostProcessingSettings.GetType());
            serializer1 = new XmlSerializer(IVLCamVariables._Settings.CameraSettings.GetType());



                cameraWindow = new CameraWindow();
            this.CameraHandle = cameraWindow.Handle;
            IsMotorMovementDone = true;
        }

        void standByTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            bool wasCameraLive = CameraIsLive;
            DisconnectCamera();// if stand by time is reached disconnect the camera
            if (wasCameraLive)
            {
                IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.GoToViewScreen, new Args());
            }
        }

        void captureTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            captureTimer.Stop();
            captureTimer.Enabled = false;
            IVLCamVariables.isCaptureFailure = true;
            IVLCamVariables.captureFailureCode = CaptureFailureCode.CaptureTimeout;
            IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.SaveFrames2Disk, new Args());
        }

        void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }
        void captureBgThreadWork(object callBack)
        {
            try
            {


                if (!isResetMode && IsMotorMovementDone)
                {
                    IVLCamVariables.isCapturing = true;// State to indicate capturing sequence has started
                    if (!isDirCreated)
                        camPropsHelper.imageSaveHelper.CreateImageCaptureDirectory();// Create capture directory for debugging of the capture sequence


                    captureTimer.Interval = captureTimeout;
                    captureTimer.Start();
                    captureTimer.Enabled = true;
                    Args logArg = new Args();
                    if (!IsAssemblySoftware)
                    {
                        if (camPropsHelper.ImagingMode == ImagingMode.Posterior_45 || camPropsHelper.ImagingMode == ImagingMode.FFA_Plus || camPropsHelper.ImagingMode == ImagingMode.FFAColor)
                        {
                            // IVLVariables.ivl_Camera.UpdateExposureGainFromTable(Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._exposureIndex.val));
                            IVLCamVariables._Settings.CameraSettings.CaptureGainIndex = Convert.ToInt32(ConfigVariables.CurrentSettings.CameraSettings._DigitalGain.val);
                            if (IVLCamVariables._Settings.CameraSettings.LiveGainIndex != IVLCamVariables._Settings.CameraSettings.CaptureGainIndex)
                            {
                                ivl_Camera.UpdateExposureGainFromTable(IVLCamVariables._Settings.CameraSettings.CaptureGainIndex);
                            }
                            int[] expGainArr = camPropsHelper.GetExposureGainFromTable(IVLCamVariables._Settings.CameraSettings.CaptureGainIndex);
                            IVLCamVariables._Settings.CameraSettings.CaptureGain = (ushort)expGainArr[1];
                            if(IVLCamVariables._Settings.CameraSettings.CameraModel != CameraModel.D)
                            IVLCamVariables._Settings.CameraSettings.CaptureExposure = (uint)expGainArr[0];

                            logArg["TimeStamp"] = DateTime.Now;
                            logArg["Msg"] = string.Format("Capture Gain  = {0}  ", IVLCamVariables._Settings.CameraSettings.CaptureGain);
                            capture_log.Add(logArg);

                            logArg["TimeStamp"] = DateTime.Now;
                            logArg["Msg"] = string.Format("Capture Exposure  = {0}  ", IVLCamVariables._Settings.CameraSettings.CaptureExposure);
                            capture_log.Add(logArg);

                        }
                        else if (camPropsHelper.ImagingMode == ImagingMode.Anterior_Prime || camPropsHelper.ImagingMode == ImagingMode.Posterior_Prime)
                        {

                            //if (IVLCamVariables.StrobeValue != 100)
                            //    IVLCamVariables.StrobeValue = 100;
                            camPropsHelper.SetGainValueFromLevel(camPropsHelper.CaptureGainLevel, false);// by sriram to set the capture gain from the config
                            bool retval = ivl_Camera.SetGain((ushort)IVLCamVariables._Settings.CameraSettings.CaptureGain);

                            logArg["TimeStamp"] = DateTime.Now;
                            logArg["Msg"] = string.Format("Capture Gain  = {0}  ", IVLCamVariables._Settings.CameraSettings.CaptureGain);
                        }
                    }
                    #region Capture Exposure and gain settings if it from assembly software UI
                    else
                    {
                        byte[] arr = { camPropsHelper._Settings.BoardSettings.CaptureStartOffset, camPropsHelper._Settings.BoardSettings.CaptureEndOffset };
                        IVLCamVariables.BoardHelper.FlashOffset(arr);
                        bool retVal = ivl_Camera.SetExposure(camPropsHelper._Settings.CameraSettings.CaptureExposure);
                        if (retVal)
                        {
                            logArg["TimeStamp"] = DateTime.Now;
                            logArg["Msg"] = string.Format("Capture Gain  = {0}  ", IVLCamVariables._Settings.CameraSettings.CaptureGain);
                            capture_log.Add(logArg);
                        }
                        retVal = ivl_Camera.SetGain(IVLCamVariables._Settings.CameraSettings.CaptureGain);

                        if (retVal)
                        {
                            logArg["TimeStamp"] = DateTime.Now;
                            logArg["Msg"] = string.Format("Capture Exposure  = {0}  ", IVLCamVariables._Settings.CameraSettings.CaptureExposure);
                            capture_log.Add(logArg);
                        }
                        double capExposureTempVar = Convert.ToDouble(IVLCamVariables._Settings.CameraSettings.CaptureExposure);
                        capExposureTempVar = capExposureTempVar / 1000;

                        byte strobeValue = Convert.ToByte(capExposureTempVar);
                        if (IVLCamVariables.ImagingMode == Imaging.ImagingMode.Anterior_Prime || IVLCamVariables.ImagingMode == Imaging.ImagingMode.Posterior_Prime)
                            strobeValue = 100;
                        //else if (IVLCamVariables._Settings.CameraSettings.CameraModel == CameraModel.D)
                        //    strobeValue = 50;

                        IVLCamVariables.BoardHelper.SetStrobeWidth(strobeValue);
                    }
                    #endregion
                    //logArg["callstack"] = Environment.StackTrace;
                    capture_log.Add(logArg);

                    //  ThreadPool.QueueUserWorkItem(new WaitCallback(f =>
                    //{
                    ResetCaptureStates();



                    ThreadPool.QueueUserWorkItem(new WaitCallback(f =>
                    {

                        if (IVLCamVariables.DisplayName.Contains("UHCCD"))
                        {
                            if (IVLCamVariables._Settings.CameraSettings.LiveExposure != IVLCamVariables._Settings.CameraSettings.CaptureExposure)
                            {
                                Thread.Sleep(200);
                            }
                        }
                        IVLCamVariables.BoardHelper.isFlushBulk = true;
                        while (IVLCamVariables.BoardHelper.isFlushBulk)
                        {
                            ;
                        }
                        Args arg = new Args();
                        arg["TimeStamp"] = DateTime.Now;
                        arg["Msg"] = string.Format("Flush Buffer and Bulk cleared");
                        //arg["callstack"] = Environment.StackTrace;
                        capture_log.Add(arg);
                        IVLCamVariables.IsFlashOnDone = false;


                        IVLCamVariables.FlashOffTime = DateTime.Now;
                        IVLCamVariables.FlashOnTime = DateTime.Now;
                        //IVLCamVariables.FrameBetweenFlashOnFlashOffDoneCnt = 0;
                        IVLCamVariables.CaptureImageIndx = -1;

                        IVLCamVariables._Settings.BoardSettings.MotorSteps = IVLCamVariables.motorOffSetMatrix[(int)IVLCamVariables.liveLedCode, (int)IVLCamVariables.captureLedCode];
                        if (IVLCamVariables.ImagingMode == Imaging.ImagingMode.FFA_Plus)
                        {
                            if (IVLCamVariables.liveLedCode == LedCode.IR || IVLCamVariables.liveLedCode == LedCode.Flash)
                            {
                                //camPropsHelper.ServoMove(IVLCamVariables._Settings.BoardSettings.BlueFilterPos);
                                Args logArg1 = new Args();
                                logArg1["TimeStamp"] = DateTime.Now;
                                IVLCamVariables.isBlueFilteredMoved = true;
                                logArg1["Msg"] = string.Format("Blue Filter Move For Capture = {0} ", IVLCamVariables.isBlueFilteredMoved);
                                //logArg1["callstack"] = Environment.StackTrace;
                                capture_log.Add(logArg1);

                                IVLCamVariables.BoardHelper.GreenFilterMove(IVLCamVariables._Settings.BoardSettings.BlueFilterPos);
                            }
                        }
                        else if (IVLCamVariables.ImagingMode == ImagingMode.Anterior_Prime || IVLCamVariables.ImagingMode == ImagingMode.Posterior_Prime)
                        {
                            IVLCamVariables.BoardHelper.IRLightOff();
                        }
                        IVLCamVariables.BoardHelper.SetRealTime();// to set real time in the microcontroller to get the timing info the microcontroller
                        motorMotionTimer.Interval = (IVLCamVariables._Settings.BoardSettings.MotorSteps * IVLCamVariables._Settings.BoardSettings.MotorPerStepTime + IVLCamVariables._Settings.BoardSettings.MotorStepOffsetTime);// time interval for motor timer time out in milliseconds always set at the time of capture
                        logArg["TimeStamp"] = DateTime.Now;
                        logArg["Msg"] = string.Format(" Motor Steps = {0}  ", motorMotionTimer.Interval);
                        //logArg["callstack"] = Environment.StackTrace;
                        capture_log.Add(logArg);
                        StartMovementCallBack(new object());
                    }));
                }
            }
            catch (Exception ex)
            {

                CameraLogger.WriteException(ex, Exception_Log);
            }
        }



        private void StartMovementCallBack(object callBack)
        {
            if (IVLCamVariables._Settings.BoardSettings.MotorSteps != 0)
                StartMotorMovement(IVLCamVariables._Settings.BoardSettings.MotorPolarityIsForward, IVLCamVariables._Settings.BoardSettings.MotorSteps);// Start motor movement to compensate focus levels from IR to Flash in case of 45 , posterior in prime and anterior in prime
            else
                MotorMovementDone();
        }

        int prevSensorPos = 0;
        bool isRotaryMovementGraphicsDrawing = false;
        Args arg = new Args();
        void UpdateDiaptorImageForRotary(int SensorPosition)
        {
            try
            {
                if (this.CameraIsLive)
                {
                    //if (!isRotaryMovementGraphicsDrawing)
                    //    isRotaryMovementGraphicsDrawing = true;

                    if (prevSensorPos != SensorPosition)
                    {
                        float widthVal = 0f;
                        Graphics g2 = Graphics.FromImage(camPropsHelper.RightBitmap);
                        g2.Clear(Color.Transparent);// .FillRectangle(SystemBrushes.Control, new Rectangle(0, 0, RBwidth, RBheight));
                        g2.Dispose();

                        g2 = Graphics.FromImage(camPropsHelper.LeftBitmap);
                        g2.Clear(Color.Transparent);
                        g2.Dispose();

                        #region code to display change of arrow direction when rotary is moved
                        if (prevSensorPos < SensorPosition)
                        {

                            RightArrowPbx.Image = camPropsHelper.positivearrowSymbol.Clone() as Bitmap;
                            LeftArrowPbx.Image = camPropsHelper.resetBitmapRight.Clone() as Bitmap;
                            

                            //arg["LeftArrowBitmap"] = camPropsHelper.resetBitmapRight.Clone();
                            ////arg["RightArrowBitmap"] = negativearrowSymbol.Clone();
                            //arg["RightArrowBitmap"] = camPropsHelper.positivearrowSymbol.Clone();
                        }
                        else
                        {
                            RightArrowPbx.Image = camPropsHelper.resetBitmapRight.Clone() as Bitmap;
                            LeftArrowPbx.Image = camPropsHelper.negativearrowSymbol.Clone() as Bitmap;
                            //arg["RightArrowBitmap"] = camPropsHelper.resetBitmapRight.Clone();
                            ////arg["LeftArrowBitmap"] = positivearrowSymbol.Clone();
                            //arg["LeftArrowBitmap"] = camPropsHelper.negativearrowSymbol.Clone();


                        }
                        #endregion

                        //Positive direction focus movement 
                        if (SensorPosition > 0)
                        {
                            widthVal = ((float)SensorPosition * (float)camPropsHelper.RightBitmap.Width) / IVLCamVariables.MaxPositiveDiaptor;
                            Graphics g = Graphics.FromImage(camPropsHelper.RightBitmap);
                            SolidBrush brush = new SolidBrush(Color.FromName(camPropsHelper.RotaryPositiveColor));
                            g.FillRectangle(brush, new Rectangle(0, 0, (int)widthVal, camPropsHelper.RightBitmap.Height));
                            g.FillRectangle(Brushes.Transparent, new Rectangle((int)widthVal, 0, camPropsHelper.RightBitmap.Width, camPropsHelper.RightBitmap.Height));
                            g.Dispose();

                        }
                        else
                        {
                            widthVal = (Math.Abs(SensorPosition) * (float)camPropsHelper.LeftBitmap.Width) / IVLCamVariables.MaxNegativeDiaptor;
                            float xVal = camPropsHelper.LeftBitmap.Width - widthVal;
                            Graphics g = Graphics.FromImage(camPropsHelper.LeftBitmap);
                            SolidBrush brush = new SolidBrush(Color.FromName(camPropsHelper.RotaryNegativeColor));
                            g.FillRectangle(brush, new Rectangle((int)xVal, 0, (int)widthVal, camPropsHelper.LeftBitmap.Height));
                            g.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, (int)xVal, camPropsHelper.LeftBitmap.Height));
                            g.Dispose();

                        }
                       
                        LeftDiaptorPbx.Image = camPropsHelper.LeftBitmap.Clone() as Bitmap;
                        RightDiaptorPbx.Image = camPropsHelper.RightBitmap.Clone() as Bitmap;
                        prevSensorPos = SensorPosition;
                    }
                    //isRotaryMovementGraphicsDrawing = false;
                   
                }
              
               // LeftArrowPbx.Image = arg["LeftArrowBitmap"] as Bitmap;

                //arg["SensorPosition"] = SensorPosition;
                //arg["LeftBitmap"] = camPropsHelper.LeftBitmap.Clone();
                //arg["RightBitmap"] = camPropsHelper.RightBitmap.Clone();
                //IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.RotaryMovedEvent, arg);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }

        internal void TriggerRecieved()
        {
            IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.SaveImgChanges, new Args());//this has been added to check whether any changes is made to the images and not saved.
            if (!isValueChanged)
            {
                CaptureStartTime = DateTime.Now;
                HowImageWasCaptured = IntucamHelper.CapturedUIMode.Trigger;
                Trigger_Or_SpacebarPressed(true);
            }
        }

        /// <summary>
        /// to start live or capture 
        /// </summary>
        /// <param name="isFromTrigger"></param>
        public bool Trigger_Or_SpacebarPressed(bool isFromTrigger)
        {
            bool returnVal = false;
            if (!IsCapturing && IVLCamVariables.isPowerConnected == Devices.PowerConnected && IVLCamVariables.isCameraConnected == Devices.CameraConnected)
            {
                    
                Args arg = new Args(); // create new arg for UI notification
                bool isCaptureSequence = false;// state to maintain whether capture or resume to live should happen
                if (!IsCapturing && IVLCamVariables.isLive)// if capture sequence is not in progress and if the camera is not live 
                    isCaptureSequence = true;// then set start capture sequence state to true

                if (isCaptureSequence)// if the capture sequence state is true
                {
                   
                    returnVal = StartCapture(isFromTrigger); // start capture with parameter initiation from trigger || (space bar || capture button)
                }
                else
                {
                    if (IVLCamVariables._eventHandler.isHandlerPresent(IVLCamVariables._eventHandler.UpdateCaptureRLiveUI))
                    {
                        arg["CaptureSequence"] = isCaptureSequence; // add the state to arg to update the UI wrt to the state
                        IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.UpdateCaptureRLiveUI, arg);// notify the UI update event
                    }
                   // returnVal = StartLive();// if the capture sequence state is false start live
                }
            }
            return returnVal;

        }
        int peakCnt;

        void FrameRateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (!IsCapturing)
                {
                    //if (IVLCamVariables._Settings.BoardSettings.EnableRightLeftSensor && !IVLCamVariables.isCapturing)
                    //{
                    //      IVLCamVariables.BoardHelper.CheckLeftRightSensorStatus();
                    //}
                    frameCnt += 1;
                    //return;
                    if (IVLCamVariables._eventHandler.isHandlerPresent(IVLCamVariables._eventHandler.FrameRateStatusUpdate))
                    {
                        Args arg = new Args();
                        double FrameRateVal = (double)(IVLCamVariables.CurrentFrameCnt - IVLCamVariables.PreviousFrameCnt) / FrameRateIntInSeconds;
                        FrameRateVal = Math.Round(FrameRateVal,1,MidpointRounding.AwayFromZero);
                        arg["FrameRate"] = FrameRateVal.ToString();
                        //arg["FrameRate"] = (IVLCamVariables.CurrentFrameCnt - IVLCamVariables.PreviousFrameCnt).ToString();
                        IVLCamVariables.PreviousFrameCnt = IVLCamVariables.CurrentFrameCnt;
                        
                        //arg["isPower"] = IVLCamVariables.BoardHelper.IsOpen;
                        //arg["isBoardOpen"] = IVLCamVariables.BoardHelper.IsOpen;
                        //arg["isCameraConnected"] = camPropsHelper.IsCameraConnected;
                        arg["TimeDiff"] = IVLCamVariables.CurrentFrameCnt;

                        //uint valExp = 0;
                        //ivl_Camera.GetExposure(ref valExp);

                        arg["Exposure"] = ivl_Camera.CurrentExposure;
                        //ushort valGain = 0;
                        //ivl_Camera.GetGain(ref valGain);
                        arg["Gain"] = ivl_Camera.CurrentGain;

                        // IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.FrameRateStatusUpdate, arg);
                        //FrameRate_lbl.Text = FrameRateLabelText + ((int)arg["TimeDiff"] - peakCnt).ToString();// e["FrameRate"] as string;// +"TrigSt =" + e["TriggerStatus"] as string; ;
                        FrameRate_lbl.Text = FrameRateLabelText + FrameRateVal.ToString();// e["FrameRate"] as string;// +"TrigSt =" + e["TriggerStatus"] as string; ;
                        peakCnt = (int)arg["TimeDiff"];

                        //This below if statements has been added by darshan on 14-08-2015 to solve defect no 0000370: The values shown in the Gain,Exposure and bottom of the screen mismatching.Since status bar label text was not getting updated
                        if (!IsCapturing)
                        {
                            gainStatus_lbl.Text = LiveGainLabelText + arg["Gain"] as string;// IVLVariables.ivl_Camera.camPropsHelper.LiveGain.ToString();
                            IVLCamVariables._Settings.CameraSettings.LiveGain = ivl_Camera.CurrentGain;
                        }
                        else
                            gainStatus_lbl.Text = CaptureGainLabelText + arg["Gain"] as string;//IVLVariables.ivl_Camera.camPropsHelper._Settings.CameraSettings.CaptureGain.ToString();
                        if (!IsCapturing)
                        {
                            ExposureStatus_lbl.Text = LiveExposureLabelText + arg["Exposure"] as string;// IVLVariables.ivl_Camera.camPropsHelper.CaptureExposure.ToString();
                            IVLCamVariables._Settings.CameraSettings.LiveExposure = ivl_Camera.CurrentExposure;
                        }
                        else
                            ExposureStatus_lbl.Text = CaptureExposureLabelText + arg["Exposure"] as string;// IVLVariables.ivl_Camera.camPropsHelper.LiveExposure.ToString();
                        //ExposureStatus_lbl.Text = IVLVariables.LangResourceManager.GetString( "LiveGain_Label_Text + exp.ToString();
                        //if ((bool)arg["isPower"])
                        //    PowerStatus_lbl.Text = PowerOnLabelText;//.LangResourceManager.GetString("Power_Text", IVLVariables.LangResourceCultureInfo) + IVLVariables.LangResourceManager.GetString("On_Text", IVLVariables.LangResourceCultureInfo);
                        //else
                        //    PowerStatus_lbl.Text = PowerOffLabelText;//.LangResourceManager.GetString("Power_Text", IVLVariables.LangResourceCultureInfo) + IVLVariables.LangResourceManager.GetString("Off_Text", IVLVariables.LangResourceCultureInfo);
                        //if ((bool)arg["isBoardOpen"])
                        //    comPort_lbl.Text = ComportOpenLabelText;//.LangResourceManager.GetString("Comport_Text", IVLVariables.LangResourceCultureInfo) + IVLVariables.LangResourceManager.GetString("Open_Text", IVLVariables.LangResourceCultureInfo);
                        //else
                        //    comPort_lbl.Text = ComportCloseLabelText;//.LangResourceManager.GetString("Comport_Text", IVLVariables.LangResourceCultureInfo) + IVLVariables.LangResourceManager.GetString("Closed_Text", IVLVariables.LangResourceCultureInfo);
                        //if ((bool)arg["isCameraConnected"])
                        //    CameraStatus_lbl.Text = CameraConnectionLabelText;//.LangResourceManager.GetString("Camera_Text", IVLVariables.LangResourceCultureInfo) + IVLVariables.LangResourceManager.GetString("Connected_Text", IVLVariables.LangResourceCultureInfo);
                        //else
                        //    CameraStatus_lbl.Text = CameraDisconnectionLabelText;//.LangResourceManager.GetString("Camera_Text", IVLVariables.LangResourceCultureInfo) + IVLVariables.LangResourceManager.GetString("Disconnected_Text", IVLVariables.LangResourceCultureInfo);
                        //totalFrameCount_Lbl.Text = "TotalFrames =" + arg["TimeDiff"] as string;
                    }
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        void FFATimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                Args arg = new Args();
                IVLCamVariables.FFASeconds++;
                TimeSpan ts = new TimeSpan(IVLCamVariables.FFASeconds * TimeSpan.TicksPerSecond);
                IVLCamVariables.FFATime = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
                arg["ffaTime"] = IVLCamVariables.FFATime;
                IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.UpdateFFATime, arg);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }

        /// <summary>
        /// To update camera power status
        /// </summary>
        /// <param name="arg"></param>
        internal void CameraPowerStatusUpdate(Args arg)
        {
            IVLCamVariables.PowerCameraStatusUpdateInProgress = true;
            string failureCategory = string.Empty;

            if (IVLCamVariables.isCameraConnected == Devices.CameraDisconnected || IVLCamVariables.isPowerConnected == Devices.PowerDisconnected)
            {
                if (captureTimer.Enabled)
                {
                    captureTimer.Enabled = false;
                    captureTimer.Stop();
                }
                if(IVLCamVariables.isCapturing || IVLCamVariables.isLive||IVLCamVariables.isResuming || isResetMode)
                    arg["ShowPopMsg"] = true;

                IVLCamVariables.isCapturing = false;
                IVLCamVariables.isResuming = false;
                isResetMode = false;
                if (motorMotionTimer.Enabled)// Stop Motor movement
                {
                    motorMotionTimer.Enabled = false;
                    motorMotionTimer.Stop();
                }
                IVLCamVariables.IsMotorMoved = true;
                isResetMode = false;
                ResetCaptureStates();
                //IVLCamVariables.isCaptureFailure = true;
                //CompleteCaptureSequence();
                //
                //    IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.SaveFrames2Disk, arg);
                if (IVLCamVariables._eventHandler.isHandlerPresent(IVLCamVariables._eventHandler.EnableDisableEmrButton))
                {
                    Args arg2 = new Args();
                    arg2["EnableEmrButton"] = true;
                    IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.EnableDisableEmrButton, arg2);
                }

                DisconnectCameraModule();

            }
            #region Update Camera Connection Status
            if (arg.ContainsKey("isCameraConnected"))
            {
                failureCategory = "Camera Disconnected";
                IVLCamVariables.captureFailureCode = CaptureFailureCode.CameraDisconnected;
                if (IVLCamVariables.isCameraConnected == Devices.CameraConnected)//checks whether the return value of isCameraConnected is of the type Devices enum type.
                    arg["isCameraConnected"] = true;
                else
                {

                    arg["isCameraConnected"] = false;
                } 
                IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.UPDATE_CAMERA_STATUS, arg);

            }
            #endregion

            #region Update Power Connection Status
            if (arg.ContainsKey("isPowerConnected"))
            {
                IVLCamVariables.captureFailureCode = CaptureFailureCode.PowerDisconnected;
                arg["FailureCode"] = IVLCamVariables.captureFailureCode;

                if (IVLCamVariables.isPowerConnected == Devices.PowerConnected)//checks whether the return value of isPowerConnected is of the type Devices enum type.
                    arg["isPowerConnected"] = true;
                else
                {
                 
                    arg["isPowerConnected"] = false;

                }
                IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.UPDATE_POWER_STATUS, arg);

            }
            #endregion
            //the below is code is added to resolve the issue of removing the power after capturing sequence is started.

            if (IVLCamVariables._eventHandler.isHandlerPresent(IVLCamVariables._eventHandler.UpdateMainWindowCursor))
            {
                arg["isDefault"] = true;
                IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.UpdateMainWindowCursor, arg);
            }
            IVLCamVariables.PowerCameraStatusUpdateInProgress = false;



        }
        
        
        /// <summary>
        /// this occurs when the motor reply is not recieved and a time out occurs the normal operation post motor to be done
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void motorMotionTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                motorMotionTimer.Stop();
                motorMotionTimer.Enabled = false;
                if (!IVLCamVariables.IsMotorMoved)
                {
                    IVLCamVariables.IsMotorMoved = true;
                    Args logArg = new Args();

                    logArg["TimeStamp"] = DateTime.Now;
                    logArg["Msg"] = string.Format("Motor Timeout");
                    //IVLCamVariables.BoardHelper.BulkTimerStartStop(true);
                    //if (IVLCamVariables.isCapturing)
                        //captureLog.Debug("Motor Timeout");
                        //IVLCamVariables.logClass.CaptureLogList.Add(string.Format("Time = {0}, Motor Timeout  ", DateTime.Now.ToString("HH-mm-ss-fff")));
                        //logArg["callstack"] = Environment.StackTrace;
                    capture_log.Add(logArg);
                     //IVLCamVariables.logClass.CameraLogList.Add(logArg);
                    //IVLCamVariables.logClass. IVLCamVariables.logClass.CameraLogList.Add(string.Format("Motor Timeout"));
                    bool isSuccess = false;
                    int errCode = -10;
                    byte[] readBuf = new byte[20];
                    //IntucamBoardCommHelper.USB_Read(readBuf, ref isSuccess, ref errCode,IVLCamVariables.BoardHelper.readTimeout);
                    string readBufStr = Encoding.UTF8.GetString(readBuf);
                    Args logArg1 = new Args();
                    
                    logArg1["TimeStamp"] = DateTime.Now;
                    logArg1["Msg"] = string.Format("Bulk Msg {0} , Success ={1},ErrorCode ={2}  ", readBufStr, isSuccess, errCode);
                    logArg1["Msg"] = string.Format("Bulk Msg {0} , Success ={1},ErrorCode ={2}  ", readBufStr, isSuccess, errCode);
                    //if (IVLCamVariables.isCapturing)
                        // captureLog.Debug(string.Format("Bulk Msg {0} , Success ={1},ErrorCode ={2}", readBufStr, isSuccess, errCode));
                        //logArg1["callstack"] = Environment.StackTrace;
                    capture_log.Add(logArg1);
                        //IVLCamVariables.logClass.CaptureLogList.Add();
                     //IVLCamVariables.logClass.CameraLogList.Add(logArg);
                    //IVLCamVariables.logClass. IVLCamVariables.logClass.CameraLogList.Add(string.Format("Bulk Msg {0} , Success ={1},ErrorCode ={2}  ", readBufStr, isSuccess, errCode));



                    //if (!IVLCamVariables.isResuming && IVLCamVariables.isCapturing)//checks for capturing and not resuming
                    //{

                    //    isResetMode = false;
                    //    IVLCamVariables.captureFailureCode = CaptureFailureCode.MotorTimeout;
                    //    IVLCamVariables.isCaptureFailure = true;
                        
                    //    IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.SaveFrames2Disk, new Args());

                    //}
                    //else
                    {
                        MotorMovementDone();

                    }
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }


        internal void CompleteCaptureSequence()
        {
            try
            {
                WriteCaptureLog();
               if(CameraIsLive)
                StopLive();
               ResetCaptureStates();
               captureTimer.Stop();
               captureTimer.Enabled = false;
                //IVLCamVariables.FrameBetweenFlashOnFlashOffDoneCnt = 0;
                IVLCamVariables.IsFlashOnDone = false;
                IsMotorMovementDone = true;
                    Args arg = new Args();
                    arg["ImageName"] = IVLCamVariables.ImagePath;
                    arg["FailureCode"] = IVLCamVariables.captureFailureCode;
                string output = "";
                foreach (char letter in IVLCamVariables.captureFailureCode.ToString())
                {
                    if (Char.IsUpper(letter) && output.Length > 0)
                        output += " " + letter;
                    else
                        output += letter;
                }
                arg["isCaptureFailed"] = IVLCamVariables.isCaptureFailure;
                arg["Capture Failure Category"] = output;
                arg["cameraSettings"] = IVLCamVariables.captureCameraSettings;
                arg["maskSettings"] = IVLCamVariables.captureMaskSettings;
                Args logArg = new Args();
                
                logArg["TimeStamp"] = DateTime.Now;
                logArg["Msg"] = string.Format("Capture Failure = {0}  ,Capture Failure Code ={1}", IVLCamVariables.isCaptureFailure, output);
                //captureLog.Info(string.Format("Capture Failure = {0}", IVLCamVariables.CaptureArg));
                //logArg["callstack"] = Environment.StackTrace;
                capture_log.Add(logArg);
                  string cameraSettings = "";
                StringWriter stringWriter = null;
                stringWriter = new StringWriter();
                serializer1.Serialize(stringWriter, IVLCamVariables._Settings.CameraSettings);
                cameraSettings = stringWriter.ToString();
                stringWriter.Close();
                stringWriter.Dispose();


                stringWriter = new StringWriter();
                serializer2.Serialize(stringWriter, IVLCamVariables._Settings.PostProcessingSettings);
                cameraSettings += Environment.NewLine + stringWriter.ToString();
                stringWriter.Close();
                stringWriter.Dispose();

                stringWriter = new StringWriter();
                serializer4.Serialize(stringWriter, IVLCamVariables._Settings.MotorOffSetSettings);
                cameraSettings += Environment.NewLine + stringWriter.ToString();
                stringWriter.Close();
                stringWriter.Dispose();

                stringWriter = new StringWriter();
                serializer3.Serialize(stringWriter, IVLCamVariables._Settings.BoardSettings);
                cameraSettings += Environment.NewLine + stringWriter.ToString();

                CaptureSettingsLog.Debug(string.Format("Capture Settings = {0}", cameraSettings));

              


                IVLCamVariables.ImageSavingInProgress = false;

                if (IVLCamVariables.isPowerConnected == Devices.PowerConnected && IVLCamVariables.isCameraConnected == Devices.CameraConnected)
                {
                    //if (IVLCamVariables.captureFailureCode != CaptureFailureCode.None)
                    //{
                    //    if (IVLCamVariables.ImagingMode == ImagingMode.FFA_Plus)
                    //        ResumeLive();
                       
                    //}
                    //else
                        IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.FrameCaptured, arg);
                }//if (IVLCamVariables.isCaptureFailure)
                //{
                //    IVLCamVariables.isCapturing = false;
                //    IVLCamVariables.isResuming = false;
                //    isResetMode = false;
                //    IVLCamVariables.IsMotorMoved = true;
                //    ResetCaptureStates();
                //}
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }
        }
        void IntucamBoardCommHelper__flashOffDone(EventArgs e)
        {
            // IVLCamVariables.IsFlashOffDone = true;
            // //    IVLCamVariables.CaptureImageIndx = (IVLCamVariables.CaptureImageList.Count);
            // //    captureLog.DebugFormat("Capture Indx current frame ={0}", IVLCamVariables.CaptureImageIndx);
            //// IVLCamVariables.IsFlashOffDone = true;
            // //if (!IVLCamVariables.isOnEventImage)
            // {
            //     IVLCamVariables.CaptureImageIndx = (IVLCamVariables.CaptureImageList.Count);
            //     captureLog.DebugFormat("Capture Indx current frame ={0}", IVLCamVariables.CaptureImageIndx);
            // }
            // //else
            // //{
            // //    IVLCamVariables.CaptureImageIndx = (IVLCamVariables.CaptureImageList.Count + 1);
            // //    captureLog.DebugFormat("Capture Indx Next frame ={0}", IVLCamVariables.CaptureImageIndx);

            // //}

        }



        public static IntucamHelper GetInstance()
        {
            try
            {
                if (camHelper == null)
                    camHelper = new IntucamHelper();
                IVLCamVariables.intucamHelper = camHelper;
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }
            return camHelper;
        }


        internal void updateAnalogInt2DigitalInt(int val)
        {
            try
            {
                // if (IVLCamVariables.isEnableFlashControlUsingKnob)
                {
                    byte[] intensity_Values = new byte[4];
                    intensity_Values[2] = 0x64;
                    float multiplier = 1.1f;// multiplication factor for white light to manage current at 0 to 1A in case of IR light
                    switch (camPropsHelper.ImagingMode)
                    {
                        case Imaging.ImagingMode.FFA_Plus:
                            {
                               

                                if (LEDSource == Led.Blue)
                                //intensity_Values[1] = (byte)(val +  IVLCamVariables._Settings.BoardSettings.FFA_Pot_Int_Offset);
                                {
                                    multiplier = 1.6f;// multiplication factor for white light to manage current at 0 to 1A in case of blue light
                                 
                                    intensity_Values[0] = 0x04; // Led Type : Blue.

                                }

                                if (LEDSource == Led.Flash)
                                {
                                    multiplier = 1.6f;// multiplication factor for white light to manage current at 0 to 1A in case of white light
                                    intensity_Values[0] = 0x02; // Led Type : Flash.

                                }
                                if (LEDSource == Led.IR)
                                // intensity_Values[1] = (byte)(val +  IVLCamVariables._Settings.BoardSettings.FFA_Pot_Int_Offset);
                                {
                                    intensity_Values[0] = 0x04; // Led Type : IR.

                                }
                                /* Set intensity of flash and turn it on */
                                multiplier *= val;
                                val = (int)multiplier;
                                intensity_Values[1] = (byte)(val + IVLCamVariables._Settings.BoardSettings.FFA_Color_Pot_Int_Offset); // intensity value is multiplier * val + offset
                                
                                IVLCamVariables.BoardHelper.Set_LED_Intensity(intensity_Values);
                                /* Turn on Blue light */
                                // this.WhiteLightOnOff(true);
                                break;
                            }
                        case Imaging.ImagingMode.FFAColor:
                            {

                                if (LEDSource == Led.Blue)
                                //intensity_Values[1] = (byte)(val +  IVLCamVariables._Settings.BoardSettings.FFA_Pot_Int_Offset);
                                {
                                    multiplier = 1.6f;// multiplication factor for white light to manage current at 0 to 1A in case of blue light

                                    intensity_Values[0] = 0x04; // Led Type : Blue.

                                }

                                if (LEDSource == Led.Flash)
                                {
                                    multiplier = 1.6f;// multiplication factor for white light to manage current at 0 to 1A in case of white light
                                    intensity_Values[0] = 0x02; // Led Type : Flash.

                                }
                                if (LEDSource == Led.IR)
                                // intensity_Values[1] = (byte)(val +  IVLCamVariables._Settings.BoardSettings.FFA_Pot_Int_Offset);
                                {
                                    intensity_Values[0] = 0x04; // Led Type : IR.

                                }
                                /* Set intensity of flash and turn it on */
                                multiplier *= val;
                                val = (int)multiplier;
                                intensity_Values[1] = (byte)(val + IVLCamVariables._Settings.BoardSettings.FFA_Color_Pot_Int_Offset); // intensity value is multiplier * val + offset

                                IVLCamVariables.BoardHelper.Set_LED_Intensity(intensity_Values);
                                break;
                            }
                        case Imaging.ImagingMode.Posterior_45:
                            {
                                if (_updateAnalogvalDigitalVal != null && IVLCamVariables._Settings.BoardSettings.EnablePCU_IntensityControl)
                                {
                                    if(camPropsHelper.IsCameraConnected == Devices.CameraConnected)
                                    {
                                    int gainExpValue = 0;
                                    if (val < maxExposureIndex)
                                        gainExpValue = val;
                                    else
                                        gainExpValue = maxExposureIndex;

                                        UpdateExposureGainFromTable(val,true);
                                    }
                                        

                                }
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
                //else
                //    UpdateExposureGainFromTable(val);
            }
            catch (Exception ex)
            {

               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
         /// <summary>
       /// Property to set and get Capture Gain which uses a local private variable _CaptureGain
       /// </summary>
       public void UpdateExposureGainFromTable(int AnalogVal,bool isFromPot)
       {
           try
           {
               if (isFromPot)
               {
                   _updateAnalogvalDigitalVal(AnalogVal);//Added to handle the scenario when 
               }
               ivl_Camera.UpdateExposureGainFromTable(AnalogVal);
               if (CameraIsLive)
               {
                    int[] values = ivl_Camera.GetExposureGainFromTable(AnalogVal);
                    IVLCamVariables._Settings.CameraSettings.LiveExposure = (uint)values[0];
                    IVLCamVariables._Settings.CameraSettings.LiveGain = (ushort)values[1];
               }
           }
           catch (Exception ex)
           {

              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log);
           }
       }

        internal void leftRightEvent(bool isLeft)
        {
            try
            {
                if (IVLCamVariables._Settings.BoardSettings.EnableRightLeftSensor)//to check whether the left ringht sensor is enabled in settings.
                {
                    if (isLeft)
                    {
                        camPropsHelper.LeftRightPos = LeftRightPosition.Left;

                    }
                    else
                        camPropsHelper.LeftRightPos = LeftRightPosition.Right;
                    arg["LeftRightPos"] = camPropsHelper.LeftRightPos;
                    if(IVLCamVariables._eventHandler.ChangeLeftRightPos_Live != null)
                        IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.ChangeLeftRightPos_Live, arg);
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }

        internal void motorSensorPositionEvent(int SensorPosition, int resetStepValue)
        {
            try
            {
                UpdateDiaptorImageForRotary(SensorPosition);
                camPropsHelper.MotorResetSteps = resetStepValue;
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }


        }

        /// <summary>
        /// Method to recieve motor fast forward done from interrupt
        /// </summary>
        /// <param name="e"></param>

        void IntucamBoardCommHelper_triggerRecieved(bool isTrigger, EventArgs e)
        {
            try
            {
                IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.triggerRecieved, new Args());

            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
                CustomMessageBox.Show(ex.StackTrace);
            }
        }
        #region Methods to Communicate with the microcontroller board

        private void StartMotorMovement(bool isForward, int steps)
        {
            try
            {
                if (steps > -100)
                {
                    if (isForward)
                    {
                        if (steps < 0)
                        {
                            StartMotorMovement(false, -steps);
                        }
                        else
                        {    
                            byte motorMovementVal = Convert.ToByte(steps);
                            IVLCamVariables.BoardHelper.MotorFastForward(motorMovementVal);
                        }
                    }
                    else
                    {
                        if (steps < 0)
                        {
                            StartMotorMovement(true, -steps);
                        }
                        else
                        {
                            byte motorMovementVal = Convert.ToByte(steps);
                            IVLCamVariables.BoardHelper.MotorFastBackward(motorMovementVal);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }

        private void ResetVSync()
        {
            try
            {
                IVLCamVariables.BoardHelper.ResetVSync();
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }

        private byte GetStrobeWidth(int index)
        {
            byte width = 0;
            try
            {

                if (ivl_Camera.isOpenCamera)
                    width = ivl_Camera.GetStrobeWidth(index);

            }
            catch (Exception ex)
            {

               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }
            return width;

        }

        #endregion




        internal bool ConnectCameraBoard()
        {
            bool returnVal = false;
            try
            {
                SetCameraSettings();
                if (IVLCamVariables.isCameraConnected==Devices.CameraConnected && !ivl_Camera.isOpenCamera  )//if camera is connected and it is not open
                {

                    ivl_Camera.camHandle = this.cameraWindow.Handle;
                    returnVal = isCameraOpen = ivl_Camera.OpenCamera();

                    if (ivl_Camera.isOpenCamera && camPropsHelper.IsCameraConnected == Devices.CameraConnected)//if camera is connected and camera is open
                    {
                        if (CameraName.Contains("E3CMOS06300"))
                        {
                            ivl_Camera.SetExposure(IVLCamVariables._Settings.CameraSettings.CaptureExposure);
                        }
                    }
                    else
                        IVLCamVariables.isCameraConnected = Devices.CameraDisconnected;
                }
                if (!IVLCamVariables.BoardHelper.IsOpen && IVLCamVariables.isPowerConnected == Devices.PowerConnected)
                {
                    IVLCamVariables.BoardHelper.Open();

                }
                if (FrameRateTimer == null)
                {
                    FrameRateTimer = new System.Timers.Timer();
                    FrameRateTimer.Elapsed += FrameRateTimer_Elapsed;
                    FrameRateTimer.Interval = 3000;
                    FrameRateTimer.Enabled = false;
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
                returnVal = false;
            }

            return returnVal;

        }

        public bool SetLiveExposure()
        {
            try
            {
                return ivl_Camera.SetExposure(camPropsHelper._Settings.CameraSettings.LiveExposure);

            }
            catch (Exception ex)
            {
                CameraLogger.WriteException(ex, Exception_Log);
                return false;
                
            }
        }
        private void DisconnectCamera()
        {
            try
            {
                if (ivl_Camera.isOpenCamera)
                {
                   
                    FrameRateTimer.Enabled = false;
                    FrameRateTimer.Stop();
                    //ivl_Camera.camHandle = IntPtr.Zero;// remove the cam handle from firing any events during close of the camera
                    ivl_Camera.CloseCamera();
                    IVLCamVariables.isLive = false;

                    isCameraSettingsSet = false;
                    this.isCameraOpen = ivl_Camera.isOpenCamera;
                    this.ivl_Camera.camHandle = IntPtr.Zero;
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        private void DisconnectBoard()
        {
            try
            {
                if (IVLCamVariables.BoardHelper.IsOpen)
                {
                    IVLCamVariables.BoardHelper.Close();
                    IVLCamVariables.isLive = false;

                }
            }
            catch (Exception ex)
            {
                    CameraLogger.WriteException(ex, Exception_Log);
            }

        }

        void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //if (IVLCamVariables.BoardHelper.InterruptDataTransferThread.ThreadState != ThreadState.AbortRequested)
            {
                IVLCamVariables.BoardHelper.InterruptDataTransferThread.Abort();
                IVLCamVariables.BoardHelper.Close();
                System.Timers.Timer t = sender as System.Timers.Timer;
                //t.Stop();
                //t.Dispose();
            }
            //else
            //{
               
            //}
        }
        public void DisconnectCameraModule()
        {
            try
            {
                //CameraPowerStatusTimer.Stop();
                //CameraPowerStatusTimer.Enabled = false;
                //CameraPowerStatusTimer.Elapsed -= CameraPowerStatusTimer_Elapsed;
                DisconnectCamera();
                DisconnectBoard();
                IntucamBoardCommHelper.BulkLogList.RemoveAll(x => x.Count == 0);
                IntucamBoardCommHelper.InterruptLogList.RemoveAll(x => x.Count == 0);
               
                IVLCamVariables.logClass.BulkTransferLogList.AddRange(IntucamBoardCommHelper.BulkLogList);
                //IVLCamVariables.logClass.BulkTransferLogList.AddRange(IntucamBoardCommHelper.InterruptLogList);
                IVLCamVariables.logClass.BulkTransferLogList = IVLCamVariables.logClass.BulkTransferLogList.OrderBy(x => x["TimeStamp"]).ToList(); ;

                IVLCamVariables.logClass.InterruptTransferLogList.AddRange( IntucamBoardCommHelper.InterruptLogList);
                IVLCamVariables.logClass.CameraLogList.AddRange(IVLCamVariables.CameraLogList);
                IVLCamVariables.logClass.FrameLogList.AddRange(Camera.frameLogList);
                IVLCamVariables.logClass.WriteLogs2File();
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }


        }
        public bool RestartCamera()
        {
            bool returnVal;
            try
            {
                ivl_Camera.CloseCamera();
                returnVal = ivl_Camera.OpenCamera();
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
                returnVal = false;
            }
            return returnVal;
        }

        /// <summary>
        /// To enable the motor time if it is not enabled.
        /// </summary>
        internal void StartCaptureMotorTimer()
        {
            if (!motorMotionTimer.Enabled)
            {
                motorMotionTimer.Enabled = true;
                motorMotionTimer.Start();
            }
        }
      

        public bool StartCapture(bool FromTrigger)
        {
            try
            {
                bool returnVal = false;
                Args logArg = new Args();
                logArg["TimeStamp"] = DateTime.Now;
                logArg["Msg"] = string.Format("Capture Mode = {0}, Capture Time = {1}  ", HowImageWasCaptured, CaptureStartTime.ToString("HH-mm-ss-fff"));
                //logArg["callstack"] = Environment.StackTrace;
                capture_log.Add(logArg);
                captureBgThreadWork(new object());

                //ThreadPool.QueueUserWorkItem(captureBgThreadWork);
                    returnVal = true;
                return returnVal;
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                return false;
                // CameraLogger.WriteException(ex, Exception_Log);
            }
        }

        private void StartEnableControls()
        {
           
        }

        public void ResumeLive()
        {
            try
            {
                IVLCamVariables.isResuming = true;
                isDirCreated = false;
                //Args arg = new Args();
                //arg["ffaTime"] = "Resuming";
                //IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.UpdateFFATime, arg);
                FrameRate_lbl.Text = "Current Camera Status = Resuming";// e["FrameRate"] as string;// +"TrigSt =" + e["TriggerStatus"] as string; ;

                motorMotionTimer.Interval = (IVLCamVariables._Settings.BoardSettings.MotorSteps * IVLCamVariables._Settings.BoardSettings.MotorPerStepTime + IVLCamVariables._Settings.BoardSettings.MotorStepOffsetTime);// time interval for motor timer time out in milliseconds always set at the time of capture
                if (IVLCamVariables._Settings.BoardSettings.MotorSteps != 0)
                {
                    StartMotorMovement(!IVLCamVariables._Settings.BoardSettings.MotorPolarityIsForward, (byte)IVLCamVariables._Settings.BoardSettings.MotorSteps);
                }
                else
                    MotorMovementDone();
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }


        }

        private DateTime captureStartTime;

        public DateTime CaptureStartTime
        {
            get { return captureStartTime; }
            set { captureStartTime = value; }
        }
        public enum CapturedUIMode { CaptureButton, SpaceBar, Trigger };

        private CapturedUIMode howImageWasCaptured;

        public CapturedUIMode HowImageWasCaptured
        {
            get { return howImageWasCaptured; }
            set { howImageWasCaptured = value; }
        }


        public string CameraName
        {
            get { return IVLCamVariables.DisplayName; }
        }

        /// <summary>
        /// To reset the capture variables added by Kishore on 25 September 2017.
        /// </summary>
        private void ResetCaptureStates()
        {
            #region Reset variables for capture
            if (IVLCamVariables._Settings.CameraSettings.isRawMode)
            {
                if (ivl_Camera.RawImageList == null)
                {
                    ivl_Camera.RawImageList = new List<byte[]>(); // create new list of raw image list if it is null
                }
                else if(ivl_Camera.RawImageList.Count > 0)
                    ivl_Camera.RawImageList.Clear(); // clear if existing list is present
            }
            else
            {
                if (ivl_Camera.CaptureImageList == null) // create new list of image list if it is null
                    ivl_Camera.CaptureImageList = new List<Bitmap>();
                else if(ivl_Camera.CaptureImageList.Count > 0 )
                    ivl_Camera.CaptureImageList.Clear();// clear if existing list is present
            }
            if (IVLCamVariables._Settings.CameraSettings.isRawMode)
            {
                IVLCamVariables.FlashIndx = 0;
                IVLCamVariables.GRIndx = 0;
                IVLCamVariables.GBIndx = 0;
                IVLCamVariables.imageCatList.Clear();
                IVLCamVariables.imageCategoryValues.Clear();
                IVLCamVariables.ivl_Camera.RawImageList.Clear();
                IVLCamVariables.CaptureImageIndx = -1;


            }
            else
            {
                IVLCamVariables.CaptureImageIndx = -1;
            }
            IVLCamVariables.isIRSaved = false;
            IVLCamVariables.IsFlashOffDone = false;
            
            IVLCamVariables.BoardHelper.isCptrRecieved = false;

            //IVLCamVariables.isResuming = IVLCamVariables.isCapturing = false;
            #endregion
        }
        double FrameRateIntInSeconds = 1f;
        private void showSplashCallback(object callback)
        {
            Args arg = new Args();
            arg["ShowSplashScreenMsg"] = "";
            arg["ShowSplash"] = true;
            IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.ShowSplashScreen, arg);

        }
        /// <summary>
        /// Initialize camera and board if they have not been opened show a splash while initializing
        /// </summary>
        public void InitCameraBoard()
        {
            if (!ivl_Camera.isOpenCamera || !IVLCamVariables.BoardHelper.IsOpen)// if either camera or board is not during start live open them
            {
                Args arg = new Args();
                arg["ShowSplashScreenMsg"] = "";
                arg["ShowSplash"] = true;

                ThreadPool.QueueUserWorkItem(showSplashCallback);
                ConnectCameraBoard();
                arg["ShowSplash"] = false;
                IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.ShowSplashScreen, arg);
            }
        }
        public bool StartLive()
        {
            bool returnVal = false;
            try
            {
                if (IVLCamVariables.isCameraConnected == Devices.CameraConnected && IVLCamVariables.isPowerConnected == Devices.PowerConnected)
                {

                    InitCameraBoard();// Call init camera board if the camera or board is not opened for the first time of live mode
                    if (!IsResuming)
                        camPropsHelper.SetLiveCameraSettings();
                    if(ivl_Camera.maskOverlayPbx != null)
                        if (!ivl_Camera.maskOverlayPbx.Visible && Convert.ToBoolean( ConfigVariables.CurrentSettings.PostProcessingSettings.MaskSettings._ApplyLiveMask.val))
                        {
                            ivl_Camera.maskOverlayPbx.Visible = true;
                        } 
                    ThreadPool.QueueUserWorkItem(new WaitCallback(f=>{

                            IVLCamVariables.BoardHelper.TriggerOn();
                            IVLCamVariables.BoardHelper.GetMotorPosition();
                            LEDSource = (Led)Enum.Parse(typeof(Led), ConfigVariables.CurrentSettings.CameraSettings.LiveLedSource.val);
                   
                        if (IVLCamVariables.ImagingMode == ImagingMode.FFA_Plus || IVLCamVariables.ImagingMode == ImagingMode.FFAColor)
                    {
                        if (IVLCamVariables._Settings.BoardSettings.EnablePCU_IntensityControl)
                            IVLCamVariables.BoardHelper.Read_LED_SupplyValues();
                        
                        UpdateExposureGainFromTable(IVLCamVariables._Settings.CameraSettings.LiveGainIndex, false);
                           

                    }
                    else if (IVLCamVariables.ImagingMode == ImagingMode.Posterior_45)
                    {
                        if (IVLCamVariables._Settings.BoardSettings.EnablePCU_IntensityControl)
                            IVLCamVariables.BoardHelper.Read_LED_SupplyValues();
                        else
                            UpdateExposureGainFromTable(IVLCamVariables._Settings.CameraSettings.LiveGainIndex, false);
                    }
                    else
                    {
                        ivl_Camera.SetGain(IVLCamVariables._Settings.CameraSettings.LiveGain);
                    }

                    //if (IVLCamVariables._eventHandler.isHandlerPresent(IVLCamVariables._eventHandler.EnableDisableEmrButton))
                    //{
                    //    Args arg = new Args();
                    //    arg["EnableEmrButton"] = true;
                    //    IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.EnableDisableEmrButton, arg);
                    //}
                }));
                    returnVal = ivl_Camera.StartLiveMode();
                    arg["isTurnOffTimer"] = returnVal;
                    IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.TurnOnOffInboxTimer, arg);
                }
                else
                {
                    //Args arg = new Args();
                    //if (IVLCamVariables._eventHandler.isHandlerPresent(IVLCamVariables._eventHandler.EnableDisableEmrButton))
                    //{
                    //    arg["EnableEmrButton"] = true;
                    //    IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.EnableDisableEmrButton, arg);
                    //}
                    if (IVLCamVariables._eventHandler.isHandlerPresent(IVLCamVariables._eventHandler.GoToViewScreen))
                    {
                        IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.GoToViewScreen, arg);
                        arg["isTurnOffTimer"] = false;
                        IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.TurnOnOffInboxTimer, arg);
                    }

                }
            }
            catch (Exception ex)
            {
                CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
                returnVal = false;
            }

            return returnVal;
        }
        internal void WriteCaptureLog()
        {
            try
            {
                #region Merge all capture logs from different classes of Camera module
                //Args arg = new Args();
                //arg["ffaTime"] = "Log writing";
                //IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.UpdateFFATime, arg);
                FrameRate_lbl.Text = "Current Camera Status = Log writing";// e["FrameRate"] as string;// +"TrigSt =" + e["TriggerStatus"] as string; ;

                List<Args> capLogList = new List<Args>();
                lock (IntucamHelper.capture_log)
                {
                    if (IntucamHelper.capture_log != null)
                    {
                        capLogList.AddRange(IntucamHelper.capture_log);
                    }
                }
                lock (Camera.capture_log)
                {
                    if (Camera.capture_log != null)
                    {
                        capLogList.AddRange(Camera.capture_log);
                    }
                }
                lock (IntucamBoardCommHelper.capture_log)
                {
                    if (IntucamBoardCommHelper.capture_log != null)
                    {
                        capLogList.AddRange(IntucamBoardCommHelper.capture_log);
                    }
                }
                lock (PostProcessing.capture_log)
                {
                    if (PostProcessing.capture_log != null)
                    {
                        capLogList.AddRange(PostProcessing.capture_log);
                    }
                }
                lock (ImageSaveHelper.capture_log)
                {
                    if (ImageSaveHelper.capture_log != null)
                    {
                        capLogList.AddRange(ImageSaveHelper.capture_log);
                    }
                }
                capLogList = capLogList.Where(x => x.ContainsKey("TimeStamp")).ToList();
                capLogList = capLogList.OrderBy(x => x["TimeStamp"]).ToList(); ;
                //capLogList.Sort();

                #endregion
                IVLCamVariables.logClass.WriteCaptureLog2File(capLogList);
                FrameRate_lbl.Text = "Current Camera Status = Log Completed";// e["FrameRate"] as string;// +"TrigSt =" + e["TriggerStatus"] as string; ;

            }
            catch (Exception ex)
            {
                CameraLogger.WriteException(ex, Exception_Log);
                
            }
          
            
        }

        public bool StopLive()
        {
            bool returnVal = false;

            try
            {
                if (CameraIsLive)//if camera is already in live then stop live
                {
                    if (ivl_Camera.isOpenCamera)
                    {
                        returnVal = ivl_Camera.StopLiveMode();
                        FrameRateTimer.Stop();
                        FrameRateTimer.Enabled = false;
                        arg["isTurnOffTimer"] = false;
                        IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.TurnOnOffInboxTimer, arg);

                    }

                    if (IVLCamVariables.BoardHelper.IsOpen)
                    {
                        IVLCamVariables.BoardHelper.TriggerOFF();
                        IVLCamVariables.BoardHelper.IRLightOff();
                    }
                    //if (camPropsHelper.FFATimer.Enabled)
                    //{
                    //    camPropsHelper.FFATimer.Enabled = false;
                    //    camPropsHelper.FFATimer.Stop();
                    //}
                    frameCnt = 0;
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
                returnVal = false;
            }

            return returnVal;
        }

        /// <summary>
        /// To dispose the times when the main window is closing
        /// </summary>
        public void DisposeCameraBoardTimers()
        {
            StopCameraBoardTimers();
           // CameraPowerStatusTimer = null;
            FrameRateTimer = null;
        }


        /// <summary>
        /// To stop the timers added by Kishore on 25 September 2017.
        /// </summary>
        public void StopCameraBoardTimers()
        {
            //CameraPowerStatusTimer.Enabled = false;
            //CameraPowerStatusTimer.Stop();
            if (ivl_Camera.isOpenCamera)
            {
                FrameRateTimer.Enabled = false;
                FrameRateTimer.Stop();
            }
        }

        /// <summary>
        /// To start the camera and board timers
        /// </summary>
        public void OpenCameraBoard()
        {
            //cameraWindow.CameraPowerUpdateThreadWork(new object());
            cameraWindow.StartPowerCameraCheck(false);
        }

        public void ResetMotor()
        {
        //    CameraPowerStatusTimer.Enabled = false;
        //    CameraPowerStatusTimer.Stop();
            InitCameraBoard();
            FrameRateTimer.Enabled = false;
            FrameRateTimer.Stop();
            isResetMode = true;
            //IVLCamVariables.IsMotorMoved = false;
            IVLCamVariables.BoardHelper.GetMotorPosition();
            Thread.Sleep(100);
            //IVLCamVariables.BoardHelper.BulkTransfer();
            //IVLCamVariables.BoardHelper.BulkTimerStartStop(true);
            if (!IVLCamVariables._Settings.BoardSettings.isMotorSensorPresent)

                motorMotionTimer.Interval = (IVLCamVariables._Settings.BoardSettings.MotorSensorStepsMax * IVLCamVariables._Settings.BoardSettings.MotorPerStepTime + IVLCamVariables._Settings.BoardSettings.MotorStepOffsetTime);// time interval for motor timer time out in milliseconds always set at the time of capture

            else
                motorMotionTimer.Interval = (Math.Abs(camPropsHelper.MotorResetSteps) * IVLCamVariables._Settings.BoardSettings.MotorPerStepTime + IVLCamVariables._Settings.BoardSettings.MotorStepOffsetTime);// time interval for motor timer time out in milliseconds always set at the time of capture

            IVLCamVariables.BoardHelper.ResetMotorPosition(false);// reset of the motor done 
       
        }
        /// <summary>
        /// Method used to change the mode from posterior to anterior or vice-versa which moves the motor reset position and then moves the motor to desired offset motor steps
        /// </summary>
        public void ChangeMode()
        {
            try
            {
                ResetMotor();
            }
            catch (Exception ex)
            {
                CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }

        public void TriggerOn()
        {
            try
            {
                IVLCamVariables.BoardHelper.TriggerOn();
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }

        public void TriggerOff()
        {
            try
            {
                IVLCamVariables.BoardHelper.TriggerOFF();
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }


        //public void callWndProc(ref System.Windows.Forms.Message m)
        //{
        //    ivl_Camera.callWndProc(ref m);
        //}


        private void SetCameraSettings()
        {
            try
            {
                if (!isCameraSettingsSet)// set the camera settings only if the settings are not set
                {

                    IVLCamVariables._Settings.ImageSaveSettings.RawImageDirPath = ConfigVariables.CurrentSettings.ImageStorageSettings._LocalStoragePath.val;
                    if (ProcessedImagePathFromConfig)
                        IVLCamVariables._Settings.ImageSaveSettings.ProcessedImageDirPath = ConfigVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val;
                    IVLCamVariables._Settings.ImageSaveSettings.isMRNFolderSave = Convert.ToBoolean(ConfigVariables.CurrentSettings.ImageStorageSettings.IsMrnFolder.val);
                    IVLCamVariables._Settings.CameraSettings.ImageWidth = Convert.ToInt32(ConfigVariables.CurrentSettings.CameraSettings._ImageWidth.val);
                    IVLCamVariables._Settings.CameraSettings.ImageHeight = Convert.ToInt32(ConfigVariables.CurrentSettings.CameraSettings._ImageHeight.val);
                    IVLCamVariables._Settings.CameraSettings.roiX = Convert.ToInt32(ConfigVariables.CurrentSettings.CameraSettings._ImageROIX.val);
                    IVLCamVariables._Settings.CameraSettings.roiY = Convert.ToInt32(ConfigVariables.CurrentSettings.CameraSettings._ImageROIY.val);
                    IVLCamVariables._Settings.ImageSaveSettings._ImageSaveFormat = (ImageSaveFormat)Enum.Parse(typeof(ImageSaveFormat), ConfigVariables.CurrentSettings.ImageStorageSettings._ImageSaveFormat.val.ToLower());
                    IVLCamVariables._Settings.CameraSettings.isRawMode = Convert.ToBoolean(ConfigVariables.CurrentSettings.CameraSettings._EnableRawMode.val);
                    IVLCamVariables._Settings.PostProcessingSettings.maskSettings.maskCentreX = Convert.ToInt32(ConfigVariables.CurrentSettings.CameraSettings._ImageOpticalCentreX.val);
                    IVLCamVariables._Settings.PostProcessingSettings.maskSettings.maskCentreY = Convert.ToInt32(ConfigVariables.CurrentSettings.CameraSettings._ImageOpticalCentreY.val);

                    IVLCamVariables._Settings.BoardSettings.InterruptTimeInterval = Convert.ToInt32(ConfigVariables.CurrentSettings.FirmwareSettings.InterruptTimeInterval.val);
                    IVLCamVariables._Settings.CameraSettings.IRCheckValue = Convert.ToDouble(ConfigVariables.CurrentSettings.CameraSettings.IRCheckValue.val);

                    IVLCamVariables._Settings.PostProcessingSettings.maskSettings.maskWidth = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.MaskSettings.CaptureMaskWidth.val);
                    IVLCamVariables._Settings.PostProcessingSettings.maskSettings.maskHeight = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.MaskSettings.CaptureMaskHeight.val);

                    IVLCamVariables._Settings.PostProcessingSettings.maskSettings.LiveMaskWidth = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.MaskSettings.LiveMaskWidth.val);
                    IVLCamVariables._Settings.PostProcessingSettings.maskSettings.LiveMaskHeight = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.MaskSettings.LiveMaskHeight.val);
                    IVLCamVariables._Settings.BoardSettings.MotorSensorStepsMax = Convert.ToInt32(ConfigVariables.CurrentSettings.FirmwareSettings.MotorSensorStepsMax.val);
                    IVLCamVariables._Settings.BoardSettings.isMotorSensorPresent = Convert.ToBoolean(ConfigVariables.CurrentSettings.FirmwareSettings.IsMotorSensorPresent.val);
                    IVLCamVariables._Settings.BoardSettings.GreenFilterPos = Convert.ToInt32(ConfigVariables.CurrentSettings.FirmwareSettings.GreenFilterPos.val);
                    IVLCamVariables._Settings.BoardSettings.BlueFilterPos = Convert.ToInt32(ConfigVariables.CurrentSettings.FirmwareSettings.BlueFilterPos.val);
                    //IVLVariables.ivl_Camera. IVLCamVariables._Settings.PostProcessingSettings.maskSettings.isApplyLiveMask = Convert.ToBoolean(ConfigVariables.CurrentSettings.PostProcessingSettings.MaskSettings._ApplyLiveMask.val);
                    IVLCamVariables._Settings.BoardSettings.InterruptTimeInterval = Convert.ToInt64(ConfigVariables.CurrentSettings.FirmwareSettings.InterruptTimeInterval.val);
                    captureTimeout = IVLCamVariables._Settings.BoardSettings.InterruptTimeInterval;
                    ivl_Camera.DefGainExpIndx = Convert.ToInt32(ConfigVariables.CurrentSettings.CameraSettings._exposureIndex.val);
                    ivl_Camera.BMwidth = Convert.ToInt32(ConfigVariables.CurrentSettings.CameraSettings._ImageWidth.val) - 2 * Convert.ToInt32 (ConfigVariables.CurrentSettings.CameraSettings._ImageROIX.val);
                    ivl_Camera.BMheight = Convert.ToInt32(ConfigVariables.CurrentSettings.CameraSettings._ImageHeight.val) - 2 * Convert.ToInt32(ConfigVariables.CurrentSettings.CameraSettings._ImageROIY.val);
                    StandByTimeOut = Convert.ToInt32(ConfigVariables.CurrentSettings.CameraSettings.CameraStandbyTime.val);
                    standByTimer.Interval = StandByTimeOut * oneMinInMilliseconds;
                }
                else
                {
                    // get settings from eeprom to be implemented later

                }
                PostProcessing.initDemosaic(IVLCamVariables._Settings.CameraSettings.ImageWidth - 2 * IVLCamVariables._Settings.CameraSettings.roiX, IVLCamVariables._Settings.CameraSettings.ImageHeight - 2 * IVLCamVariables._Settings.CameraSettings.roiY, IVLCamVariables._Settings.PostProcessingSettings.maskSettings.maskCentreX, IVLCamVariables._Settings.PostProcessingSettings.maskSettings.maskCentreY);// this is done in order to initialize the postprocessing settings in the c++ dll of demosaic to fix the defect 0001340.

                isCameraSettingsSet = true;
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
       

        void SetMotorOffSetValue()
        {
        }

        internal void MotorMovementDone()
        {
            try
            {
                
                motorMotionTimer.Enabled = false;
                motorMotionTimer.Stop();
                Args logArg = new Args();

                logArg["TimeStamp"] = DateTime.Now;
                logArg["Msg"] = string.Format("Motor Movement isCapturing = {0}, isResuming = {1} isMotorMovement = {2}  ", IVLCamVariables.isCapturing, IVLCamVariables.isResuming, IVLCamVariables.IsMotorMoved);
                if (IVLCamVariables.isCapturing)
                    //captureLog.Info("Motor Movement isCapturing ={0} isResuming = {1} isMotorMovement = {2}", IVLCamVariables.isCapturing,IVLCamVariables.isResuming,IVLCamVariables.IsMotorMoved);
                    capture_log.Add(logArg);
                    //IVLCamVariables.logClass.CaptureLogList.Add();
                 //IVLCamVariables.logClass.CameraLogList.Add(logArg);
                //IVLCamVariables.logClass. IVLCamVariables.logClass.CameraLogList.Add(string.Format("Time = {0}, Motor Movement isCapturing = {1}, isResuming = {2} isMotorMovement = {3}  ", DateTime.Now.ToString("HH-mm-ss-fff"), IVLCamVariables.isCapturing, IVLCamVariables.isResuming, IVLCamVariables.IsMotorMoved));

               
                if (IVLCamVariables.IsMotorMoved) // if motor movement done has not been called 
                {
                    if (IVLCamVariables.isCapturing && !IVLCamVariables.isResuming) // it is the capturing sequence . in the start of the capture live to capture compensation
                    {
                        //captureLog.Info("Motor Movement Done Method");
                        Args logArg1 = new Args();

                        logArg1["TimeStamp"] = DateTime.Now;
                        logArg1["Msg"] = "Motor Movement Done Method ";
                        capture_log.Add(logArg1);
                        //IVLCamVariables.logClass.CaptureLogList.Add(string.Format("Time = {0}, Motor Movement Done Method  ", DateTime.Now.ToString("HH-mm-ss-fff")));
                         //IVLCamVariables.logClass.CameraLogList.Add(logArg);
                        //IVLCamVariables.logClass. IVLCamVariables.logClass.CameraLogList.Add(string.Format("Time = {0}, Motor Movement Done Method  ", DateTime.Now.ToString("HH-mm-ss-fff")));


                        IVLCamVariables.BoardHelper.CaptureCommand(); // call capture command CPTR

                    }
                    else if (IVLCamVariables.isResuming)// capturing sequence capture to live compensation
                    {
                        LEDSource = LEDSource;
                      // if the camera is in capturing mode write capture log to file
                    {
                        WriteCaptureLog();
                        IntucamHelper.capture_log.Clear();
                        Camera.capture_log.Clear();
                        IntucamBoardCommHelper.capture_log.Clear();
                        PostProcessing.capture_log.Clear();
                        ImageSaveHelper.capture_log.Clear();
                    }

                        StartLive();

                        IVLCamVariables.isCapturing = false;
                        IVLCamVariables.isResuming = false;
                        IVLCamVariables.captureFailureCode = CaptureFailureCode.None;
                        IVLCamVariables.isCaptureFailure = false;

                        //Form form1 = new Form();
                        captureTimer.Enabled = false;
                        captureTimer.Stop();
                        //form1.Text = "Resuming";
                        //form1.Show();
                        //Thread.Sleep(1000);
                        //form1.Close();
                    }
                    else if (isResetMode)// State to maintain change in mode and reset of motor
                    {

                        
                        #region This code is done to change the offset from posterior to anterior focus after reset of the motor is done
                        if (IVLCamVariables.ImagingMode == Imaging.ImagingMode.Anterior_45 || IVLCamVariables.ImagingMode == Imaging.ImagingMode.Anterior_Prime)
                        {
                            motorMotionTimer.Interval = (camPropsHelper.Posterior2AnteriorOffsetMotorSteps * IVLCamVariables._Settings.BoardSettings.MotorPerStepTime + IVLCamVariables._Settings.BoardSettings.MotorStepOffsetTime);// time interval for motor timer time out in milliseconds always set at the time of capture

                            StartMotorMovement(true, Convert.ToByte(camPropsHelper.Posterior2AnteriorOffsetMotorSteps));// motor move from poterior to anterior steps
                            isResetMode = false;
                        }
                        else
                        {
                            isResetMode = false;
                            StartLive();
                            if (IVLCamVariables._eventHandler.isHandlerPresent(IVLCamVariables._eventHandler.UpdateMainWindowCursor))
                            {
                                arg["isDefault"] = true;
                                IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.UpdateMainWindowCursor, arg);
                            }
                        }
                        #endregion
                    }

                    else
                    {
                        StartLive();
                        //if (IVLCamVariables._eventHandler.isHandlerPresent(IVLCamVariables._eventHandler.UpdateMainWindowCursor))
                        //{
                        //    arg["isDefault"] = true;
                        //    IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.UpdateMainWindowCursor, arg);
                        //}

                    }
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }


        public bool StartEdgeDetection
        {
            set { 
                IVLCamVariables.EnableEdgeDetection = value; 
                
            }
        }
    }
}
