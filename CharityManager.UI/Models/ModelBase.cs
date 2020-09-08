using Araneo.Common.Interfaces;
using DevExpress.Mvvm;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CharityManager.UI.Models
{

    #region Enums
    public enum NotificationStatus { New, Seen }
    public enum NotificationType { Normal, Caution, Warning, Error, Success }
    public enum RequestStatus : byte { None, Research, Confirmed, Declined, Finished }

    #endregion

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

        #region ExtraProperties
        public BitmapImage Image { get { return GetProperty(() => Image); } set { SetProperty(() => Image, value); } }
        public string Name => $"{FirstName} {LastName}";
        #endregion

        public override bool Validate()
        {
            ValidateProperty(nameof(NationalNo), NationalNo);
            ValidateProperty(nameof(FirstName), FirstName);
            ValidateProperty(nameof(LastName), LastName);
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
        public int IntroducerID { get; set; }


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
        public string EntityKey { get; set; }
        public string Title { get; set; }
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
        public short? EmploymentStatus { get { return GetProperty(() => EmploymentStatus); } set { SetProperty(() => EmploymentStatus, value); } }
        public short? EducationStatus { get { return GetProperty(() => EducationStatus); } set { SetProperty(() => EducationStatus, value); } }
        public int? Income { get { return GetProperty(() => Income); } set { SetProperty(() => Income, value); } }
        public int? RelationEntityID { get { return GetProperty(() => RelationEntityID); } set { SetProperty(() => RelationEntityID, value); } }
        public int? EducationEntityID { get { return GetProperty(() => EducationEntityID); } set { SetProperty(() => EducationEntityID, value); } }
        public DateTime? BirthDate { get { return GetProperty(() => BirthDate); } set { SetProperty(() => BirthDate, value); } }
    }

    public class DocumentModel : ModelBase
    {
        public string Title { get { return GetProperty(() => Title); } set { SetProperty(() => Title, value); } }
        public int PatronID { get; set; }
        public string Path { get { return GetProperty(() => Path); } set { SetProperty(() => Path, value); } }
        public byte Type { get { return GetProperty(() => Type); } set { SetProperty(() => Type, value); } }
        public string Extension => System.IO.Path.GetExtension(Path).Replace(".", "").ToUpper();
        public long Size => File.Exists(Path) ? new FileInfo(Path).Length : -1;
    }

    public class ResearchModel : ModelBase
    {
        public int RequestID { get; set; }
        public int? UserID { get; set; }
        public DateTime? ResearchDate { get; set; }
        public int NeedTypeEntityID { get; set; }
        public string Place { get; set; }
        public int? Cost { get; set; }
        public string Comment { get; set; }
    }
    public class ManagerViewPointModel : ModelBase
    {
        public int RequestID { get; set; }
        public byte? ViewPoint { get; set; }
        public string Comment { get; set; }
    }

    public class RequestModel : ModelBase
    {
        public int PatronID { get; set; }
        public int TypeEntityID { get; set; }
        public int EstimatedValue { get; set; }
        public string No { get { return GetProperty(() => No); } set { SetProperty(() => No, value); } }
        public string Comment { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? ResearchDate { get; set; }
        public RequestStatus Status { get; set; }

        #region Extra
        public string Name { get; set; }
        #endregion
    }

    [JsonObject]
    public class NotificationModel : BindableBase
    {
        public string Caption { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public DateTime CreateDate { get; set; }
        public NotificationStatus Status { get { return GetProperty(() => Status); } set { SetProperty(() => Status, value); } }

        public NotificationModel(string caption, string message, NotificationType type = NotificationType.Normal)
        {
            Caption = caption;
            Message = message;
            Type = type;
            CreateDate = DateTime.Now;
            Status = NotificationStatus.New;
        }
        public void Seen() => Status = NotificationStatus.Seen;
    }
    [JsonObject]
    public class NoteModel : BindableBase
    {
        public string Content { get { return GetProperty(() => Content); } set { SetProperty(() => Content, value); } }
        public DateTime CreateDate { get; set; }
        public Color Color { get { return GetProperty(() => Color); } set { SetProperty(() => Color, value); } }

        public NoteModel(string content, Color? color = null)
        {
            Content = content;
            Color = color ?? Colors.White;
            CreateDate = DateTime.Now;
        }
    }
    [JsonObject]
    public class IntroducerModel : ModelBase
    {
        public string Title { get { return GetProperty(() => Title); } set { SetProperty(() => Title, value); } }
        public int? PersonID { get; set; }
        /// <summary>
        /// false = Person, true = Company
        /// </summary>
        public bool Type { get { return GetProperty(() => Type); } set { SetProperty(() => Type, value); } }

        public string Name => PersonID > 0 ? Person.Name : Title;

        #region Navigate Properties
        public PersonModel Person { get; set; }
        #endregion
        public override string[] IgnoreList => new string[] { nameof(Person) };
    }


}
