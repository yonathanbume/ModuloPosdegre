using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IRecordSubjectTypeService
    {
        Task<RecordSubjectType> Get(Guid id);
        Task<IEnumerable<RecordSubjectType>> GetAll();
        Task<IEnumerable<RecordSubjectType>> GetRecordSubjectTypes();
        Task<DataTablesStructs.ReturnedData<RecordSubjectType>> GetRecordSubjectTypesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetRecordSubjectTypesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null);
        Task Delete(RecordSubjectType recordSubjectType);
        Task Insert(RecordSubjectType recordSubjectType);
        Task Update(RecordSubjectType recordSubjectType);







        Task<bool> HasRelatedUserProcedures(Guid recordSubjectTypeId);
        Task<bool> IsCodeDuplicated(string code);
        Task<bool> IsCodeDuplicated(string code, Guid recordSubjectTypeId);
        Task<Tuple<int, List<RecordSubjectType>>> GetRecordSubjectTypes(DataTablesStructs.SentParameters sentParameters);
    }
}
