﻿using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.JOBEXCHANGE.Areas.Admin.ViewModels.ReportViewModel;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Student;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.EnrolledStudent;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.SubstituteExam;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using static AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates.ProfileDetailTemplate;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface IStudentService
    {
        Task<object> GetAllPostulants(Guid termId, Guid careerId, int status);
        Task<StudentConstancy> GetStudentConstancy(Guid studentId, Guid termId);
        Task<StudentConstancy> GetStudentLastConstancy(Guid studentId);
        Task<IEnumerable<Student>> GetAllStudent();
        Task<IEnumerable<Student>> GetAllWithUserAndCareerAndCampusAndAdmissionTerm();
        Task<Student> Get(Guid id);
        Task<IEnumerable<Student>> GetAllByFacultyOrCareer(Guid facultyId, Guid careerId);
        Task<Student> GetWithIncludes(Guid id);
        Task<TemplateDataCV> GetStudentProfileDetail(Guid id);
        Task<StudentStudiesInfoTemplate> GetStudentStudiesDetail(Guid id);
        Task<IEnumerable<Student>> GetAllWithUser();
        Task<Student> GetStudentByUser(string userId);
        Task<StudentGraduatedSurveyInformation> GetStudentGraduatedSurveyInformation(string userId);
        Task<Student> GetStudentWithCareerAndUser(Guid id);
        Task<IEnumerable<Student>> GetAllBySectionId(Guid sectionId);
        Task<IEnumerable<Student>> GetDisapprovedByFacultyAndTerm(Guid termId, Guid? facultyId);
        Task<IEnumerable<Student>> GetRegularStudents();
        Task<IEnumerable<Student>> GetGraduateds();
        Task<object> GetAllSelectByStatus(int? status = null);
        Task Delete(Student student);
        Task Insert(Student student);
        Task AddAsync(Student student);
        Task Update(Student student);        
        Task<StudentDataTemplate> GetStudentDataByUserId(string userId); //para ficha socioeconomica
        Task<List<UserLoginStudentTemplate>> GetUserLoginStudentsTemplate(byte system, byte roleType, string startDate = null, string endDate = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, string academicCoordinatorId = null,string academicRecordStaffId = null, Guid? admissionTypeId = null, Guid? curriculumId = null, Guid? campusId = null);
        Task<DataTablesStructs.ReturnedData<Student>> GetLockedOutStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, string academicCoordinatorId = null,string academicRecordStaffId = null);
        Task DownloadExcelStudentInformationData(IXLWorksheet worksheet, Guid? careerId = null, Guid? departmentId = null, Guid? provinceId = null, Guid? districtId = null, int? sex = null, int? schoolType = null, int? universityPreparation = null, Guid? admissionTypeId = null, int? startAge = null, int? endAge = null);
        Task DownloadExcelStudentFamilyInformationData(IXLWorksheet worksheet, Guid? careerId = null, Guid? departmentId = null, Guid? provinceId = null, Guid? districtId = null, int? sex = null, int? schoolType = null, int? universityPreparation = null, Guid? admissionTypeId = null, int? startAge = null, int? endAge = null);
        Task<DataTablesStructs.ReturnedData<Student>> GetAgreementStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, string academicCoordinatorId = null, string academicRecordStaffId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetGraduatedPostulantDatatable(DataTablesStructs.SentParameters sentParameters, int state, Guid? termId = null, ClaimsPrincipal user = null);
        Task<object> GetGraduatedPostulantChart(int state, Guid? termId = null, ClaimsPrincipal user = null);
        Task<IEnumerable<Student>> GetStudentRankingByTerm(Guid termId, Guid? careerId = null, Guid? campusId = null);
        Task<IEnumerable<Student>> GetGraduatedsRankingByTerms(Guid? admissionTermId = null, Guid? graduationTermId = null, Guid? careerId = null);
        Task<IEnumerable<Student>> GetNewStudentsRankingByTerm(Guid? admissionTermId = null, Guid? careerId = null, int? status = null);
        Task<(IEnumerable<Student> pagedList, int count)> GetStudentRankingWithCreditsByTermAndPaginationParameter(PaginationParameter paginationParameter, Guid termId, Guid? careerId = null, Guid? campusId = null);
        Task<(IEnumerable<Student> pagedList, int count)> GetStudentRankingByTermAndPaginationParameter(PaginationParameter paginationParameter, Guid termId, Guid? careerId = null, Guid? campusId = null);
        Task<(IEnumerable<Student> pagedList, int count)> GetGraduatedsRankingByTermsAndPaginationParameter(PaginationParameter paginationParameter, Guid? admissionTermId = null, Guid? graduationTermId = null, Guid? careerId = null);
        Task<(IEnumerable<Student> pagedList, int count)> GetNewStudentsRankingByTermAndPaginationParameter(PaginationParameter paginationParameter, Guid? admissionTermId = null, Guid? careerId = null, int? status = null);
        Task<List<StudentFilterTemplate>> GetStudentFilterTemplatesByStatus(List<int> status);
        //Task<Select2Structs.ResponseParameters> GetStudentSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<Student, Select2Structs.Result>> selectPredicate = null, Func<Student, string[]> searchValuePredicate = null, string searchValue = null, int? status = null);
        Task<DataTablesStructs.ReturnedData<object>> GetUndefeatedStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid faculty, Guid career, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentsByFacultyAndCareerAndAcademicProgramDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, string searchValue = null, ClaimsPrincipal user = null);
        Task<object> SearchStudentByTerm(string term, Guid? careerId = null, ClaimsPrincipal user = null, bool onlyActiveStudents = false);
        Task<object> SearchStudentForDegreeByTerm(string term,int? degreeType = null, Guid? careerId = null, ClaimsPrincipal user = null, bool onlyActiveStudents = false);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentsByInstitutionalRecordGeneralDatatable(DataTablesStructs.SentParameters sentParameters, Guid id, Guid termId, byte? sisfohClasification = null, Guid? categorizationLevelId = null, Guid? careerId = null);
        Task<List<StudentsByInstitutionalRecordTemplate>> GetStudentsByInstitutionalRecordGeneralData(Guid id, Guid termId, byte sisfohClasification);
        Task<StudentInformationTemplate> GetStudentinformationById(Guid id);
        Task<StudentGeneralDataTemplate> GetStudentGeneralDataById(Guid id);
        Task<bool> ValidateUserRegister(string username, Guid id);
        Task<bool> ValidateUserEmail(string email, Guid id);
        Task<bool> ValidateUserPersonalEmail(string email, Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetUserProceduresDatatable(DataTablesStructs.SentParameters sentParameters, Guid id, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetEnrolledCoursesDatatable(DataTablesStructs.SentParameters sentParameters, Guid id, Guid termId);
        Task<List<Student>> GetByDni(string document);
        Task<List<Student>> GetStudentsByDniAndTerm(string document, Guid termId);
        Task<ApplicationUser> BankBatchUserByDocument(string document, int length);
        Task<object> GetAvailableSections(Guid id);
        Task<int> GetNumberOfApprovedCourses(Guid studentId);
        Task<Select2Structs.ResponseParameters> GetStudentSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<Student, Select2Structs.Result>> selectPredicate = null, Func<Student, string[]> searchValuePredicate = null, string searchValue = null, int? status = null, Guid? careerId = null, string coordinatorId = null);
        Task<DataTablesStructs.ReturnedData<Student>> GetUnbeatenStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, string academicCoordinatorId = null,bool isDean = false);
        Task<DataTablesStructs.ReturnedData<object>> GetCurriculumStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? curriculumId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetCafeteriaStudentsDatatable(DataTablesStructs.SentParameters sentParameters, bool isCheckedAll, string searchValue = null, Guid? careerId = null, Guid? facultyId = null);
        //Task<DataTablesStructs.ReturnedData<StudentFilterTemplate>> GetStudentFilterDatatable(DataTablesStructs.SentParameters parameters, Guid? cid, Guid? fid, Guid? pid, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetBachillerInTimeDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetGraduatedStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, Guid? graduationTermId = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetGraduatedStudentInTimeDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, Guid? graduationTermId = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetDeserterStudentReportDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentDegreeReportDatatable(DataTablesStructs.SentParameters sentParameters, List<Guid> careers, int? graduatedYear = null, Guid? graduatedTermId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentDegreeFirstJobReportDatatable(DataTablesStructs.SentParameters sentParameters, List<Guid> careers, int? graduatedYear = null, Guid? graduatedTermId = null);
        Task<object> GetStudentDegreeReportChart(List<Guid> careers, int? graduatedYear = null, Guid? graduatedTermId = null);
        Task<object> GetStudentDegreeFirstJobReportChart(List<Guid> careers, int? graduatedYear = null, Guid? graduatedTermId = null);
        Task<object> GetBachillerInTimeChart(ClaimsPrincipal user = null);
        Task<object> GetGraduatedStudentChart(Guid? careerId = null, Guid? graduationTermId = null, ClaimsPrincipal user = null);
        Task<object> GetGraduatedStudentInTimeChart(Guid? careerId = null, Guid? graduationTermId = null, ClaimsPrincipal user = null);
        Task<object> GetDeserterStudentReportChart(ClaimsPrincipal user = null);
        Task<object> StudentsReport1(bool isCoordinator, List<Guid> careers);
        Task<object> StudentsReport2(int startYear, int endYear, List<Guid> careers, Guid? facultyId);
        Task<object> StudentsReport3();
        Task<object> StudentsReport4(bool isCoordinator, List<Guid> careerId);
        Task<object> StudentsReport5(bool isCoordinator, List<Guid> careerId);
        Task<object> StudentsReport6(bool isCoordinator, List<Guid> careerId);
        Task<object> StudentsReport7(bool isCoordinator, List<Guid> careerId);
        Task<object> StudentsReport8(List<Guid> careers, Guid? facultyId);
        Task<object> StudentsReport9();
        Task<DataTablesStructs.ReturnedData<object>> SearchList(DataTablesStructs.SentParameters sentParameters, bool isCoordinator, List<Guid> careerId, string DNI, string name, string code);
        Task<int> Count();
        Task<int> CountAllGraduated();
        Task<DataTablesStructs.ReturnedData<object>> GraduatedListReport(DataTablesStructs.SentParameters sentParameters, bool isCoordinator, List<Guid> careers, int gradeType, Guid careerParameterId, int year = 0 , int admissionYear = 0);
        Task<CuantityReportTemplate.Table> ReportNumberGraduates(bool isCoordinator, List<Guid> careers, string startDate, string endDate, Guid careerId);
        Task<IEnumerable<Student>> GetFacultyStudentsEnrolled(Guid termId);
        Task<object> GlobalList(bool isCoordinator, Guid? careerId, string dni, string nombre, string codigo, int draw, int start, int length);
        Task<DataTablesStructs.ReturnedData<object>> GetGlobalListDatatable(DataTablesStructs.SentParameters sentParameters,string dni = null, string name = null, string userName = null , Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeReportGraduatedDatatable(DataTablesStructs.SentParameters sentParameters, string userName = null, string dni = null, string fullName = null, int? studentState = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, Guid? admissionTermId = null , Guid? graduationTermId = null, int? graduationYear = null);
        Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeReportStudentDatatable(DataTablesStructs.SentParameters sentParameters, string userName = null, string dni = null, string fullName = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, Guid? admissionTermId = null);
        Task<GraduatedFiltersTemplate> ReportGraduates(bool isCoordinator, List<Guid> careers);
        Task<CuantityReportTemplate> ReportBachelorTitle(bool isCoordinator, List<Guid> careers);
        Task<CuantityReportTemplate> ReportBachelorTitleSunedu(bool isCoordinator, List<Guid> careers);

        Task<StudentComprobantInscriptionTemplate> GetStudentComprobantInscriptionTemplate(Guid studentId);
        Task<List<StudentComprobantInscriptionTemplate>> GetStudentsComprobantInscriptionsTemplateByFilters(Guid termId, Guid? careerId, Guid? facultyId);

        Task<DataTablesStructs.ReturnedData<StudentFilterTemplate>> GetStudentFilterDatatable(DataTablesStructs.SentParameters parameters, Guid? cid, Guid? fid, Guid? pid, string search, ClaimsPrincipal user = null);

        Task<DataTablesStructs.ReturnedData<StudentRankingByTermTemplate>> GetStudentRankingByTermDataTable(DataTablesStructs.SentParameters parameters, string userId, Guid termId, Guid? careerId, Guid? academicProgramId, Guid? campusId, string search,ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<GraduatedsTemplate>> GetGraduatedsDataTable(DataTablesStructs.SentParameters parameters, ClaimsPrincipal user, Guid? admissionTermId = null, Guid? graduationTermId = null, Guid? careerId = null, Guid? academicProgramId = null, string search = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAverageTimeToFinishByCareerAndTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid graduationTermId, Guid? careerId = null);      
        Task<DataTablesStructs.ReturnedData<StudentRankingForCreditsTemplate>> GetStudentRankingForCreditsDataTable(DataTablesStructs.SentParameters parameters, ClaimsPrincipal userId, Guid termId, Guid? careerId, Guid? academicProgramId, Guid? campusId, string search);
        Task<DataTablesStructs.ReturnedData<NewStudentTemplate>> GetNewStudentsDataTable(DataTablesStructs.SentParameters parameters, ClaimsPrincipal user, Guid? admissionTermId = null, Guid? careerId = null, Guid? academicProgramId = null, int? status = null, string search = null);
        void RemoveRange(List<Student> studentList);
        Task<object> GetBachelors(List<Guid> Faculties, List<Guid> Careers,List<Guid> AcademicPrograms, int GradeType);
        Task<object> GetBachelorsWithOutConfiguration(List<Guid> Faculties , List<Guid> Careers = null, List<Guid> AcademicPrograms = null);
        Task<DataTablesStructs.ReturnedData<object>> GetReportPersonalInformation(DataTablesStructs.SentParameters parameters, Guid? careerId = null, Guid? departmentId = null, Guid? provinceId = null, Guid? districtId = null, int? sex = null, int? schoolType = null, int? universityPreparation = null, Guid? admissionTypeId = null, int? startAge = null, int? endAge = null);
        Task<DataTablesStructs.ReturnedData<StudentInformationDataTableTemplate>> GetStudentInformationDataTable(DataTablesStructs.SentParameters parameters, Guid cid);
        Task<object> GetTopFacultiesEnrollled(Guid id);
        Task<StudyRecordTemplate> GetStudyRecord(Guid studentId);
        Task<Student> GetStudentProofOfInCome(Guid studentId);
        Task<Student> GetStudentRecordEnrollment(Guid studentId);
        Task<Student> GetStudentWithCareerAcademicUser(Guid studentId);
        Task<Student> GetStudentWithCareerAdmissionAcademicUser(Guid studentId);
        Task<HeadBoardCertificateTemplate> GetStudntCertificate(Guid studentId, string university);
        Task<IEnumerable<Student>> GetStudentsBySeciontId(Guid sectionId);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentByStatusDatatable(DataTablesStructs.SentParameters sentParameters, List<int> status, string searchValue = null, Guid? facultyId = null, Guid? careerId = null, List<Guid> careers = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentDatatableAcademicSituation(DataTablesStructs.SentParameters sentParameters, Guid tid, Guid fid, Guid cid,string searchValue = null, int? academicOrder = null, ClaimsPrincipal user = null, Guid? curriculumId = null, int? year = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentDatatableAcademicSituationCumulative(DataTablesStructs.SentParameters sentParameters, Guid tid, Guid fid, Guid cid, string searchValue = null, int? academicOrder = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentDatatableAcademicSituationWithOutComent(DataTablesStructs.SentParameters sentParameters, Guid fid, Guid cid, int? academicOrder = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentDatatableAcademicSituationDean(DataTablesStructs.SentParameters sentParameters, Guid fid, Guid cid, int? academicOrder = null);
        Task<Student> GetStudentHome(string userId);

        Task<DataTablesStructs.ReturnedData<object>> GetStudentsWithCreditsDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, int? type = null, string searchValue = null, bool? onlyWithCredits = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentsAcademicHistoryDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, Guid? faculty = null, Guid? career = null, string search = null);
        Task UpdateStudentsCurrentAcademicYearJob(string connectionString);
        Task<List<AlumniExcelTemplate>> GetStudentToExcelAlumni(Guid? faculty = null, Guid? career = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetSanctionedStudentDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, string userId, Guid? faculty = null, Guid? career = null, string search = null, Guid? term = null);
        Task<List<SacntionedExcelTemplate>> GetStudentToExcelSantioned(Guid? term = null, Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentsUnbeatenDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, string userId, Guid? faculty = null, Guid? career = null, string search = null);
        Task<List<UnbeatenExcelTemplate>> GetStudentToExcelUnbeaten(Guid? faculty = null, Guid? career = null, ClaimsPrincipal user = null);

        Task<DataTablesStructs.ReturnedData<object>> GetDesertionStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetNonEnrolledStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null, int? year = null, bool excludeLastYear = false);
        Task<List<NonEnrolledStudentTemplate>> GetNonEnrolledStudentData(Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null, int? year = null, bool excludeLastYear = false);
        Task<DataTablesStructs.ReturnedData<object>> GetExpelledStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null);
        Task<List<SacntionedExcelTemplate>> GetStudentToExcelDesertion(Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null);
        Task<List<SacntionedExcelTemplate>> GetStudentToExcelExpelled(Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null);
        Task<List<Student>> GetAllStudentsWithFacultyAndTerm(Guid facultyId, Guid careerId, Guid? termId = null);
        IQueryable<Student> GetStudentOrdered(int status, Guid careerId);
        Task<DataTablesStructs.ReturnedData<object>> GetQualifiedStudentsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, string searchValue = null, ClaimsPrincipal user = null);
        Task<object> GetQualifiedStudentsChart(Guid? careerId = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetBachelorStudentsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, string searchValue = null, ClaimsPrincipal user = null);
        Task<object> GetBachelorStudentsChart(Guid? careerId = null, ClaimsPrincipal user = null);
        Task UpdateStudentOrderJob();
        Task<DataTablesStructs.ReturnedData<object>> GetStudentDatatableEnrolledStudent(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, string userId, Guid termId, Guid facultyId, Guid careerId, int year, int? type = null, Guid? academicProgramId = null, int? cycle = null, Guid? campusId = null, string search = null);
        Task<DataTablesStructs.ReturnedData<object>> GetCurrentEnrolledStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId);
        Task<DataTablesStructs.ReturnedData<object>> GetPostPaymentEnrolledStudentDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, Guid termId, Guid? facultyId = null, Guid? careerId = null, int? year = null, int status = 1, string search = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentDatatableEnrollentTutoring(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, Guid termId, Guid? careerId = null, int? cycle = null, string search = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentDatatableEnrolledExtraCreditStudent(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, string userId, Guid termId, Guid facultyId, Guid careerId, int year, int? type = null, Guid? academicProgramId = null, int? cycle = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentDatatableEnrolledBySectionsStudent(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, string userId, Guid termId, Guid facultyId, Guid careerId, Guid academicProgramId, Guid curriculumId, Guid courseId, Guid sectionId);
        IQueryable<Student> GetStudentIncludeInformationGeo(string userId);

        Task<Student> GetStudentIncludeInformationUser(string studentId);

        IQueryable<Student> GetStudentIncludeInformationGeoCareer(string userId);
        Task<Student> GetStudentIncludeInformation(string studentId);

        Task<Student> GetStudentIncludeInformationGeoCareer2(Guid StudentId);

        Task<object> GetAllStudentWithData(Guid termId);

        Task<object> GetEntrantStudentsEnrolledDatatable(Guid careerId, Guid termId, string search);

        Task<IEnumerable<Student>> GetAllStudentWithDataIn(Guid id);

        Task<DataTablesStructs.ReturnedData<object>> GetStudentByStatusListDatatable(DataTablesStructs.SentParameters sentParameters, IEnumerable<int> status, Guid? termId = null, ClaimsPrincipal user = null);
        Task<object> GetGraduatedAndBachelorStudentPieChart(Guid? termId = null, ClaimsPrincipal user = null);
        Task<object> GetBachelorAndQualifiedStudentPieChart(Guid? termId = null, ClaimsPrincipal user = null);
        Task<object> GetStudentJson(Guid? id = null);
        Task<EnrollmentInformationTemplate> GetStudentEnrollmentInformation(Guid studentId);
        Task<Student> GetStudentWithDataByUserId(string userId);

        Task<Student> GetStudentByUserName(string username);
        Task<Student> GetStudentIdString(string studentId);
        Task<int> GetStudentsEnrolledCountByTerm(Guid termId, ClaimsPrincipal user = null);

        Task<DataTablesStructs.ReturnedData<object>> GetNextEnrollmentStudentsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, Guid? careerId = null, int? academicYear = null, ClaimsPrincipal user = null);
        Task<object> GetStudentAcademicYearsSelectClientSide(Guid? faculty = null, Guid? career = null);

        Task<Student> GetStudentByStudentInformationId(Guid studentInformId);
        Task<DataTablesStructs.ReturnedData<object>> GetTransferStudentDataTable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, string userId, Guid? faculty = null, Guid? career = null);
        Task<object> GetEnrolledByFacultyAndTermChart(Guid? termId = null, Guid? facultyId = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetEnrolledByFacultyAndTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? facultyId = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetOthersEnrolledCoursesDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId, Guid termId);

        Task<string> GetFacultyByStudentIncludeCareerUser(string userId);
        Task<string> GetStudentFullNameIncludeUserByUserId(string userId);
        Task<DataTablesStructs.ReturnedData<object>> GetEnrollmentDatatableCon(DataTablesStructs.SentParameters sentParameters);
        Task<Student> GetByIdWithData(Guid id);
        Task<List<Student>> GetStudentWitData();
        Task<bool> GetStudentToUpdateByCode(string code);
        Task<Student> GetWithData(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GraduatedListReportToAcademicRecord(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, int gradeType, Guid careerId, int year);
        Task<IEnumerable<AcademicSituationExcelTemplate>> GetStudentAcademicSituationExcelTemplate(Guid? termId, Guid? facultyId, Guid? careerId, int? academicOrder, ClaimsPrincipal user = null);
        Task<IEnumerable<EnrolledStudentExcelTemplate>> GetEnrolledStudentsExcelReport(ClaimsPrincipal User, string userId, Guid termId, Guid facultyId, Guid careerId, int? year = null ,int? type = null, Guid? academicProgramId = null, Guid? campusId = null);
        Task<IEnumerable<EnrolledStudentExcelTemplate>> GetEnrolledTutoringStudentsExcelReport(ClaimsPrincipal User, Guid termId, Guid? careerId = null, int? year = null);
        Task<DataTablesStructs.ReturnedData<SubstituteExamTemplate>> GetStudentsForSubstiteExam(DataTablesStructs.SentParameters sentParameters, Guid termid, Guid sectionId, string search);
        Task<List<SubstituteExamTemplate>> GetStudentsForSubstituteExamDataAsync(Guid termid, Guid sectionId, string search = null);
        Task DownloadGlobalListExcel(IXLWorksheet worksheet, string dni = null, string name = null, string userName = null, Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null);
        Task<List<JobExchangeGraduatedDataTemplate>> GetJobExchangeReportGraduatedData(string userName = null, string dni = null, string fullName = null, int? studentState = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, Guid? admissionTermId = null, Guid? graduationTermId = null, int? graduationYear = null);
        Task<List<JobExchangeStudentDataTemplate>> GetJobExchangeReportStudentData(string userName = null, string dni = null, string fullName = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, Guid? admissionTermId = null);

        
        Task<List<StudentJobExchangeReportTemplate>> ReportGlobalListData(string dni = null, string userName = null, Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentsByAdmissionTermReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, ClaimsPrincipal user = null, bool? showEnrolled = null, Guid? admissionTypeId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentsByAdmissionTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? careerId = null,Guid? facultyId = null, string search = null);

        Task<DataTablesStructs.ReturnedData<object>> GetDebtorStudentsReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, ClaimsPrincipal user = null);
        Task<IEnumerable<Student>> GetStudentsByAdmissionTermPdfData(Guid termId, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, ClaimsPrincipal user = null, Guid? admissionTypeId = null);
        Task<IEnumerable<EnrolledStudentExcelTemplate>> GetStudentsByEquivalenceData(Guid termId, Guid? facultyId = null, Guid? careerId = null);
        Task<List<DebtStudentTemplate>> GetDebtorStudentsDataPdf(Guid termId, Guid? facultyId, Guid? careerId, Guid? academicProgramId, ClaimsPrincipal user);
        Task<IEnumerable<Student>> GetEnrolledStudentBytermId(Guid termId, Guid? facultyId = null, Guid? careerId=null, int? studentAcademicYear = null, int? status = null, Guid? campusId = null);
        Task<IEnumerable<Student>> GetEnrolledTutoringStudentBytermId(ClaimsPrincipal User, Guid termId, Guid? careerId = null, int? studentAcademicYear = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentsWithCreditsIncomingDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, int? type = null, string searchValue = null, bool? onlyWithCredits = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentsByRecognitionDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, string search = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentsWithExtemporaneousEnrollmentDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, string searchValue = null, ClaimsPrincipal user = null);
        Task<object> SearchIncomingStudentByTerm(string term, Guid? careerId = null, ClaimsPrincipal user = null);
        IQueryable<Student> GetActiveStudents();
       IQueryable<Student> GetStudentWithStudentSectionsByTermId(Guid termId);
        Task FixCurriculumStudentsJob();
        Task<int> UpdateStudentAcademicYearJob();
        Task UpdateStudentStatusJob();
        Task UnbeaterStudentsJob();
        Task CreateStudentsJob(UserManager<ApplicationUser> userManager,int count, Term term, bool isIntegrated, Guid procedureId_conceptId);

        Task<object> SearchEnabledStudentForCourseByTerm(string term, Guid? courseId = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetEnrolledStudentLastTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, Guid? careerId = null, string searchValue = null);
        Task DownloadExcelGraduates(IXLWorksheet worksheet, bool isCoordinator, List<Guid> careers, int gradeType, Guid careerParameterId, int year = 0, int admissionYear = 0);
        Task<decimal> GetApprovedCreditsByStudentId(Guid studentId);
        Task<decimal> GetEnrolledCreditsByStudentId(Guid studentId, byte? status = null);
        Task<List<OriginReportTemplate>> GetOriginStudentReport(Guid termId, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetOriginStudent(List<Guid> careerId, DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? deparment = null, Guid? province = null, Guid? district = null);
        Task<List<ReportOriginTemplate>> GetOriginStudentDatatable(List<Guid> careerId, Guid? termId = null, Guid? deparment = null, Guid? province = null, Guid? district = null, ClaimsPrincipal user = null);
        Task<List<StudentOriginTemplate>> GetOriginStudentReportUniqe(List<Guid> careerId, Guid? termId = null, Guid? deparment = null, Guid? province = null, Guid? district = null, ClaimsPrincipal user = null);

        Task<StudentSexReportTemplate> GetStudentSexReport(Guid termId, ClaimsPrincipal user = null);
        Task<Select2Structs.ResponseParameters> GetStudentsByCareer(Select2Structs.RequestParameters requestParameters, int currentAcademicYear, Guid id, Expression<Func<Student, Select2Structs.Result>> selectPredicate = null, Func<Student, string[]> searchValuePredicate = null, string searchValue = null, int? status = null);

        Task<DataTablesStructs.ReturnedData<object>> GetStudentSecondCareerDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, string userId, Guid termId, Guid facultyId, Guid careerId, Guid? academicProgramId = null, int? academicYear = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentsEnrolledMultipleCareersDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, string userId, Guid termId, Guid facultyId, Guid careerId, Guid? academicProgramId = null, int? academicYear = null, string search = null);
        Task<Select2Structs.ResponseParameters> GetStudentsByCareerSelect2(Select2Structs.RequestParameters requestParameters, int? currentAcademicYear = null, Guid? id = null, string searchValue = null, int? status = null);
        Task<Select2Structs.ResponseParameters> GetStudentsWithPendingCourse(Select2Structs.RequestParameters requestParameters, Guid courseId, string searchValue);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentsToGraduateDatatable(DataTablesStructs.SentParameters parameters, Guid? termId, Guid? facultyId, Guid? careerId, Guid? curriculumId, string searchValue, ClaimsPrincipal user);
        Task<IEnumerable<StudentSiriesTemplate>> GetStudentsExcelSiriesReport(ClaimsPrincipal user, Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, Guid? campusId = null, int? year = null);
        Task<IEnumerable<StudentSuneduTemplate>> GetStudentsSuneduReport(ClaimsPrincipal user, Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, Guid? campusId = null, int? year = null);
        Task<decimal> GetRequiredApprovedCredits(Guid studentId);
        Task<decimal> GetElectiveApprovedCredits(Guid studentId);
        Task<int> CalculateEnrollmentAcademicYear(Guid studentId, Guid termId);
        Task<List<StudentMultipleCareersTemplate>> GetStudentsEnrolledMultipleCareersData(ClaimsPrincipal User, string userId, Guid termId, Guid facultyId, Guid careerId, Guid? academicProgramId = null, int? academicYear = null);
        Task<List<DateTime>> GetStudentsBirthData(Guid termId, Guid? careerId, ClaimsPrincipal user = null);
        Task<object> GetTotalNumberOfStudentsEnrolledByCareerAndTermIdChart(Guid? termId = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetReEnrolledStudentsDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId, Guid? careerId, ClaimsPrincipal user, string search = null);
        Task<DataTablesStructs.ReturnedData<object>> GetUserLoginStudentsDatatable(DataTablesStructs.SentParameters sentParameters, byte system, byte roleType, string startDate, string endDate, string search = null);
        Task<DataTablesStructs.ReturnedData<object>> GetHomeLoginsDatatable(DataTablesStructs.SentParameters sentParameters, byte? system = null);
        Task<DataTablesStructs.ReturnedData<object>> GetInstitutionalWelfareStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, Guid? admissionTypeId = null, string search = null);
        Task<List<StudentJobExchangeExcel>> ExcelStudentsReport1(bool isCoordinator, List<Guid> careers);
        Task<List<StudentJobExchangeExcel>> ExcelStudentsReport8(List<Guid> careers, Guid? facultyId);
        Task<List<StudentJobExchangeExcel>> ExcelStudentsReport9(Tuple<bool,List<Guid>> verification);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentsAcademicSummaryDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, Guid termId, Guid? careerId, bool? unbeaten, string search = null);
        Task<List<StudentAcademicSummaryTemplate>> GetStudentsAcademicSummaryData(ClaimsPrincipal user, Guid termId, Guid? careerId, bool? unbeaten, string search = null);
        Task<bool> IsSanctionedStudentValidToStudy(Guid studentId);
        Task<DataTablesStructs.ReturnedData<object>> GetFutureGraduatedStudentDatatable(DataTablesStructs.SentParameters parameters, Guid? careerId, Guid? curriculumId, string searchValue, ClaimsPrincipal user);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentsWithAverageFinalGrades(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid facultyId, int? academicYear = null, Guid? careerId = null, string search = null, ClaimsPrincipal user = null);
        Task<Term> GetLastTermEnrolled(Guid studentId);
        Task<List<WithdrawnStudentTemplate>> GetWithdrawnStudentsTemplate(Guid? termId);
        Task<object> GetWithdrawnStudentsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentsManagementDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, string searchValue = null);
        Task<StudentProcedureResult> StudentCareerTransferRequest(ClaimsPrincipal user, Guid studentId, Guid newCareerId, Guid newCurriculumId);
        Task<StudentProcedureResult> StudentAcademicYearWithdrawalRequest(ClaimsPrincipal user, Guid studentId);
        Task<StudentProcedureResult> StudentCourseWithdrawalRequest(ClaimsPrincipal user, Guid studentSectionId);
        Task<StudentProcedureResult> ResignStudentRequest(ClaimsPrincipal user, Guid studentId, string reason, string fileUrl);
        Task<StudentProcedureResult> ReentryStudentRequest(ClaimsPrincipal user, Guid studentId, string fileUrl);
        Task<StudentProcedureResult> StudentReservationRequest(ClaimsPrincipal user, Guid studentId, string receipt, string fileUrl, string observation);
        Task<StudentProcedureResult> StudentChangeAcademicProgramRequest(ClaimsPrincipal user, Guid studentId, Guid newAcademicProgramId);
        Task<List<FutureGradutedStudentTemplate>> GetFutureGraduatedStudentsTemplate(Guid? careerId, Guid? curriculumId, ClaimsPrincipal user);
        Task<StudentProcedureResult> StudentExoneratedCourseRequest(ClaimsPrincipal user, Guid studentId, Guid courseId);
        Task<StudentProcedureResult> StudentExtraordinaryEvaluationRequest(ClaimsPrincipal user, Guid studentId, Guid courseId);
        Task<StudentProcedureResult> ExecuteProcedureActivity(ClaimsPrincipal user, UserProcedure userProcedure, StudentUserProcedure studentUserProcedure);
        Task<StudentProcedureResult> StudentGradeRecoveryRequest(ClaimsPrincipal user, Guid studentSectionId);
        Task<StudentProcedureResult> StudentSubstituteExamRequest(ClaimsPrincipal user, Guid studentSectionId, string paymentReceipt);
        Task<StudentProcedureResult> StudentCourseWithdrawalMassiveRequest(ClaimsPrincipal user, List<Guid> studentSectionIds);
        Task<object> GetAverageGradesByStudentReport(Guid studentId);

        Task<List<EnrolledCourseTemplate>> GetEnrolledCoursesAvailableToSubstitueExam(Guid studentId);
        Task<List<EnrolledCourseTemplate>> GetEnrolledCoursesToGradeRecovery(Guid studentId);
        Task<List<EnrolledCourseTemplate>> GetCoursesAvailableForExoneratedCourse(Guid studentId);
        Task<List<EnrolledCourseTemplate>> GetCoursesAvailableForExtraordinaryEvaluation(Guid studentId);
        Task<DataTablesStructs.ReturnedData<object>> GetEnrolledStudentPaymentStatusDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, Guid termId, Guid? facultyId = null, Guid? careerId = null, byte? type = null, int status = 1, string search = null);
        Task<List<StudentPaymentStatus>> GetEnrolledStudentPaymentStatusData(ClaimsPrincipal User, Guid termId, Guid? facultyId = null, Guid? careerId = null, byte? type = null, int status = 1, string search = null);
        Task<List<StudentPaymentStatus>> GetPostPaymentEnrolledStudentData(ClaimsPrincipal User, Guid termId, Guid? facultyId = null, Guid? careerId = null, int? year = null, int status = 1, string search = null);

        Task<object> GetSuitableStudentsReportDatatable(Guid? facultyId, Guid? careerId, Guid? programId, int? year, ClaimsPrincipal user = null);
        Task<object> GetSanctionedStudentsReportDatatable(Guid? facultyId, Guid? careerId, Guid? programId, int? year, ClaimsPrincipal user = null);

        Task<object> GetJobExchangeStudentGraduatedQuantityChartData();
        Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentGraduatedQuantityDatatable(DataTablesStructs.SentParameters sentParameters);

        Task<object> GetJobExchangeStudentGraduatedWorkingCareerQuantityChartData(List<int> studentStatus = null, List < Guid> careers = null);
        Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentGraduatedWorkingCareerQuantityDatatable(DataTablesStructs.SentParameters sentParameters, List<int> studentStatus = null, List<Guid> careers = null);


        Task<object> GetJobExchangeStudentGraduatedWorkingCareerPercentageChartData();
        Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentGraduatedWorkingCareerPercentageDatatable(DataTablesStructs.SentParameters sentParameters);

        Task<object> GetJobExchangeStudentGraduatedGraduationYearQuantityChartData(int? startYear = null, int? endYear = null);
        Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentGraduatedGraduationYearQuantityDatatable(DataTablesStructs.SentParameters sentParameters, int? startYear = null, int? endYear = null);
    }
}
