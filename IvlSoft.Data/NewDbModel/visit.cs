using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
    public class visit : IDisposable, IBaseModel,IComparable<visit>
    {
        public static visit CreateNewVisit()
        {
            return new visit();
        }
        public virtual int CompareTo(visit other)
        {
            int returnVal = 0;
            if (other != null)
                returnVal = this.visitId.CompareTo(other.visitId);
            return returnVal;
        }
        public visit()
        {
            #region this is done in order to handle null datetime appearing when there is no datetime value sriram 16th may 2016
            this.voidedDate = DateTime.Now;
            this.createdDate = DateTime.Now;
            this.lastModifiedDate = DateTime.Now;
            this.endDateTime = DateTime.Now;
            this.lastAccessedDate = DateTime.Now;
            this.observations = new HashSet<eye_fundus_image>();
            this.reports = new HashSet<report>();
            #endregion
        }
        public static visit CreateVisit(
            int visitId,
            Patient patientId,
            users Creator,
            users ChangedBy,
            DateTime dateStarted,
            DateTime dateStopped,
            DateTime dateChanged,
            int totalObs,
            int totalReports,
            users lastsentby,
            DateTime lastsentdate,
            bool Voided,
            users VoidedBy,
            DateTime dateAccessed,
            users accessedBy,
            ISet<eye_fundus_image> obs,
            ISet<report> reports,
            DateTime dateVoided,
            string voidedReason,
            string UUID
                 )
        {
            return new visit
            {
             visitId = visitId,
             patient = patientId,
             createdBy = Creator,
             lastModifiedBy = ChangedBy,
             lastAccessedBy = accessedBy,
             lastModifiedDate = dateChanged,
             observations = obs,
             reports = reports,
             createdDate = dateStarted,
             lastSentDate=lastsentdate,
             endDateTime = dateStopped,
             totalObservations=totalObs,
             totalReports=totalReports,
             lastSentBy=lastsentby,
             lastAccessedDate = dateAccessed,
             voidedDate = dateVoided,
             voided=Voided,
             voidedBy = VoidedBy,
             voidedReason = voidedReason,
             //uuid=UUID
            };
        }
        #region State Properties

        public virtual int visitId { get; set; }

        public virtual Patient patient { get; set; }

        public virtual users lastSentBy { get; set; }

        public virtual users createdBy { get; set; }

        public virtual int totalObservations { get; set; }

        public virtual int totalReports { get; set; }

        public virtual users lastModifiedBy { get; set; }

        public virtual ISet<eye_fundus_image> observations { get; set; }

        public virtual ISet<report> reports { get; set; }

        public virtual users voidedBy { get; set; }

        public virtual users lastAccessedBy { get; set; }

        public virtual bool voided { get; set; }

        public virtual DateTime createdDate { get; set; }

        public virtual DateTime lastSentDate { get; set; }

        public virtual DateTime endDateTime { get; set; }

        public virtual DateTime lastModifiedDate { get; set; }

        public virtual DateTime lastAccessedDate { get; set; }

        public virtual DateTime voidedDate { get; set; }

        public virtual string voidedReason { get; set; }

        #endregion

        bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the managed and unmanaged resources.
        /// </summary>
        /// <param name="disposing"></param>
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
