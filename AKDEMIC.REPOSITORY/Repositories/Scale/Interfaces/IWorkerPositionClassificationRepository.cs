using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IWorkerPositionClassificationRepository: IRepository<WorkerPositionClassification>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetWorkerPositionClassificationDatatable(DataTablesStructs.SentParameters sentParameters, int? status, string searchvalue = null);
        Task<IEnumerable<WorkerPositionClassification>> GetAll(byte? status, string search = null);
        Task<bool> AnyByName(string name, Guid? ignoredId = null);
    }
}
