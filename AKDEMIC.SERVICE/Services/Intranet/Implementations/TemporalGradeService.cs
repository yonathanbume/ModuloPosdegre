using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class TemporalGradeService : ITemporalGradeService
    {
        private readonly ITemporalGradeRepository _temporalGradeRepository;
        public TemporalGradeService(ITemporalGradeRepository temporalGradeRepository)
        {
            _temporalGradeRepository = temporalGradeRepository;
        }

        public async Task DeleteRange(List<TemporalGrade> temporalGrades) => await _temporalGradeRepository.DeleteRange(temporalGrades);

        public async Task<List<TemporalGrade>> GetAllByFilters(Guid sectionId, Guid evaluationId) => await _temporalGradeRepository.GetAllByFilters(sectionId, evaluationId);

        public async Task InsertRange(List<TemporalGrade> temporalGrades) => await _temporalGradeRepository.InsertRange(temporalGrades);
    }
}
