using System;
using System.Collections;
using System.Reflection;
using System.ComponentModel;
namespace INTUSOFT.Data.Enumdetails
{
    public enum GenderEnum//Specifies the list of genders with default value as Not_Selected.
    {
        //This below code has been changed by Darshan on 07-09-2015 from Gender to Not_Selected as per requirement suggested in Defect no 0000626: CR:gender column to have Male,female,Not selected options present.
        [Description("Not Selected")]
        Not_Selected =1,
        Male ,
        Female
    }

}
