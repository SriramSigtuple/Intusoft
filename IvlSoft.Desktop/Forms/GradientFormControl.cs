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
    public partial class GradientFormControl : INTUSOFT.Custom.Controls.BaseGradientForm
    {
        public GradientFormControl()
        {
            InitializeComponent();
            this.Color1 = IVLVariables.GradientColorValues.Color1;
            this.Color2 = IVLVariables.GradientColorValues.Color2;
            this.ColorAngle = IVLVariables.GradientColorValues.ColorAngle;
        }
    }
}
