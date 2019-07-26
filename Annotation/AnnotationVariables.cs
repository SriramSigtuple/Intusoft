using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
namespace Annotation
{
   public static class AnnotationVariables
    {
       public static bool isGlaucomaTool = false;
       public static bool isGlaucomaToolViewing = false;
       public static string addDiscPointsWarning = "";
       public static string addCupPointsWarning = "";
       public static string warningHeader = "";
       public static string annotationMarkingColor = "";
       public static string cupColor = "";
       public static string discColor = "";
       public static int glucomeHeight = 678;
       public static int glucomeWidth = 668;


    }
   public static class XmlConfigUtility
   {
       public static void Serialize(Object data, Stream fileName)
       {
           Type type = data.GetType();
           XmlSerializer xs = new XmlSerializer(type);
           XmlTextWriter xmlWriter = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);
           xmlWriter.Formatting = Formatting.Indented;
           xs.Serialize(xmlWriter, data);
           xmlWriter.Close();
       }

       public static Object Deserialize(Type type, Stream fileName)
       {
           XmlSerializer xs = new XmlSerializer(type);

           XmlTextReader xmlReader = new XmlTextReader(fileName);
           Object data = xs.Deserialize(xmlReader);

           xmlReader.Close();

           return data;
       }
   }
}
