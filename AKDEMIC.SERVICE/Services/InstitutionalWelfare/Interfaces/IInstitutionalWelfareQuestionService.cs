using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Template;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces
{
    public interface IInstitutionalWelfareQuestionService
    {
        Task<InstitutionalWelfareQuestion> Get(Guid id);
        Task<InstitutionalWelfareQuestion> GetWithIncludes(Guid id);
        Task Update(InstitutionalWelfareQuestion entity);
        Task Insert(InstitutionalWelfareQuestion entity);
        Task Delete(InstitutionalWelfareQuestion entity);
        Task<IEnumerable<InstitutionalWelfareQuestion>> GetAllBySectionId(Guid sectionId);
        Task DeleteRange(IEnumerable<InstitutionalWelfareQuestion> entities);
        Task<bool> AnyByDescription(Guid sectionId, string description, Guid? ignoredId = null);
        Task<IEnumerable<InstitutionalWelfareQuestion>> GetAllByRecordId(Guid recordId);
        //Task<InstitutionalWelfareQuestion> GetByDescriptionAndSectionTitle(string description, string sectionTitle, Guid scholarshipId);

        Task<List<RecordQuestionExcelTemplate>> GetQuestionForExcelByRecord(Guid recordId);
    }
}
