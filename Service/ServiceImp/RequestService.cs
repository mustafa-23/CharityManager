using CharityManager.DTO;
using CharityManager.Service.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CharityManager.Service
{
    public partial class Charity : ICharity
    {
        public RequestResponse RequestLastNo()
        {
            try
            {
                using (var context = new CharityEntities())
                {
                    int no = 0;
                    if (context.Requests.Count() > 0)
                        no = context.Requests.AsNoTracking().Select(r => int.Parse(r.No)).Max();
                    return new RequestResponse { Success = true, MaxNo = no };
                }
            }
            catch (System.Exception ex)
            {
                return new RequestResponse { Success = false, Message = ex.Message, UserMessage = "خطا هنگام دریافت داده" };
            }
        }
        public ResponseBase RequestSet(RequestRequest request)
        {
            try
            {
                NullSafeCheck(request, "RequestRequest");
                NullSafeCheck(request.DTO, "RequestDTO");
                UserCheck(request, request.DTO);
                var result = EntitySet<RequestDTO, Request>(request.DTO);
                return new ResponseBase(true, result);
            }
            catch (Exception ex)
            {
                return ExceptionToBaseResponse(ex);
            }
        }
        public ResponseBase ResearchSet(ResearchRequest request)
        {
            try
            {
                NullSafeCheck(request, "RequestResearch");
                NullSafeCheck(request.DTO, "ResearchDTO");
                UserCheck(request, request.DTO);
                var result = EntitySet<ResearchDTO, Research>(request.DTO);
                return new ResponseBase(true, result);
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                return ExceptionToBaseResponse(ex);
            }
        }
        public RequestResponse RequestGetList(RequestRequest request)
        {
            List<RequestDTO> result = null;
            try
            {
                NullSafeCheck(request, "RequestRequest");

                var filter = request.Filter;

                using (var context = new CharityEntities())
                {
                    var query = context.Requests.AsNoTracking().AsQueryable();
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
                    result = query.Select(Mapper.RequestDTOMapper).ToList();
                }

                return new RequestResponse { Success = true, ResultList = result };
            }
            catch (Exception ex)
            {
                var response = ExceptionToBaseResponse(ex);
                return response.Cast<RequestResponse>();
            }
        }
        public ResponseBase RequestResearch(RequestRequest request)
        {
            try
            {
                NullSafeCheck(request, "RequestRequest");
                NullSafeCheck(request.DTO, "RequestDTO");
                UserCheck(request, request.DTO);
                using(var context = new CharityEntities())
                {
                    var entity = context.Requests.FirstOrDefault(r => r.ID == request.DTO.ID);
                    entity.ResearchDate = DateTime.Now;
                    context.SaveChanges();
                }
                return new ResponseBase(true,1);
            }
            catch (Exception ex)
            {
                return ExceptionToBaseResponse(ex);
            }
        }
        public ResearchResponse ResearchGetList(ResearchRequest request)
        {
            List<ResearchDTO> result = null;
            try
            {
                NullSafeCheck(request, "ResearchRequest");

                var filter = request.Filter;

                using (var context = new CharityEntities())
                {
                    var query = context.Researches.AsNoTracking().AsQueryable();
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
                            if(filter.RequestID > 0)
                                query = query.Where(p => p.RequestID == filter.RequestID);

                            
                        }
                    }
                    result = query.Select(Mapper.ResearchDTOMapper).ToList();
                }

                return new ResearchResponse { Success = true, ResultList = result };
            }
            catch (Exception ex)
            {
                var response = ExceptionToBaseResponse(ex);
                return response.Cast<ResearchResponse>();
            }
        }
    }
}
