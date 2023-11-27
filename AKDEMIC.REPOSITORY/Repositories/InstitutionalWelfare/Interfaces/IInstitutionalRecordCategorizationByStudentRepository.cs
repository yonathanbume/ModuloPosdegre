using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces
{
    public interface IInstitutionalRecordCategorizationByStudentRepository : IRepository<InstitutionalRecordCategorizationByStudent>
    {
        Task<InstitutionalRecordCategorizationByStudent> GetByStudentAndRecord(Guid recordId, Guid studentId);
        Task<object> GetStudentReport(Guid id, Guid termId, byte? sisfohClasification = 0, Guid? categorizationLevelId = null, Guid? careerId = null);
    }
}
