using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Validators;

namespace Common
{
    public partial class CustomFolderBrowser : Form
    {
       static CustomFolderBrowser _customFolderBrowser;
       public string folderPath = string.Empty;
       public string imageFormat = string.Empty;
       public int compressionRatio = 70;
       public static bool export = false;

       public  delegate void okbutton();
       public static event okbutton ImageSavingbtn;
       public static string[] fileNames; // string array declared to handles mutiple files.By Ashutosh 24-7-2017
       
       public  delegate void cancelButton();
       public static event cancelButton CancelButtonClickedEvent;
       public string returnPath = "";

       public static string fileName = "";
       public static string filePath = "";
        private Dictionary<string, object> customFolderData;
        FolderPathErrorCode errCode;
       public Dictionary<string, object> CustomFolderData
       {
           get { return customFolderData; }
           set {
               customFolderData = value;
               InitCustomerFolder();

           }
       }
       public bool isReportExport = false;
       public string[] comboBoxCollection;
        /// <summary>
        /// to initialize the CustomFolder values
        /// </summary>
        private void InitCustomerFolder()
        {
            InitializeResourceString();
            string appLogoFilePath = @"ImageResources\LogoImageResources\IntuSoft.ico";
            if (File.Exists(appLogoFilePath))
                this.Icon = new System.Drawing.Icon(appLogoFilePath, 256, 256);
            if (CustomFolderData.ContainsKey("_FolderPathTextboxText"))
            {
                    folder_tbx.Text = CustomFolderData["_FolderPathTextboxText"] as string;
            }
            else
                folder_tbx.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            fileName_tbx.Text = fileName;
            if (CustomFolderData.ContainsKey("ImageFormat"))
            comboBoxCollection = (CustomFolderData["ImageFormat"] as string).Split(','); //IVLVariables.CurrentSettings.ImageStorageSettings._ImageSaveFormat.range


            if (comboBoxCollection != null) //checks combo box collection is not null
                for (int i = 0; i < comboBoxCollection.Length; i++)
                {
                    comboBoxCollection[i] = comboBoxCollection[i].ToLower().Trim();
                }
            #region commented code
            //if (export)
            //{
            //    fileName_lbl.Visible = false;
            //    fileName_tbx.Visible = false;
            //    example_lbl.Visible = false;

            //}
            //else
            //{
            //    fileName_lbl.Visible = true;
            //    fileName_tbx.Visible = true;
            //    example_lbl.Visible = true;
            //}
            #endregion

            if (comboBoxCollection != null)
            imageFbormat_cbx.DataSource = comboBoxCollection;
            if(CustomFolderData.ContainsKey("_compressionRatio"))
            comboBoxCollection = (CustomFolderData["_compressionRatio"] as string).Split(','); //IVLVariables.CurrentSettings.ImageStorageSettings._compressionRatio.range.Split(',');
            if (comboBoxCollection != null)
            {
                int[] myInts = Array.ConvertAll(comboBoxCollection, int.Parse);
                myInts = myInts.Where(x => x >= 70).ToArray();
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
                compressionRatio_cbx.DataSource = myInts;
            }
            imageFormat = imageFbormat_cbx.Text;
            // compressionRatio = compressionRatio_cbx.Text;
            folderPath = folder_tbx.Text;
            
        }
        /// <summary>
        /// this is to ensure the export textbox contents to hide or visible 
        /// </summary>
        public void ShowImageExportButtons()
        {
            //checks whether isReportExport is true
            if (this.isReportExport)// report export
            {
                if (CustomFolderData.ContainsKey("ExportReport_Text"))
                    this.Text = CustomFolderData["ExportReport_Text"] as string;// IVLVariables.LangResourceManager.GetString("ExportReport_Text", IVLVariables.LangResourceCultureInfo);
                
                Paths_tbpnl.RowStyles[1] = new RowStyle(SizeType.Percent, 0f); ;
                Paths_tbpnl.RowStyles[2] = new RowStyle(SizeType.Percent, 0f); ;
                Paths_tbpnl.RowStyles[3] = new RowStyle(SizeType.Percent, 0f); ;
                main_tbpnl.RowStyles[0] = new RowStyle(SizeType.Percent, 50f);
                main_tbpnl.RowStyles[1] = new RowStyle(SizeType.Percent, 50f);

                double mulFact = 0.5;
                double height = (double)this.Height * mulFact;
                //double widthSize = 0.8;
                //double width = (double)this.Width * widthSize;
                this.Size = new Size(this.Size.Width, (int)(height));
            }
            else
            {
                if (export)// export of images
                {

                    if (CustomFolderData.ContainsKey("ExportImages_Text"))
                        this.Text = CustomFolderData["ExportImages_Text"] as string;// IVLVariables.LangResourceManager.GetString("ExportImages_Text", IVLVariables.LangResourceCultureInfo);
                    Paths_tbpnl.RowStyles[1] = new RowStyle(SizeType.Percent, 0f);
                    main_tbpnl.RowStyles[0] = new RowStyle(SizeType.Percent, 70f);
                    main_tbpnl.RowStyles[1] = new RowStyle(SizeType.Percent, 30f);
                    if (Screen.PrimaryScreen.Bounds.Width == 1366)
                    {

                        double mulFact = 0.8;
                        double height = (double)this.Height * mulFact;
                        this.Size = new Size(this.Size.Width, (int)(height));
                    }
                    else if(Screen.PrimaryScreen.Bounds.Width == 1920)
                    {
                        double mulFact = 0.85;
                        double height = (double)this.Height * mulFact;
                        this.Size = new Size(this.Size.Width, (int)(height));
                    }
                    
                }
                else
                {
                    if (CustomFolderData.ContainsKey("SaveAs_Text"))
                        this.Text = CustomFolderData["SaveAs_Text"] as string;// IVLVariables.LangResourceManager.GetString("SaveAs_Text", IVLVariables.LangResourceCultureInfo);
                }
                

            }
        }
        public CustomFolderBrowser()
        {
            InitializeComponent();
            CustomFolderData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Will return a instance of the type CustomFolderBrowser.
        /// </summary>
        /// <returns></returns>
        //public static CustomFolderBrowser GetInstance()
        //{
        //    if (_customFolderBrowser == null)
        //    {
               
        //        _customFolderBrowser = new CustomFolderBrowser();
        //    }
        //    return _customFolderBrowser;
        //}


        /// <summary>
        /// This function will initializes the names for all controls present in window.
        /// </summary>
        private void InitializeResourceString()
        {
            #region labels initilize string from resources
            if (CustomFolderData.Count > 0)
            {
                if (CustomFolderData.ContainsKey("Folder_Text"))
                folder_lbl.Text = CustomFolderData["Folder_Text"] as string; //IVLVariables.LangResourceManager.GetString("Folder_Text", IVLVariables.LangResourceCultureInfo);
                if (CustomFolderData.ContainsKey("Format_Text"))
               
                imageFormat_lbl.Text = CustomFolderData["Format_Text"] as string; //IVLVariables.LangResourceManager.GetString("Format_Text", IVLVariables.LangResourceCultureInfo);
                if (CustomFolderData.ContainsKey("CompressionRatio_Text"))

                compressionRatio_lbl.Text = CustomFolderData["CompressionRatio_Text"] as string; // IVLVariables.LangResourceManager.GetString("CompressionRatio_Text", IVLVariables.LangResourceCultureInfo);
                if (CustomFolderData.ContainsKey("ApplicableForJpg_Text"))

                suggestion_lbl.Text = CustomFolderData["ApplicableForJpg_Text"] as string; //.LangResourceManager.GetString("ApplicableForJpg_Text", IVLVariables.LangResourceCultureInfo);
                if (CustomFolderData.ContainsKey("FileName_Text"))

                fileName_lbl.Text = CustomFolderData["FileName_Text"] as string; //IVLVariables.LangResourceManager.GetString("FileName_Text", IVLVariables.LangResourceCultureInfo);
                if (CustomFolderData.ContainsKey("FileNameExample_Text"))

                example_lbl.Text = CustomFolderData["FileNameExample_Text"] as string; //IVLVariables.LangResourceManager.GetString("FileNameExample_Text", IVLVariables.LangResourceCultureInfo);

                #region Buttons initialize string from resources
                if (CustomFolderData.ContainsKey("Ok_Text"))

                ok_btn.Text = CustomFolderData["Ok_Text"] as string; // IVLVariables.LangResourceManager.GetString("Ok_Text", IVLVariables.LangResourceCultureInfo);

                ok_btn.DialogResult = DialogResult.OK;
                if (CustomFolderData.ContainsKey("Cancel_Button_Text"))
                
                cancel_btn.Text = CustomFolderData["Cancel_Button_Text"] as string; //IVLVariables.LangResourceManager.GetString("Cancel_Button_Text", IVLVariables.LangResourceCultureInfo);
                cancel_btn.DialogResult = DialogResult.Cancel;
                if (CustomFolderData.ContainsKey("Browse_Button_Text"))

                browseFolders_btn.Text = CustomFolderData["Browse_Button_Text"] as string; // IVLVariables.LangResourceManager.GetString("Browse_Button_Text", IVLVariables.LangResourceCultureInfo);
                if (CustomFolderData.ContainsKey("OpenFileLocation_Text"))//checks if dictionary has OpenFileLocation_Text. By Ashutosh 24-7-2017
                    OpenFolderLocation_cbx.Text = CustomFolderData["OpenFileLocation_Text"] as string;//if present, then passed to OpenFolderLocation_cbx.Text.
                if (CustomFolderData.ContainsKey("OpenFile_Text"))//checks if dictionary has OpenFile_Text.
                    OpenFile_cbx.Text = CustomFolderData["OpenFile_Text"] as string;//  if present, then passed to OpenFile_cbx.Text.By Ashutosh 24-7-2017

                #endregion
            }
            #endregion

            List<Control> controls = GetControls(this).ToList();
            foreach (Control c in controls)
            {
                if (c is Label)
                {
                    Label l = c as Label;
                    if (Screen.PrimaryScreen.Bounds.Width == 1920)
                    {
                        if (l.Name != "suggestion_lbl" && l.Name != "example_lbl")
                            l.Font = new Font(l.Font.FontFamily.Name, 13f);
                        else
                            l.Font = new Font(l.Font.FontFamily.Name, 9f);
                    }
                    else if (Screen.PrimaryScreen.Bounds.Width == 1366)
                    {
                        if (l.Name != "suggestion_lbl" && l.Name != "example_lbl")
                            l.Font = new Font(l.Font.FontFamily.Name, 10f);
                        else
                            l.Font = new Font(l.Font.FontFamily.Name, 8f);
                      
                    }
                }
            }
        }

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

        private void BrowseFolders_btn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                folderPath = fbd.SelectedPath;
                folder_tbx.Text = folderPath;
            }
        }

        private void imageFbormat_cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!imageFbormat_cbx.Text.Contains("jpg"))
            {
                compressionRatio_cbx.Enabled = false;
                compressionRatio = 100;
            }
            else
            {
                compressionRatio_cbx.Enabled = true;
            }
            imageFormat = imageFbormat_cbx.Text;
        }

        private void compressionRatio_cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            compressionRatio = Convert.ToInt32(compressionRatio_cbx.Text);
        }

        private void fileName_tbx_TextChanged(object sender, EventArgs e)
        {
            fileName = fileName_tbx.Text;
        }

        /// <summary>
        /// To validate the folder path and file name
        /// </summary>
        private void validateFolderPathFileName()
        {
            Common.Validators.FileNameFolderPathValidator FileFolderValidator = Common.Validators.FileNameFolderPathValidator.GetInstance();
            folderPath = folderPath.TrimStart();// This is done fix defect 0001878 when empty spaces are added at the start of the folder location a crash was occuring which has been fixed by trimming the start of the string
            folderPath = folderPath.TrimStart(new char[] { '(', ')', '{', '}', '[', ']' });// To remove Brackets of all types at the start of the folder path which results in a crash to fix defect 0001891
            folderPath = folderPath.TrimEnd(new char[] { '(', ')', '{', '}', '[', ']' });// To remove Brackets of all types at the end of the folder path which results in a crash to fix defect 0001891
            errCode = FileFolderValidator.CheckFolderPath(folderPath, ref returnPath); 
            switch (errCode)
            {
                case FolderPathErrorCode.Success://when the folder path is valid
                    {
                        if(!FileFolderValidator.CheckFileName(fileName)) //checks whether the file name is valid or not
                        {
                            ImageSavingbtn();
                            if (OpenFolderLocation_cbx.Checked)//  if checked.
                            {

                                System.Diagnostics.Process.Start(System.IO.Path.GetDirectoryName(fileNames[0]));//opens the folder containing the file . First element of string array is opened.By Ashutosh 24-7-2017
                            }
                            if (OpenFile_cbx.Checked)
                            {
                                   
                                    foreach (string item in fileNames)
                                    {
                                        try
                                        {
                                            System.Diagnostics.Process.Start(item);//opens the file/s. By Ashutosh 24-7-2017
                                        }
                                        catch (Exception ex)
                                        {

                                            throw;
                                        }  
                                            
                                        
                                    }
                            }
                        }
                        else
                        {
                            CustomMessageBox.Show(CustomFolderData["SaveAs_Warning_Text"] as string, CustomFolderData["SaveAs_Warning_Header"] as string, CustomMessageBoxIcon.Warning);
                        }
                        break;
                    }
                case FolderPathErrorCode.InvalidDirectory://when the folder path is invalid
                    {
                        DialogResult result = CustomMessageBox.Show(CustomFolderData["FolderPath_Warning_Text"] as string, CustomFolderData["FolderPath_Warning_Header"] as string, CustomMessageBoxIcon.Warning);
                        this.DialogResult = System.Windows.Forms.DialogResult.None;
                        break;
                    }
                case FolderPathErrorCode.FolderPath_Empty: //when the folder path is empty
                    {
                        try
                        {
                            DialogResult result = CustomMessageBox.Show(CustomFolderData["FolderPath_Empty_Text"] as string, CustomFolderData["FolderPath_Warning_Header"] as string, CustomMessageBoxIcon.Warning);
                            this.DialogResult = System.Windows.Forms.DialogResult.None;
                        }
                        catch (Exception ex)
                        {
                            
                            throw;
                        } 
                        break;
                    }
                case FolderPathErrorCode.DirectoryDoesnotExist:
                    {
                        DialogResult result = CustomMessageBox.Show(CustomFolderData["DirectoryDoesnotExistWarning_Text"] as string, CustomFolderData["DirectoryDoesnotExist_Header"] as string, CustomMessageBoxButtons.YesNo, CustomMessageBoxIcon.Warning);
                        if (result == System.Windows.Forms.DialogResult.Yes)
                        {
                            if (!FileFolderValidator.CheckFileName(fileName)) //checks whether the file name is valid or not
                            {
                                Directory.CreateDirectory(folderPath);
                                returnPath = folderPath;
                                ImageSavingbtn();
                                if (OpenFolderLocation_cbx.Checked)//  if checked.
                                {

                                    System.Diagnostics.Process.Start(System.IO.Path.GetDirectoryName(fileNames[0]));//opens the folder containing the file . First element of string array is opened.By Ashutosh 24-7-2017
                                }
                                if (OpenFile_cbx.Checked)
                                {

                                    foreach (string item in fileNames)
                                    {
                                        try
                                        {
                                            System.Diagnostics.Process.Start(item);//opens the file/s. By Ashutosh 24-7-2017
                                        }
                                        catch (Exception ex)
                                        {

                                            throw;
                                        }


                                    }
                                }
                            }
                            else
                            {
                                CustomMessageBox.Show(CustomFolderData["SaveAs_Warning_Text"] as string, CustomFolderData["SaveAs_Warning_Header"] as string, CustomMessageBoxIcon.Warning);
                            }
                        }
                        break;
                    }
            }
        }
        private void ok_btn_Click(object sender, EventArgs e)
        {
            validateFolderPathFileName(); //calling the validateFolderPathFileName function when the ok button is clickedd in the custom folder browser
        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }

        private void folder_tbx_TextChanged(object sender, EventArgs e)
        {
            folderPath = folder_tbx.Text;
        }

        private void CustomFolderBrowser_FormClosing(object sender, FormClosingEventArgs e)
        {
            CancelButtonClickedEvent();
        }

        private void CustomFolderBrowser_Shown(object sender, EventArgs e)
        {
        }

        private void CustomFolderBrowser_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}
