using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
  public class user_role
    {
        public static user_role CreateNewrUserRole()
        {
            return new user_role();
        }

        public static user_role CreateUserRole(
                 users userId,
               Role Role
                   )
        {
            return new user_role
            {
                user_id = userId,
                role = Role
            };
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var t = obj as user_role;

            if (t == null)
                return false;

            if (this.role == t.role && this.user_id == t.user_id)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return (this.role + "|" + this.user_id).GetHashCode();
        }


        #region State Properties

        public virtual Role role { get; set; }

        public virtual users user_id { get; set; }

        #endregion
    }
}
