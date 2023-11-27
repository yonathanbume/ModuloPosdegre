using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class CourseSyllabusTeacherService : ICourseSyllabusTeacherService
    {
        private readonly ICourseSyllabusTeacherRepository _courseSyllabusTeacherRepository;

        public CourseSyllabusTeacherService(ICourseSyllabusTeacherRepository courseSyllabusTeacherRepository)
        {
            _courseSyllabusTeacherRepository = courseSyllabusTeacherRepository;
        }

        public async Task Add(CourseSyllabusTeacher entity)
            => await _courseSyllabusTeacherRepository.Add(entity);

        public async Task Delete(CourseSyllabusTeacher entity)
            => await _courseSyllabusTeacherRepository.Delete(entity);

        public async Task<List<CourseSyllabusTeacher>> GetByCourseSyllabusId(Guid courseSyllabusId)
            => await _courseSyllabusTeacherRepository.GetByCourseSyllabusId(courseSyllabusId);

        public async Task<CourseSyllabusTeacher> GetByTeacherIdAndCourseSyllabusId(string teacherId, Guid courseSyllabusId)
            => await _courseSyllabusTeacherRepository.GetByTeacherIdAndCourseSyllabusId(teacherId, courseSyllabusId);

        public async Task Insert(CourseSyllabusTeacher entity)
            => await _courseSyllabusTeacherRepository.Insert(entity);

        public async Task Update(CourseSyllabusTeacher entity)
            => await _courseSyllabusTeacherRepository.Update(entity);
    }
}
