using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace INTUSOFT.Configuration.AdvanceSettings
{
     [Serializable]
    public class UserSettings
    {
        //public string HeaderText = "IntuVision Labs ";
        //public float ZoomMagnifierMax = 300f;
        //public int ZoomMagnifierWidth = 200;
        //public int ZoomMagnifierHeight = 200;
        //public float ZoomMagnifierMin = 100.0f;
        //public float ZoomIncrementFactor = 2.5f;
        //public float zoomScale = 0.01f;
        //public string MrnString = "IVL_";
        //public bool isMrnConfigurable = false;
        //public int mrnCnt = 0;
        //public bool is24clock = false;
        //public bool isAgeSelect = true;
        //public string DoctorName = "";

        private static  IVLControlProperties HeaderText = null;

        public IVLControlProperties _HeaderText
        {
            get { return HeaderText; }
            set { HeaderText = value; }
        }
        //private static  IVLControlProperties ZoomMagnifierMax = null;

        //public IVLControlProperties _ZoomMagnifierMax
        //{
        //    get { return ZoomMagnifierMax; }
        //    set { ZoomMagnifierMax = value; }
        //}
        private static  IVLControlProperties ZoomMagnifierWidth = null;

        public IVLControlProperties _ZoomMagnifierWidth
        {
            get { return ZoomMagnifierWidth; }
            set { ZoomMagnifierWidth = value; }
        }
        private static  IVLControlProperties ZoomMagnifierHeight = null;

        public IVLControlProperties _ZoomMagnifierHeight
        {
            get { return ZoomMagnifierHeight; }
            set { ZoomMagnifierHeight = value; }
        }
        //private static  IVLControlProperties ZoomMagnifierMin = null;

        //public IVLControlProperties _ZoomMagnifierMin
        //{
        //    get { return ZoomMagnifierMin; }
        //    set { ZoomMagnifierMin = value; }
        //}
        private static  IVLControlProperties ZoomIncrementFactor = null;

        public IVLControlProperties _ZoomIncrementFactor
        {
            get { return ZoomIncrementFactor; }
            set { ZoomIncrementFactor = value; }
        }
        private static  IVLControlProperties zoomScale = null;

        public IVLControlProperties _ZoomScale
        {
            get { return zoomScale; }
            set { zoomScale = value; }
        }
        private static  IVLControlProperties MrnString = null;

        public IVLControlProperties _MrnString
        {
            get { return MrnString; }
            set { MrnString = value; }
        }
        private static  IVLControlProperties isMrnConfigurable = null;

        public IVLControlProperties _IsMrnConfigurable
        {
            get { return isMrnConfigurable; }
            set { isMrnConfigurable = value; }
        }

        private static  IVLControlProperties mrnCnt = null;

        public IVLControlProperties _MrnCnt
        {
            get { return mrnCnt; }
            set { mrnCnt = value; }
        }

        private static  IVLControlProperties noOfImagesForReport = null;

        public IVLControlProperties _noOfImagesForReport
        {
            get { return noOfImagesForReport; }
            set { noOfImagesForReport = value; }
        }

        private static  IVLControlProperties is24clock = null;

        public IVLControlProperties _Is24clock
        {
            get { return is24clock; }
            set {
                is24clock = value;
             
                }
        }
        private static  IVLControlProperties isAgeSelect = null;

        public IVLControlProperties _IsAgeSelect
        {
            get { return isAgeSelect; }
            set { isAgeSelect = value; }
        }

        private static  IVLControlProperties isValidateNonMandatory = null;

        public IVLControlProperties _isValidateNonMandatory
        {
            get { return isValidateNonMandatory; }
            set { isValidateNonMandatory = value; }
        }

        private static  IVLControlProperties enableExportImageBrowser = null;

        public IVLControlProperties _enableExportImageBrowser
        {
            get { return enableExportImageBrowser; }
            set { enableExportImageBrowser = value; }
        }

        private static  IVLControlProperties DoctorName = null;

        public IVLControlProperties _DoctorName
        {
            get { return DoctorName; }
            set { DoctorName = value; }
        }


        private static  IVLControlProperties NoOfPatientsToBeSelected = null;

        public IVLControlProperties _NoOfPatientsToBeSelected
        {
            get { return NoOfPatientsToBeSelected; }
            set { NoOfPatientsToBeSelected = value; }
        }
        private static  IVLControlProperties Language = null;
        public IVLControlProperties _Language
        {
            get { return Language; }
            set { Language = value; }
        }
        public UserSettings()
        {
            _HeaderText = new IVLControlProperties();
            _HeaderText.name = "HeaderText";
            _HeaderText.val = "IntuVision Labs";
            _HeaderText.type = "string";
            _HeaderText.control = "System.Windows.Forms.TextBox";
            _HeaderText.text = "Header Text";
            _HeaderText.length = 50;

            //_ZoomMagnifierMax = new IVLControlProperties();
            //_ZoomMagnifierMax.name = "ZoomMagnifierMax";
            //_ZoomMagnifierMax.val = "300";
            //_ZoomMagnifierMax.type = "int";
            //_ZoomMagnifierMax.control = "System.Windows.Forms.NumericUpDown";
            //_ZoomMagnifierMax.text = "Zoom Magnifier Max";

            _ZoomMagnifierWidth = new IVLControlProperties();
            _ZoomMagnifierWidth.name = "ZoomMagnifierWidth";
            _ZoomMagnifierWidth.val = "200";
            _ZoomMagnifierWidth.type = "int";
            _ZoomMagnifierWidth.control = "System.Windows.Forms.NumericUpDown";
            _ZoomMagnifierWidth.text = "Zoom Magnifier Width";
            _ZoomMagnifierWidth.min = 200;
            _ZoomMagnifierWidth.max = 400;
            _ZoomMagnifierWidth.range = _ZoomMagnifierWidth.min.ToString() + " to " + _ZoomMagnifierWidth.max.ToString();
            
            _ZoomMagnifierHeight = new IVLControlProperties();
            _ZoomMagnifierHeight.name = "ZoomMagnifierHeight";
            _ZoomMagnifierHeight.val = "200";
            _ZoomMagnifierHeight.type = "int";
            _ZoomMagnifierHeight.control = "System.Windows.Forms.NumericUpDown";
            _ZoomMagnifierHeight.text = "Zoom Magnifier Height";
            _ZoomMagnifierHeight.min = 200;
            _ZoomMagnifierHeight.max = 400;
            _ZoomMagnifierHeight.range = _ZoomMagnifierHeight.min.ToString() + " to " + _ZoomMagnifierHeight.max.ToString();

            _noOfImagesForReport = new IVLControlProperties();
            _noOfImagesForReport.name = "noOfImagesForReport";
            _noOfImagesForReport.val = "16";
            _noOfImagesForReport.type = "int";
            _noOfImagesForReport.control = "System.Windows.Forms.NumericUpDown";
            _noOfImagesForReport.text = "No Of Images For Report";
            _noOfImagesForReport.min = 1;
            _noOfImagesForReport.max = 35;
            _noOfImagesForReport.range = _noOfImagesForReport.min.ToString() + " to " + _noOfImagesForReport.max.ToString();

           _NoOfPatientsToBeSelected = new IVLControlProperties();
           _NoOfPatientsToBeSelected.name = "NoOfPatientsToBeSelected";
           _NoOfPatientsToBeSelected.val = "10";
           _NoOfPatientsToBeSelected.type = "int";
           _NoOfPatientsToBeSelected.control = "System.Windows.Forms.NumericUpDown";
           _NoOfPatientsToBeSelected.text = "Patients to Be Selected ";
           _NoOfPatientsToBeSelected.min = 5;
           _NoOfPatientsToBeSelected.max = 25;
           _NoOfPatientsToBeSelected.range = _NoOfPatientsToBeSelected.min.ToString() + " to " + _NoOfPatientsToBeSelected.max.ToString();

            //_ZoomMagnifierMin = new IVLControlProperties();
            //_ZoomMagnifierMin.name = "ZoomMagnifierMin";
            //_ZoomMagnifierMin.val = "100";
            //_ZoomMagnifierMin.type = "int";
            //_ZoomMagnifierMin.control = "System.Windows.Forms.NumericUpDown";
            //_ZoomMagnifierMin.text = "Zoom Magnifier Min";

            _ZoomIncrementFactor = new IVLControlProperties();
            _ZoomIncrementFactor.name = "ZoomIncrementFactor";
            _ZoomIncrementFactor.type = "float";
            _ZoomIncrementFactor.val = "2.5";
            _ZoomIncrementFactor.control = "System.Windows.Forms.NumericUpDown";
            _ZoomIncrementFactor.text = "Zoom Increment Factor";
            _ZoomIncrementFactor.min = 1;
            _ZoomIncrementFactor.max = 10;
            _ZoomIncrementFactor.range = _ZoomIncrementFactor.min.ToString() + " to " + _ZoomIncrementFactor.max.ToString();

            _ZoomScale = new IVLControlProperties();
            _ZoomScale.name = "ZoomScale";
            _ZoomScale.type = "float";
            _ZoomScale.val = "0.01";
            _ZoomScale.control = "System.Windows.Forms.NumericUpDown";
            _ZoomScale.text = "Zoom Scale";

            _MrnString = new IVLControlProperties();
            _MrnString.name = "MrnString";
            _MrnString.type = "string";
            _MrnString.val = "IVL_";
            _MrnString.control = "System.Windows.Forms.TextBox";
            _MrnString.text = "Mrn String";
            _MrnString.length = 25;

            _isValidateNonMandatory = new IVLControlProperties();
            _isValidateNonMandatory.name = "IsValidateNonMandatory";
            _isValidateNonMandatory.type = "bool";
            _isValidateNonMandatory.val = false.ToString();
            _isValidateNonMandatory.control = "System.Windows.Forms.RadioButton";
            _isValidateNonMandatory.text = "Validate Non Mandatory Fields";

            _enableExportImageBrowser = new IVLControlProperties();
            _enableExportImageBrowser.name = "EnableExportImageBrowser";
            _enableExportImageBrowser.type = "bool";
            _enableExportImageBrowser.val = true.ToString();
            _enableExportImageBrowser.control = "System.Windows.Forms.RadioButton";
            _enableExportImageBrowser.text = "Enable Export Image Browser";

            _IsMrnConfigurable = new IVLControlProperties();
            _IsMrnConfigurable.name = "isMrnConfigurable";
            _IsMrnConfigurable.type = "bool";
            _IsMrnConfigurable.val = false.ToString();
            _IsMrnConfigurable.control = "System.Windows.Forms.RadioButton";
            _IsMrnConfigurable.text = "Mrn Configurable";

            _MrnCnt = new IVLControlProperties();
            _MrnCnt.name = "mrnCnt";
            _MrnCnt.val = "0";
            _MrnCnt.type = "int";
            _MrnCnt.control = "System.Windows.Forms.NumericUpDown";
            _MrnCnt.text = "MRN Count";

            _Is24clock = new IVLControlProperties();
            _Is24clock.name = "is24clock";
            _Is24clock.val = false.ToString();
            _Is24clock.type = "bool";
            _Is24clock.control = "System.Windows.Forms.RadioButton";
            _Is24clock.text = "24hr Format";

            _IsAgeSelect = new IVLControlProperties();
            _IsAgeSelect.name = "isAgeSelect";
            _IsAgeSelect.val = true.ToString();
            _IsAgeSelect.type = "bool";
            _IsAgeSelect.control = "System.Windows.Forms.RadioButton";
            _IsAgeSelect.text = "Select Age";

            _DoctorName = new IVLControlProperties();
            _DoctorName.name = "DoctorName";
            _DoctorName.val = "";
            _DoctorName.type = "string";
            _DoctorName.control = "System.Windows.Forms.TextBox";
            _DoctorName.text = "Doctor Name";
            _DoctorName.length = 20;

            _Language = new IVLControlProperties();
            _Language.name = "Language";
            _Language.val = "English";
            _Language.type = "string";
            _Language.control = "System.Windows.Forms.ComboBox";
            _Language.range = "English,Spanish";
            _Language.text = "Language";
        }
    }
}
