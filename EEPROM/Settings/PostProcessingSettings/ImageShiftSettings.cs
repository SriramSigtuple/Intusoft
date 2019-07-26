using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EEPROM
{
    [Serializable]
    public class ImageShiftSettings
    {
        // Image shift parameters
        //public bool isApplyImageShift = true;
        //public int ImageShiftX = 4;
        //public int ImageShiftY = 3;

        private EEPROM_Props isApplyImageShift = null;

        public EEPROM_Props IsApplyImageShift
        {
            get { return isApplyImageShift; }
            set { isApplyImageShift = value; }
        }
        private EEPROM_Props imageShiftX = null;

        public EEPROM_Props ImageShiftX
        {
            get { return imageShiftX; }
            set { imageShiftX = value; }
        }
        private EEPROM_Props imageShiftY = null;

        public EEPROM_Props ImageShiftY
        {
            get { return imageShiftY; }
            set { imageShiftY = value; }
        }
        public ImageShiftSettings()
        {
            EEPROM_Data_Types<byte> shiftValue = new EEPROM_Data_Types<byte>(1, 0, 1);
            IsApplyImageShift = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo( DataTypes.Byte,1),shiftValue);

            EEPROM_Data_Types<byte> shiftX = new EEPROM_Data_Types<byte>(4, byte.MinValue, byte.MaxValue);

            ImageShiftX = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo( DataTypes.Byte,2),shiftX);

            EEPROM_Data_Types<byte> shiftY = new EEPROM_Data_Types<byte>(4, byte.MinValue, byte.MaxValue);
            ImageShiftY = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 2),shiftY);

        }
       
    }
}
