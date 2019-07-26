using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EEPROM;
namespace INTUSOFT.Desktop.Forms
{

    public partial class EEPROM_Window : Form
    {
  public  EEPROM.Settings_UCL settings;

        public EEPROM_Window()
        {
            InitializeComponent();
            settings = new EEPROM.Settings_UCL();
            EEPROM_Version_Details_Page page = EEPROM_Version_Details_Page.GetInstance();
            int x = settings.detailsPage.fieldDic.Count;
            settings.Dock = DockStyle.Fill;
            splitContainer1.Panel2.Controls.Add(settings);
        }
        public void PopulateEEPROM()
        {
            
        }

    }
}
