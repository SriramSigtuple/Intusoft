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
using System.Windows.Shapes;

namespace Intusoft.WPF.UserControls
{
    /// <summary>
    /// Interaction logic for DoctorWindowView.xaml
    /// </summary>
    public partial class DoctorWindowView : Window
    {
        public DoctorListVM DoctorListVM;
        public DoctorWindowView(DoctorListVM doctorListVM)
        {
            InitializeComponent();
            this.DataContext  = doctorListVM;
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }
    }
}
