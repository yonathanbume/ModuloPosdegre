using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.CourseTerm;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface ICourseTermService
    {
        Task<IEnumerable<CourseTerm>> GetAll();
        Task<bool> AnyByCourseIdAndTermStatus(Guid courseId, int termStatus);
        Task<IEnumerable<CourseTerm>> GetAllByTermAndCareer(Guid? careerId = null,Guid? termId = null);
        Task<CourseTermATemplate> GetCourseTermATemplateById(Guid id);
        Task<CourseTerm> GetAsync(Guid id);
        Task InsertAsync(CourseTerm courseTerm);
        Task UpdateAsync(CourseTerm courseTerm);
        Task DeleteAsync(CourseTerm courseTerm);
        Task<Select2Structs.ResponseParameters> GetCourseTermsByTermSelect2(Select2Structs.RequestParameters requestParameters, Guid termId, string userId=null);
        Task<CourseTerm> GetByFilters(Guid? courseId = null, Guid? termId = null);
        Task<CourseTerm> GetCourseTermWithCourse(Guid ctid);
        Task<DataTablesStructs.ReturnedData<CourseTermDataTemplate>> GetCourseTermDataTable(DataTablesStructs.SentParameters parameters,Guid carId, Guid curId, Guid cid, Guid pid,string search);
        Task<IEnumerable<Select2Structs.Result>> GetCourseTermByTermSelect2ClientSide(Guid termId, string academicCoordinatorId = null, Guid? careerId = null);
        Task<object> GetAsModelA(Guid? termId = null, Guid? courseId = null);

        Task<CourseTerm> GetCourseTermWithTermAndSections(Guid id);
        Task<CourseTerm> GetcourseTermEnrollemtn(Guid courseId, Guid? termId = null);

        Task<object> GetCourseTerms(Guid cid,string q);
        Task<bool> AnyByIdAndTermStatus(Guid id, int termStatus);
        Task DeleteById(Guid id);
        Task AddRangeAsync(CourseTerm[] listCourseTerm);
        Task InsertRange(CourseTerm[] listCourseTerm);
        Task<CourseTerm> GetFirstByCourseAndeTermId(Guid courseId, Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetCourseTermSectionsDataTable(DataTablesStructs.SentParameters parameters, Guid careerId, Guid curriculumId, Guid cid, Guid pid, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetCourseGradeStatisticsDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? careerId = null, Guid? curriculumId = null, ClaimsPrincipal user = null, string search = null);
        Task<DataTablesStructs.ReturnedData<object>> GetCourseAttendanceStatisticsDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? careerId = null, Guid? curriculumId = null, ClaimsPrincipal user = null, string search = null);
        Task<List<CourseStatisticTemplate>> GetCourseAttendanceStatisticsTemplate(Guid termId, Guid? careerId = null, Guid? curriculumId = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetCourseTermReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid curriculumId, int academicYear);
        Task<object> GetCoursesWithSectionsSelect(Guid termId, Guid? careerId, Guid? curriculumId, int? academicYear);
        Task<List<CourseStatisticTemplate>> GetCourseGradeStatisticsTemplate(Guid termId, Guid? careerId = null, Guid? curriculumId = null, ClaimsPrincipal user = null);
    }
}