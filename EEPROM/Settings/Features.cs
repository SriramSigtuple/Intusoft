using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEPROM
{
   public class Features
    {
        private EEPROM_Props anteriorEnable;

        public EEPROM_Props AnteriorEnable
        {
            get { return anteriorEnable; }
            set { anteriorEnable = value; }
        }
        private EEPROM_Props anteriorBlueEnable;

        public EEPROM_Props AnteriorBlueEnable
        {
            get { return anteriorBlueEnable; }
            set { anteriorBlueEnable = value; }
        }
        private EEPROM_Props posteriorEnable;

        public EEPROM_Props PosteriorEnable
        {
            get { return posteriorEnable; }
            set { posteriorEnable = value; }
        }

        private EEPROM_Props fFAEnable;

        public EEPROM_Props FFAEnable
        {
            get { return fFAEnable; }
            set { fFAEnable = value; }
        }
       public Features()
       {

           EEPROM_Data_Types<byte> enableAnterior = new EEPROM_Data_Types<byte>(1, 0, 1);

           AnteriorEnable = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), enableAnterior);

           EEPROM_Data_Types<byte> enableAnteriorBlue = new EEPROM_Data_Types<byte>(1, 0, 1);

           AnteriorBlueEnable = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), enableAnteriorBlue);

           EEPROM_Data_Types<byte> enablePosterior = new EEPROM_Data_Types<byte>(1, 0, 1);

           PosteriorEnable = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), enablePosterior);

           EEPROM_Data_Types<byte> enableFFA = new EEPROM_Data_Types<byte>(1, 0, 1);

           fFAEnable = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), enableFFA);
       }
    }
}
