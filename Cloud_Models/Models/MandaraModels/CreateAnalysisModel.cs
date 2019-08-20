using System;
using System.Net.Http;

namespace Cloud_Models.Models
{
    [Serializable]
        public class CreateAnalysisModel : BaseCloudModel
    {
        /// <summary>
        /// The sample_id is the value which is the MRN (Medical Record of Number) of the patient.
        /// </summary>
        public string sample_id = string.Empty;

        /// <summary>
        /// The installation_id is the value obtained from the login api response value which indicates the installation site.
        /// </summary>
        public string installation_id = string.Empty;

        /// <summary>
        /// The product_id is the value obtained from the login api response value which indicates the type of product.
        /// </summary>
        public string product_id = string.Empty;

        /// <summary>
        /// The sample_desc is the value is additional details of the patient such as age, gender, etc.
        /// </summary>
        public string sample_desc = string.Empty;

        /// <summary>
        /// The age is the value is the patient's age optional field
        /// </summary>
        public string age = string.Empty;

        /// <summary>
        /// The gender is the value is the patient's gender optional field which takes value of M/F
        /// </summary>
        public string gender = string.Empty;

        /// <summary>
        /// ID of the visit from which the images have been uploaded.
        /// </summary>
        public int visitID = 0;

        /// <summary>
        /// ID of the Patient  
        /// </summary>
        public int PatientID = 0;
        public CreateAnalysisModel()
        {
            MethodType = HttpMethod.Post;
        
        }

    }
}
