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
  public  class AnnotationColorSelection
    {
      public static Logger Exception_Log = LogManager.GetLogger("DB_Porting.ExceptionLog");

        private static IVLControlProperties CupColor = null;

        public IVLControlProperties _CupColor
        {
            get { return AnnotationColorSelection.CupColor; }
            set { AnnotationColorSelection.CupColor = value; }
        }

        private static IVLControlProperties DiscColor = null;

        public IVLControlProperties _DiscColor
        {
            get { return AnnotationColorSelection.DiscColor; }
            set { AnnotationColorSelection.DiscColor = value; }
        }

        private static IVLControlProperties AnnotationMarkingColor = null;

        public IVLControlProperties _AnnotationMarkingColor
        {
            get { return AnnotationColorSelection.AnnotationMarkingColor; }
            set { AnnotationColorSelection.AnnotationMarkingColor = value; }
        }
        public AnnotationColorSelection()
        {
            try
            {
                _CupColor = new IVLControlProperties();
                _CupColor.name = "CupColor";
                _CupColor.type = "string";
                _CupColor.val = "Blue";
                _CupColor.control = "System.Windows.Forms.ComboBox";
                _CupColor.text = "Cup Color";
                _CupColor.range = "Aqua,Blue,Black,Cyan,DarkBlue,Green,Grey,Khaki,LimeGreen";

                _DiscColor = new IVLControlProperties();
                _DiscColor.name = "DiscColor";
                _DiscColor.type = "string";
                _DiscColor.val = "Black";
                _DiscColor.control = "System.Windows.Forms.ComboBox";
                _DiscColor.text = "Disc Color";
                _DiscColor.range = "Aqua,Blue,Black,Cyan,DarkBlue,Green,Grey,Khaki,LimeGreen";

                _AnnotationMarkingColor = new IVLControlProperties();
                _AnnotationMarkingColor.name = "AnnotationMarkingColor";
                _AnnotationMarkingColor.type = "string";
                _AnnotationMarkingColor.val = "Black";
                _AnnotationMarkingColor.control = "System.Windows.Forms.ComboBox";
                _AnnotationMarkingColor.text = "Annotation Marking Color";
                _AnnotationMarkingColor.range = "Aqua,Blue,Black,Cyan,DarkBlue,Green,Grey,Khaki,LimeGreen";
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }
    }
}
