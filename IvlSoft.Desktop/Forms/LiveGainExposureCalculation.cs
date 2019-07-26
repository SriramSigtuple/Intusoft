using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using INTUSOFT.Data.Repository;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using INTUSOFT.EventHandler;
using CameraModule;
using System.IO;
using INTUSOFT.Custom.Controls;
using System.Threading;
using INTUSOFT.Imaging;
using INTUSOFT.Desktop.Properties;

namespace INTUSOFT.Desktop.Forms
{

    public class GainExposureHelper
    {
       public static  GainExposureHelper gainExposureHelper;
        private GainExposureHelper()
        {

        }

        public static GainExposureHelper getInstance()
        {
            if (gainExposureHelper == null)
            {
                gainExposureHelper = new GainExposureHelper();
            }
                return gainExposureHelper;
        }


        public void SetLiveGain(int GainValue)
        {
            IVLVariables.CurrentSettings.CameraSettings._LiveDigitalGain.val = GainValue.ToString();

            if (IVLVariables.ivl_Camera.camPropsHelper.ImagingMode == ImagingMode.Posterior_45 || IVLVariables.ivl_Camera.camPropsHelper.ImagingMode == ImagingMode.FFAColor || IVLVariables.ivl_Camera.camPropsHelper.ImagingMode == ImagingMode.FFA_Plus)
            {
                IVLVariables.ivl_Camera.UpdateExposureGainFromTable(Convert.ToInt32(IVLVariables.CurrentSettings.CameraSettings._LiveDigitalGain.val), false);
                IVLVariables.ivl_Camera.camPropsHelper._Settings.CameraSettings.LiveGainIndex = GainValue;
                IVLVariables.ivl_Camera.camPropsHelper._Settings.CameraSettings.LiveGain =(ushort)GainValue;  
            }
            else
                IVLVariables.ivl_Camera.camPropsHelper._Settings.CameraSettings.LiveGain = ((ushort)(Convert.ToUInt16(IVLVariables.CurrentSettings.CameraSettings._LiveDigitalGain.val)));
            //IVLVariables.ivl_Camera.camPropsHelper._Settings.BoardSettings.
        }

        //public void SetFlashGain(string flashValue)
        //{
        //    IVLVariables.ivl_Camera.camPropsHelper._Settings.CameraSettings.CaptureGain = Convert.ToUInt16(flashValue);//IVLVariables.CurrentSettings.CameraSettings._DigitalGainLow.val);
        //}

        public void SetFlashGain(int flashValue)
        {
            IVLVariables.ivl_Camera.camPropsHelper._Settings.CameraSettings.CaptureGainIndex = Convert.ToUInt16(flashValue);//IVLVariables.CurrentSettings.CameraSettings._DigitalGainLow.val);
        }
    }
}
