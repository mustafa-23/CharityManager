using Araneo.Common;
using CharityManager.UI.CharityService;
using CharityManager.UI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CharityManager.UI
{
    using IntStringPair = KeyValuePair<int, string>;
    public static class AppConfigs
    {
        public static ObservableCollection<EntityModel> All { get; set; } = new ObservableCollection<EntityModel>();
        public static ObservableCollection<IntStringPair> Introducers { get; set; } = new ObservableCollection<IntStringPair>();
        public static List<EntityModel> Nations => All.Where(c => c.EntityKey == "Nation").ToList();
        public static List<EntityModel> Educations => All.Where(c => c.EntityKey == "Education").ToList();
        public static List<EntityModel> Cities => All.Where(c => c.EntityKey == "City").ToList();
        public static List<EntityModel> Relations => All.Where(c => c.EntityKey == "Relation").ToList();
        public static List<EntityModel> Assets => All.Where(c => c.EntityKey == "Asset").ToList();
        public static List<EntityModel> RequestTypes => All.Where(c => c.EntityKey == "RequestType").ToList();
        public static List<EntityModel> NeedTypes => All.Where(c => c.EntityKey == "NeedType").ToList();

        static AppConfigs()
        {
            if (AppUIManager.IsInDesignMode())
                return;
        }
        public static void Update() => Task.Run(Refresh);
        private static void Refresh()
        {
            RefreshConfigs();
            RefreshIntroducers();
        }

        private static void RefreshIntroducers()
        {
            var request = new IntroducerRequest { UserID = GlobalVar.User.ID, LoadPerson = true };
            var list = Helper.Call(s => s.IntroducerGetList(request));
            if (list?.Success ?? false)
                Helper.InvokeMainThread(() =>
                {
                    Introducers.Clear();
                    Introducers.AddRange(list.ResultList.Select(i => new IntStringPair(i.ID, i.Title ?? $"{i.Person.FirstName} {i.Person.LastName}")));
                });
        }

        private static void RefreshConfigs()
        {
            var list = Helper.Call(s => s.EntityGetList(new EntityRequest()));
            if (list?.Success ?? false)
                Helper.InvokeMainThread(() =>
                {
                    All.Clear();
                    All.AddRange(list.ResultList.Select(e => Mapper.Map(e, new EntityModel())));
                });
        }
    }
}
