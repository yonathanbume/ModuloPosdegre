using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces
{
    public interface IRecordSubjectTypeRepository : IRepository<RecordSubjectType>
    {
        Task<IEnumerable<RecordSubjectType>> GetRecordSubjectTypes();
        Task<DataTablesStructs.ReturnedData<RecordSubjectType>> GetRecordSubjectTypesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetRecordSubjectTypesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null);












        Task<bool> HasRelatedUserProcedures(Guid recordSubjectTypeId);
        Task<bool> IsCodeDuplicated(string code);
        Task<bool> IsCodeDuplicated(string code, Guid recordSubjectTypeId);
        Task<Tuple<int, List<RecordSubjectType>>> GetRecordSubjectTypes(DataTablesStructs.SentParameters sentParameters);
    }
}
