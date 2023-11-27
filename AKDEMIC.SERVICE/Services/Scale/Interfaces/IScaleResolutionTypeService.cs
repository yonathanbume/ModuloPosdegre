using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Resolution;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IScaleResolutionTypeService
    {
        Task<bool> Any(Guid id);
        Task<ScaleResolutionType> GetByName(string name);
        Task<ScaleResolutionType> Get(Guid scaleResolutionTypeId);
        Task<List<ScaleResolutionType>> GetScaleResolutionTypesBySectionId(Guid sectionId);
        Task<bool> ExistResolutionType(string name);
        Task<List<ScaleResolutionTypeTemplate>> GetScaleResolutionTypesBySectionIdAndUser(Guid sectionId,string userId);
        Task<List<ScaleResolutionType>> GetScaleResolutionTypesBySectionId(Guid sectionId, string search, PaginationParameter paginationParameter);
        Task<int> GetScaleResolutionTypesQuantityBySectionId(Guid sectionId, string search);
        Task<List<ScaleResolutionType>> GetScaleNotAssignedResolutionTypesBySectionId(Guid sectionId, string search, PaginationParameter paginationParameter);
        Task<int> GetScaleNotAssignedResolutionTypesQuantityBySectionId(Guid sectionId, string search);
        Task<Tuple<int, List<Tuple<string, int>>>> GetResolutionTypeQuantityReportByPaginationParameters(PaginationParameter paginationParameter);
        Task<List<Tuple<string, int>>> GetResolutionTypeQuantityReport(string search);
        Task<DataTablesStructs.ReturnedData<object>> GetScaleresolutionTypeDatatable(DataTablesStructs.SentParameters sentParameters, int? status = null, string search = null);
        Task<IEnumerable<ScaleResolutionType>> GetAll(string searchValue = null, bool? onlyActive = false);
        Task Insert(ScaleResolutionType entity);
        Task Add(ScaleResolutionType scaleResolutionType);
        Task<bool> AnyByName(string name, Guid? ignoredId = null);
        Task Update(ScaleResolutionType entity);
        Task Delete(ScaleResolutionType entity);
    }
}
