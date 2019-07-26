using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
   public class organization
    {
        public static organization CreateNewOrganization()
        {
            return new organization();
        }

        public static organization CreateOrganization(
           
            string Property,
            string propertyValue
           
            )
        {
            return new organization
            {
             property=Property,
             property_value=propertyValue
            };
        }
        #region State Properties

        public virtual string property { get; set; }

        public virtual string property_value { get; set; }

        #endregion

    }
}
