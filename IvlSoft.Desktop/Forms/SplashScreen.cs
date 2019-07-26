using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using INTUSOFT.Custom.Controls;
namespace INTUSOFT.Desktop.Forms
{
    public partial class SplashScreen : BaseGradientForm
    {
        public SplashScreen()
        {
            InitializeComponent();
        }
        private string splashScreenText = string.Empty;

        public string SplashScreenText
        {
            set {
                splashScreen_lbl.Text = value;
                splashScreenText = value; 
                }
        }
        private void SplashScreen_Load(object sender, EventArgs e)
        {
            Application.EnableVisualStyles();
            this.Color1 = IVLVariables.GradientColorValues.Color1;
            this.Color2 = IVLVariables.GradientColorValues.Color2;
            this.FontColor = IVLVariables.GradientColorValues.FontForeColor;
            this.ColorAngle = IVLVariables.GradientColorValues.ColorAngle;
            progressBar1.Enabled = true;
            progressBar1.Show();
            
        }
    }
}
