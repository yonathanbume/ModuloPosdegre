using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.TeacherSection;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class TeacherSectionService : ITeacherSectionService
    {
        private readonly ITeacherSectionRepository _teacherSectionRepository;

        public TeacherSectionService(ITeacherSectionRepository teacherSectionRepository)
        {
            _teacherSectionRepository = teacherSectionRepository;
        }

        Task<IEnumerable<TeacherSection>> ITeacherSectionService.GetAllBySection(Guid sectionId)
            => _teacherSectionRepository.GetAllBySection(sectionId);

        Task<object> ITeacherSectionService.GetAllAsModelAByTeacherId(string teacherId, Guid? termId = null)
            => _teacherSectionRepository.GetAllAsModelAByTeacherId(teacherId, termId);
        Task ITeacherSectionService.UpdateMainTeacher(Guid teacherSectionId, Guid sectionId)
            => _teacherSectionRepository.UpdateMainTeacher(teacherSectionId, sectionId);

        Task ITeacherSectionService.Insert(TeacherSection teacherSection)
            => _teacherSectionRepository.Insert(teacherSection);

        Task ITeacherSectionService.DeleteById(Guid teacherSectionId)
            => _teacherSectionRepository.DeleteById(teacherSectionId);

        Task<DataTablesStructs.ReturnedData<object>> ITeacherSectionService.GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue, Guid? facultyId, Guid? careerId, ClaimsPrincipal user)
            => _teacherSectionRepository.GetDataDatatable(sentParameters, searchValue, facultyId, careerId, user);

        Task<bool> ITeacherSectionService.AnyBySectionAndTeacher(Guid sectionId, string teacherId)
            => _teacherSectionRepository.AnyBySectionAndTeacher(sectionId, teacherId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetSectionsWithTermActiveDatatable(DataTablesStructs.SentParameters sentParameters, Guid facultyId, Guid careerId, Guid courseId)
        {
            return await _teacherSectionRepository.GetSectionsWithTermActiveDatatable(sentParameters,facultyId,careerId,courseId);
        }

        public async Task<IEnumerable<TeacherSectionTemplateC>> GetAllAsModelCByTermId(Guid termId , Guid? careerId = null, string coordinatorId = null,string teacherId = null, ClaimsPrincipal user = null, Guid? academicDepartmentId = null, Guid? curriculumId = null)
            => await _teacherSectionRepository.GetAllAsModelCByTermId(termId,careerId, coordinatorId,teacherId, user, academicDepartmentId, curriculumId);

        public async Task<int> CountStudentsInSectionsWithTermActive(Guid facultyId, Guid careerId, Guid courseId)
        {
            return await _teacherSectionRepository.CountStudentsInSectionsWithTermActive(facultyId,careerId,courseId);
        }

        public async Task<List<TeacherSection>> GetAllSectionsWithTermActiveWithIncludes(Guid facultyId, Guid careerId, Guid courseId)
        {
            return await _teacherSectionRepository.GetAllSectionsWithTermActiveWithIncludes(facultyId,careerId,courseId);
        }

        public async Task<object> GetSectionsByUser(string userId, string term)
            => await _teacherSectionRepository.GetSectionsByUser(userId, term);

        public async Task<Section> GetTeacherSectionsWithTermAndCareer(Guid sectionId)
            => await _teacherSectionRepository.GetTeacherSectionsWithTermAndCareer(sectionId);

        public async Task<TeacherSection> GetTeacherSectionBySection(Guid sectionId)
            => await _teacherSectionRepository.GetTeacherSectionBySection(sectionId);

        public Task<object> GetAllAsModelDByTermIdAndTeacherId(Guid termId, string teacherId)
            => _teacherSectionRepository.GetAllAsModelDByTermIdAndTeacherId(termId, teacherId);

        public Task<object> GetAllAsSelect2ClientSide(Guid? sectionId = null)
            => _teacherSectionRepository.GetAllAsSelect2ClientSide(sectionId);

        public Task InsertRangeAsync(IEnumerable<TeacherSection> teacherSections)
            => _teacherSectionRepository.InsertRange(teacherSections);

        public Task DeleteRangeAsync(IEnumerable<TeacherSection> teacherSections)
            => _teacherSectionRepository.DeleteRange(teacherSections);

        public Task UpdateRangeAsync(IEnumerable<TeacherSection> teacherSections)
            => _teacherSectionRepository.UpdateRange(teacherSections);

        public Task<object> GetAllAsSelect2ClientSide2(Guid? courseId = null, Guid? termId=null)
            => _teacherSectionRepository.GetAllAsSelect2ClientSide2(courseId,termId);
        public async Task<object> GetSectionTeachersJson(Guid sid)
            => await _teacherSectionRepository.GetSectionTeachersJson(sid);

        public Task<IEnumerable<TeacherSectionTemplateZ>> GetReportDatatable(Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null)
            => _teacherSectionRepository.GetReportDatatable(facultyId, careerId, user);

        public async Task SaveChanges()
        {
            await _teacherSectionRepository.SaveChanges();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherSectionByTeacherId(DataTablesStructs.SentParameters sentParameters, string teacherId, Guid termId, bool withDirectedCourse = false)
            => await _teacherSectionRepository.GetTeacherSectionByTeacherId(sentParameters, teacherId, termId, withDirectedCourse);

        public async Task<double> GetTotalHoursSectionsByTeacherAndTermId(string teacherId, Guid termId)
            => await _teacherSectionRepository.GetTotalHoursSectionsByTeacherAndTermId(teacherId, termId);

        public async Task<TeacherSection> Get(Guid id)
            => await _teacherSectionRepository.Get(id);

        public async Task<TeacherSection> GetByTeacherAndSectionId(string teacherId, Guid sectionId)
            => await _teacherSectionRepository.GetByTeacherAndSectionId(teacherId, sectionId);

        public async Task Update(TeacherSection entity)
            => await _teacherSectionRepository.Update(entity);

        public async Task<List<TeacherSection>> GetListBySectionId(Guid sectionId)
            => await _teacherSectionRepository.GetListBySectionId(sectionId);

        public async Task<IEnumerable<TeacherSection>> GetTeacherSectionsByTermIdAndCourseId(Guid termId, Guid courseId)
            => await _teacherSectionRepository.GetTeacherSectionsByTermIdAndCourseId(termId, courseId);

        public Task<Select2Structs.ResponseParameters> GetTeachersSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null, Guid? termId = null, Guid? careerId = null)
            => _teacherSectionRepository.GetTeachersSelect2(requestParameters,searchValue,termId,careerId);

        public async Task<object> GetTeacherCoursesSelect2(string teacherId, bool showDirectedCourses = false)
            => await _teacherSectionRepository.GetTeacherCoursesSelect2(teacherId, showDirectedCourses);

        public async Task<object> GetTeacherCourseSectionsSelect2(string teacherId, Guid courseTermId, bool showDirectedCourses = false)
            => await _teacherSectionRepository.GetTeacherCourseSectionsSelect2(teacherId, courseTermId, showDirectedCourses);

        public async Task<List<ConsolidatedAcademicLoadReport>> GetConsolidatedAcademicLoadReport(Guid termId, Guid? academicDepartmentId, bool viewAll)
            => await _teacherSectionRepository.GetConsolidatedAcademicLoadReport(termId, academicDepartmentId, viewAll);

        public Task<DataTablesStructs.ReturnedData<object>> GetTeachersByAcademicDepartmentDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? academicDepartmentId = null)
            => _teacherSectionRepository.GetTeachersByAcademicDepartmentDatatable(sentParameters, termId, academicDepartmentId);

        public Task<object> GetTeachersByAcademicDepartmentChart(Guid? termId = null, Guid? academicDepartmentId = null)
            => _teacherSectionRepository.GetTeachersByAcademicDepartmentChart(termId, academicDepartmentId);

        public Task<int> GetMagisterByTermId(Guid? termId = null, ClaimsPrincipal user = null)
            => _teacherSectionRepository.GetMagisterByTermId(termId, user);

        public Task<int> GetDoctorByTermId(Guid? termId = null, ClaimsPrincipal user = null)
            => _teacherSectionRepository.GetDoctorByTermId(termId, user);
    }
}