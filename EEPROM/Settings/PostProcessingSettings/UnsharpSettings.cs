using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EEPROM
{
    [Serializable]
    public class UnsharpMaskSettings
    {
        private EEPROM_Props unSharpAmount = null;

        public EEPROM_Props UnSharpAmount
        {
            get { return unSharpAmount; }
            set { unSharpAmount = value; }
        }

        private EEPROM_Props isApplyUnsharpSettings = null;

        public EEPROM_Props IsApplyUnsharpSettings
        {
            get { return isApplyUnsharpSettings; }
            set { isApplyUnsharpSettings = value; }
        }

        private EEPROM_Props unSharpRadius = null;

        public EEPROM_Props UnSharpRadius
        {
            get { return unSharpRadius; }
            set { unSharpRadius = value; }
        }

        private EEPROM_Props unsharpThreshold = null;

        public EEPROM_Props UnSharpThreshold
        {
            get { return unsharpThreshold; }
            set { unsharpThreshold = value; }
        }

        private EEPROM_Props medFilter = null;

        public EEPROM_Props MedFilter
        {
            get { return medFilter; }
            set { medFilter = value; }
        }
        public UnsharpMaskSettings()
        {

            EEPROM_Data_Types<byte> applyUnsharp = new EEPROM_Data_Types<byte>(1, 0, 1);

            IsApplyUnsharpSettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), applyUnsharp);

            EEPROM_Data_Types<float> unsharpAmount = new EEPROM_Data_Types<float>(0.5f, 0f, 10f);

            UnSharpAmount = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Single, 4), unsharpAmount);

            EEPROM_Data_Types<byte> unsharpRadius = new EEPROM_Data_Types<byte>(9, byte.MinValue, byte.MaxValue);

            UnSharpRadius = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), unsharpRadius);

            EEPROM_Data_Types<byte> unsharpThreshold = new EEPROM_Data_Types<byte>(40, byte.MinValue, byte.MaxValue);

            UnSharpThreshold = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), unsharpThreshold);

            EEPROM_Data_Types<byte> medFilter = new EEPROM_Data_Types<byte>(3, byte.MinValue, byte.MaxValue);

            MedFilter = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), medFilter);


        }

    }
  

}
