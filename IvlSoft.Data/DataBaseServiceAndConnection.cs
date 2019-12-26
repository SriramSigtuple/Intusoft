using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql;
using System.Data;
using System.Diagnostics;
using System.IO;
using INTUSOFT.Data.Repository;
using MySql.Data.MySqlClient;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace INTUSOFT.Data
{
  public class DataBaseServiceAndConnection
    {
      int retryCount = 0;
      const int retryMaxCount = 3;
      string MySqlDumpExePath = "mysqldump.exe";
        string serviceName = string.Empty;
        public DataBaseServiceAndConnection(string ServiceName)
        {
            serviceName = ServiceName; //to get the service name from the json file which is read in ivl main window class
        }


        /// <summary>
        /// To get the status of the mysql server. 
        /// </summary>
        /// <returns></returns>
        public bool GetMysqlServiceStatus()
        {
            bool isSeviceAvaiable;
            ServiceController mySC = new ServiceController(serviceName);
            if (mySC != null)
            {
                if (mySC.Status == ServiceControllerStatus.Running)
                {
                    // Service already running
                    isSeviceAvaiable = true;
                    retryCount = 0;
                }
                else 
                {
                    if (retryCount < retryMaxCount)
                    {
                        mySC.Start();
                        isSeviceAvaiable = true;
                        Console.WriteLine(isSeviceAvaiable.ToString());
                        System.Threading.Thread.Sleep(200);
                        retryCount++;
                    }
                    else
                        isSeviceAvaiable = false;
                }
               
            }
            else
            {
                
                isSeviceAvaiable = false;
            }
            return isSeviceAvaiable;
        }

        /// <summary>
        /// Get the connection status of the database, it returns bool
        /// </summary>
        /// <returns></returns>
        public bool GetDataBaseConnectionStatus() 
        {
            bool isDatabaseConnected;
            MySqlConnection conn = null;
            try
            {
                NHibernateHelper_MySQL.OpenSession();
             
                isDatabaseConnected = true;
            }
            catch (Exception ex)
            {
                isDatabaseConnected = false;
            }
            finally
            {
                NHibernateHelper_MySQL.CloseSession();

            }
            return isDatabaseConnected;
        }


        /// <summary>
        /// To write the backup file for the database
        /// </summary>
        /// <param name="directoryName"></param>
        public void DatabaseBackup(string directoryName)
        {
            string tmestr = "";
            directoryName = directoryName + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyy-MM-dd");//Format for the forlder name in IVL_Image repo.
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
            tmestr = NHibernateHelper_MySQL.dbName + "_" + DateTime.Now.ToString("HH_mm_ss") + ".sql";
            tmestr = directoryName + Path.DirectorySeparatorChar +tmestr;
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = GetMysqlPath()+Path.DirectorySeparatorChar + MySqlDumpExePath;// fullPath;
            string cmd = string.Format(@"-u{0} -p{1} -h{2} {3}", NHibernateHelper_MySQL.userName, NHibernateHelper_MySQL.password, NHibernateHelper_MySQL.serverPath, NHibernateHelper_MySQL.dbName);
            startInfo.Arguments = cmd;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            StreamWriter file = new StreamWriter(tmestr);
            Process processTemp = new Process();
            processTemp.StartInfo = startInfo;
            processTemp.EnableRaisingEvents = true;
            processTemp.Start();
            string res;
            res = processTemp.StandardOutput.ReadToEnd();
            file.WriteLine(res);
            processTemp.WaitForExit();
            file.Close();
        }


        /// <summary>
        /// To get the mysql path from registry key
        /// </summary>
        /// <returns></returns>
        string GetMysqlPath()
        {
            string registryPath = @"SYSTEM\CurrentControlSet\Services\" + serviceName;
            //RegistryKey keyHKLM = Registry.LocalMachine;
            RegistryKey key;
            key = Registry.LocalMachine.OpenSubKey(registryPath);
            string value = key.GetValue("ImagePath").ToString();
            string[] ImagePath = value.Split(new string[] { "--" }, StringSplitOptions.None);
            ImagePath[0] = ImagePath[0].TrimStart(new char[] { '\\', '"' });
            ImagePath[0] = ImagePath[0].Remove(ImagePath[0].Length - 2, 2);
            string directoryName = new FileInfo(ImagePath[0]).DirectoryName;
            return directoryName;
        }
    }
}
