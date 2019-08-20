using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Models.Models
{
   public class BaseCloudModel
    {
        /// <summary>
        /// The URL has the value of the login url to the cloud.
        /// </summary>

        public URL_Model URL_Model = new URL_Model
        {
            API_URL = string.Empty,
            API_URL_Start_Point = "",
            API_URL_Mid_Point = "",
            API_URL_End_Point = ""
        };
        /// <summary>
        /// The url value
        /// </summary>
        public string URL = string.Empty;

        public string Body = string.Empty;

        public string ContentType = "application/json";

        public HttpMethod MethodType;

        public string BodyMessageType = "raw";


        public Boolean CompletedStatus = false;

    }
}
