using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class TransparencyPortalRegulationService : ITransparencyPortalRegulationService
    {
        private readonly ITransparencyPortalRegulationRepository _transparencyPortalRegulationRepository;
        public TransparencyPortalRegulationService(ITransparencyPortalRegulationRepository transparencyPortalRegulationRepository)
        {
            _transparencyPortalRegulationRepository = transparencyPortalRegulationRepository;
        }

        public async Task DeleteById(Guid id)
        {
            await _transparencyPortalRegulationRepository.DeleteById(id);
        }

        public async  Task<TransparencyPortalRegulation> Get(Guid id)
        {
            return await _transparencyPortalRegulationRepository.Get(id);
        }

        public async Task<IEnumerable<TransparencyPortalRegulation>> GetAll()
        {
            return await _transparencyPortalRegulationRepository.GetAll();
        }

        public async Task<object> GetDataTable(PaginationParameter paginationParameter)
        {
            return await _transparencyPortalRegulationRepository.GetDataTable(paginationParameter);
        }

        public IQueryable<TransparencyPortalRegulation> GetIQueryable()
            => _transparencyPortalRegulationRepository.GetIQueryable();

        public async Task Insert(TransparencyPortalRegulation regulation)
        {
            await _transparencyPortalRegulationRepository.Insert(regulation);
        }

        public async Task Update(TransparencyPortalRegulation regulation)
        {
            await _transparencyPortalRegulationRepository.Update(regulation);
        }
    }
}
