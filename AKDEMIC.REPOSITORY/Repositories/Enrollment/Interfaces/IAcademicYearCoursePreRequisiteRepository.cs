using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface IAcademicYearCoursePreRequisiteRepository : IRepository<AcademicYearCoursePreRequisite>
    {
        Task<IEnumerable<AcademicYearCoursePreRequisite>> GetAllByFilter(byte? academicYear = null, Guid? academicYearCourseId = null, Guid? curriculumId = null);
        Task<bool> AnyByAcademicYearCourseAndCourseId(Guid academicYearCourseId, Guid courseId);
    }
}