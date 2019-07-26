using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
   public class PatientDetailsForCommandLineArgs
    {

       public string FirstName = "";
       public string LastName = "";
       public string MRN = "";
       public string Age = "";
       public string Gender = "";
       public DateTime VisitDateTime = DateTime.Now;
       public string ReporteeName = "";
       public string MedHistory = "";
       public string HospitalName = ""; 
       public string Address1 = ""; 
       public string Address2 = "";
       public List<string> MaskSettings = new List<string>();
       public List<string> CameraSettings = new List<string>();
       public List<string> observationPaths = new List<string>();
       public List<bool> isAnnotatedList = new List<bool>();
       public List<bool> isCDRList = new List<bool>();
       public List<string> ImageNames = new List<string>();
       public List<int> ImageSideList = new List<int>();
       public List<int> ImageIDList = new List<int>();
       public PatientDetailsForCommandLineArgs()
       {

       }
       

    }
}
