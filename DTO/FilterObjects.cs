using System.Collections.Generic;

namespace CharityManager.DTO
{
    public class FilterBase
    {
        public int ID { get; set; }
        public List<int> IDList { get; set; }
        public DateRange CreateDate { get; set; }
        public DateRange ModifyDate { get; set; }
        public bool? Active { get; set; }
    }

    public class PersonFilter : FilterBase
    {
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    public class UserFilter : FilterBase
    {
        public string UserName { get; set; }
        public int PersonID { get; set; }
    }
    public class PatronFilter : FilterBase
    {
        public int PersonID { get; set; }
        public string NationalNo { get; set; }
    }
    public class AddressFilter : FilterBase
    {
        public int PatronID { get; set; }
    }
    public class FamilyFilter : FilterBase
    {
        public int PatronID { get; set; }
    }
    public class JobFilter : FilterBase
    {
        public int PatronID { get; set; }
    }
    public class AssetFilter : FilterBase
    {
        public int PatronID { get; set; }
    }
    public class EntityFilter : FilterBase
    {
        public string Key { get; set; }
    }
    public class DocumentFilter : FilterBase
    {
        public int PatronID { get; set; }
    }
    public class IntroducerFilter : FilterBase
    {
        public int PersonID { get; set; }
    }
    public class RequestFilter : FilterBase
    {
        public string No { get; set; }
        public int PatronID { get; set; }

    }
    public class ResearchFilter : FilterBase
    {
        public int RequestID { get; set; }
    }
    public class ManagerViewPointFilter : FilterBase
    {
        public int RequestID { get; set; }
    }
}
