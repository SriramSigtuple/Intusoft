using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
   public class role_privilege
    {
        public static role_privilege CreateNewRolePrevilage()
        {
            return new role_privilege();
        }

        public static role_privilege CreateRolePrevilage(
          Role Role,
          privilege Privilege
            )
        {
            return new role_privilege
            {
                role = Role,
                privilege = Privilege,
               
            };
        }
        #region State Properties



        public virtual Role role { get; set; }




        public virtual privilege privilege { get; set; }

        







        #endregion
    }
}
