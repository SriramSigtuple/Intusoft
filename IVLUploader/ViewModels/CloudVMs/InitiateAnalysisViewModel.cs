using BaseViewModel;
using Cloud_Models.Models;
using IntuUploader.Utilities;
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
    public class InitiateAnalysisViewModel : ViewBaseModel
    {
        InitiateAnalysisModel initiateAnalysisModel;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        RESTClientHelper rESTClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public InitiateAnalysisViewModel(InitiateAnalysisModel intiateAnalysisModel)
        {
            
            rESTClient = new RESTClientHelper();
            InitiateAnalysisModel = intiateAnalysisModel;
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

        public InitiateAnalysisModel InitiateAnalysisModel { get => initiateAnalysisModel;
            set
            {
                initiateAnalysisModel = value;
                OnPropertyChanged("InitiateAnalysisModel");
            }
        }
        public async Task<Response_CookieModel> InitiateAnalysis(Cookie cookie)
        {
            

            InitiateAnalysisModel.Body = JsonConvert.SerializeObject(InitiateAnalysisModel);
            InitiateAnalysisModel.URL = InitiateAnalysisModel.URL_Model.GetUrl();

            Response_CookieModel jsonToken = await GlobalVariables.RESTClientHelper.RestCall(InitiateAnalysisModel, cookie, new System.Collections.Generic.Dictionary<string, object>());
            

            return jsonToken;
        }
        public void SetValueMethod(object param)
        {
            //this.FileUploadStatus = 100;
        }

    }
}
