using IntuUploader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Cloud_Models.Models;
using Cloud_Models.Enums;
using BaseViewModel;
using NLog;
using System.Windows;

namespace IVLUploader.ViewModels
{
    /// <summary>
    /// Class which implements the check for internet connection by pinging to 8.8.8.8 of google
    /// </summary>
    public class CloudViewModel : ViewBaseModel
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public FileInfo ActiveFnf;

        CloudModel activeCloudModel;
        LoginViewModel activeLoginViewModel;
        CreateAnalysisViewModel activeCreateAnalysisViewModel;
        UploadImagesViewModel activeUploadImagesViewModel;
        InitiateAnalysisViewModel activeIntiateAnalysisViewModel;
        GetStatusAnalysisViewModel activeGetStatusAnalysisViewModel;
        GetAnalysisResultViewModel activeGetAnalysisResultViewModel;
        

        /// <summary>
        /// Constructor
        /// </summary>
        public CloudViewModel(CloudModel cloudModel)
        {
            logger.Info("");

            ActiveCloudModel = cloudModel;
            ActiveLoginViewModel = new LoginViewModel(ActiveCloudModel.LoginModel);
            ActiveCreateAnalysisViewModel = new CreateAnalysisViewModel(ActiveCloudModel.CreateAnalysisModel);
            ActiveUploadImagesViewModel = new UploadImagesViewModel(ActiveCloudModel.UploadModel);
            ActiveIntiateAnalysisViewModel = new InitiateAnalysisViewModel(ActiveCloudModel.InitiateAnalysisModel);
            ActiveGetStatusAnalysisViewModel = new GetStatusAnalysisViewModel(ActiveCloudModel.GetAnalysisModel);
            ActiveGetAnalysisResultViewModel = new GetAnalysisResultViewModel(ActiveCloudModel.GetAnalysisResultModel);

            //ActiveCloudModel.AnalysisFlowResponseModel = new AnalysisFlowResponseModel();
            //SetValue = new RelayCommand(param=> SetValueMethod(param));
            logger.Info("");

        }

        public ICommand SetValue
        {
            get;
            set;
        }
        public CloudModel ActiveCloudModel {
            get => activeCloudModel;
            set {
                activeCloudModel = value;
                OnPropertyChanged("ActiveCloudModel");
                }
        }

        public LoginViewModel ActiveLoginViewModel { get => activeLoginViewModel;
            set {
                activeLoginViewModel = value;
                OnPropertyChanged("ActiveLoginViewModel");
              
            }
        }
        public void StartAnalsysisFlow()
        {
            logger.Info("");

            if (ActiveCloudModel.LoginCookie == null || ActiveCloudModel.LoginCookie.Expired || ActiveCloudModel.AnalysisFlowResponseModel.LoginResponse.StatusCode == 0)
                Login();
            else if (!ActiveCloudModel.CreateAnalysisModel.CompletedStatus)
                CreateAnalysis();
            else if (!ActiveCloudModel.UploadModel.CompletedStatus)
                UploadFiles2Analysis();
            else if (!ActiveCloudModel.InitiateAnalysisModel.CompletedStatus)
                StartAnalysis();
            else if (!ActiveCloudModel.GetAnalysisModel.CompletedStatus)
                GetAnalysisStatus();
            else if (!ActiveCloudModel.GetAnalysisResultModel.CompletedStatus)
                GetAnalysisResult();
            logger.Info("");

        }
        private void StartAnalysis()
        {
            logger.Info("");
            LogginVM.GetLogginVM().Logs.Add("Start Analysis");
            ActiveCloudModel.InitiateAnalysisModel.status = "initialised";
            ActiveCloudModel.InitiateAnalysisModel.Body = JsonConvert.SerializeObject(ActiveCloudModel.InitiateAnalysisModel);
            ActiveCloudModel.AnalysisFlowResponseModel.InitiateAnalysisResponse = ActiveIntiateAnalysisViewModel.InitiateAnalysis(ActiveCloudModel.LoginCookie).Result;
            ActiveCloudModel.InitiateAnalysisModel.CompletedStatus = (ActiveCloudModel.AnalysisFlowResponseModel.InitiateAnalysisResponse.StatusCode == System.Net.HttpStatusCode.OK);
            logger.Info(JsonConvert.SerializeObject(ActiveCloudModel.AnalysisFlowResponseModel.InitiateAnalysisResponse, Formatting.Indented));

            if (ActiveCloudModel.AnalysisFlowResponseModel.InitiateAnalysisResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                File.WriteAllText(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.SentItemsDir), ActiveFnf.Name), JsonConvert.SerializeObject(ActiveCloudModel, Formatting.Indented));
                File.Delete(ActiveFnf.FullName);
                this.Dispose();
                this.ActiveFnf = null;
                logger.Info("");

            }
            else
            {
                if (ActiveCloudModel.AnalysisFlowResponseModel.InitiateAnalysisResponse.StatusCode == 0)
                {
                    StartAnalsysisFlow();
                }
                else
                {
                    InboxAnalysisStatusModel inboxAnalysisStatusModel = new InboxAnalysisStatusModel();
                    inboxAnalysisStatusModel.Status = "failure";

                    if (ActiveCloudModel.AnalysisFlowResponseModel.CreateAnalysisResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {

                        inboxAnalysisStatusModel.FailureMessage = "Wrong Details";


                    }
                    else if (ActiveCloudModel.AnalysisFlowResponseModel.CreateAnalysisResponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    {
                        inboxAnalysisStatusModel.FailureMessage = "Field Missing";

                    }
                    StreamWriter st = new StreamWriter(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.InboxDir), ActiveFnf.Name));
                    st.Write(JsonConvert.SerializeObject(inboxAnalysisStatusModel, Formatting.Indented));
                    st.Flush();
                    st.Close();
                    st.Dispose();
                    File.WriteAllText(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.ActiveDir), ActiveFnf.Name), JsonConvert.SerializeObject(ActiveCloudModel, Formatting.Indented));
                    File.Delete(ActiveFnf.FullName);
                    this.ActiveFnf = null;
                    this.Dispose();
                }
            }
        }
        private void GetAnalysisStatus()
        {

            LogginVM.GetLogginVM().Logs.Add("Get Analysis Status");

            ActiveCloudModel.GetAnalysisModel.analysis_id = ActiveCloudModel.InitiateAnalysisModel.id;
            ActiveCloudModel.GetAnalysisModel.URL_Model.API_URL_End_Point = ActiveCloudModel.GetAnalysisModel.analysis_id;
            ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisStatusResponse = ActiveGetStatusAnalysisViewModel.GetStatus(ActiveCloudModel.LoginCookie).Result;
            JObject analysisStatus_JObject = JObject.Parse(ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisStatusResponse.responseBody);
            logger.Info(JsonConvert.SerializeObject(ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisStatusResponse, Formatting.Indented));

            if (ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisStatusResponse.StatusCode == System.Net.HttpStatusCode.OK)
                if ((string)analysisStatus_JObject["status"] == "success")
                {
                    ActiveCloudModel.GetAnalysisModel.CompletedStatus = true;
                    ActiveCloudModel.GetAnalysisModel.analysis_status = (string)analysisStatus_JObject["status"];
                    ActiveCloudModel.GetAnalysisResultModel.URL_Model.API_URL_Mid_Point = "?product=" + (string)analysisStatus_JObject["product"] + "&partner_branch=" +
                       (string)analysisStatus_JObject["installation"]["partner_branch"] + "&modified=" + ((string)analysisStatus_JObject["modified"]).ToLower() + "&analysis=" + ActiveCloudModel.InitiateAnalysisModel.id + "&analyser="
                       + (string)analysisStatus_JObject["analyser_version"];

                    logger.Info(JsonConvert.SerializeObject(ActiveCloudModel.GetAnalysisResultModel, Formatting.Indented));

                    StartAnalsysisFlow();
                }
                else if((string)analysisStatus_JObject["status"] == "failure")
                {
                    ActiveCloudModel.GetAnalysisModel.CompletedStatus = true;
                    ActiveCloudModel.GetAnalysisResultModel.CompletedStatus = true;


                    InboxAnalysisStatusModel inboxAnalysisStatusModel = new InboxAnalysisStatusModel { Status = (string)analysisStatus_JObject["status"] };
                    StreamWriter st = new StreamWriter(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.InboxDir), ActiveFnf.Name));
                    st.Write(JsonConvert.SerializeObject(inboxAnalysisStatusModel, Formatting.Indented));
                    st.Flush();
                    st.Close();

                    File.WriteAllText(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.ProcessedDir), ActiveFnf.Name), JsonConvert.SerializeObject(ActiveCloudModel, Formatting.Indented));
                    File.Delete(ActiveFnf.FullName);
                    this.Dispose();
                    this.ActiveFnf = null;
                }
                else
                    StartAnalsysisFlow();
         

        }
        private void Login()
        {
            logger.Info("");
            LogginVM.GetLogginVM().Logs.Add("Login");
            ActiveCloudModel.AnalysisFlowResponseModel.LoginResponse = ActiveLoginViewModel.StartLogin().Result;
            logger.Info(JsonConvert.SerializeObject(ActiveCloudModel.AnalysisFlowResponseModel.LoginResponse, Formatting.Indented));
            if (ActiveCloudModel.AnalysisFlowResponseModel.LoginResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ActiveCloudModel.LoginCookie = ActiveCloudModel.AnalysisFlowResponseModel.LoginResponse.Cookie;
                StartAnalsysisFlow();

            }
            else 
            {
                if (ActiveCloudModel.AnalysisFlowResponseModel.LoginResponse.StatusCode == 0)
                {
                    StartAnalsysisFlow();

                }
                else
                { 
                InboxAnalysisStatusModel inboxAnalysisStatusModel = new InboxAnalysisStatusModel();
                inboxAnalysisStatusModel.Status = "failure";

                if (ActiveCloudModel.AnalysisFlowResponseModel.LoginResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    inboxAnalysisStatusModel.FailureMessage = "Wrong Device ID";


                }
                else if(ActiveCloudModel.AnalysisFlowResponseModel.LoginResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    inboxAnalysisStatusModel.FailureMessage = "Wrong Credentials";

                }
                StreamWriter st = new StreamWriter(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.InboxDir), ActiveFnf.Name));
                st.Write(JsonConvert.SerializeObject(inboxAnalysisStatusModel, Formatting.Indented));
                st.Flush();
                st.Close();
                st.Dispose();
                File.WriteAllText(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.ActiveDir), ActiveFnf.Name), JsonConvert.SerializeObject(ActiveCloudModel, Formatting.Indented));
                File.Delete(ActiveFnf.FullName);
                this.ActiveFnf = null;
                this.Dispose();
                }
            }
            logger.Info("");

        }
        private void CreateAnalysis()
        {
            logger.Info("");

            JObject Login_JObject = JObject.Parse(ActiveCloudModel.AnalysisFlowResponseModel.LoginResponse.responseBody);

            ActiveCloudModel.CreateAnalysisModel.installation_id = (string)Login_JObject["message"]["installation_id"];
            List<JToken> products = Login_JObject["message"]["products"].ToList();
            foreach (var item in products)
            {
               if( item.ToString().Contains("Fundus"))
                    ActiveCloudModel.CreateAnalysisModel.product_id = (string)item["product_id"];

            }
            ActiveCloudModel.CreateAnalysisModel.Body = string.Empty;
            ActiveCloudModel.AnalysisFlowResponseModel.CreateAnalysisResponse = ActiveCreateAnalysisViewModel.StartCreateAnalysis(ActiveCloudModel.LoginCookie).Result;
            ActiveCloudModel.CreateAnalysisModel.CompletedStatus = (ActiveCloudModel.AnalysisFlowResponseModel.CreateAnalysisResponse.StatusCode == System.Net.HttpStatusCode.OK);
            logger.Info(JsonConvert.SerializeObject(ActiveCloudModel.AnalysisFlowResponseModel.CreateAnalysisResponse, Formatting.Indented));
            if (ActiveCloudModel.AnalysisFlowResponseModel.CreateAnalysisResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                StartAnalsysisFlow();

            }
            else
            {
                if (ActiveCloudModel.AnalysisFlowResponseModel.CreateAnalysisResponse.StatusCode == 0)
                {
                }
                else
                { 
                InboxAnalysisStatusModel inboxAnalysisStatusModel = new InboxAnalysisStatusModel();
                inboxAnalysisStatusModel.Status = "failure";

                if (ActiveCloudModel.AnalysisFlowResponseModel.CreateAnalysisResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {

                    inboxAnalysisStatusModel.FailureMessage = "Wrong Product ID or Installation ID";


                }
                else if (ActiveCloudModel.AnalysisFlowResponseModel.CreateAnalysisResponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    inboxAnalysisStatusModel.FailureMessage = "Fields Missing";

                }
                StreamWriter st = new StreamWriter(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.InboxDir), ActiveFnf.Name));
                st.Write(JsonConvert.SerializeObject(inboxAnalysisStatusModel, Formatting.Indented));
                st.Flush();
                st.Close();
                st.Dispose();
                File.WriteAllText(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.ActiveDir), ActiveFnf.Name), JsonConvert.SerializeObject(ActiveCloudModel, Formatting.Indented));
                File.Delete(ActiveFnf.FullName);
                this.ActiveFnf = null;
                this.Dispose();
                }
            }
            logger.Info("");

        }
        private void UploadFiles2Analysis()
        {
            Response_CookieModel response = null;
            ActiveCloudModel.UploadModel.analysis_id =
            ActiveCloudModel.UploadModel.URL_Model.API_URL_Mid_Point =
                               ActiveCloudModel.InitiateAnalysisModel.id =
                                       (string)JObject.Parse(ActiveCloudModel.AnalysisFlowResponseModel.CreateAnalysisResponse.responseBody)["analysis_id"];
            ActiveCloudModel.UploadModel.slide_id = ActiveCloudModel.CreateAnalysisModel.sample_id;
            for (int i = 0; i < ActiveCloudModel.UploadModel.images.Length; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();
                kvp.Add("relative_path", ActiveCloudModel.UploadModel.relative_path[i]);
                kvp.Add("image", new FileInfo(ActiveCloudModel.UploadModel.images[i]));
                kvp.Add("checksum", ActiveCloudModel.UploadModel.checksums[i]);
                kvp.Add("slide_id", ActiveCloudModel.UploadModel.slide_id);
                kvp.Add("upload_type", ActiveCloudModel.UploadModel.upload_type);

                for (int j = 0; j < ActiveCloudModel.UploadModel.RetryCount; j++)
                    {
                        response = ActiveUploadImagesViewModel.StartUpload(ActiveCloudModel.LoginCookie, kvp).Result;

                        if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        {

                            response = ActiveUploadImagesViewModel.StartUpload(ActiveCloudModel.LoginCookie, kvp).Result;

                        }
                        else
                        {
                             ActiveCloudModel.AnalysisFlowResponseModel.UploadResponseList.Add(response);   
                             break;
                        }

                    }
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    if (response.StatusCode == 0)
                    {

                    }
                    else
                    {
                        InboxAnalysisStatusModel inboxAnalysisStatusModel = new InboxAnalysisStatusModel();
                        inboxAnalysisStatusModel.Status = "failure";

                        if (ActiveCloudModel.AnalysisFlowResponseModel.UploadResponseList[i].StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {

                            inboxAnalysisStatusModel.FailureMessage = "Wrong Details";


                        }
                        else if (ActiveCloudModel.AnalysisFlowResponseModel.UploadResponseList[i].StatusCode == System.Net.HttpStatusCode.InternalServerError)
                        {
                            inboxAnalysisStatusModel.FailureMessage = "Fields Missing";

                        }
                        StreamWriter st = new StreamWriter(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.InboxDir), ActiveFnf.Name));
                        st.Write(JsonConvert.SerializeObject(inboxAnalysisStatusModel, Formatting.Indented));
                        st.Flush();
                        st.Close();
                        st.Dispose();
                        File.WriteAllText(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.ActiveDir), ActiveFnf.Name), JsonConvert.SerializeObject(ActiveCloudModel, Formatting.Indented));
                        File.Delete(ActiveFnf.FullName);
                        this.ActiveFnf = null;
                        this.Dispose();
                    }
                    
                }

            }

            if(this.ActiveFnf != null)
            { 
                ActiveCloudModel.UploadModel.CompletedStatus = true;

                StartAnalsysisFlow();
            }
        }
        private void GetAnalysisResult()
        {

            ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisResultResponse = ActiveGetAnalysisResultViewModel.GetAnalysisResult(ActiveCloudModel.LoginCookie).Result;
            logger.Info(JsonConvert.SerializeObject(ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisResultResponse, Formatting.Indented));

            if (ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisResultResponse.StatusCode == System.Net.HttpStatusCode.OK)

            {
                StreamWriter st = new StreamWriter(ActiveFnf.FullName);
                st.Write(JsonConvert.SerializeObject(ActiveCloudModel, Formatting.Indented));
                st.Flush();
                st.Close();
                ActiveGetAnalysisResultViewModel.GetAnalysisResultModel.CompletedStatus = true;
                InboxAnalysisStatusModel inboxAnalysisStatusModel = new InboxAnalysisStatusModel();

                inboxAnalysisStatusModel.cloudID = ActiveCloudModel.cloudID;
                inboxAnalysisStatusModel.reportID = ActiveCloudModel.reportID;
                inboxAnalysisStatusModel.visitID = ActiveCloudModel.visitID;
                inboxAnalysisStatusModel.patientID = ActiveCloudModel.patientID;

                JObject Login_JObject = JObject.Parse(ActiveCloudModel.AnalysisFlowResponseModel.LoginResponse.responseBody);
                ActiveCloudModel.CreateAnalysisModel.installation_id = (string)Login_JObject["message"]["installation_id"];
                List<JToken> products = Login_JObject["message"]["products"].ToList();
                string sub_category = string.Empty;
                foreach (var item in products)
                {
                    if (item.ToString().Contains("Fundus"))
                    {
                        ActiveCloudModel.CreateAnalysisModel.product_id = (string)item["product_id"];
                        sub_category = (string)item["sub_ctgy"];
                    }
                }
                inboxAnalysisStatusModel.ReportUri = new System.Uri(ActiveCloudModel.GetAnalysisResultModel.URL_Model.API_URL.Replace("api", "ui") + "report/" + sub_category +
                  "/" + ActiveCloudModel.CreateAnalysisModel.product_id + "/" + ActiveCloudModel.CreateAnalysisModel.sample_id + "/" + ActiveCloudModel.InitiateAnalysisModel.id );
                    

                JObject jObject = JObject.Parse(ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisResultResponse.responseBody);
                inboxAnalysisStatusModel.Status = ActiveCloudModel.GetAnalysisModel.analysis_status;

                List<JToken> right_tokens = jObject.Values().ToList()[0].Values().ToList()[5].Values().ToList()[0].Values().ToList()[3].ToList().Values().ToList();
                List<JToken> left_tokens = jObject.Values().ToList()[0].Values().ToList()[5].Values().ToList()[1].Values().ToList()[3].ToList().Values().ToList();

                int rightImageCnt = (int)((double)right_tokens.Count / 9);
                int leftImageCnt = (int)((double)left_tokens.Count / 9);
                for (int i = 0; i < rightImageCnt; i++)
                {
                    var indx = i * 9;
                    inboxAnalysisStatusModel.RightEyeDetails.Add(new ImageAnalysisResultModel
                    {
                        Analysis_Result = (string)right_tokens[indx + 7],
                        QI_Result = (string)right_tokens[indx + 6],
                        ImageName = (string)right_tokens[indx]
                    });
                    if (inboxAnalysisStatusModel.RightEyeDetails[0].Analysis_Result.Contains("PDR"))
                    {
                        inboxAnalysisStatusModel.RightAIImpressions = inboxAnalysisStatusModel.RightEyeDetails[0].Analysis_Result;
                        inboxAnalysisStatusModel.RightEyeDetails[0] = inboxAnalysisStatusModel.RightEyeDetails[inboxAnalysisStatusModel.RightEyeDetails.Count - 1];

                    }
                    else if (inboxAnalysisStatusModel.RightEyeDetails[0].Analysis_Result.Contains("NPDR") && !inboxAnalysisStatusModel.RightAIImpressions.Contains("PDR"))
                    {
                        inboxAnalysisStatusModel.RightAIImpressions = inboxAnalysisStatusModel.RightEyeDetails[0].Analysis_Result;
                        inboxAnalysisStatusModel.RightEyeDetails[0] = inboxAnalysisStatusModel.RightEyeDetails[inboxAnalysisStatusModel.RightEyeDetails.Count - 1];

                    }
                    else if (!inboxAnalysisStatusModel.LeftAIImpressions.Contains("NPDR"))
                    {
                        inboxAnalysisStatusModel.RightAIImpressions = inboxAnalysisStatusModel.RightEyeDetails[0].Analysis_Result;
                        inboxAnalysisStatusModel.RightEyeDetails[0] = inboxAnalysisStatusModel.RightEyeDetails[inboxAnalysisStatusModel.RightEyeDetails.Count - 1];

                    }
                }
                for (int i = 0; i < leftImageCnt; i++)
                {
                    var indx = i * 9;
                    inboxAnalysisStatusModel.LeftEyeDetails.Add(new ImageAnalysisResultModel
                    {

                        Analysis_Result = (string)left_tokens[indx + 7],
                        QI_Result = (string)left_tokens[indx + 6],
                        ImageName = (string)left_tokens[indx]
                    });
                    if (inboxAnalysisStatusModel.LeftEyeDetails[0].Analysis_Result.Contains("PDR"))
                    {
                        inboxAnalysisStatusModel.LeftAIImpressions = inboxAnalysisStatusModel.LeftEyeDetails[0].Analysis_Result;
                        inboxAnalysisStatusModel.LeftEyeDetails[0] = inboxAnalysisStatusModel.LeftEyeDetails[inboxAnalysisStatusModel.LeftEyeDetails.Count - 1];
                    }
                    else if (inboxAnalysisStatusModel.LeftEyeDetails[0].Analysis_Result.Contains("NPDR") && !inboxAnalysisStatusModel.LeftAIImpressions.Contains("PDR"))
                    {
                        inboxAnalysisStatusModel.LeftAIImpressions = inboxAnalysisStatusModel.LeftEyeDetails[0].Analysis_Result;
                        inboxAnalysisStatusModel.LeftEyeDetails[0] = inboxAnalysisStatusModel.LeftEyeDetails[inboxAnalysisStatusModel.LeftEyeDetails.Count - 1];

                    }
                    else if (!inboxAnalysisStatusModel.LeftAIImpressions.Contains("NPDR"))
                    {
                        inboxAnalysisStatusModel.LeftAIImpressions = inboxAnalysisStatusModel.LeftEyeDetails[0].Analysis_Result;
                        inboxAnalysisStatusModel.LeftEyeDetails[0] = inboxAnalysisStatusModel.LeftEyeDetails[inboxAnalysisStatusModel.LeftEyeDetails.Count - 1];

                    }

                }

                st = new StreamWriter(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.InboxDir), ActiveFnf.Name));
                st.Write(JsonConvert.SerializeObject(inboxAnalysisStatusModel, Formatting.Indented));
                st.Flush();
                st.Close();
                st.Dispose();
                File.WriteAllText(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.ProcessedDir), ActiveFnf.Name), JsonConvert.SerializeObject(ActiveCloudModel, Formatting.Indented));
                File.Delete(ActiveFnf.FullName);
                this.ActiveFnf = null;
                this.Dispose();
            }
            
            else
            {
                if (ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisResultResponse.StatusCode == 0)
                {
                    StartAnalsysisFlow();

                }
                else
                    StartAnalsysisFlow();
            }
            logger.Info("");

        }
        public CreateAnalysisViewModel ActiveCreateAnalysisViewModel { get => activeCreateAnalysisViewModel;
            set {
                activeCreateAnalysisViewModel = value;
                OnPropertyChanged("ActiveCreateAnalysisViewModel");
            }
        }

        public UploadImagesViewModel ActiveUploadImagesViewModel
        {
            get => activeUploadImagesViewModel;
            set {
                activeUploadImagesViewModel = value;
                OnPropertyChanged("ActiveUploadImagesViewModel");
            }
        }

        public InitiateAnalysisViewModel ActiveIntiateAnalysisViewModel { get => activeIntiateAnalysisViewModel;
            set
            {
                activeIntiateAnalysisViewModel = value;
                OnPropertyChanged("ActiveIntiateAnalysisViewModel");
            }
        }

        public GetStatusAnalysisViewModel ActiveGetStatusAnalysisViewModel
        {
            get => activeGetStatusAnalysisViewModel;
            set
            {
                activeGetStatusAnalysisViewModel = value;
                OnPropertyChanged("ActiveGetStatusAnalysisViewModel");
            }
        }

        public GetAnalysisResultViewModel ActiveGetAnalysisResultViewModel
        {
            get => activeGetAnalysisResultViewModel;
            set
            {
                activeGetAnalysisResultViewModel = value;
                OnPropertyChanged("ActiveGetAnalysisResultViewModel");

            }
        }

        public void SetValueMethod(object param)
        {
            //this.FileUploadStatus = 100;
        }

    }
}
