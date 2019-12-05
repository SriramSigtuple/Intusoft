using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.Util;
using System.Windows.Forms;
using INTUSOFT.EventHandler;
using Emgu.CV.CvEnum;
using System.Runtime.InteropServices;
using System.IO;
using Emgu.CV.UI;
using NLog;
using NLog.Config;
using NLog.Targets;


namespace INTUSOFT.Imaging
{
    internal class Camera : CameraAbstract, IDisposable
    {

        //private static readonly ILog log = LogManager.GetLogger(typeof(Camera));
        //private static readonly ILog log = LogManager.GetLogger("CameraLogger");
        //private static readonly Logger log = LogManager.GetLogger("INTUSOFT.Imaging");
        //private static readonly Logger captureLog = LogManager.GetLogger("INTUSOFT.Imaging.CaptureLog");
        //private static readonly Logger FrameLog = LogManager.GetLogger("INTUSOFT.Imaging.FrameLog");
        private static readonly Logger Exception_Log = LogManager.GetLogger("ExceptionLog");
        // private static readonly ILog captureLog = LogManager.GetLogger("CaptureLogger");

        public static List<Args> capture_log ;//
        public static List<Args> frameLogList;// = new List<Args>();

        /// <summary>
        /// List of bitmaps saved if raw mode is not used.
        /// </summary>
        internal List<Bitmap> CaptureImageList;
        public bool GreenLive = false;


        #region****************/////// Constants and  Variables Declaration ///////****************
        public bool isOpenCamera = false;
        public PictureBox pbx;
        public PictureBox maskOverlayPbx;
        private uint MSG_CAMEVENT = 0x8001; // WM_APP = 0x8000
        int[] exposureLUT = { 20000, 22500, 25000, 27500, 30000, 32500, 35000, 37500, 40000, 42500, 45000, 47500, 50000, 52500, 55000, 57500, 60000, 62500, 65000, 67500, 70000, 72500, 75000, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376, 77376 };
        int[] gainLUT = { 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, 210, 220, 230, 240, 250, 260, 270, 280, 290, 300, 310, 320, 330, 340, 350, 360, 370, 380, 390, 400, 410, 420, 430, 440, 450, 460, 470, 480, 490, 500 };
        byte[] StrobeLut = { 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 132, 136, 142, 146, 152, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156, 156 };

        public string LogFilePath = "";
        public int BMwidth = 1600, BMheight = 1440;
        List<int> FrameCategorySumList;
        public delegate void UpdateStatusBar(string a, EventArgs e);
        public EventArgs e = null;

        // public System.Windows.Forms.PictureBox image_pbx;
        static Camera camera;
        public static Vendor1 _vendor1;


        public IntPtr camHandle;
        public int imageIndx = 0;
        int width = 0, height = 0;
        static bool isDevicePresent = false;
        static Vendor1.DeviceV2[] arr;
        public int xLocRoi = 270;
        public int yLocRoi = 0;
        private bool disposed;
        private List<DateTime> FrameDateTime;
        private List<DateTime> CaptureFrameDateTime;
        public int timeDiff = 0;
        bool isCheckIRDone = false;
        bool isCaptureIndxComputed = false;
        int endIndx = 0;
        public int BitdepthVal = 0;
        public int DefGainExpIndx = 23;
        private Bitmap overlayImage;
        internal Bitmap RawImage;
        /// <summary>
        ///  Bitmap which is obtained after post processing.
        /// </summary>
        internal Bitmap CaptureBm;
        internal List<Byte[]> RawImageList;
        #endregion

        #region****************/////// Instantiation of Camera ///////****************
        public Camera()
        {
            //image_pbx = new INTUSOFT.Custom.Controls.PictureBoxExtended();
            //image_pbx = new System.Windows.Forms.PictureBox();
            capture_log = new List<Args>();
            frameLogList = new List<Args>();
            IVLCamVariables.PostProcessing = PostProcessing.GetInstance();
            RawImageList = new List<byte[]>();
            CaptureImageList = new List<Bitmap>();
           
            if (IVLCamVariables.CurrentCameraVendor == CameraVendors.Vendor1)// if the current camera is vendor1
            {
                arr = Vendor1.EnumV2();
                if (arr.Length <= 0)
                {
                    isDevicePresent = false;
                    Args logArg = new Args();
                    
                    logArg["TimeStamp"] = DateTime.Now;
                    logArg["Msg"] = "Device not present in the device array";
                    //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add("Device not present in the device array");
                    //log.Info();
                     //IVLCamVariables.CameraLogList.Add(logArg);

                }
                else
                {
                    
                    if (FrameDateTime == null)// create a new list to save the capture frames date time
                        FrameDateTime = new List<DateTime>();
                    if (CaptureFrameDateTime == null)
                        CaptureFrameDateTime = new List<DateTime>();
                    Args logArg = new Args();
                    
                    logArg["TimeStamp"] = DateTime.Now;
                    logArg["Msg"] = string.Format("Device  present in the device array{0}  ", arr[0]);
                    string str = string.Format("Device  present in the device array{0}  ", arr[0] );
                    //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);
                     //IVLCamVariables.CameraLogList.Add(logArg);
                    isDevicePresent = true;
                  
                }
            }
           
        }
        public void Dispose()
        {
            try
            {
                CloseCamera();
                this.camHandle = IntPtr.Zero;
                //this.image_pbx = null;

                this.Dispose(true);
                GC.SuppressFinalize(this);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
            } 
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (!disposed)
                {
                    if (disposing)
                    {
                    }

                }
                disposed = true;
            }
            catch ( Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
            }
            
        }

        public static Camera createCamera()
        {
            try
            {
                if (camera == null)
                {
                    camera = new Camera();
                }
                
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
            }
            IVLCamVariables.ivl_Camera = camera;
            return camera;
        }

        #endregion



        public void OnEventExposure()
        {
            try
            {
                uint val = 0;
                GetExposure(ref val);
                Args logArg = new Args();

                logArg["TimeStamp"] = DateTime.Now;
                logArg["Msg"] = string.Format("Exposure ={0}", val);
                //logArg["callstack"] = Environment.StackTrace;
                if (IVLCamVariables.isCapturing)
                    capture_log.Add(logArg);
                IVLCamVariables.CameraLogList.Add(logArg);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
            } 
        }

        #region****************/////// Live Mode ///////****************

        public static Devices isCameraConnected()
        {
            try
            {
                if (IVLCamVariables.CurrentCameraVendor == CameraVendors.Vendor1)
                {
                    IVLCamVariables.DisplayName = string.Empty;

                    arr = Vendor1.EnumV2();

                    if (arr.Length > 0)
                    {
                        IVLCamVariables.DisplayName = arr[0].displayname;
                        return Devices.CameraConnected;
                    }
                    else
                    {

                        return Devices.CameraDisconnected;
                    }
                }
                else
                {
             
                    return Devices.CameraDisconnected;
                }
            }
            catch (Exception ex)
            {
                CameraLogger.WriteException(ex, Exception_Log);
                //                 CameraLogger.WriteException(ex, Exception_Log);
                return Devices.CameraDisconnected;
            }
            
        }

        public bool StartStrobeMode()
        {
            if (IVLCamVariables.isCameraConnected == Devices.CameraConnected)
            {
                if (ret = !_vendor1.StartPullModeWithWndMsg(camHandle, MSG_CAMEVENT))// initiate the strobe function  to start live mode
                {
                    Args logArg = new Args();

                    logArg["TimeStamp"] = DateTime.Now;
                    logArg["Msg"] = "failed to start device ";
                    logArg["callstack"] = Environment.StackTrace;

                    //IVLCamVariables.CameraLogList.Add(logArg);
                    //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);
                    //log.Debug("failed to start device");
                    //MessageBox.Show("failed to start device");
                    IVLCamVariables.isLive = false;
                }
                else
                {
                    BMwidth = BMwidth - (IVLCamVariables._Settings.CameraSettings.roiX * 2);
                    ret = _vendor1.put_Roi((uint)IVLCamVariables._Settings.CameraSettings.roiX, (uint)IVLCamVariables._Settings.CameraSettings.roiY, (uint)BMwidth, (uint)BMheight);// for prime set the roi to be 4.7 mp not 6mp i.e imaging area

                    if (IVLCamVariables._Settings.CameraSettings.isRawMode)
                    {
                        if (IVLCamVariables._Settings.CameraSettings.isFourteen)
                            bytes = new byte[BMwidth * BMheight * 2];
                        else
                            bytes = new byte[BMwidth * BMheight];

                    }
                    // if the Capture image is null or if the width height do not match with sensor set height create a new bitmap

                    // if the raw image is null or if the width height do not match with sensor set height create a new bitmap
                    if (RawImage == null ||
                       RawImage.Width != BMwidth ||
                       RawImage.Height != BMheight)
                        RawImage = new Bitmap(BMwidth, BMheight, PixelFormat.Format24bppRgb);

                    if (CaptureBm == null)
                        CaptureBm = new Bitmap(BMwidth, BMheight, PixelFormat.Format24bppRgb);

                    if (IVLCamVariables.IRImage == null)
                        IVLCamVariables.IRImage = new Bitmap(BMwidth, BMheight, PixelFormat.Format24bppRgb);

                    if (overlayImage == null)
                    {
                        overlayImage = new Bitmap(BMwidth, BMheight, PixelFormat.Format24bppRgb);
                        //Graphics g = Graphics.FromImage(overlayImage);
                        //g.DrawString("Capturing",
                    }
                    ret = EnableAutoExposure(false);


                    Pause(true);// Pause the live mode once start is initiated
                }
            }

            return ret;
        }
        public override bool StartLiveMode()
        {
            if (IVLCamVariables.isCameraConnected == Devices.CameraConnected)
            {
                string str = "";
                if (FrameDateTime == null)
                    FrameDateTime = new List<DateTime>();
                if (CaptureFrameDateTime == null)
                    CaptureFrameDateTime = new List<DateTime>();
                if (IVLCamVariables._Settings.CameraSettings.isRawMode)
                {
                    if (RawImageList == null)
                        RawImageList = new List<byte[]>();
                    else
                        RawImageList.Clear();
                }
                else
                {
                    if (CaptureImageList == null)
                        CaptureImageList = new List<Bitmap>();
                    else
                        CaptureImageList.Clear();
                }
                IVLCamVariables.isLive = ret = Pause(false);// Start Live by making pause to false
                if (IVLCamVariables.intucamHelper.standByTimer.Enabled)
                {
                    IVLCamVariables.intucamHelper.standByTimer.Enabled = false;
                    IVLCamVariables.intucamHelper.standByTimer.Stop();
                    IVLCamVariables.intucamHelper.standByTimer.Enabled = true;
                    IVLCamVariables.intucamHelper.standByTimer.Start();
                }
                else
                {
                    IVLCamVariables.intucamHelper.standByTimer.Enabled = true;
                    IVLCamVariables.intucamHelper.standByTimer.Start();
                }
                Args logArg = new Args();

                logArg["TimeStamp"] = DateTime.Now;
                logArg["Msg"] = string.Format("Start Live Status = {0}  ", ret);
                //logArg["callstack"] = Environment.StackTrace;
                str = string.Format("Time = {0},Start Live Status = {1}  ", DateTime.Now.ToString("HH-mm-ss-ffff"), ret);
                if (IVLCamVariables.isCapturing)
                {
                    capture_log.Add(logArg);
                    //IVLCamVariables.logClass.CaptureLogList.Add(str);
                }
                //IVLCamVariables.CameraLogList.Add(logArg);
                //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);
            }
            return ret;

        }
        public bool EnableWhiteBalance(int enableCC)
        {
            //ret = _vendor1.put_Option(Vendor1.eOPTION.OPTION_COLORMATIX, enableCC);// Disable Color Correction Matrix 
            ret = _vendor1.put_Option(Vendor1.eOPTION.OPTION_WBGAIN, enableCC);// Disable WB Gain 
            ret = _vendor1.get_Option(Vendor1.eOPTION.OPTION_WBGAIN, out enableCC);
            int temp = 0, tint = 0;
            GetTemperatureTint(ref temp, ref tint);
            return ret;
        }
        public bool EnableCameraColorMatrix(int enableCC)
        {
            ret = _vendor1.put_Option(Vendor1.eOPTION.OPTION_COLORMATIX, enableCC);// Disable Color Correction Matrix 
            ret = _vendor1.get_Option(Vendor1.eOPTION.OPTION_COLORMATIX, out enableCC);

            return ret;
        }
        public bool Pause(bool isPause)
        {
            ret = false;
            if (IVLCamVariables.isCameraConnected == Devices.CameraConnected)
            {
                ret = _vendor1.Pause(isPause);
                if (isPause && ret)
                {
                    IVLCamVariables.isLive = false;
                }
                else
                    if (!isPause && ret)
                    {
                        IVLCamVariables.isLive = true;

                    }
            }
            return ret;
        }
        public override bool StopLiveMode()
        {
            try
            {
                //if(FrameDateTime != null)
                //FrameDateTime.Clear();// Clear the frame date time if present after logging
                ret = Pause(true);
                Args logArg = new Args();

                logArg["TimeStamp"] = DateTime.Now;
                logArg["Msg"] = string.Format("Stop Live ,Return Live = {0}  ", ret);
                //logArg["callstack"] = Environment.StackTrace;
                string str = string.Format("Time = {0},Stop Live ,Return Live = {1}  ", DateTime.Now.ToString("HH-mm-ss-ffff"), ret);
                if (IVLCamVariables.isCapturing)
                    //captureLog.Info(logText);
                    capture_log.Add(logArg);
                if (IVLCamVariables.ImagingMode == ImagingMode.FFA_Plus && IVLCamVariables.ledSource != Led.Blue)
                {
                    ServoReset();
                }
                //if (IVLCamVariables.isCapturing)
                //{
                //    IVLCamVariables.BoardHelper.ResumeLiveBoardCommands();
                //}
                if (IVLCamVariables.intucamHelper.standByTimer.Enabled)
                {
                    IVLCamVariables.intucamHelper.standByTimer.Enabled = false;
                    IVLCamVariables.intucamHelper.standByTimer.Stop();
                    IVLCamVariables.intucamHelper.standByTimer.Enabled = true;
                    IVLCamVariables.intucamHelper.standByTimer.Start();
                }
                else
                {
                    IVLCamVariables.intucamHelper.standByTimer.Enabled = true;
                    IVLCamVariables.intucamHelper.standByTimer.Start();
                }
                    //IVLCamVariables.logClass.CaptureLogList.Add(str);
                 //IVLCamVariables.CameraLogList.Add(logArg);
                //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);

                return ret;
            }
            catch (Exception ex)
            {
                var stringBuilder = new StringBuilder();

                while (ex != null)
                {
                    stringBuilder.AppendLine(ex.Message);
                    stringBuilder.AppendLine(ex.StackTrace);

                    ex = ex.InnerException;
                }
               CameraLogger.WriteException(ex, Exception_Log);
//                Exception_Log.Info(stringBuilder.ToString());
                //MessageBox.Show(ex.Message);
                return false;
            }
        }
        private void servoCallback(object callBack)
        {
            IVLCamVariables.BoardHelper.GreenFilterMove(IVLCamVariables._Settings.BoardSettings.GreenFilterPos);
        }
        internal void ServoReset()
        {
            if (IVLCamVariables.isBlueFilteredMoved)
            {
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(servoCallback));
                IVLCamVariables.isBlueFilteredMoved = false;

            }
        }
        private void ApplyOverlay(Bitmap bm)
        {
            if (IVLCamVariables.isLive)
            {
                //Graphics g = Graphics.FromImage(IVLCamVariables.overlayImage);
                //IVLCamVariables.TimeTaken = new System.Diagnostics.Stopwatch();
                //IVLCamVariables.TimeTaken.Start();
                //Bitmap overlayImage = RawImage.Clone() as Bitmap;
                //if (IVLCamVariables.EnableEdgeDetection)
                //    IVLCamVariables.PostProcessing.GetEdgeDetection(ref overlayImage, IVLCamVariables._Settings.PostProcessingSettings.edgeDetectionSettings.thresholdValue, IVLCamVariables._Settings.PostProcessingSettings.edgeDetectionSettings.thresholdLinkVal);
                //  g.DrawImage(RawImage, new Rectangle(0, 0, RawImage.Width, RawImage.Height));

                INTUSOFT.Imaging.ImagingMode imagingMode = IVLCamVariables.ImagingMode;
                Graphics overlayGraphics = Graphics.FromImage(bm);
                //overlayGraphics.DrawImage(bm, new Rectangle(0, 0, RawImage.Width, RawImage.Height));------
                //if (IVLCamVariables._Settings.PostProcessingSettings.maskSettings.isApplyLiveMask)
                //    IVLCamVariables.PostProcessing.ApplyMask(ref overlayImage, IVLCamVariables._Settings.PostProcessingSettings.maskSettings);

                //IVLCamVariables.TimeTaken.Stop();
                //System.Diagnostics.Trace.WriteLine("on event image time = " + IVLCamVariables.TimeTaken.ElapsedMilliseconds.ToString());

                //if (!IVLCamVariables.isCapturing && IVLCamVariables.isLive)
                {
                    #region To display Overlay
                    if (imagingMode == INTUSOFT.Imaging.ImagingMode.Posterior_Prime || imagingMode == Imaging.ImagingMode.Posterior_45 || imagingMode == ImagingMode.FFA_Plus || imagingMode == ImagingMode.FFAColor)// FFA_Plus and FFAColor for the overlay by sriram
                    {
                        if (IVLCamVariables._Settings.PostProcessingSettings.overlaySettings.ShowPosteriorOverlay)
                        {
                            overlayGraphics.DrawLine(new Pen(Color.FromArgb(29, 132, 150), 5f), new System.Drawing.Point(0, BMheight / 2), new System.Drawing.Point(BMwidth, BMheight / 2));
                            overlayGraphics.DrawLine(new Pen(Color.FromArgb(29, 132, 150), 5f), new System.Drawing.Point(BMwidth / 2, 0), new System.Drawing.Point(BMwidth / 2, BMheight));
                        }
                        if (IVLCamVariables._Settings.PostProcessingSettings.overlaySettings.ShowOpticDiscOverlay)
                        {
                            if (IVLCamVariables.leftRightPos == LeftRightPosition.Right)
                                overlayGraphics.DrawEllipse(new Pen(Color.FromArgb(29, 132, 150), 5f), new Rectangle(BMwidth - 750, (BMheight / 2) - 250, 500, 500));

                            else
                                overlayGraphics.DrawEllipse(new Pen(Color.FromArgb(29, 132, 150), 5f), new Rectangle(250, (BMheight / 2) - 250, 500, 500));
                        }
                    }
                    else if (imagingMode == INTUSOFT.Imaging.ImagingMode.Anterior_Prime || imagingMode == Imaging.ImagingMode.Anterior_45
                 || imagingMode == Imaging.ImagingMode.Anterior_FFA)
                    {
                        if (IVLCamVariables._Settings.PostProcessingSettings.overlaySettings.ShowAnteriorOverlay)
                        {
                            int circleWidth = 500;
                            int circleWidth1 = 400;
                            overlayGraphics.DrawEllipse(new Pen(Color.Green, 5f), (BMwidth / 2) - circleWidth1 / 2, BMheight / 2 - circleWidth1 / 2, circleWidth1, circleWidth1);
                            //points[7] = new PointF(overlay.Width / 2 - circleWidth, overlay.Height / 2 + circleWidth);
                            //overlayGraphics.DrawCurve(new Pen(Color.FromArgb(29, 132, 150), 5f), points);
                            // overlayGraphics.DrawString("(", new System.Drawing.Font(FontFamily.GenericSansSerif, 1000f, FontStyle.Regular), Brushes.Red, new PointF(overlay.Width / 2 - circleWidth, overlay.Height / 2 - circleWidth / 2));
                            // overlayGraphics.DrawString(")", new System.Drawing.Font(FontFamily.GenericSansSerif, 1000f, FontStyle.Regular), Brushes.Red, new PointF(overlay.Width / 2 + circleWidth, overlay.Height / 2 - circleWidth / 2));
                            overlayGraphics.DrawArc(new Pen(Color.Red, 5f),
                                new Rectangle((BMwidth / 2) - circleWidth, (BMheight / 2) - circleWidth, 2 * circleWidth, 2 * circleWidth), 135, 90);
                            overlayGraphics.DrawArc(new Pen(Color.Red, 5f),
                               new Rectangle((BMwidth / 2) - circleWidth, (BMheight / 2) - circleWidth, 2 * circleWidth, 2 * circleWidth), -45, 90);
                        }
                    }
                    #endregion
                    
                }
                //Args arg = new Args();
                //arg["rawImage"] = overlayImage;//.Clone();// IVLCamVariables.CaptureBm;
                if (CurrentGain != IVLCamVariables._Settings.CameraSettings.LiveGain)
                    SetGain(IVLCamVariables._Settings.CameraSettings.LiveGain);
                //IVLCamVariables.intucamHelper.motorSensorPositionEvent(IVLCamVariables.RotaryUpdateValues, IVLCamVariables.RotaryUpdateValues + IVLCamVariables.rotaryZeroDOffsetValue);
                pbx.Image = bm.Clone() as Bitmap;
                bm.Dispose();
                //IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.DisplayImage, arg);

            }
        }
        #endregion
        public bool ChangeBitDepth(BitDepth depth)
        {
            try
            {
                int depthVal = 0;
                switch (depth)
                {
                    case BitDepth.Depth_8:
                        {
                            depthVal = 0;
                            break;
                        }
                    case BitDepth.Depth_10:
                        {
                            depthVal = (int)Vendor1.eFLAG.FLAG_BITDEPTH10;
                            break;
                        }
                    case BitDepth.Depth_12:
                        {
                            depthVal = (int)Vendor1.eFLAG.FLAG_BITDEPTH12;
                            break;
                        }
                    case BitDepth.Depth_14:
                        {
                            depthVal = (int)Vendor1.eFLAG.FLAG_BITDEPTH14;
                            break;
                        }
                    case BitDepth.Depth_16:
                        {
                            depthVal = (int)Vendor1.eFLAG.FLAG_BITDEPTH16;
                            break;
                        }
                    default:
                        {
                            depthVal = 0;
                            break;
                        }
                }
                if (_vendor1 != null)
                {
                    ret = _vendor1.put_Option(Vendor1.eOPTION.OPTION_BITDEPTH, depthVal);
                    _vendor1.get_Option(Vendor1.eOPTION.OPTION_BITDEPTH, out BitdepthVal);
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
            }
            return ret;
            
        }


        public bool SetRGBGain(int[] RGB)
        {
            bool retVal= false;
            try
            {
                 retVal = _vendor1.put_WhiteBalanceGain(RGB);
            }
                
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log); 
                retVal=false;
            }
            return retVal;
            
        }
        public override bool GetRGBGain(int[] RGB)
        {
            bool retVal = false;
            try
            {
                 retVal = _vendor1.get_WhiteBalanceGain(RGB);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
            } 
            return retVal;
        }
        // public static void 

        public bool OpenCamera()
        {
            bool ret = false;
            try
            {
                arr = Vendor1.EnumV2();
                if (arr.Length > 0)
                {

                    //if (_vendor1 == null)
                    //    _vendor1 = new Vendor1();
                    string str = "";
                    //bool ret = myCamera.Open("@" + arr[0].id);// For RGB mode
                     _vendor1 =  Vendor1.Open(arr[0].id);//Temperature tint mode for white balance
                  
                    isOpenCamera = ret = (_vendor1!=null);
                    if (ret)
                    {
                        str = string.Format("open Camera {0}  ", ret );
                        Args logArg = new Args();

                        logArg["TimeStamp"] = DateTime.Now;
                        logArg["Msg"] = string.Format("open Camera {0}  ", ret);
                        logArg["callstack"] = Environment.StackTrace;
                        //IVLCamVariables.CameraLogList.Add(logArg);
                        //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);

                        ret = _vendor1.put_Option(Vendor1.eOPTION.OPTION_THREAD_PRIORITY, 0x02);// THREAD_PRIORITY_HIGHEST

                        EnableWhiteBalance(0);// Disable white balance(temperature tint) Matrix 
                        EnableCameraColorMatrix(0);
                        //ret =  _vendor1.put_Option(Vendor1.eOPTION.OPTION_COLORMATIX, 0);
                        ret = _vendor1.put_Option(Vendor1.eOPTION.OPTION_CURVE, 1);// Disable Curve 
                        ret = _vendor1.put_Option(Vendor1.eOPTION.OPTION_LINEAR, 1);// Disable Linear curve
                        if (IVLCamVariables._Settings.CameraSettings.isRawMode)
                        {
                            ret = _vendor1.put_Option(Vendor1.eOPTION.OPTION_RAW, 0x01);// 0x00 for RGB mode , 0x01 for raw mode
                        }
                        else
                            ret = _vendor1.put_Option(Vendor1.eOPTION.OPTION_RAW, 0x00);// 0x00 for RGB mode , 0x01 for raw mode

                        _vendor1.put_Speed((ushort)_vendor1.MaxSpeed);

                        if (ret = _vendor1.get_Size(out width, out height))
                        {
                            BMwidth = width;

                            BMheight = height;

                            ret = StartStrobeMode();
                        }

                    }
                    return ret;
                }
                else
                {
                    return ret = false;
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
                ret = false;
            }
            return ret;
           
        }

        public void UpdateExposureGainFromTable(int AnalogVal)
        {
            try
            {
                if (AnalogVal < exposureLUT.Length)
                {
                    if (IVLCamVariables._Settings.CameraSettings.CameraModel != CameraModel.D)
                        SetExposure((uint)exposureLUT[AnalogVal]);
                    else
                    {
                        uint val = 0;
                        GetExposure(ref val);
                        if(IVLCamVariables._Settings.CameraSettings.CaptureExposure != val)
                            SetExposure(IVLCamVariables._Settings.CameraSettings.CaptureExposure);

                    }
                    SetGain((ushort)gainLUT[AnalogVal]);
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
            }
            
        }

        public int[] GetExposureGainFromTable(int AnalogVal)
        {
            try
            {
                int[] gainExpArr = new int[2];
                if (AnalogVal < exposureLUT.Length)
                {
                    gainExpArr[0] = exposureLUT[AnalogVal];
                    gainExpArr[1] = gainLUT[AnalogVal];
                    
                }
                return gainExpArr;

            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
                return new int[2];
            }
        }


        public int GetIndexExpGainValFromLUT(int gain, int exp)
        {
            try
            {
                int expIndx = exposureLUT.ToList().FindIndex(x => (x == exp));
                int gainIndx = gainLUT.ToList().FindLastIndex(x => (x == gain));
                if (expIndx < DefGainExpIndx && gainIndx == DefGainExpIndx)
                    return expIndx + 1;
                else
                    return gainIndx + 1;
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
                return 0;
            }
            

        }
        public byte GetStrobeWidth(int indx)
        {
            byte ret = 0;
            try
            {
                ret=StrobeLut[indx];
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
                
            }
            return ret;
        }
        public void SetColorMode(bool isColor)
        {
            try
            {
                _vendor1.put_Chrome(isColor);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }
           
        }
        public bool RotateHorizontal(bool isRotate)
        {
            bool retVal = false;
            try
            {
                if (IVLCamVariables.isCameraConnected == Devices.CameraConnected)

                retVal =  _vendor1.put_HFlip(isRotate);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log); 
            }
            return retVal;

        }
        public bool RotateVertical(bool isRotate)
        {
            bool retVal = false;

            try
            {

                if (IVLCamVariables.isCameraConnected == Devices.CameraConnected)
                    retVal = _vendor1.put_VFlip(isRotate);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);   
            }
            return retVal;
        }

        public override void CloseCamera()
        {
            try
            {
                if (IVLCamVariables.CurrentCameraVendor == CameraVendors.Vendor1)
                {
                    if (IVLCamVariables.isLive)
                        StopLiveMode();

                    ServoReset();
                   bool returnValue = _vendor1.Stop();

                    CurrentExposure = 0;//reset of current exposure to 0 when camera is disconnected.
                    CurrentGain = 0;//reset of current gain to 0 when camera is disconnected.
                    _vendor1.Close();
                    isOpenCamera = false;
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log); 
            } 
        }

        #region****************/////// Frame Rate ///////****************

        #endregion

        #region****************/////// Frame Grab ///////****************
        BitmapData bdata;
        Image<Gray, byte> inp1;
        byte[] bytes;

        public bool isCapture = false;
        public bool isVflip = false;
        public bool isHflip = false;

        #region****************/////// WND PROC  ///////****************

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        
        void WndProc(ref System.Windows.Forms.Message m)
        {
            try
            {
                string str = "";
                if (MSG_CAMEVENT == m.Msg)
                {

                    #region windows events for vendor1
                    switch ((Vendor1.eEVENT)m.WParam.ToInt32())
                    {
                        case Vendor1.eEVENT.EVENT_ERROR:
                            {
                                Args logArg = new Args();

                                logArg["TimeStamp"] = DateTime.Now;
                                logArg["Msg"] = "Event Error ";
                                //logArg["callstack"] = Environment.StackTrace;
                                str = string.Format("Time = {0},Event Error  ", DateTime.Now.ToString("HH-mm-ss-ffff"));
                                if (IVLCamVariables.isCapturing)
                                    //IVLCamVariables.logClass.CaptureLogList.Add(str);
                                    capture_log.Add(logArg);
                                 //IVLCamVariables.CameraLogList.Add(logArg);
                                //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);

                                break;
                            }
                            
                        case Vendor1.eEVENT.EVENT_DISCONNECTED:// vendor1 windows event to indicate device has been disconnected
                            {
                                Args arg = new Args();
                                arg["isCameraConnected"] = false;
                                break;
                            }
                        case Vendor1.eEVENT.EVENT_EXPOSURE:// windows event to indicate change in exposure
                            OnEventExposure();
                            break;
                        case Vendor1.eEVENT.EVENT_IMAGE:// windows event to indicate image arrival
                            {
                                IVLCamVariables.CurrentFrameCnt++;
                                FrameDateTime.Add(DateTime.Now);
                               

                                if (FrameDateTime.Count > 1)
                                {
                                    TimeSpan t = FrameDateTime[FrameDateTime.Count - 1] - FrameDateTime[FrameDateTime.Count - 2];
                                    timeDiff = t.Milliseconds;

                                }
                                if (IVLCamVariables.isCapturing)
                                    CaptureFrameDateTime.Add(FrameDateTime[FrameDateTime.Count - 1]);
                                String frameDetails = string.Format("Frame Recieved, {0}, and Time Diff, {1} ,frame Time = ,{2}", (FrameDateTime.Count - 1), timeDiff, FrameDateTime[FrameDateTime.Count - 1].ToString("HH-mm-ss-ffff"));
                                
                                //if (IVLCamVariables.IsFlashOnDone)
                                //{
                                //    if (!IVLCamVariables.IsFlashOffDone)// if flash off done not recieved
                                //    {
                                //       IVLCamVariables.FrameBetweenFlashOnFlashOffDoneCnt = IVLCamVariables.FrameBetweenFlashOnFlashOffDoneCnt+1;
                                //    }

                                //}



                                Args logArg = new Args();
                                logArg["TimeStamp"] = DateTime.Now;
                                logArg["Msg"] = string.Format("FrameDetails = {0},  Capture Index ={1}, flash off done status = {2}, flash on done status = {3}  ", frameDetails, /*IVLCamVariables.FrameBetweenFlashOnFlashOffDoneCnt,*/ IVLCamVariables.CaptureImageIndx, IVLCamVariables.IsFlashOffDone, IVLCamVariables.IsFlashOnDone);
                                //logArg["callstack"] = Environment.StackTrace;
                                frameLogList.Add(logArg);

                                if (IVLCamVariables.isCapturing)
                                {

                                    //logArg["TimeStamp"] = DateTime.Now;
                                    //logArg["Msg"] = string.Format("FrameDetails = {0}, Peak Frame Count ={1}, Capture Index ={2}, flash off done status = {3}, flash on done status = {4}  ", frameDetails, IVLCamVariables.FrameBetweenFlashOnFlashOffDoneCnt, IVLCamVariables.CaptureImageIndx, IVLCamVariables.IsFlashOffDone, IVLCamVariables.IsFlashOnDone);
                                    //logArg["callstack"] = Environment.StackTrace;
                                    capture_log.Add(logArg);
                                    //IVLCamVariables.logClass.CaptureLogList.Add();
                                    //captureLog.Info(string.Format("FrameDetails = {0}, Peak Frame Count ={1}, Capture Index ={2}, flash off done status = {3}", frameDetails, IVLCamVariables.FrameBetweenFlashOnFlashOffDoneCnt, IVLCamVariables.CaptureImageIndx, IVLCamVariables.IsFlashOffDone));
                                    
                                    //IVLCamVariables.logClass.CaptureLogList.Add();

                                }
                                 //IVLCamVariables.CameraLogList.Add(logArg);
                                    //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(frameDetails);
                                    //IVLCamVariables.logClass.FrameLogList.Add(frameDetails);
                                OnEventImage();// method used to get the image from the sensor once the event is fired


                                break;
                            }
                    }
                    #endregion

                }
        
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log); 
            }
        }

        public void callWndProc(ref System.Windows.Forms.Message m)
        {
            try
            {
                WndProc(ref m);
            }
            catch (Exception ex)
            {
                 CameraLogger.WriteException(ex, Exception_Log);  
            } 
        }
        #endregion

        List<double> intensityVal = new List<double>();

        private bool RawFrameCategorization(int indx)
        {
            bool identifier = false;
            try
            {
                if (IVLCamVariables._Settings.CameraSettings.CameraModel == CameraModel.A)// if the camera is of model A type
                {
                    if (IVLCamVariables.imageCatList[indx] == ImageCategory.Flash)
                    {
                        if (IVLCamVariables.imageCatList[indx - 1] == ImageCategory.Dark ||
                            IVLCamVariables.imageCatList[indx - 1] == ImageCategory.GB ||
                            IVLCamVariables.imageCatList[indx - 1] == ImageCategory.GR)
                        {
                            IVLCamVariables.FlashIndx = indx;
                            IVLCamVariables.captureImageType = CapturedImageType.Flash;
                           // StopLiveMode();

                             identifier = true;
                        }
                        else  identifier = false;
                    }
                    else if (IVLCamVariables.imageCatList[indx] == ImageCategory.GR)
                    {
                        if (IVLCamVariables.imageCatList[indx - 1] == ImageCategory.GB)
                        {
                            IVLCamVariables.GBIndx = indx - 1;
                            IVLCamVariables.GRIndx = indx;
                            IVLCamVariables.captureImageType = CapturedImageType.GbGr;
                            //StopLiveMode();
                             identifier = true;
                        }
                        else  identifier = false;
                    }
                    else
                         identifier = false;
                }
                else if (IVLCamVariables._Settings.CameraSettings.CameraModel == CameraModel.B)
                {
                    if (IVLCamVariables.imageCatList[indx] == ImageCategory.Flash)
                    {
                        if (IVLCamVariables.liveLedCode == LedCode.Flash && (IVLCamVariables.imageCatList[indx - 1] == ImageCategory.Dark ||
                            IVLCamVariables.imageCatList[indx - 1] == ImageCategory.GB ||
                            IVLCamVariables.imageCatList[indx - 1] == ImageCategory.GR ||IVLCamVariables.imageCatList[indx -1 ] == ImageCategory.Flash)) 
                        {
                            IVLCamVariables.FlashIndx = indx;
                            IVLCamVariables.captureImageType = CapturedImageType.Flash;
                            identifier = true;
                              
                        }
                        else if (IVLCamVariables.imageCatList[indx - 1] == ImageCategory.Dark ||
                            IVLCamVariables.imageCatList[indx - 1] == ImageCategory.GB ||
                            IVLCamVariables.imageCatList[indx - 1] == ImageCategory.GR)
                        {
                                IVLCamVariables.FlashIndx = indx;
                                IVLCamVariables.captureImageType = CapturedImageType.Flash;
                             identifier = true;//isFlashImage;
                        }
                        else
                             identifier = false;
                    }
                    else if (IVLCamVariables.imageCatList[indx] == ImageCategory.GB)
                    {

                        if (IVLCamVariables.imageCatList[indx - 1] == ImageCategory.GR)
                        {
                            IVLCamVariables.GBIndx = indx;
                            IVLCamVariables.GRIndx = indx - 1;
                            //if (IVLCamVariables.GRIndx <= 1 && IVLCamVariables.imageCatList[0] != ImageCategory.Dark)
                            //    return false;
                            IVLCamVariables.captureImageType = CapturedImageType.GbGr;
                             identifier = true;
                        }
                        else
                             identifier = false;
                    }
                    else  identifier = false;
                }
                else
                    identifier = false;
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
                identifier = false;
            }
            return identifier;
        }
        GCHandle pinnedArray1 = new GCHandle();
        IntPtr pointer1 = IntPtr.Zero;
        bool isCaptureFrameRecievedDuringSave = false;
        public override void OnEventImage()
        {
            try
            {
                Bitmap bm = new Bitmap(BMwidth, BMheight, PixelFormat.Format24bppRgb);

                bool retVal = false;
                
                {

                    uint bmWidth = 0;
                    uint bmHeight = 0;


                    if ( IVLCamVariables._Settings.CameraSettings.isRawMode)
                    {

                    //    Array.Clear(bytes, 0, bytes.Length);// this is used to clear the byte array so that previous data is not used as new data in case camera frame event failure
                        pinnedArray1 = GCHandle.Alloc(bytes, GCHandleType.Pinned);
                        pointer1 = pinnedArray1.AddrOfPinnedObject();
                        retVal = _vendor1.PullImage(pointer1, 8, out bmWidth, out bmHeight);
                        if (!IVLCamVariables.isCapturing)
                        
                        {
                            bdata = bm.LockBits(new Rectangle(0, 0, BMwidth, BMheight), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

                            if (retVal && bmWidth == BMwidth && bmHeight == BMheight)// if the pull image from the sensor is completed without failure
                            {
                                PostProcessing.ImageProc_Demosaic(pointer1, pointer1, bdata.Scan0, RawImage.Width, RawImage.Height, true);

                                //log.DebugFormat("Frame transfer Status = {0}, Frame Width = {1}, Frame Height = {2}", retVal, bmWidth, bmHeight);// log the frame failure
                            }

                            // RawImage.UnlockBits(bdata);
                            bm.UnlockBits(bdata);
                            if (IVLCamVariables.ImagingMode == ImagingMode.FFA_Plus && IVLCamVariables.ledSource == Led.Blue)
                            {
                                IVLCamVariables.PostProcessing.GetMonoChromeImage(ref bm);
                            }
                            pinnedArray1.Free();
                            //log.DebugFormat("Frame transfer Status = {0}, Frame Width = {1}, Frame Height = {2}", retVal, bmWidth, bmHeight);// log the frame failure
                            RotateRawImage();
                            // IVLCamVariables.PostProcessing.ApplyPostProcessing(ref RawImage);
                            if (IVLCamVariables._Settings.CameraSettings.EnableLiveImageProcessing)
                            {
                                if (!IVLCamVariables.isCapturing)
                                {
                                    if (IVLCamVariables._Settings.PostProcessingSettings.gammaCorrectionSettings.ApplyGammaCorrection)
                                        IVLCamVariables.PostProcessing.ApplyGammaCorrection(ref bm, IVLCamVariables._Settings.PostProcessingSettings.gammaCorrectionSettings);
                                    IVLCamVariables.PostProcessing.BoostImage(ref bm);


                                    if (IVLCamVariables._Settings.PostProcessingSettings.ccSettings.isApplyLiveColorCorrection)
                                        IVLCamVariables.PostProcessing.Applycolorcorrection(ref bm, IVLCamVariables._Settings.PostProcessingSettings.ccSettings,IVLCamVariables.isColor);

                                    IVLCamVariables.PostProcessing.Denoise(ref bm, IVLCamVariables._Settings.PostProcessingSettings.unsharpMaskSettings);
                                }
                            }
                        }
                        // IVLCamVariables.PostProcessing.ApplyClahe(ref RawImage,  IVLCamVariables._Settings.PostProcessingSettings.claheSettings);
                    }

                    else
                    {
                        bdata = bm.LockBits(new Rectangle(0, 0, BMwidth, BMheight), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

                        retVal = _vendor1.PullImage(bdata.Scan0, 24, out bmWidth, out bmHeight);
                        bm.UnlockBits(bdata);
                        //pbx.Image = RawImage;
                        //return;
                        //if (IVLCamVariables.isCapturing && IVLCamVariables._Settings.ImageSaveSettings.isIR_ImageSave && !IVLCamVariables.isIRSaved)
                        //{
                        //    IVLCamVariables.isIRSaved = true;
                        //    IVLCamVariables.IRImage = bm.Clone() as Bitmap;
                        //    //Graphics g = Graphics.FromImage(IVLCamVariables.IRImage);
                        //    //g.DrawImage(RawImage, new Rectangle(0, 0, BMwidth, BMheight));
                        //}  
                    }
                    #region Raw To Bitmap conversion
                    if (!IVLCamVariables.isCapturing && !IVLCamVariables.isResuming)// && IVLCamVariables.isCameraConnected == Devices.CameraConnected && IVLCamVariables.isPowerConnected == Devices.PowerConnected)
                    {
                            ApplyOverlay(bm);
                    }
                    else if (IVLCamVariables.isCapturing && !IVLCamVariables.isResuming)
                    {
                        //pbx.Image = overlayImage;
                        #region Save Frames
                        #region raw frame add to list and categorization
                        if (IVLCamVariables._Settings.CameraSettings.isRawMode)
                        {
                            if (IVLCamVariables.IsFlashOnDone)
                            {
                                RawImageList.Add(bytes.Clone() as byte[]);// Add the raw bytes to the rawImageList

                                SetFrameType();
                                int catCnt = IVLCamVariables.imageCatList.Count - 1;
                                int categoryIndexCheck = 0;
                                if (IVLCamVariables.imageCatList.Count < IVLCamVariables._Settings.CameraSettings.SaveFramesCnt)
                                {
                                    #region Model C & D capture frame type categorization
                                    if (IVLCamVariables._Settings.CameraSettings.CameraModel == CameraModel.C || IVLCamVariables._Settings.CameraSettings.CameraModel == CameraModel.D)
                                    {
                                        if (IVLCamVariables.IsFlashOffDone)//if flash off is done
                                        {

                                            if (IVLCamVariables.CaptureImageIndx == -1 )//if capture image index not equals to '-1'.
                                            {
                                                IVLCamVariables.CaptureImageIndx = IVLCamVariables.imageCatList.Count - 1;
                                            }
                                            if (IVLCamVariables.CaptureImageIndx != -1)
                                            {
                                                if (IVLCamVariables.imageCatList.Count > (IVLCamVariables.CaptureImageIndx + 1))
                                                {
                                                    IVLCamVariables.captureImageType = CapturedImageType.Flash;
                                                    if(IVLCamVariables.isLive)
                                                    GetCaptureFrame();

                                                }
                                            }
                                            else
                                            {
                                                CaptureFailureNotify();
                                            }
                                        }
                                    }
                                    #endregion
                                    #region Model A and B capture Frame type categorization
                                    else
                                    {
                                        if (IVLCamVariables.IsFlashOffDone)
                                        {


                                            if (catCnt > categoryIndexCheck)//this is added to avoid '0' th index categorization in other words categorization starts when more than one frame is received by Kishore on 26 September 2017.
                                            {
                                                if (RawFrameCategorization(catCnt))
                                                {
                                                    GetCaptureFrame();
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    CaptureFailureNotify();
                                }
                            }
                        }
                        #endregion
                        else
                        {
                            if (IVLCamVariables.IsFlashOnDone)
                            {
                                CaptureImageList.Add(bm.Clone() as Bitmap);
                                //captureLog.Info(string.Format("Capture List = {0}", IVLCamVariables.CaptureImageList.Count - 1));
                                Args logArg = new Args();

                                logArg["TimeStamp"] = DateTime.Now;
                                logArg["Msg"] = string.Format("Capture List = {0}  ", (CaptureImageList.Count));
                                //logArg["callstack"] = Environment.StackTrace;
                                capture_log.Add(logArg);
                                //IVLCamVariables.logClass.CaptureLogList.Add(string.Format("Time = {0}, {1}", DateTime.Now.ToString("HH-mm-ss-ffff"), str));
                                //IVLCamVariables.CameraLogList.Add(logArg);
                                //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(string.Format("Time = {0}, {1}", DateTime.Now.ToString("HH-mm-ss-ffff"), str));


                                if (CaptureImageList.Count < IVLCamVariables._Settings.CameraSettings.SaveFramesCnt)
                                {
                                    if (IVLCamVariables.IsFlashOffDone)
                                    {

                                        if (IVLCamVariables.CaptureImageIndx == -1 && CaptureImageList.Count > 1)
                                        {

                                            IVLCamVariables.CaptureImageIndx = CaptureImageList.Count - 1;
                                            logArg["TimeStamp"] = DateTime.Now;
                                            logArg["Msg"] = string.Format("Capture Index = {0}  ", IVLCamVariables.CaptureImageIndx);
                                            //logArg["callstack"] = Environment.StackTrace;
                                            capture_log.Add(logArg);
                                            GetCaptureFrame();

                                        }
                                        //else //Flash off done interrupt is received but no frames received between flash on done and flash off done.
                                        //{
                                        //    CaptureFailureNotify();

                                        //}
                                    }

                                }
                                else //Flash off done interrupt is not received hence no frames received.
                                {
                                    CaptureFailureNotify();

                                }
                            }
                        }
                        #endregion
                    }
                    #endregion

                }
                //IVLCamVariables.intucamHelper.totalFrameCount_Lbl.Text = "Time taken on event image = " + st.ElapsedMilliseconds.ToString();
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex,Exception_Log);

                // CameraLogger.WriteException(ex, Exception_Log);  
            }

        }

        /// <summary>
        /// To notify the capture failure
        /// </summary>
        private void CaptureFailureNotify()
        {
            StopLiveMode();
            IVLCamVariables.isCaptureFailure = true;
            IVLCamVariables.IsFlashOnDone = false;
            IVLCamVariables.IsFlashOffDone = false;
            IVLCamVariables.captureFailureCode = CaptureFailureCode.FrameNotReceived;
            Args logArg = new Args();
            
            logArg["TimeStamp"] = DateTime.Now;
            logArg["Msg"] = string.Format("Capture Failure = {0}  ", IVLCamVariables.captureFailureCode);
            //logArg["callstack"] = Environment.StackTrace;
            capture_log.Add(logArg);
            //IVLCamVariables.logClass.CaptureLogList.Add();
             //IVLCamVariables.CameraLogList.Add(logArg);
            //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(string.Format("Time = {0},Capture Failure = {1}  ", DateTime.Now.ToString("HH-mm-ss-ffff"), IVLCamVariables.captureFailureCode));
           // IVLCamVariables.intucamHelper.CompleteCaptureSequence();
            IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.SaveFrames2Disk, new Args());
        }

        /// <summary>
        /// To get the captured frame and to save the save the captured frame
        /// </summary>
        private void GetCaptureFrame()
        {
            StopLiveMode();

            if (IVLCamVariables.IsFlashOffDone)
                IVLCamVariables.IsFlashOffDone = false;
            IVLCamVariables.isCaptureFailure = false;
            isCaptureFrameRecievedDuringSave = true;
            System.ComponentModel.BackgroundWorker bg = new System.ComponentModel.BackgroundWorker();
            bg.DoWork += bg_DoWork;
            bg.RunWorkerCompleted += bg_RunWorkerCompleted;
            bg.RunWorkerAsync();
            //if (IVLCamVariables._Settings.CameraSettings.isRawMode)
           
            //System.Threading.ThreadPool.QueueUserWorkItem(LastFrameRecievedCallbackWait);
            //else
            //{
            //    IVLCamVariables.IsFlashOnDone = false;
            //    IVLCamVariables.IsFlashOffDone = false;
            //    FrameDetectionForManufacturersDemosaicing();
            //}
            //if (IVLCamVariables._Settings.CameraSettings.isRawMode)
            //    RawFrame2CaptureImage();// Frame Detection if its camera's demosaicing instead of raw conversion
            //else
            //    FrameDetectionForManufacturersDemosaicing();
            //IVLCamVariables.FrameBetweenFlashOnFlashOffDoneCnt = 0;

        }

        void bg_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
           
            if (IVLCamVariables._Settings.CameraSettings.isRawMode)
            {
                if (IVLCamVariables._Settings.CameraSettings.CameraModel == CameraModel.C)
                {
                    if (IVLCamVariables.CaptureImageIndx != (IVLCamVariables.imageCategoryValues.Count - 1))
                    {
                        //if (IVLCamVariables.imageCategoryValues[IVLCamVariables.CaptureImageIndx].GBGRSum < IVLCamVariables.imageCategoryValues[IVLCamVariables.imageCategoryValues.Count - 1].GBGRSum)
                        //    IVLCamVariables.CaptureImageIndx = IVLCamVariables.imageCategoryValues.Count - 1;
                        for (int i = IVLCamVariables.imageCategoryValues.Count - 1; i >= 0; i--)
                        {
                            if (IVLCamVariables.imageCatList[i] == ImageCategory.Flash)
                            {
                                IVLCamVariables.CaptureImageIndx = i;
                                break;
                            }
                        }
                    }
                }
                RawFrame2CaptureImage();// Frame Detection if its camera's demosaicing instead of raw conversion

            }
            else
            {
                {
                    List<double> valList = new List<double>();
                    for (int i = IVLCamVariables.CaptureImageIndx; i < CaptureImageList.Count; i++)
                    {
                   
                    Image<Bgr, byte> inp1 = new Image<Bgr, byte>(CaptureImageList[i]);
                    //Image<Bgr, byte> inp2 = new Image<Bgr, byte>(CaptureImageList[IVLCamVariables.CaptureImageIndx + 1]);
                    inp1.ROI = new Rectangle((inp1.Width / 2) - 100, 0, 200, inp1.Height);
                    //inp2.ROI = new Rectangle((inp1.Width / 2) - 100, 0, 200, inp1.Height);

                    double sum1 = inp1[2].GetSum().Intensity;
                    valList.Add(sum1);
                    Args logArg = new Args();

                    logArg["TimeStamp"] = DateTime.Now;
                    logArg["Msg"] = string.Format("Intensity Value = {0} Index = {1}  ", sum1,i);
                    //logArg["callstack"] = Environment.StackTrace;
                    capture_log.Add(logArg);
                    //double sum2 = inp2[2].GetSum().Intensity;
                    //if (sum1 < sum2)
                    //    IVLCamVariables.CaptureImageIndx += 1;
                    inp1.ROI = new Rectangle();
                    //inp2.ROI = new Rectangle();
                    inp1.Dispose();
                    //inp2.Dispose();
                   
                    }
                    IVLCamVariables.CaptureImageIndx = valList.IndexOf(valList.Max()) + IVLCamVariables.CaptureImageIndx ;
                    //IVLCamVariables.CaptureImageIndx = CaptureImageList.Count - 1;
                }
                FrameDetectionForManufacturersDemosaicing();
            }
            IVLCamVariables.IsFlashOnDone = false;
            IVLCamVariables.IsFlashOffDone = false;
        }

        void bg_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            LastFrameRecievedCallbackWait(new object());
        }

        private void LastFrameRecievedCallbackWait(object callback)
        {
            Args logArg = new Args();

            logArg["TimeStamp"] = DateTime.Now;
            logArg["Msg"] = "Added Delay Start  " + " Capture Image Index" + IVLCamVariables.CaptureImageIndx;
            capture_log.Add(logArg);
               while (isCaptureFrameRecievedDuringSave)
                {
                      System.Threading.Thread.Sleep(Convert.ToInt32(Configuration.ConfigVariables.CurrentSettings.CameraSettings.DelayAfterFlashOffDone.val));
                      isCaptureFrameRecievedDuringSave = false;
                }
               logArg = new Args();
               logArg["TimeStamp"] = DateTime.Now;
               logArg["Msg"] = "Added Delay Stop  " + " Capture Image Index" + IVLCamVariables.CaptureImageIndx;

               capture_log.Add(logArg);
               
        }
        private void checkIR(Bitmap bm)
        {
            try
            {
                //captureLog.Info("Check IR started");
                Args logArg = new Args();
                
                logArg["TimeStamp"] = DateTime.Now;
                logArg["Msg"] = "Check IR started ";
                //logArg["callstack"] = Environment.StackTrace;
                capture_log.Add(logArg);
                //IVLCamVariables.logClass.CaptureLogList.Add(string.Format("Time = {0},Check IR started  ", DateTime.Now.ToString("HH-mm-ss-ffff")));
                 //IVLCamVariables.CameraLogList.Add(logArg);
                //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(string.Format("Time = {0},Check IR started  ", DateTime.Now.ToString("HH-mm-ss-ffff")));


                Image<Bgr, byte> inp = new Image<Bgr, byte>(bm);
                Bgr avg;
                MCvScalar std;
                inp.AvgSdv(out avg, out std);
                double threshVal = avg.Green / avg.Blue;
                if (threshVal < IVLCamVariables._Settings.CameraSettings.IRCheckValue)
                {
                    IVLCamVariables.CaptureImageIndx += 1;
                    //captureLog.Info(string.Format("Check IR done {0}", threshVal));
                    Args logArg1 = new Args();
                    
                    logArg1["TimeStamp"] = DateTime.Now;
                    logArg1["Msg"] = string.Format("Check IR done {0}  ", threshVal);
                    //logArg["callstack"] = Environment.StackTrace;
                    capture_log.Add(logArg1);
                    //IVLCamVariables.logClass.CaptureLogList.Add();
                     //IVLCamVariables.CameraLogList.Add(logArg);
                    //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(string.Format("Time = {0},Check IR done {1}  ", DateTime.Now.ToString("HH-mm-ss-ffff"), threshVal));


                }
                inp.Dispose();
                isCheckIRDone = true;
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);  
            }
        }
        private void GetCaptureFrameIndex()
        {
            try
            {
                //captureLog.Info("Check IR started");
                Args logArg = new Args();
                
                logArg["TimeStamp"] = DateTime.Now;
                logArg["Msg"] = "Check IR started   ";
                //logArg["callstack"] = Environment.StackTrace;
                capture_log.Add(logArg);
                //IVLCamVariables.logClass.CaptureLogList.Add(string.Format("Time = {0},Check IR started  ", DateTime.Now.ToString("HH-mm-ss-ffff")));
                 //IVLCamVariables.CameraLogList.Add(logArg);
                //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(string.Format("Time = {0},Check IR started  ", DateTime.Now.ToString("HH-mm-ss-ffff")));

                List<double> avg = new List<double>();
                List<Bgr> bgrAvg = new List<Bgr>();
                List<double> GrayAvg = new List<double>();
                List<int> indexList = new List<int>();
                int startIndex = CaptureImageList.Count - 2;//to check from the last 3 images from the list.

                for (int i = CaptureImageList.Count-1; i >= startIndex; i--)
                {
                    indexList.Add(i);
                    Image<Bgr, byte> inp = new Image<Bgr, byte>(CaptureImageList[i]);
                    inp.ROI = new Rectangle((inp.Width / 2) - 10,0, 20, inp.Height);
                    Bgr avgVal = inp.GetAverage();//.Blue +inp.GetAverage().Green + inp.
                    double val = avgVal.Green + avgVal.Red + avgVal.Blue;
                    avg.Add(val);
                    inp.Dispose();
                }
                IVLCamVariables.CaptureImageIndx = indexList[avg.IndexOf(avg.Max())];//since we are for looping in decrease order the array will be in increase order, so as get the exact index from the image list start index is added.
                string str = "";
                Args logArg1 = new Args();
                
                logArg1["TimeStamp"] = DateTime.Now;
                logArg1["Msg"] = "Capture Index By frame Detection = " + IVLCamVariables.CaptureImageIndx;
                //logArg["callstack"] = Environment.StackTrace;
                capture_log.Add(logArg1);
              str = string.Format("Time = {0},Capture Index By frame Detection = {1}  ", DateTime.Now.ToString("HH-mm-ss-ffff"), IVLCamVariables.CaptureImageIndx );
               //IVLCamVariables.logClass.CaptureLogList.Add(str);
               //IVLCamVariables.CameraLogList.Add(logArg);
              //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);

            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
            }
        }

        
        /// <summary>
        /// Method To set whether the frame is IR/GB/GR/Flash/Dark
        /// </summary>
        public void SetFrameType()
        {
            try
            {
                FrameCategorySumList = new List<int>();

                int length = 0;

                length = RawImageList.Count;
                byte[] inpArr = new byte[16];

                int offsetX = BMwidth / 2, offsetY = 600;// slightly above hotspot for categorization of frames by sriram
                for (int i = 0; i < 4; i++)
                {
                    int y = (offsetY + i) * BMwidth + offsetX;
                    Array.Copy(RawImageList[RawImageList.Count - 1], y, inpArr, i * 4, 4);

                }
                int[] inputArr = null;

                inputArr = Array.ConvertAll(inpArr, c => (int)c);
                // code for managing gray level equal to zero which results in divide by zero hence those values will be replaced by 1 from 0.
                for (int i = 0; i < inputArr.Length; i++)
                {
                    if (inputArr[i] == 0)
                        inputArr[i] = 1;
                }

                int val = inputArr.Sum();
                FrameCategorySumList.Add(val);
                //float GBratio =  ((float)(inputArr[3] - inputArr[5])) / (float)inputArr[3]; // (G-B)/G 
                float GbSum = inputArr[0] + inputArr[2] + inputArr[8] + inputArr[10];
                float GRSum = inputArr[5] + inputArr[7] + inputArr[13] + inputArr[15];
                float GBbyGRratio = GRSum / GbSum; // ratio of G in GB row and G in GR row ;
                ImageCategoryHelper imageCategoryValues = new ImageCategoryHelper();
                imageCategoryValues.GBGRSum = val;
                imageCategoryValues.GBByRatio = GBbyGRratio;
                IVLCamVariables.imageCategoryValues.Add(imageCategoryValues);
                ImageCategory imageCategory;
                if (GBbyGRratio > IVLCamVariables._Settings.CameraSettings.FrameDetectionValue && val > IVLCamVariables._Settings.CameraSettings.DarkFrameDetectionVal) // for detection of GB images and also added dark frame detection value by sriram
                {
                    imageCategory = ImageCategory.GB;
                    IVLCamVariables.imageCatList.Add(imageCategory);
                }
                else if (GBbyGRratio < (float)(1.0f / IVLCamVariables._Settings.CameraSettings.FrameDetectionValue) && val > IVLCamVariables._Settings.CameraSettings.DarkFrameDetectionVal) // for detection of GR images
                {
                    imageCategory = ImageCategory.GR;
                    IVLCamVariables.imageCatList.Add(imageCategory);
                }
                else if (val < IVLCamVariables._Settings.CameraSettings.DarkFrameDetectionVal)
                {
                    imageCategory = ImageCategory.Dark;

                    IVLCamVariables.imageCatList.Add(imageCategory);

                }
                else
                {
                    imageCategory = ImageCategory.Flash;
                    IVLCamVariables.imageCatList.Add(imageCategory);

                }
                // code for managing gray level equal to zero which results in divide by zero hence those values will be replaced by 1 from 0.

                string str = imageCategory + ",";

                for (int i = 0; i < 4; i++)
                {
                    int yVal = (i) * 4;
                    for (int j = 0; j < 4; j++)
                    {
                        int xVal = j;
                        //  FrameDetailsWriter.Write(inputArr[yVal + xVal] + ",");
                        str += inputArr[yVal + xVal] + ",";
                    }
                }
                if (FrameDateTime.Count > 0)
                {
                    TimeSpan t = new TimeSpan();
                    //IVLCamVariables.FrameCaptureLog[IVLCamVariables.FrameCaptureLog.Count - 1].Comments = str;
                    if (FrameDateTime.Count > 1)
                        t = FrameDateTime[FrameDateTime.Count - 1] - FrameDateTime[FrameDateTime.Count - 2];
                    Args logArg = new Args();
                    
                    logArg["TimeStamp"] = DateTime.Now;
                    logArg["Msg"] = string.Format("Frame = {0}, Frame Time Diff = {1}, Frame Category = {2}  ",(RawImageList.Count - 1).ToString(), t.Milliseconds.ToString(), str);
                    //logArg["callstack"] = Environment.StackTrace;
                    capture_log.Add(logArg);
                    //captureLog.Info("Frame " + (RawImageList.Count - 1).ToString() + " , Frame Time Diff = " + t.Milliseconds.ToString() + "  ," + " Frame Category =  " + str);
                    //IVLCamVariables.logClass.CaptureLogList.Add();
                     //IVLCamVariables.CameraLogList.Add(logArg);
                    //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(string.Format("Time = {0}, Frame = {1}, Frame Time Diff = {2}, Frame Category = {3}  ", DateTime.Now.ToString("HH-mm-ss-ffff"), (RawImageList.Count - 1).ToString(), t.Milliseconds.ToString(), str));

                    //IVLCamVariables.CaptureLog.Add(IVLCamVariables.FrameCaptureLog[IVLCamVariables.FrameCaptureLog.Count - 1]);
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);    
            }
        }

        public override void onEventStillImage(string fileName)
        {
            try
            {
                _vendor1.Snap(0);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
            }
          
        }
        public override void Capture()
        {
            try
            {
                //  stopLiveMode();
            //setResolution(1);
            uint resNum = 0;
            _vendor1.get_eSize(out resNum);
            _vendor1.get_Resolution(resNum, out width, out height);
            CaptureBm = new Bitmap(width, height);
            BitmapData bData = CaptureBm.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            uint cWidth = 0;
            uint cHeight = 0;
            _vendor1.PullStillImage(bData.Scan0, 24, out  cWidth, out cHeight);
            CaptureBm.UnlockBits(bData);
            // StopLiveMode();
            // IntuCameraDataVariables.CaptureBm.Save(FileName);
            CaptureImageList.Add(new Bitmap(CaptureBm as Bitmap));
            CaptureBm.Dispose();
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);   
            }
        }

        public void RawFrame2CaptureImage()
        {
            try
            {
                Args arg = new Args();
                int count = 0;
                List<ushort[]> RawShortFrameList = new List<ushort[]>();
                //
                //captureLog.Info("Capture Index = {0}",IVLCamVariables.CaptureImageIndx);
                Args logArg = new Args();
                
                logArg["TimeStamp"] = DateTime.Now;
                logArg["Msg"] = "Capture Index = " + IVLCamVariables.CaptureImageIndx;
                //logArg["callstack"] = Environment.StackTrace;
                capture_log.Add(logArg);
                //IVLCamVariables.logClass.CaptureLogList.Add(string.Format("Time = {0},Capture Index = {1}  ", DateTime.Now.ToString("HH-mm-ss-ffff"), IVLCamVariables.CaptureImageIndx));
                 //IVLCamVariables.CameraLogList.Add(logArg);
                //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(string.Format("Time = {0},Capture Index = {1}  ", DateTime.Now.ToString("HH-mm-ss-ffff"), IVLCamVariables.CaptureImageIndx));

                #region Raw Mode
                if (IVLCamVariables._Settings.CameraSettings.isRawMode)
                {
                    if (IVLCamVariables._Settings.CameraSettings.isFourteen)
                    {
                        for (int k = 0; k < RawImageList.Count; k++)
                        {
                            ushort[] tempVal = new ushort[RawImageList[k].Length / 2]; // Array.Convert(src, b => (UInt16)b);
                            Buffer.BlockCopy(RawImageList[k], 0, tempVal, 0, RawImageList[k].Length);
                            RawShortFrameList.Add(tempVal);
                        }
                    }
                    else
                    {
                        count = 0;
                        //                   #region Get Captured Frame using Demosaic algorithm
                        #region 3 mega pixel raw to bitmap conversion by sriram
                        try
                        {
                            bdata = RawImage.LockBits(new Rectangle(0, 0, BMwidth, BMheight), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                        }
                        catch (Exception ex)
                        {
                            var stringBuilder = new StringBuilder();

                            while (ex != null)
                            {
                                stringBuilder.AppendLine(ex.Message);
                                stringBuilder.AppendLine(ex.StackTrace);

                                ex = ex.InnerException;
                            }
                           CameraLogger.WriteException(ex, Exception_Log);
//                            Exception_Log.Info(stringBuilder.ToString());
                            RawImage = new Bitmap(BMwidth, BMheight, PixelFormat.Format24bppRgb);
                            bdata = RawImage.LockBits(new Rectangle(0, 0, BMwidth, BMheight), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                        }
                        switch (IVLCamVariables.captureImageType)
                        {
                            case CapturedImageType.GbGr:// gb gr frame captured
                                {
                                    GCHandle pinnedArray1 = GCHandle.Alloc(RawImageList[IVLCamVariables.GBIndx], GCHandleType.Pinned);
                                    IntPtr pointer1 = pinnedArray1.AddrOfPinnedObject();

                                    GCHandle pinnedArray2 = GCHandle.Alloc(RawImageList[IVLCamVariables.GRIndx], GCHandleType.Pinned);
                                    IntPtr pointer2 = pinnedArray2.AddrOfPinnedObject();
                                    PostProcessing.ImageProc_Demosaic(pointer1, pointer2, bdata.Scan0, BMwidth, BMheight, false);


                                    pinnedArray1.Free();
                                    pinnedArray2.Free();
                                    RawImage.UnlockBits(bdata);
                                    break;
                                }
                            case CapturedImageType.Flash:// flash frame captured
                                {
                                    GCHandle pinnedArray1 = new GCHandle();
                                    if (IVLCamVariables._Settings.CameraSettings.CameraModel == CameraModel.B || IVLCamVariables._Settings.CameraSettings.CameraModel == CameraModel.A)
                                        pinnedArray1 = GCHandle.Alloc(RawImageList[IVLCamVariables.FlashIndx], GCHandleType.Pinned);
                                    else if (IVLCamVariables._Settings.CameraSettings.CameraModel == CameraModel.C)
                                    {
                                        //for (int i = IVLCamVariables.imageCatList.Count-1; i >= 0; i--) // to check capture index for flash frame
                                        //{
                                        //    if (IVLCamVariables.imageCatList[i] == ImageCategory.Flash)
                                        //    {
                                        //        IVLCamVariables.CaptureImageIndx = i; // if the image category list is flash make the index as capture index
                                        //        break;
                                        //    }
                                        //}
                                        //}
                                        //if (IVLCamVariables.imageCatList[IVLCamVariables.CaptureImageIndx] != ImageCategory.Flash)
                                        //{
                                        //    IVLCamVariables.CaptureImageIndx += 1;
                                        //}
                                        pinnedArray1 = GCHandle.Alloc(RawImageList[IVLCamVariables.CaptureImageIndx], GCHandleType.Pinned);
                                    }
                                    IntPtr pointer2 = pinnedArray1.AddrOfPinnedObject();
                                    PostProcessing.ImageProc_Demosaic(IntPtr.Zero, pointer2, bdata.Scan0, BMwidth, BMheight, true);

                                    pinnedArray1.Free();
                                    RawImage.UnlockBits(bdata);
                                    

                                    break;
                                }
                        }
                        #endregion
                        RotateRawImage();
                        if (!IVLCamVariables.isCaptureFailure)
                        {
                            //IVLCamVariables.ProcessedImage = RawImage.Clone() as Bitmap;
                            Graphics g = Graphics.FromImage(CaptureBm);
                            g.DrawImage(RawImage, new Rectangle(0, 0, BMwidth, BMheight));
                            //CaptureBm = RawImage.Clone() as Bitmap;


                            if (IVLCamVariables._Settings.PostProcessingSettings.isApplyPostProcessing)
                            {
                                IVLCamVariables.PostProcessing.ApplyPostProcessing(ref CaptureBm, IVLCamVariables.isColor);
                            }
                            if (IVLCamVariables.ImagingMode == ImagingMode.FFA_Plus)
                            {
                                IVLCamVariables.PostProcessing.GetMonoChromeImage(ref CaptureBm);
                                IVLCamVariables.PostProcessing.ApplyTimeStamp(ref CaptureBm, IVLCamVariables.FFATime, Color.LimeGreen);
                                arg["FFA Image"] = IVLCamVariables.ffaTimeStamp = IVLCamVariables.FFATime;
                            }

                            //arg["rawImage"] = IVLCamVariables.CaptureBm.Clone();//Mainly used to avoid resource(IVLCamVariables.CaptureBm) being locked and exception occuring becuse of the same.
//                            pbx.Image = IVLCamVariables.CaptureBm.Clone() as Bitmap ;
                            pbx.Image = CaptureBm.Clone() as Bitmap ;
                            pbx.Refresh();
                            if (IVLCamVariables.ImagingMode == ImagingMode.FFA_Plus)
                            {
                                //IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.DisplayCapturedImage, arg);
                                maskOverlayPbx.Visible = false;
                            }
                            //image_pbx.Image = IVLCamVariables.CaptureBm;
                            //image_pbx.Refresh();

                        }
                           arg["isCaptureFailed"] = IVLCamVariables.isCaptureFailure;
                        
                            IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.SaveFrames2Disk, arg);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);   

            }
            
        }

        private void FrameDetectionForManufacturersDemosaicing()
        {
            try
            {
                IVLCamVariables.IsFlashOffDone = false;
                Args arg = new Args();
               // GetCaptureFrameIndex();
                #region toupcam colorCorrection
                    //IVLCamVariables.FrameBetweenFlashOnFlashOffDoneCnt = 0;
                    Args logArg = new Args();
                    
                    logArg["TimeStamp"] = DateTime.Now;
                    logArg["Msg"] = "Capture Indx current frame = " + IVLCamVariables.CaptureImageIndx;
                    //logArg["callstack"] = Environment.StackTrace;
                    string str = "";
                    str = string.Format("Time = {0},Capture Indx current frame = {1}  ", DateTime.Now.ToString("HH-mm-ss-ffff"), IVLCamVariables.CaptureImageIndx );
                    capture_log.Add(logArg);
                 //IVLCamVariables.logClass.CaptureLogList.Add(str);
                     //IVLCamVariables.CameraLogList.Add(logArg);
                    //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);
                  CaptureBm =  CaptureImageList[IVLCamVariables.CaptureImageIndx].Clone() as Bitmap;
                  ///IVLCamVariables.CaptureBm.Save("capturedFrame.png");
                  if (IVLCamVariables._Settings.PostProcessingSettings.isApplyPostProcessing)
                  {
                      bool isColor = true;
                      if (IVLCamVariables.ImagingMode == ImagingMode.FFA_Plus)
                          isColor = false;
                      IVLCamVariables.PostProcessing.ApplyPostProcessing(ref CaptureBm , isColor);
                  }
                   // IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.DisplayCapturedImage, arg);
                    pbx.Image = CaptureBm.Clone() as Bitmap;
                    pbx.Refresh();
                CaptureFrameDateTime.Clear();
                arg["isCaptureFailed"] = IVLCamVariables.isCaptureFailure;

                
                    IVLCamVariables._eventHandler.Notify(IVLCamVariables._eventHandler.SaveFrames2Disk, arg);
                #endregion
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);   
            }

        }

        #region****************/////// Camera Settings APIs ///////****************

        bool ret;

        private void RotateRawImage()
        {
            try
            {
                if (isVflip && isHflip)
                    RawImage.RotateFlip(RotateFlipType.Rotate180FlipXY);
                else if (isVflip)
                    RawImage.RotateFlip(RotateFlipType.Rotate180FlipY);
                else if (isHflip)
                    RawImage.RotateFlip(RotateFlipType.Rotate180FlipX);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);    
            }
            
        }
        public void FlipHorizontal(bool isRotate)
        {
            try
            {
                ret = _vendor1.put_HFlip(isRotate);
                ret = false;

                _vendor1.get_HFlip(out ret);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);    
            }
            
        }
        public void FlipVertical(bool isRotate)
        {
            try
            {
                ret = _vendor1.put_VFlip(isRotate);
                ret = false;
                _vendor1.get_VFlip(out ret);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);                   
            }
           
        }
        public override bool GetResolution(ref Dictionary<int, int> WidthHeight)
        {
            try
            {
                int resNum = (int)_vendor1.ResolutionNumber;
                for (int i = 0; i < resNum; i++)
                {
                    _vendor1.get_Resolution((uint)i, out width, out height);
                    WidthHeight.Add(width, height);
                }
                
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
                return false;
            }
            return true;
            
        }

        public override bool setResolution(int mode)//
        {
            bool retVal = false;
            try
            {
                retVal = _vendor1.put_eSize((uint)mode);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
                retVal = false;
            }
            return retVal;
        }


        public override bool EnableAutoExposure(bool enable)
        {
            bool ret = false;
            try
            {
                ret=_vendor1.put_AutoExpoEnable(enable);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
                ret = false;
            }
            return ret;
        }
       public uint CurrentExposure;
        public override bool SetExposure(uint val)
        {
            bool ret = false;
            try
            {

                if (IVLCamVariables.isCameraConnected == Devices.CameraConnected)
                {
                    string str = "";
                    Args logArg = new Args();

                    logArg["TimeStamp"] = DateTime.Now;
                    logArg["Msg"] = "Exposure =  " + val;
                    //logArg["callstack"] = Environment.StackTrace;
                    str = string.Format("Time = {0},Exposure = {1}  ", DateTime.Now.ToString("HH-mm-ss-ffff"), val);
                    //IVLCamVariables.CameraLogList.Add(logArg);
                    //IVLCamVariables.logClass. IVLCamVariables.CameraLogList.Add(str);
                    //IVLCamVariables.logClass.CaptureLogList.Add(str);
                    capture_log.Add(logArg);
                    if (CurrentExposure != val)
                    {
                        ret = _vendor1.put_ExpoTime(val);
                        CurrentExposure = val;
                    }
                }
            }
            catch (Exception ex)
            {
                CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
                ret = false;
            }

            return ret;
        }
       public ushort CurrentGain = 0;
        public override bool SetGain(ushort val)
        {
            bool ret = false;
            try
            {
                if (IVLCamVariables.isCameraConnected == Devices.CameraConnected)
                {
                    if (CurrentGain != val)
                    {
                        ret = _vendor1.put_ExpoAGain(val);
                        CurrentGain = val;
                    }
                }
            }
            catch (Exception ex)
            {
                CameraLogger.WriteException(ex, Exception_Log);
                ///                 CameraLogger.WriteException(ex, Exception_Log);
                ret = false;
            }
            return ret;
        }

        public bool GetExposure(ref uint expTime)
        {
            bool ret = false;
            try
            {
                if (IVLCamVariables.isCameraConnected == Devices.CameraConnected)

                ret= _vendor1.get_ExpoTime(out expTime);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
                ret = false;
            }
            return ret;
        }
        public bool GetGain(ref ushort expTime)
        {
            bool ret = false;
             try
            {
                if (IVLCamVariables.isCameraConnected == Devices.CameraConnected)

                   ret = _vendor1.get_ExpoAGain(out expTime);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
                ret = false;
            }
             return ret;
        }
        public override bool SetGamma(int gammaVal)
        {
            bool ret = false;
            try
            {
                ret= _vendor1.put_Gamma(gammaVal);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
                ret = false;
            }
            return ret;
        }
        public override bool GetGamma(ref int gammaVal)
        {
            bool ret = false;
            try
            {
                ret= _vendor1.get_Gamma(out gammaVal);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
                ret = false;
            }
            return ret;
        }

        public bool SetBrightness(int brightnessVal)
        {
            bool ret = false;
            try
            {
                  ret= _vendor1.put_Brightness(brightnessVal);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
                ret = false;   
            }
            return ret;
        }
        public bool GetBrightness(ref int brightnessVal)
        {
            bool ret = false;
            try
            {
                ret= _vendor1.get_Gamma(out brightnessVal);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
               ret = false;   
            }
            return ret;
        }

        public bool SetContrast(int contrastVal)
        {
            bool ret = false;
            try
            {
                ret= _vendor1.put_Contrast(contrastVal);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
                ret = false;
            }
            return ret;
        }
        public bool GetContrast(ref int contrastVal)
        {
            bool ret = false;
            try
            {
                 ret= _vendor1.get_Contrast(out contrastVal);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
                ret = false;
            }
            return ret;
        }

        public bool SetHue(int hueVal)
        {
            bool ret = false;
            try
            {
                ret=_vendor1.put_Hue(hueVal);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
              ret = false;  
            }
            return ret;
        }
        public bool GetHue(ref int hueVal)
        {
            bool ret = false;
            try
            {
              ret=  _vendor1.get_Hue(out hueVal);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
                ret = false;  
            }
            return ret;
        }

        public bool SetSaturation(int saturationVal)
        {
            bool ret = false;
            try
            {
                ret=_vendor1.put_Saturation(saturationVal);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
               ret = false;
            }
            return ret;
        }
        public bool GetSaturation(ref int saturationVal)
        {
            bool ret = false;
            try
            {
                ret= _vendor1.get_Saturation(out saturationVal);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
                ret = false;
            }
            return ret;
        }


        public override bool GetGainRange(ref ushort minVal, ref ushort maxVal, ref ushort defVal)
        {
            bool ret = false;
            try
            {
               ret= _vendor1.get_ExpoAGainRange(out minVal, out maxVal, out defVal);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
                ret = false;
            }
            return ret;
        }
        public override bool GetExposureRange(ref uint minVal, ref uint maxVal, ref uint defVal)
        {
            bool ret = false;
            try
            {
                ret= _vendor1.get_ExpTimeRange(out minVal, out maxVal, out defVal);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
                ret = false;
            }
            return ret;
        }

        public bool SetTemperatureTint(int valTemp, int valTint)
        {
            bool ret = false;
            try
            {
                ret= _vendor1.put_TempTint(valTemp, valTint);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
               ret = false;
            }
            return ret;
        }
        public bool GetTemperatureTint(ref int valTemp, ref int valTint)
        {

            bool ret = false;
            try
            {
                ret=  _vendor1.get_TempTint(out valTemp, out valTint);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
                ret = false; 
            }
            return ret;
        }

        public bool SetMonoMode(bool isMonoMode)
        {
            bool ret= false;
            try
            {
                 ret = _vendor1.put_Chrome(isMonoMode);
                 return ret;
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
                return false;
            }
            
        }
        public bool GetColorMode(ref bool isColorMode)
        {
            bool ret = false;
            try
            {
                ret =_vendor1.get_Chrome(out isColorMode);
                return ret;
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
                ret = false;
            }
            return ret;
        }
        public override bool SetFrameRateLevel(int level)
        {
            bool ret = false;
            try
            {
                ret = _vendor1.put_Speed((byte)level);
                return ret;
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
                ret = false;
            }
            return ret;

        }

        public override bool GetFrameRate(ref ushort val)
        {
            bool retVal = false;
            try
            {
                 retVal = _vendor1.get_Speed(out val);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
               retVal = false;
            }
            
            return retVal;
        }
        #endregion
    }
    public class AssistedFocus
    {
        public double avgL;
        public double avgL_periphery;
        public double avgR_periphery;
        public double avgR;
        public double covL;
        public double covL_periphery;
        public double covR_periphery;
        public double covR;
        public double sumL;
        public double sumL_periphery;
        public double sumR_periphery;
        public double sumR;
        public double sumFocus;
        public double sumFocus__periphery;



    }
    public class ComputeAutoFocus
    {
        int HotSpotX = 1024;
        int HotSpotY = 768;
        Image<Gray, float> sobelResultX, sobelResultY, sobelResultPeripheryX, sobelResultPeripheryY, rowImage, colImage, rowPeripheryImage, colPeripheryImage;
        AssistedFocus af;
        Rectangle FocusRoiLeft = new Rectangle(484, 268, 500, 800);
        Rectangle FocusRoiRight = new Rectangle(1274, 268, 500, 800);
        Rectangle FocusRoiRightPeriphery = new Rectangle(0, 0, 100, 1500);
        Rectangle FocusRoiLefttPeriphery = new Rectangle(1948, 0, 100, 1500);
        private static readonly Logger Exception_Log = LogManager.GetLogger("INTUSOFT.Desktop");

        int sizeOfRegion = 10;

        Image<Bgr, byte> FocusRoiImage;
        public ComputeAutoFocus()
        {
            try
            {
                FocusRoiLeft.X = HotSpotX - 150 - FocusRoiLeft.Width;
                FocusRoiLeft.Y = HotSpotY - FocusRoiLeft.Height / 2;
                FocusRoiRight.X = HotSpotX + 150;
                FocusRoiRight.Y = HotSpotY - FocusRoiLeft.Height / 2;
                rowImage = new Image<Gray, float>(FocusRoiLeft.Width, FocusRoiLeft.Height / sizeOfRegion);
                colImage = new Image<Gray, float>(FocusRoiLeft.Width / sizeOfRegion, FocusRoiLeft.Height);
                rowPeripheryImage = new Image<Gray, float>(100, 1500 / sizeOfRegion);
                colPeripheryImage = new Image<Gray, float>(100 / sizeOfRegion, 1500);
            }
            catch (Exception ex)
            {
                 CameraLogger.WriteException(ex, Exception_Log);  
            }
            
        }
        public AssistedFocus ComputeFocus(Bitmap src)
        {
            try
            {
                {
                    Image<Bgr, byte> inp = new Image<Bgr, byte>(src);
                    Gray avgR = new Gray();
                    Gray avgR_periphery = new Gray();
                    MCvScalar sdvR = new MCvScalar();
                    Gray avgL = new Gray();
                    Gray avgL_periphery = new Gray();
                    MCvScalar sdvL = new MCvScalar();
                    double covL = 0, covR = 0, sumL = 0, sumR = 0, covL_periphery = 0, covR_periphery = 0, sumL_periphery = 0, sumR_periphery = 0;
                    for (int k = 0; k < 2; k++)
                    {
                        if (k == 0)
                        {
                            inp.ROI = FocusRoiLeft;
                            FocusRoiImage = inp.Copy();
                            inp.ROI = new Rectangle();
                            FocusRoiImage[2] = FocusRoiImage[2].SmoothBilatral(5, 10, 10);
                            FocusRoiImage[2].AvgSdv(out avgL, out sdvL);
                            covL = sdvL.V0 / avgL.Intensity;
                            sobelResultX = FocusRoiImage[2].Sobel(1, 0, 3);
                            sobelResultY = FocusRoiImage[2].Sobel(0, 1, 3);
                            FocusRoiImage.Dispose();

                            inp.ROI = FocusRoiLefttPeriphery;
                            FocusRoiImage = inp.Copy();

                            FocusRoiImage[2] = FocusRoiImage[2].SmoothBilatral(5, 10, 10);
                            //focusroiimage[2].avgsdv(out avgl_periphery, out sdvl_periphery);
                            avgL_periphery = FocusRoiImage[2].GetSum();
                            //covL_periphery = sdvL_periphery.v0 / avgL_periphery.Intensity;
                            //sobelResultPeripheryX = FocusRoiImage[2].Sobel(1, 0, 3);
                            //sobelResultPeripheryY = FocusRoiImage[2].Sobel(0, 1, 3);
                            inp.ROI = new Rectangle();

                        }
                        else
                        {
                            inp.ROI = FocusRoiRight;
                            FocusRoiImage = inp.Copy();
                            inp.ROI = new Rectangle();
                            FocusRoiImage[2] = FocusRoiImage[2].SmoothBilatral(5, 10, 10);
                            FocusRoiImage[2].AvgSdv(out avgR, out sdvR);
                            covR = sdvR.V0 / avgR.Intensity;
                            sobelResultX = FocusRoiImage[2].Sobel(1, 0, 3);
                            sobelResultY = FocusRoiImage[2].Sobel(0, 1, 3);
                            FocusRoiImage.Dispose();
                            inp.ROI = FocusRoiRightPeriphery;
                            FocusRoiImage = inp.Copy();

                            FocusRoiImage[2] = FocusRoiImage[2].SmoothBilatral(5, 10, 10);
                            avgR_periphery = FocusRoiImage[2].GetSum();
                            //FocusRoiImage[2].AvgSdv(out avgR_periphery, out sdvR_periphery);
                            //covR_periphery = sdvR_periphery.v0 / avgR_periphery.Intensity;
                            //sobelResultPeripheryX = FocusRoiImage[2].Sobel(1, 0, 3);
                            //sobelResultPeripheryY = FocusRoiImage[2].Sobel(0, 1, 3);
                            inp.ROI = new Rectangle();

                        }

                        //Image<Gray, float> rowImage = new Image<Gray, float>(500, 1000 / sizeOfRegion);
                        for (int i = 0; i < sobelResultX.Height - sizeOfRegion; i = i + sizeOfRegion)
                        {
                            rowImage.ROI = new Rectangle(0, i / sizeOfRegion, inp.Width, 1);
                            calculateCumSum(ref sobelResultX, ref rowImage, sizeOfRegion, i, true);
                            rowImage.ROI = new Rectangle();
                        }
                        //for (int i = 0; i < sobelResultPeripheryX.Height - sizeOfRegion; i = i + sizeOfRegion)
                        //{
                        //    rowPeripheryImage.ROI = new Rectangle(0, i / sizeOfRegion, inp.Width, 1);
                        //    calculateCumSum(ref sobelResultPeripheryX, ref rowPeripheryImage, sizeOfRegion, i, true);
                        //    rowPeripheryImage.ROI = new Rectangle();
                        //}
                        rowImage = rowImage.Pow(2);
                        rowImage = rowImage.Pow(0.5);
                        //rowPeripheryImage = rowPeripheryImage.Pow(2);
                        //rowPeripheryImage = rowPeripheryImage.Pow(0.5);
                        double valX = rowImage.GetSum().Intensity;
                        //double valXPeriphery = rowPeripheryImage.GetSum().Intensity;
                        for (int i = 0; i < sobelResultX.Width - sizeOfRegion; i = i + sizeOfRegion)
                        {
                            colImage.ROI = new Rectangle(i / sizeOfRegion, 0, 1, inp.Height);
                            calculateCumSum(ref sobelResultY, ref colImage, sizeOfRegion, i, false);
                            colImage.ROI = new Rectangle();
                        }
                        //for (int i = 0; i < sobelResultPeripheryY.Width - sizeOfRegion; i = i + sizeOfRegion)
                        //{
                        //    colPeripheryImage.ROI = new Rectangle(i / sizeOfRegion, 0, 1, inp.Height);
                        //    calculateCumSum(ref sobelResultPeripheryY, ref colPeripheryImage, sizeOfRegion, i, false);
                        //    colPeripheryImage.ROI = new Rectangle();
                        //}
                        colImage = colImage.Pow(2);
                        colImage = colImage.Pow(0.5);
                        //colPeripheryImage = colPeripheryImage.Pow(2);
                        //colPeripheryImage = colPeripheryImage.Pow(0.5);
                        double valY = colImage.GetSum().Intensity;
                        //double valYPeriphery = colPeripheryImage.GetSum().Intensity;
                        if (k == 0)
                        {
                            sumL = (valX + valY);
                            //sumL_periphery = (valXPeriphery + valYPeriphery);
                        }
                        else
                        {
                            sumR = valX + valY;
                            //sumR_periphery = valXPeriphery + valYPeriphery;
                            af = new AssistedFocus();
                            af.avgL = avgL.Intensity;
                            af.avgL_periphery = avgL_periphery.Intensity;
                            af.avgR = avgR.Intensity;
                            af.avgR_periphery = avgR_periphery.Intensity;

                            af.covL = covL;
                            af.covL_periphery = covL_periphery;
                            af.covR = covR;
                            af.covR_periphery = covR_periphery;
                            af.sumL = sumL;
                            af.sumL_periphery = sumL_periphery;
                            af.sumR = sumR;
                            af.sumR_periphery = sumR_periphery;
                            if (af.avgL < 0.1)
                                af.avgL = 0.1;
                            if (af.avgR < 0.1)
                                af.avgR = 0.1;

                            //if (af.avgL_periphery < 0.1)
                            //    af.avgL_periphery = 0.1;
                            //if (af.avgR_periphery < 0.1)
                            //    af.avgR_periphery = 0.1;
                            af.sumFocus = af.sumL / af.avgL + af.sumR / af.avgR;
                            //af.sumFocus__periphery = af.sumL_periphery / af.avgL_periphery + af.sumR_periphery / af.avgR_periphery ;
                        }
                    }
                    inp.Dispose();
                }
                sobelResultX.Dispose();
                sobelResultY.Dispose();
            }
            catch (Exception ex)
            {
                 CameraLogger.WriteException(ex, Exception_Log);
            }
            return af;
        }
        private void calculateCumSum(ref Image<Gray, float> src, ref Image<Gray, float> dst, int sizeOfRegion, int indx, bool isRow)
        {
            try
            {
                  
            if (isRow)
            {
                src.ROI = new Rectangle(0, indx, src.Width, sizeOfRegion);

                CvInvoke.Reduce(src, dst, ReduceDimension.SingleRow, ReduceType.ReduceSum);

            }
            else
            {
                src.ROI = new Rectangle(indx, 0, sizeOfRegion, src.Height);
                CvInvoke.Reduce(src, dst, ReduceDimension.SingleCol, ReduceType.ReduceSum);
            }
            src.ROI = new Rectangle();
            }
            catch (Exception ex)
            {
                 CameraLogger.WriteException(ex, Exception_Log);
            }
        }
    }
    public class ImageCategoryHelper
    {
        public int GBGRSum = 0;
        public float GBByRatio = 0f;
        public ImageCategoryHelper()
        {

        }
    }
}
        #endregion
