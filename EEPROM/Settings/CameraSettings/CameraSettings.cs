using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EEPROM
{
    [Serializable]
    public class CameraSettings
    {
        private EEPROM_Props cameraModeSettings = null;

        public EEPROM_Props CameraModeSettings
        {
            get { return cameraModeSettings; }
            set { cameraModeSettings = value; }
        }

        private EEPROM_Props gainExposureSettings = null;

        public EEPROM_Props GainExposureSettings
        {
            get { return gainExposureSettings; }
            set { gainExposureSettings = value; }
        }

        private EEPROM_Props temperatureTintSettings;

        public EEPROM_Props TemperatureTintSettings
        {
            get { return temperatureTintSettings; }
            set { temperatureTintSettings = value; }
        }
        public CameraSettings()
        {
            GainExposureSettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Tree, StrutureTypes.ExposureGainSettings.ToByte<StrutureTypes>()), "");
            GainExposureSettings.value = new EEPROM.ExposureGainSettings();

            CameraModeSettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Tree, StrutureTypes.CameraModeSettings.ToByte<StrutureTypes>()), "");
            CameraModeSettings.value = new EEPROM.CameraModeSettings();

            TemperatureTintSettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Tree, StrutureTypes.TemperatureTintSettings.ToByte<StrutureTypes>()), "");
            TemperatureTintSettings.value = new EEPROM.TemperatureTintSettings();

        }
    }


    [Serializable]
    public class CameraModeSettings
    {

        private EEPROM_Props saveFramesCount = null;

        public EEPROM_Props SaveFramesCount
        {
            get { return saveFramesCount; }
            set { saveFramesCount = value; }
        }

        private EEPROM_Props enable14Bit = null;

        public EEPROM_Props Enable14Bit
        {
            get { return enable14Bit; }
            set { enable14Bit = value; }
        }

        private EEPROM_Props frameDetectionValue = null;

        public EEPROM_Props FrameDetectionValue
        {
            get { return frameDetectionValue; }
            set { frameDetectionValue = value; }
        }
        private EEPROM_Props darkFrameDetectionValue = null;

        public EEPROM_Props DarkFrameDetectionValue
        {
            get { return darkFrameDetectionValue; }
            set { darkFrameDetectionValue = value; }
        }

        private EEPROM_Props cameraModel = null;

        public EEPROM_Props CameraModel
        {
            get { return cameraModel; }
            set { cameraModel = value; }
        }



        private EEPROM_Props enableVerticalFlip = null;

        public EEPROM_Props EnableVerticalFlip
        {
            get { return enableVerticalFlip; }
            set { enableVerticalFlip = value; }
        }

        private EEPROM_Props enableHorizontalFlip = null;

        public EEPROM_Props EnableHorizontalFlip
        {
            get { return enableHorizontalFlip; }
            set { enableHorizontalFlip = value; }
        }

        private EEPROM_Props enableRawMode = null;

        public EEPROM_Props EnableRawMode
        {
            get { return enableRawMode; }
            set { enableRawMode = value; }
        }


        public CameraModeSettings()
        {

            EEPROM_Data_Types<byte> cameraModel = new EEPROM_Data_Types<byte>(1, 1, byte.MaxValue);

            CameraModel = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), cameraModel, "A,B,C,D");

            EEPROM_Data_Types<byte> saveFrameCnt = new EEPROM_Data_Types<byte>(8, 1, byte.MaxValue);

            SaveFramesCount = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), saveFrameCnt);

            EEPROM_Data_Types<byte> darkFrameDetectVal = new EEPROM_Data_Types<byte>(100, 1, byte.MaxValue);

            DarkFrameDetectionValue = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), darkFrameDetectVal);

            EEPROM_Data_Types<byte> enableRawMode = new EEPROM_Data_Types<byte>(1, 0, 1);

            EnableRawMode = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), enableRawMode);

            EEPROM_Data_Types<byte> enable14Bit = new EEPROM_Data_Types<byte>(1, 0, 1);

            Enable14Bit = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), enable14Bit);

            EEPROM_Data_Types<byte> enableVerticalFlip = new EEPROM_Data_Types<byte>(1, 0, 1);

            EnableVerticalFlip = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), enableVerticalFlip);

            EEPROM_Data_Types<byte> enableHorizontalFlip = new EEPROM_Data_Types<byte>(1, 0, 1);

            EnableHorizontalFlip = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), enableHorizontalFlip);




            EEPROM_Data_Types<float> frameDetectVal = new EEPROM_Data_Types<float>(2.5f, 0f, 100f);

            FrameDetectionValue = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Single, 4), frameDetectVal);



        }
    }

    [Serializable]
    public class TemperatureTintSettings
    {


        private EEPROM_Props liveEnableWB = null;

        public EEPROM_Props LiveEnableWB
        {
            get { return liveEnableWB; }
            set { liveEnableWB = value; }
        }

        private EEPROM_Props liveEnableCC = null;

        public EEPROM_Props LiveEnableCC
        {
            get { return liveEnableCC; }
            set { liveEnableCC = value; }
        }
        private EEPROM_Props captureEnableCC = null;

        public EEPROM_Props CaptureEnableCC
        {
            get { return captureEnableCC; }
            set { captureEnableCC = value; }
        }
        private EEPROM_Props captureEnableWB = null;

        public EEPROM_Props CaptureEnableWB
        {
            get { return captureEnableWB; }
            set { captureEnableWB = value; }
        }


        private EEPROM_Props liveTemperature = null;

        public EEPROM_Props LiveTemperature
        {
            get { return liveTemperature; }
            set { liveTemperature = value; }
        }
        private EEPROM_Props liveTint = null;

        public EEPROM_Props LiveTint
        {
            get { return liveTint; }
            set { liveTint = value; }
        }
        private EEPROM_Props captureTemperature = null;

        public EEPROM_Props CaptureTemperature
        {
            get { return captureTemperature; }
            set { captureTemperature = value; }
        }
        private EEPROM_Props captureTint = null;

        public EEPROM_Props CaptureTint
        {
            get { return captureTint; }
            set { captureTint = value; }
        }



        public TemperatureTintSettings()
        {

           

            EEPROM_Data_Types<byte> captureEnableCC = new EEPROM_Data_Types<byte>(1, 0, 1);

            CaptureEnableCC = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), captureEnableCC);

            EEPROM_Data_Types<byte> captureEnableWB = new EEPROM_Data_Types<byte>(1, 0, 1);

            CaptureEnableWB = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), captureEnableWB);

            EEPROM_Data_Types<byte> liveEnableCC = new EEPROM_Data_Types<byte>(1, 0, 1);

            LiveEnableCC = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), liveEnableCC);

            EEPROM_Data_Types<byte> liveEnableWB = new EEPROM_Data_Types<byte>(1, 0, 1);

            LiveEnableWB = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), liveEnableWB);

            EEPROM_Data_Types<Int16> liveTint = new EEPROM_Data_Types<Int16>(1000, 1, 2500);


            LiveTint = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), liveTint);

            EEPROM_Data_Types<Int16> captureTint = new EEPROM_Data_Types<Int16>(1000, 1, 2500);

            CaptureTint = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), captureTint);

            EEPROM_Data_Types<Int16> liveTemp = new EEPROM_Data_Types<Int16>(5500, 1, 15000);

            LiveTemperature = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), liveTemp);

            EEPROM_Data_Types<Int16> captureTemp = new EEPROM_Data_Types<Int16>(5500, 1, 15000);

            CaptureTemperature = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), captureTemp);




        }
    }

    [Serializable]
    public class ExposureGainSettings
    {
      
     
       private EEPROM_Props liveGainDefault = null;

        public EEPROM_Props LiveGainDefault
        {
            get { return liveGainDefault; }
            set { liveGainDefault = value; }
        }
        private  EEPROM_Props liveGainLow = null;

        public EEPROM_Props LiveGainLow
        {
            get { return liveGainLow; }
            set { liveGainLow = value; }
        }

        private  EEPROM_Props liveGainMedium = null;

        public EEPROM_Props LiveGainMedium
        {
            get { return liveGainMedium; }
            set { liveGainMedium = value; }
        }

        private  EEPROM_Props liveGainHigh = null;

        public EEPROM_Props LiveGainHigh
        {
            get { return liveGainHigh; }
            set { liveGainHigh = value; }
        }

        private EEPROM_Props captureGainDefault = null;

        public EEPROM_Props CaptureGainDefault
        {
            get { return captureGainDefault; }
            set { captureGainDefault = value; }
        }

        private  EEPROM_Props captureGainLow = null;

        public EEPROM_Props CaptureGainLow
        {
            get { return captureGainLow; }
            set { captureGainLow = value; }
        }

        private  EEPROM_Props captureGainMedium = null;

        public EEPROM_Props CaptureGainMedium
        {
            get { return captureGainMedium; }
            set { captureGainMedium = value; }
        }

        private  EEPROM_Props captureGainHigh = null;

        public EEPROM_Props CaptureGainHigh
        {
            get { return captureGainHigh; }
            set { captureGainHigh = value; }
        }



        private  EEPROM_Props captureGain = null;

        public EEPROM_Props CaptureGain
        {
            get { return captureGain; }
            set { captureGain = value; }
        }
         private  EEPROM_Props liveGain = null;

        public EEPROM_Props LiveGain
        {
            get { return liveGain; }
            set { liveGain = value; }
        }
        private  EEPROM_Props liveExposure = null;

        public EEPROM_Props LiveExposure
        {
            get { return liveExposure; }
            set { liveExposure = value; }
        }
        private  EEPROM_Props captureExposure = null;

        public EEPROM_Props CaptureExposure
        {
            get { return captureExposure; }
            set { captureExposure = value; }
        }

        public ExposureGainSettings()
        {

            EEPROM_Data_Types<Int16> liveGainLow = new EEPROM_Data_Types<Int16>(100, 100, 5000);

            LiveGainLow = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), liveGainLow);

            EEPROM_Data_Types<Int16> liveGainMed = new EEPROM_Data_Types<Int16>(100, 100, 5000);

            LiveGainMedium = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), liveGainMed);

            EEPROM_Data_Types<Int16> liveGainHigh = new EEPROM_Data_Types<Int16>(300, 100, 5000);

            LiveGainHigh = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), liveGainHigh);

            EEPROM_Data_Types<Int16> captureGainLow = new EEPROM_Data_Types<Int16>(100, 100, 5000);

            CaptureGainLow = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), captureGainLow);

            EEPROM_Data_Types<Int16> captureGainMed = new EEPROM_Data_Types<Int16>(200, 100, 5000);

            CaptureGainMedium = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), captureGainMed);

            EEPROM_Data_Types<Int16> captureGainHigh = new EEPROM_Data_Types<Int16>(300, 100, 5000);

            CaptureGainHigh = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), captureGainHigh);

            EEPROM_Data_Types<Int16> captureGain = new EEPROM_Data_Types<Int16>(100, 100, 5000);

            CaptureGain = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), captureGain);

            EEPROM_Data_Types<Int16> liveGain = new EEPROM_Data_Types<Int16>(100, 100, 5000);

            LiveGain = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), liveGain);

            EEPROM_Data_Types<Int32> liveExposure = new EEPROM_Data_Types<Int32>(77376, 2500, 300000);
            LiveExposure = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int32, 4), liveExposure);

            EEPROM_Data_Types<Int32> captureExposure = new EEPROM_Data_Types<Int32>(77376, 2500, 300000);
            CaptureExposure = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int32, 4), captureExposure);

            EEPROM_Data_Types<Int16> liveGainDefault = new EEPROM_Data_Types<Int16>(1, 1, 10);
            LiveGainDefault = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 4), liveGainDefault);

            EEPROM_Data_Types<byte> captureGainDefault = new EEPROM_Data_Types<byte>(1, 1, 10);
            CaptureGainDefault = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), captureGainDefault);
        }
        }
    }