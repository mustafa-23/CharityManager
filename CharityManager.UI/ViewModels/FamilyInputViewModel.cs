using Araneo.Common;
using CharityManager.UI.CharityService;
using CharityManager.UI.Models;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class FamilyInputViewModel : ISupportParameter
    {
        public virtual FamilyModel Model { get; set; }
        public void Confirm()
        {
            var request = new FamilyRequest { DTO = Mapper.Map(Model, new FamilyDTO()) };
            var response = Helper.Call(s => s.FamilySet(request));
            ServiceResponseHelper.CheckServiceResponse(response, "FamilySet", request);
            Messenger.Default.Send(Messages.Family.Refresh);
            SliderHelper.Close();
        }

        #region ISupportParameter
        public virtual object Parameter { get; set; }
        protected void OnParameterChanged()
        {
            if (Parameter is int patronID)
                Model = new FamilyModel { PatronID = patronID };
            else if (Parameter is FamilyModel model)
                Model = model;
        }
        #endregion
    }
}