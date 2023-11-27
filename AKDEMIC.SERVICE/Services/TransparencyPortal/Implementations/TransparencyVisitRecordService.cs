using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Implementations
{
    public class TransparencyVisitRecordService: ITransparencyVisitRecordService
    {
        private readonly ITransparencyVisitRecordRepository _transparencyVisitRecordServiceRepository;
        public TransparencyVisitRecordService(ITransparencyVisitRecordRepository transparencyVisitRecordServiceRepository)
        {
            _transparencyVisitRecordServiceRepository = transparencyVisitRecordServiceRepository;
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetConciliationActsDataTable(DataTablesStructs.SentParameters sentParameters)
        {
            return await _transparencyVisitRecordServiceRepository.GetConciliationActsDataTable(sentParameters);
        }

        public async Task InsertRange(List<TransparencyVisitsRecord> part)
        {
            await _transparencyVisitRecordServiceRepository.InsertRange(part);
        }
    }
}
