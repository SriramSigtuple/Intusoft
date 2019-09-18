﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
namespace INTUSOFT.Desktop.Forms.Convertor
{
  public class DateConvertor :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                var val = (DateTime)value ;
                return val.ToString("dd-MMM-yyyy");
            }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class TimeConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var val = (DateTime)value;

            if (System.Convert.ToBoolean(IVLVariables.CurrentSettings.UserSettings._Is24clock.val))
            return val.ToString("HH:mm");
            else
            return val.ToString("hh:mm tt");

        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class CloudStatusConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Enum.GetName(typeof(CloudReportStatus),(int)value);


        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class IndexConverter : IValueConverter
    {
        public object Convert(object value, Type TargetType, object parameter, CultureInfo culture)
        {
            ListViewItem item = (ListViewItem)value;
            ListView listView = ItemsControl.ItemsControlFromItemContainer(item) as ListView;
            int index = listView.ItemContainerGenerator.IndexFromContainer(item) + 1 ;
            return index.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class Bool2ServerStatusConvertor : IValueConverter
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

    public class TextColorStatusConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            

            if (value is Boolean && (bool)value)
            {

                return new SolidColorBrush(System.Windows.Media.Colors.Green); ;


            }
            else
            {
                return new SolidColorBrush(System.Windows.Media.Colors.Red); ;

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
