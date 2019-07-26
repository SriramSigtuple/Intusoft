using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using NLog.Config;
using NLog.Targets;
using System.IO;
using Common;
namespace CameraApiTest
{
    static class Program
    {
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
                Application.Run(new CameraApiSampleWindow());

            }
            catch (Exception ex)
            {
                ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
        }
    }
}
