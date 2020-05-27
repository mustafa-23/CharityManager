using DevExpress.Mvvm.ModuleInjection;
using DevExpress.Mvvm.POCO;
using System.Diagnostics;
using System.Globalization;
using System.Media;

namespace CharityManager.UI
{
    public class Bootstrapper
    {
        public Bootstrapper()
        {
            AppUIManager.Application.DispatcherUnhandledException += Current_DispatcherUnhandledException;
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            SystemSounds.Beep.Play();
            Debug.WriteLine(e.Exception.Message);
        }

        private void RegisterModules()
        {
            // Shell
            AppUIManager.Manager.Register(AppRegions.Shell, new Module(AppModules.Splash,
                () => ViewModelSource.Create<ViewModels.SplashViewModel>(), typeof(Views.SplashView)));
            AppUIManager.Manager.Register(AppRegions.Shell, new Module(AppModules.Licence,
                () => ViewModelSource.Create<ViewModels.LicenceViewModel>(), typeof(Views.LicenceView)));
            AppUIManager.Manager.Register(AppRegions.Shell, new Module(AppModules.Login,
                () => ViewModelSource.Create<ViewModels.LoginViewModel>(), typeof(Views.LoginView)));
            AppUIManager.Manager.Register(AppRegions.Shell, new Module(AppModules.Main,
                () => ViewModelSource.Create<ViewModels.MainViewModel>(), typeof(Views.MainView)));

            // Main
            AppUIManager.Manager.Register(AppRegions.Main, new Module(AppModules.Person,
                () => ViewModelSource.Create<ViewModels.PersonViewModel>(), typeof(Views.PersonView)));
            AppUIManager.Manager.Register(AppRegions.Main, new Module(AppModules.PersonGlance,
                () => ViewModelSource.Create<ViewModels.PersonViewModel>(), typeof(Views.PersonView)));
            // Person
            AppUIManager.Manager.Register(AppRegions.Person, new Module(AppModules.PersonInput,
                () => ViewModelSource.Create<ViewModels.PersonInputViewModel>(), typeof(Views.PersonInputView)));
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
        }
        private void InjectModules()
        {
#if !DEBUG
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
            RegisterModules();
            InjectModules();
            CreateShell();
            AppUIManager.Application.MainWindow.Show();
        }
    }
}
