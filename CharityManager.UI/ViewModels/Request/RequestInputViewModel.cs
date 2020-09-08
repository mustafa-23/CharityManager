using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using CharityManager.UI.Models;
using System.Windows.Media.Imaging;
using System.Linq;
using CharityManager.UI.CharityService;
using Araneo.Common;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class RequestInputViewModel : SelectPatronViewModel.ISelectPatronListener
    {
        public virtual PatronModel Patron { get; set; }
        public virtual BitmapImage PersonImage { get; set; }
        public virtual RequestModel Model { get; set; }

        public RequestInputViewModel()
        {
            Model = new RequestModel
            {
                TypeEntityID = AppConfigs.RequestTypes.FirstOrDefault()?.ID ?? 0,
                IssueDate = DateTime.Now,
            };
            var response = Helper.Call(s => s.RequestLastNo());
            ServiceResponseHelper.CheckServiceResponse(response, "RequestLastNo", null);
            Model.No = (response.MaxNo + 1).ToString();
        }

        #region Commands
        public void Apply()
        {
            Model.PatronID = Patron.ID;
            var request = new RequestRequest { UserID = GlobalVar.UserID, DTO = Mapper.Map(Model, new RequestDTO()) };
            var response = Helper.Call(s => s.RequestSet(request));
            ServiceResponseHelper.CheckServiceResponse(response, "RequestSet", request);
            Helper.NotifySuccess("درخواست با موفقیت ثبت شد");
        }
        public bool CanApply() => Patron != null;
        public void Back() => AppUIManager.Manager.Clear(AppRegions.Request);
        #endregion

        protected void OnPatronChanged()
        {
            PersonImage = null;
            if (Patron != null)
                PersonImage = QuickServiceCall.LoadPersonImage(Patron.PersonID);
        }

        #region SelectPatronViewModel.ISelectPatronListener
        public void OnSelect(PatronModel patron)
        {
            Patron = patron;
        }
        #endregion
    }
}