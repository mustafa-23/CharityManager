using Araneo.Common;
using CharityManager.UI.CharityService;
using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class SplashViewModel : LicenceViewModel.ILicenceActivationListener, QuicAddUserViewModel.IQuickAddUserListener
    {
        public virtual string FormEffect { get; set; } = "";
        public virtual string FormState { get; set; } = "";
        public SplashViewModel()
        {
            Task.Run(Load);
        }
        public void Load()
        {
            try
            {
                Thread.Sleep(5000);
                if (Araneo.Common.Security.LicenceHelper.CheckLicence(ApplicationInfo.AppName) == LicenceStatus.OK)
                    ShowLogin();
                else
                {
                    AppUIManager.Application.Dispatcher.Invoke(() =>
                    {
                        FormEffect = "Blured";
                        AppUIManager.Manager.Inject(AppRegions.Shell, AppModules.Licence, this);
                    });
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
            }
        }
        private void ShowLogin()
        {
            FormEffect = "Normal";
            AppUIManager.Application.Dispatcher.Invoke(ClearSplashScreen);
            GlobalVar.RefreshUsers();
            if (GlobalVar.Users?.Count > 0)
                FormState = "Login";
            else
                FormState = "AddUser";
        }
        public void OnActivation() => ShowLogin();
        public void AddUser()
        {
            FormEffect = "Blured";
            AppUIManager.Manager.Inject(AppRegions.Shell, AppModules.QuickAddUser, this);
        }

        public void OnUserAdded()
        {
            Task.Run(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
                AppUIManager.Application.Dispatcher.Invoke(ShowLogin);
            });
        }

        private void ClearSplashScreen()
        {
            AppUIManager.Manager.Remove(AppRegions.Shell, AppModules.Licence);
            AppUIManager.Manager.Remove(AppRegions.Shell, AppModules.QuickAddUser);
        }
    }
}