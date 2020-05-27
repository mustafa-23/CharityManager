using System.ServiceModel;

namespace CharityManager.Service
{
    [ServiceContract]
    public interface ICharity
    {
        /// <summary>
        /// افزودن یا ویرایش اطلاعات فردی
        /// </summary>
        /// <param name="request"><see cref="PersonRequest.DTO"/></param>
        /// <returns></returns>
        [OperationContract]
        ResponseBase PersonSet(PersonRequest request);

        /// <summary>
        /// جستجوی فرد تنها با اطلاعات منحصر به فرد
        /// </summary>
        /// <param name="request"><see cref="PersonRequest.Filter"/></param>
        [OperationContract]
        PersonResponse PersonGet(PersonRequest request);

        /// <summary>
        /// دریافت لیست مشتریان بر اساس فیلترهای داده شده
        /// </summary>
        /// <param name="request"><see cref="PersonRequest.Filter"/></param>
        [OperationContract]
        PersonResponse PersonGetList(PersonRequest request);

        [OperationContract]
        ResponseBase PersonPictureSet(PersonRequest request);
        [OperationContract]
        PictureResponse PersonPictureGet(PersonRequest request);
        [OperationContract]
        ResponseBase UserSet(UserRequest request);
        [OperationContract]
        UserResponse UserGet(UserRequest request);
        [OperationContract]
        UserResponse UserGetList(UserRequest request);
        [OperationContract]
        ResponseBase Login(UserRequest request);

        [OperationContract]
        EntityResponse EntityGetList(EntityRequest request);

        #region Patron
        [OperationContract]
        ResponseBase PatronSet(PatronRequest request);
        [OperationContract]
        PatronResponse PatronGet(PatronRequest request);
        [OperationContract]
        PatronResponse PatronGetList(PatronRequest request);

        [OperationContract]
        ResponseBase AddressSet(AddressRequest request);
        [OperationContract]
        ResponseBase AddressSetList(AddressRequest request);
        [OperationContract]
        AddressResponse AddressGetList(AddressRequest request);

        [OperationContract]
        ResponseBase FamilySet(FamilyRequest request);
        [OperationContract]
        ResponseBase FamilySetList(FamilyRequest request);
        [OperationContract]
        FamilyResponse FamilyGetList(FamilyRequest request);

        [OperationContract]
        ResponseBase JobSet(JobRequest request);
        [OperationContract]
        ResponseBase JobSetList(JobRequest request);
        [OperationContract]
        JobResponse JobGetList(JobRequest request);

        [OperationContract]
        ResponseBase AssetSet(AssetRequest request);
        [OperationContract]
        ResponseBase AssetSetList(AssetRequest request);
        [OperationContract]
        AssetResponse AssetGetList(AssetRequest request);
        #endregion

        #region Document
        [OperationContract]
        ResponseBase DocumentSet(DocumentRequest request);
        [OperationContract]
        DocumentResponse DocumentGet(DocumentRequest request);
        [OperationContract]
        DocumentResponse DocumentGetList(DocumentRequest request); 
        #endregion
    }
}
