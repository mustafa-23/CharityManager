using DevExpress.Mvvm;
using DevExpress.Mvvm.ModuleInjection;
using System.Windows.Input;

namespace CharityManager.UI
{
    public static class Commands
    {
        public static ICommand Shutdown => new DelegateCommand(() => AppUIManager.Application.Shutdown());
        public static ICommand CloseDialog => new DelegateCommand(DialogHelper.Close);
        public static ICommand CloseSlider => new DelegateCommand(SliderHelper.Close);
        public static ICommand Logout => new DelegateCommand(() =>
        {
            AppUIManager.Manager.Clear(AppRegions.Shell);
            AppUIManager.Manager.Inject(AppRegions.Shell, AppModules.Splash);
            AppUIManager.Application.MainWindow.WindowState = System.Windows.WindowState.Normal;
        });
        public static ICommand ShowProfile => new DelegateCommand(() => AppUIManager.Default.ProfileState = AppUIManager.PROFILE_SHOW);
        public static ICommand HideProfile => new DelegateCommand(() => AppUIManager.Default.ProfileState = AppUIManager.PROFILE_HIDE);
        public static ICommand EditPerson => new DelegateCommand<object>(obj =>
        {
            AppUIManager.Manager.InjectOrNavigate(AppRegions.Person, AppModules.PersonInput, obj);
        });
    }
}
