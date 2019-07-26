using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections;

namespace INTUSOFT.Data.Model
{
  public  class VisitModel
    {
      public static VisitModel CreateNewVisitModel()
      {
          return new VisitModel();
      }
      public static VisitModel CreateVisitModel
          (int id,
           DateTime visitDateTime,
           string time,
           int noOfImages,
           int noOfReports,
           string comments,
           double height,
           int weight,
           double rSph,
           double rCyl,
           int rAxis,
           double lSph,
           double lCyl,
           int lAxis,
           int sBp,
           int dBp,
           double bloodSugar,
           string referredBy,
           string deviceOperator,
           string referredTo,
           bool hideShowRow,
          DateTime visitModifyDateTime,
          DateTime visitTouchedDateTime,
          int patId,
          Patient _patient,
          IList<ImageModel> _images,
          IList<Report> _reports)
      {
          return new VisitModel
          {
              ID = id,
              VisitDateTime = visitDateTime,

              Time = time,
              NoOfImages = noOfImages,
              VisitModifyDateTime=visitModifyDateTime,
              VisitTouchDateTime=visitTouchedDateTime,
              NoOfReports = noOfReports,
              Comments = comments,
              RSph = rSph,
              RCyl = rCyl,
              RAxis = rAxis,
              LSph = lSph,
              LCyl = lCyl,
              LAxis = lAxis,
              SBp = sBp,
              DBp = dBp,
              BloodSugar = bloodSugar,
              ReferredBy = referredBy,
              ReferredTo = referredTo,
              DeviceOperator = deviceOperator,
              HideShowRow = hideShowRow,
              PatientID = patId,
              patient = _patient,
              Images = _images,
              Reports = _reports
          };
      }

      public virtual int ID { get; set; }

      public virtual DateTime VisitDateTime { get; set; }

      public virtual string Time { get; set; }

      public virtual int NoOfImages { get; set; }

      public virtual int NoOfReports { get; set; }

      public virtual string Comments { get; set; }


      public virtual double RSph { get; set; }

      public virtual double RCyl { get; set; }

      public virtual int RAxis { get; set; }

      public virtual double LSph { get; set; }

      public virtual double LCyl { get; set; }

      public virtual int LAxis { get; set; }

      public virtual int SBp { get; set; }

      public virtual int DBp { get; set; }

      public virtual double BloodSugar { get; set; }

      public virtual string ReferredBy { get; set; }

      public virtual string DeviceOperator { get; set; }

      public virtual string ReferredTo { get; set; }

      public virtual bool HideShowRow { get; set; }
      public virtual DateTime VisitModifyDateTime { get; set; }

      public virtual DateTime VisitTouchDateTime { get; set; }
      public virtual int PatientID { get; set; }

      public virtual IList<ImageModel> Images { get; set; }

      public virtual IList<Report> Reports { get; set; }

      public virtual Patient patient { get; set; }
    }
}
