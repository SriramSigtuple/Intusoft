
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace IVLUploader.Convertors
{
    public class LogFgValueConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ("Warn" == value.ToString())
            {
                return Brushes.Yellow;
            }
            else if ("Error" == value.ToString())
            {
                return Brushes.Tomato;
            }
            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}