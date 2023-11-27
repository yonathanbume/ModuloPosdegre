using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IDocumentTemplateService
    {
        Task Insert(DocumentTemplate document);
        Task DeleteById(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters sentParameters, string search = null, byte? system = null);
        Task<DocumentTemplate> Get(Guid id);
        Task<IEnumerable<DocumentTemplate>> GetAll();
        Task Update(DocumentTemplate document);
    }
}
