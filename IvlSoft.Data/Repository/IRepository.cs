using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using INTUSOFT.Data.NewDbModel;
using INTUSOFT.Data.Model;


namespace INTUSOFT.Data.Repository
{
    //public interface IDataRepository<T>
    //{
    //    bool Add<T>(T obj);
    //    void Update<T>(T obj);
    //    void Remove<T>(INTUSOFT.Data.Model.Patient _patient);
    //    INTUSOFT.Data.Model.Patient GetById(int id);
    //    INTUSOFT.Data.Model.Patient GetByName(string name);
    //    ICollection<INTUSOFT.Data.Model.Patient> GetByCategory(string category, object val);
    //    ICollection<INTUSOFT.Data.Model.Patient> GetAll();
    //}

    public interface IRepository
    {
        bool Add<T>(T modelValue);//Will add data of type T into the DB.

        bool Update<T>(T modelValue);//Will update data of type T  into the DB.

        void Remove<T>(T modelValue);//Will update voided state into the DB.

        ICollection<T> GetByCategory<T>(string category, object val) where T : class,IBaseModel;//retrives collection of data of type T based on the parameters category and val.

        ICollection<T> GetAll<T>() where T : class,IBaseModel;//retrives collection of data of type T .

        Func<T, bool> GetPredicate<T>(TypeOfPredicate typePredicate) where T : class,IBaseModel;//returns a Func<T,bool> which is evaluated in side the method.
       
    }

    public interface IPatientRepository
    {
        bool Add(INTUSOFT.Data.Model.Patient _patient);
        void Update(INTUSOFT.Data.Model.Patient _patient);
        void Remove(INTUSOFT.Data.Model.Patient _patient);
        INTUSOFT.Data.Model.Patient GetById(int id);
        INTUSOFT.Data.Model.Patient GetByName(string name);
        ICollection<INTUSOFT.Data.Model.Patient> GetByCategory(string category, object val);
        ICollection<INTUSOFT.Data.Model.Patient> GetAll();
    }

    public interface IVisitRepository
    {
        bool Add(VisitModel _visit);
        void Update(VisitModel _visit);
        void Remove(VisitModel _visit);
        VisitModel GetById(int id);
        VisitModel GetByName(string name);
        ICollection<VisitModel> GetByPatID(int PatId);
        ICollection<VisitModel> GetByCategory(string category, object val);
    }

    public interface IImageRepository
    {
        bool Add(ImageModel _image);
        void Update(ImageModel _image);
        void Remove(ImageModel _image);
        ImageModel GetById(int id);
        ImageModel GetByName(string name);
        ICollection<ImageModel> GetByVisitId(int id);
        ICollection<ImageModel> GetByCategory(string category, object val);
    }

    public interface IReportRepository
    {
        void Add(Report _report);
        void Update(Report _report);
        void Remove(Report _report);
        Report GetById(int id);
        Report GetByName(string name);

        ICollection<Report> GetByCategory(string category, object val);
    }

    public interface IAnnotationRepository
    {

        void Add(AnnotationModel _annotation);
        void Update(AnnotationModel _annotation);
        void Remove(AnnotationModel _annotation);
        AnnotationModel GetById(int id);
        AnnotationModel GetByName(string name);
        ICollection<AnnotationModel> GetByCategory(string category, object val);
    }

   //public interface IPersonRepository
   //{
   //    bool Add(Person _patient);
   // }
   //public interface INewPatientRepository
   //{
   //    bool Add(INTUSOFT.Data.NewDbModel.Patient _patient);
   //}
   //public interface INewVistRepository
   //{
   //    bool Add(visit _visit);
   //}

   //public interface IObsRepository
   //{
   //    bool Add(obs _obs);
   //}

   //public interface IPersonAddressRepository
   //{
   //    bool Add(PersonAddressModel _obs);
   // }
   //public interface IPersonAttributeRepository
   //{
   //    bool Add(person_attribute _personAttribute);
   //}

   //public interface IEyeFundusImageRepository
   //{
   //    bool Add(eye_fundus_image _eyeFundus);
   //}

   //public interface IPatientIdentityRepository
   //{
   //    bool Add(patient_identifier _patientIdentifier);
   //}
   //public interface INewReportRepository
   //{
   //    bool Add(report _report);
   //}
   //public interface INewAnnotationRepository
   //{
   //    bool Add(eye_fundus_image_annotation _annotation);
   //}
   //public interface INoteRepository
   //{
   //    bool Add(note _noteIdentifier);
   //}
   //public interface ISyncRecordOutboxRepository
   //{
   //    bool Add(sync_outbox _syncIdentifier);
   //}
   //public interface IGlobalProperties
   //{
   //    bool Add(global_property _globalProperty);
   //}
   //public interface IConcept
   //{
   //    bool Add(Concept _concept);
   //}
   //public interface IUserRepository
   //{
   //    bool Add(users _concept);
   //}
   //public interface IUserRoleRepository
   //{
   //    bool Add(user_role _usersRole);
   //}
   //public interface IRoleRepository
   //{
   //    bool Add(Role _role);
   //}
   //public interface IOrganizationRepository
   //{
   //    bool Add(organization _organization);
   //}
   //public interface IObservationAttributeRepository
   //{
   //    bool Add(ObservationAttribute _observationAttribute);
   //}

}
