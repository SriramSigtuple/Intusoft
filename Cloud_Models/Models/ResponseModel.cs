using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Net.Http;

namespace Cloud_Models.Models
{
    public class Response_CookieModel
    {
        public Cookie Cookie;
        public string responseBody;
        public HttpStatusCode StatusCode;
        public Response_CookieModel()
        {

        }
    }
}
