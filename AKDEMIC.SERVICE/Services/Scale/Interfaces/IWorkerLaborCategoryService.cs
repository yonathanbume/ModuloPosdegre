using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IWorkerLaborCategoryService
    {
        Task<WorkerLaborCategory> GetAsync(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetWorkerLaborCategoryDatatable(DataTablesStructs.SentParameters sentParameters, int? status, string searchvalue = null);
        Task<bool> AnyByName(string name,Guid? laborRegimeId, Guid? ignoredId = null);
        Task Insert(WorkerLaborCategory entity);
        Task Update(WorkerLaborCategory entity);
        Task Delete(WorkerLaborCategory entity);
        Task<WorkerLaborCategory> Get(Guid id);
        Task<IEnumerable<WorkerLaborCategory>> GetAll();
        Task<object> GetWorkerLaborCategorySelect();
        Task<object> GetSelect2ClientSide();
    }
}