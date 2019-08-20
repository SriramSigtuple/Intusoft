using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Models.Models
{
    public class LoginResponseModel
    {
        public message message;
    }
  public class message
    {
        public string env = string.Empty;
        public string installation_id = string.Empty;
        public List<product> products;
    }
    public class product
    {
        public string product_id = string.Empty;
        public string product_name = string.Empty;
        public string ctgy = string.Empty;
        public string sub_ctgy = string.Empty;
    }
}
