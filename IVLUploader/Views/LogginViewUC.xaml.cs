using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using NLog.Targets;
namespace IntuUploader.Views
{
    /// <summary>
    /// Interaction logic for LogginViewUC.xaml
    /// </summary>
    public partial class LogginViewUC : UserControl
    {
        //readonly MemoryEventTarget _logTarget;  // My new custom Target (code is attached here MemoryQueue.cs)

        public static ObservableCollection<LogEventInfo> LogCollection { get; set; }


        public LogginViewUC()
        {
            LogCollection = new ObservableCollection<LogEventInfo>();

            InitializeComponent();

            // init memory queue
            //_logTarget.EventReceived += EventReceived;
            //NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(_logTarget, LogLevel.Debug);
        }

        private void EventReceived(LogEventInfo message)
        {
            Dispatcher.Invoke(new Action(() => {
                if (LogCollection.Count >= 50) LogCollection.RemoveAt(LogCollection.Count - 1);
                LogCollection.Add(message);
            }));
        }
    }
}
