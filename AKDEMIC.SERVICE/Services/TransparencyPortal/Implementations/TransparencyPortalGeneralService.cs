using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class TransparencyPortalGeneralService : ITransparencyPortalGeneralService
    {
        private readonly ITransparencyPortalGeneralRepository _transparencyPortalGeneralRepository;
        public TransparencyPortalGeneralService(ITransparencyPortalGeneralRepository transparencyPortalGeneralRepository)
        {
            _transparencyPortalGeneralRepository = transparencyPortalGeneralRepository;
        }

        public async Task DeleteById(Guid id)
        {
            await _transparencyPortalGeneralRepository.DeleteById(id);
        }

        public async  Task<TransparencyPortalGeneral> Get(Guid id)
        {
            return await _transparencyPortalGeneralRepository.Get(id);
        }

        public async Task<IEnumerable<TransparencyPortalGeneral>> GetAll()
        {
            return await _transparencyPortalGeneralRepository.GetAll();
        }

        public async Task<List<TransparencyPortalGeneral>> GetByType(int type)
        {
            return await _transparencyPortalGeneralRepository.GetByType(type);
        }

        public async Task<TransparencyPortalGeneral> GetFirstByType(int type)
        {
            return await _transparencyPortalGeneralRepository.GetFirstByType(type);
        }

        public async Task Insert(TransparencyPortalGeneral regulation)
        {
            await _transparencyPortalGeneralRepository.Insert(regulation);
        }

        public async Task Update(TransparencyPortalGeneral regulation)
        {
            await _transparencyPortalGeneralRepository.Update(regulation);
        }
    }
}
