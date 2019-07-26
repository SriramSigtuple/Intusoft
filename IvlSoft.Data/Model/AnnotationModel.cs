using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTUSOFT.Data.Model
{
   public class AnnotationModel
    {

       public static AnnotationModel CreateNewAnnotationModel()
       {
           return new AnnotationModel();
       }

       public static AnnotationModel CreateAnnotationModel
           (int id,
          DateTime date_time,
           string comments,
           int imageId,
           string annotationxml,
           bool hideShowRow,
           bool iscup,
           ImageModel _image)
       {
           return new AnnotationModel
           {
               ID = id,
               Date_Time=date_time,
               Comments = comments ,
               HideShowRow = hideShowRow,
               AnnotationXml=annotationxml,
               ImageID = imageId,
               isCup = iscup,
               image = _image
           };
       }


       public virtual string AnnotationXml { get; set; }
       public virtual DateTime Date_Time { get; set; }
       public virtual int ID { get; set; }

       public virtual int TypeOfAnnotation { get; set; }

       public virtual int CentreX { get; set; }

       public virtual int CentreY { get; set; }

       public virtual int Width { get; set; }

       public virtual int Height { get; set; }

       public virtual string Comments { get; set; }

       public virtual string AnnotationBy { get; set; }

       public virtual bool HideShowRow { get; set; }

       public virtual int ImageID { get; set; }

       public virtual bool isCup { get;set;}

       public virtual ImageModel image { get; set; }


    }
}
