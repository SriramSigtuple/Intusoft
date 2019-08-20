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
using IVLUploader.ViewModels;
using Newtonsoft.Json;
using System.IO;
using NLog;

namespace IVLUploader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel _currentVm;

        public MainWindow()
        {
            InitializeComponent();
            _currentVm = new MainWindowViewModel();
            this.DataContext = _currentVm;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //if (_currentVm.IsServerRunning && _currentVm.UploadFiles.Count > 0 && _currentVm.fileUploader.IsLogin)
            //{
            //    MessageBoxResult res = MessageBox.Show("Do you want to cancel the current upload ?", "Warning", MessageBoxButton.YesNo);
            //    if (res == MessageBoxResult.No)
            //    {
            //        e.Cancel = true;
            //    }
            //    //else
            //    //{
            //    //    _currentVm.
            //    //}
            //}
            //else
            //    _currentVm.WriteUploaderData(new object());
        }

    }
}
