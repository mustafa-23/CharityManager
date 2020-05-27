using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using CharityManager.UI.Models;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class AddressInputViewModel:ISupportParameter
    {
        public virtual AddressModel Model { get; set; }
        public AddressInputViewModel()
        {
            Model = new AddressModel();
        }
        public void Confirm()
        {
            if (Parameter is IAddressInputListener listener)
            {
                listener.OnAddressConfirm(Model);
                SliderHelper.Close();
            }
        }


        #region ISupportParameter
        public virtual object Parameter { get; set; } 
        #endregion

        #region Interfaces
        public interface IAddressInputListener
        {
            void OnAddressConfirm(AddressModel address);
        } 
        #endregion
    }
}