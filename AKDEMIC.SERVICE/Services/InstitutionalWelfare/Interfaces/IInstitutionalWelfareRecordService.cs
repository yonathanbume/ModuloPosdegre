using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Template;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces
{
    public interface IInstitutionalWelfareRecordService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetInstitutionalWelfareRecordDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<IEnumerable<InstitutionalWelfareRecord>> GetAll();
        Task<InstitutionalWelfareRecord> Get(Guid id);
        Task Insert(InstitutionalWelfareRecord model);
        Task Update(InstitutionalWelfareRecord entity);
        Task Delete(InstitutionalWelfareRecord entity);
        Task<InstitutionalWelfareRecord> GetWithIncludes(Guid recordId);
        Task<InstitutionalWelfareRecord> GetActive();
        Task<bool> HaveAnswerStudents(Guid recordId);
        Task<bool> ExistRecordWithTerm(Guid termId);
        Task<List<StudentTextAnswerTemplate>> StudentTextAnswersByRecord(Guid recordId, Guid studentId, Guid termId);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentDetailDatatable(DataTablesStructs.SentParameters sentParameters, Guid recordId, Guid termId, bool ToEvaluate = false, string searchValue = null);
    }
}
