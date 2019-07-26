using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using NLog;
using NLog.Targets;
namespace WindowsFormsApplication1.AdvancedSettings
{
    [Serializable]
    public class PostProcessingSettings
    {

        public ImageShiftSettings ImageShiftSettings;
        public VignattingSettings VignattingSettings;
        public MaskSettings MaskSettings;
        public ColorCorrectionSettings ColorCorrectionSettings;
        public HotSpotSettings HotSpotSettings;
        public UnsharpMaskSettings UnsharpMaskSettings;
        public ClaheSettings ClaheSettings;
        public LutSettings LutSettings;
        public static Logger Exception_Log = LogManager.GetLogger("DB_Porting.ExceptionLog");

     
        public PostProcessingSettings()
        {
            ImageShiftSettings = new ImageShiftSettings();
            VignattingSettings = new VignattingSettings();
            MaskSettings = new MaskSettings();
            ColorCorrectionSettings = new ColorCorrectionSettings();
            HotSpotSettings = new HotSpotSettings();
            UnsharpMaskSettings = new UnsharpMaskSettings();
            ClaheSettings = new ClaheSettings();
            LutSettings = new LutSettings();
        }
    }
    [Serializable]
     public class ImageShiftSettings
     {
         // Image shift parameters
         //public bool isApplyImageShift = true;
         //public int ImageShiftX = 4;
         //public int ImageShiftY = 3;
        public static Logger Exception_Log = LogManager.GetLogger("DB_Porting.ExceptionLog");

         private static IVLControlProperties isApplyImageShift = null;

         public IVLControlProperties _IsApplyImageShift
         {
             get { return ImageShiftSettings.isApplyImageShift; }
             set { ImageShiftSettings.isApplyImageShift = value; }
         }
         private static IVLControlProperties ImageShiftX = null;

         public IVLControlProperties _ImageShiftX
         {
             get { return ImageShiftSettings.ImageShiftX; }
             set { ImageShiftSettings.ImageShiftX = value; }
         }
         private static IVLControlProperties ImageShiftY = null;

         public IVLControlProperties _ImageShiftY
         {
             get { return ImageShiftSettings.ImageShiftY; }
             set { ImageShiftSettings.ImageShiftY = value; }
         }

         public ImageShiftSettings()
         {
             try
             {
                 _IsApplyImageShift = new IVLControlProperties();
                 _IsApplyImageShift.name = "isApplyImageShift";
                 _IsApplyImageShift.val = true.ToString();
                 _IsApplyImageShift.type = "bool";
                 _IsApplyImageShift.control = "System.Windows.Forms.RadioButton";
                 _IsApplyImageShift.text = "Apply Image Shift";

                 _ImageShiftX = new IVLControlProperties();
                 _ImageShiftX.name = "ImageShiftX";
                 _ImageShiftX.val = "4";
                 _ImageShiftX.type = "int";
                 _ImageShiftX.control = "System.Windows.Forms.NumericUpDown";
                 _ImageShiftX.text = "Image ShiftX";
                 _ImageShiftX.min = 1;
                 _ImageShiftX.max = 100;

                 _ImageShiftY = new IVLControlProperties();
                 _ImageShiftY.name = "ImageShiftY";
                 _ImageShiftY.val = "3";
                 _ImageShiftY.type = "int";
                 _ImageShiftY.control = "System.Windows.Forms.NumericUpDown";
                 _ImageShiftY.text = "Image ShiftY";
                 _ImageShiftY.min = 1;
                 _ImageShiftY.max = 100;
             }
             catch (Exception ex)
             {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
             }
             

         }
     }
    [Serializable]
    public class MaskSettings
    {
        public static Logger Exception_Log = LogManager.GetLogger("DB_Porting.ExceptionLog");

        private static IVLControlProperties maskWidth = null;

        public IVLControlProperties _MaskWidth
        {
            get { return MaskSettings.maskWidth; }
            set { MaskSettings.maskWidth = value; }
        }
        private static IVLControlProperties maskHeight = null;

        public IVLControlProperties _MaskHeight
        {
            get { return MaskSettings.maskHeight; }
            set { MaskSettings.maskHeight = value; }
        }
        private static IVLControlProperties isApplyMask = null;

        public IVLControlProperties _IsApplyMask
        {
            get { return MaskSettings.isApplyMask; }
            set { MaskSettings.isApplyMask = value; }
        }
        private static IVLControlProperties ApplyLiveMask = null;

        public IVLControlProperties _ApplyLiveMask
        {
            get { return MaskSettings.ApplyLiveMask; }
            set { MaskSettings.ApplyLiveMask = value; }
        }

        public MaskSettings()
        {
            try
            {
                _MaskWidth = new IVLControlProperties();
                _MaskWidth.name = "maskWidth";
                _MaskWidth.val = "2000";
                _MaskWidth.type = "int";
                _MaskWidth.control = "System.Windows.Forms.NumericUpDown";
                _MaskWidth.text = "Mask Width";
                _MaskWidth.min = 1;
                _MaskWidth.max = 10000;

                _MaskHeight = new IVLControlProperties();
                _MaskHeight.name = "maskHeight";
                _MaskHeight.val = "2000";
                _MaskHeight.type = "int";
                _MaskHeight.control = "System.Windows.Forms.NumericUpDown";
                _MaskHeight.text = "Mask Height";
                _MaskHeight.min = 1;
                _MaskHeight.max = 10000;

                _IsApplyMask = new IVLControlProperties();
                _IsApplyMask.name = "isApplyMask";
                _IsApplyMask.val = false.ToString();
                _IsApplyMask.type = "bool";
                _IsApplyMask.control = "System.Windows.Forms.RadioButton";
                _IsApplyMask.text = "Apply Mask";

                _ApplyLiveMask = new IVLControlProperties();
                _ApplyLiveMask.name = "ApplyLiveMask";
                _ApplyLiveMask.val = false.ToString();
                _ApplyLiveMask.type = "bool";
                _ApplyLiveMask.control = "System.Windows.Forms.RadioButton";
                _ApplyLiveMask.text = "Apply Live Mask";
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }
    }
    [Serializable]
    public class VignattingSettings
    {
        public static Logger Exception_Log = LogManager.GetLogger("DB_Porting.ExceptionLog");

        private static IVLControlProperties isApplyVignatting = null;

        public IVLControlProperties _IsApplyVignatting
        {
            get { return VignattingSettings.isApplyVignatting; }
            set { VignattingSettings.isApplyVignatting = value; }
        }
        private static IVLControlProperties VignattingPercentageFactorLive = null;

        public IVLControlProperties _VignattingPercentageFactorLive
        {
            get { return VignattingSettings.VignattingPercentageFactorLive; }
            set { VignattingSettings.VignattingPercentageFactorLive = value; }
        }
        private static IVLControlProperties VignattingRadiusLive = null;

        public IVLControlProperties _VignattingRadiusLive
        {
            get { return VignattingSettings.VignattingRadiusLive; }
            set { VignattingSettings.VignattingRadiusLive = value; }
        }
        private static IVLControlProperties VignattingPercentageFactorPostProcessing = null;

        public IVLControlProperties _VignattingPercentageFactorPostProcessing
        {
            get { return VignattingSettings.VignattingPercentageFactorPostProcessing; }
            set { VignattingSettings.VignattingPercentageFactorPostProcessing = value; }
        }
        private static IVLControlProperties VignattingRadiusPostProcessing = null;

        public IVLControlProperties _VignattingRadiusPostProcessing
        {
            get { return VignattingSettings.VignattingRadiusPostProcessing; }
            set { VignattingSettings.VignattingRadiusPostProcessing = value; }
        }
        private static IVLControlProperties ApplyLiveParabola = null;

        public IVLControlProperties _ApplyLiveParabola
        {
            get { return VignattingSettings.ApplyLiveParabola; }
            set { VignattingSettings.ApplyLiveParabola = value; }
        }

        public VignattingSettings()
        {
            try
            {
                _IsApplyVignatting = new IVLControlProperties();
                _IsApplyVignatting.name = "isApplyVignatting";
                _IsApplyVignatting.val = false.ToString();
                _IsApplyVignatting.type = "bool";
                _IsApplyVignatting.control = "System.Windows.Forms.RadioButton";
                _IsApplyVignatting.text = "Apply Vignatting";

                _ApplyLiveParabola = new IVLControlProperties();
                _ApplyLiveParabola.name = "ApplyLiveParabola";
                _ApplyLiveParabola.val = false.ToString();
                _ApplyLiveParabola.type = "bool";
                _ApplyLiveParabola.control = "System.Windows.Forms.RadioButton";
                _ApplyLiveParabola.text = "Apply Live Parabola";

                _VignattingRadiusLive = new IVLControlProperties();
                _VignattingRadiusLive.name = "VignattingRadiusLive";
                _VignattingRadiusLive.val = "800";
                _VignattingRadiusLive.type = "int";
                _VignattingRadiusLive.control = "System.Windows.Forms.NumericUpDown";
                _VignattingRadiusLive.text = "Vignatting Radius Live";

                _VignattingRadiusPostProcessing = new IVLControlProperties();
                _VignattingRadiusPostProcessing.name = "VignattingRadiusPostProcessing";
                _VignattingRadiusPostProcessing.val = "1000";
                _VignattingRadiusPostProcessing.type = "int";
                _VignattingRadiusPostProcessing.control = "System.Windows.Forms.NumericUpDown";
                _VignattingRadiusPostProcessing.text = "Vignatting Radius Post Processing";

                _VignattingPercentageFactorLive = new IVLControlProperties();
                _VignattingPercentageFactorLive.name = "VignattingPercentageFactorLive";
                _VignattingPercentageFactorLive.val = "0.9";
                _VignattingPercentageFactorLive.type = "float";
                _VignattingPercentageFactorLive.control = "System.Windows.Forms.NumericUpDown";
                _VignattingPercentageFactorLive.text = "Vignatting Percentage Factor Live";

                _VignattingPercentageFactorPostProcessing = new IVLControlProperties();
                _VignattingPercentageFactorPostProcessing.name = "VignattingPercentageFactorPostProcessing";
                _VignattingPercentageFactorPostProcessing.val = "1.0";
                _VignattingPercentageFactorPostProcessing.type = "float";
                _VignattingPercentageFactorPostProcessing.control = "System.Windows.Forms.NumericUpDown";
                _VignattingPercentageFactorPostProcessing.text = "Vignatting Percentage Factor Post Processing";
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            

        }
    }
    [Serializable]
    public class HotSpotSettings
    {
        public static Logger Exception_Log = LogManager.GetLogger("DB_Porting.ExceptionLog");

        private static IVLControlProperties isApplyHotspotCorrection = null;

        public IVLControlProperties _IsApplyHotspotCorrection
        {
            get { return HotSpotSettings.isApplyHotspotCorrection; }
            set { HotSpotSettings.isApplyHotspotCorrection = value; }
        }
        private static IVLControlProperties hotspotRadius1 = null;

        public IVLControlProperties _HotspotRadius1
        {
            get { return HotSpotSettings.hotspotRadius1; }
            set { HotSpotSettings.hotspotRadius1 = value; }
        }
        private static IVLControlProperties hotspotRadius2 = null;

        public IVLControlProperties _HotspotRadius2
        {
            get { return HotSpotSettings.hotspotRadius2; }
            set { HotSpotSettings.hotspotRadius2 = value; }
        }
        private static IVLControlProperties ShadowRadSpot1 = null;

        public IVLControlProperties _ShadowRadSpot1
        {
            get { return HotSpotSettings.ShadowRadSpot1; }
            set { HotSpotSettings.ShadowRadSpot1 = value; }
        }
        private static IVLControlProperties ShadowradSpot2 = null;

        public IVLControlProperties _ShadowradSpot2
        {
            get { return HotSpotSettings.ShadowradSpot2; }
            set { HotSpotSettings.ShadowradSpot2 = value; }
        }

        private static IVLControlProperties HotSpotRedPeak = null;

        public IVLControlProperties _HotSpotRedPeak 
        {
            get { return HotSpotSettings.HotSpotRedPeak; }
            set { HotSpotSettings.HotSpotRedPeak = value; }
        }

        private static IVLControlProperties HotSpotGreenPeak = null;

        public IVLControlProperties _HotSpotGreenPeak 
        {
            get { return HotSpotSettings.HotSpotGreenPeak; }
            set { HotSpotSettings.HotSpotGreenPeak = value; }
        }

        private static IVLControlProperties HotSpotBluePeak = null;

        public IVLControlProperties _HotSpotBluePeak 
        {
            get { return HotSpotSettings.HotSpotBluePeak; }
            set { HotSpotSettings.HotSpotBluePeak = value; }
        }

        private static IVLControlProperties HotSpotRedRadius = null;

        public IVLControlProperties _HotSpotRedRadius 
        {
            get { return HotSpotSettings.HotSpotRedRadius; }
            set { HotSpotSettings.HotSpotRedRadius = value; }
        }

        private static IVLControlProperties HotSpotGreenRadius = null;

        public IVLControlProperties _HotSpotGreenRadius 
        {
            get { return HotSpotSettings.HotSpotGreenRadius; }
            set { HotSpotSettings.HotSpotGreenRadius = value; }
        }

        private static IVLControlProperties HotSpotBlueRadius = null;

        public IVLControlProperties _HotSpotBlueRadius 
        {
            get { return HotSpotSettings.HotSpotBlueRadius; }
            set { HotSpotSettings.HotSpotBlueRadius = value; }
        }

        private static IVLControlProperties ShadowRedPercentage = null;

        public IVLControlProperties _ShadowRedPercentage 
        {
            get { return HotSpotSettings.ShadowRedPercentage; }
            set { HotSpotSettings.ShadowRedPercentage = value; }
        }

        private static IVLControlProperties ShadowGreenPercentage = null;

        public IVLControlProperties _ShadowGreenPercentage 
        {
            get { return HotSpotSettings.ShadowGreenPercentage; }
            set { HotSpotSettings.ShadowGreenPercentage = value; }
        }

        private static IVLControlProperties ShadowBlueBPercentage = null;

        public IVLControlProperties _ShadowBlueBPercentage 
        {
            get { return HotSpotSettings.ShadowBlueBPercentage; }
            set { HotSpotSettings.ShadowBlueBPercentage = value; }
        }
        private static IVLControlProperties GainSlope = null;

        public IVLControlProperties _GainSlope
        {
            get { return HotSpotSettings.GainSlope; }
            set { HotSpotSettings.GainSlope = value; }
        }
        public HotSpotSettings()
        {
            try
            {
                _IsApplyHotspotCorrection = new IVLControlProperties();
                _IsApplyHotspotCorrection.name = "isApplyHotspotCorrection";
                _IsApplyHotspotCorrection.val = true.ToString();
                _IsApplyHotspotCorrection.type = "bool";
                _IsApplyHotspotCorrection.control = "System.Windows.Forms.RadioButton";
                _IsApplyHotspotCorrection.text = "Apply Hotspot Correction";

                _HotspotRadius1 = new IVLControlProperties();
                _HotspotRadius1.name = "hotspotRadius1";
                _HotspotRadius1.val = "-20";
                _HotspotRadius1.type = "int";
                _HotspotRadius1.control = "System.Windows.Forms.NumericUpDown";
                _HotspotRadius1.text = "Hotspot Radius1";
                _HotspotRadius1.min = -255;
                _HotspotRadius1.max = 255;

                _HotspotRadius2 = new IVLControlProperties();
                _HotspotRadius2.name = "hotspotRadius2";
                _HotspotRadius2.val = "100";
                _HotspotRadius2.type = "int";
                _HotspotRadius2.control = "System.Windows.Forms.NumericUpDown";
                _HotspotRadius2.text = "Hotspot Radius2";
                _HotspotRadius2.min = -255;
                _HotspotRadius2.max = 255;


                _ShadowRadSpot1 = new IVLControlProperties();
                _ShadowRadSpot1.name = "ShadowRadSpot1";
                _ShadowRadSpot1.val = "170";
                _ShadowRadSpot1.type = "int";
                _ShadowRadSpot1.control = "System.Windows.Forms.NumericUpDown";
                _ShadowRadSpot1.text = "Shadow RadSpot1";
                _ShadowRadSpot1.min = 100;
                _ShadowRadSpot1.max = 10000;

                _ShadowradSpot2 = new IVLControlProperties();
                _ShadowradSpot2.name = "ShadowradSpot2";
                _ShadowradSpot2.val = "400";
                _ShadowradSpot2.type = "int";
                _ShadowradSpot2.control = "System.Windows.Forms.NumericUpDown";
                _ShadowradSpot2.text = "Shadow RadSpot2";
                _ShadowradSpot2.min = 100;
                _ShadowradSpot2.max = 10000;

                _GainSlope = new IVLControlProperties();
                _GainSlope.name = "gainSlope";
                _GainSlope.val = "5";
                _GainSlope.type = "int";
                _GainSlope.control = "System.Windows.Forms.NumericUpDown";
                _GainSlope.text = "Gain Slope";
                _GainSlope.min = 1;
                _GainSlope.max = 30;

                _HotSpotRedPeak = new IVLControlProperties();
                _HotSpotRedPeak.name = "hotSpotRedPeak";
                _HotSpotRedPeak.val = "0";
                _HotSpotRedPeak.type = "int";
                _HotSpotRedPeak.control = "System.Windows.Forms.NumericUpDown";
                _HotSpotRedPeak.text = "HotSpot RedPeak";
                _HotSpotRedPeak.min = 0;
                _HotSpotRedPeak.max = 255;

                _HotSpotGreenPeak = new IVLControlProperties();
                _HotSpotGreenPeak.name = "HotSpotGreenPeak";
                _HotSpotGreenPeak.val = "0";
                _HotSpotGreenPeak.type = "int";
                _HotSpotGreenPeak.control = "System.Windows.Forms.NumericUpDown";
                _HotSpotGreenPeak.text = "HotSpot GreenPeak";
                _HotSpotGreenPeak.min = 0;
                _HotSpotGreenPeak.max = 255;

                _HotSpotBluePeak = new IVLControlProperties();
                _HotSpotBluePeak.name = "HotSpotBluePeak";
                _HotSpotBluePeak.val = "0";
                _HotSpotBluePeak.type = "int";
                _HotSpotBluePeak.control = "System.Windows.Forms.NumericUpDown";
                _HotSpotBluePeak.text = "HotSpot BluePeak";
                _HotSpotBluePeak.min = 0;
                _HotSpotBluePeak.max = 255;

                _HotSpotRedRadius = new IVLControlProperties();
                _HotSpotRedRadius.name = "HotSpotRedRadius";
                _HotSpotRedRadius.val = "0";
                _HotSpotRedRadius.type = "int";
                _HotSpotRedRadius.control = "System.Windows.Forms.NumericUpDown";
                _HotSpotRedRadius.text = "HotSpot RedRadius";
                _HotSpotRedRadius.min = 0;
                _HotSpotRedRadius.max = 10000;

                _HotSpotGreenRadius = new IVLControlProperties();
                _HotSpotGreenRadius.name = "HotSpotGreenRadius";
                _HotSpotGreenRadius.val = "0";
                _HotSpotGreenRadius.type = "int";
                _HotSpotGreenRadius.control = "System.Windows.Forms.NumericUpDown";
                _HotSpotGreenRadius.text = "HotSpot GreenRadius";
                _HotSpotGreenRadius.min = 0;
                _HotSpotGreenRadius.max = 10000;

                _HotSpotBlueRadius = new IVLControlProperties();
                _HotSpotBlueRadius.name = "HotSpotBlueRadius";
                _HotSpotBlueRadius.val = "0";
                _HotSpotBlueRadius.type = "int";
                _HotSpotBlueRadius.control = "System.Windows.Forms.NumericUpDown";
                _HotSpotBlueRadius.text = "HotSpot BlueRadius";
                _HotSpotBlueRadius.min = 0;
                _HotSpotBlueRadius.max = 10000;

                _ShadowRedPercentage = new IVLControlProperties();
                _ShadowRedPercentage.name = "ShadowRedPercentage";
                _ShadowRedPercentage.val = "20";
                _ShadowRedPercentage.type = "int";
                _ShadowRedPercentage.control = "System.Windows.Forms.NumericUpDown";
                _ShadowRedPercentage.text = "Shadow RedPercentage";
                _ShadowRedPercentage.min = 0;
                _ShadowRedPercentage.max = 100;

                _ShadowGreenPercentage = new IVLControlProperties();
                _ShadowGreenPercentage.name = "ShadowGreenPercentage";
                _ShadowGreenPercentage.val = "15";
                _ShadowGreenPercentage.type = "int";
                _ShadowGreenPercentage.control = "System.Windows.Forms.NumericUpDown";
                _ShadowGreenPercentage.text = "Shadow GreenPercentage";
                _ShadowGreenPercentage.min = 0;
                _ShadowGreenPercentage.max = 100;

                _ShadowBlueBPercentage = new IVLControlProperties();
                _ShadowBlueBPercentage.name = "ShadowBlueBPercentage";
                _ShadowBlueBPercentage.val = "10";
                _ShadowBlueBPercentage.type = "int";
                _ShadowBlueBPercentage.control = "System.Windows.Forms.NumericUpDown";
                _ShadowBlueBPercentage.text = "Shadow BlueBPercentage";
                _ShadowBlueBPercentage.min = 0;
                _ShadowBlueBPercentage.max = 100;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            

        }
    }
    [Serializable]
    public class ColorCorrectionSettings
    {
        public static Logger Exception_Log = LogManager.GetLogger("DB_Porting.ExceptionLog");

        private static IVLControlProperties isApplyColorCorrection = null;

        public IVLControlProperties _IsApplyColorCorrection
        {
            get { return ColorCorrectionSettings.isApplyColorCorrection; }
            set { ColorCorrectionSettings.isApplyColorCorrection = value; }
        }
        private static IVLControlProperties RRCompensation = null;

        public IVLControlProperties _RRCompensation
        {
            get { return ColorCorrectionSettings.RRCompensation; }
            set { ColorCorrectionSettings.RRCompensation = value; }
        }
        private static IVLControlProperties RGCompensation = null;

        public IVLControlProperties _RGCompensation
        {
            get { return ColorCorrectionSettings.RGCompensation; }
            set { ColorCorrectionSettings.RGCompensation = value; }
        }
        private static IVLControlProperties RBCompensation = null;

        public IVLControlProperties _RBCompensation
        {
            get { return ColorCorrectionSettings.RBCompensation; }
            set { ColorCorrectionSettings.RBCompensation = value; }
        }
        private static IVLControlProperties GRCompensation = null;

        public IVLControlProperties _GRCompensation
        {
            get { return ColorCorrectionSettings.GRCompensation; }
            set { ColorCorrectionSettings.GRCompensation = value; }
        }
        private static IVLControlProperties GGCompensation = null;

        public IVLControlProperties _GGCompensation
        {
            get { return ColorCorrectionSettings.GGCompensation; }
            set { ColorCorrectionSettings.GGCompensation = value; }
        }
        private static IVLControlProperties GBCompensation = null;

        public IVLControlProperties _GBCompensation
        {
            get { return ColorCorrectionSettings.GBCompensation; }
            set { ColorCorrectionSettings.GBCompensation = value; }
        }
        private static IVLControlProperties BRCompensation = null;

        public IVLControlProperties _BRCompensation
        {
            get { return ColorCorrectionSettings.BRCompensation; }
            set { ColorCorrectionSettings.BRCompensation = value; }
        }
        private static IVLControlProperties BGCompensation = null;

        public IVLControlProperties _BGCompensation
        {
            get { return ColorCorrectionSettings.BGCompensation; }
            set { ColorCorrectionSettings.BGCompensation = value; }
        }
        private static IVLControlProperties BBCompensation = null;

        public IVLControlProperties _BBCompensation
        {
            get { return ColorCorrectionSettings.BBCompensation; }
            set { ColorCorrectionSettings.BBCompensation = value; }
        }
        public ColorCorrectionSettings()
        {
            try
            {
                _IsApplyColorCorrection = new IVLControlProperties();
                _IsApplyColorCorrection.name = "isApplyColorCorrection";
                _IsApplyColorCorrection.val = true.ToString();
                _IsApplyColorCorrection.type = "bool";
                _IsApplyColorCorrection.control = "System.Windows.Forms.RadioButton";
                _IsApplyColorCorrection.text = "Apply Color Correction";

                _RRCompensation = new IVLControlProperties();
                _RRCompensation.name = "RRCompensation";
                _RRCompensation.val = "1.20";
                _RRCompensation.type = "float";
                _RRCompensation.control = "System.Windows.Forms.NumericUpDown";
                _RRCompensation.text = "RRCompensation";
                _RRCompensation.min = 0;
                _RRCompensation.max = 10;

                _RGCompensation = new IVLControlProperties();
                _RGCompensation.name = "RGCompensation";
                _RGCompensation.val = "0.10";
                _RGCompensation.type = "float";
                _RGCompensation.control = "System.Windows.Forms.NumericUpDown";
                _RGCompensation.text = "RGCompensation";
                _RGCompensation.min = 0;
                _RGCompensation.max = 10;

                _RBCompensation = new IVLControlProperties();
                _RBCompensation.name = "RBCompensation";
                _RBCompensation.val = "0.0";
                _RBCompensation.type = "float";
                _RBCompensation.control = "System.Windows.Forms.NumericUpDown";
                _RBCompensation.text = "RBCompensation";
                _RBCompensation.min = 0;
                _RBCompensation.max = 10;

                _GRCompensation = new IVLControlProperties();
                _GRCompensation.name = "GRCompensation";
                _GRCompensation.val = "0.0";
                _GRCompensation.type = "float";
                _GRCompensation.control = "System.Windows.Forms.NumericUpDown";
                _GRCompensation.text = "GRCompensation";
                _GRCompensation.min = 0;
                _GRCompensation.max = 10;

                _GGCompensation = new IVLControlProperties();
                _GGCompensation.name = "GGCompensation";
                _GGCompensation.val = "0.95";
                _GGCompensation.type = "float";
                _GGCompensation.control = "System.Windows.Forms.NumericUpDown";
                _GGCompensation.text = "GGCompensation";
                _GGCompensation.min = 0;
                _GGCompensation.max = 10;

                _GBCompensation = new IVLControlProperties();
                _GBCompensation.name = "GBCompensation";
                _GBCompensation.val = "0.0";
                _GBCompensation.type = "float";
                _GBCompensation.control = "System.Windows.Forms.NumericUpDown";
                _GBCompensation.text = "GBCompensation";
                _GBCompensation.min = 0;
                _GBCompensation.max = 10;

                _BRCompensation = new IVLControlProperties();
                _BRCompensation.name = "BRCompensation";
                _BRCompensation.val = "0.0";
                _BRCompensation.type = "float";
                _BRCompensation.control = "System.Windows.Forms.NumericUpDown";
                _BRCompensation.text = "BRCompensation";
                _BRCompensation.min = 0;
                _BRCompensation.max = 10;

                _BGCompensation = new IVLControlProperties();
                _BGCompensation.name = "BGCompensation";
                _BGCompensation.val = "0.10";
                _BGCompensation.type = "float";
                _BGCompensation.control = "System.Windows.Forms.NumericUpDown";
                _BGCompensation.text = "BGCompensation";
                _BGCompensation.min = 0;
                _BGCompensation.max = 10;

                _BBCompensation = new IVLControlProperties();
                _BBCompensation.name = "BBCompensation";
                _BBCompensation.val = "1.3";
                _BBCompensation.type = "float";
                _BBCompensation.control = "System.Windows.Forms.NumericUpDown";
                _BBCompensation.text = "BBCompensation";
                _BBCompensation.min = 0;
                _BBCompensation.max = 10;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }
    }
    [Serializable]
    public class UnsharpMaskSettings
    {
        public static Logger Exception_Log = LogManager.GetLogger("DB_Porting.ExceptionLog");

        private static IVLControlProperties IsApplyUnsharpSettings = null;

        public IVLControlProperties _IsApplyUnsharpSettings
        {
            get { return UnsharpMaskSettings.IsApplyUnsharpSettings; }
            set { UnsharpMaskSettings.IsApplyUnsharpSettings = value; }
        }

        private static IVLControlProperties UnSharpAmount = null;

        public IVLControlProperties _UnSharpAmount
        {
            get { return UnsharpMaskSettings.UnSharpAmount; }
            set { UnsharpMaskSettings.UnSharpAmount = value; }
        }

        private static IVLControlProperties MedianFilter = null;

        public IVLControlProperties _MedianFilter
        {
            get { return UnsharpMaskSettings.MedianFilter; }
            set { UnsharpMaskSettings.MedianFilter = value; }
        }

        private static IVLControlProperties UnSharpRadius = null;

        public IVLControlProperties _UnSharpRadius
        {
            get { return UnsharpMaskSettings.UnSharpRadius; }
            set { UnsharpMaskSettings.UnSharpRadius = value; }
        }

        private static IVLControlProperties Threshold = null;

        public IVLControlProperties _Threshold
        {
            get { return UnsharpMaskSettings.Threshold; }
            set { UnsharpMaskSettings.Threshold = value; }
        }

        public UnsharpMaskSettings()
        {
            try
            {
                _IsApplyUnsharpSettings = new IVLControlProperties();
                _IsApplyUnsharpSettings.name = "IsApplyUnsharpSettings";
                _IsApplyUnsharpSettings.val = true.ToString();
                _IsApplyUnsharpSettings.type = "bool";
                _IsApplyUnsharpSettings.control = "System.Windows.Forms.RadioButton";
                _IsApplyUnsharpSettings.text = "Apply Unsharp Mask ";

                _UnSharpAmount = new IVLControlProperties();
                _UnSharpAmount.name = "UnSharpAmount";
                _UnSharpAmount.val = "0.5";
                _UnSharpAmount.type = "float";
                _UnSharpAmount.control = "System.Windows.Forms.NumericUpDown";
                _UnSharpAmount.text = "UnSharp Amount";
                _UnSharpAmount.min = 0;
                _UnSharpAmount.max = 3;

                _MedianFilter = new IVLControlProperties();
                _MedianFilter.name = "MedianFilter";
                _MedianFilter.val = "3";
                _MedianFilter.type = "int";
                _MedianFilter.control = "System.Windows.Forms.NumericUpDown";
                _MedianFilter.text = "Median Filter";
                _MedianFilter.min = 0;
                _MedianFilter.max = 100;

                _UnSharpRadius = new IVLControlProperties();
                _UnSharpRadius.name = "UnSharpAmount";
                _UnSharpRadius.val = "9";
                _UnSharpRadius.type = "int";
                _UnSharpRadius.control = "System.Windows.Forms.NumericUpDown";
                _UnSharpRadius.text = "UnSharp Radius";
                _UnSharpRadius.min = 0;
                _UnSharpRadius.max = 255;

                _Threshold = new IVLControlProperties();
                _Threshold.name = "Threshold";
                _Threshold.val = "40";
                _Threshold.type = "int";
                _Threshold.control = "System.Windows.Forms.NumericUpDown";
                _Threshold.text = "Threshold";
                _Threshold.min = 0;
                _Threshold.max = 255;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
           
        }
      
    }
    [Serializable]
    public class ClaheSettings
    {
        public static Logger Exception_Log = LogManager.GetLogger("DB_Porting.ExceptionLog");

        private static IVLControlProperties ClipValue = null;

        public IVLControlProperties _ClipValue
        {
            get { return ClaheSettings.ClipValue; }
            set { ClaheSettings.ClipValue = value; }
        }

        private static IVLControlProperties IsApplyClaheSettings = null;

        public IVLControlProperties _IsApplyClaheSettings
        {
            get { return ClaheSettings.IsApplyClaheSettings; }
            set { ClaheSettings.IsApplyClaheSettings = value; }
        }
        public ClaheSettings()
        {
            try
            {
                _IsApplyClaheSettings = new IVLControlProperties();
                _IsApplyClaheSettings.name = "isApplyClaheSettings";
                _IsApplyClaheSettings.val = true.ToString();
                _IsApplyClaheSettings.type = "bool";
                _IsApplyClaheSettings.control = "System.Windows.Forms.RadioButton";
                _IsApplyClaheSettings.text = "Apply Clahe";

                _ClipValue = new IVLControlProperties();
                _ClipValue.name = "ClipValue";
                _ClipValue.val = "0.002";
                _ClipValue.type = "float";
                _ClipValue.control = "System.Windows.Forms.NumericUpDown";
                _ClipValue.text = "Clip Value";
                _ClipValue.min = 0;
                _ClipValue.max = 1;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
           
        }
    }
    [Serializable]
    public class LutSettings
    {
        public static Logger Exception_Log = LogManager.GetLogger("DB_Porting.ExceptionLog");

        private static IVLControlProperties IsApplyLutSettings = null;

        public IVLControlProperties _IsApplyLutSettings
        {
            get { return LutSettings.IsApplyLutSettings; }
            set { LutSettings.IsApplyLutSettings = value; }
        }

        private static IVLControlProperties LUTSineFactor = null;

        public IVLControlProperties _LUTSineFactor
        {
            get { return LutSettings.LUTSineFactor; }
            set { LutSettings.LUTSineFactor = value; }
        }

        private static IVLControlProperties LUTInterval1 = null;

        public IVLControlProperties _LUTInterval1
        {
            get { return LutSettings.LUTInterval1; }
            set { LutSettings.LUTInterval1 = value; }
        }

        private static IVLControlProperties LUTInterval2 = null;

        public IVLControlProperties _LUTInterval2
        {
            get { return LutSettings.LUTInterval2; }
            set { LutSettings.LUTInterval2 = value; }
        }

        private static IVLControlProperties LUTOffset= null;

        public IVLControlProperties _LUTOffset
        {
            get { return LutSettings.LUTOffset; }
            set { LutSettings.LUTOffset = value; }
        }

        public LutSettings()
        {
            try
            {
                _IsApplyLutSettings = new IVLControlProperties();
                _IsApplyLutSettings.name = "IsApplyLutSettings";
                _IsApplyLutSettings.val = true.ToString();
                _IsApplyLutSettings.type = "bool";
                _IsApplyLutSettings.control = "System.Windows.Forms.RadioButton";
                _IsApplyLutSettings.text = "Apply Lut";

                _LUTSineFactor = new IVLControlProperties();
                _LUTSineFactor.name = "LUTSineFactor";
                _LUTSineFactor.val = "35";
                _LUTSineFactor.type = "int";
                _LUTSineFactor.control = "System.Windows.Forms.NumericUpDown";
                _LUTSineFactor.text = "LutSine Factor";
                _LUTSineFactor.min = 0;
                _LUTSineFactor.max = 255;

                _LUTInterval1 = new IVLControlProperties();
                _LUTInterval1.name = "LUTInterval1";
                _LUTInterval1.val = "50";
                _LUTInterval1.type = "int";
                _LUTInterval1.control = "System.Windows.Forms.NumericUpDown";
                _LUTInterval1.text = "Lut Interval1";
                _LUTInterval1.min = 0;
                _LUTInterval1.max = 255;

                _LUTInterval2 = new IVLControlProperties();
                _LUTInterval2.name = "LUTInterval2";
                _LUTInterval2.val = "130";
                _LUTInterval2.type = "int";
                _LUTInterval2.control = "System.Windows.Forms.NumericUpDown";
                _LUTInterval2.text = "Lut Interval2";
                _LUTInterval2.min = 0;
                _LUTInterval2.max = 255;

                _LUTOffset = new IVLControlProperties();
                _LUTOffset.name = "LUTOffset";
                _LUTOffset.val = "0";
                _LUTOffset.type = "int";
                _LUTOffset.control = "System.Windows.Forms.NumericUpDown";
                _LUTOffset.text = "Lut Offset";
                _LUTOffset.min = 0;
                _LUTOffset.max = 255;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }
    }
}
