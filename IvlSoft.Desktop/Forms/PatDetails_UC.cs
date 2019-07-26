using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using INTUSOFT.EventHandler;
using INTUSOFT.Desktop.Forms;
using INTUSOFT.Desktop.Properties;
namespace INTUSOFT.Desktop
{
    public partial class PatDetails_UC : UserControl
    {
        string diagnosisLogoPath = @"ImageResources\Edit_ImageResources\diagnosis.png";
        string viewFullInfoLogoPath = @"ImageResources\PatientDetailsImageResources\i-info.png";
        public PatDetails_UC()
        {
            InitializeComponent();
            initializeLanguageFromResource();
            List<Control> controls = GetControls(this).ToList();
            foreach (Control c in controls)
            {
                if (c is Label)
                {
                    c.ForeColor = IVLVariables.GradientColorValues.FontForeColor;
                    {
                        if (Screen.PrimaryScreen.Bounds.Width == 1920)
                        {
                            c.Font = new Font(c.Font.FontFamily.Name, 13f);
                            viewFullInfo_btn.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
                            viewFullInfo_btn.Size = new Size(100, 100);
                            addDiagnosis_btn.Size = new Size(100, 100);
                        }
                        else if (Screen.PrimaryScreen.Bounds.Width == 1366)
                            c.Font = new Font(c.Font.FontFamily.Name, 10f);
                        else
                            c.Font = new Font(c.Font.FontFamily.Name, 9f);
                    }
                }
            }
            
        }

        /// <summary>
        /// This method will return all controls present in the form
        /// </summary>
        /// <param name="form">Control of type Form to be send</param>
        /// <returns>List of controls to be returned</returns>
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

        private void initializeLanguageFromResource()
        {
            ActivePat_lbl.Text = IVLVariables.LangResourceManager.GetString( "ActivePat_Label_Text",IVLVariables.LangResourceCultureInfo);
            Mrn_lbl.Text = IVLVariables.LangResourceManager.GetString( "Mrn_Label_Text",IVLVariables.LangResourceCultureInfo);
            Name_lbl.Text = IVLVariables.LangResourceManager.GetString( "Name_Label_Text",IVLVariables.LangResourceCultureInfo);
            age_lbl.Text = IVLVariables.LangResourceManager.GetString( "Age_radio_button_Text",IVLVariables.LangResourceCultureInfo);
            gender_lbl.Text = IVLVariables.LangResourceManager.GetString( "Gender_Label_Text",IVLVariables.LangResourceCultureInfo);
            viewFullInfo_btn.Text = IVLVariables.LangResourceManager.GetString("ViewPatFullInfo_Button_Text", IVLVariables.LangResourceCultureInfo);
            addDiagnosis_btn.Text = IVLVariables.LangResourceManager.GetString("AddDiagnosisButtonText", IVLVariables.LangResourceCultureInfo);
            ToolTip fullInfobtn = new ToolTip();  //code to add toot tip text
            //fullInfobtn.SetToolTip(viewFullInfo_btn, IVLVariables.LangResourceManager.GetString("ViewPatFullInfoButton_ToolTipText", IVLVariables.LangResourceCultureInfo));
            PatientInfo_Ts.Renderer = new INTUSOFT.Custom.Controls.FormToolStripRenderer();
            viewFullInfo_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("ViewPatFullInfoButton_ToolTipText", IVLVariables.LangResourceCultureInfo);
            addDiagnosis_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("AddDiagnosisButtonToolTipText", IVLVariables.LangResourceCultureInfo);
            if (File.Exists(diagnosisLogoPath))
                addDiagnosis_btn.Image = Image.FromFile(diagnosisLogoPath);
            if (File.Exists(viewFullInfoLogoPath))
                viewFullInfo_btn.Image = Image.FromFile(viewFullInfoLogoPath);
            if (IVLVariables.isCommandLineAppLaunch)
            {
                viewFullInfo_btn.Enabled = false;
                addDiagnosis_btn.Enabled = false;
            }
        }

        public void setPatValue(Args arg)
        {
            mrnVal_lbl.Text = arg["MRN"] as string;
            firstName_lbl.Text = arg["FirstName"] as string +"  "+ arg["LastName"] as string;
            //lastName_lbl.Text = arg["LastName"] as string;
            ageVal_lbl.Text = arg["Age"] as string;
            genderVal_lbl.Text = arg["Gender"] as string;
        }

        private void viewFullInfo_btn_Click_1(object sender, EventArgs e)
        {
            if (!IVLVariables.ivl_Camera.IsCapturing)
            {
                #region To fix defect 0001777 This part is added  in order to go to live screen when patient info view button is clicked the camera will be taken to view screen
                if (IVLVariables.ivl_Camera.CameraIsLive)
                {
                    IVLEventHandler eventHandler = IVLEventHandler.getInstance();
                    eventHandler.Notify(eventHandler.GoToViewScreen, new Args());
                }
                #endregion
                PatientDetails_UC pat = new PatientDetails_UC();
                pat.setPatValues();
                pat.Dock = DockStyle.Fill;
                INTUSOFT.Custom.Controls.BaseGradientForm f = new INTUSOFT.Custom.Controls.BaseGradientForm();
                
                f.Color1 = IVLVariables.GradientColorValues.Color1;
                f.Color2 = IVLVariables.GradientColorValues.Color2;
                f.ColorAngle = IVLVariables.GradientColorValues.ColorAngle;
                f.FontColor = IVLVariables.GradientColorValues.FontForeColor;
                f.FormBorderStyle = FormBorderStyle.FixedSingle;
                f.Controls.Add(pat);
                f.Size = new System.Drawing.Size(750, 650);
                f.StartPosition = FormStartPosition.CenterParent;
                string appLogoFilePath = @"ImageResources\LogoImageResources\IntuSoft.ico";
                if (File.Exists(appLogoFilePath))
                    f.Icon = new System.Drawing.Icon(appLogoFilePath, 256, 256);
                f.StartPosition = FormStartPosition.CenterParent;
                string infoIconLogoFilePath = @"ImageResources\PatientDetailsImageResources\i-info.png";
                if (File.Exists(infoIconLogoFilePath))
                    viewFullInfo_btn.Image = new Bitmap(infoIconLogoFilePath);//, 256, 256);

                //This below line has been added by Darshan on 28-10-2015 Defect no 0000724: NR:View Full Info screen Label is required.
                f.Text = IVLVariables.LangResourceManager.GetString("View_Full_Info_Text", IVLVariables.LangResourceCultureInfo);
                f.MaximizeBox = false;
                f.MinimizeBox = false;

                //#if DEBUG
                //            f.TopMost = false;
                //#else
                //            f.TopMost = true;
                //#endif
                //This below bool variable is added by Darshan on 21-08-2015 to solve Defect no:0000585: annotation window,report window,view full info,user settings window, when trigger press capture is happening.

                IVLVariables.IsAnotherWindowOpen = true;

                if (f.ShowDialog() == DialogResult.Cancel)
                {
                    IVLVariables.IsAnotherWindowOpen = false;
                    //This below bool variable is added by Darshan on 21-08-2015 to solve Defect no:0000585: annotation window,report window,view full info,user settings window, when trigger press capture is happening.
                }
            }
        }

        private void addDiagnosis_btn_Click(object sender, EventArgs e)
        {
            if (!IVLVariables.ivl_Camera.IsCapturing)
            {
                IVLVariables.IsAnotherWindowOpen = true;
                DiagnosisForm d = new DiagnosisForm();
                d.ShowDialog();
                
            }
        }
    }
}
