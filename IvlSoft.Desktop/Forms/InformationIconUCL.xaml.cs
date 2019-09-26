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

namespace INTUSOFT.Desktop.Forms
{
    /// <summary>
    /// Interaction logic for InformationIconUCL.xaml
    /// </summary>
    public partial class InformationIconUCL : UserControl
    {
        public InformationIconUCL(InfoVM infoVM)
        {
            InitializeComponent();
            this.DataContext =infoVM;
            this.Loaded += InformationIconUCL_Loaded;
        }

        private void InformationIconUCL_Loaded(object sender, RoutedEventArgs e)
        {

            HwndSource hwnd = System.Windows.PresentationSource.FromVisual(this) as HwndSource;
            HwndTarget target = hwnd.CompositionTarget;
            target.RenderMode = RenderMode.SoftwareOnly;
        }
    }
}
