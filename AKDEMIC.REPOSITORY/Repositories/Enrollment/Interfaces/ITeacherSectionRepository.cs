using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.TeacherSection;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces
{
    public interface ITeacherSectionRepository : IRepository<TeacherSection>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null);
        Task<IEnumerable<TeacherSectionTemplateZ>> GetReportDatatable(Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetSectionsWithTermActiveDatatable(DataTablesStructs.SentParameters sentParameters, Guid facultyId, Guid careerId, Guid courseId);
        Task<int> CountStudentsInSectionsWithTermActive(Guid facultyId, Guid careerId, Guid courseId);
        Task<IEnumerable<TeacherSection>> GetAllBySection(Guid sectionId);
        Task<object> GetAllAsModelAByTeacherId(string teacherId, Guid? termId = null);
        Task<IEnumerable<TeacherSectionTemplateC>> GetAllAsModelCByTermId(Guid termId, Guid? careerId = null, string coordinatorId = null,string teacherId=null, ClaimsPrincipal user = null, Guid? academicDepartmentId = null, Guid? curriculumId = null);
        Task<object> GetAllAsModelDByTermIdAndTeacherId(Guid termId, string teacherId);
        Task<bool> AnyBySectionAndTeacher(Guid sectionId, string teacherId);
        Task<List<TeacherSection>> GetAllSectionsWithTermActiveWithIncludes(Guid facultyId, Guid careerId, Guid courseId);
        Task<int> GetMagisterByTermId(Guid? termId = null, ClaimsPrincipal user = null);
        Task<int> GetDoctorByTermId(Guid? termId = null, ClaimsPrincipal user = null);
        Task UpdateMainTeacher(Guid teacherSectionId, Guid sectionId);
        Task<object> GetSectionsByUser(string userId, string term);
        Task<Section> GetTeacherSectionsWithTermAndCareer(Guid sectionId);
        Task<TeacherSection> GetTeacherSectionBySection(Guid sectionId);
        Task<object> GetAllAsSelect2ClientSide(Guid? sectionId = null);
        Task<object> GetAllAsSelect2ClientSide2(Guid? courseId = null, Guid? termId = null);
        Task<object> GetSectionTeachersJson(Guid sid);
        Task SaveChanges();
        Task<double> GetTotalHoursSectionsByTeacherAndTermId(string teacherId, Guid termId);
        Task<DataTablesStructs.ReturnedData<object>> GetTeacherSectionByTeacherId(DataTablesStructs.SentParameters sentParameters, string teacherId, Guid termId, bool withDirectedCourse = false);
        Task<DataTablesStructs.ReturnedData<object>> GetTeachersByAcademicDepartmentDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? academicDepartmentId = null);
        Task<object> GetTeachersByAcademicDepartmentChart(Guid? termId = null, Guid? academicDepartmentId = null);
        Task<TeacherSection> GetByTeacherAndSectionId(string teacherId, Guid sectionId);
        Task<List<TeacherSection>> GetListBySectionId(Guid sectionId);
        Task<IEnumerable<TeacherSection>> GetTeacherSectionsByTermIdAndCourseId(Guid termId, Guid courseId);
        Task<Select2Structs.ResponseParameters> GetTeachersSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null, Guid? termId = null, Guid? careerId = null);
        Task<object> GetTeacherCoursesSelect2(string teacherId, bool showDirectedCourses = false);
        Task<object> GetTeacherCourseSectionsSelect2(string teacherId, Guid courseTermId, bool showDirectedCourses = false);
        Task<List<ConsolidatedAcademicLoadReport>> GetConsolidatedAcademicLoadReport(Guid termId, Guid? academicDepartmentId, bool viewAll);
    }
}