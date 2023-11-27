using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.ScaleLicenseAuthorization;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IScaleLicenseAuthorizationRepository : IRepository<ScaleLicenseAuthorization>
    {
        Task<int> GetLicenseAuthorizationsQuantity(string userId);
        Task<List<ScaleLicenseAuthorization>> GetLicenseAuthorizationsByPaginationParameters(string userId, PaginationParameter paginationParameter);
        Task<Tuple<int, List<ScaleLicenseAuthorization>>> GetLicenseAuthorizationsReportByPaginationParameters(PaginationParameter paginationParameter);
        Task<List<ScaleLicenseAuthorization>> GetLicenseAuthorizationsReport(string search);
        Task<int> GetTotalLicenseTimeByUser(string userId);
        Task<DataTablesStructs.ReturnedData<object>> GetLicensesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, ClaimsPrincipal user = null);
        Task<List<TeacherLicenseTemplate>> GetLicenseRecordReport(Guid facultyId);
        Task<IEnumerable<ScaleLicenseAuthorization>> GetAllByUserIdAndRemunerateState(string userId, bool isRemunerated);
        Task<IEnumerable<ScaleLicenseAuthorization>> GetAllByUserId(string userId);
    }
}
