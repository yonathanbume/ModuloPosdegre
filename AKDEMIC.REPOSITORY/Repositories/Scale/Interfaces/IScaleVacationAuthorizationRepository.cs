using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IScaleVacationAuthorizationRepository : IRepository<ScaleVacationAuthorization>
    {
        Task<int> GetVacationAuthorizationsQuantity(string userId);
        Task<List<ScaleVacationAuthorization>> GetVacationAuthorizationsByPaginationParameters(string userId, PaginationParameter paginationParameter);
        Task<Tuple<int, List<ScaleVacationAuthorization>>> GetVacationAuthorizationsReportByPaginationParameters(PaginationParameter paginationParameter, ClaimsPrincipal user = null);
        Task<List<ScaleVacationAuthorization>> GetVacationAuthorizationsReport(string search, ClaimsPrincipal user = null);
    }
}
