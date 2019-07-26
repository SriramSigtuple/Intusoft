using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ValidatorDatas
{
    public enum MailIDErrorCode {ID_Valid, ID_Empty, AtSymbolNotPresent, AtSymbol_MultipleTimes, SpecialCharsPresent, DomainNameNotPresent, DotRSpecialCharB4_AtSymbol, HyphensInDomainName ,HyphensInUserName};
  
    public class MailErrorData
    {
        public bool isValidMail;
        public MailIDErrorCode mailErrorCode;
        public string[] MailErrorMessage = new string[] {"ID valid","Email ID is empty","'@' symbol not present", "'@' symbol present multiple times", "special characters present", "domain name not present", ". or special characters present before the '@' symbol", "'-' in domain name" ,"'-' in user name"};
        public MailErrorData()
        {
            this.isValidMail = true;
            mailErrorCode = MailIDErrorCode.ID_Valid;
        }

    }
}
