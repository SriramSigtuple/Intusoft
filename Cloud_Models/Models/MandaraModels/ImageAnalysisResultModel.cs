using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Models.Models
{
    [Serializable]
   public class ImageAnalysisResultModel
    {
        public string ImageName;
        public string QI_Result_DR;
        public string QI_Result_AMD;
        public string QI_Result_Glaucoma;
        public string Analysis_Result_DR;
        public string Analysis_Result_AMD;
        public string Analysis_Result_GLaucoma;

        public ImageAnalysisResultModel()
        {
        }


    }
}
