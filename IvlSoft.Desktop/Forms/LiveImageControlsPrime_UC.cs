using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using INTUSOFT.Data.Repository;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using INTUSOFT.EventHandler;
using CameraModule;
using System.IO;
using INTUSOFT.Custom.Controls;
using System.Threading;
using INTUSOFT.Imaging;
using INTUSOFT.Desktop.Properties;



namespace INTUSOFT.Desktop.Forms
{
    public partial class LiveImageControlsPrime_UC : UserControl
    {
        #region variables and constants
        GainExposureHelper _liveGainExp;
        public static LiveImageControlsPrime_UC _liveimageControlsPrime;
        #endregion

        public LiveImageControlsPrime_UC()
        {
            InitializeComponent(); 
            InitializeResourceString();
            _liveGainExp = GainExposureHelper.getInstance();
            
            //toolStrip1.Renderer = new Custom.Controls.FormToolStripRenderer();
            
            string highIntLogoFilePath = @"ImageResources\IntensityImageResources\High_Light.png";
            string LowIntLogoFilePath = @"ImageResources\IntensityImageResources\Low_Light.png";
            string MedIntLogoFilePath = @"ImageResources\IntensityImageResources\Med_Light.png";
       
            if (File.Exists(highIntLogoFilePath))
            {
                Bitmap bm = new Bitmap(highIntLogoFilePath);// Bitmap bm has been added so as to reduce the time of new bitmapping everytime
                highFlashGain_btn.Image =bm;//, 256, 256);
                highLiveGain_btn.Image = bm;//, 256, 256);
            }
            if (File.Exists(MedIntLogoFilePath))
            {
                Bitmap bm = new Bitmap(MedIntLogoFilePath);// Bitmap bm has been added so as to reduce the time of new bitmapping everytime
                mediumFlashGain_btn.Image = bm;//, 256, 256);
                mediumLiveGain_btn.Image = bm;//, 256, 256);
            }
            if (File.Exists(LowIntLogoFilePath))
            {
                Bitmap bm = new Bitmap(LowIntLogoFilePath);// Bitmap bm has been added so as to reduce the time of new bitmapping everytime
                LowFlashGain_btn.Image =bm;///, 256, 256);
                lowLiveGain_btn.Image = bm;//, 256, 256);
            }
            if (Screen.PrimaryScreen.Bounds.Width == 1920)
            {
                lowLiveGain_btn.Size = mediumLiveGain_btn.Size = highLiveGain_btn.Size = LowFlashGain_btn.Size =
                    mediumFlashGain_btn.Size = highFlashGain_btn.Size = new Size(80, 80);
                lowLiveGain_btn.Margin = mediumLiveGain_btn.Margin = highLiveGain_btn.Margin = LowFlashGain_btn.Margin =
                    mediumFlashGain_btn.Margin = highFlashGain_btn.Margin = new Padding(10, 1, 0, 2);
            }
            else if (Screen.PrimaryScreen.Bounds.Width == 1366)
            {
                lowLiveGain_btn.Size = mediumLiveGain_btn.Size = highLiveGain_btn.Size = LowFlashGain_btn.Size =
                mediumFlashGain_btn.Size = highFlashGain_btn.Size = new Size(60, 60);// The size has been changed to 60, 60 from 150,80 by sriram
                //lowLiveGain_btn.Margin = mediumLiveGain_btn.Margin = highLiveGain_btn.Margin = LowFlashGain_btn.Margin =
                //    mediumFlashGain_btn.Margin = highFlashGain_btn.Margin = new Padding(10, 1, 0, 2);
                //increaseCaptureGainExposure_btn.Size = decreaseCaptureGainExposure_btn.Size = increaseLiveGainExposure_btn.Size = decreaseLiveGainExposure_btn.Size =
                //   liveEG_lbl.Size = captureEG_lbl.Size = new Size(150, 80);
                //increaseCaptureGainExposure_btn.Margin = decreaseCaptureGainExposure_btn.Margin = increaseLiveGainExposure_btn.Margin = decreaseLiveGainExposure_btn.Margin =
                //       liveEG_lbl.Margin = captureEG_lbl.Margin = new Padding(10, 1, 0, 2);
            }
            else if (Screen.PrimaryScreen.Bounds.Width == 1280)
            {

                lowLiveGain_btn.Size = mediumLiveGain_btn.Size = highLiveGain_btn.Size = LowFlashGain_btn.Size =
                   mediumFlashGain_btn.Size = highFlashGain_btn.Size = new Size(60, 60);
                //increaseCaptureGainExposure_btn.Size = decreaseCaptureGainExposure_btn.Size = increaseLiveGainExposure_btn.Size = decreaseLiveGainExposure_btn.Size =
                //   liveEG_lbl.Size = captureEG_lbl.Size = new Size(60, 60);
            }

            //// This has been added to fix the defect of live gain to 77 by default by sriram this avoids the gain setting to 77 
            //if (IVLVariables.ivl_Camera.ImagingMode == ImagingMode.Anterior_Prime ) // changed the condition from and to or by sriram to handle both prime and anterior
            //{


            //    IVLVariables.CurrentLiveGain = (GainLevels)Enum.Parse(typeof(GainLevels), IVLVariables.CurrentSettings.CameraSettings._LiveGainDefault.val); // get default values from settings for live 
            //    IVLVariables.CurrentCaptureGain = (GainLevels)Enum.Parse(typeof(GainLevels), IVLVariables.CurrentSettings.CameraSettings._DigitalGainDefault.val);// get default values from setting for capture by sriram
            //    RefreshFlashGainButtons(IVLVariables.CurrentLiveGain);
            //    RefreshLiveGainButtons( IVLVariables.CurrentCaptureGain);
            //}
            //else if( IVLVariables.ivl_Camera.ImagingMode == ImagingMode.Posterior_Prime)
            //{
            //    IVLVariables.CurrentLiveGain = (GainLevels)Enum.Parse(typeof(GainLevels), IVLVariables.CurrentSettings.CameraSettings._LiveGainDefault.val); // get default values from settings for live 
            //    IVLVariables.CurrentCaptureGain = (GainLevels)Enum.Parse(typeof(GainLevels), IVLVariables.CurrentSettings.CameraSettings._DigitalGainDefault.val);// get default values from setting for capture by sriram
            //    RefreshFlashGainButtons(IVLVariables.CurrentLiveGain);
            //    RefreshLiveGainButtons(IVLVariables.CurrentCaptureGain);
            //}
            
        }
            

        #region public methods
        /// <summary>
        /// This method will return a single instance of the LiveImageControlsPrime_UC.
        /// </summary>
        /// <returns>instance of the LiveImageControlsPrime_UC</returns>
        public static LiveImageControlsPrime_UC getInstance()
        {
            if(_liveimageControlsPrime==null)
                _liveimageControlsPrime = new LiveImageControlsPrime_UC(); ;
            return _liveimageControlsPrime;
        }

        /// <summary>
        /// This function will change the colors of the buttons and set the flash exposure value to IVLVariables.ivl_Camera.CaptureGain.
        /// </summary>
        /// <param name="ch">ch indicates the GainLevels</param>
        public void RefreshFlashGainButtons(GainLevels ch)
        {

            switch (ch)
            {
                case GainLevels.Low:
                    {
                        ButtonHighlight(LowFlashGain_btn, mediumFlashGain_btn, highFlashGain_btn);
                        IVLVariables.ivl_Camera.camPropsHelper.CaptureGainLevel = Imaging.GainLevels.Low;
                       // _liveGainExp.SetFlashGain(Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._DigitalGainLow.val));
                    }
                    break;
                case GainLevels.Medium:
                    {
                        ButtonHighlight(mediumFlashGain_btn, LowFlashGain_btn, highFlashGain_btn);
                        IVLVariables.ivl_Camera.camPropsHelper.CaptureGainLevel = Imaging.GainLevels.Medium;

                       // _liveGainExp.SetFlashGain(Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._DigitalGainMedium.val));
                    }
                    break;
                case GainLevels.High:
                    {
                        ButtonHighlight(highFlashGain_btn, mediumFlashGain_btn, LowFlashGain_btn);

                        IVLVariables.ivl_Camera.camPropsHelper.CaptureGainLevel = Imaging.GainLevels.High;

                        //_liveGainExp.SetFlashGain(Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._DigitalGainHigh.val));
                    }
                    break;
                default:
                    {
                        RefreshFlashGainButtons(GainLevels.Low);
                    }
                    break;
            }
        }

        /// <summary>
        /// This method is used to highlight a specific button.
        /// </summary>
        /// <param name="button1">button to be highlighted</param>
        /// <param name="button2">button not to be highlighted</param>
        /// <param name="button3">button not to be highlighted</param>
        public void ButtonHighlight(Button button1, Button button2, Button button3)
        {
            button1.BackColor = Color.Yellow;
            button2.BackColor = Color.Khaki;
            button3.BackColor = Color.Khaki;
        }
        public void ButtonHighlight(ToolStripButton button1, ToolStripButton button2, ToolStripButton button3)
        {
            button1.BackColor = Color.Yellow;
            button2.BackColor = Color.Khaki;
            button3.BackColor = Color.Khaki;
        }
        /// <summary>
        /// This function will change the colors of the buttons and set the gain value to IVLVariables.ivl_Camera.LiveGain.
        /// </summary>
        /// <param name="ch">gainlevel</param>
        public void RefreshLiveGainButtons(GainLevels ch)
        {

            switch (ch)
            {
                case GainLevels.Low:
                    {
                        ButtonHighlight(lowLiveGain_btn, mediumLiveGain_btn, highLiveGain_btn);
                        IVLVariables.ivl_Camera.camPropsHelper.LiveGainLevel = Imaging.GainLevels.Low;

                          //  _liveGainExp.SetLiveGain(Convert.ToInt32((IVLVariables.CurrentSettings.CameraSettings._LiveGainLow.val)));
                    }
                    break;
                case GainLevels.Medium:
                    {
                        ButtonHighlight(mediumLiveGain_btn, lowLiveGain_btn, highLiveGain_btn);
                        IVLVariables.ivl_Camera.camPropsHelper.LiveGainLevel = Imaging.GainLevels.Medium;

                       // _liveGainExp.SetLiveGain(Convert.ToInt32((IVLVariables.CurrentSettings.CameraSettings._LiveGainMedium.val)));
                    }
                    break;
                case GainLevels.High:
                    {
                        ButtonHighlight(highLiveGain_btn, mediumLiveGain_btn, lowLiveGain_btn);
                        IVLVariables.ivl_Camera.camPropsHelper.LiveGainLevel = Imaging.GainLevels.High;

                        // _liveGainExp.SetLiveGain(Convert.ToInt32((IVLVariables.CurrentSettings.CameraSettings._LiveGainHigh.val)));
                    }
                    break;
                default:
                    {
                        RefreshLiveGainButtons(GainLevels.Low);
                    }
                    break;
            }
        }

        #endregion

        /// <summary>
        /// This function will intitialize all label text in the UC from the resources.
        /// </summary>
        private void InitializeResourceString()
        {
            liveGain_lbl.Text = IVLVariables.LangResourceManager.GetString("LiveGain_Label_Text", IVLVariables.LangResourceCultureInfo);
            lowLiveGain_btn.Text = IVLVariables.LangResourceManager.GetString("LowLiveGain_Text", IVLVariables.LangResourceCultureInfo);
            LowFlashGain_btn.Text = IVLVariables.LangResourceManager.GetString("LowLiveGain_Text", IVLVariables.LangResourceCultureInfo);
            mediumLiveGain_btn.Text = IVLVariables.LangResourceManager.GetString("MediumLiveGain_Text", IVLVariables.LangResourceCultureInfo);
            mediumFlashGain_btn.Text = IVLVariables.LangResourceManager.GetString("MediumLiveGain_Text", IVLVariables.LangResourceCultureInfo);
            highLiveGain_btn.Text = IVLVariables.LangResourceManager.GetString("HighLiveGain_Text", IVLVariables.LangResourceCultureInfo);
            highFlashGain_btn.Text = IVLVariables.LangResourceManager.GetString("HighLiveGain_Text", IVLVariables.LangResourceCultureInfo);
            flashGain_lbl.Text = IVLVariables.LangResourceManager.GetString("FlashGain_Label_Text", IVLVariables.LangResourceCultureInfo);
        }

        #region private events

        /// <summary>
        /// This event is used to assign the high gain value to the current gain and invokes the RefreshLiveGainButtons method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void highLiveGain_btn_Click(object sender, EventArgs e)
        {
            if (!IVLVariables.ivl_Camera.IsCapturing && !IVLVariables.ivl_Camera.IsResuming && !IVLVariables.ivl_Camera.isResetMode && IVLVariables.ivl_Camera.        IsMotorMovementDone)

            if (IVLVariables.ivl_Camera.camPropsHelper.IsCameraConnected == Devices.CameraConnected)
            {
                 RefreshLiveGainButtons(IVLVariables.CurrentLiveGain =  GainLevels.High);;

                //Live current gain value from variable to config by sriram

                IVLVariables.CurrentSettings.CameraSettings.LiveCurrentGainLevel.val = IVLVariables.CurrentLiveGain.ToString();
               
            }
        }

        /// <summary>
        /// This event is used to assign the medium gain to the current gain and invokes the RefreshLiveGainButtons method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mediumLiveGain_btn_Click(object sender, EventArgs e)
        {
            if (!IVLVariables.ivl_Camera.IsCapturing && !IVLVariables.ivl_Camera.IsResuming && !IVLVariables.ivl_Camera.isResetMode && IVLVariables.ivl_Camera.        IsMotorMovementDone)

            if (IVLVariables.ivl_Camera.camPropsHelper.IsCameraConnected == Devices.CameraConnected)
            {
                RefreshLiveGainButtons(IVLVariables.CurrentLiveGain = GainLevels.Medium);
                //Live current gain value from variable to config by sriram
                IVLVariables.CurrentSettings.CameraSettings.LiveCurrentGainLevel.val = IVLVariables.CurrentLiveGain.ToString();
            }
        }

        /// <summary>
        /// This event is used to assign the low gain to the current gain and invokes the RefreshLiveGainButtons method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lowLiveGain_btn_Click(object sender, EventArgs e)
        {
            if (!IVLVariables.ivl_Camera.IsCapturing && !IVLVariables.ivl_Camera.IsResuming && !IVLVariables.ivl_Camera.isResetMode && IVLVariables.ivl_Camera.        IsMotorMovementDone)

            if (IVLVariables.ivl_Camera.camPropsHelper.IsCameraConnected == Devices.CameraConnected)
            {
                RefreshLiveGainButtons( IVLVariables.CurrentLiveGain = GainLevels.Low);
                //Live current gain value from variable to config by sriram
                IVLVariables.CurrentSettings.CameraSettings.LiveCurrentGainLevel.val = IVLVariables.CurrentLiveGain.ToString();
              
            }
        }

        /// <summary>
        /// This event is used to assign the high flash value to the CurrentCaptureGain and invokes the RefreshFlashGainButtons method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void highFlashGain_btn_Click(object sender, EventArgs e)
        {
            if (!IVLVariables.ivl_Camera.IsCapturing && !IVLVariables.ivl_Camera.IsResuming && !IVLVariables.ivl_Camera.isResetMode && IVLVariables.ivl_Camera.        IsMotorMovementDone)

            if (IVLVariables.ivl_Camera.camPropsHelper.IsCameraConnected == Devices.CameraConnected)
            {
                RefreshFlashGainButtons(IVLVariables.CurrentCaptureGain = GainLevels.High);
                
                //capture current gain value from variable to config by sriram
                IVLVariables.CurrentSettings.CameraSettings.CaptureCurrentGainLevel.val = IVLVariables.CurrentCaptureGain.ToString();
            }
        }

        /// <summary>
        /// This event is used to assign the medium flash value to the CurrentCaptureGain and invokes the RefreshFlashGainButtons method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mediumFlashGain_btn_Click(object sender, EventArgs e)
        {
            if (!IVLVariables.ivl_Camera.IsCapturing && !IVLVariables.ivl_Camera.IsResuming && !IVLVariables.ivl_Camera.isResetMode && IVLVariables.ivl_Camera.        IsMotorMovementDone)

            if (IVLVariables.ivl_Camera.camPropsHelper.IsCameraConnected == Devices.CameraConnected)
            {
                RefreshFlashGainButtons(IVLVariables.CurrentCaptureGain = GainLevels.Medium);;
                //capture current gain value from variable to config by sriram
                IVLVariables.CurrentSettings.CameraSettings.CaptureCurrentGainLevel.val = IVLVariables.CurrentCaptureGain.ToString();
                
            }
        }

        /// <summary>
        /// This event is used to assign the low flash value to the CurrentCaptureGain and invokes the RefreshFlashGainButtons method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LowFlashGain_btn_Click(object sender, EventArgs e)
        {
            if (!IVLVariables.ivl_Camera.IsCapturing && !IVLVariables.ivl_Camera.IsResuming && !IVLVariables.ivl_Camera.isResetMode && IVLVariables.ivl_Camera.        IsMotorMovementDone)

            if (IVLVariables.ivl_Camera.camPropsHelper.IsCameraConnected == Devices.CameraConnected)
            {
                 RefreshFlashGainButtons(IVLVariables.CurrentCaptureGain = GainLevels.Low);;
                //capture current gain value from variable to config by sriram
                IVLVariables.CurrentSettings.CameraSettings.CaptureCurrentGainLevel.val = IVLVariables.CurrentCaptureGain.ToString();
               
            }
        }
        #endregion
    }
}
