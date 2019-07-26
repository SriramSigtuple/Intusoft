using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using INTUSOFT.EventHandler;
using System.IO;
using NLog;
using NLog.Config;
using NLog.Targets;
using Common;
namespace INTUSOFT.Imaging
{
   

    /// <summary>
    /// IVLCamVariables is the static class to save the data variables of the Camera Module 
    /// </summary>
    static class IVLCamVariables
    {
        //private static readonly Logger captureLog = LogManager.GetLogger("INTUSOFT.Imaging.CaptureLog");

        public static string DisplayName;
        public static List<Args> CameraLogList;
        public static List<ImageCategory> imageCatList = new List<ImageCategory>();
        public static List<ImageCategoryHelper> imageCategoryValues = new List<ImageCategoryHelper>();

        /// <summary>
        ///List to save raw bytes of an image before demosaic
        /// </summary>

      
       
        /// <summary>
        /// Raw Image converted from demosaic from the sensor.
        /// </summary>
        //public static Bitmap RawImage;// 
        /// <summary>
        /// Bitmap to save IR image
        /// </summary>
        public static Bitmap IRImage;// 

        /// <summary>
        /// List to maintain byte arr of each of the eeprom
        /// </summary>
        public static List<byte[]> eepromByteArrList;

        /// <summary>
        /// Bitmap to obtained after demosaicing
        /// </summary>
        public static Bitmap ProcessedImage;// 

        /// <summary>
        /// bitmap to be used for overlayImage
        /// </summary>
        public static Exception2StringConverter exceptionConvertor;
        #region /////////////////////////////////  Logs used for the camera /////////////////////////////////////
        // public static ILog captureLog;

        /// <summary>
        /// The total events occuring in the  camera module
        /// </summary>
        /// <summary>
        /// Stream writer used to save the logs to the hard drive
        /// </summary>
        public static StreamWriter LogWriter;
        #endregion

        #region //////////////////////////////////////  Directory paths for saving images  /////////////////////////////////////////////
        /// <summary>
        /// Path to save the raw bytes/bitmaps before processing only for debug purpose
        /// </summary>
        public static string RawImageSaveDirPath;//
        /// <summary>
        /// Path to save the processed image which gets displayed.
        /// </summary>
        public static string ProcessedImageSaveDirPath;// 
        #endregion

        public static IVLEventHandler _eventHandler = IVLEventHandler.getInstance();//common event handler used communication throughout the camera module.

        #region///////////////////////////////////   States used for capturing   //////////////////////////////////////////////////
        /// <summary>
        /// State to decide whether the capture sequence was a failure . If true its a failure else image capture sequence is successful
        /// </summary>
        public static bool isCaptureFailure = false;
        /// <summary>
        /// State to indicate whether the capturing sequence is in progress
        /// </summary>
        public static bool isCapturing;

        public static bool isResuming;
        /// <summary>
        /// state to indicate the saving of frames to the list can be started via flash on done received
        /// </summary>
        public static bool IsFlashOnDone;

        /// <summary>
        /// State to indicate the camera is in live mode / pause(stand by ) mode
        /// </summary>
        public static bool isLive = false;

        /// <summary>
        /// state to indicate stop the saving of frames to the list can be started via flash off done received
        /// </summary>
        public static bool IsFlashOffDone = false;

        public static List<Args> logList = new List<Args>();
        private static bool isMotorMoved;

        /// <summary>
        /// state to indicate the motor movement is done
        /// </summary>
        public static bool IsMotorMoved
        {
            get { return IVLCamVariables.isMotorMoved; }
            set { 
                IVLCamVariables.isMotorMoved = value;
                if (isCapturing)
                {
                    Args logArg = new Args();
                    
                    logArg["TimeStamp"] = DateTime.Now;
                    logArg["Msg"] = "isMotor Moved status = " + isMotorMoved;
                    logList.Add(logArg);
                     //IVLCamVariables.CameraLogList.Add(logArg);
                    //captureLog.Info(string.Format("isMotor Moved status ={0}",isMotorMoved));
                    //IVLCamVariables.logClass.CaptureLogList.Add(string.Format("Time = {0},isMotor Moved status = {1}  ", DateTime.Now.ToString("HH-mm-ss-ffff"), isMotorMoved));
                    //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(string.Format("Time = {0},isMotor Moved status = {1}  ", DateTime.Now.ToString("HH-mm-ss-ffff"), isMotorMoved));

                }
            }
        }

        #endregion

        public static int CaptureImageIndx = -1;// Index of the captured frame in the list.
        //public static int FrameBetweenFlashOnFlashOffDoneCnt = 0;

        public static CapturedImageType captureImageType;// Enum to indicate the captured image type if it is GbGr it means to raw frames present else only one flash raw frame.
        public static int eyeSide = 0;// Indicates the eye side obtained from the left right sensor if 0 it means left 1 means right.
        public static CameraModel cameraModel;
        public static CaptureFailureCode captureFailureCode;
        public static ImagingMode ImagingMode;
        public static TriggerStatus TrigStatus = TriggerStatus.Off;
        public static LeftRightPosition leftRightPos;
        public static string ffaTimeStamp;
        public static DateTime FlashOnTime;
        public static DateTime FlashOffTime;
        public static byte StrobeValue = 100;
        public static long time = 0;
        public static bool isMonoChrome = false;
        public static bool isIRSaved = false;
        public static Devices isCameraConnected = Devices.CameraDisconnected;
        public static Devices isPowerConnected = Devices.PowerDisconnected;
        public static bool PowerCameraStatusUpdateInProgress = false;
        public static LogClass logClass;
        public static IntucamHelper intucamHelper;
        public static Camera ivl_Camera;
        public static bool isBlueFilteredMoved = false;
        public static string captureCameraSettings = string.Empty;
        public static string captureMaskSettings = string.Empty;
        /////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Indices to indicate the frame index of gb,gr/ flash frame in the saved list from capture sequence
        public static int GBIndx = 0;
        public static int GRIndx = 0;
        public static int FlashIndx = 0;

        #endregion


        #region Variables used to calculate the frame rate of the camera by the formula (FrameRate = CurrentFrameCnt - PreviousFrameCnt)//
        /// <summary>
        /// Count to indicate the number of frames arrived during current the second

        /// </summary>
        public static int CurrentFrameCnt = 0;
        /// <summary>
        /// Count to indicate the number of frames arrived during previous the second
        /// </summary>
        public static int PreviousFrameCnt = 0;
        #endregion

        public static CameraVendors CurrentCameraVendor = CameraVendors.Vendor1;
        public static string displayImage = "";

        /// <summary>
        /// State used to toggle between pcu knob to handle if true = flash intensity else false = exposure-gain table 
        /// </summary>
        public static bool isEnableFlashControlUsingKnob = false;

        public static string FFATime;
        public static int FFASeconds;

        public static string FirmwareVersion;
        public static string ImageName;
        public static string ImagePath;
        public static int MaxNegativeDiaptor = 600;
        public static int MaxPositiveDiaptor = 400;
        public static IntucamBoardCommHelper BoardHelper;
        public static Led ledSource;
        public static LedCode liveLedCode;
        public static LedCode captureLedCode;
        public static bool isApplicationClosing = false;
        public static System.Diagnostics.Stopwatch TimeTaken;// for time optimization
        public static PostProcessing PostProcessing; // To use single instance of postprocessing module throught out the camera
        public static CameraModuleSettings _Settings;// Single instance of Camera setting through out the camera module.
        public static bool EnableEdgeDetection;
        public static bool ImageSavingInProgress = false;
        public static bool isColor = true;
        public static int[,] motorOffSetMatrix = new int[3, 3];

        public static bool isContinuousCapture = false;
        private static int rotaryUpdateValues = 0;

        public static int RotaryUpdateValues
        {
            get { return IVLCamVariables.rotaryUpdateValues; }
            set {
                IVLCamVariables.rotaryUpdateValues = value;
                if(IVLCamVariables.intucamHelper != null)
                IVLCamVariables.intucamHelper.motorSensorPositionEvent(IVLCamVariables.rotaryUpdateValues, IVLCamVariables.rotaryUpdateValues + IVLCamVariables.rotaryZeroDOffsetValue);
            
            }
        }
        public static int rotaryZeroDOffsetValue = 0;
        public static bool isAppLaunched = false;
    }
}
