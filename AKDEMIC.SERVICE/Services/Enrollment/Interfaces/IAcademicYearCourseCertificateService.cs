using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IAcademicYearCourseCertificateService
    {
        Task DeleteRange(List<AcademicYearCourseCertificate> certificates);
        Task InsertRange(List<AcademicYearCourseCertificate> certificates);
        Task<List<AcademicYearCourseCertificate>> GetCourseCertificates(Guid academicYearCourseId);
    }
}
