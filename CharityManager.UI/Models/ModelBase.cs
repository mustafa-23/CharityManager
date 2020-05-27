using Araneo.Common.Interfaces;
using DevExpress.Mvvm;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Windows.Media.Imaging;

namespace CharityManager.UI.Models
{
    public class ModelBase : BindableBase, IMappable
    {
        public int ID { get { return GetProperty(() => ID); } set { SetProperty(() => ID, value); } }
        public DateTime CreateDate { get; set; }
        public int CreateUser { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int? ModifyUser { get; set; }
        public bool Active { get; set; }
        public object Tag { get; set; }

        public virtual string[] IgnoreList => null;
    }
    [JsonObject]
    public class PersonModel : ValidatableBase
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "وارد کردن این فیلد اجباری ست")]
        public string FirstName { get { return GetProperty(() => FirstName); } set { SetProperty(() => FirstName, value); } }
        [Required(AllowEmptyStrings = false, ErrorMessage = "وارد کردن این فیلد اجباری ست")]
        public string LastName { get { return GetProperty(() => LastName); } set { SetProperty(() => LastName, value); } }
        public string FatherName { get { return GetProperty(() => FatherName); } set { SetProperty(() => FatherName, value); } }
        public DateTime? BirthDate { get { return GetProperty(() => BirthDate); } set { SetProperty(() => BirthDate, value); } }
        [Required(AllowEmptyStrings = false, ErrorMessage = "کد ملی الزامی ست")]
        public string NationalNo { get { return GetProperty(() => NationalNo); } set { SetProperty(() => NationalNo, value); } }
        public string BirthPlace { get { return GetProperty(() => BirthPlace); } set { SetProperty(() => BirthPlace, value); } }
        public string RegNo { get { return GetProperty(() => RegNo); } set { SetProperty(() => RegNo, value); } }
        public string RegPlace { get { return GetProperty(() => RegPlace); } set { SetProperty(() => RegPlace, value); } }
        public int? EducationEntityID { get { return GetProperty(() => EducationEntityID); } set { SetProperty(() => EducationEntityID, value); } }
        public int? NationEntityID { get { return GetProperty(() => NationEntityID); } set { SetProperty(() => NationEntityID, value); } }
        public string MobileNo { get { return GetProperty(() => MobileNo); } set { SetProperty(() => MobileNo, value); } }

        public BitmapImage Image { get; set; }
        public string Name => $"{FirstName} {LastName}";

        public override bool Validate()
        {
            ValidateProperty(nameof(NationalNo), NationalNo);
            ValidateProperty(nameof(NationalNo), FirstName);
            ValidateProperty(nameof(NationalNo), LastName);
            return !HasErrors;
        }
    }

    [JsonObject]
    public class PatronModel : ModelBase
    {
        public int PersonID { get; set; }
        public int MaritalStatus { get { return GetProperty(() => MaritalStatus); } set { SetProperty(() => MaritalStatus, value); } }
        public int Children { get { return GetProperty(() => Children); } set { SetProperty(() => Children, value); } }
        public string BloodType { get { return GetProperty(() => BloodType); } set { SetProperty(() => BloodType, value); } }
        public string SpecialDisease { get { return GetProperty(() => SpecialDisease); } set { SetProperty(() => SpecialDisease, value); } }
        public string Religion { get { return GetProperty(() => Religion); } set { SetProperty(() => Religion, value); } }
        public string Sect { get { return GetProperty(() => Sect); } set { SetProperty(() => Sect, value); } }

        public override string[] IgnoreList => new string[] { nameof(Person) };



        #region Navigate Properties
        public PersonModel Person { get; set; }
        #endregion
    }

    public class UserModel : ModelBase
    {
        public int PersonID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        #region Navigate Properties
        public PersonModel Person { get; set; }
        #endregion
    }

    public class EntityModel
    {
        public int ID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class AddressModel : ModelBase, IMappable
    {
        public int PatronID { get; set; }
        public byte Type { get { return GetProperty(() => Type); } set { SetProperty(() => Type, value); } }
        public int CityEntityID { get { return GetProperty(() => CityEntityID); } set { SetProperty(() => CityEntityID, value); } }
        public string Neighbourhood { get { return GetProperty(() => Neighbourhood); } set { SetProperty(() => Neighbourhood, value); } }
        public string Phone { get { return GetProperty(() => Phone); } set { SetProperty(() => Phone, value); } }
        public string Value { get { return GetProperty(() => Value); } set { SetProperty(() => Value, value); } }
        public string Comment { get { return GetProperty(() => Comment); } set { SetProperty(() => Comment, value); } }

        public string[] IgnoreList => null;
    }

    public class AssetModel : ModelBase
    {
        public int PatronID { get { return GetProperty(() => PatronID); } set { SetProperty(() => PatronID, value); } }
        public int? TypeEntityID { get { return GetProperty(() => TypeEntityID); } set { SetProperty(() => TypeEntityID, value); } }
        public long? EstimatedValue { get { return GetProperty(() => EstimatedValue); } set { SetProperty(() => EstimatedValue, value); } }
        public double Share { get { return GetProperty(() => Share); } set { SetProperty(() => Share, value); } }
        public string Address { get { return GetProperty(() => Address); } set { SetProperty(() => Address, value); } }
    }

    public class JobModel : ModelBase
    {
        public int PatronID { get; set; }
        public string Title { get; set; }
        public int Income { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }

    public class FamilyModel : ModelBase
    {
        public int PatronID { get { return GetProperty(() => PatronID); } set { SetProperty(() => PatronID, value); } }
        public string FirstName { get { return GetProperty(() => FirstName); } set { SetProperty(() => FirstName, value); } }
        public string LastName { get { return GetProperty(() => LastName); } set { SetProperty(() => LastName, value); } }
        public DateTime? BirthDate { get { return GetProperty(() => BirthDate); } set { SetProperty(() => BirthDate, value); } }
        public short? EmploymentStatus { get { return GetProperty(() => EmploymentStatus); } set { SetProperty(() => EmploymentStatus, value); } }
        public int? Income { get { return GetProperty(() => Income); } set { SetProperty(() => Income, value); } }
        public int? RelationEntityID { get { return GetProperty(() => RelationEntityID); } set { SetProperty(() => RelationEntityID, value); } }
        public int? EducationEntityID { get { return GetProperty(() => EducationEntityID); } set { SetProperty(() => EducationEntityID, value); } }
        public int? EducationStatus { get { return GetProperty(() => EducationStatus); } set { SetProperty(() => EducationStatus, value); } }
    }

    public class DocumentModel : ModelBase
    {
        public string Title { get { return GetProperty(() => Title); } set { SetProperty(() => Title, value); } }
        public int PatronID { get; set; }
        public string Path { get { return GetProperty(() => Path); } set { SetProperty(() => Path, value); } }
        public byte Type { get { return GetProperty(() => Type); } set { SetProperty(() => Type, value); } }
        public string Extension => System.IO.Path.GetExtension(Path).Replace(".", "").ToUpper();
        public long Size => new FileInfo(Path).Length;
    }
}
