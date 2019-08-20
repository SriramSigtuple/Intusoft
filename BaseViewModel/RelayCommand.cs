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
  public class RelayCommand : ICommand
    {
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;
        public RelayCommand(Action<object> execute):this(execute,null)
        {

        }
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException();
            else
            {
                _execute = execute;
                _canExecute = canExecute;
            }
        }
        public bool CanExecute(object param)
        {
            return _canExecute == null ? true : _canExecute(param);
        }

        public void Execute(object param)
        {
            _execute(param);
        }
        public event EventHandler CanExecuteChanged
        {
            add    { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
