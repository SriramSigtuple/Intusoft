using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Common.ValidatorDatas;
using System.Threading.Tasks;

namespace ReportUtils
{
    public class DataModel
    {
        private static DataModel dataModel;
        public string CurrentTemplate = "";
        public string[] VisitImageFiles;
        public string[,] ImageFileNamesFromTable;
        public string[,] ImageNamesFromTable;
        public bool[] isCDR;
        public string[] MaskSettingsArr;//MaskSettingsArr is string array created to handle changes in Mask colour.By Ashutosh 22-08-2017
        public string[] CameraSettingsArr;//CameraSettingsArr is string array created to handle changes in camera settings.By Ashutosh 31-08-2017
        public int NoOfImagesAllowed = 0;
        public string NoOfImagesAllowedText1 = string.Empty;
        public string NoOfImagesAllowedText2 = string.Empty;
        public string NoOfImagesAllowedHeader = string.Empty;
        public string Comments;
        public string _operator;
        public bool isFromCDR = false;
        public bool isFromAnnotation = false;
        public bool[] isannotated;
        public bool isDRReport = false;//Determines wheather the report is DR or normal.
        public int[] VisitImageIds;
        public int[] VisitImagesides;
        public string[] CurrentImgFiles;
        public string[] CurrentImageNames;
        public string[] CurrentImageMaskSettings;//CurrentImageMaskSettings is string array to handle changes in current image mask settings.By Ashutosh 22-08-2017
        public string[] CurrentImageCameraSettings;////CurrentImageCameraSettings is string array to handle changes in camera settings.By Ashutosh 31-08-2017
        public Dictionary<string, object> ReportData;
        public PatientDetailsForCommandLineArgs patientDetails;
        public EmailsData mailData = new EmailsData();
        public bool ContainsCmdArgs = false;
        public bool ShowEmailDialog = false;
        public DateTime visitDateTime;
        public bool Is2ImagesLS4ImagesPOR = false;
        public string appDir = string.Empty;
        public string DeviceID = string.Empty;
        private DataModel()
        {
            patientDetails = new PatientDetailsForCommandLineArgs();
        }

        public static DataModel GetInstance()
        {
            if (dataModel == null)
                dataModel = new DataModel();
            return dataModel;
        }
    }
}
