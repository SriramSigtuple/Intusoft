using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
namespace IntuUploader.Controls
{
    /// <summary>
    /// Interaction logic for IVLButton.xaml
    /// </summary>
    public partial class IVLButton : UserControl,INotifyPropertyChanged
    {
        public IVLButton()
        {
            InitializeComponent();
           // ButtonBackground = new LinearGradientBrush(Colors.Red, Colors.Green, 30);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        } 
        public string ButtonText
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        static FrameworkPropertyMetadata propertymetadata = new FrameworkPropertyMetadata("Comes as Default",
                                                                             FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                             new PropertyChangedCallback(onTextChanged),
                                                                             new CoerceValueCallback(MyCustom_CoerceValue),
                                                                             false,
                                                                             UpdateSourceTrigger.PropertyChanged);
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("ButtonText", typeof(string), typeof(IVLButton), propertymetadata, new ValidateValueCallback(MyCustom_Validate));

        private static bool MyCustom_Validate(object Value)
        {
            //Custom validation block which takes in the value of DP
            //Returns true / false based on success / failure of the validation
            //Observer.Add(string.Format("DataValidation is Fired : Value {0}", Value));


            return true;
        }
        private static object MyCustom_CoerceValue(DependencyObject dobj, object Value)
        {
            //called whenever dependency property value is reevaluated. The return value is the 
            //latest value set to the dependency property
            //Observer.Add(string.Format("CoerceValue is fired : Value {0}", Value));
            return Value;
        }
        public string ButtonImage
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }


        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("ButtonImage", typeof(string), typeof(IVLButton), new FrameworkPropertyMetadata("ButtonImage", FrameworkPropertyMetadataOptions.AffectsRender));

        //public LinearGradientBrush ButtonBackground
        //{
        //    get { return (LinearGradientBrush)GetValue(BackgroundBrush); }
        //    set { SetValue(BackgroundBrush, value); }
        //}


        //public static readonly DependencyProperty BackgroundBrush =
        //    DependencyProperty.Register("ButtonBackground", typeof(LinearGradientBrush), typeof(IVLButton), new PropertyMetadata("ButtonBackground", new PropertyChangedCallback(onBackGroundChanged)));

        //private static void onBackGroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    ((IVLButton)d).onBackGroundChanged(e);
        //}
        //private void onBackGroundChanged(DependencyPropertyChangedEventArgs e)
        //{
        //    this.ButtonBackground = (LinearGradientBrush) e.NewValue;
        //}
        public static void onTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IVLButton button = d as IVLButton;
            button.OnPropertyChanged("ButtonText");
            button.onTextChanged(e);
        }
        public void onTextChanged(DependencyPropertyChangedEventArgs e)
        {
            ButtonText = e.NewValue.ToString();
        }
    
    }
}
