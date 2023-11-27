using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public sealed class UnitActivityService : IUnitActivityService
    {
        private readonly IUnitActivityRepository _unitActivityRepository;

        public UnitActivityService(IUnitActivityRepository unitActivityRepository)
        {
            _unitActivityRepository = unitActivityRepository;
        }

        Task IUnitActivityService.DeleteAsync(UnitActivity unitActivity)
            => _unitActivityRepository.Delete(unitActivity);

        Task IUnitActivityService.DeleteRangeAsync(IEnumerable<UnitActivity> unitActivities)
            => _unitActivityRepository.DeleteRange(unitActivities);

        Task<IEnumerable<UnitActivity>> IUnitActivityService.GetAllByCourseUnitAsync(Guid courseUnitId)
            => _unitActivityRepository.GetAllByCourseUnitAsync(courseUnitId);

        Task<object> IUnitActivityService.GetAsModelA(Guid id)
            => _unitActivityRepository.GetAsModelA(id);

        Task<UnitActivity> IUnitActivityService.GetAsync(Guid id)
            => _unitActivityRepository.Get(id);

        Task<object> IUnitActivityService.GetUnitActivitiesByCourseTermIdAndSectionId(Guid courseTermId, Guid sectionId)
            => _unitActivityRepository.GetUnitActivitiesByCourseTermIdAndSectionId(courseTermId, sectionId);

        Task IUnitActivityService.InsertAsync(UnitActivity unitActivity)
            => _unitActivityRepository.Insert(unitActivity);

        Task IUnitActivityService.InsertRangeAsync(IEnumerable<UnitActivity> unitActivities)
            => _unitActivityRepository.InsertRange(unitActivities);

        Task IUnitActivityService.UpdateAsync(UnitActivity unitActivity)
            => _unitActivityRepository.Update(unitActivity);

        Task IUnitActivityService.UpdateRangeAsync(IEnumerable<UnitActivity> unitActivities)
            => _unitActivityRepository.UpdateRange(unitActivities);
    }
}