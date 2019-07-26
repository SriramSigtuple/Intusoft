using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using INTUSOFT.Imaging;
namespace AssemblySoftware
{
    public partial class EEPROM_Window : Form
    {
        EEPROM_UC prom_uc;

        public EEPROM_Window()
        {
            InitializeComponent();
           prom_uc = new EEPROM_UC();
        }

        private void resetEEPROM_btn_Click(object sender, EventArgs e)
        {
            prom_uc.ResetEEPROM();
        }

        private void SaveEEPROM_btn_Click(object sender, EventArgs e)
        {
            prom_uc.WriteEEPROM();
            this.Close();
        }

        private void readEEPROM_btn_Click(object sender, EventArgs e)
        {
            prom_uc.Dock = DockStyle.Fill;
            prom_uc.ReadEEPROM();
            this.panel1.Controls.Add(prom_uc);
        }

        private void readSingleEEPROM_btn_Click(object sender, EventArgs e)
        {
            prom_uc.ReadEEPROM(Convert.ToByte(eepromPageNumber_nud.Value));
        }

        private void writeSingleEEPROM_btn_Click(object sender, EventArgs e)
        {
            

            prom_uc.WriteEEPROM(Convert.ToByte(eepromPageNumber_nud.Value),testString_tbx.Text);

        }

        private void formTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
