using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
namespace INTUSOFT.Configuration.AdvanceSettings
{
    [Serializable]
    public class CloudSettings
    {
        private static IVLControlProperties outboxPath = null;

        public IVLControlProperties OutboxPath
        {
            get { return outboxPath; }
            set { outboxPath = value; }
        }

        private static IVLControlProperties inboxPath = null;

        public IVLControlProperties InboxPath
        {
            get { return inboxPath; }
            set { inboxPath = value; }
        }
        private static IVLControlProperties cloudPath = null;

        public IVLControlProperties CloudPath
        {
            get { return cloudPath; }
            set { cloudPath = value; }
        }

        private static IVLControlProperties sentItemsPath = null;

        public IVLControlProperties SentItemsPath
        {
            get { return sentItemsPath; }
            set { sentItemsPath = value; }
        }

        private static IVLControlProperties activeDirPath = null;

        public IVLControlProperties ActiveDirPath
        {
            get { return activeDirPath; }
            set { activeDirPath = value; }
        }

        private static IVLControlProperties readPath = null;

        public IVLControlProperties ReadPath
        {
            get { return readPath; }
            set { readPath = value; }
        }


        private static IVLControlProperties processedPath = null;

        public IVLControlProperties ProcessedPath
        {
            get { return processedPath; }
            set { processedPath = value; }
        }

        private static IVLControlProperties loginPath = null;

        public IVLControlProperties LoginPath
        {
            get { return loginPath; }
            set { loginPath = value; }
        }

        private static IVLControlProperties createAnalysisPath = null;

        public IVLControlProperties CreateAnalysisPath
        {
            get { return createAnalysisPath; }
            set { createAnalysisPath = value; }
        }

        private static IVLControlProperties uploadPath = null;

        public IVLControlProperties UploadPath
        {
            get { return uploadPath; }
            set { uploadPath = value; }
        }

        private static IVLControlProperties startAnalysisPath = null;

        public IVLControlProperties StartAnalysisPath
        {
            get { return startAnalysisPath; }
            set { startAnalysisPath = value; }
        }
        private static  IVLControlProperties api_url = null;

        public IVLControlProperties API_URL
        {
            get { return api_url; }
            set { api_url = value; }
        }

        private static  IVLControlProperties api_analyses = null;

        public IVLControlProperties API_ANALYSES
        {
            get { return api_analyses; }
            set { api_analyses = value; }
        }
        private static  IVLControlProperties api_analyses_input = null;

        public IVLControlProperties API_ANALYSES_INPUT
        {
            get { return api_analyses_input; }
            set { api_analyses_input = value; }
        }

        private static IVLControlProperties api_login_url = null;

        public IVLControlProperties API_LOGIN_URL
        {
            get { return api_login_url; }
            set { api_login_url = value; }
        }
        private static IVLControlProperties api_status_url = null;

        public IVLControlProperties API_STATUS_URL
        {
            get { return api_status_url; }
            set { api_status_url = value; }
        }

        private static IVLControlProperties username = null;

        public IVLControlProperties Username 
        {
            get { return username; }
            set { username = value; }
        }

        private static IVLControlProperties password = null;

        public IVLControlProperties Password
        {
            get { return password; }
            set { password = value; }
        }

        private static IVLControlProperties inboxTimerInterval = null;

        public IVLControlProperties InboxTimerInterval
        {
            get { return inboxTimerInterval; }
            set { inboxTimerInterval = value; }
        }
        public CloudSettings()
        {
            API_URL = new IVLControlProperties();
            API_URL.name = "API_URL";
            API_URL.val = "https://ffeddb-mandara-api.sigtuple.com/mandara/api/";
            API_URL.type = "string";
            API_URL.control = "System.Windows.Forms.TextBox";
            API_URL.text = "API URL";
            API_URL.length = 100000;

            API_ANALYSES = new IVLControlProperties();
            API_ANALYSES.name = "api_analyses";
            API_ANALYSES.val = "analyses";
            API_ANALYSES.type = "string";
            API_ANALYSES.control = "System.Windows.Forms.TextBox";
            API_ANALYSES.text = "API ANALYSES";
            API_ANALYSES.length = 100000;

            API_ANALYSES_INPUT = new IVLControlProperties();
            API_ANALYSES_INPUT.name = "api_analyses_input";
            API_ANALYSES_INPUT.val = "input";
            API_ANALYSES_INPUT.type = "string";
            API_ANALYSES_INPUT.control = "System.Windows.Forms.TextBox";
            API_ANALYSES_INPUT.text = "API ANALYSES INPUT";
            API_ANALYSES_INPUT.length = 100000;


            API_LOGIN_URL = new IVLControlProperties();
            API_LOGIN_URL.name = "api_analyses_input";
            API_LOGIN_URL.val = "auth/signin";
            API_LOGIN_URL.type = "string";
            API_LOGIN_URL.control = "System.Windows.Forms.TextBox";
            API_LOGIN_URL.text = "API Login URL";
            API_LOGIN_URL.length = 100000;


           API_STATUS_URL = new IVLControlProperties();
           API_STATUS_URL.name = "api_status_url";
           API_STATUS_URL.val = "status";
           API_STATUS_URL.type = "string";
           API_STATUS_URL.control = "System.Windows.Forms.TextBox";
           API_STATUS_URL.text = "API STATUS URL";
           API_STATUS_URL.length = 100000;

            CloudPath = new IVLControlProperties();
            CloudPath.name = "cloudPath";
            CloudPath.type = "string";
            CloudPath.val = @"C:\IVLImageRepo\Cloud";
            CloudPath.control = "System.Windows.Forms.TextBox";
            CloudPath.text = "Cloud Path";

            OutboxPath = new IVLControlProperties();
            OutboxPath.name = "outboxPath";
            OutboxPath.type = "string";
            OutboxPath.val = "Outbox";
            OutboxPath.control = "System.Windows.Forms.TextBox";
            OutboxPath.text = "Outbox Text";

            InboxPath = new IVLControlProperties();
            InboxPath.name = "inboxPath";
            InboxPath.type = "string";
            InboxPath.val = "Inbox";
            InboxPath.control = "System.Windows.Forms.TextBox";
            InboxPath.text = "Inbox Text";

            ReadPath = new IVLControlProperties();
            ReadPath.name = "readPath";
            ReadPath.type = "string";
            ReadPath.val = "Read";
            ReadPath.control = "System.Windows.Forms.TextBox";
            ReadPath.text = "Read Text";

            ProcessedPath = new IVLControlProperties();
            ProcessedPath.name = "processedPath";
            ProcessedPath.type = "string";
            ProcessedPath.val = "Processed";
            ProcessedPath.control = "System.Windows.Forms.TextBox";
            ProcessedPath.text = "Processed Text";

            SentItemsPath = new IVLControlProperties();
            SentItemsPath.name = "sentItemsPath";
            SentItemsPath.type = "string";
            SentItemsPath.val = "Sent Items";
            SentItemsPath.control = "System.Windows.Forms.TextBox";
            SentItemsPath.text = "Sent Items Text";


            ActiveDirPath = new IVLControlProperties();
            ActiveDirPath.name = "activeDirPath";
            ActiveDirPath.type = "string";
            ActiveDirPath.val = "Active File Directory";
            ActiveDirPath.control = "System.Windows.Forms.TextBox";
            ActiveDirPath.text = "Active Dir Path";

            LoginPath = new IVLControlProperties();
            LoginPath.name = "LoginPath";
            LoginPath.type = "string";
            LoginPath.val = "Login";
            LoginPath.control = "System.Windows.Forms.TextBox";
            LoginPath.text = "Login Path";

            CreateAnalysisPath = new IVLControlProperties();
            CreateAnalysisPath.name = "CreateAnalysisPath";
            CreateAnalysisPath.type = "string";
            CreateAnalysisPath.val = "Create Analysis";
            CreateAnalysisPath.control = "System.Windows.Forms.TextBox";
            CreateAnalysisPath.text = "Create Analysis Path";

            UploadPath = new IVLControlProperties();
            UploadPath.name = "UploadPath";
            UploadPath.type = "string";
            UploadPath.val = "Upload";
            UploadPath.control = "System.Windows.Forms.TextBox";
            UploadPath.text = "Upload Path";

           StartAnalysisPath = new IVLControlProperties();
           StartAnalysisPath.name = "StartAnalysisPath";
           StartAnalysisPath.type = "string";
           StartAnalysisPath.val = "Start Analysis";
           StartAnalysisPath.control = "System.Windows.Forms.TextBox";
           StartAnalysisPath.text = "Start Analysis Path";



            Username = new IVLControlProperties();
            Username.name = "username";
            Username.type = "string";
            Username.val = "";
            Username.control = "System.Windows.Forms.TextBox";
            Username.text = "Username";

            Password = new IVLControlProperties();
            Password.name = "password";
            Password.type = "string";
            Password.val = "";
            Password.control = "System.Windows.Forms.TextBox";
            Password.text = "Password";

            InboxTimerInterval = new IVLControlProperties();
            InboxTimerInterval.name = "InboxTimerInterval";
            InboxTimerInterval.type = "double";
            InboxTimerInterval.val = "10";
            InboxTimerInterval.min = 0;
            InboxTimerInterval.max = 10000;
            InboxTimerInterval.control = "System.Windows.Forms.NumericUpDown";
            InboxTimerInterval.text = "Inbox Timer Interval";


        }
    }
}
