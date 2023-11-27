using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;
using AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Implementations
{
    public class AeContactMessageService : IAeContactMessageService
    {
        private readonly IAeContactMessageRepository _aeContactMessageRepository;

        public AeContactMessageService(IAeContactMessageRepository aeContactMessageRepository)
        {
            _aeContactMessageRepository = aeContactMessageRepository;
        }

        public async Task Get<AeContactMessage>(Guid id)
            => await _aeContactMessageRepository.Get(id);

        public async Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => await _aeContactMessageRepository.GetDatatable(sentParameters, search);

        public async Task<object> GetDatatableByUser(DataTablesStructs.SentParameters sentParameters, string userId)
            => await _aeContactMessageRepository.GetDatatableByUser(sentParameters, userId);

        public async Task Insert(AeContactMessage model)
            => await _aeContactMessageRepository.Insert(model);

        public async Task Update(AeContactMessage model)
            => await _aeContactMessageRepository.Update(model);
    }
}
