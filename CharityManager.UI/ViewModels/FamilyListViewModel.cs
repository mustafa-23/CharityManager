using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using CharityManager.UI.Models;
using System.Linq;
using System.Threading.Tasks;
using Araneo.Common;
using CharityManager.UI.CharityService;
using System.Collections.ObjectModel;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class FamilyListViewModel : ISupportParameter, FamilyInputViewModel.IFamilyInputListener
    {
        public ObservableCollection<FamilyModel> FamilyList { get; set; } = new ObservableCollection<FamilyModel>();

        public void AddFamily() => SliderHelper.Open(AppModules.FamilyInput, this);
        public void OnFamilyConfirm(FamilyModel family)
        {
            family.PatronID = (int)Parameter;

            var request = new FamilyRequest { DTO = Mapper.Map(family, new FamilyDTO()) };
            var response = Helper.Call(s => s.FamilySet(request));

            Task.Run(() => RefreshFamilyList());
        }
        private void RefreshFamilyList()
        {
            AppUIManager.Application.Dispatcher.Invoke(() => FamilyList.Clear());
            if (Parameter is int patronId && patronId > 0)
            {
                int index = 1;
                var request = new FamilyRequest { Filter = new FamilyFilter { PatronID = patronId } };
                var response = Helper.Call(s => s.FamilyGetList(request));
                if (response?.Success ?? false)
                    AppUIManager.Application.Dispatcher.Invoke(() =>
                   FamilyList.AddRange(response.ResultList.Select(dto => Mapper.SmartMap(dto, new FamilyModel(), (s, d) => d.Tag = $"تحت تکفل {index++}"))));
            }
        }

        #region ISupportParameter
        public virtual object Parameter { get; set; }
        protected void OnParameterChanged() => Task.Run(() => RefreshFamilyList());
        #endregion

    }
}