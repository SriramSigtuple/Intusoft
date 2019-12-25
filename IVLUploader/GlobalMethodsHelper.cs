using REST_Helper.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntuUploader
{
 public static class GlobalMethods
    {
        

        //public static FileInfo[] GetActiveFileNames()
        //{
        //    FileInfo[] fileInfos = new DirectoryInfo(GlobalVariables.CloudPath + Path.DirectorySeparatorChar + GlobalVariables.activefileDirStr).GetFiles();
        //    return fileInfos;
        //}

        //public static string GetDirPath(DirectoryEnum directoryEnum)
        //{
        //    var dirName = string.Empty;
        //    switch(directoryEnum)
        //    {
        //        case DirectoryEnum.OutboxDir:
        //            dirName = GlobalVariables.CloudPaths.outboxStr;
        //            break;
        //        case DirectoryEnum.ActiveDir:
        //            dirName = GlobalVariables.CloudPaths.activefileDirStr;
        //            break;
        //        case DirectoryEnum.LoginDir:
        //            dirName = Path.Combine(GlobalVariables.CloudPaths.activefileDirStr, GlobalVariables.CloudPaths.loginDirStr);
        //            break;
        //        case DirectoryEnum.CreateAnalysis:
        //            dirName = Path.Combine(GlobalVariables.CloudPaths.activefileDirStr, GlobalVariables.CloudPaths.createAnalysisDirStr);
        //            break;
        //        case DirectoryEnum.UploadDir:
        //            dirName = Path.Combine(GlobalVariables.CloudPaths.activefileDirStr, GlobalVariables.CloudPaths.uploadDirStr);
        //            break;
        //        case DirectoryEnum.StartAnalysisDir:
        //            dirName = Path.Combine(GlobalVariables.CloudPaths.activefileDirStr, GlobalVariables.CloudPaths.startAnalysisDirStr);
        //            break;
        //        case DirectoryEnum.SentItemsDir:
        //            dirName = GlobalVariables.CloudPaths.sentItemsStr;
        //            break;
        //        case DirectoryEnum.ProcessedDir:
        //            dirName = GlobalVariables.CloudPaths.ProcessedStr;
        //            break;
        //        case DirectoryEnum.InboxDir:
        //            dirName = GlobalVariables.CloudPaths.inboxStr;
        //            break;
        //        case DirectoryEnum.ReadDir:
        //            dirName = GlobalVariables.CloudPaths.ReadStr;
        //            break;

        //    }
        //    return Path.Combine(GlobalVariables.CloudPaths.CloudPath, dirName);

        //}

        public static string GetDirPath(DirectoryEnum directoryEnum, AnalysisType analysisType)
        {
            var dirName = string.Empty;
            var analysisName = analysisType.ToString("g");
            switch (directoryEnum)
            {
                case DirectoryEnum.OutboxDir:
                    dirName = Path.Combine(analysisName, GlobalVariables.CloudPaths.outboxStr);
                    break;
                case DirectoryEnum.ActiveDir:
                    dirName = Path.Combine(analysisName, GlobalVariables.CloudPaths.activefileDirStr);
                    break;
                case DirectoryEnum.SentItemsDir:
                    dirName = Path.Combine(analysisName, GlobalVariables.CloudPaths.sentItemsStr);
                    break;
                case DirectoryEnum.ProcessedDir:
                    dirName = Path.Combine(analysisName, GlobalVariables.CloudPaths.ProcessedStr);
                    break;
                case DirectoryEnum.InboxDir:
                    dirName = Path.Combine(analysisName, GlobalVariables.CloudPaths.inboxStr);
                    break;
                case DirectoryEnum.ReadDir:
                    dirName = Path.Combine(analysisName,GlobalVariables.CloudPaths.ReadStr);
                    break;
                case DirectoryEnum.CloudImagesDir:
                    dirName = Path.Combine(analysisName, GlobalVariables.CloudPaths.CloudImagesStr);
                    break;
                    //case DirectoryEnum.LoginDir:
                    //    dirName = Path.Combine(CurrentSettings.CloudSettings.ActiveDirPath.val, CurrentSettings.CloudSettings.LoginPath.val);
                    //    break;
                    //case DirectoryEnum.CreateAnalysis:
                    //    dirName = Path.Combine(CurrentSettings.CloudSettings.ActiveDirPath.val, CurrentSettings.CloudSettings.CreateAnalysisPath.val);
                    //    break;
                    //case DirectoryEnum.UploadDir:
                    //    dirName = Path.Combine(CurrentSettings.CloudSettings.ActiveDirPath.val, CurrentSettings.CloudSettings.UploadPath.val);
                    //    break;
                    //case DirectoryEnum.StartAnalysisDir:
                    //    dirName = Path.Combine(CurrentSettings.CloudSettings.ActiveDirPath.val, CurrentSettings.CloudSettings.StartAnalysisPath.val);
                    //    break;
            }
            return Path.Combine(GlobalVariables.CloudPaths.CloudPath, dirName);

        }
    }
}
