using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using INTUSOFT.Desktop.Properties;
using Common;
using INTUSOFT.Configuration;
namespace INTUSOFT.Desktop.Forms
{
    public partial class SettingsWindow : Form
    {
        Settings_UCL settings;
        public SettingsWindow()
        {
            InitializeComponent();
            Dictionary<string, string> settingsStringResourcesDic = new Dictionary<string, string>();
             settingsStringResourcesDic.Add("Software_Name",IVLVariables.LangResourceManager.GetString("Software_Name", IVLVariables.LangResourceCultureInfo));
             settingsStringResourcesDic.Add("RestartApplication_Text",IVLVariables.LangResourceManager.GetString("RestartApplication_Text", IVLVariables.LangResourceCultureInfo));
             settingsStringResourcesDic.Add("ImageViewer_Save_Button_Text",IVLVariables.LangResourceManager.GetString("ImageViewer_Save_Button_Text", IVLVariables.LangResourceCultureInfo));
            Settings_UCL settings = new Settings_UCL(settingsStringResourcesDic);
            settings.Dock = DockStyle.Fill;
            this.Text = IVLVariables.LangResourceManager.GetString("IvlSettings_Text", IVLVariables.LangResourceCultureInfo);
            this.Controls.Add(settings);
        }

        private void SettingsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            ////This code block has been added by darshan to display a pop up window when any changes has been made to the settings window.
            //if (IVLVariables.ivl_Camera.camPropsHelper.ImagingMode != (Imaging.ImagingMode)Enum.Parse(typeof(Imaging.ImagingMode), IVLVariables._ivlConfig.Mode.ToString()))
                //XmlReadWrite.isSaved = false;
  
            if (!XmlReadWrite.isSaved )
            {
                if (CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("SettingsWarning_Message_Text", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("SettingsWarning_Header_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxButtons.RestartNowRestartLater, CustomMessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
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
