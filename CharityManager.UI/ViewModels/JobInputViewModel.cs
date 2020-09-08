using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using CharityManager.UI.Models;
using CharityManager.UI.CharityService;
using Araneo.Common;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class JobInputViewModel : ISupportParameter
    {
        public virtual JobModel Model { get; set; } = new JobModel();

        public void Confirm()
        {
            var request = new JobRequest { DTO = Mapper.Map(Model, new JobDTO()) };
            var response = Helper.Call(s => s.JobSet(request));
            ServiceResponseHelper.CheckServiceResponse(response, "JobSet", request);
            SliderHelper.Close();
            Messenger.Default.Send(Messages.Job.Refresh);
        }

        #region ISupportParameter
        public virtual object Parameter { get; set; }
        protected void OnParameterChanged()
        {
            if (Parameter is int patronID)
                Model = new JobModel { PatronID = patronID };
            else if (Parameter is JobModel model)
                Model = model;
        }

        #endregion
    }
}