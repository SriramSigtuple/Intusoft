using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace INTUSOFT.Desktop.Forms
{
    public partial class CaptureFailureForm : Form
    {
        public CaptureFailureForm()
        {
            InitializeComponent();
        }
        public void SetErrorMsg(string msg)
        {
            errorMsg_lbl.Text = msg;
        }
    }
}
