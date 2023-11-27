using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces
{
    public interface IUnitActivityRepository : IRepository<UnitActivity>
    {
        Task<IEnumerable<UnitActivity>> GetAllByCourseUnitAsync(Guid courseUnitId);
        Task<object> GetUnitActivitiesByCourseTermIdAndSectionId(Guid courseTermId, Guid sectionId);
        Task<object> GetAsModelA(Guid id);
    }
}