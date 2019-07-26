using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
  public class eye_fundus_image_annotation:IBaseModel, IComparable<eye_fundus_image_annotation>
    {
        public static eye_fundus_image_annotation CreateNewEyeFundusImageAnnotation()
        {
            return new eye_fundus_image_annotation();
        }

        public virtual int CompareTo(eye_fundus_image_annotation other)
        {
            int returnVal = 0;
            if (other != null)
                returnVal = this.eyeFundusImageAnnotationId.CompareTo(other.eyeFundusImageAnnotationId);
            return returnVal;
        }
        public eye_fundus_image_annotation()
        {
            #region this is done in order to handle null datetime appearing when there is no datetime value sriram 16th may 2016
            this.voidedDate = DateTime.Now;
            this.createdDate = DateTime.Now;
            this.lastModifiedDate = DateTime.Now;
            #endregion
        }
        public static eye_fundus_image_annotation CreateEyeFundusImageAnnotation(
            int eyeFundusImageAnnotationId,
            eye_fundus_image eyeFundusImgeId,
            string Comments,
            string xmlData,
            bool isCup,
            bool isAnnotated,
            users Creator,
            DateTime dateCreated,
            DateTime dateChanged,
            users changedBy,
            bool Voided,
            users VoidedBy,
            DateTime dateVoided,
            string voidReason,
            string UUID
            )
        {
            return new eye_fundus_image_annotation
            {
                eyeFundusImageAnnotationId = eyeFundusImageAnnotationId,
                eyeFundusImage = eyeFundusImgeId,
                comments=Comments,
                dataXml = xmlData,
                cdrPresent = isCup,
                createdBy = Creator,
                lastModifiedBy = changedBy,
                createdDate = dateCreated,
                lastModifiedDate = dateChanged,
                voidedDate = dateVoided,
                voided=Voided,
                voidReason = voidReason,
                voidedBy = VoidedBy,
                uuid=UUID
            };
        }

        #region State Properties

        public virtual eye_fundus_image eyeFundusImage { get; set; }

        public virtual int eyeFundusImageAnnotationId { get; set; }

        public virtual users createdBy { get; set; }

        public virtual users lastModifiedBy { get; set; }

        public virtual users voidedBy { get; set; }

        public virtual string comments { get; set; }

        public virtual string dataXml { get; set; }

        public virtual string voidReason { get; set; }

        public virtual string uuid { get; set; }

        public virtual bool cdrPresent { get; set; }

        public virtual bool voided{ get; set; }

        public virtual DateTime createdDate { get; set; }

        public virtual DateTime lastModifiedDate { get; set; }

        public virtual DateTime voidedDate { get; set; }

        #endregion
    }
}
