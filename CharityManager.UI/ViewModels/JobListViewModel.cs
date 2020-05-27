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
    public class JobListViewModel : JobInputViewModel.IJobInputListener, ISupportParameter
    {
        public ObservableCollection<JobModel> JobList { get; set; } = new ObservableCollection<JobModel>();

        public void AddJob() => SliderHelper.Open(AppModules.JobInput, this);
        public void OnJobConfirm(JobModel job)
        {
            job.PatronID = (int)Parameter;

            var request = new JobRequest { DTO = Mapper.Map(job, new JobDTO()) };
            var response = Helper.Call(s => s.JobSet(request));

            Task.Run(() => RefreshJobList());
        }
        private void RefreshJobList()
        {
            AppUIManager.Application.Dispatcher.Invoke(() => JobList.Clear());
            if (Parameter is int patronId && patronId > 0)
            {
                int index = 1;
                var request = new JobRequest { Filter = new JobFilter { PatronID = patronId } };
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