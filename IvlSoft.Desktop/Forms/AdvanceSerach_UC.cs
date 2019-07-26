using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using INTUSOFT.Desktop.Properties;
using System.Data;
using INTUSOFT.Data.NewDbModel;
using INTUSOFT.Data.Repository;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace INTUSOFT.Desktop.Forms
{
    //This advanceserach user control is added by Darshan on 30-07-2015 in order to supprt new feature for advance search using date.
    public partial class AdvanceSerach_UC : UserControl
    {
        #region variable and constants
        public delegate void AdvanceSerachDelegate(AdvanceSearchParams @params);
        public event AdvanceSerachDelegate advancesearchevent;
        List<string> options;
        string currentOption;
        DateTime Fromdate;
        DateTime Todate;
        static AdvanceSerach_UC createModifyPatient_UC = null;
        #endregion
        public AdvanceSerach_UC()
        {
            InitializeComponent();
            InitializeResourceString();
            options = new List<string>();
            options.Add(IVLVariables.LangResourceManager.GetString( "ModificationDateTime_text",IVLVariables.LangResourceCultureInfo));
            options.Add(IVLVariables.LangResourceManager.GetString( "TouchedDateTime_Text",IVLVariables.LangResourceCultureInfo));
            options.Add(IVLVariables.LangResourceManager.GetString( "RegistrationDateTime_text",IVLVariables.LangResourceCultureInfo));
            Searchoptions_cbx.DataSource = options;
            //This below code has been added by Darshan on 19-08-2015 to solve Defect no 0000580: from dates and to dates selection is random.
            Fromdtpicker_dtp.MaxDate = DateTime.Now.Date;
            Todtpicker_dtp.MaxDate = DateTime.Now.Date;
            UpdateFontForeColor();
        }
        #region public methods
        /// <summary>
        /// This method will reset the data of each control to its default value in advance search window.
        /// </summary>
        public void Advserach_reset()
        {
            Fromdtpicker_dtp.Value = DateTime.Now.Date;
            Todtpicker_dtp.Value = DateTime.Now.Date;
            Searchoptions_cbx.SelectedIndex = 0;
        }
        /// <summary>
        ///This fucntion will create a single instance of type AdvanceSerach_UC.
        /// </summary>
        /// <returns>Rerurns a user defined variable of type AdvanceSerach_UC</returns>
        public static AdvanceSerach_UC getInstance()
        {
            if (createModifyPatient_UC == null)
                createModifyPatient_UC = new AdvanceSerach_UC();
            return createModifyPatient_UC;
        }

        public void UpdateFontForeColor()
        {
            List<Control> controls = GetControls(this).ToList();
            foreach (Control c in controls)
            {
                if (c is Label)
                {
                    //Label l = c as Label;
                    //if (l.Name == PatientCRUD_ts.Name || l.Name == AllPatients_ts.Name || l.Name == Visits_ts.Name || l.Name == Consultation_ts.Name)
                    {
                        //for (int i = 0; i < controls.Count; i++)
                        if(c.Name != Searchoptions_cbx.Name)
                        {
                            c.ForeColor = IVLVariables.GradientColorValues.FontForeColor;
                            c.Refresh();
                        }

                    }
                }

            }
        }

        /// <summary>
        /// This fucntion will initializes the name for all controls in AdvanceSearch_UC.
        /// </summary>
        private void InitializeResourceString()
        {
            #region labels initilize string from resources
            Fromdate_lbl.Text = IVLVariables.LangResourceManager.GetString( "Advancesearch_fromlbl_text",IVLVariables.LangResourceCultureInfo);;
            Todate_lbl.Text = IVLVariables.LangResourceManager.GetString( "Advacesearch_tolbl_text",IVLVariables.LangResourceCultureInfo);;
            Searchoptions_lbl.Text = IVLVariables.LangResourceManager.GetString( "Advancesearch_options_text",IVLVariables.LangResourceCultureInfo);;


            #endregion

            #region Buttons initialize string from resources
            Advsearchcancel_btn.Text = IVLVariables.LangResourceManager.GetString( "Advacesearch_cancelbtn_text",IVLVariables.LangResourceCultureInfo);;
            Advsearchok_btn.Text = IVLVariables.LangResourceManager.GetString( "Advacesearch_okbtn_text",IVLVariables.LangResourceCultureInfo);;
            #endregion
            List<Control> controls = GetControls(this).ToList();
            foreach (Control c in controls)
            {
                if (c is Label)
                {
                    Label l = c as Label;
                    if (Screen.PrimaryScreen.Bounds.Width == 1920)
                    {

                        l.Font = new Font(l.Font.FontFamily.Name, 11f, FontStyle.Bold);
                    }
                    else if (Screen.PrimaryScreen.Bounds.Width == 1366)
                    {
                        l.Font = new Font(l.Font.FontFamily.Name, 10f, FontStyle.Bold);
                    }
                    else if (Screen.PrimaryScreen.Bounds.Width == 1280)
                    {
                        l.Font = new Font(l.Font.FontFamily.Name, 10f, FontStyle.Bold);
                    }
                }
            }
        }
        /// <summary>
        /// This function will return list of controls in Advancesearch form.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
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
        #endregion
        #region private events
        /// <summary>
        /// This event will give the list of resultant patients in the advance search operation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Advsearchok_btn_Click(object sender, EventArgs e)
        {
            //This below code has been added by Darshan on 19-08-2015 to solve Defect no 0000580: from dates and to dates selection is random.

            if (NewDataVariables.Patients != null && NewDataVariables.Patients.Count > 0)
            {
                if (Todtpicker_dtp.Value < Fromdtpicker_dtp.Value)
                {
                    Common.CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("FromTo_Message", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("AdvanceSearch_Text", IVLVariables.LangResourceCultureInfo), Common.CustomMessageBoxIcon.Warning);
                    Todtpicker_dtp.Value = Todtpicker_dtp.MaxDate;
                    return;
                }
                else
                {
                    Fromdate = Fromdtpicker_dtp.Value;
                    Todate = Todtpicker_dtp.Value;
                    AdvanceSearchParams @params = new AdvanceSearchParams();
                    if (Todate.Date == DateTime.Now.Date)
                        @params.toDate = DateTime.Now;
                    else
                        @params.toDate = Todate;
                    @params.fromDate = Fromdate;

                    if (Searchoptions_cbx.Text.Equals(IVLVariables.LangResourceManager.GetString("ModificationDateTime_text", IVLVariables.LangResourceCultureInfo)))
                    {
                        @params.searchType = SearchItems.patientLastModifiedDate;

                        //pats = NewDataVariables.Patients.Where(x => x.patientLastModifiedDate.Date >= Fromdate.Date && x.patientLastModifiedDate.Date <= Todate.Date && x.voided == false).ToList().Take(Convert.ToInt32(IVLVariables.CurrentSettings.UserSettings._NoOfPatientsToBeSelected.val)).ToList();
                        //patId = NewDataVariables.Identifier.Where(x => x.lastModifiedDate.Date >= Fromdate.Date && x.lastModifiedDate.Date <= Todate.Date && x.voided == false).ToList();
                    }
                    else
                        if (Searchoptions_cbx.Text.Equals(IVLVariables.LangResourceManager.GetString("TouchedDateTime_Text", IVLVariables.LangResourceCultureInfo)))
                        {
                            //pats = NewDataVariables.Patients.Where(x => x.date_accessed.Date >= Fromdate.Date && x.date_accessed.Date <= Todate.Date && x.voided == false).ToList();
                            //patId = NewDataVariables.Identifier.Where(x => x.lastModifiedDate.Date >= Fromdate.Date && x.lastModifiedDate.Date <= Todate.Date && x.voided == false).ToList();
                            @params.searchType = SearchItems.lastAccessedDate;

                        }
                        else
                            if (Searchoptions_cbx.Text.Equals(IVLVariables.LangResourceManager.GetString("RegistrationDateTime_text", IVLVariables.LangResourceCultureInfo)))
                            {
                                @params.searchType = SearchItems.patientCreatedDate;

                                //pats = NewDataVariables.Patients.Where(x => x.createdDate.Date >= Fromdate.Date && x.createdDate.Date <= Todate.Date && x.voided == false).ToList();
                                //patId = NewDataVariables.Identifier.Where(x => x.createdDate.Date >= Fromdate.Date && x.createdDate.Date <= Todate.Date && x.voided == false).ToList();
                            }
                    this.ParentForm.Close();
                    Advserach_reset();
                    //pats.Take(Convert.ToInt32(IVLVariables.CurrentSettings.UserSettings._NoOfPatientsToBeSelected.val)).ToList();
                    advancesearchevent(@params);
                }
            }
            else
            {
                this.ParentForm.Close();
            }
        }
        /// <summary>
        /// This event will cause exit from the Advance search window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Advsearchcancel_btn_Click(object sender, EventArgs e)
        {
            this.ParentForm.Close();
        }
        #endregion
    }
}

