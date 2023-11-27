using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class RecordService: IRecordService
    {
        private readonly IRecordRepository _recordRepository;
        public RecordService(IRecordRepository recordRepository)
        {

        }

        public async Task<bool> AnyByName(string value)
        {
            return await _recordRepository.AnyByName(value);
        }

        public async Task DeleteByName(string name)
        {
            await _recordRepository.DeleteByName(name);
        }

        public async Task Insert(Record record)
        {
            await _recordRepository.Insert(record);
        }
    }
}
