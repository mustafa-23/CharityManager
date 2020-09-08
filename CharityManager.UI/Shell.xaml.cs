using System.Windows;

namespace CharityManager.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Shell : Window
    {
        public Shell()
        {
            InitializeComponent();
            Loaded += Shell_Loaded;
        }

        private void Shell_Loaded(object sender, RoutedEventArgs e)
        {
            if (!mokeev1995.BlurLibrary.BlurWindow.Enabled)
                mokeev1995.BlurLibrary.BlurWindow.EnableWindowBlur(this);
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }
    }
}
