using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace Board
{
    public partial class App : Application
    {
        public static bool IsExpanded = false;
        public static bool IsRecording = false;
        public static bool IsFrameExpanded = false;
        public static bool IsCopying = false;
        public static pageClipboard pageClipboard;
        public static pageSettings pageSettings;

        public static ObservableCollection<string> clipboardHistory;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            clipboardHistory = new ObservableCollection<string>();
        }

        public static void AddToClipboardHistory(string text)
        {
            if (clipboardHistory.Count == 32)
            {
                clipboardHistory.RemoveAt(31);
            }

            clipboardHistory.Insert(0, text);
        }
    }

    internal static class Win32
    {
        internal const int WM_DRAWCLIPBOARD = 0x0308;

        internal const int WM_CHANGECBCHAIN = 0x030D;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
    }
}
