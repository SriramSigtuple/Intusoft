using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Data;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Runtime.InteropServices;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using INTUSOFT.Desktop.Properties;
using INTUSOFT.Data.Enumdetails;
using INTUSOFT.Data.NewDbModel;
using INTUSOFT.EventHandler;
using INTUSOFT.Data.Repository;
using INTUSOFT.Data.Extension;
using INTUSOFT.Custom.Controls;
using Microsoft.Win32;
using INTUSOFT.Imaging;
using Common;
using INTUSOFT.Desktop.Forms;
using NLog;
using NLog.Config;
using NLog.Targets;

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
        int MRNIndex = 0;
        int firstNameIndex = 1;
        int secondNameIndex = 2;
        int ageIndex = 3;
        int genderIndex = 4;
        int selectedVisitIndex = 0;
        System.Diagnostics.Stopwatch stW;
        public delegate void ReportDelegate();
        public event ReportDelegate reportViewEvent;
        List<INTUSOFT.Data.NewDbModel.Patient> TemPatList;
        int prevCntPatCnt = 0;
        bool isUpdate = false;
        Logger ExceptionLog = LogManager.GetLogger("ExceptionLog");

        /// <summary>
        /// Variable to store error message
        /// </summary>
        private string errorMessage;
        /// <summary>
        /// Member id
        /// </summary>
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
