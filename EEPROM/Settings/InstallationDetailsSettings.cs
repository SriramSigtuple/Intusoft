using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EEPROM
{
    [Serializable]
    public class InstallationDetailsSettings
    {

        private EEPROM_Props installation_Site;

        public EEPROM_Props Installation_Site
        {
            get { return installation_Site; }
            set { installation_Site = value; }
        }
        private EEPROM_Props installationCity;

        public EEPROM_Props InstallationCity
        {
            get { return installationCity; }
            set { installationCity = value; }
        }
        private EEPROM_Props installationPinCode;

        public EEPROM_Props InstallationPinCode
        {
            get { return installationPinCode; }
            set { installationPinCode = value; }
        }
        private EEPROM_Props installationState;

        public EEPROM_Props InstallationState
        {
            get { return installationState; }
            set { installationState = value; }
        }
        private EEPROM_Props installationCountry;

        public EEPROM_Props InstallationCountry
        {
            get { return installationCountry; }
            set { installationCountry = value; }
        }
        private EEPROM_Props contactNumber;

        public EEPROM_Props ContactNumber
        {
            get { return contactNumber; }
            set { contactNumber = value; }
        }
        private EEPROM_Props contactEmail;

        public EEPROM_Props ContactEmail
        {
            get { return contactEmail; }
            set { contactEmail = value; }
        }

        public InstallationDetailsSettings()
        {

            byte[] emailArr = new byte[64];
            Array.Copy(Encoding.UTF8.GetBytes("ivl@intuvisionlabs.com"), emailArr, Encoding.UTF8.GetBytes("ivl@intuvisionlabs.com").Length);
            ContactEmail = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.ByteArr, 64), "ivl@intuvisionlabs.com");

            
            byte[] numberArr = new byte[40];
            Array.Copy(Encoding.UTF8.GetBytes("9739460868"), numberArr, Encoding.UTF8.GetBytes("9739460868").Length);
            ContactNumber = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.ByteArr, 40), "9739460868");
            
            byte[] countryArr = new byte[24];
            Array.Copy(Encoding.UTF8.GetBytes("India"), countryArr, Encoding.UTF8.GetBytes("India").Length);
            InstallationCountry = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.ByteArr, 24), "India");
            
            byte[] stateArr = new byte[32];// Encoding.UTF8.GetBytes("Karnataka");
            Array.Copy(Encoding.UTF8.GetBytes("Karnataka"), stateArr, Encoding.UTF8.GetBytes("Karnataka").Length);
            InstallationState = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.ByteArr, 32), "Karnataka");

            byte[] pincodeArr = new byte[32];// Encoding.UTF8.GetBytes("560085");
            Array.Copy(Encoding.UTF8.GetBytes("560085"), pincodeArr, Encoding.UTF8.GetBytes("560085").Length);
            InstallationPinCode = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.ByteArr, 32), "560085");
            
            byte[] cityArr = new byte[24];// Encoding.UTF8.GetBytes("Bangalore");
            Array.Copy(Encoding.UTF8.GetBytes("Bangalore"), cityArr, Encoding.UTF8.GetBytes("Bangalore").Length);
            InstallationCity = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.ByteArr, 24), "Bangalore");
            
            byte[] siteArr = new byte[40];// Encoding.UTF8.GetBytes("Girinagar");
            Array.Copy(Encoding.UTF8.GetBytes("Girinagar"), emailArr, Encoding.UTF8.GetBytes("Girinagar").Length);
            Installation_Site = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.ByteArr, 40), "Girinagar");

        }

    }
}
