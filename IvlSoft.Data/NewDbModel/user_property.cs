using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
   public class user_property
    {
        public static user_property CreateNewrUserProperty()
        {
            return new user_property();
        }

        public static user_property CreateUserProperty(
            int userId,
          string Property,
            string PropertyValue

              )
        {
            return new user_property
            {
                user_id = userId,
                property=Property,
                property_value=PropertyValue
                
            };

        }
        #region State Properties

        public virtual string property { get; set; }

        public virtual int user_id { get; set; }

        public virtual string property_value { get; set; }

        #endregion
    }
}
