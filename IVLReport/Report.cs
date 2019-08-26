using Common;
using Common.ValidatorDatas;
using INTUSOFT.Custom.Controls;
using Newtonsoft.Json;
using NLog;
using ReportUtils;
using ReportUtils.ReportEnums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using ZXing;

namespace IVLReport
{
    public partial class Report : BaseGradientForm
    {
        #region delegates and events
        public delegate void ReportSavedDelegate(Dictionary<string, object> reportVal, EventArgs e);//Delegate used for invoking save report event.
        public event ReportSavedDelegate reportSavedEvent;//Event used for saving the report details.
        public delegate void ReportClosedDelegate();//Delegate used for invoking close report event.
        public event ReportClosedDelegate reportClosedEvent;//Event used for closing the report details.
        public delegate void ReportDefaultCursorDelegate();//Delegate used for to invoke report default cursor.
        public event ReportDefaultCursorDelegate reportDefaultCursor;//Event which will be used to change from wait cursor to default cursor when report is loaded.
        #endregion

        #region Arrays and references
        public PdfGenerator pdfGenerator;//Object of type PdfGenerator.
        CustomFolderBrowser customFolderBrowser;//Object of type CustomFolderBrowser.
        Label noImages_lbl;//Object used to display the noimageselected text.
        public List<ReportControlsStructure> reportControlStructureList = null;//List of ReportControlStructure.
        EmailWindow emailWindow;//Object name for the EmailWindow.
        DataModel _dataModel;//Object name for the DataModel.
        string[] pageSize;//Saves the page sizes.
        List<FileInfo> reportTemplates;//List of templates available in report templates.
        ReportTemplateSizeEnum reportTemplateSize;//Enum of type ReportTemplateSizeEnum.
        ReportTemplateOrientationEnum reportTemplateOrientation;//Enum of type ReportTemplateSizeEnum.
        ReportImages _reportImagesForm;//Object of type ReportImages.
        Panel p;// = new Panel();
        #endregion

        #region constants
        private const int MAX_LINES = 3;//No of lines allowed to type in the left and right eye observation textbox.
        string xmlExtension = ".xml";//Extension for a xml file.
        string reportSizeA4 = "A4";//A4 size.
        string reportSizeA5 = "A5";//A5 size.
        private const int AsciiValueOfControlBackspace = 127;//Ascii value of ctrl+backspace key.
        private const char newlineChar = '\r';//New line character.
        #endregion

        #region variables
        bool isTextChanged = false;//Variable is used to determine any text changes in the doctor and comments text box.
        public bool isPrintingOver = false;//Determines wheather report printing is over or not.
        public bool isJsonFormat = true;//Variable has been added to maintain status of the report format weather it is xml or json.
        public string adobereader_text = string.Empty;//Variable used for displaying the warning text when adobe is not present.
        string Reportsave_message = string.Empty;//Variable used for displaying the text on custom message box when report is closed without saving.
        string Imagedeleted_message = string.Empty;
        string ErrorMessage_Title = string.Empty;

        string Reportsave_header = string.Empty;//Variable used for displaying the header on custom message box when report is closed without saving.
        public string ReportFileName = string.Empty;//Variable used to the set the name of the PDF generated.
        string noImageLabel = string.Empty;//Variable used the save the text when no images are selected to display.
        public static bool isNew = true;//Determines wheather the report is new or old.
        bool isDRReport = false;//Determines wheather the report is DR or normal.
        string reportSize = string.Empty;//Size of the current report.
        string reportOrientation = string.Empty;//Orientation of current report
        public string ReportStatusNew = string.Empty;//Variable used to the set the new report status.
        public string ReportStatusSaved = string.Empty;//Variable used to the set the saved report status.
        #endregion
        List<string> visitImageFilesList;
        private static readonly Logger Exception_Log = LogManager.GetLogger("ExceptionLog");
        private double a4Width = 11.69;
        private double a4Height = 8.27;

        private double a5Width = 8.27;
        private double a5Height = 5.83;
        private string userName = string.Empty;
        private string password = string.Empty;
        private string apiRequestType = String.Empty;
        Bitmap barcodeBitmap;
        public Report(Dictionary<string, object> reportData)
        {
            InitializeComponent();
            if (reportData.ContainsKey("$Color1"))
                this.Color1 = (Color)reportData["$Color1"];
            if (reportData.ContainsKey("$Color2"))
                this.Color2 = (Color)reportData["$Color2"];
            if (reportData.ContainsKey("$ColorAngle"))
                this.ColorAngle = (float)reportData["$ColorAngle"];
            if (reportData.ContainsKey("$FontColor"))
                this.FontColor = (Color)reportData["$FontColor"];
            if (reportData.ContainsKey("$userName"))
                userName = reportData["$userName"].ToString();
            if (reportData.ContainsKey("$password"))
                password = reportData["$password"].ToString();
            if (reportData.ContainsKey("$apiRequestType"))
                apiRequestType = reportData["$apiRequestType"].ToString();
            #region object initialization
            noImages_lbl = new Label();//Intializing object noImages_lbl.
            Dictionary<string, object> emailDic = new Dictionary<string, object>();
            if (reportData.ContainsKey("EmailDic"))
                emailDic = reportData["EmailDic"] as Dictionary<string, object>;
            emailWindow = new EmailWindow(emailDic);//Intializing object emailWindow.
            emailWindow._EmailSent += emailWindow__EmailSent;//Subscribing event _EmailSent.
            emailWindow._WaitCursor += emailWindow__WaitCursor;
            _dataModel = DataModel.GetInstance();//Intializing object _dataModel.
            pdfGenerator = new PdfGenerator();//Intializing object pdfGenerator.
            pageSize = new string[] { reportSizeA4, reportSizeA5 };//Intializing object pageSize.
            _reportImagesForm = new ReportImages(reportData);//Intializing object _reportImagesForm.
            #endregion

            #region logo intitialization
            //This Icon has been added by darshan on 13-08-2015 to resolve defect no 0000554: Report icon to be changed as in the Logo of Intuvision.
            string appLogoFilePath = @"ImageResources\LogoImageResources\ReportLogo.ico";//Setting path of the ReportLogo image to the appLogoFilePath.
            string printLogoPath = @"ImageResources\Edit_ImageResources\PrintIcon.png";//Setting path of the PrintIcon image to the printLogoPath.
            string saveLogoPath = @"ImageResources\Edit_ImageResources\SaveIcon.png";//Setting path of the SaveIcon image to the saveLogoPath.
            string exportLogoPath = @"ImageResources\Edit_ImageResources\Export_Image_Square.png";//Setting path of the Email-Report image to the exportLogoPath.
            string emailImagesLogoPath = @"ImageResources\Edit_ImageResources\Email-Images.png";//Setting path of the Email-Images image to the emailImagesLogoPath.
            string emailReportsLogoPath = @"ImageResources\Edit_ImageResources\Email-Report.png";//Setting path of the Email-Report image to the emailReportsLogoPath.
            string uploadLogoPath = @"ImageResources\CloudImageResources\Export2Cloud.png";//Setting path of the Export2Cloud image to the uploadLogoPath.
            string autoAnalysisLogoPath = @"ImageResources\PatientDetailsImageResources\AA.png";
            if (File.Exists(printLogoPath))//Checks wheather the file exists in the path given in printLogoPath.
                print_btn.Image = Image.FromFile(printLogoPath);//Set the image of print_btn from printLogoPath.
            if (File.Exists(saveLogoPath))//Checks wheather the file exists in the path given in saveLogoPath.
                save_btn.Image = Image.FromFile(saveLogoPath);//Set the image of save_btn from saveLogoPath.
            if (File.Exists(exportLogoPath))//Checks wheather the file exists in the path given in exportLogoPath.
                export_btn.Image = Image.FromFile(exportLogoPath);//Set the image of export_btn from exportLogoPath.
            if (File.Exists(emailImagesLogoPath))//Checks wheather the file exists in the path given in emailImagesLogoPath.
                Email_Images_btn.Image = Image.FromFile(emailImagesLogoPath);//Set the image of Email_Images_btn from emailImagesLogoPath. 
            if (File.Exists(uploadLogoPath))//Checks wheather the file exists in the path given in uploadLogoPath.
                uploadImagesTelemed_btn.Image = Image.FromFile(uploadLogoPath);//Set the image of uploadImagesTelemed_btn from uploadLogoPath. 
            if (File.Exists(emailReportsLogoPath))//Checks wheather the file exists in the path given in emailReportsLogoPath.
                EmailReport_btn.Image = Image.FromFile(emailReportsLogoPath);//Set the image of EmailReport_btn from emailReportsLogoPath. 
            if (File.Exists(autoAnalysisLogoPath))//Checks wheather the file exists in the path given in emailReportsLogoPath.
                autoAnalysis_btn.Image = Image.FromFile(autoAnalysisLogoPath);//Set the image of EmailReport_btn from emailReportsLogoPath. 

            if (File.Exists(appLogoFilePath))//Checks wheather the file exists in the path given in appLogoFilePath.
                this.Icon = new System.Drawing.Icon(appLogoFilePath, 256, 256);//Set the icon of report window from appLogoFilePath.
           
            
            
            #endregion

            #region reportdatamodelvariables
            _dataModel.ReportData = new Dictionary<string, object>(reportData);//Initializes the _dataModel.ReportData  and sets it to the reportData.
            if (reportData.ContainsKey("$DeviceID"))
                _dataModel.DeviceID = reportData["$DeviceID"] as string;
            if (reportData.ContainsKey("$visitImages"))//Checks wheather key $visitImages is present in reportData or not.
                _dataModel.VisitImageFiles = reportData["$visitImages"] as string[];//Sets the _dataModel.VisitImageFiles to the value of $visitImages key.
            if (reportData.ContainsKey("$visitImageIds"))//Checks wheather key $visitImageIds is present in reportData or not.
                _dataModel.VisitImageIds = reportData["$visitImageIds"] as int[];//Sets the _dataModel.VisitImageIds to the value of $visitImageIds key.
            if (reportData.ContainsKey("$visitImageSides"))//Checks wheather key $visitImageSides is present in reportData or not.
                _dataModel.VisitImagesides = reportData["$visitImageSides"] as int[];//Sets the _dataModel.VisitImagesides to the value of $visitImageSides key.
            if (reportData.ContainsKey("$ImageNames"))//Checks wheather key $ImageNames is present in reportData or not.
                _dataModel.CurrentImageNames = reportData["$ImageNames"] as string[];//Sets the _dataModel.CurrentImageNames to the value of $ImageNames key.
            if (reportData.ContainsKey("$imgannotated"))//Checks wheather key $imgannotated is present in reportData or not.
                _dataModel.isannotated = reportData["$imgannotated"] as bool[];//Sets the _dataModel.isannotated to the value of $imgannotated key.
            if (reportData.ContainsKey("$MaskSettings"))// checks if Key $MaskSettings is present in dictionary.By Ashutosh 23-08-2017.
                _dataModel.MaskSettingsArr = reportData["$MaskSettings"] as string[];//if present value associated with key given as string to MaskSettingsArr.By Ashutosh 23-08-2017.
            if (reportData.ContainsKey("$CameraSettings"))// checks if Key $CameraSettings is present in dictionary.By Ashutosh 31-08-2017.
                _dataModel.CameraSettingsArr = reportData["$CameraSettings"] as string[];//if present value associated with key given as string to CameraSettingsArr.By Ashutosh 31-08-2017.
            if (reportData.ContainsKey("$appDirPath"))
                _dataModel.appDir = reportData["$appDirPath"] as string;
            
            if (_dataModel.ReportData.ContainsKey("$isFromCDR"))// check if is from cdr key is present in the report dictionary
                _dataModel.isFromCDR = (bool)_dataModel.ReportData["$isFromCDR"];// assign is cdr report dic to local var iscdr
            if (_dataModel.ReportData.ContainsKey("$isAnnotation"))//check if is  Annotation key is present in the report dictionary
                _dataModel.isFromAnnotation = (bool)_dataModel.ReportData["$isAnnotation"]; //assign is annotation report dic to local var isAnnotation
            if (reportData.ContainsKey("$CurrentImageFiles"))//Checks wheather key $CurrentImageFiles is present in reportData or not.
            {
                _dataModel.CurrentImgFiles = reportData["$CurrentImageFiles"] as string[];//CurrentImgFiles hold rect.bmp . Sets the _dataModel.CurrentImgFiles to the value of $CurrentImageFiles key.Here path of selected images are given.By Ashutosh 23-08-2017.
                _dataModel.CurrentImageMaskSettings = new string[_dataModel.CurrentImgFiles.Length];//number of images selected(integer) given CurrentImageMaskSettings.By Ashutosh 23-08-2017.
                _dataModel.CurrentImageCameraSettings = new string[_dataModel.CurrentImgFiles.Length];//number of images selected(integer) given CurrentImageCameraSettings.By Ashutosh 31-08-2017.
                if (_dataModel.VisitImageFiles != null)
                {
                    visitImageFilesList = _dataModel.VisitImageFiles.ToList();//total images present in thumbnail list are given.By Ashutosh 23-08-2017.
                }
                if (_dataModel.isFromCDR || _dataModel.isFromAnnotation) // if the loading happens from annotation or from cdr
                {
                    _dataModel.CurrentImageCameraSettings[0] = _dataModel.CameraSettingsArr[0];//camera seetings properties of each image(CurrentImgFiles) is given to CurrentImageMaskSettings.By Ashutosh 31-08-2017.
                    _dataModel.CurrentImageMaskSettings[0] = _dataModel.MaskSettingsArr[0];//mask seetings properties(class MaskSettings) of each image(CurrentImgFiles) is given to CurrentImageMaskSettings.By Ashutosh 23-08-2017.

                }
                else
                {
                    if (_dataModel.CameraSettingsArr != null)
                        for (int i = 0; i < _dataModel.CurrentImgFiles.Length; i++)//checks for the images selected .By Ashutosh 23-08-2017.
                        {
                            _dataModel.CurrentImageCameraSettings[i] = _dataModel.CameraSettingsArr[visitImageFilesList.IndexOf(_dataModel.CurrentImgFiles[i])];////camera seetings properties of each image(CurrentImgFiles) is given to CurrentImageMaskSettings.By Ashutosh 31-08-2017.
                        }
                    if (_dataModel.MaskSettingsArr != null)
                        for (int i = 0; i < _dataModel.CurrentImgFiles.Length; i++)
                        {
                            _dataModel.CurrentImageMaskSettings[i] = _dataModel.MaskSettingsArr[visitImageFilesList.IndexOf(_dataModel.CurrentImgFiles[i])];//mask seetings properties(class MaskSettings) of each image(CurrentImgFiles) is given to CurrentImageMaskSettings.By Ashutosh 23-08-2017.
                        }
                }
            }
            if (reportData.ContainsKey("$isCDR"))//Checks wheather key $isCDR is present in reportData or not.
                _dataModel.isCDR = reportData["$isCDR"] as bool[];//Sets the _dataModel.isCDR to the value of $isCDR key.
            if (reportData.ContainsKey("$NoOfImagesAllowed"))//Checks wheather key $NoOfImagesAllowed is present in reportData or not.
                _dataModel.NoOfImagesAllowed = Convert.ToInt32(reportData["$NoOfImagesAllowed"]);//Sets the _dataModel.NoOfImagesAllowed to the value of $NoOfImagesAllowed key.
            if (reportData.ContainsKey("$NoOfImagesAllowedText1"))//Checks wheather key $NoOfImagesAllowedText1 is present in reportData or not.
                _dataModel.NoOfImagesAllowedText1 = (reportData["$NoOfImagesAllowedText1"]) as string;//Sets the _dataModel.NoOfImagesAllowedText1 to the value of $NoOfImagesAllowedText1 key.
            if (reportData.ContainsKey("$NoOfImagesAllowedText2"))//Checks wheather key $NoOfImagesAllowedText2 is present in reportData or not.
                _dataModel.NoOfImagesAllowedText2 = (reportData["$NoOfImagesAllowedText2"]) as string;//Sets the _dataModel.NoOfImagesAllowedText2 to the value of $NoOfImagesAllowedText2 key.
            if (reportData.ContainsKey("$NoOfImagesAllowedHeader"))//Checks wheather key $NoOfImagesAllowedHeader is present in reportData or not.
                _dataModel.NoOfImagesAllowedHeader = (reportData["$NoOfImagesAllowedHeader"]) as string;//Sets the _dataModel.NoOfImagesAllowedHeader to the value of $NoOfImagesAllowedHeader key.
            if (reportData.ContainsKey("$CmdArgsPresent"))//Checks wheather key $CmdArgsPresent is present in reportData or not.
                _dataModel.ContainsCmdArgs = (bool)reportData["$CmdArgsPresent"];//Sets the _dataModel.ContainsCmdArgs to the value of $CmdArgsPresent key.
            if (reportData.ContainsKey("$EmailData"))//Checks wheather key $EmailData is present in reportData or not.
                _dataModel.mailData = reportData["$EmailData"] as EmailsData;//Sets the _dataModel.mailData to the value of $EmailData key.
            if (reportData.ContainsKey("$visitDateTime"))//Checks wheather key $visitDateTime is present in reportData or not.
                _dataModel.visitDateTime = (DateTime)reportData["$visitDateTime"];//Sets the _dataModel.visitDateTime to the value of $visitDateTime key.
            if (reportData.ContainsKey("$showEmailDialog"))//Checks wheather key $showEmailDialog is present in reportData or not.
                _dataModel.ShowEmailDialog = (bool)reportData["$showEmailDialog"];//Sets the _dataModel.ShowEmailDialog to the value of $showEmailDialog key.
            if (reportData.ContainsKey("$Is2ImagesLS4ImagesPOR"))//Checks wheather key $Is2ImagesLS4ImagesPOR is present in reportData or not.
                _dataModel.Is2ImagesLS4ImagesPOR = (bool)reportData["$Is2ImagesLS4ImagesPOR"];//Sets the _dataModel.Is2ImagesLS4ImagesPOR to the value of $Is2ImagesLS4ImagesPOR key.
            if (reportData.ContainsKey("$Datetime") && _dataModel.ReportData.ContainsKey("$Datetime"))
                _dataModel.ReportData["$Datetime"] = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            else
            {
                _dataModel.ReportData.Add("$Datetime", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            }
            #endregion

            #region variablerelatedtotemplates
            if (reportData.ContainsKey("$allTemplateFiles"))//Checks wheather key $allTemplateFiles is present in reportData or not.
                reportTemplates = reportData["$allTemplateFiles"] as List<FileInfo>;//Sets the reportTemplates to the value of $allTemplateFiles key.
            if (reportData.ContainsKey("$currentTemplate"))//Checks wheather key $currentTemplate is present in reportData or not.
                _dataModel.CurrentTemplate = reportData["$currentTemplate"] as string;//Sets the dataModel.CurrentTemplate to the value of $currentTemplate key.
            #endregion

            #region variables initialization
            isNew = true;
            if (reportData.ContainsKey("$adobereadernotpresent"))//Checks wheather key $adobereadernotpresent is present in reportData or not.
                adobereader_text = reportData["$adobereadernotpresent"] as string;//Sets the adobereader_text to the value of the key $adobereadernotpresent.
            if (reportData.ContainsKey("$NoImages"))//Checks wheather key $NoImages is present in reportData or not.
                noImageLabel = reportData["$NoImages"] as string;//Sets the noImageLabel to the value of the key $NoImages.
            //This code has been added to get the message and header text for report when it is getting closed.
            if (reportData.ContainsKey("$reportMessageBoxText"))//Checks wheather key $reportMessageBoxText is present in reportData or not.
                Reportsave_message = reportData["$reportMessageBoxText"] as string;//Sets the Reportsave_message to the value of the key $reportMessageBoxText.
            if (reportData.ContainsKey("$imageDeletedMessageBoxText"))//Checks wheather key $imageDeletedMessageBoxText is present in reportData or not.By Ashutosh 05-09-2018.
                Imagedeleted_message = reportData["$imageDeletedMessageBoxText"] as string;
            if (reportData.ContainsKey("$errorMessageTitle"))//Checks wheather key $errorMessageTitle is present in reportData or not.By Ashutosh 05-09-2018.
                ErrorMessage_Title = reportData["$errorMessageTitle"] as string;
            
            if (reportData.ContainsKey("$reportMessageBoxHeader"))//Checks wheather key $reportMessageBoxHeader is present in reportData or not.
                Reportsave_header = reportData["$reportMessageBoxHeader"] as string;//Sets the Reportsave_header to the value of the key $reportMessageBoxHeader.
            if (reportData.ContainsKey("$reportNewStatus"))//Checks wheather key $reportMessageBoxText is present in reportData or not.
                ReportStatusNew = reportData["$reportNewStatus"] as string;//Sets the Reportsave_message to the value of the key $reportMessageBoxText.
            if (reportData.ContainsKey("$reportExistingStatus"))//Checks wheather key $reportMessageBoxHeader is present in reportData or not.
                ReportStatusSaved = reportData["$reportExistingStatus"] as string;//Sets the Reportsave_header to the value of the key $reportMessageBoxHeader.
            if (reportData.ContainsKey("$isTelemedReport"))//Checks wheather key $isTelemedReport is present in reportData or not.
               _dataModel.isDRReport = isDRReport = Convert.ToBoolean( reportData["$isTelemedReport"]);//Sets the Reportsave_header to the value of the key $reportMessageBoxHeader.
            #endregion

            #region report module controls visibility
            if (reportData.ContainsKey("$showPrintButton"))//Checks wheather key $showPrintButton is present in reportData or not.
                print_btn.Visible = Convert.ToBoolean(reportData["$showPrintButton"]);//Sets the visibility of the print_btn to the value of $showPrintButton key.
            if (reportData.ContainsKey("$showSaveButton"))//Checks wheather key $showSaveButton is present in reportData or not.
                save_btn.Visible = Convert.ToBoolean(reportData["$showSaveButton"]);//Sets the visibility of the save_btn to the value of $showSaveButton key.
            if (reportData.ContainsKey("$showExportButton"))//Checks wheather key $showExportButton is present in reportData or not.
                export_btn.Visible = Convert.ToBoolean(reportData["$showExportButton"]);//Sets the visibility of the export_btn to the value of $showExportButton key.
            if (reportData.ContainsKey("$showEmailImagesButton"))//Checks wheather key $showEmailImagesButton is present in reportData or not.
                Email_Images_btn.Visible = Convert.ToBoolean(reportData["$showEmailImagesButton"]);//Sets the visibility of the Email_Images_btn to the value of $showEmailImagesButton key.
            if (reportData.ContainsKey("$showEmailReportButton"))//Checks wheather key $showEmailReportButton is present in reportData or not.
                EmailReport_btn.Visible = Convert.ToBoolean(reportData["$showEmailReportButton"]);//Sets the visibility of the EmailReport_btn to the value of $showEmailReportButton key.
            if (reportData.ContainsKey("$showEmailTelemedButton"))//Checks wheather key $showEmailTelemedButton is present in reportData or not.
                uploadImagesTelemed_btn.Visible = Convert.ToBoolean(reportData["$showEmailTelemedButton"]);//Sets the visibility of the uploadImagesTelemed_btn to the value of $showEmailTelemedButton key.
            if (reportData.ContainsKey("$showAutoAnalysisButton"))//Checks wheather key $showEmailTelemedButton is present in reportData or not.
                autoAnalysis_btn.Visible = Convert.ToBoolean(reportData["$showAutoAnalysisButton"]);//Sets the visibility of the uploadImagesTelemed_btn to the value of $showEmailTelemedButton key.

            //if (_dataModel.ContainsCmdArgs || !isDRReport)
            //{
            //    if (reportData.ContainsKey("$showGenObs"))//Checks wheather key $showGenObs is present in reportData or not.
            //    {
            //        genObs_lbl.Visible = Convert.ToBoolean(reportData["$showGenObs"]);//Sets the visibility of the genObs_lbl to the value of $showGenObs key.
            //        genObs_tbx.Visible = Convert.ToBoolean(reportData["$showGenObs"]);//Sets the visibility of the genObs_tbx to the value of $showGenObs key.
            //    }
            //    if (reportData.ContainsKey("$showRightObs"))//Checks wheather key $showRightObs is present in reportData or not.
            //    {
            //        rightEyeObs_lbl.Visible = Convert.ToBoolean(reportData["$showRightObs"]);//Sets the visibility of the rightEyeObs_lbl to the value of $showLeftObs key.
            //        rightEyeObs_tbx.Visible = Convert.ToBoolean(reportData["$showRightObs"]);//Sets the visibility of the rightEyeObs_tbx to the value of $showLeftObs key.
            //    }
            //    if (reportData.ContainsKey("$showLeftObs"))//Checks wheather key $showLeftObs is present in reportData or not.
            //    {
            //        leftEyeObs_lbl.Visible = Convert.ToBoolean(reportData["$showLeftObs"]);//Sets the visibility of the leftEyeObs_lbl to the value of $showLeftObs key.
            //        leftEyeObs_tbx.Visible = Convert.ToBoolean(reportData["$showLeftObs"]);//Sets the visibility of the leftEyeObs_tbx to the value of $showLeftObs key.
            //    }
            //}
            
            if (_dataModel.ContainsCmdArgs)
            {
                tableLayoutPanel2.Enabled = false;
                reportSize_cbx.Enabled = false;
                Email_Images_btn.Visible = false;//Sets the visibility of the Email_Images_btn to the false when report opened from command line.
                uploadImagesTelemed_btn.Visible = false;//Sets the visibility of the uploadImagesTelemed_btn to the false when report opened from command line.
                //comments_tbx.ReadOnly = true;//Sets the ReadOnly property of the comments_tbx to the true when report opened from command line.
            }
            #endregion

 

            #region ReportImages static variables
            if (reportData.ContainsKey("$reportImagesText"))//Checks wheather key $reportImagesText is present in reportData or not.
                ReportImages.reportImageText = reportData["$reportImagesText"] as string;//Sets the ReportImages.reportImageText to the value of the key $reportImagesText.
            if (reportData.ContainsKey("$reportImagesSizeText"))//Checks wheather key $reportImagesSizeText is present in reportData or not.
                ReportImages.reportImageSizeText = reportData["$reportImagesSizeText"] as string;//Sets the ReportImages.reportImageSizeText to the value of the key $reportImagesText.
            if (reportData.ContainsKey("$reportImagesOkBtnText"))//Checks wheather key $reportImagesOkBtnText is present in reportData or not.
                ReportImages.reportImageOkbtnText = reportData["$reportImagesOkBtnText"] as string;//Sets the ReportImages.reportImageOkbtnText to the value of the key $reportImagesOkBtnText.
            if (reportData.ContainsKey("$reportImagesWarningMsgText"))//Checks wheather key $reportImagesWarningMsgText is present in reportData or not.
                ReportImages.reportImageMsgText = reportData["$reportImagesWarningMsgText"] as string;//Sets the ReportImages.reportImageMsgText to the value of the key $reportImagesWarningMsgText.
            #endregion

            #region textsforcontrols and initialization of size combo box
            if (reportData.ContainsKey("$portraitText"))//Checks wheather key $portraitText is present in reportData or not.
                portrait_rb.Text = reportData["$portraitText"] as string;//Sets the portrait_rb text to the value of the key $portraitText.
            if (reportData.ContainsKey("$landscapeText"))//Checks wheather key $landscapeText is present in reportData or not.
                landscape_rb.Text = reportData["$landscapeText"] as string;//Sets the landscape_rb text to the value of the key $landscapeText.
            if (reportData.ContainsKey("$reportText"))//Checks wheather key $reportText is present in reportData or not.
                this.Text = reportData["$reportText"] as string;//Sets the report window text to the value of the key $reportText.
            if (reportData.ContainsKey("$reportSaveText"))//Checks wheather key $reportSaveText is present in reportData or not.
                save_btn.Text = reportData["$reportSaveText"] as string;//Sets the save_btn text to the value of the key $reportSaveText.
            if (reportData.ContainsKey("$reportPrintText"))//Checks wheather key $reportPrintText is present in reportData or not.
                print_btn.Text = reportData["$reportPrintText"] as string;//Sets the print_btn text to the value of the key $reportPrintText.
            //if (reportData.ContainsKey("$GeneralObservationText"))//Checks wheather key $GeneralObservationText is present in reportData or not.
            //    genObs_lbl.Text = reportData["$GeneralObservationText"] as string;//Sets the genObs_lbl text to the value of the key $GeneralObservationText.
            //if (reportData.ContainsKey("$LeftEyeObservationText"))//Checks wheather key $LeftEyeObservationText is present in reportData or not.
            //    leftEyeObs_lbl.Text = reportData["$LeftEyeObservationText"] as string;//Sets the leftEyeObs_lbl text to the value of the key $LeftEyeObservationText.
            //if (reportData.ContainsKey("$RightEyeObservationText"))//Checks wheather key $RightEyeObservationText is present in reportData or not.
            //    rightEyeObs_lbl.Text = reportData["$RightEyeObservationText"] as string;//Sets the rightEyeObs_lbl text to the value of the key $RightEyeObservationText.
            if (reportData.ContainsKey("$showExportButtonText"))//Checks wheather key $showExportButton is present in reportData or not.
                export_btn.Text = reportData["$showExportButtonText"] as string;//Sets the visibility of the export_btn to the value of $showExportButton key.
            if (reportData.ContainsKey("$showEmailImagesButtonText"))//Checks wheather key $showEmailImagesButton is present in reportData or not.
                Email_Images_btn.Text = reportData["$showEmailImagesButtonText"] as string;//Sets the visibility of the Email_Images_btn to the value of $showEmailImagesButton key.
            if (reportData.ContainsKey("$showEmailReportButtonText"))//Checks wheather key $showEmailReportButton is present in reportData or not.
                EmailReport_btn.Text = reportData["$showEmailReportButtonText"] as string;//Sets the visibility of the EmailReport_btn to the value of $showEmailReportButton key.
            if (reportData.ContainsKey("$showEmailTelemedButtonText"))//Checks wheather key $showEmailTelemedButton is present in reportData or not.
                uploadImagesTelemed_btn.Text = reportData["$showEmailTelemedButtonText"] as string;//Sets the visibility of the uploadImagesTelemed_btn to the value of $showEmailTelemedButton key.
            if (reportData.ContainsKey("$size_Lbl_Text"))//Checks wheather key $showEmailTelemedButton is present in reportData or not.
                ReportSize_lbl.Text = reportData["$size_Lbl_Text"] as string;//Sets the visibility of the uploadImagesTelemed_btn to the value of $showEmailTelemedButton key.
            if (reportData.ContainsKey("$orientation_Lbl_Text"))//Checks wheather key $showEmailTelemedButton is present in reportData or not.
                Orientation_lbl.Text = reportData["$orientation_Lbl_Text"] as string;//Sets the visibility of the uploadImagesTelemed_btn to the value of $showEmailTelemedButton key.
            if (reportData.ContainsKey("$file_Gbx_Text"))//Checks wheather key $showEmailTelemedButton is present in reportData or not.
                file_Gbx.Text = reportData["$file_Gbx_Text"] as string;//Sets the visibility of the uploadImagesTelemed_btn to the value of $showEmailTelemedButton key.
            if (reportData.ContainsKey("$email_Gbx_Text"))//Checks wheather key $showEmailTelemedButton is present in reportData or not.
                email_Gbx.Text = reportData["$email_Gbx_Text"] as string;//Sets the visibility of the uploadImagesTelemed_btn to the value of $showEmailTelemedButton key.
            #endregion


            List<Control> controls = GetControls(this).ToList();
            foreach (Control c in controls)
            {
                if (c is System.Windows.Forms.TextBox || c is RichTextBox || c is StatusStrip || c is ComboBox)
                    c.ForeColor = Color.Black;
            }
            this.toolStrip1.Renderer = new FormToolStripRenderer();
            this.toolStrip2.Renderer = new FormToolStripRenderer();
            this.toolStrip3.Renderer = new FormToolStripRenderer();

           

        }

        void emailWindow__WaitCursor(EventArgs e, bool isWait)
        {
            if (isWait)
            {
                if (autoAnalysis_btn.Visible == true)
                    autoAnalysis_btn.Enabled = true;
                this.Cursor = Cursors.Default;
            }
            else
            {
                if (autoAnalysis_btn.Visible == true)
                    autoAnalysis_btn.Enabled = false;
                this.Cursor = Cursors.WaitCursor;
            }
        }

        #region private events
        void emailWindow__EmailSent(string isSent, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            if (!_dataModel.ContainsCmdArgs)
                saveReport(e);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TaskBar.SetTaskbarState(TaskBar.AppBarStates.AutoHide);
            if (isNew)
            {
                this.reportSize_cbx.SelectedIndexChanged -= new EventHandler(reportSize_cbx_SelectedIndexChanged);
                reportSize_cbx.DataSource = pageSize;
                this.reportSize_cbx.SelectedIndexChanged += new EventHandler(reportSize_cbx_SelectedIndexChanged);
                _dataModel.patientDetails.Age = _dataModel.ReportData["$Age"] as string;
                _dataModel.patientDetails.Gender = _dataModel.ReportData["$Gender"] as string;
                _dataModel.patientDetails.MRN = _dataModel.ReportData["$MRN"] as string;
                _dataModel.patientDetails.HospitalName = _dataModel.ReportData["$HospitalName"] as string;
                _dataModel.patientDetails.Address1 = _dataModel.ReportData["$Address1"] as string;
                _dataModel.patientDetails.Address2 = _dataModel.ReportData["$Address2"] as string;
                {
                    string[] nameArr = (_dataModel.ReportData["$Name"] as string).Split(new char[0]);
                    _dataModel.patientDetails.FirstName = nameArr[0];
                    _dataModel.patientDetails.LastName = nameArr[1];
                }
                _dataModel.patientDetails.observationPaths = _dataModel.CurrentImgFiles.ToList<string>();
                _dataModel.patientDetails.isAnnotatedList = _dataModel.isannotated.ToList<bool>();
                _dataModel.patientDetails.isCDRList = _dataModel.isCDR.ToList<bool>();
                _dataModel.patientDetails.ImageNames = _dataModel.CurrentImageNames.ToList<string>();
                _dataModel.patientDetails.ImageSideList = _dataModel.VisitImagesides.ToList<int>();
                if (_dataModel.CurrentImgFiles.Length == 0)
                {
                    NoImagelabelvisible();
                }
                LayoutDetails.Current.Orientation = (LayoutDetails.PageOrientation)Enum.Parse(typeof(LayoutDetails.PageOrientation), _dataModel.ReportData["$CurrentTemplateName"].ToString().ToUpper() + "_" + _dataModel.ReportData["$CurrentTemplateSize"].ToString());
                SetCurrentOrientationAndSize();
                ChangeTemplate();
                IntitializeSizeAndOrientaionControls(reportSize);
                //setToolsStripLabels();
            }
            reportDefaultCursor();
            this.reportCanvas_pnl.Dock = DockStyle.Fill;
            //this.reportCanvas_pnl.Controls.Clear();
            this.reportCanvas_pnl.BackColor = Color.White;
            this.reportCanvas_pnl.Refresh();
            //parseXmlData(_dataModel.CurrentTemplate);
            //foreach (ReportControlsStructure item in reportControlStructureList)
            //{
            //    populateControls(item.reportControlProperty);
            //}
            //writeValuesToTheBindingType();
            this.Cursor = Cursors.Default;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //This code has been added by darshan solved defect no 0000744: Reopen:No confirmation message when the report is closed without saving comments.
            if ((isNew || isTextChanged) && !_dataModel.ContainsCmdArgs)
            {
                DialogResult res = CustomMessageBox.Show(Reportsave_message, Reportsave_header, CustomMessageBoxButtons.YesNo, CustomMessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    saveReport(new EventArgs());
                }
               
                TaskBar.SetTaskbarState(TaskBar.AppBarStates.AlwaysOnTop);
            }
            reportClosedEvent();
            CustomFolderBrowser.ImageSavingbtn -= customFolderBrowser_ImageSavingbtn;
            isTextChanged = false;
        }

        void textBox_KeyPress(object sender, KeyPressEventArgs e)//This was added by darshan to prevent user from entering the numbers in operator field.
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == AsciiValueOfControlBackspace)
                e.Handled = true;
        }

        void textBox_KeyPress1(object sender, KeyPressEventArgs e)//This was added by darshan to prevent user from entering the numbers in operator field.
        {
            if (e.KeyChar == AsciiValueOfControlBackspace)
                e.Handled = true;
        }

        void textBox_TextChanged(object sender, EventArgs e)
        {
            isTextChanged = true;
        }

        void imageTable_pbclick(object sender, EventArgs e)
        {
            if (isNew && !(_dataModel.ContainsCmdArgs))
            {
                this.Cursor = Cursors.WaitCursor;
                _reportImagesForm.ShowDialog();
                this.Cursor = Cursors.Default;
                if (_reportImagesForm.isOk)
                {
                    Dictionary<string, List<string>> file_imagenames = _reportImagesForm.GetFileNames();
                    string[] files = file_imagenames["FileNames"].ToArray();
                    _dataModel.CurrentImageNames = file_imagenames["ImageNames"].ToArray();
                    #region Images not getting selected in the order as the user selecting
                    //Array.Reverse(_dataModel.CurrentImageNames);
                    //Array.Reverse(files);
                    #endregion
                    UpdateImages(files);
                    isNew = true;
                }
                else
                    return;
            }
        }

        private void print_btn_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            CreatePdf();// string(required file name) passed to CreatePdf . By Ashutosh 21-7-2017
            isPrintingOver = PrintReport.PrintPDFs(CustomFolderBrowser.filePath+ CustomFolderBrowser.fileName, adobereader_text);
            if (isPrintingOver && !_dataModel.ContainsCmdArgs) //this if has been added to save the report when it is printed.
                saveReport(new EventArgs());
            this.Cursor = Cursors.Default;
        }

        private void portrait_rb_CheckedChanged_1(object sender, EventArgs e)
        {
            //if (isNew)
            //{
            //    if (portrait_rb.Checked)
            //        reportTemplateOrientation = (ReportTemplateOrientationEnum)Enum.Parse(typeof(ReportTemplateOrientationEnum), ReportTemplateOrientationEnum.Portrait.ToString());
            //    else
            //        reportTemplateOrientation = (ReportTemplateOrientationEnum)Enum.Parse(typeof(ReportTemplateOrientationEnum), ReportTemplateOrientationEnum.Landscape.ToString());
            //    ChangeTemplate();
            //}
            if (isNew)
            {
                if (portrait_rb.Checked)
                    reportTemplateOrientation = ReportTemplateOrientationEnum.Portrait;//(ReportTemplateOrientationEnum)Enum.Parse(typeof(ReportTemplateOrientationEnum), ReportTemplateOrientationEnum.Portrait.ToString());
                else
                    reportTemplateOrientation = ReportTemplateOrientationEnum.Landscape;//(ReportTemplateOrientationEnum)Enum.Parse(typeof(ReportTemplateOrientationEnum), ReportTemplateOrientationEnum.Landscape.ToString());
                ChangeTemplate();
            }
        }

        private void save_btn_Click_1(object sender, EventArgs e)
        {
            if (isNew || isTextChanged)//This if staement has been added to save only a single instance of report.
            {
                saveReport(e);
            }
        }

        void customFolderBrowser_ImageSavingbtn()
        {
            CreatePdf();
            string[] splitName = (_dataModel.ReportData["$Name"] as string).Split(new char[0]);//to split the name 
            string dirPath = customFolderBrowser.folderPath + Path.DirectorySeparatorChar + _dataModel.ReportData["$MRN"] + "_" + splitName[0] + "_" + splitName[1] + "_" + _dataModel.ReportData["$Age"] + "_" + _dataModel.ReportData["$Gender"];
            if (!Directory.Exists(dirPath))//check whether the directory is not available 
                Directory.CreateDirectory(dirPath);//creates new directory
            File.Copy(CustomFolderBrowser.filePath + CustomFolderBrowser.fileName, dirPath + Path.DirectorySeparatorChar + ReportFileName);
            CustomFolderBrowser.fileNames = new string[] { dirPath + Path.DirectorySeparatorChar + ReportFileName };//CustomFolderBrowser.fileNames handles multiple files. dirpath is a string , whereas fileNames is string array. By Ashutosh 21-7-2017
            //CustomFolderBrowser.fileName = dirPath + Path.DirectorySeparatorChar + CustomFolderBrowser.fileName;
            string reportExportTextStr = string.Empty;
            string reportExportHeaderStr = string.Empty;
            if (_dataModel.ReportData.ContainsKey("$exportReportText"))
                reportExportTextStr = _dataModel.ReportData["$exportReportText"] as string;
            if (_dataModel.ReportData.ContainsKey("$exportReportHeader"))
                reportExportHeaderStr = _dataModel.ReportData["$exportReportHeader"] as string;

            DialogResult exported = CustomMessageBox.Show(reportExportTextStr, reportExportHeaderStr, CustomMessageBoxIcon.Information);//messagebox to show that exporting is completed
            if (exported == DialogResult.OK)
            {
                customFolderBrowser.Close();
            }
        }

        void customFolderBrowser_CancelButtonClickedEvent()
        {
            CustomFolderBrowser.ImageSavingbtn -= customFolderBrowser_ImageSavingbtn;
            CustomFolderBrowser.CancelButtonClickedEvent -= customFolderBrowser_CancelButtonClickedEvent;
            customFolderBrowser.isReportExport = false;
        }

        private void EmailImages_btn_Click(object sender, EventArgs e)
        {
            //emailWindow.ImageFileNames = null;// in order to reset the imageFileNames
            //emailWindow.ReportFileName = "";
            //emailWindow.ImageFileNames = _dataModel.CurrentImgFiles;
            //emailWindow.ShowDialog();
            emailWindow.vendorVal = "Vendor2";
            ThreadPool.QueueUserWorkItem(new WaitCallback(emailWindow.Upload2Cloud));
        }

        private void EmailReport_btn_Click(object sender, EventArgs e)
        {
            CreatePdf();//string(required file name),passed to CreatePdf.By Ashutosh 21-7-2017
            emailWindow.ImageFileNames = null;// in order to reset the imageFileNames
            emailWindow.ReportFileName = "";
            emailWindow.ReportFileName = CustomFolderBrowser.filePath + CustomFolderBrowser.fileName;
            if (!_dataModel.ContainsCmdArgs && _dataModel.ShowEmailDialog)
            {

                emailWindow.ShowDialog();
            }
            else
            {
                if (string.IsNullOrEmpty(_dataModel.mailData.EmailBCC))
                    _dataModel.mailData.EmailBCC = emailWindow.BccEmail;
                else
                    _dataModel.mailData.EmailBCC += emailWindow.BccEmail;


                emailWindow.SendEmail(_dataModel.mailData);
            }
        }

      
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            emailWindow.ImageFileNames = null;// in order to reset the imageFileNames
            emailWindow.ReportFileName = "";
            emailWindow.ImageFileNames = _dataModel.CurrentImgFiles;
            _dataModel.mailData.Subject = "New Patient Referral " + _dataModel.ReportData["$HospitalName"] + " Referral on : " + DateTime.Now.ToString("dd MMM yy hh:mm tt ");
            _dataModel.mailData.Body = "Patient Details \n" + Environment.NewLine +
                    "Name :" + _dataModel.patientDetails.FirstName + " " + _dataModel.patientDetails.LastName + Environment.NewLine +
                    "MRN :" + _dataModel.patientDetails.MRN + Environment.NewLine +
                     "Age :" + _dataModel.patientDetails.Age.ToString() + Environment.NewLine +
                     "Gender :" + _dataModel.patientDetails.Gender + Environment.NewLine + Environment.NewLine;
            emailWindow.SendEmail(_dataModel.mailData);
        }

        private void leftEyeObs_tbx_TextChanged(object sender, EventArgs e)
        {
            //setActualValuesToBindingValues(leftEyeObs_tbx.Tag.ToString().Replace("$", ""), leftEyeObs_tbx.Text);
            //if (_dataModel.ReportData.ContainsKey(leftEyeObs_tbx.Tag.ToString()))//This code has been added to change the reportdata when data for that binding type is changed in report window.
            //    _dataModel.ReportData[leftEyeObs_tbx.Tag.ToString()] = leftEyeObs_tbx.Text;
        }

        private void genObs_tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == AsciiValueOfControlBackspace)
            //    e.Handled = true;
            //else if (genObs_tbx.Lines.Length >= MAX_LINES && e.KeyChar == newlineChar)
            //{
            //    e.Handled = true;
            //}
        }

        private void rightEyeObs_tbx_TextChanged(object sender, EventArgs e)
        {
            //setActualValuesToBindingValues(rightEyeObs_tbx.Tag.ToString().Replace("$", ""), rightEyeObs_tbx.Text);
            //if (_dataModel.ReportData.ContainsKey(rightEyeObs_tbx.Tag.ToString()))//This code has been added to change the reportdata when data for that binding type is changed in report window.
            //    _dataModel.ReportData[rightEyeObs_tbx.Tag.ToString()] = rightEyeObs_tbx.Text;
        }

        private void genObs_tbx_TextChanged(object sender, EventArgs e)
        {
            //setActualValuesToBindingValues(genObs_tbx.Tag.ToString().Replace("$", ""), genObs_tbx.Text);
            //if (_dataModel.ReportData.ContainsKey(genObs_tbx.Tag.ToString()))//This code has been added to change the reportdata when data for that binding type is changed in report window.
            //    _dataModel.ReportData[genObs_tbx.Tag.ToString()] = genObs_tbx.Text;
        }

        private void leftEyeObs_tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == AsciiValueOfControlBackspace)
            //    e.Handled = true;
            //else if (leftEyeObs_tbx.Lines.Length >= MAX_LINES && e.KeyChar == newlineChar)
            //{
            //    e.Handled = true;
            //}
        }

        private void rightEyeObs_tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == AsciiValueOfControlBackspace)
            //    e.Handled = true;
            //else if (rightEyeObs_tbx.Lines.Length >= MAX_LINES && e.KeyChar == newlineChar)
            //{
            //    e.Handled = true;
            //}
        }

        private void reportSize_cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isNew)
            {
                reportTemplateSize = (ReportTemplateSizeEnum)Enum.Parse(typeof(ReportTemplateSizeEnum), reportSize_cbx.Text);
                ChangeTemplate();
            }
        }

        private void comments_tbx_TextChanged(object sender, EventArgs e)
        {
            //setActualValuesToBindingValues(comments_tbx.Tag.ToString().Replace("$", ""), comments_tbx.Text);
            //_dataModel.patientDetails.MedHistory = comments_tbx.Text;
            //if (_dataModel.ReportData.ContainsKey(comments_tbx.Tag.ToString()))
            //    _dataModel.ReportData[comments_tbx.Tag.ToString()] = comments_tbx.Text;

        }

        private void doctor_tbx_TextChanged(object sender, EventArgs e)
        {
            //setActualValuesToBindingValues(doctor_tbx.Tag.ToString().Replace("$", ""), doctor_tbx.Text);
            //_dataModel._operator = doctor_tbx.Text;
            //if (_dataModel.ReportData.ContainsKey(doctor_tbx.Tag.ToString()))
            //    _dataModel.ReportData[doctor_tbx.Tag.ToString()] = doctor_tbx.Text;
        }

        private void export_btn_Click(object sender, EventArgs e)
        {
           
            this.Cursor = Cursors.WaitCursor;
            customFolderBrowser = new CustomFolderBrowser();
            CustomFolderBrowser.fileName = "report_" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss") + ".pdf";// to assingn the file name 
            customFolderBrowser.isReportExport = true;
            customFolderBrowser.CustomFolderData = _dataModel.ReportData["$customFolderData"] as Dictionary<string, object>;
            customFolderBrowser.ShowImageExportButtons(); //to enable the visibility of labels and textboxes in custom folder browser
            CustomFolderBrowser.ImageSavingbtn += customFolderBrowser_ImageSavingbtn;
            CustomFolderBrowser.CancelButtonClickedEvent += customFolderBrowser_CancelButtonClickedEvent;
            customFolderBrowser.ShowDialog();
            this.Cursor = Cursors.Default;
        }
        #endregion

        #region public methods

        /// <summary>
        /// Displays the label no image is selected.
        /// </summary>
        public void NoImagelabelvisible()
        {
            noImages_lbl.Font = new Font(FontFamily.GenericSansSerif, 25f, FontStyle.Bold, GraphicsUnit.Pixel);
            reportCanvas_pnl.Controls.Add(noImages_lbl);
            noImages_lbl.Name = "NoImage_label";
            noImages_lbl.Text = noImageLabel;
            noImages_lbl.Location = new Point(300, 350);
            noImages_lbl.Width = 400;
            noImages_lbl.Height = 85;
            noImages_lbl.Visible = true;
            noImages_lbl.BringToFront();
            reportCanvas_pnl.Refresh();
        }

        /// <summary>
        /// Update the image table present in the report window when images are selected from report images window.
        /// </summary>
        /// <param name="ImageFiles">image files selected in reportimage window</param>
        public bool UpdateImages(string[] ImageFiles)//return type chnaged from void to bool . By Ashutosh 05-09-2018.
        {
             LayoutDetails.Current.ReportImgCnt = ImageFiles.Length;
            _dataModel.CurrentImgFiles = ImageFiles;//images file names selected by user provied to CurrentImgFiles.By Ashutosh 23-08-2017.
            _dataModel.patientDetails.observationPaths = ImageFiles.ToList();
            _dataModel.CurrentImageMaskSettings = new string[_dataModel.CurrentImgFiles.Length];//CurrentImageMaskSettings  is a string array , instance of _dataModel.CurrentImgFiles.Length provided to it.By Ashutosh 23-08-2017
            _dataModel.CurrentImageCameraSettings = new string[_dataModel.CurrentImgFiles.Length];//CurrentImageCameraSettings  is a string array , instance of _dataModel.CurrentImgFiles.Length provided to it.By Ashutosh 31-08-2017
            List<string> visitImageFilesList = _dataModel.VisitImageFiles.ToList();//visitImageFilesList list created and VisitImageFiles string array passed to the list. By Ashutosh 23-08-2017.
            for (int i = 0; i < _dataModel.CurrentImgFiles.Length; i++)
            {
                if (_dataModel.VisitImageFiles.Contains(_dataModel.CurrentImgFiles[i]))//checks if VisitImageFiles contains CurrentImgFiles.By Ashutosh 05-09-2017.
                {
                    if (_dataModel.MaskSettingsArr != null )
                    _dataModel.CurrentImageMaskSettings[i] = _dataModel.MaskSettingsArr[visitImageFilesList.IndexOf(_dataModel.CurrentImgFiles[i])];//selected image file names given to CurrentImageMaskSettings.By Ashutosh 23-08-2017.
                    if (_dataModel.CameraSettingsArr != null )
                    _dataModel.CurrentImageCameraSettings[i] = _dataModel.CameraSettingsArr[visitImageFilesList.IndexOf(_dataModel.CurrentImgFiles[i])];//selected image file names given to CurrentImageCameraSettings.By Ashutosh 31-08-2017.
                }
                else
                {
                    CustomMessageBox.Show(Imagedeleted_message,ErrorMessage_Title, CustomMessageBoxButtons.OK,CustomMessageBoxIcon.Error);//if VisitImageFiles does not contain CurrentImgFiles, then error message is displayed.By Ashutosh 05-09-2017.
                    return false;//By Ashutosh 05-09-2018.
                }
            }
            if (ImageFiles.Length == 0)
            {
                NoImagelabelvisible();
            }
            else
            {
                noImages_lbl.Visible = false;
            }
            addImgBox();
            return true;//By Ashutosh 05-09-2018.
        }

        /// <summary>
        /// Sets the image panel row and column and images.
        /// </summary>
        private void addImgBox()
        {
            switch (LayoutDetails.Current.Orientation)
            {
                case LayoutDetails.PageOrientation.PORTRAIT_A4:
                case LayoutDetails.PageOrientation.PORTRAIT_A5:
                    IVL_ImagePanel.isPortrait = true;
                    break;
                default: IVL_ImagePanel.isPortrait = false;
                    break;
            }
           


            foreach (var item in p.Controls)
            {
                if (item is IVL_ImagePanel)
                {
                    IVL_ImagePanel imgPanel = item as IVL_ImagePanel;
                    SegregateImagesWrtSides(imgPanel);
                    if (reportControlStructureList != null)
                    {
                        foreach (ReportControlsStructure repCtrlStr in reportControlStructureList)
                        {
                            if (imgPanel.Tag != null)
                            {
                                if (imgPanel.Tag.ToString().Contains("$" + repCtrlStr.reportControlProperty.Binding))
                                {
                                    repCtrlStr.reportControlProperty.RowsColumns._Rows = Convert.ToByte(imgPanel.RowCount);
                                    repCtrlStr.reportControlProperty.RowsColumns._Columns = Convert.ToByte(imgPanel.ColumnCount);
                                }
                            }
                            else//this has been added to populate images in old templates.
                            {
                                repCtrlStr.reportControlProperty.RowsColumns._Rows = Convert.ToByte(imgPanel.RowCount);
                                repCtrlStr.reportControlProperty.RowsColumns._Columns = Convert.ToByte(imgPanel.ColumnCount);
                            }
                        }
                    }
                }
            }
            
        }

        /// <summary>
        /// Read an saved report.
        /// </summary>
        /// <param name="xmlFile">report string</param>
        public bool readXML(string xmlData, bool isNewReport = false)
        {
            bool isChangeTemplate = false;//ool created to handle return value of changetemplate.By Ashutosh 05-09-2018.
            try
            {
                JsonReportModel existingJsonReportModel = new JavaScriptSerializer().Deserialize<JsonReportModel>(xmlData);
                _dataModel.CurrentImageNames = new string[existingJsonReportModel.reportDetails.Count];
                _dataModel.CurrentImgFiles = new string[existingJsonReportModel.reportDetails.Count];
                for (int i = 0; i < existingJsonReportModel.reportDetails.Count; i++)
                {
                    _dataModel.CurrentImgFiles[i] = existingJsonReportModel.reportDetails[i].imageFile;
                    _dataModel.CurrentImageNames[i] = existingJsonReportModel.reportDetails[i].imageName;
                }
                isNew = false;
                reportSize_cbx.DataSource = pageSize;
                LayoutDetails.Current.Orientation = existingJsonReportModel.currentOrientation;
                SetCurrentOrientationAndSize();
                save_btn.Enabled = false;
                IntitializeSizeAndOrientaionControls(reportSize);
                setToolsStripLabels();
                //the below code is for executing old created reports.
                if (existingJsonReportModel.reportValues.Count == 0)
                {
                    existingJsonReportModel.reportValues.Add(new KVData("comments", existingJsonReportModel.comments));
                    existingJsonReportModel.reportValues.Add(new KVData("doctor", existingJsonReportModel.doctor));
                    existingJsonReportModel.reportValues.Add(new KVData("MedHistory", existingJsonReportModel.MedHistory));
                    existingJsonReportModel.reportValues.Add(new KVData("leftEyeObs", existingJsonReportModel.leftEyeObs));
                    existingJsonReportModel.reportValues.Add(new KVData("rightEyeObs", existingJsonReportModel.rightEyeObs));
                   
                }
                //the below code is to open the recently saved report.
                for (int i = 0; i < _dataModel.ReportData.Count; i++)
                {
                   KeyValuePair<string,object> val = _dataModel.ReportData.ElementAt(i);
                    string keyVal = val.Key.Replace("$","");
                    for (int j = 0; j < existingJsonReportModel.reportValues.Count; j++)
                    {
                        if (existingJsonReportModel.reportValues[j].Key.ToLower() == keyVal.ToLower())
                            _dataModel.ReportData[val.Key] = existingJsonReportModel.reportValues[j].Value;
                        else if(!_dataModel.ReportData.ContainsKey("$" + existingJsonReportModel.reportValues[j].Key.ToString()))
                            _dataModel.ReportData.Add("$"+existingJsonReportModel.reportValues[j].Key, existingJsonReportModel.reportValues[j].Value);
                        else
                            continue;
                    }
                    //existingJsonReportModel.reportValues.Where(x => x.Key == keyVal).ToList();
                }
                
                isChangeTemplate = ChangeTemplate();
                if (autoAnalysis_btn.Enabled)
                    autoAnalysis_btn.Enabled = false;
                //doctor_tbx.Text = existingJsonReportModel.doctor;
                //comments_tbx.Text = existingJsonReportModel.MedHistory;
                //rightEyeObs_tbx.Text = existingJsonReportModel.rightEyeObs;
                //leftEyeObs_tbx.Text = existingJsonReportModel.leftEyeObs;
                //genObs_tbx.Text = existingJsonReportModel.comments;
                portrait_rb.Enabled = false;
                landscape_rb.Enabled = false;
                foreach (Control item in this.reportCanvas_pnl.Controls)
                {
                    item.Enabled = isNewReport;
                }
                reportSize_cbx.Enabled = false;
                this.Cursor = Cursors.Default;
                this.reportCanvas_pnl.Refresh();
            }
            catch (Exception ex)
            {
                using (StringReader sr = new StringReader(xmlData))
                {
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.ConformanceLevel = ConformanceLevel.Auto;
                    using (XmlReader xmlReader = XmlReader.Create(sr, settings))
                    {
                        {
                            XmlSerializer xmlSer = new XmlSerializer(typeof(ReportXmlProperties));
                            ReportXmlProperties reportXml = xmlSer.Deserialize(xmlReader) as ReportXmlProperties;
                            readExistingTemplate(reportXml,isNewReport);
                        }
                    }
                }
            }
            return isNewReport;
        }

        private  Bitmap CreateQRCodeBM()
        {
            List<ReportControlsStructure> tempList = reportControlStructureList.Where(x => x.reportControlProperty.Binding.Contains("QRCode")).ToList();
            if (tempList.Any())
            {
                var QCwriter = new BarcodeWriter();
                QCwriter.Format = BarcodeFormat.QR_CODE;
                var uriValue = (_dataModel.ReportData["$QRCode"].ToString());
                var result = QCwriter.Write(uriValue);
                _dataModel.qrBitmap = result.Clone() as Bitmap;
                return result;

            }
            return new Bitmap(10, 10);
        }
        /// <summary>
        /// Will set the orientation radio button and report size for existing report.
        /// </summary>
        /// <param name="isLandScape">landscape orientation or not</param>
        /// <param name="reportSize">string which has report size A4 OR A5.</param>
        public void IntitializeSizeAndOrientaionControls(string reportSize)
        {
            if (!_dataModel.ContainsCmdArgs)
            {
                switch (LayoutDetails.Current.Orientation)
                {
                    case LayoutDetails.PageOrientation.PORTRAIT_A4: portrait_rb.Checked = true;
                        break;
                    case LayoutDetails.PageOrientation.PORTRAIT_A5: portrait_rb.Checked = true;
                        break;
                    case LayoutDetails.PageOrientation.LANDSCAPE_A4: landscape_rb.Checked = true;
                        break;
                    case LayoutDetails.PageOrientation.LANDSCAPE_A5: landscape_rb.Checked = true;
                        break;
                }
                reportSize_cbx.Text = reportSize;
            }
        }

        /// <summary>
        /// Set the values from annotation to the report control structure.
        /// </summary>
        public void SetTheValuesFormReportData()
        {
            foreach (ReportControlsStructure item in reportControlStructureList)
            {
                if (_dataModel.ReportData.ContainsKey("$" + item.reportControlProperty.Binding.ToString()))
                {
                    //if (!string.IsNullOrEmpty(_dataModel.ReportData["$" + item.reportControlProperty.Binding.ToString()] as string))
                    {
                        setActualValuesToBindingValues(item.reportControlProperty.Binding.ToString(), _dataModel.ReportData["$" + item.reportControlProperty.Binding.ToString()] as string, item);
                    }
                }
            }
        }

        /// <summary>
        /// Set the values from control to the report control structure 
        /// </summary>
        /// <param name="bindingType">control binding</param>
        /// <param name="value">value assigned to control</param>
        public void setActualValuesToBindingValues(string bindingType, string value, ReportControlsStructure repCtrlStr)
        {
            if (repCtrlStr.reportControlProperty.Binding.ToString() == bindingType)
            {
                if (!repCtrlStr.reportControlProperty.Name.Contains("Picture") && !repCtrlStr.reportControlProperty.Binding.ToString().Contains("None"))
                    repCtrlStr.reportControlProperty.Text = value;

                else
                {
                    
                    repCtrlStr.reportControlProperty.ImageName = value;
                }
            }
        }

        /// <summary>
        /// Set the values from control to the report control structure 
        /// </summary>
        /// <param name="bindingType">control binding</param>
        /// <param name="value">value assigned to control</param>
        public void setActualValuesToBindingValues(string bindingType, string value)
        {
            foreach (ReportControlsStructure repCtrlStr in reportControlStructureList)
            {
                if (repCtrlStr.reportControlProperty.Binding.ToString() == bindingType)
                {
                    if (!repCtrlStr.reportControlProperty.Name.Contains("Picture") && !repCtrlStr.reportControlProperty.Binding.ToString().Contains("None"))
                        repCtrlStr.reportControlProperty.Text = value;
                    else
                        repCtrlStr.reportControlProperty.ImageName = value;
                    break;
                }
            }
        }

        /// <summary>
        /// Deserializes the xml file into report control structure type.
        /// </summary>
        /// <param name="xmlFile">file path</param>
        public void parseXmlData(string xmlFile)
        {
            if (!File.Exists(xmlFile)) return;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFile);
            _dataModel.CurrentTemplate = xmlFile;
            XmlNode controlNodes = xmlDoc.FirstChild.ChildNodes[1];
            if (controlNodes != null)
            {
                string ori = xmlDoc.FirstChild.ChildNodes[0].InnerText;
                LayoutDetails.Current.Orientation = (LayoutDetails.PageOrientation)Enum.Parse(typeof(LayoutDetails.PageOrientation), ori);
            }
            XmlSerializer xmlSer = new XmlSerializer(typeof(List<ReportControlsStructure>));
            string dpiStr = controlNodes.ChildNodes[0].OuterXml.ToLower();
            if (dpiStr.Contains("dpi"))
            {
                if (dpiStr.Contains("dpi_72"))
                {
                    LayoutDetails.Current.Dpi = LayoutDetails.DPI.DPI_72;
                }
                else
                {
                    LayoutDetails.Current.Dpi = LayoutDetails.DPI.DPI_96;

                }
                controlNodes = xmlDoc.FirstChild.ChildNodes[2];
            }
            else
            {
                LayoutDetails.Current.Dpi = LayoutDetails.DPI.DPI_72;

            }

            foreach (XmlNode item in controlNodes.ChildNodes)
            {
                MemoryStream memStearm = new MemoryStream();
                StreamWriter writer = new StreamWriter(memStearm);
                writer.Write(item.OuterXml);
                writer.Flush();
                memStearm.Position = 0;
                reportControlStructureList = (List<ReportControlsStructure>)xmlSer.Deserialize(memStearm);
            }
            
        }

        /// <summary>
        /// to populate the controls from the template xml
        /// </summary>
        /// <param name="IVLProps"></param>
        private void populateControls(ref ReportControlProperties IVLProps)
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
                        height72 = a4Width * 72.0;
                        width72 = a4Height * 72.0;

                        height96 = a4Width * 96.0;
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

                double newLocX = (double)IVLProps.Location._X * (width96 / width72);
                double newLocY = (double)IVLProps.Location._Y * (height96 / height72);
                double newWidth = (double)IVLProps.Size.Width * (width96 / width72);
                double newHeight = (double)IVLProps.Size.Height * (height96 / height72);
                IVLProps.Location._X = Convert.ToInt16(newLocX);
                IVLProps.Location._Y = Convert.ToInt16(newLocY);
                IVLProps.Size = new System.Drawing.Size(Convert.ToInt32(newWidth), Convert.ToInt32(newHeight));
            }
            //pDesignerCore.ActiveDesignSurface.CreateControl(IVLProps);
            float fontSize = IVLProps.Font.FontSize;
            //if (LayoutDetails.Current.Orientation == LayoutDetails.PageOrientation.PORTRAIT_A4)
            //    {
            //fontSize *= fontScaleFactor;
            //    }

            //propList.Add(IVLProps);
            switch (IVLProps.Type)
            {
                case "Label":
                    {
                        Label l = new Label();
                        IVLFont i = IVLProps.Font;
                        l.Text = IVLProps.Text;
                        if (IVLProps.Binding != BindingType.None.ToString() && IVLProps.Binding == BindingType.Name.ToString()) 
                            l.AutoEllipsis = true;
                        l.Name = IVLProps.Name;
                        l.Font = new Font(i.FontFamily,fontSize , i.FontStyle);

                        if (!Color.FromName(i.FontColor).IsKnownColor)
                        {
                            l.ForeColor = ColorTranslator.FromHtml("#" + i.FontColor);
                        }
                        else
                        l.ForeColor = Color.FromName( i.FontColor);//this.FontColor;//
                        if (IVLProps.Border)
                            l.BorderStyle = BorderStyle.FixedSingle;
                        if(IVLProps.Binding != BindingType.None.ToString())
                            l.Tag = "$"+IVLProps.Binding;
                        l.Location = new Point( IVLProps.Location._X, IVLProps.Location._Y);
                        l.Size = IVLProps.Size;
                        this.p.Controls.Add(l);
                        break;
                    }
                case "TextBox":
                    {
                        System.Windows.Forms.TextBox t = new System.Windows.Forms.TextBox();
                        IVLFont i = IVLProps.Font;
                        t.Text = IVLProps.Text;
                        t.Name = IVLProps.Name;
                        t.Font = new Font(i.FontFamily, fontSize, i.FontStyle);

                        if (!Color.FromName(i.FontColor).IsKnownColor)
                        {
                            t.ForeColor = ColorTranslator.FromHtml("#" + i.FontColor);
                        }
                        else
                            t.ForeColor = Color.FromName(i.FontColor);//this.FontColor;//

                        t.Multiline = IVLProps.MultiLine;
                        //if (IVLProps.Border)
                        t.BorderStyle = BorderStyle.FixedSingle;
                        if (IVLProps.Binding != BindingType.None.ToString())
                            t.Tag = "$" + IVLProps.Binding;
                        if (IVLProps.Binding == BindingType.Doctor.ToString())
                        {
                            t.MaxLength = 20;
                            t.ShortcutsEnabled = false;
                        }
                        t.Location = new Point(IVLProps.Location._X, IVLProps.Location._Y);
                        t.Size = IVLProps.Size;
                        t.KeyPress += t_KeyPress;
                        t.TextChanged += t_TextChanged;
                        this.p.Controls.Add(t);
                        break;
                    }
                case "PictureBox":
                    {
                        PictureBox pbx = new PictureBox();
                        IVLFont i = IVLProps.Font;
                        pbx.Text = IVLProps.Text;
                        pbx.Name = IVLProps.Name;
                        pbx.Font = new Font(i.FontFamily, fontSize, i.FontStyle);
                        pbx.ForeColor = Color.FromName(i.FontColor);
                        if (IVLProps.Border)
                            pbx.BorderStyle = BorderStyle.FixedSingle;
                        if (IVLProps.Binding != BindingType.None.ToString())
                        {
                            pbx.Tag = "$" + IVLProps.Binding;
                        }

                        if(IVLProps.Binding.Contains("QRCode"))
                            pbx.Image = CreateQRCodeBM();

                        else
                            if (File.Exists(IVLProps.ImageName))
                                pbx.Image = Image.FromFile(IVLProps.ImageName);
                        pbx.Location = new Point(IVLProps.Location._X, IVLProps.Location._Y);
                        pbx.Size = IVLProps.Size;
                        pbx.SizeMode = PictureBoxSizeMode.Zoom;
                        
                        this.p.Controls.Add(pbx);
                        break;
                    }
                case "IVL_ImagePanel":
                    {

                        IVL_ImagePanel imgPanel = new IVL_ImagePanel();
                        imgPanel.isMedicalName = IVLProps.ImageMedicalName;
                        IVLFont i = IVLProps.Font;
                        imgPanel.Text = IVLProps.Text;
                        imgPanel.Name = IVLProps.Name;
                        imgPanel.Font = new Font(i.FontFamily, fontSize, i.FontStyle);
                        imgPanel.ForeColor = Color.FromName(i.FontColor);
                        
                        imgPanel.pbclick += imageTable_pbclick;
                        if (IVLProps.Border)
                            imgPanel.BorderStyle = BorderStyle.FixedSingle;
                        if (IVLProps.Binding != BindingType.None.ToString())
                            imgPanel.Tag = "$" + IVLProps.Binding;
                        //SegregateImagesWrtSides(imgPanel);

                        imgPanel.Location = new Point(IVLProps.Location._X, IVLProps.Location._Y);
                        imgPanel.Size = IVLProps.Size;
                        addImgBox();
                        this.p.Controls.Add(imgPanel);
                        break;
                    }
                //case "Label":
                //    {
                //        break;
                //    }

            }
        }
        private void SegregateImagesWrtSides( IVL_ImagePanel imgPanel)
        {
            List<string> imageFilePaths = new List<string>();
            List<string> imageFileNames = new List<string>();
            imgPanel.Controls.Clear();
            if (imgPanel.Tag != null)//this hasbeen added if the image panel does not have any bindings (ie. to populate for the old templates in report window).
            {
                if (imgPanel.Tag.ToString() == "$RightEyeImages")
                {
                    //foreach (var item in )
                    for (int j = 0; j < _dataModel.CurrentImageNames.Length; j++)
                    {
                        if (_dataModel.CurrentImageNames[j].Contains("OD"))
                        {
                            imageFileNames.Add(_dataModel.CurrentImageNames[j]);
                            imageFilePaths.Add(_dataModel.CurrentImgFiles[j]);
                        }
                    }
                }
                else if (imgPanel.Tag.ToString() == "$LeftEyeImages")
                {
                    for (int j = 0; j < _dataModel.CurrentImageNames.Length; j++)
                    {
                        if (_dataModel.CurrentImageNames[j].Contains("OS"))
                        {
                            imageFileNames.Add(_dataModel.CurrentImageNames[j]);
                            imageFilePaths.Add(_dataModel.CurrentImgFiles[j]);
                        }
                    }
                }

                else if (imgPanel.Tag.ToString() == "$None")
                {
                    imageFileNames.AddRange(_dataModel.CurrentImageNames);
                    imageFilePaths.AddRange(_dataModel.CurrentImgFiles);
                }
            }
            else 
            {
                imageFileNames.AddRange(_dataModel.CurrentImageNames);
                imageFilePaths.AddRange(_dataModel.CurrentImgFiles);
            }
            imgPanel.SetImagePathAndNames(imageFilePaths.ToArray(), imageFileNames.ToArray());

        }

        /// <summary>
        /// key press event for all the text box in the report window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void t_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == AsciiValueOfControlBackspace)
                e.Handled = true;
            else if ((sender as System.Windows.Forms.TextBox).Lines.Length >= MAX_LINES && e.KeyChar == newlineChar)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// text changed event for all the text box in the report window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void t_TextChanged(object sender, EventArgs e)
        {
            setActualValuesToBindingValues((sender as System.Windows.Forms.TextBox).Tag.ToString().Replace("$", ""), (sender as System.Windows.Forms.TextBox).Text);
            if (_dataModel.ReportData.ContainsKey((sender as System.Windows.Forms.TextBox).Tag.ToString()))
                _dataModel.ReportData[(sender as System.Windows.Forms.TextBox).Tag.ToString()] = (sender as System.Windows.Forms.TextBox).Text;
            
            else if ((sender as System.Windows.Forms.TextBox).Tag.ToString() != BindingType.None.ToString())//This code has been added to change the reportdata when data for that binding type is changed in report window.
                _dataModel.ReportData.Add((sender as System.Windows.Forms.TextBox).Tag.ToString(),(sender as System.Windows.Forms.TextBox).Text);
        }

        /// <summary>
        /// This method will save the report genarated in reports form.
        /// </summary>
        /// <param name="e"></param>
        public void saveReport(EventArgs e)
        {

                if (reportSavedEvent != null)
                    reportSavedEvent(createReport(), e);
                isJsonFormat = true;
        }
    public Dictionary<string,object> createReport()
    {
        if (isNew)

            {
            JsonReportModel jsonModel = new JsonReportModel();
            for (int i = 0; i < _dataModel.CurrentImgFiles.Length; i++)
            {
                Observations rm = new Observations();
                rm.imageFile = _dataModel.CurrentImgFiles[i];
                rm.imageName = _dataModel.CurrentImageNames[i];
                jsonModel.reportDetails.Add(rm);
            }
            jsonModel.currentOrientation = LayoutDetails.Current.Orientation;
            //the below code is to add if new items added in the template with binding will get  saved to the database.
            for (int i = 0; i < _dataModel.ReportData.Count; i++)
            {
                KeyValuePair<string, object> val = _dataModel.ReportData.ElementAt(i);
                if (val.Key.Contains("$"))
                {
                    string keyVal = val.Key.Replace("$", "");
                    if (!string.IsNullOrEmpty(keyVal))
                        jsonModel.reportValues.Add(new KVData(keyVal, val.Value));
                    //existingJsonReportModel.reportValues.Where(x => x.Key == keyVal).ToList();
                }
            }
            //jsonModel.comments = genObs_tbx.Text;//This code has been added to save the general observation in the json report data.
            //jsonModel.MedHistory = comments_tbx.Text;//This code has been added to save the comments in the json report data.
            //jsonModel.rightEyeObs = rightEyeObs_tbx.Text;//This code has been added to save the right eye comments in the json report data.
            //jsonModel.leftEyeObs = leftEyeObs_tbx.Text;//This code has been added to save the  left eye comments in the json report data.
            //jsonModel.doctor = doctor_tbx.Text;//This code has been added to save the doctor in the json report data.

            string jsonReport = "";
            var json = JsonConvert.SerializeObject(jsonModel);
            jsonReport = json.ToString();
            Dictionary<string, object> repoval = new Dictionary<string, object>();
            repoval.Add("xml", jsonReport);
            repoval.Add("dateTime", DateTime.Now);
            repoval.Add("IsModifed", true);
            repoval.Add("IsJsonFormat", isJsonFormat);
            isNew = false;
            isTextChanged = false;
                return repoval;

        }
          return  new Dictionary<string, object>();
    }

        /// <summary>
        /// Creates a PDF file.
        /// </summary>
        public void CreatePdf()// file name passed to CreatePdf as a paramter . By Ashutosh 20-7-2017
        {
            
            this.Cursor = Cursors.WaitCursor;
            

            if (_dataModel.ReportData.ContainsKey("$AnnotationComments"))
                pdfGenerator.AnnotationComments = _dataModel.ReportData["$AnnotationComments"] as Dictionary<string, string>;
            if (!_dataModel.isFromCDR && !_dataModel.isFromAnnotation)//only if report is DR and normal it enters below. 11-08-2017.
            {
                if (_dataModel.Is2ImagesLS4ImagesPOR)//To check whether to change landscape or portrait for 2 images and 4 images.
                
                    if (!isDRReport) 
                    {
                        if (_dataModel.CurrentImgFiles.Length == 2)//Check the number of images selected is equal to 2.
                            reportTemplateOrientation = ReportTemplateOrientationEnum.Landscape;
                        else
                            if (_dataModel.CurrentImgFiles.Length == 4)//Check the number of images selected is equal to 4.
                                reportTemplateOrientation = ReportTemplateOrientationEnum.Portrait;
                    }
                if(!_dataModel.ContainsCmdArgs)
                    ChangeTemplate();
            }
           ReportFileName = pdfGenerator.GenaratePdf(reportControlStructureList, ReportFileName);
            pdfGenerator.AnnotationComments = new Dictionary<string, string>();
        //pdfGenerator.GenaratePdf(reportControlStructureList, ReportFileName);
        this.Cursor = Cursors.Default;
        }
        #endregion

        #region private methods

        /// <summary>
        /// Reads an object of type ReportXmlProperties. Old xml report data
        /// </summary>
        /// <param name="reportxml">parameter of type ReportXmlProperties with list of control properties</param>
        private void readExistingTemplate(ReportXmlProperties reportxml,bool isNewReport = false)
        {
            isNew = isNewReport;
            ReportXmlProperties newreportxml = reportxml;
            XmlSerializer xmlSer = new XmlSerializer(typeof(ReportXmlProperties));
            string reportOrientation = string.Empty;
            string reportSize = string.Empty;
            LayoutDetails.Current.Orientation = newreportxml._currentOrientation;
            switch (LayoutDetails.Current.Orientation)
            {
                case LayoutDetails.PageOrientation.LANDSCAPE:
                    LayoutDetails.Current.Orientation = LayoutDetails.PageOrientation.LANDSCAPE_A4;
                    reportOrientation = landscape_rb.Text;
                    reportSize = reportSizeA4;
                    break;
                case LayoutDetails.PageOrientation.PORTRAIT:
                    LayoutDetails.Current.Orientation = LayoutDetails.PageOrientation.PORTRAIT_A4;
                    reportOrientation = portrait_rb.Text;
                    reportSize = reportSizeA4;
                    break;
            }
            reportTemplateOrientation = (ReportTemplateOrientationEnum)Enum.Parse(typeof(ReportTemplateOrientationEnum), reportOrientation);
            reportTemplateSize = (ReportTemplateSizeEnum)Enum.Parse(typeof(ReportTemplateSizeEnum), reportSize);
            _dataModel.CurrentImgFiles = newreportxml.ImageURLS;
            _dataModel.CurrentImageNames = newreportxml.ImageNamesText;
            ChangeTemplate();
            isJsonFormat = false;
            foreach (ControlProperties item in newreportxml.reportControlProperties)
            {
                if (item.BindingType.Equals("$operator"))
                {
                   // doctor_tbx.Text = item.Text;
                }
                if (item.BindingType.Equals("$comment"))
                {
                    //genObs_tbx.Text = item.Text;
                }
            }
            portrait_rb.Enabled = false;
            landscape_rb.Enabled = false;
            this.reportCanvas_pnl.Refresh();
            saveReport(new EventArgs());
        }
        float HeightScaleFactor = 1f;
        float WidthScaleFactor = 1f;
        float fontScaleFactor = 1f;
        /// <summary>
        /// Changes the current template when the size or orientation has been changed.
        /// </summary>
        private bool ChangeTemplate()
        {
            //p.Controls.Clear();
            string changedTemplateFileName = "";//Local variable used to save the file name based on size and orientation
            bool isUpdateImages = false;//bool created to handle return value of updateimages(). By Ashutosh 05-09-2018.
            int index = 0;
            if (isDRReport)//Checks for DR report or Normal report.
                changedTemplateFileName = reportTemplateOrientation.ToString() + "_DR_" + reportTemplateSize.ToString() + xmlExtension;//For Dr report
            else
                changedTemplateFileName = reportTemplateOrientation.ToString() + "_" + reportTemplateSize.ToString() + xmlExtension;//For Normal report
            try
            {
                index = reportTemplates.FindIndex(x => x.Name == changedTemplateFileName);//Gives the index of the changedTemplateFileName from reportTemplates

            }
            catch (Exception ex)
            {
                
                throw;
            }
            FileInfo finf = new FileInfo(_dataModel.CurrentTemplate);//Gets the file info of the current template.
            if (index >= 0 && (finf.Name != changedTemplateFileName || reportControlStructureList == null))//Multiple expression if statement checks for index value , reportControlStructureList is null or not and Filename matches with current template.
            {
                p = new Panel();
                LayoutDetails.Current.Orientation = (LayoutDetails.PageOrientation)Enum.Parse(typeof(LayoutDetails.PageOrientation), reportTemplateOrientation.ToString().ToUpper() + "_" + reportTemplateSize.ToString());
                if(!_dataModel.ContainsCmdArgs)
                    _dataModel.CurrentTemplate = reportTemplates[index].FullName;
                this.reportCanvas_pnl.Controls.Clear();
                SetCurrentOrientationAndSize();
                parseXmlData(_dataModel.CurrentTemplate);//Reads the current template xml file
                if (LayoutDetails.Current.Orientation == LayoutDetails.PageOrientation.PORTRAIT_A4)
                {
                    //p.Dock = DockStyle.Fill; 
                     LayoutDetails.Current.PageHeight = Convert.ToInt32(a4Width * 96.0);
                     LayoutDetails.Current.PageWidth = Convert.ToInt32(a4Height * 96.0);
                    p.Size = new System.Drawing.Size(LayoutDetails.Current.PageWidth, LayoutDetails.Current.PageHeight);
                    p.Location = new Point((reportCanvas_pnl.Width * 1/5), reportCanvas_pnl.Location.Y);
                    //p.BorderStyle = BorderStyle.Fixed3D;
                    //p.Margin = new System.Windows.Forms.Padding(25, 0, 0, 0);
                    //p.BackColor = Color.Black;
                    HeightScaleFactor = 0.87f;
                    WidthScaleFactor = 1f;
                    fontScaleFactor = 0.875f;

                }
                else if (LayoutDetails.Current.Orientation == LayoutDetails.PageOrientation.PORTRAIT_A5)
                {

                     LayoutDetails.Current.PageHeight = Convert.ToInt32(a5Width * 96.0);
                     LayoutDetails.Current.PageWidth = Convert.ToInt32(a5Height * 96.0);
                    p.Size = new System.Drawing.Size(LayoutDetails.Current.PageWidth, LayoutDetails.Current.PageHeight);
                    //p.Dock = DockStyle.Fill;
                    p.Location = new Point((reportCanvas_pnl.Width * 1/5), reportCanvas_pnl.Location.Y);
                    //p.BorderStyle = BorderStyle.Fixed3D;
                    //p.Margin = new System.Windows.Forms.Padding(25, 0, 0, 0);
                    //p.BackColor = Color.Black;

                    WidthScaleFactor = 1.20f;
                    HeightScaleFactor = 1.25f;
                    fontScaleFactor = 0.95f;
                }
                else if (LayoutDetails.Current.Orientation == LayoutDetails.PageOrientation.LANDSCAPE_A5)
                {

                    LayoutDetails.Current.PageHeight = Convert.ToInt32(a5Height * 96.0);
                     LayoutDetails.Current.PageWidth = Convert.ToInt32(a5Width * 96.0);
                    p.Size = new System.Drawing.Size(LayoutDetails.Current.PageWidth, LayoutDetails.Current.PageHeight);
                   // p.Location = new Point(reportCanvas_pnl.Location.X, reportCanvas_pnl.Location.Y);
                    p.Location = new Point(LayoutDetails.Current.PageWidth / 5, LayoutDetails.Current.PageHeight/5);
                    //p.BorderStyle = BorderStyle.Fixed3D;
                    //p.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
                    //p.BackColor = Color.Black;
                }
                else
                {
                    LayoutDetails.Current.PageHeight = Convert.ToInt32(a4Height * 96.0);
                     LayoutDetails.Current.PageWidth = Convert.ToInt32(a4Width * 96.0);
                    p.Size = new System.Drawing.Size(LayoutDetails.Current.PageWidth, LayoutDetails.Current.PageHeight);
                    p.Location = new Point(0,0);
                    //p.BorderStyle = BorderStyle.Fixed3D;
                    //p.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
                    //p.BackColor = Color.Black;
                }


                foreach (ReportControlsStructure item in reportControlStructureList)
                {
                    populateControls(ref item.reportControlProperty);

                }
                if (LayoutDetails.Current.Dpi == LayoutDetails.DPI.DPI_72)
                {
                    LayoutDetails.Current.Dpi = LayoutDetails.DPI.DPI_96;
                    write2Xml(_dataModel.CurrentTemplate, reportControlStructureList);
                }
                // this.p.Scale(new SizeF(WidthScaleFactor, HeightScaleFactor));
                 writeValuesToTheBindingType();//Writes the values from the report data to the controls in the report canvas and to the correcponding binding type in reportControlStructureList.

                //this.p.Scale(scaleFactor);
                this.reportCanvas_pnl.Controls.Add(p);
                
                isUpdateImages = UpdateImages(_dataModel.CurrentImgFiles);//updates images of the image panel in the report canvas and changes the no of row and column for the image panel.
                setToolsStripLabels();
            }
            return isUpdateImages;
        }
        private void write2Xml(string xmlFile, List<ReportControlsStructure> props)
        {
            TextWriter xmlWriter = new StreamWriter(xmlFile);
            XmlSerializer xmlSer = null;
            try
            {
                xmlSer = new XmlSerializer(typeof(List<ReportControlsStructure>));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //List<ReportControlsStructure> props = pDesignerCore.GetControlProperties();

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
        /// <summary>
        /// Sets reportOrientation and reportSize so that  are used in the ChangeTemplate.
        /// </summary>
        private void SetCurrentOrientationAndSize()
        {
            switch (LayoutDetails.Current.Orientation)
            {
                case LayoutDetails.PageOrientation.LANDSCAPE:
                case LayoutDetails.PageOrientation.LANDSCAPE_A4:
                    reportOrientation = ReportTemplateOrientationEnum.Landscape.ToString();
                    reportSize = reportSizeA4;
                    break;
                case LayoutDetails.PageOrientation.PORTRAIT:
                case LayoutDetails.PageOrientation.PORTRAIT_A4:
                    reportOrientation = ReportTemplateOrientationEnum.Portrait.ToString();
                    reportSize = reportSizeA4;
                    break;
                case LayoutDetails.PageOrientation.PORTRAIT_A5:
                    reportOrientation = ReportTemplateOrientationEnum.Portrait.ToString();
                    reportSize = reportSizeA5;
                    break;
                case LayoutDetails.PageOrientation.LANDSCAPE_A5:
                    reportOrientation = ReportTemplateOrientationEnum.Landscape.ToString();
                    reportSize = reportSizeA5;
                    break;
            }
            reportTemplateOrientation = (ReportTemplateOrientationEnum)Enum.Parse(typeof(ReportTemplateOrientationEnum), reportOrientation);
            reportTemplateSize = (ReportTemplateSizeEnum)Enum.Parse(typeof(ReportTemplateSizeEnum), reportSize);
        }

        /// <summary>
        /// Sets the values to the controls in the report window based on the report data dictionary
        /// </summary>
        private void writeValuesToTheBindingType()
        {
            //foreach (Control item in this.reportCanvas_pnl.Controls)
            foreach (Control item in this.p.Controls)
            {
                if (item.Tag != null)
                {
                    if (item is PictureBox)
                    {
                        if (_dataModel.ReportData.ContainsKey(item.Tag.ToString()))
                        {
                            if (File.Exists(_dataModel.ReportData[item.Tag.ToString()].ToString()) && !string.IsNullOrEmpty(_dataModel.ReportData[item.Tag.ToString()] as string))
                            {
                                PictureBox pbs = item as PictureBox;
                                pbs.AccessibleDescription = _dataModel.ReportData[item.Tag.ToString()].ToString();
                                pbs.Image = new Bitmap(_dataModel.ReportData[item.Tag.ToString()].ToString());
                            }
                        }
                        else
                        {
                            
                        }
                    }
                    else if (_dataModel.ReportData.ContainsKey(item.Tag.ToString()))
                    {
                        if (!string.IsNullOrEmpty(_dataModel.ReportData[item.Tag.ToString()] as string))
                        {
                            item.Text = _dataModel.ReportData[item.Tag.ToString()] as string;
                        }
                    }
                }
                else if (item.GetType() == typeof(ReportUtils.IVL_ImagePanel))
                {
                        if (LayoutDetails.Current.Orientation == LayoutDetails.PageOrientation.PORTRAIT_A4 || LayoutDetails.Current.Orientation == LayoutDetails.PageOrientation.PORTRAIT_A5)
                            IVL_ImagePanel.isPortrait = true;
                        else
                            IVL_ImagePanel.isPortrait = false;
                        //if (reportControlStructureList != null)
                        //{
                        //    foreach (ReportControlsStructure repCtrlStr in reportControlStructureList)
                        //    {
                        //        if (item.GetType().ToString().Contains(repCtrlStr.reportControlProperty.Type))
                        //        {
                        //            repCtrlStr.reportControlProperty.RowsColumns._Rows = Convert.ToByte(imgPanel.RowCount);
                        //            repCtrlStr.reportControlProperty.RowsColumns._Columns = Convert.ToByte(imgPanel.ColumnCount);
                        //        }
                        //    }
                        //}
                }
            }
            SetTheValuesFormReportData();
            this.reportCanvas_pnl.Refresh();
        }

        void setToolsStripLabels()
        {
            orientationValue_lbl.Text = reportTemplateOrientation.ToString();
            sizeValue_lbl.Text = reportTemplateSize.ToString();
            if (isNew)
            {
                ReportStatusVal_lbl.Text = ReportStatusNew;
            }
            else
            {
                ReportStatusVal_lbl.Text = ReportStatusSaved;
            }
        }
        #endregion

        private  void autoAnalysis_btn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(apiRequestType))
            {
                this.Cursor = Cursors.WaitCursor;
                emailWindow.ImageFileNames = null;// in order to reset the imageFileNames
                emailWindow.ReportFileName = "";
                emailWindow.ImageFileNames = _dataModel.CurrentImgFiles;
                emailWindow.AIUserName = userName;
                emailWindow.AIPassword = password;
                emailWindow.AIApiRequestType = apiRequestType;
                _dataModel.mailData.Subject = "New Patient Referral " + _dataModel.ReportData["$HospitalName"] + " Referral on : " + DateTime.Now.ToString("dd MMM yy hh:mm tt ");
                _dataModel.mailData.Body = "Patient Details \n" + Environment.NewLine +
                        "Name :" + _dataModel.patientDetails.FirstName + " " + _dataModel.patientDetails.LastName + Environment.NewLine +
                        "MRN :" + _dataModel.patientDetails.MRN + Environment.NewLine +
                         "Age :" + _dataModel.patientDetails.Age.ToString() + Environment.NewLine +
                         "Gender :" + _dataModel.patientDetails.Gender + Environment.NewLine + Environment.NewLine;
                emailWindow.vendorVal = (INTUSOFT.Configuration.ConfigVariables.CurrentSettings.ReportSettings.AI_Vendor.val);
                //emailWindow.Upload2Cloud(new object());
                ThreadPool.QueueUserWorkItem(new WaitCallback(emailWindow.Upload2Cloud));
            }
            else
            {
                CustomMessageBox.Show("Please enter the username, password and api request type in report settings", "Warning", CustomMessageBoxButtons.OK, CustomMessageBoxIcon.Warning, 442, 135);
            }
         }




    }
}
