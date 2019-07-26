using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
    public class Patient : Person, IDisposable, IBaseModel
    {
        #region Creation

       public static Patient CreateNewPatient()
        {
            return new Patient();
        }
       public Patient()
       {
           #region this is done in order to handle null datetime appearing when there is no datetime value sriram 16th may 2016
           this.patientVoidedDate = DateTime.Now;
           this.patientCreatedDate = DateTime.Now;
           this.date_accessed = DateTime.Now;
           this.patientLastModifiedDate = DateTime.Now;
           this.diagnosis = new HashSet<PatientDiagnosis>();
           this.observations = new HashSet<eye_fundus_image>();
           #endregion
        }
       //public virtual Patient As<Patient>() where Patient : class
       //{
       //    return this as Patient;
       //}
       public static Patient CreatePatient(
            int id,
            string historyAlignments,
            Person refferedBy,
            DateTime modified_datetime,
            DateTime dateVoided,
            DateTime dateSent,
            users voidedBy,
            ISet<patient_identifier> identifier,
            ISet<PatientDiagnosis> diagnosis,
            ISet<visit> visit,
            ISet<eye_fundus_image> observation,
            users Creator,
            string voidReason, 
            int personID,
            users changedBy,
            users sentBy,
            bool hideShowRow,
            DateTime createdTime
            )
        {
            return new Patient
            {
                personId = id,
                historyAilments = historyAlignments,
                referredBy = refferedBy,
                patientCreatedBy = Creator,
                patientCreatedDate = createdTime,
                patientLastModifiedBy = changedBy,
                patientVoided = hideShowRow,
                patientLastSentDate=dateSent,
                patientLastSentBy=sentBy,
                patientLastModifiedDate = modified_datetime,
                patientVoidedBy = voidedBy,
                patientVoidedDate = dateVoided,
                identifiers = identifier,
                diagnosis = diagnosis,
                visits = visit,
                observations = observation,
                patientVoidedReason = voidReason
            };
        }
       public static Patient CreatePatient(
        Patient proxyPat
         )
       {
           return new Patient
           {
               personId = proxyPat.personId,
               historyAilments = proxyPat.historyAilments,
               referredBy = proxyPat.referredBy,
               patientCreatedBy = proxyPat.patientCreatedBy,
               patientCreatedDate = proxyPat.patientCreatedDate,
               patientLastModifiedBy = proxyPat.patientLastModifiedBy,
               patientVoided = proxyPat.patientVoided,
               patientLastSentDate = proxyPat.patientLastSentDate,
               patientLastSentBy = proxyPat.patientLastSentBy,
               patientLastModifiedDate = proxyPat.patientLastModifiedDate,
               patientVoidedBy = proxyPat.patientVoidedBy,
               patientVoidedDate = proxyPat.patientVoidedDate,
               identifiers = proxyPat.identifiers,
               diagnosis = proxyPat.diagnosis,
               visits = proxyPat.visits,
               observations = proxyPat.observations,
               patientVoidedReason = proxyPat.patientVoidedReason
           };
       }

        #endregion // Creation

        #region State Properties

        //public virtual string uuid { get; set; }

       public virtual users patientCreatedBy { get; set; }

       public virtual users patientLastModifiedBy { get; set; }

       public virtual users patientLastSentBy { get; set; }

       public virtual users patientVoidedBy { get; set; }

       public virtual string patientVoidedReason { get; set; }

        public virtual DateTime patientCreatedDate { get; set; }

        public virtual ISet<patient_identifier> identifiers { get; set; }

        public virtual ISet<PatientDiagnosis> diagnosis { get; set; }

        public virtual ISet<visit> visits { get; set; }

        public virtual ISet<eye_fundus_image> observations { get; set; }
        
        public virtual DateTime patientLastModifiedDate { get; set; }

        public virtual DateTime patientLastSentDate { get; set; }
        
        public virtual DateTime date_accessed { get; set; }

        public virtual DateTime patientVoidedDate { get; set; }

        public virtual Person referredBy { get; set; }

        public virtual string historyAilments { get; set; }

        public virtual bool patientVoided { get; set; }

        #endregion // State Properties

        bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing == true)
            {
                //someone want the deterministic release of all resources
                //Let us release all the managed resources
            }
            else
            {
                // Do nothing, no one asked a dispose, the object went out of
                // scope and finalized is called so lets next round of GC 
                // release these resources
            }

            // Release the unmanaged resource in any case as they will not be 
            // released by GC
            disposed = true;
        }   

    }
}
