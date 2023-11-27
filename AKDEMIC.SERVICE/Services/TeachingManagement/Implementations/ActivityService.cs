using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.Activity;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public sealed class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepository;
        public ActivityService(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        Task IActivityService.DeleteAsync(Activity activity)
            => _activityRepository.Delete(activity);

        Task<object> IActivityService.GetAllAsModelA()
            => _activityRepository.GetAllAsModelA();

        Task<ActivityTemplateA> IActivityService.GetAsModelB(Guid? id)
            => _activityRepository.GetAsModelB(id);

        Task<Activity> IActivityService.GetAsync(Guid id)
            => _activityRepository.Get(id);

        Task IActivityService.InsertAsync(Activity activity)
            => _activityRepository.Insert(activity);

        Task IActivityService.UpdateAsync(Activity activity)
            => _activityRepository.Update(activity);
    }
}