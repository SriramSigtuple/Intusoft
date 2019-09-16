using INTUSOFT.Data.NewDbModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ListView = System.Windows.Controls.ListView;
using INTUSOFT.EventHandler;

namespace INTUSOFT.Desktop.Forms
{

    /// <summary>
    /// Interaction logic for ReportListView.xaml
    /// </summary>
    public partial class ReportListView : System.Windows.Controls.UserControl
    {
        private IVLEventHandler eventHandler;

        public ReportListView()
        {
            InitializeComponent();
            this.DataContext = ReportListVM.GetInstance() ;
            eventHandler = IVLEventHandler.getInstance();

        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
                if (ReportList.SelectedItems.Count == 1)
                {
                  IList items = ReportList.SelectedItems;

                report lvItem = (report)items[0];
                eventHandler.Notify(eventHandler.ReportGridDoubleClick, new Args("report", lvItem));

            }
        }
    }
}
