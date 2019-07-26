using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
  public  class person_attribute_type
    {
        public static person_attribute_type CreateNewPersonAttributeType()
        {
            return new person_attribute_type();
        }

        public static person_attribute_type CreatePersonAttributeType(

          string Name,
            int personAttributeTypeId,
          string Description,
          string UUid
            )
        {

            return new person_attribute_type
            {
               name=Name,
               personAttributeTypeId = personAttributeTypeId
            };

        }
        #region State Properties
      
        public virtual string name { get; set; }

        public virtual int personAttributeTypeId { get; set; }
      
        public virtual string description { get; set; }

        public virtual string uuid { get; set; }

        #endregion
    }
}
