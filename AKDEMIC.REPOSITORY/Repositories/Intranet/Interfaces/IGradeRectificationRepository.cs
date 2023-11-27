using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IGradeRectificationRepository : IRepository<GradeRectification>
    {
        Task<IEnumerable<GradeRectification>> GetAll(string teacherId = null, Guid? termId = null);
        Task<bool> AnySubstituteexams(Guid studentId, Guid courseId);
        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string teacherId = null, Guid? termId = null, string searchValue = null, int? state = null);
        Task<bool> AnyByEvaluation(Guid evaluationId);
    }
}
