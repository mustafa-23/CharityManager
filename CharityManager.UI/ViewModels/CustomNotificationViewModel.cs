using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Threading;
using Models = CharityManager.UI.Models;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class CustomNotificationViewModel
    {
        private readonly DispatcherTimer timer;
        private readonly Dictionary<Models.NotificationType, string> Images = new Dictionary<Models.NotificationType, string> {
            { Models.NotificationType.Normal, "info.png" },
            { Models.NotificationType.Success,"success.png" },
            { Models.NotificationType.Warning, "warn.png" },
            { Models.NotificationType.Caution, "warn.png" },
            { Models.NotificationType.Error, "warn.png" }
        };
        public virtual string Caption { get; set; }
        public virtual string Content { get; set; }
        public virtual Models.NotificationType Type { get; set; }
        public virtual double Time { get; set; }
        public virtual Brush Background => (SolidColorBrush)AppUIManager.Application.TryFindResource($"Brushes.{Type}");
        public virtual Brush TitleFore => Background.SetBrightness(-0.5);
        public virtual Brush Foreground => Background.SetBrightness(-0.3);
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
