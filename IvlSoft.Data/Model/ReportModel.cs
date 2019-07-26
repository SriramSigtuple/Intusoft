using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace INTUSOFT.Data.Model
{
   public class Report
    {
       public static Report CreateNewReport()
       {
           return new Report();
       }

       public static Report CreateReport
           (int id,
           int noOfimages,
            string referredBy,
           string referredTo,
           string deviceOperator,
           string comments,
           string email,
           int hideShowRow,
           string reportBy,
           DateTime reportDateTime,
           DateTime reportModifyDateTime,
           DateTime reportTouhedDateTime,
           int patId,
               int visitId,
                   string reportXml)
       {
           return new Report
           {
               ID = id,
               ReportModifyDateTime=reportModifyDateTime,
               ReportTouchedDateTime=reportTouhedDateTime,
               NoOfImages = noOfimages,
               ReferredBy = referredBy,
               ReferredTo = referredTo,
               Comments = comments,
               DeviceOperator = deviceOperator,
               Email = email,
               HideShowRow = hideShowRow,
               ReportBy = reportBy,
               ReportDateTime = reportDateTime,
               
               PatID = patId,
                VisitID = visitId,
                ReportXML = reportXml,
          
           };
       }

       public virtual int ID { get; set; }

       public virtual int NoOfImages { get; set; }

       public virtual string ReferredBy { get; set; }

       public virtual string ReferredTo { get; set; }

       public virtual string DeviceOperator { get; set; }

       public virtual string Comments { get; set; }

       public virtual string Email { get; set; }

       public virtual int HideShowRow { get; set; }

       public virtual DateTime ReportDateTime { get; set; }
     

       public virtual string  ReportBy { get; set; }

       public virtual string  ReportXML { get; set; }

       public virtual int PatID { get; set; }

       public virtual int VisitID { get; set; }
       public virtual DateTime ReportModifyDateTime { get; set; }
       public virtual DateTime ReportTouchedDateTime { get; set; }

       public virtual VisitModel visit { get; set; }

    }
}
