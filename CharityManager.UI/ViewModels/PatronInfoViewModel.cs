using Araneo.Common;
using CharityManager.UI.CharityService;
using CharityManager.UI.Models;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Linq;
using System.Windows.Media.Imaging;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class PatronInfoViewModel : ISupportParentViewModel
    {
        private bool changed = false;
        private bool loadLock = false;
        private PictureDTO pictureDTO;

        public virtual PersonModel Person { get; set; }
        public virtual PatronModel Model { get; set; }
        public virtual BitmapImage PersonImage { get; set; }

        public PatronInfoViewModel()
        {
            Person = new PersonModel();
            Model = new PatronModel();
        }

        private void MyParent_OnApply()
        {
            if (Person.ID > 0 && !changed) return;

            if (!Person.Validate())
            {
                Helper.NotifyWarning("موارد مشخص شده را برطرف کنید");
                return;
            }


            var request = new PersonRequest { DTO = Mapper.Map(Person, new PersonDTO()) };
            var response = Helper.Call(s => s.PersonSet(request));

            if (response.Success)
            {
                Person.ID = response.ResultID;
                Model.PersonID = response.ResultID;

                SavePicture();

                var patronReq = new PatronRequest { DTO = Mapper.Map(Model, new PatronDTO()) };
                response = Helper.Call(s => s.PatronSet(patronReq));

                if (response.Success)
                {
                    Helper.NotifySuccess("اطلاعات با موفقیت ذخیره شد");
                    Model.ID = response.ResultID;
                    MyParent.PatronID = Model.ID;
                    Messenger.Default.Send(PersonViewModel.Message.RefreshPatronList);
                }
                else
                    Helper.NotifyError(response.UserMessage);
            }
            else
                Helper.NotifyWarning(response.UserMessage);
        }

        #region Bindable Changed
        protected void OnPersonChanged(PersonModel old)
        {
            if (Person != null)
                Person.PropertyChanged += Person_PropertyChanged;
            if (old != null)
                old.PropertyChanged -= Person_PropertyChanged;
        }
        protected void OnModelChanged(PatronModel old)
        {
            if (Person != null)
                Model.PropertyChanged += Person_PropertyChanged;
            if (old != null)
                old.PropertyChanged -= Person_PropertyChanged;
        }
        private void Person_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            changed = true;
            if (e.PropertyName == nameof(Person.NationalNo))
                LoadPersonInfoByNationalNo();
        }
        #endregion

        #region Commands
        public void SetImage()
        {
            DialogHelper.OpenFileDialog.Filter = DialogHelper.FILTER_PIC;
            if (DialogHelper.OpenFileDialog.ShowDialog())
            {
                var size = DialogHelper.OpenFileDialog.File.Length;
                if (size > 256000)
                {
                    Helper.NotifyWarning("سایز عکس نباید از 250 کیلوبایت بیشتر باشد");
                    return;
                }
                PersonImage = new BitmapImage(new Uri(DialogHelper.OpenFileDialog.File.GetFullName(), UriKind.Absolute));
                changed = true;
            }
        }
        #endregion

        private void SavePicture()
        {
            if (PersonImage == null) return;

            var imageBytes = PersonImage.ToByteArray();
            imageBytes = imageBytes ?? Helper.ImageUriToByteArray(PersonImage.UriSource.AbsolutePath);

            PersonRequest request = new PersonRequest
            {
                Picture = new PictureDTO
                {
                    PersonID = Person.ID,
                    Data = imageBytes,
                }
            };
            var response = Helper.Call(s => s.PersonPictureSet(request));
            if (response == null || !response.Success)
                Helper.NotifyError("ذخیره تصویر با مشکل مواجه شد");
        }
        private void LoadPicture()
        {
            pictureDTO = null;
            PersonImage = null;

            if(Person.ID > 0)
            {
                var request = new PersonRequest { Filter = new PersonFilter { ID = Person.ID } };
                var response = Helper.Call(s => s.PersonPictureGet(request));
                pictureDTO = response?.ResultList?.FirstOrDefault();
                PersonImage = pictureDTO?.Data.ToBitmapImage();
            }
        }
        private void LoadPersonInfoByNationalNo()
        {
            if (loadLock) return;

            var personReq = new PersonRequest { Filter = new PersonFilter { NationalNo = Person.NationalNo } };
            var personRes = Helper.Call(s => s.PersonGet(personReq));

            if (personRes.Success)
            {
                Mapper.Map(personRes.Result, Person);
                var request = new PatronRequest { Filter = new PatronFilter { PersonID = Person.ID } };
                var response = Helper.Call(s => s.PatronGet(request));
                Mapper.Map(response.Result, Model);
                MyParent.PatronID = Model.ID;
                Helper.Notify("شخصی با شماره ملی وارد شده یافت شد");
                changed = false;
            }
        }
        private void LoadPatronByID(int patronID)
        {
            var patronReq = new PatronRequest { Filter = new PatronFilter { ID = patronID }, LoadPerson = true, };
            var personRes = Helper.Call(s => s.PatronGet(patronReq));

            if (personRes.Success)
            {
                loadLock = true;
                Mapper.SmartMap(personRes.Result, Model);
                Mapper.Map(personRes.Result.Person, Person);
                LoadPicture();
                loadLock = false;
                changed = false;
            }
        }

        #region ISupportParentViewModel
        public virtual object ParentViewModel { get; set; }
        private PersonInputViewModel MyParent => ParentViewModel as PersonInputViewModel;
        protected void OnParentViewModelChanged(object old)
        {
            if (MyParent?.PatronID > 0)
                LoadPatronByID(MyParent.PatronID);
            if (MyParent != null)
                MyParent.OnApply += MyParent_OnApply;
            if (old is PersonInputViewModel vm)
                vm.OnApply -= MyParent_OnApply;
        }
        #endregion
    }
}