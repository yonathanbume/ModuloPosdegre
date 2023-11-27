using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface ITemporalGradeService
    {
        Task InsertRange(List<TemporalGrade> temporalGrades);
        Task DeleteRange(List<TemporalGrade> temporalGrades);
        Task<List<TemporalGrade>> GetAllByFilters(Guid sectionId, Guid evaluationId);
    }
}
