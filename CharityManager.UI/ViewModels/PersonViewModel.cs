using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Mvvm.ModuleInjection;
using System.Collections.ObjectModel;
using CharityManager.UI.Models;
using System.Threading.Tasks;
using System.Linq;
using Araneo.Common;
using CharityManager.UI.CharityService;
using DevExpress.Mvvm.Native;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class PersonViewModel
    {
        public enum Message
        {
            RefreshPatronList
        }

        public ObservableCollection<PatronModel> Models { get; set; } = new ObservableCollection<PatronModel>();
        public virtual PatronModel SelectedModel { get; set; }

        public PersonViewModel()
        {
            Task.Run(RefreshModels);
            Messenger.Default.Register<Message>(this, OnMessageRecieved);
        }

        private void OnMessageRecieved(Message obj)
        {
            switch (obj)
            {
                case Message.RefreshPatronList:
                    Task.Run(RefreshModels);
                    break;
                default:
                    break;
            }
        }

        protected void OnSelectedModelChanged()
        {
            if (SelectedModel != null)
                AppUIManager.Manager.Clear(AppRegions.Person);
        }

        private void RefreshModels()
        {
            var request = new PatronRequest { Filter = new PatronFilter(), LoadPerson = true };
            var response = Helper.Call(s => s.PatronGetList(request));
            if (response?.Success ?? false)
            {
                AppUIManager.Application.Dispatcher.Invoke(() =>
                {
                    Models.Clear();
                    Models.AddRange(response.ResultList.Select(dto => Mapper.SmartMap(dto, new PatronModel(),
                        (s, d) => d.Person = Mapper.Map(s.Person, new PersonModel()))));
                });
            }
        }

        public void AddPerson() => AppUIManager.Manager.InjectOrNavigate(AppRegions.Person, AppModules.PersonInput);
    }
}