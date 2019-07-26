using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace INTUSOFT.Configuration.AdvanceSettings
{
  
   [Serializable]
   public class UISettings
   {
       public ViewImagingUI ViewImaging;
       public LiveImaging LiveImaging;
       public EmrUI EmrUI;
       public UISettings()
       {

           ViewImaging = new ViewImagingUI();

           LiveImaging = new LiveImaging();

           EmrUI = new EmrUI();
       }
   }

   [Serializable]
   public class ViewImagingUI
   {
       //public bool GenerateReportBtnVisible = true;
       //public bool GenerateAnnotationBtnVisible = true;
       //public bool GenerateCDRBtnVisible = false;
       //public bool ZoomFunctionVisble = true;
       //public bool BrightnessFunctionVisble = true;
       //public bool ContrastFunctionVisble = true;
       //public bool SaveFunctionVisble = true;
       //public bool SaveAsFunctionVisble = true;
       //public bool ExportFunctionVisble = true;
       //public bool RightLeftVisble = true;
       //public bool ShowFiltersVisble = true;
       //public bool RemovePostProcessingBtnVisible = true;
       //public bool PostProcessingLabelVisible = true;

       private static  IVLControlProperties GenerateReportBtnVisible = null;

       public IVLControlProperties _GenerateReportBtnVisible
       {
           get { return GenerateReportBtnVisible; }
           set { GenerateReportBtnVisible = value; }
       }
       private static  IVLControlProperties GenerateAnnotationBtnVisible = null;

       public IVLControlProperties _GenerateAnnotationBtnVisible
       {
           get { return GenerateAnnotationBtnVisible; }
           set { GenerateAnnotationBtnVisible = value; }
       }
       private static  IVLControlProperties GenerateCDRBtnVisible = null;

       public IVLControlProperties _GenerateCDRBtnVisible
       {
           get { return GenerateCDRBtnVisible; }
           set { GenerateCDRBtnVisible = value; }
       }
       private static  IVLControlProperties ZoomFunctionVisble = null;

       public IVLControlProperties _ZoomFunctionVisble
       {
           get { return ZoomFunctionVisble; }
           set { ZoomFunctionVisble = value; }
       }
       private static  IVLControlProperties BrightnessFunctionVisble = null;

       public IVLControlProperties _BrightnessFunctionVisble
       {
           get { return BrightnessFunctionVisble; }
           set { BrightnessFunctionVisble = value; }
       }
       private static  IVLControlProperties ContrastFunctionVisble = null;

       public IVLControlProperties _ContrastFunctionVisble
       {
           get { return ContrastFunctionVisble; }
           set { ContrastFunctionVisble = value; }
       }
       private static  IVLControlProperties SaveFunctionVisble = null;

       public IVLControlProperties _SaveFunctionVisble
       {
           get { return SaveFunctionVisble; }
           set { SaveFunctionVisble = value; }
       }
       private static  IVLControlProperties SaveAsFunctionVisble = null;

       public IVLControlProperties _SaveAsFunctionVisble
       {
           get { return SaveAsFunctionVisble; }
           set { SaveAsFunctionVisble = value; }
       }
       private static  IVLControlProperties ExportFunctionVisble = null;

       public IVLControlProperties _ExportFunctionVisble
       {
           get { return ExportFunctionVisble; }
           set { ExportFunctionVisble = value; }
       }
       private static  IVLControlProperties RightLeftVisble = null;


       public IVLControlProperties _RightLeftVisble
       {
           get { return RightLeftVisble; }
           set { RightLeftVisble = value; }
       }
       private static  IVLControlProperties ShowFiltersVisble = null;

       public IVLControlProperties _ShowFiltersVisble
       {
           get { return ShowFiltersVisble; }
           set { ShowFiltersVisble = value; }
       }
      
       private static  IVLControlProperties RemovePostProcessingBtnVisible = null;

       public IVLControlProperties _RemovePostProcessingBtnVisible
       {
           get { return RemovePostProcessingBtnVisible; }
           set { RemovePostProcessingBtnVisible = value; }
       }

    
       private static  IVLControlProperties PostProcessingLabelVisible = null;

       public IVLControlProperties _PostProcessingLabelVisible
       {
           get { return PostProcessingLabelVisible; }
           set { PostProcessingLabelVisible = value; }
       }

       private static  IVLControlProperties ReportWindowClose = null;
       public IVLControlProperties _ReportWindowClose
       {
           get { return ReportWindowClose; }
           set { ReportWindowClose = value; }
       }
       public ViewImagingUI()
       {
           _GenerateReportBtnVisible = new IVLControlProperties();
           _GenerateReportBtnVisible.name = "GenerateReportBtnVisible";
           _GenerateReportBtnVisible.val = true.ToString();
           _GenerateReportBtnVisible.type = "bool";
           _GenerateReportBtnVisible.control = "System.Windows.Forms.RadioButton";
           _GenerateReportBtnVisible.text = "Generate Report Button Visible";

           _GenerateAnnotationBtnVisible = new IVLControlProperties();
           _GenerateAnnotationBtnVisible.name = "GenerateAnnotationBtnVisible";
           _GenerateAnnotationBtnVisible.val = true.ToString();
           _GenerateAnnotationBtnVisible.type = "bool";
           _GenerateAnnotationBtnVisible.control = "System.Windows.Forms.RadioButton";
           _GenerateAnnotationBtnVisible.text = "Generate Annotation Button Visible";

           _GenerateCDRBtnVisible = new IVLControlProperties();
           _GenerateCDRBtnVisible.name = "GenerateCDRBtnVisible";
           _GenerateCDRBtnVisible.val = true.ToString();
           _GenerateCDRBtnVisible.type = "bool";
           _GenerateCDRBtnVisible.control = "System.Windows.Forms.RadioButton";
           _GenerateCDRBtnVisible.text = "Generate CDR Button Visible";

           _ZoomFunctionVisble = new IVLControlProperties();
           _ZoomFunctionVisble.name = "ZoomFunctionVisble";
           _ZoomFunctionVisble.val = true.ToString();
           _ZoomFunctionVisble.type = "bool";
           _ZoomFunctionVisble.control = "System.Windows.Forms.RadioButton";
           _ZoomFunctionVisble.text = "Zoom Function Visible";

           _BrightnessFunctionVisble = new IVLControlProperties();
           _BrightnessFunctionVisble.name = "BrightnessFunctionVisble";
           _BrightnessFunctionVisble.val = true.ToString();
           _BrightnessFunctionVisble.type = "bool";
           _BrightnessFunctionVisble.control = "System.Windows.Forms.RadioButton";
           _BrightnessFunctionVisble.text = "Brightness Function Visible";

           _ContrastFunctionVisble = new IVLControlProperties();
           _ContrastFunctionVisble.name = "ContrastFunctionVisble";
           _ContrastFunctionVisble.val = true.ToString();
           _ContrastFunctionVisble.type = "bool";
           _ContrastFunctionVisble.control = "System.Windows.Forms.RadioButton";
           _ContrastFunctionVisble.text = "Contrast Function Visible";

           _SaveFunctionVisble = new IVLControlProperties();
           _SaveFunctionVisble.name = "SaveFunctionVisble";
           _SaveFunctionVisble.val = true.ToString();
           _SaveFunctionVisble.type = "bool";
           _SaveFunctionVisble.control = "System.Windows.Forms.RadioButton";
           _SaveFunctionVisble.text = "Save Function Visible";

           _SaveAsFunctionVisble = new IVLControlProperties();
           _SaveAsFunctionVisble.name = "SaveAsFunctionVisble";
           _SaveAsFunctionVisble.val = true.ToString();
           _SaveAsFunctionVisble.type = "bool";
           _SaveAsFunctionVisble.control = "System.Windows.Forms.RadioButton";
           _SaveAsFunctionVisble.text = "SaveAs Function Visible";

           _ExportFunctionVisble = new IVLControlProperties();
           _ExportFunctionVisble.name = "ExportFunctionVisble";
           _ExportFunctionVisble.val = true.ToString();
           _ExportFunctionVisble.type = "bool";
           _ExportFunctionVisble.control = "System.Windows.Forms.RadioButton";
           _ExportFunctionVisble.text = "Export Function Visible";

           _RightLeftVisble = new IVLControlProperties();
           _RightLeftVisble.name = "RightLeftVisble";
           _RightLeftVisble.val = true.ToString();
           _RightLeftVisble.type = "bool";
           _RightLeftVisble.control = "System.Windows.Forms.RadioButton";
           _RightLeftVisble.text = "Right Left Visible";

           _ShowFiltersVisble = new IVLControlProperties();
           _ShowFiltersVisble.name = "ShowFiltersVisble";
           _ShowFiltersVisble.val = true.ToString();
           _ShowFiltersVisble.type = "bool";
           _ShowFiltersVisble.control = "System.Windows.Forms.RadioButton";
           _ShowFiltersVisble.text = "Show Filters Visible";

           _RemovePostProcessingBtnVisible = new IVLControlProperties();
           _RemovePostProcessingBtnVisible.name = "RemovePostProcessingBtnVisible";
           _RemovePostProcessingBtnVisible.val = true.ToString();
           _RemovePostProcessingBtnVisible.type = "bool";
           _RemovePostProcessingBtnVisible.control = "System.Windows.Forms.RadioButton";
           _RemovePostProcessingBtnVisible.text = "Remove PostProcessing Button Visible";

           _PostProcessingLabelVisible = new IVLControlProperties();
           _PostProcessingLabelVisible.name = "PostProcessingLabelVisible";
           _PostProcessingLabelVisible.val = true.ToString();
           _PostProcessingLabelVisible.type = "bool";
           _PostProcessingLabelVisible.control = "System.Windows.Forms.RadioButton";
           _PostProcessingLabelVisible.text = "PostProcessing Label Visible";

           _ReportWindowClose = new IVLControlProperties();
           _ReportWindowClose.name = "ReportWindowClose";
           _ReportWindowClose.val = true.ToString();
           _ReportWindowClose.type = "bool";
           _ReportWindowClose.control = "System.Windows.Forms.RadioButton";
           _ReportWindowClose.text = "Close Report After Saving";
       }
   }

   [Serializable]
   public class LiveImaging
   {
       //public bool CaptureBtnVisible = true;
       //public bool SaveBtnVisible = false;
       //public bool BrowseBtnVisible = false;
       //public bool FlashBtnVisible = false;
       //public bool IRBtnVisible = false;
       //public bool RightLeftVisble = true;
       //public bool LiveGainVisble = true;
       //public bool LiveExposureVisble = true;
       //public bool FlashExposureVisble = true;
       //public bool FlashGainVisble = true;
       //public bool MotorStepsVisible = true;


       private static  IVLControlProperties CaptureBtnVisible = null;

       public IVLControlProperties _CaptureBtnVisible
       {
           get { return CaptureBtnVisible; }
           set { CaptureBtnVisible = value; }
       }
       private static  IVLControlProperties SaveBtnVisible = null;

       public IVLControlProperties _SaveBtnVisible
       {
           get { return SaveBtnVisible; }
           set { SaveBtnVisible = value; }
       }
       private static  IVLControlProperties BrowseBtnVisible = null;

       public IVLControlProperties _BrowseBtnVisible
       {
           get { return BrowseBtnVisible; }
           set { BrowseBtnVisible = value; }
       }
       private  IVLControlProperties FlashBtnVisible = null;

       public IVLControlProperties _FlashBtnVisible
       {
           get { return FlashBtnVisible; }
           set { FlashBtnVisible = value; }
       }

       private  IVLControlProperties PosteriorVisible = null;

       public IVLControlProperties _PosteriorVisible
       {
           get { return PosteriorVisible; }
           set { PosteriorVisible = value; }
       }

       private  IVLControlProperties AnteriorVisible = null;

       public IVLControlProperties _AnteriorVisible
       {
           get { return AnteriorVisible; }
           set { AnteriorVisible = value; }
       }

       private  IVLControlProperties ffaVisible = null;

       public IVLControlProperties FfaVisible
       {
           get { return ffaVisible; }
           set { ffaVisible = value; }
       }

       private  IVLControlProperties ffaColorVisible = null;

       public IVLControlProperties FfaColorVisible
       {
           get { return ffaColorVisible; }
           set { ffaColorVisible = value; }
       }

       private  IVLControlProperties IRBtnVisible = null;

       public IVLControlProperties _IRBtnVisible
       {
           get { return IRBtnVisible; }
           set { IRBtnVisible = value; }
       }
       private static  IVLControlProperties RightLeftVisble = null;

       public IVLControlProperties _RightLeftVisble
       {
           get { return RightLeftVisble; }
           set { RightLeftVisble = value; }
       }
       private static  IVLControlProperties LiveGainVisble = null;

       public IVLControlProperties _LiveGainVisble
       {
           get { return LiveGainVisble; }
           set { LiveGainVisble = value; }
       }
       private static  IVLControlProperties LiveExposureVisble = null;

       public IVLControlProperties _LiveExposureVisble
       {
           get { return LiveExposureVisble; }
           set { LiveExposureVisble = value; }
       }
       private static  IVLControlProperties FlashExposureVisble = null;

       public IVLControlProperties _FlashExposureVisble
       {
           get { return FlashExposureVisble; }
           set { FlashExposureVisble = value; }
       }
       private static  IVLControlProperties FlashGainVisble = null;

       public IVLControlProperties _FlashGainVisble
       {
           get { return FlashGainVisble; }
           set { FlashGainVisble = value; }
       }
       private static  IVLControlProperties MotorStepsVisible = null;

       public IVLControlProperties _MotorStepsVisible
       {
           get { return MotorStepsVisible; }
           set { MotorStepsVisible = value; }
       }
       private  IVLControlProperties fortyFiveButtonVisible = null;

       public IVLControlProperties FortyFiveButtonVisible
       {
           get { return fortyFiveButtonVisible; }
           set { fortyFiveButtonVisible = value; }
       }
       private  IVLControlProperties startFFATimerButtonVisible = null;

       public IVLControlProperties StartFFATimerButtonVisible
       {
           get { return startFFATimerButtonVisible; }
           set { startFFATimerButtonVisible = value; }
       }
       private IVLControlProperties showLiveSource = null;

       public IVLControlProperties ShowLiveSource
       {
           get { return showLiveSource; }
           set { showLiveSource = value; }
       }


       public LiveImaging()
       {
           _CaptureBtnVisible = new IVLControlProperties();
           _CaptureBtnVisible.name = "CaptureBtnVisible";
           _CaptureBtnVisible.val = true.ToString();
           _CaptureBtnVisible.type = "bool";
           _CaptureBtnVisible.control = "System.Windows.Forms.RadioButton";
           _CaptureBtnVisible.text = "Capture Button Visible";

           _SaveBtnVisible = new IVLControlProperties();
           _SaveBtnVisible.name = "SaveBtnVisible";
           _SaveBtnVisible.val = false.ToString();
           _SaveBtnVisible.type = "bool";
           _SaveBtnVisible.control = "System.Windows.Forms.RadioButton";
           _SaveBtnVisible.text = "Save Button Visible";

           _BrowseBtnVisible = new IVLControlProperties();
           _BrowseBtnVisible.name = "BrowseBtnVisible";
           _BrowseBtnVisible.val = false.ToString();
           _BrowseBtnVisible.type = "bool";
           _BrowseBtnVisible.control = "System.Windows.Forms.RadioButton";
           _BrowseBtnVisible.text = "Browse Button Visible";

           _PosteriorVisible = new IVLControlProperties();
           _PosteriorVisible.name = "PosteriorVisible";
           _PosteriorVisible.val = true.ToString();
           _PosteriorVisible.type = "bool";
           _PosteriorVisible.control = "System.Windows.Forms.RadioButton";
           _PosteriorVisible.text = "Prime Button Visible";

           _AnteriorVisible = new IVLControlProperties();
           _AnteriorVisible.name = "AnteriorVisible";
           _AnteriorVisible.val = false.ToString();
           _AnteriorVisible.type = "bool";
           _AnteriorVisible.control = "System.Windows.Forms.RadioButton";
           _AnteriorVisible.text = "Anterior Button Visible";

           FfaVisible = new IVLControlProperties();
           FfaVisible.name = "FfaVisible";
           FfaVisible.val = false.ToString();
           FfaVisible.type = "bool";
           FfaVisible.control = "System.Windows.Forms.RadioButton";
           FfaVisible.text = "FFA Button Visible";


           StartFFATimerButtonVisible = new IVLControlProperties();
           StartFFATimerButtonVisible.name = "StartFFATimerButtonVisible";
           StartFFATimerButtonVisible.val = false.ToString();
           StartFFATimerButtonVisible.type = "bool";
           StartFFATimerButtonVisible.control = "System.Windows.Forms.RadioButton";
           StartFFATimerButtonVisible.text = "Start FFA Timer Button Visible";

           FfaColorVisible = new IVLControlProperties();
           FfaColorVisible.name = "FfaColorVisible";
           FfaColorVisible.val = false.ToString();
           FfaColorVisible.type = "bool";
           FfaColorVisible.control = "System.Windows.Forms.RadioButton";
           FfaColorVisible.text = "FFA Color Button Visible";

           FortyFiveButtonVisible = new IVLControlProperties();
           FortyFiveButtonVisible.name = "FortyFiveButtonVisible";
           FortyFiveButtonVisible.val = false.ToString();
           FortyFiveButtonVisible.type = "bool";
           FortyFiveButtonVisible.control = "System.Windows.Forms.RadioButton";
           FortyFiveButtonVisible.text = "Forty Five Button Visible";


           _FlashBtnVisible = new IVLControlProperties();
           _FlashBtnVisible.name = "FlashBtnVisible";
           _FlashBtnVisible.val = false.ToString();
           _FlashBtnVisible.type = "bool";
           _FlashBtnVisible.control = "System.Windows.Forms.RadioButton";
           _FlashBtnVisible.text = "Flash Button Visible";

           _IRBtnVisible = new IVLControlProperties();
           _IRBtnVisible.name = "IRBtnVisible";
           _IRBtnVisible.val = false.ToString();
           _IRBtnVisible.type = "bool";
           _IRBtnVisible.control = "System.Windows.Forms.RadioButton";
           _IRBtnVisible.text = "IR Button Visible";

           _RightLeftVisble = new IVLControlProperties();
           _RightLeftVisble.name = "RightLeftVisble";
           _RightLeftVisble.val = true.ToString();
           _RightLeftVisble.type = "bool";
           _RightLeftVisble.control = "System.Windows.Forms.RadioButton";
           _RightLeftVisble.text = "Right Left Visible";

           _LiveGainVisble = new IVLControlProperties();
           _LiveGainVisble.name = "LiveGainVisble";
           _LiveGainVisble.val = true.ToString();
           _LiveGainVisble.type = "bool";
           _LiveGainVisble.control = "System.Windows.Forms.RadioButton";
           _LiveGainVisble.text = "Live Gain Visible";

           _LiveExposureVisble = new IVLControlProperties();
           _LiveExposureVisble.name = "LiveExposureVisble";
           _LiveExposureVisble.val = true.ToString();
           _LiveExposureVisble.type = "bool";
           _LiveExposureVisble.control = "System.Windows.Forms.RadioButton";
           _LiveExposureVisble.text = "Live Exposure Visible";

           _FlashExposureVisble = new IVLControlProperties();
           _FlashExposureVisble.name = "FlashExposureVisble";
           _FlashExposureVisble.val = true.ToString();
           _FlashExposureVisble.type = "bool";
           _FlashExposureVisble.control = "System.Windows.Forms.RadioButton";
           _FlashExposureVisble.text = "Flash Exposure Visible";

           _FlashGainVisble = new IVLControlProperties();
           _FlashGainVisble.name = "FlashGainVisble";
           _FlashGainVisble.val = true.ToString();
           _FlashGainVisble.type = "bool";
           _FlashGainVisble.control = "System.Windows.Forms.RadioButton";
           _FlashGainVisble.text = "Flash Gain Visible";

           _MotorStepsVisible = new IVLControlProperties();
           _MotorStepsVisible.name = "MotorStepsVisible";
           _MotorStepsVisible.val = true.ToString();
           _MotorStepsVisible.type = "bool";
           _MotorStepsVisible.control = "System.Windows.Forms.RadioButton";
           _MotorStepsVisible.text = "Motor Steps Visible";

           ShowLiveSource = new IVLControlProperties();
           ShowLiveSource.name = "ShowLiveSource";
           ShowLiveSource.val = true.ToString();
           ShowLiveSource.type = "bool";
           ShowLiveSource.control = "System.Windows.Forms.RadioButton";
           ShowLiveSource.text = "Show Live Source";


       }
   }

   public class EmrUI
   {
       private static  IVLControlProperties IntuCloudButtonVisible = null;

       public IVLControlProperties _IntuCloudButtonVisible
       {
           get { return IntuCloudButtonVisible; }
           set { IntuCloudButtonVisible = value; }
       }
       public EmrUI()
       {
           _IntuCloudButtonVisible = new IVLControlProperties();
           _IntuCloudButtonVisible.name = "IntuCloudButtonVisible";
           _IntuCloudButtonVisible.val = false.ToString();
           _IntuCloudButtonVisible.type = "bool";
           _IntuCloudButtonVisible.control = "System.Windows.Forms.RadioButton";
           _IntuCloudButtonVisible.text = "IntuCloud Button Visible";
       }
     }
   }

