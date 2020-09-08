using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using CharityManager.UI.Models;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class ManagerViewPointViewModel
    {
        public DXObservableCollection<ManagerViewPointModel> Models { get; set; } = new DXObservableCollection<ManagerViewPointModel>();
    }
}