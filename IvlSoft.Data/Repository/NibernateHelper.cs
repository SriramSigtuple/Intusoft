using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.Management;
using System.Diagnostics;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using INTUSOFT.Data.NewDbModel;
using Microsoft.Win32;
using System.IO;
using Common;

namespace INTUSOFT.Data.Repository
{
    public class NHibernateHelper_MySQL
    {
        #region Variables

        
        public static string dbPath = string.Empty;
        public static string userName = "";
        public static string password = "";
        public static string serverPath = "localhost";
        public static string connectionString = "Server=localhost;User ID=" + userName + ";Password=" + password + ";CharSet=latin1";
        //public static string connectionString = "Server=localhost;Database=intusoftmrs;User ID=root;Password=toor;CharSet=latin1";
        public static string WarningText = "";
        public static string WarningHeader = "";
        public static string dbfilePath = "";// added this variable to be used to get or set db path by sriram on feb 9th 2016 
        public static string dbName = "";// added this variable to be used to get or set db name by sriram on feb 9th 2016
        private static string dbfileNameExt = ".db3";
        private static string createDB = @"SQLs\create_Intsoft_MRS.sql";
        private static string alterDB = @"SQLs\Alter_Intsoft_MRS.sql";
        private static string grantPrivileges = @"SQLs\Grant_All_Previleges.sql";
        private static string change_size_person_attribute_value = @"SQLs\Change_person_atrribute_valueSize.sql";
        private static string observationAttributeTable = "obs_attribute";
        private static string dbConnectionString = "Server=localhost;Database = " + dbName + "; User ID=" + userName + ";Password=" + password + ";CharSet=latin1";
        public static string oldDbName = "IntuNewModel1";
        public static ISession hibernateSession;
        public static string batchFileName = @"BatchScripts\InsertReportTypes.bat";
        public static string eyeFundusImageTableName = "eye_fundus_image";
        public static string maskSettingsColumnName = "mask_settings";
        public static string reportTypeColumnName = "report_type";

        public static bool isDatabaseCreating = false;

        public static ISessionFactory _sessionFactory;
        #endregion
        

        public static void CloseSession()
        {
            SessionFactory.Close();
            SessionFactory = null;
        }

        #region Public Methods


        #region Commented codes by Kishore on 30 Aug 2019, since thes code are not used


        /// <summary>
        /// Added to check weather the specified column exists or not.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        //public static bool ColumnExists(string tablename, string columnName)
        //{
        //    using (MySqlConnection conn = new MySqlConnection(dbConnectionString))
        //    {
        //        string cmdStr = "SELECT * FROM  " + tablename + "";
        //        MySqlCommand cmd = new MySqlCommand(cmdStr, conn);
        //        conn.Open();
        //        MySqlDataReader reader = cmd.ExecuteReader();
        //        for (int i = 0; i < reader.FieldCount; i++)
        //        {
        //            if (reader.GetName(i) == columnName)
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

        //public static bool TableExists(string tableName)
        //{
        //    bool tableExists = false;
        //    using (MySqlConnection conn = new MySqlConnection(connectionString))
        //    {
        //        string cmdStr = "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = '" + dbName + "'AND table_name = '" + tableName + "'";
        //        MySqlCommand cmd = new MySqlCommand(cmdStr, conn);
        //        conn.Open();
        //        MySqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            int count = reader.GetInt32(0);
        //            if (count == 0)
        //            {
        //                tableExists = false;
        //            }
        //            else if (count == 1)
        //            {
        //                tableExists = true;
        //            }
        //        }
        //    }
        //    return tableExists;
        //}

        #endregion

        /// <summary>
        /// To open the database connection 
        /// </summary>
        public static void OpenSession()
        {
            if (hibernateSession == null)
                hibernateSession =  SessionFactory.OpenSession();
            else
                if (!hibernateSession.IsOpen)
            {
                hibernateSession= SessionFactory.OpenSession();
            }
          //else
          //      hibernateSession = SessionFactory.GetCurrentSession();

            //if (!CurrentSessionContext.HasBind(GetSessionFactory()))
            //    CurrentSessionContext.Bind(GetSessionFactory().OpenSession());

            //return GetSessionFactory().GetCurrentSession();

        }
    
        //Old Implementataion.
        //private static void BuildSchema(NHibernate.Cfg.Configuration config)
        //{
        //    //{
        //    //        new SchemaExport(config).Create(false, true);
        //    //}
        //    //return;
        //    System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
        //    st.Start();
        //    MySqlConnection connection;
        //    using (connection = new MySqlConnection(NHibernateHelper_MySQL.connectionString))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            MySqlCommand check_User_Name = new MySqlCommand("SELECT * FROM concept,patient", connection);
        //            MySqlDataReader reader = check_User_Name.ExecuteReader();
        //            if (reader.HasRows)
        //            {
        //                if (!ColumnExists( reader, "profile_image"))//Has been added to check for existence of the coloumn profile_image.
        //                {
        //                    try
        //                    {
        //                        if (File.Exists("Alter_Intsoft_MRS.sql"))//Added to check the wheather the Alter_Intsoft_MRS.sql exists or not.
        //                        {
        //                            reader.Close();//reader has been closed and opened again since executereader has been called on the same connection.
        //                            //reader.Dispose();
        //                            StreamReader fileReader = new StreamReader("Alter_Intsoft_MRS.sql");
        //                            var command = new MySqlCommand(fileReader.ReadToEnd(), connection);
        //                            command.ExecuteNonQuery();
        //                        }
        //                        else
        //                        {
        //                            CustomMessageBox.Show(WarningText, WarningHeader, CustomMessageBoxIcon.Warning);
        //                        }
        //                    }
        //                    catch (Exception ex1)
        //                    {
        //                        //Console.WriteLine(ex1.Message);
        //                    }
        //                    connection.Close();
        //                    connection.Dispose();
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            string defaultConnectionString = "Server=localhost;User ID="+userName+";Password="+password+";CharSet=latin1";
        //            connection = new MySqlConnection(defaultConnectionString);
        //            connection.Open();
        //            if (File.Exists("create_Intsoft_MRS.sql"))
        //            {
        //                FileInfo file = new FileInfo("create_Intsoft_MRS.sql");
        //                string script = string.Empty;
        //                script = file.OpenText().ReadToEnd();
        //                var command = new MySqlCommand(script, connection);
        //                command.ExecuteNonQuery();
        //            }
        //            else
        //            {
        //               CustomMessageBox.Show(WarningText, WarningHeader, CustomMessageBoxIcon.Warning);
        //            }
        //            //connection.Close();
        //            //connection.Dispose();
        //        }
        //        connection.Close();
        //        connection.Dispose();
        //    }
        //    //CustomMessageBox.Show(st.ElapsedMilliseconds.ToString());
        //}

        
        /// <summary>
        /// To execute the sql script
        /// </summary>
        /// <param name="fileName"></param>
        public static void ExecuteSqlScriptFromFile(string fileName)
        {
            MySqlConnection connection;
            connection = new MySqlConnection(connectionString);
            connection.Open();
            if (File.Exists(fileName))
            {
                FileInfo file = new FileInfo(fileName);
                string script = file.OpenText().ReadToEnd();
                script = script.Replace("dbName", dbName);//This code will replace the database name in the Intusoft-runtime.properties file to the dbname in create_Inntusoft_MRS.sql file.
                MySqlScript mysqlscript = new MySqlScript(connection, script);
                 mysqlscript.Execute();
                connection.Close();
            }
            if (connectionString != dbConnectionString)
                connectionString = dbConnectionString;
        }


        /// <summary>
        /// To check for database existing or not and returns bool
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public static bool DbExists(string dbName)
        {
            try
            {
                MySqlConnection connection;
                connection = new MySqlConnection(connectionString);
                connection.Open();
                string cmdStr = "show databases";
                MySqlCommand cmd = new MySqlCommand(cmdStr, connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                List<string> dbs = new List<string>();
                while (reader.Read())
                {
                    dbs.Add(reader.GetString(0));
                }
                connection.Close();
                if (dbs.Contains(dbName.ToLower()))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                DbExists(dbName);//Run till connection is established.
                return false;
            }
        }

        #region Commented Codes by Kishore on 30 Aug 2019, since thes code are not used.


        // public static int NoOfRowsOfaTable(string tableName)
        // {
        //     string commandLine = "SELECT COUNT(*) FROM " + tableName + "";
        //     using (MySqlConnection connect = new MySqlConnection(dbConnectionString))
        //     using (MySqlCommand cmd = new MySqlCommand(commandLine, connect))
        //     {
        //         connect.Open();
        //         int count = 0;
        //          count = Convert.ToInt32(cmd.ExecuteScalar());
        //         return count;
        //     }
        // }

        // public static void ExecuteBatchFile(string batchFileName)
        // {
        //     if (File.Exists(batchFileName))
        //     {
        //         Process proc = new Process();
        //         proc.StartInfo.FileName = batchFileName;
        //         proc.StartInfo.CreateNoWindow = false;
        //         proc.Start();
        //         proc.WaitForExit(5000);
        //     }
        // }

        // public static bool CheckForMySQLServer()
        //{
        //    string query = "SELECT Name FROM Win32_Product WHERE Name LIKE '%MySQL Server%'";
        //    var searcher = new ManagementObjectSearcher(query);
        //    var collection = searcher.Get();
        //    CustomMessageBox.Show(collection.Count.ToString());
        //    return collection.Count > 0;
        //}
        #endregion

        #endregion


        #region Private Methods


        /// <summary>
        /// Getter and setter for database connection and database mapping
        /// </summary>
        private static ISessionFactory SessionFactory
        {

            get
            {
                {
                    if (_sessionFactory == null)
                    {
                        //try
                        {
                            //serverPath = "192.168.0.146";
                            //dbName = "intunewmodel";
                            dbConnectionString = "Server=" + serverPath + ";Database = " + dbName + "; User ID=" + userName + ";Password=" + password + ";CharSet=latin1";
                            _sessionFactory = Fluently.Configure().Database(MySQLConfiguration.Standard
                                                           .ConnectionString(
                                                               dbConnectionString)
                                             )
                                             .Mappings(m =>
                                                       m.HbmMappings
                                                           .AddFromAssemblyOf<Person>()).ExposeConfiguration(config =>
                                                           {
                                                               config.SetInterceptor(new CustomInterceptor());
                                                           })
                                              .ExposeConfiguration(BuildSchema)
                                             .BuildSessionFactory();
                        }
                        //catch (Exception ex)
                        //{
                        //    int x = 0;
                        //    int i = x;
                        //}
                    }
                    return _sessionFactory;
                }
            }
            set
            {
                _sessionFactory = value;
            }
        }

        /// <summary>
        /// To create a new database or alter the existing database.
        /// </summary>
        /// <param name="config"></param>
        private static void BuildSchema(NHibernate.Cfg.Configuration config)
        {
            if (!isDatabaseCreating)
            {
                //old implmentation with severpath hard coded to localhost
                // connectionString = "Server=localhost;User ID=" + userName + ";Password=" + password + ";CharSet=latin1";
                var data = new Dictionary<string, string>();
                if (File.Exists("Intusoft-runtime.properties"))
                {
                    foreach (var row in File.ReadAllLines("Intusoft-runtime.properties"))
                        data.Add(row.Split('=')[0], string.Join("=", row.Split('=').Skip(1).ToArray()));
                    dbName = data["connection.DBname"];
                    userName = data["connection.username"];
                    password = data["connection.password"];
                    serverPath = data["serverPath"];
                }
                connectionString = "Server=" + serverPath + ";User ID=" + userName + ";Password=" + password + ";CharSet=latin1";
                //if (DbExists(oldDbName) && !DbExists(dbName))
                //    ExecuteCommand("gyk");
                if (!DbExists(dbName))// to check whether the db exists if not create a db using sql from the file
                    ExecuteSqlScriptFromFile(createDB);
                else
                {
                    // if (!ColumnExists(eyeFundusImageTableName, maskSettingsColumnName))// if the db exists and the table of observation attribute doen't exists alter db to table observation attribute table.
                    ExecuteSqlScriptFromFile(alterDB);
                    //if (NoOfRowsOfaTable(reportTypeColumnName) < 3)
                    //{
                    //    ExecuteBatchFile(batchFileName);
                    //}
                }
                ExecuteSqlScriptFromFile(grantPrivileges);
                isDatabaseCreating = true;

            }
        }

        #endregion
    }
}
