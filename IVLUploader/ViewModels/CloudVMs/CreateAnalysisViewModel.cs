using BaseViewModel;
using Cloud_Models.Models;
using REST_Helper.Utilities;
using Newtonsoft.Json;
using NLog;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IntuUploader.ViewModels
{
    /// <summary>
    /// Class which implements the check for internet connection by pinging to 8.8.8.8 of google
    /// </summary>
    public class CreateAnalysisViewModel : ViewBaseModel
    {
        CreateAnalysisModel CreateAnalysisModel;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        RESTClientHelper rESTClient;
        /// <summary>
        /// Constructor
        /// </summary>
        public CreateAnalysisViewModel(CreateAnalysisModel createAnalysisModel)
        {
            

            CreateAnalysisModel = createAnalysisModel;
            rESTClient = new RESTClientHelper();
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

        public async Task<Response_CookieModel> StartCreateAnalysis(Cookie cookie)
        {
            

            CreateAnalysisModel.Body = JsonConvert.SerializeObject(CreateAnalysisModel);
            CreateAnalysisModel.URL = CreateAnalysisModel.URL_Model.GetUrl();
            Response_CookieModel jsonToken = await rESTClient.RestCall(CreateAnalysisModel,cookie, new System.Collections.Generic.Dictionary<string, object>());
            

            return jsonToken;
        }


        public void SetValueMethod(object param)
        {
            //this.FileUploadStatus = 100;
        }

    }
}
