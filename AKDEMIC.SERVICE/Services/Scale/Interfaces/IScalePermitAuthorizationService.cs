using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IScalePermitAuthorizationService
    {
        Task<ScalePermitAuthorization> Get(Guid scaleResolutionId);
        Task Insert(ScalePermitAuthorization scaleResolution);
        Task Update(ScalePermitAuthorization scaleResolution);
        Task Delete(ScalePermitAuthorization scaleResolution);
        Task<int> GetPermitAuthorizationsQuantity(string userId);
        Task<List<ScalePermitAuthorization>> GetPermitAuthorizationsByPaginationParameters(string userId, PaginationParameter paginationParameter);
        Task<Tuple<int, List<ScalePermitAuthorization>>> GetPermitAuthorizationsReportByPaginationParameters(PaginationParameter paginationParameter, ClaimsPrincipal user = null);
        Task<List<ScalePermitAuthorization>> GetPermitAuthorizationsReport(string search, ClaimsPrincipal user = null);
    }
}
