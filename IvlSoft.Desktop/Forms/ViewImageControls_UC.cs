using Annotation;
using Cloud_Models.Models;
using Common;
using Common.ValidatorDatas;
using Emgu.CV;
using Emgu.CV.Structure;
using INTUSOFT.Data.Extension;
using INTUSOFT.Data.NewDbModel;
using INTUSOFT.Data.Repository;
using INTUSOFT.EventHandler;
using INTUSOFT.Imaging;
using INTUSOFT.ThumbnailModule;
using IVLReport;
using Newtonsoft.Json;
using NLog;
using Svg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace INTUSOFT.Desktop.Forms
{
    public partial class ViewImageControls_UC : UserControl
    {
        #region variables and constants
        public enum Channels { Color, Red, Green, Blue };
        Channels CurrentChannelDisplayed;
        IVLEventHandler eventHandler;
        ToolTip scrollToolTip;
        IVLReport.Report _report;
        Dictionary<string, object> reportDic;
        int currentVal = 1;
        Dictionary<string, object> annotationDetails;
        public int zoomMagnifierMaxValue = 300;
        public int zoomMagnifierMinValue = 100;
        Bitmap OriginalBm;
        string saveasFileLabel = string.Empty;
        public Bitmap modifyingBm;
        ThumbnailData thumbnailData;
        bool isCDRvalue = false;
        string[] images;
        bool isView = false;
        string[] currentReportImageFiles, currentReportLabelNames;
        public Label noImageSelected_lbl;
        XmlSerializer maskSettingsSerializer;
        bool isLeft = false;
        Bitmap maskBitmap;
        ToolTip leftToolTip;
        ToolTip rightToolTip;
        ToolTip enableZoombtn;
        Logger ExceptionLog = LogManager.GetLogger("ExceptionLog");
        int NoOfImagesToBeSelected = Convert.ToInt32(IVLVariables.CurrentSettings.UserSettings._noOfImagesForReport.val);
        string NoOfImagesToBeSelectedText1 = IVLVariables.LangResourceManager.GetString("NoOfImagesToBeSelected_Text1", IVLVariables.LangResourceCultureInfo);
        string NoOfImagesToBeSelectedText2 = IVLVariables.LangResourceManager.GetString("NoOfImagesToBeSelected_Text2", IVLVariables.LangResourceCultureInfo);
        string NoOfImagesToBeSelectedHeader = IVLVariables.LangResourceManager.GetString("NoOfImagesToBeSelected_Header", IVLVariables.LangResourceCultureInfo);


        public bool iszoom = false;
        XmlSerializer captureSettingsSerializer;
        Image<Bgr, byte> img;
        CustomFolderBrowser customFolderBrowser;// = CustomFolderBrowser.GetInstance();// new CustomFolderBrowser();
        Dictionary<string, object> customFolderData = new Dictionary<string, object>();
        DateTime ReportCreatedDateTime;
        Image redFilterSelected, greenFilterSelected, blueFilterSelected;
        Image redFilter, greenFilter, blueFilter;
        #endregion
        public ViewImageControls_UC()
        {
            InitializeComponent();
            scrollToolTip = new ToolTip();
            leftToolTip = new ToolTip();
            rightToolTip = new ToolTip();
            enableZoombtn = new ToolTip();
            //customFolderBrowser = new  CustomFolderBrowser.GetInstance();// new CustomFolderBrowser();
            InitializeResourceStrings();

            eventHandler = IVLEventHandler.getInstance();
            eventHandler.Register(eventHandler.LoadImageFromFileViewingScreen, new NotificationHandler(LoadImageFromFile));
            eventHandler.Register(eventHandler.GetImageFilesFromThumbnails, new NotificationHandler(getImageFilesFromThumbnails));
            eventHandler.Register(eventHandler.RGBColorchange, new NotificationHandler(rgbcolorchange));
            eventHandler.Register(eventHandler.SaveImgChanges, new NotificationHandler(Saveimg_changes));
            eventHandler.Register(eventHandler.CreateReportEvent, new NotificationHandler(createReportEvent));
            eventHandler.Register(eventHandler.RefreshExistingReport, new NotificationHandler(RefreshShowReports));
            thumbnailData = new ThumbnailData();


            toolStrip3.Renderer = new Custom.Controls.FormToolStripRenderer();
            toolStrip2.Renderer = new Custom.Controls.FormToolStripRenderer();
            toolStrip1.Renderer = new Custom.Controls.FormToolStripRenderer();
            increaseZoomToolStrip.Renderer = new Custom.Controls.FormToolStripRenderer();
            decreaseZoomToolStrip.Renderer = new Custom.Controls.FormToolStripRenderer();
            decreaseContrastToolStrip.Renderer = new Custom.Controls.FormToolStripRenderer();
            increaseContrastToolStrip.Renderer = new Custom.Controls.FormToolStripRenderer();
            increaseBrightnessToolStrip.Renderer = new Custom.Controls.FormToolStripRenderer();
            decreaseBrightnessToolStrip.Renderer = new Custom.Controls.FormToolStripRenderer();


            string redFilterLogoPath = @"ImageResources\FilterImageResources\Red.png";
            string greenFilterLogoPath = @"ImageResources\FilterImageResources\Green.png";
            string blueFilterLogoPath = @"ImageResources\FilterImageResources\Blue.png";

            string redFilterSelectedLogoPath = @"ImageResources\FilterImageResources\RedNegative.png";
            string greenFilterSelectedLogoPath = @"ImageResources\FilterImageResources\GreenNegative.png";
            string blueFilterSelectedLogoPath = @"ImageResources\FilterImageResources\BlueNegative.png";

            string zoomIncreaseLogoImage = @"ImageResources\ImageToolsResources\zoomIn.png";
            string zoomDecreaseLogoImage = @"ImageResources\ImageToolsResources\zoomOut.png";
            string saveAsLogoPath = @"ImageResources\Edit_ImageResources\Save-As Icon.png";
            string saveLogoPath = @"ImageResources\Edit_ImageResources\SaveIcon.png";
            string exportLogoPath = @"ImageResources\Edit_ImageResources\Export_Image_Square.png";

            string increaseBrightnessImagePath = @"ImageResources\IntensityImageResources\brightnessIncrease.png";
            string decreaseBrightnessImagePath = @"ImageResources\IntensityImageResources\brightnessDecrease.png";
            string increaseContrastImagePath = @"ImageResources\IntensityImageResources\ContrastIncrease.png";
            string decreaseContrastImagePath = @"ImageResources\IntensityImageResources\ContrastDecrease.png";

            string addReportLogoPath = @"ImageResources\ToolsResources\AddReport.png";
            string addAnnotationLogoPath = @"ImageResources\ToolsResources\AddAnnotation.png";
            string addCDRToolLogoPath = @"ImageResources\ToolsResources\CDR.png";





            if (File.Exists(increaseBrightnessImagePath))
                increaseBrightness_btn.Image = Image.FromFile(increaseBrightnessImagePath);//Increase brightness button image
            if (File.Exists(decreaseBrightnessImagePath))
                decreaseBrightness_btn.Image = Image.FromFile(decreaseBrightnessImagePath);//Decrase brightness button image

            if (File.Exists(increaseContrastImagePath))
                increaseContrast_btn.Image = Image.FromFile(increaseContrastImagePath);//Increase contrast button image
            if (File.Exists(decreaseContrastImagePath))
                decreaseContrast_tbn.Image = Image.FromFile(decreaseContrastImagePath);//Decrease contrast buton image

            if (File.Exists(redFilterSelectedLogoPath))
                redFilterSelected = Image.FromFile(redFilterSelectedLogoPath); //Red filter selected image;
            if (File.Exists(greenFilterSelectedLogoPath))
                greenFilterSelected = Image.FromFile(greenFilterSelectedLogoPath); //Green filter selected Image;
            if (File.Exists(blueFilterSelectedLogoPath))
                blueFilterSelected = Image.FromFile(blueFilterSelectedLogoPath); //Blue filter selected Image;

            if (File.Exists(redFilterLogoPath))
            {
                redFilter = Image.FromFile(redFilterLogoPath); //Red filter image;
                showRedChannel_btn.Image = redFilter;
            }
            if (File.Exists(greenFilterLogoPath))
            {
                greenFilter = Image.FromFile(greenFilterLogoPath); //Green filter Image;
                showGreenChannel_btn.Image = greenFilter;
            }
            if (File.Exists(blueFilterLogoPath))
            {
                blueFilter = Image.FromFile(blueFilterLogoPath); //Blue filter Image;
                showBlueChannel_btn.Image = blueFilter;
            }

            if (File.Exists(zoomIncreaseLogoImage))
                increaseZoom_btn.Image = Image.FromFile(zoomIncreaseLogoImage);
            if (File.Exists(zoomDecreaseLogoImage))
                decreseZoom_btn.Image = Image.FromFile(zoomDecreaseLogoImage);
            if (File.Exists(saveAsLogoPath))
                saveAs_btn.Image = Image.FromFile(saveAsLogoPath);
            if (File.Exists(saveLogoPath))
                save_btn.Image = Image.FromFile(saveLogoPath);
            if (File.Exists(exportLogoPath))
                exportImages_btn.Image = Image.FromFile(exportLogoPath);


            if (File.Exists(addReportLogoPath))
                newReport_btn.Image = Image.FromFile(addReportLogoPath);
            if (File.Exists(addAnnotationLogoPath))
                newAnnotation_btn.Image = Image.FromFile(addAnnotationLogoPath);
            if (File.Exists(addCDRToolLogoPath))
                glaucomaTool_btn.Image = Image.FromFile(addCDRToolLogoPath);

            noImageSelected_lbl = new Label();
            noImageSelected_lbl.Text = IVLVariables.LangResourceManager.GetString("NoImagesSelected_Label_Text", IVLVariables.LangResourceCultureInfo);
            zoomMag_rb.Maximum = zoomMagnifierMaxValue;
            zoomMag_rb.Minimum = zoomMagnifierMinValue;
            zoomMag_rb.Enabled = false;
            SetConfigSettings();
            maskSettingsSerializer = new XmlSerializer(typeof(INTUSOFT.Imaging.MaskSettings));
            captureSettingsSerializer = new XmlSerializer(typeof(INTUSOFT.Imaging.CaptureLog));
            PrintReport.adobeDelayTime = Convert.ToInt32(IVLVariables.CurrentSettings.PrinterSettings._adobePrintDelay.val);// this line is added to handle the delay time for adobe reader to stay on screen to accomodate different types of printers
            //toolStrip1.Renderer = new Custom.Controls.FormToolStripRenderer();
            UpdateControlsForCurrentResolution();
            UpdateFontForeColor();
            Reports_dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            List<string> date = new List<string>();
            List<string> time = new List<string>();
            //Reports_dgv.DataSource = NewDataVariables.Reports.ToDataTable();
            //Reports_dgv.Columns[0].Name = IVLVariables.LangResourceManager.GetString("Report_Slno_Text", IVLVariables.LangResourceCultureInfo);
            //Reports_dgv.Columns[0].HeaderText = IVLVariables.LangResourceManager.GetString("Report_Slno_Text", IVLVariables.LangResourceCultureInfo);
            //Reports_dgv.Columns[1].Name = IVLVariables.LangResourceManager.GetString("Report_Date_Text", IVLVariables.LangResourceCultureInfo);
            //Reports_dgv.Columns[1].HeaderText = IVLVariables.LangResourceManager.GetString("Report_Date_Text", IVLVariables.LangResourceCultureInfo);
            //Reports_dgv.Columns[2].Name = IVLVariables.LangResourceManager.GetString("Report_Time_Text", IVLVariables.LangResourceCultureInfo);
            //Reports_dgv.Columns[2].HeaderText = IVLVariables.LangResourceManager.GetString("Report_Time_Text", IVLVariables.LangResourceCultureInfo);
            //Reports_dgv.Columns[3].Name = IVLVariables.LangResourceManager.GetString("CloudReportStatus_Text", IVLVariables.LangResourceCultureInfo);
            //Reports_dgv.Columns[3].HeaderText = IVLVariables.LangResourceManager.GetString("CloudReportStatus_Text", IVLVariables.LangResourceCultureInfo);
            if (IVLVariables.isCommandLineAppLaunch)
            {
                //                this.tableLayoutPanel1.Controls.Remove(panel2);
                this.tableLayoutPanel1.RowStyles[0] = new RowStyle(SizeType.Percent, 0f);
                this.tableLayoutPanel8.RowStyles[8] = new RowStyle(SizeType.Percent, 0f);
                this.tableLayoutPanel8.RowStyles[9] = new RowStyle(SizeType.Percent, 0f);
                this.tableLayoutPanel8.RowStyles[10] = new RowStyle(SizeType.Percent, 0f);
            }
            // Reports_dgv.Enabled = !IVLVariables.isCommandLineAppLaunch;
            // file_lbl.Enabled = !IVLVariables.isCommandLineAppLaunch;
        }




        #region public methods

        /// <summary>
        /// This function will return list of controls in emrmange form.
        /// </summary>
        /// <param name="form">form name</param>
        /// <returns>List of controls</returns>
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

        public void UpdateFontForeColor()
        {
            List<Control> controls = GetControls(this).ToList();
            foreach (Control c in controls)
            {
                if (c is ToolStrip)
                {
                    ToolStrip l = c as ToolStrip;
                    if (l.Name == toolStrip1.Name || l.Name == toolStrip3.Name || l.Name == toolStrip2.Name)
                    {
                        for (int i = 0; i < l.Items.Count; i++)
                        {
                            l.Items[i].ForeColor = IVLVariables.GradientColorValues.FontForeColor;
                            l.Refresh();
                        }

                    }
                }
                if (c is Label)
                {
                    c.ForeColor = IVLVariables.GradientColorValues.FontForeColor;
                }

            }
        }

        public void UpdateControlsForCurrentResolution()
        {
            foreach (Control c in this.tableLayoutPanel1.Controls)
            {
                if (Screen.PrimaryScreen.Bounds.Width == 1920)
                {
                    c.Font = new Font(c.Font.FontFamily.Name, 12f);
                    brightness_lbl.Font = new Font(c.Font.FontFamily.Name, 12f);
                    contrast_lbl.Font = new Font(c.Font.FontFamily.Name, 12f);
                    leftSide_btn.Font = rightSide_btn.Font = removePostProcessing_btn.Font = enableZoom_btn.Font = new Font(leftSide_btn.Font.FontFamily.Name, 12F);
                }
                else if (Screen.PrimaryScreen.WorkingArea.Width == 1366)
                {
                    c.Font = new Font(c.Font.FontFamily.Name, 9f);
                    brightness_lbl.Font = new Font(c.Font.FontFamily.Name, 9f);
                    contrast_lbl.Font = new Font(c.Font.FontFamily.Name, 9f);
                }
                else
                    c.Font = new Font(c.Font.FontFamily.Name, 9f);
            }
            if (Screen.PrimaryScreen.Bounds.Width == 1920)
            {
                showRedChannel_btn.Size = showGreenChannel_btn.Size = showBlueChannel_btn.Size = new Size(90, 50);
                newReport_btn.Size = newAnnotation_btn.Size = glaucomaTool_btn.Size = new Size(90, 70);
                save_btn.Size = saveAs_btn.Size = exportImages_btn.Size = new Size(80, 80);
                decreaseBrightness_btn.Size = decreaseContrast_tbn.Size = decreseZoom_btn.Size = increaseBrightness_btn.Size = increaseContrast_btn.Size = increaseZoom_btn.Size = new Size(40, 40);
                toolStrip3.ImageScalingSize = new Size(50, 50);
                newReport_btn.Margin = newAnnotation_btn.Margin = new System.Windows.Forms.Padding(7, 1, 0, 2);
                showRedChannel_btn.Margin = showGreenChannel_btn.Margin = showBlueChannel_btn.Margin = new System.Windows.Forms.Padding(4, 1, 0, 2);
                glaucomaTool_btn.Margin = new System.Windows.Forms.Padding(15, 1, 0, 2);
                //newReport_btn.Size = newAnnotation_btn.Size = glaucomaTool_btn.Size = new Size(95, 70);
                //showRedChannel_btn.Size = showGreenChannel_btn.Size = showBlueChannel_btn.Size = new Size(85, 50);
            }
            else if (Screen.PrimaryScreen.Bounds.Width == 1366)
            {
                showRedChannel_btn.Size = showGreenChannel_btn.Size = showBlueChannel_btn.Size = new Size(52, 47);
                save_btn.Size = saveAs_btn.Size = exportImages_btn.Size = new Size(50, 60);
                toolStrip2.ImageScalingSize = new Size(40, 40);

                decreaseBrightnessToolStrip.ImageScalingSize = new Size(24, 24);
                decreaseContrastToolStrip.ImageScalingSize = new Size(24, 24);
                decreaseZoomToolStrip.ImageScalingSize = new Size(24, 24);
                increaseBrightnessToolStrip.ImageScalingSize = new Size(24, 24);
                increaseContrastToolStrip.ImageScalingSize = new Size(24, 24);
                increaseZoomToolStrip.ImageScalingSize = new Size(24, 24);
                decreaseBrightness_btn.Size = decreaseContrast_tbn.Size = decreseZoom_btn.Size = increaseBrightness_btn.Size = increaseContrast_btn.Size = increaseZoom_btn.Size = new Size(24, 24);

            }
            else if (Screen.PrimaryScreen.Bounds.Width == 1280)
            {
                showRedChannel_btn.Size = showGreenChannel_btn.Size = showBlueChannel_btn.Size =
                     save_btn.Size = saveAs_btn.Size = exportImages_btn.Size = new Size(32, 20);
            }
        }



        /// <summary>
        /// Initializes the OriginalBm and modifyingBm to picBox
        /// </summary>
        /// <param name="picBox"></param>
        public void intitializeDisplayPbx(Bitmap picBox)
        {
            OriginalBm = picBox;
            modifyingBm = picBox;
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
                            showRedChannel_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("Red_Filter_Selected_Tool_Tip_Text", IVLVariables.LangResourceCultureInfo);
                            showGreenChannel_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("GreenFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
                            showBlueChannel_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("BlueFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
                            showRedChannel_btn.Image = redFilterSelected;
                            showBlueChannel_btn.Image = blueFilter;
                            showGreenChannel_btn.Image = greenFilter;
                        }
                        break;
                    case Channels.Green:
                        {
                            showGreenChannel_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("Red_Filter_Selected_Tool_Tip_Text", IVLVariables.LangResourceCultureInfo);
                            showRedChannel_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("RedFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
                            showBlueChannel_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("BlueFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
                            showRedChannel_btn.Image = redFilter;
                            showBlueChannel_btn.Image = blueFilter;
                            showGreenChannel_btn.Image = greenFilterSelected;

                        }
                        break;
                    case Channels.Blue:
                        {
                            showBlueChannel_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("Red_Filter_Selected_Tool_Tip_Text", IVLVariables.LangResourceCultureInfo);
                            showRedChannel_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("RedFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
                            showGreenChannel_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("GreenFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
                            showRedChannel_btn.Image = redFilter;
                            showBlueChannel_btn.Image = blueFilterSelected;
                            showGreenChannel_btn.Image = greenFilter;
                        }
                        break;
                    case Channels.Color:
                        {
                            showRedChannel_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("RedFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
                            showGreenChannel_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("GreenFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
                            showBlueChannel_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("BlueFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
                            showRedChannel_btn.Image = redFilter;
                            showBlueChannel_btn.Image = blueFilter;
                            showGreenChannel_btn.Image = greenFilter;
                        }
                        break;
                    default:
                        {
                            showRedChannel_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("RedFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
                            showGreenChannel_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("GreenFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
                            showBlueChannel_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("BlueFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
                            showRedChannel_btn.Image = redFilter;
                            showBlueChannel_btn.Image = blueFilter;
                            showGreenChannel_btn.Image = greenFilter;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
                //                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        /// <summary>
        /// Pop ups a message to save or discard the changes made on the image in view imaging screen.
        /// </summary>
        public void SaveChangedImages()
        {
            try
            {

                if (IVLVariables.isValueChanged)
                {
                    IVLVariables.IsAnotherWindowOpen = true;
                    DialogResult res = CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("Channel_Confirmation", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("Channel_Header", IVLVariables.LangResourceCultureInfo), CustomMessageBoxButtons.YesNoCancel, CustomMessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                    {
                        ThumbnailUI.isValueChanged = false;
                        save_image();
                        ResetBrightnessContrastZoom();
                        IVLVariables.ivl_Camera.isValueChanged = false;
                    }
                    else if (res == DialogResult.No)
                    {
                        ResetBrightnessContrastZoom();
                        ThumbnailUI.isValueChanged = false;
                        changeEyeSide_lbl.Focus();
                        IVLVariables.ivl_Camera.isValueChanged = false;
                    }
                    else if (res == DialogResult.Cancel)
                    {
                        IVLVariables.isValueChanged = true;
                        ThumbnailUI.isValueChanged = true;
                        IVLVariables.ivl_Camera.isValueChanged = true;
                        //valuechanged = true;

                    }

                    IVLVariables.IsAnotherWindowOpen = false;
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
                //                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }

        }

        /// <summary>
        /// Changes the color of the zoom magnifier button when it is enabled or disabled.
        /// </summary>
        /// <param name="ismodified">indicates wheather the zoom magnifier is eenabled or not</param>
        public void ZoomModifier_color(bool ismodified)
        {
            try
            {
                if (!noImageSelected_lbl.Visible)
                {
                    if (ismodified)
                    {
                        enableZoom_btn.BackColor = Color.Khaki;
                        enableZoombtn.SetToolTip(enableZoom_btn, IVLVariables.LangResourceManager.GetString("EnableZoomButton_ToolTipText", IVLVariables.LangResourceCultureInfo));
                        zoomMag_rb.Enabled = false;
                        zoomMag_rb.Value = (int)Convert.ToInt32(zoomMagnifierMinValue);
                    }
                    else
                    {
                        enableZoom_btn.BackColor = Color.Yellow;
                        enableZoombtn.SetToolTip(enableZoom_btn, IVLVariables.LangResourceManager.GetString("DisableZoomButton_ToolTipText", IVLVariables.LangResourceCultureInfo));
                        zoomMag_rb.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
                //                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        private void RefreshShowReports(string s , Args arg)
        {
            showExisitingReports();
        }

        private void CreateReportsDataGridView()
        {

        }
        bool isGridPopulating = false;
        /// <summary>
        /// The below code has been added to show the exisiting reports.
        /// </summary>
        public void showExisitingReports()
        {
            try
            {
                if (!isGridPopulating && Reports_dgv.Width > 1)
                {
                    isGridPopulating = true;
                    Reports_dgv.DataSource = new List<report>().ToDataTable();
                    Reports_dgv.ForeColor = Color.Black;
                    //if (NewDataVariables.Reports == null)
                    if (NewDataVariables.Active_Visit.reports != null)
                    {
                        NewDataVariables.Reports = NewDataVariables.Active_Visit.reports.ToList();
                        NewDataVariables.Reports = NewDataVariables.Reports.OrderBy(x => x.createdDate).ToList();
                        NewDataVariables.Reports.Reverse();
                        Reports_dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                        List<string> date = new List<string>();
                        List<string> time = new List<string>();
                        //BindingSource bindingSource = new BindingSource();
                        //bindingSource.DataSource = ;
                        if (NewDataVariables.Reports.Any())
                        {
                            Reports_dgv.DataSource = NewDataVariables.Reports.ToDataTable();
                            if (!Reports_dgv.Columns.Contains(IVLVariables.LangResourceManager.GetString("Report_Date_Text", IVLVariables.LangResourceCultureInfo)) && !Reports_dgv.Columns.Contains(IVLVariables.LangResourceManager.GetString("Report_Time_Text", IVLVariables.LangResourceCultureInfo)) && !Reports_dgv.Columns.Contains(IVLVariables.LangResourceManager.GetString("Report_View_Text", IVLVariables.LangResourceCultureInfo)) && !Reports_dgv.Columns.Contains(IVLVariables.LangResourceManager.GetString("Report_Delete_Text", IVLVariables.LangResourceCultureInfo)))
                            {
                                Reports_dgv.ClearSelection();
                                var col2 = new DataGridViewTextBoxColumn();
                                var col3 = new DataGridViewTextBoxColumn();
                                var col4 = new DataGridViewTextBoxColumn();
                                var col7 = new DataGridViewTextBoxColumn();
                                DataGridViewLinkColumn col5 = new DataGridViewLinkColumn();
                                DataGridViewImageColumn col6 = new DataGridViewImageColumn();
                                col2.HeaderText = IVLVariables.LangResourceManager.GetString("Report_Slno_Text", IVLVariables.LangResourceCultureInfo); ;
                                col2.Name = IVLVariables.LangResourceManager.GetString("Report_Slno_Text", IVLVariables.LangResourceCultureInfo); ;
                                col3.HeaderText = IVLVariables.LangResourceManager.GetString("Report_Date_Text", IVLVariables.LangResourceCultureInfo); ;
                                col3.Name = IVLVariables.LangResourceManager.GetString("Report_Date_Text", IVLVariables.LangResourceCultureInfo); ;
                                col4.HeaderText = IVLVariables.LangResourceManager.GetString("Report_Time_Text", IVLVariables.LangResourceCultureInfo); ;
                                col4.Name = IVLVariables.LangResourceManager.GetString("Report_Time_Text", IVLVariables.LangResourceCultureInfo); ;
                                col7.HeaderText = IVLVariables.LangResourceManager.GetString("CloudReportStatus_Text", IVLVariables.LangResourceCultureInfo); ;
                                col7.Name = IVLVariables.LangResourceManager.GetString("CloudReportStatus_Text", IVLVariables.LangResourceCultureInfo); ;
                                //col5.HeaderText = IVLVariables.LangResourceManager.GetString( "Report_View_Text",IVLVariables.LangResourceCultureInfo);;
                                //col5.Name = IVLVariables.LangResourceManager.GetString( "Report_View_Text",IVLVariables.LangResourceCultureInfo);;
                                {
                                    Reports_dgv.Columns.AddRange(new DataGridViewColumn[] { col2, col3, col4, col7 });
                                }
                                //DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
                                //foreach (DataGridViewRow item in Reports_dgv.Rows)
                                //{
                                //    DataGridViewLinkCell linCell = new DataGridViewLinkCell();
                                //    linCell.Value = "view";
                                //    //col5.Text = linCell;
                                //    item.Cells[IVLVariables.LangResourceManager.GetString( "Report_View_Text] = linCell;
                                //}
                                for (int i = 0; i < Reports_dgv.Columns.Count; i++)
                                {
                                    if (Reports_dgv.Columns[i].Name.Equals(IVLVariables.LangResourceManager.GetString("Report_Time_Text", IVLVariables.LangResourceCultureInfo)) ||
                                        Reports_dgv.Columns[i].Name.Equals(IVLVariables.LangResourceManager.GetString("Report_Date_Text", IVLVariables.LangResourceCultureInfo)) ||
                                        Reports_dgv.Columns[i].Name.Equals(IVLVariables.LangResourceManager.GetString("Report_Slno_Text", IVLVariables.LangResourceCultureInfo)) ||// || Reports_dgv.Columns[i].Name.Equals(IVLVariables.LangResourceManager.GetString("Report_View_Text", IVLVariables.LangResourceCultureInfo)))
                                        Reports_dgv.Columns[i].Name.Equals(IVLVariables.LangResourceManager.GetString("CloudReportStatus_Text", IVLVariables.LangResourceCultureInfo)))// || Reports_dgv.Columns[i].Name.Equals(IVLVariables.LangResourceManager.GetString("Report_View_Text", IVLVariables.LangResourceCultureInfo)))
                                    {
                                        Reports_dgv.Columns[i].Visible = true;
                                    }
                                    else
                                        Reports_dgv.Columns[i].Visible = false;
                                }
                            ResizeReportGridView();
                            }
                            for (int i = 0; i < NewDataVariables.Reports.Count; i++)
                            {
                                Reports_dgv.Rows[i].Cells[IVLVariables.LangResourceManager.GetString("Report_Slno_Text", IVLVariables.LangResourceCultureInfo)].Value = i + 1;
                                CloudAnalysisReport c = null;
                                List<CloudAnalysisReport> cloudReports = new List<CloudAnalysisReport>();
                                if(NewDataVariables.Reports[i].cloudReports != null)
                                cloudReports = NewDataVariables.Reports[i].cloudReports.ToList();
                                if (cloudReports.Any())
                                    c = cloudReports[0];
                                Reports_dgv.Rows[i].Cells[IVLVariables.LangResourceManager.GetString("Report_Date_Text", IVLVariables.LangResourceCultureInfo)].Value = NewDataVariables.Reports[i].createdDate.ToString("dd-MMM-yyyy");
                                if (c != null)
                                {
                                    //if (cloudAnalysisReports.Any())
                                    //{
                                    //    if (cloudAnalysisReports.Where(x => x.cloudAnalysisReportId == c.cloudAnalysisReportId).ToList().Any())
                                    //        c = cloudAnalysisReports.Where(x => x.cloudAnalysisReportId == c.cloudAnalysisReportId).ToList()[0];
                                    //}
                                    CloudReportStatus cloudReportStatus = (CloudReportStatus)c.cloudAnalysisReportStatus;
                                    Reports_dgv.Rows[i].Cells[IVLVariables.LangResourceManager.GetString("CloudReportStatus_Text", IVLVariables.LangResourceCultureInfo)].Value = (cloudReportStatus).ToString("g") + " " + c.failureMessage;
                                }
                                //This code has been added by Darshan on 13-08-2015 7:00 PM to solve Defect no 0000553: Time settings are not reflecting correctly.
                                if (Convert.ToBoolean(IVLVariables.CurrentSettings.UserSettings._Is24clock.val))
                                {
                                    Reports_dgv.Rows[i].Cells[IVLVariables.LangResourceManager.GetString("Report_Time_Text", IVLVariables.LangResourceCultureInfo)].Value = NewDataVariables.Reports[i].createdDate.ToString(" HH:mm ");
                                }
                                else
                                {
                                    Reports_dgv.Rows[i].Cells[IVLVariables.LangResourceManager.GetString("Report_Time_Text", IVLVariables.LangResourceCultureInfo)].Value = NewDataVariables.Reports[i].createdDate.ToString("hh:mm tt");
                                }
                            }
                            //Console.WriteLine(Reports_dgv.Width);
                            //Reports_dgv.Sort(Reports_dgv.Columns[1], ListSortDirection.Ascending);
                            //for (int i = 0; i < Reports_dgv.Columns.Count; i++)
                            //{
                            //    Reports_dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                            //}
                            
                            reportsCreated_lbl.Text = IVLVariables.LangResourceManager.GetString("ImageViewer_ReportsCreated_Label_Text", IVLVariables.LangResourceCultureInfo) + " (" + Reports_dgv.RowCount + ")";
                        }
                        isGridPopulating = false;

                    }
                    else
                        isGridPopulating = false;
                    try
                    {
                        Reports_dgv.Refresh();

                    }
                    catch (Exception ex)
                    {
                        Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
                    }

                }
            }
            catch (IndexOutOfRangeException ex)
            {
                isGridPopulating = false;
                Reports_dgv = new DataGridView();
                showExisitingReports();
                //Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
                //                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
                //                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        private void ResizeReportGridView()
        {
            try
            {
                if (Screen.PrimaryScreen.Bounds.Width == 1920)
                {
                    //Reports_dgv.Columns[IVLVariables.LangResourceManager.GetString( "Report_View_Text",IVLVariables.LangResourceCultureInfo)].Width = 62;
                    Reports_dgv.Columns[IVLVariables.LangResourceManager.GetString("Report_Slno_Text", IVLVariables.LangResourceCultureInfo)].Width = 50;
                    Reports_dgv.Columns[IVLVariables.LangResourceManager.GetString("Report_Date_Text", IVLVariables.LangResourceCultureInfo)].Width = 77;
                    Reports_dgv.Columns[IVLVariables.LangResourceManager.GetString("Report_Time_Text", IVLVariables.LangResourceCultureInfo)].Width = 127;
                    Reports_dgv.Columns[IVLVariables.LangResourceManager.GetString("CloudReportStatus_Text", IVLVariables.LangResourceCultureInfo)].Width = 127;
                    foreach (DataGridViewColumn c in Reports_dgv.Columns)
                    {
                        c.DefaultCellStyle.Font = new Font("Tahoma", 10.5F, GraphicsUnit.Pixel);
                    }
                }
                else if (Screen.PrimaryScreen.Bounds.Width == 1366)
                {
                    //Reports_dgv.Columns[IVLVariables.LangResourceManager.GetString( "Report_View_Text",IVLVariables.LangResourceCultureInfo)].Width = 50;

                    Reports_dgv.Columns[IVLVariables.LangResourceManager.GetString("Report_Date_Text", IVLVariables.LangResourceCultureInfo)].Width = 50;
                    Reports_dgv.Columns[IVLVariables.LangResourceManager.GetString("CloudReportStatus_Text", IVLVariables.LangResourceCultureInfo)].Width = 60;
                    Reports_dgv.Columns[IVLVariables.LangResourceManager.GetString("Report_Time_Text", IVLVariables.LangResourceCultureInfo)].Width = 50;
                    Reports_dgv.Columns[IVLVariables.LangResourceManager.GetString("Report_Slno_Text", IVLVariables.LangResourceCultureInfo)].Width = 30;

                    foreach (DataGridViewColumn c in Reports_dgv.Columns)
                    {
                        c.DefaultCellStyle.Font = new Font("Tahoma", 9.0F, GraphicsUnit.Pixel);
                    }
                }
                else if (Screen.PrimaryScreen.Bounds.Width == 1280)
                {
                    //Reports_dgv.Columns[IVLVariables.LangResourceManager.GetString( "Report_View_Text",IVLVariables.LangResourceCultureInfo)].Width = 50;
                    Reports_dgv.Columns[IVLVariables.LangResourceManager.GetString("Report_Date_Text", IVLVariables.LangResourceCultureInfo)].Width = 61;
                    Reports_dgv.Columns[IVLVariables.LangResourceManager.GetString("Report_Time_Text", IVLVariables.LangResourceCultureInfo)].Width = 57;
                    Reports_dgv.Columns[IVLVariables.LangResourceManager.GetString("CloudReportStatus_Text", IVLVariables.LangResourceCultureInfo)].Width = 57;
                    Reports_dgv.Columns[IVLVariables.LangResourceManager.GetString("Report_Slno_Text", IVLVariables.LangResourceCultureInfo)].Width = 58;
                    foreach (DataGridViewColumn c in Reports_dgv.Columns)
                    {
                        c.DefaultCellStyle.Font = new Font("Tahoma", 9.0F, GraphicsUnit.Pixel);
                    }
                }
            }
            catch (Exception ex)
            {

                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);

            }

        }


        /// <summary>
        /// Save the image to the hard disk and details into the database.
        /// </summary>
        public void save_image()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DirectoryInfo dir_in = null;
                if (IVLVariables.isCommandLineAppLaunch)
                {
                    dir_in = new FileInfo(thumbnailData.fileName).Directory;
                }
                else
                {
                    if (Convert.ToBoolean(IVLVariables.CurrentSettings.ImageStorageSettings.IsMrnFolder.val))
                        dir_in = new DirectoryInfo(IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_PatientIdentifier.value + Path.DirectorySeparatorChar + NewDataVariables.Active_Visit.createdDate.Date.ToString("dd_MM_yyyy"));
                    else
                        dir_in = new DirectoryInfo(IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString());
                }
                if (!dir_in.Exists)
                    dir_in.Create();

                IVLVariables.ivl_Camera.camPropsHelper.SaveImage2Dir(modifyingBm, dir_in.FullName, (ImageSaveFormat)Enum.Parse(typeof(ImageSaveFormat), IVLVariables.CurrentSettings.ImageStorageSettings._ImageSaveFormat.val.ToLower()), Convert.ToInt32(IVLVariables.CurrentSettings.ImageStorageSettings._compressionRatio.val));
                //if (Convert.ToBoolean(IVLVariables.CurrentSettings.ImageStorageSettings._IsPng.val))
                //{
                //    //save_filename = dir_in.FullName + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyyMMddhhmmss") + ".png";
                //    //string[] save_filename_db_array = save_filename.Split('\\');
                //    //save_filename_DB = save_filename_db_array[save_filename_db_array.Length - 1];
                //   // Image<Bgr, byte> tempbm = new Image<Bgr, byte>(modifyingBm);
                //    //Bitmap temp2 = tempbm.ToBitmap();
                //    //temp2.Save(save_filename, ImageFormat.Png);
                //}
                //else
                //{
                //    save_filename = dir_in.FullName + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpg";
                //    string[] save_filename_db_array = save_filename.Split('\\');
                //    save_filename_DB = save_filename_db_array[save_filename_db_array.Length - 1];
                //    modifyingBm.Save(save_filename, ImageFormat.Jpeg);
                //}
                Args arg = new Args();
                if (leftSide_btn.BackColor == Color.Yellow)
                    arg["side"] = 1;
                else
                    arg["side"] = 0;
                //This code has been added to insert user id into the table has to be removed once user or admin has been added and has be replaced by active user.
                //if (!IVLVariables.isCommandLineAppLaunch)
                //{
                //    users user = users.CreateNewUsers();
                //    user.userId = 1;
                //    Concept c = new Concept();
                //    c.conceptId = 3;
                //    obs newObs = obs.CreateNewObs();
                //    newObs.lastModifiedDate = DateTime.Now;
                //    newObs.createdDate = DateTime.Now;
                //    newObs.visit = NewDataVariables.Active_Visit;
                //    newObs.value = IVLVariables.ivl_Camera.camPropsHelper.ImageName;
                //    newObs.concept = c;
                //    newObs.createdBy = user;
                //    machine machine_id = machine.CreateNewMachine();
                //    machine_id.machineId = 1;

                //    eye_fundus_image eyeFundusImage = eye_fundus_image.CreateNewEyeFundusImage();
                //    arg["maskSettings"] = NewDataVariables.Active_EyeFundusImage.maskSetting;
                //    if ((int)arg["side"] == 1)
                //    {
                //        eyeFundusImage.eyeSide = 'L';
                //    }
                //    else
                //    {
                //        eyeFundusImage.eyeSide = 'R';
                //    }
                //    newObs.person = NewDataVariables.Active_Patient;
                //    NewIVLDataMethods.AddImage(newObs);
                //    eyeFundusImage.eyeFundusImageId = newObs.observationId;//This code has been added by darshan since the autoincrement functionality is removed for eye_fundus_image_id in eye_fundus_image.
                //    eyeFundusImage.machine = machine_id;
                //    NewIVLDataMethods.AddEyeFundusImage(eyeFundusImage);
                //    arg["id"] = newObs.observationId;

                //    NewIVLDataMethods.UpdateVisit();

                //    PatientDetais_update();
                //}
                //else
                //{
                //    arg["id"] = IVLVariables.CmdObsID++ ;

                //}
                arg["isModifiedimage"] = true;
                //arg["isannotated"] = false;
                eventHandler.Notify(eventHandler.ImageUrlToDb, arg);
                IVLVariables.isValueChanged = false;
                eventHandler.Notify(eventHandler.ThumbnailSelected, arg);
                ThumbnailUI.isValueChanged = false;

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
                //                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        /// <summary>
        /// Will update the modifieddatetime of patien when image is saved or deleted or updated or when previous operations is performed on reports
        /// </summary>
        public void PatientDetais_update()
        {
            NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].patientLastModifiedDate = DateTime.Now;
            try
            {
                NewDataVariables._Repo.Update<Patient>(NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0]);
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
                //                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        /// <summary>
        /// Will set the selceted images to be stored in the images array variable.
        /// </summary>
        public void noofimages()
        {
            Args arg = new Args();
            arg["isExport"] = false;
            eventHandler.Notify(eventHandler.GetImageFiles, arg);
            images = currentReportLabelNames;
        }

        /// <summary>
        /// Moves the track bar right side when mouse wheel is moved on upside.
        /// </summary>
        public void ZoomRbIncreaseValue()
        {
            try
            {
                #region oldimplementationofzoommagnifiervalue
                #endregion
                if (zoomMag_rb.Value < zoomMag_rb.Maximum)//This if statement has been addedto make sure the ZoomMagifier value doesnoot exceed the ZoomMagnifier Maximiumvalue.
                {
                    if ((int)(zoomMag_rb.Value + Convert.ToSingle(IVLVariables.CurrentSettings.UserSettings._ZoomIncrementFactor.val) * 10) < zoomMag_rb.Maximum)
                        zoomMag_rb.Value = (int)(zoomMag_rb.Value + Convert.ToSingle(IVLVariables.CurrentSettings.UserSettings._ZoomIncrementFactor.val) * 10);
                    else
                        zoomMag_rb.Value = zoomMag_rb.Maximum;
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
                //                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        /// <summary>
        /// Moves the track bar left side when mouse wheel is moved down.
        /// </summary>
        public void ZoomRbDecreaseValue()
        {
            try
            {
                if (zoomMag_rb.Value > zoomMag_rb.Minimum)
                    if ((int)(zoomMag_rb.Value - Convert.ToSingle(IVLVariables.CurrentSettings.UserSettings._ZoomIncrementFactor.val) * 10) > zoomMag_rb.Minimum)
                        zoomMag_rb.Value = (int)(zoomMag_rb.Value - Convert.ToSingle(IVLVariables.CurrentSettings.UserSettings._ZoomIncrementFactor.val) * 10);
                    else
                        zoomMag_rb.Value = zoomMag_rb.Minimum;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
                //                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        /// <summary>
        /// Resets each control to their default value when color image button is clicked.
        /// </summary>
        public void colorimagebutton()
        {
            try
            {
                ResetBrightnessContrastZoom();
                Args arg = new Args();
                arg["rawImage"] = OriginalBm;
                eventHandler.Notify(eventHandler.DisplayImage, arg);
                ZoomModifier_color(true);
                modifyingBm = OriginalBm.Clone() as Bitmap;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
                //                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        /// <summary>
        /// Resets each control to their default value when remove change button clicked image button is clicked.
        /// </summary>
        public void RefreshViewScreenControls()
        {
            try
            {
                if (!noImageSelected_lbl.Visible)
                {
                    SaveChangedImages();
                    if (!IVLVariables.isValueChanged)//to refresh if the value changed is false.
                    {
                        if (CurrentChannelDisplayed != Channels.Color)
                        {
                            string warningText = string.Empty;
                            try
                            {
                                warningText = IVLVariables.LangResourceManager.GetString("filterResetWarning_Text", IVLVariables.LangResourceCultureInfo);

                            }
                            catch (Exception ex)
                            {

                                throw;
                            }
                            CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("ChangeEyeSideOnlyForColorImage_Text", IVLVariables.LangResourceCultureInfo), warningText, CustomMessageBoxButtons.OK, CustomMessageBoxIcon.Information);
                            CurrentChannelDisplayed = Channels.Color;
                        }
                        RGBbackgroundcolor();
                        //This below code is added by Darshan on 21-08-2015 to solve Defect no 0000593: The highlighting should be done on the channel display button.
                        IVLVariables.iscolorChange = false;

                        if (IVLVariables.isZoomEnabled)
                            eventHandler.Notify(eventHandler.EnableZoomMagnification, new Args());
                        //This code has been changed by Darshan on 26-11-2015 to get the zoom magnifier value from settings window.
                        zoomMag_rb.Value = (int)Convert.ToInt32(zoomMagnifierMinValue);
                        IVLVariables.isValueChanged = false;
                        Args arg = new Args();
                        modifyingBm = OriginalBm.Clone(new Rectangle(0, 0, OriginalBm.Width, OriginalBm.Height), PixelFormat.Format24bppRgb);// clone the original bitmap to the modifying bm with 24 bit pixel format.
                        arg["rawImage"] = OriginalBm;
                        eventHandler.Notify(eventHandler.DisplayImage, arg);
                        modifyingBm = OriginalBm.Clone() as Bitmap;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
                //                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        /// <summary>
        /// Checks wheather the annotationxml is CDR or not
        /// </summary>
        /// <param name="AnnotationXml">annotation xml</param>
        public void checkforCDR(string AnnotationXml)
        {
            byte[] bytes = null;
            AnnotationXMLProperties anno = null;

            try
            {
                //bytes = Convert.FromBase64String(AnnotationXml);
                bytes = Encoding.UTF8.GetBytes(AnnotationXml);// chaged froom Base64 to UTF8 since annotation xml did not have closing tags.ByAshutosh 11-08-2017
                using (MemoryStream stream = new MemoryStream(bytes))
                {
                    IFormatter formatter = new BinaryFormatter();
                    anno = (AnnotationXMLProperties)formatter.Deserialize(stream);
                }
            }
            catch (Exception ex)
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

        /// <summary>
        /// Deserialilzes the xml into the type specified
        /// </summary>
        /// <param name="type">Serialized type</param>
        /// <param name="xml">xml to serialized</param>
        /// <returns>deserialized object</returns>
        public static Object Deserialize(Type type, string xml)
        {
            object data = null;
            using (StringReader sr = new StringReader(xml))
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                using (XmlReader xmlReader = XmlReader.Create(sr, settings))
                {
                    {
                        XmlSerializer xmlSer = new XmlSerializer(type);
                        try
                        {
                            data = xmlSer.Deserialize(xmlReader);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// Changes the image name on thumbnail list when a annotation is added to it.
        /// </summary>
        public void annotatedImage()
        {
            try
            {
                Args arg = new Args();
                arg["isannotated"] = NewDataVariables.Active_Obs.annotationsAvailable;
                arg["isCDR"] = NewDataVariables.Active_Obs.cdrAnnotationAvailable;
                arg["id"] = NewDataVariables.Active_Obs.observationId;
                if (NewDataVariables.Active_Obs.eyeSide == 'L')
                    arg["side"] = 1;
                else
                    arg["side"] = 0;
                //the below code has been added by Darshan to solve defect no 0000510: Duplicate numbering in comments if pressed on control key.
                eventHandler.Notify(eventHandler.ChangeThumbnailSide, arg);
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
                //                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }
        #endregion

        #region private menthods
        /// <summary>
        /// Will initializes the vislibility of each controls in the ViewImageControls_UC based on the values from the settings.
        /// </summary>
        private void SetConfigSettings()
        {
            newReport_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._GenerateReportBtnVisible.val);
            newAnnotation_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._GenerateAnnotationBtnVisible.val);
            glaucomaTool_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._GenerateCDRBtnVisible.val);
            rightSide_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._RightLeftVisble.val);
            leftSide_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._RightLeftVisble.val);
            changeEyeSide_lbl.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._RightLeftVisble.val);
            save_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._SaveFunctionVisble.val);
            saveAs_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._SaveAsFunctionVisble.val);
            exportImages_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._ExportFunctionVisble.val);
            brightness_lbl.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._BrightnessFunctionVisble.val);
            brightness_rb.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._BrightnessFunctionVisble.val);
            decreaseBrightness_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._BrightnessFunctionVisble.val);
            increaseBrightness_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._BrightnessFunctionVisble.val);
            contrast_lbl.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._ContrastFunctionVisble.val);
            contrast_rb.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._ContrastFunctionVisble.val);
            decreaseContrast_tbn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._ContrastFunctionVisble.val);
            increaseContrast_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._ContrastFunctionVisble.val);
            //This below line has been commented since contrast_lbl visibility was also getting changed.
            //contrast_lbl.Visible = IVLVariables.CurrentSettings.UISettings.ViewImaging.ZoomFunctionVisble;
            zoomMag_rb.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._ZoomFunctionVisble.val);
            decreseZoom_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._ZoomFunctionVisble.val);
            increaseZoom_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._ZoomFunctionVisble.val);
            ZoomOut_pbx.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._ZoomFunctionVisble.val);
            Zoomin_pbx.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._ZoomFunctionVisble.val);
            enableZoom_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._ZoomFunctionVisble.val);
            removePostProcessing_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._RemovePostProcessingBtnVisible.val);
            postProcessing_lbl.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._PostProcessingLabelVisible.val);
            showChannel_lbl.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._ShowFiltersVisble.val);
            showRedChannel_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._ShowFiltersVisble.val);
            showGreenChannel_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._ShowFiltersVisble.val);
            showBlueChannel_btn.Visible = Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._ShowFiltersVisble.val);
        }

        /// <summary>
        /// Intitializes the text for each lables and button on the view imaging screen from the resources file.
        /// </summary>
        private void InitializeResourceStrings()
        {
            #region Initialize label strings from resources
            reportsCreated_lbl.Text = IVLVariables.LangResourceManager.GetString("ImageViewer_ReportsCreated_Label_Text", IVLVariables.LangResourceCultureInfo);
            showChannel_lbl.Text = IVLVariables.LangResourceManager.GetString("Filter_Text", IVLVariables.LangResourceCultureInfo);
            changeEyeSide_lbl.Text = IVLVariables.LangResourceManager.GetString("ImageViewer_ChangeEyeSide_Label_Text", IVLVariables.LangResourceCultureInfo);
            postProcessing_lbl.Text = IVLVariables.LangResourceManager.GetString("ImageViewer_PostProcessing_Label_Text", IVLVariables.LangResourceCultureInfo);
            brightness_lbl.Text = IVLVariables.LangResourceManager.GetString("ImageViewer_Brightness_Label_Text", IVLVariables.LangResourceCultureInfo);
            contrast_lbl.Text = IVLVariables.LangResourceManager.GetString("ImageViewer_Contrast_Label_Text", IVLVariables.LangResourceCultureInfo);
            newReport_btn.Text = IVLVariables.LangResourceManager.GetString("ImageViewer_GenerateNewReport_Button_Text", IVLVariables.LangResourceCultureInfo);
            glaucomaTool_btn.Text = IVLVariables.LangResourceManager.GetString("GlaucomaTool_Text", IVLVariables.LangResourceCultureInfo);
            newAnnotation_btn.Text = IVLVariables.LangResourceManager.GetString("ImageViewer_AddAnnotaions2Images_Button_Text", IVLVariables.LangResourceCultureInfo);
            file_lbl.Text = IVLVariables.LangResourceManager.GetString("FileLabel_Text", IVLVariables.LangResourceCultureInfo);
            #endregion
            //Not making use of a Dictionary but directly sending it to the tool's text.By Ashutosh  24-7-2017
            #region Initialize Button strings from resources
            newAnnotation_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("ViewImageAddAnnotationToolTipText", IVLVariables.LangResourceCultureInfo);
            newReport_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("ViewImageReportToolTipText", IVLVariables.LangResourceCultureInfo);
            glaucomaTool_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("ViewImageMeasureCDRToolTipText", IVLVariables.LangResourceCultureInfo);


            ToolTip removePostProcessingbtn = new ToolTip();
            removePostProcessingbtn.SetToolTip(removePostProcessing_btn, IVLVariables.LangResourceManager.GetString("RemovePostProcessingButton_ToolTipText", IVLVariables.LangResourceCultureInfo));
            removePostProcessing_btn.Text = IVLVariables.LangResourceManager.GetString("ImageViewer_RemovePostProcessing_Button_Text", IVLVariables.LangResourceCultureInfo);



            showRedChannel_btn.Text = IVLVariables.LangResourceManager.GetString("RedFilter_Text", IVLVariables.LangResourceCultureInfo);
            showGreenChannel_btn.Text = IVLVariables.LangResourceManager.GetString("GreenFilter_Text", IVLVariables.LangResourceCultureInfo);
            showBlueChannel_btn.Text = IVLVariables.LangResourceManager.GetString("BlueFilter_Text", IVLVariables.LangResourceCultureInfo);


            showRedChannel_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("RedFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
            showGreenChannel_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("GreenFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
            showBlueChannel_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("BlueFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);


            leftSide_btn.Text = IVLVariables.LangResourceManager.GetString("ImageViewer_leftSide_Button_Text", IVLVariables.LangResourceCultureInfo);
            rightSide_btn.Text = IVLVariables.LangResourceManager.GetString("ImageViewer_rightSide_Button_Text", IVLVariables.LangResourceCultureInfo);

            decreaseBrightness_btn.Text = IVLVariables.LangResourceManager.GetString("ImageViewer_Decrease_Button_Text", IVLVariables.LangResourceCultureInfo);
            increaseBrightness_btn.Text = IVLVariables.LangResourceManager.GetString("ImageViewer_Increase_Button_Text", IVLVariables.LangResourceCultureInfo);
            decreaseBrightness_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("DecreaseBrightnessButton_ToolTipText", IVLVariables.LangResourceCultureInfo);
            increaseBrightness_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("IncreaseBrightnessButton_ToolTipText", IVLVariables.LangResourceCultureInfo);



            decreaseContrast_tbn.Text = IVLVariables.LangResourceManager.GetString("ImageViewer_Decrease_Button_Text", IVLVariables.LangResourceCultureInfo);
            increaseContrast_btn.Text = IVLVariables.LangResourceManager.GetString("ImageViewer_Increase_Button_Text", IVLVariables.LangResourceCultureInfo);
            decreaseContrast_tbn.ToolTipText = IVLVariables.LangResourceManager.GetString("DecreaseContrastButton_ToolTipText", IVLVariables.LangResourceCultureInfo);
            increaseContrast_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("IncreaseContrastButton_ToolTipText", IVLVariables.LangResourceCultureInfo);


            decreseZoom_btn.Text = IVLVariables.LangResourceManager.GetString("ImageViewer_Decrease_Button_Text", IVLVariables.LangResourceCultureInfo);
            increaseZoom_btn.Text = IVLVariables.LangResourceManager.GetString("ImageViewer_Increase_Button_Text", IVLVariables.LangResourceCultureInfo);
            decreseZoom_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("DecreaseZoomButton_ToolTipText", IVLVariables.LangResourceCultureInfo);
            increaseZoom_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("IncreaseZoomButton_ToolTipText", IVLVariables.LangResourceCultureInfo);
            enableZoom_btn.Text = IVLVariables.LangResourceManager.GetString("ImageViewer_Zoom_Label_Text", IVLVariables.LangResourceCultureInfo);


            enableZoombtn.SetToolTip(enableZoom_btn, IVLVariables.LangResourceManager.GetString("EnableZoomButton_ToolTipText", IVLVariables.LangResourceCultureInfo));


            save_btn.Text = IVLVariables.LangResourceManager.GetString("ImageViewer_Save_Button_Text", IVLVariables.LangResourceCultureInfo);
            saveAs_btn.Text = IVLVariables.LangResourceManager.GetString("ImageViewer_SaveAs_Button_Text", IVLVariables.LangResourceCultureInfo);
            exportImages_btn.Text = IVLVariables.LangResourceManager.GetString("ImageViewer_ExportImages_Button_Text", IVLVariables.LangResourceCultureInfo);



            save_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("ViewImageSave_Button_ToolTipText", IVLVariables.LangResourceCultureInfo);
            saveAs_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("ViewImageSaveAs_Button_ToolTipText", IVLVariables.LangResourceCultureInfo);
            exportImages_btn.ToolTipText = IVLVariables.LangResourceManager.GetString("ViewImageExportImages_Button_ToolTipText", IVLVariables.LangResourceCultureInfo);
            #endregion




            #region custom folder dictionary population
            // added the data to the customFolderData dictionary
            customFolderData.Add("ImageFormat", IVLVariables.CurrentSettings.ImageStorageSettings._ImageSaveFormat.range);
            customFolderData.Add("_compressionRatio", IVLVariables.CurrentSettings.ImageStorageSettings._compressionRatio.range);
            customFolderData.Add("_FolderPathTextboxText", IVLVariables.CurrentSettings.ImageStorageSettings._ExportImagePath.val);
            customFolderData.Add("FolderBrowser_Text", IVLVariables.LangResourceManager.GetString("FolderBrowser_Text", IVLVariables.LangResourceCultureInfo));
            customFolderData.Add("Folder_Text", IVLVariables.LangResourceManager.GetString("Folder_Text", IVLVariables.LangResourceCultureInfo));
            customFolderData.Add("Format_Text", IVLVariables.LangResourceManager.GetString("Format_Text", IVLVariables.LangResourceCultureInfo));
            customFolderData.Add("CompressionRatio_Text", IVLVariables.LangResourceManager.GetString("CompressionRatio_Text", IVLVariables.LangResourceCultureInfo));
            customFolderData.Add("ApplicableForJpg_Text", IVLVariables.LangResourceManager.GetString("ApplicableForJpg_Text", IVLVariables.LangResourceCultureInfo));
            customFolderData.Add("FileName_Text", IVLVariables.LangResourceManager.GetString("FileName_Text", IVLVariables.LangResourceCultureInfo));
            customFolderData.Add("FileNameExample_Text", IVLVariables.LangResourceManager.GetString("FileNameExample_Text", IVLVariables.LangResourceCultureInfo));
            customFolderData.Add("Ok_Text", IVLVariables.LangResourceManager.GetString("Ok_Text", IVLVariables.LangResourceCultureInfo));
            customFolderData.Add("Cancel_Button_Text", IVLVariables.LangResourceManager.GetString("Cancel_Button_Text", IVLVariables.LangResourceCultureInfo));
            customFolderData.Add("Browse_Button_Text", IVLVariables.LangResourceManager.GetString("Browse_Button_Text", IVLVariables.LangResourceCultureInfo));
            customFolderData.Add("SaveAs_Text", IVLVariables.LangResourceManager.GetString("SaveAs_Text", IVLVariables.LangResourceCultureInfo));
            customFolderData.Add("ExportImages_Text", IVLVariables.LangResourceManager.GetString("ExportImages_Text", IVLVariables.LangResourceCultureInfo));
            customFolderData.Add("ExportReport_Text", IVLVariables.LangResourceManager.GetString("ExportReport_Text", IVLVariables.LangResourceCultureInfo));


            customFolderData.Add("FolderPath_Warning_Text", IVLVariables.LangResourceManager.GetString("FolderPath_Warning_Text", IVLVariables.LangResourceCultureInfo));
            customFolderData.Add("FolderPath_Warning_Header", IVLVariables.LangResourceManager.GetString("FolderPath_Warning_Header", IVLVariables.LangResourceCultureInfo));
            customFolderData.Add("SaveAsCompleted_Text", IVLVariables.LangResourceManager.GetString("SaveAsCompleted_Text", IVLVariables.LangResourceCultureInfo));
            customFolderData.Add("SaveAs_Warning_Text", IVLVariables.LangResourceManager.GetString("SaveAs_Warning_Text", IVLVariables.LangResourceCultureInfo));
            customFolderData.Add("SaveAs_Warning_Header", IVLVariables.LangResourceManager.GetString("SaveAs_Warning_Header", IVLVariables.LangResourceCultureInfo));
            customFolderData.Add("FolderPath_Empty_Text", IVLVariables.LangResourceManager.GetString("FolderPath_Empty_Text", IVLVariables.LangResourceCultureInfo));
            customFolderData.Add("OpenFileLocation_Text", IVLVariables.LangResourceManager.GetString("OpenFileLocation_Text", IVLVariables.LangResourceCultureInfo));// adding key to dictionary.ashutosh 24/7 IVLVariables.LangResourceCultureInfo-An object that represents the culture for which the resource is localized.
            customFolderData.Add("OpenFile_Text", IVLVariables.LangResourceManager.GetString("OpenFile_Text", IVLVariables.LangResourceCultureInfo));// adding key to dictionary.
            customFolderData.Add("DirectoryDoesnotExistWarning_Text", IVLVariables.LangResourceManager.GetString("DirectoryDoesnotExistWarning_Text", IVLVariables.LangResourceCultureInfo));//adding keey to the dictionary for directory does not exists text.
            customFolderData.Add("DirectoryDoesnotExist_Header", IVLVariables.LangResourceManager.GetString("DirectoryDoesnotExist_Header", IVLVariables.LangResourceCultureInfo));//adding keey to the dictionary for directory does not exists header.

            #endregion
            //customFolderBrowser.CustomFolderData= customFolderData;
        }

        /// <summary>
        /// Sets the OriginalBm to the image selected in thumbnail lists.
        /// </summary>
        private void Load_Image()
        {
            Common.CommonMethods c = Common.CommonMethods.CreateInstance();
            LoadSaveImage.LoadImage(thumbnailData.fileName, ref OriginalBm);
            if (img != null)
                img.Dispose();
            img = new Image<Bgr, byte>(OriginalBm);
        }

        /// <summary>
        /// Set the modified image on the view imaging screen to the modifyingBm.
        /// </summary>
        private void GetChannelImage()
        {
            try
            {
                if (modifyingBm != null)
                    modifyingBm.Dispose();
                GC.Collect();
                switch (CurrentChannelDisplayed)
                {
                    case Channels.Red:
                        {
                            modifyingBm = img[2].ToBitmap(); // in order to have a place for indexed bitmap only occuring for channel images
                        }
                        break;
                    case Channels.Green:
                        {
                            modifyingBm = img[1].ToBitmap(); // in order to have a place for indexed bitmap only occuring for channel images
                        }
                        break;
                    case Channels.Blue:
                        {
                            modifyingBm = img[0].ToBitmap(); // in order to have a place for indexed bitmap only occuring for channel images
                        }
                        break;
                    case Channels.Color:
                        {
                            modifyingBm = OriginalBm.Clone(new RectangleF(0, 0, OriginalBm.Width, OriginalBm.Height), OriginalBm.PixelFormat);

                        }
                        break;
                    default:
                        {
                            modifyingBm = OriginalBm.Clone(new RectangleF(0, 0, OriginalBm.Width, OriginalBm.Height), OriginalBm.PixelFormat);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
                //                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        /// <summary>
        /// Sends the details to the report module that has to be displayed on the report.
        /// </summary>
        private bool getReportDetails(bool showExistingDRTemplate = false)
        {
            bool retVal = false;
            try
            {
                if (!IVLVariables.isValueChanged)//to get report details only if the value of isValueChanged bool is false by Kishore on 11-10-17.
                {
                    reportDic = new Dictionary<string, object>();
                    string exeLocation = string.Empty;
                    //string[] templateFile = Directory.GetFiles("ReportTemplates");
                    if (reportDic.ContainsKey("$NameOfTheReport"))//checks if key $NameOfTheReport is present .By Ashutosh 17-08-2017
                        reportDic["$NameOfTheReport"] = IVLVariables.CurrentSettings.ReportSettings.FundusReportText.val.ToString();// if present then it's value is replaced.By Ashutosh 17-08-2017
                    else
                        reportDic.Add("$NameOfTheReport", IVLVariables.CurrentSettings.ReportSettings.FundusReportText.val.ToString());// if not present then key and value are added.By Ashutosh 17-08-2017

                    reportDic.Add("Color1", IVLVariables.GradientColorValues.Color1);
                    reportDic.Add("Color2", IVLVariables.GradientColorValues.Color2);
                    reportDic.Add("ForeColor", IVLVariables.GradientColorValues.FontForeColor);
                    reportDic.Add("ColorAngle", IVLVariables.GradientColorValues.ColorAngle);

                    DirectoryInfo d = new DirectoryInfo(IVLVariables.appDirPathName + "ReportTemplates");
                    List<FileInfo> reportTemplates = new List<FileInfo>();
                    if (!d.Exists)
                    {
                        CustomMessageBox.Show("", "", CustomMessageBoxButtons.OK, CustomMessageBoxIcon.Error);
                        retVal = false;
                    }
                    else
                    {
                        retVal = true;
                        reportTemplates = d.GetFiles("*.xml", SearchOption.AllDirectories).ToList<FileInfo>();
                        string[] imageFileNames = null;
                        int[] imgids = null;// new int[NewDataVariables.Obs.Count];
                        int[] imgsides = null; //new int[NewDataVariables.Obs.Count];
                        bool[] imgannotated = null;// new bool[NewDataVariables.Obs.Count];
                        bool[] isCDR = null; //new bool[NewDataVariables.Obs.Count];
                        string[] maskDetailsOfImages = null;//maskDetailsOfImages of string array type set to null.By Ashutosh 22-08-2017
                        string[] cameraDetailsOfImages = null;//cameraDetailsOfImages of string array type set to null.By Ashutosh 31-08-2017
                        #region Report From Application
                        if (!IVLVariables.isCommandLineArgsPresent)
                        {
                            NewDataVariables.Obs = NewDataVariables.Obs.Where(x => x.voided == false).ToList();
                            imageFileNames = new string[NewDataVariables.Obs.Count];

                            imgids = new int[NewDataVariables.Obs.Count];
                            imgsides = new int[NewDataVariables.Obs.Count];
                            imgannotated = new bool[NewDataVariables.Obs.Count];
                            isCDR = new bool[NewDataVariables.Obs.Count];
                            maskDetailsOfImages = new string[NewDataVariables.Obs.Count];//object maskDetailsOfImages created of type string (which holds Elements present in list Obs). By Ashutosh 22-08-2017
                            cameraDetailsOfImages = new string[NewDataVariables.Obs.Count];//object maskDetailsOfImages created of type stringcameraSetting.By Ashutosh 31-08-2017.
                            for (int i = 0; i < NewDataVariables.Obs.Count; i++)
                            {
                                if (Convert.ToBoolean(IVLVariables.CurrentSettings.ImageStorageSettings.IsMrnFolder.val))
                                    imageFileNames[i] = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_PatientIdentifier.value + Path.DirectorySeparatorChar + NewDataVariables.Active_Visit.createdDate.Date.ToString("dd_MM_yyyy") + Path.DirectorySeparatorChar + NewDataVariables.Obs[i].value;
                                else
                                    imageFileNames[i] = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Obs[i].value;
                                imgids[i] = NewDataVariables.Obs[i].observationId;
                                eye_fundus_image eyefi = NewDataVariables._Repo.GetById<eye_fundus_image>(NewDataVariables.Obs[i].observationId);
                                if (eyefi.eyeSide == 'L')
                                    imgsides[i] = 1;
                                else if (eyefi.eyeSide == 'R')
                                    imgsides[i] = 0;
                                imgannotated[i] = eyefi.annotationsAvailable;
                                isCDR[i] = eyefi.cdrAnnotationAvailable;
                                maskDetailsOfImages[i] = eyefi.maskSetting;//string maskSetting given to string array maskDetailsOfImages. By Ashutosh 22-08-2017
                                cameraDetailsOfImages[i] = eyefi.cameraSetting;//string cameraSetting given to string array maskDetailsOfImages. By Ashutosh 31-08-2017
                            }
                            Patient p = NewDataVariables.GetCurrentPat();
                            reportDic.Add("$FirstName", p.firstName);
                            reportDic.Add("$LastName", p.lastName);

                            reportDic.Add("$PhoneNumber", p.primaryPhoneNumber);
                            reportDic.Add("$emailID", p.primaryEmailId);
                            reportDic.Add("$dob", p.birthdate);
                            reportDic.Add("$Name", IVLVariables.patName);
                            reportDic.Add("$Age", IVLVariables.patAge.ToString());
                            reportDic.Add("$MRN", IVLVariables.MRN);
                            reportDic.Add("$Gender", IVLVariables.patGender);
                            reportDic.Add("$PatID", IVLVariables.ActivePatID);
                            reportDic.Add("$VisitID", IVLVariables.ActiveVisitID);
                            EmailsData mailData = new EmailsData();
                            mailData.EmailTo = IVLVariables.CurrentSettings.EmailSettings.EmailToList.val;
                            mailData.EmailReplyTo = IVLVariables.CurrentSettings.EmailSettings.EmailReplyToList.val;

                            //mailData.EmailTo = "sriram@intuvisionlabs.com;"; 
                            //mailData.EmailReplyTo = "sriram@gmail.com;";
                            mailData.EmailCC = IVLVariables.CurrentSettings.EmailSettings.EmailCCList.val;
                            mailData.EmailBCC = IVLVariables.CurrentSettings.EmailSettings.EmailBCCList.val;
                            reportDic.Add("$EmailData", mailData);
                            reportDic.Add("$showEmailDialog", Convert.ToBoolean(IVLVariables.CurrentSettings.EmailSettings.ShowEmailWindow.val));
                            reportDic.Add("$Doctor", IVLVariables.CurrentSettings.UserSettings._DoctorName.val);
                            reportDic.Add("$visitDateTime", NewDataVariables.Active_Visit.createdDate);
                            if (!IVLReport.Report.isNew)
                            {
                                #region//to fix defect 0001594- the saved report was displaying today's date , added this to display date of creation.By Ashutosh 27-07-2017.


                                if (reportDic.ContainsKey("$Datetime")) // if the $Datetime is already present check
                                    reportDic["$Datetime"] = ReportCreatedDateTime.ToString("dd-MMM-yy");// report created date updated to report dictionary
                                else
                                    reportDic.Add("$Datetime", ReportCreatedDateTime.ToString("dd-MMM-yy"));// report created date added to report dictionary
                            }
                            else
                            {
                                reportDic.Add("$Datetime", DateTime.Today.ToString("dd-MMM-yy"));///The date format has been changed to maintain a uniform date format
                            }


                            #endregion

                            reportDic.Add("$MedHistory", string.Empty);
                            reportDic.Add("$Comments", string.Empty);
                            reportDic.Add("$LeftEyeObs", string.Empty);
                            reportDic.Add("$RightEyeObs", string.Empty);
                            reportDic.Add("$HospitalName", IVLVariables.CurrentSettings.UserSettings._HeaderText.val);
                            reportDic.Add("$Address1", IVLVariables.CurrentSettings.ReportSettings.Address1.val);
                            reportDic.Add("$Address2", IVLVariables.CurrentSettings.ReportSettings.Address2.val);
                            reportDic.Add("$isTelemedReport", Convert.ToBoolean(IVLVariables.CurrentSettings.ReportSettings.IsTelemedReport.val));
                            reportDic.Add("$HospitalLogo", @"ImageResources\LogoImageResources\hospitalLogo.png");

                            reportDic.Add("$MaskSettings", maskDetailsOfImages);//To add the mask settings to the dictionary added by kishore 12 september 2017.
                            reportDic.Add("$CameraSettings", cameraDetailsOfImages);//To add the camera settings to the dictionary added by kishore 12 september 2017.

                        }
                        #endregion

                        #region Report From CommandLineArguements
                        else
                        {
                            reportDic.Add("$isTelemedReport", Convert.ToBoolean(IVLVariables.CurrentSettings.ReportSettings.IsTelemedReport.val));

                            imgsides = IVLVariables.patDetails.ImageSideList.ToArray();
                            imgannotated = IVLVariables.patDetails.isAnnotatedList.ToArray();
                            isCDR = IVLVariables.patDetails.isCDRList.ToArray();
                            imgids = IVLVariables.patDetails.ImageIDList.ToArray();
                            reportDic.Add("$Name", IVLVariables.patDetails.FirstName + " " + IVLVariables.patDetails.LastName);
                            reportDic.Add("$Age", IVLVariables.patDetails.Age);
                            reportDic.Add("$MRN", IVLVariables.patDetails.MRN);
                            reportDic.Add("$Gender", IVLVariables.patDetails.Gender);
                            reportDic.Add("$Doctor", IVLVariables.patDetails.ReporteeName);
                            reportDic.Add("$MedHistory", IVLVariables.patDetails.MedHistory);
                            reportDic.Add("$HospitalName", IVLVariables.patDetails.HospitalName);
                            reportDic.Add("$Address1", IVLVariables.patDetails.Address1);
                            reportDic.Add("$Address2", IVLVariables.patDetails.Address2);
                            reportDic.Add("$MaskSettings", IVLVariables.patDetails.MaskSettings.ToArray());
                            reportDic.Add("$CameraSettings", IVLVariables.patDetails.CameraSettings.ToArray());
                            IVLVariables.mailData.Subject = "Diabetic Retinopathy(DR) Screening Report Submitted by " + IVLVariables.CurrentSettings.UserSettings._DoctorName.val + " for " +
                               IVLVariables.patDetails.MRN + "/" + IVLVariables.patDetails.FirstName + " " + IVLVariables.patDetails.LastName + "/" + IVLVariables.patDetails.Age.ToString() + "/" + IVLVariables.patDetails.Gender + " Visit Date : " + IVLVariables.patDetails.VisitDateTime.ToString("dd MMM yy hh:mm tt ");
                            IVLVariables.mailData.Body = "Dear " + IVLVariables.patDetails.ReporteeName + "," + Environment.NewLine + "\n\n" +
                                "Patient Details " + Environment.NewLine +
                                "Name :" + IVLVariables.patDetails.FirstName + " " + IVLVariables.patDetails.LastName + Environment.NewLine +
                                "MRN :" + IVLVariables.patDetails.MRN + Environment.NewLine +
                                 "Age :" + IVLVariables.patDetails.Age.ToString() + Environment.NewLine +
                                 "Gender :" + IVLVariables.patDetails.Gender + Environment.NewLine + Environment.NewLine +
                                 "\n Please find the attached DR Screening Report " + Environment.NewLine + Environment.NewLine +
                                 "Regards," + Environment.NewLine +
                                 IVLVariables.CurrentSettings.UserSettings._DoctorName.val + Environment.NewLine +
                                 IVLVariables.CurrentSettings.UserSettings._HeaderText.val;

                            reportDic.Add("$EmailData", IVLVariables.mailData);
                            reportDic.Add("$Comments", IVLVariables.observationDic.ElementAt(IVLVariables.observationDic.Count - 1).Value);
                            reportDic.Add("$LeftEyeObs", IVLVariables.observationDic.ElementAt(IVLVariables.observationDic.Count - 2).Value);
                            reportDic.Add("$RightEyeObs", IVLVariables.observationDic.ElementAt(IVLVariables.observationDic.Count - 3).Value);

                            //to get the hospital logo from the batch file if the file exists added by kishore on 12 september 2017.
                            string hospitalLogoPath = @IVLVariables.batchFilePath + Path.DirectorySeparatorChar + "hospitalLogo.png";
                            if (File.Exists(hospitalLogoPath))
                                reportDic.Add("$HospitalLogo", hospitalLogoPath);

                            string templatePath = @IVLVariables.batchFilePath + Path.DirectorySeparatorChar + "template.xml";
                            reportDic.Add("$Datetime", DateTime.Today.ToString("dd-MMM-yy"));//The date format has been changed to maintain a uniform date format.
                            if (File.Exists(templatePath))
                            {
                                reportDic.Add("$currentTemplate", templatePath);
                            }
                        }
                        #endregion

                        reportDic.Add("$appDirPath", IVLVariables.appDirPathName);

                        reportDic.Add("$visitImages", imageFileNames);
                        reportDic.Add("$ReportPatientMedicalHistory", IVLVariables.LangResourceManager.GetString("ReportPatientMedicalHistory", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$RightEyeObservationText", IVLVariables.LangResourceManager.GetString("RightEyeObservationText", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$LeftEyeObservationText", IVLVariables.LangResourceManager.GetString("LeftEyeObservationText", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$GeneralObservationText", IVLVariables.LangResourceManager.GetString("GeneralObservationText", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$visitImageIds", imgids);
                        reportDic.Add("$visitImageSides", imgsides);
                        reportDic.Add("$allTemplateFiles", reportTemplates);
                        reportDic.Add("$CurrentTemplateName", IVLVariables.CurrentSettings.ReportSettings.DefaultTemplate.val);

                        reportDic.Add("$CurrentTemplateSize", IVLVariables.CurrentSettings.ReportSettings.ReportSize.val);
                        reportDic.Add("$CmdArgsPresent", IVLVariables.isCommandLineArgsPresent);
                        reportDic.Add("$imgannotated", imgannotated);
                        reportDic.Add("$isCDR", isCDR);
                        reportDic.Add("$DeviceID", IVLVariables.CurrentSettings.CameraSettings.DeviceID.val);
                        reportDic.Add("$hospitalName", IVLVariables.CurrentSettings.UserSettings._HeaderText.val);
                        reportDic.Add("$QRCode", string.Empty);
                        reportDic.Add("$ReportURL", string.Empty);

                        reportDic.Add("$userName", IVLVariables.CurrentSettings.ReportSettings.UserName.val);
                        reportDic.Add("$password", IVLVariables.CurrentSettings.ReportSettings.Password.val);
                        reportDic.Add("$apiRequestType", IVLVariables.CurrentSettings.ReportSettings.ApiRequestType.val);

                        reportDic.Add("$isFromCDR", false);//isFromCDR is added to make the report to know from where it is being invoked.
                        //reportDic.Add("$date", DateTime.Now.ToString("dd-MM-yyyy"));
                        //reportDic.Add("$Datetime", DateTime.Today.ToString("dd-MMM-yy"));//The date format has been changed to maintain a uniform date format.
                        reportDic.Add("$adobereadernotpresent", IVLVariables.LangResourceManager.GetString("Adobe_acrobat_Text", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$NoImages", IVLVariables.LangResourceManager.GetString("NoImagesSelected_Label_Text", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$ReportedBy", IVLVariables.LangResourceManager.GetString("Reported_By_Text", IVLVariables.LangResourceCultureInfo));
                        //This code has been added to send report message and header text for report message box.
                        reportDic.Add("$reportMessageBoxText", IVLVariables.LangResourceManager.GetString("Report_Save_Message_Text", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$imageDeletedMessageBoxText", IVLVariables.LangResourceManager.GetString("Image_Deleted", IVLVariables.LangResourceCultureInfo));//By Ashutosh 05-09-2018.
                        reportDic.Add("$errorMessageTitle", IVLVariables.LangResourceManager.GetString("ErrorMessage_Text", IVLVariables.LangResourceCultureInfo));//By Ashutosh 05-09-2018.
                        reportDic.Add("$size_Lbl_Text", IVLVariables.LangResourceManager.GetString("Size_Lbl_Text", IVLVariables.LangResourceCultureInfo));//By Ashutosh 05-09-2018.
                        reportDic.Add("$orientation_Lbl_Text", IVLVariables.LangResourceManager.GetString("Orientation_Lbl_Text", IVLVariables.LangResourceCultureInfo));//By Ashutosh 05-09-2018.
                        reportDic.Add("$file_Gbx_Text", IVLVariables.LangResourceManager.GetString("FileLabel_Text", IVLVariables.LangResourceCultureInfo));//By Ashutosh 05-09-2018.
                        reportDic.Add("$email_Gbx_Text", IVLVariables.LangResourceManager.GetString("Organization_Email_Property_Text", IVLVariables.LangResourceCultureInfo));//By Ashutosh 05-09-2018.

                        reportDic.Add("$reportMessageBoxHeader", IVLVariables.LangResourceManager.GetString("Report_Save_Header_Text", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$warningMessageWhenReportPdfFileIsOpen", IVLVariables.LangResourceManager.GetString("ReporttPdfFileOpenMessage_Text", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$warningHeaderWhenReportPdfFileIsOpen", IVLVariables.LangResourceManager.GetString("ReporttPdfFileOpenHeader_Text", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$portraitText", IVLVariables.LangResourceManager.GetString("Portrait_Text", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$landscapeText", IVLVariables.LangResourceManager.GetString("Landscape_Text", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$reportText", IVLVariables.LangResourceManager.GetString("Report_text", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$reportSaveText", IVLVariables.LangResourceManager.GetString("ImageViewer_Save_Button_Text", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$reportPrintText", IVLVariables.LangResourceManager.GetString("Print_Btn_Text", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$reportNameText", IVLVariables.LangResourceManager.GetString("Report_Name_Text", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$reportGenderText", IVLVariables.LangResourceManager.GetString("Report_Gender_Text", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$reportAgeText", IVLVariables.LangResourceManager.GetString("Report_Age_Text", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$reportCommentsText", IVLVariables.LangResourceManager.GetString("ReportPatientMedicalHistory", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$reportDateText", IVLVariables.LangResourceManager.GetString("Report_DateTemplate_Text", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$reportDoctorText", IVLVariables.LangResourceManager.GetString("Report_Doctor_Text", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$reportMaxCharsText", IVLVariables.LangResourceManager.GetString("ReportMax_Charecters", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$reportImagesText", IVLVariables.LangResourceManager.GetString("Report_Images_Text", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$reportImagesSizeText", IVLVariables.LangResourceManager.GetString("Report_Image_Size_Text", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$reportImagesOkBtnText", IVLVariables.LangResourceManager.GetString("Ok_Text", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$reportImagesWarningMsgText", IVLVariables.LangResourceManager.GetString("Report_Image_WarningMsg_Text", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$reportImagesWarningHeaderText", IVLVariables.LangResourceManager.GetString("Report_Image_WarningHeader_Text", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$exportReportText", IVLVariables.LangResourceManager.GetString("Export_Report_Text", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$exportReportHeader", IVLVariables.LangResourceManager.GetString("Export_Button_Text", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$customFolderData", customFolderData); //to add the custom folder data dictionary to the report dictionary


                        //reportDic.Add("$Is2ImagesLS4ImagesPOR", Convert.ToBoolean(IVLVariables.CurrentSettings.ReportSettings.Is2ImagesLS4ImagesPOR.val));//Added to change landscape or portrzit for 2 and 4 images.



                        Dictionary<string, object> emailDic = new Dictionary<string, object>();
                        emailDic.Add("$emailSentText", IVLVariables.LangResourceManager.GetString("EmailSentText", IVLVariables.LangResourceCultureInfo));
                        emailDic.Add("$emailFromAddressInvalidText", IVLVariables.LangResourceManager.GetString("EmailFromAddressInvalidText", IVLVariables.LangResourceCultureInfo));
                        emailDic.Add("$emailToAddressInvalidText", IVLVariables.LangResourceManager.GetString("EmailToAddressInvalidText", IVLVariables.LangResourceCultureInfo));
                        emailDic.Add("$emailCancelledText", IVLVariables.LangResourceManager.GetString("EmailCancelledText", IVLVariables.LangResourceCultureInfo));
                        emailDic.Add("$emailErrorText", IVLVariables.LangResourceManager.GetString("EmailErrorText", IVLVariables.LangResourceCultureInfo));
                        emailDic.Add("$retryAgainText", IVLVariables.LangResourceManager.GetString("RetryAgainText", IVLVariables.LangResourceCultureInfo));
                        emailDic.Add("$rightEyeObservationsText", IVLVariables.LangResourceManager.GetString("RightEyeObservationsText", IVLVariables.LangResourceCultureInfo));
                        emailDic.Add("$leftEyeObservationsText", IVLVariables.LangResourceManager.GetString("LeftEyeObservationsText", IVLVariables.LangResourceCultureInfo));
                        emailDic.Add("$observationSpacingText", IVLVariables.LangResourceManager.GetString("ObservationSpacingText", IVLVariables.LangResourceCultureInfo));
                        emailDic.Add("$generalObservationsText", IVLVariables.LangResourceManager.GetString("GeneralObservationsText", IVLVariables.LangResourceCultureInfo));
                        emailDic.Add("$emailSemicolonNotPresentText", IVLVariables.LangResourceManager.GetString("EmailSemicolonNotPresentText", IVLVariables.LangResourceCultureInfo));
                        emailDic.Add("$emailEmptyText", IVLVariables.LangResourceManager.GetString("EmailEmptyText", IVLVariables.LangResourceCultureInfo));


                        reportDic.Add("EmailDic", emailDic);



                        if (!reportDic.ContainsKey("$currentTemplate"))
                        {
                            #region Gets the default template from the report settings and set it to the current template

                            if (showExistingDRTemplate)
                                reportDic.Add("$currentTemplate", @"ReportTemplates\Landscape\DR\Portrait_DR_A4.xml");
                            else
                            {
                                string defaultTemplateFileName = IVLVariables.CurrentSettings.ReportSettings.DefaultTemplate.val + "_" + IVLVariables.CurrentSettings.ReportSettings.ReportSize.val + ".xml";
                                //reportDic.Add("$currentTemplate", @"ReportTemplates\Landscape\DR\Landscape_DR_A4.xml");
                                int index = reportTemplates.FindIndex(x => x.Name == defaultTemplateFileName);//Gives the index of the changedTemplateFileName from reportTemplates
                                if (index >= 0)
                                {
                                    reportDic.Add("$currentTemplate", reportTemplates[index].FullName);
                                }
                                else
                                {
                                    reportDic.Add("$currentTemplate", @"ReportTemplates\Landscape\DR\Landscape_DR_A4.xml");
                                }
                            }
                            
                            #endregion
                        }
                        else
                        {
                            if (showExistingDRTemplate)
                                reportDic["$currentTemplate"]= @"ReportTemplates\Landscape\DR\Portrait_DR_A4.xml";
                        }
                        if (reportDic.ContainsKey("$ChangeMaskColour"))//checks if key $ChangeMaskColour is present .By Ashutosh 22-08-2017
                            reportDic["$ChangeMaskColour"] = IVLVariables.CurrentSettings.ReportSettings.ChangeMaskColour.val;// if present then it's value is replaced.By Ashutosh 22-08-2017
                        else
                            reportDic.Add("$ChangeMaskColour", IVLVariables.CurrentSettings.ReportSettings.ChangeMaskColour.val);// if not present then key and value are added.By Ashutosh 22-08-2017
                        reportDic.Add("$Specalist", IVLVariables.CurrentSettings.ReportSettings.ReporterName.val);
                        reportDic.Add("$SpecalistQualification", IVLVariables.CurrentSettings.ReportSettings.ReporterQualification.val);
                        reportDic.Add("$SpecalistHospital", IVLVariables.CurrentSettings.ReportSettings.ReporterHospital.val);
                        reportDic.Add("$showPrintButton", Convert.ToBoolean(IVLVariables.CurrentSettings.ReportSettings.ShowPrintButton.val));
                        reportDic.Add("$showSaveButton", Convert.ToBoolean(IVLVariables.CurrentSettings.ReportSettings.ShowSaveButton.val));
                        reportDic.Add("$showExportButton", Convert.ToBoolean(IVLVariables.CurrentSettings.ReportSettings.ShowExportButton.val));
                        reportDic.Add("$showExportButtonText", (IVLVariables.LangResourceManager.GetString("Export_Button_Text", IVLVariables.LangResourceCultureInfo)));
                        reportDic.Add("$showEmailImagesButton", Convert.ToBoolean(IVLVariables.CurrentSettings.ReportSettings.ShowEmailImagesButton.val));
                        reportDic.Add("$showEmailReportButton", Convert.ToBoolean(IVLVariables.CurrentSettings.ReportSettings.ShowEmailReportButton.val));
                        reportDic.Add("$showEmailTelemedButton", Convert.ToBoolean(IVLVariables.CurrentSettings.ReportSettings.ShowEmailTelemedButton.val));
                        reportDic.Add("$showAutoAnalysisButton", Convert.ToBoolean(IVLVariables.CurrentSettings.ReportSettings.ShowAutoAnalysisButton.val));
                        reportDic.Add("$showEmailImagesButtonText", (IVLVariables.LangResourceManager.GetString("NoOfImagesToBeSelected_Text2", IVLVariables.LangResourceCultureInfo)));
                        reportDic.Add("$showEmailReportButtonText", (IVLVariables.LangResourceManager.GetString("Report_Image_WarningHeader_Text", IVLVariables.LangResourceCultureInfo)));
                        reportDic.Add("$showEmailTelemedButtonText", (IVLVariables.LangResourceManager.GetString("TelemedUpload_Button_Text", IVLVariables.LangResourceCultureInfo)));
                        reportDic.Add("$showGenObs", Convert.ToBoolean(IVLVariables.CurrentSettings.ReportSettings.ShowGenObs.val));
                        reportDic.Add("$showRightObs", Convert.ToBoolean(IVLVariables.CurrentSettings.ReportSettings.ShowRightObs.val));
                        reportDic.Add("$showLeftObs", Convert.ToBoolean(IVLVariables.CurrentSettings.ReportSettings.ShowLeftObs.val));
                        reportDic.Add("$NoOfImagesAllowed", IVLVariables.CurrentSettings.UserSettings._noOfImagesForReport.val);
                        reportDic.Add("$NoOfImagesAllowedText1", IVLVariables.LangResourceManager.GetString("NoOfImagesToBeSelected_Text1", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$NoOfImagesAllowedText2", IVLVariables.LangResourceManager.GetString("NoOfImagesToBeSelected_Text2", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$NoOfImagesAllowedHeader", IVLVariables.LangResourceManager.GetString("NoOfImagesToBeSelected_Header", IVLVariables.LangResourceCultureInfo));

                        reportDic.Add("$FundusReport", IVLVariables.CurrentSettings.ReportSettings.FundusReportText.val);

                        reportDic.Add("$isAnnotation", false);
                        reportDic.Add("$reportNewStatus", IVLVariables.LangResourceManager.GetString("ReportStatusNew_Label_Text", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$reportExistingStatus", IVLVariables.LangResourceManager.GetString("ReportStatusExisting_Label_Text", IVLVariables.LangResourceCultureInfo));
                        reportDic.Add("$CompanyLogo", @"ImageResources\LogoImageResources\CompanyLogo.png");
                        reportDic.Add("$Signature", @"ImageResources\LogoImageResources\signature.jpg");
                        Args arg = new Args();
                        arg["isExport"] = false;
                        if (!IVLVariables.isCommandLineArgsPresent)
                        {
                            eventHandler.Notify(eventHandler.GetImageFiles, arg);
                        }
                        else
                        {
                            reportDic["$visitImages"] = IVLVariables.patDetails.observationPaths.ToArray();
                            currentReportImageFiles = IVLVariables.patDetails.observationPaths.ToArray();
                            currentReportLabelNames = IVLVariables.patDetails.ImageNames.ToArray();
                        }
                        string[] image = currentReportLabelNames;
                        Array.Reverse(currentReportLabelNames);
                        Array.Reverse(image);
                        reportDic.Add("$CurrentImageFiles", currentReportImageFiles);
                        reportDic.Add("$ImageNames", image);
                        reportDic.Add("$Color1", IVLVariables.GradientColorValues.Color1);
                        reportDic.Add("$Color2", IVLVariables.GradientColorValues.Color2);
                        reportDic.Add("$FontColor", IVLVariables.GradientColorValues.FontForeColor);
                        reportDic.Add("$ColorAngle", IVLVariables.GradientColorValues.ColorAngle);
                        _report = new IVLReport.Report(reportDic);
                        //#if DEBUG
                        //            _report.TopMost = false;
                        //#else
                        //                _report.TopMost = true ;
                        //#endif
                        _report.reportSavedEvent += _report_reportSavedEvent;
                        _report.reportClosedEvent += _report_reportClosedEvent;
                        _report.reportDefaultCursor += _report_reportDefaultCursor;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);

                //                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
            return retVal;
        }

        /// <summary>
        /// To turn on the trigger when the report is closed.
        /// </summary>
        void _report_reportClosedEvent()
        {
            IVLVariables.IsAnotherWindowOpen = false;
        }

        void _report_reportDefaultCursor()
        {
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Assigns the modifyingBm to the bitmap on viewimaging screen.
        /// </summary>
        private void changeDisplayBitmap()
        {
            Args arg = new Args();
            arg["displayBitmap"] = modifyingBm;
            eventHandler.Notify(eventHandler.ChangedDisplayBitmap, arg);
        }

        /// <summary>
        /// Will apply the changed brightness and contrast value to the image in view imaging screen.
        /// </summary>
        private void ChangeBrightnessContrast()
        {
            try
            {
                string ImagePath = string.Empty;
                if (IVLVariables.isCommandLineAppLaunch)
                    ImagePath = thumbnailData.fileName;
                else
                    if (Convert.ToBoolean(IVLVariables.CurrentSettings.ImageStorageSettings.IsMrnFolder.val))
                    ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_PatientIdentifier.value + Path.DirectorySeparatorChar + NewDataVariables.Active_Visit.createdDate.Date.ToString("dd_MM_yyyy") + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
                else
                    ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;

                if (!noImageSelected_lbl.Visible && File.Exists(ImagePath))
                {
                    IVLVariables.isValueChanged = true;
                    ThumbnailUI.isValueChanged = true;
                    //Bitmap sampleBmp = modifyingBm;
                    GetChannelImage();
                    if (IVLVariables.postprocessingHelper == null)
                        IVLVariables.postprocessingHelper = PostProcessing.GetInstance();
                    IVLVariables.postprocessingHelper.AdjustBrightness(modifyingBm, (int)brightness_rb.Value, (int)contrast_rb.Value, ref modifyingBm);
                    Args arg = new Args();
                    arg["rawImage"] = modifyingBm;
                    eventHandler.Notify(eventHandler.DisplayImage, arg);
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
                //                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        /// <summary>
        /// To reset the brightness, contrast and zoom values.
        /// </summary>
        private void ResetBrightnessContrastZoom()
        {
            brightness_rb.Value = 0;
            contrast_rb.Value = 1;
            zoomMag_rb.Value = (int)Convert.ToInt32(zoomMagnifierMinValue); ;
            IVLVariables.isValueChanged = false;

        }

        /// <summary>
        /// Decreases the zoomfactor based on the zoomMag_rb value.
        /// </summary>
        private void decreaseZoom()
        {
            if (!noImageSelected_lbl.Visible)
            {
                IVLVariables.zoomFactor = (Convert.ToSingle(IVLVariables.CurrentSettings.UserSettings._ZoomScale.val) * zoomMag_rb.Value);
                if (IVLVariables.zoomFactor > Convert.ToSingle(zoomMagnifierMaxValue) * Convert.ToSingle(IVLVariables.CurrentSettings.UserSettings._ZoomScale.val))
                    IVLVariables.zoomFactor = IVLVariables.zoomMax;
            }
        }

        /// <summary>
        /// Increases the zoom factor value.
        /// </summary>
        private void increaseZoom()
        {
            try
            {
                if (!noImageSelected_lbl.Visible)
                {
                    float zoomVal = zoomMag_rb.Value;
                    IVLVariables.zoomFactor = (Convert.ToSingle(IVLVariables.CurrentSettings.UserSettings._ZoomScale.val) * zoomVal);
                    if (IVLVariables.zoomFactor < Convert.ToSingle(zoomMagnifierMinValue) * Convert.ToSingle(IVLVariables.CurrentSettings.UserSettings._ZoomScale.val))
                        IVLVariables.zoomFactor = IVLVariables.zoomMin;
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
                //                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }
        #endregion

        #region private events
        /// <summary>
        /// Invokes the ChangeBrightnessContrast method when brightness value is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void brightness_rb_ValueChanged(object sender, EventArgs e)
        {
            noofimages();
            if (images.Length > 1)
            {
                brightness_rb.Value = 0;
                //CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("AnnotationNo_Images", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("ImageViewer_Brightness_Label_Text", IVLVariables.LangResourceCultureInfo).TrimEnd(':'), CustomMessageBoxIcon.Warning);
            }
            else
            {
                ChangeBrightnessContrast();
                scrollToolTip.SetToolTip(brightness_rb, brightness_rb.Value.ToString());
            }
        }

        /// <summary>
        /// This event will refresh the backgroud color of the channels button in view imaging screen.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="arg"></param>
        private void rgbcolorchange(string s, Args arg)
        {
            CurrentChannelDisplayed = Channels.Color;
            RGBbackgroundcolor();
        }

        /// <summary>
        /// This event will get the details of the image selected in view imaging screen.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="arg"></param>
        private void getImageFilesFromThumbnails(string s, Args arg)
        {
            currentReportImageFiles = (string[])arg["FileNameArr"];
            currentReportLabelNames = arg["ImageLabelText"] as string[];
        }

        /// <summary>
        /// Will loads the image selected in the thumbnail list on the screen.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="arg"></param>
        private void LoadImageFromFile(string s, Args arg)
        {
            try
            {
                int side = 0;
                thumbnailData = arg["ThumbnailData"] as ThumbnailData;
                if (!string.IsNullOrEmpty(thumbnailData.fileName) && File.Exists(thumbnailData.fileName))
                {
                    noImageSelected_lbl.Visible = false;
                    this.Enabled = true;

                    int id = thumbnailData.id;
                    if (IVLVariables.isCommandLineAppLaunch)
                    {
                        FileInfo finf = new FileInfo(thumbnailData.fileName);
                        string[] str = finf.Name.Split('_');
                        if (str[0] == "OS")
                            thumbnailData.side = 1;
                        else
                            thumbnailData.side = 0;
                    }
                    else
                    {
                        NewDataVariables.Active_Obs = NewDataVariables.Obs[NewDataVariables.Obs.FindIndex(x => x.observationId == id)];
                        if (NewDataVariables.Active_Obs.eyeSide == 'L')
                            thumbnailData.side = 1;
                        else
                            thumbnailData.side = 0;

                    }

                    if (thumbnailData.side == 1)
                    {
                        leftSide_btn.BackColor = Color.Yellow;
                        rightSide_btn.BackColor = Color.Khaki;
                        isLeft = true;
                    }
                    else
                    {
                        rightSide_btn.BackColor = Color.Yellow;
                        leftSide_btn.BackColor = Color.Khaki;
                        isLeft = false;
                    }
                    if (!IVLVariables.isdelete_thumbnail)
                    {
                        SaveChangedImages();
                        //This isshiftandcontrol is added by Darshan on 14-08-2015 to resolve Defect no 0000547: Reopen:Thumbnails are not unselected even on control press.Since the isShiftKeyPressed was not getting refereshed.
                    }
                    IVLVariables.isdelete_thumbnail = false;
                    Load_Image();
                    if (!IVLVariables.ivl_Camera.IsCapturing)
                    {
                        arg["rawImage"] = OriginalBm;
                        eventHandler.Notify(eventHandler.DisplayImage, arg);
                    }
                    modifyingBm = OriginalBm.Clone() as Bitmap;
                    //This below if statement is added by Darshan on 21-08-2015 to solve Defect no 0000593: The highlighting should be done on the channel display button.
                    if (IVLVariables.iscolorChange)
                    {
                        if (CurrentChannelDisplayed != Channels.Color)
                            CurrentChannelDisplayed = Channels.Color;
                        RGBbackgroundcolor();
                    }
                    ResetBrightnessContrastZoom();
                    brightness_rb.Enabled = true;
                    contrast_rb.Enabled = true;
                    //This line has been added by darshan on 18-09-2015 to change focus from numericupdowns
                    changeEyeSide_lbl.Focus();
                    if (IVLVariables.isZoomEnabled)
                        eventHandler.Notify(eventHandler.EnableZoomMagnification, new Args());
                }
                else
                {
                    OriginalBm = new Bitmap(10, 10);
                    modifyingBm = new Bitmap(10, 10);
                    arg["rawImage"] = OriginalBm;
                    eventHandler.Notify(eventHandler.DisplayImage, arg);
                    if (IVLVariables.isZoomEnabled)
                        eventHandler.Notify(eventHandler.EnableZoomMagnification, new Args());
                    //This below code has been added by Darshan on 08-09-2015 to solve defect no:The show channels is not getting refresh.
                    if (IVLVariables.iscolorChange)
                    {
                        if (CurrentChannelDisplayed != Channels.Color)
                            CurrentChannelDisplayed = Channels.Color;
                        RGBbackgroundcolor();
                    }
                    noImageSelected_lbl.Font = new System.Drawing.Font(noImageSelected_lbl.Font.FontFamily, 40f, GraphicsUnit.Pixel);
                    noImageSelected_lbl.Text = IVLVariables.LangResourceManager.GetString("NoImagesSelected_Label_Text", IVLVariables.LangResourceCultureInfo); ;
                    noImageSelected_lbl.Visible = true;
                    this.Enabled = false;
                    brightness_rb.Enabled = false;
                    contrast_rb.Enabled = false;
                    zoomMag_rb.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
                //                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        /// <summary>
        /// Removes the red color from the image and update the image in the view imaging screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showRedChannel_btn_Click(object sender, EventArgs e)
        {
            string ImagePath = string.Empty;
            if (IVLVariables.isCommandLineAppLaunch)
                ImagePath = thumbnailData.fileName;
            else if (NewDataVariables.Active_Obs != null)
            {
                if (!Convert.ToBoolean(IVLVariables.CurrentSettings.ImageStorageSettings.IsMrnFolder.val))
                    ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
                else
                    ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_PatientIdentifier.value + Path.DirectorySeparatorChar + NewDataVariables.Active_Visit.createdDate.Date.ToString("dd_MM_yyyy") + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
            }
            if (!noImageSelected_lbl.Visible && File.Exists(ImagePath))
            {
                noofimages();
                if (images.Length > 1)
                {
                    CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("AnnotationNo_Images", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("RedFilter_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Warning);
                }
                else
                {
                    SaveChangedImages();
                    //This below code is added by Darshan on 21-08-2015 to solve Defect no 0000593: The highlighting should be done on the channel display button.
                    Bitmap tempBm;
                    if (!IVLVariables.isValueChanged)
                    //This below code is added by Darshan on 21-08-2015 to solve Defect no 0000593: The highlighting should be done on the channel display button.
                    {
                        IVLVariables.iscolorChange = true;
                        if (CurrentChannelDisplayed != Channels.Red)
                        {
                            CurrentChannelDisplayed = Channels.Red;
                            #region 0001336 has been fixed by copying the channel image to temp bitmap and drawing to the modifying bitmap
                            tempBm = img[2].ToBitmap(); // in order to have a place for indexed bitmap only occuring for channel images

                            #endregion
                        }
                        else
                        {
                            tempBm = img.ToBitmap(); // in order to have a place for indexed bitmap only occuring for channel images

                            CurrentChannelDisplayed = Channels.Color;
                        }
                        RGBbackgroundcolor();
                    }
                    else
                        return;
                    ResetBrightnessContrastZoom();
                    #region 0001336 has been fixed by copying the channel image to temp bitmap and drawing to the modifying bitmap
                    Graphics g = Graphics.FromImage(modifyingBm);
                    g.DrawImage(tempBm, new Rectangle(0, 0, tempBm.Width, tempBm.Height));
                    g.Dispose();
                    tempBm.Dispose();
                    #endregion


                    Args arg = new Args();
                    arg["rawImage"] = modifyingBm;
                    eventHandler.Notify(eventHandler.DisplayImage, arg);

                }
            }
        }

        /// <summary>
        /// Removes the Green color from the image and update the image in the view imaging screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showGreenChannel_btn_Click(object sender, EventArgs e)
        {
            string ImagePath = string.Empty;
            if (IVLVariables.isCommandLineAppLaunch)
                ImagePath = thumbnailData.fileName;
            else if (NewDataVariables.Active_Obs != null)
            {
                if (!Convert.ToBoolean(IVLVariables.CurrentSettings.ImageStorageSettings.IsMrnFolder.val))
                    ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
                else
                    ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_PatientIdentifier.value + Path.DirectorySeparatorChar + NewDataVariables.Active_Visit.createdDate.Date.ToString("dd_MM_yyyy") + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
            }
            if (!noImageSelected_lbl.Visible && File.Exists(ImagePath))
            {
                noofimages();
                if (images.Length > 1)
                {
                    CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("AnnotationNo_Images", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("GreenFilter_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Warning);
                }
                else
                {
                    SaveChangedImages();
                    Bitmap tempBm;
                    if (!IVLVariables.isValueChanged)//This below code is added by Darshan on 21-08-2015 to solve Defect no 0000593: The highlighting should be done on the channel display button.
                    {
                        IVLVariables.iscolorChange = true;
                        if (CurrentChannelDisplayed != Channels.Green)
                        {
                            CurrentChannelDisplayed = Channels.Green;
                            #region 0001336 has been fixed by copying the channel image to temp bitmap and drawing to the modifying bitmap
                            tempBm = img[1].ToBitmap(); // in order to have a place for indexed bitmap only occuring for channel images

                            #endregion
                        }
                        else
                        {
                            tempBm = img.ToBitmap(); // in order to have a place for indexed bitmap only occuring for channel images

                            CurrentChannelDisplayed = Channels.Color;
                        }
                        RGBbackgroundcolor();
                    }
                    else
                        return;
                    ResetBrightnessContrastZoom();

                    Graphics g = Graphics.FromImage(modifyingBm);
                    g.DrawImage(tempBm, new Rectangle(0, 0, tempBm.Width, tempBm.Height));
                    g.Dispose();
                    tempBm.Dispose();
                    Args arg = new Args();
                    arg["rawImage"] = modifyingBm;
                    eventHandler.Notify(eventHandler.DisplayImage, arg);
                }
            }
        }

        /// <summary>
        /// Removes the Blue color from the image and update the image in the view imaging screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showBlueChannel_btn_Click(object sender, EventArgs e)
        {
            string ImagePath = string.Empty;
            if (IVLVariables.isCommandLineAppLaunch)
                ImagePath = thumbnailData.fileName;
            else if (NewDataVariables.Active_Obs != null)
            {
                if (!Convert.ToBoolean(IVLVariables.CurrentSettings.ImageStorageSettings.IsMrnFolder.val))
                    ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
                else
                    ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_PatientIdentifier.value + Path.DirectorySeparatorChar + NewDataVariables.Active_Visit.createdDate.Date.ToString("dd_MM_yyyy") + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
            }
            if (!noImageSelected_lbl.Visible && File.Exists(ImagePath))
            {
                noofimages();
                if (images.Length > 1)
                {
                    CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("AnnotationNo_Images", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("BlueFilter_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Warning);
                }
                else
                {
                    SaveChangedImages();
                    //This below code is added by Darshan on 21-08-2015 to solve Defect no 0000593: The highlighting should be done on the channel display button.
                    Bitmap tempBm;
                    if (!IVLVariables.isValueChanged)
                    //This below code is added by Darshan on 21-08-2015 to solve Defect no 0000593: The highlighting should be done on the channel display button.
                    {
                        IVLVariables.iscolorChange = true;
                        if (CurrentChannelDisplayed != Channels.Blue)
                        {
                            CurrentChannelDisplayed = Channels.Blue;
                            #region 0001336 has been fixed by copying the channel image to temp bitmap and drawing to the modifying bitmap
                            tempBm = img[0].ToBitmap(); // in order to have a place for indexed bitmap only occuring for channel images

                            #endregion
                        }
                        else
                        {
                            tempBm = img.ToBitmap(); // in order to have a place for indexed bitmap only occuring for channel images

                            CurrentChannelDisplayed = Channels.Color;
                        }
                        RGBbackgroundcolor();
                    }
                    else
                        return;
                    ResetBrightnessContrastZoom();
                    #region 0001336 has been fixed by copying the channel image to temp bitmap and drawing to the modifying bitmap
                    Graphics g = Graphics.FromImage(modifyingBm);
                    g.DrawImage(tempBm, new Rectangle(0, 0, tempBm.Width, tempBm.Height));
                    g.Dispose();
                    tempBm.Dispose();
                    #endregion
                    Args arg = new Args();
                    arg["rawImage"] = modifyingBm;
                    eventHandler.Notify(eventHandler.DisplayImage, arg);
                }
            }
        }

        /// <summary>
        /// Saves the report details into the DB
        /// </summary>
        /// <param name="reportVal"></param>
        /// <param name="e"></param>
        void
            _report_reportSavedEvent(Dictionary<string, object> reportVal, EventArgs e)
        {
            if(reportVal.Any())
            { 
            if (Convert.ToBoolean(IVLVariables.CurrentSettings.UISettings.ViewImaging._ReportWindowClose.val))
            {

                if (Convert.ToBoolean(reportVal["IsModifed"]) && (Convert.ToBoolean(reportVal["IsJsonFormat"])))
                    _report.Close();//This is commented to prevent report form from getting closed.
                IVLVariables.isReportWindowOpen = false;

            }

            string reportXml = reportVal["xml"] as string;
            bool ismodified = Convert.ToBoolean(reportVal["IsModifed"]);
            if (isView && ismodified)
            {
                NewDataVariables.Active_Report = NewDataVariables._Repo.GetById<report>(Convert.ToInt32(Reports_dgv.SelectedRows[0].Cells["reportId"].Value.ToString()));
                NewDataVariables.Active_Report.dataJson = reportXml;
                NewDataVariables.Active_Report.createdDate = DateTime.Now;
                NewDataVariables.Active_Report.lastModifiedDate = DateTime.Now;
                //NewIVLDataMethods.UpdateReport();
            }
            else
            {
                INTUSOFT.Data.NewDbModel.report report = new INTUSOFT.Data.NewDbModel.report();
                report.dataJson = reportXml;
                report.createdDate = (DateTime)reportVal["dateTime"];
                report.visit = NewDataVariables.Active_Visit;
                //This code has been added to insert user id into the table has to be removed once user or admin has been added and has be replaced by active user.
                users user = users.CreateNewUsers();
                user.userId = 1;
                ReportType rept = ReportType.CreateNewReportType();
                rept.reportTypeId = 1;
                report.lastModifiedDate = DateTime.Now;
                report.createdBy = user;
                report.report_type = rept;
                report.Patient = NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0];
                NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].visits.Where(y => y.visitId == NewDataVariables.Active_Visit.visitId).ToList()[0].reports.Add(report);
                //NewDataVariables.Active_Visit.reports.Add(report);
                //NewIVLDataMethods.AddReport(report);
                //NewIVLDataMethods.UpdateVisit();
                NewDataVariables.Reports.Add(report);
            }
            //PatientDetais_update();
            NewIVLDataMethods.UpdatePatient();
            }
            else
            {
                _report.Close();//This is commented to prevent report form from getting closed.
                IVLVariables.isReportWindowOpen = false;

            }

            //showExisitingReports();
            IVLVariables.IsAnotherWindowOpen = false;

        }

        /// <summary>
        /// Removes the changes applied to the image in the viewimagingscreen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removePostProcessing_btn_Click(object sender, EventArgs e)
        {
            string ImagePath = string.Empty;
            if (IVLVariables.isCommandLineAppLaunch)
                ImagePath = thumbnailData.fileName;
            else if (NewDataVariables.Active_Obs != null)
            {
                if (!Convert.ToBoolean(IVLVariables.CurrentSettings.ImageStorageSettings.IsMrnFolder.val))
                    ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
                else
                    ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_PatientIdentifier.value + Path.DirectorySeparatorChar + NewDataVariables.Active_Visit.createdDate.Date.ToString("dd_MM_yyyy") + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
            }
            if (!String.IsNullOrEmpty(ImagePath))
            {
                if (!noImageSelected_lbl.Visible && File.Exists(ImagePath))
                {
                    if (IVLVariables.isValueChanged || IVLVariables.isZoomEnabled || IVLVariables.iscolorChange)
                    {
                        IVLVariables.IsAnotherWindowOpen = true;
                        DialogResult res = CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("DiscardChanges_Text", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("DiscardChangesHeader_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxButtons.YesNo, CustomMessageBoxIcon.Question);
                        if (res == DialogResult.Yes)
                        {
                            colorimagebutton();
                            if (IVLVariables.isZoomEnabled)
                                eventHandler.Notify(eventHandler.EnableZoomMagnification, new Args());
                            if (CurrentChannelDisplayed != Channels.Color)
                                CurrentChannelDisplayed = Channels.Color;
                            RGBbackgroundcolor();
                            IVLVariables.isValueChanged = false;
                            ThumbnailUI.isValueChanged = false;
                            IVLVariables.iscolorChange = false;
                            //This below code has been added by Darshan on 13-08-2015 on 20:10 to solve Defect no:0000548 Reopen:Save image confirmation message coming up.Since the focus was not getting refreshed for brightness and contrast numericupdown.
                            brightness_lbl.Focus();
                        }
                        else
                        {

                        }
                        IVLVariables.IsAnotherWindowOpen = false;
                    }
                }
            }
        }

        /// <summary>
        /// Pop up will appear to save image or not.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void save_btn_Click(object sender, EventArgs e)
        {
            {
                string ImagePath = string.Empty;
                if (IVLVariables.isCommandLineAppLaunch)
                    ImagePath = thumbnailData.fileName;
                else if (NewDataVariables.Active_Obs != null)
                {
                    if (!Convert.ToBoolean(IVLVariables.CurrentSettings.ImageStorageSettings.IsMrnFolder.val))
                        ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
                    else
                        ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_PatientIdentifier.value + Path.DirectorySeparatorChar + NewDataVariables.Active_Visit.createdDate.Date.ToString("dd_MM_yyyy") + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
                }
                if (!noImageSelected_lbl.Visible && File.Exists(ImagePath))
                {
                    noofimages();
                    if (images.Length > 1)
                    {
                        CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("AnnotationNo_Images", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("SaveHeader_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Warning);
                    }
                    else
                    {
                        IVLVariables.IsAnotherWindowOpen = true;
                        //IVLVariables.ivl_Camera.TriggerOff();
                        //if (IVLVariables.isValueChanged)
                        //{
                        //    SaveChangedImages();
                        //}
                        //if (!IVLVariables.isValueChanged)
                        {
                            DialogResult res = CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("SaveConfirmation_Text", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("SaveHeader_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxButtons.YesNo, CustomMessageBoxIcon.Question);
                            if (res == DialogResult.Yes)
                            {
                                IVLVariables.isValueChanged = false;
                                ThumbnailUI.isValueChanged = false;
                                save_image();
                            }
                            if (res == DialogResult.No)
                            {
                            }
                        }
                        IVLVariables.IsAnotherWindowOpen = false;
                        //IVLVariables.ivl_Camera.TriggerOn();
                    }
                }
            }
        }

        /// <summary>
        /// Saves the image into the memory with the name and extension given by the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveAs_btn_Click(object sender, EventArgs e)
        {
            {
                IVLVariables.ivl_Camera.TriggerOff();
                string path = string.Empty;
                if (Convert.ToBoolean(IVLVariables.CurrentSettings.ImageStorageSettings.IsMrnFolder.val))
                    path = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_PatientIdentifier.value + Path.DirectorySeparatorChar + NewDataVariables.Active_Visit.createdDate.Date.ToString("dd_MM_yyyy");
                else
                    path = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString();
                if (!noImageSelected_lbl.Visible && File.Exists(path + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value))
                {
                    noofimages();
                    if (images.Length > 1)
                    {
                        CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("AnnotationNo_Images", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("SaveAs_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Warning);
                    }
                    else
                    {
                        CustomFolderBrowser.export = false;
                        customFolderBrowser = new CustomFolderBrowser();
                        if (thumbnailData.Name != null)
                            CustomFolderBrowser.fileName = thumbnailData.Name.Replace(" ", "_");
                        customFolderBrowser.CustomFolderData = customFolderData; //assigning the customFolderData dictionary to the CustomFolderData dictionary
                        customFolderBrowser.ShowImageExportButtons(); //to resize the custom folder browser
                        CustomFolderBrowser.ImageSavingbtn += customFolderBrowser_ImageSavingbtn; // subscribing to the event _ImageSavingbtn
                        CustomFolderBrowser.CancelButtonClickedEvent += customFolderBrowser_CancelButtonClickedEvent;
                        customFolderBrowser.ShowDialog();//Dialog() == DialogResult.Cancel)
                        // CustomFolderBrowser.ImageSavingbtn -= customFolderBrowser_ImageSavingbtn; // unsubscribing to the event _ImageSavingbtn

                    }
                }
            }
        }



        /// <summary>
        /// To export images from view imaging screen to the memory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportImages_btn_Click(object sender, EventArgs e)
        {
            string path = string.Empty;
            if (Convert.ToBoolean(IVLVariables.CurrentSettings.ImageStorageSettings.IsMrnFolder.val))
                path = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_PatientIdentifier.value + Path.DirectorySeparatorChar + NewDataVariables.Active_Visit.createdDate.Date.ToString("dd_MM_yyyy");
            else
                path = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString();
            if (!noImageSelected_lbl.Visible && File.Exists(path + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value))
            {
                RefreshViewScreenControls();
                if (!IVLVariables.isValueChanged)
                {
                    IVLVariables.ivl_Camera.TriggerOff();
                    CustomFolderBrowser.export = true;

                    customFolderBrowser = new CustomFolderBrowser();
                    customFolderBrowser.CustomFolderData = customFolderData; //assigning the customFolderData dictionary to the CustomFolderData
                    customFolderBrowser.ShowImageExportButtons();  //to resize the custom folder browser
                    CustomFolderBrowser.fileName = thumbnailData.Name.Replace(" ", "_");
                    CustomFolderBrowser.ImageSavingbtn += customFolderBrowser_ImageSavingbtn; // subscribing to the event _ImageSavingbtn
                    CustomFolderBrowser.CancelButtonClickedEvent += customFolderBrowser_CancelButtonClickedEvent;
                    if (Convert.ToBoolean(IVLVariables.CurrentSettings.UserSettings._enableExportImageBrowser.val))
                    {
                        DialogResult diagResult = customFolderBrowser.ShowDialog();
                    }
                }
            }
        }

        void customFolderBrowser_ImageSavingbtn()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                IVLVariables.ivl_Camera.TriggerOff();
                if (CustomFolderBrowser.export) //checks whether the export is true
                {
                    Args arg = new Args();
                    arg["folderPath"] = customFolderBrowser.returnPath;
                    arg["imageFormat"] = customFolderBrowser.imageFormat;
                    arg["compressionRatio"] = customFolderBrowser.compressionRatio;
                    arg["isExport"] = true;
                    eventHandler.Notify(eventHandler.GetImageFiles, arg);
                }
                else //executes when the export is false
                {
                    string fileFullPath = customFolderBrowser.folderPath + Path.DirectorySeparatorChar + CustomFolderBrowser.fileName;
                    CustomFolderBrowser.fileNames = new string[] { fileFullPath + "." + customFolderBrowser.imageFormat };
                    IVLVariables.ivl_Camera.camPropsHelper.SaveImage2Dir(modifyingBm, fileFullPath, (ImageSaveFormat)Enum.Parse(typeof(ImageSaveFormat), customFolderBrowser.imageFormat), customFolderBrowser.compressionRatio, true);
                    CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("SaveAsCompleted_Text", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("SaveAs_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Information);
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
                //                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        void customFolderBrowser_CancelButtonClickedEvent()
        {
            CustomFolderBrowser.CancelButtonClickedEvent -= customFolderBrowser_CancelButtonClickedEvent;
            CustomFolderBrowser.ImageSavingbtn -= customFolderBrowser_ImageSavingbtn; // subscribing to the event _ImageSavingbtn

            IVLVariables.IsAnotherWindowOpen = false;

        }

        /// <summary>
        /// Increase the brightness value to be applied on the image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void increaseBrightness_btn_Click(object sender, EventArgs e)
        {
            string ImagePath = string.Empty;
            if (IVLVariables.isCommandLineAppLaunch)
                ImagePath = thumbnailData.fileName;
            else if (NewDataVariables.Active_Obs != null)
            {
                if (!Convert.ToBoolean(IVLVariables.CurrentSettings.ImageStorageSettings.IsMrnFolder.val))
                    ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
                else
                    ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_PatientIdentifier.value + Path.DirectorySeparatorChar + NewDataVariables.Active_Visit.createdDate.Date.ToString("dd_MM_yyyy") + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
            }
            if (!noImageSelected_lbl.Visible && File.Exists(ImagePath))
            {
                if (brightness_rb.Value < brightness_rb.Maximum)
                    brightness_rb.Value++;
            }
        }

        /// <summary>
        /// Decrease the brightness value to be applied on the image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void decreaseBrightness_btn_Click(object sender, EventArgs e)
        {
            string ImagePath = string.Empty;
            if (IVLVariables.isCommandLineAppLaunch)
                ImagePath = thumbnailData.fileName;
            else if (NewDataVariables.Active_Obs != null)
            {
                if (!Convert.ToBoolean(IVLVariables.CurrentSettings.ImageStorageSettings.IsMrnFolder.val))
                    ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
                else
                    ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_PatientIdentifier.value + Path.DirectorySeparatorChar + NewDataVariables.Active_Visit.createdDate.Date.ToString("dd_MM_yyyy") + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
            }
            if (!noImageSelected_lbl.Visible && File.Exists(ImagePath))
            {
                if (brightness_rb.Value > brightness_rb.Minimum)
                    brightness_rb.Value--;
            }
        }

        /// <summary>
        /// Will pop up a message to save the changed image.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="arg"></param>
        private void Saveimg_changes(string s, Args arg)
        {
            SaveChangedImages();
            //valuechanged = false;
        }

        /// <summary>
        /// Invokes the ChangeBrightnessContrast method when contrast value is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contrast_rb_ValueChanged(object sender, EventArgs e)
        {
            noofimages();
            if (images.Length > 1)
            {
                contrast_rb.Value = 1;
                //CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("AnnotationNo_Images", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("ImageViewer_Contrast_Label_Text", IVLVariables.LangResourceCultureInfo).TrimEnd(':'), CustomMessageBoxIcon.Warning);
            }
            else
            {
                ChangeBrightnessContrast();
                scrollToolTip.SetToolTip(contrast_rb, contrast_rb.Value.ToString());
            }
        }

        /// <summary>
        /// Enables and disables the zoom magnifier on view imaging screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enableZoom_btn_Click(object sender, EventArgs e)
        {
            string ImagePath = string.Empty;
            if (IVLVariables.isCommandLineAppLaunch)
                ImagePath = thumbnailData.fileName;
            else if (NewDataVariables.Active_Obs != null)
            {
                if (!Convert.ToBoolean(IVLVariables.CurrentSettings.ImageStorageSettings.IsMrnFolder.val))
                    ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
                else
                    ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_PatientIdentifier.value + Path.DirectorySeparatorChar + NewDataVariables.Active_Visit.createdDate.Date.ToString("dd_MM_yyyy") + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
            }
            if (!noImageSelected_lbl.Visible && File.Exists(ImagePath))
            {
                Args arg = new Args();
                arg["isExport"] = false;
                eventHandler.Notify(eventHandler.GetImageFiles, arg);
                string[] image = currentReportLabelNames;
                if (image.Length > 1)
                {
                    CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("AnnotationNo_Images", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("ImageViewer_Zoom_Label_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Warning);
                }
                else
                {
                    if (enableZoom_btn.BackColor == Color.Khaki)
                    {
                        enableZoom_btn.BackColor = Color.Yellow;
                        iszoom = true;
                    }

                    else
                    {
                        enableZoom_btn.BackColor = Color.Khaki;
                        iszoom = false;
                        currentVal = 1;
                    }
                    eventHandler.Notify(eventHandler.EnableZoomMagnification, new Args());
                }
            }
        }

        /// <summary>
        /// Changes the image side to os(left side).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void leftSide_btn_Click(object sender, EventArgs e)
        {
            IVLVariables.IsAnotherWindowOpen = true;
            string ImagePath = string.Empty;
            if (IVLVariables.isCommandLineAppLaunch)
                ImagePath = thumbnailData.fileName;
            else if (NewDataVariables.Active_Obs != null)
            {
                if (!Convert.ToBoolean(IVLVariables.CurrentSettings.ImageStorageSettings.IsMrnFolder.val))
                    ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
                else
                    ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_PatientIdentifier.value + Path.DirectorySeparatorChar + NewDataVariables.Active_Visit.createdDate.Date.ToString("dd_MM_yyyy") + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
            }
            if (!String.IsNullOrEmpty(ImagePath))
            {
                if (!IsFileLocked(new FileInfo(ImagePath)))
                {
                    if (!noImageSelected_lbl.Visible && File.Exists(ImagePath))
                    {
                        noofimages();
                        if (images.Length > 1)
                        {
                            Common.CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("AnnotationNo_Images", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("Left_Side", IVLVariables.LangResourceCultureInfo), Common.CustomMessageBoxIcon.Warning);
                        }
                        else
                        {
                            //RefreshViewScreenControls();
                            if (leftSide_btn.BackColor != Color.Yellow)
                                ChangeEyeSide(true);
                        }
                    }
                }
                else
                {
                    CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("ODOSCrashWaring_Text", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("ODOSCrashWaring_Header", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Warning);
                }
                IVLVariables.IsAnotherWindowOpen = false;
            }
        }

        private void ChangeEyeSide(bool isLeft)
        {
            try
            {
                RefreshViewScreenControls();
                if (!IVLVariables.isValueChanged)//to check whether the bool value changed is false or not
                {
                    DialogResult result;

                    if (isLeft)
                        result = CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("Side_Change_Msg", IVLVariables.LangResourceCultureInfo) as string, IVLVariables.LangResourceManager.GetString("Left_Side", IVLVariables.LangResourceCultureInfo) as string, CustomMessageBoxButtons.YesNo, CustomMessageBoxIcon.Question);
                    else
                        result = CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("Side_Change_Msg", IVLVariables.LangResourceCultureInfo) as string, IVLVariables.LangResourceManager.GetString("Right_Side", IVLVariables.LangResourceCultureInfo) as string, CustomMessageBoxButtons.YesNo, CustomMessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        if (iszoom)
                        {
                            eventHandler.Notify(eventHandler.EnableZoomMagnification, new Args());
                        }

                        if (CurrentChannelDisplayed != Channels.Color)
                        {
                            CurrentChannelDisplayed = Channels.Color;
                        }
                        RGBbackgroundcolor();
                        ResetBrightnessContrastZoom();
                        //ImageModel img = DataVariables._imageRepo.GetById(IVLVariables.ActiveImageID);
                        //This below code has been added by Darshan on 22-08-2015 to support Advance Searching.
                        Args arg = new Args();//OD and OS in the thumbnail view is not changing so it is initialized before the image has been updated

                        if (!IVLVariables.isCommandLineAppLaunch)
                        {
                            NewDataVariables.Active_Obs.lastModifiedDate = DateTime.Now;
                            arg["id"] = NewDataVariables.Active_Obs.observationId;//
                            //the below code has been added by Darshan to solve defect no 0000510: Duplicate numbering in comments if pressed on control key.
                            arg["isannotated"] = NewDataVariables.Active_Obs.annotationsAvailable;
                            arg["isCDR"] = NewDataVariables.Active_Obs.cdrAnnotationAvailable;// This line has been added to handle the crash  when iscdr is missing od and os changed by sriram on september 24 2015

                        }
                        else
                        {
                            arg["id"] = thumbnailData.id;
                            arg["isannotated"] = thumbnailData.isAnnotated;
                            arg["isCDR"] = thumbnailData.isCDR;// This line has been added to handle the crash  when iscdr is missing od and os changed by sriram on september 24 2015

                        }

                        if (isLeft)
                        {
                            rightSide_btn.BackColor = Color.Khaki;
                            leftSide_btn.BackColor = Color.Yellow;
                            if (!IVLVariables.isCommandLineAppLaunch)
                            {
                                NewDataVariables.Active_Obs.eyeSide = 'L';
                                Patient pat = NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0];
                                NewDataVariables.Active_Obs.lastModifiedDate = DateTime.Now;
                                //newObs.eye_fundus_image = eyeFundusImage;
                                //eyeFundusImage.obs_id = newObs;

                                //eyeFundusImage.eye_fundus_image_id = newObs.observationId;
                                pat.patientLastModifiedDate = DateTime.Now;

                                pat.observations.ToList()[pat.observations.ToList().FindIndex(x => x.observationId == NewDataVariables.Active_Obs.observationId)] = NewDataVariables.Active_Obs;
                                NewDataVariables.Active_Visit.observations.ToList()[NewDataVariables.Active_Visit.observations.ToList().FindIndex(x => x.observationId == NewDataVariables.Active_Obs.observationId)] = NewDataVariables.Active_Obs;
                                NewDataVariables.Active_Visit.lastModifiedDate = DateTime.Now;
                                pat.visits.Where(x => x == NewDataVariables.Active_Visit).ToList()[0] = NewDataVariables.Active_Visit;
                                NewDataVariables.Patients[NewDataVariables.Patients.FindIndex(x => x.personId == NewDataVariables.Active_Patient)] = pat;

                                NewIVLDataMethods.UpdatePatient();
                            }
                            thumbnailData.side = 1;
                            arg["side"] = 1;//has been added to change the side in the thumbnail
                            //IVLVariables.ivl_Camera.camPropsHelper.LeftRightPos = LeftRightPosition.Left;

                        }
                        else
                        {
                            rightSide_btn.BackColor = Color.Yellow;
                            leftSide_btn.BackColor = Color.Khaki;
                            if (!IVLVariables.isCommandLineAppLaunch)
                            {

                                NewDataVariables.Active_Obs.eyeSide = 'R';
                                Patient pat = NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0];
                                NewDataVariables.Active_Obs.lastModifiedDate = DateTime.Now;
                                //newObs.eye_fundus_image = eyeFundusImage;
                                //eyeFundusImage.obs_id = newObs;

                                //eyeFundusImage.eye_fundus_image_id = newObs.observationId;
                                pat.patientLastModifiedDate = DateTime.Now;

                                pat.observations.ToList()[pat.observations.ToList().FindIndex(x => x.observationId == NewDataVariables.Active_Obs.observationId)] = NewDataVariables.Active_Obs;
                                NewDataVariables.Active_Visit.observations.ToList()[NewDataVariables.Active_Visit.observations.ToList().FindIndex(x => x.observationId == NewDataVariables.Active_Obs.observationId)] = NewDataVariables.Active_Obs;
                                NewDataVariables.Active_Visit.lastModifiedDate = DateTime.Now;
                                pat.visits.Where(x => x == NewDataVariables.Active_Visit).ToList()[0] = NewDataVariables.Active_Visit;
                                NewDataVariables.Patients[NewDataVariables.Patients.FindIndex(x => x.personId == NewDataVariables.Active_Patient)] = pat;

                                NewIVLDataMethods.UpdatePatient();
                            }
                            thumbnailData.side = 0;
                            arg["side"] = 0;//has been added to change the side in the thumbnail
                            //IVLVariables.ivl_Camera.camPropsHelper.LeftRightPos = LeftRightPosition.Right;


                        }
                        //if (!IVLVariables.isCommandLineAppLaunch)
                        //NewIVLDataMethods.UpdateImage();
                        //Below code has been added by Darshan on 14-08-2015 to solve defect no:0000548: Reopen:Save image confirmation message coming up.


                        //eventHandler.Notify(eventHandler.ChangeThumbnailSide, arg);
                        //colorBm = display_pbx.Image as Bitmap;
                        Load_Image();

                        INTUSOFT.Imaging.CaptureLog capturedImageCameraSettings = null;

                        if (!IVLVariables.isCommandLineAppLaunch)
                        {
                            INTUSOFT.Imaging.MaskSettings maskSettings = null;
                            if (NewDataVariables.Active_Obs.maskSetting != null)
                            {
                                maskSettings = (INTUSOFT.Imaging.MaskSettings)Deserialize(typeof(INTUSOFT.Imaging.MaskSettings), NewDataVariables.Active_Obs.maskSetting);
                                if (maskSettings == null)
                                    maskSettings = (INTUSOFT.Imaging.MaskSettings)JsonConvert.DeserializeObject(NewDataVariables.Active_Obs.maskSetting, typeof(INTUSOFT.Imaging.MaskSettings));
                            }
                            if (NewDataVariables.Active_Obs.cameraSetting != null)
                            {
                                capturedImageCameraSettings = (INTUSOFT.Imaging.CaptureLog)Deserialize(typeof(INTUSOFT.Imaging.CaptureLog), NewDataVariables.Active_Obs.cameraSetting);
                                if (capturedImageCameraSettings == null)
                                    capturedImageCameraSettings = (INTUSOFT.Imaging.CaptureLog)JsonConvert.DeserializeObject(NewDataVariables.Active_Obs.cameraSetting, typeof(INTUSOFT.Imaging.CaptureLog));
                            }
                            if (maskSettings != null)
                            {

                                IVLVariables.ivl_Camera.camPropsHelper.PostProcessing.ApplyMask(ref OriginalBm, maskSettings, true);
                            }
                            else
                            {
                                INTUSOFT.Imaging.MaskSettings maskSettings1 = new Imaging.MaskSettings();
                                maskSettings1.isApplyLogo = Convert.ToBoolean(IVLVariables.CurrentSettings.PostProcessingSettings.MaskSettings._ApplyLogo.val);
                                maskSettings1.isApplyMask = Convert.ToBoolean(IVLVariables.CurrentSettings.PostProcessingSettings.MaskSettings._IsApplyMask.val);
                                maskSettings1.maskWidth = Convert.ToInt32(IVLVariables.CurrentSettings.PostProcessingSettings.MaskSettings.CaptureMaskWidth.val);
                                maskSettings1.maskHeight = Convert.ToInt32(IVLVariables.CurrentSettings.PostProcessingSettings.MaskSettings.CaptureMaskHeight.val);
                                maskSettings1.maskCentreX = Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._ImageOpticalCentreX.val);
                                maskSettings1.maskCentreY = Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._ImageOpticalCentreY.val);
                                IVLVariables.ivl_Camera.camPropsHelper.PostProcessing.ApplyMask(ref OriginalBm, maskSettings1, true);
                            }
                        }
                        else
                            IVLVariables.ivl_Camera.camPropsHelper.PostProcessing.ApplyMask(ref OriginalBm, IVLVariables.ivl_Camera.camPropsHelper._Settings.PostProcessingSettings.maskSettings, true);

                        Bitmap thumbnailBM = null;
                        if (Convert.ToBoolean(IVLVariables.CurrentSettings.PostProcessingSettings.MaskSettings._ApplyLogo.val))
                        {
                            LeftRightPosition left = LeftRightPosition.Left;
                            string thumbnailName = "";
                            if (!IVLVariables.isCommandLineAppLaunch)
                            {
                                if (NewDataVariables.Active_Obs.eyeSide == 'R')
                                {
                                    thumbnailName = "OD 01";
                                    left = LeftRightPosition.Right;
                                }
                                else
                                {
                                    thumbnailName = "OS 01";
                                    left = LeftRightPosition.Left;
                                }
                            }
                            else
                            {
                                if (thumbnailData.side == 0)
                                {
                                    thumbnailName = "OD 01";
                                    left = LeftRightPosition.Right;
                                }
                                else
                                {
                                    thumbnailName = "OS 01";
                                    left = LeftRightPosition.Left;
                                }
                            }
                            IVLVariables.ivl_Camera.camPropsHelper.PostProcessing.ApplyLogo(ref OriginalBm, thumbnailName, Color.Black, left);//ApplyLogo takes 3 agruments . Here empty string and black colour passed. By Ashutosh 29-08-2017.
                        }
                        {
                            if (System.IO.File.Exists(thumbnailData.fileName))
                            {
                                System.IO.File.Delete(thumbnailData.fileName);
                                FileInfo finf = new FileInfo(thumbnailData.fileName);
                                string[] strArr = finf.Name.Split('.');

                                string thumbnailName = finf.DirectoryName + System.IO.Path.DirectorySeparatorChar + strArr[0] + "_tb." + strArr[1];

                                if (System.IO.File.Exists(thumbnailName))
                                {
                                    System.IO.File.Delete(thumbnailName);
                                }
                            }
                        }

                        if (capturedImageCameraSettings != null && !string.IsNullOrEmpty(capturedImageCameraSettings.ImageTime))//to apply time stamp for the ffa images when the eye side is changed by kishore on 6 september 2017
                        {
                            IVLVariables.ivl_Camera.camPropsHelper.PostProcessing.ApplyTimeStamp(ref OriginalBm, capturedImageCameraSettings.ImageTime, Color.LimeGreen);
                        }
                        if (IVLVariables.isCommandLineAppLaunch)
                        {
                            FileInfo finf = new FileInfo(thumbnailData.fileName);

                            string[] str = finf.Name.Split('_');
                            if ((int)arg["side"] == 1)
                                str[0] = "OS";
                            else
                                str[0] = "OD";
                            string fileName = string.Join("_", str);
                            finf = new FileInfo(finf.Directory.FullName + Path.DirectorySeparatorChar + fileName);
                            thumbnailData.fileName = finf.FullName;
                            arg["ImgLoc"] = thumbnailData.fileName;
                        }
                        FileInfo finf1 = new FileInfo(thumbnailData.fileName);

                        string[] strArr1 = finf1.Name.Split('.');
                        IVLVariables.ivl_Camera.camPropsHelper.SaveImage2Dir(OriginalBm, finf1.DirectoryName + Path.DirectorySeparatorChar + strArr1[0], (ImageSaveFormat)Enum.Parse(typeof(ImageSaveFormat), IVLVariables.CurrentSettings.ImageStorageSettings._ImageSaveFormat.val.ToLower()), Convert.ToInt32(IVLVariables.CurrentSettings.ImageStorageSettings._compressionRatio.val), true);

                        //OriginalBm.Save(thumbnailData.fileName);

                        //bm = colorBm;
                        //colorBm = bm.Clone() as Bitmap;
                        eventHandler.Notify(eventHandler.ChangeThumbnailSide, arg);//added after colorBm.Save to show the side changed image in thumbnail list.
                        arg["rawImage"] = OriginalBm;
                        eventHandler.Notify(eventHandler.DisplayImage, arg);
                        modifyingBm = OriginalBm.Clone() as Bitmap;
                        //bm = display_pbx.Image as Bitmap;
                        Load_Image();
                        enableZoom_btn.BackColor = Color.Khaki;
                        iszoom = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
                //                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
        }

        /// <summary>
        /// Changes the image side to od(right side).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rightSide_btn_Click(object sender, EventArgs e)
        {
            IVLVariables.IsAnotherWindowOpen = true;
            string ImagePath = string.Empty;
            if (IVLVariables.isCommandLineAppLaunch)
                ImagePath = thumbnailData.fileName;
            else if (NewDataVariables.Active_Obs != null)
            {
                if (!Convert.ToBoolean(IVLVariables.CurrentSettings.ImageStorageSettings.IsMrnFolder.val))
                    ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
                else
                    ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_PatientIdentifier.value + Path.DirectorySeparatorChar + NewDataVariables.Active_Visit.createdDate.Date.ToString("dd_MM_yyyy") + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
            }
            if (!String.IsNullOrEmpty(ImagePath))
            {
                if (!IsFileLocked(new FileInfo(ImagePath)))
                {
                    if (!noImageSelected_lbl.Visible && File.Exists(ImagePath))
                    {
                        noofimages();
                        if (images.Length > 1)
                        {
                            CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("AnnotationNo_Images", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("Right_Side", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Warning);
                        }
                        else
                        {
                            if (rightSide_btn.BackColor != Color.Yellow)
                            {
                                ChangeEyeSide(false);
                            }
                            //if(result == DialogResult.No)
                            //   return;
                        }
                    }
                }
                else
                {
                    CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("ODOSCrashWaring_Text", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("ODOSCrashWaring_Header", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Warning);
                }
                IVLVariables.IsAnotherWindowOpen = false;
            }
        }

        /// <summary>
        /// Checks wheather file is locked or not
        /// </summary>
        /// <param name="file">FileInfo type</param>
        /// <returns></returns>
        protected virtual bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException ex)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            //file is not locked
            return false;
        }

        /// <summary>
        /// Decreases the zoomMag_rb value when decreseZoom_btn is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void decreseZoom_btn_Click(object sender, EventArgs e)
        {
            string ImagePath = string.Empty;
            if (IVLVariables.isCommandLineAppLaunch)
                ImagePath = thumbnailData.fileName;
            else if (NewDataVariables.Active_Obs != null)
            {
                if (!Convert.ToBoolean(IVLVariables.CurrentSettings.ImageStorageSettings.IsMrnFolder.val))
                    ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
                else
                    ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_PatientIdentifier.value + Path.DirectorySeparatorChar + NewDataVariables.Active_Visit.createdDate.Date.ToString("dd_MM_yyyy") + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
            }
            if (!noImageSelected_lbl.Visible && File.Exists(ImagePath))
            {
                if (IVLVariables.isZoomEnabled)
                {
                    if (zoomMag_rb.Value > zoomMag_rb.Minimum)
                        if (Convert.ToInt32(zoomMag_rb.Value - Convert.ToSingle(IVLVariables.CurrentSettings.UserSettings._ZoomIncrementFactor.val) * 10) > zoomMag_rb.Minimum)
                            zoomMag_rb.Value = Convert.ToInt32(zoomMag_rb.Value - Convert.ToSingle(IVLVariables.CurrentSettings.UserSettings._ZoomIncrementFactor.val) * 10);
                        else
                            zoomMag_rb.Value = zoomMag_rb.Minimum;
                }
            }
        }

        /// <summary>
        /// Increases the zoomMag_rb value when increaseZoom_btn is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void increaseZoom_btn_Click(object sender, EventArgs e)
        {
            string ImagePath = string.Empty;
            if (IVLVariables.isCommandLineAppLaunch)
                ImagePath = thumbnailData.fileName;
            else if (NewDataVariables.Active_Obs != null)
            {
                if (!Convert.ToBoolean(IVLVariables.CurrentSettings.ImageStorageSettings.IsMrnFolder.val))
                    ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
                else
                    ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_PatientIdentifier.value + Path.DirectorySeparatorChar + NewDataVariables.Active_Visit.createdDate.Date.ToString("dd_MM_yyyy") + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
            }
            if (!noImageSelected_lbl.Visible && File.Exists(ImagePath))
            {
                // increaseZoom();
                if (IVLVariables.isZoomEnabled)
                {
                    if (zoomMag_rb.Value < zoomMag_rb.Maximum)
                    {
                        if (Convert.ToInt32(zoomMag_rb.Value + Convert.ToSingle(IVLVariables.CurrentSettings.UserSettings._ZoomIncrementFactor.val) * 10) < zoomMag_rb.Maximum)
                            zoomMag_rb.Value = Convert.ToInt32(zoomMag_rb.Value + Convert.ToSingle(IVLVariables.CurrentSettings.UserSettings._ZoomIncrementFactor.val) * 10);
                        else
                            zoomMag_rb.Value = zoomMag_rb.Maximum;
                    }
                }
            }
        }

        /// <summary>
        /// Invokes decreaseZoom or increaseZoom when zoomMag_rb value is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zoomMag_rb_ValueChanged(object sender, EventArgs e)
        {
            //The below code has been modified to solve Zoom Modifier defect.
            if (!noImageSelected_lbl.Visible)
            {
                if (IVLVariables.isZoomEnabled)
                {
                    if (zoomMag_rb.Value > currentVal)
                    {
                        increaseZoom();
                    }
                    else
                    {
                        decreaseZoom();
                    }
                    float zoomVal = (float)(zoomMag_rb.Value) * Convert.ToSingle(IVLVariables.CurrentSettings.UserSettings._ZoomScale.val);
                    scrollToolTip.SetToolTip(zoomMag_rb, zoomVal.ToString());
                    currentVal = zoomMag_rb.Value;
                }
            }
        }

        /// <summary>
        /// Resets the image back to its original form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showColorImage_btn_Click(object sender, EventArgs e)
        {
            RefreshViewScreenControls();
        }

        /// <summary>
        /// Decreases the contrast_rb value when clicked 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void decreaseContrast_tbn_Click(object sender, EventArgs e)
        {
            string ImagePath = string.Empty;
            if (IVLVariables.isCommandLineAppLaunch)
                ImagePath = thumbnailData.fileName;
            else if (NewDataVariables.Active_Obs != null)
            {
                if (!Convert.ToBoolean(IVLVariables.CurrentSettings.ImageStorageSettings.IsMrnFolder.val))
                    ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
                else
                    ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_PatientIdentifier.value + Path.DirectorySeparatorChar + NewDataVariables.Active_Visit.createdDate.Date.ToString("dd_MM_yyyy") + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
            }
            if (!noImageSelected_lbl.Visible && File.Exists(ImagePath))
            {
                if (contrast_rb.Value > contrast_rb.Minimum)
                    contrast_rb.Value--;
            }
        }

        /// <summary>
        /// Increases the contrast_rb value when clicked 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void increaseContrast_btn_Click(object sender, EventArgs e)
        {
            string ImagePath = string.Empty;
            if (IVLVariables.isCommandLineAppLaunch)
                ImagePath = thumbnailData.fileName;
            else if (NewDataVariables.Active_Obs != null)
            {
                if (!Convert.ToBoolean(IVLVariables.CurrentSettings.ImageStorageSettings.IsMrnFolder.val))
                    ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
                else
                    ImagePath = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_PatientIdentifier.value + Path.DirectorySeparatorChar + NewDataVariables.Active_Visit.createdDate.Date.ToString("dd_MM_yyyy") + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
            }
            if (!noImageSelected_lbl.Visible && File.Exists(ImagePath))
            {
                if (contrast_rb.Value < contrast_rb.Maximum)
                    contrast_rb.Value++;
            }
        }

        /// <summary>
        /// Will opens the repot window when view is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reports_dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        /// <summary>
        /// Opens the annotation window to save or delete annotations of the image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newAnnotation_btn_Click(object sender, EventArgs e)
        {
            IVLVariables.IsAnotherWindowOpen = true;
            Args arg = new Args();
            arg["isDefault"] = false;
            eventHandler.Notify(eventHandler.UpdateMainWindowCursor, arg);//added to make the wait cursor to appear by kishore on 18 September 2017.
            if (NewDataVariables.Active_Obs != null)
            {
                string path = string.Empty;
                if (Convert.ToBoolean(IVLVariables.CurrentSettings.ImageStorageSettings.IsMrnFolder.val))
                    path = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_PatientIdentifier.value + Path.DirectorySeparatorChar + NewDataVariables.Active_Visit.createdDate.Date.ToString("dd_MM_yyyy");
                else
                    path = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString();
                if (File.Exists(path + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value))
                {
                    if (!noImageSelected_lbl.Visible)
                    {
                        RefreshViewScreenControls();


                        arg["isExport"] = false;

                        eventHandler.Notify(eventHandler.GetImageFiles, arg);
                        string[] image = currentReportLabelNames;
                        if (image.Length > 1)
                        {
                            CustomMessageBoxPopUp(IVLVariables.LangResourceManager.GetString("AnnotationNo_Images", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("Annotation_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Warning);
                        }
                        else
                        {
                            IVLVariables.IsAnotherWindowOpen = true;
                            if (getReportDetails())
                            {
                                NewDataVariables.Annotations = NewDataVariables._Repo.GetByCategory<eye_fundus_image_annotation>("eyeFundusImage", NewDataVariables.Active_Obs).Where(x => x.voided == false).ToList();//.Annotations.ToList();
                                NewDataVariables.Annotations = NewDataVariables.Annotations.OrderBy(x => x.createdDate).ToList();
                                NewDataVariables.Annotations.Reverse();
                                reportDic.Add("$annotationDetails", IVLVariables.LangResourceManager.GetString("Annotation_Details_Text", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$annotationComments", IVLVariables.LangResourceManager.GetString("Annotation_Comments_Text", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$annotationWarningMessage", IVLVariables.LangResourceManager.GetString("AnnotationWarning_Message_Text", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$annotationHeaderMessage", IVLVariables.LangResourceManager.GetString("AnnotationWarning_Header_Text", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$ellipseBtnText", IVLVariables.LangResourceManager.GetString("Ellipse_Btn_Text", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$rectangleBtnText", IVLVariables.LangResourceManager.GetString("Rectangle_Btn_Text", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$freeDrawBtnText", IVLVariables.LangResourceManager.GetString("FreeDraw_Btn_Text", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$lineDrawBtnText", IVLVariables.LangResourceManager.GetString("LineDraw_Btn_Text", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$printBtnText", IVLVariables.LangResourceManager.GetString("Print_Btn_Text", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$saveBtnText", IVLVariables.LangResourceManager.GetString("SaveHeader_Text", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$reportMessageText", IVLVariables.LangResourceManager.GetString("AnnotationDelete_Message_Text", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$reportHeaderText", IVLVariables.LangResourceManager.GetString("AnnotationDelete_Header_Text", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$clearMessageText", IVLVariables.LangResourceManager.GetString("AnnotationClear_Message_Text", IVLVariables.LangResourceCultureInfo));//string accociated with AnnotationClear_Message_Text in language resource is given to key.By Ashutosh 07-09-2017.
                                reportDic.Add("$clearHeaderText", IVLVariables.LangResourceManager.GetString("AnnotationClear_Header_Text", IVLVariables.LangResourceCultureInfo));//string accociated with AnnotationClear_Header_Text in language resource is given to key.By Ashutosh 07-09-2017.
                                reportDic.Add("$annotationMaxCharsText", IVLVariables.LangResourceManager.GetString("MaxAnnotation_Char_Text", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$annotationMarkingColor", IVLVariables.CurrentSettings.AnnotationColorSelection._AnnotationMarkingColor.val.ToString());
                                reportDic.Add("$cupColor", IVLVariables.CurrentSettings.AnnotationColorSelection._CupColor.val.ToString());
                                reportDic.Add("$discColor", IVLVariables.CurrentSettings.AnnotationColorSelection._DiscColor.val.ToString());
                                reportDic.Add("$annotationText", IVLVariables.LangResourceManager.GetString("Annotation_Header_Text", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$filters", IVLVariables.LangResourceManager.GetString("Filter_Text", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$redFilter", IVLVariables.LangResourceManager.GetString("RedFilter_ToolTipText", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$greenFilter", IVLVariables.LangResourceManager.GetString("GreenFilter_ToolTipText", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$blueFilter", IVLVariables.LangResourceManager.GetString("BlueFilter_ToolTipText", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$redFilterText", IVLVariables.LangResourceManager.GetString("RedFilter_Text", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$greenFilterText", IVLVariables.LangResourceManager.GetString("GreenFilter_Text", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$blueFilterText", IVLVariables.LangResourceManager.GetString("BlueFilter_Text", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$colorFilter", IVLVariables.LangResourceManager.GetString("ColorFilter_ToolTipText", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$noOfToolsAllowedMessage", IVLVariables.LangResourceManager.GetString("Annotation_MaxTools_Warning_Text", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$noOfToolsAllowedHeader", IVLVariables.LangResourceManager.GetString("SaveCDRheader_Text", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$EllipseButtonToolTipText", IVLVariables.LangResourceManager.GetString("EllipseButton_ToolTipText", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$EllipseButtonText", IVLVariables.LangResourceManager.GetString("Ellipse_Btn_Text", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$RectangleButtonToolTipText", IVLVariables.LangResourceManager.GetString("RectangleButton_ToolTipText", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$RectangleButtonText", IVLVariables.LangResourceManager.GetString("Rectangle_Btn_Text", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$FreeDrawButtonToolTipText", IVLVariables.LangResourceManager.GetString("FreeDrawButton_ToolTipText", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$FreeDrawButtonText", IVLVariables.LangResourceManager.GetString("FreeDraw_Btn_Text", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$LineDrawButtonToolTipText", IVLVariables.LangResourceManager.GetString("LineDrawButton_ToolTipText", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$LineDrawButtonText", IVLVariables.LangResourceManager.GetString("LineDraw_Btn_Text", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$SaveAnnotationButtonToolTipText", IVLVariables.LangResourceManager.GetString("SaveAnnotationButton_ToolTipText", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$PrintAnnotationButtonToolTipText", IVLVariables.LangResourceManager.GetString("PrintAnnotationButton_ToolTipText", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$ExportAnnotationButtonToolTipText", IVLVariables.LangResourceManager.GetString("ExportAnnotationButton_ToolTipText", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$ExportAnnotationButtonText", IVLVariables.LangResourceManager.GetString("Export_Button_Text", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$ZoomInText", IVLVariables.LangResourceManager.GetString("ZoomIn_Text", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$ZoomOutText", IVLVariables.LangResourceManager.GetString("ZoomOut_Text", IVLVariables.LangResourceCultureInfo));
                                reportDic.Add("$ZoomResetText", IVLVariables.LangResourceManager.GetString("ZoomReset_Text", IVLVariables.LangResourceCultureInfo));


                                if (reportDic.ContainsKey("$NameOfTheReport"))//checks if key $NameOfTheReport is present .By Ashutosh 17-08-2017
                                    reportDic["$NameOfTheReport"] = IVLVariables.CurrentSettings.ReportSettings.NameOfAnnotationReport.val.ToString();// if present then it's value is replaced.By Ashutosh 17-08-2017
                                else
                                    reportDic.Add("$NameOfTheReport", IVLVariables.CurrentSettings.ReportSettings.NameOfAnnotationReport.val.ToString());// if not present then key and value are added.By Ashutosh 17-08-2017
                                //reportDic.Add("$Name", IVLVariables.LangResourceManager.GetString("PrintAnnotationButton_ToolTipText", IVLVariables.LangResourceCultureInfo));
                                if (reportDic.ContainsKey("$currentTemplate"))
                                {
                                    reportDic.Remove("$currentTemplate");

                                }
                                reportDic.Add("$currentTemplate", IVLVariables.appDirPathName + Path.DirectorySeparatorChar + @"ReportTemplates\Annotation\Normal\Annotation_LsA4.xml");
                                string[] annotation_name = { IVLVariables.LangResourceManager.GetString("Annotation_Date", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("Annotation_Time", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("Annotation_View", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("Annotation_Delete", IVLVariables.LangResourceCultureInfo), IVLVariables.Operator, IVLVariables.LangResourceManager.GetString("Reported_By_Text", IVLVariables.LangResourceCultureInfo) };
                                reportDic.Add("$CreatedFiles", annotation_name as string[]);
                                reportDic.Add("$is24hrformat", Convert.ToBoolean(IVLVariables.CurrentSettings.UserSettings._Is24clock.val));
                                //This below code has been changed because the previous IVLVariables.ActiveImageID was not getting updated.
                                if (reportDic.ContainsKey("$NameOfTheReport"))
                                    reportDic["$NameOfTheReport"] = IVLVariables.CurrentSettings.ReportSettings.NameOfAnnotationReport.val;
                                else
                                    reportDic.Add("$NameOfTheReport", IVLVariables.CurrentSettings.ReportSettings.NameOfAnnotationReport.val);

                                Annotation.Form1 annotationForm = new Form1(reportDic);

                                annotationForm.annotationSavedEvent += annotationForm_annotationSavedEvent;
                                annotationForm._patientdetailsupdate += annotationForm__patientdetailsupdate;
                                annotationForm.annotationDeleteEvent += annotationForm_annotationDeleteEvent;
                                annotationForm.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
                                annotationForm.WindowState = FormWindowState.Maximized;
                                //This below bool variable is added by Darshan on 21-08-2015 to solve Defect no:0000585: annotation window,report window,view full info,user settings window, when trigger press capture is happening.

                                //IVLVariables.ivl_Camera.TriggerOff();
                                if (annotationForm.ShowDialog() == DialogResult.Cancel)
                                {
                                    //This below bool variable is added by Darshan on 21-08-2015 to solve Defect no:0000585: annotation window,report window,view full info,user settings window, when trigger press capture is happening.
                                    IVLVariables.IsAnotherWindowOpen = false;
                                    //IVLVariables.ivl_Camera.TriggerOn();
                                    Load_Image();
                                    arg["rawImage"] = OriginalBm;
                                    eventHandler.Notify(eventHandler.DisplayImage, arg);
                                }

                            }
                        }
                    }
                    else
                        CustomMessageBoxPopUp(string.Format(IVLVariables.LangResourceManager.GetString("Annotation_Image_Warning_Text", IVLVariables.LangResourceCultureInfo)), string.Format(IVLVariables.LangResourceManager.GetString("AnnotationWarning_Header", IVLVariables.LangResourceCultureInfo)), CustomMessageBoxIcon.Warning);
                    //Common.CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("Annotation_Image_Warning_Text", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("AnnotationWarning_Header", IVLVariables.LangResourceCultureInfo), Common.CustomMessageBoxIcon.Warning);
                    arg["isDefault"] = true;
                    eventHandler.Notify(eventHandler.UpdateMainWindowCursor, arg);//added to make the default cursor to appear by kishore on 18 September 2017.
                }
                else
                    return;
            }
            else
                CustomMessageBoxPopUp(string.Format(IVLVariables.LangResourceManager.GetString("Annotation_Image_Warning_Text", IVLVariables.LangResourceCultureInfo)), string.Format(IVLVariables.LangResourceManager.GetString("AnnotationWarning_Header", IVLVariables.LangResourceCultureInfo)), CustomMessageBoxIcon.Warning);
            //Common.CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("Annotation_Image_Warning_Text", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("AnnotationWarning_Header", IVLVariables.LangResourceCultureInfo), Common.CustomMessageBoxIcon.Warning);
            arg["isDefault"] = true;
            eventHandler.Notify(eventHandler.UpdateMainWindowCursor, arg);
        }


        private void CustomMessageBoxPopUp(string text, string header, CustomMessageBoxIcon msgBoxIcons)
        {
            DialogResult res = CustomMessageBox.Show(text, header, msgBoxIcons);
            if (res == DialogResult.OK)
                IVLVariables.IsAnotherWindowOpen = false;

        }

        private void CustomMessageBoxPopUp(string text, string header, CustomMessageBoxButtons msgBoxButtons, CustomMessageBoxIcon msgBoxIcons)
        {
            DialogResult res = CustomMessageBox.Show(text, header, msgBoxIcons);
            if (res == DialogResult.OK)
                IVLVariables.IsAnotherWindowOpen = false;

        }

        /// <summary>
        /// Deletes the annotation from the annotation list.
        /// </summary>
        /// <param name="e"></param>
        void annotationForm_annotationDeleteEvent(Args e)
        {
            if (e.ContainsKey("isCDR"))
            {
                if (Convert.ToBoolean(e["isCDR"]))
                {
                    NewDataVariables.Active_Obs.cdrAnnotationAvailable = false;
                }
                else
                {
                    NewDataVariables.Active_Obs.annotationsAvailable = false;
                    NewDataVariables.Active_Obs.cdrAnnotationAvailable = false;
                }
            }
            else
                NewDataVariables.Active_Obs.annotationsAvailable = false;
            NewIVLDataMethods.UpdatePatient();//._imageRepo.Update(DataVariables.Active_Image);
            annotatedImage();
        }

        /// <summary>
        /// Saves the annotation into the database.
        /// </summary>
        /// <param name="e"></param>
        void annotationForm_annotationSavedEvent(Args e)
        {
            if (!(bool)e["isAnnotationUpdate"])
            {

                string annoXml = e["xml"] as string;
                NewDataVariables.Active_Obs.annotationsAvailable = true;
                INTUSOFT.Data.NewDbModel.eye_fundus_image_annotation annotation = new INTUSOFT.Data.NewDbModel.eye_fundus_image_annotation();
                thumbnailData.id = Convert.ToInt32(e["imageid"]);
                annotation.eyeFundusImage = NewDataVariables.Active_Obs;
                //This code has been added to insert user id into the table has to be removed once user or admin has been added and has be replaced by active user.
                users user = users.CreateNewUsers();
                user.userId = 1;
                annotation.createdBy = user;
                annotation.createdDate = DateTime.Now;
                annotation.cdrPresent = false;
                annotation.dataXml = annoXml;
                annotation.comments = e["Comments"] as string;
                NewDataVariables.Active_Obs.eye_fundus_image_annotations.Add(annotation);
                //NewIVLDataMethods.AddAnnotation(annotation);
                //NewDataVariables._Repo.Update<eye_fundus_image>(NewDataVariables.Active_Obs);//This code has been added to update the active image model after its isannoted status has been chnanged.
                //PatientDetais_update();
                annotatedImage();
                NewIVLDataMethods.UpdatePatient();
            }
            else
            {
                NewDataVariables.Active_Annotation = NewDataVariables.Annotations.Where(x => x.eyeFundusImageAnnotationId == (Convert.ToInt32(e["annotationID"]))).ToList()[0];
                string annoXml = e["xml"] as string;
                NewDataVariables.Active_Annotation.dataXml = annoXml;
                NewIVLDataMethods.UpdatePatient();
            }
        }

        /// <summary>
        /// Update the patient modifieddatetime when annotation is added or deleted
        /// </summary>
        void annotationForm__patientdetailsupdate()
        {
            NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].patientLastModifiedDate = DateTime.Now;
            NewIVLDataMethods.UpdatePatient();
            NewDataVariables.Visits.Reverse();
        }

        /// <summary>
        /// Opens report window to create a new report.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newReport_btn_Click(object sender, EventArgs e)
        {
            Args arg = new Args();
            arg["isDefault"] = false;
            eventHandler.Notify(eventHandler.UpdateMainWindowCursor, arg);//added to make the wait cursor to appear by kishore on 18 September 2017.

            IVLVariables.IsAnotherWindowOpen = true;
            if (!noImageSelected_lbl.Visible)//to check whether no images label is not visible.
                CreateReport();
            else
            {
                CustomMessageBoxPopUp(IVLVariables.LangResourceManager.GetString("Report_Image_WarningMsg_Text", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("Report_Image_WarningHeader_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxButtons.OK, CustomMessageBoxIcon.Warning);
            }
            //this.Cursor = Cursors.Default;
            arg["isDefault"] = true;
            eventHandler.Notify(eventHandler.UpdateMainWindowCursor, arg);//added to make the default cursor to appear by kishore on 18 September 2017.
        }

        public void CreateCloudFileInOutbox()
        {
            noofimages();

            if (images.Length <= Convert.ToInt32(IVLVariables.CurrentSettings.UserSettings._noOfImagesForReport.val))
            {
                if(IVLVariables.isInternetConnected)
                {
                    PopulateModelsForMandaraUpload();
                    CustomMessageBoxPopUp(currentReportImageFiles.Length.ToString() + " " + IVLVariables.LangResourceManager.GetString("UploadConfirmation_Text", IVLVariables.LangResourceCultureInfo), "Upload", CustomMessageBoxButtons.OK, CustomMessageBoxIcon.Information);


                }
                else
                {
                    CustomMessageBoxPopUp(IVLVariables.LangResourceManager.GetString("No_Internet_Text", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("Internet_Connection_Status_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxButtons.OK, CustomMessageBoxIcon.Information);

                }

                //showExisitingReports();

            }
            else
            {
                CustomMessageBoxPopUp(NoOfImagesToBeSelectedText1 + " " + NoOfImagesToBeSelected.ToString() + " " + NoOfImagesToBeSelectedText2, NoOfImagesToBeSelectedHeader, CustomMessageBoxButtons.OK, CustomMessageBoxIcon.Information);
            }
        }
        bool isUpload = false;
        private void PopulateModelsForMandaraUpload()
        {
            if (!isUpload)
            {
                isUpload = true;
                INTUSOFT.Data.NewDbModel.report report = new INTUSOFT.Data.NewDbModel.report();
                report.dataJson = "";
                report.createdDate = DateTime.Now;
                report.visit = NewDataVariables.Active_Visit;
                //This code has been added to insert user id into the table has to be removed once user or admin has been added and has be replaced by active user.
                users user = users.CreateNewUsers();
                user.userId = 1;
                ReportType rept = ReportType.CreateNewReportType();
                rept.reportTypeId = 1;
                report.lastModifiedDate = DateTime.Now;
                report.createdBy = user;
                report.report_type = rept;
                report.isCloudReport = true;
                report.Patient = NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0];
                NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].visits.Where(y => y.visitId == NewDataVariables.Active_Visit.visitId).ToList()[0].reports.Add(report);
                //NewDataVariables.Active_Visit.reports.Add(report);
                //NewIVLDataMethods.AddReport(report);
                //NewIVLDataMethods.UpdateVisit();
                NewDataVariables._Repo.Add<report>(report);

                CloudAnalysisReport cloudAnalysisReport = CloudAnalysisReport.CreateCloudAnalysisReport();
                cloudAnalysisReport.Report = report;
                cloudAnalysisReport.cloudAnalysisReportStatus = (int)CloudReportStatus.Initialized;
                NewDataVariables._Repo.Add<CloudAnalysisReport>(cloudAnalysisReport);

                CloudModel cloudModel = new CloudModel();
                cloudModel.reportID = report.reportId;
                cloudModel.visitID = NewDataVariables.Active_Visit.visitId;
                cloudModel.patientID = NewDataVariables.Active_Patient;
                cloudModel.cloudID = cloudAnalysisReport.cloudAnalysisReportId;

                cloudModel.LoginModel.URL_Model.API_URL = IVLVariables.CurrentSettings.CloudSettings.API_URL.val;
                cloudModel.LoginModel.URL_Model.API_URL_Start_Point = IVLVariables.CurrentSettings.CloudSettings.API_LOGIN_URL.val;

                cloudModel.LoginModel.username = IVLVariables.CurrentSettings.CloudSettings.Username.val;
                cloudModel.LoginModel.password = IVLVariables.CurrentSettings.CloudSettings.Password.val;

                cloudModel.LoginModel.device_id = IVLVariables.CurrentSettings.CameraSettings.DeviceID.val;

                Dictionary<string, object> kvp = new Dictionary<string, object>();
                cloudModel.LoginModel.Body = string.Empty;
                LoginResponseModel reponse = new LoginResponseModel();
                reponse.message = new message();


                cloudModel.CreateAnalysisModel.URL_Model.API_URL = IVLVariables.CurrentSettings.CloudSettings.API_URL.val; ;
                cloudModel.CreateAnalysisModel.URL_Model.API_URL_Start_Point = IVLVariables.CurrentSettings.CloudSettings.API_ANALYSES.val; ;

                cloudModel.CreateAnalysisModel.sample_id = IVLVariables.MRN;
                cloudModel.CreateAnalysisModel.age = IVLVariables.patAge.ToString();
                cloudModel.CreateAnalysisModel.gender = IVLVariables.patGender[0].ToString();
                cloudModel.CreateAnalysisModel.sample_desc = IVLVariables.patAge.ToString() + "yrs, " + IVLVariables.patGender[0].ToString();
                cloudModel.CreateAnalysisModel.visitID = IVLVariables.ActiveVisitID;
                cloudModel.CreateAnalysisModel.PatientID = IVLVariables.ActivePatID;


                cloudModel.CreateAnalysisModel.Body = string.Empty;

                //cloudModel.UploadModel.relative_path = cloudModel.CreateAnalysisModel.sample_id + Path.DirectorySeparatorChar + "LE" + Path.DirectorySeparatorChar + "UnsharpMask.jpg";
                cloudModel.UploadModel.URL_Model.API_URL = IVLVariables.CurrentSettings.CloudSettings.API_URL.val;
                cloudModel.UploadModel.URL_Model.API_URL_Start_Point = IVLVariables.CurrentSettings.CloudSettings.API_ANALYSES.val;
                cloudModel.UploadModel.URL_Model.API_URL_End_Point = IVLVariables.CurrentSettings.CloudSettings.API_ANALYSES_INPUT.val;
                cloudModel.UploadModel.RetryCount = 3;
                cloudModel.UploadModel.images = currentReportImageFiles;
                cloudModel.UploadModel.checksums = new string[currentReportImageFiles.Length];
                cloudModel.UploadModel.relative_path = new string[currentReportImageFiles.Length];
                cloudModel.UploadModel.eyeSideArr = currentReportLabelNames;
                for (int i = 0; i < currentReportImageFiles.Length; i++)
                {
                    FileInfo ImgFinf = new FileInfo(currentReportImageFiles[i]);
                   eye_fundus_image eye =  NewDataVariables.GetCurrentPat().observations.Where(x => x.value == ImgFinf.Name).ToList()[0];
                    if (string.IsNullOrEmpty(eye.checkSum))
                    {
                        eye.checkSum = (ImgFinf).GetMd5Hash();
                        NewDataVariables._Repo.Update<eye_fundus_image>(eye);
                    }
                    cloudModel.UploadModel.checksums[i] = eye.checkSum;
                    cloudModel.UploadModel.relative_path[i] = IVLVariables.MRN + "/" + (currentReportLabelNames[i].Contains("OD") ? "RE" : "LE") + "/" + ImgFinf.Name;
                }

                cloudModel.InitiateAnalysisModel.status = "initialised";

                cloudModel.InitiateAnalysisModel.URL_Model.API_URL = IVLVariables.CurrentSettings.CloudSettings.API_URL.val;
                cloudModel.InitiateAnalysisModel.URL_Model.API_URL_Start_Point = IVLVariables.CurrentSettings.CloudSettings.API_ANALYSES.val;
                cloudModel.InitiateAnalysisModel.URL_Model.API_URL_End_Point = IVLVariables.CurrentSettings.CloudSettings.API_STATUS_URL.val;

                cloudModel.InitiateAnalysisModel.Body = string.Empty;

                cloudModel.GetAnalysisModel.analysis_id = cloudModel.UploadModel.analysis_id;
                cloudModel.GetAnalysisModel.URL_Model.API_URL = IVLVariables.CurrentSettings.CloudSettings.API_URL.val;
                cloudModel.GetAnalysisModel.URL_Model.API_URL_Start_Point = IVLVariables.CurrentSettings.CloudSettings.API_ANALYSES.val;

                cloudModel.GetAnalysisResultModel.URL_Model.API_URL = IVLVariables.CurrentSettings.CloudSettings.API_URL.val;

                var cloudModelJson = JsonConvert.SerializeObject(cloudModel, Newtonsoft.Json.Formatting.Indented);
                var outboxFilePath = Path.Combine(IVLVariables.GetCloudDirPath(DirectoryEnum.OutboxDir), DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".json");
                File.WriteAllText(outboxFilePath, cloudModelJson);
                cloudAnalysisReport.fileName = new FileInfo(outboxFilePath).Name;
                NewDataVariables._Repo.Update(cloudAnalysisReport);

                report r = NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].visits.Where(y => y.visitId == NewDataVariables.Active_Visit.visitId).ToList()[0].reports.Where(x => x.reportId == cloudAnalysisReport.Report.reportId).ToList()[0];
                r.cloudReports = new HashSet<CloudAnalysisReport>(new CloudAnalysisReport[] { cloudAnalysisReport }.ToList());
                NewDataVariables.CloudAnalysisReports.Add(cloudAnalysisReport);
                NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].visits.Where(y => y.visitId == NewDataVariables.Active_Visit.visitId).ToList()[0].reports.Where(x => x.reportId == cloudAnalysisReport.Report.reportId).ToList()[0] = r;
                showExisitingReports();
                isUpload = false;
            }

        }
        private void CreateReport()
        {
            try
            //string[] reportTemplates = System.IO.Directory.GetFiles(@"ReportTemplates");
            //if (reportTemplates.Length > 0)
            {
                //string x = "f";
                //int y = 5 + Convert.ToInt32(x);

                //if (reportTemplates.Contains(IVLVariables.LangResourceManager.GetString("LandscapexmlFileName_Text", IVLVariables.LangResourceCultureInfo)) && reportTemplates.Contains(IVLVariables.LangResourceManager.GetString("PortraitxmlFileName_Text", IVLVariables.LangResourceCultureInfo)) && File.Exists(IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value))
                {
                    //MicroStopwatch stopwatch = new MicroStopwatch();
                    //stopwatch.Start();
                    noofimages();
                    IVLVariables.IsAnotherWindowOpen = true;
                    if (images.Length <= Convert.ToInt32(IVLVariables.CurrentSettings.UserSettings._noOfImagesForReport.val))
                    {
                        this.Cursor = Cursors.WaitCursor;
                        if (!IVLVariables.isCommandLineArgsPresent)
                            RefreshViewScreenControls();
                        isView = false;
                        IVLReport.Report.isNew = true;
                        //getReportDetails();
                        //stopwatch.Stop();
                        if (getReportDetails())
                        {

                            Args arg = new Args();
                            arg["isExport"] = false;
                            // eventHandler.Notify(eventHandler.GetImageFiles, arg);
                            // string[] image = currentReportLabelNames;
                            //This below bool variable is added by Darshan on 21-08-2015 to solve Defect no:0000585: annotation window,report window,view full info,user settings window, when trigger press capture is happening.
                            //IVLVariables.ivl_Camera.TriggerOff();
                            IVLVariables.isReportWindowOpen = true;
                            if (_report.ShowDialog() == DialogResult.Cancel)
                            {
                                if (IVLVariables.isCommandLineArgsPresent)//if command line args are present exit the application and avoid saving of report
                                {
                                    Application.Exit();

                                    //Load_Image();
                                    //IVLVariables.isReportWindowOpen = false;
                                    //if (!noImageSelected_lbl.Visible)
                                    //{
                                    //    arg["rawImage"] = OriginalBm;
                                    //    eventHandler.Notify(eventHandler.DisplayImage, arg);
                                    //}
                                    //IVLVariables.ivl_Camera.TriggerOn();
                                }
                                //else
                                //{
                                //    Application.Exit();
                                //}
                                IVLVariables.IsAnotherWindowOpen = false;
                                //IVLVariables.ivl_Camera.TriggerOn();


                            }
                            //This below bool variable is added by Darshan on 21-08-2015 to solve Defect no:0000585: annotation window,report window,view full info,user settings window, when trigger press capture is happening.
                            reportDic.Clear();
                        }
                        else
                            this.Cursor = Cursors.Default;
                    }
                    else
                    {
                        CustomMessageBoxPopUp(NoOfImagesToBeSelectedText1 + " " + NoOfImagesToBeSelected.ToString() + " " + NoOfImagesToBeSelectedText2, NoOfImagesToBeSelectedHeader, CustomMessageBoxButtons.OK, CustomMessageBoxIcon.Information);
                    }
                }

            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, ExceptionLog);
                //                ExceptionLog.Debug(IVLVariables.ExceptionLog.ConvertException2String(ex));
            }
            //else
            //{
            //    Common.CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("ReportWarning_Text", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("Report_text", IVLVariables.LangResourceCultureInfo), Common.CustomMessageBoxIcon.Warning);
            //}
        }

        private void createReportEvent(string s, Args arg)
        {
            CreateReport();
        }

        /// <summary>
        /// Changes the selected rows when u or d is presses.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reports_dgv_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Control)
            {
                e.Handled = true;
            }
            else if (!IVLVariables.ivl_Camera.CameraIsLive && e.KeyCode == Keys.D && Reports_dgv.RowCount > 0)
            {
                if (Reports_dgv.SelectedRows[0].Index >= 0 && Reports_dgv.SelectedRows[0].Index < Reports_dgv.RowCount - 1)
                {
                    {
                        int currentIndx = Reports_dgv.SelectedRows[0].Index;
                        Reports_dgv.Rows[currentIndx + 1].Selected = true;
                    }
                }
            }
            else
                if (!IVLVariables.ivl_Camera.CameraIsLive && e.KeyCode == Keys.U && Reports_dgv.RowCount > 0)
            {
                if (Reports_dgv.SelectedRows[0].Index < Reports_dgv.Rows.Count && Reports_dgv.SelectedRows[0].Index > 0)
                {
                    int currentIndx = Reports_dgv.SelectedRows[0].Index;
                    Reports_dgv.Rows[currentIndx - 1].Selected = true;
                }
            }
            //This below code has been added by Darshan to Resolve Defect no 0000473: The Reports grid and the thumbnails are simultaneously selected.
            else if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Up)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Opens the Glaucoma tool window to add CDR.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void glaucomaTool_btn_Click(object sender, EventArgs e)
        {
            Args arg1 = new Args();
            arg1["isDefault"] = false;
            eventHandler.Notify(eventHandler.UpdateMainWindowCursor, arg1);//added to show the wait cursor by kishore on 18 September 2017.
            //This line has been added to refresh the color of the buutons when clicked on the CDR button.
            //RefreshViewScreenControls();
            //This below if statement has been added by Darshan on 28-10-2015 to solve Defect no 0000729: No image selected,CDR values are accepted.
            #region glaucomaTool
            if (!noImageSelected_lbl.Visible)
            {
                RefreshViewScreenControls();
                //This below if statement has been added by Darshan on 28-10-2015 to solve Defect no 0000670: More than one image selected,CDR page can be opened.
                IVLVariables.IsAnotherWindowOpen = true;
                Args arg = new Args();
                arg["isExport"] = false;
                eventHandler.Notify(eventHandler.GetImageFiles, arg);
                string[] image = currentReportLabelNames;
                if (image.Length > 1)
                {
                    CustomMessageBoxPopUp(IVLVariables.LangResourceManager.GetString("AnnotationNo_Images", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("GlaucomaTool_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Warning);
                }
                else
                {
                    if (getReportDetails())
                    {
                        //method called to populate UITextValues dic with reportDic. 23-08-2017
                        Dictionary<string, object> UITextValues = new Dictionary<string, object>(reportDic); // changed dictionary value type from string to object . By Ashutosh 18-08-2017, reportDic given on 23-08-2017.
                        UITextValues["$OD"] = (!isLeft).ToString();
                        UITextValues["$FormText"] = IVLVariables.LangResourceManager.GetString("GlaucomaTool_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$DrawTools"] = IVLVariables.LangResourceManager.GetString("DrawTools_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$DrawCup"] = IVLVariables.LangResourceManager.GetString("drawCup_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$DrawDisc"] = IVLVariables.LangResourceManager.GetString("drawDisc_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$EditDisc"] = IVLVariables.LangResourceManager.GetString("EditDiscPointsText", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$save"] = IVLVariables.LangResourceManager.GetString("ImageViewer_Save_Button_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$print"] = IVLVariables.LangResourceManager.GetString("Print_Print_Button_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$filters"] = IVLVariables.LangResourceManager.GetString("Filter_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$redFilter"] = IVLVariables.LangResourceManager.GetString("RedFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$greenFilter"] = IVLVariables.LangResourceManager.GetString("GreenFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$blueFilter"] = IVLVariables.LangResourceManager.GetString("BlueFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$redFilterText"] = IVLVariables.LangResourceManager.GetString("RedFilter_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$greenFilterText"] = IVLVariables.LangResourceManager.GetString("GreenFilter_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$blueFilterText"] = IVLVariables.LangResourceManager.GetString("BlueFilter_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$colorFilter"] = IVLVariables.LangResourceManager.GetString("ColorFilter_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$measurements"] = IVLVariables.LangResourceManager.GetString("CDR_measurements_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$areaCup"] = IVLVariables.LangResourceManager.GetString("cupArea_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$areaDisc"] = IVLVariables.LangResourceManager.GetString("DiscArea_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$areaRim"] = IVLVariables.LangResourceManager.GetString("RimArea_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$verticalCDR"] = IVLVariables.LangResourceManager.GetString("VerticalCDR_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$horizontalCDR"] = IVLVariables.LangResourceManager.GetString("HorizontalCDR_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$verticalCupLen"] = IVLVariables.LangResourceManager.GetString("VerticalCupLength_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$horizontalCupLen"] = IVLVariables.LangResourceManager.GetString("HorizontalCupLength_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$VerticalDiscLen"] = IVLVariables.LangResourceManager.GetString("VerticalDiscLength_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$horizontalDiscLen"] = IVLVariables.LangResourceManager.GetString("HorizontalDiscLength_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$superior"] = IVLVariables.LangResourceManager.GetString("Superior_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$inferior"] = IVLVariables.LangResourceManager.GetString("Inferior_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$nasal"] = IVLVariables.LangResourceManager.GetString("Nasal_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$temporal"] = IVLVariables.LangResourceManager.GetString("Temporal_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$millimeterText"] = IVLVariables.LangResourceManager.GetString("Millimeter_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$EqualText"] = IVLVariables.LangResourceManager.GetString("Equal_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$Comments"] = IVLVariables.LangResourceManager.GetString("Comments_Label_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$EditCupPoints"] = IVLVariables.LangResourceManager.GetString("GlaucomaTool_EditCupPoints", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$EditDiscPoints"] = IVLVariables.LangResourceManager.GetString("GlaucomaTool_EditDiscPoints", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$saveConfirmation"] = IVLVariables.LangResourceManager.GetString("GlaucomaTool_SaveConfirmation", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$repotedBy"] = IVLVariables.LangResourceManager.GetString("Reported_By_Text", IVLVariables.LangResourceCultureInfo);//Contain the Reported By text.
                        UITextValues["$addCupPointsWarning"] = IVLVariables.LangResourceManager.GetString("AddCupPointsWarning", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$addDiscPointsWarning"] = IVLVariables.LangResourceManager.GetString("AddDiscPointsWarning", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$warningHeader"] = IVLVariables.LangResourceManager.GetString("CDRWarning_Header_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$calulateCDRText"] = IVLVariables.LangResourceManager.GetString("CalculateCDR_btn", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$saveBtnText"] = IVLVariables.LangResourceManager.GetString("SaveHeader_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$printBtnText"] = IVLVariables.LangResourceManager.GetString("Print_Btn_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$reportNameText"] = IVLVariables.LangResourceManager.GetString("Report_Name_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$reportGenderText"] = IVLVariables.LangResourceManager.GetString("Report_Gender_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$reportAgeText"] = IVLVariables.LangResourceManager.GetString("Report_Age_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$reportCommentsText"] = IVLVariables.LangResourceManager.GetString("Report_Comments_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$reportDateText"] = IVLVariables.LangResourceManager.GetString("Report_DateTemplate_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$reportDoctorText"] = IVLVariables.LangResourceManager.GetString("Report_Doctor_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$saveWarningText"] = IVLVariables.LangResourceManager.GetString("SaveCdrAlert_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$saveWarningHeader"] = IVLVariables.LangResourceManager.GetString("SaveCDRheader_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$deleteDiscText"] = IVLVariables.LangResourceManager.GetString("CDR_Disc_Deletion_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$deleteDiscHeader"] = IVLVariables.LangResourceManager.GetString("CDR_Disc_Deletion_Header", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$printWarningText"] = IVLVariables.LangResourceManager.GetString("PrintCdrAlert_Text1", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$DrawDiscButtonToolTipText"] = IVLVariables.LangResourceManager.GetString("DrawDiscButton_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$DrawDiscPointsButtonToolTipText"] = IVLVariables.LangResourceManager.GetString("DrawDiscPointsButton_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$DrawCupButtonToolTipText"] = IVLVariables.LangResourceManager.GetString("DrawCupButton_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$CalculateCDRButtonToolTipText"] = IVLVariables.LangResourceManager.GetString("CalculateCDRButton_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$SaveCDRButtonToolTipText"] = IVLVariables.LangResourceManager.GetString("SaveCDRButton_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$PrintCDRButtonToolTipText"] = IVLVariables.LangResourceManager.GetString("PrintCDRButton_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$ExportCDRButtonToolTipText"] = IVLVariables.LangResourceManager.GetString("ExportCDRButton_ToolTipText", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$ExportButtonText"] = IVLVariables.LangResourceManager.GetString("Export_Button_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$FileToolsGbxText"] = IVLVariables.LangResourceManager.GetString("FileTools_Gbx_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$CompanyLogo"] = @"ImageResources\LogoImageResources\companyLogo.jpg";
                        UITextValues["$HospitalLogo"] = @"ImageResources\LogoImageResources\hospitalLogo.png";
                        if (!UITextValues.ContainsKey("$exportReportText"))//checks if key is not  present. By Ashutosh 23-08-2017.
                            UITextValues.Add("$exportReportText", IVLVariables.LangResourceManager.GetString("Export_Report_Text", IVLVariables.LangResourceCultureInfo));// adding key  to dictionary . ashutosh 20/7
                        if (!UITextValues.ContainsKey("$exportReportHeader"))//checks if key is not  present. By Ashutosh 23-08-2017.
                            UITextValues.Add("$exportReportHeader", IVLVariables.LangResourceManager.GetString("Export_Button_Text", IVLVariables.LangResourceCultureInfo));// adding key  to dictionary . ashutosh 20/7
                        if (!UITextValues.ContainsKey("$customFolderData"))//checks if key is not  present. By Ashutosh 23-08-2017.
                            UITextValues.Add("$customFolderData", customFolderData); //to add the custom folder data dictionary to the  dictionary. By Ashutosh 18-08-2017

                        NewDataVariables.Annotations = NewDataVariables._Repo.GetByCategory<eye_fundus_image_annotation>("eyeFundusImage", NewDataVariables.Active_Obs).Where(x => x.voided == false).ToList();//.Annotations.ToList();
                        NewDataVariables.Annotations = NewDataVariables.Annotations.OrderBy(x => x.createdDate).ToList();
                        NewDataVariables.Annotations.Reverse();
                        int id = 0;
                        string xml = "";
                        if (NewDataVariables.Active_Obs.cdrAnnotationAvailable)
                        {
                            //This below code has been added by Darshan on 05-10-2015 to show only CDR graph in CDR window.
                            for (int i = 0; i < NewDataVariables.Annotations.Count; i++)
                            {
                                // try
                                {
                                    checkforCDR(NewDataVariables.Annotations[i].dataXml);
                                }
                                //catch (Exception ex)
                                //{

                                //    throw;
                                //}

                                if (isCDRvalue)
                                {
                                    xml = NewDataVariables.Annotations[i].dataXml;
                                    //id is added by Darshan 27-10-2015 to get the ID of the perticular glaucoma to get old glaucoma updated.
                                    id = NewDataVariables.Annotations[i].eyeFundusImageAnnotationId;
                                    break;
                                }
                            }
                        }
                        //Below code has been added to send patient details and information related to image to CDR form.
                        Dictionary<string, object> imageDetails = new Dictionary<string, object>();
                        string imageName;
                        try
                        {
                            imageName = currentReportLabelNames[0];
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                        int side = 0;

                        // UITextValues["$visitImages"] = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value;
                        // UITextValues["$visitImageIds"] = NewDataVariables.Active_EyeFundusImage.eyeFundusImageId.ToString();
                        if (UITextValues.ContainsKey("$NameOfTheReport"))//checks if key $NameOfTheReport is present .By Ashutosh 17-08-2017
                            UITextValues["$NameOfTheReport"] = IVLVariables.CurrentSettings.ReportSettings.NameOfCDRReport.val.ToString();// if present then it's value is replaced.By Ashutosh 17-08-2017
                        else
                            UITextValues.Add("$NameOfTheReport", IVLVariables.CurrentSettings.ReportSettings.NameOfCDRReport.val.ToString());// if not present then key and value are added.By Ashutosh 17-08-2017

                        if (NewDataVariables.Active_Obs.eyeSide == 'L')
                        {
                            side = 1;
                        }
                        else
                        {
                            side = 0;
                        }
                        string path = string.Empty;
                        if (Convert.ToBoolean(IVLVariables.CurrentSettings.ImageStorageSettings.IsMrnFolder.val))
                            path = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString() + Path.DirectorySeparatorChar + NewDataVariables.Active_PatientIdentifier.value + Path.DirectorySeparatorChar + NewDataVariables.Active_Visit.createdDate.Date.ToString("dd_MM_yyyy");
                        else
                            path = IVLVariables.CurrentSettings.ImageStorageSettings._LocalProcessedImagePath.val.ToString();
                        UITextValues["$CurrentImageFiles"] = new string[] { path + Path.DirectorySeparatorChar + NewDataVariables.Active_Obs.value };
                        UITextValues["$ImageNames"] = new string[] { imageName.ToString() };
                        UITextValues["$visitImageIds"] = new int[] { id };//.ToString();
                        UITextValues["$visitImageSides"] = new string[] { side.ToString() };
                        UITextValues["$currentTemplate"] = IVLVariables.appDirPathName + Path.Combine(new string[] { "ReportTemplates", "CDR", "Normal", "CDR_lsA4.xml" });
                        UITextValues.Add("$xml", xml);

                        //UITextValues["Name"] = IVLVariables.patName;
                        //UITextValues["Age"] = IVLVariables.patAge.ToString();
                        //UITextValues["MRN"] = IVLVariables.MRN;
                        //UITextValues["imgannotated"] = NewDataVariables.Active_EyeFundusImage.annotationsAvailable.ToString();
                        //UITextValues["isCDR"] = NewDataVariables.Active_EyeFundusImage.cdrAnnotationAvailable.ToString();
                        //UITextValues["Datetime"] = DateTime.Today.ToString("dd-MMM-yy");//The date format has been changed to maintain a uniform date format.
                        //UITextValues["Gender"] = IVLVariables.patGender;
                        //UITextValues["HospitalName"] = IVLVariables.CurrentSettings.UserSettings._HeaderText.val;
                        //UITextValues["Doctor"] = IVLVariables.CurrentSettings.UserSettings._DoctorName.val;
                        //UITextValues["adobereadernotpresent"] = IVLVariables.LangResourceManager.GetString("Adobe_acrobat_Text", IVLVariables.LangResourceCultureInfo);
                        //UITextValues["NoImages"] = IVLVariables.LangResourceManager.GetString("NoImagesSelected_Label_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$ReportPdfOpenMessage"] = IVLVariables.LangResourceManager.GetString("ReporttPdfFileOpenMessage_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$ReportPdfOpenHeader"] = IVLVariables.LangResourceManager.GetString("ReporttPdfFileOpenHeader_Text", IVLVariables.LangResourceCultureInfo);
                        UITextValues["$annotationMarkingColor"] = IVLVariables.CurrentSettings.AnnotationColorSelection._AnnotationMarkingColor.val.ToString();
                        UITextValues["$cupColor"] = IVLVariables.CurrentSettings.AnnotationColorSelection._CupColor.val.ToString();
                        UITextValues["$discColor"] = IVLVariables.CurrentSettings.AnnotationColorSelection._DiscColor.val.ToString();
                        //UITextValues["Address1"] = IVLVariables.CurrentSettings.ReportSettings.Address1.val;
                        //UITextValues["Address2"] = IVLVariables.CurrentSettings.ReportSettings.Address2.val;
                        //This below code has been changed because the previous IVLVariables.ActiveImageID was not getting updated.
                        UITextValues.Add("$is24hrformat", Convert.ToBoolean(IVLVariables.CurrentSettings.UserSettings._Is24clock.val));
                        //UITextValues.Add("$Color1", IVLVariables.GradientColorValues.Color1);
                        //UITextValues.Add("$Color2", IVLVariables.GradientColorValues.Color2);
                        //UITextValues.Add("$FontColor", IVLVariables.GradientColorValues.FontForeColor);
                        //UITextValues.Add("$ColorAngle", IVLVariables.GradientColorValues.ColorAngle);
                        Annotation.GlaucomaTool glaucomaTool = new GlaucomaTool(UITextValues);
                        glaucomaTool.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
                        glaucomaTool.WindowState = FormWindowState.Maximized;
                        //This below bool variable is added by Darshan on 21-08-2015 to solve Defect no:0000585: annotation window,report window,view full info,user settings window, when trigger press capture is happening.
                        glaucomaTool.annotationSavedEvent += glaucomaTool_annotationSavedEvent;
                        //IVLVariables.ivl_Camera.TriggerOff();
                        if (glaucomaTool.ShowDialog() == DialogResult.Cancel)
                        {
                            //This below bool variable is added by Darshan on 21-08-2015 to solve Defect no:0000585: annotation window,report window,view full info,user settings window, when trigger press capture is happening.
                            IVLVariables.IsAnotherWindowOpen = false;
                            Load_Image();
                            arg["rawImage"] = OriginalBm;
                            eventHandler.Notify(eventHandler.DisplayImage, arg);
                        }
                        else
                            return;
                    }

                }

            }
            else
                CustomMessageBoxPopUp(IVLVariables.LangResourceManager.GetString("GlaucomaTool_Image_Warning_Text", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("GlaucomaTool_Text", IVLVariables.LangResourceCultureInfo), Common.CustomMessageBoxIcon.Warning);

            arg1["isDefault"] = true;
            eventHandler.Notify(eventHandler.UpdateMainWindowCursor, arg1);//added to make the cursor default by kishore on 18 September 2017.
            #endregion
        }

        /// <summary>
        /// Save the CDR details into the DB.
        /// </summary>
        /// <param name="e"></param>
        void glaucomaTool_annotationSavedEvent(Args e)
        {
            if (!(bool)e["isAnnotationUpdate"])
            {
                string annoXml = e["xml"] as string;
                NewDataVariables.Active_Obs.annotationsAvailable = true;
                NewDataVariables.Active_Obs.cdrAnnotationAvailable = true;
                //NewDataVariables._Repo.Update<eye_fundus_image>(NewDataVariables.Active_Obs);
                INTUSOFT.Data.NewDbModel.eye_fundus_image_annotation annotation = new INTUSOFT.Data.NewDbModel.eye_fundus_image_annotation();
                thumbnailData.id = Convert.ToInt32(e["imageid"]);
                annotation.eyeFundusImage = NewDataVariables.Active_Obs;
                //This code has been added to insert user id into the table has to be removed once user or admin has been added and has be replaced by active user.
                users user = users.CreateNewUsers();
                user.userId = 1;
                annotation.createdBy = user;
                annotation.createdDate = DateTime.Now;
                annotation.dataXml = annoXml;
                annotation.cdrPresent = true;
                annotation.comments = e["Comments"] as string;
                NewDataVariables.Active_Obs.eye_fundus_image_annotations.Add(annotation);
                NewIVLDataMethods.UpdatePatient();
                //NewIVLDataMethods.AddAnnotation(annotation);
                //NewIVLDataMethods.UpdateImage();
                //PatientDetais_update();
                annotatedImage();
            }
            else
            {
                INTUSOFT.Data.NewDbModel.eye_fundus_image_annotation annotation = NewDataVariables._Repo.GetById<eye_fundus_image_annotation>(Convert.ToInt32(e["annotationID"]));
                string annoXml = e["xml"] as string;
                annotation.dataXml = annoXml;
                NewDataVariables._Repo.Update<eye_fundus_image_annotation>(annotation);
            }
        }

        /// <summary>
        /// Disable the movement of the brightness_rb scroll when up or down arrow is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void brightness_rb_KeyDown(object sender, KeyEventArgs e)
        {
            {
                if (e.KeyCode == Keys.Down)
                    e.Handled = true;
                else if (e.KeyCode == Keys.Up)
                    e.Handled = true;
            }
        }

        /// <summary>
        /// Disable the movement of the contrast_rb scroll when up or down arrow is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contrast_rb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
                e.Handled = true;
            else if (e.KeyCode == Keys.Up)
                e.Handled = true;
        }

        /// <summary>
        /// Disable the movement of the zoomMag_rb scroll when up or down arrow is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zoomMag_rb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
                e.Handled = true;
            else if (e.KeyCode == Keys.Up)
                e.Handled = true;
        }

        /// <summary>
        /// Open and shows a existing report with details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reports_dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !isGridPopulating )
            {
                IVLVariables.ivl_Camera.TriggerOff();//trigger is made  off so as to prevent it from receiving trigger notification when the report is loading.
                if (IVLVariables.isValueChanged)
                {
                    RefreshViewScreenControls();
                }
                this.Cursor = Cursors.WaitCursor;
                IVLReport.Report.isNew = false;
                isView = true;

                INTUSOFT.Data.NewDbModel.report reportVal = NewDataVariables.Reports.Find(x=>x.reportId == Convert.ToInt32(Reports_dgv.Rows[e.RowIndex].Cells["reportId"].Value));
                ReportCreatedDateTime = reportVal.createdDate;
                if (reportVal.isCloudReport)

                {
                  List<CloudAnalysisReport> cloudReports =  reportVal.cloudReports.ToList();
                    if (cloudReports.Any())
                    {
                        if (cloudReports[0].cloudAnalysisReportStatus == (int)CloudReportStatus.View)
                        {
                            getReportDetails();
                            string reportXml = reportVal.dataJson;
                            if (_report.readXML(reportXml,true))
                            {
                                try
                                {
                                    _report.ShowDialog();
                                }
                                catch (Exception ex)
                                {
                                    throw;
                                }
                            }
                            else
                                this.Cursor = Cursors.Default;
                        }
                        else if (cloudReports[0].cloudAnalysisReportStatus == (int)CloudReportStatus.Failed)
                        {
                            this.Cursor = Cursors.Default;

                            CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("CloudAnalysisFailed_Text", IVLVariables.LangResourceCultureInfo)
                                , IVLVariables.LangResourceManager.GetString("AnalysisHeader_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxButtons.OK, CustomMessageBoxIcon.Information);
                        }
                        else
                        {
                            this.Cursor = Cursors.Default;

                            CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("CloudAnalsysisPendingStatus_Text", IVLVariables.LangResourceCultureInfo)
                                 , IVLVariables.LangResourceManager.GetString("AnalysisHeader_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxButtons.OK, CustomMessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    getReportDetails(true);

                    string reportXml = reportVal.dataJson;

                    if (_report.readXML(reportXml))
                    {
                        try
                        {
                            _report.ShowDialog();
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                    else
                        this.Cursor = Cursors.Default;
                }

            }
        }

        private void leftSide_btn_MouseHover(object sender, EventArgs e)
        {

            leftToolTip.SetToolTip(leftSide_btn, IVLVariables.LangResourceManager.GetString("OS_Tool_Tip_Text", IVLVariables.LangResourceCultureInfo));
        }

        private void rightSide_btn_MouseHover(object sender, EventArgs e)
        {

            rightToolTip.SetToolTip(rightSide_btn, IVLVariables.LangResourceManager.GetString("OD_Tool_Tip_Text", IVLVariables.LangResourceCultureInfo));
        }

        private void brightness_rb_MouseDown(object sender, MouseEventArgs e)
        {
            //noofimages();
            //if (images.Length > 1)
            //{
            //    CustomMessageBox.Show(IVLVariables.LangResourceManager.GetString("AnnotationNo_Images", IVLVariables.LangResourceCultureInfo), IVLVariables.LangResourceManager.GetString("Annotation_Text", IVLVariables.LangResourceCultureInfo), CustomMessageBoxIcon.Warning);
            //}

        }

        private void toolStrip5_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void tableLayoutPanel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void formButtons1_Click(object sender, EventArgs e)
        {
            //SplitScreen ss = new SplitScreen();

            //ss.ShowDialog();
        }
    }
    
}

#endregion
