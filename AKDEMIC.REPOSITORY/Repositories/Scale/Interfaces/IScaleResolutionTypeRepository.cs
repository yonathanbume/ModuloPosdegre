using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Resolution;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IScaleResolutionTypeRepository : IRepository<ScaleResolutionType>
    {
        Task<List<ScaleResolutionType>> GetScaleResolutionTypesBySectionId(Guid sectionId);
        Task<ScaleResolutionType> GetByName(string name);
        Task<List<ScaleResolutionTypeTemplate>> GetScaleResolutionTypesBySectionIdAndUser(Guid sectionId, string userId);
        Task<bool> ExistResolutionType(string name);
        Task<List<ScaleResolutionType>> GetScaleResolutionTypesBySectionId(Guid sectionId, string search, PaginationParameter paginationParameter);
        Task<int> GetScaleResolutionTypesQuantityBySectionId(Guid sectionId, string search);
        Task<List<ScaleResolutionType>> GetScaleNotAssignedResolutionTypesBySectionId(Guid sectionId, string search, PaginationParameter paginationParameter);
        Task<int> GetScaleNotAssignedResolutionTypesQuantityBySectionId(Guid sectionId, string search);
        Task<Tuple<int, List<Tuple<string, int>>>> GetResolutionTypeQuantityReportByPaginationParameters(PaginationParameter paginationParameter);
        Task<List<Tuple<string, int>>> GetResolutionTypeQuantityReport(string search);
        Task<DataTablesStructs.ReturnedData<object>> GetScaleresolutionTypeDatatable(DataTablesStructs.SentParameters sentParameters, int? status = null, string search = null);
        Task<IEnumerable<ScaleResolutionType>> GetAll(string searchValue = null, bool? onlyActive = false);
        Task<bool> AnyByName(string name, Guid? ignoredId = null);
    }
}
