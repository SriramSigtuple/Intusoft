using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using Newtonsoft.Json;


namespace INTUSOFT.Desktop
{
    public partial class ThemesForm : Form
    {
        Themes t ;
        public delegate void GradientColorChangeEvent(Dictionary<string, object> ColorValues);
        public event GradientColorChangeEvent gradientColorChangeEvent;
        //GradientColor[] gradientColorArr;
        public ThemesForm()
        {
            InitializeComponent();
            t =  Themes.GetInstance();
            string[] themeNames = t.GetAllThemeNames();
            byte tempCurrentThemeIndx = t.CurrentTheme;
            comboBox1.DataSource = themeNames;
            comboBox1.SelectedIndex = tempCurrentThemeIndx;
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            t.CurrentTheme = (byte)comboBox1.SelectedIndex;
            if (t.ThemesList == null)
                t.GetAllThemeNames();
            GradientColor g = t.GetCurrentTheme();
            themeName_lbl.Text = g.GradientColorName;
            fontColor_lbl.Text = g.FontForeColor.Name;
            color1_lbl.Text = g.Color1.Name;
            color2_lbl.Text = g.Color2.Name;
            angle_lbl.Text = g.ColorAngle.ToString();
        }

        private void ok_btn_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> valuesDic = new Dictionary<string, object>();
            valuesDic.Add("Ok", true);
            valuesDic.Add("CurrentTheme", t.GetCurrentTheme());
            gradientColorChangeEvent(valuesDic);
            this.Close();
        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ThemesForm_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = t.CurrentTheme;
        }

        private void newTheme_btn_Click(object sender, EventArgs e)
        {
            Desktop.Forms.GradientForm g = new Forms.GradientForm();
            g.ShowDialog();
            if (g.isOk)//if the bool isOk is true
            {
                string[] themeNames = t.GetAllThemeNames();
                byte tempCurrentThemeIndx = t.CurrentTheme;
                comboBox1.DataSource = themeNames;
                comboBox1.SelectedIndex = tempCurrentThemeIndx;
            }
        }

    }
    public class GradientColor
    {
        public System.Drawing.Color Color1;
        public System.Drawing.Color Color2;
        public float ColorAngle;
        public System.Drawing.Color FontForeColor;
        public string GradientColorName;
        public GradientColor()
        {

        }
        public GradientColor(System.Drawing.Color Color1, System.Drawing.Color Color2, System.Drawing.Color FontForeColor, float ColorAngle, string GradientColorName)
        {
            this.Color1 = Color1;
            this.Color2 = Color2;
            this.FontForeColor = FontForeColor;
            this.ColorAngle = ColorAngle;
            this.GradientColorName = GradientColorName;
        }
    }

    public class Themes
    {
        static string themeFileName = @"Theme.json";
        private static string[] ThemesArr = null;
        private static Themes themes;
        public List<GradientColor> ThemesList;
        public byte CurrentTheme = byte.MinValue;
        private Themes()
        {
        }

        /// <summary>
        /// to add default themes if themes.json file is not there
        /// </summary>
        private static void AddDefaultThemes()
        {
            GradientColor theme1 = new GradientColor(Color.Black, Color.Gray, Color.White, 180F, "Theme 1");
            GradientColor theme2 = new GradientColor(Color.Crimson, Color.Brown, Color.White, 30F, "Theme 2");
            GradientColor theme3 = new GradientColor(Color.Green, Color.Blue, Color.White, 30F, "Theme 3");
            GradientColor theme4 = new GradientColor(Color.OrangeRed, Color.DarkBlue, Color.White, 30F, "Theme 4");
            GradientColor theme5 = new GradientColor(Color.DarkMagenta, Color.Purple, Color.White, 30F, "Theme 5");
            GradientColor theme6 = new GradientColor(Color.Black, Color.Black, Color.White, 30F, "Default");
            themes.CurrentTheme = 0;
            themes.ThemesList = new List<GradientColor>();
            themes.ThemesList.Add(theme1);
            themes.ThemesList.Add(theme2);
            themes.ThemesList.Add(theme3);
            themes.ThemesList.Add(theme4);
            themes.ThemesList.Add(theme5);
            themes.ThemesList.Add(theme6);
        }
        
        private static bool Deserialize(string themeFileName)
        {
            bool returnVal = false;
            if (File.Exists(themeFileName))//checks for the file exists
            {
                try
                {
                    string themeContentsStr = File.ReadAllText(themeFileName);
                    themes = (Themes)JsonConvert.DeserializeObject(themeContentsStr, typeof(Themes));
                    List<GradientColor> nullThemeList = new List<GradientColor>();
                    foreach (var item in themes.ThemesList)
                    {
                        if (item.Color1 != null && item.Color2 != null && item.ColorAngle != null && item.FontForeColor != null && !string.IsNullOrEmpty(item.GradientColorName))
                            nullThemeList.Add(item);
                    }
                    themes.ThemesList = nullThemeList;
                    if (themes.ThemesList.Count == 0)
                        returnVal = false;
                    else
                        returnVal = true;
                }
                catch (Exception)
                {
                    themes = new Themes();
                   
                }
            }
            else
                 themes = new Themes();
            return returnVal;
        }
        public void SerializeTheme()
        {
            string themeJson = JsonConvert.SerializeObject(themes);
            System.IO.File.WriteAllText(themeFileName, themeJson);
        }
        public  static Themes GetInstance()
        {
            if (themes == null)
            {
                if (!Deserialize(themeFileName))
                    AddDefaultThemes();
            }
            return themes;
        }
        public static void SetInstance(Themes themesValue)
        {
            themes = themesValue;
        }
        public GradientColor GetCurrentTheme()
        {
            return themes.ThemesList[CurrentTheme];
        }

        /// <summary>
        /// to add new custom themes
        /// </summary>
        /// <param name="g"></param>
        /// <param name="setCurrentTheme"></param>
        public void AddNewTheme(GradientColor g, bool setCurrentTheme)
        {
            themes.ThemesList.Add(g);
            if (setCurrentTheme)
            {
                themes.CurrentTheme = (byte)themes.ThemesList.IndexOf(g);
            }

        }

        /// <summary>
        /// to get all theme names
        /// </summary>
        /// <returns></returns>
        public string[] GetAllThemeNames()
        {
            string[] themeNames = new string[themes.ThemesList.Count];
            int indx = 0;
            foreach (GradientColor g in themes.ThemesList)
            {
                    themeNames[indx] = g.GradientColorName;
                    indx++;
            }
            return themeNames;
        }

    }
    public class XmlConfigUtility
    {
        public static void Serialize(Object data, string fileName)
        {
            Type type = data.GetType();
            XmlSerializer xs = new XmlSerializer(type);
            XmlTextWriter xmlWriter = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);
            xmlWriter.Formatting = System.Xml.Formatting.Indented;
            xs.Serialize(xmlWriter, data);
            xmlWriter.Close();
        }

        public static Object Deserialize(Type type, string fileName)
        {
            Object data = null;
            XmlSerializer xs = null;
            XmlTextReader xmlReader = null;
            try
            {
                xs = new XmlSerializer(type);
                xmlReader = new XmlTextReader(fileName);
                data = xs.Deserialize(xmlReader);
            }
            finally
            {
                xmlReader.Close();
            }
            return data;
        }
    }
}
