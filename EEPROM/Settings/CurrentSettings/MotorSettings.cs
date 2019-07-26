using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEPROM
{
    public class MotorSettings
    {
        private EEPROM_Props motorCompensationStepsIR2Flash;

        public EEPROM_Props MotorCompensationStepsIR2Flash
        {
            get { return motorCompensationStepsIR2Flash; }
            set { motorCompensationStepsIR2Flash = value; }
        }
        private EEPROM_Props motor_Steps_Range;

        public EEPROM_Props Motor_Steps_Range
        {
            get { return motor_Steps_Range; }
            set { motor_Steps_Range = value; }
        }
        private EEPROM_Props motor_Reset_Position;

        public EEPROM_Props Motor_Reset_Position
        {
            get { return motor_Reset_Position; }
            set { motor_Reset_Position = value; }
        }
        private EEPROM_Props motor_ZeroD_Position;

        public EEPROM_Props Motor_ZeroD_Position
        {
            get { return motor_ZeroD_Position; }
            set { motor_ZeroD_Position = value; }
        }
        private EEPROM_Props motor_EndPoint_Forward;

        public EEPROM_Props Motor_EndPoint_Forward
        {
            get { return motor_EndPoint_Forward; }
            set { motor_EndPoint_Forward = value; }
        }
        private EEPROM_Props motor_EndPoint_Backward;

        public EEPROM_Props Motor_EndPoint_Backward
        {
            get { return motor_EndPoint_Backward; }
            set { motor_EndPoint_Backward = value; }
        }

        private EEPROM_Props motorPlySteps = null;

        public EEPROM_Props MotorPlySteps
        {
            get { return motorPlySteps; }
            set { motorPlySteps = value; }
        }
        private EEPROM_Props isApplyPly = null;

        public EEPROM_Props IsApplyPly
        {
            get { return isApplyPly; }
            set { isApplyPly = value; }
        }

        private EEPROM_Props isMotorCompensation = null;

        public EEPROM_Props IsMotorCompensation
        {
            get { return isMotorCompensation; }
            set { isMotorCompensation = value; }
        }

        private EEPROM_Props isMotorPolarityForward = null;

        public EEPROM_Props IsMotorPolarityForward
        {
            get { return isMotorPolarityForward; }
            set { isMotorPolarityForward = value; }
        }
        public MotorSettings ()
	    {
            EEPROM_Data_Types<byte> applyPly = new EEPROM_Data_Types<byte>(1, 0, 1);

            IsApplyPly = EEPROM_Props.CreateIVLProperties( EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte,1), applyPly);

            EEPROM_Data_Types<byte> PolarityForward= new EEPROM_Data_Types<byte>(1, 0, 1);

            IsMotorPolarityForward = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte,1), PolarityForward);

            EEPROM_Data_Types<byte> isMotorCompensation = new EEPROM_Data_Types<byte>(1, 0, 1);

            IsMotorCompensation = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte,1), isMotorCompensation);

            EEPROM_Data_Types<byte> IR2FlashSteps = new EEPROM_Data_Types<byte>(1, 0, 1);

            MotorCompensationStepsIR2Flash = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte,1), IR2FlashSteps);

            EEPROM_Data_Types<Int16> motorEndBack= new EEPROM_Data_Types<Int16>(400, 0, 10000);

            Motor_EndPoint_Backward = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16,2), motorEndBack);
            EEPROM_Data_Types<Int16> motorEndForw = new EEPROM_Data_Types<Int16>(600, 0, 10000);
        
            Motor_EndPoint_Forward = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16,2), motorEndForw);

            EEPROM_Data_Types<Int16> motorZeroD = new EEPROM_Data_Types<Int16>(0, 0, 10000);

            Motor_ZeroD_Position = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16,2), motorZeroD);
            EEPROM_Data_Types<Int16> motorResetPos = new EEPROM_Data_Types<Int16>(0, 0, 10000);
   
            Motor_Reset_Position = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16,2),motorResetPos);
            EEPROM_Data_Types<Int16> motorStepsRange = new EEPROM_Data_Types<Int16>(0, 0, 10000);

            Motor_Steps_Range = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16,2),motorStepsRange);

            EEPROM_Data_Types<byte> plySetps = new EEPROM_Data_Types<byte>(1, 0, byte.MaxValue);
            MotorPlySteps = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte,1), plySetps);

        }

    }
}
