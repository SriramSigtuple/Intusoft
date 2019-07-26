using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReportUtils
{
    public partial class Warning_UC : UserControl
    {
        private string warningText = string.Empty;

        public string WarningText
        {
            get { return warningText; }
            set 
            {
                warningText = value;
                this.warning_txt_lbl.Text = value;
            }
        }
        public Warning_UC()
        {
            InitializeComponent();
            this.pictureBox1.Image = SystemIcons.Warning.ToBitmap();
            this.warning_txt_lbl.Text = "Images displayed based on selection order.";
        }
    }
}
