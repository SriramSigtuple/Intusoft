using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotation
{
    public class XmlCommentsProperties
    {
        public String Comments = "00";
       
        public Int32 X = 100;
        public Int32 Y = 100;
        
        public String Header = "Header";
        public String ForeColor = "Red";
        public String FontName = "Arial";
        public DateTime DateTimeVal = DateTime.Now;
        public Int32 FontSize = 12;
        public String BindingType = "";
         
    }
    [Serializable]
   public class AnnotationXMLProperties
    {
        public List<AnnotationComments> Xmlcommentsproperties;
        public List<bool> isCupProperties;
        public string CDRComments = string.Empty;
        public string ReportedBy;//This was added to save the ReportedBy name in Both CDR and Annotation.
        public GraphicsList XmlGraphicsList;
        public List<Shape> Shapes;
       public  AnnotationXMLProperties() 
        {
            Xmlcommentsproperties = new List<AnnotationComments>();
            XmlGraphicsList = new GraphicsList();
            isCupProperties = new List<bool>();
            Shapes = new List<Shape>();
            ReportedBy = string.Empty;
        }

    }
}
