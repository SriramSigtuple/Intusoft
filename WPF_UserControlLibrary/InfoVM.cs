using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BaseViewModel;
namespace Intusoft.WPF.UserControls
{
   public class InfoVM : ViewBaseModel
    {


        private static InfoVM infoVM;

        public static InfoVM GetInstance()
        {
            if (infoVM == null)
                infoVM = new InfoVM();
            return infoVM;
        }


        public InfoVM()
        {
            ClickCommand = new RelayCommand(param => click(param));
        }
        private string drQI;

        public string DrQI
        {
            get { return drQI; }
            set
            {
                drQI = value;
                OnPropertyChanged();
            }
        }
        private string amdQI;

        public string AMDQI
        {
            get { return amdQI; }
            set
            {
                amdQI = value;
                OnPropertyChanged();
            }
        }

        private string glaucomaQI;

        public string GlaucomaQI
        {
            get { return glaucomaQI; }
            set
            {
                glaucomaQI = value;
                OnPropertyChanged();
            }
        }

        private bool isVisible;

        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                isVisible = value;
                OnPropertyChanged();
            }
        }
        private bool infoIconVisible;

        public bool InfoIconVisible
        {
            get { return infoIconVisible; }
            set
            {
                infoIconVisible = value;
                OnPropertyChanged();

            }
        }

        public ICommand ClickCommand { get; set; }

        private void click(object value)
        {
            Console.WriteLine($"isClick{IsVisible}");
            InfoIconVisible = isVisible;
            IsVisible = !IsVisible;
            
        }

        private string imgName;

        public string ImgName
        {
            get { return imgName; }
            set
            {
                imgName = value;
                OnPropertyChanged();
            }
        }

    }
}
