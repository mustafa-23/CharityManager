using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Threading;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class CustomNotificationViewModel
    {
        private readonly DispatcherTimer timer;
        public enum NotificationType { Normal, Caution, Warning, Error, Success }
        private readonly Dictionary<NotificationType, Color> Foregrounds = new Dictionary<NotificationType, Color> {
            { NotificationType.Normal, Color.FromRgb(0, 92, 128) },
            { NotificationType.Success, Color.FromRgb(0, 128, 96) },
            { NotificationType.Caution, Color.FromRgb(128, 117, 0) },
            { NotificationType.Error, Color.FromRgb(255, 255, 255) }
        };
        private readonly Dictionary<NotificationType, string> Images = new Dictionary<NotificationType, string> {
            { NotificationType.Normal, "info.png" },
            { NotificationType.Success,"success.png" },
            { NotificationType.Caution, "warn.png" },
            { NotificationType.Error, "warn.png" }
        };
        public virtual string Caption { get; set; }
        public virtual string Content { get; set; }
        public virtual NotificationType Type { get; set; }
        public virtual double Time { get; set; }
        public virtual Brush Background => (SolidColorBrush)AppUIManager.Application.TryFindResource($"Brushes.{Type}");
        public virtual Brush TitleFore => Background.SetBrightness(-0.5);
        public virtual Brush Foreground => Background.SetBrightness(-0.5);
        public virtual Uri Image => new Uri($"../Images/{Images[Type]}",UriKind.Relative);
        public CustomNotificationViewModel()
        {
            timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (Time < 100)
                Time += 1;
            else
                timer.Stop();
        }
    }
}
