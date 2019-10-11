using INTUSOFT.Data.Repository;
using INTUSOFT.Desktop.Forms;
using INTUSOFT.EventHandler;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace INTUSOFT.Desktop
{
    public class EmrViewModel
    {
        #region variables and constants
        IVLEventHandler _eventHandler;
        CreatePatientDetails_form cf;
        public Timer t = new Timer();
        bool isReport;
        int currentIndx = 0;
        System.Diagnostics.Stopwatch stW;
        public delegate void ReportDelegate();
        public event ReportDelegate reportViewEvent;
        List<INTUSOFT.Data.NewDbModel.Patient> TemPatList;
        int prevCntPatCnt = 0;
        bool isUpdate = false;
        Logger ExceptionLog = LogManager.GetLogger("ExceptionLog");

        private int memberId;
        public PatientDetails_UC patd;
        CreateModifyPatient_UC createPatientUC;
        String reportno = IVLVariables.LangResourceManager.GetString("No_of_Reports_Text", IVLVariables.LangResourceCultureInfo);
        public delegate void ManageClosed(string a, EventArgs e);
        public EventArgs e = null;
        public event ManageClosed thisClosed;
        bool isRegister = false;
        string patSearchMrnText = "";
        public bool pat_search = false;
        List<DateTime> visitDate = new List<DateTime>();
        Button newConsultation_btn;
        public delegate void VisitDoubleClick(string a, EventArgs e);
        public event VisitDoubleClick _visitDoubleClick;
        int image_count = 0;
        int newScrollValue = 0;
        #region paginationvariables
        private int CurrentPage = 1;
        int PagesCount;
        int pageRows;
        int NoofPageButton = 5;
        int NoofPageToBeShifted = 1;
        int CurrentPageDecementNumber = 2;
        int PageCountDecrementNumber = 4;
        #endregion
        List<int> ReportNumbers = new List<int>();
        List<int> ImageNumbers = new List<int>();
        string patFirstNameSearchText = "";
        string patLastNameSearchText = "";
        public int corrupted_count = 0;
        int genderSelectedIndx = 0;
        int currentRow = 0;
        int visitId = 0;
        Dictionary<string, object> searchDictionary;
        bool isTab = false;
        bool isEnter = false;
        bool isControlkey_clicked = false;
        AdvanceSerach_UC advanceUC;
        #endregion
        public EmrViewModel()
        {
            isReport = false;
            //_eventHandler = IVLEventHandler.getInstance();
            //_eventHandler.Register(_eventHandler.Back2Search, new NotificationHandler(Back2Search));
            //_eventHandler.Register(_eventHandler.ImageUrlToDb, new NotificationHandler(saveImageURlDb));
            //_eventHandler.Register(_eventHandler.SetLeftRightDetailsToDb, new NotificationHandler(setLeftRight2ImageDb));
            pageRows = Convert.ToInt32(IVLVariables.CurrentSettings.UserSettings._NoOfPatientsToBeSelected.val);//Assigns no of patients to be displayed
            PagesCount = Convert.ToInt32(Math.Ceiling(NewDataVariables._Repo.GetPatientCount() * 1.0 / pageRows));
            TemPatList = NewDataVariables._Repo.GetPageData<INTUSOFT.Data.NewDbModel.Patient>(pageRows, CurrentPage).ToList<INTUSOFT.Data.NewDbModel.Patient>();
            searchDictionary = new Dictionary<string, object>();
            t.Interval = 1000;
            advanceUC = AdvanceSerach_UC.getInstance();
            //advanceUC.advancesearchevent += advanceUC_advancesearchevent;
            cf = new CreatePatientDetails_form();// this line is added by sriram in order to create patient details dialog in constructor only
        }
    }

}
