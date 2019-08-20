using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Models.Models
{
    [Serializable]
    public class UploaderModel
    {
        public string username = "ravi";
        public string password = "31jC4Smj";
        public string device_id = "intucam_1";
        public static string API_URL = "https://ffeddb-mandara-api.sigtuple.com/mandara/api/";
        public string LoginURL = "https://ffeddb-mandara-api.sigtuple.com/mandara/api/auth/signin"; // URL used to get login in to mandara cloud
        public string CreateAnalysisURL = "https://ffeddb-mandara-api.sigtuple.com/mandara/api/analyses"; // URL used to create a instance of analysis
        public string UploadURL = "https://ffeddb-mandara-api.sigtuple.com/mandara/api/analyses/5d441f524454480006bd4e69/input"; // Upload images to a analysis ID 
        public string StartAnalysisURL = "https://ffeddb-mandara-api.sigtuple.com/mandara/api/analyses";// URL used to start the analysis
        public string GetAnalysisStatusURL = "https://ffeddb-mandara-api.sigtuple.com/mandara/api/analyses/5d441f524454480006bd4e69/"; // URL used to get the analysis of a particular analysis using analysis ID
                                                                                                                                       //public string ServerStatusURL = "http://chironapp.chironx.cloud/api/chironx/status";
        public string UploadDirectoryPath = "";
        //public string HardwareID = "02-1701-0014";

        public UploaderModel()
        {

        }
    }

}
