using System;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using INTUSOFT.Desktop.Properties;
using INTUSOFT.Data.NewDbModel;
using INTUSOFT.Data.Repository;
using INTUSOFT.EventHandler;
using INTUSOFT.Data.Enumdetails;
using System.Collections;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace INTUSOFT.Desktop.Forms
{
    public partial class PatientDetails_UC : UserControl
    {
        #region variables and constants
        decimal defwh;
        static  CreateModifyPatient_UC createModifyPatient_UC = null;
        String[] sdata;
        String[] data;
        String[] Codata;
        String fname = IVLVariables.LangResourceManager.GetString( "defaultFirstName_Textbox_Text",IVLVariables.LangResourceCultureInfo);
        String lname = IVLVariables.LangResourceManager.GetString("defaultLastName_Textbox_Text", IVLVariables.LangResourceCultureInfo);
        IVLEventHandler _eventHandler;
        public  Patient currentPat;
        int mrnCnt = 0;
        RegistryKey key;
        Logger ExceptionLog = LogManager.GetLogger("ExceptionLog");

        #endregion

        public PatientDetails_UC()
        {
            InitializeComponent();
            InitializeResourceString();
            UpdateFontForeColor();
        }

        public void UpdateFontForeColor()
        {
            List<Control> controls = GetControls(this).ToList();
            foreach (Control c in controls)
            {
                if (c is Label)
                {
                    Label l = c as Label;
                    {
                        if (Screen.PrimaryScreen.Bounds.Width == 1920)
                            l.Font = new Font(l.Font.FontFamily.Name, 13f);
                        else if (Screen.PrimaryScreen.Bounds.Width == 1366)
                        {
                            l.Font = new Font(l.Font.FontFamily.Name, 9.50f);
                            this.PatEditComments_lbl.Font = new Font(this.PatEditComments_lbl.Font.FontFamily, 9.80f);
                        }
                        else
                            l.Font = new Font(l.Font.FontFamily.Name, 9f);
                    }
                }
                //if (c is Label)
                {
                    //Label l = c as Label;
                    //if (l.Name == PatientCRUD_ts.Name || l.Name == AllPatients_ts.Name || l.Name == Visits_ts.Name || l.Name == Consultation_ts.Name)
                    {
                        //for (int i = 0; i < controls.Count; i++)
                        {
                            c.ForeColor = IVLVariables.GradientColorValues.FontForeColor;
                            c.Refresh();
                        }

                    }
                }
            }
        }


        /// <summary>
        /// This function will initializes the text to be displayed for all controls present in PatientDetails_UC .
        /// </summary>
        private void InitializeResourceString()
        {
            #region labels initilize string from resources
            groupBox1.Text = IVLVariables.LangResourceManager.GetString("Pat_Details_Text", IVLVariables.LangResourceCultureInfo); 
            mrn_lbl.Text = IVLVariables.LangResourceManager.GetString( "Mrn_Label_Text",IVLVariables.LangResourceCultureInfo);;
            firstName_lbl.Text = IVLVariables.LangResourceManager.GetString("PatientDetailsFirstName_LabelText", IVLVariables.LangResourceCultureInfo); ;
            lastName_lbl.Text = IVLVariables.LangResourceManager.GetString("PatientDetailsLastName_Label_Text", IVLVariables.LangResourceCultureInfo); ;
            gender_lbl.Text = IVLVariables.LangResourceManager.GetString("PatientDetailsGender_Label_Text", IVLVariables.LangResourceCultureInfo); ;
            //occupation_lbl.Text = IVLVariables.LangResourceManager.GetString( "Registration_Occupation_Label_Text",IVLVariables.LangResourceCultureInfo);;
            //income_lbl.Text = IVLVariables.LangResourceManager.GetString( "Registration_Salary_Label_Text",IVLVariables.LangResourceCultureInfo);;
            referredBy_lbl.Text = IVLVariables.LangResourceManager.GetString( "ReferredBy_Label_Text",IVLVariables.LangResourceCultureInfo);;
            comments_lbl.Text = IVLVariables.LangResourceManager.GetString( "Comments_Label_Text",IVLVariables.LangResourceCultureInfo);;
            healthStatus_lbl.Text = IVLVariables.LangResourceManager.GetString( "HealthStatus_Label_Text",IVLVariables.LangResourceCultureInfo);;
            city_lbl.Text = IVLVariables.LangResourceManager.GetString( "City_Label_Text",IVLVariables.LangResourceCultureInfo);;
            state_lbl.Text = IVLVariables.LangResourceManager.GetString( "State_Lable_Text",IVLVariables.LangResourceCultureInfo);;
            country_lbl.Text = IVLVariables.LangResourceManager.GetString( "Country_Label_Text",IVLVariables.LangResourceCultureInfo);;
            address1_lbl.Text = IVLVariables.LangResourceManager.GetString("PatientDetailsAddress_Label_Text", IVLVariables.LangResourceCultureInfo); ;
            address2_lbl.Text = IVLVariables.LangResourceManager.GetString( "Address2_Label_Text",IVLVariables.LangResourceCultureInfo);;
            Address_gbx.Text = IVLVariables.LangResourceManager.GetString("AddressDetails_Text", IVLVariables.LangResourceCultureInfo); ;
            pincode_lbl.Text = IVLVariables.LangResourceManager.GetString("PinCode_Label_Text", IVLVariables.LangResourceCultureInfo);
            rightDiagnosis_lbl.Text = IVLVariables.LangResourceManager.GetString("RightEye_Lbl_Text", IVLVariables.LangResourceCultureInfo);
            leftDiagnosis_lbl.Text = IVLVariables.LangResourceManager.GetString("LeftEye_Lbl_Text", IVLVariables.LangResourceCultureInfo); 

            #endregion
        }

        /// <summary>
        /// This method will set the active patient details into the labels text.
        /// </summary>
        public void setPatValues()
        {
            try
            {
                if (NewDataVariables.Patients.Count != 0)
                {
                    currentPat = NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0];
                    //PatEditMrn_lbl.Text = NewDataVariables.Active_PatientIdentifier.value;
                    PatEditMrn_lbl.Text = currentPat.identifiers.ToList()[0].value;
                    PatEditFirstName_lbl.Text = currentPat.firstName;
                    PatEditLastName_lbl.Text = currentPat.lastName;
                    #region 0001699 if the user setting value of isAgeSelect is true it will diaplay age else to show date of birth
                    if (Convert.ToBoolean(IVLVariables.CurrentSettings.UserSettings._IsAgeSelect.val))
                    {
                        age_lbl.Text = IVLVariables.LangResourceManager.GetString("PatientDetailsAge_Label_Text", IVLVariables.LangResourceCultureInfo); ;

                        PatAge_lbl.Text = (DateTime.Now.Year - currentPat.birthdate.Year).ToString();
                    }
                    else
                    {
                        PatAge_lbl.Text = currentPat.birthdate.ToString("dd/MM/yyyy");
                        age_lbl.Text = IVLVariables.LangResourceManager.GetString("dob_radio_button_Text", IVLVariables.LangResourceCultureInfo) + " :";

                    }
                    #endregion
                    string gen;
                    if (currentPat.gender == 'M')
                        gen = "Male";
                    else
                        gen = "Female";
                    PatEditGender_lbl.Text = gen;
                    //if (NewDataVariables.Active_Patient.primaryPhoneNumber != null)
                    //    PatEditMobile_lbl.Text = NewDataVariables.Active_Patient.primaryPhoneNumber.ToString();
                    //else
                    //    PatEditMobile_lbl.Text = "0";
                    List<person_attribute> attributes = currentPat.attributes.ToList();
                    //attributes = NewDataVariables._Repo.GetByCategory<person_attribute>("person", NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0]).ToList();
                    //if (NewDataVariables.Active_Patient.primaryEmailId != null)
                    //    PatEmail_lbl.Text = NewDataVariables.Active_Patient.primaryEmailId;
                    //else
                    //    PatEmail_lbl.Text = IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo);
                    foreach (person_attribute item in attributes)//This for loop has been added to display the patient attributes.
                    {
                        if (item.attributeType.personAttributeTypeId == (int)PatientAttributesType.Income)
                        {
                            //if (item.value != null)
                            //    PatEditIncome_lbl.Text = item.value;
                            //else
                            //    PatEditIncome_lbl.Text = (INTUSOFT.Data.Common.GetDescription(SalaryEnum.No_Salary));
                        }
                        else if (item.attributeType.personAttributeTypeId == (int)PatientAttributesType.Occupation)
                        {
                            //if (item.value != null)
                            //    PatOccupation_lbl.Text = item.value;
                            //else
                            //    PatOccupation_lbl.Text = (INTUSOFT.Data.Common.GetDescription(OccupationEnum.Occupation_Not_Selected));
                        }
                        else if (item.attributeType.personAttributeTypeId == (int)PatientAttributesType.Height)
                        {
                            //if (item.value != null)
                            //Patheight_lbl.Text = item.value;
                            //else
                            //Patheight_lbl.Text = "1";
                        }
                        else if (item.attributeType.personAttributeTypeId == (int)PatientAttributesType.Weight)
                        {
                            //if (item.value != null)
                            //    Patweight_lbl.Text = item.value;
                            //else
                            //    Patweight_lbl.Text = "1";
                        }
                        else if (item.attributeType.personAttributeTypeId == (int)PatientAttributesType.Comments)
                        {
                            if (item.value != null)
                            {

                                PatEditComments_lbl.Text = item.value.Replace("\n", "");// Replacing \n with space to avoid truncation of the comments data to fix defect 0001485
                            }
                            else
                                PatEditComments_lbl.Text = IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo);
                        }
                        else if (item.attributeType.personAttributeTypeId == (int)PatientAttributesType.Landline)
                        {
                            if (item.value != null)
                            {
                                //string phone = item.value;

                                //string[] stdcodeandPhone = phone.Split(' ');
                                //if (stdcodeandPhone[1] != null)
                                //    PatEditLandLine_lbl.Text = stdcodeandPhone[1].ToString();
                                //else
                                //    PatEditLandLine_lbl.Text = "0";
                                //if (stdcodeandPhone[0] != null)
                                //    PatEditStdCode_lbl.Text = stdcodeandPhone[0].ToString();
                                //else
                                //    PatEditStdCode_lbl.Text = "0";
                            }
                            else
                            {
                                //PatEditStdCode_lbl.Text = "0";
                                //PatEditLandLine_lbl.Text = "0";
                            }
                        }

                    }
                    //String s = NewDataVariables.Active_PersonAddressModel.cityVillage;
                    List<PersonAddressModel> perAddresses = currentPat.addresses.ToList();
                    String s = perAddresses[0].cityVillage;
                    //This below block of code has been added to display the address of the patient
                    {
                        if (s != null)
                            Patcity_lbl.Text = s;
                        else
                            Patcity_lbl.Text = INTUSOFT.Data.Common.GetDescription(CityEnum.City_Not_Selected);
                        String strst = perAddresses[0].stateProvince;
                        if (strst != null)
                            Patstate_lbl.Text = strst;
                        else
                            Patstate_lbl.Text = INTUSOFT.Data.Common.GetDescription(StateEnum.State_Not_Selected);
                        if (perAddresses[0].country != null)
                            Patcountry_lbl.Text = perAddresses[0].country;
                        else
                            Patcountry_lbl.Text = INTUSOFT.Data.Common.GetDescription(CountryEnum.Country_Not_Selected);
                        long pincode = Convert.ToInt64(perAddresses[0].postalCode);
                        Patpincode_lbl.Text = pincode.ToString();
                        if (perAddresses[0].line1 != null)
                        {
                            if (perAddresses[0].line1.Contains('_'))
                            {
                                string[] address = perAddresses[0].line1.Split('_');
                                PatEditDoorStreet_lbl.Text = address[0];
                                if (address.Length == 2)
                                    PatEditAddressArea_lbl.Text = address[1];
                            }
                            else
                            {
                                if (perAddresses[0].line1 != null)
                                {
                                    PatEditDoorStreet_lbl.Text = perAddresses[0].line1;
                                    if (perAddresses[0].line2 != null)
                                        PatEditAddressArea_lbl.Text = perAddresses[0].line2;
                                    else
                                        PatEditAddressArea_lbl.Text = "";
                                }
                                else
                                {
                                    PatEditDoorStreet_lbl.Text = IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo);
                                    PatEditAddressArea_lbl.Text = "";
                                }
                            }
                        }
                        else
                        {
                            PatEditDoorStreet_lbl.Text = IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo);
                            PatEditAddressArea_lbl.Text = "";
                        }
                    }
                    char[] a = { '.', '0' };
                    //PatreferredBy_lbl.Text = NewDataVariables.Active_Patient.referred_by.first_name;
                    if (currentPat.referredBy != null)
                    {
                        PatreferredBy_lbl.Text = currentPat.referredBy.firstName;
                    }
                    else
                    {
                        PatreferredBy_lbl.Text = IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo);
                    }
                    if (currentPat.historyAilments != null)
                        PathealthStatus_lbl.Text = currentPat.historyAilments;
                    else
                        PathealthStatus_lbl.Text = IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo);

                    #region The flowLayput for left and right diagnosis are cleared  from previous details in order fix defect 0001782
                    leftDiagnosis_flp.Controls.Clear();
                    rightDiagnosis_flp.Controls.Clear();
                    #endregion
                    List<PatientDiagnosis> patDiagnosis = currentPat.diagnosis.Where(x => x.voided == false).ToList();
                    if (patDiagnosis.Count > 0)
                    {
                        string leftDiagnosis = string.Empty;
                        string rightDiagnosis = string.Empty;
                        DiagnosisStructure dR = new DiagnosisStructure();
                        DiagnosisStructure dL = new DiagnosisStructure();
                        for (int i = 0; i < patDiagnosis.Count; i++)
                        {
                            leftDiagnosis = patDiagnosis[i].diagnosisValueRight;
                            rightDiagnosis = patDiagnosis[i].diagnosisValueLeft;
                            //dL = (DiagnosisStructure)Newtonsoft.Json.JsonConvert.DeserializeObject(leftDiagnosis, typeof(DiagnosisStructure));
                            dL.DiagnosisList.AddRange(leftDiagnosis.Split(',').ToList());
                            dR.DiagnosisList.AddRange(rightDiagnosis.Split(',').ToList());
                        }
                        dL.DiagnosisList = dL.DiagnosisList.Distinct().ToList();
                        dR.DiagnosisList = dR.DiagnosisList.Distinct().ToList();
                        for (int i = 0; i < dL.DiagnosisList.Count; i++)
                        {
                            Label l = new Label();
                            l.Text = dL.DiagnosisList[i];
                            l.AutoSize = false;
                            l.AutoEllipsis = true;
                            leftDiagnosis_flp.Controls.Add(l);
                        }
                        for (int i = 0; i < dR.DiagnosisList.Count; i++)
                        {
                            Label r = new Label();
                            r.Text = dR.DiagnosisList[i];
                            r.AutoSize = false;
                            r.AutoEllipsis = true;
                            rightDiagnosis_flp.Controls.Add(r);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex,ExceptionLog);
//                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
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

        List<Control> controls;
        private void groupBox1_Paint(object sender, PaintEventArgs e)
        {
            {
                 //Graphics gfx = e.Graphics;
                //Pen p = new Pen(Color.Black, 3);
                //gfx.DrawLine(p, 0, 12, 0, e.ClipRectangle.Height);
                //gfx.DrawLine(p, 0, 12, 10, 12);
                //gfx.DrawLine(p, 165, 12, e.ClipRectangle.Width - 2, 12);
                //gfx.DrawLine(p, e.ClipRectangle.Width - 2, 12, e.ClipRectangle.Width - 2, e.ClipRectangle.Height - 2);
                //gfx.DrawLine(p, e.ClipRectangle.Width - 2, e.ClipRectangle.Height - 2, 0, e.ClipRectangle.Height - 2);  
            }
        }

        private void Address_gbx_Paint(object sender, PaintEventArgs e)
        {
            //Graphics gfx = e.Graphics;
            //Pen p = new Pen(Color.Black, 3);
            //gfx.DrawLine(p, 0, 9, 0, e.ClipRectangle.Height);
            //gfx.DrawLine(p, 0, 9, 10, 9);
            //gfx.DrawLine(p, 135, 9, e.ClipRectangle.Width - 2, 9);
            //gfx.DrawLine(p, e.ClipRectangle.Width - 2, 9, e.ClipRectangle.Width - 2, e.ClipRectangle.Height - 2);
            //gfx.DrawLine(p, e.ClipRectangle.Width - 2, e.ClipRectangle.Height - 2, 0, e.ClipRectangle.Height - 2);  
        }
    }
}