using BaseViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace INTUSOFT.ThumbnailModule
{
    public class ImageLabelVM : ViewBaseModel
    {
        private int qiStatus;
        private string name;

        public ImageLabelVM()
        {
        }
        public int QiStatus
        {
            get => qiStatus;
            set
            {
                qiStatus = value;
                OnPropertyChanged("QiStatus");
            }
        }
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }


        public ICommand ClickCommand { get; set; }

    }
}
