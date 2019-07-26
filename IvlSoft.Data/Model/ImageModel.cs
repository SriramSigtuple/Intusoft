using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections;

namespace INTUSOFT.Data.Model
{
    public class ImageModel
    {
        public static ImageModel CreateNewImageModel()
        {
            return new ImageModel();
        }

        public static ImageModel CreateImageModel
            (int id,
              string localUrl,
              int eyeSide,
            string serverUrl,
              int type,
            bool isAnnotated,
              bool isDilation,
             string cameraSettings,
            string comments,
            bool hideShowRow,
            int visitId,
            int patID,
            bool isCDr,
            DateTime imageDateTime,
            DateTime imageModifyDateTime,
            DateTime imageTouchDateTime,
            string imageName,
            VisitModel _visit,
            IList<AnnotationModel> annotations
            )
        {
            return new ImageModel
            {
                ID = id,
                ImageModifyDateTime=imageModifyDateTime,
                ImageTouchDateTime=imageTouchDateTime,
                LocalURL = localUrl,
                ServerURL = serverUrl,
                EyeSide = eyeSide,
                EyeType = type,
                isDilation = isDilation,
                IsAnnotated = isAnnotated,
                Comments = comments,
                HideShowRow = hideShowRow,
                VisitID = visitId,
                IsCDR  = isCDr,
                ImageDateTime = imageDateTime,
                ImageName= imageName,
                visit = _visit,
                Annotations = annotations
            };


        }

        public virtual int ID { get; set; }
        public virtual string ImageName { get; set; }

        public virtual string LocalURL { get; set; }
        public virtual string ServerURL { get; set; }

        public virtual int EyeSide { get; set; }

        public virtual int EyeType { get; set; }
        public virtual bool IsAnnotated { get; set; }
        public virtual bool IsCDR { get; set; }

        public virtual bool isDilation { get; set; }

         public virtual string CameraSettings{get;set;}
        public virtual string Comments { get; set; }

        public virtual bool HideShowRow { get; set; }

        public virtual int VisitID { get; set; }

        public virtual DateTime ImageModifyDateTime { get; set; }
        public virtual DateTime ImageTouchDateTime { get; set; }
        public virtual DateTime ImageDateTime { get; set; }

        public virtual IList<AnnotationModel> Annotations { get; set; }

        public virtual VisitModel visit { get; set; }
    }
}
