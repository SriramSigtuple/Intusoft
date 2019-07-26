using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Type;
using NHibernate.Tool.hbm2ddl;
using INTUSOFT.Data.Model;
using Microsoft.Win32;
using System.IO;
namespace INTUSOFT.Data.Repository
{
   public  class NHibernateHelper
    {
       public static string dbPath = string.Empty;
        public static string dbfilePath = "";// added this variable to be used to get or set db path by sriram on feb 9th 2016 
        public static string dbName = "";// added this variable to be used to get or set db name by sriram on feb 9th 2016
        private static string dbfileNameExt = ".db3";
       private static ISessionFactory _sessionFactory;
       private static ISessionFactory SessionFactory
       {
           get
           {

               if (_sessionFactory == null)
               {
                   try
                   {
                       RegistryKey key;
                       //try
                       //{
                       //    key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Intusoft", true);
                       //    dbPath = key.GetValue("DB_Dir").ToString();
                       //    key.Close();
                       //}
                       //catch (Exception)
                       //{
                       //    dbPath = "C:";
                       //}
                       //if (!dbfilePath.EndsWith(@"\"))//This if statement was added by Darshan to handle situation when dbfilePath doesnot end with \(slash).
                       //{
                       //    //dbfilePath += (dbName + dbfileNameExt);
                       //    dbfilePath += @"\";
                       //}

                       //if (!Directory.Exists(dbfilePath))
                       //{
                       //    dbfilePath = @"C:\";
                       //}
                           dbfilePath += (dbName + dbfileNameExt);
                       
   _sessionFactory = Fluently.Configure().Database(SQLiteConfiguration.Standard.UsingFile(dbfilePath)
                                    .ConnectionString(
                                        @"Data Source=" + dbfilePath  + ";Version=3")
                      )
                      .Mappings(m =>
                                m.FluentMappings
                                    .AddFromAssemblyOf<Patient>())
                       .ExposeConfiguration(BuildSchema)
                      .BuildSessionFactory();
                   }
                   catch (Exception ex)
                   {
                       Console.WriteLine(ex.Message);
                   }
               }
               return _sessionFactory;
           }
           set
           {
               _sessionFactory = value;
           }
       }

       public static void CloseSession()
       {
           SessionFactory = null;
          
       }
       public static ISession OpenSession()
       {
           //if (SessionFactory == null)
             
           return SessionFactory.OpenSession();
       }
       private static void BuildSchema(NHibernate.Cfg.Configuration config)
       {
           if (!File.Exists(dbfilePath))
           {
               new SchemaExport(config)
                 .Create(false, true);
           }
           else
           {
               FileInfo info = new FileInfo(dbfilePath);
               long size = info.Length;
               if (size == 0)
               {
                   new SchemaExport(config).Create(false, true);
               }
           }
       }
    }

}
