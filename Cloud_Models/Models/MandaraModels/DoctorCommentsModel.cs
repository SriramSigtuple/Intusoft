using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Models.Models.MandaraModels
{
    public class DoctorCommentsModel: BaseCloudModel
    {

        public DoctorCommentsModel()
        {
            MethodType = System.Net.Http.HttpMethod.Get;
            URL_Model.API_URL_Start_Point = "aggrcomments";
        }
    }
}
