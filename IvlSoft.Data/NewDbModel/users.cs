using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
    public class users : IBaseModel
    {
        public static users CreateNewUsers()
        {
            return new users();
        }

        public static users CreateUsers(
            int userId,
            string systemid,
            Person personId,
            string username,
            string Password,
            string secretQuestion,
            string secretAnswer,
            users Creator,
            DateTime dateCreated,
            users changedBy,
            ISet<Role> Roles,
            DateTime dateChanged,
            bool Retired,
            users retiredBy,
            DateTime dateRetired,
            string retireReason,
            string UUID
                 )
        {
            return new users
            {
                userId = userId,
                systemId = systemid,
                username=username,
                person = personId,
                password=Password,
                secret_question=secretQuestion,
                secret_answer=secretAnswer,
                lastModifiedBy = changedBy,
                createdBy = Creator,
                lastModifiedDate = dateChanged,
                createdDate = dateCreated,
                retiredDate = dateRetired,
                retiredReason = retireReason,
                voided=Retired,
                retiredBy = retiredBy,
                roles=Roles,
                uuid=UUID
            };
        }
        #region State Properties

        public virtual string systemId { get; set; }

        public virtual int userId { get; set; }

        public virtual Person person { get; set; }

        public virtual string password { get; set; }

        public virtual string username { get; set; }

        public virtual string secret_question { get; set; }

        public virtual string secret_answer { get; set; }

        public virtual users createdBy { get; set; }

        public virtual users lastModifiedBy { get; set; }

        public virtual users retiredBy { get; set; }

        public virtual bool voided { get; set; }

        public virtual DateTime createdDate { get; set; }

        public virtual DateTime lastModifiedDate { get; set; }

        public virtual DateTime retiredDate { get; set; }

        public virtual string retiredReason { get; set; }

        public virtual ISet<Role> roles { get; set; }

        public virtual string uuid { get; set; }

        #endregion
    }
}
