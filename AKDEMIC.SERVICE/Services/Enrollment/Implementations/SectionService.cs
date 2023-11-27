using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Section;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.SubstituteExam;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class SectionService : ISectionService
    {
        private readonly ISectionRepository _sectionRepository;

        public SectionService(ISectionRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }

        public async Task<Tuple<string, int, string[]>> GetSpecificSectionDate(Guid sectionId)
            => await _sectionRepository.GetSpecificSectionDate(sectionId);

        public async Task<Section> GetWithIncludes(Guid sectionId)
            => await _sectionRepository.GetWithIncludes(sectionId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllByCourseAndTermAndPaginationParameters(DataTablesStructs.SentParameters sentParameters, Guid courseId, Guid termId)
            => await _sectionRepository.GetAllByCourseAndTermAndPaginationParameters(sentParameters, courseId, termId);

        public async Task<IEnumerable<Section>> GetAllByCourseAndTerm(Guid courseId, Guid termId, string teacherId = null) =>
            await _sectionRepository.GetAllByCourseAndTerm(courseId, termId, teacherId);

        public async Task<int> CountByCourseAndTerm(Guid courseId, Guid termId) =>
            await _sectionRepository.CountByCourseAndTerm(courseId, termId);

        public async Task<Section> Get(Guid id) => await _sectionRepository.Get(id);

        public async Task<object> GetSectionsByCourseTermId(Guid courseTermId)
            => await _sectionRepository.GetSectionsByCourseTermId(courseTermId);

        public async Task<DataTablesStructs.ReturnedData<Section>> GetSectionsByTermAndStudentIdDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? studentId = null, string searchValue = null)
            => await _sectionRepository.GetSectionsByTermAndStudentIdDatatable(sentParameters, termId, studentId, searchValue);

        public async Task<IEnumerable<Section>> GetAvailableSectionsByCourseTermId(Guid? termId, Guid? currentSectionId = null)
            => await _sectionRepository.GetAvailableSectionsByCourseTermId(termId, currentSectionId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllByCourseComponentTermFacultyAndPaginationParameters(DataTablesStructs.SentParameters sentParameters, Guid component, Guid termId, Guid faculty, string search)
            => await _sectionRepository.GetAllByCourseComponentTermFacultyAndPaginationParameters(sentParameters, component, termId, faculty, search);
        public async Task<List<ProcessedSectionsTemplate>> GetGradeRegistrationReport(Guid component, Guid termId, Guid faculty)
            => await _sectionRepository.GetGradeRegistrationReport(component, termId, faculty);
        public async Task<DataTablesStructs.ReturnedData<object>> GetAllByCourseTermLatFilterAndPaginationParameters(DataTablesStructs.SentParameters sentParameters, Guid term, int lateFilter, string search)
            => await _sectionRepository.GetAllByCourseTermLatFilterAndPaginationParameters(sentParameters, term, lateFilter, search);

        public async Task<IEnumerable<Section>> GetAll(string teacherId = null, Guid? studentId = null, Guid? termId = null, Guid? courseId = null, Guid? courseTermId = null, bool showDirectedCourses = false)
            => await _sectionRepository.GetAll(teacherId, studentId, termId, courseId, courseTermId, showDirectedCourses);

        public Task<IEnumerable<SectionTemplateA>> GetAllByTermIdAsModelA(Guid termId, Guid? careerId = null, string coordinatorId = null, Guid? academicDepartmentId = null, ClaimsPrincipal user = null, Guid? curriculumId = null)
            => _sectionRepository.GetAllByTermIdAsModelA(termId, careerId, coordinatorId, academicDepartmentId, user, curriculumId);

        public async Task<Section> GetWithTeacherSections(Guid id)
            => await _sectionRepository.GetWithTeacherSections(id);

        public async Task<Section> GetWithEvaluations(Guid id)
            => await _sectionRepository.GetWithEvaluations(id);

        public async Task<Section> GetWithCourseTermCareer(Guid id)
            => await _sectionRepository.GetWithCourseTermCareer(id);

        public async Task<List<TeacherSectionTemplate>> GetTecachersByTerm(Guid termId)
            => await _sectionRepository.GetTecachersByTerm(termId);

        public async Task<IEnumerable<Select2Structs.Result>> GetSectionsByTermIdAndTeacherIdSelect2ClientSide(Guid termId, string teacherId)
            => await _sectionRepository.GetSectionsByTermIdAndTeacherIdSelect2ClientSide(termId, teacherId);

        public async Task<IEnumerable<Select2Structs.Result>> GetSectionsByStudentIdAndTermIdSelect2ClientSide(Guid studentId, Guid termId)
            => await _sectionRepository.GetSectionsByStudentIdAndTermIdSelect2ClientSide(studentId, termId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetEvaluationReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? faculty, Guid? termId, string teacher, string teacherCode = null, ClaimsPrincipal user = null, Guid? careerId = null, Guid? departmentId = null, string courseName = null, Guid? curriculumId = null, string evaluationCode = null, Guid? academicProgramId = null)
            => await _sectionRepository.GetEvaluationReportDatatable(sentParameters, faculty, termId, teacher, teacherCode, user, careerId, departmentId, courseName, curriculumId, evaluationCode, academicProgramId);

        public async Task<Section> GetSectionWithTermAndCareer(Guid sectionId)
            => await _sectionRepository.GetSectionWithTermAndCareer(sectionId);

        public async Task<Section> First()
        {
            return await _sectionRepository.First();
        }

        public Task<IEnumerable<Select2Structs.Result>> GetSectionsByCourseTermIdSelect2ClientSide(Guid courseTermId)
            => _sectionRepository.GetSectionsByCourseTermIdSelect2ClientSide(courseTermId);

        public Task InsertAsync(Section section)
            => _sectionRepository.Insert(section);

        public Task UpdateAsync(Section section)
            => _sectionRepository.Update(section);

        public Task DeleteAsync(Section section)
            => _sectionRepository.Delete(section);

        public Task<object> GetAllAsModelB(Guid? termId = null, Guid? courseId = null)
            => _sectionRepository.GetAllAsModelB(termId, courseId);

        public Task<object> GetAsModelC(Guid? id = null)
            => _sectionRepository.GetAsModelC(id);

        public Task<Section> GetWithTeacherSectionsAndClassSchedules(Guid id)
            => _sectionRepository.GetWithTeacherSectionsAndClassSchedules(id);

        public Task<Section> GetWithCourseTermAndClassSchedules(Guid id)
            => _sectionRepository.GetWithCourseTermAndClassSchedules(id);

        public Task<object> GetStudentAvailableSectionsDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId, Guid courseId, Guid termId)
            => _sectionRepository.GetStudentAvailableSectionsDatatable(sentParameters, studentId, courseId, termId);

        public async Task<List<Section>> GetSectionWithStudentSection(Guid courseId, Guid? termId = null)
            => await _sectionRepository.GetSectionWithStudentSection(courseId, termId);

        public async Task<Section> GetStudentSectionWithClassCourse(Guid id)
            => await _sectionRepository.GetStudentSectionWithClassCourse(id);

        public async Task<Section> GetSectionIncludeClassStudentAndCourse(Guid id)
            => await _sectionRepository.GetSectionIncludeClassStudentAndCourse(id);

        public async Task<object> GetCoruseSections(List<ClassSchedule> studentSchedules, Guid studentId, Guid courseId, Guid? termId = null)
            => await _sectionRepository.GetCoruseSections(studentSchedules, studentId, courseId, termId);

        public async Task<IEnumerable<Select2Structs.Result>> GetSectionsByTermIdSelect2ClientSide(Guid termId, Guid courseId)
        {
            return await _sectionRepository.GetSectionsByTermIdSelect2ClientSide(termId, courseId);
        }

        public async Task<object> GetSectionsByCareerSelect2ClientSide(Guid? careerId)
        {
            return await _sectionRepository.GetSectionsByCareerSelect2ClientSide(careerId);
        }
        public async Task<object> GetSectionByCourseId(Guid courseId)
            => await _sectionRepository.GetSectionByCourseId(courseId);
        public async Task<DataTablesStructs.ReturnedData<object>> GetSectionsByTermCareerCurriculumCycleCourseDataTable(DataTablesStructs.SentParameters parameters, Guid termid, Guid? careerId, Guid? curriculumId, int cycleId, Guid? courseId, ClaimsPrincipal user = null, string search = null)
        {
            return await _sectionRepository.GetSectionsByTermCareerCurriculumCycleCourseDataTable(parameters, termid, careerId, curriculumId, cycleId, courseId, user, search);
        }

        public async Task<SectionSubstituteExamViewModel> GetSubstituteSectionById(Guid sectionId)
        {
            return await _sectionRepository.GetSubstituteSectionById(sectionId);
        }
        public async Task<object> GetSectionByCourseTermIdSelect2(Guid coursetermId)
            => await _sectionRepository.GetSectionByCourseTermIdSelect2(coursetermId);

        public async Task<IEnumerable<Guid>> GetListSectionIdToGeneratedEvaluationReportInBlock(Guid termId, string teacherId = null, Guid? careerId = null)
            => await _sectionRepository.GetListSectionIdToGeneratedEvaluationReportInBlock(termId, teacherId, careerId);

        public async Task<int> CountByGroupId(Guid id)
        {
            return await _sectionRepository.CountByGroupId(id);
        }

        public async Task<bool> AvalableBySectionId(Guid id)
        {
            return await _sectionRepository.AvalableBySectionId(id);
        }

        public async Task AddAsync(Section section)
        {
            await _sectionRepository.Add(section);
        }

        public async Task AddRangeAsync(Section[] listSection)
        {
            await _sectionRepository.AddRange(listSection);
        }

        public async Task InsertRange(Section[] listSection)
        {
            await _sectionRepository.InsertRange(listSection);
        }

        public async Task<Section> GetFirstBySectionCodeCourseAndTermId(string sectionCode, Guid courseId, Guid termId)
        {
            return await _sectionRepository.GetFirstBySectionCodeCourseAndTermId(sectionCode, courseId, termId);
        }

        public Task<object> GetAllCourseSectionsDataTableClientSide(Guid courseId, Guid termId, string search = null) => _sectionRepository.GetAllCourseSectionsDataTableClientSide(courseId, termId, search);

        public async Task<object> GetJsonByCourseAndTermId(Guid courseId, Guid termId, bool withDirectedCourses = false)
            => await _sectionRepository.GetJsonByCourseAndTermId(courseId, termId, withDirectedCourses);

        public async Task<DataTablesStructs.ReturnedData<object>> GetSummerEnrollmentDataTable(DataTablesStructs.SentParameters sentParameters, Guid termid, Guid? careerId = null, Guid? curriculumId = null, Guid? programId = null, int? cycle = null, Guid? courseId = null, string search = null, ClaimsPrincipal user = null)
            => await _sectionRepository.GetSummerEnrollmentDataTable(sentParameters, termid, careerId, curriculumId, programId, cycle, courseId, search, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetSectionsToAssignSectionGroup(DataTablesStructs.SentParameters sentParameters, Guid? careerId, Guid? academicProgramId, string searchValue = null, ClaimsPrincipal user = null)
            => await _sectionRepository.GetSectionsToAssignSectionGroup(sentParameters, careerId, academicProgramId, searchValue, user);

        public async Task<Section> GetWithCourseTermAndCourse(Guid sectionId)
            => await _sectionRepository.GetWithCourseTermAndCourse(sectionId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? academicDepartmentId = null, string search = null, ClaimsPrincipal user = null)
            => await _sectionRepository.GetTeacherReportDatatable(sentParameters, termId, academicDepartmentId, search, user);

        public async Task<IEnumerable<Select2Structs.Result>> GetAvailableSectionsByCourseTermIdSelectClientSide(Guid courseTermId, Guid? selectedId = null)
            => await _sectionRepository.GetAvailableSectionsByCourseTermIdSelectClientSide(courseTermId, selectedId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetSectionsByCourseTermIdDatatable(DataTablesStructs.SentParameters sentParameters, Guid courseTermId)
            => await _sectionRepository.GetSectionsByCourseTermIdDatatable(sentParameters, courseTermId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsWithTimeCrossingDataTable(DataTablesStructs.SentParameters sentParameters, Guid? termId, Guid? careerId, Guid? facultyId, Guid? courseId, string search, ClaimsPrincipal user)
        {
            return await _sectionRepository.GetStudentsWithTimeCrossingDataTable(sentParameters, termId, careerId, facultyId, courseId, search, user);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetTimeCrossingCoursesByStudent(Guid studentId, Guid termId)
        {
            return await _sectionRepository.GetTimeCrossingCoursesByStudent(studentId, termId);
        }

        public async Task<IEnumerable<Section>> GetByCourseTermId(Guid courseTermId)
            => await _sectionRepository.GetByCourseTermId(courseTermId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetReportClassDetailDataTable(DataTablesStructs.SentParameters parameters, Guid sectionId)
            => await _sectionRepository.GetReportClassDetailDataTable(parameters, sectionId);

        public async Task<int> GetTotalClassesBySection(Guid sectionId)
            => await _sectionRepository.GetTotalClassesBySection(sectionId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetSectionsWithOutEvaluationReport(DataTablesStructs.SentParameters parameters, Guid? termId, Guid? careerId, Guid? curriculumId)
            => await _sectionRepository.GetSectionsWithOutEvaluationReport(parameters, termId, careerId, curriculumId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetSectionsEnabledForSubstituteExam(DataTablesStructs.SentParameters parameters, Guid termId, Guid? careerId, Guid? curriculmId, string searchValue)
            => await _sectionRepository.GetSectionsEnabledForSubstituteExam(parameters, termId, careerId, curriculmId, searchValue);

        public async Task<List<SubstituteExamSectionsTemplate>> GetSectionsByTermCareerCurriculumCycleCourseData(Guid termid, Guid? careerId, Guid? curriculumId, int cycleId, Guid? courseId, ClaimsPrincipal user = null, string search = null)
        {
            return await _sectionRepository.GetSectionsByTermCareerCurriculumCycleCourseData(termid, careerId, curriculumId, cycleId, courseId, user, search);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDirectedSectionsDatatable(DataTablesStructs.SentParameters parameters, string searchValue, ClaimsPrincipal user)
            => await _sectionRepository.GetDirectedSectionsDatatable(parameters, searchValue, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetEvaluationReportDatatableV2(DataTablesStructs.SentParameters parameters, Guid? termId, Guid? academicDepartmentId, string teacherId, string teacherCode, Guid? careerId, Guid? curriculumId, string courseSearch, ClaimsPrincipal user, int? academicYear)
            => await _sectionRepository.GetEvaluationReportDatatableV2(parameters, termId, academicDepartmentId, teacherId, teacherCode, careerId, curriculumId, courseSearch, user, academicYear);
    
        public async Task<DataTablesStructs.ReturnedData<object>> GetCapacityFullfilmentReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? careerId = null, int capacity = 0, ClaimsPrincipal user = null)
            => await _sectionRepository.GetCapacityFullfilmentReportDatatable(sentParameters, termId, careerId, capacity, user);
            public async Task<List<SectionCapacityTemplate>> GetCapacityFullfilmentReportData(Guid termId, Guid? careerId = null, int capacity = 0, ClaimsPrincipal user = null)
            => await _sectionRepository.GetCapacityFullfilmentReportData(termId, careerId, capacity, user);

        public async Task<SectionIncompleteScheduleReportTemplate> GetSectionIncompleteSchedulesTemplate(Guid termId, Guid? careerId, Guid? curriculumId, ClaimsPrincipal user)
            => await _sectionRepository.GetSectionIncompleteSchedulesTemplate(termId, careerId, curriculumId, user);

        public async Task<List<Section>> GetSectionByClassRange(string teacherId, DateTime start, DateTime end)
            => await _sectionRepository.GetSectionByClassRange(teacherId, start, end);

        public async Task<SectionProgressReportTemplate> GetSectionProgressReportTemplate(Guid sectionId, string teacherId = null)
            => await _sectionRepository.GetSectionProgressReportTemplate(sectionId, teacherId);
    }
}