using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
namespace INTUSOFT.Configuration
{
    public enum ImagingMode { Posterior_45, FFA, Anterior_Prime, Posterior_Prime, FFAColor, FFA_Plus, Anterior_FFA, Anterior_45 };
    
   public static class ConfigVariables
    {
        public static IVLConfig _ivlConfig ;

        public static Settings CurrentSettings;
        public static void GetCurrentSettings()
        {
            if (_ivlConfig == null)
                _ivlConfig = IVLConfig.getInstance();
            switch (_ivlConfig.Mode)
            {
                case ImagingMode.Posterior_Prime:
                    {
                        CurrentSettings = _ivlConfig.PrimePosteriorSettings.Settings;
                        if (CurrentSettings.CameraSettings._SaveFramesCount.val == "8")
                            CurrentSettings.CameraSettings._SaveFramesCount.val = "10";
                       
                        break;
                    }
                case ImagingMode.Anterior_Prime:
                    {
                        CurrentSettings = _ivlConfig.AnteriorSettings.Settings;
                        if (CurrentSettings.CameraSettings._SaveFramesCount.val == "8")
                            CurrentSettings.CameraSettings._SaveFramesCount.val = "10";
                        break;
                    }
                case ImagingMode.Anterior_45:
                    {
                        CurrentSettings = _ivlConfig.AnteriorSettings.Settings;
                        break;
                    }
                case ImagingMode.Anterior_FFA:
                    {
                        CurrentSettings = _ivlConfig.AnteriorSettings.Settings;
                        break;
                    }

                case ImagingMode.Posterior_45:
                    {
                        CurrentSettings = _ivlConfig.FortyFiveSettings.Settings;

                        break;
                    }
                case ImagingMode.FFAColor:
                    {
                        CurrentSettings = _ivlConfig.FfaColorSettings.Settings;
                        break;
                    }
                case ImagingMode.FFA_Plus:
                    {
                        CurrentSettings = _ivlConfig.FfaSettings.Settings;
                        break;
                    }

            }
            if (CurrentSettings.CameraSettings.DeviceID.length != 16)
                CurrentSettings.CameraSettings.DeviceID.length = 16;
            SetNegativeRangeForHSSettings();
            if( CurrentSettings.CameraSettings.IRCheckValue.max != 255)
            CurrentSettings.CameraSettings.IRCheckValue.max = 255;
        }


       /// <summary>
       /// To set the negative values for the hot spot correction added by kishore on 17 august 2017.
       /// </summary>
        private static void SetNegativeRangeForHSSettings()
        {
            if( CurrentSettings.PostProcessingSettings.HotSpotSettings._GainSlope.min != - CurrentSettings.PostProcessingSettings.HotSpotSettings._GainSlope.max)
                CurrentSettings.PostProcessingSettings.HotSpotSettings._GainSlope.min = -CurrentSettings.PostProcessingSettings.HotSpotSettings._GainSlope.max;

            if (CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotBluePeak.min != -CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotBluePeak.max)
                CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotBluePeak.min = -CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotBluePeak.max;

            if (CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotBlueRadius.min != -CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotBlueRadius.max)
                CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotBlueRadius.min = -CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotBlueRadius.max;

            if (CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotGreenPeak.min != -CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotGreenPeak.max)
                CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotGreenPeak.min = -CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotGreenPeak.max;

            if (CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotGreenRadius.min != -CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotGreenRadius.max)
                CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotGreenRadius.min = -CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotGreenRadius.max;

            if (CurrentSettings.PostProcessingSettings.HotSpotSettings._HotspotRadius1.min != -CurrentSettings.PostProcessingSettings.HotSpotSettings._HotspotRadius1.max)
                CurrentSettings.PostProcessingSettings.HotSpotSettings._HotspotRadius1.min = -CurrentSettings.PostProcessingSettings.HotSpotSettings._HotspotRadius1.max;

            if (CurrentSettings.PostProcessingSettings.HotSpotSettings._HotspotRadius2.min != -CurrentSettings.PostProcessingSettings.HotSpotSettings._HotspotRadius2.max)
                CurrentSettings.PostProcessingSettings.HotSpotSettings._HotspotRadius2.min = -CurrentSettings.PostProcessingSettings.HotSpotSettings._HotspotRadius2.max;

            if (CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotRedPeak.min != -CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotRedPeak.max)
                CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotRedPeak.min = -CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotRedPeak.max;

            if (CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotRedRadius.min != -CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotRedRadius.max)
                CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotRedRadius.min = -CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotRedRadius.max;

            if (CurrentSettings.PostProcessingSettings.HotSpotSettings._IsApplyHotspotCorrection.min != -CurrentSettings.PostProcessingSettings.HotSpotSettings._IsApplyHotspotCorrection.max)
                CurrentSettings.PostProcessingSettings.HotSpotSettings._IsApplyHotspotCorrection.min = -CurrentSettings.PostProcessingSettings.HotSpotSettings._IsApplyHotspotCorrection.max;

            if (CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowBlueBPercentage.min != -CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowBlueBPercentage.max)
                CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowBlueBPercentage.min = -CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowBlueBPercentage.max;

            if (CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowGreenPercentage.min != -CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowGreenPercentage.max)
                CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowGreenPercentage.min = -CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowGreenPercentage.max;

            if (CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowRadSpot1.min != -CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowRadSpot1.max)
                CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowRadSpot1.min = -CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowRadSpot1.max;

            if (CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowradSpot2.min != -CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowRadSpot1.max)
                CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowRadSpot1.min = -CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowRadSpot1.max;

            if (CurrentSettings.PostProcessingSettings.ClaheSettings._ClipValueB.val == "0.002")
                CurrentSettings.PostProcessingSettings.ClaheSettings._ClipValueB.val = "0.5";

            if (CurrentSettings.PostProcessingSettings.ClaheSettings._ClipValueG.val == "0.002")
                CurrentSettings.PostProcessingSettings.ClaheSettings._ClipValueG.val = "0.5";

            if (CurrentSettings.PostProcessingSettings.ClaheSettings._ClipValueR.val == "0.002")
                CurrentSettings.PostProcessingSettings.ClaheSettings._ClipValueR.val = "0.5";

            if (CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowRedPercentage.min != -CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowRedPercentage.max)
                CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowRedPercentage.min = -CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowRedPercentage.max;

            if (CurrentSettings.PostProcessingSettings.UnsharpMaskSettings._UnSharpRadius.val == "9")
                CurrentSettings.PostProcessingSettings.UnsharpMaskSettings._UnSharpRadius.val = "5";
            if (CurrentSettings.PostProcessingSettings.UnsharpMaskSettings._UnSharpAmount.val == "0.5")
                CurrentSettings.PostProcessingSettings.UnsharpMaskSettings._UnSharpAmount.val = "1";

            if (CurrentSettings.CameraSettings._exposureIndex.val != "63")
                CurrentSettings.CameraSettings._exposureIndex.val = "63";

            //if (CurrentSettings.FirmwareSettings.PotOffsetValue.val != "10")
            //    CurrentSettings.FirmwareSettings.PotOffsetValue.val = "10";
        }
        public static void SetCurrentSettings()
        {
            if (_ivlConfig == null)
                _ivlConfig = IVLConfig.getInstance();
            switch (_ivlConfig.Mode)
            {
                case ImagingMode.Posterior_Prime:
                    {
                        _ivlConfig.PrimePosteriorSettings.Settings = CurrentSettings;
                        break;
                    }
                case ImagingMode.Anterior_Prime:
                    {
                        _ivlConfig.AnteriorSettings.Settings = CurrentSettings;
                        break;
                    }
                case ImagingMode.Anterior_45:
                    {
                        _ivlConfig.AnteriorSettings.Settings = CurrentSettings;
                        break;
                    }
                case ImagingMode.Anterior_FFA:
                    {
                        _ivlConfig.AnteriorSettings.Settings = CurrentSettings;
                        break;
                    }
                case ImagingMode.Posterior_45:
                    {
                        _ivlConfig.FortyFiveSettings.Settings = CurrentSettings;
                        break;
                    }
                case ImagingMode.FFAColor:
                    {
                        _ivlConfig.FfaColorSettings.Settings = CurrentSettings;
                        break;
                    }
                case ImagingMode.FFA_Plus:
                    {
                        _ivlConfig.FfaSettings.Settings = CurrentSettings;
                        break;
                    }

            }
        }

        public static object StringVal2Object(IVLControlProperties prop)
        {
            object ConvertedVal = null;
            switch (prop.type)
            {
                case "string":
                    {
                        ConvertedVal = prop.val;
                        break;
                    }
                case "int":
                    {
                        ConvertedVal = Convert.ToInt32(prop.val);
                        break;
                    }
                case "double":
                    {
                        ConvertedVal = Convert.ToDouble(prop.val);

                        ConvertedVal = prop.val;
                        break;
                    }
                case "float":
                    {
                        ConvertedVal = Convert.ToSingle(prop.val); ;
                        break;
                    }
                case "byte":
                    {
                        ConvertedVal = Convert.ToByte(prop.val); ;
                        break;
                    }
                case "bool":
                    {
                        ConvertedVal = Convert.ToBoolean(prop.val);
                        break;
                    }
                default:
                    {
                        ConvertedVal = prop.val;
                        break;
                    }



            }
            return ConvertedVal;

        }

    }
}
