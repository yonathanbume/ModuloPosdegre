using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.CourseSyllabus;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface ICourseSyllabusService
    {
        Task<CourseSyllabus> GetIncludingTermAndCourse(Guid courseId, Guid termId);
        Task<Guid> GetIdByCourseIdAndTermId(Guid courseId, Guid termId);
        Task<CourseSyllabusTemplateA> GetAsModelAByFilter(Guid? courseId = null, Guid? termId = null);

        Task<CourseSyllabus> GetIncludingTermAndCourse(Guid syllabusId);
        Task<CourseSyllabus> GetAsync(Guid id);
        Task InsertAsync(CourseSyllabus courseSyllabus);
        Task UpdateAsync(CourseSyllabus courseSyllabus);
        Task DeleteAsync(CourseSyllabus courseSyllabus);

        Task<CourseSyllabus> GetByCourseIdAndTermId(Guid courseId, Guid termId);
    }
}