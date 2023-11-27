using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface IRecordHistoryObservationService
    {
        Task Insert(RecordHistoryObservation entity);
        Task<DataTablesStructs.ReturnedData<object>> GetObservationsDatatableByRecordHistoryId(DataTablesStructs.SentParameters sentParameters, Guid recordHistoryId);
    }
}
