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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Intusoft.WPF.UserControls
{
    /// <summary>
    /// Interaction logic for InternetStatusUCL.xaml
    /// </summary>
    public partial class InternetStatusUCL : UserControl
    {
        public InternetStatusUCL()
        {
            InitializeComponent();
            this.DataContext = InternetCheckViewModel.GetInstance();
           // this.Loaded += InternetStatusUCL_Loaded;
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            HwndSource hwnd = System.Windows.PresentationSource.FromVisual(this) as HwndSource;
            HwndTarget target = hwnd.CompositionTarget;
            target.RenderMode = RenderMode.SoftwareOnly;
        }
    }
}
