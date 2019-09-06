using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common
{


    public static class MD5Helper
        {

        private static MD5ReturnResponse MD5ReturnResponse = new MD5ReturnResponse();

        /// <summary>
        /// Extension method that converts the input string MD5 hash 
        /// </summary>
        /// <param name="input">String input to be converted to MD5 hash</param>
        /// <returns>MD5 hash value of the input string</returns>
        public static string GetMd5Hash(this string input)
        {
            MD5 md5Hash = MD5.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        /// <summary>
        /// Extension method that converts the input FileInfo to a file stream to convert to  MD5 hash
        /// </summary>
        /// <param name="input">FileInfo input to be converted to MD5 hash</param>
        /// <returns>MD5 hash value of the input FileInfo</returns>
        public static MD5ReturnResponse GetMd5Hash(this FileInfo input)
        {
            try
            {
                if (File.Exists(input.FullName))
                {
                    MD5 md5Hash = MD5.Create();
                    var fileStream = input.Open(FileMode.Open);
                    // Convert the input string to a byte array and compute the hash.
                    byte[] data = md5Hash.ComputeHash(fileStream);
                    fileStream.Close();

                    // Create a new Stringbuilder to collect the bytes
                    // and create a string.
                    StringBuilder sBuilder = new StringBuilder();

                    // Loop through each byte of the hashed data 
                    // and format each one as a hexadecimal string.
                    for (int i = 0; i < data.Length; i++)
                    {
                        sBuilder.Append(data[i].ToString("x2"));
                    }
                    MD5ReturnResponse.status = MD5responseStatusEnum.Success;
                    MD5ReturnResponse.responseMessage = sBuilder.ToString();
                }
                else
                {
                    MD5ReturnResponse.status = MD5responseStatusEnum.Failure;
                    MD5ReturnResponse.responseMessage = "File Not Exist";
                }
            }
            catch (System.Exception ex)
            {

                MD5ReturnResponse.status = MD5responseStatusEnum.Failure;
                MD5ReturnResponse.responseMessage = ex.Message;
            }

            // Return the hexadecimal string.
            return MD5ReturnResponse;
        }



    }

}
