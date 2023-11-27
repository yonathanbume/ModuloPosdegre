using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IConceptHistoryRepository: IRepository<ConceptHistory>
    {
        Task<ConceptHistory> GetLastChangeByConceptId(Guid conceptId);
        Task<DataTablesStructs.ReturnedData<object>> GetConceptHistoryDatatable(DataTablesStructs.SentParameters sentParameters, Guid conceptId);
    }
}
