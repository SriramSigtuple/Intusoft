using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
  public class eye_fundus_image:obs
    {
        public static eye_fundus_image CreateNewEyeFundusImage()
        {
            return new eye_fundus_image();
        }
        public eye_fundus_image()
        {
            #region this is done in order to handle null datetime appearing when there is no datetime value sriram 16th may 2016
            this.lastAccessedDate = DateTime.Now;
            this.eye_fundus_image_annotations = new HashSet<eye_fundus_image_annotation>();
            #endregion
        }
        public static eye_fundus_image CreateEyeFundusImage(
            int eyeFundusImgeId,
            ISet<eye_fundus_image_annotation> eye_fundus_image_annotations,
            char eyeSide,
            bool cdrPresent,
            bool isAnnotated,
            bool dialatedEye,
            string fileName,
            users accessedBy,
            DateTime dateaccessed,
            string cameraSettings,
            string maskSettings,
            machine machineId
            )
        {
            return new eye_fundus_image
            {
             observationId = eyeFundusImgeId,
             eye_fundus_image_annotations = eye_fundus_image_annotations,
             eyeSide = eyeSide,
             annotationsAvailable = isAnnotated,
             cdrAnnotationAvailable = cdrPresent,
             dilatedEye = dialatedEye,
             cameraSetting = cameraSettings,
             lastAccessedBy = accessedBy,
             fileName=fileName,
             lastAccessedDate = dateaccessed,
             maskSetting = maskSettings,
             machine = machineId
            };
        }
        #region State Properties

        //public virtual DateTime Touched_Date { get; set; }
        //public virtual DateTime Modified_Date { get; set; }

        public virtual int eye_fundus_image_id { get; set; }


        public virtual users lastAccessedBy { get; set; }

        public virtual DateTime lastAccessedDate { get; set; }

        public virtual char eyeSide { get; set; }

        public virtual ISet<eye_fundus_image_annotation> eye_fundus_image_annotations { get; set; }


        public virtual bool cdrAnnotationAvailable { get; set; }

        public virtual bool dilatedEye { get; set; }

        public virtual bool annotationsAvailable { get; set; }
        //public virtual string MRN { get; set; }

        public virtual string cameraSetting { get; set; }

        public virtual string maskSetting { get; set; }

        public virtual string fileName { get; set; }

        public virtual machine machine { get; set; }

        #endregion
    }
}
