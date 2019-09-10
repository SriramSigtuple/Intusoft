﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
namespace INTUSOFT.ThumbnailModule.Con
{
    public class Value2ToolTipTextConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var returnString = string.Empty;
            if (value is int)
            {
                switch (value)
                {
                    case 0:
                        returnString = "Not Analysed";
                        break;
                    case 1:
                        returnString = "QI In Progress (Intialised)";
                        break;
                    case 2:
                        returnString = "QI In Progress (Uploading)";
                        break;
                    case 3:
                        returnString = "QI In Progress (Processing)";
                        break;
                    case 4:
                        returnString = "Gradable";
                        break;
                    case 5:
                        returnString = "Non-Gradable";
                        break;
                    case 6:
                        returnString = "Failed";
                        break;
                    default:
                        returnString = "Not Analysed";
                        break;


                }
            }
            return returnString;
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}