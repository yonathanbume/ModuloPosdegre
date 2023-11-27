using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IGradeRecoveryRepository : IRepository<GradeRecovery>
    {
        Task<IEnumerable<GradeRecovery>> GetByGradeRecoveryExamId(Guid gradeRecoveryExamId);
        Task<object> GetAssignedStudentsWithData(Guid gradeRecoveryExamId);
        Task<DataTablesStructs.ReturnedData<object>> GetGradeRecoveryDatatable(DataTablesStructs.SentParameters parameters, Guid gradeRecoveryExamId, string searchValue);
        Task<object> GetAssignedStudentsExecuted(Guid gradeRecoveryExamId);
    }
}
