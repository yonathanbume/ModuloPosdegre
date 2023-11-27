using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces
{
    public interface IDigitalResourceRepository : IRepository<DigitalResource>
    {
        Task<DataTablesStructs.ReturnedData<DigitalResource>> GetDigitalResourceDatatable(DataTablesStructs.SentParameters parameters, ClaimsPrincipal user, string searchValue);
        Task<DataTablesStructs.ReturnedData<DigitalResource>> GetDigitalResourceToTeacherDatatable(DataTablesStructs.SentParameters parameters, string teacherId, string searchValue);
        Task<List<DigitalResourceCareer>> GetDigitalResourceCareers(Guid digitalResourceId);
        Task DeleteDigitalResourceCareer(Guid digitalResourceId);
    }
}