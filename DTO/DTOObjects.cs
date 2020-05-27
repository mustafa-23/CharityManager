using System;

namespace CharityManager.DTO
{
    public class DTOBase
    {
        public int ID { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUser { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int? ModifyUser { get; set; }
        public bool Active { get; set; }
    }

    public class PersonDTO : DTOBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string NationalNo { get; set; }
        public string BirthPlace { get; set; }
        public string RegNo { get; set; }
        public string RegPlace { get; set; }
        public string MobileNo { get; set; }
        public int? NationEntityID { get; set; }
        public int? EducationEntityID { get; set; }
    }

    public class UserDTO : DTOBase
    {
        public int PersonID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class LoginDTO : DTOBase { }

    public class PatronDTO : DTOBase
    {
        public int PersonID { get; set; }
        public int MaritalStatus { get; set; }
        public int Children { get; set; }
        public string BloodType { get; set; }
        public string SpecialDisease { get; set; }
        public string Religion { get; set; }
        public string Sect { get; set; }

        #region Navigation
        public PersonDTO Person { get; set; } 
        #endregion
    }

    public class AddressDTO : DTOBase
    {
        public int PatronID { get; set; }
        public byte Type { get; set; }
        public int CityEntityID { get; set; }
        public string Neighbourhood { get; set; }
        public string Phone { get; set; }
        public string Value { get; set; }
        public string Comment { get; set; }
    }

    public class FamilyDTO : DTOBase
    {
        public int PatronID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public short? EmploymentStatus { get; set; }
        public int? Income { get; set; }
        public int? RelationEntityID { get; set; }
        public int? EducationEntityID { get; set; }
    }

    public class AssetDTO : DTOBase
    {
        public int PatronID { get; set; }
        public int? TypeEntityID { get; set; }
        public long? EstimatedValue { get; set; }
        public string Address { get; set; }
    }

    public class JobDTO : DTOBase
    {
        public int PatronID { get; set; }
        public string Title { get; set; }
        public int Income { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }

    public class EntityDTO
    {
        public int ID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class PictureDTO:DTOBase
    {
        public int PersonID { get; set; }
        public byte[] Data { get; set; }
    }
    public class DocumentDTO:DTOBase
    {
        public string Title { get; set; }
        public int PatronID { get; set; }
        public string Path { get; set; }
        public byte Type { get; set; }
    }
}
