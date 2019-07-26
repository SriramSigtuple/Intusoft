using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EEPROM
{
    [Serializable]
    public class BrightnessContrastSettings
    {

        private EEPROM_Props brightnessVal = null;

        public EEPROM_Props BrightnessVal
        {
            get { return brightnessVal; }
            set { brightnessVal = value; }
        }

        private EEPROM_Props contrastVal = null;

        public EEPROM_Props ContrastVal
        {
            get { return contrastVal; }
            set { contrastVal = value; }
        }

        private EEPROM_Props isApplyBrightness = null;

        public EEPROM_Props IsApplyBrightness
        {
            get { return isApplyBrightness; }
            set { isApplyBrightness = value; }
        }

        private EEPROM_Props isApplyContrast = null;

        public EEPROM_Props IsApplyContrast
        {
            get { return isApplyContrast; }
            set { isApplyContrast = value; }
        }
        
        public BrightnessContrastSettings()
        {
            EEPROM_Data_Types<byte> applyContrastVal = new EEPROM_Data_Types<byte>(1, 0, 1);
            
            IsApplyContrast = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo( DataTypes.Byte,1),applyContrastVal);

            EEPROM_Data_Types<byte> applyBrightnessVal = new EEPROM_Data_Types<byte>(1, 0, 1);

            IsApplyBrightness = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo( DataTypes.Byte,1),applyBrightnessVal);


            EEPROM_Data_Types<byte> BrightnessValue = new EEPROM_Data_Types<byte>(3, 0, 100);
            BrightnessVal = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo( DataTypes.Byte,1),BrightnessValue);

            EEPROM_Data_Types<sbyte> ContrastValue = new EEPROM_Data_Types<sbyte>(1, -20, 20);

            ContrastVal = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.SByte, 1), ContrastValue);

        }
    }
    
}
