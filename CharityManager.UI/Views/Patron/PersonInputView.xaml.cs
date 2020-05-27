using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CharityManager.UI.Views
{
    /// <summary>
    /// Interaction logic for PersonInputView.xaml
    /// </summary>
    public partial class PersonInputView : UserControl
    {
        public PersonInputView()
        {
            InitializeComponent();
        }

        private void tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (border == null)
                return;

            double offset = 0;
            for (int i = 0; i < tab.SelectedIndex; i++)
                offset += ((TabItem)tab.Items[i]).ActualWidth;

            var xAnimate = new DoubleAnimation
            {
                To = offset + 12,
                Duration = TimeSpan.FromMilliseconds(700),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
            };
            var widthAnimate = new DoubleAnimation
            {
                To = ((TabItem)tab.Items[tab.SelectedIndex]).ActualWidth - 24,
                Duration = xAnimate.Duration,
                EasingFunction = xAnimate.EasingFunction,
            };

            var story = new Storyboard();
            story.Children.Add(xAnimate);
            story.Children.Add(widthAnimate);

            Storyboard.SetTarget(xAnimate, border);
            Storyboard.SetTargetProperty(xAnimate, new System.Windows.PropertyPath("RenderTransform.(TranslateTransform.X)"));

            Storyboard.SetTarget(widthAnimate, border);
            Storyboard.SetTargetProperty(widthAnimate, new System.Windows.PropertyPath("Width"));

            story.Begin(this);
        }

        private void PlayAnimation()
        {

        }
    }
}
