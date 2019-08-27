using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace INTUSOFT.Configuration.AdvanceSettings
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
        private IVLControlProperties motorPerStepTime = null;

        public IVLControlProperties MotorPerStepTime
        {
            get { return motorPerStepTime; }
            set { motorPerStepTime = value; }
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

        private static IVLControlProperties EnablePCUKnob = null;

        public IVLControlProperties _EnablePCUKnob
        {
            get { return FirmwareSettings.EnablePCUKnob; }
            set { FirmwareSettings.EnablePCUKnob = value; }
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
        private IVLControlProperties blueFilterPos = null;

        public IVLControlProperties BlueFilterPos
        {
            get { return blueFilterPos; }
            set { blueFilterPos = value; }
        }

        private IVLControlProperties greenFilterPos = null;

        public IVLControlProperties GreenFilterPos
        {
            get { return greenFilterPos; }
            set { greenFilterPos = value; }
        }
        private IVLControlProperties potOffsetValue = null;

        public IVLControlProperties PotOffsetValue
        {
            get { return potOffsetValue; }
            set { potOffsetValue = value; }
        }

        private static IVLControlProperties isReverseMotorPositionSensor = null;

        public IVLControlProperties _isReverseMotorPositionSensor
        {
            get { return FirmwareSettings.isReverseMotorPositionSensor; }
            set { FirmwareSettings.isReverseMotorPositionSensor = value; }
        }

        private IVLControlProperties motorSensorStepsMax = null;

        public IVLControlProperties MotorSensorStepsMax
        {
            get { return motorSensorStepsMax; }
            set { motorSensorStepsMax = value; }
        }
        private IVLControlProperties isMotorSensorPresent = null;

        public IVLControlProperties IsMotorSensorPresent
        {
            get { return isMotorSensorPresent; }
            set { isMotorSensorPresent = value; }
        }

        private IVLControlProperties interruptTimeInterval = null;

        public IVLControlProperties InterruptTimeInterval
        {
            get { return interruptTimeInterval; }
            set { interruptTimeInterval = value; }
        }

        private IVLControlProperties bulkTransferTimeInterval = null;

        public IVLControlProperties BulkTransferTimeInterval
        {
            get { return bulkTransferTimeInterval; }
            set { bulkTransferTimeInterval = value; }
        }
        private IVLControlProperties flashOnDoneStrobeCycleValue = null;

        public IVLControlProperties FlashOnDoneStrobeCycleValue
        {
            get { return flashOnDoneStrobeCycleValue; }
            set { flashOnDoneStrobeCycleValue = value; }
        }

        private IVLControlProperties flashOffDoneStrobeCycleValue = null;

        public IVLControlProperties FlashOffDoneStrobeCycleValue
        {
            get { return flashOffDoneStrobeCycleValue; }
            set { flashOffDoneStrobeCycleValue = value; }
        }

        private IVLControlProperties powerCameraRemovalCheck = null;

        public IVLControlProperties PowerCameraRemovalCheck
        {
            get { return powerCameraRemovalCheck; }
            set { powerCameraRemovalCheck = value; }
        }

        private IVLControlProperties negativeDiaptorMaxValue = null;

        public IVLControlProperties NegativeDiaptorMaxValue { get => negativeDiaptorMaxValue; set => negativeDiaptorMaxValue = value; }


        private IVLControlProperties positiveDiaptorMaxValue = null;

        public IVLControlProperties PositiveDiaptorMaxValue { get => positiveDiaptorMaxValue; set => positiveDiaptorMaxValue = value; }


        public FirmwareSettings()
        {
            _IsApplyPly = new IVLControlProperties();
            _IsApplyPly.name = "isApplyPly";
            _IsApplyPly.val = false.ToString();
            _IsApplyPly.type = "bool";
            _IsApplyPly.control = "System.Windows.Forms.RadioButton";
            _IsApplyPly.text = "Apply Ply";

            PowerCameraRemovalCheck = new IVLControlProperties();
            PowerCameraRemovalCheck.name = "PowerCameraRemovalCheck";
            PowerCameraRemovalCheck.val = true.ToString();
            PowerCameraRemovalCheck.type = "bool";
            PowerCameraRemovalCheck.control = "System.Windows.Forms.RadioButton";
            PowerCameraRemovalCheck.text = "Power Camera Removal Check";

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
            _MotorCaptureSteps.max = 100;

            MotorPerStepTime = new IVLControlProperties();
            MotorPerStepTime.name = "MotorPerStepTime";
            MotorPerStepTime.val = "25";
            MotorPerStepTime.type = "int";
            MotorPerStepTime.control = "System.Windows.Forms.NumericUpDown";
            MotorPerStepTime.text = "Motor Per Step Time";
            MotorPerStepTime.min = 1;
            MotorPerStepTime.max = 100;

            _BoardCommandIteration = new IVLControlProperties();
            _BoardCommandIteration.name = "BoardCommandIteration";
            _BoardCommandIteration.val = "3";
            _BoardCommandIteration.type = "byte";
            _BoardCommandIteration.control = "System.Windows.Forms.NumericUpDown";
            _BoardCommandIteration.text = "Board Command Iteration";
            _BoardCommandIteration.min = 1;
            _BoardCommandIteration.max = 10;

            _EnablePCUKnob = new IVLControlProperties();
            _EnablePCUKnob.name = "EnablePCUKnob";
            _EnablePCUKnob.val = true.ToString();
            _EnablePCUKnob.type = "bool";
            _EnablePCUKnob.control = "System.Windows.Forms.RadioButton";
            _EnablePCUKnob.text = "Enable PCU Knob";

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

            PotOffsetValue = new IVLControlProperties();
            PotOffsetValue.name = "PotOffsetValue";
            PotOffsetValue.val = "10";
            PotOffsetValue.type = "int";
            PotOffsetValue.control = "System.Windows.Forms.NumericUpDown";
            PotOffsetValue.text = "Pot Offset Value";
            PotOffsetValue.min = 1;
            PotOffsetValue.max = 255;

            BlueFilterPos = new IVLControlProperties();
            BlueFilterPos.name = "BlueFilterPos";
            BlueFilterPos.val = "1500";
            BlueFilterPos.type = "int";
            BlueFilterPos.control = "System.Windows.Forms.NumericUpDown";
            BlueFilterPos.text = "Blue Filter Position";
            BlueFilterPos.min = 300;
            BlueFilterPos.max = 5000;

            GreenFilterPos = new IVLControlProperties();
            GreenFilterPos.name = "MotorPlySteps";
            GreenFilterPos.val = "1500";
            GreenFilterPos.type = "int";
            GreenFilterPos.control = "System.Windows.Forms.NumericUpDown";
            GreenFilterPos.text = "Green Filter Position";
            GreenFilterPos.min = 300;
            GreenFilterPos.max = 5000;

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
            _FlashOffsetStart.val = "70";
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
            _FlashBoostValue.val = "70";
            _FlashBoostValue.type = "int";
            _FlashBoostValue.control = "System.Windows.Forms.NumericUpDown";
            _FlashBoostValue.text = "Flash Boost Value";
            _FlashBoostValue.min = 1;
            _FlashBoostValue.max = 255;

            MotorSensorStepsMax = new IVLControlProperties();
            MotorSensorStepsMax.name = "MotorSensorStepsMax";
            MotorSensorStepsMax.val = "630";
            MotorSensorStepsMax.type = "int";
            MotorSensorStepsMax.control = "System.Windows.Forms.NumericUpDown";
            MotorSensorStepsMax.text = "Motor Sensor Steps Max";
            MotorSensorStepsMax.min = 1;
            MotorSensorStepsMax.max = 5000;

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

            IsMotorSensorPresent = new IVLControlProperties();
            IsMotorSensorPresent.name = "IsMotorSensorPresent";
            IsMotorSensorPresent.val = "true";
            IsMotorSensorPresent.type = "bool";
            IsMotorSensorPresent.control = "System.Windows.Forms.RadioButton";
            IsMotorSensorPresent.text = "Motor Sensor Present";


            InterruptTimeInterval = new IVLControlProperties();
            InterruptTimeInterval.name = "InterruptTimeInterval";
            InterruptTimeInterval.val = "20000";
            InterruptTimeInterval.type = "int";
            InterruptTimeInterval.control = "System.Windows.Forms.NumericUpDown";
            InterruptTimeInterval.text = "Interrupt Time Interval";
            InterruptTimeInterval.max = 100000;



            BulkTransferTimeInterval = new IVLControlProperties();
            BulkTransferTimeInterval.name = "BulkTransferTimeInterval";
            BulkTransferTimeInterval.val = "20000";
            BulkTransferTimeInterval.type = "int";
            BulkTransferTimeInterval.control = "System.Windows.Forms.NumericUpDown";
            BulkTransferTimeInterval.text = "Bulk Transfer Time Interval";
            BulkTransferTimeInterval.max = 100000;

            FlashOnDoneStrobeCycleValue = new IVLControlProperties();
            FlashOnDoneStrobeCycleValue.name = "FlashOnDoneStrobeCycleValue";
            FlashOnDoneStrobeCycleValue.val = "3";
            FlashOnDoneStrobeCycleValue.type = "int";
            FlashOnDoneStrobeCycleValue.control = "System.Windows.Forms.NumericUpDown";
            FlashOnDoneStrobeCycleValue.text = "Flash On Done Strobe Cycle Value";
            FlashOnDoneStrobeCycleValue.min = 1;
            FlashOnDoneStrobeCycleValue.max = 10;


            FlashOffDoneStrobeCycleValue = new IVLControlProperties();
            FlashOffDoneStrobeCycleValue.name = "FlashOffDoneStrobeCycleValue";
            FlashOffDoneStrobeCycleValue.val = "3";
            FlashOffDoneStrobeCycleValue.type = "int";
            FlashOffDoneStrobeCycleValue.control = "System.Windows.Forms.NumericUpDown";
            FlashOffDoneStrobeCycleValue.text = "Flash Off Done Strobe Cycle Value";
            FlashOffDoneStrobeCycleValue.min = 1;
            FlashOffDoneStrobeCycleValue.max = 10;

            NegativeDiaptorMaxValue = new IVLControlProperties();
            NegativeDiaptorMaxValue.name = "NegativeDiaptorMaxValue";
            NegativeDiaptorMaxValue.val = "200";
            NegativeDiaptorMaxValue.type = "int";
            NegativeDiaptorMaxValue.control = "System.Windows.Forms.NumericUpDown";
            NegativeDiaptorMaxValue.text = "Negative Diaptor Max Value";
            NegativeDiaptorMaxValue.min = 1;
            NegativeDiaptorMaxValue.max = 1000;

           PositiveDiaptorMaxValue = new IVLControlProperties();
           PositiveDiaptorMaxValue.name = "PositiveDiaptorMaxValue";
           PositiveDiaptorMaxValue.val = "200";
           PositiveDiaptorMaxValue.type = "int";
           PositiveDiaptorMaxValue.control = "System.Windows.Forms.NumericUpDown";
           PositiveDiaptorMaxValue.text = "Positive Diaptor Max Value";
           PositiveDiaptorMaxValue.min = 1;
           PositiveDiaptorMaxValue.max = 1000;

        }
    }

    [Serializable]
    public class MotorOffSetSettings
    {

        private IVLControlProperties iR2IR = null;

        public IVLControlProperties IR2IR
        {
            get { return iR2IR; }
            set { iR2IR = value; }
        }

        private IVLControlProperties iR2Flash = null;

        public IVLControlProperties IR2Flash
        {
            get { return iR2Flash; }
            set { iR2Flash = value; }
        }

        private IVLControlProperties iR2Blue = null;

        public IVLControlProperties IR2Blue
        {
            get { return iR2Blue; }
            set { iR2Blue = value; }
        }

        private IVLControlProperties flash2IR = null;

        public IVLControlProperties Flash2IR
        {
            get { return flash2IR; }
            set { flash2IR = value; }
        }

        private IVLControlProperties flash2Flash = null;

        public IVLControlProperties Flash2Flash
        {
            get { return flash2Flash; }
            set { flash2Flash = value; }
        }

        private IVLControlProperties flash2Blue = null;

        public IVLControlProperties Flash2Blue
        {
            get { return flash2Blue; }
            set { flash2Blue = value; }
        }

        private IVLControlProperties blue2IR = null;

        public IVLControlProperties Blue2IR
        {
            get { return blue2IR; }
            set { blue2IR = value; }
        }

        private IVLControlProperties blue2Flash = null;

        public IVLControlProperties Blue2Flash
        {
            get { return blue2Flash; }
            set { blue2Flash = value; }
        }

        private IVLControlProperties blue2Blue = null;

        public IVLControlProperties Blue2Blue
        {
            get { return blue2Blue; }
            set { blue2Blue = value; }
        }

        public MotorOffSetSettings()
        {
            IR2IR = new IVLControlProperties();
            IR2IR.name = "IR2IR";
            IR2IR.val = "0";
            IR2IR.type = "int";
            IR2IR.control = "System.Windows.Forms.NumericUpDown";
            IR2IR.text = "IR 2 IR Offset Steps";
            IR2IR.min = -255;
            IR2IR.max = 255;


            IR2Flash = new IVLControlProperties();
            IR2Flash.name = "IR2Flash";
            IR2Flash.val = "25";
            IR2Flash.type = "int";
            IR2Flash.control = "System.Windows.Forms.NumericUpDown";
            IR2Flash.text = "IR 2 Flash Offset Steps";
            IR2Flash.min = -255;
            IR2Flash.max = 255;


            IR2Blue = new IVLControlProperties();
            IR2Blue.name = "IR2Blue";
            IR2Blue.val = "35";
            IR2Blue.type = "int";
            IR2Blue.control = "System.Windows.Forms.NumericUpDown";
            IR2Blue.text = "IR 2 Blue Offset Steps";
            IR2Blue.min = -255;
            IR2Blue.max = 255;


            Flash2IR = new IVLControlProperties();
            Flash2IR.name = "Flash2IR";
            Flash2IR.val = "-100";
            Flash2IR.type = "int";
            Flash2IR.control = "System.Windows.Forms.NumericUpDown";
            Flash2IR.text = "Flash 2 IR Offset Steps";
            Flash2IR.min = -255;
            Flash2IR.max = 255;


            Flash2Flash = new IVLControlProperties();
            Flash2Flash.name = "Flash2Flash";
            Flash2Flash.val = "0";
            Flash2Flash.type = "int";
            Flash2Flash.control = "System.Windows.Forms.NumericUpDown";
            Flash2Flash.text = "Flash 2 Flash Offset Steps";
            Flash2Flash.min = -255;
            Flash2Flash.max = 255;


            Flash2Blue = new IVLControlProperties();
            Flash2Blue.name = "Flash2Blue";
            Flash2Blue.val = "10";
            Flash2Blue.type = "int";
            Flash2Blue.control = "System.Windows.Forms.NumericUpDown";
            Flash2Blue.text = "Flash 2 Blue Offset Steps";
            Flash2Blue.min = -255;
            Flash2Blue.max = 255;



            Blue2IR = new IVLControlProperties();
            Blue2IR.name = "Blue2IR";
            Blue2IR.val = "-100";
            Blue2IR.type = "int";
            Blue2IR.control = "System.Windows.Forms.NumericUpDown";
            Blue2IR.text = "Blue 2 IR Offset Steps";
            Blue2IR.min = -255;
            Blue2IR.max = 255;


            Blue2Flash = new IVLControlProperties();
            Blue2Flash.name = "Blue2Flash";
            Blue2Flash.val = "-10";
            Blue2Flash.type = "int";
            Blue2Flash.control = "System.Windows.Forms.NumericUpDown";
            Blue2Flash.text = "Blue 2 Flash Offset Steps";
            Blue2Flash.min = -255;
            Blue2Flash.max = 255;



            Blue2Blue = new IVLControlProperties();
            Blue2Blue.name = "Blue2Blue";
            Blue2Blue.val = "0";
            Blue2Blue.type = "int";
            Blue2Blue.control = "System.Windows.Forms.NumericUpDown";
            Blue2Blue.text = "Blue 2 Blue Offset Steps";
            Blue2Blue.min = -255;
            Blue2Blue.max = 255;
        }
    }
}
