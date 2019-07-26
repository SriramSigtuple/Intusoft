using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using INTUSOFT.Imaging;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace AssemblySoftware
{
    static class Program
    {
        public static DirectoryInfo dinf;
        private static readonly Logger Exception_Log = LogManager.GetLogger("ExceptionLog");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            LogManager.Configuration.Variables["dir"] = new FileInfo(Application.ExecutablePath).Directory.FullName + Path.DirectorySeparatorChar;
            LogManager.Configuration.Variables["dir2"] = DateTime.Now.ToString("yyyy-MM-dd");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                //if (!Directory.Exists(@"Logs"))
                //{
                //    dinf = Directory.CreateDirectory(@"Logs");
                //}
                //else
                //    dinf = new DirectoryInfo(@"Logs");
                //if (!Directory.Exists(dinf.FullName + Path.DirectorySeparatorChar + DateTime.Now.ToString("dd-MM-yyyy")))
                //    dinf = Directory.CreateDirectory(dinf.FullName + Path.DirectorySeparatorChar + DateTime.Now.ToString("dd-MM-yyyy"));
                //else
                //    dinf = new DirectoryInfo(dinf.FullName + Path.DirectorySeparatorChar + DateTime.Now.ToString("dd-MM-yyyy"));
                //IntucamHelper.LogFilePath = dinf.FullName;
                //string filePath = new FileInfo(Application.ExecutablePath).Directory.FullName + Path.DirectorySeparatorChar
                //                                               + "Logs" + Path.DirectorySeparatorChar + DateTime.Now.ToString("dd-MM-yyyy") + Path.DirectorySeparatorChar; //log file path
                //if (!Directory.Exists(filePath))
                //    Directory.CreateDirectory(filePath);
               
                //log4net.GlobalContext.Properties["LogFileName"] = filePath + "Camera";
                //log4net.Config.XmlConfigurator.Configure();
                
                //log4net.GlobalContext.Properties["UIFileName"] = filePath + "UI";
                //log4net.Config.XmlConfigurator.Configure();

                //log4net.GlobalContext.Properties["FrameEventFileName"] = filePath + "Frames";
                //log4net.Config.XmlConfigurator.Configure();
                //// Configure log file for capture sequence//
                //log4net.GlobalContext.Properties["CaptureFileName"] = filePath +"CaptureLog";
                //log4net.Config.XmlConfigurator.Configure();
                Application.Run(new CameraUI());

                
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);

                //System.IO.StreamWriter sw = new System.IO.StreamWriter(dinf.FullName + Path.DirectorySeparatorChar + "exceptions.log", true);
                //string exception = DateTime.Now.ToString() + Environment.NewLine;
                //exception += ex.Message + Environment.NewLine;
                //exception += ex.StackTrace + Environment.NewLine;
                //sw.WriteLine(exception);
                //sw.Close();
            }
        }

        public static void RestartApplication()
        {
            Application.Restart();
        }
    }
}
