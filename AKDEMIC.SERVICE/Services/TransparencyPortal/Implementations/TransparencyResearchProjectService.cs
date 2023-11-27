using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class TransparencyResearchProjectService : ITransparencyResearchProjectService
    {
        private readonly ITransparencyResearchProjectRepository _transparencyResearchProjectRepository;
        public TransparencyResearchProjectService(ITransparencyResearchProjectRepository iransparencyResearchProjectRepository)
        {
            _transparencyResearchProjectRepository = iransparencyResearchProjectRepository;
        }

        public async Task DeleteById(Guid id)
        {
            await _transparencyResearchProjectRepository.DeleteById(id);
        }

        public async Task<bool> ExistAnyWithName(Guid id, string name)
        {
            return await _transparencyResearchProjectRepository.ExistAnyWithName(id, name);
        }

        public async  Task<TransparencyResearchProject> Get(Guid id)
        {
            return await _transparencyResearchProjectRepository.Get(id);
        }

        public async Task<IEnumerable<TransparencyResearchProject>> GetAll()
        {
            return await _transparencyResearchProjectRepository.GetAll();
        }

        public async Task<IEnumerable<TransparencyResearchProject>> GetBySlug(string slug)
        {
            return await _transparencyResearchProjectRepository.GetBySlug(slug);
        }

        public async Task Insert(TransparencyResearchProject regulation)
        {
            await _transparencyResearchProjectRepository.Insert(regulation);
        }

        public async Task Update(TransparencyResearchProject regulation)
        {
            await _transparencyResearchProjectRepository.Update(regulation);
        }
    }
}
