using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Template;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare
{
    public interface IInstitutionalWelfareAnswerByStudentRepository : IRepository<InstitutionalWelfareAnswerByStudent>
    {
        Task<bool> ExistAnswerByStudent(Guid recordId, Guid studentId, Guid termId);
        Task<bool> ExistAnswerByUserName(Guid recordId, string userName, Guid termId);
        Task<List<RecordUserReportTemplate>> GetUserAnswersByRecord(Guid recordId, Guid termId);
    }
}
