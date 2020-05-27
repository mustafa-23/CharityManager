using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using CharityManager.UI.Models;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class JobInputViewModel : ISupportParameter
    {
        public virtual JobModel Model { get; set; } = new JobModel();

        public void Confirm()
        {
            if (Parameter is IJobInputListener listener)
            {
                listener.OnJobConfirm(Model);
                SliderHelper.Close();
            }
        }

        #region ISupportParameter
        public virtual object Parameter { get; set; }
        #endregion

        #region Interfaces
        public interface IJobInputListener
        {
            void OnJobConfirm(JobModel job);
        }
        #endregion
    }
}