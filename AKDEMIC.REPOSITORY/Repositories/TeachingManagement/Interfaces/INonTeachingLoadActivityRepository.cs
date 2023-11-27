using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces
{
    public interface INonTeachingLoadActivityRepository : IRepository<NonTeachingLoadActivity>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetNonTeachingLoadActivitiesDatatable(DataTablesStructs.SentParameters parameters, Guid nonTeachingLoadId);
        Task<bool> AnyByNonTeachingLoad(Guid nonTeachingLoadId);
    }
}
