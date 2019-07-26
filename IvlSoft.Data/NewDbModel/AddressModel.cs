using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
   public class PersonAddressModel:IComparable<PersonAddressModel>,IDisposable,IBaseModel
    {
        #region Creation

       public static PersonAddressModel CreateNewPersonAddress()
        {

            return new PersonAddressModel();
        }

       public PersonAddressModel()
       {
           #region this is done in order to handle null datetime appearing when there is no datetime value sriram 16th may 2016
           this.voidedDate = DateTime.Now;
           this.createdDate = DateTime.Now;
           this.date_accessed = DateTime.Now;
           this.lastModifiedDate = DateTime.Now;
           #endregion
       }

       public static PersonAddressModel CreatePersonAddress(
            Person id,
            int addressId,
            bool Preffered,
            string Address1,
            string Address2,
            string cityVillage,
            string stateProvince,
            string countyDistrict,
            string Country,
            string postalCode,
            string landCode,
            DateTime modified_datetime,
            DateTime touched_dateTime,
            DateTime dateVoided,
            users voidedBy,
            users Creator,
            int accessedBy,
            string voidReason,
            users changedBy,
            string UUId,
            bool hideShowRow,
            DateTime createdTime
            )
        {
            return new PersonAddressModel
            {
                person = id,
                personAddressId = addressId,
                //MRN = mrn,
                preffered=Preffered,
                line1 = Address1,
                Land_Code=landCode,
                line2 = Address2,
                cityVillage = cityVillage,
                stateProvince = stateProvince,
                countyDistrict = countyDistrict,
                country=Country,
                postalCode = postalCode,
                createdBy = Creator,
                createdDate = createdTime,
                lastModifiedBy = changedBy,
                voidedDate = dateVoided,
                lastModifiedDate = modified_datetime,
                voidedBy = voidedBy,
                void_reason = voidReason,
                voided = hideShowRow,
                uuid = UUId,
            };
        }


        #endregion // Creation

        #region State Properties

        //public virtual DateTime Touched_Date { get; set; }
        //public virtual DateTime Modified_Date { get; set; }

       public virtual int personAddressId { get; set; }

       public virtual Person person { get; set; }

       public virtual string line1 { get; set; }

       public virtual string line2 { get; set; }

        public virtual string uuid { get; set; }

        public virtual users createdBy { get; set; }

        public virtual users lastModifiedBy { get; set; }


        public virtual users voidedBy { get; set; }


        
        public virtual string void_reason { get; set; }



        public virtual DateTime createdDate { get; set; }


        public virtual DateTime lastModifiedDate { get; set; }
        public virtual DateTime date_accessed { get; set; }

        public virtual DateTime voidedDate { get; set; }



        public virtual string cityVillage { get; set; }

        public virtual string stateProvince { get; set; }

        public virtual string countyDistrict { get; set; }

        public virtual string country { get; set; }

        public virtual string postalCode { get; set; }

      
  

   

     


        public virtual bool voided { get; set; }


        public virtual bool preffered { get; set; }


      

        public virtual string Land_Code { get; set; }



      

        #endregion // State Properties


        public virtual int CompareTo(PersonAddressModel other)
        {
            int retValue = 0;
            if (other != null)
            {
                retValue = personAddressId.CompareTo(other.personAddressId);
                
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
