using System.Windows.Forms;
using System.Drawing;
using System;
using INTUSOFT.Custom.Controls;
namespace AssemblySoftware
{
    partial class CameraUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.cameraStatus_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.Resolution_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.FrameRate_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.FrameStatus_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.resolution_combx = new System.Windows.Forms.ToolStripComboBox();
            this.temperaturStatus_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.tintStatus_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.cameraConnection_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.powerConnection_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.ComPortStatus_lbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.control_tb = new System.Windows.Forms.TabControl();
            this.basicControls_tbp = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.ExpGain_gbx = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.expVal_nud = new System.Windows.Forms.NumericUpDown();
            this.gainVal_nud = new System.Windows.Forms.NumericUpDown();
            this.gain_lbl = new System.Windows.Forms.Label();
            this.exp_lbl = new System.Windows.Forms.Label();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.captureExpGain_gbx = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.flashGain_nud = new System.Windows.Forms.NumericUpDown();
            this.label33 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.flashExp_nud = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.label34 = new System.Windows.Forms.Label();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.trigger_lbl = new System.Windows.Forms.Label();
            this.cameraModel_cmbx = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel21 = new System.Windows.Forms.TableLayoutPanel();
            this.mode_cmbx = new System.Windows.Forms.ComboBox();
            this.label51 = new System.Windows.Forms.Label();
            this.tableLayoutPanel22 = new System.Windows.Forms.TableLayoutPanel();
            this.CCMode_rb = new System.Windows.Forms.RadioButton();
            this.rawModeFrameGrab_rb = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.ir_rb = new System.Windows.Forms.RadioButton();
            this.flash_rb = new System.Windows.Forms.RadioButton();
            this.blue_rb = new System.Windows.Forms.RadioButton();
            this.liveAnt_IR_rb = new System.Windows.Forms.RadioButton();
            this.liveAnt_Flash_rb = new System.Windows.Forms.RadioButton();
            this.liveAnt_Blue_rb = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.IrOffsetSteps_nud = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.captureFocusSteps_lbl = new System.Windows.Forms.Label();
            this.flashOffset2_nud = new System.Windows.Forms.NumericUpDown();
            this.flashoffset1_nud = new System.Windows.Forms.NumericUpDown();
            this.panel12 = new System.Windows.Forms.Panel();
            this.LeftRight_lbl = new System.Windows.Forms.Label();
            this.ffaTimerStatus_lbl = new System.Windows.Forms.Label();
            this.powerStatus_pbx = new System.Windows.Forms.PictureBox();
            this.cameraStatus_pbx = new System.Windows.Forms.PictureBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label24 = new System.Windows.Forms.Label();
            this.tableLayoutPanel30 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel31 = new System.Windows.Forms.TableLayoutPanel();
            this.label44 = new System.Windows.Forms.Label();
            this.flashBoost_nud = new System.Windows.Forms.NumericUpDown();
            this.triggerRecieved_pbx = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel34 = new System.Windows.Forms.TableLayoutPanel();
            this.singleFrameCapture_combox = new System.Windows.Forms.ComboBox();
            this.label43 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel17 = new System.Windows.Forms.TableLayoutPanel();
            this.captureIR_rb = new System.Windows.Forms.RadioButton();
            this.captureFlash_rb = new System.Windows.Forms.RadioButton();
            this.captureBlue_rb = new System.Windows.Forms.RadioButton();
            this.captureAnt_IR_rb = new System.Windows.Forms.RadioButton();
            this.captureAnt_Flash_rb = new System.Windows.Forms.RadioButton();
            this.captureAnt_Blue_rb = new System.Windows.Forms.RadioButton();
            this.postProcessing_tbp = new System.Windows.Forms.TabPage();
            this.allChannelLutCbx = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tempTint_gbx = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.temperature_tb = new System.Windows.Forms.TrackBar();
            this.tint_tb = new System.Windows.Forms.TrackBar();
            this.label35 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.TemperatureVal_lbl = new System.Windows.Forms.Label();
            this.tintVal_lbl = new System.Windows.Forms.Label();
            this.applyChannelWiseLutLklbl = new System.Windows.Forms.LinkLabel();
            this.linkLabel4 = new System.Windows.Forms.LinkLabel();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.contrastSettings_lnklbl = new System.Windows.Forms.LinkLabel();
            this.brightnessSettings_lnklbl = new System.Windows.Forms.LinkLabel();
            this.maskSettings_lnkLbl = new System.Windows.Forms.LinkLabel();
            this.ccSettings_lnkLbl = new System.Windows.Forms.LinkLabel();
            this.ApplyLiteCorrection_cbx = new System.Windows.Forms.CheckBox();
            this.hsSettings_lnkLbl = new System.Windows.Forms.LinkLabel();
            this.ApplyContrast_cbx = new System.Windows.Forms.CheckBox();
            this.shiftSettings_lnkLbl = new System.Windows.Forms.LinkLabel();
            this.button4 = new System.Windows.Forms.Button();
            this.applyBrightness_cbx = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.PostProcessing_btn = new System.Windows.Forms.Button();
            this.reloadImage_btn = new System.Windows.Forms.Button();
            this.browseRawImage_btn = new System.Windows.Forms.Button();
            this.shiftImage_tbp = new System.Windows.Forms.TabPage();
            this.ApplyShiftImage_btn = new System.Windows.Forms.Button();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.Yshift_nud = new System.Windows.Forms.NumericUpDown();
            this.xShift_nud = new System.Windows.Forms.NumericUpDown();
            this.label19 = new System.Windows.Forms.Label();
            this.ColorCorrection_tbp = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel20 = new System.Windows.Forms.TableLayoutPanel();
            this.label49 = new System.Windows.Forms.Label();
            this.label50 = new System.Windows.Forms.Label();
            this.brightness_nud = new System.Windows.Forms.NumericUpDown();
            this.contrast_nud = new System.Windows.Forms.NumericUpDown();
            this.offset1_nud = new System.Windows.Forms.NumericUpDown();
            this.LUT_interval2_nud = new System.Windows.Forms.NumericUpDown();
            this.LUT_interval1_nud = new System.Windows.Forms.NumericUpDown();
            this.LUT_SineFactor_nud = new System.Windows.Forms.NumericUpDown();
            this.hsvBoostVal_nud = new System.Windows.Forms.NumericUpDown();
            this.medianfilter_nud = new System.Windows.Forms.NumericUpDown();
            this.label71 = new System.Windows.Forms.Label();
            this.label72 = new System.Windows.Forms.Label();
            this.label70 = new System.Windows.Forms.Label();
            this.label69 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label57 = new System.Windows.Forms.Label();
            this.label55 = new System.Windows.Forms.Label();
            this.unsharpAmount_nud = new System.Windows.Forms.NumericUpDown();
            this.unsharpRadius_nud = new System.Windows.Forms.NumericUpDown();
            this.unsharpThresh_nud = new System.Windows.Forms.NumericUpDown();
            this.ClaheClipValueR_nud = new System.Windows.Forms.NumericUpDown();
            this.label56 = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.label54 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.ClaheClipValueG_nud = new System.Windows.Forms.NumericUpDown();
            this.ClaheClipValueB_nud = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel29 = new System.Windows.Forms.TableLayoutPanel();
            this.gammaVal_nud = new System.Windows.Forms.NumericUpDown();
            this.label48 = new System.Windows.Forms.Label();
            this.showLut_btn = new System.Windows.Forms.Button();
            this.ApplyCC_btn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.blueGreen_nud = new System.Windows.Forms.NumericUpDown();
            this.greenGreen_nud = new System.Windows.Forms.NumericUpDown();
            this.redGreen_nud = new System.Windows.Forms.NumericUpDown();
            this.blueBlue_nud = new System.Windows.Forms.NumericUpDown();
            this.blueRed_nud = new System.Windows.Forms.NumericUpDown();
            this.greenBlue_nud = new System.Windows.Forms.NumericUpDown();
            this.greenRed_nud = new System.Windows.Forms.NumericUpDown();
            this.redBlue_nud = new System.Windows.Forms.NumericUpDown();
            this.redRed_nud = new System.Windows.Forms.NumericUpDown();
            this.Mask_tbp = new System.Windows.Forms.TabPage();
            this.label52 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.label73 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label74 = new System.Windows.Forms.Label();
            this.batchMask_btn = new System.Windows.Forms.Button();
            this.ClearMaskMarking_btn = new System.Windows.Forms.Button();
            this.applyMask_btn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.CaptureMaskHeight_nud = new System.Windows.Forms.NumericUpDown();
            this.CaptureMaskWidth_nud = new System.Windows.Forms.NumericUpDown();
            this.LiveMaskHeight_nud = new System.Windows.Forms.NumericUpDown();
            this.LiveMaskWidth_nud = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.maskY_nud = new System.Windows.Forms.NumericUpDown();
            this.maskX_nud = new System.Windows.Forms.NumericUpDown();
            this.CentreImage_tbp = new System.Windows.Forms.TabPage();
            this.setCentreImage_btn = new System.Windows.Forms.Button();
            this.misc_tbpg = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.label42 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
            this.left_rb = new System.Windows.Forms.RadioButton();
            this.right_rb = new System.Windows.Forms.RadioButton();
            this.motorSensor_gbx = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel16 = new System.Windows.Forms.TableLayoutPanel();
            this.positivePos_p = new System.Windows.Forms.Panel();
            this.negativePos_p = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel23 = new System.Windows.Forms.TableLayoutPanel();
            this.redGain_tb = new System.Windows.Forms.TrackBar();
            this.label9 = new System.Windows.Forms.Label();
            this.redGainVal_lbl = new System.Windows.Forms.Label();
            this.tableLayoutPanel24 = new System.Windows.Forms.TableLayoutPanel();
            this.greenGain_tb = new System.Windows.Forms.TrackBar();
            this.label10 = new System.Windows.Forms.Label();
            this.greenGainVal_lbl = new System.Windows.Forms.Label();
            this.tableLayoutPanel25 = new System.Windows.Forms.TableLayoutPanel();
            this.blueGain_tb = new System.Windows.Forms.TrackBar();
            this.label11 = new System.Windows.Forms.Label();
            this.blueGainVal_lbl = new System.Windows.Forms.Label();
            this.tableLayoutPanel26 = new System.Windows.Forms.TableLayoutPanel();
            this.liveRedGain_tb = new System.Windows.Forms.TrackBar();
            this.label12 = new System.Windows.Forms.Label();
            this.liveR_val_lbl = new System.Windows.Forms.Label();
            this.tableLayoutPanel27 = new System.Windows.Forms.TableLayoutPanel();
            this.liveGreenGain_tb = new System.Windows.Forms.TrackBar();
            this.label58 = new System.Windows.Forms.Label();
            this.liveG_val_lbl = new System.Windows.Forms.Label();
            this.tableLayoutPanel28 = new System.Windows.Forms.TableLayoutPanel();
            this.liveBlueGain_tb = new System.Windows.Forms.TrackBar();
            this.label60 = new System.Windows.Forms.Label();
            this.liveB_val_lbl = new System.Windows.Forms.Label();
            this.ffa_tbpg = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel18 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel19 = new System.Windows.Forms.TableLayoutPanel();
            this.label46 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.greenFilterPos_nud = new System.Windows.Forms.NumericUpDown();
            this.blueFilterPos_nud = new System.Windows.Forms.NumericUpDown();
            this.panel5 = new System.Windows.Forms.Panel();
            this.eightBit_rb = new System.Windows.Forms.RadioButton();
            this.forteenBit_rb = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel33 = new System.Windows.Forms.TableLayoutPanel();
            this.label75 = new System.Windows.Forms.Label();
            this.label76 = new System.Windows.Forms.Label();
            this.frameDetectionVal_nud = new System.Windows.Forms.NumericUpDown();
            this.darkFameDetectionVal_nud = new System.Windows.Forms.NumericUpDown();
            this.panel11 = new System.Windows.Forms.Panel();
            this.saveFramesCount_nud = new System.Windows.Forms.NumericUpDown();
            this.label45 = new System.Windows.Forms.Label();
            this.panel14 = new System.Windows.Forms.Panel();
            this.button5 = new System.Windows.Forms.Button();
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.FFA_Color_Pot_Int_Offset_nud = new System.Windows.Forms.NumericUpDown();
            this.FFA_Pot_Int_Offset_nud = new System.Windows.Forms.NumericUpDown();
            this.HotSpot_tbp = new System.Windows.Forms.TabPage();
            this.label66 = new System.Windows.Forms.Label();
            this.label67 = new System.Windows.Forms.Label();
            this.shadowRedPeak_nud = new System.Windows.Forms.NumericUpDown();
            this.shadowGreenPeak_nud = new System.Windows.Forms.NumericUpDown();
            this.shadowBluePeak_nud = new System.Windows.Forms.NumericUpDown();
            this.label68 = new System.Windows.Forms.Label();
            this.HsRedPeak_nud = new System.Windows.Forms.NumericUpDown();
            this.HsGreenPeak_nud = new System.Windows.Forms.NumericUpDown();
            this.HsBluePeak_nud = new System.Windows.Forms.NumericUpDown();
            this.HsRedRadius_nud = new System.Windows.Forms.NumericUpDown();
            this.HsGreenRadius_nud = new System.Windows.Forms.NumericUpDown();
            this.HsBlueRadius_nud = new System.Windows.Forms.NumericUpDown();
            this.label59 = new System.Windows.Forms.Label();
            this.label61 = new System.Windows.Forms.Label();
            this.label62 = new System.Windows.Forms.Label();
            this.label63 = new System.Windows.Forms.Label();
            this.label64 = new System.Windows.Forms.Label();
            this.label65 = new System.Windows.Forms.Label();
            this.coOrdinates_lbl = new System.Windows.Forms.Label();
            this.twoX_zoom_rb = new System.Windows.Forms.RadioButton();
            this.oneX_zoom_rb = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.HotSpotCentreY_nud = new System.Windows.Forms.NumericUpDown();
            this.HotSpotCentreX_nud = new System.Windows.Forms.NumericUpDown();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.applyHotSpotCorrection_btn = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.hotSpotValues_btn = new System.Windows.Forms.Button();
            this.centreValues_btn = new System.Windows.Forms.Button();
            this.label30 = new System.Windows.Forms.Label();
            this.HSRad2_nud = new System.Windows.Forms.NumericUpDown();
            this.label31 = new System.Windows.Forms.Label();
            this.HSRad1_nud = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.ShadowRad2_nud = new System.Windows.Forms.NumericUpDown();
            this.label25 = new System.Windows.Forms.Label();
            this.ShadowRad1_nud = new System.Windows.Forms.NumericUpDown();
            this.allChannelLut_tbp = new System.Windows.Forms.TabPage();
            this.channnelsTbpl = new System.Windows.Forms.TableLayoutPanel();
            this.redChannelTblPnl = new System.Windows.Forms.TableLayoutPanel();
            this.redKutSettingTbpl = new System.Windows.Forms.TableLayoutPanel();
            this.sineFactorRedChannelLbl = new System.Windows.Forms.Label();
            this.interval1RedChannelLbl = new System.Windows.Forms.Label();
            this.Interval2RedChannelLbl = new System.Windows.Forms.Label();
            this.lutOffsetRedChannelLbl = new System.Windows.Forms.Label();
            this.redChannelSineFactor_nud = new System.Windows.Forms.NumericUpDown();
            this.redChannelLutInterval1_nud = new System.Windows.Forms.NumericUpDown();
            this.redChannelLutInterval2_nud = new System.Windows.Forms.NumericUpDown();
            this.redChannelLutOffset_nud = new System.Windows.Forms.NumericUpDown();
            this.redChannelLutLbl = new System.Windows.Forms.Label();
            this.greenChannelTbpl = new System.Windows.Forms.TableLayoutPanel();
            this.greenLutSettingsTbpl = new System.Windows.Forms.TableLayoutPanel();
            this.greenChannelSineFactorLbl = new System.Windows.Forms.Label();
            this.interval1GreenChannelLbl = new System.Windows.Forms.Label();
            this.interval2GreenChannelLbl = new System.Windows.Forms.Label();
            this.lutOffsetGreenChannelLbl = new System.Windows.Forms.Label();
            this.greenChannelSineFactor_nud = new System.Windows.Forms.NumericUpDown();
            this.greenChannelLutInterval1_nud = new System.Windows.Forms.NumericUpDown();
            this.greenChannelLutInterval2_nud = new System.Windows.Forms.NumericUpDown();
            this.greenChannelLutOffset_nud = new System.Windows.Forms.NumericUpDown();
            this.greenChannelLutLbl = new System.Windows.Forms.Label();
            this.blueChannelTbpl = new System.Windows.Forms.TableLayoutPanel();
            this.blueLutSettingsTbpl = new System.Windows.Forms.TableLayoutPanel();
            this.blueChannelSineFactorLbl = new System.Windows.Forms.Label();
            this.interval1BlueChannelLbl = new System.Windows.Forms.Label();
            this.interval2BlueChannelLbl = new System.Windows.Forms.Label();
            this.lutOffsetBlueChannelLbl = new System.Windows.Forms.Label();
            this.blueChannelSineFactor_nud = new System.Windows.Forms.NumericUpDown();
            this.blueChannelLutInterval1_nud = new System.Windows.Forms.NumericUpDown();
            this.blueChannelLutInterval2_nud = new System.Windows.Forms.NumericUpDown();
            this.blueChannelLutOffset_nud = new System.Windows.Forms.NumericUpDown();
            this.blueChannelLutLbl = new System.Windows.Forms.Label();
            this.showLutTbpl = new System.Windows.Forms.TableLayoutPanel();
            this.showAllChannelBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel32 = new System.Windows.Forms.TableLayoutPanel();
            this.neg_pbx = new System.Windows.Forms.PictureBox();
            this.pos_pbx = new System.Windows.Forms.PictureBox();
            this.panel10 = new System.Windows.Forms.Panel();
            this.startFFATimer_btn = new INTUSOFT.Custom.Controls.FormButtons();
            this.clearOverlay_btn = new INTUSOFT.Custom.Controls.FormButtons();
            this.browse_btn = new INTUSOFT.Custom.Controls.FormButtons();
            this.saveFrames_btn = new INTUSOFT.Custom.Controls.FormButtons();
            this.BrowseOverlay_btn = new INTUSOFT.Custom.Controls.FormButtons();
            this.connect_btn = new INTUSOFT.Custom.Controls.FormButtons();
            this.MotorForward_cbx = new INTUSOFT.Custom.Controls.FormCheckBox();
            this.greenIRLive_cbx = new INTUSOFT.Custom.Controls.FormCheckBox();
            this.contCapture_cbx = new INTUSOFT.Custom.Controls.FormCheckBox();
            this.PatientName_tbx = new INTUSOFT.Custom.Controls.FormTextBox();
            this.enableLivePostProcessing_cbx = new INTUSOFT.Custom.Controls.FormCheckBox();
            this.postProcessing_cbx = new INTUSOFT.Custom.Controls.FormCheckBox();
            this.showOverlay_cbx = new INTUSOFT.Custom.Controls.FormCheckBox();
            this.flashBoost_cbx = new INTUSOFT.Custom.Controls.FormCheckBox();
            this.liveCC_cbx = new INTUSOFT.Custom.Controls.FormCheckBox();
            this.formCheckBox1 = new INTUSOFT.Custom.Controls.FormCheckBox();
            this.EnableTemperatureTint_cbx = new INTUSOFT.Custom.Controls.FormCheckBox();
            this.getRaw_btn = new INTUSOFT.Custom.Controls.FormButtons();
            this.ApplyHSVBoost_cbx = new INTUSOFT.Custom.Controls.FormCheckBox();
            this.applyClahe_cbx = new INTUSOFT.Custom.Controls.FormCheckBox();
            this.isApplyLut_cbx = new INTUSOFT.Custom.Controls.FormCheckBox();
            this.applyMask_cbx = new INTUSOFT.Custom.Controls.FormCheckBox();
            this.applyColorCorrection_cbx = new INTUSOFT.Custom.Controls.FormCheckBox();
            this.applyHotSpotCorrection_cbx = new INTUSOFT.Custom.Controls.FormCheckBox();
            this.applyShift_cbx = new INTUSOFT.Custom.Controls.FormCheckBox();
            this.saveSettings_btn = new INTUSOFT.Custom.Controls.FormButtons();
            this.applyGamma_cbx = new INTUSOFT.Custom.Controls.FormCheckBox();
            this.LiveMask_cbx = new INTUSOFT.Custom.Controls.FormCheckBox();
            this.colorChannel_btn = new INTUSOFT.Custom.Controls.FormButtons();
            this.redChannel_btn = new INTUSOFT.Custom.Controls.FormButtons();
            this.greenChannel_btn = new INTUSOFT.Custom.Controls.FormButtons();
            this.blueChannel_btn = new INTUSOFT.Custom.Controls.FormButtons();
            this.vFlip_cbx = new INTUSOFT.Custom.Controls.FormCheckBox();
            this.hFlip_cbx = new INTUSOFT.Custom.Controls.FormCheckBox();
            this.saveLiveFrame_btn = new INTUSOFT.Custom.Controls.FormButtons();
            this.applyOverlayGrid_btn = new INTUSOFT.Custom.Controls.FormButtons();
            this.unsharp_btn = new INTUSOFT.Custom.Controls.FormButtons();
            this.createLUT_btn = new INTUSOFT.Custom.Controls.FormButtons();
            this.SaveProcessedImage_cbx = new INTUSOFT.Custom.Controls.FormCheckBox();
            this.SaveDebugImage_cbx = new INTUSOFT.Custom.Controls.FormCheckBox();
            this.saveRaw_cbx = new INTUSOFT.Custom.Controls.FormCheckBox();
            this.SaveIr_cbx = new INTUSOFT.Custom.Controls.FormCheckBox();
            this.showExtViewer_cbx = new INTUSOFT.Custom.Controls.FormCheckBox();
            this.formButtons1 = new INTUSOFT.Custom.Controls.FormButtons();
            this.GetHoSpotParams_btn = new INTUSOFT.Custom.Controls.FormButtons();
            this.rightArrow_pbx = new INTUSOFT.Custom.Controls.PictureBoxExtended();
            this.leftArrow_pbx = new INTUSOFT.Custom.Controls.PictureBoxExtended();
            this.overlay_pbx = new INTUSOFT.Custom.Controls.PictureBoxExtended();
            this.display_pbx = new INTUSOFT.Custom.Controls.PictureBoxExtended();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.control_tb.SuspendLayout();
            this.basicControls_tbp.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.ExpGain_gbx.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.expVal_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gainVal_nud)).BeginInit();
            this.tableLayoutPanel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel9.SuspendLayout();
            this.captureExpGain_gbx.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flashGain_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.flashExp_nud)).BeginInit();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.tableLayoutPanel21.SuspendLayout();
            this.tableLayoutPanel22.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IrOffsetSteps_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.flashOffset2_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.flashoffset1_nud)).BeginInit();
            this.panel12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.powerStatus_pbx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cameraStatus_pbx)).BeginInit();
            this.panel7.SuspendLayout();
            this.tableLayoutPanel30.SuspendLayout();
            this.tableLayoutPanel31.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flashBoost_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.triggerRecieved_pbx)).BeginInit();
            this.tableLayoutPanel34.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tableLayoutPanel17.SuspendLayout();
            this.postProcessing_tbp.SuspendLayout();
            this.tempTint_gbx.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.temperature_tb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tint_tb)).BeginInit();
            this.shiftImage_tbp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Yshift_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xShift_nud)).BeginInit();
            this.ColorCorrection_tbp.SuspendLayout();
            this.tableLayoutPanel20.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.brightness_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contrast_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.offset1_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LUT_interval2_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LUT_interval1_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LUT_SineFactor_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hsvBoostVal_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.medianfilter_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unsharpAmount_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unsharpRadius_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unsharpThresh_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClaheClipValueR_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClaheClipValueG_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClaheClipValueB_nud)).BeginInit();
            this.tableLayoutPanel29.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gammaVal_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueGreen_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenGreen_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.redGreen_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueBlue_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueRed_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenBlue_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenRed_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.redBlue_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.redRed_nud)).BeginInit();
            this.Mask_tbp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CaptureMaskHeight_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CaptureMaskWidth_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LiveMaskHeight_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LiveMaskWidth_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maskY_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maskX_nud)).BeginInit();
            this.CentreImage_tbp.SuspendLayout();
            this.misc_tbpg.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tableLayoutPanel14.SuspendLayout();
            this.tableLayoutPanel13.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel15.SuspendLayout();
            this.motorSensor_gbx.SuspendLayout();
            this.tableLayoutPanel16.SuspendLayout();
            this.panel8.SuspendLayout();
            this.tableLayoutPanel23.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.redGain_tb)).BeginInit();
            this.tableLayoutPanel24.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.greenGain_tb)).BeginInit();
            this.tableLayoutPanel25.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.blueGain_tb)).BeginInit();
            this.tableLayoutPanel26.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.liveRedGain_tb)).BeginInit();
            this.tableLayoutPanel27.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.liveGreenGain_tb)).BeginInit();
            this.tableLayoutPanel28.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.liveBlueGain_tb)).BeginInit();
            this.ffa_tbpg.SuspendLayout();
            this.tableLayoutPanel18.SuspendLayout();
            this.tableLayoutPanel19.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.greenFilterPos_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueFilterPos_nud)).BeginInit();
            this.panel5.SuspendLayout();
            this.tableLayoutPanel33.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frameDetectionVal_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.darkFameDetectionVal_nud)).BeginInit();
            this.panel11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.saveFramesCount_nud)).BeginInit();
            this.panel14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FFA_Color_Pot_Int_Offset_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FFA_Pot_Int_Offset_nud)).BeginInit();
            this.HotSpot_tbp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shadowRedPeak_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.shadowGreenPeak_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.shadowBluePeak_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HsRedPeak_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HsGreenPeak_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HsBluePeak_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HsRedRadius_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HsGreenRadius_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HsBlueRadius_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HotSpotCentreY_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HotSpotCentreX_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HSRad2_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HSRad1_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShadowRad2_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShadowRad1_nud)).BeginInit();
            this.allChannelLut_tbp.SuspendLayout();
            this.channnelsTbpl.SuspendLayout();
            this.redChannelTblPnl.SuspendLayout();
            this.redKutSettingTbpl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.redChannelSineFactor_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.redChannelLutInterval1_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.redChannelLutInterval2_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.redChannelLutOffset_nud)).BeginInit();
            this.greenChannelTbpl.SuspendLayout();
            this.greenLutSettingsTbpl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.greenChannelSineFactor_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenChannelLutInterval1_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenChannelLutInterval2_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenChannelLutOffset_nud)).BeginInit();
            this.blueChannelTbpl.SuspendLayout();
            this.blueLutSettingsTbpl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.blueChannelSineFactor_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueChannelLutInterval1_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueChannelLutInterval2_nud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueChannelLutOffset_nud)).BeginInit();
            this.showLutTbpl.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel32.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.neg_pbx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pos_pbx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightArrow_pbx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftArrow_pbx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.overlay_pbx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.display_pbx)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.statusStrip1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 96.49596F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.504043F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1366, 738);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cameraStatus_lbl,
            this.Resolution_lbl,
            this.FrameRate_lbl,
            this.FrameStatus_lbl,
            this.toolStripStatusLabel1,
            this.toolStripDropDownButton1,
            this.temperaturStatus_lbl,
            this.tintStatus_lbl,
            this.cameraConnection_lbl,
            this.powerConnection_lbl,
            this.ComPortStatus_lbl});
            this.statusStrip1.Location = new System.Drawing.Point(0, 712);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1366, 26);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // cameraStatus_lbl
            // 
            this.cameraStatus_lbl.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cameraStatus_lbl.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.cameraStatus_lbl.Name = "cameraStatus_lbl";
            this.cameraStatus_lbl.Size = new System.Drawing.Size(0, 21);
            // 
            // Resolution_lbl
            // 
            this.Resolution_lbl.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.Resolution_lbl.ForeColor = System.Drawing.Color.Red;
            this.Resolution_lbl.Name = "Resolution_lbl";
            this.Resolution_lbl.Size = new System.Drawing.Size(0, 21);
            // 
            // FrameRate_lbl
            // 
            this.FrameRate_lbl.Name = "FrameRate_lbl";
            this.FrameRate_lbl.Size = new System.Drawing.Size(13, 21);
            this.FrameRate_lbl.Text = "0";
            // 
            // FrameStatus_lbl
            // 
            this.FrameStatus_lbl.Name = "FrameStatus_lbl";
            this.FrameStatus_lbl.Size = new System.Drawing.Size(25, 21);
            this.FrameStatus_lbl.Text = "Fail";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 21);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resolution_combx});
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(13, 24);
            this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            // 
            // resolution_combx
            // 
            this.resolution_combx.Name = "resolution_combx";
            this.resolution_combx.Size = new System.Drawing.Size(121, 23);
            // 
            // temperaturStatus_lbl
            // 
            this.temperaturStatus_lbl.Name = "temperaturStatus_lbl";
            this.temperaturStatus_lbl.Size = new System.Drawing.Size(43, 21);
            this.temperaturStatus_lbl.Text = "Temp :";
            // 
            // tintStatus_lbl
            // 
            this.tintStatus_lbl.Name = "tintStatus_lbl";
            this.tintStatus_lbl.Size = new System.Drawing.Size(34, 21);
            this.tintStatus_lbl.Text = "Tint :";
            // 
            // cameraConnection_lbl
            // 
            this.cameraConnection_lbl.Name = "cameraConnection_lbl";
            this.cameraConnection_lbl.Size = new System.Drawing.Size(0, 21);
            // 
            // powerConnection_lbl
            // 
            this.powerConnection_lbl.Name = "powerConnection_lbl";
            this.powerConnection_lbl.Size = new System.Drawing.Size(0, 21);
            // 
            // ComPortStatus_lbl
            // 
            this.ComPortStatus_lbl.Name = "ComPortStatus_lbl";
            this.ComPortStatus_lbl.Size = new System.Drawing.Size(0, 21);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76.00303F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.99697F));
            this.tableLayoutPanel2.Controls.Add(this.control_tb, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1360, 706);
            this.tableLayoutPanel2.TabIndex = 7;
            // 
            // control_tb
            // 
            this.control_tb.Controls.Add(this.basicControls_tbp);
            this.control_tb.Controls.Add(this.postProcessing_tbp);
            this.control_tb.Controls.Add(this.shiftImage_tbp);
            this.control_tb.Controls.Add(this.ColorCorrection_tbp);
            this.control_tb.Controls.Add(this.Mask_tbp);
            this.control_tb.Controls.Add(this.CentreImage_tbp);
            this.control_tb.Controls.Add(this.misc_tbpg);
            this.control_tb.Controls.Add(this.ffa_tbpg);
            this.control_tb.Controls.Add(this.HotSpot_tbp);
            this.control_tb.Controls.Add(this.allChannelLut_tbp);
            this.control_tb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.control_tb.Location = new System.Drawing.Point(1036, 3);
            this.control_tb.Multiline = true;
            this.control_tb.Name = "control_tb";
            this.control_tb.SelectedIndex = 0;
            this.control_tb.Size = new System.Drawing.Size(321, 700);
            this.control_tb.TabIndex = 9;
            this.control_tb.SelectedIndexChanged += new System.EventHandler(this.control_tb_SelectedIndexChanged);
            // 
            // basicControls_tbp
            // 
            this.basicControls_tbp.Controls.Add(this.tableLayoutPanel3);
            this.basicControls_tbp.Location = new System.Drawing.Point(4, 40);
            this.basicControls_tbp.Name = "basicControls_tbp";
            this.basicControls_tbp.Size = new System.Drawing.Size(313, 656);
            this.basicControls_tbp.TabIndex = 2;
            this.basicControls_tbp.Text = "Basic";
            this.basicControls_tbp.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.ExpGain_gbx, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel5, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.panel9, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel7, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.groupBox1, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.panel2, 0, 8);
            this.tableLayoutPanel3.Controls.Add(this.panel12, 0, 10);
            this.tableLayoutPanel3.Controls.Add(this.panel7, 0, 9);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel30, 0, 7);
            this.tableLayoutPanel3.Controls.Add(this.groupBox4, 0, 6);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 11;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.28743F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.695233F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.64154F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.52115F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1.193607F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.69036F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.19797F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.813875F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.967851F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(313, 656);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // ExpGain_gbx
            // 
            this.ExpGain_gbx.Controls.Add(this.tableLayoutPanel4);
            this.ExpGain_gbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExpGain_gbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExpGain_gbx.ForeColor = System.Drawing.Color.Black;
            this.ExpGain_gbx.Location = new System.Drawing.Point(3, 168);
            this.ExpGain_gbx.Name = "ExpGain_gbx";
            this.ExpGain_gbx.Size = new System.Drawing.Size(307, 75);
            this.ExpGain_gbx.TabIndex = 41;
            this.ExpGain_gbx.TabStop = false;
            this.ExpGain_gbx.Text = "Live Exposure and Gain";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel4.Controls.Add(this.expVal_nud, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.gainVal_nud, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.gain_lbl, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.exp_lbl, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 19);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(301, 53);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // expVal_nud
            // 
            this.expVal_nud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.expVal_nud.Increment = new decimal(new int[] {
            2500,
            0,
            0,
            0});
            this.expVal_nud.Location = new System.Drawing.Point(93, 3);
            this.expVal_nud.Name = "expVal_nud";
            this.expVal_nud.Size = new System.Drawing.Size(205, 23);
            this.expVal_nud.TabIndex = 11;
            this.expVal_nud.ValueChanged += new System.EventHandler(this.expVal_nud_ValueChanged);
            // 
            // gainVal_nud
            // 
            this.gainVal_nud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gainVal_nud.Location = new System.Drawing.Point(93, 29);
            this.gainVal_nud.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.gainVal_nud.Minimum = new decimal(new int[] {
            5000,
            0,
            0,
            -2147483648});
            this.gainVal_nud.Name = "gainVal_nud";
            this.gainVal_nud.Size = new System.Drawing.Size(205, 23);
            this.gainVal_nud.TabIndex = 12;
            this.gainVal_nud.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.gainVal_nud.ValueChanged += new System.EventHandler(this.gainVal_nud_ValueChanged);
            // 
            // gain_lbl
            // 
            this.gain_lbl.AutoSize = true;
            this.gain_lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gain_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gain_lbl.Location = new System.Drawing.Point(3, 26);
            this.gain_lbl.Name = "gain_lbl";
            this.gain_lbl.Size = new System.Drawing.Size(84, 27);
            this.gain_lbl.TabIndex = 8;
            this.gain_lbl.Text = "Gain";
            // 
            // exp_lbl
            // 
            this.exp_lbl.AutoSize = true;
            this.exp_lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exp_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exp_lbl.Location = new System.Drawing.Point(3, 0);
            this.exp_lbl.Name = "exp_lbl";
            this.exp_lbl.Size = new System.Drawing.Size(84, 26);
            this.exp_lbl.TabIndex = 5;
            this.exp_lbl.Text = "Exposure";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 5;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel5.Controls.Add(this.startFFATimer_btn, 4, 0);
            this.tableLayoutPanel5.Controls.Add(this.clearOverlay_btn, 3, 0);
            this.tableLayoutPanel5.Controls.Add(this.browse_btn, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.saveFrames_btn, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.panel6, 2, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 123);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(307, 39);
            this.tableLayoutPanel5.TabIndex = 46;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.BrowseOverlay_btn);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(119, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(52, 33);
            this.panel6.TabIndex = 1;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.captureExpGain_gbx);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(3, 249);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(307, 68);
            this.panel9.TabIndex = 52;
            // 
            // captureExpGain_gbx
            // 
            this.captureExpGain_gbx.Controls.Add(this.tableLayoutPanel6);
            this.captureExpGain_gbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.captureExpGain_gbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.captureExpGain_gbx.Location = new System.Drawing.Point(0, 0);
            this.captureExpGain_gbx.Name = "captureExpGain_gbx";
            this.captureExpGain_gbx.Size = new System.Drawing.Size(307, 68);
            this.captureExpGain_gbx.TabIndex = 0;
            this.captureExpGain_gbx.TabStop = false;
            this.captureExpGain_gbx.Text = "Capture Exposure and Gain";
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel6.Controls.Add(this.flashGain_nud, 1, 1);
            this.tableLayoutPanel6.Controls.Add(this.label33, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.label32, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.flashExp_nud, 1, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 19);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 2;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(301, 46);
            this.tableLayoutPanel6.TabIndex = 0;
            // 
            // flashGain_nud
            // 
            this.flashGain_nud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flashGain_nud.Location = new System.Drawing.Point(93, 26);
            this.flashGain_nud.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.flashGain_nud.Minimum = new decimal(new int[] {
            5000,
            0,
            0,
            -2147483648});
            this.flashGain_nud.Name = "flashGain_nud";
            this.flashGain_nud.Size = new System.Drawing.Size(205, 23);
            this.flashGain_nud.TabIndex = 18;
            this.flashGain_nud.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.flashGain_nud.ValueChanged += new System.EventHandler(this.flashGain_nud_ValueChanged);
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.Location = new System.Drawing.Point(3, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(84, 23);
            this.label33.TabIndex = 14;
            this.label33.Text = "Exposure";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.Location = new System.Drawing.Point(3, 23);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(84, 23);
            this.label32.TabIndex = 16;
            this.label32.Text = "Gain";
            // 
            // flashExp_nud
            // 
            this.flashExp_nud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flashExp_nud.Increment = new decimal(new int[] {
            2500,
            0,
            0,
            0});
            this.flashExp_nud.Location = new System.Drawing.Point(93, 3);
            this.flashExp_nud.Name = "flashExp_nud";
            this.flashExp_nud.Size = new System.Drawing.Size(205, 23);
            this.flashExp_nud.TabIndex = 17;
            this.flashExp_nud.ValueChanged += new System.EventHandler(this.flashExp_nud_ValueChanged);
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.72401F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.27599F));
            this.tableLayoutPanel7.Controls.Add(this.connect_btn, 1, 0);
            this.tableLayoutPanel7.Controls.Add(this.tableLayoutPanel8, 0, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(307, 114);
            this.tableLayoutPanel7.TabIndex = 53;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 1;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Controls.Add(this.label34, 0, 2);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel12, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel21, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel22, 0, 3);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 4;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.76923F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 28.84615F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.53846F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27.88461F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(186, 108);
            this.tableLayoutPanel8.TabIndex = 50;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label34.Location = new System.Drawing.Point(3, 64);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(180, 12);
            this.label34.TabIndex = 2;
            this.label34.Text = "Camera Modes";
            // 
            // tableLayoutPanel12
            // 
            this.tableLayoutPanel12.ColumnCount = 2;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.66667F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 57.33333F));
            this.tableLayoutPanel12.Controls.Add(this.trigger_lbl, 0, 0);
            this.tableLayoutPanel12.Controls.Add(this.cameraModel_cmbx, 1, 0);
            this.tableLayoutPanel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel12.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.RowCount = 1;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel12.Size = new System.Drawing.Size(180, 27);
            this.tableLayoutPanel12.TabIndex = 3;
            // 
            // trigger_lbl
            // 
            this.trigger_lbl.AutoSize = true;
            this.trigger_lbl.Dock = System.Windows.Forms.DockStyle.Top;
            this.trigger_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trigger_lbl.Location = new System.Drawing.Point(3, 0);
            this.trigger_lbl.Name = "trigger_lbl";
            this.trigger_lbl.Size = new System.Drawing.Size(70, 13);
            this.trigger_lbl.TabIndex = 0;
            this.trigger_lbl.Text = "Model";
            // 
            // cameraModel_cmbx
            // 
            this.cameraModel_cmbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraModel_cmbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cameraModel_cmbx.FormattingEnabled = true;
            this.cameraModel_cmbx.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D"});
            this.cameraModel_cmbx.Location = new System.Drawing.Point(79, 3);
            this.cameraModel_cmbx.Name = "cameraModel_cmbx";
            this.cameraModel_cmbx.Size = new System.Drawing.Size(98, 21);
            this.cameraModel_cmbx.TabIndex = 21;
            this.cameraModel_cmbx.SelectedIndexChanged += new System.EventHandler(this.cameraModel_cmbx_SelectedIndexChanged);
            // 
            // tableLayoutPanel21
            // 
            this.tableLayoutPanel21.ColumnCount = 2;
            this.tableLayoutPanel21.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.66667F));
            this.tableLayoutPanel21.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 57.33333F));
            this.tableLayoutPanel21.Controls.Add(this.mode_cmbx, 1, 0);
            this.tableLayoutPanel21.Controls.Add(this.label51, 0, 0);
            this.tableLayoutPanel21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel21.Location = new System.Drawing.Point(3, 36);
            this.tableLayoutPanel21.Name = "tableLayoutPanel21";
            this.tableLayoutPanel21.RowCount = 1;
            this.tableLayoutPanel21.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel21.Size = new System.Drawing.Size(180, 25);
            this.tableLayoutPanel21.TabIndex = 4;
            // 
            // mode_cmbx
            // 
            this.mode_cmbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mode_cmbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mode_cmbx.FormattingEnabled = true;
            this.mode_cmbx.Items.AddRange(new object[] {
            "Posterior_Prime",
            "Anterior_Prime",
            "Posterior_45",
            "45Color",
            "FFA_Plus"});
            this.mode_cmbx.Location = new System.Drawing.Point(79, 3);
            this.mode_cmbx.Name = "mode_cmbx";
            this.mode_cmbx.Size = new System.Drawing.Size(98, 21);
            this.mode_cmbx.TabIndex = 57;
            this.mode_cmbx.SelectedIndexChanged += new System.EventHandler(this.mode_cmbx_SelectedIndexChanged);
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(3, 0);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(51, 13);
            this.label51.TabIndex = 58;
            this.label51.Text = "Op Mode";
            // 
            // tableLayoutPanel22
            // 
            this.tableLayoutPanel22.ColumnCount = 2;
            this.tableLayoutPanel22.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel22.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel22.Controls.Add(this.CCMode_rb, 1, 0);
            this.tableLayoutPanel22.Controls.Add(this.rawModeFrameGrab_rb, 0, 0);
            this.tableLayoutPanel22.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel22.Location = new System.Drawing.Point(3, 79);
            this.tableLayoutPanel22.Name = "tableLayoutPanel22";
            this.tableLayoutPanel22.RowCount = 1;
            this.tableLayoutPanel22.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel22.Size = new System.Drawing.Size(180, 26);
            this.tableLayoutPanel22.TabIndex = 5;
            // 
            // CCMode_rb
            // 
            this.CCMode_rb.AutoSize = true;
            this.CCMode_rb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CCMode_rb.Location = new System.Drawing.Point(93, 3);
            this.CCMode_rb.Name = "CCMode_rb";
            this.CCMode_rb.Size = new System.Drawing.Size(84, 20);
            this.CCMode_rb.TabIndex = 1;
            this.CCMode_rb.Text = "CC";
            this.CCMode_rb.UseVisualStyleBackColor = true;
            this.CCMode_rb.CheckedChanged += new System.EventHandler(this.colorMode_rb_CheckedChanged);
            // 
            // rawModeFrameGrab_rb
            // 
            this.rawModeFrameGrab_rb.AutoSize = true;
            this.rawModeFrameGrab_rb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rawModeFrameGrab_rb.Location = new System.Drawing.Point(3, 3);
            this.rawModeFrameGrab_rb.Name = "rawModeFrameGrab_rb";
            this.rawModeFrameGrab_rb.Size = new System.Drawing.Size(84, 20);
            this.rawModeFrameGrab_rb.TabIndex = 0;
            this.rawModeFrameGrab_rb.Text = "Raw";
            this.rawModeFrameGrab_rb.UseVisualStyleBackColor = true;
            this.rawModeFrameGrab_rb.CheckedChanged += new System.EventHandler(this.rawModeFrameGrab_rb_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel10);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(3, 330);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(307, 69);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Live Light";
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.ColumnCount = 6;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0F));
            this.tableLayoutPanel10.Controls.Add(this.ir_rb, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.flash_rb, 1, 0);
            this.tableLayoutPanel10.Controls.Add(this.blue_rb, 2, 0);
            this.tableLayoutPanel10.Controls.Add(this.liveAnt_IR_rb, 3, 0);
            this.tableLayoutPanel10.Controls.Add(this.liveAnt_Flash_rb, 4, 0);
            this.tableLayoutPanel10.Controls.Add(this.liveAnt_Blue_rb, 5, 0);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(3, 19);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 1;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(301, 47);
            this.tableLayoutPanel10.TabIndex = 0;
            // 
            // ir_rb
            // 
            this.ir_rb.AutoSize = true;
            this.ir_rb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ir_rb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ir_rb.Location = new System.Drawing.Point(3, 3);
            this.ir_rb.Name = "ir_rb";
            this.ir_rb.Size = new System.Drawing.Size(94, 41);
            this.ir_rb.TabIndex = 51;
            this.ir_rb.TabStop = true;
            this.ir_rb.Text = "IR";
            this.ir_rb.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.ir_rb.UseVisualStyleBackColor = true;
            this.ir_rb.CheckedChanged += new System.EventHandler(this.ir_rb_CheckedChanged);
            // 
            // flash_rb
            // 
            this.flash_rb.AutoSize = true;
            this.flash_rb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flash_rb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flash_rb.Location = new System.Drawing.Point(103, 3);
            this.flash_rb.Name = "flash_rb";
            this.flash_rb.Size = new System.Drawing.Size(94, 41);
            this.flash_rb.TabIndex = 51;
            this.flash_rb.TabStop = true;
            this.flash_rb.Text = "FL";
            this.flash_rb.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.flash_rb.UseVisualStyleBackColor = true;
            this.flash_rb.CheckedChanged += new System.EventHandler(this.flash_rb_CheckedChanged);
            // 
            // blue_rb
            // 
            this.blue_rb.AutoSize = true;
            this.blue_rb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blue_rb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blue_rb.Location = new System.Drawing.Point(203, 3);
            this.blue_rb.Name = "blue_rb";
            this.blue_rb.Size = new System.Drawing.Size(94, 41);
            this.blue_rb.TabIndex = 56;
            this.blue_rb.TabStop = true;
            this.blue_rb.Text = "Blue";
            this.blue_rb.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.blue_rb.UseVisualStyleBackColor = true;
            this.blue_rb.CheckedChanged += new System.EventHandler(this.blue_rb_CheckedChanged);
            // 
            // liveAnt_IR_rb
            // 
            this.liveAnt_IR_rb.AutoSize = true;
            this.liveAnt_IR_rb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.liveAnt_IR_rb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.liveAnt_IR_rb.Location = new System.Drawing.Point(303, 3);
            this.liveAnt_IR_rb.Name = "liveAnt_IR_rb";
            this.liveAnt_IR_rb.Size = new System.Drawing.Size(1, 41);
            this.liveAnt_IR_rb.TabIndex = 51;
            this.liveAnt_IR_rb.TabStop = true;
            this.liveAnt_IR_rb.Text = "Ant_IR";
            this.liveAnt_IR_rb.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.liveAnt_IR_rb.UseVisualStyleBackColor = true;
            // 
            // liveAnt_Flash_rb
            // 
            this.liveAnt_Flash_rb.AutoSize = true;
            this.liveAnt_Flash_rb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.liveAnt_Flash_rb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.liveAnt_Flash_rb.Location = new System.Drawing.Point(303, 3);
            this.liveAnt_Flash_rb.Name = "liveAnt_Flash_rb";
            this.liveAnt_Flash_rb.Size = new System.Drawing.Size(1, 41);
            this.liveAnt_Flash_rb.TabIndex = 51;
            this.liveAnt_Flash_rb.TabStop = true;
            this.liveAnt_Flash_rb.Text = "Ant_FL";
            this.liveAnt_Flash_rb.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.liveAnt_Flash_rb.UseVisualStyleBackColor = true;
            // 
            // liveAnt_Blue_rb
            // 
            this.liveAnt_Blue_rb.AutoSize = true;
            this.liveAnt_Blue_rb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.liveAnt_Blue_rb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.liveAnt_Blue_rb.Location = new System.Drawing.Point(303, 3);
            this.liveAnt_Blue_rb.Name = "liveAnt_Blue_rb";
            this.liveAnt_Blue_rb.Size = new System.Drawing.Size(1, 41);
            this.liveAnt_Blue_rb.TabIndex = 51;
            this.liveAnt_Blue_rb.TabStop = true;
            this.liveAnt_Blue_rb.Text = "Ant_Blue";
            this.liveAnt_Blue_rb.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.liveAnt_Blue_rb.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tableLayoutPanel11);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 541);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(307, 47);
            this.panel2.TabIndex = 57;
            // 
            // tableLayoutPanel11
            // 
            this.tableLayoutPanel11.ColumnCount = 4;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.47099F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.30375F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.05376F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.01434F));
            this.tableLayoutPanel11.Controls.Add(this.IrOffsetSteps_nud, 1, 1);
            this.tableLayoutPanel11.Controls.Add(this.label6, 2, 1);
            this.tableLayoutPanel11.Controls.Add(this.captureFocusSteps_lbl, 0, 1);
            this.tableLayoutPanel11.Controls.Add(this.MotorForward_cbx, 0, 0);
            this.tableLayoutPanel11.Controls.Add(this.greenIRLive_cbx, 1, 0);
            this.tableLayoutPanel11.Controls.Add(this.flashOffset2_nud, 3, 1);
            this.tableLayoutPanel11.Controls.Add(this.flashoffset1_nud, 3, 0);
            this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel11.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 2;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel11.Size = new System.Drawing.Size(307, 47);
            this.tableLayoutPanel11.TabIndex = 58;
            // 
            // IrOffsetSteps_nud
            // 
            this.IrOffsetSteps_nud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.IrOffsetSteps_nud.Location = new System.Drawing.Point(107, 26);
            this.IrOffsetSteps_nud.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.IrOffsetSteps_nud.Name = "IrOffsetSteps_nud";
            this.IrOffsetSteps_nud.Size = new System.Drawing.Size(77, 20);
            this.IrOffsetSteps_nud.TabIndex = 47;
            this.IrOffsetSteps_nud.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.IrOffsetSteps_nud.ValueChanged += new System.EventHandler(this.IrOffsetSteps_nud_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(190, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 24);
            this.label6.TabIndex = 46;
            this.label6.Text = "Flash Offset";
            this.label6.Visible = false;
            // 
            // captureFocusSteps_lbl
            // 
            this.captureFocusSteps_lbl.AutoSize = true;
            this.captureFocusSteps_lbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.captureFocusSteps_lbl.Location = new System.Drawing.Point(3, 23);
            this.captureFocusSteps_lbl.Name = "captureFocusSteps_lbl";
            this.captureFocusSteps_lbl.Size = new System.Drawing.Size(98, 24);
            this.captureFocusSteps_lbl.TabIndex = 46;
            this.captureFocusSteps_lbl.Text = " Motor Offset";
            // 
            // flashOffset2_nud
            // 
            this.flashOffset2_nud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flashOffset2_nud.Location = new System.Drawing.Point(235, 26);
            this.flashOffset2_nud.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.flashOffset2_nud.Name = "flashOffset2_nud";
            this.flashOffset2_nud.Size = new System.Drawing.Size(69, 20);
            this.flashOffset2_nud.TabIndex = 47;
            this.flashOffset2_nud.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.flashOffset2_nud.ValueChanged += new System.EventHandler(this.flashOffset_nud_ValueChanged);
            this.flashOffset2_nud.Click += new System.EventHandler(this.flashOffset_nud_ValueChanged);
            // 
            // flashoffset1_nud
            // 
            this.flashoffset1_nud.Location = new System.Drawing.Point(235, 3);
            this.flashoffset1_nud.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.flashoffset1_nud.Name = "flashoffset1_nud";
            this.flashoffset1_nud.Size = new System.Drawing.Size(38, 20);
            this.flashoffset1_nud.TabIndex = 49;
            this.flashoffset1_nud.ValueChanged += new System.EventHandler(this.flashoffset1_nud_ValueChanged);
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.LeftRight_lbl);
            this.panel12.Controls.Add(this.contCapture_cbx);
            this.panel12.Controls.Add(this.ffaTimerStatus_lbl);
            this.panel12.Controls.Add(this.powerStatus_pbx);
            this.panel12.Controls.Add(this.cameraStatus_pbx);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel12.Location = new System.Drawing.Point(3, 627);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(307, 26);
            this.panel12.TabIndex = 95;
            // 
            // LeftRight_lbl
            // 
            this.LeftRight_lbl.AutoSize = true;
            this.LeftRight_lbl.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftRight_lbl.Location = new System.Drawing.Point(60, 0);
            this.LeftRight_lbl.Name = "LeftRight_lbl";
            this.LeftRight_lbl.Size = new System.Drawing.Size(41, 13);
            this.LeftRight_lbl.TabIndex = 3;
            this.LeftRight_lbl.Text = "label26";
            // 
            // ffaTimerStatus_lbl
            // 
            this.ffaTimerStatus_lbl.AutoSize = true;
            this.ffaTimerStatus_lbl.Dock = System.Windows.Forms.DockStyle.Left;
            this.ffaTimerStatus_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ffaTimerStatus_lbl.Location = new System.Drawing.Point(60, 0);
            this.ffaTimerStatus_lbl.Name = "ffaTimerStatus_lbl";
            this.ffaTimerStatus_lbl.Size = new System.Drawing.Size(0, 17);
            this.ffaTimerStatus_lbl.TabIndex = 2;
            this.ffaTimerStatus_lbl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // powerStatus_pbx
            // 
            this.powerStatus_pbx.Dock = System.Windows.Forms.DockStyle.Left;
            this.powerStatus_pbx.Location = new System.Drawing.Point(31, 0);
            this.powerStatus_pbx.Name = "powerStatus_pbx";
            this.powerStatus_pbx.Size = new System.Drawing.Size(29, 26);
            this.powerStatus_pbx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.powerStatus_pbx.TabIndex = 1;
            this.powerStatus_pbx.TabStop = false;
            // 
            // cameraStatus_pbx
            // 
            this.cameraStatus_pbx.Dock = System.Windows.Forms.DockStyle.Left;
            this.cameraStatus_pbx.Location = new System.Drawing.Point(0, 0);
            this.cameraStatus_pbx.Name = "cameraStatus_pbx";
            this.cameraStatus_pbx.Size = new System.Drawing.Size(31, 26);
            this.cameraStatus_pbx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.cameraStatus_pbx.TabIndex = 0;
            this.cameraStatus_pbx.TabStop = false;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.PatientName_tbx);
            this.panel7.Controls.Add(this.label24);
            this.panel7.Controls.Add(this.enableLivePostProcessing_cbx);
            this.panel7.Controls.Add(this.postProcessing_cbx);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(3, 594);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(307, 27);
            this.panel7.TabIndex = 94;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Dock = System.Windows.Forms.DockStyle.Left;
            this.label24.Location = new System.Drawing.Point(0, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(71, 13);
            this.label24.TabIndex = 93;
            this.label24.Text = "Patient Name";
            // 
            // tableLayoutPanel30
            // 
            this.tableLayoutPanel30.ColumnCount = 5;
            this.tableLayoutPanel30.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.75F));
            this.tableLayoutPanel30.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 56.25F));
            this.tableLayoutPanel30.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel30.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel30.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutPanel30.Controls.Add(this.showOverlay_cbx, 4, 0);
            this.tableLayoutPanel30.Controls.Add(this.flashBoost_cbx, 0, 0);
            this.tableLayoutPanel30.Controls.Add(this.tableLayoutPanel31, 1, 0);
            this.tableLayoutPanel30.Controls.Add(this.triggerRecieved_pbx, 2, 0);
            this.tableLayoutPanel30.Controls.Add(this.tableLayoutPanel34, 3, 0);
            this.tableLayoutPanel30.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel30.Location = new System.Drawing.Point(3, 483);
            this.tableLayoutPanel30.Name = "tableLayoutPanel30";
            this.tableLayoutPanel30.RowCount = 1;
            this.tableLayoutPanel30.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel30.Size = new System.Drawing.Size(307, 52);
            this.tableLayoutPanel30.TabIndex = 96;
            // 
            // tableLayoutPanel31
            // 
            this.tableLayoutPanel31.ColumnCount = 1;
            this.tableLayoutPanel31.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel31.Controls.Add(this.label44, 0, 0);
            this.tableLayoutPanel31.Controls.Add(this.flashBoost_nud, 0, 1);
            this.tableLayoutPanel31.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel31.Location = new System.Drawing.Point(62, 3);
            this.tableLayoutPanel31.Name = "tableLayoutPanel31";
            this.tableLayoutPanel31.RowCount = 2;
            this.tableLayoutPanel31.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 39.39394F));
            this.tableLayoutPanel31.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60.60606F));
            this.tableLayoutPanel31.Size = new System.Drawing.Size(69, 46);
            this.tableLayoutPanel31.TabIndex = 54;
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label44.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label44.Location = new System.Drawing.Point(3, 0);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(63, 18);
            this.label44.TabIndex = 54;
            this.label44.Text = "Boost Val";
            // 
            // flashBoost_nud
            // 
            this.flashBoost_nud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flashBoost_nud.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flashBoost_nud.Location = new System.Drawing.Point(3, 21);
            this.flashBoost_nud.Name = "flashBoost_nud";
            this.flashBoost_nud.Size = new System.Drawing.Size(63, 20);
            this.flashBoost_nud.TabIndex = 53;
            this.flashBoost_nud.Value = new decimal(new int[] {
            48,
            0,
            0,
            0});
            this.flashBoost_nud.ValueChanged += new System.EventHandler(this.flashBoost_nud_ValueChanged);
            // 
            // triggerRecieved_pbx
            // 
            this.triggerRecieved_pbx.BackColor = System.Drawing.Color.Black;
            this.triggerRecieved_pbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.triggerRecieved_pbx.Location = new System.Drawing.Point(137, 3);
            this.triggerRecieved_pbx.Name = "triggerRecieved_pbx";
            this.triggerRecieved_pbx.Size = new System.Drawing.Size(30, 46);
            this.triggerRecieved_pbx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.triggerRecieved_pbx.TabIndex = 0;
            this.triggerRecieved_pbx.TabStop = false;
            // 
            // tableLayoutPanel34
            // 
            this.tableLayoutPanel34.ColumnCount = 1;
            this.tableLayoutPanel34.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel34.Controls.Add(this.singleFrameCapture_combox, 0, 1);
            this.tableLayoutPanel34.Controls.Add(this.label43, 0, 0);
            this.tableLayoutPanel34.Location = new System.Drawing.Point(173, 3);
            this.tableLayoutPanel34.Name = "tableLayoutPanel34";
            this.tableLayoutPanel34.RowCount = 2;
            this.tableLayoutPanel34.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel34.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.tableLayoutPanel34.Size = new System.Drawing.Size(58, 46);
            this.tableLayoutPanel34.TabIndex = 55;
            // 
            // singleFrameCapture_combox
            // 
            this.singleFrameCapture_combox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.singleFrameCapture_combox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.singleFrameCapture_combox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.singleFrameCapture_combox.FormattingEnabled = true;
            this.singleFrameCapture_combox.Items.AddRange(new object[] {
            "False",
            "True"});
            this.singleFrameCapture_combox.Location = new System.Drawing.Point(3, 18);
            this.singleFrameCapture_combox.Name = "singleFrameCapture_combox";
            this.singleFrameCapture_combox.Size = new System.Drawing.Size(52, 25);
            this.singleFrameCapture_combox.TabIndex = 50;
            this.singleFrameCapture_combox.SelectedIndexChanged += new System.EventHandler(this.singleFrameCapture_combox_SelectedIndexChanged);
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label43.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label43.Location = new System.Drawing.Point(3, 0);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(52, 15);
            this.label43.TabIndex = 49;
            this.label43.Text = "Single Frame";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tableLayoutPanel17);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(3, 405);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(307, 72);
            this.groupBox4.TabIndex = 97;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Capture Light";
            this.groupBox4.Visible = false;
            // 
            // tableLayoutPanel17
            // 
            this.tableLayoutPanel17.ColumnCount = 6;
            this.tableLayoutPanel17.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.06507F));
            this.tableLayoutPanel17.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.81469F));
            this.tableLayoutPanel17.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.12024F));
            this.tableLayoutPanel17.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0F));
            this.tableLayoutPanel17.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0F));
            this.tableLayoutPanel17.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0F));
            this.tableLayoutPanel17.Controls.Add(this.captureIR_rb, 0, 0);
            this.tableLayoutPanel17.Controls.Add(this.captureFlash_rb, 1, 0);
            this.tableLayoutPanel17.Controls.Add(this.captureBlue_rb, 2, 0);
            this.tableLayoutPanel17.Controls.Add(this.captureAnt_IR_rb, 3, 0);
            this.tableLayoutPanel17.Controls.Add(this.captureAnt_Flash_rb, 4, 0);
            this.tableLayoutPanel17.Controls.Add(this.captureAnt_Blue_rb, 5, 0);
            this.tableLayoutPanel17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel17.Location = new System.Drawing.Point(3, 19);
            this.tableLayoutPanel17.Name = "tableLayoutPanel17";
            this.tableLayoutPanel17.RowCount = 1;
            this.tableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel17.Size = new System.Drawing.Size(301, 50);
            this.tableLayoutPanel17.TabIndex = 0;
            this.tableLayoutPanel17.Visible = false;
            // 
            // captureIR_rb
            // 
            this.captureIR_rb.AutoSize = true;
            this.captureIR_rb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.captureIR_rb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.captureIR_rb.Location = new System.Drawing.Point(3, 3);
            this.captureIR_rb.Name = "captureIR_rb";
            this.captureIR_rb.Size = new System.Drawing.Size(84, 44);
            this.captureIR_rb.TabIndex = 51;
            this.captureIR_rb.TabStop = true;
            this.captureIR_rb.Text = "IR";
            this.captureIR_rb.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.captureIR_rb.UseVisualStyleBackColor = true;
            this.captureIR_rb.Visible = false;
            // 
            // captureFlash_rb
            // 
            this.captureFlash_rb.AutoSize = true;
            this.captureFlash_rb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.captureFlash_rb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.captureFlash_rb.Location = new System.Drawing.Point(93, 3);
            this.captureFlash_rb.Name = "captureFlash_rb";
            this.captureFlash_rb.Size = new System.Drawing.Size(95, 44);
            this.captureFlash_rb.TabIndex = 51;
            this.captureFlash_rb.TabStop = true;
            this.captureFlash_rb.Text = "FL";
            this.captureFlash_rb.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.captureFlash_rb.UseVisualStyleBackColor = true;
            // 
            // captureBlue_rb
            // 
            this.captureBlue_rb.AutoSize = true;
            this.captureBlue_rb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.captureBlue_rb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.captureBlue_rb.Location = new System.Drawing.Point(194, 3);
            this.captureBlue_rb.Name = "captureBlue_rb";
            this.captureBlue_rb.Size = new System.Drawing.Size(102, 44);
            this.captureBlue_rb.TabIndex = 56;
            this.captureBlue_rb.TabStop = true;
            this.captureBlue_rb.Text = "Blue";
            this.captureBlue_rb.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.captureBlue_rb.UseVisualStyleBackColor = true;
            // 
            // captureAnt_IR_rb
            // 
            this.captureAnt_IR_rb.AutoSize = true;
            this.captureAnt_IR_rb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.captureAnt_IR_rb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.captureAnt_IR_rb.Location = new System.Drawing.Point(302, 3);
            this.captureAnt_IR_rb.Name = "captureAnt_IR_rb";
            this.captureAnt_IR_rb.Size = new System.Drawing.Size(1, 44);
            this.captureAnt_IR_rb.TabIndex = 51;
            this.captureAnt_IR_rb.TabStop = true;
            this.captureAnt_IR_rb.Text = "Ant_IR";
            this.captureAnt_IR_rb.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.captureAnt_IR_rb.UseVisualStyleBackColor = true;
            // 
            // captureAnt_Flash_rb
            // 
            this.captureAnt_Flash_rb.AutoSize = true;
            this.captureAnt_Flash_rb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.captureAnt_Flash_rb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.captureAnt_Flash_rb.Location = new System.Drawing.Point(302, 3);
            this.captureAnt_Flash_rb.Name = "captureAnt_Flash_rb";
            this.captureAnt_Flash_rb.Size = new System.Drawing.Size(1, 44);
            this.captureAnt_Flash_rb.TabIndex = 51;
            this.captureAnt_Flash_rb.TabStop = true;
            this.captureAnt_Flash_rb.Text = "Ant_FL";
            this.captureAnt_Flash_rb.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.captureAnt_Flash_rb.UseVisualStyleBackColor = true;
            // 
            // captureAnt_Blue_rb
            // 
            this.captureAnt_Blue_rb.AutoSize = true;
            this.captureAnt_Blue_rb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.captureAnt_Blue_rb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.captureAnt_Blue_rb.Location = new System.Drawing.Point(302, 3);
            this.captureAnt_Blue_rb.Name = "captureAnt_Blue_rb";
            this.captureAnt_Blue_rb.Size = new System.Drawing.Size(1, 44);
            this.captureAnt_Blue_rb.TabIndex = 51;
            this.captureAnt_Blue_rb.TabStop = true;
            this.captureAnt_Blue_rb.Text = "Ant_Blue";
            this.captureAnt_Blue_rb.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.captureAnt_Blue_rb.UseVisualStyleBackColor = true;
            // 
            // postProcessing_tbp
            // 
            this.postProcessing_tbp.Controls.Add(this.allChannelLutCbx);
            this.postProcessing_tbp.Controls.Add(this.liveCC_cbx);
            this.postProcessing_tbp.Controls.Add(this.flowLayoutPanel1);
            this.postProcessing_tbp.Controls.Add(this.tempTint_gbx);
            this.postProcessing_tbp.Controls.Add(this.applyChannelWiseLutLklbl);
            this.postProcessing_tbp.Controls.Add(this.linkLabel4);
            this.postProcessing_tbp.Controls.Add(this.linkLabel3);
            this.postProcessing_tbp.Controls.Add(this.linkLabel2);
            this.postProcessing_tbp.Controls.Add(this.linkLabel1);
            this.postProcessing_tbp.Controls.Add(this.contrastSettings_lnklbl);
            this.postProcessing_tbp.Controls.Add(this.brightnessSettings_lnklbl);
            this.postProcessing_tbp.Controls.Add(this.maskSettings_lnkLbl);
            this.postProcessing_tbp.Controls.Add(this.ccSettings_lnkLbl);
            this.postProcessing_tbp.Controls.Add(this.ApplyLiteCorrection_cbx);
            this.postProcessing_tbp.Controls.Add(this.hsSettings_lnkLbl);
            this.postProcessing_tbp.Controls.Add(this.ApplyContrast_cbx);
            this.postProcessing_tbp.Controls.Add(this.shiftSettings_lnkLbl);
            this.postProcessing_tbp.Controls.Add(this.button4);
            this.postProcessing_tbp.Controls.Add(this.applyBrightness_cbx);
            this.postProcessing_tbp.Controls.Add(this.button3);
            this.postProcessing_tbp.Controls.Add(this.button6);
            this.postProcessing_tbp.Controls.Add(this.PostProcessing_btn);
            this.postProcessing_tbp.Controls.Add(this.reloadImage_btn);
            this.postProcessing_tbp.Controls.Add(this.browseRawImage_btn);
            this.postProcessing_tbp.Controls.Add(this.formCheckBox1);
            this.postProcessing_tbp.Controls.Add(this.EnableTemperatureTint_cbx);
            this.postProcessing_tbp.Controls.Add(this.getRaw_btn);
            this.postProcessing_tbp.Controls.Add(this.ApplyHSVBoost_cbx);
            this.postProcessing_tbp.Controls.Add(this.applyClahe_cbx);
            this.postProcessing_tbp.Controls.Add(this.isApplyLut_cbx);
            this.postProcessing_tbp.Controls.Add(this.applyMask_cbx);
            this.postProcessing_tbp.Controls.Add(this.applyColorCorrection_cbx);
            this.postProcessing_tbp.Controls.Add(this.applyHotSpotCorrection_cbx);
            this.postProcessing_tbp.Controls.Add(this.applyShift_cbx);
            this.postProcessing_tbp.Controls.Add(this.saveSettings_btn);
            this.postProcessing_tbp.Location = new System.Drawing.Point(4, 40);
            this.postProcessing_tbp.Name = "postProcessing_tbp";
            this.postProcessing_tbp.Padding = new System.Windows.Forms.Padding(3);
            this.postProcessing_tbp.Size = new System.Drawing.Size(313, 656);
            this.postProcessing_tbp.TabIndex = 3;
            this.postProcessing_tbp.Text = "Post Processing";
            this.postProcessing_tbp.UseVisualStyleBackColor = true;
            // 
            // allChannelLutCbx
            // 
            this.allChannelLutCbx.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.allChannelLutCbx.AutoSize = true;
            this.allChannelLutCbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.allChannelLutCbx.Location = new System.Drawing.Point(15, 362);
            this.allChannelLutCbx.Name = "allChannelLutCbx";
            this.allChannelLutCbx.Size = new System.Drawing.Size(129, 17);
            this.allChannelLutCbx.TabIndex = 98;
            this.allChannelLutCbx.Text = "Channel wise LUT";
            this.allChannelLutCbx.UseVisualStyleBackColor = true;
            this.allChannelLutCbx.CheckedChanged += new System.EventHandler(this.allChannelLutCbx_CheckedChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AllowDrop = true;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(225, 99);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(82, 243);
            this.flowLayoutPanel1.TabIndex = 96;
            this.flowLayoutPanel1.DragDrop += new System.Windows.Forms.DragEventHandler(this.flowLayoutPanel1_DragDrop);
            // 
            // tempTint_gbx
            // 
            this.tempTint_gbx.Controls.Add(this.tableLayoutPanel9);
            this.tempTint_gbx.Enabled = false;
            this.tempTint_gbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tempTint_gbx.Location = new System.Drawing.Point(9, 552);
            this.tempTint_gbx.Name = "tempTint_gbx";
            this.tempTint_gbx.Size = new System.Drawing.Size(293, 68);
            this.tempTint_gbx.TabIndex = 94;
            this.tempTint_gbx.TabStop = false;
            this.tempTint_gbx.Text = "Temperature And Tint";
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 3;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.18584F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.81416F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 287F));
            this.tableLayoutPanel9.Controls.Add(this.temperature_tb, 1, 0);
            this.tableLayoutPanel9.Controls.Add(this.tint_tb, 1, 1);
            this.tableLayoutPanel9.Controls.Add(this.label35, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.label36, 0, 1);
            this.tableLayoutPanel9.Controls.Add(this.TemperatureVal_lbl, 2, 0);
            this.tableLayoutPanel9.Controls.Add(this.tintVal_lbl, 2, 1);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 2;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48.88889F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51.11111F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(287, 49);
            this.tableLayoutPanel9.TabIndex = 0;
            // 
            // temperature_tb
            // 
            this.temperature_tb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.temperature_tb.Location = new System.Drawing.Point(3, 3);
            this.temperature_tb.Maximum = 1000000;
            this.temperature_tb.Minimum = 178;
            this.temperature_tb.Name = "temperature_tb";
            this.temperature_tb.Size = new System.Drawing.Size(1, 17);
            this.temperature_tb.TabIndex = 17;
            this.temperature_tb.Value = 178;
            this.temperature_tb.Scroll += new System.EventHandler(this.temperature_tb_Scroll);
            this.temperature_tb.ValueChanged += new System.EventHandler(this.temperatureVal_nud_ValueChanged);
            // 
            // tint_tb
            // 
            this.tint_tb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tint_tb.Location = new System.Drawing.Point(3, 26);
            this.tint_tb.Maximum = 500;
            this.tint_tb.Minimum = 100;
            this.tint_tb.Name = "tint_tb";
            this.tint_tb.Size = new System.Drawing.Size(1, 20);
            this.tint_tb.TabIndex = 19;
            this.tint_tb.Value = 100;
            this.tint_tb.Scroll += new System.EventHandler(this.tint_tb_Scroll);
            this.tint_tb.ValueChanged += new System.EventHandler(this.tint_tb_ValueChanged);
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.Location = new System.Drawing.Point(3, 0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(1, 23);
            this.label35.TabIndex = 18;
            this.label35.Text = "Temperature";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label36.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label36.Location = new System.Drawing.Point(3, 23);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(1, 26);
            this.label36.TabIndex = 20;
            this.label36.Text = "Tint";
            // 
            // TemperatureVal_lbl
            // 
            this.TemperatureVal_lbl.AutoSize = true;
            this.TemperatureVal_lbl.Location = new System.Drawing.Point(3, 0);
            this.TemperatureVal_lbl.Name = "TemperatureVal_lbl";
            this.TemperatureVal_lbl.Size = new System.Drawing.Size(14, 13);
            this.TemperatureVal_lbl.TabIndex = 21;
            this.TemperatureVal_lbl.Text = "0";
            // 
            // tintVal_lbl
            // 
            this.tintVal_lbl.AutoSize = true;
            this.tintVal_lbl.Location = new System.Drawing.Point(3, 23);
            this.tintVal_lbl.Name = "tintVal_lbl";
            this.tintVal_lbl.Size = new System.Drawing.Size(14, 13);
            this.tintVal_lbl.TabIndex = 21;
            this.tintVal_lbl.Text = "0";
            // 
            // applyChannelWiseLutLklbl
            // 
            this.applyChannelWiseLutLklbl.AutoSize = true;
            this.applyChannelWiseLutLklbl.Location = new System.Drawing.Point(152, 353);
            this.applyChannelWiseLutLklbl.Name = "applyChannelWiseLutLklbl";
            this.applyChannelWiseLutLklbl.Size = new System.Drawing.Size(126, 13);
            this.applyChannelWiseLutLklbl.TabIndex = 89;
            this.applyChannelWiseLutLklbl.TabStop = true;
            this.applyChannelWiseLutLklbl.Text = "Apply Channel Wise LUT";
            this.applyChannelWiseLutLklbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.applyChannelWiseLutLklbl_LinkClicked);
            // 
            // linkLabel4
            // 
            this.linkLabel4.AutoSize = true;
            this.linkLabel4.Location = new System.Drawing.Point(98, 329);
            this.linkLabel4.Name = "linkLabel4";
            this.linkLabel4.Size = new System.Drawing.Size(88, 13);
            this.linkLabel4.TabIndex = 89;
            this.linkLabel4.TabStop = true;
            this.linkLabel4.Text = "Apply HSV Boost";
            this.linkLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ccSettings_lnkLbl_LinkClicked);
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.Location = new System.Drawing.Point(99, 303);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(63, 13);
            this.linkLabel3.TabIndex = 89;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "Apply Clahe";
            this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ccSettings_lnkLbl_LinkClicked);
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(98, 280);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(70, 13);
            this.linkLabel2.TabIndex = 89;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Apply Lite CC";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ccSettings_lnkLbl_LinkClicked);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(98, 256);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(76, 13);
            this.linkLabel1.TabIndex = 89;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Apply Unsharp";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ccSettings_lnkLbl_LinkClicked);
            // 
            // contrastSettings_lnklbl
            // 
            this.contrastSettings_lnklbl.AutoSize = true;
            this.contrastSettings_lnklbl.Location = new System.Drawing.Point(99, 231);
            this.contrastSettings_lnklbl.Name = "contrastSettings_lnklbl";
            this.contrastSettings_lnklbl.Size = new System.Drawing.Size(87, 13);
            this.contrastSettings_lnklbl.TabIndex = 89;
            this.contrastSettings_lnklbl.TabStop = true;
            this.contrastSettings_lnklbl.Text = "Contrast Settings";
            this.contrastSettings_lnklbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ccSettings_lnkLbl_LinkClicked);
            // 
            // brightnessSettings_lnklbl
            // 
            this.brightnessSettings_lnklbl.AutoSize = true;
            this.brightnessSettings_lnklbl.Location = new System.Drawing.Point(99, 206);
            this.brightnessSettings_lnklbl.Name = "brightnessSettings_lnklbl";
            this.brightnessSettings_lnklbl.Size = new System.Drawing.Size(97, 13);
            this.brightnessSettings_lnklbl.TabIndex = 89;
            this.brightnessSettings_lnklbl.TabStop = true;
            this.brightnessSettings_lnklbl.Text = "Brightness Settings";
            this.brightnessSettings_lnklbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ccSettings_lnkLbl_LinkClicked);
            // 
            // maskSettings_lnkLbl
            // 
            this.maskSettings_lnkLbl.AutoSize = true;
            this.maskSettings_lnkLbl.Location = new System.Drawing.Point(98, 180);
            this.maskSettings_lnkLbl.Name = "maskSettings_lnkLbl";
            this.maskSettings_lnkLbl.Size = new System.Drawing.Size(74, 13);
            this.maskSettings_lnkLbl.TabIndex = 89;
            this.maskSettings_lnkLbl.TabStop = true;
            this.maskSettings_lnkLbl.Text = "Mask Settings";
            this.maskSettings_lnkLbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.maskSettings_lnkLbl_LinkClicked);
            // 
            // ccSettings_lnkLbl
            // 
            this.ccSettings_lnkLbl.AutoSize = true;
            this.ccSettings_lnkLbl.Location = new System.Drawing.Point(165, 154);
            this.ccSettings_lnkLbl.Name = "ccSettings_lnkLbl";
            this.ccSettings_lnkLbl.Size = new System.Drawing.Size(62, 13);
            this.ccSettings_lnkLbl.TabIndex = 89;
            this.ccSettings_lnkLbl.TabStop = true;
            this.ccSettings_lnkLbl.Text = "CC Settings";
            this.ccSettings_lnkLbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ccSettings_lnkLbl_LinkClicked);
            // 
            // ApplyLiteCorrection_cbx
            // 
            this.ApplyLiteCorrection_cbx.AutoSize = true;
            this.ApplyLiteCorrection_cbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ApplyLiteCorrection_cbx.Location = new System.Drawing.Point(7, 256);
            this.ApplyLiteCorrection_cbx.Name = "ApplyLiteCorrection_cbx";
            this.ApplyLiteCorrection_cbx.Size = new System.Drawing.Size(73, 17);
            this.ApplyLiteCorrection_cbx.TabIndex = 86;
            this.ApplyLiteCorrection_cbx.Text = "Unsharp";
            this.ApplyLiteCorrection_cbx.UseVisualStyleBackColor = true;
            this.ApplyLiteCorrection_cbx.CheckedChanged += new System.EventHandler(this.ApplyLiteCorrection_cbx_CheckedChanged);
            // 
            // hsSettings_lnkLbl
            // 
            this.hsSettings_lnkLbl.AutoSize = true;
            this.hsSettings_lnkLbl.Location = new System.Drawing.Point(98, 130);
            this.hsSettings_lnkLbl.Name = "hsSettings_lnkLbl";
            this.hsSettings_lnkLbl.Size = new System.Drawing.Size(88, 13);
            this.hsSettings_lnkLbl.TabIndex = 89;
            this.hsSettings_lnkLbl.TabStop = true;
            this.hsSettings_lnkLbl.Text = "Hot-spot Settings";
            this.hsSettings_lnkLbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.hsSettings_lnkLbl_LinkClicked);
            // 
            // ApplyContrast_cbx
            // 
            this.ApplyContrast_cbx.AutoSize = true;
            this.ApplyContrast_cbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ApplyContrast_cbx.Location = new System.Drawing.Point(8, 231);
            this.ApplyContrast_cbx.Name = "ApplyContrast_cbx";
            this.ApplyContrast_cbx.Size = new System.Drawing.Size(73, 17);
            this.ApplyContrast_cbx.TabIndex = 86;
            this.ApplyContrast_cbx.Text = "Contrast";
            this.ApplyContrast_cbx.UseVisualStyleBackColor = true;
            this.ApplyContrast_cbx.CheckedChanged += new System.EventHandler(this.ApplyContrast_cbx_CheckedChanged);
            // 
            // shiftSettings_lnkLbl
            // 
            this.shiftSettings_lnkLbl.AutoSize = true;
            this.shiftSettings_lnkLbl.Location = new System.Drawing.Point(99, 107);
            this.shiftSettings_lnkLbl.Name = "shiftSettings_lnkLbl";
            this.shiftSettings_lnkLbl.Size = new System.Drawing.Size(69, 13);
            this.shiftSettings_lnkLbl.TabIndex = 89;
            this.shiftSettings_lnkLbl.TabStop = true;
            this.shiftSettings_lnkLbl.Text = "Shift Settings";
            this.shiftSettings_lnkLbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.shiftSettings_lnkLbl_LinkClicked);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(32, 70);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(94, 23);
            this.button4.TabIndex = 67;
            this.button4.Text = "Reset Order";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // applyBrightness_cbx
            // 
            this.applyBrightness_cbx.AutoSize = true;
            this.applyBrightness_cbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.applyBrightness_cbx.Location = new System.Drawing.Point(5, 205);
            this.applyBrightness_cbx.Name = "applyBrightness_cbx";
            this.applyBrightness_cbx.Size = new System.Drawing.Size(85, 17);
            this.applyBrightness_cbx.TabIndex = 86;
            this.applyBrightness_cbx.Text = "Brightness";
            this.applyBrightness_cbx.UseVisualStyleBackColor = true;
            this.applyBrightness_cbx.CheckedChanged += new System.EventHandler(this.applyBrightness_cbx_CheckedChanged);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(183, 70);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(94, 23);
            this.button3.TabIndex = 67;
            this.button3.Text = "Batch PP";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(138, 386);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(116, 56);
            this.button6.TabIndex = 58;
            this.button6.Text = "Show O/P ";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // PostProcessing_btn
            // 
            this.PostProcessing_btn.Location = new System.Drawing.Point(10, 386);
            this.PostProcessing_btn.Name = "PostProcessing_btn";
            this.PostProcessing_btn.Size = new System.Drawing.Size(116, 56);
            this.PostProcessing_btn.TabIndex = 58;
            this.PostProcessing_btn.Text = "Update ";
            this.PostProcessing_btn.UseVisualStyleBackColor = true;
            this.PostProcessing_btn.Click += new System.EventHandler(this.PostProcessing_btn_Click);
            // 
            // reloadImage_btn
            // 
            this.reloadImage_btn.Location = new System.Drawing.Point(138, 8);
            this.reloadImage_btn.Name = "reloadImage_btn";
            this.reloadImage_btn.Size = new System.Drawing.Size(119, 56);
            this.reloadImage_btn.TabIndex = 59;
            this.reloadImage_btn.Text = "Reload";
            this.reloadImage_btn.UseVisualStyleBackColor = true;
            this.reloadImage_btn.Click += new System.EventHandler(this.reloadImage_btn_Click);
            // 
            // browseRawImage_btn
            // 
            this.browseRawImage_btn.Location = new System.Drawing.Point(13, 8);
            this.browseRawImage_btn.Name = "browseRawImage_btn";
            this.browseRawImage_btn.Size = new System.Drawing.Size(119, 56);
            this.browseRawImage_btn.TabIndex = 59;
            this.browseRawImage_btn.Text = "Browse";
            this.browseRawImage_btn.UseVisualStyleBackColor = true;
            this.browseRawImage_btn.Click += new System.EventHandler(this.browseRawImage_btn_Click);
            // 
            // shiftImage_tbp
            // 
            this.shiftImage_tbp.Controls.Add(this.ApplyShiftImage_btn);
            this.shiftImage_tbp.Controls.Add(this.label21);
            this.shiftImage_tbp.Controls.Add(this.label20);
            this.shiftImage_tbp.Controls.Add(this.Yshift_nud);
            this.shiftImage_tbp.Controls.Add(this.xShift_nud);
            this.shiftImage_tbp.Controls.Add(this.label19);
            this.shiftImage_tbp.Location = new System.Drawing.Point(4, 40);
            this.shiftImage_tbp.Name = "shiftImage_tbp";
            this.shiftImage_tbp.Padding = new System.Windows.Forms.Padding(3);
            this.shiftImage_tbp.Size = new System.Drawing.Size(313, 656);
            this.shiftImage_tbp.TabIndex = 5;
            this.shiftImage_tbp.Text = "Shift Image";
            this.shiftImage_tbp.UseVisualStyleBackColor = true;
            // 
            // ApplyShiftImage_btn
            // 
            this.ApplyShiftImage_btn.Location = new System.Drawing.Point(38, 92);
            this.ApplyShiftImage_btn.Name = "ApplyShiftImage_btn";
            this.ApplyShiftImage_btn.Size = new System.Drawing.Size(167, 23);
            this.ApplyShiftImage_btn.TabIndex = 130;
            this.ApplyShiftImage_btn.Text = "Apply Shift Correction";
            this.ApplyShiftImage_btn.UseVisualStyleBackColor = true;
            this.ApplyShiftImage_btn.Click += new System.EventHandler(this.ApplyShiftImage_btn_Click);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(137, 48);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(41, 13);
            this.label21.TabIndex = 90;
            this.label21.Text = " Y Shift";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(3, 48);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(41, 13);
            this.label20.TabIndex = 89;
            this.label20.Text = " X Shift";
            // 
            // Yshift_nud
            // 
            this.Yshift_nud.Location = new System.Drawing.Point(184, 46);
            this.Yshift_nud.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.Yshift_nud.Name = "Yshift_nud";
            this.Yshift_nud.Size = new System.Drawing.Size(68, 20);
            this.Yshift_nud.TabIndex = 87;
            this.Yshift_nud.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.Yshift_nud.ValueChanged += new System.EventHandler(this.Yshift_nud_ValueChanged);
            // 
            // xShift_nud
            // 
            this.xShift_nud.Location = new System.Drawing.Point(63, 46);
            this.xShift_nud.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.xShift_nud.Name = "xShift_nud";
            this.xShift_nud.Size = new System.Drawing.Size(68, 20);
            this.xShift_nud.TabIndex = 88;
            this.xShift_nud.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.xShift_nud.ValueChanged += new System.EventHandler(this.xShift_nud_ValueChanged);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(6, 19);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(99, 13);
            this.label19.TabIndex = 86;
            this.label19.Text = "Shift parameters";
            // 
            // ColorCorrection_tbp
            // 
            this.ColorCorrection_tbp.Controls.Add(this.tableLayoutPanel20);
            this.ColorCorrection_tbp.Controls.Add(this.showLut_btn);
            this.ColorCorrection_tbp.Controls.Add(this.ApplyCC_btn);
            this.ColorCorrection_tbp.Controls.Add(this.label4);
            this.ColorCorrection_tbp.Controls.Add(this.label8);
            this.ColorCorrection_tbp.Controls.Add(this.label7);
            this.ColorCorrection_tbp.Controls.Add(this.label5);
            this.ColorCorrection_tbp.Controls.Add(this.blueGreen_nud);
            this.ColorCorrection_tbp.Controls.Add(this.greenGreen_nud);
            this.ColorCorrection_tbp.Controls.Add(this.redGreen_nud);
            this.ColorCorrection_tbp.Controls.Add(this.blueBlue_nud);
            this.ColorCorrection_tbp.Controls.Add(this.blueRed_nud);
            this.ColorCorrection_tbp.Controls.Add(this.greenBlue_nud);
            this.ColorCorrection_tbp.Controls.Add(this.greenRed_nud);
            this.ColorCorrection_tbp.Controls.Add(this.redBlue_nud);
            this.ColorCorrection_tbp.Controls.Add(this.redRed_nud);
            this.ColorCorrection_tbp.Location = new System.Drawing.Point(4, 40);
            this.ColorCorrection_tbp.Name = "ColorCorrection_tbp";
            this.ColorCorrection_tbp.Padding = new System.Windows.Forms.Padding(3);
            this.ColorCorrection_tbp.Size = new System.Drawing.Size(313, 656);
            this.ColorCorrection_tbp.TabIndex = 6;
            this.ColorCorrection_tbp.Text = "Color Correction";
            this.ColorCorrection_tbp.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel20
            // 
            this.tableLayoutPanel20.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.OutsetDouble;
            this.tableLayoutPanel20.ColumnCount = 2;
            this.tableLayoutPanel20.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.93174F));
            this.tableLayoutPanel20.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.06826F));
            this.tableLayoutPanel20.Controls.Add(this.label49, 0, 0);
            this.tableLayoutPanel20.Controls.Add(this.label50, 0, 1);
            this.tableLayoutPanel20.Controls.Add(this.brightness_nud, 1, 0);
            this.tableLayoutPanel20.Controls.Add(this.contrast_nud, 1, 1);
            this.tableLayoutPanel20.Controls.Add(this.offset1_nud, 1, 13);
            this.tableLayoutPanel20.Controls.Add(this.LUT_interval2_nud, 1, 12);
            this.tableLayoutPanel20.Controls.Add(this.LUT_interval1_nud, 1, 11);
            this.tableLayoutPanel20.Controls.Add(this.LUT_SineFactor_nud, 1, 10);
            this.tableLayoutPanel20.Controls.Add(this.hsvBoostVal_nud, 1, 9);
            this.tableLayoutPanel20.Controls.Add(this.medianfilter_nud, 1, 8);
            this.tableLayoutPanel20.Controls.Add(this.label71, 0, 13);
            this.tableLayoutPanel20.Controls.Add(this.label72, 0, 12);
            this.tableLayoutPanel20.Controls.Add(this.label70, 0, 11);
            this.tableLayoutPanel20.Controls.Add(this.label69, 0, 10);
            this.tableLayoutPanel20.Controls.Add(this.label13, 0, 9);
            this.tableLayoutPanel20.Controls.Add(this.label57, 0, 8);
            this.tableLayoutPanel20.Controls.Add(this.label55, 0, 7);
            this.tableLayoutPanel20.Controls.Add(this.unsharpAmount_nud, 1, 7);
            this.tableLayoutPanel20.Controls.Add(this.unsharpRadius_nud, 1, 6);
            this.tableLayoutPanel20.Controls.Add(this.unsharpThresh_nud, 1, 5);
            this.tableLayoutPanel20.Controls.Add(this.ClaheClipValueR_nud, 1, 2);
            this.tableLayoutPanel20.Controls.Add(this.label56, 0, 2);
            this.tableLayoutPanel20.Controls.Add(this.label53, 0, 5);
            this.tableLayoutPanel20.Controls.Add(this.label54, 0, 6);
            this.tableLayoutPanel20.Controls.Add(this.label22, 0, 3);
            this.tableLayoutPanel20.Controls.Add(this.label23, 0, 4);
            this.tableLayoutPanel20.Controls.Add(this.ClaheClipValueG_nud, 1, 3);
            this.tableLayoutPanel20.Controls.Add(this.ClaheClipValueB_nud, 1, 4);
            this.tableLayoutPanel20.Controls.Add(this.applyGamma_cbx, 0, 14);
            this.tableLayoutPanel20.Controls.Add(this.tableLayoutPanel29, 1, 14);
            this.tableLayoutPanel20.Location = new System.Drawing.Point(4, 112);
            this.tableLayoutPanel20.Name = "tableLayoutPanel20";
            this.tableLayoutPanel20.RowCount = 15;
            this.tableLayoutPanel20.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.19737F));
            this.tableLayoutPanel20.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.81967F));
            this.tableLayoutPanel20.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18.59649F));
            this.tableLayoutPanel20.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.77193F));
            this.tableLayoutPanel20.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.45038F));
            this.tableLayoutPanel20.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.160305F));
            this.tableLayoutPanel20.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.02662F));
            this.tableLayoutPanel20.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.01141F));
            this.tableLayoutPanel20.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel20.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel20.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel20.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel20.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel20.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel20.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel20.Size = new System.Drawing.Size(293, 468);
            this.tableLayoutPanel20.TabIndex = 86;
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(6, 3);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(56, 13);
            this.label49.TabIndex = 0;
            this.label49.Text = "Brightness";
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(6, 30);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(46, 13);
            this.label50.TabIndex = 0;
            this.label50.Text = "Contrast";
            // 
            // brightness_nud
            // 
            this.brightness_nud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.brightness_nud.Location = new System.Drawing.Point(122, 6);
            this.brightness_nud.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.brightness_nud.Name = "brightness_nud";
            this.brightness_nud.Size = new System.Drawing.Size(165, 20);
            this.brightness_nud.TabIndex = 1;
            this.brightness_nud.ValueChanged += new System.EventHandler(this.brightnessVal_nud_ValueChanged);
            // 
            // contrast_nud
            // 
            this.contrast_nud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contrast_nud.Location = new System.Drawing.Point(122, 33);
            this.contrast_nud.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.contrast_nud.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            this.contrast_nud.Name = "contrast_nud";
            this.contrast_nud.Size = new System.Drawing.Size(165, 20);
            this.contrast_nud.TabIndex = 2;
            this.contrast_nud.ValueChanged += new System.EventHandler(this.contrastVal_nud_ValueChanged);
            // 
            // offset1_nud
            // 
            this.offset1_nud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.offset1_nud.Location = new System.Drawing.Point(122, 413);
            this.offset1_nud.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.offset1_nud.Name = "offset1_nud";
            this.offset1_nud.Size = new System.Drawing.Size(165, 20);
            this.offset1_nud.TabIndex = 10;
            this.offset1_nud.Value = new decimal(new int[] {
            160,
            0,
            0,
            0});
            this.offset1_nud.ValueChanged += new System.EventHandler(this.offset1_nud_ValueChanged);
            // 
            // LUT_interval2_nud
            // 
            this.LUT_interval2_nud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LUT_interval2_nud.Location = new System.Drawing.Point(122, 386);
            this.LUT_interval2_nud.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.LUT_interval2_nud.Name = "LUT_interval2_nud";
            this.LUT_interval2_nud.Size = new System.Drawing.Size(165, 20);
            this.LUT_interval2_nud.TabIndex = 10;
            this.LUT_interval2_nud.Value = new decimal(new int[] {
            160,
            0,
            0,
            0});
            this.LUT_interval2_nud.ValueChanged += new System.EventHandler(this.LUT_interval2_nud_ValueChanged);
            // 
            // LUT_interval1_nud
            // 
            this.LUT_interval1_nud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LUT_interval1_nud.Location = new System.Drawing.Point(122, 351);
            this.LUT_interval1_nud.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.LUT_interval1_nud.Name = "LUT_interval1_nud";
            this.LUT_interval1_nud.Size = new System.Drawing.Size(165, 20);
            this.LUT_interval1_nud.TabIndex = 7;
            this.LUT_interval1_nud.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.LUT_interval1_nud.ValueChanged += new System.EventHandler(this.LUT_interval1_nud_ValueChanged);
            // 
            // LUT_SineFactor_nud
            // 
            this.LUT_SineFactor_nud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LUT_SineFactor_nud.Location = new System.Drawing.Point(122, 328);
            this.LUT_SineFactor_nud.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.LUT_SineFactor_nud.Name = "LUT_SineFactor_nud";
            this.LUT_SineFactor_nud.Size = new System.Drawing.Size(165, 20);
            this.LUT_SineFactor_nud.TabIndex = 7;
            this.LUT_SineFactor_nud.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.LUT_SineFactor_nud.ValueChanged += new System.EventHandler(this.LUT_SineFactor_nud_ValueChanged);
            // 
            // hsvBoostVal_nud
            // 
            this.hsvBoostVal_nud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hsvBoostVal_nud.Location = new System.Drawing.Point(122, 296);
            this.hsvBoostVal_nud.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.hsvBoostVal_nud.Name = "hsvBoostVal_nud";
            this.hsvBoostVal_nud.Size = new System.Drawing.Size(165, 20);
            this.hsvBoostVal_nud.TabIndex = 7;
            this.hsvBoostVal_nud.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.hsvBoostVal_nud.ValueChanged += new System.EventHandler(this.hsvBoostVal_nud_ValueChanged);
            // 
            // medianfilter_nud
            // 
            this.medianfilter_nud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.medianfilter_nud.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.medianfilter_nud.Location = new System.Drawing.Point(122, 267);
            this.medianfilter_nud.Name = "medianfilter_nud";
            this.medianfilter_nud.Size = new System.Drawing.Size(165, 20);
            this.medianfilter_nud.TabIndex = 7;
            this.medianfilter_nud.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.medianfilter_nud.ValueChanged += new System.EventHandler(this.medianfilter_nud_ValueChanged_1);
            // 
            // label71
            // 
            this.label71.AutoSize = true;
            this.label71.Location = new System.Drawing.Point(6, 410);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(44, 13);
            this.label71.TabIndex = 9;
            this.label71.Text = "Offset 1";
            // 
            // label72
            // 
            this.label72.AutoSize = true;
            this.label72.Location = new System.Drawing.Point(6, 383);
            this.label72.Name = "label72";
            this.label72.Size = new System.Drawing.Size(75, 13);
            this.label72.TabIndex = 9;
            this.label72.Text = "LUT Interval 2";
            // 
            // label70
            // 
            this.label70.AutoSize = true;
            this.label70.Location = new System.Drawing.Point(6, 348);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(75, 13);
            this.label70.TabIndex = 8;
            this.label70.Text = "LUT Interval 1";
            // 
            // label69
            // 
            this.label69.AutoSize = true;
            this.label69.Location = new System.Drawing.Point(6, 325);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(85, 13);
            this.label69.TabIndex = 8;
            this.label69.Text = "LUT Sine Factor";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 293);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(89, 13);
            this.label13.TabIndex = 8;
            this.label13.Text = "HSV Boost Value";
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Location = new System.Drawing.Point(6, 264);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(97, 13);
            this.label57.TabIndex = 8;
            this.label57.Text = "Median Filter Value";
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(6, 215);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(86, 13);
            this.label55.TabIndex = 8;
            this.label55.Text = "Unsharp Amount";
            // 
            // unsharpAmount_nud
            // 
            this.unsharpAmount_nud.DecimalPlaces = 2;
            this.unsharpAmount_nud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.unsharpAmount_nud.Location = new System.Drawing.Point(122, 218);
            this.unsharpAmount_nud.Name = "unsharpAmount_nud";
            this.unsharpAmount_nud.Size = new System.Drawing.Size(165, 20);
            this.unsharpAmount_nud.TabIndex = 7;
            this.unsharpAmount_nud.Value = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            this.unsharpAmount_nud.ValueChanged += new System.EventHandler(this.unsharpAmount_nud_ValueChanged);
            // 
            // unsharpRadius_nud
            // 
            this.unsharpRadius_nud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.unsharpRadius_nud.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.unsharpRadius_nud.Location = new System.Drawing.Point(122, 189);
            this.unsharpRadius_nud.Name = "unsharpRadius_nud";
            this.unsharpRadius_nud.Size = new System.Drawing.Size(165, 20);
            this.unsharpRadius_nud.TabIndex = 6;
            this.unsharpRadius_nud.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.unsharpRadius_nud.ValueChanged += new System.EventHandler(this.unsharpRadius_nud_ValueChanged);
            // 
            // unsharpThresh_nud
            // 
            this.unsharpThresh_nud.DecimalPlaces = 5;
            this.unsharpThresh_nud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.unsharpThresh_nud.Location = new System.Drawing.Point(122, 164);
            this.unsharpThresh_nud.Name = "unsharpThresh_nud";
            this.unsharpThresh_nud.Size = new System.Drawing.Size(165, 20);
            this.unsharpThresh_nud.TabIndex = 5;
            this.unsharpThresh_nud.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.unsharpThresh_nud.ValueChanged += new System.EventHandler(this.unsharpThresh_nud_ValueChanged);
            // 
            // ClaheClipValueR_nud
            // 
            this.ClaheClipValueR_nud.DecimalPlaces = 5;
            this.ClaheClipValueR_nud.Location = new System.Drawing.Point(122, 62);
            this.ClaheClipValueR_nud.Name = "ClaheClipValueR_nud";
            this.ClaheClipValueR_nud.Size = new System.Drawing.Size(165, 20);
            this.ClaheClipValueR_nud.TabIndex = 7;
            this.ClaheClipValueR_nud.Value = new decimal(new int[] {
            2,
            0,
            0,
            196608});
            this.ClaheClipValueR_nud.ValueChanged += new System.EventHandler(this.ClaheClipValueR_nud_ValueChanged);
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Location = new System.Drawing.Point(6, 59);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(95, 13);
            this.label56.TabIndex = 8;
            this.label56.Text = "Clahe Clip Value R";
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Location = new System.Drawing.Point(6, 161);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(54, 13);
            this.label53.TabIndex = 8;
            this.label53.Text = "Threshold";
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Location = new System.Drawing.Point(6, 186);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(40, 13);
            this.label54.TabIndex = 8;
            this.label54.Text = "Radius";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(6, 107);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(95, 13);
            this.label22.TabIndex = 8;
            this.label22.Text = "Clahe Clip Value G";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(6, 131);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(94, 13);
            this.label23.TabIndex = 8;
            this.label23.Text = "Clahe Clip Value B";
            // 
            // ClaheClipValueG_nud
            // 
            this.ClaheClipValueG_nud.DecimalPlaces = 5;
            this.ClaheClipValueG_nud.Location = new System.Drawing.Point(122, 110);
            this.ClaheClipValueG_nud.Name = "ClaheClipValueG_nud";
            this.ClaheClipValueG_nud.Size = new System.Drawing.Size(165, 20);
            this.ClaheClipValueG_nud.TabIndex = 7;
            this.ClaheClipValueG_nud.Value = new decimal(new int[] {
            2,
            0,
            0,
            196608});
            this.ClaheClipValueG_nud.ValueChanged += new System.EventHandler(this.ClaheClipValueG_nud_ValueChanged);
            // 
            // ClaheClipValueB_nud
            // 
            this.ClaheClipValueB_nud.DecimalPlaces = 5;
            this.ClaheClipValueB_nud.Location = new System.Drawing.Point(122, 134);
            this.ClaheClipValueB_nud.Name = "ClaheClipValueB_nud";
            this.ClaheClipValueB_nud.Size = new System.Drawing.Size(165, 20);
            this.ClaheClipValueB_nud.TabIndex = 7;
            this.ClaheClipValueB_nud.Value = new decimal(new int[] {
            2,
            0,
            0,
            196608});
            this.ClaheClipValueB_nud.ValueChanged += new System.EventHandler(this.ClaheClipValueB_nud_ValueChanged);
            // 
            // tableLayoutPanel29
            // 
            this.tableLayoutPanel29.ColumnCount = 2;
            this.tableLayoutPanel29.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.78788F));
            this.tableLayoutPanel29.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.21212F));
            this.tableLayoutPanel29.Controls.Add(this.gammaVal_nud, 1, 0);
            this.tableLayoutPanel29.Controls.Add(this.label48, 0, 0);
            this.tableLayoutPanel29.Location = new System.Drawing.Point(122, 444);
            this.tableLayoutPanel29.Name = "tableLayoutPanel29";
            this.tableLayoutPanel29.RowCount = 1;
            this.tableLayoutPanel29.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel29.Size = new System.Drawing.Size(165, 18);
            this.tableLayoutPanel29.TabIndex = 12;
            // 
            // gammaVal_nud
            // 
            this.gammaVal_nud.DecimalPlaces = 2;
            this.gammaVal_nud.Location = new System.Drawing.Point(100, 3);
            this.gammaVal_nud.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.gammaVal_nud.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            -2147483648});
            this.gammaVal_nud.Name = "gammaVal_nud";
            this.gammaVal_nud.Size = new System.Drawing.Size(62, 20);
            this.gammaVal_nud.TabIndex = 11;
            this.gammaVal_nud.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.gammaVal_nud.ValueChanged += new System.EventHandler(this.gammaVal_nud_ValueChanged_1);
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(3, 0);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(60, 13);
            this.label48.TabIndex = 12;
            this.label48.Text = "Gamma val";
            // 
            // showLut_btn
            // 
            this.showLut_btn.Location = new System.Drawing.Point(13, 620);
            this.showLut_btn.Name = "showLut_btn";
            this.showLut_btn.Size = new System.Drawing.Size(272, 36);
            this.showLut_btn.TabIndex = 10;
            this.showLut_btn.Text = "Show LUT Curve";
            this.showLut_btn.UseVisualStyleBackColor = true;
            this.showLut_btn.Click += new System.EventHandler(this.showLut_btn_Click);
            // 
            // ApplyCC_btn
            // 
            this.ApplyCC_btn.Location = new System.Drawing.Point(13, 586);
            this.ApplyCC_btn.Name = "ApplyCC_btn";
            this.ApplyCC_btn.Size = new System.Drawing.Size(272, 28);
            this.ApplyCC_btn.TabIndex = 10;
            this.ApplyCC_btn.Text = "Apply Color Correction";
            this.ApplyCC_btn.UseVisualStyleBackColor = true;
            this.ApplyCC_btn.Click += new System.EventHandler(this.ApplyCC_btn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(213, 17);
            this.label4.TabIndex = 85;
            this.label4.Text = "Color Correction parameters";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 88);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(28, 13);
            this.label8.TabIndex = 82;
            this.label8.Text = "Blue";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 62);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 13);
            this.label7.TabIndex = 83;
            this.label7.Text = "Green";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(27, 13);
            this.label5.TabIndex = 84;
            this.label5.Text = "Red";
            // 
            // blueGreen_nud
            // 
            this.blueGreen_nud.DecimalPlaces = 2;
            this.blueGreen_nud.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.blueGreen_nud.Location = new System.Drawing.Point(143, 86);
            this.blueGreen_nud.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.blueGreen_nud.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.blueGreen_nud.Name = "blueGreen_nud";
            this.blueGreen_nud.Size = new System.Drawing.Size(55, 20);
            this.blueGreen_nud.TabIndex = 8;
            this.blueGreen_nud.Value = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.blueGreen_nud.ValueChanged += new System.EventHandler(this.blueGreen_nud_ValueChanged);
            this.blueGreen_nud.KeyDown += new System.Windows.Forms.KeyEventHandler(this.blueGreen_nud_KeyDown);
            // 
            // greenGreen_nud
            // 
            this.greenGreen_nud.DecimalPlaces = 2;
            this.greenGreen_nud.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.greenGreen_nud.Location = new System.Drawing.Point(143, 60);
            this.greenGreen_nud.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.greenGreen_nud.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.greenGreen_nud.Name = "greenGreen_nud";
            this.greenGreen_nud.Size = new System.Drawing.Size(55, 20);
            this.greenGreen_nud.TabIndex = 5;
            this.greenGreen_nud.Value = new decimal(new int[] {
            95,
            0,
            0,
            131072});
            this.greenGreen_nud.ValueChanged += new System.EventHandler(this.greenGreen_nud_ValueChanged);
            this.greenGreen_nud.KeyDown += new System.Windows.Forms.KeyEventHandler(this.greenGreen_nud_KeyDown);
            // 
            // redGreen_nud
            // 
            this.redGreen_nud.DecimalPlaces = 2;
            this.redGreen_nud.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.redGreen_nud.Location = new System.Drawing.Point(143, 36);
            this.redGreen_nud.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.redGreen_nud.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.redGreen_nud.Name = "redGreen_nud";
            this.redGreen_nud.Size = new System.Drawing.Size(55, 20);
            this.redGreen_nud.TabIndex = 2;
            this.redGreen_nud.Value = new decimal(new int[] {
            4,
            0,
            0,
            65536});
            this.redGreen_nud.ValueChanged += new System.EventHandler(this.redGreen_nud_ValueChanged);
            this.redGreen_nud.KeyDown += new System.Windows.Forms.KeyEventHandler(this.redGreen_nud_KeyDown);
            // 
            // blueBlue_nud
            // 
            this.blueBlue_nud.DecimalPlaces = 2;
            this.blueBlue_nud.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.blueBlue_nud.Location = new System.Drawing.Point(208, 86);
            this.blueBlue_nud.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.blueBlue_nud.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.blueBlue_nud.Name = "blueBlue_nud";
            this.blueBlue_nud.Size = new System.Drawing.Size(58, 20);
            this.blueBlue_nud.TabIndex = 9;
            this.blueBlue_nud.Value = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            this.blueBlue_nud.ValueChanged += new System.EventHandler(this.blueBlue_nud_ValueChanged);
            this.blueBlue_nud.KeyDown += new System.Windows.Forms.KeyEventHandler(this.blueBlue_nud_KeyDown);
            // 
            // blueRed_nud
            // 
            this.blueRed_nud.DecimalPlaces = 2;
            this.blueRed_nud.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.blueRed_nud.Location = new System.Drawing.Point(78, 86);
            this.blueRed_nud.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.blueRed_nud.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.blueRed_nud.Name = "blueRed_nud";
            this.blueRed_nud.Size = new System.Drawing.Size(53, 20);
            this.blueRed_nud.TabIndex = 7;
            this.blueRed_nud.ValueChanged += new System.EventHandler(this.blueRed_nud_ValueChanged);
            this.blueRed_nud.KeyDown += new System.Windows.Forms.KeyEventHandler(this.blueRed_nud_KeyDown);
            // 
            // greenBlue_nud
            // 
            this.greenBlue_nud.DecimalPlaces = 2;
            this.greenBlue_nud.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.greenBlue_nud.Location = new System.Drawing.Point(208, 60);
            this.greenBlue_nud.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.greenBlue_nud.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.greenBlue_nud.Name = "greenBlue_nud";
            this.greenBlue_nud.Size = new System.Drawing.Size(58, 20);
            this.greenBlue_nud.TabIndex = 6;
            this.greenBlue_nud.ValueChanged += new System.EventHandler(this.greenBlue_nud_ValueChanged);
            this.greenBlue_nud.KeyDown += new System.Windows.Forms.KeyEventHandler(this.greenBlue_nud_KeyDown);
            // 
            // greenRed_nud
            // 
            this.greenRed_nud.DecimalPlaces = 2;
            this.greenRed_nud.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.greenRed_nud.Location = new System.Drawing.Point(78, 60);
            this.greenRed_nud.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.greenRed_nud.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.greenRed_nud.Name = "greenRed_nud";
            this.greenRed_nud.Size = new System.Drawing.Size(53, 20);
            this.greenRed_nud.TabIndex = 4;
            this.greenRed_nud.ValueChanged += new System.EventHandler(this.greenRed_nud_ValueChanged);
            this.greenRed_nud.KeyDown += new System.Windows.Forms.KeyEventHandler(this.greenRed_nud_KeyDown);
            // 
            // redBlue_nud
            // 
            this.redBlue_nud.DecimalPlaces = 2;
            this.redBlue_nud.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.redBlue_nud.Location = new System.Drawing.Point(208, 33);
            this.redBlue_nud.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.redBlue_nud.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.redBlue_nud.Name = "redBlue_nud";
            this.redBlue_nud.Size = new System.Drawing.Size(58, 20);
            this.redBlue_nud.TabIndex = 3;
            this.redBlue_nud.ValueChanged += new System.EventHandler(this.redBlue_nud_ValueChanged);
            this.redBlue_nud.KeyDown += new System.Windows.Forms.KeyEventHandler(this.redBlue_nud_KeyDown);
            // 
            // redRed_nud
            // 
            this.redRed_nud.DecimalPlaces = 2;
            this.redRed_nud.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.redRed_nud.Location = new System.Drawing.Point(78, 36);
            this.redRed_nud.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.redRed_nud.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.redRed_nud.Name = "redRed_nud";
            this.redRed_nud.Size = new System.Drawing.Size(53, 20);
            this.redRed_nud.TabIndex = 1;
            this.redRed_nud.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.redRed_nud.ValueChanged += new System.EventHandler(this.redRed_nud_ValueChanged);
            this.redRed_nud.KeyDown += new System.Windows.Forms.KeyEventHandler(this.redRed_nud_KeyDown);
            // 
            // Mask_tbp
            // 
            this.Mask_tbp.Controls.Add(this.LiveMask_cbx);
            this.Mask_tbp.Controls.Add(this.label52);
            this.Mask_tbp.Controls.Add(this.label40);
            this.Mask_tbp.Controls.Add(this.label38);
            this.Mask_tbp.Controls.Add(this.label39);
            this.Mask_tbp.Controls.Add(this.label73);
            this.Mask_tbp.Controls.Add(this.label41);
            this.Mask_tbp.Controls.Add(this.label29);
            this.Mask_tbp.Controls.Add(this.label74);
            this.Mask_tbp.Controls.Add(this.batchMask_btn);
            this.Mask_tbp.Controls.Add(this.ClearMaskMarking_btn);
            this.Mask_tbp.Controls.Add(this.applyMask_btn);
            this.Mask_tbp.Controls.Add(this.label3);
            this.Mask_tbp.Controls.Add(this.CaptureMaskHeight_nud);
            this.Mask_tbp.Controls.Add(this.CaptureMaskWidth_nud);
            this.Mask_tbp.Controls.Add(this.LiveMaskHeight_nud);
            this.Mask_tbp.Controls.Add(this.LiveMaskWidth_nud);
            this.Mask_tbp.Controls.Add(this.numericUpDown3);
            this.Mask_tbp.Controls.Add(this.maskY_nud);
            this.Mask_tbp.Controls.Add(this.maskX_nud);
            this.Mask_tbp.Location = new System.Drawing.Point(4, 40);
            this.Mask_tbp.Name = "Mask_tbp";
            this.Mask_tbp.Padding = new System.Windows.Forms.Padding(3);
            this.Mask_tbp.Size = new System.Drawing.Size(313, 656);
            this.Mask_tbp.TabIndex = 7;
            this.Mask_tbp.Text = "Mask";
            this.Mask_tbp.UseVisualStyleBackColor = true;
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Location = new System.Drawing.Point(148, -186);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(107, 13);
            this.label52.TabIndex = 78;
            this.label52.Text = "Capture Mask Height";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(165, 176);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(107, 13);
            this.label40.TabIndex = 78;
            this.label40.Text = "Capture Mask Height";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(165, 99);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(90, 13);
            this.label38.TabIndex = 78;
            this.label38.Text = "Live Mask Height";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(18, 176);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(104, 13);
            this.label39.TabIndex = 78;
            this.label39.Text = "Capture Mask Width";
            // 
            // label73
            // 
            this.label73.AutoSize = true;
            this.label73.Location = new System.Drawing.Point(165, 41);
            this.label73.Name = "label73";
            this.label73.Size = new System.Drawing.Size(48, 13);
            this.label73.TabIndex = 77;
            this.label73.Text = "Centre Y";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(18, 41);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(48, 13);
            this.label41.TabIndex = 77;
            this.label41.Text = "Centre X";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(16, 99);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(87, 13);
            this.label29.TabIndex = 78;
            this.label29.Text = "Live Mask Width";
            // 
            // label74
            // 
            this.label74.AutoSize = true;
            this.label74.Location = new System.Drawing.Point(29, 395);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(41, 13);
            this.label74.TabIndex = 77;
            this.label74.Text = "label74";
            // 
            // batchMask_btn
            // 
            this.batchMask_btn.Location = new System.Drawing.Point(21, 353);
            this.batchMask_btn.Name = "batchMask_btn";
            this.batchMask_btn.Size = new System.Drawing.Size(139, 23);
            this.batchMask_btn.TabIndex = 76;
            this.batchMask_btn.Text = "Batch Mask ";
            this.batchMask_btn.UseVisualStyleBackColor = true;
            this.batchMask_btn.Click += new System.EventHandler(this.batchMask_btn_Click);
            // 
            // ClearMaskMarking_btn
            // 
            this.ClearMaskMarking_btn.Location = new System.Drawing.Point(21, 308);
            this.ClearMaskMarking_btn.Name = "ClearMaskMarking_btn";
            this.ClearMaskMarking_btn.Size = new System.Drawing.Size(139, 23);
            this.ClearMaskMarking_btn.TabIndex = 76;
            this.ClearMaskMarking_btn.Text = "Clear Mask Marking";
            this.ClearMaskMarking_btn.UseVisualStyleBackColor = true;
            this.ClearMaskMarking_btn.Click += new System.EventHandler(this.ClearMaskMarking_btn_Click);
            // 
            // applyMask_btn
            // 
            this.applyMask_btn.Location = new System.Drawing.Point(21, 259);
            this.applyMask_btn.Name = "applyMask_btn";
            this.applyMask_btn.Size = new System.Drawing.Size(139, 23);
            this.applyMask_btn.TabIndex = 76;
            this.applyMask_btn.Text = "Apply Mask";
            this.applyMask_btn.UseVisualStyleBackColor = true;
            this.applyMask_btn.Click += new System.EventHandler(this.applyMask_btn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 75;
            this.label3.Text = "Mask Parameters";
            // 
            // CaptureMaskHeight_nud
            // 
            this.CaptureMaskHeight_nud.Location = new System.Drawing.Point(168, 205);
            this.CaptureMaskHeight_nud.Maximum = new decimal(new int[] {
            15000,
            0,
            0,
            0});
            this.CaptureMaskHeight_nud.Name = "CaptureMaskHeight_nud";
            this.CaptureMaskHeight_nud.Size = new System.Drawing.Size(84, 20);
            this.CaptureMaskHeight_nud.TabIndex = 71;
            this.CaptureMaskHeight_nud.Value = new decimal(new int[] {
            1600,
            0,
            0,
            0});
            this.CaptureMaskHeight_nud.ValueChanged += new System.EventHandler(this.CaptureMaskHeight_nud_ValueChanged);
            // 
            // CaptureMaskWidth_nud
            // 
            this.CaptureMaskWidth_nud.Location = new System.Drawing.Point(19, 205);
            this.CaptureMaskWidth_nud.Maximum = new decimal(new int[] {
            15000,
            0,
            0,
            0});
            this.CaptureMaskWidth_nud.Name = "CaptureMaskWidth_nud";
            this.CaptureMaskWidth_nud.Size = new System.Drawing.Size(84, 20);
            this.CaptureMaskWidth_nud.TabIndex = 72;
            this.CaptureMaskWidth_nud.Value = new decimal(new int[] {
            1600,
            0,
            0,
            0});
            this.CaptureMaskWidth_nud.ValueChanged += new System.EventHandler(this.CaptureMaskWidth_nud_ValueChanged);
            // 
            // LiveMaskHeight_nud
            // 
            this.LiveMaskHeight_nud.Location = new System.Drawing.Point(168, 124);
            this.LiveMaskHeight_nud.Maximum = new decimal(new int[] {
            15000,
            0,
            0,
            0});
            this.LiveMaskHeight_nud.Name = "LiveMaskHeight_nud";
            this.LiveMaskHeight_nud.Size = new System.Drawing.Size(84, 20);
            this.LiveMaskHeight_nud.TabIndex = 71;
            this.LiveMaskHeight_nud.Value = new decimal(new int[] {
            1600,
            0,
            0,
            0});
            this.LiveMaskHeight_nud.ValueChanged += new System.EventHandler(this.maskHeight_nud_ValueChanged);
            // 
            // LiveMaskWidth_nud
            // 
            this.LiveMaskWidth_nud.Location = new System.Drawing.Point(19, 124);
            this.LiveMaskWidth_nud.Maximum = new decimal(new int[] {
            15000,
            0,
            0,
            0});
            this.LiveMaskWidth_nud.Name = "LiveMaskWidth_nud";
            this.LiveMaskWidth_nud.Size = new System.Drawing.Size(84, 20);
            this.LiveMaskWidth_nud.TabIndex = 72;
            this.LiveMaskWidth_nud.Value = new decimal(new int[] {
            1600,
            0,
            0,
            0});
            this.LiveMaskWidth_nud.ValueChanged += new System.EventHandler(this.maskWidth_nud_ValueChanged);
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(322, 49);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            15000,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(84, 20);
            this.numericUpDown3.TabIndex = 73;
            this.numericUpDown3.Value = new decimal(new int[] {
            645,
            0,
            0,
            0});
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.maskY_nud_ValueChanged);
            // 
            // maskY_nud
            // 
            this.maskY_nud.Location = new System.Drawing.Point(168, 62);
            this.maskY_nud.Maximum = new decimal(new int[] {
            15000,
            0,
            0,
            0});
            this.maskY_nud.Name = "maskY_nud";
            this.maskY_nud.Size = new System.Drawing.Size(84, 20);
            this.maskY_nud.TabIndex = 73;
            this.maskY_nud.Value = new decimal(new int[] {
            645,
            0,
            0,
            0});
            this.maskY_nud.ValueChanged += new System.EventHandler(this.maskY_nud_ValueChanged);
            // 
            // maskX_nud
            // 
            this.maskX_nud.Location = new System.Drawing.Point(19, 62);
            this.maskX_nud.Maximum = new decimal(new int[] {
            15000,
            0,
            0,
            0});
            this.maskX_nud.Name = "maskX_nud";
            this.maskX_nud.Size = new System.Drawing.Size(84, 20);
            this.maskX_nud.TabIndex = 74;
            this.maskX_nud.Value = new decimal(new int[] {
            1050,
            0,
            0,
            0});
            this.maskX_nud.ValueChanged += new System.EventHandler(this.maskX_nud_ValueChanged);
            // 
            // CentreImage_tbp
            // 
            this.CentreImage_tbp.Controls.Add(this.setCentreImage_btn);
            this.CentreImage_tbp.Location = new System.Drawing.Point(4, 40);
            this.CentreImage_tbp.Name = "CentreImage_tbp";
            this.CentreImage_tbp.Padding = new System.Windows.Forms.Padding(3);
            this.CentreImage_tbp.Size = new System.Drawing.Size(313, 656);
            this.CentreImage_tbp.TabIndex = 8;
            this.CentreImage_tbp.Text = "Centre";
            this.CentreImage_tbp.UseVisualStyleBackColor = true;
            // 
            // setCentreImage_btn
            // 
            this.setCentreImage_btn.Location = new System.Drawing.Point(15, 107);
            this.setCentreImage_btn.Name = "setCentreImage_btn";
            this.setCentreImage_btn.Size = new System.Drawing.Size(75, 23);
            this.setCentreImage_btn.TabIndex = 90;
            this.setCentreImage_btn.Text = "Set Centre";
            this.setCentreImage_btn.UseVisualStyleBackColor = true;
            // 
            // misc_tbpg
            // 
            this.misc_tbpg.Controls.Add(this.panel3);
            this.misc_tbpg.Location = new System.Drawing.Point(4, 40);
            this.misc_tbpg.Name = "misc_tbpg";
            this.misc_tbpg.Padding = new System.Windows.Forms.Padding(3);
            this.misc_tbpg.Size = new System.Drawing.Size(313, 656);
            this.misc_tbpg.TabIndex = 9;
            this.misc_tbpg.Text = "Misc";
            this.misc_tbpg.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(307, 650);
            this.panel3.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.tableLayoutPanel14);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(307, 650);
            this.panel4.TabIndex = 2;
            // 
            // tableLayoutPanel14
            // 
            this.tableLayoutPanel14.ColumnCount = 1;
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel14.Controls.Add(this.tableLayoutPanel13, 0, 10);
            this.tableLayoutPanel14.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel14.Controls.Add(this.groupBox3, 0, 1);
            this.tableLayoutPanel14.Controls.Add(this.motorSensor_gbx, 0, 2);
            this.tableLayoutPanel14.Controls.Add(this.panel8, 0, 3);
            this.tableLayoutPanel14.Controls.Add(this.tableLayoutPanel23, 0, 7);
            this.tableLayoutPanel14.Controls.Add(this.tableLayoutPanel24, 0, 8);
            this.tableLayoutPanel14.Controls.Add(this.tableLayoutPanel25, 0, 9);
            this.tableLayoutPanel14.Controls.Add(this.tableLayoutPanel26, 0, 4);
            this.tableLayoutPanel14.Controls.Add(this.tableLayoutPanel27, 0, 5);
            this.tableLayoutPanel14.Controls.Add(this.tableLayoutPanel28, 0, 6);
            this.tableLayoutPanel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel14.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.RowCount = 11;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.606299F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.81102F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.976377F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.46708F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.04441F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.269526F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.350689F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.188361F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.963246F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.78254F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel14.Size = new System.Drawing.Size(307, 650);
            this.tableLayoutPanel14.TabIndex = 0;
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.ColumnCount = 5;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.81911F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.77133F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel13.Controls.Add(this.colorChannel_btn, 1, 0);
            this.tableLayoutPanel13.Controls.Add(this.redChannel_btn, 2, 0);
            this.tableLayoutPanel13.Controls.Add(this.greenChannel_btn, 3, 0);
            this.tableLayoutPanel13.Controls.Add(this.blueChannel_btn, 4, 0);
            this.tableLayoutPanel13.Controls.Add(this.label42, 0, 0);
            this.tableLayoutPanel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel13.Location = new System.Drawing.Point(3, 602);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.RowCount = 1;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel13.Size = new System.Drawing.Size(301, 45);
            this.tableLayoutPanel13.TabIndex = 60;
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label42.Location = new System.Drawing.Point(3, 0);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(53, 17);
            this.label42.TabIndex = 1;
            this.label42.Text = "Filters";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.vFlip_cbx);
            this.groupBox2.Controls.Add(this.hFlip_cbx);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(273, 42);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Frame Rotate";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableLayoutPanel15);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 61);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(301, 65);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Left Right Sensor";
            // 
            // tableLayoutPanel15
            // 
            this.tableLayoutPanel15.ColumnCount = 2;
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel15.Controls.Add(this.left_rb, 0, 0);
            this.tableLayoutPanel15.Controls.Add(this.right_rb, 1, 0);
            this.tableLayoutPanel15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel15.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel15.Name = "tableLayoutPanel15";
            this.tableLayoutPanel15.RowCount = 1;
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel15.Size = new System.Drawing.Size(295, 46);
            this.tableLayoutPanel15.TabIndex = 0;
            // 
            // left_rb
            // 
            this.left_rb.AutoSize = true;
            this.left_rb.Location = new System.Drawing.Point(3, 3);
            this.left_rb.Name = "left_rb";
            this.left_rb.Size = new System.Drawing.Size(40, 17);
            this.left_rb.TabIndex = 0;
            this.left_rb.TabStop = true;
            this.left_rb.Text = "OS";
            this.left_rb.UseVisualStyleBackColor = true;
            // 
            // right_rb
            // 
            this.right_rb.AutoSize = true;
            this.right_rb.Location = new System.Drawing.Point(150, 3);
            this.right_rb.Name = "right_rb";
            this.right_rb.Size = new System.Drawing.Size(41, 17);
            this.right_rb.TabIndex = 0;
            this.right_rb.TabStop = true;
            this.right_rb.Text = "OD";
            this.right_rb.UseVisualStyleBackColor = true;
            // 
            // motorSensor_gbx
            // 
            this.motorSensor_gbx.Controls.Add(this.tableLayoutPanel16);
            this.motorSensor_gbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.motorSensor_gbx.Location = new System.Drawing.Point(3, 132);
            this.motorSensor_gbx.Name = "motorSensor_gbx";
            this.motorSensor_gbx.Size = new System.Drawing.Size(301, 48);
            this.motorSensor_gbx.TabIndex = 2;
            this.motorSensor_gbx.TabStop = false;
            this.motorSensor_gbx.Text = "Motor Sensor";
            // 
            // tableLayoutPanel16
            // 
            this.tableLayoutPanel16.ColumnCount = 2;
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 86.47687F));
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.52313F));
            this.tableLayoutPanel16.Controls.Add(this.positivePos_p, 0, 0);
            this.tableLayoutPanel16.Controls.Add(this.negativePos_p, 0, 1);
            this.tableLayoutPanel16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel16.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel16.Name = "tableLayoutPanel16";
            this.tableLayoutPanel16.RowCount = 2;
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel16.Size = new System.Drawing.Size(295, 29);
            this.tableLayoutPanel16.TabIndex = 0;
            // 
            // positivePos_p
            // 
            this.positivePos_p.Dock = System.Windows.Forms.DockStyle.Fill;
            this.positivePos_p.Location = new System.Drawing.Point(3, 3);
            this.positivePos_p.Name = "positivePos_p";
            this.positivePos_p.Size = new System.Drawing.Size(249, 8);
            this.positivePos_p.TabIndex = 0;
            // 
            // negativePos_p
            // 
            this.negativePos_p.Dock = System.Windows.Forms.DockStyle.Fill;
            this.negativePos_p.Location = new System.Drawing.Point(3, 17);
            this.negativePos_p.Name = "negativePos_p";
            this.negativePos_p.Size = new System.Drawing.Size(249, 9);
            this.negativePos_p.TabIndex = 0;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.saveLiveFrame_btn);
            this.panel8.Controls.Add(this.applyOverlayGrid_btn);
            this.panel8.Controls.Add(this.unsharp_btn);
            this.panel8.Controls.Add(this.createLUT_btn);
            this.panel8.Location = new System.Drawing.Point(3, 186);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(273, 88);
            this.panel8.TabIndex = 3;
            // 
            // tableLayoutPanel23
            // 
            this.tableLayoutPanel23.ColumnCount = 3;
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.40418F));
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.47735F));
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.83972F));
            this.tableLayoutPanel23.Controls.Add(this.redGain_tb, 1, 0);
            this.tableLayoutPanel23.Controls.Add(this.label9, 0, 0);
            this.tableLayoutPanel23.Controls.Add(this.redGainVal_lbl, 2, 0);
            this.tableLayoutPanel23.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel23.Location = new System.Drawing.Point(3, 416);
            this.tableLayoutPanel23.Name = "tableLayoutPanel23";
            this.tableLayoutPanel23.RowCount = 1;
            this.tableLayoutPanel23.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel23.Size = new System.Drawing.Size(301, 49);
            this.tableLayoutPanel23.TabIndex = 4;
            // 
            // redGain_tb
            // 
            this.redGain_tb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.redGain_tb.Location = new System.Drawing.Point(100, 3);
            this.redGain_tb.Maximum = 128;
            this.redGain_tb.Minimum = -128;
            this.redGain_tb.Name = "redGain_tb";
            this.redGain_tb.Size = new System.Drawing.Size(143, 43);
            this.redGain_tb.TabIndex = 0;
            this.redGain_tb.Scroll += new System.EventHandler(this.redGain_tb_Scroll_2);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Red Gain";
            // 
            // redGainVal_lbl
            // 
            this.redGainVal_lbl.AutoSize = true;
            this.redGainVal_lbl.Location = new System.Drawing.Point(249, 0);
            this.redGainVal_lbl.Name = "redGainVal_lbl";
            this.redGainVal_lbl.Size = new System.Drawing.Size(13, 13);
            this.redGainVal_lbl.TabIndex = 1;
            this.redGainVal_lbl.Text = "0";
            // 
            // tableLayoutPanel24
            // 
            this.tableLayoutPanel24.ColumnCount = 3;
            this.tableLayoutPanel24.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.75261F));
            this.tableLayoutPanel24.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.12892F));
            this.tableLayoutPanel24.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.83972F));
            this.tableLayoutPanel24.Controls.Add(this.greenGain_tb, 1, 0);
            this.tableLayoutPanel24.Controls.Add(this.label10, 0, 0);
            this.tableLayoutPanel24.Controls.Add(this.greenGainVal_lbl, 2, 0);
            this.tableLayoutPanel24.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel24.Location = new System.Drawing.Point(3, 471);
            this.tableLayoutPanel24.Name = "tableLayoutPanel24";
            this.tableLayoutPanel24.RowCount = 1;
            this.tableLayoutPanel24.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel24.Size = new System.Drawing.Size(301, 42);
            this.tableLayoutPanel24.TabIndex = 4;
            // 
            // greenGain_tb
            // 
            this.greenGain_tb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.greenGain_tb.Location = new System.Drawing.Point(101, 3);
            this.greenGain_tb.Maximum = 128;
            this.greenGain_tb.Minimum = -128;
            this.greenGain_tb.Name = "greenGain_tb";
            this.greenGain_tb.Size = new System.Drawing.Size(142, 36);
            this.greenGain_tb.TabIndex = 0;
            this.greenGain_tb.Scroll += new System.EventHandler(this.greenGain_tb_Scroll_2);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Green Gain";
            // 
            // greenGainVal_lbl
            // 
            this.greenGainVal_lbl.AutoSize = true;
            this.greenGainVal_lbl.Location = new System.Drawing.Point(249, 0);
            this.greenGainVal_lbl.Name = "greenGainVal_lbl";
            this.greenGainVal_lbl.Size = new System.Drawing.Size(13, 13);
            this.greenGainVal_lbl.TabIndex = 1;
            this.greenGainVal_lbl.Text = "0";
            // 
            // tableLayoutPanel25
            // 
            this.tableLayoutPanel25.ColumnCount = 3;
            this.tableLayoutPanel25.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.75261F));
            this.tableLayoutPanel25.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.12892F));
            this.tableLayoutPanel25.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.83972F));
            this.tableLayoutPanel25.Controls.Add(this.blueGain_tb, 1, 0);
            this.tableLayoutPanel25.Controls.Add(this.label11, 0, 0);
            this.tableLayoutPanel25.Controls.Add(this.blueGainVal_lbl, 2, 0);
            this.tableLayoutPanel25.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel25.Location = new System.Drawing.Point(3, 519);
            this.tableLayoutPanel25.Name = "tableLayoutPanel25";
            this.tableLayoutPanel25.RowCount = 1;
            this.tableLayoutPanel25.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel25.Size = new System.Drawing.Size(301, 77);
            this.tableLayoutPanel25.TabIndex = 4;
            // 
            // blueGain_tb
            // 
            this.blueGain_tb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blueGain_tb.Location = new System.Drawing.Point(101, 3);
            this.blueGain_tb.Maximum = 128;
            this.blueGain_tb.Minimum = -128;
            this.blueGain_tb.Name = "blueGain_tb";
            this.blueGain_tb.Size = new System.Drawing.Size(142, 71);
            this.blueGain_tb.TabIndex = 0;
            this.blueGain_tb.Scroll += new System.EventHandler(this.blueGain_tb_Scroll_2);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "Blue Gain";
            // 
            // blueGainVal_lbl
            // 
            this.blueGainVal_lbl.AutoSize = true;
            this.blueGainVal_lbl.Location = new System.Drawing.Point(249, 0);
            this.blueGainVal_lbl.Name = "blueGainVal_lbl";
            this.blueGainVal_lbl.Size = new System.Drawing.Size(13, 13);
            this.blueGainVal_lbl.TabIndex = 1;
            this.blueGainVal_lbl.Text = "0";
            // 
            // tableLayoutPanel26
            // 
            this.tableLayoutPanel26.ColumnCount = 3;
            this.tableLayoutPanel26.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.40418F));
            this.tableLayoutPanel26.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.47735F));
            this.tableLayoutPanel26.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.83972F));
            this.tableLayoutPanel26.Controls.Add(this.liveRedGain_tb, 1, 0);
            this.tableLayoutPanel26.Controls.Add(this.label12, 0, 0);
            this.tableLayoutPanel26.Controls.Add(this.liveR_val_lbl, 2, 0);
            this.tableLayoutPanel26.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel26.Location = new System.Drawing.Point(3, 280);
            this.tableLayoutPanel26.Name = "tableLayoutPanel26";
            this.tableLayoutPanel26.RowCount = 1;
            this.tableLayoutPanel26.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel26.Size = new System.Drawing.Size(301, 36);
            this.tableLayoutPanel26.TabIndex = 4;
            // 
            // liveRedGain_tb
            // 
            this.liveRedGain_tb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.liveRedGain_tb.Location = new System.Drawing.Point(100, 3);
            this.liveRedGain_tb.Maximum = 128;
            this.liveRedGain_tb.Minimum = -128;
            this.liveRedGain_tb.Name = "liveRedGain_tb";
            this.liveRedGain_tb.Size = new System.Drawing.Size(143, 30);
            this.liveRedGain_tb.TabIndex = 0;
            this.liveRedGain_tb.Scroll += new System.EventHandler(this.liveRedGain_tb_Scroll);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(75, 13);
            this.label12.TabIndex = 1;
            this.label12.Text = "Live Red Gain";
            // 
            // liveR_val_lbl
            // 
            this.liveR_val_lbl.AutoSize = true;
            this.liveR_val_lbl.Location = new System.Drawing.Point(249, 0);
            this.liveR_val_lbl.Name = "liveR_val_lbl";
            this.liveR_val_lbl.Size = new System.Drawing.Size(13, 13);
            this.liveR_val_lbl.TabIndex = 1;
            this.liveR_val_lbl.Text = "0";
            // 
            // tableLayoutPanel27
            // 
            this.tableLayoutPanel27.ColumnCount = 3;
            this.tableLayoutPanel27.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.40418F));
            this.tableLayoutPanel27.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.47735F));
            this.tableLayoutPanel27.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.83972F));
            this.tableLayoutPanel27.Controls.Add(this.liveGreenGain_tb, 1, 0);
            this.tableLayoutPanel27.Controls.Add(this.label58, 0, 0);
            this.tableLayoutPanel27.Controls.Add(this.liveG_val_lbl, 2, 0);
            this.tableLayoutPanel27.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel27.Location = new System.Drawing.Point(3, 322);
            this.tableLayoutPanel27.Name = "tableLayoutPanel27";
            this.tableLayoutPanel27.RowCount = 1;
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel27.Size = new System.Drawing.Size(301, 44);
            this.tableLayoutPanel27.TabIndex = 4;
            // 
            // liveGreenGain_tb
            // 
            this.liveGreenGain_tb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.liveGreenGain_tb.Location = new System.Drawing.Point(100, 3);
            this.liveGreenGain_tb.Maximum = 128;
            this.liveGreenGain_tb.Minimum = -128;
            this.liveGreenGain_tb.Name = "liveGreenGain_tb";
            this.liveGreenGain_tb.Size = new System.Drawing.Size(143, 38);
            this.liveGreenGain_tb.TabIndex = 0;
            this.liveGreenGain_tb.Scroll += new System.EventHandler(this.liveGreenGain_tb_Scroll);
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Location = new System.Drawing.Point(3, 0);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(84, 13);
            this.label58.TabIndex = 1;
            this.label58.Text = "Live Green Gain";
            // 
            // liveG_val_lbl
            // 
            this.liveG_val_lbl.AutoSize = true;
            this.liveG_val_lbl.Location = new System.Drawing.Point(249, 0);
            this.liveG_val_lbl.Name = "liveG_val_lbl";
            this.liveG_val_lbl.Size = new System.Drawing.Size(13, 13);
            this.liveG_val_lbl.TabIndex = 1;
            this.liveG_val_lbl.Text = "0";
            // 
            // tableLayoutPanel28
            // 
            this.tableLayoutPanel28.ColumnCount = 3;
            this.tableLayoutPanel28.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.40418F));
            this.tableLayoutPanel28.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.47735F));
            this.tableLayoutPanel28.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.83972F));
            this.tableLayoutPanel28.Controls.Add(this.liveBlueGain_tb, 1, 0);
            this.tableLayoutPanel28.Controls.Add(this.label60, 0, 0);
            this.tableLayoutPanel28.Controls.Add(this.liveB_val_lbl, 2, 0);
            this.tableLayoutPanel28.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel28.Location = new System.Drawing.Point(3, 372);
            this.tableLayoutPanel28.Name = "tableLayoutPanel28";
            this.tableLayoutPanel28.RowCount = 1;
            this.tableLayoutPanel28.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel28.Size = new System.Drawing.Size(301, 38);
            this.tableLayoutPanel28.TabIndex = 4;
            // 
            // liveBlueGain_tb
            // 
            this.liveBlueGain_tb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.liveBlueGain_tb.Location = new System.Drawing.Point(100, 3);
            this.liveBlueGain_tb.Maximum = 128;
            this.liveBlueGain_tb.Minimum = -128;
            this.liveBlueGain_tb.Name = "liveBlueGain_tb";
            this.liveBlueGain_tb.Size = new System.Drawing.Size(143, 32);
            this.liveBlueGain_tb.TabIndex = 0;
            this.liveBlueGain_tb.Scroll += new System.EventHandler(this.liveBlueGain_tb_Scroll);
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Location = new System.Drawing.Point(3, 0);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(76, 13);
            this.label60.TabIndex = 1;
            this.label60.Text = "Live Blue Gain";
            // 
            // liveB_val_lbl
            // 
            this.liveB_val_lbl.AutoSize = true;
            this.liveB_val_lbl.Location = new System.Drawing.Point(249, 0);
            this.liveB_val_lbl.Name = "liveB_val_lbl";
            this.liveB_val_lbl.Size = new System.Drawing.Size(13, 13);
            this.liveB_val_lbl.TabIndex = 1;
            this.liveB_val_lbl.Text = "0";
            // 
            // ffa_tbpg
            // 
            this.ffa_tbpg.Controls.Add(this.tableLayoutPanel18);
            this.ffa_tbpg.Location = new System.Drawing.Point(4, 40);
            this.ffa_tbpg.Name = "ffa_tbpg";
            this.ffa_tbpg.Padding = new System.Windows.Forms.Padding(3);
            this.ffa_tbpg.Size = new System.Drawing.Size(313, 656);
            this.ffa_tbpg.TabIndex = 10;
            this.ffa_tbpg.Text = "FFA";
            this.ffa_tbpg.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel18
            // 
            this.tableLayoutPanel18.ColumnCount = 1;
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel18.Controls.Add(this.tableLayoutPanel19, 0, 1);
            this.tableLayoutPanel18.Controls.Add(this.panel5, 0, 2);
            this.tableLayoutPanel18.Controls.Add(this.tableLayoutPanel33, 0, 4);
            this.tableLayoutPanel18.Controls.Add(this.panel11, 0, 3);
            this.tableLayoutPanel18.Controls.Add(this.panel14, 0, 0);
            this.tableLayoutPanel18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel18.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel18.Name = "tableLayoutPanel18";
            this.tableLayoutPanel18.RowCount = 5;
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 29.43548F));
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.58824F));
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.47059F));
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.52941F));
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 341F));
            this.tableLayoutPanel18.Size = new System.Drawing.Size(307, 650);
            this.tableLayoutPanel18.TabIndex = 0;
            // 
            // tableLayoutPanel19
            // 
            this.tableLayoutPanel19.ColumnCount = 2;
            this.tableLayoutPanel19.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.00733F));
            this.tableLayoutPanel19.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.69643F));
            this.tableLayoutPanel19.Controls.Add(this.label46, 0, 0);
            this.tableLayoutPanel19.Controls.Add(this.label47, 1, 0);
            this.tableLayoutPanel19.Controls.Add(this.greenFilterPos_nud, 0, 1);
            this.tableLayoutPanel19.Controls.Add(this.blueFilterPos_nud, 1, 1);
            this.tableLayoutPanel19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel19.Location = new System.Drawing.Point(3, 93);
            this.tableLayoutPanel19.Name = "tableLayoutPanel19";
            this.tableLayoutPanel19.RowCount = 2;
            this.tableLayoutPanel19.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel19.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel19.Size = new System.Drawing.Size(301, 88);
            this.tableLayoutPanel19.TabIndex = 1;
            // 
            // label46
            // 
            this.label46.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label46.Location = new System.Drawing.Point(3, 0);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(129, 44);
            this.label46.TabIndex = 0;
            this.label46.Text = "Green Filter Pos";
            // 
            // label47
            // 
            this.label47.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label47.Location = new System.Drawing.Point(138, 0);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(160, 44);
            this.label47.TabIndex = 1;
            this.label47.Text = "Blue Filter Pos";
            // 
            // greenFilterPos_nud
            // 
            this.greenFilterPos_nud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.greenFilterPos_nud.Location = new System.Drawing.Point(3, 47);
            this.greenFilterPos_nud.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.greenFilterPos_nud.Minimum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.greenFilterPos_nud.Name = "greenFilterPos_nud";
            this.greenFilterPos_nud.Size = new System.Drawing.Size(129, 20);
            this.greenFilterPos_nud.TabIndex = 0;
            this.greenFilterPos_nud.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.greenFilterPos_nud.ValueChanged += new System.EventHandler(this.greenFilterPos_nud_ValueChanged);
            // 
            // blueFilterPos_nud
            // 
            this.blueFilterPos_nud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blueFilterPos_nud.Location = new System.Drawing.Point(138, 47);
            this.blueFilterPos_nud.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.blueFilterPos_nud.Minimum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.blueFilterPos_nud.Name = "blueFilterPos_nud";
            this.blueFilterPos_nud.Size = new System.Drawing.Size(160, 20);
            this.blueFilterPos_nud.TabIndex = 0;
            this.blueFilterPos_nud.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.blueFilterPos_nud.ValueChanged += new System.EventHandler(this.blueFilterPos_nud_ValueChanged);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.eightBit_rb);
            this.panel5.Controls.Add(this.forteenBit_rb);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 187);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(301, 44);
            this.panel5.TabIndex = 3;
            // 
            // eightBit_rb
            // 
            this.eightBit_rb.AutoSize = true;
            this.eightBit_rb.Location = new System.Drawing.Point(66, 5);
            this.eightBit_rb.Name = "eightBit_rb";
            this.eightBit_rb.Size = new System.Drawing.Size(45, 17);
            this.eightBit_rb.TabIndex = 0;
            this.eightBit_rb.TabStop = true;
            this.eightBit_rb.Text = "8 bit";
            this.eightBit_rb.UseVisualStyleBackColor = true;
            // 
            // forteenBit_rb
            // 
            this.forteenBit_rb.AutoSize = true;
            this.forteenBit_rb.Location = new System.Drawing.Point(6, 4);
            this.forteenBit_rb.Name = "forteenBit_rb";
            this.forteenBit_rb.Size = new System.Drawing.Size(54, 17);
            this.forteenBit_rb.TabIndex = 0;
            this.forteenBit_rb.TabStop = true;
            this.forteenBit_rb.Text = "14 bit ";
            this.forteenBit_rb.UseVisualStyleBackColor = true;
            this.forteenBit_rb.CheckedChanged += new System.EventHandler(this.forteenBit_rb_CheckedChanged);
            // 
            // tableLayoutPanel33
            // 
            this.tableLayoutPanel33.ColumnCount = 2;
            this.tableLayoutPanel33.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel33.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel33.Controls.Add(this.label75, 0, 0);
            this.tableLayoutPanel33.Controls.Add(this.label76, 0, 1);
            this.tableLayoutPanel33.Controls.Add(this.frameDetectionVal_nud, 1, 0);
            this.tableLayoutPanel33.Controls.Add(this.darkFameDetectionVal_nud, 1, 1);
            this.tableLayoutPanel33.Location = new System.Drawing.Point(3, 309);
            this.tableLayoutPanel33.Name = "tableLayoutPanel33";
            this.tableLayoutPanel33.RowCount = 2;
            this.tableLayoutPanel33.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel33.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel33.Size = new System.Drawing.Size(273, 86);
            this.tableLayoutPanel33.TabIndex = 4;
            // 
            // label75
            // 
            this.label75.AutoSize = true;
            this.label75.Location = new System.Drawing.Point(3, 0);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(115, 13);
            this.label75.TabIndex = 0;
            this.label75.Text = "Frame Detection Value";
            // 
            // label76
            // 
            this.label76.AutoSize = true;
            this.label76.Location = new System.Drawing.Point(3, 43);
            this.label76.Name = "label76";
            this.label76.Size = new System.Drawing.Size(114, 26);
            this.label76.TabIndex = 0;
            this.label76.Text = "Dark Frame Detection Value";
            // 
            // frameDetectionVal_nud
            // 
            this.frameDetectionVal_nud.DecimalPlaces = 1;
            this.frameDetectionVal_nud.Location = new System.Drawing.Point(139, 3);
            this.frameDetectionVal_nud.Name = "frameDetectionVal_nud";
            this.frameDetectionVal_nud.Size = new System.Drawing.Size(120, 20);
            this.frameDetectionVal_nud.TabIndex = 1;
            this.frameDetectionVal_nud.Value = new decimal(new int[] {
            25,
            0,
            0,
            65536});
            this.frameDetectionVal_nud.ValueChanged += new System.EventHandler(this.frameDetectionVal_nud_ValueChanged);
            // 
            // darkFameDetectionVal_nud
            // 
            this.darkFameDetectionVal_nud.Location = new System.Drawing.Point(139, 46);
            this.darkFameDetectionVal_nud.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.darkFameDetectionVal_nud.Name = "darkFameDetectionVal_nud";
            this.darkFameDetectionVal_nud.Size = new System.Drawing.Size(120, 20);
            this.darkFameDetectionVal_nud.TabIndex = 1;
            this.darkFameDetectionVal_nud.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.darkFameDetectionVal_nud.ValueChanged += new System.EventHandler(this.darkFameDetectionVal_nud_ValueChanged);
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.saveFramesCount_nud);
            this.panel11.Controls.Add(this.label45);
            this.panel11.Controls.Add(this.SaveProcessedImage_cbx);
            this.panel11.Controls.Add(this.SaveDebugImage_cbx);
            this.panel11.Controls.Add(this.saveRaw_cbx);
            this.panel11.Controls.Add(this.SaveIr_cbx);
            this.panel11.Controls.Add(this.showExtViewer_cbx);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel11.Location = new System.Drawing.Point(3, 237);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(301, 66);
            this.panel11.TabIndex = 5;
            // 
            // saveFramesCount_nud
            // 
            this.saveFramesCount_nud.Location = new System.Drawing.Point(109, 43);
            this.saveFramesCount_nud.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.saveFramesCount_nud.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.saveFramesCount_nud.Name = "saveFramesCount_nud";
            this.saveFramesCount_nud.Size = new System.Drawing.Size(76, 20);
            this.saveFramesCount_nud.TabIndex = 4;
            this.saveFramesCount_nud.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(3, 46);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(100, 13);
            this.label45.TabIndex = 3;
            this.label45.Text = "Save Frames Count";
            // 
            // panel14
            // 
            this.panel14.Controls.Add(this.button5);
            this.panel14.Controls.Add(this.label28);
            this.panel14.Controls.Add(this.label27);
            this.panel14.Controls.Add(this.FFA_Color_Pot_Int_Offset_nud);
            this.panel14.Controls.Add(this.FFA_Pot_Int_Offset_nud);
            this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel14.Location = new System.Drawing.Point(3, 3);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(301, 84);
            this.panel14.TabIndex = 6;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(196, 8);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 2;
            this.button5.Text = "button5";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(3, 14);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(100, 13);
            this.label28.TabIndex = 1;
            this.label28.Text = "FFA_Pot_Int_Offset";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(-3, 45);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(99, 13);
            this.label27.TabIndex = 1;
            this.label27.Text = "FFA Int Color Offset";
            // 
            // FFA_Color_Pot_Int_Offset_nud
            // 
            this.FFA_Color_Pot_Int_Offset_nud.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.FFA_Color_Pot_Int_Offset_nud.Location = new System.Drawing.Point(114, 38);
            this.FFA_Color_Pot_Int_Offset_nud.Name = "FFA_Color_Pot_Int_Offset_nud";
            this.FFA_Color_Pot_Int_Offset_nud.Size = new System.Drawing.Size(61, 20);
            this.FFA_Color_Pot_Int_Offset_nud.TabIndex = 3;
            // 
            // FFA_Pot_Int_Offset_nud
            // 
            this.FFA_Pot_Int_Offset_nud.Location = new System.Drawing.Point(109, 12);
            this.FFA_Pot_Int_Offset_nud.Name = "FFA_Pot_Int_Offset_nud";
            this.FFA_Pot_Int_Offset_nud.Size = new System.Drawing.Size(61, 20);
            this.FFA_Pot_Int_Offset_nud.TabIndex = 0;
            this.FFA_Pot_Int_Offset_nud.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.FFA_Pot_Int_Offset_nud.ValueChanged += new System.EventHandler(this.FFA_Pot_Int_Offset_nud_ValueChanged);
            // 
            // HotSpot_tbp
            // 
            this.HotSpot_tbp.Controls.Add(this.formButtons1);
            this.HotSpot_tbp.Controls.Add(this.GetHoSpotParams_btn);
            this.HotSpot_tbp.Controls.Add(this.label66);
            this.HotSpot_tbp.Controls.Add(this.label67);
            this.HotSpot_tbp.Controls.Add(this.shadowRedPeak_nud);
            this.HotSpot_tbp.Controls.Add(this.shadowGreenPeak_nud);
            this.HotSpot_tbp.Controls.Add(this.shadowBluePeak_nud);
            this.HotSpot_tbp.Controls.Add(this.label68);
            this.HotSpot_tbp.Controls.Add(this.HsRedPeak_nud);
            this.HotSpot_tbp.Controls.Add(this.HsGreenPeak_nud);
            this.HotSpot_tbp.Controls.Add(this.HsBluePeak_nud);
            this.HotSpot_tbp.Controls.Add(this.HsRedRadius_nud);
            this.HotSpot_tbp.Controls.Add(this.HsGreenRadius_nud);
            this.HotSpot_tbp.Controls.Add(this.HsBlueRadius_nud);
            this.HotSpot_tbp.Controls.Add(this.label59);
            this.HotSpot_tbp.Controls.Add(this.label61);
            this.HotSpot_tbp.Controls.Add(this.label62);
            this.HotSpot_tbp.Controls.Add(this.label63);
            this.HotSpot_tbp.Controls.Add(this.label64);
            this.HotSpot_tbp.Controls.Add(this.label65);
            this.HotSpot_tbp.Controls.Add(this.coOrdinates_lbl);
            this.HotSpot_tbp.Controls.Add(this.twoX_zoom_rb);
            this.HotSpot_tbp.Controls.Add(this.oneX_zoom_rb);
            this.HotSpot_tbp.Controls.Add(this.label2);
            this.HotSpot_tbp.Controls.Add(this.label1);
            this.HotSpot_tbp.Controls.Add(this.label15);
            this.HotSpot_tbp.Controls.Add(this.label17);
            this.HotSpot_tbp.Controls.Add(this.HotSpotCentreY_nud);
            this.HotSpot_tbp.Controls.Add(this.HotSpotCentreX_nud);
            this.HotSpot_tbp.Controls.Add(this.button2);
            this.HotSpot_tbp.Controls.Add(this.button1);
            this.HotSpot_tbp.Controls.Add(this.applyHotSpotCorrection_btn);
            this.HotSpot_tbp.Controls.Add(this.label18);
            this.HotSpot_tbp.Controls.Add(this.hotSpotValues_btn);
            this.HotSpot_tbp.Controls.Add(this.centreValues_btn);
            this.HotSpot_tbp.Controls.Add(this.label30);
            this.HotSpot_tbp.Controls.Add(this.HSRad2_nud);
            this.HotSpot_tbp.Controls.Add(this.label31);
            this.HotSpot_tbp.Controls.Add(this.HSRad1_nud);
            this.HotSpot_tbp.Controls.Add(this.label16);
            this.HotSpot_tbp.Controls.Add(this.label14);
            this.HotSpot_tbp.Controls.Add(this.label37);
            this.HotSpot_tbp.Controls.Add(this.ShadowRad2_nud);
            this.HotSpot_tbp.Controls.Add(this.label25);
            this.HotSpot_tbp.Controls.Add(this.ShadowRad1_nud);
            this.HotSpot_tbp.Location = new System.Drawing.Point(4, 40);
            this.HotSpot_tbp.Name = "HotSpot_tbp";
            this.HotSpot_tbp.Padding = new System.Windows.Forms.Padding(3);
            this.HotSpot_tbp.Size = new System.Drawing.Size(313, 656);
            this.HotSpot_tbp.TabIndex = 11;
            this.HotSpot_tbp.Text = "HotSpot";
            this.HotSpot_tbp.UseVisualStyleBackColor = true;
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label66.Location = new System.Drawing.Point(5, 327);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(84, 13);
            this.label66.TabIndex = 149;
            this.label66.Text = "Red Peak %age";
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label67.Location = new System.Drawing.Point(5, 369);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(93, 13);
            this.label67.TabIndex = 150;
            this.label67.Text = "Green Peak %age";
            // 
            // shadowRedPeak_nud
            // 
            this.shadowRedPeak_nud.Location = new System.Drawing.Point(110, 325);
            this.shadowRedPeak_nud.Name = "shadowRedPeak_nud";
            this.shadowRedPeak_nud.Size = new System.Drawing.Size(51, 20);
            this.shadowRedPeak_nud.TabIndex = 152;
            this.shadowRedPeak_nud.Value = new decimal(new int[] {
            22,
            0,
            0,
            0});
            this.shadowRedPeak_nud.ValueChanged += new System.EventHandler(this.shadowRedPeak_nud_ValueChanged);
            // 
            // shadowGreenPeak_nud
            // 
            this.shadowGreenPeak_nud.Location = new System.Drawing.Point(110, 362);
            this.shadowGreenPeak_nud.Name = "shadowGreenPeak_nud";
            this.shadowGreenPeak_nud.Size = new System.Drawing.Size(51, 20);
            this.shadowGreenPeak_nud.TabIndex = 153;
            this.shadowGreenPeak_nud.Value = new decimal(new int[] {
            22,
            0,
            0,
            0});
            this.shadowGreenPeak_nud.ValueChanged += new System.EventHandler(this.shadowGreenPeak_nud_ValueChanged);
            // 
            // shadowBluePeak_nud
            // 
            this.shadowBluePeak_nud.Location = new System.Drawing.Point(110, 409);
            this.shadowBluePeak_nud.Name = "shadowBluePeak_nud";
            this.shadowBluePeak_nud.Size = new System.Drawing.Size(51, 20);
            this.shadowBluePeak_nud.TabIndex = 154;
            this.shadowBluePeak_nud.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.shadowBluePeak_nud.ValueChanged += new System.EventHandler(this.shadowBluePeak_nud_ValueChanged);
            // 
            // label68
            // 
            this.label68.AutoSize = true;
            this.label68.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label68.Location = new System.Drawing.Point(6, 411);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(85, 13);
            this.label68.TabIndex = 151;
            this.label68.Text = "Blue Peak %age";
            // 
            // HsRedPeak_nud
            // 
            this.HsRedPeak_nud.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HsRedPeak_nud.Location = new System.Drawing.Point(71, 109);
            this.HsRedPeak_nud.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.HsRedPeak_nud.Minimum = new decimal(new int[] {
            255,
            0,
            0,
            -2147483648});
            this.HsRedPeak_nud.Name = "HsRedPeak_nud";
            this.HsRedPeak_nud.Size = new System.Drawing.Size(38, 20);
            this.HsRedPeak_nud.TabIndex = 137;
            this.HsRedPeak_nud.ValueChanged += new System.EventHandler(this.HsRedPeak_nud_ValueChanged);
            // 
            // HsGreenPeak_nud
            // 
            this.HsGreenPeak_nud.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HsGreenPeak_nud.Location = new System.Drawing.Point(71, 137);
            this.HsGreenPeak_nud.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.HsGreenPeak_nud.Minimum = new decimal(new int[] {
            255,
            0,
            0,
            -2147483648});
            this.HsGreenPeak_nud.Name = "HsGreenPeak_nud";
            this.HsGreenPeak_nud.Size = new System.Drawing.Size(35, 20);
            this.HsGreenPeak_nud.TabIndex = 138;
            this.HsGreenPeak_nud.ValueChanged += new System.EventHandler(this.HsGreenPeak_nud_ValueChanged);
            // 
            // HsBluePeak_nud
            // 
            this.HsBluePeak_nud.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HsBluePeak_nud.Location = new System.Drawing.Point(71, 167);
            this.HsBluePeak_nud.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.HsBluePeak_nud.Minimum = new decimal(new int[] {
            255,
            0,
            0,
            -2147483648});
            this.HsBluePeak_nud.Name = "HsBluePeak_nud";
            this.HsBluePeak_nud.Size = new System.Drawing.Size(40, 20);
            this.HsBluePeak_nud.TabIndex = 139;
            this.HsBluePeak_nud.ValueChanged += new System.EventHandler(this.HsBluePeak_nud_ValueChanged);
            // 
            // HsRedRadius_nud
            // 
            this.HsRedRadius_nud.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HsRedRadius_nud.Location = new System.Drawing.Point(203, 109);
            this.HsRedRadius_nud.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.HsRedRadius_nud.Name = "HsRedRadius_nud";
            this.HsRedRadius_nud.Size = new System.Drawing.Size(50, 20);
            this.HsRedRadius_nud.TabIndex = 140;
            this.HsRedRadius_nud.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.HsRedRadius_nud.ValueChanged += new System.EventHandler(this.HsRedRadius_nud_ValueChanged);
            // 
            // HsGreenRadius_nud
            // 
            this.HsGreenRadius_nud.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HsGreenRadius_nud.Location = new System.Drawing.Point(203, 137);
            this.HsGreenRadius_nud.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.HsGreenRadius_nud.Name = "HsGreenRadius_nud";
            this.HsGreenRadius_nud.Size = new System.Drawing.Size(50, 20);
            this.HsGreenRadius_nud.TabIndex = 141;
            this.HsGreenRadius_nud.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.HsGreenRadius_nud.ValueChanged += new System.EventHandler(this.HsGreenRadius_nud_ValueChanged);
            // 
            // HsBlueRadius_nud
            // 
            this.HsBlueRadius_nud.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HsBlueRadius_nud.Location = new System.Drawing.Point(203, 167);
            this.HsBlueRadius_nud.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.HsBlueRadius_nud.Name = "HsBlueRadius_nud";
            this.HsBlueRadius_nud.Size = new System.Drawing.Size(50, 20);
            this.HsBlueRadius_nud.TabIndex = 142;
            this.HsBlueRadius_nud.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.HsBlueRadius_nud.ValueChanged += new System.EventHandler(this.HsBlueRadius_nud_ValueChanged);
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label59.Location = new System.Drawing.Point(6, 111);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(55, 13);
            this.label59.TabIndex = 143;
            this.label59.Text = "Red Peak";
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label61.Location = new System.Drawing.Point(4, 139);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(64, 13);
            this.label61.TabIndex = 144;
            this.label61.Text = " GreenPeak";
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label62.Location = new System.Drawing.Point(7, 167);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(56, 13);
            this.label62.TabIndex = 145;
            this.label62.Text = "Blue Peak";
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label63.Location = new System.Drawing.Point(121, 109);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(66, 13);
            this.label63.TabIndex = 146;
            this.label63.Text = " Red Radius";
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label64.Location = new System.Drawing.Point(125, 137);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(72, 13);
            this.label64.TabIndex = 147;
            this.label64.Text = "Green Radius";
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label65.Location = new System.Drawing.Point(125, 169);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(67, 13);
            this.label65.TabIndex = 148;
            this.label65.Text = " Blue Radius";
            // 
            // coOrdinates_lbl
            // 
            this.coOrdinates_lbl.AutoSize = true;
            this.coOrdinates_lbl.Location = new System.Drawing.Point(200, 52);
            this.coOrdinates_lbl.Name = "coOrdinates_lbl";
            this.coOrdinates_lbl.Size = new System.Drawing.Size(13, 13);
            this.coOrdinates_lbl.TabIndex = 136;
            this.coOrdinates_lbl.Text = "0";
            // 
            // twoX_zoom_rb
            // 
            this.twoX_zoom_rb.AutoSize = true;
            this.twoX_zoom_rb.Location = new System.Drawing.Point(257, 17);
            this.twoX_zoom_rb.Name = "twoX_zoom_rb";
            this.twoX_zoom_rb.Size = new System.Drawing.Size(36, 17);
            this.twoX_zoom_rb.TabIndex = 134;
            this.twoX_zoom_rb.Text = "2x";
            this.twoX_zoom_rb.UseVisualStyleBackColor = true;
            // 
            // oneX_zoom_rb
            // 
            this.oneX_zoom_rb.AutoSize = true;
            this.oneX_zoom_rb.Checked = true;
            this.oneX_zoom_rb.Location = new System.Drawing.Point(203, 17);
            this.oneX_zoom_rb.Name = "oneX_zoom_rb";
            this.oneX_zoom_rb.Size = new System.Drawing.Size(36, 17);
            this.oneX_zoom_rb.TabIndex = 135;
            this.oneX_zoom_rb.TabStop = true;
            this.oneX_zoom_rb.Text = "1x";
            this.oneX_zoom_rb.UseVisualStyleBackColor = true;
            this.oneX_zoom_rb.CheckedChanged += new System.EventHandler(this.oneX_zoom_rb_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(153, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 133;
            this.label2.Text = "Centre";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(153, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 133;
            this.label1.Text = "Zoom";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(3, 54);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(48, 13);
            this.label15.TabIndex = 133;
            this.label15.Text = "Centre Y";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(3, 21);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(48, 13);
            this.label17.TabIndex = 132;
            this.label17.Text = "Centre X";
            // 
            // HotSpotCentreY_nud
            // 
            this.HotSpotCentreY_nud.Location = new System.Drawing.Point(63, 52);
            this.HotSpotCentreY_nud.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.HotSpotCentreY_nud.Name = "HotSpotCentreY_nud";
            this.HotSpotCentreY_nud.Size = new System.Drawing.Size(68, 20);
            this.HotSpotCentreY_nud.TabIndex = 130;
            this.HotSpotCentreY_nud.Value = new decimal(new int[] {
            768,
            0,
            0,
            0});
            this.HotSpotCentreY_nud.ValueChanged += new System.EventHandler(this.HotSpotCentreY_nud_ValueChanged);
            // 
            // HotSpotCentreX_nud
            // 
            this.HotSpotCentreX_nud.Location = new System.Drawing.Point(58, 19);
            this.HotSpotCentreX_nud.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.HotSpotCentreX_nud.Name = "HotSpotCentreX_nud";
            this.HotSpotCentreX_nud.Size = new System.Drawing.Size(68, 20);
            this.HotSpotCentreX_nud.TabIndex = 131;
            this.HotSpotCentreX_nud.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.HotSpotCentreX_nud.ValueChanged += new System.EventHandler(this.HotSpotCentreX_nud_ValueChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(7, 504);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(130, 33);
            this.button2.TabIndex = 129;
            this.button2.Text = "Show Radius Marking";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_2);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(156, 504);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 33);
            this.button1.TabIndex = 129;
            this.button1.Text = "Clear Radius Marking";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.ClearHotSpotMarking_btn_Click);
            // 
            // applyHotSpotCorrection_btn
            // 
            this.applyHotSpotCorrection_btn.Location = new System.Drawing.Point(53, 553);
            this.applyHotSpotCorrection_btn.Name = "applyHotSpotCorrection_btn";
            this.applyHotSpotCorrection_btn.Size = new System.Drawing.Size(176, 29);
            this.applyHotSpotCorrection_btn.TabIndex = 129;
            this.applyHotSpotCorrection_btn.Text = "Apply HS Correction";
            this.applyHotSpotCorrection_btn.UseVisualStyleBackColor = true;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(12, 3);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(119, 13);
            this.label18.TabIndex = 128;
            this.label18.Text = "HotSpot parameters";
            // 
            // hotSpotValues_btn
            // 
            this.hotSpotValues_btn.Location = new System.Drawing.Point(124, 447);
            this.hotSpotValues_btn.Name = "hotSpotValues_btn";
            this.hotSpotValues_btn.Size = new System.Drawing.Size(78, 34);
            this.hotSpotValues_btn.TabIndex = 126;
            this.hotSpotValues_btn.Text = "Hotspot Values";
            this.hotSpotValues_btn.UseVisualStyleBackColor = true;
            this.hotSpotValues_btn.Click += new System.EventHandler(this.hotSpotValues_btn_Click);
            // 
            // centreValues_btn
            // 
            this.centreValues_btn.Location = new System.Drawing.Point(8, 447);
            this.centreValues_btn.Name = "centreValues_btn";
            this.centreValues_btn.Size = new System.Drawing.Size(78, 34);
            this.centreValues_btn.TabIndex = 126;
            this.centreValues_btn.Text = "Centre Values";
            this.centreValues_btn.UseVisualStyleBackColor = true;
            this.centreValues_btn.Click += new System.EventHandler(this.centreValues_btn_Click);
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(180, 241);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(49, 13);
            this.label30.TabIndex = 125;
            this.label30.Text = "Radius 1";
            // 
            // HSRad2_nud
            // 
            this.HSRad2_nud.Location = new System.Drawing.Point(236, 239);
            this.HSRad2_nud.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.HSRad2_nud.Name = "HSRad2_nud";
            this.HSRad2_nud.Size = new System.Drawing.Size(57, 20);
            this.HSRad2_nud.TabIndex = 124;
            this.HSRad2_nud.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.HSRad2_nud.ValueChanged += new System.EventHandler(this.HSRad2_nud_ValueChanged);
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(10, 241);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(54, 13);
            this.label31.TabIndex = 123;
            this.label31.Text = "Inner Rad";
            // 
            // HSRad1_nud
            // 
            this.HSRad1_nud.Location = new System.Drawing.Point(77, 241);
            this.HSRad1_nud.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.HSRad1_nud.Minimum = new decimal(new int[] {
            2048,
            0,
            0,
            -2147483648});
            this.HSRad1_nud.Name = "HSRad1_nud";
            this.HSRad1_nud.Size = new System.Drawing.Size(58, 20);
            this.HSRad1_nud.TabIndex = 122;
            this.HSRad1_nud.Value = new decimal(new int[] {
            50,
            0,
            0,
            -2147483648});
            this.HSRad1_nud.ValueChanged += new System.EventHandler(this.HSRad1_nud_ValueChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(7, 208);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(115, 16);
            this.label16.TabIndex = 119;
            this.label16.Text = "Shadow Values";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(27, 80);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(120, 16);
            this.label14.TabIndex = 119;
            this.label14.Text = "Hot Spot Values";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(10, 290);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(56, 13);
            this.label37.TabIndex = 113;
            this.label37.Text = "Outer Rad";
            // 
            // ShadowRad2_nud
            // 
            this.ShadowRad2_nud.Location = new System.Drawing.Point(77, 288);
            this.ShadowRad2_nud.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.ShadowRad2_nud.Name = "ShadowRad2_nud";
            this.ShadowRad2_nud.Size = new System.Drawing.Size(58, 20);
            this.ShadowRad2_nud.TabIndex = 112;
            this.ShadowRad2_nud.Value = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.ShadowRad2_nud.ValueChanged += new System.EventHandler(this.ShadowRad2_nud_ValueChanged);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(181, 290);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(49, 13);
            this.label25.TabIndex = 113;
            this.label25.Text = "Radius 2";
            // 
            // ShadowRad1_nud
            // 
            this.ShadowRad1_nud.Location = new System.Drawing.Point(236, 288);
            this.ShadowRad1_nud.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.ShadowRad1_nud.Name = "ShadowRad1_nud";
            this.ShadowRad1_nud.Size = new System.Drawing.Size(55, 20);
            this.ShadowRad1_nud.TabIndex = 112;
            this.ShadowRad1_nud.Value = new decimal(new int[] {
            170,
            0,
            0,
            0});
            this.ShadowRad1_nud.ValueChanged += new System.EventHandler(this.ShadowRad1_nud_ValueChanged);
            // 
            // allChannelLut_tbp
            // 
            this.allChannelLut_tbp.Controls.Add(this.channnelsTbpl);
            this.allChannelLut_tbp.Location = new System.Drawing.Point(4, 40);
            this.allChannelLut_tbp.Name = "allChannelLut_tbp";
            this.allChannelLut_tbp.Padding = new System.Windows.Forms.Padding(3);
            this.allChannelLut_tbp.Size = new System.Drawing.Size(313, 656);
            this.allChannelLut_tbp.TabIndex = 12;
            this.allChannelLut_tbp.Text = "AllChannel LUT";
            this.allChannelLut_tbp.UseVisualStyleBackColor = true;
            // 
            // channnelsTbpl
            // 
            this.channnelsTbpl.ColumnCount = 1;
            this.channnelsTbpl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.channnelsTbpl.Controls.Add(this.redChannelTblPnl, 0, 0);
            this.channnelsTbpl.Controls.Add(this.greenChannelTbpl, 0, 1);
            this.channnelsTbpl.Controls.Add(this.blueChannelTbpl, 0, 2);
            this.channnelsTbpl.Controls.Add(this.showLutTbpl, 0, 3);
            this.channnelsTbpl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.channnelsTbpl.Location = new System.Drawing.Point(3, 3);
            this.channnelsTbpl.Name = "channnelsTbpl";
            this.channnelsTbpl.RowCount = 4;
            this.channnelsTbpl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.channnelsTbpl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.channnelsTbpl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.channnelsTbpl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.channnelsTbpl.Size = new System.Drawing.Size(307, 650);
            this.channnelsTbpl.TabIndex = 0;
            // 
            // redChannelTblPnl
            // 
            this.redChannelTblPnl.ColumnCount = 1;
            this.redChannelTblPnl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.redChannelTblPnl.Controls.Add(this.redKutSettingTbpl, 0, 1);
            this.redChannelTblPnl.Controls.Add(this.redChannelLutLbl, 0, 0);
            this.redChannelTblPnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.redChannelTblPnl.Location = new System.Drawing.Point(3, 3);
            this.redChannelTblPnl.Name = "redChannelTblPnl";
            this.redChannelTblPnl.RowCount = 2;
            this.redChannelTblPnl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.35599F));
            this.redChannelTblPnl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 89.64401F));
            this.redChannelTblPnl.Size = new System.Drawing.Size(301, 189);
            this.redChannelTblPnl.TabIndex = 0;
            // 
            // redKutSettingTbpl
            // 
            this.redKutSettingTbpl.ColumnCount = 2;
            this.redKutSettingTbpl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.redKutSettingTbpl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.redKutSettingTbpl.Controls.Add(this.sineFactorRedChannelLbl, 0, 0);
            this.redKutSettingTbpl.Controls.Add(this.interval1RedChannelLbl, 0, 1);
            this.redKutSettingTbpl.Controls.Add(this.Interval2RedChannelLbl, 0, 2);
            this.redKutSettingTbpl.Controls.Add(this.lutOffsetRedChannelLbl, 0, 3);
            this.redKutSettingTbpl.Controls.Add(this.redChannelSineFactor_nud, 1, 0);
            this.redKutSettingTbpl.Controls.Add(this.redChannelLutInterval1_nud, 1, 1);
            this.redKutSettingTbpl.Controls.Add(this.redChannelLutInterval2_nud, 1, 2);
            this.redKutSettingTbpl.Controls.Add(this.redChannelLutOffset_nud, 1, 3);
            this.redKutSettingTbpl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.redKutSettingTbpl.Location = new System.Drawing.Point(3, 22);
            this.redKutSettingTbpl.Name = "redKutSettingTbpl";
            this.redKutSettingTbpl.RowCount = 4;
            this.redKutSettingTbpl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.redKutSettingTbpl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.redKutSettingTbpl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.redKutSettingTbpl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.redKutSettingTbpl.Size = new System.Drawing.Size(295, 164);
            this.redKutSettingTbpl.TabIndex = 0;
            // 
            // sineFactorRedChannelLbl
            // 
            this.sineFactorRedChannelLbl.AutoSize = true;
            this.sineFactorRedChannelLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sineFactorRedChannelLbl.Location = new System.Drawing.Point(3, 0);
            this.sineFactorRedChannelLbl.Name = "sineFactorRedChannelLbl";
            this.sineFactorRedChannelLbl.Size = new System.Drawing.Size(97, 41);
            this.sineFactorRedChannelLbl.TabIndex = 0;
            this.sineFactorRedChannelLbl.Text = "Sine Factor";
            this.sineFactorRedChannelLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // interval1RedChannelLbl
            // 
            this.interval1RedChannelLbl.AutoSize = true;
            this.interval1RedChannelLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.interval1RedChannelLbl.Location = new System.Drawing.Point(3, 41);
            this.interval1RedChannelLbl.Name = "interval1RedChannelLbl";
            this.interval1RedChannelLbl.Size = new System.Drawing.Size(97, 41);
            this.interval1RedChannelLbl.TabIndex = 0;
            this.interval1RedChannelLbl.Text = "LUT Interval 1";
            this.interval1RedChannelLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Interval2RedChannelLbl
            // 
            this.Interval2RedChannelLbl.AutoSize = true;
            this.Interval2RedChannelLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Interval2RedChannelLbl.Location = new System.Drawing.Point(3, 82);
            this.Interval2RedChannelLbl.Name = "Interval2RedChannelLbl";
            this.Interval2RedChannelLbl.Size = new System.Drawing.Size(97, 41);
            this.Interval2RedChannelLbl.TabIndex = 0;
            this.Interval2RedChannelLbl.Text = "LUT Interval 2";
            this.Interval2RedChannelLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lutOffsetRedChannelLbl
            // 
            this.lutOffsetRedChannelLbl.AutoSize = true;
            this.lutOffsetRedChannelLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lutOffsetRedChannelLbl.Location = new System.Drawing.Point(3, 123);
            this.lutOffsetRedChannelLbl.Name = "lutOffsetRedChannelLbl";
            this.lutOffsetRedChannelLbl.Size = new System.Drawing.Size(97, 41);
            this.lutOffsetRedChannelLbl.TabIndex = 0;
            this.lutOffsetRedChannelLbl.Text = "Offset Value";
            this.lutOffsetRedChannelLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // redChannelSineFactor_nud
            // 
            this.redChannelSineFactor_nud.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.redChannelSineFactor_nud.Location = new System.Drawing.Point(113, 10);
            this.redChannelSineFactor_nud.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.redChannelSineFactor_nud.Name = "redChannelSineFactor_nud";
            this.redChannelSineFactor_nud.Size = new System.Drawing.Size(172, 20);
            this.redChannelSineFactor_nud.TabIndex = 1;
            this.redChannelSineFactor_nud.ValueChanged += new System.EventHandler(this.redChannelSineFactor_nud_ValueChanged);
            // 
            // redChannelLutInterval1_nud
            // 
            this.redChannelLutInterval1_nud.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.redChannelLutInterval1_nud.Location = new System.Drawing.Point(113, 51);
            this.redChannelLutInterval1_nud.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.redChannelLutInterval1_nud.Name = "redChannelLutInterval1_nud";
            this.redChannelLutInterval1_nud.Size = new System.Drawing.Size(172, 20);
            this.redChannelLutInterval1_nud.TabIndex = 1;
            this.redChannelLutInterval1_nud.ValueChanged += new System.EventHandler(this.redChannelLutInterval1_nud_ValueChanged);
            // 
            // redChannelLutInterval2_nud
            // 
            this.redChannelLutInterval2_nud.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.redChannelLutInterval2_nud.Location = new System.Drawing.Point(113, 92);
            this.redChannelLutInterval2_nud.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.redChannelLutInterval2_nud.Name = "redChannelLutInterval2_nud";
            this.redChannelLutInterval2_nud.Size = new System.Drawing.Size(172, 20);
            this.redChannelLutInterval2_nud.TabIndex = 1;
            this.redChannelLutInterval2_nud.ValueChanged += new System.EventHandler(this.redChannelLutInterval2_nud_ValueChanged);
            // 
            // redChannelLutOffset_nud
            // 
            this.redChannelLutOffset_nud.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.redChannelLutOffset_nud.Location = new System.Drawing.Point(113, 133);
            this.redChannelLutOffset_nud.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.redChannelLutOffset_nud.Name = "redChannelLutOffset_nud";
            this.redChannelLutOffset_nud.Size = new System.Drawing.Size(172, 20);
            this.redChannelLutOffset_nud.TabIndex = 1;
            this.redChannelLutOffset_nud.ValueChanged += new System.EventHandler(this.redChannelLutOffset_nud_ValueChanged);
            // 
            // redChannelLutLbl
            // 
            this.redChannelLutLbl.AutoSize = true;
            this.redChannelLutLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.redChannelLutLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.redChannelLutLbl.Location = new System.Drawing.Point(3, 0);
            this.redChannelLutLbl.Name = "redChannelLutLbl";
            this.redChannelLutLbl.Size = new System.Drawing.Size(295, 19);
            this.redChannelLutLbl.TabIndex = 1;
            this.redChannelLutLbl.Text = "RedChannel";
            this.redChannelLutLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // greenChannelTbpl
            // 
            this.greenChannelTbpl.ColumnCount = 1;
            this.greenChannelTbpl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.greenChannelTbpl.Controls.Add(this.greenLutSettingsTbpl, 0, 1);
            this.greenChannelTbpl.Controls.Add(this.greenChannelLutLbl, 0, 0);
            this.greenChannelTbpl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.greenChannelTbpl.Location = new System.Drawing.Point(3, 198);
            this.greenChannelTbpl.Name = "greenChannelTbpl";
            this.greenChannelTbpl.RowCount = 2;
            this.greenChannelTbpl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.greenChannelTbpl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.greenChannelTbpl.Size = new System.Drawing.Size(301, 189);
            this.greenChannelTbpl.TabIndex = 1;
            // 
            // greenLutSettingsTbpl
            // 
            this.greenLutSettingsTbpl.ColumnCount = 2;
            this.greenLutSettingsTbpl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.greenLutSettingsTbpl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.greenLutSettingsTbpl.Controls.Add(this.greenChannelSineFactorLbl, 0, 0);
            this.greenLutSettingsTbpl.Controls.Add(this.interval1GreenChannelLbl, 0, 1);
            this.greenLutSettingsTbpl.Controls.Add(this.interval2GreenChannelLbl, 0, 2);
            this.greenLutSettingsTbpl.Controls.Add(this.lutOffsetGreenChannelLbl, 0, 3);
            this.greenLutSettingsTbpl.Controls.Add(this.greenChannelSineFactor_nud, 1, 0);
            this.greenLutSettingsTbpl.Controls.Add(this.greenChannelLutInterval1_nud, 1, 1);
            this.greenLutSettingsTbpl.Controls.Add(this.greenChannelLutInterval2_nud, 1, 2);
            this.greenLutSettingsTbpl.Controls.Add(this.greenChannelLutOffset_nud, 1, 3);
            this.greenLutSettingsTbpl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.greenLutSettingsTbpl.Location = new System.Drawing.Point(3, 21);
            this.greenLutSettingsTbpl.Name = "greenLutSettingsTbpl";
            this.greenLutSettingsTbpl.RowCount = 4;
            this.greenLutSettingsTbpl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.greenLutSettingsTbpl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.greenLutSettingsTbpl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.greenLutSettingsTbpl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.greenLutSettingsTbpl.Size = new System.Drawing.Size(295, 165);
            this.greenLutSettingsTbpl.TabIndex = 0;
            // 
            // greenChannelSineFactorLbl
            // 
            this.greenChannelSineFactorLbl.AutoSize = true;
            this.greenChannelSineFactorLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.greenChannelSineFactorLbl.Location = new System.Drawing.Point(3, 0);
            this.greenChannelSineFactorLbl.Name = "greenChannelSineFactorLbl";
            this.greenChannelSineFactorLbl.Size = new System.Drawing.Size(97, 41);
            this.greenChannelSineFactorLbl.TabIndex = 0;
            this.greenChannelSineFactorLbl.Text = "Sine Factor";
            this.greenChannelSineFactorLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // interval1GreenChannelLbl
            // 
            this.interval1GreenChannelLbl.AutoSize = true;
            this.interval1GreenChannelLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.interval1GreenChannelLbl.Location = new System.Drawing.Point(3, 41);
            this.interval1GreenChannelLbl.Name = "interval1GreenChannelLbl";
            this.interval1GreenChannelLbl.Size = new System.Drawing.Size(97, 41);
            this.interval1GreenChannelLbl.TabIndex = 0;
            this.interval1GreenChannelLbl.Text = "LUT Interval 1";
            this.interval1GreenChannelLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // interval2GreenChannelLbl
            // 
            this.interval2GreenChannelLbl.AutoSize = true;
            this.interval2GreenChannelLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.interval2GreenChannelLbl.Location = new System.Drawing.Point(3, 82);
            this.interval2GreenChannelLbl.Name = "interval2GreenChannelLbl";
            this.interval2GreenChannelLbl.Size = new System.Drawing.Size(97, 41);
            this.interval2GreenChannelLbl.TabIndex = 0;
            this.interval2GreenChannelLbl.Text = "LUT Interval 2";
            this.interval2GreenChannelLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lutOffsetGreenChannelLbl
            // 
            this.lutOffsetGreenChannelLbl.AutoSize = true;
            this.lutOffsetGreenChannelLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lutOffsetGreenChannelLbl.Location = new System.Drawing.Point(3, 123);
            this.lutOffsetGreenChannelLbl.Name = "lutOffsetGreenChannelLbl";
            this.lutOffsetGreenChannelLbl.Size = new System.Drawing.Size(97, 42);
            this.lutOffsetGreenChannelLbl.TabIndex = 0;
            this.lutOffsetGreenChannelLbl.Text = "Offset Value";
            this.lutOffsetGreenChannelLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // greenChannelSineFactor_nud
            // 
            this.greenChannelSineFactor_nud.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.greenChannelSineFactor_nud.Location = new System.Drawing.Point(113, 10);
            this.greenChannelSineFactor_nud.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.greenChannelSineFactor_nud.Name = "greenChannelSineFactor_nud";
            this.greenChannelSineFactor_nud.Size = new System.Drawing.Size(172, 20);
            this.greenChannelSineFactor_nud.TabIndex = 1;
            this.greenChannelSineFactor_nud.ValueChanged += new System.EventHandler(this.greenChannelSineFactor_nud_ValueChanged);
            // 
            // greenChannelLutInterval1_nud
            // 
            this.greenChannelLutInterval1_nud.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.greenChannelLutInterval1_nud.Location = new System.Drawing.Point(113, 51);
            this.greenChannelLutInterval1_nud.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.greenChannelLutInterval1_nud.Name = "greenChannelLutInterval1_nud";
            this.greenChannelLutInterval1_nud.Size = new System.Drawing.Size(172, 20);
            this.greenChannelLutInterval1_nud.TabIndex = 1;
            this.greenChannelLutInterval1_nud.ValueChanged += new System.EventHandler(this.greenChannelLutInterval1_nud_ValueChanged);
            // 
            // greenChannelLutInterval2_nud
            // 
            this.greenChannelLutInterval2_nud.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.greenChannelLutInterval2_nud.Location = new System.Drawing.Point(113, 92);
            this.greenChannelLutInterval2_nud.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.greenChannelLutInterval2_nud.Name = "greenChannelLutInterval2_nud";
            this.greenChannelLutInterval2_nud.Size = new System.Drawing.Size(172, 20);
            this.greenChannelLutInterval2_nud.TabIndex = 1;
            this.greenChannelLutInterval2_nud.ValueChanged += new System.EventHandler(this.greenChannelLutInterval2_nud_ValueChanged);
            // 
            // greenChannelLutOffset_nud
            // 
            this.greenChannelLutOffset_nud.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.greenChannelLutOffset_nud.Location = new System.Drawing.Point(113, 134);
            this.greenChannelLutOffset_nud.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.greenChannelLutOffset_nud.Name = "greenChannelLutOffset_nud";
            this.greenChannelLutOffset_nud.Size = new System.Drawing.Size(172, 20);
            this.greenChannelLutOffset_nud.TabIndex = 1;
            this.greenChannelLutOffset_nud.ValueChanged += new System.EventHandler(this.greenChannelLutOffset_nud_ValueChanged);
            // 
            // greenChannelLutLbl
            // 
            this.greenChannelLutLbl.AutoSize = true;
            this.greenChannelLutLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.greenChannelLutLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.greenChannelLutLbl.Location = new System.Drawing.Point(3, 0);
            this.greenChannelLutLbl.Name = "greenChannelLutLbl";
            this.greenChannelLutLbl.Size = new System.Drawing.Size(295, 18);
            this.greenChannelLutLbl.TabIndex = 1;
            this.greenChannelLutLbl.Text = "Green Channel";
            this.greenChannelLutLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // blueChannelTbpl
            // 
            this.blueChannelTbpl.ColumnCount = 1;
            this.blueChannelTbpl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.blueChannelTbpl.Controls.Add(this.blueLutSettingsTbpl, 0, 1);
            this.blueChannelTbpl.Controls.Add(this.blueChannelLutLbl, 0, 0);
            this.blueChannelTbpl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blueChannelTbpl.Location = new System.Drawing.Point(3, 393);
            this.blueChannelTbpl.Name = "blueChannelTbpl";
            this.blueChannelTbpl.RowCount = 2;
            this.blueChannelTbpl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.blueChannelTbpl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.blueChannelTbpl.Size = new System.Drawing.Size(301, 189);
            this.blueChannelTbpl.TabIndex = 2;
            // 
            // blueLutSettingsTbpl
            // 
            this.blueLutSettingsTbpl.ColumnCount = 2;
            this.blueLutSettingsTbpl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.blueLutSettingsTbpl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.blueLutSettingsTbpl.Controls.Add(this.blueChannelSineFactorLbl, 0, 0);
            this.blueLutSettingsTbpl.Controls.Add(this.interval1BlueChannelLbl, 0, 1);
            this.blueLutSettingsTbpl.Controls.Add(this.interval2BlueChannelLbl, 0, 2);
            this.blueLutSettingsTbpl.Controls.Add(this.lutOffsetBlueChannelLbl, 0, 3);
            this.blueLutSettingsTbpl.Controls.Add(this.blueChannelSineFactor_nud, 1, 0);
            this.blueLutSettingsTbpl.Controls.Add(this.blueChannelLutInterval1_nud, 1, 1);
            this.blueLutSettingsTbpl.Controls.Add(this.blueChannelLutInterval2_nud, 1, 2);
            this.blueLutSettingsTbpl.Controls.Add(this.blueChannelLutOffset_nud, 1, 3);
            this.blueLutSettingsTbpl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blueLutSettingsTbpl.Location = new System.Drawing.Point(3, 21);
            this.blueLutSettingsTbpl.Name = "blueLutSettingsTbpl";
            this.blueLutSettingsTbpl.RowCount = 4;
            this.blueLutSettingsTbpl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.blueLutSettingsTbpl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.blueLutSettingsTbpl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.blueLutSettingsTbpl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.blueLutSettingsTbpl.Size = new System.Drawing.Size(295, 165);
            this.blueLutSettingsTbpl.TabIndex = 0;
            // 
            // blueChannelSineFactorLbl
            // 
            this.blueChannelSineFactorLbl.AutoSize = true;
            this.blueChannelSineFactorLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blueChannelSineFactorLbl.Location = new System.Drawing.Point(3, 0);
            this.blueChannelSineFactorLbl.Name = "blueChannelSineFactorLbl";
            this.blueChannelSineFactorLbl.Size = new System.Drawing.Size(97, 41);
            this.blueChannelSineFactorLbl.TabIndex = 0;
            this.blueChannelSineFactorLbl.Text = "Sine Factor";
            this.blueChannelSineFactorLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // interval1BlueChannelLbl
            // 
            this.interval1BlueChannelLbl.AutoSize = true;
            this.interval1BlueChannelLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.interval1BlueChannelLbl.Location = new System.Drawing.Point(3, 41);
            this.interval1BlueChannelLbl.Name = "interval1BlueChannelLbl";
            this.interval1BlueChannelLbl.Size = new System.Drawing.Size(97, 41);
            this.interval1BlueChannelLbl.TabIndex = 0;
            this.interval1BlueChannelLbl.Text = "LUT Interval 1";
            this.interval1BlueChannelLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // interval2BlueChannelLbl
            // 
            this.interval2BlueChannelLbl.AutoSize = true;
            this.interval2BlueChannelLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.interval2BlueChannelLbl.Location = new System.Drawing.Point(3, 82);
            this.interval2BlueChannelLbl.Name = "interval2BlueChannelLbl";
            this.interval2BlueChannelLbl.Size = new System.Drawing.Size(97, 41);
            this.interval2BlueChannelLbl.TabIndex = 0;
            this.interval2BlueChannelLbl.Text = "LUT Interval 2";
            this.interval2BlueChannelLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lutOffsetBlueChannelLbl
            // 
            this.lutOffsetBlueChannelLbl.AutoSize = true;
            this.lutOffsetBlueChannelLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lutOffsetBlueChannelLbl.Location = new System.Drawing.Point(3, 123);
            this.lutOffsetBlueChannelLbl.Name = "lutOffsetBlueChannelLbl";
            this.lutOffsetBlueChannelLbl.Size = new System.Drawing.Size(97, 42);
            this.lutOffsetBlueChannelLbl.TabIndex = 0;
            this.lutOffsetBlueChannelLbl.Text = "Offset Value";
            this.lutOffsetBlueChannelLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // blueChannelSineFactor_nud
            // 
            this.blueChannelSineFactor_nud.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.blueChannelSineFactor_nud.Location = new System.Drawing.Point(113, 10);
            this.blueChannelSineFactor_nud.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.blueChannelSineFactor_nud.Name = "blueChannelSineFactor_nud";
            this.blueChannelSineFactor_nud.Size = new System.Drawing.Size(172, 20);
            this.blueChannelSineFactor_nud.TabIndex = 1;
            this.blueChannelSineFactor_nud.ValueChanged += new System.EventHandler(this.blueChannelSineFactor_nud_ValueChanged);
            // 
            // blueChannelLutInterval1_nud
            // 
            this.blueChannelLutInterval1_nud.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.blueChannelLutInterval1_nud.Location = new System.Drawing.Point(113, 51);
            this.blueChannelLutInterval1_nud.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.blueChannelLutInterval1_nud.Name = "blueChannelLutInterval1_nud";
            this.blueChannelLutInterval1_nud.Size = new System.Drawing.Size(172, 20);
            this.blueChannelLutInterval1_nud.TabIndex = 1;
            this.blueChannelLutInterval1_nud.ValueChanged += new System.EventHandler(this.blueChannelLutInterval1_nud_ValueChanged);
            // 
            // blueChannelLutInterval2_nud
            // 
            this.blueChannelLutInterval2_nud.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.blueChannelLutInterval2_nud.Location = new System.Drawing.Point(113, 92);
            this.blueChannelLutInterval2_nud.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.blueChannelLutInterval2_nud.Name = "blueChannelLutInterval2_nud";
            this.blueChannelLutInterval2_nud.Size = new System.Drawing.Size(172, 20);
            this.blueChannelLutInterval2_nud.TabIndex = 1;
            this.blueChannelLutInterval2_nud.ValueChanged += new System.EventHandler(this.blueChannelLutInterval2_nud_ValueChanged);
            // 
            // blueChannelLutOffset_nud
            // 
            this.blueChannelLutOffset_nud.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.blueChannelLutOffset_nud.Location = new System.Drawing.Point(113, 134);
            this.blueChannelLutOffset_nud.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.blueChannelLutOffset_nud.Name = "blueChannelLutOffset_nud";
            this.blueChannelLutOffset_nud.Size = new System.Drawing.Size(172, 20);
            this.blueChannelLutOffset_nud.TabIndex = 1;
            this.blueChannelLutOffset_nud.ValueChanged += new System.EventHandler(this.blueChannelLutOffset_nud_ValueChanged);
            // 
            // blueChannelLutLbl
            // 
            this.blueChannelLutLbl.AutoSize = true;
            this.blueChannelLutLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blueChannelLutLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blueChannelLutLbl.Location = new System.Drawing.Point(3, 0);
            this.blueChannelLutLbl.Name = "blueChannelLutLbl";
            this.blueChannelLutLbl.Size = new System.Drawing.Size(295, 18);
            this.blueChannelLutLbl.TabIndex = 1;
            this.blueChannelLutLbl.Text = "Blue CHannel";
            this.blueChannelLutLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // showLutTbpl
            // 
            this.showLutTbpl.ColumnCount = 1;
            this.showLutTbpl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.64261F));
            this.showLutTbpl.Controls.Add(this.showAllChannelBtn, 0, 0);
            this.showLutTbpl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.showLutTbpl.Location = new System.Drawing.Point(3, 588);
            this.showLutTbpl.Name = "showLutTbpl";
            this.showLutTbpl.RowCount = 1;
            this.showLutTbpl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.showLutTbpl.Size = new System.Drawing.Size(301, 59);
            this.showLutTbpl.TabIndex = 3;
            // 
            // showAllChannelBtn
            // 
            this.showAllChannelBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.showAllChannelBtn.Location = new System.Drawing.Point(66, 15);
            this.showAllChannelBtn.Name = "showAllChannelBtn";
            this.showAllChannelBtn.Size = new System.Drawing.Size(168, 28);
            this.showAllChannelBtn.TabIndex = 4;
            this.showAllChannelBtn.Text = "Show All Channel LUT Curves";
            this.showAllChannelBtn.UseVisualStyleBackColor = true;
            this.showAllChannelBtn.Click += new System.EventHandler(this.showAllChannelBtn_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel32);
            this.panel1.Controls.Add(this.overlay_pbx);
            this.panel1.Controls.Add(this.display_pbx);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1027, 700);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanel32
            // 
            this.tableLayoutPanel32.ColumnCount = 5;
            this.tableLayoutPanel32.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3.030303F));
            this.tableLayoutPanel32.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.46465F));
            this.tableLayoutPanel32.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.010101F));
            this.tableLayoutPanel32.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.46465F));
            this.tableLayoutPanel32.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3.030303F));
            this.tableLayoutPanel32.Controls.Add(this.neg_pbx, 1, 0);
            this.tableLayoutPanel32.Controls.Add(this.rightArrow_pbx, 0, 0);
            this.tableLayoutPanel32.Controls.Add(this.pos_pbx, 3, 0);
            this.tableLayoutPanel32.Controls.Add(this.leftArrow_pbx, 4, 0);
            this.tableLayoutPanel32.Controls.Add(this.panel10, 2, 0);
            this.tableLayoutPanel32.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel32.Location = new System.Drawing.Point(0, 674);
            this.tableLayoutPanel32.Name = "tableLayoutPanel32";
            this.tableLayoutPanel32.RowCount = 1;
            this.tableLayoutPanel32.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel32.Size = new System.Drawing.Size(1027, 26);
            this.tableLayoutPanel32.TabIndex = 11;
            // 
            // neg_pbx
            // 
            this.neg_pbx.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.neg_pbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.neg_pbx.Location = new System.Drawing.Point(34, 3);
            this.neg_pbx.Name = "neg_pbx";
            this.neg_pbx.Size = new System.Drawing.Size(471, 20);
            this.neg_pbx.TabIndex = 0;
            this.neg_pbx.TabStop = false;
            this.neg_pbx.SizeChanged += new System.EventHandler(this.neg_pbx_SizeChanged);
            // 
            // pos_pbx
            // 
            this.pos_pbx.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pos_pbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pos_pbx.Location = new System.Drawing.Point(521, 3);
            this.pos_pbx.Name = "pos_pbx";
            this.pos_pbx.Size = new System.Drawing.Size(471, 20);
            this.pos_pbx.TabIndex = 0;
            this.pos_pbx.TabStop = false;
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.Red;
            this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel10.Location = new System.Drawing.Point(511, 3);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(4, 20);
            this.panel10.TabIndex = 4;
            // 
            // startFFATimer_btn
            // 
            this.startFFATimer_btn.Enabled = false;
            this.startFFATimer_btn.Location = new System.Drawing.Point(235, 3);
            this.startFFATimer_btn.Name = "startFFATimer_btn";
            this.startFFATimer_btn.Size = new System.Drawing.Size(65, 33);
            this.startFFATimer_btn.TabIndex = 1;
            this.startFFATimer_btn.Text = "Start FFA";
            this.startFFATimer_btn.UseVisualStyleBackColor = true;
            this.startFFATimer_btn.Click += new System.EventHandler(this.startFFATimer_btn_Click);
            // 
            // clearOverlay_btn
            // 
            this.clearOverlay_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clearOverlay_btn.Location = new System.Drawing.Point(177, 3);
            this.clearOverlay_btn.Name = "clearOverlay_btn";
            this.clearOverlay_btn.Size = new System.Drawing.Size(52, 33);
            this.clearOverlay_btn.TabIndex = 1;
            this.clearOverlay_btn.Text = "Clear Overlays";
            this.clearOverlay_btn.UseVisualStyleBackColor = true;
            this.clearOverlay_btn.Click += new System.EventHandler(this.clearOverlay_btn_Click);
            // 
            // browse_btn
            // 
            this.browse_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browse_btn.Location = new System.Drawing.Point(61, 3);
            this.browse_btn.Name = "browse_btn";
            this.browse_btn.Size = new System.Drawing.Size(52, 33);
            this.browse_btn.TabIndex = 3;
            this.browse_btn.Text = "Browse";
            this.browse_btn.UseVisualStyleBackColor = true;
            this.browse_btn.Click += new System.EventHandler(this.browse_btn_Click);
            // 
            // saveFrames_btn
            // 
            this.saveFrames_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.saveFrames_btn.Location = new System.Drawing.Point(3, 3);
            this.saveFrames_btn.Name = "saveFrames_btn";
            this.saveFrames_btn.Size = new System.Drawing.Size(52, 33);
            this.saveFrames_btn.TabIndex = 0;
            this.saveFrames_btn.Text = "Save-Frames";
            this.saveFrames_btn.UseVisualStyleBackColor = true;
            this.saveFrames_btn.Click += new System.EventHandler(this.saveFrames_btn_Click);
            // 
            // BrowseOverlay_btn
            // 
            this.BrowseOverlay_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BrowseOverlay_btn.Location = new System.Drawing.Point(0, 0);
            this.BrowseOverlay_btn.Name = "BrowseOverlay_btn";
            this.BrowseOverlay_btn.Size = new System.Drawing.Size(52, 33);
            this.BrowseOverlay_btn.TabIndex = 1;
            this.BrowseOverlay_btn.Text = "Overlays";
            this.BrowseOverlay_btn.UseVisualStyleBackColor = true;
            this.BrowseOverlay_btn.Click += new System.EventHandler(this.BrowseOverlay_btn_Click);
            // 
            // connect_btn
            // 
            this.connect_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.connect_btn.Location = new System.Drawing.Point(195, 3);
            this.connect_btn.Name = "connect_btn";
            this.connect_btn.Size = new System.Drawing.Size(109, 108);
            this.connect_btn.TabIndex = 49;
            this.connect_btn.Text = "Connect";
            this.connect_btn.UseVisualStyleBackColor = true;
            this.connect_btn.Click += new System.EventHandler(this.connect_btn_Click);
            // 
            // MotorForward_cbx
            // 
            this.MotorForward_cbx.AutoSize = true;
            this.MotorForward_cbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MotorForward_cbx.Location = new System.Drawing.Point(3, 3);
            this.MotorForward_cbx.Name = "MotorForward_cbx";
            this.MotorForward_cbx.Size = new System.Drawing.Size(98, 17);
            this.MotorForward_cbx.TabIndex = 48;
            this.MotorForward_cbx.Text = "Motor Forward";
            this.MotorForward_cbx.UseVisualStyleBackColor = true;
            this.MotorForward_cbx.CheckedChanged += new System.EventHandler(this.MotorForward_cbx_CheckedChanged);
            // 
            // greenIRLive_cbx
            // 
            this.greenIRLive_cbx.AutoSize = true;
            this.greenIRLive_cbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.greenIRLive_cbx.Location = new System.Drawing.Point(107, 3);
            this.greenIRLive_cbx.Name = "greenIRLive_cbx";
            this.greenIRLive_cbx.Size = new System.Drawing.Size(77, 17);
            this.greenIRLive_cbx.TabIndex = 48;
            this.greenIRLive_cbx.Text = "Green IR";
            this.greenIRLive_cbx.UseVisualStyleBackColor = true;
            this.greenIRLive_cbx.CheckedChanged += new System.EventHandler(this.greenIRLive_cbx_CheckedChanged);
            // 
            // contCapture_cbx
            // 
            this.contCapture_cbx.AutoSize = true;
            this.contCapture_cbx.Location = new System.Drawing.Point(228, 0);
            this.contCapture_cbx.Name = "contCapture_cbx";
            this.contCapture_cbx.Size = new System.Drawing.Size(67, 17);
            this.contCapture_cbx.TabIndex = 2;
            this.contCapture_cbx.Text = "ContCptr";
            this.contCapture_cbx.UseVisualStyleBackColor = true;
            this.contCapture_cbx.CheckedChanged += new System.EventHandler(this.contCapture_cbx_CheckedChanged);
            // 
            // PatientName_tbx
            // 
            this.PatientName_tbx.Dock = System.Windows.Forms.DockStyle.Left;
            this.PatientName_tbx.Location = new System.Drawing.Point(71, 0);
            this.PatientName_tbx.Name = "PatientName_tbx";
            this.PatientName_tbx.Size = new System.Drawing.Size(93, 20);
            this.PatientName_tbx.TabIndex = 92;
            this.PatientName_tbx.TextChanged += new System.EventHandler(this.PatientName_tbx_TextChanged);
            // 
            // enableLivePostProcessing_cbx
            // 
            this.enableLivePostProcessing_cbx.AutoSize = true;
            this.enableLivePostProcessing_cbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enableLivePostProcessing_cbx.Location = new System.Drawing.Point(173, 3);
            this.enableLivePostProcessing_cbx.Name = "enableLivePostProcessing_cbx";
            this.enableLivePostProcessing_cbx.Size = new System.Drawing.Size(61, 17);
            this.enableLivePostProcessing_cbx.TabIndex = 55;
            this.enableLivePostProcessing_cbx.Text = "Live pp";
            this.enableLivePostProcessing_cbx.UseVisualStyleBackColor = true;
            this.enableLivePostProcessing_cbx.CheckedChanged += new System.EventHandler(this.enableLivePostProcessing_cbx_CheckedChanged);
            // 
            // postProcessing_cbx
            // 
            this.postProcessing_cbx.AutoSize = true;
            this.postProcessing_cbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.postProcessing_cbx.Location = new System.Drawing.Point(231, 3);
            this.postProcessing_cbx.Name = "postProcessing_cbx";
            this.postProcessing_cbx.Size = new System.Drawing.Size(38, 17);
            this.postProcessing_cbx.TabIndex = 55;
            this.postProcessing_cbx.Text = "pp";
            this.postProcessing_cbx.UseVisualStyleBackColor = true;
            this.postProcessing_cbx.CheckedChanged += new System.EventHandler(this.postProcessing_cbx_CheckedChanged);
            // 
            // showOverlay_cbx
            // 
            this.showOverlay_cbx.AutoSize = true;
            this.showOverlay_cbx.Checked = true;
            this.showOverlay_cbx.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showOverlay_cbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.showOverlay_cbx.Location = new System.Drawing.Point(237, 3);
            this.showOverlay_cbx.Name = "showOverlay_cbx";
            this.showOverlay_cbx.Size = new System.Drawing.Size(67, 46);
            this.showOverlay_cbx.TabIndex = 52;
            this.showOverlay_cbx.Text = "Overlay";
            this.showOverlay_cbx.UseVisualStyleBackColor = true;
            // 
            // flashBoost_cbx
            // 
            this.flashBoost_cbx.AutoSize = true;
            this.flashBoost_cbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flashBoost_cbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flashBoost_cbx.Location = new System.Drawing.Point(3, 3);
            this.flashBoost_cbx.Name = "flashBoost_cbx";
            this.flashBoost_cbx.Size = new System.Drawing.Size(53, 46);
            this.flashBoost_cbx.TabIndex = 52;
            this.flashBoost_cbx.Text = "Boost";
            this.flashBoost_cbx.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.flashBoost_cbx.UseVisualStyleBackColor = true;
            this.flashBoost_cbx.CheckedChanged += new System.EventHandler(this.flashBoost_cbx_CheckedChanged);
            // 
            // liveCC_cbx
            // 
            this.liveCC_cbx.AutoSize = true;
            this.liveCC_cbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.liveCC_cbx.Location = new System.Drawing.Point(119, 153);
            this.liveCC_cbx.Name = "liveCC_cbx";
            this.liveCC_cbx.Size = new System.Drawing.Size(49, 17);
            this.liveCC_cbx.TabIndex = 97;
            this.liveCC_cbx.Text = "LCC";
            this.liveCC_cbx.UseVisualStyleBackColor = true;
            this.liveCC_cbx.CheckedChanged += new System.EventHandler(this.liveCC_cbx_CheckedChanged);
            // 
            // formCheckBox1
            // 
            this.formCheckBox1.AutoSize = true;
            this.formCheckBox1.Location = new System.Drawing.Point(155, 623);
            this.formCheckBox1.Name = "formCheckBox1";
            this.formCheckBox1.Size = new System.Drawing.Size(111, 17);
            this.formCheckBox1.TabIndex = 95;
            this.formCheckBox1.Text = "EnableColorMatrix";
            this.formCheckBox1.UseVisualStyleBackColor = true;
            this.formCheckBox1.CheckedChanged += new System.EventHandler(this.formCheckBox1_CheckedChanged_1);
            // 
            // EnableTemperatureTint_cbx
            // 
            this.EnableTemperatureTint_cbx.AutoSize = true;
            this.EnableTemperatureTint_cbx.Location = new System.Drawing.Point(12, 623);
            this.EnableTemperatureTint_cbx.Name = "EnableTemperatureTint_cbx";
            this.EnableTemperatureTint_cbx.Size = new System.Drawing.Size(137, 17);
            this.EnableTemperatureTint_cbx.TabIndex = 95;
            this.EnableTemperatureTint_cbx.Text = "EnableTemperatureTint";
            this.EnableTemperatureTint_cbx.UseVisualStyleBackColor = true;
            this.EnableTemperatureTint_cbx.CheckedChanged += new System.EventHandler(this.EnableTemperatureTint_cbx_CheckedChanged);
            // 
            // getRaw_btn
            // 
            this.getRaw_btn.Location = new System.Drawing.Point(12, 496);
            this.getRaw_btn.Name = "getRaw_btn";
            this.getRaw_btn.Size = new System.Drawing.Size(247, 44);
            this.getRaw_btn.TabIndex = 93;
            this.getRaw_btn.Text = "Get Raw";
            this.getRaw_btn.UseVisualStyleBackColor = true;
            this.getRaw_btn.Click += new System.EventHandler(this.getRaw_btn_Click);
            // 
            // ApplyHSVBoost_cbx
            // 
            this.ApplyHSVBoost_cbx.AutoSize = true;
            this.ApplyHSVBoost_cbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ApplyHSVBoost_cbx.Location = new System.Drawing.Point(9, 329);
            this.ApplyHSVBoost_cbx.Name = "ApplyHSVBoost_cbx";
            this.ApplyHSVBoost_cbx.Size = new System.Drawing.Size(91, 17);
            this.ApplyHSVBoost_cbx.TabIndex = 92;
            this.ApplyHSVBoost_cbx.Text = " HSV Boost";
            this.ApplyHSVBoost_cbx.UseVisualStyleBackColor = true;
            this.ApplyHSVBoost_cbx.CheckedChanged += new System.EventHandler(this.ApplyHSVBoost_cbx_CheckedChanged);
            // 
            // applyClahe_cbx
            // 
            this.applyClahe_cbx.AutoSize = true;
            this.applyClahe_cbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.applyClahe_cbx.Location = new System.Drawing.Point(10, 303);
            this.applyClahe_cbx.Name = "applyClahe_cbx";
            this.applyClahe_cbx.Size = new System.Drawing.Size(62, 17);
            this.applyClahe_cbx.TabIndex = 92;
            this.applyClahe_cbx.Text = " Clahe";
            this.applyClahe_cbx.UseVisualStyleBackColor = true;
            this.applyClahe_cbx.CheckedChanged += new System.EventHandler(this.applyClahe_cbx_CheckedChanged);
            // 
            // isApplyLut_cbx
            // 
            this.isApplyLut_cbx.AutoSize = true;
            this.isApplyLut_cbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.isApplyLut_cbx.Location = new System.Drawing.Point(9, 280);
            this.isApplyLut_cbx.Name = "isApplyLut_cbx";
            this.isApplyLut_cbx.Size = new System.Drawing.Size(50, 17);
            this.isApplyLut_cbx.TabIndex = 92;
            this.isApplyLut_cbx.Text = "LUT";
            this.isApplyLut_cbx.UseVisualStyleBackColor = true;
            this.isApplyLut_cbx.CheckedChanged += new System.EventHandler(this.isApplyLut_cbx_CheckedChanged);
            // 
            // applyMask_cbx
            // 
            this.applyMask_cbx.AutoSize = true;
            this.applyMask_cbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.applyMask_cbx.Location = new System.Drawing.Point(7, 180);
            this.applyMask_cbx.Name = "applyMask_cbx";
            this.applyMask_cbx.Size = new System.Drawing.Size(56, 17);
            this.applyMask_cbx.TabIndex = 86;
            this.applyMask_cbx.Text = "Mask";
            this.applyMask_cbx.UseVisualStyleBackColor = true;
            this.applyMask_cbx.CheckedChanged += new System.EventHandler(this.applyMask_cbx_CheckedChanged);
            // 
            // applyColorCorrection_cbx
            // 
            this.applyColorCorrection_cbx.AutoSize = true;
            this.applyColorCorrection_cbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.applyColorCorrection_cbx.Location = new System.Drawing.Point(6, 153);
            this.applyColorCorrection_cbx.Name = "applyColorCorrection_cbx";
            this.applyColorCorrection_cbx.Size = new System.Drawing.Size(113, 17);
            this.applyColorCorrection_cbx.TabIndex = 86;
            this.applyColorCorrection_cbx.Text = "ColorCorrection";
            this.applyColorCorrection_cbx.UseVisualStyleBackColor = true;
            this.applyColorCorrection_cbx.CheckedChanged += new System.EventHandler(this.applyColorCorrection_cbx_CheckedChanged);
            // 
            // applyHotSpotCorrection_cbx
            // 
            this.applyHotSpotCorrection_cbx.AutoSize = true;
            this.applyHotSpotCorrection_cbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.applyHotSpotCorrection_cbx.Location = new System.Drawing.Point(7, 130);
            this.applyHotSpotCorrection_cbx.Name = "applyHotSpotCorrection_cbx";
            this.applyHotSpotCorrection_cbx.Size = new System.Drawing.Size(76, 17);
            this.applyHotSpotCorrection_cbx.TabIndex = 86;
            this.applyHotSpotCorrection_cbx.Text = " HotSpot";
            this.applyHotSpotCorrection_cbx.UseVisualStyleBackColor = true;
            this.applyHotSpotCorrection_cbx.CheckedChanged += new System.EventHandler(this.applyHotSpotCorrection_cbx_CheckedChanged);
            // 
            // applyShift_cbx
            // 
            this.applyShift_cbx.AutoSize = true;
            this.applyShift_cbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.applyShift_cbx.Location = new System.Drawing.Point(7, 107);
            this.applyShift_cbx.Name = "applyShift_cbx";
            this.applyShift_cbx.Size = new System.Drawing.Size(52, 17);
            this.applyShift_cbx.TabIndex = 86;
            this.applyShift_cbx.Text = "Shift";
            this.applyShift_cbx.UseVisualStyleBackColor = true;
            this.applyShift_cbx.CheckedChanged += new System.EventHandler(this.applyShift_cbx_CheckedChanged);
            // 
            // saveSettings_btn
            // 
            this.saveSettings_btn.Location = new System.Drawing.Point(12, 445);
            this.saveSettings_btn.Name = "saveSettings_btn";
            this.saveSettings_btn.Size = new System.Drawing.Size(245, 46);
            this.saveSettings_btn.TabIndex = 88;
            this.saveSettings_btn.Text = "Save Settings";
            this.saveSettings_btn.UseVisualStyleBackColor = true;
            this.saveSettings_btn.Click += new System.EventHandler(this.saveSettings_btn_Click);
            // 
            // applyGamma_cbx
            // 
            this.applyGamma_cbx.AutoSize = true;
            this.applyGamma_cbx.Location = new System.Drawing.Point(6, 444);
            this.applyGamma_cbx.Name = "applyGamma_cbx";
            this.applyGamma_cbx.Size = new System.Drawing.Size(91, 17);
            this.applyGamma_cbx.TabIndex = 11;
            this.applyGamma_cbx.Text = "Apply Gamma";
            this.applyGamma_cbx.UseVisualStyleBackColor = true;
            this.applyGamma_cbx.CheckedChanged += new System.EventHandler(this.applyGamma_cbx_CheckedChanged);
            // 
            // LiveMask_cbx
            // 
            this.LiveMask_cbx.AutoSize = true;
            this.LiveMask_cbx.Location = new System.Drawing.Point(173, 265);
            this.LiveMask_cbx.Name = "LiveMask_cbx";
            this.LiveMask_cbx.Size = new System.Drawing.Size(75, 17);
            this.LiveMask_cbx.TabIndex = 79;
            this.LiveMask_cbx.Text = "Live Mask";
            this.LiveMask_cbx.UseVisualStyleBackColor = true;
            this.LiveMask_cbx.CheckedChanged += new System.EventHandler(this.LiveMask_cbx_CheckedChanged);
            // 
            // colorChannel_btn
            // 
            this.colorChannel_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.colorChannel_btn.Location = new System.Drawing.Point(65, 3);
            this.colorChannel_btn.Name = "colorChannel_btn";
            this.colorChannel_btn.Size = new System.Drawing.Size(50, 39);
            this.colorChannel_btn.TabIndex = 0;
            this.colorChannel_btn.UseVisualStyleBackColor = true;
            this.colorChannel_btn.Click += new System.EventHandler(this.colorChannel_btn_Click);
            // 
            // redChannel_btn
            // 
            this.redChannel_btn.BackColor = System.Drawing.Color.Red;
            this.redChannel_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.redChannel_btn.Location = new System.Drawing.Point(121, 3);
            this.redChannel_btn.Name = "redChannel_btn";
            this.redChannel_btn.Size = new System.Drawing.Size(54, 39);
            this.redChannel_btn.TabIndex = 0;
            this.redChannel_btn.Text = "R";
            this.redChannel_btn.UseVisualStyleBackColor = false;
            this.redChannel_btn.Click += new System.EventHandler(this.redChannel_btn_Click);
            // 
            // greenChannel_btn
            // 
            this.greenChannel_btn.BackColor = System.Drawing.Color.Green;
            this.greenChannel_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.greenChannel_btn.Location = new System.Drawing.Point(181, 3);
            this.greenChannel_btn.Name = "greenChannel_btn";
            this.greenChannel_btn.Size = new System.Drawing.Size(54, 39);
            this.greenChannel_btn.TabIndex = 0;
            this.greenChannel_btn.Text = "G";
            this.greenChannel_btn.UseVisualStyleBackColor = false;
            this.greenChannel_btn.Click += new System.EventHandler(this.greenChannel_btn_Click);
            // 
            // blueChannel_btn
            // 
            this.blueChannel_btn.BackColor = System.Drawing.Color.Blue;
            this.blueChannel_btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blueChannel_btn.Location = new System.Drawing.Point(241, 3);
            this.blueChannel_btn.Name = "blueChannel_btn";
            this.blueChannel_btn.Size = new System.Drawing.Size(57, 39);
            this.blueChannel_btn.TabIndex = 0;
            this.blueChannel_btn.Text = "B";
            this.blueChannel_btn.UseVisualStyleBackColor = false;
            this.blueChannel_btn.Click += new System.EventHandler(this.blueChannel_btn_Click);
            // 
            // vFlip_cbx
            // 
            this.vFlip_cbx.AutoSize = true;
            this.vFlip_cbx.Location = new System.Drawing.Point(14, 19);
            this.vFlip_cbx.Name = "vFlip_cbx";
            this.vFlip_cbx.Size = new System.Drawing.Size(80, 17);
            this.vFlip_cbx.TabIndex = 1;
            this.vFlip_cbx.Text = "Vertical Flip";
            this.vFlip_cbx.UseVisualStyleBackColor = true;
            this.vFlip_cbx.CheckedChanged += new System.EventHandler(this.vFlip_cbx_CheckedChanged);
            // 
            // hFlip_cbx
            // 
            this.hFlip_cbx.AutoSize = true;
            this.hFlip_cbx.Location = new System.Drawing.Point(144, 19);
            this.hFlip_cbx.Name = "hFlip_cbx";
            this.hFlip_cbx.Size = new System.Drawing.Size(92, 17);
            this.hFlip_cbx.TabIndex = 1;
            this.hFlip_cbx.Text = "Horizontal Flip";
            this.hFlip_cbx.UseVisualStyleBackColor = true;
            this.hFlip_cbx.CheckedChanged += new System.EventHandler(this.hFlip_cbx_CheckedChanged);
            // 
            // saveLiveFrame_btn
            // 
            this.saveLiveFrame_btn.Location = new System.Drawing.Point(96, 61);
            this.saveLiveFrame_btn.Name = "saveLiveFrame_btn";
            this.saveLiveFrame_btn.Size = new System.Drawing.Size(117, 23);
            this.saveLiveFrame_btn.TabIndex = 3;
            this.saveLiveFrame_btn.Text = "Save Live Frame";
            this.saveLiveFrame_btn.UseVisualStyleBackColor = true;
            this.saveLiveFrame_btn.Click += new System.EventHandler(this.saveLiveFrame_btn_Click);
            // 
            // applyOverlayGrid_btn
            // 
            this.applyOverlayGrid_btn.Location = new System.Drawing.Point(19, 61);
            this.applyOverlayGrid_btn.Name = "applyOverlayGrid_btn";
            this.applyOverlayGrid_btn.Size = new System.Drawing.Size(75, 23);
            this.applyOverlayGrid_btn.TabIndex = 3;
            this.applyOverlayGrid_btn.Text = "Apply Grid Overlay";
            this.applyOverlayGrid_btn.UseVisualStyleBackColor = true;
            // 
            // unsharp_btn
            // 
            this.unsharp_btn.Location = new System.Drawing.Point(99, 32);
            this.unsharp_btn.Name = "unsharp_btn";
            this.unsharp_btn.Size = new System.Drawing.Size(75, 23);
            this.unsharp_btn.TabIndex = 3;
            this.unsharp_btn.Text = "Unsharp";
            this.unsharp_btn.UseVisualStyleBackColor = true;
            this.unsharp_btn.Click += new System.EventHandler(this.unsharp_btn_Click_1);
            // 
            // createLUT_btn
            // 
            this.createLUT_btn.Location = new System.Drawing.Point(99, 3);
            this.createLUT_btn.Name = "createLUT_btn";
            this.createLUT_btn.Size = new System.Drawing.Size(75, 23);
            this.createLUT_btn.TabIndex = 3;
            this.createLUT_btn.Text = "Create LUT";
            this.createLUT_btn.UseVisualStyleBackColor = true;
            this.createLUT_btn.Click += new System.EventHandler(this.createLUT_btn_Click);
            // 
            // SaveProcessedImage_cbx
            // 
            this.SaveProcessedImage_cbx.AutoSize = true;
            this.SaveProcessedImage_cbx.Location = new System.Drawing.Point(6, 26);
            this.SaveProcessedImage_cbx.Name = "SaveProcessedImage_cbx";
            this.SaveProcessedImage_cbx.Size = new System.Drawing.Size(136, 17);
            this.SaveProcessedImage_cbx.TabIndex = 1;
            this.SaveProcessedImage_cbx.Text = "Save Processed Image";
            this.SaveProcessedImage_cbx.UseVisualStyleBackColor = true;
            this.SaveProcessedImage_cbx.CheckedChanged += new System.EventHandler(this.SaveProcessedImage_cbx_CheckedChanged);
            // 
            // SaveDebugImage_cbx
            // 
            this.SaveDebugImage_cbx.AutoSize = true;
            this.SaveDebugImage_cbx.Location = new System.Drawing.Point(174, 3);
            this.SaveDebugImage_cbx.Name = "SaveDebugImage_cbx";
            this.SaveDebugImage_cbx.Size = new System.Drawing.Size(108, 17);
            this.SaveDebugImage_cbx.TabIndex = 1;
            this.SaveDebugImage_cbx.Text = "Save Raw Image";
            this.SaveDebugImage_cbx.UseVisualStyleBackColor = true;
            this.SaveDebugImage_cbx.CheckedChanged += new System.EventHandler(this.SaveDebugImage_cbx_CheckedChanged);
            // 
            // saveRaw_cbx
            // 
            this.saveRaw_cbx.AutoSize = true;
            this.saveRaw_cbx.Location = new System.Drawing.Point(70, 3);
            this.saveRaw_cbx.Name = "saveRaw_cbx";
            this.saveRaw_cbx.Size = new System.Drawing.Size(104, 17);
            this.saveRaw_cbx.TabIndex = 1;
            this.saveRaw_cbx.Text = "Save Raw bytes";
            this.saveRaw_cbx.UseVisualStyleBackColor = true;
            this.saveRaw_cbx.CheckedChanged += new System.EventHandler(this.saveRaw_cbx_CheckedChanged);
            // 
            // SaveIr_cbx
            // 
            this.SaveIr_cbx.AutoSize = true;
            this.SaveIr_cbx.Location = new System.Drawing.Point(6, 3);
            this.SaveIr_cbx.Name = "SaveIr_cbx";
            this.SaveIr_cbx.Size = new System.Drawing.Size(68, 17);
            this.SaveIr_cbx.TabIndex = 1;
            this.SaveIr_cbx.Text = "Save IR ";
            this.SaveIr_cbx.UseVisualStyleBackColor = true;
            this.SaveIr_cbx.CheckedChanged += new System.EventHandler(this.SaveIr_cbx_CheckedChanged);
            // 
            // showExtViewer_cbx
            // 
            this.showExtViewer_cbx.AutoSize = true;
            this.showExtViewer_cbx.Location = new System.Drawing.Point(174, 26);
            this.showExtViewer_cbx.Name = "showExtViewer_cbx";
            this.showExtViewer_cbx.Size = new System.Drawing.Size(106, 17);
            this.showExtViewer_cbx.TabIndex = 2;
            this.showExtViewer_cbx.Text = "ShowInIrfanView";
            this.showExtViewer_cbx.UseVisualStyleBackColor = true;
            this.showExtViewer_cbx.CheckedChanged += new System.EventHandler(this.showExtViewer_cbx_CheckedChanged);
            // 
            // formButtons1
            // 
            this.formButtons1.Location = new System.Drawing.Point(163, 601);
            this.formButtons1.Name = "formButtons1";
            this.formButtons1.Size = new System.Drawing.Size(123, 34);
            this.formButtons1.TabIndex = 155;
            this.formButtons1.Text = "Hotspot Params";
            this.formButtons1.UseVisualStyleBackColor = true;
            // 
            // GetHoSpotParams_btn
            // 
            this.GetHoSpotParams_btn.Location = new System.Drawing.Point(7, 601);
            this.GetHoSpotParams_btn.Name = "GetHoSpotParams_btn";
            this.GetHoSpotParams_btn.Size = new System.Drawing.Size(130, 34);
            this.GetHoSpotParams_btn.TabIndex = 156;
            this.GetHoSpotParams_btn.Text = "HotspotParamsBatch ";
            this.GetHoSpotParams_btn.UseVisualStyleBackColor = true;
            // 
            // rightArrow_pbx
            // 
            this.rightArrow_pbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightArrow_pbx.Index = 0;
            this.rightArrow_pbx.Location = new System.Drawing.Point(3, 3);
            this.rightArrow_pbx.Name = "rightArrow_pbx";
            this.rightArrow_pbx.Size = new System.Drawing.Size(25, 20);
            this.rightArrow_pbx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.rightArrow_pbx.TabIndex = 3;
            this.rightArrow_pbx.TabStop = false;
            // 
            // leftArrow_pbx
            // 
            this.leftArrow_pbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftArrow_pbx.Index = 0;
            this.leftArrow_pbx.Location = new System.Drawing.Point(998, 3);
            this.leftArrow_pbx.Name = "leftArrow_pbx";
            this.leftArrow_pbx.Size = new System.Drawing.Size(26, 20);
            this.leftArrow_pbx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.leftArrow_pbx.TabIndex = 2;
            this.leftArrow_pbx.TabStop = false;
            // 
            // overlay_pbx
            // 
            this.overlay_pbx.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.overlay_pbx.Index = 0;
            this.overlay_pbx.Location = new System.Drawing.Point(44, 6);
            this.overlay_pbx.Name = "overlay_pbx";
            this.overlay_pbx.Size = new System.Drawing.Size(100, 50);
            this.overlay_pbx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.overlay_pbx.TabIndex = 10;
            this.overlay_pbx.TabStop = false;
            this.overlay_pbx.Visible = false;
            this.overlay_pbx.MouseClick += new System.Windows.Forms.MouseEventHandler(this.overlay_pbx_MouseClick);
            // 
            // display_pbx
            // 
            this.display_pbx.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.display_pbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.display_pbx.Index = 0;
            this.display_pbx.Location = new System.Drawing.Point(0, 0);
            this.display_pbx.Name = "display_pbx";
            this.display_pbx.Size = new System.Drawing.Size(1027, 700);
            this.display_pbx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.display_pbx.TabIndex = 9;
            this.display_pbx.TabStop = false;
            // 
            // CameraUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1366, 738);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "CameraUI";
            this.Text = "Assembly View";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CameraUI_FormClosing);
            this.Shown += new System.EventHandler(this.CameraUI_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CameraUI_KeyDown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.control_tb.ResumeLayout(false);
            this.basicControls_tbp.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ExpGain_gbx.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.expVal_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gainVal_nud)).EndInit();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.captureExpGain_gbx.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flashGain_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.flashExp_nud)).EndInit();
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.tableLayoutPanel12.ResumeLayout(false);
            this.tableLayoutPanel12.PerformLayout();
            this.tableLayoutPanel21.ResumeLayout(false);
            this.tableLayoutPanel21.PerformLayout();
            this.tableLayoutPanel22.ResumeLayout(false);
            this.tableLayoutPanel22.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel10.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel11.ResumeLayout(false);
            this.tableLayoutPanel11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IrOffsetSteps_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.flashOffset2_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.flashoffset1_nud)).EndInit();
            this.panel12.ResumeLayout(false);
            this.panel12.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.powerStatus_pbx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cameraStatus_pbx)).EndInit();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.tableLayoutPanel30.ResumeLayout(false);
            this.tableLayoutPanel30.PerformLayout();
            this.tableLayoutPanel31.ResumeLayout(false);
            this.tableLayoutPanel31.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flashBoost_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.triggerRecieved_pbx)).EndInit();
            this.tableLayoutPanel34.ResumeLayout(false);
            this.tableLayoutPanel34.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.tableLayoutPanel17.ResumeLayout(false);
            this.tableLayoutPanel17.PerformLayout();
            this.postProcessing_tbp.ResumeLayout(false);
            this.postProcessing_tbp.PerformLayout();
            this.tempTint_gbx.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.temperature_tb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tint_tb)).EndInit();
            this.shiftImage_tbp.ResumeLayout(false);
            this.shiftImage_tbp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Yshift_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xShift_nud)).EndInit();
            this.ColorCorrection_tbp.ResumeLayout(false);
            this.ColorCorrection_tbp.PerformLayout();
            this.tableLayoutPanel20.ResumeLayout(false);
            this.tableLayoutPanel20.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.brightness_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contrast_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.offset1_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LUT_interval2_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LUT_interval1_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LUT_SineFactor_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hsvBoostVal_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.medianfilter_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unsharpAmount_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unsharpRadius_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unsharpThresh_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClaheClipValueR_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClaheClipValueG_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClaheClipValueB_nud)).EndInit();
            this.tableLayoutPanel29.ResumeLayout(false);
            this.tableLayoutPanel29.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gammaVal_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueGreen_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenGreen_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.redGreen_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueBlue_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueRed_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenBlue_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenRed_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.redBlue_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.redRed_nud)).EndInit();
            this.Mask_tbp.ResumeLayout(false);
            this.Mask_tbp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CaptureMaskHeight_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CaptureMaskWidth_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LiveMaskHeight_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LiveMaskWidth_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maskY_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maskX_nud)).EndInit();
            this.CentreImage_tbp.ResumeLayout(false);
            this.misc_tbpg.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.tableLayoutPanel14.ResumeLayout(false);
            this.tableLayoutPanel13.ResumeLayout(false);
            this.tableLayoutPanel13.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel15.ResumeLayout(false);
            this.tableLayoutPanel15.PerformLayout();
            this.motorSensor_gbx.ResumeLayout(false);
            this.tableLayoutPanel16.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.tableLayoutPanel23.ResumeLayout(false);
            this.tableLayoutPanel23.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.redGain_tb)).EndInit();
            this.tableLayoutPanel24.ResumeLayout(false);
            this.tableLayoutPanel24.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.greenGain_tb)).EndInit();
            this.tableLayoutPanel25.ResumeLayout(false);
            this.tableLayoutPanel25.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.blueGain_tb)).EndInit();
            this.tableLayoutPanel26.ResumeLayout(false);
            this.tableLayoutPanel26.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.liveRedGain_tb)).EndInit();
            this.tableLayoutPanel27.ResumeLayout(false);
            this.tableLayoutPanel27.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.liveGreenGain_tb)).EndInit();
            this.tableLayoutPanel28.ResumeLayout(false);
            this.tableLayoutPanel28.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.liveBlueGain_tb)).EndInit();
            this.ffa_tbpg.ResumeLayout(false);
            this.tableLayoutPanel18.ResumeLayout(false);
            this.tableLayoutPanel19.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.greenFilterPos_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueFilterPos_nud)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.tableLayoutPanel33.ResumeLayout(false);
            this.tableLayoutPanel33.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frameDetectionVal_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.darkFameDetectionVal_nud)).EndInit();
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.saveFramesCount_nud)).EndInit();
            this.panel14.ResumeLayout(false);
            this.panel14.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FFA_Color_Pot_Int_Offset_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FFA_Pot_Int_Offset_nud)).EndInit();
            this.HotSpot_tbp.ResumeLayout(false);
            this.HotSpot_tbp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.shadowRedPeak_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.shadowGreenPeak_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.shadowBluePeak_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HsRedPeak_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HsGreenPeak_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HsBluePeak_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HsRedRadius_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HsGreenRadius_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HsBlueRadius_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HotSpotCentreY_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HotSpotCentreX_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HSRad2_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HSRad1_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShadowRad2_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShadowRad1_nud)).EndInit();
            this.allChannelLut_tbp.ResumeLayout(false);
            this.channnelsTbpl.ResumeLayout(false);
            this.redChannelTblPnl.ResumeLayout(false);
            this.redChannelTblPnl.PerformLayout();
            this.redKutSettingTbpl.ResumeLayout(false);
            this.redKutSettingTbpl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.redChannelSineFactor_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.redChannelLutInterval1_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.redChannelLutInterval2_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.redChannelLutOffset_nud)).EndInit();
            this.greenChannelTbpl.ResumeLayout(false);
            this.greenChannelTbpl.PerformLayout();
            this.greenLutSettingsTbpl.ResumeLayout(false);
            this.greenLutSettingsTbpl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.greenChannelSineFactor_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenChannelLutInterval1_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenChannelLutInterval2_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenChannelLutOffset_nud)).EndInit();
            this.blueChannelTbpl.ResumeLayout(false);
            this.blueChannelTbpl.PerformLayout();
            this.blueLutSettingsTbpl.ResumeLayout(false);
            this.blueLutSettingsTbpl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.blueChannelSineFactor_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueChannelLutInterval1_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueChannelLutInterval2_nud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueChannelLutOffset_nud)).EndInit();
            this.showLutTbpl.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel32.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.neg_pbx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pos_pbx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightArrow_pbx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftArrow_pbx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.overlay_pbx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.display_pbx)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        //private FormButtons connect_btn;
        //private FormButtons saveFrames_btn;
        private TableLayoutPanel tableLayoutPanel1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel cameraStatus_lbl;
        private ToolStripStatusLabel Resolution_lbl;
        private ToolStripStatusLabel FrameRate_lbl;
        private ToolStripStatusLabel FrameStatus_lbl;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Panel panel1;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripComboBox resolution_combx;
        private PictureBoxExtended overlay_pbx;
        private ToolStripStatusLabel temperaturStatus_lbl;
        private ToolStripStatusLabel tintStatus_lbl;
        private PictureBoxExtended display_pbx;
        private TableLayoutPanel tableLayoutPanel32;
        private PictureBox neg_pbx;
        private PictureBoxExtended rightArrow_pbx;
        private PictureBox pos_pbx;
        private PictureBoxExtended leftArrow_pbx;
        private Panel panel10;
        private ToolStripStatusLabel cameraConnection_lbl;
        private ToolStripStatusLabel powerConnection_lbl;
        private TabControl control_tb;
        private TabPage basicControls_tbp;
        private TableLayoutPanel tableLayoutPanel3;
        private GroupBox ExpGain_gbx;
        private TableLayoutPanel tableLayoutPanel4;
        private NumericUpDown expVal_nud;
        private NumericUpDown gainVal_nud;
        private Label gain_lbl;
        private Label exp_lbl;
        private TableLayoutPanel tableLayoutPanel5;
        private FormButtons clearOverlay_btn;
        private FormButtons browse_btn;
        private FormButtons saveFrames_btn;
        private Panel panel6;
        private FormButtons BrowseOverlay_btn;
        private Panel panel9;
        private GroupBox captureExpGain_gbx;
        private TableLayoutPanel tableLayoutPanel6;
        private NumericUpDown flashGain_nud;
        private Label label33;
        private Label label32;
        private NumericUpDown flashExp_nud;
        private TableLayoutPanel tableLayoutPanel7;
        private FormButtons connect_btn;
        private TableLayoutPanel tableLayoutPanel8;
        private Label label34;
        private TableLayoutPanel tableLayoutPanel12;
        private Label trigger_lbl;
        private ComboBox cameraModel_cmbx;
        private TableLayoutPanel tableLayoutPanel21;
        private ComboBox mode_cmbx;
        private Label label51;
        private TableLayoutPanel tableLayoutPanel22;
        private RadioButton CCMode_rb;
        private RadioButton rawModeFrameGrab_rb;
        private GroupBox groupBox1;
        private TableLayoutPanel tableLayoutPanel10;
        private Label label43;
        private ComboBox singleFrameCapture_combox;
        private RadioButton ir_rb;
        private FormCheckBox flashBoost_cbx;
        private NumericUpDown flashBoost_nud;
        private Label label44;
        private FormCheckBox postProcessing_cbx;
        private PictureBox triggerRecieved_pbx;
        private RadioButton flash_rb;
        private RadioButton blue_rb;
        private Panel panel2;
        private FormButtons startFFATimer_btn;
        private TableLayoutPanel tableLayoutPanel11;
        private NumericUpDown IrOffsetSteps_nud;
        private Label label6;
        private Label captureFocusSteps_lbl;
        private FormCheckBox MotorForward_cbx;
        private FormCheckBox greenIRLive_cbx;
        private NumericUpDown flashOffset2_nud;
        private NumericUpDown flashoffset1_nud;
        private FormCheckBox contCapture_cbx;
        private Panel panel7;
        private FormTextBox PatientName_tbx;
        private Label label24;
        private TabPage postProcessing_tbp;
        private FormCheckBox formCheckBox1;
        private FormCheckBox EnableTemperatureTint_cbx;
        private GroupBox tempTint_gbx;
        private TableLayoutPanel tableLayoutPanel9;
        private TrackBar temperature_tb;
        private TrackBar tint_tb;
        private Label label35;
        private Label label36;
        private Label TemperatureVal_lbl;
        private Label tintVal_lbl;
        private LinkLabel linkLabel4;
        private LinkLabel linkLabel3;
        private LinkLabel linkLabel2;
        private LinkLabel linkLabel1;
        private LinkLabel contrastSettings_lnklbl;
        private LinkLabel brightnessSettings_lnklbl;
        private LinkLabel maskSettings_lnkLbl;
        private LinkLabel ccSettings_lnkLbl;
        private CheckBox ApplyLiteCorrection_cbx;
        private LinkLabel hsSettings_lnkLbl;
        private CheckBox ApplyContrast_cbx;
        private LinkLabel shiftSettings_lnkLbl;
        private CheckBox applyBrightness_cbx;
        private Button button3;
        private Button PostProcessing_btn;
        private Button reloadImage_btn;
        private Button browseRawImage_btn;
        private FormButtons getRaw_btn;
        private FormCheckBox ApplyHSVBoost_cbx;
        private FormCheckBox applyClahe_cbx;
        private FormCheckBox isApplyLut_cbx;
        private FormCheckBox applyMask_cbx;
        private FormCheckBox applyColorCorrection_cbx;
        private FormCheckBox applyHotSpotCorrection_cbx;
        private FormCheckBox applyShift_cbx;
        private FormButtons saveSettings_btn;
        private TabPage shiftImage_tbp;
        private Button ApplyShiftImage_btn;
        private Label label21;
        private Label label20;
        private NumericUpDown Yshift_nud;
        private NumericUpDown xShift_nud;
        private Label label19;
        private TabPage ColorCorrection_tbp;
        private TableLayoutPanel tableLayoutPanel20;
        private Label label49;
        private Label label50;
        private NumericUpDown brightness_nud;
        private NumericUpDown contrast_nud;
        private NumericUpDown offset1_nud;
        private NumericUpDown LUT_interval2_nud;
        private NumericUpDown LUT_interval1_nud;
        private NumericUpDown LUT_SineFactor_nud;
        private NumericUpDown hsvBoostVal_nud;
        private NumericUpDown medianfilter_nud;
        private Label label71;
        private Label label72;
        private Label label70;
        private Label label69;
        private Label label13;
        private Label label57;
        private Label label55;
        private NumericUpDown unsharpAmount_nud;
        private NumericUpDown unsharpRadius_nud;
        private NumericUpDown unsharpThresh_nud;
        private NumericUpDown ClaheClipValueR_nud;
        private Label label56;
        private Label label53;
        private Label label54;
        private Label label22;
        private Label label23;
        private NumericUpDown ClaheClipValueG_nud;
        private NumericUpDown ClaheClipValueB_nud;
        private Button showLut_btn;
        private Button ApplyCC_btn;
        private Label label4;
        private Label label8;
        private Label label7;
        private Label label5;
        private NumericUpDown blueGreen_nud;
        private NumericUpDown greenGreen_nud;
        private NumericUpDown redGreen_nud;
        private NumericUpDown blueBlue_nud;
        private NumericUpDown blueRed_nud;
        private NumericUpDown greenBlue_nud;
        private NumericUpDown greenRed_nud;
        private NumericUpDown redBlue_nud;
        private NumericUpDown redRed_nud;
        private TabPage Mask_tbp;
        private Label label74;
        private Button batchMask_btn;
        private Button ClearMaskMarking_btn;
        private Button applyMask_btn;
        private Label label3;
        private NumericUpDown LiveMaskHeight_nud;
        private NumericUpDown LiveMaskWidth_nud;
        private NumericUpDown maskY_nud;
        private NumericUpDown maskX_nud;
        private TabPage CentreImage_tbp;
        private Button setCentreImage_btn;
        private TabPage misc_tbpg;
        private Panel panel3;
        private Panel panel4;
        private TableLayoutPanel tableLayoutPanel14;
        private GroupBox groupBox2;
        private FormCheckBox vFlip_cbx;
        private FormCheckBox hFlip_cbx;
        private GroupBox groupBox3;
        private TableLayoutPanel tableLayoutPanel15;
        private RadioButton left_rb;
        private RadioButton right_rb;
        private GroupBox motorSensor_gbx;
        private TableLayoutPanel tableLayoutPanel16;
        private Panel positivePos_p;
        private Panel negativePos_p;
        private Panel panel8;
        private FormButtons saveLiveFrame_btn;
        private FormButtons applyOverlayGrid_btn;
        private FormButtons unsharp_btn;
        private FormButtons createLUT_btn;
        private TableLayoutPanel tableLayoutPanel23;
        private TrackBar redGain_tb;
        private Label label9;
        private Label redGainVal_lbl;
        private TableLayoutPanel tableLayoutPanel24;
        private TrackBar greenGain_tb;
        private Label label10;
        private Label greenGainVal_lbl;
        private TableLayoutPanel tableLayoutPanel25;
        private TrackBar blueGain_tb;
        private Label label11;
        private Label blueGainVal_lbl;
        private TableLayoutPanel tableLayoutPanel26;
        private TrackBar liveRedGain_tb;
        private Label label12;
        private Label liveR_val_lbl;
        private TableLayoutPanel tableLayoutPanel27;
        private TrackBar liveGreenGain_tb;
        private Label label58;
        private Label liveG_val_lbl;
        private TableLayoutPanel tableLayoutPanel28;
        private TrackBar liveBlueGain_tb;
        private Label label60;
        private Label liveB_val_lbl;
        private TabPage ffa_tbpg;
        private TableLayoutPanel tableLayoutPanel18;
        private TableLayoutPanel tableLayoutPanel19;
        private Label label46;
        private Label label47;
        private Panel panel5;
        private RadioButton eightBit_rb;
        private RadioButton forteenBit_rb;
        private TableLayoutPanel tableLayoutPanel33;
        private Label label75;
        private Label label76;
        private NumericUpDown frameDetectionVal_nud;
        private NumericUpDown darkFameDetectionVal_nud;
        private Panel panel11;
        private FormCheckBox SaveProcessedImage_cbx;
        private FormCheckBox SaveDebugImage_cbx;
        private FormCheckBox saveRaw_cbx;
        private FormCheckBox SaveIr_cbx;
        private FormCheckBox showExtViewer_cbx;
        private TabPage HotSpot_tbp;
        private FormButtons formButtons1;
        private FormButtons GetHoSpotParams_btn;
        private Label label66;
        private Label label67;
        private NumericUpDown shadowRedPeak_nud;
        private NumericUpDown shadowGreenPeak_nud;
        private NumericUpDown shadowBluePeak_nud;
        private Label label68;
        private NumericUpDown HsRedPeak_nud;
        private NumericUpDown HsGreenPeak_nud;
        private NumericUpDown HsBluePeak_nud;
        private NumericUpDown HsRedRadius_nud;
        private NumericUpDown HsGreenRadius_nud;
        private NumericUpDown HsBlueRadius_nud;
        private Label label59;
        private Label label61;
        private Label label62;
        private Label label63;
        private Label label64;
        private Label label65;
        private Label coOrdinates_lbl;
        private RadioButton twoX_zoom_rb;
        private RadioButton oneX_zoom_rb;
        private Label label2;
        private Label label1;
        private Label label15;
        private Label label17;
        private NumericUpDown HotSpotCentreY_nud;
        private NumericUpDown HotSpotCentreX_nud;
        private Button button2;
        private Button button1;
        private Button applyHotSpotCorrection_btn;
        private Label label18;
        private Button centreValues_btn;
        private Label label30;
        private NumericUpDown HSRad2_nud;
        private Label label31;
        private NumericUpDown HSRad1_nud;
        private Label label16;
        private Label label14;
        private Label label37;
        private NumericUpDown ShadowRad2_nud;
        private Label label25;
        private NumericUpDown ShadowRad1_nud;
        private Panel panel12;
        private PictureBox powerStatus_pbx;
        private PictureBox cameraStatus_pbx;
        private ToolStripStatusLabel ComPortStatus_lbl;
        private Label ffaTimerStatus_lbl;
        private Button button4;
        private FlowLayoutPanel flowLayoutPanel1;
        private FormCheckBox showOverlay_cbx;
        private Panel panel14;
        private Label label28;
        private Label label27;
        private NumericUpDown FFA_Color_Pot_Int_Offset_nud;
        private NumericUpDown FFA_Pot_Int_Offset_nud;
        private Label label40;
        private Label label38;
        private Label label39;
        private Label label29;
        private NumericUpDown CaptureMaskHeight_nud;
        private NumericUpDown CaptureMaskWidth_nud;
        private Label label52;
        private Label label73;
        private Label label41;
        private NumericUpDown numericUpDown3;
        private FormCheckBox LiveMask_cbx;
        private FormCheckBox liveCC_cbx;
        private FormCheckBox enableLivePostProcessing_cbx;
        private NumericUpDown greenFilterPos_nud;
        private NumericUpDown blueFilterPos_nud;
        private FormCheckBox applyGamma_cbx;
        private TableLayoutPanel tableLayoutPanel29;
        private NumericUpDown gammaVal_nud;
        private Label label48;
        private Button hotSpotValues_btn;
        private Label label45;
        private NumericUpDown saveFramesCount_nud;
        private TableLayoutPanel tableLayoutPanel13;
        private FormButtons colorChannel_btn;
        private FormButtons redChannel_btn;
        private FormButtons greenChannel_btn;
        private FormButtons blueChannel_btn;
        private Label label42;
        private TableLayoutPanel tableLayoutPanel30;
        private TableLayoutPanel tableLayoutPanel31;
        private TableLayoutPanel tableLayoutPanel34;
        private RadioButton liveAnt_IR_rb;
        private GroupBox groupBox4;
        private RadioButton liveAnt_Flash_rb;
        private RadioButton liveAnt_Blue_rb;
        private TableLayoutPanel tableLayoutPanel17;
        private RadioButton captureIR_rb;
        private RadioButton captureFlash_rb;
        private RadioButton captureBlue_rb;
        private RadioButton captureAnt_IR_rb;
        private RadioButton captureAnt_Flash_rb;
        private RadioButton captureAnt_Blue_rb;
        private Button button5;
        private Label LeftRight_lbl;
        private TabPage allChannelLut_tbp;
        private TableLayoutPanel channnelsTbpl;
        private TableLayoutPanel redChannelTblPnl;
        private TableLayoutPanel greenChannelTbpl;
        private TableLayoutPanel blueChannelTbpl;
        private TableLayoutPanel redKutSettingTbpl;
        private Label sineFactorRedChannelLbl;
        private Label interval1RedChannelLbl;
        private Label Interval2RedChannelLbl;
        private Label lutOffsetRedChannelLbl;
        private Label redChannelLutLbl;
        private TableLayoutPanel greenLutSettingsTbpl;
        private Label greenChannelLutLbl;
        private TableLayoutPanel blueLutSettingsTbpl;
        private Label blueChannelLutLbl;
        private NumericUpDown redChannelSineFactor_nud;
        private NumericUpDown redChannelLutInterval1_nud;
        private NumericUpDown redChannelLutInterval2_nud;
        private NumericUpDown redChannelLutOffset_nud;
        private Label greenChannelSineFactorLbl;
        private Label interval1GreenChannelLbl;
        private Label interval2GreenChannelLbl;
        private Label lutOffsetGreenChannelLbl;
        private NumericUpDown greenChannelSineFactor_nud;
        private NumericUpDown greenChannelLutInterval1_nud;
        private NumericUpDown greenChannelLutInterval2_nud;
        private NumericUpDown greenChannelLutOffset_nud;
        private Label blueChannelSineFactorLbl;
        private Label interval1BlueChannelLbl;
        private Label interval2BlueChannelLbl;
        private Label lutOffsetBlueChannelLbl;
        private NumericUpDown blueChannelSineFactor_nud;
        private NumericUpDown blueChannelLutInterval1_nud;
        private NumericUpDown blueChannelLutInterval2_nud;
        private NumericUpDown blueChannelLutOffset_nud;
        private TableLayoutPanel showLutTbpl;
        private Button showAllChannelBtn;
        private CheckBox allChannelLutCbx;
        private LinkLabel applyChannelWiseLutLklbl;
        private Button button6;
    }
}




