using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Net.Http;

namespace Cloud_Models.Models
{
    [Serializable]
    public class AnalysisFlowResponseModel
    {
        public Response_CookieModel LoginResponse;
        public Response_CookieModel CreateAnalysisResponse;
        public Response_CookieModel InitiateAnalysisResponse;
        public List<Response_CookieModel> UploadResponseList;
        public Response_CookieModel GetAnalysisStatusResponse;
        public Response_CookieModel GetDoctorApprovalResponse;
        public Response_CookieModel NotifyEmail2DoctorResponse;
        public Response_CookieModel GetAnalysisPostDoctorResponse;
        public Response_CookieModel DoctorCommentsResponse;
        public Response_CookieModel GetAnalysisResultResponse;

        public AnalysisFlowResponseModel()
        {
            LoginResponse = new Response_CookieModel();
            CreateAnalysisResponse = new Response_CookieModel();
            InitiateAnalysisResponse = new Response_CookieModel();
            GetAnalysisStatusResponse = new Response_CookieModel();
            GetAnalysisResultResponse = new Response_CookieModel();
            UploadResponseList = new List<Response_CookieModel>();
            GetDoctorApprovalResponse = new Response_CookieModel();
            NotifyEmail2DoctorResponse = new Response_CookieModel();
            GetAnalysisPostDoctorResponse = new Response_CookieModel();
            DoctorCommentsResponse = new Response_CookieModel();
        }
    }
}
