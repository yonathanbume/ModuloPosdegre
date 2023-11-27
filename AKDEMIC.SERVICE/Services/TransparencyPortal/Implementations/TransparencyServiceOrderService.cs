using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class TransparencyServiceOrderService: ITransparencyServiceOrderService
    {
        private readonly ITransparencyServiceOrderRepository _transparencyServiceOrderServiceRepository;
        public TransparencyServiceOrderService(ITransparencyServiceOrderRepository transparencyServiceOrderServiceRepository)
        {
            _transparencyServiceOrderServiceRepository = transparencyServiceOrderServiceRepository;
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetConciliationActsDataTable(DataTablesStructs.SentParameters sentParameters)
        {
            return await _transparencyServiceOrderServiceRepository.GetConciliationActsDataTable(sentParameters);
        }

        public async Task InsertRange(List<TransparencyServiceOrder> part)
        {
            await _transparencyServiceOrderServiceRepository.InsertRange(part);
        }
    }
}
