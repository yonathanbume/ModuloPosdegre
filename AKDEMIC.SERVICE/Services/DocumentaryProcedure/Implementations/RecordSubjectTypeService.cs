using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class RecordSubjectTypeService : IRecordSubjectTypeService
    {
        private readonly IRecordSubjectTypeRepository _recordSubjectTypeRepository;

        public RecordSubjectTypeService(IRecordSubjectTypeRepository recordSubjectTypeRepository)
        {
            _recordSubjectTypeRepository = recordSubjectTypeRepository;
        }

        public async Task<RecordSubjectType> Get(Guid id)
        {
            return await _recordSubjectTypeRepository.Get(id);
        }

        public async Task<IEnumerable<RecordSubjectType>> GetAll()
        {
            return await _recordSubjectTypeRepository.GetAll();
        }

        public async Task<IEnumerable<RecordSubjectType>> GetRecordSubjectTypes()
        {
            return await _recordSubjectTypeRepository.GetRecordSubjectTypes();
        }

        public async Task<DataTablesStructs.ReturnedData<RecordSubjectType>> GetRecordSubjectTypesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _recordSubjectTypeRepository.GetRecordSubjectTypesDatatable(sentParameters, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetRecordSubjectTypesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await _recordSubjectTypeRepository.GetRecordSubjectTypesSelect2(requestParameters, searchValue);
        }

        public async Task Delete(RecordSubjectType recordSubjectType)
        {
            await _recordSubjectTypeRepository.Delete(recordSubjectType);
        }

        public async Task Insert(RecordSubjectType recordSubjectType)
        {
            await _recordSubjectTypeRepository.Insert(recordSubjectType);
        }

        public async Task Update(RecordSubjectType recordSubjectType)
        {
            await _recordSubjectTypeRepository.Update(recordSubjectType);
        }













        public async Task<bool> HasRelatedUserProcedures(Guid recordSubjectTypeId)
        {
            return await _recordSubjectTypeRepository.HasRelatedUserProcedures(recordSubjectTypeId);
        }

        public async Task<bool> IsCodeDuplicated(string code)
        {
            return await _recordSubjectTypeRepository.IsCodeDuplicated(code);
        }

        public async Task<bool> IsCodeDuplicated(string code, Guid recordSubjectTypeId)
        {
            return await _recordSubjectTypeRepository.IsCodeDuplicated(code, recordSubjectTypeId);
        }

        public async Task<Tuple<int, List<RecordSubjectType>>> GetRecordSubjectTypes(DataTablesStructs.SentParameters sentParameters)
        {
            return await _recordSubjectTypeRepository.GetRecordSubjectTypes(sentParameters);
        }
    }
}
