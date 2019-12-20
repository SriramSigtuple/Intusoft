using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Models.Models
{
    public class NotifyEmailModel: BaseCloudModel
    {
        public string analysis_id = string.Empty;
        public string product_id = string.Empty;
        public string product_unique_id = string.Empty;
        public string recepients = string.Empty;
        public string notification_string = @"notification/email?";
        public const string type = "assign";
        public string sub_category = "fundus";
        public NotifyEmailModel()
        {
            MethodType = HttpMethod.Get;
            URL_Model.API_URL_Start_Point = "analyses";
            URL_Model.API_URL_Mid_Point = notification_string;
        }
    }
}
