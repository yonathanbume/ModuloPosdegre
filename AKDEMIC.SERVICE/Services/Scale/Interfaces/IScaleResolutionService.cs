using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Resolution;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IScaleResolutionService
    {
        Task<ScaleResolution> Get(Guid scaleResolutionId);
        Task Add(ScaleResolution scaleResolution);
        Task Insert(ScaleResolution scaleResolution);
        Task Update(ScaleResolution scaleResolution);
        Task Delete(ScaleResolution scaleResolution);
        Task<bool> DeleteBySection(Guid Id, byte sectionNumber);
        Task<int> GetScaleResolutionsQuantityBySectionResolutionType(Guid sectionResolutionTypeId);
        Task<int> GetScaleResolutionsQuantity(string userId, Guid sectionId, Guid resolutionTypeId, string search);
        Task<List<ScaleResolution>> GetScaleResolutionsByPaginationParameters(string userId, Guid sectionId, Guid resolutionTypeId, string search, PaginationParameter paginationParameter);
        Task<List<ScaleResolution>> GetScaleResolutionsBySectionUserId(string userId, Guid sectionId);
        Task<List<ScaleResolution>> GetQuinquenniumResolutionsBySectionUserId(string userId, Guid sectionId);
        Task<List<ScaleResolution>> GetContractsResolutionsBySectionUserId(string userId, Guid sectionId);

        Task<Tuple<int, List<Tuple<ApplicationUser, int>>>> GetUserResolutionsQuantityReportByPaginationParameters(PaginationParameter paginationParameter, Guid? dedicationId = null , Guid? conditionId = null);
        Task<List<Tuple<ApplicationUser, int>>> GetUserResolutionsQuantityReport(string search);

        Task<List<ScaleResolutionInvestigationTemplate>> GetInvestigationScaleResolutionsByPaginationParameters(string userId,Guid sectionId, Guid resolutionTypeId, string search, PaginationParameter paginationParameter);

        Task<DataTablesStructs.ReturnedData<object>> GetAllTeacherReportDatatable(DataTablesStructs.SentParameters sentParameters, string startDate = null, string endDate = null, Guid? scaleResolutionTypeId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAllUserReportDatatable(DataTablesStructs.SentParameters sentParameters, string userId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetContractsByUserDatatable(DataTablesStructs.SentParameters sentParameters, string userId);
        Task<DataTablesStructs.ReturnedData<object>> GetBenefitsByUserDatatable(DataTablesStructs.SentParameters sentParameters, string userId);
        Task<string> GetWokerStatusDescriptionBySectionNumber(byte contracts);
    }
}
