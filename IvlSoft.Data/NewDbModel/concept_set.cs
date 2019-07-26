using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
   public class concept_set
    {
        public static concept_set CreateNewConcept()
        {
            return new concept_set();
        }

        public static concept_set CreateConcept(
            int conceptSetId,
            int conceptSet,
            int conceptId,
            double sortWeight,
            string UUID
            )
        {

            return new concept_set
            {
               conceptId=conceptId,
               conceptSet = conceptSet,
               conceptSetId = conceptSetId,
               sortWeight = sortWeight,
               uuid=UUID
            };
        }
        #region State Properties

        public virtual int conceptSetId { get; set; }

        public virtual int conceptSet { get; set; }

        public virtual int conceptId { get; set; }

        public virtual double sortWeight { get; set; }

        public virtual string uuid { get; set; }

        #endregion
    }
}
