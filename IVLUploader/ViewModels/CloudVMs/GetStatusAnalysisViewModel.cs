using BaseViewModel;
using Cloud_Models.Models;
using IntuUploader;
using System.Collections.Generic;
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
    public class GetStatusAnalysisViewModel : ViewBaseModel
    {
        GetAnalysisModel getAnalysisModel;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        public GetStatusAnalysisViewModel(GetAnalysisModel getAnalysisModel)
        {
            logger.Info("");

            GetAnalysisModel = getAnalysisModel;
            //SetValue = new RelayCommand(param=> SetValueMethod(param));
            logger.Info("");

        }

        public async Task<Response_CookieModel> GetStatus(Cookie cookie)
        {
            logger.Info("");

            GetAnalysisModel.URL = GetAnalysisModel.URL_Model.GetUrl();

            Response_CookieModel jsonToken = await GlobalVariables.RESTClientHelper.RestCall(GetAnalysisModel, cookie, new Dictionary<string, object>());
            logger.Info("");

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
