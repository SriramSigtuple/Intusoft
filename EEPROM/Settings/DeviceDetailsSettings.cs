using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace EEPROM
{
    [Serializable]
    public class DeviceDetailsSettings
    {


        private EEPROM_Props device_number;

        public EEPROM_Props Device_number
        {
            get { return device_number; }
            set { device_number = value; }
        }
        private EEPROM_Props model;

        public EEPROM_Props Model
        {
            get { return model; }
            set { model = value; }
        }
        private EEPROM_Props production_batch;

        public EEPROM_Props Production_batch
        {
            get { return production_batch; }
            set { production_batch = value; }
        }
        private EEPROM_Props optics_Version;

        public EEPROM_Props Optics_Version
        {
            get { return optics_Version; }
            set { optics_Version = value; }
        }
        private EEPROM_Props camera_model;

        public EEPROM_Props Camera_model
        {
            get { return camera_model; }
            set { camera_model = value; }
        }


        public DeviceDetailsSettings()
        {

            byte[] deviceNumberArr = new byte[16];
            Array.Copy(Encoding.UTF8.GetBytes("01-1701-0000"), deviceNumberArr, Encoding.UTF8.GetBytes("01-1701-0000").Length);
            Device_number = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.ByteArr, 12), "01-1701-0000");

            byte[] modelArr = new byte[16];
            Array.Copy(Encoding.UTF8.GetBytes("INTUCAM-45"), modelArr, Encoding.UTF8.GetBytes("INTUCAM-45").Length);
            Model = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.ByteArr, 16), "INTUCAM-45");

            byte[] prodBactchArr = new byte[8];
            Array.Copy(Encoding.UTF8.GetBytes("1701"), prodBactchArr, Encoding.UTF8.GetBytes("1701").Length);
            Production_batch = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.ByteArr, 8), "1701");

            byte[] opticsVerArr = new byte[8]; Encoding.UTF8.GetBytes("1.0.A");
            Array.Copy(Encoding.UTF8.GetBytes("1.0.A"), opticsVerArr, Encoding.UTF8.GetBytes("1.0.A").Length);
            Optics_Version = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.ByteArr, 8), "1.0.A");

            byte[] camModelArr = new byte[8];
            Array.Copy(Encoding.UTF8.GetBytes("V1M1"), camModelArr, Encoding.UTF8.GetBytes("V1M1").Length);// = (Vendor 1, Model 1)");
            Camera_model = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.ByteArr, 8), "V1M1");

        }
    }
}
