using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces
{
    public interface IInstitutionalRecordCategorizationByStudentService
    {
        Task<InstitutionalRecordCategorizationByStudent> Get(Guid id);
        Task<InstitutionalRecordCategorizationByStudent> GetByStudentAndRecord(Guid recordId, Guid studentId);
        Task Update(InstitutionalRecordCategorizationByStudent institutionalRecordCategorizationByStudent);
        Task Insert(InstitutionalRecordCategorizationByStudent institutionalRecordCategorizationByStudent);
        Task Delete(InstitutionalRecordCategorizationByStudent institutionalRecordCategorizationByStudent);
        Task<object> GetStudentReport(Guid id, Guid termId, byte? sisfohClasification = 0, Guid? categorizationLevelId = null, Guid? careerId = null);
    }
}
