using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
  public class global_property:IBaseModel
    {
        public static global_property CreateNewGlobalProperty()
        {
            return new global_property();
        }
        public static global_property CreateGlobalProperty(
            string Property,
            string PropertyValue,
            string Description
            )
        {
            return new global_property
            {
              key = Property,
              value = PropertyValue,
              description=Description
            };
        }
        #region State Properties

        //public virtual int concept_id { get; set; }

        public virtual string key { get; set; }

        public virtual string value { get; set; }

        public virtual string description { get; set; }

        public virtual bool voided { get; set; }

        public virtual DateTime createdDate { get; set; }

        public virtual DateTime lastModifiedDate { get; set; }

        #endregion
    }
}
