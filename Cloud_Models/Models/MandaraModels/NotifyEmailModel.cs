using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Models.Models.MandaraModels
{
    public class NotifyEmailModel: BaseCloudModel
    {
        public NotifyEmailModel()
        {
            MethodType = HttpMethod.Post;
        }
    }
}
