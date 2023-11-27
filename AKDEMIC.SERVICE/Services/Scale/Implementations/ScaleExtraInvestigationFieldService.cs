using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class ScaleExtraInvestigationFieldService : IScaleExtraInvestigationFieldService
    {
        private readonly IScaleExtraInvestigationFieldRepository _scaleExtraInvestigationFieldRepository;

        public ScaleExtraInvestigationFieldService(IScaleExtraInvestigationFieldRepository scaleExtraInvestigationFieldRepository)
        {
            _scaleExtraInvestigationFieldRepository = scaleExtraInvestigationFieldRepository;
        }

        public async Task<ScaleExtraInvestigationField> Get(Guid id)
        {
            return await _scaleExtraInvestigationFieldRepository.Get(id);
        }

        public async Task Insert(ScaleExtraInvestigationField entity)
        {
            await _scaleExtraInvestigationFieldRepository.Insert(entity);
        }

        public async Task Update(ScaleExtraInvestigationField entity)
        {
            await _scaleExtraInvestigationFieldRepository.Update(entity);
        }

        public async Task Delete(ScaleExtraInvestigationField entity)
        {
            await _scaleExtraInvestigationFieldRepository.Delete(entity);
        }

        public async Task<ScaleExtraInvestigationField> GetScaleExtraInvestigationFieldByResolutionId(Guid resolutionId)
        {
            return await _scaleExtraInvestigationFieldRepository.GetScaleExtraInvestigationFieldByResolutionId(resolutionId);
        }

        public async Task<IEnumerable<ScaleExtraInvestigationField>> GetByScaleResolutionTypeAndUser(string userId, string resolutionTypeName)
        {
            return await _scaleExtraInvestigationFieldRepository.GetByScaleResolutionTypeAndUser(userId, resolutionTypeName);
        }

        public async Task<IEnumerable<ScaleExtraInvestigationField>> GetAllByUserId(string userId)
        {
            return await _scaleExtraInvestigationFieldRepository.GetAllByUserId(userId);
        }
    }
}
