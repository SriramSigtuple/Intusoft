using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.IO;
using Common;
using ReportUtils;
namespace IVLReport
{
   public static class UploadImages
    {
     static  string vendorVal = string.Empty;
     static string AIUserName = string.Empty;
     static string AIPassword = string.Empty;
     static string AIApiRequestType = string.Empty;
       public static void UploadImagesDetails(List<KeyValuePair<string,string>> Details, string VendorVal, string userName, string password, string apiRequestType)
       {
           vendorVal = VendorVal;
           AIPassword = password;
           AIUserName = userName;
           AIApiRequestType = apiRequestType;

          Upload(Details);
       }
      static  string token = "";
     static JToken result;
     static DataModel _dataModel;

     public delegate void WaitCursor();
     public static event WaitCursor _WaitCursor;
       public static async void Upload(List<KeyValuePair<string, string>> Details)
        {
            System.Uri myUri = new System.Uri("https://api.netra.ai/v1/getToken");
                //HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(myUri);
                //webRequest.Method = "POST";
                //webRequest.ContentType = "application/x-www-form-urlencoded";
                //webRequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), webRequest);
            _dataModel = DataModel.GetInstance();   
         if (vendorVal == "Vendor3")
            {
             int imgs = 0;
             bool present = false;
                for (int i = 0; i < _dataModel.CurrentImgFiles.Length; i++)
                {
                    if (_dataModel.CurrentImageNames[i].Contains("OS"))
                        imgs++;
                    if (_dataModel.CurrentImageNames[i].Contains("OD"))
                        present = true;
                }
                if (imgs == 1 && present)
                {
                    List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();
                    values.Add(new KeyValuePair<string, string>("username", AIUserName));
                    values.Add(new KeyValuePair<string, string>("password", AIPassword));
                    values.Add(new KeyValuePair<string, string>("apiRequestType", AIApiRequestType));

                    result = await GetTokenFromApi("http://netraservice.azurewebsites.net/login", values);
                    PostData(Details);
                }
                else
                {
                    CustomMessageBox.Show("Please select one left image and one right image", "Warning", CustomMessageBoxButtons.OK, CustomMessageBoxIcon.Warning, 442, 135);
                    _WaitCursor();
                }
            }

            else if (vendorVal == "Vendor2")
            {
                GetToken();
                System.Threading.Thread.Sleep(5000);
                int count = 0;
                foreach (JProperty release in result)
                {
                    if (count == 1)
                        token = release.Value.ToString();
                    count++;
                }
                for (int i = 0; i < _dataModel.CurrentImgFiles.Length; i++)
                {
                    ImageData imageDataArr = new ImageData();
                    FileInfo finf = new FileInfo(_dataModel.CurrentImgFiles[i]);
                    imageDataArr.baseName = finf.Name;
                    //imageDataArr[0].imgData = "data:image/jpeg;base64," + ConvertImageURLToBase64(finf.FullName);
                    imageDataArr.imgData = ConvertImageURLToBase64(finf.FullName);
                    string ImgKey = string.Empty;
                    string ImgNameKey = string.Empty;
                    if (_dataModel.CurrentImageNames[i].Contains("OS"))
                    {
                        ImgKey = "imgLeftData";
                        ImgNameKey = "imgLeftName";
                    }
                    else
                    {
                        ImgKey = "imgRightData";
                        ImgNameKey = "imgRightName";
                    }
                    Details.Add(new KeyValuePair<string, string>(ImgKey, imageDataArr.imgData));
                    Details.Add(new KeyValuePair<string, string>(ImgNameKey, finf.Name));
                    //string imgDataStr = JsonConvert.SerializeObject( imageDataArr);

                }
                //string base64URL = ConvertImageURLToBase64("");
                ////base64URL = "data:image/jpeg;base64," + base64URL;
                //  Console.WriteLine(base64URL);
                SendImage(token, "", Details);
            }
        }
        public static async void GetToken()
        {
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>("username", "anil@intuvisionlabs.com"));
            values.Add(new KeyValuePair<string, string>("password", "Gt#8$DkMy"));
            values.Add(new KeyValuePair<string, string>("apiRequestType", "DIAB_RETINA"));

             result = await PostFormUrlEncoded("https://api.netra.ai/v1/getToken", values);

           
            Console.WriteLine(result);
        }
        public static  void SendImage(string token, string base64Str ,List<KeyValuePair<string,string>> Details)
        {
            Details.Add(new KeyValuePair<string, string>("token", token));
            uploadImage(Details);
            
        }

        

        public static async Task<JToken> PostFormUrlEncoded(string url, IEnumerable<KeyValuePair<string, string>> postData)
        {
            using (var httpClient = new HttpClient())
            {
                using (var content = new FormUrlEncodedContent(postData))
                {
                    content.Headers.Clear();
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    string resultResponse = string.Empty;
                    resultResponse = await response.Content.ReadAsStringAsync();
                    return  (JToken)JsonConvert.DeserializeObject(resultResponse);
                }

            }
        }
        static void uploadImage(List<KeyValuePair<string,string>> uploadValues)
        {
            //var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.netra.ai/v1/doAnalysis");
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.netra.ai/v1/doPatientAnalysis");
               httpWebRequest.ContentType = "application/json";
               httpWebRequest.Method = "POST";
              // string requestBody = string.Format("{{\"token\":\"{0}\",\"image\":\"{1}\",\"imageName\":\"{2}\"}}", token, base64, "DIAB_RETINA");
               string requestBody = "{"; 
           
            for (int i = 0; i < uploadValues.Count; i++)
               {
                   string iterValue2UploadStr = "\"" + uploadValues[i].Key + "\""+":" + "\"" + uploadValues[i].Value + "\"";
                   if (i < uploadValues.Count - 1)
                       iterValue2UploadStr += ",";
                   requestBody += iterValue2UploadStr;
               }
            requestBody += "}";
         string firstName= uploadValues[uploadValues.FindIndex(x=>x.Key == "firstName")].Value;
         string lastName = uploadValues[uploadValues.FindIndex(x => x.Key == "lastName")].Value;
         string dob = uploadValues[uploadValues.FindIndex(x => x.Key == "dob")].Value;
         string gender = uploadValues[uploadValues.FindIndex(x => x.Key == "gender")].Value;
         string patientID = uploadValues[uploadValues.FindIndex(x => x.Key == "patientID")].Value;

         //ImageData leftData = new ImageData();
         //leftData.baseName = "leftImage.jpg";
         string leftData = uploadValues[uploadValues.FindIndex(x => x.Key == "imgLeftData")].Value;
         string rightData = uploadValues[uploadValues.FindIndex(x => x.Key == "imgRightData")].Value;
         string imgRightName = uploadValues[uploadValues.FindIndex(x => x.Key == "imgRightName")].Value;
         string imgLeftName = uploadValues[uploadValues.FindIndex(x => x.Key == "imgLeftName")].Value;
         //string imgLeftData = JsonConvert.SerializeObject(leftData);

         //ImageData[] imgLeftDataArray = new ImageData[1];
         //imgLeftDataArray[0] = leftData;
         //string imgLeftData = JsonConvert.SerializeObject(imgLeftDataArray);

         //ImageData rightData = new ImageData();
         //rightData.baseName = "rightImage.jpg";
         //rightData.imgData = uploadValues[uploadValues.FindIndex(x => x.Key == "imgRightData")].Value;

         //ImageData[] imgRightDataArray = new ImageData[1];
         //imgRightDataArray[0] = rightData;
         //string imgRightData = JsonConvert.SerializeObject(imgRightDataArray);
         //string imgRightData = JsonConvert.SerializeObject(rightData);
         string emailID = uploadValues[uploadValues.FindIndex(x => x.Key == "emailID")].Value;
         string phoneCode = uploadValues[uploadValues.FindIndex(x => x.Key == "phoneCode")].Value;
         string phoneNumber = uploadValues[uploadValues.FindIndex(x => x.Key == "phoneNumber")].Value;
            
            
            //string requestBody1 = string.Format("{{\"token\":\"{0}\",\"firstName\":\"{1}\",\"lastName\":\"{2}\",\"dob\":\"{3}\",\"gender\":\"{4}\",\"patientID\":\"{5}\",\"imgLeftData\":\"{6}\",\"imgRightData\":\"{7}\",\"emailID\":\"{8}\",\"phoneCode\":\"{9}\",\"phoneNumber\":\"{10}\"}}", 
            //    token,firstName,lastName,dob,gender,patientID,imgLeftData,imgRightData,emailID,phoneCode,phoneNumber);// base64, "DIAB_RETINA");
         string requestBody1 = "{\"token\":\"" + token + "\", \"firstName\":\"" + firstName + "\", \"lastName\":\"" + lastName + "\", \"dob\":\"" + dob + "\", \"gender\":\"" + gender + "\", \"patientID\":\"" + patientID + "\", \"emailID\":\"" + emailID + "\", \"phoneCode\":\"" + phoneCode + "\", \"phoneNumber\":\"" + phoneNumber + "\", \"imgLeftData\": \"data:image/jpeg;base64," + leftData + "\", \"imgLeftName\":\""+imgLeftName+"\", \"imgRightData\": \"data:image/jpeg;base64," + rightData + "\", \"imgRightName\":\""+imgRightName+"\"}";
         //string requestBody1 = "{\"token\":\"" + token + "\", \"firstName\":\"" + firstName + "\", \"lastName\":\"" + lastName + "\", \"dob\":\"" + dob + "\", \"gender\":\"" + gender + "\", \"patientID\":\"" + patientID + "\", \"emailID\":\"" + emailID + "\", \"phoneCode\":\"" + phoneCode + "\", \"phoneNumber\":\"" + phoneNumber + "\", \"imgLeftData\":\"" + imgLeftData + "\", \"imgRightData\":\"" + imgRightData + "\"}";
               using (var streamWriter = (new StreamWriter( httpWebRequest.GetRequestStream())))
               {
                   streamWriter.Write(requestBody1);
                   streamWriter.Flush();
                   streamWriter.Close();
               }

               var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
               using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
               {
                   var result = streamReader.ReadToEnd();

                   if (result.Contains("200"))
                       CustomMessageBox.Show("Files Uploaded to cloud successfully", "Uploading to Cloud", CustomMessageBoxButtons.OK, CustomMessageBoxIcon.Information);//.GetValue("name"));
               }
            

        }

        public static async Task<JToken> GetTokenFromApi(string url, IEnumerable<KeyValuePair<string, string>> postData)
        {

            List<KeyValuePair<string, string>> tempList = postData.ToList<KeyValuePair<string, string>>();
            var httpClient = new HttpClient();
            var content = new MultipartFormDataContent();
            foreach (var item in tempList)
            {
                content.Add(new StringContent(item.Value), item.Key);
            }
            HttpResponseMessage response = await httpClient.PostAsync(url, content);

            //string resultResponse = string.Empty;
            string resultResponse = await response.Content.ReadAsStringAsync();
            JToken tokenValue = (JToken)JsonConvert.DeserializeObject(resultResponse);
            int count = 0;
            foreach (JProperty release in tokenValue)
            {
                if (count == 3)
                    token = release.Value.ToString();
                count++;
            }
            return tokenValue;
            // return resultResponse;

        }

        public static async void PostData(List<KeyValuePair<string,string>> uploadValues)
        {
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>("token", token));
            values.Add(new KeyValuePair<string, string>("firstName", uploadValues[uploadValues.FindIndex(x => x.Key == "firstName")].Value));
            values.Add(new KeyValuePair<string, string>("lastName", uploadValues[uploadValues.FindIndex(x => x.Key == "lastName")].Value));
            values.Add(new KeyValuePair<string, string>("dob", uploadValues[uploadValues.FindIndex(x => x.Key == "dob")].Value));
            values.Add(new KeyValuePair<string, string>("gender", uploadValues[uploadValues.FindIndex(x => x.Key == "gender")].Value));
            values.Add(new KeyValuePair<string, string>("patientID", uploadValues[uploadValues.FindIndex(x => x.Key == "patientID")].Value));
            values.Add(new KeyValuePair<string, string>("emailID", uploadValues[uploadValues.FindIndex(x => x.Key == "emailID")].Value));

            values.Add(new KeyValuePair<string, string>("phoneCode", uploadValues[uploadValues.FindIndex(x => x.Key == "phoneCode")].Value));
            values.Add(new KeyValuePair<string, string>("phoneNumber", uploadValues[uploadValues.FindIndex(x => x.Key == "phoneNumber")].Value));
            values.Add(new KeyValuePair<string, string>("deviceID", uploadValues[uploadValues.FindIndex(x => x.Key == "deviceID")].Value));
            values.Add(new KeyValuePair<string, string>("hospitalName", uploadValues[uploadValues.FindIndex(x => x.Key == "hospitalName")].Value));
            //values.Add(new KeyValuePair<string, string>("leftData", leftData));
            //values.Add(new KeyValuePair<string, string>("rightData", rightData));
            values.Add(new KeyValuePair<string, string>("imgLeftName", "left.jpg"));
            values.Add(new KeyValuePair<string, string>("imgRightName", "right.jpg"));


            result = await PostImagesToURL("http://netraservice.azurewebsites.net/uploadImages", values);


        }
        public static async Task<JToken> PostImagesToURL(string url, IEnumerable<KeyValuePair<string, string>> postData)
        {
            if (_dataModel.CurrentImgFiles.Length == 2)
            {
                HttpClient client = new HttpClient();

                var content = new MultipartFormDataContent();
                content.Add(new StringContent(token), "token");
                foreach (var item in postData)
                {
                    content.Add(new StringContent(item.Value), item.Key);
                }
                for (int i = 0; i < _dataModel.CurrentImgFiles.Length; i++)
                {
                    if (_dataModel.CurrentImageNames[i].Contains("OS"))
                    {
                        Stream leftStream = File.OpenRead(_dataModel.CurrentImgFiles[i]);
                        StreamContent left = new StreamContent(leftStream);
                        content.Add(left, "leftData", _dataModel.CurrentImgFiles[i]);
                    }
                    else
                    {
                        Stream rightStream = File.OpenRead(_dataModel.CurrentImgFiles[i]);
                        StreamContent right = new StreamContent(rightStream);
                        content.Add(right, "rightData", _dataModel.CurrentImgFiles[i]);
                    }
                }

                var response = await client.PostAsync("http://netraservice.azurewebsites.net/uploadImages", content);
                JToken tokenValue;
                if (response.IsSuccessStatusCode)
                {
                    string resultResponse = await response.Content.ReadAsStringAsync();
                    tokenValue = (JToken)JsonConvert.DeserializeObject(resultResponse);
                    if (resultResponse.Contains("1"))
                        CustomMessageBox.Show("Files Uploaded to cloud successfully", "Uploading to Cloud", CustomMessageBoxButtons.OK, CustomMessageBoxIcon.Information);//.GetValue("name"));
                    _WaitCursor();
                    return tokenValue;

                }
                //return response.Content.ReadAsStreamAsync();
                //return string.Empty;
                else
                    return null;
            }
            else
            {
                CustomMessageBox.Show("Please select only two images", "Warning", CustomMessageBoxButtons.OK, CustomMessageBoxIcon.Information);//.GetValue("name"));
                _WaitCursor();
                return null;
            }
        }


        static void GetRequestStreamCallback(IAsyncResult callbackResult)
        {
            HttpWebRequest webRequest = (HttpWebRequest)callbackResult.AsyncState;
            Stream postStream = webRequest.EndGetRequestStream(callbackResult);
            string requestBody = string.Format("{{\"username\":\"{0}\",\"password\":\"{1}\",\"apiRequestType\":\"{2}\"}}", "anil@intuvisionlabs.com", "Gt#8$DkMy","DIAB_RETINA");
            byte[] byteArray = Encoding.UTF8.GetBytes(requestBody);
            postStream.Write(byteArray, 0, byteArray.Length);
            postStream.Close();

            webRequest.BeginGetResponse(new AsyncCallback(GetResponseStreamCallback), webRequest);
        }

        static   void GetResponseStreamCallback(IAsyncResult callbackResult)
        {
            HttpWebRequest request = (HttpWebRequest)callbackResult.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(callbackResult);
            using (StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream()))
            {
                string result = httpWebStreamReader.ReadToEnd();
                Console.WriteLine(result);
            }
        }




        //public static void Main()
        //{

        //    var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.netra.ai/v1/getToken");
        //    //var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://reqres.in/api/users");
        //    httpWebRequest.ContentType = "application/x-www-form-urlencoded";
        //    httpWebRequest.Method = "POST";
        //    //System.Collections.Generic.Dictionary<String, string> values = new System.Collections.Generic.Dictionary<string, string>();

        //        //HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(myUri);
        //        //webRequest.Method = "POST";
        //        //webRequest.ContentType = "application/x-www-form-urlencoded";
        //        //webRequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), webRequest);

        //    //values.Add("username", "anil@intuvisionlabs.com");
        //    //values.Add("password", "Gt#8$DkMy");
        //    //values.Add("apiRequestType", "DIAB_RETINA");
        //    //using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //    string json = string.Empty;
        //    using (var streamWriter = (httpWebRequest.GetRequestStream()))
        //    {

        //         json = string.Format("{{\"username\":\"{0}\",\"password\":\"{1}\",\"apiRequestType\":\"{2}\"}}", "anil@intuvisionlabs.com", "Gt#8$DkMy", "DIAB_RETINA");
        //        //string json = "{{\"username\":\"anil@intuvisionlabs.com\"," +
        //        //              "\"password\":\"Gt#8$DkMy\"," + "\"apiRequestType\":\"DIAB_RETINA\"}}";
        //        byte[] byteArray = Encoding.UTF8.GetBytes(json);

        //        streamWriter.Write(byteArray, 0, byteArray.Length);
        //        streamWriter.Flush();

        //        streamWriter.Close();

        //        //streamWriter.Write(json);
        //        //streamWriter.Flush();
        //        //streamWriter.Close();
        //    }

        //    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //    {
        //        var result = streamReader.ReadToEnd();

        //        Console.WriteLine("Hello {0}", result);
        //    }

        //    //string urlForBase64 = "http://www.gravatar.com/avatar/6810d91caff032b202c50701dd3af745?d=identicon&r=PG";

        //    //Program P = new Program();

        //    //string base64String = P.ConvertImageURLToBase64(urlForBase64);

        //    //Console.WriteLine("Hello {0}", base64String);

        //}

       

        ////void GetRequestStreamCallback(IAsyncResult callbackResult)
        ////{
        ////    HttpWebRequest webRequest = (HttpWebRequest)callbackResult.AsyncState;
        ////    Stream postStream = webRequest.EndGetRequestStream(callbackResult);

        ////    string requestBody = string.Format("{{\"username\":\"{0}\",\"password\":\"{1}\"}}", usernameBox.Text, passBox.Text);
        ////    byte[] byteArray = Encoding.UTF8.GetBytes(requestBody);
           
        ////    postStream.Write(byteArray, 0, byteArray.Length);
        ////    postStream.Close();

        ////    webRequest.BeginGetResponse(new AsyncCallback(GetResponseStreamCallback), webRequest);
        ////}

        ////void GetResponseStreamCallback(IAsyncResult callbackResult)
        ////{
        ////    HttpWebRequest request = (HttpWebRequest)callbackResult.AsyncState;
        ////    HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(callbackResult);
        ////    using (StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream()))
        ////    {
        ////        string result = httpWebStreamReader.ReadToEnd();
        ////        Debug.WriteLine(result);
        ////    }
        ////}

     public static String ConvertImageURLToBase64(String url)
     {
         StringBuilder _sb = new StringBuilder();

         Byte[] _byte = GetImage(url);

         _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));

         return _sb.ToString();
     }

     private static byte[] GetImage(string url)
     {
         Stream stream = null;
         byte[] buf;

         try
         {
             //WebProxy myProxy = new WebProxy();
             //HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

             //HttpWebResponse response = (HttpWebResponse)req.GetResponse();
             buf = File.ReadAllBytes(@url);
             //bm.Save(stream,ImageFormat.Jpeg);

             //using (BinaryReader br = new BinaryReader(stream))
             //{
             //    int len = (int)(response.ContentLength);
             //    buf = br.ReadBytes(len);
             //    br.Close();
             //}

             //stream.Close();
             //response.Close();
         }
         catch (Exception exp)
         {
             buf = null;
         }

         return (buf);
     }
	

    } 
   
   
   }

