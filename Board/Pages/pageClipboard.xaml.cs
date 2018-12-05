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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Board
{
    public partial class pageClipboard : Page
    {
        public pageClipboard()
        {
            InitializeComponent();

            lvClipboard.ItemsSource = App.clipboardHistory;
        }

        private void lvClipboard_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvClipboard.SelectedValue != null)
            {
                string clipboard = (string)lvClipboard.SelectedValue;
                App.IsCopying = true;
                Clipboard.SetText(clipboard);
            }
        }

        private void lvClipboard_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Delete)
            {
                if (lvClipboard.SelectedIndex != -1)
                {
                    App.clipboardHistory.RemoveAt(lvClipboard.SelectedIndex);
                }
            }
        }
    }
}
