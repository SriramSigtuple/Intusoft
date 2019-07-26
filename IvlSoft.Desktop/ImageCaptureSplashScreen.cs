using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace INTUSOFT.Desktop
{
    public partial class ImageCaptureSplashScreen : Form
    {
        Timer t;
        int val = 0;
        public ImageCaptureSplashScreen()
        {
            InitializeComponent();
            t = new Timer();
            t.Interval = 40;
            t.Tick += t_Tick;
            this.Shown += ImageCaptureSplashScreen_Shown;
            ImageCapture_lbl.ForeColor = Color.FromArgb(15, 15, 15);
            progress_lbl.ForeColor = Color.FromArgb(15, 15, 15);
            ImageCapture_lbl.BackColor = Color.White;
            progress_lbl.BackColor = Color.White;
            this.TransparencyKey = SystemColors.ActiveBorder;
        }

        void ImageCaptureSplashScreen_Shown(object sender, EventArgs e)
        {
            t.Start();
        }

        void t_Tick(object sender, EventArgs e)
        {
            if (IVLVariables.showSplash)
            {
                if(val<100)
                val += 1;
                ImageCapture_pb.Value = val;
                progress_lbl.Text = val.ToString() + "%";
            }
            else
            {
                t.Stop();
                this.Close();
            }
        }

    }
}
