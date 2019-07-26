using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PdfFileWriter;
using System.Diagnostics;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.IO;
using INTUSOFT.ImageListView;
using System.Reflection;
using IVLReport;
using ReportUtils;
using ToolBoxLib;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Windows.Markup;
using DesignSurfaceExt;
using DesignSurfaceManagerExtension;
using pDesigner;
using PdfFileWriter;
namespace ReportTemplateCreator
{
    public partial class ReportTemplateCreatorWindow : Form
    {
        static Form rootComponent = null;//Assosiated with the Designsurface.
        Label label = null;//When label is added on the form a new object of label will be created.
        PictureBox logo = null;//When PictureBox is added on the form a new object of type PictureBox will be created.
        IVL_ImageTable imgTable = null;//When IVL_ImageTable is added on the form a new object of type IVL_ImageTable will be created.
        System.Windows.Forms.TextBox textBox = null;//When TextBox is added on the form a new object of type TextBox will be created.
        TableLayoutPanel tabLayout = null;//When TextBox is added on the form a new object of type TextBox will be created.
        Control SelectedControl = null;//Save the selected control in the form.
        FileInfo[] templateFiles = null;//Gets the list of all template files in the specified directory. 
        IDesignerHost idh;
        ControlProperties IVLProps;
        Report report;
        static int A4LanscapeWidth = 842;
        static int A4LandscapeHeight = 595;
        static int A4PortraitWidth = 595;
        static int A4PortraitHeight = 842;
        static int A5LanscapeWidth = 595;
        static int A5LandscapeHeight = 419;
        static int A5PortraitWidth = 419;
        static int A5PortraitHeight = 595;
        PdfGenerator pdfGenerator;
        
        static int marginValue = 3;
        static bool isValueChanged = false;
        string ReportFileName = "Report.pdf";
        string toolSelected = string.Empty;
        int MappingValue;
        private PdfDocument Document;
        private PdfPage Page;
        private PdfContents Contents;
        private PdfFont ArialNormal;
        string[] imgFiles;
        FontStyle font_style;
        ReportControlsStructure repoCtrlProp;
        string templateFile;
        public pDesigner.pDesigner pDesignerCore;// = new pDesigner.pDesigner();
        private IpDesigner IpDesignerCore = null;
        ToolBox tb;
        [DllImport("User32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int w, int h, bool Repaint);
        Dictionary<string, object> reportData;// = new Dictionary<string, object>();
        double dpi = 72;
        private double a4Width = 11.69;
        private double a4Height = 8.27;

        private double a5Width = 8.27;
        private double a5Height = 5.83;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReportTemplateCreatorWindow()
        {
            InitializeComponent();
            pdfGenerator = new PdfGenerator();
            dpi = 96;
            A4LandscapeHeight = (int) (dpi * a4Height);
            A4LanscapeWidth =(int)( dpi * a4Width);

            A4PortraitHeight = (int)(dpi *a4Width );
            A4PortraitWidth = (int)(dpi * a4Height);

            A5LandscapeHeight = (int)(dpi * a5Height);
            A5LanscapeWidth = (int)(dpi * a5Width);

            A5PortraitHeight = (int)(dpi * a5Width);
            A5PortraitWidth= (int)(dpi * a5Height);
            
            this.KeyPreview = true;
            pDesignerCore = new pDesigner.pDesigner();
            IpDesignerCore = this.pDesignerCore as IpDesigner;
            pDesignerCore.Parent = this.Form_pnl;
            tb = new ToolBox(new Size(100, 800), 18, null);
            font_style = new FontStyle();
            //this.TopMost = true;
            ToolBoxTab WinForm = new ToolBoxTab();
            //WinForm.ImgList = WinFormsImgList;
            tb.AddTab(WinForm);
            if (File.Exists(@"IconsReportTools\newLogo1.ico"))
                this.Icon = new System.Drawing.Icon(@"IconsReportTools\newLogo1.ico", 256, 256);
            int curimg = 0;
            //this.TopMost = true;
            if(File.Exists("IconsReportTools\\Label_new.png"))
            AddItemToTB("Text Box", curimg++, 0, typeof(Label), "IconsReportTools\\Label_new.png", "Adds a Label");
            else
                AddItemToTB("Text Box", -8029092, 0, typeof(Label), "ImageResources\\IconsReportTools\\Label_new.png", "Adds a Label");

            if (File.Exists("IconsReportTools\\Insert Text Box_new.png"))
            AddItemToTB("Edit Box", curimg++, 0, typeof(System.Windows.Forms.TextBox), "IconsReportTools\\Insert Text Box_new.png", "Adds a Text Box");
            else
                AddItemToTB("Edit Box", -1, 0, typeof(System.Windows.Forms.TextBox), "ImageResources\\IconsReportTools\\Insert Text Box_new.png", "Adds a Text Box");

            if (File.Exists("IconsReportTools\\Picture-Box Icon_New.png"))
            AddItemToTB("Logo", curimg++, 0, typeof(PictureBox), "IconsReportTools\\Picture-Box Icon_New.png", " Adds a Picture Box");
            else
             AddItemToTB("Logo", curimg++, 0, typeof(PictureBox), "ImageResources\\IconsReportTools\\Picture-Box Icon_New.png", " Adds a Picture Box");

            if (File.Exists("IconsReportTools\\Add Table_new.png"))
            AddItemToTB("Table", curimg++, 0, typeof(TableLayoutPanel), "IconsReportTools\\Add Table_new.png", "Adds a Table Layout Panel");
            else
                AddItemToTB("Table", curimg++, 0, typeof(TableLayoutPanel), "ImageResources\\IconsReportTools\\Add Table_new.png", "Adds a Table Layout Panel");

            if (File.Exists("IconsReportTools\\table.png"))
            AddItemToTB("Image Table", curimg++, 0, typeof(IVL_ImagePanel), "IconsReportTools\\table.png", "Adds a IVL ImageTable");
            else
                AddItemToTB("Image Table", curimg++, 0, typeof(IVL_ImagePanel), "ImageResources\\IconsReportTools\\table.png", "Adds a IVL ImageTable");
            //AddItemToTB("Image Table", curimg++, 0, typeof(ImageListView), "IconsReportTools\\table.png", "Adds a IVL ImageTable");

            if (File.Exists("IconsReportTools\\Save Icon.png"))
            AddItemToTB("Save", curimg++, 0, null, "IconsReportTools\\Save Icon.png", "Save a report");
            else
                AddItemToTB("Save", curimg++, 0, null, "ImageResources\\IconsReportTools\\Save Icon.png", "Save a report");

            if (File.Exists("IconsReportTools\\Open-Load_new.png"))
            AddItemToTB("Load", curimg++, 0, null, "IconsReportTools\\Open-Load_new.png", "Opens a saved report");
            else
                AddItemToTB("Load", curimg++, 0, null, "ImageResources\\IconsReportTools\\Open-Load_new.png", "Opens a saved report");
            
            if (File.Exists("IconsReportTools\\Print Icon_New.png"))
            AddItemToTB("Preview", curimg++, 0, null, "IconsReportTools\\Print Icon_New.png", "Prints a report");
             else
            AddItemToTB("Preview", curimg++, 0, null, "ImageResources\\IconsReportTools\\Print Icon_New.png", "Prints a report");

            tb.SelectedTab = 0;
            //- Add the toolboxItems to the future toolbox 
            //- the pointer
            ToolboxItem toolPointer = new System.Drawing.Design.ToolboxItem();
            toolPointer.DisplayName = "<Pointer>";
            toolPointer.Bitmap = new System.Drawing.Bitmap(16, 16);
            tb.SetClickDelegate(new ToolBox.OnToolBoxClick(OnTBClick));
            IpDesignerCore.Toolbox = tb;
            tb.Dock = DockStyle.Fill;
            pDesignerCore.DesignSurfaceManager.updateStatusBar += DesignSurfaceManager_updateStatusBar;
            pDesignerCore.DesignSurfaceManager.UpdateValueChanged+=DesignSurfaceManager_UpdateValueChanged; 

            pDesignerCore._HRulerPixelValue += pDesignerCore__HRulerPixelValue;
            pDesignerCore._HRulerCmValue += pDesignerCore__HRulerCmValue;
            pDesignerCore._VRulerPixelValue += pDesignerCore__VRulerPixelValue;
            pDesignerCore._VRulerCmValue += pDesignerCore__VRulerCmValue;
            this.controls_pnl.Controls.Add(tb);
            this.SetStyle(ControlStyles.Selectable, false);
            a4ToolStripMenuItem.Checked = true;
            DPI1toolStripItem.Checked = true;
            (pDesignerCore as IpDesigner).AddDesignSurface<Form>(A4LanscapeWidth, A4LandscapeHeight, AlignmentModeEnum.SnapLines, new Size(A4LanscapeWidth, A4LandscapeHeight));
            //(pDesignerCore as IpDesigner).AddDesignSurface<Form>(A4PortraitWidth, A4PortraitHeight, AlignmentModeEnum.SnapLines, new Size(A4PortraitWidth, A4PortraitHeight));
           
           
            IDesignSurfaceExt surf =pDesignerCore.DesignSurfaceManager.ActiveDesignSurface;
            {
                ReportTemplateCreatorWindow.CreateControlsUsingTheDesignSurface(surf);
                setStatusBarValues(null);
            }
        }

        void DesignSurfaceManager_UpdateValueChanged()
        {
            isValueChanged = true;
        }

        void pDesignerCore__VRulerCmValue(string value)
        {
            VRulerCmValue_toolstriplbl.Text = value;
        }

        void pDesignerCore__VRulerPixelValue(string value)
        {
            VRulerPixValue_toolstriplbl.Text = value;
        }

        void pDesignerCore__HRulerCmValue(string value)
        {
            HRulerCmValue_toolstriplbl.Text = value;
        }

        void pDesignerCore__HRulerPixelValue(string value)
        {
            HRulerPixValue_toolstriplbl.Text = value;
        }

        void DesignSurfaceManager_updateStatusBar(ReportControlProperties ctrlName)
        {
            setStatusBarValues(ctrlName);
        }

        public void OnTBClick(int TabIndex, int ItemIndex, string btnName)
        {
            if (btnName == "Save")
            {
                SaveReport();
            }
            else if (btnName == "Load")
            {
                openReport();
            }
            else if (btnName == "Preview")
            {
                List<string> currentReportImageFiles = new List<string> ();
                List<string> image = new List<string>();
                reportData = new Dictionary<string, object>();
                List<ReportControlsStructure> reportControlStructureList = pDesignerCore.GetControlProperties();

                for (int i = 0; i < reportControlStructureList.Count; i++)
                {
                    ReportControlProperties r = reportControlStructureList[i].reportControlProperty;
                    if (r.Type == "IVL_ImagePanel")
                    {
                        for (int j = 0; j < r.NumberOfImages; j++)
                        {
                            currentReportImageFiles.Add("Test1.jpg");
                            if (r.Binding.ToLower().Contains("right"))
                            {
                                image.Add("OD " + currentReportImageFiles.Count);
                            }
                            else if (r.Binding.ToLower().Contains("left"))
                                image.Add("OS " + currentReportImageFiles.Count);
                            else
                                image.Add("Image " + currentReportImageFiles.Count);
                        }
                        
                    }
                }

                reportData.Add("$CurrentImageFiles", currentReportImageFiles.ToArray());
                reportData.Add("$ImageNames", image.ToArray());
                report = new Report(reportData);
                report.reportControlStructureList = pDesignerCore.GetControlProperties();
                 ReportFileName ="report_" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss") + ".pdf";//it will save the exisiting directory as the path
                 //report.Preview(ReportFileName);
                 report.pdfGenerator.GenaratePdf(pDesignerCore.GetControlProperties(), ReportFileName);
                 Process p = new Process();
                 p.StartInfo = new ProcessStartInfo(System.IO.Path.Combine(Common.CustomFolderBrowser.filePath,Common.CustomFolderBrowser.fileName));
                 p.Start();
            }
            else
            {
                if (btnName.Equals("Image Table"))
                {
                    if (LayoutDetails.Current.Orientation == LayoutDetails.PageOrientation.LANDSCAPE_A4)
                        IVL_ImagePanel.isPortrait = false;
                    else if (LayoutDetails.Current.Orientation == LayoutDetails.PageOrientation.PORTRAIT_A4)
                        IVL_ImagePanel.isPortrait = true;
                }
                tb.SetItem(ItemIndex);
                ControlSelectedValue_toolstriplbl.Text = btnName;
            }
        }

        public void AddItemToTB(string Capt, int ImgIndex, int TabIndex, Type t, string iconPath, string toolTip)
        {
            System.Drawing.Design.ToolboxItem Itm = new ToolboxItem(t);
            //ToolBoxLib.ToolBoxItemDetails details = null;
            ToolBoxLib.ToolBoxItemDetails details = new ToolBoxItemDetails();
            details.Caption = Capt;
            details.ImageIndex = ImgIndex;
            tb.AddItem(Itm, TabIndex, details, iconPath, toolTip);
        }

        public static void CreateControlsUsingTheDesignSurface(IDesignSurfaceExt surface)
        {
            try
            {
                rootComponent = surface.GetIDesignerHost().RootComponent as Form;
            
                {
                    rootComponent.BackColor = Color.White;
                    //rootComponent.FormBorderStyle = FormBorderStyle.None;
                    rootComponent.ControlBox = false;
                    //rootComponent.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                    rootComponent.Size = new Size(A4LanscapeWidth, A4LandscapeHeight);
                    //rootComponent.Size = new Size(A4PortraitWidth, A4PortraitHeight);

                    //rootComponent.Location = new Point(32, 32);
                    rootComponent.Text = "";
                    rootComponent.Name = "Landscape_frm";
                    rootComponent.MaximizeBox = false;
                    rootComponent.MinimizeBox = false;
                    rootComponent.FormBorderStyle = FormBorderStyle.FixedSingle;
                    LayoutDetails.Current.Orientation = LayoutDetails.PageOrientation.LANDSCAPE_A4;
                    LayoutDetails.Current.PageHeight = A4LandscapeHeight;
                    LayoutDetails.Current.PageWidth = A4LanscapeWidth;
                    rootComponent.AutoSize = false;
                    //LayoutDetails.Current.Orientation = LayoutDetails.PageOrientation.PORTRAIT_A4;
                    //LayoutDetails.Current.PageHeight = A4PortraitHeight;
                    //LayoutDetails.Current.PageWidth = A4PortraitWidth;
                    IVL_ImagePanel.isPortrait = false;
                    MoveWindow(rootComponent.Handle, rootComponent.Location.X, rootComponent.Location.Y, LayoutDetails.Current.PageWidth, LayoutDetails.Current.PageHeight, false);
                }
            }//end_try
            catch (Exception ex)
            {
                Console.WriteLine(" the DesignSurface N. has generated an Exception: " + ex.Message);
            }//end_catch
        }

        private void TextBox_Btn_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.TextBox txt = new System.Windows.Forms.TextBox();
        }

        private void OnMenuClick(object sender, EventArgs e)
        {

            if (!IpDesignerCore.PropertyGridHost.ContainsFocus)
            {
                string cmd = (sender as ToolStripMenuItem).Text;
                if (cmd == "Cut")
                    IpDesignerCore.CutOnDesignSurface();
                else if (cmd == "Copy")
                    IpDesignerCore.CopyOnDesignSurface();
                else if (cmd == "Paste")
                    IpDesignerCore.PasteOnDesignSurface();
                else if (cmd == "Delete")
                    IpDesignerCore.DeleteOnDesignSurface();
            }
            //else
            //{
            //    IpDesignerCore.PropertyGridHost.ParentForm.KeyPreview = true;
            //}
        }

        private void write2Xml(string xmlFile)
        {
            TextWriter xmlWriter = new StreamWriter(xmlFile);
            ReportControlProperties reportProp = new ReportControlProperties();
            XmlSerializer xmlSer = null;
            try
            {
                xmlSer = new XmlSerializer(typeof(List<ReportControlsStructure>));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            List<ReportControlsStructure> props = pDesignerCore.GetControlProperties();

            //for (int i = 0; i < props.Count; i++)
            //{
            //ReportControlProperties IVLProps = props[i].reportControlProperty;
            //float newLocX = (float)IVLProps.Location._X * 0.75f;
            //float newLocY = (float)IVLProps.Location._Y * 0.75f;
            //float newWidth = (float)IVLProps.Size.Width * 0.75f;
            //float newHeight = (float)IVLProps.Size.Height * 0.75f;
            //IVLProps.Location._X = Convert.ToInt16(newLocX);
            //IVLProps.Location._Y = Convert.ToInt16(newLocY);
            //IVLProps.Size = new System.Drawing.Size(Convert.ToInt32(newWidth), Convert.ToInt32(newHeight));
            //props[i].reportControlProperty = IVLProps;
            //}
            xmlSer.Serialize(xmlWriter, props);
            xmlWriter.Flush();
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
            string dpiStr = "<DPI>" + LayoutDetails.Current.Dpi + "</DPI>";
            y = "<ReportTemplate>" + oriantationStr + dpiStr + y + "</ReportTemplate>";

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
            if (ori == "LANDSCAPE_A4")
            {
                refreshDesignSurface(A4LanscapeWidth, A4LandscapeHeight, "LandscapeA4_frm");
                a4ToolStripMenuItem1.Checked = false;
                portraitToolStripMenuItem.Checked = false;
                IVL_ImagePanel.isPortrait = false;

                LayoutDetails.Current.Orientation = LayoutDetails.PageOrientation.LANDSCAPE_A4;
                LayoutDetails.Current.PageHeight = A4LandscapeHeight;
                LayoutDetails.Current.PageWidth = A4LanscapeWidth;
                setStatusBarValues(null);
            }
            else if (ori == "LANDSCAPE_A5")
            {
                refreshDesignSurface(A5LanscapeWidth, A5LandscapeHeight, "LandscapeA5_frm");
                a4ToolStripMenuItem.Checked = false;
                portraitToolStripMenuItem.Checked = false;
                a5ToolStripMenuItem.Checked = true;
                a5ToolStripMenuItem1.Checked = false;
                a4ToolStripMenuItem1.Checked = false;
                IVL_ImagePanel.isPortrait = false;
                LayoutDetails.Current.Orientation = LayoutDetails.PageOrientation.LANDSCAPE_A5;
                LayoutDetails.Current.PageHeight = A5LandscapeHeight;
                LayoutDetails.Current.PageWidth = A5LanscapeWidth;
                setStatusBarValues(null);
            }
            else if (ori == "PORTRAIT_A4")
            {
                refreshDesignSurface(A4PortraitWidth, A4PortraitHeight, "PortraitA4_frm");
                a4ToolStripMenuItem.Checked = false;
                landscapeToolStripMenuItem.Checked = false;
                a5ToolStripMenuItem.Checked = false;
                a5ToolStripMenuItem1.Checked = false;
                IVL_ImagePanel.isPortrait = true;

                LayoutDetails.Current.Orientation = LayoutDetails.PageOrientation.PORTRAIT_A4;
                LayoutDetails.Current.PageHeight = A4PortraitHeight;
                LayoutDetails.Current.PageWidth = A4PortraitWidth;
                setStatusBarValues(null);
            }
            else if (ori == "PORTRAIT_A5")
            {
                refreshDesignSurface(A5PortraitWidth, A5PortraitHeight, "PortraitA5_frm");
                a4ToolStripMenuItem.Checked = false;
                landscapeToolStripMenuItem.Checked = false;
                a5ToolStripMenuItem.Checked = false;
                a4ToolStripMenuItem1.Checked = false;
                IVL_ImagePanel.isPortrait = true;

                LayoutDetails.Current.Orientation = LayoutDetails.PageOrientation.PORTRAIT_A5;
                LayoutDetails.Current.PageHeight = A5PortraitHeight;
                LayoutDetails.Current.PageWidth = A5PortraitWidth;
                setStatusBarValues(null);
            }
            //createReportLayout();
            //isReportLoading = true;
            XmlNode controlNodes = xmlDoc.FirstChild.ChildNodes[1];
            
            List<ReportControlsStructure> report = null;
            
            XmlSerializer xmlSer = new XmlSerializer(typeof(List<ReportControlsStructure>));
           string dpiStr = controlNodes.ChildNodes[0].OuterXml.ToLower();
            if (dpiStr.Contains("dpi"))
            {
                if (dpiStr.Contains("dpi_72"))
                {
                    LayoutDetails.Current.Dpi = LayoutDetails.DPI.DPI_72;
                    DPI1toolStripItem.Checked = true;
                }
                else
                {
                    LayoutDetails.Current.Dpi = LayoutDetails.DPI.DPI_96;
                    DPI2toolStripItem.Checked = true;

                }
                controlNodes = xmlDoc.FirstChild.ChildNodes[2];
            }
            else 
            {
                LayoutDetails.Current.Dpi = LayoutDetails.DPI.DPI_72;
                DPI1toolStripItem.Checked = true;
            
            }
            foreach (XmlNode item in controlNodes.ChildNodes)
            {
                MemoryStream memStearm = new MemoryStream();
                StreamWriter writer = new StreamWriter(memStearm);
                writer.Write(item.OuterXml);
                writer.Flush();
                memStearm.Position = 0;
                report = (List<ReportControlsStructure>)xmlSer.Deserialize(memStearm);
                //IVLProps = (ReportControlProperties)xmlSer.Deserialize(memStearm);
                //populateControls(IVLProps);
            }

             foreach (ReportControlsStructure item in report)
            {
                populateControls(item.reportControlProperty);
                pDesignerCore.AddToReportControlList(item);
            }
             if (LayoutDetails.Current.Dpi == LayoutDetails.DPI.DPI_72)
                 LayoutDetails.Current.Dpi = LayoutDetails.DPI.DPI_96;
            //isReportLoading = false;
            this.Cursor = Cursors.Default;
            //rootComponent.Refresh();
        }

        private void parseXmlData(string xmlFile, string type)
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
            else if (ori == "PORTRAIT")
            {
                LayoutDetails.Current.Orientation = LayoutDetails.PageOrientation.PORTRAIT;
            }
            XmlNode controlNodes = xmlDoc.FirstChild.ChildNodes[2];
            XmlSerializer xmlSer = new XmlSerializer(typeof(ControlProperties));
            if (xmlSer != null)
            {
            }
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
            this.Cursor = Cursors.Default;
        }

        public void setStatusBarValues(ReportControlProperties controlName)
        {
            float pixelValueX = 0;
            float centimeterValueX = 0;
            float inchValueX = 0;
            float pixelValueY = 0;
            float centimeterValueY = 0;
            float inchValueY = 0;
            float pixelValueWidth = 0;
            float inchValueWidth = 0;
            float centimeterValueWidth = 0;
            float pixelValueHeight = 0;
            float inchValueHeight = 0;
            float centimeterValueHeight = 0;
            string[] layoutDetails= LayoutDetails.Current.Orientation.ToString().Split('_');
            OrientationHeightValue_toolstriplbl.Text = LayoutDetails.Current.PageHeight.ToString();
            OrientationWidthValue_toolstriplbl.Text = LayoutDetails.Current.PageWidth.ToString();
            if (controlName != null)
            {
                pixelValueX = (float)controlName.Location._X;
                inchValueX = pixelValueX /(float) dpi;
                centimeterValueX = inchValueX * 2.54f;

                pixelValueY = (float)controlName.Location._Y;
                inchValueY = pixelValueY / (float)dpi;
                centimeterValueY = inchValueY * 2.54f;

                pixelValueWidth = controlName.Size.Width * 1.53f;
                inchValueWidth = pixelValueWidth / (float)dpi;
                centimeterValueWidth = inchValueWidth * 2.54f;

                pixelValueHeight = controlName.Size.Height * 1.53f;
                inchValueHeight = pixelValueHeight / (float)dpi;
                centimeterValueHeight = inchValueHeight * 2.54f;
                ControlSelectedValue_toolstriplbl.Text = controlName.Name;
                selectedObjectProperties_lbl.Text = " X (pix): " + Math.Round(pixelValueX, 2) + " X (Inch): " + Math.Round(inchValueX, 2) + " X(cm): " + Math.Round(centimeterValueX, 2) + " Y (Pix): " + Math.Round(pixelValueY, 2) + " Y (Inch): " + Math.Round(inchValueY, 2) + " Y (cm): " + Math.Round(centimeterValueY, 2) + " width (Pix): " + Math.Round(pixelValueWidth, 2) + " width(Inch): " + Math.Round(inchValueWidth, 2) + " width(cm): " + Math.Round(centimeterValueWidth, 2) + " height(Pix): " + Math.Round(pixelValueHeight, 2) + " height(Inch): " + Math.Round(inchValueHeight, 2) + " height(cm): " + Math.Round(centimeterValueHeight, 2);
                //selectedObjectProperties_lbl.Text = " X (pix): " + Math.Round(pixelValueX, 2) + " X (Inch): " + Math.Round(inchValueX, 2) + " X(cm): " + Math.Round(centimeterValueX, 2) + " Y (Pix): " + Math.Round(pixelValueY, 2) + " Y (Inch): " + Math.Round(inchValueY, 2) + " Y (cm): " + Math.Round(centimeterValueY, 2) + " width (Pix): " + Math.Round(pixelValueWidth, 2) + " (Inch): " + Math.Round(inchValueWidth, 2) + " (cm): " + Math.Round(centimeterValueWidth, 2) + " height(Pix): " + Math.Round(pixelValueHeight, 2) + " (Inch): " + Math.Round(inchValueHeight, 2) + " (cm): " + Math.Round(centimeterValueHeight, 2);
            }
        }


        private void populateControls(ReportControlProperties IVLProps)
        {
            if (LayoutDetails.Current.Dpi == LayoutDetails.DPI.DPI_72)
            {
                double width72 = 0f;
                double height72 = 0f;

                 double width96 = 0f;
                double height96 = 0f;
                if (LayoutDetails.Current.Orientation.ToString().ToLower().Contains("landscape"))
                {
                     if (LayoutDetails.Current.Orientation.ToString().ToLower().Contains("a4"))
                     {
                         width72 = a4Width * 72.0;
                         height72 = a4Height * 72.0;

                         width96 = a4Width * 96.0;
                         height96 = a4Height * 96.0;

                     }
                     else
                     {
                         width72 = a5Width * 72.0;
                         height72 = a5Height * 72.0;

                         width96 = a5Width * 96.0;
                         height96 = a5Height * 96.0;
                     }

                }
                else
                {
                    if (LayoutDetails.Current.Orientation.ToString().ToLower().Contains("a4"))
                    {
                       height72  = a4Width * 72.0;
                       width72 = a4Height * 72.0;

                      height96   = a4Width * 96.0;
                      width96 = a4Height * 96.0;

                    }
                    else
                     {
                         height72 = a5Width * 72.0;
                         width72 = a5Height * 72.0;

                         height96 = a5Width * 96.0;
                         width96 = a5Height * 96.0;
                     }

                }

                double newLocX = (double)IVLProps.Location._X * (width96/width72);
                double newLocY = (double)IVLProps.Location._Y * (height96 / height72);
                double newWidth = (double)IVLProps.Size.Width * (width96 / width72);
                double newHeight = (double)IVLProps.Size.Height * (height96 / height72);
                IVLProps.Location._X = Convert.ToInt16(newLocX);
                IVLProps.Location._Y = Convert.ToInt16(newLocY);
                IVLProps.Size = new System.Drawing.Size(Convert.ToInt32(newWidth), Convert.ToInt32(newHeight));
            }

            pDesignerCore.ActiveDesignSurface.CreateControl(IVLProps);

            //propList.Add(IVLProps);
        }


        /// <summary>
        /// This event is fired when is pressed.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
           if ((keyData == Keys.Up))
            {
                pDesignerCore.ChangeYLocationOfControls(true);
            }
            else if (keyData == Keys.Down)
            {
                pDesignerCore.ChangeYLocationOfControls(false);
            }
           else if (keyData == Keys.Right)
           {
               pDesignerCore.ChangeXLocationOfControls(true);
           }
           else if (keyData == Keys.Left)
           {
               pDesignerCore.ChangeXLocationOfControls(false);
           }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void populateControls(ControlProperties IVLProps)
        {
            {
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
            }
        }

        private void addLogo(ControlProperties IVLProps)
        {
            repoCtrlProp = new ReportControlsStructure();
            repoCtrlProp.reportControlProperty.Name = IVLProps.Name;//Place for ImageLocation
            //logo.Name = IVLProps.Name;
            //repoCtrlProp.Binding = IVLProps.Binding.ToString();
            repoCtrlProp.reportControlProperty.Location._X = (short)IVLProps.X;
            repoCtrlProp.reportControlProperty.Location._Y = (short)IVLProps.Y;
            repoCtrlProp.reportControlProperty.Size = new Size(IVLProps.Width, IVLProps.Height);
            repoCtrlProp.reportControlProperty.ImageName = IVLProps.ImageName;
            repoCtrlProp.reportControlProperty.Font.FontColor = IVLProps.ForeColor;
            repoCtrlProp.reportControlProperty.Font.FontFamily = IVLProps.FontName;
            repoCtrlProp.reportControlProperty.Font.FontSize = IVLProps.FontSize;
            GetFontStyle(IVLProps.FontStyle);
            repoCtrlProp.reportControlProperty.Font.FontStyle = font_style;
            repoCtrlProp.reportControlProperty.Border = false;
            repoCtrlProp.reportControlProperty.Type = "PictureBox";
            pDesignerCore.ActiveDesignSurface.CreateControl(repoCtrlProp.reportControlProperty);
            pDesignerCore.AddToReportControlList(repoCtrlProp);
        }

        private void addImgBox(ControlProperties IVLProps)
        {
            repoCtrlProp = new ReportControlsStructure();
            repoCtrlProp.reportControlProperty.Name = IVLProps.Name;//Place for ImageLocation
            //repoCtrlProp.Binding = IVLProps.Binding.ToString();
            repoCtrlProp.reportControlProperty.Location._X = (short)IVLProps.X;
            repoCtrlProp.reportControlProperty.Location._Y = (short)IVLProps.Y;
            repoCtrlProp.reportControlProperty.Size = new Size(IVLProps.Width, IVLProps.Height);
            repoCtrlProp.reportControlProperty.ImageName = IVLProps.ImageName;
            repoCtrlProp.reportControlProperty.Font.FontColor = IVLProps.ForeColor;
            repoCtrlProp.reportControlProperty.Font.FontColor = IVLProps.ForeColor;
            repoCtrlProp.reportControlProperty.Font.FontFamily = IVLProps.FontName;
            repoCtrlProp.reportControlProperty.Font.FontSize = IVLProps.FontSize;
            GetFontStyle(IVLProps.FontStyle);
            repoCtrlProp.reportControlProperty.Font.FontStyle = font_style;
            repoCtrlProp.reportControlProperty.Type = "IVL_ImageTable";
            repoCtrlProp.reportControlProperty.Border = true;
            pDesignerCore.ActiveDesignSurface.CreateControl(repoCtrlProp.reportControlProperty);
            pDesignerCore.AddToReportControlList(repoCtrlProp);
        }

        private void addTabelLayout(ControlProperties IVLProps)
        {
            repoCtrlProp = new ReportControlsStructure();
            repoCtrlProp.reportControlProperty.Name = IVLProps.Name;//Place for ImageLocation
            repoCtrlProp.reportControlProperty.Location._X = (short)IVLProps.X;
            repoCtrlProp.reportControlProperty.Location._Y = (short)IVLProps.Y;
            repoCtrlProp.reportControlProperty.Size = new Size(IVLProps.Width, IVLProps.Height);
            repoCtrlProp.reportControlProperty.ImageName = IVLProps.ImageName;
            repoCtrlProp.reportControlProperty.RowsColumns._Rows = (byte)IVLProps.Rows;
            repoCtrlProp.reportControlProperty.Font.FontColor = IVLProps.ForeColor;
            repoCtrlProp.reportControlProperty.RowsColumns._Columns = (byte)IVLProps.Columns;
            repoCtrlProp.reportControlProperty.Font.FontColor = IVLProps.ForeColor;
            repoCtrlProp.reportControlProperty.Font.FontFamily = IVLProps.FontName;
            repoCtrlProp.reportControlProperty.Font.FontSize = IVLProps.FontSize;
            GetFontStyle(IVLProps.FontStyle);
            repoCtrlProp.reportControlProperty.Font.FontStyle = font_style;
            repoCtrlProp.reportControlProperty.Type = "TableLayoutPanel";
            repoCtrlProp.reportControlProperty.Border = true;
            pDesignerCore.ActiveDesignSurface.CreateControl(repoCtrlProp.reportControlProperty);
            pDesignerCore.AddToReportControlList(repoCtrlProp);
        }

        private void addTextBox(ControlProperties IVLProps)
        {
            repoCtrlProp = new ReportControlsStructure();
            repoCtrlProp.reportControlProperty.Name = IVLProps.Name;//Place for ImageLocation
            repoCtrlProp.reportControlProperty.Location._X = (short)IVLProps.X;
            repoCtrlProp.reportControlProperty.Location._Y = (short)IVLProps.Y;
            repoCtrlProp.reportControlProperty.Size = new Size(IVLProps.Width, IVLProps.Height);
            repoCtrlProp.reportControlProperty.ImageName = IVLProps.ImageName;
            repoCtrlProp.reportControlProperty.Font.FontColor = IVLProps.ForeColor;
            repoCtrlProp.reportControlProperty.Font.FontFamily = IVLProps.FontName;
            repoCtrlProp.reportControlProperty.Font.FontSize = IVLProps.FontSize;
            GetFontStyle(IVLProps.FontStyle);
            repoCtrlProp.reportControlProperty.Font.FontStyle = font_style;
            repoCtrlProp.reportControlProperty.Type = "TextBox";
            repoCtrlProp.reportControlProperty.Text = IVLProps.Text;
            repoCtrlProp.reportControlProperty.Border = true;
            pDesignerCore.ActiveDesignSurface.CreateControl(repoCtrlProp.reportControlProperty);
            pDesignerCore.AddToReportControlList(repoCtrlProp);
        }

        public void GetFontStyle(string Fontstyle)
        {
            switch (Fontstyle)
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
            }
        }

        private void addLabel(ControlProperties IVLProps)
        {
            repoCtrlProp = new ReportControlsStructure();
            repoCtrlProp.reportControlProperty.Name = IVLProps.Name;
            repoCtrlProp.reportControlProperty.Text = IVLProps.Text;
            repoCtrlProp.reportControlProperty.Location._X = (short)IVLProps.X;
            repoCtrlProp.reportControlProperty.Location._Y = (short)IVLProps.Y;
            repoCtrlProp.reportControlProperty.Size = new Size(IVLProps.Width, IVLProps.Height);
            repoCtrlProp.reportControlProperty.ImageName = IVLProps.ImageName;
            repoCtrlProp.reportControlProperty.Type = "Label";
            repoCtrlProp.reportControlProperty.Font.FontColor = IVLProps.ForeColor;
            repoCtrlProp.reportControlProperty.Font.FontFamily = IVLProps.FontName;
            repoCtrlProp.reportControlProperty.Font.FontSize = IVLProps.FontSize;
            GetFontStyle(IVLProps.FontStyle);
            repoCtrlProp.reportControlProperty.Font.FontStyle = font_style;
            pDesignerCore.ActiveDesignSurface.CreateControl(repoCtrlProp.reportControlProperty);
            pDesignerCore.AddToReportControlList(repoCtrlProp);
        }

        public void SaveReport()
        {
            string dir = "ReportTemplates";
            DirectoryInfo dInf = new DirectoryInfo(dir);
            if (!Directory.Exists(dir))
                dInf = Directory.CreateDirectory(dir);
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Application.ExecutablePath + Path.DirectorySeparatorChar + "ReportTemplates"; ;
            isValueChanged = false;
            sfd.DefaultExt = ".xml";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                write2Xml(sfd.FileName);
            }
        }

        public void openReport()
        {
            getAvailableTemplates();
        }

        private void getAvailableTemplates()
        {
            string templatePath = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Application.ExecutablePath + Path.DirectorySeparatorChar + "ReportTemplates";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                templatePath = ofd.FileName;
            else
                return;
            System.IO.FileInfo finf = new FileInfo(templatePath);
            templateFile = finf.FullName;
            try
            {
                parseXmlData(templateFile);
            }
            catch (Exception ex)
            {
                parseXmlData(templateFile, "");
            }
            //templates_cbx.Items.AddRange(templateFiles);
        }

        private void addFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
          DialogResult dialogResult=  MessageBox.Show("Do you want to save the current report", "Save Report", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
          if (dialogResult == DialogResult.OK)
          {
              SaveReport();
          }
            pDesignerCore.RemoveDesignSurface(pDesignerCore.ActiveDesignSurface);
            pDesignerCore.RefreshControlList();
            pDesignerCore.AddDesignSurface<Form>(842, 595, AlignmentModeEnum.SnapLines, new Size(A4LanscapeWidth, A4LandscapeHeight));
            IDesignSurfaceExt surf = pDesignerCore.DesignSurfaceManager.ActiveDesignSurface;
            ReportTemplateCreatorWindow.CreateControlsUsingTheDesignSurface(surf);
            setStatusBarValues(null);
            a4ToolStripMenuItem.Checked = true;
        }

        public void refreshDesignSurface(int width, int height,string SurfaceName)
        {
            if (pDesignerCore.GetControlProperties().Count > 0 && isValueChanged)
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to save the current report", "Save Report", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    SaveReport();
                }
            }
            pDesignerCore.RemoveDesignSurface(pDesignerCore.ActiveDesignSurface);
            rootComponent = null;
            pDesignerCore.RefreshControlList();
            IDesignSurfaceExt surf1 = pDesignerCore.DesignSurfaceManager.ActiveDesignSurface;
            pDesignerCore.AddDesignSurface<Form>(width, height, AlignmentModeEnum.SnapLines, new Size(width, height));
            IDesignSurfaceExt surf = pDesignerCore.DesignSurfaceManager.ActiveDesignSurface;
            rootComponent = surf.GetIDesignerHost().RootComponent as Form;
            isValueChanged = false;
            rootComponent.BackColor = Color.White;
                    rootComponent.ControlBox = false;

            rootComponent.FormBorderStyle = FormBorderStyle.FixedSingle;
            //rootComponent.AutoScroll = true;
            //rootComponent.AutoSize = true;
            rootComponent.Size = new Size(width, height);
           
            rootComponent.Text = "";
            rootComponent.Name = SurfaceName;
            rootComponent.MaximizeBox = false;
            rootComponent.MinimizeBox = false;
            rootComponent.ControlBox = false;
            //rootComponent.AutoSize = true;
            MoveWindow(rootComponent.Handle, rootComponent.Location.X, rootComponent.Location.Y, width, height, false);
        }

        private void saveFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveReport();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openReport();
        }

        private void landscapeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            a4ToolStripMenuItem1.Checked = false;
            portraitToolStripMenuItem.Checked = false;
            a5ToolStripMenuItem1.Checked = false;
        }

        private void portraitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            a4ToolStripMenuItem.Checked = false;
            landscapeToolStripMenuItem.Checked = false;
            a5ToolStripMenuItem.Checked = false;
        }

       
        private void a4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshDesignSurface(A4LanscapeWidth, A4LandscapeHeight, "LandscapeA4_frm");
            a4ToolStripMenuItem1.Checked = false;
            portraitToolStripMenuItem.Checked = false;
            LayoutDetails.Current.Orientation = LayoutDetails.PageOrientation.LANDSCAPE_A4;
            LayoutDetails.Current.PageHeight = A4LandscapeHeight;
            LayoutDetails.Current.PageWidth = A4LanscapeWidth;
            IVL_ImagePanel.isPortrait = false;
            MoveWindow(rootComponent.Handle, rootComponent.Location.X, rootComponent.Location.Y, LayoutDetails.Current.PageWidth, LayoutDetails.Current.PageHeight, false);
            setStatusBarValues(null);
        }

        private void a4ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            refreshDesignSurface(A4PortraitWidth, A4PortraitHeight, "PortraitA4_frm");
            a4ToolStripMenuItem.Checked = false;
            landscapeToolStripMenuItem.Checked = false;
            a5ToolStripMenuItem.Checked = false;
            a5ToolStripMenuItem1.Checked = false;
            IVL_ImagePanel.isPortrait = true;

            LayoutDetails.Current.Orientation = LayoutDetails.PageOrientation.PORTRAIT_A4;
            LayoutDetails.Current.PageHeight = A4PortraitHeight;
            LayoutDetails.Current.PageWidth = A4PortraitWidth;
            MoveWindow(rootComponent.Handle, rootComponent.Location.X, rootComponent.Location.Y, LayoutDetails.Current.PageWidth, LayoutDetails.Current.PageHeight, false);
            setStatusBarValues(null);

        }

        private void a5ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            refreshDesignSurface(A5PortraitWidth, A5PortraitHeight, "PortraitA5_frm");
            a4ToolStripMenuItem.Checked = false;
            landscapeToolStripMenuItem.Checked = false;
            a5ToolStripMenuItem.Checked = false;
            a4ToolStripMenuItem1.Checked = false;
            IVL_ImagePanel.isPortrait = true;

            LayoutDetails.Current.Orientation = LayoutDetails.PageOrientation.PORTRAIT_A5;
            LayoutDetails.Current.PageHeight = A5PortraitHeight;
            LayoutDetails.Current.PageWidth = A5PortraitWidth;
            MoveWindow(rootComponent.Handle, rootComponent.Location.X, rootComponent.Location.Y, LayoutDetails.Current.PageWidth, LayoutDetails.Current.PageHeight, false);
            setStatusBarValues(null);
        }

        private void a5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshDesignSurface(A5LanscapeWidth, A5LandscapeHeight, "LandscapeA5_frm");
            a4ToolStripMenuItem.Checked = false;
            portraitToolStripMenuItem.Checked = false;
            a5ToolStripMenuItem.Checked = true;
            a5ToolStripMenuItem1.Checked = false;
            a4ToolStripMenuItem1.Checked = false;
            IVL_ImagePanel.isPortrait = false;
            LayoutDetails.Current.Orientation = LayoutDetails.PageOrientation.LANDSCAPE_A5;
            LayoutDetails.Current.PageHeight = A5LandscapeHeight;
            LayoutDetails.Current.PageWidth = A5LanscapeWidth;
            MoveWindow(rootComponent.Handle, rootComponent.Location.X, rootComponent.Location.Y, LayoutDetails.Current.PageWidth, LayoutDetails.Current.PageHeight, false);
            setStatusBarValues(null);
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IDesignSurfaceExt isurf = pDesignerCore.ActiveDesignSurface;
            if (null != isurf)
                isurf.GetUndoEngineExt().Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IDesignSurfaceExt isurf = pDesignerCore.ActiveDesignSurface;
            if (null != isurf)
                isurf.GetUndoEngineExt().Redo();
        }

        private void ReportTemplateCreatorWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            CustomPropertyGridHost.Serialize();
        }

        private void DPI2toolStripItem_Click(object sender, EventArgs e)
        {

        }

        private void DPI1toolStripItem_Click(object sender, EventArgs e)
        {

        }

    }

    public class Report_rectangle
    {
        public float x;
        public float y;
        public float width;
        public float height;
        public PointF center;
        public Report_rectangle()
        {

        }
        public void compute_center()
        {
            center.X = x + width / 2;
            center.Y = y + height / 2;
        }

        public void computeAspectratio(int imgWidth, int imgHeight)
        {
            if (width > height)
            {
                x = center.X - (height / 3) * 2;
                //width = 4 * (height / 3);
                float aspect = (float)((float)imgWidth / (float)imgHeight);
                width = (float)(height * aspect);

            }
            else if (height >= width)
            {
                y = center.Y - (width / 3);
                float aspect = (float)((float)imgHeight / (float)imgWidth);
                height = (float)(width * aspect);
            }
        }
    }


    public class ReportPictureBox
    {
        public ReportPictureBox()
        {

        }

        public static Point TranslateZoomMousePosition(Point coordinates, float width, float height,Image image)
        {
            // test to make sure our image is not null
            if (image == null) return coordinates;
            // Make sure our control width and height are not 0 and our 
            // image width and height are not 0
            if (width == 0 || height == 0 || image.Width == 0 || image.Height == 0) return coordinates;
            // This is the one that gets a little tricky. Essentially, need to check 
            // the aspect ratio of the image to the aspect ratio of the control
            // to determine how it is being rendered
            float imageAspect = (float)image.Width / image.Height;
            float controlAspect = (float)width / height;
            float newX = coordinates.X;
            float newY = coordinates.Y;
            if (imageAspect > controlAspect)
            {
                // This means that we are limited by width, 
                // meaning the image fills up the entire control from left to right
                float ratioWidth = (float)image.Width / width;
                newX *= ratioWidth;
                float scale = (float)width / image.Width;
                float displayHeight = scale * image.Height;
                float diffHeight = height - displayHeight;
                diffHeight /= 2;
                newY -= diffHeight;
                newY /= scale;
            }
            else
            {
                // This means that we are limited by height, 
                // meaning the image fills up the entire control from top to bottom
                float ratioHeight = (float)image.Height / height;
                newY *= ratioHeight;
                float scale = (float)height / image.Height;
                float displayWidth = scale * image.Width;
                float diffWidth = width - displayWidth;
                diffWidth /= 2;
                newX -= diffWidth;
                newX /= scale;
            }
            return new Point((int)newX, (int)newY);
        }
    }
}
