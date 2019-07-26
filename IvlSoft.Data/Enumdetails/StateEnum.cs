using System;
using System.Collections;
using System.Reflection;
using System.ComponentModel;
namespace INTUSOFT.Data.Enumdetails
{
    public enum StateEnum//Specifies the list of states in india with default value as State_Not_Selected.
    {
        //This below code has been changed by Darshan on 07-09-2015 as per requirement suggested in Defect no 0000460: CR:The underscore must be removed in the Patient details fields.

     [Description("State Not Selected")]
     State_Not_Selected=1,
     
     AndhraPradesh,
     
     ArunachalPradesh,
     Assam,
     Bihar,
     Chhattisgarh,
     Delhi,
     Goa,
     Gujarat,
     Haryana,
   
     HimachalPradesh,
     
     JammuKashmir,
     Jharkhand,
     Karnataka,
     Kerala,
     
     MadhyaPradesh,
     Maharashtra,
     Manipur,
     Meghalaya,
     Mizoram,
     Nagaland,
     Odisha,
     Punjab,
     Rajasthan,
     Sikkim,
     
     TamilNadu,
     Tripura,
    
     UttarPradesh,
     Uttarakhand,
     
     WestBengal
    
   }
}
