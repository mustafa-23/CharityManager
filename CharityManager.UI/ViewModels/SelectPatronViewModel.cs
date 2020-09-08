using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using CharityManager.UI.Models;
using CharityManager.UI.CharityService;
using System.Threading.Tasks;
using System.Linq;
using Araneo.Common;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class SelectPatronViewModel : ISupportParameter
    {
        public DXObservableCollection<PatronModel> Models { get; set; } = new DXObservableCollection<PatronModel>();
        public PatronModel SelectedModel { get; set; }

        private ISelectPatronListener Listener => Parameter as ISelectPatronListener;

        public SelectPatronViewModel() => Task.Factory.StartNew(Refresh);

        public void Select()
        {
            Listener?.OnSelect(SelectedModel);
            SliderHelper.Close();
        }

        private void Refresh()
        {
            Models.Clear();
            var request = new PatronRequest { Filter = new PatronFilter(), LoadPerson = true };
            var response = Helper.Call(s => s.PatronGetList(request));
            ServiceResponseHelper.CheckServiceResponse(response, "PatronGet", request);
            var temp = response.ResultList.Select(p => new PatronModel
            {
                Person = Mapper.Map(p.Person, new PersonModel()),
                PersonID = p.PersonID,
                ID = p.ID
            });
            Helper.InvokeMainThread(() => Models.AddRange(temp));
        }
        #region ISupportParameter
        public virtual object Parameter { get; set; } 
        #endregion

        public interface ISelectPatronListener
        {
            void OnSelect(PatronModel patron);
        }
    }
}