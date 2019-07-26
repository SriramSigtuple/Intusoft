using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
  public  class patient_identifier_type
    {
        public static patient_identifier_type CreateNewPatientIdentifierType()
        {
            return new patient_identifier_type();
        }

        public static patient_identifier_type CreatePatientIdentifierType(
            int patientIdentifierTypeId,
          string Name,
            string Description,
            string Format,
            bool Required,
            string formatDescription,
            string Validator,
            bool Retired,
            users RetiredBy,
            users Creator,
            users modifiedBy,
            DateTime dateCreated,
            DateTime dateRetired,
            DateTime dateModified,
            string retiredReason,
            string UUID
                 )
        {
            return new patient_identifier_type
            {

                patientIdentifierTypeId = patientIdentifierTypeId,
                name=Name,
                description=Description,
                format=Format,
                formatDescription = formatDescription,
                validator=Validator,
                retired=Retired,
                required=Required,
                creator=Creator,
                lastModifiedDate=dateModified,
                lastModifiedBy=modifiedBy,
                createdDate = dateCreated,
                retiredDate = dateRetired,
                retiredBy = RetiredBy,
                retiredReason = retiredReason,
                uuid=UUID
            };

        }
        #region State Properties



        public virtual int patientIdentifierTypeId { get; set; }

        public virtual string name { get; set; }

        public virtual string description { get; set; }

        public virtual users retiredBy { get; set; }

        public virtual users creator { get; set; }

        public virtual users lastModifiedBy { get; set; }

        public virtual bool required { get; set; }

        public virtual bool retired { get; set; }

        public virtual DateTime createdDate { get; set; }

        public virtual DateTime retiredDate { get; set; }

        public virtual DateTime lastModifiedDate { get; set; }

        public virtual string retiredReason { get; set; }

        public virtual string formatDescription { get; set; }

        public virtual string validator { get; set; }


        public virtual string format { get; set; }

        public virtual string uuid { get; set; }

        #endregion
    }
}
