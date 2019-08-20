using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Models.Models
{
    [Serializable]
    public class GetAnalysisModel : BaseCloudModel
    {
        /// <summary>
        /// The analysis id is the value obtained from create analysis api response.
        /// </summary>
        public string analysis_id = string.Empty;
        public string analysis_status = string.Empty;
        public GetAnalysisModel()
        {
            MethodType = HttpMethod.Get;
        }


    }
}
