using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace CharityManager.UI.Controls
{
    [TemplatePart(Name = "PART_Indicator", Type = typeof(Border))]
    public class AnimatedTabControl : TabControl
    {
        public double InitialIndicatorWidth { get; set; }
        private Border Indicator { get; set; }

        public Thickness IndicatorMargin { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Indicator = GetTemplateChild("PART_Indicator") as Border;
        }
        public AnimatedTabControl()
        {
            SelectionChanged += Tab_SelectionChanged;
        }
        private void Tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Indicator == null || Items.Count == 0)
                return;


            double offset = 0;
            for (int i = 0; i < SelectedIndex; i++)
                offset += ((TabItem)Items[i]).ActualWidth;

            var xAnimate = new DoubleAnimation
            {
                To = offset + 12,
                Duration = TimeSpan.FromMilliseconds(700),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
            };

            var width = (Items[SelectedIndex] as TabItem).ActualWidth;
            width = width > 0 ? width : InitialIndicatorWidth;

            var widthAnimate = new DoubleAnimation
            {
                To = width - 24,
                Duration = xAnimate.Duration,
                EasingFunction = xAnimate.EasingFunction,
            };

            var story = new Storyboard();
            story.Children.Add(xAnimate);
            story.Children.Add(widthAnimate);

            Storyboard.SetTarget(xAnimate, Indicator);
            Storyboard.SetTargetProperty(xAnimate, new PropertyPath("RenderTransform.(TranslateTransform.X)"));

            Storyboard.SetTarget(widthAnimate, Indicator);
            Storyboard.SetTargetProperty(widthAnimate, new PropertyPath("Width"));

            story.Begin(this);
        }
        ~AnimatedTabControl()
        {
            SelectionChanged -= Tab_SelectionChanged;
        }
    }
}
