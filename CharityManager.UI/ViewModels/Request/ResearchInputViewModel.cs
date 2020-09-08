using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using CharityManager.UI.Models;
using CharityManager.UI.CharityService;
using Araneo.Common;
using System.Linq;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class ResearchInputViewModel : ISupportParameter
    {
        public virtual ResearchModel Model { get; set; }

        #region Commands
        public void Confirm()
        {
            var request = new ResearchRequest { DTO = Mapper.Map(Model, new ResearchDTO()), UserID = GlobalVar.UserID };
            var response = Helper.Call(s => s.ResearchSet(request));
            ServiceResponseHelper.CheckServiceResponse(response, "ResearchSet", request);
            Messenger.Default.Send(Messages.Research.Refresh);
            SliderHelper.Close();
        }
        #endregion

        #region ISupportParameter
        public virtual object Parameter { get; set; }
        protected void OnParameterChanged()
        {
            if (Parameter is int requestId)
                Model = new ResearchModel
                {
                    RequestID = requestId,
                    ResearchDate = DateTime.Now,
                    UserID = GlobalVar.UserID,
                    NeedTypeEntityID = AppConfigs.NeedTypes.FirstOrDefault()?.ID ?? 0,
                };
            else if (Parameter is ResearchModel model)
                Model = model;
        }
        #endregion
    }
}