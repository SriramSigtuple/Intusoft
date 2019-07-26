using System;
using System.Collections.Generic;
using INTUSOFT.Desktop.Properties;
using System.IO;
using System.ComponentModel;
using System.Data;
using INTUSOFT.Data.NewDbModel;
using INTUSOFT.Data.Repository;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTUSOFT.Custom.Controls;
using System.Windows.Forms;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace INTUSOFT.Desktop.Forms
{
    //This form has been added to reduce conflicts between patient gridview and patient details.
    public partial class CreatePatientDetails_form : BaseGradientForm
    {
        public int currentIndx = 0;
        CreateModifyPatient_UC createPatientUC;
        int actualWidth = 0;
        int actualHeight = 0;
        Logger ExceptionLog = LogManager.GetLogger("ExceptionLog");
        public CreatePatientDetails_form()
        {
            InitializeComponent();
            this.Text = IVLVariables.LangResourceManager.GetString( "Pat_Details_Text",IVLVariables.LangResourceCultureInfo);

            string appLogoFilePath = @"ImageResources\LogoImageResources\IntuSoft.ico";
            if (File.Exists(appLogoFilePath))
                this.Icon = new System.Drawing.Icon(appLogoFilePath, 256, 256);
            createPatientUC = CreateModifyPatient_UC.getInstance();// A single instance of user control is created instead of calling it repeatedly during populate details by sriram
            createPatientUC.Dock = DockStyle.Fill;
            actualWidth = this.Width;
            actualHeight = this.Height;
            
            
            this.FormClosing += CreatePatientDetails_form_FormClosing;
            
        }

        void CreatePatientDetails_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Controls.Clear();
        }

        //The below function has been added to display existing patient details or to register a new patient.
        public void Populate_Details(Patient p,bool IsCreatePatient)
        {
            try
            {
                this.Text = IVLVariables.LangResourceManager.GetString("Pat_Details_Text", IVLVariables.LangResourceCultureInfo);
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                createPatientUC.CurrentIndx = this.currentIndx;
                createPatientUC.isCreatePat = IsCreatePatient;
                this.Width = actualWidth;
                this.Height = actualHeight;
                if (IsCreatePatient)
                {
                    createPatientUC.ClearAllFields();
                    createPatientUC.setDefaultValues();
                }
                else
                {
                    createPatientUC.currentIdentifier = NewDataVariables.Active_PatientIdentifier;
                    createPatientUC.currentPersonAddress = NewDataVariables.Active_PersonAddressModel;
                    createPatientUC.currentPersonAttribute = NewDataVariables._Repo.GetByCategory<person_attribute>("person", p).ToList();
                    createPatientUC.setPatDetails(p);
                    createPatientUC.enableConrols();
                }
                this.Controls.Add(createPatientUC);
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex,ExceptionLog);
//                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }
 
        public void Advanceserach_details()
        {
            try
            {
                this.Text = IVLVariables.LangResourceManager.GetString("AdvanceSearch_linklabel_Text", IVLVariables.LangResourceCultureInfo);
                this.Width = 450;//This has been changed from 385 to 390 since the year was not clear.
                this.Height = 228;
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                AdvanceSerach_UC advanceUC = AdvanceSerach_UC.getInstance();
                advanceUC.Advserach_reset();
                advanceUC.Dock = DockStyle.Fill;
                this.Controls.Add(advanceUC);
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
//                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        private void CreatePatientDetails_form_Load(object sender, EventArgs e)
        {
            createPatientUC.UpdateFontForeColor();
        }
    }
}
