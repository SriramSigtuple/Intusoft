using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
    public class patient_identifier : IComparable<patient_identifier>, IDisposable, IBaseModel
    {
        public static patient_identifier CreateNewPatientIdentifier()
        {
            return new patient_identifier();
        }
        public patient_identifier()
        {
            #region this is done in order to handle null datetime appearing when there is no datetime value sriram 16th may 2016
            this.voidedDate = DateTime.Now;
            this.createdDate = DateTime.Now;
            this.lastModifiedDate = DateTime.Now;
            #endregion
        }
        public static patient_identifier CreatePatientIdentifier(
            int patientIdentifierId,
            Patient patientId,
            string Identifier,
            patient_identifier_type IdentifierType,
            bool Preffered,
            bool Voided,
            users voidedBy,
            users Creator,
            users changedBy,
            DateTime dateCreated,
            DateTime dateVoided,
            DateTime dateChanged,
            string voidReason,
            string UUID
                 )
        {
            return new patient_identifier
            {
                patientIdentifierId = patientIdentifierId,
                patient = patientId,
                value = Identifier,
                type = IdentifierType,
                preferred = Preffered,
                voided=Voided,
                createdBy = Creator,
                lastModifiedDate = dateChanged,
                createdDate = dateCreated,
                voidedDate = dateVoided,
                voidedReason = voidReason,
                voidedBy = voidedBy,
                lastModifiedBy = changedBy,
                uuid=UUID

            };

        }
        #region State Properties

        public virtual int patientIdentifierId { get; set; }

        public virtual Patient patient { get; set; }

        public virtual string value { get; set; }

        public virtual users voidedBy { get; set; }

        public virtual users createdBy { get; set; }

        public virtual patient_identifier_type type { get; set; }

        public virtual users lastModifiedBy { get; set; }

        public virtual bool voided { get; set; }

        public virtual bool preferred { get; set; }

        public virtual DateTime createdDate { get; set; }

        public virtual DateTime lastModifiedDate { get; set; }

        public virtual DateTime voidedDate { get; set; }

        public virtual string voidedReason { get; set; }

        public virtual string uuid { get; set; }

        #endregion

        public virtual int CompareTo(patient_identifier other)
        {
            int retValue = 0;
            if (other != null)
            {
                retValue = patientIdentifierId.CompareTo(other.patientIdentifierId);

            }
            return retValue;
        }

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
