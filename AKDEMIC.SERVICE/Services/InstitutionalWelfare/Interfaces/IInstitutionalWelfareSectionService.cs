using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces
{
    public interface IInstitutionalWelfareSectionService
    {
        Task Insert(InstitutionalWelfareSection entity);
        Task Delete(InstitutionalWelfareSection entity);
        Task DeleteRange(IEnumerable<InstitutionalWelfareSection> entites);
        Task<InstitutionalWelfareSection> Get(Guid id);
        Task<InstitutionalWelfareSection> GetWithIncludes(Guid id);
        Task Update(InstitutionalWelfareSection entity);
        Task<IEnumerable<InstitutionalWelfareSection>> GetDetailsByRecordId(Guid recordId);
        //Task<IEnumerable<InstitutionalWelfareSection>> GetQuestionnaireDetailsByAcademicAgreementId(Guid academicAgreementId);
        Task<bool> AnyByTitle(Guid recordId, string title,Guid? ignoredId = null);
        Task<IEnumerable<InstitutionalWelfareSection>> GetInstitutionalWelfareSectionsByRecordId(Guid institutionalWelfareRecordId, byte? sisfohClasification = 0, Guid? categorizationLevelId = null, Guid? careerId = null);
    }
}
