using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.Activity;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces
{
    public interface IActivityRepository : IRepository<Activity>
    {
        Task<object> GetAllAsModelA();
        Task<ActivityTemplateA> GetAsModelB(Guid? id = null);
    }
}