using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using INTUSOFT.Data.Model;
namespace INTUSOFT.Data.Mapping
{
  public  class ReportMap:ClassMap<Report>
    {
      public ReportMap()
      {
          Id(x => x.ID);
          Map(x => x.DeviceOperator);
          Map(x => x.Comments);
          Map(x => x.Email);
          Map(x => x.HideShowRow);
          Map(x => x.NoOfImages);
          Map(x => x.ReferredBy);
          Map(x => x.ReferredTo);
          Map(x => x.ReportBy);
          Map(x => x.ReportDateTime);
          Map(x => x.ReportModifyDateTime);
          Map(x => x.ReportTouchedDateTime);
          Map(x => x.ReportXML);
          Map(x => x.VisitID);
         // References(x => x.visit);
          //References(x => x.visit.ID).Column("VisitID").LazyLoad();
          Table("Report");
      }
    }
}
