using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface IAcademicYearCourseCertificateRepository : IRepository<AcademicYearCourseCertificate>
    {
        Task<List<AcademicYearCourseCertificate>> GetCourseCertificates(Guid academicYearCourseId);
    }
}
