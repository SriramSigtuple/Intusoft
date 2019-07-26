using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using Common;

namespace INTUSOFT.Configuration
{
    public partial class Settings_UCL : UserControl
    {
        TabControl t;
        Config_UC config;
        Dictionary<string, string> configResourceDic;
        /// <summary>
        /// Constructor to populate the settings from IVLConfig.xml
        /// </summary>
        public Settings_UCL(Dictionary<string, string> ConfigResourceDic)
        {

            InitializeComponent();
            configResourceDic = ConfigResourceDic;
            if (configResourceDic == null)
                configResourceDic = new Dictionary<string, string>();

            InitializeResourceString();
            SetMode();
            PopulateConfig();
           // DeviceSettings_tbp.Controls.Add(config);
        }

        private void PopulateConfig()
        {
                if (t != null)//This condition has been added to dispose the tabcontrol since it was consuming user objects.
                {
                    t.Dispose();
                    //t = null;
                }
               
                {
                    t = new TabControl();
                }
                t.Multiline = true;
                t.Size = new System.Drawing.Size(this.Width, this.Height - (int)tableLayoutPanel1.RowStyles[1].Height);
                {
                     config = new Config_UC();
                 
                    KeyValuePair<string, object> temp = config.rootDict;
                    Dictionary<string, object> dic = temp.Value as Dictionary<string, object>;
                    FieldInfo[] f = Config_UC.fieldInfos;
                    foreach (FieldInfo val in f)
                    {
                        if (val.Name.Equals("fileName"))
                            break;
                         TabPage tPage = new TabPage();
                        FieldInfo[] tempFieldInfo = val.FieldType.GetFields(System.Reflection.BindingFlags.Public | BindingFlags.Instance);
                        object obj = val.GetValue(ConfigVariables.CurrentSettings);
                        string output = "";
                        //This below foreach has been added by Darshan on 08-09-2015 to solve Defect no 0000631: The spaces are not left for the Labels in settings window.
                        foreach (char letter in val.Name)
                        {
                            if (Char.IsUpper(letter) && output.Length > 0)
                                output += " " + letter;
                            else
                                output += letter;
                        }
                        tPage.Text = output;
                        tPage.AutoScroll = true;
                        if (tempFieldInfo.Count() != 0)
                        {
                            config.vIndent = 10;
                            config.addBranchL(tempFieldInfo, val.Name, 1, tPage, obj);
                        }
                        else
                        {
                            PropertyInfo[] pinf = val.FieldType.GetProperties(System.Reflection.BindingFlags.Public | BindingFlags.Instance);
                            config.vIndent = 10;
                            config.addBranchL(pinf, val.Name, 1, tPage, obj);
                        }

                        t.Controls.Add(tPage);
                        //tPage.Dispose();
                    }
                    t.Dock = DockStyle.Fill;
                }
                // this.Controls.Add(t);
                // config.Dock = DockStyle.Fill;

                tableLayoutPanel1.Controls.Add(t, 0, 0);
            
        }

        private void InitializeResourceString()
        {
            if(configResourceDic.ContainsKey("ImageViewer_Save_Button_Text"))
            saveSettings_btn.Text = configResourceDic["ImageViewer_Save_Button_Text"];             //Reset_btn.Text = Resources.Reset_Text;
            //DeviceSettings_tbp.Text = Resources.Device_Settings_Text;
            //ImageStorage_tbp.Text = Resources.Image_Storage_Text;
            //ReportSettings_tbp.Text = Resources.Report_Settings_Text;
            //PrinterSettings_tbp.Text = Resources.Printer_Settings_Text;
        }

        private void saveSettings_btn_Click(object sender, EventArgs e)
        {
            XmlReadWrite.isSaved = false;
            {
                config.SaveXml();
            }
            
            this.ParentForm.Close();
            //string messageBoxText = "";
            //string messageTitle = "";
            ////0000573 Feature has been added by sriram on August 18th 2015
            //if (configResourceDic.ContainsKey("RestartApplication_Text"))
            //    messageBoxText = configResourceDic["RestartApplication_Text"];
            // if (configResourceDic.ContainsKey("Software_Name"))
            //    messageTitle = configResourceDic["Software_Name"];
            //Common.CustomMessageBox.Show(messageBoxText,messageTitle, Common.CustomMessageBoxButtons.OK, Common.CustomMessageBoxIcon.Information);
            //////Program.RestartApplication();// this has been uncommented in order to restart the application after the settings have been saved.
            // try
            // {
            //     //run the program again and close this one
            //     System.Diagnostics.Process.Start(Application.StartupPath + "\\IntuSoft.exe");
            //     //or you can use Application.ExecutablePath

            //     //close this one
            //     System.Diagnostics.Process.GetCurrentProcess().Kill();
            // }
            // catch
            // { }
            //
        }

        private void Reset_btn_Click(object sender, EventArgs e)
        {
            IVLConfig.fileName = null;
            PopulateConfig();
        }


        private void cancel_btn_Click(object sender, EventArgs e)
        {
            this.ParentForm.Close();
        }

        private void RestoreFactorySettings_btn_Click(object sender, EventArgs e)
        {

        }

        private void saveSettings_btn_Click_1(object sender, EventArgs e)
        {
            {
                config.SaveXml();
            }
            this.ParentForm.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void ChangeModeForSettings()
        {
            ConfigVariables.GetCurrentSettings();
            t.TabPages.Clear();
            PopulateConfig();
            XmlReadWrite.isSaved = false;
        }

        private void SetMode()
        {
            switch (ConfigVariables._ivlConfig.Mode)
            {
                case ImagingMode.Posterior_Prime:
                    {
                        primeSettings_rb.Checked = true;
                        break;
                    }
                case ImagingMode.Anterior_Prime:
                    {
                        Anterior_rb.Checked = true;
                        break;
                    }
                case ImagingMode.Posterior_45:
                    {
                        fortyFive_rb.Checked = true;
                        break;
                    }
                     case ImagingMode.Anterior_45:
                    {
                        Anterior_rb.Checked = true;
                        break;
                    }
                         case ImagingMode.Anterior_FFA:
                    {
                        Anterior_rb.Checked = true;
                        break;
                    }
                         case ImagingMode.FFA_Plus:
                    {
                        ffa_rb.Checked = true;
                        break;
                    }
                         case ImagingMode.FFAColor:
                    {
                        FfaColor_rb.Checked = true;
                        break;
                    }


            }
        }

        private void primeSettings_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (primeSettings_rb.Checked)
            {
                if (ConfigVariables._ivlConfig.Mode != ImagingMode.Posterior_Prime)
                {
                    ConfigVariables._ivlConfig.Mode = ImagingMode.Posterior_Prime;
                    ChangeModeForSettings();
                }
            }
        }

        private void Anterior_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (Anterior_rb.Checked)
            {
                if (ConfigVariables._ivlConfig.Mode != ImagingMode.Anterior_Prime)
                {
                    ConfigVariables._ivlConfig.Mode = ImagingMode.Anterior_Prime;
                    ChangeModeForSettings();
                }
            }
        }

        private void fortyFive_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (fortyFive_rb.Checked)
            {
                if (ConfigVariables._ivlConfig.Mode != ImagingMode.Posterior_45)
                {
                    ConfigVariables._ivlConfig.Mode = ImagingMode.Posterior_45;
                    ChangeModeForSettings();
                }
            }
        }

        private void ffa_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (ffa_rb.Checked)
            {
                if (ConfigVariables._ivlConfig.Mode != ImagingMode.FFA_Plus)
                {
                    ConfigVariables._ivlConfig.Mode = ImagingMode.FFA_Plus;
                    ChangeModeForSettings();
                }
            }
        }

        private void FfaColor_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (FfaColor_rb.Checked)
            {
                if (ConfigVariables._ivlConfig.Mode != ImagingMode.FFAColor)
                {
                    ConfigVariables._ivlConfig.Mode = ImagingMode.FFAColor;
                    ChangeModeForSettings();
                }
            }
        }
    }
}
