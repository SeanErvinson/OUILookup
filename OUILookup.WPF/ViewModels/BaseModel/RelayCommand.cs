using System;
using System.Windows.Input;

namespace OUILookup.WPF.ViewModels.BaseModel
{
    class RelayCommand : ICommand
    {
        private readonly Action<object> _executeMethod;
        private readonly Predicate<object> _verifyMethod;

        public RelayCommand(Action<object> execute) : this(execute, null) { }

        public RelayCommand(Action<object> execute, Predicate<object> verify)
        {
            _executeMethod = execute ?? throw new ArgumentNullException("execute");
            _verifyMethod = verify;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _verifyMethod == null ? true : _verifyMethod(parameter);
        }

        public void Execute(object parameter)
        {
            _executeMethod(parameter);
        }
    }
}
