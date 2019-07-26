using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using INTUSOFT.Data.Model;
namespace INTUSOFT.Data.Mapping
{
  public  class AnnotationMap:ClassMap<AnnotationModel>
    {
      public AnnotationMap()
      {
          Id(x => x.ID);
          Map(x => x.AnnotationBy);
          Map(x => x.AnnotationXml);
          Map(x => x.CentreX);
          Map(x => x.CentreY);
          Map(x => x.Comments);
          Map(x => x.Date_Time);
          Map(x => x.Height);
          Map(x => x.ImageID);
          Map(x => x.isCup);
          Map(x => x.TypeOfAnnotation);
          Map(x => x.Width);
          Map(x => x.HideShowRow);
          //References(x => x.image);
          Table("Annotation");
      }
    }
}
