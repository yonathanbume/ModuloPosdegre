using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface INonTeachingLoadScheduleService
    {
        Task Insert(NonTeachingLoadSchedule entity);
        Task Update(NonTeachingLoadSchedule entity);
        Task<NonTeachingLoadSchedule> Get(Guid id);
        Task Delete(NonTeachingLoadSchedule entity);
        Task<DataTablesStructs.ReturnedData<object>> GetNonTeachingLoadScheduleDatatable(DataTablesStructs.SentParameters sentParameters, Guid nonTeachingLoadId);
        Task<double> GetRemainingMinutesByNonteachingLoadId(Guid nonTeachingLoadId);
        Task<Tuple<string, bool>> HasConflict(string teacherId, int weekDay, TimeSpan startTime, TimeSpan endTime, Guid? ignoredId = null);
        Task<double> GetCurrentMinutesByNonteachingLoadId(Guid nonTeachingLoadId);
        Task<object> GetScheduleObject(string teacherId, DateTime start, DateTime end);
        Task<object> GetScheduleObjectDetail(Guid id);
        Task<List<NonTeachingLoadSchedule>> GetByNonTeachingLoad(Guid id);
    }
}
