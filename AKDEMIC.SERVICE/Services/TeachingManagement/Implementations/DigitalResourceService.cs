using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public sealed class DigitalResourceService : IDigitalResourceService
    {
        private readonly IDigitalResourceRepository _digitalResourceRepository;
        public DigitalResourceService(IDigitalResourceRepository digitalResourceRepository)
        {
            _digitalResourceRepository = digitalResourceRepository;
        }

        public async Task DeleteDigitalResourceCareer(Guid digitalResourceId)
            => await _digitalResourceRepository.DeleteDigitalResourceCareer(digitalResourceId);

        public async Task<List<DigitalResourceCareer>> GetDigitalResourceCareers(Guid digitalResourceId)
            => await _digitalResourceRepository.GetDigitalResourceCareers(digitalResourceId);
        
        public async Task<DataTablesStructs.ReturnedData<DigitalResource>> GetDigitalResourceDatatable(DataTablesStructs.SentParameters parameters, ClaimsPrincipal user, string searchValue)
            => await _digitalResourceRepository.GetDigitalResourceDatatable(parameters, user, searchValue);

        public async Task<DataTablesStructs.ReturnedData<DigitalResource>> GetDigitalResourceToTeacherDatatable(DataTablesStructs.SentParameters parameters, string teacherId, string searchValue)
            => await _digitalResourceRepository.GetDigitalResourceToTeacherDatatable(parameters, teacherId, searchValue);

        Task IDigitalResourceService.DeleteAsync(DigitalResource digitalResource)
            => _digitalResourceRepository.Delete(digitalResource);
        Task<DigitalResource> IDigitalResourceService.GetAsync(Guid id)
            => _digitalResourceRepository.Get(id);

        Task IDigitalResourceService.InsertAsync(DigitalResource digitalResource)
            => _digitalResourceRepository.Insert(digitalResource);

        Task IDigitalResourceService.UpdateAsync(DigitalResource digitalResource)
            => _digitalResourceRepository.Update(digitalResource);
    }
}