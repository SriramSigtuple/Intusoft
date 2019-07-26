using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using NLog;
using NLog.Targets;
namespace WindowsFormsApplication1.AdvancedSettings
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
        public static Logger Exception_Log = LogManager.GetLogger("DB_Porting.ExceptionLog");

        private static IVLControlProperties SaveFramesCount = null;

        public IVLControlProperties _SaveFramesCount
        {
            get { return CameraSettings.SaveFramesCount; }
            set { CameraSettings.SaveFramesCount = value; }
        }
        private static IVLControlProperties isFourteen = null;

        public IVLControlProperties _isFourteen
        {
            get { return CameraSettings.isFourteen; }
            set { CameraSettings.isFourteen = value; }
        }
        private static IVLControlProperties isIrSave = null;

        public IVLControlProperties _IsIrSave
        {
            get { return CameraSettings.isIrSave; }
            set { CameraSettings.isIrSave = value; }
        }
        private static IVLControlProperties isRawSave = null;

        public IVLControlProperties _IsRawSave
        {
            get { return CameraSettings.isRawSave; }
            set { CameraSettings.isRawSave = value; }
        }
        private static IVLControlProperties FrameDetectionValue = null;

        public IVLControlProperties _FrameDetectionValue
        {
            get { return CameraSettings.FrameDetectionValue; }
            set { CameraSettings.FrameDetectionValue = value; }
        }
        private static IVLControlProperties DarkFrameDetectionValue = null;

        public IVLControlProperties _DarkFrameDetectionValue
        {
            get { return CameraSettings.DarkFrameDetectionValue; }
            set { CameraSettings.DarkFrameDetectionValue = value; }
        }

        private static IVLControlProperties CameraModel = null;

        public IVLControlProperties _CameraModel
        {
            get { return CameraSettings.CameraModel; }
            set { CameraSettings.CameraModel = value; }
        }

        private static IVLControlProperties ImageWidth = null;
        /// <summary>
        /// Type int
        /// </summary>
        public IVLControlProperties _ImageWidth
        {
            get { return CameraSettings.ImageWidth; }
            set { CameraSettings.ImageWidth = value; }
        }
        private static IVLControlProperties ImageHeight = null;

        public IVLControlProperties _ImageHeight
        {
            get { return CameraSettings.ImageHeight; }
            set { CameraSettings.ImageHeight = value; }
        }
        private static IVLControlProperties ResumeLiveCnt = null;

        public IVLControlProperties _ResumeLiveCnt
        {
            get { return CameraSettings.ResumeLiveCnt; }
            set { CameraSettings.ResumeLiveCnt = value; }
        }
        private static IVLControlProperties ResumeLabelVisible = null;

        public IVLControlProperties _ResumeLabelVisible
        {
            get { return CameraSettings.ResumeLabelVisible; }
            set { CameraSettings.ResumeLabelVisible = value; }
        }
        private static IVLControlProperties temperature = null;

        public IVLControlProperties _Temperature
        {
            get { return CameraSettings.temperature; }
            set { CameraSettings.temperature = value; }
        }
        private static IVLControlProperties tint = null;

        public IVLControlProperties _Tint
        {
            get { return CameraSettings.tint; }
            set { CameraSettings.tint = value; }
        }

        private static IVLControlProperties presetGain = null;

        public IVLControlProperties _presetGain
        {
            get { return CameraSettings.presetGain; }
            set { CameraSettings.presetGain = value; }
        }

        private static IVLControlProperties digitalGain = null;

        public IVLControlProperties _DigitalGain
        {
            get { return CameraSettings.digitalGain; }
            set { CameraSettings.digitalGain = value; }
        }
        private static IVLControlProperties LiveDigitalGain = null;

        public IVLControlProperties _LiveDigitalGain
        {
            get { return CameraSettings.LiveDigitalGain; }
            set { CameraSettings.LiveDigitalGain = value; }
        }

        private static IVLControlProperties MotorPositiveColor = null;

        public IVLControlProperties _MotorPositiveColor
        {
            get { return CameraSettings.MotorPositiveColor; }
            set { CameraSettings.MotorPositiveColor = value; }
        }

        private static IVLControlProperties MotorNegativeColor = null;

        public IVLControlProperties _MotorNegativeColor
        {
            get { return CameraSettings.MotorNegativeColor; }
            set { CameraSettings.MotorNegativeColor = value; }
        }

        private static IVLControlProperties Exposure = null;

        public IVLControlProperties _Exposure
        {
            get { return CameraSettings.Exposure; }
            set { CameraSettings.Exposure = value; }
        }
        private static IVLControlProperties FlashExposure = null;

        public IVLControlProperties _FlashExposure
        {
            get { return CameraSettings.FlashExposure; }
            set { CameraSettings.FlashExposure = value; }
        }
        private static IVLControlProperties Saturation = null;

        public IVLControlProperties _Saturation
        {
            get { return CameraSettings.Saturation; }
            set { CameraSettings.Saturation = value; }
        }
        private static IVLControlProperties ShowCaptureFailure = null;

        public IVLControlProperties _ShowCaptureFailure
        {
            get { return CameraSettings.ShowCaptureFailure; }
            set { CameraSettings.ShowCaptureFailure = value; }
        }
        private static IVLControlProperties ImageOpticalCentreX = null;

        public IVLControlProperties _ImageOpticalCentreX
        {
            get { return CameraSettings.ImageOpticalCentreX; }
            set { CameraSettings.ImageOpticalCentreX = value; }
        }

        private static IVLControlProperties KnobIndexDifferenceValue = null;

        public IVLControlProperties _KnobIndexDifferenceValue
        {
            get { return CameraSettings.KnobIndexDifferenceValue; }
            set { CameraSettings.KnobIndexDifferenceValue = value; }
        }
        private static IVLControlProperties ImageOpticalCentreY = null;

        public IVLControlProperties _ImageOpticalCentreY
        {
            get { return CameraSettings.ImageOpticalCentreY; }
            set { CameraSettings.ImageOpticalCentreY = value; }
        }
        private static IVLControlProperties DeviceType = null;

        public IVLControlProperties _DeviceType
        {
            get { return CameraSettings.DeviceType; }
            set { CameraSettings.DeviceType = value; }
        }
        public CameraSettings()
        {
            try
            {
                _CameraModel = new IVLControlProperties();
                _CameraModel.name = "CameraModel";
                _CameraModel.type = "string";
                _CameraModel.val = "c";
                _CameraModel.control = "System.Windows.Forms.ComboBox";
                _CameraModel.text = "Camera Model";
                _CameraModel.range = "A,B,C";

                _DeviceType = new IVLControlProperties();
                _DeviceType.name = "DeviceType";
                _DeviceType.type = "string";
                _DeviceType.val = "45";
                _DeviceType.control = "System.Windows.Forms.ComboBox";
                _DeviceType.text = "Camera Type";
                _DeviceType.range = "45,FFA,Prime";

                _SaveFramesCount = new IVLControlProperties();
                _SaveFramesCount.name = "SaveFramesCount";
                _SaveFramesCount.val = "8";
                _SaveFramesCount.type = "int";
                _SaveFramesCount.control = "System.Windows.Forms.NumericUpDown";
                _SaveFramesCount.text = "Save Frames Count";
                _SaveFramesCount.min = 1;
                _SaveFramesCount.max = 30;

                _KnobIndexDifferenceValue = new IVLControlProperties();
                _KnobIndexDifferenceValue.name = "KnobIndexDifferenceValue";
                _KnobIndexDifferenceValue.val = "23";
                _KnobIndexDifferenceValue.type = "int";
                _KnobIndexDifferenceValue.control = "System.Windows.Forms.NumericUpDown";
                _KnobIndexDifferenceValue.text = "KnobIndex Difference Value";
                _KnobIndexDifferenceValue.min = 1;
                _KnobIndexDifferenceValue.max = 43;

                _DarkFrameDetectionValue = new IVLControlProperties();
                _DarkFrameDetectionValue.name = "DarkFrameDetectionValue";
                _DarkFrameDetectionValue.val = "100";
                _DarkFrameDetectionValue.type = "int";
                _DarkFrameDetectionValue.control = "System.Windows.Forms.NumericUpDown";
                _DarkFrameDetectionValue.text = "Dark FrameDetection Value";
                _DarkFrameDetectionValue.min = 1;
                _DarkFrameDetectionValue.max = 255;

                _ImageWidth = new IVLControlProperties();
                _ImageWidth.name = "ImageWidth";
                _ImageWidth.val = "2048";
                _ImageWidth.type = "int";
                _ImageWidth.control = "System.Windows.Forms.NumericUpDown";
                _ImageWidth.text = "Image Width";
                _ImageWidth.min = 1;
                _ImageWidth.max = 10000;

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

                _ImageHeight = new IVLControlProperties();
                _ImageHeight.name = "ImageHeight";
                _ImageHeight.val = "1536";
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
                _presetGain.min = 0;
                _presetGain.max = 5000;

                _DigitalGain = new IVLControlProperties();
                _DigitalGain.name = "digitalGain";
                _DigitalGain.val = "100";
                _DigitalGain.type = "int";
                _DigitalGain.control = "System.Windows.Forms.NumericUpDown";
                _DigitalGain.text = "Digital Gain";
                _DigitalGain.min = 0;
                _DigitalGain.max = 5000;

                _LiveDigitalGain = new IVLControlProperties();
                _LiveDigitalGain.name = "LiveDigitalGain";
                _LiveDigitalGain.val = "23";
                _LiveDigitalGain.type = "int";
                _LiveDigitalGain.control = "System.Windows.Forms.NumericUpDown";
                _LiveDigitalGain.text = "Live Digital Gain";
                _LiveDigitalGain.min = 0;
                _LiveDigitalGain.max = 5000;

                _Exposure = new IVLControlProperties();
                _Exposure.name = "Exposure";
                _Exposure.val = "77376";
                _Exposure.type = "int";
                _Exposure.control = "System.Windows.Forms.NumericUpDown";
                _Exposure.text = "Exposure";
                _Exposure.min = 2500;
                _Exposure.max = 300000;

                _FlashExposure = new IVLControlProperties();
                _FlashExposure.name = "FlashExposure";
                _FlashExposure.val = "77376";
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
                _ImageOpticalCentreX.val = "1024";
                _ImageOpticalCentreX.type = "int";
                _ImageOpticalCentreX.control = "System.Windows.Forms.NumericUpDown";
                _ImageOpticalCentreX.text = "Image Optical CentreX";
                _ImageOpticalCentreX.min = 1;
                _ImageOpticalCentreX.max = 10000;

                _ImageOpticalCentreY = new IVLControlProperties();
                _ImageOpticalCentreY.name = "ImageOpticalCentreY";
                _ImageOpticalCentreY.val = "768";
                _ImageOpticalCentreY.type = "int";
                _ImageOpticalCentreY.control = "System.Windows.Forms.NumericUpDown";
                _ImageOpticalCentreY.text = "Image Optical CentreY";
                _ImageOpticalCentreY.min = 1;
                _ImageOpticalCentreY.max = 10000;

                _IsIrSave = new IVLControlProperties();
                _IsIrSave.name = "isIrSave";
                _IsIrSave.val = true.ToString();
                _IsIrSave.type = "bool";
                _IsIrSave.control = "System.Windows.Forms.RadioButton";
                _IsIrSave.text = "Save IR";

                _IsRawSave = new IVLControlProperties();
                _IsRawSave.name = "isRawSave";
                _IsRawSave.val = false.ToString();
                _IsRawSave.type = "bool";
                _IsRawSave.control = "System.Windows.Forms.RadioButton";
                _IsRawSave.text = "Save Raw";

                _isFourteen = new IVLControlProperties();
                _isFourteen.name = "isFourteen";
                _isFourteen.val = true.ToString();
                _isFourteen.type = "bool";
                _isFourteen.control = "System.Windows.Forms.RadioButton";
                _isFourteen.text = "Fourteen Bit";

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
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }
    }
}
