using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using INTUSOFT.Imaging;
using INTUSOFT.EventHandler;
using NLog;
using NLog.Config;
using NLog.Targets;
namespace CameraApiTest
{
    public partial class CameraApiSampleWindow : Form
    {
        #region Variables and Constants
        bool isCameraConnected = false;
        IntucamHelper IVL_CAM = null;
        IVLEventHandler eventHandler;
        public Logger Exception_Log = LogManager.GetLogger("CameraApiTest.ExceptionLog");// for exception logging
        List<Control> ListOfControls;
        #endregion

        /// <summary>
        /// Constructor for the sample application
        /// </summary>
        public CameraApiSampleWindow()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

            IVL_CAM = IntucamHelper.GetInstance();// Get IVL camera object
            IVL_CAM.Pbx = display_pbx;
            eventHandler = IVLEventHandler.getInstance();// Get IVL event handler Object

            //eventHandler.Register(eventHandler.DisplayImage, new NotificationHandler(DisplayImage));
            //eventHandler.Register(eventHandler.DisplayCapturedImage, new NotificationHandler(DisplayCapturedImage));
            eventHandler.Register(eventHandler.RotaryMovedEvent, new NotificationHandler(RotaryDisplayDiaptor));
            eventHandler.Register(eventHandler.UPDATE_POWER_STATUS, new NotificationHandler(ShowPowerConnection));
            eventHandler.Register(eventHandler.UPDATE_CAMERA_STATUS, new NotificationHandler(ShowCameraConnection));
            eventHandler.Register(eventHandler.FrameRateStatusUpdate, new NotificationHandler(FrameStatusUpdater));
            eventHandler.Register(eventHandler.ChangeLeftRightPos_Live, new NotificationHandler(ChangeLeftRightPosition));
            eventHandler.Register(eventHandler.FrameCaptured, new NotificationHandler(FrameCaptured));
            IVL_CAM._EnableControls += EnableDisableControls;
            ListOfControls = GetControls(this).ToList();
            EnableDisableControls(false);

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

        //private void DisplayCapturedImage(string s, Args arg)
        //{
        //    Bitmap dispBm = arg["rawImage"] as Bitmap;
        //    display_pbx.Image = dispBm; // to display image on to the picture box
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="arg"></param>
        private void ChangeLeftRightPosition(string s, Args arg)
        {
            {
                if ((LeftRightPosition)Enum.Parse(typeof(LeftRightPosition), arg["LeftRightPos"].ToString()) == LeftRightPosition.Left)
                {
                   leftEyeSide_rb.Checked = true;
                }
                else
                {
                    rightSide_rb.Checked = true;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="arg"></param>
        void FrameCaptured(string s, Args arg)
        {
            IVL_CAM.ResumeLive();
        }
        /// <summary>
        /// Connect camera button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectCamera_btn_Click(object sender, EventArgs e)
        {
            if (!isCameraConnected)// If the camera is not connected 
            {
               // Camera.CameraHandle = this.Handle;
                if (posteriorMode_rb.Checked)
                    IVL_CAM.camPropsHelper.ImagingMode = ImagingMode.Posterior_Prime;
                else
                    IVL_CAM.camPropsHelper.ImagingMode = ImagingMode.Anterior_Prime;
                
                    IVL_CAM.OpenCameraBoard();
                    isCameraConnected = IVL_CAM.isCameraOpen;
                if (isCameraConnected)
                {
                    connectCamera_btn.Text = "Disconnect";
                    StartLive_btn.Enabled = true;
                 
                }
            }
            else
            {
                bool returnVal = IVL_CAM.StopLive();
                if (returnVal)
                    StartLive_btn.Text = "Start Live ";
                IVL_CAM.DisconnectCameraModule();
                EnableDisableControls(false);
                connectCamera_btn.Enabled = true;
                connectCamera_btn.Text = "Connect";
                isCameraConnected = false;
            }
        }

        /// <summary>
        /// To enable and disable the controls if the camera is connected.
        /// </summary>
        private void EnableDisableControls(bool isEnabled)
        {
            foreach (Control item in ListOfControls)
            {
                if (item is TableLayoutPanel|| item is SplitContainer || item is SplitterPanel || item is PictureBox)
                    continue;
                else
                item.Enabled = isEnabled;
            }
        }


        /// <summary>
        /// Start live button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartLive_btn_Click(object sender, EventArgs e)
        {
            if (IVL_CAM.isCameraOpen)
            {
                if (!IVL_CAM.CameraIsLive)
                {
                    bool returnVal = IVL_CAM.StartLive();
                    EnableDisableControls(true);
                    if (returnVal)
                    {
                        StartLive_btn.Text = "Stop Live ";
                        lowLiveInt_rb.Checked = true;
                        lowCaptureInt_rb.Checked = true;
                    }
                }
                else
                {
                    bool returnVal = IVL_CAM.StopLive();
                    EnableDisableControls(false);
                    StartLive_btn.Enabled = true;
                    connectCamera_btn.Enabled = true;
                    if (returnVal)
                        StartLive_btn.Text = "Start Live ";
                }
            }
        }

        //void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    UpdateCaptureFrameButton();
        //}


        /// <summary>
        /// To update the capture button when the capture frame button is clicked or trigger is pressed.
        /// </summary>
        void UpdateCaptureFrameButton()
        {
            if (IVL_CAM.IsCapturing)
            {
                CaptureFrame_btn.Enabled = false;
                return;
            }
            //else
        }

        /// <summary>
        /// Capture button click event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CaptureFrame_btn_Click(object sender, EventArgs e)
        {
            if (IVL_CAM.CameraIsLive && IVL_CAM.isCameraOpen )// if the camera is in live mode and the camera is open then start capture sequence
            {
                IVL_CAM.HowImageWasCaptured = IntucamHelper.CapturedUIMode.CaptureButton;
                IVL_CAM.StartCapture(false); // start the capture sequence where false indicates it is not from trigger.
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lowLiveInt_rb_CheckedChanged(object sender, EventArgs e)
        {
            IVL_CAM.camPropsHelper.LiveGainLevel = GainLevels.Low; // Set the live Intensity level to low

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void medLiveInt_rb_CheckedChanged(object sender, EventArgs e)
        {
            IVL_CAM.camPropsHelper.LiveGainLevel = GainLevels.Medium;// Set the live Intensity level to medium

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void highLiveInt_rb_CheckedChanged(object sender, EventArgs e)
        {
            IVL_CAM.camPropsHelper.LiveGainLevel = GainLevels.High;// Set the live Intensity level to high

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lowCaptureInt_rb_CheckedChanged(object sender, EventArgs e)
        {
            IVL_CAM.camPropsHelper.CaptureGainLevel = GainLevels.Low;// Set the Capture Intensity level to Low

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void medCaptureInt_rb_CheckedChanged(object sender, EventArgs e)
        {
            IVL_CAM.camPropsHelper.CaptureGainLevel = GainLevels.Medium;// Set the Capture Intensity level to medium

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void highCaptureInt_rb_CheckedChanged(object sender, EventArgs e)
        {
            IVL_CAM.camPropsHelper.CaptureGainLevel = GainLevels.High;// Set the Capture Intensity level to high
        }
        ///// <summary>
        ///// The event which gets fired when a frame has arrived in the camera
        ///// </summary>
        ///// <param name="s"> Display Image Event name</param>
        ///// <param name="arg">The arg contains the frame in the form of bitmap the key for which is "rawImage"</param>
        //void DisplayImage(string s, Args arg)
        //{
        //    Bitmap dispBm = arg["rawImage"] as Bitmap;
        //    display_pbx.Image = dispBm; // to display image on to the picture box
        //}
        /// <summary>
        /// The event which gets fired when a rotary knob of the camera is moved which indicated change in focus
        /// </summary>
        /// <param name="s">Rotary update event name</param>
        /// <param name="arg">the arg contains the whether the focus knob has moved to negative or positive diaptor</param>
        void RotaryDisplayDiaptor(string s, Args arg)
        {
            if (arg.ContainsKey("SensorPosition"))
            {
                    if (arg.ContainsKey("LeftBitmap"))
                        negative_pbx.Image = arg["LeftBitmap"] as Bitmap;
               
                if (arg.ContainsKey("RightBitmap"))
                        positive_pbx.Image = arg["RightBitmap"] as Bitmap;
               
                  
                if (arg.ContainsKey("RightArrowBitmap"))
                        postiveArrow_pbx.Image = arg["RightArrowBitmap"] as Bitmap;
                  
                if (arg.ContainsKey("LeftArrowBitmap"))
                        negativeArrow_pbx.Image = arg["LeftArrowBitmap"] as Bitmap;
            }
        }
        /// <summary>
        /// Event which gets fired when the power is connected or disconnected
        /// </summary>
        /// <param name="s"></param>
        /// <param name="arg">contains the status whether there was device removal or arrival true indicating the device has arrived false indicating the device has been removed</param>
        void ShowPowerConnection(string s, Args arg)
        {

        }
        /// <summary>
        /// Event which gets fired when the camera is connected or disconnected
        /// </summary>
        /// <param name="s"></param>
        /// <param name="arg">contains the status whether there was camera removal or arrival true indicating the camera has arrived false indicating the camera has been removed</param>
        void ShowCameraConnection(string s, Args arg)
        {

        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Space)
            {
                if(IVL_CAM.isCameraOpen && IVL_CAM.CameraIsLive)
                {
                    IVL_CAM.CaptureStartTime = DateTime.Now;
                    IVL_CAM.HowImageWasCaptured = IntucamHelper.CapturedUIMode.SpaceBar;
                    IVL_CAM.Trigger_Or_SpacebarPressed(false);

                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void leftEyeSide_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (leftEyeSide_rb.Checked)
            {
                IVL_CAM.camPropsHelper.LeftRightPos = LeftRightPosition.Left;
            }
            else
            {
                IVL_CAM.camPropsHelper.LeftRightPos = LeftRightPosition.Right;

            }
        }
        /// <summary>
        /// Change mode from posterior to anterior or vice versa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void posteriorMode_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (IVL_CAM != null)
            {
                if (posteriorMode_rb.Checked)
                {
                    if (IVL_CAM.camPropsHelper.ImagingMode != ImagingMode.Posterior_Prime)
                    {
                        IVL_CAM.camPropsHelper.ImagingMode = ImagingMode.Posterior_Prime;
                        IVL_CAM.ChangeMode();
                    }
                }
                else
                {
                    if (IVL_CAM.camPropsHelper.ImagingMode != ImagingMode.Anterior_Prime)
                    {
                        IVL_CAM.camPropsHelper.ImagingMode = ImagingMode.Anterior_Prime;
                        IVL_CAM.ChangeMode();

                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CameraApiSampleWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IVL_CAM.isCameraOpen)// If the camera is already open 
                IVL_CAM.DisconnectCameraModule();// Then disconnect the camera when the application is closed
        }


        private void FrameStatusUpdater(string s, Args arg)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CameraApiSampleWindow_Shown(object sender, EventArgs e)
        {
            IVL_CAM.camPropsHelper.RightBitmap = new Bitmap(positive_pbx.ClientSize.Width, positive_pbx.ClientSize.Height);
            IVL_CAM.camPropsHelper.LeftBitmap = new Bitmap(positive_pbx.ClientSize.Width, positive_pbx.ClientSize.Height);
            IVL_CAM.camPropsHelper.resetBitmapLeft = new Bitmap(negativeArrow_pbx.ClientSize.Width, negativeArrow_pbx.ClientSize.Height);
            IVL_CAM.camPropsHelper.resetBitmapRight = new Bitmap(negativeArrow_pbx.ClientSize.Width, negativeArrow_pbx.ClientSize.Height);
            IVL_CAM.camPropsHelper.negativearrowSymbol = new Bitmap(negativeArrow_pbx.ClientSize.Width, negativeArrow_pbx.ClientSize.Height);
            IVL_CAM.camPropsHelper.positivearrowSymbol = new Bitmap(negativeArrow_pbx.ClientSize.Width, negativeArrow_pbx.ClientSize.Height);
            connectCamera_btn.Enabled = true;
        }


    }
}
