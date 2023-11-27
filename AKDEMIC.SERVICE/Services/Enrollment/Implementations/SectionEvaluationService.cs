using AKDEMIC.CORE.Structs;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class SectionEvaluationService : ISectionEvaluationService
    {
        private readonly ISectionEvaluationRepository _sectionEvaluationRepository;

        public SectionEvaluationService(ISectionEvaluationRepository sectionEvaluationRepository)
        {
            _sectionEvaluationRepository = sectionEvaluationRepository;
        }

        public async Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters, Guid courseId, Guid termId, Guid? sectionId)
            => await _sectionEvaluationRepository.GetDatatable(sentParameters, courseId, termId, sectionId);
    }
}
