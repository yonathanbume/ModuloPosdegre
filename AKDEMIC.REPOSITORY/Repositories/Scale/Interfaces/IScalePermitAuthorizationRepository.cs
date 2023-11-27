using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IScalePermitAuthorizationRepository : IRepository<ScalePermitAuthorization>
    {
        Task<int> GetPermitAuthorizationsQuantity(string userId);
        Task<List<ScalePermitAuthorization>> GetPermitAuthorizationsByPaginationParameters(string userId, PaginationParameter paginationParameter);
        Task<Tuple<int, List<ScalePermitAuthorization>>> GetPermitAuthorizationsReportByPaginationParameters(PaginationParameter paginationParameter, ClaimsPrincipal user = null);
        Task<List<ScalePermitAuthorization>> GetPermitAuthorizationsReport(string search, ClaimsPrincipal user = null);
    }
}
