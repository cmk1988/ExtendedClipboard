using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.IO;
using System.Collections.Specialized;

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
            try
            {

                if (m.Msg == NativeMethods.WM_CLIPBOARDUPDATE)
                {
                    if (System.Windows.Clipboard.ContainsAudio())
                        streams[i] = System.Windows.Clipboard.GetAudioStream();
                    else
                        streams[i] = null;
                    if (System.Windows.Clipboard.ContainsFileDropList())
                        drop[i] = System.Windows.Clipboard.GetFileDropList();
                    else
                        drop[i] = null;
                    if (System.Windows.Clipboard.ContainsImage())
                        img[i] = System.Windows.Clipboard.GetImage();
                    else
                        img[i] = null;
                    if (System.Windows.Clipboard.ContainsText())
                        s[i] = System.Windows.Clipboard.GetText();
                    else
                        s[i] = null;
                    se(i, s[i]);
                }
                base.WndProc(ref m);
            }
            catch
            {

            }
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
        Stream[] streams = new Stream[6];
        StringCollection[] drop = new StringCollection[6];
        BitmapSource[] img = new BitmapSource[6];

        public void Com(string str)
        {
            try
            {
                if (System.Windows.Clipboard.ContainsAudio())
                    streams[i] = System.Windows.Clipboard.GetAudioStream();
                else
                    streams[i] = null;
                if (System.Windows.Clipboard.ContainsFileDropList())
                    drop[i] = System.Windows.Clipboard.GetFileDropList();
                else
                    drop[i] = null;
                if (System.Windows.Clipboard.ContainsImage())
                    img[i] = System.Windows.Clipboard.GetImage();
                else
                    img[i] = null;
                if (System.Windows.Clipboard.ContainsText())
                    s[i] = System.Windows.Clipboard.GetText();
                else
                    s[i] = null;
                i = int.Parse(str);

                if (streams[i] != null)
                    System.Windows.Clipboard.SetAudio(streams[i]);
                if (drop[i] != null)
                    System.Windows.Clipboard.SetFileDropList(drop[i]);
                if (img[i] != null)
                    System.Windows.Clipboard.SetImage(img[i]);
                if (!string.IsNullOrEmpty(s[i]))
                    System.Windows.Clipboard.SetText(s[i]);
                act(i, s[i]);
            }
            catch
            {

            }
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
