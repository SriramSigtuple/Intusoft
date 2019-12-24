using BaseViewModel;
using Cloud_Models.Models;
using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace IntuUploader.ViewModels
{
    /// <summary>
    /// Class which implements the check for internet connection by pinging to 8.8.8.8 of google
    /// </summary>
    public class CloudViewModel : ViewBaseModel
    {
        static Logger exceptionLog = LogManager.GetLogger("ExceptionLogger");
        private Logger logger;// = LogManager.GetLogger("Logger");
        public DirectoryEnum nextDirectory;
        public FileInfo ActiveFnf = null;

        public delegate void DelegatePendingFileCreation(AnalysisType analysisType);
        public event DelegatePendingFileCreation CreatePendingFilesEvent;

        public delegate void DelegateMove_WriteFile(AnalysisType analysisType);
        public event DelegateMove_WriteFile Write_R_Move_File_Event;
        CloudModel activeCloudModel;
        AnalysisViewModel activeLoginViewModel;
        AnalysisViewModel activeCreateAnalysisViewModel;
        AnalysisViewModel activeUploadImagesViewModel;
        AnalysisViewModel activeIntiateAnalysisViewModel;
        AnalysisViewModel activeGetStatusAnalysisViewModel;
        AnalysisViewModel activeGetAnalysisResultViewModel;
        string RightEyeComments = string.Empty;
        string LeftEyeComments = string.Empty;
        string RejectComments = string.Empty;
        public delegate void StartStopTimer(bool isStart);
        public event StartStopTimer startStopEvent;
        private bool isMove2NextDir = false;

        private bool _IsBusy;

        public bool IsBusy
        {
            get { return _IsBusy; }
            set
            {
                _IsBusy = value;
                if (!_IsBusy)
                {
                    if (CreatePendingFilesEvent != null)
                        CreatePendingFilesEvent(this.AnalysisType);
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CloudViewModel(AnalysisType analysisType)
        {
            AnalysisType = analysisType;

            if (AnalysisType == AnalysisType.Fundus)
                logger = LogManager.GetLogger("FundusLogger");
            else
                logger = LogManager.GetLogger("QILogger");
        }

        public void SetCloudModel(CloudModel cloudModel)
        {

            isMove2NextDir = false;
            ActiveCloudModel = cloudModel;
            ActiveLoginViewModel = new AnalysisViewModel(ActiveCloudModel.LoginModel);
            ActiveCreateAnalysisViewModel = new AnalysisViewModel(ActiveCloudModel.CreateAnalysisModel);
            ActiveUploadImagesViewModel = new AnalysisViewModel(ActiveCloudModel.UploadModel);
            ActiveIntiateAnalysisViewModel = new AnalysisViewModel(ActiveCloudModel.InitiateAnalysisModel);
            ActiveGetStatusAnalysisViewModel = new AnalysisViewModel(ActiveCloudModel.GetAnalysisModel);
            ActiveGetAnalysisResultViewModel = new AnalysisViewModel(ActiveCloudModel.GetAnalysisResultModel);
            ActiveApproveDoctorViewmodel = new AnalysisViewModel(ActiveCloudModel.DoctorApprovalModel);
            ActiveNotifyEmailViewModel = new AnalysisViewModel(ActiveCloudModel.NotifyEmailModel);
            ActiveGetStatusPostDoctorApprovalViewModel = new AnalysisViewModel(ActiveCloudModel.GetAnalysisPostDoctorApproval);
            ActiveDoctorCommentsViewModel = new AnalysisViewModel(ActiveCloudModel.DoctorCommentsModel);
            RightEyeComments = string.Empty;
            LeftEyeComments = string.Empty;
            //ac = new AnalysisViewModel(ActiveCloudModel.DoctorApprovalModel);
            //ActiveCloudModel.AnalysisFlowResponseModel = new AnalysisFlowResponseModel();
            //SetValue = new RelayCommand(param=> SetValueMethod(param));

        }
        public ICommand SetValue
        {
            get;
            set;
        }
        public CloudModel ActiveCloudModel
        {
            get => activeCloudModel;
            set
            {
                activeCloudModel = value;
                OnPropertyChanged("ActiveCloudModel");
            }
        }

        public AnalysisViewModel ActiveLoginViewModel
        {
            get => activeLoginViewModel;
            set
            {
                activeLoginViewModel = value;
                OnPropertyChanged("ActiveLoginViewModel");

            }
        }
        public void StartAnalysisFlow()
        {
            if (GlobalVariables.isInternetPresent)
            {

                if (ActiveCloudModel.LoginCookie == null || ActiveCloudModel.LoginCookie.Expires < DateTime.Now)
                    Login();
                else if (!ActiveCloudModel.CreateAnalysisModel.CompletedStatus)
                    CreateAnalysis();
                else if (!ActiveCloudModel.UploadModel.CompletedStatus)
                    UploadFiles2Analysis();
                else if (!ActiveCloudModel.InitiateAnalysisModel.CompletedStatus)
                    StartAnalysis();
                else if (!ActiveCloudModel.GetAnalysisModel.CompletedStatus)
                    GetAnalysisStatus();
                else if (!ActiveCloudModel.DoctorApprovalModel.CompletedStatus)
                    GetDoctorApproval();
                else if (!ActiveCloudModel.NotifyEmailModel.CompletedStatus)
                    NotifyEmail2Doctor();
                else if (!ActiveCloudModel.GetAnalysisPostDoctorApproval.CompletedStatus)
                    GetAnalysisPostDoctorApprovalStatus();
                else if (!ActiveCloudModel.DoctorCommentsModel.CompletedStatus)
                    GetDoctorComments();
                else if (!ActiveCloudModel.GetAnalysisResultModel.CompletedStatus)
                    GetAnalysisResult();
                else
                    IsMove2NextDir = true;
            }
            else
            {
                logger.Info("Internet connection{GlobalVariables.isInternetPresent}");
                //StreamWriter st = new StreamWriter(ActiveFnf.FullName);
                //st.Write(JsonConvert.SerializeObject(ActiveCloudModel, Formatting.Indented));
                //st.Flush();
                //st.Close();
                //st.Dispose();
                //this.IsBusy = false; 
                IsMove2NextDir = false;
                //this.Dispose();
            }



        }

        /// <summary>
        /// To start the QI analysis
        /// </summary>
        private void StartAnalysis()
        {
            try
            {
                logger.Info("Start Analysis");
                logger.Info("Start Analysis Result");

                isValidLoginCookie();
                ActiveCloudModel.InitiateAnalysisModel.Body = string.Empty;

                ActiveCloudModel.InitiateAnalysisModel.status = "initialised";
                ActiveCloudModel.InitiateAnalysisModel.Body = JsonConvert.SerializeObject(ActiveCloudModel.InitiateAnalysisModel);
                ActiveCloudModel.AnalysisFlowResponseModel.InitiateAnalysisResponse = ActiveIntiateAnalysisViewModel.InitiateRestCall(ActiveCloudModel.LoginCookie, new Dictionary<string, object>()).Result;
                logger.Info(JsonConvert.SerializeObject(ActiveCloudModel.AnalysisFlowResponseModel.InitiateAnalysisResponse, Formatting.Indented));
                logger.Info("Start Analysis Result status {0}", ActiveCloudModel.AnalysisFlowResponseModel.InitiateAnalysisResponse.StatusCode);

                if (ActiveCloudModel.AnalysisFlowResponseModel.InitiateAnalysisResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ActiveCloudModel.InitiateAnalysisModel.CompletedStatus = (ActiveCloudModel.AnalysisFlowResponseModel.InitiateAnalysisResponse.StatusCode == System.Net.HttpStatusCode.OK);

                    //File.Delete(ActiveFnf.FullName);

                    //StreamWriter st = new StreamWriter(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.SentItemsDir,AnalysisType), ActiveFnf.Name));
                    //st.Write(JsonConvert.SerializeObject(ActiveCloudModel, Formatting.Indented));
                    //st.Flush();
                    //st.Close();
                    //st.Dispose();
                    //File.WriteAllText(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.SentItemsDir), ActiveFnf.Name), JsonConvert.SerializeObject(ActiveCloudModel, Formatting.Indented));

                    //this.Dispose();

                    //startStopEvent(true);
                    // this.IsBusy = false; ;
                    StartAnalysisFlow();

                }
                else
                {
                    {
                        ManageFailureResponse(ActiveCloudModel.AnalysisFlowResponseModel.InitiateAnalysisResponse, "Start Analysis");
                    }
                }
            }
            catch (Exception ex)
            {

                logger.Info(ex.StackTrace);
                exceptionLog.Error(Common.Exception2StringConverter.GetInstance().ConvertException2String(ex));

            }

        }


        /// <summary>
        /// Initiate Doctor Approval
        /// </summary>
        private void GetDoctorApproval()
        {
            logger.Info("Start Doctor Approval");
            ActiveCloudModel.DoctorApprovalModel.analysisId = ActiveCloudModel.UploadModel.analysis_id;
            ActiveCloudModel.AnalysisFlowResponseModel.GetDoctorApprovalResponse = ActiveApproveDoctorViewmodel.InitiateRestCall(ActiveCloudModel.LoginCookie, new Dictionary<string, object>()).Result;
            logger.Info(ActiveCloudModel.AnalysisFlowResponseModel.GetDoctorApprovalResponse.StatusCode.ToString() + " " + ActiveFnf.Name);
            logger.Info(JsonConvert.SerializeObject(ActiveCloudModel.AnalysisFlowResponseModel.GetDoctorApprovalResponse, Formatting.Indented));
            if (ActiveCloudModel.AnalysisFlowResponseModel.GetDoctorApprovalResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {

                ActiveCloudModel.DoctorApprovalModel.CompletedStatus = true;
                StartAnalysisFlow();
            }
            else
            {
                ManageFailureResponse(ActiveCloudModel.AnalysisFlowResponseModel.GetDoctorApprovalResponse, "Doctor Approval");
            }

        }

        private void NotifyEmail2Doctor()
        {
            logger.Info("Notify Email to Doctor");
            JObject Login_JObject = JObject.Parse(ActiveCloudModel.AnalysisFlowResponseModel.LoginResponse.responseBody);
            List<JToken> products = Login_JObject["message"]["products"].ToList();
            var sub_ctgy = string.Empty;
            foreach (var item in products)
            {
                if (item["sub_ctgy"].ToString().ToLower().Contains(AnalysisType.ToString("g").ToLower()))
                    sub_ctgy = item["sub_ctgy"].ToString().ToLower();
            }

            Login_JObject = JObject.Parse(ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisStatusResponse.responseBody);

            ActiveCloudModel.NotifyEmailModel.URL_Model.API_URL_Mid_Point += $"product={ActiveCloudModel.CreateAnalysisModel.product_id}&puid={Login_JObject["puid"].ToString()}&sample_id={ActiveCloudModel.CreateAnalysisModel.sample_id}&analysisId={ActiveCloudModel.UploadModel.analysis_id}&" +
                $"recipients={ActiveCloudModel.DoctorApprovalModel.reviewerId}&type=assign&sub_ctgy={sub_ctgy}";


            ActiveCloudModel.AnalysisFlowResponseModel.NotifyEmail2DoctorResponse = ActiveNotifyEmailViewModel.InitiateRestCall(ActiveCloudModel.LoginCookie, new Dictionary<string, object>()).Result;
            logger.Info(ActiveCloudModel.AnalysisFlowResponseModel.NotifyEmail2DoctorResponse.StatusCode.ToString() + " " + ActiveFnf.Name);
            logger.Info(JsonConvert.SerializeObject(ActiveCloudModel.AnalysisFlowResponseModel.NotifyEmail2DoctorResponse, Formatting.Indented));
            if (ActiveCloudModel.AnalysisFlowResponseModel.NotifyEmail2DoctorResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ActiveCloudModel.NotifyEmailModel.CompletedStatus = true;
                StartAnalysisFlow();
            }
            else
            {
                ManageFailureResponse(ActiveCloudModel.AnalysisFlowResponseModel.NotifyEmail2DoctorResponse, "Notify Email");
            }

        }
        /// <summary>
        /// 
        /// </summary>
        private void GetDoctorComments()
        {
            logger.Info("Get Doctor comments");

            ActiveCloudModel.DoctorCommentsModel.URL_Model.API_URL_End_Point = ActiveCloudModel.UploadModel.analysis_id;
            ActiveCloudModel.AnalysisFlowResponseModel.DoctorCommentsResponse = ActiveDoctorCommentsViewModel.InitiateRestCall(ActiveCloudModel.LoginCookie, new Dictionary<string, object>()).Result;
            logger.Info(ActiveCloudModel.AnalysisFlowResponseModel.DoctorCommentsResponse.StatusCode.ToString() + " " + ActiveFnf.Name);
            logger.Info(JsonConvert.SerializeObject(ActiveCloudModel.AnalysisFlowResponseModel.DoctorCommentsResponse, Formatting.Indented));
            if (ActiveCloudModel.AnalysisFlowResponseModel.DoctorCommentsResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ActiveCloudModel.DoctorCommentsModel.CompletedStatus = true;
                JObject doctorComments = JObject.Parse(ActiveCloudModel.AnalysisFlowResponseModel.DoctorCommentsResponse.responseBody);
                if(doctorComments["message"].HasValues)
                {
                    if (doctorComments["message"].ToString().Contains("REJECT-FUNDUS") && doctorComments["message"]["REJECT-FUNDUS"].Last.HasValues)
                        RejectComments += doctorComments["message"]["REJECT-FUNDUS"].Last["description"].ToString();
                        if (doctorComments["message"]["DR-RE"].Last.HasValues)
                            RightEyeComments += " DR - " + doctorComments["message"]["DR-RE"].Last["description"].ToString();
                        if (doctorComments["message"]["GLAUCOMA-RE"].Last.HasValues)
                            RightEyeComments += " Glaucoma - " + doctorComments["message"]["GLAUCOMA-RE"].Last["description"].ToString();

                        if (doctorComments["message"]["AMD-RE"].Last.HasValues)
                            RightEyeComments += " AMD - " + doctorComments["message"]["AMD-RE"].Last["description"].ToString();

                        if (doctorComments["message"]["DR-LE"].Last.HasValues)
                            LeftEyeComments += " DR - " + doctorComments["message"]["DR-LE"].Last["description"].ToString();
                        if (doctorComments["message"]["GLAUCOMA-LE"].Last.HasValues)
                            LeftEyeComments += " Glaucoma - " + doctorComments["message"]["GLAUCOMA-LE"].Last["description"].ToString();
                        if (doctorComments["message"]["AMD-LE"].Last.HasValues)
                            LeftEyeComments += " AMD - " + doctorComments["message"]["AMD-LE"].Last["description"].ToString();

                }
                StartAnalysisFlow();
            }
            else
            {
                ManageFailureResponse(ActiveCloudModel.AnalysisFlowResponseModel.DoctorCommentsResponse, "Doctor Comments");
            }
        }
        /// <summary>
        /// To read and update the analysis status.
        /// </summary>
        private async void GetAnalysisStatus()
        {
            try
            {
                logger.Info("Get Analysis Status");
                isValidLoginCookie();
                logger.Info("Get Analysis status ");//,ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisResultResponse.StatusCode);
                ActiveCloudModel.GetAnalysisModel.Body = string.Empty;

                ActiveCloudModel.GetAnalysisModel.analysis_id = ActiveCloudModel.InitiateAnalysisModel.id;
                ActiveCloudModel.GetAnalysisModel.URL_Model.API_URL_End_Point = ActiveCloudModel.GetAnalysisModel.analysis_id;

                    ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisStatusResponse = await ActiveGetStatusAnalysisViewModel.InitiateRestCall(ActiveCloudModel.LoginCookie, new Dictionary<string, object>());

                    logger.Info("Get Analysis status {0} {1}", ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisStatusResponse.StatusCode, ActiveFnf.Name);

                if (ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisStatusResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    JObject analysisStatus_JObject = JObject.Parse(ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisStatusResponse.responseBody);
                    logger.Info(JsonConvert.SerializeObject(ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisStatusResponse, Formatting.Indented));
                    logger.Info("Get Analysis response status {0}", (string)analysisStatus_JObject["status"]);
                    {
                        if ((string)analysisStatus_JObject["status"] == "success")
                        {
                            ActiveCloudModel.GetAnalysisModel.analysis_status = (string)analysisStatus_JObject["status"];
                            ActiveCloudModel.GetAnalysisResultModel.URL_Model.API_URL_Mid_Point = "?product=" + (string)analysisStatus_JObject["product"] + "&partner_branch=" +
                               (string)analysisStatus_JObject["installation"]["partner_branch"] + "&modified=" + ((string)analysisStatus_JObject["modified"]).ToLower() + "&analysis=" + ActiveCloudModel.InitiateAnalysisModel.id + "&analyser="
                               + (string)analysisStatus_JObject["analyser_version"];

                            logger.Info(JsonConvert.SerializeObject(ActiveCloudModel.GetAnalysisResultModel, Formatting.Indented));
                            ActiveCloudModel.GetAnalysisModel.CompletedStatus = true;
                            StartAnalysisFlow();
                        }
                        else if ((string)analysisStatus_JObject["status"] == "failure")
                        {

                            var failure_message = analysisStatus_JObject["failure_reason"].ToString();
                            ManageFailureResponse(ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisStatusResponse, "Get Analysis Result", failure_message);
                            //this.Dispose();
                            ActiveCloudModel.GetAnalysisModel.CompletedStatus = true;
                            ActiveCloudModel.GetAnalysisResultModel.CompletedStatus = true;
                        }
                        else
                        {
                            logger.Info(JsonConvert.SerializeObject(ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisStatusResponse, Formatting.Indented));
                            try
                            {
                                IsMove2NextDir = false;

                            }
                            catch (Exception ex)
                            {
                                exceptionLog.Error(Common.Exception2StringConverter.GetInstance().ConvertException2String(ex));

                            }
                        }
                    }

                }
                else
                {
                    ManageFailureResponse(ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisStatusResponse, "Status");
                    logger.Info(JsonConvert.SerializeObject(ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisStatusResponse, Formatting.Indented));
                    ////this.Dispose();
                    //this.IsBusy = false;;
                }
            }
            catch (Exception ex)
            {

                logger.Info(ex.StackTrace);
                exceptionLog.Error(Common.Exception2StringConverter.GetInstance().ConvertException2String(ex));

            }



        }

        /// <summary>
        /// To read and update the analysis status.
        /// </summary>
        private async void GetAnalysisPostDoctorApprovalStatus()
        {
            try
            {
                logger.Info("Get Analysis doctor approval Status");
                isValidLoginCookie();
                logger.Info("Get Analysis doctor approval Status ");//,ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisResultResponse.StatusCode);
                ActiveCloudModel.GetAnalysisPostDoctorApproval.Body = string.Empty;

                ActiveCloudModel.GetAnalysisPostDoctorApproval.analysis_id = ActiveCloudModel.InitiateAnalysisModel.id;
                ActiveCloudModel.GetAnalysisPostDoctorApproval.URL_Model.API_URL_End_Point = ActiveCloudModel.GetAnalysisModel.analysis_id;

                ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisPostDoctorResponse = await ActiveGetStatusPostDoctorApprovalViewModel.InitiateRestCall(ActiveCloudModel.LoginCookie, new Dictionary<string, object>());

                logger.Info("Get Analysis doctor approval Status {0} {1}", ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisPostDoctorResponse.StatusCode, ActiveFnf.Name);

                if (ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisPostDoctorResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    JObject analysisStatus_JObject = JObject.Parse(ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisPostDoctorResponse.responseBody);
                    logger.Info(JsonConvert.SerializeObject(ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisPostDoctorResponse, Formatting.Indented));
                    logger.Info("Get Analysis doctor approval Status {0}", (string)analysisStatus_JObject["status"]);
                    {
                        if ((string)analysisStatus_JObject["review_status"] == "Accepted" || (string)analysisStatus_JObject["review_status"] == "Rejected")
                        {
                            ActiveCloudModel.GetAnalysisPostDoctorApproval.analysis_status = (string)analysisStatus_JObject["status"];
                            ActiveCloudModel.GetAnalysisResultModel.URL_Model.API_URL_Mid_Point = "?product=" + (string)analysisStatus_JObject["product"] + "&partner_branch=" +
                               (string)analysisStatus_JObject["installation"]["partner_branch"] + "&modified=" + ((string)analysisStatus_JObject["modified"]).ToLower() + "&analysis=" + ActiveCloudModel.InitiateAnalysisModel.id + "&analyser="
                               + (string)analysisStatus_JObject["analyser_version"];

                            logger.Info(JsonConvert.SerializeObject(ActiveCloudModel.GetAnalysisPostDoctorApproval, Formatting.Indented));
                            ActiveCloudModel.GetAnalysisPostDoctorApproval.CompletedStatus = true;
                            StartAnalysisFlow();
                        }
                        else if ((string)analysisStatus_JObject["status"] == "failure")
                        {

                            var failure_message = analysisStatus_JObject["failure_reason"].ToString();
                            ManageFailureResponse(ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisStatusResponse, "Get Analysis doctor approval Status", failure_message);
                            //this.Dispose();
                            ActiveCloudModel.GetAnalysisPostDoctorApproval.CompletedStatus = true;
                        }
                        else
                        {
                            logger.Info(JsonConvert.SerializeObject(ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisPostDoctorResponse, Formatting.Indented));
                            try
                            {
                                IsMove2NextDir = false;

                            }
                            catch (Exception ex)
                            {
                                exceptionLog.Error(Common.Exception2StringConverter.GetInstance().ConvertException2String(ex));

                            }
                        }
                    }
                }
                else
                {
                    ManageFailureResponse(ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisPostDoctorResponse, "Status");
                    logger.Info(JsonConvert.SerializeObject(ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisPostDoctorResponse, Formatting.Indented));
                    ////this.Dispose();
                    //this.IsBusy = false;;
                }
            }
            catch (Exception ex)
            {

                logger.Info(ex.StackTrace);
                exceptionLog.Error(Common.Exception2StringConverter.GetInstance().ConvertException2String(ex));

            }



        }

        /// <summary>
        /// To login to the QI api.
        /// </summary>
        private void Login()
        {
            try
            {
                logger.Info("Iam Login");
                ActiveCloudModel.AnalysisFlowResponseModel.LoginResponse = ActiveLoginViewModel.InitiateRestCall(new System.Net.Cookie(), new Dictionary<string, object>()).Result;
                logger.Info(ActiveCloudModel.AnalysisFlowResponseModel.LoginResponse.StatusCode.ToString() + " " + ActiveFnf.Name);
                logger.Info(JsonConvert.SerializeObject(ActiveCloudModel.AnalysisFlowResponseModel.LoginResponse, Formatting.Indented));
                if (ActiveCloudModel.AnalysisFlowResponseModel.LoginResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    if (ActiveCloudModel.AnalysisFlowResponseModel.LoginResponse.responseBody.Contains("installation_id"))
                    {
                        ActiveCloudModel.LoginCookie = ActiveCloudModel.AnalysisFlowResponseModel.LoginResponse.Cookie;
                        ActiveCloudModel.LoginModel.CompletedStatus = true;
                        StartAnalysisFlow();
                    }
                    else
                    {
                        ManageFailureResponse(ActiveCloudModel.AnalysisFlowResponseModel.LoginResponse, "Login", "Wrong Installation ID");
                        //this.Dispose();
                    }


                }
                else
                {

                    ManageFailureResponse(ActiveCloudModel.AnalysisFlowResponseModel.LoginResponse, "Login", "Wrong Credentials");

                }
            }
            catch (Exception ex)
            {

                logger.Error(ex);
                exceptionLog.Error(Common.Exception2StringConverter.GetInstance().ConvertException2String(ex));

                ManageFailureResponse(ActiveCloudModel.AnalysisFlowResponseModel.LoginResponse, "Login", ex.Message);

            }



        }

        /// <summary>
        /// To initiate the creation of .analysis
        /// </summary>
        private void CreateAnalysis()
        {
            try
            {
                logger.Info("Create Analysis");

                logger.Info("Create Analysis  {0}", ActiveFnf.Name);
                isValidLoginCookie();
                JObject Login_JObject = JObject.Parse(ActiveCloudModel.AnalysisFlowResponseModel.LoginResponse.responseBody);

                ActiveCloudModel.CreateAnalysisModel.Body = string.Empty;
                ActiveCloudModel.CreateAnalysisModel.installation_id = (string)Login_JObject["message"]["installation_id"];
                List<JToken> products = Login_JObject["message"]["products"].ToList();
                foreach (var item in products)
                {
                    if (item["sub_ctgy"].ToString().ToLower().Contains(AnalysisType.ToString("g").ToLower()))
                        ActiveCloudModel.CreateAnalysisModel.product_id = (string)item["product_id"];
                    else if ((item["ctgy"].ToString().ToLower().Contains(AnalysisType.ToString("g").ToLower())))
                        ActiveCloudModel.CreateAnalysisModel.product_id = (string)item["product_id"];

                }
                ActiveCloudModel.CreateAnalysisModel.Body = string.Empty;
                ActiveCloudModel.AnalysisFlowResponseModel.CreateAnalysisResponse = ActiveCreateAnalysisViewModel.InitiateRestCall(ActiveCloudModel.LoginCookie, new Dictionary<string, object>()).Result;
                logger.Info("Create Analysis Result status {0}  {1}", ActiveCloudModel.AnalysisFlowResponseModel.CreateAnalysisResponse.StatusCode, ActiveFnf.Name);

                logger.Info(JsonConvert.SerializeObject(ActiveCloudModel.AnalysisFlowResponseModel.CreateAnalysisResponse, Formatting.Indented));
                if (ActiveCloudModel.AnalysisFlowResponseModel.CreateAnalysisResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ActiveCloudModel.CreateAnalysisModel.CompletedStatus = (ActiveCloudModel.AnalysisFlowResponseModel.CreateAnalysisResponse.StatusCode == System.Net.HttpStatusCode.OK);

                    StartAnalysisFlow();

                }
                else
                {
                    ActiveCloudModel.CreateAnalysisModel.CompletedStatus = (ActiveCloudModel.AnalysisFlowResponseModel.CreateAnalysisResponse.StatusCode == System.Net.HttpStatusCode.OK);

                    ManageFailureResponse(ActiveCloudModel.AnalysisFlowResponseModel.CreateAnalysisResponse, "Create");


                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                exceptionLog.Error(Common.Exception2StringConverter.GetInstance().ConvertException2String(ex));

                ManageFailureResponse(ActiveCloudModel.AnalysisFlowResponseModel.CreateAnalysisResponse, ex.Message);
            }


        }


        /// <summary>
        /// To upload the file to analysis.
        /// </summary>
        private void UploadFiles2Analysis()
        {
            try
            {
                isValidLoginCookie();
                logger.Info("Upload Analysis");

                Response_CookieModel response = null;
                ActiveCloudModel.UploadModel.Body = string.Empty;

                ActiveCloudModel.UploadModel.analysis_id =
                ActiveCloudModel.UploadModel.URL_Model.API_URL_Mid_Point =
                                   ActiveCloudModel.InitiateAnalysisModel.id =
                                           (string)JObject.Parse(ActiveCloudModel.AnalysisFlowResponseModel.CreateAnalysisResponse.responseBody)["analysis_id"];
                ActiveCloudModel.UploadModel.slide_id = ActiveCloudModel.CreateAnalysisModel.sample_id;
                for (int i = 0; i < ActiveCloudModel.UploadModel.images.Length; i++)
                {
                    Dictionary<string, object> kvp = new Dictionary<string, object>();
                    kvp.Add("relative_path", ActiveCloudModel.UploadModel.relative_path[i]);
                    var fInf = new FileInfo(ActiveCloudModel.UploadModel.images[i]);
                    kvp.Add("image", new FileInfo(ActiveCloudModel.UploadModel.images[i]));
                    var checkSumValue = fInf.GetMd5Hash();
                    if (ActiveCloudModel.UploadModel.checksums[i].Equals(checkSumValue.responseMessage))
                        kvp.Add("checksum", ActiveCloudModel.UploadModel.checksums[i]);
                    else
                    {
                        kvp.Add("checksum", checkSumValue.responseMessage);
                        logger.Info("CheckSum Mismatch");
                        logger.Info("CheckSum From File" + ActiveCloudModel.UploadModel.checksums[i]);
                        logger.Info("CheckSum From Code" + checkSumValue.responseMessage);

                    }

                    kvp.Add("slide_id", ActiveCloudModel.UploadModel.slide_id);
                    kvp.Add("upload_type", ActiveCloudModel.UploadModel.upload_type);

                    for (int j = 0; j < ActiveCloudModel.UploadModel.RetryCount; j++)
                    {
                        response = ActiveUploadImagesViewModel.InitiateRestCall(ActiveCloudModel.LoginCookie, kvp).Result;
                        logger.Info(response.responseBody);
                        logger.Info(response.StatusCode);
                        logger.Info(response.Cookie);
                        if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            response = ActiveUploadImagesViewModel.InitiateRestCall(ActiveCloudModel.LoginCookie, kvp).Result;
                        }
                        else
                        {
                            ActiveCloudModel.AnalysisFlowResponseModel.UploadResponseList.Add(response);
                            break;
                        }

                    }


                }
                logger.Info("Upload Analysis {0}", response.StatusCode);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    ManageFailureResponse(response, "Upload");

                }
                else
                {
                    ActiveCloudModel.UploadModel.CompletedStatus = true;
                    nextDirectory = DirectoryEnum.SentItemsDir;
                    IsMove2NextDir = true;
                    // StartAnalsysisFlow();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                exceptionLog.Error(Common.Exception2StringConverter.GetInstance().ConvertException2String(ex));

                ManageFailureResponse(ActiveCloudModel.AnalysisFlowResponseModel.CreateAnalysisResponse, ex.Message);
            }

        }


        /// <summary>
        /// To read and update the analysis result.
        /// </summary>
        private void GetAnalysisResult()
        {
            try
            {
                isValidLoginCookie();
                logger.Info("Get Analysis Result");
                ActiveCloudModel.GetAnalysisResultModel.Body = string.Empty;
                ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisResultResponse = ActiveGetAnalysisResultViewModel.InitiateRestCall(ActiveCloudModel.LoginCookie, new Dictionary<string, object>()).Result;
                logger.Info("Get Analysis Result status {0}", ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisResultResponse.StatusCode);

                logger.Info(JsonConvert.SerializeObject(ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisResultResponse, Formatting.Indented));

                if (ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisResultResponse.StatusCode == System.Net.HttpStatusCode.OK)

                {
                    InboxAnalysisStatusModel inboxAnalysisStatusModel = new InboxAnalysisStatusModel();
                    inboxAnalysisStatusModel.cloudID = ActiveCloudModel.cloudID;
                    inboxAnalysisStatusModel.reportID = ActiveCloudModel.reportID;
                    inboxAnalysisStatusModel.visitID = ActiveCloudModel.visitID;
                    inboxAnalysisStatusModel.patientID = ActiveCloudModel.patientID;


                    if (AnalysisType == AnalysisType.QI)
                    {
                        AddQIResult(ref inboxAnalysisStatusModel);

                    }
                    else
                    {

                        GetAnalysisData(ref inboxAnalysisStatusModel);
                        SetAIImpressions(ref inboxAnalysisStatusModel);

                    }
                    nextDirectory = DirectoryEnum.ProcessedDir;
                    Write2Inbox(inboxAnalysisStatusModel);

                }
                else
                {

                    ManageFailureResponse(ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisResultResponse, "Result");
                }
            }
            catch (Exception ex)
            {

                logger.Error(ex);
                exceptionLog.Error(Common.Exception2StringConverter.GetInstance().ConvertException2String(ex));

                ManageFailureResponse(ActiveCloudModel.AnalysisFlowResponseModel.CreateAnalysisResponse, ex.Message);
            }


        }

        //private void GetAnalysisData(ref InboxAnalysisStatusModel inboxAnalysisStatusModel)
        //{
        //    JObject Login_JObject = JObject.Parse(ActiveCloudModel.AnalysisFlowResponseModel.LoginResponse.responseBody);
        //    ActiveCloudModel.CreateAnalysisModel.installation_id = (string)Login_JObject["message"]["installation_id"];
        //    List<JToken> products = Login_JObject["message"]["products"].ToList();
        //    string sub_category = string.Empty;
        //    if (AnalysisType == AnalysisType.Fundus)
        //    {
        //        foreach (var item in products)
        //        {
        //            if (item["sub_ctgy"].ToString().ToLower().Contains("fundus"))
        //            {
        //                ActiveCloudModel.CreateAnalysisModel.product_id = (string)item["product_id"];
        //                sub_category = (string)item["sub_ctgy"];
        //            }
        //        }
        //        inboxAnalysisStatusModel.ReportUri = new System.Uri(ActiveCloudModel.GetAnalysisResultModel.URL_Model.API_URL.Replace("api", "ui") + "report/" + sub_category +
        //          "/" + ActiveCloudModel.CreateAnalysisModel.product_id + "/" + ActiveCloudModel.CreateAnalysisModel.sample_id + "/" + ActiveCloudModel.InitiateAnalysisModel.id);
        //    }



        //    JObject jObject = JObject.Parse(ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisResultResponse.responseBody);
        //    var temp = jObject["analysisData"];
        //    List<JToken> Subsections = new List<JToken>();
        //    var cnt = temp.Count();
        //    for (int i = 0; i < temp.Count(); i++)
        //    {
        //        Subsections.Add(temp[i]);

        //    }
        //    List<JToken[]> dataValues = new List<JToken[]>();
        //    for (int i = 0; i < Subsections.Count; i++)
        //    {
        //        var RightJtoken = Subsections[i].Children().Values().Last().First.ToArray().Values().ToList()[3].ToArray(); //Right Array index 0
        //        dataValues.Add(RightJtoken);
        //        if (!inboxAnalysisStatusModel.RightEyeDetails.Any())
        //        {
        //            for (int k = 0; k < RightJtoken.Length; k++)
        //            {
        //                inboxAnalysisStatusModel.RightEyeDetails.Add(new ImageAnalysisResultModel());
        //            }
        //        }

        //        var LeftJtoken = Subsections[i].Children().Values().Last().Last.ToArray().Values().ToList()[3].ToArray(); // Left Array index 1
        //        dataValues.Add(LeftJtoken);

        //        if (!inboxAnalysisStatusModel.LeftEyeDetails.Any())
        //        {
        //            for (int j = 0; j < LeftJtoken.Length; j++)
        //            {
        //                inboxAnalysisStatusModel.LeftEyeDetails.Add(new ImageAnalysisResultModel());
        //            }
        //        }

        //    }
        //    for (int i = 0; i < 2; i++)
        //    {
        //        for (int j = 0; j < dataValues[i].Length; j++)
        //        {
        //            if (i % 2 == 0)
        //            {
        //                inboxAnalysisStatusModel.RightEyeDetails[j].ImageName = dataValues[0].ToArray()[j].ToArray().First().ToString();
        //                inboxAnalysisStatusModel.RightEyeDetails[j].Analysis_Result_DR = dataValues[0].ToArray()[j].Values().Last().Last.Value<string>();
        //                inboxAnalysisStatusModel.RightEyeDetails[j].Analysis_Result_GLaucoma = dataValues[2].ToArray()[j].Values().Last().Last.Value<string>();
        //                inboxAnalysisStatusModel.RightEyeDetails[j].Analysis_Result_AMD = dataValues[4].ToArray()[j].Values().Last().Last.Value<string>();

        //                //inboxAnalysisStatusModel.RightEyeDetails[j].QI_Result_DR =
        //                //inboxAnalysisStatusModel.RightEyeDetails[j].QI_Result_AMD =
        //                //inboxAnalysisStatusModel.RightEyeDetails[j].QI_Result_Glaucoma =

        //            }
        //            else
        //            {
        //                inboxAnalysisStatusModel.LeftEyeDetails[j].ImageName = dataValues[1].ToArray()[j].ToArray().First().ToString();
        //                inboxAnalysisStatusModel.LeftEyeDetails[j].Analysis_Result_DR = dataValues[1].ToArray()[j].Values().Last().Last.Value<string>();
        //                inboxAnalysisStatusModel.LeftEyeDetails[j].Analysis_Result_GLaucoma = dataValues[3].ToArray()[j].Values().Last().Last.Value<string>();
        //                inboxAnalysisStatusModel.LeftEyeDetails[j].Analysis_Result_AMD = dataValues[5].ToArray()[j].Values().Last().Last.Value<string>();
        //                //inboxAnalysisStatusModel.LeftEyeDetails[j].QI_Result_DR =
        //                //inboxAnalysisStatusModel.LeftEyeDetails[j].QI_Result_AMD =
        //                //inboxAnalysisStatusModel.LeftEyeDetails[j].QI_Result_Glaucoma =

        //            }
        //        }

        //    }

        //    inboxAnalysisStatusModel.Status = ActiveCloudModel.GetAnalysisModel.analysis_status;

        //}

        private void GetAnalysisData(ref InboxAnalysisStatusModel inboxAnalysisStatusModel)
        {
            JObject Login_JObject = JObject.Parse(ActiveCloudModel.AnalysisFlowResponseModel.LoginResponse.responseBody);
            ActiveCloudModel.CreateAnalysisModel.installation_id = (string)Login_JObject["message"]["installation_id"];
            List<JToken> products = Login_JObject["message"]["products"].ToList();
            string sub_category = string.Empty;
            if (AnalysisType == AnalysisType.Fundus)
            {
                foreach (var item in products)
                {
                    if (item["sub_ctgy"].ToString().ToLower().Contains("fundus"))
                    {
                        ActiveCloudModel.CreateAnalysisModel.product_id = (string)item["product_id"];
                        sub_category = (string)item["sub_ctgy"];
                    }
                }
                inboxAnalysisStatusModel.ReportUri = new System.Uri(ActiveCloudModel.GetAnalysisResultModel.URL_Model.API_URL.Replace("api", "ui") + "report/" + sub_category +
                  "/" + ActiveCloudModel.CreateAnalysisModel.product_id + "/" + ActiveCloudModel.CreateAnalysisModel.sample_id + "/" + ActiveCloudModel.InitiateAnalysisModel.id);
            }



            JObject jObject = JObject.Parse(ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisResultResponse.responseBody);
            var temp = jObject["analysisData"];
            List<JToken> Subsections = new List<JToken>();
            var cnt = temp.Count();
            for (int i = 0; i < temp.Count(); i++)
                Subsections.Add(temp[i]);

            List<JToken[]> dataValues = new List<JToken[]>();
            for (int i = 0; i < Subsections.Count; i++)
            {
                var RightJtoken = Subsections[i].Children().Values().Last().First.ToArray().Values().ToList()[3].ToArray(); //Right Array index 0

                //dataValues.Add(RightJtoken);
                if (!inboxAnalysisStatusModel.RightEyeDetails.Any())
                {
                    for (int k = 0; k < RightJtoken.Length; k++)
                    {
                        ImageAnalysisResultModel rightImageAnalysisResultModel = new ImageAnalysisResultModel();
                        var testValue = RightJtoken[k].ToArray()[0].ToString();
                        rightImageAnalysisResultModel.ImageName = testValue;
                        //rightImageAnalysisResultModel.Analysis_Result_DR = RightJtoken[k].ToArray().Last().Children().ToList()[0].First.ToString();
                        //rightImageAnalysisResultModel.Analysis_Result_GLaucoma = RightJtoken[k].ToArray().Last().Children().ToList()[0].First.ToString();
                        //rightImageAnalysisResultModel.Analysis_Result_AMD = RightJtoken[k].ToArray().Last().Children().ToList()[0].First.ToString();
                        rightImageAnalysisResultModel.ShowInSummary = Convert.ToBoolean(RightJtoken[k].ToList()[5].Children().Values().ToList()[0].ToString());
                        inboxAnalysisStatusModel.RightEyeDetails.Add(rightImageAnalysisResultModel);
                    }
                }

                var LeftJtoken = Subsections[i].Children().Values().Last().Last.ToArray().Values().ToList()[3].ToArray(); // Left Array index 1
                //dataValues.Add(LeftJtoken);

                if (!inboxAnalysisStatusModel.LeftEyeDetails.Any())
                {
                    for (int j = 0; j < LeftJtoken.Length; j++)
                    {
                        ImageAnalysisResultModel leftImageAnalysisResultModel = new ImageAnalysisResultModel();
                        leftImageAnalysisResultModel.ImageName = LeftJtoken[j].ToArray()[0].ToString();
                        //leftImageAnalysisResultModel.Analysis_Result_DR = LeftJtoken[j].ToArray().Last().Children().ToList()[0].First.ToString();
                        //leftImageAnalysisResultModel.Analysis_Result_GLaucoma = LeftJtoken[j].ToArray().Last().Children().ToList()[0].First.ToString();
                        //leftImageAnalysisResultModel.Analysis_Result_AMD = LeftJtoken[j].ToArray().Last().Children().ToList()[0].First.ToString();
                        leftImageAnalysisResultModel.ShowInSummary = Convert.ToBoolean(LeftJtoken[j].ToList()[5].Children().Values().ToList()[0].ToString());
                        inboxAnalysisStatusModel.LeftEyeDetails.Add(leftImageAnalysisResultModel);

                    }

                }

            }
            //for (int i = 0; i < 2; i++)
            //{
            //    for (int j = 0; j < dataValues[i].Length; j++)
            //    {
            //        if (i % 2 == 0)
            //        {


            //            //inboxAnalysisStatusModel.RightEyeDetails[j].QI_Result_DR =
            //            //inboxAnalysisStatusModel.RightEyeDetails[j].QI_Result_AMD =
            //            //inboxAnalysisStatusModel.RightEyeDetails[j].QI_Result_Glaucoma =

            //        }
            //        else
            //        {
            //            inboxAnalysisStatusModel.LeftEyeDetails[j].ImageName = dataValues[1].ToArray()[j].ToArray().First().ToString();
            //            inboxAnalysisStatusModel.LeftEyeDetails[j].Analysis_Result_DR = dataValues[1].ToArray()[j].Values().Last().Last.Value<string>();
            //            //inboxAnalysisStatusModel.LeftEyeDetails[j].QI_Result_DR =
            //            //inboxAnalysisStatusModel.LeftEyeDetails[j].QI_Result_AMD =
            //            //inboxAnalysisStatusModel.LeftEyeDetails[j].QI_Result_Glaucoma =

            //        }
            //    }

            //}

            inboxAnalysisStatusModel.Status = ActiveCloudModel.GetAnalysisModel.analysis_status;

        }

        private void SetAIImpressions(ref InboxAnalysisStatusModel inboxAnalysisStatusModel)
        {
            //#region Left Eye Impressions
            //if (!inboxAnalysisStatusModel.LeftEyeDetails.Any(x => x.Analysis_Result_DR.Equals("PDR")))
            //{
            //    if (!inboxAnalysisStatusModel.LeftEyeDetails.Any(x => x.Analysis_Result_DR.Equals("NPDR")))
            //    {
            //        if (!inboxAnalysisStatusModel.LeftEyeDetails.Any(x => x.Analysis_Result_DR.Equals("NonGradable")))
            //            inboxAnalysisStatusModel.LeftAIImpressionsDR = "Non-Referrable DR";
            //        else
            //            inboxAnalysisStatusModel.LeftAIImpressionsDR = "Non-Gradable";

            //    }
            //    else
            //    {
            //        inboxAnalysisStatusModel.LeftAIImpressionsDR = "Referrable DR";
            //    }
            //}
            //else
            //{
            //    inboxAnalysisStatusModel.LeftAIImpressionsDR = "Referrable DR";

            //}
            //if (!inboxAnalysisStatusModel.LeftEyeDetails.Any(x => x.Analysis_Result_AMD.ToLower().Equals("amd")))
            //{
            //    if (!inboxAnalysisStatusModel.LeftEyeDetails.Any(x => x.Analysis_Result_AMD.Equals("NonGradable")))
            //        inboxAnalysisStatusModel.LeftAIImpressionsAMD = "Non-Referrable AMD";
            //    else
            //        inboxAnalysisStatusModel.LeftAIImpressionsAMD = "Non-Gradable";
            //}
            //else
            //{
            //    inboxAnalysisStatusModel.LeftAIImpressionsAMD = "Referrable AMD";

            //}

            //if (!inboxAnalysisStatusModel.LeftEyeDetails.Any(x => x.Analysis_Result_GLaucoma.Equals("Glaucoma")))
            //{
            //    if (!inboxAnalysisStatusModel.LeftEyeDetails.Any(x => x.Analysis_Result_GLaucoma.Equals("NonGradable")))
            //        inboxAnalysisStatusModel.LeftAIImpressionsGlaucoma = "Non-Referrable Glaucoma";
            //    else
            //        inboxAnalysisStatusModel.LeftAIImpressionsGlaucoma = "Non-Gradable";

            //}
            //else
            //{
            //    inboxAnalysisStatusModel.LeftAIImpressionsGlaucoma = "Referrable Glaucoma";
            //}

            //#endregion

            //if (!inboxAnalysisStatusModel.RightEyeDetails.Any(x => x.Analysis_Result_DR.Equals("PDR")))
            //{
            //    if (!inboxAnalysisStatusModel.RightEyeDetails.Any(x => x.Analysis_Result_DR.Equals("NPDR")))
            //    {
            //        if (!inboxAnalysisStatusModel.RightEyeDetails.Any(x => x.Analysis_Result_DR.Equals("NonGradable")))
            //            inboxAnalysisStatusModel.RightAIImpressionsDR = "Non-Referrable DR";
            //        else
            //            inboxAnalysisStatusModel.RightAIImpressionsDR = "Non-Gradable";

            //    }
            //    else
            //    {
            //        inboxAnalysisStatusModel.RightAIImpressionsDR = "Referrable DR";
            //    }
            //}
            //else
            //{
            //    inboxAnalysisStatusModel.RightAIImpressionsDR = "Referrable DR";

            //}
            //if (!inboxAnalysisStatusModel.RightEyeDetails.Any(x => x.Analysis_Result_AMD.ToLower().Equals("amd")))
            //{
            //    if (!inboxAnalysisStatusModel.RightEyeDetails.Any(x => x.Analysis_Result_AMD.Equals("NonGradable")))
            //        inboxAnalysisStatusModel.RightAIImpressionsAMD = "Non-Referrable AMD";
            //    else
            //        inboxAnalysisStatusModel.RightAIImpressionsAMD = "Non-Gradable";

            //}
            //else
            //{
            //    inboxAnalysisStatusModel.RightAIImpressionsAMD = "Referrable AMD";

            //}

            //if (!inboxAnalysisStatusModel.RightEyeDetails.Any(x => x.Analysis_Result_GLaucoma.Equals("Glaucoma")))
            //{
            //    if (!inboxAnalysisStatusModel.RightEyeDetails.Any(x => x.Analysis_Result_GLaucoma.Equals("NonGradable")))
            //        inboxAnalysisStatusModel.RightAIImpressionsGlaucoma = "Non-Referrable Glaucoma";
            //    else
            //        inboxAnalysisStatusModel.RightAIImpressionsGlaucoma = "Non-Gradable";

            //}
            //else
            //{
            //    inboxAnalysisStatusModel.RightAIImpressionsGlaucoma = "Referrable Glaucoma ";
            //}
            inboxAnalysisStatusModel.RightAIImpressionsDR = RightEyeComments;
            inboxAnalysisStatusModel.LeftAIImpressionsDR = LeftEyeComments;
            inboxAnalysisStatusModel.Reject_Message = RejectComments;

            
        }
        private void AddQIResult(ref InboxAnalysisStatusModel inboxAnalysisStatusModel)
        {
            JObject Login_JObject = JObject.Parse(ActiveCloudModel.AnalysisFlowResponseModel.LoginResponse.responseBody);
            ActiveCloudModel.CreateAnalysisModel.installation_id = (string)Login_JObject["message"]["installation_id"];
            List<JToken> products = Login_JObject["message"]["products"].ToList();
            string sub_category = string.Empty;


            JObject jObject = JObject.Parse(ActiveCloudModel.AnalysisFlowResponseModel.GetAnalysisResultResponse.responseBody);
            var temp = jObject["analysisData"];
            List<JToken> Subsections = new List<JToken>();
            var cnt = temp.Count();
            for (int i = 0; i < temp.Count(); i++)
            {
                Subsections.Add(temp[i]);

            }
            bool isRight = false;
            List<JToken[]> dataValues = new List<JToken[]>();
            for (int i = 0; i < Subsections.Count; i++)
            {
                var RightJtoken = Subsections[i].Children().Values().Last().First.ToArray().Values().ToList()[3].ToArray(); //Right Array index 0
                if (RightJtoken.Any())
                {
                    dataValues.Add(RightJtoken);
                    isRight = true;
                }
                if (!inboxAnalysisStatusModel.RightEyeDetails.Any())
                {
                    for (int k = 0; k < RightJtoken.Length; k++)
                    {
                        inboxAnalysisStatusModel.RightEyeDetails.Add(new ImageAnalysisResultModel());
                    }
                }

                var LeftJtoken = Subsections[i].Children().Values().Last().Last.ToArray().Values().ToList()[3].ToArray(); // Left Array index 1
                if (LeftJtoken.Any())
                    dataValues.Add(LeftJtoken);

                if (!inboxAnalysisStatusModel.LeftEyeDetails.Any())
                {
                    for (int j = 0; j < LeftJtoken.Length; j++)
                    {
                        inboxAnalysisStatusModel.LeftEyeDetails.Add(new ImageAnalysisResultModel());
                    }
                }

            }
            {
                if (isRight)
                {
                    inboxAnalysisStatusModel.RightEyeDetails[0].QI_Result_DR = dataValues[0].ToArray()[0].ToArray().First().ToString();
                    inboxAnalysisStatusModel.RightEyeDetails[0].QI_Result_AMD = dataValues[0].ToArray()[0].ToArray().First().ToString();
                    inboxAnalysisStatusModel.RightEyeDetails[0].QI_Result_Glaucoma = dataValues[0].ToArray()[0].ToArray().First().ToString();

                }
                else
                {
                    inboxAnalysisStatusModel.LeftEyeDetails[0].QI_Result_DR = dataValues[0].ToArray()[0].ToArray().First().ToString();
                    inboxAnalysisStatusModel.LeftEyeDetails[0].QI_Result_AMD = dataValues[0].ToArray()[0].ToArray().First().ToString();
                    inboxAnalysisStatusModel.LeftEyeDetails[0].QI_Result_Glaucoma = dataValues[0].ToArray()[0].ToArray().First().ToString();
                }

            }

            inboxAnalysisStatusModel.Status = ActiveCloudModel.GetAnalysisModel.analysis_status;
        }
        /// <summary>
        /// To manage the failure cases of analysis result.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="stage"></param>
        /// <param name="failureMessage"></param>
        private void ManageFailureResponse(Response_CookieModel response, string stage, string failureMessage = null)
        {
            logger.Info("Iam Stage {0} Status Code {1}", stage, response.StatusCode);
            var failureObj = JObject.Parse(response.responseBody);
            if (string.IsNullOrEmpty(failureMessage))
                failureMessage = (string)failureObj["message"];
            if (response.StatusCode == 0)
            {
                logger.Info("Internet Connection present = {GlobalVariables.isInternetPresent}");
                StreamWriter st1 = null;
                try
                {
                    IsMove2NextDir = false;
                    //st1 =   new StreamWriter(ActiveFnf.FullName, false);
                    //st1.Write(JsonConvert.SerializeObject(ActiveCloudModel, Formatting.Indented));

                }
                catch (Exception exp)
                {
                    logger.Error(exp);
                    exceptionLog.Error(Common.Exception2StringConverter.GetInstance().ConvertException2String(exp));


                }
                finally
                {
                    if (st1 != null)
                    {
                        st1.Flush();
                        st1.Close();
                        st1.Dispose();

                    }
                    //startStopEvent(true);
                    //this.IsBusy = false; ;
                }





            }

            else if (response.StatusCode == System.Net.HttpStatusCode.BadGateway)
            {

                logger.Info("Retry");
                StreamWriter st1 = null;
                try
                {
                    IsMove2NextDir = false;
                    //st1 = new StreamWriter(ActiveFnf.FullName, false);
                    //st1.Write(JsonConvert.SerializeObject(ActiveCloudModel, Formatting.Indented));

                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    exceptionLog.Error(Common.Exception2StringConverter.GetInstance().ConvertException2String(ex));

                }

                finally
                {
                    if (st1 != null)
                    {
                        st1.Flush();
                        st1.Close();
                        st1.Dispose();
                        st1.Dispose();

                    }
                    //startStopEvent(true);
                    // this.IsBusy = false; ;

                }

            }
            else
            {

                logger.Info("Move file to Read");
                InboxAnalysisStatusModel inboxAnalysisStatusModel = new InboxAnalysisStatusModel();
                inboxAnalysisStatusModel.Status = "failure";
                inboxAnalysisStatusModel.StatusCode = response.StatusCode;
                inboxAnalysisStatusModel.FailureMessage = failureMessage;
                nextDirectory = DirectoryEnum.ProcessedDir;
                Write2Inbox(inboxAnalysisStatusModel);




                //this.Dispose();
            }
        }


        /// <summary>
        /// To write the file to inbox.
        /// </summary>
        /// <param name="inboxAnalysisStatusModel"></param>
        private void Write2Inbox(InboxAnalysisStatusModel inboxAnalysisStatusModel)
        {
            StreamWriter st = null;
            try
            {
                inboxAnalysisStatusModel.DoctorDetailsForUploadModel = ActiveCloudModel.DoctorDetailsForUploadModel;
                st = new StreamWriter(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.InboxDir, AnalysisType), ActiveFnf.Name), false);
                st.Write(JsonConvert.SerializeObject(inboxAnalysisStatusModel, Formatting.Indented));
                st.Flush();
                st.Close();
                st.Dispose();
                IsMove2NextDir = true;
                // MoveFile2ProcessedDir();




            }
            catch (Exception ex)
            {
                exceptionLog.Error(Common.Exception2StringConverter.GetInstance().ConvertException2String(ex));

            }
            finally
            {

                //if (File.Exists(ActiveFnf.FullName))
                //    File.Delete(ActiveFnf.FullName);

                st = new StreamWriter(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.InboxDir, AnalysisType), ActiveFnf.Name.Split('.')[0] + "_pending"), false);
                st.Flush();
                st.Close();
                st.Dispose();
                //startStopEvent(true); 
                //this.IsBusy = false; ;

            }
        }
        private void MoveFile2ProcessedDir()
        {
            try
            {
                if (!File.Exists(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.ProcessedDir, AnalysisType), ActiveFnf.Name)))
                {

                    //StreamWriter st = new StreamWriter(Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.ProcessedDir, AnalysisType), ActiveFnf.Name), false);
                    StreamWriter st = new StreamWriter(ActiveFnf.FullName, false);
                    st.Write(JsonConvert.SerializeObject(ActiveCloudModel, Formatting.Indented));
                    st.Flush();
                    st.Close();
                    st.Dispose();
                    File.Move(ActiveFnf.FullName, Path.Combine(GlobalMethods.GetDirPath(DirectoryEnum.ProcessedDir, AnalysisType), ActiveFnf.Name));
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        private void isValidLoginCookie()
        {
            if (ActiveCloudModel.LoginCookie.Expires < DateTime.Now)
            {
                logger.Info("In cookie validation");
                Login();
            }


        }
        public AnalysisViewModel ActiveCreateAnalysisViewModel
        {
            get => activeCreateAnalysisViewModel;
            set
            {
                activeCreateAnalysisViewModel = value;
                OnPropertyChanged("ActiveCreateAnalysisViewModel");
            }
        }

        public AnalysisViewModel ActiveUploadImagesViewModel
        {
            get => activeUploadImagesViewModel;
            set
            {
                activeUploadImagesViewModel = value;
                OnPropertyChanged("ActiveUploadImagesViewModel");
            }
        }

        private AnalysisViewModel activeNotifyEmailViewModel;

        public AnalysisViewModel ActiveNotifyEmailViewModel
        {
            get { return activeNotifyEmailViewModel; }
            set
            {
                activeNotifyEmailViewModel = value;
                OnPropertyChanged("ActiveNotifyEmailViewModel");
            }
        }

        private AnalysisViewModel activeDoctorCommentsViewModel;

        public AnalysisViewModel ActiveDoctorCommentsViewModel
        {
            get { return activeDoctorCommentsViewModel; }
            set
            {
                activeDoctorCommentsViewModel = value;
                OnPropertyChanged("ActiveDoctorCommentsViewModel");
            }
        }

        private AnalysisViewModel activeGetStatusPostDoctorApprovalViewModel;

        public AnalysisViewModel ActiveGetStatusPostDoctorApprovalViewModel
        {
            get { return activeGetStatusPostDoctorApprovalViewModel; }
            set
            {
                activeGetStatusPostDoctorApprovalViewModel = value;
                OnPropertyChanged("ActiveGetStatusPostDoctorApprovalViewModel");
            }
        }

        public AnalysisViewModel ActiveIntiateAnalysisViewModel
        {
            get => activeIntiateAnalysisViewModel;
            set
            {
                activeIntiateAnalysisViewModel = value;
                OnPropertyChanged("ActiveIntiateAnalysisViewModel");
            }
        }

        public AnalysisViewModel ActiveGetStatusAnalysisViewModel
        {
            get => activeGetStatusAnalysisViewModel;
            set
            {
                activeGetStatusAnalysisViewModel = value;
                OnPropertyChanged("ActiveGetStatusAnalysisViewModel");
            }
        }

        public AnalysisViewModel ActiveGetAnalysisResultViewModel
        {
            get => activeGetAnalysisResultViewModel;
            set
            {
                activeGetAnalysisResultViewModel = value;
                OnPropertyChanged("ActiveGetAnalysisResultViewModel");

            }
        }

        private AnalysisViewModel activeApproveDoctorViewmodel;

        public AnalysisViewModel ActiveApproveDoctorViewmodel
        {
            get
            {
                return activeApproveDoctorViewmodel;
            }
            set
            {
                activeApproveDoctorViewmodel = value;
                OnPropertyChanged("ActiveApproveDoctorViewmodel");
            }
        }

        private AnalysisType analysisType;

        public AnalysisType AnalysisType
        {
            get { return analysisType; }
            set
            {
                analysisType = value;
                OnPropertyChanged("AnalysisType");
            }
        }

        public bool IsMove2NextDir
        {
            get => isMove2NextDir;
            set
            {
                isMove2NextDir = value;
                if (Write_R_Move_File_Event != null)
                    Write_R_Move_File_Event(this.AnalysisType);
            }
        }

        public void SetValueMethod(object param)
        {
            //this.FileUploadStatus = 100;
        }

    }
}
