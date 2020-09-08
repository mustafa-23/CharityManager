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

        #region User
        [OperationContract]
        ResponseBase UserSet(UserRequest request);
        [OperationContract]
        UserResponse UserGet(UserRequest request);
        [OperationContract]
        UserResponse UserGetList(UserRequest request);
        [OperationContract]
        ResponseBase Login(UserRequest request); 
        [OperationContract]
        LoginResponse GetUserLogins(UserRequest request); 
        #endregion

        [OperationContract]
        EntityResponse EntityGetList(EntityRequest request);

        #region Introducer
        [OperationContract]
        ResponseBase IntroducerSet(IntroducerRequest request);

        [OperationContract]
        IntroducerResponse IntroducerGetList(IntroducerRequest request);
        #endregion

        #region Patron
        [OperationContract]
        ResponseBase PatronSet(PatronRequest request);
        [OperationContract]
        PatronResponse PatronGet(PatronRequest request);
        [OperationContract]
        PatronResponse PatronGetList(PatronRequest request);
        //Address
        [OperationContract]
        ResponseBase AddressSet(AddressRequest request);
        [OperationContract]
        ResponseBase AddressDelete(AddressRequest request);
        [OperationContract]
        ResponseBase AddressSetList(AddressRequest request);
        [OperationContract]
        AddressResponse AddressGetList(AddressRequest request);
        //Family
        [OperationContract]
        ResponseBase FamilySet(FamilyRequest request);
        [OperationContract]
        ResponseBase FamilySetList(FamilyRequest request);
        [OperationContract]
        FamilyResponse FamilyGetList(FamilyRequest request);
        [OperationContract]
        ResponseBase FamilyDelete(FamilyRequest request);
        //Job
        [OperationContract]
        ResponseBase JobSet(JobRequest request);
        [OperationContract]
        ResponseBase JobSetList(JobRequest request);
        [OperationContract]
        JobResponse JobGetList(JobRequest request);
        [OperationContract]
        ResponseBase JobDelete(JobRequest request);
        //Asset
        [OperationContract]
        ResponseBase AssetSet(AssetRequest request);
        [OperationContract]
        ResponseBase AssetSetList(AssetRequest request);
        [OperationContract]
        AssetResponse AssetGetList(AssetRequest request);
        [OperationContract]
        ResponseBase AssetDelete(AssetRequest request);
        
        /// <summary>
        /// ثبت معرف حمایت شونده
        /// </summary>
        /// <param name="request">فقط ID حمایت شونده و معرف داخل DTO پر شود</param>
        [OperationContract]
        ResponseBase PatronIntroducerSet(PatronRequest request);
        #endregion

        #region Document
        [OperationContract]
        ResponseBase DocumentSet(DocumentRequest request);
        [OperationContract]
        DocumentResponse DocumentGet(DocumentRequest request);
        [OperationContract]
        DocumentResponse DocumentGetList(DocumentRequest request);
        #endregion

        #region Request
        [OperationContract]
        RequestResponse RequestLastNo();
        [OperationContract]
        ResponseBase RequestSet(RequestRequest request);
        [OperationContract]
        RequestResponse RequestGetList(RequestRequest request);
        /// <summary>
        /// ثبت درخواست انجام تحقیقات بر روی درخواست کمک ثبت شده
        /// </summary>
        [OperationContract]
        ResponseBase RequestResearch(RequestRequest request);
        /// <summary>
        /// ثبت نتیجه تحقیقات
        /// </summary>
        [OperationContract]
        ResponseBase ResearchSet(ResearchRequest request);
        /// <summary>
        /// دریافت لیست تحقیقات
        /// </summary>
        [OperationContract]
        ResearchResponse ResearchGetList(ResearchRequest request);
        #endregion
    }
}
