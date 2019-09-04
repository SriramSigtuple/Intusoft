using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
namespace INTUSOFT.ThumbnailModule.Con
{
  public class Value2StatusColorConvertor :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                SolidColorBrush solidColorBrush = new SolidColorBrush(System.Windows.Media.Colors.Transparent);// (SystemColors.ControlLightBrushKey);
                if (value is int)
                {
                    switch (value)
                    {
                        case 0:
                            solidColorBrush = new SolidColorBrush(System.Windows.Media.Colors.Transparent);

                        break;
                        case 1:
                            solidColorBrush = new SolidColorBrush(System.Windows.Media.Colors.Yellow);
                            break;
                        case 2:
                            solidColorBrush = new SolidColorBrush(System.Windows.Media.Colors.Green);
                            break;
                        case 3:
                            solidColorBrush = new SolidColorBrush(System.Windows.Media.Colors.Gray);
                            break;
                        case 4:
                            solidColorBrush = new SolidColorBrush(System.Windows.Media.Colors.Red);
                            break;
                        default:
                        solidColorBrush = new SolidColorBrush(System.Windows.Media.Colors.Transparent);

                        break;

                    }
                }
                return solidColorBrush;
            }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
