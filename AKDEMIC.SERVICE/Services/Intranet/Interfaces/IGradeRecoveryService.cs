using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IGradeRecoveryService
    {
        Task Insert(GradeRecovery entity);
        Task Update(GradeRecovery entity);
        Task Updaterange(IEnumerable<GradeRecovery> entities);

        Task InsertRange(IEnumerable<GradeRecovery> entity);
        Task<object> GetAssignedStudentsWithData(Guid gradeRecoveryExamId);
        Task<IEnumerable<GradeRecovery>> GetByGradeRecoveryExamId(Guid gradeRecoveryExamId);
        Task Delete(GradeRecovery entity);
        Task DeleteRange(IEnumerable<GradeRecovery> entities);
        Task<DataTablesStructs.ReturnedData<object>> GetGradeRecoveryDatatable(DataTablesStructs.SentParameters parameters, Guid gradeRecoveryExamId, string searchValue);
        Task<object> GetAssignedStudentsExecuted(Guid gradeRecoveryExamId);
    }
}
