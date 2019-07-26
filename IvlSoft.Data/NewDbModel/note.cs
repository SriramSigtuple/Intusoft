using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
  public class note
    {

        public static note CreateNewNote()
        {
            return new note();
        }

        public static note CreateNote(
            int visitId,
            int noteId,
            int obsId,
            Patient patientId,
            string Text,
            users Creator,
            users ChangedBy,
            DateTime dateCreated,
            DateTime dateChanged,
            bool Voided,
            users VoidedBy,
            DateTime dateVoided,
            string voidedReason,
            string UUID
                 )
        {
            return new note
            {
                noteId=noteId,
                visitId=visitId,
                patientId=patientId,
                obsId=obsId,
                text=Text,
                creator=Creator,
                changedBy=ChangedBy,
                changedDate=dateChanged,
                createdDate=dateCreated,
                voidedDate=dateVoided,
                voided=Voided,
                voidedBy=VoidedBy,
                voidedReason=voidedReason,
                uuid=UUID

            };

        }
        #region State Properties

        public virtual int visitId { get; set; }

        public virtual int noteId { get; set; }

        public virtual int obsId { get; set; }

        public virtual Patient patientId { get; set; }

        public virtual users creator { get; set; }

        public virtual users changedBy { get; set; }

        public virtual users voidedBy { get; set; }

        public virtual bool voided { get; set; }

        public virtual DateTime createdDate { get; set; }

        public virtual DateTime changedDate { get; set; }

        public virtual DateTime voidedDate { get; set; }

        public virtual string voidedReason { get; set; }

        public virtual string text { get; set; }

        public virtual string uuid { get; set; }

        #endregion

    }
}
