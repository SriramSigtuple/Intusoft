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
using SingleInstanceApp;
namespace IVLUploader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application,ISingleInstance
    {

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private const string Unique = "IVLUploader";

    [STAThread]
        public static void Main()
        {
            if (SingleInstance<App>.InitializeAsFirstInstance(Unique))
            {
                var application = new App();

                application.InitializeComponent();
                application.Run();

                // Allow single instance code to perform cleanup operations
                SingleInstance<App>.Cleanup();
            }
        }

        #region ISingleInstanceApp Members

        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            // handle command line arguments of second instance
            // …

            return true;
        }

        #endregion


        protected override void OnStartup(StartupEventArgs e)
        {
          

            base.OnStartup(e);
            LogManager.Configuration.Variables["dir1"] = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).Directory.FullName;
            LogManager.Configuration.Variables["dir2"] = DateTime.Now.ToString("dd-MM-yyyy");
            LogManager.Configuration.Variables["dir3"] = DateTime.Now.ToString("HH:mm:ss");

            LogManager.Configuration.AddRuleForAllLevels("ctrl");
            LogManager.Configuration.AddRuleForAllLevels("Logger");
            LogManager.Configuration.AddRuleForAllLevels("Console");
            //LogginVM.GetLogginVM();
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
