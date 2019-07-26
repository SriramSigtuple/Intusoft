using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
   public class privilege
    {
        public static privilege CreateNewPrivilege()
        {
            return new privilege();
        }

        public static privilege CreatePrivilege(

          string Privilage,
         
          string Description,

            string UUid
            )
        {
            return new privilege
            {
               privilegeId = Privilage,
               description=Description,
               uuid=UUid
            };
        }
        #region State Properties

        public virtual string privilegeId { get; set; }

        public virtual string description { get; set; }

        public virtual string uuid { get; set; }

        #endregion
    }
}
