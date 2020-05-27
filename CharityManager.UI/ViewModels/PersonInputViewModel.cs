using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using System.Windows;
using DevExpress.Mvvm.POCO;
using System.Collections.ObjectModel;
using CharityManager.UI.Models;
using System.Threading.Tasks;
using Araneo.Common;
using System.Linq;
using CharityManager.UI.CharityService;

namespace CharityManager.UI.ViewModels
{
    /// <summary>
    /// <see cref="Views.PersonInputView"/>
    /// </summary>
    [POCOViewModel]
    public class PersonInputViewModel : ISupportParameter
    {
        public delegate void VoidHandler();
        public event VoidHandler OnApply;

        private enum Tabs
        {
            PersonInfo, Address, Family, Job, Asset, Document, Helps, Other
        }

        #region Commands

        #endregion

        public virtual int SelectedTabIndex { get; set; }
        public virtual int PatronID { get; set; }

        public Visibility ExtraTabVisibility => PatronID == 0 ? Visibility.Collapsed : Visibility.Visible;

        public PersonInputViewModel()
        {
            SelectedTabIndex = 0;
        }

        protected void OnPatronIDChanged() => POCOViewModelExtensions.RaisePropertyChanged(this, vm => vm.ExtraTabVisibility);
        public void Apply() => OnApply?.Invoke();

        #region ISupportParameter
        public virtual object Parameter { get; set; }
        protected void OnParameterChanged()
        {
            if (Parameter is PatronModel p)
                PatronID = p.ID;
        }
        #endregion

    }
}