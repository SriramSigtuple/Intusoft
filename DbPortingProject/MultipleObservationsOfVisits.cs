using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using Svg;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Linq;
using System.Data.Common;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using INTUSOFT.Data.Repository;
using INTUSOFT.Data;
using INTUSOFT.Data.Extension;
using INTUSOFT.Data.Model;
using INTUSOFT.Data.Enumdetails;
using INTUSOFT.Data.NewDbModel;
using INTUSOFT.Data.Mapping;
using MySql.Data;
using System.Collections;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NLog;
using NLog.Targets;

namespace DBPorting
{
    public partial class MultipleObservationsOfVisits : Form
    {
        List<DateTime> visitDate = new List<DateTime>();
        List<int> ReportNumbers = new List<int>();
        List<int> ImageNumbers = new List<int>();
        public Logger Exception_Log = LogManager.GetLogger("DB_Porting.ExceptionLog");
        public MultipleObservationsOfVisits()
        {
            InitializeComponent();
            string appLogoFilePath = @"ImageResources\LogoImageResources\IntuSoft.ico";
            List<DateTime> visitDate = new List<DateTime>();
            if (File.Exists(appLogoFilePath))
                this.Icon = new System.Drawing.Icon(appLogoFilePath);
            var data = new Dictionary<string, string>();
            foreach (var row in File.ReadAllLines("Intusoft-runtime.properties"))
                data.Add(row.Split('=')[0], string.Join("=", row.Split('=').Skip(1).ToArray()));
            NHibernateHelper_MySQL.dbName = data["connection.DBname"];
            NHibernateHelper_MySQL.userName = data["connection.username"];
            NHibernateHelper_MySQL.password = data["connection.password"];
            NewDataVariables._Repo = Repository.GetInstance();
            InitilizeDataGridViewStyle();
            NewDataVariables.Patients = NewDataVariables._Repo.GetAll<INTUSOFT.Data.NewDbModel.Patient>().ToList();
            this.MinimumSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
        }

        private void getAllObservation_lbl_Click(object sender, EventArgs e)
        {
            try
            {
                INTUSOFT.Data.NewDbModel.Patient p = NewDataVariables._Repo.GetById<INTUSOFT.Data.NewDbModel.Patient>(13);
                List<obs> allObservations = NewDataVariables._Repo.GetByCategory<INTUSOFT.Data.NewDbModel.obs>("person_id", p).ToList();// as List<NewPatient>;
                MessageBox.Show(allObservations.Count.ToString());

                List<report> allReports = NewDataVariables._Repo.GetByCategory<INTUSOFT.Data.NewDbModel.report>("Patient", p).ToList();// as List<NewPatient>;
                MessageBox.Show(allReports.Count.ToString());

                INTUSOFT.Data.NewDbModel.eye_fundus_image obs = NewDataVariables._Repo.GetById<INTUSOFT.Data.NewDbModel.eye_fundus_image>(59);
                List<eye_fundus_image_annotation> allAnnotation = NewDataVariables._Repo.GetByCategory<INTUSOFT.Data.NewDbModel.eye_fundus_image_annotation>("eyeFundusImage", obs).ToList();// as List<NewPatient>;
                MessageBox.Show(allAnnotation.Count.ToString());
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        /// <summary>
        /// Setting the style of the DataGridView control.
        /// </summary>
        private void InitilizeDataGridViewStyle()
        {
            try
            {
                patGridView_dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Bold, GraphicsUnit.Point);
                patGridView_dgv.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.ControlDark;
                patGridView_dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                patGridView_dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                patGridView_dgv.DefaultCellStyle.Font = new Font("Tahoma", 10, FontStyle.Regular, GraphicsUnit.Point);
                patGridView_dgv.DefaultCellStyle.BackColor = Color.Empty;
                patGridView_dgv.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.Info;
                patGridView_dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                patGridView_dgv.GridColor = SystemColors.ControlDarkDark;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void MultipleObservationsOfVisits_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable d = NewDataVariables.Patients.ToDataTable();
                setPatientTableOrder(d);
                LoadDataGridView(d, patGridView_dgv);
                SetPatientColumnsVisible();
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
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
                            patGridView_dgv.Columns[i].HeaderText = "FirstName";
                        }
                        else if (patGridView_dgv.Columns[i].Name.Contains("LastName"))
                        {
                            //patGridView_dgv.Columns[i].HeaderText = "Last Name";
                            patGridView_dgv.Columns[i].HeaderText = "LastName";
                        }
                        else if (patGridView_dgv.Columns[i].Name.Contains("Age"))
                        {
                            patGridView_dgv.Columns[i].HeaderText = "Age";
                        }
                        else if (patGridView_dgv.Columns[i].Name.Contains("Gender"))
                        {
                            patGridView_dgv.Columns[i].HeaderText = "Gender";
                        }
                    }
                }
                for (int i = 0; i < patGridView_dgv.Columns.Count; i++)
                {
                    patGridView_dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                if (Screen.PrimaryScreen.Bounds.Width == 1280)
                {
                    patGridView_dgv.Columns["FirstName"].Width = 130;
                    patGridView_dgv.Columns["LastName"].Width = 131;
                    patGridView_dgv.Columns["Gender"].Width = 75;
                    patGridView_dgv.Columns["Age"].Width = 60;
                    patGridView_dgv.Columns["MRN"].Width = 85;
                }
                else
                    if (Screen.PrimaryScreen.Bounds.Width == 1366)
                    {
                        patGridView_dgv.Columns["FirstName"].Width = 140;
                        patGridView_dgv.Columns["LastName"].Width = 143;
                        patGridView_dgv.Columns["Gender"].Width = 85;
                        patGridView_dgv.Columns["Age"].Width = 60;
                        patGridView_dgv.Columns["MRN"].Width = 85;
                    }
                    else
                    {
                        patGridView_dgv.Columns["FirstName"].Width = 220;
                        patGridView_dgv.Columns["LastName"].Width = 220;
                        patGridView_dgv.Columns["Gender"].Width = 100;
                        patGridView_dgv.Columns["Age"].Width = 83;
                        patGridView_dgv.Columns["MRN"].Width = 105;
                    }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
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
                if (d.Rows.Count > 0)
                    d.Rows[0].Selected = false;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void setPatientTableOrder(DataTable d)
        {
            try
            {
                List<patient_identifier> patId = NewDataVariables._Repo.GetAll<patient_identifier>().ToList();
                patId.Reverse();
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
                    DateTime dt = Convert.ToDateTime(d.Rows[i]["birthdate"]);
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
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }


        public void ShowIndividualPatDetails()
        {
            try
            {
                if (patGridView_dgv.SelectedRows.Count > 0)
                {
                    //currentRow = patGridView_dgv.SelectedRows[0].Index;
                    {
                        int id = Convert.ToInt32(patGridView_dgv.SelectedRows[0].Cells["personId"].Value.ToString());
                        NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0] = NewDataVariables.Patients.Find(x => x.personId == id);
                        NewDataVariables.Visits.Reverse();
                        visitDate.Clear();
                        foreach (var item in NewDataVariables.Visits)
                        {
                            visitDate.Add(item.createdDate);
                        }
                        DataTable d = NewDataVariables.Visits.ToDataTable();
                        SetVisitData(d);
                        this.LoadDataGridView(d, visitsView_dgv);
                        //SetVisitColumnVisibilty();

                        NewDataVariables.Visit_Obs = NewDataVariables._Repo.GetByCategory<eye_fundus_image>("person_id", NewDataVariables.Active_Patient).OrderByDescending(x => x.createdDate < DateTime.Now).ToList();
                        //NewDataVariables.Obs.Reverse();
                        d = NewDataVariables.Visit_Obs.ToDataTable();
                        SetObsData(d);
                        this.LoadDataGridView(d, Obs_dgv);
                        //SetObsColumnVisibilty();

                        NewDataVariables.Reports = NewDataVariables._Repo.GetByCategory<report>("Patient", NewDataVariables.Active_Patient).OrderByDescending(x => x.createdDate < DateTime.Now).ToList();
                        //NewDataVariables.Obs.Reverse();
                        d = NewDataVariables.Reports.ToDataTable();
                        SetReportData(d);
                        this.LoadDataGridView(d, report_dgv);
                        //SetReportColumnVisibilty();
                    }
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }


        public void ShowIndividualObservationDetails()
        {
            try
            {
                if (Obs_dgv.SelectedRows.Count > 0)
                {
                    //currentRow = patGridView_dgv.SelectedRows[0].Index;
                    {
                        int id = Convert.ToInt32(Obs_dgv.SelectedRows[0].Cells["observationId"].Value.ToString());
                        NewDataVariables.Active_Obs = NewDataVariables._Repo.GetById<eye_fundus_image>(id);
                        //NewDataVariables.Active_Obs = NewDataVariables.Obs.Find(x => x.observationId == id);
                        NewDataVariables.Annotations = NewDataVariables._Repo.GetByCategory<eye_fundus_image_annotation>("eyeFundusImage", NewDataVariables.Active_Obs).OrderByDescending(x => x.createdDate < DateTime.Now).ToList();
                        //NewDataVariables.Obs.Reverse();
                        DataTable d = NewDataVariables.Annotations.ToDataTable();
                        this.LoadDataGridView(d, annotation_dgv);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
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
                int visitDateTimeIndx = d.Columns.IndexOf("createdDate");
                DataColumn time = new DataColumn("Time");
                d.Columns.Add(time);
                for (int i = 0; i < d.Rows.Count; i++)
                {
                    d.Rows[i][visitDateTimeIndx] = visitDate[i].ToString("dd-MMM-yyyy");
                    ///This code has been added by Darshan on 14-08-2015 6:20 PM to solve Defect no 0000553: Time settings are not reflecting correctly.
                    //if (Convert.ToBoolean(IVLVariables.CurrentSettings.UserSettings._Is24clock.val))
                    //{
                    d.Rows[i]["Time"] = visitDate[i].ToString(" HH:mm ");
                    //}
                    int visitId = Convert.ToInt32(d.Rows[i]["visitId"]);
                    visit visit = NewDataVariables._Repo.GetById<visit>(visitId);
                    ImageNumbers.Add(Convert.ToInt32(NewDataVariables._Repo.GetByCategory<obs>("visit", visit).Where(x => x.voided == false).Count()));
                    ReportNumbers.Add(Convert.ToInt32(NewDataVariables._Repo.GetByCategory<report>("visit", visit).Count));
                }
                //d.Columns.RemoveAt(d.Columns.IndexOf("NoOfReports"));
                //d.Columns.RemoveAt(d.Columns.IndexOf("NoOfImages"));
                d.Columns["createdDate"].ColumnName = "Date";
                d.Columns["Time"].ColumnName = "Time";
                //d.Columns["NoOfImages"].ColumnName = IVLVariables.LangResourceManager.GetString( "No_of_Images_Text",IVLVariables.LangResourceCultureInfo);
                // d.Columns["NoOfReports"].ColumnName = IVLVariables.LangResourceManager.GetString( "No_of_Reports_Text",IVLVariables.LangResourceCultureInfo);

            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
        }


        private void SetVisitColumnVisibilty()
        {
            try
            {
                if (this.Visible)
                {
                    //visitsView_dgv.Refresh();
                    for (int i = 0; i < visitsView_dgv.Columns.Count; i++)
                    {
                        if (!visitsView_dgv.Columns[i].Name.Equals("visitId") && !visitsView_dgv.Columns[i].Name.Equals("Date") && !visitsView_dgv.Columns[i].Name.Equals("Time") && !visitsView_dgv.Columns[i].Name.Contains("# Reports") && !visitsView_dgv.Columns[i].Name.Contains("# Images"))
                            visitsView_dgv.Columns[i].Visible = false;
                        else
                        {
                            visitsView_dgv.Columns[i].Visible = true;
                            //This code has been added by Darshan on 13-08-2015 7:00 PM to solve Defect no 0000553: Time settings are not reflecting correctly.
                        }
                        visitsView_dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    //DataGridViewDisableButtonColumn col;
                    //The below code has been added to add link in place of text.
                    if (!visitsView_dgv.Columns.Contains("# Reports") && !visitsView_dgv.Columns.Contains("# Images"))
                    {
                        DataGridViewLinkColumn column = new DataGridViewLinkColumn();// visitsView_dgv.Columns[IVLVariables.LangResourceManager.GetString( "No_of_Reports_Text];
                        column.Name = "# Reports";
                        visitsView_dgv.Columns.Add(column);
                        column = new DataGridViewLinkColumn();// visitsView_dgv.Columns[IVLVariables.LangResourceManager.GetString( "No_of_Reports_Text];
                        column.Name = "# Images";
                        visitsView_dgv.Columns.Add(column);
                    }

                    foreach (DataGridViewRow item in visitsView_dgv.Rows)
                    {
                        string dateVal = item.Cells["Date"].Value.ToString();
                        DateTime day = Convert.ToDateTime(dateVal);
                        DataGridViewLinkCell linCell = new DataGridViewLinkCell();
                        linCell.Value = ReportNumbers[visitsView_dgv.Rows.IndexOf(item)].ToString();
                        item.Cells["# Reports"] = linCell;
                        linCell = new DataGridViewLinkCell();
                        linCell.Value = ImageNumbers[visitsView_dgv.Rows.IndexOf(item)].ToString();
                        item.Cells["# Images"] = linCell;
                    }
                    visitsView_dgv.Refresh();
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }


        public void SetObsData(DataTable d)
        {
            try
            {
                d.Columns["ObservationId"].ColumnName = "ObservationId";
                d.Columns["ObservationId"].SetOrdinal(1);



                DataColumn visitCol = new DataColumn("VisitId");
                d.Columns.Add(visitCol);
                for (int i = 0; i < d.Rows.Count; i++)
                {
                    visit v = (visit)(NewDataVariables.Visit_Obs[i].visit);
                    d.Rows[i]["VisitId"] = v.visitId;
                }
                d.Columns["VisitId"].SetOrdinal(3);

                DataColumn c = new DataColumn("PersonId");
                d.Columns.Add(c);
                for (int i = 0; i < d.Rows.Count; i++)
                {
                    Person p = (Person)(NewDataVariables.Visit_Obs[i].patient);
                    d.Rows[i]["PersonId"] = p.personId;
                }
                d.Columns["PersonId"].SetOrdinal(2);

                //d.Columns["visit"].ColumnName = "VisitId";
                //d.Columns["VisitId"].SetOrdinal(3);

                //DataColumn c = new DataColumn("PersonId");
                //d.Columns.Add(c);
                //for (int i = 0; i < d.Rows.Count; i++)
                //{
                //    Person dt = (d.Rows[i]["person_id"]) as Person;
                //    d.Rows[i]["PersonId"] = dt.personId;
                //}
                //d.Columns["PersonId"].SetOrdinal(2);



                d.Columns["value"].ColumnName = "Value";
                d.Columns["Value"].SetOrdinal(4);
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            

        }

        private void SetObsColumnVisibilty()
        {
            try
            {
                if (this.Visible)
                {
                    //visitsView_dgv.Refresh();
                    for (int i = 0; i < Obs_dgv.Columns.Count; i++)
                    {
                        if (!Obs_dgv.Columns[i].Name.Equals("ObservationId") && !Obs_dgv.Columns[i].Name.Equals("PersonId") && !Obs_dgv.Columns[i].Name.Equals("VisitId") && !Obs_dgv.Columns[i].Name.Equals("Value"))
                            Obs_dgv.Columns[i].Visible = false;
                        else
                        {
                            Obs_dgv.Columns[i].Visible = true;
                            //This code has been added by Darshan on 13-08-2015 7:00 PM to solve Defect no 0000553: Time settings are not reflecting correctly.
                        }
                        Obs_dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    //DataGridViewDisableButtonColumn col;
                    //The below code has been added to add link in place of text.
                    Obs_dgv.Refresh();
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        public void SetReportData(DataTable d)
        {
            try
            {
                d.Columns["reportId"].ColumnName = "ReportId";
                d.Columns["ReportId"].SetOrdinal(1);



                DataColumn visitCol = new DataColumn("VisitId");
                d.Columns.Add(visitCol);
                for (int i = 0; i < d.Rows.Count; i++)
                {
                    visit v = (visit)(NewDataVariables.Reports[i].visit);
                    d.Rows[i]["VisitId"] = v.visitId;
                }
                d.Columns["VisitId"].SetOrdinal(3);

                DataColumn c = new DataColumn("PersonId");
                d.Columns.Add(c);
                for (int i = 0; i < d.Rows.Count; i++)
                {
                    Person p = (Person)(NewDataVariables.Reports[i].Patient);
                    d.Rows[i]["PersonId"] = p.personId;
                }
                d.Columns["PersonId"].SetOrdinal(2);

            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
            //d.Columns["value"].ColumnName = "Value";
            //d.Columns["Value"].SetOrdinal(4);

        }

        private void SetReportColumnVisibilty()
        {
            try
            {
                if (this.Visible)
                {
                    //visitsView_dgv.Refresh();
                    for (int i = 0; i < report_dgv.Columns.Count; i++)
                    {
                        if (!report_dgv.Columns[i].Name.Equals("ReportId") && !report_dgv.Columns[i].Name.Equals("PersonId") && !report_dgv.Columns[i].Name.Equals("VisitId") && !report_dgv.Columns[i].Name.Equals("Value"))
                            report_dgv.Columns[i].Visible = false;
                        else
                        {
                            report_dgv.Columns[i].Visible = true;
                            //This code has been added by Darshan on 13-08-2015 7:00 PM to solve Defect no 0000553: Time settings are not reflecting correctly.
                        }
                        report_dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    //DataGridViewDisableButtonColumn col;
                    //The below code has been added to add link in place of text.
                    report_dgv.Refresh();
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void updatePatGridView()
        {
            try
            {
                {
                    if (patGridView_dgv.SelectedRows.Count > 0)
                    {
                        int id = Convert.ToInt32(patGridView_dgv.SelectedRows[0].Cells["personId"].Value.ToString());
                        NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0] = NewDataVariables.Patients.Find(x => x.personId == id);
                        //NewPatient NewPatient_Db = NewDataVariables.Patients.Find(x => x.personId == id);
                        NewDataVariables.Active_PatientIdentifier = NewDataVariables.Identifier.Find(x => x.patient.personId == id);
                        NewDataVariables.Active_PersonAddressModel = NewDataVariables.Address.Find(x => x.person.personId == id);
                        ShowIndividualPatDetails();
                        ////MessageBox.Show(time.ToString());
                        int value = patGridView_dgv.Rows.Count;
                        //currentIndx = patGridView_dgv.SelectedRows[0].Index;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void updateAnnotationGridView()
        {
            try
            {
                {
                    if (Obs_dgv.SelectedRows.Count > 0)
                    {
                        int id = Convert.ToInt32(Obs_dgv.SelectedRows[0].Cells["observationId"].Value.ToString());
                        NewDataVariables.Active_Obs = NewDataVariables.Visit_Obs.Find(x => x.observationId == id);
                        //NewPatient NewPatient_Db = NewDataVariables.Patients.Find(x => x.personId == id);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void patGridView_dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            updatePatGridView();
        }

        private void Obs_dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowIndividualObservationDetails();
        }
    }
}
