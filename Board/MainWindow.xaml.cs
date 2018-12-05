using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Board
{
    public partial class MainWindow : Window
    {
        private HwndSource hWndSource;
        private IntPtr hWndNextViewer;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            hWndSource = PresentationSource.FromVisual(this) as HwndSource;
        }

        private IntPtr WinProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
           

            switch (msg)
            {
                case Win32.WM_CHANGECBCHAIN:
                    if (wParam == hWndNextViewer)
                    {
                        // clipboard viewer chain changed, need to fix it. 
                        hWndNextViewer = lParam;
                    }
                    else if (hWndNextViewer != IntPtr.Zero)
                    {
                        // pass the message to the next viewer. 
                        Win32.SendMessage(hWndNextViewer, msg, wParam, lParam);
                    }
                    break;

                case Win32.WM_DRAWCLIPBOARD:
                    // clipboard content changed 
                    if (Clipboard.ContainsText() && !App.IsCopying)
                    {
                        App.AddToClipboardHistory(Clipboard.GetText());
                    }
                    // pass the message to the next viewer. 
                    Win32.SendMessage(hWndNextViewer, msg, wParam, lParam);
                    break;
            }

            return IntPtr.Zero;
        }

        private void Expand()
        {
            btnShowCB.Visibility = Visibility.Visible;
            btnSettings.Visibility = Visibility.Visible;
            btnRecord.Visibility = Visibility.Visible;
            btnExit.Visibility = Visibility.Visible;
            scbWindowBackground.Opacity = 0.4;
            App.IsExpanded = true;
        }

        private void Collapse()
        {
            btnShowCB.Visibility = Visibility.Collapsed;
            btnSettings.Visibility = Visibility.Collapsed;
            btnRecord.Visibility = Visibility.Collapsed;
            btnExit.Visibility = Visibility.Collapsed;
            scbWindowBackground.Opacity = 0;
            App.IsExpanded = false;
        }

        private void ExpandFrame()
        {
            frameMain.Visibility = Visibility.Visible;
            App.IsFrameExpanded = true;
        }

        private void CollapseFrame(bool setCollapsed = true)
        {
            frameMain.Visibility = Visibility.Collapsed;
            if (setCollapsed) { App.IsFrameExpanded = false; }
        }

        private void StartRecording()
        {
            hWndSource.AddHook(WinProc);
            hWndNextViewer = Win32.SetClipboardViewer(hWndSource.Handle);   // set this window as a viewer 
            App.IsRecording = true;
            btnRecord.Content = "\xE73B";
        }

        private void StopRecording()
        {
            Win32.ChangeClipboardChain(hWndSource.Handle, hWndNextViewer);

            hWndNextViewer = IntPtr.Zero;
            hWndSource.RemoveHook(WinProc);
            App.IsRecording = false;
            btnRecord.Content = "\xE7C8";
        }

        private void btnShowCB_Click(object sender, RoutedEventArgs e)
        {
            if (!App.IsFrameExpanded)
            {
                if (App.pageClipboard == null) { App.pageClipboard = new pageClipboard(); }
                frameMain.Navigate(App.pageClipboard);
                ExpandFrame();
            }
            else
            {
                CollapseFrame();
            }
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            if (!App.IsFrameExpanded)
            {
                if (App.pageSettings == null) { App.pageSettings = new pageSettings(); }
                frameMain.Navigate(App.pageSettings);
                ExpandFrame();
            }
            else
            {
                CollapseFrame();
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnMain_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) { DragMove(); }
        }

        private void btnMain_MouseEnter(object sender, MouseEventArgs e)
        {
            Expand();
            if (App.IsFrameExpanded) { ExpandFrame(); }
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            Collapse();
            CollapseFrame(false);
        }

        private void btnRecord_Click(object sender, RoutedEventArgs e)
        {
            if (!App.IsRecording)
            {
                StartRecording();
            }
            else
            {
                StopRecording();
            }
        }
    }
}
