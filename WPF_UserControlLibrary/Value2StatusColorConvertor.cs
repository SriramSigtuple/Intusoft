using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Common.Enums;
using INTUSOFT.Data.NewDbModel;
namespace Intusoft.WPF.UserControls.Convertor
{
    public class FontSizeClass
    {
        private static double fontSize;

        public static double FontSize
        {
            get { return fontSize; }
            set
            {
                if (fontSize != value)
                {
                    fontSize = value;
                }


            }
        }
    }
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

            if (System.Convert.ToBoolean(InternetCheckViewModel.Settings.UserSettings._Is24clock.val))
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
            var cloudReport = (CloudAnalysisReport)value;
            var failureMessage = string.IsNullOrEmpty(cloudReport.failureMessage) ? "" : cloudReport.failureMessage;
            return Enum.GetName(typeof(CloudReportStatus),(int)cloudReport.cloudAnalysisReportStatus) + " " + failureMessage ;

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

    public class InvertBoolConverter : IValueConverter
    {

        private bool _inverted = false;

        public bool Inverted

        {

            get { return _inverted; }

            set { _inverted = value; }

        }



        private bool _not = false;

        public bool Not

        {

            get { return _not; }

            set { _not = value; }

        }



        private object BoolToVisibility(object value)

        {

            if (!(value is bool))

                return DependencyProperty.UnsetValue;



            return ((bool)value ^ Not) ? Visibility.Visible

                : Visibility.Collapsed;

        }
        private object VisibilityToBool(object value)

        {

            if (!(value is Visibility))

                return DependencyProperty.UnsetValue;



            return (((Visibility)value) == Visibility.Visible) ^ Not;

        }


        public object Convert(object value, Type targetType,

                object parameter, CultureInfo culture)

        {

            return  BoolToVisibility(value);

        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
