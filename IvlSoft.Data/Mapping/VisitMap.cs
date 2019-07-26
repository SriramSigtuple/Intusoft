using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using INTUSOFT.Data.Model;
namespace INTUSOFT.Data.Mapping
{
  public  class VisitMap:ClassMap<VisitModel>
    {
      public VisitMap()
      {
          Id(x => x.ID);
          Map(x => x.BloodSugar);
          Map(x => x.Comments);
          Map(x => x.DBp);
          Map(x => x.DeviceOperator);
          Map(x => x.HideShowRow);
          Map(x => x.LAxis);
          Map(x => x.LCyl);
          Map(x => x.LSph);
          Map(x => x.NoOfImages);
          Map(x => x.NoOfReports);
          Map(x => x.PatientID);
          Map(x => x.RAxis);
          Map(x => x.RCyl);
          Map(x => x.ReferredBy);
          Map(x => x.ReferredTo);
          Map(x => x.RSph);
          Map(x => x.SBp);
          Map(x => x.Time);
          Map(x => x.VisitDateTime);
          Map(x => x.VisitModifyDateTime);
          Map(x => x.VisitTouchDateTime);
          //References(x => x.patient);

          //References(x => x.patient.ID).Column("PatientID").LazyLoad();
          //HasMany(x => x.Images).KeyColumn("VisitID").Cascade.All().Inverse();
         // HasMany(x => x.Images).KeyColumn("VisitID").Cascade.All().Inverse();
          //HasMany(x => x.Reports).KeyColumn("VisitID").Cascade.All().Inverse();
         // HasMany(x => x.Reports).KeyColumn("VisitID").Cascade.All().Inverse();
          Table("Visit");
      }
    }
}
