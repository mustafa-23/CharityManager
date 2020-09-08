using Araneo.Common;
using CharityManager.UI.CharityService;
using CharityManager.UI.Models;
using DevExpress.Mvvm.Native;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CharityManager.UI
{
    static class GlobalVar
    {
        static GlobalVar()
        {
#if DEBUG
            var response = Helper.Call(s => s.UserGetList(new UserRequest { Filter = new UserFilter() }));
            User = Mapper.Map(response?.ResultList?.FirstOrDefault(), new UserModel());
#endif
        }
        public static UserModel User { get; private set; }
        public static int UserID => User.ID;

        public static void SetUser(UserModel user) => User = user;
        public static ObservableCollection<UserModel> Users { get; private set; } = new ObservableCollection<UserModel>();

        public static void RefreshUsers()
        {
            AppUIManager.Application.Dispatcher.Invoke(() => Users.Clear());
            var request = new UserRequest { Filter = new UserFilter { Active = true } };
            var response = Helper.Call(s => s.UserGetList(request));
            var temp = response?.ResultList.Select(dto => Mapper.Map(dto, new UserModel())).ToList();

            if (temp?.Count() > 0)
            {
                var personReq = new PersonRequest { Filter = new PersonFilter { IDList = temp.Select(u => u.PersonID).ToArray() } };
                var personRes = Helper.Call(s => s.PersonGetList(personReq));
                if (personRes.Success)
                {
                    temp.ForEach(user => user.Person = Mapper.Map(personRes.ResultList.FirstOrDefault(p => p.ID == user.PersonID), new PersonModel()));
                    AppUIManager.Application.Dispatcher.Invoke(() =>
                    {
                        AttachPictures(temp);
                        Users.AddRange(temp);
                    });
                }
            }
        }
        private static void AttachPictures(IEnumerable<UserModel> users)
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
    }
}
