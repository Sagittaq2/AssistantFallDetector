using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AssistantFallDetector.ViewModels.Base
{
    public class DelegateCommand : ICommand
    {
        private Action execute;
        private Func<bool> canExecute;

        public DelegateCommand(Action exec, Func<bool> canExec)
        {
            this.execute = exec;
            this.canExecute = canExec;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
                return true;

            return canExecute();
        }


        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (execute != null)
                execute();
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(null, new EventArgs());
        }
    }

    public class DelegateCommand<T> : ICommand
    {
        private Action<T> execute;
        private Func<T, bool> canExecute;

        public DelegateCommand(Action<T> exec, Func<T, bool> canExec)
        {
            this.execute = exec;
            this.canExecute = canExec;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
                return true;

            return canExecute((T)parameter);
        }


        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (execute != null)
                execute((T)parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(null, new EventArgs());
        }
    }
}
