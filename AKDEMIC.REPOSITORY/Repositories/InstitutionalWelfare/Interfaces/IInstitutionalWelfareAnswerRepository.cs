using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare
{
    public interface IInstitutionalWelfareAnswerRepository : IRepository<InstitutionalWelfareAnswer>
    {
        Task<IEnumerable<InstitutionalWelfareAnswer>> GetAllByQuestionId(Guid questionId);
        Task<IEnumerable<InstitutionalWelfareAnswer>> GetAllBySectionId(Guid sectionId);
    }
}
