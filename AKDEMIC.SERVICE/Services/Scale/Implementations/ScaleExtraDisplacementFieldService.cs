using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class ScaleExtraDisplacementFieldService : IScaleExtraDisplacementFieldService
    {
        private readonly IScaleExtraDisplacementFieldRepository _scaleExtraDisplacementFieldRepository;

        public ScaleExtraDisplacementFieldService(IScaleExtraDisplacementFieldRepository scaleExtraDisplacementFieldRepository)
        {
            _scaleExtraDisplacementFieldRepository = scaleExtraDisplacementFieldRepository;
        }

        public async Task<ScaleExtraDisplacementField> Get(Guid id)
        {
            return await _scaleExtraDisplacementFieldRepository.Get(id);
        }

        public async Task Insert(ScaleExtraDisplacementField entity)
        {
            await _scaleExtraDisplacementFieldRepository.Insert(entity);
        }

        public async Task Update(ScaleExtraDisplacementField entity)
        {
            await _scaleExtraDisplacementFieldRepository.Update(entity);
        }

        public async Task Delete(ScaleExtraDisplacementField entity)
        {
            await _scaleExtraDisplacementFieldRepository.Delete(entity);
        }

        public async Task<ScaleExtraDisplacementField> GetScaleExtraDisplacementFieldByResolutionId(Guid resolutionId)
        {
            return await _scaleExtraDisplacementFieldRepository.GetScaleExtraDisplacementFieldByResolutionId(resolutionId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDisplacementRecordByUser(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null)
        {
            return await _scaleExtraDisplacementFieldRepository.GetDisplacementRecordByUser(sentParameters,userId,searchValue);
        }

        public async Task<string> GetScalefieldByUserId(string id)
        {
            return await _scaleExtraDisplacementFieldRepository.GetScalefieldByUserId(id);
        }

        public async Task<IEnumerable<ScaleExtraDisplacementField>> GetByScaleResolutionTypeAndUser(string userId, string resolutionTypeName)
        {
            return await _scaleExtraDisplacementFieldRepository.GetByScaleResolutionTypeAndUser(userId,resolutionTypeName);
        }
    }
}
