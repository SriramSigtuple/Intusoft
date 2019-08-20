using System;
using System.Windows;
using System.Windows.Data;
namespace IVLUploader.Convertors
{
    public class Bool2FontStyleConvertor :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Boolean && (bool)value)
            {
                return   FontWeights.Bold;
            }
            return FontWeights.Regular;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Visibility && (Visibility)value == Visibility.Visible)
            {
                return true;
            }
            return false;
        }
    }
}
