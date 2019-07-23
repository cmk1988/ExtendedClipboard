using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace ExtendedClipboard
{
    internal static class NativeMethods
    {
        public const int WM_CLIPBOARDUPDATE = 0x031D;
        public static IntPtr HWND_MESSAGE = new IntPtr(-3);
        
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AddClipboardFormatListener(IntPtr hwnd);
        
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
    }
    public class MainViewModel : Form
    {
        public static event EventHandler ClipboardUpdate;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_CLIPBOARDUPDATE)
            {
                s[i] = System.Windows.Clipboard.GetText();
                se(i, s[i]);
            }
            base.WndProc(ref m);
        }

        Action<int, string> act;
        Action<int, string> se;
        public MainViewModel(Action<int, string> act, Action<int, string> se)
        {
            this.act = act;
            this.se = se;
            command = new Command();
            command.CanExecuteFunc = x => true;
            command.ExecuteFunc = x => Com((string)x);
            NativeMethods.SetParent(Handle, NativeMethods.HWND_MESSAGE);
            NativeMethods.AddClipboardFormatListener(Handle);
        }

        int i = 0;
        string[] s = new string[6];

        public void Com(string str)
        {
            s[i] = System.Windows.Clipboard.GetText();
            i = int.Parse(str);
            System.Windows.Clipboard.SetText(s[i] ?? "");
            act(i, s[i]);
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
