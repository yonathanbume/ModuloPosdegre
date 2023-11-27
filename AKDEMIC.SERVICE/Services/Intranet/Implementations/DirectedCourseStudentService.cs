using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class DirectedCourseStudentService : IDirectedCourseStudentService
    {
        private readonly IDirectedCourseStudentRepository _directedCourseRepository;
        public DirectedCourseStudentService(IDirectedCourseStudentRepository directedCourseRepository)
        {
            _directedCourseRepository = directedCourseRepository;
        }

        public Task<int> CountAttempts(Guid? studentId = null, Guid? courseId = null)
            => _directedCourseRepository.CountAttempts(studentId, courseId);

        public Task<DirectedCourseStudent> Get(Guid id) => _directedCourseRepository.Get(id);
        public Task<DirectedCourseStudent> GetByFilters(Guid? studentId = null, Guid? directedcourseId = null, byte? status = null)
            => _directedCourseRepository.GetByFilters(studentId, directedcourseId, status);

        //public async Task<IEnumerable<DirectedCourseStudent>> GetAllByTeacherIdAndCourseId(Guid courseId, string teacherId)
        //    => await _directedCourseRepository.GetAllByTeacherIdAndCourseId(courseId, teacherId);

        //public Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, ClaimsPrincipal user = null)
        //    => _directedCourseRepository.GetDataDatatable(sentParameters, searchValue, user);

        //public async Task<DataTablesStructs.ReturnedData<object>> GetEvaluationReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string teacherId,ClaimsPrincipal user = null)
        //    => await _directedCourseRepository.GetEvaluationReportDatatable(sentParameters, termId, teacherId, user);

        public Task Insert(DirectedCourseStudent directedCourse) => _directedCourseRepository.Insert(directedCourse);

        public Task Update(DirectedCourseStudent directedCourse)
            => _directedCourseRepository.Update(directedCourse);

        public async Task DeleteById(Guid id) => await _directedCourseRepository.DeleteById(id);
        public async Task<object> GetStudentsByCourseAndTerm(Guid id, Guid termId)
        {
            return await _directedCourseRepository.GetStudentsByCourseAndTerm(id, termId);
        }
    }
}
