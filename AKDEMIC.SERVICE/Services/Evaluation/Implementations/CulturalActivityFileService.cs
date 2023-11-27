using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces;
using AKDEMIC.SERVICE.Services.Evaluation.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Evaluation.Implementations
{
    public class CulturalActivityFileService : ICulturalActivityFileService
    {
        private readonly ICulturalActivityFileRepository _repository;

        public CulturalActivityFileService(ICulturalActivityFileRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<CulturalActivityFile>> GetFilesByActivity(Guid id) =>
            await _repository.GetFilesByActivity(id);
        public async Task DeleteRange(IEnumerable<CulturalActivityFile> entities) =>
            await _repository.DeleteRange(entities);
        public async Task<CulturalActivityFile>Get(Guid id) =>
            await _repository.Get(id);
    }
}
