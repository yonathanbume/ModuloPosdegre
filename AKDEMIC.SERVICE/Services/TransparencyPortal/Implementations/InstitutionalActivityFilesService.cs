using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class InstitutionalActivityFilesService : IInstitutionalActivityFilesService
    {
        private readonly IInstitutionalActivityFilesRepository _institutionalActivityFilesRepository;
        public InstitutionalActivityFilesService(IInstitutionalActivityFilesRepository InstitutionalActivityFilesRepository)
        {
            _institutionalActivityFilesRepository = InstitutionalActivityFilesRepository;
        }

        public async Task DeleteById(Guid id)
        {
            await _institutionalActivityFilesRepository.DeleteById(id);
        }

        public async  Task<InstitutionalActivityFile> Get(Guid id)
        {
            return await _institutionalActivityFilesRepository.Get(id);
        }

        public async Task<List<InstitutionalActivityFile>> GetByInstitutionalActivityId(Guid id)
        {
            return await _institutionalActivityFilesRepository.GetByInstitutionalActivityId(id);
        }

        public async Task Insert(InstitutionalActivityFile regulation)
        {
            await _institutionalActivityFilesRepository.Insert(regulation);
        }

        public async Task Update(InstitutionalActivityFile regulation)
        {
            await _institutionalActivityFilesRepository.Update(regulation);
        }
    }
}
