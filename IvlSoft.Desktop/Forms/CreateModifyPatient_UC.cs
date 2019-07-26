using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
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
using Common;
using NLog;
using NLog.Config;
using NLog.Targets;
using NHibernate.Util;

namespace INTUSOFT.Desktop.Forms
{
    public partial class CreateModifyPatient_UC : UserControl
    {
        #region Constands and Variables
        decimal defwh;
      static  CreateModifyPatient_UC createModifyPatient_UC = null;
      public int CurrentIndx = 0;
      //EnumToStringClass enums;
        //String city;
      public bool isValid = false;
      public bool isclear = false;
      public bool isUpdate = false;
      public  bool isFieldValueChanged = false;
        String fname=null;
        String lname =null;
        IVLEventHandler _eventHandler;
       public  Patient currentPat;
       public patient_identifier currentIdentifier;
       public PersonAddressModel currentPersonAddress;
       public List<person_attribute> currentPersonAttribute;
       public person_attribute_type attributeType;
        public person_attribute personAttribute;
        string[] Occupation_description_values = null;
          //public  List<person_attribute> personAttributesList=new List<person_attribute>();
          //personAttributesList=NewDataVariables._personattributeRepo.GetAll().ToList();
          //int z = personAttributesList.Count;
        bool isempty = false;
        public bool isCreatePat = false;
        int mrnCnt = 0;
        List<Control> controls;
        RegistryKey key;
        Logger ExceptionLog = LogManager.GetLogger("ExceptionLog");

        #endregion
        public CreateModifyPatient_UC()
        {
            InitializeComponent();
            InitializeResourceString();
            _eventHandler = IVLEventHandler.getInstance();
            defwh = Convert.ToDecimal(1.0);
            DateTime minDOB = new DateTime(1915, 1, 1, 0, 0, 0);
            DateTime maxDOB = new DateTime(DateTime.Now.Year-2, 12, 31, 0, 0, 0);
            dateOfBirth_dtPicker.MinDate = minDOB;
            dateOfBirth_dtPicker.MaxDate =maxDOB;
            fname = IVLVariables.LangResourceManager.GetString("defaultFirstName_Textbox_Text", IVLVariables.LangResourceCultureInfo); ;
            lname = IVLVariables.LangResourceManager.GetString( "defaultLastName_Textbox_Text",IVLVariables.LangResourceCultureInfo);;
            //TimeSpan ts = new TimeSpan(DateTime.Now.Hour,DateTime.Now.Minute, DateTime.Now.Second);
            dateOfBirth_dtPicker.Value = maxDOB;// +ts;// +dateOfBirth_dtPicker.Value.TimeOfDay;
            age_nud.Maximum = DateTime.Now.Year - dateOfBirth_dtPicker.MinDate.Year;
            age_nud.Minimum = 2;
            PatEditLastName_tbx.CharacterCasing = CharacterCasing.Upper;
            PatEditFirstName_tbx.CharacterCasing = CharacterCasing.Upper;
            PatEditMrn_tbx.CharacterCasing = CharacterCasing.Upper;
            Email_tbx.CharacterCasing = CharacterCasing.Lower;
            //This below code has been changed by Darshan on 07-09-2015 as per requirement suggested in Defect no 0000460: CR:The underscore must be removed in the Patient details fields.
            GenderEnum[]  Gender_Enum_values = Enum.GetValues(typeof(GenderEnum)) as GenderEnum[];
            string[] Gender_description_values = Gender_Enum_values.OfType<object>().Select(o => o.ToString()).ToArray();
            for(int i=0;i<Gender_Enum_values.Length;i++)
            {
                if (Gender_description_values[i].Contains('_'))
                    Gender_description_values[i] = INTUSOFT.Data.Common.GetDescription(Gender_Enum_values[i]);
            }
             PatEditGender_cmbx.DataSource = Gender_description_values;
             //This below code has been changed by Darshan on 07-09-2015 as per requirement suggested in Defect no 0000460: CR:The underscore must be removed in the Patient details fields.
            OccupationEnum[] Occupation_Enum_values = Enum.GetValues(typeof(OccupationEnum)) as OccupationEnum[];
           Occupation_description_values = Occupation_Enum_values.OfType<object>().Select(o => o.ToString()).ToArray();
            for (int i = 0; i < Occupation_Enum_values.Length; i++)
            {
               if (Occupation_description_values[i].Contains('_'))
                    Occupation_description_values[i] = INTUSOFT.Data.Common.GetDescription(Occupation_Enum_values[i]);
            }
            Occupation_combobx.DataSource = Occupation_description_values;
            //OccupationEnum[] Occupation_Enum_values = Enum.GetValues(typeof(OccupationEnum)) as OccupationEnum[];
            //string[] Occupation_description_values = Gender_Enum_values.OfType<object>().Select(o => o.ToString()).ToArray();
            //for (int i = 0; i < Gender_Enum_values.Length; i++)
            //{
            //    if (Occupation_description_values[i].Contains('_'))
            //        Occupation_description_values[i] = INTUSOFT.Data.Common.GetDescription(Occupation_Enum_values[i]);
            //}
            //This below code has been changed by Darshan on 07-09-2015 as per requirement suggested in Defect no 0000460: CR:The underscore must be removed in the Patient details fields.
            CityEnum[] City_Enum_values = Enum.GetValues(typeof(CityEnum)) as CityEnum[];
            string[] City_description_values = City_Enum_values.OfType<object>().Select(o => o.ToString()).ToArray();
            for (int i = 0; i < City_Enum_values.Length; i++)
            {
                if (City_description_values[i].Contains('_'))
                    City_description_values[i] = INTUSOFT.Data.Common.GetDescription(City_Enum_values[i]);
            }
            City_combobx.DataSource = City_description_values;
            //This below code has been changed by Darshan on 07-09-2015 as per requirement suggested in Defect no 0000460: CR:The underscore must be removed in the Patient details fields.
            StateEnum[] State_Enum_values = Enum.GetValues(typeof(StateEnum)) as StateEnum[];
            string[] State_description_values = State_Enum_values.OfType<object>().Select(o => o.ToString()).ToArray();
            for (int i = 0; i < State_Enum_values.Length; i++)
            {
                if (State_description_values[i].Contains('_'))
                    State_description_values[i] = INTUSOFT.Data.Common.GetDescription(State_Enum_values[i]);
            }
            State_combobx.DataSource = State_description_values;
            //This below code has been changed by Darshan on 07-09-2015 as per requirement suggested in Defect no 0000460: CR:The underscore must be removed in the Patient details fields.
            CountryEnum[] Country_Enum_values = Enum.GetValues(typeof(CountryEnum)) as CountryEnum[];
            string[] Country_description_values = Country_Enum_values.OfType<object>().Select(o => o.ToString()).ToArray();
            for (int i = 0; i < Country_Enum_values.Length; i++)
            {
                if (Country_description_values[i].Contains('_'))
                    Country_description_values[i] = INTUSOFT.Data.Common.GetDescription(Country_Enum_values[i]);
            }
            Country_combobx.DataSource = Country_description_values;
            //enums = new EnumToStringClass(typeof(SalaryEnum));
            //List<String> incomelist = MyClass.GetEnumDescription(SalaryEnum.Salary_0_to_1000).ToList() ;
            //This below code has been changed by Darshan on 07-09-2015 as per requirement suggested in Defect no 0000460: CR:The underscore must be removed in the Patient details fields.
            SalaryEnum[] Salary_Enum_values = Enum.GetValues(typeof(SalaryEnum)) as SalaryEnum[];
            string[] Salary_description_values = Salary_Enum_values.OfType<object>().Select(o => o.ToString()).ToArray();
            for (int i = 0; i < Salary_Enum_values.Length; i++)
            {
                if (Salary_description_values[i].Contains('_'))
                    Salary_description_values[i] = INTUSOFT.Data.Common.GetDescription(Salary_Enum_values[i]);
            }
            #region numeric up downs textchangeevent
            age_nud.TextChanged += new System.EventHandler(age_nud_TextChanged);
            height_nud.TextChanged += new System.EventHandler(height_nud_TextChanged);
            weight_nud.TextChanged += new System.EventHandler(weight_nud_TextChanged);
            #endregion
            PatEditIncome_cmbx.DataSource = Salary_description_values;
            UpdateFontForeColor();
        }
        #region Public methods



        public void UpdateFontForeColor()
        {
            List<Control> controls = GetControls(this).ToList();
            foreach (Control c in controls)
            {
                if (c.Name != mandatory_lbl1.Name && c.Name != mandatory_lbl2.Name && c.Name != mandatory_lbl3.Name && c.Name != mandatory_lbl4.Name && c.Name != mandatory_lbl5.Name && c.Name != PatEditGender_cmbx.Name && c.Name != PatEditIncome_cmbx.Name && c.Name != Country_combobx.Name && c.Name != City_combobx.Name && c.Name != Occupation_combobx.Name && c.Name != State_combobx.Name && c.Name != Update_btn.Name && c.Name != clearFields_btn.Name && c.Name != Reload_btn.Name && c.Name != cancel_btn.Name)
                    c.ForeColor = IVLVariables.GradientColorValues.FontForeColor;
                //if (c.Name != PatEditGender_cmbx.Name && c.Name != PatEditIncome_cmbx.Name && c.Name != Country_combobx.Name && c.Name != City_combobx.Name && c.Name != Occupation_combobx.Name && c.Name != State_combobx.Name && c.Name != Update_btn.Name && c.Name != clearFields_btn.Name && c.Name != Reload_btn.Name && c.Name != cancel_btn.Name)
                //    c.ForeColor = IVLVariables.GradientColorValues.FontForeColor;
                if (c is TextBox || c is RichTextBox)
                    c.ForeColor = Color.Black;
                if (c is NumericUpDown)
                    c.ForeColor = Color.Black;
            }
        }

        /// <summary>
        /// This was being used to set the save button name in createpatientmodify_uc.
        /// </summary>
        public void resetUpdateButtonText()
        {
            this.Update_btn.Text = IVLVariables.LangResourceManager.GetString("Update_Button_Text", IVLVariables.LangResourceCultureInfo);
        }
        /// <summary>
        /// This function is used to return the controls used in the Createpatient form.
        /// </summary>
        /// <param name="form">form name</param>
        /// <returns>returns controls of the from IEnumerable<Control></returns>
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
        //This below method has been added by Darshan ion 19-08-2015 to solve Defect no 0000503: Modification made in the IVL config file creating Same entries to be saved multiple times.
        /// <summary>
        /// This function is used to check weather the current MRN exists or not.
        /// </summary>
        /// <returns>true if MRN exists else false if MRN donot exists</returns>
        public bool IsMRN_Exists()
        {
            //List<Patient> patients = DataVariables._patientRepo.GetAll().ToList<Patient>();
            if (NewDataVariables.Identifier.Where(x => x.value.Equals(PatEditMrn_tbx.Text)).Count() == 0)//Checks if the MRN already exists.
                return false;
            else
                return true;
        }

        /// <summary>
        /// This function which will create a single instance of type CreateModifyPatient_UC and returns it if no instance has been created before else it returns exisiting one.
        /// </summary>
        /// <returns>returns a user defined variable of type CreateModifyPatient_UC</returns>
        public static CreateModifyPatient_UC getInstance()
        {
            if (createModifyPatient_UC == null)
                createModifyPatient_UC = new CreateModifyPatient_UC();
            return createModifyPatient_UC;
        }

        /// <summary>
        /// This function is used to clear the data from all controls (except labels) present in create patient window.
        /// </summary>
        public void ClearAllFields()
        {
            try
            {
                cancel_btn.Enabled = true;
                List<Control> controls = GetControls(this).ToList();
                foreach (Control item in controls)
                {
                    //The below code is modified by adding Richtextbox to solve defect no 0000081 and 0000090
                    if ((item is TextBox && item.Name != "PatEditMrn_tbx") || item is ComboBox || item is NumericUpDown || item is DateTimePicker || item is RichTextBox)
                    {
                        if (item is TextBox || item is RichTextBox)
                        {
                            item.Text = "";
                        }
                        else if (item is ComboBox)
                        {
                            item.Text = "";
                            //INTUSOFT.Data.Common.GetDescription has been added because the comboboxes were not getting updated when clear was pressed.
                            PatEditGender_cmbx.SelectedItem = INTUSOFT.Data.Common.GetDescription(GenderEnum.Not_Selected);
                            Occupation_combobx.SelectedItem = INTUSOFT.Data.Common.GetDescription(OccupationEnum.Occupation_Not_Selected);
                            PatEditIncome_cmbx.SelectedItem = INTUSOFT.Data.Common.GetDescription(SalaryEnum.No_Salary);
                        }
                        else if (item is NumericUpDown)
                        {
                            item.Text = "";
                            DateTime maxDOB = new DateTime(DateTime.Now.Year - 2, 1, 1, 0, 0, 0);
                            dateOfBirth_dtPicker.Value = maxDOB;
                            dateOfBirth_dtPicker.Format = DateTimePickerFormat.Custom;
                            dateOfBirth_dtPicker.CustomFormat = " ";
                            age_nud.Value = 2;
                            weight_nud.Value = (decimal)1.0;
                            height_nud.Value = (decimal)1.0;
                            item.Text = "";
                        }
                    }
                    else if (isCreatePat && item.Name == "PatEditMrn_tbx" && !Convert.ToBoolean(IVLVariables.CurrentSettings.UserSettings._IsMrnConfigurable.val))
                    {
                        key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Intusoft", true);
                        mrnCnt = Convert.ToInt32(key.GetValue("MRN").ToString()) + 1;
                        key.Close();
                        //This below code has been modified by Darshan 17-08-2015 to solve defect no 0000574: system is getting crashed.
                        string newMRN = IVLVariables.CurrentSettings.UserSettings._MrnString.val + (IVLVariables.CurrentSettings.UserSettings._MrnCnt.val).ToString();
                        {
                            if (INTUSOFT.Data.Repository.NewDataVariables.Identifier != null)
                            {
                                if (IVLVariables.CurrentSettings.UserSettings._MrnString.val.Length > 25)
                                {
                                    IVLVariables.CurrentSettings.UserSettings._MrnString.val = IVLVariables.CurrentSettings.UserSettings._MrnString.val.Remove(24);
                                }
                                if (INTUSOFT.Data.Repository.NewDataVariables.Identifier.Where(x => (x.value == newMRN && x.voided == false)).ToList().Count != 0)
                                {
                                    PatEditMrn_tbx.Text = IVLVariables.CurrentSettings.UserSettings._MrnString.val + (mrnCnt + 1).ToString();
                                }
                                PatEditMrn_tbx.Text = IVLVariables.CurrentSettings.UserSettings._MrnString.val + (mrnCnt).ToString();
                            }
                            else
                            {
                                PatEditMrn_tbx.Text = IVLVariables.CurrentSettings.UserSettings._MrnString.val + (mrnCnt).ToString();
                            }
                            item.Enabled = false;
                        }
                    }
                    Update_btn.Text = IVLVariables.LangResourceManager.GetString("Update_SaveUpdate", IVLVariables.LangResourceCultureInfo);
                    isUpdate = true;
                }
            }
            catch (Exception ex)
            {
                //ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
            }
        }

        /// <summary>
        /// This function is used to set the details of the existing patient to all controls (except labels) in the create patient window.
        /// </summary>
        /// <param name="p">p gives details about the existing patient</param>
        public void setPatDetails(Patient p)
        {
            try
            {
                
                dateOfBirth_dtPicker.Format = DateTimePickerFormat.Custom;
                dateOfBirth_dtPicker.CustomFormat = IVLVariables.LangResourceManager.GetString("DateFormat_Text", IVLVariables.LangResourceCultureInfo);
                currentPat = p;
                List<person_attribute> personAttributes = new List<person_attribute>();
                personAttributes = p.attributes.ToList();// NewDataVariables._Repo.GetByCategory<person_attribute>("person", NewDataVariables.Active_Patient).ToList(); ;
                PatEditMrn_tbx.Text = NewDataVariables.Active_PatientIdentifier.value;
                PatEditFirstName_tbx.Text = p.firstName;
                PatEditLastName_tbx.Text = p.lastName;
                if (!age_nud.Enabled)
                {
                    age_nud.Enabled = true;
                }
                int age_verify = (int)(DateTime.Now.Year - p.birthdate.Year);
                if (age_verify > 3)
                    age_nud.Value = (decimal)(DateTime.Now.Year - p.birthdate.Year);
                else
                    age_nud.Value = (decimal)3;
                string gen;
                if (p.gender == 'M')
                    gen = GenderEnum.Male.ToString();
                else
                    gen = GenderEnum.Female.ToString();
                GenderEnum g = GenderEnum.Female;
                if (Enum.GetNames(typeof(GenderEnum)).Contains(gen))
                {
                    g = (GenderEnum)Enum.Parse(typeof(GenderEnum), gen);
                    PatEditGender_cmbx.SelectedItem = gen;
                }
                else
                    PatEditGender_cmbx.SelectedItem = GenderEnum.Not_Selected;
                if (string.IsNullOrEmpty(NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].primaryPhoneNumber))
                    PatEditMobile_tbx.Text = "0";
                else
                    PatEditMobile_tbx.Text = NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].primaryPhoneNumber;
                if (NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].primaryEmailId == IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo) || NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].primaryEmailId == null)
                {
                    Email_tbx.CharacterCasing = CharacterCasing.Normal;
                    if (NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].primaryEmailId != null)
                        Email_tbx.Text = NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].primaryEmailId;
                    else
                        Email_tbx.Text = IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo);
                }
                else
                {
                    Email_tbx.Text = NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].primaryEmailId;
                }
                foreach (person_attribute item in personAttributes)//This for loop has been added to display the patient attributes.
                {
                    if (item.attributeType.personAttributeTypeId == (int)PatientAttributesType.Income)
                    {
                        string income_str = item.value;
                        if (Enum.GetNames(typeof(SalaryEnum)).Contains(income_str))
                        {
                            SalaryEnum income = (SalaryEnum)Enum.Parse(typeof(SalaryEnum), item.value);
                            if (income.ToString().Contains('_'))
                                PatEditIncome_cmbx.SelectedItem = INTUSOFT.Data.Common.GetDescription(income);
                            else
                                PatEditIncome_cmbx.SelectedItem = item.value;
                        }
                        else
                        {
                            if (item.value != null)
                                PatEditIncome_cmbx.SelectedItem = item.value;
                            else
                                PatEditIncome_cmbx.SelectedItem = (INTUSOFT.Data.Common.GetDescription(SalaryEnum.No_Salary));
                        }
                    }
                    else if (item.attributeType.personAttributeTypeId == (int)PatientAttributesType.Occupation)
                    {
                        string str = item.value;
                        //This below code has been changed by Darshan on 07-09-2015 as per requirement suggested in Defect no 0000460: CR:The underscore must be removed in the Patient details fields.
                        if (Enum.GetNames(typeof(OccupationEnum)).Contains(str))
                        {
                            OccupationEnum occupation = OccupationEnum.Occupation_Not_Selected;
                            bool returnVal = Enum.TryParse<OccupationEnum>(item.value, out occupation);
                            if (returnVal)
                            {
                                if (occupation.ToString().Contains('_'))
                                    Occupation_combobx.SelectedItem = INTUSOFT.Data.Common.GetDescription(occupation);
                                else
                                    Occupation_combobx.SelectedItem = item.value;
                            }

                        }

                        else
                        {
                            if (item.value != null)
                            {
                                if (Enum.GetNames(typeof(OccupationEnum)).Contains(str))
                                    Occupation_combobx.SelectedItem = item.value;
                                else
                                {
                                    Occupation_combobx.SelectedItem = OccupationEnum.Others.ToString();
                                    otherOccupationDetails_tbx.Text = item.value;
                                }

                            }


                            else
                                Occupation_combobx.SelectedItem = (INTUSOFT.Data.Common.GetDescription(OccupationEnum.Occupation_Not_Selected));
                        }
                    }
                    else if (item.attributeType.personAttributeTypeId == (int)PatientAttributesType.Height)
                    {
                        if (item.value != null)
                            height_nud.Value = Convert.ToDecimal(item.value);
                        else
                            height_nud.Value = Convert.ToDecimal(1);

                    }
                    else if (item.attributeType.personAttributeTypeId == (int)PatientAttributesType.Weight)
                    {
                        if (item.value != null)
                            weight_nud.Value = Convert.ToDecimal(item.value);
                        else
                            weight_nud.Value = Convert.ToDecimal(1);
                    }
                    else if (item.attributeType.personAttributeTypeId == (int)PatientAttributesType.Comments)
                    {
                        if (item.value != null)
                            PatEditcomments_tbx.Text = item.value;
                        else
                            PatEditcomments_tbx.Text = IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo);
                    }
                    else if (item.attributeType.personAttributeTypeId == (int)PatientAttributesType.Landline)
                    {
                        //long phone = Convert.ToInt6(item.value);
                        if (item.value != null)
                        {
                            string phone = item.value;
                            string[] stdcodeandPhone = phone.Split(' ');
                            if (stdcodeandPhone.Length > 1)
                            {
                                if (stdcodeandPhone[1] != null)
                                    PatEditLandLine_tbx.Text = stdcodeandPhone[1].ToString();
                                else
                                    PatEditLandLine_tbx.Text = "0";
                            }
                            if (stdcodeandPhone[0] != null)
                                PatEditStdCode_tbx.Text = stdcodeandPhone[0].ToString();
                            else
                                PatEditStdCode_tbx.Text = "0";
                        }
                        else
                        {
                            PatEditLandLine_tbx.Text = "0";
                            PatEditStdCode_tbx.Text = "0";
                        }
                    }
                }
                if (string.IsNullOrEmpty(PatEditLandLine_tbx.Text))
                    PatEditLandLine_tbx.Text = "0";
                if (string.IsNullOrEmpty(PatEditStdCode_tbx.Text))
                    PatEditStdCode_tbx.Text = "0";
                if (string.IsNullOrEmpty(PatEditcomments_tbx.Text))
                    PatEditcomments_tbx.Text = IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo);
                if (NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].referredBy != null)
                {
                    referredBy_tbx.Text = NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].referredBy.firstName;
                }
                else
                {
                    referredBy_tbx.Text = IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo);
                }

                //This below code has been changed by Darshan on 07-09-2015 as per requirement suggested in Defect no 0000460: CR:The underscore must be removed in the Patient details fields.
                if (Convert.ToBoolean(IVLVariables.CurrentSettings.UserSettings._IsAgeSelect.val))
                    Age_radio.Checked = Convert.ToBoolean(IVLVariables.CurrentSettings.UserSettings._IsAgeSelect.val);
                else
                    Dob_radio.Checked = !(Convert.ToBoolean(IVLVariables.CurrentSettings.UserSettings._IsAgeSelect.val));
                String s = NewDataVariables.Active_PersonAddressModel.cityVillage;
                //This below code has been changed by Darshan on 07-09-2015 as per requirement suggested in Defect no 0000460: CR:The underscore must be removed in the Patient details fields.
                //This below block of code has been added to display the city of the patient
                if (Enum.GetNames(typeof(CityEnum)).Contains(s))
                {
                    CityEnum city = ((CityEnum)Enum.Parse(typeof(CityEnum), NewDataVariables.Active_PersonAddressModel.cityVillage));
                    if (city.ToString().Contains('_'))
                        City_combobx.Text = INTUSOFT.Data.Common.GetDescription(city);
                    else
                        City_combobx.Text = city.ToString();
                    //City_combobx.Text = ((CityEnum)Enum.Parse(typeof(CityEnum), p.City)).ToString();
                }
                else
                {
                    City_combobx.Text = INTUSOFT.Data.Common.GetDescription(CityEnum.City_Not_Selected);
                }
                City_combobx.SelectionLength = 0;
                String strst = NewDataVariables.Active_PersonAddressModel.stateProvince;
                //This below code has been changed by Darshan on 07-09-2015 as per requirement suggested in Defect no 0000460: CR:The underscore must be removed in the Patient details fields.
                //This below block of code has been added to display the city of the patient
                if (Enum.GetNames(typeof(StateEnum)).Contains(strst))
                {
                    StateEnum state = ((StateEnum)Enum.Parse(typeof(StateEnum), NewDataVariables.Active_PersonAddressModel.stateProvince));
                    if (state.ToString().Contains('_'))
                        State_combobx.Text = INTUSOFT.Data.Common.GetDescription(state);
                    else
                        State_combobx.Text = state.ToString();
                    //State_combobx.Text = ((StateEnum)Enum.Parse(typeof(StateEnum), p.State)).ToString();
                }
                else
                    State_combobx.Text = INTUSOFT.Data.Common.GetDescription(StateEnum.State_Not_Selected);
                State_combobx.SelectionLength = 0;
                String strc = NewDataVariables.Active_PersonAddressModel.country;
                //This below code has been changed by Darshan on 07-09-2015 as per requirement suggested in Defect no 0000460: CR:The underscore must be removed in the Patient details fields.
                //This below block of code has been added to display the city of the patient
                if (Enum.GetNames(typeof(CountryEnum)).Contains(strc))
                {
                    CountryEnum country = (CountryEnum)Enum.Parse(typeof(CountryEnum), NewDataVariables.Active_PersonAddressModel.country);
                    if (country.ToString().Contains('_'))
                        Country_combobx.Text = INTUSOFT.Data.Common.GetDescription(country);
                    else
                        Country_combobx.Text = country.ToString();
                    //Country_combobx.Text = ((CountryEnum)Enum.Parse(typeof(CountryEnum), p.Country)).ToString();
                }
                else
                    Country_combobx.Text = INTUSOFT.Data.Common.GetDescription(CountryEnum.Country_Not_Selected);
                Country_combobx.SelectionLength = 0;//This has been added to stop the country combo box from getting selected when undo is pressed.
                dateOfBirth_dtPicker.Value = p.birthdate;
                //This below block of code has been added to display the city of the patientu
                if (NewDataVariables.Active_PersonAddressModel.line1 != null)
                {
                    if (NewDataVariables.Active_PersonAddressModel.line1.Contains('_'))
                    {
                        string[] address = NewDataVariables.Active_PersonAddressModel.line1.Split('_');
                        PatEditDoorStreet_Tbx.Text = address[0];
                        PatEditAddressArea_tbx.Text = address[1];
                    }
                    else
                    {
                        if (NewDataVariables.Active_PersonAddressModel.line1 != null)
                        {
                            PatEditDoorStreet_Tbx.Text = NewDataVariables.Active_PersonAddressModel.line1;
                            if (NewDataVariables.Active_PersonAddressModel.line2 != null)
                                PatEditAddressArea_tbx.Text = NewDataVariables.Active_PersonAddressModel.line2;
                            else
                                PatEditAddressArea_tbx.Text = "";
                        }
                        else
                        {
                            PatEditDoorStreet_Tbx.Text = IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo);
                            PatEditAddressArea_tbx.Text = "";
                        }
                    }
                }
                else
                {
                    PatEditDoorStreet_Tbx.Text = IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo);
                    PatEditAddressArea_tbx.Text = "";
                }
                long pincode;
                if (Convert.ToInt64(NewDataVariables.Active_PersonAddressModel.postalCode) != null)
                    pincode = Convert.ToInt64(NewDataVariables.Active_PersonAddressModel.postalCode);
                else
                    pincode = 0;
                pincode_tbx.Text = pincode.ToString();
                //referredBy_tbx.Text = p.referred_by.first_name;
                if (p.historyAilments != null)
                    healthStatus_tbx.Text = p.historyAilments;
                else
                    healthStatus_tbx.Text = IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo);
                List<Control> c = GetControls(this).ToList();
                Update_btn.Text = IVLVariables.LangResourceManager.GetString("Update_SaveUpdate", IVLVariables.LangResourceCultureInfo);
                foreach (Control item in c)
                {
                    //The below code is modified by adding Richtextbox to solve defect no 0000081 and 0000090
                    if (item is TextBox || item is ComboBox || item is NumericUpDown || item is DateTimePicker || item is RichTextBox || item is RadioButton)
                        item.Enabled = false;
                }
                Reload_btn.Enabled = false;//Has been added to maintain the status of the reload button
            }
            catch (Exception ex)
            {
                //ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
            }
        }

        public void AutoComplete(ComboBox cb, System.Windows.Forms.KeyPressEventArgs e)
        {
            this.AutoComplete(cb, e, false);
        }

        /// <summary>
        /// This function will set the default values to all controls in the create patient window.
        /// </summary>
        public void setDefaultValues()
        {
            try
            {
                if (isCreatePat)
                {
                }
                enableConrols();
                dateOfBirth_dtPicker.Format = DateTimePickerFormat.Custom;
                dateOfBirth_dtPicker.CustomFormat = "dd/MM/yyyy";
                PatEditFirstName_tbx.Text = IVLVariables.LangResourceManager.GetString("defaultFirstName_Textbox_Text", IVLVariables.LangResourceCultureInfo);
                setDefaultTextboxFontNColor(PatEditFirstName_tbx, true);
                PatEditLastName_tbx.Text = IVLVariables.LangResourceManager.GetString("defaultLastName_Textbox_Text", IVLVariables.LangResourceCultureInfo);
                setDefaultTextboxFontNColor(PatEditLastName_tbx, true);
                PatEditIncome_cmbx.SelectedItem = INTUSOFT.Data.Common.GetDescription(SalaryEnum.No_Salary);
                PatEditLandLine_tbx.Text = IVLVariables.LangResourceManager.GetString("defaultLandLine_Textbox_Text", IVLVariables.LangResourceCultureInfo);
                setDefaultTextboxFontNColor(PatEditLandLine_tbx, true);
                PatEditStdCode_tbx.Text = IVLVariables.LangResourceManager.GetString("defaultstdCode_Textbox_Text", IVLVariables.LangResourceCultureInfo).ToString();
                setDefaultTextboxFontNColor(PatEditStdCode_tbx, true);
                PatEditMobile_tbx.Text = IVLVariables.LangResourceManager.GetString("defaultMobile_Textbox_Text", IVLVariables.LangResourceCultureInfo);
                setDefaultTextboxFontNColor(PatEditMobile_tbx, true);
                PatEditDoorStreet_Tbx.Text = IVLVariables.LangResourceManager.GetString("defaultAddress1_Textbox_Text", IVLVariables.LangResourceCultureInfo);
                setDefaultTextboxFontNColor(PatEditDoorStreet_Tbx, true);
                PatEditcomments_tbx.Text = IVLVariables.LangResourceManager.GetString("defaultComments_Textbox_Text", IVLVariables.LangResourceCultureInfo);
                setDefaultTextboxFontNColor(PatEditcomments_tbx, true);
                Occupation_combobx.SelectedItem = INTUSOFT.Data.Common.GetDescription(OccupationEnum.Occupation_Not_Selected);
                City_combobx.Text = INTUSOFT.Data.Common.GetDescription(CityEnum.City_Not_Selected);
                PatEditGender_cmbx.SelectedItem = INTUSOFT.Data.Common.GetDescription(GenderEnum.Not_Selected);
                State_combobx.Text = INTUSOFT.Data.Common.GetDescription(StateEnum.State_Not_Selected);
                Country_combobx.Text = INTUSOFT.Data.Common.GetDescription(CountryEnum.Country_Not_Selected);
                PatEditAddressArea_tbx.Text = IVLVariables.LangResourceManager.GetString("defaultAddress2_Textbox_Text", IVLVariables.LangResourceCultureInfo);
                setDefaultTextboxFontNColor(PatEditAddressArea_tbx, true);
                Email_tbx.Text = IVLVariables.LangResourceManager.GetString("defaultEmail_Textbox_Text", IVLVariables.LangResourceCultureInfo);
                setDefaultTextboxFontNColor(Email_tbx, true);
                otherOccupationDetails_tbx.Text = IVLVariables.LangResourceManager.GetString("DefaultOtherOccupation_Text", IVLVariables.LangResourceCultureInfo);
                setDefaultTextboxFontNColor(otherOccupationDetails_tbx, true);
                pincode_tbx.Text = IVLVariables.LangResourceManager.GetString("defaultPincode_Textbox_Text", IVLVariables.LangResourceCultureInfo);
                setDefaultTextboxFontNColor(pincode_tbx, true);
                referredBy_tbx.Text = IVLVariables.LangResourceManager.GetString("defaultReferredBy_Textbox_Text", IVLVariables.LangResourceCultureInfo);
                setDefaultTextboxFontNColor(referredBy_tbx, true);
                healthStatus_tbx.Text = IVLVariables.LangResourceManager.GetString("defaultHealthStatus_Textbox_Text", IVLVariables.LangResourceCultureInfo);
                setDefaultTextboxFontNColor(healthStatus_tbx, true);
                age_nud.Value = 2;
                age_nud.ForeColor = Color.Gray;
                //dateOfBirth_dtPicker.Value = DateTime.Now.Date;
                weight_nud.Value = Convert.ToDecimal(1);
                weight_nud.ForeColor = Color.Gray;
                height_nud.Value = (decimal)1.0;
                height_nud.ForeColor = Color.Gray;
                if (Convert.ToBoolean(IVLVariables.CurrentSettings.UserSettings._IsAgeSelect.val))
                    Age_radio.Checked = Convert.ToBoolean(IVLVariables.CurrentSettings.UserSettings._IsAgeSelect.val);
                else
                    Dob_radio.Checked = !(Convert.ToBoolean(IVLVariables.CurrentSettings.UserSettings._IsAgeSelect.val));
                Reload_btn.Enabled = false;//Has been added to maintain the status of the reload button
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
//                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        /// <summary>
        /// This function is used to enable all the controls in create patient window if they are disabled.
        /// </summary>
        public void enableConrols()
        {
            try
            {
                List<Control> c = GetControls(this).ToList();
                foreach (var item in c)
                {
                    //The below code is modified by adding Richtextbox to solve defect no 0000081 and 0000090
                    if (!Convert.ToBoolean(IVLVariables.CurrentSettings.UserSettings._IsMrnConfigurable.val) && !isCreatePat)
                    {
                        if ((item is TextBox && item.Name != "PatEditMrn_tbx") || item is RadioButton || item is ComboBox || item is NumericUpDown || item is DateTimePicker || item is RichTextBox)
                            item.Enabled = true;
                    }
                    //This below else if statements has been added by Darshan ion 19-08-2015 to solve Defect no 0000503: Modification made in the IVL config file creating Same entries to be saved multiple times.
                    else if (Convert.ToBoolean(IVLVariables.CurrentSettings.UserSettings._IsMrnConfigurable.val) && !isCreatePat)
                    {
                        if ((item is TextBox && item.Name != "PatEditMrn_tbx") || item is RadioButton || item is ComboBox || item is NumericUpDown || item is DateTimePicker || item is RichTextBox)
                            item.Enabled = true;
                    }
                    else if (!Convert.ToBoolean(IVLVariables.CurrentSettings.UserSettings._IsMrnConfigurable.val) && isCreatePat)
                    {
                        if ((item is TextBox && item.Name != "PatEditMrn_tbx") || item is RadioButton || item is ComboBox || item is NumericUpDown || item is DateTimePicker || item is RichTextBox)
                            item.Enabled = true;
                    }
                    else
                    {
                        if ((item is TextBox) || item is ComboBox || item is NumericUpDown || item is DateTimePicker || item is RichTextBox || item is RadioButton)
                            item.Enabled = true;
                    }

                    if (item is TextBox)
                        setDefaultTextboxFontNColor(item as TextBox, false);
                    else if (item is RichTextBox)
                        setDefaultTextboxFontNColor(item as RichTextBox, false);
                    Reload_btn.Enabled = false;//Has been added to maintain the status of the reload button
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
//                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        /// <summary>
        /// This function is used to pop up warning message.
        /// </summary>
        /// <param name="alertMsgs">alertMsgs will contain the header and message body of the pop up warning message</param>
        /// <returns></returns>
        public bool AlertModificationExit(Args alertMsgs)
        {
            DialogResult result = CustomMessageBox.Show(alertMsgs["alert_msg"] as string, alertMsgs["alert_header"] as string, CustomMessageBoxButtons.YesNo,CustomMessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //The below code has been added in order solve defect no 0000072
        /// <summary>
        /// This function is used to cancel the operation and exit from the create patient window.
        /// </summary>
        public void Cancel_Modification()
        {
            try
            {
                Args arg = new Args();
                arg["alert_msg"] = IVLVariables.LangResourceManager.GetString("Cancelconfirmation_Text", IVLVariables.LangResourceCultureInfo);
                arg["alert_header"] = IVLVariables.LangResourceManager.GetString("Cancel_Button_Text", IVLVariables.LangResourceCultureInfo);
                if (AlertModificationExit(arg))
                {
                    if (isCreatePat)
                    {
                        isCreatePat = false;
                        //setDefaultValues();
                    }
                    else
                    {
                        //setPatDetails(currentPat);
                    }
                    this.ParentForm.Close();
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
//                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        public void AutoComplete(ComboBox cb, System.Windows.Forms.KeyPressEventArgs e, bool blnLimitToList)
        {
            string strFindStr = "";
            if (e.KeyChar == (char)8)
            {
                if (cb.SelectionStart <= 1)
                {
                    cb.Text = "";
                    return;
                }
                if (cb.SelectionLength == 0)
                    strFindStr = cb.Text.Substring(0, cb.Text.Length - 1);
                else
                    strFindStr = cb.Text.Substring(0, cb.SelectionStart - 1);
            }
            else
            {
                if (cb.SelectionLength == 0)
                    strFindStr = cb.Text + e.KeyChar;
                else
                    strFindStr = cb.Text.Substring(0, cb.SelectionStart) + e.KeyChar;
            }
            int intIdx = -1;
            // Search the string in the ComboBox list.
            intIdx = cb.FindString(strFindStr);
            if (intIdx != -1)
            {
                cb.SelectedText = "";
                cb.SelectedIndex = intIdx;
                cb.SelectionStart = strFindStr.Length;
                cb.SelectionLength = cb.Text.Length;
                e.Handled = true;
            }
            else
            {
                e.Handled = blnLimitToList;
            }
        }
        #endregion
        #region Private methods

        /// <summary>
        /// This function will reload the patient details in create patient window when they are cleared.
        /// </summary>
        private void ReloadDetails()
        {
            try
            {
                if (isCreatePat)
                {
                    setDefaultValues();
                    PatEditFirstName_tbx.Clear();
                    setDefaultTextboxFontNColor(PatEditFirstName_tbx, false);
                    isFieldValueChanged = false;
                }
                else
                {
                    setPatDetails(currentPat);
                    enableConrols();
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
//                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        /// <summary>
        /// This function is used to save details of all the controls in create patient window into a local variable of type INTUSOFT.Data.Model.Patient.
        /// </summary>
        /// <returns>returns true if details is saved completely else retuen false.</returns>
        private bool populatePatDetails()
        {
            //This code has been added to insert user id into the table has to be removed once user or admin has been added and has be replaced by active user.
            users user = users.CreateNewUsers();
            user.userId = 1;
            List<int> personAttributeType = new List<int>();
            if (isCreatePat)
            {
                currentPat = null;
            }
            else
            {
            }
            if (currentPat == null)
            {
                currentPat = Patient.CreateNewPatient();
                currentPat.createdBy = user;
                currentPat.patientCreatedBy = user;
                currentPersonAddress = PersonAddressModel.CreateNewPersonAddress();
                currentPat.attributes = new HashSet<person_attribute>();
                currentIdentifier = patient_identifier.CreateNewPatientIdentifier();
                currentPat.identifiers = new HashSet<patient_identifier>();
                currentPersonAttribute = new List<person_attribute>();
                currentPat.addresses = new HashSet<PersonAddressModel>();
                currentPat.visits = new HashSet<visit>();
                currentPat.diagnosis = new HashSet<PatientDiagnosis>();
                attributeType = person_attribute_type.CreateNewPersonAttributeType();
                personAttribute = person_attribute.CreateNewPersonAttribute();
            }
            if (Convert.ToBoolean(IVLVariables.CurrentSettings.UserSettings._IsMrnConfigurable.val) && isCreatePat)
            {
                if (NewDataVariables.Identifier != null)
                {
                    if (NewDataVariables.Identifier.Where(x => x.value == PatEditMrn_tbx.Text).ToList().Count != 0)
                    {
                        CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("Uniquemrn_text", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("Mrn_Label_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Warning);
                        isValid = false;
                        return false;
                    }
                }
            }
            if (Convert.ToBoolean(IVLVariables.CurrentSettings.UserSettings._IsMrnConfigurable.val) && isCreatePat)
            {
                if (!string.IsNullOrEmpty(PatEditMrn_tbx.Text) && !PatEditMrn_tbx.Text.StartsWith(" "))
                {
                    currentIdentifier.value = PatEditMrn_tbx.Text;
                    if (isCreatePat)
                        currentIdentifier.createdBy = user;
                    isValid = true;
                }
                else
                {
                    CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("Registration_MRN_IS_Required", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("Mrn_Label_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Warning);
                    isValid = false;
                    PatEditMrn_tbx.Focus();
                    return false;
                }
            }
            //This below code has been modified by darshan to solve defect no 0000504: IVL config modified:Entry can be made without MRN,DOB/Age not displayed.
            else if (!string.IsNullOrEmpty(PatEditMrn_tbx.Text) && !PatEditMrn_tbx.Text.StartsWith(" "))
            {
                currentIdentifier.value = PatEditMrn_tbx.Text;
                currentIdentifier.createdDate = DateTime.Now;
                if (isCreatePat)
                    currentIdentifier.createdBy = user;
                isValid = true;
            }
            else
            {
                CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("Registration_MRN_IS_Required", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("Mrn_Label_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Warning);
                isValid = false;
                PatEditMrn_tbx.Focus();
                return false;
            }
            //This line has extented code in order to fix Defect no 0000075
            isempty = string.IsNullOrEmpty(PatEditFirstName_tbx.Text) || string.IsNullOrWhiteSpace(PatEditFirstName_tbx.Text) || PatEditFirstName_tbx.Text.StartsWith(" ") || PatEditFirstName_tbx.Text.Equals("FIRST NAME");
            if (!string.IsNullOrEmpty(PatEditFirstName_tbx.Text) && !string.IsNullOrWhiteSpace(PatEditFirstName_tbx.Text) && !PatEditFirstName_tbx.Text.StartsWith(" ") && !fname.ToLower().Equals(PatEditFirstName_tbx.Text.ToLower()) && !PatEditFirstName_tbx.Text.Any(c => char.IsDigit(c)) && !PatEditFirstName_tbx.Text.Any(c => char.IsControl(c)) && !PatEditFirstName_tbx.Text.Any(c => char.IsSymbol(c)) && !PatEditFirstName_tbx.Text.Any(c => char.IsPunctuation(c)))
            {
                currentPat.firstName = PatEditFirstName_tbx.Text;
                isValid = true;
            }
            else
            {
                if (PatEditFirstName_tbx.Text.StartsWith(" "))
                    CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("FirstNameSpace_Text", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("FirstName_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Warning);
                else
                    if (PatEditFirstName_tbx.Text.Any(c => char.IsControl(c)) || PatEditFirstName_tbx.Text.Any(c => char.IsSymbol(c)) || PatEditFirstName_tbx.Text.Any(c => char.IsPunctuation(c)))
                        CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("Valid_First_Name", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("FirstName_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Warning);
                    else
                        CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("Registration_FirstName_Required_Text"/*Defect No: 0001751 Registration_ FirstName_Required_Text text has been modified to Registration_FirstName_Required_Text*/, IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("FirstName_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Warning);
                isValid = false;
                PatEditFirstName_tbx.Text = string.Empty;
                PatEditFirstName_tbx.Focus();
                setDefaultTextboxFontNColor(PatEditFirstName_tbx, false);//This method is getting invoked to change the font style of the first name after the first name required message comes and go.
                return false;
            }
            //This line has extented code in order to fix Defect no 0000075
            isempty = string.IsNullOrEmpty(PatEditLastName_tbx.Text) || string.IsNullOrWhiteSpace(PatEditLastName_tbx.Text) || PatEditLastName_tbx.Text.StartsWith(" ");
            if (!string.IsNullOrEmpty(PatEditLastName_tbx.Text) && !string.IsNullOrWhiteSpace(PatEditLastName_tbx.Text) && !PatEditLastName_tbx.Text.StartsWith(" ") && !lname.ToLower().Equals(PatEditLastName_tbx.Text.ToLower()) && !PatEditLastName_tbx.Text.Any(c => char.IsDigit(c)) && !PatEditLastName_tbx.Text.Any(c => char.IsControl(c)) && !PatEditLastName_tbx.Text.Any(c => char.IsSymbol(c)) && !PatEditLastName_tbx.Text.Any(c => char.IsPunctuation(c)))
            {
                currentPat.lastName = PatEditLastName_tbx.Text;
                isValid = true;
            }
            else
            {
                if (PatEditLastName_tbx.Text.StartsWith(" "))
                    CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("LastName_space_Text", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("LastName_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Warning);
                else
                    if (PatEditLastName_tbx.Text.Any(c => char.IsControl(c)) || PatEditLastName_tbx.Text.Any(c => char.IsSymbol(c)) || PatEditLastName_tbx.Text.Any(c => char.IsPunctuation(c)))
                        CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("Valid_Last_Name", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("LastName_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Warning);
                    else
                        CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("Registration_LastName_is_Required_Text", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("LastName_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Warning);
                isValid = false;
                PatEditLastName_tbx.Text = string.Empty;
                PatEditLastName_tbx.Focus();
                setDefaultTextboxFontNColor(PatEditLastName_tbx, false);
                return false;
            }
            isempty = PatEditGender_cmbx.SelectedItem.Equals(INTUSOFT.Data.Common.GetDescription(GenderEnum.Not_Selected));
            if (!PatEditGender_cmbx.SelectedItem.Equals(INTUSOFT.Data.Common.GetDescription(GenderEnum.Not_Selected)))
            {
                if (PatEditGender_cmbx.Text.Equals("Male"))
                    currentPat.gender = 'M';
                else
                    currentPat.gender = 'F';
                isValid = true;
            }
            else
            {
                CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("Registration_MaritalStatus_Select_Text", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("Gender_Label_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Warning);
                isValid = false;
                return false;
            }
            string val = ((age_nud).Text);
            if ((age_nud.Value <= 2 || age_nud.Value >= 100 || string.IsNullOrEmpty(val)))
            {
                CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("Valid_Age", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("Age_Label_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Warning);
                isValid = false;
                age_nud.Focus();
                return false;
            }
            else
                currentPat.birthdate = (dateOfBirth_dtPicker.Value);
                currentPat.birthdateEstimated = false;
                if (!string.IsNullOrEmpty(PatEditStdCode_tbx.Text) && !PatEditStdCode_tbx.Text.Any(c => char.IsLetter(c)) && !PatEditStdCode_tbx.Text.Any(c => char.IsPunctuation(c)) && !PatEditStdCode_tbx.Text.Any(c => char.IsSymbol(c)) && !(PatEditStdCode_tbx.Text == IVLVariables.LangResourceManager.GetString("defaultstdCode_Textbox_Text", IVLVariables.LangResourceCultureInfo)))
                    currentPersonAddress.Land_Code = PatEditStdCode_tbx.Text;
                else
                {
                    currentPersonAddress.Land_Code = null;
                }
            //This below address code has been modified to solve defect no 512 on 03-08-2015
            if (PatEditDoorStreet_Tbx.Text == IVLVariables.LangResourceManager.GetString("defaultAddress1_Textbox_Text", IVLVariables.LangResourceCultureInfo) || PatEditAddressArea_tbx.Text == IVLVariables.LangResourceManager.GetString("defaultAddress2_Textbox_Text", IVLVariables.LangResourceCultureInfo))
            {
                if (PatEditDoorStreet_Tbx.Text.Equals(IVLVariables.LangResourceManager.GetString("defaultAddress1_Textbox_Text", IVLVariables.LangResourceCultureInfo)) && !PatEditAddressArea_tbx.Text.Equals(IVLVariables.LangResourceManager.GetString("defaultAddress2_Textbox_Text", IVLVariables.LangResourceCultureInfo)))
                {
                    currentPersonAddress.line1 = "";
                }
                else
                    if (PatEditAddressArea_tbx.Text.Equals(IVLVariables.LangResourceManager.GetString("defaultAddress2_Textbox_Text", IVLVariables.LangResourceCultureInfo)) && !PatEditDoorStreet_Tbx.Text.Equals(IVLVariables.LangResourceManager.GetString("defaultAddress1_Textbox_Text", IVLVariables.LangResourceCultureInfo)))
                    {
                        currentPersonAddress.line2 = "";
                    }
                    else if (PatEditDoorStreet_Tbx.Text.Equals(IVLVariables.LangResourceManager.GetString("defaultAddress1_Textbox_Text", IVLVariables.LangResourceCultureInfo)) && PatEditAddressArea_tbx.Text.Equals(IVLVariables.LangResourceManager.GetString("defaultAddress2_Textbox_Text", IVLVariables.LangResourceCultureInfo)))
                    {
                        //currentPersonAddress.line1 = IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo);
                        currentPersonAddress.line1 = null;
                    }
            }
            else if (string.IsNullOrEmpty(PatEditDoorStreet_Tbx.Text) || string.IsNullOrEmpty(PatEditAddressArea_tbx.Text) || PatEditDoorStreet_Tbx.Text.Equals(IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo)))
            {
                if ((string.IsNullOrEmpty(PatEditDoorStreet_Tbx.Text) || PatEditDoorStreet_Tbx.Text.Equals(IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo))) && !string.IsNullOrEmpty(PatEditAddressArea_tbx.Text))
                {
                    if (PatEditDoorStreet_Tbx.Text.Equals(IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo)))
                    {
                        currentPersonAddress.line1 = null;
                        currentPersonAddress.line2 = null;
                    }
                    else
                    {
                        currentPersonAddress.line1 = "";
                        currentPersonAddress.line2 = PatEditAddressArea_tbx.Text;
                    }
                }
                else
                   if (string.IsNullOrEmpty(PatEditAddressArea_tbx.Text) && !string.IsNullOrEmpty(PatEditDoorStreet_Tbx.Text))
                   {
                       if (PatEditDoorStreet_Tbx.Text.Equals(IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo)))
                       {
                           currentPersonAddress.line1 = null;
                           currentPersonAddress.line2 = null;
                       }
                       else
                       {
                           currentPersonAddress.line1 = PatEditDoorStreet_Tbx.Text;
                           currentPersonAddress.line2 = "";
                       }
                   }
                    else if (string.IsNullOrEmpty(PatEditAddressArea_tbx.Text) && string.IsNullOrEmpty(PatEditDoorStreet_Tbx.Text))
                    {
                        //currentPersonAddress.line1 = IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo);
                        currentPersonAddress.line1 = null;
                        currentPersonAddress.line2 = "";
                    }
            }
            else
            {
                currentPersonAddress.line1 = PatEditDoorStreet_Tbx.Text;
                currentPersonAddress.line2 = PatEditAddressArea_tbx.Text;
            }
            {
                string mobile;
                if (!string.IsNullOrEmpty(PatEditMobile_tbx.Text) && !PatEditMobile_tbx.Text.Any(c => char.IsLetter(c)) && !PatEditMobile_tbx.Text.Any(c => char.IsPunctuation(c)) && !PatEditMobile_tbx.Text.Any(c => char.IsSymbol(c)) && !(PatEditMobile_tbx.Text == IVLVariables.LangResourceManager.GetString("defaultMobile_Textbox_Text", IVLVariables.LangResourceCultureInfo)))
                    mobile = (PatEditMobile_tbx.Text);
                else
                    mobile = null;
                if (isCreatePat)
                {
                    
                    currentPat.primaryPhoneNumber = mobile;
                }
                else
                {
                    if (PatEditMobile_tbx.Text.Equals("0"))
                        currentPat.primaryPhoneNumber = null;
                    else
                        currentPat.primaryPhoneNumber = mobile;
                }
            }
            {
                string Email;
                if (string.IsNullOrEmpty((Email_tbx.Text)) || Email_tbx.Text == IVLVariables.LangResourceManager.GetString("defaultEmail_Textbox_Text", IVLVariables.LangResourceCultureInfo))
                {
                    Email_tbx.CharacterCasing = CharacterCasing.Normal;
                    //Email = IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo);
                    Email = null;
                }
                else
                {
                    Email = Email_tbx.Text;
                }
                if (isCreatePat)
                {
                    currentPat.primaryEmailId = Email;
                }
                else
                {
                    if (Email_tbx.Text.Equals(IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo)))
                    currentPat.primaryEmailId = null;
                    else
                        currentPat.primaryEmailId= Email;
                }
            }
            {
                if (isCreatePat)
                {
                    personAttribute = person_attribute.CreateNewPersonAttribute();
                    attributeType = person_attribute_type.CreateNewPersonAttributeType();
                    attributeType.personAttributeTypeId = 2;
                    if (!PatEditIncome_cmbx.SelectedItem.Equals(INTUSOFT.Data.Common.GetDescription(SalaryEnum.No_Salary)))
                        personAttribute.value = PatEditIncome_cmbx.SelectedItem.ToString();
                    else
                        personAttribute.value = null;
                    personAttribute.attributeType = attributeType;
                    currentPersonAttribute.Add(personAttribute);
                }
                else
                {
                    if (!PatEditIncome_cmbx.SelectedItem.Equals(INTUSOFT.Data.Common.GetDescription(SalaryEnum.No_Salary)))
                    currentPersonAttribute.Where(w => w.attributeType.personAttributeTypeId == 2).ToList().ForEach(s => s.value = PatEditIncome_cmbx.SelectedItem.ToString());
                 else
                        currentPersonAttribute.Where(w => w.attributeType.personAttributeTypeId == 2).ToList().ForEach(s => s.value = null);
                }
            }
            {
                if (isCreatePat)
                {
                    personAttribute = person_attribute.CreateNewPersonAttribute();
                    attributeType = person_attribute_type.CreateNewPersonAttributeType();
                    attributeType.personAttributeTypeId = 3;
                    if (!Occupation_combobx.SelectedItem.Equals(INTUSOFT.Data.Common.GetDescription(OccupationEnum.Occupation_Not_Selected)))
                    {    //the code has been changed to specify the other occupation details
                        if (Occupation_combobx.SelectedIndex == Occupation_description_values.Length - 1)
                            //"otherOccupationDetails_tbx" is the textbox to specify the other occupation details
                            //if others is selected then text in the otherOccupationDetails_tbx will be saved in DB
                            personAttribute.value = otherOccupationDetails_tbx.Text;
                        else
                            personAttribute.value = Occupation_combobx.SelectedItem.ToString();

                           
                    }
                    else
                    personAttribute.value = null;

                    personAttribute.attributeType = attributeType;
                    currentPersonAttribute.Add(personAttribute);
                }
                else
                {
                    if (!Occupation_combobx.SelectedItem.Equals(INTUSOFT.Data.Common.GetDescription(OccupationEnum.Occupation_Not_Selected)))
                    {   //the code been changed to check the other specified occupation which is not in occupation list
                        if (Occupation_combobx.SelectedIndex == Occupation_description_values.Length - 1)

                            currentPersonAttribute.Where(w => w.attributeType.personAttributeTypeId == 3).ToList().ForEach(s => s.value = otherOccupationDetails_tbx.Text );
                        
                            else
                        currentPersonAttribute.Where(w => w.attributeType.personAttributeTypeId == 3).ToList().ForEach(s => s.value = Occupation_combobx.SelectedItem.ToString());

                    }
                    else
                        currentPersonAttribute.Where(w => w.attributeType.personAttributeTypeId == 3).ToList().ForEach(s => s.value = null);
                }
            }
            {
                if (isCreatePat)
                {
                    personAttribute = person_attribute.CreateNewPersonAttribute();
                    attributeType = person_attribute_type.CreateNewPersonAttributeType();
                    attributeType.personAttributeTypeId = 5;
                    if (height_nud.Value!=(Convert.ToDecimal(1)))
                    personAttribute.value = height_nud.Value.ToString();
                    else
                    personAttribute.value = null;
                    personAttribute.attributeType = attributeType;
                    currentPersonAttribute.Add(personAttribute);
                }
                else
                
                    if (height_nud.Value!=(Convert.ToDecimal(1)))// && !string.IsNullOrEmpty(height_nud.Value.ToString()))
                        currentPersonAttribute.Where(w => w.attributeType.personAttributeTypeId == 5).ToList().ForEach(s => s.value = height_nud.Value.ToString());
                    else
                        currentPersonAttribute.Where(w => w.attributeType.personAttributeTypeId == 5).ToList().ForEach(s => s.value = null);
            }
            {
                if (isCreatePat)
                {
                    personAttribute = person_attribute.CreateNewPersonAttribute();
                    attributeType = person_attribute_type.CreateNewPersonAttributeType();
                    attributeType.personAttributeTypeId = 6;
                    if (weight_nud.Value != (Convert.ToDecimal(1)))
                        personAttribute.value = weight_nud.Value.ToString();
                    else
                        personAttribute.value = null;
                    personAttribute.attributeType = attributeType;
                    currentPersonAttribute.Add(personAttribute);
                }
                else
                {
                    if (weight_nud.Value != (Convert.ToDecimal(1)))
                        currentPersonAttribute.Where(w => w.attributeType.personAttributeTypeId == 6).ToList().ForEach(s => s.value = weight_nud.Value.ToString());
                    else
                        currentPersonAttribute.Where(w => w.attributeType.personAttributeTypeId == 6).ToList().ForEach(s => s.value = null);
                }
            }
            if ((Enum.GetNames(typeof(CityEnum)).Contains(City_combobx.Text)))
            {
                currentPersonAddress.cityVillage = City_combobx.Text;
            }
            else
            {
                //currentPersonAddress.cityVillage = INTUSOFT.Data.Common.GetDescription(CityEnum.City_Not_Selected);
                currentPersonAddress.cityVillage = null;
            }
            currentPersonAddress.person = currentPat;
            currentIdentifier.patient = currentPat;
            patient_identifier_type identifier = patient_identifier_type.CreateNewPatientIdentifierType();
            identifier.patientIdentifierTypeId = 1;
            if(isCreatePat)
                currentIdentifier.type = identifier;
            if (!healthStatus_tbx.Text.Any(c => char.IsDigit(c)) && !string.IsNullOrEmpty((healthStatus_tbx.Text)) && !(healthStatus_tbx.Text == IVLVariables.LangResourceManager.GetString("defaultHealthStatus_Textbox_Text", IVLVariables.LangResourceCultureInfo)) && !healthStatus_tbx.Text.Equals(IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo)))
                currentPat.historyAilments = healthStatus_tbx.Text;
            else
            {
                //currentPat.historyAilments = IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo);
                currentPat.historyAilments = null;
            }
            {
                string comments;
                if (!string.IsNullOrEmpty((PatEditcomments_tbx.Text)) && !(PatEditcomments_tbx.Text == IVLVariables.LangResourceManager.GetString("defaultComments_Textbox_Text", IVLVariables.LangResourceCultureInfo)))
                    comments = PatEditcomments_tbx.Text;
                else
                {
                    //comments = IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo);
                    comments = null;
                }
                    if (isCreatePat)
                {
                    personAttribute = person_attribute.CreateNewPersonAttribute();
                    attributeType = person_attribute_type.CreateNewPersonAttributeType();
                    attributeType.personAttributeTypeId = 7;
                    if (!PatEditcomments_tbx.Text.Equals(IVLVariables.LangResourceManager.GetString("defaultComments_Textbox_Text", IVLVariables.LangResourceCultureInfo)))
                    {
                        personAttribute.value = comments;
                    }
                    else
                    {
                        personAttribute.value = null;
                    }

                    personAttribute.attributeType = attributeType;
                    currentPersonAttribute.Add(personAttribute);
                }
                else
                {
                    if (!PatEditcomments_tbx.Text.Equals(IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo)))
                        currentPersonAttribute.Where(w => w.attributeType.personAttributeTypeId == 7).ToList().ForEach(s => s.value = comments);
                    else
                        currentPersonAttribute.Where(w => w.attributeType.personAttributeTypeId == 7).ToList().ForEach(s => s.value = null);
                }
            }
            if (!string.IsNullOrEmpty(pincode_tbx.Text) && !pincode_tbx.Text.Any(c => char.IsLetter(c)) && !pincode_tbx.Text.Any(c => char.IsPunctuation(c)) && !pincode_tbx.Text.Any(c => char.IsSymbol(c)) && !(pincode_tbx.Text == IVLVariables.LangResourceManager.GetString("defaultPincode_Textbox_Text", IVLVariables.LangResourceCultureInfo)))
                currentPersonAddress.postalCode = (pincode_tbx.Text);
            else
            {
                currentPersonAddress.postalCode = null;
            }
            if ((Enum.GetNames(typeof(StateEnum)).Contains(State_combobx.Text)))
            {
                currentPersonAddress.stateProvince = State_combobx.Text;
            }
            else
            {
                //currentPersonAddress.stateProvince = INTUSOFT.Data.Common.GetDescription(StateEnum.State_Not_Selected);
                currentPersonAddress.stateProvince = null;
            }
            if (isCreatePat)
                currentPersonAddress.createdBy = user;
            if ((Enum.GetNames(typeof(CountryEnum)).Contains(Country_combobx.Text)))
                currentPersonAddress.country = Country_combobx.Text;
            else
            {
                //currentPersonAddress.country = INTUSOFT.Data.Common.GetDescription(CountryEnum.Country_Not_Selected);
                currentPersonAddress.country = null;
            }
            {
                string landLine;
                if (!string.IsNullOrEmpty((PatEditLandLine_tbx.Text)) && !PatEditLandLine_tbx.Text.Any(c => char.IsLetter(c)) && !PatEditLandLine_tbx.Text.Any(c => char.IsPunctuation(c)) && !PatEditLandLine_tbx.Text.Any(c => char.IsSymbol(c)) && !(PatEditLandLine_tbx.Text == IVLVariables.LangResourceManager.GetString("defaultLandLine_Textbox_Text", IVLVariables.LangResourceCultureInfo)))
                    landLine = (PatEditLandLine_tbx.Text);
                else
                {
                    landLine = null;
                } if (isCreatePat)
                {
                    personAttribute = person_attribute.CreateNewPersonAttribute();
                    attributeType = person_attribute_type.CreateNewPersonAttributeType();
                    attributeType.personAttributeTypeId = 4;
                    //if(currentPersonAddress.Land_Code==string.Empty)
                    if (currentPersonAddress.Land_Code == null && landLine == null)
                        personAttribute.value = null;
                    else
                        personAttribute.value = currentPersonAddress.Land_Code + " " + landLine;
                    personAttribute.attributeType = attributeType;
                    currentPersonAttribute.Add(personAttribute);
                }
                else
                {
                    if (PatEditLandLine_tbx.Text == "0" && currentPersonAddress.Land_Code == "0")
                    currentPersonAttribute.Where(w => w.attributeType.personAttributeTypeId == 4).ToList().ForEach(s => s.value = null);
                    else
                        currentPersonAttribute.Where(w => w.attributeType.personAttributeTypeId == 4).ToList().ForEach(s => s.value = currentPersonAddress.Land_Code + " " + landLine);
                }
            }
            //Old Implementation of the Reffered By.
            //Person refferedBy = NewDataVariables._personRepo.GetByName(referredBy_tbx.Text);
            //// This line has been added to fix the defect number 0000074 by sriram on march 23rd 2015
            //if (!referredBy_tbx.Text.Any(c => char.IsDigit(c)) && !string.IsNullOrEmpty((referredBy_tbx.Text)) && !(referredBy_tbx.Text == IVLVariables.LangResourceManager.GetString("defaultReferredBy_Textbox_Text", IVLVariables.LangResourceCultureInfo)))
            //{
            //    refferedBy = NewDataVariables._personRepo.GetByName(referredBy_tbx.Text);
            //    //currentPat.referred_by = referredBy_tbx.Text;
            //}
            //else
            //{
            //    //currentPat.referred_by = IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo);
            //    refferedBy = null;
            //}
            if (!referredBy_tbx.Text.Any(c => char.IsDigit(c)) && !(referredBy_tbx.Text == IVLVariables.LangResourceManager.GetString("defaultReferredBy_Textbox_Text", IVLVariables.LangResourceCultureInfo)))
            {
                if (currentPat.referredBy == null && !referredBy_tbx.Text.Equals(IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo)) && !string.IsNullOrEmpty((referredBy_tbx.Text)))
                {
                    Person referredBy = Person.CreateNewPerson();
                    referredBy.firstName = referredBy_tbx.Text;
                    referredBy.createdBy = user;
                    referredBy.createdDate = DateTime.Now;
                    NewDataVariables._Repo.Add<Person>(referredBy);
                    currentPat.referredBy = referredBy;
                }
                else if(currentPat.referredBy != null && !referredBy_tbx.Text.Equals(IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo)))
                {
                    if (!string.IsNullOrEmpty((referredBy_tbx.Text)))
                    {
                        currentPat.referredBy.firstName = referredBy_tbx.Text;
                    }
                    else
                    {
                        currentPat.referredBy.firstName = IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo);
                    }
                    currentPat.referredBy.lastModifiedDate = DateTime.Now;
                    NewDataVariables._Repo.Update<Person>(currentPat.referredBy);
                }
            }
            //ModifiedDateTime and RegistrationDateTime has been Added by Darshan on 31-07-2015 to support advance searching.
            if (isCreatePat)
            {
                currentPat.date_accessed = DateTime.Now;
                currentPat.createdDate = DateTime.Now;
                currentPersonAddress.createdDate = DateTime.Now;
                currentPersonAddress.date_accessed = DateTime.Now;
                currentIdentifier.createdDate = DateTime.Now;
            }
            currentPat.patientLastModifiedDate = DateTime.Now;
            currentPersonAddress.lastModifiedDate = DateTime.Now;
            currentIdentifier.lastModifiedDate = DateTime.Now;
            return true;
        }

        /// <summary>
        /// This function will change the color of the TextBox based on isDefault value.
        /// </summary>
        /// <param name="t">TextBox name</param>
        /// <param name="isDefault">isDefault true for new patients and false for existing patients</param>
        private void setDefaultTextboxFontNColor(TextBox t, bool isDefault)
        {
            if (isDefault)
            {
                t.ForeColor = Color.Gray;
                t.Font = new Font(t.Font, FontStyle.Italic);
            }
            else
            {
                t.ForeColor = Color.Black;
                t.Font = new Font(t.Font, FontStyle.Regular);
            }
        }

        /// <summary>
        /// This function will change the color of the RichTextBox based on isDefault value.
        /// </summary>
        /// <param name="t">RichTextBox name</param>
        /// <param name="isDefault">isDefault true for new patients and false for existing patients</param>
        private void setDefaultTextboxFontNColor(RichTextBox t, bool isDefault)
        {
            if (isDefault)
            {
                t.ForeColor = Color.Gray;
                t.Font = new Font(t.Font, FontStyle.Italic);
            }
            else
            {
                t.ForeColor = Color.Black;
                t.Font = new Font(t.Font, FontStyle.Regular);
            }
        }

        /// <summary>
        /// This function will initializes the names for all controls present in create patient window.
        /// </summary>
        private void InitializeResourceString()
        {
            #region labels initilize string from resources
            mrn_lbl.Text = IVLVariables.LangResourceManager.GetString( "Mrn_Label_Text",IVLVariables.LangResourceCultureInfo);
            firstName_lbl.Text = IVLVariables.LangResourceManager.GetString( "FirstName_Label_Text",IVLVariables.LangResourceCultureInfo);
            lastName_lbl.Text = IVLVariables.LangResourceManager.GetString( "LastName_Label_Text",IVLVariables.LangResourceCultureInfo);
            gender_lbl.Text = IVLVariables.LangResourceManager.GetString("PatientDetailsGender_Label_Text", IVLVariables.LangResourceCultureInfo);
            age_lbl.Text = IVLVariables.LangResourceManager.GetString( "Age_Label_Text",IVLVariables.LangResourceCultureInfo);
            occupation_lbl.Text = IVLVariables.LangResourceManager.GetString( "Registration_Occupation_Label_Text",IVLVariables.LangResourceCultureInfo);
            income_lbl.Text = IVLVariables.LangResourceManager.GetString( "Registration_Salary_Label_Text",IVLVariables.LangResourceCultureInfo);
            email_lbl.Text = IVLVariables.LangResourceManager.GetString( "Email_Label_Text",IVLVariables.LangResourceCultureInfo);
            landline_lbl.Text = IVLVariables.LangResourceManager.GetString( "Landline_Label_Text",IVLVariables.LangResourceCultureInfo);
            mobile_lbl.Text = IVLVariables.LangResourceManager.GetString( "Mobile_Lable_Text",IVLVariables.LangResourceCultureInfo);
            referredBy_lbl.Text = IVLVariables.LangResourceManager.GetString( "ReferredBy_Label_Text",IVLVariables.LangResourceCultureInfo);
            comments_lbl.Text = IVLVariables.LangResourceManager.GetString( "Comments_Label_Text",IVLVariables.LangResourceCultureInfo);
            healthStatus_lbl.Text = IVLVariables.LangResourceManager.GetString( "HealthStatus_Label_Text",IVLVariables.LangResourceCultureInfo);
            city_lbl.Text = IVLVariables.LangResourceManager.GetString( "City_Label_Text",IVLVariables.LangResourceCultureInfo);
            state_lbl.Text = IVLVariables.LangResourceManager.GetString( "State_Lable_Text",IVLVariables.LangResourceCultureInfo);
            country_lbl.Text = IVLVariables.LangResourceManager.GetString( "Country_Label_Text",IVLVariables.LangResourceCultureInfo);
            height_lbl.Text = IVLVariables.LangResourceManager.GetString( "Height_Label_Text",IVLVariables.LangResourceCultureInfo);
            weight_lbl.Text = IVLVariables.LangResourceManager.GetString( "Weight_Label_Text",IVLVariables.LangResourceCultureInfo);
            address1_lbl.Text = IVLVariables.LangResourceManager.GetString( "Address1_Label_Text",IVLVariables.LangResourceCultureInfo);
            address2_lbl.Text = IVLVariables.LangResourceManager.GetString( "Address2_Label_Text",IVLVariables.LangResourceCultureInfo);
            Maxcomments_lbl.Text = "( " + IVLVariables.LangResourceManager.GetString("MaximumText", IVLVariables.LangResourceCultureInfo) + " " + PatEditcomments_tbx.MaxLength.ToString() + " " + IVLVariables.LangResourceManager.GetString("CharactersText", IVLVariables.LangResourceCultureInfo) + " )";
            MaxHealthstatus_lbl.Text = IVLVariables.LangResourceManager.GetString( "MaxHealthStatus_Text",IVLVariables.LangResourceCultureInfo);
            pincode_lbl.Text = IVLVariables.LangResourceManager.GetString("PinCode_Label_Text", IVLVariables.LangResourceCultureInfo);
            #endregion
            #region Buttons initialize string from resources
            clearFields_btn.Text = IVLVariables.LangResourceManager.GetString( "Clear_Button_Text",IVLVariables.LangResourceCultureInfo);
            cancel_btn.Text = IVLVariables.LangResourceManager.GetString( "Cancel_Button_Text",IVLVariables.LangResourceCultureInfo);
            Update_btn.Text = IVLVariables.LangResourceManager.GetString( "Update_SaveUpdate",IVLVariables.LangResourceCultureInfo);
            Reload_btn.Text = IVLVariables.LangResourceManager.GetString( "Reload_btn_Text",IVLVariables.LangResourceCultureInfo);
            Age_radio.Text = IVLVariables.LangResourceManager.GetString( "Age_radio_button_Text",IVLVariables.LangResourceCultureInfo);
            Dob_radio.Text = IVLVariables.LangResourceManager.GetString( "dob_radio_button_Text",IVLVariables.LangResourceCultureInfo);
            #endregion
            List<Control> controls = GetControls(this).ToList();
            foreach (Control c in controls)
            {
                if (c is Label)
                {
                    Label l = c as Label;
                    if (Screen.PrimaryScreen.Bounds.Width == 1920)
                    {
                        if (l.Name == "MaxHealthstatus_lbl" || l.Name == "Maxcomments_lbl")
                            l.Font = new Font(l.Font.FontFamily.Name, 9f);
                        else
                            l.Font = new Font(l.Font.FontFamily.Name, 11f);
                    }
                    else if (Screen.PrimaryScreen.Bounds.Width == 1366)
                    {
                        if (l.Name == "MaxHealthstatus_lbl" || l.Name == "Maxcomments_lbl")
                            l.Font = new Font(l.Font.FontFamily.Name, 9f, FontStyle.Italic);
                        else
                            l.Font = new Font(l.Font.FontFamily.Name, 10f);
                    }
                }
            }
        }
        #endregion
        #region Private events

        /// <summary>
        /// This event will call ClearAllFields() for clearing all fields in the createpatient window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearFields_btn_Click(object sender, EventArgs e)
        {
            bool val = false;
            Args arg = new Args();
            arg["alert_msg"] = IVLVariables.LangResourceManager.GetString( "Clearconfirmation_Text",IVLVariables.LangResourceCultureInfo);
            arg["alert_header"] = IVLVariables.LangResourceManager.GetString( "Clear_Button_Text",IVLVariables.LangResourceCultureInfo);
            //This below if statement has been modified by Darshan on 29-10-2015 to resolved Defect no 0000699: Clear working in patient details for new patient entry.
            if (!isCreatePat)
            {
                val = AlertModificationExit(arg);
                if (val)
                {
                    isclear = true;
                    ClearAllFields();
                    //isUpdate = false;
                }
                else
                    if (!val)
                        return;
            }
            else
            {
                if (isFieldValueChanged)
                {
                    arg["alert_msg"] = IVLVariables.LangResourceManager.GetString("ClearFields_Conformation_Text", IVLVariables.LangResourceCultureInfo);
                    arg["alert_header"] = IVLVariables.LangResourceManager.GetString("ClearFields_Conformation_Header_Text", IVLVariables.LangResourceCultureInfo);
                    val = AlertModificationExit(arg);
                    if (val)
                    {
                        isclear = true;
                        ClearAllFields();
                        isFieldValueChanged = false;
                        //isUpdate = false;
                    }
                    else
                        if (!val)
                            return;
                }
            }
        }

        /// <summary>
        /// This event will call Cancel_Modification() for cancellation of the operation and exit from the create patient window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancel_btn_Click(object sender, EventArgs e)
        {
            Cancel_Modification();
        }

        /// <summary>
        /// This event which save the new patients deatils or save the modified patients deatils.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update_btn_Click(object sender, EventArgs e)
        {
            Args arg = new Args();
            if (populatePatDetails())//;
            {
                if (isCreatePat)
                {
                    //currentPat.ID =DataVariables._patientRepo.GetLastPatientID() + 1;

                    currentIdentifier.patient = currentPat;
                    //NewDataVariables._Repo.Add<patient_identifier>(currentIdentifier);
                    
                    currentPat.identifiers.Add(currentIdentifier);
                    currentPersonAddress.person = currentPat;
                    //NewDataVariables._Repo.Add<PersonAddressModel>(currentPersonAddress);

                    currentPat.addresses.Add(currentPersonAddress);
                    //NewIVLDataMethods.AddPatientIdentifier(currentIdentifier);
                    //if(currentPersonAddress.cityVillage!=null || currentPersonAddress.stateProvince !=null || currentPersonAddress.country!=null || currentPersonAddress.line1 != null || currentPersonAddress.line2 !=null || currentPersonAddress.postalCode !=null)
                    //NewIVLDataMethods.AddPersonAddress(currentPersonAddress);
                    foreach (person_attribute item in currentPersonAttribute)
                    {
                        item.person = currentPat;
                        //NewDataVariables._Repo.Add<person_attribute>(item);
                        currentPat.attributes.Add(item);

                    }
                    //currentPat.attributes = currentPersonAttribute.to;
                   // currentPat = NewDataVariables._Repo.GetById<Patient>(currentPat.personId);
                    //Patient p = NewDataVariables._Repo.GetByCategory<Patient>("personId",currentPat.personId).ToList<Patient>()[0];
                    currentPat.patientLastModifiedDate = DateTime.Now; 
                    NewDataVariables.Patients.Add(currentPat);
                    NewIVLDataMethods.AddPatient(currentPat);
                    NewIVLDataMethods.UpdatePatient();
                    //NewDataVariables.Patients = null;
                    //NewDataVariables.Patients = NewDataVariables._Repo.GetPageData<Patient>(10, 0).ToList();
                    IVLVariables.mrnCnt = mrnCnt;
                    IVLVariables.CurrentSettings.UserSettings._MrnCnt.val = IVLVariables.mrnCnt.ToString();
                    key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Intusoft", true);
                    key.SetValue("MRN", mrnCnt.ToString());
                    key.Close();
                    isCreatePat = false;
                }
                else
                {
                    NewIVLDataMethods.UpdatePatient();
                    NewIVLDataMethods.UpdatePatientAddress();
                    NewIVLDataMethods.UpdatePatientIdentifier();
                    foreach (person_attribute item in currentPersonAttribute)
                    {
                        NewDataVariables._Repo.Update<person_attribute>(item);
                    }
                }

                arg["isModified"] = true;
                isUpdate = false;
                arg["CurrentIndx"] = this.CurrentIndx;
                _eventHandler.Notify(_eventHandler.Back2Search, arg);
                this.ParentForm.Close();
            }
        }

        /// <summary>
        /// This event will allow only numbers in PatEditMobile_tbx TextBox.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e give code for the key pressed</param>
        private void PatEditMobile_tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) || e.KeyChar == 127)
                e.Handled = true;
        }

        /// <summary>
        /// This event will allow only numbers in PatEditStdCode_tbx TextBox.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e give code for the key pressed</param>
        private void PatEditStdCode_tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) || e.KeyChar == 127)
                e.Handled = true;
        }

        /// <summary>
        /// This event will allow only numbers in PatEditLandLine_tbx TextBox.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e give code for the key pressed</param>
        private void PatEditLandLine_tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) || e.KeyChar == 127)
                e.Handled = true;
        }

        /// <summary>
        /// This event will allow only numbers in pincode_tbx TextBox
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e give code for the key pressed</param>
        private void pincode_tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) || e.KeyChar == 127)
                e.Handled = true;
        }

        //This code has been added to resolve defect no 0000043
        /// <summary>
        /// This event allow only characters in PatEditFirstName_tbx TextBox. 
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e give code for the key pressed</param>
        private void PatEditFirstName_tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsPunctuation(e.KeyChar) && !char.IsSymbol(e.KeyChar) && e.KeyChar!= 127)
                e.Handled = false;
            else
                if (char.IsControl(e.KeyChar) && e.KeyChar != 127)
                    e.Handled = false;
                else
                    e.Handled = true;
        }

        //This code has been added to resolve defect no 0000043
        /// <summary>
        /// This event allow only characters in PatEditLastName_tbx TextBox. 
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e give code for the key pressed</param>
        private void PatEditLastName_tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsPunctuation(e.KeyChar) && !char.IsSymbol(e.KeyChar) && e.KeyChar != 127)

                e.Handled = false;
            else
                if (char.IsControl(e.KeyChar) && e.KeyChar != 127)
                    e.Handled = false;
                else
                    e.Handled = true;
        }

        private void dateOfBirth_dtPicker_keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                //dateOfBirth_dtPicker.Format = DateTimePickerFormat.Custom;//old implementation
                //dateOfBirth_dtPicker.CustomFormat = "dd- -yyyy ";
            }
        }

        private void dateOfBirth_dtPicker_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        /// <summary>
        /// This event will clear the deafult value which will be in gray color when PatEditFirstName_tbx is in focus and changes color to black.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e gives name of the EventArgs</param>
        private void PatEditFirstName_tbx_Enter(object sender, EventArgs e)
        {
            if (PatEditFirstName_tbx.Text.ToLower() == IVLVariables.LangResourceManager.GetString( "defaultFirstName_Textbox_Text",IVLVariables.LangResourceCultureInfo).ToLower())
            {
                PatEditFirstName_tbx.Text = string.Empty;
                setDefaultTextboxFontNColor(PatEditFirstName_tbx, false);
            }
            PatEditFirstName_tbx.SelectAll();
        }

        /// <summary>
        /// This event will clear the deafult value which will be in gray color when PatEditLastName_tbx is in focus and changes color to black.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e gives name of the EventArgs</param>
        private void PatEditLastName_tbx_Enter(object sender, EventArgs e)
        {
            if (PatEditLastName_tbx.Text.ToLower() == IVLVariables.LangResourceManager.GetString("defaultLastName_Textbox_Text", IVLVariables.LangResourceCultureInfo).ToLower())
            {
                PatEditLastName_tbx.Text = string.Empty;
                setDefaultTextboxFontNColor(PatEditLastName_tbx, false);
            }
            PatEditLastName_tbx.SelectAll();
        }

        /// <summary>
        /// This event will clear the deafult value which will be in gray color when PatEditMobile_tbx is in focus and changes color to black.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e gives name of the EventArgs</param>
        private void PatEditMobile_tbx_Enter(object sender, EventArgs e)
        {
            if (PatEditMobile_tbx.Text == IVLVariables.LangResourceManager.GetString("defaultMobile_Textbox_Text", IVLVariables.LangResourceCultureInfo) || PatEditMobile_tbx.Text == IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo))
            {
                PatEditMobile_tbx.Text = string.Empty;
                setDefaultTextboxFontNColor(PatEditMobile_tbx, false);
            }
        }

        /// <summary>
        /// This event will clear the deafult value which will be in gray color when PatEditStdCode_tbx is in focus and changes color to black.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e gives name of the EventArgs</param>
        private void PatEditStdCode_tbx_Enter(object sender, EventArgs e)
        {
            if (PatEditStdCode_tbx.Text == IVLVariables.LangResourceManager.GetString("defaultstdCode_Textbox_Text", IVLVariables.LangResourceCultureInfo) || PatEditStdCode_tbx.Text == "0")
            {
                PatEditStdCode_tbx.Text = string.Empty;
                setDefaultTextboxFontNColor(PatEditStdCode_tbx, false);
            }
        }

        /// <summary>
        /// This event will clear the deafult value which will be in gray color when PatEditLandLine_tbx is in focus and changes color to black.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e gives name of the EventArgs</param>
        private void PatEditLandLine_tbx_Enter(object sender, EventArgs e)
        {
            if (PatEditLandLine_tbx.Text == IVLVariables.LangResourceManager.GetString("defaultLandLine_Textbox_Text", IVLVariables.LangResourceCultureInfo) || PatEditLandLine_tbx.Text == "0")
            {
                PatEditLandLine_tbx.Text = string.Empty;
                setDefaultTextboxFontNColor(PatEditLandLine_tbx, false);
            }
        }

        /// <summary>
        /// This event will clear the deafult value which will be in gray color when Email_tbx is in focus and changes color to black.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e gives name of the EventArgs</param>
        private void Email_tbx_Enter(object sender, EventArgs e)
        {
            if (Email_tbx.Text == IVLVariables.LangResourceManager.GetString("defaultEmail_Textbox_Text", IVLVariables.LangResourceCultureInfo) || Email_tbx.Text == IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo))
            {
                Email_tbx.Text = string.Empty;
                setDefaultTextboxFontNColor(Email_tbx, false);
            }
        }

        /// <summary>
        /// This event will clear the default value which will be in gray color when referredBy_tbx is in focus and changes color to black.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e gives name of the EventArgs</param>
        private void referredBy_tbx_Enter(object sender, EventArgs e)
        {
            if (referredBy_tbx.Text == IVLVariables.LangResourceManager.GetString("defaultReferredBy_Textbox_Text", IVLVariables.LangResourceCultureInfo) || referredBy_tbx.Text == IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo))
            {
                referredBy_tbx.Text = string.Empty;
                setDefaultTextboxFontNColor(referredBy_tbx, false);
            }
        }

        /// <summary>
        /// This event will clear the deafult value which will be in gray color when healthStatus_tbx is in focus and changes color to black.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e gives name of the EventArgs</param>
        private void healthStatus_tbx_Enter(object sender, EventArgs e)
        {
            if (healthStatus_tbx.Text == IVLVariables.LangResourceManager.GetString("defaultHealthStatus_Textbox_Text", IVLVariables.LangResourceCultureInfo) || healthStatus_tbx.Text == IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo))
            {
                healthStatus_tbx.Text = string.Empty;
                setDefaultTextboxFontNColor(healthStatus_tbx, false);
            }
        }

        /// <summary>
        /// This event will clear the default value which will be in gray color when PatEditcomments_tbx is in focus and changes color to black.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e gives name of the EventArgs</param>
        private void PatEditcomments_tbx_Enter(object sender, EventArgs e)
        {
            if (PatEditcomments_tbx.Text == IVLVariables.LangResourceManager.GetString("defaultComments_Textbox_Text", IVLVariables.LangResourceCultureInfo) || PatEditcomments_tbx.Text == IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo))
            {
                PatEditcomments_tbx.Text = string.Empty;
                setDefaultTextboxFontNColor(PatEditcomments_tbx, false);
            }
        }

        /// <summary>
        /// This private event will clear the default value which will be in gray color when PatEditDoorStreet_Tbx is in focus and changes color to black.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e gives name of the EventArgs</param>
        private void PatEditDoorStreet_Tbx_Enter(object sender, EventArgs e)
        {
            if (PatEditDoorStreet_Tbx.Text == IVLVariables.LangResourceManager.GetString("defaultAddress1_Textbox_Text", IVLVariables.LangResourceCultureInfo) || PatEditDoorStreet_Tbx.Text == IVLVariables.LangResourceManager.GetString("NotMentioned_Text", IVLVariables.LangResourceCultureInfo))
            {
                PatEditDoorStreet_Tbx.Text = string.Empty;
                setDefaultTextboxFontNColor(PatEditDoorStreet_Tbx, false);
            }
        }

        /// <summary>
        /// This event will clear the default value which will be in gray color when PatEditAddressArea_tbx is in focus and changes color to black.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e gives name of the EventArgs</param>
        private void PatEditAddressArea_tbx_Enter(object sender, EventArgs e)
        {
            if (PatEditAddressArea_tbx.Text == IVLVariables.LangResourceManager.GetString("defaultAddress2_Textbox_Text", IVLVariables.LangResourceCultureInfo))
            {
                PatEditAddressArea_tbx.Text = string.Empty;
                setDefaultTextboxFontNColor(PatEditAddressArea_tbx, false);
            }
        }

        private void Dob_radio_CheckedChanged_1(object sender, EventArgs e)
        {
            bool rad = Dob_radio.Checked;
            if (rad)
            {
            }
            else
            {
            }
        }

        /// <summary>
        /// This event which will enable Age_radio when its is in focus and disable Age_radio when Dob_radio is in focus.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e gives name of the EventArgs</param>
        private void Age_radio_CheckedChanged(object sender, EventArgs e)
        {
            if (Age_radio.Checked)
            {
                dateOfBirth_dtPicker.Enabled = false;
                dateOfBirth_dtPicker.Visible = false;
                age_nud.Enabled = true;
                age_nud.Visible = true;
                AgeDob_p.Controls.Remove(dateOfBirth_dtPicker);
                AgeDob_p.Controls.Add(age_nud);
            }
            else
            {
                dateOfBirth_dtPicker.Enabled = true;
                dateOfBirth_dtPicker.Visible = true;
                age_nud.Visible = false;
                AgeDob_p.Controls.Remove(age_nud);
                dateOfBirth_dtPicker.Dock = DockStyle.Fill;
                AgeDob_p.Controls.Add(dateOfBirth_dtPicker);
                age_nud.Enabled = false;
            }
        }

        /// <summary>
        /// This event will clear the default value which will be in gray color when pincode_tbx is in focus and changes color to black.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e gives name of the EventArgs</param>
        private void pincode_tbx_Enter(object sender, EventArgs e)
        {
            if (pincode_tbx.Text == IVLVariables.LangResourceManager.GetString("defaultPincode_Textbox_Text", IVLVariables.LangResourceCultureInfo) || pincode_tbx.Text == "0")
            {
                pincode_tbx.Text = string.Empty;
                setDefaultTextboxFontNColor(pincode_tbx, false);
            }
        }
        /// <summary>
        /// This event will clear the default value which will be in gray color when pincode_tbx is in focus and changes color to black.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e gives name of the EventArgs</param>
        private void otherOccupationDetails_tbx_Enter(object sender, EventArgs e)
        {
            // checks for the otherOccupationDetails_tbx text equals to the default value of the otherOccupationDetails_tbx
            if (otherOccupationDetails_tbx.Text == IVLVariables.LangResourceManager.GetString("DefaultOtherOccupation_Text", IVLVariables.LangResourceCultureInfo) || otherOccupationDetails_tbx.Text == "Specify other occupation")
            {
                //if true the otherOccupationdetails_tbx text becomes null and changes the font color to black and font to regular
                otherOccupationDetails_tbx.Text = string.Empty;
                setDefaultTextboxFontNColor(otherOccupationDetails_tbx, false);
            }
        }
 
        /// <summary>
        /// This event which allow only characters in referredBy_tbx TextBox. 
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e give code for the key pressed</param>
        private void referredBy_tbx_keyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 127)
                e.Handled = false;
            else
                if (char.IsControl(e.KeyChar) && e.KeyChar != 127)
                    e.Handled = false;
                else
                    e.Handled = true;
        }

        /// <summary>
        /// This event which will display the tool tip when mouse is hovered on mrn_lbl.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mrn_lbl_MouseHover(object sender, EventArgs e)
        {
            ToolTip tool1 = new ToolTip();
            tool1.SetToolTip(mrn_lbl, IVLVariables.LangResourceManager.GetString("MrnToolTip_lbl", IVLVariables.LangResourceCultureInfo));
        }

         //The below code is added solve defect no 0000030
        /// <summary>
        /// This event will set the change the Country_combobx selection and disables City_combobx and  State_combobx if the selected value is others. 
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e give code for the key pressed</param>
        private void Country_combobx_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //String country = "Others";
            isFieldValueChanged = true;
            Reload_btn.Enabled = true;//Has been added to  maintain the status of the reload button
            if (Country_combobx.SelectedItem != null)//has been added to avoid the crash of null reference when the combobox is empty
            {
                if (Country_combobx.SelectedItem.Equals(CountryEnum.others.ToString()))
                {
                    City_combobx.Enabled = false;
                    State_combobx.Enabled = false;
                    City_combobx.Text = INTUSOFT.Data.Common.GetDescription(CityEnum.City_Not_Selected);
                    State_combobx.Text = INTUSOFT.Data.Common.GetDescription(StateEnum.State_Not_Selected);
                }
                else
                {
                    City_combobx.Enabled = true;
                    State_combobx.Enabled = true;
                }
            }
        }

        /// <summary>
        /// This event allow only characters in City_combobx TextBox. 
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e give code for the key pressed</param>
        private void City_combobx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) || e.KeyChar == 127)
                e.Handled = true;
            else
                AutoComplete(City_combobx, e);
        }

        /// <summary>
        /// This event allow only characters in State_combobx TextBox. 
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e give code for the key pressed</param>
        private void State_combobx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) || e.KeyChar == 127)
                e.Handled = true;
            else
                AutoComplete(State_combobx, e);
        }

        /// <summary>
        ///This event allow only characters in Country_combobx TextBox. 
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e give code for the key pressed</param>
        private void Country_combobx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) || e.KeyChar == 127)
                e.Handled = true;
            else
                AutoComplete(Country_combobx, e);
        }

        /// <summary>
        /// This event will set the value of City_combobx to State Not Selected if City_combobx is empty.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e gives name of the EventArgs</param>
        private void City_combobx_MouseLeave(object sender, EventArgs e)
        {
            if (City_combobx.Text == " ")
                City_combobx.Text = INTUSOFT.Data.Common.GetDescription(CityEnum.City_Not_Selected);
        }

        /// <summary>
        /// This event will set the value of State_combobx to City Not Selected if State_combobx is empty.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e gives name of the EventArgs</param>
        private void State_combobx_MouseLeave(object sender, EventArgs e)
        {
            if (State_combobx.Text == " ")
                State_combobx.Text = INTUSOFT.Data.Common.GetDescription(StateEnum.State_Not_Selected);
        }

        /// <summary>
        /// This event will set the value of Country_combobx to Country Not Selected if Country_combobx is empty.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e gives name of the EventArgs</param>
        private void Country_combobx_MouseLeave(object sender, EventArgs e)
        {
            if (Country_combobx.Text == " ")
                Country_combobx.Text = INTUSOFT.Data.Common.GetDescription(CountryEnum.Country_Not_Selected);
        }

        /// <summary>
        /// This event will clear the deafult value which is in gray color when age_nud is in focus and changes color to black.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e gives name of the EventArgs</param>
        private void age_nud_Enter(object sender, EventArgs e)
        {
            age_nud.Select(0, 3);
            age_nud.ForeColor = Color.Black;
        }

        /// <summary>
        ///This event will clear the deafult value which is in gray color when weight_nud is in focus and changes color to black.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e gives name of the EventArgs</param>
        private void weight_nud_Enter(object sender, EventArgs e)
        {
            weight_nud.ForeColor = Color.Black;
            weight_nud.Select(weight_nud.ToString().Length, 0);
        }

        /// <summary>
        /// This event will clear the deafult value which is in gray color when height_nud is in focus and changes color to black.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e gives name of the EventArgs</param>
        private void height_nud_Enter(object sender, EventArgs e)
        {
            height_nud.ForeColor = Color.Black;
            height_nud.Select(height_nud.ToString().Length, 0);
        }

        /// <summary>
        /// This event will select the entire value in  weight_nud when value is changed.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e gives name of the EventArgs</param>
        private void weight_nud_ValueChanged(object sender, EventArgs e)
        {
            weight_nud.Value = Math.Round(weight_nud.Value,weight_nud.DecimalPlaces, MidpointRounding.AwayFromZero);//to round of the values in the weight_nud text box.
            isFieldValueChanged = true;
            Reload_btn.Enabled = true;//Has been added to  maintain the status of the reload button
        }
 
        /// <summary>
        /// This event will select the entire value in height_nud when value is changed.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e gives name of the EventArgs</param>
        private void height_nud_ValueChanged(object sender, EventArgs e)
        {
            //height_nud.Select(height_nud.ToString().Length, 0);
            height_nud.Value = Math.Round(height_nud.Value, height_nud.DecimalPlaces, MidpointRounding.AwayFromZero);//to round of the values in the height_nud text box.
            isFieldValueChanged = true;
            Reload_btn.Enabled = true;//Has been added to  maintain the status of the reload button
        }

        /// <summary>
        /// This event will change the date in dateOfBirth_dtPicker based on value entered in age_nud.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e gives name of the EventArgs</param>
        private void age_nud_ValueChanged(object sender, EventArgs e)
        {
            DateTime dt = new DateTime(DateTime.Now.Year - (int)age_nud.Value, 1, 1);// dateOfBirth_dtPicker.Value.AddYears(-(int)age_nud.Value);
            if (dateOfBirth_dtPicker.Value.Year != dt.Year)
                dateOfBirth_dtPicker.Value = dt;
            isFieldValueChanged = true;
            Reload_btn.Enabled = true;//Has been added to  maintain the status of the reload button

        }

        /// <summary>
        /// This event will change the value in age_nud based on date in dateOfBirth_dtPicker.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e gives name of the EventArgs</param>
        private void dateOfBirth_dtPicker_ValueChanged_1(object sender, EventArgs e)
        {
            age_nud.Value = DateTime.Now.Year - dateOfBirth_dtPicker.Value.Year;
        }
 
        /// <summary>
        /// This event will clear the deafult value which will be in gray color when PatEditMrn_tbx is in focus and changes color to black.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e gives name of the EventArgs</param>
        private void PatEditMrn_tbx_Enter(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(IVLVariables.CurrentSettings.UserSettings._IsMrnConfigurable.val))
            {
                //This below if statement is added by Darshan on 17-08-2015 to solve Defect no 0000504: IVL config modified:Entry can be made without MRN,DOB/Age not displayed.
                if (isCreatePat)
                {
                    PatEditMrn_tbx.Text = string.Empty;
                    setDefaultTextboxFontNColor(PatEditMrn_tbx, false);
                }
            }
        }

        /// <summary>
        /// This event will call ReloadDetails() to reload all details in create patient window.
        /// </summary>
        /// <param name="sender">sender object name</param>
        /// <param name="e">e gives name of the EventArgs</param>
        private void Reload_btn_Click(object sender, EventArgs e)
        {
            ReloadDetails();
        }

        /// <summary>
        /// This key press event has been added to stop negative symbol from the age.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void age_nud_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsPunctuation(e.KeyChar) ||  e.KeyChar== 127)
            {
                
                e.Handled = true;
            }
            
        }

        private void PatEditFirstName_tbx_TextChanged(object sender, EventArgs e)
        {
            if (PatEditFirstName_tbx.Text == string.Empty)
                isFieldValueChanged = false;
            else
            {
                isFieldValueChanged = true;
                Reload_btn.Enabled = true;//Has been added to  maintain the status of the reload button
            }
        }

        private void PatEditLastName_tbx_TextChanged(object sender, EventArgs e)
        {
            isFieldValueChanged = true;
            Reload_btn.Enabled = true;//Has been added to  maintain the status of the reload button
        }

        private void PatEditGender_cmbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            isFieldValueChanged = true;
            Reload_btn.Enabled = true;//Has been added to  maintain the status of the reload button
        }

        private void Occupation_combobx_SelectionChangeCommitted(object sender, EventArgs e)
        {
            isFieldValueChanged = true;
            Reload_btn.Enabled = true;//Has been added to  maintain the status of the reload button
        }

        private void PatEditIncome_cmbx_SelectionChangeCommitted(object sender, EventArgs e)
        {
            isFieldValueChanged = true;
            Reload_btn.Enabled = true;//Has been added to  maintain the status of the reload button
        }

        private void PatEditMobile_tbx_TextChanged(object sender, EventArgs e)
        {
            isFieldValueChanged = true;
            Reload_btn.Enabled = true;//Has been added to  maintain the status of the reload button
        }

        private void PatEditStdCode_tbx_TextChanged(object sender, EventArgs e)
        {
            isFieldValueChanged = true;
            Reload_btn.Enabled = true;//Has been added to  maintain the status of the reload button
        }

        private void PatEditLandLine_tbx_TextChanged(object sender, EventArgs e)
        {
            isFieldValueChanged = true;
            Reload_btn.Enabled = true;//Has been added to  maintain the status of the reload button
        }

        private void Email_tbx_TextChanged(object sender, EventArgs e)
        {
            isFieldValueChanged = true;
            Reload_btn.Enabled = true;//Has been added to  maintain the status of the reload button
        }

        private void referredBy_tbx_TextChanged(object sender, EventArgs e)
        {
            isFieldValueChanged = true;
            Reload_btn.Enabled = true;//Has been added to  maintain the status of the reload button
        }

        private void healthStatus_tbx_TextChanged(object sender, EventArgs e)
        {
            isFieldValueChanged = true;
            Reload_btn.Enabled = true;//Has been added to  maintain the status of the reload button
        }

        private void PatEditcomments_tbx_TextChanged(object sender, EventArgs e)
        {
            isFieldValueChanged = true;
            Reload_btn.Enabled = true;//Has been added to  maintain the status of the reload button
        }

        private void PatEditDoorStreet_Tbx_TextChanged(object sender, EventArgs e)
        {
            isFieldValueChanged = true;
            Reload_btn.Enabled = true;//Has been added to  maintain the status of the reload button
        }

        private void PatEditAddressArea_tbx_TextChanged(object sender, EventArgs e)
        {
            isFieldValueChanged = true;
            Reload_btn.Enabled = true;//Has been added to  maintain the status of the reload button
        }

        private void City_combobx_SelectionChangeCommitted(object sender, EventArgs e)
        {
            isFieldValueChanged = true;
            Reload_btn.Enabled = true;//Has been added to  maintain the status of the reload button
        }

        private void State_combobx_SelectionChangeCommitted(object sender, EventArgs e)
        {
            isFieldValueChanged = true;
            Reload_btn.Enabled = true;//Has been added to  maintain the status of the reload button
        }

        private void pincode_tbx_TextChanged(object sender, EventArgs e)
        {
            isFieldValueChanged = true;
            Reload_btn.Enabled = true;//Has been added to  maintain the status of the reload button
        }

        //Text Change event for the numeric up downs has been added to handle the undo button enabling issue.
        private void age_nud_TextChanged(object sender, EventArgs e)
        {
            isFieldValueChanged = true;
            Reload_btn.Enabled = true;//Has been added to  maintain the status of the reload button
        }

        private void height_nud_TextChanged(object sender, EventArgs e)
        {
            isFieldValueChanged = true;
            Reload_btn.Enabled = true;//Has been added to  maintain the status of the reload button
        }

        private void weight_nud_TextChanged(object sender, EventArgs e)
        {
            isFieldValueChanged = true;
            Reload_btn.Enabled = true;//Has been added to  maintain the status of the reload button
        }

        private void weight_nud_Leave(object sender, EventArgs e)
        {
            numericUpdownLeave(sender);
        }

        public void numericUpdownLeave(object sender)
        {
            NumericUpDown nud = sender as NumericUpDown;
            if (nud != null)
            {
                if (nud.Text == string.Empty)
                {
                    nud.Value = nud.Minimum;
                    nud.Text = nud.Value.ToString();
                }
            }
        }

        private void height_nud_Leave(object sender, EventArgs e)
        {
            numericUpdownLeave(sender);
        }
        #endregion

        private void Email_tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 127)
                e.Handled = true;
        }
        private void healthStatus_tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 127)
                e.Handled = true;
        }

        private void PatEditcomments_tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 127)
                e.Handled = true;
        }

        private void PatEditDoorStreet_Tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 127)
                e.Handled = true;
        }

        private void PatEditAddressArea_tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 127)
                e.Handled = true;
        }

        private void weight_nud_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(  e.KeyChar== 127)
                e.Handled = true;
        }

        private void height_nud_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 127)
                e.Handled = true;
        }
        /// <summary>
        /// This event is to show a textbox and to specify the other occupation in that textbox 
        /// </summary>
        /// <param name="sender">Occupation_combobx</param>
        /// <param name="e">EventArgs e</param>
        private void Occupation_combobx_SelectedIndexChanged(object sender, EventArgs e)
        {
            //check the index value equals to the length of the array - 1
            if ((Occupation_description_values.Length - 1) == Occupation_combobx.SelectedIndex)
            //MessageBox.Show(Occupation_description_values[Occupation_combobx.SelectedIndex]);
            {
                //if the above condition is true then the textbox will be visible
                otherOccupationDetails_tbx.Visible = true;
                Occupation_description_values[(Occupation_description_values.Length - 1)] = otherOccupationDetails_tbx.Text;
            }
                //else the textbox will be hidden
            else if (otherOccupationDetails_tbx.Visible)
                otherOccupationDetails_tbx.Visible = false;
        }
        /// <summary>
        /// This event raises whenever the otherOccupationDetails_tbx left empty or null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void otherOccupationDetails_tbx_Leave(object sender, EventArgs e)
        {
            //checks for the otherOccupationsDetails_tbx value is empty or null
            if (string.IsNullOrEmpty(otherOccupationDetails_tbx.Text))
            {
                //it will assign the default value of the otherOccupationDetails_tbx
                otherOccupationDetails_tbx.Text = IVLVariables.LangResourceManager.GetString("DefaultOtherOccupation_Text", IVLVariables.LangResourceCultureInfo);
                setDefaultTextboxFontNColor(otherOccupationDetails_tbx, true);
            }
        }
        /// <summary>
        /// This event raises whenever the PatEditMobile_tbx left empty or null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PatEditMobile_tbx_Leave(object sender, EventArgs e)
        {
            //checks for the PatEditMobile_tbx value is empty or null
            if (string.IsNullOrEmpty(PatEditMobile_tbx.Text))
            {
                //it will assign the default value of the PatEditMobile_tbx
                PatEditMobile_tbx.Text = IVLVariables.LangResourceManager.GetString("defaultMobile_Textbox_Text", IVLVariables.LangResourceCultureInfo);
                setDefaultTextboxFontNColor(PatEditMobile_tbx, true);
            }
        }
        /// <summary>
        /// This event raises whenever the PatEditLandLine_tbx left empty or null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void PatEditLandLine_tbx_Leave(object sender, EventArgs e)
        {
            //checks for the PatEditLandLine_tbx value is empty or null
            if (string.IsNullOrEmpty(PatEditLandLine_tbx.Text))
            {
                //it will assign the default value of the PatEditLandLine_tbx
                PatEditLandLine_tbx.Text = IVLVariables.LangResourceManager.GetString("defaultLandLine_Textbox_Text", IVLVariables.LangResourceCultureInfo);
                setDefaultTextboxFontNColor(PatEditLandLine_tbx, true);
            }
        }

        /// <summary>
        /// This event raises whenever the PatEditStdCode_tbx left empty or null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PatEditStdCode_tbx_Leave(object sender, EventArgs e)
        {
            //checks for the PatEditStdCode_tbx value is empty or null
            if (string.IsNullOrEmpty(PatEditStdCode_tbx.Text))
            {
                //it will assign the default value of the PatEditStdCode_tbx
                PatEditStdCode_tbx.Text = IVLVariables.LangResourceManager.GetString("defaultstdCode_Textbox_Text", IVLVariables.LangResourceCultureInfo);
                setDefaultTextboxFontNColor(PatEditStdCode_tbx, true);
            }
        }

        /// <summary>
        /// This event raises whenever the Email_tbx left empty or null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Email_tbx_Leave(object sender, EventArgs e)
        {
            //checks for the Email_tbx value is empty or null
            if (string.IsNullOrEmpty(Email_tbx.Text))
            {
                //it will assign the default value of the Email_tbx
                Email_tbx.Text = IVLVariables.LangResourceManager.GetString("defaultEmail_Textbox_Text", IVLVariables.LangResourceCultureInfo);
                setDefaultTextboxFontNColor(Email_tbx, true);
            }
        }

        /// <summary>
        /// This event raises whenever the referredBy_tbx left empty or null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void referredBy_tbx_Leave(object sender, EventArgs e)
        {
            //checks for the referredBy_tbx value is empty or null
            if (string.IsNullOrEmpty(referredBy_tbx.Text))
            {
                //it will assign the default value of the referredBy_tbx
                referredBy_tbx.Text = IVLVariables.LangResourceManager.GetString("defaultReferredBy_Textbox_Text", IVLVariables.LangResourceCultureInfo);
                setDefaultTextboxFontNColor(referredBy_tbx, true);
            }
        }

        /// <summary>
        /// This event raises whenever the healthStatus_tbx left empty or null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void healthStatus_tbx_Leave(object sender, EventArgs e)
        {
            //checks for the healthStatus_tbx value is empty or null
            if (string.IsNullOrEmpty(healthStatus_tbx.Text))
            {
                //it will assign the default value of the healthStatus_tbx
                healthStatus_tbx.Text = IVLVariables.LangResourceManager.GetString("defaultHealthStatus_Textbox_Text", IVLVariables.LangResourceCultureInfo);
                setDefaultTextboxFontNColor(healthStatus_tbx, true);
            }
        }

        /// <summary>
        /// This event raises whenever the PatEditcomments_tbx left empty or null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PatEditcomments_tbx_Leave(object sender, EventArgs e)
        {
            //checks for the PatEditcomments_tbx value is empty or null
            if (string.IsNullOrEmpty(PatEditcomments_tbx.Text))
            {
                //it will assign the default value of the PatEditcomments_tbx
                PatEditcomments_tbx.Text = IVLVariables.LangResourceManager.GetString("defaultComments_Textbox_Text", IVLVariables.LangResourceCultureInfo);
                setDefaultTextboxFontNColor(PatEditcomments_tbx, true);
            }
        }

        /// <summary>
        /// This event raises whenever the PatEditDoorStreet_Tbx left empty or null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PatEditDoorStreet_Tbx_Leave(object sender, EventArgs e)
        {
            //checks for the PatEditDoorStreet_Tbx value is empty or null
            if (string.IsNullOrEmpty(PatEditDoorStreet_Tbx.Text))
            {
                //it will assign the default value of the PatEditDoorStreet_Tbx
                PatEditDoorStreet_Tbx.Text = IVLVariables.LangResourceManager.GetString("defaultAddress1_Textbox_Text", IVLVariables.LangResourceCultureInfo);
                setDefaultTextboxFontNColor(PatEditDoorStreet_Tbx, true);
            }
        }

        /// <summary>
        /// This event raises whenever the PatEditAddressArea_tbx left empty or null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PatEditAddressArea_tbx_Leave(object sender, EventArgs e)
        {
            //checks for the PatEditAddressArea_tbx value is empty or null
            if (string.IsNullOrEmpty(PatEditAddressArea_tbx.Text))
            {
                //it will assign the default value of the PatEditAddressArea_tbx
                PatEditAddressArea_tbx.Text = IVLVariables.LangResourceManager.GetString("defaultAddress2_Textbox_Text", IVLVariables.LangResourceCultureInfo);
                setDefaultTextboxFontNColor(PatEditAddressArea_tbx, true);
            }
        }

        /// <summary>
        /// This event raises whenever the pincode_tbx left empty or null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pincode_tbx_Leave(object sender, EventArgs e)
        {
            //checks for the pincode_tbx value is empty or null
            if (string.IsNullOrEmpty(pincode_tbx.Text))
            {
                //it will assign the default value of the pincode_tbx
                pincode_tbx.Text = IVLVariables.LangResourceManager.GetString("defaultPincode_Textbox_Text", IVLVariables.LangResourceCultureInfo);
                setDefaultTextboxFontNColor(pincode_tbx, true);
            }
        }
        /// <summary>
        /// This event raises whenever the PatEditFirstName_tbx left empty or null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PatEditFirstName_tbx_Leave(object sender, EventArgs e)
        {
            //checks for the PatEditFirstName_tbx value is empty or null
            if (string.IsNullOrEmpty(PatEditFirstName_tbx.Text))
            {
                //it will assign the default value of the PatEditFirstName_tbx
                PatEditFirstName_tbx.Text = IVLVariables.LangResourceManager.GetString("defaultFirstName_Textbox_Text", IVLVariables.LangResourceCultureInfo);
                setDefaultTextboxFontNColor(PatEditFirstName_tbx, true);
            }
        }
        /// <summary>
        /// This event raises whenever the PatEditLastName_tbx left empty or null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PatEditLastName_tbx_Leave(object sender, EventArgs e)
        {
            //checks for the PatEditLastName_tbx value is empty or null
            if (string.IsNullOrEmpty(PatEditLastName_tbx.Text))
            {
                //it will assign the default value of the PatEditLastName_tbx
                PatEditLastName_tbx.Text = IVLVariables.LangResourceManager.GetString("defaultLastName_Textbox_Text", IVLVariables.LangResourceCultureInfo);
                setDefaultTextboxFontNColor(PatEditLastName_tbx, true);
            }
        }

        private void otherOccupationDetails_tbx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)//check whether enter key is pressed
                e.SuppressKeyPress = true;//enter key is disabled
        }

        private void pincode_tbx_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')//check whether space bar  is pressed
                e.Handled = true;//space bar is disabled
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) || e.KeyChar == 127)//to check whether the alphabets or other special character is pressed
                e.Handled = true;
        }


    }
}