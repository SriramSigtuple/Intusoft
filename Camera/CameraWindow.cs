using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using NLog.Config;
using NLog.Targets;
using INTUSOFT.EventHandler;
using System.Management;
using System.Threading;
namespace INTUSOFT.Imaging
{
    public partial class CameraWindow : Form
    {
        Camera camera;
        private static readonly Logger Exception_Log = LogManager.GetLogger("INTUSOFT.Desktop");
        private uint WmDeviceChange = 537; // WM_APP = 0x8000
        bool cameraConnected = false;
        bool powerConnected = false;
        System.Windows.Forms.Timer t;

        IntucamHelper i;
        public CameraWindow()
        {
           InitializeComponent();
           t = new System.Windows.Forms.Timer();
          t.Interval = 200;
            t.Tick+=t_Tick;
        }

        void t_Tick(object sender, EventArgs e)
        {
            isCheckingPower = true;
        }

        protected override void WndProc(ref Message m)
        {
            try
            {
                 IVLCamVariables.CameraLogList = new List<Args>();
                if (camera == null)
                    camera = Camera.createCamera();
                if (m.Msg == WmDeviceChange)
                {
                    //if (isCheckingPower)
                    {
                        if (IVLCamVariables.intucamHelper.AppLaunched)
                        {
                            if (Convert.ToBoolean(Configuration.ConfigVariables.CurrentSettings.FirmwareSettings.PowerCameraRemovalCheck.val))
                            {
                                StartPowerCameraCheck(false);
                                //CameraPowerUpdateThreadWork();
                            }
                        }
                    }
                }
                camera.callWndProc(ref m);
                base.WndProc(ref m);
            }
            catch (Exception ex)
            {
                 CameraLogger.WriteException(ex, Exception_Log); ;
            }
            
        }

        /// <summary>
        /// To update the camera and power status
        /// </summary>
        //private async void CameraPowerUpdateThreadWork()
        internal  void CameraPowerUpdateThreadWork(object callBack)
        {
                if (!IVLCamVariables.isApplicationClosing )// used in order to stop events firing when application is closing to avoid any exception by sriram
                {
                    // new Thread(() =>
                    {
                        Args args = new Args();
                        Devices d = Devices.OtherDevices;

                        GetUSBDevices(d);
                      // ThreadPool.QueueUserWorkItem(o=> GetUSBDevices( d));// seperate thread for Management searcher object sometimes the search of object hangs to avoid that a new thread is added
                        
                                
                    }
                    //}).Start();

                }
               // await Task.Delay(10);
        }
        public void StartPowerCameraCheck(bool isAsync)
        {
            //
            if(isAsync)
                ThreadPool.QueueUserWorkItem(new WaitCallback(CameraPowerUpdateThreadWork));
            else
                CameraPowerUpdateThreadWork(new object());
        }
        bool isCheckingPower = false;
        private void GetUSBDevices(Devices devices)
        {
            {
                Args args = new Args();
                isCheckingPower = false;
                devices = Devices.PowerDisconnected;
                int count = 0;
                bool isBoardPresent = false;
                try//try catch is added to avoid crash if any other USB device is connectedor removed.
                {
                    //string deviceDesciption = "INTUCAM-45 FUNDUS CAMERA";
                    //string deviceID = @"USB\VID_0456&PID_0512\5&3AEB2854&0&2";
                    //using (var searcher = new ManagementObjectSearcher(@"Select Description From Win32_PnPEntity where DeviceID Like ""USB%"""))
    //                ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"Select Description From Win32_PnPEntity where Description Like ""INTUCAM%""");
    ////               foreach (ManagementObject item in new ManagementObjectSearcher(@"Select Description From Win32_PnPEntity where Description Like ""INTUCAM%""").Get())
    ////{
    ////     count++;
    ////}
    //               ManagementObjectCollection  collection = searcher.Get();
    //                //using (var searcher = )
    //                //{
    //                //    count = searcher.Get().Count;
    //                //} 
    //                if (collection.Count > 0)
    //                    devices = Devices.PowerConnected;

                    isBoardPresent = IntucamBoardCommHelper.IsBoardPresent();
                    Thread.Sleep(200);
                }
                catch (Exception ex)
                {
                    //CameraLogger.WriteException(ex, Exception_Log); ;
                    //if it catches the exception because of connecting other USB devices, ensures to continue in live.
                    devices = Devices.PowerDisconnected;
                }
                if (isBoardPresent)//checks if d returns the power or camera connected, not  other devices arrived
                {
                    devices = Devices.PowerConnected;

                }
                else
                    devices = Devices.PowerDisconnected;
                if (devices != IVLCamVariables.isPowerConnected)//checks if the previous state of connectivity is changed 
                {
                    args["isPowerConnected"] = IVLCamVariables.isPowerConnected = devices;
                }

                Camera.isCameraConnected();

                if (string.IsNullOrEmpty(IVLCamVariables.DisplayName) && IVLCamVariables.isCameraConnected != Devices.CameraDisconnected)
                    args["isCameraConnected"] = IVLCamVariables.isCameraConnected = Devices.CameraDisconnected;
                else if (!string.IsNullOrEmpty(IVLCamVariables.DisplayName) && IVLCamVariables.isCameraConnected != Devices.CameraConnected)
                    args["isCameraConnected"] = IVLCamVariables.isCameraConnected = Devices.CameraConnected;

                //System.Threading.Thread.Sleep(500);
                if (args.Count > 0)
                    IVLCamVariables.intucamHelper.CameraPowerStatusUpdate(args);
                isCheckingPower = true;
            }
        }
    }
}
