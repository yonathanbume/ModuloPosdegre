using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class StudentCourseCertificateService : IStudentCourseCertificateService
    {
        private readonly IStudentCourseCertificateRepository _studentCourseCertificateRepository;
        public StudentCourseCertificateService(IStudentCourseCertificateRepository studentCourseCertificateRepository)
        {
            _studentCourseCertificateRepository = studentCourseCertificateRepository;
        }

        public async Task Insert(StudentCourseCertificate certificate)
            => await _studentCourseCertificateRepository.Insert(certificate);
    }
}
