using System;
using System.IO;
using Newtonsoft.Json;

namespace INTUSOFT.Desktop
{
    [Serializable]
    public class Constants
    {
        public string frameworkVersion = "Microsoft .NET Framework";
        public string adobeReaderVersion = "Adobe Reader XI (11.0.10)";
        public string flashProgrammerVersion = "Renesas Electronics Corporation (HmseUsb) RenesasUSB";
        public string mysqlVersion = "MySQL Server 5.7";
        public string servicePackVersion = "Service Pack 1";
        public string cameraVersionText = "(10/16/2019 1.0.0.15729)";
        public string vcredistributable2015 = "Microsoft Visual C++ 2015 Redistributable";
        public string vcredistributable2013 = "Microsoft Visual C++ 2013 Redistributable";
        public string cameraDriverVersion = @"Windows Driver Package - Hangzhou ToupTek Photonics Co., ";
        public string boardDriver = "INTUVISION LABS Pvt. Ltd. INTUCAM-45 FUNDUS CAMERA";
        public string windowsVersion10 = "Windows 10";
        public string windowsVersion8 = "Windows 8";
        public string sqlServiceText = "57";
        public int prerquisitesCount = 9;

        public string SoftwareReleaseDate = "";

        public Constants()
        {
            
            
        }

    }
}
