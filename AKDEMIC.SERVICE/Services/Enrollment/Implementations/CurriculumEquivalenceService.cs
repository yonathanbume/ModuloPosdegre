using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class CurriculumEquivalenceService : ICurriculumEquivalenceService
    {
        private readonly ICurriculumEquivalenceRepository _curriculumEquivalenceRepository;
        public CurriculumEquivalenceService(ICurriculumEquivalenceRepository curriculumEquivalenceRepository)
        {
            _curriculumEquivalenceRepository = curriculumEquivalenceRepository;
        }

        public async Task Delete(CurriculumEquivalence curriculumEquivalence)
            => await _curriculumEquivalenceRepository.Delete(curriculumEquivalence);

        public async Task<CurriculumEquivalence> GetByNewCurriculumId(Guid curriculumId)
            => await _curriculumEquivalenceRepository.GetByNewCurriculumId(curriculumId);

        public async Task Insert(CurriculumEquivalence curriculumEquivalence)
            => await _curriculumEquivalenceRepository.Insert(curriculumEquivalence);
    }
}
