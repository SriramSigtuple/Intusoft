using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace INTUSOFT.Desktop.AdvancedSettings
{
    [Serializable]
    public class CameraSettings
    {
        //public int SaveFramesCount = 8;
        //public bool isIrSave = true;
        //public bool isRawSave = true;
        //public string CameraModel = "A";
        //public float FrameDetectionValue = 2.5f;
        //public int DarkFrameDetectionValue = 100;
        //public int ImageWidth = 2048;
        //public int ImageHeight = 1536;
        //public int ResumeLiveCnt = 3;
        //public int temperature = 6500;
        //public int tint = 1000;
        //public int presetGain = 180;
        //public int digitalGain = 100;
        //public int LiveDigitalGain = 100;
        //public int Exposure = 50000;
        //public int FlashExposure = 50000;
        //public int Saturation = 110;
        //public bool ShowCaptureFailure = true;
        //public int ImageOpticalCentreX = 1060;
        //public int ImageOpticalCentreY = 750;


        private  IVLControlProperties SaveFramesCount = null;

        public IVLControlProperties _SaveFramesCount
        {
            get { return SaveFramesCount; }
            set { SaveFramesCount = value; }
        }

        private  IVLControlProperties Enable14Bit = null;

        public IVLControlProperties _Enable14Bit
        {
            get { return Enable14Bit; }
            set { Enable14Bit = value; }
        }

        private IVLControlProperties EnableWB = null;

        public IVLControlProperties _EnableWB
        {
            get { return EnableWB; }
            set { EnableWB = value; }
        }
        private  IVLControlProperties FrameDetectionValue = null;

        public IVLControlProperties _FrameDetectionValue
        {
            get { return FrameDetectionValue; }
            set { FrameDetectionValue = value; }
        }
        private  IVLControlProperties DarkFrameDetectionValue = null;

        public IVLControlProperties _DarkFrameDetectionValue
        {
            get { return DarkFrameDetectionValue; }
            set { DarkFrameDetectionValue = value; }
        }

        private  IVLControlProperties CameraModel = null;

        public IVLControlProperties _CameraModel
        {
            get { return CameraModel; }
            set { CameraModel = value; }
        }

        private  IVLControlProperties ImageWidth = null;

        public IVLControlProperties _ImageWidth
        {
            get { return ImageWidth; }
            set { ImageWidth = value; }
        }
        private  IVLControlProperties ImageHeight = null;

        public IVLControlProperties _ImageHeight
        {
            get { return ImageHeight; }
            set { ImageHeight = value; }
        }
        private IVLControlProperties IRTemperature = null;

        public IVLControlProperties _IRTemperature
        {
            get { return IRTemperature; }
            set { IRTemperature = value; }
        }
        private IVLControlProperties IRTint = null;

        public IVLControlProperties _IRTint
        {
            get { return IRTint; }
            set { IRTint = value; }
        }
        private IVLControlProperties FlashTemperature = null;

        public IVLControlProperties _FlashTemperature
        {
            get { return FlashTemperature; }
            set { FlashTemperature = value; }
        }
        private IVLControlProperties FlashTint = null;

        public IVLControlProperties _FlashTint
        {
            get { return FlashTint; }
            set { FlashTint = value; }
        }

        private IVLControlProperties LiveGainDefault = null;

        public IVLControlProperties _LiveGainDefault
        {
            get { return LiveGainDefault; }
            set { LiveGainDefault = value; }
        }
        private  IVLControlProperties LiveGainLow = null;

        public IVLControlProperties _LiveGainLow
        {
            get { return LiveGainLow; }
            set { LiveGainLow = value; }
        }

        private  IVLControlProperties LiveGainMedium = null;

        public IVLControlProperties _LiveGainMedium
        {
            get { return LiveGainMedium; }
            set { LiveGainMedium = value; }
        }

        private  IVLControlProperties LiveGainHigh = null;

        public IVLControlProperties _LiveGainHigh
        {
            get { return LiveGainHigh; }
            set { LiveGainHigh = value; }
        }

        private IVLControlProperties DigitalGainDefault = null;

        public IVLControlProperties _DigitalGainDefault
        {
            get { return DigitalGainDefault; }
            set { DigitalGainDefault = value; }
        }

        private  IVLControlProperties DigitalGainLow = null;

        public IVLControlProperties _DigitalGainLow
        {
            get { return DigitalGainLow; }
            set { DigitalGainLow = value; }
        }

        private  IVLControlProperties DigitalGainMedium = null;

        public IVLControlProperties _DigitalGainMedium
        {
            get { return DigitalGainMedium; }
            set { DigitalGainMedium = value; }
        }

        private  IVLControlProperties DigitalGainHigh = null;

        public IVLControlProperties _DigitalGainHigh
        {
            get { return DigitalGainHigh; }
            set { DigitalGainHigh = value; }
        }
        private  IVLControlProperties ResumeLiveCnt = null;

        public IVLControlProperties _ResumeLiveCnt
        {
            get { return ResumeLiveCnt; }
            set { ResumeLiveCnt = value; }
        }
        private  IVLControlProperties ResumeLabelVisible = null;

        public IVLControlProperties _ResumeLabelVisible
        {
            get { return ResumeLabelVisible; }
            set { ResumeLabelVisible = value; }
        }

        private  IVLControlProperties MotorPositiveColor = null;

        public IVLControlProperties _MotorPositiveColor
        {
            get { return MotorPositiveColor; }
            set { MotorPositiveColor = value; }
        }

        private  IVLControlProperties MotorNegativeColor = null;

        public IVLControlProperties _MotorNegativeColor
        {
            get { return MotorNegativeColor; }
            set { MotorNegativeColor = value; }
        }

        private  IVLControlProperties temperature = null;

        public IVLControlProperties _Temperature
        {
            get { return temperature; }
            set { temperature = value; }
        }
        private  IVLControlProperties tint = null;

        public IVLControlProperties _Tint
        {
            get { return tint; }
            set { tint = value; }
        }

        private  IVLControlProperties presetGain = null;

        public IVLControlProperties _presetGain
        {
            get { return presetGain; }
            set { presetGain = value; }
        }

        private  IVLControlProperties digitalGain = null;

        public IVLControlProperties _DigitalGain
        {
            get { return digitalGain; }
            set { digitalGain = value; }
        }
        private  IVLControlProperties ImageOpticalCentreX = null;

        public IVLControlProperties _ImageOpticalCentreX
        {
            get { return ImageOpticalCentreX; }
            set { ImageOpticalCentreX = value; }
        }
        private  IVLControlProperties EnableVerticalFlip = null;

        public IVLControlProperties _EnableVerticalFlip
        {
            get { return EnableVerticalFlip; }
            set { EnableVerticalFlip = value; }
        }

        private  IVLControlProperties EnableHorizontalFlip = null;

        public IVLControlProperties _EnableHorizontalFlip
        {
            get { return EnableHorizontalFlip; }
            set { EnableHorizontalFlip = value; }
        }

        private  IVLControlProperties EnableRawMode = null;

        public IVLControlProperties _EnableRawMode
        {
            get { return EnableRawMode; }
            set { EnableRawMode = value; }
        }
        private  IVLControlProperties LiveDigitalGain = null;

        public IVLControlProperties _LiveDigitalGain
        {
            get { return LiveDigitalGain; }
            set { LiveDigitalGain = value; }
        }
        private  IVLControlProperties Exposure = null;

        public IVLControlProperties _Exposure
        {
            get { return Exposure; }
            set { Exposure = value; }
        }
        private  IVLControlProperties FlashExposure = null;

        public IVLControlProperties _FlashExposure
        {
            get { return FlashExposure; }
            set { FlashExposure = value; }
        }
        private  IVLControlProperties Saturation = null;

        public IVLControlProperties _Saturation
        {
            get { return Saturation; }
            set { Saturation = value; }
        }
        private  IVLControlProperties ShowCaptureFailure = null;

        public IVLControlProperties _ShowCaptureFailure
        {
            get { return ShowCaptureFailure; }
            set { ShowCaptureFailure = value; }
        }

        private  IVLControlProperties ImageOpticalCentreY = null;

        public IVLControlProperties _ImageOpticalCentreY
        {
            get { return ImageOpticalCentreY; }
            set { ImageOpticalCentreY = value; }
        }

        private IVLControlProperties ImageROIY = null;

        public IVLControlProperties _ImageROIY
        {
            get { return ImageROIY; }
            set { ImageROIY = value; }
        }

        private IVLControlProperties ImageROIX = null;

        public IVLControlProperties _ImageROIX
        {
            get { return ImageROIX; }
            set { ImageROIX = value; }
        }

        public CameraSettings()
        {
            _CameraModel = new IVLControlProperties();
            _CameraModel.name = "CameraModel";
            _CameraModel.type = "string";
            _CameraModel.val = "A";
            _CameraModel.control = "System.Windows.Forms.ComboBox";
            _CameraModel.text = "Camera Model";
            _CameraModel.range = "A,B,C,D";


            _SaveFramesCount = new IVLControlProperties();
            _SaveFramesCount.name = "SaveFramesCount";
            _SaveFramesCount.val = "8";
            _SaveFramesCount.type = "int";
            _SaveFramesCount.control = "System.Windows.Forms.NumericUpDown";
            _SaveFramesCount.text = "Save Frames Count";
            _SaveFramesCount.min = 1;
            _SaveFramesCount.max = 30;


            _DarkFrameDetectionValue = new IVLControlProperties();
            _DarkFrameDetectionValue.name = "DarkFrameDetectionValue";
            _DarkFrameDetectionValue.val = "100";
            _DarkFrameDetectionValue.type = "int";
            _DarkFrameDetectionValue.control = "System.Windows.Forms.NumericUpDown";
            _DarkFrameDetectionValue.text = "Dark FrameDetection Value";
            _DarkFrameDetectionValue.min = 1;
            _DarkFrameDetectionValue.max = 255;

            _MotorPositiveColor = new IVLControlProperties();
            _MotorPositiveColor.name = "_MotorPositiveColor";
            _MotorPositiveColor.type = "string";
            _MotorPositiveColor.val = "Blue";
            _MotorPositiveColor.control = "System.Windows.Forms.ComboBox";
            _MotorPositiveColor.text = "Motor Positive Color";
            _MotorPositiveColor.range = "Aqua,Blue,Black,Cyan,Red,DarkBlue,Green,Grey,Khaki,LimeGreen,YellowGreen";

            _MotorNegativeColor = new IVLControlProperties();
            _MotorNegativeColor.name = "_MotorNegativeColor";
            _MotorNegativeColor.type = "string";
            _MotorNegativeColor.val = "YellowGreen";
            _MotorNegativeColor.control = "System.Windows.Forms.ComboBox";
            _MotorNegativeColor.text = "Motor Negative Color";
            _MotorNegativeColor.range = "Aqua,Blue,Black,Cyan,Red,DarkBlue,Green,Grey,Khaki,LimeGreen,YellowGreen";

            _ImageWidth = new IVLControlProperties();
            _ImageWidth.name = "ImageWidth";
            _ImageWidth.val = "3072";
            _ImageWidth.type = "int";
            _ImageWidth.control = "System.Windows.Forms.NumericUpDown";
            _ImageWidth.text = "Image Width";
            _ImageWidth.min = 1;
            _ImageWidth.max = 10000;

            _ImageHeight = new IVLControlProperties();
            _ImageHeight.name = "ImageHeight";
            _ImageHeight.val = "2048";
            _ImageHeight.type = "int";
            _ImageHeight.control = "System.Windows.Forms.NumericUpDown";
            _ImageHeight.text = "Image Height";
            _ImageHeight.min = 1;
            _ImageHeight.max = 10000;

            _ResumeLiveCnt = new IVLControlProperties();
            _ResumeLiveCnt.name = "ResumeLiveCnt";
            _ResumeLiveCnt.val = "3";
            _ResumeLiveCnt.type = "int";
            _ResumeLiveCnt.control = "System.Windows.Forms.NumericUpDown";
            _ResumeLiveCnt.text = "Resume Live Count";
            _ResumeLiveCnt.min = 1;
            _ResumeLiveCnt.max = 10;

            _Temperature = new IVLControlProperties();
            _Temperature.name = "Temperature";
            _Temperature.val = "6500";
            _Temperature.type = "int";
            _Temperature.control = "System.Windows.Forms.NumericUpDown";
            _Temperature.text = "Temperature"; ;
            _Temperature.min = 2500;
            _Temperature.max = 15000;

            _Tint = new IVLControlProperties();
            _Tint.name = "tint";
            _Tint.val = "1000";
            _Tint.type = "int";
            _Tint.control = "System.Windows.Forms.NumericUpDown";
            _Tint.text = "Tint";
            _Tint.min = 650;
            _Tint.max = 5000;

            _presetGain = new IVLControlProperties();
            _presetGain.name = "presetGain";
            _presetGain.val = "180";
            _presetGain.type = "int";
            _presetGain.control = "System.Windows.Forms.NumericUpDown";
            _presetGain.text = "Preset Gain";
            _presetGain.min = 1;
            _presetGain.max = 5000;

            _DigitalGain = new IVLControlProperties();
            _DigitalGain.name = "digitalGain";
            _DigitalGain.val = "100";
            _DigitalGain.type = "int";
            _DigitalGain.control = "System.Windows.Forms.NumericUpDown";
            _DigitalGain.text = "Digital Gain";
            _DigitalGain.min = 1;
            _DigitalGain.max = 5000;

            _LiveDigitalGain = new IVLControlProperties();
            _LiveDigitalGain.name = "LiveDigitalGain";
            _LiveDigitalGain.val = "100";
            _LiveDigitalGain.type = "int";
            _LiveDigitalGain.control = "System.Windows.Forms.NumericUpDown";
            _LiveDigitalGain.text = "Live Digital Gain";
            _LiveDigitalGain.min = 1;
            _LiveDigitalGain.max = 5000;

            _Exposure = new IVLControlProperties();
            _Exposure.name = "Exposure";
            _Exposure.val = "100000";
            _Exposure.type = "int";
            _Exposure.control = "System.Windows.Forms.NumericUpDown";
            _Exposure.text = "Exposure";
            _Exposure.min = 2500;
            _Exposure.max = 300000;

            _FlashExposure = new IVLControlProperties();
            _FlashExposure.name = "FlashExposure";
            _FlashExposure.val = "100000";
            _FlashExposure.type = "int";
            _FlashExposure.control = "System.Windows.Forms.NumericUpDown";
            _FlashExposure.text = "Flash Exposure";
            _FlashExposure.min = 2500;
            _FlashExposure.max = 300000;

            _Saturation = new IVLControlProperties();
            _Saturation.name = "Saturation";
            _Saturation.val = "110";
            _Saturation.type = "int";
            _Saturation.control = "System.Windows.Forms.NumericUpDown";
            _Saturation.text = "Saturation";


            _ImageOpticalCentreX = new IVLControlProperties();
            _ImageOpticalCentreX.name = "ImageOpticalCentreX";
            _ImageOpticalCentreX.val = "1266";
            _ImageOpticalCentreX.type = "int";
            _ImageOpticalCentreX.control = "System.Windows.Forms.NumericUpDown";
            _ImageOpticalCentreX.text = "Image Optical CentreX";
            _ImageOpticalCentreX.min = 1;
            _ImageOpticalCentreX.max = 10000;

            _ImageOpticalCentreY = new IVLControlProperties();
            _ImageOpticalCentreY.name = "ImageOpticalCentreY";
            _ImageOpticalCentreY.val = "1024";
            _ImageOpticalCentreY.type = "int";
            _ImageOpticalCentreY.control = "System.Windows.Forms.NumericUpDown";
            _ImageOpticalCentreY.text = "Image Optical CentreY";
            _ImageOpticalCentreY.min = 1;
            _ImageOpticalCentreY.max = 10000;

            _ImageROIY = new IVLControlProperties();
            _ImageROIY.name = "ImageROIY";
            _ImageROIY.val = "0";
            _ImageROIY.type = "int";
            _ImageROIY.control = "System.Windows.Forms.NumericUpDown";
            _ImageROIY.text = "Image ROI Y";
            _ImageROIY.min = 0;
            _ImageROIY.max = 10000;

            _ImageROIX = new IVLControlProperties();
            _ImageROIX.name = "ImageROIX";
            _ImageROIX.val = "270";
            _ImageROIX.type = "int";
            _ImageROIX.control = "System.Windows.Forms.NumericUpDown";
            _ImageROIX.text = "Image ROI X";
            _ImageROIX.min = 1;
            _ImageROIX.max = 10000;

            _LiveGainDefault = new IVLControlProperties();
            _LiveGainDefault.name = "LiveGainDefault";
            _LiveGainDefault.val = "Low";
            _LiveGainDefault.type = "int";
            _LiveGainDefault.control = "System.Windows.Forms.ComboBox";
            _LiveGainDefault.text = " Live Gain Default";
            _LiveGainDefault.range = "Low,Medium,High";

            _LiveGainLow = new IVLControlProperties();
            _LiveGainLow.name = "LiveGainLow";
            _LiveGainLow.val = "100";
            _LiveGainLow.type = "int";
            _LiveGainLow.control = "System.Windows.Forms.NumericUpDown";
            _LiveGainLow.text = " Live Low Gain";
            _LiveGainLow.min = 100;
            _LiveGainLow.max = 5000;

            _LiveGainMedium = new IVLControlProperties();
            _LiveGainMedium.name = "LiveGainMedium";
            _LiveGainMedium.val = "300";
            _LiveGainMedium.type = "int";
            _LiveGainMedium.control = "System.Windows.Forms.NumericUpDown";
            _LiveGainMedium.text = "Live Medium Gain";
            _LiveGainMedium.min = 100;
            _LiveGainMedium.max = 5000;

            _LiveGainHigh = new IVLControlProperties();
            _LiveGainHigh.name = "LiveGainHigh";
            _LiveGainHigh.val = "400";
            _LiveGainHigh.type = "int";
            _LiveGainHigh.control = "System.Windows.Forms.NumericUpDown";
            _LiveGainHigh.text = "Live High Gain";
            _LiveGainHigh.min = 100;
            _LiveGainHigh.max = 5000;

            _DigitalGainDefault = new IVLControlProperties();
            _DigitalGainDefault.name = "DigitalGainDefault";
            _DigitalGainDefault.val = "Low";
            _DigitalGainDefault.type = "int";
            _DigitalGainDefault.control = "System.Windows.Forms.ComboBox";
            _DigitalGainDefault.text = " Digital Gain Default";
            _DigitalGainDefault.range = "Low,Medium,High";

            _DigitalGainLow = new IVLControlProperties();
            _DigitalGainLow.name = "DigitalGainLow";
            _DigitalGainLow.val = "100";
            _DigitalGainLow.type = "int";
            _DigitalGainLow.control = "System.Windows.Forms.NumericUpDown";
            _DigitalGainLow.text = "Digital Low Gain";
            _DigitalGainLow.min = 100;
            _DigitalGainLow.max = 5000;

            _DigitalGainMedium = new IVLControlProperties();
            _DigitalGainMedium.name = "DigitalGainMedium";
            _DigitalGainMedium.val = "300";
            _DigitalGainMedium.type = "int";
            _DigitalGainMedium.control = "System.Windows.Forms.NumericUpDown";
            _DigitalGainMedium.text = "Digital Medium Gain";
            _DigitalGainMedium.min = 100;
            _DigitalGainMedium.max = 5000;

            _DigitalGainHigh = new IVLControlProperties();
            _DigitalGainHigh.name = "DigitalGainHigh";
            _DigitalGainHigh.val = "400";
            _DigitalGainHigh.type = "int";
            _DigitalGainHigh.control = "System.Windows.Forms.NumericUpDown";
            _DigitalGainHigh.text = "Digital High Gain";
            _DigitalGainHigh.min = 100;
            _DigitalGainHigh.max = 5000;

            _Enable14Bit = new IVLControlProperties();
            _Enable14Bit.name = "Enable14Bit";
            _Enable14Bit.val = false.ToString();
            _Enable14Bit.type = "bool";
            _Enable14Bit.control = "System.Windows.Forms.RadioButton";
            _Enable14Bit.text = "Enable 14Bit";

            _EnableWB = new IVLControlProperties();
            _EnableWB.name = "EnableWB";
            _EnableWB.val = true.ToString();
            _EnableWB.type = "bool";
            _EnableWB.control = "System.Windows.Forms.RadioButton";
            _EnableWB.text = "Enable WB";

            _EnableVerticalFlip = new IVLControlProperties();
            _EnableVerticalFlip.name = "EnableVerticalFlip";
            _EnableVerticalFlip.val = true.ToString();
            _EnableVerticalFlip.type = "bool";
            _EnableVerticalFlip.control = "System.Windows.Forms.RadioButton";
            _EnableVerticalFlip.text = "Enable Vertical Flip";

            _EnableHorizontalFlip = new IVLControlProperties();
            _EnableHorizontalFlip.name = "EnableHorizontalFlip";
            _EnableHorizontalFlip.val = false.ToString();
            _EnableHorizontalFlip.type = "bool";
            _EnableHorizontalFlip.control = "System.Windows.Forms.RadioButton";
            _EnableHorizontalFlip.text = "Enable Horizontal Flip";

            _EnableRawMode = new IVLControlProperties();
            _EnableRawMode.name = "EnableRawMode";
            _EnableRawMode.val = false.ToString();//for prime and ffa by dashan
            _EnableRawMode.type = "bool";
            _EnableRawMode.control = "System.Windows.Forms.RadioButton";
            _EnableRawMode.text = "Enable Raw Mode";

            _ResumeLabelVisible = new IVLControlProperties();
            _ResumeLabelVisible.name = "ResumeLabelVisible";
            _ResumeLabelVisible.val = true.ToString();
            _ResumeLabelVisible.type = "bool";
            _ResumeLabelVisible.control = "System.Windows.Forms.RadioButton";
            _ResumeLabelVisible.text = "Resume Label Visible";

            _ShowCaptureFailure = new IVLControlProperties();
            _ShowCaptureFailure.name = "ShowCaptureFailure";
            _ShowCaptureFailure.val = true.ToString();
            _ShowCaptureFailure.type = "bool";
            _ShowCaptureFailure.control = "System.Windows.Forms.RadioButton";
            _ShowCaptureFailure.text = "Show Capture Failure";

            _FrameDetectionValue = new IVLControlProperties();
            _FrameDetectionValue.name = "FrameDetectionValue";
            _FrameDetectionValue.val = "2.5";
            _FrameDetectionValue.type = "float";
            _FrameDetectionValue.control = "System.Windows.Forms.NumericUpDown";
            _FrameDetectionValue.text = "Frame Detection Value";
            _FrameDetectionValue.min = 1;
            _FrameDetectionValue.max = 100;

            _IRTemperature = new IVLControlProperties();
            _IRTemperature.name = "IRTemperature";
            _IRTemperature.val = "6500";
            _IRTemperature.type = "int";
            _IRTemperature.control = "System.Windows.Forms.NumericUpDown";
            _IRTemperature.text = "IR Temperature";
            _IRTemperature.min = 2000;
            _IRTemperature.max = 15000;

            _FlashTemperature = new IVLControlProperties();
            _FlashTemperature.name = "FlashTemperature";
            _FlashTemperature.val = "6500";
            _FlashTemperature.type = "int";
            _FlashTemperature.control = "System.Windows.Forms.NumericUpDown";
            _FlashTemperature.text = "Flash Temperature";
            _FlashTemperature.min = 2000;
            _FlashTemperature.max = 15000;

            _IRTint = new IVLControlProperties();
            _IRTint.name = "IRTint";
            _IRTint.val = "1200";
            _IRTint.type = "int";
            _IRTint.control = "System.Windows.Forms.NumericUpDown";
            _IRTint.text = "IR Tint ";
            _IRTint.min = 200;
            _IRTint.max = 2500;

            _FlashTint = new IVLControlProperties();
            _FlashTint.name = "FlashTint";
            _FlashTint.val = "1200";
            _FlashTint.type = "int";
            _FlashTint.control = "System.Windows.Forms.NumericUpDown";
            _FlashTint.text = "Flash Tint ";
            _FlashTint.min = 200;
            _FlashTint.max = 2500;

        }
    }
}
