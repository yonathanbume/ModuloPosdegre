using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.Activity;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface IActivityService
    {
        Task<object> GetAllAsModelA();
        Task<ActivityTemplateA> GetAsModelB(Guid? id = null);
        Task<Activity> GetAsync(Guid id);
        Task InsertAsync(Activity activity);
        Task UpdateAsync(Activity activity);
        Task DeleteAsync(Activity activity);

    }
}