using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EEPROM
{
    [Serializable]
    public class HotSpotSettings
    {
       
        private EEPROM_Props shadowSettings = null;

        public EEPROM_Props ShadowSettings
        {
            get { return shadowSettings; }
            set { shadowSettings = value; }
        }

        private EEPROM_Props hSCorrectionSettings = null;

        public EEPROM_Props HSCorrectionSettings
        {
            get { return hSCorrectionSettings; }
            set { hSCorrectionSettings = value; }
        }

        public HotSpotSettings()
        {

            ShadowSettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Tree, StrutureTypes.ShadowCorrectionSettings.ToByte<StrutureTypes>()), "", "0");
            ShadowSettings.value = new ShadowCorrectionSettings();

            HSCorrectionSettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Tree, StrutureTypes.HotSpotCorrectionSettings.ToByte<StrutureTypes>()), "", "0");
            HSCorrectionSettings.value = new HotSpotCorrectionSettings();
        
         
        
        }
    }

    public class HotSpotCorrectionSettings
    {
        private EEPROM_Props isApplyHotspotCorrection = null;

        public EEPROM_Props IsApplyHotspotCorrection
        {
            get { return isApplyHotspotCorrection; }
            set { isApplyHotspotCorrection = value; }
        }

        private EEPROM_Props gainSlope = null;

        public EEPROM_Props GainSlope
        {
            get { return gainSlope; }
            set { gainSlope = value; }
        }
        private EEPROM_Props hotSpotRedPeak = null;

        public EEPROM_Props HotSpotRedPeak
        {
            get { return hotSpotRedPeak; }
            set { hotSpotRedPeak = value; }
        }

        private EEPROM_Props hotSpotGreenPeak = null;

        public EEPROM_Props HotSpotGreenPeak
        {
            get { return hotSpotGreenPeak; }
            set { hotSpotGreenPeak = value; }
        }
        private EEPROM_Props hotSpotBluePeak = null;

        public EEPROM_Props HotSpotBluePeak
        {
            get { return hotSpotBluePeak; }
            set { hotSpotBluePeak = value; }
        }

        private EEPROM_Props hotSpotRedRadius = null;

        public EEPROM_Props HotSpotRedRadius
        {
            get { return hotSpotRedRadius; }
            set { hotSpotRedRadius = value; }
        }
        private EEPROM_Props hotSpotGreenRadius = null;

        public EEPROM_Props HotSpotGreenRadius
        {
            get { return hotSpotGreenRadius; }
            set { hotSpotGreenRadius = value; }
        }

        private EEPROM_Props hotSpotBlueRadius = null;

        public EEPROM_Props HotSpotBlueRadius
        {
            get { return hotSpotBlueRadius; }
            set { hotSpotBlueRadius = value; }
        }

        public HotSpotCorrectionSettings()
        {
            EEPROM_Data_Types<byte> applyHS = new EEPROM_Data_Types<byte>(0, byte.MinValue, 1);
            IsApplyHotspotCorrection = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), applyHS);


            EEPROM_Data_Types<byte> gainSlope = new EEPROM_Data_Types<byte>(5, byte.MinValue, byte.MaxValue);
            GainSlope = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), gainSlope);

            EEPROM_Data_Types<byte> redPeak = new EEPROM_Data_Types<byte>(0, byte.MinValue, byte.MaxValue);
            HotSpotRedPeak = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), redPeak);

            EEPROM_Data_Types<byte> greenPeak = new EEPROM_Data_Types<byte>(0, byte.MinValue, byte.MaxValue);
            HotSpotGreenPeak = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), greenPeak);

            EEPROM_Data_Types<byte> bluePeak = new EEPROM_Data_Types<byte>(0, byte.MinValue, byte.MaxValue);
            HotSpotBluePeak = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), bluePeak);


            EEPROM_Data_Types<Int16> hsRedRad = new EEPROM_Data_Types<Int16>(0, 0, 1000);
            HotSpotRedRadius = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 1), hsRedRad);

            EEPROM_Data_Types<Int16> hsGreenRad = new EEPROM_Data_Types<Int16>(0, 0, 1000);
            HotSpotGreenRadius = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 1), hsGreenRad);

            EEPROM_Data_Types<Int16> hsBlueRad = new EEPROM_Data_Types<Int16>(0, 0, 1000);
            HotSpotBlueRadius = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 1), hsBlueRad);
        }
    }

    public class ShadowCorrectionSettings
    {

        private EEPROM_Props shadowRedPercentage = null;

        public EEPROM_Props ShadowRedPercentage
        {
            get { return shadowRedPercentage; }
            set { shadowRedPercentage = value; }
        }

        private EEPROM_Props shadowGreenPercentage = null;

        public EEPROM_Props ShadowGreenPercentage
        {
            get { return shadowGreenPercentage; }
            set { shadowGreenPercentage = value; }
        }

        private EEPROM_Props shadowBluePercentage = null;

        public EEPROM_Props ShadowBluePercentage
        {
            get { return shadowBluePercentage; }
            set { shadowBluePercentage = value; }
        }

        private EEPROM_Props hotspotRadius1 = null;

        public EEPROM_Props HotspotRadius1
        {
            get { return hotspotRadius1; }
            set { hotspotRadius1 = value; }
        }
        private EEPROM_Props hotspotRadius2 = null;

        public EEPROM_Props HotspotRadius2
        {
            get { return hotspotRadius2; }
            set { hotspotRadius2 = value; }
        }
        private EEPROM_Props shadowRadSpot1 = null;

        public EEPROM_Props ShadowRadSpot1
        {
            get { return shadowRadSpot1; }
            set { shadowRadSpot1 = value; }
        }
        private EEPROM_Props shadowRadspot2 = null;

        public EEPROM_Props ShadowRadSpot2
        {
            get { return shadowRadspot2; }
            set { shadowRadspot2 = value; }
        }
        public ShadowCorrectionSettings()
        {

            EEPROM_Data_Types<Int16> hsRad1 = new EEPROM_Data_Types<Int16>(-20, -1000, 1000);
            
            HotspotRadius1 = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), hsRad1);

            EEPROM_Data_Types<Int16> hsRad2 = new EEPROM_Data_Types<Int16>(100, -1000, 1000);

            HotspotRadius2 = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), hsRad2);


            EEPROM_Data_Types<Int16> shadowRad1 = new EEPROM_Data_Types<Int16>(170, -1000, 1000);

            ShadowRadSpot1 = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), shadowRad1);
            
            EEPROM_Data_Types<Int16> shadowRad2 = new EEPROM_Data_Types<Int16>(400, -1000, 1000);

            ShadowRadSpot2 = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), shadowRad2);


            EEPROM_Data_Types<byte> redPer = new EEPROM_Data_Types<byte>(20, byte.MinValue, byte.MaxValue);

            ShadowRedPercentage = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1),redPer);

            EEPROM_Data_Types<byte> greenPer = new EEPROM_Data_Types<byte>(15, byte.MinValue, byte.MaxValue);

            ShadowGreenPercentage = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1), greenPer);

            EEPROM_Data_Types<byte> bluePer = new EEPROM_Data_Types<byte>(10, byte.MinValue, byte.MaxValue);
             
            ShadowBluePercentage = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Byte, 1),bluePer);

        }
    }
}
