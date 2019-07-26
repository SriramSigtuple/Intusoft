using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace INTUSOFT.Data.Enumdetails
{
    public enum RoleEnum//Specifies the list of Roles of a user.
    {
        [Description("Role Not Selected")]
        Role_Not_Selected = 1,
        ADMIN,
        OPERATOR
    }
}
