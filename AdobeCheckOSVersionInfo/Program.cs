using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Microsoft.VisualBasic.Devices;
using System.IO;
using Newtonsoft.Json;
using Common;
namespace AdobeCheckOSVersionInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamWriter st = new StreamWriter("AdobeOsInfo.json", false);
            
            AdobeAndOSInfo info = new AdobeAndOSInfo();

            info.IsAdobeInstalled = isAdobeInstallationCheck();
            info.OSInfo = GetOSVersionInfo();

            string jsonValue = JsonConvert.SerializeObject(info);
            st.Write(jsonValue);
            st.Flush();
            st.Close();
            st.Dispose();
        }

        private static bool isAdobeInstallationCheck()
        {
            RegistryKey adobe = Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Adobe");
            if (adobe != null)
            {
                RegistryKey acroRead = adobe.OpenSubKey("Acrobat Reader");
                if (acroRead != null)
                {
                    return true;
                }
                else
                    return false;
            }
            else
            {
                return false;
                ;
            }
        }

        private static string GetOSVersionInfo()
        {
            return new ComputerInfo().OSFullName;
        }
    }

}
