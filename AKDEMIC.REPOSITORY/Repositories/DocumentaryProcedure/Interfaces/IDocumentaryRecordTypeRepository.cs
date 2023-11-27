using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces
{
    public interface IDocumentaryRecordTypeRepository : IRepository<DocumentaryRecordType>
    {
        Task<bool> AnyDocumentaryRecordTypeByCode(string code);
        Task<IEnumerable<DocumentaryRecordType>> GetDocumentaryRecordTypes();
        Task<DataTablesStructs.ReturnedData<DocumentaryRecordType>> GetDocumentaryRecordTypesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetDocumentaryRecordTypesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null);










        Task<bool> HasRelatedUserProcedures(Guid documentaryRecordTypeId);
        Task<bool> IsCodeDuplicated(string code);
        Task<bool> IsCodeDuplicated(string code, Guid documentaryRecordTypeId);
        Task<Tuple<int, List<DocumentaryRecordType>>> GetDocumentaryRecordTypes(DataTablesStructs.SentParameters sentParameters);
    }
}
