using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class MainViewModel
    {
        public string Test { get; set; } = nameof(MainViewModel);
        public MainViewModel()
        {
            if (POCOViewModelExtensions.IsInDesignMode(this))
                return;
            AppUIManager.Application.MainWindow.WindowState = System.Windows.WindowState.Maximized;
        }
        public void Normal()
        {
            Helper.Notify("امیدوارم روز خوبی داشته باشید","صبح بخیر کاربر گرامی");
        }
        public void Caution()
        {
            Helper.NotifyCaution("امیدوارم روز خوبی داشته باشید","صبح بخیر کاربر گرامی");
        }
        public void Warning()
        {
            Helper.NotifyWarning("امیدوارم روز خوبی داشته باشید","صبح بخیر کاربر گرامی");
        }
        public void Error()
        {
            Helper.NotifyError("امیدوارم روز خوبی داشته باشید","صبح بخیر کاربر گرامی");
        }
        public void Success()
        {
            Helper.NotifySuccess("امیدوارم روز خوبی داشته باشید","صبح بخیر کاربر گرامی");
        }
    }
}