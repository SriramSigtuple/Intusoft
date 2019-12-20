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
using Intusoft.WPF.UserControls;

namespace test_app_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DoctorListVM doctorListVM = new DoctorListVM();
            DoctorWindowView doctorWindowView = new DoctorWindowView(doctorListVM);
            var returnVal = doctorWindowView.ShowDialog();

           doctorListVM =  doctorWindowView.DataContext as DoctorListVM;
           MessageBox.Show( doctorListVM.DiagResult.ToString());
           
        }
    }
}
