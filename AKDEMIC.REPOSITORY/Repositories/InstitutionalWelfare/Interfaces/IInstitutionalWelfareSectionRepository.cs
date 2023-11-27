using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare
{
    public interface IInstitutionalWelfareSectionRepository : IRepository<InstitutionalWelfareSection>
    {
        Task<IEnumerable<InstitutionalWelfareSection>> GetDetailsByRecordId(Guid recordId);
        //Task<IEnumerable<QuestionnaireSection>> GetQuestionnaireDetailsByAcademicAgreementId(Guid academicAgreementId);
        Task<bool> AnyByTitle(Guid recordId, string title,Guid? ignoredId = null);
        Task<IEnumerable<InstitutionalWelfareSection>> GetInstitutionalWelfareSectionsByRecordId(Guid institutionalWelfareRecordId, byte? sisfohClasification = 0, Guid? categorizationLevelId = null, Guid? careerId = null);
        Task<InstitutionalWelfareSection> GetWithIncludes(Guid id);
    }
}
