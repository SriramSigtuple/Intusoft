using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.IO;
using System.Threading;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Common;
using NLog;
using NLog.Config;
using NLog.Targets;
using NHibernate.Engine;
using NHibernate.Proxy;
using Newtonsoft.Json.Converters;
using Microsoft.Win32;
using System.Windows.Forms;
using INTUSOFT.Data.NewDbModel;
using NHibernate.Linq;
using NHibernate;
using NHibernate.Criterion;

namespace INTUSOFT.Data.Repository
{
    public enum TypeOfPredicate { Voided, OrderByDate };

    public enum IdentifierAlias { patient_identifiers, identifiers ,patient_diagnosis,diagnosis};

    public class Repository : IRepository
    {
        private static Repository _repo;
        private static readonly Logger Exception_Log = LogManager.GetLogger("ExceptionLog");

        public Repository()
        { }

        public static Repository GetInstance()
        {
            if (_repo == null)
                _repo = new Repository();
            return _repo;
        }

        public bool Add<T>(T modelVal)
        {
            lock (this)
            {
                ITransaction transaction;
                try
                {
                    NHibernateHelper_MySQL.OpenSession();
                    using (transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
                    {
                        NHibernateHelper_MySQL.hibernateSession.Save(modelVal);
                        NHibernateHelper_MySQL.hibernateSession.Flush();
                        transaction.Commit();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    ExceptionLogWriter.WriteLog(ex, Exception_Log);
                    return false;
                }
            }
           
        }

        public bool Update<T>(T _genericObject)
        {
            lock (this)
            {
                try
                {
                    NHibernateHelper_MySQL.OpenSession();
                    using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
                    {
                        NHibernateHelper_MySQL.hibernateSession.Update(_genericObject);
                        transaction.Commit();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    ExceptionLogWriter.WriteLog(ex, Exception_Log);
                    return false;
                }
            }
           
        }

        public T GetById<T>(int id)
        {
            NHibernateHelper_MySQL.OpenSession();
          // T t = NHibernateHelper_MySQL.hibernateSession.Load<T>(id);
            T t = NHibernateHelper_MySQL.hibernateSession.Get<T>(id);
            return t;
        }

        public ICollection<T> GetAll<T>() where T : class,IBaseModel
        {
            lock (this)
            {
                NHibernateHelper_MySQL.OpenSession();

                var _genericObject = new List<T>(NHibernateHelper_MySQL.hibernateSession.Query<T>().Where(GetPredicate<T>(TypeOfPredicate.Voided)));

                for (int i = 0; i < _genericObject.Count; i++)
                {
                    _genericObject[i] = GetRealEntity<T>(_genericObject[i]);
                }
                return _genericObject;
            }
           
        }
        private T GetRealEntity<T>(T proxyValue)where T:class,IBaseModel
        {
            lock (this)
            {
                NHibernateHelper_MySQL.OpenSession();
                T value = (T)NHibernateHelper_MySQL.hibernateSession.GetSessionImplementation().PersistenceContext.Unproxy(proxyValue);
                return value;
            }
          

        }
        public ICollection<T> GetPageData<T>(int pageSize, int page) where T : class,IBaseModel
        {
            lock (this)
            {
                NHibernateHelper_MySQL.OpenSession();
                Expression<Func<T, bool>> propertyExpression = null;
                propertyExpression = pr => pr.voided == false;
                var _genericObject = NHibernateHelper_MySQL.hibernateSession.CreateCriteria<T>().Add(Restrictions.Where<T>(propertyExpression)).AddOrder(Order.Desc("patientCreatedDate")).SetFirstResult(((page - 1) * pageSize)).SetMaxResults(pageSize).List<T>();
                //for (int i = 0; i < _genericObject.Count; i++)
                //{
                //    T obj = _genericObject[i];
                //    _genericObject[i] = _genericObject[i];
                //}
                return _genericObject;
            }
           
        }

        public void Remove<T>(T _genericObject)
        {
            lock (this)
            {
                NHibernateHelper_MySQL.OpenSession();
                using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
                {
                    NHibernateHelper_MySQL.hibernateSession.Update(_genericObject);
                    transaction.Commit();
                }
            }
            
        }

        public ICollection<T> GetByCategory<T>(string _Category, object val) where T : class,IBaseModel
        {
            lock (this)
            {
                NHibernateHelper_MySQL.OpenSession();
                {
                    using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
                    {
                        //ICriteria criteriacrit = NHibernateHelper_MySQL.hibernateSession.CreateCriteria<T>().Add(Restrictions.Eq(_Category, val));
                        var _genericObject = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(T)).Add(Restrictions.Eq(_Category, val)).List<T>();
                        _genericObject = _genericObject.Where(GetPredicate<T>(TypeOfPredicate.Voided)).ToList<T>();
                        return _genericObject;
                    }
                }
            }
           
        }
        public int GetPatientCount() //to get the patient count through sql querying instead of getting patients from the database everytime, this has been added to solve the defect no 0001780 by Kishore on Jan 18 2018.
        {
                lock (this)
                {
                int returnVal = 0;

                NHibernateHelper_MySQL.OpenSession();
                    {
                        using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
                        {
                            //ICriteria criteriacrit = NHibernateHelper_MySQL.hibernateSession.CreateCriteria<T>().Add(Restrictions.Eq(_Category, val));
                            returnVal = Convert.ToInt32(NHibernateHelper_MySQL.hibernateSession.CreateQuery("SELECT count(*)FROM Patient where voided = 0").UniqueResult());

                            //returnVal = Convert.ToInt32( queryVal.ToString());
                        }
                    }
                return returnVal;
            }
          
           
        }
        public Func<T, bool> GetPredicate<T>(TypeOfPredicate typePredicate) where T : class,IBaseModel
        {
            Func<T, bool> retPredicate;
            switch (typePredicate)
            {
                case TypeOfPredicate.Voided: retPredicate = (p => p.voided == false);
                    break;
                case TypeOfPredicate.OrderByDate: retPredicate = (p => p.createdDate < DateTime.Now);
                    break;
                default: retPredicate = (p => p.voided == false);
                    break;
            }
            return retPredicate;
        }

       
        //Old Implementation
        //public ICollection<Patient> Search(Patient _patient)
        //{
        //    NHibernateHelper_MySQL.OpenSession();
        //    {
        //        var criteria = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(Patient), "patient");
        //        criteria.CreateAlias("patient.identifiers", "identifiers");
        //        var disjunction = Restrictions.Disjunction();
        //        if (_patient.identifiers != null)
        //        {
        //            string a = _patient.identifiers.First().value;
        //            disjunction.Add(Restrictions.Like("identifiers.value", a, MatchMode.Anywhere));
        //        }
        //        if (!string.IsNullOrEmpty(_patient.firstName))
        //        {
        //            disjunction.Add(Restrictions.Like("firstName", _patient.firstName, MatchMode.Anywhere));
        //        }
        //        if (!string.IsNullOrEmpty(_patient.lastName))
        //        {
        //            disjunction.Add(Restrictions.Like("lastName", _patient.lastName, MatchMode.Anywhere));
        //        }
        //        if (_patient.gender != '\0')
        //        {
        //            disjunction.Add(Restrictions.Eq("gender", _patient.gender));
        //        }
        //        if ((DateTime.Now.Year - _patient.birthdate.Year) > 3)
        //        {
        //            disjunction.Add(Restrictions.Eq("birthdate", _patient.birthdate));
        //        }
        //        IList<Patient> pats = null;
        //        pats = criteria.Add(disjunction).List<Patient>().Where(x => x.voided == false).ToList(); ;
        //        return pats;
        //    }
        //}

        public ICollection<T> Search<T>(Dictionary<string, object> searchDic,int pageSize,int page) where T : class,IBaseModel
        {
            lock (this)
            {
                Type searchType = typeof(T);
                Expression<Func<T, bool>> propertyExpression = null;
                propertyExpression = pr => pr.voided == false;
                var criteria = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(searchType, searchType.Name.ToLower());//.Add(Restrictions.Where<T>(propertyExpression)).AddOrder(Order.Desc("patientCreatedDate")).SetFirstResult(((page - 1) * pageSize)).SetMaxResults(pageSize).List<T>();
                if (searchType == typeof(Patient))
                {
                    criteria = criteria.CreateAlias(IdentifierAlias.patient_identifiers.ToString().Replace('_', '.'), IdentifierAlias.identifiers.ToString());

                }

                var disjunction = Restrictions.Conjunction();
                foreach (KeyValuePair<string, object> item in searchDic)
                {
                    if (item.Key.Contains("criteria"))
                    {
                        string[] keys = item.Key.Split('_');
                        //criteria.CreateCriteria(keys[1]);
                        string[] splitAssociationPath = keys[1].Split('.');
                        criteria.CreateAlias(keys[1], splitAssociationPath[1]);
                        SimpleExpression[] srValues = item.Value as SimpleExpression[];
                        var tempDisjunction = Restrictions.Disjunction();
                        for (int i = 0; i < srValues.Length; i++)
                        {

                            if (srValues[i].PropertyName.Contains("voided"))
                            {
                                var tempConjuction = Restrictions.Conjunction();
                                tempConjuction.Add(srValues[i]);
                                disjunction.Add(tempConjuction);
                            }
                            else
                            {
                                tempDisjunction.Add(srValues[i]);
                                disjunction.Add(tempDisjunction);

                            }
                        }
                    }
                    else if (item.Value is SimpleExpression)
                    {
                        disjunction.Add(item.Value as SimpleExpression);

                    }
                }
                //IList<T> pats = criteria.Add(disjunction).AddOrder(Order.Desc("patientCreatedDate")).SetFirstResult(((page - 1) * pageSize)).SetMaxResults(pageSize).List<T>().Where(x => x.voided == false).ToList();
                IList<T> pats = criteria.Add(disjunction).AddOrder(Order.Desc("patientCreatedDate")).List<T>().Where(x => x.voided == false).ToList().Distinct().ToList();
                return pats;
            }
          
        }

        public ICollection<T> AdvanceSearch<T>(Dictionary<string, object> searchDic, int pageSize, int page,DateTime fromDate,DateTime toDate) where T : class,IBaseModel
        {
            lock (this)
            {
                Type searchType = typeof(T);
                Expression<Func<T, bool>> propertyExpression = null;
                propertyExpression = pr => pr.voided == false;
                var criteria = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(searchType, searchType.Name.ToLower());//.Add(Restrictions.Where<T>(propertyExpression)).AddOrder(Order.Desc("patientCreatedDate")).SetFirstResult(((page - 1) * pageSize)).SetMaxResults(pageSize).List<T>();

                if (searchType == typeof(Patient))
                    criteria.CreateAlias(IdentifierAlias.patient_identifiers.ToString().Replace('_', '.'), IdentifierAlias.identifiers.ToString());
                var disjunction = Restrictions.Disjunction();
                foreach (KeyValuePair<string, object> item in searchDic)
                {
                    if (item.Value is string)
                    {
                        disjunction.Add(Restrictions.Like(item.Key, item.Value.ToString(), MatchMode.Anywhere));
                    }
                    else
                    {
                        disjunction.Add(Restrictions.Eq(item.Key, item.Value));
                    }
                }
                IList<T> pats = criteria.Add(disjunction).AddOrder(Order.Desc("patientCreatedDate")).SetFirstResult(((page - 1) * pageSize)).SetMaxResults(pageSize).List<T>().Where(x => x.voided == false).ToList();
                return pats;
            }
           
        }
    }

    //Json Repository implementation has been added by Darshan for saving all type of details inside a json file.
    //    public class UserRepository_jason
    //    {
    //        RegistryKey key;
    //        string[] UserJsonFileName = new string[2];
    //        public static bool isBackup = false;
    //        public static string UserDetailsSavePath = "";
    //        public static string UsertDetailsBackupSavePath = "";
    //        string file_path;
    //        FileStream fileStream;
    //        List<UserModel> userlist;
    //        static UserRepository_jason _userRepo;

    //        public static UserRepository_jason GetInstance()
    //        {
    //            if (_userRepo == null)
    //                _userRepo = new UserRepository_jason();
    //            return _userRepo;
    //        }
    //        public UserRepository_jason()
    //        {
    //            key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Intusoft", true);
    //            UserDetailsSavePath = key.GetValue("Users_Json").ToString();
    //            UsertDetailsBackupSavePath = key.GetValue("PatientBackup_Folder").ToString();
    //            FileInfo dinf = new FileInfo(UserDetailsSavePath);
    //            if (!Directory.Exists(dinf.Directory.FullName))
    //                Directory.CreateDirectory(dinf.Directory.FullName);
    //            dinf = new FileInfo(UsertDetailsBackupSavePath);
    //            if (!Directory.Exists(dinf.Directory.FullName))
    //                Directory.CreateDirectory(dinf.Directory.FullName);
    //            string[] temp = UserDetailsSavePath.Split(Path.DirectorySeparatorChar);
    //            UserJsonFileName = temp[temp.Length - 1].Split('.');
    //            userlist = new List<UserModel>();
    //            GetAll();
    //        }
    //        public int GetLastUserID()
    //        {
    //            if (userlist.Count > 0)
    //                return userlist.Last().ID;
    //            return 0;
    //        }
    //        private void Read()
    //        {
    //            key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Intusoft", true);
    //            file_path = key.GetValue("Users_Json").ToString();
    //            key.Close();
    //            if (System.IO.File.Exists(file_path))
    //            {
    //                TextReader txReader = new StreamReader(file_path);
    //                string newVal = txReader.ReadToEnd();
    //                //Patients patList = new Patients();
    //                if (!string.IsNullOrEmpty(newVal))
    //                    userlist = new JavaScriptSerializer().Deserialize<IList<UserModel>>(newVal).ToList<UserModel>();
    //                txReader.Close();
    //            }
    //            else
    //            {
    //                FileInfo dinf = new FileInfo(file_path);
    //                if (!Directory.Exists(dinf.Directory.FullName))
    //                    Directory.CreateDirectory(dinf.Directory.FullName);

    //            }
    //        }
    //        public void write()
    //        {

    //            {
    //                TextWriter txWriter = null;
    //                string jsonPath = "";
    //                if (isBackup)
    //                {
    //                    jsonPath = UsertDetailsBackupSavePath + DateTime.Now.ToString("yyyyMMdd") + Path.DirectorySeparatorChar + UserJsonFileName[0] + DateTime.Now.ToString("_HHmmss") + "." + UserJsonFileName[1];
    //                }
    //                else
    //                {
    //                    jsonPath = UserDetailsSavePath;
    //                }


    //                txWriter = new StreamWriter(jsonPath, false);


    //                JsonWriter jsonWriter = new JsonTextWriter(txWriter);
    //                txWriter.WriteLine(JsonConvert.SerializeObject(userlist, new IsoDateTimeConverter()));
    //                txWriter.Close();
    //                txWriter.Dispose();
    //            }


    //        }
    //        public ICollection<UserModel> GetAll()
    //        {

    //            Read();
    //            return userlist.Where(x => x.HideShowRow == false).ToList();
    //        }
    //        public bool Add(UserModel proxyPat)
    //        {

    //            userlist.Add(proxyPat);
    //            write();
    //            return true;

    //        }
    //        public void Update(UserModel proxyPat)
    //        {
    //            for (int i = 0; i < userlist.Count; i++)
    //            {
    //                if (proxyPat.ID.Equals(userlist[i].ID))
    //                    userlist[i] = proxyPat;
    //            }

    //            write();
    //        }
    //        public void Remove(UserModel proxyPat)
    //        {
    //            proxyPat.HideShowRow = true;
    //            for (int i = 0; i < userlist.Count; i++)
    //            {
    //                if (proxyPat.ID.Equals(userlist[i].ID))
    //                    userlist[i] = proxyPat;
    //            }
    //            write();
    //            Read();
    //        }
    //    }
    //    public class PatientRepository_jason
    //    {
    //        RegistryKey key;
    //        string[] PatientJsonFileName = new string[2];
    //        public static bool isBackup = false;
    //        static JsonWriter NormalJsonWriter;
    //        static TextWriter NormalJsontxtWriter;
    //        static JsonWriter BackupJsonWriter;
    //        static TextWriter BackupJsontxtWriter;
    //        public static string PatientDetailsSavePath = "";
    //        public static string PatientDetailsBackupSavePath = "";
    //        string file_path;
    //        FileStream fileStream;
    //      public  List<Patient> patlist;
    //        static PatientRepository_jason _patRepo;
    //        public static PatientRepository_jason GetInstance()
    //        {
    //            if (_patRepo == null)
    //                _patRepo = new PatientRepository_jason();
    //            return _patRepo;
    //        }
    //        public PatientRepository_jason()
    //        {

    //            //This below GetJsonFilePath has been added by Darshan on 03-09-2015 to get file path from registry key.
    //            PatientDetailsSavePath = Common.GetJsonFilePath("Patient_details_Json").ToString();
    //            PatientDetailsBackupSavePath = Common.GetJsonFilePath("PatientBackup_Folder").ToString();
    //            FileInfo dinf = new FileInfo(PatientDetailsSavePath);
    //            if (!Directory.Exists(dinf.Directory.FullName))
    //                Directory.CreateDirectory(dinf.Directory.FullName);
    //            dinf = new FileInfo(PatientDetailsBackupSavePath);

    //            if (!Directory.Exists(dinf.Directory.FullName))
    //                Directory.CreateDirectory(dinf.Directory.FullName);

    //            string[] temp = PatientDetailsSavePath.Split(Path.DirectorySeparatorChar);
    //            PatientJsonFileName = temp[temp.Length - 1].Split('.');
    //            patlist = new List<Patient>();
    //            Read();
    //            //GetAll();
    //        }

    //        public int GetLastPatientID()
    //        {
    //            if (patlist.Count > 0)
    //                return patlist.Last().ID;
    //            return 0;
    //        }
    //        private void Read()
    //        {

    //            if (System.IO.File.Exists(PatientDetailsSavePath))
    //            {
    //                     //This below if statement was added by Darshan on 28-08-2015 to check weather json file is corrupted or not.
    //                if (Common.isCheckJsonCorruption(PatientDetailsSavePath))
    //                {
    //                    //This below fileIsreadonly has been added by Darshan on 03-09-2015 to change json file read only property.


    //                    //Common.fileIsreadonly(PatientDetailsSavePath, true);
    //                    //Common.Repostiory_ExceptionLog("patientdetails","patient details read");

    //                    patlist = Common.JsonFileReader<List<Patient>>(PatientDetailsSavePath);
    //                 }
    //                else
    //                {
    //                    //This below fileIsreadonly has been added by Darshan on 03-09-2015 to change json file read only property.
    //                    //Common.fileIsreadonly(PatientDetailsSavePath, false);
    //                    Common.checkBackUpDirectories(PatientDetailsBackupSavePath, PatientDetailsSavePath);
    //                    //This below fileIsreadonly has been added by Darshan on 03-09-2015 to change json file read only property.
    //                   // Common.fileIsreadonly(PatientDetailsSavePath, true);
    //                    //Common.Repostiory_ExceptionLog("patientdetails", "patient details read");

    //                    patlist = Common.JsonFileReader<List<Patient>>(PatientDetailsSavePath);


    //                }
    //            }
    //            else
    //            {
    //                FileInfo dinf = new FileInfo(PatientDetailsSavePath);
    //                if (!Directory.Exists(dinf.Directory.FullName))
    //                    Directory.CreateDirectory(dinf.Directory.FullName);
    //                if (!System.IO.File.Exists(PatientDetailsSavePath))
    //                    File.Create(PatientDetailsSavePath);
    //                string jsonBackupPath = PatientDetailsBackupSavePath + PatientJsonFileName[0] + "." + PatientJsonFileName[1] + "." + "bak";
    //                if (!System.IO.File.Exists(jsonBackupPath))
    //                    File.Create(jsonBackupPath);

    //               }
    //        }
    //        public void write()
    //        {


    //            {
    //                string jsonPath = "";
    //                string jsonBackupPath = "";
    //                  {
    //                    jsonPath = PatientDetailsSavePath;
    //                    jsonBackupPath = PatientDetailsBackupSavePath  + PatientJsonFileName[0] + "." + PatientJsonFileName[1]+"."+"bak";
    //                   }
    //                //This below code has been added by Darshan on 16-09-2015 to stop courroupting of json file.
    //                  Common.JsonFileWriter(jsonBackupPath, patlist.ToList<object>());
    //                  //Common.Repostiory_ExceptionLog("patientdetails", "patient backup details write");
    //                  Common.JsonFileWriter(jsonPath, patlist.ToList<object>());
    //                  //Common.Repostiory_ExceptionLog("patientdetails", "patient details write");
    //            }


    //        }
    //        public ICollection<Patient> GetAll(Common.GetValueType val)
    //        {

    //            Read();
    //            List<Patient> patient_List = patlist.ToList();
    //            switch (val)
    //            {
    //                case Common.GetValueType.Existing:
    //                    {
    //                        patient_List = patient_List.Where(x => x.HideShowRow == false).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.Deleted:
    //                    {
    //                        patient_List = patient_List.Where(x => x.HideShowRow == true).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.All:
    //                        {
    //                          // patlist;//.Where(x => x.HideShowRow == false).ToList();
    //                            break;
    //                        }
    //            }
    //            return patient_List;
    //        }
    //        public bool Add(Patient proxyPat)
    //        {
    //           // patlist = Read().ToList<Patient>();
    //            patlist.Add(proxyPat);

    //            write();

    //            return true;

    //        }
    //        public void Update(Patient proxyPat)
    //        {
    //            for (int i = 0; i < patlist.Count; i++)
    //            {
    //                if (proxyPat.MRN.Equals(patlist[i].MRN))
    //                    patlist[i] = proxyPat;
    //            }
    //            //System.Diagnostics.Stopwatch a = new System.Diagnostics.Stopwatch();

    //            //a.Start();
    //            write();
    //            //a.Stop();
    //            //MessageBox.Show(a.ElapsedMilliseconds.ToString());
    //        }
    //        public void Remove(Patient proxyPat)
    //        {
    //            proxyPat.HideShowRow = true;
    //            for (int i = 0; i < patlist.Count; i++)
    //            {
    //                if (proxyPat.MRN.Equals(patlist[i].MRN))
    //                    patlist[i] = proxyPat;
    //            }
    //            write();
    //            Read();

    //        }
    //        public ICollection<Patient> SearchByPatMrn(string mrn)
    //        {
    //            return patlist.Where(x => x.MRN.ToLower().Contains(mrn.ToLower()) && x.HideShowRow == false).ToList();

    //        }
    //        public ICollection<Patient> SearchByPatFirstName(string FirstName)
    //        {
    //            return patlist.Where(x => x.FirstName.ToLower().Contains(FirstName.ToLower()) && x.HideShowRow == false).ToList();
    //        }
    //        public ICollection<Patient> SearchByPatLastName(string LastName)
    //        {
    //            return patlist.Where(x => x.LastName.ToLower().Contains(LastName.ToLower()) && x.HideShowRow == false).ToList();
    //        }
    //        public Patient GetByName(string name)
    //        {
    //            Patient proxyPat = new Patient();
    //            for (int i = 0; i < patlist.Count; i++)
    //            {
    //                if (patlist[i].FirstName.Equals(name))
    //                {
    //                    proxyPat = patlist[i];
    //                    break;
    //                }
    //            }
    //            return proxyPat;
    //        }
    //        public ICollection<Patient> Search(Patient _patient,Common.GetValueType val)
    //        {
    //          // && !string.IsNullOrEmpty(x.MRN)) || (x.FirstName.ToLower().Contains(_patient.FirstName.ToLower()) && !string.IsNullOrEmpty(x.FirstName)) || (x.LastName.ToLower().Contains(_patient.LastName.ToLower()) && !string.IsNullOrEmpty(x.LastName)) || x.Gender.Equals(_patient.Gender) && x.HideShowRow == false).ToList<Patient>();

    //            {
    //                List<Patient> patient_list = patlist.Where(x => (!string.IsNullOrEmpty(_patient.MRN) && !string.IsNullOrEmpty(x.MRN) && x.MRN.ToLower().Contains(_patient.MRN.ToLower())) || (!string.IsNullOrEmpty(_patient.FirstName) && !string.IsNullOrEmpty(x.FirstName) && x.FirstName.ToLower().Contains(_patient.FirstName.ToLower())) || (!string.IsNullOrEmpty(_patient.LastName) && !string.IsNullOrEmpty(x.LastName) && x.LastName.ToLower().Contains(_patient.LastName.ToLower())) || !string.IsNullOrEmpty(x.Gender) && x.Gender.Equals(_patient.Gender)).ToList<Patient>();
    //                switch (val)
    //                {
    //                    case Common.GetValueType.Existing:
    //                        {
    //                            patient_list = patient_list.Where(x => x.HideShowRow == false).ToList();
    //                            break;
    //                        }
    //                    case Common.GetValueType.Deleted:
    //                        {
    //                            patient_list = patient_list.Where(x => x.HideShowRow == true).ToList();
    //                            break;
    //                        }
    //                    case Common.GetValueType.All:
    //                        {
    //                            break;
    //                        }

    //                }

    //                // List<Patient> patient_list = patlist.Where(x=> x.Gender.ToLower().Equals(_patient.Gender.ToLower()) && x.HideShowRow == false).ToList<Patient>();
    //                return patient_list;
    //            }

    //         }
    //        //This below method has been added by Darshan on 30-07-2015 for advance search feature.
    //        public List<int> AdvanceSearch(DateTime fromdate, DateTime todate,int option,Common.GetValueType val)
    //        {
    //            List<Patient> patient_list=null;
    //            List<int> patids = new List<int>();
    //            switch (option)
    //            {
    //                case 1:
    //                    {
    //                        patient_list = patlist.Where(x => (x.Modified_DateTime.Date >= fromdate.Date) && (x.Modified_DateTime.Date <= todate.Date)).ToList<Patient>();
    //                    }
    //                    break;
    //                case 2:
    //                    {
    //                        patient_list = patlist.Where(x => (x.Touched_DateTime.Date >= fromdate.Date) && (x.Touched_DateTime.Date <= todate.Date)).ToList<Patient>();
    //                    }
    //                    break;
    //                case 3:
    //                    {
    //                        patient_list = patlist.Where(x => (x.RegistrationDateTime.Date >= fromdate.Date) && (x.RegistrationDateTime.Date <= todate.Date)).ToList<Patient>();
    //                    }
    //                    break;
    //            }
    //            switch (val)
    //            {
    //                case Common.GetValueType.Existing:
    //                    {
    //                        patient_list = patient_list.Where(x => x.HideShowRow == false).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.Deleted:
    //                    {
    //                        patient_list = patient_list.Where(x => x.HideShowRow == true).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.All:
    //                    {
    //                        break;
    //                    }

    //            }
    //            foreach (Patient item in patient_list)
    //            {
    //                patids.Add(item.ID);
    //            }
    //            return patids;
    //           }

    //        public Patient GetById(int id)

    //        {
    //            Patient proxyPat = new Patient();
    //            for (int i = 0; i < patlist.Count; i++)
    //            {
    //                if (patlist[i].ID.Equals(id))
    //                {
    //                    proxyPat = patlist[i];
    //                    break;
    //                }
    //            }
    //            return proxyPat;
    //        }
    //        //public ICollection<Patient> GetByCategory(string _Category, object val)
    //        //{
    //        //    patlist = Read().ToList<Patient>();
    //        //    List<Patient> RetPatlist = new List<Patient>();
    //        //    Type t = typeof(INTUSOFT.Data.Model.Patient);
    //        //    System.Reflection.FieldInfo[] fname = t.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
    //        //    List<System.Reflection.FieldInfo> fieldNamesList = fname.ToList();
    //        //    int indx = 0;
    //        //    for (int i = 0; i < fieldNamesList.Count; i++)
    //        //    {
    //        //        if (fieldNamesList[i].Name == _Category)
    //        //        {
    //        //            indx = i;
    //        //            break;
    //        //        }
    //        //    }
    //        //    int indx = fieldNamesList.FindIndex(x => x.Name.Contains(_Category));
    //        //    foreach (var item in patlist)
    //        //    {
    //        //        for (int i = 0; i < fname.Length; i++)
    //        //        {
    //        //            System.Reflection.FieldInfo[] fnameARr = item.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
    //        //            object val1 = fnameARr.GetValue(indx);
    //        //            if (val.Equals(fname.GetValue(indx)))
    //        //                RetPatlist.Add(item);
    //        //        }
    //        //    }
    //        //    return RetPatlist;
    //        //}
    //    }
    //    public class VisitRepository_jason
    //    {
    //        RegistryKey key;
    //        string[] VisitJsonFileName  = new string[2];
    //        FileStream fileStream;
    //       static VisitRepository_jason _visitRepo;
    //       public static bool isBackup = false;
    //       static JsonWriter NormalJsonWriter;
    //       static TextWriter NormalJsontxtWriter;
    //       static JsonWriter BackupJsonWriter;
    //       static TextWriter BackupJsontxtWriter;
    //       public static string visitDetailsSavePath = "";
    //       public static string visitDetailsBackupSavePath = "";
    //        public static VisitRepository_jason GetInstance()
    //       {
    //           if (_visitRepo == null)
    //               _visitRepo = new VisitRepository_jason();
    //           return _visitRepo;
    //       }
    //        public VisitRepository_jason()
    //        {
    //            //This below GetJsonFilePath has been added by Darshan on 03-09-2015 to get file path from registry key.

    //            visitDetailsSavePath = Common.GetJsonFilePath("Patient_Visit_Json").ToString();
    //            visitDetailsBackupSavePath = Common.GetJsonFilePath("PatientBackup_Folder").ToString();
    //            FileInfo dinf = new FileInfo(visitDetailsSavePath);
    //            if (!Directory.Exists(dinf.Directory.FullName))
    //                Directory.CreateDirectory(dinf.Directory.FullName);
    //            dinf = new FileInfo(visitDetailsBackupSavePath);
    //            if (!Directory.Exists(dinf.Directory.FullName))
    //                Directory.CreateDirectory(dinf.Directory.FullName);
    //            string[] temp = visitDetailsSavePath.Split(Path.DirectorySeparatorChar);
    //            VisitJsonFileName = temp[temp.Length - 1].Split('.');
    //            visitlist = new List<VisitModel>();
    //          visitlist =   Read().ToList();
    //            //GetAll();
    //        }
    //        List<VisitModel> visitlist;
    //        private ICollection<VisitModel> Read()
    //        {
    //            if (System.IO.File.Exists(visitDetailsSavePath))
    //            {
    //                //This below if statement was added by Darshan on 28-08-2015 to check weather json file is corrupted or not.
    //                if (Common.isCheckJsonCorruption(visitDetailsSavePath))
    //                {
    //                    //This below fileIsreadonly has been added by Darshan on 03-09-2015 to change json file read only property.
    //                    //Common.fileIsreadonly(visitDetailsSavePath, true);
    //                    //currentJsonFile.IsReadOnly = true;
    //                    //Common.Repostiory_ExceptionLog("Visitdetails","visit details read");
    //                    visitlist = Common.JsonFileReader<List<VisitModel>>(visitDetailsSavePath);
    //                }
    //                else
    //                {
    //                    //This below fileIsreadonly has been added by Darshan on 03-09-2015 to change json file read only property.
    //                    //Common.fileIsreadonly(visitDetailsSavePath, false);
    //                    Common.checkBackUpDirectories(visitDetailsBackupSavePath, visitDetailsSavePath);
    //                    //This below fileIsreadonly has been added by Darshan on 03-09-2015 to change json file read only property.
    //                    //Common.fileIsreadonly(visitDetailsSavePath, true);
    //                    //Common.Repostiory_ExceptionLog("Visitdetails","visit details read");
    //                    visitlist = Common.JsonFileReader<List<VisitModel>>(visitDetailsSavePath);
    //                }
    //            }
    //            else
    //            {
    //                FileInfo dinf = new FileInfo(visitDetailsSavePath);
    //                if (!Directory.Exists(dinf.Directory.FullName))
    //                    Directory.CreateDirectory(dinf.Directory.FullName);
    //                if (!System.IO.File.Exists(visitDetailsSavePath))
    //                    File.Create(visitDetailsSavePath);
    //                string jsonBackupPath = visitDetailsBackupSavePath + VisitJsonFileName[0] + "." + VisitJsonFileName[1] + "." + "bak";
    //                if (!System.IO.File.Exists(jsonBackupPath))
    //                    File.Create(jsonBackupPath);
    //            }
    //            return visitlist;
    //        }
    //        public void write()
    //        {

    //            {
    //                string jsonBackupPath = "";
    //                string jsonPath = "";
    //                   {
    //                    jsonPath = visitDetailsSavePath;
    //                    jsonBackupPath = visitDetailsBackupSavePath + VisitJsonFileName[0]  + "." + VisitJsonFileName[1]+"."+"bak";
    //                   }
    //                   //This below code has been added by Darshan on 16-09-2015 to stop courroupting of json file.
    //                   Common.JsonFileWriter(jsonBackupPath, visitlist.ToList<object>());
    //                   //Common.Repostiory_ExceptionLog("visitdetails", "visit backup details write");
    //                   Common.JsonFileWriter(jsonPath, visitlist.ToList<object>());
    //                   //Common.Repostiory_ExceptionLog("visitdetails", "visit details write");
    //            }


    //        }

    //        public bool Add(VisitModel _Visit)
    //        {
    //            visitlist = Read().ToList<VisitModel>();
    //            visitlist.Add(_Visit);
    //            write();
    //            return true;
    //        }
    //        public void Update(VisitModel _Visit)
    //        {
    //            //visitlist = Read().ToList<VisitModel>();
    //            for (int i = 0; i < visitlist.Count; i++)
    //            {
    //                if (_Visit.ID.Equals(visitlist[i].ID))
    //                    visitlist[i] = _Visit;
    //            }
    //            write();
    //        }
    //        public void Remove(VisitModel _Visit)
    //        {
    //            _Visit.HideShowRow = true;
    //           // visitlist = Read().ToList<VisitModel>();
    //            for (int i = 0; i < visitlist.Count; i++)
    //            {
    //                if (_Visit.ID.Equals(visitlist[i].ID))
    //                    visitlist[i].HideShowRow = _Visit.HideShowRow;
    //            }
    //            write();


    //        }
    //        public VisitModel GetById(int id)
    //        {
    //            VisitModel v = new VisitModel();
    //            //visitlist = Read().ToList<VisitModel>();
    //            for (int i = 0; i < visitlist.Count; i++)
    //            {
    //                if (visitlist[i].ID.Equals(id))
    //                {
    //                    v = visitlist[i];
    //                    break;
    //                }
    //            }
    //            return v;
    //        }
    //        public ICollection<VisitModel> GetByCategory(string name, object val)
    //        {
    //            List<VisitModel> visits = new List<VisitModel>();
    //            visitlist = Read().ToList<VisitModel>();
    //            switch (name)
    //            {
    //                case "ID":
    //                    visits = visitlist.Where(x => (x.ID == Convert.ToInt32(val)) && (x.HideShowRow == false)).ToList();
    //                    break;
    //            }
    //            return visits;

    //        }
    //        public ICollection<VisitModel> GetAll(Common.GetValueType val)
    //        {
    //            List<VisitModel> visit_model = Read().ToList<VisitModel>();


    //            switch (val)
    //            {
    //                case Common.GetValueType.Existing:
    //                    {
    //                        visit_model = visit_model.Where(x => x.HideShowRow == false).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.Deleted:
    //                    {
    //                        visit_model = visit_model.Where(x => x.HideShowRow == true).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.All:
    //                    {
    //                        break;
    //                    }

    //            }
    //            return visit_model;
    //        }
    //        public int GetLastVisitID()
    //        {
    //           // visitlist = Read().ToList<VisitModel>();
    //            if(visitlist.Count >0)
    //           return visitlist.Last().ID;
    //            return 0;
    //           // return visitlist;
    //        }
    //        public List<int> VisitAdvanceSearch(DateTime fromdate, DateTime todate,int option, Common.GetValueType val)
    //        {

    //            List<int> patids = new List<int>() ;
    //            List<VisitModel> patient_list=null;
    //            switch (option)
    //            {
    //                case 1:
    //                    {
    //                        patient_list = visitlist.Where(x => (x.VisitModifyDateTime.Date >= fromdate.Date) && (x.VisitModifyDateTime.Date <= todate.Date)).ToList<VisitModel>();

    //                    }
    //                    break;
    //                case 2:
    //                    {
    //                        patient_list = visitlist.Where(x => (x.VisitTouchDateTime.Date >= fromdate.Date) && (x.VisitTouchDateTime.Date <= todate.Date)).ToList<VisitModel>();
    //                    }
    //                    break;
    //                case 3:
    //                    {
    //                        patient_list = visitlist.Where(x => (x.VisitDateTime.Date >= fromdate.Date) && (x.VisitDateTime.Date <= todate.Date)).ToList<VisitModel>();
    //                    }
    //                    break;
    //            }
    //            switch (val)
    //            {
    //                case Common.GetValueType.Existing:
    //                    {
    //                        patient_list = patient_list.Where(x => x.HideShowRow == false).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.Deleted:
    //                    {
    //                        patient_list = patient_list.Where(x => x.HideShowRow == true).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.All:
    //                    {
    //                        break;
    //                    }

    //            }
    //            foreach (VisitModel item in patient_list)
    //            {
    //                patids.Add(item.PatientID);
    //            }
    //            return patids;

    //        }
    //        public ICollection<VisitModel> GetByVistID(int VisitId , Common.GetValueType val)
    //        {
    //          //  visitlist = Read().ToList<VisitModel>();
    //            List<VisitModel> visit_list = visitlist.Where(x => (x.ID == VisitId)).ToList() ;
    //            switch (val)
    //            {
    //                case Common.GetValueType.Existing:
    //                    {
    //                        visit_list = visit_list.Where(x => x.HideShowRow == false).ToList();
    //                        break; 
    //                    }
    //                case Common.GetValueType.Deleted:
    //                    {
    //                        visit_list = visit_list.Where(x => x.HideShowRow == true).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.All:
    //                    {
    //                        break;
    //                    }

    //            }
    //            return visit_list ;//.Where(x => (x.ID == VisitId) && (x.HideShowRow == false)).ToList();
    //        }
    //        public ICollection<VisitModel> GetByPatID(int PatId,Common.GetValueType val)
    //        {
    //           // visitlist = Read().ToList<VisitModel>();
    //            List<VisitModel> visit_list = visitlist.Where(x => (x.PatientID == PatId)).ToList();
    //            switch (val)
    //            {
    //                case Common.GetValueType.Existing:
    //                    {
    //                        visit_list = visit_list.Where(x => x.HideShowRow == false).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.Deleted:
    //                    {
    //                        visit_list = visit_list.Where(x => x.HideShowRow == true).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.All:
    //                    {
    //                        break;
    //                    }

    //            }
    //            return visit_list;
    //        }
    //    }

    //    public class ImageRepository_jason
    //    {
    //        RegistryKey key;
    //        string file_path;
    //        static JsonWriter NormalJsonWriter;
    //        static TextWriter NormalJsontxtWriter;
    //        static JsonWriter BackupJsonWriter;
    //        static TextWriter BackupJsontxtWriter;
    //        string[] ImageJsonFileName = new string[2];
    //        public static string ImageDetailsSavePath = "";
    //        public static string ImageDetailsBackupSavePath = "";
    //        FileStream fileStream;
    //        List<ImageModel> imagelist;
    //        public static bool isBackup = false;

    //       static ImageRepository_jason _imageRepo;
    //        public static ImageRepository_jason GetInstance()
    //        {
    //            if (_imageRepo == null)
    //            {
    //                _imageRepo = new ImageRepository_jason();
    //            }
    //            return _imageRepo;
    //        }
    //        public ImageRepository_jason()
    //        {

    //            //This below GetJsonFilePath has been added by Darshan on 03-09-2015 to get file path from registry key.

    //            ImageDetailsSavePath = Common.GetJsonFilePath("Patient_image_Json").ToString();
    //            ImageDetailsBackupSavePath = Common.GetJsonFilePath("PatientBackup_Folder").ToString();
    //            FileInfo dinf = new FileInfo(ImageDetailsSavePath);

    //            if (!Directory.Exists(dinf.Directory.FullName))
    //                Directory.CreateDirectory(dinf.Directory.FullName);
    //            dinf = new FileInfo(ImageDetailsBackupSavePath);
    //            if (!Directory.Exists(dinf.Directory.FullName))
    //                Directory.CreateDirectory(dinf.Directory.FullName);
    //            string[] temp = ImageDetailsSavePath.Split(Path.DirectorySeparatorChar);
    //            ImageJsonFileName = temp[temp.Length - 1].Split('.');

    //            //key.Close();

    //            imagelist = new List<ImageModel>();
    //            Read();
    //            //GetAll();
    //        }
    //        public int GetLastImageID()
    //        {
    //            // visitlist = Read().ToList<VisitModel>();
    //            if (imagelist.Count > 0)
    //                return imagelist.Last().ID;
    //            return 0;
    //            // return visitlist;
    //        }
    //        private void Read()
    //        {

    //            if (System.IO.File.Exists(ImageDetailsSavePath))
    //            {

    //                //This below if statement was added by Darshan on 28-08-2015 to check weather json file is corrupted or not.
    //                if (Common.isCheckJsonCorruption(ImageDetailsSavePath))
    //                {
    //                    //This below fileIsreadonly has been added by Darshan on 03-09-2015 to change json file read only property.

    //                   // Common.fileIsreadonly(ImageDetailsSavePath, true);
    //                    //Common.Repostiory_ExceptionLog("Imagedetails","Image details read");


    //                    imagelist = Common.JsonFileReader<List<ImageModel>>(ImageDetailsSavePath);


    //                }
    //                else
    //                {
    //                    //This below fileIsreadonly has been added by Darshan on 03-09-2015 to change json file read only property.

    //                    //Common.fileIsreadonly(ImageDetailsSavePath, false);

    //                    Common.checkBackUpDirectories(ImageDetailsBackupSavePath, ImageDetailsSavePath);


    //                    //This below fileIsreadonly has been added by Darshan on 03-09-2015 to change json file read only property.

    //                   // Common.fileIsreadonly(ImageDetailsSavePath, true);
    //                    //Common.Repostiory_ExceptionLog("Imagedetails", "Image details read");

    //                    imagelist = Common.JsonFileReader<List<ImageModel>>(ImageDetailsSavePath);



    //                }

    //            }
    //            else
    //            {
    //                FileInfo dinf = new FileInfo(ImageDetailsSavePath);
    //                if (!Directory.Exists(dinf.Directory.FullName))
    //                    Directory.CreateDirectory(dinf.Directory.FullName);
    //                if (!System.IO.File.Exists(ImageDetailsSavePath))
    //                    File.Create(ImageDetailsSavePath);
    //                string jsonBackupPath = ImageDetailsBackupSavePath + ImageJsonFileName[0] + "." + ImageJsonFileName[1] + "." + "bak";
    //                if (!System.IO.File.Exists(jsonBackupPath))
    //                    File.Create(jsonBackupPath);

    //            }
    //        }
    //        public void write()
    //        {

    //            {

    //                string jsonBackupPath = "";
    //                string jsonPath = "";

    //                {
    //                    jsonPath = ImageDetailsSavePath;
    //                    jsonBackupPath = ImageDetailsBackupSavePath + ImageJsonFileName[0] + "." + ImageJsonFileName[1]+"."+"bak";

    //                }
    //                //This below code has been added by Darshan on 16-09-2015 to stop courroupting of json file.

    //                Common.JsonFileWriter(jsonBackupPath, imagelist.ToList<object>());
    //                //Common.Repostiory_ExceptionLog("imagedetails", "image backup details write");
    //                Common.JsonFileWriter(jsonPath, imagelist.ToList<object>());
    //                //Common.Repostiory_ExceptionLog("imagedetails", "image details write");

    //            }


    //        }
    //        public bool Add(ImageModel _image)
    //        {
    //           // imagelist = Read().ToList<ImageModel>();
    //            imagelist.Add(_image);
    //            write();
    //            return true;

    //        }
    //        public ICollection<ImageModel> GetByCategory(string name, object value,Common.GetValueType val)
    //        {
    //            List<ImageModel> images = new List<ImageModel>();
    //           // imagelist = Read().ToList<ImageModel>();
    //            switch (name)
    //            {
    //                case "VisitID":
    //                    {
    //                        //GetAll();
    //                        images = imagelist.Where(x => (x.VisitID == (int)value)).ToList();
    ////                        images = imagelist.Where(x => (x.VisitID == Convert.ToInt32(val)) && (x.HideShowRow == false)).ToList();
    //                    } break;
    //                case "ID":
    //                    images = imagelist.Where(x => (x.ID == (int)value)).ToList();// && (x.HideShowRow == false)).ToList();
    //                    break;
    //            }

    //            switch (val)
    //            {
    //                case Common.GetValueType.Existing:
    //                    {
    //                        images = images.Where(x => x.HideShowRow == false).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.Deleted:
    //                    {
    //                        images = images.Where(x => x.HideShowRow == true).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.All:
    //                    {
    //                        break;
    //                    }

    //            }
    //            return images;

    //        }
    //        public void Update(ImageModel _image)
    //        {
    //            //imagelist = Read().ToList<ImageModel>();
    //            for (int i = 0; i < imagelist.Count; i++)
    //            {
    //                if (_image.ID.Equals(imagelist[i].ID))
    //                    imagelist[i] = _image;
    //            }
    //            write();

    //        }
    //        public void Remove(ImageModel _image)
    //        {
    //            _image.HideShowRow = true;
    //           // imagelist = Read().ToList<ImageModel>();
    //            for (int i = 0; i < imagelist.Count; i++)
    //            {
    //                if (_image.ID.Equals(imagelist[i].ID))
    //                    imagelist[i].HideShowRow = _image.HideShowRow;
    //            }
    //            write();

    //        }
    //        public ICollection<ImageModel> GetAll(Common.GetValueType val)
    //        {
    //            Read();
    //            List<ImageModel> image_List = imagelist.ToList();
    //            switch (val)
    //            {
    //                case Common.GetValueType.Existing:
    //                    {
    //                        image_List = image_List.Where(x => x.HideShowRow == false).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.Deleted:
    //                    {
    //                        image_List = image_List.Where(x => x.HideShowRow == true).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.All:
    //                    {
    //                        break;
    //                    }

    //            }
    //            return image_List;
    //        }
    //        public List<int> ImageAdvanceSearch(DateTime fromdate, DateTime todate, int option , Common.GetValueType val)
    //        {

    //            List<int> patids = new List<int>();
    //            List<ImageModel> patient_list = null;
    //            switch (option)
    //            {
    //                case 1:
    //                    {
    //                        patient_list = imagelist.Where(x => (x.ImageModifyDateTime.Date >= fromdate.Date) && (x.ImageModifyDateTime.Date <= todate.Date)).ToList<ImageModel>();

    //                    }
    //                    break;
    //                case 2:
    //                    {
    //                        patient_list = imagelist.Where(x => (x.ImageTouchDateTime.Date >= fromdate.Date) && (x.ImageTouchDateTime.Date <= todate.Date)).ToList<ImageModel>();
    //                    }
    //                    break;
    //                case 3:
    //                    {
    //                        patient_list = imagelist.Where(x => (x.ImageDateTime.Date >= fromdate.Date) && (x.ImageDateTime.Date <= todate.Date)).ToList<ImageModel>();
    //                    }
    //                    break;
    //            }
    //            switch (val)
    //            {
    //                case Common.GetValueType.Existing:
    //                    {
    //                        patient_list = patient_list.Where(x => x.HideShowRow == false).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.Deleted:
    //                    {
    //                        patient_list = patient_list.Where(x => x.HideShowRow == true).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.All:
    //                    {
    //                        break;
    //                    }

    //            }
    //            return patids;

    //        }
    //        public ImageModel GetById(int id)
    //        {
    //            ImageModel proxyPat = new ImageModel();
    //            //imagelist = Read().ToList<ImageModel>();
    //            for (int i = 0; i < imagelist.Count; i++)
    //            {
    //                if (imagelist[i].ID.Equals(id))
    //                {
    //                    proxyPat = imagelist[i];
    //                    break;
    //                }
    //            }
    //            return proxyPat;
    //        }
    //        public ICollection<ImageModel> GetByVisitId(int id , Common.GetValueType val)
    //        {
    //            //imagelist = Read().ToList<ImageModel>();
    //            List<ImageModel> retImageList = imagelist.Where(x => (x.VisitID == id)).ToList() ;
    //            switch (val)
    //            {
    //                case Common.GetValueType.Existing:
    //                    {
    //                        retImageList = retImageList.Where(x => x.HideShowRow == false).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.Deleted:
    //                    {
    //                        retImageList = retImageList.Where(x => x.HideShowRow == true).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.All:
    //                    {
    //                        break;
    //                    }

    //            }
    //            //retImageList = retImageList.Where(x => x.HideShowRow == false).ToList<ImageModel>();
    //            return retImageList;// imagelist.Where(x => (x.VisitID == id) && (x.HideShowRow == false)).ToList<ImageModel>();
    //        }

    //        public ImageModel GetByName(string name)
    //        {

    //            ImageModel proxyPat = new ImageModel();
    //           // imagelist = Read().ToList<ImageModel>();
    //            for (int i = 0; i < imagelist.Count; i++)
    //            {
    //                if (imagelist[i].ImageName.Equals(name))
    //                {
    //                    proxyPat = imagelist[i];
    //                    break;
    //                }
    //            }
    //            return proxyPat;
    //        }
    //    }
    //    public class ReporttRepository_jason
    //    {
    //        RegistryKey key;
    //        string[] ReportJsonFileName = new string[2];
    //        public static string ReportDetailsSavePath = "";
    //        public static string ReportDetailsBackupSavePath = "";
    //        static JsonWriter NormalJsonWriter;
    //        static TextWriter NormalJsontxtWriter;
    //        static JsonWriter BackupJsonWriter;
    //        static TextWriter BackupJsontxtWriter;
    //        string file_path;
    //        FileStream fileStream;
    //        public static bool isBackup = false;

    //        List<Report> reportlist;
    //        static ReporttRepository_jason _patRepo;

    //        public ReporttRepository_jason()
    //        {
    //            //This below GetJsonFilePath has been added by Darshan on 03-09-2015 to get file path from registry key.
    //            ReportDetailsSavePath = Common.GetJsonFilePath("Patient_report_Json").ToString();
    //            ReportDetailsBackupSavePath = Common.GetJsonFilePath("PatientBackup_Folder").ToString();
    //            FileInfo dinf = new FileInfo(ReportDetailsSavePath);

    //            if (!Directory.Exists(dinf.Directory.FullName))
    //                Directory.CreateDirectory(dinf.Directory.FullName);
    //            dinf = new FileInfo(ReportDetailsBackupSavePath);
    //            if (!Directory.Exists(dinf.Directory.FullName))
    //                Directory.CreateDirectory(dinf.Directory.FullName);
    //            string[] temp = ReportDetailsSavePath.Split(Path.DirectorySeparatorChar);
    //            ReportJsonFileName = temp[temp.Length - 1].Split('.');


    //            reportlist = new List<Report>();
    //          reportlist =   Read().ToList();
    //            //GetAll();
    //        }
    //        public static ReporttRepository_jason GetInstance()
    //        {
    //            if (_patRepo == null)
    //                _patRepo = new ReporttRepository_jason();
    //            return _patRepo;
    //        }
    //        public int GetLastReportID()
    //        {
    //            // visitlist = Read().ToList<VisitModel>();
    //            if (reportlist.Count > 0)
    //                return reportlist.Last().ID;
    //            return 0;
    //            // return visitlist;
    //        }
    //        private ICollection<Report> Read()
    //        {

    //            if (System.IO.File.Exists(ReportDetailsSavePath))
    //            {
    //                //This below if statement was added by Darshan on 28-08-2015 to check weather json file is corrupted or not.

    //                if (Common.isCheckJsonCorruption(ReportDetailsSavePath))
    //                {
    //                    //This below fileIsreadonly has been added by Darshan on 03-09-2015 to change json file read only property.

    //                   // Common.fileIsreadonly(ReportDetailsSavePath, true);
    //                    //Common.Repostiory_ExceptionLog("Reportdetails","Report details read");
    //                    reportlist = Common.JsonFileReader<List<Report>>(ReportDetailsSavePath);


    //                }
    //                else
    //                {
    //                    //This below fileIsreadonly has been added by Darshan on 03-09-2015 to change json file read only property.

    //                    //Common.fileIsreadonly(ReportDetailsSavePath, false);
    //                    Common.checkBackUpDirectories(ReportDetailsBackupSavePath, ReportDetailsSavePath);
    //                    //This below fileIsreadonly has been added by Darshan on 03-09-2015 to change json file read only property.

    //                   // Common.fileIsreadonly(ReportDetailsSavePath, true);
    //                    //Common.Repostiory_ExceptionLog("Reportdetails", "Report details read");


    //                    reportlist = Common.JsonFileReader<List<Report>>(ReportDetailsSavePath); 


    //                }
    //            }
    //            else
    //            {
    //                FileInfo dinf = new FileInfo(ReportDetailsSavePath);
    //                if (!Directory.Exists(dinf.Directory.FullName))
    //                    Directory.CreateDirectory(dinf.Directory.FullName);
    //                if (!System.IO.File.Exists(ReportDetailsSavePath))
    //                    File.Create(ReportDetailsSavePath);
    //                string jsonBackupPath = ReportDetailsBackupSavePath + ReportJsonFileName[0] + "." + ReportJsonFileName[1] + "." + "bak";
    //                if (!System.IO.File.Exists(jsonBackupPath))
    //                    File.Create(jsonBackupPath);

    //            }
    //            return reportlist;
    //        }
    //        public List<int> ReportAdvanceSearch(DateTime fromdate, DateTime todate, int option,Common.GetValueType val)
    //        {
    //            List<Report> patient_list = null;
    //            List<int> patids = new List<int>();
    //            switch (option)
    //            {
    //                case 1:
    //                    {
    //                        patient_list = reportlist.Where(x => (x.ReportModifyDateTime.Date >= fromdate.Date) && (x.ReportModifyDateTime.Date <= todate.Date)).ToList<Report>();
    //                    }
    //                    break;
    //                case 2:
    //                    {
    //                        patient_list = reportlist.Where(x => (x.ReportTouchedDateTime.Date >= fromdate.Date) && (x.ReportTouchedDateTime.Date <= todate.Date)).ToList<Report>();
    //                    }
    //                    break;
    //                case 3:
    //                    {
    //                        patient_list = reportlist.Where(x => (x.ReportDateTime.Date >= fromdate.Date) && (x.ReportDateTime.Date <= todate.Date)).ToList<Report>();
    //                    }
    //                    break;
    //            }
    //            switch (val)
    //            {
    //                case Common.GetValueType.Existing:
    //                    {
    //                        patient_list = patient_list.Where(x => x.HideShowRow == 0).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.Deleted:
    //                    {
    //                        patient_list = patient_list.Where(x => x.HideShowRow == 1).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.All:
    //                    {
    //                        break;
    //                    }

    //            }
    //            foreach (Report item in patient_list)
    //            {
    //                patids.Add(item.ID);
    //            }
    //            return patids;
    //         }
    //        public ICollection<Report> GetByCategory(string name, object value , Common.GetValueType val)
    //        {
    //            List<Report> reports = new List<Report>();
    //            reportlist = Read().ToList<Report>();
    //            switch (name)
    //            {
    //                case "Visit_ID":
    //                    reports = reportlist.Where(x => (x.VisitID == Convert.ToInt32(value)) ).ToList();
    //                    break;
    //                case "ID":
    //                    reports = reportlist.Where(x => (x.ID == Convert.ToInt32(value)) ).ToList();
    //                    break;

    //            }
    //            switch (val)
    //            { 
    //                case Common.GetValueType.Existing:
    //                    {
    //                        reports = reports.Where(x => x.HideShowRow == 0).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.Deleted:
    //                    {
    //                        reports = reports.Where(x => x.HideShowRow == 1).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.All:
    //                    {
    //                        break;
    //                    }

    //            }
    //            return reports;

    //        }
    //        public void write()
    //        {

    //            {

    //                string jsonPath = "";
    //                string jsonBackupPath = "";

    //                {
    //                    jsonPath = ReportDetailsSavePath;
    //                    jsonBackupPath = ReportDetailsBackupSavePath + ReportJsonFileName[0] +  "." + ReportJsonFileName[1]+"."+"bak";


    //                }
    //                //This below code has been added by Darshan on 16-09-2015 to stop courroupting of json file.

    //                Common.JsonFileWriter(jsonBackupPath, reportlist.ToList<object>());
    //                //Common.Repostiory_ExceptionLog("reportdetails", "report backup details write");
    //                Common.JsonFileWriter(jsonPath, reportlist.ToList<object>());
    //                //Common.Repostiory_ExceptionLog("reportdetails", "report details write");

    //            }

    //        }
    //        public void Add(Report _report)
    //        {

    //            //reportlist = Read().ToList<Report>();
    //            reportlist.Add(_report);
    //            write();
    //        }
    //        public void Update(Report _report)
    //        {

    //           // reportlist = Read().ToList<Report>();

    //            for (int i = 0; i < reportlist.Count; i++)
    //            {
    //                if (_report.ID.Equals(reportlist[i].ID))
    //                    reportlist[i] = _report;
    //            }
    //            write();

    //        }
    //        public void Remove(Report _report)
    //        {
    //            _report.HideShowRow = 1;
    //            reportlist = Read().ToList<Report>();
    //            for (int i = 0; i < reportlist.Count; i++)
    //            {
    //                if (_report.ID.Equals(reportlist[i].ID))
    //                    reportlist[i].HideShowRow = _report.HideShowRow;
    //            }
    //            write();

    //        //reportlist= reportlist.Where(x => x.ID == _report.ID ? x.HideShowRow = 1).ToList();

    //        //    reportlist[_reportList] = _report;

    //        //    write(reportlist);


    //        }
    //        public ICollection<Report> GetAll(Common.GetValueType val)
    //        {
    //            reportlist = Read().ToList<Report>();

    //            switch (val)
    //            {
    //                case Common.GetValueType.Existing:
    //                    {
    //                        reportlist = reportlist.Where(x => x.HideShowRow == 0).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.Deleted:
    //                    {
    //                        reportlist = reportlist.Where(x => x.HideShowRow == 1).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.All:
    //                    {
    //                        break;
    //                    }

    //            }
    //            return reportlist ;//.Where(x => x.HideShowRow == 0).ToList();
    //        }

    //        public Report GetById(int id)
    //        {
    //            Report proxyPat = new Report();
    //            //reportlist = Read().ToList<Report>();
    //            for (int i = 0; i < reportlist.Count; i++)
    //            {
    //                if (reportlist[i].ID.Equals(id))
    //                {
    //                    proxyPat = reportlist[i];
    //                    break;
    //                }
    //            }
    //            return proxyPat;
    //        }
    //        //public ICollection<Report> GetByCategory(string _Category, object val)
    //        //{
    //        //    try
    //        //    {
    //        //        using (ISession session = NHibernateHelper.OpenSession())
    //        //        {
    //        //            var _reports = session
    //        //                     .CreateCriteria(typeof(Report))
    //        //                     .Add(Restrictions.Eq(_Category, val))
    //        //                     .List<Report>();
    //        //            return _reports;

    //        //        }
    //        //    }
    //        //    catch (Exception ex)
    //        //    {
    //        //        Console.WriteLine(ex.Message);
    //        //        return new List<Report>();
    //        //    }

    //        //}

    //        //public Report GetByName(string name)
    //        //{

    //        //}

    //    }
    //    public class AnnotationRepository_jason
    //    {
    //        RegistryKey key;
    //        string[] AnotationJsonFileName = new string[2];
    //        public static string AnotationDetailsSavePath = "";
    //        public static string AnotationDetailsBackupSavePath = "";
    //        static JsonWriter NormalJsonWriter;
    //        static TextWriter NormalJsontxtWriter;
    //        static JsonWriter BackupJsonWriter;
    //        static TextWriter BackupJsontxtWriter;
    //        string file_path;
    //        public static bool isBackup = false;

    //        FileStream fileStream;
    //        static AnnotationRepository_jason _patRepo;

    //        List<AnnotationModel> AnnotationList;

    //        public AnnotationRepository_jason()
    //        {
    //            //Old implementation
    //            //key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Intusoft", true);
    //            //AnotationDetailsSavePath = key.GetValue("Patient_Annotation_Json").ToString();
    //            //AnotationDetailsBackupSavePath = key.GetValue("PatientBackup_Folder").ToString();
    //            //This below GetJsonFilePath has been added by Darshan on 03-09-2015 to get file path from registry key.

    //            AnotationDetailsSavePath = Common.GetJsonFilePath("Patient_Annotation_Json").ToString();
    //            AnotationDetailsBackupSavePath = Common.GetJsonFilePath("PatientBackup_Folder").ToString();
    //            FileInfo dinf = new FileInfo(AnotationDetailsSavePath);

    //            if (!Directory.Exists(dinf.Directory.FullName))
    //                Directory.CreateDirectory(dinf.Directory.FullName);
    //            dinf = new FileInfo(AnotationDetailsBackupSavePath);
    //            if (!Directory.Exists(dinf.Directory.FullName))
    //                Directory.CreateDirectory(dinf.Directory.FullName);
    //            string[] temp = AnotationDetailsSavePath.Split(Path.DirectorySeparatorChar);
    //            AnotationJsonFileName = temp[temp.Length - 1].Split('.');
    //            AnnotationList = new List<AnnotationModel>();
    //          AnnotationList =  Read().ToList();
    //            //GetAll();
    //        }
    //        public static AnnotationRepository_jason GetInstance()
    //        {
    //            if (_patRepo == null)
    //                _patRepo = new AnnotationRepository_jason();
    //            return _patRepo;
    //        }
    //        public int GetLastAnnotationID()
    //        {
    //            if (AnnotationList.Count > 0)
    //                return AnnotationList.Last().ID;
    //            return 0;
    //        }
    //        private ICollection<AnnotationModel> Read()
    //        {

    //            if (System.IO.File.Exists(AnotationDetailsSavePath))
    //            {

    //                //This below if statement was added by Darshan on 28-08-2015 to check weather json file is corrupted or not.
    //                if (Common.isCheckJsonCorruption(AnotationDetailsSavePath))
    //                {

    //                    //This below fileIsreadonly has been added by Darshan on 03-09-2015 to change json file read only property.
    //                   // Common.fileIsreadonly(AnotationDetailsSavePath, true);
    //                    //Common.Repostiory_ExceptionLog("Annotaiondetails","Annotation details Read");

    //                    AnnotationList = Common.JsonFileReader<List<AnnotationModel>>(AnotationDetailsSavePath);


    //                }
    //                else
    //                {
    //                    //This below fileIsreadonly has been added by Darshan on 03-09-2015 to change json file read only property.
    //                    //Common.fileIsreadonly(AnotationDetailsSavePath, false);

    //                    Common.checkBackUpDirectories(AnotationDetailsBackupSavePath, AnotationDetailsSavePath);
    //                    //This below fileIsreadonly has been added by Darshan on 03-09-2015 to change json file read only property.


    //                    //Common.fileIsreadonly(AnotationDetailsSavePath, true);
    //                    //Common.Repostiory_ExceptionLog("Annotaiondetails", "Annotation details Read");

    //                    AnnotationList = Common.JsonFileReader<List<AnnotationModel>>(AnotationDetailsSavePath);


    //                }
    //            }
    //            else
    //            {
    //                FileInfo dinf = new FileInfo(AnotationDetailsSavePath);
    //                if (!Directory.Exists(dinf.Directory.FullName))
    //                    Directory.CreateDirectory(dinf.Directory.FullName);
    //                if (!System.IO.File.Exists(AnotationDetailsSavePath))
    //                    File.Create(AnotationDetailsSavePath);
    //                string jsonBackupPath = AnotationDetailsBackupSavePath + AnotationJsonFileName[0] + "." + AnotationJsonFileName[1] + "." + "bak";
    //                if (!System.IO.File.Exists(jsonBackupPath))
    //                    File.Create(jsonBackupPath);

    //            }
    //            return AnnotationList;
    //        }
    //        public void write()
    //        {

    //            {
    //                TextWriter txWriter = null;
    //                string jsonPath = "";
    //                string jsonBackupPath = "";

    //                {
    //                    jsonPath = AnotationDetailsSavePath;
    //                    jsonBackupPath = AnotationDetailsBackupSavePath + AnotationJsonFileName[0] + "." + AnotationJsonFileName[1]+"."+"bak";


    //                }
    //                //This below code has been added by Darshan on 16-09-2015 to stop courroupting of json file.
    //                Common.JsonFileWriter(jsonBackupPath, AnnotationList.ToList<object>());
    //                //Common.Repostiory_ExceptionLog("annotationdetails", "annotation backup details write");
    //                Common.JsonFileWriter(jsonPath, AnnotationList.ToList<object>());
    //                //Common.Repostiory_ExceptionLog("annotationdetails", "annotation details write");

    //            }

    //        }
    //        public ICollection<AnnotationModel> GetAll(Common.GetValueType val)
    //        {

    //            Read();
    //            List<AnnotationModel> annotation_model = AnnotationList.ToList();
    //            switch (val)
    //            {
    //                case Common.GetValueType.Existing:
    //                    {
    //                        annotation_model = annotation_model.Where(x => x.HideShowRow == false).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.Deleted:
    //                    {
    //                        annotation_model = annotation_model.Where(x => x.HideShowRow == true).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.All:
    //                    {
    //                        break;
    //                    }

    //            }
    //            return annotation_model;//.Where(x => x.HideShowRow == false).ToList();
    //        }
    //        public bool Add(AnnotationModel proxyPat)
    //        {

    //            // patlist = Read().ToList<Patient>();
    //            AnnotationList.Add(proxyPat);
    //            write();
    //            return true;

    //        }
    //        public ICollection<AnnotationModel> GetByCategory(string name, object value,Common.GetValueType val)
    //        {
    //            List<AnnotationModel> reports = new List<AnnotationModel>();
    //            reports = Read().ToList<AnnotationModel>();
    //            switch (name)
    //            {
    //                case "Image_ID":
    //                    reports = AnnotationList.Where(x => (x.ImageID == Convert.ToInt32(value))  ).ToList();
    //                    break;
    //                case "ID":
    //                    reports = AnnotationList.Where(x => (x.ID == Convert.ToInt32(value)) ).ToList();
    //                    break;

    //            }
    //            switch (val)
    //            {
    //                case Common.GetValueType.Existing:
    //                    {
    //                        reports = reports.Where(x => x.HideShowRow == false).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.Deleted:
    //                    {
    //                        reports = reports.Where(x => x.HideShowRow == true).ToList();
    //                        break;
    //                    }
    //                case Common.GetValueType.All:
    //                    {
    //                        break;
    //                    }

    //            }
    //            return reports;

    //        }
    //        public void Update(AnnotationModel proxyPat)
    //        {
    //            for (int i = 0; i < AnnotationList.Count; i++)
    //            {
    //                if (proxyPat.ID.Equals(AnnotationList[i].ID))
    //                    AnnotationList[i] = proxyPat;
    //            }

    //            write();

    //        }
    //        public AnnotationModel GetById(int id)
    //        {
    //            AnnotationModel proxyPat = new AnnotationModel();
    //            //reportlist = Read().ToList<Report>();
    //            for (int i = 0; i < AnnotationList.Count; i++)
    //            {
    //                if (AnnotationList[i].ID.Equals(id))
    //                {
    //                    proxyPat = AnnotationList[i];
    //                    break;
    //                }
    //            }
    //            return proxyPat;
    //        }
    //        public void Remove(AnnotationModel proxyPat)
    //        {
    //            proxyPat.HideShowRow = true;
    //            for (int i = 0; i < AnnotationList.Count; i++)
    //            {
    //                if (proxyPat.ID.Equals(AnnotationList[i].ID))
    //                    AnnotationList[i] = proxyPat;
    //            }
    //            write();

    //        }

    //        }


    //public class PatientRepository : IPatientRepository
    //{
    //    static PatientRepository _patRepo;

    //    public static PatientRepository GetInstance()
    //    {
    //        if (_patRepo == null)
    //            _patRepo = new PatientRepository();
    //        return _patRepo;
    //    }
    //    public PatientRepository()
    //    {
    //    }


    //    public ICollection<INTUSOFT.Data.Model.Patient> Search(INTUSOFT.Data.Model.Patient _patient)
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())
    //        {

    //            //The below code has been modified by assing var disjunction to solve Defect no0000080 to add or implementation in search.            

    //            var criteria = session.CreateCriteria<INTUSOFT.Data.Model.Patient>();
    //            var disjunction = Restrictions.Disjunction();
    //            if (_patient.ID != 0)
    //            {

    //                disjunction.Add(Restrictions.Eq("Id", _patient.ID));

    //            }
    //            if (!string.IsNullOrEmpty(_patient.MRN))
    //            {

    //                disjunction.Add(Restrictions.Like("MRN", _patient.MRN, MatchMode.Anywhere));

    //            }
    //            if (!string.IsNullOrEmpty(_patient.FirstName))
    //            {
    //                disjunction.Add(Restrictions.Like("FirstName", _patient.FirstName, MatchMode.Anywhere));

    //            }

    //            if (!string.IsNullOrEmpty(_patient.LastName))
    //            {
    //                disjunction.Add(Restrictions.Like("LastName", _patient.LastName, MatchMode.Anywhere));

    //            }
    //            if (!string.IsNullOrEmpty(_patient.Gender))
    //            {
    //                disjunction.Add(Restrictions.Eq("Gender", _patient.Gender));

    //            }
    //            if ((DateTime.Now.Year - _patient.DOB.Year) > 3)
    //            {
    //                disjunction.Add(Restrictions.Eq("DOB", _patient.DOB));

    //            }

    //            IList<INTUSOFT.Data.Model.Patient> pats = criteria.Add(disjunction).List<INTUSOFT.Data.Model.Patient>().Where(x => x.HideShowRow == false).ToList(); ;
    //            return pats;
    //        }
    //    }
    //    public bool Add(INTUSOFT.Data.Model.Patient _patient)
    //    {
    //        try
    //        {
    //            using (ISession session = NHibernateHelper.OpenSession())
    //            using (ITransaction transaction = session.BeginTransaction())
    //            {
    //                session.Save(_patient);
    //                transaction.Commit();
    //            }
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //            return false;
    //        }
    //    }
    //    public void Update(INTUSOFT.Data.Model.Patient _patient)
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())
    //        using (ITransaction transaction = session.BeginTransaction())
    //        {
    //            session.Update(_patient);
    //            transaction.Commit();
    //        }
    //    }
    //    public void Remove(INTUSOFT.Data.Model.Patient _patient)
    //    {
    //        _patient.HideShowRow = true;
    //        //using (ISession session = NHibernateHelper.OpenSession())
    //        //using (ITransaction transaction = session.BeginTransaction())
    //        {

    //            // session.Delete(_patient);
    //            //transaction.Commit();
    //            Update(_patient);
    //        }
    //    }

    //    public INTUSOFT.Data.Model.Patient GetById(int id)
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())

    //            return session.Get<INTUSOFT.Data.Model.Patient>(id);
    //    }
    //    public ICollection<INTUSOFT.Data.Model.Patient> GetByCategory(string _Category, object val)
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())
    //        {
    //            using (ITransaction transaction = session.BeginTransaction())
    //            {
    //                var _patients = session.CreateCriteria(typeof(INTUSOFT.Data.Model.Patient))
    //                     .Add(Restrictions.Eq(_Category, val))
    //                     .List<INTUSOFT.Data.Model.Patient>();

    //                return _patients;
    //            }


    //        }
    //    }
    //    public ICollection<INTUSOFT.Data.Model.Patient> SearchByPatMrn(string mrn)
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())
    //        {
    //            return session.Query<INTUSOFT.Data.Model.Patient>().ToList().Where(x => x.MRN.ToLower().Contains(mrn.ToLower()) && x.HideShowRow == false).ToList();
    //        }

    //    }
    //    public ICollection<INTUSOFT.Data.Model.Patient> SearchByPatFirstName(string FirstName)
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())
    //        {
    //            return session.Query<INTUSOFT.Data.Model.Patient>().ToList().Where(x => x.FirstName.ToLower().Contains(FirstName.ToLower()) && x.HideShowRow == false).ToList();
    //        }

    //    }
    //    public ICollection<INTUSOFT.Data.Model.Patient> SearchByPatLastName(string LastName)
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())
    //        {
    //            return session.Query<INTUSOFT.Data.Model.Patient>().ToList().Where(x => x.LastName.ToLower().Contains(LastName.ToLower()) && x.HideShowRow == false).ToList();
    //        }

    //    }
    //    public ICollection<INTUSOFT.Data.Model.Patient> GetByDate(string date)
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())
    //        {
    //            var _patients = session
    //                     .CreateCriteria<INTUSOFT.Data.Model.Patient>().Add(
    //       Restrictions.Eq("RegistrationDateTime", date)).List<INTUSOFT.Data.Model.Patient>();
    //            return _patients;


    //        }
    //    }

    //    public INTUSOFT.Data.Model.Patient GetByName(string name)
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())
    //        {
    //            DetachedCriteria criteria = null;

    //            if (!string.IsNullOrEmpty(name))
    //            {
    //                criteria = DetachedCriteria.For(typeof(INTUSOFT.Data.Model.Patient));
    //                criteria.Add(Expression.Eq("HideShowRow", false));
    //            }
    //            INTUSOFT.Data.Model.Patient _patient = criteria.GetExecutableCriteria(session).UniqueResult<INTUSOFT.Data.Model.Patient>();
    //            //return pats;
    //            //  _patient = session
    //            //      .CreateCriteria(typeof(Patient))
    //            //      .Add(Restrictions.Eq("Name", name)Restrictions.Eq()
    //            //      .UniqueResult<Patient>();
    //            return _patient;
    //        }
    //    }

    //    public ICollection<INTUSOFT.Data.Model.Patient> GetAll()
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())
    //        {
    //            System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
    //            st.Start();
    //            var patients = session.Query<INTUSOFT.Data.Model.Patient>().ToList();
    //            st.Stop();
    //            Console.WriteLine(st.ElapsedMilliseconds);
    //            DataVariables.Patients = patients.ToList();
    //            return patients;
    //        }
    //    }
    //}

    //public class VisitRepository : IVisitRepository
    //{
    //    static VisitRepository _visitRepo;

    //    public static VisitRepository GetInstance()
    //    {
    //        if (_visitRepo == null)
    //            _visitRepo = new VisitRepository();
    //        return _visitRepo;
    //    }
    //    public VisitRepository()
    //    {

    //    }
    //    public bool Add(VisitModel _Visit)
    //    {
    //        try
    //        {
    //            using (ISession session = NHibernateHelper.OpenSession())
    //            using (ITransaction transaction = session.BeginTransaction())
    //            {
    //                session.Save(_Visit);
    //                transaction.Commit();
    //            }
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //            return false;
    //        }
    //    }
    //    public void Update(VisitModel _Visit)
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())
    //        using (ITransaction transaction = session.BeginTransaction())
    //        {
    //            session.Update(_Visit);
    //            transaction.Commit();
    //        }
    //    }
    //    public void Remove(VisitModel _Visit)
    //    {
    //        //using (ISession session = NHibernateHelper.OpenSession())
    //        //using (ITransaction transaction = session.BeginTransaction())
    //        {
    //            //session.Delete(_Visit);
    //            //transaction.Commit();
    //            Update(_Visit);
    //        }
    //    }

    //    public VisitModel GetById(int id)
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())

    //            return session.Get<VisitModel>(id);
    //    }
    //    public ICollection<VisitModel> GetByCategory(string _Category, object val)
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())
    //        {
    //            var _visits = session
    //                     .CreateCriteria(typeof(VisitModel))
    //                     .Add(Restrictions.Eq(_Category, val))
    //                     .List<VisitModel>().Where(x => x.HideShowRow == false).ToList();
    //            return _visits;

    //        }
    //    }
    //    public ICollection<VisitModel> GetByPatID(int PatId)
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())
    //        {
    //            // var _visits = session.Query<VisitModel>().Where(x=>x.PatientID ==PatId).ToList();
    //            var _visits = session.CreateCriteria(typeof(VisitModel)).Add(Restrictions.Eq("PatientID", PatId)).List<VisitModel>().Where(x => x.HideShowRow == false).ToList();
    //            return _visits;

    //        }
    //    }

    // public  ICollection<VisitModel> GetAll()
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())
    //        {
    //            // var _visits = session.Query<VisitModel>().Where(x=>x.PatientID ==PatId).ToList();
    //            var _visits = session.Query<VisitModel>().ToList();
    //            return _visits;

    //        }
    //    }
    //    public VisitModel GetByName(string name)
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())
    //        {
    //            VisitModel _visit = session
    //                 .CreateCriteria(typeof(VisitModel))
    //                 .Add(Restrictions.Eq("Name", name))
    //                 .UniqueResult<VisitModel>();
    //            return _visit;
    //        }
    //    }

    //}

    //public class ImageRepository : IImageRepository
    //{
    //    static ImageRepository _ImageRepo;

    //    public static ImageRepository GetInstance()
    //    {
    //        if (_ImageRepo == null)
    //            _ImageRepo = new ImageRepository();
    //        return _ImageRepo;
    //    }
    //    public ImageRepository()
    //    {

    //    }
    //    public bool Add(ImageModel _image)
    //    {
    //        try
    //        {
    //            using (ISession session = NHibernateHelper.OpenSession())
    //            using (ITransaction transaction = session.BeginTransaction())
    //            {
    //                session.Save(_image);
    //                transaction.Commit();
    //            }
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //            return false;
    //        }
    //    }
    //    public void Update(ImageModel _image)
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())
    //        using (ITransaction transaction = session.BeginTransaction())
    //        {
    //            session.Update(_image);
    //            transaction.Commit();
    //        }
    //    }
    //    public void Remove(ImageModel _image)
    //    {
    //        //using (ISession session = NHibernateHelper.OpenSession())
    //        //using (ITransaction transaction = session.BeginTransaction())
    //        {
    //            //session.Delete(_image);
    //            //transaction.Commit();
    //            Update(_image);
    //        }
    //    }

    //    public ImageModel GetById(int id)
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())

    //            return session.Get<ImageModel>(id);
    //    }
    //    public ICollection<ImageModel> GetByVisitId(int id)
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())
    //        {
    //            var _images = session.Query<ImageModel>().ToList().Where(x => x.VisitID == id && x.HideShowRow == false).ToList();
    //            return _images;
    //        }
    //    }
    //    public ICollection<ImageModel> GetByCategory(string _Category, object val)
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())
    //        {
    //            var _images = session
    //                     .CreateCriteria(typeof(ImageModel))
    //                     .Add(Restrictions.Eq(_Category, val))
    //                     .List<ImageModel>().ToList();
    //            return _images;

    //        }
    //    }

    //    public ImageModel GetByName(string name)
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())
    //        {
    //            ImageModel _image = session
    //                 .CreateCriteria(typeof(INTUSOFT.Data.Model.Patient))
    //                 .Add(Restrictions.Eq("Name", name))
    //                 .UniqueResult<ImageModel>();
    //            return _image;
    //        }
    //    }
    //    public ICollection<ImageModel> GetAll()
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())
    //        {
    //            var _annotations = session
    //                     .Query<ImageModel>()
    //                     .ToList<ImageModel>();
    //            return _annotations;

    //        }
    //    }
    //}

    //public class ReporttRepository : IReportRepository
    //{
    //    static ReporttRepository _reportRepo;

    //    public static ReporttRepository GetInstance()
    //    {
    //        if (_reportRepo == null)
    //            _reportRepo = new ReporttRepository();
    //        return _reportRepo;
    //    }
    //    public ReporttRepository()
    //    {

    //    }
    //    public void Add(Report _report)
    //    {
    //        try
    //        {
    //            using (ISession session = NHibernateHelper.OpenSession())
    //            using (ITransaction transaction = session.BeginTransaction())
    //            {
    //                session.Save(_report);
    //                transaction.Commit();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);

    //        }

    //    }
    //    public void Update(Report _report)
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())
    //        using (ITransaction transaction = session.BeginTransaction())
    //        {
    //            session.Update(_report);
    //            transaction.Commit();
    //        }
    //    }
    //    public void Remove(Report _report)
    //    {
    //        //using (ISession session = NHibernateHelper.OpenSession())
    //        //using (ITransaction transaction = session.BeginTransaction())
    //        {
    //           Update(_report);
    //        }
    //    }

    //    public Report GetById(int id)
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())

    //            return session.Get<Report>(id);
    //    }
    //    public ICollection<Report> GetByCategory(string _Category, object val)
    //    {
    //        try
    //        {
    //            using (ISession session = NHibernateHelper.OpenSession())
    //            {
    //                var _reports = session
    //                         .CreateCriteria(typeof(Report))
    //                         .Add(Restrictions.Eq(_Category, val))
    //                         .List<Report>();
    //                return _reports;

    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //            return new List<Report>();
    //        }

    //    }

    //    public ICollection<Report> GetAll()
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())
    //        {
    //            var _annotations = session
    //                     .Query<Report>()
    //                     .ToList<Report>();
    //            return _annotations;

    //        }
    //    }
    //    public Report GetByName(string name)
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())
    //        {
    //            Report _report = session
    //                 .CreateCriteria(typeof(Report))
    //                 .Add(Restrictions.Eq("Name", name))
    //                 .UniqueResult<Report>();
    //            return _report;
    //        }
    //    }

    //}

    //public class AnnotationRepository : IAnnotationRepository
    //{
    //    static AnnotationRepository _annotationRepo;

    //    public static AnnotationRepository GetInstance()
    //    {
    //        if (_annotationRepo == null)
    //            _annotationRepo = new AnnotationRepository();
    //        return _annotationRepo;
    //    }
    //    public AnnotationRepository()
    //    {

    //    }
    //    public void Add(AnnotationModel _annotation)
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())
    //        using (ITransaction transaction = session.BeginTransaction())
    //        {
    //            try
    //            {
    //                session.Save(_annotation);
    //                transaction.Commit();

    //            }
    //            catch (Exception ex)
    //            {
    //                Console.WriteLine(ex);
    //            }
    //        }
    //    }
    //    public ICollection<AnnotationModel> GetAll()
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())
    //        {
    //            var _annotations = session
    //                     .Query<AnnotationModel>()
    //                     .ToList<AnnotationModel>().ToList();
    //            return _annotations;

    //        }
    //    }
    //    public void Update(AnnotationModel _annotation)
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())
    //        using (ITransaction transaction = session.BeginTransaction())
    //        {
    //            session.Update(_annotation);
    //            transaction.Commit();
    //        }
    //    }
    //    public void Remove(AnnotationModel _annotation)
    //    {
    //        //using (ISession session = NHibernateHelper.OpenSession())
    //        //using (ITransaction transaction = session.BeginTransaction())
    //        {
    //            //session.Delete(_annotation);
    //            //transaction.Commit();
    //            Update(_annotation);
    //        }
    //    }

    //    public AnnotationModel GetById(int id)
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())

    //            return session.Get<AnnotationModel>(id);
    //    }
    //    public ICollection<AnnotationModel> GetByCategory(string _Category, object val)
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())
    //        {
    //            var _annotations = session
    //                     .CreateCriteria(typeof(AnnotationModel))
    //                      .Add(Restrictions.Eq(_Category, val))
    //                     .List<AnnotationModel>().Where(x => x.HideShowRow == false).ToList();
    //            return _annotations;

    //        }
    //    }
    //    public AnnotationModel GetByName(string name)
    //    {
    //        using (ISession session = NHibernateHelper.OpenSession())
    //        {
    //            AnnotationModel _annotation = session
    //                 .CreateCriteria(typeof(AnnotationModel))
    //                 .Add(Restrictions.Eq("Name", name))
    //                 .UniqueResult<AnnotationModel>();
    //            return _annotation;
    //        }
    //    }

    //}

    //public static class DataVariables
    //{
    //    public static ImageRepository _imageRepo;
    //    public static VisitRepository _visitViewRepo;
    //    public static PatientRepository _patientRepo;
    //    public static AnnotationRepository _annotationRepo;
    //    public static ReporttRepository _reportRepo;
    //    private static List<INTUSOFT.Data.Model.Patient> _patients;

    //    public static List<INTUSOFT.Data.Model.Patient> Patients
    //    {
    //        get { return _patients; }
    //        set { _patients = value; }
    //    }
    //    private static List<ImageModel> _images;

    //    public static List<ImageModel> Images
    //    {
    //        get { return _images; }
    //        set { _images = value; }
    //    }
    //    private static List<VisitModel> _visits;

    //    public static List<VisitModel> Visits
    //    {
    //        get { return _visits; }
    //        set { _visits = value; }
    //    }
    //    private static List<Report> _reports;

    //    public static List<Report> Reports
    //    {
    //        get { return _reports; }
    //        set { _reports = value; }
    //    }
    //    private static List<AnnotationModel> _annotations;

    //    public static List<AnnotationModel> Annotations
    //    {
    //        get { return _annotations; }
    //        set { _annotations = value; }
    //    }


    //    private static INTUSOFT.Data.Model.Patient _Active_Patient;

    //    public static INTUSOFT.Data.Model.Patient Active_Patient
    //    {
    //        get { return _Active_Patient; }
    //        set
    //        {
    //            _Active_Patient = value;
    //            Visits = _visitViewRepo.GetByCategory("PatientID", _Active_Patient.ID).Where(x => x.HideShowRow == false).OrderByDescending(x=>x.VisitDateTime < DateTime.Now).ToList();
    //            Visits.Reverse();

    //        }
    //    }
    //    private static ImageModel _image;

    //    public static ImageModel Active_Image
    //    {
    //        get { return _image; }
    //        set
    //        {
    //            _image = value;
    //            Annotations = _annotationRepo.GetByCategory("ImageID", Active_Image.ID).Where(x => x.HideShowRow == false).ToList();
    //        }
    //    }
    //    private static VisitModel _visit;

    //    public static VisitModel Active_Visit
    //    {
    //        get { return _visit; }
    //        set
    //        {
    //            _visit = value;
    //            Images = _imageRepo.GetByCategory("VisitID", Active_Visit.ID).Where(x => x.HideShowRow == false).ToList();
    //            Reports = _reportRepo.GetByCategory("VisitID", Active_Visit.ID).Where(x => x.HideShowRow == 0).ToList();
    //        }
    //    }

    //    private static Report _report;

    //    public static Report Active_Report
    //    {
    //        get { return _report; }
    //        set { _report = value; }
    //    }

    //    private static AnnotationModel _annotation;

    //    public static AnnotationModel Active_Annotation
    //    {
    //        get { return _annotation; }
    //        set { _annotation = value; }
    //    }

    //}
    //public static class IVLDataMethods
    //{
    //    public static void UpdatePatient()
    //    {
    //        DataVariables._patientRepo.Update(DataVariables.Active_Patient);
    //        DataVariables.Visits = DataVariables._visitViewRepo.GetByCategory("PatientID", DataVariables.Active_Patient.ID).Where(x => x.HideShowRow == false).ToList();

    //        DataVariables.Patients[DataVariables.Patients.FindIndex(x => x.ID == DataVariables.Active_Patient.ID)] = DataVariables.Active_Patient;

    //        DataVariables.Patients = DataVariables.Patients.Where(x => x.HideShowRow == false).ToList();//.OrderByDescending(x => x.RegistrationDateTime < DateTime.Now).ToList();
    //        DataVariables.Patients.Sort((x, y) => x.RegistrationDateTime.CompareTo(y.RegistrationDateTime));
    //        DataVariables.Patients.Reverse();
    //        //DataVariables.Patients = DataVariables.Patients.Where(x => x.HideShowRow == false).OrderByDescending(x => x.RegistrationDateTime < DateTime.Now).ToList();
    //    }
    //    public static void AddPatient(INTUSOFT.Data.Model.Patient patient)
    //    {

    //        DataVariables._patientRepo.Add(patient);
    //        DataVariables.Active_Patient = patient;
    //        DataVariables.Patients.Add(DataVariables.Active_Patient);
    //        DataVariables.Patients = DataVariables.Patients.Where(x => x.HideShowRow == false).ToList();//.OrderByDescending(x => x.RegistrationDateTime < DateTime.Now).ToList();
    //        DataVariables.Patients.Sort((x, y) => x.RegistrationDateTime.CompareTo(y.RegistrationDateTime));
    //        DataVariables.Patients.Reverse();

    //    }
    //    public static void RemovePatient()
    //    {
    //        UpdatePatient();
    //    }

    //    public static void UpdateVisit()
    //    {

    //        DataVariables._visitViewRepo.Update(DataVariables.Active_Visit);
    //        int indx = DataVariables.Visits.FindIndex(x=>x.ID == DataVariables.Active_Visit.ID);
    //        DataVariables.Visits[indx] = DataVariables.Active_Visit; //Active_Patient.visits.ToList().Where(x => x.HideShowRow == false).ToList();
    //        DataVariables.Visits = DataVariables.Visits.Where(x => x.HideShowRow == false).ToList();//.OrderByDescending(x => x.VisitDateTime < DateTime.Now).ToList();
    //        DataVariables.Visits.Sort((x, y) => x.VisitDateTime.CompareTo(y.VisitDateTime));

    //        DataVariables.Visits.Reverse();
    //    }
    //    public static void AddVisit(VisitModel visit)
    //    {
    //        DataVariables._visitViewRepo.Add(visit);
    //        DataVariables.Visits.Add(visit);
    //        DataVariables.Visits = DataVariables.Visits.Where(x => x.HideShowRow == false).ToList();//.OrderByDescending(x => x.VisitDateTime < DateTime.Now).ToList();
    //        DataVariables.Visits.Sort((x, y) => x.VisitDateTime.CompareTo(y.VisitDateTime));

    //        DataVariables.Visits.Reverse();
    //    }
    //    public static void RemoveVisit()
    //    {
    //        UpdateVisit();
    //    }


    //    public static void UpdateReport()
    //    {

    //        DataVariables._reportRepo.Update(DataVariables.Active_Report);
    //        DataVariables.Reports[DataVariables.Reports.FindIndex(x => x.ID == DataVariables.Active_Report.ID)] = DataVariables.Active_Report;
    //        DataVariables.Reports = DataVariables.Reports.Where(x => x.HideShowRow == 0).ToList(); //.OrderByDescending(x=>x.ReportDateTime< DateTime.Now) .ToList();
    //        DataVariables.Reports.Sort((x, y) => x.ReportDateTime.CompareTo(y.ReportDateTime));

    //        DataVariables.Reports.Reverse();

    //    }
    //    public static void AddReport(Report r)
    //    {
    //        DataVariables._reportRepo.Add(r);
    //        DataVariables.Active_Report = r;
    //        DataVariables.Reports.Add(DataVariables.Active_Report);// = DataVariables.Active_Visit.Reports.Where(x => x.HideShowRow == 0).ToList();
    //        DataVariables.Reports = DataVariables.Reports.Where(x => x.HideShowRow == 0).ToList(); //.OrderByDescending(x=>x.ReportDateTime< DateTime.Now) .ToList();

    //        DataVariables.Reports.Sort((x, y) => x.ReportDateTime.CompareTo(y.ReportDateTime));

    //        DataVariables.Reports.Reverse();
    //    }
    //    public static void RemoveReport()
    //    {
    //        UpdateReport();
    //    }
    //    public static void UpdateImage()
    //    {
    //        DataVariables._imageRepo.Update(DataVariables.Active_Image);
    //        DataVariables.Images[DataVariables.Images.FindIndex(x => x.ID == DataVariables.Active_Image.ID)] = DataVariables.Active_Image;
    //        DataVariables.Images = DataVariables.Images.Where(x => x.HideShowRow == false).ToList();//.OrderByDescending(x=>x.ImageDateTime < DateTime.Now) .ToList();
    //        DataVariables.Images.Sort((x, y) => x.ImageDateTime.CompareTo(y.ImageDateTime));

    //        DataVariables.Images.Reverse();
    //    }
    //    public static void AddImage(ImageModel i)
    //    {
    //        DataVariables._imageRepo.Add(i);
    //        DataVariables.Active_Image = i;
    //        DataVariables.Images.Add(DataVariables.Active_Image);// = DataVariables.Active_Visit.Reports.Where(x => x.HideShowRow == 0).ToList();
    //        DataVariables.Images = DataVariables.Images.Where(x => x.HideShowRow == false).ToList();//.OrderByDescending(x => x.ImageDateTime < DateTime.Now).ToList();
    //        DataVariables.Images.Sort((x, y) => x.ImageDateTime.CompareTo(y.ImageDateTime));

    //        DataVariables.Images.Reverse();
    //    }
    //    public static void RemoveImage()
    //    {
    //        UpdateImage();
    //    }
    //    public static void UpdateAnnotation()
    //    {
    //        DataVariables._annotationRepo.Update(DataVariables.Active_Annotation);
    //        DataVariables.Annotations[DataVariables.Annotations.FindIndex(x => x.ID == DataVariables.Active_Annotation.ID)] = DataVariables.Active_Annotation;
    //        DataVariables.Annotations = DataVariables.Annotations.Where(x => x.HideShowRow == false).ToList();//.OrderByDescending(x=>x.Date_Time < DateTime.Now) .ToList();
    //        DataVariables.Annotations.Sort((x, y) => x.Date_Time.CompareTo(y.Date_Time));

    //        DataVariables.Annotations.Reverse();
    //    }
    //    public static void AddAnnotation(AnnotationModel a)
    //    {
    //        DataVariables._annotationRepo.Add(a);
    //        DataVariables.Active_Annotation = a;
    //        DataVariables.Annotations.Add(DataVariables.Active_Annotation);
    //        DataVariables.Annotations = DataVariables.Annotations.Where(x => x.HideShowRow == false).ToList();//.OrderByDescending(x => x.Date_Time < DateTime.Now).ToList();
    //        DataVariables.Annotations.Sort((x, y) => x.Date_Time.CompareTo(y.Date_Time));

    //        DataVariables.Annotations.Reverse();

    //    }
    //    public static void RemoveAnnotation()
    //    {
    //        UpdateAnnotation();

    //    }
    //}
    //public class PersonRepository : IRepository
    //{
    //    static PersonRepository _patRepo;

    //    public static PersonRepository GetInstance()
    //    {
    //        if (_patRepo == null)
    //            _patRepo = new PersonRepository();
    //        return _patRepo;
    //    }
    //    public PersonRepository()
    //    {
    //    }


    //    public void Add<Person>(Person _person)
    //    {
    //        try
    //        {
    //            NHibernateHelper_MySQL.OpenSession();
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                NHibernateHelper_MySQL.hibernateSession.Save(_person);
    //                transaction.Commit();
    //            }
    //            //return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //            //return false;
    //        }
    //    }
    //    public void Update<Person>(Person _Person)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //        {
    //            NHibernateHelper_MySQL.hibernateSession.Update(_Person);
    //            transaction.Commit();
    //        }
    //    }

    //    public Person GetById(int id)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        return NHibernateHelper_MySQL.hibernateSession.Get<Person>(id);
    //    }

    //    public ICollection<Person> GetAll<Person>() where T : class,IBaseModel
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            var _persons = NHibernateHelper_MySQL.hibernateSession.Query<Person>().ToList().Where(x => x.voided == false).ToList<Person>();
    //            return _persons;
    //        }
    //    }

    //    public Dictionary<string, Func<T, bool>> GetPredicate<T>() where T : class,IBaseModel
    //    {
    //        Dictionary<string, Func<T, bool>> returnPredicateDic = new Dictionary<string, Func<T, bool>>();//dictionary for adding predicates
    //        Func<T, bool> retPredicate = (proxyPat => proxyPat.voided == false);//  predicate for voided/soft delete check
    //        returnPredicateDic.Add(TypeOfPredicate.Voided.ToString(), retPredicate);// adding of voided predicate to return dictionary
    //        //if (SortByDateType == TypeOfDate.CreatedDate)// check for date type for order by 
    //        //    retPredicate = (proxyPat => proxyPat.voided == false);
    //        //else if (SortByDateType == TypeOfDate.ModifiedDate)
    //        //    retPredicate = (proxyPat => proxyPat.createdDate < DateTime.Now);
    //        //returnPredicateDic.Add(TypeOfPredicate.OrderByDate.ToString(), retPredicate);// adding of order by date predicate to return dictionary
    //        return returnPredicateDic;
    //    }

    //    public Person GetByName(string name)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            Person _visit = NHibernateHelper_MySQL.hibernateSession
    //                 .CreateCriteria(typeof(Person))
    //                 .Add(Restrictions.Eq("Name", name))
    //                 .UniqueResult<Person>();
    //            return _visit;
    //        }
    //    }

    //    public void Remove<Person>(Person _Person)
    //    {
    //        {
    //            Update(_Person);
    //        }
    //    }

    //    public ICollection<Person> GetByCategory(string _Category, object val)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                var _persons = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(Person))
    //                     .Add(Restrictions.Eq(_Category, val))
    //                     .List<Person>();
    //                return _persons;
    //            }
    //        }
    //    }
    //}

    //public class NewPatientRepository : IRepository
    //{
    //    static NewPatientRepository _patRepo;

    //    public static NewPatientRepository GetInstance()
    //    {
    //        if (_patRepo == null)
    //            _patRepo = new NewPatientRepository();
    //        return _patRepo;
    //    }
    //    public NewPatientRepository()
    //    {
    //    }

    //    public void Add<Patient>(Patient _patient)
    //    {
    //        ISession session;
    //        ITransaction transaction;
    //        try
    //        {
    //            NHibernateHelper_MySQL.OpenSession();
    //            using (transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                NHibernateHelper_MySQL.hibernateSession.Save(_patient);
    //                transaction.Commit();
    //            }
    //            //return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //            //return false;
    //        }
    //    }

    //    public void Update<Patient>(Patient _Patient)
    //    {
    //        try
    //        {
    //            ISession session;
    //            ITransaction transaction;
    //            NHibernateHelper_MySQL.OpenSession();
    //            {
    //                using (transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //                {
    //                    bool a = _Patient.IsProxy();
    //                    bool b = NHibernateProxyHelper.IsProxy(_Patient);
    //                    INHibernateProxy proxy = _Patient as INHibernateProxy;
    //                    NHibernateHelper_MySQL.hibernateSession.Update(_Patient);
    //                    transaction.Commit();
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            int x = 0;
    //            MessageBox.Show(ex.Message);
    //        }
    //    }

    //    public Patient GetById(int id)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        //try
    //        {
    //            return NHibernateHelper_MySQL.hibernateSession.Get<Patient>(id);
    //        }
    //        return NHibernateHelper_MySQL.hibernateSession.Get<Patient>(id);
    //    }
    //    public void Remove<Patient>(Patient _patient)
    //    {
    //        {
    //            Update(_patient);
    //        }
    //    }

    //    //public ICollection<Patient> GetAll<Patient>()
    //    //{
    //    //    System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
    //    //    st.Start();
    //    //    NHibernateHelper_MySQL.OpenSession();
    //    //    List<Patient> pats = new List<Patient>();
    //    //    var _persons = NHibernateHelper_MySQL.hibernateSession.CreateCriteria<Patient>().SetFetchMode("creator", FetchMode.Join).Add(Expression.Eq("voided", false)).List<Patient>();//.ToList().Where(x => x.voided == false).ToList();
    //    //    pats = _persons.ToList();
    //    //    return pats;
    //    //}

    //    public IEnumerable<T> GetAll<T>() where T : class,IBaseModel
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        Dictionary<string, Func<T, bool>> predicateDic = GetPredicate<T>();
    //        Func<T, bool> expression = null;
    //        if (predicateDic.ContainsKey(TypeOfPredicate.Voided.ToString()))
    //            expression = predicateDic[TypeOfPredicate.Voided.ToString()];
    //        var _genericObject = NHibernateHelper_MySQL.hibernateSession.Query<T>().Where(expression).ToList();
    //        return _genericObject;
    //    }

    //    public Dictionary<string, Func<T, bool>> GetPredicate<T>() where T : class,IBaseModel
    //    {
    //        Dictionary<string, Func<T, bool>> returnPredicateDic = new Dictionary<string, Func<T, bool>>();//dictionary for adding predicates
    //        Func<T, bool> retPredicate = (proxyPat => proxyPat.voided == false);//  predicate for voided/soft delete check
    //        returnPredicateDic.Add(TypeOfPredicate.Voided.ToString(), retPredicate);// adding of voided predicate to return dictionary
    //        //if (SortByDateType == TypeOfDate.CreatedDate)// check for date type for order by 
    //        //    retPredicate = (proxyPat => proxyPat.voided == false);
    //        //else if (SortByDateType == TypeOfDate.ModifiedDate)
    //        //    retPredicate = (proxyPat => proxyPat.createdDate < DateTime.Now);
    //        //returnPredicateDic.Add(TypeOfPredicate.OrderByDate.ToString(), retPredicate);// adding of order by date predicate to return dictionary
    //        return returnPredicateDic;
    //    }

    //    public Patient GetByName(string name)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            Patient _patient = NHibernateHelper_MySQL.hibernateSession
    //                 .CreateCriteria(typeof(Patient))
    //                 .Add(Restrictions.Eq("Name", name))
    //                 .UniqueResult<Patient>();
    //            return _patient;
    //        }
    //    }

    //    public ICollection<Patient> GetByCategory<Patient>(string _Category, object val) where T : class,IBaseModel 
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                var _patients = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(Patient))
    //                     .Add(Restrictions.Eq(_Category, val))
    //                     .List<Patient>();
    //                return _patients;
    //            }
    //        }
    //    }

    //    public ICollection<Patient> Search(Patient _patient)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            var criteria = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(Patient), "patient");
    //            var criteria2 = NHibernateHelper_MySQL.hibernateSession.CreateCriteria<patient_identifier>();
    //            criteria.CreateAlias("patient.identifiers", "identifiers");
    //            var disjunction = Restrictions.Disjunction();
    //            if (_patient.patientId != 0)
    //            {
    //                disjunction.Add(Restrictions.Eq("Id", _patient.patientId));
    //            }
    //            if (_patient.identifiers != null)
    //            {
    //                string a = _patient.identifiers.First().value;
    //                disjunction.Add(Restrictions.Like("identifiers.value", a, MatchMode.Anywhere));
    //            }
    //            if (!string.IsNullOrEmpty(_patient.firstName))
    //            {
    //                disjunction.Add(Restrictions.Like("firstName", _patient.firstName, MatchMode.Anywhere));
    //            }
    //            if (!string.IsNullOrEmpty(_patient.lastName))
    //            {
    //                disjunction.Add(Restrictions.Like("lastName", _patient.lastName, MatchMode.Anywhere));
    //            }
    //            if (_patient.gender != '\0')
    //            {
    //                disjunction.Add(Restrictions.Eq("gender", _patient.gender));
    //            }
    //            if ((DateTime.Now.Year - _patient.birthdate.Year) > 3)
    //            {
    //                disjunction.Add(Restrictions.Eq("birthdate", _patient.birthdate));
    //            }
    //            IList<Patient> pats = criteria.Add(disjunction).List<Patient>().Where(x => x.voided == false).ToList(); ;
    //            return pats;
    //        }
    //    }

    //    public ICollection<Patient> GetByDate(string date)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            var _patients = NHibernateHelper_MySQL.hibernateSession
    //                     .CreateCriteria<Patient>().Add(
    //            Restrictions.Eq("date_created", date)).List<Patient>();
    //            return _patients;
    //        }
    //    }
    //}

    //public class UserRepository : IRepository
    //{
    //    static UserRepository _patRepo;

    //    public static UserRepository GetInstance()
    //    {
    //        if (_patRepo == null)
    //            _patRepo = new UserRepository();
    //        return _patRepo;
    //    }

    //    public UserRepository()
    //    {
    //    }

    //    public void Add<users>(users _person)
    //    {
    //        try
    //        {
    //            NHibernateHelper_MySQL.OpenSession();
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                NHibernateHelper_MySQL.hibernateSession.Save(_person);
    //                transaction.Commit();
    //            }
    //            //return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //            //return false;
    //        }
    //    }

    //    public void Update<users>(users _Person)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //        {
    //            NHibernateHelper_MySQL.hibernateSession.Update(_Person);
    //            transaction.Commit();
    //        }
    //    }

    //    public users GetById(int id)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        return NHibernateHelper_MySQL.hibernateSession.Get<users>(id);
    //    }

    //    public ICollection<users> GetAll<users>()
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            // var _visits = session.Query<VisitModel>().Where(x=>x.PatientID ==PatId).ToList();
    //            var _persons = NHibernateHelper_MySQL.hibernateSession.Query<users>().ToList().Where(x => x.retired == false).ToList<users>();
    //            return _persons;
    //        }
    //    }

    //    public users GetByName(string userName)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            users _visit = NHibernateHelper_MySQL.hibernateSession
    //                 .CreateCriteria(typeof(users))
    //                 .Add(Restrictions.Eq("username", userName))
    //                 .UniqueResult<users>();
    //            return _visit;
    //        }
    //    }

    //    public void Remove<users>(users _Person)
    //    {
    //        {
    //            Update(_Person);
    //        }
    //    }
    //    public ICollection<users> GetByCategory(string _Category, object val)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                var _persons = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(users))
    //                     .Add(Restrictions.Eq(_Category, val))
    //                     .List<users>();
    //                return _persons;
    //            }
    //        }
    //    }

    //}
    //public class NewVistRepository : IRepository
    //{
    //    static NewVistRepository _patRepo;
    //    public static NewVistRepository GetInstance()
    //    {
    //        if (_patRepo == null)
    //            _patRepo = new NewVistRepository();
    //        return _patRepo;
    //    }
    //    public NewVistRepository()
    //    {
    //    }

    //    public void Add<visit>(visit _visit)
    //    {
    //        try
    //        {
    //            NHibernateHelper_MySQL.OpenSession();
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                NHibernateHelper_MySQL.hibernateSession.Save(_visit);
    //                transaction.Commit();
    //            }
    //            //return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //            //return false;
    //        }
    //    }
    //    public visit GetVisit(int visitId)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            return NHibernateHelper_MySQL.hibernateSession.Get<visit>(visitId);
    //        }
    //    }
    //    public ICollection<visit> GetByCategory(string _Category, object val)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                var _visits = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(visit))
    //                     .Add(Restrictions.Eq(_Category, val))
    //                     .List<visit>();
    //                return _visits;
    //            }
    //        }
    //    }

    //    public void Update<visit>(visit _visit)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //        {
    //            NHibernateHelper_MySQL.hibernateSession.Update(_visit);
    //            transaction.Commit();
    //        }
    //    }
    //    public void Remove<visit>(visit _visit)
    //    {
    //        {
    //            Update(_visit);
    //        }
    //    }
    //    public ICollection<visit> GetAll()
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            // var _visits = session.Query<VisitModel>().Where(x=>x.PatientID ==PatId).ToList();
    //            var _visits = NHibernateHelper_MySQL.hibernateSession.Query<visit>().ToList().Where(x => x.voided == false).ToList();
    //            return _visits;
    //        }
    //    }
    //    public ICollection<visit> GetByPatID(Patient PatId)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            // var _visits = session.Query<VisitModel>().Where(x=>x.PatientID ==PatId).ToList();
    //            var _visits = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(visit)).Add(Restrictions.Eq("patient_id", PatId)).List<visit>().Where(x => x.voided == false).ToList();
    //            return _visits;
    //        }
    //    }
    //}

    //public class ObsRepository : IRepository
    //{
    //    static ObsRepository _patRepo;
    //    public static ObsRepository GetInstance()
    //    {
    //        if (_patRepo == null)
    //            _patRepo = new ObsRepository();
    //        return _patRepo;
    //    }
    //    public ObsRepository()
    //    {
    //    }

    //    public void Add<obs>(obs _obs)
    //    {
    //        try
    //        {
    //            NHibernateHelper_MySQL.OpenSession();
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                NHibernateHelper_MySQL.hibernateSession.Save(_obs);
    //                transaction.Commit();
    //            }
    //            //return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //            //return false;
    //        }
    //    }

    //    public int GetImageCount(int visitID)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            //var _obs=null;
    //            try
    //            {
    //                var _obs = NHibernateHelper_MySQL.hibernateSession.Query<obs>().ToList().Where(x => x.visit.visitId == visitID).Where(x => x.voided == false).ToList();
    //                return _obs.Count;
    //            }
    //            catch (Exception ex)
    //            {
    //                Console.WriteLine(ex.Message);
    //            }
    //            return 1;
    //        }
    //    }

    //    public obs GetObs(int obsId)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            return NHibernateHelper_MySQL.hibernateSession.Get<obs>(obsId);
    //        }
    //    }
    //    public void Update<obs>(obs _obs)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //        {
    //            NHibernateHelper_MySQL.hibernateSession.Update(_obs);
    //            transaction.Commit();
    //        }
    //    }
    //    public void Remove<obs>(obs _obs)
    //    {
    //        {
    //            Update(_obs);
    //        }
    //    }
    //    public ICollection<obs> GetAll<obs>()
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            // var _visits = session.Query<VisitModel>().Where(x=>x.PatientID ==PatId).ToList();
    //            var _obs = NHibernateHelper_MySQL.hibernateSession.Query<obs>().ToList().Where(x => x.voided == false).ToList();
    //            return _obs;
    //        }
    //    }
    //    public ICollection<obs> GetByCategory(string _Category, object val)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                var _obs = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(obs))
    //                     .Add(Restrictions.Eq(_Category, val))
    //                     .List<obs>();
    //                return _obs;
    //            }
    //        }
    //    }

    //    public ICollection<obs> GetByObsID(int visitId)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            // var _visits = session.Query<VisitModel>().Where(x=>x.PatientID ==PatId).ToList();
    //            var _obs = NHibernateHelper_MySQL.hibernateSession.Query<obs>().ToList().Where(x => x.visit.visitId == visitId).Where(x => x.voided == false).ToList<obs>();
    //            return _obs;
    //        }
    //    }
    //}

    //public class PersonAddressRepository : IRepository
    //{
    //    static PersonAddressRepository _patRepo;
    //    public static PersonAddressRepository GetInstance()
    //    {
    //        if (_patRepo == null)
    //            _patRepo = new PersonAddressRepository();
    //        return _patRepo;
    //    }
    //    public PersonAddressRepository()
    //    {
    //    }
    //    public void Add<PersonAddressModel>(PersonAddressModel _address)
    //    {
    //        try
    //        {
    //            NHibernateHelper_MySQL.OpenSession();
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                NHibernateHelper_MySQL.hibernateSession.Save(_address);
    //                transaction.Commit();
    //            }
    //            //return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //            //return false;
    //        }
    //    }

    //    public PersonAddressModel GetAddress(int addressId)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            return NHibernateHelper_MySQL.hibernateSession.Get<PersonAddressModel>(addressId);
    //        }
    //    }

    //    public void Update<PersonAddressModel>(PersonAddressModel _address)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //        {
    //            NHibernateHelper_MySQL.hibernateSession.Update(_address);
    //            transaction.Commit();
    //        }
    //    }

    //    public void Remove<PersonAddressModel>(PersonAddressModel _address)
    //    {
    //        {
    //            Update(_address);
    //        }
    //    }

    //    public ICollection<PersonAddressModel> GetAll<PersonAddressModel>()
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            // var _visits = session.Query<VisitModel>().Where(x=>x.PatientID ==PatId).ToList();
    //            var _address = NHibernateHelper_MySQL.hibernateSession.Query<PersonAddressModel>().ToList().Where(x => x.voided == false).ToList();
    //            return _address;
    //        }
    //    }

    //    public ICollection<PersonAddressModel> GetByCategory(string _Category, object val)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                var _address = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(PersonAddressModel))
    //                     .Add(Restrictions.Eq(_Category, val))
    //                     .List<PersonAddressModel>();
    //                return _address;
    //            }
    //        }
    //    }

    //    public ICollection<PersonAddressModel> GetByPatientID(Person patId)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            // var _visits = session.Query<VisitModel>().Where(x=>x.PatientID ==PatId).ToList();
    //            var _obs = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(PersonAddressModel)).Add(Restrictions.Eq("person_id", patId)).List<PersonAddressModel>().Where(x => x.voided == false).ToList();
    //            return _obs;
    //        }
    //    }
    //}

    //public class PersonAttributeRepository : IRepository
    //{
    //    static PersonAttributeRepository _patRepo;
    //    public static PersonAttributeRepository GetInstance()
    //    {
    //        if (_patRepo == null)
    //            _patRepo = new PersonAttributeRepository();
    //        return _patRepo;
    //    }
    //    public PersonAttributeRepository()
    //    {
    //    }
    //    public void Add<person_attribute>(person_attribute _personAttribute)
    //    {
    //        try
    //        {
    //            NHibernateHelper_MySQL.OpenSession();
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                NHibernateHelper_MySQL.hibernateSession.Save(_personAttribute);
    //                transaction.Commit();
    //            }
    //            //return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //            //return false;
    //        }
    //    }

    //    public person_attribute GetAttributes(int attributeId)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            return NHibernateHelper_MySQL.hibernateSession.Get<person_attribute>(attributeId);
    //        }
    //    }

    //    public void Update<person_attribute>(person_attribute _attribute)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //        {
    //            NHibernateHelper_MySQL.hibernateSession.Update(_attribute);
    //            transaction.Commit();
    //        }
    //    }

    //    public void Remove<person_attribute>(person_attribute _attribute)
    //    {
    //        {
    //            Update(_attribute);
    //        }
    //    }

    //    public ICollection<person_attribute> GetAll<person_attribute>()
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            // var _visits = session.Query<VisitModel>().Where(x=>x.PatientID ==PatId).ToList();
    //            var _attribute = NHibernateHelper_MySQL.hibernateSession.Query<person_attribute>().ToList();
    //            return _attribute;
    //        }
    //    }

    //    public ICollection<person_attribute> GetByCategory(string _Category, object val)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                var _attributes = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(person_attribute))
    //                     .Add(Restrictions.Eq(_Category, val))
    //                     .List<person_attribute>();
    //                return _attributes;
    //            }
    //        }
    //    }

    //    public ICollection<person_attribute> GetByPatientID(Person patId)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            // var _visits = session.Query<VisitModel>().Where(x=>x.PatientID ==PatId).ToList();
    //            var _obs = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(person_attribute)).Add(Restrictions.Eq("patient", patId)).List<person_attribute>();
    //            return _obs;
    //        }
    //    }
    //}

    //public class ObservationAttributeRepository : IRepository
    //{
    //    static ObservationAttributeRepository _obsAttributeRepo;
    //    public static ObservationAttributeRepository GetInstance()
    //    {
    //        if (_obsAttributeRepo == null)
    //            _obsAttributeRepo = new ObservationAttributeRepository();
    //        return _obsAttributeRepo;
    //    }
    //    public ObservationAttributeRepository()
    //    {
    //    }
    //    public void Add<ObservationAttribute>(ObservationAttribute _observationAttribute)
    //    {
    //        try
    //        {
    //            NHibernateHelper_MySQL.OpenSession();
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                NHibernateHelper_MySQL.hibernateSession.Save(_observationAttribute);
    //                transaction.Commit();
    //            }
    //            //return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //            //return false;
    //        }
    //    }

    //    public ObservationAttribute GetAttributes(int attributeId)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            return NHibernateHelper_MySQL.hibernateSession.Get<ObservationAttribute>(attributeId);
    //        }
    //    }

    //    public void Update<ObservationAttribute>(ObservationAttribute _attribute)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //        {
    //            NHibernateHelper_MySQL.hibernateSession.Update(_attribute);
    //            transaction.Commit();
    //        }
    //    }

    //    public void Remove<ObservationAttribute>(ObservationAttribute _attribute)
    //    {
    //        {
    //            Update(_attribute);
    //        }
    //    }

    //    public ICollection<ObservationAttribute> GetAll<ObservationAttribute>()
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            // var _visits = session.Query<VisitModel>().Where(x=>x.PatientID ==PatId).ToList();
    //            var _attribute = NHibernateHelper_MySQL.hibernateSession.Query<ObservationAttribute>().ToList();
    //            return _attribute;
    //        }
    //    }

    //    public ICollection<ObservationAttribute> GetByCategory(string _Category, object val)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                var _attributes = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(ObservationAttribute))
    //                     .Add(Restrictions.Eq(_Category, val))
    //                     .List<ObservationAttribute>();
    //                return _attributes;
    //            }
    //        }
    //    }

    //    public ICollection<ObservationAttribute> GetByObsID(obs obsId)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            // var _visits = session.Query<VisitModel>().Where(x=>x.PatientID ==PatId).ToList();
    //            var _obs = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(ObservationAttribute)).Add(Restrictions.Eq("observation", obsId)).List<ObservationAttribute>();
    //            return _obs;
    //        }
    //    }
    //}

    //public class EyeFundusImageRepository : IRepository
    //{
    //    static EyeFundusImageRepository _patRepo;
    //    public static EyeFundusImageRepository GetInstance()
    //    {
    //        if (_patRepo == null)
    //            _patRepo = new EyeFundusImageRepository();
    //        return _patRepo;
    //    }
    //    public EyeFundusImageRepository()
    //    {
    //    }

    //    public void Add<eye_fundus_image>(eye_fundus_image _eyeFundusImage)
    //    {
    //        try
    //        {
    //            NHibernateHelper_MySQL.OpenSession();
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                NHibernateHelper_MySQL.hibernateSession.Save(_eyeFundusImage);
    //                transaction.Commit();
    //            }
    //            //return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //            //return false;
    //        }
    //    }

    //    public eye_fundus_image GetById(int eye_fundus_image_id)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            return NHibernateHelper_MySQL.hibernateSession.Get<eye_fundus_image>(eye_fundus_image_id);
    //        }
    //    }
    //    public void Update<eye_fundus_image>(eye_fundus_image _eyeFundusImage)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //        {
    //            NHibernateHelper_MySQL.hibernateSession.Update(_eyeFundusImage);
    //            transaction.Commit();
    //        }
    //    }
    //    public void Remove<eye_fundus_image>(eye_fundus_image _eyeFundusImage)
    //    {
    //        {
    //            Update<eye_fundus_image>(_eyeFundusImage);
    //        }
    //    }
    //    public ICollection<eye_fundus_image> GetAll<eye_fundus_image>()
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            // var _visits = session.Query<VisitModel>().Where(x=>x.PatientID ==PatId).ToList();
    //            var _eyeFundusImage = NHibernateHelper_MySQL.hibernateSession.Query<eye_fundus_image>().ToList();
    //            return _eyeFundusImage;
    //        }
    //    }
    //    public ICollection<eye_fundus_image> GetByCategory(string _Category, object val)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                var _images = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(eye_fundus_image))
    //                     .Add(Restrictions.Eq(_Category, val))
    //                     .List<eye_fundus_image>();
    //                return _images;
    //            }
    //        }
    //    }

    //    public ICollection<eye_fundus_image> GetByObsID(obs obsId)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            // var _visits = session.Query<VisitModel>().Where(x=>x.PatientID ==PatId).ToList();
    //            var _eye_fundus_image = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(eye_fundus_image)).Add(Restrictions.Eq("obs_id", obsId)).List<eye_fundus_image>();
    //            return _eye_fundus_image;
    //        }
    //    }
    //    public ICollection<eye_fundus_image> GetByObs(List<obs> observation)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            // var _visits = session.Query<VisitModel>().Where(x=>x.PatientID ==PatId).ToList();
    //            var _eye_fundus_image = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(eye_fundus_image)).Add(Restrictions.Eq("obs_id", observation)).List<eye_fundus_image>();
    //            return _eye_fundus_image;
    //        }
    //    }

    //    //public ICollection<eye_fundus_image> GetByVisitID(int visitId)
    //    //{
    //    //    NewNibernateHelper.OpenSession();
    //    //    {
    //    //        //var _visits = session.Query<VisitModel>().Where(x=>x.PatientID ==PatId).ToList();
    //    //        var _eye_fundus_image = NewNibernateHelper.hibernateSession.Query<eye_fundus_image>().ToList().Where(x => x.obs_id.visit.visitId == visitId).ToList<eye_fundus_image>();
    //    //        return _eye_fundus_image;
    //    //    }
    //    //}
    //}

    //public class PatientIdentifierRepository : IRepository
    //{
    //    static PatientIdentifierRepository _patRepo;

    //    public static PatientIdentifierRepository GetInstance()
    //    {
    //        if (_patRepo == null)
    //            _patRepo = new PatientIdentifierRepository();
    //        return _patRepo;
    //    }
    //    public PatientIdentifierRepository()
    //    {
    //    }

    //    public void Add<patient_identifier>(patient_identifier _patientIdentifier)
    //    {
    //        try
    //        {
    //            NHibernateHelper_MySQL.OpenSession();
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                NHibernateHelper_MySQL.hibernateSession.Save(_patientIdentifier);
    //                transaction.Commit();
    //            }
    //            //return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //            //return false;
    //        }
    //    }
    //    public patient_identifier GetIdentifier(int patIdentifierId)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            return NHibernateHelper_MySQL.hibernateSession.Get<patient_identifier>(patIdentifierId);
    //        }
    //    }
    //    public void Update<patient_identifier>(patient_identifier _patientIdentifier)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //        {
    //            NHibernateHelper_MySQL.hibernateSession.Update(_patientIdentifier);
    //            transaction.Commit();
    //        }
    //    }
    //    public void Remove<patient_identifier>(patient_identifier _identifier)
    //    {
    //        {
    //            Update(_identifier);
    //        }
    //    }
    //    public ICollection<patient_identifier> GetAll<patient_identifier>()
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            // var _visits = session.Query<VisitModel>().Where(x=>x.PatientID ==PatId).ToList();
    //            var _identifier = NHibernateHelper_MySQL.hibernateSession.Query<patient_identifier>().ToList().Where(x => x.voided == false).ToList();
    //            return _identifier;
    //        }
    //    }
    //    public ICollection<patient_identifier> GetByCategory(string _Category, object val)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                var _identifier = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(patient_identifier))
    //                     .Add(Restrictions.Eq(_Category, val))
    //                     .List<patient_identifier>();
    //                return _identifier;
    //            }
    //        }
    //    }

    //    public ICollection<patient_identifier> GetByPatientID(Patient patId)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            // var _visits = session.Query<VisitModel>().Where(x=>x.PatientID ==PatId).ToList();
    //            var _identifier = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(patient_identifier)).Add(Restrictions.Eq("patient", patId)).List<patient_identifier>().Where(x => x.voided == false).ToList();
    //            return _identifier;
    //        }
    //    }
    //}

    //public class NewReportRepository : IRepository
    //{
    //    static NewReportRepository _patRepo;
    //    public static NewReportRepository GetInstance()
    //    {
    //        if (_patRepo == null)
    //            _patRepo = new NewReportRepository();
    //        return _patRepo;
    //    }
    //    public NewReportRepository()
    //    {
    //    }
    //    public void Add<report>(report _report)
    //    {
    //        try
    //        {
    //            NHibernateHelper_MySQL.OpenSession();
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                NHibernateHelper_MySQL.hibernateSession.Save(_report);
    //                transaction.Commit();
    //            }
    //            //return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //            //return false;
    //        }
    //    }
    //    public report GetReport(int reportId)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            return NHibernateHelper_MySQL.hibernateSession.Get<report>(reportId);
    //        }
    //    }
    //    public void Update<report>(report _report)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //        {
    //            NHibernateHelper_MySQL.hibernateSession.Update(_report);
    //            transaction.Commit();
    //        }
    //    }
    //    public void Remove<report>(report _report)
    //    {
    //        {
    //            Update(_report);
    //        }
    //    }
    //    public ICollection<report> GetByCategory(string _Category, object val)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                var _report = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(report))
    //                     .Add(Restrictions.Eq(_Category, val))
    //                     .List<report>();
    //                return _report;
    //            }
    //        }
    //    }

    //    public ICollection<report> GetAll<report>()
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            // var _visits = session.Query<VisitModel>().Where(x=>x.PatientID ==PatId).ToList();
    //            var _report = NHibernateHelper_MySQL.hibernateSession.Query<report>().ToList().Where(x => x.voided == false).ToList();
    //            return _report;
    //        }
    //    }

    //    public int GetReportCount(int visitID)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            var _obs = NHibernateHelper_MySQL.hibernateSession.Query<report>().ToList().Where(x => x.visit.visitId == visitID).Where(x => x.voided == false).ToList();
    //            return _obs.Count;
    //        }
    //    }

    //    public ICollection<report> GetByPatientID(visit visitId)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            // var _visits = session.Query<VisitModel>().Where(x=>x.PatientID ==PatId).ToList();
    //            var _identifier = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(report)).Add(Restrictions.Eq("visit", visitId)).List<report>().Where(x => x.voided == false).ToList();
    //            return _identifier;
    //        }
    //    }
    //}

    //public class NewAnnotationRepository : IRepository
    //{
    //    static NewAnnotationRepository _patRepo;

    //    public static NewAnnotationRepository GetInstance()
    //    {
    //        if (_patRepo == null)
    //            _patRepo = new NewAnnotationRepository();
    //        return _patRepo;
    //    }
    //    public NewAnnotationRepository()
    //    {
    //    }
    //    public void Add<eye_fundus_image_annotation>(eye_fundus_image_annotation _patientIdentifier)
    //    {
    //        try
    //        {
    //            NHibernateHelper_MySQL.OpenSession();
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                NHibernateHelper_MySQL.hibernateSession.Save(_patientIdentifier);
    //                transaction.Commit();
    //            }
    //            //return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //            //return false;
    //        }
    //    }
    //    public eye_fundus_image_annotation GetAnnotation(int annotationId)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            return NHibernateHelper_MySQL.hibernateSession.Get<eye_fundus_image_annotation>(annotationId);
    //        }
    //    }
    //    public eye_fundus_image_annotation GetById(int annotationId)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            return NHibernateHelper_MySQL.hibernateSession.Get<eye_fundus_image_annotation>(annotationId);
    //        }
    //    }
    //    public void Update<eye_fundus_image_annotation>(eye_fundus_image_annotation _annotation)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //        {
    //            NHibernateHelper_MySQL.hibernateSession.Update(_annotation);
    //            transaction.Commit();
    //        }
    //    }
    //    public void Remove<eye_fundus_image_annotation>(eye_fundus_image_annotation _annotation)
    //    {
    //        {
    //            Update<eye_fundus_image_annotation>(_annotation);
    //        }
    //    }
    //    public ICollection<eye_fundus_image_annotation> GetAll<eye_fundus_image_annotation>()
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            // var _visits = session.Query<VisitModel>().Where(x=>x.PatientID ==PatId).ToList();
    //            var _eye_fundus_image_annotation = NHibernateHelper_MySQL.hibernateSession.Query<eye_fundus_image_annotation>().ToList().Where(x => x.voided == false).ToList();
    //            return _eye_fundus_image_annotation;
    //        }
    //    }
    //    public ICollection<eye_fundus_image_annotation> GetByCategory(string _Category, object val)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                var _annotation = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(eye_fundus_image_annotation))
    //                     .Add(Restrictions.Eq(_Category, val))
    //                     .List<eye_fundus_image_annotation>();
    //                return _annotation;
    //            }
    //        }
    //    }

    //    public ICollection<eye_fundus_image_annotation> GetByImageID(eye_fundus_image imageId)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            // var _visits = session.Query<VisitModel>().Where(x=>x.PatientID ==PatId).ToList();
    //            var _annotation = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(eye_fundus_image_annotation)).Add(Restrictions.Eq("eye_fundus_image_id", imageId)).List<eye_fundus_image_annotation>().Where(x => x.voided == false).ToList();
    //            return _annotation;
    //        }
    //    }
    //}

    //public class ConceptRepository : IRepository
    //{
    //    static ConceptRepository _patRepo;

    //    public static ConceptRepository GetInstance()
    //    {
    //        if (_patRepo == null)
    //            _patRepo = new ConceptRepository();
    //        return _patRepo;
    //    }
    //    public ConceptRepository()
    //    {
    //    }


    //    public void Add<Concept>(Concept _concept)
    //    {
    //        try
    //        {
    //            NHibernateHelper_MySQL.OpenSession();
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                NHibernateHelper_MySQL.hibernateSession.Save(_concept);
    //                transaction.Commit();
    //            }
    //            //return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //            //return false;
    //        }
    //    }
    //    public Concept GetAnnotation(int conceptId)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            return NHibernateHelper_MySQL.hibernateSession.Get<Concept>(conceptId);
    //        }
    //    }
    //    public void Update<Concept>(Concept _concept)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //        {
    //            NHibernateHelper_MySQL.hibernateSession.Update(_concept);
    //            transaction.Commit();
    //        }
    //    }
    //    public void Remove<Concept>(Concept _concept)
    //    {
    //        {
    //            Update(_concept);
    //        }
    //    }

    //    public ICollection<Concept> GetAll<Concept>()
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            // var _visits = session.Query<VisitModel>().Where(x=>x.PatientID ==PatId).ToList();
    //            var _concept = NHibernateHelper_MySQL.hibernateSession.Query<Concept>().ToList();
    //            return _concept;
    //        }
    //    }
    //    public ICollection<Concept> GetByCategory(string _Category, object val)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                var _concept = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(Concept))
    //                     .Add(Restrictions.Eq(_Category, val))
    //                     .List<Concept>();
    //                return _concept;
    //            }
    //        }
    //    }
    //}

    //public class OrganizationRepository : IRepository
    //{
    //    static OrganizationRepository _orgRepo;

    //    public static OrganizationRepository GetInstance()
    //    {
    //        if (_orgRepo == null)
    //            _orgRepo = new OrganizationRepository();
    //        return _orgRepo;
    //    }

    //    public OrganizationRepository()
    //    {
    //    }

    //    public void Add<organization>(organization _organization)
    //    {
    //        try
    //        {
    //            NHibernateHelper_MySQL.OpenSession();
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                NHibernateHelper_MySQL.hibernateSession.Save(_organization);
    //                transaction.Commit();
    //            }
    //            //return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //            //return false;
    //        }
    //    }

    //    public organization GetOrganization(int orgId)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            return NHibernateHelper_MySQL.hibernateSession.Get<organization>(orgId);
    //        }
    //    }

    //    public void Update<organization>(organization _organization)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        try
    //        {
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                NHibernateHelper_MySQL.hibernateSession.Update(_organization);
    //                transaction.Commit();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            int i = 0;
    //            int x = i;
    //        }
    //    }

    //    public void Remove<organization>(organization _organization)
    //    {
    //        {
    //            Update(_organization);
    //        }
    //    }

    //    public ICollection<organization> GetAll<organization>()
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            // var _visits = session.Query<VisitModel>().Where(x=>x.PatientID ==PatId).ToList();
    //            var _concept = NHibernateHelper_MySQL.hibernateSession.Query<organization>().ToList();
    //            return _concept;
    //        }
    //    }

    //    public ICollection<organization> GetByCategory(string _Category, object val)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                var _concept = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(organization))
    //                     .Add(Restrictions.Eq(_Category, val))
    //                     .List<organization>();
    //                return _concept;
    //            }
    //        }
    //    }
    //}

    //public class GlobalPropertyRepository : IRepository
    //{
    //    static GlobalPropertyRepository _globalRepo;

    //    public static GlobalPropertyRepository GetInstance()
    //    {
    //        if (_globalRepo == null)
    //            _globalRepo = new GlobalPropertyRepository();
    //        return _globalRepo;
    //    }

    //    public GlobalPropertyRepository()
    //    {
    //    }

    //    public void Add<global_property>(global_property _globalProperty)
    //    {
    //        try
    //        {
    //            NHibernateHelper_MySQL.OpenSession();
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                NHibernateHelper_MySQL.hibernateSession.Save(_globalProperty);
    //                transaction.Commit();
    //            }
    //            //return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //            //return false;
    //        }
    //    }

    //    public global_property GetAnnotation(int conceptId)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            return NHibernateHelper_MySQL.hibernateSession.Get<global_property>(conceptId);
    //        }
    //    }

    //    public void Update<global_property>(global_property _globalProperty)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        try
    //        {
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                NHibernateHelper_MySQL.hibernateSession.Update(_globalProperty);
    //                transaction.Commit();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            int i = 0;
    //            int x = i;
    //        }
    //    }

    //    public void Remove<global_property>(global_property _globalProperty)
    //    {
    //        {
    //            Update(_globalProperty);
    //        }
    //    }

    //    public ICollection<global_property> GetAll<global_property>()
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            var _concept = NHibernateHelper_MySQL.hibernateSession.Query<global_property>().ToList();
    //            return _concept;
    //        }
    //    }

    //    public ICollection<global_property> GetByCategory(string _Category, object val)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                var _concept = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(global_property))
    //                     .Add(Restrictions.Eq(_Category, val))
    //                     .List<global_property>();
    //                return _concept;
    //            }
    //        }
    //    }
    //}

    //public class UserRoleRepository : IRepository
    //{
    //    static UserRoleRepository _userRoleRepo;

    //    public static UserRoleRepository GetInstance()
    //    {
    //        if (_userRoleRepo == null)
    //            _userRoleRepo = new UserRoleRepository();
    //        return _userRoleRepo;
    //    }

    //    public UserRoleRepository()
    //    {
    //    }

    //    public void Add<user_role>(user_role _userRole)
    //    {
    //        try
    //        {
    //            NHibernateHelper_MySQL.OpenSession();
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                NHibernateHelper_MySQL.hibernateSession.Save(_userRole);
    //                transaction.Commit();
    //            }
    //            //return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //            //return false;
    //        }
    //    }

    //    public user_role GetUserRole(int conceptId)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            return NHibernateHelper_MySQL.hibernateSession.Get<user_role>(conceptId);
    //        }
    //    }

    //    public void Update<user_role>(user_role _userRole)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        try
    //        {
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                NHibernateHelper_MySQL.hibernateSession.Update(_userRole);
    //                transaction.Commit();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            int i = 0;
    //            int x = i;
    //        }
    //    }

    //    public void Remove<user_role>(user_role _userRole)
    //    {
    //        {
    //            Update(_userRole);
    //        }
    //    }

    //    public ICollection<user_role> GetAll<user_role>()
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            // var _visits = session.Query<VisitModel>().Where(x=>x.PatientID ==PatId).ToList();
    //            var _userRole = NHibernateHelper_MySQL.hibernateSession.Query<user_role>().ToList();
    //            return _userRole;
    //        }
    //    }

    //    public user_role GetByUserId(users user)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            user_role _visit = NHibernateHelper_MySQL.hibernateSession
    //                 .CreateCriteria(typeof(user_role))
    //                 .Add(Restrictions.Eq("user_id", user))
    //                 .UniqueResult<user_role>();
    //            return _visit;
    //        }
    //    }
    //    public ICollection<user_role> GetByCategory(string _Category, object val)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                var _userRole = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(user_role))
    //                     .Add(Restrictions.Eq(_Category, val))
    //                     .List<user_role>();
    //                return _userRole;
    //            }
    //        }
    //    }
    //}

    //public class RoleRepository : IRepository
    //{
    //    static RoleRepository _roleRepo;

    //    public static RoleRepository GetInstance()
    //    {
    //        if (_roleRepo == null)
    //            _roleRepo = new RoleRepository();
    //        return _roleRepo;
    //    }

    //    public RoleRepository()
    //    {
    //    }

    //    public void Add<Role>(Role _role)
    //    {
    //        try
    //        {
    //            NHibernateHelper_MySQL.OpenSession();
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                NHibernateHelper_MySQL.hibernateSession.Save(_role);
    //                transaction.Commit();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //        }
    //    }

    //    public Role GetUserRole<Role>(int conceptId)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            return NHibernateHelper_MySQL.hibernateSession.Get<Role>(conceptId);
    //        }
    //    }

    //    public void Update<Role>(Role _userRole)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        try
    //        {
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                NHibernateHelper_MySQL.hibernateSession.Update(_userRole);
    //                transaction.Commit();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            int i = 0;
    //            int x = i;
    //        }
    //    }

    //    public void Remove<Role>(Role _Role)
    //    {
    //        {
    //            Update(_Role);
    //        }
    //    }

    //    public ICollection<Role> GetAll<Role>()
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            // var _visits = session.Query<VisitModel>().Where(x=>x.PatientID ==PatId).ToList();
    //            var _role = NHibernateHelper_MySQL.hibernateSession.Query<Role>().ToList();
    //            return _role;
    //        }
    //    }

    //    public ICollection<Role> GetByCategory(string _Category, object val)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            using (ITransaction transaction = NHibernateHelper_MySQL.hibernateSession.BeginTransaction())
    //            {
    //                var _role = NHibernateHelper_MySQL.hibernateSession.CreateCriteria(typeof(Role))
    //                     .Add(Restrictions.Eq(_Category, val))
    //                     .List<Role>();
    //                return _role;
    //            }
    //        }
    //    }

    //    public Role GetByName(string name)
    //    {
    //        NHibernateHelper_MySQL.OpenSession();
    //        {
    //            Role _role = NHibernateHelper_MySQL.hibernateSession
    //                 .CreateCriteria(typeof(Role))
    //                 .Add(Restrictions.Eq("roleId", name))
    //                 .UniqueResult<Role>();
    //            return _role;
    //        }
    //    }
    //}

    public static class NewDataVariables
    {
        public static Repository _Repo;
        //public static NewVistRepository _visitViewRepo;// = NewVistRepository.GetInstance();
        //public static NewPatientRepository _patientRepo;// = NewPatientRepository.GetInstance();
        //public static ObsRepository _imageRepo;// = ObsRepository.GetInstance();
        //public static PersonAddressRepository _personaddressRepo;// = PersonAddressRepository.GetInstance();
        //public static PersonAttributeRepository _personattributeRepo;// = PersonAttributeRepository.GetInstance();
        //public static EyeFundusImageRepository _eyeimagerepo;// = EyeFundusImageRepository.GetInstance();
        //public static PatientIdentifierRepository _patIdentfier;// = PatientIdentifierRepository.GetInstance();
        //public static NewReportRepository _reportRepo;// = NewReportRepository.GetInstance();
        //public static NewAnnotationRepository _annotationRepo;// = NewAnnotationRepository.GetInstance();
        //public static ConceptRepository _conceptRepo;// = ConceptRepository.GetInstance();
        //public static GlobalPropertyRepository _globalProperty;//
        //public static UserRepository _users;//
        //public static UserRoleRepository _usersRoleRepo;
        //public static RoleRepository _roleRepo;
        //public static OrganizationRepository _orgRepo;
        //public static ObservationAttributeRepository _obsAttributeRepo;

        //public static ReporttRepository _reportRepo;
        private static List<Person> _persons;
        public static List<Person> Persons
        {
            get { return _persons; }
            set { _persons = value; }
        }

        private static List<Patient> _patients;
        public static List<Patient> Patients
        {
            get
            {
                if (_patients == null)
                    //_patients = _Repo.GetAll<Patient>().ToList();
                _patients = new List<Patient>();
                return _patients; 
            }
            set { _patients = value; }
        }

        private static List<person_attribute> _attribute;
        public static List<person_attribute> Attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }

        private static List<Concept> _concept;
        public static List<Concept> Concept
        {
            get { return _concept; }
            set { _concept = value; }
        }

        private static List<patient_identifier> _identifier;
        public static List<patient_identifier> Identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }

        private static List<PersonAddressModel> _address;
        public static List<PersonAddressModel> Address
        {
            get { return _address; }
            set { _address = value; }
        }

        private static List<eye_fundus_image> _obs;

        public static List<eye_fundus_image> Obs
        {
            get { return _obs; }
            set { _obs = value; }
        }
        private static List<visit> _visits;

        public static List<visit> Visits
        {
            get { return _visits; }
            set { _visits = value; }
        }

        private static List<PatientDiagnosis> _patientDiagnosis;

        public static List<PatientDiagnosis> PatientDiagnosis
        {
            get { return _patientDiagnosis; }
            set { _patientDiagnosis = value; }
        }

        private static List<report> _reports;

        public static List<report> Reports
        {
            get { return _reports; }
            set { _reports = value; }
        }
        private static List<eye_fundus_image_annotation> _annotations;

        public static List<eye_fundus_image_annotation> Annotations
        {
            get { return _annotations; }
            set { _annotations = value; }
        }

        //private static List<eye_fundus_image> _eyeFundsImage;

        //public static List<eye_fundus_image> EyeFundusImage
        //{
        //    get { return _eyeFundsImage; }
        //    set { _eyeFundsImage = value; }
        //}
        private static List<users> _Users;

        public static List<users> Users
        {
            get { return _Users; }
            set { _Users = value; }
        }

        private static List<Role> _Role;

        public static List<Role> Role
        {
            get { return _Role; }
            set { _Role = value; }
        }
        public static Patient GetCurrentPat()
        {
            return Patients.Where(x => x.personId == _Active_Patient).ToList()[0];
        }
        private static int _Active_Patient;

        public static int Active_Patient
        {
            get { return _Active_Patient; }
            set
            {
                if (value != null)
                {
                    _Active_Patient =  value;

                    Patient p = GetCurrentPat();
                    //Visits = _Repo.GetByCategory<visit>("patient", _Active_Patient).OrderByDescending(x => x.createdDate < DateTime.Now).ToList();
                    Visits = p.visits.Where(x => x.voided == false).OrderByDescending(x => x.createdDate < DateTime.Now).ToList();
                    Visits.Reverse();
                    //_address = _Repo.GetByCategory<PersonAddressModel>("person", _Active_Patient).OrderByDescending(x => x.createdDate < DateTime.Now).ToList();
                    Address = p.addresses.Where(x => x.voided == false).OrderByDescending(x => x.createdDate < DateTime.Now).ToList();
                    Address.Reverse();
                    //_attribute = _Repo.GetByCategory<person_attribute>("person", _Active_Patient).ToList<person_attribute>();
                    Attribute = p.attributes.Where(x => x.voided == false).OrderByDescending(x => x.createdDate < DateTime.Now).ToList();
                    //_identifier = _Repo.GetByCategory<patient_identifier>("patient", _Active_Patient).OrderByDescending(x => x.createdDate < DateTime.Now).ToList();
                    Identifier = p.identifiers.Where(x => x.voided == false).OrderByDescending(x => x.createdDate < DateTime.Now).ToList();

                    //PatientDiagnosis = _Repo.GetByCategory<PatientDiagnosis>("patient", _Active_Patient).OrderByDescending(x => x.createdDate < DateTime.Now).ToList<PatientDiagnosis>();
                    PatientDiagnosis = p.diagnosis.Where(x => x.voided == false).OrderByDescending(x => x.createdDate < DateTime.Now).ToList();
                    PatientDiagnosis.Reverse();
                }
            }
        }
        //private static Patient _Active_Patient;

        //public static Patient Active_Patient
        //{
        //    get { return _Active_Patient; }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            _Active_Patient = value;
        //            //Visits = _Repo.GetByCategory<visit>("patient", _Active_Patient).OrderByDescending(x => x.createdDate < DateTime.Now).ToList();
        //            Visits = _Active_Patient.visits.Where(x => x.voided == false).OrderByDescending(x => x.createdDate < DateTime.Now).ToList();
        //            Visits.Reverse();
        //            //_address = _Repo.GetByCategory<PersonAddressModel>("person", _Active_Patient).OrderByDescending(x => x.createdDate < DateTime.Now).ToList();
        //            Address = _Active_Patient.addresses.Where(x => x.voided == false).OrderByDescending(x => x.createdDate < DateTime.Now).ToList();
        //            Address.Reverse();
        //            //_attribute = _Repo.GetByCategory<person_attribute>("person", _Active_Patient).ToList<person_attribute>();
        //            Attribute = _Active_Patient.attributes.Where(x => x.voided == false).OrderByDescending(x => x.createdDate < DateTime.Now).ToList();
        //            //_identifier = _Repo.GetByCategory<patient_identifier>("patient", _Active_Patient).OrderByDescending(x => x.createdDate < DateTime.Now).ToList();
        //            Identifier = _Active_Patient.identifiers.Where(x => x.voided == false).OrderByDescending(x => x.createdDate < DateTime.Now).ToList();

        //            //PatientDiagnosis = _Repo.GetByCategory<PatientDiagnosis>("patient", _Active_Patient).OrderByDescending(x => x.createdDate < DateTime.Now).ToList<PatientDiagnosis>();
        //            PatientDiagnosis = _Active_Patient.diagnosis.Where(x => x.voided == false).OrderByDescending(x => x.createdDate < DateTime.Now).ToList();
        //            PatientDiagnosis.Reverse();
        //        }
        //    }
        //}
        private static users _Active_User;

        public static users Active_User
        {
            get { return _Active_User; }
            set
            {
                _Active_User = value;
            }
        }

        private static visit _visit;

        public static visit Active_Visit
        {
            get { return _visit; }
            set
            {
                _visit = value;
                //Obs = _Repo.GetByCategory<obs>("visit", Active_Visit).ToList();
                Obs = _visit.observations.Where(x => x.voided == false).OrderBy(x => x.createdDate).ToList();
                Reports = _visit.reports.Where(x => x.voided == false).OrderByDescending(x => x.createdDate).ToList();
                
                //if (EyeFundusImage == null)
                //    EyeFundusImage = new List<eye_fundus_image>();
                //else
                //    EyeFundusImage.Clear();
                //foreach (var item in Obs)
                //{
                //    //eye_fundus_image eyeFundusImage = _Repo.GetById<eye_fundus_image>(item.observationId);
                //    EyeFundusImage.Add(item.eye_fundus_image);
                //}
                //Reports = _Repo.GetByCategory<report>("visit", Active_Visit).ToList();
            }
        }
        private static eye_fundus_image _Obs;

        public static eye_fundus_image Active_Obs
        {
            get { return _Obs; }
            set
            {
                _Obs = value;
                //Active_EyeFundusImage = _Obs.eye_fundus_image;
                 //Active_EyeFundusImage =  EyeFundusImage.Where(x => x.eye_fundus_image_id == value.observationId).ToList()[0];

                Annotations = _Obs.eye_fundus_image_annotations.Where(x => x.voided == false).OrderByDescending(x => x.createdDate).ToList();// _Repo.GetByCategory<eye_fundus_image_annotation>("eyeFundusImage", Active_EyeFundusImage).OrderByDescending(x => x.createdDate < DateTime.Now).ToList(); ;
            }
        }

        private static report _report;

        public static report Active_Report
        {
            get { return _report; }
            set { _report = value; }
        }

        private static patient_identifier _patientIdentifier;

        public static patient_identifier Active_PatientIdentifier
        {
            get { return _patientIdentifier; }
            set { _patientIdentifier = value; }
        }

        private static PersonAddressModel _personAddressModel;

        public static PersonAddressModel Active_PersonAddressModel
        {
            get { return _personAddressModel; }
            set { _personAddressModel = value; }
        }

        private static users _Active_Login_User;

        public static users Active_Login_User
        {
            get { return _Active_Login_User; }
            set
            {
                _Active_Login_User = value;
            }
        }

        //private static eye_fundus_image _eyeFundusImage;

        //public static eye_fundus_image Active_EyeFundusImage
        //{
        //    get { return _eyeFundusImage; }
        //    set { _eyeFundusImage = value; }
        //}

        private static eye_fundus_image_annotation _annotation;

        public static eye_fundus_image_annotation Active_Annotation
        {
            get { return _annotation; }
            set
            {
                _annotation = value;
            }
        }
    }

    public static class NewIVLDataMethods
    {
        public static void UpdatePatient()
        {
            Patient np =NewDataVariables.Patients.Where(x=>x.personId == NewDataVariables.Active_Patient).ToList()[0];
            //np.patientLastModifiedDate = DateTime.Now;
            np.lastAccessedDate = DateTime.Now;
            NewDataVariables._Repo.Update<Patient>(np);
            //NewDataVariables.Visits = NewDataVariables._Repo.GetByCategory<visit>("patient", NewDataVariables.Active_Patient).ToList();
            //NewDataVariables.Visits = NewDataVariables.Active_Patient.visits.ToList(); //NewDataVariables._Repo.GetByCategory<visit>("patient", NewDataVariables.Active_Patient).ToList();
            NewDataVariables.Patients[NewDataVariables.Patients.FindIndex(x => x.personId == NewDataVariables.Active_Patient)] = NewDataVariables.GetCurrentPat();
            NewDataVariables.Patients = NewDataVariables.Patients.Where(x => x.voided == false).ToList();//.OrderByDescending(x => x.RegistrationDateTime < DateTime.Now).ToList();
            NewDataVariables.Patients.Sort((x, y) => x.createdDate.CompareTo(y.createdDate));
            NewDataVariables.Patients.Reverse();
        }

        public static void AddPatient(Patient patient)
        {
            bool isPatAdded =  NewDataVariables._Repo.Add<Patient>(patient);
            //NewDataVariables.Active_Patient = patient;
            NewDataVariables.Active_Patient = patient.personId;
            //NewDataVariables.Patients.Add(patient);//.GetAll().ToList();
            NewDataVariables.Patients = NewDataVariables.Patients.Where(x => x.voided == false).ToList();//.OrderByDescending(x => x.RegistrationDateTime < DateTime.Now).ToList();
            NewDataVariables.Patients.Sort((x, y) => x.createdDate.CompareTo(y.createdDate));
            NewDataVariables.Patients.Reverse();
        }

        public static void RemovePatient()
        {
            UpdatePatient();
            //UpdatePatientIdentifier();
            //UpdatePatientAddress();
        }

        public static void AddPatientIdentifier(patient_identifier identifier)
        {
            NewDataVariables._Repo.Add<patient_identifier>(identifier);
            NewDataVariables.Active_PatientIdentifier = identifier;
            NewDataVariables.Identifier.Add(identifier);
            NewDataVariables.Patients.Where(x=>x.personId==NewDataVariables.Active_Patient).ToList()[0].identifiers = new HashSet<patient_identifier>();
            NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].identifiers.Add(NewDataVariables.Active_PatientIdentifier);
            NewDataVariables.Identifier = NewDataVariables.Identifier.Where(X => X.voided == false).ToList();
            NewDataVariables.Identifier.Sort((x, y) => x.createdDate.CompareTo(y.createdDate));
            NewDataVariables.Identifier.Reverse();
        }

        public static void UpdatePatientIdentifier()
        {
            NewDataVariables._Repo.Update<patient_identifier>(NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].identifiers.ToList()[0]);
            NewDataVariables.Identifier[NewDataVariables.Identifier.FindIndex(x => x.patientIdentifierId == NewDataVariables.Patients.Where(y => y.personId == NewDataVariables.Active_Patient).ToList()[0].identifiers.ToList()[0].patientIdentifierId)] = NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].identifiers.ToList()[0];
            NewDataVariables.Identifier = NewDataVariables.Identifier.Where(x => x.voided == false).ToList();//.OrderByDescending(x => x.RegistrationDateTime < DateTime.Now).ToList();
            NewDataVariables.Identifier.Sort((x, y) => x.createdDate.CompareTo(y.createdDate));
            NewDataVariables.Identifier.Reverse();
        }

        public static void AddPersonAddress(PersonAddressModel address)
        {
            NewDataVariables._Repo.Add<PersonAddressModel>(address);
            NewDataVariables.Active_PersonAddressModel = address;
            NewDataVariables.Address.Add(address);
            NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].addresses = new HashSet<PersonAddressModel>();
            NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].addresses.Add(NewDataVariables.Active_PersonAddressModel);
            NewDataVariables.Address = NewDataVariables.Address.Where(X => X.voided == false).ToList();
            NewDataVariables.Address.Sort((x, y) => x.createdDate.CompareTo(y.createdDate));
            NewDataVariables.Address.Reverse();
        }

        public static void UpdatePatientAddress()
        {
            NewDataVariables._Repo.Update<PersonAddressModel>(NewDataVariables.Active_PersonAddressModel);
            NewDataVariables.Address[NewDataVariables.Address.FindIndex(x => x.person == NewDataVariables.Active_PersonAddressModel.person)] = NewDataVariables.Active_PersonAddressModel;
            NewDataVariables.Address = NewDataVariables.Address.Where(x => x.voided == false).ToList();//.OrderByDescending(x => x.RegistrationDateTime < DateTime.Now).ToList();
            NewDataVariables.Address.Sort((x, y) => x.createdDate.CompareTo(y.createdDate));
            NewDataVariables.Address.Reverse();
        }

        //public static void UpdateVisit()
        //{
        //    NewDataVariables.Active_Visit.lastModifiedDate = DateTime.Now;
        //    NewDataVariables._Repo.Update<visit>(NewDataVariables.Active_Visit);
        //    int indx = NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].visits.ToList().FindIndex(x => x.visitId == NewDataVariables.Active_Visit.visitId);
        //    NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].visits.ToList()[indx] = NewDataVariables.Active_Visit; //Active_Patient.visits.ToList().Where(x => x.HideShowRow == false).ToList();
        //    NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].visits.ToList().Where(x => x.voided == false).ToList();//.OrderByDescending(x => x.VisitDateTime < DateTime.Now).ToList();
        //    //NewDataVariables.Active_Patient.visits.ToList().Sort((x, y) => x.createdDate.CompareTo(y.createdDate));
        //    NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].visits.ToList().OrderByDescending(x => x.createdDate);
        //    NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].visits.Reverse();
        //}

        //public static void AddVisit(visit visit)
        //{
        //    NewDataVariables._Repo.Add<visit>(visit);
        //    NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].visits.Add(visit);
        //    NewDataVariables.Patients.Where(x => x.personId == NewDataVariables.Active_Patient).ToList()[0].visits.Where(x => x.voided == false).OrderByDescending(x => x.createdDate);
        //    //NewDataVariables.Active_Patient.visits.Reverse();
        //    //NewDataVariables.Visits.Add(visit);
        //    //NewDataVariables.Visits = NewDataVariables.Visits.Where(x => x.voided == false).ToList();//.OrderByDescending(x => x.VisitDateTime < DateTime.Now).ToList();
        //    //NewDataVariables.Visits.Sort((x, y) => x.createdDate.CompareTo(y.createdDate));
        //    //NewDataVariables.Visits.Reverse();
        //}

        //public static void RemoveVisit()
        //{
        //    UpdateVisit();
        //}

        //public static void UpdateReport()
        //{
        //    NewDataVariables._Repo.Update<report>(NewDataVariables.Active_Report);
        //    NewDataVariables.Reports[NewDataVariables.Reports.FindIndex(x => x.reportId == NewDataVariables.Active_Report.reportId)] = NewDataVariables.Active_Report;
        //    NewDataVariables.Reports = NewDataVariables.Reports.Where(x => x.voided == false).ToList(); //.OrderByDescending(x=>x.ReportDateTime< DateTime.Now) .ToList();
        //    NewDataVariables.Reports.Sort((x, y) => x.createdDate.CompareTo(y.createdDate));
        //    NewDataVariables.Reports.Reverse();
        //}

        //public static void AddReport(report r)
        //{
        //    NewDataVariables._Repo.Add<report>(r);
        //    NewDataVariables.Active_Report = r;
        //    NewDataVariables.Reports.Add(NewDataVariables.Active_Report);// = DataVariables.Active_Visit.Reports.Where(x => x.HideShowRow == 0).ToList();
        //    NewDataVariables.Reports = NewDataVariables.Reports.Where(x => x.voided == false).ToList(); //.OrderByDescending(x=>x.ReportDateTime< DateTime.Now) .ToList();
        //    NewDataVariables.Reports.Sort((x, y) => x.createdDate.CompareTo(y.createdDate));
        //    NewDataVariables.Reports.Reverse();
        //}

        //public static void RemoveReport()
        //{
        //    UpdateReport();
        //}

        //public static void UpdateImage()
        //{
        //    NewDataVariables._Repo.Update<obs>(NewDataVariables.Active_Obs);
        //    //NewDataVariables._Repo.Update<eye_fundus_image>(NewDataVariables.Active_EyeFundusImage);
        //    NewDataVariables.Obs[NewDataVariables.Obs.FindIndex(x => x.observationId == NewDataVariables.Active_Obs.observationId)] = NewDataVariables.Active_Obs;
        //    NewDataVariables.Obs = NewDataVariables.Obs.Where(x => x.voided == false).ToList();//.OrderByDescending(x=>x.ImageDateTime < DateTime.Now) .ToList();
        //    NewDataVariables.Obs.Sort((x, y) => x.createdDate.CompareTo(y.createdDate));
        //    //NewDataVariables.EyeFundusImage[NewDataVariables.EyeFundusImage.FindIndex(x => x.eye_fundus_image_id == NewDataVariables.Active_EyeFundusImage.eye_fundus_image_id)] = NewDataVariables.Active_EyeFundusImage;
        //}

        //public static void AddImage(obs i)
        //{
        //    bool retVal =  NewDataVariables._Repo.Add<obs>(i);
        //   // NewDataVariables.Active_Obs = i;
        //  //  NewDataVariables.Obs.Add(i);
        //    NewDataVariables.Obs = NewDataVariables.Obs.Where(x => x.voided == false).ToList();
        //    NewDataVariables.Obs.Sort((x, y) => x.createdDate.CompareTo(y.createdDate));
        //}

        //public static void RemoveImage()
        //{
        //    UpdateImage();
        //}

        //public static void UpdateAnnotation()
        //{
        //    NewDataVariables._Repo.Update<eye_fundus_image_annotation>(NewDataVariables.Active_Annotation);
        //    NewDataVariables.Annotations[NewDataVariables.Annotations.FindIndex(x => x.eyeFundusImageAnnotationId == NewDataVariables.Active_Annotation.eyeFundusImageAnnotationId)] = NewDataVariables.Active_Annotation;
        //    NewDataVariables.Annotations = NewDataVariables.Annotations.Where(x => x.voided == false).ToList();
        //    NewDataVariables.Annotations.Sort((x, y) => x.createdDate.CompareTo(y.createdDate));
        //    NewDataVariables.Annotations.Reverse();
        //}

        //public static void AddAnnotation(eye_fundus_image_annotation a)
        //{
        //    NewDataVariables._Repo.Add<eye_fundus_image_annotation>(a);
        //    NewDataVariables.Active_Annotation = a;
        //    NewDataVariables.Annotations.Add(NewDataVariables.Active_Annotation);
        //    NewDataVariables.Annotations = NewDataVariables.Annotations.Where(x => x.voided == false).ToList();
        //    NewDataVariables.Annotations.Sort((x, y) => x.createdDate.CompareTo(y.createdDate));
        //    NewDataVariables.Annotations.Reverse();
        //}

        //public static void RemoveAnnotation()
        //{
        //    UpdateAnnotation();
        //}

        public static void UpdateUser()
        {
            NewDataVariables._Repo.Update<users>(NewDataVariables.Active_User);
            NewDataVariables.Users[NewDataVariables.Users.FindIndex(x => x.userId == NewDataVariables.Active_User.userId)] = NewDataVariables.Active_User;
            NewDataVariables.Users = NewDataVariables.Users.Where(x => x.voided == false).ToList();
            NewDataVariables.Users.Sort((x, y) => x.createdDate.CompareTo(y.createdDate));
            NewDataVariables.Users.Reverse();
        }

        public static void AddUser(users a)
        {
            NewDataVariables._Repo.Add<users>(a);
            NewDataVariables.Active_User = a;
            NewDataVariables.Users.Add(NewDataVariables.Active_User);
            NewDataVariables.Users = NewDataVariables.Users.Where(x => x.voided == false).ToList();
            NewDataVariables.Users.Sort((x, y) => x.createdDate.CompareTo(y.createdDate));
            NewDataVariables.Users.Reverse();
        }

        public static void RemoveUser()
        {
            UpdateUser();
        }

        public static void AddEyeFundusImage(eye_fundus_image eye)
        {
            NewDataVariables._Repo.Add<eye_fundus_image>(eye);
            //NewDataVariables.Active_EyeFundusImage = eye;
            //NewDataVariables.EyeFundusImage.Add(NewDataVariables.Active_EyeFundusImage);
            //NewDataVariables.EyeFundusImage = NewDataVariables.EyeFundusImage.ToList();
            //NewDataVariables.EyeFundusImage.Reverse();
        }
    }
}
