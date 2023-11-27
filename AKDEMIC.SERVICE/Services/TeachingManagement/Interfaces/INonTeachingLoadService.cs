using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface INonTeachingLoadService
    {
        Task<IEnumerable<NonTeachingLoad>> GetAll(string userId, int? category = null, int? minHours = null, int? maxHours = null, Guid? teachingLoadTypeId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetNonTeachingLoadByTeacher(DataTablesStructs.SentParameters parameters, string userId, Guid? termId);
        Task<NonTeachingLoad> Get(Guid id);
        Task Insert(NonTeachingLoad nonTeachingLoad);
        Task Update(NonTeachingLoad nonTeachingLoad);
        Task Delete(NonTeachingLoad nonTeachingLoad);
        Task DeleteById(Guid id);
        Task<bool> AnyByTeachingLoadType(Guid? teachingLoadTypeId, Guid? teachingLoadSubTypeId);
    }
}
