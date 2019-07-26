using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
  public  class person_attribute:IComparable<person_attribute>,IDisposable,IBaseModel
    {

        public static person_attribute CreateNewPersonAttribute()
        {
            return new person_attribute();
        }

        public static person_attribute CreatePersonAttribute(

          person_attribute_type personAttributeTypeId,
            int personAttributeId,
          Person personId,
            string Value,
            string UUid
            )
        {

            return new person_attribute
            {
                personAttributeId = personAttributeId,
                attributeType = personAttributeTypeId,
                person = personId,
                value=Value,
                uuid = UUid
            };
        }

        #region State Properties

        public virtual person_attribute_type attributeType { get; set; }

        public virtual int personAttributeId { get; set; }

        public virtual string value { get; set; }

        public virtual Person person { get; set; }

        public virtual bool voided { get; set; }

        public virtual string uuid { get; set; }

        public virtual DateTime createdDate { get; set; }

        public virtual DateTime lastModifiedDate { get; set; }

        #endregion

        public virtual int CompareTo(person_attribute other)
        {
            int retValue = 0;
            if (other != null)
            {
                retValue = personAttributeId.CompareTo(other.personAttributeId);

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
