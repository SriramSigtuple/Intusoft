using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using NLog;
using NLog.Config;
using NLog.Targets;
using Common;


namespace AssemblySoftware
{
    public class UISettings
    {
        public bool IRON = true;
        public bool EnableLivePP = false;
        public bool LowFlash = false;
        public byte LiveLEDLight = 1;
        public bool liveIR = false;
        public bool liveFL = false;
        public bool liveBlue = false;
        public bool liveAntIR = false;
        public bool liveAntFL = false;
        public bool liveAntBlue = false;

        public bool captureIR = false;
        public bool captureFL = false;
        public bool captureBlue = false;
        public bool captureAntIR = false;
        public bool captureAntFL = false;
        public bool captureAntBlue = false;

        public int LiveGainVal = 100;
        public int CaptureGainVal = 100;
        public int CaptureExposureVal = 77200;
        public int LiveExposureVal = 77200;
        public bool isRawMode = true;
        public bool isSingleFrame = false;
        public bool isFlashBoost = false;
        public int FlashBoostVal = 70;
        public int FlashOffset = 82;
        public int FlashOffset1 = 82;
        public int FlashOffset2 = 82;
        public bool isApplyShift = false;
        public bool isApplyHSCorrection = false;
        public bool isApplyCC = false;
        public bool isApplyMask = false;
        public bool isFFAMode = false;
        public bool isApplyPP = false;
        public int MotorStepsVal = 25;
        public string Model = "A";
        public int frameSaveCount = 8;
        public bool isGreenLive = false;
        public int temperature = 6500;
        public int tint = 1200;
        public int startFrameIndx = 3;
        public int endFrameIndx = 7;
        public int displayFrameIndx = 5;
        public int brightnessValue = 0;
        public int contrastValue = 0;
        public bool isApplyBrightness = false;
        public bool isApplyContrast = false;
        public bool isHorizontalFlip = false;
        public bool isVerticalFlip = false;
        public bool isContinousCapture = false;
        public bool showInExternalViewer = false;
        public bool isApplyUnsharp = false;
        public bool isApplyLiveCC = false;
        public bool isMotorForward = false;
        public bool isForteenBit = false;
        public bool isRawSave = false;
        public bool isRawImageSave = false;
        public bool isProcessedImageSave = false;
        public bool isIRImageSave = false;

        public bool isApplyLUT = false;
        public bool isApplyClahe = false;
        public bool isApplyHSVBoost = false;
        public float HSVBoostVal = 1.5f;
        public float ClaheClipValR = 0.002f;
        public float ClaheClipValG = 0.002f;
        public float ClaheClipValB = 0.002f;
        public int redGain = 0;
        public int greenGain = 0;
        public int blueGain = 0;
        public float FrameDetectionValue = 2.5f;
        public int DarkFrameDetectionValue = 100;
        public bool EnableTemperatureTint = false;
        public bool EnableCC = false;
        public int FFA_Pot_Int_Offset = 10;
        public int FFA_Color_Pot_Int_Offset = 10;
        public int Green_Filter_Pos = 1500;
        public int Blue_Filter_Pos = 1500;

        public ColorCorrectionMatrix ccMatrix;
        public UnsharpMaskSettings unsharpMaskSettings;
        public ShiftVals shiftSettings;
        public HotSpotSetttings hotspotSettings;
        public ImageCentreSettings imageCentreSettings;
        public AnalogGain analogGainSettings;
        public LUT_Settings lutSettings;
        public RedChannel_LUT_Settings redChannelLutSettings;
        public GreenChannel_LUT_Settings greenChannelLutSettings;
        public BlueChannel_LUT_Settings blueChannelLutSettings;
        public GammaCorrectionSettings gammaCorrectionSettings;

        public UISettings()
        {
            ccMatrix = new ColorCorrectionMatrix();
            unsharpMaskSettings = new UnsharpMaskSettings();
            shiftSettings = new ShiftVals();
            hotspotSettings = new HotSpotSetttings();
            imageCentreSettings = new ImageCentreSettings();
            analogGainSettings = new AnalogGain();
            lutSettings = new LUT_Settings();
            redChannelLutSettings = new RedChannel_LUT_Settings();
            greenChannelLutSettings = new GreenChannel_LUT_Settings();
            blueChannelLutSettings = new BlueChannel_LUT_Settings();
            gammaCorrectionSettings = new GammaCorrectionSettings();
        }

    }
    public class GammaCorrectionSettings
    {
        public bool ApplyGammaCorrection = false;
        public float GammaCorrectionValue = 0.5f;

        public GammaCorrectionSettings()
        {

        }
    }
    public class AnalogGain
    {
        public int liveR = 0;
        public int liveG = 0;
        public int liveB = 0;
        public int CaptureR = 0;
        public int CaptureG = 0;
        public int CaptureB = 0;
        public AnalogGain()
        {

        }
    }
    public class ColorCorrectionMatrix
    {
        public float rrVal = 1.0f;
        public float rgVal = 0.4f;
        public float rbVal = 0.0f;
        public float grVal = 0.0f;
        public float ggVal = 0.95f;
        public float gbVal = 0.0f;
        public float brVal = 0.0f;
        public float bgVal = 0.2f;
        public float bbVal = 1.5f;
        public ColorCorrectionMatrix()
        {

        }
    }
    public class UnsharpMaskSettings
    {
        public float thresh = 40f;
        public int radius = 9;
        public float amount = 0.5f;
        public int medianFilterWindow = 3;
        public UnsharpMaskSettings()
        {

        }
    }
    public class ImageCentreSettings
    {
        public int CentreX = 1024;
        public int CentreY = 768;
        public int CaptureMaskWidth = 2000;
        public int CaptureMaskHeight = 2000;
        public int LiveMaskWidth = 2000;
        public int LiveMaskHeight = 2000;
        public bool ApplyLiveMask = false;
        public ImageCentreSettings()
        {

        }
    }
    public class HotSpotSetttings
    {
        public bool isApplyHotspotCorrection = true;
        public float HotSpotCorrectionFactor = 0.10f;
        public int HotSpotRadius = 350;
        public int hotspotRadius1 = -20;
        public int hotspotRadius2 = 100;
        public int ShadowRadSpot1 = 170;
        public int ShadowradSpot2 = 400;
        public int gainSlope = 5;
        public bool isNewMethod = false;
        public float factorR = 1f;
        public float factorG = 1f;
        public float factorB = 1f;
        public int method = 4;
        public int valueR = 25;
        public int valueG = 16;
        public int valueB = 1;
        public int hotspotOffsetR = 10;
        public int hotspotOffsetG = 15;
        public int hotspotOffsetB = 3;

        public int HotSpotRedPeak;
        public int HotSpotGreenPeak;
        public int HotSpotBluePeak;
        public int HotSpotRedRadius;
        public int HotSpotGreenRadius;
        public int HotSpotBlueRadius;
        public int ShadowRedPeakPercentage = 20;
        public int ShadowGreenPeakPercentage = 15;
        public int ShadowBluePeakPercentage = 10;

        public HotSpotSetttings()
        {

        }
    }
    public class ShiftVals
    {
        public int shiftX = 5;
        public int shiftY = 4;
        public ShiftVals()
        {

        }
    }
    public class LUT_Settings
    {
        public int SineFactor = 64;
        public int Interval_1 = 64;
        public int Interval_2 = 160;
        public int offset = 0;
        public LUT_Settings()
        {

        }
    }

    public class RedChannel_LUT_Settings
    {
        public int SineFactorR = 64;
        public int Interval_1R = 64;
        public int Interval_2R = 160;
        public int offsetR = 0;
        public RedChannel_LUT_Settings()
        {

        }
    }

    public class GreenChannel_LUT_Settings
    {
        public int SineFactorG = 64;
        public int Interval_1G = 64;
        public int Interval_2G = 160;
        public int offsetG = 0;
        public GreenChannel_LUT_Settings()
        {

        }
    }

    public class BlueChannel_LUT_Settings
    {
        public int SineFactorB = 64;
        public int Interval_1B = 64;
        public int Interval_2B = 160;
        public int offsetB = 0;
        public BlueChannel_LUT_Settings()
        {

        }
    }


    public class SettingsConfig
    {
        public int mode = 0;
        public FFA_UI_Settings FFA_Settings;
        public Lite_UI_Settings Lite_Settings;
        public fortyFive_UI_Settings FortyFive_Settings;
        public Anterior_UI_Settings Anterior_Settings;
        public FOURTY_FIVE_PLUS_COLOR_UI_Settings FourtyFivePlus_Color_Settings;
        public FOURTY_FIVE_PLUS_FFA_UI_Settings FourtyFivePlus_FFA_Settings;
        private static SettingsConfig _settingsConfig;
        public static string fileName = @"SettingsConfig.xml";
        public static Logger Exception_Log = LogManager.GetLogger("DB_Porting.ExceptionLog");

        public SettingsConfig()
        {
            FFA_Settings = new FFA_UI_Settings();
            Lite_Settings = new Lite_UI_Settings();
            FortyFive_Settings = new fortyFive_UI_Settings();
            Anterior_Settings = new Anterior_UI_Settings();
            FourtyFivePlus_Color_Settings = new FOURTY_FIVE_PLUS_COLOR_UI_Settings();
            FourtyFivePlus_FFA_Settings = new FOURTY_FIVE_PLUS_FFA_UI_Settings();
        }
        public static SettingsConfig GetInstance()
        {
            if (_settingsConfig == null)
            {


                try
                {

                    Type t = typeof(SettingsConfig);
                    FileInfo filePath = new FileInfo(fileName);
                    if (filePath.IsReadOnly)
                        filePath.IsReadOnly = false;
                    _settingsConfig = (SettingsConfig)Deserialize(t, fileName);
                }
                catch (Exception ex)
                {
                    //Logger Exception_Log = LogManager.GetLogger("AssemblySoftware.ExceptionLog");
                    //Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);

                    //exceptionLog.Debug(ex.StackTrace);
                    _settingsConfig = new SettingsConfig();
                    Serialize(_settingsConfig, fileName);// This line has been added in order to save ivlconfig file if file is not present in the executable directory by sriram

                }
            }
            return _settingsConfig;
        }
        public static void Serialize(Object data, string fileName)
        {
            try
            {
                Type type = data.GetType();
                XmlSerializer xs = new XmlSerializer(type);
                XmlTextWriter xmlWriter = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);
                xmlWriter.Formatting = Formatting.Indented;
                xs.Serialize(xmlWriter, data);
                xmlWriter.Close();
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        public static Object Deserialize(Type type, string fileName)
        {

            try
            {
                XmlSerializer xs = new XmlSerializer(type);

                XmlTextReader xmlReader = new XmlTextReader(fileName);
                Object data = xs.Deserialize(xmlReader);

                xmlReader.Close();

                return data;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
                XmlTextReader xmlReader = new XmlTextReader(fileName);
                return xmlReader;

            }
            
        }
    }
    public class Anterior_UI_Settings
    {
        public UISettings UI_Settings;
        public Anterior_UI_Settings()
        {
            this.UI_Settings = new UISettings();

            this.UI_Settings.CaptureGainVal = 100;
            this.UI_Settings.CaptureExposureVal = 77200;
            this.UI_Settings.LiveExposureVal = 77200;
            this.UI_Settings.isRawMode = true;
            this.UI_Settings.isSingleFrame = false;
            this.UI_Settings.isFlashBoost = false;
            this.UI_Settings.FlashBoostVal = 70;
            this.UI_Settings.FlashOffset = 82;
            this.UI_Settings.FlashOffset1 = 82;
            this.UI_Settings.FlashOffset2 = 82;
            this.UI_Settings.isApplyShift = false;
            this.UI_Settings.isApplyHSCorrection = false;
            this.UI_Settings.isApplyCC = false;
            this.UI_Settings.isApplyMask = false;
            this.UI_Settings.isFFAMode = false;
            this.UI_Settings.isApplyPP = false;
            this.UI_Settings.MotorStepsVal = 25;
            this.UI_Settings.Model = "A";
            this.UI_Settings.frameSaveCount = 8;
            this.UI_Settings.isGreenLive = false;
            this.UI_Settings.temperature = 6500;
            this.UI_Settings.tint = 1200;
        }
    }
    public class FFA_UI_Settings
    {
        public UISettings UI_Settings;
        public FFA_UI_Settings()
        {
            this.UI_Settings = new UISettings();

            this.UI_Settings.CaptureGainVal = 100;
            this.UI_Settings.CaptureExposureVal = 77200;
            this.UI_Settings.LiveExposureVal = 77200;
            this.UI_Settings.isRawMode = true;
            this.UI_Settings.isSingleFrame = false;
            this.UI_Settings.isFlashBoost = false;
            this.UI_Settings.FlashBoostVal = 70;
            this.UI_Settings.FlashOffset = 82;
            this.UI_Settings.FlashOffset1 = 82;
            this.UI_Settings.FlashOffset2 = 82;
            this.UI_Settings.isApplyShift = false;
            this.UI_Settings.isApplyHSCorrection = false;
            this.UI_Settings.isApplyCC = false;
            this.UI_Settings.isApplyMask = false;
            this.UI_Settings.isFFAMode = false;
            this.UI_Settings.isApplyPP = false;
            this.UI_Settings.MotorStepsVal = 25;
            this.UI_Settings.Model = "A";
            this.UI_Settings.frameSaveCount = 8;
            this.UI_Settings.isGreenLive = false;
            this.UI_Settings.temperature = 6500;
            this.UI_Settings.tint = 1200;
        }
    }
    public class Lite_UI_Settings
    {
        public UISettings UI_Settings;
        public Lite_UI_Settings()
        {
            this.UI_Settings = new UISettings();
            #region Default Lut Settings Value Prime
            this.UI_Settings.lutSettings.Interval_1 = 50;
            this.UI_Settings.lutSettings.Interval_2 = 170;
            this.UI_Settings.lutSettings.offset = 25;
            this.UI_Settings.lutSettings.SineFactor = 40;

            #endregion

            #region Default CC Settings Value Prime
            this.UI_Settings.ccMatrix.rrVal = 1.2f;
            this.UI_Settings.ccMatrix.rgVal = 0.2f;
            this.UI_Settings.ccMatrix.rbVal = 0f;
            this.UI_Settings.ccMatrix.grVal = 0f;
            this.UI_Settings.ccMatrix.ggVal = 0.95f;
            this.UI_Settings.ccMatrix.gbVal = 0f;
            this.UI_Settings.ccMatrix.brVal = 0f;
            this.UI_Settings.ccMatrix.bgVal = 0f;
            this.UI_Settings.ccMatrix.bbVal = 1.0f;

            #endregion
            #region Default UnSharp Settings Value Prime
            this.UI_Settings.unsharpMaskSettings.amount = .5f;

            #endregion
            this.UI_Settings.CaptureGainVal = 300;
            this.UI_Settings.CaptureExposureVal = 100000;
            this.UI_Settings.LiveExposureVal = 100000;
            this.UI_Settings.isRawMode = false;
            this.UI_Settings.isSingleFrame = true;
            this.UI_Settings.isFlashBoost = true;
            this.UI_Settings.FlashBoostVal = 95;
            this.UI_Settings.FlashOffset1 = 90;
            this.UI_Settings.FlashOffset2 = 10;
            this.UI_Settings.isApplyShift = true;
            this.UI_Settings.isApplyHSCorrection = false;
            this.UI_Settings.isApplyCC = true;
            this.UI_Settings.isApplyMask = false;
            this.UI_Settings.isFFAMode = false;
            this.UI_Settings.isApplyPP = true;
            this.UI_Settings.MotorStepsVal = 10;
            this.UI_Settings.Model = "A";
            this.UI_Settings.frameSaveCount = 8;
            this.UI_Settings.isGreenLive = true;
            this.UI_Settings.temperature = 6500;
            this.UI_Settings.tint = 1200;
            this.UI_Settings.isApplyClahe = true;
            this.UI_Settings.isApplyLUT = true;
            this.UI_Settings.isApplyPP = true;
            this.UI_Settings.isApplyUnsharp = true;
            this.UI_Settings.isForteenBit = true;
            this.UI_Settings.startFrameIndx = 1;
            this.UI_Settings.endFrameIndx = 7;
            this.UI_Settings.frameSaveCount = 9;
            this.UI_Settings.isVerticalFlip = true;

        }
    }
    public class fortyFive_UI_Settings
    {
        public UISettings UI_Settings;

        public fortyFive_UI_Settings()
        {
            this.UI_Settings = new UISettings();
            #region Default Lut Settings Value 45
            this.UI_Settings.lutSettings.Interval_1 = 50;
            this.UI_Settings.lutSettings.Interval_2 = 130;
            this.UI_Settings.lutSettings.offset = 0;
            this.UI_Settings.lutSettings.SineFactor = 35;
            #endregion

            #region Default CC Settings Value 45
            this.UI_Settings.ccMatrix.rrVal = 1.2f;
            this.UI_Settings.ccMatrix.rgVal = 0.2f;
            this.UI_Settings.ccMatrix.rbVal = 0f;
            this.UI_Settings.ccMatrix.grVal = 0f;
            this.UI_Settings.ccMatrix.ggVal = 0.95f;
            this.UI_Settings.ccMatrix.gbVal = 0f;
            this.UI_Settings.ccMatrix.brVal = 0f;
            this.UI_Settings.ccMatrix.bgVal = 0.2f;
            this.UI_Settings.ccMatrix.bbVal = 1.3f;
            #endregion


            this.UI_Settings.CaptureGainVal = 100;
            this.UI_Settings.CaptureExposureVal = 77376;
            this.UI_Settings.LiveExposureVal = 77376;
            this.UI_Settings.isRawMode = true;
            this.UI_Settings.isSingleFrame = false;
            this.UI_Settings.isFlashBoost = false;
            this.UI_Settings.FlashBoostVal = 70;
            this.UI_Settings.FlashOffset = 82;
            this.UI_Settings.FlashOffset1 = 9;
            this.UI_Settings.FlashOffset2 = 0;
            this.UI_Settings.isApplyShift = true;
            this.UI_Settings.isApplyHSCorrection = true;
            this.UI_Settings.isApplyCC = true;
            this.UI_Settings.isApplyMask = false;
            this.UI_Settings.isFFAMode = false;
            this.UI_Settings.isApplyPP = true;
            this.UI_Settings.MotorStepsVal = 25;
            this.UI_Settings.Model = "B";
            this.UI_Settings.frameSaveCount = 8;
            this.UI_Settings.isGreenLive = false;
            this.UI_Settings.temperature = 6500;
            this.UI_Settings.tint = 1200;

        }
    }
    public class FOURTY_FIVE_PLUS_FFA_UI_Settings
    {
        public UISettings UI_Settings;
        public FOURTY_FIVE_PLUS_FFA_UI_Settings()
        {
            this.UI_Settings = new UISettings();

            this.UI_Settings.CaptureGainVal = 200;
            this.UI_Settings.CaptureExposureVal = 77376;
            this.UI_Settings.LiveExposureVal = 77376;
            this.UI_Settings.isRawMode = true;
            this.UI_Settings.isSingleFrame = true;
            this.UI_Settings.isFlashBoost = true;
            this.UI_Settings.FlashBoostVal = 70;
            this.UI_Settings.FlashOffset = 82;
            this.UI_Settings.FlashOffset1 = 90;
            this.UI_Settings.FlashOffset2 = 10;
            this.UI_Settings.isApplyShift = false;
            this.UI_Settings.isApplyHSCorrection = false;
            this.UI_Settings.isApplyCC = false;
            this.UI_Settings.isApplyMask = false;
            this.UI_Settings.isFFAMode = false;
            this.UI_Settings.isApplyPP = false;
            this.UI_Settings.MotorStepsVal = 25;
            this.UI_Settings.Model = "A";
            this.UI_Settings.frameSaveCount = 8;
            this.UI_Settings.isGreenLive = false;
            this.UI_Settings.temperature = 6500;
            this.UI_Settings.tint = 1200;
            
        }
    }
    public class FOURTY_FIVE_PLUS_COLOR_UI_Settings
    {
        public UISettings UI_Settings;
        public FOURTY_FIVE_PLUS_COLOR_UI_Settings()
        {
            this.UI_Settings = new UISettings();

            this.UI_Settings.CaptureGainVal = 200;
            this.UI_Settings.CaptureExposureVal = 77376;
            this.UI_Settings.LiveExposureVal = 77376;
            this.UI_Settings.isRawMode = true;
            this.UI_Settings.isSingleFrame = true;
            this.UI_Settings.isFlashBoost = true;
            this.UI_Settings.FlashBoostVal = 70;
            this.UI_Settings.FlashOffset = 82;
            this.UI_Settings.FlashOffset1 = 90;
            this.UI_Settings.FlashOffset2 = 10;
            this.UI_Settings.isApplyShift = false;
            this.UI_Settings.isApplyHSCorrection = false;
            this.UI_Settings.isApplyCC = false;
            this.UI_Settings.isApplyMask = false;
            this.UI_Settings.isFFAMode = false;
            this.UI_Settings.isApplyPP = false;
            this.UI_Settings.MotorStepsVal = 25;
            this.UI_Settings.Model = "A";
            this.UI_Settings.frameSaveCount = 8;
            this.UI_Settings.isGreenLive = false;
            this.UI_Settings.temperature = 6500;
            this.UI_Settings.tint = 1200;
        }
    }
}
