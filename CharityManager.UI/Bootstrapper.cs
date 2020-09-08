using DevExpress.Mvvm.ModuleInjection;
using DevExpress.Mvvm.POCO;
using System.Globalization;

namespace CharityManager.UI
{
    public class Bootstrapper
    {
        private void RegisterModules()
        {
            // Shell
            AppUIManager.Manager.Register(AppRegions.Shell, new Module(AppModules.Splash,
                () => ViewModelSource.Create<ViewModels.SplashViewModel>(), typeof(Views.SplashView)));
            AppUIManager.Manager.Register(AppRegions.Shell, new Module(AppModules.Licence,
                () => ViewModelSource.Create<ViewModels.LicenceViewModel>(), typeof(Views.LicenceView)));
            AppUIManager.Manager.Register(AppRegions.Shell, new Module(AppModules.QuickAddUser,
                () => ViewModelSource.Create<ViewModels.QuicAddUserViewModel>(), typeof(Views.QuicAddUserView)));
            AppUIManager.Manager.Register(AppRegions.Shell, new Module(AppModules.Main,
                () => ViewModelSource.Create<ViewModels.MainViewModel>(), typeof(Views.MainView)));

            // Main
            AppUIManager.Manager.Register(AppRegions.Main, new Module(AppModules.Person,
                () => ViewModelSource.Create<ViewModels.PersonViewModel>(), typeof(Views.PersonView)));
            AppUIManager.Manager.Register(AppRegions.Main, new Module(AppModules.Introducer,
                () => ViewModelSource.Create<ViewModels.IntroducerViewModel>(), typeof(Views.IntroducerView)));
            AppUIManager.Manager.Register(AppRegions.Main, new Module(AppModules.Request,
                () => ViewModelSource.Create<ViewModels.RequestViewModel>(), typeof(Views.RequestView)));
            // Person
            AppUIManager.Manager.Register(AppRegions.Person, new Module(AppModules.PersonInput,
                () => ViewModelSource.Create<ViewModels.PersonInputViewModel>(), typeof(Views.PersonInputView)));
            // Introducer
            AppUIManager.Manager.Register(AppRegions.Introducer, new Module(AppModules.IntroducerInput,
                () => ViewModelSource.Create<ViewModels.IntroducerInputViewModel>(), typeof(Views.IntroducerInputView)));
            // Request
            AppUIManager.Manager.Register(AppRegions.Request, new Module(AppModules.RequestInput,
                () => ViewModelSource.Create<ViewModels.RequestInputViewModel>(), typeof(Views.RequestInputView)));

            // Slider
            AppUIManager.Manager.Register(AppRegions.Slider, new Module(AppModules.AddressInput,
                () => ViewModelSource.Create<ViewModels.AddressInputViewModel>(), typeof(Views.AddressInputView)));
            AppUIManager.Manager.Register(AppRegions.Slider, new Module(AppModules.FamilyInput,
                () => ViewModelSource.Create<ViewModels.FamilyInputViewModel>(), typeof(Views.FamilyInputView)));
            AppUIManager.Manager.Register(AppRegions.Slider, new Module(AppModules.JobInput,
                () => ViewModelSource.Create<ViewModels.JobInputViewModel>(), typeof(Views.JobInputView)));
            AppUIManager.Manager.Register(AppRegions.Slider, new Module(AppModules.AssetInput,
                () => ViewModelSource.Create<ViewModels.AssetsInputViewModel>(), typeof(Views.AssetsInputView)));
            AppUIManager.Manager.Register(AppRegions.Slider, new Module(AppModules.DocumentInput,
                () => ViewModelSource.Create<ViewModels.DocumentInputViewModel>(), typeof(Views.DocumentInputView)));
            AppUIManager.Manager.Register(AppRegions.Slider, new Module(AppModules.SelectPatron,
                () => ViewModelSource.Create<ViewModels.SelectPatronViewModel>(), typeof(Views.SelectPatronView)));
            AppUIManager.Manager.Register(AppRegions.Slider, new Module(AppModules.ResearchInput,
                () => ViewModelSource.Create<ViewModels.ResearchInputViewModel>(), typeof(Views.ResearchInputView)));
            AppUIManager.Manager.Register(AppRegions.Slider, new Module(AppModules.ManagerViewPointInput,
                () => ViewModelSource.Create<ViewModels.ManagerViewPointInputViewModel>(), typeof(Views.ManagerViewPointInputView)));
            // Sidebar
            AppUIManager.Manager.Register(AppRegions.SideBar, new Module(AppModules.UserProfile,
                () => ViewModelSource.Create<ViewModels.UserProfileViewModel>(), typeof(Views.UserProfileView)));
            AppUIManager.Manager.Register(AppRegions.SideBar, new Module(AppModules.NotificationCenter,
                () => ViewModelSource.Create<ViewModels.NotificationCenterViewModel>(), typeof(Views.NotificationCenterView)));
            AppUIManager.Manager.Register(AppRegions.SideBar, new Module(AppModules.Notes,
                () => ViewModelSource.Create<ViewModels.NotesViewModel>(), typeof(Views.NotesView)));
        }
        private void InjectModules()
        {
#if DEBUG
            AppUIManager.Manager.Inject(AppRegions.Shell, AppModules.Main);
#else
            AppUIManager.Manager.Inject(AppRegions.Shell, AppModules.Splash);
#endif
        }
        private void CreateShell()
        {
            AppUIManager.Application.MainWindow = new Shell();
        }
        public void Run()
        {
            CultureInfo.CurrentCulture = new CultureInfo("fa-IR");
            AppConfigs.Update();
            RegisterModules();
            InjectModules();
            CreateShell();
            AppUIManager.Application.MainWindow.Show();
        }
    }
}
