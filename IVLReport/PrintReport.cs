using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using System.Diagnostics;
using System.Printing;
using System.IO;
using System.Drawing.Printing;
using Microsoft.Win32;
using PdfFileWriter;
using Common;

namespace IVLReport
{
   public class PrintReport
    {
        public static PrintDialog print_dialog;// = new PrintDialog();
        public static int processID = 0;
        public static int adobeDelayTime = 20000;
        public static Boolean PrintPDFs(string pdfFileName, string adobereader_text)
        {
            try
            {
                {
                    Process proc = new Process();
                    proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    proc.StartInfo.Verb = "print";
                    print_dialog = new PrintDialog();
                    //Define location of adobe reader/command line
                    //switches to launch adobe in "print" mode
                    var adobe = Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Windows").OpenSubKey("CurrentVersion").OpenSubKey("App Paths").OpenSubKey("AcroRd32.exe");
                    var path = adobe.GetValue("");
                    if (File.Exists(path.ToString()))
                    {
                        var adobeOtherWay = Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Classes").OpenSubKey("acrobat").OpenSubKey("shell").OpenSubKey("open").OpenSubKey("command");
                        string pathOtherWay = adobeOtherWay.GetValue("").ToString();
                        if (File.Exists(path.ToString()))
                        {
                            string[] strArr = pathOtherWay.Split('"');
                            proc.StartInfo.FileName =
                             strArr[1];
                            // if (print_dialog.ShowDialog() == DialogResult.OK)
                            {
                                //print_dialog.PrinterSettings.PrinterName;
                                string str;
                                string printername = print_dialog.PrinterSettings.PrinterName;
                                const string flagNoSplashScreen = "/s";
                                const string flagOpenMinimized = "/h";
                                var flagPrintFileToPrinter = string.Format("/t \"{0}\" \"{1}\"", pdfFileName, printername);
                                var args = string.Format("{0} {1} {2}", flagNoSplashScreen, flagOpenMinimized, flagPrintFileToPrinter);
                                var startInfo = new ProcessStartInfo
                                {
                                    FileName = path.ToString(),
                                    Arguments = args,
                                    CreateNoWindow = true,
                                    ErrorDialog = false,
                                    UseShellExecute = false,
                                    WindowStyle = ProcessWindowStyle.Hidden
                                };
                                // proc.StartInfo.Arguments = args;
                                proc.StartInfo = startInfo;
                            }
                            proc.Start();
                            processID = proc.Id;
                            if (proc.HasExited == false)
                            {
                                proc.WaitForExit(adobeDelayTime);
                            }
                            proc.EnableRaisingEvents = true;
                            proc.Close();
                            KillAdobe("AcroRd32");
                            return true;
                        }
                        else
                        {
                            CustomMessageBox.Show(adobereader_text, "Adobe", CustomMessageBoxIcon.Warning);
                            return false;
                        }
                    }
                    else
                    {
                        CustomMessageBox.Show(adobereader_text, "Adobe", CustomMessageBoxIcon.Warning);
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        //For whatever reason, sometimes adobe likes to be a stage 5 clinger.
        //So here we kill it with fire.
        private static bool KillAdobe(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses().Where(
                         clsProcess => clsProcess.Id == processID))
            {
                clsProcess.Kill();
                return true;
            }
            return false;
        }
    }
}
