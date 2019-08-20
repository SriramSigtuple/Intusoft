using Cloud_Models.Models;
using NLog;
using System.IO;

namespace IntuUploader.Utilities
{
    public static class URL_ComputeHelper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static string GetUrl(this URL_Model urlModel)
        {
            logger.Info("");

            var url = urlModel.API_URL;

            if (!string.IsNullOrEmpty(urlModel.API_URL_Start_Point))
                url += urlModel.API_URL_Start_Point;

            if (!string.IsNullOrEmpty(urlModel.API_URL_Mid_Point))
                url += "/" + urlModel.API_URL_Mid_Point;

            if (!string.IsNullOrEmpty(urlModel.API_URL_End_Point))
                url += "/" + urlModel.API_URL_End_Point;
            logger.Info("");

            return url;
        }
    }
}
