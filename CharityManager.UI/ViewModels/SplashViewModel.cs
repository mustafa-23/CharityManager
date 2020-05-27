using Araneo.Common;
using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class SplashViewModel : LicenceViewModel.ILicenceActivationListener
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
            AppUIManager.Manager.Remove(AppRegions.Shell,AppModules.Licence);
            FormEffect = "Normal";
            FormState = "Login";
        }
        public void OnActivation() => ShowLogin();
    }
}