using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using INTUSOFT.EventHandler;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Drawing.Drawing2D;
using INTUSOFT.Desktop.Properties;
using INTUSOFT.Imaging;
using System.Threading;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Common;
using System.Windows.Forms.Integration;

namespace INTUSOFT.Desktop.Forms
{
    public partial class Imaging_UC : UserControl
    {
        bool isMagnifier = false;
        IVLEventHandler eventHandler;
       public LiveImageControls_UC liveImagingControl;
        ViewImageControls_UC viewImagingControl;
        delegate void DelegateSetLiveOrView(string imageFilename, Args arg);
        private DelegateSetLiveOrView m_DelegateSetLiveOrView;
        delegate void DelegateSetView();
        private DelegateSetView m_DelegateSetView;
        int RBwidth;
        int RBheight;
        int LBwidth;
        int LBheight;
        object lockObj = new object();
        Bitmap bm1;
        Bitmap bm;
        //ImageCaptureSplashScreen imag;
        Point p;
        public bool isImagingDisabled = true;
        System.Timers.Timer ResetTimer;
        int pictBoxLeft;
        Bitmap resetBitmapLeft, resetBitmapRight;
        int pictboxTop;
        int pictboxBottom;
        int MagnifierWidth = 200;
        int MagnifierHeight = 200;
        float zoomFactor = 1.0f;
        GraphicsPath gp;
        int anteriorOverlayWidthHeight = 400;
        int leftBorderWidth;
        SolidBrush solidBrush;
        int rightBorderWidth;
        int topBorderHeight;
        int bottomBorderHeight;
        Bitmap negativeSymbol, positiveSymbol;

        Bitmap gradable_bm, nonGradable_bm, qiProgress_bm, qiFailed_bm;
        Bitmap overlayBm;
        int prevSensorPos = 0;
        static Imaging_UC _imaging_UC;
        public bool isImaging = false;
        public bool isLiveScreen = true;
        //INTUSOFT.Custom.Controls.PictureBoxExtended maskOverlay_pbx;
        string FrameRateLabelText;
        string CameraConnectionLabelText;
        string CameraDisconnectionLabelText;
        string LiveExposureLabelText;
        string LiveGainLabelText;
        string CaptureExposureLabelText;
        string CaptureGainLabelText;
        string ComportOpenLabelText;
        string ComportCloseLabelText;
        string PowerOnLabelText;
        string PowerOffLabelText;
        ElementHost infoIcon_p;
        InformationIconUCL informationIcon ;

        public static Imaging_UC GetInstance()
        {
            if (_imaging_UC == null)
            {
                _imaging_UC = new Imaging_UC();
            }
            return _imaging_UC;
        }
        private Imaging_UC()
        {
            InitializeComponent();
            eventHandler = IVLEventHandler.getInstance();
            eventHandler.Register(eventHandler.SetImagingScreen, new NotificationHandler(SetLiveRViewMode));
            eventHandler.Register(eventHandler.HideSplash, new NotificationHandler(closeSplash));
            eventHandler.Register(eventHandler.EnableZoomMagnification, new NotificationHandler(enableZoomMag));
            eventHandler.Register(eventHandler.ChangedDisplayBitmap, new NotificationHandler(ChangedImage));
            //eventHandler.Register(eventHandler.PosteriorAnteriorSelection, new NotificationHandler(SetOverlay));
            eventHandler.Register(eventHandler.FrameRateStatusUpdate, new NotificationHandler(camera_statusBarUpdate));
            eventHandler.Register(eventHandler.RotaryMovedEvent, new NotificationHandler(RotaryMovemetDone));
            eventHandler.Register(eventHandler.DisplayImage, new NotificationHandler(DisplayImageFromCamera));
            eventHandler.Register(eventHandler.DisplayCapturedImage, new NotificationHandler(DisplayCapturedImage));
            eventHandler.Register(eventHandler.UpdateOverlay, new NotificationHandler(updateOverlay));

            //status_ts.Visible = false;
            liveImagingControl = new LiveImageControls_UC();
            viewImagingControl = new ViewImageControls_UC();
            liveImagingControl.Dock = DockStyle.Fill;
            viewImagingControl.Dock = DockStyle.Fill;
            m_DelegateSetLiveOrView = new DelegateSetLiveOrView(this.SetLiveRViewMode);
            m_DelegateSetView = new DelegateSetView(this.setViewScreen);
            IVLVariables.ivl_Camera.camPropsHelper.resetBitmapRight = new Bitmap(negativeDiaptor_pbx.ClientSize.Width, negativeDiaptor_pbx.ClientSize.Height);
            IVLVariables.ivl_Camera.camPropsHelper.negativearrowSymbol = new Bitmap(negativeDiaptor_pbx.ClientSize.Width, negativeDiaptor_pbx.ClientSize.Height);
            IVLVariables.ivl_Camera.camPropsHelper.positivearrowSymbol = new Bitmap(negativeDiaptor_pbx.ClientSize.Width, negativeDiaptor_pbx.ClientSize.Height);
           
            IVLVariables.ivl_Camera.camPropsHelper.LeftBitmap = new Bitmap(neg_pbx.ClientSize.Width, neg_pbx.ClientSize.Height);// Added this to manage the blinking happening in the UI to show the sensor position when the rotary is moved instead of using the panel drawing
            IVLVariables.ivl_Camera.camPropsHelper.RightBitmap = new Bitmap(pos_pbx.ClientSize.Width, pos_pbx.ClientSize.Height);// Added this to manage the blinking happening in the UI to show the sensor position when the rotary is moved instead of using the panel drawing
            viewImagingControl.noImageSelected_lbl = this.NoImageSelected_lbl;
            //IVLVariables.ivl_Camera.camPropsHelper.RBwidth = pos_pbx.Width;
            //IVLVariables.ivl_Camera.camPropsHelper.RBheight = pos_pbx.Height;
            //IVLVariables.ivl_Camera.camPropsHelper.LBwidth = neg_pbx.Width;
            //IVLVariables.ivl_Camera.camPropsHelper.LBheight = neg_pbx.Height;
            MinResumeCount = 0;
            count = 10;
            //focus_lbl.Text = IVLVariables.LangResourceManager.GetString( "MotorFocus_Text",IVLVariables.LangResourceCultureInfo);
            #region added block of code for drawing negative and positive symbol and overlays.
            negativeSymbol = new Bitmap(32, 32);
            positiveSymbol = new Bitmap(32, 32);
            //solidBrush = new SolidBrush(Color.FromName(IVLVariables.CurrentSettings.CameraSettings._MotorNegativeColor.val));
            solidBrush = new SolidBrush(Color.White);
            float diaptorFontSize = 14.0f;
            float diatptorFontYAxis = 4.5f;
            float diaptorLineSize = 3f;
            Graphics g = Graphics.FromImage(negativeSymbol);
            g.FillRectangle(Brushes.Black, new Rectangle(0, 0, negativeSymbol.Width, negativeSymbol.Height));
            //g.DrawLine(new Pen(Brushes.Aqua, 2f), new Point(negativeSymbol.Width / 2, 2), new Point(negativeSymbol.Width / 2, negativeSymbol.Height - 2));
            g.DrawLine(new Pen(solidBrush, diaptorLineSize), new Point(2, negativeSymbol.Height / 2), new Point((negativeSymbol.Width / 2) - 1, negativeSymbol.Height / 2));
            g.DrawString("D", new System.Drawing.Font(new System.Drawing.FontFamily("Times New Roman"), diaptorFontSize), solidBrush, new PointF((float)(negativeSymbol.Width / 2) - 1, diatptorFontYAxis));
            //negativeSymbol.Save("Negative.bmp");
            g = Graphics.FromImage(positiveSymbol);
            //solidBrush = new SolidBrush(Color.FromName(IVLVariables.CurrentSettings.CameraSettings._MotorPositiveColor.val));
            solidBrush = new SolidBrush(Color.White);
            g.FillRectangle(Brushes.Black, new Rectangle(0, 0, positiveSymbol.Width, positiveSymbol.Height));
            g.DrawLine(new Pen(solidBrush, diaptorLineSize), new Point(positiveSymbol.Width / 4, positiveSymbol.Height / 4 + 1), new Point(positiveSymbol.Width / 4, (positiveSymbol.Height / 2) + positiveSymbol.Height / 4 - 1)); //vertical  line for the plus symbol to show rotary movement
            g.DrawLine(new Pen(solidBrush, 3f), new Point(2, positiveSymbol.Height / 2), new Point((int)((float)(positiveSymbol.Width / 2) - 1f), positiveSymbol.Height / 2));//  horizontal line for the plus symbol to show rotary movement
            g.DrawString("D", new System.Drawing.Font(new System.Drawing.FontFamily("Times New Roman"), diaptorFontSize), solidBrush, new PointF((float)(positiveSymbol.Width / 2) - 1, diatptorFontYAxis));// D to show diaptor in rotary movement
            //This code has been modified by Darshan on 24-08-2015 to solve Defect no 0000567: The OD and OS sections are masked.
            #endregion
            
            display_pbx.MouseWheel += display_pbx_MouseWheel;
            Rectangle screenRectangle = RectangleToScreen(this.ClientRectangle);
            //titleBarHeight = screenRectangle.Top - this.Top;
            leftBorderWidth = screenRectangle.Left - base.Left;
            rightBorderWidth = base.Right - screenRectangle.Right;
            topBorderHeight = screenRectangle.Top - base.Top;
            bottomBorderHeight = base.Bottom - screenRectangle.Bottom;
            overlay_pbx.Visible = false;

            negativeSymbol = new Bitmap(32, 32);
            positiveSymbol = new Bitmap(32, 32);
            //IVLVariables.ivl_Camera.camPropsHelper.RotaryNegativeColor = IVLVariables.CurrentSettings.CameraSettings._MotorNegativeColor.val;
            IVLVariables.ivl_Camera.camPropsHelper.RotaryNegativeColor = "White";
            //solidBrush = new SolidBrush(Color.FromName(IVLVariables.CurrentSettings.CameraSettings._MotorNegativeColor.val));
            solidBrush = new SolidBrush(Color.White);
             g = Graphics.FromImage(negativeSymbol);
             g.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, negativeSymbol.Width, negativeSymbol.Height));

            //g.DrawLine(new Pen(Brushes.Aqua, 2f), new Point(negativeSymbol.Width / 2, 2), new Point(negativeSymbol.Width / 2, negativeSymbol.Height - 2));
            g.DrawLine(new Pen(solidBrush, diaptorLineSize), new Point(2, negativeSymbol.Height / 2), new Point((negativeSymbol.Width / 2) - 1, negativeSymbol.Height / 2));
            g.DrawString("D", new System.Drawing.Font(new System.Drawing.FontFamily("Times New Roman"), diaptorFontSize), solidBrush, new PointF((float)(negativeSymbol.Width / 2) - 1, diatptorFontYAxis));
            g = Graphics.FromImage(positiveSymbol);
            //IVLVariables.ivl_Camera.camPropsHelper.RotaryPositiveColor = IVLVariables.CurrentSettings.CameraSettings._MotorPositiveColor.val;
            IVLVariables.ivl_Camera.camPropsHelper.RotaryPositiveColor = "White";

            //solidBrush = new SolidBrush(Color.FromName(IVLVariables.CurrentSettings.CameraSettings._MotorPositiveColor.val));
            solidBrush = new SolidBrush(Color.White);
            g.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, positiveSymbol.Width, positiveSymbol.Height));
            g.DrawLine(new Pen(solidBrush, diaptorLineSize), new Point(positiveSymbol.Width / 4, positiveSymbol.Height / 4 + 1), new Point(positiveSymbol.Width / 4, (positiveSymbol.Height / 2) + positiveSymbol.Height / 4 - 1)); //vertical  line for the plus symbol to show rotary movement
            g.DrawLine(new Pen(solidBrush, 3f), new Point(2, positiveSymbol.Height / 2), new Point((int)((float)(positiveSymbol.Width / 2) - 1f), positiveSymbol.Height / 2));//  horizontal line for the plus symbol to show rotary movement
            g.DrawString("D", new System.Drawing.Font(new System.Drawing.FontFamily("Times New Roman"), diaptorFontSize), solidBrush, new PointF((float)(positiveSymbol.Width / 2) - 1, diatptorFontYAxis));// D to show diaptor in rotary movement
            negativeDiaptor_pbx.Image = negativeSymbol;
            positiveDiaptor_pbx.Image = positiveSymbol;
            IVLVariables.ivl_Camera.camPropsHelper.CreatePositiveNegativeDiaptorSymbols();
            status_ts.Renderer = new INTUSOFT.Custom.Controls.FormToolStripRenderer();

            IVLVariables.ivl_Camera.Pbx = (PictureBox)display_pbx;
            IVLVariables.ivl_Camera.MaskOverlayPbx = (PictureBox)maskOverlay_Pbx;
            IVLVariables.ivl_Camera.LeftDiaptorPbx = neg_pbx;
            IVLVariables.ivl_Camera.RightDiaptorPbx = pos_pbx;
            IVLVariables.ivl_Camera.LeftArrowPbx = negativeArrow_pbx ;
            IVLVariables.ivl_Camera.RightArrowPbx = positiveArrow_pbx;
            #region Label Texts for Camera status bar
            IVLVariables.ivl_Camera.FrameRateLabelText = IVLVariables.LangResourceManager.GetString("FrameRate_Text", IVLVariables.LangResourceCultureInfo) ;
            IVLVariables.ivl_Camera.CameraConnectionLabelText =  IVLVariables.LangResourceManager.GetString("Camera_Text", IVLVariables.LangResourceCultureInfo) + IVLVariables.LangResourceManager.GetString("Connected_Text", IVLVariables.LangResourceCultureInfo);
            IVLVariables.ivl_Camera.CameraDisconnectionLabelText=IVLVariables.LangResourceManager.GetString("Camera_Text", IVLVariables.LangResourceCultureInfo) + IVLVariables.LangResourceManager.GetString("Disconnected_Text", IVLVariables.LangResourceCultureInfo);
            IVLVariables.ivl_Camera.LiveExposureLabelText = IVLVariables.LangResourceManager.GetString("IR_Intensity_Label_Text", IVLVariables.LangResourceCultureInfo);
            IVLVariables.ivl_Camera.LiveGainLabelText = IVLVariables.LangResourceManager.GetString("LiveGain_Label_Text", IVLVariables.LangResourceCultureInfo);
            IVLVariables.ivl_Camera.CaptureExposureLabelText = IVLVariables.LangResourceManager.GetString("Flash_Intesnity_Label_Text", IVLVariables.LangResourceCultureInfo);
            IVLVariables.ivl_Camera.CaptureGainLabelText = IVLVariables.LangResourceManager.GetString("gain_Label_Text", IVLVariables.LangResourceCultureInfo);
            IVLVariables.ivl_Camera.ComportOpenLabelText = IVLVariables.LangResourceManager.GetString("Comport_Text", IVLVariables.LangResourceCultureInfo)+ IVLVariables.LangResourceManager.GetString("Open_Text", IVLVariables.LangResourceCultureInfo);
            IVLVariables.ivl_Camera.ComportCloseLabelText = IVLVariables.LangResourceManager.GetString("Comport_Text", IVLVariables.LangResourceCultureInfo) + IVLVariables.LangResourceManager.GetString("Closed_Text", IVLVariables.LangResourceCultureInfo);
            IVLVariables.ivl_Camera.PowerOnLabelText = IVLVariables.LangResourceManager.GetString("Power_Text", IVLVariables.LangResourceCultureInfo)+ IVLVariables.LangResourceManager.GetString("On_Text", IVLVariables.LangResourceCultureInfo);
            IVLVariables.ivl_Camera.PowerOffLabelText = IVLVariables.LangResourceManager.GetString("Power_Text", IVLVariables.LangResourceCultureInfo) + IVLVariables.LangResourceManager.GetString("Off_Text", IVLVariables.LangResourceCultureInfo);
            

            IVLVariables.ivl_Camera.FrameRate_lbl = this.FrameRate_lbl;
            IVLVariables.ivl_Camera.ExposureStatus_lbl = this.ExposureStatus_lbl;
            IVLVariables.ivl_Camera.gainStatus_lbl = this.gainStatus_lbl ;

            #endregion

            #region QI status display bitmaps creation
            gradable_bm = new Bitmap(100, 200);
            nonGradable_bm = new Bitmap(100, 200);
            qiProgress_bm = new Bitmap(100, 200);
            qiFailed_bm = new Bitmap(100, 200);

            Graphics qiGraphics = Graphics.FromImage(gradable_bm);
            qiGraphics.FillRectangle(Brushes.Green, new Rectangle(0, 0, gradable_bm.Width, gradable_bm.Height));
            qiGraphics.Dispose();
            gradableImg_lbl.Image = gradable_bm;
            gradableText_lbl.Text = IVLVariables.LangResourceManager.GetString("Gradable_Text", IVLVariables.LangResourceCultureInfo);

             qiGraphics = Graphics.FromImage(nonGradable_bm);
            qiGraphics.FillRectangle(Brushes.Gray, new Rectangle(0, 0, gradable_bm.Width, gradable_bm.Height));
            qiGraphics.Dispose();
            nonGradableImg_lbl.Image = nonGradable_bm;
            nonGradableText_lbl.Text = IVLVariables.LangResourceManager.GetString("NonGradable_Text", IVLVariables.LangResourceCultureInfo);


            qiGraphics = Graphics.FromImage(qiProgress_bm);
            qiGraphics.FillRectangle(Brushes.Yellow, new Rectangle(0, 0, gradable_bm.Width, gradable_bm.Height));
            qiGraphics.Dispose();
            QIProgressImg_lbl.Image = qiProgress_bm;
            QIProgressText_lbl.Text = IVLVariables.LangResourceManager.GetString("QIProgress_Text", IVLVariables.LangResourceCultureInfo);


            qiGraphics = Graphics.FromImage(qiFailed_bm);
            qiGraphics.FillRectangle(Brushes.Red, new Rectangle(0, 0, gradable_bm.Width, gradable_bm.Height));
            qiGraphics.Dispose();
            QIFailedImg_lbl.Image = qiFailed_bm;
            QIFailedText_lbl.Text = IVLVariables.LangResourceManager.GetString("QIFailed_Text", IVLVariables.LangResourceCultureInfo);
            #endregion


            
            //elementHost.Dock = DockStyle.Bottom;
            //reportListView.Parent = this.reportGridView_p;
            //this.display_pbx.Visible = false;
            //elementHost.Parent = display_pbx;
        }
        bool isFFAImage = false;
        private void DisplayImageFromCamera(string s, Args arg)
        {

            display_pbx.Image = arg["rawImage"] as Bitmap;
            this.display_pbx.Controls.Clear();
            
            infoIcon_p = new ElementHost();
            InfoVM infoVM = new InfoVM();
            informationIcon = new InformationIconUCL(infoVM);
            //infoIcon_p.Dock = DockStyle.Bottom;
            infoIcon_p.BackColor = Color.Transparent;
            infoIcon_p.Size = new Size(105, 150);
            infoIcon_p.Child = informationIcon;
            this.display_pbx.Controls.Add(infoIcon_p);

            // infoIcon_p.Location = new Point(300, 300);
            var heightValue = ( 1.75) * (double)(infoIcon_p.ClientSize.Height);
          var value = (int) Math.Round(heightValue, MidpointRounding.AwayFromZero);
                infoIcon_p.Location = new Point(display_pbx.ClientSize.Width - 130,display_pbx.ClientSize.Height - 201);
            //elementHost.Location = new Point(this.Width - 10, this.Height);
            Console.WriteLine(infoIcon_p.Location);
            Console.WriteLine(display_pbx.ClientSize);
        }


        /// <summary>
        /// To display the captured image after capturing
        /// </summary>
        /// <param name="s"></param>
        /// <param name="arg"></param>
        private void DisplayCapturedImage(string s, Args arg)
        {
            try
            {
                //if (arg.ContainsKey("FFA Image"))///added to make the maskoverlay pbx with time.
                //{
                //    if (maskOverlay_Pbx != null)
                //    {
                //        if (maskOverlay_Pbx.Visible)
                //        {
                //            Graphics g = Graphics.FromHwnd(maskOverlay_Pbx.Handle);
                //            g.DrawString(arg["FFA Image"] as string, new Font(FontFamily.GenericSansSerif, 60f, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.LimeGreen, new PointF(maskOverlay_Pbx.Image.Width - 200, maskOverlay_Pbx.Image.Height - 150));
                //            g.Dispose();
                //        }
                //    }
                //}
            }
            catch (InvalidProgramException ex)
            {

                throw;
            }
        }

        public void UpdateFontForeColor()
        {
            viewImagingControl.UpdateFontForeColor();
            List<Control> controls = GetControls(this).ToList();
            foreach (Control c in controls)
            {
                if (c is ToolStrip)
                {
                    ToolStrip l = c as ToolStrip;
                    {
                        for (int i = 0; i < l.Items.Count; i++)
                        {
                            l.Items[i].ForeColor = IVLVariables.GradientColorValues.FontForeColor;
                            l.Refresh();
                        }

                    }
                }

            }
        }


        public static IEnumerable<Control> GetControls(Control form)
        {
            foreach (Control childControl in form.Controls)
            {   // Recurse child controls.
                foreach (Control grandChild in GetControls(childControl))
                {
                    yield return grandChild;
                }
                yield return childControl;
            }
        }
        private void RotaryMovemetDone(string s, Args arg)
        {
            if (arg.ContainsKey("SensorPosition"))
            {
                //DisplayRotaryMovement(Convert.ToInt32(arg["SensorPosition"]));
                try
                {
                    if (arg.ContainsKey("LeftBitmap"))
                        neg_pbx.Image = arg["LeftBitmap"] as Bitmap;
                }
                catch (Exception)
                {
                }
                try
                {
                    if (arg.ContainsKey("RightBitmap"))
                        pos_pbx.Image = arg["RightBitmap"] as Bitmap;
                }
                catch (Exception)
                {
                }
                try
                {
                    if (arg.ContainsKey("RightArrowBitmap"))
                        positiveArrow_pbx.Image = arg["RightArrowBitmap"] as Bitmap;
                }
                catch (Exception)
                {
                }
                try
                {
                    if (arg.ContainsKey("LeftArrowBitmap"))
                        negativeArrow_pbx.Image = arg["LeftArrowBitmap"] as Bitmap;
                }
                catch (Exception)
                {
                }
            }
        }
       
        //Added by darshan on 28-06-2016 to implement the posterior and anterior functionality.
        //private void SetOverlay(string s, Args arg)
        //{
        //    if ((bool)arg["SetOverlay"] == true)
        //    {
        //        if (overlay_pbx.Dock != DockStyle.Fill)
        //            overlay_pbx.Dock = DockStyle.Fill;
        //        if (overlay_pbx.Parent != display_pbx)
        //            overlay_pbx.Parent = display_pbx;
        //        if (overlay_pbx.BackColor != Color.Transparent)
        //            overlay_pbx.BackColor = Color.Transparent;
        //        overlay_pbx.Visible = true;

        //        if (IVLVariables.ivl_Camera.ImagingMode == ImagingMode.Anterior)
        //        {
        //            overlayBm = Image.FromFile(@"overlays/AnteriorOverlay.png") as Bitmap;
        //            overlayBm.MakeTransparent(Color.Black);
        //            overlay_pbx.Image = overlayBm;

        //        }
        //        else
        //        {
        //            Graphics overlayGraphics = null;

        //            //overlayGraphics = Graphics.FromImage(PosteriorOverlayBitmap);
        //            //overlayGraphics.FillRectangle(Brushes.Red, new Rectangle(0, 0, PosteriorOverlayBitmap.Width, PosteriorOverlayBitmap.Height));

        //            //overlayGraphics.FillRectangle(Brushes.Black, new Rectangle(0, 0, PosteriorOverlayBitmap.Width, PosteriorOverlayBitmap.Height));



        //            //PosteriorOverlayBitmap.MakeTransparent(Color.Red);


        //          //  IVLVariables.ivl_Camera.PostProcessing.ApplyMask(ref PosteriorOverlayBitmap, IVLVariables.ivl_Camera.camPropsHelper._Settings.PostProcessingSettings.maskSettings);
        //            //overlayGraphics = Graphics.FromImage(PosteriorOverlayBitmap);

        //            //overlayGraphics.DrawLine(new Pen(Color.FromArgb(29, 132, 150), 5f), new Point(0, PosteriorOverlayBitmap.Height / 2), new Point(PosteriorOverlayBitmap.Width, PosteriorOverlayBitmap.Height / 2));
        //            //overlayGraphics.DrawLine(new Pen(Color.FromArgb(29, 132, 150), 5f), new Point(PosteriorOverlayBitmap.Width / 2, 0), new Point(PosteriorOverlayBitmap.Width / 2, PosteriorOverlayBitmap.Height));
        //            ////overlayBm = new Bitmap(Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._ImageWidth.val),
        //            //// Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._ImageHeight.val));
        //            //PosteriorOverlayBitmap.MakeTransparent(Color.Red);
        //            //overlayGraphics.Dispose();

        //            //overlay_pbx.Image = PosteriorOverlayBitmap;


        //        }
        //    }
        //    else
        //    {
        //            overlay_pbx.Dock = DockStyle.None;
        //            overlay_pbx.Parent = null;
        //        overlay_pbx.Visible = false;
        //    }
        //    }
           

          



        //private void updateStatusBar()

        void camera_statusBarUpdate(string a, Args e)
        {
            if (!IVLVariables.ApplicationClosing)
            {
                FrameRate_lbl.Text = FrameRateLabelText + ((int)e["TimeDiff"] - peakCnt).ToString();// e["FrameRate"] as string;// +"TrigSt =" + e["TriggerStatus"] as string; ;
                peakCnt = (int)e["TimeDiff"];
                
                ushort GainVal = 0;
                //This below if statements has been added by darshan on 14-08-2015 to solve defect no 0000370: The values shown in the Gain,Exposure and bottom of the screen mismatching.Since status bar label text was not getting updated
                if (!IVLVariables.ivl_Camera.IsCapturing)
                    gainStatus_lbl.Text = LiveGainLabelText + e["Gain"] as string;// IVLVariables.ivl_Camera.camPropsHelper.LiveGain.ToString();
                else
                    gainStatus_lbl.Text = CaptureGainLabelText + e["Gain"] as string;//IVLVariables.ivl_Camera.camPropsHelper._Settings.CameraSettings.CaptureGain.ToString();
                uint exp = 0;
                if (!IVLVariables.ivl_Camera.IsCapturing)
                    ExposureStatus_lbl.Text = LiveExposureLabelText + e["Exposure"] as string;// IVLVariables.ivl_Camera.camPropsHelper.CaptureExposure.ToString();
                else
                    ExposureStatus_lbl.Text = CaptureExposureLabelText + e["Exposure"] as string;// IVLVariables.ivl_Camera.camPropsHelper.LiveExposure.ToString();
                //ExposureStatus_lbl.Text = IVLVariables.LangResourceManager.GetString( "LiveGain_Label_Text + exp.ToString();
                //if ((bool)e["isPower"])
                //    PowerStatus_lbl.Text = PowerOnLabelText;//.LangResourceManager.GetString("Power_Text", IVLVariables.LangResourceCultureInfo) + IVLVariables.LangResourceManager.GetString("On_Text", IVLVariables.LangResourceCultureInfo);
                //else
                //    PowerStatus_lbl.Text = PowerOffLabelText;//.LangResourceManager.GetString("Power_Text", IVLVariables.LangResourceCultureInfo) + IVLVariables.LangResourceManager.GetString("Off_Text", IVLVariables.LangResourceCultureInfo);
                //if ((bool)e["isBoardOpen"])
                //    comPort_lbl.Text = ComportOpenLabelText;//.LangResourceManager.GetString("Comport_Text", IVLVariables.LangResourceCultureInfo) + IVLVariables.LangResourceManager.GetString("Open_Text", IVLVariables.LangResourceCultureInfo);
                //else
                //    comPort_lbl.Text = ComportCloseLabelText;//.LangResourceManager.GetString("Comport_Text", IVLVariables.LangResourceCultureInfo) + IVLVariables.LangResourceManager.GetString("Closed_Text", IVLVariables.LangResourceCultureInfo);
                //if ((bool)e["isCameraConnected"])
                //    CameraStatus_lbl.Text = CameraConnectionLabelText;//.LangResourceManager.GetString("Camera_Text", IVLVariables.LangResourceCultureInfo) + IVLVariables.LangResourceManager.GetString("Connected_Text", IVLVariables.LangResourceCultureInfo);
                //else
                //    CameraStatus_lbl.Text = CameraDisconnectionLabelText;//.LangResourceManager.GetString("Camera_Text", IVLVariables.LangResourceCultureInfo) + IVLVariables.LangResourceManager.GetString("Disconnected_Text", IVLVariables.LangResourceCultureInfo);
                totalFrameCount_Lbl.Text = "TotalFrames =" + e["TimeDiff"] as string;
            }

        }
        int peakCnt = 0;


        MouseEventArgs e1;
        private void enableZoomMag(string s, Args arg)
        {
            if (!isMagnifier && !viewImagingControl.noImageSelected_lbl.Visible)
            {
                //zoom_pbx.Visible = true;
                overlay_pbx.Visible = true;
                overlay_pbx.Dock = DockStyle.None;
                overlay_pbx.Width = Convert.ToInt32(IVLVariables.CurrentSettings.UserSettings._ZoomMagnifierWidth.val) ;
                overlay_pbx.Height = Convert.ToInt32(IVLVariables.CurrentSettings.UserSettings._ZoomMagnifierHeight.val);
                gp = new GraphicsPath();
              
                gp.AddEllipse(0, 0, MagnifierWidth, MagnifierHeight);
                Graphics g = overlay_pbx.CreateGraphics();
                g.DrawPath(new Pen(Color.Black, 10f), gp);
                overlay_pbx.Region = new Region(gp);
               
            }
                e1 = new MouseEventArgs(System.Windows.Forms.MouseButtons.Right, 1, display_pbx.Location.X + display_pbx.Width / 2, display_pbx.Location.Y + display_pbx.Height / 2, 0);
                EnableZoomFeature(e1);
            
        }

        void display_pbx_MouseWheel(object sender, MouseEventArgs e)
        {

                if (e.Delta > 0)
                {
                    IVLVariables.zoomFactor += (Convert.ToSingle(IVLVariables.CurrentSettings.UserSettings._ZoomScale.val) * (Convert.ToSingle(IVLVariables.CurrentSettings.UserSettings._ZoomIncrementFactor.val) * 10));
                    if (IVLVariables.zoomFactor > Convert.ToInt32(viewImagingControl.zoomMagnifierMaxValue) * Convert.ToSingle(IVLVariables.CurrentSettings.UserSettings._ZoomScale.val))
                    {
                        //IVLVariables.zoomFactor = 3.0f;This line has been commented to replace the hardcoded value with a logical computed value given below.
                        IVLVariables.zoomFactor = (float)(Convert.ToInt32(viewImagingControl.zoomMagnifierMaxValue) / 100);
                    }
                    if (IVLVariables.isZoomEnabled)
                    {
                        viewImagingControl.ZoomRbIncreaseValue();
                        drawImage();//This function is being invoked to make the zoom feature work properly when mouse wheel is moved up.
                    }
                }
                else if (e.Delta < 0)
                {
                    IVLVariables.zoomFactor -= (Convert.ToSingle(IVLVariables.CurrentSettings.UserSettings._ZoomScale.val) * (Convert.ToSingle(IVLVariables.CurrentSettings.UserSettings._ZoomIncrementFactor.val) * 10));
                    if (IVLVariables.zoomFactor < Convert.ToInt32(viewImagingControl.zoomMagnifierMinValue) * Convert.ToSingle(IVLVariables.CurrentSettings.UserSettings._ZoomScale.val))
                    {
                        //IVLVariables.zoomFactor = 1.0f;This line has been commented to replace the hardcoded value with a logical computed value given below.
                        IVLVariables.zoomFactor = (float)(Convert.ToInt32(viewImagingControl.zoomMagnifierMinValue) / 100);
                    }
                    if (IVLVariables.isZoomEnabled)
                    {
                        viewImagingControl.ZoomRbDecreaseValue();
                        drawImage();//This function is being invoked to make the zoom feature work properly when mouse wheel is moved down.
                    }
                }
        }

        private void drawImage()
        {
            {
                Rectangle rect = new Rectangle(0, 0, MagnifierWidth, MagnifierHeight);
                int zoomW = (int)(MagnifierWidth / IVLVariables.zoomFactor);
                int zoomH = (int)(MagnifierHeight / IVLVariables.zoomFactor);
                if (zoomW > 650)//This if statement has been added to stop the scatting of the image in the zoom magnifier.
                    zoomW = 600;
                if (zoomH > 650)//This if statement has been added to stop the scatting of the image in the zoom magnifier.
                    zoomH = 600;
                if (bm1 == null)
                    bm1 = new Bitmap(MagnifierWidth, MagnifierWidth);
                Graphics g = Graphics.FromImage(bm1);
                bm = viewImagingControl.modifyingBm;
                //overlay_pbx.Image = display_pbx.Image;
                // added code to make the zoom functionality response  faster 
                // g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                int x = p.X - 10;
                int y = p.Y - 50;
                //if (y > 0 && y < bm.Height)//Old Implementation
                if (y > 0 && y < (bm.Height - (zoomH / 2)))
                {
                    g.DrawImage(bm, rect, x, y, zoomW, zoomH, GraphicsUnit.Pixel);
                    overlay_pbx.Image = bm1;
                    overlay_pbx.Left = pictBoxLeft - 20;
                    overlay_pbx.Top = pictboxTop - 30;
                    display_pbx.Focus();
                }//display_pbx.Refresh();
            }
        }

       int count =0;
       int MinResumeCount = 0;
       private void closeSplash(string s , Args arg)
       {
           //splashThread.Join();
           //splashThread.Abort();
          // imag.Dispose();
           if (arg.ContainsKey("isCaptureFailed"))
               if ((bool)arg["isCaptureFailed"])
               {
                   if (!arg.ContainsKey("FailureCategory"))
                       arg["FailureCategory"] = "001";
                   Common.CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("CapturedFail_Warning_Text", IVLVariables.LangResourceCultureInfo) + arg["FailureCategory"] as string + " ," + IVLVariables.LangResourceManager.GetString("PleaeRetry_Text", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("CapturFailed_Text", IVLVariables.LangResourceCultureInfo), Common.CustomMessageBoxIcon.Error);
                   arg["isDefault"] = true;
                   eventHandler.Notify(eventHandler.UpdateMainWindowCursor, arg);
               }
       }
        /// <summary>
        /// To disable or enable the live imaging controls. 
        /// </summary>
        /// <param name="isDisable"></param>
       public void DisableLiveScreen(bool isDisable, Args arg)
       {
           //if (liveImagingControl != null)
           //{
           //     isImagingDisabled = !isDisable;
           //     liveImagingControl.Enabled = isDisable;
                
           //}
            if(!isDisable && IVLVariables.isCapturing)//if isDisable is false and isCapturing is true then it will pop up capture failure form.
                liveImagingControl.CameraPowerDisconnectedInLive("",arg);
            setViewScreen();//to set view screen.
            Thread.Sleep(500);
            arg["BtnEnable"] = isDisable;
            eventHandler.Notify(eventHandler.ThumbnailSelected, arg);//to select the thumbnail file if power or camera is disconnected and to navigate to view screen.
            eventHandler.Notify(eventHandler.EnableImagingBtn, arg);//to enable or disable imaging button
       }
       public void setLiveScreen()
       {
           if (ImagingViewControls_p.Controls.Contains(viewImagingControl))
           {
               ImagingViewControls_p.Controls.Remove(viewImagingControl);
               viewImagingControl.noImageSelected_lbl.Visible = false;
           }
           if (liveImagingControl == null)
           {
               liveImagingControl = new LiveImageControls_UC();
               liveImagingControl.Dock = DockStyle.Fill;
           }
           if (!ImagingViewControls_p.Contains(liveImagingControl))
           {
                updateOverlay("", new Args());
                ImagingViewControls_p.Controls.Add(liveImagingControl);
                liveImagingControl.Refresh();
            }
       

           status_ts.Visible = true;
           liveImagingControl.SetCurrentMode((ImagingMode)IVLVariables._ivlConfig.Mode);
           isImaging = true;
            isLiveScreen = true;
       }

       public void setViewScreen()
       {
           if (this.InvokeRequired)
           {
               this.Invoke(m_DelegateSetView);
           }
           else
           {
               if (ImagingViewControls_p.Controls.Contains(liveImagingControl))
                   ImagingViewControls_p.Controls.Remove(liveImagingControl);
               if (viewImagingControl == null)
               {
                   viewImagingControl = new ViewImageControls_UC();
                   viewImagingControl.Dock = DockStyle.Fill;
                   viewImagingControl.noImageSelected_lbl = NoImageSelected_lbl;
               }
               else
               {
                   if (viewImagingControl.noImageSelected_lbl.Visible)
                       viewImagingControl.noImageSelected_lbl.Visible = false;
                   this.viewImagingControl.ZoomModifier_color(true);
               }
               //overlay_pbx.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.PostProcessingSettings.MaskSettings._ApplyLiveMask.val);
               // overlayRetinal_pbx.Visible = false;
               // motorSensor_tbpnl.Visible = false;//This line was added by darshan to implement the CR:
               //statusStrip1.Visible = false;
               if (IVLVariables.ivl_Camera.CameraIsLive)//To set the view screen, checking for thew live mode and to stop live mode.
               {
                   IVLVariables.ivl_Camera.StopLive();
               }

               if (!ImagingViewControls_p.Contains(viewImagingControl))
               {

                   ImagingViewControls_p.Controls.Clear();
                   ImagingViewControls_p.Controls.Add(viewImagingControl);

               }
                if (!IVLVariables.isCommandLineAppLaunch)
                    viewImagingControl.showExisitingReports();
                maskOverlay_Pbx.Visible = false;
               //if (this.Controls.Contains(maskOverlay_pbx))
               //    this.Controls.Remove(maskOverlay_pbx);
               //maskOverlay_pbx.Visible = false;// overlay if present//issue no 0001644: Some unwanted black patch appearing on image in View image screen has been fixed by kishore on August 11 2017 at 4:50 pm.

               overlay_pbx.Visible = false;
               //overlay_pbx.Dock = DockStyle.None;
               isImaging = false;
               bool isPowerCameraConnected = IVLVariables.ivl_Camera.isCameraOpen && IVLVariables.ivl_Camera.camPropsHelper.IsCameraConnected == Devices.CameraConnected && IVLVariables.ivl_Camera.camPropsHelper.IsBoardOpen && IVLVariables.ivl_Camera.camPropsHelper.IsPowerConnected == Devices.PowerConnected;
               if (isPowerCameraConnected)
               {
                   if (IVLVariables.isCommandLineAppLaunch)
                       IVLVariables.ivl_Camera.TriggerOn();
                   else if (INTUSOFT.Data.Repository.NewDataVariables.Active_Visit.createdDate.Date == DateTime.Now.Date)
                       IVLVariables.ivl_Camera.TriggerOn();
               }
                isLiveScreen = false;
           }
       }

        public void Push2Cloud()
        {
            viewImagingControl.CreateCloudFileInOutbox();
        }
       Bitmap tempBm;
       private void updateOverlay(string s, Args arg)
       {
           #region Create mask overlay and add display_pbx

           ImagingMode imagingMode = IVLVariables.ivl_Camera.camPropsHelper.ImagingMode;
           if (imagingMode == ImagingMode.Posterior_45 || imagingMode == ImagingMode.Posterior_Prime || imagingMode == ImagingMode.FFA_Plus || imagingMode == ImagingMode.FFAColor)// if live mask true and posterior is enabled
           {
               if (Convert.ToBoolean(IVLVariables.CurrentSettings.PostProcessingSettings.MaskSettings._ApplyLiveMask.val))
               {
                   //maskOverlay_Pbx =new INTUSOFT.Custom.Controls.PictureBoxExtended();
                   maskOverlay_Pbx.Visible = true;

                   maskOverlay_Pbx.Dock = DockStyle.Fill;
                   maskOverlay_Pbx.Parent = display_pbx;
                   maskOverlay_Pbx.BackColor = Color.Transparent;
                   tempBm = new Bitmap(Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._ImageWidth.val) - 2 * Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._ImageROIX.val), Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._ImageHeight.val) - 2 * Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._ImageROIY.val), PixelFormat.Format24bppRgb);
                   Graphics g = Graphics.FromImage(tempBm);
                   g.FillEllipse(Brushes.White, new Rectangle(Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._ImageOpticalCentreX.val) - Convert.ToInt32(IVLVariables.CurrentSettings.PostProcessingSettings.MaskSettings.LiveMaskWidth.val) / 2, Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._ImageOpticalCentreY.val) - Convert.ToInt32(IVLVariables.CurrentSettings.PostProcessingSettings.MaskSettings.LiveMaskHeight.val) / 2, Convert.ToInt32(IVLVariables.CurrentSettings.PostProcessingSettings.MaskSettings.LiveMaskWidth.val), Convert.ToInt32(IVLVariables.CurrentSettings.PostProcessingSettings.MaskSettings.LiveMaskHeight.val)));
                   g.Dispose();
                   tempBm.MakeTransparent(Color.White);
                   maskOverlay_Pbx.Image = tempBm;
               }
               else
               {
                   maskOverlay_Pbx.Visible = false;
                   //if(this.Controls.Contains(maskOverlay_pbx))
                   //    this.Controls.Remove(maskOverlay_pbx);
               }
           }
           else
           {
               maskOverlay_Pbx.Visible = false;

               //if (this.Controls.Contains(maskOverlay_pbx))
               //    this.Controls.Remove(maskOverlay_pbx);
              // maskOverlay_pbx.Visible = false;

           }
           #endregion
       }
        private void SetLiveRViewMode(string s, Args arg)
        {
            #region Added this code to fix the defect 0000565 by sriram on August 14 2015
            if (this.InvokeRequired)
            {
                this.Invoke(m_DelegateSetLiveOrView, s, arg);
            }
            else
            {
                if (arg.ContainsKey("isImaging"))
                {
                    if ((bool)arg["isImaging"])
                    {

                        setLiveScreen();
                        arg["BtnEnable"] = false; //to disable the live imaging button
                        eventHandler.Notify(eventHandler.EnableImagingBtn, arg);
                    }
                    else
                    {
                        setViewScreen();
                        // ThumbnailModule.ThumbnailData tempTBData = arg["ThumbnailData"] as ThumbnailModule.ThumbnailData;

                        //// if (System.IO.File.Exists(tempTBData.fileName))
                        //{
                        //    eventHandler.Notify(eventHandler.LoadImageFromFileViewingScreen, arg);
                        //}
                        // display_pbx.Refresh();

                        return;// this line has been added in order to manage to return of focus to ivlmainwindow to fix defect 0000695 by sriram on september 2015
                    }
                }
            }
            #endregion
          
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void ChangedImage(string s, Args arg)
        {
            bm = arg["displayBitmap"] as Bitmap;
        }

        private void EnableZoomFeature(MouseEventArgs e)
        {
            if (viewImagingControl != null && !viewImagingControl.noImageSelected_lbl.Visible)
            {
                if (!isMagnifier)// &&)
                {
                    //to change the mouse speed. For instance in a button click handler or Form_load
                    //SPEED is an integer value between 0 and 20. 10 is the default.
                    {
                        int x = e.X - leftBorderWidth - rightBorderWidth;
                        int y = e.Y - topBorderHeight - bottomBorderHeight;
                        {
                            p = display_pbx.TranslatePointToImageCoordinates(new Point(x, y));
                            pictboxTop = e.Location.Y + display_pbx.Location.Y - topBorderHeight - bottomBorderHeight - 15;// -pictureBox2.Location.Y - titleBarHeight;
                            pictBoxLeft = e.Location.X + display_pbx.Location.X - leftBorderWidth - rightBorderWidth - 25;
                            //bm = display_pbx.Image as Bitmap;
                            bm = viewImagingControl.modifyingBm;
                            this.viewImagingControl.iszoom = true;
                            this.viewImagingControl.ZoomModifier_color(false);
                        }
                        overlay_pbx.Dock = DockStyle.None;
                        overlay_pbx.Visible = true;
                        overlay_pbx.BringToFront();
                        drawImage();
                        isMagnifier = true;
                    }
                }
                else
                {
                    isMagnifier = false;
                    this.viewImagingControl.iszoom = false;
                    this.viewImagingControl.ZoomModifier_color(true);
                    overlay_pbx.Visible = false;
                    overlay_pbx.Size = display_pbx.Size;
                    GraphicsPath gp = new GraphicsPath();
                    gp.AddRectangle(new Rectangle( 0, 0, display_pbx.Width, display_pbx.Height));
                    Graphics g = overlay_pbx.CreateGraphics();
                    g.DrawPath(new Pen(Color.Black, 10f), gp);
                    overlay_pbx.Region = new Region(gp);
                }
                IVLVariables.isZoomEnabled = isMagnifier;
            }
        }

        public void display_pbx_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (!IVLVariables.ivl_Camera.CameraIsLive && !IVLVariables.ivl_Camera.IsCapturing && !isImaging) //  if (IVLVariables.ivl_Camera == null)//This line has been modified to enable the zoom magnifier when mouse right button is clicked.
                {
                    eventHandler.Notify(eventHandler.EnableZoomMagnification, new Args());
                    //EnableZoomFeature(e);
                }
            }
        }

        private void display_pbx_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMagnifier)
            {
                {
                    {
                        int x = e.X;// -leftBorderWidth - rightBorderWidth;
                        int y = e.Y;// -topBorderHeight - bottomBorderHeight;
                        pictboxTop = e.Location.Y + display_pbx.Location.Y - topBorderHeight - bottomBorderHeight;// -pictureBox2.Location.Y - titleBarHeight;
                        pictBoxLeft = e.Location.X + display_pbx.Location.X - leftBorderWidth - rightBorderWidth;
                        {
                             p = display_pbx.TranslatePointToImageCoordinates(new Point(x, y));
                        }
                        drawImage();
                    }
                }
            }
        }

        private void Imaging_UC_Resize(object sender, EventArgs e)
        {
            IVLVariables.ivl_Camera.camPropsHelper.LeftBitmap = new Bitmap(neg_pbx.Width, neg_pbx.Height);// Added this to manage the blinking happening in the UI to show the sensor position when the rotary is moved instead of using the panel drawing
            IVLVariables.ivl_Camera.camPropsHelper.RightBitmap = new Bitmap(pos_pbx.Width, pos_pbx.Height);// Added this to manage the blinking happening in the UI to show the sensor position when the rotary is moved instead of using the panel drawing

        }
        PaintEventArgs graphicsE;
        private void Imaging_UC_Paint(object sender, PaintEventArgs e)
        {
            graphicsE = e;

        }

        private void display_pbx_Paint(object sender, PaintEventArgs e)
        {
            int x = 0;

            
        }

        private void overlay_pbx_Click(object sender, EventArgs e)
        {

        }

        private void display_pbx_Click(object sender, EventArgs e)
        {

        }

    }

    public static class WinAPI
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto),]
        public static extern int SystemParametersInfo(uint uiAction, uint uiParam, uint pvParam, uint fWinIni);
        public const UInt32 SPI_SETMOUSESPEED = 0x0071;
        public static uint Reset_speed = 4; 
        public static void setMouseSpeed()
        {
            SystemParametersInfo(
                SPI_SETMOUSESPEED,
                0,
                Reset_speed,
                0);
        }
        public static void GetMouseSpeed()
        {
            SystemParametersInfo(
                SPI_SETMOUSESPEED,
                0,
                1,
                0);
       }
    }
}
