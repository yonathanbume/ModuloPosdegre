using AKDEMIC.CORE.Structs;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class PsychologicalRecordService : IPsychologicalRecordService
    {
        private readonly IPsychologicalRecordRepository _psychologicalRecordRepository;

        public PsychologicalRecordService(IPsychologicalRecordRepository psychologicalRecordRepository)
        {
            _psychologicalRecordRepository = psychologicalRecordRepository;
        }

        public async Task<object> GetChartReport()
            => await _psychologicalRecordRepository.GetChartReport();

        public async Task<DataTablesStructs.ReturnedData<object>> GetDetailsDatatable(DataTablesStructs.SentParameters sentParameters)
            => await _psychologicalRecordRepository.GetDetailsDatatable(sentParameters);
    }
}
