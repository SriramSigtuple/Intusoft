using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using NLog;
using NLog.Targets;

namespace WindowsFormsApplication1.AdvancedSettings
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

        public static Logger Exception_Log = LogManager.GetLogger("DB_Porting.ExceptionLog");

        private static IVLControlProperties dbPath = null;

        public IVLControlProperties _dbPath
        {
            get { return ImageStorageSettings.dbPath; }
            set { ImageStorageSettings.dbPath = value; }
        }
        private static IVLControlProperties dbName = null;

        public IVLControlProperties _dbName
        {
            get { return ImageStorageSettings.dbName; }
            set { ImageStorageSettings.dbName = value; }
        }
        private static IVLControlProperties isPng = null;

        
        public IVLControlProperties _IsPng
        {
            get { return ImageStorageSettings.isPng; }
            set { ImageStorageSettings.isPng = value; }
        }
        private static IVLControlProperties LocalStoragePath = null;

        public IVLControlProperties _LocalStoragePath
        {
            get { return ImageStorageSettings.LocalStoragePath; }
            set { ImageStorageSettings.LocalStoragePath = value; }
        }
        private static IVLControlProperties IslocalStorange = null;

        public IVLControlProperties _IslocalStorange
        {
            get { return ImageStorageSettings.IslocalStorange; }
            set { ImageStorageSettings.IslocalStorange = value; }
        }
        private static IVLControlProperties IsRemoteStorage = null;

        public IVLControlProperties _IsRemoteStorage
        {
            get { return ImageStorageSettings.IsRemoteStorage; }
            set { ImageStorageSettings.IsRemoteStorage = value; }
        }
        private static IVLControlProperties ArchivingPeriodicity = null;

        public IVLControlProperties _ArchivingPeriodicity
        {
            get { return ImageStorageSettings.ArchivingPeriodicity; }
            set { ImageStorageSettings.ArchivingPeriodicity = value; }
        }
        private static IVLControlProperties NumberingFormat = null;

        public IVLControlProperties _NumberingFormat
        {
            get { return ImageStorageSettings.NumberingFormat; }
            set { ImageStorageSettings.NumberingFormat = value; }
        }
        public ImageStorageSettings()
        {
            try
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
                _LocalStoragePath.val = @"C:\";
                _LocalStoragePath.control = "System.Windows.Forms.TextBox";
                _LocalStoragePath.text = "Local Storage Path";

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

                _dbPath = new IVLControlProperties();
                _dbPath.name = "dbPath";
                _dbPath.type = "string";
                _dbPath.val = @"C:\"; ;
                _dbPath.control = "System.Windows.Forms.TextBox";
                _dbPath.text = "Database Path";
                _dbName.length = 40;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }
    }
}
