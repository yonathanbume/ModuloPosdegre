using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.DirectedCourses;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IDirectedCourseRepository : IRepository<DirectedCourse>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId, Guid? careerId, Guid? facultyId, Guid? courseId, string searchValue = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable2(DataTablesStructs.SentParameters sentParameters, string searchValue = null, ClaimsPrincipal user = null);
        Task<object> GetAllByStudentAndTerm(Guid termId, Guid studentId);
        Task<DirectedCourse> GetByFilters(Guid? term = null, Guid? careerid = null, Guid? courseId = null);
        Task<int> CountAttempts(Guid? studentId = null, Guid? courseId = null);
        Task<List<DirectedCourseStudentTemplate>> GetDirectedCoursesDataReport(Guid? termId, Guid? careerId, Guid? facultyId, Guid? courseId, string searchValue, ClaimsPrincipal user);
        Task<DataTablesStructs.ReturnedData<object>> GetEvaluationReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string teacherId, ClaimsPrincipal user = null);
        Task<IEnumerable<DirectedCourse>> GetAllByTeacherIdAndCourseId(Guid courseId, string teacherId);
        Task<EnrollmentDirectedCourseDataTemplate> GetEnrollmentDirectedCourseData(Guid termId, Guid careerId, Guid curriculums);
        Task<List<DirectedCourse>> GetByCareerAndTerm(Guid careerid, Guid termId);
        Task<DataTablesStructs.ReturnedData<object>> GetDirectedCoursesByTeacherDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string teacherId);
    }
}
