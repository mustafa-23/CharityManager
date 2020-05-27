using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using CharityManager.UI.Models;
using DevExpress.Mvvm.POCO;
using System.Threading.Tasks;
using CharityManager.UI.CharityService;
using System.Linq;
using Araneo.Common;
using System.Threading;
using System.Collections.Generic;
using DevExpress.Mvvm.Native;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class LoginViewModel
    {
        public ObservableCollection<UserModel> Users { get; set; } = new ObservableCollection<UserModel>();
        public virtual UserModel SelectedUser { get; set; }
        public virtual string Status { get; set; }
        public virtual string ViewState { get; set; }

        public LoginViewModel()
        {
            if (POCOViewModelExtensions.IsInDesignMode(this))
                return;
            Task.Run(LoadUsers);
        }
        public void Login(string password)
        {
            if (password?.Length > 0 && SelectedUser.Password == Helper.HashPassword(password))
            {
                //    var request = new UserRequest { DTO = Mapper.Map(SelectedUser, new UserDTO()) };
                //    var response = Helper.Call(s => s.Login(request));
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
        private void LoadUsers()
        {
            Status = "در حال بارگزاری اطلاعات کاربران...";
            var request = new UserRequest { Filter = new UserFilter { Active = true } };
            var response = Helper.Call(s => s.UserGetList(request));
            if (response?.ResultList?.Length > 0)
            {
                var personReq = new PersonRequest { Filter = new PersonFilter { IDList = response.ResultList.Select(u => u.PersonID).ToArray() } };
                var personRes = Helper.Call(s => s.PersonGetList(personReq));
                if (personRes.Success)
                {
                    var tempList = response.ResultList.Select(dto => Mapper.SmartMap(dto, new UserModel(), (s, d) =>
                        {
                            d.Person = Mapper.Map(personRes.ResultList.FirstOrDefault(p => p.ID == d.PersonID), new PersonModel());
                        })).ToList();
                    AppUIManager.Application.Dispatcher.Invoke(() =>
                    {
                        AttachPictures(tempList);
                        Users.AddRange(tempList);
                        SelectedUser = Users.FirstOrDefault();
                    });
                }
            }
        }
        private void AttachPictures(List<UserModel> users)
        {
            var request = new PersonRequest { Filter = new PersonFilter { IDList = users.Select(s => s.PersonID).ToArray() } };
            var response = Helper.Call(s => s.PersonPictureGet(request));
            if (response?.Success ?? false)
            {
                foreach (var pic in response.ResultList)
                {
                    var person = users.FirstOrDefault(u => u.PersonID == pic.PersonID).Person;
                    if (person != null)
                        person.Image = pic.Data.ToBitmapImage();
                }
            }
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