using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
  public class concept_class
    {
       public static concept_class CreateNewConceptClass()
        {
            return new concept_class();
        }

       public static concept_class CreateConceptClass(
            int classId,
            string Name,
            string Description,
            string UUID
            )
        {
            return new concept_class
            {
             conceptClassId = classId,
             name=Name,
             description=Description,
             uuid=UUID
            };

        }
        #region State Properties

        //public virtual DateTime Touched_Date { get; set; }
        //public virtual DateTime Modified_Date { get; set; }

       public virtual int conceptClassId { get; set; }
        /// <summary>
        /// Gets/sets the e-mail address for the Patient.
        /// </summary>
        //public virtual string MRN { get; set; }
        /// <summary>
        /// Gets/sets the Patient's first name.  
        /// </summary>
     public virtual string name { get; set; }

     public virtual string description { get; set; }
      
     public virtual string uuid { get; set; }

        #endregion

    }
}
