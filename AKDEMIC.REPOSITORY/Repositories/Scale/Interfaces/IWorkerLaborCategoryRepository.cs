using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IWorkerLaborCategoryRepository : IRepository<WorkerLaborCategory>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetWorkerLaborCategoryDatatable(DataTablesStructs.SentParameters sentParameters, int? status, string searchvalue = null);
        Task<bool> AnyByName(string name, Guid? laborRegimeId, Guid? ignoredId = null);
        Task<object> GetSelect2ClientSide();
        Task<object> GetWorkerLaborCategorySelect();
    }
}