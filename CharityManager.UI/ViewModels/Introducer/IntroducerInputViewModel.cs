using Araneo.Common;
using CharityManager.UI.CharityService;
using CharityManager.UI.Models;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class IntroducerInputViewModel : ISupportParameter
    {
        private bool imageChanged;
        #region Bindables
        public virtual IntroducerModel Model { get; set; }
        public virtual PersonModel Person { get; set; }
        #endregion

        public IntroducerInputViewModel()
        {
            Model = new IntroducerModel();
        }

        #region Commands
        public void Apply()
        {
            if (Model.Type && string.IsNullOrEmpty(Model.Title))
                Helper.NotifyWarning("لطفا هنوان را وارد کنید");

            if (!Model.Type)
            {
                CreatePerson();
                if (imageChanged)
                    SetImage();
            }

            var request = new IntroducerRequest
            {
                UserID = GlobalVar.User.ID,
                DTO = new IntroducerDTO
                {
                    ID = Model.ID,
                    PersonID = Model.Type ? null : (int?)Person.ID,
                    Title = Model.Title,
                    Type = Model.Type
                }
            };
            var response = Helper.Call(s => s.IntroducerSet(request));
            ServiceResponseHelper.CheckServiceResponse(response, "IntroducerSet", request);
            Helper.NotifySuccess($"ثبت معرف با موفقیت انجام شد", "ثبت معرف");
            Listener?.OnIntroducerInputApply(true);
        }
        public void Back()
        {
            AppUIManager.Manager.Clear(AppRegions.Introducer);
            Listener?.OnIntroducerInputApply(false);
        }
        #endregion

        #region BindableChanged
        protected void OnPersonChanged(PersonModel old)
        {
            if (Person != null)
            {
                Person.PropertyChanged += Person_PropertyChanged;
                Person.Image = QuickServiceCall.LoadPersonImage(Person.ID);
                imageChanged = false;
            }
            if (old != null)
                old.PropertyChanged -= Person_PropertyChanged;
        }
        #endregion

        #region Methods
        private void SetImage()
        {
            if (Person.Image == null) return;

            var imageBytes = Person.Image.ToByteArray();
            imageBytes = imageBytes ?? Helper.ImageUriToByteArray(Person.Image.UriSource.AbsolutePath);

            PersonRequest request = new PersonRequest
            {
                Picture = new PictureDTO
                {
                    PersonID = Person.ID,
                    Data = imageBytes,
                    CreateUser = GlobalVar.User.ID,
                }
            };
            var response = Helper.Call(s => s.PersonPictureSet(request));
            ServiceResponseHelper.CheckServiceResponse(response, "PersonPictureSet");
        }
        private void CreatePerson()
        {
            if (Person.Validate() == false)
                throw new Exception("لطفا موارد مشخص شده را اصلاح کنید");
            var request = new PersonRequest { DTO = Mapper.Map(Person, new PersonDTO()) };
            var response = Helper.Call(s => s.PersonSet(request));
            ServiceResponseHelper.CheckServiceResponse(response, "PersonSet", request);
            Person.ID = response.ResultID;
        }
        #endregion

        #region Events
        private void Person_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Person.NationalNo))
            {
                var person = QuickServiceCall.PersonByNationalNo(Person.NationalNo);
                Mapper.Map(person, Person);
                if (person != null)
                    Person.Image = QuickServiceCall.LoadPersonImage(Person.ID);
                imageChanged = false;
            }
            else if (e.PropertyName == nameof(Person.Image))
                imageChanged = true;
        }
        #endregion

        #region ISupportParameter
        public virtual object Parameter { get; set; }
        public IListener Listener => Parameter as IListener;
        protected void OnParameterChanged()
        {
            if (Parameter is IntroducerModel model)
                Model = model;
            else if (Listener != null)
                Model = Listener.Model;

            if (Model == null)
                Model = new IntroducerModel();
            Person = Model?.Person ?? new PersonModel();
        }
        #endregion

        public interface IListener
        {
            IntroducerModel Model { get; set; }
            void OnIntroducerInputApply(bool result);
        }
    }
}