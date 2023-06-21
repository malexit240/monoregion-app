using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Monoregion.App.Helpers
{
    public class SingleExecutionCommand : ICommand
    {
        private Func<object, Task> _command;
        private Func<object, bool> _canExecute;
        private bool _isExecuting;
        private int _delayMillisec;

        private SingleExecutionCommand(object onContinueCommand)
        {
        }

        private SingleExecutionCommand()
        {
        }

        public static SingleExecutionCommand FromFunc(Func<Task> command, Func<bool> canExecute = null, int delayMillisec = 400)
        {
            if (command is null)
            {
                throw new ArgumentNullException("Command parameter 'command' cannot be null");
            }

            var ret = new SingleExecutionCommand
            {
                _command = obj => command(),
                _canExecute = obj => canExecute is null || canExecute(),
                _delayMillisec = delayMillisec,
            };

            return ret;
        }

        public static SingleExecutionCommand FromFunc(Func<object, Task> command, Func<object, bool> canExecute = null, int delayMillisec = 400)
        {
            if (command is null)
            {
                throw new ArgumentNullException("Command parameter 'command' cannot be null");
            }

            var ret = new SingleExecutionCommand
            {
                _command = obj => command(obj),
                _canExecute = obj => canExecute is null || canExecute(obj),
                _delayMillisec = delayMillisec,
            };

            return ret;
        }

        internal static ICommand FromFunc(Func<Task> onConfirmWithdrawCommandAsync, bool canWithdrawFunds)
        {
            throw new NotImplementedException();
        }

        public static SingleExecutionCommand FromFunc<T>(Func<T, Task> func, Func<T, bool> canExecute = null, int delayMillisec = 400)
        {
            var ret = new SingleExecutionCommand
            {
                _command = obj => func((T)obj),
                _canExecute = obj => canExecute is null || canExecute((T)obj),
                _delayMillisec = delayMillisec,
            };

            return ret;
        }

        #region -- ICommand implementation --

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            var isExecutable = _canExecute is null || _canExecute(parameter);

            return isExecutable;
        }

        public async void Execute(object parameter)
        {
            if (_isExecuting)
            {
                return;
            }

            _isExecuting = true;

            await _command(parameter);
            if (_delayMillisec > 0)
            {
                await Task.Delay(_delayMillisec);
            }

            _isExecuting = false;
        }

        #endregion
    }
}
