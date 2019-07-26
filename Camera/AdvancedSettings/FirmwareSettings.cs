using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace INTUSOFT.Desktop.AdvancedSettings
{
    [Serializable]
    public class FirmwareSettings
    {
        // Motor Ply Compensation parameters
        //public int MotorCaptureSteps = 25;
        //public int MotorCaptureStepsMax = 25;
        // public int MotorPlySteps = 3;
        //public bool isApplyPly = false;
        //public bool isMotorCompensation = true;
        //public bool isMotorPolarityForward = false;
        //public bool isUSBCommunication = true;
        //public int BaudRate = 115200;
        //public int CameraPowerTimerInterval = 1000;
        private  IVLControlProperties MotorCaptureSteps = null;

        public IVLControlProperties _MotorCaptureSteps
        {
            get { return MotorCaptureSteps; }
            set { MotorCaptureSteps = value; }
        }
        //private  IVLControlProperties MotorCaptureStepsMax = null;

        //public IVLControlProperties _MotorCaptureStepsMax
        //{
        //    get { return MotorCaptureStepsMax; }
        //    set { MotorCaptureStepsMax = value; }
        //}
        private  IVLControlProperties MotorPlySteps = null;

        public IVLControlProperties _MotorPlySteps
        {
            get { return MotorPlySteps; }
            set { MotorPlySteps = value; }
        }
        private  IVLControlProperties isApplyPly = null;

        public IVLControlProperties _IsApplyPly
        {
            get { return isApplyPly; }
            set { isApplyPly = value; }
        }
        private  IVLControlProperties isMotorCompensation = null;

        public IVLControlProperties _IsMotorCompensation
        {
            get { return isMotorCompensation; }
            set { isMotorCompensation = value; }
        }
        private  IVLControlProperties isMotorPolarityForward = null;

        public IVLControlProperties _IsMotorPolarityForward
        {
            get { return isMotorPolarityForward; }
            set { isMotorPolarityForward = value; }
        }
        private  IVLControlProperties isUSBCommunication = null;

        public IVLControlProperties _IsUSBCommunication
        {
            get { return isUSBCommunication; }
            set { isUSBCommunication = value; }
        }
        private  IVLControlProperties CameraPowerTimerInterval = null;

        public IVLControlProperties _CameraPowerTimerInterval
        {
            get { return CameraPowerTimerInterval; }
            set { CameraPowerTimerInterval = value; }
        }


        private  IVLControlProperties FlashOffsetStart = null;

        public IVLControlProperties _FlashOffsetStart
        {
            get { return FlashOffsetStart; }
            set { FlashOffsetStart = value; }
        }

        private  IVLControlProperties FlashOffsetEnd = null;

        public IVLControlProperties _FlashOffsetEnd
        {
            get { return FlashOffsetEnd; }
            set { FlashOffsetEnd = value; }
        }

        private  IVLControlProperties isFlashBoost = null;

        public IVLControlProperties _isFlashBoost
        {
            get { return isFlashBoost; }
            set { isFlashBoost = value; }
        }
        private  IVLControlProperties FlashBoostValue = null;
        /// <summary>
        /// Flash boost value contains the value used to boost the flash intensity it is int form 
        /// </summary>
        public IVLControlProperties _FlashBoostValue
        {
            get { return FlashBoostValue; }
            set { FlashBoostValue = value; }
        }

        private  IVLControlProperties EnableSingleFrameCapture = null;

        public IVLControlProperties _EnableSingleFrameCapture
        {
            get { return EnableSingleFrameCapture; }
            set { EnableSingleFrameCapture = value; }
        }

        private  IVLControlProperties BoardCommandIteration = null;

        public IVLControlProperties _BoardCommandIteration
        {
            get { return BoardCommandIteration; }
            set { BoardCommandIteration = value; }
        }

        private  IVLControlProperties EnableLeftRightSensor = null;

        public IVLControlProperties _EnableLeftRightSensor
        {
            get { return EnableLeftRightSensor; }
            set { EnableLeftRightSensor
                = value; }
        }
       public FirmwareSettings()
        {
            _IsApplyPly = new IVLControlProperties();
            _IsApplyPly.name = "isApplyPly";
            _IsApplyPly.val = false.ToString();
            _IsApplyPly.type = "bool";
            _IsApplyPly.control = "System.Windows.Forms.RadioButton";
            _IsApplyPly.text = "Apply Ply";

            _IsMotorCompensation = new IVLControlProperties();
            _IsMotorCompensation.name = "isMotorCompensation";
            _IsMotorCompensation.val = true.ToString();
            _IsMotorCompensation.type = "bool";
            _IsMotorCompensation.control = "System.Windows.Forms.RadioButton";
            _IsMotorCompensation.text = "Motor Compensation";

            _EnableSingleFrameCapture = new IVLControlProperties();
            _EnableSingleFrameCapture.name = "enableSingleFrameCapture";
            _EnableSingleFrameCapture.val = true.ToString();
            _EnableSingleFrameCapture.type = "bool";
            _EnableSingleFrameCapture.control = "System.Windows.Forms.RadioButton";
            _EnableSingleFrameCapture.text = "Enable Single Frame Capture";

            _IsMotorPolarityForward = new IVLControlProperties();
            _IsMotorPolarityForward.name = "isMotorPolarityForward";
            _IsMotorPolarityForward.val = false.ToString();
            _IsMotorPolarityForward.type = "bool";
            _IsMotorPolarityForward.control = "System.Windows.Forms.RadioButton";
            _IsMotorPolarityForward.text = "Motor Polarity Forward";


            _IsUSBCommunication = new IVLControlProperties();
            _IsUSBCommunication.name = "isUSBCommunication";
            _IsUSBCommunication.val = true.ToString();
            _IsUSBCommunication.type = "bool";
            _IsUSBCommunication.control = "System.Windows.Forms.RadioButton";
            _IsUSBCommunication.text = "Is USBCommunication";


            _MotorCaptureSteps = new IVLControlProperties();
            _MotorCaptureSteps.name = "MotorCaptureSteps";
            _MotorCaptureSteps.val = "25";
            _MotorCaptureSteps.type = "int";
            _MotorCaptureSteps.control = "System.Windows.Forms.NumericUpDown";
            _MotorCaptureSteps.text = "Motor Capture Steps";
            _MotorCaptureSteps.min = 1;
            _MotorCaptureSteps.max = 25;

            _BoardCommandIteration = new IVLControlProperties();
            _BoardCommandIteration.name = "BoardCommandIteration";
            _BoardCommandIteration.val = "2";
            _BoardCommandIteration.type = "byte";
            _BoardCommandIteration.control = "System.Windows.Forms.NumericUpDown";
            _BoardCommandIteration.text = "Board Command Iteration";
            _BoardCommandIteration.min = 1;
            _BoardCommandIteration.max = 10;


            //_MotorCaptureStepsMax = new IVLControlProperties();
            //_MotorCaptureStepsMax.name = "MotorCaptureStepsMax";
            //_MotorCaptureStepsMax.val = "25";
            //_MotorCaptureStepsMax.type = "int";
            //_MotorCaptureStepsMax.control = "System.Windows.Forms.NumericUpDown";
            //_MotorCaptureStepsMax.text = "Max Motor Capture Steps";


            _MotorPlySteps = new IVLControlProperties();
            _MotorPlySteps.name = "MotorPlySteps";
            _MotorPlySteps.val = "3";
            _MotorPlySteps.type = "int";
            _MotorPlySteps.control = "System.Windows.Forms.NumericUpDown";
            _MotorPlySteps.text = "Motor Ply Steps";
            _MotorPlySteps.min = 1;
            _MotorPlySteps.max = 100;



            _CameraPowerTimerInterval = new IVLControlProperties();
            _CameraPowerTimerInterval.name = "CameraPowerTimerInterval";
            _CameraPowerTimerInterval.val = "1000";
            _CameraPowerTimerInterval.type = "int";
            _CameraPowerTimerInterval.control = "System.Windows.Forms.NumericUpDown";
            _CameraPowerTimerInterval.text = "Camera Power Timer Interval";
            _CameraPowerTimerInterval.min = 100;
            _CameraPowerTimerInterval.max = 10000;



           

            _FlashOffsetStart = new IVLControlProperties();
            _FlashOffsetStart.name = "FlashOffsetStart";
            _FlashOffsetStart.val = "90";
            _FlashOffsetStart.type = "int";
            _FlashOffsetStart.control = "System.Windows.Forms.NumericUpDown";
            _FlashOffsetStart.text = "Flash OffSet Start Value";
            _FlashOffsetStart.min = 0;
            _FlashOffsetStart.max = 255;

            _FlashOffsetEnd = new IVLControlProperties();
            _FlashOffsetEnd.name = "FlashOffsetEnd";
            _FlashOffsetEnd.val = "10";
            _FlashOffsetEnd.type = "int";
            _FlashOffsetEnd.control = "System.Windows.Forms.NumericUpDown";
            _FlashOffsetEnd.text = "Flash OffSet End Value";
            _FlashOffsetEnd.min = 0;
            _FlashOffsetEnd.max = 255;

            _FlashBoostValue = new IVLControlProperties();
            _FlashBoostValue.name = "FlashBoostValue";
            _FlashBoostValue.val = "95";
            _FlashBoostValue.type = "int";
            _FlashBoostValue.control = "System.Windows.Forms.NumericUpDown";
            _FlashBoostValue.text = "Flash Boost Value";
            _FlashBoostValue.min = 1;
            _FlashBoostValue.max = 255;

            _isFlashBoost = new IVLControlProperties();
            _isFlashBoost.name = "isFlashBoost";
            _isFlashBoost.val = "true";
            _isFlashBoost.type = "bool";
            _isFlashBoost.control = "System.Windows.Forms.RadioButton";
            _isFlashBoost.text = "Enable Flash Boost";

            _EnableLeftRightSensor = new IVLControlProperties();
            _EnableLeftRightSensor.name = "isEnableLeftRightSensor";
            _EnableLeftRightSensor.val = "true";
            _EnableLeftRightSensor.type = "bool";
            _EnableLeftRightSensor.control = "System.Windows.Forms.RadioButton";
            _EnableLeftRightSensor.text = "Enable Left Right Sensor";


        }
    }
}
