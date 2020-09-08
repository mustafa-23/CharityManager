using System.Windows;

namespace CharityManager.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Startup += App_Startup;
            Exit += App_Exit;
            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            new Bootstrapper().Run();
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            AppUIManager.Default.SaveUserProfile();
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (e.Exception is CallServiceException cse)
                Helper.NotifyError(cse.UserMessage, "خطای سرویس");
            else
                Helper.NotifyError(e.Exception.Message,"خطای نامشخص");
            e.Handled = true;
        }
    }
}
