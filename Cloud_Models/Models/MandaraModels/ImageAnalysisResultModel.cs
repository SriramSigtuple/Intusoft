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
        public string QI_Result;
        public string Analysis_Result;

        public ImageAnalysisResultModel()
        {
        }


    }
}
