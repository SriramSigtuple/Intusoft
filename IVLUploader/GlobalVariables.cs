using Cloud_Models.Models;
using IntuUploader.Utilities;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntuUploader
{
    public enum DirectoryEnum {OutboxDir,ActiveDir,LoginDir,CreateAnalysis,UploadDir,StartAnalysisDir,SentItemsDir,ProcessedDir,InboxDir,ReadDir };
    public static class GlobalVariables
    {
        public static RESTClientHelper RESTClientHelper;

        public static DirectoryPathModel CloudPaths;

        public static Logger eventLog = LogManager.GetLogger("EventLog");

        public static UploaderSettings UploaderSettings = null;

        public static bool isInternetPresent = false;

    }
}
