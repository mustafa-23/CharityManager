using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using CharityManager.UI.Models;
using System.Windows.Input;
using System.Windows.Controls;
using System.Management.Instrumentation;
using CharityManager.UI.CharityService;
using Araneo.Common;
using System.Text.RegularExpressions;
using DevExpress.Data.Mask;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class QuicAddUserViewModel : ISupportParameter
    {
        #region Bindable Properties
        public virtual int Page { get; set; }
        public virtual PersonModel Person { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual string FormState { get; set; }
        #endregion
        public QuicAddUserViewModel()
        {
            Page = 0;
            Person = new PersonModel();
        }

        #region Commands
        public void Back() => AppUIManager.Manager.Remove(AppRegions.Shell, AppModules.QuickAddUser, true);
        public void SetPage(int page) => Page = page;
        public void Input(KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;
            if (e.Source is TextBox txt)
            {
                switch (txt.Name)
                {
                    case "txtNationalNo":
                        if (CheckNationalNo())
                        {
                            if (IsUnique())
                                Page = 1;
                            else
                                Page = 3;
                        }
                        break;
                    case "txtName":
                        Page = 2;
                        break;
                    case "txtFamily":
                        Page = 3;
                        break;
                    case "txtUserName":
                        if (CheckUserName())
                            Page = 4;
                        break;
                    case "txtPassword":
                        if (Password?.Length > 0)
                            CreateUser();
                        else
                            Helper.Notify("کلمه عبور باید بیش از 3 کاراکتر باشد");
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion

        #region Methods
        private void CreateUser()
        {
            if (Person.ID == 0)
                CreatePerson();
            var request = new UserRequest
            {
                DTO = new UserDTO
                {
                    PersonID = Person.ID,
                    UserName = UserName,
                    Password = Password,
                }
            };
            var response = Helper.Call(s => s.UserSet(request));
            ServiceResponseHelper.CheckServiceResponse(response, "UserSet", request);
            FormState = "Message";
            Listener?.OnUserAdded();
        }
        private void CreatePerson()
        {
            var request = new PersonRequest { DTO = Mapper.Map(Person, new PersonDTO()) };
            var response = Helper.Call(s => s.PersonSet(request));
            ServiceResponseHelper.CheckServiceResponse(response, "PersonSet", request);
            Person.ID = response.ResultID;
        }
        private bool IsUnique()
        {
            var request = new PersonRequest { Filter = new PersonFilter { NationalNo = Person.NationalNo } };
            var response = Helper.Call(s => s.PersonGet(request));
            if (response?.Success == true)
            {
                Mapper.Map(response.Result, Person);
                Helper.Notify($"{Person.Name} با شماره ملی {Person.NationalNo} یافت شد.");
                return false;
            }
            return true;
        }
        private bool CheckNationalNo()
        {
            if (Person.NationalNo?.Length == 10)
            {
                int sum = 0;
                for (int i = 0; i < 9; i++)
                    sum += (10 - i) * (Person.NationalNo[i] - '0');
                var q = sum % 11;

                q = (q >= 2) ? 11 - q : q;
                var valid = Person.NationalNo[9].ToString() == q.ToString();
                if (!valid)
                    Helper.Notify("شماره ملی وارد شده معتبر نیست");
                return valid;
            }
            Helper.NotifyCaution("کد ملی باید 10 رقم باشد");
            return false;

        }
        private bool CheckUserName()
        {
            if (UserName?.Length > 3)
            {
                Regex regUser = new Regex("[a-z0-9]+");
                var match = regUser.IsMatch(UserName);
                if (!match)
                    Helper.NotifyCaution("نام کاربری باید فقط شامل حروف انگلیسی کوچک و عدد باشد");
                return match;
            }
            else
                Helper.NotifyCaution("نام کاربری باید حداقل 3 حرف داشته باشد");
            return false;
        }
        #endregion

        #region ISupportParameters
        public virtual object Parameter { get; set; }
        public IQuickAddUserListener Listener => Parameter as IQuickAddUserListener;
        #endregion

        #region Interfaces
        public interface IQuickAddUserListener
        {
            void OnUserAdded();
        }
        #endregion    
    }
}