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
    public partial class Pagination : Form
    {
        private int CurrentPage = 1;
        int PagesCount;
        int pageRows;
        int NoofPageButton = 5;
        int NoofPageToBeShifted = 2;
        int CurrentPageDecementNumber = 2;
        int PageCountDecrementNumber = 4;
        List<INTUSOFT.Data.NewDbModel.Patient> Templist = null;
        public Logger Exception_Log = LogManager.GetLogger("DB_Porting.ExceptionLog");
        public Pagination()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads the data from the database to the datagridview based on current page.
        /// </summary>
        private void RebindGridForPageChange()
        {
            try
            {
                //Rebinding the Datagridview with data
                Templist = NewDataVariables._Repo.GetPageData<INTUSOFT.Data.NewDbModel.Patient>(pageRows, CurrentPage).ToList<INTUSOFT.Data.NewDbModel.Patient>();//Get the data from the database of the CurrentPage and saves into the temp list.
                //patient_dgv.DataSource = Templist;//Sets the patient_dgv datasource to templist.
                DataTable d = Templist.ToDataTable();
                setPatientTableOrder(d);
                LoadDataGridView(d, patient_dgv);
                SetPatientColumnsVisible();
                patient_dgv.Refresh();
            }
            catch (Exception ex)
            {

                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);

            }
           
        }

       /// <summary>
       /// Method that handles the pagination button clicks
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void ToolStripButtonClick(object sender, EventArgs e)
        {
            try
            {
                ToolStripButton ToolStripButton = ((ToolStripButton)sender);
                //Determining the current page
                if (ToolStripButton == btnBackward)
                    CurrentPage--;
                else if (ToolStripButton == btnForward)
                    CurrentPage++;
                else if (ToolStripButton == btnLast)
                    CurrentPage = PagesCount;
                else if (ToolStripButton == btnFirst)
                    CurrentPage = 1;
                else
                    CurrentPage = Convert.ToInt32(ToolStripButton.Text);
                if (CurrentPage < 1)
                    CurrentPage = 1;
                else if (CurrentPage > PagesCount)
                    CurrentPage = PagesCount;

                RebindGridForPageChange();//Rebind the Datagridview with the data.

                RefreshPagination();//Change the pagiantions buttons according to page number
            }
            catch (Exception ex) 
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
        }

        /// <summary>
        /// Refresh the page numbers displayed and shifting of the page numbers.
        /// </summary>
        private void RefreshPagination()
        {
            try
            {
                ToolStripButton[] items = new ToolStripButton[] { toolStripButton1, toolStripButton2, toolStripButton3, toolStripButton4, toolStripButton5 };

                //pageStartIndex contains the first button number of pagination.
                int pageStartIndex = 1;
                if (PagesCount > NoofPageButton && CurrentPage > NoofPageToBeShifted)
                    pageStartIndex = CurrentPage - CurrentPageDecementNumber;

                if (PagesCount > NoofPageButton && CurrentPage > PagesCount - NoofPageToBeShifted)
                    pageStartIndex = PagesCount - PageCountDecrementNumber;

                for (int i = pageStartIndex; i < pageStartIndex + NoofPageButton; i++)
                {
                    if (i > PagesCount)
                    {
                        items[i - pageStartIndex].Visible = false;
                    }
                    else
                    {
                        //Changing the page numbers
                        items[i - pageStartIndex].Text = i.ToString();

                        //Setting the Appearance of the page number buttons
                        if (i == CurrentPage)
                        {
                            items[i - pageStartIndex].BackColor = Color.Black;
                            items[i - pageStartIndex].ForeColor = Color.White;
                        }
                        else
                        {
                            items[i - pageStartIndex].BackColor = Color.White;
                            items[i - pageStartIndex].ForeColor = Color.Black;
                        }
                    }
                }

                //Enabling or Disalbing pagination first, last, previous , next buttons
                if (CurrentPage == 1)
                    btnBackward.Enabled = btnFirst.Enabled = false;
                else
                    btnBackward.Enabled = btnFirst.Enabled = true;

                //Enabling or Disalbing pagination first, last, previous , next buttons
                if (CurrentPage == PagesCount)
                    btnForward.Enabled = btnLast.Enabled = false;
                else
                    btnForward.Enabled = btnLast.Enabled = true;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        /// <summary>
        /// Does the necessary functionality required during the loding of the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PortingForm_Load(object sender, EventArgs e)
        {
            try
            {
                pageRows = Convert.ToInt32(IVLVariables._ivlConfig.UserSettings._noOfPatientsPerPage.val);//Assigns no of patients to be displayed
                PagesCount = Convert.ToInt32(Math.Ceiling(NewDataVariables.Patients.Count * 1.0 / pageRows));
                CurrentPage = 1;
                RefreshPagination();
                RebindGridForPageChange();
                INTUSOFT.Data.NewDbModel.Patient patient = NewDataVariables._Repo.GetById<INTUSOFT.Data.NewDbModel.Patient>(4);
                System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
                st.Start();
                List<visit> visits = NewDataVariables._Repo.GetByCategory<visit>("patient", patient).ToList<visit>();
                MessageBox.Show(st.ElapsedMilliseconds.ToString());
                st.Stop();
                System.Diagnostics.Stopwatch sto = new System.Diagnostics.Stopwatch();
                sto.Start();
                List<obs> obss = NewDataVariables._Repo.GetByCategory<obs>("visit", visits[1]).ToList<obs>();
                MessageBox.Show(sto.ElapsedMilliseconds.ToString());
                sto.Stop();
                System.Diagnostics.Stopwatch str = new System.Diagnostics.Stopwatch();
                str.Start();
                List<report> reports = NewDataVariables._Repo.GetByCategory<report>("visit", visits[1]).ToList<report>();
                MessageBox.Show(str.ElapsedMilliseconds.ToString());
                str.Stop();
            }
            catch (Exception ex)
            {
                
                 Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);

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

        /// <summary>
        /// This fucnction is used to set exisiting patient details into the patient datagridview.
        /// </summary>
        private void SetPatientColumnsVisible()
        {
            try
            {
                for (int i = 0; i < patient_dgv.Columns.Count; i++)
                {
                    if (!patient_dgv.Columns[i].Name.Contains("MRN") && !patient_dgv.Columns[i].Name.Contains("FirstName") && !patient_dgv.Columns[i].Name.Contains("LastName")
                        && !patient_dgv.Columns[i].Name.Contains("Gender") && !patient_dgv.Columns[i].Name.Contains("Age"))
                        patient_dgv.Columns[i].Visible = false;
                    else
                    {
                        patient_dgv.Columns[i].Visible = true;
                        if (patient_dgv.Columns[i].Name.Contains("FirstName"))
                        {
                            //patient_dgv.Columns[i].HeaderText = "First Name";
                            patient_dgv.Columns[i].HeaderText = "FirstName";
                        }
                        else if (patient_dgv.Columns[i].Name.Contains("LastName"))
                        {
                            //patient_dgv.Columns[i].HeaderText = "Last Name";
                            patient_dgv.Columns[i].HeaderText = "LastName";
                        }
                        else if (patient_dgv.Columns[i].Name.Contains("Age"))
                        {
                            patient_dgv.Columns[i].HeaderText = "Age";
                        }
                        else if (patient_dgv.Columns[i].Name.Contains("Gender"))
                        {
                            patient_dgv.Columns[i].HeaderText = "Gender";
                        }
                    }
                }
                for (int i = 0; i < patient_dgv.Columns.Count; i++)
                {
                    patient_dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                if (Screen.PrimaryScreen.Bounds.Width == 1280)
                {
                    patient_dgv.Columns["FirstName"].Width = 130;
                    patient_dgv.Columns["LastName"].Width = 131;
                    patient_dgv.Columns["Gender"].Width = 75;
                    patient_dgv.Columns["Age"].Width = 60;
                    patient_dgv.Columns["MRN"].Width = 85;
                }
                else
                    if (Screen.PrimaryScreen.Bounds.Width == 1366)
                    {
                        patient_dgv.Columns["FirstName"].Width = 140;
                        patient_dgv.Columns["LastName"].Width = 143;
                        patient_dgv.Columns["Gender"].Width = 85;
                        patient_dgv.Columns["Age"].Width = 60;
                        patient_dgv.Columns["MRN"].Width = 85;
                    }
                    else
                    {
                        patient_dgv.Columns["FirstName"].Width = 220;
                        patient_dgv.Columns["LastName"].Width = 220;
                        patient_dgv.Columns["Gender"].Width = 100;
                        patient_dgv.Columns["Age"].Width = 83;
                        patient_dgv.Columns["MRN"].Width = 105;
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
                int count = patient_dgv.Columns.Count;
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

        private void btnForward_MouseHover(object sender, EventArgs e)
        {
            btnForward.AutoToolTip = false;
            btnForward.ToolTipText = "Next Page";
        }

        private void btnBackward_MouseHover(object sender, EventArgs e)
        {
            btnBackward.AutoToolTip = false;
            btnBackward.ToolTipText = "Previous Page";
        }
    }
}
