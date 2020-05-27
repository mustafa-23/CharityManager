using System.Windows.Controls;

namespace CharityManager.UI.Views
{
    /// <summary>
    /// Interaction logic for JobListView.xaml
    /// </summary>
    public partial class JobListView : UserControl
    {
        private bool showActionPanel;
        public bool ShowActionPanel
        {
            get { return showActionPanel; }
            set
            {
                showActionPanel = value;
                ShowActionPanelChanged();
            }
        }

        private void ShowActionPanelChanged()
        {
            actionPanel.Visibility = ShowActionPanel ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }
        public JobListView()
        {
            InitializeComponent();
        }
    }
}
