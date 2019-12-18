using INTUSOFT.Configuration.AdvancedSettings;
using INTUSOFT.Configuration.AdvanceSettings;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
namespace INTUSOFT.Configuration
{
    public class IVLConfig
       {
       private static IVLConfig _ivlConfig; 
       public ImagingMode Mode;

       public static string fileName = "";
       private AnteriorSettings _AnteriorSettings;

       public AnteriorSettings AnteriorSettings
       {
           get { return _AnteriorSettings; }
           set { _AnteriorSettings = value; }
       }
       private PrimePosteriorSettings _PrimePosteriorSettings;

       public PrimePosteriorSettings PrimePosteriorSettings
       {
           get { return _PrimePosteriorSettings; }
           set { _PrimePosteriorSettings = value; }
       }

       private FortyFiveSettings fortyFiveSettings;

       public FortyFiveSettings FortyFiveSettings
       {
           get { return fortyFiveSettings; }
           set { fortyFiveSettings = value; }
       }
       private FFAColorSettings ffaColorSettings;

       public FFAColorSettings FfaColorSettings
       {
           get { return ffaColorSettings; }
           set { ffaColorSettings = value; }
       }

       private FFASettings ffaSettings;

       public FFASettings FfaSettings
       {
           get { return ffaSettings; }
           set { ffaSettings = value; }
       }
            
       public static void ResetIVLConfig()
       {
           _ivlConfig = null;
       }
       public IVLConfig()
       {
           AnteriorSettings = new AnteriorSettings();
           PrimePosteriorSettings = new PrimePosteriorSettings();
           FortyFiveSettings = new FortyFiveSettings();
           FfaColorSettings = new FFAColorSettings();
           FfaSettings = new FFASettings();
       }
       private void createIvlConfig()
       {
       }
       public static IVLConfig getInstance()
       {
           if (_ivlConfig == null)
           {
               try
               {

                  Type t =  typeof(IVLConfig);
                 
                  FileInfo filePath = new FileInfo(fileName);
                  {
                      if (filePath.IsReadOnly)
                          filePath.IsReadOnly = false;
                  }
                   
                   _ivlConfig = (IVLConfig)XmlConfigUtility.Deserialize(t, fileName);
               }
               catch (Exception)
               {
                   Type t = typeof(IVLConfig);
                   _ivlConfig = new IVLConfig();
                  
                   {
                       XmlConfigUtility.Serialize(_ivlConfig, fileName);
                   }
               }
           }
           return _ivlConfig;
       }
   }

   public class Settings
   {
       public UserSettings UserSettings;
       public FirmwareSettings FirmwareSettings;
       public PostProcessingSettings PostProcessingSettings;
       public CameraSettings CameraSettings;
       public PrinterSettings PrinterSettings;
       public ImageStorageSettings ImageStorageSettings;
       public UISettings UISettings;
       public AnnotationColorSelection AnnotationColorSelection;
       public ImageNameSettings ImageNameSettings;
       public EmailSettings EmailSettings;
       public CloudSettings CloudSettings;
       public ReportSettings ReportSettings;
       public PrinterPPSettings PrinterPPSettings;
       public MotorOffSetSettings MotorOffSetSettings;
       public DoctorSettings DoctorSettings;
       public Settings()
       {
           UserSettings = new UserSettings();
           AnnotationColorSelection = new AnnotationColorSelection();
           FirmwareSettings = new FirmwareSettings();
           PostProcessingSettings = new PostProcessingSettings();
           CameraSettings = new CameraSettings();
           PrinterSettings = new PrinterSettings();
           ImageStorageSettings = new ImageStorageSettings();
           UISettings = new UISettings();
           ImageNameSettings = new ImageNameSettings();
           EmailSettings = new EmailSettings();
           CloudSettings = new CloudSettings();
           ReportSettings = new ReportSettings();
           PrinterPPSettings = new PrinterPPSettings();
           MotorOffSetSettings = new AdvanceSettings.MotorOffSetSettings();
           DoctorSettings = new DoctorSettings();
       }
   }
   public class AnteriorSettings
   {
       public Settings Settings;

       public AnteriorSettings()
       {
          Settings = new Settings();
          #region Firmware Settings for Anterior Prime
          Settings.FirmwareSettings._MotorCaptureSteps.val = "35";
          Settings.FirmwareSettings._EnableSingleFrameCapture.val = "true";
          Settings.FirmwareSettings._FlashBoostValue.val = "35";// for 45 it is 0 
          Settings.FirmwareSettings._isFlashBoost.val = "true";// for 45 it is false
          Settings.FirmwareSettings._FlashOffsetStart.val = "60";// for 45  it is 65
          Settings.FirmwareSettings._FlashOffsetEnd.val = "30";// for 45 it is 60


          Settings.MotorOffSetSettings.IR2IR.val = "0".ToString();
          Settings.MotorOffSetSettings.IR2Flash.val = "18".ToString();
          Settings.MotorOffSetSettings.IR2Blue.val = "35".ToString();//it is to be decided
          Settings.MotorOffSetSettings.Flash2IR.val = "-100".ToString();
          Settings.MotorOffSetSettings.Flash2Flash.val = "0".ToString();
          Settings.MotorOffSetSettings.Flash2Blue.val = "0".ToString();//it is to be decided
          Settings.MotorOffSetSettings.Blue2IR.val = "-100".ToString();
          Settings.MotorOffSetSettings.Blue2Flash.val = "0".ToString();//it is to be decided
          Settings.MotorOffSetSettings.Blue2Blue.val = "0".ToString();
          #endregion

          Settings.PostProcessingSettings.HotSpotSettings._IsApplyHotspotCorrection.val = "false";

          #region Camera Setting for Anterior Prime not required to change for 45
          Settings.CameraSettings._EnableWB.val = false.ToString();
          Settings.CameraSettings._SaveFramesCount.val = "10";
          Settings.CameraSettings._ImageWidth.val = 3072.ToString();
          Settings.CameraSettings._ImageHeight.val = 2048.ToString();
          Settings.CameraSettings._ImageOpticalCentreX.val = 1250.ToString();
          Settings.CameraSettings._ImageOpticalCentreY.val = 1024.ToString();
          Settings.CameraSettings._ImageHeight.val = 2048.ToString();
          Settings.CameraSettings._ImageROIX.val = 270.ToString();
          Settings.CameraSettings._FlashExposure.val = 100000.ToString();
          Settings.CameraSettings._Exposure.val = 100000.ToString();
          Settings.CameraSettings.DelayAfterFlashOffDone.val = 500.ToString();

          #endregion

          #region UI Settings for Anterior Prime
          Settings.UISettings.LiveImaging._PosteriorVisible.val = true.ToString();// for 45 it is false and for anterior it is false
          Settings.UISettings.LiveImaging._AnteriorVisible.val = true.ToString();// for 45 it is false and for prime it is true
          Settings.UISettings.LiveImaging.FortyFiveButtonVisible.val = false.ToString();// for 45 it is true
          Settings.UISettings.LiveImaging.FfaColorVisible.val = false.ToString();
          Settings.UISettings.LiveImaging.FfaVisible.val = false.ToString();
          Settings.UISettings.LiveImaging.ShowLiveSource.val = false.ToString();

          #endregion

          #region Post Processing Settings for Anterior Prime
          Settings.PostProcessingSettings.ColorCorrectionSettings._RRCompensation.val = "1.20";
          Settings.PostProcessingSettings.ColorCorrectionSettings._RGCompensation.val = "0.20";
          Settings.PostProcessingSettings.ColorCorrectionSettings._RBCompensation.val = "0.0";
          Settings.PostProcessingSettings.ColorCorrectionSettings._GRCompensation.val = "0.0";
          Settings.PostProcessingSettings.ColorCorrectionSettings._GGCompensation.val = "0.95";
          Settings.PostProcessingSettings.ColorCorrectionSettings._GBCompensation.val = "0.0";
          Settings.PostProcessingSettings.ColorCorrectionSettings._BRCompensation.val = "0.0";
          Settings.PostProcessingSettings.ColorCorrectionSettings._BGCompensation.val = "0.0";
          Settings.PostProcessingSettings.ColorCorrectionSettings._BBCompensation.val = "1.0";

          Settings.PostProcessingSettings.LutSettings._LUTSineFactor.val = "40";
          Settings.PostProcessingSettings.LutSettings._LUTInterval1.val = "50";
          Settings.PostProcessingSettings.LutSettings._LUTInterval2.val = "130";
          Settings.PostProcessingSettings.LutSettings._LUTOffset.val = "15";
           #endregion
       }
   }

   public class PrimePosteriorSettings
   {
       public Settings Settings;
       public PrimePosteriorSettings()
       {
           Settings = new Settings();
           #region Firmware Settings for Prime
           Settings.FirmwareSettings._MotorCaptureSteps.val = "10";
           Settings.FirmwareSettings._EnableSingleFrameCapture.val = "true";
           Settings.FirmwareSettings._FlashBoostValue.val = "70";// for 45 it is 0 
           Settings.FirmwareSettings._isFlashBoost.val = "true";// for 45 it is false
           Settings.FirmwareSettings._FlashOffsetStart.val = "60";// for 45  it is 65
           Settings.FirmwareSettings._FlashOffsetEnd.val = "30";// for 45 it is 60


           Settings.MotorOffSetSettings.IR2IR.val = "0".ToString();
           Settings.MotorOffSetSettings.IR2Flash.val = "12".ToString();
           Settings.MotorOffSetSettings.IR2Blue.val = "-100".ToString();
           Settings.MotorOffSetSettings.Flash2IR.val = "-100".ToString();
           Settings.MotorOffSetSettings.Flash2Flash.val = "0".ToString();
           Settings.MotorOffSetSettings.Flash2Blue.val = "-100".ToString();
           Settings.MotorOffSetSettings.Blue2IR.val = "-100".ToString();
           Settings.MotorOffSetSettings.Blue2Flash.val = "-100".ToString();
           Settings.MotorOffSetSettings.Blue2Blue.val = "-100".ToString();
           #endregion

           #region Camera Setting for Prime not required to change for 45
           Settings.CameraSettings._EnableWB.val = false.ToString();
           Settings.CameraSettings._SaveFramesCount.val = "10";
           Settings.CameraSettings._ImageWidth.val = 3072.ToString();
           Settings.CameraSettings._ImageHeight.val = 2048.ToString();
           Settings.CameraSettings._ImageOpticalCentreX.val = 1250.ToString();
           Settings.CameraSettings._ImageOpticalCentreY.val = 1024.ToString();
           Settings.CameraSettings._ImageHeight.val = 2048.ToString();
           Settings.CameraSettings._ImageROIX.val = 270.ToString();
           Settings.CameraSettings._FlashExposure.val = 100000.ToString();
           Settings.CameraSettings._Exposure.val = 100000.ToString();

           Settings.CameraSettings.DelayAfterFlashOffDone.val = 500.ToString();
           #endregion

           #region Post Processing Settings for Prime not required to change for 45
           Settings.PostProcessingSettings.MaskSettings._ApplyLiveMask.val = "true";
           Settings.PostProcessingSettings.MaskSettings.CaptureMaskHeight.val = 2250.ToString();
           Settings.PostProcessingSettings.MaskSettings.CaptureMaskWidth.val = 2250.ToString();
           Settings.PostProcessingSettings.MaskSettings.LiveMaskHeight.val = 2250.ToString();
           Settings.PostProcessingSettings.MaskSettings.LiveMaskWidth.val = 2250.ToString();
           Settings.PostProcessingSettings.HotSpotSettings._IsApplyHotspotCorrection.val = "false";
           Settings.PostProcessingSettings.ClaheSettings._ClipValueR.val = "0.5";
           Settings.PostProcessingSettings.ClaheSettings._ClipValueG.val = "0.5";
           Settings.PostProcessingSettings.ClaheSettings._ClipValueB.val = "0.5";
           #endregion
          

           #region UI Settings for Prime
           Settings.UISettings.LiveImaging._PosteriorVisible.val = true.ToString();// for 45 it is false and for anterior it is false
           Settings.UISettings.LiveImaging._AnteriorVisible.val = true.ToString();// for 45 it is false and for prime it is true
           Settings.UISettings.LiveImaging.FortyFiveButtonVisible.val = false.ToString();// for 45 it is true
           Settings.UISettings.LiveImaging.FfaColorVisible.val = false.ToString();
           Settings.UISettings.LiveImaging.FfaVisible.val = false.ToString();
           Settings.UISettings.LiveImaging.ShowLiveSource.val = false.ToString();

           Settings.CameraSettings._EnableVerticalFlip.val = true.ToString();
           #endregion
       }

   }
   public class FortyFiveSettings
       
   {
       public Settings Settings;
       public FortyFiveSettings()
       {
           Settings = new Settings();
           Settings.CameraSettings._CameraModel.val = "D";
           Settings.CameraSettings._EnableWB.val = "false";
           Settings.CameraSettings._EnableVerticalFlip.val = "false";
           Settings.CameraSettings._exposureIndex.val = "63";
           Settings.CameraSettings._LiveDigitalGain.val = "23";
           Settings.CameraSettings._EnableHorizontalFlip.val = "true";

           Settings.ImageStorageSettings._IsRawSave.val = "true";
           Settings.ImageStorageSettings._IsRawImageSave.val = "true";
           Settings.ImageStorageSettings._IsProcessedImageSave.val = "true";
           Settings.PostProcessingSettings.MaskSettings._IsApplyMask.val = "true";
           Settings.PostProcessingSettings.OverlaySettings.ShowOpticDiscOverlay.val = "false";

           #region Firmware Settings for 45
           Settings.FirmwareSettings._EnableSingleFrameCapture.val = "true";
           Settings.FirmwareSettings._FlashBoostValue.val = "1";// for 45 it is 0 
           Settings.FirmwareSettings._isFlashBoost.val = "false";// for 45 it is false
           Settings.FirmwareSettings._FlashOffsetStart.val = "20";// for 45  it is 65
           Settings.FirmwareSettings._FlashOffsetEnd.val = "17";// for 45 it is 60
           #endregion

           #region UI Settings for 45
           Settings.UISettings.LiveImaging._PosteriorVisible.val = false.ToString();// for 45 it is false and for anterior it is false
           Settings.UISettings.LiveImaging._AnteriorVisible.val = false.ToString();// for 45 it is false and for prime it is true
           Settings.UISettings.LiveImaging.FortyFiveButtonVisible.val = true.ToString();// for 45 it is true
           Settings.UISettings.LiveImaging.FfaColorVisible.val = false.ToString();
           Settings.UISettings.LiveImaging.FfaVisible.val = false.ToString();
           Settings.UISettings.LiveImaging._IRBtnVisible.val = true.ToString();
           Settings.UISettings.LiveImaging._FlashBtnVisible.val = true.ToString();
           Settings.UISettings.LiveImaging.ShowLiveSource.val = false.ToString();


           #endregion

           Settings.PostProcessingSettings.MaskSettings.LiveMaskHeight.val = "2020";//added to set the default height value 
           Settings.PostProcessingSettings.MaskSettings.CaptureMaskHeight.val = "2000";//added to set the default height value 
           Settings.PostProcessingSettings.MaskSettings.LiveMaskWidth.val = "2020";//added to set the default width value 
           Settings.PostProcessingSettings.MaskSettings.CaptureMaskWidth.val = "2000";//added to set the default width value 
           Settings.PostProcessingSettings.ClaheSettings._ClipValueR.val = "0.5";
           Settings.PostProcessingSettings.ClaheSettings._ClipValueG.val = "0.5";
           Settings.PostProcessingSettings.ClaheSettings._ClipValueB.val = "0.5";

       }

   }

   public class FFAColorSettings
   {
       public Settings Settings;
       public FFAColorSettings()
       {
           Settings = new Settings();

           Settings.PostProcessingSettings.UnsharpMaskSettings._MedFilter.val = "3";

           Settings.PostProcessingSettings.MaskSettings.LiveMaskHeight.val = "2020";//added to set the default height value 
           Settings.PostProcessingSettings.MaskSettings.LiveMaskWidth.val = "2020";//added to set the default width value 
           Settings.PostProcessingSettings.MaskSettings.CaptureMaskWidth.val = "2000";//added to set the default width value 
           Settings.PostProcessingSettings.MaskSettings.CaptureMaskHeight.val = "2000";//added to set the default height value 

           Settings.PostProcessingSettings.ImageShiftSettings.LivePP.val = "true";
           Settings.PostProcessingSettings.ImageShiftSettings.HsvBoost.val = "3";
           Settings.PostProcessingSettings.ClaheSettings._ClipValueR.val = "0.5";
           Settings.PostProcessingSettings.ClaheSettings._ClipValueG.val = "0.5";
           Settings.PostProcessingSettings.ClaheSettings._ClipValueB.val = "0.5";

           Settings.ImageStorageSettings._IsIrSave.val = "true";

           Settings.CameraSettings._EnableRawMode.val = "true";
           Settings.CameraSettings._DigitalGain.val = "0";
           Settings.CameraSettings._LiveDigitalGain.val = "20";
           Settings.CameraSettings._EnableHorizontalFlip.val = "true";


           #region Firmware Settings for 45
           Settings.FirmwareSettings._EnableSingleFrameCapture.val = "true";
           Settings.FirmwareSettings._FlashBoostValue.val = "50";// for 45 it is 0 
           Settings.FirmwareSettings._isFlashBoost.val = "false";// for 45 it is false
           Settings.FirmwareSettings._FlashOffsetStart.val = "20";// for 45  it is 65
           Settings.FirmwareSettings._FlashOffsetEnd.val = "17";// for 45 it is 60

           Settings.FirmwareSettings._MotorCaptureSteps.val = "22";
           Settings.FirmwareSettings.BlueFilterPos.val = "1400";
           Settings.FirmwareSettings.GreenFilterPos.val = "1400";
           #endregion

           Settings.UISettings.LiveImaging.StartFFATimerButtonVisible.val = "true";
           Settings.UISettings.LiveImaging._PosteriorVisible.val = "false";
           Settings.UISettings.LiveImaging.FortyFiveButtonVisible.val = "false";
           Settings.UISettings.LiveImaging._AnteriorVisible.val = "false";
           Settings.UISettings.LiveImaging.FfaColorVisible.val = "true";
           Settings.UISettings.LiveImaging.FfaVisible.val = "true";
           Settings.UISettings.LiveImaging._IRBtnVisible.val = "true";
           Settings.UISettings.LiveImaging._FlashBtnVisible.val = "true";
           Settings.PostProcessingSettings.OverlaySettings.ShowOpticDiscOverlay.val = "false";
       }
   }

   public class FFASettings
   {
       public Settings Settings;
       public FFASettings()
       {
           Settings = new Settings();
           Settings.PostProcessingSettings.ColorCorrectionSettings._BBCompensation.val = "1.5";
           Settings.PostProcessingSettings.ColorCorrectionSettings._GGCompensation.val = "1.5";
           Settings.PostProcessingSettings.ColorCorrectionSettings._RRCompensation.val = "1.5";
       
           
           Settings.PostProcessingSettings.ColorCorrectionSettings._RGCompensation.val = "0.0";
           Settings.PostProcessingSettings.ColorCorrectionSettings._RBCompensation.val = "0.0";
           Settings.PostProcessingSettings.ColorCorrectionSettings._RBCompensation.val = "0.0";
           Settings.PostProcessingSettings.ColorCorrectionSettings._GBCompensation.val = "0.0";
           Settings.PostProcessingSettings.ColorCorrectionSettings._GRCompensation.val = "0.0";
           Settings.PostProcessingSettings.ColorCorrectionSettings._BGCompensation.val = "0.0";
           Settings.PostProcessingSettings.ColorCorrectionSettings._BRCompensation.val = "0.0";

           Settings.PostProcessingSettings.UnsharpMaskSettings._MedFilter.val = "3";
           Settings.PostProcessingSettings.UnsharpMaskSettings._UnSharpAmount.val = "1.0";


           Settings.PostProcessingSettings.LutSettings._IsApplyLutSettings.val = "false";


           Settings.PostProcessingSettings.MaskSettings.LiveMaskHeight.val = "2020";//added to set the default height value 
           Settings.PostProcessingSettings.MaskSettings.CaptureMaskHeight.val = "2000";//added to set the default height value 
           Settings.PostProcessingSettings.MaskSettings.LiveMaskWidth.val = "2020";//added to set the default width value 
           Settings.PostProcessingSettings.MaskSettings.CaptureMaskWidth.val = "2000";//added to set the default width value 
           Settings.PostProcessingSettings.MaskSettings._ApplyLiveMask.val = false.ToString();

           Settings.PostProcessingSettings.ImageShiftSettings.LivePP.val = "true";
           Settings.PostProcessingSettings.ImageShiftSettings._IsApplyImageShift.val = "false";
           Settings.PostProcessingSettings.ImageShiftSettings.HsvBoost.val = "3";

           Settings.PostProcessingSettings.ClaheSettings._ClipValueR.val = "0.83";
           Settings.PostProcessingSettings.ClaheSettings._ClipValueG.val = "0.83";
           Settings.PostProcessingSettings.ClaheSettings._ClipValueB.val = "0.83";

           Settings.ImageStorageSettings._IsIrSave.val = "true";


           Settings.CameraSettings._EnableRawMode.val = "true";
           Settings.CameraSettings._DigitalGain.val = "20";
           Settings.CameraSettings._LiveDigitalGain.val = "20";
           Settings.CameraSettings._EnableHorizontalFlip.val = "true";

            #region Firmware Settings for 45
           Settings.FirmwareSettings._EnableSingleFrameCapture.val = "true";
           Settings.FirmwareSettings._FlashBoostValue.val = "95";// for 45 it is 0 
           Settings.FirmwareSettings._isFlashBoost.val = "false";// for 45 it is false
           Settings.FirmwareSettings._FlashOffsetStart.val = "20";// for 45  it is 65
           Settings.FirmwareSettings._FlashOffsetEnd.val = "17";// for 45 it is 60

           Settings.FirmwareSettings._MotorCaptureSteps.val = "35";//added to set the default value by kishore on 17 august 2017.


           Settings.FirmwareSettings._IsMotorCompensation.val = "true";
           Settings.FirmwareSettings.BlueFilterPos.val = "1750";
           Settings.FirmwareSettings.GreenFilterPos.val = "1400"; 
           #endregion
           Settings.UISettings.LiveImaging.StartFFATimerButtonVisible.val = "true";
           Settings.UISettings.LiveImaging._PosteriorVisible.val = "false";
           Settings.UISettings.LiveImaging.FortyFiveButtonVisible.val = "false";
           Settings.UISettings.LiveImaging._AnteriorVisible.val = "false";
           Settings.UISettings.LiveImaging.FfaColorVisible.val = "true";
           Settings.UISettings.LiveImaging.FfaVisible.val = "true";
           Settings.UISettings.LiveImaging._IRBtnVisible.val = "true";
           Settings.UISettings.LiveImaging._FlashBtnVisible.val = "true";
           Settings.PostProcessingSettings.OverlaySettings.ShowOpticDiscOverlay.val = "false";

       }
   }
   public class XmlConfigUtility
   {
       public static void Serialize(Object data, string fileName)
       {
           Type type = data.GetType();
           XmlSerializer xs = new XmlSerializer(type);
           XmlTextWriter xmlWriter = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);
           xmlWriter.Formatting = Formatting.Indented;
           xs.Serialize(xmlWriter, data);
           xmlWriter.Close();
       }

       public static Object Deserialize(Type type, string fileName)
       {
           Object data = null;
           XmlSerializer xs = null;
           XmlTextReader xmlReader = null;
           try
           {
                xs = new XmlSerializer(type);
                xmlReader = new XmlTextReader(fileName);
                data = xs.Deserialize(xmlReader);
           }
           finally
           {
               xmlReader.Close();
           }
           return data;
       }
   }
 }
