using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class ShellViewModel
    {
        [ServiceProperty(Key = "CustomNotificationService")]
        public virtual INotificationService NotificationService => null;
        [ServiceProperty(Key = "OpenFileService")]
        public virtual IOpenFileDialogService OpenFileService => null;
    }
}