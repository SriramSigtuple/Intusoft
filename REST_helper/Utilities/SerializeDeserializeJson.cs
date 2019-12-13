using Cloud_Models.Models;
using System.IO;

namespace IntuUploader.Utilities
{
    public static class URL_ComputeHelper
    {
        public static string GetUrl(URL_Model urlModel)
        {
            var url = urlModel.API_URL;

            if (!string.IsNullOrEmpty(urlModel.API_URL_Start_Point))
                url += Path.DirectorySeparatorChar + urlModel.API_URL_Start_Point;

            if (!string.IsNullOrEmpty(urlModel.API_URL_Mid_Point))
                url += Path.DirectorySeparatorChar + urlModel.API_URL_Mid_Point;

            if (!string.IsNullOrEmpty(urlModel.API_URL_End_Point))
                url += Path.DirectorySeparatorChar + urlModel.API_URL_End_Point;

            return url;
        }
    }
}
