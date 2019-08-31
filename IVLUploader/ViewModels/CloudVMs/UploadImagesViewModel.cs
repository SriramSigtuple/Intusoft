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
        RESTClientHelper RESTClientHelper;
        /// <summary>
        /// Constructor
        /// </summary>
        public UploadImagesViewModel(UploadModel uploadModel)
        {
            
             RESTClientHelper = new RESTClientHelper();

            UploadModel = uploadModel;
            //SetValue = new RelayCommand(param=> SetValueMethod(param));
            

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

        public async Task<Response_CookieModel> StartUpload(Cookie cookie, Dictionary<string,object> keyValuePairs)

        {
            

            UploadModel.URL = UploadModel.URL_Model.GetUrl();

            Response_CookieModel response =  await GlobalVariables.RESTClientHelper.RestCall(UploadModel, cookie, keyValuePairs);

           
            

            return response;
        }


        public void SetValueMethod(object param)
        {
            //this.FileUploadStatus = 100;
        }

    }
}
