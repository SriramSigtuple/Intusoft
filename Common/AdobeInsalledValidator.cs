using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Common
{
    [Serializable]
    public class AdobeAndOSInfo
    {
        public bool IsAdobeInstalled;
        public string OSInfo;
        public AdobeAndOSInfo()
        {
            IsAdobeInstalled = false;
            OSInfo = string.Empty;

        }
    }
}
