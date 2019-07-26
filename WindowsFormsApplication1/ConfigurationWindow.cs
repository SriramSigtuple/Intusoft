using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using INTUSOFT.Configuration;
using Common;
namespace AssemblySoftware
{
    public partial class ConfigurationWindow : Form
    {
        Settings_UCL settings;
        public ConfigurationWindow()
        {
            InitializeComponent();
            Dictionary<string, string> settingsStringResourcesDic = new Dictionary<string, string>();
             settingsStringResourcesDic.Add("Software_Name","Assembly Software");
             settingsStringResourcesDic.Add("RestartApplication_Text","Resatart");
             settingsStringResourcesDic.Add("ImageViewer_Save_Button_Text","Save");
            Settings_UCL settings = new Settings_UCL(settingsStringResourcesDic);
            settings.Dock = DockStyle.Fill;
            this.Text = "Settings";
            this.Controls.Add(settings);
        }


        private void ConfigurationWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
        ////This code block has been added by darshan to display a pop up window when any changes has been made to the settings window.
            //if (IVLVariables.ivl_Camera.camPropsHelper.ImagingMode != (Imaging.ImagingMode)Enum.Parse(typeof(Imaging.ImagingMode), IVLVariables._ivlConfig.Mode.ToString()))
                //XmlReadWrite.isSaved = false;
  
            if (!XmlReadWrite.isSaved )
            {
                if (CustomMessageBox.Show("Warning", "Warning", CustomMessageBoxButtons.RestartNowRestartLater, CustomMessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                {
                   
                    //if (IVLVariables.CurrentSettings != null)
                    //    XmlConfigUtility.Serialize(IVLVariables.CurrentSettings, IVLConfig.fileName);
                    XmlReadWrite.isSaved = !XmlReadWrite.isSaved;
                    //IVLVariables.ivl_Camera.camPropsHelper.ImagingMode = (Imaging.ImagingMode)Enum.Parse(typeof(Imaging.ImagingMode), IVLVariables._ivlConfig.Mode.ToString());
                    //Common.CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("RestartApplication_Text", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("Software_Name", IVLVariables.LangResourceCultureInfo), Common.CustomMessageBoxButtons.OK, Common.CustomMessageBoxIcon.Information);
                    Program.RestartApplication();
                    
                }
                else
                XmlReadWrite.isSaved = !XmlReadWrite.isSaved;
            }
        }

    }


    }
