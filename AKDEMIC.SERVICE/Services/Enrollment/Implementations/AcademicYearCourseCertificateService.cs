using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class AcademicYearCourseCertificateService : IAcademicYearCourseCertificateService
    {
        private readonly IAcademicYearCourseCertificateRepository _academicYearCourseCertificateRepository;
        public AcademicYearCourseCertificateService(IAcademicYearCourseCertificateRepository academicYearCourseCertificateRepository)
        {
            _academicYearCourseCertificateRepository = academicYearCourseCertificateRepository;
        }

        public async Task DeleteRange(List<AcademicYearCourseCertificate> certificates)
            => await _academicYearCourseCertificateRepository.DeleteRange(certificates);

        public async Task<List<AcademicYearCourseCertificate>> GetCourseCertificates(Guid academicYearCourseId)
            => await _academicYearCourseCertificateRepository.GetCourseCertificates(academicYearCourseId);

        public async Task InsertRange(List<AcademicYearCourseCertificate> certificates)
            => await _academicYearCourseCertificateRepository.InsertRange(certificates);
    }
}
