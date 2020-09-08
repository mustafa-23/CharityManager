using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Mvvm.ModuleInjection;
using System.Threading.Tasks;
using DevExpress.Mvvm.Native;
using CharityManager.UI.Models;
using CharityManager.UI.CharityService;
using System.Linq;
using Araneo.Common;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace CharityManager.UI.ViewModels
{
    /// <summary>
    /// <see cref="Views.IntroducerView"/>
    /// </summary>
    [POCOViewModel]
    public class IntroducerViewModel : IntroducerInputViewModel.IListener
    {
        private readonly Dictionary<int, BitmapImage> cache = new Dictionary<int, BitmapImage>();
        private const string STATE_VIEW = "View";
        private const string STATE_EDIT = "Edit";
        public virtual string ViewState { get; set; } = STATE_VIEW;
        public DXObservableCollection<IntroducerModel> Models { get; set; } = new DXObservableCollection<IntroducerModel>();
        public virtual IntroducerModel SelectedModel { get; set; } = new IntroducerModel();
        public virtual BitmapImage PersonImage { get; set; }


        public IntroducerViewModel()
        {
            Task.Run(RefreshIntroducers);
        }

        protected void OnSelectedModelChanged()
        {
            if (SelectedModel?.PersonID > 0)
            {
                int id = SelectedModel.PersonID ?? 0;
                if (!cache.ContainsKey(id))
                    cache.Add(id, QuickServiceCall.LoadPersonImage(id));
                PersonImage = cache[id];
            }
            else
                PersonImage = null;
        }

        #region Commands
        public void AddIntroducer()
        {
            ViewState = STATE_EDIT;
            Model = null;
            AppUIManager.Manager.InjectOrNavigate(AppRegions.Introducer, AppModules.IntroducerInput, this);
        }
        public void Edit()
        {
            ViewState = STATE_EDIT;
            Model = SelectedModel;
            AppUIManager.Manager.InjectOrNavigate(AppRegions.Introducer, AppModules.IntroducerInput, this);
        }
        public bool CanEdit() => SelectedModel != null;
        #endregion

        private void RefreshIntroducers()
        {
            int selectedId = SelectedModel?.ID ?? 0;
            var request = new IntroducerRequest
            {
                UserID = GlobalVar.User.ID,
                Filter = new IntroducerFilter(),
                LoadPerson = true
            };
            var response = Helper.Call(s => s.IntroducerGetList(request));
            ServiceResponseHelper.CheckServiceResponse(response, "IntroducerGetList");

            var temp = response.ResultList.Select(i => Mapper.SmartMap(i, new IntroducerModel(),
                (dto, model) => model.Person = Mapper.Map(dto.Person, new PersonModel())));
            Helper.InvokeMainThread(() =>
            {
                Models.Clear();
                Models.AddRange(temp);
                SelectedModel = Models.FirstOrDefault(i => i.ID == selectedId);
            });

        }

        #region IntroducerInputViewModel.IListener
        public IntroducerModel Model { get; set; }
        public void OnIntroducerInputApply(bool result)
        {
            if (SelectedModel?.PersonID > 0)
                cache.Remove(SelectedModel.PersonID ?? 0);
            if (result)
                Task.Run(RefreshIntroducers);

            AppConfigs.Update();
            ViewState = STATE_VIEW;
            AppUIManager.Manager.Remove(AppRegions.Introducer, AppModules.IntroducerInput);
        }
        #endregion
    }
}