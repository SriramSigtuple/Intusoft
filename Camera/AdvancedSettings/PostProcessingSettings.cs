using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace INTUSOFT.Desktop.AdvancedSettings
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
       public BrightnessContrastSettings BrightnessContrastSettings;
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
            BrightnessContrastSettings = new BrightnessContrastSettings();
        }
    }
    [Serializable]
     public class ImageShiftSettings
     {
         // Image shift parameters
         //public bool isApplyImageShift = true;
         //public int ImageShiftX = 4;
         //public int ImageShiftY = 3;

         private  IVLControlProperties isApplyImageShift = null;

         public IVLControlProperties _IsApplyImageShift
         {
             get { return isApplyImageShift; }
             set { isApplyImageShift = value; }
         }
         private  IVLControlProperties ImageShiftX = null;

         public IVLControlProperties _ImageShiftX
         {
             get { return ImageShiftX; }
             set { ImageShiftX = value; }
         }
         private  IVLControlProperties ImageShiftY = null;

         public IVLControlProperties _ImageShiftY
         {
             get { return ImageShiftY; }
             set { ImageShiftY = value; }
         }
         public ImageShiftSettings()
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
     }
    [Serializable]
    public class MaskSettings
    {
        private  IVLControlProperties maskWidth = null;

        public IVLControlProperties _MaskWidth
        {
            get { return maskWidth; }
            set { maskWidth = value; }
        }
        private  IVLControlProperties maskHeight = null;

        public IVLControlProperties _MaskHeight
        {
            get { return maskHeight; }
            set { maskHeight = value; }
        }
        private  IVLControlProperties isApplyMask = null;

        public IVLControlProperties _IsApplyMask
        {
            get { return isApplyMask; }
            set { isApplyMask = value; }
        }
        private  IVLControlProperties ApplyLiveMask = null;

        public IVLControlProperties _ApplyLiveMask
        {
            get { return ApplyLiveMask; }
            set { ApplyLiveMask = value; }
        }

        private IVLControlProperties ApplyLogo = null;

        public IVLControlProperties _ApplyLogo
        {
            get { return ApplyLogo; }
            set { ApplyLogo = value; }
        }
        public MaskSettings()
        {
            _MaskWidth = new IVLControlProperties();
            _MaskWidth.name = "maskWidth";
            _MaskWidth.val = "2400";
            _MaskWidth.type = "int";
            _MaskWidth.control = "System.Windows.Forms.NumericUpDown";
            _MaskWidth.text = "Mask Width";
            _MaskWidth.min = 1;
            _MaskWidth.max = 10000;

            _MaskHeight = new IVLControlProperties();
            _MaskHeight.name = "maskHeight";
            _MaskHeight.val = "2400";
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

            _ApplyLogo = new IVLControlProperties();
            _ApplyLogo.name = "ApplyLogo";
            _ApplyLogo.val = false.ToString();
            _ApplyLogo.type = "bool";
            _ApplyLogo.control = "System.Windows.Forms.RadioButton";
            _ApplyLogo.text = "Apply Logo";

            _ApplyLiveMask = new IVLControlProperties();
            _ApplyLiveMask.name = "ApplyLiveMask";
            _ApplyLiveMask.val = false.ToString();
            _ApplyLiveMask.type = "bool";
            _ApplyLiveMask.control = "System.Windows.Forms.RadioButton";
            _ApplyLiveMask.text = "Apply Live Mask";
        }
    }

    [Serializable]
    public class VignattingSettings
    {
        private  IVLControlProperties isApplyVignatting = null;

        public IVLControlProperties _IsApplyVignatting
        {
            get { return isApplyVignatting; }
            set { isApplyVignatting = value; }
        }

        private  IVLControlProperties VignattingPercentageFactorLive = null;

        public IVLControlProperties _VignattingPercentageFactorLive
        {
            get { return VignattingPercentageFactorLive; }
            set { VignattingPercentageFactorLive = value; }
        }

        private  IVLControlProperties VignattingRadiusLive = null;

        public IVLControlProperties _VignattingRadiusLive
        {
            get { return VignattingRadiusLive; }
            set { VignattingRadiusLive = value; }
        }

        private  IVLControlProperties VignattingPercentageFactorPostProcessing = null;

        public IVLControlProperties _VignattingPercentageFactorPostProcessing
        {
            get { return VignattingPercentageFactorPostProcessing; }
            set { VignattingPercentageFactorPostProcessing = value; }
        }

        private  IVLControlProperties VignattingRadiusPostProcessing = null;

        public IVLControlProperties _VignattingRadiusPostProcessing
        {
            get { return VignattingRadiusPostProcessing; }
            set { VignattingRadiusPostProcessing = value; }
        }

        private  IVLControlProperties ApplyLiveParabola = null;

        public IVLControlProperties _ApplyLiveParabola
        {
            get { return ApplyLiveParabola; }
            set { ApplyLiveParabola = value; }
        }

        public VignattingSettings()
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
    }

    [Serializable]
    public class HotSpotSettings
    {
        // Hot Spot Co - ordinates
        //public bool isApplyHotspotCorrection = true;
        //public float HotSpotCorrectionFactor = 0.10f;
        //public int HotSpotRadius = 350;
        //public int hotspotRadius1 = 20;
        //public int hotspotRadius2 = 50;
        //public int ShadowRadSpot1 = 175;
        //public int ShadowradSpot2 = 150;
        //public int gainSlope = 5;
        //public bool isNewMethod = false;
        //public float factorR = 1f;
        //public float factorG = 1f;
        //public float factorB = 1f;
        //public int method = 4;
        //public int valueR = 25;
        //public int valueG = 16;
        //public int valueB = 1;
        //public int hotspotOffsetR = 10;
        //public int hotspotOffsetG = 15;
        //public int hotspotOffsetB = 3;


        private  IVLControlProperties isApplyHotspotCorrection = null;

        public IVLControlProperties _IsApplyHotspotCorrection
        {
            get { return isApplyHotspotCorrection; }
            set { isApplyHotspotCorrection = value; }
        }
        private  IVLControlProperties HotSpotCorrectionFactor = null;

        public IVLControlProperties _HotSpotCorrectionFactor
        {
            get { return HotSpotCorrectionFactor; }
            set { HotSpotCorrectionFactor = value; }
        }
        private  IVLControlProperties HotSpotRadius = null;

        public IVLControlProperties _HotSpotRadius
        {
            get { return HotSpotRadius; }
            set { HotSpotRadius = value; }
        }
        private  IVLControlProperties hotspotRadius1 = null;

        public IVLControlProperties _HotspotRadius1
        {
            get { return hotspotRadius1; }
            set { hotspotRadius1 = value; }
        }
        private  IVLControlProperties hotspotRadius2 = null;

        public IVLControlProperties _HotspotRadius2
        {
            get { return hotspotRadius2; }
            set { hotspotRadius2 = value; }
        }
        private  IVLControlProperties ShadowRadSpot1 = null;

        public IVLControlProperties _ShadowRadSpot1
        {
            get { return ShadowRadSpot1; }
            set { ShadowRadSpot1 = value; }
        }
        private  IVLControlProperties ShadowradSpot2 = null;

        public IVLControlProperties _ShadowradSpot2
        {
            get { return ShadowradSpot2; }
            set { ShadowradSpot2 = value; }
        }
        private  IVLControlProperties gainSlope = null;

        public IVLControlProperties _GainSlope
        {
            get { return gainSlope; }
            set { gainSlope = value; }
        }

        private  IVLControlProperties isNewMethod = null;

        public IVLControlProperties _isNewMethod
        {
            get { return isNewMethod; }
            set { isNewMethod = value; }
        }

        private  IVLControlProperties factorR = null;

        public IVLControlProperties _factorR
        {
            get { return factorR; }
            set { factorR = value; }
        }

        private  IVLControlProperties factorG = null;

        public IVLControlProperties _factorG
        {
            get { return factorG; }
            set { factorG = value; }
        }

        private  IVLControlProperties factorB = null;

        public IVLControlProperties _factorB
        {
            get { return factorB; }
            set { factorB = value; }
        }

        private  IVLControlProperties method = null;

        public IVLControlProperties _Method
        {
            get { return method; }
            set { method = value; }
        }
        private  IVLControlProperties valueR = null;

        public IVLControlProperties _ValueR
        {
            get { return valueR; }
            set { valueR = value; }
        }
        private  IVLControlProperties valueG = null;

        public IVLControlProperties _ValueG
        {
            get { return valueG; }
            set { valueG = value; }
        }
        private  IVLControlProperties valueB = null;

        public IVLControlProperties _ValueB
        {
            get { return valueB; }
            set { valueB = value; }
        }
        private  IVLControlProperties hotspotOffsetR = null;

        public IVLControlProperties _HotspotOffsetR
        {
            get { return hotspotOffsetR; }
            set { hotspotOffsetR = value; }
        }
        private  IVLControlProperties hotspotOffsetG = null;

        public IVLControlProperties _HotspotOffsetG
        {
            get { return hotspotOffsetG; }
            set { hotspotOffsetG = value; }
        }
        private  IVLControlProperties hotspotOffsetB = null;

        public IVLControlProperties _HotspotOffsetB
        {
            get { return hotspotOffsetB; }
            set { hotspotOffsetB = value; }
        }
        public HotSpotSettings()
        {
            _IsApplyHotspotCorrection = new IVLControlProperties();
            _IsApplyHotspotCorrection.name = "isApplyHotspotCorrection";
            _IsApplyHotspotCorrection.val = false.ToString();
            _IsApplyHotspotCorrection.type = "bool";
            _IsApplyHotspotCorrection.control = "System.Windows.Forms.RadioButton";
            _IsApplyHotspotCorrection.text = "Apply Hotspot Correction";

            _HotSpotCorrectionFactor = new IVLControlProperties();
            _HotSpotCorrectionFactor.name = "HotSpotCorrectionFactor";
            _HotSpotCorrectionFactor.val = "0.1";
            _HotSpotCorrectionFactor.type = "float";
            _HotSpotCorrectionFactor.control = "System.Windows.Forms.NumericUpDown";
            _HotSpotCorrectionFactor.text = "Hot Spot Correction Factor";
          


            _HotSpotRadius = new IVLControlProperties();
            _HotSpotRadius.name = "HotSpotRadius";
            _HotSpotRadius.val = "350";
            _HotSpotRadius.type = "int";
            _HotSpotRadius.control = "System.Windows.Forms.NumericUpDown";
            _HotSpotRadius.text = "Hot Spot Radius";
          


            _HotspotRadius1 = new IVLControlProperties();
            _HotspotRadius1.name = "hotspotRadius1";
            _HotspotRadius1.val = "20";
            _HotspotRadius1.type = "int";
            _HotspotRadius1.control = "System.Windows.Forms.NumericUpDown";
            _HotspotRadius1.text = "Hotspot Radius1";
            _HotspotRadius1.min = 10;
            _HotspotRadius1.max = 70;


            _HotspotRadius2 = new IVLControlProperties();
            _HotspotRadius2.name = "hotspotRadius2";
            _HotspotRadius2.val = "50";
            _HotspotRadius2.type = "int";
            _HotspotRadius2.control = "System.Windows.Forms.NumericUpDown";
            _HotspotRadius2.text = "Hotspot Radius2";
            _HotspotRadius2.min = 50;
            _HotspotRadius2.max = 100;


            _ShadowRadSpot1 = new IVLControlProperties();
            _ShadowRadSpot1.name = "ShadowRadSpot1";
            _ShadowRadSpot1.val = "175";
            _ShadowRadSpot1.type = "int";
            _ShadowRadSpot1.control = "System.Windows.Forms.NumericUpDown";
            _ShadowRadSpot1.text = "Shadow RadSpot1";
            _ShadowRadSpot1.min = 100;
            _ShadowRadSpot1.max = 300;

            _ShadowradSpot2 = new IVLControlProperties();
            _ShadowradSpot2.name = "ShadowradSpot2";
            _ShadowradSpot2.val = "150";
            _ShadowradSpot2.type = "int";
            _ShadowradSpot2.control = "System.Windows.Forms.NumericUpDown";
            _ShadowradSpot2.text = "Shadow RadSpot2";
            _ShadowradSpot2.min = 100;
            _ShadowradSpot2.max = 300;

            _GainSlope = new IVLControlProperties();
            _GainSlope.name = "gainSlope";
            _GainSlope.val = "5";
            _GainSlope.type = "int";
            _GainSlope.control = "System.Windows.Forms.NumericUpDown";
            _GainSlope.text = "Gain Slope";
            _GainSlope.min = 1;
            _GainSlope.max = 30;

            _isNewMethod = new IVLControlProperties();
            _isNewMethod.name = "isNewMethod";
            _isNewMethod.val = false.ToString();
            _isNewMethod.type = "bool";
            _isNewMethod.control = "System.Windows.Forms.RadioButton";
            _isNewMethod.text = "New Method";

            _factorR = new IVLControlProperties();
            _factorR.name = "factorR";
            _factorR.val = "1";
            _factorR.type = "float";
            _factorR.control = "System.Windows.Forms.NumericUpDown";
            _factorR.text = "FactorR";
            _factorR.min = 0;
            _factorR.max = 3;

            _factorB = new IVLControlProperties();
            _factorB.name = "factorB";
            _factorB.val = "1";
            _factorB.type = "float";
            _factorB.control = "System.Windows.Forms.NumericUpDown";
            _factorB.text = "FactorB";
            _factorB.min = 0;
            _factorB.max = 3;  

            _factorG = new IVLControlProperties();
            _factorG.name = "factorG";
            _factorG.val = "1";
            _factorG.type = "float";
            _factorG.control = "System.Windows.Forms.NumericUpDown";
            _factorG.text = "FactorG";
            _factorG.min = 0;
            _factorG.max = 3;

            _Method = new IVLControlProperties();
            _Method.name = "method";
            _Method.val = "4";
            _Method.type = "int";
            _Method.control = "System.Windows.Forms.NumericUpDown";
            _Method.text = "Method";


            _ValueR = new IVLControlProperties();
            _ValueR.name = "valueR";
            _ValueR.val = "25";
            _ValueR.type = "int";
            _ValueR.control = "System.Windows.Forms.NumericUpDown";
            _ValueR.text = "ValueR";
            _ValueR.min = 0;
            _ValueR.max = 45;


            _ValueG = new IVLControlProperties();
            _ValueG.name = "valueG";
            _ValueG.val = "16";
            _ValueG.type = "int";
            _ValueG.control = "System.Windows.Forms.NumericUpDown";
            _ValueG.text = "ValueG";
            _ValueG.min = 0;
            _ValueG.max = 45;

            _ValueB = new IVLControlProperties();
            _ValueB.name = "valueB";
            _ValueB.val = "1";
            _ValueB.type = "int";
            _ValueB.control = "System.Windows.Forms.NumericUpDown";
            _ValueB.text = "ValueB";
            _ValueB.min = 0;
            _ValueB.max = 45;

            _HotspotOffsetR = new IVLControlProperties();
            _HotspotOffsetR.name = "hotspotOffsetR";
            _HotspotOffsetR.val = "10";
            _HotspotOffsetR.type = "int";
            _HotspotOffsetR.control = "System.Windows.Forms.NumericUpDown";
            _HotspotOffsetR.text = "Hotspot OffSetR";
            _HotspotOffsetR.min = 0;
            _HotspotOffsetR.max = 45;


            _HotspotOffsetG = new IVLControlProperties();
            _HotspotOffsetG.name = "hotspotOffsetG";
            _HotspotOffsetG.val = "15";
            _HotspotOffsetG.type = "int";
            _HotspotOffsetG.control = "System.Windows.Forms.NumericUpDown";
            _HotspotOffsetG.text = "Hotspot OffSetG";
            _HotspotOffsetG.min = 0;
            _HotspotOffsetG.max = 45;

            _HotspotOffsetB = new IVLControlProperties();
            _HotspotOffsetB.name = "hotspotOffsetB";
            _HotspotOffsetB.val = "3";
            _HotspotOffsetB.type = "int";
            _HotspotOffsetB.control = "System.Windows.Forms.NumericUpDown";
            _HotspotOffsetB.text = "Hotspot OffSetB";
            _HotspotOffsetB.min =0;
            _HotspotOffsetB.max = 45;
            
        }
    }
    [Serializable]
    public class ColorCorrectionSettings
    {
        //Post Processing Color Correction
        //public bool isApplyColorCorrection = true;
        //public float RRCompensation = 1f;
        //public float RGCompensation = 0.4f;
        //public float RBCompensation = 0f;
        //public float GRCompensation = 0f;
        //public float GGCompensation = 0.95f;
        //public float GBCompensation = 0f;
        //public float BRCompensation = 0f;
        //public float BGCompensation = 0.2f;
        //public float BBCompensation = 1.5f;

        private  IVLControlProperties isApplyColorCorrection = null;

        public IVLControlProperties _IsApplyColorCorrection
        {
            get { return isApplyColorCorrection; }
            set { isApplyColorCorrection = value; }
        }
        private  IVLControlProperties RRCompensation = null;

        public IVLControlProperties _RRCompensation
        {
            get { return RRCompensation; }
            set { RRCompensation = value; }
        }
        private  IVLControlProperties RGCompensation = null;

        public IVLControlProperties _RGCompensation
        {
            get { return RGCompensation; }
            set { RGCompensation = value; }
        }
        private  IVLControlProperties RBCompensation = null;

        public IVLControlProperties _RBCompensation
        {
            get { return RBCompensation; }
            set { RBCompensation = value; }
        }
        private  IVLControlProperties GRCompensation = null;

        public IVLControlProperties _GRCompensation
        {
            get { return GRCompensation; }
            set { GRCompensation = value; }
        }
        private  IVLControlProperties GGCompensation = null;

        public IVLControlProperties _GGCompensation
        {
            get { return GGCompensation; }
            set { GGCompensation = value; }
        }
        private  IVLControlProperties GBCompensation = null;

        public IVLControlProperties _GBCompensation
        {
            get { return GBCompensation; }
            set { GBCompensation = value; }
        }
        private  IVLControlProperties BRCompensation = null;

        public IVLControlProperties _BRCompensation
        {
            get { return BRCompensation; }
            set { BRCompensation = value; }
        }
        private  IVLControlProperties BGCompensation = null;

        public IVLControlProperties _BGCompensation
        {
            get { return BGCompensation; }
            set { BGCompensation = value; }
        }
        private  IVLControlProperties BBCompensation = null;

        public IVLControlProperties _BBCompensation
        {
            get { return BBCompensation; }
            set { BBCompensation = value; }
        }
        public ColorCorrectionSettings()
        {
            _IsApplyColorCorrection = new IVLControlProperties();
            _IsApplyColorCorrection.name = "isApplyColorCorrection";
            _IsApplyColorCorrection.val = true.ToString();
            _IsApplyColorCorrection.type = "bool";
            _IsApplyColorCorrection.control = "System.Windows.Forms.RadioButton";
            _IsApplyColorCorrection.text = "Apply Color Correction";

            _RRCompensation = new IVLControlProperties();
            _RRCompensation.name = "RRCompensation";
            _RRCompensation.val = "1.0";
            _RRCompensation.type = "float";
            _RRCompensation.control = "System.Windows.Forms.NumericUpDown";
            _RRCompensation.text = "RRCompensation";
            _RRCompensation.min = 0;
            _RRCompensation.max = 10;

            _RGCompensation = new IVLControlProperties();
            _RGCompensation.name = "RGCompensation";
            _RGCompensation.val = "0.4";
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
            _BGCompensation.val = "0.2";
            _BGCompensation.type = "float";
            _BGCompensation.control = "System.Windows.Forms.NumericUpDown";
            _BGCompensation.text = "BGCompensation";
            _BGCompensation.min = 0;
            _BGCompensation.max = 10;

            _BBCompensation = new IVLControlProperties();
            _BBCompensation.name = "BBCompensation";
            _BBCompensation.val = "1.5";
            _BBCompensation.type = "float";
            _BBCompensation.control = "System.Windows.Forms.NumericUpDown";
            _BBCompensation.text = "BBCompensation";
            _BBCompensation.min = 0;
            _BBCompensation.max = 10;




        }
    }
    [Serializable]
    public class UnsharpMaskSettings
    {
        private  IVLControlProperties UnSharpAmount = null;

        public IVLControlProperties _UnSharpAmount
        {
            get { return UnSharpAmount; }
            set { UnSharpAmount = value; }
        }

        private  IVLControlProperties IsApplyUnsharpSettings = null;

        public IVLControlProperties _IsApplyUnsharpSettings
        {
            get { return IsApplyUnsharpSettings; }
            set { IsApplyUnsharpSettings = value; }
        }

        private  IVLControlProperties UnSharpRadius = null;

        public IVLControlProperties _UnSharpRadius
        {
            get { return UnSharpRadius; }
            set { UnSharpRadius = value; }
        }

        private  IVLControlProperties Threshold = null;

        public IVLControlProperties _Threshold
        {
            get { return Threshold; }
            set { Threshold = value; }
        }

        private IVLControlProperties MedFilter = null;

        public IVLControlProperties _MedFilter
        {
            get { return MedFilter; }
            set { MedFilter = value; }
        }
        public UnsharpMaskSettings()
        {
            _IsApplyUnsharpSettings = new IVLControlProperties();
            _IsApplyUnsharpSettings.name = "IsApplyUnsharpSettings";
            _IsApplyUnsharpSettings.val = true.ToString();
            _IsApplyUnsharpSettings.type = "bool";
            _IsApplyUnsharpSettings.control = "System.Windows.Forms.RadioButton";
            _IsApplyUnsharpSettings.text = "Apply Unsharp Settings";

            _UnSharpAmount = new IVLControlProperties();
            _UnSharpAmount.name = "UnSharpAmount";
            _UnSharpAmount.val = "0.5";
            _UnSharpAmount.type = "float";
            _UnSharpAmount.control = "System.Windows.Forms.NumericUpDown";
            _UnSharpAmount.text = "UnSharp Amount";
            _UnSharpAmount.min = 0;
            _UnSharpAmount.max = 10;

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

            _MedFilter = new IVLControlProperties();
            _MedFilter.name = "MedFilter";
            _MedFilter.val = "7";
            _MedFilter.type = "int";
            _MedFilter.control = "System.Windows.Forms.NumericUpDown";
            _MedFilter.text = "Threshold";
            _MedFilter.min = 1;
            _MedFilter.max = 255;



        }
      
    }
    [Serializable]
    public class ClaheSettings
    {
        private  IVLControlProperties ClipValueB = null;

        public IVLControlProperties _ClipValueB
        {
            get { return ClipValueB; }
            set { ClipValueB = value; }
        }

        private   IVLControlProperties ClipValueG = null;

        public IVLControlProperties _ClipValueG
        {
            get { return ClipValueG; }
            set { ClipValueG = value; }
        }
        private  IVLControlProperties ClipValueR = null;

        public IVLControlProperties _ClipValueR
        {
            get { return ClipValueR; }
            set { ClipValueR = value; }
        }

        private  IVLControlProperties IsApplyClaheSettings = null;

        public IVLControlProperties _IsApplyClaheSettings
        {
            get { return IsApplyClaheSettings; }
            set { IsApplyClaheSettings = value; }
        }
        public ClaheSettings()
        {
            
            _IsApplyClaheSettings = new IVLControlProperties();
            _IsApplyClaheSettings.name = "isApplyClaheSettings";
            _IsApplyClaheSettings.val = true.ToString();
            _IsApplyClaheSettings.type = "bool";
            _IsApplyClaheSettings.control = "System.Windows.Forms.RadioButton";
            _IsApplyClaheSettings.text = "Apply Clahe";

            _ClipValueR = new IVLControlProperties();
            _ClipValueR.name = "ClipValueR";
            _ClipValueR.val = "0.0002";
            _ClipValueR.type = "float";
            _ClipValueR.control = "System.Windows.Forms.NumericUpDown";
            _ClipValueR.text = "Clip Value Red";
            _ClipValueR.min = 0;
            _ClipValueR.max = 1;

            _ClipValueG = new IVLControlProperties();
            _ClipValueG.name = "ClipValueG";
            _ClipValueG.val = "0.0002";
            _ClipValueG.type = "float";
            _ClipValueG.control = "System.Windows.Forms.NumericUpDown";
            _ClipValueG.text = "Clip Value Green";
            _ClipValueG.min = 0;
            _ClipValueG.max = 1;


            _ClipValueB = new IVLControlProperties();
            _ClipValueB.name = "ClipValueB";
            _ClipValueB.val = "0.0002";
            _ClipValueB.type = "float";
            _ClipValueB.control = "System.Windows.Forms.NumericUpDown";
            _ClipValueB.text = "Clip Value Blue";
            _ClipValueB.min = 0;
            _ClipValueB.max = 1;
        }
    }

    [Serializable]
    public class BrightnessContrastSettings
    {
        private IVLControlProperties BrightnessVal = null;

        public IVLControlProperties _BrightnessVal
        {
            get { return BrightnessVal; }
            set { BrightnessVal = value; }
        }

        private IVLControlProperties ContrastVAl = null;

        public IVLControlProperties _ContrastVAl
        {
            get { return ContrastVAl; }
            set { ContrastVAl = value; }
        }

        private IVLControlProperties IsApplyBrightness = null;

        public IVLControlProperties _IsApplyBrightness
        {
            get { return IsApplyBrightness; }
            set { IsApplyBrightness = value; }
        }

        private IVLControlProperties IsApplyContrast = null;

        public IVLControlProperties _IsApplyContrast
        {
            get { return  IsApplyContrast; }
            set {  IsApplyContrast = value; }
        }
        public BrightnessContrastSettings()
        {

            _IsApplyContrast = new IVLControlProperties();
            _IsApplyContrast.name = "IsApplyContrast";
            _IsApplyContrast.val = true.ToString();
            _IsApplyContrast.type = "bool";
            _IsApplyContrast.control = "System.Windows.Forms.RadioButton";
            _IsApplyContrast.text = "Apply Contrast";

            _IsApplyBrightness = new IVLControlProperties();
            _IsApplyBrightness.name = "IsApplyBrightness";
            _IsApplyBrightness.val = true.ToString();
            _IsApplyBrightness.type = "bool";
            _IsApplyBrightness.control = "System.Windows.Forms.RadioButton";
            _IsApplyBrightness.text = "Apply Brightness";

            _BrightnessVal = new IVLControlProperties();
            _BrightnessVal.name = "BrightnessVal";
            _BrightnessVal.val = "3";
            _BrightnessVal.type = "int";
            _BrightnessVal.control = "System.Windows.Forms.NumericUpDown";
            _BrightnessVal.text = "Brightness Value";
            _BrightnessVal.min = 0;
            _BrightnessVal.max = 100;

            _ContrastVAl = new IVLControlProperties();
            _ContrastVAl.name = "ContrastVAl";
            _ContrastVAl.val = "1";
            _ContrastVAl.type = "int";
            _ContrastVAl.control = "System.Windows.Forms.NumericUpDown";
            _ContrastVAl.text = "Contrast Value";
            _ContrastVAl.min = -20;
            _ContrastVAl.max = 20;

        }
    }
    [Serializable]
    public class LutSettings
    {

        private  IVLControlProperties IsApplyLutSettings = null;

        public IVLControlProperties _IsApplyLutSettings
        {
            get { return IsApplyLutSettings; }
            set { IsApplyLutSettings = value; }
        }
        
        private  IVLControlProperties LUTSineFactor = null;

        public IVLControlProperties _LUTSineFactor
        {
            get { return LUTSineFactor; }
            set { LUTSineFactor = value; }
        }

        private  IVLControlProperties LUTInterval1 = null;

        public IVLControlProperties _LUTInterval1
        {
            get { return LUTInterval1; }
            set { LUTInterval1 = value; }
        }

        private  IVLControlProperties LUTInterval2 = null;

        public IVLControlProperties _LUTInterval2
        {
            get { return LUTInterval2; }
            set { LUTInterval2 = value; }
        }

        private  IVLControlProperties LUTOffset= null;

        public IVLControlProperties _LUTOffset
        {
            get { return LUTOffset; }
            set { LUTOffset = value; }
        }
        public LutSettings()
        {
            _IsApplyLutSettings = new IVLControlProperties();
            _IsApplyLutSettings.name = "IsApplyLutSettings";
            _IsApplyLutSettings. val = true.ToString();
            _IsApplyLutSettings.type = "bool";
            _IsApplyLutSettings.control = "System.Windows.Forms.RadioButton";
            _IsApplyLutSettings.text = "Apply Lut";

            _LUTSineFactor = new IVLControlProperties();
            _LUTSineFactor.name = "LUTSineFactor";
            _LUTSineFactor.val = "40";
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
            _LUTInterval2.val = "170";
            _LUTInterval2.type = "int";
            _LUTInterval2.control = "System.Windows.Forms.NumericUpDown";
            _LUTInterval2.text = "Lut Interval2";
            _LUTInterval2.min = 0;
            _LUTInterval2.max = 255;

            _LUTOffset = new IVLControlProperties();
            _LUTOffset.name = "LUTOffset";
            _LUTOffset.val = "25";
            _LUTOffset.type = "int";
            _LUTOffset.control = "System.Windows.Forms.NumericUpDown";
            _LUTOffset.text = "Lut Offset";
            _LUTOffset.min = 0;
            _LUTOffset.max = 255;
        }
    }

}
