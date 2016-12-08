using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CFW.ViewModel
{
    public class DelegateCommand : ICommand
    {
        private readonly Action Action;

        public DelegateCommand(Action action)
        {
            Action = action;
        }

        public void Execute (object parameter)
        {
            Action();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
