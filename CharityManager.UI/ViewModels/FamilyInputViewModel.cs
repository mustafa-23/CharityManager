using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using CharityManager.UI.Models;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class FamilyInputViewModel : ISupportParameter
    {
        public virtual FamilyModel Model { get; set; }
        public FamilyInputViewModel()
        {
            Model = new FamilyModel();
        }
        public void Confirm()
        {
            if (Parameter is IFamilyInputListener listener)
            {
                listener.OnFamilyConfirm(Model);
                SliderHelper.Close();
            }
        }

        #region ISupportParameter
        public virtual object Parameter { get; set; }
        #endregion

        #region Interfaces
        public interface IFamilyInputListener
        {
            void OnFamilyConfirm(FamilyModel family);
        }
        #endregion
    }
}