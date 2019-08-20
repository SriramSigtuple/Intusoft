using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Cloud_Models.Models
{
    [Serializable]
  public class UploadModel : BaseCloudModel
    {

        /// <summary>
        /// image is the binary data of the image to be uploaded to the cloud.
        /// </summary>
        public string[] images;

        /// <summary>
        /// slide_id is the MRN value which is same as the sample_id used during the create analysis.
        /// </summary>
        public string slide_id = string.Empty;

        /// <summary>
        /// This is a fixed value of folder.
        /// </summary>
        public string upload_type = "folder";

        /// <summary>
        /// relative path of the image which indicates whether its a left or right eye the value is for eg :slide_id/eyetype/filename
        /// </summary>
        public string[] relative_path ;

        /// <summary>
        /// The checksum values of the image file to indicates which is not tampered with while uploading.
        /// </summary>
        public string[] checksums ;

        /// <summary>
        /// Array which holds values of left or right side of eye
        /// </summary>
        public string[] eyeSideArr;

        /// <summary>
        /// The analysis id is the value obtained from create analysis api response.
        /// </summary>
        public string analysis_id = string.Empty;

        public UploadModel()
        {
            URL_Model.API_URL_Start_Point = "analyses";
            URL_Model.API_URL_Mid_Point = analysis_id;
            URL_Model.API_URL_End_Point = "input";
            MethodType = HttpMethod.Post;
            BodyMessageType = "FormData";


        }
    }
}
