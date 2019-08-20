using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using BaseViewModel;
using System.Threading;
using System.Net.Http.Handlers;
using Cloud_Models.Models;

namespace IVLUploader.ViewModels
{
   public class FileUploadHelper:ViewBaseModel
    {

        private const string emailNotRegisterd = "This email is not registered";
        private const string incorrectPassword = "This password is not correct";
        private const string loggedInSuccessfully = "You are logged in successfully";
        private const string serverIsRunning = "Server is Running";
        public static string fileUploadedSuccessfully = "Your Data has been uploaded Successfully";
        private static FileUploadHelper _fileUploadHelper;

        private string loginToken = string.Empty;
        //public string LoginURL = "http://chironapp.chironx.cloud/api/chironx/authenticate/login";
        //public string UploadURL = "http://chironapp.chironx.cloud/api/chironx/upload/Data?q=Intuvision";
        //public string ServerStatusURL = "http://chironapp.chironx.cloud/api/chironx/status";
        private bool isLogin = false;

        public bool IsLogin
        {
            get { return isLogin; }
            set {
                isLogin = value;
                if (isLogin)
                {
                    OnPropertyChanged("IsLogin");
                }
            }
        }
       private  bool isServerRunning = false;
        public  bool IsServerRunning
        {
            get { 
                return isServerRunning; }
            set { 
                isServerRunning = value;
                OnPropertyChanged("IsServerRunning");
                }
        }

        

        public static  FileUploadHelper GetInstance()
        {
                if (_fileUploadHelper == null)
                    _fileUploadHelper = new FileUploadHelper();
                return FileUploadHelper._fileUploadHelper;
        }
       private FileUploadHelper()
       {
           //UpdateServerStatus();
       }


       private async Task<JToken> Login(Dictionary<string, string> credentials, string URL)
       {
           string responseMsg = null;

           try
           {
               HttpClient p = new HttpClient();
               HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new System.Uri(URL));
               var values = new List<KeyValuePair<string, string>>();

               foreach (KeyValuePair<string, string> item in credentials)
               {
                   values.Add(item);
               }
               //values.Add(new KeyValuePair<string, string>("email", "sriram@intuvisionlabs.com"));
               //values.Add(new KeyValuePair<string, string>("hashedPassword", "intuvision123"));

               request.Content = new FormUrlEncodedContent(values);
               HttpResponseMessage response = await p.SendAsync(request);
               if (response.IsSuccessStatusCode)
               {
                   responseMsg = await response.Content.ReadAsStringAsync();
               }
              
           }
           catch (Exception)
           {
               Response_CookieModel r = new Response_CookieModel();
               r.responseBody = "Check internet Connection";
               //r.status = 100;
               responseMsg = JsonConvert.SerializeObject(r);
           }
           return (JToken)JsonConvert.DeserializeObject(responseMsg);


       }
       public void UpdateServerStatus()
       {
           //ThreadPool.QueueUserWorkItem(new WaitCallback(f=>
           //{
           //var releases = GetServerStatus();
           //Dictionary<string, string> resultValuesDic = new Dictionary<string, string>();
           //foreach (JProperty item in releases.Result.Children())
           //{
           //    resultValuesDic.Add(item.Name, item.Value.ToString());
           //}
           //    if(resultValuesDic.ContainsKey("status"))
           //{
           //    if(resultValuesDic["status"]== "200")
           //    {
           //        if (resultValuesDic["message"].Contains(serverIsRunning))
           //            IsServerRunning = true;
           //        else
           //            IsServerRunning = false;
           //    }
           //    else
           //        IsServerRunning = false;
           //}
           //}));
       }

       public void Login2Server()
       {
           ThreadPool.QueueUserWorkItem(new WaitCallback(p =>
           {
               Dictionary<string, string> loginCredentials = new Dictionary<string, string>();
               loginCredentials.Add("username", "netraimage");
               loginCredentials.Add("password", "31jC4Smj");
               loginCredentials.Add("apiRequestType", "Hi");
               var releases = Login(loginCredentials, "http://netraservice.azurewebsites.net/login");
               List<JToken> resultList = releases.Result.Children().ToList();
               Dictionary<string, string> resultValuesDic = new Dictionary<string, string>();
               foreach (JProperty item in releases.Result.Children())
               {
                   resultValuesDic.Add(item.Name, item.Value.ToString());
               }
               if (resultValuesDic.ContainsKey("status"))// && status.Value.ToString() == "200")
               {
                   if (resultValuesDic["status"] == "200")
                   {
                       if (resultValuesDic["message"].Contains(loggedInSuccessfully))
                       {
                           loginToken = (resultList[1] as JProperty).Value.ToString();
                           IsLogin = true;
                       }
                       else
                           IsLogin = false;
                   }
                   else
                   IsLogin = false;
               }
               else
               {
                   loginToken = string.Empty;
                   IsLogin = false;
               }
           }));
       }
       //private async Task<JToken> GetServerStatus()
       //{
       //    string responseMsg = null;

       //    try
       //    {
       //        HttpClient p = new HttpClient();
       //        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new System.Uri(UploaderDetails.uploadValues.ServerStatusURL));
       //        HttpResponseMessage response = await p.SendAsync(request);
       //        if (response.IsSuccessStatusCode)
       //        {
       //            responseMsg = await response.Content.ReadAsStringAsync();
       //        }
       //    }
       //    catch (Exception ex)
       //    {
       //        Response_CookieModel r = new Response_CookieModel();
       //        r.message = "Check internet Connection";
       //        r.status = 100;
       //        responseMsg = JsonConvert.SerializeObject(r);
       //    }
       //    return (JToken)JsonConvert.DeserializeObject(responseMsg);
         
       //}
       public async Task<JToken> UploadFiles(FileStream stream,string fileName )
       {
           //HttpClient p = new HttpClient();
           string responseMsg = string.Empty;
           try
           {
               ProgressMessageHandler processMessageHander = new ProgressMessageHandler();
               processMessageHander.HttpSendProgress += ProcessMessageHander_HttpSendProgress;
               HttpClient p = HttpClientFactory.Create(processMessageHander);
               HttpResponseMessage response = new HttpResponseMessage(); 
                   p.DefaultRequestHeaders.Add("Authorization", "Bearer " + loginToken);
                   MultipartFormDataContent form = new MultipartFormDataContent();
                   HttpContent content = new StringContent("images");
                   form.Add(content, "images");


                   content = new StreamContent(stream);
                   content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                   {
                       Name = "images",
                       FileName = fileName
                   };
                   form.Add(content);
                       form.Add(new StringContent(UploaderDetails.uploadValues.device_id), "HardwareId");

                   response = await p.PostAsync(UploaderDetails.uploadValues.UploadURL, form);


               if (response.IsSuccessStatusCode)
               {
                   responseMsg = await response.Content.ReadAsStringAsync();
               }
           }
           catch (Exception)
           {
               Response_CookieModel r = new Response_CookieModel();
               //r.message = "Check internet Connection";
               //r.status = 100;
               responseMsg = JsonConvert.SerializeObject(r);
           }
           
           return (JToken)JsonConvert.DeserializeObject(responseMsg);
       }

        private int progress = 0;
        public int Progress
        {
            get
            {
                return progress;
            }

            set
            {
                progress = value;
                OnPropertyChanged("Progress");
            }
        }
        private void ProcessMessageHander_HttpSendProgress(object sender, HttpProgressEventArgs e)
        {
            Console.WriteLine(e.ProgressPercentage);
          Progress =  e.ProgressPercentage;
        }
    }

   public static class UploaderDetails
   {
       public static UploaderModel uploadValues;
   }
   
  
   
}
