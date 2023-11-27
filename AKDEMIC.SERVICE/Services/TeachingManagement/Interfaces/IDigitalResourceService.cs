using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface IDigitalResourceService
    {
        Task InsertAsync(DigitalResource digitalResource);
        Task UpdateAsync(DigitalResource digitalResource);
        Task DeleteAsync(DigitalResource digitalResource);
        Task<DigitalResource> GetAsync(Guid id);
        Task<List<DigitalResourceCareer>> GetDigitalResourceCareers(Guid digitalResourceId);
        Task DeleteDigitalResourceCareer(Guid digitalResourceId);
        Task<DataTablesStructs.ReturnedData<DigitalResource>> GetDigitalResourceDatatable(DataTablesStructs.SentParameters parameters, ClaimsPrincipal user, string searchValue);
        Task<DataTablesStructs.ReturnedData<DigitalResource>> GetDigitalResourceToTeacherDatatable(DataTablesStructs.SentParameters parameters, string teacherId, string searchValue);
    }
}