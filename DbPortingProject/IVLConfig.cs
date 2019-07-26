using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using WindowsFormsApplication1.AdvancedSettings;
using NLog;
using NLog.Targets;
namespace DBPorting
{
   public class IVLConfig
       {
       private static IVLConfig _ivlConfig;
       public static string fileName = "";
       public UserSettings UserSettings;
       public FirmwareSettings FirmwareSettings;
       public PostProcessingSettings PostProcessingSettings;
       public CameraSettings CameraSettings;
       public PrinterSettings PrinterSettings;
       public ImageStorageSettings ImageStorageSettings;
       public UISettings UISettings;
       public AnnotationColorSelection AnnotationColorSelection;
       public static Logger Exception_Log = LogManager.GetLogger("DB_Porting.ExceptionLog");

       public static void ResetIVLConfig()
       {
           _ivlConfig = null;
       }
       public IVLConfig()
       {
           UserSettings = new UserSettings();
           AnnotationColorSelection = new AnnotationColorSelection();
           FirmwareSettings = new FirmwareSettings();
           PostProcessingSettings = new PostProcessingSettings();
           CameraSettings = new CameraSettings();
           PrinterSettings = new PrinterSettings();
           ImageStorageSettings = new ImageStorageSettings();
           UISettings = new UISettings();
       }
       private void createIvlConfig()
       {
       }
       public static IVLConfig getInstance()
       {
           try
           {
               if (_ivlConfig == null)
               {
                   try
                   {
                       Type t = typeof(IVLConfig);
                       FileInfo filePath = new FileInfo(fileName);
                       {
                           if (filePath.IsReadOnly)
                               filePath.IsReadOnly = false;
                       }
                       _ivlConfig = (IVLConfig)XmlConfigUtility.Deserialize(t, fileName);
                   }
                   catch (Exception)
                   {
                       Type t = typeof(IVLConfig);

                       _ivlConfig = new IVLConfig();

                       {
                           // This below code has been changed by darshan on 17-mar-2016 handle the situation when the config file has been corrupted to retrive the data from the backup config file.
                           if (File.Exists(IVLVariables.backupConfigFile))
                               _ivlConfig = (IVLConfig)XmlConfigUtility.Deserialize(t, IVLVariables.backupConfigFile);
                           else
                               XmlConfigUtility.Serialize(_ivlConfig, IVLVariables.backupConfigFile);
                           XmlConfigUtility.Serialize(_ivlConfig, fileName);
                       }
                   }
               }
               return _ivlConfig;
           }
           catch (Exception ex)
           {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
                return _ivlConfig;
           }
           
       }
       //~IVLConfig()
       //{
       //    _ivlConfig = null;
       //    fileName = null;
       //    _ivlConfig = IVLConfig.getInstance();
       //    XmlConfigUtility.Serialize(_ivlConfig, "IVLRestore.xml");
       //}
   }


   public class XmlConfigUtility
   {
       public static Logger Exception_Log = LogManager.GetLogger("DB_Porting.ExceptionLog");

       public static void Serialize(Object data, string fileName)
       {
           try
           {
               Type type = data.GetType();
               XmlSerializer xs = new XmlSerializer(type);
               XmlTextWriter xmlWriter = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);
               xmlWriter.Formatting = Formatting.Indented;
               xs.Serialize(xmlWriter, data);
               xmlWriter.Close();
           }
           catch (Exception ex)
           {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
           }
           
       }

       public static Object Deserialize(Type type, string fileName)
       {
           Object data = null;
           try
           {
               
               XmlSerializer xs = null;
               XmlTextReader xmlReader = null;
               try
               {
                   xs = new XmlSerializer(type);
                   xmlReader = new XmlTextReader(fileName);
                   data = xs.Deserialize(xmlReader);
               }
               finally
               {
                   xmlReader.Close();
               }
               return data;
           }
           catch (Exception ex)
           {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
                return data;

           }
           
       }
   }
 }
