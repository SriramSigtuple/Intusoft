using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
    public class ObservationAttribute
    {
        public static ObservationAttribute CreateNewObservationAttribute()
        {
            return new ObservationAttribute();
        }

        public static ObservationAttribute CreateObservationAttribute(
           ObservationAttributeType personAttributeTypeId,
             int personAttributeId,
           obs obsId,
             string Value,
             string UUid
             )
        {
            return new ObservationAttribute
            {
                obsAttributeId = personAttributeId,
                attributeType = personAttributeTypeId,
                observation = obsId,
                value = Value,
                uuid = UUid
            };
        }

        #region State Properties

        public virtual ObservationAttributeType attributeType { get; set; }

        public virtual int obsAttributeId { get; set; }

        public virtual string value { get; set; }

        public virtual obs observation { get; set; }

        public virtual string uuid { get; set; }

        #endregion

        public virtual int CompareTo(ObservationAttribute other)
        {
            int retValue = 0;
            if (other != null)
            {
                retValue = obsAttributeId.CompareTo(other.obsAttributeId);
            }
            return retValue;
        }

    }
}
