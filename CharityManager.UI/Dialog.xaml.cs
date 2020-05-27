using System;
using System.Windows;

namespace CharityManager.UI
{
    /// <summary>
    /// Interaction logic for Dialog.xaml
    /// </summary>
    public partial class Dialog : Window
    {
        public Dialog()
        {
            InitializeComponent();
        }

        private void Dialog_Loaded(object sender, RoutedEventArgs e)
        {
            mokeev1995.BlurLibrary.BlurWindow.EnableWindowBlur(this);
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            WindowState = WindowState.Maximized;
        }
    }
}
