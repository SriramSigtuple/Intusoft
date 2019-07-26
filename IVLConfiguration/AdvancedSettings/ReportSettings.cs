using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
namespace INTUSOFT.Configuration.AdvanceSettings
{
    [Serializable]
    public class ReportSettings
    {

        //public int adobePrintDelay = 20000;
        private static  IVLControlProperties defaultTemplate = null;
        public IVLControlProperties DefaultTemplate
        {
            get { return defaultTemplate; }
            set { defaultTemplate = value; }
        }

        private static  IVLControlProperties reportSize = null;
        public IVLControlProperties ReportSize
        {
            get { return reportSize; }
            set { reportSize = value; }
        }

        private static  IVLControlProperties isTelemedReport = null;
        public IVLControlProperties IsTelemedReport
        {
            get { return isTelemedReport; }
            set { isTelemedReport = value; }
        }

        //private static  IVLControlProperties is2ImagesLS4ImagesPOR = null;

        //public IVLControlProperties Is2ImagesLS4ImagesPOR
        //{
        //    get { return is2ImagesLS4ImagesPOR; }
        //    set { is2ImagesLS4ImagesPOR = value; }
        //}
        

        //properties to enable the visibility of the print button.
        private static  IVLControlProperties showPrintButton = null;
        public IVLControlProperties ShowPrintButton
        {
            get { return showPrintButton; }
            set { showPrintButton = value; }
        }

        //properties to enable the visibility of the save button.
        private static  IVLControlProperties showSaveButton = null;
        public IVLControlProperties ShowSaveButton
        {
            get { return showSaveButton; }
            set { showSaveButton = value; }
        }

        //properties to enable the visibility of the export button.
        private static  IVLControlProperties showExportButton = null;
        public IVLControlProperties ShowExportButton
        {
            get { return showExportButton; }
            set { showExportButton = value; }
        }

        //properties to enable the visibility of the email images button.
        private static  IVLControlProperties showEmailImagesButton = null;
        public IVLControlProperties ShowEmailImagesButton
        {
            get { return showEmailImagesButton; }
            set { showEmailImagesButton = value; }
        }

        //properties to enable the visibility of the email report button.
        private static  IVLControlProperties showEmailReportButton = null;
        public IVLControlProperties ShowEmailReportButton
        {
            get { return showEmailReportButton; }
            set { showEmailReportButton = value; }
        }

        //properties to enable the visibility of the email telemed  button.
        private static  IVLControlProperties showEmailTelemedButton = null;
        public IVLControlProperties ShowEmailTelemedButton
        {
            get { return showEmailTelemedButton; }
            set { showEmailTelemedButton = value; }
        }

        private static  IVLControlProperties reporterName = null;
        public IVLControlProperties ReporterName
        {
            get { return reporterName; }
            set { reporterName = value; }
        }

        private static  IVLControlProperties address1 = null;//properties to display the address line 1 to the report.
        public IVLControlProperties Address1
        {
            get { return address1; }
            set { address1 = value; }
        }

        private static  IVLControlProperties address2 = null;//properties to display the address line 1 to the report.
        public IVLControlProperties Address2
        {
            get { return address2; }
            set { address2 = value; }
        }

        private static  IVLControlProperties reporterQualification = null;
        public IVLControlProperties ReporterQualification
        {
            get { return reporterQualification; }
            set { reporterQualification = value; }
        }

        private static  IVLControlProperties reporterHospital = null;
        public IVLControlProperties ReporterHospital
        {
            get { return reporterHospital; }
            set { reporterHospital = value; }
        }

        private static  IVLControlProperties showGenObs = null;

        public IVLControlProperties ShowGenObs
        {
            get { return showGenObs; }
            set { showGenObs = value; }
        }

        private static  IVLControlProperties fundusReportText = null;

        public IVLControlProperties FundusReportText
        {
            get { return fundusReportText; }
            set { fundusReportText = value; }
        }

        private static  IVLControlProperties showLeftObs = null;

        public IVLControlProperties ShowLeftObs
        {
            get { return showLeftObs; }
            set { showLeftObs = value; }
        }

        private static  IVLControlProperties showRightObs = null;

        public IVLControlProperties ShowRightObs
        {
            get { return showRightObs; }
            set { showRightObs = value; }
        }
        //properties of CDR report.By Ashutosh 17-08-2017
        private static  IVLControlProperties nameOfCdrReport= null;
        public IVLControlProperties NameOfCDRReport
        {
            get { return nameOfCdrReport; }
            set { nameOfCdrReport = value; }
        }
        //properties of Annotation report.By Ashutosh 17-08-2017
        private static  IVLControlProperties nameOfAnnotationReport = null;
        public IVLControlProperties NameOfAnnotationReport
        {
            get { return nameOfAnnotationReport; }
            set { nameOfAnnotationReport = value; }
        }

        //properties of Mask colour.By Ashutosh 22-08-2017
        private static  IVLControlProperties changeMaskColour = null;
        public IVLControlProperties ChangeMaskColour
        {
            get { return changeMaskColour; }
            set { changeMaskColour = value; }
        }

        private static IVLControlProperties showAutoAnalysisButton = null;

        public IVLControlProperties ShowAutoAnalysisButton
        {
            get { return showAutoAnalysisButton; }
            set { showAutoAnalysisButton = value; }
        }

        private static IVLControlProperties exportZipFilePath = null;

        public  IVLControlProperties ExportZipFilePath
        {
            get { return ReportSettings.exportZipFilePath; }
            set { ReportSettings.exportZipFilePath = value; }
        }
        private static IVLControlProperties aI_Vendor = null;

        public  IVLControlProperties AI_Vendor
        {
            get { return ReportSettings.aI_Vendor; }
            set { ReportSettings.aI_Vendor = value; }
        }

        private static IVLControlProperties userName = null;

        public  IVLControlProperties UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private static IVLControlProperties password = null;

        public  IVLControlProperties Password
        {
            get { return password; }
            set { password = value; }
        }

        private static IVLControlProperties apiRequestType = null;

        public  IVLControlProperties ApiRequestType
        {
            get { return apiRequestType; }
            set { apiRequestType = value; }
        }

        public ReportSettings()
        {
            DefaultTemplate = new IVLControlProperties();
            DefaultTemplate.name = "defaultTemplate";
            DefaultTemplate.val = "Landscape";
            DefaultTemplate.type = "string";
            DefaultTemplate.control = "System.Windows.Forms.ComboBox";
            DefaultTemplate.text = "Default Template";
            DefaultTemplate.range = "Landscape,Portrait,Landscape_DR,Portrait_DR";
            DefaultTemplate.length = 100000;

            #region Properties of Change mask colour feature.By Ashutosh 22-08-2017
            ChangeMaskColour = new IVLControlProperties();
            ChangeMaskColour.name = "ChangeMaskColour";
            ChangeMaskColour.val = "White";
            ChangeMaskColour.type = "string";
            ChangeMaskColour.control = "System.Windows.Forms.ComboBox";
            ChangeMaskColour.text = "Change Mask Colour";
            ChangeMaskColour.range = "White,Black,Red,Blue,Green";
            ChangeMaskColour.length = 100;
            #endregion

            ReporterName = new IVLControlProperties();
            ReporterName.name = "reporterName";
            ReporterName.val = " ";
            ReporterName.type = "string";
            ReporterName.control = "System.Windows.Forms.TextBox";
            ReporterName.text = "Reporter Name";
            ReporterName.length = 200;

            Address1 = new IVLControlProperties();
            Address1.name = "Address1";
            Address1.val = " ";
            Address1.type = "string";
            Address1.control = "System.Windows.Forms.TextBox";
            Address1.text = "Address1";
            Address1.length = 100;

            Address2 = new IVLControlProperties();
            Address2.name = "Address2";
            Address2.val = " ";
            Address2.type = "string";
            Address2.control = "System.Windows.Forms.TextBox";
            Address2.text = "Address2";
            Address2.length = 100;

            ReporterQualification = new IVLControlProperties();
            ReporterQualification.name = "reporterQualification";
            ReporterQualification.val = " ";
            ReporterQualification.type = "string";
            ReporterQualification.control = "System.Windows.Forms.TextBox";
            ReporterQualification.text = "Reporter Qualification";
            ReporterQualification.length = 200;

            ReporterHospital = new IVLControlProperties();
            ReporterHospital.name = "reporterHospital";
            ReporterHospital.val = " ";
            ReporterHospital.type = "string";
            ReporterHospital.control = "System.Windows.Forms.TextBox";
            ReporterHospital.text = "Reporter Hospital";
            ReporterHospital.length = 200;

            ReportSize = new IVLControlProperties();
            ReportSize.name = "reportSize";
            ReportSize.val = "A4";
            ReportSize.type = "string";
            ReportSize.control = "System.Windows.Forms.ComboBox";
            ReportSize.range = "A4,A5";
            ReportSize.text = "Report Size";
            ReportSize.length = 100;

            IsTelemedReport = new IVLControlProperties();
            IsTelemedReport.name = "isTelemedReport";
            IsTelemedReport.val = "false";
            IsTelemedReport.type = "bool";
            IsTelemedReport.control = "System.Windows.Forms.RadioButton";
            IsTelemedReport.text = "Is Telemed Report";
            IsTelemedReport.length = 100000;


            //Is2ImagesLS4ImagesPOR = new IVLControlProperties();
            //Is2ImagesLS4ImagesPOR.name = "is2ImagesLS4ImagesPOR";
            //Is2ImagesLS4ImagesPOR.val = "true";
            //Is2ImagesLS4ImagesPOR.type = "bool";
            //Is2ImagesLS4ImagesPOR.control = "System.Windows.Forms.RadioButton";
            //Is2ImagesLS4ImagesPOR.text = "Is 2 Images LS 4 Images POR";
            //Is2ImagesLS4ImagesPOR.length = 100000;

            //if to show the print button in the report module ,the visibility is true
            ShowPrintButton = new IVLControlProperties();
            ShowPrintButton.name = "showPrintButton";
            ShowPrintButton.val = "true";
            ShowPrintButton.type = "bool";
            ShowPrintButton.control = "System.Windows.Forms.RadioButton";
            ShowPrintButton.text = "Show Print Button";
            ShowPrintButton.length = 100000;

            //if to show the save button in the report module ,the visibility is true
            ShowSaveButton = new IVLControlProperties();
            ShowSaveButton.name = "showSaveButton";
            ShowSaveButton.val = "true";
            ShowSaveButton.type = "bool";
            ShowSaveButton.control = "System.Windows.Forms.RadioButton";
            ShowSaveButton.text = "Show Save Button";
            ShowSaveButton.length = 100000;

            //if to show the export button in the report module ,the visibility is true
            ShowExportButton = new IVLControlProperties();
            ShowExportButton.name = "showExportButton";
            ShowExportButton.val = "true";
            ShowExportButton.type = "bool";
            ShowExportButton.control = "System.Windows.Forms.RadioButton";
            ShowExportButton.text = "Show Export Button";
            ShowExportButton.length = 100000;

            //if to show the email images button in the report module ,the visibility is true
            ShowEmailImagesButton = new IVLControlProperties();
            ShowEmailImagesButton.name = "showEmailImagesButton";
            ShowEmailImagesButton.val = "false";
            ShowEmailImagesButton.type = "bool";
            ShowEmailImagesButton.control = "System.Windows.Forms.RadioButton";
            ShowEmailImagesButton.text = "Show Email Images Button";
            ShowEmailImagesButton.length = 100000;

            //if to show the email report button in the report module ,the visibility is true
            ShowEmailReportButton = new IVLControlProperties();
            ShowEmailReportButton.name = "showEmailReportButton";
            ShowEmailReportButton.val = "false";
            ShowEmailReportButton.type = "bool";
            ShowEmailReportButton.control = "System.Windows.Forms.RadioButton";
            ShowEmailReportButton.text = "Show Email Report Button";
            ShowEmailReportButton.length = 100000;

            //if to show the email telemed button in the report module ,the visibility is true
            ShowEmailTelemedButton = new IVLControlProperties();
            ShowEmailTelemedButton.name = "showEmailTelemedButton";
            ShowEmailTelemedButton.val = "false";
            ShowEmailTelemedButton.type = "bool";
            ShowEmailTelemedButton.control = "System.Windows.Forms.RadioButton";
            ShowEmailTelemedButton.text = "Show Email Telemed Button";
            ShowEmailTelemedButton.length = 100000;


            ShowAutoAnalysisButton = new IVLControlProperties();
            ShowAutoAnalysisButton.name = "showAutoAnalysisButton";
            ShowAutoAnalysisButton.val = "false";
            ShowAutoAnalysisButton.type = "bool";
            ShowAutoAnalysisButton.control = "System.Windows.Forms.RadioButton";
            ShowAutoAnalysisButton.text = "Show Auto Analysis Button";
            ShowAutoAnalysisButton.length = 100000;


            ShowGenObs = new IVLControlProperties();
            ShowGenObs.name = "showGenObs";
            ShowGenObs.val = "true";
            ShowGenObs.type = "bool";
            ShowGenObs.control = "System.Windows.Forms.RadioButton";
            ShowGenObs.text = "Show Gen Obs";
            ShowGenObs.length = 100000;

            ShowLeftObs = new IVLControlProperties();
            ShowLeftObs.name = "showLeftObs";
            ShowLeftObs.val = "true";
            ShowLeftObs.type = "bool";
            ShowLeftObs.control = "System.Windows.Forms.RadioButton";
            ShowLeftObs.text = "Show Left Obs";
            ShowLeftObs.length = 100000;

            ShowRightObs = new IVLControlProperties();
            ShowRightObs.name = "showRightObs";
            ShowRightObs.val = "true";
            ShowRightObs.type = "bool";
            ShowRightObs.control = "System.Windows.Forms.RadioButton";
            ShowRightObs.text = "Show Right Obs";
            ShowRightObs.length = 100000;

            FundusReportText = new IVLControlProperties();
            FundusReportText.name = "fundusReportText";
            FundusReportText.val = " Fundus Report";
            FundusReportText.type = "string";
            FundusReportText.control = "System.Windows.Forms.TextBox";
            FundusReportText.text = "Fundus Report Text";
            FundusReportText.length = 200;

            //properties of CDR report.By Ashutosh 17-08-2017
            NameOfCDRReport = new IVLControlProperties();
            NameOfCDRReport.name = "NameOfCDRReport";
            NameOfCDRReport.val = "Cup Disc Ratio Report";
            NameOfCDRReport.type = "string";
            NameOfCDRReport.control = "System.Windows.Forms.TextBox";
            NameOfCDRReport.text = "Cup Disc Ratio Report Text";
            NameOfCDRReport.length = 200;

            //properties of Annotation report.By Ashutosh 17-08-2017
            NameOfAnnotationReport = new IVLControlProperties();
            NameOfAnnotationReport.name = "NameOfAnnotationReport";
            NameOfAnnotationReport.val = "Annotation Report ";
            NameOfAnnotationReport.type = "string";
            NameOfAnnotationReport.control = "System.Windows.Forms.TextBox";
            NameOfAnnotationReport.text = "Annotation Report Text";
            NameOfAnnotationReport.length = 200;

            ExportZipFilePath = new IVLControlProperties();
            ExportZipFilePath.name = "ExportZipFilePath";
            ExportZipFilePath.val = "";
            ExportZipFilePath.type = "string";
            ExportZipFilePath.control = "System.Windows.Forms.TextBox";
            ExportZipFilePath.text = "Export Zip File Path";
            ExportZipFilePath.length = 200;

            AI_Vendor = new IVLControlProperties();
            AI_Vendor.name = "AI_Vendor";
            AI_Vendor.val = "Vendor1";
            AI_Vendor.type = "string";
            AI_Vendor.control = "System.Windows.Forms.ComboBox";
            AI_Vendor.range = "Vendor1,Vendor2,Vendor3";
            AI_Vendor.text = "AI Vendor";
            AI_Vendor.length = 200;


            UserName = new IVLControlProperties();
            UserName.name = "UserName";
            UserName.val = "";
            UserName.type = "string";
            UserName.control = "System.Windows.Forms.TextBox";
            UserName.text = "UserName";
            UserName.length = 500;


            Password = new IVLControlProperties();
            Password.name = "Password";
            Password.val = "";
            Password.type = "string";
            Password.control = "System.Windows.Forms.TextBox";
            Password.text = "Password";
            Password.length = 500;


            ApiRequestType = new IVLControlProperties();
            ApiRequestType.name = "ApiRequestType";
            ApiRequestType.val = "";
            ApiRequestType.type = "string";
            ApiRequestType.control = "System.Windows.Forms.TextBox";
            ApiRequestType.text = "Api Request Type";
            ApiRequestType.length = 500;

        }
    }
}
