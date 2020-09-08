using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CharityManager.UI.Views
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        Shell Shell => Application.Current.MainWindow as Shell;

        public MenuView()
        {
            InitializeComponent();
        }

        private void tgbMenu_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (Shell.WindowState == WindowState.Maximized)
                {
                    double percentVertical = e.GetPosition(null).Y / Shell.ActualHeight;
                    double targetVertical = Shell.RestoreBounds.Height * percentVertical;

                    var currentMousePoint = e.GetPosition(null);


                    Shell.WindowState = WindowState.Normal;

                    Shell.Left = currentMousePoint.X - Shell.RestoreBounds.Width;
                    Shell.Top = currentMousePoint.Y - targetVertical;
                }
                else
                    Shell.DragMove();
            }
        }

        #region Command Methods
        public void Click()
        {
            SwitchState();
        }
        public void Minimize()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
        #endregion

        private void SwitchState() => Shell.WindowState = (Shell.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
                SwitchState();
        }
    }
}
