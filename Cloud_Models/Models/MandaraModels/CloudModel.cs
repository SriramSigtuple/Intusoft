using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Models.Models
{
    [Serializable]
    public class CloudModel
    {

        public int visitID;
        public int patientID;
        public int reportID;
        public int cloudID;
        public int ImageID;
        public Cookie LoginCookie;

        public LoginModel LoginModel;

        public CreateAnalysisModel CreateAnalysisModel;

        public UploadModel UploadModel;

        public InitiateAnalysisModel InitiateAnalysisModel;

        public GetAnalysisModel GetAnalysisModel;

        public GetAnalysisResultModel GetAnalysisResultModel;

        public ApproveModel DoctorApprovalModel;

        public NotifyEmailModel NotifyEmailModel;

        public AnalysisFlowResponseModel AnalysisFlowResponseModel;
        public CloudModel()
        {
            LoginModel = new LoginModel();
            CreateAnalysisModel = new CreateAnalysisModel();
            UploadModel = new UploadModel();
            InitiateAnalysisModel = new InitiateAnalysisModel();
            GetAnalysisModel = new GetAnalysisModel();
            GetAnalysisResultModel = new GetAnalysisResultModel();
            DoctorApprovalModel = new ApproveModel();
            NotifyEmailModel = new NotifyEmailModel();

            AnalysisFlowResponseModel = new AnalysisFlowResponseModel();
        }
    }
}
