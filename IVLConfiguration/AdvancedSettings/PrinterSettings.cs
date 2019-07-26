using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
namespace INTUSOFT.Configuration.AdvanceSettings
{
    [Serializable]
    public class PrinterSettings
    {
        private static  IVLControlProperties adobePrintDelay = null;

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

    [Serializable]
    public class PrinterPPSettings
    {
        public ColorCorrectionSettings CCSettings;
        public LutSettings LUTSettings;
        public PrinterPPSettings()
        {
            CCSettings = new AdvanceSettings.ColorCorrectionSettings();
            LUTSettings = new LutSettings();
            //PrinterPostProcessingSettings.ClaheSettings._IsApplyClaheSettings.val = "false";
            //PrinterPostProcessingSettings.ImageShiftSettings._IsApplyImageShift.val = "false";
            //PrinterPostProcessingSettings.UnsharpMaskSettings._IsApplyUnsharpSettings.val = "false";
            //PrinterPostProcessingSettings.HotSpotSettings._IsApplyHotspotCorrection.val = "false";


            CCSettings._RRCompensation.val = "1.55";
            CCSettings._RGCompensation.val = "0.0";
            CCSettings._RBCompensation.val = "0.0";
            CCSettings._GRCompensation.val = "0.0";
            CCSettings._GGCompensation.val = "1.2";
            CCSettings._GBCompensation.val = "0.0";
            CCSettings._BRCompensation.val = "0.0";
            CCSettings._BGCompensation.val = "0.0";
            CCSettings._BBCompensation.val = "1.0";


            LUTSettings._LUTInterval1.val = "50";
            LUTSettings._LUTInterval2.val = "130";
            LUTSettings._LUTOffset.val = "0";
            LUTSettings._LUTSineFactor.val = "20";


        }

    }
}
