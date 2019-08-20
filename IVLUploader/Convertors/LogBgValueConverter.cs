using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace IVLUploader.Convertors
{
    public class LogBgValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ("Warn" == value.ToString())
            {
                return Brushes.Black;
            }
            else if ("Error" == value.ToString())
            {
                return Brushes.Black;
            }
            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}