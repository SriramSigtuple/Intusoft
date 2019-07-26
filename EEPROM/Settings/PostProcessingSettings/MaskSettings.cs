using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EEPROM
{
    [Serializable]
    public class MaskSettings
    {
        private  EEPROM_Props maskWidth = null;

        public EEPROM_Props MaskWidth
        {
            get { return maskWidth; }
            set { maskWidth = value; }
        }
        private  EEPROM_Props maskHeight = null;

        public EEPROM_Props MaskHeight
        {
            get { return maskHeight; }
            set { maskHeight = value; }
        }
        private  EEPROM_Props isApplyMask = null;

        public EEPROM_Props IsApplyMask
        {
            get { return isApplyMask; }
            set { isApplyMask = value; }
        }
        private  EEPROM_Props applyLiveMask = null;

        public EEPROM_Props ApplyLiveMask
        {
            get { return applyLiveMask; }
            set { applyLiveMask = value; }
        }

        private EEPROM_Props applyLogo = null;

        public EEPROM_Props ApplyLogo
        {
            get { return applyLogo; }
            set { applyLogo = value; }
        }

        private EEPROM_Props liveMaskWidth;

        public EEPROM_Props LiveMaskWidth
        {
            get { return liveMaskWidth; }
            set { liveMaskWidth = value; }
        }

        private EEPROM_Props liveMaskHeight;

        public EEPROM_Props LiveMaskHeight
        {
            get { return liveMaskHeight; }
            set { liveMaskHeight = value; }
        }
        public MaskSettings()
        {

            EEPROM_Data_Types<Int16> _livemMaskWidthVal = new EEPROM_Data_Types<Int16>(2400, 1, 10000);

            LiveMaskWidth = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), _livemMaskWidthVal);

            EEPROM_Data_Types<Int16> _livemMaskHeightVal = new EEPROM_Data_Types<Int16>(2400, 1, 10000);

            LiveMaskHeight = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), _livemMaskHeightVal);

            EEPROM_Data_Types<Int16> maskWidth = new EEPROM_Data_Types<Int16>(2400, 1, 10000);

            MaskWidth = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), maskWidth);

            EEPROM_Data_Types<Int16> maskHeight = new EEPROM_Data_Types<Int16>(2400, 1, 10000);

            MaskHeight = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), maskHeight);

            EEPROM_Data_Types<byte> applyCaptureMask = new EEPROM_Data_Types<byte>(1, 0, 1);

            IsApplyMask = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo( DataTypes.Byte,2),applyCaptureMask);

            EEPROM_Data_Types<byte> applyLogo = new EEPROM_Data_Types<byte>(1, 0, 1);

            ApplyLogo = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo( DataTypes.Byte,1), applyLogo);

            EEPROM_Data_Types<byte> applyLiveMask = new EEPROM_Data_Types<byte>(1, 0, 1);

            ApplyLiveMask = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo( DataTypes.Byte,1), applyLiveMask);

           
        }
    }
   
}
