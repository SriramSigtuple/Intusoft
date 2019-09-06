using BaseViewModel;
using Cloud_Models.Models;
using IntuUploader.Utilities;
using NLog;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IntuUploader.ViewModels
{
    /// <summary>
    /// Class which implements the check for internet connection by pinging to 8.8.8.8 of google
    /// </summary>
    public class GetAnalysisResultViewModel : ViewBaseModel
    {
        GetAnalysisResultModel getAnalysisResultModel;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        RESTClientHelper rESTClient;
        /// <summary>
        /// Constructor
        /// </summary>
        public GetAnalysisResultViewModel(GetAnalysisResultModel getAnalysisResultModel)
        {
            
            rESTClient = new RESTClientHelper();
            GetAnalysisResultModel = getAnalysisResultModel;
            //SetValue = new RelayCommand(param=> SetValueMethod(param));
            


        }

        public async Task<Response_CookieModel> GetAnalysisResult(Cookie cookie)
        {
            

            GetAnalysisResultModel.URL = GetAnalysisResultModel.URL_Model.GetUrl();

            Response_CookieModel jsonToken = await GlobalVariables.RESTClientHelper.RestCall(GetAnalysisResultModel, cookie, new Dictionary<string, object>());
            

            return jsonToken;
            //JObject GetAnalysisStatus_JObject = JObject.Parse(jsonToken.responseBody);
            //var cloudVal = JsonConvert.SerializeObject(cloudModel, Formatting.Indented);
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

        public GetAnalysisResultModel GetAnalysisResultModel
        { get => getAnalysisResultModel;
            set
            {
                getAnalysisResultModel = value;
                OnPropertyChanged("GetAnalysisResultModel");
            }
        }

        public void SetValueMethod(object param)
        {
            //this.FileUploadStatus = 100;
        }

    }
}
