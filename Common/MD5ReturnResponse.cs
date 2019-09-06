using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum MD5responseStatusEnum  {Failure, Success}
    public class MD5ReturnResponse
    {
        public string responseMessage = string.Empty;
        public MD5responseStatusEnum status = MD5responseStatusEnum.Failure;
        
    }
}
