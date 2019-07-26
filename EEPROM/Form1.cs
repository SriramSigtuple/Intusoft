using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EEPROM
{
    public partial class Form1 : Form
    {
        Settings_UCL settings;
        public Form1()
        {
            InitializeComponent();
            settings = new Settings_UCL();
            EEPROM_Version_Details_Page page =    EEPROM_Version_Details_Page.GetInstance();

            int x = settings.detailsPage.fieldDic.Count;
            
            settings.Dock = DockStyle.Fill;
            panel2.Controls.Add(settings);
        }

        private void showEEPROMSettings_btn_Click(object sender, EventArgs e)
        {

        }
    }
}
