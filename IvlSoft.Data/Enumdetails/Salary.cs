using System;
using System.Collections;
using System.Reflection;
using System.ComponentModel;

namespace INTUSOFT.Data.Enumdetails
{

    public enum SalaryEnum//Specifies the list of salaries of a patient.
    {
        //This below code has been changed by Darshan on 07-09-2015 as per requirement suggested in Defect no 0000460: CR:The underscore must be removed in the Patient details fields.
        //No Salary has been changed from No salary to Not Selected as suggested in Defect no 0000638: Salary,default to be changed as Not Selected. 
        [Description("Not Selected")]
        No_Salary = 1,
        [Description("0 to 1000")]
        Salary_0_to_1000,
        [Description("1000 to 25000")]
        Salary_1000_to_25000,
        [Description("25000 to 50000")]
        Salary_25000_to_50000,
        [Description("50000 to 100000")]
        Salary_50000_to_100000,
        [Description("Above 100000")]
        Salary_Above_100000
    
    }
 

}
