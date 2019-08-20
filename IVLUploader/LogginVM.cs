using BaseViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntuUploader
{
   public class LogginVM : ViewBaseModel
    {
        private static LogginVM logginVM;

        public static LogginVM GetLogginVM()
        {
            if (logginVM == null)
                logginVM = new LogginVM();
            return logginVM;
        }
        private LogginVM()
        {
            Logs = new BindingList<string>();
        }
        private BindingList<string> logs;
        public BindingList<string> Logs
        {   get
            {
                return logs;
            }
            set
            {
                logs = value;
                OnPropertyChanged("Logs");

            }
        }

        public string LogValue
        {
           get => logValue;
           set
            {
                logValue = value;
                OnPropertyChanged("LogValue");
            }
        }

        private string logValue;

    }
}
