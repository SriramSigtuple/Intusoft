using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EEPROM
{
     [Serializable]
    public class PostProcessingSettings
    {

        private EEPROM_Props imageShiftSettings;

        public EEPROM_Props ImageShiftSettings
        {
            get { return imageShiftSettings; }
            set { imageShiftSettings = value; }
        }
        private EEPROM_Props vignattingSettings;

        public EEPROM_Props VignattingSettings
        {
            get { return vignattingSettings; }
            set { vignattingSettings = value; }
        }
        private EEPROM_Props maskSettings;

        public EEPROM_Props MaskSettings
        {
            get { return maskSettings; }
            set { maskSettings = value; }
        }
        private EEPROM_Props colorCorrectionSettings;

        public EEPROM_Props ColorCorrectionSettings
        {
            get { return colorCorrectionSettings; }
            set { colorCorrectionSettings = value; }
        }
        private EEPROM_Props hotSpotSettings;

        public EEPROM_Props HotSpotSettings
        {
            get { return hotSpotSettings; }
            set { hotSpotSettings = value; }
        }
        private EEPROM_Props unsharpMaskSettings;

        public EEPROM_Props UnsharpMaskSettings
        {
            get { return unsharpMaskSettings; }
            set { unsharpMaskSettings = value; }
        }
        private EEPROM_Props claheSettings;

        public EEPROM_Props ClaheSettings
        {
            get { return claheSettings; }
            set { claheSettings = value; }
        }
        private EEPROM_Props lutSettings;

        public EEPROM_Props LutSettings
        {
            get { return lutSettings; }
            set { lutSettings = value; }
        }
        private EEPROM_Props brightnessContrastSettings;

        public EEPROM_Props BrightnessContrastSettings
        {
            get { return brightnessContrastSettings; }
            set { brightnessContrastSettings = value; }
        }
        public PostProcessingSettings()
        {
            ImageShiftSettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Tree,StrutureTypes.ImageShiftSettings.ToByte<StrutureTypes>()), "");
            ImageShiftSettings.value = new EEPROM.ImageShiftSettings();
            VignattingSettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Tree, StrutureTypes.VignattingSettings.ToByte<StrutureTypes>()), "");

            VignattingSettings.value = new EEPROM.VignattingSettings();
            MaskSettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Tree, StrutureTypes.MaskSettings.ToByte<StrutureTypes>()),  "");

            MaskSettings.value = new EEPROM.MaskSettings();
            ColorCorrectionSettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Tree, StrutureTypes.ColorCorrectionSettings.ToByte<StrutureTypes>()),  "");

            ColorCorrectionSettings.value = new EEPROM.ColorCorrectionSettings();
            HotSpotSettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Tree, StrutureTypes.HotSpotCorrectionSettings.ToByte<StrutureTypes>()), "");

            HotSpotSettings.value = new EEPROM.HotSpotSettings();
            UnsharpMaskSettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Tree, StrutureTypes.UnsharpMaskSettings.ToByte<StrutureTypes>()), "");

            UnsharpMaskSettings.value = new EEPROM.UnsharpMaskSettings();

            ClaheSettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Tree, StrutureTypes.ClaheSettings.ToByte<StrutureTypes>()), "");

            ClaheSettings.value = new EEPROM.ClaheSettings();
            LutSettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Tree, StrutureTypes.LutSettings.ToByte<StrutureTypes>()), "");

            LutSettings.value = new EEPROM.LutSettings();
            BrightnessContrastSettings = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Tree, StrutureTypes.BrightnessContrastSettings.ToByte<StrutureTypes>()), "");
            BrightnessContrastSettings.value = new EEPROM.BrightnessContrastSettings();
        }
    }
   
   

  
   
    
    

}
