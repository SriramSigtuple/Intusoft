using BaseViewModel;
using Cloud_Models.Models;
using IntuUploader;
using IntuUploader.Utilities;
using NLog;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IVLUploader.ViewModels
{
    /// <summary>
    /// Class which implements the check for internet connection by pinging to 8.8.8.8 of google
    /// </summary>
    public class UploadImagesViewModel : ViewBaseModel
    {
        UploadModel UploadModel;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        public UploadImagesViewModel(UploadModel uploadModel)
        {
            logger.Info("");

            UploadModel = uploadModel;
            //SetValue = new RelayCommand(param=> SetValueMethod(param));
            logger.Info("");

        }

        public ICommand SetValue
        {
            get;
            set;
        }


        private URL_Model loginURlModel;

        public URL_Model LoginURLModel
        {
            get { return loginURlModel; }
            set { loginURlModel = value; }
        }       

        public async Task<List<Response_CookieModel>> StartUpload(Cookie cookie)

        {
            logger.Info("");

            List<Response_CookieModel> responseList = new List<Response_CookieModel>();
            UploadModel.URL = UploadModel.URL_Model.GetUrl();

            for (int i = 0; i < UploadModel.images.Length; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();
                kvp.Add("relative_path", UploadModel.relative_path[i]);
                kvp.Add("image", new FileInfo(UploadModel.images[i]));
                kvp.Add("checksum", UploadModel.checksums[i]);
                kvp.Add("slide_id", UploadModel.slide_id);
                kvp.Add("upload_type", UploadModel.upload_type);
                responseList.Add( await GlobalVariables.RESTClientHelper.RestCall(UploadModel, cookie, kvp));
            }
            logger.Info("");

            return responseList;
        }


        public void SetValueMethod(object param)
        {
            //this.FileUploadStatus = 100;
        }

    }
}
