using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Models.Models
{
    [Serializable]
    public class InitiateAnalysisModel : BaseCloudModel
    {
        public string id = string.Empty;
        public string status = string.Empty;
        public InitiateAnalysisModel()
        {
            URL_Model.API_URL_End_Point = "analyses/status";
            MethodType = HttpMethod.Post;

        }
    }
}
