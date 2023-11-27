using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public class NonTeachingLoadScheduleService : INonTeachingLoadScheduleService
    {
        private readonly INonTeachingLoadScheduleRepositoy _nonTeachingLoadScheduleRepositoy;

        public NonTeachingLoadScheduleService(INonTeachingLoadScheduleRepositoy nonTeachingLoadScheduleRepositoy)
        {
            _nonTeachingLoadScheduleRepositoy = nonTeachingLoadScheduleRepositoy;
        }

        public async Task Delete(NonTeachingLoadSchedule entity)
            => await _nonTeachingLoadScheduleRepositoy.Delete(entity);

        public async Task<NonTeachingLoadSchedule> Get(Guid id)
            => await _nonTeachingLoadScheduleRepositoy.Get(id);

        public async Task<List<NonTeachingLoadSchedule>> GetByNonTeachingLoad(Guid id)
            => await _nonTeachingLoadScheduleRepositoy.GetByNonTeachingLoad(id);

        public async Task<double> GetCurrentMinutesByNonteachingLoadId(Guid nonTeachingLoadId)
            => await _nonTeachingLoadScheduleRepositoy.GetCurrentMinutesByNonteachingLoadId(nonTeachingLoadId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetNonTeachingLoadScheduleDatatable(DataTablesStructs.SentParameters sentParameters, Guid nonTeachingLoadId)
            => await _nonTeachingLoadScheduleRepositoy.GetNonTeachingLoadScheduleDatatable(sentParameters, nonTeachingLoadId);

        public async Task<double> GetRemainingMinutesByNonteachingLoadId(Guid nonTeachingLoadId)
            => await _nonTeachingLoadScheduleRepositoy.GetRemainingMinutesByNonteachingLoadId(nonTeachingLoadId);

        public async Task<object> GetScheduleObject(string teacherId, DateTime start, DateTime end)
            => await _nonTeachingLoadScheduleRepositoy.GetScheduleObject(teacherId, start, end);

        public async Task<object> GetScheduleObjectDetail(Guid id)
            => await _nonTeachingLoadScheduleRepositoy.GetScheduleObjectDetail(id);

        public async Task<Tuple<string, bool>> HasConflict(string teacherId, int weekDay, TimeSpan startTime, TimeSpan endTime, Guid? ignoredId = null)
            => await _nonTeachingLoadScheduleRepositoy.HasConflict(teacherId, weekDay, startTime, endTime, ignoredId);

        public async Task Insert(NonTeachingLoadSchedule entity)
            => await _nonTeachingLoadScheduleRepositoy.Insert(entity);

        public async Task Update(NonTeachingLoadSchedule entity)
            => await _nonTeachingLoadScheduleRepositoy.Update(entity);
    }
}
