using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.UI;
using System.Runtime.InteropServices;
using INTUSOFT.Imaging;
using INTUSOFT.Custom.Controls;
using INTUSOFT.EventHandler;
using System.Globalization;
using NLog;
using NLog.Config;
using NLog.Targets;
using INTUSOFT.Configuration;
namespace AssemblySoftware
{

    public partial class CameraUI : Form
    {
        private List<HarrProgressBar> _items = new List<HarrProgressBar>();
        public Logger Exception_Log = LogManager.GetLogger("AssemblySoftware.ExceptionLog");// for exception logging

        //public const string LiveModeProcessingDLLName = @"LiveModeProcessing.dll";
        //[DllImport(LiveModeProcessingDLLName, EntryPoint = "AssistedFocus_GetFrameState")]
        //public static extern void AssistedFocus_GetFrameState(IntPtr src, ref int state, int currentGain,ref float edgeStrength,int indx);
        //[DllImport(LiveModeProcessingDLLName, EntryPoint = "AssistedFocus_Init")]
        //public static extern void AssistedFocus_Init(int AvgBrightnessRef, double covLowerLimit, double covUpperLimit, int numberOfScales, int EntryPeakPercentage, int ExitPeakPercentage, int MaxGain);

        //[DllImport(LiveModeProcessingDLLName, EntryPoint = "AssistedFocus_FrameProperties")]
        // public static extern void AssistedFocus_FrameProperties(IntPtr src, ref double entropyR, ref double entropyB, ref double entropyG, ref double contrastR,
        //        ref double brightnessR, ref double edgestrengthR, ref double contrastG, ref double brightnessG, ref double edgestrengthG, ref double contrastB, ref double brightnessB, ref double edgestrengthB);
        int iterCommandCnt = 3;
        IVLEventHandler eventHandler;
       INTUSOFT.Imaging.ImagingMode  imagingMode;
        IntucamHelper ivl_camera;
        PostProcessing postProcessing;
        Timer t = new Timer();
        DirectoryInfo dInf, saveDinf;
        string SaveDirectoryPath = @"C:\IVL_ImageRepo\Images";
        static Bitmap srcImg, cameraConnectImg, cameraDisconnectImg;
        Bitmap TriggerRecieved, TriggerReset;
        Bitmap displayImg;
        bool isCameraOpen = false;
        int FrameRate = 0;
        int tempVal = 6500;
        int tintVal = 1000;
        ushort minGVal = 0, maxGVal = 0, defGVal = 0;
        int temperatureMax = 15000, temperatureMin = 2000;
        int tintMax = 2500, tintMin = 200;
        int prevHue = 0;
        int prevSat = 128;
        int prevBrightness = 0;
        int prevContrast = 0;
        uint minEVal = 0, maxEVal = 0, defEVal = 0;
        string titleText = "";
        Bitmap focusBitmap, unfocusBitmap, BrightBitmap, PeakBitmap;
        public System.IO.StreamWriter AutoFocusWriter;
        System.Timers.Timer ResetTimer;
        bool isCapturing = false;
        int RBwidth;
        int RBheight;
        int LBwidth;
        int LBheight;
        bool isChannelWiseLut = false;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Space)
            {
                // space bar pressed    
                if (ivl_camera.isCameraOpen)
                {
                    istrigger = false;
                    saveFrames();
                }
            }
            // right arrow key  pressed
            else if (keyData == Keys.Right)
                next_btn_Click(null, null);
            else if (keyData == Keys.Left)
                previous_btn_Click(null, null);
            else if (keyData == Keys.Add)
                MotorBackward_btn_Click(null, null);
            else if (keyData == Keys.Subtract)
                MotorForward_btn_Click(null, null);
            else if (keyData == (Keys.Alt | Keys.P))
                Show_EEPROM_Window();
            else if(keyData == (Keys.Alt | Keys.S))
            {
                ConfigurationWindow c = new ConfigurationWindow();
                c.ShowDialog();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Show_EEPROM_Window()
        {

        }
        Bitmap LeftArrowBitmap, RightArrowBitmap;
        Bitmap LeftBitmap, RightBitmap;
        System.ComponentModel.BackgroundWorker bg;
        public CameraUI()
        {
            InitializeComponent();
            this.BringToFront();
            Control.CheckForIllegalCrossThreadCalls = false;
            this.flowLayoutPanel1.DragEnter += new DragEventHandler(flowLayoutPanel1_DragEnter);
            this.flowLayoutPanel1.DragDrop += new DragEventHandler(flowLayoutPanel1_DragDrop);
            //bg = new BackgroundWorker();
            //bg.WorkerSupportsCancellation = true;
            //bg.DoWork += bg_DoWork;
            Size s = new Size(flowLayoutPanel1.Width, 25);
            HarrProgressBar pgb;

            pgb = new HarrProgressBar();
            pgb.Padding = new Padding(5);
            pgb.LeftText = "0";
            pgb.StatusText = "Shift";
            pgb.FillDegree = 20;
            pgb.StatusBarColor = 0;
            pgb.Size = s;
            pgb.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this._items.Add(pgb);
            this.flowLayoutPanel1.Controls.Add(pgb);

            pgb = new HarrProgressBar();
            pgb.Padding = new Padding(5);
            pgb.LeftText = "4";
            pgb.StatusText = "HS";
            pgb.FillDegree = 40;
            pgb.StatusBarColor = 1;
            pgb.Size = s;
            this._items.Add(pgb);
            this.flowLayoutPanel1.Controls.Add(pgb);

            pgb = new HarrProgressBar();
            pgb.Padding = new Padding(5);
            pgb.LeftText = "6";
            pgb.StatusText = "CC";
            pgb.FillDegree = 85;
            pgb.StatusBarColor = 2;
            pgb.Size = s;
            this._items.Add(pgb);
            this.flowLayoutPanel1.Controls.Add(pgb);

            pgb = new HarrProgressBar();
            pgb.Padding = new Padding(5);
            pgb.LeftText = "8";
            pgb.StatusText = "Mask";
            pgb.FillDegree = 95;
            pgb.StatusBarColor = 3;
            pgb.Size = s;
            this._items.Add(pgb);
            this.flowLayoutPanel1.Controls.Add(pgb);

            pgb = new HarrProgressBar();
            pgb.Padding = new Padding(5);
            pgb.LeftText = "7";
            pgb.StatusText = "Brightness";
            pgb.FillDegree = 100;
            pgb.Size = s;
            this._items.Add(pgb);
            this.flowLayoutPanel1.Controls.Add(pgb);

            pgb = new HarrProgressBar();
            pgb.Padding = new Padding(5);
            pgb.LeftText = "5";
            pgb.StatusText = "Unsharp";
            pgb.FillDegree = 100;
            pgb.Size = s;
            this._items.Add(pgb);
            this.flowLayoutPanel1.Controls.Add(pgb);

            pgb = new HarrProgressBar();
            pgb.Padding = new Padding(5);
            pgb.LeftText = "2";
            pgb.StatusText = "LUT";
            pgb.FillDegree = 100;
            pgb.Size = s;
            this._items.Add(pgb);
            this.flowLayoutPanel1.Controls.Add(pgb);

            pgb = new HarrProgressBar();
            pgb.Padding = new Padding(5);
            pgb.LeftText = "3";
            pgb.StatusText = "Clahe";
            pgb.FillDegree = 100;
            pgb.Size = s;
            this._items.Add(pgb);
            this.flowLayoutPanel1.Controls.Add(pgb);

            pgb = new HarrProgressBar();
            pgb.Padding = new Padding(5);
            pgb.LeftText = "9";
            pgb.StatusText = "Gamma";
            pgb.FillDegree = 100;
            pgb.Size = s;
            this._items.Add(pgb);
            this.flowLayoutPanel1.Controls.Add(pgb);

            pgb = new HarrProgressBar();
            pgb.Padding = new Padding(5);
            pgb.LeftText = "1";
            pgb.StatusText = "HSVBoost";
            pgb.FillDegree = 100;
            pgb.Size = s;
            this._items.Add(pgb);
            this.flowLayoutPanel1.Controls.Add(pgb);

            ivl_camera = IntucamHelper.GetInstance();
            ivl_camera.IsAssemblySoftware = true;
            ivl_camera.Pbx = display_pbx;
            ivl_camera.LeftArrowPbx = rightArrow_pbx;
            ivl_camera.RightArrowPbx = leftArrow_pbx;
            ivl_camera.LeftDiaptorPbx = neg_pbx;
            ivl_camera.RightDiaptorPbx = pos_pbx;
            ivl_camera.camPropsHelper._Settings.ImageSaveSettings.RawImageDirPath = SaveDirectoryPath;
            ivl_camera.camPropsHelper.LeftBitmap = new Bitmap(neg_pbx.Width, neg_pbx.Height);// Added this to manage the blinking happening in the UI to show the sensor position when the rotary is moved instead of using the panel drawing
            ivl_camera.camPropsHelper.RightBitmap = new Bitmap(pos_pbx.Width, pos_pbx.Height);// Added this to manage the blinking happening in the UI to show the sensor position when the rotary is moved instead of using the panel drawing
            //cameraModel_cmbx.SelectedIndex = 0;
            LeftArrowBitmap = Image.FromFile(@"ImageResources\Arrow.jpg") as Bitmap;
            RightArrowBitmap = Image.FromFile(@"ImageResources\Arrow.jpg") as Bitmap;
            LeftArrowBitmap.RotateFlip(RotateFlipType.Rotate180FlipY);
            ivl_camera.camPropsHelper.resetBitmapLeft = new Bitmap(leftArrow_pbx.Width, leftArrow_pbx.Height);
            ivl_camera.camPropsHelper.resetBitmapRight = new Bitmap(rightArrow_pbx.Width, rightArrow_pbx.Height);
            ivl_camera.camPropsHelper.negativearrowSymbol = new Bitmap(rightArrow_pbx.ClientSize.Width, rightArrow_pbx.ClientSize.Height);
            ivl_camera.camPropsHelper.positivearrowSymbol = new Bitmap(rightArrow_pbx.ClientSize.Width, rightArrow_pbx.ClientSize.Height);
            ivl_camera.camPropsHelper.RotaryPositiveColor = ivl_camera.camPropsHelper.RotaryNegativeColor = "Blue";
            ivl_camera.camPropsHelper.CreatePositiveNegativeDiaptorSymbols();
            createImage();

            //gPanel11 = Graphics.FromHwnd(panel11.Handle);
            //gPanel12 = Graphics.FromHwnd(panel12.Handle);

            eventHandler = IVLEventHandler.getInstance();
            postProcessing = PostProcessing.GetInstance();
            //AssistedFocus_Init(0, 0, 0, 3, 0, 0, 0);
            display_pbx.MouseMove += display_pbx_MouseMove;
            overlay_pbx.MouseMove += overlay_pbx_MouseMove;
            TriggerRecieved = new Bitmap(100, 100);
            ResetTimer = new System.Timers.Timer();
            ResetTimer.Elapsed += ResetTimer_Elapsed;
            //LeftBitmap = new Bitmap(pos_pbx.Width, pos_pbx.Height);
            //RightBitmap = new Bitmap(pos_pbx.Width, pos_pbx.Height);

            //RBwidth = RightBitmap.Width;
            //RBheight = RightBitmap.Height;
            //LBwidth = LeftBitmap.Width;
            //LBheight = LeftBitmap.Height;


            TriggerReset = new Bitmap(100, 100);
            Graphics g = Graphics.FromImage(TriggerRecieved);
            g.FillEllipse(Brushes.Gold, new Rectangle(0, 0, 100, 100));
            g.Dispose();

            g = Graphics.FromImage(TriggerReset);
            g.FillEllipse(Brushes.Green, new Rectangle(0, 0, 100, 100));
            resolution_combx.SelectedIndexChanged += resolution_combx_SelectedIndexChanged;
            eventHandler.Register(eventHandler.FrameCaptured, new NotificationHandler(FrameCaptureDone));
            eventHandler.Register(eventHandler.UPDATE_POWER_STATUS, new NotificationHandler(ShowPowerConnection));
            eventHandler.Register(eventHandler.UPDATE_CAMERA_STATUS, new NotificationHandler(ShowCameraConnection));
            //eventHandler.Register(eventHandler.UpdateCommandLogView, new NotificationHandler(updateCommandLogView));
            eventHandler.Register(eventHandler.FrameRateStatusUpdate, new NotificationHandler(ivl_camera_statusBarUpdate));
            eventHandler.Register(eventHandler.triggerRecieved, new NotificationHandler(TriggerRecievedNotification));
            eventHandler.Register(eventHandler.DisplayImage, new NotificationHandler(DisplayImage));
            eventHandler.Register(eventHandler.UpdateFFATime, new NotificationHandler(updateFFATime));
            eventHandler.Register(eventHandler.ShowSplashScreen, new NotificationHandler(ShowSplashScreen));
            //eventHandler.Register(eventHandler.ChangeLeftRightPos_Live, new NotificationHandler(LeftRightEvent));
            ExpGain_gbx.MouseEnter += ExpGain_gbx_MouseEnter;
            ExpGain_gbx.MouseLeave += ExpGain_gbx_MouseLeave;
            if (!Directory.Exists(SaveDirectoryPath))
                dInf = Directory.CreateDirectory(SaveDirectoryPath);
            else
                dInf = new DirectoryInfo(SaveDirectoryPath);
            titleText = this.Text;
            //t.Tick += t_Tick;
            // t.Interval = 1000;

            byte[] valArr = new byte[3];
            valArr[0] = 0x02;
            valArr[1] = 0x49;
            valArr[2] = 0xF0;
            //var byeVal = valArr[0] + (valArr[1] << 8) + (valArr[2] << 16);
            //MessageBox.Show(byeVal.ToString());
            //Console.WriteLine(byeVal);
            if (File.Exists(@"ImageResources\color.png"))
                colorChannel_btn.Image = Image.FromFile(@"ImageResources\color.png"); //colorImage;
            double sineFactor = 4096;
            double interval1 = 4096; // 1st phase of curve is from 0 to interval 1 -- Sine wave (0 to PI/2)
            double interval2 = 8192; // 2nd phase of curve is from interval1 to interval1+interval 2  -- Cosine Wave (0 to PI)
            double CosineFactor = sineFactor / 2;

            PostProcessing.ImageProc_CalculateLut(sineFactor, interval1, interval2, 14, true, 0, false,0);
            sineFactor = 100;
            interval1 = 64; // 1st phase of curve is from 0 to interval 1 -- Sine wave (0 to PI/2)
            interval2 = 90; // 2nd phase of curve is from interval1 to interval1+interval 2  -- Cosine Wave (0 to PI)
            CosineFactor = sineFactor / 2;

            PostProcessing.ImageProc_CalculateLut(sineFactor, interval1, interval2, 8, false, 0, false, 0);
        }

        SplashScreen splashScreen;
        private void ShowSplashScreen(string s, Args arg)
        {

            if ((bool)arg["ShowSplash"])
            {
                if (splashScreen == null)
                    splashScreen = new SplashScreen();
                splashScreen.SplashScreenText = "Camera and board getting initialized";
                if (ivl_camera.camPropsHelper.IsCameraConnected == Devices.CameraConnected && ivl_camera.camPropsHelper.IsPowerConnected == Devices.PowerConnected)
                    splashScreen.ShowDialog();
                else
                {

                    splashScreen.Close();
                }
            }
            else
            {
                splashScreen.Close();

            }
        }

        private void LeftRightEvent(bool isLeft)
        {
            try
            {
                Args arg = new Args();
                //if (ivl_camera._Settings.BoardSettings.EnableRightLeftSensor)//to check whether the left ringht sensor is enabled in settings.
                {
                    if (isLeft)
                    {
                        ivl_camera.camPropsHelper.LeftRightPos = LeftRightPosition.Left;

                    }
                    else
                        ivl_camera.camPropsHelper.LeftRightPos = LeftRightPosition.Right;
                    arg["LeftRightPos"] = ivl_camera.camPropsHelper.LeftRightPos;
                    if (eventHandler.ChangeLeftRightPos_Live != null)
                        eventHandler.Notify(eventHandler.ChangeLeftRightPos_Live, arg);
                }
            }
            catch (Exception ex)
            {
                //Exception_Log.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);
            }

        }
        void flowLayoutPanel1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        bool istrigger = false;
        private void TriggerRecievedNotification(string s, Args e)
        {

            istrigger = true;
            saveFrames();
        }
        void IntucamBoardCommHelper__updateAnalogInt2DigitalInt(int val)
        {
            //if (ivl_camera != null)
            //{
            //    expVal_nud.Value = val;
            //    gainVal_nud.Value = val;
            //}
        }
        void IntucamBoardCommHelper__motorResetEvent()
        {
            ResetTimer.Enabled = false;
            ResetTimer.Stop();

            //IntucamBoardCommHelper.GetMotorPosition(iterCommandCnt);
            // IntucamBoardCommHelper.CommandLogList.Add(new LogArgs("Reset Motor Timer done "));
        }
        void IntucamBoardCommHelper_irStatus(bool status, string s)
        {

        }

        void ResetTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ResetTimer.Enabled = false;
            ResetTimer.Stop();
            //IntucamBoardCommHelper.CommandLogList.Add(new LogArgs("Reset Motor Timer Timeout"));
            //IntucamBoardCommHelper.GetMotorPosition(iterCommandCnt);

        }
        void updateFFATime(string s, Args a)
        {
            ffaTimerStatus_lbl.Text = "FFA Time : " + (string)a["ffaTime"];
        }

        int prevSensorPos = 0;
        Bitmap resetBitmapLeft, resetBitmapRight;
        Graphics gPanel12, gPanel11;
        void ShowCameraConnection(String s, Args arg)
        {
            if ((bool)arg["isCameraConnected"])
                cameraStatus_pbx.Image = Image.FromFile(@"ImageResources\CameraConnected.png");
            else
                cameraStatus_pbx.Image = Image.FromFile(@"ImageResources\CameraDisconnected.png");
        }
        void ShowPowerConnection(String s, Args arg)
        {
            try
            {
                if ((bool)arg["isPowerConnected"])
                    powerStatus_pbx.Image = Image.FromFile(@"ImageResources\PowerConnected.png");
                else
                    powerStatus_pbx.Image = Image.FromFile(@"ImageResources\PowerDisconnected.png");
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }



        }
        private void ShowImage(string s, Args arg)
        {
            display_pbx.Image = arg["rawImage"] as Bitmap;
        }
        private void DisplayImage(string s, Args arg)
        {
            try
            {
                Bitmap overlay = arg["rawImage"] as Bitmap;
                display_pbx.Image = overlay;
                //display_pbx.Image = arg["rawImage"] as Bitmap;
            }
            catch (InvalidProgramException ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }

        }

        void IntucamBoardCommHelper__leftRightEvent(bool isLeft)
        {
            if (isLeft)
            {
                left_rb.Checked = true;
            }
            else
            {
                right_rb.Checked = true;
            }
        }
        Point p;
        void overlay_pbx_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (control_tb.SelectedTab.Name == HotSpot_tbp.Name)
                {
                    if (overlay != null)
                    {
                        Graphics g = Graphics.FromImage(overlay);
                        g.FillRectangle(Brushes.Black, new Rectangle(0, 0, overlay.Width, overlay.Height));
                        p = overlay_pbx.TranslatePointToImageCoordinates(e.Location);
                        if (twoX_zoom_rb.Checked)
                        {
                            //coOrdinates_lbl.Text = (p.X ).ToString() + "_" + (p.Y ).ToString();
                            coOrdinates_lbl.Text = (p.X * 2).ToString() + "_" + (p.Y * 2).ToString();
                            //HotSpotCentreX_nud.Value = p.X * 2;
                            //HotSpotCentreY_nud.Value = p.Y * 2;

                        }
                        else
                        {
                            coOrdinates_lbl.Text = (p.X).ToString() + "_" + (p.Y).ToString();
                            //HotSpotCentreX_nud.Value = p.X ;
                            //HotSpotCentreY_nud.Value = p.Y;
                        }
                        Pen _pen = new Pen(Color.White, 2.0f);
                        int shiftVal = 10;
                        g.DrawLine(_pen, p.X, p.Y, p.X, p.Y - shiftVal);
                        g.DrawLine(_pen, p.X, p.Y, p.X, p.Y + shiftVal);
                        g.DrawLine(_pen, p.X, p.Y, p.X - shiftVal, p.Y);
                        g.DrawLine(_pen, p.X, p.Y, p.X + shiftVal, p.Y);
                        g.Dispose();
                        overlay.MakeTransparent(Color.Black);
                        overlay_pbx.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        void display_pbx_MouseMove(object sender, MouseEventArgs e)
        {
            // UpdateZoomedImage(e);

        }
        int FocusSize = 20;
        void drawPeakBitmap()
        {
            // PeakBitmap = new Bitmap(2048, 1536, PixelFormat.Format24bppRgb);
            //Graphics g = Graphics.FromImage(PeakBitmap);
            //g.DrawLine(new Pen(Color.White, 5.0f), new Point(374 +  FocusSize, 268 + FocusSize), new Point(414 + FocusSize, 268 + FocusSize));
            //g.DrawLine(new Pen(Color.White, 5.0f), new Point(374 + FocusSize, 268 + FocusSize), new Point(374 + FocusSize, 308 + FocusSize));

            //g.DrawLine(new Pen(Color.White, 5.0f), new Point(1674, 268), new Point(1634-FocusSize, 268));
            //g.DrawLine(new Pen(Color.White, 5.0f), new Point(1674, 268), new Point(1674, 308+FocusSize));

            //g.DrawLine(new Pen(Color.White, 5.0f), new Point(374, 1228), new Point(374, 1268));
            //g.DrawLine(new Pen(Color.White, 5.0f), new Point(374, 1268), new Point(414, 1268));

            //g.DrawLine(new Pen(Color.White, 5.0f), new Point(1674, 1268), new Point(1634, 1268));
            //g.DrawLine(new Pen(Color.White, 5.0f), new Point(1674, 1268), new Point(1674, 1228));

            //g.DrawLine(new Pen(Color.Green, 5.0f), new Point((PeakBitmap.Width / 2) - 150 - FocusSize, (PeakBitmap.Height / 2) - 150 - FocusSize), new Point((PeakBitmap.Width / 2) - 150, (PeakBitmap.Height / 2) - 150 - FocusSize));
            //g.DrawLine(new Pen(Color.Green, 5.0f), new Point((PeakBitmap.Height / 2) - 150 - FocusSize, (PeakBitmap.Height / 2) - 150 - FocusSize), new Point((PeakBitmap.Height / 2) - 150 - FocusSize, (PeakBitmap.Height / 2) - 150));

            //g.DrawLine(new Pen(Color.Green, 5.0f), new Point((PeakBitmap.Width / 2) - 150 - FocusSize, (PeakBitmap.Height / 2) + 150 + FocusSize), new Point((PeakBitmap.Width / 2) - 150, (PeakBitmap.Height / 2) + 150 +FocusSize));
            //g.DrawLine(new Pen(Color.Green, 5.0f), new Point((PeakBitmap.Height / 2) - 150 - FocusSize, (PeakBitmap.Height / 2) + 150 - FocusSize), new Point((PeakBitmap.Height / 2) - 150 - FocusSize, (PeakBitmap.Height / 2) + 150));

            //g.DrawLine(new Pen(Color.Green, 5.0f), new Point((PeakBitmap.Width / 2) + 150 + FocusSize, (PeakBitmap.Height / 2) + 150 + FocusSize), new Point((PeakBitmap.Width / 2) - 150, (PeakBitmap.Height / 2) + 150 + FocusSize));
            //g.DrawLine(new Pen(Color.Green, 5.0f), new Point((PeakBitmap.Height / 2) + 150 + FocusSize, (PeakBitmap.Height / 2) + 150 + FocusSize), new Point((PeakBitmap.Height / 2) - 150 - FocusSize, (PeakBitmap.Height / 2) + 150));

            //g.DrawLine(new Pen(Color.Green, 5.0f), new Point((PeakBitmap.Width / 2) + 150 + FocusSize, (PeakBitmap.Height / 2) - 150 - FocusSize), new Point((PeakBitmap.Width / 2) + 150, (PeakBitmap.Height / 2) - 150 - FocusSize));
            //g.DrawLine(new Pen(Color.Green, 5.0f), new Point((PeakBitmap.Width / 2) - 150 - FocusSize, (PeakBitmap.Height / 2) - 150 - FocusSize), new Point((PeakBitmap.Width / 2) - 150, (PeakBitmap.Height / 2) - 150 - FocusSize));




        }



        void t_Tick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            // FrameRate_lbl.Text = "Frame Rate :" + (ivl_camera.FrameCnt - ivl_camera.prevCnt).ToString();
        }


        void ExpGain_gbx_MouseLeave(object sender, EventArgs e)
        {
            ExpGain_gbx_Leave(null, null);
        }

        void ExpGain_gbx_MouseEnter(object sender, EventArgs e)
        {
            ExpGain_gbx_Enter(null, null);
        }

        void frameTimer_Tick(object sender, EventArgs e)
        {

        }


        private void connect_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ivl_camera.isCameraOpen)
                {
                    #region toupCam
                    if (ivl_camera != null)
                    {

                        // ivl_camera.picBox = display_pbx;
                        ivl_camera.camPropsHelper._Settings.CameraSettings.isFourteen = (bool)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.CameraSettings._Enable14Bit);// ConfigVariables.CurrentSettings.isForteenBit;
                        ivl_camera.camPropsHelper._Settings.CameraSettings.isRawMode = (bool)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.CameraSettings._EnableRawMode);
                        if (mode_cmbx.SelectedIndex == 4 || mode_cmbx.SelectedIndex == 5)// whether the mode is either FFA plus color / FFA plus 
                        {
                            ivl_camera.camPropsHelper._Settings.CameraSettings.roiX = 0;
                            ivl_camera.camPropsHelper._Settings.CameraSettings.roiY = 0;
                            //if (!ConfigVariables.CurrentSettings.isRawMode)
                            //{
                            //    ivl_camera.camPropsHelper._Settings.CameraSettings.ImageWidth = 3072;
                            //    ivl_camera.camPropsHelper._Settings.CameraSettings.ImageHeight = 2048;
                            //}
                        }
                        else if (mode_cmbx.SelectedIndex == 3)
                        {
                            ivl_camera.camPropsHelper._Settings.CameraSettings.roiX = 0;
                            ivl_camera.camPropsHelper._Settings.CameraSettings.roiY = 0;
                        }
                         ivl_camera.OpenCameraBoard();
                       
                        ivl_camera.InitCameraBoard();
                        isCameraOpen = ivl_camera.isCameraOpen;

                        if (isCameraOpen)
                        {
                    #endregion
                         
                            Args arg = new Args();
                            arg["isCameraConnected"] = isCameraOpen;
                            eventHandler.Notify(eventHandler.UPDATE_CAMERA_STATUS, arg);
                            connect_btn.Text = "Disconnect";
                            EnableControls(true);
                            cameraStatus_lbl.Image = cameraConnectImg;
                            Dictionary<int, int> widthHeight = new Dictionary<int, int>();
                            ivl_camera.camPropsHelper.GetResolution(ref widthHeight);
                            ivl_camera.camPropsHelper._Settings.ImageSaveSettings._ImageSaveFormat = ImageSaveFormat.png;
                            foreach (var item in widthHeight)
                                resolution_combx.Items.Add(item.Key.ToString() + "X" + item.Value.ToString());
                            ivl_camera.camPropsHelper._Settings.CameraSettings.ImageWidth = widthHeight.Keys.ElementAt(0);

                            ivl_camera.camPropsHelper._Settings.CameraSettings.ImageHeight = widthHeight[widthHeight.Keys.ElementAt(0)];


                            if (flashBoost_cbx.Checked)
                                setFlashBoost();
                            setDefaultValues();
                            //ivl_camera.SetFrameRateLevel(0);
                            resolution_combx.SelectedIndex = 0;
                            // ivl_camera.SetGain(100);4
                            //ivl_camera.SetTemperatureTint(6500, 1000);
                            int[] rgb = new int[3];
                            ivl_camera.camPropsHelper.GetRGBGain(rgb);
                            //EEPROM prom = new EEPROM();
                            overlay_pbx.Visible = true;
                            //ivl_camera.saveFramesCnt = (int)saveFramesCount_nud.Value;
                          //  _settingsConfig = SettingsConfig.GetInstance();

                            SetCurrentSettings();
                            ivl_camera.camPropsHelper._Settings.CameraSettings.LiveExposure = ((uint)expVal_nud.Value);
                            ivl_camera.camPropsHelper._Settings.CameraSettings.LiveGain = ((ushort)gainVal_nud.Value);
                            ivl_camera.camPropsHelper._Settings.CameraSettings.CaptureGain = (ushort)flashGain_nud.Value;
                            ivl_camera.camPropsHelper._Settings.CameraSettings.CaptureExposure = ((uint)flashExp_nud.Value);

                            ivl_camera.camPropsHelper.RotateImageVertical(vFlip_cbx.Checked);
                            ivl_camera.camPropsHelper.RotateImageHorizontal(hFlip_cbx.Checked);
                            EnableTemperatureTint_cbx_CheckedChanged(null, null);
                            formCheckBox1_CheckedChanged_1(null, null);

                            //if (ir_rb.Checked)// || low_flash_rb.Checked)
                            //    ivl_camera.IRLightOnOff(ir_rb.Checked )//|| low_flash_rb.Checked);
                            //else
                            //    ivl_camera.WhiteLightOnOff(ir_rb.Checked);

                            if (imagingMode == INTUSOFT.Imaging.ImagingMode.FFA_Plus)
                                ivl_camera.camPropsHelper.SetMonoChromeMode(true);
                            else
                                ivl_camera.camPropsHelper.SetMonoChromeMode(false);

                            ivl_camera.StartLive();

                            overlay_pbx.Visible = false;



                        }
                    }


                }
                else
                {
                    isCameraOpen = false;
                    connect_btn.Text = "Connect";
                    cameraStatus_lbl.Image = cameraDisconnectImg;
                    overlay_pbx.Image = new Bitmap(10, 10);
                    display_pbx.Image = new Bitmap(10, 10);
                    t.Enabled = false;
                    t.Stop();
                    ivl_camera.DisconnectCameraModule();
                    XmlConfigUtility.Serialize(ConfigVariables._ivlConfig, IVLConfig.fileName);

                    //SettingsConfig.Serialize(_settingsConfig, SettingsConfig.fileName);

                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            

        }
        int peakCnt = 0;
        int peakDrop = 0;
        private void SetUIFromConfig()
        {
          mode_cmbx.SelectedIndex =  mode_cmbx.Items.IndexOf(ConfigVariables._ivlConfig.Mode.ToString());
            //mode_cmbx.Text = ;

        }
        void ivl_camera__focusState(int focusState, EventArgs e)
        {
            //if (ivl_camera.isFocusStart)
            //    AutoFocusWriter.WriteLine(ivl_camera.currentPeak);
            //switch (focusState)
            //{
            //    case 0:
            //        {
            //            BrightBitmap.MakeTransparent(Color.Black);
            //            break;
            //        }

            //    case 1:
            //        {
            //            if (ivl_camera.isFocusStart)
            //            {

            //                //ivl_camera.isForward = !ivl_camera.isForward;
            //                //if (ivl_camera.isForward)
            //                //{
            //                //    IntucamBoardCommHelper.MotorForward(Convert.ToByte(4));
            //                //}
            //                //else
            //                //{
            //                //    IntucamBoardCommHelper.MotorBackward(Convert.ToByte(4));

            //                //}
            //            }
            //            break;
            //        }
            //    case 2:
            //        {

            //            if (ivl_camera.focusList.Count == 30)
            //            {
            //                //drawGraph(ivl_camera.focusList);
            //                //ivl_camera.focusList.RemoveRange(0, ivl_camera.focusList.Count - (ivl_camera.focusList.Count - 5));
            //            }

            //            peakDetection();
            //            #region commented Peak Detection
            //            //if (ivl_camera.peakList.Count > 5)
            //            //{
            //            //    if (ivl_camera.peakList[ivl_camera.peakList.Count - 1] > ivl_camera.peakList[ivl_camera.peakList.Count - 2] && ivl_camera.peakList[ivl_camera.peakList.Count - 2] > ivl_camera.peakList[ivl_camera.peakList.Count - 3] &&
            //            //        ivl_camera.peakList[ivl_camera.peakList.Count - 3] > ivl_camera.peakList[ivl_camera.peakList.Count - 4] &&
            //            //            ivl_camera.peakList[ivl_camera.peakList.Count - 4] > ivl_camera.peakList[ivl_camera.peakList.Count - 5])
            //            //        ivl_camera.prevPeak = ivl_camera.peakList[ivl_camera.peakList.Count - 1];
            //            //    #region commented code
            //            //    //    if (ivl_camera.currentPeak > (0.99) * ivl_camera.prevPeak)
            //            //    //        ivl_camera.prevPeak = ivl_camera.currentPeak;

            //            ////if (ivl_camera.currentPeak > ivl_camera.prevPeak)
            //            //    //{
            //            //    //    peakCnt++;
            //            //    //    if(peakCnt >10)
            //            //    //    ivl_camera.prevPeak = ivl_camera.currentPeak;
            //            //    //   // CovVal_lbl.Text = ivl_camera.currentPeak.ToString();
            //            //    //    FocusSize -= 1;
            //            //    //    label6.Visible = false;
            //            //    //    IntucamBoardCommHelper.MotorForward(Convert.ToByte(1));

            //            ////    //drawPeakBitmap();
            //            //    //    //PeakBitmap.MakeTransparent(Color.Black);
            //            //    //    //focusMarker_pbx.Image = PeakBitmap;
            //            //    //}
            //            //    //else if(ivl_camera.prevPeak !=0 && ivl_camera.currentPeak > (0.97 * ivl_camera.prevPeak))
            //            //    //{
            //            //    //    //ivl_camera.prevPeak = ivl_camera.currentPeak;
            //            //    //    // CovVal_lbl.Text = ivl_camera.currentPeak.ToString();
            //            //    //    IntucamBoardCommHelper.MotorFastForward(Convert.ToByte(20));

            //            ////    label6.Visible = true;
            //            //    //    FocusSize += 1;
            //            //    //}

            //            //    #endregion
            //            //    else if (ivl_camera.currentPeak < (.99) * ivl_camera.prevPeak)
            //            //    {
            //            //        if (ivl_camera.isFocusStart)
            //            //        {
            //            //            peakDrop++;
            //            //            if (peakDrop > 10)
            //            //            {
            //            //                ivl_camera.isFocusStart = false;
            //            //                ivl_camera.PeakIndx = ivl_camera.stepCnt - 5;

            //            //                label6.Visible = false;
            //            //                if (!ivl_camera.isForward)
            //            //                {
            //            //                    ivl_camera.isFocusStart = false;
            //            //                    ivl_camera.isForward = true;

            //            //                }
            //            //                else
            //            //                    ivl_camera.isForward = false;
            //            //                #region Commented code
            //            //                //IntucamBoardCommHelper.b_MFBK_Done = false;
            //            //                //try
            //            //                //{
            //            //                //    if (ivl_camera.PeakIndx > 4)
            //            //                //        IntucamBoardCommHelper.MotorFastBackward(Convert.ToByte(ivl_camera.PeakIndx - 4));
            //            //                //    else
            //            //                //        IntucamBoardCommHelper.MotorFastBackward(Convert.ToByte(ivl_camera.PeakIndx - 1));
            //            //                //}
            //            //                //catch (Exception ex)
            //            //                //{
            //            //                //    Console.WriteLine(ex.Message);
            //            //                //}
            //            //                //while (!IntucamBoardCommHelper.b_MFBK_Done)
            //            //                //{
            //            //                //}
            //            //                //IntucamBoardCommHelper.b_MFBK_Done = false;
            //            //                //IntucamBoardCommHelper.b_MFFR_Done = false;
            //            //                //IntucamBoardCommHelper.MotorFastForward(Convert.ToByte(ivl_camera.PeakIndx + 1));
            //            //                //while (!IntucamBoardCommHelper.b_MFFR_Done)
            //            //                //{
            //            //                //}
            //            //                //IntucamBoardCommHelper.b_MFFR_Done = false;
            //            //                #endregion
            //            //                ivl_camera.isFocusStart = true;
            //            //                ivl_camera.PeakIndx = 0;
            //            //                peakDrop = 0;
            //            //            }
            //            //        }
            //            //    }
            //            //    if (ivl_camera.prevPeak == 0)
            //            //    {
            //            //        label6.Visible = false;
            //            //        peakCnt = 0;
            //            //        ivl_camera.prevPeak = ivl_camera.currentPeak;
            //            //    }
            //            #endregion

            //            break;


            //        }

            //}
        }

        //public static Settings ConfigVariables.CurrentSettings
        //{
        //    get { return ConfigVariables.ConfigVariables.CurrentSettings; }
        //    set { ConfigVariables.ConfigVariables.CurrentSettings = value; }
        //}
        void SetCurrentSettings()
        {
            try
            {

                if (ivl_camera.isCameraOpen)
                {
                    if ((bool)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.FirmwareSettings._EnableSingleFrameCapture))
                        singleFrameCapture_combox.Text = "True";
                    else
                        singleFrameCapture_combox.Text = "False";
                    //ivl_camera.camPropsHelper.ImagingMode = Enum.Parse(INTUSOFT.Imaging.ImagingMode,;
                       
                    ivl_camera.camPropsHelper.SetMonoChromeMode(false);
                    startFFATimer_btn.Enabled = false;

                    isdefaultExp = false;
                    flashBoost_nud.Value = (int)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.FirmwareSettings._FlashBoostValue);
                    if (ivl_camera.camPropsHelper.IsBoardOpen)
                        flashBoost_cbx.Checked = (bool)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.FirmwareSettings._isFlashBoost);
                    flashoffset1_nud.Value = (int)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.FirmwareSettings._FlashOffsetStart);
                    flashOffset2_nud.Value = (int)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.FirmwareSettings._FlashOffsetEnd);
                    gainVal_nud.Value = (int)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.CameraSettings.LiveGain);
                    flashGain_nud.Value = (int)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.CameraSettings.CaptureGain); ;
                    expVal_nud.Value = (int)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.CameraSettings._Exposure); ;
                    flashExp_nud.Value = (int)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.CameraSettings._FlashExposure); ;
                    IrOffsetSteps_nud.Value =(int)ConfigVariables.StringVal2Object( ConfigVariables.CurrentSettings.MotorOffSetSettings.IR2Flash);
                    MotorForward_cbx.Checked = (bool)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.FirmwareSettings._IsMotorPolarityForward); ;
                    forteenBit_rb.Checked = (bool)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.CameraSettings._Enable14Bit); ;
                   
                    vFlip_cbx.Checked = (bool)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.CameraSettings._EnableVerticalFlip);
                    hFlip_cbx.Checked = (bool)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.CameraSettings._EnableHorizontalFlip);
//                    EnableTemperatureTint_cbx.Checked = (bool)ConfigVariables.CurrentSettings.EnableTemperatureTint;
  //                  formCheckBox1.Checked = (bool)ConfigVariables.CurrentSettings.EnableCC;
                    if (EnableTemperatureTint_cbx.Checked)
                    {
    //                    temperature_tb.Value = (int)ConfigVariables.CurrentSettings.temperature;
      //                  tint_tb.Value =(int) ConfigVariables.CurrentSettings.tint;
                    }
                    //liveRedGain_tb.Value = (int)ConfigVariables.CurrentSettings.analogGainSettings.liveR;
                    //liveGreenGain_tb.Value = (int)ConfigVariables.CurrentSettings.analogGainSettings.liveG;
                    //liveBlueGain_tb.Value = (int)ConfigVariables.CurrentSettings.analogGainSettings.liveB;

                    //redGain_tb.Value = (int)ConfigVariables.CurrentSettings.analogGainSettings.CaptureR;
                    //greenGain_tb.Value = (int)ConfigVariables.CurrentSettings.analogGainSettings.CaptureG;
                    //blueGain_tb.Value = (int)ConfigVariables.CurrentSettings.analogGainSettings.CaptureB;


                    frameDetectionVal_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.CameraSettings._FrameDetectionValue));
                    darkFameDetectionVal_nud.Value = Convert.ToDecimal(  ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.CameraSettings._DarkFrameDetectionValue));
                    SetGetRGBGain(false);
                    SaveIr_cbx.Checked = (bool)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.ImageStorageSettings._IsIrSave);
                    SaveDebugImage_cbx.Checked = true;
                    SaveProcessedImage_cbx.Checked = true;
                    saveRaw_cbx.Checked = true;
                    FFA_Color_Pot_Int_Offset_nud.Value =(int) ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.FirmwareSettings.PotOffsetValue);
                    FFA_Pot_Int_Offset_nud.Value = (int)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.FirmwareSettings.PotOffsetValue);
                    //liveCC_cbx.Checked = (bool)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.ImageShiftSettings.;
                    enableLivePostProcessing_cbx.Checked =(bool) ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.ImageShiftSettings.LivePP);
                    LedCode liveLed = (LedCode) Enum.Parse( typeof( LedCode), ConfigVariables.CurrentSettings.CameraSettings.LiveLedSource.val);
                    if (liveLed == LedCode.IR)
                        ir_rb.Checked = true;
                    else
                        if (liveLed == LedCode.Flash)
                            flash_rb.Checked = true;
                        else if (liveLed == LedCode.Blue)
                            blue_rb.Checked = true;


                    switch (mode_cmbx.Text)
                    {
                        case "45 plus Color":
                            {
                                flashoffset1_nud.Visible = false;
                                flashOffset2_nud.Visible = false;

                                ivl_camera.camPropsHelper.Read_LED_SupplyValues();
                                // ivl_camera.ServoMove(1150, 1320);
                                ivl_camera.camPropsHelper.ServoMove((int)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.FirmwareSettings.GreenFilterPos));
                            }
                            break;
                        case "45 plus FFA":
                            {
                                flashoffset1_nud.Visible = false;
                                flashOffset2_nud.Visible = false;
                                ivl_camera.camPropsHelper.Read_LED_SupplyValues();
                                ivl_camera.camPropsHelper.SetMonoChromeMode(true);
                                startFFATimer_btn.Enabled = true;
                                // ivl_camera.ServoMove(1780, 1670);
                                if (ivl_camera.LEDSource == Led.IR)
                                    ivl_camera.camPropsHelper.ServoMove((int)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.FirmwareSettings.GreenFilterPos));
                                else
                                    ivl_camera.camPropsHelper.ServoMove((int)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.FirmwareSettings.BlueFilterPos));

                                break;
                            }
                        case "45":
                            {
                                flashoffset1_nud.Visible = false;
                                flashOffset2_nud.Visible = false;
                                break;
                            }

                    }

                }
                #region post Processing settings from config file
                // else
                {
                    if (rawModeFrameGrab_rb.Enabled)
                    {
                        rawModeFrameGrab_rb.Checked = (bool)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.CameraSettings._EnableRawMode);
                        CCMode_rb.Checked = !rawModeFrameGrab_rb.Checked;
                    }
                    redRed_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object( ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._RRCompensation));
                    redGreen_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._RGCompensation));
                    redBlue_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._RBCompensation));
                    greenRed_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._GRCompensation));
                    greenGreen_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._GGCompensation));
                    greenBlue_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._GBCompensation));
                    blueRed_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._BRCompensation));
                    blueGreen_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._BGCompensation));
                    blueBlue_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._BBCompensation));
                    isApplyLut_cbx.Checked =(bool) ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.ClaheSettings._IsApplyClaheSettings);
                    SaveIr_cbx.Checked = (bool)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.ImageStorageSettings._IsRawSave);
                    saveFramesCount_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.CameraSettings._SaveFramesCount));
                    applyBrightness_cbx.Checked = (bool)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.BrightnessContrastSettings._IsApplyBrightness);
                    ApplyContrast_cbx.Checked = (bool)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.BrightnessContrastSettings._IsApplyContrast);

                    blueFilterPos_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.FirmwareSettings.BlueFilterPos));
                    greenFilterPos_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.FirmwareSettings.GreenFilterPos));

                    brightness_nud.Value =Convert.ToDecimal(  ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.BrightnessContrastSettings._BrightnessVal));
                    contrast_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.BrightnessContrastSettings._ContrastVAl));
                    contCapture_cbx.Checked = (bool)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.CameraSettings.IsContinousCapture);
                    cameraModel_cmbx.Text = (string)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.CameraSettings._CameraModel);
                    applyGamma_cbx.Checked = (bool)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.GammaSettings.IsApplyGammaSettings);
                    gammaVal_nud.Value = Convert.ToDecimal(ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.GammaSettings.GammaValue));

                    showExtViewer_cbx.Checked =(bool)ConfigVariables.StringVal2Object( ConfigVariables.CurrentSettings.ImageStorageSettings.IsShowExtViewer);
                    ApplyLiteCorrection_cbx.Checked =(bool) ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.UnsharpMaskSettings._IsApplyUnsharpSettings);
                    postProcessing_cbx.Checked = (bool)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.PostProcessing.EnablePostProcessing);
                    applyColorCorrection_cbx.Checked = (bool)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._IsApplyColorCorrection);
                    applyShift_cbx.Checked = (bool)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.ImageShiftSettings._IsApplyImageShift);
                    applyHotSpotCorrection_cbx.Checked =(bool) ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._IsApplyHotspotCorrection);
                    applyMask_cbx.Checked =(bool) ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.MaskSettings._IsApplyMask);
                    applyClahe_cbx.Checked = (bool)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.ClaheSettings._IsApplyClaheSettings);

                    xShift_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.ImageShiftSettings._ImageShiftX));//.shiftX);
                    Yshift_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.ImageShiftSettings._ImageShiftY));//.shiftY);

                    HotSpotCentreX_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.CameraSettings._ImageOpticalCentreX));//.CentreX;;
                    HotSpotCentreY_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.CameraSettings._ImageOpticalCentreY));//.CentreY;;



                    HSRad1_nud.Value =Convert.ToDecimal( ConfigVariables.StringVal2Object( ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._HotspotRadius1));
                    HSRad2_nud.Value =Convert.ToDecimal(  ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._HotspotRadius2));

                    ShadowRad1_nud.Value =Convert.ToDecimal(  ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowRadSpot1));
                    ShadowRad2_nud.Value =Convert.ToDecimal(  ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowradSpot2));
                    ivl_camera.camPropsHelper._Settings.PostProcessingSettings.hotspotSettings.radSpot1 =(int)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowRadSpot1);//.ShadowRadSpot1;

                    unsharpAmount_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.UnsharpMaskSettings._UnSharpAmount));//amount;
                    unsharpRadius_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.UnsharpMaskSettings._UnSharpRadius));//radius;
                    unsharpThresh_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.UnsharpMaskSettings._Threshold));//thresh;
                    medianfilter_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.UnsharpMaskSettings._MedFilter));//.medianFilterWindow;
                    ClaheClipValueR_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.ClaheSettings._ClipValueR));
                    ClaheClipValueG_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.ClaheSettings._ClipValueG));
                    ClaheClipValueB_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.ClaheSettings._ClipValueB));//.ClaheClipValB;

                    maskX_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.CameraSettings._ImageOpticalCentreX));//.CentreX;
                    maskY_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.CameraSettings._ImageOpticalCentreY));//.CentreY;

                    LiveMask_cbx.Checked = (bool)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.MaskSettings._ApplyLiveMask);
                    LiveMaskWidth_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.MaskSettings.LiveMaskWidth));
                    LiveMaskHeight_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.MaskSettings.LiveMaskHeight));//.LiveMaskHeight;

                    CaptureMaskWidth_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.MaskSettings.CaptureMaskWidth));//.imageCentreSettings.CaptureMaskWidth;
                    CaptureMaskHeight_nud.Value =Convert.ToDecimal( ConfigVariables.StringVal2Object( ConfigVariables.CurrentSettings.PostProcessingSettings.MaskSettings.CaptureMaskHeight));//.CaptureMaskHeight;

                    ApplyHSVBoost_cbx.Checked =(bool) ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.ImageShiftSettings.IsApplyHSV);
                    hsvBoostVal_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.ImageShiftSettings.HsvBoost));//.HSVBoostVal;

                    shadowBluePeak_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowBlueBPercentage));
                    shadowGreenPeak_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowGreenPercentage));
                    shadowRedPeak_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowRedPercentage));

                    HsRedPeak_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotRedPeak));
                    HsGreenPeak_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotGreenPeak));
                    HsBluePeak_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotBluePeak));

                    HsRedRadius_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotRedRadius));

                    HsGreenRadius_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotGreenRadius));
                    HsBlueRadius_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotBlueRadius));

                    LUT_interval1_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings._LUTInterval1));
                    LUT_interval2_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings._LUTInterval2));
                    LUT_SineFactor_nud.Value =Convert.ToDecimal(  ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings._LUTSineFactor));
                    offset1_nud.Value = Convert.ToDecimal( ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings._LUTOffset));

                    redChannelSineFactor_nud.Value = Convert.ToDecimal(ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_SineFactor_R));
                    redChannelLutInterval1_nud.Value = Convert.ToDecimal(ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_interval1_R));
                    redChannelLutInterval2_nud.Value = Convert.ToDecimal(ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_interval2_R));
                    redChannelLutOffset_nud.Value = Convert.ToDecimal(ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_Offset_R));

                    greenChannelSineFactor_nud.Value = Convert.ToDecimal(ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_SineFactor_G));
                    greenChannelLutInterval1_nud.Value = Convert.ToDecimal(ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_interval1_G));
                    greenChannelLutInterval2_nud.Value = Convert.ToDecimal(ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_interval2_G));
                    greenChannelLutOffset_nud.Value = Convert.ToDecimal(ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_Offset_G));

                    blueChannelSineFactor_nud.Value = Convert.ToDecimal(ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_SineFactor_B));
                    blueChannelLutInterval1_nud.Value = Convert.ToDecimal(ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_interval1_B));
                    blueChannelLutInterval2_nud.Value = Convert.ToDecimal(ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_interval2_B));
                    blueChannelLutOffset_nud.Value = Convert.ToDecimal(ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_Offset_B));

                    isChannelWiseLut = Convert.ToBoolean(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings._IsChannelWiseLUT.val);

                    if ((bool)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.CameraSettings._Enable14Bit))
                        forteenBit_rb.Checked = true;
                    else
                        eightBit_rb.Checked = true;
                }
                #endregion

            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }
        int loop = 0;
        bool peakFound = false;
        bool LoopCompleted = false;
        // Old Peak Detection commented by sriram 
        //void peakDetection()
        //{
        //    if (ivl_camera.isFocusStart)
        //    {
        //        //if (ivl_camera.peakList.Count > 2)
        //        if (ivl_camera.prevPeak != 0 && ivl_camera.currentPeak < (0.90) * ivl_camera.prevPeak)
        //        {
        //            peakDrop++;

        //        }
        //        if (peakDrop >= 3)
        //        {

        //            label6.Text = " peak Dropped go in opposite direction";
        //            //peakFound = true;
        //            label6.Visible = true;
        //            peakDrop = 0;
        //            if (ivl_camera.isFocusStart)
        //            {
        //                if (ivl_camera.isForward)
        //                {
        //                    forwardMotion_lbl.Text = "Backward";

        //                    IntucamBoardCommHelper.MotorFastBackward(Convert.ToByte(1));
        //                    IntucamBoardCommHelper.MotorFastBackward(Convert.ToByte(ivl_camera.stepCnt));
        //                    ivl_camera.stepCnt = 0;
        //                    ivl_camera.isForward = false;
        //                    if (!LoopCompleted)
        //                    {
        //                        ivl_camera.isFocusStart = false;
        //                        ivl_camera.isForward = false;
        //                        AutoFocusWriter.Close();
        //                        AutoFocusWriter.Dispose();
        //                        MessageBox.Show("Focus Complete Forward  = " +autoFocusWatch.ElapsedMilliseconds.ToString());
        //                    }
        //                }
        //                else
        //                {
        //                    forwardMotion_lbl.Text = "Forward";

        //                    IntucamBoardCommHelper.MotorFastForward(Convert.ToByte(1));
        //                    IntucamBoardCommHelper.MotorFastForward(Convert.ToByte(ivl_camera.stepCnt));
        //                    ivl_camera.stepCnt = 0;
        //                    ivl_camera.isForward = true;
        //                    if (LoopCompleted)
        //                    {
        //                        ivl_camera.isFocusStart = false;
        //                        ivl_camera.isForward = true;
        //                        AutoFocusWriter.Close();
        //                        AutoFocusWriter.Dispose();
        //                        MessageBox.Show("Focus Complete Backward =  " + autoFocusWatch.ElapsedMilliseconds.ToString());

        //                    }
        //                }

        //            }

        //        }


        //    }
        //}

        int motorSteps = 8;
        void peakDetection()
        {

        }
        void ivl_camera_FrameCaptured(bool value, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        void ivl_camera_FrameTransferStatus(string a, EventArgs e)
        {
            //throw new NotImplementedException();
            FrameStatus_lbl.Text = a;
        }

        void ivl_camera_statusBarUpdate(string a, Args e)
        {
            try
            {
                FrameRate_lbl.Text = "Frame Rate :" + e["FrameRate"] as string + "TrigSt =" + e["TriggerStatus"] as string;
                toolStripStatusLabel1.Text = "Exposure : " + e["Exposure"] + "  Gain : " + e["Gain"];
                if ((bool)e["isBoardOpen"])
                    ComPortStatus_lbl.Text = "Board   : " + "Open";
                else
                    ComPortStatus_lbl.Text = "Board   : " + "Closed"; ;
                FrameStatus_lbl.Text = string.Format("Time Difference = {0}", (int)e["TimeDiff"]);
                // serialRetVal_lbl.Text = "SerialData : "+ IntucamBoardCommHelper.SerialRetVal;
                //if (control_tb.SelectedTab == misc_tbpg)
                //{
                //    IntucamBoardCommHelper.CheckLeftRightSensorStatus();
                //    IntucamBoardCommHelper.CheckMotorSensorStatus();
                //}
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }

        }

        private void CameraUI_Shown(object sender, EventArgs e)
        {
            RBwidth = pos_pbx.Width;
            RBheight = pos_pbx.Height;
            LBwidth = neg_pbx.Width;
            LBheight = neg_pbx.Height;
            LeftBitmap = new Bitmap(neg_pbx.Width, neg_pbx.Height);// Added this to manage the blinking happening in the UI to show the sensor position when the rotary is moved instead of using the panel drawing
            RightBitmap = new Bitmap(pos_pbx.Width, pos_pbx.Height);// Added this to manage the blinking happening in the UI to show the sensor position when the rotary is moved instead of using the panel drawing
            SetUIFromConfig();
        
        }


        bool spaceBar = false;
        private void CameraUI_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Space)
            //{

            //  //  saveFrames_btn_Click(null,null);
            //    //capture_btn_Click(null, null);
            //}
        }

        private void capture_btn_Click(object sender, EventArgs e)
        {
            //if (capture_btn.Text == "Record")
            //{

            //    if (ivl_camera.writeThreadBusy)
            //        return;
            //    string fileDir = saveDinf.FullName + Path.DirectorySeparatorChar + "video_" + DateTime.Now.ToString("HHmmss");
            //    if (!Directory.Exists(fileDir))
            //        saveFrameDinf = Directory.CreateDirectory(fileDir);
            //    Camera.FileName = fileDir;
            //    int i = CvInvoke.CV_FOURCC('D', 'I', 'B', ' ');//Four Char code for unCompressed Raw Frames
            //    ivl_camera.vw = new VideoWriter(saveFrameDinf.FullName+Path.DirectorySeparatorChar+ "Video" + ".avi", i, 6, 2048, 1536, false);
            //    capture_btn.Text = "Stop Record";
            //    ivl_camera.StartRecord = true;

            //    //t.Enabled = true;
            //    //t.Start();

            //}
            //else
            //{
            //    ivl_camera.StartRecord = false;
            //    capture_btn.Text = "Record";


            //    //ivl_camera.setResolution(0);
            //    //ivl_camera.StartLiveMode();
            //    //t.Enabled = false;
            //    //t.Stop();

            //}
            //capture_btn.TabStop = false;

            //capture_btn.Refresh();


        }

        private void CameraUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (ivl_camera != null)
                {
                    ivl_camera.DisconnectCameraModule();
                }
                XmlConfigUtility.Serialize(ConfigVariables._ivlConfig, IVLConfig.fileName);
                   // SettingsConfig.Serialize(_settingsConfig, SettingsConfig.fileName);

            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
        }

        private void autoExp_cbx_CheckedChanged(object sender, EventArgs e)
        {
            //ivl_camera.EnableAutoExposure(autoExp_cbx.Checked);
        }

        private void gainVal_tb_Scroll(object sender, EventArgs e)
        {

        }

        private void expVal_tb_Scroll(object sender, EventArgs e)
        {

        }

        private void basicControls_tbp_Click(object sender, EventArgs e)
        {

        }

        private void ExpGain_gbx_Enter(object sender, EventArgs e)
        {
            // ExpGain_gbx.Size = new Size(220, 200);
        }

        private void ExpGain_gbx_Leave(object sender, EventArgs e)
        {
            // ExpGain_gbx.Size = new Size(220, 20);
        }


        private void hueVal_tb_Scroll(object sender, EventArgs e)
        {
            //hueVal_nud.Value = hueVal_tb.Value;
            //hueVal_tb.Refresh();
            //hueVal_nud.Refresh();

        }

        private void saturationVal_tb_Scroll(object sender, EventArgs e)
        {
            //saturationVal_nud.Value = saturationVal_tb.Value;
            //saturationVal_tb.Refresh();
            //saturationVal_nud.Refresh();
        }


        private void EnableControls(bool enable)
        {
            ExpGain_gbx.Enabled = enable;
            //resolution_p.Enabled = enable;
            saveFrames_btn.Enabled = enable;
            //tempTint_gbx.Enabled = enable;
            captureExpGain_gbx.Enabled = enable;
            rawModeFrameGrab_rb.Enabled = enable;
            CCMode_rb.Enabled = enable;
        }



        private void temperature_tb_Scroll(object sender, EventArgs e)
        {

            try
            {
                if (EnableTemperatureTint_cbx.Checked)
                {
                    
                        ConfigVariables.CurrentSettings.CameraSettings._Temperature.val= temperature_tb.Value.ToString();
                    UpdateConfigSettings();
                    TemperatureVal_lbl.Text = temperature_tb.Value.ToString();
                    if (ivl_camera != null)
                    {
                        ivl_camera.camPropsHelper.SetTemperatureTint(temperature_tb.Value, tint_tb.Value);
                        int[] RGBGain = new int[3];
                        //ivl_camera.GetTemperatureTint(ref tempVal, ref tintVal);// ref RGBGain);
                        temperaturStatus_lbl.Text = "Temp: " + tempVal.ToString();
                        tintStatus_lbl.Text = "Temp: " + tintVal.ToString();

                    }
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            

        }

        private void colorMode_rb_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                

                ConfigVariables.CurrentSettings.CameraSettings._EnableRawMode.val = rawModeFrameGrab_rb.Checked.ToString();
                UpdateConfigSettings();
                ivl_camera.camPropsHelper.IsRawMode = rawModeFrameGrab_rb.Checked;
                this.Cursor = Cursors.WaitCursor;
                if (ivl_camera.isCameraOpen)
                {
                    ivl_camera.RestartCamera();
                    ivl_camera.camPropsHelper._Settings.CameraSettings.LiveGain = (ushort)gainVal_nud.Value;
                    ivl_camera.camPropsHelper._Settings.CameraSettings.LiveExposure = (uint)expVal_nud.Value;
                    ivl_camera.camPropsHelper.RotateImageVertical(vFlip_cbx.Checked);
                    ivl_camera.camPropsHelper.RotateImageHorizontal(hFlip_cbx.Checked);

                    this.Cursor = Cursors.Default;
                    bool val = CCMode_rb.Checked;
                    tempTint_gbx.Enabled = EnableTemperatureTint_cbx.Checked;
                    display_pbx.Refresh();
                    //ivl_camera.SetColorMode(colorMode_rb.Checked);
                    //bool colorMode = false;
                    //ivl_camera.GetColorMode(ref colorMode);
                }
                this.Cursor = Cursors.Default;

            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        void FrameCaptureDone(string s, Args arg)
        {
            try
            {
                // bg.CancelAsync();
                if ((bool)arg["isCaptureFailed"])
                {
                    string errorCode = "";
                    if (arg.ContainsKey("Capture Failure Category"))
                        errorCode = arg["Capture Failure Category"] as string;
                    MessageBox.Show("Capture Failure  " + errorCode);

                }
                display_pbx.Refresh();


                colorBm = display_pbx.Image as Bitmap;



                if (channelsImg != null)
                    channelsImg.Dispose();
                this.Cursor = Cursors.Default;
                if (contCapture_cbx.Checked)
                    saveFrames();
                else
                {
                    ivl_camera.TriggerOn();
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }
        private void createImage()
        {
            try
            {
                Bitmap testImage = new Bitmap(6, 6, PixelFormat.Format24bppRgb);
                for (int i = 0; i < testImage.Height; i++)
                {
                    for (int j = 0; j < testImage.Width; j++)
                    {
                        switch (i)
                        {
                            case 0:
                                {
                                    Color c = Color.FromArgb(128, 0, 0);
                                    testImage.SetPixel(j, i, c);
                                    break;
                                }
                            case 1:
                                {

                                    Color c = Color.FromArgb(0, 128, 0);
                                    testImage.SetPixel(j, i, c);
                                    break;
                                }
                            case 2:
                                {
                                    Color c = Color.FromArgb(0, 0, 128);
                                    testImage.SetPixel(j, i, c);
                                    break;
                                }
                            case 3:
                                {
                                    Color c = Color.FromArgb(0, 128, 128);
                                    testImage.SetPixel(j, i, c);
                                    break;
                                }
                            case 4:
                                {
                                    Color c = Color.FromArgb(128, 128, 0);
                                    testImage.SetPixel(j, i, c);
                                    break;
                                }
                            case 5:
                                {
                                    Color c = Color.FromArgb(128, 0, 128);
                                    testImage.SetPixel(j, i, c);
                                    break;
                                }
                        }

                    }
                }
                testImage.Save("testImg.png");
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }
        bool isdefaultExp = false;
        private void setDefaultValues()
        {
            try
            {
                if (ivl_camera.isCameraOpen)
                {


                    resolution_combx.SelectedIndex = 0;
                    //ivl_camera.setResolution(resolution_combx.SelectedIndex);
                    //maxEVal = 64;
                    //minEVal = 0;
                    //maxGVal = 64;
                    //minGVal = 0;
                    bool ret = ivl_camera.camPropsHelper.GetExposureRange(ref minEVal, ref maxEVal, ref defEVal);
                    maxEVal = 300000;
                    isdefaultExp = true;
                    temperature_tb.SetRange(temperatureMin, temperatureMax);
                    tint_tb.SetRange(tintMin, tintMax);
                    // maxEVal = 300000;// maximmum exposure to be set 300 ms by sriram on 16th dec 2015
                    //defEVal = 77500;// maximmum exposure to be set 300 ms by sriram on 16th dec 2015
                    ret = ivl_camera.camPropsHelper.GetGainRange(ref minGVal, ref maxGVal, ref defGVal);
                    //Resolution_lbl.Text = "Res:" + resolution_combx.SelectedItem.ToString();
                    // expVal_tb.Maximum = (int)(300000);//this is used to accomodate both 2mp and 3 mp toupcams exposure
                    expVal_nud.Minimum = (int)(minEVal);
                    flashExp_nud.Minimum = (int)(minEVal);
                    // expVal_nud.Maximum = (int)(300000);//this is used to accomodate both 2mp and 3 mp toupcams exposure
                    expVal_nud.Maximum = (int)maxEVal;
                    flashExp_nud.Maximum = (int)maxEVal;
                    gainVal_nud.Minimum = minGVal;
                    flashGain_nud.Minimum = minGVal;
                    gainVal_nud.Maximum = maxGVal;
                    flashGain_nud.Maximum = maxGVal;
                    isdefaultExp = false;
                    if (resolution_combx.SelectedItem.ToString().Contains("3072"))
                    {
                        //CCMode_rb.Checked = true;
                        forteenBit_rb.Checked =(bool) ConfigVariables.StringVal2Object( ConfigVariables.CurrentSettings.CameraSettings._Enable14Bit);

                    }



                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }
        private void resolution_combx_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        string fileName = "";
        private void saveImages_btn_Click(object sender, EventArgs e)
        {
            fileName = saveDinf.FullName + Path.DirectorySeparatorChar + DateTime.Now.ToString("HHmmss") + ".png";

            //ivl_camera.SaveImage(resolution_combx.SelectedIndex, fileName);
            SaveSettingsLog();
        }
        private void SaveSettingsLog()
        {
            try
            {
                string[] logFileNameArr = fileDir.Split('.');
                StreamWriter st = new StreamWriter(fileDir + Path.DirectorySeparatorChar + "_Settings.log");
                st.WriteLine("Live Exposure =" + expVal_nud.Value.ToString());
                st.WriteLine("Flash Exposure =" + FlashExposure.ToString());
                st.WriteLine("Live Gain =" + gainVal_nud.Value.ToString());
                st.WriteLine("Flash Gain =" + flashGain_nud.Value.ToString());
                //st.WriteLine("Hue =" + hueVal_nud.Value.ToString());
                //st.WriteLine("Saturation =" + saturationVal_nud.Value.ToString());
                st.WriteLine("Temperature =" + temperature_tb.Value.ToString());
                st.WriteLine("Tint =" + tint_tb.Value.ToString());
                st.Close();
                st.Dispose();
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
           

        }

        private void SavePPSettings(FileInfo finf)
        {
            try
            {
                string[] logFileNameArr = fileDir.Split('.');
                StreamWriter st = new StreamWriter(finf.Directory.FullName + Path.DirectorySeparatorChar + DateTime.Now.ToString("HHmmss") + "_PPSettings.log");
                st.WriteLine("RR Compensation =" + redRed_nud.Value.ToString());
                st.WriteLine("RG Compensation =" + redGreen_nud.Value.ToString());
                st.WriteLine("RB Compensation =" + redBlue_nud.Value.ToString());
                st.WriteLine("GR Compensation =" + greenRed_nud.Value.ToString());
                st.WriteLine("GG Compensation =" + greenGreen_nud.Value.ToString());
                st.WriteLine("GB Compensation =" + greenBlue_nud.Value.ToString());
                st.WriteLine("BR Compensation =" + blueRed_nud.Value.ToString());
                st.WriteLine("BG Compensation =" + blueGreen_nud.Value.ToString());
                st.WriteLine("BB Compensation =" + blueBlue_nud.Value.ToString());
                st.WriteLine("Red Channel LUT ");
                st.WriteLine("Sine Factor =" + redChannelSineFactor_nud.Value.ToString());
                st.WriteLine("LUT Interval 1 =" + redChannelLutInterval1_nud.Value.ToString());
                st.WriteLine("LUT Interval 2 =" + redChannelLutInterval2_nud.Value.ToString());
                st.WriteLine("Sine Factor =" + redChannelLutOffset_nud.Value.ToString());

                st.WriteLine("Green Channel LUT ");
                st.WriteLine("Sine Factor =" + greenChannelSineFactor_nud.Value.ToString());
                st.WriteLine("LUT Interval 1 =" + greenChannelLutInterval1_nud.Value.ToString());
                st.WriteLine("LUT Interval 2 =" + greenChannelLutInterval2_nud.Value.ToString());
                st.WriteLine("Sine Factor =" + greenChannelLutOffset_nud.Value.ToString());

                st.WriteLine("Blue Channel LUT ");
                st.WriteLine("Sine Factor =" + blueChannelSineFactor_nud.Value.ToString());
                st.WriteLine("LUT Interval 1 =" + blueChannelLutInterval1_nud.Value.ToString());
                st.WriteLine("LUT Interval 2 =" + blueChannelLutInterval2_nud.Value.ToString());
                st.WriteLine("Sine Factor =" + blueChannelLutOffset_nud.Value.ToString());
                st.Close();
                st.Dispose();
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
        }


        int prevExp = 0;
        int prevTemp = 6500;
        int prevTint = 1000;
        bool val;

        private void expVal_nud_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if(!isdefaultExp)
                {
                    // IntucamHelper.UpdateExposureGainFromTable(FlashExposure);
                    
                    ivl_camera.camPropsHelper._Settings.CameraSettings.LiveExposure = ((uint)expVal_nud.Value);
                    ivl_camera.SetLiveExposure();
                    //  ivl_camera.UpdateExposureGainFromTable((int)expVal_nud.Value);
                    
                    {

                        ConfigVariables.CurrentSettings.CameraSettings._Exposure.val= expVal_nud.Value.ToString();
                    }
                    UpdateConfigSettings();

                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            

        }
        int PrevGain = 0;
        private void gainVal_nud_ValueChanged(object sender, EventArgs e)
        {
            try
            {

                ivl_camera.camPropsHelper._Settings.CameraSettings.LiveGain = ((ushort)gainVal_nud.Value);
                
                    ConfigVariables.CurrentSettings.CameraSettings._LiveDigitalGain.val= gainVal_nud.Value.ToString();
                UpdateConfigSettings();

            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
        }

        private void hueVal_nud_ValueChanged(object sender, EventArgs e)
        {
            //prevHue = (int)hueVal_nud.Value;
            //ivl_camera.SetHue((int)hueVal_nud.Value);

        }

        private void saturationVal_nud_ValueChanged(object sender, EventArgs e)
        {
            //prevSat = (int)saturationVal_nud.Value;
            //ivl_camera.SetSaturation((int)saturationVal_nud.Value);

        }

        private void brightnessVal_nud_ValueChanged(object sender, EventArgs e)
        {
            

                ConfigVariables.CurrentSettings.PostProcessingSettings.BrightnessContrastSettings._BrightnessVal.val = brightness_nud.Value.ToString();
            UpdateConfigSettings();
            {
                ivl_camera.camPropsHelper._Settings.PostProcessingSettings.brightnessContrastSettings.brightnessValue = (int)brightness_nud.Value;
            }
        }

        private void contrastVal_nud_ValueChanged(object sender, EventArgs e)
        {
            

                ConfigVariables.CurrentSettings.PostProcessingSettings.BrightnessContrastSettings._ContrastVAl.val = contrast_nud.Value.ToString();
            UpdateConfigSettings();
            {
                ivl_camera.camPropsHelper._Settings.PostProcessingSettings.brightnessContrastSettings.contrastValue = (int)contrast_nud.Value;
            }

        }

        private void gammaVal_nud_ValueChanged(object sender, EventArgs e)
        {
            //ivl_camera.SetGamma((int)gammaVal_nud.Value);

        }

        private void temperatureVal_nud_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (EnableTemperatureTint_cbx.Checked)
                {
                    
                        ConfigVariables.CurrentSettings.CameraSettings._Temperature.val= temperature_tb.Value.ToString();
                    UpdateConfigSettings();
                    TemperatureVal_lbl.Text = temperature_tb.Value.ToString();
                    if (ivl_camera != null)
                    {
                        ivl_camera.camPropsHelper.SetTemperatureTint(temperature_tb.Value, tint_tb.Value);
                        int[] RGBGain = new int[3];
                        //ivl_camera.GetTemperatureTint(ref tempVal, ref tintVal);// ref RGBGain);
                        temperaturStatus_lbl.Text = "Temp: " + tempVal.ToString();
                        tintStatus_lbl.Text = "Temp: " + tintVal.ToString();

                    }
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            

        }

        DirectoryInfo saveFrameDinf;
        FileInfo[] saveFrameFinf;
        private void saveFrames_btn_Click(object sender, EventArgs e)
        {
            //  LogArgs arg = new LogArgs("Capture Button click Event Recieved");
            //IntucamBoardCommHelper.CaptureLogList.Add(arg);
            saveFrames();



        }
        int frameCnt = 1;
        private void previous_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveFrameDinf == null)
                    return;
                saveFrameFinf = saveFrameDinf.GetFiles();
                if (saveFrameFinf.Length < 0)
                    return;
                --frameCnt;
                if (frameCnt >= 0)
                {
                    display_pbx.Image = new Bitmap(saveFrameFinf[frameCnt].FullName);
                    this.Text = titleText + saveFrameFinf[frameCnt].FullName;
                }
                else
                {
                    frameCnt = saveFrameFinf.Length - 1;
                    display_pbx.Image = new Bitmap(saveFrameFinf[frameCnt].FullName);
                    this.Text = titleText + " " + saveFrameFinf[frameCnt].FullName;
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void next_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveFrameDinf == null)
                    return;
                saveFrameFinf = saveFrameDinf.GetFiles();
                if (saveFrameFinf.Length < 0)
                    return;

                ++frameCnt;
                if (frameCnt < saveFrameFinf.Length)
                {
                    display_pbx.Image = new Bitmap(saveFrameFinf[frameCnt].FullName);
                    this.Text = titleText + saveFrameFinf[frameCnt].FullName;

                }
                else
                {
                    frameCnt = 0;
                    display_pbx.Image = new Bitmap(saveFrameFinf[frameCnt].FullName);
                    this.Text = titleText + " " + saveFrameFinf[frameCnt].FullName;
                }

            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }
        FileInfo[] fArr;
        private void browse_btn_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                browse_btn.TabStop = true;

                if (ivl_camera != null && !string.IsNullOrEmpty(ivl_camera.camPropsHelper.RawImageSavedPath))
                {
                    
                    System.Diagnostics.Process.Start(ivl_camera.camPropsHelper.RawImageSavedPath);
                }
                else
                    System.Diagnostics.Process.Start(Path.GetDirectoryName(SaveDirectoryPath));

                //  ofd.InitialDirectory = SaveDirectoryPath;
                string fileName = "";
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            


        }
        bool isCapture = false;
        string fileDir = "";
        const int FLASH_IR_OFFSET = 10;
        private void startCapture()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (istrigger)
                {
                    triggerRecieved_pbx.Image = TriggerRecieved;
                    triggerRecieved_pbx.Refresh();
                }
                ivl_camera.camPropsHelper.ffaTimeStamp = ffaTimerStatus_lbl.Text;
                isCapture = true;
                saveFrames_btn.Text = "Resume";
                saveFrames_btn.Refresh();
                ivl_camera.camPropsHelper._Settings.CameraSettings.CaptureGain = ((ushort)flashGain_nud.Value);
                ivl_camera.camPropsHelper._Settings.CameraSettings.CaptureExposure = ((uint)flashExp_nud.Value);
                //switch (imagingMode)
                //{
                //    case INTUSOFT.Imaging.ImagingMode.Posterior_45:
                //        {
                //            ivl_camera.camPropsHelper._Settings.CameraSettings.
                //        }
                //}

                SaveSettingsLog();

                ivl_camera.StartCapture(istrigger);
                this.Cursor = Cursors.Default;
                overlay_pbx.Visible = false;

            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }
        private void ResumeLive()
        {
            try
            {
                isCapture = false;
                saveFrames_btn.Text = "Save Frames";
                this.Text = titleText;
                ivl_camera.camPropsHelper.EnableCameraColorMatrix(formCheckBox1.Checked);
                ivl_camera.camPropsHelper.EnableWhiteBalance(EnableTemperatureTint_cbx.Checked);
                ivl_camera.ResumeLive();
                ivl_camera.camPropsHelper._Settings.CameraSettings.LiveGain = ((ushort)gainVal_nud.Value);
                ivl_camera.camPropsHelper._Settings.CameraSettings.LiveExposure = ((uint)expVal_nud.Value);
                ivl_camera.SetLiveExposure();
                ivl_camera.TriggerOn();
                if (istrigger)
                {
                    triggerRecieved_pbx.Image = TriggerReset;
                    triggerRecieved_pbx.Refresh();
                }
                istrigger = false;
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
            return;
        }
        private void saveFrames()
        {
            try
            {
                if (!ivl_camera.IsCapturing)
                {
                    if (ivl_camera.isCameraOpen)
                    {
                        if (!isCapture)
                            startCapture();

                    }
                }
                else if (ivl_camera.IsCapturing)
                    ResumeLive();
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }



        FileInfo finf;
        string processedImgPath = string.Empty;
        List<Bitmap> saveImageList = new List<Bitmap>();
        private void formButtons1_Click(object sender, EventArgs e)
        {

            //OpenFileDialog f = new OpenFileDialog();
            //if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //    finf = new FileInfo(f.FileName);
            //else return;
            //fArr = finf.Directory.GetFiles("*.png");
            //foreach (var fi in fArr)
            //{
            //    calculateInt(fi.FullName);
            //    saveImageList.Add(new Bitmap(fi.FullName));
            //}
            //saveImageFramesList();
            imageSelection();

        }

        private void imageSelection()
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                Image<Bgr, byte> input = new Image<Bgr, byte>(10, 10);
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    input = new Image<Bgr, byte>(ofd.FileName);
                }
                input.ROI = new Rectangle(950, 750, 180, 180);
                input.SetZero();
                input.ROI = new Rectangle();
                input.ROI = new Rectangle(768, 512, 512, 512);
                display_pbx.Image = input.ToBitmap();
                double value = input[2].GetAverage().Intensity;
                double value1 = input[0].GetAverage().Intensity;
                MessageBox.Show(value.ToString() + "  and  " + value1.ToString());

            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            


        }
        string overlayFileName = "";
        private void BrowseOverlay_btn_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                DirectoryInfo dir = new DirectoryInfo(Application.ExecutablePath);
                dir = new DirectoryInfo(dir.Parent.FullName);
                DirectoryInfo[] dirs = dir.GetDirectories();
                string strPath = "";
                foreach (var item in dirs)
                {
                    if (item.Name.ToLower().Contains("overlay"))
                    {
                        strPath = item.FullName;
                        break;
                    }
                }
                ofd.InitialDirectory = strPath;
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    overlayFileName = ofd.FileName;
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            

        }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void applyAGain()
        {
            int[] rgb = new int[3];
            int temp = 0;
            int tint = 0;
            //ivl_camera.SetRGB(rgb, out temp, out tint);

        }
        private void redGain_tb_Scroll(object sender, EventArgs e)
        {
            applyAGain();
        }

        private void greenGain_tb_Scroll(object sender, EventArgs e)
        {
            applyAGain();

        }

        private void blueGain_tb_Scroll(object sender, EventArgs e)
        {
            applyAGain();
        }

        DirectoryInfo dir;
        DirectoryInfo vidDir;
        private void makeAvi_btn_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                string videoName = "";
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    videoName = ofd.FileName;
                }
                else
                    return;
                System.IO.FileInfo finf = new FileInfo(videoName);
                string[] strArr = finf.Name.Split('.');
                vidDir = new DirectoryInfo(finf.Directory.FullName + Path.DirectorySeparatorChar + strArr[0]);

                if (!Directory.Exists(finf.Directory.FullName + Path.DirectorySeparatorChar + strArr[0]))
                    vidDir = Directory.CreateDirectory(finf.Directory.FullName + Path.DirectorySeparatorChar + strArr[0]);


                Image<Bgr, byte> inp = new Image<Bgr, byte>(2048, 1536);
                //Emgu.CV.ICapture cap = ;
                //int count = 0;
                //while ((inp = cap.QueryFrame()) != null)
                //{
                //    inp.Save(vidDir.FullName + Path.DirectorySeparatorChar + count.ToString() + ".png");

                //    count++;
                //}
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void batchPC_btn_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog ofd = new FolderBrowserDialog();
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    vidDir = new DirectoryInfo(ofd.SelectedPath);
                else
                    return;
                FileInfo[] finfArr = vidDir.GetFiles("*.png").OrderBy(p => p.CreationTime).ToArray();
                int count = 0;
                StreamWriter edgeStrength = new StreamWriter(vidDir.FullName + Path.DirectorySeparatorChar + "EdgeStrength.csv");
                double[] entropyR = new double[4];
                double[] entropyG = new double[4];
                double[] entropyB = new double[4];

                double[] brightR = new double[4];
                double[] brightG = new double[4];
                double[] brightB = new double[4];

                double[] contrastR = new double[4];
                double[] contrastG = new double[4];
                double[] contrastB = new double[4];

                double[] edgeR = new double[4];
                double[] edgeG = new double[4];
                double[] edgeB = new double[4];
                edgeStrength.WriteLine("FileName" + "," + "EdgeZone1R" + ","
            + "EdgeZone2R" + "," + "EdgeZone3R" + "," + "EdgeZone4R" + "," + "EdgeZone1G" + ","
            + "EdgeZone2G" + "," + "EdgeZone3G" + "," + "EdgeZone4G" + "," + "EdgeZone1B" + ","
            + "EdgeZone2B" + "," + "EdgeZone3B" + "," + "EdgeZone4B" + "," + "ContrastZone1R" + ","
            + "ContrastZone2R" + "," + "ContrastZone3R" + "," + "ContrastZone4R" + "," + "ContrastZone1G" + ","
            + "ContrastZone2G" + "," + "ContrastZone3G" + "," + "ContrastZone4G" + "," + "ContrastZone1B" + ","
            + "ContrastZone2B" + "," + "ContrastZone3B" + "," + "ContrastZone4B" + "," + "BrightnessZone1R" + ","
            + "BrightnessZone2R" + "," + "BrightnessZone3R" + "," + "BrightnessZone4R" + "," + "BrightnessZone1G" + ","
            + "BrightnessZone2G" + "," + "BrightnessZone3G" + "," + "BrightnessZone4G" + "," + "BrightnessZone1B" + ","
            + "BrightnessZone2B" + "," + "BrightnessZone3B" + "," + "BrightnessZone4B" + "," + "EntropyZone1R" + ","
            + "EntropyZone2R" + "," + "EntropyZone3R" + "," + "EntropyZone4R" + "," + "EntropyZone1G" + ","
            + "EntropyZone2G" + "," + "EntropyZone3G" + "," + "EntropyZone4G" + "," + "EntropyZone1B" + ","
            + "EntropyZone2B" + "," + "EntropyZone3B" + "," + "EntropyZone4B");
                foreach (FileInfo finf in finfArr)
                {
                    //OpenFileDialog ofd = new OpenFileDialog();
                    //FileInfo finf = new FileInfo(@"G:\FocusVideo\sabari\201.png");
                    //if (ofd.ShowDialog() == DialogResult.OK)
                    //{
                    //    finf = new FileInfo(ofd.FileName);
                    //}
                    //else
                    //    return;
                    // FileInfo finf = new FileInfo(@"G:\FocusVideo\sabari\201.png");
                    Image<Bgr, byte> inp_img = new Image<Bgr, byte>(finf.FullName);
                    Bitmap inp = new Bitmap(finf.FullName);


                    BitmapData bData = inp.LockBits(new Rectangle(0, 0, inp.Width, inp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                    //  unsafe
                    {


                        //AssistedFocus_FrameProperties(bData.Scan0,ref entropyR[0],ref entropyB[0],ref entropyG[0],
                        //  ref contrastR[0],ref brightR[0],ref edgeR[0],ref contrastG[0],ref brightG[0],ref edgeG[0],ref contrastB[0],ref brightB[0],ref edgeB[0]);

                    }

                    inp.UnlockBits(bData);
                    edgeStrength.WriteLine(finf.Name + "," + edgeR[0] + ","
                        + edgeR[1] + "," + edgeR[2] + "," + edgeR[3] + "," + edgeG[0] + ","
                        + edgeG[1] + "," + edgeG[2] + "," + edgeG[3] + "," + edgeB[0] + ","
                        + edgeB[1] + "," + edgeB[2] + "," + edgeB[3] + "," + contrastR[0] + ","
                        + contrastR[1] + "," + contrastR[2] + "," + contrastR[3] + "," + contrastG[0] + ","
                        + contrastG[1] + "," + contrastG[2] + "," + contrastG[3] + "," + contrastB[0] + ","
                        + contrastB[1] + "," + contrastB[2] + "," + contrastB[3] + "," + brightR[0] + ","
                        + brightR[1] + "," + brightR[2] + "," + brightR[3] + "," + brightG[0] + ","
                        + brightG[1] + "," + brightG[2] + "," + brightG[3] + "," + brightB[0] + ","
                        + brightB[1] + "," + brightB[2] + "," + brightB[3] + "," + entropyR[0] + ","
                        + entropyR[1] + "," + entropyR[2] + "," + entropyR[3] + "," + entropyG[0] + ","
                        + entropyG[1] + "," + entropyG[2] + "," + entropyG[3] + "," + entropyB[0] + ","
                        + entropyB[1] + "," + entropyB[2] + "," + entropyB[3]);
                    //frameCnt_lbl.Text = count.ToString();
                    //frameCnt_lbl.Refresh();
                    inp.Dispose();
                    inp_img.Dispose();
                    count++;

                }
                edgeStrength.Close();
                edgeStrength.Dispose();
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void unsharpMask_cbx_CheckedChanged(object sender, EventArgs e)
        {
            //if (unsharpMask_cbx.Checked)
            //{
            //    if (saveFrames_btn.Text == "Resume")
            //    {
            //        Bitmap bm = displayImg.Clone() as Bitmap;// display_pbx.Image as Bitmap;
            //        //ivl_camera.unsharpMask(ref bm);
            //        display_pbx.Image = bm;
            //       // bm.Dispose();
            //    }

            //}
            //else
            //    if (saveFrames_btn.Text == "Resume")
            //    display_pbx.Image = displayImg.Clone() as Bitmap;

        }

        private void greenIRLive_cbx_CheckedChanged(object sender, EventArgs e)
        {
            //if (saveImages_btn.Text != "Resume")
            {
                //if (greenIRLive_cbx.Checked)
                //{
                //    ivl_camera.
                //    //ivl_camera.GreenLive = true;
                //}
                //else
                //    ivl_camera.GreenLive = false;
                

                    //switch (mode_cmbx.Text)
                    //{
                    //    case "FFA":
                    //        {
                    //            _settingsConfig.FFA_Settings.UI_Settings.isGreenLive = greenIRLive_cbx.Checked;
                    //            break;
                    //        }
                    //    case "Posterior":
                    //        {
                    //            _settingsConfig.Lite_Settings.UI_Settings.isGreenLive = greenIRLive_cbx.Checked;
                    //            break;
                    //        }
                    //    case "45":
                    //        {
                    //            _settingsConfig.FortyFive_Settings.UI_Settings.isGreenLive = greenIRLive_cbx.Checked;
                    //            break;
                    //        }
                    //}
                ConfigVariables.CurrentSettings.CameraSettings.EnableMonoChromeMode.val = greenIRLive_cbx.Checked.ToString();
                UpdateConfigSettings();
                if (ivl_camera != null)
                    ivl_camera.camPropsHelper.SetMonoChromeMode(greenIRLive_cbx.Checked);
            }
        }

        private void ComputeFocus_btn_Click(object sender, EventArgs e)
        {
        }
        int stepsForward = 0, stepsBackward = 0;

        private void MotorForward_btn_Click(object sender, EventArgs e)
        {
            //IntucamBoardCommHelper.MotorForward(Convert.ToByte(motorSteps_nud.Value));
            //stepsForward++;
            //stepsForward_lbl.Text = stepsForward.ToString();
        }

        private void MotorBackward_btn_Click(object sender, EventArgs e)
        {
            //IntucamBoardCommHelper.MotorBackward(Convert.ToByte(motorSteps_nud.Value));
            //stepsBackward++;
            //stepsBackward_lbl.Text = stepsBackward.ToString();
        }
        List<AssistedFocus> afList;
        AssistedFocus af;
        private void startVideo_btn_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ComputeAutoFocus CAF = new ComputeAutoFocus();
                // ofd.Filter = "Avi files|*.avi";
                string fileName = "";
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    fileName = ofd.FileName;
                else
                    return;
                FileInfo finf = new FileInfo(fileName);
                FileInfo[] finfArr = finf.Directory.GetFiles("*.png").OrderBy(x => x.CreationTime).ToArray();
                // Capture cap = new Capture(fileName);
                int count = 0;
                afList = new List<AssistedFocus>();
                // while ((bgrImg = cap.QueryFrame()) != null)
                List<int> focusValues = new List<int>();
                foreach (FileInfo f in finfArr)
                {
                    Bitmap bm1 = new Bitmap(f.FullName);
                    // CAF = new ComputeAutoFocus();
                    af = CAF.ComputeFocus(bm1);
                    bm1.Dispose();
                    //Focus_lbl.Text = afList.Count.ToString();
                    //Focus_lbl.Refresh();

                    if (afList.Count < 20)
                    {
                        afList.Add(af);
                        focusValues.Add((int)(af.sumFocus / 1000));
                    }
                    else
                    {
                        //drawGraph(focusValues);
                        afList.RemoveRange(0, afList.Count - (afList.Count - 5));
                        focusValues.RemoveRange(0, afList.Count - (afList.Count - 5));
                        afList.Add(af);
                        focusValues.Add((int)(af.sumFocus / 1000));
                        // GC.Collect();

                    }
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }
        Bitmap bm = new Bitmap(40, 200);
        private void drawGraph(List<int> values)
        {
            //// pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            // if (bm != null)
            //     bm.Dispose();
            //  bm = new Bitmap(100,values.Max()+10);

            // Graphics g = Graphics.FromImage(bm);
            // g.FillRectangle(Brushes.Black,new Rectangle(0,0,bm.Width,bm.Height));
            //for (int i = 1; i < values.Count; i++)
            // {
            //     double val1 = values[i];
            //     double val2 = values[i - 1];

            //     g.DrawLine(new Pen(Color.AntiqueWhite, 0.1f), new Point( i-1 , bm.Height -(int)val2), new Point(i,bm.Height-(int)val1));
            // }
            // pictureBox1.Image = bm;
            // pictureBox1.Refresh();
        }

        private void LoopMotor_btn_Click(object sender, EventArgs e)
        {
            //IntucamBoardCommHelper.MotorFastForward(Convert.ToByte(motorSteps_nud.Value));
            ////IntucamBoardCommHelper.MotorLoop();
            //System.Threading.Thread.Sleep(2000);  // 20ms is the time taken to move the moter by 1 step.          
            //IntucamBoardCommHelper.MotorFastBackward(Convert.ToByte(motorSteps_nud.Value));

        }

        private void redGain_tb_Scroll_1(object sender, EventArgs e)
        {
            //redGainVal_lbl.Text = redGain_tb.Value.ToString();
            //applyAGain();

        }

        private void greenGain_tb_Scroll_1(object sender, EventArgs e)
        {
            //greenGainVal_lbl.Text = greenGain_tb.Value.ToString();
            //applyAGain();
        }

        private void blueGain_tb_Scroll_1(object sender, EventArgs e)
        {
            //BlueGainVal_lbl.Text = blueGain_tb.Value.ToString();
            //applyAGain();
        }


        private void temperatureVal_nud_ValueChanged_1(object sender, EventArgs e)
        {
            //ivl_camera.SetTemperatureTint((int)temperatureVal_nud.Value, (int)tintVal_nud.Value);
            //prevTemp =(int) temperatureVal_nud.Value;
            //prevTint = (int)tintVal_nud.Value;
            //temperatureVal_lbl.Text = tint_tb.Value.ToString();
            int[] RGBGain = new int[3];
            // ivl_camera.SetTemperatureTint((int)temperatureVal_nud.Value, (int)tintVal_nud.Value);
            ivl_camera.camPropsHelper.GetRGBGain(RGBGain);

        }

        private void tintVal_nud_ValueChanged_1(object sender, EventArgs e)
        {
            //prevTemp = (int)temperatureVal_nud.Value;
            //prevTint = (int)tintVal_nud.Value;
            //ivl_camera.SetTemperatureTint((int)temperatureVal_nud.Value,(int) tintVal_nud.Value);
            //tintVal1_lbl.Text = tint_tb.Value.ToString();
            int[] RGBGain = new int[3];
            //ivl_camera.SetTemperatureTint((int)temperatureVal_nud.Value, (int)tintVal_nud.Value);
            //ivl_camera.GetRGBGain(prevTemp, prevTint, ref RGBGain);


        }
        System.Diagnostics.Stopwatch autoFocusWatch;


        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                AutoFocusWriter = new StreamWriter(@"Logs\Autofocus_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".csv");
                AutoFocusWriter.WriteLine("Peaks");
                autoFocusWatch = new System.Diagnostics.Stopwatch();
                autoFocusWatch.Start();
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            // IntucamBoardCommHelper.MotorCapturePosition(iterCommandCnt);
            
            // MotorPosition_lbl.Text = IntucamBoardCommHelper.convertInt.ToString();

            //if (!IntucamBoardCommHelper.isMotorForward)
            //{
            //    IntucamBoardCommHelper.b_MFFR_Done = false;
            //    IntucamBoardCommHelper.MotorFastForward(Convert.ToByte(4), iterCommandCnt);
            //    while (IntucamBoardCommHelper.b_MFFR_Done == false)
            //    {
            //    }
            //    IntucamBoardCommHelper.b_MFFR_Done = false;

            //} 
            //if (ivl_camera.isForward)
            //{
            //    //forwardMotion_lbl.Text = "Forward" ;
            //    LoopCompleted = true;
            //}
            //else
            //{
            //    //forwardMotion_lbl.Text = "Backward";
            //    LoopCompleted = false;
            //}
            //forwardMotion_lbl.Text += " "+IntucamBoardCommHelper.convertInt;
        }

        private void clearLabel_btn_Click(object sender, EventArgs e)
        {
            //stepsBackward_lbl.Text = "0";
            //stepsForward_lbl.Text = "0";
            //fastForward_lbl.Text = "0";
            //fastBackward_lbl.Text = "0";
            stepsForward = 0;
            stepsBackward = 0;
            stepsFastForward = 0;
            stepsFastBackward = 0;
        }

        int stepsFastForward = 0;
        int stepsFastBackward = 0;

        private void motorFastForward_btn_Click_1(object sender, EventArgs e)
        {
            //IntucamBoardCommHelper.MotorFastForward(Convert.ToByte(motorSteps_nud.Value));
            //stepsFastForward ++;

            //fastForward_lbl.Text =( (int)(motorSteps_nud.Value) * (int) stepsFastForward).ToString();
        }

        private void MotorFastBackward_btn_Click(object sender, EventArgs e)
        {
            //IntucamBoardCommHelper.MotorFastBackward(Convert.ToByte(motorSteps_nud.Value));
            //stepsFastBackward++;

            //fastBackward_lbl.Text = ((int)(motorSteps_nud.Value) * (int)stepsFastBackward).ToString();

        }
        Bitmap colorBm;
        string fileNameMask = "";
        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                    fileNameMask = ofd.FileName;
                else
                    return;
                if (colorBm != null)
                    colorBm.Dispose();

                colorBm = new Bitmap(fileNameMask);
                overlay_pbx.BackColor = Color.Transparent;
                overlay_pbx.Dock = DockStyle.Fill;
                overlay_pbx.Parent = display_pbx;
                overlay_pbx.Visible = true;
                drawMaskRectangle();

                display_pbx.Image = colorBm;

            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            

            //if (colorBm != null)
            //    colorBm.Dispose();

        }
        Bitmap maskBitmap;


        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            drawMaskRectangle();
            //if (colorBm != null)
            //    colorBm.Dispose();
            //colorBm = new Bitmap(fileNameMask);

            //ApplyMask(ref colorBm);
            //display_pbx.Image = colorBm;
            //display_pbx.Refresh();

        }
        Bitmap maskRect;
        private void drawMaskRectangle()
        {
            try
            {
                if (maskRect == null)
                    maskRect = new Bitmap(colorBm.Width, colorBm.Height);

                Graphics g = Graphics.FromImage(maskRect);
                g.FillRectangle(Brushes.Black, new Rectangle(0, 0, maskRect.Width, maskRect.Height));
                g.DrawEllipse(new Pen(Color.Red, 5f), new Rectangle((int)maskX_nud.Value - (int)LiveMaskWidth_nud.Value / 2, (int)maskY_nud.Value - (int)LiveMaskHeight_nud.Value / 2, (int)LiveMaskWidth_nud.Value, (int)LiveMaskHeight_nud.Value));
                maskRect.MakeTransparent(Color.Black);
                overlay_pbx.Image = maskRect;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

            drawMaskRectangle();
            //ApplyMask(ref colorBm);
            display_pbx.Image = colorBm;
            //display_pbx.Refresh();

        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            drawMaskRectangle();
            //if (colorBm != null)
            //    colorBm.Dispose();
            //colorBm = new Bitmap(fileNameMask);
            //ApplyMask(ref colorBm);
            //display_pbx.Image = colorBm;
            //display_pbx.Refresh();
        }
        private void ShiftImage(ref Bitmap bm, int factorX, int factorY)
        {
            try
            {
                // Image<Hsv, byte> inp = new Image<Hsv, byte>(bm);
                Image<Bgr, byte> inp = new Image<Bgr, byte>(bm);
                //inp[0] = inp[0].SmoothMedian(9);
                //inp[1] = inp[1].SmoothMedian(7);
                //inp[2] = inp[2].SmoothMedian(7);

                Image<Gray, byte> blueChan = inp[0].Copy();
                Image<Gray, byte> greenChan = inp[1].Copy();
                Image<Gray, byte> redChan = inp[2].Copy();

                // redChan = redChan.Add(new Gray(5));
                //  greenChan = greenChan.Add(new Gray(5));
                //blueChan = blueChan.Add(new Gray(25));

                // take central portion of the image
                blueChan.ROI = new Rectangle(factorX, factorY, blueChan.Width - (2 * factorX), blueChan.Height - (2 * factorY));
                Image<Gray, byte> tempImg = new Image<Gray, byte>(blueChan.Width, blueChan.Height);
                //tempImg.ROI = new Rectangle(0, 0, 2048 - 10, 1536 - 5);
                CvInvoke.cvCopy(blueChan, tempImg, IntPtr.Zero);

                // stretch the blue image by 24,18
                blueChan.ROI = new Rectangle();
                blueChan = tempImg.Resize(blueChan.Width, blueChan.Height, Emgu.CV.CvEnum.Inter.Cubic);


                // shrink red channel image
                tempImg = redChan.Resize(blueChan.Width - 2 * factorX, blueChan.Height - 2 * factorY, Emgu.CV.CvEnum.Inter.Cubic);

                Image<Gray, byte> tempImg2 = new Image<Gray, byte>(blueChan.Width, blueChan.Height);
                tempImg2.ROI = new Rectangle(factorX, factorY, blueChan.Width - 2 * factorX, blueChan.Height - 2 * factorY);
                CvInvoke.cvCopy(tempImg, tempImg2, IntPtr.Zero);

                tempImg2.ROI = new Rectangle();
                Emgu.CV.Util.VectorOfMat vMat = new Emgu.CV.Util.VectorOfMat(new Mat[] {blueChan.Mat, greenChan.Mat, tempImg2.Mat });

                CvInvoke.Merge(vMat, inp);

                // redChan._GammaCorrect(.7);
                //greenChan._GammaCorrect(.85);
                //greenChan._EqualizeHist();
                //greenChan._GammaCorrect(.85);
                // blueChan._GammaCorrect(.9);
                //CvInvoke.cvNormalize(blueChan.Ptr, blueChan.Ptr, 0, 255, Emgu.CV.CvEnum.NORM_TYPE.CV_MINMAX, IntPtr.Zero);
                //CvInvoke.cvNormalize(greenChan.Ptr, greenChan.Ptr, 0, 255, Emgu.CV.CvEnum.NORM_TYPE.CV_MINMAX, IntPtr.Zero);

                //    CvInvoke.cvNormalize(redChan.Ptr, redChan.Ptr, 0, 255, Emgu.CV.CvEnum.NORM_TYPE.CV_MINMAX,IntPtr.Zero);
                // Image<Hsv, byte> inp1 = inp.Convert<Hsv, byte>();
                //  CvInvoke.cvNormalize(inp1[1].Ptr, inp1[1].Ptr, 0, 255, Emgu.CV.CvEnum.NORM_TYPE.CV_RELATIVE, IntPtr.Zero);
                //blueChan = inp1[0].Copy();
                //greenChan = inp1[1].Copy();
                //redChan = inp1[2].Copy();

                //redChan._GammaCorrect(0.7);
                //greenChan._GammaCorrect(0.5);
                ////blueChan._GammaCorrect(0.5);
                //CvInvoke.cvMerge(blueChan, greenChan, redChan, IntPtr.Zero, inp1);

                bm = inp.ToBitmap();
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }
        private void noiseRemoval(ref Bitmap bm)
        {

            // Image<Hsv, byte> inp = new Image<Hsv, byte>(bm);
            //  Image<Bgr, byte> inp = new Image<Bgr, byte>(bm);
            //  //inp[0] = inp[0].SmoothMedian(9);
            //  //inp[1] = inp[1].SmoothMedian(7);
            //  //inp[2] = inp[2].SmoothMedian(7);

            //  Image<Gray, byte> blueChan = inp[0].Copy();
            //  Image<Gray, byte> greenChan = inp[1].Copy();
            //  Image<Gray, byte> redChan = inp[2].Copy();

            //  // redChan = redChan.Add(new Gray(5));
            ////  greenChan = greenChan.Add(new Gray(5));
            //  //blueChan = blueChan.Add(new Gray(25));

            //  // take central portion of the image
            //  blueChan.ROI = new Rectangle(12, 9, blueChan.Width-24, blueChan.Height-18);
            //  Image<Gray, byte> tempImg = new Image<Gray, byte>(2048 - 24, 1536 - 18);
            //  //tempImg.ROI = new Rectangle(0, 0, 2048 - 10, 1536 - 5);
            //  CvInvoke.cvCopy(blueChan,tempImg , IntPtr.Zero);

            //  // stretch the blue image by 24,18
            //  blueChan.ROI = new Rectangle();
            //  blueChan = tempImg.Resize(2048, 1536, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);


            //  // shrink red channel image
            //  tempImg = redChan.Resize(2048 - 24, 1536 - 18, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

            //  Image<Gray, byte> tempImg2 = new Image<Gray, byte>(2048, 1536);
            //  tempImg2.ROI = new Rectangle(12, 9, 2048 - 24, 1536 - 18);
            //  CvInvoke.cvCopy(tempImg, tempImg2, IntPtr.Zero);

            //  tempImg2.ROI = new Rectangle();

            //  CvInvoke.cvMerge(blueChan, greenChan, tempImg2, IntPtr.Zero, inp);

            // redChan._GammaCorrect(.7);
            //greenChan._GammaCorrect(.85);
            //greenChan._EqualizeHist();
            //greenChan._GammaCorrect(.85);
            // blueChan._GammaCorrect(.9);
            //CvInvoke.cvNormalize(blueChan.Ptr, blueChan.Ptr, 0, 255, Emgu.CV.CvEnum.NORM_TYPE.CV_MINMAX, IntPtr.Zero);
            //CvInvoke.cvNormalize(greenChan.Ptr, greenChan.Ptr, 0, 255, Emgu.CV.CvEnum.NORM_TYPE.CV_MINMAX, IntPtr.Zero);

            //    CvInvoke.cvNormalize(redChan.Ptr, redChan.Ptr, 0, 255, Emgu.CV.CvEnum.NORM_TYPE.CV_MINMAX,IntPtr.Zero);
            // Image<Hsv, byte> inp1 = inp.Convert<Hsv, byte>();
            //  CvInvoke.cvNormalize(inp1[1].Ptr, inp1[1].Ptr, 0, 255, Emgu.CV.CvEnum.NORM_TYPE.CV_RELATIVE, IntPtr.Zero);
            //blueChan = inp1[0].Copy();
            //greenChan = inp1[1].Copy();
            //redChan = inp1[2].Copy();

            //redChan._GammaCorrect(0.7);
            //greenChan._GammaCorrect(0.5);
            ////blueChan._GammaCorrect(0.5);
            //CvInvoke.cvMerge(blueChan, greenChan, redChan, IntPtr.Zero, inp1);
            //Image<Rgb, byte> inp = new Image<Rgb, byte>(bm);
            //Applycolorcorrection(ref bm);
            //bm = inp.ToBitmap();

        }
        byte[] raw_Data;
        private void PopulateImageData()
        {
            try
            {
                Image<Gray, byte> inp_Gr = new Image<Gray, byte>(1802, 1538);
                Image<Gray, byte> inp_R = new Image<Gray, byte>(1802, 1538);
                Image<Gray, byte> inp_Gb = new Image<Gray, byte>(1802, 1538);
                Image<Gray, byte> inp_B = new Image<Gray, byte>(1802, 1538);

                inp_B.SetZero();
                inp_Gb.SetZero();
                inp_Gr.SetZero();
                inp_R.SetZero();
                inp_Gr.ROI = new Rectangle(1, 1, 1800, 1536);
                inp_R.ROI = new Rectangle(1, 1, 1800, 1536);
                inp_Gb.ROI = new Rectangle(1, 1, 1800, 1536);
                inp_B.ROI = new Rectangle(1, 1, 1800, 1536);
                int k = 0, i = 0;
                for (k = 0, i = 0; i < 1536; i++)
                {
                    k = 0;
                    for (int j = 0; j < 1800; j = j + 2)
                    {
                        if (i % 2 == 0)
                        {
                            int m = k;
                            inp_Gr.Data[i, m, 0] = (byte)raw_Data[k++];
                            m = k;
                            inp_R.Data[i, m, 0] = (byte)raw_Data[k++];
                        }
                        else
                        {
                            int m = k;
                            inp_Gb.Data[i, m, 0] = raw_Data[k++];
                            m = k;
                            inp_B.Data[i, m, 0] = raw_Data[k++];
                        }
                    }
                }
                inp_Gr.ROI = new Rectangle();
                inp_R.ROI = new Rectangle();
                inp_Gb.ROI = new Rectangle();
                inp_B.ROI = new Rectangle();
                Image<Gray, byte> tempR = new Image<Gray, byte>(inp_R.Width, inp_R.Height);
                Image<Gray, byte> tempB = new Image<Gray, byte>(inp_R.Width, inp_R.Height);
                Image<Gray, byte> tempGr = new Image<Gray, byte>(inp_R.Width, inp_R.Height);
                Image<Gray, byte> tempGb = new Image<Gray, byte>(inp_R.Width, inp_R.Height);
                Image<Gray, byte> RowImg = new Image<Gray, byte>(inp_R.Width, 1);
                Image<Gray, byte> ColImg = new Image<Gray, byte>(1, inp_R.Height);

                inp_Gr.ROI = new Rectangle(0, inp_R.Height - 3, inp_R.Width, 1);
                RowImg = inp_Gr.Copy();
                inp_Gr.ROI = new Rectangle();
                inp_Gr.ROI = new Rectangle(0, inp_R.Height - 1, inp_R.Width, 1);

                CvInvoke.cvCopy(RowImg, inp_Gr, IntPtr.Zero);
                inp_Gr.ROI = new Rectangle();

                inp_R.ROI = new Rectangle(0, inp_R.Height - 3, inp_R.Width, 1);
                RowImg = inp_R.Copy();
                inp_R.ROI = new Rectangle();
                inp_R.ROI = new Rectangle(0, inp_R.Height - 1, inp_R.Width, 1);

                CvInvoke.cvCopy(RowImg, inp_R, IntPtr.Zero);
                inp_R.ROI = new Rectangle();

                inp_Gb.ROI = new Rectangle(0, 2, inp_Gb.Width, 1);
                RowImg = inp_Gb.Copy();
                inp_Gb.ROI = new Rectangle();
                inp_Gb.ROI = new Rectangle(0, 0, inp_Gb.Width, 1);
                CvInvoke.cvCopy(RowImg, inp_Gb, IntPtr.Zero);
                inp_Gb.ROI = new Rectangle();

                inp_B.ROI = new Rectangle(0, 2, inp_Gb.Width, 1);
                RowImg = inp_B.Copy();
                inp_B.ROI = new Rectangle();
                inp_B.ROI = new Rectangle(0, 0, inp_Gb.Width, 1);

                CvInvoke.cvCopy(RowImg, inp_B, IntPtr.Zero);
                inp_B.ROI = new Rectangle();

                inp_Gr.ROI = new Rectangle(inp_R.Width - 3, 0, 1, inp_R.Height);
                ColImg = inp_Gr.Copy();
                inp_Gr.ROI = new Rectangle();
                inp_Gr.ROI = new Rectangle(inp_R.Width - 1, 0, 1, inp_Gb.Height);

                CvInvoke.cvCopy(ColImg, inp_Gr, IntPtr.Zero);
                inp_Gr.ROI = new Rectangle();

                inp_R.ROI = new Rectangle(2, 0, 1, inp_R.Height);
                ColImg = inp_R.Copy();
                inp_R.ROI = new Rectangle();
                inp_R.ROI = new Rectangle(0, 0, 1, inp_Gb.Height);

                CvInvoke.cvCopy(ColImg, inp_R, IntPtr.Zero);
                inp_R.ROI = new Rectangle();

                inp_Gb.ROI = new Rectangle(2, 0, 1, inp_Gb.Height);
                ColImg = inp_Gb.Copy();
                inp_Gb.ROI = new Rectangle();
                inp_Gb.ROI = new Rectangle(0, 0, 1, inp_Gb.Height);

                CvInvoke.cvCopy(ColImg, inp_Gb, IntPtr.Zero);
                inp_Gb.ROI = new Rectangle();

                inp_B.ROI = new Rectangle(inp_R.Width - 3, 0, 1, inp_R.Height);
                ColImg = inp_B.Copy();
                inp_B.ROI = new Rectangle();
                inp_B.ROI = new Rectangle(inp_R.Height - 1, 0, 1, inp_R.Height);
                CvInvoke.cvCopy(ColImg, inp_B, IntPtr.Zero);
                inp_B.ROI = new Rectangle();
                for (int l = 0; l < inp_R.Height - 3; l++)
                {
                    if (l % 2 == 0)
                    {
                        for (int n = 0; n < inp_B.Width - 3; n++)
                        {
                            inp_B.Data[l + 1, n, 0] = (byte)((float)((float)inp_B.Data[l, n, 0] + (float)inp_B.Data[l + 2, n, 0]) / 2);
                            inp_Gb.Data[l + 1, n, 0] = (byte)((float)((float)inp_Gb.Data[l, n, 0] + (float)inp_Gb.Data[l + 2, n, 0]) / 2);
                        }
                    }
                    else
                    {
                        for (int n = 0; n < inp_B.Width - 3; n++)
                        {
                            inp_R.Data[l + 1, n, 0] = (byte)((float)((float)inp_R.Data[l, n, 0] + (float)inp_R.Data[l + 2, n, 0]) / 2);
                            inp_Gr.Data[l + 1, n, 0] = (byte)((float)((float)inp_Gr.Data[l, n, 0] + (float)inp_Gr.Data[l + 2, n, 0]) / 2);
                        }
                    }
                }

                for (int l = 0; l < inp_R.Width - 3; l++)
                {
                    if (l % 2 == 0)
                    {
                        for (int n = 0; n < inp_B.Height - 3; n++)
                        {
                            float val1 = (float)inp_R.Data[n, l, 0];
                            float val2 = (float)inp_R.Data[n + 2, l, 0];
                            inp_R.Data[n + 1, l, 0] = (byte)((float)((float)inp_R.Data[n, l, 0] + (float)inp_R.Data[n + 2, l, 0]) / 2);
                            inp_Gr.Data[n + 1, l, 0] = (byte)((float)((float)inp_Gr.Data[n, l, 0] + (float)inp_Gr.Data[n + 2, l, 0]) / 2);
                        }
                    }
                    else
                    {
                        for (int n = 0; n < inp_B.Height - 3; n++)
                        {
                            inp_B.Data[n + 1, l, 0] = (byte)((float)((float)inp_B.Data[n, l, 0] + (float)inp_B.Data[n + 2, l, 0]) / 2);
                            inp_Gb.Data[n + 1, l, 0] = (byte)((float)((float)inp_Gb.Data[n, l, 0] + (float)inp_Gb.Data[n + 2, l, 0]) / 2);
                        }

                    }
                }

                inp_Gr = inp_Gr.Add(inp_Gb);
                CvInvoke.cvConvertScale(inp_Gr, inp_Gr, 0.5, 0);
                Image<Bgr, byte> resultImg = new Image<Bgr, byte>(inp_Gr.Width - 2, inp_Gr.Height - 2);
                inp_Gr.ROI = new Rectangle(1, 1, 1800, 1536);
                inp_R.ROI = new Rectangle(1, 1, 1800, 1536);
                inp_Gb.ROI = new Rectangle(1, 1, 1800, 1536);
                inp_B.ROI = new Rectangle(1, 1, 1800, 1536);
                Mat[] inpArr = new Mat[]{inp_B.Mat, inp_Gr.Mat, inp_R.Mat};
                Emgu.CV.Util.VectorOfMat vMat = new Emgu.CV.Util.VectorOfMat(inpArr);
                CvInvoke.Merge(vMat, resultImg.Mat);
                resultImg.Save("resultImg.png");
                inp_Gr.Save("GR.png");
                inp_R.Save("R.png");
                inp_Gb.Save("Gb.png");
                inp_B.Save("B.png");
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void ApplyPostProcessing(ref Bitmap bm)
        {
           
            try
            {
                if (string.IsNullOrEmpty(fileNameMask))
                    return;
                finf = new FileInfo(fileNameMask);
                Bitmap tempbm = new Bitmap(finf.FullName);
                colorBm = new Bitmap(tempbm.Width, tempbm.Height, PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(colorBm);
                g.DrawImage(tempbm, new Rectangle(0, 0, tempbm.Width, tempbm.Height));
                tempbm.Dispose();
                string[] strArr = finf.Name.Split('.');
                List<int> listItems = new List<int>();
                List<PostProcessingStep> postProcessingStepList = new List<PostProcessingStep>();
                PostProcessingStep[] PP_Enum_values = Enum.GetValues(typeof(PostProcessingStep)) as PostProcessingStep[];
                for (int i = 0; i < this._items.Count; i++)
                {

                    int val = Convert.ToInt32(((HarrProgressBar)this.flowLayoutPanel1.Controls[i]).LeftText);
                    //PostProcessingStep pps = PostProcessingStep.ShiftImage; 
                    //var arr = pp_description_values.Where(x => x.Contains(((HarrProgressBar)this.flowLayoutPanel1.Controls[i]).)).ToList();
                    //Enum.TryParse( arr[0], out pps);
                    postProcessingStepList.Add(PP_Enum_values[val]);
                    
                    
                    listItems.Add(val);
                }
                postProcessing.orderList = listItems;
                postProcessing.PP_OrderList = postProcessingStepList;
                System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
                st.Start();
                bool isColor = true;
                if (imagingMode == INTUSOFT.Imaging.ImagingMode.FFA_Plus)
                    isColor = false;
                postProcessing.ApplyPostProcessing(ref colorBm, isColor );
                if (!isColor)
                    postProcessing.GetMonoChromeImage(ref colorBm);
               // System.Windows.Forms.MessageBox.Show(st.ElapsedMilliseconds.ToString());


                //st.Stop();
                //Console.WriteLine(st.ElapsedMilliseconds);
                //MessageBox.Show(st.ElapsedMilliseconds.ToString());
                //colorBm.Save(finf.DirectoryName + Path.DirectorySeparatorChar + "Processed"+Path.DirectorySeparatorChar + finf.Name.Split('.')[0] + "ResultImage.png");
               if(!isBatchPP)
                display_pbx.Image = colorBm;

                if(string.IsNullOrEmpty(saveDir))
                    processedImgPath = finf.DirectoryName + Path.DirectorySeparatorChar + finf.Name.Split('.')[0] +DateTime.Now.ToString("yyyyMMddHHmmss")+".png"; // save the full path of the post processed image path if the save dir is not present
                else
                processedImgPath = saveDir+Path.DirectorySeparatorChar+DateTime.Now.ToString("yyyyMMddHHmmss")+".png"; // save the full path of the post processed image 

                colorBm.Save(processedImgPath);
                SavePPSettings(finf);
                //MessageBox.Show("Post processing done  " + st.ElapsedMilliseconds.ToString());
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
        }
        private void Unsharp_btn_Click(object sender, EventArgs e)
        {
            //display_pbx.Image = colorBm;
        }
        private void gammaCorrection(ref Bitmap bm, double gammaVal)
        {
            Image<Bgr, byte> inp = new Image<Bgr, byte>(bm);
            inp._EqualizeHist();
            inp._GammaCorrect(gammaVal);
            bm = inp.ToBitmap();
        }
        private void gammaCorrection_tb_ValueChanged(object sender, EventArgs e)
        {

            Bitmap bm = new Bitmap(fileNameMask);
            display_pbx.Image = bm;
        }

        private void getMotorMovement_btn_Click(object sender, EventArgs e)
        {

        }

        private void left_rb_CheckedChanged(object sender, EventArgs e)
        {
            //ivl_camera.isLeftRegionCompute = left_rb.Checked;

        }
        int FlashExposure = 50000;
        private void increaseExposure_btn_Click(object sender, EventArgs e)
        {
            FlashExposure += 2500;
        }

        private void decreaseExposure_btn_Click(object sender, EventArgs e)
        {
            FlashExposure -= 2500;

        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            drawMaskRectangle();
            //if (colorBm != null)
            //    colorBm.Dispose();
            //colorBm = new Bitmap(fileNameMask);
            //ApplyMask(ref colorBm);
            //display_pbx.Image = colorBm;
            //display_pbx.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    dInf = new DirectoryInfo(fbd.SelectedPath);
                    FileInfo[] finf = dInf.GetFiles("*.png");
                    foreach (FileInfo item in finf)
                    {
                        fileNameMask = item.FullName;

                        Bitmap bm = new Bitmap(item.FullName);
                        ApplyPostProcessing(ref bm);
                        bm.Dispose();
                    }
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void applyMask_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (colorBm != null)
                {
                    Image<Bgr, byte> inputImg = new Image<Bgr, byte>(colorBm);
                    postProcessing.ApplyMask(ref colorBm, ivl_camera.camPropsHelper._Settings.PostProcessingSettings.maskSettings,true);
                    inputImg.Dispose();
                    FileInfo fileInfo = new FileInfo(fileNameMask);
                    display_pbx.Image = colorBm;
                    colorBm.Save(fileInfo.Directory.FullName + Path.DirectorySeparatorChar + "Mask_" + fileInfo.Name);

                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }



        private void centreValues_btn_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    fileNameMask = ofd.FileName;
                else
                    return;
                Image<Bgr, byte> inp = new Image<Bgr, byte>(fileNameMask);
                FileInfo f = new FileInfo(fileNameMask);
                string[] str = f.Name.Split('.');
                StreamWriter stWriterHorizontal = new StreamWriter(f.DirectoryName + Path.DirectorySeparatorChar + str[0] + "_Horizontal.csv");
                StreamWriter stWriterVertical = new StreamWriter(f.DirectoryName + Path.DirectorySeparatorChar + str[0] + "_Vertical.csv");
                stWriterHorizontal.WriteLine("R,G,B");
                stWriterVertical.WriteLine("R,G,B");
                // code for dumping centre values in the vertical centre line
                for (int i = 0; i < inp.Height; i++)
                {
                    stWriterVertical.WriteLine(inp.Data[i, (int)HotSpotCentreX_nud.Value, 2].ToString() + "  ," + inp.Data[i, (int)HotSpotCentreX_nud.Value, 1].ToString() + "  ," + inp.Data[i, (int)HotSpotCentreX_nud.Value, 0].ToString());
                }
                for (int i = 0; i < inp.Width; i++)
                {
                    stWriterHorizontal.WriteLine(inp.Data[(int)HotSpotCentreY_nud.Value, i, 2].ToString() + "  ," + inp.Data[(int)HotSpotCentreY_nud.Value, i, 1].ToString() + "  ," + inp.Data[(int)HotSpotCentreY_nud.Value, i, 0].ToString());
                }
                stWriterHorizontal.Close();
                stWriterVertical.Close();
                inp.Dispose();
                MessageBox.Show("write Completed");
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
           
        }
        string[] strArr;
        Image<Bgr, byte> tempImg;
        Image<Gray, byte> tempImg1;
        private void PostProcessing_btn_Click(object sender, EventArgs e)
        {
            try
            {
                // #region Post Processing
                if (colorBm != null)
                    colorBm.Dispose();
                if (string.IsNullOrEmpty(fileNameMask))
                    return;
                Bitmap tempbm = new Bitmap(fileNameMask);
                colorBm = new Bitmap(tempbm.Width, tempbm.Height, PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(colorBm);
                g.DrawImage(tempbm, new Rectangle(0, 0, tempbm.Width, tempbm.Height));
                tempbm.Dispose();
                display_pbx.Image = colorBm;

                FileInfo finf = new FileInfo(fileNameMask);
                strArr = finf.Name.Split('.');

                ApplyPostProcessing(ref colorBm);
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void browseRawImage_btn_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "PNG|*.png|JPG (*.jpg,*.jpeg)|*.jpg;*.jpeg|TIFF (*.tif,*.tiff)|*.tif;*.tiff|BMP|*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    fileNameMask = ofd.FileName;
                    Bitmap tempbm = new Bitmap(fileNameMask);
                    colorBm = new Bitmap(tempbm.Width, tempbm.Height, PixelFormat.Format24bppRgb);
                    Graphics g = Graphics.FromImage(colorBm);
                    g.DrawImage(tempbm, new Rectangle(0, 0, tempbm.Width, tempbm.Height));
                    tempbm.Dispose();
                    FileInfo tempFinf = new FileInfo(fileNameMask);
                    string readLine = "";
                    if (File.Exists(tempFinf.Directory.FullName + Path.DirectorySeparatorChar + "_Settings.log"))
                    {
                        StreamReader st = new StreamReader(tempFinf.Directory.FullName + Path.DirectorySeparatorChar + "_Settings.log");
                        while ((readLine = st.ReadLine()) != null)
                        {
                            if (readLine.Contains("Flash Gain"))
                            {
                                string[] strAr = readLine.Split('=');
                                currentGain = Convert.ToInt32(strAr[strAr.Length - 1]);
                            }
                        }
                        st.Close();
                    }
                    if (colorBm.Width > 2048)
                        PostProcessing.isFourteenBit = true;
                    else
                        PostProcessing.isFourteenBit = false;
                    if (colorBm.Width != PostProcessing.Width && colorBm.Height != PostProcessing.Height || HotSpotCentreX_nud.Value != ivl_camera.camPropsHelper._Settings.PostProcessingSettings.maskSettings.maskCentreX || HotSpotCentreY_nud.Value != ivl_camera.camPropsHelper._Settings.PostProcessingSettings.maskSettings.maskCentreY)
                    {
                        if (PostProcessing.isDemosaicInit)
                        {
                            PostProcessing.ImageProc_Demosaic_Exit();
                            //PostProcessing.AssistedFocus_Exit();
                        }
                        PostProcessing.isDemosaicInit = false;
                    }
                    PostProcessing.initDemosaic(colorBm.Width, colorBm.Height, (int)HotSpotCentreX_nud.Value, (int)HotSpotCentreY_nud.Value);
                    bool isBatch = false;
                    if (isBatch)
                    {
                        FileInfo[] fArr = tempFinf.Directory.GetFiles();

                        foreach (var item in fArr)
                        {
                            fileNameMask = item.FullName;
                            colorBm = new Bitmap(fileNameMask);
                            ApplyPostProcessing(ref colorBm);
                            colorBm.Save(item.DirectoryName + Path.DirectorySeparatorChar + "Processed" + Path.DirectorySeparatorChar + item.Name);
                        }
                    }
                    else
                    {
                        colorBm = new Bitmap(fileNameMask);
                        tempImg = new Image<Bgr, byte>(colorBm.Width, colorBm.Height);
                    }
                    display_pbx.Image = colorBm;
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void postProcessing_tbp_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged_1(object sender, EventArgs e)
        {

        }

        public void drawRadiusForHotSpot()
        {
            try
            {
                if (colorBm != null)
                {
                    Bitmap overlay = new Bitmap(colorBm.Width, colorBm.Height);
                    Graphics g = Graphics.FromImage(overlay);
                    //int shadowRadius2 = (int)ShadowRad1_nud.Value + (int)ShadowRad2_nud.Value + (int)HSRad1_nud.Value + (int)HSRad2_nud.Value;
                    //int shadowRadius1 = (int)ShadowRad1_nud.Value + (int)HSRad1_nud.Value + (int)HSRad2_nud.Value;
                    //int HotSpotRadius2 = (int)HSRad1_nud.Value + (int)HSRad2_nud.Value;
                    //int HotSpotRadius1 = (int)HSRad1_nud.Value;
                    if (control_tb.SelectedTab == HotSpot_tbp)
                    {
                        int shadowRadius2 = (int)ShadowRad2_nud.Value;
                        int shadowRadius1 = (int)ShadowRad1_nud.Value;
                        int HotSpotRadius2 = (int)HSRad2_nud.Value;
                        int HotSpotRadius1 = (int)HSRad1_nud.Value;

                        g.DrawEllipse(new Pen(Color.Red, 2.0f), new Rectangle((int)HotSpotCentreX_nud.Value - shadowRadius2, (int)HotSpotCentreY_nud.Value - shadowRadius2, 2 * shadowRadius2, 2 * shadowRadius2));
                        g.DrawEllipse(new Pen(Color.Red, 2.0f), new Rectangle((int)HotSpotCentreX_nud.Value - shadowRadius1, (int)HotSpotCentreY_nud.Value - shadowRadius1, 2 * shadowRadius1, 2 * shadowRadius1));
                        g.DrawEllipse(new Pen(Color.Red, 2.0f), new Rectangle((int)HotSpotCentreX_nud.Value - HotSpotRadius1, (int)HotSpotCentreY_nud.Value - HotSpotRadius1, 2 * HotSpotRadius1, 2 * HotSpotRadius1));
                        g.DrawEllipse(new Pen(Color.Red, 2.0f), new Rectangle((int)HotSpotCentreX_nud.Value - HotSpotRadius2, (int)HotSpotCentreY_nud.Value - HotSpotRadius2, 2 * HotSpotRadius2, 2 * HotSpotRadius2));
                        g.Dispose();
                    }
                    else if (control_tb.SelectedTab == Mask_tbp)
                    {
                        g.DrawEllipse(new Pen(Color.Red, 5.0f), new Rectangle((int)maskX_nud.Value - (int)LiveMaskWidth_nud.Value / 2, (int)maskY_nud.Value - (int)LiveMaskHeight_nud.Value / 2, (int)LiveMaskWidth_nud.Value, (int)LiveMaskHeight_nud.Value));
                        g.DrawEllipse(new Pen(Color.Green, 5.0f), new Rectangle((int)maskX_nud.Value - (int)CaptureMaskWidth_nud.Value / 2, (int)maskY_nud.Value - (int)CaptureMaskHeight_nud.Value / 2, (int)CaptureMaskWidth_nud.Value, (int)CaptureMaskHeight_nud.Value));

                    }
                    else
                    {
                        ClearHotSpotMarking();
                    }
                    overlay.MakeTransparent(Color.Black);
                    overlay_pbx.BackColor = Color.Transparent;
                    overlay_pbx.Image = overlay;
                    overlay_pbx.Dock = DockStyle.Fill;
                    overlay_pbx.Parent = display_pbx;
                    //overlay_pbx.BringToFront();
                    overlay_pbx.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void ShadowRad1_nud_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                drawRadiusForHotSpot();
                
                {
                    ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowRadSpot1.val= ShadowRad1_nud.Value.ToString();
                    ivl_camera.camPropsHelper._Settings.PostProcessingSettings.hotspotSettings.radSpot1 = (int)ShadowRad1_nud.Value;

                    UpdateConfigSettings();

                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
           
        }

        private void ShadowRad2_nud_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                drawRadiusForHotSpot();
                
                {
                    ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowradSpot2.val= ShadowRad2_nud.Value.ToString();
                    ivl_camera.camPropsHelper._Settings.PostProcessingSettings.hotspotSettings.radSpot2 = (int)ShadowRad2_nud.Value;

                    UpdateConfigSettings();

                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            

        }

        private void HSRad1_nud_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                drawRadiusForHotSpot();
                
                {
                    ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._HotspotRadius1.val= HSRad1_nud.Value.ToString();
                    ivl_camera.camPropsHelper._Settings.PostProcessingSettings.hotspotSettings.hotSpotRad1 = (int)HSRad1_nud.Value;

                    UpdateConfigSettings();

                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void HSRad2_nud_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                drawRadiusForHotSpot();
                
                {
                    ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._HotspotRadius2.val= HSRad2_nud.Value.ToString();
                    ivl_camera.camPropsHelper._Settings.PostProcessingSettings.hotspotSettings.hotSpotRad2 = (int)HSRad2_nud.Value;

                    UpdateConfigSettings();

                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }
        private void ClearHotSpotMarking()
        {
            try
            {
                Bitmap overlay = new Bitmap(2048, 1536);
                overlay.MakeTransparent(Color.Black);
                overlay_pbx.BackColor = Color.Transparent;
                overlay_pbx.Image = overlay;
                overlay_pbx.Dock = DockStyle.Fill;
                overlay_pbx.Parent = display_pbx;
                //overlay_pbx.BringToFront();
                overlay_pbx.Visible = true;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }
        private void ClearHotSpotMarking_btn_Click(object sender, EventArgs e)
        {
            ClearHotSpotMarking();
        }

        int currentGain = 180;
        private void GetImage()
        {
            try
            {
                if (colorBm != null)
                    colorBm.Dispose();
                colorBm = new Bitmap(fileNameMask);
                finf = new FileInfo(fileNameMask);
                strArr = finf.Name.Split('.');
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }
        private void applyHotSpotCorrection_btn_Click(object sender, EventArgs e)
        {
            GetImage();
            bool isColor = true;
            if (imagingMode == INTUSOFT.Imaging.ImagingMode.FFA_Plus)
                isColor = false;
            postProcessing.ApplyHotSpotCompensation(ref colorBm, ivl_camera.camPropsHelper._Settings.PostProcessingSettings.hotspotSettings,isColor);
            display_pbx.Image = colorBm;
        }

        private void ApplyShiftImage_btn_Click(object sender, EventArgs e)
        {
            GetImage();

            display_pbx.Image = colorBm;

        }

        private void ApplyCC_btn_Click(object sender, EventArgs e)
        {
            GetImage();
            bool isColor = true;
            if (imagingMode == INTUSOFT.Imaging.ImagingMode.FFA_Plus)
                isColor = false;
            postProcessing.Applycolorcorrection(ref colorBm, ivl_camera.camPropsHelper._Settings.PostProcessingSettings.ccSettings,isColor);
            display_pbx.Image = colorBm;

        }

        private void setCentreImage_btn_Click(object sender, EventArgs e)
        {
            GetImage();

            //Size newSize = new Size((int)(colorBm.Width * 10), (int)(colorBm.Height * 10));
            //Bitmap bmp = new Bitmap(newSize.Width,newSize.Height);
            //Graphics g = Graphics.FromImage(bmp);
            //g.DrawImage(colorBm, new RectangleF(0, 0, bmp.Width, bmp.Height),new Rectangle(0,0,colorBm.Width,colorBm.Height),GraphicsUnit.Pixel);
            //display_pbx.Image = bmp;
            UpdateZoomedImage(null);

        }
        Graphics bmGraphics;
        Bitmap overlay;
        private void UpdateZoomedImage(MouseEventArgs e)
        {
            try
            {
                if (display_pbx.Image != null)
                {
                    // Calculate the width and height of the portion of the image we want
                    // to show in the picZoom picturebox. This value changes when the zoom
                    // factor is changed.
                    int zoomWidth = 0;
                    int zoomHeight = 0;
                    int halfWidth = 0;
                    int halfHeight = 0;
                    if (twoX_zoom_rb.Checked)
                    {
                        zoomWidth = colorBm.Width / 2;
                        zoomHeight = colorBm.Height / 2;
                        halfWidth = zoomWidth / 2;
                        halfHeight = zoomHeight / 2;
                    }
                    else
                    {
                        zoomWidth = colorBm.Width;
                        zoomHeight = colorBm.Height;
                    }
                    // Calculate the horizontal and vertical midpoints for the crosshair
                    // cursor and correct centering of the new image
                    //int halfWidth = zoomWidth / 2;
                    //int halfHeight = zoomHeight / 2;
                    // Create a new temporary bitmap to fit inside the picZoom picturebox
                    // Bitmap tempBitmap = new Bitmap(zoomWidth, zoomHeight, PixelFormat.Format24bppRgb);
                    Bitmap tempBitmap = new Bitmap(zoomWidth, zoomHeight, PixelFormat.Format24bppRgb);
                    overlay = new Bitmap(zoomWidth, zoomHeight);
                    overlay.MakeTransparent(Color.Black);
                    overlay_pbx.BackColor = Color.Transparent;
                    overlay_pbx.Image = overlay;
                    overlay_pbx.Dock = DockStyle.Fill;
                    overlay_pbx.Parent = display_pbx;
                    //overlay_pbx.BringToFront();
                    overlay_pbx.Visible = true;
                    // Create a temporary Graphics object to work on the bitmap
                    bmGraphics = Graphics.FromImage(tempBitmap);

                    // Clear the bitmap with the selected backcolor

                    // Set the interpolation mode
                    bmGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                    // Draw the portion of the main image onto the bitmap
                    // The target rectangle is already known now.
                    // Here the mouse position of the cursor on the main image is used to
                    // cut out a portion of the main image.
                    if (twoX_zoom_rb.Checked)
                        bmGraphics.DrawImage(colorBm,
                                             new Rectangle(0, 0, zoomWidth, zoomHeight),
                                             new Rectangle(zoomWidth - halfWidth, zoomHeight - halfHeight, zoomWidth, zoomHeight),
                                             GraphicsUnit.Pixel);

                    else
                        bmGraphics.DrawImage(colorBm,
                                         new Rectangle(0, 0, zoomWidth, zoomHeight),
                                         new Rectangle(0, 0, zoomWidth, zoomHeight),
                                         GraphicsUnit.Pixel);
                    // Draw the bitmap on the picZoom picturebox
                    display_pbx.Image = tempBitmap;

                    // Draw a crosshair on the bitmap to simulate the cursor position


                    // Dispose of the Graphics object
                    // bmGraphics.Dispose();

                    // Refresh the picZoom picturebox to reflect the changes
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void oneX_zoom_rb_CheckedChanged(object sender, EventArgs e)
        {
            UpdateZoomedImage(null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clearOvelay();
        }
        private void clearOvelay()
        {
            try
            {
                if (overlay_pbx.Image != null)
                {
                    overlay = new Bitmap(overlay_pbx.Image.Width, overlay_pbx.Image.Height, PixelFormat.Format24bppRgb);
                    overlay.MakeTransparent(Color.Black);
                    overlay_pbx.Image = overlay;
                    overlayFileName = "";
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void rawModeFrameGrab_rb_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                

                    ConfigVariables.CurrentSettings.CameraSettings._EnableRawMode.val= rawModeFrameGrab_rb.Checked.ToString();
                UpdateConfigSettings();
                ivl_camera.camPropsHelper._Settings.CameraSettings.isRawMode = rawModeFrameGrab_rb.Checked;
                this.Cursor = Cursors.WaitCursor;
                if (ivl_camera.isCameraOpen)
                {
                    ivl_camera.RestartCamera();
                    ivl_camera.StartLive();
                    ivl_camera.camPropsHelper._Settings.CameraSettings.LiveExposure = (uint)expVal_nud.Value;
                    ivl_camera.camPropsHelper._Settings.CameraSettings.LiveGain = (ushort)gainVal_nud.Value;
                    ivl_camera.camPropsHelper.RotateImageVertical(vFlip_cbx.Checked);
                    ivl_camera.camPropsHelper.RotateImageHorizontal(hFlip_cbx.Checked);
                }

                this.Cursor = Cursors.Default;
                bool val = CCMode_rb.Checked;
                tempTint_gbx.Enabled = EnableTemperatureTint_cbx.Checked;
                display_pbx.Refresh();
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            

        }

        void flashOffset_nud_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                
                ConfigVariables.CurrentSettings.FirmwareSettings._FlashOffsetEnd.val= flashOffset2_nud.Value.ToString();
                byte[] arr = { Convert.ToByte(flashoffset1_nud.Value), Convert.ToByte(flashOffset2_nud.Value) };
                ivl_camera.camPropsHelper._Settings.BoardSettings.CaptureStartOffset = arr[0];
                ivl_camera.camPropsHelper._Settings.BoardSettings.CaptureEndOffset = arr[1];
               // ivl_camera.camPropsHelper.SetFlashOffSet(arr);
                UpdateConfigSettings();

            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void flashExp_tb_Scroll(object sender, EventArgs e)
        {
        }

        private void flashExp_nud_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!isdefaultExp)
                {
                    FlashExposure = (int)flashExp_nud.Value;
                    


                        ConfigVariables.CurrentSettings.CameraSettings._Exposure.val= FlashExposure.ToString();
                    UpdateConfigSettings();
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void flashGain_nud_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                {
                    ConfigVariables.CurrentSettings.CameraSettings._DigitalGain.val = flashGain_nud.Value.ToString();
                    UpdateConfigSettings();
                    ivl_camera.camPropsHelper._Settings.CameraSettings.CaptureGain = (ushort)flashGain_nud.Value;
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void reloadImage_btn_Click(object sender, EventArgs e)
        {
            GetImage();
            display_pbx.Image = colorBm;

        }

        private void saveSettings_btn_Click(object sender, EventArgs e)
        {

        }


        private void shiftSettings_lnkLbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            control_tb.SelectedTab = shiftImage_tbp;
        }

        private void hsSettings_lnkLbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            control_tb.SelectedTab = HotSpot_tbp;

        }

        private void ccSettings_lnkLbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            control_tb.SelectedTab = ColorCorrection_tbp;

        }

        private void maskSettings_lnkLbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            control_tb.SelectedTab = Mask_tbp;

        }

        private void cameraModel_cmbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            

                //switch (mode_cmbx.Text)
                //{
                //    case "FFA":
                //        {
                //            _settingsConfig.FFA_Settings.UI_Settings.Model = cameraModel_cmbx.Text;
                //            break;
                //        }
                //    case "Posterior":
                //        {
                //            _settingsConfig.Lite_Settings.UI_Settings.Model = cameraModel_cmbx.Text;
                //            break;
                //        }
                //    case "45":
                //        {
                //            _settingsConfig.FortyFive_Settings.UI_Settings.Model = cameraModel_cmbx.Text;
                //            break;
                //        }
                //}
                ConfigVariables.CurrentSettings.CameraSettings._CameraModel.val= cameraModel_cmbx.Text;
            UpdateConfigSettings();
            ivl_camera.camPropsHelper._Settings.CameraSettings.CameraModel = (CameraModel)Enum.Parse(typeof(CameraModel), cameraModel_cmbx.Text);

        }

        private void clearOverlay_btn_Click(object sender, EventArgs e)
        {
            clearOvelay();
        }

        private void overlay_pbx_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (!ivl_camera.CameraIsLive && control_tb.SelectedTab == HotSpot_tbp)
                {
                    Point tempPoint = new Point();
                    if (oneX_zoom_rb.Checked)
                        tempPoint = p;
                    else
                        tempPoint = new Point(p.X * 2, p.Y * 2);

                    if (tempPoint.X < 0)
                        HotSpotCentreX_nud.Value = 0;
                    else if (tempPoint.X > colorBm.Width)
                        HotSpotCentreX_nud.Value = colorBm.Width;
                    else
                        HotSpotCentreX_nud.Value = tempPoint.X;

                    if (tempPoint.Y < 0)
                        HotSpotCentreY_nud.Value = 0;
                    else if (tempPoint.X > colorBm.Height)
                        HotSpotCentreY_nud.Value = colorBm.Height;
                    else
                        HotSpotCentreY_nud.Value = tempPoint.Y;
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            drawRadiusForHotSpot();

        }


        private void temperature_tb_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (EnableTemperatureTint_cbx.Checked)
                {
                    
                        ConfigVariables.CurrentSettings.CameraSettings._Tint.val = tint_tb.Value.ToString();
                    UpdateConfigSettings();
                    temperaturStatus_lbl.Text = temperature_tb.Value.ToString();
                    TemperatureVal_lbl.Text = temperature_tb.Value.ToString();
                    if (ivl_camera != null)
                    {
                        ivl_camera.camPropsHelper.SetTemperatureTint(temperature_tb.Value, tint_tb.Value);
                        //ivl_camera.GetTemperatureTint(ref tempVal, ref tintVal);
                        int[] RGBGain = new int[3];
                        //ivl_camera.GetTemperatureTint(ref tempVal, ref tintVal);// ref RGBGain);
                        temperaturStatus_lbl.Text = "Temp: " + tempVal.ToString();
                        tintStatus_lbl.Text = "Temp: " + tintVal.ToString();

                    }
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void tint_tb_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (EnableTemperatureTint_cbx.Checked)
                {
                    
                        ConfigVariables.CurrentSettings.CameraSettings._Tint.val= tint_tb.Value.ToString();
                    UpdateConfigSettings();
                    tintStatus_lbl.Text = tint_tb.Value.ToString();
                    tintVal_lbl.Text = tint_tb.Value.ToString();
                    if (ivl_camera != null)
                    {
                        ivl_camera.camPropsHelper.SetTemperatureTint(temperature_tb.Value, tint_tb.Value);
                        //ivl_camera.GetTemperatureTint(ref tempVal, ref tintVal);
                        int[] RGBGain = new int[3];
                        //ivl_camera.GetTemperatureTint(ref tempVal, ref tintVal);// ref RGBGain);
                        temperaturStatus_lbl.Text = "Temp: " + tempVal.ToString();
                        tintStatus_lbl.Text = "Temp: " + tintVal.ToString();

                    }
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void colorChannel_btn_Click(object sender, EventArgs e)
        {
            GetImageFromFile();
            if (channelsImg != null)
            {
                resetFilterButtons();
                colorChannel_btn.BackColor = Color.White;
                colorChannel_btn.ForeColor = Color.Black;
                display_pbx.Image = channelsImg.ToBitmap();
            }
        }
        Image<Bgr, byte> channelsImg;
        private void resetFilterButtons()
        {
            redChannel_btn.BackColor = Color.Red;
            greenChannel_btn.BackColor = Color.Green;
            blueChannel_btn.BackColor = Color.Blue;
            if (File.Exists(@"color.png"))
                colorChannel_btn.Image = Image.FromFile(@"color.png"); //colorImage;
        }

        private void GetImageFromFile()
        {
            //if (!string.IsNullOrEmpty(fileNameMask))
            //{
            //    channelsImg = new Image<Bgr, byte>(fileNameMask);
            //}
            if (colorBm != null)
            {
                channelsImg = new Image<Bgr, byte>(colorBm);
            }

        }
        private void redChannel_btn_Click(object sender, EventArgs e)
        {
            try
            {
                GetImageFromFile();
                if (channelsImg != null)
                {
                    resetFilterButtons();
                    redChannel_btn.BackColor = Color.White;
                    redChannel_btn.ForeColor = Color.Black;
                    display_pbx.Image = channelsImg[2].ToBitmap();
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            

        }

        private void greenChannel_btn_Click(object sender, EventArgs e)
        {
            try
            {
                GetImageFromFile();
                if (channelsImg != null)
                {
                    resetFilterButtons();
                    greenChannel_btn.BackColor = Color.White;
                    greenChannel_btn.ForeColor = Color.Black;
                    display_pbx.Image = channelsImg[1].ToBitmap();
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void blueChannel_btn_Click(object sender, EventArgs e)
        {
            try
            {
                GetImageFromFile();
                if (channelsImg != null)
                {
                    resetFilterButtons();
                    blueChannel_btn.BackColor = Color.White;
                    blueChannel_btn.ForeColor = Color.Black;
                    display_pbx.Image = channelsImg[0].ToBitmap();
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void singleFrameCapture_combox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (singleFrameCapture_combox.Text == "True")
                {
                    

                        (ConfigVariables.CurrentSettings.FirmwareSettings._EnableSingleFrameCapture.val) = true.ToString();
                    UpdateConfigSettings();

                    if (ivl_camera != null)
                    {
                        ivl_camera.camPropsHelper._Settings.BoardSettings.IsSingleFrameCapture = (bool) ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.FirmwareSettings._EnableSingleFrameCapture);
                    }
                }
                else
                {
                    

                        (ConfigVariables.CurrentSettings.FirmwareSettings._EnableSingleFrameCapture.val) = false.ToString();
                    UpdateConfigSettings();

                    if (ivl_camera != null)
                        ivl_camera.camPropsHelper.IsSingleFrameCapture = (bool)ConfigVariables.StringVal2Object(ConfigVariables.CurrentSettings.FirmwareSettings._EnableSingleFrameCapture);

                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void flash_rb_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ivl_camera.LEDSource = Led.Flash;
                if (ivl_camera.camPropsHelper.ImagingMode == INTUSOFT.Imaging.ImagingMode.FFAColor || ivl_camera.camPropsHelper.ImagingMode == INTUSOFT.Imaging.ImagingMode.FFA_Plus)
                    ivl_camera.camPropsHelper.EnableFlashControlUsingKnob();
                
                    if (flash_rb.Checked)
                        ConfigVariables.CurrentSettings.CameraSettings.LiveLedSource.val = Led.Flash.ToString();
                UpdateConfigSettings();
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void ir_rb_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ivl_camera.LEDSource = Led.IR;
                
                    if (ir_rb.Checked)
                        ConfigVariables.CurrentSettings.CameraSettings.LiveLedSource.val = Led.IR.ToString();
                //if (ir_rb.Checked && ivl_camera.camPropsHelper.ImagingMode == ImagingMode.FFA_Plus)
                //{
                //    IrOffsetSteps_nud.Value = 35;
                //}
                UpdateConfigSettings();
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }


        private void vFlip_cbx_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                
                    //switch (mode_cmbx.Text)
                    //{
                    //    case "FFA":
                    //        {
                    //            _settingsConfig.FFA_Settings.UI_Settings.isVerticalFlip = vFlip_cbx.Checked;
                    //            break;
                    //        }
                    //    case "Posterior":
                    //        {
                    //            _settingsConfig.Lite_Settings.UI_Settings.isVerticalFlip = vFlip_cbx.Checked;
                    //            break;
                    //        }
                    //    case "45":
                    //        {
                    //            _settingsConfig.FortyFive_Settings.UI_Settings.isVerticalFlip = vFlip_cbx.Checked;
                    //            break;
                    //        }
                    //}

                   ConfigVariables.CurrentSettings.CameraSettings._EnableVerticalFlip.val = vFlip_cbx.Checked.ToString();
                UpdateConfigSettings();
                if (ivl_camera != null)
                    if (ivl_camera.camPropsHelper._Settings.CameraSettings.isRawMode)
                    {
                        ivl_camera.camPropsHelper._Settings.CameraSettings.isVFlipForRaw = vFlip_cbx.Checked;
                    }
                    else
                        ivl_camera.camPropsHelper.RotateImageVertical(vFlip_cbx.Checked);

            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void hFlip_cbx_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                
                    //switch (mode_cmbx.Text)
                    //{
                    //    case "FFA":
                    //        {
                    //            _settingsConfig.FFA_Settings.UI_Settings.isHorizontalFlip = hFlip_cbx.Checked;
                    //            break;
                    //        }
                    //    case "Posterior":
                    //        {
                    //            _settingsConfig.Lite_Settings.UI_Settings.isHorizontalFlip = hFlip_cbx.Checked;
                    //            break;
                    //        }
                    //    case "45":
                    //        {
                    //            _settingsConfig.FortyFive_Settings.UI_Settings.isHorizontalFlip = hFlip_cbx.Checked;
                    //            break;
                    //        }
                    //}

                    ConfigVariables.CurrentSettings.CameraSettings._EnableHorizontalFlip.val = hFlip_cbx.Checked.ToString();
                UpdateConfigSettings();

                if (ivl_camera != null)
                    if (ivl_camera.camPropsHelper._Settings.CameraSettings.isRawMode)
                    {
                        ivl_camera.camPropsHelper._Settings.CameraSettings.isHFlipForRaw = hFlip_cbx.Checked;

                    }
                    else
                        ivl_camera.camPropsHelper.RotateImageHorizontal(hFlip_cbx.Checked);

            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void setFlashBoost()
        {
            try
            {
                if (ivl_camera.camPropsHelper.IsBoardOpen)
                {
                    ivl_camera.camPropsHelper._Settings.BoardSettings.FlashBoostValue = (byte)flashBoost_nud.Value;
                    ivl_camera.camPropsHelper.EnableFlashBoost(flashBoost_cbx.Checked);
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }
        private void flashBoost_cbx_CheckedChanged(object sender, EventArgs e)
        {

            

                ConfigVariables.CurrentSettings.FirmwareSettings._isFlashBoost.val = flashBoost_cbx.Checked.ToString();
            UpdateConfigSettings();

            setFlashBoost();
        }

        private void flashBoost_nud_ValueChanged(object sender, EventArgs e)
        {
            

                ConfigVariables.CurrentSettings.FirmwareSettings._FlashBoostValue.val = flashBoost_nud.Value.ToString();
            UpdateConfigSettings();

            setFlashBoost();
        }

        private void postProcessing_cbx_CheckedChanged(object sender, EventArgs e)
        {
            
                ConfigVariables.CurrentSettings.PostProcessingSettings.PostProcessing.EnablePostProcessing.val= postProcessing_cbx.Checked.ToString();
            UpdateConfigSettings();
            if (ivl_camera != null)
                ivl_camera.camPropsHelper._Settings.PostProcessingSettings.isApplyPostProcessing = postProcessing_cbx.Checked;
        }

        private void applyShift_cbx_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                
                    ConfigVariables.CurrentSettings.PostProcessingSettings.ImageShiftSettings._IsApplyImageShift.val = applyShift_cbx.Checked.ToString();
                UpdateConfigSettings();

                ivl_camera.camPropsHelper._Settings.PostProcessingSettings.imageShiftSettings.isApplyShiftCorrection = applyShift_cbx.Checked;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            

        }

        private void applyHotSpotCorrection_cbx_CheckedChanged(object sender, EventArgs e)
        {
            
                 ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._IsApplyHotspotCorrection.val =applyHotSpotCorrection_cbx.Checked.ToString();
            UpdateConfigSettings();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.hotspotSettings.isEnableHS = applyHotSpotCorrection_cbx.Checked;
        }

        private void applyColorCorrection_cbx_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                
                     ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._IsApplyColorCorrection.val = applyColorCorrection_cbx.Checked.ToString();
                UpdateConfigSettings();
                {
                    ivl_camera.camPropsHelper._Settings.PostProcessingSettings.ccSettings.isApplyColorCorrection = applyColorCorrection_cbx.Checked;
                    float[,] ColorMatrix = {{(float) redRed_nud.Value,(float)	redGreen_nud.Value,(float)	redBlue_nud.Value},
                                        {(float)greenRed_nud.Value, (float)greenGreen_nud.Value,	(float)greenBlue_nud.Value},
                                            {(float)blueRed_nud.Value	,(float)blueGreen_nud.Value,	(float)blueBlue_nud.Value}};
                    postProcessing.colorMatrix = ColorMatrix;
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void applyMask_cbx_CheckedChanged(object sender, EventArgs e)
        {
            
                 ConfigVariables.CurrentSettings.PostProcessingSettings.MaskSettings._IsApplyMask.val = applyMask_cbx.Checked.ToString();
            UpdateConfigSettings();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.maskSettings.isApplyMask = applyMask_cbx.Checked;
        }

        private void ffaCapture_cbx_CheckedChanged(object sender, EventArgs e)
        {
            
                ConfigVariables.CurrentSettings.CameraSettings._CameraModel.val = cameraModel_cmbx.Text;
            UpdateConfigSettings();
        }

        private void IrOffsetSteps_nud_ValueChanged(object sender, EventArgs e)
        {
            
            {
                ConfigVariables.CurrentSettings.MotorOffSetSettings.IR2Flash.val= IrOffsetSteps_nud.Value.ToString();
                UpdateConfigSettings();
            }
            if (ivl_camera != null)
            {

                ivl_camera.camPropsHelper._Settings.BoardSettings.MotorSteps = (int)IrOffsetSteps_nud.Value;
            }
        }

        private void saveFramesCount_nud_ValueChanged(object sender, EventArgs e)
        {
            if (ivl_camera != null)
            {
                ivl_camera.camPropsHelper._Settings.CameraSettings.SaveFramesCnt = (int)saveFramesCount_nud.Value;
                

                    ConfigVariables.CurrentSettings.CameraSettings._SaveFramesCount.val= saveFramesCount_nud.Value.ToString();
                UpdateConfigSettings();
            }
        }
        int FFAseconds;
        private void startFFATimer_btn_Click(object sender, EventArgs e)
        {
            if (startFFATimer_btn.Text == "Start FFA")
            {
                startFFATimer_btn.Text = "Stop FFA Timer";
                ivl_camera.camPropsHelper.EnableFFATimer(true);
            }
            else
            {
                startFFATimer_btn.Text = "Start FFA";
                ivl_camera.camPropsHelper.EnableFFATimer(false);

            }
        }

        void ffaTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            FFAseconds++;
            if (startFFATimer_btn.Text == "Stop FFA")
            {
                TimeSpan ts = new TimeSpan(FFAseconds * TimeSpan.TicksPerSecond);
                ffaTimerStatus_lbl.Text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
            }
        }




        private void flashoffset1_nud_ValueChanged(object sender, EventArgs e)
        {
            

                ConfigVariables.CurrentSettings.FirmwareSettings._FlashOffsetStart.val = flashoffset1_nud.Value.ToString();
            UpdateConfigSettings();
            byte[] arr = { Convert.ToByte(flashoffset1_nud.Value), Convert.ToByte(flashOffset2_nud.Value) };
            ivl_camera.camPropsHelper._Settings.BoardSettings.CaptureStartOffset = arr[0];
            ivl_camera.camPropsHelper._Settings.BoardSettings.CaptureEndOffset = arr[1];
            //ivl_camera.camPropsHelper.SetFlashOffSet(arr);
        }


        private void applyBrightness_cbx_CheckedChanged(object sender, EventArgs e)
        {
                ConfigVariables.CurrentSettings.PostProcessingSettings.BrightnessContrastSettings._IsApplyBrightness.val= applyBrightness_cbx.Checked.ToString();

            UpdateConfigSettings();

                ivl_camera.camPropsHelper._Settings.PostProcessingSettings.brightnessContrastSettings.isApplyBrightness = applyBrightness_cbx.Checked;
        }

        private void ApplyContrast_cbx_CheckedChanged(object sender, EventArgs e)
        {


                ConfigVariables.CurrentSettings.PostProcessingSettings.BrightnessContrastSettings._IsApplyContrast.val = ApplyContrast_cbx.Checked.ToString();
            UpdateConfigSettings();
                ivl_camera.camPropsHelper._Settings.PostProcessingSettings.brightnessContrastSettings.isApplyContrast = ApplyContrast_cbx.Checked;
        }


        private void mode_cmbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                startFFATimer_btn.Enabled = false;

                postProcessing = PostProcessing.GetInstance();
                if (postProcessing.CaptureMaskBitmap != null)
                {
                    postProcessing.CaptureMaskBitmap.Dispose();
                    GC.Collect();
                }
                if (postProcessing.LiveMaskBitmap != null)
                {
                    postProcessing.LiveMaskBitmap.Dispose();
                    GC.Collect();
                }
                    ivl_camera.camPropsHelper.SetMonoChromeMode(false);

                switch (mode_cmbx.Text)
                {
                    case "Posterior_Prime":
                        {
                                imagingMode = INTUSOFT.Imaging.ImagingMode.Posterior_Prime;
                                ConfigVariables._ivlConfig.Mode = INTUSOFT.Configuration.ImagingMode.Posterior_Prime;

                            break;
                        }
                    case "Posterior_45":
                        {
                            imagingMode = INTUSOFT.Imaging.ImagingMode.Posterior_45;
                            ConfigVariables._ivlConfig.Mode = INTUSOFT.Configuration.ImagingMode.Posterior_45;


                            break;
                        }
                    case "Anterior_Prime":
                        {
                                imagingMode = INTUSOFT.Imaging.ImagingMode.Anterior_Prime;
                                ConfigVariables._ivlConfig.Mode = INTUSOFT.Configuration.ImagingMode.Anterior_Prime;


                            break;
                        }
                    case "45Color":
                        {
                            ConfigVariables._ivlConfig.Mode = INTUSOFT.Configuration.ImagingMode.FFAColor;

                                imagingMode = INTUSOFT.Imaging.ImagingMode.FFAColor;

                            break;
                        }

                    case "FFA_Plus":
                        {
                            ConfigVariables._ivlConfig.Mode = INTUSOFT.Configuration.ImagingMode.FFA_Plus;
                           
                                imagingMode = INTUSOFT.Imaging.ImagingMode.FFA_Plus;

                            break;
                        }
                }
                ivl_camera.camPropsHelper.ImagingMode = imagingMode;

                ConfigVariables.GetCurrentSettings();
                SetCurrentSettings();
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }
       
        void UpdateConfigSettings()
        {
            try
            {

                    switch (mode_cmbx.Text)
                    {
                        case "Posterior":
                            {
                                ConfigVariables._ivlConfig.Mode = INTUSOFT.Configuration.ImagingMode.Posterior_Prime;

                                break;
                            }
                        case "45":
                            {
                                ConfigVariables._ivlConfig.Mode = INTUSOFT.Configuration.ImagingMode.Posterior_45;

                                break;
                            }
                        case "Anterior":
                            {
                                ConfigVariables._ivlConfig.Mode = INTUSOFT.Configuration.ImagingMode.Anterior_Prime;

                                break;
                            }
                        case "45 plus Color":
                            {
                                ConfigVariables._ivlConfig.Mode = INTUSOFT.Configuration.ImagingMode.FFAColor;

                                break;
                            }

                        case "45 plus FFA":
                            {
                                ConfigVariables._ivlConfig.Mode = INTUSOFT.Configuration.ImagingMode.FFA_Plus;
                                break;
                            }
                    }
                ConfigVariables.GetCurrentSettings();

            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }
        private void contCapture_cbx_CheckedChanged(object sender, EventArgs e)
        {
            

                ConfigVariables.CurrentSettings.CameraSettings.IsContinousCapture.val= contCapture_cbx.Checked.ToString();
            ivl_camera.camPropsHelper._Settings.CameraSettings.isContinuousCapture = contCapture_cbx.Checked;
            UpdateConfigSettings();
            //switch (mode_cmbx.Text)
            //{
            //    case "FFA":
            //        {
            //            _settingsConfig.FFA_Settings.UI_Settings.isContinousCapture =
            //            break;
            //        }
            //    case "Posterior":
            //        {
            //            _settingsConfig.Lite_Settings.UI_Settings.isContinousCapture = contCapture_cbx.Checked;
            //            break;
            //        }
            //    case "45":
            //        {
            //            _settingsConfig.FortyFive_Settings.UI_Settings.isContinousCapture = contCapture_cbx.Checked;
            //            break;
            //        }
            //}
        }
        string saveDir = string.Empty;
        bool isBatchPP = false;
        private void button3_Click_1(object sender, EventArgs e)
        {
            try
            {
                FileInfo[] finf = null;
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string[] extensions = new[] { ".jpg", ".png", ".bmp" };
                    DirectoryInfo dir = new FileInfo(ofd.FileName).Directory;
                finf=   dir.EnumerateFiles()
         .Where(f => extensions.Contains(f.Extension.ToLower()))
         .ToArray();
                   
                    

                }
                else
                    return;
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    saveDir = fbd.SelectedPath;
                }
                if (finf != null)
                {
                    isBatchPP = true;
                    foreach (FileInfo item in finf)
                    {
                        fileNameMask = item.FullName;
                        if (colorBm != null)
                            colorBm.Dispose();
                        if (string.IsNullOrEmpty(fileNameMask))
                            return;
                        colorBm = new Bitmap(fileNameMask);

                        ApplyPostProcessing(ref colorBm);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void showExtViewer_cbx_CheckedChanged(object sender, EventArgs e)
        {
                ConfigVariables.CurrentSettings.ImageStorageSettings.IsShowExtViewer.val= showExtViewer_cbx.Checked.ToString();
            UpdateConfigSettings();
            if (ivl_camera != null)
                ivl_camera.camPropsHelper.showInExteralViewer = showExtViewer_cbx.Checked;
        }

        private void ApplyLiteCorrection_cbx_CheckedChanged(object sender, EventArgs e)
        {
                ConfigVariables.CurrentSettings.PostProcessingSettings.UnsharpMaskSettings._IsApplyUnsharpSettings.val = ApplyLiteCorrection_cbx.Checked.ToString();
            UpdateConfigSettings();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.unsharpMaskSettings.isApplyUnsharpMask = ApplyLiteCorrection_cbx.Checked;
        }

        private void MotorForward_cbx_CheckedChanged(object sender, EventArgs e)
        {
                ConfigVariables.CurrentSettings.FirmwareSettings._IsMotorPolarityForward.val= MotorForward_cbx.Checked.ToString();
            UpdateConfigSettings();
        }

        private void PatientName_tbx_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(PatientName_tbx.Text))
                ivl_camera.camPropsHelper._Settings.ImageSaveSettings.ProcessedImageDirPath = SaveDirectoryPath + Path.DirectorySeparatorChar + DateTime.Now.ToString("dd-MM-yyyy") + Path.DirectorySeparatorChar + PatientName_tbx.Text;
        }

        private void forteenBit_rb_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                    ConfigVariables.CurrentSettings.CameraSettings._Enable14Bit.val= forteenBit_rb.Checked.ToString();
                UpdateConfigSettings();
                if (ivl_camera != null)
                {
                    if (ivl_camera.isCameraOpen)
                    {
                        ivl_camera.camPropsHelper._Settings.CameraSettings.isFourteen = forteenBit_rb.Checked;
                        PostProcessing.isFourteenBit = forteenBit_rb.Checked;
                        if (forteenBit_rb.Checked)
                            ivl_camera.camPropsHelper.CameraBitDepth = BitDepth.Depth_14;
                        else
                            ivl_camera.camPropsHelper.CameraBitDepth = BitDepth.Depth_8;
                    }

                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void SaveIr_cbx_CheckedChanged(object sender, EventArgs e)
        {
                ConfigVariables.CurrentSettings.ImageStorageSettings._IsIrSave.val= SaveIr_cbx.Checked.ToString();
            UpdateConfigSettings();
            if (ivl_camera != null)
                ivl_camera.camPropsHelper._Settings.ImageSaveSettings.isIR_ImageSave = SaveIr_cbx.Checked;
        }

        private void getRaw_btn_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Raw|*.raw";

                string rawFileName = "";


                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rawFileName = ofd.FileName;
                }
                else
                    return;
                PostProcessing.initDemosaic(2048, 1536, (int)HotSpotCentreX_nud.Value, (int)HotSpotCentreY_nud.Value);
                byte[] rawBytes = File.ReadAllBytes(rawFileName);


                colorBm = new Bitmap(3072, 2048, PixelFormat.Format24bppRgb);
                ushort[] tempVal = new ushort[rawBytes.Length / 2]; // Array.Convert(src, b => (UInt16)b);
                int sum = 0;
                Buffer.BlockCopy(rawBytes, 0, tempVal, 0, rawBytes.Length);
                GCHandle pinnedArray1 = GCHandle.Alloc(tempVal, GCHandleType.Pinned);
                IntPtr pointer1 = pinnedArray1.AddrOfPinnedObject();
                BitmapData bdata = colorBm.LockBits(new Rectangle(0, 0, colorBm.Width, colorBm.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                FileInfo finf = new FileInfo(rawFileName);
                try
                {
                    PostProcessing.ImageProc_Demosaic_16bit(pointer1, bdata.Scan0, colorBm.Width, colorBm.Height, isApplyLut_cbx.Checked);

                }
                catch (Exception ex)
                {
                    Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);

                }
                colorBm.UnlockBits(bdata);
                colorBm.RotateFlip(RotateFlipType.Rotate180FlipX);
                pinnedArray1.Free();
                colorBm.Save(finf.DirectoryName + Path.DirectorySeparatorChar + "processed.png");
                finf = new FileInfo(finf.DirectoryName + Path.DirectorySeparatorChar + "processed.png");
                fileNameMask = finf.FullName;
                // Image<Bgr, byte> inp = new Image<Bgr, byte>(colorBm);
                //inp = inp.SmoothMedian(ConfigVariables.CurrentSettings.unsharpMaskSettings.medianFilterWindow);
                //colorBm = inp.ToBitmap();
                if (postProcessing_cbx.Checked)
                    ApplyPostProcessing(ref colorBm);
                display_pbx.Image = colorBm;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void createLUT_btn_Click(object sender, EventArgs e)
        {
            try
            {
                int maxValue = (int)Math.Pow(2, 14);
                int quaterthValue = (int)maxValue / 4;
                int halfthValue = (int)2 * quaterthValue;
                int threeFourthValue = (int)3 * quaterthValue;
                double sineFactor = 4096;
                double interval1 = 4096;  // 1st phase of curve is from 0 to interval 1 -- Sine wave (0 to PI/2)
                double interval2 = 8192; // 2nd phase of curve is from interval1 to interval1+interval 2  -- Cosine Wave (0 to PI)
                // 3rd phase of the curve is from interval1+interval2 upto max value -- Linear

                double CosineFactor = sineFactor / 2; // nominally be sinefactor/2
                ushort[] LUT = new ushort[maxValue];
                for (ushort i = 0; i < maxValue; i++)
                {
                    if (i < quaterthValue)
                    {
                        double value = sineFactor * Math.Sin((double)i / interval1 * Math.PI / 2);
                        LUT[i] = (ushort)(i + (ushort)value);
                    }
                    else if (i < threeFourthValue)
                    {


                        double value = CosineFactor * (1 + Math.Cos(((double)i - interval1) / interval2 * (Math.PI / 2)));
                        LUT[i] = (ushort)(i + (ushort)value);
                    }
                    else
                    {
                        LUT[i] = i;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void isApplyLut_cbx_CheckedChanged(object sender, EventArgs e)
        {
            if (isApplyLut_cbx.Checked)
            {
                allChannelLutCbx.Enabled = false;
                applyChannelWiseLutLklbl.Enabled = false;
                ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings._IsApplyLutSettings.val = isApplyLut_cbx.Checked.ToString();
                //ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings._IsChannelWiseLUT.val = isApplyLut_cbx.Checked.ToString();
                UpdateConfigSettings();
                ivl_camera.camPropsHelper._Settings.PostProcessingSettings.lutSettings.isApplyLUT = isApplyLut_cbx.Checked;
                //ivl_camera.camPropsHelper._Settings.PostProcessingSettings.lutSettings.isChannelWiseLUT = isApplyLut_cbx.Checked;
            }
            else
            {
                allChannelLutCbx.Enabled = true;
                applyChannelWiseLutLklbl.Enabled = true;
                ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings._IsApplyLutSettings.val = isApplyLut_cbx.Checked.ToString();
                //ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings._IsChannelWiseLUT.val = isApplyLut_cbx.Checked.ToString();
                UpdateConfigSettings();
                ivl_camera.camPropsHelper._Settings.PostProcessingSettings.lutSettings.isApplyLUT = isApplyLut_cbx.Checked;
                //ivl_camera.camPropsHelper._Settings.PostProcessingSettings.lutSettings.isChannelWiseLUT = isApplyLut_cbx.Checked;
            }
        }


        private void unsharp_btn_Click_1(object sender, EventArgs e)
        {
            colorBm = new Bitmap(fileNameMask);

            display_pbx.Image = colorBm;

        }

        #region Color Correction Numeric UpDown Events
        private void redRed_nud_ValueChanged(object sender, EventArgs e)
        {
                    ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._RRCompensation.val = redRed_nud.Value.ToString();
                    UpdateConfigSettings();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.ccSettings.rrVal = (float)redRed_nud.Value;

        }

        private void redGreen_nud_ValueChanged(object sender, EventArgs e)
        {
                    ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._RGCompensation.val = redGreen_nud.Value.ToString();
                    UpdateConfigSettings();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.ccSettings.rgVal = (float)redGreen_nud.Value;
        }

        private void redBlue_nud_ValueChanged(object sender, EventArgs e)
        {
                    ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._RBCompensation.val = redBlue_nud.Value.ToString();
                    UpdateConfigSettings();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.ccSettings.rbVal = (float)redBlue_nud.Value;

        }

        private void greenRed_nud_ValueChanged(object sender, EventArgs e)
        {
                ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._GRCompensation.val = greenRed_nud.Value.ToString();
                UpdateConfigSettings();

            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.ccSettings.grVal = (float)greenRed_nud.Value;

            postProcessing.colorMatrix[1, 0] = (float)greenRed_nud.Value;
        }

        private void greenGreen_nud_ValueChanged(object sender, EventArgs e)
        {
                ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._GGCompensation.val = greenGreen_nud.Value.ToString();
                UpdateConfigSettings();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.ccSettings.ggVal = (float)greenGreen_nud.Value;

            postProcessing.colorMatrix[1, 1] = (float)greenGreen_nud.Value;
        }

        private void greenBlue_nud_ValueChanged(object sender, EventArgs e)
        {
                ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._GBCompensation.val = greenBlue_nud.Value.ToString();
                UpdateConfigSettings();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.ccSettings.gbVal = (float)greenBlue_nud.Value;

            postProcessing.colorMatrix[1, 2] = (float)greenBlue_nud.Value;
        }

        private void blueRed_nud_ValueChanged(object sender, EventArgs e)
        {
                ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._BRCompensation.val = blueRed_nud.Value.ToString();
                UpdateConfigSettings();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.ccSettings.brVal = (float)blueRed_nud.Value;

            postProcessing.colorMatrix[2, 0] = (float)blueRed_nud.Value;
        }

        private void blueGreen_nud_ValueChanged(object sender, EventArgs e)
        {
                ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._BGCompensation.val = blueGreen_nud.Value.ToString();
                UpdateConfigSettings();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.ccSettings.bgVal = (float)blueGreen_nud.Value;

            postProcessing.colorMatrix[2, 1] = (float)blueGreen_nud.Value;
        }

        private void blueBlue_nud_ValueChanged(object sender, EventArgs e)
        {
                ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._BBCompensation.val = blueBlue_nud.Value.ToString();
                UpdateConfigSettings();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.ccSettings.bbVal = (float)blueBlue_nud.Value;

            postProcessing.colorMatrix[2, 2] = (float)blueBlue_nud.Value;
        }
        #endregion

        #region Image Shift Settings
        private void xShift_nud_ValueChanged(object sender, EventArgs e)
        {
                ConfigVariables.CurrentSettings.PostProcessingSettings.ImageShiftSettings._ImageShiftX.val= xShift_nud.Value.ToString();
                UpdateConfigSettings();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.imageShiftSettings.ShiftX = (int)xShift_nud.Value;
        }

        private void Yshift_nud_ValueChanged(object sender, EventArgs e)
        {
                ConfigVariables.CurrentSettings.PostProcessingSettings.ImageShiftSettings._ImageShiftY.val= Yshift_nud.Value.ToString();
                UpdateConfigSettings();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.imageShiftSettings.ShiftY = (int)Yshift_nud.Value;

        }
        #endregion

        #region HotSpot Centre Value Changed Settings
        private void HotSpotCentreX_nud_ValueChanged(object sender, EventArgs e)
        {
                ConfigVariables.CurrentSettings.CameraSettings._ImageOpticalCentreX.val = HotSpotCentreX_nud.Value.ToString();
                UpdateConfigSettings();
                maskX_nud.Value = (int)HotSpotCentreX_nud.Value;
                drawRadiusForHotSpot();
        }

        private void HotSpotCentreY_nud_ValueChanged(object sender, EventArgs e)
        {
                ConfigVariables.CurrentSettings.CameraSettings._ImageOpticalCentreY.val= HotSpotCentreY_nud.Value.ToString();
                maskY_nud.Value = (int)HotSpotCentreY_nud.Value;
                UpdateConfigSettings();
                drawRadiusForHotSpot();
        }
        #endregion

        #region UnSharp Settings Value Changed Events
        private void medianfilter_nud_ValueChanged(object sender, EventArgs e)
        {
                ConfigVariables.CurrentSettings.PostProcessingSettings.UnsharpMaskSettings._MedFilter.val = medianfilter_nud.Value.ToString();
                UpdateConfigSettings();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.unsharpMaskSettings.medFilterValue = (int)medianfilter_nud.Value;

        }

        private void unsharpAmount_nud_ValueChanged(object sender, EventArgs e)
        {
                ConfigVariables.CurrentSettings.PostProcessingSettings.UnsharpMaskSettings._UnSharpAmount.val = unsharpAmount_nud.Value.ToString();
                UpdateConfigSettings();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.unsharpMaskSettings.unsharpAmount = (double)unsharpAmount_nud.Value;
        }

        private void unsharpRadius_nud_ValueChanged(object sender, EventArgs e)
        {
                 ConfigVariables.CurrentSettings.PostProcessingSettings.UnsharpMaskSettings._UnSharpRadius.val = unsharpRadius_nud.Value.ToString();
                UpdateConfigSettings();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.unsharpMaskSettings.radius = (double)unsharpRadius_nud.Value;
        }

        private void unsharpThresh_nud_ValueChanged(object sender, EventArgs e)
        {
                 ConfigVariables.CurrentSettings.PostProcessingSettings.UnsharpMaskSettings._Threshold.val = unsharpThresh_nud.Value.ToString(); 
                UpdateConfigSettings();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.unsharpMaskSettings.unsharpThresh = (double)unsharpThresh_nud.Value;
        }
        #endregion

        #region RGB Compensation KeyDown Events
        private void redRed_nud_KeyDown(object sender, KeyEventArgs e)
        {
               
                ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._RRCompensation.val = redRed_nud.Value.ToString();
                UpdateConfigSettings();
            postProcessing.colorMatrix[0, 0] = (float)redRed_nud.Value;
        }

        private void redGreen_nud_KeyDown(object sender, KeyEventArgs e)
        {
                ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._RGCompensation.val = redGreen_nud.Value.ToString();

                UpdateConfigSettings();
            postProcessing.colorMatrix[0, 1] = (float)redGreen_nud.Value;
        }

        private void redBlue_nud_KeyDown(object sender, KeyEventArgs e)
        {
                ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._RBCompensation.val = redBlue_nud.Value.ToString();
                UpdateConfigSettings();
            postProcessing.colorMatrix[0, 2] = (float)redBlue_nud.Value;
        }

        private void greenRed_nud_KeyDown(object sender, KeyEventArgs e)
        {
                ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._GRCompensation.val = greenRed_nud.Value.ToString();
                UpdateConfigSettings();
            postProcessing.colorMatrix[1, 0] = (float)greenRed_nud.Value;
        }

        private void greenGreen_nud_KeyDown(object sender, KeyEventArgs e)
        {
                ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._GGCompensation.val = greenGreen_nud.Value.ToString();

                UpdateConfigSettings();
            postProcessing.colorMatrix[1, 1] = (float)greenGreen_nud.Value;
        }

        private void greenBlue_nud_KeyDown(object sender, KeyEventArgs e)
        {
            ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._GBCompensation.val = greenBlue_nud.Value.ToString();
                UpdateConfigSettings();
            postProcessing.colorMatrix[1, 2] = (float)greenBlue_nud.Value;
        }

        private void blueRed_nud_KeyDown(object sender, KeyEventArgs e)
        {
                ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._BRCompensation.val = blueRed_nud.Value.ToString();
                UpdateConfigSettings();
            postProcessing.colorMatrix[2, 0] = (float)blueRed_nud.Value;
        }

        private void blueGreen_nud_KeyDown(object sender, KeyEventArgs e)
        {
                ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._BGCompensation.val  = blueGreen_nud.Value.ToString();
                UpdateConfigSettings();
            postProcessing.colorMatrix[2, 1] = (float)blueGreen_nud.Value;
        }

        private void blueBlue_nud_KeyDown(object sender, KeyEventArgs e)
        {

                ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._BBCompensation.val = blueBlue_nud.Value.ToString();
                UpdateConfigSettings();
            postProcessing.colorMatrix[2, 2] = (float)blueBlue_nud.Value;
        }
        #endregion

        #region Mask Settings Numeric UpDown Value Changed Events
        private void maskX_nud_ValueChanged(object sender, EventArgs e)
        {
                ConfigVariables.CurrentSettings.CameraSettings._ImageOpticalCentreX.val= maskX_nud.Value.ToString();
                UpdateConfigSettings();
                ivl_camera.camPropsHelper._Settings.PostProcessingSettings.maskSettings.maskCentreX = (int)maskX_nud.Value;
            drawRadiusForHotSpot();
        }

        private void maskY_nud_ValueChanged(object sender, EventArgs e)
        {
                ConfigVariables.CurrentSettings.CameraSettings._ImageOpticalCentreY.val= maskY_nud.Value.ToString();
                UpdateConfigSettings();
                ivl_camera.camPropsHelper._Settings.PostProcessingSettings.maskSettings.maskCentreY = (int)maskY_nud.Value;

            drawRadiusForHotSpot();

        }

        private void maskWidth_nud_ValueChanged(object sender, EventArgs e)
        {
                ConfigVariables.CurrentSettings.PostProcessingSettings.MaskSettings.LiveMaskWidth.val = LiveMaskWidth_nud.Value.ToString();
                UpdateConfigSettings();
                ivl_camera.camPropsHelper._Settings.PostProcessingSettings.maskSettings.LiveMaskWidth = (int)LiveMaskWidth_nud.Value;
            drawRadiusForHotSpot();
        }

        private void maskHeight_nud_ValueChanged(object sender, EventArgs e)
        {
                ConfigVariables.CurrentSettings.PostProcessingSettings.MaskSettings.LiveMaskHeight.val= LiveMaskHeight_nud.Value.ToString();
                UpdateConfigSettings();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.maskSettings.LiveMaskHeight = (int)LiveMaskHeight_nud.Value;

            drawRadiusForHotSpot();

        }

        private void CaptureMaskWidth_nud_ValueChanged(object sender, EventArgs e)
        {

            {
                ConfigVariables.CurrentSettings.PostProcessingSettings.MaskSettings.CaptureMaskWidth.val = CaptureMaskWidth_nud.Value.ToString();
                UpdateConfigSettings();

            }
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.maskSettings.maskWidth = (int)CaptureMaskWidth_nud.Value;

            drawRadiusForHotSpot();
        }

        private void CaptureMaskHeight_nud_ValueChanged(object sender, EventArgs e)
        {

            {
                ConfigVariables.CurrentSettings.PostProcessingSettings.MaskSettings.CaptureMaskHeight.val = CaptureMaskHeight_nud.Value.ToString();
                UpdateConfigSettings();
            }
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.maskSettings.maskHeight = (int)CaptureMaskHeight_nud.Value;
            drawRadiusForHotSpot();
        }

        private void LiveMask_cbx_CheckedChanged(object sender, EventArgs e)
        {

            {
                ConfigVariables.CurrentSettings.PostProcessingSettings.MaskSettings._ApplyLiveMask.val = LiveMask_cbx.Checked.ToString();
                UpdateConfigSettings();
            }
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.maskSettings.isApplyLiveMask = LiveMask_cbx.Checked;
        }

        #endregion

        #region HotSpot Settings Value Changed Events
        private void HsRedPeak_nud_ValueChanged(object sender, EventArgs e)
        {
            ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotRedPeak.val = HsRedPeak_nud.Value.ToString();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.hotspotSettings.HotSpotRedPeak = (int)HsRedPeak_nud.Value;
            UpdateConfigSettings();
        }

        private void HsGreenPeak_nud_ValueChanged(object sender, EventArgs e)
        {
            ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotGreenPeak.val = HsGreenPeak_nud.Value.ToString();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.hotspotSettings.HotSpotGreenPeak = (int)HsGreenPeak_nud.Value;
            UpdateConfigSettings();
        }

        private void HsBluePeak_nud_ValueChanged(object sender, EventArgs e)
        {
            ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotBluePeak.val = HsBluePeak_nud.Value.ToString();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.hotspotSettings.HotSpotBluePeak = (int)HsBluePeak_nud.Value;
            UpdateConfigSettings();
        }

        private void HsRedRadius_nud_ValueChanged(object sender, EventArgs e)
        {
            ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotRedRadius.val = HsRedRadius_nud.Value.ToString();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.hotspotSettings.HotSpotRedRadius = (int)HsRedRadius_nud.Value;
            UpdateConfigSettings();
        }

        private void HsGreenRadius_nud_ValueChanged(object sender, EventArgs e)
        {

            {
                ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotGreenRadius.val = HsGreenRadius_nud.Value.ToString();
                ivl_camera.camPropsHelper._Settings.PostProcessingSettings.hotspotSettings.HotSpotGreenRadius = (int)HsGreenRadius_nud.Value;
                UpdateConfigSettings();

            }
        }

        private void HsBlueRadius_nud_ValueChanged(object sender, EventArgs e)
        {

            {
                ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotBlueRadius.val = HsBlueRadius_nud.Value.ToString();
                ivl_camera.camPropsHelper._Settings.PostProcessingSettings.hotspotSettings.HotSpotBlueRadius = (int)HsBlueRadius_nud.Value;
                UpdateConfigSettings();

            }
        }

        private void shadowRedPeak_nud_ValueChanged(object sender, EventArgs e)
        {

            {
                ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowRedPercentage.val = shadowRedPeak_nud.Value.ToString();
                ivl_camera.camPropsHelper._Settings.PostProcessingSettings.hotspotSettings.ShadowRedPeakPercentage = (int)shadowRedPeak_nud.Value;
                UpdateConfigSettings();

            }
        }

        private void shadowGreenPeak_nud_ValueChanged(object sender, EventArgs e)
        {

            {
                ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowGreenPercentage.val = shadowGreenPeak_nud.Value.ToString();
                ivl_camera.camPropsHelper._Settings.PostProcessingSettings.hotspotSettings.ShadowGreenPeakPercentage = (int)shadowGreenPeak_nud.Value;
                UpdateConfigSettings();

            }
        }

        private void shadowBluePeak_nud_ValueChanged(object sender, EventArgs e)
        {

            {
                ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowBlueBPercentage.val = shadowBluePeak_nud.Value.ToString();
                ivl_camera.camPropsHelper._Settings.PostProcessingSettings.hotspotSettings.ShadowBluePeakPercentage = (int)shadowBluePeak_nud.Value;
                UpdateConfigSettings();

            }
        }
        #endregion

        #region LUT Settings for All Channels
        private void LUT_SineFactor_nud_ValueChanged(object sender, EventArgs e)
        {

            {
                ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings._LUTSineFactor.val = LUT_SineFactor_nud.Value.ToString();
                ivl_camera.camPropsHelper._Settings.PostProcessingSettings.lutSettings.LUT_SineFactor = (int)LUT_SineFactor_nud.Value;
                postProcessing.CalculateChannelWiseLUT((int)LUT_interval1_nud.Value, (int)LUT_interval2_nud.Value, (int)LUT_SineFactor_nud.Value, 8, false, (int)offset1_nud.Value, false, 0);

                UpdateConfigSettings();

            }
        }

        private void LUT_interval1_nud_ValueChanged(object sender, EventArgs e)
        {

            {
                ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings._LUTInterval1.val = LUT_interval1_nud.Value.ToString();
                ivl_camera.camPropsHelper._Settings.PostProcessingSettings.lutSettings.LUT_interval1 = (int)LUT_interval1_nud.Value;
                postProcessing.CalculateChannelWiseLUT((int)LUT_interval1_nud.Value, (int)LUT_interval2_nud.Value, (int)LUT_SineFactor_nud.Value, 8, false, (int)offset1_nud.Value, false, 0);

                UpdateConfigSettings();

            }
        }

        private void LUT_interval2_nud_ValueChanged(object sender, EventArgs e)
        {

            {
                ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings._LUTInterval2.val = LUT_interval2_nud.Value.ToString();
                ivl_camera.camPropsHelper._Settings.PostProcessingSettings.lutSettings.LUT_interval2 = (int)LUT_interval2_nud.Value;
                postProcessing.CalculateChannelWiseLUT((int)LUT_interval1_nud.Value, (int)LUT_interval2_nud.Value, (int)LUT_SineFactor_nud.Value, 8, false, (int)offset1_nud.Value, false, 0);


                UpdateConfigSettings();

            }
        }

        private void showLut_btn_Click(object sender, EventArgs e)
        {
            lutForm lut = new lutForm();
            lut.Show();
        }

        private void offset1_nud_ValueChanged(object sender, EventArgs e)
        {

            {
                ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings._LUTOffset.val = offset1_nud.Value.ToString();
                ivl_camera.camPropsHelper._Settings.PostProcessingSettings.lutSettings.LUT_Offset = (int)offset1_nud.Value;
                postProcessing.CalculateChannelWiseLUT((int)LUT_interval1_nud.Value, (int)LUT_interval2_nud.Value, (int)LUT_SineFactor_nud.Value, 8, false, (int)offset1_nud.Value, false, 0);
                UpdateConfigSettings();

            }
        }
        #endregion

        #region Channel Wise LUT Numeric UpDown Value Changed Events
        private void redChannelSineFactor_nud_ValueChanged(object sender, EventArgs e)
        {
            ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_SineFactor_R.val = redChannelSineFactor_nud.Value.ToString();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.lutSettings.LUTR.LUT_SineFactor = (int)redChannelSineFactor_nud.Value;
            UpdateConfigSettings();
            postProcessing.CalculateChannelWiseLUT((int)redChannelLutInterval1_nud.Value, (int)redChannelLutInterval2_nud.Value, (int)redChannelSineFactor_nud.Value, 8, false, (int)redChannelLutOffset_nud.Value, true, 1);

        }

        private void redChannelLutInterval1_nud_ValueChanged(object sender, EventArgs e)
        {
            ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_interval1_R.val = redChannelLutInterval1_nud.Value.ToString();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.lutSettings.LUTR.LUT_interval1 = (int)redChannelLutInterval1_nud.Value;
            UpdateConfigSettings();
            postProcessing.CalculateChannelWiseLUT((int)redChannelLutInterval1_nud.Value, (int)redChannelLutInterval2_nud.Value, (int)redChannelSineFactor_nud.Value, 8, false, (int)redChannelLutOffset_nud.Value, true, 1);

        }

        private void redChannelLutInterval2_nud_ValueChanged(object sender, EventArgs e)
        {
            ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_interval2_R.val = redChannelLutInterval2_nud.Value.ToString();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.lutSettings.LUTR.LUT_interval2 = (int)redChannelLutInterval2_nud.Value;
            UpdateConfigSettings();
            postProcessing.CalculateChannelWiseLUT((int)redChannelLutInterval1_nud.Value, (int)redChannelLutInterval2_nud.Value, (int)redChannelSineFactor_nud.Value, 8, false, (int)redChannelLutOffset_nud.Value, true, 1);

        }

        private void redChannelLutOffset_nud_ValueChanged(object sender, EventArgs e)
        {
            ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_Offset_R.val = redChannelLutInterval2_nud.Value.ToString();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.lutSettings.LUTR.LUT_Offset = (int)redChannelLutInterval2_nud.Value;
            UpdateConfigSettings();
            postProcessing.CalculateChannelWiseLUT((int)redChannelLutInterval1_nud.Value, (int)redChannelLutInterval2_nud.Value, (int)redChannelSineFactor_nud.Value, 8, false, (int)redChannelLutOffset_nud.Value, true, 1);

        }

        private void greenChannelSineFactor_nud_ValueChanged(object sender, EventArgs e)
        {
            ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_SineFactor_G.val = greenChannelSineFactor_nud.Value.ToString();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.lutSettings.LUTG.LUT_SineFactor = (int)greenChannelSineFactor_nud.Value;
            UpdateConfigSettings();
            postProcessing.CalculateChannelWiseLUT((int)greenChannelLutInterval1_nud.Value, (int)greenChannelLutInterval2_nud.Value, (int)greenChannelSineFactor_nud.Value, 8, false, (int)greenChannelLutOffset_nud.Value, true, 2);

        }

        private void greenChannelLutInterval1_nud_ValueChanged(object sender, EventArgs e)
        {
            ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_interval1_G.val = greenChannelLutInterval1_nud.Value.ToString();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.lutSettings.LUTG.LUT_interval1 = (int)greenChannelLutInterval1_nud.Value;
            UpdateConfigSettings();
            postProcessing.CalculateChannelWiseLUT((int)greenChannelLutInterval1_nud.Value, (int)greenChannelLutInterval2_nud.Value, (int)greenChannelSineFactor_nud.Value, 8, false, (int)greenChannelLutOffset_nud.Value, true, 2);

        }

        private void greenChannelLutInterval2_nud_ValueChanged(object sender, EventArgs e)
        {
            ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_interval2_G.val = greenChannelLutInterval2_nud.Value.ToString();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.lutSettings.LUTG.LUT_interval2 = (int)greenChannelLutInterval2_nud.Value;
            UpdateConfigSettings();
            postProcessing.CalculateChannelWiseLUT((int)greenChannelLutInterval1_nud.Value, (int)greenChannelLutInterval2_nud.Value, (int)greenChannelSineFactor_nud.Value, 8, false, (int)greenChannelLutOffset_nud.Value, true, 2);

        }

        private void greenChannelLutOffset_nud_ValueChanged(object sender, EventArgs e)
        {
            ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_Offset_G.val = greenChannelLutOffset_nud.Value.ToString();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.lutSettings.LUTG.LUT_Offset = (int)greenChannelLutOffset_nud.Value;
            UpdateConfigSettings();
            postProcessing.CalculateChannelWiseLUT((int)greenChannelLutInterval1_nud.Value, (int)greenChannelLutInterval2_nud.Value, (int)greenChannelSineFactor_nud.Value, 8, false, (int)greenChannelLutOffset_nud.Value, true, 2);

        }

        private void blueChannelSineFactor_nud_ValueChanged(object sender, EventArgs e)
        {
            ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_SineFactor_B.val = blueChannelSineFactor_nud.Value.ToString();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.lutSettings.LUTB.LUT_SineFactor = (int)blueChannelSineFactor_nud.Value;
            UpdateConfigSettings();
            postProcessing.CalculateChannelWiseLUT((int)blueChannelLutInterval1_nud.Value, (int)blueChannelLutInterval2_nud.Value, (int)blueChannelSineFactor_nud.Value, 8, false, (int)blueChannelLutOffset_nud.Value, true, 3);

        }

        private void blueChannelLutInterval1_nud_ValueChanged(object sender, EventArgs e)
        {
            ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_interval1_B.val = blueChannelLutInterval1_nud.Value.ToString();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.lutSettings.LUTB.LUT_interval1 = (int)blueChannelLutInterval1_nud.Value;
            UpdateConfigSettings();
            postProcessing.CalculateChannelWiseLUT((int)blueChannelLutInterval1_nud.Value, (int)blueChannelLutInterval2_nud.Value, (int)blueChannelSineFactor_nud.Value, 8, false, (int)blueChannelLutOffset_nud.Value, true, 3);

        }

        private void blueChannelLutInterval2_nud_ValueChanged(object sender, EventArgs e)
        {
            ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_interval2_B.val = blueChannelLutInterval2_nud.Value.ToString();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.lutSettings.LUTB.LUT_interval2 = (int)blueChannelLutInterval2_nud.Value;
            UpdateConfigSettings();
            postProcessing.CalculateChannelWiseLUT((int)blueChannelLutInterval1_nud.Value, (int)blueChannelLutInterval2_nud.Value, (int)blueChannelSineFactor_nud.Value, 8, false, (int)blueChannelLutOffset_nud.Value, true, 3);

        }

        private void blueChannelLutOffset_nud_ValueChanged(object sender, EventArgs e)
        {
            ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_Offset_B.val = blueChannelLutOffset_nud.Value.ToString();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.lutSettings.LUTB.LUT_Offset = (int)blueChannelLutOffset_nud.Value;
            UpdateConfigSettings();
            postProcessing.CalculateChannelWiseLUT((int)blueChannelLutInterval1_nud.Value, (int)blueChannelLutInterval2_nud.Value, (int)blueChannelSineFactor_nud.Value, 8, false, (int)blueChannelLutOffset_nud.Value, true, 3);

        }

        private void channelWiseLutCbx_CheckedChanged(object sender, EventArgs e)
        {
            //if (channelWiseLutCbx.Checked)
            //    PostProcessing.isChannelWise = true;
            //else
            PostProcessing.isChannelWise = false;
        }

        private void applyChannelWiseLutLklbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            control_tb.SelectedTab = allChannelLut_tbp;
        }

        private void allChannelLutCbx_CheckedChanged(object sender, EventArgs e)
        {
            if (allChannelLutCbx.Checked)
            {
                isApplyLut_cbx.Enabled = false;
                linkLabel2.Enabled = false;
                PostProcessing.isChannelWise = true;
                ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings._IsChannelWiseLUT.val = allChannelLutCbx.Checked.ToString();
                ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings._IsApplyLutSettings.val = allChannelLutCbx.Checked.ToString();
                UpdateConfigSettings();
                ivl_camera.camPropsHelper._Settings.PostProcessingSettings.lutSettings.isChannelWiseLUT = allChannelLutCbx.Checked;
                ivl_camera.camPropsHelper._Settings.PostProcessingSettings.lutSettings.isApplyLUT = allChannelLutCbx.Checked;
            }
            else
            {
                isApplyLut_cbx.Enabled = true;
                linkLabel2.Enabled = true;
                PostProcessing.isChannelWise = false;
                ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings._IsChannelWiseLUT.val = allChannelLutCbx.Checked.ToString();
                ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings._IsApplyLutSettings.val = allChannelLutCbx.Checked.ToString();
                UpdateConfigSettings();
                ivl_camera.camPropsHelper._Settings.PostProcessingSettings.lutSettings.isChannelWiseLUT = allChannelLutCbx.Checked;
                ivl_camera.camPropsHelper._Settings.PostProcessingSettings.lutSettings.isApplyLUT = allChannelLutCbx.Checked;
            }
        }
        #endregion

        private void control_tb_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (control_tb.SelectedTab != control_tb.TabPages["HotSpot_tbp"])
            //    ClearHotSpotMarking();
            //else
            drawRadiusForHotSpot();
        }

        private void applyClahe_cbx_CheckedChanged(object sender, EventArgs e)
        {
            ConfigVariables.CurrentSettings.PostProcessingSettings.ClaheSettings._IsApplyClaheSettings.val = applyClahe_cbx.Checked.ToString();
            UpdateConfigSettings();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.claheSettings.isApplyClahe = applyClahe_cbx.Checked;
        }


        private void SetGetRGBGain(bool isCaptureGain)
        {
            try
            {
                if (ivl_camera != null)
                {
                    int[] RGBGain = new int[3];
                    if (isCaptureGain)
                    {
                        RGBGain[0] = redGain_tb.Value;
                        RGBGain[1] = greenGain_tb.Value;
                        RGBGain[2] = blueGain_tb.Value;
                        ivl_camera.camPropsHelper.SetRGBGain(RGBGain);
                        ivl_camera.camPropsHelper.GetRGBGain(RGBGain);
                        redGainVal_lbl.Text = RGBGain[0].ToString();// = redGain_tb.Value;
                        greenGainVal_lbl.Text = RGBGain[1].ToString();// = redGain_tb.Value;
                        blueGainVal_lbl.Text = RGBGain[2].ToString();// = redGain_tb.Value;
                    }
                    else
                    {
                        RGBGain[0] = liveRedGain_tb.Value;
                        RGBGain[1] = liveGreenGain_tb.Value;
                        RGBGain[2] = liveBlueGain_tb.Value;
                        ivl_camera.camPropsHelper.SetRGBGain(RGBGain);
                        ivl_camera.camPropsHelper.GetRGBGain(RGBGain);
                        liveR_val_lbl.Text = RGBGain[0].ToString();// = redGain_tb.Value;
                        liveG_val_lbl.Text = RGBGain[1].ToString();// = redGain_tb.Value;
                        liveB_val_lbl.Text = RGBGain[2].ToString();// = redGain_tb.Value;
                    }

                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        #region Scroll Events
        private void redGain_tb_Scroll_2(object sender, EventArgs e)
        {
                ConfigVariables.CurrentSettings.CameraSettings.RedAnalogGain.val= redGain_tb.Value.ToString();

                UpdateConfigSettings();
            SetGetRGBGain(true);

        }
        private void greenGain_tb_Scroll_2(object sender, EventArgs e)
        {
                ConfigVariables.CurrentSettings.CameraSettings.GreenAnalogGain.val= greenGain_tb.Value.ToString();

                UpdateConfigSettings();
            SetGetRGBGain(true);

        }

        private void blueGain_tb_Scroll_2(object sender, EventArgs e)
        {
                ConfigVariables.CurrentSettings.CameraSettings.BlueAnalogGain.val= blueGain_tb.Value.ToString();

                UpdateConfigSettings();
            SetGetRGBGain(true);

        }
        #endregion

        #region Commented Events
        private void liveRedGain_tb_Scroll(object sender, EventArgs e)
        {
            //
            //{
            //    ConfigVariables.CurrentSettings.analogGainSettings.liveR = (int)liveRedGain_tb.Value;

            //    UpdateConfigSettings();

            //}
            //SetGetRGBGain(false);
        }

        private void liveGreenGain_tb_Scroll(object sender, EventArgs e)
        {
            //
            //{
            //    ConfigVariables.CurrentSettings.analogGainSettings.liveG = (int)liveGreenGain_tb.Value;
            //    UpdateConfigSettings();

            //}
            //SetGetRGBGain(false);
        }

        private void liveBlueGain_tb_Scroll(object sender, EventArgs e)
        {
            //
            //{
            //    ConfigVariables.CurrentSettings.analogGainSettings.liveB = (int)liveBlueGain_tb.Value;

            //    UpdateConfigSettings();

            //}
            //SetGetRGBGain(false);
        }

        private void saveLiveFrame_btn_Click(object sender, EventArgs e)
        {
            if (ivl_camera != null)
            {

                //ivl_camera.SaveImage(0, saveDinf.FullName + Path.DirectorySeparatorChar + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".png");
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //ConfigVariables.CurrentSettings.LowFlash = low_flash_rb.Checked;
            //UpdateConfigSettings();
            //if (low_flash_rb.Checked)
            //{

            //    ivl_camera.EnableFlashControlUsingKnob();

            //}
        }
        #endregion






        private void MotorPosReset_btn_Click(object sender, EventArgs e)
        {
            ivl_camera.camPropsHelper.ResetMotorPosition();
            ResetTimer.Interval = Math.Abs(prevSensorPos) * 10 + 25;
            ResetTimer.Enabled = true;
            ResetTimer.Start();
        }

        private void GetHoSpotParams_btn_Click(object sender, EventArgs e)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(fileNameMask);
                FileInfo[] arr = fileInfo.Directory.GetFiles("*.png");
                int count = 0;
                StreamWriter stWriterBrightness = new StreamWriter(fileInfo.Directory.FullName + Path.DirectorySeparatorChar + "Save_Brightness " + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".csv");
                StreamWriter stWriterEntropy = new StreamWriter(fileInfo.Directory.FullName + Path.DirectorySeparatorChar + "Save_Entropy" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".csv");
                StreamWriter stWriterContrrast = new StreamWriter(fileInfo.Directory.FullName + Path.DirectorySeparatorChar + "Save_Contrast" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".csv");
                StreamWriter stWriterSTD = new StreamWriter(fileInfo.Directory.FullName + Path.DirectorySeparatorChar + "Save_STD" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".csv");
                StreamWriter stWriterFocus = new StreamWriter(fileInfo.Directory.FullName + Path.DirectorySeparatorChar + "Save_Focus" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".csv");
                StreamWriter ResultCategorizationWriter = new StreamWriter(fileInfo.Directory.FullName + Path.DirectorySeparatorChar + "ResultCategory" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".csv");
                stWriterEntropy.WriteLine("Name, r1EntropyBlue, r1EntropyGreen, r1EntropyRed, r2EntropyBlue, r2EntropyGreen, r2EntropyRed, r3EntropyBlue, r3EntropyGreen, r3EntropyRed,r4EntropyBlue,r4EntropyGreen,r4EntropyRed");
                stWriterSTD.WriteLine("Name, r1STDBlue, r1STDGreen, r2STDRed, r2STDBlue, r2STDGreen, r2STDRed, r3STDBlue, r3STDGreen, r3STDRed,r4Blue,r4STDGreen,r4STDRed");
                stWriterContrrast.WriteLine("Name,r1contrastBlue,r1contrastGreen,r1contrastRed,r2contrastBlue,r2contrastGreen,r2contrastRed,r3contrastBlue,r3contrastGreen,r3contrastRed,r4ContrastBlue,r4ContrastGreen,r4ContrastRed");
                stWriterBrightness.WriteLine("Name,r1brightnessBlue,r1brightnessGreen,r1brightnessRed,r2brightnessBlue,r2brightnessGreen,r2brightnessRed,r3brightnessBlue,r3brightnessGreen,r3brightnessRed,r4brightnessBlue,r4brightnessGreen,r4brightnessRed");
                stWriterFocus.WriteLine("Name,FocusBlue,FocusGreen,FocusRed,");
                ResultCategorizationWriter.WriteLine("Name, Category,Brightness,Focus,std,cov,ent");
                string goodImgDirInf = fileInfo.Directory.FullName + Path.DirectorySeparatorChar + "GoodImages";
                string BadImgDirInf = fileInfo.Directory.FullName + Path.DirectorySeparatorChar + "BadImages";
                if (!Directory.Exists(goodImgDirInf))
                    Directory.CreateDirectory(goodImgDirInf);
                if (!Directory.Exists(BadImgDirInf))
                    Directory.CreateDirectory(BadImgDirInf);
                foreach (FileInfo item in arr)
                {

                    colorBm = new Bitmap(item.FullName);
                    unsafe
                    {
                        if (colorBm != null)
                        {
                            bool isColor = true;
                            if (imagingMode == INTUSOFT.Imaging.ImagingMode.FFA_Plus)
                                isColor = false;
                            postProcessing.ApplyPostProcessing(ref colorBm,isColor);

                            //postProcessing.CalculateHotspotParams(ref colorBm, (int)maskX_nud.Value, (int)maskY_nud.Value, fileInfo.Directory.FullName);
                            stWriterEntropy.WriteLine(item.Name + "," + postProcessing.hotSpotParams->r1Entropy[0] + "," + postProcessing.hotSpotParams->r1Entropy[1] + "," + postProcessing.hotSpotParams->r1Entropy[2]
                                + "," + postProcessing.hotSpotParams->r2Entropy[0] + "," + postProcessing.hotSpotParams->r2Entropy[1] + "," + postProcessing.hotSpotParams->r2Entropy[2]
                                    + "," + postProcessing.hotSpotParams->r3Entropy[0] + "," + postProcessing.hotSpotParams->r3Entropy[1] + "," + postProcessing.hotSpotParams->r3Entropy[2]
                                    + "," + postProcessing.hotSpotParams->r4Entropy[0] + "," + postProcessing.hotSpotParams->r4Entropy[1] + "," + postProcessing.hotSpotParams->r4Entropy[2]);
                            stWriterSTD.WriteLine(item.Name + "," + postProcessing.hotSpotParams->r1STD[0] + "," + postProcessing.hotSpotParams->r1STD[1] + "," + postProcessing.hotSpotParams->r1STD[2]
                                + "," + postProcessing.hotSpotParams->r2STD[0] + "," + postProcessing.hotSpotParams->r2STD[1] + "," + postProcessing.hotSpotParams->r2STD[2]
                                    + "," + postProcessing.hotSpotParams->r3STD[0] + "," + postProcessing.hotSpotParams->r3STD[1] + "," + postProcessing.hotSpotParams->r3STD[2]
                                    + "," + postProcessing.hotSpotParams->r4STD[0] + "," + postProcessing.hotSpotParams->r4STD[1] + "," + postProcessing.hotSpotParams->r4STD[2]);

                            stWriterContrrast.WriteLine(item.Name + "," + postProcessing.hotSpotParams->r1Contrast[0] + "," + postProcessing.hotSpotParams->r1Contrast[1] + "," + postProcessing.hotSpotParams->r1Contrast[2]
                                + "," + postProcessing.hotSpotParams->r2Contrast[0] + "," + postProcessing.hotSpotParams->r2Contrast[1] + "," + postProcessing.hotSpotParams->r2Contrast[2]
                                    + "," + postProcessing.hotSpotParams->r3Contrast[0] + "," + postProcessing.hotSpotParams->r3Contrast[1] + "," + postProcessing.hotSpotParams->r3Contrast[2]
                                    + "," + postProcessing.hotSpotParams->r4Contrast[0] + "," + postProcessing.hotSpotParams->r4Contrast[1] + "," + postProcessing.hotSpotParams->r4Contrast[2]);

                            stWriterBrightness.WriteLine(item.Name + "," + postProcessing.hotSpotParams->r1Brighntess[0] + "," + postProcessing.hotSpotParams->r1Brighntess[1] + "," + postProcessing.hotSpotParams->r1Brighntess[2]
                                + "," + postProcessing.hotSpotParams->r2Brighntess[0] + "," + postProcessing.hotSpotParams->r2Brighntess[1] + "," + postProcessing.hotSpotParams->r2Brighntess[2]
                                    + "," + postProcessing.hotSpotParams->r3Brighntess[0] + "," + postProcessing.hotSpotParams->r3Brighntess[1] + "," + postProcessing.hotSpotParams->r3Brighntess[2]
                                    + "," + postProcessing.hotSpotParams->r4Brighntess[0] + "," + postProcessing.hotSpotParams->r4Brighntess[1] + "," + postProcessing.hotSpotParams->r4Brighntess[2]);

                            stWriterFocus.WriteLine(item.Name + "," + postProcessing.hotSpotParams->FocusVal[0] + "," + postProcessing.hotSpotParams->FocusVal[1] + "," + postProcessing.hotSpotParams->FocusVal[2]);
                            if (postProcessing.hotSpotParams->r3Brighntess[2] > 60 && postProcessing.hotSpotParams->r3Brighntess[2] < 220)
                            {
                                if (postProcessing.hotSpotParams->FocusVal[2] > 2500 && postProcessing.hotSpotParams->r3Entropy[1] > 4 && postProcessing.hotSpotParams->r3STD[1] < 50 && postProcessing.hotSpotParams->r3Contrast[1] > 0.2)
                                {
                                    ResultCategorizationWriter.WriteLine(item.Name + "," + "Good Image," + postProcessing.hotSpotParams->r3Brighntess[2] + "," + postProcessing.hotSpotParams->FocusVal[2] + "," + postProcessing.hotSpotParams->r3STD[1] + "," + postProcessing.hotSpotParams->r3Contrast[1] + "," + postProcessing.hotSpotParams->r3Entropy[1]);
                                    colorBm.Save(goodImgDirInf + Path.DirectorySeparatorChar + item.Name);
                                }
                                else
                                {
                                    ResultCategorizationWriter.WriteLine(item.Name + "," + "Bad Image," + postProcessing.hotSpotParams->r3Brighntess[2] + "," + postProcessing.hotSpotParams->FocusVal[2] + "," + postProcessing.hotSpotParams->r3STD[1] + "," + postProcessing.hotSpotParams->r3Contrast[1] + "," + postProcessing.hotSpotParams->r3Entropy[1]);
                                    colorBm.Save(BadImgDirInf + Path.DirectorySeparatorChar + item.Name);

                                }
                            }
                            else
                            {
                                ResultCategorizationWriter.WriteLine(item.Name + "," + "Bad Image," + postProcessing.hotSpotParams->r3Brighntess[2] + "," + postProcessing.hotSpotParams->FocusVal[2] + "," + postProcessing.hotSpotParams->r3STD[1] + "," + postProcessing.hotSpotParams->r3Contrast[1] + "," + postProcessing.hotSpotParams->r3Entropy[1]);
                                colorBm.Save(BadImgDirInf + Path.DirectorySeparatorChar + item.Name);

                            }
                        }
                        colorBm.Dispose();

                        count++;
                    }
                }


                stWriterBrightness.Close();
                stWriterBrightness.Dispose();
                stWriterContrrast.Close();
                stWriterContrrast.Dispose();
                stWriterEntropy.Close();
                stWriterEntropy.Dispose();
                stWriterSTD.Close();
                stWriterSTD.Dispose();
                stWriterFocus.Close();
                stWriterFocus.Dispose();
                ResultCategorizationWriter.Close();
                ResultCategorizationWriter.Dispose();
                MessageBox.Show("Writing Completed");
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void neg_pbx_SizeChanged(object sender, EventArgs e)
        {
            RBwidth = pos_pbx.Width;
            RBheight = pos_pbx.Height;
            LBwidth = neg_pbx.Width;
            LBheight = neg_pbx.Height;
            LeftBitmap = new Bitmap(neg_pbx.Width, neg_pbx.Height);// Added this to manage the blinking happening in the UI to show the sensor position when the rotary is moved instead of using the panel drawing
            RightBitmap = new Bitmap(pos_pbx.Width, pos_pbx.Height);// Added this to manage the blinking happening in the UI to show the sensor position when the rotary is moved instead of using the panel drawing
        }

        private void ClearMaskMarking_btn_Click(object sender, EventArgs e)
        {
            ClearHotSpotMarking();
        }

        private void batchMask_btn_Click(object sender, EventArgs e)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(fileNameMask);
                FileInfo[] arr = fileInfo.Directory.GetFiles("*.png");
                string maskSaveDirName = fileInfo.Directory.FullName + Path.DirectorySeparatorChar + "MaskedImages" + Path.DirectorySeparatorChar;
                if (!Directory.Exists(fileInfo.Directory.FullName + Path.DirectorySeparatorChar + "MaskedImages"))
                    Directory.CreateDirectory(fileInfo.Directory.FullName + Path.DirectorySeparatorChar + "MaskedImages");
                int count = 0;
                foreach (FileInfo item in arr)
                {
                    colorBm = new Bitmap(item.FullName);
                    applyMask_btn_Click(null, null);
                    colorBm.Save(maskSaveDirName + item.Name);
                    count++;
                    label74.Text = count.ToString();
                }
                label74.Text = "Masking Completed";
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }


        #region HSV Boost Events
        private void ApplyHSVBoost_cbx_CheckedChanged(object sender, EventArgs e)
        {
            ConfigVariables.CurrentSettings.PostProcessingSettings.ImageShiftSettings.IsApplyHSV.val = ApplyHSVBoost_cbx.Checked.ToString();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.isApplyHSVBoost = ApplyHSVBoost_cbx.Checked;
            UpdateConfigSettings();
        }


        private void hsvBoostVal_nud_ValueChanged(object sender, EventArgs e)
        {
            ConfigVariables.CurrentSettings.PostProcessingSettings.ImageShiftSettings.HsvBoost.val = hsvBoostVal_nud.Value.ToString();
            UpdateConfigSettings();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.hsvValue = (float)hsvBoostVal_nud.Value;
            //ApplyPostProcessing(ref colorBm);
        }
        #endregion


        

        private void formButtons1_Click_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(fileNameMask))
            {
                colorBm = new Bitmap(fileNameMask);
                postProcessing.CalculateHotspotParams(ref colorBm, (int)maskX_nud.Value, (int)maskY_nud.Value, "");

            }

        }
        bool isPause = false;

        #region Frane Detection Events
        private void frameDetectionVal_nud_ValueChanged(object sender, EventArgs e)
        {
            if (ivl_camera != null)
                ivl_camera.camPropsHelper._Settings.CameraSettings.FrameDetectionValue = (float)frameDetectionVal_nud.Value;
        }

        private void darkFameDetectionVal_nud_ValueChanged(object sender, EventArgs e)
        {
            if (ivl_camera != null)
                ivl_camera.camPropsHelper._Settings.CameraSettings.DarkFrameDetectionVal = (int)darkFameDetectionVal_nud.Value;
        }
        #endregion

        private void pause_btn_Click(object sender, EventArgs e)
        {

        }

        #region CheckBox Check Changed events
        private void saveRaw_cbx_CheckedChanged(object sender, EventArgs e)
        {
            
            {
                ConfigVariables.CurrentSettings.ImageStorageSettings._IsRawSave.val= saveRaw_cbx.Checked.ToString();
                UpdateConfigSettings();
            }
            if (ivl_camera != null)
                ivl_camera.camPropsHelper._Settings.ImageSaveSettings.isRawSave = saveRaw_cbx.Checked;

        }

        private void SaveProcessedImage_cbx_CheckedChanged(object sender, EventArgs e)
        {
            
            {
                ConfigVariables.CurrentSettings.ImageStorageSettings._IsProcessedImageSave.val= SaveProcessedImage_cbx.Checked.ToString();
                UpdateConfigSettings();
            }
            if (ivl_camera != null)
                ivl_camera.camPropsHelper._Settings.ImageSaveSettings.isSaveProcessedImage = SaveProcessedImage_cbx.Checked;
        }

        private void SaveDebugImage_cbx_CheckedChanged(object sender, EventArgs e)
        {
            
            {
                ConfigVariables.CurrentSettings.ImageStorageSettings._IsRawImageSave.val= SaveDebugImage_cbx.Checked.ToString();
                UpdateConfigSettings();
            }
            if (ivl_camera != null)
                ivl_camera.camPropsHelper._Settings.ImageSaveSettings.isSaveRawImage = SaveDebugImage_cbx.Checked;
        }

        private void EnableTemperatureTint_cbx_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateConfigSettings();

                ivl_camera.camPropsHelper.EnableWhiteBalance(EnableTemperatureTint_cbx.Checked);

                tempTint_gbx.Enabled = EnableTemperatureTint_cbx.Checked;
                if (EnableTemperatureTint_cbx.Checked)
                    ivl_camera.camPropsHelper.SetTemperatureTint(temperature_tb.Value, tint_tb.Value);
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }

        }

        private void formCheckBox1_CheckedChanged_1(object sender, EventArgs e)
        {

            ConfigVariables.CurrentSettings.CameraSettings.EnableCC.val = formCheckBox1.Checked.ToString();
            UpdateConfigSettings();

            ivl_camera.camPropsHelper.EnableCameraColorMatrix(formCheckBox1.Checked);
        }

        private void liveCC_cbx_CheckedChanged(object sender, EventArgs e)
        {

            {
                ConfigVariables.CurrentSettings.PostProcessingSettings.ImageShiftSettings.LiveCC.val = liveCC_cbx.Checked.ToString();
                UpdateConfigSettings();
            }
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.ccSettings.isApplyLiveColorCorrection = liveCC_cbx.Checked;
        }

        private void enableLivePostProcessing_cbx_CheckedChanged(object sender, EventArgs e)
        {

            ConfigVariables.CurrentSettings.PostProcessingSettings.ImageShiftSettings.LivePP.val = enableLivePostProcessing_cbx.Checked.ToString();
            UpdateConfigSettings();
            if (ivl_camera != null)
                ivl_camera.camPropsHelper._Settings.CameraSettings.EnableLiveImageProcessing = enableLivePostProcessing_cbx.Checked;
        }


        #endregion

       

        private void medianfilter_nud_ValueChanged_1(object sender, EventArgs e)
        {
                ConfigVariables.CurrentSettings.PostProcessingSettings.UnsharpMaskSettings._MedFilter.val= medianfilter_nud.Value.ToString();
                UpdateConfigSettings();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.unsharpMaskSettings.medFilterValue = (int)medianfilter_nud.Value;
        }


        #region CLAHE Settings Numeric UpDown Value Changed Events
        private void ClaheClipValueR_nud_ValueChanged(object sender, EventArgs e)
        {
                ConfigVariables.CurrentSettings.PostProcessingSettings.ClaheSettings._ClipValueR.val= ClaheClipValueR_nud.Value.ToString();
                UpdateConfigSettings();
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.claheSettings.clipValR = (float)ClaheClipValueR_nud.Value;
        }

        private void ClaheClipValueG_nud_ValueChanged(object sender, EventArgs e)
        {
            {
                ConfigVariables.CurrentSettings.PostProcessingSettings.ClaheSettings._ClipValueG.val= ClaheClipValueG_nud.Value.ToString();
                UpdateConfigSettings();
            }
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.claheSettings.clipValG = (float)ClaheClipValueG_nud.Value;
        }

        private void ClaheClipValueB_nud_ValueChanged(object sender, EventArgs e)
        {
            {
                ConfigVariables.CurrentSettings.PostProcessingSettings.ClaheSettings._ClipValueB.val = ClaheClipValueB_nud.Value.ToString();
                UpdateConfigSettings();
            }
            ivl_camera.camPropsHelper._Settings.PostProcessingSettings.claheSettings.clipValB = (float)ClaheClipValueB_nud.Value;
        }
        #endregion

        private void tint_tb_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (EnableTemperatureTint_cbx.Checked)
                {
                        ConfigVariables.CurrentSettings.CameraSettings._Tint.val= tint_tb.Value.ToString();
                    UpdateConfigSettings();
                    tintStatus_lbl.Text = tint_tb.Value.ToString();
                    tintVal_lbl.Text = tint_tb.Value.ToString();
                    if (ivl_camera != null)
                    {
                        ivl_camera.camPropsHelper.SetTemperatureTint(temperature_tb.Value, tint_tb.Value);
                        //ivl_camera.GetTemperatureTint(ref tempVal, ref tintVal);
                        int[] RGBGain = new int[3];
                        //ivl_camera.GetTemperatureTint(ref tempVal, ref tintVal);// ref RGBGain);
                        temperaturStatus_lbl.Text = "Temp: " + tempVal.ToString();
                        tintStatus_lbl.Text = "Temp: " + tintVal.ToString();

                    }
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }




        void flowLayoutPanel1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                HarrProgressBar data = (HarrProgressBar)e.Data.GetData(typeof(HarrProgressBar));
                FlowLayoutPanel _destination = (FlowLayoutPanel)sender;
                FlowLayoutPanel _source = (FlowLayoutPanel)data.Parent;

                if (_source != _destination)
                {
                    // Add control to panel
                    _destination.Controls.Add(data);
                    data.Size = new Size(_destination.Width, 50);

                    // Reorder
                    Point p = _destination.PointToClient(new Point(e.X, e.Y));
                    var item = _destination.GetChildAtPoint(p);
                    int index = _destination.Controls.GetChildIndex(item, false);
                    _destination.Controls.SetChildIndex(data, index);

                    // Invalidate to paint!
                    _destination.Invalidate();
                    _source.Invalidate();
                }
                else
                {
                    // Just add the control to the new panel.
                    // No need to remove from the other panel, this changes the Control.Parent property.
                    Point p = _destination.PointToClient(new Point(e.X, e.Y));
                    var item = _destination.GetChildAtPoint(p);
                    int index = _destination.Controls.GetChildIndex(item, false);
                    _destination.Controls.SetChildIndex(data, index);
                    _destination.Invalidate();
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private int FindCSTIndex(Control cst_ctr)
        {
            for (int i = 0; i < this.flowLayoutPanel1.Controls.Count; i++)
            {
                Label target = this.flowLayoutPanel1.Controls[i] as Label;

                if (cst_ctr.Parent == target)
                    return i;
            }
            return -1;
        }

        private void OnCstMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Control cst = sender as Control;
                cst.DoDragDrop(cst.Parent, DragDropEffects.Move);
            }
        }
        private void FFA_Pot_Int_Offset_nud_ValueChanged(object sender, EventArgs e)
        {
            
                ConfigVariables.CurrentSettings.FirmwareSettings.PotOffsetValue.val= FFA_Pot_Int_Offset_nud.Value.ToString();
            UpdateConfigSettings();
            ivl_camera.camPropsHelper._Settings.BoardSettings.FFA_Pot_Int_Offset = (int)FFA_Pot_Int_Offset_nud.Value;
        }





        private void greenFilterPos_nud_ValueChanged(object sender, EventArgs e)
        {
            
                ConfigVariables.CurrentSettings.FirmwareSettings.GreenFilterPos.val = greenFilterPos_nud.Value.ToString();
            UpdateConfigSettings();
            if (ivl_camera != null)
                ivl_camera.camPropsHelper._Settings.BoardSettings.GreenFilterPos = (int)greenFilterPos_nud.Value;
        }

        private void blueFilterPos_nud_ValueChanged(object sender, EventArgs e)
        {
            
                ConfigVariables.CurrentSettings.FirmwareSettings.BlueFilterPos.val = blueFilterPos_nud.Value.ToString();
            UpdateConfigSettings();
            if (ivl_camera != null)
                ivl_camera.camPropsHelper._Settings.BoardSettings.BlueFilterPos = (int)blueFilterPos_nud.Value;
        }

        #region Gamma Settings 
        private void gammaVal_nud_ValueChanged_1(object sender, EventArgs e)
        {
            
                ConfigVariables.CurrentSettings.PostProcessingSettings.GammaSettings.GammaValue.val = gammaVal_nud.Value.ToString();
            UpdateConfigSettings();
            if (ivl_camera != null)
                ivl_camera.camPropsHelper._Settings.PostProcessingSettings.gammaCorrectionSettings.GammaCorrectionValue = (float)gammaVal_nud.Value;
        }

        private void applyGamma_cbx_CheckedChanged(object sender, EventArgs e)
        {
            
                ConfigVariables.CurrentSettings.PostProcessingSettings.GammaSettings.IsApplyGammaSettings.val= applyGamma_cbx.Checked.ToString();
            UpdateConfigSettings();
            if (ivl_camera != null)
                ivl_camera.camPropsHelper._Settings.PostProcessingSettings.gammaCorrectionSettings.ApplyGammaCorrection = applyGamma_cbx.Checked;
        }
        #endregion

        private void hotSpotValues_btn_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    fileNameMask = ofd.FileName;
                else
                    return;
                Image<Bgr, byte> inp = new Image<Bgr, byte>(fileNameMask);
                FileInfo f = new FileInfo(fileNameMask);
                string[] str = f.Name.Split('.');
                StreamWriter stWriterVertical = new StreamWriter(f.DirectoryName + Path.DirectorySeparatorChar + str[0] + "_HotspotVertical.csv");
                stWriterVertical.WriteLine("R,G,B");
                // code for dumping centre values in the vertical centre line
                byte[] arr = new byte[300];
                int count = 0;
                double sum = 0;
                for (int i = (int)HotSpotCentreY_nud.Value - 100; i < (int)HotSpotCentreY_nud.Value + 200; i++)
                {
                    byte valB = inp.Data[i, (int)HotSpotCentreX_nud.Value, 0];
                    byte valG = inp.Data[i, (int)HotSpotCentreX_nud.Value, 1];
                    byte valR = inp.Data[i, (int)HotSpotCentreX_nud.Value, 2];
                    stWriterVertical.WriteLine(valR.ToString() + "  ," + valG.ToString() + "  ," + valB.ToString());
                    //arr.Add(inp.Data[i, (int)HotSpotCentreX_nud.Value, 0]);
                    sum += (double)inp.Data[i, (int)HotSpotCentreX_nud.Value, 0];
                    arr[count++] = inp.Data[i, (int)HotSpotCentreX_nud.Value, 0];

                }
                double avg = sum / (double)count;// arr.ToArray().Average();
                count = 0;
                int indx1 = -1;
                int indx2 = -1;
                int indx3 = -1;
                int indx4 = -1;
                bool index1Found = false;
                bool index2Found = false;
                bool index3Found = false;
                bool index4Found = false;
                double mulFact = 0.7;
                int MaxIndxCnt = 3;
                for (int i = 0; i < arr.Length; i++)
                {
                    if ((double)arr[i] >= mulFact * avg && !index1Found)
                    {
                        if (indx1 < 0)
                        {
                            indx1 = i;
                            count = 0;
                            count++;

                        }
                        else if (i == indx1 + count)
                        {
                            count++;
                            if (count == MaxIndxCnt)
                            {
                                count = 0;
                                index1Found = true;
                            }
                        }
                        else
                        {
                            if (!index1Found)
                            {
                                indx1 = -1;
                                count = 0;
                            }
                        }
                    }
                    else if ((double)arr[i] <= mulFact * avg && indx1 > 0 && index1Found && !index2Found)
                    {
                        if (indx2 < 0)
                        {
                            indx2 = i;
                            count = 0;

                            count++;

                        }
                        else if (i == indx2 + count)
                        {
                            count++;
                            if (count == MaxIndxCnt)
                            {
                                count = 0;
                                index2Found = true;
                            }
                        }
                        else
                        {
                            if (!index2Found)
                            {
                                indx2 = -1;
                                count = 0;
                            }
                        }
                    }
                    else if ((double)arr[i] >= mulFact * avg && indx2 > 0 && index2Found && !index3Found)
                    {
                        if (indx3 < 0)
                        {
                            indx3 = i;
                            count = 0;
                            count++;

                        }
                        else if (i == indx3 + count)
                        {
                            count++;
                            if (count == MaxIndxCnt)
                            {
                                index3Found = true;
                                count = 0;
                            }
                        }
                        else
                        {
                            if (!index3Found)
                            {
                                indx3 = -1;
                                count = 0;
                            }
                        }
                    }
                    else if ((double)arr[i] <= mulFact * avg && indx3 > 0 && !index4Found && index3Found)
                    {
                        if (indx4 < 0)
                        {
                            indx4 = i;
                            count = 0;

                            count++;

                        }
                        else if (i == indx4 + count)
                        {
                            count++;
                            if (count == MaxIndxCnt)
                            {
                                index4Found = true;
                                count = 0;
                                break;
                            }
                        }
                        else
                        {
                            if (!index4Found)
                            {
                                indx4 = -1;
                                count = 0;
                            }
                        }
                    }
                }
                stWriterVertical.Close();


                indx1 += ((int)HotSpotCentreY_nud.Value - 100);
                indx2 += ((int)HotSpotCentreY_nud.Value - 100);
                indx3 += ((int)HotSpotCentreY_nud.Value - 100);
                indx4 += ((int)HotSpotCentreY_nud.Value - 100);

                int radius1 = (int)((double)(indx2 - indx1) / 2.0);
                int radius2 = (int)((double)(indx4 - indx3) / 2.0);
                int centre1Y = (int)HotSpotCentreY_nud.Value + radius1;
                int centre2Y = (int)HotSpotCentreY_nud.Value + radius2;
                colorBm = inp.ToBitmap();// = new Image<Bgr, byte>(fileNameMask);

                Graphics g = Graphics.FromImage(colorBm);
                // g.DrawEllipse(new Pen(Color.AntiqueWhite, 2f), new Rectangle((int)HotSpotCentreX_nud.Value - radius1 / 2, centre1Y, radius1, radius1));
                g.DrawLine(new Pen(Color.AntiqueWhite, 2f), new Point((int)HotSpotCentreX_nud.Value, indx1), new Point((int)HotSpotCentreX_nud.Value, indx2));// new Rectangle((int)HotSpotCentreX_nud.Value - radius1 / 2, centre1Y, radius1, radius1));
                //   g.DrawEllipse(new Pen(Color.AntiqueWhite, 2f), new Rectangle((int)HotSpotCentreX_nud.Value - radius2 / 2, centre2Y, radius1, radius1));
                g.DrawLine(new Pen(Color.AntiqueWhite, 2f), new Point((int)HotSpotCentreX_nud.Value, indx3), new Point((int)HotSpotCentreX_nud.Value, indx4));
                display_pbx.Image = colorBm;
                inp.Dispose();

                MessageBox.Show("write Completed");
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void blue_rb_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                UpdateConfigSettings();
                if (blue_rb.Checked)
                {
                    ivl_camera.LEDSource = Led.Blue;
                    ivl_camera.camPropsHelper.EnableFlashControlUsingKnob();

                }
                
                    if (blue_rb.Checked)
                        ConfigVariables.CurrentSettings.CameraSettings.LiveLedSource.val = LedCode.Blue.ToString();
                UpdateConfigSettings();
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }


        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FileInfo finf = new FileInfo(ofd.FileName);
                    FileInfo[] finfArr = finf.Directory.GetFiles("*.png");
                    string directoryName = @finf.Directory.FullName + Path.DirectorySeparatorChar + "TimeStamped";
                    if (!Directory.Exists(directoryName))
                        Directory.CreateDirectory(directoryName);
                    StreamReader str = new StreamReader("timeStamp.csv");
                    string fileNameTimeStamp = "";
                    Dictionary<string, string> timeStampList = new Dictionary<string, string>();
                    List<string> fileNameTimeStampList = new List<string>();
                    while ((fileNameTimeStamp = str.ReadLine()) != null)
                    {
                        string[] timeStampArr = fileNameTimeStamp.Split(',');
                        timeStampList.Add(timeStampArr[0], timeStampArr[timeStampArr.Length - 1]);
                    }
                    foreach (FileInfo item in finfArr)
                    {
                        if (timeStampList.ContainsKey(item.Name))
                        {
                            colorBm = new Bitmap(item.FullName);
                            postProcessing = PostProcessing.GetInstance();

                            if (colorBm.Width > 2048)
                                PostProcessing.isFourteenBit = true;
                            else
                                PostProcessing.isFourteenBit = false;
                            if (colorBm.Width != PostProcessing.Width && colorBm.Height != PostProcessing.Height || HotSpotCentreX_nud.Value != ivl_camera.camPropsHelper._Settings.PostProcessingSettings.maskSettings.maskCentreX || HotSpotCentreY_nud.Value != ivl_camera.camPropsHelper._Settings.PostProcessingSettings.maskSettings.maskCentreY)
                            {
                                if (PostProcessing.isDemosaicInit)
                                {
                                    PostProcessing.ImageProc_Demosaic_Exit();
                                    //PostProcessing.AssistedFocus_Exit();
                                }
                                PostProcessing.isDemosaicInit = false;
                            }
                            PostProcessing.initDemosaic(colorBm.Width, colorBm.Height, (int)HotSpotCentreX_nud.Value, (int)HotSpotCentreY_nud.Value);
                            bool isColor = true;
                            if (imagingMode == INTUSOFT.Imaging.ImagingMode.FFA_Plus)
                                isColor = false;
                            postProcessing.ApplyPostProcessing(ref colorBm,isColor);
                            Graphics g = Graphics.FromImage(colorBm);
                            g.DrawString(timeStampList[item.Name], new Font(FontFamily.GenericSansSerif, 60f, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.LimeGreen, new PointF(colorBm.Width - 200, colorBm.Height - 150));
                            string[] filenameArr = item.Name.Split('.');
                            string val = filenameArr[0];
                            DateTime da = DateTime.Now;
                            if ((val.Substring(8, 2)) == "01")
                                da = new DateTime(Convert.ToInt32(val.Substring(0, 4)), Convert.ToInt32(val.Substring(4, 2)), Convert.ToInt32(val.Substring(6, 2)), Convert.ToInt32("13"), Convert.ToInt32(val.Substring(10, 2)), Convert.ToInt32(val.Substring(12, 2)));// val.ToDateTime(format: "yyyyMMddhhmmss");

                            else
                                da = new DateTime(Convert.ToInt32(val.Substring(0, 4)), Convert.ToInt32(val.Substring(4, 2)), Convert.ToInt32(val.Substring(6, 2)), Convert.ToInt32(val.Substring(8, 2)), Convert.ToInt32(val.Substring(10, 2)), Convert.ToInt32(val.Substring(12, 2)));// val.ToDateTime(format: "yyyyMMddhhmmss");

                            //      DateTime dt = DateTime.Parse(filenameArr[0], "yyyyMMddhhmmss");//,CultureInfo.InvariantCulture);
                            string filename = da.ToString("yyyyMMddHHmmss");
                            colorBm.Save(directoryName + Path.DirectorySeparatorChar + da.ToString("yyyyMMddHHmmss") + "." + filenameArr[1]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }

        }

        private void mode_cmbx_TextChanged(object sender, EventArgs e)
        {
            mode_cmbx_SelectedIndexChanged(sender, e);

        }

        private void showAllChannelBtn_Click(object sender, EventArgs e)
        {
            LutForm3Channels lutForm3Channel = new LutForm3Channels();
            lutForm3Channel.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(processedImgPath)) // if the processed image has been created
            {
                System.Diagnostics.Process.Start(processedImgPath); // open the image in the default application 
            }
        }

        

    }

    public static class DateTimeExtensions
    {
        static Logger Exception_Log = LogManager.GetLogger("AssemblySoftware.ExceptionLog");

        public static DateTime ToDateTime(this string s,
                  string format = "ddMMyyyy", string cultureString = "tr-TR")
        {
            var r = new DateTime();
            try
            {
                 r = DateTime.ParseExact(
                    s: s,
                    format: format,
                    provider: CultureInfo.GetCultureInfo(cultureString));
                
            }
            catch (FormatException ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            catch (CultureNotFoundException ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            return r;
        }

        public static DateTime ToDateTime(this string s,
                    string format, CultureInfo culture)
        {
            var r = new DateTime();
            try
            {
                 r = DateTime.ParseExact(s: s, format: format,
                                        provider: culture);
                
            }
            catch (FormatException ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            catch (CultureNotFoundException ex)
            {

                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            return r;
        }

    }
}
 