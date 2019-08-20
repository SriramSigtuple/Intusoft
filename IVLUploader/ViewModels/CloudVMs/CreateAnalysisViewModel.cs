using BaseViewModel;
using Cloud_Models.Models;
using IntuUploader;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using IntuUploader.Utilities;
using NLog;

namespace IVLUploader.ViewModels
{
    /// <summary>
    /// Class which implements the check for internet connection by pinging to 8.8.8.8 of google
    /// </summary>
    public class CreateAnalysisViewModel : ViewBaseModel
    {
        CreateAnalysisModel CreateAnalysisModel;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        public CreateAnalysisViewModel(CreateAnalysisModel createAnalysisModel)
        {
            logger.Info("");

            CreateAnalysisModel = createAnalysisModel;
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

        public async Task<Response_CookieModel> StartCreateAnalysis(Cookie cookie)
        {
            logger.Info("");

            CreateAnalysisModel.Body = JsonConvert.SerializeObject(CreateAnalysisModel);
            CreateAnalysisModel.URL = CreateAnalysisModel.URL_Model.GetUrl();
            Response_CookieModel jsonToken = await GlobalVariables.RESTClientHelper.RestCall(CreateAnalysisModel,cookie, new System.Collections.Generic.Dictionary<string, object>());
            logger.Info("");

            return jsonToken;
        }


        public void SetValueMethod(object param)
        {
            //this.FileUploadStatus = 100;
        }

    }
}
