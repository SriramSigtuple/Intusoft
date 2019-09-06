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
    public class GetStatusAnalysisViewModel : ViewBaseModel
    {
        GetAnalysisModel getAnalysisModel;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        RESTClientHelper rESTClient;
        /// <summary>
        /// Constructor
        /// </summary>
        public GetStatusAnalysisViewModel(GetAnalysisModel getAnalysisModel)
        {
            

            GetAnalysisModel = getAnalysisModel;
            rESTClient = new RESTClientHelper();
            //SetValue = new RelayCommand(param=> SetValueMethod(param));
            

        }

        public async Task<Response_CookieModel> GetStatus(Cookie cookie)
        {
            

            GetAnalysisModel.URL = GetAnalysisModel.URL_Model.GetUrl();

            Response_CookieModel jsonToken = await GlobalVariables.RESTClientHelper.RestCall(GetAnalysisModel, cookie, new Dictionary<string, object>());
            

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

        public GetAnalysisModel GetAnalysisModel
        { get => getAnalysisModel;
            set
            {
                getAnalysisModel = value;
                OnPropertyChanged("GetAnalysisModel");
            }
        }

        public void SetValueMethod(object param)
        {
            //this.FileUploadStatus = 100;
        }

    }
}
