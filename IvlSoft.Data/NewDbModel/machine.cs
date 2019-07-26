using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
  public class machine
    {
        public static machine CreateNewMachine()
        {
            return new machine();
        }

        public static machine CreateMachine(
           
          string Name,
            int machineId,
          string Description,
          string serialNumber,
          string modelNumber,
            string UUid
            )
        {

            return new machine
            {
              name=Name,
              machineId = machineId,
              description=Description,
              serialNumber = serialNumber,
              modelNumber = modelNumber,
              uuid=UUid
            };

        }
        #region State Properties

        public virtual string name { get; set; }

        public virtual string description { get; set; }

        public virtual string serialNumber { get; set; }

        public virtual string modelNumber { get; set; }

        public virtual string uuid { get; set; }

        public virtual int machineId { get; set; }

        #endregion
    }
}
