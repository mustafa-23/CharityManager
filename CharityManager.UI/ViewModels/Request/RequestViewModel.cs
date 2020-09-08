using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using CharityManager.UI.Models;
using DevExpress.Mvvm.ModuleInjection;
using System.Linq;
using Araneo.Common;
using System.Threading.Tasks;
using CharityManager.UI.CharityService;
using System.Windows.Media.Imaging;

namespace CharityManager.UI.ViewModels
{
    /// <summary>
    /// <see cref="Views.RequestView"/>
    /// </summary>
    [POCOViewModel]
    public class RequestViewModel
    {
        public DXObservableCollection<RequestModel> Models { get; set; } = new DXObservableCollection<RequestModel>();
        public virtual RequestModel SelectedModel { get; set; }
        public virtual PatronModel Patron { get; set; }
        public virtual BitmapImage Image { get; set; }

        public RequestViewModel()
        {
            Task.Run(Refresh);
        }

        public void AddRequest() => AppUIManager.Manager.InjectOrNavigate(AppRegions.Request, AppModules.RequestInput);
        protected void OnSelectedModelChanged()
        {
            var request = new PatronRequest
            {
                Filter = new PatronFilter { ID = SelectedModel.PatronID },
                LoadPerson = true,
            };
            var response = Helper.Call(s => s.PatronGet(request));
            ServiceResponseHelper.CheckServiceResponse(response, "PatronGet", request);
            Patron = Mapper.SmartMap(response.Result, new PatronModel(), (s, d) =>
            {
                d.Person = Mapper.Map(response.Result.Person, new PersonModel());
            });
            Image = QuickServiceCall.LoadPersonImage(Patron.PersonID);
        }
        private void Refresh()
        {
            Models.Clear();
            var response = Helper.Call(s => s.RequestGetList(new RequestRequest()));
            ServiceResponseHelper.CheckServiceResponse(response, "RequestGetList");
            var list = response.ResultList.Select(r => Mapper.Map(r, new RequestModel())).ToList();

            var patronReq = new PatronRequest { Filter = new PatronFilter { IDList = list.Select(r => r.PatronID).ToArray() }, LoadPerson = true };
            var patronRes = Helper.Call(s => s.PatronGetList(patronReq));
            ServiceResponseHelper.CheckServiceResponse(response, "PatronGetList", patronReq);
            if (patronRes.ResultList?.Count() > 0)
                list.ForEach(r =>
                {
                    var person = patronRes.ResultList.FirstOrDefault(p => p.ID == r.PatronID)?.Person;
                    r.Name = $"{person.FirstName} {person.LastName}";
                });
            Models.AddRange(list);
        }
    }
}