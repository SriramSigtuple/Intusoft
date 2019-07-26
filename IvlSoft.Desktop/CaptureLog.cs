using System;
using System.Collections.Generic;
using System.Linq;
using INTUSOFT.Imaging;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Desktop
{
    public class CaptureLog
    {
        public ushort currentLiveGain;
        public uint currentLiveExposure;
        public ushort currentFlashGain;
        public uint currentFlashExposure;
        public  Imaging.ImagingMode currentCameraType;
        public CaptureLog()
        {

        }
    }

    public class MaskSettings
    {
        public int MaskWidth;
        public int MaskHeight;
        public int ImageCenterX;
        public int ImageCenterY;
        public MaskSettings()
        {

        }
    }
}
