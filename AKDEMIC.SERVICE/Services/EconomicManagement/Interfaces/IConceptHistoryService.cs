using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IConceptHistoryService
    {
        Task Insert(ConceptHistory conceptHistory);
        Task Add(ConceptHistory conceptHistory);
        Task<ConceptHistory> GetLastChangeByConceptId(Guid conceptId);
        Task<DataTablesStructs.ReturnedData<object>> GetConceptHistoryDatatable(DataTablesStructs.SentParameters sentParameters, Guid conceptId);
    }
}
