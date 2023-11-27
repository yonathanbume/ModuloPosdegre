using AKDEMIC.ENTITIES.Models.TeachingManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface IUnitResourceService
    {
        Task<IEnumerable<UnitResource>> GetAllByCourseUnitId(Guid courseUnitId);
        Task<UnitResource> GetAsync(Guid id);
        Task InsertAsync(UnitResource unitResource);
        Task InsertRangeAsync(IEnumerable<UnitResource> unitResources);
        Task UpdateAsync(UnitResource unitResource);
        Task UpdateRangeAsync(IEnumerable<UnitResource> unitResources);
        Task DeleteAsync(UnitResource unitResource);
        Task DeleteRangeAsync(IEnumerable<UnitResource> unitResources);
        Task<object> GetAsModelA(Guid id);
    }
}