using CharityManager.DTO;
using CharityManager.Service.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using static CharityManager.Service.Mapper;

namespace CharityManager.Service
{
    public partial class Charity : ICharity
    {
        public LoginResponse GetUserLogins(UserRequest request)
        {
            List<LoginDTO> result = null;
            try
            {
                NullSafeCheck(request);
                NullSafeCheck(request.Filter, "Filter");

                var filter = request.Filter;

                using (var context = new CharityEntities())
                {
                    var query = context.Logins.AsNoTracking().AsQueryable();
                    if (filter.ID > 0)
                        query = query.Where(p => p.CreateUser == filter.ID);
                    else
                        throw new Exception("UserID is not valid");
                    result = query.Select(LoginDTOMapper).ToList();
                }

                return new LoginResponse { Success = true, ResultList = result };
            }
            catch (Exception ex)
            {
                var response = ExceptionToBaseResponse(ex);
                return response.Cast<LoginResponse>();
            }
        }
    }
}
