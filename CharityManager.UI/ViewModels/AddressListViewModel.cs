using Araneo.Common;
using CharityManager.UI.CharityService;
using CharityManager.UI.Models;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class AddressListViewModel : ISupportParameter
    {
        public ObservableCollection<AddressModel> AddressList { get; set; } = new ObservableCollection<AddressModel>();

        public AddressListViewModel()
        {
            Messenger.Default.Register<Messages.Address>(this, OnMessageRecieved);
        }
        private void OnMessageRecieved(Messages.Address message) => Task.Run(RefreshAddressList);
        private void RefreshAddressList()
        {
            AppUIManager.Application.Dispatcher.Invoke(() => AddressList.Clear());
            if (Parameter is int patronId && patronId > 0)
            {
                int index = 1;
                var request = new AddressRequest { Filter = new AddressFilter { PatronID = patronId, Active = true } };
                var response = Helper.Call(s => s.AddressGetList(request));
                if (response?.Success ?? false)
                    AppUIManager.Application.Dispatcher.Invoke(() =>
                    AddressList.AddRange(response.ResultList.Select(dto => Mapper.SmartMap(dto, new AddressModel(), (s, d) => d.Tag = $"نشانی {index++}"))));
            }
        }

        #region ISupportParameter
        public virtual object Parameter { get; set; }
        protected void OnParameterChanged() => Task.Run(() => RefreshAddressList());
        #endregion
    }
}