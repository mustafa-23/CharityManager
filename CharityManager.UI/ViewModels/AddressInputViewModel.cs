using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using CharityManager.UI.Models;
using CharityManager.UI.CharityService;
using Araneo.Common;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class AddressInputViewModel : ISupportParameter
    {
        public virtual AddressModel Model { get; set; }

        #region Commands
        public void Confirm()
        {
            var request = new AddressRequest { DTO = Mapper.Map(Model, new AddressDTO()) };
            var response = Helper.Call(s => s.AddressSet(request));
            ServiceResponseHelper.CheckServiceResponse(response, "AddressSet", request);
            Messenger.Default.Send(Messages.Address.Refresh);
            SliderHelper.Close();
        } 
        #endregion

        #region ISupportParameter
        public virtual object Parameter { get; set; }
        protected void OnParameterChanged()
        {
            if (Parameter is int patronID)
                Model = new AddressModel { PatronID = patronID };
            else if (Parameter is AddressModel model)
                Model = model;
        }
        #endregion
    }
}