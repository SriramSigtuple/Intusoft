using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
  public class concept_datatype
    {

       public static concept_datatype CreateNewConceptDataType()
        {
            return new concept_datatype();
        }

       public static concept_datatype CreateConceptDataType(
             int datatypeId,
             string Name,
           string h17Abbrevation,
             string Description,
             string UUID
             )
        {

            return new concept_datatype
            {
              conceptDatatypeId = datatypeId,
              name=Name,
              h17Abbreviation = h17Abbrevation,
              description=Description,
              uuid=UUID

            };

        }

         #region State Properties

         int conceptDatatypeId { get; set; }

         string name { get; set; }

         string h17Abbreviation { get; set; }

         string description { get; set; }

         string uuid { get; set; }

        #endregion

    }
}
