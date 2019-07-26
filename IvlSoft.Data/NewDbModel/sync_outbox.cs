using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
  public class sync_outbox
    {

        public static sync_outbox CreateNewSyncOutbox()
        {
            return new sync_outbox();
        }

        public static sync_outbox CreateRoleSyncOutbox(

            int outboxRecordId,
            string outboxRecordType,
            int objectId,
            string State,
            string Status,
            string PayLoad,
            int retryCount,
            users Creator,
            DateTime dateCreated,
            int patientOutboxRecordId
            
            )
        {

            return new sync_outbox
            {
                outbox_record_id = outboxRecordId,
                record_type=outboxRecordType,
                object_id=objectId,
                state=State,
                status=Status,
                payload=PayLoad,
                creator=Creator,
                date_created=dateCreated,
                retry_count=retryCount,
                patient_outbox_record_id=patientOutboxRecordId

            };

        }
        #region State Properties

        public virtual int outbox_record_id { get; set; }

        public virtual string record_type { get; set; }

        public virtual int object_id { get; set; }

        public virtual string state { get; set; }

        public virtual DateTime date_created { get; set; }


        public virtual string status { get; set; }


        public virtual string payload { get; set; }


        public virtual users creator { get; set; }

        public virtual int retry_count { get; set; }


        public virtual int patient_outbox_record_id { get; set; }
        
        #endregion
    }
}
