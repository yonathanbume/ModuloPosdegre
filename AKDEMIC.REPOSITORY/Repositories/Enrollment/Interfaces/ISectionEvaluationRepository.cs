using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface ISectionEvaluationRepository : IRepository<SectionEvaluation>
    {
        Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters, Guid courseId, Guid termId, Guid? sectionId);
    }
}
