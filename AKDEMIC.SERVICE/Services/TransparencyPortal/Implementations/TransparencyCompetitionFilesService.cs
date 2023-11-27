using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class TransparencyCompetitionFilesService : ITransparencyCompetitionFilesService
    {
        private readonly ITransparencyCompetitionFilesRepository _transparencyCompetitionFilesRepository;
        public TransparencyCompetitionFilesService(ITransparencyCompetitionFilesRepository transparencyCompetitionFilesRepository)
        {
            _transparencyCompetitionFilesRepository = transparencyCompetitionFilesRepository;
        }

        public async Task DeleteById(Guid id)
        {
            await _transparencyCompetitionFilesRepository.DeleteById(id);
        }

        public async  Task<TransparencyCompetitionFile> Get(Guid id)
        {
            return await _transparencyCompetitionFilesRepository.Get(id);
        }

        public async Task<List<TransparencyCompetitionFile>> GetByTransparencyCompetitionId(Guid id)
        {
            return await _transparencyCompetitionFilesRepository.GetByTransparencyCompetitionId(id);
        }

        public async Task Insert(TransparencyCompetitionFile regulation)
        {
            await _transparencyCompetitionFilesRepository.Insert(regulation);
        }

        public async Task Update(TransparencyCompetitionFile regulation)
        {
            await _transparencyCompetitionFilesRepository.Update(regulation);
        }
    }
}
