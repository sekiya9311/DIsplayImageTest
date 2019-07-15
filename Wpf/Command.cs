using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Wpf
{
    class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly Action _act;

        public Command(Action act)
        {
            _act = act;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            _act?.Invoke();
        }
    }
}
