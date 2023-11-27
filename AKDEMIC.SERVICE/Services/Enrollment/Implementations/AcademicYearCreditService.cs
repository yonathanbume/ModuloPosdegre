using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class AcademicYearCreditService : IAcademicYearCreditService
    {
        private readonly IAcademicYearCreditRepository _academicYearCreditRepository;
        public AcademicYearCreditService(IAcademicYearCreditRepository academicYearCreditRepository)
        {
            _academicYearCreditRepository = academicYearCreditRepository;
        }

        public async Task<AcademicYearCredit> Get(params object[] keyValues) => await _academicYearCreditRepository.Get(keyValues);

        public async Task<List<AcademicYearCredit>> GetCurriculumAcademicYearCredits(Guid curriculumId)
            => await _academicYearCreditRepository.GetCurriculumAcademicYearCredits(curriculumId);

        public async Task InsertRange(List<AcademicYearCredit> academicYearCredits)
            => await _academicYearCreditRepository.InsertRange(academicYearCredits);

        public async Task UpdateRange(List<AcademicYearCredit> academicYearCredits)
            => await _academicYearCreditRepository.UpdateRange(academicYearCredits);
    }
}
