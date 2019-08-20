using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;


namespace IVLUploader.Convertors
{
  public  class Bool2ServerStatusConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            RadialGradientBrush radBrush = new RadialGradientBrush();
            radBrush.GradientOrigin = new System.Windows.Point(0.5, 0.1);
            GradientStop gradStop = new GradientStop(Colors.White, 0.1);
            radBrush.GradientStops.Add(gradStop);

           
            if (value is Boolean && (bool)value)
            {

                //return new SolidColorBrush(System.Windows.Media.Colors.LimeGreen); ;

                GradientStop gradStop1 = new GradientStop(Colors.Green, 0.8);
                radBrush.GradientStops.Add(gradStop1);

            }
            else
            {
                GradientStop gradStop1 = new GradientStop(Colors.Red, 0.8);
                radBrush.GradientStops.Add(gradStop1);
            }
            return radBrush; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
  public class Bool2UploadStatusConvertor : IValueConverter
  {
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
          if (value is Boolean && (bool)value)
          {

              return new SolidColorBrush(System.Windows.Media.Colors.Lavender); ;
          }
          else
              return new SolidColorBrush(System.Windows.Media.Colors.SlateGray); 
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
          throw new NotImplementedException();
      }
  }
  public class Bool2ProgressBarForegroundColor : IValueConverter
  {
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
          if (value is Boolean && (bool)value)
              return Brushes.Green;
          else
              return Brushes.Red;
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
          throw new NotImplementedException();
      }
  }
}
