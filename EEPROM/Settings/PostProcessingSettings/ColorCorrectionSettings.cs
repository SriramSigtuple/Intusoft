using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EEPROM
{
    [Serializable]
    public class ColorCorrectionSettings
    {
        //Post Processing Color Correction
        //public bool isApplyColorCorrection = true;
        //public float RRCompensation = 1f;
        //public float RGCompensation = 0.4f;
        //public float RBCompensation = 0f;
        //public float GRCompensation = 0f;
        //public float GGCompensation = 0.95f;
        //public float GBCompensation = 0f;
        //public float BRCompensation = 0f;
        //public float BGCompensation = 0.2f;
        //public float BBCompensation = 1.5f;

        private EEPROM_Props isApplyColorCorrection = null;

        public EEPROM_Props IsApplyColorCorrection
        {
            get { return isApplyColorCorrection; }
            set { isApplyColorCorrection = value; }
        }
        private EEPROM_Props rRCompensation = null;

        public EEPROM_Props RRCompensation
        {
            get { return rRCompensation; }
            set { rRCompensation = value; }
        }
        private EEPROM_Props rGCompensation = null;

        public EEPROM_Props RGCompensation
        {
            get { return rGCompensation; }
            set { rGCompensation = value; }
        }
        private EEPROM_Props rBCompensation = null;

        public EEPROM_Props RBCompensation
        {
            get { return rBCompensation; }
            set { rBCompensation = value; }
        }
        private EEPROM_Props gRCompensation = null;

        public EEPROM_Props GRCompensation
        {
            get { return gRCompensation; }
            set { gRCompensation = value; }
        }
        private EEPROM_Props gGCompensation = null;

        public EEPROM_Props GGCompensation
        {
            get { return gGCompensation; }
            set { gGCompensation = value; }
        }
        private EEPROM_Props gBCompensation = null;

        public EEPROM_Props GBCompensation
        {
            get { return gBCompensation; }
            set { gBCompensation = value; }
        }
        private EEPROM_Props bRCompensation = null;

        public EEPROM_Props BRCompensation
        {
            get { return bRCompensation; }
            set { bRCompensation = value; }
        }
        private EEPROM_Props bGCompensation = null;

        public EEPROM_Props BGCompensation
        {
            get { return bGCompensation; }
            set { bGCompensation = value; }
        }
        private EEPROM_Props bBCompensation = null;

        public EEPROM_Props BBCompensation
        {
            get { return bBCompensation; }
            set { bBCompensation = value; }
        }
        public ColorCorrectionSettings()
        {

            EEPROM_Data_Types<byte> applyCC = new EEPROM_Data_Types<byte>(1, 0, 1);

            IsApplyColorCorrection = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), applyCC);

            EEPROM_Data_Types<float> rr = new EEPROM_Data_Types<float>(1.3f, -10f, 10f);

            RRCompensation = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Single, 4), rr);

            EEPROM_Data_Types<float> rg = new EEPROM_Data_Types<float>(0.2f, -10f, 10f);

            RGCompensation = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Single,4),rg);

            EEPROM_Data_Types<float> rb = new EEPROM_Data_Types<float>(0f, -10f, 10f);

            RBCompensation = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Single,4),rb);

            EEPROM_Data_Types<float> gr = new EEPROM_Data_Types<float>(0f, -10f, 10f);

            GRCompensation = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Single,4),gr);
            EEPROM_Data_Types<float> gg = new EEPROM_Data_Types<float>(0.95f, -10f, 10f);

            GGCompensation = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Single,4),gg);

            EEPROM_Data_Types<float> gb = new EEPROM_Data_Types<float>(0f, -10f, 10f);

            GBCompensation = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Single,4),gb);
            EEPROM_Data_Types<float> br = new EEPROM_Data_Types<float>(0f, -10f, 10f);

            BRCompensation = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Single,4),br);
            EEPROM_Data_Types<float> bg = new EEPROM_Data_Types<float>(0.2f, -10f, 10f);

            BGCompensation = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Single,4),bg);
            EEPROM_Data_Types<float> bb = new EEPROM_Data_Types<float>(1.3f, -10f, 10f);

            BBCompensation = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Single,4), bb);


        }
    }
}
