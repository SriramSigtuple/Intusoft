using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
 public   class ReportType
    {
        public static ReportType CreateNewReportType()
        {
            return new ReportType();
        }

        public static ReportType CreateReportType(
            int reportTypeId,
            string name,
            string description,
            string UUID
                 )
        {
            return new ReportType
            {
               reportTypeId = reportTypeId,
               name=name,
               description=description,
               uuid=UUID
            };
        }
        #region State Properties

        public virtual int reportTypeId { get; set; }

        public virtual string name { get; set; }

        public virtual string description { get; set; }

        public virtual string uuid { get; set; }

        #endregion

    }
}
