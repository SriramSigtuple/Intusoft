using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using NLog.Targets;

namespace Common
{
    public static class ExceptionLogWriter
    {
        
        /// <summary>
        /// It will write the exception to the log and close the application
        /// </summary>
        /// <param name="exceptionMessage"></param>
        public static void WriteLog(Exception exceptionMessage,Logger logger)
        {
            
            LogClass.GetInstance().WriteLogs2File();
            Exception2StringConverter ex = Exception2StringConverter.GetInstance();
            string exceptionStr =  ex.ConvertException2String(exceptionMessage);
            logger.Info(exceptionStr);
            CustomMessageBox.Show(exceptionStr,"Exception");
            Environment.Exit(0);
           
        }
    }
}
