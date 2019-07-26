using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using INTUSOFT.Imaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace INTUSOFT.Desktop.Forms
{
    public partial class LiveImageControls45_UC : UserControl
    {
        #region variables and constants
        private GainExposureHelper _liveGainExposure;
        private static LiveImageControls45_UC _liveimageControls45_UC;
        int maxExposureIndex = 63;
        #endregion

        public LiveImageControls45_UC()
        {
            InitializeComponent();
            InitializeResourceString();
            maxExposureIndex = Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._exposureIndex.val);
            IntucamHelper._updateAnalogvalDigitalVal+=IntucamHelper__updateAnalogVal2DigitalVal;
            _liveGainExposure = GainExposureHelper.getInstance();
            if (Screen.PrimaryScreen.Bounds.Width == 1920)
            {
                increaseCaptureGainExposure_btn.Size = decreaseCaptureGainExposure_btn.Size = increaseLiveGainExposure_btn.Size = decreaseLiveGainExposure_btn.Size =
                    liveEG_lbl.Size = captureEG_lbl.Size = new Size(80, 80);
                increaseCaptureGainExposure_btn.Margin = decreaseCaptureGainExposure_btn.Margin = increaseLiveGainExposure_btn.Margin = decreaseLiveGainExposure_btn.Margin =
                    liveEG_lbl.Margin = captureEG_lbl.Margin = new Padding(10, 1, 0, 2);
            }
            else if (Screen.PrimaryScreen.Bounds.Width == 1366)
            {
                increaseCaptureGainExposure_btn.Size = decreaseCaptureGainExposure_btn.Size = increaseLiveGainExposure_btn.Size = decreaseLiveGainExposure_btn.Size =
                   liveEG_lbl.Size = captureEG_lbl.Size = new Size(60, 60);
                //increaseCaptureGainExposure_btn.Margin = decreaseCaptureGainExposure_btn.Margin = increaseLiveGainExposure_btn.Margin = decreaseLiveGainExposure_btn.Margin =
                //       liveEG_lbl.Margin = captureEG_lbl.Margin = new Padding(10, 1, 0, 2);
            }
            else if (Screen.PrimaryScreen.Bounds.Width == 1280)
            {
                increaseCaptureGainExposure_btn.Size = decreaseCaptureGainExposure_btn.Size = increaseLiveGainExposure_btn.Size = decreaseLiveGainExposure_btn.Size =
                   liveEG_lbl.Size = captureEG_lbl.Size = new Size(60, 60);
            }
            //toolStrip1.Renderer = new Custom.Controls.FormToolStripRenderer();
            //toolStrip2.Renderer = new Custom.Controls.FormToolStripRenderer();


            this.Resize += LiveImageControls45_UC_Resize;
            this.SizeChanged += LiveImageControls45_UC_SizeChanged;
        }

        void LiveImageControls45_UC_SizeChanged(object sender, EventArgs e)
        {
        }

        public void DisableLiveorCaptureControls(bool isLive, bool isDisabled)
        {
            if(isLive)
            {
                toolStrip1.Enabled = isDisabled;
            }
            else
            {
                toolStrip2.Enabled = isDisabled;
            }
        }
        void LiveImageControls45_UC_Resize(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// This event is triggered when the intensity nob is rotated.
        /// </summary>
        /// <param name="val">value from the intucamhelper</param>
        void IntucamHelper__updateAnalogVal2DigitalVal(int val)
        {
            //if (Convert.ToBoolean(IVLVariables.CurrentSettings.FirmwareSettings._EnablePCUKnob.val))// && IVLVariables.ivl_Camera.islive)
            {
                //if (val < 0)
                //{
                //    int liveGainVal = Convert.ToInt32(liveEG_lbl.Text);
                //    // if(liveGainVal < 0)
                //    liveGainVal = Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._KnobIndexDifferenceValue.val) + (liveGainVal);
                //    //else
                //    //    liveGainVal = Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._KnobIndexDifferenceValue.val) + liveGainVal;
                //    int[] gainExpArr = IVLVariables.ivl_Camera.camPropsHelper.GetExposureGainFromTable(liveGainVal);// gain exposure value from lut
                //    IVLVariables.ivl_Camera.camPropsHelper._Settings.CameraSettings.LiveExposure = (uint)gainExpArr[0];// apply lut exposure to settings of live exposure
                //    IVLVariables.ivl_Camera.camPropsHelper._Settings.CameraSettings.LiveGain = (ushort)gainExpArr[1];// apply lut gain to settings of live gain
                 //   gainExposureHelper.SetLiveGain(liveGainVal);
                //}
                //else
                {
                    
                    int calculatedExposureGainValue = val - Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._KnobIndexDifferenceValue.val);
                    IVLVariables.CurrentSettings.CameraSettings._LiveDigitalGain.val = val.ToString();
                    // MessageBox.Show(calculatedExposureGainValue.ToString());
                  //  gainExposureHelper.SetLiveGain(val);
                  //  int[] gainExpArr = IVLVariables.ivl_Camera.camPropsHelper.GetExposureGainFromTable(val);// gain exposure value from lut
                  //IVLVariables.ivl_Camera.camPropsHelper._Settings.CameraSettings.LiveExposure = (uint)gainExpArr[0];// apply lut exposure to settings of live exposure
                  //IVLVariables.ivl_Camera.camPropsHelper._Settings.CameraSettings.LiveGain = (ushort) gainExpArr[1];// apply lut gain to settings of live gain
                    liveEG_lbl.Text = calculatedExposureGainValue.ToString();
                }
                int CaptureExpIndx = Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._DigitalGain.val) - Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._KnobIndexDifferenceValue.val);// this has been changed from exposure index to digital gain
                captureEG_lbl.Text = CaptureExpIndx.ToString();
            }
        }

        /// <summary>
        /// Returns a single instance of type LiveImageControls45_UC.
        /// </summary>
        /// <returns>Returns instance of type LiveImageControls45_UC</returns>
        public static LiveImageControls45_UC getInstance()
        {
            if (_liveimageControls45_UC == null)
                _liveimageControls45_UC = new LiveImageControls45_UC();
            return _liveimageControls45_UC;
        }

        /// <summary>
        /// Assigns the text for each labels and buttons in the LiveImageControls45_UC class.
        /// </summary>
        private void InitializeResourceString()
        {
            exp_lbl.Text = IVLVariables.LangResourceManager.GetString("Exposure_Label_Text", IVLVariables.LangResourceCultureInfo);
            increaseCaptureGainExposure_btn.Text = IVLVariables.LangResourceManager.GetString("ImageViewer_Increase_Button_Text", IVLVariables.LangResourceCultureInfo);
            decreaseCaptureGainExposure_btn.Text = IVLVariables.LangResourceManager.GetString("ImageViewer_Decrease_Button_Text", IVLVariables.LangResourceCultureInfo);
            increaseLiveGainExposure_btn.Text = IVLVariables.LangResourceManager.GetString("ImageViewer_Increase_Button_Text", IVLVariables.LangResourceCultureInfo);
            decreaseLiveGainExposure_btn.Text = IVLVariables.LangResourceManager.GetString("ImageViewer_Decrease_Button_Text", IVLVariables.LangResourceCultureInfo);
            liveGain_lbl.Text = IVLVariables.LangResourceManager.GetString("IR_Intensity_Label_Text", IVLVariables.LangResourceCultureInfo);
        }

        /// <summary>
        /// This event is triggered when increase button of gain is clicked It will set the newly calculated gain value to liveGain_tbx text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void increaseLiveGainExposure_btn_Click(object sender, EventArgs e)
        {
            if (!IVLVariables.ivl_Camera.IsCapturing && !IVLVariables.ivl_Camera.IsResuming && !IVLVariables.ivl_Camera.isResetMode && IVLVariables.ivl_Camera.IsMotorMovementDone)

            if (Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._LiveDigitalGain.val) < maxExposureIndex)
                {
                    int i = Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._LiveDigitalGain.val);
                    i++;//= 10;
                    _liveGainExposure.SetLiveGain(i);
                    int displayText = i - Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._KnobIndexDifferenceValue.val);
                    liveEG_lbl.Text = displayText.ToString();
                }
        }

        /// <summary>
        /// This event is triggered when decreaseLiveGain button of gain is clicked. It will set the newly calculated gain value to liveGain_tbx text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void decreaseLiveGainExposure_btn_Click(object sender, EventArgs e)
        {
            if (!IVLVariables.ivl_Camera.IsCapturing && !IVLVariables.ivl_Camera.IsResuming && !IVLVariables.ivl_Camera.isResetMode && IVLVariables.ivl_Camera.        IsMotorMovementDone)

                if (Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._LiveDigitalGain.val) > 0)
                {
                    int i = Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._LiveDigitalGain.val);
                    i--;
                    int displayText = i - Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._KnobIndexDifferenceValue.val);
                    _liveGainExposure.SetLiveGain(i);
                    liveEG_lbl.Text = displayText.ToString();
                }
        }

        private void decreaseCaptureGainExposure_btn_Click(object sender, EventArgs e)
        {
            if (!IVLVariables.ivl_Camera.IsCapturing && !IVLVariables.ivl_Camera.IsResuming && !IVLVariables.ivl_Camera.isResetMode && IVLVariables.ivl_Camera.        IsMotorMovementDone)

            if (Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._DigitalGain.val) > 0)
            {
                int i = Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._DigitalGain.val);
                i = i - 1;
                IVLVariables.CurrentSettings.CameraSettings._DigitalGain.val = i.ToString();// this has been changed from exposure index to digital gain
                captureEG_lbl.Text = (i - Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._KnobIndexDifferenceValue.val)).ToString();
            }
        }

        private void increaseCaptureGainExposure_btn_Click(object sender, EventArgs e)
        {
            if (!IVLVariables.ivl_Camera.IsCapturing && !IVLVariables.ivl_Camera.IsResuming && !IVLVariables.ivl_Camera.isResetMode && IVLVariables.ivl_Camera.        IsMotorMovementDone)

            if (Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._DigitalGain.val) < maxExposureIndex)
            {
                int i = Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._DigitalGain.val);
                i = i + 1;
                IVLVariables.CurrentSettings.CameraSettings._DigitalGain.val = i.ToString();// this has been changed from exposure index to digital gain
                captureEG_lbl.Text = (i - Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._KnobIndexDifferenceValue.val)).ToString();
            }
        }
    }
}
