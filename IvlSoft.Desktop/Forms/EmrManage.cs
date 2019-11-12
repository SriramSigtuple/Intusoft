using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Data;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Runtime.InteropServices;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using INTUSOFT.Desktop.Properties;
using INTUSOFT.Data.Enumdetails;
using INTUSOFT.Data.NewDbModel;
using INTUSOFT.EventHandler;
using INTUSOFT.Data.Repository;
using INTUSOFT.Data.Extension;
using INTUSOFT.Custom.Controls;
using Microsoft.Win32;
using INTUSOFT.Imaging;
using Common;
using NLog;
using NLog.Config;
using NLog.Targets;
using NHibernate.Linq;
using NHibernate;
using NHibernate.Criterion;
using Newtonsoft.Json;

namespace INTUSOFT.Desktop.Forms
{
    public partial class EmrManage : UserControl
    {
        #region variables and constants
        bool visitColumnsVisible = false;
        IVLEventHandler _eventHandler;
        CreatePatientDetails_form cf;
        public Timer t = new Timer();
        bool isReport;
        int currentIndx = 0;
        int MRNIndex = 0;
        int firstNameIndex = 1;
        int secondNameIndex = 2;
        int ageIndex = 3;
        int genderIndex = 4;
        int selectedVisitIndex = 0;
        System.Diagnostics.Stopwatch stW;
        public delegate void ReportDelegate();
        public event ReportDelegate reportViewEvent;
        //List<INTUSOFT.Data.NewDbModel.Patient> TemPatList;
        int prevCntPatCnt = 0;
        bool isUpdate = false;
        Logger ExceptionLog = LogManager.GetLogger("ExceptionLog");
        int pageCount = 0;
        int patCount = 0;
        /// <summary>
        /// Variable to store error message
        /// </summary>
        private string errorMessage;

        List<string> patDetList;
        /// <summary>
        /// Member id
        /// </summary>
        private int memberId;
        public PatientDetails_UC patd;
        CreateModifyPatient_UC createPatientUC;
        String reportno = IVLVariables.LangResourceManager.GetString("No_of_Reports_Text", IVLVariables.LangResourceCultureInfo);
        public delegate void ManageClosed(string a, EventArgs e);
        public EventArgs e = null;
        public event ManageClosed thisClosed;
        bool isRegister = false;
        string patSearchMrnText = "";
        public bool pat_search = false;
        public bool Emr_Load = false;
        List<DateTime> visitDate = new List<DateTime>();
        public delegate void VisitDoubleClick(string a, EventArgs e);
        public event VisitDoubleClick _visitDoubleClick;
        int image_count = 0;
        int newScrollValue = 0;
        #region paginationvariables
        private int currentPage = 1;

        public int CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value;
            pageNum_nud.Value = currentPage;
            }
        }
        int pagesCount;

        public int PagesCount
        {
            get { return pagesCount; }
            set 
            {
                pagesCount = value;
                RebindGridForPageChange();
            }
        }
        int pageRows;
        int NoofPageButton = 5;
        int NoofPageToBeShifted = 1;
        int CurrentPageDecementNumber = 2;
        int PageCountDecrementNumber = 4;
        #endregion
        List<int> ReportNumbers = new List<int>();
        List<int> AIReportNumbers = new List<int>();
        List<int> ImageNumbers = new List<int>();
        string patFirstNameSearchText = "";
        string patLastNameSearchText = "";
        public int corrupted_count = 0;
        int genderSelectedIndx = 0;
        int currentRow = 0;
        int visitId = 0;
        Dictionary<string, object> searchDictionary;
        bool isTab = false;
        bool isEnter = false;
        bool isControlkey_clicked = false;
        AdvanceSerach_UC advanceUC;
        #endregion

        public EmrManage()
        {
            InitializeComponent();
            isReport = false;
            _eventHandler = IVLEventHandler.getInstance();
            _eventHandler.Register(_eventHandler.Back2Search, new NotificationHandler(Back2Search));
            _eventHandler.Register(_eventHandler.SetLeftRightDetailsToDb, new NotificationHandler(setLeftRight2ImageDb));
            this.PatSearchMrn_tbx.CharacterCasing = CharacterCasing.Upper;
            this.PatSearchFirstName_tbx.CharacterCasing = CharacterCasing.Upper;
            this.PatSearchLastName_tbx.CharacterCasing = CharacterCasing.Upper;
            this.diagnosis_tbx.CharacterCasing = CharacterCasing.Upper;
            NewVisit_Btn.ForeColor = Color.Black;
            stW = new System.Diagnostics.Stopwatch();
            this.InitializeResourceString();

            patDetList = new List<string>();

           //for (int i = 0; i < TemPatList.Count; i++)
           //{
           //    //INTUSOFT.Data.NewDbModel.Patient p = TemPatList[i];
           //    //NewDataVariables.Patients[i] = Patient.CreatePatient(TemPatList[i]);
           //    INTUSOFT.Data.NewDbModel.Patient p = Patient.CreatePatient(TemPatList[i]);

           //    //TemPatList[i] = p.Actual ;
           //}
            searchDictionary = new Dictionary<string, object>();
            t.Interval = 1000;
            // AdvanceSearch_grpbx.Visible = false;
            this.InitializeDropDownList();
            this.InitilizeDataGridViewStyle();
            visitsView_dgv.ScrollBars = ScrollBars.Vertical;
            this.visitsView_dgv.ForeColor = Color.Black;
            this.visitsView_dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9, FontStyle.Bold, GraphicsUnit.Point);//This code has been added to change the font of header field text of the vist grid to Bold. 
            this.visitsView_dgv.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Regular, GraphicsUnit.Point);//This line was added by darshan to solve the resizing of the view data grid view font.
            PatientDetails_panel.Visible = false;
            //This below code has been added by Darshan on 08-09-2015 create a single instance of the advancesearchevent.
            advanceUC = AdvanceSerach_UC.getInstance();
            advanceUC.advancesearchevent += advanceUC_advancesearchevent;
            cf = new CreatePatientDetails_form();// this line is added by sriram in order to create patient details dialog in constructor only
            PatientCRUD_ts.Renderer = new INTUSOFT.Custom.Controls.FormToolStripRenderer();
            UpdateGridView();
            UpdateControlsForCurrentResolution();
            toolStripPaging.Renderer = new FormToolStripRenderer();
           

            //this.Refresh();
        }
        /// <summary>
        /// This event will get the list of patients from advance search window. 
        /// </summary>
        /// <param name="patients">List of patients</param>
        void advanceUC_advancesearchevent(AdvanceSearchParams @params)
        {
            search(@params);
        }


        private void CreateArrowSymbolForPageInit()
        {
            //The below code is to draw a triangle in the page number toolstripbutton
            Bitmap b = new Bitmap(40, 40);
            SolidBrush solidBrush = new SolidBrush(IVLVariables.GradientColorValues.FontForeColor);
            Graphics g = Graphics.FromImage(b);
            g.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, b.Width, b.Height));
            g.DrawPolygon(new Pen(solidBrush, 5f), new Point[] { new Point(b.Width, 1), new Point(b.Width, b.Height - 2), new Point(2, b.Height / 2) });
            g.FillPolygon(solidBrush, new Point[] { new Point(b.Width, 1), new Point(b.Width, b.Height - 2), new Point(2, b.Height / 2) });
            btnBackward.Image = b;//this is for btnFirst button
            Bitmap b1 = b.Clone() as Bitmap;
            b1.RotateFlip(RotateFlipType.Rotate180FlipY);//rotating the image so as to get the btnLast button image
            btnForward.Image = b1;//this is for btnLast button.

        }


        #region public methods
        /// <summary>
        /// This function  will unselect the patient selected.
        /// </summary>
        public void Unselect()
        {
            if(patGridView_dgv.Rows.Count > 1)
                patGridView_dgv.Rows[0].Selected = false;
            clearVisitGrid();
            PatientDetails_panel.Visible = false;
            //This below code is added by Darshan on 17-08-2015  to resolve Defect no 0000561: Clicked on new patient after deselecting patients in grid,highlighting must happen to first patient
            //currentIndx = 0;
            if (currentIndx >= 0)
                currentIndx = -1;
            //PatSearchMrn_tbx.Focus();
        }

        private void UpdateFontForeColor()
        {
            List<Control> controls = GetControls(this).ToList();
            foreach (Control c in controls)
            {
                if (c is ToolStrip)
                {
                    ToolStrip l = c as ToolStrip;
                    if (l.Name == PatientCRUD_ts.Name || l.Name == AllPatients_ts.Name || l.Name == Visits_ts.Name || l.Name == Consultation_ts.Name || c.Name == toolStripPaging.Name)
                    {
                        for (int i = 0; i < l.Items.Count; i++)
                        {
                            l.Items[i].ForeColor = IVLVariables.GradientColorValues.FontForeColor;
                            l.Refresh();
                        }

                    }
                }
                if (c.Name != PatEditGender_cmbx.Name)
                {
                    c.ForeColor = IVLVariables.GradientColorValues.FontForeColor;
                    c.Refresh();
                }
                if (c is NumericUpDown)
                    c.ForeColor = Color.Black;
                if (c is TextBox)
                {
                    c.ForeColor = Color.Black;
                    c.Refresh();
                }

            }
            visitsView_dgv.ForeColor = Color.Black;
            PatEditGender_cmbx.ForeColor = Color.Black;
            advanceUC.UpdateFontForeColor();
            if(patd != null)
                patd.UpdateFontForeColor();
        }


        public void UpdateControlsForCurrentResolution()
        {
            List<Control> controls = GetControls(this).ToList();
            foreach (Control c in controls)
            {
                if (c is Label)
                {
                    Label l = c as Label;
                    if (Screen.PrimaryScreen.Bounds.Width == 1920)
                    {
                        //This below code has been modified by Darshan on 17-08-2015 to resolve Defect no 0000536: Check the Alignment and the Boldness of characters in Highlighted areas.
                        l.Font = new Font(l.Font.FontFamily.Name, 11f);
                        if (l.Name.Equals(allPatient_Image.Name))
                            l.Font = new Font(l.Font.FontFamily.Name, 11f, FontStyle.Bold);
                        else if (l.Name.Equals(Visits_lbl.Name))
                            l.Font = new Font(l.Font.FontFamily.Name, 11f, FontStyle.Bold);
                        else if (l.Name.Equals(mrn_lbl.Name))
                            l.Font = new Font(l.Font.FontFamily.Name, 11f, FontStyle.Bold);
                        else if (l.Name.Equals(firstName_lbl.Name))
                            l.Font = new Font(l.Font.FontFamily.Name, 11f, FontStyle.Bold);
                        else if (l.Name.Equals(lastName_lbl.Name))
                            l.Font = new Font(l.Font.FontFamily.Name, 11f, FontStyle.Bold);
                        else if (l.Name.Equals(gender_lbl.Name))
                            l.Font = new Font(l.Font.FontFamily.Name, 11f, FontStyle.Bold);
                        else if (l.Name.Equals(diagnosis_lbl.Name))
                            l.Font = new Font(l.Font.FontFamily.Name, 11f, FontStyle.Bold);
                        else if (l.Name.Equals(totlPages_lbl.Name) || l.Name.Equals(pageNum_lbl.Name))
                            l.Font = new Font(l.Font.FontFamily.Name, 10f, FontStyle.Bold);
                        else
                            l.Font = new Font(l.Font.FontFamily.Name, 11f);
                    }
                    else if (Screen.PrimaryScreen.Bounds.Width == 1366)
                    {
                        //This below code has been modified by Darshan on 17-08-2015 to resolve Defect no 0000536: Check the Alignment and the Boldness of characters in Highlighted areas.
                        if (l.Name.Equals(allPatient_Image.Name))
                            l.Font = new Font(l.Font.FontFamily.Name, 10f, FontStyle.Bold);
                        else if (l.Name.Equals(Visits_lbl.Name))
                            l.Font = new Font(l.Font.FontFamily.Name, 10f, FontStyle.Bold);
                        else if (l.Name.Equals(mrn_lbl.Name))
                            l.Font = new Font(l.Font.FontFamily.Name, 10f, FontStyle.Bold);
                        else if (l.Name.Equals(firstName_lbl.Name))
                            l.Font = new Font(l.Font.FontFamily.Name, 10f, FontStyle.Bold);
                        else if (l.Name.Equals(lastName_lbl.Name))
                            l.Font = new Font(l.Font.FontFamily.Name, 10f, FontStyle.Bold);
                        else if (l.Name.Equals(gender_lbl.Name))
                            l.Font = new Font(l.Font.FontFamily.Name, 10f, FontStyle.Bold);
                        else if (l.Name.Equals(diagnosis_lbl.Name))
                            l.Font = new Font(l.Font.FontFamily.Name, 10f, FontStyle.Bold);
                        else if (l.Name.Equals(totlPages_lbl.Name) || l.Name.Equals(pageNum_lbl.Name))
                            l.Font = new Font(l.Font.FontFamily.Name, 9f, FontStyle.Bold);
                        else
                            l.Font = new Font(l.Font.FontFamily.Name, 10f);
                    }
                    else if (Screen.PrimaryScreen.Bounds.Width == 1280)
                    {
                        //This below code has been modified by Darshan on 17-08-2015 to resolve Defect no 0000536: Check the Alignment and the Boldness of characters in Highlighted areas.
                        if (l.Name.Equals(allPatient_Image.Name))
                            l.Font = new Font(l.Font.FontFamily.Name, 10f, FontStyle.Bold);
                        else if (l.Name.Equals(Visits_lbl.Name))
                            l.Font = new Font(l.Font.FontFamily.Name, 10f, FontStyle.Bold);
                        else if (l.Name.Equals(mrn_lbl.Name))
                            l.Font = new Font(l.Font.FontFamily.Name, 10f, FontStyle.Bold);
                        else if (l.Name.Equals(firstName_lbl.Name))
                            l.Font = new Font(l.Font.FontFamily.Name, 10f, FontStyle.Bold);
                        else if (l.Name.Equals(lastName_lbl.Name))
                            l.Font = new Font(l.Font.FontFamily.Name, 10f, FontStyle.Bold);
                        else if (l.Name.Equals(gender_lbl.Name))
                            l.Font = new Font(l.Font.FontFamily.Name, 10f, FontStyle.Bold);
                        else if (l.Name.Equals(diagnosis_lbl.Name))
                            l.Font = new Font(l.Font.FontFamily.Name, 10f, FontStyle.Bold);
                        else
                            l.Font = new Font(l.Font.FontFamily.Name, 10f);
                    }
                }
                if (c is NumericUpDown)
                {
                    if (Screen.PrimaryScreen.Bounds.Width == 1366)
                        pageNum_nud.Margin = new Padding(3, 7, 3, 3);
                }
                if (c is GroupBox)
                {
                    if (Screen.PrimaryScreen.Bounds.Height == 768)
                    {
                        groupBox1.Height = 200;
                    }
                }
                if (c is ToolStrip)
                {
                    //the belowcode is for aligning the page number toolstrip.
                    if (c.Name == toolStripPaging.Name)
                    {
                        if (Screen.PrimaryScreen.Bounds.Height == 1080)
                        {
                            RefreshPagination();
                            if (pageCount < 9)
                            {
                                for (int i = 0; i < toolStripPaging.Items.Count; i++)
                                {
                                    toolStripPaging.Items[i].Size = new Size((toolStripPaging.Width / (4 + pageCount)), toolStripPaging.Items[i].Size.Height);

                                }
                            }
                            btnLast.Font = new Font(c.Font.FontFamily, 12.5f);
                            btnFirst.Font = new Font(c.Font.FontFamily, 12.5f);
                        }
                    }

                    
                }
                
            }
           
        }
        /// <summary>
        /// This function will show details of the patient selected in patient DataGridView.
        /// </summary>
        public void ShowIndividualPatDetails()
        {
            try
            {
                
                if (patGridView_dgv.SelectedRows.Count > 0)
                {
                    currentRow = patGridView_dgv.SelectedRows[0].Index;
                    {
                        //SetCreateUpdatePatientView();
                        //createPatientUC.setPatDetails(currentPat);
                        //if (Screen.PrimaryScreen.Bounds.Height == 728)
                        //{
                        //    PatDetails_Grpbx.Height = 365;
                        //    pat.Height = 350;
                        //}
                        //PatientsGroupBox_p.Controls.Remove(pat);
                        PatientDetails_panel.Visible = true;
                       
                        //checkForDeletedObservations();
                        //old implementation
                        //if (DataVariables.Active_Patient.visits != null)
                        //{
                        //    DataVariables.Visits = DataVariables.Active_Patient.visits.Where(x => x.HideShowRow == false).ToList(); //  DataVariables._visitViewRepo.GetByPatID(Convert.ToInt32(patGridView_dgv.SelectedRows[0].Cells["ID"].Value.ToString())).ToList().OrderByDescending(x => x.VisitDateTime < DateTime.Now).Reverse().ToList();
                        //}
                        //else
                        // DataVariables.Visits = new List<VisitModel>();
                        AddPatDetails();
                        visitDate.Clear();
                        foreach (var item in NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].visits.Where(x => x.voided == false).OrderByDescending(x => x.createdDate))
                        {
                            visitDate.Add(item.createdDate);
                        }
                        visitColumnsVisible = false;
                        DataTable d = NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].visits.Where(x => x.voided == false).OrderByDescending(x => x.createdDate).ToList().ToDataTable();
                        SetVisitData(d);
                        this.LoadDataGridView(d, visitsView_dgv);
                        SetVisitColumnVisibilty();
                        //if (visitsView_dgv.Rows.Count > 0)
                        //    visitsView_dgv.Rows[0].Selected = true;
                        Args arg = new Args();
                        //List<patient_identifier> identifier = NewDataVariables._Repo.GetByCategory<patient_identifier>("patient", NewDataVariables.Active_Patient).ToList();
                        List<patient_identifier> identifier = NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].identifiers.ToList();// NewDataVariables._Repo.GetByCategory<patient_identifier>("patient", NewDataVariables.Active_Patient).ToList();
                        arg["MRN"] = NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].identifiers.ToList()[0].value;
                        arg["FirstName"] = NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].firstName;
                        arg["LastName"] = NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].lastName;
                        arg["Age"] = (DateTime.Now.Year - NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].birthdate.Year).ToString();
                        if (NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].gender == 'M')
                            arg["Gender"] = "Male";
                        else
                            arg["Gender"] = "Female";
                        NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].lastAccessedDate = DateTime.Now;
                        NewIVLDataMethods.UpdatePatient();
                        //if (visitsView_dgv.SelectedRows.Count <= 0)
                        //{
                        //    visitColumnsVisible = true;
                        //    LiveImaging_btn.Enabled = false;
                        //    ViewImages_btn.Enabled = false;
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
//                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        public void checkForDeletedObservations()
        {
            try
            {
                //List<obs> obervations = NewDataVariables._Repo.GetByCategory<obs>("person", NewDataVariables.Active_Patient).OrderByDescending(x => x.createdDate < DateTime.Now).ToList();
                List<eye_fundus_image> obervations = NewDataVariables.Active_Visit.observations.OrderByDescending(x=>x.createdDate < DateTime.Now).ToList();// NewDataVariables._Repo.GetByCategory<obs>("person", NewDataVariables.Active_Patient).OrderByDescending(x => x.createdDate < DateTime.Now).ToList();
                foreach (var item in obervations)
                {
                    if (!File.Exists(IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + item.value))
                    {
                        eye_fundus_image delObs = obervations.Where(x => x.value == item.value).ToList<eye_fundus_image>().First();
                        delObs.voided = true;
                        NewDataVariables._Repo.Remove<eye_fundus_image>(delObs);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
//                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }


        private void ToolStripButtonClick(object sender, EventArgs e)
        {
            try
            {
                ToolStripButton ToolStripButton = ((ToolStripButton)sender);
                //Determining the current page
                if (ToolStripButton == btnBackward)
                {
                    CurrentPage--;
                }
                else if (ToolStripButton == btnForward)
                    CurrentPage++;
                else if (ToolStripButton == btnLast)
                    CurrentPage = PagesCount;
                else if (ToolStripButton == btnFirst)
                    CurrentPage = 1;
                else
                    CurrentPage = Convert.ToInt32(ToolStripButton.Text);
                if (CurrentPage > PagesCount)
                    CurrentPage = PagesCount;
                RefreshNumericUpDown();
                if(!pat_search)//has been added to check if it from pat search or not.
                NewDataVariables.Patients = NewDataVariables._Repo.GetPageData<INTUSOFT.Data.NewDbModel.Patient>(Convert.ToInt32(IVLVariables.CurrentSettings.UserSettings._NoOfPatientsToBeSelected.val), CurrentPage).ToList<INTUSOFT.Data.NewDbModel.Patient>();//Get the data from the database of the CurrentPage and saves into the temp list.
                RebindGridForPageChange();//Rebind the Datagridview with the data.
                //updatePatGridView();
                if (patGridView_dgv.Rows.Count > 0)
                {
                    patGridView_dgv.Rows[0].Selected = true;
                    patGridView_dgv.Focus();
                    updatePatGridView();//This method is being invoke to update the DataVariables.Active_Patient details.
                }
                RefreshPagination();//Change the pagiantions buttons according to page number
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Loads the data from the database to the datagridview based on current page.
        /// </summary>
        private void RebindGridForPageChange()
        {
            try
            {
                // Rebinding the Datagridview with data
                //TemPatListlist = NewDataVariables._Repo.GetPageData<INTUSOFT.Data.NewDbModel.Patient>(Convert.ToInt32(IVLVariables.CurrentSettings.UserSettings._NoOfPatientsToBeSelected.val), CurrentPage).ToList<INTUSOFT.Data.NewDbModel.Patient>();//Get the data from the database of the CurrentPage and saves into the temp list.
                //patient_dgv.DataSource = Templist;//Sets the patient_dgv datasource to templist.
                DataTable d = null;
                //if (PagesCount > 0)
                {
                    if (pat_search)
                        d = NewDataVariables.Patients.Skip((CurrentPage - 1) * pageRows).Take(pageRows).ToList().ToDataTable();
                    else
                    {
                        NewDataVariables.Patients = NewDataVariables._Repo.GetPageData<INTUSOFT.Data.NewDbModel.Patient>(pageRows, CurrentPage).ToList<INTUSOFT.Data.NewDbModel.Patient>();
                        d = NewDataVariables.Patients.ToDataTable();
                    }

                    //else if(NewDataVariables.Patients.Count> pageRows)
                    //    d = NewDataVariables.Patients.Skip((CurrentPage - 1) * pageRows).Take(pageRows).ToList().ToDataTable();
                    //else
                    //{
                    //    d = NewDataVariables.Patients.ToDataTable();
                    //}
                    setPatientTableOrder(d);
                    LoadDataGridView(d, patGridView_dgv);
                    SetPatientColumnsVisible();
                    int rowHeight = (int)((float)(patGridView_dgv.Height - patGridView_dgv.ColumnHeadersHeight) / (float)(Convert.ToInt32(IVLVariables.CurrentSettings.UserSettings._NoOfPatientsToBeSelected.val)));
                    if (patGridView_dgv.Rows.Count > 0)
                    {
                        for (int i = 0; i < patGridView_dgv.Rows.Count; i++)
                        {
                            patGridView_dgv.Rows[i].Height = rowHeight;
                        }
                    }
                    patGridView_dgv.Refresh();
                    RefreshPagination();
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
                //                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }


        private void RefreshPagination()
        {
            try
            {
                RefreshNumericUpDown();
                ToolStripButton[] items = new ToolStripButton[] { toolStripButton1, toolStripButton2, toolStripButton3, toolStripButton4, toolStripButton5 };

                //pageStartIndex contains the first button number of pagination.
                int pageStartIndex = 1;
                if (PagesCount > NoofPageButton && CurrentPage > NoofPageToBeShifted)
                    pageStartIndex = CurrentPage - CurrentPageDecementNumber;

                if (PagesCount > NoofPageButton && CurrentPage > PagesCount - NoofPageToBeShifted)
                    pageStartIndex = PagesCount - PageCountDecrementNumber;

                items[0].Text = CurrentPage.ToString();
                items[0].Visible = true;
                items[0].ForeColor = IVLVariables.GradientColorValues.FontForeColor;
                items[0].Font = new Font(items[0].Font.FontFamily, 12f, FontStyle.Bold | FontStyle.Underline);
                //for (int i = pageStartIndex; i < pageStartIndex + NoofPageButton; i++)
                //{
                //    if (i > PagesCount)
                //    {
                //        items[i - pageStartIndex].Visible = false;
                //    }
                //    else
                //    {
                //        //Changing the page numbers
                //        items[i - pageStartIndex].Text = i.ToString();
                //        items[i - pageStartIndex].Visible = true;
                //        items[i - pageStartIndex].ForeColor = IVLVariables.GradientColorValues.FontForeColor;
                //        //Setting the Appearance of the page number buttons
                //        if (i == CurrentPage)
                //        {
                //            items[i - pageStartIndex].Font = new Font(items[i - pageStartIndex].Font.FontFamily, 12f, FontStyle.Bold|FontStyle.Underline);
                //        }
                //        else
                //        {
                //            items[i - pageStartIndex].Font = new Font(items[i - pageStartIndex].Font.FontFamily, 12f);
                //        }
                //    }
                //}
                //for (int i = 0; i < patGridView_dgv.Rows.Count; i++)
                //{
                //    patGridView_dgv.Rows[i].Height = (patGridView_dgv.Height - 50) / Convert.ToInt32(IVLVariables.CurrentSettings.UserSettings._NoOfPatientsToBeSelected.val);
                //}
                //patGridView_dgv.Refresh();
                //Enabling or Disalbing pagination first, last, previous , next buttons
                //switch(CurrentPage)
                //{
                //    case 1: {

                //          btnBackward.Enabled = btnFirst.Enabled = false;
                //    btnBackward.ToolTipText = IVLVariables.LangResourceManager.GetString("BackwardButtonDisable_ToolTipText", IVLVariables.LangResourceCultureInfo);
                //    btnFirst.ToolTipText = IVLVariables.LangResourceManager.GetString("FirstButtonDisable_ToolTipText", IVLVariables.LangResourceCultureInfo);
                //    btnLast.ToolTipText = IVLVariables.LangResourceManager.GetString("LastButtonEnable_ToolTipText", IVLVariables.LangResourceCultureInfo);
                //    btnForward.ToolTipText = IVLVariables.LangResourceManager.GetString("ForwardButtonEnable_ToolTipText", IVLVariables.LangResourceCultureInfo);
                //        break;
                //            }

                //         case 2: {

                //          btnBackward.Enabled = btnFirst.Enabled = false;
                //    btnBackward.ToolTipText = IVLVariables.LangResourceManager.GetString("BackwardButtonDisable_ToolTipText", IVLVariables.LangResourceCultureInfo);
                //    btnFirst.ToolTipText = IVLVariables.LangResourceManager.GetString("FirstButtonDisable_ToolTipText", IVLVariables.LangResourceCultureInfo);
                //    btnLast.ToolTipText = IVLVariables.LangResourceManager.GetString("LastButtonEnable_ToolTipText", IVLVariables.LangResourceCultureInfo);
                //    btnForward.ToolTipText = IVLVariables.LangResourceManager.GetString("ForwardButtonEnable_ToolTipText", IVLVariables.LangResourceCultureInfo);
                //        break;
                //            }
                //}

                int pageInitSwitchCaseValue = 0;
                if (CurrentPage == 1 && CurrentPage == PagesCount)// first page = 1 and current page = pages count
                {
                    pageInitSwitchCaseValue = 1;

                }
                else if (CurrentPage == 1)//current page is 1
                {
                    pageInitSwitchCaseValue = 2;

                }

                else if (CurrentPage == PagesCount && PagesCount > 0)//current page is the last page
                {
                    pageInitSwitchCaseValue = 3;

                }
                else if (PagesCount == 0)
                {
                    pageInitSwitchCaseValue = 4;

                }

                else
                {
                    pageInitSwitchCaseValue = 5;

                }
                pageCount = pageInitSwitchCaseValue;
                EnableDisablePageInitButtonsAndUpdateToolTipTexts(pageInitSwitchCaseValue);

            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
//                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }


        /// <summary>
        /// To enable and disable the buttons in the page initialization tool strip
        /// </summary>
        /// <param name="PageInitCaseValue"></param>
        private void EnableDisablePageInitButtonsAndUpdateToolTipTexts(int PageInitCaseValue)
        {
            switch (PageInitCaseValue)
            {
                case 1:
                    {

                        btnBackward.Enabled = btnFirst.Enabled = false;//first button and backward button enabled is false
                        btnForward.Enabled = btnLast.Enabled = false;//last button and forward button enabled is false
                        btnBackward.ToolTipText = IVLVariables.LangResourceManager.GetString("BackwardButtonDisable_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        btnFirst.ToolTipText = IVLVariables.LangResourceManager.GetString("FirstButtonDisable_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        btnLast.ToolTipText = IVLVariables.LangResourceManager.GetString("LastButtonDisable_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        btnForward.ToolTipText = IVLVariables.LangResourceManager.GetString("ForwardButtonDisable_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        break;
                    }

                case 2:
                    {

                        btnBackward.Enabled = btnFirst.Enabled = false;//first button and backward button enabled is false
                        btnForward.Enabled = btnLast.Enabled = true;//last button and forward button enabled is true
                        btnBackward.ToolTipText = IVLVariables.LangResourceManager.GetString("BackwardButtonDisable_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        btnFirst.ToolTipText = IVLVariables.LangResourceManager.GetString("FirstButtonDisable_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        btnLast.ToolTipText = IVLVariables.LangResourceManager.GetString("LastButtonEnable_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        btnForward.ToolTipText = IVLVariables.LangResourceManager.GetString("ForwardButtonEnable_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        break;
                    }
                case 3:
                    {

                        btnForward.Enabled = btnLast.Enabled = false;//last button and forward button enabled is false
                        btnBackward.Enabled = btnFirst.Enabled = true;//first button and backward button enabled is true
                        btnForward.ToolTipText = IVLVariables.LangResourceManager.GetString("ForwardButtonDisable_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        btnBackward.ToolTipText = IVLVariables.LangResourceManager.GetString("BackwardButtonEnable_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        btnLast.ToolTipText = IVLVariables.LangResourceManager.GetString("LastButtonDisable_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        btnFirst.ToolTipText = IVLVariables.LangResourceManager.GetString("FirstButtonEnable_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        break;
                    }
                case 4:
                    {
                        btnForward.Enabled = btnLast.Enabled = false;//last button and forward button enabled is false
                        btnBackward.Enabled = btnFirst.Enabled = false;//first button and backward button enabled is false
                        btnForward.ToolTipText = IVLVariables.LangResourceManager.GetString("ForwardButtonDisable_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        btnLast.ToolTipText = IVLVariables.LangResourceManager.GetString("LastButtonDisable_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        btnBackward.ToolTipText = IVLVariables.LangResourceManager.GetString("BackwardButtonDisable_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        btnFirst.ToolTipText = IVLVariables.LangResourceManager.GetString("FirstButtonDisable_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        break;
                    }
                case 5:
                    {
                        btnBackward.Enabled = btnFirst.Enabled = true;//first button and backward button enabled is true
                        btnForward.Enabled = btnLast.Enabled = true;//last button and forward button enabled is true
                        btnLast.ToolTipText = IVLVariables.LangResourceManager.GetString("LastButtonEnable_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        btnForward.ToolTipText = IVLVariables.LangResourceManager.GetString("ForwardButtonEnable_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        btnBackward.ToolTipText = IVLVariables.LangResourceManager.GetString("BackwardButtonEnable_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        btnFirst.ToolTipText = IVLVariables.LangResourceManager.GetString("FirstButtonEnable_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        break;
                    }

            }
        }
        //This below function has been added by Darshan for advance searching.
        /// <summary>
        /// This function will get patients from the result obtained in advance search operation.
        /// </summary>
        /// <param name="patients"></param>
        public void Advance_search(List<Patient> patients)
        {
            DataTable d = null;
            List<Patient> pats = new List<Patient>();
            if (patients != null)
            {
                //patients.OrderByDescending(x => x.RegistrationDateTime);
                patients.Sort((x, y) => x.createdDate.CompareTo(y.createdDate));
                patients.Reverse();//This line has been added to stop the patient details from getting into a reverse order.
                NewDataVariables.Patients = patients;
                PagesCount = Convert.ToInt32(Math.Ceiling(patients.Count * 1.0 / pageRows));
                //RefreshNumericUpDown();
                //RebindGridForPageChange();
                //TemPatList = NewDataVariables._Repo.GetPageData<INTUSOFT.Data.NewDbModel.Patient>(pageRows, CurrentPage).ToList<INTUSOFT.Data.NewDbModel.Patient>();
                d = patients.ToDataTable();
            }
            else
            {
                //pats = NewDataVariables.Patients.ToList();//This code in else has been modified to stop the patient details from getting into a reverse order.
                ////pats.OrderByDescending(x => x.RegistrationDateTime);
                ////pats.Sort((x, y) => x.createdDate.CompareTo(y.createdDate));
                //pats.Reverse();
                pageRows = Convert.ToInt32(IVLVariables.CurrentSettings.UserSettings._NoOfPatientsToBeSelected.val);//Assigns no of patients to be displayed
                PagesCount = Convert.ToInt32(Math.Ceiling(NewDataVariables._Repo.GetPatientCount() * 1.0 / pageRows));
                //RefreshNumericUpDown();
                NewDataVariables.Patients = NewDataVariables._Repo.GetPageData<INTUSOFT.Data.NewDbModel.Patient>(pageRows, CurrentPage).ToList<INTUSOFT.Data.NewDbModel.Patient>();
                //RebindGridForPageChange();
                d = NewDataVariables.Patients.ToDataTable();
            }
            pat_search = true;
            RebindGridForPageChange();
            //RefreshPagination();
            Unselect();
            //setPatientTableOrder(d);
            //this.LoadDataGridView(d, patGridView_dgv);
            
            clearVisitGrid();
            SetPatientColumnsVisible();
            PatientDetails_panel.Visible = false;
            t.Stop();
            PatientsGroupBox_p.Controls.Remove(patd);
        }

        /// <summary>
        /// This function will be called to UnRegister the events.
        /// </summary>
        public void UnsubscribleEvents()
        {
            _eventHandler.UnRegister(_eventHandler.Back2Search, new NotificationHandler(Back2Search));
            _eventHandler.UnRegister(_eventHandler.SetLeftRightDetailsToDb, new NotificationHandler(setLeftRight2ImageDb));
        }

        /// <summary>
        /// This function is used to enable imaging button when camera and power is on and disabled when both camera and power is off.
        /// </summary>
        public void UpdateVisitButton()
        {

            try
            {
                if (this.Visible)
                {
                    if (patGridView_dgv.SelectedRows.Count != 0)//this if statement is added by darshan for coloumn visibility of a patient 25-07-2015.
                    {
                        //The below foreach was added by Darshan on 25-08-2015 to stop flickering of visit datagridview.
                        //foreach (DataGridViewRow item in visitsView_dgv.Rows)
                        //{
                        //    string dateVal = item.Cells[IVLVariables.LangResourceManager.GetString("Visit_Date_Text", IVLVariables.LangResourceCultureInfo)].Value.ToString();
                        //    DateTime day = Convert.ToDateTime(dateVal);
                        //    DataGridViewDisableButtonCell disableButtonCell = item.Cells[IVLVariables.LangResourceManager.GetString("VisitsAddImages_ColName", IVLVariables.LangResourceCultureInfo)] as DataGridViewDisableButtonCell;
                        //    if (DateTime.Now.Date == day.Date)
                        //    {
                        //        if (IVLVariables.ivl_Camera.camPropsHelper.IsPowerConnected && IVLVariables.ivl_Camera.camPropsHelper.IsCameraConnected)/* Added by sriram to check camera status using the toupcam camera status instead of microcontoller.*/
                        //        {
                        //            if (!disableButtonCell.Enabled)
                        //            {
                        //                disableButtonCell.Enabled = true;
                        //                visitsView_dgv.Refresh();
                        //            }
                        //        }
                        //        else
                        //        {
                        //            if (disableButtonCell.Enabled)
                        //            {
                        //                disableButtonCell.Enabled = false;
                        //                visitsView_dgv.Refresh();
                        //            }
                        //        }
                        //    }
                        //}
                        NewVisit_Btn.Enabled = true;
                        if (visitsView_dgv.SelectedRows.Count > 0 && visitsView_dgv.SelectedRows[0].Index >= 0)
                        {
                            DeleteConsultation_btn.Enabled = true;
                            DataGridViewRow visitRow = visitsView_dgv.SelectedRows[0];
                            string dateVal = visitRow.Cells[IVLVariables.LangResourceManager.GetString("Visit_Date_Text", IVLVariables.LangResourceCultureInfo)].Value.ToString();
                            int imagesValue = 0;
                            try
                            {
                                string str = visitRow.Cells[IVLVariables.LangResourceManager.GetString("No_of_Images_Text", IVLVariables.LangResourceCultureInfo)].Value.ToString();
                                imagesValue = Convert.ToInt32(str); ;
                            }
                            catch (Exception ex)
                            {
                                
                                throw;
                            }
                            DateTime day = Convert.ToDateTime(dateVal);
                            if (DateTime.Now.Date == day.Date)
                            {
                                if (IVLVariables.ivl_Camera.camPropsHelper.IsPowerConnected == Devices.PowerConnected && IVLVariables.ivl_Camera.camPropsHelper.IsCameraConnected == Devices.CameraConnected)/* Added by sriram to check camera status using the toupcam camera status instead of microcontoller.*/
                                {
                                    if (!LiveImaging_btn.Enabled)
                                    {
                                        LiveImaging_btn.Enabled = true;
                                        LiveImaging_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("LiveImagingEnabled_ToolTipText", IVLVariables.LangResourceCultureInfo);
                                    }
                                }
                                else
                                {
                                    if (LiveImaging_btn.Enabled)
                                    {
                                        LiveImaging_btn.Enabled = false;
                                        LiveImaging_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("LiveImagingDisabled_ToolTipText", IVLVariables.LangResourceCultureInfo);
                                    }
                                }
                            }
                            else
                            {
                                LiveImaging_btn.Enabled = false;
                                LiveImaging_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("LiveImagingDisabled_ToolTipText", IVLVariables.LangResourceCultureInfo);

                            }
                            if (imagesValue != 0)
                            {
                                ViewImages_btn.Enabled = true;
                                ViewImages_btn.ToolTipText = String.Format(IVLVariables.LangResourceManager.GetString("ViewImagesEnabled_ToolTipText", IVLVariables.LangResourceCultureInfo) + " " + imagesValue + " " + IVLVariables.LangResourceManager.GetString("NoOfImagesToBeSelected_Text2", IVLVariables.LangResourceCultureInfo));
                            }
                            else
                            {
                                ViewImages_btn.Enabled = false;
                                ViewImages_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("ViewImagesDisabled_ToolTipText", IVLVariables.LangResourceCultureInfo);
                            }
                        }
                        else
                            DeleteConsultation_btn.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
                //                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        /// <summary>
        /// This function will return list of controls in emrmange form.
        /// </summary>
        /// <param name="form">form name</param>
        /// <returns>List of controls</returns>
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

        #region private methods
        /// <summary>
        /// This function will delete the selected patient.
        /// </summary>
        public void DeletePatient()
        {
            try
            {
                if (patGridView_dgv.SelectedRows.Count != 0)//if a patient is selected 
                {
                    Args arg = new Args();
                    DialogResult val = CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("DeletePatientConfirmation_Text", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("Delete_Button_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxButtons.YesNo, CustomMessageBoxIcon.Question);
                    if (val == DialogResult.Yes)
                    {
                        if (NewDataVariables.Patients[patGridView_dgv.CurrentRow.Index] != null)
                        {
                            //List<VisitModel> visits = DataVariables._visitViewRepo.GetByPatID(Convert.ToInt32(patGridView_dgv.SelectedRows[0].Cells["ID"].Value.ToString())).ToList().OrderByDescending(x => x.VisitDateTime < DateTime.Now).Reverse().ToList();
                            //List<VisitModel> visit = DataVariables._visitViewRepo.GetByPatID(currentPat.ID).Reverse().ToList<VisitModel>();
                            foreach (visit item in NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].visits.Where(x => x.voided == false).OrderByDescending(x => x.createdDate))
                            {
                                item.voided = true;
                                NewDataVariables._Repo.Remove<visit>(item);
                            }
                            //This below code has been Added by Darshan on 31-07-2015 to support advance searching.
                            NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].patientLastModifiedDate = DateTime.Now;
                            NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].date_accessed = DateTime.Now;
                            NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].patientVoidedDate = DateTime.Now;
                            NewDataVariables.Active_PatientIdentifier.lastModifiedDate = DateTime.Now;
                            NewDataVariables.Active_PatientIdentifier.voidedDate = DateTime.Now;
                            NewDataVariables.Active_PersonAddressModel.lastModifiedDate = DateTime.Now;
                            NewDataVariables.Active_PersonAddressModel.date_accessed = DateTime.Now;
                            NewDataVariables.Active_PersonAddressModel.voidedDate = DateTime.Now;
                            NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].voided = true;
                            NewDataVariables.Active_PatientIdentifier.voided = true;
                            NewDataVariables.Active_PersonAddressModel.voided = true;
                            NewIVLDataMethods.RemovePatient();
                            arg["isModified"] = true;
                            arg["isDeletePatient"] = true;
                            if (currentIndx != 0)
                                arg["CurrentIndx"] = currentIndx - 1;
                            else
                                arg["CurrentIndx"] = currentIndx;
                            _eventHandler.Notify(_eventHandler.Back2Search, arg);
                            if (pat_search)//This if statement has been added by Darshan to refresh the search fields after deletion during searching process.
                                ResetSearch();
                        }
                    }
                    else
                        return;
                }
                else //to show message box if no patient is selected and delete button is clicked.
                    CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("Delete_Patient_Select_Patient", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("Patient_Warning_Header", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Warning);

            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
//                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        /// <summary>
        /// This function will reset all controls in search section to their default values.
        /// </summary>
        private void ResetSearch()
        {
            try
            {
                PatSearchFirstName_tbx.Text = "";
                PatSearchMrn_tbx.Text = "";
                PatSearchLastName_tbx.Text = "";
                PatEditGender_cmbx.SelectedIndex = 0;
                diagnosis_tbx.Text = "";
                //Advance_search(null); // this line has been commented as it's coming under any logic by sriram which improves the new patient button click 
                //t.Start();
                if (PagesCount > 0)
                    CurrentPage = 1;//this has been added to set the current page as 1 for resetting the search
                Advance_search(null);//this line has been added by darshan to refresh the patients grid view when reset is clicked after advance search operation.
                pat_search = false;
                if (patGridView_dgv.Rows.Count > 0 && currentIndx == -1)
                {
                    //patGridView_dgv.Rows[currentIndx].Selected = true;
                    patGridView_dgv.Focus();
                    updatePatGridView();//This method is being invoke to update the DataVariables.Active_Patient details.
                    updateVisitGridView();
                }
                currentIndx = 0;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
///                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        /// <summary>
        /// Initializes resource strings
        /// </summary>
        private void InitializeResourceString()
        {
            // Registeration
            // Search, Print, Export, Update, Delete
            CreatePatient_btn.Text = IVLVariables.LangResourceManager.GetString("Registration_Register_Button_Text", IVLVariables.LangResourceCultureInfo);
            gender_lbl.Text = IVLVariables.LangResourceManager.GetString("PatientDetailsGender_Label_Text", IVLVariables.LangResourceCultureInfo);
            mrn_lbl.Text = IVLVariables.LangResourceManager.GetString("Mrn_Label_Text", IVLVariables.LangResourceCultureInfo);
            firstName_lbl.Text = IVLVariables.LangResourceManager.GetString("FirstName_Label_Text", IVLVariables.LangResourceCultureInfo);
            lastName_lbl.Text = IVLVariables.LangResourceManager.GetString("LastName_Label_Text", IVLVariables.LangResourceCultureInfo);
            diagnosis_lbl.Text = IVLVariables.LangResourceManager.GetString("Diagnosis_Lbl_Text", IVLVariables.LangResourceCultureInfo);
            allPatient_Image.Text = IVLVariables.LangResourceManager.GetString("AllPatients_Label_Text", IVLVariables.LangResourceCultureInfo);
            Visits_lbl.Text = IVLVariables.LangResourceManager.GetString("Consultations_Label_Text", IVLVariables.LangResourceCultureInfo);
            NewVisit_Btn.Text = IVLVariables.LangResourceManager.GetString("NewConsultation_Button_Text", IVLVariables.LangResourceCultureInfo);
            ResetSearch_linklbl.Text = IVLVariables.LangResourceManager.GetString("ResetSearch_Button_Text", IVLVariables.LangResourceCultureInfo);
            AdvanceSearch_linklbl.Text = IVLVariables.LangResourceManager.GetString("AdvanceSearch_linklabel_Text", IVLVariables.LangResourceCultureInfo);
            
            DeletePat_btn.Text = IVLVariables.LangResourceManager.GetString("Delete_Patient_Button_Text", IVLVariables.LangResourceCultureInfo);
            Update_btn.Text = IVLVariables.LangResourceManager.GetString("Update_Button_Text", IVLVariables.LangResourceCultureInfo);
            groupBox1.Text = IVLVariables.LangResourceManager.GetString("Search_Search_Button_Text", IVLVariables.LangResourceCultureInfo);
            DeleteConsultation_btn.Text = IVLVariables.LangResourceManager.GetString("Delete_Patient_Button_Text", IVLVariables.LangResourceCultureInfo);
            DeleteConsultation_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("Delete_Button_Text", IVLVariables.LangResourceCultureInfo);

            AllPatients_ts.Renderer = new Custom.Controls.FormToolStripRenderer();
            PatientCRUD_ts.Renderer = new Custom.Controls.FormToolStripRenderer();
            Visits_ts.Renderer = new Custom.Controls.FormToolStripRenderer();
            Consultation_ts.Renderer = new Custom.Controls.FormToolStripRenderer();

            ToolTip patLastName = new ToolTip();
            patLastName.SetToolTip(PatSearchLastName_tbx, IVLVariables.LangResourceManager.GetString("PatLastNameSearch_ToolTipText", IVLVariables.LangResourceCultureInfo));
            ToolTip patFirstName = new ToolTip();
            patFirstName.SetToolTip(PatSearchFirstName_tbx, IVLVariables.LangResourceManager.GetString("PatFirstNameSearch_ToolTipText", IVLVariables.LangResourceCultureInfo));
            ToolTip patMRN = new ToolTip();
            patMRN.SetToolTip(PatSearchMrn_tbx, IVLVariables.LangResourceManager.GetString("PatMRNSearch_ToolTipText", IVLVariables.LangResourceCultureInfo));


            ToolTip newConsultationBtn = new ToolTip();
            NewVisit_Btn.ToolTipText = (IVLVariables.LangResourceManager.GetString("NewConsultationButton_ToolTipText", IVLVariables.LangResourceCultureInfo));
            ToolTip updateBtn = new ToolTip();
            Update_btn.ToolTipText= IVLVariables.LangResourceManager.GetString("ModifyPatDetailsButton_ToolTipText", IVLVariables.LangResourceCultureInfo);
            ToolTip deletePatDetailsBtn = new ToolTip();
            DeletePat_btn.ToolTipText = (IVLVariables.LangResourceManager.GetString("DeletePatDetailsButton_ToolTipText", IVLVariables.LangResourceCultureInfo));
            
            allPatient_lbl.Text = IVLVariables.LangResourceManager.GetString("AllPatients_Label_Text", IVLVariables.LangResourceCultureInfo);

            ToolTip advSearchLinklbl = new ToolTip();
            advSearchLinklbl.SetToolTip(AdvanceSearch_linklbl, IVLVariables.LangResourceManager.GetString("AdvanceSearchLinklabel_ToolTipText", IVLVariables.LangResourceCultureInfo));
            ToolTip createPatBtn = new ToolTip();
            CreatePatient_btn.ToolTipText = (IVLVariables.LangResourceManager.GetString("CreatePatientButton_ToolTipText", IVLVariables.LangResourceCultureInfo));
            ToolTip resetSearchLinklbl = new ToolTip();
            resetSearchLinklbl.SetToolTip(ResetSearch_linklbl, IVLVariables.LangResourceManager.GetString("ResetSearchLinklabel_ToolTipText", IVLVariables.LangResourceCultureInfo));

            LiveImaging_btn.Text = IVLVariables.LangResourceManager.GetString("Imaging_btn_Text", IVLVariables.LangResourceCultureInfo);
            ViewImages_btn.Text = IVLVariables.LangResourceManager.GetString("ViewVisitImages_Button_Text", IVLVariables.LangResourceCultureInfo);
        }

        /// <summary>
        /// This function used to search patients based on details entered in serach section.
        /// </summary>
        private void search(AdvanceSearchParams @params = null)
        {
            try
            {
                if (this.Visible && NewDataVariables._Repo != null )
                {
                    Patient p = Patient.CreateNewPatient();
                    p.firstName = PatSearchFirstName_tbx.Text;
                    p.lastName = PatSearchLastName_tbx.Text;
                    patient_identifier pi = patient_identifier.CreateNewPatientIdentifier();
                    PatientDiagnosis pd = PatientDiagnosis.CreateNewDiagnosis();
                    pi.value = PatSearchMrn_tbx.Text;
                    pd.diagnosisValueLeft = diagnosis_tbx.Text;
                    pd.diagnosisValueRight = diagnosis_tbx.Text;
                    if (searchDictionary.Count > 0)
                        searchDictionary.Clear();
                    if (!string.IsNullOrEmpty(pi.value))
                    {
                        if (p.identifiers == null)
                        {
                            p.identifiers = new HashSet<patient_identifier>();
                        }
                        p.identifiers.Add(pi);
                    }
                    if (!string.IsNullOrEmpty(pd.diagnosisValueRight))
                    {
                        if (p.diagnosis == null)
                        {
                            p.diagnosis = new HashSet<PatientDiagnosis>();
                        }
                        p.diagnosis.Add(pd);
                    }
                    pat_search = true;
                    if (PatEditGender_cmbx.SelectedValue.ToString().Equals(INTUSOFT.Data.Common.GetDescription(GenderEnum.Not_Selected)))
                        p.gender = '\0';
                    else
                    {
                        if (PatEditGender_cmbx.SelectedValue.ToString() == GenderEnum.Male.ToString())
                            p.gender = 'M';
                        else
                            p.gender = 'F';
                    }
                    List<patient_identifier> patindefier = new List<patient_identifier>();
                    List<Patient> pats = new List<Patient>();
                    if (string.IsNullOrEmpty(p.firstName) && string.IsNullOrEmpty(p.lastName) && p.gender == '\0' && string.IsNullOrEmpty(pi.value)&& string.IsNullOrEmpty(pd.diagnosisValueLeft) && @params == null)
                    {
                        
                        //pats = NewDataVariables.Patients.ToList();
                        NewDataVariables.Patients = NewDataVariables._Repo.GetPageData<INTUSOFT.Data.NewDbModel.Patient>(pageRows, CurrentPage).ToList<INTUSOFT.Data.NewDbModel.Patient>();
                        pat_search = false; //has been added to note that it is searching all the patients
                        pats = NewDataVariables.Patients;
                        PagesCount = Convert.ToInt32(Math.Ceiling(NewDataVariables._Repo.GetPatientCount() * 1.0 / pageRows));
                        //RefreshNumericUpDown();
                    }
                    else
                    {
                        //pats = NewDataVariables._Repo.Search(p).ToList<Patient>();//Old Implementation
                        
                        if (!string.IsNullOrEmpty(p.firstName))
                        {
                            //sr = new SimpleExpression("firstName",p.firstName,
                            searchDictionary.Add("firstName",Restrictions.Like(SearchItems.firstName.ToString(), p.firstName, MatchMode.Anywhere));
                        }
                        if (!string.IsNullOrEmpty(p.lastName))
                            searchDictionary.Add("lastName",Restrictions.Like( SearchItems.lastName.ToString(), p.lastName, MatchMode.Anywhere));
                        if (p.gender != '\0')
                            searchDictionary.Add("gender",Restrictions.Eq(SearchItems.gender.ToString(), p.gender));
                        if (!string.IsNullOrEmpty(pi.value))
                            searchDictionary.Add("MRN",Restrictions.Like(SearchItems.identifiers_value.ToString().Replace('_', '.'), pi.value,MatchMode.Anywhere));
                        if (!string.IsNullOrEmpty(pd.diagnosisValueRight))
                        {
                            SimpleExpression[] sr = new SimpleExpression[3];
                            sr[0] = Restrictions.Like("diagnosis.diagnosisValueLeft", pd.diagnosisValueLeft, MatchMode.Anywhere);
                            sr[1] = Restrictions.Eq("diagnosis.voided", false);
                            sr[2] = Restrictions.Like("diagnosis.diagnosisValueRight", pd.diagnosisValueRight, MatchMode.Anywhere);
                            //criteria.Add();
                            //criteria.Add(Restrictions.Like("diagnosisValueRight",pd.diagnosisValueRight,MatchMode.Anywhere));
                            //AbstractCriterion leftDiagnosisRestriction =   Restrictions.On<Patient>(e=>e.diagnosis.ToList().Where(x=>x.diagnosisValueLeft.Contains(pd.diagnosisValueLeft));
                            //searchDictionary.Add("diagnosisL" , leftDiagnosisRestriction);
                            searchDictionary.Add("criteria_patient.diagnosis", sr);
                            //AbstractCriterion rightDiagnosisRestriction =   Restrictions.On<PatientDiagnosis>(e=>e.diagnosisValueRight).IsLike(string.Format("%{0}%",pd.diagnosisValueRight));
                            //searchDictionary.Add("diagnosisR" ,rightDiagnosisRestriction );
                       
                            //searchDictionary.Add("diagnosis." + SearchItems.diagnosisValueLeft.ToString(), pd.diagnosisValueLeft);
                            //searchDictionary.Add("diagnosis." + SearchItems.diagnosisValueRight.ToString(), pd.diagnosisValueRight);
                        }
                        if (@params != null)
                        {
                            SimpleExpression[] sr = new SimpleExpression[2];
                            sr[0] = Restrictions.Le(@params.searchType.ToString(), @params.toDate);
                            sr[1] = Restrictions.Ge(@params.searchType.ToString(), @params.fromDate);
                            //criteria.Add();
                            //criteria.Add(Restrictions.Like("diagnosisValueRight",pd.diagnosisValueRight,MatchMode.Anywhere));
                            //AbstractCriterion leftDiagnosisRestriction =   Restrictions.On<Patient>(e=>e.diagnosis.ToList().Where(x=>x.diagnosisValueLeft.Contains(pd.diagnosisValueLeft));
                            //searchDictionary.Add("diagnosisL" , leftDiagnosisRestriction);
                            //searchDictionary.Add("criteria_." + @params.searchType.ToString(), sr);
                            searchDictionary.Add("toDate", sr[0]);
                            searchDictionary.Add("fromDate", sr[1]);
                        }
                        pats = NewDataVariables._Repo.Search<Patient>(searchDictionary, pageRows, 1).ToList<Patient>();
                        NewDataVariables.Patients = pats;
                        PagesCount = Convert.ToInt32(Math.Ceiling(pats.Count * 1.0 / pageRows));
                        //RefreshNumericUpDown();
                    }
                    //DateTime dt = DateTime.Now;
                    //pats = pats.OrderByDescending(x => x.createdDate < DateTime.Now).ToList();
                    //pats.Reverse();
                    //DataTable d = pats.ToDataTable();
                    //setPatientTableOrder(d);
                    //this.LoadDataGridView(d, patGridView_dgv);
                    //RefreshPagination();
                    //RebindGridForPageChange();
                    Unselect();
                    clearVisitGrid();
                    SetPatientColumnsVisible();
                    PatientDetails_panel.Visible = false;
                    PatientsGroupBox_p.Controls.Remove(patd);
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
//                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        /// <summary>
        /// This function will navigate to the view image screen or live imaging screen. 
        /// </summary>
        /// <param name="visitID">gives the visit id</param>
        /// <param name="isImaging">true if camera is conncted else it will be false</param>
        private void GotoImaging(int visitID, bool isImaging)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                //NewDataVariables.Obs = NewDataVariables._Repo.GetByCategory<obs>("person", NewDataVariables.Active_Patient).OrderByDescending(x => x.createdDate < DateTime.Now).ToList();

                //NewDataVariables.Reports = NewDataVariables._Repo.GetByCategory<report>("Patient", NewDataVariables.Active_Patient).OrderByDescending(x => x.createdDate < DateTime.Now).ToList();
                if (visitID > -1)// check the double click is not datagridview header
                {
                    //NewDataVariables.Obs = NewDataVariables.Obs.Where(x => x.voided == false).OrderByDescending(x => x.createdDate < DateTime.Now).ToList();
                    //foreach (obs item in NewDataVariables.Obs)
                    //{
                    //    eye_fundus_image eyeFundusImage = NewDataVariables._Repo.GetById<eye_fundus_image>(item.observationId);
                    //    NewDataVariables.EyeFundusImage.Add(eyeFundusImage);
                    //}
                    //try
                    //{
                    //    NewDataVariables.EyeFundusImage = NewDataVariables.EyeFundusImage.ToList();
                    //}
                    //catch (Exception ex)
                    //{
                    //    string message = ex.Message;
                    //}
                    //List<string> urls = new List<string>();
                    //List<bool> isannotated = new List<bool>();
                    //List<bool> iscdr = new List<bool>();
                    //List<int> side = new List<int>();
                    //List<int> id = new List<int>();
                    List<ThumbnailModule.ThumbnailData> thumbnailList = new List<ThumbnailModule.ThumbnailData>();
                    foreach (var item in NewDataVariables.Obs)
                    {
                        ThumbnailModule.ThumbnailData tData = new ThumbnailModule.ThumbnailData();
                        if (Convert.ToBoolean(IVLVariables.CurrentSettings.ImageStorageSettings.IsMrnFolder.val))
                            tData.fileName = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_PatientIdentifier.value + Path.DirectorySeparatorChar + NewDataVariables.Active_Visit.createdDate.Date.ToString("dd_MM_yyyy") + Path.DirectorySeparatorChar + item.value;
                        else
                            tData.fileName = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + item.value;
                        //urls.Add(IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + item.value);
                        int xV = 0;
                        if (NewDataVariables.Obs.IndexOf(item) == NewDataVariables.Obs.Count - 1)
                        {
                            xV = 0;
                        }
                        tData.id = item.observationId;
                       eye_fundus_image _eyeFundusImage = NewDataVariables.Obs.Find(x => x.observationId == tData.id);
                        //List<eye_fundus_image> eyeFundusList = NewDataVariables.EyeFundusImage.Where(x=>x.eyeFundusImageId == tData.id).ToList() ;
                        // eye_fundus_image _eyeFundusImage = eyeFundusList[0];
                     tData.side =   _eyeFundusImage.eyeSide.Equals('L')? 1 : 0;
                        //if (_eyeFundusImage.eyeSide == 'L')
                        //    tData.side = 1;
                        //else
                        //    tData.side = 0;
                        tData.isAnnotated = _eyeFundusImage.annotationsAvailable;
                        tData.isCDR = _eyeFundusImage.cdrAnnotationAvailable;
                        thumbnailList.Add(tData);
                        //id.Add(item.observationId);
                    }


                    //foreach (var item in NewDataVariables.EyeFundusImage)
                    //{
                    //    obs ob = NewDataVariables._Repo.GetById<obs>(item.eyeFundusImageId);
                    //    if (!ob.voided)
                    //    {
                    //        if (item.eyeSide == 'L')
                    //            side.Add(1);
                    //        else if (item.eyeSide == 'R')
                    //            side.Add(0);
                    //        isannotated.Add(item.annotationsAvailable);
                    //        iscdr.Add(item.cdrAnnotationAvailable);
                    //    }
                    //}
                    Args arg = new Args();
                    arg["isImaging"] = (bool)isImaging;
                    arg["Thumbnails"] = thumbnailList;
                    //arg["Thumbnails"] = urls;
                    //arg["Side"] = side;
                    //arg["id"] = id;
                    //arg["isannotated"] = isannotated;
                    //arg["isCDR"] = iscdr;
                    IVLVariables.MRN = patGridView_dgv.SelectedRows[0].Cells["MRN"].Value.ToString();
                    arg["MRN"] = patGridView_dgv.SelectedRows[0].Cells["MRN"].Value.ToString();
                    arg["FirstName"] = patGridView_dgv.SelectedRows[0].Cells["FirstName"].Value.ToString();
                    arg["LastName"] = patGridView_dgv.SelectedRows[0].Cells["LastName"].Value.ToString();
                    arg["Gender"] = patGridView_dgv.SelectedRows[0].Cells["Gender"].Value.ToString();
                    arg["Age"] = patGridView_dgv.SelectedRows[0].Cells["Age"].Value.ToString();
                    //Patient p =DataVariables._patientRepo.GetById(IVLVariables.ActivePatID);
                    //IVLVariables._patientRepo.Update(p);
                    //if (urls.Count > 0)
                    //    arg["ThumbnailFileName"] = urls[urls.Count - 1];
                    //if (side.Count > 0)
                    //    arg["side"] = side[side.Count - 1];

                    //bool isFilePresent = false;

                    //if (arg.ContainsKey("ThumbnailFileName"))//This if statement is added to check weather all images of a visit exists or not.
                    //{
                    //    for (int i = 0; i < urls.Count; i++)
                    //    {
                    //        isFilePresent = File.Exists(urls[i].ToString());
                    //        if (!isFilePresent)
                    //            break;
                    //    }
                    //}

                    IVLVariables.patName = patGridView_dgv.SelectedRows[0].Cells["FirstName"].Value.ToString() + " " + patGridView_dgv.SelectedRows[0].Cells["LastName"].Value.ToString();
                    IVLVariables.patAge = Convert.ToInt32(patGridView_dgv.SelectedRows[0].Cells["Age"].Value.ToString());
                    IVLVariables.patGender = patGridView_dgv.SelectedRows[0].Cells["Gender"].Value.ToString();
                    //if (isFilePresent || isImaging)
                    {
                        _eventHandler.Notify(_eventHandler.ShowThumbnails, arg);

                        if (NewDataVariables.Obs.Count > 0)
                        {
                            arg["ThumbnailData"] = thumbnailList[thumbnailList.Count - 1];
                           //arg["idval"] = IVLVariables.ActiveImageID = NewDataVariables.EyeFundusImage[NewDataVariables.EyeFundusImage.Count -1].eyeFundusImageId;
                           //arg["ThumbnailFileName"] = thumbnailList[thumbnailList.Count - 1].fileName;
                           //arg["side"] = thumbnailList[thumbnailList.Count - 1].side;

                        }

                        //if (NewDataVariables.Obs.Count > 0)
                        //    _eventHandler.Notify(_eventHandler.ThumbnailSelected, arg);
                        if (!isImaging)
                        {
                            if (NewDataVariables.Obs.Count == 0)
                                return;
                        }
                        _eventHandler.Notify(_eventHandler.SetActivePatDetails, arg);
                        arg["imageCount"] = NewDataVariables.Obs.Count;
                        _eventHandler.Notify(_eventHandler.Navigate2ViewImageScreen, arg);
                    }
                    //else
                    //{
                    //    Common.CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("ImagesInImageRepo_Text", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("NoImageHeader_Text", IVLVariables.LangResourceCultureInfo), Common.CustomMessageBoxIcon.Warning);
                    //}
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
//                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        /// <summary>
        /// This function will delete the selected visit.
        /// </summary>
        public void DeleteConsultation()
        {
            try
            {
                //if (patGridView_dgv.SelectedRows.Count == 0)//if no patient is selected.
                //{
                //    CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("Delete_Select_Patient", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("Conslutation_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Warning);
                //    return;
                //}
                //else if (visitsView_dgv.SelectedRows.Count != 0)//if a visit is selected.
                //{
                //    DialogResult val = CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("DeleteVisitConfirmation_Text", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("DeleteVisitHeader_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxButtons.YesNo, CustomMessageBoxIcon.Question);
                //    if (val == DialogResult.Yes)
                //    {
                //        int selectedRow = visitsView_dgv.SelectedRows[0].Index;
                //        if (selectedRow > 0)
                //            selectedRow = selectedRow - 1;
                //        //List<VisitModel> visit = visits.Where(x=>x.ID ==id  && x.HideShowRow == false).ToList();
                //        //List<VisitModel> visit = DataVariables._visitViewRepo.GetByVistID(Convert.ToInt32(visitsView_dgv.SelectedRows[0].Cells["ID"].Value.ToString())).ToList();
                //        NewDataVariables.Active_Visit.voided = true;
                //        NewDataVariables.Active_Visit.lastModifiedDate = DateTime.Now;
                //        NewDataVariables.Active_Visit.voidedDate = DateTime.Now;
                //        NewDataVariables.Active_Visit.endDateTime = DateTime.Now;
                //        NewIVLDataMethods.RemoveVisit();
                //        NewDataVariables.Active_Patient.patientLastModifiedDate = DateTime.Now;
                //        NewIVLDataMethods.UpdatePatient();
                //        NewDataVariables.Active_PatientIdentifier.lastModifiedDate = DateTime.Now;
                //        NewIVLDataMethods.UpdatePatientIdentifier();
                //        visitDate.Clear();
                //        //NewDataVariables.Active_Patient.visits.Reverse();
                //        DataTable d = NewDataVariables.Active_Patient.visits.Where(x => x.voided == false).ToList().ToDataTable();
                //        foreach (visit v in NewDataVariables.Active_Patient.visits.Where(x=>x.voided==false).ToList().OrderByDescending(x=>x.createdDate))
                //        {
                //            visitDate.Add(v.createdDate);
                //        }
                //        SetVisitData(d);
                //        visitColumnsVisible = false;
                //        this.LoadDataGridView(d, visitsView_dgv);
                //        SetVisitColumnVisibilty();
                //        int row = visitsView_dgv.Rows.Count;
                //        if (selectedRow < row)
                //        {
                //            visitsView_dgv.Rows[selectedRow].Selected = true;
                //            updateVisitGridView();
                //        }
                //        UpdateVisitButton();
                //        // This has been commented in order to fix the defect 0000051 by sriram on march 23rd 2015
                //        //visitsView_dgv.Rows.Clear();
                //    }
                //}
                //else //else to show message box if no visits is selected and delete butto iss clicked.
                //    CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("Delete_Select_Visit", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("Conslutation_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Warning);

            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
//                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        /// <summary>
        /// To generate the error message
        /// </summary>
        /// <param name="error">error message</param>
        private void AddErrorMessage(string error)
        {
            if (this.errorMessage == string.Empty)
            {
                this.errorMessage = IVLVariables.LangResourceManager.GetString("Error_Message_Header", IVLVariables.LangResourceCultureInfo) + "\n\n";
            }
            this.errorMessage += error + "\n";
        }

        /// <summary>
        /// Initializes all dropdown controls
        /// </summary>
        private void InitializeDropDownList()
        {
            GenderEnum[] Gender_Enum_values = Enum.GetValues(typeof(GenderEnum)) as GenderEnum[];
            string[] Gender_description_values = Gender_Enum_values.OfType<object>().Select(o => o.ToString()).ToArray();
            for (int i = 0; i < Gender_Enum_values.Length; i++)
            {
                if (Gender_description_values[i].Contains('_'))
                    Gender_description_values[i] = INTUSOFT.Data.Common.GetDescription(Gender_Enum_values[i]);
            }
            PatEditGender_cmbx.DataSource = Gender_description_values;
        }

        /// <summary>
        /// Initializes all dropdown controls in update section
        /// </summary>
        private void InitializeUpdate()
        {
            GenderEnum[] Gender_Enum_values = Enum.GetValues(typeof(GenderEnum)) as GenderEnum[];
            string[] Gender_description_values = Gender_Enum_values.OfType<object>().Select(o => o.ToString()).ToArray();
            for (int i = 0; i < Gender_Enum_values.Length; i++)
            {
                if (Gender_description_values[i].Contains('_'))
                    Gender_description_values[i] = INTUSOFT.Data.Common.GetDescription(Gender_Enum_values[i]);
            }
            PatEditGender_cmbx.DataSource = Gender_description_values;
        }

        /// <summary>
        /// Resets the update section of manage screen
        /// </summary>
        private void ResetUpdate()
        {
            isRegister = true;
            PatSearchFirstName_tbx.Text = string.Empty;
            PatEditGender_cmbx.SelectedIndex = -1;
            PatSearchMrn_tbx.Text = string.Empty;
            PatSearchLastName_tbx.Text = string.Empty;
        }

        /// <summary>
        /// Method to show general error message on any system level exception
        /// </summary>
        private void ShowErrorMessage(Exception ex)
        {
            CustomMessageBox.Show(
                ex.Message,
                IVLVariables.LangResourceManager.GetString("System_Error_Message_Title", IVLVariables.LangResourceCultureInfo),
                CustomMessageBoxButtons.OK,
                CustomMessageBoxIcon.Error);
        }

        /// <summary>
        /// Setting the style of the DataGridView control.
        /// </summary>
        private void InitilizeDataGridViewStyle()
        {
            patGridView_dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 11, FontStyle.Bold, GraphicsUnit.Point);
            patGridView_dgv.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.ControlDark;
            patGridView_dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            patGridView_dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            patGridView_dgv.DefaultCellStyle.Font = new Font("Tahoma", 12, FontStyle.Regular, GraphicsUnit.Point);
            patGridView_dgv.DefaultCellStyle.BackColor = Color.Empty;
            patGridView_dgv.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.Info;
            patGridView_dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            patGridView_dgv.GridColor = SystemColors.ControlDarkDark;
        }

        /// <summary>
        /// Method to load data grid view
        /// </summary>
        /// <param name="data">data table</param>
        private void LoadDataGridView(DataTable data, DataGridView d)
        {
            try
            {
                d.DataSource = data;
                int count = patGridView_dgv.Columns.Count;
                foreach (DataGridViewColumn c in d.Columns)
                {
                    c.DefaultCellStyle.Font = new Font("Tahoma", 13.5F, GraphicsUnit.Pixel);
                }
                d.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                //visitColumnsVisible = true;
                //if (d.Rows.Count > 0)
                //    d.Rows[0].Selected = true;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
//                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        /// <summary>
        /// This function will clear the visit datagridview.
        /// </summary>
        private void clearVisitGrid()
        {
            {
                DataTable DT = (DataTable)visitsView_dgv.DataSource;
                if (DT != null)
                {
                    visitColumnsVisible = false;
                    DT.Clear();
                    updateVisitGridView();//added to update visits grid view if no patients were selected.
                }
            }
        }

        /// <summary>
        /// This function will clear the patient datagridview.
        /// </summary>
        private void clearPatientGrid()
        {
            {
                DataTable DT = (DataTable)patGridView_dgv.DataSource;
                if (DT != null)
                    DT.Clear();
            }
        }

        /// <summary>
        /// This function will set the order of the patient datagridview.
        /// </summary>
        /// <param name="d">Data Table</param>
        private void setPatientTableOrder(DataTable d)
        {
            try
            {
                //List<patient_identifier> patId = NewDataVariables._Repo.GetAll<patient_identifier>().ToList();
                //patId.Reverse();
                d.Columns["identifiers"].ColumnName = "MRN";
                d.Columns["MRN"].SetOrdinal(1);
                d.Columns["firstName"].ColumnName = "FirstName";
                d.Columns["FirstName"].SetOrdinal(2);
                d.Columns["lastName"].ColumnName = "LastName";
                d.Columns["LastName"].SetOrdinal(3);
                DataColumn c = new DataColumn("Age");
                d.Columns.Add(c);
                for (int i = 0; i < d.Rows.Count; i++)
                {
                    DateTime dt = new DateTime();
                    var sysFormat = new[] { "M-d-yyyy", "yy-MM-dd", "dd-MM-yyyy", "dd-MM-yy", "MM-dd-yyyy", "M/d/yyyy", "M/d/yy", "MM/dd/yy", "MM/dd/yyyy", "yy/MM/dd" }
                              .Union(IVLVariables.LangResourceCultureInfo.DateTimeFormat.GetAllDateTimePatterns()).ToArray();
                    string dateTime = d.Rows[i]["birthdate"].ToString();
                    {
                        dt = DateTime.ParseExact(dateTime, sysFormat, IVLVariables.LangResourceCultureInfo, DateTimeStyles.AssumeLocal);
                    }
                    //DateTime dt = Convert.ToDateTime(d.Rows[i]["birthdate"]);
                    d.Rows[i]["Age"] = DateTime.Now.Year - dt.Year;
                }
                d.Columns["Age"].SetOrdinal(4);
                DataColumn gender = new DataColumn("Gender");
                d.Columns.Add(gender);
                for (int i = 0; i < d.Rows.Count; i++)
                {
                    if (Convert.ToChar(d.Rows[i]["gender"]) == 'M')
                        d.Rows[i]["Gender"] = "Male";
                    else
                        d.Rows[i]["Gender"] = "Female";
                }
                d.Columns["Gender"].SetOrdinal(5);
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
//                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        /// <summary>
        /// This function will load the modified data. 
        /// </summary>
        private void loadModifyData()
        {
            //PatSearchMrn_tbx.Text = currentPat.MRN;
            //PatSearchMrn_tbx.Enabled = false;
            //PatSearchFirstName_tbx.Text = currentPat.FirstName;
            //PatSearchLastName_tbx.Text = currentPat.LastName;
        }

        #endregion

        #region private events
        /// <summary>
        /// This event which will invoke  ResetSearch() reset all controls in search section to their default value .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetSearch_linklbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ResetSearch();
        }

        /// <summary>
        /// This event will do nothing when mouse left button is clicked  with control key.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void patGridView_dgv_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Control.ModifierKeys == Keys.Control)
                    isControlkey_clicked = true;
                else
                    isControlkey_clicked = false;
                updateVisitGridView();
            }
        }

        //The below code is added to resolve Defect no 0000071.
        /// <summary>
        /// This event will show a tool tip when mouse is hovered on mrn_lbl.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mrn_lbl_MouseHover(object sender, EventArgs e)
        {
            ToolTip too1 = new ToolTip();
            too1.SetToolTip(mrn_lbl, IVLVariables.LangResourceManager.GetString("MrnToolTip_lbl", IVLVariables.LangResourceCultureInfo));
        }

        /// <summary>
        /// This event will allow only charaters in PatEditLastName_tbx textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PatEditLastName_tbx_keyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsPunctuation(e.KeyChar) && !char.IsSymbol(e.KeyChar))
                e.Handled = false;
            else
                if (char.IsControl(e.KeyChar))
                    e.Handled = false;
                else
                    e.Handled = true;
        }

        /// <summary>
        /// This event which will delete the selected visit when delete key is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void visitsView_dgv_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Delete)
            //{
            //    if (visitsView_dgv.SelectedRows.Count > 0)
            //    {
            //        //This below code has been changed by darshan on 30-05-2016.
            //        DialogResult val = CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("DeleteVisitConfirmation_Text", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("DeleteVisitHeader_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxButtons.YesNo, CustomMessageBoxIcon.Question);
            //        if (val == DialogResult.Yes)
            //        {
            //            DeleteConsultation();
            //        }
            //        else if (val == DialogResult.No)
            //        {
            //            return;
            //        }
            //    }
            //}
            if (e.KeyCode == Keys.Space)
            {
                if (visitsView_dgv.SelectedRows.Count == 0)
                    return;
            }
            //if (e.KeyCode == Keys.Enter || e.KeyCode==Keys.Down || e.KeyCode ==Keys.Up ||e.KeyCode==Keys.Tab)
            //{
            //    e.Handled = true;
            //}
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down)//Code has been added darshan to resolve defect no 0001216 note id 0002577.
            {
                if (visitsView_dgv.SelectedRows.Count > 0)
                {
                    if (visitsView_dgv.SelectedRows[0].Index >= 0 && visitsView_dgv.SelectedRows[0].Index < visitsView_dgv.RowCount - 1)
                    {
                        int currentIndx = visitsView_dgv.SelectedRows[0].Index;
                        visitsView_dgv.Rows[currentIndx + 1].Selected = true;
                        updateVisitGridView();
                    }
                }
            }
            if (e.KeyCode == Keys.Up)//Code has been added darshan to resolve defect no 0001216 note id 0002577.
            {
                if (visitsView_dgv.SelectedRows.Count > 0)
                {
                    if (visitsView_dgv.SelectedRows[0].Index < visitsView_dgv.Rows.Count && visitsView_dgv.SelectedRows[0].Index > 0)
                    {
                        int currentIndx = visitsView_dgv.SelectedRows[0].Index;
                        visitsView_dgv.Rows[currentIndx - 1].Selected = true;
                        updateVisitGridView();
                    }
                }
            }
            if (e.KeyCode == Keys.Tab)//Code has been added darshan to resolve defect no 0001216 note id 0002577.
            {
                if (visitsView_dgv.SelectedRows.Count > 0)
                {
                    int count = 0;
                    DataGridViewCell currentCell = visitsView_dgv.CurrentCell;
                    int nextRow = currentCell.RowIndex;
                    int nextCol = currentCell.ColumnIndex + 1;
                    for (int i = 0; i < visitsView_dgv.Columns.Count; i++)
                    {
                        if (visitsView_dgv.Columns[i].Visible)
                            count++;
                    }
                    if (count == 6)
                    {
                        nextCol = 0;
                        nextRow++;
                    }
                    if (nextRow == visitsView_dgv.RowCount)
                    {
                        nextRow = 0;
                    }
                    if (currentCell.ColumnIndex == 25)
                    {
                        visitsView_dgv.Rows[nextRow].Selected = true;
                        updateVisitGridView();
                    }
                }
            }
        }

        public void UpdateGridView()
        {
            //patGridView_dgv.BackgroundColor = IVLVariables.GradientColorValues.Color2;
            //visitsView_dgv.BackgroundColor = IVLVariables.GradientColorValues.Color2;
            cf.Color1 = IVLVariables.GradientColorValues.Color1;
            cf.Color2 = IVLVariables.GradientColorValues.Color2;
            cf.ColorAngle = IVLVariables.GradientColorValues.ColorAngle;
            UpdateFontForeColor();
            CreateArrowSymbolForPageInit();
            //PatientCRUD_ts.BackColor = Color.Transparent;
            //AllPatients_ts.BackColor = Color.Transparent;
            //this.patGridView_dgv.SetCellsTransparent();
            //this.visitsView_dgv.SetCellsTransparent();
        }

        /// <summary>
        /// This method will update the visit data gridview  based on the selected row.
        /// </summary>
        public void updateVisitGridView()
        {
            //Code has been added darshan to resolve defect no 0001216 note id 0002577.
            if (visitsView_dgv.SelectedRows.Count > 0)
            {
                NewVisit_Btn.Enabled = true;
                DeleteConsultation_btn.Enabled = true;
                int id = Convert.ToInt32(visitsView_dgv.SelectedRows[0].Cells["visitId"].Value.ToString());
                NewDataVariables.Active_Visit = NewDataVariables.Visits.Find(x => x.visitId == id);
                UpdateVisitButton();
            }
            else
            {
                //NewVisit_Btn.Enabled = false;
                LiveImaging_btn.Enabled = false;
                ViewImages_btn.Enabled = false;
                ViewImages_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("ViewImagesDisabled_ToolTipText", IVLVariables.LangResourceCultureInfo);
                DeleteConsultation_btn.Enabled = false;
            }
        }

        /// <summary>
        /// This event which will set the text in PatSearchFirstName_tbx to a string variable patFirstNameSearchText for searching purpose.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PatEditFirstName_tbx_TextChanged(object sender, EventArgs e)
        {
            {
               
                patFirstNameSearchText = PatSearchFirstName_tbx.Text;
                search();

            }
        }

        /// <summary>
        /// This event will invoke search() when PatEditMrn_tbx text is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PatEditMrn_tbx_TextChanged(object sender, EventArgs e)
        {
            if (!isUpdate)
            {
                patSearchMrnText = PatSearchMrn_tbx.Text;
                search();
            }
        }

        /// <summary>
        /// This event is used to change the side of the image.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="arg"></param>
        private void setLeftRight2ImageDb(string s, Args arg)
        {
            int id = (int)arg["id"];
            int side = (int)arg["side"];
            eye_fundus_image image = NewDataVariables._Repo.GetById<eye_fundus_image>(id);
            if (image != null)
            {
                if (side == 0)
                    image.eyeSide = 'R';
                else
                    image.eyeSide = 'L';
                NewDataVariables._Repo.Update<eye_fundus_image>(image);
            }
        }

        /// <summary>
        /// This event will open create patient window to create a new patient.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreatePatient_btn_Click(object sender, EventArgs e)
        {
            CreatePatient();
        }

        public void CreatePatient()
        {
            //This below code has been modified by darshan to solve defect no:0000558 When the search is done and clicked on New patient,No action was taken and defect no 0000559: Unselected all patients in the grid,Clicked on new patient,No action taken. Date 13-08-2015
            if (pat_search)
                ResetSearch();
            //This below registry key operation has been added by Darshan to handle scenario when Registry is not present
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Intusoft", true);
            if (key == null)
            {
                key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Intusoft");
                key.SetValue("MRN", (IVLVariables.CurrentSettings.UserSettings._MrnCnt.val).ToString());
            }
            else if (Registry.GetValue(@"HKEY_LOCAL_MACHINE\Software\Intusoft", "MRN", null) == null)
            {
                key.SetValue("MRN", (IVLVariables.CurrentSettings.UserSettings._MrnCnt.val).ToString());
            }
            key.Close();
            if (patGridView_dgv.SelectedRows.Count > 0)
                patGridView_dgv.SelectedRows[0].Selected = false;
            clearVisitGrid();
            PatientsGroupBox_p.Controls.Remove(patd);
            PatientDetails_panel.Visible = false;
            cf.Populate_Details(null, true);
            cf.currentIndx = 0;
            
            //cf.FontColor = IVLVariables.GradientColorValues.FontForeColor;

            if (cf.ShowDialog() == DialogResult.Cancel)
            {
                //Defect 0000583 has been fixed by adding a condition for when no records are present and new patient cancel button is clicked by sriram on August 18th 2015
                if (patGridView_dgv.Rows.Count > 0)
                {
                    patGridView_dgv.Rows[currentIndx].Selected = true;
                    updatePatGridView();
                }
                patGridView_dgv.Focus();
            }
            if (Convert.ToBoolean(IVLVariables.CurrentSettings.ImageStorageSettings.IsMrnFolder.val))
            {
                string path = string.Empty;
                patDetList = new List<string>();
                patDetList.Add("MRN: " + NewDataVariables.Active_PatientIdentifier.value.ToString());
                patDetList.Add("Name: " + NewDataVariables.Active_PatientIdentifier.patient.firstName.ToString() + " " + NewDataVariables.Active_PatientIdentifier.patient.lastName.ToString());
                patDetList.Add("Age: " + (DateTime.Now.Year - NewDataVariables.Active_PatientIdentifier.patient.birthdate.Year).ToString());
                patDetList.Add("Gender: " + NewDataVariables.Active_PatientIdentifier.patient.gender.ToString());
                path = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_PatientIdentifier.value.ToString();
                Directory.CreateDirectory(path);
                Common.PatientDetailsJson.GetInstance().CreatePatientJsonFile(patDetList, path);
            }
        }

       
       
        /// <summary>
        /// Change Image name with requirement
        /// </summary>
        /// <param name="OldImageName">Image saved from camera</param>
        /// <param name="newImageName">Image saved with image name</param>
        private void ChangeImageName(string OldImageName, string newImageName)
        {
            Bitmap bm = new Bitmap(OldImageName);

            try
            {

                ImageSaveFormat imageSaveFormat = (ImageSaveFormat)Enum.Parse(typeof(ImageSaveFormat), IVLVariables.CurrentSettings.ImageStorageSettings._ImageSaveFormat.val.ToLower());
                IVLVariables.ivl_Camera.camPropsHelper.SaveImage2Dir(bm, newImageName, imageSaveFormat, Convert.ToInt32(IVLVariables.CurrentSettings.ImageStorageSettings._compressionRatio.val));//Save the file in the path.
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
//                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }
        /// <summary>
        /// This event will set the patient details when returned back after adding or updating a patient.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="arg"></param>
        private void Back2Search(String s, Args arg)
        {
            try
            {
                //clearVisitGrid();
                PatEditGender_cmbx.SelectedIndex = genderSelectedIndx;
                PatSearchMrn_tbx.Text = patSearchMrnText;
                PatSearchLastName_tbx.Text = patLastNameSearchText;
                PatSearchFirstName_tbx.Text = patFirstNameSearchText;
                if ((bool)arg["isModified"])
                {
                    pageRows = Convert.ToInt32(IVLVariables.CurrentSettings.UserSettings._NoOfPatientsToBeSelected.val);//Assigns no of patients to be displayed
                    //PagesCount = Convert.ToInt32(Math.Ceiling(NewDataVariables._Repo.GetPatientCount() * 1.0 / pageRows));
                    if (PagesCount < CurrentPage)// to check the page count is reduced when patient is deleted
                        CurrentPage = PagesCount;
                    NewDataVariables.Patients = NewDataVariables._Repo.GetPageData<INTUSOFT.Data.NewDbModel.Patient>(pageRows, CurrentPage).ToList<INTUSOFT.Data.NewDbModel.Patient>();
                    //DataTable data = NewDataVariables.Patients.ToDataTable();
                    //prevCntPatCnt = NewDataVariables.Patients.Count;
                    //setPatientTableOrder(data);
                    //this.LoadDataGridView(data, patGridView_dgv);
                    RebindGridForPageChange();
                    RefreshPagination();
                    SetPatientColumnsVisible();
                    int row = patGridView_dgv.Rows.Count;
                    //This below if statement was added by darshan to solve Defect no 0000508: system is getting crashed on 13-08-2015.Disableage
                    if ((int)arg["CurrentIndx"] < row)
                    {
                        patGridView_dgv.Rows[(int)arg["CurrentIndx"]].Selected = true;
                        updatePatGridView();
                        patGridView_dgv.FirstDisplayedScrollingRowIndex = newScrollValue;
                    }
                    else
                    {
                        if ((int)arg["CurrentIndx"] == 0 && row == 0)
                        {
                            PatientDetails_panel.Visible = false;
                            ResetSearch();
                        }
                    }
                    patGridView_dgv.Focus();
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
//                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        /// <summary>
        /// This event is used to set the order of coloums in visit datagridview.
        /// </summary>
        /// <param name="d">Date Table</param>
        private void SetVisitData(DataTable d)
        {
            try
            {
                ReportNumbers = new List<int>();
                ImageNumbers = new List<int>();
                AIReportNumbers = new List<int>();
                int visitDateTimeIndx = d.Columns.IndexOf("createdDate");
                DataColumn time = new DataColumn("Time");
                d.Columns.Add(time);
                for (int i = 0; i < d.Rows.Count; i++)
                {
                    d.Rows[i][visitDateTimeIndx] = visitDate[i].ToString("dd-MMM-yyyy");
                    ///This code has been added by Darshan on 14-08-2015 6:20 PM to solve Defect no 0000553: Time settings are not reflecting correctly.
                    if (Convert.ToBoolean(IVLVariables.CurrentSettings.UserSettings._Is24clock.val))
                    {
                        d.Rows[i]["Time"] = visitDate[i].ToString(" HH:mm ");
                    }
                    else
                    {
                        d.Rows[i]["Time"] = visitDate[i].ToString("hh:mm tt");
                    }
                    int visitId = Convert.ToInt32(d.Rows[i]["visitId"]);
                    //visit visit = NewDataVariables._Repo.GetById<visit>(visitId);
                    visit visit = NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].visits.Where(x => x.voided == false).OrderByDescending(XmlAnyAttributeAttribute => XmlAnyAttributeAttribute.createdDate).ToList()[i];// NewDataVariables._Repo.GetById<visit>(visitId);
                    //ImageNumbers.Add(Convert.ToInt32(NewDataVariables._Repo.GetByCategory<obs>("visit", visit).Where(x => x.voided == false).Count()));
                    ImageNumbers.Add(Convert.ToInt32(NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].visits.Where(x => x.voided == false).OrderByDescending(XmlAnyAttributeAttribute => XmlAnyAttributeAttribute.createdDate).ToList()[i].observations.Where(x => x.voided == false).Count()));
                    //ReportNumbers.Add(Convert.ToInt32(NewDataVariables._Repo.GetByCategory<report>("visit", visit).Count));
                    ReportNumbers.Add(Convert.ToInt32(NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].visits.Where(x => x.voided == false).OrderByDescending(XmlAnyAttributeAttribute => XmlAnyAttributeAttribute.createdDate).ToList()[i].reports.Where(x=>x.voided == false).ToList().Count));

                    AIReportNumbers.Add(Convert.ToInt32(NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].visits.Where(x => x.voided == false).OrderByDescending(XmlAnyAttributeAttribute => XmlAnyAttributeAttribute.createdDate).ToList()[i].reports.Where(x => x.voided == false && x.isCloudReport == true).ToList().Count));

                }
                //d.Columns.RemoveAt(d.Columns.IndexOf("NoOfReports"));
                //d.Columns.RemoveAt(d.Columns.IndexOf("NoOfImages"));
                d.Columns["createdDate"].ColumnName = IVLVariables.LangResourceManager.GetString("Visit_Date_Text", IVLVariables.LangResourceCultureInfo);
                d.Columns["Time"].ColumnName = IVLVariables.LangResourceManager.GetString("Visit_Time_Text", IVLVariables.LangResourceCultureInfo);
                //d.Columns["NoOfImages"].ColumnName = IVLVariables.LangResourceManager.GetString("No_of_Images_Text", IVLVariables.LangResourceCultureInfo);
                //d.Columns["NoOfReports"].ColumnName = IVLVariables.LangResourceManager.GetString("No_of_Reports_Text", IVLVariables.LangResourceCultureInfo);
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
//                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        /// <summary>
        /// This function is used to display the visits of the selected patient.
        /// </summary>
        private void SetVisitColumnVisibilty()
        {
            try
            {
                if (this.Visible)
                {
                    //visitsView_dgv.Refresh();
                    for (int i = 0; i < visitsView_dgv.Columns.Count; i++)
                    {
                        if (!visitsView_dgv.Columns[i].Name.Contains(IVLVariables.LangResourceManager.GetString("Visit_Date_Text", IVLVariables.LangResourceCultureInfo)) && !visitsView_dgv.Columns[i].Name.Contains(IVLVariables.LangResourceManager.GetString("Visit_Time_Text", IVLVariables.LangResourceCultureInfo)) && !visitsView_dgv.Columns[i].Name.Contains(IVLVariables.LangResourceManager.GetString("No_of_Reports_Text", IVLVariables.LangResourceCultureInfo)) && !visitsView_dgv.Columns[i].Name.Contains(IVLVariables.LangResourceManager.GetString("No_of_Images_Text", IVLVariables.LangResourceCultureInfo))
                            && !visitsView_dgv.Columns[i].Name.Contains(IVLVariables.LangResourceManager.GetString("VisitsAddImages_ColName", IVLVariables.LangResourceCultureInfo)) && !visitsView_dgv.Columns[i].Name.Contains(IVLVariables.LangResourceManager.GetString("VisitsViewImages_ColName", IVLVariables.LangResourceCultureInfo)) && !visitsView_dgv.Columns[i].Name.Contains(IVLVariables.LangResourceManager.GetString("VisitsViewReports_ColName", IVLVariables.LangResourceCultureInfo)) && !visitsView_dgv.Columns[i].Name.Contains(IVLVariables.LangResourceManager.GetString("DeleteVisits_Button_Text", IVLVariables.LangResourceCultureInfo)))
                            visitsView_dgv.Columns[i].Visible = false;
                        else
                        {
                            visitsView_dgv.Columns[i].Visible = true;
                            //This code has been added by Darshan on 13-08-2015 7:00 PM to solve Defect no 0000553: Time settings are not reflecting correctly.
                        }
                        visitsView_dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    DataGridViewDisableButtonColumn col;
                    //The below code has been added to add link in place of text.
                    if (!visitsView_dgv.Columns.Contains(IVLVariables.LangResourceManager.GetString("No_of_Reports_Text", IVLVariables.LangResourceCultureInfo)) && !visitsView_dgv.Columns.Contains(IVLVariables.LangResourceManager.GetString("No_of_Images_Text", IVLVariables.LangResourceCultureInfo)) && !visitsView_dgv.Columns.Contains(IVLVariables.LangResourceManager.GetString("No_of_AIReports_Text", IVLVariables.LangResourceCultureInfo)))
                    {
                        DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                        //DataGridViewLinkColumn column = new DataGridViewLinkColumn();// visitsView_dgv.Columns[IVLVariables.LangResourceManager.GetString( "No_of_Reports_Text];
                        column.Name = IVLVariables.LangResourceManager.GetString("No_of_Images_Text", IVLVariables.LangResourceCultureInfo);
                        visitsView_dgv.Columns.Add(column);
                        //column = new DataGridViewLinkColumn();// visitsView_dgv.Columns[IVLVariables.LangResourceManager.GetString( "No_of_Reports_Text];
                        column = new DataGridViewTextBoxColumn();

                        column.Name = IVLVariables.LangResourceManager.GetString("No_of_Reports_Text", IVLVariables.LangResourceCultureInfo);
                        visitsView_dgv.Columns.Add(column);

                        column = new DataGridViewTextBoxColumn();
                        column.Name = IVLVariables.LangResourceManager.GetString("No_of_AIReports_Text", IVLVariables.LangResourceCultureInfo);
                        visitsView_dgv.Columns.Add(column);
                    }
                    //if (!visitsView_dgv.Columns.Contains(IVLVariables.LangResourceManager.GetString("VisitsAddImages_ColName", IVLVariables.LangResourceCultureInfo)))
                    //{
                    //    col = new DataGridViewDisableButtonColumn();
                    //    col.UseColumnTextForButtonValue = true;
                    //    col.Text = IVLVariables.LangResourceManager.GetString("AddVisitImages_Button_Text", IVLVariables.LangResourceCultureInfo);
                    //    col.Name = IVLVariables.LangResourceManager.GetString("VisitsAddImages_ColName", IVLVariables.LangResourceCultureInfo);
                    //    visitsView_dgv.Columns.Add(col);
                    //}
                    //The below code has been added to add icon in place of button.
                    //if (!visitsView_dgv.Columns.Contains(IVLVariables.LangResourceManager.GetString("DeleteVisits_Button_Text", IVLVariables.LangResourceCultureInfo)))
                    //{
                    //    DataGridViewImageColumn col1 = new DataGridViewImageColumn();
                    //    col1.Name = IVLVariables.LangResourceManager.GetString("DeleteVisits_Button_Text", IVLVariables.LangResourceCultureInfo);
                    //    string deleteLogoFilePath = @"ImageResources\Edit_ImageResources\Delete_Red.png";

                    //    if (File.Exists(deleteLogoFilePath))
                    //    {
                    //        col1.Image = Image.FromFile(deleteLogoFilePath);
                    //    }
                    //    visitsView_dgv.Columns.Add(col1);
                    //}

                    if (Screen.PrimaryScreen.Bounds.Width == 1280)
                    {
                        //Below value has been changed by Darshan on 28-10-2015 to solve Defect no 0000644: When no Consultations are present,the "Visit Date" label is coming up as "Visit".
                        visitsView_dgv.Columns[IVLVariables.LangResourceManager.GetString("Visit_Date_Text", IVLVariables.LangResourceCultureInfo)].Width = 92;
                        visitsView_dgv.Columns[IVLVariables.LangResourceManager.GetString("Visit_Time_Text", IVLVariables.LangResourceCultureInfo)].Width = 92;
                        visitsView_dgv.Columns[IVLVariables.LangResourceManager.GetString("No_of_Images_Text", IVLVariables.LangResourceCultureInfo)].Width = 92;
                        visitsView_dgv.Columns[IVLVariables.LangResourceManager.GetString("No_of_Reports_Text", IVLVariables.LangResourceCultureInfo)].Width = 92;
                        visitsView_dgv.Columns[IVLVariables.LangResourceManager.GetString("No_of_AIReports_Text", IVLVariables.LangResourceCultureInfo)].Width = 92;

                        //visitsView_dgv.Columns[IVLVariables.LangResourceManager.GetString("DeleteVisits_Button_Text", IVLVariables.LangResourceCultureInfo)].Width = 90;
                        //visitsView_dgv.Columns[IVLVariables.LangResourceManager.GetString("VisitsAddImages_ColName", IVLVariables.LangResourceCultureInfo)].Width = 55;
                    }
                    else
                        if (Screen.PrimaryScreen.Bounds.Width == 1366)
                        {
                            //Below value has been changed by Darshan on 28-10-2015 to solve Defect no 0000644: When no Consultations are present,the "Visit Date" label is coming up as "Visit".
                            visitsView_dgv.Columns[IVLVariables.LangResourceManager.GetString("Visit_Date_Text", IVLVariables.LangResourceCultureInfo)].Width = 130;
                            visitsView_dgv.Columns[IVLVariables.LangResourceManager.GetString("Visit_Time_Text", IVLVariables.LangResourceCultureInfo)].Width = 130;
                            visitsView_dgv.Columns[IVLVariables.LangResourceManager.GetString("No_of_Images_Text", IVLVariables.LangResourceCultureInfo)].Width = 80;
                            visitsView_dgv.Columns[IVLVariables.LangResourceManager.GetString("No_of_Reports_Text", IVLVariables.LangResourceCultureInfo)].Width = 80;
                            visitsView_dgv.Columns[IVLVariables.LangResourceManager.GetString("No_of_AIReports_Text", IVLVariables.LangResourceCultureInfo)].Width = 80;
                        //visitsView_dgv.Columns[IVLVariables.LangResourceManager.GetString("DeleteVisits_Button_Text", IVLVariables.LangResourceCultureInfo)].Width = 85;
                        //visitsView_dgv.Columns[IVLVariables.LangResourceManager.GetString("VisitsAddImages_ColName", IVLVariables.LangResourceCultureInfo)].Width = 120;
                    }
                        else if (Screen.PrimaryScreen.Bounds.Width == 1920)
                        {
                            //Below value has been changed by Darshan on 28-10-2015 to solve Defect no 0000644: When no Consultations are present,the "Visit Date" label is coming up as "Visit".
                            visitsView_dgv.Columns[IVLVariables.LangResourceManager.GetString("Visit_Date_Text", IVLVariables.LangResourceCultureInfo)].Width = 150;
                            visitsView_dgv.Columns[IVLVariables.LangResourceManager.GetString("Visit_Time_Text", IVLVariables.LangResourceCultureInfo)].Width = 150;
                            visitsView_dgv.Columns[IVLVariables.LangResourceManager.GetString("No_of_Images_Text", IVLVariables.LangResourceCultureInfo)].Width = 150;
                            visitsView_dgv.Columns[IVLVariables.LangResourceManager.GetString("No_of_Reports_Text", IVLVariables.LangResourceCultureInfo)].Width = 150;
                            visitsView_dgv.Columns[IVLVariables.LangResourceManager.GetString("No_of_AIReports_Text", IVLVariables.LangResourceCultureInfo)].Width = 150;

                        //visitsView_dgv.Columns[IVLVariables.LangResourceManager.GetString("DeleteVisits_Button_Text", IVLVariables.LangResourceCultureInfo)].Width = 125;
                        // visitsView_dgv.Columns[IVLVariables.LangResourceManager.GetString("VisitsAddImages_ColName", IVLVariables.LangResourceCultureInfo)].Width = 140;
                    }
                    foreach (DataGridViewRow item in visitsView_dgv.Rows)
                    {
                        string dateVal = item.Cells[IVLVariables.LangResourceManager.GetString("Visit_Date_Text", IVLVariables.LangResourceCultureInfo)].Value.ToString();
                        DateTime day = Convert.ToDateTime(dateVal);
                        //DataGridViewLinkCell linCell = new DataGridViewLinkCell();
                        string imageNumbersStr = ImageNumbers[visitsView_dgv.Rows.IndexOf(item)].ToString();
                        //linCell.Value = reportNumbersStr ;
                        //item.Cells[IVLVariables.LangResourceManager.GetString("No_of_Reports_Text", IVLVariables.LangResourceCultureInfo)] = linCell;
                        item.Cells[IVLVariables.LangResourceManager.GetString("No_of_Images_Text", IVLVariables.LangResourceCultureInfo)].Value = imageNumbersStr;
                        //linCell = new DataGridViewLinkCell();
                        string reportNumbersStr = ReportNumbers[visitsView_dgv.Rows.IndexOf(item)].ToString();

                        string AIReportNumbersStr = AIReportNumbers[visitsView_dgv.Rows.IndexOf(item)].ToString();

                        //linCell.Value = imageNumbersStr;
                        // item.Cells[IVLVariables.LangResourceManager.GetString("No_of_Images_Text", IVLVariables.LangResourceCultureInfo)] = linCell;
                        item.Cells[IVLVariables.LangResourceManager.GetString("No_of_Reports_Text", IVLVariables.LangResourceCultureInfo)].Value = reportNumbersStr;
                        item.Cells[IVLVariables.LangResourceManager.GetString("No_of_AIReports_Text", IVLVariables.LangResourceCultureInfo)].Value = AIReportNumbersStr;
                        //DataGridViewDisableButtonCell disableButtonCell = item.Cells[IVLVariables.LangResourceManager.GetString("VisitsAddImages_ColName", IVLVariables.LangResourceCultureInfo)] as DataGridViewDisableButtonCell;
                        //if (DateTime.Now.Date == day.Date)
                        //{
                        //    if (!IVLVariables.ivl_Camera.camPropsHelper.IsCameraConnected)
                        //        disableButtonCell.Enabled = false;
                        //    else if (IVLVariables.ivl_Camera.camPropsHelper.IsPowerConnected && IVLVariables.ivl_Camera.camPropsHelper.IsCameraConnected)
                        //        disableButtonCell.Enabled = true;
                        //    else
                        //        disableButtonCell.Enabled = false;
                        //}
                        //else
                        //    disableButtonCell.Enabled = false;
                    }
                    visitColumnsVisible = true;
                    updateVisitGridView();
                    visitsView_dgv.Refresh();
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
//                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        /// <summary>
        /// This event will set the text in PatEditLastName_tbx to a string variable patLastNameSearchText for searching purpose.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PatEditLastName_tbx_TextChanged(object sender, EventArgs e)
        {
            {
                patLastNameSearchText = PatSearchLastName_tbx.Text;
                search();
            }
        }

        /// <summary>
        /// This method will update the patient data gridview  based on the selected row.
        /// </summary>
        private void updatePatGridView()
        {
            try
            {
                if (patGridView_dgv.SelectedRows.Count > 0)
                {
                    DeletePat_btn.Enabled = true;
                    Update_btn.Enabled = true;
                    int id = Convert.ToInt32(patGridView_dgv.SelectedRows[0].Cells["personId"].Value.ToString());
                    stW.Start();
                    for (int i = 0; i < NewDataVariables.Patients.Count; i++)
                    {
                        if(NewDataVariables.Patients[i].personId == id)
                        {
                            NewDataVariables.Active_Patient = NewDataVariables.Patients[i].personId;
                            break;
                        }
                    }
                    //NewPatient NewPatient_Db = NewDataVariables.Patients.Find(x => x.personId == id);
                    NewDataVariables.Active_PatientIdentifier = NewDataVariables.Identifier.Find(x => x.patient.personId == id);
                    NewDataVariables.Active_PersonAddressModel = NewDataVariables.Address.Find(x => x.person.personId == id);
                    //NewDataVariables.Active_PatientIdentifier.lastModifiedDate = DateTime.Now;
                    stW.Stop();
                    long time = stW.ElapsedMilliseconds;
                    //MessageBox.Show(time.ToString());
                    stW.Start();
                    ShowIndividualPatDetails();
                    stW.Stop();
                    time = stW.ElapsedMilliseconds;
                    ////MessageBox.Show(time.ToString());
                    int value = patGridView_dgv.Rows.Count;
                    currentIndx = patGridView_dgv.SelectedRows[0].Index;
                    NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].date_accessed = DateTime.Now;
                    //NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].patientLastModifiedDate = DateTime.Now;
                    IVLVariables.ActivePatID = NewDataVariables.Patients.IndexOf(NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0]);
                    NewDataVariables.Patients[IVLVariables.ActivePatID] = NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0];
                    //NewDataVariables._Repo.Update<Patient>(NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0]);
                    //NewDataVariables.Active_PatientIdentifier.lastModifiedDate = DateTime.Now;
                    //NewDataVariables._Repo.Update<patient_identifier>(NewDataVariables.Active_PatientIdentifier);
                    //if (isUpdate)
                    //{
                    //    loadModifyData();
                    //}
                    if (isEnter || isTab)
                    {
                        if (isEnter)
                            isEnter = !isEnter;
                        if (isTab)
                            isTab = !isTab;
                        currentIndx = patGridView_dgv.SelectedRows[0].Index;
                    }

                }
                else
                {
                    DeletePat_btn.Enabled = false;
                    Update_btn.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
//                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        /// <summary>
        /// This event will open a window for advance serching.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdvanceSearch_linklbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //This below ResetSearch() has been added by Darshan on 02-09-2015 to solve Defect no 0000614: The results are conflicting when the search columns are entered simultaneously.
            if (pat_search)
                ResetSearch();
            cf.Advanceserach_details();
            if (cf.ShowDialog() == DialogResult.Cancel)
            {
            }
        }

        /// <summary>
        /// This event which will call DeletePatient for deletion purpose.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeletePat_btn_Click(object sender, EventArgs e)
        {
            DeletePatient();
        }

        /// <summary>
        /// This event is used to create a new consultation for a patient.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newConsultation_btn_Click(object sender, EventArgs e)
        {
            CreateConsultation();
        }
        public void CreateConsultation()
        {
            if (createPatientUC != null && createPatientUC.isUpdate)
            {
                Args arg = new Args();
                arg["alert_msg"] = IVLVariables.LangResourceManager.GetString("AlertPatientDetailsEdit_Text", IVLVariables.LangResourceCultureInfo);
                arg["alert_header"] = IVLVariables.LangResourceManager.GetString("NewConsultation_Button_Text", IVLVariables.LangResourceCultureInfo);
                bool val = createPatientUC.AlertModificationExit(arg);
                if (!val)
                    return;
                createPatientUC.resetUpdateButtonText();
                createPatientUC.isUpdate = false;
            }
            // Added in order to fix the defect 0000062 in order to check a selected patient before creating a consultation added by sriram on 23rd march 2015
            if (patGridView_dgv.SelectedRows.Count == 0)
            {
                CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("New_Consultation_Select_Patient", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("Conslutation_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Warning);
                return;
            }
            //This code has been added to insert user id into the table has to be removed once user or admin has been added and has be replaced by active user.
            users user = users.CreateNewUsers();
            user.userId = 1;
            visit newVisit = visit.CreateNewVisit();
            newVisit.createdDate = DateTime.Now;
            newVisit.createdBy = user;
            newVisit.lastAccessedDate = DateTime.Now;
            //This below code has been Added by Darshan on 31-07-2015 to support advance searching.
            NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].patientLastModifiedDate = DateTime.Now;
            newVisit.patient = NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0];
            //NewIVLDataMethods.AddVisit(newVisit);
            NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].visits.Add(newVisit);
            NewIVLDataMethods.UpdatePatient();
            NewDataVariables.Active_PatientIdentifier.lastModifiedDate = DateTime.Now;
            NewIVLDataMethods.UpdatePatientIdentifier();
            updatePatGridView();
            updateVisitGridView();
            //UpdateVisitButton();
            visitsView_dgv.Refresh();
            if (Convert.ToBoolean(IVLVariables.CurrentSettings.ImageStorageSettings.IsMrnFolder.val))
                Directory.CreateDirectory(IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_PatientIdentifier.value.ToString() + Path.DirectorySeparatorChar + newVisit.createdDate.Date.ToString().Replace('/', '_').Remove(10));
            //visitsView_dgv.Rows[0].Selected = true;
        }

        /// <summary>
        /// This fucnction is used to set exisiting patient details into the patient datagridview.
        /// </summary>
        private void SetPatientColumnsVisible()
        {
            try
            {
                for (int i = 0; i < patGridView_dgv.Columns.Count; i++)
                {
                    if (!patGridView_dgv.Columns[i].Name.Contains("MRN") && !patGridView_dgv.Columns[i].Name.Contains("FirstName") && !patGridView_dgv.Columns[i].Name.Contains("LastName")
                        && !patGridView_dgv.Columns[i].Name.Contains("Gender") && !patGridView_dgv.Columns[i].Name.Contains("Age"))
                        patGridView_dgv.Columns[i].Visible = false;
                    else
                    {
                        patGridView_dgv.Columns[i].Visible = true;
                        if (patGridView_dgv.Columns[i].Name.Contains("FirstName"))
                        {
                            //patGridView_dgv.Columns[i].HeaderText = "First Name";
                            patGridView_dgv.Columns[i].HeaderText = IVLVariables.LangResourceManager.GetString("FirstName_Text", IVLVariables.LangResourceCultureInfo);
                        }
                        else if (patGridView_dgv.Columns[i].Name.Contains("LastName"))
                        {
                            //patGridView_dgv.Columns[i].HeaderText = "Last Name";
                            patGridView_dgv.Columns[i].HeaderText = IVLVariables.LangResourceManager.GetString("LastName_Text", IVLVariables.LangResourceCultureInfo);
                        }
                        else if (patGridView_dgv.Columns[i].Name.Contains("Age"))
                        {
                            patGridView_dgv.Columns[i].HeaderText = IVLVariables.LangResourceManager.GetString("Age_radio_button_Text", IVLVariables.LangResourceCultureInfo);
                        }
                        else if (patGridView_dgv.Columns[i].Name.Contains("Gender"))
                        {
                            patGridView_dgv.Columns[i].HeaderText = IVLVariables.LangResourceManager.GetString("Gender_Label_Text", IVLVariables.LangResourceCultureInfo);
                        }
                    }
                }
                for (int i = 0; i < patGridView_dgv.Columns.Count; i++)
                {
                    patGridView_dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                if (patGridView_dgv.Rows.Count > 0)
                {
                    if (currentIndx < 0)
                        patGridView_dgv.Rows[0].Selected = false;
                    else if(currentIndx < patGridView_dgv.Rows.Count)
                        patGridView_dgv.Rows[currentIndx].Selected = true;
                    else
                        patGridView_dgv.Rows[0].Selected = true ;
                }

                if (Screen.PrimaryScreen.Bounds.Width == 1280)
                {
                    patGridView_dgv.Columns["FirstName"].Width = 127;
                    patGridView_dgv.Columns["LastName"].Width = 127;
                    patGridView_dgv.Columns["Gender"].Width = 75;
                    patGridView_dgv.Columns["Age"].Width = 60;
                    patGridView_dgv.Columns["MRN"].Width = 85;
                }
                else
                    if (Screen.PrimaryScreen.Bounds.Width == 1366)
                    {
                        patGridView_dgv.Columns["FirstName"].Width = 137;
                        patGridView_dgv.Columns["LastName"].Width = 139;
                        patGridView_dgv.Columns["Gender"].Width = 85;
                        patGridView_dgv.Columns["Age"].Width = 60;
                        patGridView_dgv.Columns["MRN"].Width = 85;
                    }
                    else
                    {
                        patGridView_dgv.Columns["FirstName"].Width = 217;
                        patGridView_dgv.Columns["LastName"].Width = 216;
                        patGridView_dgv.Columns["Gender"].Width = 100;
                        patGridView_dgv.Columns["Age"].Width = 83;
                        patGridView_dgv.Columns["MRN"].Width = 105;
                        
                    }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
//                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        /// <summary>
        /// This event will open a window to modify the details of the selected patient.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update_btn_Click(object sender, EventArgs e)
        {
            UpdatePatient();
        }

        public void UpdatePatient()
        {
            if (patGridView_dgv.SelectedRows.Count != 0)//if a patient is selected.
            {
                cf.currentIndx = currentIndx;
                
                //NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient);// = NewDataVariables._Repo.GetById<Patient>(NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].personId);
                cf.Populate_Details(NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0], false);
                cf.Color1 = IVLVariables.GradientColorValues.Color1;
                cf.Color2 = IVLVariables.GradientColorValues.Color2;
                cf.ColorAngle = IVLVariables.GradientColorValues.ColorAngle;
                cf.ShowDialog();
                patGridView_dgv.Focus();//this code has been added to solve defect When the patient data is modified,and pressed the upper/lower keys the focus is not on the patient grid by Darshan.
                if (Convert.ToBoolean(IVLVariables.CurrentSettings.ImageStorageSettings.IsMrnFolder.val))
                {
                    patDetList = new List<string>();
                    patDetList.Add("MRN: " + NewDataVariables.Active_PatientIdentifier.value.ToString());
                    patDetList.Add("Name: " + NewDataVariables.Active_PatientIdentifier.patient.firstName.ToString() + " " + NewDataVariables.Active_PatientIdentifier.patient.lastName.ToString());
                    patDetList.Add("Age: " + (DateTime.Now.Year - NewDataVariables.Active_PatientIdentifier.patient.birthdate.Year).ToString());
                    patDetList.Add("Gender: " + NewDataVariables.Active_PatientIdentifier.patient.gender.ToString());
                    string path = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_PatientIdentifier.value.ToString();
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    Common.PatientDetailsJson.GetInstance().CreatePatientJsonFile(patDetList, path);
                }
            }
            else //to show message box if no patient is selected and edit button is clicked.
                CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("Modify_Select_Patient", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("Patient_Warning_Header", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Warning);

        }

        /// <summary>
        /// This event will help in movement of scroll bar of datagridview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void patGridView_dgv_MouseWheel(object sender, MouseEventArgs e)
        {
            //Defect no  0000024 of mouse scroll is solved by invoking Mousewheel object//
            int currentIndex = this.patGridView_dgv.FirstDisplayedScrollingRowIndex;
            int scrollLines = 0;
            // Added this if statement to fix defect 0000625  by darshan  on september 11th 2015
            if (SystemInformation.MouseWheelScrollLines <= 3)
            {
                scrollLines = SystemInformation.MouseWheelScrollLines;
            }
            else
            {
                scrollLines = 3;
            }
            if (e.Delta > 0)
            {
                this.patGridView_dgv.FirstDisplayedScrollingRowIndex = Math.Max(0, currentIndex - scrollLines);
            }
            else if (e.Delta < 0)
            {
                // Added this line to fix defect 0000625  by darshan  on september 9th 2015
                if (patGridView_dgv.Rows[0].Index != patGridView_dgv.RowCount - 1 && patGridView_dgv.RowCount > 3)
                    this.patGridView_dgv.FirstDisplayedScrollingRowIndex = currentIndex + scrollLines;
            }
        }

        /// <summary>
        /// This event will load the EmrManage user control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EmrManage_Load(object sender, EventArgs e)
        {
            {
                NewDataVariables.CloudAnalysisReports = NewDataVariables._Repo.GetAll<CloudAnalysisReport>().ToList();
                NewDataVariables.Obs = NewDataVariables._Repo.GetAll<eye_fundus_image>().ToList();

                pageRows = Convert.ToInt32(IVLVariables.CurrentSettings.UserSettings._NoOfPatientsToBeSelected.val);//Assigns no of patients to be displayed
                //PagesCount = Convert.ToInt32(Math.Ceiling(NewDataVariables.Patients.Count * 1.0 / pageRows));
                PagesCount = Convert.ToInt32(Math.Ceiling(NewDataVariables._Repo.GetPatientCount() * 1.0 / pageRows));//Convert.ToInt32(Math.Ceiling(NewDataVariables.Patients.Count * 1.0 / pageRows));
                totlPages_lbl.Text = IVLVariables.LangResourceManager.GetString("OutOf_Text", IVLVariables.LangResourceCultureInfo) + " " + PagesCount;
                pageNum_lbl.Text = IVLVariables.LangResourceManager.GetString("Page_Lbl_text", IVLVariables.LangResourceCultureInfo);
                pageNum_nud.Maximum = PagesCount;
               // NewDataVariables.Patients = NewDataVariables._Repo.GetPageData<INTUSOFT.Data.NewDbModel.Patient>(pageRows, CurrentPage).ToList<INTUSOFT.Data.NewDbModel.Patient>();
                if (patGridView_dgv.Rows.Count > 0)
                {
                    patGridView_dgv.Rows[0].Selected = true;
                    updatePatGridView();
                }
                if (visitsView_dgv.Rows.Count > 0)
                {
                    visitsView_dgv.Rows[0].Selected = true;
                }
                updateVisitGridView();
                //UpdateVisitButton();
                Emr_Load = true;
            }
            patGridView_dgv.Focus();
        }

        /// <summary>
        /// This event will unselect the patient when we click patient datagridview with control key.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void patGridView_dgv_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                if (e.RowIndex == -1)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        if (patGridView_dgv.Rows.Count > 0)//This line was added by darshan to solve the crash when header field is clicked with control key.
                            Unselect();
                    }
                }
            }
        }

        /// <summary>
        /// This event is triggered when any of cell in visit datagridview is clicked it navigate to the next screen based on which cell is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void visitsView_dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            visitsView_dgv.Focus();
           
            //if (visitsView_dgv.SelectedRows.Count > 0)
            //{
            //    updatePatGridView();
            //    int str = 0;
            //    try
            //    {
            //         str = Convert.ToInt32(visitsView_dgv.SelectedRows[0].Cells[IVLVariables.LangResourceManager.GetString("No_of_Images_Text", IVLVariables.LangResourceCultureInfo)].Value.ToString());

            //    }
            //    catch (Exception)
            //    {
                    
            //        throw;
            //    }
            //    if (createPatientUC != null && createPatientUC.isUpdate)
            //    {
            //        createPatientUC.Cancel_Modification();
            //        createPatientUC.Refresh();
            //    }
            //    visitsView_dgv.Focus();
            //    bool isLiveImaging = false;
            //    if (e.ColumnIndex < 0)
            //        return;
            //    if (e.RowIndex >= 0)
            //    {
            //        visitId = Convert.ToInt32(visitsView_dgv.SelectedRows[0].Cells["visitId"].Value.ToString());
            //        NewDataVariables.Active_Visit = NewDataVariables.Visits[NewDataVariables.Visits.FindIndex(x => x.visitId == visitId)];
            //        selectedVisitIndex = e.RowIndex;
            //        //if (visitsView_dgv.Columns[e.ColumnIndex].Name == IVLVariables.LangResourceManager.GetString("VisitsAddImages_ColName", IVLVariables.LangResourceCultureInfo) &&
            //        //    ((DataGridViewDisableButtonCell)(visitsView_dgv.Rows[e.RowIndex].Cells[IVLVariables.LangResourceManager.GetString("VisitsAddImages_ColName", IVLVariables.LangResourceCultureInfo)])).Enabled &&
            //        //    IVLVariables.ivl_Camera.camPropsHelper.IsBoardOpen && IVLVariables.ivl_Camera.camPropsHelper.IsPowerConnected && IVLVariables.ivl_Camera.camPropsHelper.IsCameraConnected)
            //        //{
            //        //    isLiveImaging = true;
            //        //    IVLVariables.CanAddImages = true;// Added to fix the defect 0000582  and 0000562 by sriram on August 18th 2015
            //        //    IVLVariables.ShowImagingBtn = ((DataGridViewDisableButtonCell)(visitsView_dgv.Rows[e.RowIndex].Cells[IVLVariables.LangResourceManager.GetString("VisitsAddImages_ColName", IVLVariables.LangResourceCultureInfo)])).Enabled;
            //        //    if (IVLVariables.ivl_Camera.camPropsHelper.IsBoardOpen && IVLVariables.ivl_Camera.camPropsHelper.IsPowerConnected && IVLVariables.ivl_Camera.camPropsHelper.IsCameraConnected)
            //        //        GotoImaging(e.RowIndex, isLiveImaging);
            //        //}
            //       // else
            //        if (visitsView_dgv.Columns[e.ColumnIndex].Name == IVLVariables.LangResourceManager.GetString("No_of_Images_Text", IVLVariables.LangResourceCultureInfo) && str != 0)
            //        {
            //            IVLVariables.ShowImagingBtn = ((DataGridViewDisableButtonCell)(visitsView_dgv.Rows[e.RowIndex].Cells[IVLVariables.LangResourceManager.GetString("VisitsAddImages_ColName", IVLVariables.LangResourceCultureInfo)])).Enabled;
            //            IVLVariables.CanAddImages = IVLVariables.ShowImagingBtn;
            //            GotoImaging(e.RowIndex, isLiveImaging);
            //            //clearVisitGrid();
            //        }
            //        else if (visitsView_dgv.Columns[e.ColumnIndex].Name == IVLVariables.LangResourceManager.GetString("No_of_Reports_Text", IVLVariables.LangResourceCultureInfo))
            //        {
            //        }
            //        else if (visitsView_dgv.Columns[e.ColumnIndex].Name == IVLVariables.LangResourceManager.GetString("DeleteVisits_Button_Text", IVLVariables.LangResourceCultureInfo))
            //        {
            //            DialogResult val = CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("DeleteVisitConfirmation_Text", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("DeleteVisitHeader_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxButtons.YesNo, CustomMessageBoxIcon.Question);
            //            if (val == DialogResult.Yes)
            //            {
            //                DeleteConsultation();
            //            }
            //        }
            //    }
            //}
        }

        public void AddImages()
        {
            //isLiveImaging = true;
            //IVLVariables.CanAddImages = true;// Added to fix the defect 0000582  and 0000562 by sriram on August 18th 2015
            //IVLVariables.ShowImagingBtn = ((DataGridViewDisableButtonCell)(visitsView_dgv.Rows[e.RowIndex].Cells[IVLVariables.LangResourceManager.GetString("VisitsAddImages_ColName", IVLVariables.LangResourceCultureInfo)])).Enabled;
            //if (IVLVariables.ivl_Camera.camPropsHelper.IsBoardOpen && IVLVariables.ivl_Camera.camPropsHelper.IsPowerConnected && IVLVariables.ivl_Camera.camPropsHelper.IsCameraConnected)
               // GotoImaging(e.RowIndex, isLiveImaging);
        }
        /// <summary>
        /// This event will change the genderSelectedIndx value to the PatEditGender_cmbx selected item index.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PatEditGender_cmbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!panel1.Controls.Contains(createPatientUC))
            {
                genderSelectedIndx = PatEditGender_cmbx.SelectedIndex;
                if(Emr_Load)
                search();
            }
        }

        /// <summary>
        /// This event will change the selected row in patinet datagridview when either up arrow or down arrow is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void patGridView_dgv_KeyDown(object sender, KeyEventArgs e)
        {
            isEnter = false;
            Args arg = new Args();
            arg["alert_msg"] = IVLVariables.LangResourceManager.GetString("AlertPatientDetailsEdit_Text", IVLVariables.LangResourceCultureInfo);
            arg["alert_header"] = IVLVariables.LangResourceManager.GetString("Pat_Details_Text", IVLVariables.LangResourceCultureInfo);
            if (patGridView_dgv.SelectedRows.Count > 0)
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (createPatientUC != null && createPatientUC.isUpdate)
                    {
                        bool val = createPatientUC.AlertModificationExit(arg);
                        if (!val)
                            return;
                    }
                    // this has been added to fix the defect 0000067 a check for row index has been added by sriram on march 23rd 2015
                    if (patGridView_dgv.SelectedRows[0].Index >= 0 && patGridView_dgv.SelectedRows[0].Index < patGridView_dgv.RowCount - 1)
                    {
                        {
                            //This below code has been added by darshan to solve defect no:0000557: The patient details grid is showing duplicate details when arrow press searching was done.
                            patGridView_dgv.Refresh();
                            int currentIndx = patGridView_dgv.SelectedRows[0].Index;
                            patGridView_dgv.Rows[currentIndx + 1].Selected = true;
                            updatePatGridView();
                        }
                    }
                }
                else if (e.KeyCode == Keys.Up)
                {
                    if (createPatientUC != null && createPatientUC.isUpdate)
                    {
                        bool val = createPatientUC.AlertModificationExit(arg);
                        if (!val)
                            return;
                    }
                    if (patGridView_dgv.SelectedRows[0].Index < patGridView_dgv.Rows.Count && patGridView_dgv.SelectedRows[0].Index > 0)
                    {
                        //This below code has been added by darshan to solve defect no:0000557: The patient details grid is showing duplicate details when arrow press searching was done.
                        patGridView_dgv.Refresh();
                        int currentIndx = patGridView_dgv.SelectedRows[0].Index;
                        patGridView_dgv.Rows[currentIndx - 1].Selected = true;
                        updatePatGridView();
                        //ShowIndividualPatDetails();
                    }
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    if (patGridView_dgv.SelectedRows[0].Index >= 0 && patGridView_dgv.SelectedRows[0].Index < patGridView_dgv.RowCount - 1)
                    {
                        {
                            //This below code has been added by darshan to solve defect no:0000557: The patient details grid is showing duplicate details when arrow press searching was done.
                            isEnter = true;
                            patGridView_dgv.Refresh();
                            int currentIndx = patGridView_dgv.SelectedRows[0].Index;
                            patGridView_dgv.Rows[currentIndx + 1].Selected = true;
                            updatePatGridView();
                            //patGridView_dgv.FirstDisplayedScrollingRowIndex = newScrollValue;
                        }
                    }
                }
                else
                    if (e.KeyCode == Keys.Delete)
                    {
                        if (NewDataVariables.Active_Patient != null)
                        {
                            DeletePatient();
                        }
                    }
                    else
                        if (e.KeyCode == Keys.Tab)
                        {
                            int count = 0;
                            if (patGridView_dgv.CurrentCell != null)
                            {
                                DataGridViewCell currentCell = patGridView_dgv.CurrentCell;
                                int nextRow = currentCell.RowIndex;
                                int nextCol = currentCell.ColumnIndex + 1;
                                for (int i = 0; i < patGridView_dgv.Columns.Count; i++)
                                {
                                    if (patGridView_dgv.Columns[i].Visible)
                                        count++;
                                }
                                if (count == 5)
                                {
                                    nextCol = 0;
                                    nextRow++;
                                    isTab = true;
                                }
                                if (nextRow == patGridView_dgv.RowCount)
                                {
                                    isTab = false;
                                    nextRow = 0;
                                }
                                if (currentCell.ColumnIndex == 5)
                                {
                                    patGridView_dgv.Rows[nextRow].Selected = true;
                                    updatePatGridView();
                                }
                            }
                        }
            }
            else if (patGridView_dgv.SelectedRows.Count == 0)
            {
                return;
            }
        }

        /// <summary>
        /// This event will allow only charaters in PatEditFirstName_tbx textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PatEditFirstName_tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsPunctuation(e.KeyChar) && !char.IsSymbol(e.KeyChar))
                e.Handled = false;
            else
                if (char.IsControl(e.KeyChar))
                    e.Handled = false;
                else
                    e.Handled = true;
        }

        /// <summary>
        /// Old implementation for painting borders of a datagridview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void visitsView_dgv_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //Pen p = new Pen(Color.Black, 5.0f);
            //if (e.RowIndex == 0)
            //{
            //    e.Graphics.FillRectangle(Brushes.Black, e.CellBounds);
            //    e.Graphics.DrawLine(p, e.CellBounds.Left - 1, e.CellBounds.Top-1, e.CellBounds.Right - 1, e.CellBounds.Top-1);
            //    e.Graphics.DrawLine(p, e.CellBounds.Left , e.CellBounds.Bottom-1, e.CellBounds.Right - 1, e.CellBounds.Bottom-1);
            //}
            //if(e.RowIndex == 2)
            //    e.Graphics.DrawLine(p, e.CellBounds.Right-1, e.CellBounds.Top, e.CellBounds.Right-1, e.CellBounds.Bottom);
        }

        /// <summary>
        /// This event will call newConsultation_btn_Click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewVisit_Btn_Click(object sender, EventArgs e)
        {
            newConsultation_btn_Click(null, null);
        }

        /// <summary>
        /// Will handle the crash when a cell is cliked simultaniously with mouse button on patGridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void patGridView_dgv_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                if (e.Button == MouseButtons.Left)
                {
                    patGridView_dgv.ClearSelection();
                    string s = "";
                    Args args = new Args();
                    args["isModified"] = false;
                    Back2Search(s, args);
                }
            }
        }

        private void PatientsGroupBox_p_Paint(object sender, PaintEventArgs e)
        {
            //private void groupBox1_Paint(object sender, PaintEventArgs e)  
        }

        private void groupBox1_Paint(object sender, PaintEventArgs e)
        {
            //Graphics gfx = e.Graphics;
            //Pen p = new Pen(Color.Black, 3);
            //gfx.DrawLine(p, 0, 11, 0, e.ClipRectangle.Height);        

            //gfx.DrawLine(p, 0, 11, 10, 11);
            //gfx.DrawLine(p, 80, 11, e.ClipRectangle.Width - 2, 11);
            //gfx.DrawLine(p, e.ClipRectangle.Width - 2, 11, e.ClipRectangle.Width - 2, e.ClipRectangle.Height - 2);
            //gfx.DrawLine(p, e.ClipRectangle.Width - 2, e.ClipRectangle.Height - 2, 0, e.ClipRectangle.Height - 2);  
        }

        private void Manage_FormClosed(object sender, FormClosedEventArgs e)
        {
            thisClosed("Manage Has been closed", e);
        }

        /// <summary>
        /// This method will populate the createPatientUC to the panel1.
        /// </summary>
        private void SetCreateUpdatePatientView()
        {
            try
            {
                if (!panel1.Controls.Contains(createPatientUC))
                {
                    panel1.Controls.Clear();
                    createPatientUC = CreateModifyPatient_UC.getInstance();
                    createPatientUC.Dock = DockStyle.Fill;
                    createPatientUC.UpdateFontForeColor();
                    panel1.Controls.Add(createPatientUC);
                }

            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
//                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        /// <summary>
        /// Will select the entire row in the datagridview when clicked on it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void patGridView_dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex != -1)
            //    //pat_search = false;
            //    patGridView_dgv.Focus();//Defect changed of scroll by invoking Focus method// 
            //updatePatGridView();
            //updateVisitGridView();
        }

        /// <summary>
        /// Disables the default deletion of the user row property of a patGridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void patGridView_dgv_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void patGridView_dgv_Scroll(object sender, ScrollEventArgs e)
        {
            newScrollValue = e.NewValue;
        }

        /// <summary>
        /// Disables the default deletion of the user row property of a visitsView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void visitsView_dgv_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            e.Cancel = true;
        }
        #endregion

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
        public void RemovePatDetails()
        {

            if (PatientsGroupBox_p.Controls.Count != 0)
                if (PatientsGroupBox_p.Controls[0].Controls.Count > 0 && PatientsGroupBox_p != null)
                {
                    PatientsGroupBox_p.Controls.Clear();
                    patd.Dispose();
                    patd = null;
                }
        }
        public void AddPatDetails()
        {
            if (patd == null)
            {
                patd = new PatientDetails_UC();
                patd.Dock = DockStyle.Fill;
            }
            patd.setPatValues();
            PatientsGroupBox_p.Visible = true;
            patGridView_dgv.Focus();
            if (PatientsGroupBox_p.Controls.Contains(patd))
                PatientsGroupBox_p.Controls.Clear();
            PatientsGroupBox_p.Controls.Add(patd);
            PatientsGroupBox_p.Refresh();
        }
        public GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            // top left arc  
            path.AddArc(arc, 180, 90);

            // top right arc  
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc  
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc 
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        public void DrawRoundedRectangle(Graphics graphics, Pen pen, Rectangle bounds, int cornerRadius)
        {
            if (graphics == null)
                throw new ArgumentNullException("graphics");
            if (pen == null)
                throw new ArgumentNullException("pen");

            using (GraphicsPath path = RoundedRect(bounds, cornerRadius))
            {
                graphics.DrawPath(pen, path);
            }
        }

        private void PatientControls_p_Resize(object sender, EventArgs e)
        {
            ResizeToRoundedRectangle(sender, (PatientControls_p.Bounds.Height/2));
        }

        private void patGridView_dgv_Resize(object sender, EventArgs e)
        {
            ResizeToRoundedRectangle(sender,10);
            RebindGridForPageChange();
            //for (int i = 0; i < patGridView_dgv.Rows.Count; i++)
            //{
            //    patGridView_dgv.Rows[i].Height = (int)((float)(patGridView_dgv.Height - patGridView_dgv.ColumnHeadersHeight) / (float)(Convert.ToInt32(IVLVariables.CurrentSettings.UserSettings._NoOfPatientsToBeSelected.val)));
            //}
            //patGridView_dgv.Refresh();

        }
        private void ResizeToRoundedRectangle(object sender, int cornerRadius)
        {
            if (sender is Control)
            {
                Control c = sender as Control;
                Rectangle Bounds = new Rectangle(0, 0, c.Bounds.Width, c.Bounds.Height);
                //int CornerRadius = c.Bounds.Height / 2;
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                path = RoundedRect(Bounds, cornerRadius);
                c.Region = new Region(path);
            }
        }

        private void visitsView_dgv_Resize(object sender, EventArgs e)
        {
            ResizeToRoundedRectangle(sender, 10);
        }

        private void Consultation_p_Resize(object sender, EventArgs e)
        {
            ResizeToRoundedRectangle(sender, (Consultation_p.Bounds.Height / 2));
        }

        private void DeleteConsultation_btn_Click(object sender, EventArgs e)
        {
            DeleteConsultation();
        }

        private void LiveImaging_btn_Click(object sender, EventArgs e)
        {
            if ( IVLVariables.ivl_Camera.camPropsHelper.IsPowerConnected == Devices.PowerConnected && IVLVariables.ivl_Camera.camPropsHelper.IsCameraConnected == Devices.CameraConnected)
            {
                IVLVariables.currentVisitDateTime = NewDataVariables.Active_Visit.createdDate.Date;
                int indx = visitsView_dgv.SelectedRows[0].Index;
               bool isLiveImaging = true;
                IVLVariables.CanAddImages = true;// Added to fix the defect 0000582  and 0000562 by sriram on August 18th 2015
                IVLVariables.ShowImagingBtn = true;
                if (IVLVariables.ivl_Camera.camPropsHelper.IsPowerConnected == Devices.PowerConnected && IVLVariables.ivl_Camera.camPropsHelper.IsCameraConnected == Devices.CameraConnected)
                    GotoImaging(visitsView_dgv.SelectedRows[0].Index, isLiveImaging);
            }
        }

        private void visitsView_dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (visitColumnsVisible)
            {
                int index = 0;
                if (visitsView_dgv.SelectedRows.Count > 0)
                    index = visitsView_dgv.SelectedRows[0].Index;
                else
                    index = -1;
                    if (index >= 0)
                    {
                        visitId = Convert.ToInt32(visitsView_dgv.SelectedRows[0].Cells["visitId"].Value.ToString());
                        NewDataVariables.Active_Visit = NewDataVariables.Visits[NewDataVariables.Visits.FindIndex(x => x.visitId == visitId)];
                        //UpdateVisitButton();
                    }
                    else
                    {
                        visitsView_dgv.ClearSelection(); //SelectedRows[0].Selected = false;
                        updateVisitGridView();
                        return;
                    }
                    updateVisitGridView();
                    //UpdateVisitButton();
                
            }
        }

        private void ViewImages_btn_Click(object sender, EventArgs e)
        {
            //if (IVLVariables.ivl_Camera.camPropsHelper.IsBoardOpen && IVLVariables.ivl_Camera.camPropsHelper.IsPowerConnected && IVLVariables.ivl_Camera.camPropsHelper.IsCameraConnected)
            {
                IVLVariables.ShowImagingBtn = true;
                IVLVariables.CanAddImages = IVLVariables.ShowImagingBtn;
                IVLVariables.currentVisitDateTime = NewDataVariables.Active_Visit.createdDate.Date;
                GotoImaging(visitsView_dgv.SelectedRows[0].Index, false);
            }
        }

        private void diagnosis_tbx_TextChanged(object sender, EventArgs e)
        {
            search();
        }

        private void patGridView_dgv_SelectionChanged(object sender, EventArgs e)
        {
            if(patGridView_dgv.SelectedRows.Count > 0)
                updatePatGridView();
        }

        private void pageNum_nud_ValueChanged(object sender, EventArgs e)
        {
            CurrentPage = Convert.ToInt32(pageNum_nud.Value);
            if(CurrentPage != 0 && !pat_search)
                NewDataVariables.Patients = NewDataVariables._Repo.GetPageData<INTUSOFT.Data.NewDbModel.Patient>(Convert.ToInt32(IVLVariables.CurrentSettings.UserSettings._NoOfPatientsToBeSelected.val), CurrentPage).ToList<INTUSOFT.Data.NewDbModel.Patient>();//Get the data from the database of the CurrentPage and saves into the temp list.
            RebindGridForPageChange();
        }


        private void RefreshNumericUpDown()
        {
            //if(PagesCount > 0)
            //    pageNum_nud.Value = CurrentPage;
            if (PagesCount == 0)
                pageNum_nud.Minimum = pageNum_nud.Maximum = 0;
            else
            {
                pageNum_nud.Minimum = 1;
                pageNum_nud.Maximum = PagesCount;
            }
            totlPages_lbl.Text = IVLVariables.LangResourceManager.GetString("OutOf_Text", IVLVariables.LangResourceCultureInfo)+" " + PagesCount;
            ///totlPages_lbl.Text = "of " + "10000";
        }

        private void pageNum_nud_KeyDown(object sender, KeyEventArgs e)
        {
            #region this is added fix defect 0001851 when a new number is entered in the numeric updown deselect the current patient and clear the visit grid view to avoid the crash
            if (patGridView_dgv.SelectedRows.Count > 0)
            {
                patGridView_dgv.SelectedRows[0].Selected = false;
                while (visitsView_dgv.Rows.Count > 0)
                {
                    visitsView_dgv.Rows.RemoveAt(0);
                }
            }
            #endregion
        }

        //private void pagesOk_btn_Click(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(pageNum_tbx.Text))
        //    {
        //        if (Convert.ToInt32(pageNum_tbx.Text) > PagesCount || Convert.ToInt32(pageNum_tbx.Text) == 0)
        //        {
        //            CustomMessageBox.Show("Enter the valid page number", "Page Count", CustomMessageBoxButtons.OK, CustomMessageBoxIcon.Warning);
        //        }
        //        else
        //        {
        //            CurrentPage = Convert.ToInt32(pageNum_tbx.Text);
        //            NewDataVariables.Patients = NewDataVariables._Repo.GetPageData<INTUSOFT.Data.NewDbModel.Patient>(Convert.ToInt32(IVLVariables.CurrentSettings.UserSettings._NoOfPatientsToBeSelected.val), CurrentPage).ToList<INTUSOFT.Data.NewDbModel.Patient>();//Get the data from the database of the CurrentPage and saves into the temp list.
        //            RebindGridForPageChange();
        //        }
        //    }
        //    else
        //    {
        //        CustomMessageBox.Show("Page number should not be empty", "Page Count", CustomMessageBoxButtons.OK, CustomMessageBoxIcon.Warning);
        //    }
        //}

        //private void pageNum_tbx_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
        //    {
        //        e.Handled = true;
        //    }
        //}
    }

    public enum SearchItems
    {
        firstName = 1,
        lastName,
        identifiers_value,
        diagnosisValueRight,
        diagnosisValueLeft,
        gender,
        patientCreatedDate,
        patientLastModifiedDate,
        lastAccessedDate 
    }
    public class AdvanceSearchParams
    {
        public DateTime fromDate;
        public DateTime toDate;
        public SearchItems searchType;
    }

}
