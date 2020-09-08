using Araneo.Common;
using CharityManager.UI.Models;
using DevExpress.Mvvm;
using DevExpress.Mvvm.ModuleInjection;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.POCO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace CharityManager.UI
{
    public class AppUIManager : BindableBase
    {
        #region Constant
        public const string MENU_PERSON = "person";
        public const string MENU_INTRODUCER = "introducer";
        public const string MENU_REQUEST = "request";
        public const string MENU_REPORT = "report";
        public const string MENU_BACKUP = "backup";
        public const string MENU_SETTING = "setting";

        public const string SLIDER_OPEN = "Opened";
        public const string SLIDER_CLOSE = "Closed";

        public const string PROFILE_SHOW = "Show";
        public const string PROFILE_HIDE = "Hide";

        private static readonly string PROFILE = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\userprofile.txt";
        #endregion

        private readonly ICollectionView CollectionView;

        public static ObservableCollection<NotificationModel> Notifications { get; private set; } = new ObservableCollection<NotificationModel>();
        public static ObservableCollection<NoteModel> Notes { get; private set; } = new ObservableCollection<NoteModel>();
        public bool NewNotifications => Notifications.Any(n => n.Status == NotificationStatus.New);

        public static Application Application => Application.Current;
        public static IModuleManager Manager => ModuleManager.DefaultManager;

        private static AppUIManager _instance;
        public static AppUIManager Default => _instance ?? (_instance = new AppUIManager());

        public string SliderState { get { return GetProperty(() => SliderState); } set { SetProperty(() => SliderState, value); } }
        public string SideBarState { get { return GetProperty(() => SideBarState); } set { SetProperty(() => SideBarState, value); } }
        public string Menu { get { return GetProperty(() => Menu); } set { SetProperty(() => Menu, value, () => MenuChanged()); } }

        private void MenuChanged()
        {
            Manager.Clear(AppRegions.Main);
            switch (Menu?.ToLower())
            {
                case MENU_PERSON:
                    Manager.InjectOrNavigate(AppRegions.Main, AppModules.Person);
                    break;
                case MENU_INTRODUCER:
                    Manager.InjectOrNavigate(AppRegions.Main, AppModules.Introducer);
                    break;
                case MENU_REQUEST:
                    Manager.InjectOrNavigate(AppRegions.Main, AppModules.Request);
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
            LoadUserProfile();
            CollectionView = CollectionViewSource.GetDefaultView(Notifications);
            CollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(NotificationModel.CreateDate)));

            SliderState = SLIDER_CLOSE;
            SliderState = PROFILE_HIDE;
        }

        public static bool IsInDesignMode()
        {
            if (Application.MainWindow?.DataContext != null)
                return POCOViewModelExtensions.IsInDesignMode(Application.MainWindow.DataContext);
            return true;
        }

        public void AddNotification(string caption, string message, NotificationType type)
        {
            Notifications.Add(new NotificationModel(caption, message));
            RaisePropertyChanged(nameof(NewNotifications));
        }
        public void LoadUserProfile()
        {
            if (!File.Exists(PROFILE))
                return;

            var jo = JObject.Parse(File.ReadAllText(PROFILE));

            string temp = jo.Value<string>(nameof(Notifications));
            Notifications.AddRange(JsonConvert.DeserializeObject<NotificationModel[]>(temp));

            temp = jo.Value<string>(nameof(Notes));
            if(temp!=null)
            Notes.AddRange(JsonConvert.DeserializeObject<NoteModel[]>(temp));
        }
        public void SaveUserProfile()
        {
            string notes = JsonConvert.SerializeObject(Notes);
            JObject jo = new JObject
            {
                { nameof(Notifications), new JValue(JsonConvert.SerializeObject(Notifications)) },
                { nameof(Notes), new JValue(notes) },
            };
            File.WriteAllText(PROFILE, jo.ToString());
        }
        public void ReadAllMesasges()
        {
            Notifications.ForEach(s => s.Seen());
            RaisePropertyChanged(nameof(NewNotifications));
        }
        public void DeleteMessages() => Notifications.Clear();
    }
}
