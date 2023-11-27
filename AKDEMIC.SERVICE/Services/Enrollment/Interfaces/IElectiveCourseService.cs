using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface IElectiveCourseService
    {
        Task InsertElectiveCourse(ElectiveCourse electiveCourse);
        Task UpdateElectiveCourse(ElectiveCourse electiveCourse);
        Task DeleteElectiveCourse(ElectiveCourse electiveCourse);
        Task<ElectiveCourse> GetElectiveCourseById(Guid id);
        Task<IEnumerable<ElectiveCourse>> GetAllElectiveCourses();
        Task<IEnumerable<ElectiveCourse>> GetAllElectiveCoursesWithData();
        Task<List<ElectiveCourse>> GetWithDataByCareerIdAndAcadmic(Guid careerId, int academicYear, int academicYearDispersion);
    }
}
