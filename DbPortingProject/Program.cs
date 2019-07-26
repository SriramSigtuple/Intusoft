using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;
using NLog;
using NLog.Targets;
//using AdminAdditionOperation;

namespace DBPorting
{
    static class Program
    {

        static Logger Exception_Log = LogManager.GetLogger("DB_Porting.ExceptionLog");
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new DbTransferForm());
            }
            catch (Exception ex)
            {

                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);

            }
          
            //InitializationOfResourceStrings();
            //Application.Run(new AdminAdditionOperation.Forms.MainForm());
        }
    }
}
