using Araneo.Common;
using CharityManager.DTO;
using CharityManager.Service.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using static CharityManager.Service.Mapper;

namespace CharityManager.Service
{
    public partial class Charity : ICharity
    {
        private const string BAD_REQ_USER_MESSAGE = "درخواست ارسالی نامعتبر است";
        private const string INVALID_REQ_USER_MESSAGE = "اطلاعات ارسال شده کامل نیست";

        #region Person
        public ResponseBase PersonSet(PersonRequest request)
        {
            try
            {
                NullSafeCheck(request, "PersonRequest");
                NullSafeCheck(request.DTO, "PersonDTO");
                if (string.IsNullOrWhiteSpace(request.DTO.NationalNo))
                    return new ResponseBase(false, -1, "NationalNo is required", "وارد کردن شماره ملی الزامی ست");

                if (request.DTO.ID == 0)
                {
                    var personReq = new PersonRequest { Filter = new PersonFilter { NationalNo = request.DTO.NationalNo } };
                    var personRes = PersonGet(personReq);
                    if (personRes?.Result != null)
                        return new ResponseBase(false, -1, $"NationalNo '{request.DTO.NationalNo}' already exist", "این شماره ملی قبلا ثبت شده است");
                }

                var result = EntitySet<DTO.PersonDTO, Person>(request.DTO);
                return new ResponseBase(true, result);
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                return ExceptionToBaseResponse(ex);
            }
        }
        public PersonResponse PersonGet(PersonRequest request)
        {
            try
            {
                PersonDTO result = null;
                using (var context = new CharityEntities())
                {
                    var query = context.People.AsNoTracking().AsQueryable();
                    if (request.Filter.ID > 0)
                        query = query.Where(p => p.Active && p.ID == request.Filter.ID);
                    else
                        query = query.Where(p => p.NationalNo == request.Filter.NationalNo);

                    result = query.Select(PersonDTOMapper).FirstOrDefault();
                }

                if (result == null)
                    throw new CustomException("نتیجه ای یافت نشد", "Person not found");


                return new PersonResponse { Success = result != null, ResultID = result?.ID ?? 0, Result = result };
            }
            catch (Exception ex)
            {
                var response = ExceptionToBaseResponse(ex);
                return response.Cast<PersonResponse>();
            }
        }
        public PersonResponse PersonGetList(PersonRequest request)
        {
            List<PersonDTO> result = null;
            try
            {
                NullSafeCheck(request);
                NullSafeCheck(request.Filter);
                var filter = request.Filter;

                using (var context = new CharityEntities())
                {
                    var query = context.People.AsNoTracking().AsQueryable();
                    if (filter.ID > 0)
                        query = query.Where(p => p.Active && p.ID == filter.ID);
                    else if (filter.IDList?.Count > 0)
                    {
                        query = query.Where(p => p.Active && filter.IDList.Contains(p.ID));
                    }
                    else
                    {
                        if (filter.Active != null)
                            query = query.Where(p => p.Active == filter.Active);
                        if (filter.CreateDate?.From != null)
                            query = query.Where(p => p.CreateDate >= filter.CreateDate.From);
                        if (filter.CreateDate?.To != null)
                            query = query.Where(p => p.CreateDate <= filter.CreateDate.To);

                        query = query.Where(p => p.NationalNo.Contains(filter.NationalNo));
                        query = query.Where(p => p.FirstName.StartsWith(filter.FirstName));
                        query = query.Where(p => p.LastName.StartsWith(filter.LastName));
                    }
                    result = query.Select(PersonDTOMapper).ToList();

                }

                return new PersonResponse { Success = true, ResultList = result };
            }
            catch (Exception ex)
            {
                var response = ExceptionToBaseResponse(ex);
                return response.Cast<PersonResponse>();
            }
        }
        public ResponseBase PersonPictureSet(PersonRequest request)
        {
            try
            {
                NullSafeCheck(request, "PersonRequest");
                NullSafeCheck(request.Picture, "Picture");

                if (request.Picture.PersonID <= 0)
                    throw new CustomException(BAD_REQ_USER_MESSAGE, $"PersonID {request.Picture.PersonID} is not valid");

                using (var context = new CharityEntities())
                {
                    var picture = context.Pictures.FirstOrDefault(p => p.PersonID == request.Picture.PersonID);
                    request.Picture.ID = picture?.ID ?? 0;
                }
                var result = EntitySet<PictureDTO, Picture>(request.Picture);
                return new ResponseBase(true, result);
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                return ExceptionToBaseResponse(ex);
            }
        }
        public PictureResponse PersonPictureGet(PersonRequest request)
        {
            try
            {
                NullSafeCheck(request, "Request");
                NullSafeCheck(request.Filter, "Filter");

                var filter = request.Filter;
                IEnumerable<PictureDTO> result = null;

                using (var context = new CharityEntities())
                {
                    var query = context.Pictures.AsNoTracking().AsQueryable();

                    if (filter.ID > 0)
                        query = query.Where(p => p.PersonID == filter.ID);
                    else if (filter.IDList?.Count > 0)
                        query = query.Where(p => filter.IDList.Contains(p.PersonID));
                    else if (filter.NationalNo?.Length == 10)
                    {
                        var id = context.People.FirstOrDefault(p => p.NationalNo.Equals(filter.NationalNo))?.ID ?? 0;
                        query = query.Where(p => p.PersonID == id);
                    }
                    else
                        throw new CustomException("فیلتری یافت نشد. برای دریافت همه تصاویر از سرویس دیگری استفاده کنید", "No Filter found!");

                    result = query.Select(PictureDTOMapper);
                    if (result == null)
                        throw new CustomException("تصویری یافت نشد", $"No picture found for personId '{request.Filter.ID}'");
                    return new PictureResponse { Success = true, ResultList = result.ToList() };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                return ExceptionToBaseResponse(ex).Cast<PictureResponse>();
            }
        }
        #endregion

        #region User
        public ResponseBase UserSet(UserRequest request)
        {
            try
            {
                NullSafeCheck(request);

                if (request.DTO.PersonID <= 0)
                    throw new CustomException(BAD_REQ_USER_MESSAGE, $"PersonID {request.DTO.PersonID} is not valid");

                if (request.DTO.ID == 0)
                {
                    var userReq = new UserRequest { Filter = new UserFilter { PersonID = request.DTO.PersonID } };
                    var user = UserGet(userReq);
                    if (user.Result != null)
                        throw new CustomException("اطلاعات فردی برای کاربر دیگری استفاده شده است",
                            $"PersonID '{request.DTO.PersonID}' is bound to UserID '{user.ResultID}'");
                    userReq.Filter = new UserFilter { UserName = request.DTO.UserName };
                    user = UserGet(userReq);
                    if (user.Result != null)
                        throw new CustomException("این نام کاربری قبلا ثبت شده است", $"User with username '{request.DTO.UserName}' already exist");

                    var personReq = new PersonRequest { Filter = new PersonFilter { ID = request.DTO.PersonID } };
                    var personRes = PersonGet(personReq);
                    if (string.IsNullOrEmpty(request.DTO.Password))
                        request.DTO.Password = HashPassword(personRes.Result.NationalNo);
                    else
                        request.DTO.Password = HashPassword(request.DTO.Password);
                }
                var result = EntitySet<UserDTO, User>(request.DTO);
                return new ResponseBase(true, result);
            }
            catch (Exception ex)
            {
                return ExceptionToBaseResponse(ex);
            }
        }
        public UserResponse UserGet(UserRequest request)
        {
            try
            {
                NullSafeCheck(request);
                NullSafeCheck(request.Filter);


                using (var context = new CharityEntities())
                {
                    var query = context.Users.AsNoTracking().AsQueryable();
                    if (request.Filter.ID > 0)
                        query = query.Where(u => u.ID == request.Filter.ID);
                    else if (!string.IsNullOrEmpty(request.Filter.UserName))
                        query = query.Where(u => u.UserName.ToLower() == request.Filter.UserName.ToLower());
                    else if (request.Filter.PersonID > 0)
                        query = query.Where(u => u.PersonID == request.Filter.PersonID);

                    var user = UserDTOMapper(query.FirstOrDefault());
                    return new UserResponse { Success = user != null, ResultID = user?.ID ?? 0, Result = user };
                }
            }
            catch (Exception ex)
            {
                var response = ExceptionToBaseResponse(ex);
                return response.Cast<UserResponse>();
            }
        }
        public UserResponse UserGetList(UserRequest request)
        {
            List<UserDTO> result = null;
            try
            {
                NullSafeCheck(request);
                NullSafeCheck(request.Filter, "Filter");

                var filter = request.Filter;

                using (var context = new CharityEntities())
                {
                    var query = context.Users.AsNoTracking().AsQueryable();
                    if (filter.ID > 0)
                        query = query.Where(p => p.Active && p.ID == filter.ID);
                    else
                    {
                        if (filter.Active != null)
                            query = query.Where(u => u.Active == filter.Active);
                        if (filter.CreateDate?.From != null)
                            query = query.Where(u => u.CreateDate >= filter.CreateDate.From);
                        if (filter.CreateDate?.To != null)
                            query = query.Where(u => u.CreateDate <= filter.CreateDate.To);
                        if (request.Filter?.UserName?.Length > 0)
                            query = query.Where(u => u.UserName.StartsWith(filter.UserName));
                    }
                    result = query.Select(UserDTOMapper).ToList();
                }

                return new UserResponse { Success = true, ResultList = result };
            }
            catch (Exception ex)
            {
                var response = ExceptionToBaseResponse(ex);
                return response.Cast<UserResponse>();
            }
        }

        public ResponseBase Login(UserRequest request)
        {
            try
            {
                NullSafeCheck(request);
                var loginDto = new LoginDTO { CreateUser = request.DTO.ID };
                var result = EntitySet<LoginDTO, Login>(loginDto);
                return new ResponseBase(true, result);
            }
            catch (Exception e)
            {
                return ExceptionToBaseResponse(e);
            }

        }
        #endregion

        #region Patron
        public ResponseBase PatronSet(PatronRequest request)
        {
            try
            {
                NullSafeCheck(request, "PatronRequest");
                NullSafeCheck(request.DTO, "DTO");

                if (request.DTO.PersonID <= 0)
                    throw new CustomException(BAD_REQ_USER_MESSAGE, $"PersonID {request.DTO.PersonID} is not valid");

                var result = EntitySet<PatronDTO, Patron>(request.DTO);
                return new ResponseBase(true, result);
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                return ExceptionToBaseResponse(ex);
            }
        }
        public PatronResponse PatronGet(PatronRequest request)
        {
            try
            {
                NullSafeCheck(request, "Request");
                NullSafeCheck(request.Filter, "Filter");

                if (request.Filter.ID <= 0 && request.Filter.PersonID <= 0 && string.IsNullOrEmpty(request.Filter.NationalNo))
                    throw new CustomException(INVALID_REQ_USER_MESSAGE, "ID,PersonID and NationalNo is invalid");

                PatronDTO result = null;
                using (var context = new CharityEntities())
                {
                    var query = context.Patrons.AsNoTracking().AsQueryable();

                    if (request.Filter.ID > 0)
                        query = query.Where(p => p.Active && p.ID == request.Filter.ID);

                    else if (request.Filter.PersonID > 0)
                        query = query.Where(p => p.PersonID == request.Filter.PersonID);

                    else if (string.IsNullOrEmpty(request.Filter.NationalNo) == false)
                    {
                        var person = context.People.AsNoTracking().FirstOrDefault(p => p.NationalNo == request.Filter.NationalNo);
                        query = query.Where(p => p.PersonID == person.ID);
                    }

                    result = query.Select(PatronDTOMapper).FirstOrDefault();

                    if (request.LoadPerson)
                    {
                        var people = context.People.AsNoTracking();
                        var person = people.FirstOrDefault(p => p.ID == result.PersonID);
                        result.Person = PersonDTOMapper(person);
                    }
                }

                if (result == null)
                    throw new CustomException("نتیجه ای یافت نشد", "Patron not found");

                return new PatronResponse { Success = result != null, ResultID = result?.ID ?? 0, Result = result };
            }
            catch (Exception ex)
            {
                var response = ExceptionToBaseResponse(ex);
                return response.Cast<PatronResponse>();
            }
        }
        public PatronResponse PatronGetList(PatronRequest request)
        {
            List<PatronDTO> result = null;
            try
            {
                NullSafeCheck(request, "Request");
                NullSafeCheck(request.Filter, "Filter");

                using (var context = new CharityEntities())
                {
                    var filter = request.Filter;

                    var query = context.Patrons.AsNoTracking().AsQueryable();
                    if (filter.ID > 0)
                        query = query.Where(p => p.Active && p.ID == filter.ID);
                    else
                    {
                        if (filter.Active != null)
                            query = query.Where(p => p.Active == filter.Active);
                        if (filter.CreateDate?.From != null)
                            query = query.Where(p => p.CreateDate >= filter.CreateDate.From);
                        if (filter.CreateDate?.To != null)
                            query = query.Where(p => p.CreateDate <= filter.CreateDate.To);
                        if (filter.PersonID > 0)
                            query = query.Where(p => p.PersonID == filter.PersonID);
                    }
                    result = query.Select(PatronDTOMapper).ToList();

                    if (request.LoadPerson)
                    {
                        var persons = context.People.AsNoTracking();
                        result.ForEach(patron =>
                        {
                            var person = persons.FirstOrDefault(p => p.ID == patron.PersonID);
                            patron.Person = PersonDTOMapper(person);
                        });
                    }
                }

                return new PatronResponse { Success = true, ResultList = result };
            }
            catch (Exception ex)
            {
                var response = ExceptionToBaseResponse(ex);
                return response.Cast<PatronResponse>();
            }
        }
        public ResponseBase PatronIntroducerSet(PatronRequest request)
        {
            try
            {
                NullSafeCheck(request, "PatronRequest");
                NullSafeCheck(request.DTO, "DTO");

                if (request.DTO.ID <= 0)
                    throw new CustomException(BAD_REQ_USER_MESSAGE, $"Patron ID {request.DTO.ID} is not valid");
                if ((request.DTO.IntroducerID ?? 0) <= 0)
                    throw new CustomException(BAD_REQ_USER_MESSAGE, $"IntroducerID {request.DTO.IntroducerID} is not valid");

                using (var context = new CharityEntities())
                {
                    var patron = context.Patrons.FirstOrDefault(p => p.ID == request.DTO.ID);
                    patron.IntroducerID = request.DTO.IntroducerID;

                    context.SaveChanges();
                }

                return new ResponseBase(true, 1);
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                return ExceptionToBaseResponse(ex);
            }
        }
        #endregion

        #region Address
        public ResponseBase AddressSet(AddressRequest request)
        {
            try
            {
                NullSafeCheck(request, "AddressRequest");
                NullSafeCheck(request.DTO, "AddressDTO");
                var result = EntitySet<AddressDTO, Address>(request.DTO);
                return new ResponseBase(true, result);
            }
            catch (Exception ex)
            {
                return ExceptionToBaseResponse(ex);
            }
        }
        public ResponseBase AddressDelete(AddressRequest request)
        {
            try
            {
                EntityDelete<AddressDTO, Address>(request.DTO.ID);
                return new ResponseBase(true, 1);
            }
            catch (Exception ex)
            {
                return new ResponseBase(false, -1, ex.Message, "حذف آدرس با شکست مواجه شد");
            }
        }
        public ResponseBase AddressSetList(AddressRequest request)
        {
            try
            {
                NullSafeCheck(request, "AddressRequest");
                NullSafeCheck(request.DTOList, "DTOList");
                foreach (var item in request.DTOList)
                    EntitySet<AddressDTO, Address>(item);
                return new ResponseBase(true, 0);
            }
            catch (Exception ex)
            {
                return ExceptionToBaseResponse(ex);
            }
        }
        public AddressResponse AddressGetList(AddressRequest request)
        {
            List<AddressDTO> result = null;
            try
            {
                NullSafeCheck(request, "AddressRequest");

                var filter = request.Filter;

                using (var context = new CharityEntities())
                {
                    var query = context.Addresses.AsNoTracking().AsQueryable();
                    if (filter != null)
                    {
                        if (filter.ID > 0)
                            query = query.Where(p => p.Active && p.ID == filter.ID);
                        else
                        {
                            if (filter.Active != null)
                                query = query.Where(p => p.Active == filter.Active);
                            if (filter.CreateDate?.From != null)
                                query = query.Where(p => p.CreateDate >= filter.CreateDate.From);
                            if (filter.CreateDate?.To != null)
                                query = query.Where(p => p.CreateDate <= filter.CreateDate.To);

                            query = query.Where(p => p.PatronID == request.Filter.PatronID);
                        }
                    }
                    result = query.Select(AddressDTOMapper).ToList();
                }

                return new AddressResponse { Success = true, ResultList = result };
            }
            catch (Exception ex)
            {
                var response = ExceptionToBaseResponse(ex);
                return response.Cast<AddressResponse>();
            }
        }
        #endregion

        #region Family
        public ResponseBase FamilySet(FamilyRequest request)
        {
            try
            {
                NullSafeCheck(request, "FamilyRequest");
                NullSafeCheck(request.DTO, "FamilyDTO");
                var result = EntitySet<FamilyDTO, Family>(request.DTO);
                return new ResponseBase(true, result);
            }
            catch (Exception ex)
            {
                return ExceptionToBaseResponse(ex);
            }
        }
        public ResponseBase FamilySetList(FamilyRequest request)
        {
            try
            {
                NullSafeCheck(request, "FamilyRequest");
                NullSafeCheck(request.DTOList, "DTOList");
                foreach (var item in request.DTOList)
                    EntitySet<FamilyDTO, Family>(item);
                return new ResponseBase(true, 0);
            }
            catch (Exception ex)
            {
                return ExceptionToBaseResponse(ex);
            }
        }
        public FamilyResponse FamilyGetList(FamilyRequest request)
        {
            List<FamilyDTO> result = null;
            try
            {
                NullSafeCheck(request, "FamilyRequest");

                var filter = request.Filter;

                using (var context = new CharityEntities())
                {
                    var query = context.Families.AsNoTracking().AsQueryable();
                    if (filter != null)
                    {
                        if (filter.ID > 0)
                            query = query.Where(p => p.Active && p.ID == filter.ID);
                        else
                        {
                            if (filter.Active != null)
                                query = query.Where(p => p.Active == filter.Active);
                            if (filter.CreateDate?.From != null)
                                query = query.Where(p => p.CreateDate >= filter.CreateDate.From);
                            if (filter.CreateDate?.To != null)
                                query = query.Where(p => p.CreateDate <= filter.CreateDate.To);

                            query = query.Where(p => p.PatronID == request.Filter.PatronID);
                        }
                    }
                    result = query.Select(FamilyDTOMapper).ToList();
                }

                return new FamilyResponse { Success = true, ResultList = result };
            }
            catch (Exception ex)
            {
                var response = ExceptionToBaseResponse(ex);
                return response.Cast<FamilyResponse>();
            }
        }
        public ResponseBase FamilyDelete(FamilyRequest request)
        {
            try
            {
                EntityDelete<FamilyDTO, Family>(request.DTO.ID);
                return new ResponseBase(true, 1);
            }
            catch (Exception ex)
            {
                return new ResponseBase(false, -1, ex.Message, "حذف فرد تحت تکفل با شکست مواجه شد");
            }
        }
        #endregion

        #region Job
        public ResponseBase JobSet(JobRequest request)
        {
            try
            {
                NullSafeCheck(request, "JobRequest");
                NullSafeCheck(request.DTO, "JobDTO");
                var result = EntitySet<JobDTO, Job>(request.DTO);
                return new ResponseBase(true, result);
            }
            catch (Exception ex)
            {
                return ExceptionToBaseResponse(ex);
            }
        }
        public ResponseBase JobSetList(JobRequest request)
        {
            try
            {
                NullSafeCheck(request, "JobRequest");
                NullSafeCheck(request.DTOList, "DTOList");
                foreach (var item in request.DTOList)
                    EntitySet<JobDTO, Job>(item);
                return new ResponseBase(true, 0);
            }
            catch (Exception ex)
            {
                return ExceptionToBaseResponse(ex);
            }
        }
        public JobResponse JobGetList(JobRequest request)
        {
            List<JobDTO> result = null;
            try
            {
                NullSafeCheck(request, "JobRequest");

                var filter = request.Filter;

                using (var context = new CharityEntities())
                {
                    var query = context.Jobs.AsNoTracking().AsQueryable();
                    if (filter != null)
                    {
                        if (filter.ID > 0)
                            query = query.Where(p => p.Active && p.ID == filter.ID);
                        else
                        {
                            if (filter.Active != null)
                                query = query.Where(p => p.Active == filter.Active);
                            if (filter.CreateDate?.From != null)
                                query = query.Where(p => p.CreateDate >= filter.CreateDate.From);
                            if (filter.CreateDate?.To != null)
                                query = query.Where(p => p.CreateDate <= filter.CreateDate.To);

                            query = query.Where(p => p.PatronID == request.Filter.PatronID);
                        }
                    }
                    result = query.Select(JobDTOMapper).ToList();
                }

                return new JobResponse { Success = true, ResultList = result };
            }
            catch (Exception ex)
            {
                var response = ExceptionToBaseResponse(ex);
                return response.Cast<JobResponse>();
            }
        }
        public ResponseBase JobDelete(JobRequest request)
        {
            try
            {
                EntityDelete<JobDTO, Job>(request.DTO.ID);
                return new ResponseBase(true, 1);
            }
            catch (Exception ex)
            {
                return new ResponseBase(false, -1, ex.Message, "حذف اطلاعات شغلی با شکست مواجه شد");
            }
        }
        #endregion

        #region Introducer
        public ResponseBase IntroducerSet(IntroducerRequest request)
        {
            try
            {
                NullSafeCheck(request, "IntroducerRequest");
                NullSafeCheck(request.DTO, "IntroducerDTO");
                UserCheck(request, request.DTO);
                var result = EntitySet<IntroducerDTO, Introducer>(request.DTO);
                return new ResponseBase(true, result);
            }
            catch (Exception ex)
            {
                return ExceptionToBaseResponse(ex);
            }
        }
        public IntroducerResponse IntroducerGetList(IntroducerRequest request)
        {
            List<IntroducerDTO> result = null;
            try
            {
                NullSafeCheck(request, "IntroducerRequest");
                UserCheck(request);

                var filter = request.Filter;

                using (var context = new CharityEntities())
                {
                    var query = context.Introducers.AsNoTracking().AsQueryable();
                    if (filter != null)
                    {
                        if (filter.ID > 0)
                            query = query.Where(p => p.Active && p.ID == filter.ID);
                        if (filter.PersonID > 0)
                            query = query.Where(p => p.Active && p.PersonID == filter.PersonID);
                        else
                        {
                            if (filter.Active != null)
                                query = query.Where(p => p.Active == filter.Active);
                            if (filter.CreateDate?.From != null)
                                query = query.Where(p => p.CreateDate >= filter.CreateDate.From);
                            if (filter.CreateDate?.To != null)
                                query = query.Where(p => p.CreateDate <= filter.CreateDate.To);
                        }
                    }
                    result = query.Select(IntroducerDTOMapper).ToList();

                    if (request.LoadPerson)
                    {
                        var people = context.People.AsNoTracking();
                        result.ForEach(i => i.Person = PersonDTOMapper.Invoke(people.FirstOrDefault(p => p.ID == i.PersonID)));
                    }
                }

                return new IntroducerResponse { Success = true, ResultList = result };
            }
            catch (Exception ex)
            {
                var response = ExceptionToBaseResponse(ex);
                return response.Cast<IntroducerResponse>();
            }
        }
        #endregion

        #region Asset
        public ResponseBase AssetSet(AssetRequest request)
        {
            try
            {
                NullSafeCheck(request, "AssetRequest");
                NullSafeCheck(request.DTO, "AssetDTO");
                var result = EntitySet<AssetDTO, Asset>(request.DTO);
                return new ResponseBase(true, result);
            }
            catch (Exception ex)
            {
                return ExceptionToBaseResponse(ex);
            }
        }
        public ResponseBase AssetSetList(AssetRequest request)
        {
            try
            {
                NullSafeCheck(request, "AssetRequest");
                NullSafeCheck(request.DTOList, "DTOList");
                foreach (var item in request.DTOList)
                    EntitySet<AssetDTO, Asset>(item);
                return new ResponseBase(true, 0);
            }
            catch (Exception ex)
            {
                return ExceptionToBaseResponse(ex);
            }
        }
        public AssetResponse AssetGetList(AssetRequest request)
        {
            List<AssetDTO> result = null;
            try
            {
                NullSafeCheck(request, "AssetRequest");

                var filter = request.Filter;

                using (var context = new CharityEntities())
                {
                    var query = context.Assets.AsNoTracking().AsQueryable();
                    if (filter != null)
                    {
                        if (filter.ID > 0)
                            query = query.Where(p => p.Active && p.ID == filter.ID);
                        else
                        {
                            if (filter.Active != null)
                                query = query.Where(p => p.Active == filter.Active);
                            if (filter.CreateDate?.From != null)
                                query = query.Where(p => p.CreateDate >= filter.CreateDate.From);
                            if (filter.CreateDate?.To != null)
                                query = query.Where(p => p.CreateDate <= filter.CreateDate.To);

                            query = query.Where(p => p.PatronID == request.Filter.PatronID);
                        }
                    }
                    result = query.Select(AssetDTOMapper).ToList();
                }

                return new AssetResponse { Success = true, ResultList = result };
            }
            catch (Exception ex)
            {
                var response = ExceptionToBaseResponse(ex);
                return response.Cast<AssetResponse>();
            }
        }
        public ResponseBase AssetDelete(AssetRequest request)
        {
            try
            {
                EntityDelete<AssetDTO, Asset>(request.DTO.ID);
                return new ResponseBase(true, 1);
            }
            catch (Exception ex)
            {
                return new ResponseBase(false, -1, ex.Message, "حذف دارایی با شکست مواجه شد");
            }
        }
        #endregion

        #region Entity
        public EntityResponse EntityGetList(EntityRequest request)
        {
            List<EntityDTO> result = null;
            try
            {
                NullSafeCheck(request, "EntityRequest");

                var filter = request.Filter;

                using (var context = new CharityEntities())
                {
                    var query = context.Entities.AsNoTracking().AsQueryable();
                    if (filter != null)
                    {
                        if (filter.ID > 0)
                            query = query.Where(p => p.ID == filter.ID);
                        else
                        {
                            if (filter.Key?.Length > 0)
                                query = query.Where(p => p.EntityKey.ToLower() == filter.Key.ToLower());
                        }
                    }
                    result = query.Select(EntityDTOMapper).ToList();
                }

                return new EntityResponse { Success = true, ResultList = result };
            }
            catch (Exception ex)
            {
                var response = ExceptionToBaseResponse(ex);
                return response.Cast<EntityResponse>();
            }
        }
        #endregion

        #region ManagerViewPoint
        public ResponseBase ManagerViewPointSet(AssetRequest request)
        {
            try
            {
                NullSafeCheck(request, "AssetRequest");
                NullSafeCheck(request.DTO, "AssetDTO");
                var result = EntitySet<AssetDTO, Asset>(request.DTO);
                return new ResponseBase(true, result);
            }
            catch (Exception ex)
            {
                return ExceptionToBaseResponse(ex);
            }
        }

        #endregion

        private int EntitySet<T, E>(T dto) where T : DTO.DTOBase
        {
            using (var context = new CharityEntities())
            {
                if (dto.ID == 0)
                {
                    dto.CreateDate = DateTime.Now;
                    dto.Active = true;
                }
                else
                    dto.ModifyDate = DateTime.Now;

                E entity = Map<T, E>(dto);
                var entry = context.Entry(entity);
                entry.State = dto.ID == 0 ? System.Data.Entity.EntityState.Added : System.Data.Entity.EntityState.Modified;
                if (dto.ID > 0)
                {
                    entry.Property("CreateDate").IsModified = false;
                    entry.Property("CreateUser").IsModified = false;
                }
                context.SaveChanges();
                return (int)entry.Property("ID").OriginalValue;
            }
        }
        private bool EntityDelete<T, E>(int id) where T : DTOBase
        {
            using (var context = new CharityEntities())
            {
                T dto = Activator.CreateInstance<T>();
                dto.ID = id;
                dto.Active = false;
                dto.ModifyDate = DateTime.Now;

                E entity = Map<T, E>(dto);

                var entry = context.Entry(entity);
                var activeProp = entry.Property("Active");

                if (activeProp != null)
                {
                    entry.State = System.Data.Entity.EntityState.Modified;
                    foreach (var item in entity.GetType().GetProperties())
                    {
                        if (item.GetGetMethod().IsVirtual)
                            continue;
                        if (item.Name == "Active" || item.Name == "ModifyDate" || item.Name == "State" || item.Name == "ID")
                            continue;
                        var prop = entry.Property(item.Name);
                        if (prop != null)
                            prop.IsModified = false;
                    }
                }
                else
                    entry.State = System.Data.Entity.EntityState.Deleted;

                context.SaveChanges();
                return true;
            }
        }
        private string HashPassword(string pass)
        {
            using (var cryptor = new SHA1CryptoServiceProvider())
                return Convert.ToBase64String(cryptor.ComputeHash(Encoding.ASCII.GetBytes(pass)));
        }
        private ResponseBase ExceptionToBaseResponse(Exception ex)
        {
            var response = new ResponseBase(false, -1, ex.Message, "عملیات با شکست مواجه شد");
            if (ex is CustomException ce)
            {
                response.UserMessage = ce.UserMessage;
                response.Message = ce.Message;
            }
            return response;
        }
        private void NullSafeCheck(object obj, string name = null)
        {
            if (obj == null) throw new CustomException(BAD_REQ_USER_MESSAGE, $"{name ?? obj.GetType().Name} is null");
        }
        private void UserCheck(RequestBase request, DTOBase dto = null)
        {
            if (request.UserID <= 0)
                throw new CustomException("کاربر درخواست کننده نامعتبر است.", $"UserID '{request.UserID}' is not valid");
            var userReq = new UserRequest { Filter = new UserFilter { ID = request.UserID } };
            var response = UserGet(userReq);
            if (!(response?.Success == true))
                throw new CustomException("کاربری با شناسه ارسالی یافت نشد", $"UserID '{request.UserID}' is not bound to any user.");
            else if (dto != null)
            {
                if (dto.ID == 0)
                    dto.CreateUser = request.UserID;
                else
                    dto.ModifyUser = request.UserID;
            }
        }
    }
}
