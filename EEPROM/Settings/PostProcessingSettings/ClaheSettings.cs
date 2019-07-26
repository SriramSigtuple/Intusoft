using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EEPROM
{
    [Serializable]
    public class ClaheSettings
    {
        private EEPROM_Props clipValueB = null;

        public EEPROM_Props ClipValueB
        {
            get { return clipValueB; }
            set { clipValueB = value; }
        }

        private EEPROM_Props clipValueG = null;

        public EEPROM_Props ClipValueG
        {
            get { return clipValueG; }
            set { clipValueG = value; }
        }
        private EEPROM_Props clipValueR = null;

        public EEPROM_Props ClipValueR
        {
            get { return clipValueR; }
            set { clipValueR = value; }
        }

        private EEPROM_Props isApplyClaheSettings = null;

        public EEPROM_Props IsApplyClaheSettings
        {
            get { return isApplyClaheSettings; }
            set { isApplyClaheSettings = value; }
        }
        public ClaheSettings()
        {

            EEPROM_Data_Types<byte> applyClaheVal = new EEPROM_Data_Types<byte>(1, 0, 1);

            IsApplyClaheSettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), applyClaheVal); ;

            EEPROM_Data_Types<float> _ClipValueRVal = new EEPROM_Data_Types<float>(0.002f, 0f,1f);

            ClipValueR = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Single, 4), _ClipValueRVal);
            EEPROM_Data_Types<float> _ClipValueGVal = new EEPROM_Data_Types<float>(0.002f, 0f, 1f);

            ClipValueG = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Single, 4), _ClipValueGVal);
            EEPROM_Data_Types<float> _ClipValueBVal = new EEPROM_Data_Types<float>(0.002f, 0f, 1f);

            ClipValueB = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo( DataTypes.Single,4),_ClipValueBVal); 

        }
    }
}
