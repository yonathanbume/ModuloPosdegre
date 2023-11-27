﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.ScaleLicenseAuthorization;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IScaleLicenseAuthorizationService
    {
        Task<ScaleLicenseAuthorization> Get(Guid scaleResolutionId);
        Task<int> GetTotalLicenseTimeByUser(string userId);
        Task Insert(ScaleLicenseAuthorization scaleResolution);
        Task Update(ScaleLicenseAuthorization scaleResolution);
        Task Delete(ScaleLicenseAuthorization scaleResolution);
        Task<int> GetLicenseAuthorizationsQuantity(string userId);
        Task<List<ScaleLicenseAuthorization>> GetLicenseAuthorizationsByPaginationParameters(string userId, PaginationParameter paginationParameter);
        Task<Tuple<int, List<ScaleLicenseAuthorization>>> GetLicenseAuthorizationsReportByPaginationParameters(PaginationParameter paginationParameter);
        Task<List<ScaleLicenseAuthorization>> GetLicenseAuthorizationsReport(string search);

        Task<DataTablesStructs.ReturnedData<object>> GetLicensesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, ClaimsPrincipal user = null);
        Task<List<TeacherLicenseTemplate>> GetLicenseRecordReport(Guid facultyId);
        Task<IEnumerable<ScaleLicenseAuthorization>> GetAllByUserIdAndRemunerateState(string userId, bool isRemunerated);
        Task<IEnumerable<ScaleLicenseAuthorization>> GetAllByUserId(string userId);
    }
}
