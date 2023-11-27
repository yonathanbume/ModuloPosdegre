using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Resolution;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class ScaleResolutionTypeService : IScaleResolutionTypeService
    {
        private readonly IScaleResolutionTypeRepository _scaleResolutionTypeRepository;

        public ScaleResolutionTypeService(IScaleResolutionTypeRepository scaleResolutionTypeRepository)
        {
            _scaleResolutionTypeRepository = scaleResolutionTypeRepository;
        }

        public async Task<bool> Any(Guid id)
        {
            return await _scaleResolutionTypeRepository.Any(id);
        }

        public async Task<ScaleResolutionType> Get(Guid scaleResolutionTypeId)
        {
            return await _scaleResolutionTypeRepository.Get(scaleResolutionTypeId);
        }

        public async Task<List<ScaleResolutionType>> GetScaleResolutionTypesBySectionId(Guid sectionId)
        {
            return await _scaleResolutionTypeRepository.GetScaleResolutionTypesBySectionId(sectionId);
        }

        public async Task<List<ScaleResolutionType>> GetScaleResolutionTypesBySectionId(Guid sectionId, string search, PaginationParameter paginationParameter)
        {
            return await _scaleResolutionTypeRepository.GetScaleResolutionTypesBySectionId(sectionId, search, paginationParameter);
        }

        public async Task<int> GetScaleResolutionTypesQuantityBySectionId(Guid sectionId, string search)
        {
            return await _scaleResolutionTypeRepository.GetScaleResolutionTypesQuantityBySectionId(sectionId, search);
        }

        public async Task<List<ScaleResolutionType>> GetScaleNotAssignedResolutionTypesBySectionId(Guid sectionId, string search, PaginationParameter paginationParameter)
        {
            return await _scaleResolutionTypeRepository.GetScaleNotAssignedResolutionTypesBySectionId(sectionId, search, paginationParameter);
        }

        public async Task<int> GetScaleNotAssignedResolutionTypesQuantityBySectionId(Guid sectionId, string search)
        {
            return await _scaleResolutionTypeRepository.GetScaleNotAssignedResolutionTypesQuantityBySectionId(sectionId, search);
        }

        public async Task<Tuple<int, List<Tuple<string, int>>>> GetResolutionTypeQuantityReportByPaginationParameters(PaginationParameter paginationParameter)
        {
            return await _scaleResolutionTypeRepository.GetResolutionTypeQuantityReportByPaginationParameters(paginationParameter);
        }

        public async Task<List<Tuple<string, int>>> GetResolutionTypeQuantityReport(string search)
        {
            return await _scaleResolutionTypeRepository.GetResolutionTypeQuantityReport(search);
        }

        public async Task<List<ScaleResolutionTypeTemplate>> GetScaleResolutionTypesBySectionIdAndUser(Guid sectionId, string userId)
            => await _scaleResolutionTypeRepository.GetScaleResolutionTypesBySectionIdAndUser(sectionId,userId);

        public Task<bool> ExistResolutionType(string name)
            => _scaleResolutionTypeRepository.ExistResolutionType(name);

        public async Task<DataTablesStructs.ReturnedData<object>> GetScaleresolutionTypeDatatable(DataTablesStructs.SentParameters sentParameters, int? status = null, string search = null)
            => await _scaleResolutionTypeRepository.GetScaleresolutionTypeDatatable(sentParameters, status, search);

        public async Task<IEnumerable<ScaleResolutionType>> GetAll(string searchValue = null, bool? onlyActive = false)
            => await _scaleResolutionTypeRepository.GetAll(searchValue, onlyActive);

        public async Task Insert(ScaleResolutionType entity)
            => await _scaleResolutionTypeRepository.Insert(entity);

        public async Task<bool> AnyByName(string name, Guid? ignoredId = null)
            => await _scaleResolutionTypeRepository.AnyByName(name, ignoredId);

        public async Task Update(ScaleResolutionType entity)
            => await _scaleResolutionTypeRepository.Update(entity);

        public async Task Delete(ScaleResolutionType entity)
            => await _scaleResolutionTypeRepository.Delete(entity);

        public async Task Add(ScaleResolutionType scaleResolutionType)
            => await _scaleResolutionTypeRepository.Add(scaleResolutionType);

        public Task<ScaleResolutionType> GetByName(string name)
            => _scaleResolutionTypeRepository.GetByName(name);
    }
}
