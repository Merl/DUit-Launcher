using System;
using System.Windows;
using System.Windows.Input;

namespace DUit_Launcher
{
    public class TbDoubleClick : ICommand
    {
        public DUitLauncher moep;
        public void Execute(object parameter)
        {
            ((DUitLauncher)Application.Current.MainWindow).restoreWindow();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
