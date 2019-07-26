using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
  public  class role_role
    {
        public static role_role CreateNewrRoleRole()
        {
            return new role_role();
        }

        public static role_role CreateRoleRole(

          Role childRole,
          Role parentRole
              )
        {
             return new role_role
            {
               child_role=childRole,
               parent_role=parentRole
              };

        }
        #region State Properties

        public virtual Role child_role { get; set; }

        public virtual Role parent_role { get; set; }

           #endregion
    }
}
