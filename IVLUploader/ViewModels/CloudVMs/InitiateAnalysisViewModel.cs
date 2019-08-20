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
    public class InitiateAnalysisViewModel : ViewBaseModel
    {
        InitiateAnalysisModel initiateAnalysisModel;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        public InitiateAnalysisViewModel(InitiateAnalysisModel intiateAnalysisModel)
        {
            logger.Info("");

            InitiateAnalysisModel = intiateAnalysisModel;
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

        public InitiateAnalysisModel InitiateAnalysisModel { get => initiateAnalysisModel;
            set
            {
                initiateAnalysisModel = value;
                OnPropertyChanged("InitiateAnalysisModel");
            }
        }
        public async Task<Response_CookieModel> InitiateAnalysis(Cookie cookie)
        {
            logger.Info("");

            InitiateAnalysisModel.Body = JsonConvert.SerializeObject(InitiateAnalysisModel);
            InitiateAnalysisModel.URL = InitiateAnalysisModel.URL_Model.GetUrl();

            Response_CookieModel jsonToken = await GlobalVariables.RESTClientHelper.RestCall(InitiateAnalysisModel, cookie, new System.Collections.Generic.Dictionary<string, object>());
            logger.Info("");

            return jsonToken;
        }
        public void SetValueMethod(object param)
        {
            //this.FileUploadStatus = 100;
        }

    }
}
