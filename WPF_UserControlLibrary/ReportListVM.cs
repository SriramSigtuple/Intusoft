using BaseViewModel;
using INTUSOFT.Data.NewDbModel;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Intusoft.WPF.UserControls
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

        private int width;

        public int Width
        {
            get { return width; }
            set
            {
                if(width != value)
                {
                    width = value;
                    OnPropertyChanged();
                }
              
            }
        }

        private int windowWidth;

        public int WindowWidth
        {
            get { return windowWidth; }
            set
            {
                if(windowWidth != value)
                {
                    windowWidth = value;
                    OnPropertyChanged();
                }
               
            }
        }
        private int fontSize;

        public int FontSize
        {
            get { return fontSize; }
            set
            {
                if(fontSize != value)
                {
                    fontSize = value;
                    OnPropertyChanged();
                }
               

            }
        }


        //public ICommand DoubleClick { get; set; }
        private void doublclickFunction(report report)
        {
        }
    }
}
