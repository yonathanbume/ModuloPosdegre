using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces
{
    public interface IInstitutionalWelfareScholarshipService
    {
        Task Insert(InstitutionalWelfareScholarship entity);
        Task Update(InstitutionalWelfareScholarship entity);
        Task Delete(InstitutionalWelfareScholarship entity);
        Task<InstitutionalWelfareScholarship> Get(Guid id);
        Task<bool> AnyStudent(Guid scholarshipId);
        Task<List<ScholarshipStudent>> GetScholarshipStudents(Guid scholarshipId, byte status);
        Task<DataTablesStructs.ReturnedData<object>> GetScholarshipDatatable(DataTablesStructs.SentParameters parameters, Guid? scholarshipTypeId, string searchValue, ClaimsPrincipal user= null);
    }
}
