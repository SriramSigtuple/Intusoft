using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportUtils.ReportEnums
{
    public enum BindingType//Gives the BindingTypes of the fields in the report.
    {
        None,
        Age, 
        Name, 
        Doctor, 
        MRN, 
        Gender, 
        Datetime, 
        Comments,
        AnnotationComments,
        Comment1,
        Comment2,
        Comment3,
        Comment4,
        Comment5,
        Address1,
        Address2,
        LeftEyeObs,
        RightEyeObs,
        HospitalName,
        HorizontalCDR,
        CompanyLogo,
        HospitalLogo,
        VerticalCDR,
        DiscArea,
        RimArea,
        CupArea,
        HorizontalDiscLength,
        HorizontalCupLength,
        VerticalDiscLength,
        VerticalCupLength,
        Temporal,
        Inferior,
        Superior,
        Nasal,
        ReportedBy,
        HistoryofAilment,
        GeneralObservation,
        ISNT,
        MedHistory,
        Signature,
        Specalist,
        SpecalistQualification,
        SpecalistHospital,
        FundusReport,
        NameOfTheReport,//binding type added to handle the report names of Normal Report , CDR and Annotation. By Ashutosh 17-08-2017.
        VisitDateTime,
        PatientCreatedDateTime
    }

    public enum ReportTemplateOrientationEnum
    {
        Landscape,Portrait
    }
    public enum ReportTemplateSizeEnum
    {
        A4,A5
    }
}