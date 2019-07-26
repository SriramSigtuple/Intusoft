using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using NLog;
using NLog.Targets;
namespace WindowsFormsApplication1.AdvancedSettings

{
    [Serializable]
    public class PrinterSettings
    {
        public static Logger Exception_Log = LogManager.GetLogger("DB_Porting.ExceptionLog");

        //public int adobePrintDelay = 20000;
        private static IVLControlProperties adobePrintDelay = null;

        public IVLControlProperties _adobePrintDelay
        {
            get { return PrinterSettings.adobePrintDelay; }
            set { PrinterSettings.adobePrintDelay = value; }
        }
        public PrinterSettings()
        {

            try
            {
                _adobePrintDelay = new IVLControlProperties();
                _adobePrintDelay.name = "adobePrintDelay";
                _adobePrintDelay.val = "20000";
                _adobePrintDelay.type = "int";
                _adobePrintDelay.control = "System.Windows.Forms.NumericUpDown";
                _adobePrintDelay.text = "Adobe Print Delay";
                _adobePrintDelay.min = 10000;
                _adobePrintDelay.max = 30000;
                _adobePrintDelay.range = _adobePrintDelay.min.ToString() + " to " + _adobePrintDelay.max.ToString();
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            


        }
    }
}
