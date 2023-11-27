using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class InstitutionalActivityService : IInstitutionalActivityService
    {
        private readonly IInstitutionalActivityRepository _institutionalActivityRepository;
        public InstitutionalActivityService(IInstitutionalActivityRepository institutionalActivityRepository)
        {
            _institutionalActivityRepository = institutionalActivityRepository;
        }

        public async Task DeleteById(Guid id)
        {
            await _institutionalActivityRepository.DeleteById(id);
        }

        public async Task<bool> ExistAnyWithName(Guid id, string name)
        {
            return await _institutionalActivityRepository.ExistAnyWithName(id, name);
        }

        public async  Task<InstitutionalActivity> Get(Guid id)
        {
            return await _institutionalActivityRepository.Get(id);
        }

        public async Task<IEnumerable<InstitutionalActivity>> GetAll()
        {
            return await _institutionalActivityRepository.GetAll();
        }

        public async Task<IEnumerable<InstitutionalActivity>> GetBySlug(string slug)
        {
            return await _institutionalActivityRepository.GetBySlug(slug);
        }

        public async Task Insert(InstitutionalActivity regulation)
        {
            await _institutionalActivityRepository.Insert(regulation);
        }

        public async Task Update(InstitutionalActivity regulation)
        {
            await _institutionalActivityRepository.Update(regulation);
        }
    }
}
