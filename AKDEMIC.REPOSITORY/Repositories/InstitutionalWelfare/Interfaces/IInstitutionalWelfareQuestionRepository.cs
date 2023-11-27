using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Template;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare
{
    public interface IInstitutionalWelfareQuestionRepository : IRepository<InstitutionalWelfareQuestion>
    {
        Task<IEnumerable<InstitutionalWelfareQuestion>> GetAllBySectionId(Guid sectionId);
        Task<bool> AnyByDescription(Guid sectionId, string description, Guid? ignoredId);
        Task<IEnumerable<InstitutionalWelfareQuestion>> GetAllByRecordId(Guid recordId);
        Task<InstitutionalWelfareQuestion> GetWithIncludes(Guid id);
        Task<List<RecordQuestionExcelTemplate>> GetQuestionForExcelByRecord(Guid recordId);
    }
}
