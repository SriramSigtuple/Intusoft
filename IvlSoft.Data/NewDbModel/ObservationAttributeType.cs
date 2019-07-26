using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
    public class ObservationAttributeType
    {
        public static ObservationAttributeType CreateNewObservationAttributeType()
        {
            return new ObservationAttributeType();
        }

        public static ObservationAttributeType CreateObservationAttributeType(

          string Name,
            int obsAttributeTypeId,
          string Description,
          string UUid
            )
        {
            return new ObservationAttributeType
            {
                name = Name,
                obsAttributeTypeId = obsAttributeTypeId
            };
        }

        #region State Properties

        public virtual string name { get; set; }

        public virtual int obsAttributeTypeId { get; set; }

        public virtual string description { get; set; }

        public virtual string uuid { get; set; }

        #endregion
    }
}
