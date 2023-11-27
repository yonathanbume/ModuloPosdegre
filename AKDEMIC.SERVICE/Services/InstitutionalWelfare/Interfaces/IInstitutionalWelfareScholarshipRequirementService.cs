using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces
{
    public interface IInstitutionalWelfareScholarshipRequirementService
    {
        Task Insert(InstitutionalWelfareScholarshipRequirement entity);
        Task DeleteRange(IEnumerable<InstitutionalWelfareScholarshipRequirement> entities);
        Task Update(InstitutionalWelfareScholarshipRequirement entity);
        Task<InstitutionalWelfareScholarshipRequirement> Get(Guid id);
        Task Delete(InstitutionalWelfareScholarshipRequirement entity);
        Task<List<InstitutionalWelfareScholarshipRequirement>> GetAllByScholarship(Guid scholarshipId);
    }
}
