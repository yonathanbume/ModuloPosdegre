using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces
{
    public interface INonTeachingLoadRepository : IRepository<NonTeachingLoad>
    {
        Task<IEnumerable<NonTeachingLoad>> GetAll(string userId, int? category = null, int? minHours = null, int? maxHours = null, Guid? teachingLoadTypeId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetNonTeachingLoadByTeacher(DataTablesStructs.SentParameters parameters, string userId, Guid? termId);
        Task<bool> AnyByTeachingLoadType(Guid? teachingLoadTypeId, Guid? teachingLoadSubTypeId);
    }
}
