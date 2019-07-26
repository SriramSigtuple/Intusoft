using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EEPROM
{
    [Serializable]
    public class LutSettings
    {

        private EEPROM_Props isApplyLutSettings = null;

        public EEPROM_Props IsApplyLutSettings
        {
            get { return isApplyLutSettings; }
            set { isApplyLutSettings = value; }
        }

        private EEPROM_Props lutSineFactor = null;

        public EEPROM_Props LutSineFactor
        {
            get { return lutSineFactor; }
            set { lutSineFactor = value; }
        }

        private EEPROM_Props lutInterval1 = null;

        public EEPROM_Props LutInterval1
        {
            get { return lutInterval1; }
            set { lutInterval1 = value; }
        }

        private EEPROM_Props lutInterval2 = null;

        public EEPROM_Props LutInterval2
        {
            get { return lutInterval2; }
            set { lutInterval2 = value; }
        }

        private EEPROM_Props lutOffset = null;

        public EEPROM_Props LutOffset
        {
            get { return lutOffset; }
            set { lutOffset = value; }
        }
        public LutSettings()
        {

            EEPROM_Data_Types<byte> applyLut = new EEPROM_Data_Types<byte>(1, 0, 1);

            IsApplyLutSettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), applyLut);


            EEPROM_Data_Types<byte> lutSineFactor = new EEPROM_Data_Types<byte>(40, byte.MinValue, byte.MaxValue);

            LutSineFactor = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), lutSineFactor);

            EEPROM_Data_Types<byte> lutInt1 = new EEPROM_Data_Types<byte>(50, byte.MinValue, byte.MaxValue);

            LutInterval1 = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo( DataTypes.Byte,1), lutInt1);
            EEPROM_Data_Types<byte> lutInt2 = new EEPROM_Data_Types<byte>(130, byte.MinValue, byte.MaxValue);

            LutInterval2 = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo( DataTypes.Byte,1), lutInt2);
            EEPROM_Data_Types<byte> lutOffset = new EEPROM_Data_Types<byte>(25, byte.MinValue, byte.MaxValue);

            LutOffset = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), lutOffset);

       
        }
    }
}
