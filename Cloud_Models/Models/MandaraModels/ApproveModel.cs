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
        public string doctor_id = string.Empty;

        public ApproveModel()
        {
            MethodType = HttpMethod.Post;
        }
    }
}
