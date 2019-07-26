using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
    public class obs : IDisposable, IBaseModel,IComparable<obs>
    {
        public static obs CreateNewObs()
        {
            return new obs();
        }
        public virtual int CompareTo(obs other)
        {
            int returnVal = 0;
            if (other != null)
                returnVal = this.observationId.CompareTo(other.observationId);
            return returnVal;
        }
        public obs()
        {
            #region this is done in order to handle null datetime appearing when there is no datetime value sriram 16th may 2016
            this.voidedDate = DateTime.Now;
            this.createdDate = DateTime.Now;
            this.lastModifiedDate = DateTime.Now;
            this.takenDateTime = DateTime.Now;
            #endregion
        }
        public static obs CreateObs(
            Concept conceptId,
            Patient patientId,
            int obsId,
            visit visitId,
            string Value,
            eye_fundus_image eye_fundus_image,
            string Comments,
            users Creator,
            users ChangedBy,
            users sentBy,
            DateTime obsDateTime,
            DateTime dateCreated,
            DateTime sentDate,
            obs obsGroupId,
            DateTime dateChanged,
            bool Voided,
            ISet<obs> groupMembers,
            users VoidedBy,
            DateTime dateVoided,
            string voidedReason,
            string UUID
                 )
        {
            return new obs
            {
                observationId = obsId,
                concept = conceptId,
                comments=Comments,
                patient=patientId,
                visit = visitId,
                value=Value,
                //eye_fundus_image = eye_fundus_image,
                createdBy = Creator,
                lastModifiedBy = ChangedBy,
                lastModifiedDate = dateChanged,
                createdDate = dateCreated,
                voidedDate = dateVoided,
                takenDateTime = obsDateTime,
                groupObservation = obsGroupId,
                lastSentBy=sentBy,
                lastSentDate=sentDate,
                voided=Voided,
                groupMembers=groupMembers,
                voidedBy = VoidedBy,
                voidedReason = voidedReason,
                //uuid=UUID
            };
        }
        #region State Properties

        public virtual visit visit { get; set; }

        public virtual Concept concept { get; set; }

        public virtual int observationId { get; set; }

        public virtual Patient patient { get; set; }

        public virtual obs groupObservation { get; set; }

        //public virtual eye_fundus_image eye_fundus_image { get; set; }

        public virtual users createdBy { get; set; }

        public virtual users lastModifiedBy { get; set; }

        public virtual users lastSentBy { get; set; }

        public virtual users voidedBy { get; set; }

        public virtual ISet<obs> groupMembers { get; set; }

        public virtual bool voided { get; set; }

        public virtual DateTime createdDate { get; set; }

        public virtual DateTime takenDateTime { get; set; }

        public virtual DateTime lastModifiedDate { get; set; }

        public virtual DateTime lastSentDate { get; set; }

        public virtual DateTime voidedDate { get; set; }

        public virtual string voidedReason { get; set; }

        public virtual string value { get; set; }

        public virtual string comments { get; set; }
        

        //public virtual string uuid { get; set; }

        #endregion

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
