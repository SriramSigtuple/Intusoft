using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Handlers;
using Cloud_Models.Models;
using System.Text;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using NLog;

namespace IntuUploader.Utilities
{
    public class RESTClientHelper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static RESTClientHelper clientHelper;

        /// <summary>
        /// Singleton Implementation for RESTClientHelper
        /// </summary>
        /// <returns>returns the singleton instance of RESTClientHelper </returns>
        public static RESTClientHelper GetInstance()

        {
            logger.Info("");

            if (clientHelper == null)
                clientHelper = new RESTClientHelper();

            GlobalVariables.eventLog.Info("Get Singleton Instance of Rest Client Helper");
            logger.Info("");

            return clientHelper;

        }
        /// <summary>
        /// Constructor
        /// </summary>
        private RESTClientHelper()
        {

        }   
        public async Task<Response_CookieModel> RestCall( BaseCloudModel model, Cookie cookie, Dictionary<string,object> keyValuePairs)
        {
            logger.Info("");

            string responseMsg = null;
            Response_CookieModel response_cookie = new Response_CookieModel();
            // reference https://stackoverflow.com/questions/19954287/how-to-upload-file-to-server-with-http-post-multipart-form-data
            //reference https://stackoverflow.com/questions/10679214/how-do-you-set-the-content-type-header-for-an-httpclient-request

            try
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                CookieContainer cookies = new CookieContainer();
                if (!string.IsNullOrEmpty(cookie.Value))
                cookies.Add(cookie);
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = cookies;

                HttpResponseMessage response = null;

                var client = new HttpClient(handler);
                MultipartFormDataContent form = null;


                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = model.MethodType,
                    RequestUri = new Uri(model.URL),
                    Content = new StringContent(model.Body, Encoding.UTF8, model.ContentType),
                    Headers =
                    {
                          { "X-Version", "1" },
                        { HttpRequestHeader.Accept.ToString(), "application/json" }
                    }

                };
                client.BaseAddress = httpRequestMessage.RequestUri;
                //reference https://social.msdn.microsoft.com/Forums/en-US/f553e3fb-9007-42e9-8289-9bf0e52c0e07/set-content-type-in-httpclienthttprequestmessage-throws-exception?forum=winappswithcsharp
                    httpRequestMessage.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(model.ContentType);
                if(keyValuePairs.ContainsKey("checksum"))
                client.DefaultRequestHeaders.Add("checksum", keyValuePairs["checksum"].ToString());
                // Setting of Content Type to application/json or multipart/form-data
                httpRequestMessage.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(model.ContentType);

                if (model.BodyMessageType != "raw")
                {
                    form = new MultipartFormDataContent();
                    foreach (var item in keyValuePairs)
                    {
                        if (item.Value is string)
                        {
                            //if(item.Key.Contains("checksum"))
                            //{
                            //    httpRequestMessage.Content.Headers.Add(item.Key, item.Value.ToString());
                            //}
                            //else
                            form.Add(new StringContent(item.Value.ToString()), item.Key);

                        }
                        else
                        {
                            FileInfo finf = (FileInfo)item.Value;
                            var stream = new FileStream(finf.FullName, FileMode.Open);

                            HttpContent  content = new StreamContent(stream);
                            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                            {
                                Name = "image",
                                FileName = finf.Name
                            };
                            form.Add(content, "image");
                        }
                           

                    }

                }
                switch (model.MethodType.Method)
                {
                    case "POST":
                        {
                            if(form == null)
                            response = await client.SendAsync(httpRequestMessage);
                            else
                            response = await client.PostAsync(model.URL, form);
                            break;
                        }
                    case "GET":
                        {
                            response = await client.GetAsync(model.URL);
                            break;
                        }
                    default:
                        break;
                }
                response_cookie.responseBody = await response.Content.ReadAsStringAsync();
                response_cookie.StatusCode = response.StatusCode;
                if (response.StatusCode == HttpStatusCode.OK)
                {

                    //reference https://stackoverflow.com/questions/29224734/how-to-read-cookies-from-httpresponsemessage/29224955
                    var responseCookies = cookies.GetCookies(client.BaseAddress).Cast<Cookie>();
                    List<Cookie> c = responseCookies.ToList();

                    if (c.Any())
                        response_cookie.Cookie = c[0];
                   
                }
                else if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    response_cookie.Cookie = null; 
                }
            }
            catch (Exception ex)
            {
                Response_CookieModel r = new Response_CookieModel();
                r.responseBody = "Check internet Connection";
                responseMsg = JsonConvert.SerializeObject(r);
            }

            //JToken token = (JToken)JsonConvert.DeserializeObject(responseMsg);
            //return token;
            logger.Info("");

            return response_cookie;
        }

        public string GetBodyForREST(Dictionary<string,object> inputDictionary)
        {
            logger.Info("");

            //reference https://stackoverflow.com/questions/5597349/how-do-i-convert-a-dictionary-to-a-json-string-in-c
            var convertedDictionary = inputDictionary.ToDictionary(item => item.Key.ToString(), item => item.Value.ToString()); //This converts your dictionary to have the Key and Value of type string.
            logger.Info("");

            return JsonConvert.SerializeObject(convertedDictionary);
        }
        public void PostImage(ref MultipartFormDataContent form, KeyValuePair<string,object> fileNameKVP)
        {
            logger.Info("");

            //HttpClient httpClient = new HttpClient();
            //MultipartFormDataContent form = new MultipartFormDataContent();

            byte[] imagebytearraystring = ImageFileToByteArray((FileInfo)fileNameKVP.Value);
            form.Add(new ByteArrayContent(imagebytearraystring, 0, imagebytearraystring.Length), fileNameKVP.Key,((FileInfo) (fileNameKVP.Value)).Name);
            //HttpResponseMessage response = httpClient.PostAsync("your url", form).Result;

            //httpClient.Dispose();
            //string sd = response.Content.ReadAsStringAsync().Result;
            logger.Info("");

        }

        private byte[] ImageFileToByteArray(FileInfo finf)
        {
            logger.Info("");

            FileStream fs = File.OpenRead(finf.FullName);
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
            fs.Close();
            logger.Info("");

            return bytes;
        }


    }
}


    
