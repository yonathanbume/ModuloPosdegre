using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IDirectedCourseStudentService
    {
        Task Insert(DirectedCourseStudent directedCourse);
        Task DeleteById(Guid id);
        Task<DirectedCourseStudent> Get(Guid id);

        Task<DirectedCourseStudent> GetByFilters(Guid? studentId = null, Guid? directedcourseId = null, byte? status = null);
        Task<int> CountAttempts(Guid? studentId = null, Guid? courseId = null);

        Task Update(DirectedCourseStudent directedCourse);
        Task<object> GetStudentsByCourseAndTerm(Guid id, Guid termId);
        //Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, ClaimsPrincipal user = null);

        //Task<DirectedCourseStudent> GetByFilters(Guid? term = null, Guid? careerId = null, Guid? courseId = null);
        //Task<DataTablesStructs.ReturnedData<object>> GetEvaluationReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string teacherId, ClaimsPrincipal user = null);
        //Task<IEnumerable<DirectedCourseStudent>> GetAllByTeacherIdAndCourseId(Guid courseId, string teacherId);
    }
}
