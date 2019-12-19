using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Models.Models.MandaraModels
{
    public class ApproveModel: BaseCloudModel
    {
        public string analysis_id = string.Empty;
        public string reviewer_id = string.Empty;
        public string reviewer_status = "Pending";
        public string review_string = "review";
        public ApproveModel()
        {
            MethodType = HttpMethod.Post;
        }
    }
}
