using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces
{
    public interface IInstitutionalWelfareAnswerService
    {
        Task DeleteRange(IEnumerable<InstitutionalWelfareAnswer> entites);
        Task Delete(InstitutionalWelfareAnswer entity);
        Task<IEnumerable<InstitutionalWelfareAnswer>> GetAllBySectionId(Guid sectionId);
        Task<IEnumerable<InstitutionalWelfareAnswer>> GetAllByQuestionId(Guid questionId);
        Task<InstitutionalWelfareAnswer>Get(Guid selection);
    }
}
