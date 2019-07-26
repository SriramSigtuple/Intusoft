using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.IO.Ports;
using System.Timers;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using INTUSOFT.EventHandler;
using NLog;
using NLog.Config;
using NLog.Targets;
using System.ComponentModel;
using EEPROM;
using System.Threading.Tasks;
using System.Threading;
using INTUSOFT.Configuration;
namespace INTUSOFT.Imaging
{
    internal class IntucamBoardCommHelper
    {
        //private  readonly ILog log = LogManager.GetLogger(typeof(Camera));
        //private readonly Logger log = LogManager.GetLogger("INTUSOFT.Imaging");// overall log for camera and board
        //private readonly Logger BulkTransferLog = LogManager.GetLogger("INTUSOFT.Imaging.BulkTransferLog");// overall log for camera and board
        //private readonly Logger captureLog = LogManager.GetLogger("INTUSOFT.Imaging.CaptureLog");// to log capture sequence
        private static readonly Logger Exception_Log = LogManager.GetLogger("ExceptionLog");// for exception logging

        public static List<Args> capture_log;// = new List<Args>();
        public static List<Args> BulkLogList;
        public static List<Args> InterruptLogList;
        
        //private  readonly ILog log = LogManager.GetLogger("CameraLogger");
        //private  readonly ILog captureLog = LogManager.GetLogger("CaptureLogger");


        //private  ILog captureLog = LogManager.GetLogger("CaptureFile");
        public bool isCommandSendRecieveActive = false;
        public bool isFlushBulk = false;
        int servoRetryCount = 0;

        public string LogFilePath = "";
        public byte StrobeRate = 0;
        System.Timers.Timer flashOnInterruptTimeoutTimer = new System.Timers.Timer();// Timer to maintain motor event timeout
        System.Timers.Timer flashOffInterruptTimeoutTimer = new System.Timers.Timer();// Timer to maintain motor event timeout
        System.Timers.Timer motorResetDoneTimer = new System.Timers.Timer();// Timer to maintain motor event timeout
        public Thread dataTransferThread;
        public Thread InterruptDataTransferThread;
        private System.Timers.Timer BoardTimer;
        //private MicroTimer _triggerTimer;
       // private MicroTimer BulkTransferTimer;
        public int iterCnt = 2;
        public int convertInt = 0;
        int EEPromPageLength = 64;
        public byte imagingMode = 0;
        public delegate void FlashOffDoneEvent(EventArgs e);

        public delegate void UpdateAnalogInt2DigitalInt(int val);
        public event UpdateAnalogInt2DigitalInt _updateAnalogInt2DigitalInt;
        public delegate void GetTrigger(bool isTrigger, EventArgs e);
        public event GetTrigger triggerRecieved;
        public delegate void Capture(string[] val, EventArgs e);
        public event Capture _CaptureEvent;
        public delegate void LeftRightEvent(bool isLeft);
        public event LeftRightEvent _leftRightEvent;
        public delegate void MotorSensorPositionEvent(int SensorPosition, int resetStepVal);
        public event MotorSensorPositionEvent _motorSensorPositionEvent;
        public delegate void IRStatus(bool status, string s);
        public delegate void FlashOffDone();
        public event FlashOffDone flashDoneEvent;
        public bool isMotorForward = false;
        public int FrameCnt = 0;
        public bool isTriggerRecieved = false;
        ThreadStart ts1;// = new ThreadStart(ReadInterruptData);
        ThreadStart ts;// = new ThreadStart(SendBoardData);
        public Args[] BulkTransferQueue = new Args[20];
        private int bulkIndex = 0;

        public int BulkIndex
        {
            get
            { return bulkIndex; }
            set 
            {
                bulkIndex = value; 
            }
        }
        public bool isCameraConnected = false;
        public bool isStrobeConnected = false;// state variable to check strobe connection
        public string returnVal = "";
        private bool isOpen = false;

        private bool isDevicePresent = false;
        public bool isStartTransferData = false;
        public bool isStartInterruptTransferData = false;
        public bool isClearPending = false;
        public bool IsDevicePresent
        {
            get {
                UInt16[] pids = new ushort[1];
                UInt16[] vids = new ushort[1];
                IS_USB_Device_Present(ref isDevicePresent,pids,vids);
                for (int i = 0; i < pids.Length; i++)
			{

                Args logArg = new Args();
                logArg["TimeStamp"] = DateTime.Now;
                logArg["Msg"] = pids[i].ToString() + vids[i].ToString();
                 BulkLogList.Add(logArg);
                //IVLCamVariables.logClass. BulkLogList.Add(pids[i].ToString() + vids[i].ToString());
			 
			}
                return isDevicePresent; 
            }
        }
        public bool IsOpen
        {
            get {
                bool prevOpenState = isOpen;
                //if (IsDevicePresent)
                    USBIsOpen(ref isOpen);
                    if (!isOpen && prevOpenState)
                    {
                        isStartInterruptTransferData = false;
                        Close();
                    }//else
                    //isOpen = false;
                return isOpen; 
            }
        }
        public bool isEnableLeftRightSensor = false;
        public bool isReverseMotorPositionSensor = false;

        /* For Enabling or Disabling USB Functionality */
        public bool USBMode_Active = true;
        public byte[] USB_DATA_RECEIVED = new byte[25];
        public byte[] EEPROM_DATA_ARRAY = new byte[70];

        public const int DbtDevicearrival = 0x8000; // system detected a new device        
        public const int DbtDeviceremovecomplete = 0x8004; // device is gone      
        public const int WmDevicechange = 0x0219; // device change event  
        public DateTime Flash_On_Time;
        public DateTime Flash_Off_Time;
        public string isIRStatus = "";
        public int MotorZeroD_OffsetValue = 0;
        public int StartFlashOn = 0;
        public EventArgs e = null;
        //public  int Power_Stat_Count = 0;
        //public  int Power_Stat_Count_Prev = -1;
        public static IntucamBoardCommHelper boardHelper;
        string CommandString = "";
        string ValueString = "000000";

        public uint timeout = 20;
        public uint interruptTimeout = 500;
        public uint writeTimeout = 50;
        public uint readTimeout = 50; 
        /************ DLL Imports For Usb Functionality **************/
        [DllImport("IntuSoftUSBController.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "USBControllerInit")]
        private static extern void USBControllerInit();

        [DllImport("IntuSoftUSBController.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "USBOpen")]
        private static extern void USBOpen(ref bool isSucess, ref int err_Code);

        [DllImport("IntuSoftUSBController.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "USBClose")]
        private static extern void USBClose();

        [DllImport("IntuSoftUSBController.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "USBIsOpen")]
        private static extern void USBIsOpen(ref bool isUsbOpen);

        [DllImport("IntuSoftUSBController.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "IS_USB_Device_Present")]
        private static extern void IS_USB_Device_Present(ref bool isDevicePresent,UInt16[] pids,UInt16[] vids);

        [DllImport("IntuSoftUSBController.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "USB_Read_Write")]
        private static extern void USB_Read_Write(byte[] WriteData, byte[] ReadData, ref bool isSucess, ref int err_Code, uint readTimeout, uint writeTimeout);//, ref int Read_Err_Code, ref int Write_Err_Code, byte[] DataArr, int DataArrLen);

        [DllImport("IntuSoftUSBController.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "USB_Write")]
        private static extern void USB_Write(byte[] WriteData, ref bool isSucess, ref int WriteErr_Code,uint writeTimeout);

        [DllImport("IntuSoftUSBController.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "USB_Read")]
        public static extern void USB_Read(byte[] ReadData, ref bool isSucess, ref int ReadErr_Code,uint readTimeout);

        [DllImport("IntuSoftUSBController.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "USB_Read_Write4")]
        private static extern void USB_Read_Write(byte[] WriteData, byte[] ReadData, ref bool ReadisSucess, ref bool WriteisSucess, ref int err_Code, ref int Read_Err_Code, ref int Write_Err_Code,uint readTimeout, uint writeTimeout);

        [DllImport("IntuSoftUSBController.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "USBReadInterrupt")]
        private static extern void USBReadInterrupt(ref bool isSucess, ref int err_Code, ref int interrupt_code, byte[] intData,uint interruptTimeout);

        [DllImport("IntuSoftUSBController.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "IsBoardPresent")]
        public static extern bool IsBoardPresent();
        /***********************************************************/


        public static IntucamBoardCommHelper GetInstance()
        {
            try
            {
                if (boardHelper == null)
                    boardHelper = new IntucamBoardCommHelper();
            }
            catch (Exception ex)
            {
                 CameraLogger.WriteException(ex, Exception_Log);
            }

            return boardHelper;
        }
        public IntucamBoardCommHelper()
        {
            capture_log = new List<Args>();
            BulkLogList = new List<Args>();
            InterruptLogList = new List<Args>();
            for (int i = 0; i < BulkTransferQueue.Length; i++)
            {
                BulkTransferQueue[i] = new Args();
            }
            BulkIndex = 0;
            USBControllerInit();// Initialize the usb communication methods 
            ts = new ThreadStart(SendBoardData);
            ts1 = new ThreadStart(ReadInterruptData);
        }





        public void CreateInterruptTimer()
        {
            try
            {
                long interruptTimerInterval = IVLCamVariables._Settings.BoardSettings.InterruptTimeInterval;
                IVLCamVariables._eventHandler = EventHandler.IVLEventHandler.getInstance();
                if (flashOnInterruptTimeoutTimer== null)
                flashOnInterruptTimeoutTimer = new System.Timers.Timer();
                flashOnInterruptTimeoutTimer.Elapsed += flashOnInterruptTimeoutTimer_Elapsed;
                if (flashOffInterruptTimeoutTimer == null)
                flashOffInterruptTimeoutTimer = new System.Timers.Timer();
                flashOffInterruptTimeoutTimer.Elapsed += flashOffInterruptTimeoutTimer_Elapsed;
                if(IVLCamVariables._Settings == null)
                    IVLCamVariables._Settings = CameraModuleSettings.GetInstance();
            }
            catch (Exception ex)
            {
                 CameraLogger.WriteException(ex, Exception_Log);
            }

        }

        /// <summary>
        /// If the Flash off interrupt timer elapsed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void flashOffInterruptTimeoutTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (IVLCamVariables.IsFlashOnDone && !IVLCamVariables.IsFlashOffDone)
                {
                    if (!IVLCamVariables.isCaptureFailure)
                    {
                        flashOffInterruptTimeoutTimer.Enabled = false;
                        flashOffInterruptTimeoutTimer.Stop();
                        flashOffInterruptTimeoutTimer.Elapsed -= flashOffInterruptTimeoutTimer_Elapsed;
                        flashOnInterruptTimeoutTimer.Interval = 1000000000;
                        IVLCamVariables.captureFailureCode = CaptureFailureCode.FlashOffNotReceived;
                        IVLCamVariables.isCaptureFailure = true;
                        IVLCamVariables.intucamHelper.TriggerOn();
                        IVLCamVariables.intucamHelper.CompleteCaptureSequence();

                        //IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.SaveFrames2Disk, new Args());
                    }
                }
            }
            catch (Exception ex)
            {
                 CameraLogger.WriteException(ex, Exception_Log);
            }
            
        }
        internal void ResumeLiveBoardCommands()
        {
            GetMotorPosition();
            if (IVLCamVariables.ImagingMode == ImagingMode.FFA_Plus || IVLCamVariables.ImagingMode == ImagingMode.FFAColor)
            {
                if (IVLCamVariables._Settings.BoardSettings.EnablePCU_IntensityControl)
                    Read_LED_SupplyValues();
                IVLCamVariables.intucamHelper.UpdateExposureGainFromTable(IVLCamVariables._Settings.CameraSettings.LiveGainIndex, false);

            }
            else if (IVLCamVariables.ImagingMode == ImagingMode.Posterior_45)
            {
                if (IVLCamVariables._Settings.BoardSettings.EnablePCU_IntensityControl)
                    Read_LED_SupplyValues();
                else
                    IVLCamVariables.intucamHelper.UpdateExposureGainFromTable(IVLCamVariables._Settings.CameraSettings.LiveGainIndex, false);
            }
            else
            {
                IVLCamVariables.intucamHelper.ivl_Camera.SetGain(IVLCamVariables._Settings.CameraSettings.LiveGain);
            }
            IVLCamVariables.intucamHelper.LEDSource = (Led)Enum.Parse(typeof(Led), ConfigVariables.CurrentSettings.CameraSettings.LiveLedSource.val);
      
        
        }
        /// <summary>
        /// If the Flash on interrupt timer elapsed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void flashOnInterruptTimeoutTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!IVLCamVariables.IsFlashOnDone)
                {
                    if (!IVLCamVariables.isCaptureFailure)
                    {
                        flashOnInterruptTimeoutTimer.Enabled = false;
                        flashOnInterruptTimeoutTimer.Stop();
                        flashOnInterruptTimeoutTimer.Interval = 1000000000;
                        IVLCamVariables.captureFailureCode = CaptureFailureCode.FlashOnNotReceived;
                        IVLCamVariables.isCaptureFailure = true;
                        IVLCamVariables.intucamHelper.TriggerOn();
                        IVLCamVariables.intucamHelper.CompleteCaptureSequence();

                        //IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.SaveFrames2Disk, new Args());
                    }
                }
            }
            catch (Exception ex)
            {
                CameraLogger.WriteException(ex, Exception_Log);
            }
            
        }
         void SendBoardData()
        {
            while (isStartTransferData)
            {
               // token.ThrowIfCancellationRequested();
                //if (isClearPending)
                //{
                //    if (!isCommandSendRecieveActive && IVLCamVariables.IsMotorMoved)
                //    {
                //        BulkQueueClearPending();
                //        isClearPending = false;
                //    }
                //}
                //else
                    {
                         BulkTransfer();
                    }
                    Thread.Sleep(5);
            }
        }
        bool isInterruptReading = false;
        public void ReadInterruptData()
        {
            try
            {
                if (!IVLCamVariables.isApplicationClosing)// read interrupts only if the application is not in closing state by sriram
                {
                    while (isStartInterruptTransferData)
                    {

                        bool isSuccess_intr = false;
                        int errCode_intr = 0;

                        int interrupt_code = 0;
                        byte[] intData = new byte[70];
                        //IS_USB_Device_Present(ref isUsbOpen);
                        // if (isUsbOpen && isOpen)// This is commented no check for USB open is done when interrupt timer is ticked.
                        {
                            //Args readLog = new Args();

                            //readLog["TimeStamp"] = DateTime.Now;
                            //readLog["Msg"] = "I_I_M = Start reading interrupt= " + interrupt_code + "error code   " + errCode_intr;
                            ////readLog["callstack"] = Environment.StackTrace;
                            ////triggerRecieved(true, e);
                            //if (IVLCamVariables.isCapturing)
                            //    capture_log.Add(readLog);

                            //InterruptLogList.Add(readLog);
                            USBReadInterrupt(ref isSuccess_intr, ref errCode_intr, ref interrupt_code, intData, interruptTimeout);

                            if (isSuccess_intr)
                            {
                                string str = "";
                                InterruptCode retIntCode = (InterruptCode)interrupt_code;// convert return interrupt code from the microcontroller to enulkjadm in c#
                                /* Check Which Interrupt Has occurred */
                                switch (retIntCode)
                                {
                                    case InterruptCode.TriggerPressed:
                                        /* Trigger Pressed */
                                        {
                                            Args logArg = new Args();

                                            logArg["TimeStamp"] = DateTime.Now;
                                            logArg["Msg"] = "I_I_M = Trigger recieved  interrupt code = " + retIntCode + "error code   " + errCode_intr;
                                            //logArg["callstack"] = Environment.StackTrace;
                                            str = string.Format("I_I_M = Trigger recieved  interrupt code ={0} error code {1}  ", retIntCode, errCode_intr);
                                            //triggerRecieved(true, e);
                                           // ThreadPool.QueueUserWorkItem(new WaitCallback(TriggerRecievedCallBack));
                                            IVLCamVariables.intucamHelper.TriggerRecieved();
                                            if (IVLCamVariables.isCapturing)
                                                capture_log.Add(logArg);

                                            InterruptLogList.Add(logArg);
                                            //IVLCamVariables.intucamHelper.totalFrameCount_Lbl.Text = "0";

                                        }
                                        break;
                                    case InterruptCode.MotorFastForward:
                                        /* Motor Fast Forward Done */
                                        {
                                                Args logArg = new Args();

                                                logArg["TimeStamp"] = DateTime.Now;
                                                logArg["Msg"] = "I_I_M = Motor Forward done interrupt code = " + retIntCode + "error code   " + errCode_intr;
                                                //logArg["callstack"] = Environment.StackTrace;
                                                InterruptLogList.Add(logArg);
                                                if (IVLCamVariables.isCapturing)
                                                    capture_log.Add(logArg);
                                                if (!IVLCamVariables.IsMotorMoved)// The motor movement is called only if the timeout hasn't occured if the interrupt is not already read
                                                {
                                                    IVLCamVariables.IsMotorMoved = true;
                                                    if (IVLCamVariables.isCapturing && !IVLCamVariables.isResuming)
                                                        CaptureCommand();
                                                    else
                                                    {
                                                        ThreadPool.QueueUserWorkItem(new WaitCallback(MotorMovementDoneCallBack));
                                                        //       IVLCamVariables.intucamHelper.MotorMovementDone();
                                                    }
                                                }
                                          }
                                        break;

                                    case InterruptCode.MotorFastBackward:
                                        /* Motor Fast Backward Done */
                                        //if(_mfbkDoneEvent != null)
                                        {

                                            Args logArg = new Args();

                                            logArg["TimeStamp"] = DateTime.Now;
                                            logArg["Msg"] = "I_I_M = Motor Backward done interrupt code = " + retIntCode + "error code   " + errCode_intr;
                                            //logArg["callstack"] = Environment.StackTrace;
                                            //str = string.Format("I_I_M =, Motor Backward done recieved Time = {0}, interrupt code = {1}, error code {2}, motorMovement Done Status = {3} isCapturing = {4}, isResuming ={5}", DateTime.Now.ToString("HH-mm-ss-fff"), retIntCode, errCode_intr, IVLCamVariables.IsMotorMoved, IVLCamVariables.isCapturing, IVLCamVariables.isResuming );
                                            IVLCamVariables.IsMotorMoved = true;

                                            if (IVLCamVariables.isCapturing)
                                                capture_log.Add(logArg);

                                            InterruptLogList.Add(logArg);
                                            if (IVLCamVariables.isCapturing && !IVLCamVariables.isResuming)
                                                CaptureCommand();
                                            else
                                            {
                                                ThreadPool.QueueUserWorkItem(new WaitCallback(MotorMovementDoneCallBack));
                                         
                                                //       IVLCamVariables.intucamHelper.MotorMovementDone();
                                            }
                                        }
                                        break;
                                    case InterruptCode.FlashOnDone:// Flash on done interrupt 
                                        {
                                            int Hours = intData[4];
                                            int Mins = intData[5];
                                            int Secs = intData[6];
                                            int Msecs = intData[7] * 256 + intData[8];
                                            string str1 = Hours + ":" + Mins + ":" + Secs + ":" + Msecs;
                                            IVLCamVariables.FlashOnTime = new DateTime(1, 1, 1, Hours, Mins, Secs, Msecs);
                                            int FrameIndx = 0;
                                            str = string.Format("I_I_M =, Flash On done recieved Time = {0}, interrupt code= {1}, error code {2} Time ={3},capture Index = {4} ,Flash on Done Time ={5} ", DateTime.Now.ToString("HH-mm-ss-fff"), retIntCode, errCode_intr, str1, FrameIndx,IVLCamVariables.FlashOnTime);
                                            Args logArg = new Args();

                                            logArg["TimeStamp"] = DateTime.Now;
                                            logArg["Msg"] = "I_I_M = Flash On done recieved  interrupt code = " + retIntCode +"Flash on Done Time = "+IVLCamVariables.FlashOnTime.ToString("HH-mm-ss-fff")+ " error code   " + errCode_intr + "capture Index = " + FrameIndx;
                                            //logArg["callstack"] = Environment.StackTrace;
                                            if (IVLCamVariables.isCapturing)
                                                //IVLCamVariables.logClass.CaptureLogList.Add(str);
                                                capture_log.Add(logArg);
                                            {
                                                //IVLCamVariables.CameraLogList.Add(logArg);
                                                //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);
                                                InterruptLogList.Add(logArg);
                                            }
                                            if (isCptrRecieved)
                                            {
                                                // this is commented to avoid start saving frames to be enabled from flash on done instead of capture command from bulk transfer by sriram 08 march 2017
                                                IVLCamVariables.IsFlashOnDone = true;
                                                flashOnInterruptTimeoutTimer.Enabled = false;
                                                flashOnInterruptTimeoutTimer.Stop();
                                                //flashOnInterruptTimeoutTimer.Elapsed -= flashOnInterruptTimeoutTimer_Elapsed;

                                                flashOffInterruptTimeoutTimer.Interval = 6 * IVLCamVariables.StrobeValue;
                                                flashOffInterruptTimeoutTimer.Enabled = true;
                                                flashOffInterruptTimeoutTimer.Start();
                                                //i.StartStopFlashInterruptTimoutTimer(IVLCamVariables._Settings.BoardSettings.flashOnDoneStrobeCycleValue, false);// to stop flash on done time out timer
                                                //i.StartStopFlashInterruptTimoutTimer(IVLCamVariables._Settings.BoardSettings.flashOffDoneStrobeCycleValue * IVLCamVariables.StrobeValue, true);// to stop flash off done time out timer

                                            }
                                            else
                                            {

                                                // failure case strobe irregular
                                                IVLCamVariables.captureFailureCode = CaptureFailureCode.CaptureCommandNotReceived;
                                                IVLCamVariables.isCaptureFailure = true;
                                                
                                                    IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.SaveFrames2Disk, new Args());


                                            }
                                            break;
                                        }
                                    case InterruptCode.FlashOffDone:// Flash of done interrupt 
                                        {
                                            string str1 = "";
                                            Args logArg = new Args();

                                            if (IVLCamVariables.IsFlashOnDone)
                                            {
                                                flashOffInterruptTimeoutTimer.Enabled = false;
                                                flashOffInterruptTimeoutTimer.Stop();
                                                IVLCamVariables.IsFlashOffDone = true;

                                                //if ((IVLCamVariables.CaptureImageIndx = IVLCamVariables.FrameBetweenFlashOnFlashOffDoneCnt) == 0)
                                                //    IVLCamVariables.CaptureImageIndx = -1;
                                                //i.StartStopFlashInterruptTimoutTimer(IVLCamVariables._Settings.BoardSettings.flashOffDoneStrobeCycleValue * IVLCamVariables.StrobeValue, false);// to stop flash off done time out timer
                                                int Hours = intData[4];
                                                int Mins = intData[5];
                                                int Secs = intData[6];
                                                int Msecs = intData[7] * 256 + intData[8];
                                                str1 = Hours + ":" + Mins + ":" + Secs + ":" + Msecs;
                                                IVLCamVariables.FlashOffTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Hours, Mins, Secs, Msecs);
                                                TimeSpan t = IVLCamVariables.FlashOffTime - IVLCamVariables.FlashOnTime;

                                                logArg["TimeStamp"] = DateTime.Now;
                                                logArg["Msg"] = "I_I_M = Flash Off done recieved  interrupt code = " + retIntCode +"Flash on Done Time = " + IVLCamVariables.FlashOffTime.ToString("HH-mm-ss-fff") +"Flash off to on time Diff = "+t.Milliseconds.ToString()+ " error code   " + errCode_intr;
                                                //logArg["callstack"] = Environment.StackTrace;
                                               
                                            }
                                            str = string.Format("I_I_M =, Flash Off done recieved Time = {0}, interrupt code= {1}, error code {2} Time ={3}  ", DateTime.Now.ToString("HH-mm-ss-fff"), retIntCode, errCode_intr, str1);
                                            if (IVLCamVariables.isCapturing)
                                                //IVLCamVariables.logClass.CaptureLogList.Add(str);
                                                capture_log.Add(logArg);

                                            //IVLCamVariables.CameraLogList.Add(logArg);
                                            InterruptLogList.Add(logArg);
                                            //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);

                                        }
                                        break;
                                    case InterruptCode.RotaryDone: // Interrupts when a rotary movement is done
                                        {
                                            Args logArg = new Args();

                                            logArg["TimeStamp"] = DateTime.Now;
                                            logArg["Msg"] = "I_I_M = Rotary movement done recieved  interrupt code = " + retIntCode + "error code   " + errCode_intr;

                                            if (!IVLCamVariables.isCapturing)
                                            {
                                                InterruptLogList.Add(logArg);
                                                //ThreadPool.QueueUserWorkItem(new WaitCallback(GetMotorPositionCallBack));
                                                GetMotorPosition();
                                            }
                                            BulkLogList.Add(logArg);
                                            break;
                                        }
                                    case InterruptCode.LR_Event: //Interrupt to receive the left right event.
                                        {
                                            if (IVLCamVariables._Settings.BoardSettings.EnableRightLeftSensor)
                                            {
                                                byte[] arr = new byte[5];
                                                arr[0] = intData[2];
                                                arr[1] = intData[3];
                                                string leftRightInterruptData = Encoding.UTF8.GetString(arr);
                                                if (leftRightInterruptData.ToUpper().Contains("LE"))
                                                    IVLCamVariables.intucamHelper.leftRightEvent(true);

                                                else if (leftRightInterruptData.ToUpper().Contains("RE"))
                                                    IVLCamVariables.intucamHelper.leftRightEvent(false);

                                            }
                                            break;
                                        }
                                    case InterruptCode.MotorResetDone: // Interrupts when a Motor Reset  movement is done
                                        {
                                            Args logArg = new Args();

                                            logArg["TimeStamp"] = DateTime.Now;
                                            logArg["Msg"] = "I_I_M = Motor Reset movement done recieved  interrupt code = " + retIntCode + "error code   " + errCode_intr;
                                            //logArg["callstack"] = Environment.StackTrace;

                                            str = string.Format("I_I_M = Motor Reset movement Done  interrupt code ={0} error code {1}  ", retIntCode, errCode_intr);
                                            IVLCamVariables.IsMotorMoved = true;
                                            InterruptLogList.Add(logArg);
                                            //IVLCamVariables.CameraLogList.Add(logArg);
                                            //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);

                                            ThreadPool.QueueUserWorkItem(new WaitCallback(MotorMovementDoneCallBack));
                                            //IVLCamVariables.intucamHelper.MotorMovementDone();
                                            // _motorResetEvent();
                                            break;
                                        }


                                    case InterruptCode.Camera_Arrived:
                                        {
                                            str = string.Format("I_I_M = Camera Connected  interrupt code ={0} error code {1}  ", retIntCode, errCode_intr);
                                            //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);
                                            Args logArg = new Args();

                                            logArg["TimeStamp"] = DateTime.Now;
                                            logArg["Msg"] = string.Format("I_I_M = Camera Connected  interrupt code ={0} error code {1}  ", retIntCode, errCode_intr);
                                            //logArg["callstack"] = Environment.StackTrace;
                                            //IVLCamVariables.CameraLogList.Add(logArg);
                                            IVLEventHandler Handler = IVLEventHandler.getInstance();
                                            Args arg = new Args();
                                            arg["isCameraConnected"] = true;
                                            // Handler.Notify(Handler.CAMERA_DISCONNECTED, arg);
                                            break;
                                        }
                                    case InterruptCode.Camera_Removed:
                                        {
                                            str = string.Format("I_I_M = Camera removed  interrupt code ={0} error code {1}  ", retIntCode, errCode_intr);
                                            //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);
                                            Args logArg = new Args();

                                            logArg["TimeStamp"] = DateTime.Now;
                                            logArg["Msg"] = string.Format("I_I_M = Camera removed  interrupt code ={0} error code {1}  ", retIntCode, errCode_intr);
                                            //logArg["callstack"] = Environment.StackTrace;
                                            //IVLCamVariables.CameraLogList.Add(logArg);
                                            IVLEventHandler Handler = IVLEventHandler.getInstance();
                                            Args arg = new Args();
                                            arg["isCameraConnected"] = false;
                                            // Handler.Notify(Handler.CAMERA_DISCONNECTED, arg);
                                            break;
                                        }
                                    case InterruptCode.PotIntensityChanged: // Interrupts when Intensity POT is varied in the PCU
                                        {

                                            if (!IVLCamVariables.isCapturing && IVLCamVariables.isLive)//In order to fix the defect 0001776 if capturing is not in progress and it is in live then only read pot value by sriram
                                            {
                                             //   Read_LED_SupplyValues();
                                                    int val = -1;// bulk transfer not working and to software value
                                                    val = Convert.ToInt32(intData[4]);
                                                    IVLCamVariables.intucamHelper.updateAnalogInt2DigitalInt(val);
                                            
                                            }
                                            Args logArg = new Args();

                                            logArg["TimeStamp"] = DateTime.Now;
                                            logArg["Msg"] = "I_I_M = POT Intensity Varied  interrupt code = " + retIntCode + "error code   " + errCode_intr;
                                            //logArg["callstack"] = Environment.StackTrace;

                                            str = string.Format("I_I_M =, POT Intensity Varied Time = {0}, interrupt code= {1}, error code {2}  ", DateTime.Now.ToString("HH-mm-ss-fff"), retIntCode, errCode_intr);
                                            if (IVLCamVariables.isCapturing)
                                            {
                                                //captureLog.Debug("I_I_M = POT Intensity Varied  interrupt code ={0} error code {1}", retIntCode, errCode_intr);
                                                //IVLCamVariables.logClass.CaptureLogList.Add(str);
                                                capture_log.Add(logArg);
                                            }
                                            //IVLCamVariables.CameraLogList.Add(logArg);
                                            InterruptLogList.Add(logArg);
                                            //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);

                                            break;
                                        }

                                    default:
                                        {
                                            Args logArg = new Args();

                                            logArg["TimeStamp"] = DateTime.Now;
                                            logArg["Msg"] = "I_I_M = Others_  interrupt code = " + retIntCode + "error code   " + errCode_intr;
                                            ////logArg["callstack"] = Environment.StackTrace;

                                            //str = string.Format("I_I_M =, Others_ Time = {0}, interrupt code= {1}, error code {2}  ", DateTime.Now.ToString("HH-mm-ss-fff"), retIntCode, errCode_intr);
                                            //if (IVLCamVariables.isCapturing)
                                            //    capture_log.Add(logArg);

                                            //{
                                            //    //IVLCamVariables.logClass.CaptureLogList.Add(str);
                                            //    InterruptLogList.Add(logArg);
                                            //    //    //IVLCamVariables.CameraLogList.Add(logArg);
                                            //    //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);
                                            //}
                                            break;

                                        }
                                }
                            }

                        }
                        Thread.Sleep(10);
                    }
                }

            }
            catch (ThreadAbortException ex)
            {
                int x = 0;
            }

            catch (Exception ex)
            {
                CameraLogger.WriteException(ex, Exception_Log);
            }

        }

        private void GetMotorPositionCallBack(object callback)
        {
            GetMotorPosition();
        }
        private void MotorMovementDoneCallBack(object callback)
        {
            IVLCamVariables.intucamHelper.MotorMovementDone();
        }

        private void TriggerRecievedCallBack(object callBack)
        {
            //triggerRecieved(true, e);
            IVLCamVariables.intucamHelper.TriggerRecieved();
        
        }
        public bool Open()
        {
            #region earlier implementation of com port opening
            //try
            //{
            //    ManagementObjectCollection collection;
            //    comPort = new SerialPort();
            //    using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_SerialPort"))
            //        collection = searcher.Get();
            //    foreach (var item in collection)
            //    {
            //        string details = item.GetPropertyValue("Description").ToString();
            //        if (details.Contains("Silicon Labs"))
            //        {
            //            comPort.PortName = item.GetPropertyValue("DeviceID").ToString();
            //            break;
            //        }
            //    }
            //    if (!string.IsNullOrEmpty(comPort.PortName))
            //    {
            //        comPort.Open();
            //        comPort.ReadTimeout = 1000;
            //        comPort.BaudRate = 9600;
            //        comPort.RtsEnable = true;
            //    }
            //}

            //catch (Exception ex)
            //{
            //    Console.Write(ex.Message);
            //}
            #endregion
            try
            {
                bool isSuccess = false;
                int errCode = 0;
                
                USBOpen(ref isSuccess, ref errCode);
                Args logArg = new Args();

                logArg["TimeStamp"] = DateTime.Now;
                logArg["Msg"] = string.Format("Time = {0} , Board Open, Success ={1}, Error Code ={2}  ", DateTime.Now.ToString("HH-mm-ss-fff"), isSuccess, errCode);
                BulkLogList.Add(logArg);
                string logValue = string.Format("Time = {0} , Board Open, Success ={1}, Error Code ={2}  ", DateTime.Now.ToString("HH-mm-ss-fff"),isSuccess, errCode );
                 //IVLCamVariables.CameraLogList.Add(logArg);
                //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(logValue);

                if (isSuccess)
                {

                    /* Usb Device Is opened succesfully*/
                    isOpen = true;
                    CreateInterruptTimer();
                    isStartTransferData = true;
                    isStartInterruptTransferData = true;
                    if(dataTransferThread == null || dataTransferThread.ThreadState != ThreadState.Running || !dataTransferThread.IsAlive)
                        dataTransferThread = new Thread(ts);
                    if (InterruptDataTransferThread == null || InterruptDataTransferThread.ThreadState != ThreadState.Running || !InterruptDataTransferThread.IsAlive)
                        InterruptDataTransferThread = new Thread(ts1);

                    for (int i = 0; i < BulkTransferQueue.Length; i++)
                    {
                        BulkTransferQueue[i] = new Args();//arg; 
                    }
                    bulkIndex = 0;
                    addIndex = 0;
                    if (IVLCamVariables.ImagingMode == ImagingMode.FFA_Plus)// if the servo was moved and power got disconnected and reconnected then move the servo back to reset if the live mode light was not blue
                    {
                        GreenFilterMove(IVLCamVariables._Settings.BoardSettings.GreenFilterPos);
                        BulkTransfer();
                        IVLCamVariables.isBlueFilteredMoved = false;
                    }
                    GetFirmwareVersion();// To get firmware version when the board is opened to fix defect 0001826
                    Args logArg1 = new Args();

                    logArg1["TimeStamp"] = DateTime.Now;
                    logArg1["Msg"] = "Board opened = " + isSuccess;
                    //IVLCamVariables.logClass. BulkLogList.Add(string.Format("Time = {0}, Board opened={1}", DateTime.Now.ToString("HH-mm-ss-fff"), isSuccess));
                    BulkLogList.Add(logArg1);

                    dataTransferThread.Priority = ThreadPriority.Normal;
                    dataTransferThread.IsBackground = true;// this is done in order to stop the thread when the application is exited.

                    dataTransferThread.Start();
                    
                    InterruptDataTransferThread.Priority = ThreadPriority.Normal;
                    InterruptDataTransferThread.IsBackground = true;// this is done in order to stop the thread when the application is exited.

                    InterruptDataTransferThread.Start();
                    // Set Flash offset from the config to the board when the board is opened 
                    byte[] arr = { Convert.ToByte(ConfigVariables.CurrentSettings.FirmwareSettings._FlashOffsetStart.val), Convert.ToByte(ConfigVariables.CurrentSettings.FirmwareSettings._FlashOffsetEnd.val) };
                    IVLCamVariables._Settings.BoardSettings.CaptureStartOffset = arr[0];
                    IVLCamVariables._Settings.BoardSettings.CaptureEndOffset = arr[1];
                    FlashOffset(arr);

                    // Set whether single frame capture is enabled when the board is opened
                    IVLCamVariables._Settings.BoardSettings.IsSingleFrameCapture = Convert.ToBoolean(ConfigVariables.CurrentSettings.FirmwareSettings._EnableSingleFrameCapture.val);
                    if (IVLCamVariables._Settings.BoardSettings.IsSingleFrameCapture)
                        IVLCamVariables.BoardHelper.SetFullFrameCapture(1);
                    else
                        IVLCamVariables.BoardHelper.SetFullFrameCapture(0);
                    //if (IVLCamVariables.liveLedCode != LedCode.Blue)// if the servo was moved and power got disconnected and reconnected then move the servo back to reset if the live mode light was not blue
                    
                   // powerStatusUpdate();
                    //IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.Power_CONNECTED, arg);
                }
                else
                {
                    // _triggerTimer.MicroTimerElapsed -= _triggerTimer_MicroTimerElapsed;

                    isOpen = false;
                    //powerStatusUpdate();
                    //IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.Power_CONNECTED, arg);// notify the UI about the board not open or disconnected usb
                }
            }
            catch (Exception ex)
            {
                 CameraLogger.WriteException(ex, Exception_Log);
            }

            return isOpen;

        }

        void BulkTransferTimer_MicroTimerElapsed(object sender, MicroTimerEventArgs timerEventArgs)
        {
            //if(bulkTransferTimerRunning)//Checks for the bool Bulk transfer timer running is true or false to proceed for the bulk transfer.
            //    BulkTransfer();
        }

       internal  void BulkTransfer()
        {

            try
            {

               // while (true)
                {
                   
                    //token.ThrowIfCancellationRequested();
                     if (!isCommandSendRecieveActive && IVLCamVariables.IsMotorMoved)
                    {
                        if (isFlushBulk)
                        {
                            BulkQueueClearPending();
                            isFlushBulk = false;
                        }
                        else
                        {
                            if (BulkIndex >= 0)
                            {
                                if (BulkTransferQueue[BulkIndex].Count > 0) //checks for the count bulk transfer queue is greater than zero.
                                {
                                    isCommandSendRecieveActive = true;

                                    doMethod();
                                    //System.Threading.ThreadStart ts = new System.Threading.ThreadStart(doMethod);

                                    //System.Threading.Thread t = new System.Threading.Thread(ts);
                                    //t.Start();
                                }
                                else
                                {
                                    //if (BulkIndex != BulkTransferQueue.Length - 1)
                                    //    BulkIndex += 1;
                                    //else
                                    //    BulkIndex = 0;
                                }
                            }
                        }
                    }
                 //   await Task.Delay(100, token);


                }
            }
            catch (Exception ex)
            {

                 CameraLogger.WriteException(ex, Exception_Log);

            }
           
        }

        /// <summary>
        /// To transfer Bulk data
        /// </summary>
       private void doMethod()
       {
           try
           {
                   Args arg = new Args();
                       for (int i = 0; i < BulkTransferQueue[BulkIndex].Count; i++)
                       {

                           KeyValuePair<string, object> keyVal = BulkTransferQueue[BulkIndex].ElementAt(i);
                           arg.Add(keyVal.Key, keyVal.Value);
                       }
                       DoBulkTransferThreadWork(arg);

                       Args logArg1 = new Args();
                       
                       logArg1["TimeStamp"] = DateTime.Now;
                       logArg1["Msg"] = "Bulk Queue Count = " + BulkIndex.ToString();
                       BulkLogList.Add(logArg1);
                      
                       //IVLCamVariables.logClass. BulkLogList.Add(str);
                       BulkTransferQueue[BulkIndex] = new Args();

                       if (BulkIndex == BulkTransferQueue.Length - 1)
                       {
                           BulkIndex = 0;// Reset of bulk Index

                       }
                       else
                       {
                           BulkIndex += 1;// Increment Bulk Index to next index

                       }//BulkTransferQueue.RemoveAt(0);
                       isCommandSendRecieveActive = false;

           }
           catch (Exception ex)
           {

               CameraLogger.WriteException(ex, Exception_Log);
           }
           
       }
        public void Close()
        {
            try
            {

                isStartInterruptTransferData = false;
                isStartTransferData = false;
                Args logArg = new Args();
                logArg["TimeStamp"] = DateTime.Now;
                logArg["Msg"] = "Board closed";
                 BulkLogList.Add(logArg);
                 FlushBulkBuffer();
                //IVLCamVariables.logClass. BulkLogList.Add(string.Format("Time = {0}, Board closed",DateTime.Now.ToString("HH-mm-ss-fff")));
                 if (isOpen)
                 {
                     ResetBoard();
                     BulkTransfer();
                    while (isCommandSendRecieveActive)
                    {
                        ;
                    }
                }
                 for (int i = 0; i < BulkTransferQueue.Length; i++)
                 {
                     BulkTransferQueue[i] = new Args();
                 }
                 bulkIndex = 0;
                 addIndex = 0;
                 Thread.Sleep(2000);// Added to handle the clearing of all the interrupt reads 
                USBClose();
            }
            catch (Exception ex)
            {
                 CameraLogger.WriteException(ex, Exception_Log);
            }

        }

        public void ReadEEPROMStructureDetails()
        {
            try
            {
                eepromRead(1);
                byte[] byteArr = new byte[EEPROM_Version_Details_Page.pageSize];

                IVLCamVariables.eepromByteArrList = new List<byte[]>();

                byte[] EEPROMVersionArr = new byte[3];
                Array.Copy(EEPROM_DATA_ARRAY, 0, EEPROMVersionArr, 0, EEPROMVersionArr.Length);
                string versionFromEEPROM = Encoding.UTF8.GetString(EEPROMVersionArr);

                if (versionFromEEPROM != "00")// CurrentEEPROMVersion)
                {
                    int LastStructIndex = ((int)EEPROM_DATA_ARRAY[3] * (int)4);
                    byte numberOfPages2Read = (byte)((int)EEPROM_DATA_ARRAY[LastStructIndex] + (int)EEPROM_DATA_ARRAY[LastStructIndex + 1]);
                    for (byte i = 2; i <= 2 + numberOfPages2Read; i++)
                    {
                        eepromRead(i);
                        Array.Copy(EEPROM_DATA_ARRAY, 0, byteArr, 0, byteArr.Length);
                        IVLCamVariables.eepromByteArrList.Add(byteArr);

                    }

                }
                else
                {
                }

            }
            catch (Exception ex)
            {
                 CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        string ControllerDataRecieved()
        {
            try
            {
                char[] removeEndChars = new char[] { '\n', '\r', '\0' };
                /* copy the received message to returnVal */
                int dataLen = 0;
                /* copy the received message to returnVal */
                for (int i = 0; i < USB_DATA_RECEIVED.Length; i++)
                {
                    if (USB_DATA_RECEIVED[i] != '\0')
                        dataLen++;
                    else
                        break;
                }
                byte[] resultArr = new byte[dataLen];
                Array.Copy(USB_DATA_RECEIVED, 0, resultArr, 0, dataLen);

                returnVal = Encoding.UTF8.GetString(resultArr);
                //returnVal = returnVal.Trim(removeEndChars);
                //  CompareRetBulkDataFromBoard(returnVal);
            }
            catch (Exception ex)
            {
                 CameraLogger.WriteException(ex, Exception_Log);
            }
            return returnVal;
        }

        internal void FlushBulkBuffer()
        {

            //int ReadErrCode = -10;
            //bool isSuccess = false;
            
            //for (int i = 0; i < USB_DATA_RECEIVED.Length; i++)
            //{
            //    USB_DATA_RECEIVED[i] = 0;
            //}
            //if (isOpen)
            //{
            //    while (!isSuccess)
            //    {
            //        USB_Read(USB_DATA_RECEIVED, ref isSuccess, ref ReadErrCode, readTimeout);
            //        if (ReadErrCode == -7 || ReadErrCode == -5)
            //            isSuccess = true;
            //    }
            //}
            BulkQueueClearPending();

        }

        public string SendBoardCommand(byte[] Command,ref bool isSuccess)
        {
            try
            {
                for (int i = 0; i < USB_DATA_RECEIVED.Length; i++)
                {
                    USB_DATA_RECEIVED[i] = 0;
                }
                int errCode = 0;
                int ReadErrCode = -10;
                int WriteErrCode = -10;
                bool ReadSuccess = false;
                bool WriteSuccess = false;
                string CommandStr = Encoding.UTF8.GetString(Command);// sending command
                Args logArg = new Args();

                logArg["TimeStamp"] = DateTime.Now;
                logArg["Msg"] = " O_M = " + CommandStr ;
                //logArg["callstack"] = Environment.StackTrace;

                string str = string.Format("Time = {0}, O_M = {1}  ", DateTime.Now.ToString("HH-mm-ss-fff"), CommandStr );
                if (IVLCamVariables.isCapturing)
                    capture_log.Add(logArg);
                {
                    //IVLCamVariables.logClass.CaptureLogList.Add(str);
                    BulkLogList.Add(logArg);
                    //IVLCamVariables.CameraLogList.Add(logArg);
                }
                //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);


                USB_Read_Write(Command, USB_DATA_RECEIVED, ref ReadSuccess, ref WriteSuccess, ref errCode, ref ReadErrCode, ref WriteErrCode,readTimeout,writeTimeout);// send command byte array to the microcontroller
                if (!WriteSuccess)
                {
                    for (int i = 0; i < IVLCamVariables._Settings.BoardSettings.BoardIterCnt; i++)
                    {
                        str = string.Format("Time = {0}, Write only Success= {1}, iteration {2}  ", DateTime.Now.ToString("HH-mm-ss-fff"), isSuccess, i );

                        USB_Write(Command, ref isSuccess, ref WriteErrCode,writeTimeout);
                        WriteSuccess = isSuccess;
                        Args logArg1 = new Args();
                        
                        logArg1["TimeStamp"] = DateTime.Now;
                        logArg1["Msg"] = "Write only Success = " + isSuccess + " iteration = " + i;
                        //logArg["callstack"] = Environment.StackTrace;

                        if (isSuccess && WriteErrCode == 0)
                        {

                            if (IVLCamVariables.isCapturing)
                                capture_log.Add(logArg1);
                            {
                                //IVLCamVariables.logClass.CaptureLogList.Add(str);
                                BulkLogList.Add(logArg1);
                                //IVLCamVariables.CameraLogList.Add(logArg);
                            }
                            //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);

                            break;
                        }
                    }
                }
                if (!ReadSuccess && !CommandStr.Contains("MRST") && !CommandStr.Contains("FLOS"))
                {
                    for (int j = 0; j < IVLCamVariables._Settings.BoardSettings.BoardIterCnt; j++)
                    {
                        str = string.Format("Time = {0}, Read only Success= {1}, error code {2}, returnValue ={3}, iteration ={4}  ", DateTime.Now.ToString("HH-mm-ss-fff"), isSuccess, errCode, returnVal, j );
                        USB_Read(USB_DATA_RECEIVED, ref isSuccess, ref ReadErrCode,readTimeout);
                        returnVal = Encoding.UTF8.GetString(USB_DATA_RECEIVED);
                        ReadSuccess = isSuccess;
                        Args logArg1 = new Args();

                        logArg1["TimeStamp"] = DateTime.Now;
                        logArg1["Msg"] = "Read only Success= " + isSuccess + "error code   " + errCode + "returnValue = " + returnVal + "iteration = " + j;
                        //logArg1["callstack"] = Environment.StackTrace;

                        if (IVLCamVariables.isCapturing)
                            capture_log.Add(logArg1);

                        //captureLog.Debug(" Read Success = ,{0} ,Error Code, = {1} ,returnValue ={2},iteration ={3}, ", isSuccess, errCode, returnVal, j);// if capturing sequence add to the capture log else add to the normal log
                        //IVLCamVariables.logClass.CaptureLogList.Add(str);
                        {
                            BulkLogList.Add(logArg1);
                            //IVLCamVariables.CameraLogList.Add(logArg);
                        }   //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);

                        if (isSuccess && ReadErrCode == 0)
                        {
                            Args logArg2 = new Args();
                            returnVal = Encoding.UTF8.GetString(USB_DATA_RECEIVED);
                            logArg2["TimeStamp"] = DateTime.Now;
                            logArg2["Msg"] = "Read and Write = " + isSuccess + "returnValue =  " + returnVal;
                            //logArg2["callstack"] = Environment.StackTrace;
                            if (IVLCamVariables.isCapturing)
                                capture_log.Add(logArg2);
                            {
                                //IVLCamVariables.logClass.CaptureLogList.Add(str);
                                BulkLogList.Add(logArg2);
                                //IVLCamVariables.CameraLogList.Add(logArg);
                            }   //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);

                            break;
                        }
                    }
                }
                if (WriteSuccess && ReadSuccess)
                {
                    Args logArg1 = new Args();

                    
                //    str = string.Format("Time = {0}, Return Read Write Value = {1}, Success = {2}, Error Code, = {3} ", DateTime.Now.ToString("HH-mm-ss-fff"), returnVal, isSuccess, errCode);
                    returnVal = Encoding.UTF8.GetString(USB_DATA_RECEIVED);
                    isSuccess = WriteSuccess && ReadSuccess;
                    logArg1["TimeStamp"] = DateTime.Now;
                    logArg1["Msg"] = "Return Read Write Value = " + returnVal + "Success =  " + isSuccess + "Error Code = " + errCode;
                    //logArg1["callstack"] = Environment.StackTrace;
                    if (IVLCamVariables.isCapturing)
                        capture_log.Add(logArg1);
                    {
                        //captureLog.Debug("Return Read Write Value =, {0}, Success = ,{1} ,Error Code, = {2}", returnVal, isSuccess, errCode);// if capturing sequence add to the capture log else add to the normal log
                        //IVLCamVariables.logClass.CaptureLogList.Add(str);
                        BulkLogList.Add(logArg1);
                        //IVLCamVariables.CameraLogList.Add(logArg);
                    }
                    //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);
                    /* If Any data was Received */
                    BulkTransferReturnData bReturnData = new BulkTransferReturnData();
                    bReturnData.USBArr = USB_DATA_RECEIVED;
                    bReturnData.returnData = returnVal;
                    bReturnData.isSuccess = isSuccess;
                    bReturnData.CommandData = Encoding.UTF8.GetString(Command).Substring(0, 4);
                    UseBulkTransferReturnData(bReturnData);
                    //IVLCamVariables.CameraLogList.Add(logArg);
                    //IVLCamVariables.logClass. BulkLogList.Add(str);
                }
                // only for servo command if it is busy retry
                while (returnVal.ToLower().Contains("srvo_bsy"))// if servo is busy retry for 5 times
                {
                    servoRetryCount++;// to count to check iterations for servo
                    Args arg = new Args();
                    arg["TimeStamp"] = DateTime.Now;
                    arg["Msg"] = "Bulk Reply =" + returnVal+ "Success =  " + isSuccess + "Error Code = " + errCode + "Servo retry Count = " + servoRetryCount;
                    //arg["callstack"] = Environment.StackTrace;
                    if (IVLCamVariables.isCapturing)
                        capture_log.Add(arg);
                    if (servoRetryCount < 5)// if count is less than 5 retry servo command
                    {
                        System.Threading.Thread.Sleep(50);// sleep for 20 milliseconds so servo is completed.
                        SendBoardCommand(Command, ref isSuccess);// re send servo command
                    }
                    else // return 
                    {
                        servoRetryCount = 0;// reset iteration count to zero
                        return returnVal;// return value of from the board
                    }
                }
                return returnVal;
            }


            catch (Exception ex)
            {
                 CameraLogger.WriteException(ex, Exception_Log);
                return string.Empty;
            }
        }
       public bool isCptrRecieved = false;
        public void whiteLightOn()
        {
            try
            {

                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                //bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; } ;
                    string CommandString = "FLON";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    if (IVLCamVariables.ImagingMode != ImagingMode.Anterior_Prime)
                        arr[4] = Convert.ToByte(0); //0 - Internal Flash
                    else
                        arr[4] = Convert.ToByte(2); //2 - Anterior Flash 

                 
                    bool isSuccess = false;
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   
                    //string str = SendBoardCommand(arr, ref isSuccess);
                    //string[] RecStrArr = str.Split('_');

            }
            catch (Exception ex)
            {
                 CameraLogger.WriteException(ex, Exception_Log);
            }
        }


        //void bg_DoWork(object sender, DoWorkEventArgs e)
        //{

        //    DoBulkTransferThreadWork((Args)e.Argument);
        //}

        private void DoBulkTransferThreadWork(Args arg)
        {
            bool isSuccess = false;
            string str = "";
            {
                UseBulkTransferSendData(arg);
                str = SendBoardCommand((byte[])arg["CommandArr"], ref isSuccess);

            }
        }
       public int addIndex = 0;
        private void Add2BulkQueue(Args arg)
        {
//            List<Args> tempArgList = new List<Args>();
//            tempArgList = BulkTransferQueue.Where(x => x.Count > 0).ToList();
//           if(tempArgList.Count >=0)
//           {
//               if (tempArgList.Where(x => ((string)x["CommandString"]).Substring(0,4) ==  ((String)arg["CommandString"]).Substring(0,4)).Count() > 0)
//               {
//                   Args logArg = new Args();

//                   BulkTransferQueue[addIndex] = arg;

//                   if (addIndex == BulkTransferQueue.Length - 1)
//                       addIndex = 0;
//                   else
//                       addIndex++;
//                   //BulkTransferQueue.Add(arg);

//                   logArg["TimeStamp"] = DateTime.Now;
//                   logArg["Msg"] = string.Format("Added to bulktransfer queue = {0} , queue status = {1} , Current Bulk transfer index count = {2}, Bulk queue Count ={3}", arg["CommandString"] as string, isCommandSendRecieveActive, BulkIndex, addIndex);
//                   //logArg["callstack"] = Environment.StackTrace;

//                   if (IVLCamVariables.isCapturing)
//                       capture_log.Add(logArg);

//                   BulkLogList.Add(logArg);
//                   //IVLCamVariables.CameraLogList.Add(logArg);
//                   //IVLCamVariables.logClass. BulkLogList.Add(str);
//               }
//           }
//else           
           {
               
                Args logArg = new Args();

                    BulkTransferQueue[addIndex] = arg;

                    if (addIndex == BulkTransferQueue.Length - 1)
                        addIndex = 0;
                    else
                        addIndex++;
                    //BulkTransferQueue.Add(arg);

                    logArg["TimeStamp"] = DateTime.Now;
                    logArg["Msg"] = string.Format("Added to bulktransfer queue = {0} , queue status = {1} , Current Bulk transfer index count = {2}, Bulk queue Count ={3}", arg["CommandString"] as string, isCommandSendRecieveActive, BulkIndex, addIndex);
                    //logArg["callstack"] = Environment.StackTrace;

                    if (IVLCamVariables.isCapturing)
                        capture_log.Add(logArg);

                    BulkLogList.Add(logArg);
                    //IVLCamVariables.CameraLogList.Add(logArg);
                    //IVLCamVariables.logClass. BulkLogList.Add(str);
                }
                //else
                //{

                //    logArg["TimeStamp"] = DateTime.Now;
                //    logArg["Msg"] = string.Format("Command already present in the bulktransfer queue = {0} , queue status = {1} , Current Bulk transfer index count = {2}, Bulk queue Count ={3}", arg["CommandString"] as string, isCommandSendRecieveActive, BulkIndex, addIndex);
                //    //logArg["callstack"] = Environment.StackTrace;

                //    if (IVLCamVariables.isCapturing)
                //        capture_log.Add(logArg);

                //    BulkLogList.Add(logArg);
                //}
        }

        private void UseBulkTransferSendData(Args SendData)
        {
            string sendData = SendData["CommandString"] as string;
            sendData = sendData.Substring(0,4);

            switch (sendData)
            {
                case "MFBK" :
                    {
                        IVLCamVariables.IsMotorMoved = false;
                        IVLCamVariables.intucamHelper.StartCaptureMotorTimer();
                        break;
                    }
                    case "MFFR" :
                    {
                        IVLCamVariables.IsMotorMoved = false;
                        IVLCamVariables.intucamHelper.StartCaptureMotorTimer();// This line is added in order to handle timeout for forward motor movement as well in order fix defect 0001637
                        break;
                    }
                    case "MRST":
                    {
                        IVLCamVariables.IsMotorMoved = false;
                         IVLCamVariables.intucamHelper.StartCaptureMotorTimer();
                        break;
                    }
            }

        }
        private void UseBulkTransferReturnData(BulkTransferReturnData returnData)
        {
            string[] returnDataArr = returnData.returnData.Split('_');
            string str = "";
            if(returnData.CommandData == returnDataArr[0])
                switch (returnDataArr[0])
                {
                    case "MFBK":
                        {
                     //       if (returnDataArr[1].Contains("OK") || returnDataArr[1].Contains("Done"))
                     //       {
                     ////               IVLCamVariables.intucamHelper.StartCaptureMotorTimer();
                     //       }
                            break;
                        }
                    case "MFFR":
                        {
                            //if (returnDataArr[1].Contains("OK") || returnDataArr[1].Contains("Done"))
                            
                       //         IVLCamVariables.intucamHelper.StartCaptureMotorTimer();

                            break;
                        }
                    case "MRST":
                        {
                                if (returnDataArr[1].Contains("OK"))
                                {
                                    // i.StartCaptureMotorTimer();

                                }
                            break;
                        }
                    case "CPTR":
                        {
                            if (returnDataArr[1].Contains("OK") || returnDataArr[1].Contains("Done"))
                            {
                                if (!isCptrRecieved && !IVLCamVariables.isResuming)
                                {
                                    Args logArg = new Args();
             
                                    logArg["TimeStamp"] = DateTime.Now;
                                    logArg["Msg"] = "Capture event recieved";
                                    //logArg["callstack"] = Environment.StackTrace;
                                    str = string.Format("Time = {0}, Capture event recieved  ", DateTime.Now.ToString("HH-mm-ss-fff"));
                                    isCptrRecieved = true;
                                    if (IVLCamVariables.isCapturing)
                                        capture_log.Add(logArg);

                                        //IVLCamVariables.logClass.CaptureLogList.Add(str);
                                    {
                                         BulkLogList.Add(logArg);
                                     //IVLCamVariables.CameraLogList.Add(logArg);
                                    }
                                    //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);

                                    if (!IVLCamVariables.IsFlashOnDone)
                                    {
                                        flashOffInterruptTimeoutTimer.Elapsed += flashOffInterruptTimeoutTimer_Elapsed;//To subscribe for the flash on done time out elapsed method.
                                        flashOnInterruptTimeoutTimer.Interval = IVLCamVariables._Settings.BoardSettings.flashOnDoneStrobeCycleValue * IVLCamVariables.StrobeValue;
                                        flashOnInterruptTimeoutTimer.Enabled = true;
                                        flashOnInterruptTimeoutTimer.Start();
                                    }
                                    //i.StartStopFlashInterruptTimoutTimer(IVLCamVariables._Settings.BoardSettings.flashOnDoneStrobeCycleValue * IVLCamVariables.StrobeValue, true);
                                    // _CaptureEvent(RecStrArr, e); //calling an event to say capture event is successful to start saving frames
                                }
                                else
                                {
                                    //Args arg1 = new Args();
                                    //arg1["isCaptureFailed"] = true;
                                    //arg1["Capture Failure Category"] = "004";// if cptr_ok was not recieved
                                    //IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.FrameCaptured, arg1);
                                }
                            }
                            }
                            break;
                    case "MTRP":
                        {
                                char a = (char)returnData.USBArr[5];
                                char b = (char)returnData.USBArr[6];
                                char c = (char)returnData.USBArr[7];
                                char d = (char)returnData.USBArr[8];
                                char e = (char)returnData.USBArr[9];
                                char f = (char)returnData.USBArr[10];
                                int motorPositionStepCountVal = a * (byte.MaxValue + 1) + b;

                                if (motorPositionStepCountVal > Int16.MaxValue)
                                    motorPositionStepCountVal = motorPositionStepCountVal - (-2 * Int16.MinValue);
                                int maxValue = 400; int minValue = -600;// variables to maintain the rotary range
                                motorPositionStepCountVal = motorPositionStepCountVal / 32;



                                MotorZeroD_OffsetValue = e * (byte.MaxValue + 1) + f;

                                if (MotorZeroD_OffsetValue > Int16.MaxValue)
                                    MotorZeroD_OffsetValue = MotorZeroD_OffsetValue - (-2 * Int16.MinValue);
                             IVLCamVariables.rotaryZeroDOffsetValue =  MotorZeroD_OffsetValue = MotorZeroD_OffsetValue / 32;

                                if (isReverseMotorPositionSensor)
                                {

                                    motorPositionStepCountVal = -motorPositionStepCountVal;

                                    //Done to limit the min and max values of rotary in the reverse direction
                                    minValue = -IVLCamVariables.MaxPositiveDiaptor;
                                    maxValue = IVLCamVariables.MaxNegativeDiaptor;
                                }
                                else
                                {
                                    //Done to limit the min and max values of rotary 

                                    maxValue = IVLCamVariables.MaxPositiveDiaptor;
                                    minValue = -IVLCamVariables.MaxNegativeDiaptor;
                                }
                                if (motorPositionStepCountVal < maxValue && motorPositionStepCountVal > minValue)
                                {
                                    IVLCamVariables.RotaryUpdateValues = motorPositionStepCountVal;
                                    //IVLCamVariables.intucamHelper.motorSensorPositionEvent(motorPositionStepCountVal, motorPositionStepCountVal + MotorZeroD_OffsetValue);
                                }
                            break;
                        }
                    case "VERN":
                        {
                            IVLCamVariables.FirmwareVersion = returnDataArr[1];
                            break;
                        }
                    case "RADC":
                        {
                            int val = -1;// bulk transfer not working and to software value
                            if (returnDataArr[0] == "RADC")
                            {
                                byte[] byteArr = Encoding.UTF8.GetBytes(returnData.returnData);
                                 val = Convert.ToInt32(byteArr[5]);
                            }
                            ThreadPool.QueueUserWorkItem(new WaitCallback(f=>{
                                IVLCamVariables.intucamHelper.updateAnalogInt2DigitalInt(val);
                            }));

                            break;
                        }
                    case "LRST":
                        {
                            bool leftRightVal = false;
                                    if (returnData.returnData.Contains("LEFT"))
                                        leftRightVal = true;
                                    else if (returnData.returnData.Contains("RIGHT"))
                                        leftRightVal = false;
                                    IVLCamVariables.intucamHelper.leftRightEvent(leftRightVal);
                            break;
                        }
                    case "GTRT":
                        {
                                int Hours = returnData.USBArr[9];
                                int Mins = returnData.USBArr[10];
                                int Secs = returnData.USBArr[11];
                                int Msecs = returnData.USBArr[12] * 256 + returnData.USBArr[13];
                                if (returnData.returnData.Contains("RTC"))
                                {

                                }
                                else if (returnData.returnData.Contains("FONT"))
                                {
                                    Flash_On_Time = new DateTime(1, 1, 1, Hours, Mins, Secs, Msecs);
                                }
                                else if (returnData.returnData.Contains("FOFT"))
                                {
                                    Flash_Off_Time = new DateTime(1, 1, 1, Hours, Mins, Secs, Msecs);
                                }
                            break;
                        }
                    case "TRON":
                        {
                            //IVLCamVariables.FFATime = returnData.returnData;
                            //Args arg = new Args();
                            //arg["ffaTime"] = IVLCamVariables.FFATime;
                            //IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.UpdateFFATime, arg);
                            break;
                        }
                    case "TRST":
                        {
                            IVLCamVariables.FFATime = returnData.returnData.Substring(5,1);
                            byte val = Encoding.ASCII.GetBytes(IVLCamVariables.FFATime)[0];
                            //Args arg = new Args();
                            //arg["ffaTime"] = val.ToString() ;
                            //IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.UpdateFFATime, arg);
                            //IVLCamVariables.intucamHelper.totalFrameCount_Lbl.Text = "Trigger Status = " + val.ToString();
                            break;
                        }

                }
            }

            
        public void whiteLightOff()
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   // bg.DoWork +=(sender,args)=>{ args.Result = DoBulkTransferThreadWork((Args)args.Argument);};
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    string CommandString = "FLOF";
                    ValueString = "000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = Convert.ToByte(imagingMode);// 0 flash & 4 blue

                    bool isSuccess = false;
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   

                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        public void BlueLightOn()
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };

                    string CommandString = "FLON";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = Convert.ToByte(1);// 0 flash & 4 blue
                    bool isSuccess = false;
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                  //bg.RunWorkerAsync(arg);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        public void BlueLightOff()
        {
            try
            {
                {
                    //BackgroundWorker bg = new BackgroundWorker();
                    //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                    //bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    string CommandString = "FLOF";
                    ValueString = "000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);

                    bool isSuccess = false;
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);

                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }
        }
        public void IRLightOn()
        {
            try
            {
                {
                    //BackgroundWorker bg = new BackgroundWorker();
                    //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                    //bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    string CommandString = "IRON";
                    ValueString = "000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    if (IVLCamVariables.ImagingMode != ImagingMode.Anterior_Prime)
                        arr[4] = Convert.ToByte(0); //0 - ir 
                    else
                        arr[4] = Convert.ToByte(2); //0 - ir 

                    bool isSuccess = false;
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   

                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        public void IRLightOff()
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    string CommandString = "IROF";
                    string valueString = "000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + valueString);
                    arr[4] = Convert.ToByte(imagingMode);
                    //  CommandString = Encoding.UTF8.GetString(arr);
                    bool isSuccess = false;
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
      
        public void CheckPowerStatus()
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    string CommandString = "PWST";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    bool isSuccess = false;
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   

                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        public void CameraConnected()
        {
            try
            {
                CheckCameraStatus();
                if (isCameraConnected)
                {
                    isStrobeConnected = true;
                    GetFrameRate();
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        public void GetTriggerStatus()
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    CommandString = "TRST";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    bool isSuccess = false;
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                  //bg.RunWorkerAsync(arg);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   

                    //if (RecStrArr[0] == CommandString)
                    //{
                    //    IVLCamVariables.TrigStatus = (TriggerStatus)Enum.ToObject(typeof(TriggerStatus), USB_DATA_RECEIVED[5]);
                    //}
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        public void CheckCameraStatus()
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    string CommandString = "CAMS";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = Convert.ToByte(imagingMode);

                    bool isSuccess = false; Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   
                    //string[] RecStrArr = str.Split('_');

                    //if (RecStrArr[0] == CommandString)
                    //{
                    //    if (str.Contains("CAMS_CAMC"))
                    //    {
                    //        isCameraConnected = true;
                    //        Args arg = new Args();
                    //        //arg["isCameraConnected"] = isCameraConnected;
                    //        //_eventHandler.Notify(_eventHandler.CAMERA_DISCONNECTED, arg);
                    //    }

                    //    else if (str.Contains("CAMS_CAMD"))
                    //    {
                    //        isCameraConnected = false;
                    //        Args arg = new Args();
                    //        //arg["isCameraConnected"] = isCameraConnected;
                    //        //_eventHandler.Notify(_eventHandler.CAMERA_DISCONNECTED, arg);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }

        public void CaptureCommand()
        {
            try
            {
                {
                  // BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData;



                    //    Add2BulkQueue(arg);};
                    string CommandString = "CPTR";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = Convert.ToByte(imagingMode); // 0 for 45 & 45+ color

                    bool isSuccess = false;
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();
                    Add2BulkQueue(arg);
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        /// <summary>
        /// Reset Vsync pulse once the capture sequence is complete/ if the camera is put to live mode
        /// </summary>
        /// <param name="iterCommandCnt"></param>
        public void ResetVSync()
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    CommandString = "REST";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = Convert.ToByte(imagingMode);

                    bool isSuccess = false; Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        public void MotorForward(byte steps)
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    CommandString = "MFOR";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = Convert.ToByte(steps);

                    bool isSuccess = false;
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   

                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }

        public void MotorBackward(byte steps)
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData;
                    
                    
                   // };
                    CommandString = "MBAK";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = Convert.ToByte(steps);

                    bool isSuccess = false; Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                     Add2BulkQueue(arg);
                   
                  
                   

                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        /// <summary>
        /// Fast forward of motor 
        /// </summary>
        /// <param name="steps">Number of steps the motor has to forward</param>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed </param>
        public void MotorFastForward(byte steps)
        {
            try
            {
                {
                  // BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData;
                   
                   // };
                    CommandString = "MFFR";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = Convert.ToByte(steps);

                    bool isSuccess = false; Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();


                     Add2BulkQueue(arg);

                    //IVLCamVariables.IsMotorMoved = false;
                   
                    string[] RecStrArr = returnData.returnData.Split('_');
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }
        }
        /// <summary>
        /// Fast Backward of motor 
        /// </summary>
        /// <param name="steps">>Number of steps the motor has to Backward</param>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void MotorFastBackward(byte steps)
        {
            try
            {
                {
                  // BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData;
                   
                    
                   // };
                    CommandString = "MFBK";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = Convert.ToByte(steps);

                    bool isSuccess = false;
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);

                    BulkTransferReturnData returnData = new BulkTransferReturnData();
                     Add2BulkQueue(arg);

                   //IVLCamVariables.IsMotorMoved = false;
                  
                   
                    string[] RecStrArr = returnData.returnData.Split('_');

                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        /// <summary>
        /// Position of the motor after forward / backward is done
        /// </summary>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void MotorCapturePosition()
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    CommandString = "MPOS";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    bool isSuccess = false;
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                  
                   

                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        /// <summary>
        /// Loop the motor from forward to backward
        /// </summary>
        /// <param name="loopArr">loopArr contains two items loopArr[0] contains the number of steps and loopArr[1] contains the number of cyles the loop has to be repeated</param>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void MotorLoop(byte[] loopArr)
        {
            try
            {
                {

                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    CommandString = "MLOP";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = loopArr[0];
                    arr[5] = loopArr[1];

                    bool isSuccess = false;
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        /// <summary>
        /// Reset Board
        /// </summary>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void ResetBoard()
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    CommandString = "BRST";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    bool isSuccess = false; Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   

                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        // Added firmware version function to the Alt +i in order address the CR 0000577 by sriram on August 17th 2015
        /// <summary>
        /// Method to obtain the firmware version uploaded on the board
        /// </summary>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void GetFirmwareVersion()
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData;
                   
                    
                   // }; 
                    CommandString = "VERN";//00000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = byte.MinValue;
                    bool isSuccess = false;
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   

                }

            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        /// <summary>
        /// Set Flash offset in the board to manage delay and duration of flash 
        /// </summary>
        /// <param name="offset">offset is an array of count 2 first index contains the offset1 and second index offset2</param>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void FlashOffset(byte[] offset)
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    CommandString = "FLOS";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = Convert.ToByte(offset[0]);
                    arr[5] = Convert.ToByte(offset[1]);
                    bool isSuccess = false;
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                  BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }
        }

        /// <summary>
        ///  EEPROM Write function 
        /// </summary>
        /// <param name="pageNumber">The page number where the EEPROM has to happen</param>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void eepromWrite(byte pageNumber)
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                    //bg.RunWorkerCompleted += (sender, args) => { 
                    //    BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData;
                        
                    //};
                    CommandString = "EEWR";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = Convert.ToByte(pageNumber);
                    bool isSuccess = false; Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   
                    string[] RecStrArr = returnData.returnData.Split('_');

                    if (RecStrArr[0] == returnData.CommandData)
                    {
                        if (!RecStrArr[1].Contains("OK"))
                        {
                            eepromWrite(pageNumber);
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
        /// <summary>
        /// Enable Full frame Capture Mode 
        /// </summary>
        /// <param name="isFullFrameCapture">byte number takes only two values 0/1  ,if it is 0 disabled it is 1 it is enabled</param>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void SetFullFrameCapture(byte isFullFrameCapture)
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    CommandString = "CMMD";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = Convert.ToByte(isFullFrameCapture);
                    bool isSuccess = false; Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }
        }
        /// <summary>
        /// Check the Left Right Sensor Status 
        /// </summary>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void CheckLeftRightSensorStatus()
        {
            try
            {
                {
                  // BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { 
                   //     BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData;
                       
                   // };
                    CommandString = "LRST";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    bool isSuccess = false;
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   
                    string[] RecStrArr = returnData.returnData.Split('_');
                   
                   

                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        /// <summary>

        /// </summary>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void CheckMotorSensorStatus()
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    CommandString = "MSST";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    bool isSuccess = false;
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   

                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        /// <summary>
        /// Set the flash boost value
        /// </summary>
        /// <param name="FLashBoostVal">Flash boost value</param>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void SetFlashBoost(byte FLashBoostVal)
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    CommandString = "FBST";//000000";
                    //ValueString = "000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = Convert.ToByte(FLashBoostVal);
                    arr[5] = Convert.ToByte(imagingMode);

                    bool isSuccess = false;
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        /// <summary>
        /// To get the position of the stepper motor from the motor sensor. It triggers an event to indicate the position of the Stepper motor
        /// </summary>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void GetMotorPosition()
        {
            try
            {
                  // BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData;
                    
                   // };
                    CommandString = "MTRP";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    bool isSuccess = false; Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();
                    Add2BulkQueue(arg);
            }
            catch (Exception ex)
            {
                 CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        /// <summary>
        /// Reset the stepper motor .To either zero Diaptor position or sensor position
        /// </summary>
        /// <param name="isPos">if true it is reset to sensor position else if it is false reset to zero Diaptor position</param>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void ResetMotorPosition(bool isPos)
        {
            try
            {
                {
                   
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData;
                   // string[] RecStrArr = bReturnData.returnData.Split('_');
                   // if (RecStrArr[0] == bReturnData.CommandData)
                   // {
                   //     if (i == null)
                   //         i = IntucamHelper.GetInstance();
                   //     if (RecStrArr[1].Contains("OK"))
                   //         i.StartCaptureMotorTimer();
                   // }
                   // };
                    CommandString = "MRST";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    if (isPos)
                        arr[4] = Convert.ToByte(0);// Reset to sensor Postion 
                    else
                        arr[4] = Convert.ToByte(1);// Reset to zero diaptor
                    bool isSuccess = false;
                    BulkTransferReturnData returnData = new BulkTransferReturnData();
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                     Add2BulkQueue(arg);


                   

                    string[] RecStrArr = returnData.returnData.Split('_');
                   
                  //bg.RunWorkerAsync(arg);
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }

        public void BulkQueueClearPending()
        {
            BulkIndex = 0;
            addIndex = 0;
            for (int i = 0; i < BulkTransferQueue.Length; i++)
            {
                BulkTransferQueue[i] = new Args();
            }
        }
        public bool isbulkTransferQueueCleared = true;
        /// <summary>
        ///  EEPROM Read function 
        /// </summary>
        /// <param name="pageNumber">Read the EERPOM from the page number</param>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void eepromRead(byte pageNumber)
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    CommandString = "EERD";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = Convert.ToByte(pageNumber);

                    bool isSuccess = false;
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   
                }

            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        /// <summary>
        /// Set Zero Diaptor Position for Stepper motor Reset
        /// </summary>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void SetZeroDiaptor()
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   // bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    CommandString = "ZDST";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    bool isSuccess = false; Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        /// <summary>
        /// To Get the strobe rate from the board. if the result is greater than zero it means the strobe is connected to the board else there is a disconnection
        /// </summary>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void GetFrameRate()
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { 
                   //     BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData;
                      
                    
                   // };
                    CommandString = "FMRT";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    bool isSuccess = false; Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   
                    string[] RecStrArr = returnData.returnData.Split('_');

                    if (RecStrArr[0] == returnData.CommandData)
                    {
                        StrobeRate = Convert.ToByte(USB_DATA_RECEIVED[5]);
                        if (StrobeRate > 0)
                            isStrobeConnected = true;
                        else
                            isStrobeConnected = false;
                    }
                    //else if (iterCommandCnt > 0)
                    //    GetFrameRate(iterCommandCnt);
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        /// <summary>
        /// Reset the Trigger status when the capture sequence completes or if the live mode is started
        /// </summary>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void TriggerOn()
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData;
                   
                   // };
                    CommandString = "TRON";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    bool isSuccess = false;
                    string str = "";
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   
                    if (returnData.returnData.Contains("TRON_BSY"))
                    {
                        
                        int errCode = -10;
                        int intCode = -10;
                        byte[] intData = new byte[10];
                        USBReadInterrupt(ref returnData.isSuccess, ref errCode, ref intCode, intData,interruptTimeout);
                        str = string.Format("TRON _Bsy Due to interrupt {0}  ", intCode );
                        Args logArg = new Args();

                        logArg["TimeStamp"] = DateTime.Now;
                        logArg["Msg"] = string.Format("TRON _Bsy Due to interrupt {0}  ", intCode);
                         //IVLCamVariables.CameraLogList.Add(logArg);
                        //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);

                    }
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        /// <summary>
        /// Turn off Trigger status to indicate the capture sequence is in progress
        /// </summary>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void TriggerOFF()
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { 
                   //     
                   //     BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData;
                   // };

                    CommandString = "TROF";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    bool isSuccess = false; 
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                  //bg.RunWorkerAsync(arg);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   

                    string[] RecStrArr = returnData.returnData.Split('_');
                    if (RecStrArr[0] == returnData.CommandData)
                    {

                    }
                }

            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }

        /// <summary>
        /// To get Pot intensity value from the board.
        /// </summary>
        /// <param name="iterCommandCnt"></param>
        public void Read_LED_SupplyValues()
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData;
                   // string[] RecStrArr = bReturnData.returnData.Split('_');
                   // if (i == null)
                   //     i = IntucamHelper.GetInstance();
                   // if (RecStrArr[0] == "RADC")
                   // {
                   //     byte[] byteArr = Encoding.UTF8.GetBytes(bReturnData.returnData);
                   //     int val = Convert.ToInt32(byteArr[5]);
                   //     i.updateAnalogInt2DigitalInt(val);
                   // }
                   // else // bulk transfer not working and to software value
                   // {
                   //     i.updateAnalogInt2DigitalInt(-1);
                   // }
                    
                   // };
                    CommandString = "RADC";//:00000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = (byte)1;
                    bool isSuccess = false; Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   
                    string[] RecStrArr = returnData.returnData.Split('_');
                   
                 // bg.RunWorkerAsync(arg);
                   
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }

        /// <summary>
        /// Get the LED Intensities values that have been set from the manufacturer
        /// </summary>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void Read_LED_Intensities()
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };

                    CommandString = "RDIN";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    string SendingString = Encoding.UTF8.GetString(arr);
                    bool isSuccess = false; Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   
                 // bg.RunWorkerAsync(arg);
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }

        /// <summary>
        /// Set the Max of Flash and IR  LED Intensities
        /// </summary>
        /// <param name="intensities">intensities is an 1D array with two indices first index indicates the flash intensity and second index is the Max IR intensity</param>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void SetFlashIRMaxIntensity(byte[] intensities)
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    CommandString = "MXIN";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = intensities[0];
                    arr[5] = intensities[1];
                    bool isSuccess = false; Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   
                 
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }

        /// <summary>
        /// Set the Flash LED Intensity value
        /// </summary>
        /// <param name="FlashInt">Intensity value to be set</param>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void SetFlashIntensity(byte FlashInt)
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    CommandString = "FLIN";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = FlashInt;
                    bool isSuccess = false; Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        /// <summary>
        /// Set the IR LED Intensity value
        /// </summary>
        /// <param name="IRInt">Intensity value to be set</param>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void SetIRIntensity(byte IRInt)
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    CommandString = "IRIN";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = IRInt;
                    bool isSuccess = false;
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                 CameraLogger.WriteException(ex, Exception_Log);
            }


        }
        /// <summary>
        /// A single function used to set all the LED intensities
        /// </summary>
        /// <param name="intensityDetails">1D array with count 3 each indicating an LED</param>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void Set_LED_Intensity(byte[] intensityDetails)
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    CommandString = "STIN";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = intensityDetails[0];
                    arr[5] = intensityDetails[1];
                    arr[6] = intensityDetails[2];
                    bool isSuccess = false;
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        /// <summary>
        /// Get Status of all the sensors, LEDs,Trigger Status
        /// </summary>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void GetAllStatus()
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    CommandString = "ALST";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    bool isSuccess = false;
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        /// <summary>
        /// Get the Status of the Flash LED , it either indicates On,Off,Fail
        /// </summary>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void GetFlashStatus()
        {
            try
            {
                {
                    CommandString = "FLST";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    bool isSuccess = false; Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   

                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        /// <summary>
        /// Get the Status of the IR LED , it either indicates On,Off,Fail
        /// </summary>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void GetIRStatus()
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    CommandString = "IRST";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    bool isSuccess = false; Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        /// <summary>
        /// To Enable stand by 
        /// </summary>
        /// <param name="enableStandby"></param>
        /// <param name="iterCommandCnt"></param>
        public void SetStandBy(byte enableStandby)
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    CommandString = "STBY";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    bool isSuccess = false;
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        /// <summary>
        /// Reset all the values of the components to factory default values
        /// </summary>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void FactoryReset()
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    CommandString = "FRST";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    bool isSuccess = false; Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);

            }

        }
        /// <summary>
        /// When the device powered up / reset the motor comesback to ZeroD/Sensor Postion
        /// </summary>
        /// <param name="initEnable"> 0 is disabled 1 is enabled</param>
        /// <param name="iterCommandCnt"></param>
        public void MotorInitPosEnable(byte initEnable)
        {
            try
            {
                {
                   
                    CommandString = "MTEN";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = initEnable;
                    bool isSuccess = false; Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        /// <summary>
        /// To Control the max position of the stepper motor movement
        /// </summary>
        /// <param name="EndPoint">This indicates the max position to be set</param>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void SetMotorEndPoint(byte EndPoint)
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    CommandString = "ESET";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = EndPoint;

                    bool isSuccess = false; Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);

            }

        }
        /// <summary>
        /// Enable the test mode disables the device details during production
        /// </summary>
        /// <param name="Enable">if 0 diabled else if 1 it is enabled</param>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void EnableProductionMode(byte Enable)
        {
            try
            {
                {
                    CommandString = "TMST";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = Enable;

                    bool isSuccess = false; Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   

                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        /// <summary>
        /// This writes the data to EEPROM the board does the verification
        /// </summary>
        /// <param name="pagenumber">The Page number where the data to be written in the EEPROM</param>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void EEPROM_Write_WithVerification(byte pagenumber)
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    CommandString = "EEWV";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = pagenumber;

                    bool isSuccess = false; Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   

                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        public void eepromReset()
        {
            try
            {
                eepromWrite(199);
                eepromWrite(200);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        /// <summary>
        /// Gets the board time
        /// </summary>
        /// <param name="mode">there are 3 possible mode 0 = real time/current board time, 1 = flash on time, 2 = flash off time</param>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void GetRealTime(byte mode)
        {
            try
            {
                   //BackgroundWorker bg = new BackgroundWorker();
                  // bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                    //bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; 
                    
                    //};
                    CommandString = "GTRT";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = Convert.ToByte(mode);

                    bool isSuccess = false;
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  

                   
                    string[] RecStrArr = returnData.returnData.Split('_');

                  

            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);

            }

        }
        /// <summary>
        /// Method to set the strobe width before capture command is initiated
        /// </summary>
        /// <param name="strobeWidth">Value of the strobe width in byte </param>
        public void SetStrobeWidth(byte strobeWidth)
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender,args)=>{ BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData;};
                    CommandString = "STBW";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = strobeWidth;
                    bool isSuccess = false; Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   

                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }

        /// <summary>
        /// Set the system/pc time to the board 
        /// </summary>
        /// <param name="iterCommandCnt">Number of iterations the command to initiated until the command is passed</param>
        public void SetRealTime()
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    DateTime cur_time = new DateTime();
                    cur_time = DateTime.Now;
                    byte hour = (byte)cur_time.Hour;
                    byte mins = (byte)cur_time.Minute;
                    byte second = (byte)cur_time.Second;
                    byte milli_lower = (byte)(cur_time.Millisecond % 256);
                    byte milli_upper = (byte)(cur_time.Millisecond / 256);

                    CommandString = "STRT";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = Convert.ToByte(hour);
                    arr[5] = Convert.ToByte(mins);
                    arr[6] = Convert.ToByte(second);
                    arr[7] = Convert.ToByte(milli_upper);
                    arr[8] = Convert.ToByte(milli_lower);


                    bool isSuccess = false;
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   


                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);

            }

        }

        public void GreenFilterMove(int Position)
        {
            try
            {
                {
                   ////BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    servoRetryCount = 0;// reset of servo motor busy check to zero 
                    CommandString = "SRVO";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = Convert.ToByte(2); // Green Filter is Servo 2
                    arr[5] = Convert.ToByte(Position / 256);
                    arr[6] = Convert.ToByte(Position % 256);
                    bool isSuccess = false;
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                    if(IVLCamVariables.isBlueFilteredMoved)
                        System.Diagnostics.Trace.WriteLine(string.Format("Add to queue blue filter move ={0}, Add Index ={1}, BulkIndex ={2}", IVLCamVariables.isBlueFilteredMoved, addIndex, bulkIndex));
                   
                  
                   
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }

        public void BlueFilterMove(int Position)
        {
            try
            {
                {
                   //BackgroundWorker bg = new BackgroundWorker();
                   //bg.DoWork += (sender, args) => { args.Result = DoBulkTransferThreadWork((Args)args.Argument); };
                   // bg.RunWorkerCompleted += (sender, args) => { BulkTransferReturnData bReturnData = args.Result as BulkTransferReturnData; };
                    servoRetryCount = 0;// reset of servo motor busy check to zero 
                    CommandString = "SRVO";//000000";
                    byte[] arr = Encoding.UTF8.GetBytes(CommandString + ValueString);
                    arr[4] = Convert.ToByte(1); // Blue Filter is Servo 1
                    arr[5] = Convert.ToByte(Position / 256);
                    arr[6] = Convert.ToByte(Position % 256);
                    bool isSuccess = false;
                    Args arg = new Args();
                    arg["CommandArr"] = arr;
                    arg["isSuccess"] = isSuccess;arg["CommandString"] = Encoding.UTF8.GetString(arr);
                    BulkTransferReturnData returnData = new BulkTransferReturnData();

                    Add2BulkQueue(arg);
                   
                  
                   
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
    }
    public class BulkTransferReturnData
    {
        public string returnData;
        public string CommandData;
             
        public bool isSuccess;
        public byte[] USBArr;
        public BulkTransferReturnData()
        {
            returnData = "";
            CommandData = "";
            isSuccess = false;
            USBArr = new byte[25];
        }
    }
    class USBDeviceInfo
    {
        public USBDeviceInfo(string deviceID, string pnpDeviceID, string description)
        {
            this.DeviceID = deviceID;
            this.PnpDeviceID = pnpDeviceID;
            this.Description = description;
        }
        public string DeviceID { get; private set; }
        public string PnpDeviceID { get; private set; }
        public string Description { get; private set; }
    }
}
