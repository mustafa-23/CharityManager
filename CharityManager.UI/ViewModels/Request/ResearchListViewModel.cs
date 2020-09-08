using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using CharityManager.UI.Models;
using CharityManager.UI.CharityService;
using DevExpress.Mvvm.Native;
using Araneo.Common;
using System.Linq;
using System.Threading.Tasks;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class ResearchListViewModel : ISupportParameter
    {
        public virtual RequestModel Model { get; set; }
        public virtual DXObservableCollection<ResearchModel> Researches { get; set; } = new DXObservableCollection<ResearchModel>();
        public void Refresh() => Task.Run(UpdateResearches);
        public void Research()
        {
            var request = new RequestRequest { DTO = new RequestDTO { ID = Model.ID }, UserID = GlobalVar.UserID };
            var response = Helper.Call(s => s.RequestResearch(request));
            ServiceResponseHelper.CheckServiceResponse(response, "RequestResearch", request);
            Helper.NotifySuccess("درخواست تحقیق با موفقیت ثبت شد");
        }
        private void UpdateResearches()
        {
            if (!(Model?.ID > 0)) return;
            var request = new ResearchRequest { Filter = new ResearchFilter { RequestID = Model.ID } };
            var response = Helper.Call(s => s.ResearchGetList(request));
            ServiceResponseHelper.CheckServiceResponse(response, "ResearchGetList", request);
            var temp = response.ResultList.Select(x => Mapper.Map(x, new ResearchModel()));
            Helper.InvokeMainThread(() => Researches.AddRange(temp));
        }
        public virtual object Parameter { get; set; }
        protected void OnParameterChanged()
        {
            Model = Parameter as RequestModel;
            Refresh();
        }
    }
}