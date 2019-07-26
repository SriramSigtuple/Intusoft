using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.ValidatorDatas;

namespace Common.Validators
{
    
    public class EmailValidator
    {

        #region Constants
        char[] SpecialChars = "!#$%^&*,+=/;:()".ToCharArray();
        char DotCharacter = '.';
        char AtCharacter = '@';
        char SemiColonCharacter = ';';
        char HyphenCharacter = '-';
        #endregion

        #region Variables
        string[] splitEmailId = null;
        int count;
        int indexOf;
        string userName;
        string domainName;
        string[] splitDomainName;
        string[] splitUserName;
        #endregion

        #region Public Methods
        //ctor
        public EmailValidator()
        {

        }



        /// <summary>
        /// To Check the given mail ID is valid or not
        /// </summary>
        /// <param name="strEmail"></param>
        /// <returns></returns>
        public MailErrorData IsValidEmail(string inputEmail)
        {
            inputEmail = inputEmail.Trim(SemiColonCharacter);//trim the last char if it is ;
            inputEmail = inputEmail.Trim(new char[0]);//trim the last null character
            MailErrorData errData = new MailErrorData();

                if (String.IsNullOrEmpty(inputEmail))//checks whether the str email is null or empty
                {
                    errData.isValidMail = false;
                    errData.mailErrorCode = MailIDErrorCode.ID_Empty;
                }
                else
                {
                    errData = ContainsAtTheRate(inputEmail);
                }
            return errData;
        }

        #endregion

        #region Private Methods


        /// <summary>
        /// To check whether it contains '@' in the mail ID
        /// </summary>
        /// <param name="strEmail"></param>
        /// <returns></returns>
        private MailErrorData ContainsAtTheRate(string inputEmail)
        {
            MailErrorData errData = new MailErrorData();

           splitEmailId = inputEmail.Split(AtCharacter);//split the str email at the character @
           count = splitEmailId.Length - 1;// count of chars
            if (count == 0 )//checks the str email does not contain @
            {
                errData.isValidMail = false;
                errData.mailErrorCode = MailIDErrorCode.AtSymbolNotPresent;
            }
            else if (count > 1)
            {
                errData.isValidMail = false;
                errData.mailErrorCode = MailIDErrorCode.AtSymbol_MultipleTimes;
            }
            else
            {
                userName = splitEmailId[0];//assigning the splitted splitEmail[0] to user name
                domainName = splitEmailId[splitEmailId.Length - 1];//assigning the splitted splitEmail[1] to domain  name
                errData = ValidateEmailUserName(userName);
                if (errData.isValidMail)
                    errData = ValidateEmailDomainName(domainName);
            }
            return errData;
        }


        /// <summary>
        /// To validate the username in the mail ID
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        private MailErrorData ValidateEmailUserName(string userName) // validateEmailUserName
        {
            MailErrorData errData = new MailErrorData();
            indexOf = userName.IndexOfAny(SpecialChars);//checking for the presence of special characters 
            if (indexOf != -1 )//checks index not equals 1 OR last character of the username is '.'
            {
                errData.isValidMail = false;
                errData.mailErrorCode = MailIDErrorCode.SpecialCharsPresent;
            }
            else if (userName[userName.Length - 1] == DotCharacter || userName[userName.Length - 1] == HyphenCharacter)//checks for the presence of '.' and '-' as the end characteer
            {
                errData.isValidMail = false;
                errData.mailErrorCode = MailIDErrorCode.DotRSpecialCharB4_AtSymbol;
            }
            else
            {
                count = userName.Count(x => x == DotCharacter);//counts the number of occurences of '.' in domain name and user name
                splitUserName = userName.Split(DotCharacter);
                for (int i = 0; i <= count; i++)
                {
                    errData = CheckForHyphens(splitUserName[i]);
                    if (!errData.isValidMail)
                    {
                        errData.mailErrorCode = MailIDErrorCode.HyphensInUserName;
                        break;
                    }
                }
            }
            return errData;
        }



        /// <summary>
        /// To validate the domain name in the mailID
        /// </summary>
        /// <param name="domainName"></param>
        /// <returns></returns>
        private MailErrorData ValidateEmailDomainName(string domainName) // ValidEmailDomainName
        {
            MailErrorData errData = new MailErrorData();
            indexOf = domainName.IndexOfAny(SpecialChars);//checks the presence of special characters in domain name and returning tru or false
            if (string.IsNullOrEmpty(domainName) || indexOf != -1 || domainName[domainName.Length - 1] == DotCharacter)//checks the domain name is null is empty
            {
                errData.isValidMail = false;
                errData.mailErrorCode = MailIDErrorCode.DomainNameNotPresent;
            }
            else
            {
                    count = domainName.Count(x => x == DotCharacter);//counts the number of occurences of '.' in domain name
                    splitDomainName = domainName.Split(DotCharacter);
                    for (int i = 0; i <= count; i++)
                    {
                        errData = CheckForHyphens(splitDomainName[i]);
                        if (!errData.isValidMail)
                        {
                            errData.mailErrorCode = MailIDErrorCode.HyphensInDomainName;
                            break;
                        }
                    }
            }
            return errData;
        }




        /// <summary>
        /// To check for the improper hyphens in the domain name
        /// </summary>
        /// <param name="SplitDomainName"></param>
        /// <returns></returns>
        private MailErrorData CheckForHyphens(string SplitName)
        {
            MailErrorData errData = new MailErrorData();
            string Domain = SplitName;
            if (!SplitName.Contains(HyphenCharacter) && !string.IsNullOrEmpty(Domain))
            {
                //checks for the presence of any special characters in splitDomainName[count - i]
                if (Domain[0] == HyphenCharacter || Domain[Domain.Length-1] == HyphenCharacter)
                {
                    errData.isValidMail = false;
                }
                else
                    errData.isValidMail = true;
            }
            else
            {
                errData.isValidMail = false;
            }
            return errData;
        }


        #endregion

    }




      
}
