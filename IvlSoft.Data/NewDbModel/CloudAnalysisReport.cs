using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Data.NewDbModel
{
    public class CloudAnalysisReport:  IDisposable, IBaseModel, IComparable<CloudAnalysisReport>
    {
        public static CloudAnalysisReport CreateCloudAnalysisReport()
        {
            return new CloudAnalysisReport();
        }

        public virtual int CompareTo(CloudAnalysisReport other)
        {
            int returnVal = 0;
            if (other != null)
                returnVal = this.cloudAnalysisReportId.CompareTo(other.cloudAnalysisReportId);
            return returnVal;
        }
        public CloudAnalysisReport()
        {
            this.createdDate = DateTime.Now;
            this.lastModifiedDate = DateTime.Now;
        }

        public static CloudAnalysisReport CreateCloudAnalysisReport(int cloudId, report Report, bool voided, int cloudAnalysisReportStatusStr, string leftEyeImp, string rightEyeImp, DateTime createdDateTime, DateTime lastModifiedDateTime,string fileName)
        {
            return new CloudAnalysisReport
            {
                cloudAnalysisReportId = cloudId,
                Report = Report,
                voided = voided,
                fileName = fileName,
                leftEyeImpression = leftEyeImp,
                rightEyeImpression = rightEyeImp,
                cloudAnalysisReportStatus = cloudAnalysisReportStatusStr
            };
        }
        public static CloudAnalysisReport CreateNewCloudAnlysisReport(CloudAnalysisReport proxyCloudAnalysisReport)
        {
            return new CloudAnalysisReport
            {
                cloudAnalysisReportId = proxyCloudAnalysisReport.cloudAnalysisReportId,
                Report = proxyCloudAnalysisReport.Report,
                leftEyeImpression = proxyCloudAnalysisReport.leftEyeImpression,
                rightEyeImpression = proxyCloudAnalysisReport.rightEyeImpression,
                fileName = proxyCloudAnalysisReport.fileName,
                voided = proxyCloudAnalysisReport.voided,
                cloudAnalysisReportStatus = proxyCloudAnalysisReport.cloudAnalysisReportStatus,
                lastModifiedDate = proxyCloudAnalysisReport.lastModifiedDate,
                createdDate = proxyCloudAnalysisReport.createdDate
            };
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }


        #region State Properties

        public virtual DateTime lastModifiedDate { get ; set ; }

        public virtual int cloudAnalysisReportId { get; set; }

        public virtual string leftEyeImpression { get; set; }

        public virtual string rightEyeImpression { get; set; }

        public virtual string fileName { get; set; }
        public virtual DateTime createdDate { get; set; }

        public virtual bool voided { get; set; }

        public virtual int cloudAnalysisReportStatus { get; set; }

        public virtual report Report { get; set; }

        #endregion
    }
}
