using Araneo.Common;
using CharityManager.UI.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CharityManager.UI
{
    public static class AppConfigs
    {
        public static ObservableCollection<EntityModel> All { get; set; } = new ObservableCollection<EntityModel>();
        public static List<EntityModel> Nations => All.Where(c=>c.Key == "Nation").ToList();
        public static List<EntityModel> Educations => All.Where(c => c.Key == "Education").ToList();
        public static List<EntityModel> Cities => All.Where(c => c.Key == "City").ToList();
        public static List<EntityModel> Relations => All.Where(c => c.Key == "Relation").ToList();
        public static List<EntityModel> Assets => All.Where(c => c.Key == "Asset").ToList();

        static AppConfigs()
        {
            if (AppUIManager.IsInDesignMode())
                return;
            Refresh();
        }

        private static void Refresh()
        {
            Nations.Clear();
            Educations.Clear();
            Cities.Clear();
            Relations.Clear();
            var list = Helper.Call(s => s.EntityGetList(new CharityService.EntityRequest()));
            if (list.Success)
                All.AddRange(list.ResultList.Select(e => Mapper.Map(e, new EntityModel())));
        }
    }
}
