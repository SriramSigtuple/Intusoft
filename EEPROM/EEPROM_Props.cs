using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEPROM
{
    public class EEPROM_Props
    {
        private static EEPROM_Props _controlProperties;
        public string name = string.Empty;
        public string text = string.Empty;
        public string type = string.Empty;
        public EEPROM_Type_Info eepromDataType ;
        public string range = "";
        public PageDetails PageDetails;
        public object value;
        public static EEPROM_Props createInstance()
        {
            return _controlProperties = new EEPROM_Props();
        }
        public static EEPROM_Props GetInstance()
        {
            return _controlProperties;
        }
        public static EEPROM_Props CreateIVLProperties()
        {
            return new EEPROM_Props();
        }
        public static EEPROM_Props CreateIVLProperties( EEPROM_Type_Info typeInfo,object ValueObject,
            string Range = "0"
           )
        {
            return new EEPROM_Props
            {
               value = ValueObject,
                range = Range,
                eepromDataType = typeInfo

            };
        }
        private EEPROM_Props()
        {

        }
    }
}
