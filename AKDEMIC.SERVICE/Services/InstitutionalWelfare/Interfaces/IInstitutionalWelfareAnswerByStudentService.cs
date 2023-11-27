using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Template;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces
{
    public interface IInstitutionalWelfareAnswerByStudentService
    {
        Task<InstitutionalWelfareAnswerByStudent> Get(Guid id);
        Task Update(InstitutionalWelfareAnswerByStudent institutionalWelfareAnswerByStudent);
        Task Insert(InstitutionalWelfareAnswerByStudent institutionalWelfareAnswerByStudent);
        Task Delete(InstitutionalWelfareAnswerByStudent institutionalWelfareAnswerByStudent);
        Task InsertRange(List<InstitutionalWelfareAnswerByStudent> answers);
        Task<bool> ExistAnswerByStudent(Guid recordId, Guid studentId, Guid termId);
        Task<bool> ExistAnswerByUserName(Guid recordId, string userName, Guid termId);
        Task<List<RecordUserReportTemplate>> GetUserAnswersByRecord(Guid recordId, Guid termId);
    }
}
