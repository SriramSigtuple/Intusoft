using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Drawing.Imaging;
using System.IO;
using Svg;
using Emgu.CV;
using System.Text.RegularExpressions;
using Emgu.CV.Structure;
using Emgu.Util;
using INTUSOFT.Data.Repository;
using INTUSOFT.Data.Extension;
using INTUSOFT.Data.NewDbModel;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;
using INTUSOFT.EventHandler;
using Common;
using INTUSOFT.Data;
using INTUSOFT.Custom.Controls;

namespace Annotation
{
    public partial class Form1 : BaseGradientForm
    {
        private string AnnotationImagefileName = "";
        public enum Channels { Color, Red, Green, Blue };
        Channels CurrentChannelDisplayed;
        Image redFilterSelected, greenFilterSelected, blueFilterSelected;
        Image redFilter, greenFilter, blueFilter;
        string annotationtype = "";
        int annotationID = 0;
        IVLReport.Report _report;
        string[] headers;
        bool is24hourformat;
        int numberOfCDR = 0;
        public bool isCDRvalue = false;
        //This below bool variable was added by Darshan on 21-08-2015 to solve Defect no 0000591: when deleted the saved annotation,the current annotation performing graph is getting deleted.
        bool isview = false;
        public Dictionary<string, object> annotationDetails = null;
        AnnotationComments ac;
        bool isGraphicsListPresent = false;
        bool isSvgFormat = true;
        AnnotationText temp;
        string warningMessage = string.Empty;
        string warningHeader = string.Empty;
        string deleteMessage = string.Empty;
        string deleteHeader = string.Empty;
        string clearAnnotationMessage = string.Empty;//string created to handle cltrl+mouse click in annoatation datagridview.By Ashutosh 07-09-2017.
        string clearAnnotationHeader = string.Empty;//string created to handle cltrl+mouse click in annoatation datagridview.By Ashutosh 07-09-2017.
        string dr_operator = string.Empty;
        string reported_by = string.Empty;
        string noOfImagesAllowedMessage = string.Empty;
        string noOfImagesAllowedHeader = string.Empty;
        string deleteLogoPath = string.Empty;
        bool isControl = false;
        string[] annotationComments = null;
        Dictionary<string, object> reportDic = null;
        Image<Gray, byte> redChannel, greenChannel, blueChannel;
        Image<Bgr, byte> colorImage;
        int comment_Id = 0;
        int Annotation_ImageId = 0;
        string CDRcomments = string.Empty;
        SvgDocument svgDoc = null;
        private DrawArea drawArea1,drawArea2;
        public bool isPrint = false;// bool initialisation  to enable/disable print button.By Ashutosh 20-7-2017
        public bool isExport = false;//// bool initialisation to enable/disable export button.By Ashutosh 20-7-2017
        CustomFolderBrowser customFolderBrowser;//Object of type CustomFolderBrowser.
        int currentRowIndex = 0;//This variable has been added by Darshan to get the current row index of the selceted row.
        public static Bitmap tempBm = null;
        public delegate void AnnotationSavedDelegate(Args e);
        public event AnnotationSavedDelegate annotationSavedEvent;
        public delegate void AnnotationDeleteDelegate(Args e);
        public event AnnotationDeleteDelegate annotationDeleteEvent;
        public delegate void PatientDetailsUpdate();
        public event PatientDetailsUpdate _patientdetailsupdate;
        public IVLEventHandler _eventHandler;

        private Point startingPoint = Point.Empty;
        private Point movingPoint = Point.Empty;
        private bool panning = false;
        Bitmap pbxBm, temBm;
        float defaultZoom = 43f;
        float zoom = 43f;
        float zoomFactor = 5;
        float zoomMax = 400;
        float zoomMin = 1;
        float previousZoom = 43f;
        public float Zoom
        {
            get { return zoom; }
            set
            {
                    previousZoom = zoom;
                    zoom = value;
                    if (zoom <= zoomMin || zoom >= zoomMax)
                    {
                        if (zoom <= zoomMin)
                        {
                            zoom = 1;

                        }
                        else if (zoom >= zoomMax)
                            zoom = zoomMax;
                        if (zoomVal_tbx.Text != zoom.ToString())
                            zoomVal_tbx.Text = zoom.ToString();
                    }
                    ApplyZoom(zoom / previousZoom);
                    
                    
            }
        }
        #region Picturebox width and height are saved to static variables so that they can used during translation of the screen points to actual image points and also the image displayed on the picturebox is made public and static so that it is available for further analysis by sriram on august 31st 2015
        public static int pbxWidth = 0;
        public static int pbxHeight = 0;

        public static int ImageWidth = 0;
        public static int ImageHeight = 0;

        #endregion
        public Form1(Dictionary<string, object> annotationDetails)
        {

            InitializeComponent();
            this.filter_ts.Renderer = new FormToolStripRenderer();
            this.drawingTools_ts.Renderer = new FormToolStripRenderer();
            this.zoom_ts.Renderer = new FormToolStripRenderer();

            reportDic = new Dictionary<string, object>();
            NewDataVariables.Active_Annotation = null;//Form1 is loded from view image control, Active_Annotations is set to  null.By Ashutosh 21-08-2017
            reportDic = annotationDetails;// passing local dictionary(annotationDetails) to global dic(reportDic). enables its usage in entire class.By Ashutosh 20-7-2017
            this.Color1 = (Color)annotationDetails["$Color1"];
            this.Color2 = (Color)annotationDetails["$Color2"];
            this.FontColor = (Color)annotationDetails["$FontColor"];
            this.ColorAngle = (float)annotationDetails["$ColorAngle"];
            if (reportDic.ContainsKey("$CreatedFiles"))
                headers = reportDic["$CreatedFiles"] as string[];
            is24hourformat = (bool)annotationDetails["$is24hrformat"];
            AnnotationVariables.isGlaucomaTool = false;
            string ellipseToolLogoPath = @"ImageResources\AnnotationImageResources\Draw_Circle.png";
            string rectangleToolLogoPath = @"ImageResources\AnnotationImageResources\Draw_Rectangle.png";
            string pencilToolLogoPath = @"ImageResources\AnnotationImageResources\Draw_Freehand.png";
            string lineToolLogoPath = @"ImageResources\AnnotationImageResources\Draw_Arrow.png";
            string printToolLogoPath = @"ImageResources\Edit_ImageResources\PrintIcon.png";
            string saveToolLogoPath = @"ImageResources\Edit_ImageResources\SaveIcon.png";
            deleteLogoPath = @"ImageResources\Edit_ImageResources\Trash_Delete.jpg";
            string redFilterLogoPath = @"ImageResources\FilterImageResources\Red.png";
            string redToColorFilterLogoPath = @"ImageResources\FilterImageResources\RedNegative.png";
            string greenFilterLogoPath = @"ImageResources\FilterImageResources\Green.png";
            string greenToColorFilterLogoPath = @"ImageResources\FilterImageResources\GreenNegative.png";
            string blueFilterLogoPath = @"ImageResources\FilterImageResources\Blue.png";
            string blueToColorFilterLogoPath = @"ImageResources\FilterImageResources\BlueNegative.png";
            string appLogoFilePath = @"ImageResources\LogoImageResources\IntuSoft.ico";
            string exportToolLogoPath = @"ImageResources\Edit_ImageResources\Export_Image_Square.png";//initialsing  the path of image to string.By Ashutosh 20-7-2017

            string ZoomInLogoPath = @"ImageResources\FilterImageResources\ZoomIN.png";
            string ZoomOutLogoPath = @"ImageResources\FilterImageResources\ZoomOut.png";
            string ZoomResetLogoPath = @"ImageResources\FilterImageResources\ZoomReset.png";

            
            if (File.Exists(appLogoFilePath))
                this.Icon = new System.Drawing.Icon(appLogoFilePath, 256, 256);
            if (File.Exists(ellipseToolLogoPath))
                Ellipse.Image = Image.FromFile(ellipseToolLogoPath);
            if (File.Exists(rectangleToolLogoPath))
                Rectangle.Image = Image.FromFile(rectangleToolLogoPath);
            if (File.Exists(pencilToolLogoPath))
                FreeDraw_Btn.Image = Image.FromFile(pencilToolLogoPath);
            if (File.Exists(ZoomInLogoPath))
                zoomIn_btn.Image = Image.FromFile(ZoomInLogoPath);
            if (File.Exists(ZoomOutLogoPath))
                zoomOut_btn.Image = Image.FromFile(ZoomOutLogoPath);
            if (File.Exists(ZoomResetLogoPath))
                zoomReset_btn.Image = Image.FromFile(ZoomResetLogoPath);

            //Open_Btn.Image = Image.FromFile(@"ImageResources\open.bmp");
            if (File.Exists(saveToolLogoPath))
                saveDrawing_btn.Image = Image.FromFile(saveToolLogoPath);
            if (File.Exists(exportToolLogoPath))//check if exportToolLogoPath string exists in file.
                Export_btn.Image = Image.FromFile(exportToolLogoPath);// if string exists , then initalised to Export_btn.Image  . By Ashutosh 20-7-2017
            if (File.Exists(lineToolLogoPath))
                LineDraw_btn.Image = Image.FromFile(lineToolLogoPath);
            if (File.Exists(printToolLogoPath))
                Print_btn.Image = Image.FromFile(printToolLogoPath);
            if (File.Exists(redFilterLogoPath))
                redChannel_btn.Image = redFilter = Image.FromFile(redFilterLogoPath);
            if (File.Exists(greenFilterLogoPath))
                greenChannel_btn.Image = greenFilter = Image.FromFile(greenFilterLogoPath);
            if (File.Exists(blueFilterLogoPath))
                blueChannel_btn.Image = blueFilter = Image.FromFile(blueFilterLogoPath);

            if (File.Exists(redToColorFilterLogoPath))
                redFilterSelected = Image.FromFile(redToColorFilterLogoPath); //Red filter selected image;
            if (File.Exists(greenToColorFilterLogoPath))
                greenFilterSelected = Image.FromFile(greenToColorFilterLogoPath); //Green filter selected Image;
            if (File.Exists(blueToColorFilterLogoPath))
                blueFilterSelected = Image.FromFile(blueToColorFilterLogoPath); //Blue filter selected Image;

            this.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.MaximumSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.MinimumSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            drawArea1 = new DrawArea();
            drawArea1._commentsAddedEvent += drawArea1__commentsAddedEvent;
            drawArea1._currentId += drawArea1__currentId;
            this.userControl11._annotationactiveid += userControl11__annotationactiveid;
            drawArea1._unselectall += drawArea1__unselectall;

            ac = new AnnotationComments();
            Annotation_ImageId = (reportDic["$visitImageIds"] as int[])[0];
            //drawArea1.Parent = Display_pbx;
            drawArea1.SizeMode = PictureBoxSizeMode.Normal;
            drawArea1.Paint += drawArea1_Paint;
            drawArea1.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            drawArea_p.Controls.Add(drawArea1);
            drawArea1.BackColor = Color.Transparent;
            AnnotationImagefileName = (reportDic["$CurrentImageFiles"] as string[])[0];
            Common.CommonMethods c = Common.CommonMethods.CreateInstance();
            Bitmap bm = new Bitmap(10, 10);
            // initialization of the static public image to be used throught out the application by sriram on august 31st 2015
            tempBm = new Bitmap(10, 10);
            pbxBm = new Bitmap(10, 10);
            this.annotationDetails = annotationDetails;
            LoadSaveImage.LoadImage(AnnotationImagefileName, ref bm);
            colorImage = new Image<Bgr, byte>(bm);
            redChannel = colorImage[2].Copy();
            greenChannel = colorImage[1].Copy();
            blueChannel = colorImage[0].Copy();
            // load the image from the file by sriram on august 31st 2015
            LoadSaveImage.LoadImage(AnnotationImagefileName, ref tempBm);
            pbxBm = bm;
            //Display_pbx.Image = pbxBm;
            //Display_pbx.SendToBack();
            drawArea1.Image = pbxBm;
            reported_by = annotationDetails["$ReportedBy"].ToString();
            if (annotationDetails.ContainsKey("$annotationDetails"))
                annotationFiles_lbl.Text = annotationDetails["$annotationDetails"].ToString();
            if (annotationDetails.ContainsKey("$annotationComments"))
                annotationComments_lbl.Text = annotationDetails["$annotationComments"].ToString();
            if (annotationDetails.ContainsKey("$annotationWarningMessage"))
                warningMessage = annotationDetails["$annotationWarningMessage"].ToString();
            if (annotationDetails.ContainsKey("$annotationHeaderMessage"))
                warningHeader = annotationDetails["$annotationHeaderMessage"].ToString();
            //if (annotationDetails.ContainsKey("$ellipseBtnText"))
            //    Ellipse.Text = annotationDetails["$ellipseBtnText"].ToString();
            //if (annotationDetails.ContainsKey("$rectangleBtnText"))
            //    Rectangle.Text = annotationDetails["$rectangleBtnText"].ToString();
            //if (annotationDetails.ContainsKey("$freeDrawBtnText"))
            //    FreeDraw_Btn.Text = annotationDetails["$freeDrawBtnText"].ToString();
            //if (annotationDetails.ContainsKey("$lineDrawBtnText"))
            //    LineDraw_btn.Text = annotationDetails["$lineDrawBtnText"].ToString();
            if (annotationDetails.ContainsKey("$printBtnText"))
                Print_btn.Text = annotationDetails["$printBtnText"].ToString();
            if (annotationDetails.ContainsKey("$saveBtnText"))
                saveDrawing_btn.Text = annotationDetails["$saveBtnText"].ToString();
            if (annotationDetails.ContainsKey("$reportMessageText"))
                deleteMessage = annotationDetails["$reportMessageText"].ToString();
            if (annotationDetails.ContainsKey("$reportHeaderText"))
                deleteHeader = annotationDetails["$reportHeaderText"].ToString();
            if (annotationDetails.ContainsKey("$clearMessageText"))//checks if key $clearMessageText is present in annotationDetails . By Ashutosh 07-09-2017.
                clearAnnotationMessage = annotationDetails["$clearMessageText"].ToString();// if present value associated with key is given to clearAnnotationMessage.By Ashutosh 07-09-2017.
            if (annotationDetails.ContainsKey("$clearHeaderText"))//checks if key $clearHeaderText is present in annotationDetails .By Ashutosh 07-09-2017.
                clearAnnotationHeader = annotationDetails["$clearHeaderText"].ToString();// if present value associated with key is given to clearAnnotationHeader.By Ashutosh 07-09-2017.
            if (annotationDetails.ContainsKey("$annotationMaxCharsText"))
                maxChar_lbl.Text = annotationDetails["$annotationMaxCharsText"].ToString();
            if (annotationDetails.ContainsKey("$filters"))
                filter_lbl.Text = annotationDetails["$filters"].ToString();
            if (annotationDetails.ContainsKey("$noOfToolsAllowedMessage"))
                noOfImagesAllowedMessage = annotationDetails["$noOfToolsAllowedMessage"].ToString();
            if (annotationDetails.ContainsKey("$noOfToolsAllowedHeader"))
                noOfImagesAllowedHeader = annotationDetails["$noOfToolsAllowedHeader"].ToString();
            if (annotationDetails.ContainsKey("$redFilterText"))
                redChannel_btn.Text = annotationDetails["$redFilterText"].ToString();
            if (annotationDetails.ContainsKey("$greenFilterText"))
                greenChannel_btn.Text = annotationDetails["$greenFilterText"].ToString();
            if (annotationDetails.ContainsKey("$blueFilterText"))
                blueChannel_btn.Text = annotationDetails["$blueFilterText"].ToString();
            if (annotationDetails.ContainsKey("$EllipseButtonToolTipText"))
                Ellipse.ToolTipText = annotationDetails["$EllipseButtonToolTipText"].ToString();
            if (annotationDetails.ContainsKey("$RectangleButtonToolTipText"))
                Rectangle.ToolTipText = annotationDetails["$RectangleButtonToolTipText"].ToString();
            if (annotationDetails.ContainsKey("$FreeDrawButtonToolTipText"))
                FreeDraw_Btn.ToolTipText = annotationDetails["$FreeDrawButtonToolTipText"].ToString();
            if (annotationDetails.ContainsKey("$LineDrawButtonToolTipText"))
                LineDraw_btn.ToolTipText = annotationDetails["$LineDrawButtonToolTipText"].ToString();

            if (annotationDetails.ContainsKey("$EllipseButtonText"))
                Ellipse.Text = annotationDetails["$EllipseButtonText"].ToString();
            if (annotationDetails.ContainsKey("$RectangleButtonText"))
                Rectangle.Text = annotationDetails["$RectangleButtonText"].ToString();
            if (annotationDetails.ContainsKey("$FreeDrawButtonText"))
                FreeDraw_Btn.Text = annotationDetails["$FreeDrawButtonText"].ToString();
            if (annotationDetails.ContainsKey("$LineDrawButtonText"))
                LineDraw_btn.Text = annotationDetails["$LineDrawButtonText"].ToString();

            if (annotationDetails.ContainsKey("$SaveAnnotationButtonToolTipText"))
                saveDrawing_btn.ToolTipText = annotationDetails["$SaveAnnotationButtonToolTipText"].ToString();
            if (annotationDetails.ContainsKey("$PrintAnnotationButtonToolTipText"))
                Print_btn.ToolTipText = annotationDetails["$PrintAnnotationButtonToolTipText"].ToString();
            if (annotationDetails.ContainsKey("$ExportAnnotationButtonToolTipText"))
                Export_btn.ToolTipText = annotationDetails["$ExportAnnotationButtonToolTipText"].ToString();
            if (annotationDetails.ContainsKey("$ExportAnnotationButtonText"))
                Export_btn.Text = annotationDetails["$ExportAnnotationButtonText"].ToString();
            if (annotationDetails.ContainsKey("$annotationText"))
                this.Text = annotationDetails["$annotationText"].ToString();
            if (annotationDetails.ContainsKey("$annotationMarkingColor"))
                AnnotationVariables.annotationMarkingColor = annotationDetails["$annotationMarkingColor"].ToString();
            if (annotationDetails.ContainsKey("$cupColor"))
                AnnotationVariables.cupColor = annotationDetails["$cupColor"].ToString();
            if (annotationDetails.ContainsKey("$discColor"))
                AnnotationVariables.discColor = annotationDetails["$discColor"].ToString();
           if(annotationDetails.ContainsKey("$ZoomInText"))
               zoomIn_btn.Text = annotationDetails["$ZoomInText"].ToString();
           if (annotationDetails.ContainsKey("$ZoomOutText"))
               zoomOut_btn.Text = annotationDetails["$ZoomOutText"].ToString();
           if (annotationDetails.ContainsKey("$ZoomResetText"))
               zoomReset_btn.Text = annotationDetails["$ZoomResetText"].ToString();


            DrawObject.LastUsedColor = Color.FromName(AnnotationVariables.annotationMarkingColor);//Added to resolve defect 0001210: Saved CDR image lines colour is not proper in view mode.
            if (_eventHandler == null)//Added by darshan on 25-07-2016 as per NR:0001211 Note no:(0002558)
            {
                _eventHandler = IVLEventHandler.getInstance();
                _eventHandler.Register(_eventHandler.AnnotationButtonsRefresh, new NotificationHandler(annotationButtonsRefresh));
            }
            if (annotationDetails.ContainsKey("$Doctor"))//This if statement has been added to replicate the same funtionality as in CDR section.
            {
                if (!string.IsNullOrEmpty(annotationDetails["$Doctor"].ToString()))
                {
                    dr_operator = annotationDetails["$Doctor"].ToString();
                    Reportename_txt.Text = annotationDetails["$Doctor"].ToString();//assigns the Reporter name to Reportename_txt.
                    Reportename_txt.ForeColor = Color.Black;
                    Font f = Reportename_txt.Font;
                    Font newFont = new Font(f.FontFamily, f.Size, FontStyle.Regular, GraphicsUnit.Point);
                    Reportename_txt.Font = newFont;
                }
                else
                {
                    Reportename_txt.Text = reported_by;//assigns sets default name to Reportename_txt.
                    Font f = Reportename_txt.Font;
                    Font newFont = new Font(f.FontFamily, f.Size, FontStyle.Italic, GraphicsUnit.Point);
                    Reportename_txt.Font = newFont;
                }
            }
            List<Control> controls = GetControls(this).ToList();
            foreach (Control ctrls in controls)
            {
                {
                    {
                        if (Screen.PrimaryScreen.Bounds.Width == 1920)
                        {
                            ctrls.Font = new Font(ctrls.Font.FontFamily.Name, 13f);
                            filter_lbl.Font = new Font(ctrls.Font.FontFamily.Name, 13f, FontStyle.Bold);
                            ZoomText_lbl.Font = new Font(ctrls.Font.FontFamily.Name, 13f, FontStyle.Bold);
                            zoomIn_btn.Font = new Font(ctrls.Font.FontFamily.Name, 9f, FontStyle.Regular);
                            zoomOut_btn.Font = new Font(ctrls.Font.FontFamily.Name, 9f, FontStyle.Regular);
                            zoomReset_btn.Font = new Font(ctrls.Font.FontFamily.Name, 9f, FontStyle.Regular);
                        }
                        else if (Screen.PrimaryScreen.Bounds.Width == 1366)
                        {
                            ctrls.Font = new Font(ctrls.Font.FontFamily.Name, 10f);
                            filter_lbl.Font = new Font(ctrls.Font.FontFamily.Name, 10, FontStyle.Bold);
                        }
                        else
                            ctrls.Font = new Font(ctrls.Font.FontFamily.Name, 9f);

                        if (ctrls is TextBox)
                            ctrls.ForeColor = Color.Black;
                    }
                }
            }

            if (Screen.PrimaryScreen.Bounds.Width == 1920)
            {
                if (pbxBm.Width == 2048 && pbxBm.Height == 1536)// Zoom default for 3mp images
                    defaultZoom = 61f;
                else
                    defaultZoom = 46f;// Zoom default for 6mp images
               
            }
            if (Screen.PrimaryScreen.Bounds.Width == 1366)
            {
                if (pbxBm.Width == 2048 && pbxBm.Height == 1536)// Zoom default for 3mp images
                    defaultZoom = 43f;
                else
                    defaultZoom = 32f;// Zoom default for 6mp images

            }
            zoom = defaultZoom;//Apply the default Zoom
            previousZoom = defaultZoom;

            this.fileTools_tsp.Renderer = new FormToolStripRenderer();
            this.Load += Form1_Load;
            //this.TopMost = true;//has been commented to prevent the custom message box getting background.
        }

        void zoomReset_btn_Click(object sender, EventArgs e)
        {
            zoomVal_tbx.Text =  defaultZoom.ToString();
        }

        void drawArea1_Paint(object sender, PaintEventArgs e)
        {
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
        
        void drawArea1__unselectall()
        {
            this.userControl11.unselectall();
        }

        void userControl11__annotationactiveid(int id,bool isDelete = false)
        {
            drawArea1.GraphSelect(id);
            drawArea1.Refresh();
            if (isDelete)
                DeleteDrawing();
        }


        private void HighlightSelectedButton(ToolStripButton c, ToolStrip p)
        {

            foreach (ToolStripItem item in p.Items)
            {
                if (item == c)
                {
                    //This if statement has been added by Darshan to not change the color of the colorImage_btn button since in view screen color image button is not highlighted.
                    //if (!item.Name.Equals(colorImage_btn.Name))
                    item.BackColor = Color.Blue;
                }
                else
                    item.BackColor = SystemColors.ButtonFace;
            }
        }

        private void RefreshToolStripButton(ToolStrip p)
        {
            foreach (ToolStripItem item in p.Items)
            {
                item.BackColor = SystemColors.ButtonFace;
            }
        }

        void drawArea1__currentId(int id)
        {
            this.userControl11.activeId = id;
            comment_Id = id;
            this.userControl11.Highlight_control();
        }

        void drawArea1__commentsAddedEvent(AnnotationText c, EventArgs e)
        {
            if (!drawArea1.ListGraphics.currentType.Equals("Annotation.DrawLine"))
            {
                temp = c;
                this.userControl11.activeId = drawArea1.ListGraphics.Count;
                comment_Id = c.ID;
                //the below code has been added by Darshan to solve defect no 0000510: Duplicate numbering in comments if pressed on control key.
                if (drawArea1.ListGraphics.ids.Contains(comment_Id))
                {
                    if (this.userControl11.class11.Controls.Count != drawArea1.ListGraphics.Count)
                        this.userControl11.addControl2FLP(c.ID, "");
                }
            }
        }

        public void Refresh_DrawArea()
        {
            //drawArea1.Parent = Display_pbx;
            Common.CommonMethods c = Common.CommonMethods.CreateInstance();
            Bitmap bm = new Bitmap(10, 10);
            tempBm = new Bitmap(10, 10);
            LoadSaveImage.LoadImage(AnnotationImagefileName, ref bm);
            LoadSaveImage.LoadImage(AnnotationImagefileName, ref tempBm);
            //Display_pbx.Image = bm;
            drawArea1.Refresh();
            ResizeDrawArea();
            drawArea1.Initialize(this);
        }

        void Form1_Load(object sender, EventArgs e)
        {
            ResizeDrawArea();
            drawArea1.Initialize(this);
            showExisitingAnnotation();
            calculateNoOfCDR();
            //GlaucomaTool.pbxHeight = Display_pbx.Height;
            //GlaucomaTool.pbxWidth = Display_pbx.Width;
            //pbxHeight = Display_pbx.Height;
            //pbxWidth = Display_pbx.Width;
            GlaucomaTool.pbxHeight = drawArea1.Height;
            GlaucomaTool.pbxWidth = drawArea1.Width;
            pbxHeight = pbxBm.Height;
            pbxWidth = pbxBm.Width;
            zoomVal_tbx.Text = defaultZoom.ToString();
            zoomVal_tbx.KeyDown += zoomVal_tbx_KeyDown;
            zoomVal_tbx.KeyPress += zoomVal_tbx_KeyPress;
            zoomVal_tbx.ShortcutsEnabled = false;// This added fix defect 0001882 to prevent copy paste or cut any short cuts on the textbox has been disabled
            Args arg = new Args();
            arg["Print"] = false;
            arg["Save"] = false;
            arg["Export"] = false;//set to false when Form1 is loaded.By Ashutosh 20-7-2017
            _eventHandler.Notify(_eventHandler.AnnotationButtonsRefresh, arg);
        }

        void zoomVal_tbx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == (Keys.Control | Keys.Back))
                e.Handled = true;
            else
            {
            }
        }

        void zoomVal_tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            //This is done handle defect 0001881 to avoid crash using control +backspace keys
            if (e.KeyChar == 127)
            {
                //if not last word
                e.Handled = true;
            }
            //
            else if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
            {
            }
            else
                e.Handled = true;
        }



        #region Mouse Down Events for pbx
        void pbx_MouseDown(object sender, MouseEventArgs e)
        {
            panning = true;
            this.Cursor = Cursors.Hand;
            startingPoint = new Point(e.Location.X - movingPoint.X,
                                      e.Location.Y - movingPoint.Y);
        }
        #endregion


        #region Mouse Up Events for pbx
        void pbx_MouseUp(object sender, MouseEventArgs e)
        {
            panning = false;
            this.Cursor = Cursors.Default;
        }

        #endregion


        #region Mouse Move Events for pbx
        void pbx_MouseMove(object sender, MouseEventArgs e)
        {
            if (panning)
            {
                movingPoint = new Point(e.Location.X - startingPoint.X,
                                        e.Location.Y - startingPoint.Y);
                //Display_pbx.Invalidate();
                drawArea1.Invalidate();
            }
        }
        #endregion

        Point ImagePoint = new Point(-1,-1);
        Point WidthHeightPoint = new Point(-1,-1);

        void ApplyZoom(float ZoomRatioVal)
        {
            if (drawArea2 != null)
            {
                drawArea1 = null;
                drawArea1 = drawArea2;
            }
            else if (drawArea1.ListGraphics.Count > 0)
            {
                drawArea2 = drawArea1;
                drawArea2.ListGraphics = drawArea1.ListGraphics;
            }
            float ZoomVal = Zoom / 100;
            int width = (int)((float)pbxBm.Width *ZoomVal);
            int height = (int)((float)pbxBm.Height *ZoomVal );
            pbxWidth = drawArea1.Width = width;
            pbxHeight = drawArea1.Height = height;
            
            Bitmap bm = new Bitmap(width, height);
            Graphics graph = Graphics.FromImage(bm);
            graph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graph.DrawImage(tempBm, new Rectangle(0, 0, bm.Width, bm.Height));
            graph.Dispose();
            if (Zoom == 32)
                ZoomRatioVal = 1;
            DrawExistingGraphics(ZoomRatioVal);
            drawArea1.Image = bm;
            drawArea1.Refresh();
        }

        void DrawExistingGraphics(float ZoomValRatio)
        {
            for (int i = 0; i < drawArea1.ListGraphics.Count; i++)
            {
                Graphics g = drawArea1.CreateGraphics();
                if (drawArea1.ListGraphics[i] is DrawEllipse)
                {
                    DrawEllipse dellipse = drawArea1.ListGraphics[i] as DrawEllipse;

                    Rectangle r = dellipse.rectangle;
                    float newX = (float)r.X * ZoomValRatio;
                    float newY = (float)r.Y * ZoomValRatio;

                    float newWidth = (float)r.Width *ZoomValRatio;
                    float newHeight = (float)r.Height * ZoomValRatio;



                    dellipse.rectangle = new System.Drawing.Rectangle(Convert.ToInt32(newX), Convert.ToInt32(newY), Convert.ToInt32(newWidth), Convert.ToInt32(newHeight));
                    dellipse.Draw(g);
                }
                else if (drawArea1.ListGraphics[i] is DrawRectangle)
                {
                    DrawRectangle dRect = drawArea1.ListGraphics[i] as DrawRectangle;
                    Rectangle r = dRect.rectangle;
                    float newX = (float)r.X * ZoomValRatio;
                    float newY = (float)r.Y * ZoomValRatio;

                    float newWidth = (float)r.Width * ZoomValRatio;
                    float newHeight = (float)r.Height * ZoomValRatio;

                    dRect.rectangle = new System.Drawing.Rectangle(Convert.ToInt32(newX), Convert.ToInt32(newY), Convert.ToInt32(newWidth), Convert.ToInt32(newHeight));
                    dRect.Draw(g);
                }
                else if (drawArea1.ListGraphics[i] is DrawPolygon)
                {
                    DrawPolygon dRect = drawArea1.ListGraphics[i] as DrawPolygon;
                    List<object> ObjArr = dRect.pointArray.ToArray().ToList();
                    List<Point> pArr = new List<Point>();

                    for (int j = 0; j < dRect.pointArray.Count; j++)
                    {
                        Point p = (Point)ObjArr[j];
                        PointF floatP = new PointF();

                        floatP.X = (float)p.X * ZoomValRatio;
                        floatP.Y = (float)p.Y * ZoomValRatio;
                        pArr.Add(Point.Round(floatP));
                    }
                    dRect.pointArray = new System.Collections.ArrayList();
                    dRect.pointArray.AddRange(pArr);
                    dRect.Draw(g);
                }
                else if (drawArea1.ListGraphics[i] is DrawLine)
                {
                    DrawLine dline = drawArea1.ListGraphics[i] as DrawLine;

                    Point StartPoint = dline.startPoint;
                    Point EndPoint = dline.endPoint;

                    float newStartX = (float)StartPoint.X * ZoomValRatio;
                    float newStartY = (float)StartPoint.Y * ZoomValRatio;

                    float newEndX = (float)EndPoint.X * ZoomValRatio;
                    float newEndY = (float)EndPoint.Y * ZoomValRatio;
                    dline.startPoint = Point.Round(new PointF(newStartX, newStartY));
                    dline.endPoint = Point.Round(new PointF(newEndX, newEndY));
                    dline.Draw(g);
                }
            }
        }

        /// <summary>
        /// to  unselect the graphics and the corresponding comment box 
        /// </summary>
        private void DrawAreaUnselectAll()
        {
            drawArea1.ListGraphics.UnselectAll();
            drawArea1.unselectalltext();
            drawArea1.Refresh();
        }
        private void Ellipse_Click(object sender, EventArgs e)
        {
            if (!isControl)
            {
                if (isNumberOfDrawObjectsWithInLimit())
                {
                    DrawAreaUnselectAll();
                    HighlightSelectedButton(Ellipse, this.drawingTools_ts);
                    CommandEllipse();
                }
            }
        }

        public bool isNumberOfDrawObjectsWithInLimit()
        {
            if (drawArea1.graphicsList.ids.Length < 5)
            {
                return true;
            }
            else
            {
                CustomMessageBox.Show(noOfImagesAllowedMessage, noOfImagesAllowedHeader, CustomMessageBoxButtons.OK, CustomMessageBoxIcon.Warning);
                return false;
            }
        }

        private void Rectangle_Click(object sender, EventArgs e)
        {
            if (!isControl)
            {
                DrawAreaUnselectAll();
                if (isNumberOfDrawObjectsWithInLimit())
                {
                    HighlightSelectedButton(Rectangle, this.drawingTools_ts);
                    CommandRectangle();
                }
            }
        }

        private void FreeDraw_Btn_Click(object sender, EventArgs e)
        {
            if (!isControl)
            {
                DrawAreaUnselectAll();
                if (isNumberOfDrawObjectsWithInLimit())
                {
                    HighlightSelectedButton(FreeDraw_Btn, this.drawingTools_ts);
                    CommandPolygon();
                }
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

        private void annotationButtonsRefresh(string s, Args arg)//Added by darshan on 25-07-2016 to implement as per NR:0001211 Note no:(0002558)
        {
            if (arg.ContainsKey("Print"))
            {
                Print_btn.Enabled = (bool)arg["Print"];
            }
            if (arg.ContainsKey("Save"))
            {
                saveDrawing_btn.Enabled = (bool)arg["Save"];
            }
            if (arg.ContainsKey("Export"))// checks if export is present in dictionary
            {
                Export_btn.Enabled = (bool)arg["Export"];// if present bool state of "Export" set to Export_btn.Enabled .By Ashutosh 20-7-2017
            }
            if (arg.ContainsKey("IsViewed"))
            {
                isview = (bool)arg["IsViewed"];
            }
        }

        /// <summary>
        /// Set Polygon draw tool
        /// </summary>
        private void CommandPolygon()
        {
            drawArea1.ActiveTool = DrawArea.DrawToolType.Polygon;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (!Reportename_txt.Focused)
                DeleteDrawing(); 
            }
        }
        private void DeleteDrawing()
        {
                drawArea1.ListGraphics.isDelete = true;
                if (drawArea1.ListGraphics.SelectionCount != 0 && drawArea1.ListGraphics.SelectionCount == 1)
                {
                    drawArea1.ListGraphics.DeleteSelection();
                    if (!drawArea1.ListGraphics.currentType.Equals("Annotation.DrawLine"))
                    {
                        this.userControl11.removeControl(comment_Id);
                        drawArea1.Refresh();
                        int[] id = drawArea1.ListGraphics.ids.Reverse().ToArray();
                        //for (int i = 0; i < drawArea1.ListGraphics.ids.Length; i++)
                        this.userControl11.Update_Labels(id);
                    }
                    drawArea1.Refresh();
                }
        }
        private void LineDraw_btn_Click(object sender, EventArgs e)
        {
            if (!isControl)
            {
                DrawAreaUnselectAll();
                HighlightSelectedButton(LineDraw_btn, this.drawingTools_ts);
                CommandLine();
            }
        }

        public void SaveAnnotation(Args e)
        {
            if (drawArea1.ListGraphics.Count != 0)
            {
                int j = 0;
                String AnnotationXml = "";
                string comments = "";
                annotationtype = drawArea1.ActiveTool.ToString();
                string name = DateTime.Now.ToString("ddMMyyyyhhmmss");
                string newFileName = name + "_Graphics.xml";
                SvgDocument svgDocument = new SvgDocument();
                SvgDescription reportedByDescription = new SvgDescription();
                SvgDescription cdrcommentsDescription = new SvgDescription();
                AnnotationXMLProperties annoprop = new AnnotationXMLProperties();
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
                annoprop.Xmlcommentsproperties = userControl11.Get_annotationComments();
                for (int i = 0; i < drawArea1.graphicsList.Count; i++)
                {
                    DrawObject dObject = drawArea1.graphicsList[i];
                    annoprop.Shapes.Add(dObject.Shape);
                    annoprop.isCupProperties.Add(false);
                }
                svgDocument.Children.Add(reportedByDescription);
                for (int i = annoprop.Shapes.Count - 1; i >= 0; --i)
                {
                    int id = i + 1;

                    if (annoprop.Shapes[i]._shapeType.ToString() == "Annotation_Rectangle")
                    {
                        SvgRectangle rect = new SvgRectangle();
                        rect.Height = annoprop.Shapes[i].Height;
                        rect.Width = annoprop.Shapes[i].Width;
                        rect.X = annoprop.Shapes[i].StartPoint.X;
                        rect.Y = annoprop.Shapes[i].StartPoint.Y;
                        SvgDescription desc = new SvgDescription();
                        desc.Content = annoprop.Xmlcommentsproperties[j].Comments;
                        j = j + 1;
                        rect.Children.Add(desc);
                        svgDocument.Children.Add(rect);
                    }
                    else
                        if (annoprop.Shapes[i]._shapeType.ToString() == "Annotation_Ellipse")
                        {
                            SvgEllipse ellipse = new SvgEllipse();
                            int centerX = (annoprop.Shapes[i].Width / 2) + annoprop.Shapes[i].StartPoint.X;
                            int centerY = (annoprop.Shapes[i].Height / 2) + annoprop.Shapes[i].StartPoint.Y;
                            int radiusX = centerX - annoprop.Shapes[i].StartPoint.X;
                            int radiusY = centerY - annoprop.Shapes[i].StartPoint.Y;
                            ellipse.CenterX = centerX;
                            ellipse.CenterY = centerY;
                            ellipse.RadiusX = radiusX;
                            ellipse.RadiusY = radiusY;
                            //ellipse.ID = id.ToString();
                            SvgDescription desc = new SvgDescription();
                            desc.Content = annoprop.Xmlcommentsproperties[j].Comments;
                            j = j + 1;
                            ellipse.Children.Add(desc);
                            svgDocument.Children.Add(ellipse);
                        }
                        else
                            if (annoprop.Shapes[i]._shapeType.ToString() == "Annotation_Line")
                            {
                                SvgLine line = new SvgLine();
                                line.StartX = annoprop.Shapes[i].StartPoint.X;
                                line.StartY = annoprop.Shapes[i].StartPoint.Y;
                                line.EndY = annoprop.Shapes[i].EndPoint.Y;
                                line.EndX = annoprop.Shapes[i].EndPoint.X;
                                svgDocument.Children.Add(line);
                            }
                            else if (annoprop.Shapes[i]._shapeType.ToString() == "Annotation_Polygon")
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
                                    if (drawArea1.iscup_List[i])
                                        polygon.ID = "cup";
                                    else
                                        polygon.ID = "disc";
                                }
                                polygon.Points = pointCollection;
                                if (!drawArea1.iscup_List.Contains(true))
                                {
                                    SvgDescription desc = new SvgDescription();
                                    desc.Content = annoprop.Xmlcommentsproperties[j].Comments;
                                    j = j + 1;
                                    //desc.ID = annoprop.Xmlcommentsproperties[i].ID.ToString();
                                    polygon.Children.Add(desc);
                                }
                                svgDocument.Children.Add(polygon);
                            }
                }
                if (annoprop.isCupProperties.Contains(true))
                {
                    //annoprop.CDRComments = comments_tbx.Text;
                    cdrcommentsDescription.ID = "CDRComments";
                    cdrcommentsDescription.Content = CDRcomments;
                    svgDocument.Children.Add(cdrcommentsDescription);
                }
                AnnotationXml = svgDocument.GetXML();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Regex.Replace(AnnotationXml, "<!DOCTYPE.+?>", string.Empty));
                AnnotationXml = doc.InnerXml;
                doc.LoadXml(Regex.Replace(AnnotationXml, "<svg.+?>", "<svg>"));
                AnnotationXml = doc.InnerXml;
                //Commented lines are the old implementation has been retained for reference
                //AnnotationXml = sw.ToString();
                Dictionary<string, object> annoval = new Dictionary<string, object>();
                e["xml"] = AnnotationXml;
                e["dateTime"] = DateTime.Now;
                e["Comments"] = comments;
                e["imageid"] = Annotation_ImageId;
                e["annotationID"] = annotationID;
                if (annotationSavedEvent != null)
                    this.Cursor = Cursors.WaitCursor;
                annotationSavedEvent(e);
                this.Cursor = Cursors.Default;
                Args arg = new Args();
                arg["Print"] = false;
                arg["Save"] = false;
                arg["Export"] = false;//set to false.By Ashutosh 20-7-2017

                _eventHandler.Notify(_eventHandler.AnnotationButtonsRefresh, arg);
            }
        }

        public void saveButtonFunctionality()
        {
            Args arg = new Args();
            arg["isAnnotationUpdate"] = false;
            SaveAnnotation(arg);
            drawArea1.ListGraphics.Clear();
            this.userControl11.Release_Flp();
            drawArea1.Refresh();
            RefreshToolStripButton(this.drawingTools_ts);
            RefreshToolStripButton(this.filter_ts);
            //Display_pbx.Image = colorImage.ToBitmap();
            //drawArea1.Image = colorImage.ToBitmap();
            // below method is being invoked since the Reportename_txt was not getting refresh after saving.
            ReportedByTextFontChange();
            annotationComments_lbl.Focus();
            showExisitingAnnotation();
            this.Cursor = Cursors.Default;
        }

        private void saveDrawing_btn_Click(object sender, EventArgs e)
        {
            HighlightSelectedButton(saveDrawing_btn, this.drawingTools_ts);
            saveButtonFunctionality();
        }

        public void DrawComments(AnnotationComments annotationcomments)
        {
            Panel annotationPanel = new Panel();
            annotationPanel.BackColor = SystemColors.ControlDarkDark;
            AnnotationText c = new AnnotationText();
            c.Size = new Size(229, 194);
            annotationPanel.Size = new Size(229, 194);
            c.Dock = DockStyle.Fill;
            annotationPanel.Location = new Point(annotationcomments.X, annotationcomments.Y);
            annotationPanel.Show();
        }

        public void checkforCDR(string AnnotationXml)
        {
            byte[] bytes = null;
            AnnotationXMLProperties anno = null;
            //Below code in the try block has been retained to handle the situation when the annotation xml data is getting readed from old annotationxml format(binary format).
            try
            {
                bytes = Convert.FromBase64String(AnnotationXml);
                using (MemoryStream stream = new MemoryStream(bytes))
                {
                    IFormatter formatter = new BinaryFormatter();
                    anno = (AnnotationXMLProperties)formatter.Deserialize(stream);
                }
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
                            }
                            else
                            {
                                anno.isCupProperties.Add(false);
                            }
                        }
                    }
                }
            }
            isCDRvalue = false;
            for (int i = 0; i < anno.isCupProperties.Count; i++)
            {
                if (anno.isCupProperties[i])
                    isCDRvalue = anno.isCupProperties[i];
            }
        }

        public void viewannotation(string AnnotationXml)
        {
            byte[] bytes = null;
            AnnotationXMLProperties anno = null;
            AnnotationComments comment = null;
            int commentId = 1;
            //Below code in the try block has been retained to handle the situation when the annotation xml data is getting readed from old annotationxml format(binary format).
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
                            isSvgFormat = false;
                        }
                    }
                }
                catch (Exception)
                {
                    svgDoc = SvgDocument.FromSvg<SvgDocument>(AnnotationXml);
                    SvgElementCollection svg = svgDoc.Children;
                    anno = new AnnotationXMLProperties();
                    //SvgElementCollection sg = new SvgElementCollection();
                    for (int i = 0; i < svg.Count; i++)
                    {
                        Shape s = null;
                        if (svg[i] is Svg.SvgRectangle)
                        {
                            SvgRectangle rect = svg[i] as SvgRectangle;
                            comment = new AnnotationComments();
                            s = new Shape();
                            s._shapeType = ShapeType.Annotation_Rectangle;
                            anno.isCupProperties.Add(false);
                            s.StartPoint = new Point(Convert.ToInt32(rect.X), Convert.ToInt32(rect.Y));
                            s.Width = Convert.ToInt32(rect.Width);
                            s.Height = Convert.ToInt32(rect.Height);
                            if (rect.HasChildren())
                            {
                                SvgDescription descr = rect.Children[0] as SvgDescription;
                                comment.ID = commentId;
                                comment.Comments = descr.Content;
                            }
                            anno.Shapes.Add(s);
                            anno.Xmlcommentsproperties.Add(comment);
                            commentId = commentId + 1;
                        }
                        else
                            if (svg[i] is Svg.SvgEllipse)
                            {
                                SvgEllipse ellipse = svg[i] as SvgEllipse;
                                comment = new AnnotationComments();
                                s = new Shape();
                                s._shapeType = ShapeType.Annotation_Ellipse;
                                anno.isCupProperties.Add(false);
                                s.Height = Convert.ToInt32(ellipse.RadiusY) * 2;
                                s.Width = Convert.ToInt32(ellipse.RadiusX * 2);
                                s.StartPoint.X = Convert.ToInt32(ellipse.CenterX) - Convert.ToInt32(ellipse.RadiusX);
                                s.StartPoint.Y = Convert.ToInt32(ellipse.CenterY) - Convert.ToInt32(ellipse.RadiusY);
                                if (ellipse.HasChildren())
                                {
                                    SvgDescription descr = ellipse.Children[0] as SvgDescription;
                                    comment.ID = commentId;
                                    comment.Comments = descr.Content;
                                }
                                anno.Shapes.Add(s);
                                anno.Xmlcommentsproperties.Add(comment);
                                commentId = commentId + 1;
                            }
                            else if (svg[i] is Svg.SvgLine)
                            {
                                SvgLine svgLine = svg[i] as SvgLine;
                                s = new Shape();
                                s._shapeType = ShapeType.Annotation_Line;
                                anno.isCupProperties.Add(false);
                                s.StartPoint.X = Convert.ToInt32(svgLine.StartX);
                                s.StartPoint.Y = Convert.ToInt32(svgLine.StartY);
                                s.EndPoint.X = Convert.ToInt32(svgLine.EndX);
                                s.EndPoint.Y = Convert.ToInt32(svgLine.EndY);
                                anno.Shapes.Add(s);
                            }
                            else
                                if (svg[i] is Svg.SvgPolygon)
                                {
                                    SvgPolygon svgPolygon = svg[i] as SvgPolygon;
                                    comment = new AnnotationComments();
                                    s = new Shape();
                                    s._shapeType = ShapeType.Annotation_Polygon;
                                    if (svgPolygon.ID == "cup")
                                    {
                                        anno.isCupProperties.Add(true);
                                    }
                                    else if (svgPolygon.ID == "disc")
                                    {
                                        anno.isCupProperties.Add(false);
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
                                    if (svgPolygon.HasChildren())
                                    {
                                        if (svgPolygon.ID != "cup" && svgPolygon.ID != "disc")
                                        {
                                            SvgDescription descr = svgPolygon.Children[0] as SvgDescription;
                                            comment.ID = commentId;
                                            comment.Comments = descr.Content;
                                            anno.Xmlcommentsproperties.Add(comment);
                                            commentId = commentId + 1;
                                        }
                                    }
                                }
                                else if (svg[i] is Svg.SvgDescription)
                                {
                                    SvgDescription description = svg[i] as SvgDescription;
                                    if (description.ID == "Reported By")
                                        anno.ReportedBy = description.Content;
                                    else if (description.ID == "CDRComments")
                                    {
                                        CDRcomments = description.Content;
                                        comment.ID = commentId;
                                        comment.Comments = CDRcomments;
                                        anno.Xmlcommentsproperties.Add(comment);
                                        commentId = commentId + 1;
                                    }
                                }
                    }
                    anno.Shapes.Reverse();
                    anno.isCupProperties.Reverse();
                    isSvgFormat = true;
                }
            }
            drawArea1.iscup_List = anno.isCupProperties;
            drawArea1.comments = anno.Xmlcommentsproperties;
            #region Code to move the binary formatting serialization to existing serialization of graphics
            if (anno.Shapes == null)// == null)// Check if shapes list when saving of graphics in the older method
            {
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
                    else if (dObject is DrawEllipse)
                    {
                        DrawEllipse dEllipse = dObject as DrawEllipse;
                        s._shapeType = ShapeType.Annotation_Ellipse;
                        s.StartPoint = new Point(dEllipse.rectangle.X, dEllipse.rectangle.Y);
                        s.Width = dEllipse.rectangle.Width;
                        s.Height = dEllipse.rectangle.Height;
                    }
                    else if (dObject is DrawRectangle)
                    {
                        DrawRectangle dRectangle = dObject as DrawRectangle;
                        s._shapeType = ShapeType.Annotation_Rectangle;

                        s.StartPoint = new Point(dRectangle.rectangle.X, dRectangle.rectangle.Y);
                        s.Width = dRectangle.rectangle.Width;
                        s.Height = dRectangle.rectangle.Height;
                    }
                    else if (dObject is DrawLine)
                    {
                        DrawLine dLine = dObject as DrawLine;
                        s.StartPoint = dLine.startPoint;
                        s.EndPoint = dLine.endPoint;
                        s._shapeType = ShapeType.Annotation_Line;
                    }


                    anno.Shapes.Add(s);
                }
            }
            #endregion
            //if (anno.Shapes != null && anno.Shapes.Count > 0)
            {
                //This for loop has been added by Darshan on 27-10-2015 to draw existing graph to the drawarea.
                for (int i = anno.Shapes.Count - 1; i >= 0; --i)
                {
                    Shape s = anno.Shapes[i];
                    if (s._shapeType == ShapeType.Annotation_Ellipse)
                    {
                        ToolEllipse t = new ToolEllipse();
                        t.AddExistingObject(drawArea1, s);
                    }
                    else
                        if (s._shapeType == ShapeType.Annotation_Rectangle)
                        {
                            ToolRectangle t = new ToolRectangle();
                            t.AddExistingObject(drawArea1, s);
                        }
                        else
                            if (s._shapeType == ShapeType.Annotation_Line)
                            {
                                ToolLine t = new ToolLine();
                                t.AddExistingObject(drawArea1, s);
                            }
                            else
                                if (s._shapeType == ShapeType.Annotation_Polygon)
                                {
                                    ToolPolygon t = new ToolPolygon();
                                    t.AddExistingObject(drawArea1, s);
                                }
                }
            }
            foreach (AnnotationComments item in anno.Xmlcommentsproperties)
            {
                this.userControl11.addControl2FLP(item.ID, item.Comments);
            }
            for (int i = 0; i < drawArea1.graphicsList.Count; i++)
            {
                if (drawArea1.iscup_List[i])
                    isCDRvalue = drawArea1.iscup_List[i];
            }
            if (!string.IsNullOrEmpty(anno.CDRComments))
                CDRcomments = anno.CDRComments;
            // drawArea1.Refresh();
            Args e = new Args();
            //This if statement is added by Darshan on 27-10-2015 to save the shape for  the older graph.
            if (!isSvgFormat || isGraphicsListPresent)
            {
                e["isAnnotationUpdate"] = true;
                SaveAnnotation(e);
                isGraphicsListPresent = false;
                isSvgFormat = true;
            }
            //This code has been added by darshan to display the saved ReportedBy name.
            if (!string.IsNullOrEmpty(anno.ReportedBy))
            {
                Reportename_txt.Text = anno.ReportedBy;
            }
            else
            {
                Reportename_txt.Text = reported_by;

            }
            Reportename_txt.Enabled = false;
            ApplyZoom(zoom/previousZoom);
            //zoomVal_tbx.Text = defaultZoom.ToString();
        }

        public void showExisitingAnnotation()
        {
            AnnotationsFiles_dgv.ForeColor = Color.Black;
            List<string> date = new List<string>();
            List<string> time = new List<string>();
            List<eye_fundus_image_annotation> list = new List<eye_fundus_image_annotation>();
            //The below code has been added to filter the CDR reports from annotation reports and to show only the annotation report of the image by kishore on 18 September 2017.
            for (int i = 0; i < NewDataVariables.Active_Obs.eye_fundus_image_annotations.Count; i++)
            {
                if (!NewDataVariables.Active_Obs.eye_fundus_image_annotations.ToList()[i].cdrPresent && NewDataVariables.Active_Obs.eye_fundus_image_annotations.ToList()[i].voided == false)
                {
                    list.Add(NewDataVariables.Active_Obs.eye_fundus_image_annotations.ToList()[i]);
                }
            }
            AnnotationsFiles_dgv.DataSource = list.ToDataTable();
            AnnotationsFiles_dgv.AllowUserToAddRows = false;
            AnnotationsFiles_dgv.RowHeadersVisible = false;

            AnnotationsFiles_dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            if (!AnnotationsFiles_dgv.Columns.Contains(headers[0]) && !AnnotationsFiles_dgv.Columns.Contains(headers[1]) && !AnnotationsFiles_dgv.Columns.Contains(headers[2]) && !AnnotationsFiles_dgv.Columns.Contains(headers[3]))
            {
                var col3 = new DataGridViewTextBoxColumn();
                var col4 = new DataGridViewTextBoxColumn();
                DataGridViewLinkColumn col5 = new DataGridViewLinkColumn();
                DataGridViewImageColumn col6 = new DataGridViewImageColumn();
                col3.HeaderText = headers[0];
                if (Screen.PrimaryScreen.Bounds.Width != 1920)
                {
                    col3.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else
                {
                    col3.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                col3.Name = headers[0];
                col4.HeaderText = headers[1];
                if (Screen.PrimaryScreen.Bounds.Width != 1920)
                {
                    col4.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else
                {
                    col4.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                col4.Name = headers[1];
                col5.HeaderText = headers[2];
                col5.Name = headers[2];
                col5.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col6.HeaderText = headers[3];
                col6.Name = headers[3];
                col6.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (File.Exists(deleteLogoPath))
                    col6.Image = Image.FromFile(deleteLogoPath);
                {
                    AnnotationsFiles_dgv.Columns.AddRange(new DataGridViewColumn[] { col3, col4, col5, col6 });
                }

                for (int i = 0; i < AnnotationsFiles_dgv.Columns.Count; i++)
                {
                    if (AnnotationsFiles_dgv.Columns[i].Name.Equals(headers[0]) || AnnotationsFiles_dgv.Columns[i].Name.Equals(headers[1]) || AnnotationsFiles_dgv.Columns[i].Name.Equals(headers[2]) || AnnotationsFiles_dgv.Columns[i].Name.Equals(headers[3]))
                    {
                        AnnotationsFiles_dgv.Columns[i].Visible = true;
                    }
                    else
                        AnnotationsFiles_dgv.Columns[i].Visible = false;
                }
            }
            foreach (DataGridViewRow item in AnnotationsFiles_dgv.Rows)
            {
                DataGridViewLinkCell linCell = new DataGridViewLinkCell();
                linCell.Value = headers[2].ToLower();
                //col5.Text = linCell;
                item.Cells[headers[2]] = linCell;
                item.Cells[headers[2]].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            for (int i = 0; i < list.Count; i++)
            {
                AnnotationsFiles_dgv.Rows[i].Cells[headers[0]].Value = list[i].createdDate.ToString("dd-MMM-yyyy");
                //This code has been added by Darshan on 14-08-2015 6:36 PM to solve Defect no 0000553: Time settings are not reflecting correctly.
                if (is24hourformat)
                {
                    AnnotationsFiles_dgv.Rows[i].Cells[headers[1]].Value = list[i].createdDate.ToString(" HH:mm ");
                }
                else
                {
                    AnnotationsFiles_dgv.Rows[i].Cells[headers[1]].Value = list[i].createdDate.ToString("hh:mm tt");
                }
            }
            if (Screen.PrimaryScreen.Bounds.Width == 1920)
            {
                AnnotationsFiles_dgv.Columns[headers[0]].Width = 140;
                AnnotationsFiles_dgv.Columns[headers[1]].Width = 140;
                AnnotationsFiles_dgv.Columns[headers[2]].Width = 75;
                AnnotationsFiles_dgv.Columns[headers[3]].Width = 78;
                foreach (DataGridViewColumn c in AnnotationsFiles_dgv.Columns)
                {
                    c.DefaultCellStyle.Font = new Font("Tahoma", 13.5F, GraphicsUnit.Pixel);
                }
            }
            else
                if (Screen.PrimaryScreen.Bounds.Width == 1366)
                {
                    AnnotationsFiles_dgv.Columns[headers[0]].Width = 80;
                    AnnotationsFiles_dgv.Columns[headers[1]].Width = 80;
                    AnnotationsFiles_dgv.Columns[headers[2]].Width = 70;
                    AnnotationsFiles_dgv.Columns[headers[3]].Width = 70;
                    foreach (DataGridViewColumn c in AnnotationsFiles_dgv.Columns)
                    {
                        c.DefaultCellStyle.Font = new Font("Tahoma", 11.0F, GraphicsUnit.Pixel);
                    }
                }
                else
                    if (Screen.PrimaryScreen.Bounds.Width == 1280)
                    {
                        AnnotationsFiles_dgv.Columns[headers[0]].Width = 75;
                        AnnotationsFiles_dgv.Columns[headers[1]].Width = 75;
                        AnnotationsFiles_dgv.Columns[headers[2]].Width = 65;
                        AnnotationsFiles_dgv.Columns[headers[3]].Width = 63;
                        foreach (DataGridViewColumn c in AnnotationsFiles_dgv.Columns)
                        {
                            c.DefaultCellStyle.Font = new Font("Tahoma", 11.0F, GraphicsUnit.Pixel);
                        }
                    }
            AnnotationsFiles_dgv.Refresh();
        }

        private void AnnotationsFiles_dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {


                AnnotationsFiles_dgv.Focus();
                if (e.RowIndex < 0)
                {
                    AnnotationsFiles_dgv.ClearSelection();
                    return;
                }
                if (e.ColumnIndex < 0)
                    return;
                //This code has been modified to solve defect no 0000522: The deleted annotation report is coming up again,
                if (AnnotationsFiles_dgv.Columns[e.ColumnIndex].Name == headers[3])
                {
                    //delete_Annotation();
                    DialogResult res = CustomMessageBox.Show(deleteMessage, deleteHeader, CustomMessageBoxButtons.YesNo, CustomMessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                    {
                        int id = Convert.ToInt32(AnnotationsFiles_dgv.Rows[e.RowIndex].Cells["eyeFundusImageAnnotationId"].Value);
                        NewDataVariables.Active_Annotation = NewDataVariables.Active_Obs.eye_fundus_image_annotations.ToList().Find(x => x.eyeFundusImageAnnotationId == id);
                        Args arg = new Args();//Added by darshan on 25-07-2016 as per NR:0001211 Note no:(0002558)
                        //This below if statement was added by Darshan on 21-08-2015 to solve Defect no 0000591: when deleted the saved annotation,the current annotation performing graph is getting deleted.
                        if (isview)
                        {
                            drawArea1.ListGraphics.DeleteAll();
                            this.userControl11.Release_Flp();
                            drawArea1.Enabled = true;
                            drawArea1.Refresh();
                            //This below code has been added to enable the controls in annotation grid view after deletion.
                            drawingTools_ts.Enabled = true;
                            if (isview)
                            {
                                RefreshToolStripButton(this.filter_ts);
                                //Display_pbx.Image = colorImage.ToBitmap();
                                //drawArea1.Image = colorImage.ToBitmap();
                            }
                            Ellipse.Enabled = true;
                            Rectangle.Enabled = true;
                            FreeDraw_Btn.Enabled = true;
                            LineDraw_btn.Enabled = true;
                            //saveDrawing_btn.Enabled = true;
                            ReportedByTextFontChange();
                            Reportename_txt.Enabled = true;
                            arg["Print"] = false;
                            arg["Save"] = false;
                            arg["Export"] = false;//set to false when point are deleted.By Ashutosh 20-7-2017

                        }
                        else
                        {
                            if (drawArea1.ListGraphics.Count > 0)
                            {
                                arg["Print"] = true;//This code has been added to resolve the issue 0001397
                                arg["Export"] = true;//if count greater than 0,export set to true.By Ashutosh 20-7-2017
                                arg["Save"] = true;
                            }
                        }
                        Patient pat = new Patient();
                        pat = NewDataVariables.GetCurrentPat();
                        eye_fundus_image eyeFundusImage = new eye_fundus_image();
                        eyeFundusImage = NewDataVariables.Active_Obs;
                        _eventHandler.Notify(_eventHandler.AnnotationButtonsRefresh, arg);
                        checkforCDR(NewDataVariables.Active_Annotation.dataXml);
                        eyeFundusImage.eye_fundus_image_annotations.Where(x => x.eyeFundusImageAnnotationId == NewDataVariables.Active_Annotation.eyeFundusImageAnnotationId).ToList()[0].voided = true;
                        eyeFundusImage.eye_fundus_image_annotations.Where(x => x.eyeFundusImageAnnotationId == NewDataVariables.Active_Annotation.eyeFundusImageAnnotationId).ToList()[0].lastModifiedDate = DateTime.Now;
                        eyeFundusImage.eye_fundus_image_annotations.Where(x => x.eyeFundusImageAnnotationId == NewDataVariables.Active_Annotation.eyeFundusImageAnnotationId).ToList()[0].voidedDate = DateTime.Now;
                        NewDataVariables.Active_Obs = eyeFundusImage;
                        pat.observations.Where(y => y.observationId == NewDataVariables.Active_Obs.observationId).ToList()[0].eye_fundus_image_annotations.Where(z => z.eyeFundusImageAnnotationId == NewDataVariables.Active_Annotation.eyeFundusImageAnnotationId).ToList()[0] = NewDataVariables.Active_Obs.eye_fundus_image_annotations.Where(x => x.eyeFundusImageAnnotationId == NewDataVariables.Active_Annotation.eyeFundusImageAnnotationId).ToList()[0];
                        NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0] = pat;
                        //NewIVLDataMethods.RemoveAnnotation();
                        NewIVLDataMethods.UpdatePatient();
                        AnnotationsFiles_dgv.Refresh();
                        //This below bool variable was added by Darshan on 21-08-2015 to solve Defect no 0000591: when deleted the saved annotation,the current annotation performing graph is getting deleted.
                        isview = false;
                        showExisitingAnnotation();
                        int i = AnnotationsFiles_dgv.RowCount;
                        if (i == 0)
                        {
                            this.Cursor = Cursors.WaitCursor;
                            Args annotationArgs = new Args();
                            annotationArgs["imageid"] = Annotation_ImageId;
                            //annotationArgs["isCDR"] = false;//this has been commented for the defect no : 0001694
                            annotationArgs["isDeleted"] = true;
                            annotationDeleteEvent(annotationArgs);
                            this.Cursor = Cursors.Default;
                        }
                        else if (isCDRvalue)
                        {
                            numberOfCDR--;
                            this.Cursor = Cursors.WaitCursor;
                            Dictionary<string, object> annoval = new Dictionary<string, object>();
                            Args annotationArgs = new Args();
                            annotationArgs["imageid"] = Annotation_ImageId;
                            if (numberOfCDR == 0)
                            {
                                annotationArgs["isCDR"] = isCDRvalue;
                            }
                            annotationArgs["isDeleted"] = true;
                            annotationDeleteEvent(annotationArgs);
                            this.Cursor = Cursors.Default;
                        }
                        isCDRvalue = false;
                        //_patientdetailsupdate();
                    }
                }
                else
                    if (AnnotationsFiles_dgv.Columns[e.ColumnIndex].Name == headers[2])
                    {
                        //This below bool variable was added by Darshan on 21-08-2015 to solve Defect no 0000591: when deleted the saved annotation,the current annotation performing graph is getting deleted.
                        isview = true;
                        //INTUSOFT.Data.NewDbModel.eye_fundus_image_annotation annoVal = NewDataVariables.Active_Annotation = NewDataVariables._Repo.GetById<INTUSOFT.Data.NewDbModel.eye_fundus_image_annotation>(Convert.ToInt32(AnnotationsFiles_dgv.Rows[e.RowIndex].Cells["eyeFundusImageAnnotationId"].Value));// NewDataVariables.Active_Annotation included to obtain saved date in report when printed/exported.By Ashutosh 21-08-2017
                        INTUSOFT.Data.NewDbModel.eye_fundus_image_annotation annoVal = NewDataVariables.Active_Annotation = NewDataVariables.Active_Obs.eye_fundus_image_annotations.Where(x => x.eyeFundusImageAnnotationId == (Convert.ToInt32(AnnotationsFiles_dgv.Rows[e.RowIndex].Cells["eyeFundusImageAnnotationId"].Value))).ToList()[0];// NewDataVariables.Active_Annotation included to obtain saved date in report when printed/exported.By Ashutosh 21-08-2017
                        annotationID = annoVal.eyeFundusImageAnnotationId;
                        if (annoVal.cdrPresent)
                            return;
                        bool isTbxEnabled = false;
                        drawArea1.graphicsList.DeleteAll();
                        this.userControl11.Release_Flp();
                        currentRowIndex = AnnotationsFiles_dgv.CurrentRow.Index;
                        string cmts = annoVal.comments;
                        this.Cursor = Cursors.WaitCursor;
                        //HighlightSelectedButton(colorImage_btn, this.toolStrip2);
                        //Display_pbx.Image = colorImage.ToBitmap();
                        //drawArea1.Image = colorImage.ToBitmap();
                        viewannotation(annoVal.dataXml);
                        this.Cursor = Cursors.Default;
                        CommandPointer();
                        Args arg = new Args();//Added by darshan on 25-07-2016 as per NR:0001211 Note no:(0002558)
                        arg["Print"] = true;
                        arg["Export"] = true;//if count greater than 0,export set to true.By Ashutosh 20-7-2017
                        if (NewDataVariables.Active_Obs.eye_fundus_image_annotations.Where(x => x.eyeFundusImageAnnotationId == NewDataVariables.Active_Annotation.eyeFundusImageAnnotationId).ToList()[0].createdDate.Date != DateTime.Now.Date)
                        {
                            drawArea1.Enabled = false;
                            //toolStrip1.Enabled = false;
                            Ellipse.Enabled = false;
                            Rectangle.Enabled = false;
                            FreeDraw_Btn.Enabled = false;
                            LineDraw_btn.Enabled = false;
                            isTbxEnabled = false;
                        }
                        else
                        {
                            drawArea1.Enabled = true;
                            //toolStrip1.Enabled = false;
                            Ellipse.Enabled = true;
                            Rectangle.Enabled = true;
                            FreeDraw_Btn.Enabled = true;
                            LineDraw_btn.Enabled = true;
                            isTbxEnabled = true;
                        }
                        arg["Save"] = false;
                        _eventHandler.Notify(_eventHandler.AnnotationButtonsRefresh, arg);
                        this.userControl11.Disable_control(isTbxEnabled);
                        this.userControl11.unselectall();
                        AnnotationsFiles_dgv.Rows[currentRowIndex].Selected = true;
                        this.Cursor = Cursors.Default;
                    }
                    else//This else statement was added by Darshan to refresh the annotation once it is unselected with the help of control key. 
                    {
                        if (((Control.ModifierKeys & Keys.Control) == Keys.Control))
                        {
                            if (isview && (AnnotationsFiles_dgv.Columns[e.ColumnIndex].Name == headers[0] || AnnotationsFiles_dgv.Columns[e.ColumnIndex].Name == headers[1]))//if isview is true and either headers[0] or headers[1]is clicked then dialog box opens.by ashutosh 07-09-2017.
                            {
                                DialogResult res = CustomMessageBox.Show(clearAnnotationMessage, clearAnnotationHeader, CustomMessageBoxButtons.YesNo, CustomMessageBoxIcon.Warning);//if condition true , then message box pops.by ashutosh 07-09-2017.
                                if (res == DialogResult.Yes)
                                {

                                    if (e.RowIndex > -1)
                                    {
                                        //AnnotationsFiles_dgv.Focus();
                                        // AnnotationsFiles_dgv.Rows[currentRowIndex].Selected = true;
                                        //if (isview) commented by ashutosh 07-09-2017.
                                        //{
                                        isview = false;//if yes , then isview is set to false.by ashutosh 07-09-2017.
                                        drawArea1.ListGraphics.DeleteAll();
                                        drawArea1.Enabled = true;
                                        this.userControl11.Release_Flp();
                                        drawArea1.Refresh();
                                        drawingTools_ts.Enabled = true;
                                        Ellipse.Enabled = true;
                                        Rectangle.Enabled = true;
                                        FreeDraw_Btn.Enabled = true;
                                        LineDraw_btn.Enabled = true;
                                        //saveDrawing_btn.Enabled = true;
                                        ReportedByTextFontChange();
                                        Reportename_txt.Enabled = true;
                                        Args arg = new Args();//Added by darshan on 25-07-2016 as per NR:0001211 Note no:(0002558)
                                        arg["Print"] = false;
                                        arg["Export"] = false;

                                        arg["Save"] = false;
                                        _eventHandler.Notify(_eventHandler.AnnotationButtonsRefresh, arg);
                                        //}
                                    }
                                }
                                else
                                {
                                    AnnotationsFiles_dgv.CurrentRow.Selected = true;//if user slects no,then current row is highlighted.by ashutosh 07-09-2017.
                                }
                            }
                        }

                    }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            isControl = false;
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control || (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                isControl = true;
            }
        }

        /// <summary>
        /// This will change the annotation when up and down arrow is pressed.
        /// </summary>
        /// <param name="rowIndex"></param>
        public void AnnotationChange(int rowIndex)
        {
            if (isview)
            {
                eye_fundus_image_annotation annoVal = NewDataVariables.Annotations.Where(x => x.eyeFundusImageAnnotationId == (Convert.ToInt32(AnnotationsFiles_dgv.Rows[rowIndex].Cells["eyeFundusImageAnnotationId"].Value))).ToList()[0];
                annotationID = annoVal.eyeFundusImageAnnotationId;
                drawArea1.graphicsList.DeleteAll();
                this.userControl11.Release_Flp();
                string cmts = annoVal.comments;
                //HighlightSelectedButton(colorImage_btn, this.toolStrip2);
                //Display_pbx.Image = colorImage.ToBitmap();
                drawArea1.Image = colorImage.ToBitmap();
                viewannotation(annoVal.dataXml);
                drawArea1.Enabled = false;
                //toolStrip1.Enabled = false;
                Ellipse.Enabled = false;
                Rectangle.Enabled = false;
                FreeDraw_Btn.Enabled = false;
                LineDraw_btn.Enabled = false;
                saveDrawing_btn.Enabled = false;
                Print_btn.Enabled = true;
                Export_btn.Enabled = true;
                this.userControl11.Disable_control(annoVal.createdDate.Date == DateTime.Now.Date);
                this.userControl11.unselectall();
            }
        }

        private void AnnotationsFiles_dgv_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
            }
            if (AnnotationsFiles_dgv.SelectedRows.Count > 0)
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (isview)
                    {
                        if (currentRowIndex + 1 < AnnotationsFiles_dgv.RowCount)
                        {
                            currentRowIndex = currentRowIndex + 1;
                            AnnotationChange(currentRowIndex);
                        }
                    }
                }
                else if (e.KeyCode == Keys.Up)
                {
                    if (isview)
                    {
                        if (currentRowIndex - 1 >= 0)
                        {
                            currentRowIndex = currentRowIndex - 1;
                            AnnotationChange(currentRowIndex);
                        }
                    }
                }
            }
        }

        private void CalculateCDR_btn_Click(object sender, EventArgs e)
        {
            drawArea1.CalculatePolygonArea();
        }

        //This Form Closing event has been added by Darshan on 01-09-2015 to solve Defect no 0000602: CR:Confirmation message required before closing the Annotation Window.
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (drawArea1.ListGraphics.Count > 0 && !isview)
            {
                DialogResult res = CustomMessageBox.Show(warningMessage, warningHeader, CustomMessageBoxButtons.YesNo, CustomMessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    Args arg = new Args();
                    arg["isAnnotationUpdate"] = false;
                    SaveAnnotation(arg);
                }
            }
            if (drawArea2 != null)
                drawArea2 = null;
        }

        private void Print_btn_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            HighlightSelectedButton(Print_btn, this.drawingTools_ts);
            isPrint = true;

            CustomFolderBrowser.fileName = "Annotation_Report_" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss") + ".pdf";// to assingn the file name
            GenerateReport();
            _report.isPrintingOver = IVLReport.PrintReport.PrintPDFs(CustomFolderBrowser.filePath + _report.ReportFileName, _report.adobereader_text);
            if (_report.isPrintingOver)
            {
                if (!isview)
                    saveButtonFunctionality();
            }
            //CustomFolderBrowser.ImageSavingbtn += CustomFolderBrowser_ImageSavingbtn;
        }

        private void GenerateReport()
        {
            //Bitmap bm = new Bitmap(Display_pbx.Image as Bitmap);
            //Bitmap bm = new Bitmap(drawArea1.Image as Bitmap);
            Bitmap bm = tempBm.Clone() as Bitmap;

            PicBoxSizeMode sizeMode = PicBoxSizeMode.Normal;
            if (drawArea1.graphicsList.Count > 0)
            {
                //This below code has been added by Darshan on 19-10-2015 to draw the graphs in the annotation on the image (Since graphs are being drawn on a panel). And to save the annotated image for printing purpose.


                #region oldImplementation
                //Bitmap bm = new Bitmap(Display_pbx.Image.Width, Display_pbx.Image.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);// as Bitmap;
                //Common.CommonMethods c = Common.CommonMethods.CreateInstance();
                //LoadSaveImage.LoadImage(AnnotationImagefileName, ref bm);
                #endregion

                for (int i = 0; i < drawArea1.ListGraphics.Count; i++)
                {
                    if (drawArea1.ListGraphics[i] is DrawEllipse)
                    {
                        DrawEllipse dellipse = drawArea1.ListGraphics[i] as DrawEllipse;
                        dellipse.DrawEllipseToBitmap(ref bm, drawArea1.ListGraphics[i].ID, sizeMode);
                    }
                    else if (drawArea1.ListGraphics[i] is DrawRectangle)
                    {
                        DrawRectangle dRect = drawArea1.ListGraphics[i] as DrawRectangle;
                        dRect.DrawRectangleToBitmap(ref bm, drawArea1.ListGraphics[i].ID,sizeMode);
                    }
                    else if (drawArea1.ListGraphics[i] is DrawPolygon)
                    {
                        DrawPolygon dRect = drawArea1.ListGraphics[i] as DrawPolygon;
                        dRect.DrawPolygonToImage(ref bm, drawArea1.ListGraphics[i].ID,sizeMode);
                    }
                    else if (drawArea1.ListGraphics[i] is DrawLine)
                    {
                        DrawLine dline = drawArea1.ListGraphics[i] as DrawLine;
                        dline.DrawLineToBitmap(ref bm,sizeMode);
                    }
                }
                AnnotationXMLProperties annoprop = new AnnotationXMLProperties();
                //bm.Save("Test.png");
                annoprop.Xmlcommentsproperties = userControl11.Get_annotationComments();
                //List<FileInfo> reportTemplates = new List<FileInfo>(); // reportTemplates is of type List.By Ashutosh 16-08-2017//new DirectoryInfo(@"ReportTemplates").GetFiles(@"ReportTemplates", SearchOption.AllDirectories).ToList();
                //string[] reportTemplatesFileNames = System.IO.Directory.GetFiles(@"ReportTemplates", "*", SearchOption.AllDirectories);//searches for subdirectories and provides the name of XMLfile(Annotation_LsA4.xml) to string[] reportTemplatesFileNames By Ashutosh 16-08-2017
                //for (int i = 0; i < reportTemplatesFileNames.Length; i++)
                //{
                //    reportTemplates.Add(new FileInfo(reportTemplatesFileNames[i]));
                //} 


                Dictionary<string, string> annotationCommentsDic = new Dictionary<string, string>();// Added to save the annotation comments and send the annotaion comments and send it to the pdfgenerator.
                for (int i = 0; i < annoprop.Xmlcommentsproperties.Count; i++)
                {
                    annotationCommentsDic.Add((i + 1).ToString(), annoprop.Xmlcommentsproperties[i].Comments);//Adds the comments from annoprop.Xmlcommentsproperties to annotationCommentsDic.
                }
                if (reportDic.ContainsKey("$AnnotationComments"))//Checks for the availability of the key $AnnotationComments if exists removes it.
                    reportDic.Remove("$AnnotationComments");
                annotationCommentsDic.Add("$tableColumnWidthA4", "2.2");//second column width in the tabel for A4 sheet. 
                annotationCommentsDic.Add("$tableColumnHeightA4", "0.3");//first column width in the tabel for A4 sheet.
                annotationCommentsDic.Add("$tableColumnWidthA5", "1.6");//second column width in the tabel for A5 sheet.
                annotationCommentsDic.Add("$tableColumnHeightA5", "0.15");//first column width in the tabel for A5 sheet.

                reportDic.Add("$AnnotationComments", annotationCommentsDic);
                //reportDic.Add("$Datetime", annotationDetails["Datetime"]);

                //if (reportDic.ContainsKey("$Datetime"))// check if key is present .By Ashutosh 18-08-2017
                //    reportDic["$Datetime"] = annotationDetails["Datetime"];// if present then its value associated with key is replaced with a value.By Ashutosh 18-08-2017
                //else
                //    reportDic.Add("$Datetime", annotationDetails["Datetime"]);// else key and value added added.By Ashutosh 18-08-2017

                //if (reportDic.ContainsKey("$allTemplateFiles"))// check if key is present .By Ashutosh 16-08-2017
                //    reportDic["$allTemplateFiles"] = reportTemplates;// if present then its replaced by reportTemplates.By Ashutosh 16-08-2017
                //    else
                //    reportDic.Add("$allTemplateFiles", reportTemplates);// else reportTemplates is added.By Ashutosh 16-08-2017
                if (reportDic.ContainsKey("$isFromCDR"))//check if isFromCDR is present.By Ashutosh 16-08-2017
                    reportDic["$isFromCDR"] = false;// if present then conent of isFromCDR is made false.By Ashutosh 16-08-2017
                else
                    reportDic.Add("$isFromCDR", false);// if not ,its added.By Ashutosh 16-08-2017
                if (reportDic.ContainsKey("$isAnnotation"))//checks if isAnnotation is present.By Ashutosh 16-08-2017
                    reportDic["$isAnnotation"] = true;//if present then conent of isAnnotation is made false.By Ashutosh 16-08-2017
                else
                    reportDic.Add("$isAnnotation", true);//if not ,its added.By Ashutosh 16-08-2017

            }
            //bm.Save("Rect.bmp");
            if (NewDataVariables.Active_Annotation != null)//Active_Annotation is null for new annotation , for saved annotation it's not null.By Ashutosh 21-08-2017.
            {
                if (reportDic.ContainsKey("$Datetime"))//checks if key is present .By Ashutosh 21-08-2017
                    reportDic["$Datetime"] = NewDataVariables.Active_Annotation.createdDate.ToString("dd-MMM-yy");//if present, createdDate replaces value associated with key $Datetime.By  Ashutosh 21-08-2017.
                else
                    reportDic.Add("$Datetime", NewDataVariables.Active_Annotation.createdDate.ToString("dd-MMM-yy"));//if not present ,key and value added to dictionary.By  Ashutosh 21-08-2017.
            }
            if (reportDic.ContainsKey("$CurrentImagebm"))//to add the bitmap to the dictionary so as to add it in pdf creation
                reportDic["$CurrentImagebm"] = bm.Clone() as Bitmap;
            else
                reportDic.Add("$CurrentImagebm", bm.Clone() as Bitmap);

            bm.Dispose();
            reportDic.Remove("$Doctor");
            if (Reportename_txt.Text.Equals(reportDic["$ReportedBy"]) || string.IsNullOrEmpty(Reportename_txt.Text.ToString()))
            {
                reportDic.Add("$Doctor", string.Empty);
            }
            else
            {
                reportDic.Add("$Doctor", Reportename_txt.Text);
                //annotationDetails.Add("$operator",  char.ToUpper(Reportename_txt.Text[0]) + Reportename_txt.Text.Substring(1));//Old Implementation.
            }
            //reportDic.Add("$reportNameText", annotationDetails["reportNameText"]);
            _report = new IVLReport.Report(reportDic);
            string currentTemplate = reportDic["$currentTemplate"] as string;
            _report.parseXmlData(reportDic["$currentTemplate"] as string);
            _report.SetTheValuesFormReportData();
            try
            {
                _report.CreatePdf();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            this.Cursor = Cursors.Default;
        }

        //This below event has been added to solve Defect no 0000718: The annotation comments section is not getting refresh.
        private void AnnotationsFiles_dgv_ColumnHeaderMouseClick(object sender, System.Windows.Forms.DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (isview)
                        AnnotationsFiles_dgv.Rows[currentRowIndex].Selected = true;
                }
            }
        }

        private void Reportename_txt_Enter(object sender, EventArgs e)
        {
            if (Reportename_txt.Text == reported_by)
            {
                Reportename_txt.Text = "";
                Reportename_txt.ForeColor = Color.Black;
                Font f = Reportename_txt.Font;
                Font newFont = new Font(f.FontFamily, f.Size, FontStyle.Regular, GraphicsUnit.Point);
                Reportename_txt.Font = newFont;
            }
        }

        /// <summary>
        /// This method will change the font style and Reportename_txt text box contents.
        /// </summary>
        public void ReportedByTextFontChange()
        {
            if (!string.IsNullOrEmpty(dr_operator))
            {
                Reportename_txt.Text = dr_operator.ToString();//assigns the Reporter name to Reportename_txt.
                Reportename_txt.ForeColor = Color.Black;
                Font f = Reportename_txt.Font;
                Font newFont = new Font(f.FontFamily, f.Size, FontStyle.Regular, GraphicsUnit.Point);
                Reportename_txt.Font = newFont;
            }
            else
            {
                Reportename_txt.Text = reported_by;//assigns sets default name to Reportename_txt.
                Reportename_txt.ForeColor = SystemColors.WindowFrame;
                Font f = Reportename_txt.Font;
                Font newFont = new Font(f.FontFamily, f.Size, FontStyle.Italic, GraphicsUnit.Point);
                Reportename_txt.Font = newFont;
            }
        }

        private void Reportename_txt_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Reportename_txt.Text))
            {
                ReportedByTextFontChange();//This method is being invoked to replicate the same functionality as in CDR tool.
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

        private void redChannel_btn_Click(object sender, EventArgs e)
        {
            //HighlightSelectedButton(redChannel_btn, this.toolStrip2);
            //Display_pbx.Image = redChannel.ToBitmap();
            if (CurrentChannelDisplayed != Channels.Red)
            {
                CurrentChannelDisplayed = Channels.Red;
                #region 0001336 has been fixed by copying the channel image to temp bitmap and drawing to the modifying bitmap

                Image<Bgr, byte> outputSingleChannelImage = new Image<Bgr, byte>(colorImage.Width, colorImage.Height);
                CvInvoke.Merge(new Emgu.CV.Util.VectorOfMat(colorImage[2].Mat, colorImage[2].Mat, colorImage[2].Mat), outputSingleChannelImage.Mat);
                tempBm = outputSingleChannelImage.ToBitmap(); // in order to have a place for indexed bitmap only occuring for channel images

                outputSingleChannelImage.Dispose();
                #endregion
            }
            else
            {
                tempBm = colorImage.ToBitmap(); // in order to have a place for indexed bitmap only occuring for channel images

                CurrentChannelDisplayed = Channels.Color;
            }
            ApplyZoom(1);

            RGBbackgroundcolor();
        }

        private void greenChannel_btn_Click(object sender, EventArgs e)
        {
            //HighlightSelectedButton(greenChannel_btn, this.toolStrip2);
            //Display_pbx.Image = greenChannel.ToBitmap();
            if (CurrentChannelDisplayed != Channels.Green)
            {
                CurrentChannelDisplayed = Channels.Green;
                #region 0001336 has been fixed by copying the channel image to temp bitmap and drawing to the modifying bitmap
                Image<Bgr, byte> outputSingleChannelImage = new Image<Bgr, byte>(colorImage.Width, colorImage.Height);
                CvInvoke.Merge(new Emgu.CV.Util.VectorOfMat(colorImage[1].Mat, colorImage[1].Mat, colorImage[1].Mat), outputSingleChannelImage.Mat);
                tempBm = outputSingleChannelImage.ToBitmap(); // in order to have a place for indexed bitmap only occuring for channel images

                outputSingleChannelImage.Dispose();

                #endregion
            }
            else
            {
                tempBm = colorImage.ToBitmap(); // in order to have a place for indexed bitmap only occuring for channel images

                CurrentChannelDisplayed = Channels.Color;
            }
            //Display_pbx.Image = tempBm;
            ApplyZoom(1);


            RGBbackgroundcolor();
        }

        private void blueChannel_btn_Click(object sender, EventArgs e)
        {
            //HighlightSelectedButton(blueChannel_btn, this.toolStrip2);
            //Display_pbx.Image = blueChannel.ToBitmap();
            if (CurrentChannelDisplayed != Channels.Blue)
            {
                CurrentChannelDisplayed = Channels.Blue;
                #region 0001336 has been fixed by copying the channel image to temp bitmap and drawing to the modifying bitmap
                Image<Bgr, byte> outputSingleChannelImage = new Image<Bgr, byte>(colorImage.Width, colorImage.Height);
                CvInvoke.Merge(new Emgu.CV.Util.VectorOfMat(colorImage[0].Mat, colorImage[0].Mat, colorImage[0].Mat), outputSingleChannelImage.Mat);
                tempBm = outputSingleChannelImage.ToBitmap(); // in order to have a place for indexed bitmap only occuring for channel images

                outputSingleChannelImage.Dispose();

                #endregion
            }
            else
            {
                tempBm = colorImage.ToBitmap(); // in order to have a place for indexed bitmap only occuring for channel images

                CurrentChannelDisplayed = Channels.Color;
            }
            //Display_pbx.Image = tempBm;
            ApplyZoom(1);

            RGBbackgroundcolor();
        }

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


        private void Reportename_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && (e.KeyCode == Keys.A))
            {
                if (sender != null)
                    ((TextBox)sender).SelectAll();
                e.Handled = true;
            }
        }

        public void calculateNoOfCDR()
        {
            foreach (eye_fundus_image_annotation item in NewDataVariables.Annotations)
            {
                if (item.cdrPresent)
                    numberOfCDR++;
            }
        }
        private void customFolderBrowser_CancelButtonClickedEvent()
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
            HighlightSelectedButton(Export_btn, this.drawingTools_ts);

            //CustomFolderBrowser.fileName = "Annotation_Report_" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss") + ".pdf";// to assingn the file name
            isPrint = false;
            customFolderBrowser.isReportExport = true;// this is necessary for ShowImageExportButtons() which along with isReportExport is part of customfolderbrowser.
            GenerateReport();
            Args arg = new Args(); // Args is a dictionary of <string , object >
            arg["isAnnotationUpdate"] = false;
            customFolderBrowser.CustomFolderData = annotationDetails["$customFolderData"] as Dictionary<string, object>;//dictionary was not present earlier, due to which folder path when empty or invalid path led to application crash. By Ashutosh 18-08-2017
            customFolderBrowser.ShowImageExportButtons(); //this is to ensure the export labels and textbox contents to hide or visible.
            //SaveAnnotation(arg);//gets the datetimeimageidCommentannotationID from SaveAnnotation.this line has been coomented since the logic of making it true is wrong 20/7

            // this part subcribes the delegate or method in the right to internal list on the left

            CustomFolderBrowser.CancelButtonClickedEvent += customFolderBrowser_CancelButtonClickedEvent;
            //

            //+= subscribes to an event. The delegate or method on the right-hand side of the += will be added to an internal list that the event keeps track of, and when the owning class fires that event, all the delegates in the list will be called.
            CustomFolderBrowser.ImageSavingbtn += CustomFolderBrowser_ImageSavingbtn;

            customFolderBrowser.ShowDialog();// this pops up the dialog
            this.Cursor = Cursors.Default;//Gets the default cursor, which is usually an arrow cursor
        }


        private void CustomFolderBrowser_ImageSavingbtn()
        {
            string[] splitName = (reportDic["$Name"] as string).Split(new char[0]);//to split the name - first name and last name
            string dirPath = customFolderBrowser.folderPath + Path.DirectorySeparatorChar + reportDic["$MRN"] + "_" + splitName[0] + "_" + splitName[1] + "_" + reportDic["$Age"] + "_" + reportDic["$Gender"];
            if (!Directory.Exists(dirPath))//check whether the directory is not available 
                Directory.CreateDirectory(dirPath);//creates new directory
            //File.Move- moves files from source to destinagtion and provides option to specify a new filename.
            File.Copy(CustomFolderBrowser.filePath + CustomFolderBrowser.fileName, dirPath + Path.DirectorySeparatorChar + CustomFolderBrowser.fileName);
            CustomFolderBrowser.fileNames = new string[] { dirPath + Path.DirectorySeparatorChar + CustomFolderBrowser.fileName };
            //fileNames can handle multiple files.dirpath is a string , whereas fileNames is string[].
            //CustomFolderBrowser.fileName = dirPath + Path.DirectorySeparatorChar + CustomFolderBrowser.fileName;
            //CustomFolderBrowser.fileName-D:\Projects\Intusoft\IvlSoft.Desktop\bin\x64\Debug
            string reportExportTextStr = string.Empty;
            string reportExportHeaderStr = string.Empty;
            //below statements necessary for confirmation regarding the success of export.
            if (reportDic.ContainsKey("$exportReportText"))
                reportExportTextStr = reportDic["$exportReportText"] as string;
            if (reportDic.ContainsKey("$exportReportHeader"))
                reportExportHeaderStr = reportDic["$exportReportHeader"] as string;
            //messagebox to show that exporting is completed
            DialogResult exported = CustomMessageBox.Show(reportExportTextStr, reportExportHeaderStr, CustomMessageBoxIcon.Information);
            if (exported == DialogResult.OK)
            {
                customFolderBrowser.Close();
            }
            
        }

        private void zoomIn_btn_Click(object sender, EventArgs e)
        {
            float newZoomValue = Zoom + zoomFactor;
            if (newZoomValue < zoomMax)
            {
                zoomVal_tbx.Text = newZoomValue.ToString();
            }
        }

        private void zoomOut_btn_Click(object sender, EventArgs e)
        {
            float newZoomValue = (Zoom-zoomFactor);
            if ( newZoomValue > zoomMin)
            {

                zoomVal_tbx.Text = (newZoomValue).ToString();

                //Zoom -= zoomFactor;// ZoomOut();
            }
        }



        private void zoomVal_tbx_TextChanged(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(zoomVal_tbx.Text))
          Zoom =  Convert.ToSingle(zoomVal_tbx.Text);
        }

    }
}