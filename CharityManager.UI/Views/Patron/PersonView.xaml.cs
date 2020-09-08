using DevExpress.Mvvm.ModuleInjection;
using System.Windows.Controls;

namespace CharityManager.UI.Views
{
    /// <summary>
    /// Interaction logic for PersonView.xaml
    /// </summary>
    public partial class PersonView : UserControl
    {
        public PersonView()
        {
            InitializeComponent();
            ModuleManager.DefaultManager.GetEvents(AppRegions.Person).ViewModelCreated += PersonView_ViewModelCreated;
            ModuleManager.DefaultManager.GetEvents(AppRegions.Person).ViewModelRemoved += PersonView_ViewModelRemoved;
        }
        private void PersonView_ViewModelRemoved(object sender, ViewModelRemovedEventArgs e)
        {
            glanceView.Visibility = System.Windows.Visibility.Visible;
        }

        private void PersonView_ViewModelCreated(object sender, ViewModelCreatedEventArgs e)
        {
            glanceView.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
