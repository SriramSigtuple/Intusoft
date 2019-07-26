using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;
using NLog;
using NLog.Config;
using NLog.Targets;


namespace INTUSOFT.Imaging
{
    public class PostProcessing
    {
        private static readonly Logger log = LogManager.GetLogger("INTUSOFT.Imaging");
        private static readonly Logger Exception_Log = LogManager.GetLogger("ExceptionLog");

        public Image<Bgr, byte> CaptureMaskBitmap, LiveMaskBitmap;
        static Image<Gray, byte> redChannelImage;
        public Bitmap srcBm;
        public List<int> orderList;
        public List<PostProcessingStep> PP_OrderList;
        public static int maskCentreX = 900, maskCentreY = 700, maskWidth = 1700, maskHeight = 1700;
        public int LiveMaskImageWidth = 1600, LiveMaskImageHeight = 1440, CaputureMaskImageWidth = 1600, CaputureMaskImageHeight = 1440;
        public int CameraShiftY1 = 10, CameraShiftY2 = 60, CameraShiftHeight = 120;
        public int hotspotOffsetR = 10, hotspotOffsetG = 15, hotspotOffsetB = 3;


        public int RimThickness = 50, Shadow4Calculation = 200;
        public float factorR = 1, factorG = 0.75f, factorB = 1;
        public static bool isDemosaicInit = false;
        public static int Width = 2048, Height = 1536;
        public bool isParabolaCreated = false;
        static PostProcessing _postProcessing;
        public static bool isFourteenBit = false;


        public static List<INTUSOFT.EventHandler.Args> capture_log;
        public INTUSOFT.EventHandler.Args logArg;


        public static bool isChannelWise;



        public float clipValueClahe = 0.001f;
        public float[,] colorMatrix = new float[3, 3];
        public float hsvBoostVal = 1.5f;



        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProc_ComputeParabolaForVignatting")]
        private static extern void ImageProc_ComputeParabolaForVignatting(int centreX, int centreY, float percentFactor, int radius, int width, int height);

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProc_ComputeParabolaForVignattingHotspot")]
        private static extern void ImageProc_ComputeParabolaForVignattingHotspot(int centreX, int centreY, float percentFactor, int radius, int width, int height, float hotspotFactor, int hotspotRadius);

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProc_Demosaic_Init")]
        private static extern void ImageProc_Demosaic_Init(int IMG_WIDTH, int IMG_HEIGHT, bool isFourteenBit, bool isRawMode, int centreX, int centreY);

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProc_Demosaic_Exit")]
        public static extern void ImageProc_Demosaic_Exit();

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProc_ApplyParabolicCompensationLive")]
        private static extern void ImageProc_ApplyParabolicCompensationLive(IntPtr srcPtr, IntPtr destImg);

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProc_ApplyParabolicHotspotCompensationLive")]
        private static extern void ImageProc_ApplyParabolicHotspotCompensationLive(IntPtr srcPtr, IntPtr destImg);

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProc_ApplyParabolicCompensationPostProcessing")]
        private static extern void ImageProc_ApplyParabolicCompensationPostProcessing(IntPtr srcPtr);

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProc_ApplyLUT")]
        private static extern void ImageProc_ApplyLUT(IntPtr srcPtr, IntPtr MonoChromeImage, int width, int height, bool is8bit, bool isColor, bool isChannelWise);

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProc_ApplyParabolicCompensationPostProcessing_New")]
        private static extern void ImageProc_ApplyParabolicCompensationPostProcessing_New(IntPtr srcPtr, int IMG_WIDTH, int IMG_HEIGHT);

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProc_ApplyHotSpotCompensationCPlusCplus")]
        private static extern void ImageProc_ApplyHotSpotCompensationCPlusCplus(IntPtr bm, int centreX, int centreY, int method, int ShadowRadSpot1, int ShadowRadSpot2, int hotspotRad1, int hotspotRad2, int currentGain, int presetGain, int percentageR, int percentageG, int percentageB, int hotspotOffsetR, int hotspotOffsetG, int hotspotOffsetB, int gainSlope);
        [DllImport("IntuSoftDemosaic.dll", EntryPoint = "ImageProc_CalculateImageParams")]
        private static extern IntPtr ImageProc_CalculateImageParams(IntPtr GbPtr, int IMG_WIDTH, int IMG_HEIGHT, int method, int CenterX, int CenterY, int Rim_thickness, int Shadow_thickness, int Hotspot_thickness);

        [DllImport("IntuSoftDemosaic.dll", EntryPoint = "ImageProc_ApplyHSVBOOSt")]
        private static extern IntPtr ImageProc_ApplyHSVBOOSt(IntPtr inp, IntPtr outputImg, float Value, int width, int height);

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProc_ApplyClahe")]
        public static extern IntPtr ImageProc_ApplyClahe(IntPtr inp, IntPtr MonoChromeImage, int width, int height, float ClipValB, float ClipValG, float ClipValR, bool isColor);

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProc_ApplyUnsharpMask")]
        public static extern void ImageProc_ApplyUnsharpMask(IntPtr inp,IntPtr MonoChromeImage, double thres, double amount, double sigma,int medianFilterSize, bool isColor);

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProc_Demosaic")]
        public static extern void ImageProc_Demosaic(IntPtr GbPtr, IntPtr GrPtr, IntPtr outputImg, int IMG_WIDTH, int IMG_HEIGHT, bool isFlash);

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProc_Demosaic_16bit")]
        public static extern void ImageProc_Demosaic_16bit(IntPtr GbPtr, IntPtr outputImg, int IMG_WIDTH, int IMG_HEIGHT, bool isApplylut);

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProc_CalculateLut")]
        public static extern void ImageProc_CalculateLut(double sineFactor, double interval1, double interval2, int bitDepth, bool isFourteenBit, int offset, bool isChannelWise, int channelCode);

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProc_CalculateChannelWiseLut")]
        public static extern void ImageProc_CalculateChannelWiseLut(double sineFactor, double interval1, double interval2, int bitDepth, bool isFourteenBit, int offset, int channelCode);


        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProc_CalculateMidtoneLut")]
        public static extern void ImageProc_CalculateMidtoneLut(int Amplitude, int StartPoint, int EndPoint, int bitDepth);

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProc_ApplyHotSpotCompensation2CPlusCplus")]
        private static extern void ImageProc_ApplyHotSpotCompensation2CPlusCplus(IntPtr bm, int centreX, int centreY, int imgWidth, int imgHeight, int innerRadius, int peakRadius1, int peakRadius2, int outerRadius, int peakdropPercentageR, int peakdropPercentageG, int peakdropPercentageB);

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProc_HotspotCompensation")]
        private static extern void ImageProc_HotspotCompensation(IntPtr bm, IntPtr MonoChromeImage, int centreX, int centreY, int imgWidth, int imgHeight, int HsRedPeak, int HsGreenPeak, int HsBluePeak, int HsRedRadius, int HsGreenRadius, int HsBlueRadius, bool isColor);

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProc_ShadowCompensation")]
        private static extern void ImageProc_ShadowCompensation(IntPtr bm, IntPtr MonoChromeImage, int centreX, int centreY, int imgWidth, int imgHeight, int innerRadius, int peakRadius1, int peakRadius2, int outerRadius, int peakdropPercentageR, int peakdropPercentageG, int peakdropPercentageB, bool isColor);

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ColorCorrection")]
        public static extern void ColorCorrection(IntPtr inp, int n, double a, int brightnessAdjust, int colorCorretion, int guidedFilter);

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProc_CalculateHotspotImageParams")]
        public static extern IntPtr ImageProc_CalculateHotspotImageParams(IntPtr ImgPtr, int CenterX, int CenterY, int width, int height, int r1, int r2, int r3, int r4,IntPtr redChanImg);

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "CreateMask")]
        public static extern void CreateMask(IntPtr maskImg, bool isLive);

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProc_ApplyMask")]
        public static extern void ImageProc_ApplyMask(IntPtr ImgPtr, bool isLive,int Width, int Height);

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProc_Denoise")]
        public static extern void ImageProc_Denoise(IntPtr ImgPtr, int Val);

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProc_BoostImage")]
        public static extern void ImageProc_BoostImage(IntPtr ImgPtr, float Val);

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProc_GetMonoChromeImage")]
        public static extern void ImageProc_GetMonoChromeImage(IntPtr ImgPtr);
        [DllImport("LiveModeProcessing.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "AssistedFocus_Init")]
        public static extern void AssistedFocus_Init(int AvgBrightnessRef, double covLowerLimit, double covUpperLimit, int numberOfScales, int EntryPeakPercentage, int ExitPeakPercentage, int MaxGain, int width, int height);

        [DllImport("LiveModeProcessing.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "AssistedFocus_Exit")]
        public static extern void AssistedFocus_Exit();

        [DllImport("LiveModeProcessing.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "AssistedFocus_GetFrameState")]
        unsafe public static extern void AssistedFocus_GetFrameState(IntPtr img, int* state, int gain, float* sharpnessVal);
        

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProc_InitStitcher")]
        private static extern void InitStitcher();

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProc_ImageStich")]
        unsafe private static extern IntPtr ImageStich(IntPtr[] Input1, int arrCnt, StitchingParams* StitchingParamValues, ref int widht, ref int height, int rowMax, int colMax);

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImageProc_GetStitchedImage")]
        unsafe private static extern void GetStitchedOutputImage(IntPtr output);

        [DllImport("IntuSoftDemosaic.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ApplyClaheCPlusPlus")]
        private static extern void ApplyClaheCPlusPlus( IntPtr vMat,IntPtr output,double[] clipVals,int count);

        unsafe public struct StitchingParams
        {
            public double ConfThresh;
            public int oct;
            public int layer;
            public int oct_desc;
            public int layer_desc;
        }


        unsafe struct ImageParamsStruct
        {

            //double Entropy_red_shadow_mask_1;
            //double Entropy_green_shadow_mask_1;
            //double Entropy_blue_shadow_mask_1;

            //double Entropy_red_shadow_mask_2;
            //double Entropy_green_shadow_mask_2;
            //double Entropy_blue_shadow_mask_2;

            //double Entropy_red_shadow_mask_3;
            //double Entropy_green_shadow_mask_3;
            //double Entropy_blue_shadow_mask_3;

            //double Entropy_red_shadow_mask_4;
            //double Entropy_green_shadow_mask_4;
            //double Entropy_blue_shadow_mask_4;

            //double Entropy_red_shadow_mask_5;
            //double Entropy_green_shadow_mask_5;
            //double Entropy_blue_shadow_mask_5;

            //double Entropy_red_shadow_mask_6;
            //double Entropy_green_shadow_mask_6;
            //double Entropy_blue_shadow_mask_6;

            //double Entropy_red_shadow_mask_7;
            //double Entropy_green_shadow_mask_7;
            //double Entropy_blue_shadow_mask_7;

            //double Entropy_red_shadow_mask_8;
            //double Entropy_green_shadow_mask_8;
            //double Entropy_blue_shadow_mask_8;

            //double Entropy_red_circum_mask_1;
            //double Entropy_green_circum_mask_1;
            //double Entropy_blue_circum_mask_1;

            //double Entropy_red_circum_mask_2;
            //double Entropy_green_circum_mask_2;
            //double Entropy_blue_circum_mask_2;

            //double Entropy_red_circum_mask_3;
            //double Entropy_green_circum_mask_3;
            //double Entropy_blue_circum_mask_3;

            //double Entropy_red_circum_mask_4;
            //double Entropy_green_circum_mask_4;
            //double Entropy_blue_circum_mask_4;

            //double Entropy_red_circum_mask_5;
            //double Entropy_green_circum_mask_5;
            //double Entropy_blue_circum_mask_5;

            //double Entropy_red_circum_mask_6;
            //double Entropy_green_circum_mask_6;
            //double Entropy_blue_circum_mask_6;

            //double Entropy_red_circum_mask_7;
            //double Entropy_green_circum_mask_7;
            //double Entropy_blue_circum_mask_7;

            //double Entropy_red_circum_mask_8;
            //double Entropy_green_circum_mask_8;
            //double Entropy_blue_circum_mask_8;

            //double STD_red_shadow_mask_1;
            //double STD_green_shadow_mask_1;
            //double STD_blue_shadow_mask_1;

            //double STD_red_shadow_mask_2;
            //double STD_green_shadow_mask_2;
            //double STD_blue_shadow_mask_2;

            //double STD_red_shadow_mask_3;
            //double STD_green_shadow_mask_3;
            //double STD_blue_shadow_mask_3;

            //double STD_red_shadow_mask_4;
            //double STD_green_shadow_mask_4;
            //double STD_blue_shadow_mask_4;

            //double STD_red_shadow_mask_5;
            //double STD_green_shadow_mask_5;
            //double STD_blue_shadow_mask_5;

            //double STD_red_shadow_mask_6;
            //double STD_green_shadow_mask_6;
            //double STD_blue_shadow_mask_6;

            //double STD_red_shadow_mask_7;
            //double STD_green_shadow_mask_7;
            //double STD_blue_shadow_mask_7;

            //double STD_red_shadow_mask_8;
            //double STD_green_shadow_mask_8;
            //double STD_blue_shadow_mask_8;

            //double STD_red_circum_mask_1;
            //double STD_green_circum_mask_1;
            //double STD_blue_circum_mask_1;

            //double STD_red_circum_mask_2;
            //double STD_green_circum_mask_2;
            //double STD_blue_circum_mask_2;

            //double STD_red_circum_mask_3;
            //double STD_green_circum_mask_3;
            //double STD_blue_circum_mask_3;

            //double STD_red_circum_mask_4;
            //double STD_green_circum_mask_4;
            //double STD_blue_circum_mask_4;

            //double STD_red_circum_mask_5;
            //double STD_green_circum_mask_5;
            //double STD_blue_circum_mask_5;

            //double STD_red_circum_mask_6;
            //double STD_green_circum_mask_6;
            //double STD_blue_circum_mask_6;

            //double STD_red_circum_mask_7;
            //double STD_green_circum_mask_7;
            //double STD_blue_circum_mask_7;

            //double STD_red_circum_mask_8;
            //double STD_green_circum_mask_8;
            //double STD_blue_circum_mask_8;
            public double Entropy_red_full;
            public double Entropy_green_full;
            public double Entropy_blue_full;

            public double Entropy_red_shadowed;
            public double Entropy_green_shadowed;
            public double Entropy_blue_shadowed;

            public double Entropy_red_circum;
            public double Entropy_green_circum;
            public double Entropy_blue_circum;

            public double Mean_red_full;
            public double Mean_green_full;
            public double Mean_blue_full;

            public double Mean_red_shadowed;
            public double Mean_green_shadowed;
            public double Mean_blue_shadowed;

            public double Mean_red_circum;
            public double Mean_green_circum;
            public double Mean_blue_circum;

            public double std_red_full;
            public double std_green_full;
            public double std_blue_full;

            public double std_red_shadowed;
            public double std_green_shadowed;
            public double std_blue_shadowed;

            public double std_red_circum;
            public double std_green_circum;
            public double std_blue_circum;

            public double percentageEntropyChange_R;
            public double percentageEntropyChange_G;
            public double percentageEntropyChange_B;

            public double percentageSTDChange_R;
            public double percentageSTDChange_G;
            public double percentageSTDChange_B;
        }

        unsafe public struct HotSpotParams
        {
            public fixed double r1Entropy[3];
            public fixed double r2Entropy[3];
            public fixed double r3Entropy[3];
            public fixed double r4Entropy[3];

            public fixed double r1STD[3];
            public fixed double r2STD[3];
            public fixed double r3STD[3];
            public fixed double r4STD[3];

            public fixed double r1Brighntess[3];
            public fixed double r2Brighntess[3];
            public fixed double r3Brighntess[3];
            public fixed double r4Brighntess[3];

            public fixed double r1Contrast[3];
            public fixed double r2Contrast[3];
            public fixed double r3Contrast[3];
            public fixed double r4Contrast[3];

            //public fixed double r23Contrast[3];
            //public fixed double r23Brighntess[3];
            //public fixed double r23STD[3];
            //public fixed double r23Entropy[3];

            //public fixed double r12Contrast[3];
            //public fixed double r12Brighntess[3];
            //public fixed double r12STD[3];
            //public fixed double r12Entropy[3];

            public fixed double FocusVal[3];
        }
        unsafe ImageParamsStruct* _Parameters;
        unsafe public HotSpotParams* hotSpotParams;

        public PostProcessing()
        {
            if(IVLCamVariables._Settings == null)
                IVLCamVariables._Settings = CameraModuleSettings.GetInstance();
            orderList = new List<int>();
            for (int i = 1; i < 11; i++)
            {
                orderList.Add(i);
            }
            capture_log = new List<EventHandler.Args>();

        }

        public static PostProcessing GetInstance()
        {
            try
            {
                if (_postProcessing == null)
                {
                    _postProcessing = new PostProcessing();
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
            }
            return _postProcessing;
        }
        public static void initDemosaic(int width, int height, int centreX, int centreY)
        {
            try
            {
                unsafe
                {
                    if (!isDemosaicInit)
                    {
                        Width = width;
                        Height = height;

                        ImageProc_Demosaic_Init(Width, Height, isFourteenBit, IVLCamVariables._Settings.CameraSettings.isRawMode, centreX, centreY);
                        
                        redChannelImage = new Image<Gray, byte>(Width, Height);
                        //if (IVLCamVariables._Settings.PostProcessingSettings.hotspotSettings.isEnableHS)
                        {
                            AssistedFocus_Init(0, 0, 0, 3, 0, 0, 0, width, height);
                            hsStruct = new HotSpotParams();
                        }
                        isDemosaicInit = true;

                    }
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
            }
        }

        public void CalculateLUT(LutSettings lutSettings, int channelCode = 0)
        {
            try
            {
                int bitDepth = 0;
                if (IVLCamVariables._Settings.CameraSettings.isFourteen)
                    bitDepth = 14;
                else
                    bitDepth = 8;
                if (lutSettings.isChannelWiseLUT)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        if (i == 1)
                            ImageProc_CalculateLut(lutSettings.LUTR.LUT_SineFactor, lutSettings.LUTR.LUT_interval1, lutSettings.LUTR.LUT_interval2, bitDepth, IVLCamVariables._Settings.CameraSettings.isFourteen, lutSettings.LUTR.LUT_Offset, lutSettings.isChannelWiseLUT, i);
                        else if (i == 2)
                            ImageProc_CalculateLut(lutSettings.LUTG.LUT_SineFactor, lutSettings.LUTG.LUT_interval1, lutSettings.LUTG.LUT_interval2, bitDepth, IVLCamVariables._Settings.CameraSettings.isFourteen, lutSettings.LUTG.LUT_Offset, lutSettings.isChannelWiseLUT, i);
                        else if (i == 3)
                            ImageProc_CalculateLut(lutSettings.LUTB.LUT_SineFactor, lutSettings.LUTB.LUT_interval1, lutSettings.LUTB.LUT_interval2, bitDepth, IVLCamVariables._Settings.CameraSettings.isFourteen, lutSettings.LUTB.LUT_Offset, lutSettings.isChannelWiseLUT, i);

                    }
                }
                else
                    ImageProc_CalculateLut(lutSettings.LUT_SineFactor, lutSettings.LUT_interval1, lutSettings.LUT_interval2, bitDepth, IVLCamVariables._Settings.CameraSettings.isFourteen, lutSettings.LUT_Offset, lutSettings.isChannelWiseLUT,channelCode);

            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
            }
            
        }


        public void CalculateChannelWiseLUT(int interval1, int interval2, int sineFactor, int bitDepth, bool is14bit, int offset, bool isChannelWise, int channelCode)
        {
            try
            {
                if (IVLCamVariables._Settings.CameraSettings.isFourteen)
                    bitDepth = 14;
                else
                    bitDepth = 8;
                ImageProc_CalculateLut(sineFactor, interval1, interval2, bitDepth, IVLCamVariables._Settings.CameraSettings.isFourteen, offset, isChannelWise, channelCode);
            }
            catch (Exception ex)
            {
                CameraLogger.WriteException(ex, Exception_Log);
                //                 CameraLogger.WriteException(ex, Exception_Log);
            }

        }

        unsafe static HotSpotParams hsStruct;
        public double time;
        public void CalculateHotspotParams(ref Bitmap srcImg, int centreX, int centreY, string dirName)
        {
            try
            {
                int count = 0;
                unsafe
                {

                    BitmapData bdata = srcImg.LockBits(new Rectangle(0, 0, srcImg.Width, srcImg.Height), ImageLockMode.ReadWrite, srcImg.PixelFormat);
                    int r1 = 120;
                    int r2 = 300;
                    int r3 = 600;
                    int r4 = 900;
                    var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(hsStruct));
                    Marshal.StructureToPtr(hsStruct, ptr, false);
                    hotSpotParams = (HotSpotParams*)ptr;

                    hotSpotParams = (HotSpotParams*)ImageProc_CalculateHotspotImageParams(bdata.Scan0, centreX, centreY, srcImg.Width, srcImg.Height, r1, r2, r3, r4,redChannelImage);

                   // Image<Bgr, byte> inp = new Image<Bgr, byte>(srcImg);
                    float value = 0f;
                    int state = 0;
                    //  Image<Gray, byte> greenChan = inp[1].Copy();

                   // AssistedFocus_GetFrameState(greenChan.Ptr, &state, 0, &value);
                    //hotSpotParams->FocusVal[1] = value;
                    AssistedFocus_GetFrameState(bdata.Scan0, &state, 0, &value);
                    //hotSpotParams->FocusVal[2] = Math.Sqrt(value);
                    hotSpotParams->FocusVal[2] = (value);
                    srcImg.UnlockBits(bdata);

                   // inp.Dispose();
                    //RedChan.Dispose();

                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
            }
        }

        public void ComputeParabola(int centreX, int centreY, float percentFactor, int radius, int width, int height, float hotspotFactor, int hotspotRadius)
        {
            try
            {
                ImageProc_Demosaic_Init(width, height, isFourteenBit, IVLCamVariables._Settings.CameraSettings.isRawMode, IVLCamVariables._Settings.PostProcessingSettings.maskSettings.maskCentreX, IVLCamVariables._Settings.PostProcessingSettings.maskSettings.maskCentreY);
                ImageProc_ComputeParabolaForVignatting(centreX, centreY, percentFactor, radius, width, height);
                ImageProc_ComputeParabolaForVignattingHotspot(centreX, centreY, percentFactor, radius, width, height, hotspotFactor, hotspotRadius);
                isParabolaCreated = true;
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);
            }
           
        }

        public void ReleaseParabola()
        {
            try
            {
                ImageProc_Demosaic_Exit();
                isParabolaCreated = false;
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log); 
            }
            
        }

        private void GenerateMask(int width, int height, MaskSettings settings)
        {
            try
            {
                if (IVLCamVariables.isLive)
                {
                    if (LiveMaskBitmap == null || width != (IVLCamVariables._Settings.CameraSettings.ImageWidth - 2 * IVLCamVariables._Settings.CameraSettings.roiX) || height != (IVLCamVariables._Settings.CameraSettings.ImageHeight - IVLCamVariables._Settings.CameraSettings.roiY) || maskCentreX != settings.maskCentreX || maskCentreY != settings.maskCentreY || LiveMaskImageHeight != settings.LiveMaskHeight || LiveMaskImageWidth != settings.LiveMaskWidth)//Condition has been added in  case of the scenario where the maskbitmap is not equal to null and width and height of the image differ with the IVLCamVariables._Settings.CameraSettings.ImageWidth - IVLCamVariables._Settings.CameraSettings.roiX and IVLCamVariables._Settings.CameraSettings.ImageHeight - IVLCamVariables._Settings.CameraSettings.roiY.
                    {
                        maskCentreX = settings.maskCentreX;
                        maskCentreY = settings.maskCentreY;
                        LiveMaskImageWidth = settings.LiveMaskWidth;
                        LiveMaskImageHeight = settings.LiveMaskHeight;
                        Bitmap tempBm = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                        Graphics g = Graphics.FromImage(tempBm);
                        g.FillEllipse(Brushes.White, new Rectangle(settings.maskCentreX - settings.LiveMaskWidth / 2, settings.maskCentreY - settings.LiveMaskHeight / 2, settings.LiveMaskWidth, settings.LiveMaskHeight));
                        g.Dispose();
                        LiveMaskBitmap = new Image<Bgr, byte>(tempBm);
                        //CreateMask(LiveMaskBitmap[1].Ptr, true);
                        tempBm.Dispose();
                        //LiveMaskBitmap.Dispose();
                    }
                }

                if (CaptureMaskBitmap == null || width != (IVLCamVariables._Settings.CameraSettings.ImageWidth - 2 * IVLCamVariables._Settings.CameraSettings.roiX) || height != (IVLCamVariables._Settings.CameraSettings.ImageHeight - IVLCamVariables._Settings.CameraSettings.roiY) || maskCentreX != settings.maskCentreX || maskCentreY != settings.maskCentreY || CaputureMaskImageHeight != settings.maskHeight || CaputureMaskImageWidth != settings.maskWidth)//Condition has been added in  case of the scenario where the maskbitmap is not equal to null and width and height of the image differ with the IVLCamVariables._Settings.CameraSettings.ImageWidth - IVLCamVariables._Settings.CameraSettings.roiX and IVLCamVariables._Settings.CameraSettings.ImageHeight - IVLCamVariables._Settings.CameraSettings.roiY.
                {
                    maskCentreX = settings.maskCentreX;
                    maskCentreY = settings.maskCentreY;
                    CaputureMaskImageWidth = settings.maskWidth;
                    CaputureMaskImageHeight = settings.maskHeight;
                    Bitmap tempBm = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                    Graphics g = Graphics.FromImage(tempBm);
                    g.FillEllipse(Brushes.White, new Rectangle(settings.maskCentreX - settings.maskWidth / 2, settings.maskCentreY - settings.maskHeight / 2, settings.maskWidth, settings.maskHeight));
                    g.Dispose();

                    CaptureMaskBitmap = new Image<Bgr, byte>(tempBm);
                    //CreateMask(CaptureMaskBitmap[1].Ptr, false);
                    tempBm.Dispose();
                    // CaptureMaskBitmap.Dispose();
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log); 
            }
        }

        Image<Bgr, byte> inp;

        public void ApplyMask(ref Bitmap bm, MaskSettings settings,bool isColor)
        {
            try
            {
                IVLCamVariables._Settings.CameraSettings.roiX = 0;
                GenerateMask(bm.Width, bm.Height, settings); int bmWidth = bm.Width; int bmHeight = bm.Height;

                //BitmapData bdata = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                if (!IVLCamVariables.isLive)
                {
                    // ImageProc_ApplyMask(bdata.Scan0, false, bmWidth, bmHeight);
                    if (isColor)
                    {
                        Image<Bgr, byte> inp = new Image<Bgr, byte>(bm);
                        inp._And(CaptureMaskBitmap);
                        bm = inp.ToBitmap();
                    }
                    else
                    {
                        redChannelImage._And(CaptureMaskBitmap[0]);
                    }
                }
                else if (settings.isApplyLiveMask)
                {
                    Image<Bgr, byte> inp = new Image<Bgr, byte>(bm);
                    inp._And(LiveMaskBitmap);
                    bm = inp.ToBitmap();
                    //IVLCamVariables.TimeTaken = new System.Diagnostics.Stopwatch();
                    //IVLCamVariables.TimeTaken.Start();
                    //Image<Bgr, byte> inp = new Image<Bgr, byte>(bm);
                    // bm = inp.Copy(CaptureMaskBitmap[1]).ToBitmap();
                    ////bm = inp.ToBitmap();
                    //IVLCamVariables.TimeTaken.Stop();
                    //System.Diagnostics.Trace.WriteLine(IVLCamVariables.TimeTaken.ElapsedMilliseconds);
                   //  ImageProc_ApplyMask(bdata.Scan0, true, bmWidth, bmHeight);
                    
                    //Console.WriteLine(IVLCamVariables.TimeTaken.ElapsedMilliseconds);
                }
               // bm.UnlockBits(bdata);
               // bm.Save("save.png");
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log); 
            }


        }

        public void ApplyLogo(ref Bitmap bm, string nameOfTheImage, Color logoColour, LeftRightPosition Left)//string and color variables added as parameters to handle change of logo colour when mask colour is changed. By Ashutosh 29-08-2017.
        {
            try
            {
                Graphics g = Graphics.FromImage(bm);
                Point p = new Point(0, 0);
                string side = "";
                SolidBrush brush = new SolidBrush(Color.White);//colour of logo background is white by default.By Ashutosh 29-08-2017.
                SolidBrush brushForString = new SolidBrush(Color.White);//colour of string is white by default.By Ashutosh 29-08-2017.
                if (!string.IsNullOrEmpty(nameOfTheImage))//checks if nameOfTheImage is empty , proceeds if it is not empty.
                {
                    string[] positionOfTheImage = nameOfTheImage.Split(new char[0]);//seperates name of the image .
                    //if (positionOfTheImage[0].Contains( "OS"))//checks if it contains OS
                    //{
                    //    IVLCamVariables.leftRightPos = LeftRightPosition.Left;// if it contains then image is considered as left side.

                    //}
                    //else
                    //    IVLCamVariables.leftRightPos = LeftRightPosition.Right;//else it is right(OD)

                }
                Image<Bgr, byte> logo = null;
                string logoForMaskFilePath = @"ImageResources\LogoImageResources\logo1.jpg";
                if (File.Exists(logoForMaskFilePath))
                    logo = new Image<Bgr, byte>(logoForMaskFilePath);
                
                if (logo != null)
                {
                    if (logoColour != Color.Black)//checks if logo colour is not black
                    {
                        logo =   logo.ThresholdBinary(new Bgr(1,1,1),new Bgr(Color.White));//theresholding applied because logo1 image is not uniformly black and white, it has gray pixels.
                        logo._Not();//inversion of logo._Not() function performs the bit-wise inversion inplace.By Ashutosh 29-08-2017.
                        brushForString = new SolidBrush(Color.Black);//brushForString is white by defualt it is changed to black.
                        brush = new SolidBrush(logoColour);//brush colour will be that of colour set by user.
                        Bitmap maskBm = new Bitmap(logo.Width, logo.Height);//maskBm is a mask whose colour is that set by user. its dimensions are same as that of logo.
                        Graphics fillColour = Graphics.FromImage(maskBm);//graphics used to fill the mask.
                        fillColour.FillRectangle(brush, new Rectangle(0, 0, maskBm.Width, maskBm.Height));//fills the rectangle of maskbm with colour set by user.
                        Image<Bgr, byte> invertlogo = new Image<Bgr, byte>(maskBm);//input image bm given to object invertlogo.

                        Image<Bgr, byte> resultlogo = invertlogo.CopyBlank();//resultlogo will have same dimensions as invertlogo but is blank.
                        Image<Gray, byte> logoMask = logo.Convert<Gray, byte>();// logoMask object to which logo converted to gray from bgr is given.
                        invertlogo.Copy(resultlogo, logoMask);//resultlogo is the destination. mask is applied on the input image to provide desired result
                        logo = resultlogo.Copy();//copied to logo.

                        resultlogo.Dispose();
                        maskBm.Dispose();
                        logoMask.Dispose();
                        invertlogo.Dispose();

                        
                    }
                    if (bm.Width > 2048)
                        logo = logo.Resize(1, Emgu.CV.CvEnum.Inter.Cubic);
                    //if(IVLCamVariables.leftRightPos == LeftRightPosition.Left)
                    if (Left == LeftRightPosition.Left)//this has been added to check the eye side to draw the logo.
                    {
                        //logo._Flip(Emgu.CV.CvEnum.FLIP.HORIZONTAL);
                        if (bm.Width > 2048)
                            p = new Point((int)(0.05 * bm.Width), 0);
                        else
                            p = new Point(0, 0);
                        side = "OS (LE)";
                    }
                    else
                    {
                        if (bm.Width > 2048)
                            p = new Point(bm.Width - 400, 0);
                        else
                            p = new Point(bm.Width - 200, 0);
                        side = "OD (RE)";
                    }
                    g.DrawImage(logo.ToBitmap(), p);
                    Font f = null;
                    if (bm.Width <= 2048)//Added to set the logo size for Posterior_45
                    {
                        f = new Font(FontFamily.GenericSerif, 40, FontStyle.Bold, GraphicsUnit.Pixel);
                        g.DrawString(side, f, brushForString, new PointF(p.X + 5, p.Y + 85));//brushForString is white for black mask and black for all other masks.By Ashutosh 29-08-2017.
                    }
                    else
                    {
                        f = new Font(FontFamily.GenericSerif, 60, FontStyle.Bold, GraphicsUnit.Pixel);
                        g.DrawString(side, f, brushForString, new PointF(p.X, p.Y + 120));//brushForString is white for black mask and black for all other masks.By Ashutosh 29-08-2017.
                    }
                    logo.Dispose();
                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log); 
            }
            
        }

        /// <summary>
        /// This method is to draw the patient details on the image while exporting
        /// </summary>
        /// <param name="bm"></param>
        /// <param name="s"></param>
        public void ApplyPatientDetails(ref Bitmap bm, string[] s)
        {
            try
            {
                Graphics g = Graphics.FromImage(bm);
                Point p = new Point(0, 0);
                SolidBrush brushForString = new SolidBrush(Color.DeepSkyBlue);
                Font f = null;
                if (bm.Width > 2048)
                {
                    p = new Point(0, bm.Height - 200);
                    for (int i = 0; i < s.Length; i++)
                    {
                        if (s[i].Length > 16)//if the text length exceeds 16 character to reduce the font size.
                            f = new Font(FontFamily.GenericSerif, 35, FontStyle.Bold, GraphicsUnit.Pixel);
                        else
                            f = new Font(FontFamily.GenericSerif, 40, FontStyle.Bold, GraphicsUnit.Pixel);
                        g.DrawString(s[i], f, brushForString, new PointF(p.X, p.Y+=50));
                        if (i == 2)
                        {
                            p.X = bm.Width - 300;
                            p.Y = bm.Height - 150;
                        }
                    }
                }
                else
                {
                    p = new Point(0, bm.Height - 150);
                    for (int i = 0; i < s.Length; i++)
                    {
                        if (s[i].Length > 16)
                            f = new Font(FontFamily.GenericSerif, 20, FontStyle.Bold, GraphicsUnit.Pixel);
                        else
                            f = new Font(FontFamily.GenericSerif, 25, FontStyle.Bold, GraphicsUnit.Pixel);
                        g.DrawString(s[i], f, brushForString, new PointF(p.X, p.Y += 40));
                        if (i == 2)
                        {
                            p.X = bm.Width - 200;
                            p.Y = bm.Height - 120;
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public void ApplyHotSpotCompensation(ref Bitmap bm, HotSpotSettings settings, bool isColor)
        {
            try
            {
                unsafe
                {

                    CalculateHotspotParams(ref bm, IVLCamVariables._Settings.PostProcessingSettings.maskSettings.maskCentreX, IVLCamVariables._Settings.PostProcessingSettings.maskSettings.maskCentreY, "");


                    //double entropyVal = hotSpotParams->r4Entropy[0] + hotSpotParams->r4STD[0];
                    //double entropyRatio = hotSpotParams->r4Entropy[2] / hotSpotParams->r4Entropy[1];
                    //double entropyCompareVal = Math.Round(entropyRatio, 1);
                    int rPeakPercentage = settings.ShadowRedPeakPercentage;
                    int gPeakPercentage = settings.ShadowGreenPeakPercentage;
                    int bPeakPercentage = settings.ShadowBluePeakPercentage;

                        if (hotSpotParams->r3Brighntess[2] > 60 && hotSpotParams->r3Brighntess[2] < 220)
                        {
                            if (hotSpotParams->FocusVal[2] > 2500 && hotSpotParams->r3Entropy[1] > 4 && hotSpotParams->r3STD[1] < 50 && hotSpotParams->r3Contrast[1] > 0.2)
                            {
                                //ResultCategorizationWriter.WriteLine(item.Name + "," + "Good Image," + postProcessing.hotSpotParams->r3Brighntess[2] + "," + postProcessing.hotSpotParams->FocusVal[2] + "," + postProcessing.hotSpotParams->r3STD[1] + "," + postProcessing.hotSpotParams->r3Contrast[1] + "," + postProcessing.hotSpotParams->r3Entropy[1]);
                                //colorBm.Save(goodImgDirInf + Path.DirectorySeparatorChar + item.Name);

                            }
                            else
                            {
                                rPeakPercentage = (int)((double)settings.ShadowRedPeakPercentage * .5);
                                gPeakPercentage = (int)((double)settings.ShadowGreenPeakPercentage * .5);
                                bPeakPercentage = (int)((double)settings.ShadowBluePeakPercentage * .5);

                            }
                        }
                        else
                        {
                            rPeakPercentage = (int)((double)settings.ShadowRedPeakPercentage * .5);
                            gPeakPercentage = (int)((double)settings.ShadowGreenPeakPercentage * .5);
                            bPeakPercentage = (int)((double)settings.ShadowBluePeakPercentage * .5);

                        }
                        if (isColor)
                        {
                            BitmapData bdata = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadWrite, bm.PixelFormat);
                            ImageProc_HotspotCompensation(bdata.Scan0,IntPtr.Zero, IVLCamVariables._Settings.PostProcessingSettings.maskSettings.maskCentreX, IVLCamVariables._Settings.PostProcessingSettings.maskSettings.maskCentreY, bm.Width, bm.Height, settings.HotSpotRedPeak,
                                settings.HotSpotGreenPeak,
                                settings.HotSpotBluePeak,
                                settings.HotSpotRedRadius,
                                settings.HotSpotGreenRadius,
                                settings.HotSpotBlueRadius, isColor);


                            ImageProc_ShadowCompensation(bdata.Scan0,IntPtr.Zero, IVLCamVariables._Settings.PostProcessingSettings.maskSettings.maskCentreX, IVLCamVariables._Settings.PostProcessingSettings.maskSettings.maskCentreY, bm.Width, bm.Height, settings.hotSpotRad1,
                                   settings.hotSpotRad2, settings.radSpot1, settings.radSpot2,
                                    rPeakPercentage, gPeakPercentage, bPeakPercentage, isColor);
                            bm.UnlockBits(bdata);
                        }
                        else
                        {
                            inp = new Image<Bgr, byte>(bm);
                            ImageProc_HotspotCompensation(IntPtr.Zero,inp[1].Ptr, IVLCamVariables._Settings.PostProcessingSettings.maskSettings.maskCentreX, IVLCamVariables._Settings.PostProcessingSettings.maskSettings.maskCentreY, bm.Width, bm.Height, settings.HotSpotRedPeak,
                               settings.HotSpotGreenPeak,
                               settings.HotSpotBluePeak,
                               settings.HotSpotRedRadius,
                               settings.HotSpotGreenRadius,
                               settings.HotSpotBlueRadius, isColor);


                            ImageProc_ShadowCompensation(IntPtr.Zero,inp[1].Ptr, IVLCamVariables._Settings.PostProcessingSettings.maskSettings.maskCentreX, IVLCamVariables._Settings.PostProcessingSettings.maskSettings.maskCentreY, bm.Width, bm.Height, settings.hotSpotRad1,
                                   settings.hotSpotRad2, settings.radSpot1, settings.radSpot2,
                                    rPeakPercentage, gPeakPercentage, bPeakPercentage, isColor);
                            bm = inp.ToBitmap();
                        }
                        GC.Collect();

                }
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log); 
            }
        }

        public static void unsharpMask(ref Bitmap bm)
        {
            try
            {
                Image<Bgr, byte> im = new Image<Bgr, byte>(bm);
                Image<Bgr, float> im1 = im.Convert<Bgr, float>();
                // Matrix<float> sharpenKernel = new Matrix<float>(new float[3, 3] { { 0f, -1 / 5f, 0f }, { -1 / 5f, 1f, -1 / 5f }, { 0f, -1 / 5f, 0f } });
                Matrix<float> sharpenKernel = new Matrix<float>(new float[3, 3] { { -1f, -1f, -1f }, { -1f, 8f, -1f }, { -1f, -1f, -1f } });
                // Matrix<float> sharpenKernel = new Matrix<float>(new float[3, 3] { { -1 / 16f, -2 / 16f, -1 / 16f }, { -2 / 16f, 12 / 16f, -2 / 16f }, { -1 / 16f, -2 / 16f, -1 / 16f } });
                //Matrix<float> sharpenKernel = new Matrix<float>(new float[3, 3] { { -1f, -1f, -1f }, { -1f, 9f, -1f }, { -1f, -1f, -1f } });
                //ConvolutionKernelF c = new ConvolutionKernelF(sharpenKernel);
                //GCHandle pinnedArray1 = GCHandle.Alloc(sharpenKernel, GCHandleType.Pinned);
                // IntPtr pointer1 = pinnedArray1.AddrOfPinnedObject();
                Image<Bgr, float> imfiltered = im1.Copy();
                //sharpenKernel._Mul(1 / 16);
                // ImageViewer img = new ImageViewer(im1);
                // img.Show();
                CvInvoke.Filter2D(im1[0], imfiltered[0], sharpenKernel, new Point(-1, -1));
                CvInvoke.Filter2D(im1[1], imfiltered[1], sharpenKernel, new Point(-1, -1));
                CvInvoke.Filter2D(im1[2], imfiltered[2], sharpenKernel, new Point(-1, -1));
                //imfiltered[0] = im1[0].Copy();
                //imfiltered[2] = im1[2].Copy();

                double[] minVal = new double[3];
                double[] maxVal = new double[3];
                Point[] minLoc = new Point[3];
                Point[] maxLoc = new Point[3];
                imfiltered.MinMax(out minVal, out maxVal, out minLoc, out maxLoc);
                imfiltered[0] = imfiltered[0].Sub(new Gray(minVal[0]));
                imfiltered[1] = imfiltered[1].Sub(new Gray(minVal[1]));
                imfiltered[2] = imfiltered[2].Sub(new Gray(minVal[2]));

                imfiltered[0] = imfiltered[0].Mul(255.0 / maxVal[0]);
                imfiltered[1] = imfiltered[1].Mul(255.0 / maxVal[1]);
                imfiltered[2] = imfiltered[2].Mul(255.0 / maxVal[2]);
                Image<Bgr, float> sharpenedImg = new Image<Bgr, float>(im1.Width, im1.Height);
                sharpenedImg = im1.Add(imfiltered);
                sharpenedImg.MinMax(out minVal, out maxVal, out minLoc, out maxLoc);
                sharpenedImg[0] = sharpenedImg[0].Sub(new Gray(minVal[0]));
                sharpenedImg[1] = sharpenedImg[1].Sub(new Gray(minVal[1]));
                sharpenedImg[2] = sharpenedImg[2].Sub(new Gray(minVal[2]));

                sharpenedImg[0] = sharpenedImg[0].Mul(255.0 / maxVal[0]);
                sharpenedImg[1] = sharpenedImg[1].Mul(255.0 / maxVal[1]);
                sharpenedImg[2] = sharpenedImg[2].Mul(255.0 / maxVal[2]);
                //im = sharpenedImg.Convert<Bgr, byte>();
                im = imfiltered.Convert<Bgr, byte>();
                //Bitmap  bm1 = im.ToBitmap();

                // im = new Image<Bgr, byte>(bm);
                //im[1] = im[1].SmoothMedian(3);
                //Image<Gray, byte> addImg = im[1].SmoothGaussian(5);
                //im[1] = im[1].AddWeighted(addImg, 4.5, -3.5, 0);
                bm = im.ToBitmap();
                //bm.Save(finf.DirectoryName + Path.DirectorySeparatorChar + strArr[0] + "_sharpeningKernel.png");
                im.Dispose();
                im1.Dispose();
                imfiltered.Dispose();
                sharpenedImg.Dispose();
                //// return im.ToBitmap();

            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log); 
            }
            
        }
        public void unsharpMask_Channel(ref Bitmap bm, UnsharpMaskSettings settings , bool isColor)
        {

            try
            {
                if (isColor)
                {
                    BitmapData bdata = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadWrite, bm.PixelFormat);
                    ImageProc_ApplyUnsharpMask(bdata.Scan0,IntPtr.Zero, settings.unsharpThresh, settings.unsharpAmount, settings.radius, settings.medFilterValue, isColor);
                    bm.UnlockBits(bdata);
                }
                else
                    ImageProc_ApplyUnsharpMask(IntPtr.Zero, redChannelImage.Ptr, settings.unsharpThresh, settings.unsharpAmount, settings.radius, settings.medFilterValue, isColor);
            }
            catch (Exception ex)
            {
                CameraLogger.WriteException(ex, Exception_Log);
                //                 CameraLogger.WriteException(ex, Exception_Log); 
            }

        }
        public void ApplyGammaCorrection(ref Bitmap bm, GammaCorrectionSettings settings)
        {
            try
            {
                Image<Bgr, byte> inp = new Image<Bgr, byte>(bm);
                inp._GammaCorrect(settings.GammaCorrectionValue);
                bm = inp.ToBitmap();
                inp.Dispose();
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log); 
            }
            

        }
        public void GetMonoChromeImage(ref Bitmap colorBm)
        {
            try
            {
                BitmapData bdata = colorBm.LockBits(new Rectangle(0, 0, colorBm.Width, colorBm.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                ImageProc_GetMonoChromeImage(bdata.Scan0);
                colorBm.UnlockBits(bdata);
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log); 
            }
        }

        public void ShiftImage(ref Bitmap bm, ImageShiftSettings settings)
        {
            try
            {
                // Image<Hsv, byte> inp = new Image<Hsv, byte>(bm);
                Image<Bgr, byte> inp = new Image<Bgr, byte>(bm);
                int factorX = settings.ShiftX;
                int factorY = settings.ShiftY;
                // take central portion of the image
                // Channel Zero is Blue. 
                // cannot set ROI independently
                inp.ROI = new Rectangle(factorX, factorY, inp.Width - (2 * factorX), inp.Height - (2 * factorY));
                Image<Gray, byte> tempImg = new Image<Gray, byte>(inp.Width, inp.Height);
                CvInvoke.cvCopy(inp[0], tempImg, IntPtr.Zero);

                // stretch the blue image by the input factors
                inp.ROI = new Rectangle();
                inp[0] = tempImg.Resize(inp.Width, inp.Height, Emgu.CV.CvEnum.Inter.Cubic);


                // shrink red channel image
                tempImg = inp[2].Resize(inp.Width - 2 * factorX, inp.Height - 2 * factorY, Emgu.CV.CvEnum.Inter.Cubic);

                Image<Gray, byte> tempImg2 = new Image<Gray, byte>(inp.Width, inp.Height);
                tempImg2.ROI = new Rectangle(factorX, factorY, inp.Width - 2 * factorX, inp.Height - 2 * factorY);
                CvInvoke.cvCopy(tempImg, tempImg2, IntPtr.Zero);

                tempImg2.ROI = new Rectangle();
                Mat[] inpArr = new Mat[3];
                inpArr[0] = inp[0].Mat;
                inpArr[1] = inp[1].Mat;
                inpArr[2] = tempImg2.Mat;
                Emgu.CV.Util.VectorOfMat vMat = new Emgu.CV.Util.VectorOfMat(inpArr);
                CvInvoke.Merge(vMat, inp);
                //inp[1].ROI = new Rectangle(0, 0, inp[1].Width, factorY);
                //inp[1].SetZero();
                //inp[1].ROI = new Rectangle();

                //inp[1].ROI = new Rectangle(0, inp[1].Height - factorY, inp[1].Width, factorY);
                //inp[1].SetZero();
                //inp[1].ROI = new Rectangle();
                // resize image to the smaller size after shifting,
                // Did not work
                //inp.ROI = new Rectangle(factorX, factorY, inp.Width - (2 * factorX), inp.Height - (2 * factorY));

                bm = inp.ToBitmap();
                tempImg.Dispose();
                tempImg2.Dispose();
                inp.Dispose();
                GC.Collect();
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log); 
            }

        }
        public void ApplyCameraShiftCorrection(ref Bitmap bm)
        {
            try
            {
                Image<Bgr, byte> inp = new Image<Bgr, byte>(bm);
                Image<Bgr, byte> inp1 = inp.CopyBlank();
                inp.ROI = new Rectangle(0, CameraShiftY1, inp.Width, inp.Height - CameraShiftHeight);
                inp1.ROI = new Rectangle(0, CameraShiftY2, inp1.Width, inp1.Height - CameraShiftHeight);
                CvInvoke.cvCopy(inp, inp1, IntPtr.Zero);
                inp.ROI = new Rectangle();
                inp1.ROI = new Rectangle();
                bm = inp1.ToBitmap();
                inp.Dispose();
                inp1.Dispose();
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log); 
            }
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bm">bm is the input bitmap</param>
        /// <param name="centreX"> centreX x co-ordinate is the hotspot centre for reference</param>
        /// <param name="centreY">centreY y co-ordinate is the hotspot centre for reference</param>
        /// <param name="percentFactor"> percentFactor is the amplification factor for the edge of the image w.r.t the centre of image</param>
        /// <param name="radius">radius is the radius of the image circle where amplification will be applied</param>
        public void ApplyVignattingCompensationPostProcessing(ref Bitmap bm, int centreX, int centreY, float percentFactor, int radius)
        {
            try
            {
                Image<Rgb, byte> inp1 = new Image<Rgb, byte>(bm);
                ImageProc_ApplyParabolicCompensationPostProcessing(inp1.Ptr);

                //ApplyParabolicCompensationPostProcessing_New(,inp1.Width,inp1.Height);
                bm = inp1.ToBitmap();
                inp1.Dispose();
                GC.Collect();
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log); 
            }
            
        }
        public void ApplyVignattingCompensationLive(ref Image<Gray, byte> destImg, int centreX, int centreY, float percentFactor, int radius)
        {
            try
            {
                // Image<Rgb, byte> srcImg = new Image<Rgb, byte>(bm);

                ImageProc_ApplyParabolicCompensationLive(destImg.Ptr, destImg.Ptr);
                // bm = inp1.ToBitmap();
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log); 
            }
            
        }
        public void ApplyVignattingHotspotCompensationLive(ref Image<Gray, byte> destImg, int centreX, int centreY, float percentFactor, int radius, float hotspotFactor, int hotspotRadius)
        {
            try
            {
                // Image<Rgb, byte> srcImg = new Image<Rgb, byte>(bm);

                ImageProc_ApplyParabolicHotspotCompensationLive(destImg.Ptr, destImg.Ptr);
                // bm = inp1.ToBitmap();
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log); 
            }
            
        }

        public void Applycolorcorrection(ref Bitmap bm, CCSettings settings , bool isColor)
        {
            try
            {
                colorMatrix[0, 0] = settings.rrVal;
                colorMatrix[0, 1] = settings.rgVal;
                colorMatrix[0, 2] = settings.rbVal;
                colorMatrix[1, 0] = settings.grVal;
                colorMatrix[1, 1] = settings.ggVal;
                colorMatrix[1, 2] = settings.gbVal;
                colorMatrix[2, 0] = settings.brVal;
                colorMatrix[2, 1] = settings.bgVal;
                colorMatrix[2, 2] = settings.bbVal;
                
                    Image<Rgb, byte> baBayerBmp = new Image<Rgb, byte>(bm);

                    #region cool white settings
                    // Good 1 cool white
                    //float[,] ColorMatrix = {{1.25f,	0.2f,	0f},
                    //                            {-0.15f,	0.8f,	0f},
                    //                                {0f	,0f,	0.9f}};

                    //float[,] ColorMatrix = {{1.35f,	0.1f,	0f},
                    //                            {-0.05f,	0.7f,	0f},
                    //                                {0f	,0f,	0.9f}};
                    #endregion

                    Matrix<float> colorMat = new Matrix<float>(colorMatrix);

                CvInvoke.Transform(baBayerBmp.Mat, baBayerBmp.Mat, colorMat.Mat);
                    bm = baBayerBmp.ToBitmap();
                    //else
                    //    redChannelImage = baBayerBmp[1].Copy();

                    baBayerBmp.Dispose();
                    colorMat.Dispose();
                    GC.Collect();


            }

            catch (Exception ex)
            {
                CameraLogger.WriteException(ex, Exception_Log);
                //                 CameraLogger.WriteException(ex, Exception_Log); 
            }
            
        }
        public void ApplycolorcorrectionStatic(ref Bitmap bm)
        {
            try
            {
                Image<Rgb, byte> baBayerBmp = new Image<Rgb, byte>(bm);

                #region cool white settings
                // Good 1 cool white
                //float[,] ColorMatrix = {{1.25f,	0.2f,	0f},
                //                            {-0.15f,	0.8f,	0f},
                //                                {0f	,0f,	0.9f}};

                //float[,] ColorMatrix = {{1.35f,	0.1f,	0f},
                //                            {-0.05f,	0.7f,	0f},
                //                                {0f	,0f,	0.9f}};
                #endregion
                Matrix<float> colorMat = new Matrix<float>(colorMatrix);
                CvInvoke.Transform(baBayerBmp.Mat, baBayerBmp.Mat, colorMat.Mat);
                bm = baBayerBmp.ToBitmap();
                baBayerBmp.Dispose();
                colorMat.Dispose();
                GC.Collect();
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);  
            }
            
        }




        public void ApplyPostProcessing(ref Bitmap captureBm, bool isColor)
        {
            try
            {
                System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
                string str ="";
                //if (!isColor)
                //{
                //    inp = new Image<Bgr, byte>(captureBm);
                //    redChannelImage = inp[1].Copy();
                //}
                try
                {
                    for (int i = 0; i < PP_OrderList.Count; i++)
                    {
                        //PostProcessingStep ppStep = (PostProcessingStep) Enum.Parse(typeof(PostProcessingStep), orderList[i].ToString());

                        switch (PP_OrderList[i])
                        {

                            case PostProcessingStep.ShiftImage:
                                {
                                    {
                                        watch = new System.Diagnostics.Stopwatch();
                                        watch.Start();

                                        if (IVLCamVariables._Settings.PostProcessingSettings.imageShiftSettings.isApplyShiftCorrection)
                                            ShiftImage(ref captureBm, IVLCamVariables._Settings.PostProcessingSettings.imageShiftSettings);
                                        watch.Stop();
                                         str += string.Format("SHift Time = {0}",watch.ElapsedMilliseconds) +Environment.NewLine;
                                       

                                    }
                                    break;
                                }
                            case PostProcessingStep.HSVBoost:
                                {
                                    watch = new System.Diagnostics.Stopwatch();

                                    watch.Start();
                                    {
                                        if (IVLCamVariables._Settings.PostProcessingSettings.isApplyHSVBoost)
                                            ApplyHSVBoost(ref captureBm, hsvBoostVal);
                                        watch.Stop();
                                        str += string.Format("Hsv Boost Time = {0}", watch.ElapsedMilliseconds) + Environment.NewLine;
                                    }
                                    break;
                                }
                            case PostProcessingStep.LUT:
                                {
                                    watch = new System.Diagnostics.Stopwatch();

                                    watch.Start();
                                    if (IVLCamVariables._Settings.PostProcessingSettings.lutSettings.isApplyLUT)
                                        ApplyLut(ref captureBm, true ,isColor);
                                    watch.Stop();
                                    str += string.Format("LUT Time = {0}", watch.ElapsedMilliseconds) + Environment.NewLine;
                                    break;
                                }
                            case PostProcessingStep.Clahe:
                                {
                                    watch = new System.Diagnostics.Stopwatch();
                                    watch.Start();
                                    if (IVLCamVariables._Settings.PostProcessingSettings.claheSettings.isApplyClahe)
                                    {
                                        ApplyClahe(ref captureBm, IVLCamVariables._Settings.PostProcessingSettings.claheSettings ,isColor);

                                        watch.Stop();
                                        str += string.Format("Clahe Time = {0}", watch.ElapsedMilliseconds) + Environment.NewLine;

                                    }
                                    break;
                                }
                            case PostProcessingStep.HotSpotCorrection:
                                {        // hotspot to be done before applying vignetting compensation
                                    watch = new System.Diagnostics.Stopwatch();
                                    watch.Start();
                                    if (IVLCamVariables._Settings.PostProcessingSettings.hotspotSettings.isEnableHS)
                                        ApplyHotSpotCompensation(ref captureBm, IVLCamVariables._Settings.PostProcessingSettings.hotspotSettings,isColor); // changed parameter of method in order to ensure proper hotspot correction is applied by sriram on september 9th 2015
                                    watch.Stop();
                                    str += string.Format("HS Time = {0}", watch.ElapsedMilliseconds) + Environment.NewLine;
                                    break;
                                }
                            case PostProcessingStep.UnsharpMask:
                                {
                                    watch = new System.Diagnostics.Stopwatch();
                                    watch.Start();
                                    if (IVLCamVariables._Settings.PostProcessingSettings.unsharpMaskSettings.isApplyUnsharpMask)
                                    {
                                        unsharpMask_Channel(ref captureBm, IVLCamVariables._Settings.PostProcessingSettings.unsharpMaskSettings, isColor);
                                        watch.Stop();
                                        str += string.Format("Unsharp Time = {0}", watch.ElapsedMilliseconds) + Environment.NewLine;
                                    }
                                    break;
                                }
                            case PostProcessingStep.ColorCorrection:
                                {
                                    watch = new System.Diagnostics.Stopwatch();
                                    watch.Start();
                                    if (IVLCamVariables._Settings.PostProcessingSettings.ccSettings.isApplyColorCorrection)
                                    {
                                        // ColorCorrection(inp.Ptr, 4, 0.55, 1, 1, 0);
                                        Applycolorcorrection(ref captureBm, IVLCamVariables._Settings.PostProcessingSettings.ccSettings,isColor);
                                        watch.Stop();
                                        str += string.Format("CC Time = {0}", watch.ElapsedMilliseconds) + Environment.NewLine;
                                    }
                                    break;
                                }
                            case PostProcessingStep.BrightnessContrast:
                                {
                                    watch = new System.Diagnostics.Stopwatch();
                                    watch.Start();
                                    if (IVLCamVariables._Settings.PostProcessingSettings.brightnessContrastSettings.isApplyBrightness ||
                                    IVLCamVariables._Settings.PostProcessingSettings.brightnessContrastSettings.isApplyContrast)
                                    {

                                        Bitmap bm = captureBm.Clone() as Bitmap;
                                        AdjustBrightness(bm, IVLCamVariables._Settings.PostProcessingSettings.brightnessContrastSettings.brightnessValue,
                                            IVLCamVariables._Settings.PostProcessingSettings.brightnessContrastSettings.contrastValue, ref captureBm);
                                        bm.Dispose();
                                        watch.Stop();
                                        str += string.Format("BC Time = {0}", watch.ElapsedMilliseconds) + Environment.NewLine;
                                    }
                                    break;
                                }
                            case PostProcessingStep.Mask:
                                {
                                    watch = new System.Diagnostics.Stopwatch();
                                    watch.Start();
                                    if (IVLCamVariables._Settings.PostProcessingSettings.maskSettings.isApplyMask)
                                    {

                                        ApplyMask(ref captureBm, IVLCamVariables._Settings.PostProcessingSettings.maskSettings,isColor);
                                        watch.Stop();
                                        str += string.Format("Mask Time = {0}", watch.ElapsedMilliseconds) + Environment.NewLine;
                                    }
                                    break;
                                }
                            case PostProcessingStep.Gamma:
                                {
                                    watch = new System.Diagnostics.Stopwatch();
                                    watch.Start();
                                    if (IVLCamVariables._Settings.PostProcessingSettings.gammaCorrectionSettings.ApplyGammaCorrection)
                                    {

                                        ApplyGammaCorrection(ref captureBm, IVLCamVariables._Settings.PostProcessingSettings.gammaCorrectionSettings);
                                        watch.Stop();
                                        str += string.Format("Gamma Time = {0}", watch.ElapsedMilliseconds) + Environment.NewLine;
                                    }
                                    break;
                                }
                        }
                    }
                        //if (!isColor)
                        //{
                        //    inp[1] = redChannelImage.Copy();
                        //    captureBm = inp.ToBitmap();
                        //    inp.Dispose();
                        //    redChannelImage.Dispose();
                        //}
                        if (IVLCamVariables._Settings.PostProcessingSettings.maskSettings.isApplyLogo)
                        {
                            watch = new System.Diagnostics.Stopwatch();
                            watch.Start();
                            ApplyLogo(ref captureBm,"",Color.Black, IVLCamVariables.leftRightPos);
                            watch.Stop();
                            str += string.Format("Logo Time = {0}", watch.ElapsedMilliseconds) + Environment.NewLine;
                        }
                    logArg = new EventHandler.Args();
                    logArg["TimeStamp"] = DateTime.Now;
                    logArg["Msg"] = string.Format("Apply Post Processing {0}", str);
                    //logArg["callstack"] = Environment.StackTrace;
                    capture_log.Add(logArg);
                   //IVLCamVariables.logClass.CaptureLogList.Add(string.Format("Apply Post Processing {0}", str));
                
                }
                catch (Exception ex)
                {
                   CameraLogger.WriteException(ex, Exception_Log);
//                     CameraLogger.WriteException(ex, Exception_Log);  
                }
            }

            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);  
            }

        }
        public void AdjustBrightness(Bitmap Image, int BrightnessValue, int contrastValue, ref Bitmap brightnessContrastbm)
        {
            try
            {
                System.Drawing.Bitmap TempBitmap = Image;
                float FinalValue = (float)BrightnessValue / 255.0f;
                brightnessContrastbm = Image.Clone(new Rectangle(0, 0, Image.Width, Image.Height), PixelFormat.Format24bppRgb);

                float contrastVal = (float)((float)contrastValue * 0.1f + 1.0f);
                System.Drawing.Graphics NewGraphics = System.Drawing.Graphics.FromImage(brightnessContrastbm);
                NewGraphics.FillRectangle(Brushes.Black, new Rectangle(0, 0, Image.Width, Image.Height));
                float[][] FloatColorMatrix ={
                      new float[] {contrastVal, 0, 0, 0, 0},
                      new float[] {0, contrastVal, 0, 0, 0},
                      new float[] {0, 0, contrastVal, 0, 0},
                      new float[] {0, 0, 0, contrastVal, 0},
                      new float[] {FinalValue, FinalValue, FinalValue, 1, 1}
                 };

                System.Drawing.Imaging.ColorMatrix NewColorMatrix = new System.Drawing.Imaging.ColorMatrix(FloatColorMatrix);
                System.Drawing.Imaging.ImageAttributes Attributes = new System.Drawing.Imaging.ImageAttributes();
                Attributes.SetColorMatrix(NewColorMatrix);
                NewGraphics.DrawImage(TempBitmap, new System.Drawing.Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), 0, 0, TempBitmap.Width, TempBitmap.Height, System.Drawing.GraphicsUnit.Pixel, Attributes);
                Attributes.Dispose();
                NewGraphics.Dispose();
            }
            catch (Exception ex) 
            {
               CameraLogger.WriteException(ex, Exception_Log);
                // CameraLogger.WriteException(ex, Exception_Log);  
            }
        }
        public void ApplyClahe(ref Bitmap captureBm, ClaheSettings claheSettings , bool isColor)
        {
            try
            {
                double[] clipVals = new double[] { claheSettings.clipValB, claheSettings.clipValG, claheSettings.clipValR };
               Image<Bgr,byte> output = new Image<Bgr, byte>(captureBm);
                BitmapData bdata = captureBm.LockBits(new Rectangle(0, 0, captureBm.Width, captureBm.Height), ImageLockMode.ReadWrite, captureBm.PixelFormat);

                ApplyClaheCPlusPlus(bdata.Scan0,output.Mat.Ptr, clipVals, 3);

                captureBm.UnlockBits(bdata);
             captureBm =   output.ToBitmap();
             output.Dispose();
                if (isColor)
                {
                    //captureBm = inp.ToBitmap();
                    //BitmapData bdata = captureBm.LockBits(new Rectangle(0, 0, captureBm.Width, captureBm.Height), ImageLockMode.ReadWrite, captureBm.PixelFormat);

                    //ImageProc_ApplyClahe(bdata.Scan0,IntPtr.Zero, captureBm.Width, captureBm.Height,
                    //    claheSettings.clipValR,
                    //    claheSettings.clipValG,
                    //    claheSettings.clipValB, isColor);
                   // captureBm.UnlockBits(bdata);
                }
                else
                {
                   // redChannelImage = new Image<Bgr, byte>(captureBm)[1].Copy();
                    //redChannelImage = inp[1].Copy();
                   // IntPtr ptr = redChannelImage.Ptr;

                    //ApplyClaheCPlusPlus(redChannelImage.Ptr, redChannelImage.Mat.Ptr, clipVals, 1);

                    //.CLAHE(redChannelImage.Mat, claheSettings.clipValR, new Size(32, 32), redChannelImage.Mat);
                    //ImageProc_ApplyClahe(IntPtr.Zero, redChannelImage.Mat.Ptr, captureBm.Width, captureBm.Height,
                    //    claheSettings.clipValR,
                    //    claheSettings.clipValG,
                    //    claheSettings.clipValB, isColor);
                }
                //inp.Dispose();
            }
            catch (Exception ex)
            {
                CameraLogger.WriteException(ex, Exception_Log);
                //                 CameraLogger.WriteException(ex, Exception_Log);  
            }
            
        }
        public void Denoise(ref Bitmap bm, UnsharpMaskSettings unsharpSettings)
        {
            try
            {
                //Image<Bgr, byte> inp = new Image<Bgr, byte>(bm);
                BitmapData bdata = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadWrite, bm.PixelFormat);
                ImageProc_Denoise(bdata.Scan0, unsharpSettings.medFilterValue);
                bm.UnlockBits(bdata);
                // bm = inp.SmoothMedian(unsharpSettings.medFilterValue).ToBitmap();
                //inp.Dispose();
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);  
            }
            

        }
        public void ApplyTimeStamp(ref Bitmap bm, string time, Color fontColor)
        {
                SolidBrush s = new SolidBrush(fontColor);
                Graphics g = Graphics.FromImage(bm);
                g.DrawString(time, new Font(FontFamily.GenericSansSerif, 60f, FontStyle.Bold, GraphicsUnit.Pixel), s, new PointF(bm.Width - 200, bm.Height - 150));

        }
        public void BoostImage(ref Bitmap bm)
        {
            try
            {
              //  Image<Bgr, byte> inp = new Image<Bgr, byte>(bm);

                //
                //inp._Mul(IVLCamVariables._Settings.PostProcessingSettings.hsvValue);
                // inpDouble = inpDouble.Pow(IVLCamVariables._Settings.PostProcessingSettings.hsvBoostValue);
                //inp =   inp.Add(inp);
                //bm = inp.ToBitmap();
                //inp.Dispose();
                BitmapData bdata = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadWrite, bm.PixelFormat);
                ImageProc_BoostImage(bdata.Scan0, IVLCamVariables._Settings.PostProcessingSettings.hsvValue);
                bm.UnlockBits(bdata);

            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);  
            }
            
        }
        public void ApplyClaheMonoChrome(ref Bitmap bm)
        {
           
        }
        public void ApplyHSVBoost(ref Bitmap srcBm, float value)
        {
            try
            {
                Image<Bgr, byte> hsvImg = new Image<Bgr, byte>(srcBm);
                hsvImg._Mul(IVLCamVariables._Settings.PostProcessingSettings.hsvValue);

                //BitmapData bdata = srcBm.LockBits(new Rectangle(0, 0, srcBm.Width, srcBm.Height), ImageLockMode.ReadWrite, srcBm.PixelFormat);
                //ImageProc_ApplyHSVBOOSt(bdata.Scan0, hsvImg.Ptr, value, srcBm.Width, srcBm.Height);
                //srcBm.UnlockBits(bdata);
                srcBm = hsvImg.ToBitmap();
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);  
            }
        }
        public void ApplyLut(ref Bitmap srcbm, bool is8bit, bool isColor)
        {
            try
            {
                //if (isColor)
                {
                    BitmapData bdata = srcbm.LockBits(new Rectangle(0, 0, srcbm.Width, srcbm.Height), ImageLockMode.ReadWrite, srcbm.PixelFormat);

                    ImageProc_ApplyLUT(bdata.Scan0,IntPtr.Zero ,srcbm.Width, srcbm.Height, is8bit, isColor, IVLCamVariables._Settings.PostProcessingSettings.lutSettings.isChannelWiseLUT);

                    srcbm.UnlockBits(bdata);
                }
                //else 
                //if(!isColor)
                //{

                //    redChannelImage = new Image<Bgr, byte>(srcbm)[1].Copy();
                //    //ImageProc_ApplyLUT(IntPtr.Zero,redChannelImage.Ptr, redChannelImage.Width, redChannelImage.Height, is8bit, isColor);
                //}
            }
            catch (Exception ex)
            {
                CameraLogger.WriteException(ex, Exception_Log);
                //                 CameraLogger.WriteException(ex, Exception_Log);  
            }
            
        }
        public void LiteColorCorrection(ref Bitmap srcBm)
        {
            try
            {
                
                //Image<Bgr, byte> tempImg = new Image<Bgr, byte>(srcBm);
                //tempImg[0] = tempImg[0].Sub(new Gray(12));
                //tempImg[1] = tempImg[1].Add(new Gray(12));
                //tempImg[2] = tempImg[2].Sub(new Gray(22));
                //CvInvoke.cvSubS(tempImg[0].Ptr, new MCvScalar(12, 0, 0), tempImg[0].Ptr, IntPtr.Zero);
                //CvInvoke.cvAddS(tempImg[1].Ptr, new MCvScalar(12, 0, 0), tempImg[1].Ptr, IntPtr.Zero);
                //CvInvoke.cvSubS(tempImg[2].Ptr, new MCvScalar(22, 0, 0), tempImg[2].Ptr, IntPtr.Zero);
                //// float[,] ColorMatrix = {{-22,0f,0f},
                ////                         {0f,22f,0f},
                ////                             {0f,0f,-12f}};
                ////PostProcessing.ApplycolorcorrectionStatic(ref colorBm, ColorMatrix);
                //tempImg._GammaCorrect(.8);

                //tempImg = tempImg.Mul(1.12);
                //Bitmap bm = tempImg.ToBitmap();

                ////Camera_WinForm.AdjustBrightness(tempImg.ToBitmap(),0, 12, ref bm);
                //Image<Hsv, byte> tempImg1 = new Image<Hsv, byte>(bm);
                //CvInvoke.cvSubS(tempImg1[1].Ptr, new MCvScalar(100, 0, 0), tempImg1[1].Ptr, IntPtr.Zero);
                //// CvInvoke.cvAddS(tempImg[2].Ptr, new MCvScalar(100, 0, 0), tempImg[2].Ptr, IntPtr.Zero);

                //tempImg = tempImg1.Convert<Bgr, byte>();

                //srcBm = tempImg.ToBitmap();
                //tempImg.Dispose();
                //bm.Dispose();
                //tempImg1.Dispose();
            }
            catch (Exception ex)
            {
               CameraLogger.WriteException(ex, Exception_Log);
//                 CameraLogger.WriteException(ex, Exception_Log);  
            }
            
        }

        public void GetEdgeDetection(ref Bitmap bm ,double threshVal,double threshLinkVal)
        {
          Image<Bgr, byte> inp = new Image<Bgr, byte>(bm);
          Image<Gray,byte> edgeIm =  inp[2].Canny(threshVal, threshLinkVal);
          //inp[0] = edgeIm.Copy();
          //inp[1] = edgeIm.Copy();
          //inp[2] = edgeIm.Copy();
          bm = edgeIm.ToBitmap();
          inp.Dispose();
          edgeIm.Dispose();
        }
    }
}
