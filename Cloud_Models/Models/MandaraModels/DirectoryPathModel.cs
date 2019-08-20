using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Models.Models
{
    [Serializable]
  public class DirectoryPathModel
    {
        public  string outboxStr = "Outbox";
        public  string sentItemsStr = "Sent Items";
        public  string activefileDirStr = "Active File Directory";
        public  string loginDirStr = "Login";
        public  string createAnalysisDirStr = "Create Analysis";
        public  string uploadDirStr = "Upload";
        public  string startAnalysisDirStr = "Start Analysis";

        public  string inboxStr = "Inbox";
        public  string ReadStr = "Read";
        public  string ProcessedStr = "Processed";
        public  string CloudPath = @"C:\IVLImageRepo\Cloud";

        
    }
}
