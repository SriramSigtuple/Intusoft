using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace IVLReport
{
    [Serializable]
  public  class MailAddresses
    {
      public List<string> mailAddressList;
      public MailAddresses()
      {
          mailAddressList = new List<string>();
      }
    }

    public class XmlConfigUtility
    {
        public static void Serialize(Object data, string fileName)
        {
            Type type = data.GetType();
            XmlSerializer xs = new XmlSerializer(type);
            XmlTextWriter xmlWriter = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
            xs.Serialize(xmlWriter, data);
            xmlWriter.Close();
        }

        public static Object Deserialize(Type type, string fileName)
        {
            Object data = null;
            XmlSerializer xs = null;
            XmlTextReader xmlReader = null;
            try
            {
                xs = new XmlSerializer(type);
                xmlReader = new XmlTextReader(fileName);
                data = xs.Deserialize(xmlReader);
            }
            finally
            {
                xmlReader.Close();
            }
            return data;
        }
    }
}
