using IntuUploader;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace IVLUploader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        //[STAThread]
        //public static void Main(string[] args)
        //{
        //    if (!IsFirstInstance()) return;

        //    var app = new App();
        //    app.InitializeComponent();
        //    app.Run();
        //}

        //public static bool IsFirstInstance()
        //{
        //    //give the mutex a name => it will be systemwide.
        //    var mutex = new Mutex(true, "somethingunique");
        //    return mutex.WaitOne(TimeSpan.Zero, false);
        //}

        protected override void OnStartup(StartupEventArgs e)
        {
          

            base.OnStartup(e);
            LogManager.Configuration.Variables["dir"] = Directory.GetCurrentDirectory();
            LogManager.Configuration.Variables["dir2"] = DateTime.Now.ToString("dd-MM-yyyy");
            LogManager.Configuration.AddRuleForAllLevels("ctrl");
            LogManager.Configuration.AddRuleForAllLevels("Logger");
            LogManager.Configuration.AddRuleForAllLevels("Console");
            LogginVM.GetLogginVM();
            SetupExceptionHandling();
        }

        private void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                LogUnhandledException((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

            DispatcherUnhandledException += (s, e) =>
                LogUnhandledException(e.Exception, "Application.Current.DispatcherUnhandledException");

            TaskScheduler.UnobservedTaskException += (s, e) =>
                LogUnhandledException(e.Exception, "TaskScheduler.UnobservedTaskException");
        }

        private void LogUnhandledException(Exception exception, string source)
        {
            string message = $"Unhandled exception ({source})";
            try
            {
                System.Reflection.AssemblyName assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
                message = string.Format("Unhandled exception in {0} v{1}", assemblyName.Name, assemblyName.Version);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception in LogUnhandledException");
            }
            finally
            {
                _logger.Error(exception, message);
            }
        }



    }
   
}
