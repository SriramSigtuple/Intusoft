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
    public partial class GradientForm : BaseGradientForm
    {
        public bool isOk = false;
        Themes t ;
        private string[] themeNames;
        public delegate void GradientColorChangeEvent(GradientColor ColorValues);
        public event GradientColorChangeEvent gradientColorChangeEvent;
        public GradientForm()
        {
            InitializeComponent();
            colorDialog1.FullOpen = true;
            gradientColorChangeEvent += GradientForm_gradientColorChangeEvent;
            color1_lbl.Text = "Color 1 : " + this.Color1.Name;
            color2_lbl.Text = "Color 2 : " + this.Color2.Name;
            colorAngle_lbl.Text = "Angle : " + this.SweepAngle_nud.Value.ToString();
            fontColor_lbl.Text = "Font Color : " + this.FontColor.Name;
            theme_lbl.Text = "Theme Name : " + this.ThemeName.ToString();
            ToolTip color1_btn = new ToolTip();
            color1_btn.SetToolTip(Color1_btn, "Select color 1");
            ToolTip color2_btn = new ToolTip();
            color2_btn.SetToolTip(Color2_btn, "Select color 2");
            ToolTip FontForeColor_btn = new ToolTip();
            FontForeColor_btn.SetToolTip(FontColor_btn, "Select Font Color");
            ToolTip ThemeName_tooltip = new ToolTip();
            ThemeName_tooltip.SetToolTip(themeName_tbx, "Enter theme name");
            t =  Themes.GetInstance();
            themeNames = t.GetAllThemeNames();
        }

        void GradientForm_gradientColorChangeEvent(GradientColor ColorValues)
        {
            t.AddNewTheme(ColorValues, setCurrentTheme_cbx.Checked);
        }

        private void Color1_btn_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Color1 = colorDialog1.Color;
                color1_lbl.Text = "Color 1 : " + Color1.Name;
            }

        }

        private void Color2_btn_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Color2 = colorDialog1.Color;
                color2_lbl.Text = "Color 2 : " + Color2.Name;
            }
        }

        private void SweepAngle_nud_ValueChanged(object sender, EventArgs e)
        {
            ColorAngle = (float)SweepAngle_nud.Value;
            colorAngle_lbl.Text = "Angle : " + ColorAngle.ToString();
        }

        private void OK_btn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(themeName_tbx.Text))
            {
                foreach (var item in themeNames)
                {
                    if (themeName_tbx.Text == item)//to check if the theme name already exists 
                    {
                        Common.CustomMessageBox.Show("Theme name already exists");
                        return;
                    }
                }
                GradientColor valuesDic = new GradientColor();
                valuesDic.Color1 = Color1;
                valuesDic.Color2 = Color2;
                valuesDic.FontForeColor = FontColor;
                valuesDic.ColorAngle = (float)SweepAngle_nud.Value;
                valuesDic.GradientColorName = themeName_tbx.Text;
                gradientColorChangeEvent(valuesDic);
                isOk = true;
                this.Close();
            }
            else
                Common.CustomMessageBox.Show("Theme name should not be empty","Warning",Common.CustomMessageBoxButtons.OK);

        }

        private void Cancel_btn_Click(object sender, EventArgs e)
        {
            isOk = false;
            this.Close();
        }

       

        private void label2_MouseMove(object sender, MouseEventArgs e)
        {
          
        }

        private void FontColor_btn_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FontColor = colorDialog1.Color;
                fontColor_lbl.Text = "Font Color : " + FontColor.Name;
            }
        }

        private void label2_MouseUp(object sender, MouseEventArgs e)
        {
            //if (e.Button == System.Windows.Forms.MouseButtons.Left)
            //{
            //    label2.Location = e.Location;
            //}
        }

        private void GradientForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
            }
        }


        private void themeName_tbx_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(themeName_tbx.Text))//if theme name textbox is null or empty theok button and check box will be disabled.
            {
                OK_btn.Enabled = false;
                setCurrentTheme_cbx.Enabled = false;
            }
            else //else ok button and check box will be enabled
            {
                OK_btn.Enabled = true;
                setCurrentTheme_cbx.Enabled = true;
            }
            theme_lbl.Text = "Theme Name : " + themeName_tbx.Text;
        }
    }
}
