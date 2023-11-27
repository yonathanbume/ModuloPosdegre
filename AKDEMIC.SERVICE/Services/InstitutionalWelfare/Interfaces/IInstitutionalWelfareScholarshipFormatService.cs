using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces
{
    public interface IInstitutionalWelfareScholarshipFormatService
    {
        Task Add(InstitutionalWelfareScholarshipFormat entity);
        Task Insert(InstitutionalWelfareScholarshipFormat entity);
        Task Update(InstitutionalWelfareScholarshipFormat entity);
        Task<InstitutionalWelfareScholarshipFormat> Get(Guid id);
        Task Delete(InstitutionalWelfareScholarshipFormat entity);
        Task DeleteRange(IEnumerable<InstitutionalWelfareScholarshipFormat> entity);
        Task<List<InstitutionalWelfareScholarshipFormat>> GetAllByScholarship(Guid scholarshipId);
    }
}
