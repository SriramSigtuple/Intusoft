using BaseViewModel;
using Cloud_Models.Models;
using IntuUploader.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.Generic;
using System.Web;
using Cloud_Models;
using System.Diagnostics;
using System.Threading;
using System.Collections.ObjectModel;
using IntuUploader;
using NLog;

namespace IVLUploader.ViewModels
{
    public class MainWindowViewModel:ViewBaseModel
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private InternetCheckViewModel internetCheckViewModel;
        private const string serverNotRunning = "Cloud Server is not running";
       private const string serverRunning = "Cloud Server is running";
        OutboxViewModel OutboxViewModel;
        SentItemsViewModel SentItemsViewModel;
        private LogginVM logginVM;
        public  MainWindowViewModel()
       {
            logger.Info("");
            //logger.
            LogginVM = LogginVM.GetLogginVM();
            GlobalVariables.RESTClientHelper = RESTClientHelper.GetInstance();


            if (File.Exists("DirectoryPath.json"))
            {
                StreamReader st = new StreamReader("DirectoryPath.json");

                if ((GlobalVariables.CloudPaths = JsonConvert.DeserializeObject<DirectoryPathModel>(st.ReadToEnd())) == null)
                    GlobalVariables.CloudPaths = new DirectoryPathModel();

                st.Close();
            }
            else
                GlobalVariables.CloudPaths = new DirectoryPathModel();

           
            InternetCheckViewModel = InternetCheckViewModel.GetInstance();

            OutboxViewModel = OutboxViewModel.GetInstance();

            SentItemsViewModel = SentItemsViewModel.GetInstance();

            logger.Info("");


        }

        //async void Login()
        //{
        //    RESTClientHelper rESTClient = RESTClientHelper.GetInstance();
        //    Stopwatch st = new Stopwatch();
        //    st.Start();
        //    CloudModel cloudModel = new CloudModel();
        //    cloudModel.LoginModel.URL = "https://ffeddb-mandara-api.sigtuple.com/mandara/api/auth/signin";
        //    cloudModel.LoginModel.BodyMessageType = "raw";
        //    cloudModel.LoginModel.MethodType = HttpMethod.Post;
        //    cloudModel.LoginModel.username = "ravi";
        //    cloudModel.LoginModel.password = "ff2a5d1d2612dff7ab7749c1f6f142e0";
        //    cloudModel.LoginModel.device_id = "intucam_1";

        //    Dictionary<string, object> kvp = new Dictionary<string, object>();
        //    cloudModel.LoginModel.Body = JsonConvert.SerializeObject(cloudModel.LoginModel);
        //    Response_CookieModel jsonToken =  await rESTClient.RestCall(cloudModel.LoginModel, new System.Net.Cookie("cookie",""), new System.Collections.Generic.Dictionary<string, object>());
        //    cloudModel.LoginCookie = jsonToken.Cookie;
        //    if (cloudModel.LoginCookie != null)
        //        cloudModel.LoginModel.CompletedStatus = true;
        //    JObject Login_JObject = JObject.Parse(jsonToken.responseBody);
        //    LoginResponseModel reponse = new LoginResponseModel();
        //    reponse.message = new message();
        //    reponse.message.installation_id = (string) Login_JObject["message"]["installation_id"];
        //    var product_ID = (string)  Login_JObject["message"]["products"][1]["product_id"];


        //    cloudModel.CreateAnalysisModel.URL = "https://ffeddb-mandara-api.sigtuple.com/mandara/api/analyses";
        //    cloudModel.CreateAnalysisModel.BodyMessageType = "raw";
        //    cloudModel.CreateAnalysisModel.MethodType = HttpMethod.Post;
        //    cloudModel.CreateAnalysisModel.installation_id = (string)Login_JObject["message"]["installation_id"];
        //    cloudModel.CreateAnalysisModel.product_id = (string)Login_JObject["message"]["products"][1]["product_id"];
        //    cloudModel.CreateAnalysisModel.sample_id = "12345";
        //    cloudModel.CreateAnalysisModel.age = "25";
        //    cloudModel.CreateAnalysisModel.gender = "M";
        //    cloudModel.CreateAnalysisModel.sample_desc = "25yrs, M";



        //    cloudModel.CreateAnalysisModel.Body = JsonConvert.SerializeObject(cloudModel.CreateAnalysisModel);
        //     jsonToken = await rESTClient.RestCall(cloudModel.CreateAnalysisModel, cloudModel.LoginCookie , new System.Collections.Generic.Dictionary<string, object>());
        //    JObject createAnalysis_JObject = JObject.Parse(jsonToken.responseBody);
        //    // //JObject jp = token.Values()[0]<JObject>();
        //    //JToken jo = JObject.Parse(jsonToken.responseBody).Children().Values().ToList()[0];
        //    // string vs = jo.Value<string>();

        //    cloudModel.UploadModel.analysis_id = (string)createAnalysis_JObject["analysis_id"];
        //    cloudModel.UploadModel.relative_path = new string[] { cloudModel.CreateAnalysisModel.sample_id + Path.DirectorySeparatorChar + "LE" + Path.DirectorySeparatorChar + "UnsharpMask.jpg" };
        //       cloudModel.UploadModel.images = new string[] { @"C:\Users\Admin\Desktop\pptImages\UnsharpMask.jpg" };
        //       cloudModel.UploadModel.checksums = new string[] { (new FileInfo(@"C:\Users\Admin\Desktop\pptImages\UnsharpMask.jpg")).GetMd5Hash() };
        //       cloudModel.UploadModel.slide_id = cloudModel.CreateAnalysisModel.sample_id;


        //    cloudModel.UploadModel.URL_Model.API_URL = "https://ffeddb-mandara-api.sigtuple.com/mandara/api/";
        //    cloudModel.UploadModel.URL_Model.API_URL_Mid_Point = (string)createAnalysis_JObject["analysis_id"];
        //    cloudModel.UploadModel.URL = cloudModel.UploadModel.URL_Model.GetUrl()+"/";
        //    kvp.Clear();
        //    kvp.Add("relative_path", cloudModel.CreateAnalysisModel.sample_id +"/" + "LE" + "/" + "UnsharpMask.jpg");
        //    kvp.Add("image", new FileInfo( @"C:\Users\Admin\Desktop\pptImages\UnsharpMask.jpg" ));
        //    kvp.Add("checksum",  (new FileInfo(@"C:\Users\Admin\Desktop\pptImages\UnsharpMask.jpg")).GetMd5Hash() );
        //    kvp.Add("slide_id", cloudModel.CreateAnalysisModel.sample_id);
        //    kvp.Add("upload_type", cloudModel.UploadModel.upload_type);
        //    jsonToken =  await rESTClient.RestCall(cloudModel.UploadModel, jsonToken.Cookie, kvp);


        //    cloudModel.InitiateAnalysisModel.id = cloudModel.UploadModel.analysis_id;
        //    cloudModel.InitiateAnalysisModel.status = "initialized";

        //    cloudModel.InitiateAnalysisModel.URL_Model.API_URL = "https://ffeddb-mandara-api.sigtuple.com/mandara/api";
        //    cloudModel.InitiateAnalysisModel.URL = cloudModel.InitiateAnalysisModel.URL_Model.GetUrl();
        //    cloudModel.InitiateAnalysisModel.Body =   JsonConvert.SerializeObject(cloudModel.InitiateAnalysisModel);
        //    jsonToken = await rESTClient.RestCall(cloudModel.InitiateAnalysisModel, cloudModel.LoginCookie, new Dictionary<string, object>());

        //    JObject initiateAnalysis_JObject = JObject.Parse(jsonToken.responseBody);

        //    cloudModel.GetAnalysisModel.analysis_id = cloudModel.UploadModel.analysis_id;
        //   cloudModel.GetAnalysisModel.URL_Model.API_URL = "https://ffeddb-mandara-api.sigtuple.com/mandara/api";
        //   cloudModel.GetAnalysisModel.URL_Model.API_URL_End_Point = cloudModel.GetAnalysisModel.analysis_id;
        //    cloudModel.GetAnalysisModel.URL = cloudModel.GetAnalysisModel.URL_Model.GetUrl();
        //    jsonToken = await rESTClient.RestCall(cloudModel.GetAnalysisModel, cloudModel.LoginCookie, new Dictionary<string, object>());
        //    JObject GetAnalysisStatus_JObject = JObject.Parse(jsonToken.responseBody);
        //    var cloudVal = JsonConvert.SerializeObject(cloudModel, Formatting.Indented);
        //    File.WriteAllText(string.Format("analysisFile_{0}.txt", DateTime.Now.ToString("yyyymmddHHMMssfff")),cloudVal);
        //    st.Stop();
        //    Console.WriteLine(cloudVal);
        //}

        private List<KeyValuePair<string, object>> recursiveValues(List<KeyValuePair<string, object>> kvp, List<JToken> jToken)
        {

            foreach (JProperty keyValuePair in jToken.Children().ToList())
            {
                if (keyValuePair.Value.Children().Any())
                    recursiveValues(kvp, keyValuePair.Value.Children().ToList());
                else
                kvp.Add(new KeyValuePair<string, object>( keyValuePair.Name, keyValuePair.Value));
            }
            return kvp;
        }
        
       

      
        private string serverRunningToolTip = serverNotRunning;

      public string ServerRunningToolTip
      {
          get { return serverRunningToolTip; }
            set
            {
                serverRunningToolTip = value;
                OnPropertyChanged("ServerRunningToolTip");
            }
      }

        private System.Windows.Media.Color offsetColor2 = System.Windows.Media.Colors.Red;
        public Color OffsetColor2
        {
            get
            {
                return offsetColor2;
            }

            set
            {
                offsetColor2 = value;
                OnPropertyChanged("OffsetColor2");
            }
        }
        private int uploadStatusFile;

       public int UploadStatusFile
       {
           get { return uploadStatusFile; }
           set {
               uploadStatusFile = value;
               OnPropertyChanged("UploadStatusFile");
               }
       }
       public ICommand StartUploadCmd
       {
           get;
           set;
       }


       //private void Login(object param)
       //{
       //    if (!fileUploader.IsLogin)
       //    {
       //        fileUploader.Login2Server();
       //    }
          
       //}


       private string uploadingFile = string.Empty;

       public string UploadingFile
       {
           get { return uploadingFile; }
            set
            {
                uploadingFile = value;
                OnPropertyChanged("UploadingFile");
            }
       }
       void fileCheckTimer_Elapsed(object sender)
       {
           //fileUploader.UpdateServerStatus();
          // GetOutboxFiles();
       }

      
       



       public ICommand RemoveFile
       {
           get;
           set;
       }

        //private BindingList<UploadFileViewModel> uploadFiles;

        //public BindingList<UploadFileViewModel> UploadFiles
        //{
        //    get { return uploadFiles; }
        //    set
        //    {
        //        uploadFiles = value;
        //        OnPropertyChanged("UploadFiles");
        //    }
        //}

        private int selectedIndex;

       public int SelectedIndex
       {
           get { return selectedIndex; }
           set { 
               selectedIndex = value;
               OnPropertyChanged("SelectedIndex");
           }
       }

       

       // public void RemoveFileFromCollection(object param)
       //{
       //    UploadFiles.RemoveAt(SelectedIndex);

       //}
        public ICommand CloseAppCmd
        {
            get;
            set;
        }
        public InternetCheckViewModel InternetCheckViewModel { get => internetCheckViewModel;
            set
            {
                internetCheckViewModel = value;
                OnPropertyChanged("InternetCheckViewModel");
            }
           
        }

        public LogginVM LogginVM { get => logginVM;
            set {
                logginVM = value;
                OnPropertyChanged("LogginVM");
            }
        }

        //public BindingList<FileInfo> OutboxFiles { get => outboxFiles;
        //    set
        //    {
        //        outboxFiles = value;
        //        OnPropertyChanged("OutboxFiles");
        //    }
        //}



    }
}
