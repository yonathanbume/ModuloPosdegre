using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.SyllabusTeacher;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces
{
    public interface ISyllabusTeacherRepository : IRepository<SyllabusTeacher>
    {
        Task<SyllabusTeacher> GetByCourseTermId(Guid courseTermId);
        Task<object> GetChartJsReport(Guid termId, Guid? facultyId);
        Task<DataTablesStructs.ReturnedData<object>> GetSyllabusTeacherDatatable(DataTablesStructs.SentParameters parameters, Guid termId, Guid academicDepartmentId, string teacherSearch, ClaimsPrincipal user = null);
        Task<IEnumerable<SyllabusTeacherTemplate>> GetSyllabusTeacher(Guid termId, string teacherId);
        Task<List<SyllabusTeacherReportTemplate>> GetSyllabusTeacherReport(Guid termId, Guid academicDepartmentId, ClaimsPrincipal user = null);
        Task<List<SyllabusTeacher>> GetSyllabusTeacherCourses(Guid termId, Guid careerId, Guid curriculumId);
        Task<DataTablesStructs.ReturnedData<object>> GetSyllabusTeacherOutOfDateDatatable(DataTablesStructs.SentParameters parameters, Guid termId, Guid? careerId, Guid? curriculumId);
    }
}