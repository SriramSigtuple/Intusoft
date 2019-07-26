using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTUSOFT.Data.Model;
using INTUSOFT.Data.Repository;
using WindowsFormsApplication1;
using System.Globalization;
using System.Resources;

namespace DBPorting
{
    public static class IVLVariables
    {
        public static bool ShowImagingBtn = false;
        public static bool showSplash = false;
        public static bool isdelete_thumbnail = false;
        public static bool iscolorChange = false;
        public static int mrnCnt = 0;
        public static int imageWidth = 2048;
        public static int imageHeight = 1536;
        public static bool isZoomEnabled = false;
        public static string backupConfigFile = "IVLConfigBackUp.xml";
        public static float zoomFactor = 1.0f;
        public static bool isCapturing = false;
        public static float zoomMax = 10.0f;
        public static float zoomMin = 1.0f;
        public static bool isShiftKey = false;
        public static bool isControlKey = false;
        private static int _ActivePatID = 0;
        public static ResourceManager LangResourceManager;    // declare Resource manager to access to specific cultureinfo
        public static CultureInfo LangResourceCultureInfo;            //declare culture info
        public static int BoardCommandIterationCnt = 3;
        public static bool isCameraOpen;
        public static int KnobDifferenceValue;
        public static bool isAnotherWindowOpen = false;
        public static bool CanAddImages = false;
        public static bool isbrightness = false;
        public static string patName = "";
        public static int patAge = 3;
        public static bool isone_onlycorrupted = false;
        public static string patGender = "";
        public static bool isCameraConnected = false;
        public static String MRN = "";
        public static String Operator="DR.Anil";
        public static string saveImageDirectory = @"IVLImageRepo\Images";
        public static bool isLiveImaging = false;
        public static bool isPosterior = true;
        public static bool isOldFirmware = false;
        public static IVLConfig _ivlConfig;
        public static List<Exception> ExceptionList;
        public static long serverTimerTaken = 0;

    }

    public static class IVLMethods
    {
        //public static void CheckCameraConnection()
        //{
        //    IVLVariables.isCameraConnected = IntucamHelper.isCameraConnected();
        //     if(!IVLVariables.isCapturing && IVLVariables.isCameraOpen && IntucamBoardCommHelper.isOpen)
        //     {
        //         IntucamBoardCommHelper.CameraConnected(IVLVariables.BoardCommandIterationCnt);
        //         IVLVariables.isCameraConnected = IVLVariables.isCameraConnected  && IntucamBoardCommHelper.isCameraConnected && IntucamBoardCommHelper.isStrobeConnected;
        //     }
        //}
    }
}
