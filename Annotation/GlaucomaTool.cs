using Common;
using Emgu.CV;
using Emgu.CV.Structure;
using INTUSOFT.Custom.Controls;
using INTUSOFT.EventHandler;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Annotation
{
    public partial class GlaucomaTool : BaseGradientForm
    {
        private string AnnotationImagefileName = "";
        public enum Channels { Color, Red, Green, Blue };
        Channels CurrentChannelDisplayed;
        GraphicsPath gp;
        int disc_count = 0;
        int cup_count = 0;
        string annotationtype = "";
        string areaCup = "";
        string areaDisc = "";
        string areaRim = "";
        IVLReport.Report _report = null;
        string VerticalDiscLen = "";
        string horizontalDiscLen = "";
        string DiscDeletionWarningMessage = "";
        string DiscDeletionWarningHeader = "";
        string verticalCupLen = "";
        string horizontalCupLen = "";
        string verticalCDR = "";
        string horizontalCDR = "";
        string millimeterText = "";
        string superior = "";
        string inferior = "";
        bool isGraphicsListPresent = false;
        bool isSave = false;
        string temporal = "";
        string nasal = "";
        string equalText = "";
        string nameOfTheReport = "";
        int x = 0, y = 0;
        string cdrxml = "";
        string comments = "";
        string defaultComment = "";
        string reportedBy = string.Empty;
        string saveConfirmation = "";
        string saveWarningText = string.Empty;
        string saveWarningHeader = string.Empty;
        string[] headers;
        bool is24hourformat;
        bool isFromSvg = true;
        Dictionary<string, object> glaucomaToolValues = null;
        //This below bool variable was added by Darshan on 21-08-2015 to solve Defect no 0000591: when deleted the saved annotation,the current annotation performing graph is getting deleted.
        bool isview = false;
        Bitmap OD_Isnt = new Bitmap(10, 10);
        Bitmap OS_Isnt = new Bitmap(10, 10);
        AnnotationComments ac;
        int comment_Id = 0;
        int Annotation_ImageId = 0;
        DrawArea drawArea1;
        bool isControl = false;
        public delegate void AnnotationSavedDelegate(Args e);
        public event AnnotationSavedDelegate annotationSavedEvent;
        Image<Gray, byte> redChannel, greenChannel, blueChannel;
        Image<Bgr, byte> colorImage;
        bool isOD = false;
        double k = 0.0064;
        double sqK = 0.00004096;
        Image redFilterSelected, greenFilterSelected, blueFilterSelected;
        Image redFilter, greenFilter, blueFilter;
        public bool isPrint = false;
        CustomFolderBrowser customFolderBrowser;//variable of type CustomFolderBrowser.By Ashutosh 21/7
        public IVLEventHandler _eventHandler;
        #region Picturebox width and height are saved to static variables so that they can used during translation of the screen points to actual image points and also the image displayed on the picturebox is made public and static so that it is available for further analysis by sriram on august 31st 2015
        public static int pbxWidth = 0;
        public static int pbxHeight = 0;
        public static Bitmap tempBm = null;
        #endregion
        public GlaucomaTool(Dictionary<string, object> UITextValues)//changed dictionary value type from string to object . By Ashutosh 18-08-2017
        {

            InitializeComponent();
            this.CDRTool_ts.Renderer = new FormToolStripRenderer();
            this.toolStrip2.Renderer = new FormToolStripRenderer();
            glaucomaToolValues = new Dictionary<string, object>();// changed dictionary value type from string to object . By Ashutosh 18-08-2017
            glaucomaToolValues = UITextValues;
            AnnotationVariables.isGlaucomaTool = true;
            string[] ImageSides = UITextValues["$visitImageSides"] as string[];
            if (ImageSides[0] == "0")
                isOD = true;
            else
                isOD = false;
            // changed dictionary value type from string to object . By Ashutosh 18-08-2017
            MovePoints_btn.Text = UITextValues["$EditDisc"] as string;
            DrawDisc_Btn.ToolTipText = UITextValues["$DrawDiscButtonToolTipText"] as string;
            MovePoints_btn.ToolTipText = UITextValues["$DrawDiscPointsButtonToolTipText"] as string;
            drawCup_btn.ToolTipText = UITextValues["$DrawCupButtonToolTipText"] as string;
            CalculateCDR_btn.ToolTipText = UITextValues["$CalculateCDRButtonToolTipText"] as string;
            saveDrawing_btn.ToolTipText = UITextValues["$SaveCDRButtonToolTipText"] as string;
            Print_btn.ToolTipText = UITextValues["$PrintCDRButtonToolTipText"] as string;
            Export_btn.ToolTipText = UITextValues["$ExportCDRButtonToolTipText"] as string;
            this.Color1 = (Color)UITextValues["$Color1"];
            this.Color2 = (Color)UITextValues["$Color2"];
            this.FontColor = (Color)UITextValues["$FontColor"];
            this.ColorAngle = (float)UITextValues["$ColorAngle"];
            
            this.Text = UITextValues["$FormText"] as string;
            //drawTools_lbl.Text = UITextValues["$DrawTools"] as string;
            drawTools_gbx.Text = UITextValues["$DrawTools"] as string;
            //drawCup_lbl.Text = UITextValues["$DrawCup"] as string;
            drawCup_btn.Text = UITextValues["$DrawCup"] as string;
            DrawDisc_Btn.Text = UITextValues["$DrawDisc"] as string;
            //drawDisc_lbl.Text = UITextValues["$DrawDisc"] as string;
            //saveDrawing_btn.Text = UITextValues["$saveBtnText"] as string;
            //save_lbl.Text = UITextValues["$saveBtnText"] as string;
            Print_btn.Text = UITextValues["$printBtnText"] as string;
            saveDrawing_btn.Text = UITextValues["$saveBtnText"] as string;
            //print_lbl.Text = UITextValues["$printBtnText"] as string;
            fileTools_gbx.Text = UITextValues["$FileToolsGbxText"] as string;
            filter_lbl.Text = UITextValues["$filters"] as string;
            //redChannel_btn.Text = UITextValues["$redFilter"] as string;
            //greenChannel_btn.Text = UITextValues["GreenFilter_Text"] as string;
            //blueChannel_btn.Text = UITextValues["$blueFilter"] as string;
            //colorImage_btn.Text = UITextValues["$colorFilter"] as string;
            saveWarningText = UITextValues["$saveWarningText"] as string;
            saveWarningHeader = UITextValues["$saveWarningHeader"] as string;
            measurementHeader_lbl.Text = UITextValues["$measurements"] as string;
            Export_btn.Text = UITextValues["$ExportButtonText"] as string;
            redChannel_btn.Text = UITextValues["$redFilterText"] as string;
            greenChannel_btn.Text = UITextValues["$greenFilterText"] as string;
            blueChannel_btn.Text = UITextValues["$blueFilterText"] as string;
            //greenChannel_btn.Text
            //blueChannel_btn.Text 
            
            AnnotationVariables.addCupPointsWarning = UITextValues["$addCupPointsWarning"] as string;
            AnnotationVariables.addDiscPointsWarning = UITextValues["$addDiscPointsWarning"] as string;
            AnnotationVariables.warningHeader = UITextValues["$warningHeader"] as string;
            AnnotationVariables.annotationMarkingColor = UITextValues["$annotationMarkingColor"] as string;
            AnnotationVariables.cupColor = UITextValues["$cupColor"] as string;
            AnnotationVariables.discColor = UITextValues["$discColor"] as string;
            areaCup = UITextValues["$areaCup"] as string;
            areaCup_lbl.Text = areaCup;
            areaDisc = UITextValues["$areaDisc"] as string;
            areaDisc_lbl.Text = areaDisc;
            areaRim = UITextValues["$areaRim"] as string;
            areaRim_lbl.Text = areaRim;
            verticalCDR = UITextValues["$verticalCDR"] as string;
            VerticalCDR_lbl.Text = verticalCDR;
            horizontalCDR = UITextValues["$horizontalCDR"] as string;
            HorizontalCDR_lbl.Text = horizontalCDR;
            verticalCupLen = UITextValues["$verticalCupLen"] as string;
            VerticalLineCup_lbl.Text = verticalCupLen;
            horizontalCupLen = UITextValues["$horizontalCupLen"] as string;
            HorizontalLineCup_lbl.Text = horizontalCupLen;
            VerticalDiscLen = UITextValues["$VerticalDiscLen"] as string;
            VerticalLineDisc_lbl.Text = VerticalDiscLen;
            comments_tbx.Text = UITextValues["$Comments"] as string;
            DiscDeletionWarningMessage = UITextValues["$deleteDiscText"] as string;
            DiscDeletionWarningHeader = UITextValues["$deleteDiscHeader"] as string;
            defaultComment = comments_tbx.Text;
            horizontalDiscLen = UITextValues["$horizontalDiscLen"] as string;
            HorizontalLineDisc_lbl.Text = horizontalDiscLen;
            superior = UITextValues["$superior"] as string;
            inferior = UITextValues["$inferior"] as string;
            nasal = UITextValues["$nasal"] as string;
            //CalculateCDR_btn.Text = UITextValues["$calulateCDRText"] as string;
            CalculateCDR_btn.ToolTipText = UITextValues["$calulateCDRText"] as string;
            temporal = UITextValues["$temporal"] as string;
            superior_lbl.Text = superior;
            inferior_lbl.Text = inferior;
            reportedBy = UITextValues["$repotedBy"] as string;//assigns Reported By text to Reportedby_lbl.
            if (UITextValues.ContainsKey("$Doctor"))
            {
                if (!string.IsNullOrEmpty(UITextValues["$Doctor"] as string))
                {
                    Reportename_txt.Text = UITextValues["$Doctor"].ToString();//assigns the Reporter name to Reportname_txt.
                    Reportename_txt.ForeColor = Color.Black;
                    Font f = Reportename_txt.Font;
                    Font newFont = new Font(f.FontFamily, f.Size, FontStyle.Regular, GraphicsUnit.Point);
                    Reportename_txt.Font = newFont;
                }
                else
                {
                    Reportename_txt.Text = reportedBy;//assigns sets default name to Reportename_txt.
                    Font f = Reportename_txt.Font;
                    Font newFont = new Font(f.FontFamily, f.Size, FontStyle.Italic, GraphicsUnit.Point);
                    Reportename_txt.Font = newFont;
                }
            }
            //MovePoints_btn.Text = UITextValues["$EditDiscPoints"] as string;
            //modifyDiscPoints_lbl.Text = UITextValues["$EditDiscPoints"] as string;
            saveConfirmation = UITextValues["$saveConfirmation"] as string;
            if (isOD)
            {
                temporal_lbl.Text = temporal;
                nasal_lbl.Text = nasal;
            }
            else
            {
                temporal_lbl.Text = nasal;
                nasal_lbl.Text = temporal;
            }
            millimeterText = UITextValues["$millimeterText"] as string;
            equalText = UITextValues["$EqualText"] as string;

            //headers = h;
            is24hourformat = (bool)UITextValues["$is24hrformat"] ;

            string redFilterLogoPath = @"ImageResources\FilterImageResources\Red.png";
            string redToColorFilterLogoPath = @"ImageResources\FilterImageResources\RedNegative.png";
            string greenFilterLogoPath = @"ImageResources\FilterImageResources\Green.png";
            string greenToColorFilterLogoPath = @"ImageResources\FilterImageResources\GreenNegative.png";
            string blueFilterLogoPath = @"ImageResources\FilterImageResources\Blue.png";
            string blueToColorFilterLogoPath = @"ImageResources\FilterImageResources\BlueNegative.png";
            string printToolLogoPath = @"ImageResources\Edit_ImageResources\PrintIcon.png";
            string saveToolLogoPath = @"ImageResources\Edit_ImageResources\SaveIcon.png";
            string discToolLogoPath = @"ImageResources\CDR_ImageResources\AddDiscPoints_3D.png"; ;
            string cupToolLogoPath = @"ImageResources\CDR_ImageResources\AddCupPoints_3D.png"; ;
            string movePointsLogoPath = @"ImageResources\CDR_ImageResources\Move.png"; ;
            string cdrToolLogoPath = @"ImageResources\CDR_ImageResources\cdr.jpg";
            string ISNT_OD_ToolLogoPath = @"ImageResources\CDR_ImageResources\ISNT_OD.jpg";
            string ISNT_OS_ToolLogoPath = @"ImageResources\CDR_ImageResources\ISNT_OS.jpg";
            string appLogoPath = @"ImageResources\LogoImageResources\IntuSoft.ico";
            string exportToolLogoPath = @"ImageResources\Edit_ImageResources\Export_Image_Square.png";//path of image initialised to string. By Ashutosh 20-7-2017

            if (File.Exists(discToolLogoPath))
                DrawDisc_Btn.Image = Image.FromFile(discToolLogoPath);
            if (File.Exists(cupToolLogoPath))
                drawCup_btn.Image = Image.FromFile(cupToolLogoPath);
            if (File.Exists(movePointsLogoPath))
                MovePoints_btn.Image = Image.FromFile(movePointsLogoPath);
            //Open_Btn.Image = Image.FromFile(@"ImageResources\open.bmp");
            if (File.Exists(saveToolLogoPath))
                saveDrawing_btn.Image = Image.FromFile(saveToolLogoPath);
            if (File.Exists(exportToolLogoPath))// checks if file contains exportToolLogoPath string .
                Export_btn.Image = Image.FromFile(exportToolLogoPath);// if present path given to buttons image property.By Ashutosh 20-7-2017
            if (File.Exists(redFilterLogoPath))
                redChannel_btn.Image = redFilter = Image.FromFile(redFilterLogoPath);
            if (File.Exists(greenFilterLogoPath))
                greenChannel_btn.Image = greenFilter = Image.FromFile(greenFilterLogoPath);
            if (File.Exists(blueFilterLogoPath))
                blueChannel_btn.Image = blueFilter = Image.FromFile(blueFilterLogoPath);
            if (File.Exists(cdrToolLogoPath))
                CalculateCDR_btn.Image = Image.FromFile(cdrToolLogoPath);
            //gp = new GraphicsPath();
            //gp.AddEllipse(0, 0, 50, 50);
            //isnt_pbx.Region = new Region(gp);

            if (File.Exists(redToColorFilterLogoPath))
                redFilterSelected = Image.FromFile(redToColorFilterLogoPath); //Red filter selected image;
            if (File.Exists(greenToColorFilterLogoPath))
                greenFilterSelected = Image.FromFile(greenToColorFilterLogoPath); //Green filter selected Image;
            if (File.Exists(blueToColorFilterLogoPath))
                blueFilterSelected = Image.FromFile(blueToColorFilterLogoPath); //Blue filter selected Image;

            if (File.Exists(ISNT_OD_ToolLogoPath))
                OD_Isnt = Image.FromFile(ISNT_OD_ToolLogoPath) as Bitmap;
            if (File.Exists(ISNT_OS_ToolLogoPath))
                OS_Isnt = Image.FromFile(ISNT_OS_ToolLogoPath) as Bitmap;
            if (isOD)
            {
                isnt_pbx.Image = OD_Isnt;
                isnt_pbx.Tag = ISNT_OD_ToolLogoPath;
            }
            else
            {
                isnt_pbx.Image = OS_Isnt;
                isnt_pbx.Tag = ISNT_OS_ToolLogoPath;
            }
            {
                if (File.Exists(printToolLogoPath))
                    Print_btn.Image = Image.FromFile(printToolLogoPath);
            }
            this.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.MaximumSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.MinimumSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            drawArea1 = new DrawArea();
            drawArea1._commentsAddedEvent += drawArea1__commentsAddedEvent;
            drawArea1._currentId += drawArea1__currentId;
            drawArea1._unselectall += drawArea1__unselectall;
            ac = new AnnotationComments();
            Annotation_ImageId = (glaucomaToolValues["$visitImageIds"] as int[])[0];
            drawArea1.Parent = Display_pbx;
            drawArea1.ShowComments = false;
            drawArea1.BackColor = Color.Transparent;
            AnnotationImagefileName = (glaucomaToolValues["$CurrentImageFiles"] as string[])[0];
            Common.CommonMethods c = Common.CommonMethods.CreateInstance();
            Bitmap bm = new Bitmap(10, 10);
            // initialization of the static public image to be used throught out the application by sriram on august 31st 2015
            tempBm = new Bitmap(10, 10);
            LoadSaveImage.LoadImage(AnnotationImagefileName, ref bm);
            colorImage = new Image<Bgr, byte>(bm);
            redChannel = colorImage[2].Copy();
            greenChannel = colorImage[1].Copy();
            blueChannel = colorImage[0].Copy();
            List<Control> controls = GetControls(this).ToList();
            
            foreach (Control ctrls in controls)
            {
                {
                    {
                        if (Screen.PrimaryScreen.Bounds.Width == 1920)
                        {
                            ctrls.Font = new Font(ctrls.Font.FontFamily.Name, 13f);
                            filter_lbl.Font = new Font(ctrls.Font.FontFamily.Name, 13f, FontStyle.Bold);
                            //drawTools_lbl.Font = new Font(ctrls.Font.FontFamily.Name, 13f, FontStyle.Bold);
                            measurementHeader_lbl.Font = new Font(ctrls.Font.FontFamily.Name, 13f, FontStyle.Bold);
                        }
                        else if (Screen.PrimaryScreen.Bounds.Width == 1366)
                        {
                            ctrls.Font = new Font(ctrls.Font.FontFamily.Name, 10f);
                            filter_lbl.Font = new Font(ctrls.Font.FontFamily.Name, 10f, FontStyle.Bold);
                            //drawTools_lbl.Font = new Font(ctrls.Font.FontFamily.Name, 10f, FontStyle.Bold);
                            measurementHeader_lbl.Font = new Font(ctrls.Font.FontFamily.Name, 10f, FontStyle.Bold);
                        }
                        else
                            ctrls.Font = new Font(ctrls.Font.FontFamily.Name, 9f);
                        if (ctrls is TextBox)
                            ctrls.ForeColor = Color.Black;
                    }
                }
            }
            this.fileTools_tsp.Renderer = new FormToolStripRenderer();

            // load the image from the file by sriram on august 31st 2015
            LoadSaveImage.LoadImage(AnnotationImagefileName, ref tempBm);
            if (File.Exists(appLogoPath))
                this.Icon = new System.Drawing.Icon(appLogoPath, 256, 256);
            this.cdrxml = glaucomaToolValues["$xml"]as string;
            if (_eventHandler == null)
            {
                _eventHandler = IVLEventHandler.getInstance();
                _eventHandler.Register(_eventHandler.UpdateGlaucomaToolControls, new NotificationHandler(updateGlaucomaToolControls));
                _eventHandler.Register(_eventHandler.EnableMovePointInCDRTool, new NotificationHandler(enableMovePoints));//Register(String n, NotificationHandler handler) defined in IVLEventHandler.By ashutosh 25-07-2017
            }
            Display_pbx.Image = bm;
            this.Load += Form1_Load;
            //this.TopMost = true;//has been commented to prevent the custom message box getting background.
        }

        public static IEnumerable<Control> GetControls(Control form)
        {
            foreach (Control childControl in form.Controls)
            {   // Recurse child controls.
                foreach (Control grandChild in GetControls(childControl))
                {
                    yield return grandChild;
                }
                yield return childControl;
            }
        }

        //Measurements_default_values() method has been added by Darshan on 22-09-2015 to set the measurement values to default values when either disc or cup graph is deleted.
        public void Measurements_default_values()
        {
            string default_value = "0";
            areaCupVal_lbl.Text = default_value;
            areaDiscVal_lbl.Text = default_value;
            areaRimVal_lbl.Text = default_value;
            verticalLineDiscVal_lbl.Text = default_value;
            verticalLineCupVal_lbl.Text = default_value;
            horizontalLineCupVal_lbl.Text = default_value;
            horizontalLineDiscVal_lbl.Text = default_value;
            verticalCDRVal_lbl.Text = default_value;
            horizontalCDRVal_lbl.Text = default_value;
            superior_lbl.Text = default_value;
            inferior_lbl.Text = default_value;
            if (isOD)
            {
                temporal_lbl.Text = default_value;
                nasal_lbl.Text = default_value;
            }
            else
            {
                temporal_lbl.Text = default_value;
                nasal_lbl.Text = default_value;
            }
        }

        void drawArea1__unselectall()
        {
        }

        void userControl11__annotationactiveid(int id)
        {
            drawArea1.GraphSelect(id);
            drawArea1.Refresh();
        }

        void drawArea1__currentId(int id)
        {
            comment_Id = id;
        }

        AnnotationText temp;
        void drawArea1__commentsAddedEvent(AnnotationText c, EventArgs e)
        {
            temp = c;
            comment_Id = c.ID;
            //the below code has been added by Darshan to solve defect no 0000510: Duplicate numbering in comments if pressed on control key.
        }

        public void Refresh_DrawArea()
        {
            drawArea1.Parent = Display_pbx;
            Common.CommonMethods c = Common.CommonMethods.CreateInstance();
            Bitmap bm = new Bitmap(10, 10);
            tempBm = new Bitmap(10, 10);
            LoadSaveImage.LoadImage(AnnotationImagefileName, ref bm);
            LoadSaveImage.LoadImage(AnnotationImagefileName, ref tempBm);
            Display_pbx.Image = bm;
            drawArea1.Refresh();
            ResizeDrawArea();
            drawArea1.Initialize(this);
        }
        private void enableMovePoints(string s, Args arg)//
        {
            HighlightSelectedButton(MovePoints_btn, CDRTool_ts);
        }
        public void SetFileName(string fileName)
        {
        }

        void Form1_Load(object sender, EventArgs e)
        {
            ResizeDrawArea();
            drawArea1.Initialize(this);
            pbxHeight = Display_pbx.Height;
            pbxWidth = Display_pbx.Width;
            if (!string.IsNullOrEmpty(cdrxml))
            {
                this.Cursor = Cursors.WaitCursor;
                
                Args arg = new Args();
                arg["ModifyDiscPoints"] = true;
                arg["DrawCup"] = true;
                arg["ModifyCupPoints"] = true;
                arg["MeasureCDR"] = true;
                arg["Print"] = true;
                arg["Save"] = false;
                arg["Export"] = true;// set to true .By Ashutosh 20-7-2017
                arg["IsViewed"] = true;
                _eventHandler.Notify(_eventHandler.UpdateGlaucomaToolControls, arg);
                this.Cursor = Cursors.Default;
                AnnotationVariables.isGlaucomaToolViewing = true;
                //if (this.InvokeRequired)
                //{
                //    this.Invoke(m_DelegateSetLiveOrView, sender, e);
                //}
                viewannotation(cdrxml);
                CDR_measurements();
            }
            //this.toolStrip2.Focus();
        }

        private void Open_Btn_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            string fileName = "";
            ofd.Filter = "XML Files|.xml";
            if (ofd.ShowDialog() == DialogResult.OK)
                fileName = ofd.FileName;
            else
                return;
        }
        /// <summary>
        /// /draws disc 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawDisc_Btn_Click(object sender, EventArgs e)
        {
            if (!isControl)
            {
                DrawArea.isDrawCup = false;
                HighlightSelectedButton(DrawDisc_Btn, this.CDRTool_ts);
                CommandPolygon();
                if (ToolPolygon.DrawDiscPolygon != null)
                {
                    for (int i = 0; i < drawArea1.graphicsList.Count; i++)
                    {
                        DrawPolygon polyGon = drawArea1.graphicsList[i] as DrawPolygon;
                        if (!polyGon.isCup)
                        {
                            drawArea1.graphicsList[i].Selected = true;
                        }
                        else
                            drawArea1.graphicsList[i].Selected = false;
                    }
                }
                drawArea1.Refresh();
                int CursorPos = comments_tbx.SelectionStart;
                if (comments_tbx.Text != defaultComment)
                    CDRTool_ts.Focus();
                //disc_count++;
            }
        }

        /// <summary>
        /// Set draw area to all form client space except toolbar
        /// </summary>
        private void ResizeDrawArea()
        {
            Rectangle rect = this.ClientRectangle;
            drawArea1.Left = rect.Left;
            drawArea1.Top = rect.Top;// +toolBar1.Height;
            drawArea1.Width = rect.Width;
            drawArea1.Height = rect.Height;//- toolBar1.Height;
        }

        /// <summary>
        /// Set Pointer draw tool
        /// </summary>
        private void CommandPointer()
        {
            drawArea1.ActiveTool = DrawArea.DrawToolType.Pointer;
        }

        /// <summary>
        /// Set Rectangle draw tool
        /// </summary>
        private void CommandRectangle()
        {
            drawArea1.ActiveTool = DrawArea.DrawToolType.Rectangle;
        }

        /// <summary>
        /// Set Ellipse draw tool
        /// </summary>
        private void CommandEllipse()
        {
            drawArea1.ActiveTool = DrawArea.DrawToolType.Ellipse;
        }

        /// <summary>
        /// Set Line draw tool
        /// </summary>
        private void CommandLine()
        {
            drawArea1.ActiveTool = DrawArea.DrawToolType.Line;
        }

        /// <summary>
        /// Set Polygon draw tool
        /// </summary>
        private void CommandPolygon()
        {
            drawArea1.ActiveTool = DrawArea.DrawToolType.Polygon;
        }

        private void drawArea1_Load(object sender, EventArgs e)
        {
        }

        private void LineDraw_btn_Click(object sender, EventArgs e)
        {
            if (!isControl)
                CommandLine();
        }

        public void SaveAnnotation(Args e)
        {
            if (drawArea1.iscup_drawn && drawArea1.graphicsList.Count == 2)
            {
                if (saveDrawing_btn.Enabled)
                {
                    SvgDocument svgDocument = new SvgDocument();
                    SvgDescription reportedByDescription = new SvgDescription();
                    SvgDescription commentsDescription = new SvgDescription();
                    isSave = true;
                    String AnnotationXml = "";
                    string Comments = "";
                    annotationtype = drawArea1.ActiveTool.ToString();
                    {
                        {
                            {
                                AnnotationXMLProperties annoprop = new AnnotationXMLProperties();
                                {
                                    List<bool> id = new List<bool>();
                                    //this below if condition has been added by Darshan to save the ReportedBy.
                                    if (string.IsNullOrEmpty(Reportename_txt.Text))
                                    {
                                        //annoprop.ReportedBy = string.Empty;
                                        reportedByDescription.ID = "Reported By";
                                        reportedByDescription.Content = string.Empty;
                                    }
                                    else
                                    {
                                        //annoprop.ReportedBy = Reportename_txt.Text;
                                        reportedByDescription.ID = "Reported By";
                                        reportedByDescription.Content = Reportename_txt.Text;
                                    }
                                    //annoprop.XmlGraphicsList = drawArea1.ListGraphics;
                                    for (int i = 0; i < drawArea1.graphicsList.Count; i++)
                                    {
                                        DrawObject dObject = drawArea1.graphicsList[i];
                                        annoprop.Shapes.Add(dObject.Shape);
                                        if (drawArea1.graphicsList[i] is DrawPolygon)//.graphicsList[i]
                                        {
                                            DrawPolygon d = drawArea1.graphicsList[i] as DrawPolygon;
                                            annoprop.isCupProperties.Add(d.isCup);
                                        }
                                    }
                                    svgDocument.Children.Add(reportedByDescription);
                                    for (int i = annoprop.Shapes.Count - 1; i >= 0; --i)
                                    {
                                        if (annoprop.Shapes[i]._shapeType.ToString() == "Annotation_Polygon")
                                        {
                                            SvgPolygon polygon = new SvgPolygon();
                                            SvgPointCollection pointCollection = new SvgPointCollection();
                                            foreach (Point item in annoprop.Shapes[i].PointArray)
                                            {
                                                pointCollection.Add(item.X);
                                                pointCollection.Add(item.Y);
                                            }
                                            if (annoprop.isCupProperties.Contains(true))
                                            {
                                                if (annoprop.isCupProperties[i])
                                                    polygon.ID = "cup";
                                                else
                                                    polygon.ID = "disc";
                                            }
                                            polygon.Points = pointCollection;
                                            //desc.ID = annoprop.Xmlcommentsproperties[i].ID.ToString();
                                            svgDocument.Children.Add(polygon);
                                        }
                                    }
                                    commentsDescription.ID = "CDRComments";// this was present inside if . since ID is necesaary when comments are present and absent put outside if.By Ashutosh 11-08-2017
                                    if (comments_tbx.Text != defaultComment && !(string.IsNullOrEmpty(comments_tbx.Text)))
                                    {
                                        //annoprop.CDRComments = comments_tbx.Text;
                                        commentsDescription.Content = comments_tbx.Text;
                                    }
                                    else
                                    {
                                        commentsDescription.Content = string.Empty;
                                    }
                                    svgDocument.Children.Add(commentsDescription);
                                    AnnotationXml = svgDocument.GetXML();
                                    XmlDocument doc = new XmlDocument();
                                    doc.LoadXml(Regex.Replace(AnnotationXml, "<!DOCTYPE.+?>", string.Empty));
                                    AnnotationXml = doc.InnerXml;
                                    doc.LoadXml(Regex.Replace(AnnotationXml, "<svg.+?>", "<svg>"));
                                    AnnotationXml = doc.InnerXml;
                                }
                            }
                        }
                        Dictionary<string, object> annoval = new Dictionary<string, object>();
                        e["xml"] = AnnotationXml;
                        e["dateTime"] = DateTime.Now;
                        e["Comments"] = Comments;
                        e["imageid"] = Annotation_ImageId;
                        e["annotationID"] = Annotation_ImageId;
                        if (annotationSavedEvent != null)
                            this.Cursor = Cursors.WaitCursor;
                        annotationSavedEvent(e);
                    }
                }
            }
            else
            {
                CustomMessageBox.Show(saveWarningText, saveWarningHeader, CustomMessageBoxButtons.OK, CustomMessageBoxIcon.Information);
            }
        }

        private void saveDrawing_btn_Click(object sender, EventArgs e)
        {
            HighlightSelectedButton(saveDrawing_btn, this.CDRTool_ts);
            Args arg = new Args();
            arg["isAnnotationUpdate"] = false;
            SaveAnnotation(arg);
            //drawArea1.ListGraphics.Clear();
            //drawArea1.Refresh();
            refreshCDRScreen();
            this.Cursor = Cursors.Default;
        }

        public void refreshCDRScreen()
        {
            Args arg = new Args();
            arg["ModifyDiscPoints"] = false;
            arg["DrawCup"] = false;
            arg["ModifyCupPoints"] = false;
            arg["MeasureCDR"] = false;
            arg["Print"] = false;
            arg["Save"] = false;
            arg["Export"] = false;// set to false when screen refreshed . method unused as of now 20/7
            _eventHandler.Notify(_eventHandler.UpdateGlaucomaToolControls, arg);
            //Measurements_default_values();//this has been commented to displays the CDR values  since the image and CDR markings and comments are showing 
        }

        public void DrawComments(AnnotationComments annotationcomments)
        {
            Panel annotationPanel = new Panel();
            AnnotationText c = new AnnotationText();
            c.Size = new Size(229, 194);
            annotationPanel.Size = new Size(229, 194);
            c.Dock = DockStyle.Fill;
            //  c.ac.Comments = annotationcomments.Comments;
            // c.ac.Header = annotationcomments.Header;
            //c.SetBounds(c.Location.X, c.Location.Y, c.Width, c.Height);
            // annotationPanel.Controls.Add(c);
            //drawArea1.Controls.Add(annotationPanel);
            // drawArea1.collapsablePanels.Add(c);
            //c.Comments_visible();
            annotationPanel.Location = new Point(annotationcomments.X, annotationcomments.Y);
            annotationPanel.Show();
            //c.Show();
        }

        public void viewannotation(string AnnotationXml)
        {
            byte[] bytes = null;
            AnnotationXMLProperties anno = null;
            //Below code in the try block has been retained to handle the situation when the data is getting readed from old annotationxml(binary format).
            try
            {
                bytes = Convert.FromBase64String(AnnotationXml);
                using (MemoryStream stream = new MemoryStream(bytes))
                {
                    IFormatter formatter = new BinaryFormatter();
                    anno = (AnnotationXMLProperties)formatter.Deserialize(stream);
                }
                isGraphicsListPresent = true;// Bool used for saving the older graphics to new method
            }
            catch (Exception)
            {
                try
                {
                    using (StringReader sr = new StringReader(AnnotationXml))
                    {
                        XmlReaderSettings settings = new XmlReaderSettings();
                        settings.ConformanceLevel = ConformanceLevel.Auto;
                        using (XmlReader xmlReader = XmlReader.Create(sr, settings))
                        {
                            {
                                XmlSerializer xmlSer = new XmlSerializer(typeof(AnnotationXMLProperties));
                                anno = xmlSer.Deserialize(xmlReader) as AnnotationXMLProperties;
                            }
                            isGraphicsListPresent = false;// Bool used for saving the older graphics to new method
                            isFromSvg = false;
                        }
                    }
                }
                catch (Exception)
                {
                    SvgDocument svgDoc = SvgDocument.FromSvg<SvgDocument>(AnnotationXml);
                    SvgElementCollection svg = svgDoc.Children;
                    anno = new AnnotationXMLProperties();
                    for (int i = 0; i < svg.Count; i++)
                    {
                        Shape s = null;

                        if (svg[i] is Svg.SvgPolygon)
                        {
                            SvgPolygon svgPolygon = svg[i] as SvgPolygon;
                            s = new Shape();
                            s._shapeType = ShapeType.Annotation_Polygon;
                            if (svgPolygon.ID == "cup")
                            {
                                anno.isCupProperties.Add(true);
                                drawArea1.iscup_drawn = true;
                            }
                            else
                            {
                                anno.isCupProperties.Add(false);
                            }
                            SvgPointCollection points = svgPolygon.Points;
                            for (int j = 0; j < points.Count; j++)
                            {
                                Point p = new Point(Convert.ToInt32(points[j]), Convert.ToInt32(points[j + 1]));
                                j = j + 1;
                                s.PointArray.Add(p);

                            }
                            anno.Shapes.Add(s);

                        }
                        else if (svg[i] is Svg.SvgDescription)
                        {
                            SvgDescription description = svg[i] as SvgDescription;
                            if (description.ID == "Reported By")
                                anno.ReportedBy = description.Content;
                            else if (description.ID == "CDRComments")
                                anno.CDRComments = description.Content;
                        }
                    }
                    anno.Shapes.Reverse();
                    anno.isCupProperties.Reverse();
                    isFromSvg = true;
                }
            }
            {
                //drawArea1.ListGraphics = anno.XmlGraphicsList;
                drawArea1.iscup_List = anno.isCupProperties;
                #region Code to move the binary formatting serialization to existing serialization of graphics
                if (anno.Shapes == null)// == null)// Check if shapes list when saving of graphics in the older method
                {
                    isGraphicsListPresent = true;// Bool used for saving the older graphics to new method
                    anno.Shapes = new List<Shape>();
                    for (int i = 0; i < anno.XmlGraphicsList.Count; i++)
                    {
                        DrawObject dObject = anno.XmlGraphicsList[i];

                        Shape s = new Shape();
                        if (dObject is DrawPolygon)
                        {
                            DrawPolygon polyGon = dObject as DrawPolygon;
                            for (int j = 0; j < polyGon.pointArray.Count; j++)
                            {
                                s._shapeType = ShapeType.Annotation_Polygon;
                                s.PointArray.Add((Point)polyGon.pointArray[j]);

                            }
                        }


                        anno.Shapes.Add(s);
                    }
                }
                #endregion
                //This for loop has been added by Darshan on 27-10-2015 to draw existing graph to the drawarea.
                for (int i = anno.Shapes.Count - 1; i >= 0; --i)
                {
                    Shape s = anno.Shapes[i];
                    if (s._shapeType == ShapeType.Annotation_Polygon)
                    {
                        ToolPolygon t = new ToolPolygon();
                        DrawArea.isDrawCup = drawArea1.iscup_List[i];

                        t.AddExistingObject(drawArea1, s);
                    }
                }
                //This code has been added by darshan to display the saved ReportedBy name.
                if (!string.IsNullOrEmpty(anno.ReportedBy))
                {
                    Reportename_txt.Text = anno.ReportedBy;
                }
                if (!string.IsNullOrEmpty(anno.CDRComments))
                {
                    comments_tbx.Text = comments = anno.CDRComments;
                }
                drawArea1.comments = anno.Xmlcommentsproperties;
            }
            drawArea1.Refresh();
            Args e = new Args();
            //This if statement is added by Darshan on 27-10-2015 to save the shape for each  older graph.
            if (!isFromSvg)
            {
                e["isAnnotationUpdate"] = true;
                for (int i = 0; i < drawArea1.iscup_List.Count; i++)
                {
                    if (drawArea1.iscup_List[i])
                    {
                        drawArea1.iscup_drawn = drawArea1.iscup_List[i];
                    }
                }
                SaveAnnotation(e);
                isGraphicsListPresent = false;
                isFromSvg = true;
            }
        }

        public void commentsdgv_view(string comments)
        {

        }

        // public void showExisitingAnnotation()
        // {
        //     AnnotationsFiles_dgv.ForeColor = Color.Black;
        //     repo = AnnotationRepository_jason.GetInstance();
        //     List<INTUSOFT.Data.Model.AnnotationModel> reports = repo.GetByCategory("Image_ID", Annotation_ImageId).ToList<INTUSOFT.Data.Model.AnnotationModel>();
        //     reports = reports.OrderBy(x => x.Date_Time).ToList();
        //     reports.Reverse();
        //     //AnnotationsFiles_dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //     List<string> date = new List<string>();
        //     List<string> time = new List<string>();
        //     //DataGridViewColumn  col = new DataGridViewColumn(); 
        //     //      DataGridViewCell cell = new DataGridViewCell(); 
        //     //DataGridViewColumn col = new DataGridViewColumn();
        //     ////DataGridView co1l = new DataGridViewColumn() ;
        //     AnnotationsFiles_dgv.DataSource = reports.ToDataTable();
        //     AnnotationsFiles_dgv.AllowUserToAddRows = false;
        //     AnnotationsFiles_dgv.RowHeadersVisible = false;
        //     AnnotationsFiles_dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect; 

        //     if (!AnnotationsFiles_dgv.Columns.Contains(headers[0]) && !AnnotationsFiles_dgv.Columns.Contains(headers[1]) && !AnnotationsFiles_dgv.Columns.Contains(headers[2]) && !AnnotationsFiles_dgv.Columns.Contains(headers[3]))
        //     {

        //         var col3 = new DataGridViewTextBoxColumn();
        //         var col4 = new DataGridViewTextBoxColumn();
        //         DataGridViewLinkColumn col5 = new DataGridViewLinkColumn();
        //         DataGridViewImageColumn col6 = new DataGridViewImageColumn();
        //         col3.HeaderText = headers[0];
        //         if (Screen.PrimaryScreen.Bounds.Width != 1920)
        //         {
        //             col3.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
        //         }
        //         else
        //         {
        //             col3.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

        //         }

        //         col3.Name = headers[0];

        //         col4.HeaderText = headers[1];
        //         if (Screen.PrimaryScreen.Bounds.Width != 1920)
        //         {
        //             col4.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
        //         }
        //         else
        //         {
        //             col4.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

        //         }

        //         col4.Name = headers[1];
        //         col5.HeaderText = headers[2];
        //         col5.Name = headers[2];
        //         col5.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ;

        //         col6.HeaderText = headers[3];
        //         col6.Name = headers[3];
        //         col6.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

        //         col6.Image = Image.FromFile(@"ImageResources\Trash_Delete.png");

        //         try
        //         {
        //             AnnotationsFiles_dgv.Columns.AddRange(new DataGridViewColumn[] { col3, col4, col5, col6 });
        //         }
        //         //DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
        //         catch (Exception e)
        //         {
        //             Console.WriteLine(e.Message);
        //         }
        //         //foreach (DataGridViewRow item in AnnotationsFiles_dgv.Rows)
        //         //{
        //         //    DataGridViewLinkCell linCell = new DataGridViewLinkCell();
        //         //    linCell.Value = "view";
        //         //    //col5.Text = linCell;
        //         //    item.Cells[Resources.Report_View_Text] = linCell;
        //         //}

        //         for (int i = 0; i < AnnotationsFiles_dgv.Columns.Count; i++)
        //         {
        //             if (AnnotationsFiles_dgv.Columns[i].Name.Equals(headers[0]) || AnnotationsFiles_dgv.Columns[i].Name.Equals(headers[1]) || AnnotationsFiles_dgv.Columns[i].Name.Equals(headers[2]) || AnnotationsFiles_dgv.Columns[i].Name.Equals(headers[3]))
        //             {
        //                 AnnotationsFiles_dgv.Columns[i].Visible = true;

        //             }

        //             else
        //                 AnnotationsFiles_dgv.Columns[i].Visible = false;


        //         }


        //     }


        //     foreach (DataGridViewRow item in AnnotationsFiles_dgv.Rows)
        //     {
        //         DataGridViewLinkCell linCell = new DataGridViewLinkCell();
        //         linCell.Value = "view";
        //         //col5.Text = linCell;
        //         item.Cells[headers[2]] = linCell;
        //         item.Cells[headers[2]].Style.Alignment = DataGridViewContentAlignment.MiddleCenter; 

        //     }

        //     for (int i = 0; i < reports.Count; i++)
        //     {
        //         AnnotationsFiles_dgv.Rows[i].Cells[headers[0]].Value = reports[i].Date_Time.ToString("dd-MMM-yyyy");
        //         //This code has been added by Darshan on 14-08-2015 6:36 PM to solve Defect no 0000553: Time settings are not reflecting correctly.
        //         if (is24hourformat)
        //         {
        //             AnnotationsFiles_dgv.Rows[i].Cells[headers[1]].Value = reports[i].Date_Time.ToString(" HH:mm ");

        //         }
        //         else
        //         {

        //             AnnotationsFiles_dgv.Rows[i].Cells[headers[1]].Value = reports[i].Date_Time.ToString("hh:mm tt");
        //         }

        //     }
        //     //AnnotationsFiles_dgv.Sort(AnnotationsFiles_dgv.Columns[1], ListSortDirection.Ascending);
        //     //for (int i = 0; i < AnnotationsFiles_dgv.Columns.Count; i++)
        //     //{
        //     //    AnnotationsFiles_dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

        //     //}



        //     if (Screen.PrimaryScreen.Bounds.Width == 1920)
        //     {
        //         AnnotationsFiles_dgv.Columns[headers[0]].Width = 140;
        //         AnnotationsFiles_dgv.Columns[headers[1]].Width = 140;
        //         AnnotationsFiles_dgv.Columns[headers[2]].Width = 75;
        //         AnnotationsFiles_dgv.Columns[headers[3]].Width = 78;

        //         foreach (DataGridViewColumn c in AnnotationsFiles_dgv.Columns)
        //         {
        //             c.DefaultCellStyle.Font = new Font("Tahoma", 13.5F, GraphicsUnit.Pixel);
        //         }
        //     }
        //     else
        //         if (Screen.PrimaryScreen.Bounds.Width == 1366)
        //         {
        //             AnnotationsFiles_dgv.Columns[headers[0]].Width = 70;
        //             AnnotationsFiles_dgv.Columns[headers[1]].Width = 70;
        //             AnnotationsFiles_dgv.Columns[headers[2]].Width = 64;
        //             AnnotationsFiles_dgv.Columns[headers[3]].Width = 64;
        //             foreach (DataGridViewColumn c in AnnotationsFiles_dgv.Columns)
        //             {
        //                 c.DefaultCellStyle.Font = new Font("Tahoma", 11.0F, GraphicsUnit.Pixel);
        //             }
        //         }
        //         else
        //             if (Screen.PrimaryScreen.Bounds.Width == 1280)
        //             {
        //                 AnnotationsFiles_dgv.Columns[headers[0]].Width = 70;
        //                 AnnotationsFiles_dgv.Columns[headers[1]].Width = 70;
        //                 AnnotationsFiles_dgv.Columns[headers[2]].Width = 60;
        //                 AnnotationsFiles_dgv.Columns[headers[3]].Width = 63;
        //                 foreach (DataGridViewColumn c in AnnotationsFiles_dgv.Columns)
        //                 {
        //                     c.DefaultCellStyle.Font = new Font("Tahoma", 11.0F, GraphicsUnit.Pixel);
        //                 }

        //             }
        //     //AnnotationsFiles_dgv.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
        //     AnnotationsFiles_dgv.Refresh();
        // }
        public void loadDrawings()
        {
            string newFileName = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML Files|*.xml";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                newFileName = ofd.FileName;
            }
            else return;
            using (Stream stream = new FileStream(
                            newFileName, FileMode.Open, FileAccess.Read))
            {
                // Deserialize object from text format
                IFormatter formatter = new BinaryFormatter();

                SerializationEventArgs args = new SerializationEventArgs(
                    formatter, stream, newFileName);

                // raise event to load document from file
                drawArea1.ListGraphics = (GraphicsList)args.Formatter.Deserialize(args.SerializationStream);
                drawArea1.Refresh();
            }


        }
        public static void Serialize(Object data, string fileName)
        {
            Type type = data.GetType();
            XmlSerializer xs = new XmlSerializer(type);
            XmlTextWriter xmlWriter = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
            xs.Serialize(xmlWriter, data);
            xmlWriter.Close();
        }
        public static Object Deserialize(Type type, string fileName)
        {
            XmlSerializer xs = new XmlSerializer(type);

            XmlTextReader xmlReader = new XmlTextReader(fileName);
            Object data = xs.Deserialize(xmlReader);

            xmlReader.Close();

            return data;
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            isControl = false;
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control || (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                isControl = true;
            }
        }
        public void CDR_measurements()
        {
            drawArea1.CalculatePolygonArea();
            unsafe
            {
                string unitSquareMM = "\xB2";
                areaCupVal_lbl.Text = Math.Round(DrawArea.retCDRValues->CupArea * sqK, 2).ToString() + "  " + millimeterText + unitSquareMM;
                areaDiscVal_lbl.Text = Math.Round(DrawArea.retCDRValues->DiscArea * sqK, 2).ToString() + "  " + millimeterText + unitSquareMM;
                areaRimVal_lbl.Text = Math.Round(DrawArea.retCDRValues->RimArea * sqK, 2).ToString() + "  " + millimeterText + unitSquareMM;
                verticalLineDiscVal_lbl.Text = Math.Round(DrawArea.retCDRValues->VerticalLengthDisc * k, 2).ToString() + "   " + millimeterText;
                verticalLineCupVal_lbl.Text = Math.Round(DrawArea.retCDRValues->VerticalLengthCup * k, 2).ToString() + "   " + millimeterText;
                horizontalLineCupVal_lbl.Text = Math.Round(DrawArea.retCDRValues->HorizontalLengthCup * k, 2).ToString() + "   " + millimeterText;
                horizontalLineDiscVal_lbl.Text = Math.Round(DrawArea.retCDRValues->HorizontalLengthDisc * k, 2).ToString() + "   " + millimeterText;
                verticalCDRVal_lbl.Text = Math.Round(DrawArea.retCDRValues->VerticalCDR, 2).ToString();
                horizontalCDRVal_lbl.Text = Math.Round(DrawArea.retCDRValues->HorizontalCDR, 2).ToString();
                //areaCup_lbl.Text = "Cup Area = " + DrawArea.retCDRValues->CupArea.ToString();
                //areaCup_lbl.Text = "Cup Area = " + DrawArea.retCDRValues->CupArea.ToString();
                superior_lbl.Text = Math.Round(DrawArea.retCDRValues->SuperiorRegionArea * sqK, 2).ToString() + "" + millimeterText + unitSquareMM;
                inferior_lbl.Text = Math.Round(DrawArea.retCDRValues->InferiorRegionArea * sqK, 2).ToString() + "" + millimeterText + unitSquareMM;
                //if (isOD)//Changed because Nasal and temporal values were reversed
                //{

                temporal_lbl.Text = Math.Round(DrawArea.retCDRValues->TemporalRegionArea * sqK, 2).ToString() + "" + millimeterText + unitSquareMM;
                nasal_lbl.Text = Math.Round(DrawArea.retCDRValues->NasalRegionArea * sqK, 2).ToString() + "" + millimeterText + unitSquareMM;
                //}
                //else
                //{
                //    temporal_lbl.Text = Math.Round(DrawArea.retCDRValues->NasalRegionArea * sqK, 2).ToString() + "" + millimeterText + unitSquareMM;
                //    nasal_lbl.Text = Math.Round(DrawArea.retCDRValues->TemporalRegionArea * sqK, 2).ToString() + "" + millimeterText + unitSquareMM;
                //}
            }
        }
        private void CalculateCDR_btn_Click(object sender, EventArgs e)
        {
            if (drawArea1.iscup_drawn && drawArea1.graphicsList.Count == 2)
            {
                drawArea1.graphicsList.UnselectAll();
                drawArea1.Refresh();
                HighlightSelectedButton(CalculateCDR_btn, this.CDRTool_ts);
                CDR_measurements();
            }
        }

        //This Form Closing event has been added by Darshan on 01-09-2015 to solve Defect no 0000602: CR:Confirmation message required before closing the Annotation Window.
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventArgs e1 = null;
            if (comments_tbx.Text != comments || saveDrawing_btn.Enabled)
            {
                if (drawArea1.ListGraphics.Count > 1 && !isview && !isSave )
                {
                    DialogResult res = CustomMessageBox.Show(saveConfirmation, this.Text, CustomMessageBoxButtons.YesNo, CustomMessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                    {
                        Args arg = new Args();
                        arg["isAnnotationUpdate"] = false;
                        SaveAnnotation(arg);
                    }
                }
                
            }
            ToolPolygon.DrawCupPolygon = null;
            ToolPolygon.DrawDiscPolygon = null;
        }

        private void drawCup_btn_Click(object sender, EventArgs e)
        {
            if (!isControl)
            {
                drawArea1.cupColor = Color.Blue;
                HighlightSelectedButton(drawCup_btn, this.CDRTool_ts);
                DrawArea.isDrawCup = true;
                CommandPolygon();
                if (ToolPolygon.DrawCupPolygon != null)
                {
                    for (int i = 0; i < drawArea1.graphicsList.Count; i++)
                    {
                        DrawPolygon polyGon = drawArea1.graphicsList[i] as DrawPolygon;
                        if (polyGon.isCup)
                        {
                            drawArea1.graphicsList[i].Selected = true;
                        }
                        else
                            drawArea1.graphicsList[i].Selected = false;
                    }
                }
                drawArea1.Refresh();
            }
        }
        private void redChannel_btn_Click(object sender, EventArgs e)
        {
            //This below line has been added by darshan to solve Defect no 0000680: The Highlighting of the color channels must happen as in view image screen.
            //HighlightSelectedButton(redChannel_btn, this.toolStrip2);
            //Display_pbx.Image = redChannel.ToBitmap();
            if (CurrentChannelDisplayed != Channels.Red)
            {
                CurrentChannelDisplayed = Channels.Red;
                #region 0001336 has been fixed by copying the channel image to temp bitmap and drawing to the modifying bitmap
                tempBm = colorImage[2].ToBitmap(); // in order to have a place for indexed bitmap only occuring for channel images

                #endregion
            }
            else
            {
                tempBm = colorImage.ToBitmap(); // in order to have a place for indexed bitmap only occuring for channel images

                CurrentChannelDisplayed = Channels.Color;
            }
            Display_pbx.Image = tempBm;
            RGBbackgroundcolor();
        }

        private void greenChannel_btn_ButtonClick(object sender, EventArgs e)
        {
            //This below line has been added by darshan to solve Defect no 0000680: The Highlighting of the color channels must happen as in view image screen.
            //HighlightSelectedButton(greenChannel_btn, this.toolStrip2);
            //Display_pbx.Image = greenChannel.ToBitmap();
            if (CurrentChannelDisplayed != Channels.Green)
            {
                CurrentChannelDisplayed = Channels.Green;
                #region 0001336 has been fixed by copying the channel image to temp bitmap and drawing to the modifying bitmap
                tempBm = colorImage[1].ToBitmap(); // in order to have a place for indexed bitmap only occuring for channel images

                #endregion
            }
            else
            {
                tempBm = colorImage.ToBitmap(); // in order to have a place for indexed bitmap only occuring for channel images

                CurrentChannelDisplayed = Channels.Color;
            }
            Display_pbx.Image = tempBm;
            RGBbackgroundcolor();
        }
        private void blueChannel_btn_Click(object sender, EventArgs e)
        {
            //This below line has been added by darshan to solve Defect no 0000680: The Highlighting of the color channels must happen as in view image screen.
            //HighlightSelectedButton(blueChannel_btn, this.toolStrip2);
            //Display_pbx.Image = blueChannel.ToBitmap();
            if (CurrentChannelDisplayed != Channels.Blue)
            {
                CurrentChannelDisplayed = Channels.Blue;
                #region 0001336 has been fixed by copying the channel image to temp bitmap and drawing to the modifying bitmap
                tempBm = colorImage[0].ToBitmap(); // in order to have a place for indexed bitmap only occuring for channel images

                #endregion
            }
            else
            {
                tempBm = colorImage.ToBitmap(); // in order to have a place for indexed bitmap only occuring for channel images

                CurrentChannelDisplayed = Channels.Color;
            }
            Display_pbx.Image = tempBm;
            RGBbackgroundcolor();
        }

        /// <summary>
        /// Sets the backgroundcolor of the filter buttons present on the view imaging screen.
        /// </summary>
        public void RGBbackgroundcolor()
        {
            try
            {
                switch (CurrentChannelDisplayed)
                {
                    case Channels.Red:
                        {
                            redChannel_btn.ToolTipText = "Back to color";// IVLVariables.LangResourceManager.GetString("Red_Filter_Selected_Tool_Tip_Text", IVLVariables.LangResourceCultureInfo);
                            greenChannel_btn.ToolTipText = "Show green channel";// IVLVariables.LangResourceManager.GetString("GreenFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
                            blueChannel_btn.ToolTipText = "Show blue channel";//IVLVariables.LangResourceManager.GetString("BlueFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
                            redChannel_btn.Image = redFilterSelected;
                            greenChannel_btn.Image = greenFilter;
                            blueChannel_btn.Image = blueFilter;
                        }
                        break;
                    case Channels.Green:
                        {
                            greenChannel_btn.ToolTipText = "Back to color";//IVLVariables.LangResourceManager.GetString("Red_Filter_Selected_Tool_Tip_Text", IVLVariables.LangResourceCultureInfo);
                            redChannel_btn.ToolTipText = "Show red channel";//IVLVariables.LangResourceManager.GetString("RedFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
                            blueChannel_btn.ToolTipText = "Show blue channel";//IVLVariables.LangResourceManager.GetString("BlueFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
                            redChannel_btn.Image = redFilter;
                            blueChannel_btn.Image = blueFilter;
                            greenChannel_btn.Image = greenFilterSelected;

                        }
                        break;
                    case Channels.Blue:
                        {
                            blueChannel_btn.ToolTipText = "Back to color";//IVLVariables.LangResourceManager.GetString("Red_Filter_Selected_Tool_Tip_Text", IVLVariables.LangResourceCultureInfo);
                            redChannel_btn.ToolTipText = "Show red channel";//IVLVariables.LangResourceManager.GetString("RedFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
                            greenChannel_btn.ToolTipText = "Show green channel";//IVLVariables.LangResourceManager.GetString("GreenFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
                            redChannel_btn.Image = redFilter;
                            blueChannel_btn.Image = blueFilterSelected;
                            greenChannel_btn.Image = greenFilter;
                        }
                        break;
                    case Channels.Color:
                        {
                            redChannel_btn.ToolTipText = "Show red channel";//IVLVariables.LangResourceManager.GetString("RedFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
                            greenChannel_btn.ToolTipText = "Show green channel";//IVLVariables.LangResourceManager.GetString("GreenFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
                            blueChannel_btn.ToolTipText = "Show blue channel";//IVLVariables.LangResourceManager.GetString("BlueFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
                            redChannel_btn.Image = redFilter;
                            blueChannel_btn.Image = blueFilter;
                            greenChannel_btn.Image = greenFilter;
                        }
                        break;
                    default:
                        {
                            redChannel_btn.ToolTipText = "Show red channel";//IVLVariables.LangResourceManager.GetString("RedFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
                            greenChannel_btn.ToolTipText = "Show green channel";//IVLVariables.LangResourceManager.GetString("GreenFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
                            blueChannel_btn.ToolTipText = "Show blue channel";//IVLVariables.LangResourceManager.GetString("BlueFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
                            redChannel_btn.Image = redFilter;
                            blueChannel_btn.Image = blueFilter;
                            greenChannel_btn.Image = greenFilter;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                //Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
                //                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        private void colorImage_btn_Click(object sender, EventArgs e)
        {
            //This below line has been added by darshan to solve Defect no 0000680: The Highlighting of the color channels must happen as in view image screen.
            //HighlightSelectedButton(colorImage_btn, this.toolStrip2);
            //Display_pbx.Image = colorImage.ToBitmap();
        }

        private void GlaucomaTool_Load(object sender, EventArgs e)
        {
            ResizeDrawArea();
            drawArea1.Initialize(this);
            pbxHeight = Display_pbx.Height;
            pbxWidth = Display_pbx.Width;
            if (!string.IsNullOrEmpty(cdrxml))
            {
                //viewannotation(cdrxml);
                //CalculateCDR_btn_Click(null, null);
            }
            drawArea1.Refresh();
        }

        // Added this event to increase the font size when comments textbox is entered from focus by sriram on september 24 2015
        private void comments_tbx_Enter(object sender, EventArgs e)
        {
            EnterCommentsBox();
        }
        private void EnterCommentsBox()
        {
            if (comments_tbx.Text == defaultComment)
            {
                comments_tbx.Text = "";
                comments_tbx.ForeColor = Color.Black;
                Font f = comments_tbx.Font;
                Font newFont = new Font(f.FontFamily, f.Size, FontStyle.Regular, GraphicsUnit.Point);
                comments_tbx.Font = newFont;

            }
            CommandPointer();
            HighlightSelectedButton(null, CDRTool_ts);
            for (int i = 0; i < drawArea1.graphicsList.Count; i++)
            {
                drawArea1.graphicsList[i].Selected = false;
            }

            drawArea1.Refresh();
        }
        // Added this event to increase the font size when comments textbox is left from focus by sriram on september 24 2015
        private void comments_tbx_Leave(object sender, EventArgs e)
        {
            
            if (string.IsNullOrEmpty(comments_tbx.Text))
            {
                comments_tbx.ForeColor = SystemColors.WindowFrame;
                Font f = comments_tbx.Font;
                Font newFont = new Font(f.FontFamily, f.Size, FontStyle.Italic, GraphicsUnit.Point);
                comments_tbx.Font = newFont;
                comments_tbx.Text = comments;
            }
        }

        private void GlaucomaTool_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                drawArea1.ListGraphics.isDelete = true;
                if (drawArea1.ListGraphics.SelectionCount != 0 && drawArea1.ListGraphics.SelectionCount == 1)
                {

                    if (drawArea1.ListGraphics.Count < 2 || drawArea1.ListGraphics.selection_id == 2)
                    {
                        drawArea1.ListGraphics.DeleteSelection();
                        drawArea1.Refresh();
                    }
                    else
                    {
                        DialogResult res = CustomMessageBox.Show(DiscDeletionWarningMessage, DiscDeletionWarningHeader, CustomMessageBoxButtons.YesNo, CustomMessageBoxIcon.Warning);
                        if (res == DialogResult.Yes)
                        {
                            drawArea1.ListGraphics.DeleteAll();
                            drawArea1.Refresh();
                        }
                        else if (res == DialogResult.No)
                        {
                            return;
                        }
                    }
                    int[] id = drawArea1.ListGraphics.ids.Reverse().ToArray();
                    Measurements_default_values();
                }
            }
            else if (e.KeyCode == Keys.A && e.Control)
            {
                if (comments_tbx.Focused)
                {
                    comments_tbx.SelectAll();
                }
                else if (Reportename_txt.Focused)
                {
                    Reportename_txt.SelectAll();
                }
            }
        }

        private void drawCupPoints_btn_Click(object sender, EventArgs e)
        {
            ToolPolygon.modifyPolygon = true;
            DrawArea.isDrawCup = true;
            drawArea1.ActiveTool = DrawArea.DrawToolType.Pointer;
        }

        //The parameter of this function has been changed by Darshan to make this function usable for all toolstrip available.
        private void HighlightSelectedButton(ToolStripButton c, ToolStrip p)
        {
            foreach (ToolStripItem item in p.Items)
            {
                if (c != null)//checks if any of the item in toolstrip is selected.By Ashutosh 25-07-2017
                {
                    if (item == c)// if selected
                    {
                        //This if statement has been added by Darshan to not change the color of the colorImage_btn button since in view screen color image button is not highlighted.
                        //if (!item.Name.Equals(colorImage_btn.Name))
                            item.BackColor = Color.Blue;

                    }
                    else
                    item.BackColor = SystemColors.ButtonFace;
                }
                else// it no item is selected then button colour will be default color.By Ashutosh 25-07-2017
                    item.BackColor = SystemColors.ButtonFace;
            }
        }

        private void MovePoints_btn_Click(object sender, EventArgs e)
        {
            ToolPolygon.modifyPolygon = true;
            HighlightSelectedButton(MovePoints_btn, this.CDRTool_ts);
            DrawArea.isDrawCup = false;
            drawArea1.ActiveTool = DrawArea.DrawToolType.Pointer;
        }

        private void updateGlaucomaToolControls(string s, Args arg)
        {
            if (arg.ContainsKey("ModifyDiscPoints"))
            {
                MovePoints_btn.Enabled = (bool)arg["ModifyDiscPoints"];
            }
            if (arg.ContainsKey("DrawCup"))
            {
                drawCup_btn.Enabled = (bool)arg["DrawCup"];
            }
            if (arg.ContainsKey("MeasureCDR"))
            {
                CalculateCDR_btn.Enabled = (bool)arg["MeasureCDR"];
            }
            if (arg.ContainsKey("Print"))
            {
                Print_btn.Enabled = (bool)arg["Print"];
            }
            if (arg.ContainsKey("Save"))
            {
                saveDrawing_btn.Enabled = (bool)arg["Save"];
            }
            if (arg.ContainsKey("Export"))// check if the dictionary contains the string.
            {
                Export_btn.Enabled = (bool)arg["Export"];//if string present , its bool status given to Export_btn.Enabled. By Ashutosh 21-7-2017
            }
            if (arg.ContainsKey("IsViewed"))// check if the dictionary contains the string.
            {
                isview = (bool)arg["IsViewed"];//if string present , its bool status given to Export_btn.Enabled. By Ashutosh 21-7-2017
            }
        }

        private void comments_tbx_TextChanged(object sender, EventArgs e)
        {
        }

        private void Print_btn_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            //This below code has been added by Darshan on 20-10-2015 to draw the graphs in the GlaucomaTool on the image (Since graphs are being drawn on a panel). And to save the CDR image for printing purpose.
            isPrint = true;
            
            GenerateReport();//method generates report. ashutosh 21/7
            if (isPrint)
            {
                _report.isPrintingOver = IVLReport.PrintReport.PrintPDFs(CustomFolderBrowser.filePath+ _report.ReportFileName, _report.adobereader_text);
                //This below 3 line has been added to save the glacoma report when it is printed.
                if (_report.isPrintingOver)
                {
                    Args arg = new Args();
                    arg["isAnnotationUpdate"] = false;
                    SaveAnnotation(arg);
                }
            }
            this.Cursor = Cursors.Default;
        }

        public void GenerateReport()
        {
            Bitmap bm = new Bitmap(Display_pbx.Image as Bitmap);// tempbm.ToBitmap();// as Bitmap; This line has been changed by Darshan on 11-mar-2016 to print image in anyone of the color channel.
            for (int i = 0; i < drawArea1.ListGraphics.Count; i++)
            {
                if (drawArea1.ListGraphics[i] is DrawPolygon)
                {
                    DrawPolygon dpolygon = drawArea1.ListGraphics[i] as DrawPolygon;
                    dpolygon.DrawPolygonToImage(ref bm, drawArea1.ListGraphics[i].ID);
                }

            }
            if (glaucomaToolValues.ContainsKey("$CurrentImagebm"))
                glaucomaToolValues["$CurrentImagebm"] = bm.Clone() as Bitmap;
            else
                glaucomaToolValues.Add("$CurrentImagebm", bm.Clone() as Bitmap);
            
            bm.Dispose();
            //Below code has been added to send patient details and CDR measurement value to the report.
            if (glaucomaToolValues.ContainsKey("$ISNT"))
                glaucomaToolValues["$ISNT"] = isnt_pbx.Tag.ToString();
            else
                glaucomaToolValues.Add("$ISNT", isnt_pbx.Tag.ToString());
            //This below if statement has been added to send comments from CDR form to CDR report.
            if (string.IsNullOrEmpty(comments_tbx.Text))
            {
                glaucomaToolValues["$Comments"] = string.Empty;
            }
            else
            {
                string comments = Regex.Replace(comments_tbx.Text, @"\r\n?|\n", " ");
                glaucomaToolValues["$Comments"] = comments;
            }


            Dictionary<string, string> cdrValuesDic = new Dictionary<string, string>();// Added to save the mesaurement details from CDR into a dictionary.
            cdrValuesDic.Add(glaucomaToolValues["$areaDisc"] as string, areaDiscVal_lbl.Text);//Adds the areaDisc label text and value to cdrValuesDic.
            cdrValuesDic.Add(glaucomaToolValues["$areaCup"] as string, areaCupVal_lbl.Text);//Adds the areaCup label text and value to cdrValuesDic.
            cdrValuesDic.Add(glaucomaToolValues["$areaRim"] as string, areaRimVal_lbl.Text);//Adds the areaRim label text and value to cdrValuesDic.

            cdrValuesDic.Add(glaucomaToolValues["$verticalCupLen"] as string, verticalLineCupVal_lbl.Text);//Adds the verticalCupLen label text and value to cdrValuesDic.
            cdrValuesDic.Add(glaucomaToolValues["$horizontalCupLen"] as string, horizontalLineCupVal_lbl.Text);//Adds the horizontalCupLen label text and value to cdrValuesDic.
            cdrValuesDic.Add(glaucomaToolValues["$VerticalDiscLen"] as string, verticalLineDiscVal_lbl.Text);//Adds the VerticalDiscLen label text and value to cdrValuesDic.
            cdrValuesDic.Add(glaucomaToolValues["$horizontalDiscLen"] as string, horizontalLineDiscVal_lbl.Text);//Adds the horizontalDiscLen label text and value to cdrValuesDic.
            cdrValuesDic.Add(glaucomaToolValues["$verticalCDR"] as string, verticalCDRVal_lbl.Text);//Adds the verticalCDR label text and value to cdrValuesDic.
            cdrValuesDic.Add(glaucomaToolValues["$horizontalCDR"] as string, horizontalCDRVal_lbl.Text);//Adds the horizontalCDR label text and value to cdrValuesDic.

            if (cdrValuesDic.ContainsKey("$AnnotationComments"))//Checks for the availability of the key $AnnotationComments if exists removes it.
                cdrValuesDic.Remove("$AnnotationComments");
            cdrValuesDic.Add("$tableColumnWidthA4", "1.2");//second column width in the tabel for A4 sheet. 
            cdrValuesDic.Add("$tableColumnHeightA4", "1.3");//first column width in the tabel for A4 sheet.;
            cdrValuesDic.Add("$tableColumnWidthA5", "0.6");//second column width in the tabel for A5 sheet.
            cdrValuesDic.Add("$tableColumnHeightA5", "0.65");//first column width in the tabel for A5 sheet.;

            #region check this
            if (glaucomaToolValues.ContainsKey("$AnnotationComments"))
                glaucomaToolValues["$AnnotationComments"] = cdrValuesDic;
            else
                glaucomaToolValues.Add("$AnnotationComments", cdrValuesDic);
            #endregion

          
            {
                if (glaucomaToolValues.ContainsKey("$Nasal"))
                    glaucomaToolValues["$Nasal"] = temporal_lbl.Text;
                else
                    glaucomaToolValues.Add("$Nasal", temporal_lbl.Text);

                if (glaucomaToolValues.ContainsKey("$Temporal"))
                    glaucomaToolValues["$Temporal"] = nasal_lbl.Text;
                else
                    glaucomaToolValues.Add("$Temporal", nasal_lbl.Text);
            }
            if (glaucomaToolValues.ContainsKey("$Superior"))
                glaucomaToolValues["$Superior"] = superior_lbl.Text;
            else
                glaucomaToolValues.Add("$Superior", superior_lbl.Text);

            if (glaucomaToolValues.ContainsKey("$Inferior"))
                glaucomaToolValues["$Inferior"] = inferior_lbl.Text;
            else
                glaucomaToolValues.Add("$Inferior", inferior_lbl.Text);

            

            glaucomaToolValues.Remove("$Doctor");
            //This below if statement will send the text present in Reported by text to the operator field in CDR report.
            if (Reportename_txt.Text.Equals(glaucomaToolValues["$repotedBy"]) || string.IsNullOrEmpty(Reportename_txt.Text.ToString()))
            {
                glaucomaToolValues.Add("$Doctor", string.Empty);
            }
            else
            {
                glaucomaToolValues.Add("$Doctor", Reportename_txt.Text);
            }



            if (glaucomaToolValues.ContainsKey("$isFromCDR"))
                glaucomaToolValues["$isFromCDR"] = true;
            else
                glaucomaToolValues.Add("$isFromCDR", true);//isFromCDR is added to make the report to know from where it is being invoked.

            if (glaucomaToolValues.ContainsKey("$isAnnotation"))
                glaucomaToolValues["$isAnnotation"] = false;
            else
                glaucomaToolValues.Add("$isAnnotation", false);


           

            if (glaucomaToolValues.ContainsKey("$isOD"))
                glaucomaToolValues["$isOD"] = isOD;
            else
                glaucomaToolValues.Add("$isOD", isOD);

            _report = new IVLReport.Report(glaucomaToolValues);
            
            _report.parseXmlData(glaucomaToolValues["$currentTemplate"] as string);
            _report.SetTheValuesFormReportData();
            CustomFolderBrowser.fileName = "CDR_Report_" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss") + ".pdf";
            try
            {
                _report.CreatePdf();
            }
            catch (Exception ex)
            {
                
                throw;
            }
            
         
            this.Cursor = Cursors.Default;
        }
        private void Reportename_txt_Enter(object sender, EventArgs e)
        {
            if (Reportename_txt.Text == reportedBy)
            {
                Reportename_txt.Text = "";
                Reportename_txt.ForeColor = Color.Black;
                Font f = Reportename_txt.Font;
                Font newFont = new Font(f.FontFamily, f.Size, FontStyle.Regular, GraphicsUnit.Point);
                Reportename_txt.Font = newFont;
            }
        }

        private void Reportename_txt_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Reportename_txt.Text))
            {
                //This if statement has been added to replicate the same functionality as it was in constructor.
                if (glaucomaToolValues.ContainsKey("Doctor"))
                {
                    if (!string.IsNullOrEmpty(glaucomaToolValues["Doctor"] as string))
                    {
                        Reportename_txt.Text = glaucomaToolValues["Doctor"].ToString();
                        Reportename_txt.ForeColor = Color.Black;
                        Font f = Reportename_txt.Font;
                        Font newFont = new Font(f.FontFamily, f.Size, FontStyle.Regular, GraphicsUnit.Point);
                        Reportename_txt.Font = newFont;
                    }
                    else
                    {
                        Reportename_txt.Text = reportedBy;
                        Reportename_txt.ForeColor = SystemColors.WindowFrame;
                        Font f = Reportename_txt.Font;
                        Font newFont = new Font(f.FontFamily, f.Size, FontStyle.Italic, GraphicsUnit.Point);
                        Reportename_txt.Font = newFont;
                    }
                }
            }
        }

        private void Reportename_txt_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
            {
                if (char.IsControl(e.KeyChar))//Code modifed by darshan to solved defect no:Reported By field texts are getting hide after click the enter button
                {
                    if (e.KeyChar == '\r' || e.KeyChar == 127)
                        e.Handled = true;
                }
                else
                    e.Handled = false;
            }
            else
                e.Handled = true;
        }

        private void GlaucomaTool_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
        }

        private void comments_tbx_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == 127)
                e.Handled = true;
            
        }

        void customFolderBrowser_CancelButtonClickedEvent()
        {
            CustomFolderBrowser.CancelButtonClickedEvent -= customFolderBrowser_CancelButtonClickedEvent;
            //-=unsubscribes
            CustomFolderBrowser.ImageSavingbtn -= CustomFolderBrowser_ImageSavingbtn;

            customFolderBrowser.isReportExport = false;
        }

        private void Export_btn_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            customFolderBrowser = new CustomFolderBrowser();
            CustomFolderBrowser.fileName = "CDR_Report_" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss") + ".pdf";// to assingn the file name
            isPrint = false;
            customFolderBrowser.isReportExport = true;// this is necessary for ShowImageExportButtons() which along with isReportExport is part of customfolderbrowser.
            GenerateReport();
            Args arg = new Args(); // Args is a dictionary of <string , object >
            //arg["isAnnotationUpdate"] = true;//this line has been commented since the logic of making it true is wrong 20/7
            arg["isAnnotationUpdate"] = false;
            customFolderBrowser.CustomFolderData = glaucomaToolValues["$customFolderData"] as Dictionary<string, object>;//dictionary was not present earlier, due to which folder path when empty or invalid path led to application crash. By Ashutosh 18-08-2017

            customFolderBrowser.ShowImageExportButtons(); //this is to ensure the export labels and textbox contents to hide or visible.
            //SaveAnnotation(arg);//gets the datetime , imageid,Comments,annotationID from SaveAnnotation.this line has been coomented since the logic of making it true is wrong 20/7
            CustomFolderBrowser.CancelButtonClickedEvent += customFolderBrowser_CancelButtonClickedEvent;
            // this part subcribes the delegate or method in the right to internal list on the left
            CustomFolderBrowser.ImageSavingbtn += CustomFolderBrowser_ImageSavingbtn;
            //+= subscribes to an event. The delegate or method on the right-hand side of the += will be added to an internal list that the event keeps track of, and when the owning class fires that event, all the delegates in the list will be called.
            customFolderBrowser.ShowDialog();// this pops up the dialog
            this.Cursor = Cursors.Default;//Gets the default cursor, which is usually an arrow cursor
        }

        void CustomFolderBrowser_ImageSavingbtn()
        {
            string[] splitName = ( glaucomaToolValues["$Name"] as string).Split(new char[0]);//to split the name - first name and last name wrt space
            string dirPath = customFolderBrowser.folderPath + Path.DirectorySeparatorChar + glaucomaToolValues["$MRN"] + "_" + splitName[0] + "_" + splitName[1] + "_" + glaucomaToolValues["$Age"] + "_" +  glaucomaToolValues["$Gender"];
            if (!Directory.Exists(dirPath))//check whether the directory is not available 
                Directory.CreateDirectory(dirPath);//creates new directory
            //File.Move- moves files from source to destinagtion and provides option to specify a new filename.
            File.Copy(CustomFolderBrowser.filePath + CustomFolderBrowser.fileName, dirPath + Path.DirectorySeparatorChar + CustomFolderBrowser.fileName);
            CustomFolderBrowser.fileNames = new string[]{dirPath + Path.DirectorySeparatorChar + CustomFolderBrowser.fileName};
            //fileNames can handle multiple files. dirpath is a string , whereas fileNames is string[].
            //CustomFolderBrowser.fileName = dirPath + Path.DirectorySeparatorChar + CustomFolderBrowser.fileName;
            string reportExportTextStr = string.Empty;
            string reportExportHeaderStr = string.Empty;
            //below statements necessary for confirmation regarding the success of export.
            if (glaucomaToolValues.ContainsKey("$exportReportText"))
                reportExportTextStr = glaucomaToolValues["$exportReportText"] as string;
            if (glaucomaToolValues.ContainsKey("$exportReportHeader"))
                reportExportHeaderStr = glaucomaToolValues["$exportReportHeader"] as string;
            //messagebox to show that exporting is completed
            DialogResult exported = CustomMessageBox.Show(reportExportTextStr, reportExportHeaderStr, CustomMessageBoxIcon.Information);
            if (exported == DialogResult.OK)
            {
                customFolderBrowser.Close();
            }
        }

        private void comments_tbx_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            EnterCommentsBox();
        }

        private void comments_tbx_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Args arg = new Args();
            if (!saveDrawing_btn.Enabled)
                if (comments_tbx.Text != comments)//to enable save button if the comments in cdr form has been changed
                {
                    arg["Save"] = true;
                    _eventHandler.Notify(_eventHandler.UpdateGlaucomaToolControls, arg);
                }
        }
    }
}