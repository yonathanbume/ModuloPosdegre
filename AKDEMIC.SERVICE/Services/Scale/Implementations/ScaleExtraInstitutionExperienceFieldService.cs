using System;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class ScaleExtraInstitutionExperienceFieldService : IScaleExtraInstitutionExperienceFieldService
    {
        private readonly IScaleExtraInstitutionExperienceFieldRepository _scaleExtraInstitutionExperienceFieldRepository;

        public ScaleExtraInstitutionExperienceFieldService(IScaleExtraInstitutionExperienceFieldRepository scaleExtraInstitutionExperienceFieldRepository)
        {
            _scaleExtraInstitutionExperienceFieldRepository = scaleExtraInstitutionExperienceFieldRepository;
        }

        public async Task<ScaleExtraInstitutionExperienceField> Get(Guid id)
        {
            return await _scaleExtraInstitutionExperienceFieldRepository.Get(id);
        }

        public async Task Insert(ScaleExtraInstitutionExperienceField entity)
        {
            await _scaleExtraInstitutionExperienceFieldRepository.Insert(entity);
        }

        public async Task Update(ScaleExtraInstitutionExperienceField entity)
        {
            await _scaleExtraInstitutionExperienceFieldRepository.Update(entity);
        }

        public async Task Delete(ScaleExtraInstitutionExperienceField entity)
        {
            await _scaleExtraInstitutionExperienceFieldRepository.Delete(entity);
        }

        public async Task<ScaleExtraInstitutionExperienceField> GetScaleExtraInstitutionExperienceFieldByResolutionId(Guid resolutionId)
        {
            return await _scaleExtraInstitutionExperienceFieldRepository.GetScaleExtraInstitutionExperienceFieldByResolutionId(resolutionId);
        }
    }
}
