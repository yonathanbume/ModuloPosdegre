using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IGradeRectificationService
    {
        Task<IEnumerable<GradeRectification>> GetAll(string teacherId = null, Guid? termId = null);
        Task<GradeRectification> Get(Guid id);
        Task Insert(GradeRectification gradeCorrection);
        Task InsertRange(List<GradeRectification> gradeCorrections);
        Task Update(GradeRectification gradeCorrection);
        Task DeleteById(Guid id);
        Task<bool> AnySubstituteexams(Guid studentId, Guid courseId);
        Task<bool> AnyByEvaluation(Guid evaluationId);
        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string teacherId = null, Guid? termId = null, string searchValue = null, int? state = null);

    }
}
