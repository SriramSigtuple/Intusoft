using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
namespace INTUSOFT.Desktop.AdvancedSettings
{
    [Serializable]
    public class PrinterSettings
    {

        //public int adobePrintDelay = 20000;
        private  IVLControlProperties adobePrintDelay = null;

        public IVLControlProperties _adobePrintDelay
        {
            get { return adobePrintDelay; }
            set { adobePrintDelay = value; }
        }
        public PrinterSettings()
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
    }
}
