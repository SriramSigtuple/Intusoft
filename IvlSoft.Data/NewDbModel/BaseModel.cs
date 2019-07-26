using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
    public interface IBaseModel//Base model for every datamodels in NewDbModel.
    {
        bool voided { get; set; }
        DateTime createdDate { get; set; }
        DateTime lastModifiedDate { get; set; }

    }
}
