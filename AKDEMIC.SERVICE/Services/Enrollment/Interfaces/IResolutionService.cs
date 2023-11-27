using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IResolutionService
    {
        Task<Resolution> GetAsync(Guid id);
        Task Insert(Resolution entity);
        Task Update(Resolution entity);
        Task AddAsync(Resolution entity);
    }
}