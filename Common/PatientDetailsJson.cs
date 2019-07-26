using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO.Compression;
using System.IO;
using Common;
using System.Net;
using System.Net.Mail;
using Newtonsoft.Json;

namespace Common
{
    public class PatientDetailsJson
    {
        private static PatientDetailsJson _patientDetailsJson;
        public static PatientDetailsJson GetInstance()
        {
            if (_patientDetailsJson == null)
                _patientDetailsJson = new PatientDetailsJson();
            return _patientDetailsJson;
        }

        private PatientDetailsJson()
        {

        }
        string jsonPatientDetails;
        private string patientDetailsJson = "PatientDetails.json";

        public void CreatePatientJsonFile(List<string> list, string path)
        {
            string jsonPatientDetails;
            var json = JsonConvert.SerializeObject(list);
            jsonPatientDetails = json.ToString();
            System.IO.File.WriteAllText(path + Path.DirectorySeparatorChar + patientDetailsJson, jsonPatientDetails);

        }

    }
}

