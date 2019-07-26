using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using INTUSOFT.Data.Model;
namespace INTUSOFT.Data.Mapping
{
  public  class ImageMap:ClassMap<ImageModel>
    {
      public ImageMap()
      {
          Id(x => x.ID);
          Map(x => x.CameraSettings);
          Map(x => x.Comments);
          Map(x => x.EyeSide);
          Map(x => x.EyeType);
          Map(x => x.HideShowRow);
          Map(x => x.ImageDateTime);
          Map(x => x.ImageModifyDateTime);
          Map(x => x.ImageName);
          Map(x => x.ImageTouchDateTime);
          Map(x => x.IsAnnotated).Column("isAnnotated");
          Map(x => x.IsCDR).Column("isCDR");
          Map(x => x.isDilation);
          Map(x => x.LocalURL);
          Map(x => x.ServerURL);
          Map(x => x.VisitID);
          //References(x => x.visit);
          //References(x => x.visit.ID).Column("VisitID").LazyLoad();
          //HasMany(x => x.Annotations).KeyColumn("ImageID").Cascade.All().Inverse();
          //HasMany(x => x.Annotations).KeyColumn("ImageID").Cascade.All().Inverse();
          Table("Image");
      }
    }
}
