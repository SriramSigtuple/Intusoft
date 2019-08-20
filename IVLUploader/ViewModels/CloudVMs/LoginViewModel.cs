using BaseViewModel;
using Cloud_Models.Models;
using System.Windows.Input;
using IntuUploader.Utilities;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;
using IntuUploader;
using NLog;

namespace IVLUploader.ViewModels
{
    /// <summary>
    /// Class which implements the check for internet connection by pinging to 8.8.8.8 of google
    /// </summary>
    public class LoginViewModel : ViewBaseModel
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        RESTClientHelper RESTClientHelper;
        /// <summary>
        /// Constructor
        /// </summary>
        public LoginViewModel(LoginModel loginModel)
        {
            LoginModel = loginModel;
            //SetValue = new RelayCommand(param=> SetValueMethod(param));
        }

        public ICommand SetValue
        {
            get;
            set;
        }
        public async Task<Response_CookieModel> StartLogin()
        {
            logger.Info("");

            LoginModel.Body = JsonConvert.SerializeObject(LoginModel);
            LoginModel.URL = LoginModel.URL_Model.GetUrl();

            Response_CookieModel jsonToken = await GlobalVariables.RESTClientHelper.RestCall(LoginModel,new System.Net.Cookie(),new System.Collections.Generic.Dictionary<string, object>());
            logger.Info("");
            return jsonToken;
        }

        private LoginModel loginModel;

        public LoginModel LoginModel
        {
            get { return loginModel; }
            set {
                loginModel = value;
                OnPropertyChanged("LoginModel");
            }
        }       




        public void SetValueMethod(object param)
        {
            //this.FileUploadStatus = 100;
        }

    }
}
