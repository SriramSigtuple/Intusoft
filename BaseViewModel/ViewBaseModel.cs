using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Input;
namespace BaseViewModel
{
    public abstract class ViewBaseModel:INotifyPropertyChanged,IDisposable
    {

        protected virtual void VerifyPropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string exceptionMsg = "Invalid Property Name = " + propertyName;
                if (ThrowInvalidPropertyName)
                    throw new Exception(exceptionMsg);
                else
                    Debug.Fail(exceptionMsg);
            }
        }

        protected virtual bool ThrowInvalidPropertyName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }

        }

        public void Dispose()
        {
            OnDispose();
        }

        protected void OnDispose()
        {
        }
    }
}
