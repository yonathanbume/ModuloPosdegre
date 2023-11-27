using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IDirectedCourseStudentRepository : IRepository<DirectedCourseStudent>
    {
        //Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, ClaimsPrincipal user = null);

        Task<DirectedCourseStudent> GetByFilters(Guid? studentId = null, Guid? directedcourseId = null, byte? status = null);
        Task<int> CountAttempts(Guid? studentId = null, Guid? courseId = null);
        Task<object> GetStudentsByCourseAndTerm(Guid id, Guid termId);
        //Task<DataTablesStructs.ReturnedData<object>> GetEvaluationReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string teacherId, ClaimsPrincipal user = null);
        //Task<IEnumerable<DirectedCourse>> GetAllByTeacherIdAndCourseId(Guid courseId, string teacherId);
    }
}
