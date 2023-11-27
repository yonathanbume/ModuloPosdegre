using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.SyllabusTeacher;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface ISyllabusTeacherService
    {
        Task InsertAsync(SyllabusTeacher syllabusTeacher);
        Task<SyllabusTeacher> Get(Guid id);
        Task Update(SyllabusTeacher entity);
        Task<SyllabusTeacher> GetByCourseTermId(Guid courseTermId);
        Task Delete(SyllabusTeacher syllabusTeacher);
        Task<object> GetChartJsReport(Guid termId, Guid? facultyId);
        Task<DataTablesStructs.ReturnedData<object>> GetSyllabusTeacherDatatable(DataTablesStructs.SentParameters parameters, Guid termId, Guid academicDepartmentId, string teacherSearch, ClaimsPrincipal user = null);
        Task<IEnumerable<SyllabusTeacherTemplate>> GetSyllabusTeacher(Guid termId, string teacherId);
        Task<List<SyllabusTeacherReportTemplate>> GetSyllabusTeacherReport(Guid termId, Guid academicDepartmentId, ClaimsPrincipal user = null);
        Task<List<SyllabusTeacher>> GetSyllabusTeacherCourses(Guid termId, Guid careerId, Guid curriculumId);
        Task<DataTablesStructs.ReturnedData<object>> GetSyllabusTeacherOutOfDateDatatable(DataTablesStructs.SentParameters parameters, Guid termId, Guid? careerId, Guid? curriculumId);
    }
}