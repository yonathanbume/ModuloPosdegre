using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public class ExternalLinkService : IExternalLinkService
    {
        private readonly IExternalLinkRepository _externalLinkRepository;

        public ExternalLinkService(IExternalLinkRepository externalLinkRepository)
        {
            _externalLinkRepository = externalLinkRepository;
        }

        public Task Delete(ExternalLink externalLink)
            => _externalLinkRepository.Delete(externalLink);

        public Task<ExternalLink> Get(Guid id)
            => _externalLinkRepository.Get(id);

        public Task<IEnumerable<ExternalLink>> GetAll()
            => _externalLinkRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, int? type = null, string searchValue = null)
            => _externalLinkRepository.GetDatatable(sentParameters, type, searchValue);

        public Task Insert(ExternalLink externalLink)
            => _externalLinkRepository.Insert(externalLink);

        public Task Update(ExternalLink externalLink)
            => _externalLinkRepository.Update(externalLink);
    }
}
