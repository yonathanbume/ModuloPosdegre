using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces
{
    public interface IDocumentTypeRepository : IRepository<DocumentType>
    {
        Task<bool> AnyDocumentTypeByCode(string code);
        Task<IEnumerable<DocumentType>> GetDocumentTypes();
        Task<DataTablesStructs.ReturnedData<DocumentType>> GetDocumentTypesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetDocumentTypesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null);
        Task<DocumentType> GetByCode(string code);


        Task<DocumentType> GetByName(string name);
        Task<Tuple<int, List<DocumentType>>> GetDatatableDocumentTypes(DataTablesStructs.SentParameters sentParameters);
        Task<bool> HasCode(string code);
        Task<bool> HasCode(string code, Guid id);
        Task<bool> HasRelated(Guid id);
    }
}
