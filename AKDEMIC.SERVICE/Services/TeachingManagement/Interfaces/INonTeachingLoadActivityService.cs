using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface INonTeachingLoadActivityService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetNonTeachingLoadActivitiesDatatable(DataTablesStructs.SentParameters parameters, Guid nonTeachingLoadId);
        Task Insert(NonTeachingLoadActivity entity);
        Task Delete(NonTeachingLoadActivity entity);
        Task Update(NonTeachingLoadActivity entity);
        Task<NonTeachingLoadActivity> Get(Guid id);
        Task<bool> AnyByNonTeachingLoad(Guid nonTeachingLoadId);
    }
}
