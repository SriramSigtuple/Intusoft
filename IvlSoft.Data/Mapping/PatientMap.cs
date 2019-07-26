using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using INTUSOFT.Data.Model;
namespace INTUSOFT.Data.Mapping
{
  public  class PatientMap:ClassMap<Patient>
    {
      public PatientMap()
      {
          Id(x => x.ID);
          Map(x => x.FirstName);
          Map(x => x.LastName);
          Map(x => x.DOB);
          Map(x => x.Gender);
          Map(x => x.Mobile);
          Map(x => x.Landline);
          Map(x => x.Land_Code);
          Map(x => x.Modified_DateTime);
          Map(x => x.RegistrationDateTime);
          Map(x => x.Touched_DateTime);
          Map(x => x.Email);
          Map(x => x.HistoryAilments);
          Map(x => x.Comments);
          Map(x => x.BloodGroup);
          Map(x => x.Weight);
          Map(x => x.Height);
          Map(x => x.HideShowRow);
          Map(x => x.Income);
          Map(x => x.MRN);
          Map(x => x.Occupation);
          Map(x => x.PatientPhoto);
          Map(x => x.ReferredBy);
          Map(x => x.UserName);
          Map(x => x.NoOfVisits);
          //Map(x => x.Address);
          //Map(x => x.City);
          //Map(x => x.State);
          //Map(x => x.Country);
          //Map(x => x.Pincode);
          Component<AddressModel>(x => x.Address_Local, m =>
          {
              m.Map(x => x.Address);//.Column("Address");
              m.Map(x => x.City);
              m.Map(x => x.State);
              m.Map(x => x.Country);
              m.Map(x => x.Pincode);
          });
         // HasMany(x => x.visits).Fetch.Join().KeyColumn("PatientID").Cascade.All().Inverse();
          //HasMany(x => x.visits).Fetch.Join().KeyColumn("PatientID").Cascade.All().Inverse();
          Table("Patient");
      }
    }
}
