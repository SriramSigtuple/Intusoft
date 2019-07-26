using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
namespace INTUSOFT.Data.Model
{
    /// <summary>
    /// Represents a Patient of a company.  This class
    /// has built-in validation logic. It is wrapped
    /// by the PatientViewModel class, which enables it to
    /// be easily displayed and edited by a WPF user interface.
    /// </summary>
    public class Patient
    {
        #region Creation

        public static Patient CreateNewPatient()
        {
            
            return new Patient();
        }

        public static Patient CreatePatient(
            int id,
            string mrn,
            string firstName,
            string lastName,
            DateTime dob,
            DateTime modified_datetime,
           DateTime touched_dateTime,
           string username,
            String gender,
            string address,
            string city,
            string occupation,
            string income,
            string state,
            string country,
            decimal pincode,
            decimal landline,
            Decimal mobile,
            string email,
            Decimal weight,
            decimal height,
            string bloodgroup,
            string referredBy,
            string comments,
            string historyOfailments,
            bool hideShowRow,
            string patientPhoto,
            int noOfVisits,
            string land_code,
            DateTime regTime,
            AddressModel address_local,
            IList<VisitModel> _visits
            )
        {
            return new Patient
            {
                ID = id,
                MRN = mrn,
                FirstName = firstName,
                LastName = lastName,
                Gender = gender,
                Email = email,
                DOB = dob,
                Address = address,
                City = city,
                State = state,
                Country = country,
                Occupation = occupation,
                Income = income,
                UserName = username,
                Modified_DateTime = modified_datetime,
                Touched_DateTime = touched_dateTime,
                Pincode = pincode,
                Landline = landline,
                Mobile = mobile,
                BloodGroup = bloodgroup,
                ReferredBy = referredBy,
                Comments = comments,
                HistoryAilments = historyOfailments,
                HideShowRow = hideShowRow,
                PatientPhoto = patientPhoto,
                NoOfVisits = noOfVisits,
                Weight = weight,
                Height = height,
                RegistrationDateTime = regTime,
                Land_Code=land_code,
                Address_Local = address_local,
                visits = _visits
            };
        }

       
        #endregion // Creation

        #region State Properties

        //public virtual DateTime Touched_Date { get; set; }
        //public virtual DateTime Modified_Date { get; set; }
       
        public virtual int ID { get; set; }
        /// <summary>
        /// Gets/sets the e-mail address for the Patient.
        /// </summary>
           public virtual string MRN { get; set; }
        /// <summary>
        /// Gets/sets the Patient's first name.  
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// Gets/sets whether the Patient's Gender to either male or female 
        /// </summary>
        public virtual string Gender { get; set; }

        /// <summary>
        /// Gets/sets the Patient's last name.  
        /// </summary>
        public virtual string LastName { get; set; }
        
        /// <summary>
        /// Gets/sets the patient's age.
        /// </summary>
        public virtual DateTime DOB { get; set; }

        public virtual string Occupation { get; set; }
        public virtual string Income { get; set; }
        /// <summary>
        /// Returns the total amount of money spent by the Patient.
        /// </summary>
        public virtual string Email { get; set; }
        /// <summary>
        /// Gets/sets the MRN for the Patient.
        /// </summary>
      

        public virtual DateTime RegistrationDateTime { get; set; }

        public virtual string UserName { get; set; }
        public virtual DateTime Modified_DateTime { get; set; }
        public virtual DateTime Touched_DateTime { get; set; }


        public virtual string Address { get; set; }

        public virtual string City { get; set; }

        public virtual string State { get; set; }

        public virtual string Country { get; set; }

        public virtual Decimal Pincode { get; set; }

        public virtual decimal Landline { get; set; }

        public virtual decimal Mobile { get; set; }

        public virtual string BloodGroup { get; set; }

        public virtual string ReferredBy { get; set; }

        public virtual Decimal Weight { get; set; }

        public virtual decimal Height { get; set; }

        public virtual string Comments { get; set; }

        public virtual string PatientPhoto { get; set; }

        public virtual string HistoryAilments { get; set; }

        public virtual int NoOfVisits { get; set; }

        public virtual bool HideShowRow { get; set; }
       
        public virtual string Land_Code { get; set; }

        public virtual IList<VisitModel> visits { get; set; }
             
        public virtual AddressModel Address_Local{ get; set; }

        #endregion // State Properties

    }
}