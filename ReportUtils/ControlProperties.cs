using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportUtils
{
   public class ControlProperties
    {
        public String Name = "00";
        public String Type = "Type";
        public Int32 X = 100;
        public Int32 Y = 100;
        public Int32 Width = 50;
        public Int32 Height = 50;
        public Int32 Rows = 0;
        public Int32 Columns = 0;
        public String Text = "Text";
        public String ForeColor = "Red";
        public String FontName = "Arial";
        public String FontStyle = "Regular";
        public Int32 FontSize = 12;
        public String ImageName = "";
        public String BindingType ="";
        //public Int32 Xpadding = 0;    
        //public Int32 Ypadding = 0;
    }
   public class ReportXmlProperties
   {
       public List<ControlProperties> reportControlProperties;
       public LayoutDetails.PageOrientation _currentOrientation;
       public string[] ImageURLS;
       public string[] ImageNamesText;
       public ReportXmlProperties()
       {
           reportControlProperties = new List<ControlProperties>();
       }
   }
}
