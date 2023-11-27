using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces
{
    public interface IInstitutionalWelfareScholarshipFormatRepository : IRepository<InstitutionalWelfareScholarshipFormat>
    {
        Task<List<InstitutionalWelfareScholarshipFormat>> GetAllByScholarship(Guid scholarshipId);
    }
}
