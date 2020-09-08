using CharityManager.UI.Models;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class ManagerViewPointInputViewModel : ISupportParameter
    {
        public virtual ManagerViewPointModel Model { get; set; }
        public void Confirm()
        {
            Helper.Call(s => s.);
        }

        #region ISupportParameter
        public virtual object Parameter { get; set; } 
        protected void OnParameterChanged()
        {
            if (Parameter is int id)
                Model = new ManagerViewPointModel
                {
                    RequestID = id,
                    ViewPoint = 0,
                };
            else if (Parameter is ManagerViewPointModel model)
                Model = model;
        }
        #endregion
    }
}
