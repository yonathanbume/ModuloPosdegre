using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.CourseSyllabus;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public class CourseSyllabusService : ICourseSyllabusService
    {
        private readonly ICourseSyllabusRepository _courseSyllabusRepository;

        public CourseSyllabusService(ICourseSyllabusRepository courseSyllabusRepository)
        {
            _courseSyllabusRepository = courseSyllabusRepository;
        }

        public Task DeleteAsync(CourseSyllabus courseSyllabus)
            => _courseSyllabusRepository.Delete(courseSyllabus);

        public Task<CourseSyllabusTemplateA> GetAsModelAByFilter(Guid? courseId = null, Guid? termId = null)
            => _courseSyllabusRepository.GetAsModelAByFilter(courseId, termId);

        public Task<CourseSyllabus> GetAsync(Guid id)
            => _courseSyllabusRepository.Get(id);

        public async Task<CourseSyllabus> GetByCourseIdAndTermId(Guid courseId, Guid termId)
            => await _courseSyllabusRepository.GetByCourseIdAndTermId(courseId, termId);

        public Task<Guid> GetIdByCourseIdAndTermId(Guid courseId, Guid termId)
            => _courseSyllabusRepository.GetIdByCourseIdAndTermId(courseId, termId);

        public async Task<CourseSyllabus> GetIncludingTermAndCourse(Guid courseId, Guid termId) =>
            await _courseSyllabusRepository.GetIncludingTermAndCourse(courseId, termId);

        public async Task<CourseSyllabus> GetIncludingTermAndCourse(Guid syllabusId) =>
            await _courseSyllabusRepository.GetIncludingTermAndCourse(syllabusId);

        public Task InsertAsync(CourseSyllabus courseSyllabus)
            => _courseSyllabusRepository.Insert(courseSyllabus);

        public Task UpdateAsync(CourseSyllabus courseSyllabus)
            => _courseSyllabusRepository.Update(courseSyllabus);
    }
}