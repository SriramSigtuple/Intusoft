using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTUSOFT.Imaging
{
  public abstract class CameraAbstract
    {
      public abstract bool StartLiveMode();
      public abstract bool StopLiveMode();
      public abstract void CloseCamera();
      public abstract void Capture();
      public abstract bool GetResolution(ref Dictionary<int, int> WidthHeight);
      public abstract bool setResolution(int mode);
      public abstract bool SetGamma(int gammaVal);
      public abstract bool SetGain(ushort val);
      public abstract bool SetExposure(uint val);
      public abstract bool EnableAutoExposure(bool enable);
      public abstract bool GetRGBGain(int[] RGB);
      public abstract bool SetFrameRateLevel(int level);
      public abstract bool GetFrameRate(ref ushort val);
      public abstract bool GetExposureRange(ref uint minVal, ref uint maxVal, ref uint defVal);
      public abstract bool GetGainRange(ref ushort minVal, ref ushort maxVal, ref ushort defVal);
      public abstract bool GetGamma(ref int gammaVal);
      public abstract void OnEventImage();
      public abstract void onEventStillImage(string fileName);
    }
}
