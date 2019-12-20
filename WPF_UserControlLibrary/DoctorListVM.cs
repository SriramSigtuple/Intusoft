using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseViewModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;

namespace Intusoft.WPF.UserControls
{
    public class DoctorListVM : ViewBaseModel
    {
        public DoctorListVM()
        {
            Doctors = new BindingList<string>();
            OkCommand = new RelayCommand(okbuttonclick);
            CancelCommand = new RelayCommand(cancel_button_click);
        }
        private  BindingList<string> doctors;

        public BindingList<string>  Doctors
        {
            get { return doctors; }
            set { 
                doctors = value;
                OnPropertyChanged("Doctors");

            }
        }

        private int selectedDoctor;

        public int SelectedDoctor
        {
            get { return selectedDoctor; }
            set
            {
                selectedDoctor = value;
                OnPropertyChanged("SelectedDoctor");
            }
        }

        public ICommand OkCommand { get; set; }

        public ICommand CancelCommand { get; set; }


        private void okbuttonclick( object state)
        {
            DiagResult = true;

            (state as Window).Close();

        }

        private void cancel_button_click(object state)
        {
            DiagResult = false;
            (state as Window).Close();

        }

        private bool diagResult;

        public bool DiagResult
        {
            get { return diagResult; }
            set { 
                diagResult = value;
            }
        }






    }
}
