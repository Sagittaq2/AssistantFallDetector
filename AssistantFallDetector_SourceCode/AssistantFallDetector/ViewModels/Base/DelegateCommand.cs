using System;
using System.Windows.Input;

namespace AssistantFallDetector.ViewModels.Base
{
    public class DelegateCommand : ICommand
    {
        private Action execute;
        private Func<bool> canExecute;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="execute">Method to execute with the command</param>
        /// <param name="canExecute">Method evaluates whether the command is executed</param>
        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (this.canExecute != null)
                return this.canExecute();

            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            this.execute();
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(null, new EventArgs());
        }
    }

    /// <summary>
    /// Allow sends an object to a command
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
