using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class TransparencyResearchProjectFilesService : ITransparencyResearchProjectFilesService
    {
        private readonly ITransparencyResearchProjectFilesRepository _transparencyResearchProjectFilesRepository;
        public TransparencyResearchProjectFilesService(ITransparencyResearchProjectFilesRepository transparencyResearchProjectFilesRepository)
        {
            _transparencyResearchProjectFilesRepository = transparencyResearchProjectFilesRepository;
        }

        public async Task DeleteById(Guid id)
        {
            await _transparencyResearchProjectFilesRepository.DeleteById(id);
        }

        public async  Task<TransparencyResearchProjectFile> Get(Guid id)
        {
            return await _transparencyResearchProjectFilesRepository.Get(id);
        }

        public async Task<List<TransparencyResearchProjectFile>> GetByTransparencyResearchProjectId(Guid id)
        {
            return await _transparencyResearchProjectFilesRepository.GetByTransparencyResearchProjectId(id);
        }

        public async Task Insert(TransparencyResearchProjectFile regulation)
        {
            await _transparencyResearchProjectFilesRepository.Insert(regulation);
        }

        public async Task Update(TransparencyResearchProjectFile regulation)
        {
            await _transparencyResearchProjectFilesRepository.Update(regulation);
        }
    }
}
