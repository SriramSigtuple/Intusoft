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
       public static Logger Exception_Log = LogManager.GetLogger("DB_Porting.ExceptionLog");

       private static IVLControlProperties GenerateReportBtnVisible = null;

       public IVLControlProperties _GenerateReportBtnVisible
       {
           get { return ViewImagingUI.GenerateReportBtnVisible; }
           set { ViewImagingUI.GenerateReportBtnVisible = value; }
       }
       private static IVLControlProperties GenerateAnnotationBtnVisible = null;

       public IVLControlProperties _GenerateAnnotationBtnVisible
       {
           get { return ViewImagingUI.GenerateAnnotationBtnVisible; }
           set { ViewImagingUI.GenerateAnnotationBtnVisible = value; }
       }
       private static IVLControlProperties GenerateCDRBtnVisible = null;

       public IVLControlProperties _GenerateCDRBtnVisible
       {
           get { return ViewImagingUI.GenerateCDRBtnVisible; }
           set { ViewImagingUI.GenerateCDRBtnVisible = value; }
       }
       private static IVLControlProperties ZoomFunctionVisble = null;

       public IVLControlProperties _ZoomFunctionVisble
       {
           get { return ViewImagingUI.ZoomFunctionVisble; }
           set { ViewImagingUI.ZoomFunctionVisble = value; }
       }
       private static IVLControlProperties BrightnessFunctionVisble = null;

       public IVLControlProperties _BrightnessFunctionVisble
       {
           get { return ViewImagingUI.BrightnessFunctionVisble; }
           set { ViewImagingUI.BrightnessFunctionVisble = value; }
       }
       private static IVLControlProperties ContrastFunctionVisble = null;

       public IVLControlProperties _ContrastFunctionVisble
       {
           get { return ViewImagingUI.ContrastFunctionVisble; }
           set { ViewImagingUI.ContrastFunctionVisble = value; }
       }
       private static IVLControlProperties SaveFunctionVisble = null;

       public IVLControlProperties _SaveFunctionVisble
       {
           get { return ViewImagingUI.SaveFunctionVisble; }
           set { ViewImagingUI.SaveFunctionVisble = value; }
       }
       private static IVLControlProperties SaveAsFunctionVisble = null;

       public IVLControlProperties _SaveAsFunctionVisble
       {
           get { return ViewImagingUI.SaveAsFunctionVisble; }
           set { ViewImagingUI.SaveAsFunctionVisble = value; }
       }
       private static IVLControlProperties ExportFunctionVisble = null;

       public IVLControlProperties _ExportFunctionVisble
       {
           get { return ViewImagingUI.ExportFunctionVisble; }
           set { ViewImagingUI.ExportFunctionVisble = value; }
       }
       private static IVLControlProperties RightLeftVisble = null;


       public IVLControlProperties _RightLeftVisble
       {
           get { return ViewImagingUI.RightLeftVisble; }
           set { ViewImagingUI.RightLeftVisble = value; }
       }
       private static IVLControlProperties ShowFiltersVisble = null;

       public IVLControlProperties _ShowFiltersVisble
       {
           get { return ViewImagingUI.ShowFiltersVisble; }
           set { ViewImagingUI.ShowFiltersVisble = value; }
       }
      
       private static IVLControlProperties RemovePostProcessingBtnVisible = null;

       public IVLControlProperties _RemovePostProcessingBtnVisible
       {
           get { return ViewImagingUI.RemovePostProcessingBtnVisible; }
           set { ViewImagingUI.RemovePostProcessingBtnVisible = value; }
       }

    
       private static IVLControlProperties PostProcessingLabelVisible = null;

       public IVLControlProperties _PostProcessingLabelVisible
       {
           get { return ViewImagingUI.PostProcessingLabelVisible; }
           set { ViewImagingUI.PostProcessingLabelVisible = value; }
       }

       private static IVLControlProperties ReportWindowClose = null;
       public IVLControlProperties _ReportWindowClose
       {
           get { return ReportWindowClose; }
           set { ReportWindowClose = value; }
       }
       public ViewImagingUI()
       {
           try
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
           catch (Exception ex)
           {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
           }
           

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

       public static Logger Exception_Log = LogManager.GetLogger("DB_Porting.ExceptionLog");

       private static IVLControlProperties CaptureBtnVisible = null;

       public IVLControlProperties _CaptureBtnVisible
       {
           get { return LiveImaging.CaptureBtnVisible; }
           set { LiveImaging.CaptureBtnVisible = value; }
       }
       private static IVLControlProperties SaveBtnVisible = null;

       public IVLControlProperties _SaveBtnVisible
       {
           get { return LiveImaging.SaveBtnVisible; }
           set { LiveImaging.SaveBtnVisible = value; }
       }
       private static IVLControlProperties BrowseBtnVisible = null;

       public IVLControlProperties _BrowseBtnVisible
       {
           get { return LiveImaging.BrowseBtnVisible; }
           set { LiveImaging.BrowseBtnVisible = value; }
       }
       private static IVLControlProperties FlashBtnVisible = null;

       public IVLControlProperties _FlashBtnVisible
       {
           get { return LiveImaging.FlashBtnVisible; }
           set { LiveImaging.FlashBtnVisible = value; }
       }
       private static IVLControlProperties IRBtnVisible = null;

       public IVLControlProperties _IRBtnVisible
       {
           get { return LiveImaging.IRBtnVisible; }
           set { LiveImaging.IRBtnVisible = value; }
       }
       private static IVLControlProperties RightLeftVisble = null;

       public IVLControlProperties _RightLeftVisble
       {
           get { return LiveImaging.RightLeftVisble; }
           set { LiveImaging.RightLeftVisble = value; }
       }
       private static IVLControlProperties LiveGainVisble = null;

       public IVLControlProperties _LiveGainVisble
       {
           get { return LiveImaging.LiveGainVisble; }
           set { LiveImaging.LiveGainVisble = value; }
       }
       private static IVLControlProperties LiveExposureVisble = null;

       public IVLControlProperties _LiveExposureVisble
       {
           get { return LiveImaging.LiveExposureVisble; }
           set { LiveImaging.LiveExposureVisble = value; }
       }
       private static IVLControlProperties FlashExposureVisble = null;

       public IVLControlProperties _FlashExposureVisble
       {
           get { return LiveImaging.FlashExposureVisble; }
           set { LiveImaging.FlashExposureVisble = value; }
       }
       private static IVLControlProperties FlashGainVisble = null;

       public IVLControlProperties _FlashGainVisble
       {
           get { return LiveImaging.FlashGainVisble; }
           set { LiveImaging.FlashGainVisble = value; }
       }
       private static IVLControlProperties MotorStepsVisible = null;

       public IVLControlProperties _MotorStepsVisible
       {
           get { return LiveImaging.MotorStepsVisible; }
           set { LiveImaging.MotorStepsVisible = value; }
       }

       public LiveImaging()
       {
           try
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
           }
           catch (Exception ex)
           {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
           }
           
       }
   }
   public class EmrUI
   {

       public EmrUI()
       {

       }

   }

}

