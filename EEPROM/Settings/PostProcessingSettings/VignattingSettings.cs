using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEPROM
{
    [Serializable]
    public class VignattingSettings
    {
        private EEPROM_Props isApplyVignatting = null;

        public EEPROM_Props IsApplyVignatting
        {
            get { return isApplyVignatting; }
            set { isApplyVignatting = value; }
        }

        private EEPROM_Props vignattingPercentageFactorLive = null;

        public EEPROM_Props VignattingPercentageFactorLive
        {
            get { return vignattingPercentageFactorLive; }
            set { vignattingPercentageFactorLive = value; }
        }

        private EEPROM_Props vignattingRadiusLive = null;

        public EEPROM_Props VignattingRadiusLive
        {
            get { return vignattingRadiusLive; }
            set { vignattingRadiusLive = value; }
        }

        private EEPROM_Props vignattingPercentageFactorPostProcessing = null;

        public EEPROM_Props VignattingPercentageFactorPostProcessing
        {
            get { return vignattingPercentageFactorPostProcessing; }
            set { vignattingPercentageFactorPostProcessing = value; }
        }

        private EEPROM_Props vignattingRadiusPostProcessing = null;

        public EEPROM_Props VignattingRadiusPostProcessing
        {
            get { return vignattingRadiusPostProcessing; }
            set { vignattingRadiusPostProcessing = value; }
        }

        private EEPROM_Props applyLiveParabola = null;

        public EEPROM_Props ApplyLiveParabola
        {
            get { return applyLiveParabola; }
            set { applyLiveParabola = value; }
        }

        public VignattingSettings()
        {

            EEPROM_Data_Types<byte> applyVignate = new EEPROM_Data_Types<byte>(1, 0, 1);

            IsApplyVignatting = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), applyVignate);

            EEPROM_Data_Types<byte> applyLiveParabola = new EEPROM_Data_Types<byte>(1, 0, 1);

            ApplyLiveParabola = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo( DataTypes.Byte,1), applyLiveParabola);

            EEPROM_Data_Types<Int16> vignateRadLive = new EEPROM_Data_Types<Int16>(800, 0, 10000);

            VignattingRadiusLive = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), vignateRadLive);

            EEPROM_Data_Types<Int16> vignatePercentageFactLive = new EEPROM_Data_Types<Int16>(800, 0, 10000);

            VignattingPercentageFactorLive = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), vignatePercentageFactLive);

            EEPROM_Data_Types<Int16> vignateRadiusPostProcessing = new EEPROM_Data_Types<Int16>(1000, 0, 10000);

            VignattingRadiusPostProcessing = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), vignateRadiusPostProcessing);


            EEPROM_Data_Types<Int16> vignatePercentageFactorPostProcessing = new EEPROM_Data_Types<Int16>(1, 0, 10000);

            VignattingPercentageFactorPostProcessing = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo( DataTypes.Int16,2), vignatePercentageFactorPostProcessing);

        }
    }
}
