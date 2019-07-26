using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using IVLTemplateCreator.IVLControls;
using System.IO;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using ReportUtils;
using System.Globalization;
using System.Resources;

using IVLReport;
namespace IVLTemplateCreator
{
    public partial class TemplateForm : Form
    {
        IVL_Label label;
        IVL_Bitmap logo;
        IVLTemplateCreator.IVLControls.IVL_ImageTable imgTable;
        IVL_TextBox textBox;
        IVL_TabelLayout tabLayout;
        //IVL_TabelLayout tableLayout;
        Control selectedControl;
        string culturalInfo = string.Empty;
        ControlProperties IVLProps = new ControlProperties();
        List<ControlProperties> propList = new List<ControlProperties>();
        FileInfo[] templateFiles;
        string currentTemplate = "";
        bool isReportLoading = false;
        string fontstyle2 = string.Empty;
       static ResourceManager LangResourceManager;
      static CultureInfo LangResourceCultureInfo;
        string[] imgFiles;

        public TemplateForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TaskBar.SetTaskbarState(TaskBar.AppBarStates.AutoHide);
            LangResourceManager = new ResourceManager("IVLTemplateCreator.LanguageResources.Res", typeof(TemplateForm).Assembly);
           
            
            //getAvailableTemplates();
            //LangResourceCultureInfo=
            //LangResourceCultureInfo = CultureInfo.CreateSpecificCulture("en");
            culturalInfo = "en";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            TaskBar.SetTaskbarState(TaskBar.AppBarStates.AlwaysOnTop);
        }

        private void portrait_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (portrait_rb.Checked)
                LayoutDetails.Current.Orientation = LayoutDetails.PageOrientation.PORTRAIT;
            else
                LayoutDetails.Current.Orientation = LayoutDetails.PageOrientation.LANDSCAPE;
         
            createReportLayout();
        }

        private void noOfImg_cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            LayoutDetails.Current.ReportImgCnt =int.Parse( noOfImg_cbx.SelectedItem.ToString()) ;
            imgFiles = new string[LayoutDetails.Current.ReportImgCnt];
            for (int i = 0; i < LayoutDetails.Current.ReportImgCnt; i++)
            {
                imgFiles[i] = "test.jpg";
            }
            IVLProps = getControlProp(imgTable);

            reportCanvas_pnl.Controls.Remove(imgTable);

            //initIVLProps("ImgBox");
            addImgBox(IVLProps);
        }

        private void write2Xml(string xmlFile)
        {
            TextWriter xmlWriter = new StreamWriter(xmlFile);
            XmlSerializer xmlSer = new XmlSerializer(typeof(ControlProperties));

            foreach (Control item in reportCanvas_pnl.Controls)
            {
                IVLProps = getControlProp(item);
                xmlSer.Serialize(xmlWriter, IVLProps);
                xmlWriter.Flush();
            }
            xmlWriter.Close();

            StreamReader xmlRead = new StreamReader(xmlFile);
            string str = xmlRead.ReadToEnd();
            xmlRead.Close();
            Regex r = new Regex("\\<\\?(.*?)>", RegexOptions.Multiline);
            string x = r.Replace(str, "");
            Regex r1 = new Regex("xmlns(.*?)XMLSchema\"", RegexOptions.Multiline);
            string y = r1.Replace(x, "");
            y = "<Layout>" + y + "</Layout>";
            string oriantationStr = "<Orientation>" + LayoutDetails.Current.Orientation + "</Orientation>";
            string imgCnt = "<NoOfImg>" + LayoutDetails.Current.ReportImgCnt + "</NoOfImg>";

            y = "<ReportTemplate>" + oriantationStr + imgCnt + y + "</ReportTemplate>";
            StreamWriter finalWriter = new StreamWriter(xmlFile);
            finalWriter.Write(y);
            finalWriter.Close();
        }
        private void parseXmlData(string xmlFile)
        {
            if (!File.Exists(xmlFile)) return;
            this.Cursor = Cursors.WaitCursor;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFile);
            string ori = xmlDoc.FirstChild.ChildNodes[0].InnerText;
            string imgCnt = xmlDoc.FirstChild.ChildNodes[1].InnerText;
            LayoutDetails.Current.ReportImgCnt = int.Parse(imgCnt);
            imgFiles = new string[LayoutDetails.Current.ReportImgCnt];
            for (int i = 0; i < LayoutDetails.Current.ReportImgCnt; i++)
            {
                imgFiles[i] = "test.jpg";
            }
            if (ori == "LANDSCAPE")
            {
                LayoutDetails.Current.Orientation = LayoutDetails.PageOrientation.LANDSCAPE;
            }
            else
            {
                LayoutDetails.Current.Orientation = LayoutDetails.PageOrientation.PORTRAIT;
            }
            createReportLayout();
            isReportLoading = true;
            XmlNode controlNodes = xmlDoc.FirstChild.ChildNodes[2];
            XmlSerializer xmlSer = new XmlSerializer(typeof(ControlProperties));
            foreach (XmlNode item in controlNodes.ChildNodes)
            {
                MemoryStream memStearm = new MemoryStream();
                StreamWriter writer = new StreamWriter(memStearm);
                writer.Write(item.OuterXml);
                writer.Flush();
                memStearm.Position = 0;
                IVLProps = (ControlProperties)xmlSer.Deserialize(memStearm);
                populateControls(IVLProps);
            }
            isReportLoading = false;

            this.Cursor = Cursors.Default;
            this.reportCanvas_pnl.Refresh();
        }

        private ControlProperties getControlProp(Control c)
        {
            IVLProps = new ControlProperties();
            IVLProps.Name = c.Name;
            IVLProps.BindingType = c.Tag.ToString();
            IVLProps.X = c.Location.X;
            IVLProps.Y = c.Location.Y;
            IVLProps.Width = c.Width;
            IVLProps.Height = c.Height;
            IVLProps.Text = c.Text;
            if (c.Name.Contains("TableLayout"))
            {
                IVL_TabelLayout tableLayout = c as IVL_TabelLayout;
                //tableLayout = c as IVL_TabelLayout; 
                IVLProps.Columns = tableLayout.ColumnCount;
                IVLProps.Rows = tableLayout.RowCount;
            }
            IVLProps.ForeColor = c.ForeColor.Name;
            IVLProps.FontName = c.Font.FontFamily.Name;
            IVLProps.FontStyle = c.Font.Style.ToString();
            IVLProps.FontSize = (int)c.Font.Size;
            IVLProps.ImageName = c.AccessibleDescription;
            IVLProps.Type = c.GetType().ToString();
            return IVLProps;
        }
        FontStyle font_style = new FontStyle();
        string[] s1 = new string[2];
        public void Font_style(string fontstyle)
        {
            if (fontstyle.Contains(','))
            {
                s1=fontstyle.Split(',');
                fontstyle = s1[0] + s1[1].TrimStart(' ');
            }
            switch (fontstyle)
            {
                case "Bold": font_style = FontStyle.Bold;
                    break;
                case "Regular": font_style = FontStyle.Regular;
                    break;
                case "Italic": font_style = FontStyle.Italic;
                    break;
                case "Strikeout": font_style = FontStyle.Strikeout;
                    break;
                case "Underline": font_style = FontStyle.Underline;
                    break;
                case "BoldRegular": font_style = FontStyle.Bold | FontStyle.Regular;
                    break;
                case "BoldItalic": font_style = FontStyle.Bold | FontStyle.Italic;
                    break;
                case "BoldStrikeout": font_style = FontStyle.Bold | FontStyle.Strikeout;
                    break;
                case "BoldUnderline": font_style = FontStyle.Bold | FontStyle.Underline;
                    break;
                case "RegularItalic": font_style = FontStyle.Regular | FontStyle.Italic;
                    break;
                case "RegularBold": font_style = FontStyle.Regular | FontStyle.Bold;
                    break;
                case "RegularStrikeout": font_style = FontStyle.Regular | FontStyle.Strikeout;
                    break;
                case "RegularUnderline": font_style = FontStyle.Regular | FontStyle.Underline;
                    break;
                case "ItalicBold": font_style = FontStyle.Italic | FontStyle.Bold;
                    break;
                case "ItalicRegular": font_style = FontStyle.Italic | FontStyle.Regular;
                    break;
                case "ItalicStrikeout": font_style = FontStyle.Italic | FontStyle.Strikeout;
                    break;
                case "ItalicUnderline": font_style = FontStyle.Italic | FontStyle.Underline;
                    break;
                case "StrikeoutBold": font_style = FontStyle.Strikeout | FontStyle.Bold;
                    break;
                case "StrikeoutRegular": font_style = FontStyle.Strikeout | FontStyle.Regular;
                    break;
                case "StrikeoutItalic": font_style = FontStyle.Strikeout | FontStyle.Italic;
                    break;
                case "StrikeoutUnderline": font_style = FontStyle.Strikeout | FontStyle.Underline;
                    break;
                case "UnderlineRegular": font_style = FontStyle.Underline | FontStyle.Regular;
                    break;
                case "UnderlineItalic": font_style = FontStyle.Underline | FontStyle.Italic;
                    break;
                case "UnderlineBold": font_style = FontStyle.Underline | FontStyle.Bold;
                    break;
                case "UnderlineStrikeout": font_style = FontStyle.Underline | FontStyle.Strikeout;
                    break;
            }
        }
        private void setControlProp(Control c,ControlProperties IVLProps)
        {
            //if(!string.IsNullOrEmpty(IVLProps.Text))
             c.Text = IVLProps.Text;
             c.Location = new Point((int)xlocation_nud.Value, (int)yLocation_nud.Value);
             c.Size = new System.Drawing.Size((int)width_nud.Value, (int)height_nud.Value);
             c.Font = new Font(IVLProps.FontName, IVLProps.FontSize, font_style);
            c.ForeColor = Color.FromName(IVLProps.ForeColor);
            c.Tag = IVLProps.BindingType;
            font_style = FontStyle.Regular;
        }
        private void populateControls(ControlProperties IVLProps)
        {
            LangResourceCultureInfo = CultureInfo.CreateSpecificCulture(culturalInfo);
            if (IVLProps.Name.Contains("Bitmap"))
            {
                addLogo(IVLProps);
            }
            else if (IVLProps.Name.Contains("TextBox"))
            {
                addTextBox(IVLProps);
            }
            else if (IVLProps.Name.Contains("Label"))
            {
                addLabel(IVLProps);
            }
            else if (IVLProps.Name.Contains("ImgBox"))
            {
                addImgBox(IVLProps);
            }
            else if (IVLProps.Name.Contains("TableLayout"))
            {
                addTabelLayout(IVLProps);
            }
            propList.Add(IVLProps);
        }
        private void initIVLProps(String type)
        {
             IVLProps = new ControlProperties();

            IVLProps.Name = type + propList.Count.ToString();
            IVLProps.X = LayoutDetails.LeftMargin;
            IVLProps.Y = LayoutDetails.TopMargin;
            IVLProps.Text = IVLProps.Name;
            // xp.BackgroundColor = "White";
            IVLProps.FontSize = 10;
            switch (type)
            {
                case "ImgBox":
                       IVLProps.X = LayoutDetails.LeftMargin;
                       IVLProps.Y = LayoutDetails.TopMargin+100;
                       bool isPortrait = LayoutDetails.Current.Orientation == LayoutDetails.PageOrientation.PORTRAIT;
                       if (isPortrait)
                       {
                           IVLProps.Width = 480;
                           IVLProps.Height = 360;
                       }
                       else
                       {
                           IVLProps.Width = 800;
                           IVLProps.Height = 600;
                       }
                    break;

                case "TextBox":
                    IVLProps.Width = 100;
                    IVLProps.Height = 30;
                    IVLProps.ForeColor = Color.Black.Name;
                    break;

                case "Label":
                    IVLProps.Width = 100;
                    IVLProps.Height = 30;
                    IVLProps.ForeColor = Color.Black.Name;
                    break;

                case "Bitmap":
                    IVLProps.Width = 100;
                    IVLProps.Height = 75;
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "JPG (*.jpg,*.jpeg)|*.jpg;*.jpeg|PNG (*.png,*.png)|*.png;*.png|TIFF (*.tif,*.tiff)|*.tif;*.tiff|BMP (*.bmp)|PNG (*.png)";
                    if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    IVLProps.ImageName = ofd.FileName;
                    break;
                case "TableLayout":
                    IVLProps.Width = 200;
                    IVLProps.Height = 130;
                    IVLProps.ForeColor = Color.Black.Name;
                    break;
                }
            propList.Add(IVLProps);
        }
        private void createReportLayout()
        {
            reportCanvas_pnl.Controls.Clear();
            propList.Clear();
            if (LayoutDetails.Current.Orientation == LayoutDetails.PageOrientation.PORTRAIT)
            {
                reportCanvas_pnl.Width = LayoutDetails.Portrait_Width;
                reportCanvas_pnl.Height = LayoutDetails.Portrait_Height;
                reportCanvas_pnl.Dock = DockStyle.None;
                int diff = (reportCanvas_pnl.Parent.Width - toolBox_pnl.Width) / 2 - reportCanvas_pnl.Width / 2;
                reportCanvas_pnl.Location = new Point(diff, 0);
            }
            else
            {
                reportCanvas_pnl.Width = LayoutDetails.Landscape_Width;
                reportCanvas_pnl.Height = LayoutDetails.Landscape_Height;
                reportCanvas_pnl.Dock = DockStyle.Left;
            }
        }

        private void addLabel(ControlProperties IVLProps)
        {
            label = new IVL_Label();
            label.Tag = IVLProps.BindingType;
            
            label.Name = IVLProps.Name;
            
            label.Size = new Size(IVLProps.Width, IVLProps.Height);
            label.Location = new Point(IVLProps.X, IVLProps.Y);
            label.Text = IVLProps.Text;
           
            Font_style(IVLProps.FontStyle);
            label.Font = new Font(IVLProps.FontName, IVLProps.FontSize,font_style);
            label.ForeColor = Color.FromName(IVLProps.ForeColor);
            label.BorderStyle = BorderStyle.None;
            label.Margin = new Padding(0);
            //label.AutoSize = false;
            if (string.IsNullOrEmpty(IVLProps.BindingType))
            {
                if (IVLProps.Text.Equals("Name:"))
                {
                   
                    {
                        label.Text = LangResourceManager.GetString("Report_Name_Text", LangResourceCultureInfo); ;
                      
                    }
                }
                else
                    if (IVLProps.Text.Equals("Age:"))
                    {
                        
                        {
                            label.Text = LangResourceManager.GetString("Report_Age_Text", LangResourceCultureInfo);
                        
                        }

                    }
                    else if (IVLProps.Text.Equals("Date:"))
                    {
                       
                        {
                            label.Text = LangResourceManager.GetString("Report_DateTemplate_Text", LangResourceCultureInfo);
                       
                        }

                    }
                    else if (IVLProps.Text.Equals("Doctor:"))
                    {
                        
                        {
                            label.Text = LangResourceManager.GetString("Report_Doctor_Text", LangResourceCultureInfo);
                           
                        }
                    }
                    else if (IVLProps.Text.Equals("Comments:"))
                    {
                        
                        {
                            label.Text = LangResourceManager.GetString("Report_Comments_Text", LangResourceCultureInfo);
                          
                        }
                    }
                    else if (IVLProps.Text.Equals("Gender:"))
                    {
                      
                        {
                            label.Text = LangResourceManager.GetString("Report_Gender_Text", LangResourceCultureInfo);
                            

                        }
                    }
                    else if (IVLProps.Text.Equals("(Max 85 characters)"))
                    {
                       
                        {
                            label.Text = LangResourceManager.GetString("ReportMax_Charecters", LangResourceCultureInfo);
                            
                        }
                    }
            }
            this.reportCanvas_pnl.Controls.Add(label);
            label.MouseCaptureChanged += label_MouseCaptureChanged;
            label_MouseCaptureChanged(label, null);
        }
        private void addTextBox(ControlProperties IVLProps)
        {
            textBox = new IVL_TextBox();
            textBox.Name = IVLProps.Name;
            textBox.Tag = IVLProps.BindingType;
            textBox.Size = new Size(IVLProps.Width, IVLProps.Height);
            textBox.Location = new Point(IVLProps.X, IVLProps.Y);
            textBox.Text = IVLProps.Text;
            textBox.Font = new Font(IVLProps.FontName, IVLProps.FontSize);
            textBox.ForeColor = Color.FromName(IVLProps.ForeColor);
            textBox.BorderStyle = BorderStyle.Fixed3D;
            textBox.Margin = new Padding(0);
            
            this.reportCanvas_pnl.Controls.Add(textBox);
            textBox.MouseCaptureChanged += textBox_MouseCaptureChanged;
            textBox_MouseCaptureChanged(textBox, null);
        }
        private void addImgBox(ControlProperties IVLProps)
        {
            bool isPortrait = LayoutDetails.Current.Orientation == LayoutDetails.PageOrientation.PORTRAIT;

            IVLTemplateCreator.IVLControls.IVL_ImageTable imgTable = new IVLTemplateCreator.IVLControls.IVL_ImageTable(imgFiles, isPortrait);
            imgTable.Tag = IVLProps.BindingType;
            imgTable.Name = IVLProps.Name;
            imgTable.Location = new Point(IVLProps.X, IVLProps.Y);
            imgTable.Size = new Size(IVLProps.Width, IVLProps.Height);
            imgTable.AccessibleDescription = IVLProps.ImageName;//Place for ImageLocation

            this.reportCanvas_pnl.Controls.Add(imgTable);
            imgTable.MouseCaptureChanged += imgTable_MouseCaptureChanged;
            imgTable_MouseCaptureChanged(imgTable, null);
        }
        private void addTabelLayout(ControlProperties IVLProps)
        {
            bool isPortrait = LayoutDetails.Current.Orientation == LayoutDetails.PageOrientation.PORTRAIT;
             tabLayout = new IVL_TabelLayout();
            propertyGrid1.SelectedObject = tabLayout;
            tabLayout.Tag = IVLProps.BindingType;
            tabLayout.Name = IVLProps.Name;
            tabLayout.Location = new Point(IVLProps.X, IVLProps.Y);
            tabLayout.Size = new Size(IVLProps.Width, IVLProps.Height);
            tabLayout.rowPercent = (int)rowStylepercent_nud.Value;
            tabLayout.colPercent = (int)colStylePercent_nud.Value;
            //tabLayout.rows = (int)rowCount_nud.Value;
            //tabLayout.cols = (int)colCount_nud.Value;
            tabLayout.RowCount = IVLProps.Rows;
            tabLayout.ColumnCount = IVLProps.Columns;
            this.reportCanvas_pnl.Controls.Add(tabLayout);
            tabLayout.MouseCaptureChanged += tabLayout_MouseCaptureChanged;
            tabLayout_MouseCaptureChanged(tabLayout, null);
        }

        void tabLayout_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (isReportLoading) return;
            tabLayout = sender as IVL_TabelLayout;
            selectedControl = tabLayout;
            tabLayout.BringToFront();
            drawSelectionRect();  
        }
        private void addLogo(ControlProperties IVLProps)
        {
            logo = new IVL_Bitmap();
            logo.AccessibleDescription = IVLProps.ImageName;//Place for ImageLocation
            logo.Name = IVLProps.Name;
            logo.Tag = IVLProps.BindingType;
            logo.Location = new Point(IVLProps.X, IVLProps.Y);
            logo.Size = new Size(IVLProps.Width, IVLProps.Height);
            logo.ImageLocation = IVLProps.ImageName;
            this.reportCanvas_pnl.Controls.Add(logo);
             logo.MouseCaptureChanged += logo_MouseCaptureChanged;
            logo_MouseCaptureChanged(logo, null);
        }

        void label_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (isReportLoading) return;
            label = sender as IVL_Label;
            selectedControl = label;
            IVLProps = getControlProp(label);
            label.BringToFront();
            showControlProperties();
            drawSelectionRect();  
        }
        void logo_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (isReportLoading) return;
            logo = sender as IVL_Bitmap;
            selectedControl = logo;
            logo.BringToFront();
            drawSelectionRect();  
         }
        void textBox_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (isReportLoading) return;
            textBox = sender as IVL_TextBox;
            selectedControl = textBox;
            IVLProps = getControlProp(textBox);
            textBox.BringToFront();
            showControlProperties();
            drawSelectionRect();  
         }
        void imgTable_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (isReportLoading) return;
            imgTable = sender as IVLTemplateCreator.IVLControls.IVL_ImageTable;
            selectedControl = imgTable;
            drawSelectionRect();
        }
       private void drawSelectionRect()
        {
            this.reportCanvas_pnl.Refresh();
            Graphics g = Graphics.FromHwnd(selectedControl.Handle);
            Pen pen = new Pen(Color.Red, 2);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            g.DrawRectangle(pen, new Rectangle(0, 0, selectedControl.Width - 2, selectedControl.Height - 2));
        }
        private void showControlProperties()
        {
            if( controlText_tbx.Text != IVLProps.Text)
            controlText_tbx.Text = IVLProps.Text;
            fontName_cbx.Text = IVLProps.FontName;
            xlocation_nud.Value = IVLProps.X;
            yLocation_nud.Value = IVLProps.Y;
            width_nud.Value = IVLProps.Width;
            height_nud.Value = IVLProps.Height;
            if (IVLProps.FontStyle.Contains(','))
            {
                string[] str = IVLProps.FontStyle.Split(',');
                Fontstyle_cbx.Text = str[0];
                Fontstyle_cbx_2.Text = str[1].TrimStart(' ');
            }
            else
            {
                Fontstyle_cbx.Text = IVLProps.FontStyle;
                Fontstyle_cbx_2.Text = string.Empty;
                Fontstyle_cbx_2.Text = IVLProps.FontStyle;
            }
            fontSize_cbx.Text = IVLProps.FontSize.ToString();
            colorPicker_btn.BackColor = Color.FromName(IVLProps.ForeColor);
            binding_cbx.Text = IVLProps.BindingType;
        }
        private void lbl_btn_Click(object sender, EventArgs e)
        {
            initIVLProps("Label");
            addLabel(IVLProps);
        }
        private void textbox_btn_Click(object sender, EventArgs e)
        {
            initIVLProps("TextBox");
            addTextBox(IVLProps);
        }

        private void bitmap_btn_Click(object sender, EventArgs e)
        {
            initIVLProps("Bitmap");
            addLogo(IVLProps);
        }
        private void save_btn_Click(object sender, EventArgs e)
        {
            string dir = "ReportTemplates";
            DirectoryInfo dInf=new DirectoryInfo(dir);
            if (!Directory.Exists(dir))
               dInf= Directory.CreateDirectory(dir);
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = dInf.FullName;
            sfd.DefaultExt = ".xml";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                write2Xml(sfd.FileName);
            }
        }

        private void getAvailableTemplates()
        {
            string templatePath = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Application.ExecutablePath + Path.DirectorySeparatorChar+"ReportTemplates";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                templatePath = ofd.FileName;
            else
                return;
            System.IO.FileInfo finf = new FileInfo(templatePath);
            DirectoryInfo dInf = (finf.Directory);
            templateFiles = dInf.GetFiles();
            templates_cbx.Items.AddRange(templateFiles);
        }

        private void templates_cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentTemplate = templateFiles.Where(x => x.Name == templates_cbx.SelectedItem.ToString()).ToArray()[0].FullName;
           
            parseXmlData(currentTemplate);
        }

        private void controlText_tbx_TextChanged(object sender, EventArgs e)
        {
            IVLProps.Text = controlText_tbx.Text;
            setControlProp(selectedControl, IVLProps);
        }

        private void fontSize_cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            int font = int.Parse( fontSize_cbx.Text);
            IVLProps.FontSize = font;
            setControlProp(selectedControl, IVLProps);
        }

        private void colorPicker_btn_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                IVLProps.ForeColor= cd.Color.Name;
                setControlProp(selectedControl, IVLProps);
                colorPicker_btn.BackColor = cd.Color;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Delete)
            {
                reportCanvas_pnl.Controls.Remove(selectedControl);
                selectedControl = null;
            }
        }

        private void binding_cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            IVLProps.BindingType = binding_cbx.SelectedItem.ToString();
            IVLProps.Text = IVLProps.BindingType;
            setControlProp(selectedControl, IVLProps);
        }

        private void fontName_cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            IVLProps.FontName = fontName_cbx.SelectedItem.ToString();
            setControlProp(selectedControl, IVLProps);
        }

        private void reportCanvas_pnl_Click(object sender, EventArgs e)
        {
            this.reportCanvas_pnl.Refresh();
        }

        private void formButtons1_Click(object sender, EventArgs e)
        {
            getAvailableTemplates();
        }

        private void binding_cbx_TextChanged(object sender, EventArgs e)
        {
            IVLProps.BindingType = binding_cbx.Text;
            if(!string.IsNullOrEmpty(IVLProps.BindingType))
            IVLProps.Text = IVLProps.BindingType;
            setControlProp(selectedControl, IVLProps);
        }
        private void tableLayoutPanel_btn_Click(object sender, EventArgs e)
        {
             initIVLProps("TableLayout");
            addTabelLayout(IVLProps);
        }

        FontDialog dlg = new FontDialog();
        

        private void button1_Click(object sender, EventArgs e)
        {
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
            }
        }


        private void Fontstyle_cbx_TextChanged(object sender, EventArgs e)
        {
            Ivlprop_fontstyle();
        }

        private void Fontstyle_cbx_2_TextChanged(object sender, EventArgs e)
        {
            Ivlprop_fontstyle();
        }
        public void Ivlprop_fontstyle()
        {
            if (Fontstyle_cbx_2.Text.Equals(Fontstyle_cbx.Text))
            {
                IVLProps.FontStyle = Fontstyle_cbx.Text;
            }
            else
            {
                IVLProps.FontStyle = Fontstyle_cbx.Text + Fontstyle_cbx_2.Text;
             }
            Font_style(IVLProps.FontStyle);
            setControlProp(selectedControl, IVLProps);
        }
        Dictionary<string, object> reportDic;
        Report _report;
        private void preview_btn_Click(object sender, EventArgs e)
        {
            
            reportDic = new Dictionary<string, object>();
            string[] reportTemplates = new string[templateFiles.Length];

            for (int i = 0; i < templateFiles.Length; i++)
            {
                reportTemplates[i] = templateFiles[i].FullName;
            }
         
            reportDic.Add("$allTemplateFiles", reportTemplates);
            reportDic.Add("$currentTemplate", reportTemplates[templates_cbx.SelectedIndex]);
            reportDic.Add("$name", "Harinarayanan Satyanarayanan");
            reportDic.Add("$age", "27");
            reportDic.Add("$mrn", "ivl_Test");
            reportDic.Add("$isFromCDR", false);//isFromCDR is added to make the report to know from where it is being invoked.
            reportDic.Add("$date", DateTime.Now.ToString("dd-MM-yyyy"));
            reportDic.Add("$gender", "Male");
            reportDic.Add("$hospital", "SRI LAKSHMI SPECALITY");
            reportDic.Add("$operator", "DARSHAN");
            reportDic.Add("$comments", string.Empty);

            string[] image = {"OS 1"};
            string[] currentReportImageFiles = { "test.png" };
            
            reportDic.Add("$CurrentImageFiles", currentReportImageFiles);
            reportDic.Add("$ImageNames", image);
            if (reportTemplates[templates_cbx.SelectedIndex].Contains("CDR_ls.xml"))
            {
               
                //Below code has been added to send patient details and CDR measurement value to the report.

               
                //This below if statement has been added to send comments from CDR form to CDR report.
             
               
                reportDic.Add("$discArea", "2.0mm");
                reportDic.Add("$cupArea", "2.0mm");
                reportDic.Add("$rimArea", "2.0mm");
                reportDic.Add("$horizontalCupLength", "2.0mm");
                reportDic.Add("$verticalCupLength", "2.0mm");
                reportDic.Add("$horizontalDiscLength", "2.0mm");
                reportDic.Add("$verticalDiscLength", "2.0mm");
                reportDic.Add("$horizontalCDR", "2.0mm");

                reportDic.Add("$verticalCDR", "2.0mm");
                //if (isOD)
                //{
                //    reportDic.Add("$nasal", "2.0mm");
                //    reportDic.Add("$temporal", "2.0mm");
                //}
                //else
                {
                    reportDic.Add("$nasal", "2.0mm");
                    reportDic.Add("$temporal", "2.0mm");
                }
                reportDic.Add("$superior", "2.0mm");
                reportDic.Add("$inferior", "2.0mm");
                        
              
                //This below if statement will send the text present in Reported by text to the operator field in CDR report.

                reportDic.Remove("$isFromCDR");
               
            
                reportDic.Add("$isFromCDR", true);//isFromCDR is added to make the report to know from where it is being invoked.

                
            }
            _report = new IVLReport.Report(reportDic);


//#if DEBUG
//            _report.TopMost = false;
//#else
//                _report.TopMost = true ;

//#endif
            _report.ShowDialog();
        }

        private void xlocation_nud_ValueChanged(object sender, EventArgs e)
        {
            setControlProp(selectedControl, IVLProps);

        }

        private void yLocation_nud_ValueChanged(object sender, EventArgs e)
        {
            setControlProp(selectedControl, IVLProps);

        }

        private void width_nud_ValueChanged(object sender, EventArgs e)
        {
            setControlProp(selectedControl, IVLProps);

        }

        private void height_nud_ValueChanged(object sender, EventArgs e)
        {
            setControlProp(selectedControl, IVLProps);

        }

        private void language_cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void language_cbx_SelectionChangeCommitted(object sender, EventArgs e)
        {
            LangResourceCultureInfo = CultureInfo.CreateSpecificCulture(language_cbx.Text);
            culturalInfo = language_cbx.Text;
            parseXmlData(currentTemplate);
        }
    }
}
