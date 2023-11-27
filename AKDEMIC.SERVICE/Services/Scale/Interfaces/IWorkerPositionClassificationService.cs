using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IWorkerPositionClassificationService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetWorkerPositionClassificationDatatable(DataTablesStructs.SentParameters sentParameters, int? status, string searchvalue = null);
        Task<IEnumerable<WorkerPositionClassification>> GetAll(byte? status, string search = null);
        Task<bool> AnyByName(string name, Guid? ignoredId = null);
        Task Insert(WorkerPositionClassification entity);
        Task Update(WorkerPositionClassification entity);
        Task Delete(WorkerPositionClassification entity);
        Task<WorkerPositionClassification> Get(Guid id);
    }
}
