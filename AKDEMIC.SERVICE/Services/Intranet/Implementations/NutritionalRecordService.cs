using AKDEMIC.CORE.Structs;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class NutritionalRecordService : INutritionalRecordService
    {
        private readonly INutritionalRecordRepository _nutritionalRecordRepository;

        public NutritionalRecordService(INutritionalRecordRepository nutritionalRecordRepository)
        {
            _nutritionalRecordRepository = nutritionalRecordRepository;
        }

        public async Task<object> GetDatatableArmsDetail(DataTablesStructs.SentParameters sentParameters, decimal n_minimo, decimal n_maximo)
            => await _nutritionalRecordRepository.GetDatatableArmsDetail(sentParameters, n_minimo, n_maximo);

        public async Task<object> GetReportWaist(decimal n_minimo, decimal n_maximo)
            => await _nutritionalRecordRepository.GetReportWaist(n_minimo, n_maximo);
    }
}
