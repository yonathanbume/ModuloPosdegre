using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface IRecordHistoryObservationRepository : IRepository<RecordHistoryObservation>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetObservationsDatatableByRecordHistoryId(DataTablesStructs.SentParameters sentParameters, Guid recordHistoryId);
    }
}
