using System;
using System.Collections.Generic;
using System.Linq;
using ReportUtils;
using System.Text;
using System.Threading.Tasks;

namespace IVLReport
{
 public  class Observations
    {
     public string imageFile = "";
     public string imageName = "";

    }
 public class KVData
 {
     public string Key;
     public object Value;
     public KVData()
     {

     }
     public KVData(string key, object val)
     {
         Key = key;
         Value = val;
     }
 }
 public class JsonReportModel
 {
     public string comments = "";
     public string doctor = "";
     public string rightEyeObs = "";
     public string leftEyeObs = "";
     public string MedHistory = "";
     public List<KVData> reportValues;
     public LayoutDetails.PageOrientation currentOrientation;
     public List<Observations> reportDetails;
     public JsonReportModel()
     {
         reportDetails = new List<Observations>();
         reportValues = new List<KVData>();
     }
 }

}
