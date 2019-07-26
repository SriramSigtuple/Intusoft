using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
namespace INTUSOFT.Configuration.AdvanceSettings
{
    [Serializable]
    public class EmailSettings
    {

        //public int adobePrintDelay = 20000;
        private static  IVLControlProperties emailReplyToList = null;

        public IVLControlProperties EmailReplyToList
        {
            get { return emailReplyToList; }
            set { emailReplyToList = value; }
        }

        private static  IVLControlProperties emailToList = null;

        public IVLControlProperties EmailToList
        {
            get { return emailToList; }
            set { emailToList = value; }
        }
        private static  IVLControlProperties emailCCList = null;

        public IVLControlProperties EmailCCList
        {
            get { return emailCCList; }
            set { emailCCList = value; }
        }
        private static  IVLControlProperties emailBCCList = null;

        public IVLControlProperties EmailBCCList
        {
            get { return emailBCCList; }
            set { emailBCCList = value; }
        }
        private static  IVLControlProperties showEmailWindow = null;

        public IVLControlProperties ShowEmailWindow
        {
            get { return showEmailWindow; }
            set { showEmailWindow = value; }
        }
        public EmailSettings()
        {
            EmailReplyToList = new IVLControlProperties();
            EmailReplyToList.name = "emailReplyToList";
            EmailReplyToList.val = "";
            EmailReplyToList.type = "string";
            EmailReplyToList.control = "System.Windows.Forms.TextBox";
            EmailReplyToList.text = "Email Reply To List";
            EmailReplyToList.length = 100000;

            EmailToList = new IVLControlProperties();
            EmailToList.name = "emailToList";
            EmailToList.val = "ivltelemed@gmail.com;";
            EmailToList.type = "string";
            EmailToList.control = "System.Windows.Forms.TextBox";
            EmailToList.text = "Email To List";
            EmailToList.length = 100000;

            EmailCCList = new IVLControlProperties();
            EmailCCList.name = "emailCCList";
            EmailCCList.val = "";
            EmailCCList.type = "string";
            EmailCCList.control = "System.Windows.Forms.TextBox";
            EmailCCList.text = "Email CC List";
            EmailCCList.length = 100000;

            EmailBCCList = new IVLControlProperties();
            EmailBCCList.name = "emailBCCList";
            EmailBCCList.val = "";
            EmailBCCList.type = "string";
            EmailBCCList.control = "System.Windows.Forms.TextBox";
            EmailBCCList.text = "Email BCC List";
            EmailBCCList.length = 100000;

            ShowEmailWindow = new IVLControlProperties();
            ShowEmailWindow.name = "showEmailWindow";
            ShowEmailWindow.val = "true";
            ShowEmailWindow.type = "bool";
            ShowEmailWindow.control = "System.Windows.Forms.RadioButton";
            ShowEmailWindow.text = "Show Email Window";
        }
    }
}
