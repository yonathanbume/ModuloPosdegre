using System;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;

namespace AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces
{
    public interface IResearchLineHistoricRepository
    {
        Task UpdateLine(Guid lineId);
        Task DeleteHistoric(Guid lineId);
        Task<DataTablesStructs.ReturnedData<object>> GetHistoricDatatable(DataTablesStructs.SentParameters sentParameters, Guid lineId);
        Task DeleteById(Guid id);
        Task Insert(ResearchLineHistoric researchLineHistoric);
        Task Update(ResearchLineHistoric researchLineHistoric);
    }
}
