using CharityManager.DTO;
using System;
using System.Collections.Generic;

namespace CharityManager.Service
{
    public class ResponseBase
    {
        public int ResultID { get; set; }
        public string Message { get; set; }
        public string UserMessage { get; set; }
        public bool Success { get; set; }
        public ResponseBase()
        {

        }
        public ResponseBase(bool success, int resultID, string message = null, string userMessage = null)
        {
            ResultID = resultID;
            Message = message;
            UserMessage = userMessage;
            Success = success;
        }

        public T Cast<T>() where T : ResponseBase
        {
            var cast = Activator.CreateInstance<T>();
            cast.Success = Success;
            cast.ResultID = ResultID;
            cast.Message = Message;
            cast.UserMessage = UserMessage;

            return cast;
        }
    }
    public class ResponseTemplate<T> : ResponseBase
    {

        public List<T> ResultList { get; set; }
        public T Result { get; set; }
    }
    public class PersonResponse : ResponseTemplate<PersonDTO> { }
    public class PictureResponse : ResponseTemplate<PictureDTO> { }
    public class UserResponse : ResponseTemplate<UserDTO> { }
    public class PatronResponse : ResponseTemplate<PatronDTO> { }
    public class AddressResponse : ResponseTemplate<AddressDTO> { }
    public class FamilyResponse : ResponseTemplate<FamilyDTO> { }
    public class JobResponse : ResponseTemplate<JobDTO> { }
    public class AssetResponse : ResponseTemplate<AssetDTO> { }
    public class EntityResponse : ResponseTemplate<EntityDTO> { }
    public class DocumentResponse : ResponseTemplate<DocumentDTO> { }
}
