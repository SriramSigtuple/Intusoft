using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using NHibernate.Engine;
using NHibernate.Proxy;
using Newtonsoft.Json.Converters;
using Microsoft.Win32;
using System.Windows.Forms;
using INTUSOFT.Data.Model;
using INTUSOFT.Data.NewDbModel;
using NHibernate.Linq;
using NHibernate;
using NHibernate.Criterion;

namespace INTUSOFT.Data.Repository
{
  public  class OldRepositories
    {
        public class PatientRepository : IPatientRepository
        {
            static PatientRepository _patRepo;

            public static PatientRepository GetInstance()
            {
                if (_patRepo == null)
                    _patRepo = new PatientRepository();
                return _patRepo;
            }
            public PatientRepository()
            {
            }


            public ICollection<INTUSOFT.Data.Model.Patient> Search(INTUSOFT.Data.Model.Patient _patient)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {

                    //The below code has been modified by assing var disjunction to solve Defect no0000080 to add or implementation in search.            

                    var criteria = session.CreateCriteria<INTUSOFT.Data.Model.Patient>();
                    var disjunction = Restrictions.Disjunction();
                    if (_patient.ID != 0)
                    {

                        disjunction.Add(Restrictions.Eq("Id", _patient.ID));

                    }
                    if (!string.IsNullOrEmpty(_patient.MRN))
                    {

                        disjunction.Add(Restrictions.Like("MRN", _patient.MRN, MatchMode.Anywhere));

                    }
                    if (!string.IsNullOrEmpty(_patient.FirstName))
                    {
                        disjunction.Add(Restrictions.Like("FirstName", _patient.FirstName, MatchMode.Anywhere));

                    }

                    if (!string.IsNullOrEmpty(_patient.LastName))
                    {
                        disjunction.Add(Restrictions.Like("LastName", _patient.LastName, MatchMode.Anywhere));

                    }
                    if (!string.IsNullOrEmpty(_patient.Gender))
                    {
                        disjunction.Add(Restrictions.Eq("Gender", _patient.Gender));

                    }
                    if ((DateTime.Now.Year - _patient.DOB.Year) > 3)
                    {
                        disjunction.Add(Restrictions.Eq("DOB", _patient.DOB));

                    }

                    IList<INTUSOFT.Data.Model.Patient> pats = criteria.Add(disjunction).List<INTUSOFT.Data.Model.Patient>().Where(x => x.HideShowRow == false).ToList(); ;
                    return pats;
                }
            }
            public bool Add(INTUSOFT.Data.Model.Patient _patient)
            {
                try
                {
                    using (ISession session = NHibernateHelper.OpenSession())
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(_patient);
                        transaction.Commit();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            public void Update(INTUSOFT.Data.Model.Patient _patient)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(_patient);
                    transaction.Commit();
                }
            }
            public void Remove(INTUSOFT.Data.Model.Patient _patient)
            {
                _patient.HideShowRow = true;
                //using (ISession session = NHibernateHelper.OpenSession())
                //using (ITransaction transaction = session.BeginTransaction())
                {

                    // session.Delete(_patient);
                    //transaction.Commit();
                    Update(_patient);
                }
            }

            public INTUSOFT.Data.Model.Patient GetById(int id)
            {
                using (ISession session = NHibernateHelper.OpenSession())

                    return session.Get<INTUSOFT.Data.Model.Patient>(id);
            }
            public ICollection<INTUSOFT.Data.Model.Patient> GetByCategory(string _Category, object val)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        var _patients = session.CreateCriteria(typeof(INTUSOFT.Data.Model.Patient))
                             .Add(Restrictions.Eq(_Category, val))
                             .List<INTUSOFT.Data.Model.Patient>();

                        return _patients;
                    }


                }
            }
            public ICollection<INTUSOFT.Data.Model.Patient> SearchByPatMrn(string mrn)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    return session.Query<INTUSOFT.Data.Model.Patient>().ToList().Where(x => x.MRN.ToLower().Contains(mrn.ToLower()) && x.HideShowRow == false).ToList();
                }

            }
            public ICollection<INTUSOFT.Data.Model.Patient> SearchByPatFirstName(string FirstName)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    return session.Query<INTUSOFT.Data.Model.Patient>().ToList().Where(x => x.FirstName.ToLower().Contains(FirstName.ToLower()) && x.HideShowRow == false).ToList();
                }

            }
            public ICollection<INTUSOFT.Data.Model.Patient> SearchByPatLastName(string LastName)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    return session.Query<INTUSOFT.Data.Model.Patient>().ToList().Where(x => x.LastName.ToLower().Contains(LastName.ToLower()) && x.HideShowRow == false).ToList();
                }

            }
            public ICollection<INTUSOFT.Data.Model.Patient> GetByDate(string date)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    var _patients = session
                             .CreateCriteria<INTUSOFT.Data.Model.Patient>().Add(
               Restrictions.Eq("RegistrationDateTime", date)).List<INTUSOFT.Data.Model.Patient>();
                    return _patients;


                }
            }

            public INTUSOFT.Data.Model.Patient GetByName(string name)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    DetachedCriteria criteria = null;

                    if (!string.IsNullOrEmpty(name))
                    {
                        criteria = DetachedCriteria.For(typeof(INTUSOFT.Data.Model.Patient));
                        criteria.Add(Expression.Eq("HideShowRow", false));
                    }
                    INTUSOFT.Data.Model.Patient _patient = criteria.GetExecutableCriteria(session).UniqueResult<INTUSOFT.Data.Model.Patient>();
                    //return pats;
                    //  _patient = session
                    //      .CreateCriteria(typeof(Patient))
                    //      .Add(Restrictions.Eq("Name", name)Restrictions.Eq()
                    //      .UniqueResult<Patient>();
                    return _patient;
                }
            }

            public ICollection<INTUSOFT.Data.Model.Patient> GetAll()
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
                    st.Start();
                    var patients = session.Query<INTUSOFT.Data.Model.Patient>().ToList();
                    st.Stop();
                    Console.WriteLine(st.ElapsedMilliseconds);
                    DataVariables.Patients = patients.ToList();
                    return patients;
                }
            }
        }

        public class VisitRepository : IVisitRepository
        {
            static VisitRepository _visitRepo;

            public static VisitRepository GetInstance()
            {
                if (_visitRepo == null)
                    _visitRepo = new VisitRepository();
                return _visitRepo;
            }
            public VisitRepository()
            {

            }
            public bool Add(VisitModel _Visit)
            {
                try
                {
                    using (ISession session = NHibernateHelper.OpenSession())
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(_Visit);
                        transaction.Commit();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            public void Update(VisitModel _Visit)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(_Visit);
                    transaction.Commit();
                }
            }
            public void Remove(VisitModel _Visit)
            {
                //using (ISession session = NHibernateHelper.OpenSession())
                //using (ITransaction transaction = session.BeginTransaction())
                {
                    //session.Delete(_Visit);
                    //transaction.Commit();
                    Update(_Visit);
                }
            }

            public VisitModel GetById(int id)
            {
                using (ISession session = NHibernateHelper.OpenSession())

                    return session.Get<VisitModel>(id);
            }
            public ICollection<VisitModel> GetByCategory(string _Category, object val)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    var _visits = session
                             .CreateCriteria(typeof(VisitModel))
                             .Add(Restrictions.Eq(_Category, val))
                             .List<VisitModel>().Where(x => x.HideShowRow == false).ToList();
                    return _visits;
                }
            }
            public ICollection<VisitModel> GetByPatID(int PatId)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    var _visits = session.Query<VisitModel>().Where(x => x.PatientID == PatId).ToList<VisitModel>();
                    //var _visits = session.CreateCriteria(typeof(VisitModel)).Add(Restrictions.Eq("PatientID", PatId)).List<VisitModel>().ToList();
                    return _visits;

                }
            }

            public ICollection<VisitModel> GetAll()
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    // var _visits = session.Query<VisitModel>().Where(x=>x.PatientID ==PatId).ToList();
                    var _visits = session.Query<VisitModel>().ToList().ToList();
                    return _visits;

                }
            }
            public VisitModel GetByName(string name)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    VisitModel _visit = session
                         .CreateCriteria(typeof(VisitModel))
                         .Add(Restrictions.Eq("Name", name))
                         .UniqueResult<VisitModel>();
                    return _visit;
                }
            }

        }

        public class ImageRepository : IImageRepository
        {
            static ImageRepository _ImageRepo;

            public static ImageRepository GetInstance()
            {
                if (_ImageRepo == null)
                    _ImageRepo = new ImageRepository();
                return _ImageRepo;
            }
            public ImageRepository()
            {

            }
            public bool Add(ImageModel _image)
            {
                try
                {
                    using (ISession session = NHibernateHelper.OpenSession())
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(_image);
                        transaction.Commit();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            public void Update(ImageModel _image)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(_image);
                    transaction.Commit();
                }
            }
            public void Remove(ImageModel _image)
            {
                //using (ISession session = NHibernateHelper.OpenSession())
                //using (ITransaction transaction = session.BeginTransaction())
                {
                    //session.Delete(_image);
                    //transaction.Commit();
                    Update(_image);
                }
            }

            public ImageModel GetById(int id)
            {
                using (ISession session = NHibernateHelper.OpenSession())

                    return session.Get<ImageModel>(id);
            }
            public ICollection<ImageModel> GetByVisitId(int id)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    var _images = session.Query<ImageModel>().ToList().Where(x => x.VisitID == id).ToList();
                    return _images;
                }
            }
            public ICollection<ImageModel> GetByCategory(string _Category, object val)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    var _images = session
                             .CreateCriteria(typeof(ImageModel))
                             .Add(Restrictions.Eq(_Category, val))
                             .List<ImageModel>().Where(x => x.HideShowRow == false).ToList();
                    return _images;

                }
            }

            public ImageModel GetByName(string name)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    ImageModel _image = session
                         .CreateCriteria(typeof(INTUSOFT.Data.Model.Patient))
                         .Add(Restrictions.Eq("Name", name))
                         .UniqueResult<ImageModel>();
                    return _image;
                }
            }
            public ICollection<ImageModel> GetAll()
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    var _annotations = session
                             .Query<ImageModel>()
                             .ToList<ImageModel>().ToList();
                    return _annotations;

                }
            }
        }

        public class ReporttRepository : IReportRepository
        {
            static ReporttRepository _reportRepo;

            public static ReporttRepository GetInstance()
            {
                if (_reportRepo == null)
                    _reportRepo = new ReporttRepository();
                return _reportRepo;
            }
            public ReporttRepository()
            {

            }
            public void Add(Report _report)
            {
                try
                {
                    using (ISession session = NHibernateHelper.OpenSession())
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(_report);
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }

            }
            public void Update(Report _report)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(_report);
                    transaction.Commit();
                }
            }
            public void Remove(Report _report)
            {
                //using (ISession session = NHibernateHelper.OpenSession())
                //using (ITransaction transaction = session.BeginTransaction())
                {
                    Update(_report);
                }
            }

            public Report GetById(int id)
            {
                using (ISession session = NHibernateHelper.OpenSession())

                    return session.Get<Report>(id);
            }
            public ICollection<Report> GetByCategory(string _Category, object val)
            {
                try
                {
                    using (ISession session = NHibernateHelper.OpenSession())
                    {
                        var _reports = session
                                 .CreateCriteria(typeof(Report))
                                 .Add(Restrictions.Eq(_Category, val))
                                 .List<Report>();
                        return _reports;

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new List<Report>();
                }

            }

            public ICollection<Report> GetAll()
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    var _annotations = session
                             .Query<Report>()
                             .ToList<Report>().ToList();
                    return _annotations;

                }
            }
            public Report GetByName(string name)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    Report _report = session
                         .CreateCriteria(typeof(Report))
                         .Add(Restrictions.Eq("Name", name))
                         .UniqueResult<Report>();
                    return _report;
                }
            }

        }

        public class AnnotationRepository : IAnnotationRepository
        {
            static AnnotationRepository _annotationRepo;

            public static AnnotationRepository GetInstance()
            {
                if (_annotationRepo == null)
                    _annotationRepo = new AnnotationRepository();
                return _annotationRepo;
            }
            public AnnotationRepository()
            {

            }
            public void Add(AnnotationModel _annotation)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.Save(_annotation);
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
            public ICollection<AnnotationModel> GetAll()
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    var _annotations = session
                             .Query<AnnotationModel>()
                             .ToList<AnnotationModel>().ToList();
                    return _annotations;

                }
            }
            public void Update(AnnotationModel _annotation)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(_annotation);
                    transaction.Commit();
                }
            }
            public void Remove(AnnotationModel _annotation)
            {
                //using (ISession session = NHibernateHelper.OpenSession())
                //using (ITransaction transaction = session.BeginTransaction())
                {
                    //session.Delete(_annotation);
                    //transaction.Commit();
                    Update(_annotation);
                }
            }

            public AnnotationModel GetById(int id)
            {
                using (ISession session = NHibernateHelper.OpenSession())

                    return session.Get<AnnotationModel>(id);
            }
            public ICollection<AnnotationModel> GetByCategory(string _Category, object val)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    var _annotations = session
                             .CreateCriteria(typeof(AnnotationModel))
                              .Add(Restrictions.Eq(_Category, val))
                             .List<AnnotationModel>().Where(x => x.HideShowRow == false).ToList();
                    return _annotations;

                }
            }
            public AnnotationModel GetByName(string name)
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    AnnotationModel _annotation = session
                         .CreateCriteria(typeof(AnnotationModel))
                         .Add(Restrictions.Eq("Name", name))
                         .UniqueResult<AnnotationModel>();
                    return _annotation;
                }
            }

        }
        public static class DataVariables
        {
            public static ImageRepository _imageRepo;
            public static VisitRepository _visitViewRepo;
            public static PatientRepository _patientRepo;
            public static AnnotationRepository _annotationRepo;
            public static ReporttRepository _reportRepo;
            private static List<INTUSOFT.Data.Model.Patient> _patients;

            public static List<INTUSOFT.Data.Model.Patient> Patients
            {
                get { return _patients; }
                set { _patients = value; }
            }
            private static List<ImageModel> _images;

            public static List<ImageModel> Images
            {
                get { return _images; }
                set { _images = value; }
            }
            private static List<VisitModel> _visits;

            public static List<VisitModel> Visits
            {
                get { return _visits; }
                set { _visits = value; }
            }
            private static List<Report> _reports;

            public static List<Report> Reports
            {
                get { return _reports; }
                set { _reports = value; }
            }
            private static List<AnnotationModel> _annotations;

            public static List<AnnotationModel> Annotations
            {
                get { return _annotations; }
                set { _annotations = value; }
            }


            private static INTUSOFT.Data.Model.Patient _Active_Patient;

            public static INTUSOFT.Data.Model.Patient Active_Patient
            {
                get { return _Active_Patient; }
                set
                {
                    _Active_Patient = value;
                    Visits = _visitViewRepo.GetByCategory("PatientID", _Active_Patient.ID).Where(x => x.HideShowRow == false).OrderByDescending(x => x.VisitDateTime < DateTime.Now).ToList();
                    Visits.Reverse();

                }
            }
            private static ImageModel _image;

            public static ImageModel Active_Image
            {
                get { return _image; }
                set
                {
                    _image = value;
                    Annotations = _annotationRepo.GetByCategory("ImageID", Active_Image.ID).Where(x => x.HideShowRow == false).ToList();
                }
            }
            private static VisitModel _visit;

            public static VisitModel Active_Visit
            {
                get { return _visit; }
                set
                {
                    _visit = value;
                    Images = _imageRepo.GetByCategory("VisitID", Active_Visit.ID).Where(x => x.HideShowRow == false).ToList();
                    Reports = _reportRepo.GetByCategory("VisitID", Active_Visit.ID).Where(x => x.HideShowRow == 0).ToList();
                }
            }

            private static Report _report;

            public static Report Active_Report
            {
                get { return _report; }
                set { _report = value; }
            }

            private static AnnotationModel _annotation;

            public static AnnotationModel Active_Annotation
            {
                get { return _annotation; }
                set { _annotation = value; }
            }

        }
        public static class IVLDataMethods
        {
            public static void UpdatePatient()
            {
                DataVariables._patientRepo.Update(DataVariables.Active_Patient);
                DataVariables.Visits = DataVariables._visitViewRepo.GetByCategory("PatientID", DataVariables.Active_Patient.ID).Where(x => x.HideShowRow == false).ToList();

                DataVariables.Patients[DataVariables.Patients.FindIndex(x => x.ID == DataVariables.Active_Patient.ID)] = DataVariables.Active_Patient;

                DataVariables.Patients = DataVariables.Patients.Where(x => x.HideShowRow == false).ToList();//.OrderByDescending(x => x.RegistrationDateTime < DateTime.Now).ToList();
                DataVariables.Patients.Sort((x, y) => x.RegistrationDateTime.CompareTo(y.RegistrationDateTime));
                DataVariables.Patients.Reverse();
                //DataVariables.Patients = DataVariables.Patients.Where(x => x.HideShowRow == false).OrderByDescending(x => x.RegistrationDateTime < DateTime.Now).ToList();
            }
            public static void AddPatient(INTUSOFT.Data.Model.Patient patient)
            {

                DataVariables._patientRepo.Add(patient);
                DataVariables.Active_Patient = patient;
                DataVariables.Patients.Add(DataVariables.Active_Patient);
                DataVariables.Patients = DataVariables.Patients.Where(x => x.HideShowRow == false).ToList();//.OrderByDescending(x => x.RegistrationDateTime < DateTime.Now).ToList();
                DataVariables.Patients.Sort((x, y) => x.RegistrationDateTime.CompareTo(y.RegistrationDateTime));
                DataVariables.Patients.Reverse();

            }
            public static void RemovePatient()
            {
                UpdatePatient();
            }

            public static void UpdateVisit()
            {

                DataVariables._visitViewRepo.Update(DataVariables.Active_Visit);
                int indx = DataVariables.Visits.FindIndex(x => x.ID == DataVariables.Active_Visit.ID);
                DataVariables.Visits[indx] = DataVariables.Active_Visit; //Active_Patient.visits.ToList().Where(x => x.HideShowRow == false).ToList();
                DataVariables.Visits = DataVariables.Visits.Where(x => x.HideShowRow == false).ToList();//.OrderByDescending(x => x.VisitDateTime < DateTime.Now).ToList();
                DataVariables.Visits.Sort((x, y) => x.VisitDateTime.CompareTo(y.VisitDateTime));

                DataVariables.Visits.Reverse();
            }
            public static void AddVisit(VisitModel visit)
            {
                DataVariables._visitViewRepo.Add(visit);
                DataVariables.Visits.Add(visit);
                DataVariables.Visits = DataVariables.Visits.Where(x => x.HideShowRow == false).ToList();//.OrderByDescending(x => x.VisitDateTime < DateTime.Now).ToList();
                DataVariables.Visits.Sort((x, y) => x.VisitDateTime.CompareTo(y.VisitDateTime));

                DataVariables.Visits.Reverse();
            }
            public static void RemoveVisit()
            {
                UpdateVisit();
            }


            public static void UpdateReport()
            {

                DataVariables._reportRepo.Update(DataVariables.Active_Report);
                DataVariables.Reports[DataVariables.Reports.FindIndex(x => x.ID == DataVariables.Active_Report.ID)] = DataVariables.Active_Report;
                DataVariables.Reports = DataVariables.Reports.Where(x => x.HideShowRow == 0).ToList(); //.OrderByDescending(x=>x.ReportDateTime< DateTime.Now) .ToList();
                DataVariables.Reports.Sort((x, y) => x.ReportDateTime.CompareTo(y.ReportDateTime));

                DataVariables.Reports.Reverse();

            }
            public static void AddReport(Report r)
            {
                DataVariables._reportRepo.Add(r);
                DataVariables.Active_Report = r;
                DataVariables.Reports.Add(DataVariables.Active_Report);// = DataVariables.Active_Visit.Reports.Where(x => x.HideShowRow == 0).ToList();
                DataVariables.Reports = DataVariables.Reports.Where(x => x.HideShowRow == 0).ToList(); //.OrderByDescending(x=>x.ReportDateTime< DateTime.Now) .ToList();

                DataVariables.Reports.Sort((x, y) => x.ReportDateTime.CompareTo(y.ReportDateTime));

                DataVariables.Reports.Reverse();
            }
            public static void RemoveReport()
            {
                UpdateReport();
            }
            public static void UpdateImage()
            {
                DataVariables._imageRepo.Update(DataVariables.Active_Image);
                DataVariables.Images[DataVariables.Images.FindIndex(x => x.ID == DataVariables.Active_Image.ID)] = DataVariables.Active_Image;
                DataVariables.Images = DataVariables.Images.Where(x => x.HideShowRow == false).ToList();//.OrderByDescending(x=>x.ImageDateTime < DateTime.Now) .ToList();
                DataVariables.Images.Sort((x, y) => x.ImageDateTime.CompareTo(y.ImageDateTime));

                //DataVariables.Images.Reverse();
            }
            public static void AddImage(ImageModel i)
            {
                DataVariables._imageRepo.Add(i);
                DataVariables.Active_Image = i;
                DataVariables.Images.Add(DataVariables.Active_Image);// = DataVariables.Active_Visit.Reports.Where(x => x.HideShowRow == 0).ToList();
                DataVariables.Images = DataVariables.Images.Where(x => x.HideShowRow == false).ToList();//.OrderByDescending(x => x.ImageDateTime < DateTime.Now).ToList();
                DataVariables.Images.Sort((x, y) => x.ImageDateTime.CompareTo(y.ImageDateTime));

                DataVariables.Images.Reverse();
            }
            public static void RemoveImage()
            {
                UpdateImage();
            }
            public static void UpdateAnnotation()
            {
                DataVariables._annotationRepo.Update(DataVariables.Active_Annotation);
                DataVariables.Annotations[DataVariables.Annotations.FindIndex(x => x.ID == DataVariables.Active_Annotation.ID)] = DataVariables.Active_Annotation;
                DataVariables.Annotations = DataVariables.Annotations.Where(x => x.HideShowRow == false).ToList();//.OrderByDescending(x=>x.Date_Time < DateTime.Now) .ToList();
                DataVariables.Annotations.Sort((x, y) => x.Date_Time.CompareTo(y.Date_Time));

                DataVariables.Annotations.Reverse();
            }
            public static void AddAnnotation(AnnotationModel a)
            {
                DataVariables._annotationRepo.Add(a);
                DataVariables.Active_Annotation = a;
                DataVariables.Annotations.Add(DataVariables.Active_Annotation);
                DataVariables.Annotations = DataVariables.Annotations.Where(x => x.HideShowRow == false).ToList();//.OrderByDescending(x => x.Date_Time < DateTime.Now).ToList();
                DataVariables.Annotations.Sort((x, y) => x.Date_Time.CompareTo(y.Date_Time));

                DataVariables.Annotations.Reverse();

            }
            public static void RemoveAnnotation()
            {
                UpdateAnnotation();

            }
        }
    }
}
