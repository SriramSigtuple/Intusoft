using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EEPROM
{
    [Serializable]
    public class FirmwareSettings
    {
      
      
        private  EEPROM_Props isUSBCommunication = null;

        public EEPROM_Props IsUSBCommunication
        {
            get { return isUSBCommunication; }
            set { isUSBCommunication = value; }
        }
        private  EEPROM_Props cameraPowerTimerInterval = null;

        public EEPROM_Props CameraPowerTimerInterval
        {
            get { return cameraPowerTimerInterval; }
            set { cameraPowerTimerInterval = value; }
        }


        private  EEPROM_Props flashOffsetStart = null;

        public EEPROM_Props FlashOffsetStart
        {
            get { return flashOffsetStart; }
            set { flashOffsetStart = value; }
        }

        private  EEPROM_Props flashOffsetEnd = null;

        public EEPROM_Props FlashOffsetEnd
        {
            get { return flashOffsetEnd; }
            set { flashOffsetEnd = value; }
        }

        private  EEPROM_Props isFlashBoost = null;

        public EEPROM_Props IsFlashBoost
        {
            get { return isFlashBoost; }
            set { isFlashBoost = value; }
        }
        private  EEPROM_Props flashBoostValue = null;
        /// <summary>
        /// Flash boost value contains the value used to boost the flash intensity it is int form 
        /// </summary>
        public EEPROM_Props FlashBoostValue
        {
            get { return flashBoostValue; }
            set { flashBoostValue = value; }
        }

        private  EEPROM_Props enableSingleFrameCapture = null;

        public EEPROM_Props EnableSingleFrameCapture
        {
            get { return enableSingleFrameCapture; }
            set { enableSingleFrameCapture = value; }
        }

        private  EEPROM_Props boardCommandIteration = null;

        public EEPROM_Props BoardCommandIteration
        {
            get { return boardCommandIteration; }
            set { boardCommandIteration = value; }
        }

        private  EEPROM_Props enableLeftRightSensor = null;

        public EEPROM_Props EnableLeftRightSensor
        {
            get { return enableLeftRightSensor; }
            set { enableLeftRightSensor
                = value; }
        }
       public FirmwareSettings()
        {

            EEPROM_Data_Types<byte> enableSingleFrameVal = new EEPROM_Data_Types<byte>(1, 0, 1);

            EnableSingleFrameCapture = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte,1),enableSingleFrameVal); ;

            EEPROM_Data_Types<byte> isUSBCom = new EEPROM_Data_Types<byte>(1, 0, 1);

            IsUSBCommunication = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte,1), isUSBCom); ;
            EEPROM_Data_Types<byte> isFlashBoost = new EEPROM_Data_Types<byte>(1, 0, 1);

            IsFlashBoost = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte,1), isFlashBoost); ;
            EEPROM_Data_Types<byte> enableLeftRightSensor = new EEPROM_Data_Types<byte>(1, 0, 1);

            EnableLeftRightSensor = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo( DataTypes.Byte,1),enableLeftRightSensor); ;
            EEPROM_Data_Types<byte> boardIter = new EEPROM_Data_Types<byte>(2, 0, 10);

            BoardCommandIteration = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte,1), boardIter);
            EEPROM_Data_Types<UInt16> camInt = new EEPROM_Data_Types<UInt16>(1000, 0, UInt16.MaxValue);

            CameraPowerTimerInterval = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.UInt16,2), camInt);
            EEPROM_Data_Types<byte> flashOffsetStart = new EEPROM_Data_Types<byte>(90, 1, byte.MaxValue);


            FlashOffsetStart = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), flashOffsetStart);

            EEPROM_Data_Types<byte> flashOffsetEnd = new EEPROM_Data_Types<byte>(10, 1, byte.MaxValue);

            FlashOffsetEnd = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), flashOffsetEnd);

            EEPROM_Data_Types<byte> flashBoostVal = new EEPROM_Data_Types<byte>(95, 1, byte.MaxValue);

            FlashBoostValue = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), flashBoostVal);


        }
    }
}
