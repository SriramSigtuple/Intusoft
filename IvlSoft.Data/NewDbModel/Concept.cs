using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
  public  class Concept
    {
        public static Concept CreateNewConcept()
        {
            return new Concept();
        }

        public static Concept CreateConcept(
            int conceptId,
            string shortName,
            string Description,
             bool isSet,
            int datatypeId,
            string fullspecifiedName,
            int classId,
            string UUID
            )
        {

            return new Concept
            {
               conceptId = conceptId,
               shortName = shortName,
               description=Description,
               set = isSet,
               datatype=datatypeId,
               fullySpecifiedName=fullspecifiedName,
               conceptClass=classId,
               uuid=UUID
            };
        }
        #region State Properties

        public virtual int conceptId { get; set; }

        public virtual string shortName { get; set; }

        public virtual string description { get; set; }

        public virtual int datatype { get; set; }

        public virtual int conceptClass { get; set; }
      
        public virtual bool set { get; set; }

        public virtual string fullySpecifiedName { get; set; }

        public virtual string uuid { get; set; }

        #endregion
    }
}
