using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Models.Models
{
    public class RequestModel
    {
        public int status = 0;
        public string URL= string.Empty;
        public HttpMethod MethodType;
        public string Headers = string.Empty;
        public string Body = string.Empty;
        public string ContentType = string.Empty;
        public RequestModel()
        {

        }
    }
}
