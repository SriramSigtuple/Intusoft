using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using INTUSOFT.Desktop.Forms;
using System.Threading;
using System.ServiceProcess;
using System.IO;
using System.Globalization;
using System.ComponentModel;
using System.Resources;
using System.Data.SqlClient;
using Microsoft.Win32;
using INTUSOFT.Desktop.Properties;
using System.Runtime.InteropServices;
using NLog;
using NLog.Config;
using NLog.Targets;
using CommandLine;
using CommandLine.Text;
using System.Diagnostics;

namespace INTUSOFT.Desktop
{
 static  partial class   Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        ///
        [DllImport("user32.dll")]
        private static extern Boolean ShowWindow(IntPtr hWnd, Int32 nCmdShow);
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        static RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Intusoft\\");
        private const int SW_SHOWMAXIMIZED = 3;
        private const int SW_RESTORE = 9;
        public static IvlMainWindow _currentInstance;
       static Logger exceptionLog = LogManager.GetLogger("ExceptionLog");

        [STAThread]
        static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                IVLVariables.isCommandLineArgsPresent = true;

                var options = new Options();
                var parser = new CommandLine.Parser(with => with.HelpWriter = Console.Error);
                if (parser.ParseArgumentsStrict(args, options, () => Environment.Exit(-2)))
                {
                    string path = string.Empty;
                    Common.Validators.FileNameFolderPathValidator f = Common.Validators.FileNameFolderPathValidator.GetInstance();

                    switch (f.CheckFolderPath(options.ImageSavePath, ref path))
                    {
                        case Common.Validators.FolderPathErrorCode.Success:
                            {
                                Directory.SetCurrentDirectory(new FileInfo(Application.ExecutablePath).Directory.FullName);

                                IVLVariables.isCommandLineAppLaunch = true;
                                IVLVariables.CmdImageSavePath = path;
                                break;
                            }
                        case Common.Validators.FolderPathErrorCode.FolderPath_Empty:
                            {
                                Directory.SetCurrentDirectory(new FileInfo(Application.ExecutablePath).Directory.FullName);

                                IVLVariables.isCommandLineAppLaunch = true;

                                break;
                            }
                        case Common.Validators.FolderPathErrorCode.InvalidDirectory:
                            {
                                MessageBox.Show("Please enter a valid path for the application to run");
                                return;
                            }
                        case Common.Validators.FolderPathErrorCode.DirectoryDoesnotExist:
                            {
                                if (!Directory.Exists(options.ImageSavePath))
                                {
                                    Directory.CreateDirectory(options.ImageSavePath);
                                    IVLVariables.isCommandLineAppLaunch = true;
                                    Directory.SetCurrentDirectory(new FileInfo(Application.ExecutablePath).Directory.FullName);

                                    IVLVariables.CmdImageSavePath = options.ImageSavePath;
                                }
                                break;
                            }
                    }

                    if (options.ReportBatchFilePath == null && options.ImageSavePath == null)//this has been added to run the old batchfile and to get the batchfile path.
                    {
                        options.ReportBatchFilePath = options.DefinitionFiles[0];
                    }
                    if (!string.IsNullOrEmpty(options.ReportBatchFilePath))
                    {
                        IVLVariables.isCommandLineAppLaunch = false;

                        IVLVariables.batchFilePath = options.ReportBatchFilePath;
                    }

                }

            }
            if(registryKey != null) 
                IVLVariables.appDirPathName = (string)registryKey.GetValue("AppDataPath");
            bool createdNew = false;
                using (Mutex mutex = new Mutex(true, "IntuSoft", out createdNew))
                {
                    if (createdNew)
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        try
                        {

                            if (string.IsNullOrEmpty(IVLVariables.appDirPathName))
                                IVLVariables.appDirPathName = new FileInfo(Application.ExecutablePath).Directory.FullName + Path.DirectorySeparatorChar;
                            else if (Directory.Exists(IVLVariables.appDirPathName))
                                IVLVariables.appDirPathName = IVLVariables.appDirPathName + Path.DirectorySeparatorChar;// new FileInfo(Application.ExecutablePath).Directory.FullName + Path.DirectorySeparatorChar;
                            else
                                IVLVariables.appDirPathName = new FileInfo(Application.ExecutablePath).Directory.FullName + Path.DirectorySeparatorChar;


                            LogManager.Configuration.Variables["dir"] = IVLVariables.appDirPathName;
                            LogManager.Configuration.Variables["dir2"] = DateTime.Now.ToString("yyyy-MM-dd");
                            LogManager.Configuration.Variables["dir3"] = DateTime.Now.ToString("HH:mm:ss");
                         
                        #region Previous Log Codes Commented
                        //string filePath = new FileInfo(Application.ExecutablePath).Directory.FullName + Path.DirectorySeparatorChar
                        //                                 + "Logs" + Path.DirectorySeparatorChar + DateTime.Now.ToString("dd-MM-yyyy") + Path.DirectorySeparatorChar; //log file path
                        //if (!Directory.Exists(filePath))
                        //    Directory.CreateDirectory(filePath);

                        //log4net.GlobalContext.Properties["LogFileName"] = filePath + "Camera";
                        //log4net.Config.XmlConfigurator.Configure();

                        //log4net.GlobalContext.Properties["UIFileName"] = filePath + "UI";
                        //log4net.Config.XmlConfigurator.Configure();

                        //log4net.GlobalContext.Properties["FrameEventFileName"] = filePath + "Frames";
                        //log4net.Config.XmlConfigurator.Configure();
                        //// Configure log file for capture sequence//
                        //log4net.GlobalContext.Properties["CaptureFileName"] = filePath + "CaptureLog";
                        //log4net.Config.XmlConfigurator.Configure();

                        //log4net.GlobalContext.Properties["CaptureSettingsFileName"] = filePath + "CaptureSettingsLog";
                        //log4net.Config.XmlConfigurator.Configure();
                        #endregion

                       
                        _currentInstance = new IvlMainWindow();
                            // code to create logs folder during start of the application
                            Application.Run(_currentInstance);
                       
                        }
                        catch (Exception ex)
                        {
                            Common.ExceptionLogWriter.WriteLog(ex,exceptionLog);
//                            exceptionLog.Info(ex.StackTrace);
                        }
                    }
                    else
                    {
                        //STRING_Program_062
                        //Common.CustomMessageBox.Show(Resources.Program_01 ,Resources.Software_Name, Common.Common.CustomMessageBoxButtons.OK, Common.CustomMessageBoxIcon.Information);
                        try
                        {
                            #region Region of Commented Code
                            //string filePath = new FileInfo(Application.ExecutablePath).Directory.FullName + Path.DirectorySeparatorChar
                            //                                 + "Logs" + Path.DirectorySeparatorChar + DateTime.Now.ToString("dd-MM-yyyy") + Path.DirectorySeparatorChar; //log file path
                            //if (!Directory.Exists(filePath))
                            //    Directory.CreateDirectory(filePath);

                            //log4net.GlobalContext.Properties["LogFileName"] = filePath + "Camera";
                            //log4net.Config.XmlConfigurator.Configure();

                            //log4net.GlobalContext.Properties["UIFileName"] = filePath + "UI";
                            //log4net.Config.XmlConfigurator.Configure();

                            //log4net.GlobalContext.Properties["FrameEventFileName"] = filePath + "Frames";
                            //log4net.Config.XmlConfigurator.Configure();
                            //// Configure log file for capture sequence//
                            //log4net.GlobalContext.Properties["CaptureFileName"] = filePath + "CaptureLog";
                            //log4net.Config.XmlConfigurator.Configure();
                            #endregion
                            if (IVLVariables.LangResourceManager == null)
                                IVLVariables.LangResourceManager = new ResourceManager("INTUSOFT.Desktop.LanguageResources.Res", typeof(IvlMainWindow).Assembly);
                            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName(IVLVariables.LangResourceManager.GetString("Software_Name", IVLVariables.LangResourceCultureInfo));
                            foreach (System.Diagnostics.Process item in processes)
                            {
                                ShowWindow(item.MainWindowHandle, SW_RESTORE);
                                ShowWindow(item.MainWindowHandle, SW_SHOWMAXIMIZED);
                                SetForegroundWindow(item.MainWindowHandle);
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            Common.ExceptionLogWriter.WriteLog(ex,exceptionLog);
                        }
                    }
                }
            }


        //0000573 Feature has been added by sriram on August 18th 2015
        public static void RestartApplication()
        {
            Application.Restart();
        }
    }

   static partial  class  Program
    {
        private enum OptimizeFor
        {
            Unspecified,
            Speed,
            Accuracy
        }

        //
        // You no longer need to inherit from CommandLineOptionsBase (removed)
        //
        private  class Options
        {
            [Option('F', "CommandLineImaging", MetaValue = "String", HelpText = "Start Imaging Screen From Command Line")]
            public string ImageSavePath { get; set; }

            [Option('R', "CommandLineReporting", MetaValue = "String", HelpText = "Start Command Line Reporting From Batch Script")]
            public string ReportBatchFilePath { get; set; }


            [ValueList(typeof(List<string>))]
            public IList<string> DefinitionFiles { get; set; }

            [OptionList('o', "operators", Separator = ';', HelpText = "Operators included in processing (+;-;...)." +
                " Separate each operator with a semicolon." + " Do not include spaces between operators and separator.")]
            public IList<string> AllowedOperators { get; set; }

            //
            // Marking a property of type IParserState with ParserStateAttribute allows you to
            // receive an instance of ParserState (that contains a IList<ParsingError>).
            // This is equivalent from inheriting from CommandLineOptionsBase (of previous versions)
            // with the advantage to not propagating a type of the library.
            //
            [ParserState]
            public IParserState LastParserState { get; set; }

            [HelpOption]
            public string GetUsage()
            {
                return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
            }
        }

    }
}