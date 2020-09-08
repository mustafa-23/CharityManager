using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using CharityManager.UI.Models;
using System.Threading.Tasks;
using System.Linq;
using Araneo.Common;
using CharityManager.UI.CharityService;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class JobListViewModel : ISupportParameter
    {
        public ObservableCollection<JobModel> JobList { get; set; } = new ObservableCollection<JobModel>();

        public JobListViewModel()
        {
            Messenger.Default.Register<Messages.Job>(this, OnMessageRecieved);
        }
        private void OnMessageRecieved(Messages.Job message) => Task.Run(RefreshJobList);
        private void RefreshJobList()
        {
            AppUIManager.Application.Dispatcher.Invoke(() => JobList.Clear());
            if (Parameter is int patronId && patronId > 0)
            {
                int index = 1;
                var request = new JobRequest { Filter = new JobFilter { PatronID = patronId, Active = true } };
                var response = Helper.Call(s => s.JobGetList(request));
                if (response?.Success ?? false)
                    AppUIManager.Application.Dispatcher.Invoke(() =>
                   JobList.AddRange(response.ResultList.Select(dto => Mapper.SmartMap(dto, new JobModel(), (s, d) => d.Tag = $"درآمد {index++}"))));
            }
        }


        #region ISupportParameter
        public virtual object Parameter { get; set; }
        protected void OnParameterChanged() => Task.Run(() => RefreshJobList());
        #endregion
    }
}