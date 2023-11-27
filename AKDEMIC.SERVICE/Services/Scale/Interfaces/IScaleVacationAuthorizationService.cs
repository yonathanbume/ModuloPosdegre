using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IScaleVacationAuthorizationService
    {
        Task<ScaleVacationAuthorization> Get(Guid scaleResolutionId);
        Task Insert(ScaleVacationAuthorization scaleResolution);
        Task Update(ScaleVacationAuthorization scaleResolution);
        Task Delete(ScaleVacationAuthorization scaleResolution);
        Task<int> GetVacationAuthorizationsQuantity(string userId);
        Task<List<ScaleVacationAuthorization>> GetVacationAuthorizationsByPaginationParameters(string userId, PaginationParameter paginationParameter);
        Task<Tuple<int, List<ScaleVacationAuthorization>>> GetVacationAuthorizationsReportByPaginationParameters(PaginationParameter paginationParameter, ClaimsPrincipal user = null);
        Task<List<ScaleVacationAuthorization>> GetVacationAuthorizationsReport(string search, ClaimsPrincipal user = null);
    }
}
