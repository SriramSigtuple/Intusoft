using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using System.Xml.Serialization;
using System.Windows.Forms;
using Common;

namespace INTUSOFT.Configuration
{

    public class XmlReadWrite
    {
        public String fileName = null;
        public static string password = string.Empty;
        public static string[] StringArrForPassword = new string[3];
        MemoryStream memStream ;
        XmlSerializer xmlSerialization;
        public XmlReadWrite()
        {
        }
        public XmlReadWrite(String fname)
        {
            fileName = fname;
        }
        private String defaultname = @"Resources\config.xml";
        public static Dictionary<String, Object> dic;
        
        public void ReadXml()
        {
            XmlDocument xml = new XmlDocument();
            {
                if (fileName != null)
                    xml.Load(fileName);
                else
                    xml.Load(defaultname);
            }
            XmlNodeList list = xml.ChildNodes;
            dic = traverse_nodes(list);
            //When we traverse the XML first tag will be the one containing the 
            //xml format and version, since we don to require this tag presently, 
            //removing the xml tag after reading.
            if (dic.ContainsKey("xml"))//Remove the Xml Tag
                dic.Remove("xml");
            if(!isControlPropertiesXml)
            PopulateControlProperties(dic);

        }
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
            WriteXml(fileName);
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

        public void WriteXml(String fname)
        {
            {
                StreamWriter sw = new StreamWriter(fname, false);
                xml_writer = new XmlTextWriter(sw);
                xml_writer.Formatting = Formatting.Indented;
                xml_writer.WriteStartDocument();
                constructXmlStr(dic);
                xml_writer.WriteEndDocument();
                xml_writer.Close();
                sw.Close();
            }
        }
        private void constructXmlStr(Dictionary<String, Object> dic)
        {
           
            foreach (KeyValuePair<String, Object> item in dic)
            {
                if (is_branch(item))
                {
                    xml_writer.WriteStartElement(item.Key);
                    constructXmlStr(item.Value as Dictionary<String, Object>);
                    xml_writer.WriteEndElement();
                }
                else
                {
                    xml_writer.WriteElementString(item.Key, item.Value.ToString());
                }
            }

            return;
        }
        private IVLControlProperties getControlProp(Control c)
        {
            IVLControlProperties cp = new IVLControlProperties();
            //cp.control = c;
            
            return cp;
           
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
           //if(ConfigVariables.CurrentSettings.UserSettings.HeaderText
        }
        public void assignToConfigType(KeyValuePair<string, object> val)
        {
            string value = val.Key.ToString();
            //switch (value)
            //{
            //    //case "UserSettings": ConfigVariables.CurrentSettings.UserSettings.HeaderText
               
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
                    //ConfigVariables.CurrentSettings.
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
                #region code temproarily maintain the password until it is saved as MD5 in config for first release of Drishti Integration
                if (parts[parts.Length - 1].Contains("Password"))
                {
                    StringArrForPassword[0] = parts[0];
                    StringArrForPassword[1] = parts[1];
                    StringArrForPassword[2] = val.ToString();
                }
                #endregion

                SetValueIterate(parts[parts.Length - 1],val.ToString());
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
            Config_UC.setValueIterate(Config_UC.fieldInfos, key, val.ToString(),ConfigVariables.CurrentSettings);
        }
        public static String GetValue(String node)
        {
            string[] parts = node.Split('.');
            Dictionary<String, Object> tmp = dic;
            foreach (string str in parts)
            {

                Object o = getChild(str, tmp);
                if (o is String) return tmp[str].ToString();//returns Values;
                tmp = o as Dictionary<String, Object>;
            }
            return "";
        }

        private static Object getChild(String str, Dictionary<String, Object> tmp)
        {
            if (tmp[str] is String) return str;
            else return tmp[str];
        }
    }
}
