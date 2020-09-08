using Araneo.Common;
using CharityManager.UI.CharityService;
using CharityManager.UI.Models;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class AssetsInputViewModel : ISupportParameter
    {
        public virtual AssetModel Model { get; set; }

        #region Commands
        public void Confirm()
        {
            var request = new AssetRequest { DTO = Mapper.Map(Model, new AssetDTO()) };
            var response = Helper.Call(s => s.AssetSet(request));
            ServiceResponseHelper.CheckServiceResponse(response, "AssetSet",request);
            Messenger.Default.Send(Messages.Asset.Refresh);
            SliderHelper.Close();
        }
        #endregion

        #region ISupportParameter
        public virtual object Parameter { get; set; }
        protected void OnParameterChanged()
        {
            if (Parameter is int patronID)
                Model = new AssetModel { PatronID = patronID };
            else if (Parameter is AssetModel model)
                Model = model;
        }
        #endregion
    }
}