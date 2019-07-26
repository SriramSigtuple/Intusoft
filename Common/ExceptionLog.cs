using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
   public class Exception2StringConverter
    {
       static Exception2StringConverter exceptionLog;

       public static Exception2StringConverter GetInstance()
       {
           if (exceptionLog == null)
               exceptionLog = new Exception2StringConverter();
           return exceptionLog;
       }
       private Exception2StringConverter()
       {
       }
       public string ConvertException2String(Exception ex)
       {
           StringBuilder stringBuilder = new StringBuilder();

           while (ex != null) // to get all the exception
           {
               stringBuilder.AppendLine(ex.Message);
               stringBuilder.AppendLine(ex.StackTrace);
               ex = ex.InnerException;
           }
           string excepStr = stringBuilder.ToString();
           return excepStr; //returning the exception string
       }

    }
}
