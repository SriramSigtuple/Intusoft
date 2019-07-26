using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using System.Windows.Forms;
using Common;
using log4net;
using log4net.Config;
namespace INTUSOFT.Imaging
{

    public class XmlReadWrite
    {
        private static readonly ILog Exception_Log = LogManager.GetLogger("INTUSOFT.Desktop"); 

        public String fileName = null;
        MemoryStream memStream ;
        XmlSerializer xmlSerialization;
        public XmlReadWrite()
        {
        }
        public XmlReadWrite(String fname)
        {
            fileName = fname;
        }
        public static Dictionary<String, Object> dic;
        
        XmlTextWriter xml_writer;
        XmlWriter xmlStreamWriter;
        public void ReadXmlFromStream()
        {
            //dic = traverse_nodes(list);
            ////When we traverse the XML first tag will be the one containing the 
            ////xml format and version, since we don to require this tag presently, 
            ////removing the xml tag after reading.
            //if (dic.ContainsKey("xml"))//Remove the Xml Tag
            //    dic.Remove("xml");
        }
        public void WriteXml()
        {
           // WriteXml(fileName);
        }

        public KeyValuePair<String, Object> GetRoot()
        {
            if (dic.Count > 0)
            {
                KeyValuePair<String, Object> root = dic.First();
                return root;
            }
            else
            {
                return new KeyValuePair<string, object>();
            }
        }

        bool isControlPropertiesXml = false;
        private Dictionary<String, Object> traverse_nodes(XmlNodeList list)
        {
            Dictionary<String, Object> local_dictionary = new Dictionary<string, object>();
            foreach (XmlNode node in list)
            {
                //if any xml comments are present ignore them.
                if (node.Name == "type")
                {
                    isControlPropertiesXml = true;
                    break;
                }
              

                if (is_branch(node))
                {
               
                    local_dictionary.Add(node.Name, traverse_nodes(node.ChildNodes)); //call the same function
                   
                }
                else
                {
                    local_dictionary.Add(node.Name, node.InnerText); // add to leaf
                    
                }
            }
            return local_dictionary;
        }
        public void GetConfigType(string settingName)
        {
           //if(IVLVariables.CurrentSettings.UserSettings.HeaderText
        }
        public void assignToConfigType(KeyValuePair<string, object> val)
        {
            string value = val.Key.ToString();
            //switch (value)
            //{
            //    //case "UserSettings": IVLVariables.CurrentSettings.UserSettings.HeaderText
               
            //}
            //var typeValue = dic.Where(a => a.Key == val.Key).FirstOrDefault();
            
        }
        private void PopulateControlProperties(Dictionary<string ,object> local_dic)
        {
            //for (int i = 0; i < local_dic.Count; i++)
            foreach (KeyValuePair<string,object> item in local_dic)
            {
                Dictionary<string, object> val = item.Value as Dictionary<string, object>;
                //var typeValue = val.Where(a => a.Key ==item.key).FirstOrDefault();
                if (val!= null)
                {
                    if (is_branch(item))
                    {
                    
                        assignToConfigType(item);
                    //IVLVariables.CurrentSettings.
                    }

                    //if(item is List<KeyValuePair<string, object>>)
                    PopulateControlProperties(val);
                   
                }
                else if(val == null)
                {
                    var newItem = item;
                    GetConfigType(item.Key);
                    //KeyValuePair<string,object> 
                      
                    //KeyValuePair<string, object> neKV = ;
                }
            }
        }

        public Boolean is_branch(XmlNode node)
        {
            if (node.ChildNodes.Count > 1) return true;
            else return false;
        }
        public Boolean is_branch(KeyValuePair<String, Object> item)
        {
            if (item.Value is String) return false;
            else return true;
        }
        public static bool isSaved = true;
        public static  void SetValue(String node, String val)
        {
            string[] parts = node.Split('.');
            //for (int i = 0; i < parts.Length; i++)
            {
                SetValueIterate(parts[0],val.ToString());
                isSaved = false;
                //break;
            }

        }

        public static void SetValueIterate( Dictionary<string ,object> tempDic,object val,string node)
        {
            foreach (KeyValuePair<string, object> item in tempDic)
            {
                if ((item.Value is Dictionary<string, object>))
                {
                    SetValueIterate(item.Value as Dictionary<string, object>, val, node);
                }
                else
                {
                    if (item.Key.Equals(node))
                    {
                        tempDic[item.Key] = val;
                        break;
                    }
                }

            }
        }

        public static void SetValueIterate(string key, string val)
        {
                try
                {

                    for (int i = 0; i < EEPROM_UC.eepromFields.Count; i++)
                    {
                        if (EEPROM_UC.eepromFields.ElementAt(i).Key.Equals(key)) 
                        {
                            KeyValuePair<string,object> obj = EEPROM_UC.eepromFields.ElementAt(i); ;//.GetValue(EEPROM.GetInstance());
                          object retVal = new object();
                          EEPromUtils.SetValue(obj, val, ref retVal);
                          EEPROM_UC.eepromFields[obj.Key] = retVal;

                            break;

                        }

                    }
                }
                catch (Exception ex)
                {
                    var stringBuilder = new StringBuilder();

                    while (ex != null)
                    {
                        stringBuilder.AppendLine(ex.Message);
                        stringBuilder.AppendLine(ex.StackTrace);

                        ex = ex.InnerException;
                    }
                    Exception_Log.Fatal(stringBuilder.ToString());
                }
            }
    }
}
