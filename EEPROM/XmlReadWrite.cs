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

namespace EEPROM
{

    public class XmlReadWrite
    {
        XmlSerializer xmlSerialization;
        static bool isChangeValueSaved = false;

        public XmlReadWrite()
        {
        }
        public static  void SetValue(String node, object val)
        {
            EEPROM_Version_Details_Page detailsPage = EEPROM_Version_Details_Page.GetInstance();
            string[] parts = node.Split('.');
            isChangeValueSaved = false;
            SetValueIterate(detailsPage.fieldDic, parts,1,val);
        }
        public static void SetValueIterate(Dictionary<object, object> tempDic, string[] nodes, int index, object val)
        {
            foreach (KeyValuePair<object, object> item in tempDic)
            {
                if (isChangeValueSaved)
                    break;
                if (index < nodes.Length - 1 && item.Key is EEPROM_Props)
                {
                    EEPROM_Props keyEEPROM = item.Key as EEPROM_Props;

                    if ((keyEEPROM.name == nodes[index]))
                    {
                        index += 1;
                         SetValueIterate(item.Value as Dictionary<object, object>, nodes, index, val);
                         if (isChangeValueSaved)
                             break;
                    }
                }
                else
                {

                    if (item.Key is string)
                    {
                        EEPROM_Props changeValEepromProps = item.Value as EEPROM_Props;

                        if (changeValEepromProps.text== nodes[index])  
                        {
                            if (changeValEepromProps.eepromDataType.dataType == DataTypes.ByteArr)
                                changeValEepromProps.value = val;
                            else
                            {
                              string str =  changeValEepromProps.eepromDataType.dataType.ToString();
                              Type typeVal = Type.GetType(str);
                               changeValEepromProps.SetEEPROM_DataType_Value(val,SetValueItem.Value);
                                isChangeValueSaved = true;
                                break;
                            
                            }

                        }
                    }

                }
            }
           
        }

    }
}
