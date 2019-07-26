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
    public class FirmwareSettings
    {
        public static Logger Exception_Log = LogManager.GetLogger("DB_Porting.ExceptionLog");

        private static IVLControlProperties MotorCaptureSteps = null;

        public IVLControlProperties _MotorCaptureSteps
        {
            get { return FirmwareSettings.MotorCaptureSteps; }
            set { FirmwareSettings.MotorCaptureSteps = value; }
        }
        private static IVLControlProperties isMotorCompensation = null;

        public IVLControlProperties _IsMotorCompensation
        {
            get { return FirmwareSettings.isMotorCompensation; }
            set { FirmwareSettings.isMotorCompensation = value; }
        }
        private static IVLControlProperties isMotorPolarityForward = null;

        public IVLControlProperties _IsMotorPolarityForward
        {
            get { return FirmwareSettings.isMotorPolarityForward; }
            set { FirmwareSettings.isMotorPolarityForward = value; }
        }
        private static IVLControlProperties CameraPowerTimerInterval = null;

        public IVLControlProperties _CameraPowerTimerInterval
        {
            get { return FirmwareSettings.CameraPowerTimerInterval; }
            set { FirmwareSettings.CameraPowerTimerInterval = value; }
        }
        private static IVLControlProperties SingleFrame = null;

        public IVLControlProperties _SingleFrame
        {
            get { return FirmwareSettings.SingleFrame; }
            set { FirmwareSettings.SingleFrame = value; }
        }

        //private static IVLControlProperties FirmWareVersion = null;

        //public IVLControlProperties _FirmWareVersion
        //{
        //    get { return FirmwareSettings.FirmWareVersion; }
        //    set { FirmwareSettings.FirmWareVersion = value; }
        //}


        private static IVLControlProperties FlashOffset1 = null;

        public IVLControlProperties _FlashOffset1
        {
            get { return FirmwareSettings.FlashOffset1; }
            set { FirmwareSettings.FlashOffset1 = value; }
        }

        private static IVLControlProperties FlashOffset2 = null;

        public IVLControlProperties _FlashOffset2
        {
            get { return FirmwareSettings.FlashOffset2; }
            set { FirmwareSettings.FlashOffset2 = value; }
        }
        private static IVLControlProperties isFlashBoost = null;

        public IVLControlProperties _isFlashBoost
        {
            get { return FirmwareSettings.isFlashBoost; }
            set { FirmwareSettings.isFlashBoost = value; }
        }

        private static IVLControlProperties EnablePCUKnob = null;

        public IVLControlProperties _EnablePCUKnob
        {
            get { return FirmwareSettings.EnablePCUKnob; }
            set { FirmwareSettings.EnablePCUKnob = value; }
        }

        private static IVLControlProperties FlashBoostValue = null;

        /// <summary>
        /// Flash boost value contains the value used to boost the flash intensity it is int form 
        /// </summary>
        public IVLControlProperties _FlashBoostValue
        {
            get { return FirmwareSettings.FlashBoostValue; }
            set { FirmwareSettings.FlashBoostValue = value; }
        }
        private static IVLControlProperties EnableLeftRightSensor = null;

        public IVLControlProperties _EnableLeftRightSensor
        {
            get { return FirmwareSettings.EnableLeftRightSensor; }
            set { FirmwareSettings.EnableLeftRightSensor
                = value; }
        }

        private static IVLControlProperties BoardCommandIteration = null;

        public IVLControlProperties _BoardCommandIteration
        {
            get { return FirmwareSettings.BoardCommandIteration; }
            set { FirmwareSettings.BoardCommandIteration = value; }
        }

        private static IVLControlProperties isReverseMotorPositionSensor = null;

        public IVLControlProperties _isReverseMotorPositionSensor
        {
            get { return FirmwareSettings.isReverseMotorPositionSensor; }
            set { FirmwareSettings.isReverseMotorPositionSensor = value; }
        }
       public FirmwareSettings()
        {
            try
            {
                _IsMotorCompensation = new IVLControlProperties();
                _IsMotorCompensation.name = "isMotorCompensation";
                _IsMotorCompensation.val = true.ToString();
                _IsMotorCompensation.type = "bool";
                _IsMotorCompensation.control = "System.Windows.Forms.RadioButton";
                _IsMotorCompensation.text = "Motor Compensation";

                _IsMotorPolarityForward = new IVLControlProperties();
                _IsMotorPolarityForward.name = "isMotorPolarityForward";
                _IsMotorPolarityForward.val = false.ToString();
                _IsMotorPolarityForward.type = "bool";
                _IsMotorPolarityForward.control = "System.Windows.Forms.RadioButton";
                _IsMotorPolarityForward.text = "Motor Polarity Forward";


                _EnablePCUKnob = new IVLControlProperties();
                _EnablePCUKnob.name = "EnablePCUKnob";
                _EnablePCUKnob.val = true.ToString();
                _EnablePCUKnob.type = "bool";
                _EnablePCUKnob.control = "System.Windows.Forms.RadioButton";
                _EnablePCUKnob.text = "Enable PCU Knob";

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
                _BoardCommandIteration.val = "3";
                _BoardCommandIteration.type = "byte";
                _BoardCommandIteration.control = "System.Windows.Forms.NumericUpDown";
                _BoardCommandIteration.text = "Board Command Iteration";
                _BoardCommandIteration.min = 1;
                _BoardCommandIteration.max = 10;

                _CameraPowerTimerInterval = new IVLControlProperties();
                _CameraPowerTimerInterval.name = "CameraPowerTimerInterval";
                _CameraPowerTimerInterval.val = "1000";
                _CameraPowerTimerInterval.type = "int";
                _CameraPowerTimerInterval.control = "System.Windows.Forms.NumericUpDown";
                _CameraPowerTimerInterval.text = "Camera Power Timer Interval";
                _CameraPowerTimerInterval.min = 100;
                _CameraPowerTimerInterval.max = 10000;

                _SingleFrame = new IVLControlProperties();
                _SingleFrame.name = "SingleFrame";
                _SingleFrame.val = true.ToString();
                _SingleFrame.type = "bool";
                _SingleFrame.control = "System.Windows.Forms.RadioButton";
                _SingleFrame.text = "Enable Single Frame Capture";


                //_FirmWareVersion = new IVLControlProperties();
                //_FirmWareVersion.name = "FirmWareVersion";
                //_FirmWareVersion.val = "V2.0.14";
                //_FirmWareVersion.type = "string";
                //_FirmWareVersion.control = "System.Windows.Forms.TextBox";
                //_FirmWareVersion.text = "FirmWareVersion";

                _FlashOffset1 = new IVLControlProperties();
                _FlashOffset1.name = "FlashOffset1";
                _FlashOffset1.val = "70";
                _FlashOffset1.type = "int";
                _FlashOffset1.control = "System.Windows.Forms.NumericUpDown";
                _FlashOffset1.text = "Flash Offset1 Value";
                _FlashOffset1.min = 1;
                _FlashOffset1.max = 255;

                _FlashOffset2 = new IVLControlProperties();
                _FlashOffset2.name = "FlashOffset2";
                _FlashOffset2.val = "5";
                _FlashOffset2.type = "int";
                _FlashOffset2.control = "System.Windows.Forms.NumericUpDown";
                _FlashOffset2.text = "Flash Offset2 Value";
                _FlashOffset2.min = 1;
                _FlashOffset2.max = 255;

                _FlashBoostValue = new IVLControlProperties();
                _FlashBoostValue.name = "FlashBoostValue";
                _FlashBoostValue.val = "70";
                _FlashBoostValue.type = "int";
                _FlashBoostValue.control = "System.Windows.Forms.NumericUpDown";
                _FlashBoostValue.text = "Flash Boost Value";
                _FlashBoostValue.min = 1;
                _FlashBoostValue.max = 255;

                _isFlashBoost = new IVLControlProperties();
                _isFlashBoost.name = "isFlashBoost";
                _isFlashBoost.val = "false";
                _isFlashBoost.type = "bool";
                _isFlashBoost.control = "System.Windows.Forms.RadioButton";
                _isFlashBoost.text = "Enable Flash Boost";

                _EnableLeftRightSensor = new IVLControlProperties();
                _EnableLeftRightSensor.name = "isEnableLeftRightSensor";
                _EnableLeftRightSensor.val = "true";
                _EnableLeftRightSensor.type = "bool";
                _EnableLeftRightSensor.control = "System.Windows.Forms.RadioButton";
                _EnableLeftRightSensor.text = "Enable Left Right Sensor";

                _isReverseMotorPositionSensor = new IVLControlProperties();
                _isReverseMotorPositionSensor.name = "isReverseMotorPositionSensor";
                _isReverseMotorPositionSensor.val = "true";
                _isReverseMotorPositionSensor.type = "bool";
                _isReverseMotorPositionSensor.control = "System.Windows.Forms.RadioButton";
                _isReverseMotorPositionSensor.text = "Reverse Motor Position Sensor";
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            

         }
    }
}
