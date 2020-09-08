using CharityManager.DTO;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CharityManager.Service
{
    public class RequestBase
    {
        public int UserID { get; set; }
    }

    /// <summary>
    /// a message sent to server
    /// </summary>
    /// <typeparam name="T">DTO object</typeparam>
    /// <typeparam name="U">Filter object</typeparam>
    public class RequestTemplate<T, U> : RequestBase
    {
        public T DTO { get; set; }
        public List<T> DTOList { get; set; }
        public U Filter { get; set; }
    }

    public class PersonRequest : RequestTemplate<PersonDTO, PersonFilter>
    {
        public PictureDTO Picture { get; set; }
    }
    public class UserRequest : RequestTemplate<UserDTO, UserFilter> { }

    public class PatronRequest : RequestTemplate<PatronDTO, PatronFilter>
    {
        public bool LoadPerson { get; set; }
    }

    public class AddressRequest : RequestTemplate<AddressDTO, AddressFilter> { }
    public class FamilyRequest : RequestTemplate<FamilyDTO, FamilyFilter> { }
    public class JobRequest : RequestTemplate<JobDTO, JobFilter> { }
    public class AssetRequest : RequestTemplate<AssetDTO, AssetFilter> { }
    public class EntityRequest : RequestTemplate<EntityDTO, EntityFilter> { }
    public class DocumentRequest : RequestTemplate<DocumentDTO, DocumentFilter> { }
    public class IntroducerRequest : RequestTemplate<IntroducerDTO, IntroducerFilter>
    {
        public bool LoadPerson { get; set; }
    }
    public class RequestRequest : RequestTemplate<RequestDTO, RequestFilter> { }
    public class ResearchRequest : RequestTemplate<ResearchDTO, ResearchFilter> { }
    public class ManagerViewPointRequest : RequestTemplate<ManagerViewPointDTO, ManagerViewPointFilter> { }
}
