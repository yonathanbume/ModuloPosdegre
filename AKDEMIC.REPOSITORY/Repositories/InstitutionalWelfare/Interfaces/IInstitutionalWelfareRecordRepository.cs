using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Template;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare
{
    public interface IInstitutionalWelfareRecordRepository : IRepository<InstitutionalWelfareRecord>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetInstitutionalWelfareRecordDatatable(DataTablesStructs.SentParameters sentParameters,string searchValue = null);
        Task<InstitutionalWelfareRecord> GetActive();
        Task<InstitutionalWelfareRecord> GetWithIncludes(Guid recordId);
        Task<bool> HaveAnswerStudents(Guid recordId);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentDetailDatatable(DataTablesStructs.SentParameters sentParameters, Guid recordId, Guid termId, bool toEvaluate = false, string searchValue = null);
        Task<List<StudentTextAnswerTemplate>> StudentTextAnswersByRecord(Guid recordId, Guid studentId, Guid termId);
        Task<bool> ExistRecordWithTerm(Guid termId);
    }
}
