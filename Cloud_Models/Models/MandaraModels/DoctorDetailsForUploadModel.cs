using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Models.Models
{

  [Serializable]
  public class DoctorDetailsForUploadModel
    {
        public int DoctorIndex;
        public string DoctorID;
        public string DoctorName;
        public string DoctorQualifications;
        public string HospitalName;
        public string Email;
        public DoctorDetailsForUploadModel()
        {
            DoctorID = string.Empty;
            DoctorName = string.Empty;
            DoctorQualifications = string.Empty;
            HospitalName = string.Empty;
            Email = string.Empty;
        }

    }
}
