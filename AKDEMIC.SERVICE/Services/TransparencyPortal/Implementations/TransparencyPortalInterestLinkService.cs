using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class TransparencyPortalInterestLinkService : ITransparencyPortalInterestLinkService
    {
        private readonly ITransparencyPortalInterestLinkRepository _interestLinkRepository;

        public TransparencyPortalInterestLinkService(ITransparencyPortalInterestLinkRepository interestLinkRepository)
        {
            _interestLinkRepository = interestLinkRepository;
        }

        public async Task Delete(TransparencyPortalInterestLink entity)
            => await _interestLinkRepository.Delete(entity);

        public async Task<TransparencyPortalInterestLink> Get(Guid id)
            => await _interestLinkRepository.Get(id);

        public async Task<IEnumerable<TransparencyPortalInterestLink>> GetAll()
            => await _interestLinkRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, string search, byte? type = null)
            => await _interestLinkRepository.GetDatatable(sentParameters, search, type);

        public async Task Insert(TransparencyPortalInterestLink entity)
            => await _interestLinkRepository.Insert(entity);

        public async Task Update(TransparencyPortalInterestLink entity)
            => await _interestLinkRepository.Update(entity);
    }
}
