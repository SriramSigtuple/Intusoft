using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Models.Models
{
    public class ApproveModel: BaseCloudModel
    {
        public string analysisId = string.Empty;
        public string reviewerId = string.Empty;
        public string reviewerStatus = "Pending";
       
        public ApproveModel()
        {
            MethodType = HttpMethod.Post;
            URL_Model.API_URL_Start_Point = "analyses";
            URL_Model.API_URL_End_Point = "review";
        }
    }
}
