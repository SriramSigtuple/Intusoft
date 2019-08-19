using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace INTUSOFT.Configuration.AdvanceSettings
{
    [Serializable]
    public class ImageStorageSettings
    {
        //public bool isPng = true;
        //public string LocalStoragePath  = @"C:\";
        //public bool IslocalStorange = true;
        //public bool IsRemoteStorage = false;
        //public int ArchivingPeriodicity = 1;
        //public string NumberingFormat = "";
        private static IVLControlProperties dbPath = null;

        public IVLControlProperties _dbPath
        {
            get { return dbPath; }
            set { dbPath = value; }
        }
        private static IVLControlProperties compressionRatio = null;

        public IVLControlProperties _compressionRatio
        {
            get { return compressionRatio; }
            set { compressionRatio = value; }
        }
        private static IVLControlProperties dbName = null;

        public IVLControlProperties _dbName
        {
            get { return dbName; }
            set { dbName = value; }
        }
        private static IVLControlProperties isPng = null;


        public IVLControlProperties _IsPng
        {
            get { return isPng; }
            set { isPng = value; }
        }

        private static IVLControlProperties ImageSaveFormat = null;


        public IVLControlProperties _ImageSaveFormat
        {
            get { return ImageSaveFormat; }
            set { ImageSaveFormat = value; }
        }
        private static IVLControlProperties LocalStoragePath = null;

        public IVLControlProperties _LocalStoragePath
        {
            get { return LocalStoragePath; }
            set { LocalStoragePath = value; }
        }
        private static IVLControlProperties LocalProcessedImagePath = null;

        public IVLControlProperties _LocalProcessedImagePath
        {
            get { return LocalProcessedImagePath; }
            set { LocalProcessedImagePath = value; }
        }

        private static IVLControlProperties ExportImagePath = null;

        public IVLControlProperties _ExportImagePath
        {
            get { return ExportImagePath; }
            set { ExportImagePath = value; }
        }


        private static IVLControlProperties IslocalStorange = null;

        public IVLControlProperties _IslocalStorange
        {
            get { return IslocalStorange; }
            set { IslocalStorange = value; }
        }
        private static IVLControlProperties IsRemoteStorage = null;

        public IVLControlProperties _IsRemoteStorage
        {
            get { return IsRemoteStorage; }
            set { IsRemoteStorage = value; }
        }

        private static IVLControlProperties ArchivingPeriodicity = null;

        public IVLControlProperties _ArchivingPeriodicity
        {
            get { return ArchivingPeriodicity; }
            set { ArchivingPeriodicity = value; }
        }
        private static IVLControlProperties NumberingFormat = null;

        public IVLControlProperties _NumberingFormat
        {
            get { return NumberingFormat; }
            set { NumberingFormat = value; }
        }
        private static IVLControlProperties isShowExtViewer = null;

        public IVLControlProperties IsShowExtViewer
        {
            get { return ImageStorageSettings.isShowExtViewer; }
            set { ImageStorageSettings.isShowExtViewer = value; }
        }

        private static IVLControlProperties isIrSave = null;

        public IVLControlProperties _IsIrSave
        {
            get { return isIrSave; }
            set { isIrSave = value; }
        }
        private static IVLControlProperties isRawSave = null;

        public IVLControlProperties _IsRawSave
        {
            get { return isRawSave; }
            set { isRawSave = value; }
        }

        private static IVLControlProperties isRawImageSave = null;

        public IVLControlProperties _IsRawImageSave
        {
            get { return isRawImageSave; }
            set { isRawImageSave = value; }
        }

        private static IVLControlProperties isProcessedImageSave = null;

        public IVLControlProperties _IsProcessedImageSave
        {
            get { return isProcessedImageSave; }
            set { isProcessedImageSave = value; }
        }

        private static IVLControlProperties isMrnFolder = null;

        public IVLControlProperties IsMrnFolder
        {
            get { return isMrnFolder; }
            set { isMrnFolder = value; }
        }

        //private static IVLControlProperties mrnSavePath = null;

        //public IVLControlProperties MRNSavePath
        //{
        //    get { return mrnSavePath; }
        //    set { mrnSavePath = value; }
        //}


        public ImageStorageSettings()
        {
            _IsPng = new IVLControlProperties();
            _IsPng.name = "FlashExposureVisble";
            _IsPng.val = true.ToString();
            _IsPng.type = "bool";
            _IsPng.control = "System.Windows.Forms.RadioButton";
            _IsPng.text = "Png";

            _IslocalStorange = new IVLControlProperties();
            _IslocalStorange.name = "IslocalStorange";
            _IslocalStorange.val = true.ToString();
            _IslocalStorange.type = "bool";
            _IslocalStorange.control = "System.Windows.Forms.RadioButton";
            _IslocalStorange.text = "Local Storage";


            _IsRemoteStorage = new IVLControlProperties();
            _IsRemoteStorage.name = "IsRemoteStorage";
            _IsRemoteStorage.val = false.ToString();
            _IsRemoteStorage.type = "bool";
            _IsRemoteStorage.control = "System.Windows.Forms.RadioButton";
            _IsRemoteStorage.text = "Remote Storage";


            _LocalStoragePath = new IVLControlProperties();
            _LocalStoragePath.name = "LocalStoragePath";
            _LocalStoragePath.type = "string";
            _LocalStoragePath.val = @"C:\IVLImageRepo\Images";
            _LocalStoragePath.control = "System.Windows.Forms.TextBox";
            _LocalStoragePath.text = "Local Storage Path";

            _ExportImagePath = new IVLControlProperties();
            _ExportImagePath.name = "ExportImagePath";
            _ExportImagePath.type = "string";
            _ExportImagePath.val = @"C:\Users\IVL-Anoop\Desktop";
            _ExportImagePath.control = "System.Windows.Forms.TextBox";
            _ExportImagePath.text = "Export Image Path";

            _LocalProcessedImagePath = new IVLControlProperties();
            _LocalProcessedImagePath.name = "LocalProcessedImagePath";
            _LocalProcessedImagePath.type = "string";
            _LocalProcessedImagePath.val = @"C:\IVLImageRepo\Images";
            _LocalProcessedImagePath.control = "System.Windows.Forms.TextBox";
            _LocalProcessedImagePath.text = "Local Processed Image Path";

          

            _NumberingFormat = new IVLControlProperties();
            _NumberingFormat.name = "NumberingFormat";
            _NumberingFormat.type = "string";
            _NumberingFormat.val = "";
            _NumberingFormat.control = "System.Windows.Forms.TextBox";
            _NumberingFormat.text = "Numbering Format";



            _ArchivingPeriodicity = new IVLControlProperties();
            _ArchivingPeriodicity.name = "ArchivingPeriodicity";
            _ArchivingPeriodicity.type = "int";
            _ArchivingPeriodicity.val = "1";
            _ArchivingPeriodicity.control = "System.Windows.Forms.NumericUpDown";
            _ArchivingPeriodicity.text = "Archiving Periodicity";


            _dbName = new IVLControlProperties();
            _dbName.name = "dbName";
            _dbName.type = "string";
            _dbName.val = "patient";
            _dbName.control = "System.Windows.Forms.ComboBox";
            _dbName.text = "Database Name";
            _dbName.range = "patient,demo";

            _compressionRatio = new IVLControlProperties();
            _compressionRatio.name = "compressionRatio";
            _compressionRatio.type = "int";
            _compressionRatio.val = "95";
            _compressionRatio.control = "System.Windows.Forms.ComboBox";
            _compressionRatio.text = "Compression Ratio";
            _compressionRatio.range = "50,55,60,65,70,75,80,85,90,95,100";

            _ImageSaveFormat = new IVLControlProperties();
            _ImageSaveFormat.name = "ImageSaveFormat";
            _ImageSaveFormat.type = "string";
            _ImageSaveFormat.val = "Jpg";
            _ImageSaveFormat.control = "System.Windows.Forms.ComboBox";
            _ImageSaveFormat.text = "Image Save Format";
            _ImageSaveFormat.range = "Jpg, Png, Bmp, Tiff ";

            _dbPath = new IVLControlProperties();
            _dbPath.name = "dbPath";
            _dbPath.type = "string";
            _dbPath.val = @"C:\"; ;
            _dbPath.control = "System.Windows.Forms.TextBox";
            _dbPath.text = "Database Path";
            _dbName.length = 40;

            IsShowExtViewer = new IVLControlProperties();
            IsShowExtViewer.name = "IsShowExtViewer";
            IsShowExtViewer.val = false.ToString();
            IsShowExtViewer.type = "bool";
            IsShowExtViewer.control = "System.Windows.Forms.RadioButton";
            IsShowExtViewer.text = "Is Show Ext Viewer";

            _IsIrSave = new IVLControlProperties();
            _IsIrSave.name = "isIrSave";
            _IsIrSave.val = true.ToString();
            _IsIrSave.type = "bool";
            _IsIrSave.control = "System.Windows.Forms.RadioButton";
            _IsIrSave.text = "Save IR";

            _IsRawSave = new IVLControlProperties();
            _IsRawSave.name = "isRawSave";
            _IsRawSave.val = true.ToString();
            _IsRawSave.type = "bool";
            _IsRawSave.control = "System.Windows.Forms.RadioButton";
            _IsRawSave.text = "Save Raw";


            _IsProcessedImageSave = new IVLControlProperties();
            _IsProcessedImageSave.name = "isProcessedImageSave";
            _IsProcessedImageSave.val = true.ToString();
            _IsProcessedImageSave.type = "bool";
            _IsProcessedImageSave.control = "System.Windows.Forms.RadioButton";
            _IsProcessedImageSave.text = "Save Processed Image";

            _IsRawImageSave = new IVLControlProperties();
            _IsRawImageSave.name = "isRawImageSave";
            _IsRawImageSave.val = true.ToString();
            _IsRawImageSave.type = "bool";
            _IsRawImageSave.control = "System.Windows.Forms.RadioButton";
            _IsRawImageSave.text = "Save Raw Image";


            IsMrnFolder = new IVLControlProperties();
            IsMrnFolder.name = "IsMrnFolder";
            IsMrnFolder.val = false.ToString();
            IsMrnFolder.type = "bool";
            IsMrnFolder.control = "System.Windows.Forms.RadioButton";
            IsMrnFolder.text = "Is Mrn Folder";

            //MRNSavePath = new IVLControlProperties();
            //MRNSavePath.name = "MRNSavePath";
            //MRNSavePath.type = "string";
            //MRNSavePath.val = @"C:\IntuVision\Images";
            //MRNSavePath.control = "System.Windows.Forms.TextBox";
            //MRNSavePath.text = "MRN Save Path";

        }
    }
}
