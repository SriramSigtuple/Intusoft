using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTUSOFT.Imaging;
using INTUSOFT.Data.Repository;
using INTUSOFT.Desktop;
using System.Globalization;
using System.Resources;
using Common;
using INTUSOFT.Configuration;
namespace INTUSOFT.Desktop
{
        public enum GainLevels { Low, Medium, High };

    public static class IVLVariables
    {
        
        public static bool ShowImagingBtn = false;
        public static bool showSplash = false;
        public static bool isdelete_thumbnail = false;
        public static bool iscolorChange = false;
        public static int mrnCnt = 0;
        public static int imageWidth = 2048;
        public static int imageHeight = 1536;
        public static bool enableOverlay = true;
        public static bool isZoomEnabled = false;
        public static float zoomFactor = 1.0f;
        public static bool isCapturing = false;
        public static float zoomMax = 10.0f;
        public static float zoomMin = 1.0f;
        public static bool isShiftKey = false;
        public static bool isControlKey = false;
        private static int _ActivePatID = 0;
        public static ResourceManager LangResourceManager;    // declare Resource manager to access to specific cultureinfo
        public static CultureInfo LangResourceCultureInfo;            //declare culture info
        public static Exception2StringConverter ExceptionLog;
        public static string appDirPathName = string.Empty;

        public static Settings CurrentSettings
        {
            get { return  ConfigVariables.CurrentSettings; }
            set { ConfigVariables.CurrentSettings = value; }
        }
        public static int ActivePatID
        {
            get { return IVLVariables._ActivePatID; }
            set { IVLVariables._ActivePatID = value; }
        }
        private static int _ActiveVisitID = 0;

       public static int ActiveVisitID
        {
            get { return IVLVariables._ActiveVisitID; }
            set { IVLVariables._ActiveVisitID = value; }
        }
        private static int _ActiveImageID = 0;

        public static int ActiveImageID
        {
            get { return IVLVariables._ActiveImageID; }
            set { IVLVariables._ActiveImageID = value; }
        }
        private static bool isAnotherWindowOpen = false;
        public static bool isReportWindowOpen = false;
        public static bool CanAddImages = false;
        public static bool isValueChanged = false;
        public static string patName = "";
        public static int patAge = 3;
        public static bool isone_onlycorrupted = false;
        public static string patGender = "";
        public static String MRN = "";
        public static String Operator="DR.Anil";
        public static bool isPosterior = true;
        public static string saveImageDirectory = @"C:\IVLImageRepo\Images";
        
        public static  IVLConfig _ivlConfig;
        public static List<Exception> ExceptionList;
        public static long serverTimerTaken = 0;
        public static IntucamHelper ivl_Camera;
        public static bool istrigger = false;
        public static bool ApplicationClosing = false;
        public static PostProcessing postprocessingHelper;
        public static GainLevels CurrentLiveGain = GainLevels.Low;
        public static GainLevels CurrentCaptureGain = GainLevels.Low;
        public static bool isDefaultAnteriorGain = true; // State to take the value from defualt gain value in case of anterior if true the default value is set else current value is set for both live and capture
        public static bool isDefaultPrimeGain = true;// State to take the value from defualt gain value in case of prime if true the default value is set else current value is set for both live and capture
        public static bool isDefaultPrimeLedSource = true;// State to take the value from defualt gain value in case of prime if true the default value is set else current value is set for both live and capture
        public static bool isDefaultAnteriorLedSource = true;// State to take the value from defualt gain value in case of prime if true the default value is set else current value is set for both live and capture
        public static bool isDefault45LedSource = true;// State to take the value from defualt gain value in case of prime if true the default value is set else current value is set for both live and capture
        public static bool isDefaultFFAPlusLedSource = true;// State to take the value from defualt gain value in case of prime if true the default value is set else current value is set for both live and capture
        public static bool isCommandLineArgsPresent = false;
        public static bool isCommandLineAppLaunch = false;
        public static string CmdImageSavePath = "";
        //The below static variables are added by Darshan on 02-09-2015 to maintain a single instance of json repositories throght the application.
        public static string batchFilePath = "";
        public static int CmdObsID = 1;
        public static Common.PatientDetailsForCommandLineArgs patDetails;
        public static Common.ValidatorDatas.EmailsData mailData;
        public static Dictionary<string, string> observationDic;
        public static Common.Validators.FileNameFolderPathValidator FileFolderValidator;
        public static DateTime currentVisitDateTime;

        /// <summary>
        /// This property will set value for variable isAnotherWindowOpen and triggerOn and Off.
        /// </summary>
        public static bool IsAnotherWindowOpen
        {
            set
            {
                isAnotherWindowOpen = value;
                if(isAnotherWindowOpen && ivl_Camera!=null)
                    ivl_Camera.TriggerOff();
                else if(!isAnotherWindowOpen && ivl_Camera != null && currentVisitDateTime.Date == DateTime.Now.Date)
                    ivl_Camera.TriggerOn();
            }
            get
            {
                return isAnotherWindowOpen;
            }
        }
        private static GradientColor gradientColorValues;

        public static GradientColor GradientColorValues
        {
            get { return IVLVariables.gradientColorValues; }
            set { IVLVariables.gradientColorValues = value; }
        }
        public static Themes IVLThemes;
    }

    
}
