using System.Windows;
using System.Windows.Input;
using IntuUploader.ViewModels;
namespace IntuUploader
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
            this.Loaded += MainWindow_Loaded;
          
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _currentVm = new MainWindowViewModel();
            this.DataContext = _currentVm;
            MyNotifyIcon.DataContext = this;

        }
        /// <summary>
        /// Sets <see cref="Window.WindowStartupLocation"/> and
        /// <see cref="Window.Owner"/> properties of a dialog that
        /// is about to be displayed.
        /// </summary>
        /// <param name="window">The processed window.</param>
        private void ShowDialog(Window window)
        {
            window.Owner = this;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        { 
            if(_currentVm.InternetCheckViewModel.QIUploaderVM.SentItemsViewModel.activeFileCloudVM.isBusy
                ||_currentVm.InternetCheckViewModel.QIUploaderVM.OutboxViewModel.activeFileCloudVM.isBusy 
                ||_currentVm.InternetCheckViewModel.FundusUploaderVM.SentItemsViewModel.activeFileCloudVM.isBusy
                || _currentVm.InternetCheckViewModel.FundusUploaderVM.SentItemsViewModel.activeFileCloudVM.isBusy)
            {
                MessageBox.Show("Uploads are in progress ", "Warning", MessageBoxButton.OK);
                while (_currentVm.InternetCheckViewModel.QIUploaderVM.SentItemsViewModel.activeFileCloudVM.isBusy
                || _currentVm.InternetCheckViewModel.QIUploaderVM.OutboxViewModel.activeFileCloudVM.isBusy
                || _currentVm.InternetCheckViewModel.FundusUploaderVM.SentItemsViewModel.activeFileCloudVM.isBusy
                || _currentVm.InternetCheckViewModel.FundusUploaderVM.SentItemsViewModel.activeFileCloudVM.isBusy)
                {
                    this.Cursor = Cursors.Wait;
                }
                this.Cursor = Cursors.Arrow;
                //e.Cancel = true;
            }
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
