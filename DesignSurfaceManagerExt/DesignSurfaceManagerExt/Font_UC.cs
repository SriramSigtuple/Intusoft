using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using ReportUtils;
using System.Linq;
using System.Drawing.Text;
using Common;
using System.Reflection;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignSurfaceManagerExtension
{
    //public enum FontChangeValue { ForeColor,Font };

    public partial class Font_UC : UserControl
    {
        ColorDialog colorDialog;
        public delegate void FontValueUpdate(ValueChanged valueChanged, object value, Color color);
        public event FontValueUpdate fontValueUpdate;
        public bool fontValueChange = true;
        //public FontFamily[] fontFamily;
        public FontFamily[] fontFamily;

        Color[] custColors;
        Color selcetedButtonColor = Color.LightYellow;
        Color unSelcetedButtonColor = Color.LightGray;
        Dictionary<string, bool> stateOfFontStyle;
        ToolTip toolip;
        Font font;
        FontStyle fontStyle;
        const string isRegular = "is_Regular";
        const string isBold = "is_Bold";
        const string isItalics = "is_Italic";
        const string isUnderline = "is_Underline";
        //Dictionary<String, Color> colors = typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.Public).ToDictionary(p => p.Name, p => (Color)p.GetValue(null, null));
        Color color;

        public Font_UC()
        {
            fontValueChange = false;
            InitializeComponent();

            if (System.IO.File.Exists(@"fontFamily.txt"))
            {
                List<string> fontList = new List<string>();
                System.IO.StreamReader stR = new System.IO.StreamReader(@"fontFamily.txt");
                string fontName = "";// stR.ReadToEnd();
                while ((fontName = stR.ReadLine()) != null)
                    fontList.Add(fontName);
                fontFamily = new FontFamily[fontList.Count];
                for (int i = 0; i < fontList.Count; i++)
                {
                    fontFamily[i] = new FontFamily(fontList[i]);
                }
                stR.Close();
                stR.Dispose();
            }
            else if (fontFamily == null || fontFamily.Length == 0)
            {
                CustomFontFamilies[] CustomFontFamilyEnumvalues = Enum.GetValues(typeof(CustomFontFamilies)) as CustomFontFamilies[];
                //if (CustomFontFamilyEnumvalues == null)
                //{
                //     string[] dirPath = Control Panel\Appearance and Personalization\Fonts;
                //     File.Move(NameOfFont,dirPath);
                //    //CustomFontFamilyEnumvalues= Enum.GetValues(nameoffont in cutsomfontfamilies enum) as CustomFontFamilies[] ;
                //}
                List<string> FontFamiliesValues = CustomFontFamilyEnumvalues.OfType<object>().Select(o => o.ToString()).ToList();

                for (int i = 0; i < FontFamiliesValues.Count; i++)
                {
                    if (FontFamiliesValues[i].Contains('_'))
                    {
                        FontFamiliesValues[i] = Common.CommonStaticMethods.GetDescription(CustomFontFamilyEnumvalues[i]);
                    }
                }
                List<string> CustomFontFamilyDescriptionValues = new List<string>();
                var fontsCollection = new InstalledFontCollection();
                foreach (string  item in FontFamiliesValues)
                {
                    if (fontsCollection.Families.Any(x => x.Name.Equals(item, StringComparison.CurrentCultureIgnoreCase)))
                        CustomFontFamilyDescriptionValues.Add(item);
                }
                fontFamily = new FontFamily[CustomFontFamilyDescriptionValues.Count];
                for (int i = 0; i < CustomFontFamilyDescriptionValues.Count; i++)
                {
                    fontFamily[i] = new FontFamily(CustomFontFamilyDescriptionValues[i]);
                }
            }

            if (System.IO.File.Exists(@"ColorNames.txt"))
            {
                List<string> colorList = new List<string>();
                System.IO.StreamReader stR = new System.IO.StreamReader(@"ColorNames.txt");
                string colorName = "";// stR.ReadToEnd();
                while ((colorName = stR.ReadLine()) != null)
                    colorList.Add(colorName);
                custColors = new Color[colorList.Count];
                for (int i = 0; i < colorList.Count; i++)
                {
                    custColors[i] = Color.FromName(colorList[i]);
                }
                stR.Close();
                stR.Dispose();
            }
            else if (custColors == null || custColors.Length == 0)
            {
                custColors = new Color[] { Color.Aqua, Color.Black, Color.Blue, Color.Chocolate, Color.DarkBlue, Color.DarkGray, Color.DarkGreen, Color.DarkOrange, Color.DarkRed, Color.DeepPink, Color.Gold, Color.Gray, Color.Green, Color.Indigo, Color.Khaki, Color.LightBlue, Color.LightGreen, Color.LightYellow, Color.Lime, Color.Orange, Color.Pink, Color.Purple, Color.Red, Color.Violet, Color.Wheat, Color.Yellow };
            }

            toolip = new ToolTip();
            stateOfFontStyle = new Dictionary<string, bool>();
            stateOfFontStyle.Add(isRegular, false);
            stateOfFontStyle.Add(isBold, false);
            stateOfFontStyle.Add(isItalics, false);
            stateOfFontStyle.Add(isUnderline, false);
            FontFamily_cbx.DataSource = fontFamily.Select(f => f.Name).ToList<string>();

            FontStyle[] Font_Style_Enum_values = Enum.GetValues(typeof(FontStyle)) as FontStyle[];
            string[] FontStyle_values = Font_Style_Enum_values.OfType<object>().Select(o => o.ToString()).ToArray();
            fontValueChange = true;
            colorDialog = new ColorDialog();
        }

        List<string> InstalledFontFamilies(string name)
        {
            List<string> fontFamilies = new List<string>();
            var fontsCollection = new InstalledFontCollection();
            foreach (var fontFamiliy in fontsCollection.Families)
            {
                if (fontFamiliy.Name == name)
                    fontFamilies.Add(name);
            }
            return fontFamilies;
        }


        private void FontColorDialog_btn_Click(object sender, EventArgs e)
        {
            colorDialog.FullOpen = false;
            colorDialog.SolidColorOnly = true;
            colorDialog.AnyColor = false;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                if (colorDialog.Color.IsKnownColor)
                    color_tbx.Text = colorDialog.Color.Name;
                else
                    color_tbx.Text = "#" + colorDialog.Color.Name;
                //font = new Font(new FontFamily(FontFamily_cbx.Text), Convert.ToSingle(FontSize_nud.Value), (FontStyle)Enum.Parse(typeof(FontStyle), FontType_cbx.Text));
                font = new Font(new FontFamily(FontFamily_cbx.Text), Convert.ToSingle(FontSize_nud.Value), fontStyle);

                if (color_tbx.Text.Contains('#'))
                {
                    color = ColorTranslator.FromHtml(color_tbx.Text);
                }
                else
                {
                    color = Color.FromName(color_tbx.Text);
                }
                fontValueUpdate(ValueChanged.ForeColor, font, color);
            }
        }

        string GetColourName(string htmlString)
        {
            Color color = (Color)new ColorConverter().ConvertFromString(htmlString);
            KnownColor knownColor = color.ToKnownColor();
            string name = knownColor.ToString();
            return name.Equals("0") ? "Unknown" : name;
        }

        private System.Drawing.Color GetSystemDrawingColorFromHexString(string hexString)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(hexString, @"([0-9]|[a-f]|[A-F]){6}\b"))
                throw new ArgumentException();
            int red = int.Parse(hexString.Substring(1, 2), NumberStyles.HexNumber);
            int green = int.Parse(hexString.Substring(3, 2), NumberStyles.HexNumber);
            int blue = int.Parse(hexString.Substring(5, 2), NumberStyles.HexNumber);
            return Color.FromArgb(red, green, blue);
        }

        public void ReloadFontChanges(Control ctrl, ReportControlProperties repCtrlProp)
        {
            fontValueChange = false;
            color_tbx.Text = ctrl.ForeColor.Name;
            FontFamily_cbx.SelectedItem = ctrl.Font.Name.ToString();

            fontStyle = repCtrlProp.Font.FontStyle;
            setDefaultFontStyleValue(fontStyle);
            RefreshButtonColors();
            FontSize_nud.Value = Convert.ToDecimal(ctrl.Font.Size);
            repCtrlProp.Font.FontColor = ctrl.ForeColor.Name;
            repCtrlProp.Font.FontFamily = ctrl.Font.Name.ToString();
            repCtrlProp.Font.FontSize = ctrl.Font.Size;
            repCtrlProp.Font.FontStyle = ctrl.Font.Style;
            fontValueChange = true;
        }

        private void FontColor_tbx_TextChanged(object sender, EventArgs e)
        {
            if (fontValueChange)
            {
                font = new Font(new FontFamily(FontFamily_cbx.Text), Convert.ToSingle(FontSize_nud.Value), fontStyle);
                color = Color.FromName(color_tbx.Text);
                fontValueUpdate(ValueChanged.ForeColor, font, color);
            }
        }


        private void FontType_cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fontValueChange)
                ChangeFont();
        }

        private void FontFamily_cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fontValueChange)
                ChangeFont();
        }

        public void ChangeFont()
        {
            font = new Font(new FontFamily(FontFamily_cbx.Text), Convert.ToSingle(FontSize_nud.Value), fontStyle,GraphicsUnit.Point);
            if (color_tbx.Text.Contains('#'))
            {
                color = ColorTranslator.FromHtml(color_tbx.Text);
            }
            else
            {
                color = Color.FromName(color_tbx.Text);
            }
            fontValueUpdate(ValueChanged.Font, font, color);
        }

        private void FontSize_nud_ValueChanged(object sender, EventArgs e)
        {
            if (fontValueChange)
                ChangeFont();
        }

        private void color_cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fontValueChange)
            {
                font = new Font(new FontFamily(FontFamily_cbx.Text), Convert.ToSingle(FontSize_nud.Value), fontStyle);
                color = Color.FromName(color_tbx.Text);
                fontValueUpdate(ValueChanged.ForeColor, font, color);
            }
        }

        private void color_cbx_TextChanged(object sender, EventArgs e)
        {
            //if (fontValueChange)
            //{
            //    font = new Font(new FontFamily(FontFamily_cbx.Text), Convert.ToSingle(FontSize_nud.Value), (FontStyle)Enum.Parse(typeof(FontStyle), FontType_cbx.Text));
            //    color = Color.FromName(color_cbx.Text);
            //    fontValueUpdate(ValueChanged.ForeColor, font, color);
            //}
        }

        public void ResetFontColorButton()
        {
            foreach (Control item in tableLayoutPanel3.Controls)
            {
                if (item is Button)
                {
                    Button button = item as Button;
                    button.BackColor = unSelcetedButtonColor;
                }
            }
        }

        void ResetFontStyleDictionary()
        {
            foreach (string item in stateOfFontStyle.Keys.ToArray())
            {
                stateOfFontStyle[item] = false;
            }
        }

        private void Regular_btn_Click(object sender, EventArgs e)
        {
            try
            {
                ResetFontStyleDictionary();
                stateOfFontStyle[isRegular] = true;
                ResetFontColorButton();
                RefreshButtonColors();
                ChangeFont();
            }
            catch (Exception ex)
            {
                throw;
            } 
            
        }

        private void Bold_btn_Click(object sender, EventArgs e)
        {
            stateOfFontStyle[isBold] = !stateOfFontStyle[isBold];
            stateOfFontStyle[isRegular] = false;
            ResetFontColorButton();
            RefreshButtonColors();
            ChangeFont();
        }

        public void setDefaultFontStyleValue(FontStyle fontStyleReset)
        {
            foreach (string item in stateOfFontStyle.Keys.ToArray())
            {
                string[] strArr = item.Split('_');
                if (fontStyleReset.ToString().Contains(strArr[strArr.Length - 1]))
                {
                    stateOfFontStyle[item] = true;
                }
                else
                {
                    stateOfFontStyle[item] = false;
                }
            }
        }

        public void RefreshButtonColors()
        {
            foreach (KeyValuePair<string, bool> item in stateOfFontStyle)
            {
                if (item.Value)
                {
                    string[] strArr = item.Key.Split('_');
                    fontStyle |= (FontStyle)Enum.Parse(typeof(FontStyle), strArr[strArr.Length - 1]);

                    foreach (Control c in tableLayoutPanel3.Controls)
                    {
                        if (c is Button)
                        {
                            Button button = c as Button;
                            if (button.Name.Contains(strArr[strArr.Length - 1]))
                                button.BackColor = selcetedButtonColor;
                        }
                    }
                }
                else
                {
                    string[] strArr = item.Key.Split('_');
                    fontStyle &= ~(FontStyle)Enum.Parse(typeof(FontStyle), strArr[strArr.Length - 1]);
                    foreach (Control c in tableLayoutPanel3.Controls)
                    {
                        if (c is Button)
                        {
                            Button button = c as Button;
                            if (button.Name.Contains(strArr[strArr.Length - 1]))
                                button.BackColor = unSelcetedButtonColor;
                        }
                    }
                }
            }
        }

        private void Italics_btn_Click(object sender, EventArgs e)
        {
            stateOfFontStyle[isItalics] = !stateOfFontStyle[isItalics];
            stateOfFontStyle[isRegular] = false;
            ResetFontColorButton();
            RefreshButtonColors();
            ChangeFont();
        }

        private void UnderLine_btn_Click(object sender, EventArgs e)
        {
            stateOfFontStyle[isUnderline] = !stateOfFontStyle[isRegular];
            stateOfFontStyle[isRegular] = false;
            ResetFontColorButton();
            RefreshButtonColors();
            ChangeFont();
        }

        private void Regular_btn_MouseHover(object sender, EventArgs e)
        {
            toolip.SetToolTip(Regular_btn, "Regular");
        }

        private void Bold_btn_MouseCaptureChanged(object sender, EventArgs e)
        {
            toolip.SetToolTip(Bold_btn, "Bold");
        }

        private void Italics_btn_MouseHover(object sender, EventArgs e)
        {
            toolip.SetToolTip(Italics_btn, "Italics");
        }

        private void UnderLine_btn_MouseHover(object sender, EventArgs e)
        {
            toolip.SetToolTip(Underline_btn, "Under Line");
        }
    }

    public enum CustomFontFamilies
    {
        Algerian,
        //Jost_Futuristic_Style,
        Arial,
        [Description("Book Antiqua")]
        Book_Antiqua,
        Calibri,
        [Description("Californian FB")]
        Californian_FB,
        Cambria,
        Georgia,
        [Description("Segoe UI")]
        Segoe_UI,
        [Description("Times New Roman")]
        Times_New_Roman, Verdana

    }
}
