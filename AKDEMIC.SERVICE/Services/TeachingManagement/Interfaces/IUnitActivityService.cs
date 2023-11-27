using AKDEMIC.ENTITIES.Models.TeachingManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface IUnitActivityService
    {
        Task<IEnumerable<UnitActivity>> GetAllByCourseUnitAsync(Guid courseUnitId);
        Task<object> GetUnitActivitiesByCourseTermIdAndSectionId(Guid courseTermId, Guid sectionId);
        Task<UnitActivity> GetAsync(Guid id);
        Task InsertAsync(UnitActivity unitActivity);
        Task InsertRangeAsync(IEnumerable<UnitActivity> unitActivities);
        Task UpdateAsync(UnitActivity unitActivity);
        Task UpdateRangeAsync(IEnumerable<UnitActivity> unitActivities);
        Task DeleteAsync(UnitActivity unitActivity);
        Task DeleteRangeAsync(IEnumerable<UnitActivity> unitActivities);
        Task<object> GetAsModelA(Guid id);
    }
}