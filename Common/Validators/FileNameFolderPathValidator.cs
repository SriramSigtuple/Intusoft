using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace Common.Validators
{

    public enum FolderPathErrorCode { FolderPath_Empty, Success, InvalidDirectory, DirectoryDoesnotExist }
    public class FileNameFolderPathValidator
    {
        string fileNameInvalidChar = @"[/*?|>\""<:\\]";
        string folderNameInvalidChar = @"^(([a-zA-Z]\:)|(\\))(\\{1}|((\\{1})[^\\]([^/:*?<>|]*))+)$";
        char[] invalidFileCharsArr;
        char[] invalidFolderCharsArr;
        List<char> invalidCharsList;
        Regex objAlphaPatternFile;
        Regex objAlphaPatternFolder;
        private static FileNameFolderPathValidator fileNameFolderPathValidator;

        public static FileNameFolderPathValidator GetInstance()
        {
            if (fileNameFolderPathValidator == null)
                fileNameFolderPathValidator = new FileNameFolderPathValidator();
            return fileNameFolderPathValidator;
        }

        private FileNameFolderPathValidator()
        {
            objAlphaPatternFile = new Regex(fileNameInvalidChar);//check for the presence of special characters in the filename
            objAlphaPatternFolder = new Regex(folderNameInvalidChar);//check for the presence of special characters in the foldername
            invalidFolderCharsArr = Path.GetInvalidPathChars();
            invalidFileCharsArr = Path.GetInvalidFileNameChars();
            invalidCharsList = new List<char>();
            invalidCharsList.AddRange(invalidFileCharsArr);
            invalidCharsList.AddRange(invalidFolderCharsArr);
        }
        /// <summary>
        /// Checks wheather the path specified contains illegal characters or not.
        /// </summary>
        /// <param name="fileName">Path where image need to be saved</param>
        /// <returns>True if contains illegal charaters else false</returns>
        private bool ContainsInvalidPathCharacters(string fileName)
        {
            bool isFileNameValid = string.IsNullOrEmpty(fileName) || string.IsNullOrWhiteSpace(fileName); //checks for null and white space
            bool isSpecialChars = objAlphaPatternFile.IsMatch(fileName); // validate the filename
            isFileNameValid = isSpecialChars || isFileNameValid;
            return isFileNameValid;
        }

        /// <summary>
        /// validating the File name
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool CheckFileName(string fileName)
        {
            return ContainsInvalidPathCharacters(fileName);
        }

        /// <summary>
        /// validating the folder path
        /// </summary>
        /// <param name="FolderPath"></param>
        /// <param name="returnPath"></param>
        /// <returns></returns>
        public FolderPathErrorCode CheckFolderPath(string FolderPath, ref string returnPath)
        {
            if (string.IsNullOrEmpty(FolderPath) || string.IsNullOrWhiteSpace(FolderPath)) //checks whether the folder path is null or empty or white space
                return FolderPathErrorCode.FolderPath_Empty;

            //else if (objAlphaPatternFolder.IsMatch(FolderPath)) // old implementation using regex
            else
            {


                FolderPathErrorCode errCode = FolderPathErrorCode.FolderPath_Empty;

                if (Directory.Exists(FolderPath)) //checks whether the folder path exists in the directory
                {
                    returnPath = FolderPath;
                    errCode = FolderPathErrorCode.Success;
                }
                else
                {
                   // DirectoryInfo dirInf = new DirectoryInfo(FolderPath);
                    string [] folderNameArr = FolderPath.Split(Path.DirectorySeparatorChar);
                    string folderName = folderNameArr[folderNameArr.Length-1];//dirInf.Name;
                    foreach (char folderPathCharItem in folderName)
                    {
                        foreach (char invalidCharItem in invalidCharsList)
                        {
                            if (folderPathCharItem.CompareTo(invalidCharItem) == 0)
                            {
                                errCode = FolderPathErrorCode.InvalidDirectory;
                                break;
                            }
                        }

                    }
                    if (errCode != FolderPathErrorCode.InvalidDirectory)
                    {
                        errCode = FolderPathErrorCode.DirectoryDoesnotExist;
                    }
                }
                return errCode;

            }

        }
    }
}