using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.DirectedCourses;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class DirectedCourseService : IDirectedCourseService
    {
        private readonly IDirectedCourseRepository _directedCourseRepository;
        public DirectedCourseService(IDirectedCourseRepository directedCourseRepository)
        {
            _directedCourseRepository = directedCourseRepository;
        }

        public Task<int> CountAttempts(Guid? studentId = null, Guid? courseId = null)
            => _directedCourseRepository.CountAttempts(studentId, courseId);

        public Task<DirectedCourse> Get(Guid id) => _directedCourseRepository.Get(id);

        public Task<DirectedCourse> GetByFilters(Guid? term = null, Guid? careerid = null, Guid? courseId = null)
            => _directedCourseRepository.GetByFilters(term,careerid, courseId);

        public async Task<IEnumerable<DirectedCourse>> GetAllByTeacherIdAndCourseId(Guid courseId, string teacherId)
            => await _directedCourseRepository.GetAllByTeacherIdAndCourseId(courseId, teacherId);
        public async Task<EnrollmentDirectedCourseDataTemplate> GetEnrollmentDirectedCourseData(Guid termId, Guid careerId, Guid curriculums)
            => await _directedCourseRepository.GetEnrollmentDirectedCourseData(termId, careerId, curriculums);
        public Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId, Guid? careerId, Guid? facultyId, Guid? courseId, string searchValue = null, ClaimsPrincipal user = null)
            => _directedCourseRepository.GetDataDatatable(sentParameters, termId, careerId, facultyId, courseId, searchValue, user);
        public Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable2(DataTablesStructs.SentParameters sentParameters, string searchValue = null, ClaimsPrincipal user = null)
         => _directedCourseRepository.GetDataDatatable2(sentParameters, searchValue, user);

        public async Task<List<DirectedCourseStudentTemplate>> GetDirectedCoursesDataReport(Guid? termId, Guid? careerId, Guid? facultyId, Guid? courseId, string searchValue, ClaimsPrincipal user)
        {
            return await _directedCourseRepository.GetDirectedCoursesDataReport(termId, careerId, facultyId, courseId, searchValue, user);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetEvaluationReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string teacherId,ClaimsPrincipal user = null)
            => await _directedCourseRepository.GetEvaluationReportDatatable(sentParameters, termId, teacherId, user);

        public Task Insert(DirectedCourse directedCourse) => _directedCourseRepository.Insert(directedCourse);

        public Task Update(DirectedCourse directedCourse)
            => _directedCourseRepository.Update(directedCourse);

        public async Task DeleteById(Guid id) => await _directedCourseRepository.DeleteById(id);

        public async Task<object> GetAllByStudentAndTerm(Guid termId, Guid studentId)
        {
            return await _directedCourseRepository.GetAllByStudentAndTerm(termId, studentId);
        }

        public async Task<List<DirectedCourse>> GetByCareerAndTerm(Guid careerid,Guid termId)
        {
            return await _directedCourseRepository.GetByCareerAndTerm(careerid,termId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDirectedCoursesByTeacherDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string teacherId)
            => await _directedCourseRepository.GetDirectedCoursesByTeacherDatatable(sentParameters, termId, teacherId);
    }
}
