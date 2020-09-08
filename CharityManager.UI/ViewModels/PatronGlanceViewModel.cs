using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using CharityManager.UI.Models;
using CharityManager.UI.CharityService;
using System.Windows.Media.Imaging;
using System.Linq;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class PatronGlanceViewModel : ISupportParameter
    {
        public virtual PatronModel Model { get; set; }
        public virtual int PatronID { get; set; }
        public virtual BitmapImage PersonImage { get; set; }

        public PatronGlanceViewModel()
        {

        }
        public virtual object Parameter { get; set; }
        protected void OnParameterChanged()
        {
            if (Parameter is PatronModel patron)
            {
                Model = patron;
                LoadPersonImage();
            }
        }
        private void LoadPersonImage()
        {
            PersonImage = null;
            var request = new PersonRequest { Filter = new PersonFilter { ID = Model.PersonID } };
            var response = Helper.Call(s => s.PersonPictureGet(request));
            if (response?.Success == true)
                PersonImage = response.ResultList.FirstOrDefault()?.Data.ToBitmapImage();
        }
    }
}