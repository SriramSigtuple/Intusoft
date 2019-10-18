using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using NLog;
using NLog.Config;
using NLog.Targets;
using INTUSOFT.Configuration;
namespace INTUSOFT.Imaging
{
   public class CameraPropertiesHelper
    {
       public CameraModel cameraModel;
       private static readonly Logger Exception_Log = LogManager.GetLogger("ExceptionLog");
       private Bitmap leftBitmap;

       public Bitmap LeftBitmap
       {
           get { return leftBitmap; }
           set {
               leftBitmap = value;
               LBheight = leftBitmap.Height;
               LBwidth = leftBitmap.Width;
           }
       }
       private Bitmap rightBitmap;

       public Bitmap RightBitmap
       {
           get { return rightBitmap; }
           set {
               rightBitmap = value;
               RBheight = rightBitmap.Height;
               RBwidth = RightBitmap.Width;
           }
       }
       public Bitmap resetBitmapRight, resetBitmapLeft, positivearrowSymbol, negativearrowSymbol;
       public String RotaryPositiveColor = "White", RotaryNegativeColor = "White";
        int RBwidth;
        int RBheight;
        int LBwidth;
        int LBheight;
       private static CameraPropertiesHelper cameraPropsHelper;
       private IntucamHelper camHelper;
       public ImageSaveHelper imageSaveHelper;// object of imagesavehelper to save captured images
       /// <summary>
       /// Number of motor steps for reset position 
       /// </summary>
       public int MotorResetSteps;
       /// <summary>
       ///  offset steps from posterior to anterior which moves the motor after reset is done.
       /// </summary>
       public int Posterior2AnteriorOffsetMotorSteps = 115;
       private Camera ivl_Camera;
       private bool returnVal;
       BitDepth cameraBitDepth;

       public CameraModuleSettings _Settings
       {
           get { return IVLCamVariables._Settings; }
           set { IVLCamVariables._Settings = value; }
       }
       public static string LogFilePath = "";
       public string ffaTimeStamp = "";
       public System.Timers.Timer FFATimer = new System.Timers.Timer();// Timer to maintain motor event timeout
      
       public bool showInExteralViewer = false;
       private TriggerStatus _trigStatus;

       public TriggerStatus TrigStatus
       {
           get { return _trigStatus = IVLCamVariables.TrigStatus; }
       }

       public static CameraPropertiesHelper GetInstance()
       {
           try
           {
               if (cameraPropsHelper == null)
               {
                   cameraPropsHelper = new CameraPropertiesHelper();
               }
           }
           catch (Exception ex)
           {
              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log);
           }
               
           return cameraPropsHelper;
       }
       private CameraPropertiesHelper()
       {
         ivl_Camera = Camera.createCamera();
         imageSaveHelper = ImageSaveHelper.GetInstance();
         _Settings = CameraModuleSettings.GetInstance();
       }
       public bool EnableCameraColorMatrix(bool enable)
       {
           bool ret = false;
           try
           {
               int value = 0;
               if (enable)
                   value = 1;
               if (IVLCamVariables.isCameraConnected == Devices.CameraConnected)
               ret=ivl_Camera.EnableCameraColorMatrix(value);
           }
           catch (Exception ex)
           {
              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log);
               ret = false;
           }
           return ret;
       }
       public bool EnableWhiteBalance(bool enable)
       {
           bool returnVal=false;
           try
           {
               int value = 0;
               if (enable)
                   value = 1;
               if(IVLCamVariables.isCameraConnected == Devices.CameraConnected)
               returnVal= ivl_Camera.EnableWhiteBalance(value);
           }
           catch (Exception ex)
           {
              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log);
              returnVal = false; 
           }
           return returnVal; 
       }
       public bool IsApplicationClosing
       {
           get { return IVLCamVariables.isApplicationClosing; }
           set { IVLCamVariables.isApplicationClosing = value; }
       }
       public BitDepth CameraBitDepth
       {
           get { return cameraBitDepth; }
           set
           {
               if (IVLCamVariables.isCameraConnected == Devices.CameraConnected)
               {
                   ivl_Camera.ChangeBitDepth(value);
                   cameraBitDepth = value;
               }
           }
       }
       public bool SetTemperatureTint(int temperature, int tint)
       {
           bool ret = false;
           try
           {
             ret = ivl_Camera.SetTemperatureTint(temperature, tint);
           }
           catch (Exception ex)
           {
              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log);
               ret = false; 
           }
           return ret;
       }

       public void SetMonoChromeMode(bool IsMonoChrome)
       {
           try
           {
               IVLCamVariables.isMonoChrome = IsMonoChrome;
           }
           catch (Exception ex)
           {
              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log);
           }
               
           //return returnVal = ivl_Camera.SetMonoMode(IsMonoChrome);
       }
       public void Read_LED_SupplyValues()
       {
           try
           {
               IVLCamVariables.BoardHelper.Read_LED_SupplyValues();
           }
           catch (Exception ex)
           {
              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log);
           }
           
       }
       public void RotateImageHorizontal(bool isRotate)
       {
           try
           {
               ivl_Camera.RotateHorizontal(isRotate);
           }
           catch (Exception ex)
           {
              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log); 
           }
          
       }
       public void RotateImageVertical(bool isRotate)
       {
           try
           {
               ivl_Camera.RotateVertical(isRotate);
           }
           catch (Exception ex)
           {
              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log);
           }
           
       }
       public void SetFlashOffSet(byte[] FlashOffsetArr)
       {
           try
           {
               IVLCamVariables.BoardHelper.FlashOffset(FlashOffsetArr);
           }
           catch (Exception ex)
           {
              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log);
           }
               
       }
       public void EnableFlashBoost(bool isEnableBoost)
       {
           try
           {
               if (!isEnableBoost)// Disabled mode of flash Boost;
                   IVLCamVariables._Settings.BoardSettings.FlashBoostValue = 0;

               IVLCamVariables.BoardHelper.SetFlashBoost(IVLCamVariables._Settings.BoardSettings.FlashBoostValue);// if the first parameter is zero it means no flash boost is applied.
           }
           catch (Exception ex)
           {
              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log);
           }
           
       }
       public string GetFirmwareVersion()
       {
           string firmware = "";
           try
           {
               //IVLCamVariables.BoardHelper.GetFirmwareVersion();
               firmware = IVLCamVariables.FirmwareVersion;
           }
           catch (Exception ex)
           {
              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log);
           }
           return firmware;
           
       }
       public void EnableFlashControlUsingKnob()
       {
           try
           {
               IVLCamVariables.isEnableFlashControlUsingKnob = true;
               IVLCamVariables.BoardHelper.Read_LED_SupplyValues();
           }
           catch (Exception ex)
           {
              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log);
           }
           

       }
       public void ResetMotorPosition()
       {
           try
           {
               IVLCamVariables.BoardHelper.ResetMotorPosition(IVLCamVariables._Settings.BoardSettings.isResetZeroD);
           }
           catch (Exception ex)
           {
              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log);                 
           }
           
       }
       public List<byte[]> EEPROM_ByteArrList
       {
           get
           {
               return IVLCamVariables.eepromByteArrList;
           }
       }
       internal bool isPositiveNegativeDiaptorSymbolsCreated = false;
       public void CreatePositiveNegativeDiaptorSymbols()
       {
           #region added block of code for drawing negative and positive symbol and overlays.
           try
           {
               if (!isPositiveNegativeDiaptorSymbolsCreated)
               {
                   //positivearrowSymbol = new Bitmap(32, 32);
                   //negativearrowSymbol = new Bitmap(32, 32);


                   float diaptorLineSize = 3f;

                   SolidBrush solidBrush = new SolidBrush(Color.FromName(RotaryNegativeColor));

                   Graphics g = Graphics.FromImage(negativearrowSymbol);
                   g.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, negativearrowSymbol.Width, negativearrowSymbol.Height));
                   g.DrawPolygon(new Pen(solidBrush, diaptorLineSize), new Point[] { new Point(negativearrowSymbol.Width, 1), new Point(negativearrowSymbol.Width, negativearrowSymbol.Height - 2), new Point(2, negativearrowSymbol.Height / 2) });
                   g.FillPolygon(solidBrush, new Point[] { new Point(negativearrowSymbol.Width, 1), new Point(negativearrowSymbol.Width, negativearrowSymbol.Height - 2), new Point(2, negativearrowSymbol.Height / 2) });
                   //negativeSymbol.Save("Negative.bmp");

                   g = Graphics.FromImage(positivearrowSymbol);
                   g.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, positivearrowSymbol.Width, positivearrowSymbol.Height));
                   g.DrawPolygon(new Pen(solidBrush, diaptorLineSize), new Point[] { new Point(0, 1), new Point(0, positivearrowSymbol.Height - 2), new Point(positivearrowSymbol.Width - 2, positivearrowSymbol.Height / 2) });
                   g.FillPolygon(solidBrush, new Point[] { new Point(0, 1), new Point(0, positivearrowSymbol.Height - 2), new Point(positivearrowSymbol.Width - 2, positivearrowSymbol.Height / 2) });
                   isPositiveNegativeDiaptorSymbolsCreated = true;
               }
           }
           catch (Exception ex)
           {
              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log);
           }
          
           #endregion
       }
       public void SetLiveCameraSettings()
       {
           try
           {
               if (SettingsFromConfig)
               {

                   IVLCamVariables._Settings.ImageNameSettings.containsEyeSide = Convert.ToBoolean(ConfigVariables.CurrentSettings.ImageNameSettings.IsEyeSidePresent.val);
                   IVLCamVariables._Settings.ImageNameSettings.containsMRN = Convert.ToBoolean(ConfigVariables.CurrentSettings.ImageNameSettings.IsMRNPresent.val);
                   IVLCamVariables._Settings.ImageNameSettings.containsFirstName = Convert.ToBoolean(ConfigVariables.CurrentSettings.ImageNameSettings.IsFirstNamePresent.val);
                   IVLCamVariables._Settings.ImageNameSettings.containsLastName = Convert.ToBoolean(ConfigVariables.CurrentSettings.ImageNameSettings.IsLastNamePresent.val);


                   IVLCamVariables._Settings.CameraSettings.CameraModel = (CameraModel)Enum.Parse(typeof(CameraModel), ConfigVariables.CurrentSettings.CameraSettings._CameraModel.val);
                   IVLCamVariables._Settings.CameraSettings.EnableLiveImageProcessing = Convert.ToBoolean(ConfigVariables.CurrentSettings.PostProcessingSettings.ImageShiftSettings.LivePP.val);
                   IVLCamVariables._Settings.PostProcessingSettings.hsvValue = Convert.ToSingle(ConfigVariables.CurrentSettings.PostProcessingSettings.ImageShiftSettings.HsvBoost.val);
                   IVLCamVariables._Settings.PostProcessingSettings.maskSettings.isApplyLogo = Convert.ToBoolean(ConfigVariables.CurrentSettings.PostProcessingSettings.MaskSettings._ApplyLogo.val);
                   IVLCamVariables._Settings.PostProcessingSettings.maskSettings.isApplyMask = Convert.ToBoolean(ConfigVariables.CurrentSettings.PostProcessingSettings.MaskSettings._IsApplyMask.val);
                   IVLCamVariables._Settings.PostProcessingSettings.maskSettings.isApplyLiveMask = Convert.ToBoolean(ConfigVariables.CurrentSettings.PostProcessingSettings.MaskSettings._ApplyLiveMask.val);
                   IVLCamVariables._Settings.PostProcessingSettings.isApplyPostProcessing = true;
                   IVLCamVariables._Settings.PostProcessingSettings.maskSettings.maskCentreX = Convert.ToInt32(ConfigVariables.CurrentSettings.CameraSettings._ImageOpticalCentreX.val);
                   IVLCamVariables._Settings.PostProcessingSettings.maskSettings.maskCentreY = Convert.ToInt32(ConfigVariables.CurrentSettings.CameraSettings._ImageOpticalCentreY.val);

                   IVLCamVariables._Settings.PostProcessingSettings.maskSettings.maskWidth = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.MaskSettings.CaptureMaskWidth.val);
                   IVLCamVariables._Settings.PostProcessingSettings.maskSettings.maskHeight = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.MaskSettings.CaptureMaskHeight.val);

                   IVLCamVariables._Settings.PostProcessingSettings.maskSettings.LiveMaskWidth = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.MaskSettings.LiveMaskWidth.val);
                   IVLCamVariables._Settings.PostProcessingSettings.maskSettings.LiveMaskHeight = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.MaskSettings.LiveMaskHeight.val);


                   IVLCamVariables._Settings.BoardSettings.EnableRightLeftSensor = Convert.ToBoolean(ConfigVariables.CurrentSettings.FirmwareSettings._EnableLeftRightSensor.val);
                   //for CCD cameras from config to the camera settings in the camera module
                   IVLCamVariables._Settings.CameraSettings.FrameDetectionValue = Convert.ToSingle(ConfigVariables.CurrentSettings.CameraSettings._FrameDetectionValue.val);
                   // Dark Frame Detection value from config to the camera settings in the camera module
                   IVLCamVariables._Settings.CameraSettings.DarkFrameDetectionVal = Convert.ToInt32(ConfigVariables.CurrentSettings.CameraSettings._DarkFrameDetectionValue.val);
                   IVLCamVariables._Settings.CameraSettings.SaveFramesCnt = Convert.ToInt32(ConfigVariables.CurrentSettings.CameraSettings._SaveFramesCount.val);
                   IVLCamVariables._Settings.PostProcessingSettings.unsharpMaskSettings.isApplyUnsharpMask = Convert.ToBoolean(ConfigVariables.CurrentSettings.PostProcessingSettings.UnsharpMaskSettings._IsApplyUnsharpSettings.val);
                   IVLCamVariables._Settings.PostProcessingSettings.claheSettings.isApplyClahe = Convert.ToBoolean(ConfigVariables.CurrentSettings.PostProcessingSettings.ClaheSettings._IsApplyClaheSettings.val);
                   IVLCamVariables._Settings.PostProcessingSettings.lutSettings.isApplyLUT = Convert.ToBoolean(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings._IsApplyLutSettings.val);
                   IVLCamVariables._Settings.PostProcessingSettings.unsharpMaskSettings.unsharpAmount = Convert.ToDouble(ConfigVariables.CurrentSettings.PostProcessingSettings.UnsharpMaskSettings._UnSharpAmount.val);
                   IVLCamVariables._Settings.PostProcessingSettings.unsharpMaskSettings.radius = Convert.ToDouble(ConfigVariables.CurrentSettings.PostProcessingSettings.UnsharpMaskSettings._UnSharpRadius.val);
                   IVLCamVariables._Settings.PostProcessingSettings.unsharpMaskSettings.unsharpThresh = Convert.ToDouble(ConfigVariables.CurrentSettings.PostProcessingSettings.UnsharpMaskSettings._Threshold.val);
                   IVLCamVariables._Settings.PostProcessingSettings.unsharpMaskSettings.medFilterValue = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.UnsharpMaskSettings._MedFilter.val);
                   IVLCamVariables._Settings.PostProcessingSettings.claheSettings.clipValB = Convert.ToSingle(ConfigVariables.CurrentSettings.PostProcessingSettings.ClaheSettings._ClipValueB.val);
                   IVLCamVariables._Settings.PostProcessingSettings.claheSettings.clipValG = Convert.ToSingle(ConfigVariables.CurrentSettings.PostProcessingSettings.ClaheSettings._ClipValueG.val);
                   IVLCamVariables._Settings.PostProcessingSettings.claheSettings.clipValR = Convert.ToSingle(ConfigVariables.CurrentSettings.PostProcessingSettings.ClaheSettings._ClipValueR.val);
                   IVLCamVariables._Settings.PostProcessingSettings.brightnessContrastSettings.brightnessValue = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.BrightnessContrastSettings._BrightnessVal.val);
                   IVLCamVariables._Settings.PostProcessingSettings.brightnessContrastSettings.contrastValue = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.BrightnessContrastSettings._ContrastVAl.val);
                   IVLCamVariables._Settings.PostProcessingSettings.brightnessContrastSettings.isApplyBrightness = Convert.ToBoolean(ConfigVariables.CurrentSettings.PostProcessingSettings.BrightnessContrastSettings._IsApplyBrightness.val);
                   IVLCamVariables._Settings.PostProcessingSettings.brightnessContrastSettings.isApplyContrast = Convert.ToBoolean(ConfigVariables.CurrentSettings.PostProcessingSettings.BrightnessContrastSettings._IsApplyContrast.val);
                   IVLCamVariables._Settings.PostProcessingSettings.ccSettings.isApplyColorCorrection = Convert.ToBoolean(ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._IsApplyColorCorrection.val);
                   IVLCamVariables._Settings.PostProcessingSettings.hotspotSettings.isEnableHS = Convert.ToBoolean(ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._IsApplyHotspotCorrection.val);
                   IVLCamVariables._Settings.PostProcessingSettings.imageShiftSettings.isApplyShiftCorrection = Convert.ToBoolean(ConfigVariables.CurrentSettings.PostProcessingSettings.ImageShiftSettings._IsApplyImageShift.val);
                   //IVLVariables.ivl_Camera. IVLCamVariables._Settings.PostProcessingSettings.isApplyVignattingCorrection = Convert.ToBoolean(ConfigVariables.CurrentSettings.PostProcessingSettings.VignattingSettings._IsApplyVignatting.val);
                   #region hot spot settings
                   IVLCamVariables._Settings.PostProcessingSettings.hotspotSettings.radSpot1 = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowRadSpot1.val);
                   IVLCamVariables._Settings.PostProcessingSettings.hotspotSettings.radSpot2 = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowradSpot2.val);
                   IVLCamVariables._Settings.PostProcessingSettings.hotspotSettings.hotSpotRad1 = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._HotspotRadius1.val);
                   IVLCamVariables._Settings.PostProcessingSettings.hotspotSettings.hotSpotRad2 = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._HotspotRadius2.val);
                   IVLCamVariables._Settings.PostProcessingSettings.hotspotSettings.HotSpotRedPeak = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotRedPeak.val);
                   IVLCamVariables._Settings.PostProcessingSettings.hotspotSettings.HotSpotBluePeak = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotBluePeak.val);
                   IVLCamVariables._Settings.PostProcessingSettings.hotspotSettings.HotSpotGreenPeak = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotGreenPeak.val);
                   IVLCamVariables._Settings.PostProcessingSettings.hotspotSettings.HotSpotRedRadius = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotRedRadius.val);
                   IVLCamVariables._Settings.PostProcessingSettings.hotspotSettings.HotSpotGreenRadius = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotGreenRadius.val);
                   IVLCamVariables._Settings.PostProcessingSettings.hotspotSettings.HotSpotBlueRadius = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._HotSpotBlueRadius.val);
                   IVLCamVariables._Settings.PostProcessingSettings.hotspotSettings.ShadowRedPeakPercentage = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowRedPercentage.val);
                   IVLCamVariables._Settings.PostProcessingSettings.hotspotSettings.ShadowBluePeakPercentage = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowBlueBPercentage.val);
                   IVLCamVariables._Settings.PostProcessingSettings.hotspotSettings.ShadowGreenPeakPercentage = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.HotSpotSettings._ShadowGreenPercentage.val);
                   #endregion
                   IVLCamVariables._Settings.CameraSettings.ImageWidth = Convert.ToInt32(ConfigVariables.CurrentSettings.CameraSettings._ImageWidth.val);
                   IVLCamVariables._Settings.CameraSettings.ImageHeight = Convert.ToInt32(ConfigVariables.CurrentSettings.CameraSettings._ImageHeight.val);
                   IVLCamVariables._Settings.CameraSettings.roiX = Convert.ToInt32(ConfigVariables.CurrentSettings.CameraSettings._ImageROIX.val);
                   IVLCamVariables._Settings.CameraSettings.roiY = Convert.ToInt32(ConfigVariables.CurrentSettings.CameraSettings._ImageROIY.val);
                   IVLCamVariables._Settings.CameraSettings.IRTint = Convert.ToInt32(ConfigVariables.CurrentSettings.CameraSettings._IRTint.val);
                   IVLCamVariables._Settings.CameraSettings.IRTemperature = Convert.ToInt32(ConfigVariables.CurrentSettings.CameraSettings._IRTemperature.val);
                   IVLCamVariables._Settings.CameraSettings.FlashTint = Convert.ToInt32(ConfigVariables.CurrentSettings.CameraSettings._FlashTint.val);
                   IVLCamVariables._Settings.CameraSettings.FlashTemperature = Convert.ToInt32(ConfigVariables.CurrentSettings.CameraSettings._FlashTemperature.val);
                   IVLCamVariables._Settings.CameraSettings.isEnableTempTint = Convert.ToBoolean(ConfigVariables.CurrentSettings.CameraSettings._EnableWB.val);
                   IVLCamVariables._Settings.ImageSaveSettings.isIR_ImageSave = Convert.ToBoolean(ConfigVariables.CurrentSettings.ImageStorageSettings._IsIrSave.val);
                   IVLCamVariables._Settings.ImageSaveSettings.isRawSave = Convert.ToBoolean(ConfigVariables.CurrentSettings.ImageStorageSettings._IsRawSave.val);
                   IVLCamVariables._Settings.ImageSaveSettings.isSaveProcessedImage = Convert.ToBoolean(ConfigVariables.CurrentSettings.ImageStorageSettings._IsProcessedImageSave.val);
                   IVLCamVariables._Settings.ImageSaveSettings.isSaveRawImage = Convert.ToBoolean(ConfigVariables.CurrentSettings.ImageStorageSettings._IsRawImageSave.val);
                   IVLCamVariables._Settings.CameraSettings.isFourteen = Convert.ToBoolean(ConfigVariables.CurrentSettings.CameraSettings._Enable14Bit.val);
                   if (IVLCamVariables._Settings.CameraSettings.isFourteen)
                       CameraBitDepth = BitDepth.Depth_14;
                   else
                       CameraBitDepth = BitDepth.Depth_8;
                   IVLCamVariables._Settings.BoardSettings.EnableRightLeftSensor = Convert.ToBoolean(ConfigVariables.CurrentSettings.FirmwareSettings._EnableLeftRightSensor.val);
                   IVLCamVariables._Settings.BoardSettings.MotorSteps = Convert.ToInt32(ConfigVariables.CurrentSettings.FirmwareSettings._MotorCaptureSteps.val);
                   IVLCamVariables._Settings.ImageSaveSettings.jpegCompression = Convert.ToInt32(ConfigVariables.CurrentSettings.ImageStorageSettings._compressionRatio.val);
                   IVLCamVariables._Settings.PostProcessingSettings.ccSettings.rrVal = Convert.ToSingle(ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._RRCompensation.val);
                   IVLCamVariables._Settings.PostProcessingSettings.ccSettings.rgVal = Convert.ToSingle(ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._RGCompensation.val);
                   IVLCamVariables._Settings.PostProcessingSettings.ccSettings.rbVal = Convert.ToSingle(ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._RBCompensation.val);
                   IVLCamVariables._Settings.PostProcessingSettings.ccSettings.grVal = Convert.ToSingle(ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._GRCompensation.val);
                   IVLCamVariables._Settings.PostProcessingSettings.ccSettings.ggVal = Convert.ToSingle(ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._GGCompensation.val);
                   IVLCamVariables._Settings.PostProcessingSettings.ccSettings.gbVal = Convert.ToSingle(ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._GBCompensation.val);
                   IVLCamVariables._Settings.PostProcessingSettings.ccSettings.brVal = Convert.ToSingle(ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._BRCompensation.val);
                   IVLCamVariables._Settings.PostProcessingSettings.ccSettings.bgVal = Convert.ToSingle(ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._BGCompensation.val);
                   IVLCamVariables._Settings.PostProcessingSettings.ccSettings.bbVal = Convert.ToSingle(ConfigVariables.CurrentSettings.PostProcessingSettings.ColorCorrectionSettings._BBCompensation.val);
                   IVLCamVariables._Settings.PostProcessingSettings.imageShiftSettings.ShiftX = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.ImageShiftSettings._ImageShiftX.val);
                   IVLCamVariables._Settings.PostProcessingSettings.imageShiftSettings.ShiftY = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.ImageShiftSettings._ImageShiftY.val);
                   #region Lut Settings from the config file to the post processing settings in the camera module
                   IVLCamVariables._Settings.PostProcessingSettings.lutSettings.LUT_SineFactor = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings._LUTSineFactor.val);
                   IVLCamVariables._Settings.PostProcessingSettings.lutSettings.LUT_interval1 = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings._LUTInterval1.val);
                   IVLCamVariables._Settings.PostProcessingSettings.lutSettings.LUT_interval2 = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings._LUTInterval2.val);
                   IVLCamVariables._Settings.PostProcessingSettings.lutSettings.LUT_Offset = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings._LUTOffset.val);

                   IVLCamVariables._Settings.PostProcessingSettings.lutSettings.LUTR.LUT_SineFactor = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_SineFactor_R.val);
                   IVLCamVariables._Settings.PostProcessingSettings.lutSettings.LUTR.LUT_interval1 = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_interval1_R.val);
                   IVLCamVariables._Settings.PostProcessingSettings.lutSettings.LUTR.LUT_interval2 = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_interval2_R.val);
                   IVLCamVariables._Settings.PostProcessingSettings.lutSettings.LUTR.LUT_Offset = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_Offset_R.val);

                   IVLCamVariables._Settings.PostProcessingSettings.lutSettings.LUTG.LUT_SineFactor = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_SineFactor_G.val);
                   IVLCamVariables._Settings.PostProcessingSettings.lutSettings.LUTG.LUT_interval1 = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_interval1_G.val);
                   IVLCamVariables._Settings.PostProcessingSettings.lutSettings.LUTG.LUT_interval2 = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_interval2_G.val);
                   IVLCamVariables._Settings.PostProcessingSettings.lutSettings.LUTG.LUT_Offset = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_Offset_G.val);

                   IVLCamVariables._Settings.PostProcessingSettings.lutSettings.LUTB.LUT_SineFactor = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_SineFactor_B.val);
                   IVLCamVariables._Settings.PostProcessingSettings.lutSettings.LUTB.LUT_interval1 = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_interval1_B.val);
                   IVLCamVariables._Settings.PostProcessingSettings.lutSettings.LUTB.LUT_interval2 = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_interval2_B.val);
                   IVLCamVariables._Settings.PostProcessingSettings.lutSettings.LUTB.LUT_Offset = Convert.ToInt32(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings.LUT_Offset_B.val);

                   IVLCamVariables._Settings.PostProcessingSettings.lutSettings.isChannelWiseLUT = Convert.ToBoolean(ConfigVariables.CurrentSettings.PostProcessingSettings.LutSettings._IsChannelWiseLUT.val);
                   IVLCamVariables._Settings.PostProcessingSettings.gammaCorrectionSettings.ApplyGammaCorrection = Convert.ToBoolean(ConfigVariables.CurrentSettings.PostProcessingSettings.GammaSettings.IsApplyGammaSettings.val);
                   IVLCamVariables._Settings.PostProcessingSettings.gammaCorrectionSettings.GammaCorrectionValue = Convert.ToSingle(ConfigVariables.CurrentSettings.PostProcessingSettings.GammaSettings.GammaValue.val);
                   #endregion

                   IVLCamVariables._Settings.PostProcessingSettings.overlaySettings.ShowPosteriorOverlay = Convert.ToBoolean(ConfigVariables.CurrentSettings.PostProcessingSettings.OverlaySettings.ShowPosteriorOverlay.val);
                   IVLCamVariables._Settings.PostProcessingSettings.overlaySettings.ShowAnteriorOverlay = Convert.ToBoolean(ConfigVariables.CurrentSettings.PostProcessingSettings.OverlaySettings.ShowAnteriorOverlay.val);
                   IVLCamVariables._Settings.PostProcessingSettings.overlaySettings.ShowOpticDiscOverlay = Convert.ToBoolean(ConfigVariables.CurrentSettings.PostProcessingSettings.OverlaySettings.ShowOpticDiscOverlay.val);

                   IVLCamVariables._Settings.BoardSettings.MotorPerStepTime = Convert.ToInt32(ConfigVariables.CurrentSettings.FirmwareSettings.MotorPerStepTime.val);
                   IVLCamVariables._Settings.BoardSettings.BlueFilterPos = Convert.ToInt32(ConfigVariables.CurrentSettings.FirmwareSettings.BlueFilterPos.val);
                   IVLCamVariables._Settings.BoardSettings.GreenFilterPos = Convert.ToInt32(ConfigVariables.CurrentSettings.FirmwareSettings.GreenFilterPos.val);
                   IVLCamVariables._Settings.BoardSettings.FFA_Color_Pot_Int_Offset = Convert.ToInt32(ConfigVariables.CurrentSettings.FirmwareSettings.PotOffsetValue.val);
                   IVLCamVariables._Settings.BoardSettings.FFA_Pot_Int_Offset = Convert.ToInt32(ConfigVariables.CurrentSettings.FirmwareSettings.PotOffsetValue.val);
                   IVLCamVariables._Settings.BoardSettings.EnablePCU_IntensityControl = Convert.ToBoolean(ConfigVariables.CurrentSettings.FirmwareSettings._EnablePCUKnob.val);
                   EnableWhiteBalance(Convert.ToBoolean(ConfigVariables.CurrentSettings.CameraSettings._EnableWB.val));
                   //IVLVariables.ivl_Camera.EnableCameraColorMatrix(formCheckBox2.Checked);
                   // Set value of flash boost from config to the settings of camera
                   IVLCamVariables._Settings.BoardSettings.FlashBoostValue = (Convert.ToByte(ConfigVariables.CurrentSettings.FirmwareSettings._FlashBoostValue.val));
                   // Enable flash boost depending on the state of the isFlash boost from config this uses the flash boost value from board settings of the camera

                   if (IVLCamVariables._Settings.BoardSettings.FlashBoostValue > 0)
                       EnableFlashBoost(Convert.ToBoolean(ConfigVariables.CurrentSettings.FirmwareSettings._isFlashBoost.val));
                   if (Convert.ToBoolean(ConfigVariables.CurrentSettings.FirmwareSettings._EnablePCUKnob.val))
                       EnableFlashControlUsingKnob();

                   IVLCamVariables._Settings.MotorOffSetSettings.iR2IR = Convert.ToInt32(ConfigVariables.CurrentSettings.MotorOffSetSettings.IR2IR.val);
                   IVLCamVariables._Settings.MotorOffSetSettings.iR2Flash = Convert.ToInt32(ConfigVariables.CurrentSettings.MotorOffSetSettings.IR2Flash.val);
                   IVLCamVariables._Settings.MotorOffSetSettings.iR2Blue = Convert.ToInt32(ConfigVariables.CurrentSettings.MotorOffSetSettings.IR2Blue.val);
                   IVLCamVariables._Settings.MotorOffSetSettings.flash2IR = Convert.ToInt32(ConfigVariables.CurrentSettings.MotorOffSetSettings.Flash2IR.val);
                   IVLCamVariables._Settings.MotorOffSetSettings.flash2Flash = Convert.ToInt32(ConfigVariables.CurrentSettings.MotorOffSetSettings.Flash2Flash.val);
                   IVLCamVariables._Settings.MotorOffSetSettings.flash2Blue = Convert.ToInt32(ConfigVariables.CurrentSettings.MotorOffSetSettings.Flash2Blue.val);
                   IVLCamVariables._Settings.MotorOffSetSettings.blue2IR = Convert.ToInt32(ConfigVariables.CurrentSettings.MotorOffSetSettings.Blue2IR.val);
                   IVLCamVariables._Settings.MotorOffSetSettings.blue2Flash = Convert.ToInt32(ConfigVariables.CurrentSettings.MotorOffSetSettings.Blue2Flash.val);
                   IVLCamVariables._Settings.MotorOffSetSettings.blue2Blue = Convert.ToInt32(ConfigVariables.CurrentSettings.MotorOffSetSettings.Blue2Blue.val);


                   IVLCamVariables.motorOffSetMatrix[0, 0] = IVLCamVariables._Settings.MotorOffSetSettings.iR2IR;
                   IVLCamVariables.motorOffSetMatrix[0, 1] = IVLCamVariables._Settings.MotorOffSetSettings.iR2Flash;
                   IVLCamVariables.motorOffSetMatrix[0, 2] = IVLCamVariables._Settings.MotorOffSetSettings.iR2Blue;
                   IVLCamVariables.motorOffSetMatrix[1, 0] = IVLCamVariables._Settings.MotorOffSetSettings.flash2IR;
                   IVLCamVariables.motorOffSetMatrix[1, 1] = IVLCamVariables._Settings.MotorOffSetSettings.flash2Flash;
                   IVLCamVariables.motorOffSetMatrix[1, 2] = IVLCamVariables._Settings.MotorOffSetSettings.flash2Blue;
                   IVLCamVariables.motorOffSetMatrix[2, 0] = IVLCamVariables._Settings.MotorOffSetSettings.blue2IR;
                   IVLCamVariables.motorOffSetMatrix[2, 1] = IVLCamVariables._Settings.MotorOffSetSettings.blue2Flash;
                   IVLCamVariables.motorOffSetMatrix[2, 2] = IVLCamVariables._Settings.MotorOffSetSettings.blue2Blue;

                   IVLCamVariables._Settings.CameraSettings.LiveGainIndex = Convert.ToInt32(ConfigVariables.CurrentSettings.CameraSettings._LiveDigitalGain.val);
                   IVLCamVariables._Settings.CameraSettings.CaptureGainIndex = Convert.ToInt32(ConfigVariables.CurrentSettings.CameraSettings._DigitalGain.val);
                  RotateImageHorizontal(Convert.ToBoolean(ConfigVariables.CurrentSettings.CameraSettings._EnableHorizontalFlip.val));
                  RotateImageVertical(Convert.ToBoolean(ConfigVariables.CurrentSettings.CameraSettings._EnableVerticalFlip.val));
                   #region live and capture gain values from config settings to camera settings by sriram on 16 august 2017
                   IVLCamVariables._Settings.CameraSettings.CaptureGainHigh = Convert.ToUInt16(ConfigVariables.CurrentSettings.CameraSettings._DigitalGainHigh.val);
                   IVLCamVariables._Settings.CameraSettings.CaptureGainMed = Convert.ToUInt16(ConfigVariables.CurrentSettings.CameraSettings._DigitalGainMedium.val);
                   IVLCamVariables._Settings.CameraSettings.CaptureGainLow = Convert.ToUInt16(ConfigVariables.CurrentSettings.CameraSettings._DigitalGainLow.val);

                   IVLCamVariables._Settings.CameraSettings.LiveGainHigh = Convert.ToUInt16(ConfigVariables.CurrentSettings.CameraSettings._LiveGainHigh.val);
                   IVLCamVariables._Settings.CameraSettings.LiveGainMed = Convert.ToUInt16(ConfigVariables.CurrentSettings.CameraSettings._LiveGainMedium.val);
                   IVLCamVariables._Settings.CameraSettings.LiveGainLow = Convert.ToUInt16(ConfigVariables.CurrentSettings.CameraSettings._LiveGainLow.val);
                    IVLCamVariables._Settings.CameraSettings.CaptureExposure = Convert.ToUInt16(ConfigVariables.CurrentSettings.CameraSettings._Exposure.val);
                    #endregion


                    #region Diaptor Max Values

                    IVLCamVariables.MaxNegativeDiaptor = Convert.ToInt32(ConfigVariables.CurrentSettings.FirmwareSettings.NegativeDiaptorMaxValue.val);
                    IVLCamVariables.MaxPositiveDiaptor = Convert.ToInt32(ConfigVariables.CurrentSettings.FirmwareSettings.PositiveDiaptorMaxValue.val);
                    #endregion


                    byte strobeValue = 156;
                   if (IVLCamVariables.ImagingMode == Imaging.ImagingMode.Anterior_Prime || IVLCamVariables.ImagingMode == Imaging.ImagingMode.Posterior_Prime)
                       strobeValue = 100;
                   IVLCamVariables.BoardHelper.SetStrobeWidth(strobeValue);
                    // This is the order of the post processing steps the steps are applied based enable/disable in the config
                    PostProcessingStep[] postProcessingSteps = new PostProcessingStep[] {PostProcessingStep.ShiftImage,PostProcessingStep.HotSpotCorrection,PostProcessingStep.Clahe,
                    PostProcessingStep.UnsharpMask, PostProcessingStep.LUT,PostProcessingStep.ColorCorrection,PostProcessingStep.Gamma, PostProcessingStep.BrightnessContrast, PostProcessingStep.HSVBoost, PostProcessingStep.Mask};
                    PostProcessing.PP_OrderList = postProcessingSteps.ToList();
                    CreatePositiveNegativeDiaptorSymbols();
               }
               else
               {
                   // get settings from eeprom to be implemented later
               }
                IVLCamVariables.PostProcessing.CalculateLUT(IVLCamVariables._Settings.PostProcessingSettings.lutSettings,4);

           }
           catch (Exception ex)
           {
               CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log);
           }

       }
       public void SaveImage2Dir(Bitmap srcBm, string folderPath, ImageSaveFormat format, int compressionRatio, bool isSaveAsOrExport = false, string[] patDetails = null)
       {
           try
           {
               Bitmap tempBm = new Bitmap(srcBm);
               if(patDetails != null)
                PostProcessing.ApplyPatientDetails(ref tempBm, patDetails);//to write the patient details on the exported image.
               imageSaveHelper.SaveProcessedImage(tempBm, folderPath, format, compressionRatio, isSaveAsOrExport);
           }
           catch (Exception ex)
           {
              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log); 
           }
           
       }
       public void SaveLiveFrame()
       {
           try
           {
               //SaveImage2Dir(IVLCamVariables.RawImage, IVLCamVariables._Settings.ImageSaveSettings.ProcessedImageDirPath + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyyMMddhhmmss"), ImageSaveFormat.png, 100);
           }
           catch (Exception ex)
           {

              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log);
           }
           

       }
       private bool isRawMode;

       public bool IsRawMode
       {
           get { return isRawMode; }
           set { isRawMode = value; }
       }
       private ImagingMode _imagingMode;

       public ImagingMode ImagingMode
       {
           get
           {
               return _imagingMode;
           }
           set
           {
               _imagingMode = value;
               IVLCamVariables.ImagingMode = value;
               if (IVLCamVariables.ImagingMode == ImagingMode.FFA_Plus)
                   IVLCamVariables.isColor = false;
               else
                   IVLCamVariables.isColor = true;
               IVLCamVariables.BoardHelper.imagingMode = (byte)(value);
               switch (value)
               {
                   case Imaging.ImagingMode.Posterior_45:
                       {
                           IVLCamVariables.captureLedCode = LedCode.Flash;
                           break;
                       }
                   case Imaging.ImagingMode.Anterior_Prime:
                       {
                           IVLCamVariables.captureLedCode = LedCode.Flash;
                           break;
                       }
                   case Imaging.ImagingMode.Posterior_Prime:
                       {
                           IVLCamVariables.captureLedCode = LedCode.Flash;
                           break;
                       }
                   case Imaging.ImagingMode.FFA_Plus:
                       {
                           IVLCamVariables.captureLedCode = LedCode.Blue;
                           IVLCamVariables.BoardHelper.imagingMode = (byte)(1);

                           break;
                       }
                   case Imaging.ImagingMode.FFAColor:
                       {
                           IVLCamVariables.captureLedCode = LedCode.Flash;
                           IVLCamVariables.BoardHelper.imagingMode = (byte)(0);

                           break;
                       }
                   case Imaging.ImagingMode.Anterior_FFA://cornea blue
                       {
                           IVLCamVariables.captureLedCode = LedCode.Blue;
                           break;
                       }
               }
              

           }
       }

       private bool isBoardOpen = false;

       public bool IsBoardOpen
       {
           get
           {
               isBoardOpen = IVLCamVariables.BoardHelper.IsOpen;
               return isBoardOpen;
           }
       }
     
       private LeftRightPosition _leftRightPos;

       public LeftRightPosition LeftRightPos
       {
           get
           {

               return _leftRightPos = IVLCamVariables.leftRightPos;
           }
           set
           {
               _leftRightPos = value;
               IVLCamVariables.leftRightPos = value;
           }
       }
       public string ImageName
       {
           get { return IVLCamVariables.ImageName; }
           set { IVLCamVariables.ImageName = value; }
       }

       private bool isSingleFrameCapture = false;
       /// <summary>
       /// if true 1 is sent to the board else 0 is sent in order to set full frame capture mode.
       /// </summary>
       public bool IsSingleFrameCapture
       {
           set
           {

               isSingleFrameCapture = value;
               IVLCamVariables._Settings.BoardSettings.IsSingleFrameCapture = value;
               if (isSingleFrameCapture)
                   IVLCamVariables.BoardHelper.SetFullFrameCapture(1);
               else
                   IVLCamVariables.BoardHelper.SetFullFrameCapture(0);
           }
       }

       public PostProcessing PostProcessing
       {
           get { return IVLCamVariables.PostProcessing; }
           set { IVLCamVariables.PostProcessing = value; }
       }

      
       public int[] GetExposureGainFromTable(int AnalogVal)
       {
           try
           {
               return ivl_Camera.GetExposureGainFromTable(AnalogVal);
           }
           catch (Exception ex)
           {
              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log);
               return new int[2];
           }
       }
       public bool SetRGBGain(int[] RGB)
       {
           bool returnVal = false;
           try
           {
               if (ivl_Camera.isOpenCamera)
                   returnVal = ivl_Camera.SetRGBGain(RGB);
           }
           catch (Exception ex)
           {

              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log);
               returnVal = false;
           }
           
           return returnVal;
       }
       public bool GetRGBGain(int[] RGB)
       {
           bool returnVal = false;
           try
           {
               if (ivl_Camera.isOpenCamera)
                   returnVal = ivl_Camera.GetRGBGain(RGB);
           }
           catch (Exception ex)
           {
              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log);
               returnVal = false;
           }
           
           return returnVal;

       }

       /// <summary>
       /// The method used to write all the events occured during the life cycle of camera and board in the open state
       /// </summary>

       private Devices isPowerConnected;

       public Devices IsPowerConnected
       {
           get
           {
                    isPowerConnected =IVLCamVariables.isPowerConnected;
               return isPowerConnected;
           }
       }
       private string rawImageSavedPath;

       public string RawImageSavedPath
       {
           get { return rawImageSavedPath = IVLCamVariables.RawImageSaveDirPath; }
       }
       private string imageSavedPath;

       public string ImageSavedPath
       {
           get
           {
               imageSavedPath = IVLCamVariables.ImagePath;
               return imageSavedPath;
           }
       }
       private Devices isCameraConnected;

       public Devices IsCameraConnected
       {
           get
           {
               isCameraConnected =  IVLCamVariables.isCameraConnected;

               //  if(IVLCamVariables.BoardHelper.isOpen && ivl_Camera.isOpenCamera && !IVLCamVariables.isCapturing)
               //IVLCamVariables.BoardHelper.CameraConnected(_Settings.BoardSettings.BoardIterCnt);

               return isCameraConnected;
           }
       }
       #region Methods for Exposure and Gain

     
       public int GetIndexExpGainValFromLUT(int gain, int exp)
       {
           int index = 0;
           try
           {
               if (ivl_Camera.isOpenCamera)
                   index = ivl_Camera.GetIndexExpGainValFromLUT(gain, exp);
           }
           catch (Exception ex)
           {
              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log);
           }
           
           return index;

       }
       public bool GetGainRange(ref ushort min, ref ushort max, ref  ushort def)
       {
           bool returnVal = false;
           try
           {
               if (ivl_Camera.isOpenCamera)
                   returnVal = ivl_Camera.GetGainRange(ref min, ref max, ref  def);
           }
           catch (Exception ex)
           {
              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log);
               returnVal = false;
           }
           return returnVal;

       }
       public bool GetExposureRange(ref uint min, ref uint max, ref  uint def)
       {
           bool returnVal = false;
           try
           {
               if (ivl_Camera.isOpenCamera)
                   returnVal = ivl_Camera.GetExposureRange(ref min, ref max, ref  def);
           }
           catch (Exception ex)
           {
              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log);
              returnVal= false;
           }
           
           return returnVal;
       }
       #endregion

       public bool GetResolution(ref Dictionary<int, int> resolutions)
       {
           bool returnVal = false;
           try
           {
               returnVal = ivl_Camera.GetResolution(ref resolutions);
           }
           catch (Exception ex)
           {

              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log);
               returnVal=false;
           }
           return returnVal;
       }
       
       public void ServoMove(int filterPos)
       {
           try
           {
               IVLCamVariables.BoardHelper.GreenFilterMove(filterPos);

           }
           catch (Exception ex)
           {
              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log);
           }
       }
       public void IRLightOnOff(bool isOn)
       {
           try
           {
               if (IVLCamVariables.BoardHelper.IsOpen)
               {
                   if (isOn)
                       IVLCamVariables.BoardHelper.IRLightOn();
                   else
                       IVLCamVariables.BoardHelper.IRLightOff();
               }
           }
           catch (Exception ex)
           {
              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log);
           }
           
       }
       public void EnableFFATimer(bool isStart)
       {
           try
           {
               if (isStart)
               {
                   IVLCamVariables.FFASeconds = 0;
                   FFATimer.Start();
               }
               else
               {
                   FFATimer.Stop();
                   IVLCamVariables.FFATime = string.Empty;
               }
           }
           catch (Exception ex)
           {

               CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log);
           }
       }
       public void WhiteLightOnOff(bool isOn)
       {
           try
           {
               if (IVLCamVariables.BoardHelper.IsOpen)
               {
                   if (isOn)
                       IVLCamVariables.BoardHelper.whiteLightOn();
                   else
                       IVLCamVariables.BoardHelper.whiteLightOff();
               }
           }
           catch (Exception ex)
           {
              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log);
           }
           
       }
       public void BlueLightOnOff(bool isOn)
       {
           try
           {
               if (IVLCamVariables.BoardHelper.IsOpen)
               {
                   if (isOn)
                   {
                       IVLCamVariables.BoardHelper.BlueLightOn();

                   }
                   else
                       IVLCamVariables.BoardHelper.BlueLightOff();
               }
           }
           catch (Exception ex)
           {

              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log);
           }
           
       }
       private GainLevels liveGainLevel;

       public GainLevels LiveGainLevel
       {
           get { return liveGainLevel; }
           set {
               liveGainLevel = value;
               SetGainValueFromLevel(value, true);
           
           }
       }

       private GainLevels captureGainLevel;

       public GainLevels CaptureGainLevel
       {
           get { return captureGainLevel; }

           set {
               captureGainLevel = value;
               SetGainValueFromLevel(value, false);
           }
       }
       internal void SetGainValueFromLevel(GainLevels level, bool isLive)// made the method public to be accessed by the intucamhelper
       {
           try
           {
               switch (level)
               {
                   case GainLevels.Low:
                       {

                           if (isLive)
                           {
                               IVLCamVariables._Settings.CameraSettings.LiveGain = IVLCamVariables._Settings.CameraSettings.LiveGainLow;

                              
                           }
                           else
                               IVLCamVariables._Settings.CameraSettings.CaptureGain = IVLCamVariables._Settings.CameraSettings.CaptureGainLow;
                           break;

                       }
                   case GainLevels.Medium:
                       {
                           if (isLive)
                               IVLCamVariables._Settings.CameraSettings.LiveGain = IVLCamVariables._Settings.CameraSettings.LiveGainMed;
                           else
                               IVLCamVariables._Settings.CameraSettings.CaptureGain = IVLCamVariables._Settings.CameraSettings.CaptureGainMed;
                           break;
                       }
                   case GainLevels.High:
                       {
                           if (isLive)
                               IVLCamVariables._Settings.CameraSettings.LiveGain = IVLCamVariables._Settings.CameraSettings.LiveGainHigh;
                           else
                               IVLCamVariables._Settings.CameraSettings.CaptureGain = IVLCamVariables._Settings.CameraSettings.CaptureGainHigh;
                           break;
                       }
                       
               }
               //if (isLive)
               //{
               //    if (ivl_Camera.isOpenCamera)
               //    {
               //        ivl_Camera.SetGain(IVLCamVariables._Settings.CameraSettings.LiveGain);
                      
                      
               //    }
               //}
           }
           catch (Exception ex)
           {
              CameraLogger.WriteException(ex, Exception_Log);
               // CameraLogger.WriteException(ex, Exception_Log); 
           }
           
          
       }
       private bool settingsFromConfig = true;

       public bool SettingsFromConfig
       {
           get { return settingsFromConfig; }
           set { settingsFromConfig = value; }
       }

    }
}
