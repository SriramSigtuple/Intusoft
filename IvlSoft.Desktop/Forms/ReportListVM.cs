using BaseViewModel;
using INTUSOFT.Data.NewDbModel;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace INTUSOFT.Desktop
{
    public class ReportListVM : ViewBaseModel
    {
        private static ReportListVM reportList;
            public static ReportListVM GetInstance()
        {
            if (reportList == null)
                reportList = new ReportListVM();
            return reportList;
        }
        private ReportListVM()
        {
            //DoubleClick = new RelayCommand(param=> doublclickFunction((report)param));
        }
        private BindingList<report> reports;

        public BindingList<report> Reports
        {
            get { return reports; }
            set {
                reports = value;
                OnPropertyChanged();

            }
        }

        //public ICommand DoubleClick { get; set; }
        private void doublclickFunction(report report)
        {
        }
    }
}
