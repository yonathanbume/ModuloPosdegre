using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.PortalNew;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public class PortalNewService : IPortalNewService
    {
        private readonly IPortalNewRepository _portalNewRepository;
        public PortalNewService(IPortalNewRepository portalNewRepository)
        {
            _portalNewRepository = portalNewRepository;
        }

        public Task Delete(PortalNew portalNew)
            => _portalNewRepository.Delete(portalNew);

        public Task<List<PortalNewTemplate>> GeNextUpcomingNews(int newsCount = 5)
            => _portalNewRepository.GeNextUpcomingNews(newsCount);

        public Task<PortalNew> Get(Guid id)
            => _portalNewRepository.Get(id);

        public Task<IEnumerable<PortalNew>> GetAll()
            => _portalNewRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetPortalNewsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _portalNewRepository.GetPortalNewsDatatable(sentParameters, searchValue);

        public Task Insert(PortalNew portalNew)
            => _portalNewRepository.Insert(portalNew);

        public Task Update(PortalNew portalNew)
            => _portalNewRepository.Update(portalNew);
    }
}
