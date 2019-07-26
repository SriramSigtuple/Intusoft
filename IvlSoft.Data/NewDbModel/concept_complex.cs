using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
   public class concept_complex:Concept
    {
        public static concept_complex CreateNewConceptComplex()
        {
            return new concept_complex();
        }

        public static concept_complex CreateConceptComplex(
            int conceptId,
            string Handler
           
            )
        {
            return new concept_complex
            {
                conceptId = conceptId,
                handler = Handler
            };

        }
        #region State Properties
        //public virtual int concept_id { get; set; }

        public virtual string handler { get; set; }

        #endregion
    }
}
