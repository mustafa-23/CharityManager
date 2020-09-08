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
    public class FamilyListViewModel : ISupportParameter
    {
        public ObservableCollection<FamilyModel> FamilyList { get; set; } = new ObservableCollection<FamilyModel>();

        public FamilyListViewModel()
        {
            Messenger.Default.Register<Messages.Family>(this, OnMessageRecieved);
        }
        private void OnMessageRecieved(Messages.Family message) => Task.Run(RefreshFamilyList);
        private void RefreshFamilyList()
        {
            AppUIManager.Application.Dispatcher.Invoke(() => FamilyList.Clear());
            if (Parameter is int patronId && patronId > 0)
            {
                int index = 1;
                var request = new FamilyRequest { Filter = new FamilyFilter { PatronID = patronId, Active = true } };
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