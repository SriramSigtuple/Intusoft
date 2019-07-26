using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Microsoft.Win32;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;
namespace INTUSOFT.Data
{
    //This below static class has been added by Darshan on 28-08-2015 to write from backup json file to current json file when current json file has been corrupted.
 
    public static  class Common
    {
     public  enum GetValueType { Existing, Deleted, All };
        static Type t;
       static object objectList;
       static TextWriter JsontxtWriter;
      static JsonWriter jsonWriter;
      public static DirectoryInfo log_directoryName = new DirectoryInfo(@"D:\intusoft_2015_09_08\IvlSoft.Desktop\bin\x86\Debug\Logs");

        /// <summary>
        /// isCheckJsonCorruption method check wheather a json file is corrupted or not. 
        /// </summary>
        /// <param name="jsonFilePath">jsonFilePath gives the path of specific json file </param>
        /// <returns>true is returned if json file is not corrupted else false is returned if json file is corrupted</returns>

        public static bool isCheckJsonCorruption(string jsonFilePath)
        {
            bool isNotCorrupt = false;//bool variable for assing boolean value based on if condition specified.
            string[] PatientJsonFilePath = jsonFilePath.Split('.');//Splits the jsonFilePath and stores it into PatientJsonFilePath string array.

            if (PatientJsonFilePath[PatientJsonFilePath.Length - 1].Equals("json") || PatientJsonFilePath[PatientJsonFilePath.Length - 1].Equals("bak"))//check wheather the extension of file in isjsonFilePath is .json.
            {
               //byte[] arr=(byte[])File.ReadAllLines(jsonFilePath);
                //byte[] arr = File.ReadAllBytes(jsonFilePath);
                //string resultantJsonFile = System.Text.Encoding.UTF8.GetString(arr);
                string[] resultantJsonFile = File.ReadAllLines(jsonFilePath);//Read all lines from json file and save it into string array resultantJsonFile.
            if (resultantJsonFile.Length > 0)
            {
                if (resultantJsonFile[0].Last().Equals('\0'))
                    resultantJsonFile[0] = resultantJsonFile[0].Trim('\0');
                char firstCharJson = resultantJsonFile[0].First();//Save the first charecter of string array resultantJsonFile into firstCharJson
                char lastChatJson = resultantJsonFile[0].Last();//Save the last charecter of string array resultantJsonFile into lastCharJson
                int open_braces_count = resultantJsonFile[0].Where(x => x == '{').Count();//Saves number of open braces  in string array resultantJsonFile into open_braces_count
                int close_braces_count = resultantJsonFile[0].Where(x => x == '}').Count();//Saves number of close braces  in string array resultantJsonFile into close_braces_count
                if (firstCharJson == '[' && lastChatJson == ']')//Check for first and last charecter of json file specified in jsonFilePath
                {
                    if (open_braces_count == close_braces_count)//Check for equality of open braces and close braces of json file specified in jsonFilePath
                    {
                        
                            isNotCorrupt = true;//If file is not corrupted.
                     }
                    else
                    {
                        isNotCorrupt = false;//If file is not corrupted.
                     }
                }
                else
                {
                    isNotCorrupt = false;//If file is corrupted.
                 }
            }         
        }
            return isNotCorrupt;
        }
        /// <summary>
        /// CheckbackUpDirectories will get the backup directories and check weather the list of json files are corrupted or not 
        /// </summary>
        public static void checkBackUpDirectories(string BackupSavePath,string file_path)
        {
            bool isValidJsonFile = false;
            string[] arrayOfPath=file_path.Split('\\');
            string[] jsonfileextension = arrayOfPath[2].Split('.');
            string[] arrayOfDirectories = Directory.GetDirectories(BackupSavePath).ToArray();
            arrayOfDirectories = arrayOfDirectories.Reverse().ToArray();
             
            string[] backupJsonFiles = Directory.GetFiles(BackupSavePath, jsonfileextension[0] + "*.json" + ".bak");
                backupJsonFiles = backupJsonFiles.Reverse().ToArray();
                for (int j = 0; j < backupJsonFiles.Length; j++)
                {
                    if (Common.isCheckJsonCorruption(backupJsonFiles[j]))
                    {
                        File.Create(file_path).Close();
                        var resultantVariable = File.ReadAllLines(backupJsonFiles[j]);
                        File.WriteAllLines(file_path, resultantVariable);
                         
                        isValidJsonFile = true;
                        break;
                    }

                }
                      
         }
 
     /// <summary>
      /// JsonFileReader function Reads contents from a specific json file.
     /// </summary>
      /// <param name="file_path">Specifies the path of the file</param>
     /// <returns>Returns the contents in the form of string</returns>
    
      public static T JsonFileReader<T>(string file_path)
      {
          //This below code been modified by Darshan on 16-09-2015 to stop courroupting of json file.
                  File.SetAttributes(file_path, FileAttributes.Normal);
                   byte[] arr = File.ReadAllBytes(file_path);
                          
          //TextReader txReader = new StreamReader(file_path);
          string newVal = System.Text.Encoding.UTF8.GetString(arr);
          newVal = newVal.Trim('\0');
          
          JavaScriptSerializer serializer = new JavaScriptSerializer();
          serializer.MaxJsonLength = Int32.MaxValue;
          return serializer.Deserialize<T>(newVal);
      }

         //This method was added by Darshan on 03-09-2015.
        /// <summary>
        /// GetJsonFilePath function returns the filepath of json files of the key specified in the parameter.
        /// </summary>
        /// <param name="subKey">Specifies the key name</param>
        /// <returns>Rerurns the file path</returns>
      public static string GetJsonFilePath(string subKey)
      {
          string file_path = "";

          RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Intusoft", true);
          if (key == null)
          {
              Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Intusoft");
          }
          {
              object obj = key.GetValue(subKey);//.ToString();

              if (obj != null)
                  file_path = obj.ToString();
              else
              {
                  key.SetValue(subKey, @"D:\Backup_JsonFiles");
                  object obj1 = key.GetValue(subKey);//.ToString(
                  file_path = obj1.ToString();

              }
          }
          key.Close();
          return file_path;
      }

        //This method was added by Darshan on 03-09-2015.
        /// <summary>
        /// fileIsreadonly method will change the isreadOnly property of a file
        /// </summary>
        /// <param name="file_path">Path of the File</param>
        /// <param name="isreadonly">Bool to set to true or false</param>
      public static void fileIsreadonly(string file_path, bool isreadonly)
      {
          if (File.Exists(file_path))
          {
              FileInfo filePath = new FileInfo(file_path);
              filePath.IsReadOnly = isreadonly;
          }
      }
      //This below GetDescription method has been added by Darshan on 07-09-2015 as per requirement suggested in Defect no 0000460: CR:The underscore must be removed in the Patient details fields.

      /// <summary>
      /// GetDescription will get the Descripion of a enum value.
      /// </summary>
      /// <param name="value">Enum value whose decription will be returned</param>
      /// <returns>Descrpiption</returns>
      public static string GetDescription(this Enum value)
      {
          Type type = value.GetType();
          string name = Enum.GetName(type, value);
          if (name != null)
          {
              FieldInfo field = type.GetField(name);
              if (field != null)
              {
                  DescriptionAttribute attr =
                         Attribute.GetCustomAttribute(field,
                           typeof(DescriptionAttribute)) as DescriptionAttribute;
                  if (attr != null)
                  {
                      return attr.Description;
                  }
              }
          }
          return null;
      }

      public static void Repostiory_ExceptionLog( string filename , string details)
      {
         
          System.IO.StreamWriter sw = new System.IO.StreamWriter(log_directoryName.FullName + Path.DirectorySeparatorChar + DateTime.Now.ToString("dd-MM-yyyy")+Path.DirectorySeparatorChar+filename+".csv", true);
          string Time_stamp = DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss-fff") + "," + details+Environment.NewLine;
           sw.WriteLine(Time_stamp);
          sw.Close();

      }
      //This below method JsonFileWriter been modified by Darshan on 16-09-2015 to stop courroupting of json file.

      /// <summary>
      /// This function writes into the json file of specified type
      /// </summary>
      /// <param name="file_path">Specifies the path if the file</param>
      /// <param name="details">Specifies which kind if model</param>
        public static void JsonFileWriter(string file_path, List<object> details)
      {
          FileStream f = null;
          File.SetAttributes(file_path, FileAttributes.Normal);
          f = File.Create(file_path, 4096, FileOptions.WriteThrough);
          //File.WriteAllText(jsonBackupPath, JsonConvert.SerializeObject(patlist, new IsoDateTimeConverter()));
          string val = JsonConvert.SerializeObject(details, new IsoDateTimeConverter());

          val = val.Trim('\0');

          //byte[] arr = new byte[val.Length * sizeof(char)];
          byte[] arr = System.Text.Encoding.UTF8.GetBytes(val);
          //System.Buffer.BlockCopy(val.ToCharArray(), 0, arr, 0, arr.Length);
          //f.Write(, 0,
          try
          {
              f.Write(arr, 0, arr.Length);

              //f = File.Open(jsonBackupPath, FileMode.Open, FileAccess.ReadWrite);
              f.Flush(true);
            //  f.Read(arr, 0, arr.Length);
            //val = System.Text.Encoding.UTF8.GetString(arr);
            //char a = val.Last();

              
          }
          catch (Exception ex)
          {
              MessageBox.Show(ex.Message);
          }
          finally
          {
              Thread.Sleep(200);
              if (f != null)
                  f.Close();
          }
      }
  }
}
