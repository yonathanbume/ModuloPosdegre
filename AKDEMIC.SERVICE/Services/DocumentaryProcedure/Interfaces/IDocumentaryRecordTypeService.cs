using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IDocumentaryRecordTypeService
    {
        Task<DocumentaryRecordType> Get(Guid id);
        Task<IEnumerable<DocumentaryRecordType>> GetAll();
        Task<IEnumerable<DocumentaryRecordType>> GetDocumentaryRecordTypes();
        Task<DataTablesStructs.ReturnedData<DocumentaryRecordType>> GetDocumentaryRecordTypesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetDocumentaryRecordTypesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null);
        Task Delete(DocumentaryRecordType documentaryRecordType);
        Task Insert(DocumentaryRecordType documentaryRecordType);
        Task Update(DocumentaryRecordType documentaryRecordType);

        Task Add(DocumentaryRecordType entity);





        Task<bool> HasRelatedUserProcedures(Guid documentaryRecordTypeId);
        Task<bool> IsCodeDuplicated(string code);
        Task<bool> IsCodeDuplicated(string code, Guid documentaryRecordTypeId);
        Task<Tuple<int, List<DocumentaryRecordType>>> GetDocumentaryRecordTypes(DataTablesStructs.SentParameters sentParameters);
    }
}
