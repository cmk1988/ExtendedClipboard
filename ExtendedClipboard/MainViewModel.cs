using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;

namespace ExtendedClipboard
{
    public class MainViewModel
    {
        Action<int> act;
        public MainViewModel(Action<int> act)
        {
            this.act = act;
            command = new Command();
            command.CanExecuteFunc = x => true;
            command.ExecuteFunc = x => Com((string)x);
        }

        int i = 0;
        string[] s = new string[6];

        public void Com(string str)
        {
            s[i] = System.Windows.Clipboard.GetText();
            i = int.Parse(str);
            System.Windows.Clipboard.SetText(s[i] ?? "");
            act(i);
        }

        Command command;
        public Command Command => command;
    }

    public class Command : ICommand
    {
        public Predicate<object> CanExecuteFunc
        {
            get;
            set;
        }

        public Action<object> ExecuteFunc
        {
            get;
            set;
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteFunc(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            ExecuteFunc(parameter);
        }
    }
}
