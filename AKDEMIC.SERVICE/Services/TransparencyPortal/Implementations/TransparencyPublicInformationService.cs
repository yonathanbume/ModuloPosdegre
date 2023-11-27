using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class TransparencyPublicInformationService : ITransparencyPublicInformationService
    {
        private readonly ITransparencyPublicInformationRepository _transparencyPublicInformationRepository;
        public TransparencyPublicInformationService(ITransparencyPublicInformationRepository transparencyPublicInformationRepository)
        {
            _transparencyPublicInformationRepository = transparencyPublicInformationRepository;
        }

        public async Task DeleteById(Guid id)
        {
            await _transparencyPublicInformationRepository.DeleteById(id);
        }

        public async Task<bool> ExistAnyWithName(Guid id, string name)
        {
            return await _transparencyPublicInformationRepository.ExistAnyWithName(id, name);
        }

        public async  Task<TransparencyPublicInformation> Get(Guid id)
        {
            return await _transparencyPublicInformationRepository.Get(id);
        }

        public async Task<IEnumerable<TransparencyPublicInformation>> GetAll()
        {
            return await _transparencyPublicInformationRepository.GetAll();
        }

        public async Task<IEnumerable<TransparencyPublicInformation>> GetBySlug(string slug)
        {
            return await _transparencyPublicInformationRepository.GetBySlug(slug);
        }

        public async Task Insert(TransparencyPublicInformation regulation)
        {
            await _transparencyPublicInformationRepository.Insert(regulation);
        }

        public async Task Update(TransparencyPublicInformation regulation)
        {
            await _transparencyPublicInformationRepository.Update(regulation);
        }
    }
}
