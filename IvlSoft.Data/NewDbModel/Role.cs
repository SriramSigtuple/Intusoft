using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
   public class Role
    {
        public static Role CreateNewRole()
        {
            return new Role();
        }

        public static Role CreateRole(
          string role,
          string Description,
          string UUid
            )
        {
            return new Role
            {
                roleId = role,
                description = Description,
                uuid = UUid
            };
        }
        #region State Properties

        public virtual string roleId { get; set; }

        public virtual string description { get; set; }

        public virtual string uuid { get; set; }

        #endregion
    }
}
