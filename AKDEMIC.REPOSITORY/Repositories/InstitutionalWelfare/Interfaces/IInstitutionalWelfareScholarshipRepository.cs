using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces
{
    public interface IInstitutionalWelfareScholarshipRepository : IRepository<InstitutionalWelfareScholarship>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetScholarshipDatatable(DataTablesStructs.SentParameters parameters, Guid? scholarshipTypeId, string searchValue, ClaimsPrincipal user = null);
        Task<List<ScholarshipStudent>> GetScholarshipStudents(Guid scholarshipId, byte status);
        Task<bool> AnyStudent(Guid scholarshipId);
    }
}
