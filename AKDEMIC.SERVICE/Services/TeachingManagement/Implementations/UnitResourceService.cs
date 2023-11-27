using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public sealed class UnitResourceService : IUnitResourceService
    {
        private readonly IUnitResourceRepository _unitResourceRepository;
        public UnitResourceService(IUnitResourceRepository unitResourceRepository)
        {
            _unitResourceRepository = unitResourceRepository;
        }

        Task IUnitResourceService.DeleteAsync(UnitResource unitResource)
            => _unitResourceRepository.Delete(unitResource);

        Task IUnitResourceService.DeleteRangeAsync(IEnumerable<UnitResource> unitResources)
            => _unitResourceRepository.DeleteRange(unitResources);

        Task<IEnumerable<UnitResource>> IUnitResourceService.GetAllByCourseUnitId(Guid courseUnitId)
            => _unitResourceRepository.GetAllByCourseUnitId(courseUnitId);

        Task<object> IUnitResourceService.GetAsModelA(Guid id)
            => _unitResourceRepository.GetAsModelA(id);

        Task<UnitResource> IUnitResourceService.GetAsync(Guid id)
            => _unitResourceRepository.Get(id);

        Task IUnitResourceService.InsertAsync(UnitResource unitResource)
            => _unitResourceRepository.Insert(unitResource);

        Task IUnitResourceService.InsertRangeAsync(IEnumerable<UnitResource> unitResources)
            => _unitResourceRepository.InsertRange(unitResources);

        Task IUnitResourceService.UpdateAsync(UnitResource unitResource)
            => _unitResourceRepository.Update(unitResource);

        Task IUnitResourceService.UpdateRangeAsync(IEnumerable<UnitResource> unitResources)
            => _unitResourceRepository.UpdateRange(unitResources);
    }
}