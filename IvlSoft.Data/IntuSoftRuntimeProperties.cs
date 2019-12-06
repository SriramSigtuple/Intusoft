using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data
{
    public class IntuSoftRuntimeProperties
    {
        public string dbName = "intunewmodel";
        public string password = "toor";
        public string userName = "root";
        public string db_backup_path = @"C:\IVLImageRepo\MysqlBackup";
        public string db_interval = "20000";
        public string server_path = "localhost";
        public static string filePath = @"";
        public IntuSoftRuntimeProperties()
        {

        }
    }
}
