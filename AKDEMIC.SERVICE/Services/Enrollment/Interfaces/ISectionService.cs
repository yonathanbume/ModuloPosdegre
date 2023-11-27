﻿using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Section;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.SubstituteExam;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Interfaces
{
    public interface ISectionService
    {
        Task<Section> GetWithIncludes(Guid sectionId);
        Task<Tuple<string, int, string[]>> GetSpecificSectionDate(Guid sectionId);
        Task<DataTablesStructs.ReturnedData<object>> GetAllByCourseAndTermAndPaginationParameters(DataTablesStructs.SentParameters sentParameters, Guid courseId, Guid termId);
        Task<DataTablesStructs.ReturnedData<Section>> GetSectionsByTermAndStudentIdDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? studentId = null, string searchValue = null);
        Task<IEnumerable<Section>> GetAllByCourseAndTerm(Guid courseId, Guid termId, string teacherId = null);
        Task<IEnumerable<Section>> GetByCourseTermId(Guid courseTermId);
        Task<IEnumerable<Guid>> GetListSectionIdToGeneratedEvaluationReportInBlock(Guid termId, string teacherId=null, Guid? careerId = null);
        Task<int> CountByCourseAndTerm(Guid courseId, Guid termId);
        Task<IEnumerable<Section>> GetAvailableSectionsByCourseTermId(Guid? termId, Guid? currentSectionId = null);
        Task<IEnumerable<Select2Structs.Result>> GetAvailableSectionsByCourseTermIdSelectClientSide(Guid courseTermId, Guid? selectedId = null);
        Task<Section> GetWithCourseTermAndCourse(Guid sectionId);
        Task<Section> GetWithTeacherSectionsAndClassSchedules(Guid id);
        Task<Section> Get(Guid id);
        Task<Section> First();
        Task<Section> GetWithTeacherSections(Guid id);
        Task<Section> GetWithEvaluations(Guid id);
        Task<Section> GetWithCourseTermCareer(Guid id);
        Task<Section> GetWithCourseTermAndClassSchedules(Guid id);
        Task<object> GetSectionsByCourseTermId(Guid courseTermId);
        Task<IEnumerable<Section>> GetAll(string teacherId = null, Guid? studentId = null, Guid? termId = null, Guid? courseId = null, Guid? courseTermId = null, bool showDirectedCourses = false);

        Task<DataTablesStructs.ReturnedData<object>> GetAllByCourseComponentTermFacultyAndPaginationParameters(DataTablesStructs.SentParameters sentParameters, Guid component, Guid termId, Guid faculty, string search);
        Task<List<ProcessedSectionsTemplate>> GetGradeRegistrationReport(Guid component, Guid termId, Guid faculty);
        Task<DataTablesStructs.ReturnedData<object>> GetAllByCourseTermLatFilterAndPaginationParameters(DataTablesStructs.SentParameters sentParameters, Guid term, int lateFilter, string search);

        Task<IEnumerable<SectionTemplateA>> GetAllByTermIdAsModelA(Guid termId ,Guid? careerId = null, string coordinatorId = null, Guid? academicDepartmentId = null, ClaimsPrincipal user = null, Guid? curriclumId = null);
        Task<List<TeacherSectionTemplate>> GetTecachersByTerm(Guid termId);
        Task<DataTablesStructs.ReturnedData<object>> GetEvaluationReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? faculty, Guid? termId, string teacher,string teacherCode = null, ClaimsPrincipal user = null, Guid? careerId = null, Guid? departmentId = null, string courseName = null, Guid? curriculumId = null, string evaluationCode = null, Guid? academicProgramId = null);
        Task<Section> GetSectionWithTermAndCareer(Guid sectionId);
        Task<IEnumerable<Select2Structs.Result>> GetSectionsByTermIdAndTeacherIdSelect2ClientSide(Guid termId, string teacherId);
        Task<IEnumerable<Select2Structs.Result>> GetSectionsByCourseTermIdSelect2ClientSide(Guid courseTermId);
        Task<IEnumerable<Select2Structs.Result>> GetSectionsByStudentIdAndTermIdSelect2ClientSide(Guid studentId, Guid termId);
        Task InsertAsync(Section section);
        Task UpdateAsync(Section section);
        Task DeleteAsync(Section section);
        Task<object> GetAllAsModelB(Guid? termId = null, Guid? courseId = null);
        Task<object> GetAsModelC(Guid? id = null);

        Task<object> GetStudentAvailableSectionsDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId, Guid courseId, Guid termId);
        Task<List<Section>> GetSectionByClassRange(string teacherId, DateTime start, DateTime end);
        Task<List<Section>> GetSectionWithStudentSection(Guid courseId, Guid? termId = null);

        Task<Section> GetStudentSectionWithClassCourse(Guid id);
        Task<IEnumerable<Select2Structs.Result>> GetSectionsByTermIdSelect2ClientSide(Guid termId, Guid courseId);
        Task<Section> GetSectionIncludeClassStudentAndCourse(Guid id);

        Task<object> GetCoruseSections(List<ClassSchedule> studentSchedules, Guid studentId, Guid courseId, Guid? termId = null);

        Task<object> GetSectionsByCareerSelect2ClientSide(Guid? careerId);
        Task<object> GetSectionByCourseId(Guid courseId);
        Task<List<SubstituteExamSectionsTemplate>> GetSectionsByTermCareerCurriculumCycleCourseData(Guid termid, Guid? careerId, Guid? curriculumId, int cycleId, Guid? courseId, ClaimsPrincipal user = null, string search = null);
        Task<DataTablesStructs.ReturnedData<object>> GetSectionsByTermCareerCurriculumCycleCourseDataTable(DataTablesStructs.SentParameters parameters, Guid termid, Guid? careerId, Guid? curriculumId, int cycleId, Guid? courseId, ClaimsPrincipal user = null,string search = null);
        Task<SectionSubstituteExamViewModel> GetSubstituteSectionById(Guid sectionId);
        Task<object> GetSectionByCourseTermIdSelect2(Guid coursetermId);
        Task<object> GetAllCourseSectionsDataTableClientSide( Guid courseId, Guid termId,string search);
        Task<int> CountByGroupId(Guid id);
        Task<bool> AvalableBySectionId(Guid id);
        Task AddAsync(Section section);
        Task AddRangeAsync(Section[] listSection);
        Task InsertRange(Section[] listSection);
        Task<Section> GetFirstBySectionCodeCourseAndTermId(string sectionCode, Guid courseId, Guid termid);
        Task<object> GetJsonByCourseAndTermId(Guid courseId, Guid termId, bool withDirectedCourses = false);

        Task<DataTablesStructs.ReturnedData<object>> GetSummerEnrollmentDataTable(DataTablesStructs.SentParameters sentParameters, Guid termid, Guid? careerId = null, Guid? curriculumId = null, Guid? programId = null, int? cycle = null, Guid? courseId = null, string search = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetSectionsToAssignSectionGroup(DataTablesStructs.SentParameters sentParameters, Guid? careerId, Guid? academicProgramId, string searchValue = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetTeacherReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? academicDepartmentId = null,string search=null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetSectionsByCourseTermIdDatatable(DataTablesStructs.SentParameters sentParameters, Guid courseTermId);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentsWithTimeCrossingDataTable(DataTablesStructs.SentParameters sentParameters, Guid? termId, Guid? careerId, Guid? facultyId, Guid? courseId, string search, ClaimsPrincipal user);
        Task<DataTablesStructs.ReturnedData<object>> GetTimeCrossingCoursesByStudent(Guid studentId, Guid termId);
        Task<DataTablesStructs.ReturnedData<object>> GetReportClassDetailDataTable(DataTablesStructs.SentParameters parameters, Guid sectionId);
        Task<int> GetTotalClassesBySection(Guid sectionId);

        Task<SectionIncompleteScheduleReportTemplate> GetSectionIncompleteSchedulesTemplate(Guid termId, Guid? careerId, Guid? curriculumId, ClaimsPrincipal use);
        Task<DataTablesStructs.ReturnedData<object>> GetSectionsWithOutEvaluationReport(DataTablesStructs.SentParameters parameters, Guid? termId, Guid? careerId, Guid? curriculumId);
        Task<DataTablesStructs.ReturnedData<object>> GetSectionsEnabledForSubstituteExam(DataTablesStructs.SentParameters parameters, Guid termId, Guid? careerId, Guid? curriculmId, string searchValue);
        Task<DataTablesStructs.ReturnedData<object>> GetDirectedSectionsDatatable(DataTablesStructs.SentParameters parameters, string searchValue, ClaimsPrincipal user);
        Task<DataTablesStructs.ReturnedData<object>> GetEvaluationReportDatatableV2(DataTablesStructs.SentParameters parameters, Guid? termId, Guid? academicDepartmentId, string teacherId, string teacherCode, Guid? careerId, Guid? curriculumId, string courseSearch, ClaimsPrincipal user, int? academicYear);
        Task<DataTablesStructs.ReturnedData<object>> GetCapacityFullfilmentReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? careerId = null, int capacity = 0, ClaimsPrincipal user = null);
        Task<List<SectionCapacityTemplate>> GetCapacityFullfilmentReportData(Guid termId, Guid? careerId = null, int capacity = 0, ClaimsPrincipal user = null);
        Task<SectionProgressReportTemplate> GetSectionProgressReportTemplate(Guid sectionId, string teacherId = null);
    }
}