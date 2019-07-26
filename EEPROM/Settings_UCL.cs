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

namespace EEPROM
{
    public partial class Settings_UCL : UserControl
    {
        TabControl t;
       public EEPROM_Version_Details_Page detailsPage;
        Config_UC config;
        private bool isReadOnly;

        public static bool IsReadOnly
        {
            get { return Variables.isReadOnly; }
            set { Variables.isReadOnly = value; }
        }
        /// <summary>
        /// Constructor to populate the settings from IVLConfig.xml
        /// </summary>
        public Settings_UCL()
        {
            InitializeComponent();
            EEPROM_Version_Details_Page detailsPage = EEPROM_Version_Details_Page.GetInstance();
            List<byte[]> arr = new List<byte[]>();
            detailsPage.GetFields(arr);
            config = new Config_UC();
            PopulateEEPROMDic();
        }

        private void PopulateEEPROMDic()
        {
            if (detailsPage == null)
                detailsPage = EEPROM_Version_Details_Page.GetInstance();
            if (t == null)
            {
                t = new TabControl();
            }
            t.Multiline = true;
            
            t.Size = new System.Drawing.Size(this.Width, this.Height -(int) tableLayoutPanel1.RowStyles[1].Height);

            foreach (KeyValuePair<object, object> val in  detailsPage.fieldDic)
            {

                TabPage tPage = new TabPage();
                string output = "";
                EEPROM_Props eepromTab = val.Key as EEPROM_Props;
                tPage.Text = eepromTab.text;
                tPage.AutoScroll = true;
                config.vIndent = 10;
                string parentName = "Settings";
                string strutureTypeStr = StrutureTypes.FeatureSettings.ToString();
                config.addBranchL(val, "Settings", 1, tPage);
                t.Controls.Add(tPage);
            }
            t.Dock = DockStyle.Fill;
            tableLayoutPanel1.Controls.Add(t, 0, 0);
        }

        private void saveSettings_btn_Click(object sender, EventArgs e)
        {
            
            this.ParentForm.Close();
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





    }
}
