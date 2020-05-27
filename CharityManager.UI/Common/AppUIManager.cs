using DevExpress.Mvvm;
using DevExpress.Mvvm.ModuleInjection;
using DevExpress.Mvvm.POCO;
using System;
using System.Windows;

namespace CharityManager.UI
{
    public class AppUIManager : BindableBase
    {
        #region Constant
        public const string MENU_PERSON = "person";
        public const string MENU_REQUEST = "request";
        public const string MENU_REPORT = "report";
        public const string MENU_BACKUP = "backup";
        public const string MENU_SETTING = "setting";

        public const string SLIDER_OPEN = "Opened";
        public const string SLIDER_CLOSE = "Closed";

        public const string PROFILE_SHOW = "Show";
        public const string PROFILE_HIDE = "Hide";
        #endregion

        public static Application Application => Application.Current;
        public static IModuleManager Manager => ModuleManager.DefaultManager;

        private static AppUIManager _instance;
        public static AppUIManager Default => _instance ?? (_instance = new AppUIManager());

        public string SliderState { get { return GetProperty(() => SliderState); } set { SetProperty(() => SliderState, value); } }
        public string ProfileState { get { return GetProperty(() => ProfileState); } set { SetProperty(() => ProfileState, value); } }
        public string Menu { get { return GetProperty(() => Menu); } set { SetProperty(() => Menu, value, () => MenuChanged()); } }

        private void MenuChanged()
        {
            switch (Menu?.ToLower())
            {
                case MENU_PERSON:
                    Manager.InjectOrNavigate(AppRegions.Main, AppModules.Person);
                    break;
                case MENU_REQUEST:
                    break;
                case MENU_REPORT:
                    break;
                case MENU_BACKUP:
                    break;
                case MENU_SETTING:
                    break;
                default:
                    break;
            }
        }

        private AppUIManager()
        {
            SliderState = SLIDER_CLOSE;
            SliderState = PROFILE_HIDE;
        }

        public static bool IsInDesignMode()
        {
            if (Application.MainWindow?.DataContext != null)
                return POCOViewModelExtensions.IsInDesignMode(Application.MainWindow.DataContext);
            return true;
        }

    }
}
