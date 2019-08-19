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
            info.OSInfo = GetOSVersionInfo();

            info.IsAdobeInstalled = isAdobeInstallationCheck(info.OSInfo);

            string jsonValue = JsonConvert.SerializeObject(info);
            st.Write(jsonValue);
            st.Flush();
            st.Close();
            st.Dispose();
        }

        private static bool isAdobeInstallationCheck(string osInfo)
        {
            RegistryKey adobe = null;
            if (!osInfo.Contains("10"))
             adobe = Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Adobe");
            else
             adobe = Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Classes").OpenSubKey("acrobat").OpenSubKey("DefaultIcon");

            if (adobe != null)
            {
                RegistryKey acroRead = null;
                if (!osInfo.Contains("10"))
                {
                    acroRead = adobe.OpenSubKey("Acrobat Reader");
                    if (acroRead != null)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                    var names = adobe.GetValueNames();
                    string value = (string) adobe.GetValue("");
                    if (value.Contains("Reader 11.0\\Reader\\AcroRd32.exe"))
                        return true;
                    else
                        return false;
                }


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
