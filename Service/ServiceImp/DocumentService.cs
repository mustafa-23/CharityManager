using CharityManager.DTO;
using CharityManager.Service.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using static CharityManager.Service.Mapper;

namespace CharityManager.Service
{
    public partial class Charity
    {
        public ResponseBase DocumentSet(DocumentRequest request)
        {
            try
            {
                NullSafeCheck(request, "DocumentRequest");
                NullSafeCheck(request.DTO, "DocumentDTO");
                var result = EntitySet<DocumentDTO, Document>(request.DTO);
                return new ResponseBase(true, result);
            }
            catch (Exception ex)
            {
                StringBuilder msg = new StringBuilder();
                if (ex is DbEntityValidationException dbex)
                    foreach (var error in dbex.EntityValidationErrors)
                    {
                        var errors = error.ValidationErrors.Select(e => $"({e.PropertyName},{e.ErrorMessage})");
                        msg.AppendLine(string.Join(",", errors));
                    }
                if (msg.Length > 0)
                    return new ResponseBase(false, -1, msg.ToString(), "ثبت مدرک با مشکل مواجه شد");
                else
                    return ExceptionToBaseResponse(ex);
            }
        }
        public DocumentResponse DocumentGet(DocumentRequest request)
        {
            try
            {
                DocumentDTO result = null;

                NullSafeCheck(request, "DocumentRequest");
                NullSafeCheck(request.Filter, "Filter");
                if (request.Filter.ID <= 0)
                    throw new ArgumentException($"DocumentID is not valid '{request.Filter.ID}'");

                using (var context = new CharityEntities())
                {
                    var document = context.Documents.AsNoTracking().FirstOrDefault(d => d.ID == request.Filter.ID);
                    result = DocumentDTOMapper.Invoke(document);
                }
                return new DocumentResponse { Success = true, Result = result };
            }
            catch (Exception ex)
            {
                var response = ExceptionToBaseResponse(ex);
                return response.Cast<DocumentResponse>();
            }
        }
        public DocumentResponse DocumentGetList(DocumentRequest request)
        {
            List<DocumentDTO> result = null;
            try
            {
                NullSafeCheck(request, "DocumentRequest");

                var filter = request.Filter;

                using (var context = new CharityEntities())
                {
                    var query = context.Documents.AsNoTracking().AsQueryable();
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
                            if (filter.PatronID > 0)
                                query = query.Where(p => p.PatronID == request.Filter.PatronID);
                        }
                    }
                    result = query.Select(DocumentDTOMapper).ToList();
                }

                return new DocumentResponse { Success = true, ResultList = result };
            }
            catch (Exception ex)
            {
                var response = ExceptionToBaseResponse(ex);
                return response.Cast<DocumentResponse>();
            }
        }
    }
}
