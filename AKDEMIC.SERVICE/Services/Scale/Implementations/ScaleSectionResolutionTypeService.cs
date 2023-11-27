using System;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class ScaleSectionResolutionTypeService : IScaleSectionResolutionTypeService
    {
        private readonly IScaleSectionResolutionTypeRepository _scaleSectionResolutionTypeRepository;

        public ScaleSectionResolutionTypeService(IScaleSectionResolutionTypeRepository scaleSectionResolutionTypeRepository)
        {
            _scaleSectionResolutionTypeRepository = scaleSectionResolutionTypeRepository;
        }

        public async Task<ScaleSectionResolutionType> Get(Guid id)
        {
            return await _scaleSectionResolutionTypeRepository.Get(id);
        }

        public async Task<bool> IsActive(Guid sectionId, Guid resolutionTypeId)
        {
            return await _scaleSectionResolutionTypeRepository.IsActive(sectionId, resolutionTypeId);
        }

        public async Task Insert(ScaleSectionResolutionType scaleSectionResolutionType)
        {
            await _scaleSectionResolutionTypeRepository.Insert(scaleSectionResolutionType);
        }

        public async Task Update(ScaleSectionResolutionType scaleSectionResolutionType)
        {
            await _scaleSectionResolutionTypeRepository.Update(scaleSectionResolutionType);
        }

        public async Task<ScaleSectionResolutionType> GetByRelations(Guid sectionId, Guid resolutionTypeId)
        {
            return await _scaleSectionResolutionTypeRepository.GetByRelations(sectionId, resolutionTypeId);
        }

        public async Task DeleteByRelations(Guid sectionId, Guid resolutionTypeId)
        {
            await _scaleSectionResolutionTypeRepository.DeleteByRelations(sectionId, resolutionTypeId);
        }

        public async Task Delete(ScaleSectionResolutionType sectionResolutionType)
        {
            await _scaleSectionResolutionTypeRepository.Delete(sectionResolutionType);
        }

        public async Task<ScaleSectionResolutionType> GetIncludeResolutionType(Guid id)
        {
            return await _scaleSectionResolutionTypeRepository.GetIncludeResolutionType(id);
        }
    }
}
