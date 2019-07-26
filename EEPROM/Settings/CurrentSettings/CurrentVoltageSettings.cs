using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace EEPROM
{
    public class CurrentVoltageSettings
    {
        private EEPROM_Props live_Mode_Current_Value;

        public EEPROM_Props Live_Mode_Current_Value 
        {
            get { return live_Mode_Current_Value; }
            set { live_Mode_Current_Value = value; }
        }
        private EEPROM_Props live_Mode_Max_Percentage;

        public EEPROM_Props Live_Mode_Max_Percentage
        {
            get { return live_Mode_Max_Percentage; }
            set { live_Mode_Max_Percentage = value; }
        }
        private EEPROM_Props live_Mode_Max_Int_Value;

        public EEPROM_Props Live_Mode_Max_Int_Value
        {
            get { return live_Mode_Max_Int_Value; }
            set { live_Mode_Max_Int_Value = value; }
        }
        private EEPROM_Props live_Mode_Voltage_Value;

        public EEPROM_Props Live_Mode_Voltage_Value
        {
            get { return live_Mode_Voltage_Value; }
            set { live_Mode_Voltage_Value = value; }
        }
        private EEPROM_Props live_Mode_Current_Boost_Value;

        public EEPROM_Props Live_Mode_Current_Boost_Value
        {
            get { return live_Mode_Current_Boost_Value; }
            set { live_Mode_Current_Boost_Value = value; }
        }
        private EEPROM_Props live_Mode_Current_Boost_Enable;

        public EEPROM_Props Live_Mode_Current_Boost_Enable
        {
            get { return live_Mode_Current_Boost_Enable; }
            set { live_Mode_Current_Boost_Enable = value; }
        }
        private EEPROM_Props capture_Mode_Current;

        public EEPROM_Props Capture_Mode_Current_Value
        {
            get { return capture_Mode_Current; }
            set { capture_Mode_Current = value; }
        }
        private EEPROM_Props capture_Mode_Max_Percentage;

        public EEPROM_Props Capture_Mode_Max_Percentage
        {
            get { return capture_Mode_Max_Percentage; }
            set { capture_Mode_Max_Percentage = value; }
        }
        private EEPROM_Props capture_Mode_Max_Int_Value;

        public EEPROM_Props Capture_Mode_Max_Int_Value
        {
            get { return capture_Mode_Max_Int_Value; }
            set { capture_Mode_Max_Int_Value = value; }
        }
        private EEPROM_Props capture_Mode_Voltage_Value;

        public EEPROM_Props Capture_Mode_Voltage_Value
        {
            get { return capture_Mode_Voltage_Value; }
            set { capture_Mode_Voltage_Value = value; }
        }
        private EEPROM_Props capture_Mode_Current_Boost_Value;

        public EEPROM_Props Capture_Mode_Current_Boost_Value
        {
            get { return capture_Mode_Current_Boost_Value; }
            set { capture_Mode_Current_Boost_Value = value; }
        }
        private EEPROM_Props capture_Mode_Current_Boost_Enable;

        public EEPROM_Props Capture_Mode_Current_Boost_Enable
        {
            get { return capture_Mode_Current_Boost_Enable; }
            set { capture_Mode_Current_Boost_Enable = value; }
        }

        public CurrentVoltageSettings()
        {


            EEPROM_Data_Types<float> liveModeCurrentVal = new EEPROM_Data_Types<float>(0.4f, 0f, 3f);

            Live_Mode_Current_Value = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Single, 4), liveModeCurrentVal);

            EEPROM_Data_Types<byte> liveModeMaxPer = new EEPROM_Data_Types<byte>(50, 0, 100);

            Live_Mode_Max_Percentage = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), liveModeMaxPer);

            EEPROM_Data_Types<byte> liveModeCurrentBoostVal = new EEPROM_Data_Types<byte>(0, 0, 100);

            Live_Mode_Current_Boost_Value = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), liveModeCurrentBoostVal);

            EEPROM_Data_Types<byte> liveModeCurrentBoostEnable = new EEPROM_Data_Types<byte>(0, 0, 1);

            Live_Mode_Current_Boost_Enable = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), liveModeCurrentBoostEnable);

            EEPROM_Data_Types<byte> liveModeMaxIntVal = new EEPROM_Data_Types<byte>(150, byte.MinValue, byte.MaxValue);

            Live_Mode_Max_Int_Value = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), liveModeMaxIntVal);

            EEPROM_Data_Types<float> liveModeVoltageVal = new EEPROM_Data_Types<float>(0f, 0f, 6f);

            Live_Mode_Voltage_Value = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Single, 4), liveModeVoltageVal);


            EEPROM_Data_Types<float> captureModeCurrentVal = new EEPROM_Data_Types<float>(0.4f, 0f, 3f);

            Capture_Mode_Current_Value = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Single, 4), captureModeCurrentVal);

            EEPROM_Data_Types<byte> captureModeMaxPer = new EEPROM_Data_Types<byte>(50, 0, 100);

            Capture_Mode_Max_Percentage = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), captureModeMaxPer);

            EEPROM_Data_Types<byte> captureModeCurrentBoostVal = new EEPROM_Data_Types<byte>(0, 0, 100);

            Capture_Mode_Current_Boost_Value = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), captureModeCurrentBoostVal);

            EEPROM_Data_Types<byte> captureModeCurrentBoostEnable = new EEPROM_Data_Types<byte>(0, 0, 1);

            Capture_Mode_Current_Boost_Enable = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), captureModeCurrentBoostEnable);

            EEPROM_Data_Types<byte> captureModeMaxIntVal = new EEPROM_Data_Types<byte>(150, byte.MinValue, byte.MaxValue);

            Capture_Mode_Max_Int_Value = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), captureModeMaxIntVal);

            EEPROM_Data_Types<float> captureModeVoltageVal = new EEPROM_Data_Types<float>(0f, 0f, 6f);

            Capture_Mode_Voltage_Value = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Single, 4), captureModeVoltageVal);

        }

    }
}
