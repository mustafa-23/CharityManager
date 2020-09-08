using Araneo.Common;
using CharityManager.UI.CharityService;
using CharityManager.UI.Models;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Core.Native;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class LoginViewModel
    {
        public ObservableCollection<UserModel> Users { get; set; } = new ObservableCollection<UserModel>();
        public virtual UserModel SelectedUser { get; set; }
        public virtual string Status { get; set; }
        public virtual string ViewState { get; set; }
        public virtual DateTime? LastLogin { get; set; }

        public LoginViewModel()
        {
            if (POCOViewModelExtensions.IsInDesignMode(this))
                return;
            GlobalVar.Users.CollectionChanged += Users_CollectionChanged;
            SelectedUser = GlobalVar.Users.FirstOrDefault();
        }

        private void Users_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SelectedUser = GlobalVar.Users.FirstOrDefault();
        }
        protected void OnSelectedUserChanged()
        {
            var request = new UserRequest { Filter = new UserFilter { ID = SelectedUser.ID } };
            var response = Helper.Call(s => s.GetUserLogins(request));
            ServiceResponseHelper.CheckServiceResponse(response,"GetUserLogins",request);
            LastLogin = response.ResultList.LastOrDefault()?.CreateDate;
        }
        public void Login(string password)
        {
            if (password?.Length > 0 && SelectedUser.Password == Helper.HashPassword(password))
            {
                var request = new UserRequest { DTO = Mapper.Map(SelectedUser, new UserDTO()) };
                var response = Helper.Call(s => s.Login(request));
                GlobalVar.SetUser(SelectedUser);
                ViewState = "Welcome";
                Task.Run(InjectMainModule);
            }
            else
                Helper.NotifyWarning("کلمه عبور اشتباه است");
        }
        private void InjectMainModule()
        {
            AppUIManager.Application.Dispatcher.Invoke(() =>
            {
                AppUIManager.Manager.Remove(AppRegions.Shell, AppModules.Splash);
                AppUIManager.Manager.Inject(AppRegions.Shell, AppModules.Main);
                WelcomeMessage();
            });
        }

        private void WelcomeMessage()
        {
            string time = "وقت";
            switch (DateTime.Now.Hour)
            {
                case int m when m >= 0 && m < 11:
                    time = "صبح";
                    break;
                case int n when n >= 11 && n < 13:
                    time = "ظهر";
                    break;
                case int n when n >= 13 && n < 16:
                    time = "بعد از ظهر";
                    break;
                case int n when n >= 16 && n < 19:
                    time = "عصر";
                    break;
            }
            var msg = $"{time} بخیر کاربر گرامی";
            Helper.Notify("امیدوارم روز کاری خوبی داشته باشید", msg);
        }
    }
}