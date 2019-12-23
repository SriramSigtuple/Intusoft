using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Models.Models
{
    [Serializable]
   public class InboxAnalysisStatusModel
    {
        public List<ImageAnalysisResultModel> RightEyeDetails;
        public List<ImageAnalysisResultModel> LeftEyeDetails;

        public string Status;
        public string FailureMessage;
        public string RightAIImpressionsDR;
        public string RightAIImpressionsAMD;
        public string RightAIImpressionsGlaucoma;
        public string LeftAIImpressionsDR;
        public string LeftAIImpressionsAMD;
        public string LeftAIImpressionsGlaucoma;
        public string Reject_Message;
        public HttpStatusCode StatusCode;
        public Uri ReportUri;
        public int visitID;
        public int patientID;
        public int reportID;
        public int cloudID;
        public InboxAnalysisStatusModel()
        {
            RightEyeDetails = new List<ImageAnalysisResultModel>();
            LeftEyeDetails = new List<ImageAnalysisResultModel>();
        }


    }
}
