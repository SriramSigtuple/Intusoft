using System;
using System.Collections;
using System.Reflection;
using System.ComponentModel;
namespace INTUSOFT.Data.Enumdetails
{
    public enum CountryEnum//Specifies the list of counrties with default value as Country_Not_Selected.
    {
        //This below code has been changed by Darshan on 07-09-2015  as per requirement suggested in Defect no 0000460: CR:The underscore must be removed in the Patient details fields.
        [Description("Country Not Selected")]
        Country_Not_Selected=1,
        India,
        others
    }
}
