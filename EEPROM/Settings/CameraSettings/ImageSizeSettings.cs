using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEPROM
{
  public  class ImageSizeSettings
    {
      
        private  EEPROM_Props liveImageWidth = null;

        public EEPROM_Props LiveImageWidth
        {
            get { return liveImageWidth; }
            set { liveImageWidth = value; }
        }
        private  EEPROM_Props liveImageHeight = null;

        public EEPROM_Props LiveImageHeight
        {
            get { return liveImageHeight; }
            set { liveImageHeight = value; }
        }

        private EEPROM_Props captureImageWidth = null;

        public EEPROM_Props CaptureImageWidth
        {
            get { return captureImageWidth; }
            set { captureImageWidth = value; }
        }
        private EEPROM_Props captureImageHeight = null;

        public EEPROM_Props CaptureImageHeight
        {
            get { return captureImageHeight; }
            set { captureImageHeight = value; }
        }
      private  EEPROM_Props imageOpticalCentreX = null;

        public EEPROM_Props ImageOpticalCentreX
        {
            get { return imageOpticalCentreX; }
            set { imageOpticalCentreX = value; }
        }
       private  EEPROM_Props imageOpticalCentreY = null;

        public EEPROM_Props ImageOpticalCentreY
        {
            get { return imageOpticalCentreY; }
            set { imageOpticalCentreY = value; }
        }

        private EEPROM_Props liveImageROIY = null;

        public EEPROM_Props LiveImageROIY
        {
            get { return liveImageROIY; }
            set { liveImageROIY = value; }
        }

        private EEPROM_Props liveImageROIX = null;

        public EEPROM_Props LiveImageROIX
        {
            get { return liveImageROIX; }
            set { liveImageROIX = value; }
        }

        private EEPROM_Props captureImageROIY = null;

        public EEPROM_Props CaptureImageROIY
        {
            get { return captureImageROIY; }
            set { captureImageROIY = value; }
        }

        private EEPROM_Props captureImageROIX = null;

        public EEPROM_Props CaptureImageROIX
        {
            get { return captureImageROIX; }
            set { captureImageROIX = value; }
        }
        public ImageSizeSettings()
        {

            EEPROM_Data_Types<Int16> liveImageWidth = new EEPROM_Data_Types<Int16>(3072, 0, 10000);

            LiveImageWidth = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), liveImageWidth); ;

            EEPROM_Data_Types<Int16> liveImageHeight = new EEPROM_Data_Types<Int16>(2048, 0, 10000);

            LiveImageHeight = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), liveImageHeight); ;

            EEPROM_Data_Types<Int16> liveImageROIX = new EEPROM_Data_Types<Int16>(270, 0, 10000);

            LiveImageROIX = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), liveImageROIX); ;

            EEPROM_Data_Types<Int16> liveImageROIY = new EEPROM_Data_Types<Int16>(270, 0, 10000);

            LiveImageROIY = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), liveImageROIY); ;

            EEPROM_Data_Types<Int16> captureImageWidth = new EEPROM_Data_Types<Int16>(3072, 0, 10000);

            CaptureImageWidth = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), captureImageWidth); ;

            EEPROM_Data_Types<Int16> captureImageHeight = new EEPROM_Data_Types<Int16>(2048, 0, 10000);

            CaptureImageHeight = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2), captureImageHeight); ;

            EEPROM_Data_Types<Int16> captureImageROIX = new EEPROM_Data_Types<Int16>(270, 0, 10000);

            CaptureImageROIX = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo( DataTypes.Int16,2), captureImageROIX); ;

            EEPROM_Data_Types<Int16> captureImageROIY = new EEPROM_Data_Types<Int16>(270, 0, 10000);

            CaptureImageROIY = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo( DataTypes.Int16,2),captureImageROIY); ;

            EEPROM_Data_Types<Int16> imageOpticalCentreX = new EEPROM_Data_Types<Int16>(1536, 0, 10000);

            ImageOpticalCentreX = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo( DataTypes.Int16,2), imageOpticalCentreX); ;

            EEPROM_Data_Types<Int16> imageOpticalCentreY = new EEPROM_Data_Types<Int16>(1024, 0, 10000);

            ImageOpticalCentreY = EEPROM_Props.CreateIVLProperties(EEPROM_Type_Info.CreateEEPROM_TypeInfo(DataTypes.Int16, 2),imageOpticalCentreY); ;

      }
    }
}
