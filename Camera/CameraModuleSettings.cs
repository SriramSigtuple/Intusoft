using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace INTUSOFT.Imaging
{
    public class CameraModuleSettings
    {
        private static CameraModuleSettings _cameraModuleSettings;
        public CameraSettings CameraSettings;
        public BoardSettings BoardSettings;
        public PostProcessingSettings PostProcessingSettings;
        public ImageSaveSettings ImageSaveSettings;
        public ImageNameSettings ImageNameSettings;
        public MotorOffSetSettings MotorOffSetSettings;
        private static readonly Logger Exception_Log = LogManager.GetLogger("ExceptionLog");
        public CameraModuleSettings()
        {
            CameraSettings = new CameraSettings();
            BoardSettings = new BoardSettings();
            PostProcessingSettings = new PostProcessingSettings();
            ImageSaveSettings = new ImageSaveSettings();
            ImageNameSettings = new ImageNameSettings();
            MotorOffSetSettings = new MotorOffSetSettings();
        }
        public static CameraModuleSettings GetInstance()
        {
            try
            {
                if (_cameraModuleSettings == null)
                {
                    _cameraModuleSettings = new CameraModuleSettings();
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log); 
            } 
            return _cameraModuleSettings;
        }
    }
    public class CameraSettings
    {
        #region Variables

        public bool isVFlipForRaw = false;// to maintain vertical flip in raw mode form live to capture mode
        public bool isHFlipForRaw = false;// to maintain horizontal flip in raw mode form live to capture mode
        public bool isFourteen = false;// to maintain 14 fourteen mode if the mode implementation from the camera.
        public bool isEnableTempTint = false;// to enable temp tint from the camera.
        public float FrameDetectionValue = 2.5f;
        public int DarkFrameDetectionVal = 100;
        public int ImageWidth = 2048;// Image width
        public int ImageHeight = 1536;// Image Height
        public int SaveFramesCnt = 8;
        public bool isContinuousCapture = false;
        public bool isRawMode = true;
        public CameraModel CameraModel = CameraModel.B;
        public bool isFFA_mode = false;//To indicate FFA mode
        public ushort LiveGain = 100;
        public ushort CaptureGain = 200;
        public uint LiveExposure = 100000;
        public uint CaptureExposure = 100000;
        public int IRTemperature = 6500;
        public int IRTint = 1200;
        public int FlashTemperature = 6500;
        public int FlashTint = 1200;
        public int roiX = 270;
        public int roiY = 0;
        public bool EnableLiveImageProcessing = false;
        public double IRCheckValue = 1.5;

        public int LiveGainIndex = 0;
        public int CaptureGainIndex = 0;

        public ushort LiveGainLow = 100;
        public ushort LiveGainMed = 200;
        public ushort LiveGainHigh = 300;

        public ushort CaptureGainLow = 100;
        public ushort CaptureGainMed = 200;
        public ushort CaptureGainHigh = 300;
        #endregion

        public CameraSettings()
        {

        }
    }

    public class BoardSettings
    {
        #region Variables
        public int MotorSteps = 25;// 25 for 45 mode 10 for posterior in prime 35 for anterior prime 0 for FFA Plus
        public int Posterior2AnteriorOffsetMotorSteps = 115;// Motor Steps to change from posterior to anterior
        public int MotorPerStepTime = 10;// Time taken in milliseconds for each steps.
        public int MotorStepOffsetTime = 25;// Offset time to maintain the tolerance levels in milliseconds.
        public int BoardIterCnt = 3;// Repetition count of the board command if failed.
        public byte FlashBoostValue = 0;// Flash boost value 

        public bool EnableRightLeftSensor = false;// To enable Right Left sensor status checking
        public bool EnablePCU_IntensityControl = false;// to enable Power Control Unit Intensity Control
        public bool EnableRotaryPositionDisplay = false;// To enable Enable check of rotary position using motor sensor
        public bool isReverseMotorPosition = false;// Reverse Motor Position during capture
        public bool MotorPolarityIsForward = false;// To say whether should move forward/backward
        public bool isResetZeroD = false;// Should the reset motor go to zero D or Sensor reset position if true takes to zero diaptor
        public bool IsSingleFrameCapture = false; // Enable Single frame capture mode
        public int FFA_Pot_Int_Offset = 10;
        public int FFA_Color_Pot_Int_Offset = 10;
        public int GreenFilterPos = 1500;
        public int BlueFilterPos = 1500;
        public byte CaptureStartOffset = 30;
        public byte CaptureEndOffset = 10;
        public int MotorSensorStepsMax = 630;
        public bool isMotorSensorPresent = true;
        public long InterruptTimeInterval = 11000;
        public long BulkTransferTimeInterval = 20000;
        public int flashOnDoneStrobeCycleValue = 3;
        public int flashOffDoneStrobeCycleValue = 3;

        #endregion
        public BoardSettings()
        {

        }
    }
    public class MotorOffSetSettings
    {
        public int iR2IR = 0;
        public int iR2Flash = 25;
        public int iR2Blue = 35;
        public int flash2IR = -100;
        public int flash2Flash = 0;
        public int flash2Blue = 10;
        public int blue2IR = -100;
        public int blue2Flash = -10;
        public int blue2Blue = 0;
        public MotorOffSetSettings()
        {

        }
    }
    public class PostProcessingSettings
    {
        #region Variables
        public bool isApplyPostProcessing = false;// enable post processing for captured image
        public HotSpotSettings hotspotSettings;
        public LutSettings lutSettings;
        public ImageShiftSettings imageShiftSettings;
        public CCSettings ccSettings;
        public MaskSettings maskSettings;
        public UnsharpMaskSettings unsharpMaskSettings;
        public ClaheSettings claheSettings;
        public BrightnessContrastSettings brightnessContrastSettings;
        public GammaCorrectionSettings gammaCorrectionSettings;
        public EdgeDetectionSettings edgeDetectionSettings;
        public OverlaySettings overlaySettings;
        public bool isApplyHSVBoost = false;
        public float hsvValue = 1.0f;
        #endregion
        public PostProcessingSettings()
        {

            hotspotSettings = new HotSpotSettings();
            lutSettings = new LutSettings();
            imageShiftSettings = new ImageShiftSettings();
            ccSettings = new CCSettings();
            maskSettings = new MaskSettings();
            unsharpMaskSettings = new UnsharpMaskSettings();
            claheSettings = new ClaheSettings();
            brightnessContrastSettings = new BrightnessContrastSettings();
            gammaCorrectionSettings = new GammaCorrectionSettings();
            edgeDetectionSettings = new EdgeDetectionSettings();
            overlaySettings = new OverlaySettings();
        }
    }
    public class BrightnessContrastSettings
    {
        public bool isApplyBrightness = false;
        public bool isApplyContrast = false;
        public int brightnessValue = 0;
        public int contrastValue = 0;
        public BrightnessContrastSettings()
        {

        }
    }
    public class EdgeDetectionSettings
    {
        public bool enableEdgeDetection = false;
        public double thresholdValue = 50;
        public double thresholdLinkVal = 120;
        public EdgeDetectionSettings()
        {

        }
    }
    public class MaskSettings
    {
        public bool isApplyMask = false;
        public bool isApplyLiveMask = false;
        public bool isApplyLogo = false;
        public int maskCentreX = 900;
        public int maskCentreY = 700;
        public int maskWidth = 1700;
        public int maskHeight = 1700;
        public int LiveMaskWidth = 1700;
        public int LiveMaskHeight = 1700;

        public MaskSettings()
        {

        }

    }
    public class LutSettings
    {
        public bool isApplyLUT = false;
        public int LUT_interval1;
        public int LUT_interval2;
        public int LUT_SineFactor;
        public int LUT_Offset;
        public bool isChannelWiseLUT = false;
        public LUTParams LUTR;
        public LUTParams LUTG;
        public LUTParams LUTB;
        public LutSettings()
        {
            LUTR = new LUTParams();
            LUTG = new LUTParams();
            LUTB = new LUTParams();
        }
    }

    public class LUTParams
    {
        public int LUT_interval1;
        public int LUT_interval2;
        public int LUT_SineFactor;
        public int LUT_Offset;
        public LUTParams()
        {

        }
    }

    public class ImageShiftSettings
    {
        public bool isApplyShiftCorrection = false;
        public int ShiftX = 8;
        public int ShiftY = 7;

        public ImageShiftSettings()
        {

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
    public class CCSettings
    {
        public bool isApplyColorCorrection = false;
        public bool isApplyLiveColorCorrection = false;
        public float rrVal = 1.0f;
        public float rgVal = 0.4f;
        public float rbVal = 0.0f;
        public float grVal = 0.0f;
        public float ggVal = 0.95f;
        public float gbVal = 0.0f;
        public float brVal = 0.0f;
        public float bgVal = 0.2f;
        public float bbVal = 1.5f;
        public CCSettings()
        {

        }
    }


    public class HotSpotSettings
    {
        public bool isEnableHS = false;

        public int radSpot1 = 170;
        public int radSpot2 = 400;
        public int hotSpotRad1 = -50;
        public int hotSpotRad2 = 100;
        public int HotSpotRedPeak = 2;
        public int HotSpotGreenPeak = 15;
        public int HotSpotBluePeak = 6;
        public int HotSpotRedRadius = 100;
        public int HotSpotGreenRadius = 120;
        public int HotSpotBlueRadius = 120;
        public int ShadowRedPeakPercentage = 20;
        public int ShadowGreenPeakPercentage = 15;
        public int ShadowBluePeakPercentage = 10;
        public HotSpotSettings()
        {

        }
    }

    public class OverlaySettings
    {
        public bool ShowPosteriorOverlay = true;
        public bool ShowAnteriorOverlay = true;
        public bool ShowOpticDiscOverlay = true;

        public OverlaySettings()
        {

        }
    }
    public class ClaheSettings
    {
        public bool isApplyClahe = false;

        public float clipValR = 0.002f;
        public float clipValG = 0.002f;
        public float clipValB = 0.002f;

        public ClaheSettings()
        {

        }
    }
    public class UnsharpMaskSettings
    {
        public bool isApplyUnsharpMask = false;
        public double unsharpThresh = 40;
        public double radius = 9;
        public double unsharpAmount = 1.4;
        public int medFilterValue = 3;
        public UnsharpMaskSettings()
        {

        }
    }
    public class ImageNameSettings
    {
        public bool containsEyeSide = false;
        public bool containsFirstName = false;
        public bool containsLastName = false;
        public bool containsMRN = false;
        public bool containsGender = false;
        public bool containsAge = false;
        public ImageNameSettings()
        {

        }
    }
    public class ImageSaveSettings
    {
        #region Variables
        public int jpegCompression = 100;
        public string RawImageDirPath = "";
        public string ProcessedImageDirPath = "";


        public ImageSaveFormat _ImageSaveFormat;
        public bool isSaveProcessedImage = false;
        public bool isIR_ImageSave = false;//bool to enable IR Image saving
        public bool isRawSave = false;//bool to enable Raw bytes saving
        public bool isSaveRawImage = false;// bool to enable raw image saving
        public bool isMRNFolderSave = false;// bool to save the processed image in MRN folder
        #endregion
        public ImageSaveSettings()
        {

        }

    }

}
