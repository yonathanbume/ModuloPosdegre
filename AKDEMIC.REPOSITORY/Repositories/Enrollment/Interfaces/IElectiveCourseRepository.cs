using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface IElectiveCourseRepository : IRepository<ElectiveCourse> 
    {
        Task<IEnumerable<ElectiveCourse>> GetAllElectiveCoursesWithData();
        Task<List<ElectiveCourse>> GetWithDataByCareerIdAndAcadmic(Guid careerId, int academicYear, int academicYearDispersion);
    }
}
