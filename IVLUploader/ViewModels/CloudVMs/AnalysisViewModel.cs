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
    public class AnalysisViewModel : ViewBaseModel
    {
        BaseCloudModel AnalysisModel;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        RESTClientHelper rESTClient;
        /// <summary>
        /// Constructor
        /// </summary>
        public AnalysisViewModel(BaseCloudModel analysisModel)
        {
            

            AnalysisModel = analysisModel;
            rESTClient = new RESTClientHelper();
            //SetValue = new RelayCommand(param=> SetValueMethod(param));
            

        }

        public ICommand SetValue
        {
            get;
            set;
        }


        public async Task<Response_CookieModel> InitiateRestCall(Cookie cookie, System.Collections.Generic.Dictionary<string, object> dic)
        {
            

            AnalysisModel.Body = JsonConvert.SerializeObject(AnalysisModel);
            AnalysisModel.URL = AnalysisModel.URL_Model.GetUrl();
            Response_CookieModel jsonToken = await rESTClient.RestCall(AnalysisModel,cookie, dic);
            

            return jsonToken;
        }


        public void SetValueMethod(object param)
        {
            //this.FileUploadStatus = 100;
        }

    }
}
