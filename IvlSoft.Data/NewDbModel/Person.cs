using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
    public class Person : IBaseModel
    {
        #region Creation

        public static Person CreateNewPerson()
        {
            return new Person();
        }

        public Person()
        {
            #region this is done in order to handle null datetime appearing when there is no datetime value sriram 16th may 2016
            this.voidedDate = DateTime.Now;
            this.createdDate = DateTime.Now;
            this.lastModifiedDate = DateTime.Now;
            this.lastAccessedDate = DateTime.Now;
            #endregion
        }

        public static Person CreatePerson(
            int id,
            string middelName,
            string firstName,
            string lastName,
             string phoneno,
            string emilId,
            DateTime dob,
            DateTime modified_datetime,
            DateTime touched_dateTime,
            DateTime dateVoided,
            users voidedBy,
            bool birthdateEstimated,
            bool isPatient,
            ISet<person_attribute> attribute,
            ISet<PersonAddressModel> address,
            users Creator,
            users accessedBy,
            string voidReason,
            users changedBy,
            byte[] profileimagename,
            string uuid,
            char Gender,
            bool hideShowRow,
            DateTime createdTime
            )
        {
            return new Person
            {
                personId = id,
                firstName = firstName,
                middleName = middelName,
                lastName = lastName,
                profileImageName = profileimagename,
                primaryPhoneNumber=phoneno,
                primaryEmailId=emilId,
                gender = Gender,
                birthdate = dob,
                birthdateEstimated = birthdateEstimated,
                createdBy = Creator,
                lastModifiedBy = changedBy,
                voidedDate = dateVoided,
                lastAccessedBy = accessedBy,
                attributes = attribute,
                addresses = address,
                lastAccessedDate = touched_dateTime,
                lastModifiedDate = modified_datetime,
                voidedBy = voidedBy,
                voidedReason = voidReason,
                voided = hideShowRow,
                createdDate = createdTime,
                //uuid=uuid,
            };
        }

        #endregion // Creation
        
        #region State Properties

        public virtual int personId { get; set; }

        public virtual string firstName { get; set; }

        public virtual string middleName { get; set; }

        public virtual byte[] profileImageName { get; set; }

        public virtual string primaryPhoneNumber { get; set; }

        public virtual string primaryEmailId { get; set; }

        public virtual ISet<person_attribute> attributes { get; set; }

        public virtual ISet<PersonAddressModel> addresses { get; set; }

        //public virtual string uuid { get; set; }

        public virtual users createdBy { get; set; }

        public virtual users lastModifiedBy { get; set; }

        public virtual users voidedBy { get; set; }

        public virtual char gender { get; set; }

        public virtual string lastName { get; set; }

        public virtual DateTime birthdate { get; set; }

        public virtual string voidedReason { get; set; }

        public virtual DateTime createdDate { get; set; }

        public virtual DateTime lastModifiedDate { get; set; }

        public virtual DateTime lastAccessedDate { get; set; }

        public virtual DateTime voidedDate { get; set; }

        public virtual string PatientPhoto { get; set; }

        public virtual users lastAccessedBy { get; set; }

        public virtual bool voided { get; set; }

        public virtual bool birthdateEstimated { get; set; }

        #endregion // State Properties
    }
}
