using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
    public class report : IDisposable, IBaseModel, IComparable<report>
    {
        public static report CreateNewReport()
        {
            return new report();
        }
        public virtual int CompareTo(report other)
        {
            int returnVal = 0;
            if (other != null)
                returnVal = this.reportId.CompareTo(other.reportId);
            return returnVal;
        }
        public report()
        {
            #region this is done in order to handle null datetime appearing when there is no datetime value sriram 16th may 2016
            this.voidedDate = DateTime.Now;
            this.createdDate = DateTime.Now;
            this.lastModifiedDate = DateTime.Now;
            #endregion
        }

        public static report CreateReport(
            visit visit,
            int reportId,
            ReportType reportType,
            string jsonData,
            users Creator,
            users ChangedBy,
            Patient Patient, 
            DateTime dateCreated,
            DateTime dateChanged,
            bool Voided,
            users VoidedBy,
            DateTime dateVoided,
            string voidedReason,
            string UUID
                 )
        {
            return new report
            {
             reportId = reportId,
             visit=visit,
             dataJson = jsonData,
             lastModifiedDate = dateChanged,
             createdDate = dateCreated,
             voidedDate = dateVoided,
             createdBy = Creator,
             report_type=reportType,
             lastModifiedBy = ChangedBy,
             Patient=Patient,
             voided=Voided,
             voidedBy = VoidedBy,
             voidedReason = voidedReason,
             uuid=UUID
            };
        }
        #region State Properties

        public virtual visit visit { get; set; }

        public virtual int reportId { get; set; }

        public virtual users createdBy { get; set; }

        public virtual users lastModifiedBy { get; set; }

        public virtual users voidedBy { get; set; }
         
        public virtual bool voided { get; set; }

        public virtual DateTime createdDate { get; set; }

        public virtual DateTime lastModifiedDate { get; set; }

        public virtual DateTime voidedDate { get; set; }

        public virtual ReportType report_type { get; set; }

        public virtual Patient Patient { get; set; }

        public virtual string voidedReason { get; set; }

        public virtual string dataJson { get; set; }

        public virtual string uuid { get; set; }
        
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
