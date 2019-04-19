using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MT_MusicPlayer.Common
{
    public class RelayCommand : ICommand
    {
        private readonly Action Exec;
        private readonly Action<object> ExecObj;
        private readonly Func<bool> CanExec;

        public RelayCommand(Action execute) : this(execute, () => true) { }
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            Exec = execute;
            CanExec = canExecute;
        }

        public RelayCommand(Action<object> execute) : this(execute, () => true) { }
        public RelayCommand(Action<object> execute, Func<bool> canExecute)
        {
            ExecObj = execute;
            CanExec = canExecute;
        }

        public void Execute(object param)
        {
            Exec?.Invoke();
            ExecObj?.Invoke(param);
        }

        public bool CanExecute(object param) => CanExec.Invoke();

        #pragma warning disable 0067
        public event EventHandler CanExecuteChanged;
        #pragma warning restore 0067
    }
}