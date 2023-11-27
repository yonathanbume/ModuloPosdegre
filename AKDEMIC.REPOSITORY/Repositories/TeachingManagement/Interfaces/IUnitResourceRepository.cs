using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces
{
    public interface IUnitResourceRepository : IRepository<UnitResource>
    {
        Task<IEnumerable<UnitResource>> GetAllByCourseUnitId(Guid courseUnitId);
        Task<object> GetAsModelA(Guid id);
    }
}