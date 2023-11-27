using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Course;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using static AKDEMIC.CORE.Structs.DataTablesStructs;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface ICourseService
    {
        Task<bool> AnyByCode(string code, Guid? id = null);
        Task<bool> AnyInArea(Guid areaId);
        Task<bool> AnyByCodeAndName(string code, string name);
        Task<Course> GetAsync(Guid id);
        Task<Course> GetByCode(string code);
        Task<int> Count();
        Task<bool> AnyCourseTerm(Guid courseId);
        Task<IEnumerable<Course>> GetAllByTeacherId(string teacherId, Guid termId);

        Task<DataTablesStructs.ReturnedData<object>> GetAllWithSyllabusByTermAndPaginationParameters(DataTablesStructs.SentParameters sentParameters, Guid termId);
        Task<List<CourseTeacherReportExcel>> GetCourseTeacherExcel(ClaimsPrincipal user, Guid termId, Guid? careerId, Guid? academicProgramId, Guid? curriculumId, byte? academicYear, bool? onlyWithSections, bool? onlyWithOutCoordinators);
        Task<DataTablesStructs.ReturnedData<object>> GetAllByTermAndAreaCareerAndAcademicYearAndPaginationParameters(DataTablesStructs.SentParameters sentParameters, Guid termId,
            Guid areaCareerId, Guid acaprog, byte? academicYear, string searchValue = null, bool? onlyWithSections = null, ClaimsPrincipal user = null, bool? onlyWithOutCoordinatos = null, Guid? curriculumId = null);

        Task<IEnumerable<CourseATemplate>> GetCoursesATemplate(Guid termId, Guid? careerId, Guid? courseTypeId, ClaimsPrincipal user = null);
        Task<CourseBTemplate> GetCourseBTemplate(Guid courseTermId);
        Task<DataTablesStructs.ReturnedData<object>> GetAllWithSyllabusComplianceDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, int state, Guid careerOrAreaId, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAllByTermIdAndPaginationParameters(DataTablesStructs.SentParameters sentParameters, Guid termId, string name = null);
        Task<IEnumerable<Select2Structs.Result>> GetCoursesSelect2ClientSide(Guid termId,Guid facultyId ,Guid academicProgramId, Guid? curriculumId, string search = null);
        Task<IEnumerable<Select2Structs.Result>> GetCoursesSelect2ClientSide(Guid? careerId = null, string search = null, Guid? curriculumId = null, bool onlyWithSections = false);
       

        Task InsertAsync(Course course);
        Task UpdateAsync(Course course);
        Task DeleteAsync(Course course);
        Task<object> GetAllAsSelectClientSide(string name = null, Guid? careerId = null, Guid? academicProgramId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAllAsModelB(SentParameters sentParameters, Guid termId, Guid? careerId = null, string teacherId = null, bool forCareerDirector = false, Guid? areaCareerId = null, Guid? planId = null, string coordinatorId = null, Guid? programId = null, int? cycle = null, string search = null, ClaimsPrincipal user = null, bool? withSections = null);
        Task<Select2Structs.ResponseParameters> GetCoursesServerSideSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null, Guid? selectedId = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAllByParameters(DataTablesStructs.SentParameters sentParameters, Guid pid, Guid aid, Guid cid, byte? ayid, Guid apid, string search);
        Task DeleteById(Guid id);
        Task<object> GetWithAcademicHistory(Guid id, Guid studentId, Guid curriculumId);
        Task<IEnumerable<Course>> GetAll();
        Task InsertRange(Course[] listCourse);
        Task UpdateCourseCodeJob();
        Task<Course> GetCourseBySectionId(Guid sectionId);
        Task<Select2Structs.ResponseParameters> GetCoursesSelect2(Select2Structs.RequestParameters requestParameters, ClaimsPrincipal claimsPrincipal = null, string searchValue = null, Guid? academicProgramId = null, bool? generalCourses = null);
 
        Task<DataTablesStructs.ReturnedData<object>> GetCoursesDatatble(DataTablesStructs.SentParameters parameters, Guid? termId, Guid? areaCareerId, Guid? academicProgramId, Guid? curriculumId, int? cycle, ClaimsPrincipal user, string searchValue);
        Task<DataTablesStructs.ReturnedData<object>> GetCoursesDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, Guid? programId = null, Guid? curriculumId = null, int? cycle = null, string search = null, ClaimsPrincipal user = null, Guid? termId = null);
        Task<EditTemplate> GetCourseEditTemplate(Guid id);
        Task<object> GetTeacherCoursesSelect2ClientSide(string teacherId, Guid? termId = null);
        Task<int> GetQuantityCoursesAssigned(Guid termId, string teacherId);
        Task<List<CourseScheduleLoad>> GetCourseSectionsScheduleLoad(Guid termId, Guid? careerId, Guid? academicProgramId, Guid? curriculumId, int? academicYear);
        Task<DataTablesStructs.ReturnedData<object>> GetCurriculumsByCourseDatatable(DataTablesStructs.SentParameters parameters, Guid courseId);
    }
}