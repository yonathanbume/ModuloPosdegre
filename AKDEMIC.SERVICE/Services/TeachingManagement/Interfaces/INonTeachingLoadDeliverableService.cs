using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface INonTeachingLoadDeliverableService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetNonTeachingLoadDeliverablesDatatable(DataTablesStructs.SentParameters parameters, Guid nonTeachingLoadId);
        Task Insert(NonTeachingLoadDeliverable entity);
        Task Update(NonTeachingLoadDeliverable entity);
        Task Delete(NonTeachingLoadDeliverable entity);
        Task<bool> AnyByNonTeachingLoad(Guid nonTeachingLoadId);
        Task<NonTeachingLoadDeliverable> Get(Guid id);
        Task<List<NonTeachingLoadDeliverable>> GetNonTeachingLoadDeliverables(Guid nonTeachingLoadId);
    }
}
