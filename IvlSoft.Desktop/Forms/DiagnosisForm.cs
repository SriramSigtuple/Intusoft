using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using INTUSOFT.Data.Repository;
using INTUSOFT.Data.NewDbModel;
using INTUSOFT.Desktop.Enums;
using INTUSOFT.Custom.Controls;
namespace INTUSOFT.Desktop.Forms
{

    enum DiagnosisSide { Left, Right };
    enum PopulateDiagnosis { FromUI, FromDB };

    public partial class DiagnosisForm : BaseGradientForm
    {
        DiagnosisSide diagnosisSide;
        string ConsolidatedDiagnosisLeft = string.Empty;
        string ConsolidatedDiagnosisRight = string.Empty;
        string diagnosis = string.Empty;
        bool isFormLoad = true;
        bool isDiagnosisClear = false;

        string saveLogoPath = @"ImageResources\Edit_ImageResources\SaveIcon.png";
        string cancelLogoPath = @"ImageResources\Edit_ImageResources\cancel.png";
        string clearLogoPath = @"ImageResources\Edit_ImageResources\clear.png";
        Diagnosis diagnosisClass;
        public DiagnosisForm()
        {
            InitializeComponent();
            diagnosisClass = Diagnosis.GetInstance();
            //DiagnosisEnum[] Diagnosis_Enum_values = Enum.GetValues(typeof(DiagnosisEnum)) as DiagnosisEnum[];
            //Diagnosis_description_values = Diagnosis_Enum_values.OfType<object>().Select(o => o.ToString()).ToArray();
            char singleQuoteChar = char.Parse("'");

            //for (int i = 0; i < Diagnosis_description_values.Count; i++)
            //{
            //    //if (Diagnosis_description_values[i].Contains('_'))
            //        //Diagnosis_description_values[i] = INTUSOFT.Data.Common.GetDescription(Diagnosis_Enum_values[i]);
            //    Diagnosis_description_values[i] = Diagnosis_description_values[i].Replace('_', ' ');
            //    Diagnosis_description_values[i] = Diagnosis_description_values[i].Replace('9', singleQuoteChar);
            //}
            //diagnosisArr2 = new string[Diagnosis_description_values.Count];
            //Array.Copy(Diagnosis_description_values, diagnosisArr2, diagnosisArr2.Count);
            rightDiagnosis_cbx.DataSource = diagnosisClass.diagnosisArr2;// Diagnosis_description_values;// This added to fix the defect 0001863 by sriram
            leftDiagnosis_cbx.DataSource = diagnosisClass.diagnosisArr2;
            leftEye_lbl.Text = IVLVariables.LangResourceManager.GetString("LeftEyeDiagnosis_Lbl_Text", IVLVariables.LangResourceCultureInfo);
            rightEye_lbl.Text = IVLVariables.LangResourceManager.GetString("RighttEyeDiagnosis_Lbl_Text", IVLVariables.LangResourceCultureInfo);
            leftEyeRegion_lbl.Text = IVLVariables.LangResourceManager.GetString("LeftEyeDiagnosisRegion_Lbl_Text", IVLVariables.LangResourceCultureInfo);
            rightEyeRegion_lbl.Text = IVLVariables.LangResourceManager.GetString("RightEyeDiagnosisRegion_Lbl_Text", IVLVariables.LangResourceCultureInfo);
            saveDiagnosis_btn.Text = IVLVariables.LangResourceManager.GetString("SaveHeader_Text", IVLVariables.LangResourceCultureInfo);
            cancelDiagnosis_btn.Text = IVLVariables.LangResourceManager.GetString("Advacesearch_cancelbtn_text", IVLVariables.LangResourceCultureInfo);
            clearDiagnosis_btn.Text = IVLVariables.LangResourceManager.GetString("Clear_Button_Text", IVLVariables.LangResourceCultureInfo);
            if (File.Exists(saveLogoPath))
                saveDiagnosis_btn.Image = Image.FromFile(saveLogoPath);
            if (File.Exists(cancelLogoPath))
                cancelDiagnosis_btn.Image = Image.FromFile(cancelLogoPath);
            if (File.Exists(clearLogoPath))
                clearDiagnosis_btn.Image = Image.FromFile(clearLogoPath);
            this.main_tbl.RowStyles[0] = new RowStyle(SizeType.Percent, 10f);
            this.main_tbl.RowStyles[1] = new RowStyle(SizeType.Percent, 0f);
            this.main_tbl.RowStyles[2] = new RowStyle(SizeType.Percent, 15f);
            this.main_tbl.RowStyles[3] = new RowStyle(SizeType.Percent, 55f);
            this.main_tbl.RowStyles[4] = new RowStyle(SizeType.Percent, 20f);
            UpdateColor();
            toolStrip1.Renderer = new FormToolStripRenderer();
        }

        private void UpdateColor()
        {
            this.Color1 = IVLVariables.GradientColorValues.Color1;
            this.Color2 = IVLVariables.GradientColorValues.Color2;
            this.ColorAngle = IVLVariables.GradientColorValues.ColorAngle;
            //this.FontColor = IVLVariables.GradientColorValues.FontForeColor;

            List<Control> controls = GetControls(this).ToList();
            foreach (Control c in controls)
            {
                if (c is ComboBox || c is FlowLayoutPanel)
                {
                    continue;
                }
                else
                    c.ForeColor = IVLVariables.GradientColorValues.FontForeColor;
            }
        }

        private void addDiagnosis_btn_Click(object sender, EventArgs e)
        {
            //AddDiagnosis(true);
        }
        void AddDiagnosis(PopulateDiagnosis diagnosisFrom, DiagnosisSide side)
        {
            if (!isFormLoad)
            {
             
                string diagnosisStr = string.Empty;
                if (side == DiagnosisSide.Left)// if side is left, add diagnosis in left side
                {
                    if (diagnosisFrom == PopulateDiagnosis.FromUI)//the population is from user
                    {
                        if (leftDiagnosis_cbx.SelectedItem == null || leftDiagnosis_cbx.SelectedItem.ToString() == DiagnosisEnum.None.ToString() )
                        {
                            if( string.IsNullOrEmpty(leftDiagnosis_cbx.Text))
                                return;
                            else
                                diagnosisClass.diagnosisArr2.Add(leftDiagnosis_cbx.Text);
                        }
                        //diagnosisStr = leftDiagnosis_cbx.SelectedItem.ToString();
                        diagnosisStr = leftDiagnosis_cbx.Text;
                        saveDiagnosis_btn.Enabled = true;
                        clearDiagnosis_btn.Enabled = true;
                    }
                    else // the population is from database
                        diagnosisStr = diagnosis;
                }
                else //add diagnosis in right side
                {
                    if (diagnosisFrom == PopulateDiagnosis.FromUI)//the population is from user
                    {
                        if (rightDiagnosis_cbx.SelectedItem == null || rightDiagnosis_cbx.SelectedItem.ToString() == DiagnosisEnum.None.ToString() )
                        {
                            if( string.IsNullOrEmpty(rightDiagnosis_cbx.Text))
                                return;
                            else
                                diagnosisClass.diagnosisArr2.Add(rightDiagnosis_cbx.Text);
                        }
                        //diagnosisStr = rightDiagnosis_cbx.SelectedItem.ToString();
                        diagnosisStr = rightDiagnosis_cbx.Text;
                        saveDiagnosis_btn.Enabled = true;
                        clearDiagnosis_btn.Enabled = true;
                    }
                    else // the population is from database
                        diagnosisStr = diagnosis;
                }
                if (diagnosisStr != DiagnosisEnum.None.ToString())
                {
                    DiagnosisUC d = new DiagnosisUC(diagnosisStr);
                    d.RemoveDiagnosisEvent += d_RemoveDiagnosisEvent;
                    if (side == DiagnosisSide.Left)
                    {
                        if (IsDiagnosisExist(leftEye_flp, diagnosisStr))
                            this.leftEye_flp.Controls.Add(d);
                    }
                    else
                    {
                        if (IsDiagnosisExist(rightEye_flp, diagnosisStr))
                            this.rightEye_flp.Controls.Add(d);
                    }
                    cbx1_lbx.Visible = false;
                    cbx2_lbx.Visible = false;
                }
            }
        }

        private bool IsDiagnosisExist(FlowLayoutPanel flp, string diagnosisStr)
        {
            List<DiagnosisUC> diagnosisUCList = flp.Controls.OfType<DiagnosisUC>().ToList();
          diagnosisUCList =  diagnosisUCList.Where(x => x.DiagnosisLblText == diagnosisStr).ToList();
            int count = diagnosisUCList.Count;
            if (count == 0)
                return true;
            else
               return false;
          //return c.ContainsKey(diagnosisStr);
        }

        void d_RemoveDiagnosisEvent(DiagnosisUC val)
        {
            if (val.Parent == this.leftEye_flp)
                this.leftEye_flp.Controls.Remove(val);
            else
                this.rightEye_flp.Controls.Remove(val);
            if (this.rightEye_flp.Controls.Count < 1 && this.leftEye_flp.Controls.Count < 1)
            {
                saveDiagnosis_btn.Enabled = false;
                clearDiagnosis_btn.Enabled = false;
            }
        }
        void ComputeConsolidateDiagnosis()
        {
            DiagnosisStructure dsL = new DiagnosisStructure();
            foreach (var item in this.leftEye_flp.Controls)
            {
                DiagnosisUC c = item as DiagnosisUC;
                dsL.DiagnosisList.Add(c.GetDiagnosis());
            }
            //ConsolidatedDiagnosisLeft = JsonConvert.SerializeObject(dsL);
            ConsolidatedDiagnosisLeft = string.Empty;
            ConsolidatedDiagnosisRight = string.Empty;
            for (int i = 0; i < dsL.DiagnosisList.Count; i++)
            {
                ConsolidatedDiagnosisLeft += dsL.DiagnosisList[i] + ",";
            }
            ConsolidatedDiagnosisLeft = ConsolidatedDiagnosisLeft.TrimEnd(',');
            DiagnosisStructure dsR = new DiagnosisStructure();
            foreach (var item in this.rightEye_flp.Controls)
            {
                DiagnosisUC c = item as DiagnosisUC;
                dsR.DiagnosisList.Add(c.GetDiagnosis());
            }
            //ConsolidatedDiagnosisRight = JsonConvert.SerializeObject(dsR);
            for (int i = 0; i < dsR.DiagnosisList.Count; i++)
            {
                ConsolidatedDiagnosisRight += dsR.DiagnosisList[i] + ",";
            }
            ConsolidatedDiagnosisRight = ConsolidatedDiagnosisRight.TrimEnd(',');
          
        }

        private void GetDiagnosis()
        {
            //ComputeConsolidateDiagnosis();
            this.leftEye_flp.Controls.Clear();
            this.rightEye_flp.Controls.Clear();
            NewDataVariables.PatientDiagnosis =  NewDataVariables.PatientDiagnosis.Where(x => x.voided == false).ToList();
            if (NewDataVariables.PatientDiagnosis != null && NewDataVariables.PatientDiagnosis.Count > 0 )
            {
                string leftDiagnosis = string.Empty;
                DiagnosisStructure dL = new DiagnosisStructure();
                for (int i = 0; i < NewDataVariables.PatientDiagnosis.Count; i++)
                {
                    leftDiagnosis = NewDataVariables.PatientDiagnosis[i].diagnosisValueLeft;
                    //dL = (DiagnosisStructure)Newtonsoft.Json.JsonConvert.DeserializeObject(leftDiagnosis, typeof(DiagnosisStructure));
                    if (!string.IsNullOrEmpty(leftDiagnosis))
                    {
                        dL.DiagnosisList = leftDiagnosis.Split(',').ToList();
                        for (int j = 0; j < dL.DiagnosisList.Count; j++)
                        {
                            //DiagnosisUC l = new DiagnosisUC(dL.DiagnosisList[j]);
                            //l.RemoveDiagnosisEvent += l_RemoveDiagnosisEvent;
                            ////l.Text = dL.DiagnosisList[j];
                            //l.AutoSize = false;
                            ////l.AutoEllipsis = true;
                            //leftEye_flp.Controls.Add(l);
                            diagnosis = dL.DiagnosisList[j];
                            AddDiagnosis(PopulateDiagnosis.FromDB, DiagnosisSide.Left);
                        }
                    }
                }
                string rightDiagnosis = string.Empty;
                DiagnosisStructure dR = new DiagnosisStructure();
                for (int i = 0; i < NewDataVariables.PatientDiagnosis.Count; i++)
                {
                    rightDiagnosis = NewDataVariables.PatientDiagnosis[i].diagnosisValueRight;
                    //dR = (DiagnosisStructure)Newtonsoft.Json.JsonConvert.DeserializeObject(rightDiagnosis, typeof(DiagnosisStructure));
                    if (!string.IsNullOrEmpty(rightDiagnosis))
                    {
                        dR.DiagnosisList = rightDiagnosis.Split(',').ToList();
                        for (int j = 0; j < dR.DiagnosisList.Count; j++)
                        {
                            //DiagnosisUC l = new DiagnosisUC(dR.DiagnosisList[j]);
                            //l.RemoveDiagnosisEvent += r_RemoveDiagnosisEvent;
                            ////l.Text = dR.DiagnosisList[j];
                            //l.AutoSize = false;
                            ////l.AutoEllipsis = true;
                            //rightEye_flp.Controls.Add(l);
                            diagnosis = dR.DiagnosisList[j];
                            AddDiagnosis(PopulateDiagnosis.FromDB, DiagnosisSide.Right);
                        }
                    }
                }
            }
        }

        void r_RemoveDiagnosisEvent(DiagnosisUC val)
        {
            this.rightEye_flp.Controls.Remove(val);
        }

        void l_RemoveDiagnosisEvent(DiagnosisUC val)
        {
            this.leftEye_flp.Controls.Remove(val);
        }



        private void diagnosis_cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
                //AddDiagnosis(true, true);
        }

        private void saveDiagnosis_btn_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            saveDiagnosis_btn.Enabled = false;
            SaveDiagnosis();
            Common.CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("DiagnosisSaveText", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("SaveHeader_Text", IVLVariables.LangResourceCultureInfo), Common.CustomMessageBoxButtons.OK, Common.CustomMessageBoxIcon.Information);
            this.Cursor = Cursors.Default;
            this.Close();
        }



        private void cancelDiagnosis_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void diagnosis_cbx2_SelectedIndexChanged(object sender, EventArgs e)
        {
                //AddDiagnosis(true,false);
        }

        private void DiagnosisForm_Load(object sender, EventArgs e)
        {
            isFormLoad = false;
            if (NewDataVariables.PatientDiagnosis != null && NewDataVariables.PatientDiagnosis.Count > 0)
            {
                GetDiagnosis();
                clearDiagnosis_btn.Enabled = true;
            }
            else
            {
                saveDiagnosis_btn.Enabled = false;
                clearDiagnosis_btn.Enabled = false;
            }
        }


        //public void AutoComplete(ListBox lb, ComboBox cb, System.Windows.Forms.KeyPressEventArgs e)
        //{
        //    //this.AutoComplete(cb, e, false);
        //    lb.
            
        //}

        //public void AutoComplete(ComboBox cb, System.Windows.Forms.KeyPressEventArgs e, bool blnLimitToList)
        //{
        //    string strFindStr = "";
        //    if (e.KeyChar == (char)8)
        //    {
        //        if (cb.SelectionStart <= 1)
        //        {
        //            cb.Text = "";
        //            return;
        //        }
        //        if (cb.SelectionLength == 0)
        //            strFindStr = cb.Text.Substring(0, cb.Text.Length - 1);
        //        else
        //            strFindStr = cb.Text.Substring(0, cb.SelectionStart - 1);
        //    }
        //    else
        //    {
        //        if (cb.SelectionLength == 0)
        //            strFindStr = cb.Text + e.KeyChar;
        //        else
        //            strFindStr = cb.Text.Substring(0, cb.SelectionStart) + e.KeyChar;
        //    }
        //    int intIdx = -1;
        //    // Search the string in the ComboBox list.
        //    intIdx = cb.FindString(strFindStr);
        //    if (intIdx != -1)
        //    {
        //        cb.SelectedText = "";
        //        cb.SelectedIndex = intIdx;
        //        cb.SelectionStart = strFindStr.Length;
        //        cb.SelectionLength = cb.Text.Length;
        //        e.Handled = true;
        //    }
        //    else
        //    {
        //        e.Handled = blnLimitToList;
        //    }
        //}


        char semiColonCharacter = ';';
        private void getSearchStringFromtbx(ListBox lb, ComboBox c, KeyEventArgs e)
        {
            lb.BringToFront();
            lb.ClearSelected();

            string searchString2 = c.Text;

            if (string.IsNullOrEmpty(searchString2))// if search string is empty hide the list box of mail ids
                lb.Visible = false;
            else
                FindMyStringInList(c, lb, searchString2);// get the actual mail id from the search string and highlight it in the list box
        }


        private void SaveDiagnosis()
        {
            ComputeConsolidateDiagnosis();
            PatientDiagnosis p = new PatientDiagnosis();
            p.diagnosisValueLeft = ConsolidatedDiagnosisLeft;
            p.diagnosisValueRight = ConsolidatedDiagnosisRight;
            p.createdDate = DateTime.Now;
            p.lastModifiedDate = DateTime.Now;
            Patient pat = NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0];
            p.patient = pat;
            p.visit = NewDataVariables.Active_Visit;
            p.concept = NewDataVariables._Repo.GetById<Concept>(12);
            if (isDiagnosisClear)
            {
                for (int i = 0; i < pat.diagnosis.Count; i++)
                {
                    if (pat.diagnosis.ToList()[i].voided != true)
                        pat.diagnosis.ToList()[i].voided = true;
                }
            }
            else
            {
                pat.diagnosis.Add(p);
            }
            if (pat.diagnosis.Count > 0)
            {
                NewDataVariables.PatientDiagnosis = pat.diagnosis.ToList();
                NewDataVariables.Patients[NewDataVariables.Patients.FindIndex(x => x.personId == NewDataVariables.Active_Patient)] = pat;
                NewIVLDataMethods.UpdatePatient();
            }
        }

        /// <summary>
        /// to search the string entered in to textbox is already in the listbox 
        /// </summary>
        /// <param name="lb"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        void FindMyStringInList(ComboBox c, ListBox lb, string searchString)
        {

            List<string> lisBoxList = new List<string>();
            //lb.DataSource = diagnosis_cbx.DataSource;
            for (int i = 0; i < c.Items.Count; ++i)
            {
                string lbString = c.Items[i].ToString();
                if (lbString.ToLower().Contains(searchString.ToLower()))//checks the searchstring is there or not
                {
                    //lb.Visible = true;
                    lisBoxList.Add(c.Items[i].ToString());

                    //return i;
                }
                //else
                //    lb.Visible = false;
            }
            if (lisBoxList.Count > 0)
            {
                lb.DataSource = lisBoxList;
                lb.Visible = true;
            }
            else
                lb.Visible = false;
            //return -1;
        }


        private void cbx1_lbx_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            leftDiagnosis_cbx.Text = cbx1_lbx.SelectedItem.ToString();
        }

        private void cbx2_lbx_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            rightDiagnosis_cbx.Text = cbx2_lbx.SelectedItem.ToString();
        }

        private void clearDiagnosis_btn_Click(object sender, EventArgs e)
        {
            if (Common.CustomMessageBox.Show((IVLVariables.LangResourceManager.GetString("Diagnosis_Clear_Text", IVLVariables.LangResourceCultureInfo) + Environment.NewLine + IVLVariables.LangResourceManager.GetString("Diagnosis_Warning_Text",IVLVariables.LangResourceCultureInfo)), IVLVariables.LangResourceManager.GetString("Diagnosis_Lbl_Text", IVLVariables.LangResourceCultureInfo), Common.CustomMessageBoxButtons.YesNo, Common.CustomMessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                this.rightEye_flp.Controls.Clear();
                this.leftEye_flp.Controls.Clear();
                saveDiagnosis_btn.Enabled = false;
                clearDiagnosis_btn.Enabled = false;
                isDiagnosisClear = true;
                SaveDiagnosis();
                isDiagnosisClear = false;
            }
            
        }

        private void leftDiagnosis_cbx_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddDiagnosis(PopulateDiagnosis.FromUI, DiagnosisSide.Left);
            }
            else if (e.KeyCode == Keys.ControlKey && e.KeyCode == Keys.ShiftKey)
                e.Handled = true;
            else
                this.getSearchStringFromtbx(cbx1_lbx, leftDiagnosis_cbx, e);
        }

        private void rightDiagnosis_cbx_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                AddDiagnosis(PopulateDiagnosis.FromUI, DiagnosisSide.Right);
            else if (e.KeyCode == Keys.ControlKey && e.KeyCode == Keys.ShiftKey)
                e.Handled = true;
            else
                this.getSearchStringFromtbx(cbx2_lbx, rightDiagnosis_cbx, e);
        }

        private void DiagnosisForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!saveDiagnosis_btn.Enabled)
            {
                diagnosisClass.SerializeDiagnosis();
                IVLVariables.IsAnotherWindowOpen = false;
            }
            else
            {
                DialogResult result = Common.CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("SaveDiagnosisConfirmation_Text", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("SaveHeader_Text", IVLVariables.LangResourceCultureInfo), Common.CustomMessageBoxButtons.YesNo, Common.CustomMessageBoxIcon.Question);
                    if(result == System.Windows.Forms.DialogResult.Yes)
                    {
                        saveDiagnosis_btn.Enabled = false;
                        this.Cursor = Cursors.WaitCursor;
                        SaveDiagnosis();
                        Common.CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("DiagnosisSaveText", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("SaveHeader_Text", IVLVariables.LangResourceCultureInfo), Common.CustomMessageBoxButtons.OK, Common.CustomMessageBoxIcon.Information);
                        this.Cursor = Cursors.Default;
                        this.Close();
                    }
            }
        }

        //private void diagnosis_cbx_MouseClick(object sender, MouseEventArgs e)
        //{
        //    if(!diagnosis_cbx.DroppedDown)
        //    AddDiagnosis(true, true);
        //}

        //private void diagnosis_cbx2_MouseClick(object sender, MouseEventArgs e)
        //{
        //    AddDiagnosis(true, false);
        //}

        //private void diagnosis_cbx_Click(object sender, EventArgs e)
        //{
        //    AddDiagnosis(true, true);
        //    System.Diagnostics.Trace.WriteLine("");
        //}
    }

    [Serializable]
    public class DiagnosisStructure
    {
        public List<string> DiagnosisList;
        public DiagnosisStructure()
        {
            DiagnosisList = new List<string>();
        }
    }

    public class Diagnosis
    {
        static string diagnosisFileName = @"diagnosis.json";
        private static Diagnosis diagnosis;
        public List<string> Diagnosis_description_values;
        public List<string> diagnosisArr2;
        public Diagnosis()
        {

        }

        private static void AddDefaultDiagnosis()
        {
            DiagnosisEnum[] Diagnosis_Enum_values = Enum.GetValues(typeof(DiagnosisEnum)) as DiagnosisEnum[];
            diagnosis.Diagnosis_description_values = Diagnosis_Enum_values.OfType<object>().Select(o => o.ToString()).ToList();
            char singleQuoteChar = char.Parse("'");

            for (int i = 0; i < Diagnosis_Enum_values.Length; i++)
            {
                //if (Diagnosis_description_values[i].Contains('_'))
                //Diagnosis_description_values[i] = INTUSOFT.Data.Common.GetDescription(Diagnosis_Enum_values[i]);
                diagnosis.Diagnosis_description_values[i] = diagnosis.Diagnosis_description_values[i].Replace('_', ' ');
                diagnosis.Diagnosis_description_values[i] = diagnosis.Diagnosis_description_values[i].Replace('9', singleQuoteChar);
            }
            diagnosis.diagnosisArr2 = new List<string>();//[diagnosis.Diagnosis_description_values.Count];
            diagnosis.diagnosisArr2 = diagnosis.Diagnosis_description_values;//Array.Copy(diagnosis.Diagnosis_description_values, diagnosis.diagnosisArr2, diagnosis.diagnosisArr2.Count);

        }

        private static bool Deserialize(string fileName)
        {
            bool returnVal = false;
            if (File.Exists(fileName))
            {
                try
                {
                    string diagnosisStr = File.ReadAllText(fileName);
                    diagnosis = (Diagnosis)JsonConvert.DeserializeObject(diagnosisStr, typeof(Diagnosis));
                }
                catch (Exception)
                {
                    diagnosis = new Diagnosis();

                }
                if (diagnosis.Diagnosis_description_values.Count == 0)
                    returnVal = false;
                else
                    returnVal = true;
            }
            else
                diagnosis = new Diagnosis();

            return returnVal;
        }

        public void SerializeDiagnosis()
        {
            string diagnosisJson = JsonConvert.SerializeObject(diagnosis);
            System.IO.File.WriteAllText(diagnosisFileName, diagnosisJson);
        }

        public static Diagnosis GetInstance()
        {
            if(diagnosis == null)
                if (!Deserialize(diagnosisFileName))
                    AddDefaultDiagnosis();
            return diagnosis;
        }

    }

}
