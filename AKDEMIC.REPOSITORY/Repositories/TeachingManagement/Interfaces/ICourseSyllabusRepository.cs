using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.CourseSyllabus;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces
{
    public interface ICourseSyllabusRepository : IRepository<CourseSyllabus>
    {
        Task<CourseSyllabus> GetIncludingTermAndCourse(Guid courseId, Guid termId);
        Task<CourseSyllabusTemplateA> GetAsModelAByFilter(Guid? courseId = null, Guid? termId = null);
        Task<Guid> GetIdByCourseIdAndTermId(Guid courseId, Guid termId);
        Task<CourseSyllabus> GetIncludingTermAndCourse(Guid syllabusId);
        Task<CourseSyllabus> GetByCourseIdAndTermId(Guid courseId, Guid termId);
    }
}