using Araneo.Common;
using CharityManager.UI.CharityService;
using CharityManager.UI.Models;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.Native;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class AssetListViewModel : ISupportParameter, AssetsInputViewModel.IAssetInputListener
    {
        public ObservableCollection<AssetModel> AssetList { get; set; } = new ObservableCollection<AssetModel>();

        public void Add() => SliderHelper.Open(AppModules.AssetInput, this);
        public void OnAssetConfirm(AssetModel asset)
        {
            asset.PatronID = (int)Parameter;

            var request = new AssetRequest { DTO = Mapper.Map(asset, new AssetDTO()) };
            var response = Helper.Call(s => s.AssetSet(request));

            Task.Run(() => RefreshAssetList());
        }

        private void RefreshAssetList()
        {
            AppUIManager.Application.Dispatcher.Invoke(() => AssetList.Clear());
            if (Parameter is int patronId && patronId > 0)
            {
                int index = 1;
                var request = new AssetRequest { Filter = new AssetFilter { PatronID = patronId } };
                var response = Helper.Call(s => s.AssetGetList(request));
                if (response?.Success ?? false)
                    AppUIManager.Application.Dispatcher.Invoke(() =>
                   AssetList.AddRange(response.ResultList.Select(dto => Mapper.SmartMap(dto, new AssetModel(), (s, d) => d.Tag = $"دارایی {index++}"))));
            }
        }

        #region ISupportParameter
        public virtual object Parameter { get; set; }
        protected void OnParameterChanged() => Task.Run(() => RefreshAssetList());
        #endregion
    }
}