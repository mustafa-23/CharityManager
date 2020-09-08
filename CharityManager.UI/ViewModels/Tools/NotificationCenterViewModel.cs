using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.POCO;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class NotificationCenterViewModel
    {
        public void Seen() => AppUIManager.Default.ReadAllMesasges();
        public void Delete() => AppUIManager.Default.DeleteMessages();
    }
}