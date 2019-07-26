using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;

namespace INTUSOFT.Data.Enumdetails
{
    public enum UserImageTypeEnum//Types which user can upload his photo for profile picture,
    {
        [Description("Not Selected")]
        Not_Selected=1,
        Upload,
        Webcam
    }
}
