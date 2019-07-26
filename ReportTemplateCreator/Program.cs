using System;
using System.Collections.Generic;
using System.Linq;
using ReportUtils;
using pDesigner;
using System.IO;
using System.Drawing;
using DesignSurfaceExt;
using NLog;
using NLog.Config;
using NLog.Targets;
using System.Windows.Forms;

namespace ReportTemplateCreator
{
    static class Program
    {
        static string filePath = string.Empty;
        static Logger exceptionLog = LogManager.GetLogger("ReportTemplateCreator.ExceptionLog");
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                
               //filePath  = new FileInfo(Application.ExecutablePath).Directory.FullName + Path.DirectorySeparatorChar
               //                                              + "Logs" + Path.DirectorySeparatorChar + DateTime.Now.ToString("dd-MM-yyyy") + Path.DirectorySeparatorChar; //log file path
               // if (!Directory.Exists(filePath))
               //     Directory.CreateDirectory(filePath);

               // log4net.GlobalContext.Properties["ReportTemplateEventFileName"] = filePath + "ReportTemplate";
               // log4net.Config.XmlConfigurator.Configure();
                //MessageBox.Show(filePath + "ReportTemplate");
                ReportTemplateCreatorWindow reportTemplateCreator = new ReportTemplateCreatorWindow();
                //MessageBox.Show("ReportTemplate");
                Application.EnableVisualStyles();
                LogManager.Configuration.Variables["dir"] = new FileInfo(Application.ExecutablePath).Directory.FullName + Path.DirectorySeparatorChar;
                LogManager.Configuration.Variables["dir2"] = DateTime.Now.ToString("yyyy-MM-dd");
                //Application.Run(reportTemplateCreator);
                reportTemplateCreator.ShowDialog();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Catch");
                
                exceptionLog.Info(ex.StackTrace);
                Common.ExceptionLogWriter.WriteLog(ex,exceptionLog);
//                
            }
        }
    }
}
