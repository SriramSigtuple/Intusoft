using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using INTUSOFT.Data.Repository;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.InteropServices;
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
using INTUSOFT.Data.Repository;
using INTUSOFT.Data.NewDbModel;
using NLog.Config;
using NLog.Targets;
using NLog;
using Cloud_Models.Models;
using Common;
using Newtonsoft.Json;

namespace INTUSOFT.Desktop.Forms
{
    public partial class LiveImageControls_UC : UserControl
    {
        static Logger exceptionLog = LogManager.GetLogger("ExceptionLog"); 
         ImageSaveHelper img;
        IVLEventHandler eventHandler;
        Args arg = new Args();
        public delegate void FrameCaptured(string FileName, EventArgs e);

        delegate void enableControlsDelegate(bool isEnable);
        enableControlsDelegate mEnableControlsDelegate;
        LiveImageControlsPrime_UC _livePrimeControls;
        LiveImageControls45_UC _liveFortyFiveControls;
        GainExposureHelper GainExposureHelper;
        EventArgs e = null;
        public event FrameCaptured _frameCaptured;
        public delegate void StatusBarUpdate();
        public event StatusBarUpdate _statusBarUpdate;
        public Dictionary<string, string> patDetails;

        //int FrameRate = 0;
        //int tempVal = 6500;
        //int tintVal = 1000;
        int maxExposureValue = 77500;
        int exposureIncrementValue = 2500;
        int minExposureValue = 2500;
        public bool isLive = false;
        public bool isPower = false;
        public delegate void ResetMotorDelegate();
        public event ResetMotorDelegate resetMotorEvent;
        LiveImageControls_UC _liveimageControls;

        Custom.Controls.FormButtons prime_btn;
        Custom.Controls.FormButtons antirior_btn;
        Custom.Controls.FormButtons ffaColor_btn;
        Custom.Controls.FormButtons fortyFiveMode_btn;
        Custom.Controls.FormButtons ffa_btn;
        string captureBtnText = "";
        string resumeBtnText = "";
      
        public LiveImageControls_UC()
        {
                InitializeComponent();
                img = ImageSaveHelper.GetInstance();
            prime_btn = new FormButtons();
            prime_btn.BackColor = Color.Khaki;
            prime_btn.ForeColor = SystemColors.ActiveCaptionText;
            prime_btn.Dock = DockStyle.Fill;

            prime_btn.Click += posterior_btn_Click;

            this.DoubleBuffered = true;
            antirior_btn = new FormButtons();
            antirior_btn.BackColor = Color.Khaki;
            antirior_btn.Dock = DockStyle.Fill;

            antirior_btn.ForeColor = SystemColors.ActiveCaptionText;

            antirior_btn.Click += antirior_btn_Click;

            ffa_btn = new FormButtons();
            ffa_btn.BackColor = Color.Khaki;
            ffa_btn.Dock = DockStyle.Fill;

            ffa_btn.ForeColor = SystemColors.ActiveCaptionText;

            ffa_btn.Click += ffa_btn_Click;

            ffaColor_btn = new FormButtons();
            ffaColor_btn.Dock = DockStyle.Fill;

            ffaColor_btn.BackColor = Color.Khaki;
            ffaColor_btn.ForeColor = SystemColors.ActiveCaptionText;

            ffaColor_btn.Click += ffaColor_btn_Click;

            fortyFiveMode_btn = new FormButtons();
            fortyFiveMode_btn.Dock = DockStyle.Fill;
            fortyFiveMode_btn.BackColor = Color.Khaki;
            fortyFiveMode_btn.ForeColor = SystemColors.ActiveCaptionText;

            fortyFiveMode_btn.Click += fortyFiveMode_btn_Click;

            //Control_tbLt.RowStyles[2] = new RowStyle(SizeType.Percent,10f);
            //Control_tbLt.RowStyles[1] = new RowStyle(SizeType.Percent,10f);
            Control_tbLt.RowStyles[0] = new RowStyle(SizeType.Percent,10f);
            Control_tbLt.RowStyles[3] = new RowStyle(SizeType.Percent,10f);

            InitializeResourceString();
            //MotorSteps_tb.Minimum = Convert.ToInt32(IVLVariables.CurrentSettings.FirmwareSettings._MotorCaptureSteps.min);//This line has been added by darshan to set the minimum value for the motor steps with the minimum value in the firmware settings.
            //MotorSteps_tb.Maximum = Convert.ToInt32(IVLVariables.CurrentSettings.FirmwareSettings._MotorCaptureSteps.max);
            //MotorSteps_tb.Value = 20;
            eventHandler = IVLEventHandler.getInstance();
            eventHandler.Register(eventHandler.FrameCaptured, new NotificationHandler(FrameCaptureDone));
            eventHandler.Register(eventHandler.CaptureEvent, new NotificationHandler(captureEvent));
            eventHandler.Register(eventHandler.ImageAdded, new NotificationHandler(imageAdded));
            eventHandler.Register(eventHandler.ChangeLeftRightPos_Live, new NotificationHandler(ChangeLeftRightPosition));
            eventHandler.Register(eventHandler.UpdateFFATime, new NotificationHandler(updateFFATime));
            eventHandler.Register(eventHandler.ImageUrlToDb, new NotificationHandler(saveImageURlDb));
           
            IVLVariables.ivl_Camera._EnableControls += ivl_Camera__EnableControls;
            mEnableControlsDelegate = new enableControlsDelegate(ivl_Camera__EnableControls);
            // liveGain = Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._LiveDigitalGain.val);
            //flashGain = Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._DigitalGain.val);
            //eventHandler.Register(eventHandler.triggerRecieved, new NotificationHandler(TriggerRecieved));
            //eventHandler.Register(eventHandler.MotorForwardDone, new NotificationHandler(MotorForwardDone));
            //eventHandler.Register(eventHandler.MotorBackwardDone, new NotificationHandler(MotorBackwardDone));
            // ConnectCamera();
            // temperatureVal_lbl.Text = IVLVariables.CurrentSettings.temperature.ToString();
            left_btn.BackColor = Color.Yellow;
            _livePrimeControls = LiveImageControlsPrime_UC.getInstance();
            _liveFortyFiveControls = LiveImageControls45_UC.getInstance();
            GainExposureHelper = GainExposureHelper.getInstance();
            SetUIFromConfigSettings();

           captureBtnText = IVLVariables.LangResourceManager.GetString("Capture_Button_Text", IVLVariables.LangResourceCultureInfo);
           resumeBtnText = IVLVariables.LangResourceManager.GetString("Resume_Button_Text", IVLVariables.LangResourceCultureInfo);
            ChangedButtonSize();
            SetDoubleBuffered(tableLayoutPanel9);
            foreach (Control c in this.Control_tbLt.Controls)
            {
                        if (Screen.PrimaryScreen.Bounds.Width == 1920)
                            c.Font = new Font(c.Font.FontFamily.Name, 12f);
                        else if (Screen.PrimaryScreen.Bounds.Width == 1366)
                            c.Font = new Font(c.Font.FontFamily.Name, 10f);
                        else
                            c.Font = new Font(c.Font.FontFamily.Name, 9f);
            }
            if (Screen.PrimaryScreen.Bounds.Width == 1920)
            {
                tableLayoutPanel9.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 40f);
                tableLayoutPanel9.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 60f);
            }
            else if (Screen.PrimaryScreen.Bounds.Width == 1366)
            {
                tableLayoutPanel9.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 35f);
                tableLayoutPanel9.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 65f);
            }
            liveSource_Gbx.ForeColor = IVLVariables.GradientColorValues.FontForeColor;
            ImagingMode_gbx.ForeColor = IVLVariables.GradientColorValues.FontForeColor;
            this.Focus();
        }

        void ivl_Camera__EnableControls(bool isEnabled)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(mEnableControlsDelegate,isEnabled);
            }
            else
            this.Enabled = isEnabled;
           // this.Parent.Parent.UseWaitCursor = true;
        }


        public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {
            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                return;
            System.Reflection.PropertyInfo aProp = typeof(System.Windows.Forms.Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            aProp.SetValue(c, true, null);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        void updateFFATime(string s, Args a)
        {
            ffaTimerStatus_lbl.Text = "FFA Time : " + (string)a["ffaTime"];
        }
        public void ChangedButtonSize()
        {

            List<FormButtons> buttonList = new List<FormButtons>(); 
            int buttonCnt = 0;
            if (prime_btn.Visible)
                buttonList.Add(prime_btn);
            if (antirior_btn.Visible)
                buttonList.Add(antirior_btn);

            if (ffa_btn.Visible)
                buttonList.Add(ffa_btn);

            if (ffaColor_btn.Visible)
                buttonList.Add(ffaColor_btn);

            if (fortyFiveMode_btn.Visible)
                buttonList.Add(fortyFiveMode_btn);

            int rowCnt = 0;
            tableLayoutPanel1.RowStyles.RemoveAt(0);
            for (int i = 0; i < buttonList.Count; i++)
            {
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent,100f/buttonList.Count));
                tableLayoutPanel1.Controls.Add(buttonList[i], i, i);
            }
            //if(fortyFiveMode_btn.Visible)
            //{
            //    if(buttonCnt < 2)
            //{
            //    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            // //   tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f/3f));
            //   // tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f/3f));

            //}
            //else
            //{
            //    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent,100f/(float)buttonCnt));
            //}

            //    tableLayoutPanel1.Controls.Add(fortyFiveMode_btn, 0, tableLayoutPanel1.RowStyles.Count - 1);
            //}

            // if(antirior_btn.Visible)
            //{
            //    if(buttonCnt < 2)
            //{
            //    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f/3f));
            //    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f/3f));
            //    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f/3f));

            //}
            //else
            //{
            //    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent,100f/(float)buttonCnt));
            //}

            //    tableLayoutPanel1.Controls.Add(antirior_btn, 0, tableLayoutPanel1.RowStyles.Count - 1);
            //}

            // if(ffaColor_btn.Visible)
            //{
            //    if(buttonCnt < 2)
            //{
            //    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f/3f));
            //    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f/3f));
            //    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f/3f));

            //}
            //else
            //{
            //    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent,100f/(float)buttonCnt));
            //}

            //    tableLayoutPanel1.Controls.Add(ffaColor_btn, 0, tableLayoutPanel1.RowStyles.Count - 1);
            //}

            // if(ffa_btn.Visible)
            //{
            //    if(buttonCnt < 2)
            //{
            //    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f/3f));
            //    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f/3f));
            //    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f/3f));

            //}
            //else
            //{
            //    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent,100f/(float)buttonCnt));
            //}

            //    tableLayoutPanel1.Controls.Add(ffa_btn, 0, tableLayoutPanel1.RowStyles.Count - 1);
            //}

            // if(prime_btn.Visible)
            //{
            //    if(buttonCnt < 2)
            //{
            //   tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f/3f));
            //    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f/3f));
            //    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f/3f));

            //}
            //else
            //{
            //    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent,100f/(float)buttonCnt));
            //}
                
            //    tableLayoutPanel1.Controls.Add(prime_btn,0,tableLayoutPanel1.RowStyles.Count-1);
            //}



        }

        public LiveImageControls_UC getInstance()
        {
            _liveimageControls = this;
            return _liveimageControls;
        }

        void IntucamBoardCommHelper__leftRightEvent(bool isLeft)
        {
            if (isLeft)
            {
                left_btn.BackColor = Color.Yellow;
                right_btn.BackColor = Color.Khaki;
            }
            else
            {
                left_btn.BackColor = Color.Khaki;
                right_btn.BackColor = Color.Yellow;
            }
        }

        
        private void SetUIFromConfigSettings()
        {
            capture_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.LiveImaging._CaptureBtnVisible.val);
            right_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.LiveImaging._RightLeftVisble.val);
            left_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.LiveImaging._RightLeftVisble.val);
            FlashLight_rbx.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.LiveImaging._FlashBtnVisible.val);
            IRlight_rbx.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.LiveImaging._IRBtnVisible.val);

            


            prime_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.LiveImaging._PosteriorVisible.val);
            antirior_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.LiveImaging._AnteriorVisible.val);
            ffaColor_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.LiveImaging.FfaColorVisible.val);
            ffa_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.LiveImaging.FfaVisible.val);
            fortyFiveMode_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.LiveImaging.FortyFiveButtonVisible.val);
            ffaTimerStatus_lbl.Visible = startFFATimer_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.LiveImaging.StartFFATimerButtonVisible.val);
        }

        private void ResetImagingButtons()
        {
            antirior_btn.BackColor = Color.Khaki;
            prime_btn.BackColor = Color.Khaki;
            ffa_btn.BackColor = Color.Khaki;
            ffaColor_btn.BackColor = Color.Khaki;
            fortyFiveMode_btn.BackColor = Color.Khaki;
            //ffaTimerStatus_lbl.Visible = false;

        }
        public void PosteriorAnteriorBtnSelection()
        {
            ResetImagingButtons();
            tableLayoutPanel3.Visible = false;

            //switch (IVLVariables._ivlConfig.Mode)
            switch (IVLVariables.ivl_Camera.camPropsHelper.ImagingMode)
            {
                default: prime_btn.BackColor = Color.Yellow;
                    break;
                case ImagingMode.Anterior_Prime:
                    {
                        antirior_btn.BackColor = Color.Yellow;
                        break;
                    }
                case ImagingMode.Anterior_45:
                    antirior_btn.BackColor = Color.Yellow;
                    break;
                case ImagingMode.Anterior_FFA:
                    antirior_btn.BackColor = Color.Yellow;
                    break;
                case ImagingMode.FFA_Plus:
                    ffa_btn.BackColor = Color.Yellow;
                    ffaTimerStatus_lbl.Visible = true;
                    break;
                case ImagingMode.FFAColor:
                    {
                        ffaColor_btn.BackColor = Color.Yellow;
                        tableLayoutPanel3.Visible = true;

                        break;
                    }
                case ImagingMode.Posterior_45:
                    {
                        fortyFiveMode_btn.BackColor = Color.Yellow;
                        tableLayoutPanel3.Visible = true;

                        break;
                    }
                case ImagingMode.Posterior_Prime:
                    {
                        prime_btn.BackColor = Color.Yellow;
                        break;
                    }
            }
        }

        private void InitializeResourceString()
        {
            capture_btn.Text = IVLVariables.LangResourceManager.GetString("Capture_Button_Text", IVLVariables.LangResourceCultureInfo);
            FlashLight_rbx.Text = IVLVariables.LangResourceManager.GetString("Flash_Button_Text", IVLVariables.LangResourceCultureInfo);
            IRlight_rbx.Text = IVLVariables.LangResourceManager.GetString("IR_Button_Text", IVLVariables.LangResourceCultureInfo);
            left_btn.Text = IVLVariables.LangResourceManager.GetString("ImageViewer_leftSide_Button_Text", IVLVariables.LangResourceCultureInfo);
            right_btn.Text = IVLVariables.LangResourceManager.GetString("ImageViewer_rightSide_Button_Text", IVLVariables.LangResourceCultureInfo);
            info_lbl.Text = IVLVariables.LangResourceManager.GetString("IntensityToolTipTitle_Text", IVLVariables.LangResourceCultureInfo);
            InfoIcon_pbx.Image = SystemIcons.Information.ToBitmap();
            int1_lbl.Text = IVLVariables.LangResourceManager.GetString("MydMode_Text", IVLVariables.LangResourceCultureInfo);
            int3_lbl.Text = IVLVariables.LangResourceManager.GetString("OpticDisc_Text", IVLVariables.LangResourceCultureInfo);
            int4_lbl.Text = IVLVariables.LangResourceManager.GetString("Val1", IVLVariables.LangResourceCultureInfo);
            int5_lbl.Text = IVLVariables.LangResourceManager.GetString("CentralRetina_Text", IVLVariables.LangResourceCultureInfo);
            int6_lbl.Text = IVLVariables.LangResourceManager.GetString("Val2", IVLVariables.LangResourceCultureInfo);
            int7_lbl.Text = IVLVariables.LangResourceManager.GetString("PeripheralRetina_Text", IVLVariables.LangResourceCultureInfo);
            int8_lbl.Text = IVLVariables.LangResourceManager.GetString("Val3", IVLVariables.LangResourceCultureInfo);
            int9_lbl.Text = IVLVariables.LangResourceManager.GetString("NonMydMode_Text", IVLVariables.LangResourceCultureInfo);
            int12_lbl.Text = IVLVariables.LangResourceManager.GetString("CentralRetina_Text", IVLVariables.LangResourceCultureInfo);
            int13_lbl.Text = IVLVariables.LangResourceManager.GetString("Val3", IVLVariables.LangResourceCultureInfo);
            prime_btn.Text = IVLVariables.LangResourceManager.GetString("Posterior_Btn_Text", IVLVariables.LangResourceCultureInfo);
            antirior_btn.Text = IVLVariables.LangResourceManager.GetString("Anterior_Btn_Text", IVLVariables.LangResourceCultureInfo);
            ffa_btn.Text = IVLVariables.LangResourceManager.GetString("FFA_Text", IVLVariables.LangResourceCultureInfo);
            ffaColor_btn.Text = IVLVariables.LangResourceManager.GetString("FFA_Color_Text", IVLVariables.LangResourceCultureInfo);
            fortyFiveMode_btn.Text = IVLVariables.LangResourceManager.GetString("Posterior_Btn_Text", IVLVariables.LangResourceCultureInfo);
            startFFATimer_btn.Text = IVLVariables.LangResourceManager.GetString("startFFA_Text", IVLVariables.LangResourceCultureInfo);
            liveSource_Gbx.Text = IVLVariables.LangResourceManager.GetString("LiveSource_Gbx_Text", IVLVariables.LangResourceCultureInfo);
            ImagingMode_gbx.Text = IVLVariables.LangResourceManager.GetString("ImagingMode_txt", IVLVariables.LangResourceCultureInfo);
            blueLight_rb.Text = IVLVariables.LangResourceManager.GetString("BlueFilter_Text", IVLVariables.LangResourceCultureInfo);
        }

        void captureEvent(string s, Args arg)
        {
            saveFrames();
        }

        void FrameCaptureDone(string s, Args arg)
        {
            if (IVLVariables.ivl_Camera.IsCaptureFailure)// if capture failure happens pop up a message of the capture failure category and resume live
            {
                isLive = false;
                CaptureFailureForm c = new CaptureFailureForm();
                c.TopMost = true;
                string msg = "";
                if (arg.ContainsKey("Capture Failure Category"))
                    msg = arg["Capture Failure Category"] as string;
                c.SetErrorMsg(msg);
                c.Location = new Point(this.Location.X + this.Width / 2, this.Location.Y );
                c.Show();
                c.Refresh();
                if (IVLVariables.ivl_Camera.camPropsHelper.ImagingMode == ImagingMode.FFA_Plus)
                    Thread.Sleep(1000);
                else
                    Thread.Sleep(2000);

                c.Close();
                    ResumeLive();
            }
            else // if capture is success save the image url to database and resume live
            {
                eventHandler.Notify(eventHandler.ImageUrlToDb, arg);
                ResumeLive();
            }
        }

       internal void CameraPowerDisconnectedInLive(string s , Args arg)
        {
            CaptureFailureForm c = new CaptureFailureForm();
            c.TopMost = true;
            string msg = "";
            if (arg.ContainsKey("FailureCode"))
                msg = arg["FailureCode"] as string;
            c.SetErrorMsg(msg);
            c.Show();
            c.Refresh();
            Thread.Sleep(2000);
            c.Close();

        }
        /// <summary>
        /// This event will save the image url into a imagemodel variable and save the image details into json file.This also modifies the patient and visit modify datetime and update it.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="arg"></param>
       private void saveImageURlDb(string s, Args arg)
       {
           try
           {
               {
                   if (File.Exists(IVLVariables.ivl_Camera.camPropsHelper.ImageSavedPath))//This condition has been added to save the image details into the db only if it is save in the hard drive.
                   {
                       ThumbnailModule.ThumbnailData tdata = new ThumbnailModule.ThumbnailData();
                       char eyeSide = new char();

                       tdata.fileName = IVLVariables.ivl_Camera.camPropsHelper.ImageSavedPath;

                       if (!arg.ContainsKey("isModifiedimage"))
                       {
                           arg["isModifiedimage"] = false;
                           tdata.isModified = false;
                       }
                       else
                           tdata.isModified = (bool)arg["isModifiedimage"];

                       if (!tdata.isModified)
                       {
                           if (IVLVariables.ivl_Camera.camPropsHelper.LeftRightPos == LeftRightPosition.Left)
                           {
                               tdata.side = 1;
                               eyeSide = 'L';
                           }
                           else
                           {
                               tdata.side = 0;
                               eyeSide = 'R';
                           }
                       }
                       else
                       {
                           tdata.side = (int)arg["side"];
                           if (tdata.side == 1)
                               eyeSide = 'L';
                           else
                               eyeSide = 'R';

                       }

                       //string[] imagePath = (arg["ImageName"] as string).Split('\\');

                       string imageName = IVLVariables.ivl_Camera.camPropsHelper.ImageName;
                       if (!IVLVariables.isCommandLineAppLaunch)
                       {

                           users creator = users.CreateNewUsers();
                           creator.userId = 1;
                          
                           obs newObs = obs.CreateNewObs();
                           Concept concept = Concept.CreateNewConcept();
                           concept.conceptId = 3;
                           //Added to save the machine data in the column machineID has to be removed once the machine details interface has been added.
                           machine machine_id = machine.CreateNewMachine();
                           machine_id.machineId = 1;

                           eye_fundus_image eyeFundusImage = eye_fundus_image.CreateNewEyeFundusImage();

                           eyeFundusImage.eyeSide = eyeSide;
                           eyeFundusImage.machine = machine_id;
                           

                           if (arg.ContainsKey("cameraSettings"))
                               eyeFundusImage.cameraSetting = arg["cameraSettings"] as string;
                           if (arg.ContainsKey("maskSettings"))
                               eyeFundusImage.maskSetting = arg["maskSettings"] as string;
                           Patient pat = NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0];
                           eyeFundusImage.value = imageName;
                           eyeFundusImage.createdBy = creator;
                           eyeFundusImage.concept = concept;
                           eyeFundusImage.createdDate = DateTime.Now;
                           eyeFundusImage.lastModifiedDate = DateTime.Now;
                           eyeFundusImage.patient = pat;
                           eyeFundusImage.visit = NewDataVariables.Active_Visit;
                          //if( string.IsNullOrEmpty (eyeFundusImage.checkSum))
                          //  {
                          //   eyeFundusImage.checkSum =   Common.MD5Helper.GetMd5Hash(new FileInfo(Path.Combine(IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val, imageName)));
                          //  }
                           //newObs.eye_fundus_image = eyeFundusImage;
                           //eyeFundusImage.obs_id = newObs;

                           //eyeFundusImage.eye_fundus_image_id = newObs.observationId;
                           pat.patientLastModifiedDate = DateTime.Now;
                           pat.observations.Add(eyeFundusImage);
                           NewDataVariables.Active_Visit.observations.Add(eyeFundusImage);
                           NewDataVariables.Active_Visit.lastModifiedDate = DateTime.Now;
                           pat.visits.Where(x => x == NewDataVariables.Active_Visit).ToList()[0] = NewDataVariables.Active_Visit;
                           NewDataVariables.Patients[NewDataVariables.Patients.FindIndex(x => x.personId == NewDataVariables.Active_Patient)] = pat;

                          
                            eyeFundusImage.qiFileName =  Push2QI(eyeFundusImage);
                            NewIVLDataMethods.UpdatePatient();
                            NewDataVariables.Active_Obs = pat.visits.Where(x => x == NewDataVariables.Active_Visit).ToList()[0].observations.ToList()[0];
                            NewDataVariables.Obs.Add(eyeFundusImage);
                            tdata.id = eyeFundusImage.observationId;
                            // arg["id"] = newObs.observationId;
                            //foreach (var item in NewDataVariables.Visits)
                            //{
                            //    visitDate.Add(item.createdDate);
                            //}
                            //DataTable d = NewDataVariables.Visits.ToDataTable();
                            //d.Columns.Add(IVLVariables.LangResourceManager.GetString("VisitsAddImages_ColName", IVLVariables.LangResourceCultureInfo), typeof(Button));
                            //SetVisitData(d);
                            //clearVisitGrid();

                            //if (eyeFundusImage.eyeSide == 'L')
                            //{
                            //    tdata.side = 1;
                            //    arg["side"] = 1;
                            //}
                            //else
                            //{
                            //    tdata.side = 0;
                            //    arg["side"] = 0;
                            //} 
                            //if (!arg.ContainsKey("isModifiedimage"))
                            //    arg["isModifiedimage"] = false;
                        }
                       else
                       {
                           //if (eyeSide == 'L')
                           //    arg["side"] = 1;
                           //else
                           //    arg["side"] = 0;
                           tdata.id = IVLVariables.CmdObsID;
                           //arg["id"] = IVLVariables.CmdObsID;
                           tdata.isModified = false;
                           arg["isModifiedimage"] = false;
                           IVLVariables.CmdObsID++;

                       }
                       arg["ThumbnailData"] = tdata;
                       eventHandler.Notify(eventHandler.ThumbnailAdd, arg);
                   }
               }
           }
           catch (Exception ex)
           {
               Common.ExceptionLogWriter.WriteLog(ex, exceptionLog);
               //                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
           }
       }
        private CloudModel GetQIModel(eye_fundus_image eye_Fundus_Image)
        {
            //NewDataVariables.Active_Visit.reports.Add(report);
            //NewIVLDataMethods.AddReport(report);
            //NewIVLDataMethods.UpdateVisit();

            CloudModel cloudModel = new CloudModel();
            cloudModel.visitID = NewDataVariables.Active_Visit.visitId;
            cloudModel.patientID = NewDataVariables.Active_Patient;
            cloudModel.ImageID = eye_Fundus_Image.eye_fundus_image_id;
            cloudModel.LoginModel.URL_Model.API_URL = IVLVariables.CurrentSettings.CloudSettings.API_URL.val;
            cloudModel.LoginModel.URL_Model.API_URL_Start_Point = IVLVariables.CurrentSettings.CloudSettings.API_LOGIN_URL.val;

            cloudModel.LoginModel.username = IVLVariables.CurrentSettings.CloudSettings.Username.val;
            cloudModel.LoginModel.password = IVLVariables.CurrentSettings.CloudSettings.Password.val;

            cloudModel.LoginModel.device_id = IVLVariables.CurrentSettings.CameraSettings.DeviceID.val;

            Dictionary<string, object> kvp = new Dictionary<string, object>();
            cloudModel.LoginModel.Body = string.Empty;
            LoginResponseModel reponse = new LoginResponseModel();
            reponse.message = new message();


            cloudModel.CreateAnalysisModel.URL_Model.API_URL = IVLVariables.CurrentSettings.CloudSettings.API_URL.val; ;
            cloudModel.CreateAnalysisModel.URL_Model.API_URL_Start_Point = IVLVariables.CurrentSettings.CloudSettings.API_ANALYSES.val; ;

            cloudModel.CreateAnalysisModel.sample_id = IVLVariables.MRN;
            cloudModel.CreateAnalysisModel.age = IVLVariables.patAge.ToString();
            cloudModel.CreateAnalysisModel.gender = IVLVariables.patGender[0].ToString();
            cloudModel.CreateAnalysisModel.sample_desc = IVLVariables.patAge.ToString() + "yrs, " + IVLVariables.patGender[0].ToString();
            cloudModel.CreateAnalysisModel.visitID = IVLVariables.ActiveVisitID;
            cloudModel.CreateAnalysisModel.PatientID = IVLVariables.ActivePatID;
            cloudModel.CreateAnalysisModel.AnalysisType = 1;

            cloudModel.CreateAnalysisModel.Body = string.Empty;

            //cloudModel.UploadModel.relative_path = cloudModel.CreateAnalysisModel.sample_id + Path.DirectorySeparatorChar + "LE" + Path.DirectorySeparatorChar + "UnsharpMask.jpg";
            cloudModel.UploadModel.URL_Model.API_URL = IVLVariables.CurrentSettings.CloudSettings.API_URL.val;
            cloudModel.UploadModel.URL_Model.API_URL_Start_Point = IVLVariables.CurrentSettings.CloudSettings.API_ANALYSES.val;
            cloudModel.UploadModel.URL_Model.API_URL_End_Point = IVLVariables.CurrentSettings.CloudSettings.API_ANALYSES_INPUT.val;
            cloudModel.UploadModel.RetryCount = 3;
            cloudModel.UploadModel.images = new string[] {Path.Combine( IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val ,eye_Fundus_Image.value)};
            cloudModel.UploadModel.checksums = new string[1];
            cloudModel.UploadModel.relative_path = new string[1];
            cloudModel.UploadModel.eyeSideArr = new string[] { eye_Fundus_Image.eyeSide.ToString() };
            for (int i = 0; i < cloudModel.UploadModel.images.Length; i++)
            {
                FileInfo ImgFinf = new FileInfo(cloudModel.UploadModel.images[i]);
                cloudModel.UploadModel.checksums[i] = "";
                cloudModel.UploadModel.relative_path[i] = IVLVariables.MRN + "/" + (cloudModel.UploadModel.eyeSideArr[i].Contains("R") ? "RE" : "LE") + "/" + ImgFinf.Name;
            }

            cloudModel.InitiateAnalysisModel.status = "initialised";

            cloudModel.InitiateAnalysisModel.URL_Model.API_URL = IVLVariables.CurrentSettings.CloudSettings.API_URL.val;
            cloudModel.InitiateAnalysisModel.URL_Model.API_URL_Start_Point = IVLVariables.CurrentSettings.CloudSettings.API_ANALYSES.val;
            cloudModel.InitiateAnalysisModel.URL_Model.API_URL_End_Point = IVLVariables.CurrentSettings.CloudSettings.API_STATUS_URL.val;

            cloudModel.InitiateAnalysisModel.Body = string.Empty;

            cloudModel.GetAnalysisModel.analysis_id = cloudModel.UploadModel.analysis_id;
            cloudModel.GetAnalysisModel.URL_Model.API_URL = IVLVariables.CurrentSettings.CloudSettings.API_URL.val;
            cloudModel.GetAnalysisModel.URL_Model.API_URL_Start_Point = IVLVariables.CurrentSettings.CloudSettings.API_ANALYSES.val;

            cloudModel.GetAnalysisResultModel.URL_Model.API_URL = IVLVariables.CurrentSettings.CloudSettings.API_URL.val;

            return cloudModel;
        }

        private string Push2QI(eye_fundus_image eye_Fundus)
        {
            CloudModel cloudModel =  GetQIModel(eye_Fundus);
            var cloudModelJson = JsonConvert.SerializeObject(cloudModel, Newtonsoft.Json.Formatting.Indented);
            var outboxFilePath = Path.Combine(IVLVariables.GetCloudDirPath(DirectoryEnum.OutboxDir, AnalysisType.QI), DateTime.Now.ToString("yyyymmddHHMMssfff") + ".json");
            File.WriteAllText(outboxFilePath, cloudModelJson);
            return new FileInfo(outboxFilePath).Name;
            // car = NewDataVariables._Repo.GetById<obs>(cloudModel.ImageID);
            //car.fileName = new FileInfo(outboxFilePath).Name;
            //NewDataVariables._Repo.Update<CloudAnalysisReport>(car);

        }
        //private void saveImageURlDb(string s, Args arg)
        //{
        //    try
        //    {
        //        {
        //            if (File.Exists(IVLVariables.ivl_Camera.camPropsHelper.ImageSavedPath))//This condition has been added to save the image details into the db only if it is save in the hard drive.
        //            {
        //                ThumbnailModule.ThumbnailData tdata = new ThumbnailModule.ThumbnailData();
        //                char eyeSide = new char();

        //                tdata.fileName = IVLVariables.ivl_Camera.camPropsHelper.ImageSavedPath;

        //                if (!arg.ContainsKey("isModifiedimage"))
        //                {
        //                    arg["isModifiedimage"] = false;
        //                    tdata.isModified = false;
        //                }
        //                else
        //                  tdata.isModified = (bool)  arg["isModifiedimage"];

        //                if (!tdata.isModified)
        //                {
        //                    if (IVLVariables.ivl_Camera.camPropsHelper.LeftRightPos == LeftRightPosition.Left)
        //                    {
        //                        tdata.side = 1;
        //                        eyeSide = 'L';
        //                    }
        //                    else
        //                    {
        //                        tdata.side = 0;
        //                        eyeSide = 'R';
        //                    }
        //                }
        //                else
        //                {
        //                    tdata.side = (int)arg["side"];
        //                    if (tdata.side == 1)
        //                        eyeSide = 'L';
        //                    else
        //                        eyeSide = 'R';
                          
        //                }

        //                //string[] imagePath = (arg["ImageName"] as string).Split('\\');

        //                string imageName = IVLVariables.ivl_Camera.camPropsHelper.ImageName;
        //                if (!IVLVariables.isCommandLineAppLaunch)
        //                {

        //                    users creator = users.CreateNewUsers();
        //                    creator.userId = 1;
        //                    obs newObs = obs.CreateNewObs();
        //                    Concept concept = Concept.CreateNewConcept();
        //                    concept.conceptId = 3;
        //                    //Added to save the machine data in the column machineID has to be removed once the machine details interface has been added.
        //                    machine machine_id = machine.CreateNewMachine();
        //                    machine_id.machineId = 1;

        //                    eye_fundus_image eyeFundusImage = eye_fundus_image.CreateNewEyeFundusImage();

        //                    eyeFundusImage.eyeSide = eyeSide;
        //                    eyeFundusImage.machine = machine_id;


        //                    if (arg.ContainsKey("cameraSettings"))
        //                        eyeFundusImage.cameraSetting = arg["cameraSettings"] as string;
        //                    if (arg.ContainsKey("maskSettings"))
        //                        eyeFundusImage.maskSetting = arg["maskSettings"] as string;
        //                    newObs.value = imageName;
        //                    newObs.createdBy = creator;
        //                    newObs.concept = concept;
        //                    newObs.createdDate = DateTime.Now;
        //                    newObs.lastModifiedDate = DateTime.Now;
        //                    newObs.patient = NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0];
        //                    newObs.visit = NewDataVariables.Active_Visit;
        //                    NewIVLDataMethods.AddImage(newObs);//will save the major details related to the image into the DB.
        //                    eyeFundusImage.eye_fundus_image_id = newObs.observationId;
        //                    NewIVLDataMethods.AddEyeFundusImage(eyeFundusImage);//will save the additional details related to the image into the DB.
        //                    NewDataVariables.Active_Obs = newObs;
        //                    NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].patientLastModifiedDate = DateTime.Now;
        //                    NewDataVariables.Active_Visit.lastModifiedDate = DateTime.Now;
        //                    NewIVLDataMethods.UpdatePatient();
        //                    NewIVLDataMethods.UpdateVisit();
        //                    NewDataVariables.Active_PatientIdentifier.lastModifiedDate = DateTime.Now;
        //                    NewIVLDataMethods.UpdatePatientIdentifier();
        //                    tdata.id = newObs.observationId;
        //                   // arg["id"] = newObs.observationId;
        //                    //foreach (var item in NewDataVariables.Visits)
        //                    //{
        //                    //    visitDate.Add(item.createdDate);
        //                    //}
        //                    //DataTable d = NewDataVariables.Visits.ToDataTable();
        //                    //d.Columns.Add(IVLVariables.LangResourceManager.GetString("VisitsAddImages_ColName", IVLVariables.LangResourceCultureInfo), typeof(Button));
        //                    //SetVisitData(d);
        //                    //clearVisitGrid();

        //                    //if (eyeFundusImage.eyeSide == 'L')
        //                    //{
        //                    //    tdata.side = 1;
        //                    //    arg["side"] = 1;
        //                    //}
        //                    //else
        //                    //{
        //                    //    tdata.side = 0;
        //                    //    arg["side"] = 0;
        //                    //} 
        //                    //if (!arg.ContainsKey("isModifiedimage"))
        //                    //    arg["isModifiedimage"] = false;
        //                }
        //                else
        //                {
        //                    //if (eyeSide == 'L')
        //                    //    arg["side"] = 1;
        //                    //else
        //                    //    arg["side"] = 0;
        //                    tdata.id = IVLVariables.CmdObsID;
        //                    //arg["id"] = IVLVariables.CmdObsID;
        //                    tdata.isModified = false;
        //                    arg["isModifiedimage"] = false;
        //                    IVLVariables.CmdObsID++;

        //                }
        //                arg["ThumbnailData"] = tdata;
        //                eventHandler.Notify(eventHandler.ThumbnailAdd, arg);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
        //        //                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
        //    }
        //}



        private void imageAdded(string s, Args arg)
        {

        }

        public void SetCurrentMode(ImagingMode mode)
        {
          
            if ( !IVLVariables.ivl_Camera.IsCapturing && !IVLVariables.ivl_Camera.IsResuming && !IVLVariables.ivl_Camera.isResetMode && IVLVariables.ivl_Camera.IsMotorMovementDone)
            {
                if(IVLVariables.ivl_Camera.isCameraOpen)
                    IVLVariables.ivl_Camera.StopLive();
                //IVLVariables.ivl_Camera.TriggerOff();// turn off trigger when mode is changed                
                arg["isDefault"] = false;
                arg["EnableEmrButton"] = false;
                eventHandler.Notify(eventHandler.UpdateMainWindowCursor, arg);
                //eventHandler.Notify(eventHandler.EnableDisableEmrButton, arg);
                blueLight_rb.Enabled = false;
                
            switch (mode)
            {
                case ImagingMode.Posterior_Prime:
                    {
                        IVLVariables._ivlConfig.Mode = Configuration.ImagingMode.Posterior_Prime;
                        Configuration.ConfigVariables.GetCurrentSettings();
                        Control_tbLt.RowStyles[2] = new RowStyle(SizeType.Percent, 0f);
                        Control_tbLt.RowStyles[1] = new RowStyle(SizeType.Percent, 0f);// IVLVariables.ivl_Camera.camPropsHelper.ImagingMode = Imaging.ImagingMode.Posterior_Prime;
                       // if (!IVLVariables.ivl_Camera.CameraName.Contains("E3CMOS06300KPA(USB2.0)") )
                            if (IVLVariables.isDefaultPrimeGain)
                            {
                                IVLVariables.CurrentSettings.CameraSettings.CaptureCurrentGainLevel.val = IVLVariables.CurrentSettings.CameraSettings._DigitalGainDefault.val;
                                IVLVariables.CurrentSettings.CameraSettings.LiveCurrentGainLevel.val = IVLVariables.CurrentSettings.CameraSettings._LiveGainDefault.val;
                                IVLVariables.isDefaultPrimeGain = false;
                            }
                            IVLVariables.CurrentLiveGain = (GainLevels)Enum.Parse(typeof(GainLevels), IVLVariables.CurrentSettings.CameraSettings.LiveCurrentGainLevel.val);
                            IVLVariables.CurrentCaptureGain = (GainLevels)Enum.Parse(typeof(GainLevels), IVLVariables.CurrentSettings.CameraSettings.CaptureCurrentGainLevel.val);
                            if (IVLVariables.isDefaultPrimeLedSource)
                            {
                                IVLVariables.CurrentSettings.CameraSettings.LiveLedSource.val = IVLVariables.CurrentSettings.CameraSettings.DefaultLiveLedSource.val;
                                IVLVariables.isDefaultPrimeLedSource = false;
                            }
                        if (IVLVariables.ivl_Camera.camPropsHelper.ImagingMode != mode)
                        {
                            IVLVariables.ivl_Camera.camPropsHelper.ImagingMode = Imaging.ImagingMode.Posterior_Prime;
                            IVLVariables.ivl_Camera.ChangeMode();
                            arg["isDefault"] = true;

                            eventHandler.Notify(eventHandler.UpdateMainWindowCursor, arg);
                        }
                        else
                        {
                                IVLVariables.ivl_Camera.StartLive();
                                arg["isDefault"] = true;
                                eventHandler.Notify(eventHandler.UpdateMainWindowCursor, arg);
                        }
                        //else
                        //    Common.CustomMessageBox.Show("Camera is connected to USB 2.0 please connect it to USB 3.0 port");
                        break;
                    }
                case ImagingMode.Posterior_45:
                    {
                        IVLVariables._ivlConfig.Mode = Configuration.ImagingMode.Posterior_45;
                        Configuration.ConfigVariables.GetCurrentSettings();
                        Control_tbLt.RowStyles[2] = new RowStyle(SizeType.Percent, 0f);
                        Control_tbLt.RowStyles[1] = new RowStyle(SizeType.Percent, 0f);
                        IVLVariables.ivl_Camera.camPropsHelper.ImagingMode = Imaging.ImagingMode.Posterior_45;
                        IVLVariables._ivlConfig.FortyFiveSettings.Settings.UISettings.LiveImaging._IRBtnVisible.val = string.Format("true");
                        IVLVariables._ivlConfig.FortyFiveSettings.Settings.UISettings.LiveImaging._FlashBtnVisible.val = string.Format("true");
                        if (IVLVariables.isDefault45LedSource)
                        {
                            IVLVariables.CurrentSettings.CameraSettings.LiveLedSource.val = IVLVariables.CurrentSettings.CameraSettings.DefaultLiveLedSource.val;
                            IVLVariables.isDefault45LedSource = false;
                        }
{
                            IVLVariables.ivl_Camera.StartLive();
                            arg["isDefault"] = true;
                            eventHandler.Notify(eventHandler.UpdateMainWindowCursor, arg);
                        }
                        break;
                    }
                case ImagingMode.Anterior_Prime:
                    {
                        blueLight_rb.Enabled = true;
                        IVLVariables._ivlConfig.Mode = Configuration.ImagingMode.Anterior_Prime;
                        Configuration.ConfigVariables.GetCurrentSettings();
                        Control_tbLt.RowStyles[2] = new RowStyle(SizeType.Percent, 0f);
                        Control_tbLt.RowStyles[1] = new RowStyle(SizeType.Percent, 0f);
                        //IVLVariables.ivl_Camera.camPropsHelper.ImagingMode = Imaging.ImagingMode.Anterior_Prime;
                       // if (!IVLVariables.ivl_Camera.CameraName.Contains("E3CMOS06300KPA(USB2.0)"))
                        {
                            if (IVLVariables.isDefaultAnteriorGain)
                            {
                                IVLVariables.CurrentSettings.CameraSettings.CaptureCurrentGainLevel.val = IVLVariables.CurrentSettings.CameraSettings._DigitalGainDefault.val;
                                IVLVariables.CurrentSettings.CameraSettings.LiveCurrentGainLevel.val = IVLVariables.CurrentSettings.CameraSettings._LiveGainDefault.val;
                                IVLVariables.isDefaultAnteriorGain = false;
                            }
                            IVLVariables.CurrentLiveGain = (GainLevels)Enum.Parse(typeof(GainLevels), IVLVariables.CurrentSettings.CameraSettings.LiveCurrentGainLevel.val);
                            IVLVariables.CurrentCaptureGain = (GainLevels)Enum.Parse(typeof(GainLevels), IVLVariables.CurrentSettings.CameraSettings.CaptureCurrentGainLevel.val);
                            if (IVLVariables.isDefaultAnteriorLedSource)
                            {
                                IVLVariables.CurrentSettings.CameraSettings.LiveLedSource.val = IVLVariables.CurrentSettings.CameraSettings.DefaultLiveLedSource.val;
                                IVLVariables.isDefaultAnteriorLedSource = false;
                            }
                        }
                        if (IVLVariables.ivl_Camera.camPropsHelper.ImagingMode != mode)
                        {
                            IVLVariables.ivl_Camera.camPropsHelper.ImagingMode = Imaging.ImagingMode.Anterior_Prime;
                            IVLVariables.ivl_Camera.ChangeMode();
                          
                            arg["isDefault"] = true;
                            eventHandler.Notify(eventHandler.UpdateMainWindowCursor, arg);
                        }
                        else
                        {
                                IVLVariables.ivl_Camera.StartLive();
                                arg["isDefault"] = true;
                                eventHandler.Notify(eventHandler.UpdateMainWindowCursor, arg);
                        }
                        liveSource_Gbx.Visible = false;


                        //else
                        //    Common.CustomMessageBox.Show("Camera is connected to USB 2.0 please connect it to USB 3.0 port");
                        break;
                    }
                case ImagingMode.FFAColor:
                    {

                        IVLVariables._ivlConfig.Mode = Configuration.ImagingMode.FFAColor;
                        Configuration.ConfigVariables.GetCurrentSettings();

                        IVLVariables.ivl_Camera.camPropsHelper.ImagingMode = Imaging.ImagingMode.FFAColor;
                        IVLVariables.ivl_Camera.camPropsHelper.Read_LED_SupplyValues();
                        Control_tbLt.RowStyles[2] = new RowStyle(SizeType.Percent, 10f);
                        Control_tbLt.RowStyles[1] = new RowStyle(SizeType.Percent, 10f);
                        startFFATimer_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.LiveImaging.StartFFATimerButtonVisible.val);//added to make the visibility of the timer button
                        ffaTimerStatus_lbl.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.LiveImaging.StartFFATimerButtonVisible.val);//added to make the visibility of the timer label
                        startFFATimer_btn.Enabled = false;// Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.LiveImaging.StartFFATimerButtonVisible.val);//added to make the visibility of the timer button
                        ffaTimerStatus_lbl.Enabled = false;// Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.LiveImaging.StartFFATimerButtonVisible.val);//added to make the visibility of the timer label
                        
                        // IVLVariables.ivl_Camera.ServoMove();
                        IVLVariables.ivl_Camera.camPropsHelper.SetMonoChromeMode(false);
                        if (IVLVariables.isDefault45LedSource)
                        {
                            IVLVariables.CurrentSettings.CameraSettings.LiveLedSource.val = IVLVariables.CurrentSettings.CameraSettings.DefaultLiveLedSource.val;
                            IVLVariables.isDefault45LedSource = false;
                        }
                            IVLVariables.ivl_Camera.StartLive();
                            arg["isDefault"] = true;
                            eventHandler.Notify(eventHandler.UpdateMainWindowCursor, arg);

                        //tableLayoutPanel2.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 50f); ;
                        //tableLayoutPanel2.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 50f); ;
                        //tableLayoutPanel2.ColumnStyles[2] = new ColumnStyle(SizeType.Percent, 0f); ;
                        break;
                    }
                case ImagingMode.FFA_Plus:
                    {
                        blueLight_rb.Enabled = true;

                        IVLVariables._ivlConfig.Mode = Configuration.ImagingMode.FFA_Plus;
                        Configuration.ConfigVariables.GetCurrentSettings();

                        IVLVariables.ivl_Camera.camPropsHelper.ImagingMode = Imaging.ImagingMode.FFA_Plus;
                        IVLVariables.ivl_Camera.camPropsHelper.Read_LED_SupplyValues();
                        Control_tbLt.RowStyles[2] = new RowStyle(SizeType.Percent,10f);
                        Control_tbLt.RowStyles[1] = new RowStyle(SizeType.Percent,10f);
                        startFFATimer_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.LiveImaging.StartFFATimerButtonVisible.val);//added to make the visibility of the timer button
                        ffaTimerStatus_lbl.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.LiveImaging.StartFFATimerButtonVisible.val);//added to make the visibility of the timer label
                        startFFATimer_btn.Enabled = true;// 
                        ffaTimerStatus_lbl.Enabled = true;//

                        //tableLayoutPanel2.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 0f); ;
                        //tableLayoutPanel2.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 50f); ;
                        //tableLayoutPanel2.ColumnStyles[2] = new ColumnStyle(SizeType.Percent, 50f); ;
                        //IVLVariables.ivl_Camera.ServoMove();
                        IVLVariables.ivl_Camera.camPropsHelper.SetMonoChromeMode(true);
                        if (IVLVariables.isDefaultFFAPlusLedSource)
                        {
                            IVLVariables.CurrentSettings.CameraSettings.LiveLedSource.val = IVLVariables.CurrentSettings.CameraSettings.DefaultLiveLedSource.val;
                            IVLVariables.isDefaultFFAPlusLedSource = false;
                        }
                            IVLVariables.ivl_Camera.StartLive();
                            arg["isDefault"] = true;
                            eventHandler.Notify(eventHandler.UpdateMainWindowCursor, arg);
                        break;
                    }
                case ImagingMode.Anterior_45:
                    {
                        IVLVariables._ivlConfig.Mode = Configuration.ImagingMode.Anterior_45;
                        Configuration.ConfigVariables.GetCurrentSettings();
                        Control_tbLt.RowStyles[2] = new RowStyle(SizeType.Percent, 0f);
                        Control_tbLt.RowStyles[1] = new RowStyle(SizeType.Percent, 0f);
                        IVLVariables.ivl_Camera.camPropsHelper.ImagingMode = Imaging.ImagingMode.Anterior_45;
                        if (IVLVariables.isDefaultAnteriorLedSource)
                        {
                            IVLVariables.CurrentSettings.CameraSettings.LiveLedSource.val = IVLVariables.CurrentSettings.CameraSettings.DefaultLiveLedSource.val;
                            IVLVariables.isDefaultAnteriorLedSource = false;
                        }
                            IVLVariables.ivl_Camera.StartLive();
                            arg["isDefault"] = true;
                            eventHandler.Notify(eventHandler.UpdateMainWindowCursor, arg);
                        break;
                    }
                case ImagingMode.Anterior_FFA:
                    {
                        blueLight_rb.Enabled = true;
                        Control_tbLt.RowStyles[2] = new RowStyle(SizeType.Percent, 0f);
                        Control_tbLt.RowStyles[1] = new RowStyle(SizeType.Percent, 0f);
                        IVLVariables._ivlConfig.Mode = Configuration.ImagingMode.Anterior_FFA;
                        Configuration.ConfigVariables.GetCurrentSettings();

                        IVLVariables.ivl_Camera.camPropsHelper.ImagingMode = Imaging.ImagingMode.Anterior_FFA;
                        if (IVLVariables.isDefaultAnteriorLedSource)
                        {
                            IVLVariables.CurrentSettings.CameraSettings.LiveLedSource.val = IVLVariables.CurrentSettings.CameraSettings.DefaultLiveLedSource.val;
                            IVLVariables.isDefaultAnteriorLedSource = false;
                        }
                         
                            IVLVariables.ivl_Camera.StartLive();
                            arg["isDefault"] = true;
                            eventHandler.Notify(eventHandler.UpdateMainWindowCursor, arg);
                       } 
                       break;

                    
            }

            UpdateLiveSource();
            Configuration.ConfigVariables.GetCurrentSettings();
            
                SetLiveModeUIControls();
           /* if (IVLVariables.ivl_Camera.camPropsHelper.ImagingMode == Imaging.ImagingMode.Anterior_Prime || IVLVariables.ivl_Camera.camPropsHelper.ImagingMode == Imaging.ImagingMode.Posterior_Prime)//TO change mode only when it is in posterior and anterior prime.
                IVLVariables.ivl_Camera.ChangeMode();
            else
            {
                IVLVariables.ivl_Camera.StartLive();
                arg["isDefault"] = true;
                arg["EnableEmrButton"] = true;
                eventHandler.Notify(eventHandler.UpdateMainWindowCursor, arg);
                eventHandler.Notify(eventHandler.EnableDisableEmrButton, arg);
            }*/

            eventHandler.Notify(eventHandler.UpdateOverlay, new Args());
            posteriorAnteriorButtonRefresh();

         }
        }
        public void SetLiveModeUIControls()
        {

            SetUIFromConfigSettings();
            //if(!IVLVariables.ivl_Camera.CameraIsLive)
            //isLive = IVLVariables.ivl_Camera.StartLive();
            liveSource_Gbx.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.LiveImaging.ShowLiveSource.val);

            capture_btn.Text = IVLVariables.LangResourceManager.GetString("Capture_Button_Text", IVLVariables.LangResourceCultureInfo);

            if (IVLVariables.ivl_Camera.camPropsHelper.ImagingMode == ImagingMode.Posterior_Prime || IVLVariables.ivl_Camera.camPropsHelper.ImagingMode == ImagingMode.Anterior_Prime)
            {
                if (!panel3.Contains(_livePrimeControls))
                    panel3.Controls.Add(_livePrimeControls);
                _livePrimeControls.Dock = DockStyle.Fill;
                IVLVariables.CurrentLiveGain = (GainLevels)Enum.Parse(typeof(GainLevels), IVLVariables.CurrentSettings.CameraSettings.LiveCurrentGainLevel.val);
                IVLVariables.CurrentCaptureGain = (GainLevels)Enum.Parse(typeof(GainLevels), IVLVariables.CurrentSettings.CameraSettings.CaptureCurrentGainLevel.val);
                #region to set capture and live gain level from config settings to camera properties helper when mode is changed by sriram on 16 august 2017
                IVLVariables.ivl_Camera.camPropsHelper.CaptureGainLevel = (Imaging.GainLevels)Enum.Parse(typeof(Imaging.GainLevels), IVLVariables.CurrentSettings.CameraSettings.CaptureCurrentGainLevel.val);
                IVLVariables.ivl_Camera.camPropsHelper.LiveGainLevel = (Imaging.GainLevels)Enum.Parse(typeof(Imaging.GainLevels), IVLVariables.CurrentSettings.CameraSettings.LiveCurrentGainLevel.val);
                #endregion

                IVLVariables.ivl_Camera.camPropsHelper.CaptureGainLevel = (Imaging.GainLevels)Enum.Parse(typeof(Imaging.GainLevels), IVLVariables.CurrentSettings.CameraSettings.CaptureCurrentGainLevel.val);
                IVLVariables.ivl_Camera.camPropsHelper.LiveGainLevel = (Imaging.GainLevels)Enum.Parse(typeof(Imaging.GainLevels), IVLVariables.CurrentSettings.CameraSettings.LiveCurrentGainLevel.val);
                _livePrimeControls.RefreshLiveGainButtons(IVLVariables.CurrentLiveGain);
                _livePrimeControls.RefreshFlashGainButtons(IVLVariables.CurrentCaptureGain);
                IVLVariables.ivl_Camera.camPropsHelper._Settings.CameraSettings.LiveExposure = Convert.ToUInt32(IVLVariables.CurrentSettings.CameraSettings._Exposure.val);
                    
            }

            else if (IVLVariables.ivl_Camera.camPropsHelper.ImagingMode == ImagingMode.Posterior_45 || IVLVariables.ivl_Camera.camPropsHelper.ImagingMode == ImagingMode.FFA_Plus || IVLVariables.ivl_Camera.camPropsHelper.ImagingMode == ImagingMode.FFAColor)
            {
                if (!panel3.Controls.Contains(_liveFortyFiveControls))
                {

                    panel3.Controls.Add(_liveFortyFiveControls);
                }
                //   _liveFortyFiveControls.Dock = DockStyle.None;
                _liveFortyFiveControls.Dock = DockStyle.Fill;

                   

                //Disable gain and exposure controls for FFA device   
                    if (IVLVariables.ivl_Camera.camPropsHelper.ImagingMode == ImagingMode.FFA_Plus || IVLVariables.ivl_Camera.camPropsHelper.ImagingMode == ImagingMode.FFAColor)
                    {
                        //IVLVariables.CurrentSettings.CameraSettings._LiveDigitalGain.val = "63";
                        _liveFortyFiveControls.DisableLiveorCaptureControls(true,false);//.Enabled = false;
                    }
                    else
                        _liveFortyFiveControls.DisableLiveorCaptureControls(true, true);//.Enabled = false;


                    int liveGain = Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._LiveDigitalGain.val);
                    GainExposureHelper.SetLiveGain(liveGain);

                    int CaptureGain = Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._DigitalGain.val);
                    int knobDiff = Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._KnobIndexDifferenceValue.val);
                    _liveFortyFiveControls.liveEG_lbl.Text = (liveGain - knobDiff).ToString();

                    GainExposureHelper.SetFlashGain(CaptureGain);
                    _liveFortyFiveControls.captureEG_lbl.Text = (CaptureGain - knobDiff).ToString();
            }
        }

        public void posteriorAnteriorButtonRefresh()
        {
            PosteriorAnteriorBtnSelection();
        }

        private void browse_btn_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (IVLVariables.ivl_Camera.camPropsHelper.ImageSavedPath != null)
                System.Diagnostics.Process.Start(IVLVariables.ivl_Camera.camPropsHelper.ImageSavedPath);
            else
                System.Diagnostics.Process.Start(IVLVariables.ivl_Camera.camPropsHelper._Settings.ImageSaveSettings.RawImageDirPath);
        }


        int PlySteps = 0;
        public void saveFrames()
        {
           
            //if (!IVLVariables.ivl_Camera.isResetMode && IVLVariables.ivl_Camera.IsMotorMovementDone)//checks for the camera connected and is not reset mode and the motor movement done is true and the board is openned.
            {
                if (!IVLVariables.ivl_Camera.IsCapturing)
                {
                    //if (left_btn.BackColor == Color.Yellow)
                    //    IVLVariables.ivl_Camera.camPropsHelper.LeftRightPos = LeftRightPosition.Left;
                    //else
                    //    IVLVariables.ivl_Camera.camPropsHelper.LeftRightPos = LeftRightPosition.Right;
                    //arg = new Args();
                    //if (capture_btn.Text == captureBtnText)
                    //{
                    //    arg["BtnEnable"] = true;
                    //}
                    //else
                    //{
                    //    arg["BtnEnable"] = false;
                    //}
                    if (!IVLVariables.isCommandLineAppLaunch)
                    {
                        arg["StartTimer"] = false;
                        eventHandler.Notify(eventHandler.StartStopServerDatabaseTimer, arg);
                    } 
                    eventHandler.Notify(eventHandler.CaptureScreenUpdate, arg);
                   // arg["StatusEnable"] = false;
                    //eventHandler.Notify(eventHandler.EnableCapturePowerStatusTimer, arg);
                    capture_btn.Text = resumeBtnText;
                    //if (Convert.ToBoolean(IVLVariables.CurrentSettings.ImageNameSettings.ChangeImageName.val))
                    //{
                    //    List<string> ImageNameList = new List<string>();
                    //    if (Convert.ToBoolean(IVLVariables.CurrentSettings.ImageNameSettings.IsMRNPresent.val))
                    //        ImageNameList.Add(NewDataVariables.Active_PatientIdentifier.value);
                    //    if (Convert.ToBoolean(IVLVariables.CurrentSettings.ImageNameSettings.IsFirstNamePresent.val))
                    //        ImageNameList.Add(NewDataVariables.Active_Patient.firstName);
                    //    if (Convert.ToBoolean(IVLVariables.CurrentSettings.ImageNameSettings.IsLastNamePresent.val))
                    //        ImageNameList.Add(NewDataVariables.Active_Patient.lastName);
                    //    if (Convert.ToBoolean(IVLVariables.CurrentSettings.ImageNameSettings.IsEyeSidePresent.val))
                    //    {
                    //        if (LeftRightPosition.Left == IVLVariables.ivl_Camera.camPropsHelper.LeftRightPos)
                    //            ImageNameList.Add("L");
                    //        else
                    //            ImageNameList.Add("R");
                    //    }
                    //    IVLVariables.ivl_Camera.camPropsHelper.ImageName = string.Join("_", ImageNameList.ToArray(), 0, ImageNameList.Count);
                    //}
                    //IVLVariables.ivl_Camera.StartCapture(IVLVariables.istrigger);
                }
               
           
        }
        }
        private void ResumeLive()
        {
            if (!IVLVariables.ivl_Camera.isResetMode && IVLVariables.ivl_Camera.IsMotorMovementDone && IVLVariables.ivl_Camera.camPropsHelper.IsCameraConnected==Devices.CameraConnected  && IVLVariables.ivl_Camera.camPropsHelper.IsBoardOpen)//checks for the camera connected and is not reset mode and the motor movement done is true and the board is openned.

             {
                   
                    IVLVariables.istrigger = false;
                    if (IVLVariables.ivl_Camera.camPropsHelper.ImagingMode == ImagingMode.Posterior_45 || IVLVariables.ivl_Camera.camPropsHelper.ImagingMode == ImagingMode.FFA_Plus || IVLVariables.ivl_Camera.camPropsHelper.ImagingMode == ImagingMode.FFAColor)
                    {
                        if (IVLVariables.ivl_Camera.camPropsHelper.ImagingMode == ImagingMode.FFA_Plus || IVLVariables.ivl_Camera.camPropsHelper.ImagingMode == ImagingMode.FFAColor || !Convert.ToBoolean(IVLVariables.CurrentSettings.FirmwareSettings._EnablePCUKnob.val))// Added check for PCU knob for both FFA and 45 by sriram
                        {
                            GainExposureHelper.SetLiveGain(Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._LiveDigitalGain.val));
                            GainExposureHelper.SetFlashGain(Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._DigitalGain.val));

                        }
                    }
                    else
                    {
                        GainLevels ch = GainLevels.Low;
                        if (IVLVariables.ivl_Camera.camPropsHelper.ImagingMode == Imaging.ImagingMode.Anterior_Prime || IVLVariables.ivl_Camera.camPropsHelper.ImagingMode == Imaging.ImagingMode.Anterior_45
                  || IVLVariables.ivl_Camera.camPropsHelper.ImagingMode == Imaging.ImagingMode.Anterior_FFA)
                            ch = IVLVariables.CurrentLiveGain;
                        else
                            ch = IVLVariables.CurrentLiveGain;
                        _livePrimeControls.RefreshLiveGainButtons(ch);
                    }
                    IVLVariables.ivl_Camera.ResumeLive();
                    #region   //Added to fix the defect number 0001422

                    //StartLiveMode();
                    #endregion
                    capture_btn.Text = IVLVariables.LangResourceManager.GetString("Capture_Button_Text", IVLVariables.LangResourceCultureInfo);
                    arg["StatusEnable"] = true;
                    arg["isDefault"] = true;
                    arg["isImaging"] = true;
                    arg["StartTimer"] = true;
                    if(!IVLVariables.isCommandLineAppLaunch)
                    eventHandler.Notify(eventHandler.StartStopServerDatabaseTimer, arg);
                    eventHandler.Notify(eventHandler.UpdateMainWindowCursor, arg);
                    return;
                }
            }
        private void capture_btn_Click(object sender, EventArgs e)
        {
            IVLVariables.ivl_Camera.CaptureStartTime = DateTime.Now;
            IVLVariables.ivl_Camera.HowImageWasCaptured = IntucamHelper.CapturedUIMode.CaptureButton; 
            //if (!IVLVariables.ivl_Camera.IsCapturing && !IVLVariables.ivl_Camera.IsResuming && !IVLVariables.ivl_Camera.isResetMode && IVLVariables.ivl_Camera.IsMotorMovementDone)// this condition has been added to fix the defect 0001258 by sriram on 20 oct 2016
            IVLVariables.ivl_Camera.Trigger_Or_SpacebarPressed(IVLVariables.istrigger);
//            saveFrames();
        }


        private void left_btn_Click(object sender, EventArgs e)
        {
            if (!IVLVariables.ivl_Camera.IsCapturing && !IVLVariables.ivl_Camera.IsResuming && !IVLVariables.ivl_Camera.isResetMode && IVLVariables.ivl_Camera.        IsMotorMovementDone)

            if (!Convert.ToBoolean(IVLVariables.CurrentSettings.FirmwareSettings._EnableLeftRightSensor.val))
            {
                arg["LeftRightPos"] = LeftRightPosition.Left;
                eventHandler.Notify(eventHandler.ChangeLeftRightPos_Live, arg);
            }
        }


        private void right_btn_Click(object sender, EventArgs e)
        {
            if (!IVLVariables.ivl_Camera.IsCapturing && !IVLVariables.ivl_Camera.IsResuming && !IVLVariables.ivl_Camera.isResetMode && IVLVariables.ivl_Camera.        IsMotorMovementDone)

            if (!Convert.ToBoolean(IVLVariables.CurrentSettings.FirmwareSettings._EnableLeftRightSensor.val))
            {
                arg["LeftRightPos"] = LeftRightPosition.Right;
                eventHandler.Notify(eventHandler.ChangeLeftRightPos_Live, arg);
            }
        }

        private void ChangeLeftRightPosition(string s, Args arg)
        {
            {
                if ((LeftRightPosition)Enum.Parse(typeof(LeftRightPosition), arg["LeftRightPos"].ToString()) == LeftRightPosition.Left)
                {
                    left_btn.BackColor = Color.Yellow;
                    right_btn.BackColor = Color.Khaki;
                    IVLVariables.ivl_Camera.camPropsHelper.LeftRightPos = LeftRightPosition.Left;
                }
                else
                {
                    left_btn.BackColor = Color.Khaki;
                    right_btn.BackColor = Color.Yellow;
                    IVLVariables.ivl_Camera.camPropsHelper.LeftRightPos = LeftRightPosition.Right;
                }
            }
        }
        private void resetMotorSensor_btn_Click(object sender, EventArgs e)
        {
            resetMotorEvent();
        }

        private void posterior_btn_Click(object sender, EventArgs e)
        {
            if (ImagingMode.Posterior_Prime != IVLVariables.ivl_Camera.camPropsHelper.ImagingMode)//if the existing mode is not new mode then change mode.
            SetCurrentMode(ImagingMode.Posterior_Prime);

        }

        private void antirior_btn_Click(object sender, EventArgs e)
        {
            if (ImagingMode.Anterior_Prime != IVLVariables.ivl_Camera.camPropsHelper.ImagingMode)//if the existing mode is not new mode then change mode.
            SetCurrentMode(ImagingMode.Anterior_Prime);
        }

        private void FlashLight_rbx_CheckedChanged(object sender, EventArgs e)
        {

            ChangeLiveSource();
        }

        private void IRlight_rbx_CheckedChanged(object sender, EventArgs e)
        {
            ChangeLiveSource();
        }

        private void ffaColor_btn_Click(object sender, EventArgs e)
        {
            if (ImagingMode.FFAColor != IVLVariables.ivl_Camera.camPropsHelper.ImagingMode)//if the existing mode is not new mode then change mode.
            SetCurrentMode(ImagingMode.FFAColor);
        }
        private void ffa_btn_Click(object sender, EventArgs e)
        {
            if (ImagingMode.FFA_Plus != IVLVariables.ivl_Camera.camPropsHelper.ImagingMode)//if the existing mode is not new mode then change mode.
            SetCurrentMode(ImagingMode.FFA_Plus);
        }
        private void fortyFiveMode_btn_Click(object sender, EventArgs e)
        {
            if (ImagingMode.Posterior_45 != IVLVariables.ivl_Camera.camPropsHelper.ImagingMode)//if the existing mode is not new mode then change mode.
            SetCurrentMode(ImagingMode.Posterior_45);
        }

        private void UpdateLiveSource()
        {
            if (IVLVariables.CurrentSettings.CameraSettings.LiveLedSource.val == "IR")
                IRlight_rbx.Checked = true;
            else if (IVLVariables.CurrentSettings.CameraSettings.LiveLedSource.val == "Flash")
                FlashLight_rbx.Checked = true;
            else
                blueLight_rb.Checked = true;

        }
        private bool isEnableFFATimer = false;

        public bool IsEnableFFATimer
        {
            get { return isEnableFFATimer; }
            set {
                isEnableFFATimer = value;
                EnableDisableFFATimer();   
            }
        }
       
        private void startFFA_btn_Click(object sender, EventArgs e)
        {
            IsEnableFFATimer = !IsEnableFFATimer;
        }

        private void EnableDisableFFATimer()
        {
            if (IsEnableFFATimer)
            {
                startFFATimer_btn.Text = IVLVariables.LangResourceManager.GetString("stopFFA_Text", IVLVariables.LangResourceCultureInfo); ;
                IVLVariables.ivl_Camera.camPropsHelper.EnableFFATimer(IsEnableFFATimer);
            }
            else
            {
                startFFATimer_btn.Text = IVLVariables.LangResourceManager.GetString("startFFA_Text", IVLVariables.LangResourceCultureInfo); ;
                ffaTimerStatus_lbl.Text = "FFA Time : " + "";
                IVLVariables.ivl_Camera.camPropsHelper.EnableFFATimer(IsEnableFFATimer);
            }
        }

        private void blueLight_rb_CheckedChanged(object sender, EventArgs e)
        {
            ChangeLiveSource();
        }

        private void ChangeLiveSource()
        {
            if (IRlight_rbx.Checked)
            {
                if (IVLVariables.ivl_Camera.LEDSource != Led.IR)
                {
                    IVLVariables.CurrentSettings.CameraSettings.LiveLedSource.val = "IR";
                    IVLVariables.ivl_Camera.LEDSource = Led.IR;
                }
            }
            else
                if (FlashLight_rbx.Checked)
                {
                    if (IVLVariables.ivl_Camera.LEDSource != Led.Flash)
                    {
                        IVLVariables.CurrentSettings.CameraSettings.LiveLedSource.val = "Flash";
                        IVLVariables.ivl_Camera.LEDSource = Led.Flash;
                    }
                }
                else
                    if (blueLight_rb.Checked)
                    {
                        if (IVLVariables.ivl_Camera.LEDSource != Led.Blue)
                        {
                            IVLVariables.ivl_Camera.LEDSource = Led.Blue;
                            IVLVariables.CurrentSettings.CameraSettings.LiveLedSource.val = "Blue";
                        }
                    }
        }
    }
}
