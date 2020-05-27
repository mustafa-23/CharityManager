using CharityManager.UI.Models;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class AssetsInputViewModel : ISupportParameter
    {
        public virtual AssetModel Model { get; set; }
        public AssetsInputViewModel()
        {
            Model = new AssetModel();
        }

        #region Commands
        public void Confirm()
        {
            if (Parameter is IAssetInputListener listener)
            {
                listener.OnAssetConfirm(Model);
                SliderHelper.Close();
            }
        }
        #endregion

        #region ISupportParameter
        public virtual object Parameter { get; set; }
        #endregion

        #region Interfaces
        public interface IAssetInputListener
        {
            void OnAssetConfirm(AssetModel asset);
        }
        #endregion
    }
}