using System;
using System.Collections;
using System.Reflection;
using System.ComponentModel;
namespace INTUSOFT.Data.Enumdetails
{
    public enum OccupationEnum//Specifies the list of Occupations in India with default value as Occupation_Not_Selected.
    {
        //This below code has been changed by Darshan on 07-09-2015 as per requirement suggested in Defect no 0000460: CR:The underscore must be removed in the Patient details fields.
       [Description("Not Selected")]
        Occupation_Not_Selected=1,
        Doctor,
        Engineer,
        Professor,
        Artists,
        Farmer,
        [Description("Sports Person")]
        Sports_person,
        [Description("Govt employee")]
        Govt_employee,
        Others
    }
}
