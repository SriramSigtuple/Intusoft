using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
    public class PatientDiagnosis : IDisposable, IBaseModel, IComparable<PatientDiagnosis>
    {
        public static PatientDiagnosis CreateNewDiagnosis()
        {
            return new PatientDiagnosis();
        }
        public virtual int CompareTo(PatientDiagnosis other)
        {
            int returnVal = 0;
            if (other != null)
                returnVal = this.patientDiagnosisId.CompareTo(other.patientDiagnosisId);
            return returnVal;
        }

        public PatientDiagnosis()
        {
            #region this is done in order to handle null datetime appearing when there is no datetime value sriram 16th may 2016
            this.voidedDate = DateTime.Now;
            this.createdDate = DateTime.Now;
            this.lastModifiedDate = DateTime.Now;
            #endregion
        }

        public static PatientDiagnosis CreatePatientDiagnosis(
            Concept conceptId,
            Patient patientId,
            visit visitId,
            int DiagnosisId,
            string value_left,
            string value_right,
            DateTime createdDate,
            DateTime lastModifiedDate,
            bool Voided,
            DateTime dateVoided
                )
        {
            return new PatientDiagnosis
            {
                patientDiagnosisId = DiagnosisId,
                patient = patientId,
                visit = visitId,
                concept=conceptId,
                createdDate=createdDate,
                lastModifiedDate=lastModifiedDate,
                voidedDate=dateVoided,
                diagnosisValueLeft = value_left,
                diagnosisValueRight = value_right,
                voided=Voided

            };
        }


        #region State Properties

        public virtual int patientDiagnosisId { get; set; }

        public virtual Patient patient { get; set; }

        public virtual visit visit { get; set; }

        public virtual Concept concept { get; set; }

        public virtual DateTime createdDate { get; set; }

        public virtual DateTime lastModifiedDate { get; set; }

        public virtual DateTime voidedDate { get; set; }

        public virtual bool voided { get; set; }

        public virtual string diagnosisValueLeft { get; set; }

        public virtual string diagnosisValueRight { get; set; }

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
