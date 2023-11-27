using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.JOBEXCHANGE.Areas.Admin.ViewModels.ReportViewModel;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Student;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.EnrolledStudent;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.SubstituteExam;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Math;
using Microsoft.AspNetCore.Identity;
using static AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates.ProfileDetailTemplate;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<Student> Get(Guid id)
        {
            return await _studentRepository.Get(id);
        }

        public async Task<Student> GetStudentByUser(string userId)
        {
            return await _studentRepository.GetStudentByUser(userId);
        }

        public async Task<IEnumerable<Student>> GetAllBySectionId(Guid sectionId) =>
            await _studentRepository.GetAllBySectionId(sectionId);

        public async Task<IEnumerable<Student>> GetDisapprovedByFacultyAndTerm(Guid termId, Guid? facultyId)
            => await _studentRepository.GetDisapprovedByFacultyAndTerm(termId, facultyId);

        public async Task Delete(Student student) =>
            await _studentRepository.Delete(student);

        public async Task Insert(Student student) =>
            await _studentRepository.Insert(student);

        public async Task AddAsync(Student student)
            => await _studentRepository.Add(student);

        public async Task Update(Student student) =>
            await _studentRepository.Update(student);

        public async Task<Student> GetStudentWithCareerAndUser(Guid id)
            => await _studentRepository.GetStudentWithCareerAndUser(id);

        public async Task<IEnumerable<Student>> GetStudentRankingByTerm(Guid termId, Guid? careerId = null, Guid? campusId = null)
            => await _studentRepository.GetStudentRankingByTerm(termId, careerId, campusId);

        public async Task<IEnumerable<Student>> GetGraduatedsRankingByTerms(Guid? admissionTermId = null, Guid? graduationTermId = null, Guid? careerId = null)
            => await _studentRepository.GetGraduatedsRankingByTerms(admissionTermId, graduationTermId, careerId);

        public async Task<IEnumerable<Student>> GetNewStudentsRankingByTerm(Guid? admissionTermId = null, Guid? careerId = null, int? status = null)
            => await _studentRepository.GetNewStudentsRankingByTerm(admissionTermId, careerId, status);

        public async Task<(IEnumerable<Student> pagedList, int count)> GetGraduatedsRankingByTermsAndPaginationParameter(PaginationParameter paginationParameter, Guid? admissionTermId = null, Guid? graduationTermId = null, Guid? careerId = null)
            => await _studentRepository.GetGraduatedsRankingByTermsAndPaginationParameter(paginationParameter, admissionTermId, graduationTermId, careerId);

        public async Task<(IEnumerable<Student> pagedList, int count)> GetNewStudentsRankingByTermAndPaginationParameter(PaginationParameter paginationParameter, Guid? admissionTermId = null, Guid? careerId = null, int? status = null)
            => await _studentRepository.GetNewStudentsRankingByTermAndPaginationParameter(paginationParameter, admissionTermId, careerId, status);

        public async Task<(IEnumerable<Student> pagedList, int count)> GetStudentRankingWithCreditsByTermAndPaginationParameter(PaginationParameter paginationParameter, Guid termId, Guid? careerId = null, Guid? campusId = null)
            => await _studentRepository.GetStudentRankingWithCreditsByTermAndPaginationParameter(paginationParameter, termId, careerId, campusId);

        public async Task<(IEnumerable<Student> pagedList, int count)> GetStudentRankingByTermAndPaginationParameter(PaginationParameter paginationParameter, Guid termId, Guid? careerId = null, Guid? campusId = null)
            => await _studentRepository.GetStudentRankingByTermAndPaginationParameter(paginationParameter, termId, careerId, campusId);

        public async Task<object> GetAllPostulants(Guid termId, Guid careerId, int status)
            => await _studentRepository.GetAllPostulants(termId, careerId, status);
        public async Task<IEnumerable<Student>> GetAllStudent()
            => await _studentRepository.GetAll();
        public async Task<IEnumerable<Student>> GetAllWithUserAndCareerAndCampusAndAdmissionTerm()
            => await _studentRepository.GetAllWithUserAndCareerAndCampusAndAdmissionTerm();

        public async Task<IEnumerable<Student>> GetAllWithUser()
            => await _studentRepository.GetAllWithUser();

        public async Task<IEnumerable<Student>> GetRegularStudents()
            => await _studentRepository.GetRegularStudents();

        public async Task<Select2Structs.ResponseParameters> GetStudentSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<Student, Select2Structs.Result>> selectPredicate = null, Func<Student, string[]> searchValuePredicate = null, string searchValue = null, int? status = null, Guid? careerId = null, string coordinatorId = null)
            => await _studentRepository.GetStudentSelect2(requestParameters, selectPredicate, searchValuePredicate, searchValue, status, careerId, coordinatorId);

        public async Task<DataTablesStructs.ReturnedData<Student>> GetUnbeatenStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, string academicCoordinatorId = null, bool isDean = false)
            => await _studentRepository.GetUnbeatenStudentsDatatable(sentParameters, searchValue, academicCoordinatorId, isDean);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, string academicCoordinatorId = null, string academicRecordStaffId = null, Guid? admissionTypeId = null, Guid? curriculumId = null, Guid? campusId = null)
            => await _studentRepository.GetStudentsDatatable(sentParameters, searchValue, facultyId, careerId, academicProgramId, academicCoordinatorId, academicRecordStaffId, admissionTypeId, curriculumId,campusId);

        public async Task<DataTablesStructs.ReturnedData<Student>> GetLockedOutStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, string academicCoordinatorId = null, string academicRecordStaffId = null)
         => await _studentRepository.GetLockedOutStudentsDatatable(sentParameters, searchValue, facultyId, careerId, academicProgramId, academicCoordinatorId, academicRecordStaffId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? facultyId = null, Guid? careerId = null)
            => await _studentRepository.GetStudentsDatatable(sentParameters, searchValue, facultyId, careerId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetUndefeatedStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid faculty, Guid career, string search)
            => await _studentRepository.GetUndefeatedStudentDatatable(sentParameters, faculty, career, search);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsByFacultyAndCareerAndAcademicProgramDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, string searchValue = null, ClaimsPrincipal user = null)
            => await _studentRepository.GetStudentsByFacultyAndCareerAndAcademicProgramDatatable(sentParameters, facultyId, careerId, academicProgramId, searchValue, user);

        public async Task<object> SearchStudentByTerm(string term, Guid? careerId = null, ClaimsPrincipal user = null, bool onlyActiveStudents = false)
            => await _studentRepository.SearchStudentByTerm(term, careerId, user, onlyActiveStudents);

        public async Task<StudentInformationTemplate> GetStudentinformationById(Guid id)
            => await _studentRepository.GetStudentinformationById(id);

        public async Task<StudentGeneralDataTemplate> GetStudentGeneralDataById(Guid id)
            => await _studentRepository.GetStudentGeneralDataById(id);

        public async Task<bool> ValidateUserRegister(string username, Guid id)
            => await _studentRepository.ValidateUserRegister(username, id);

        public async Task<bool> ValidateUserEmail(string email, Guid id)
            => await _studentRepository.ValidateUserEmail(email, id);

        public async Task<bool> ValidateUserPersonalEmail(string email, Guid id)
            => await _studentRepository.ValidateUserPersonalEmail(email, id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserProceduresDatatable(DataTablesStructs.SentParameters sentParameters, Guid id, string search)
            => await _studentRepository.GetUserProceduresDatatable(sentParameters, id, search);

        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrolledCoursesDatatable(DataTablesStructs.SentParameters sentParameters, Guid id, Guid termId)
            => await _studentRepository.GetEnrolledCoursesDatatable(sentParameters, id, termId);

        public async Task<object> GetAvailableSections(Guid id)
            => await _studentRepository.GetAvailableSections(id);

        public async Task<object> StudentsReport1(bool isCoordinator, List<Guid> careers)
        {
            return await _studentRepository.StudentsReport1(isCoordinator, careers);
        }

        public async Task<object> StudentsReport2(int startYear, int endYear, List<Guid> careers, Guid? facultyId)
        {
            return await _studentRepository.StudentsReport2(startYear, endYear, careers, facultyId);
        }

        public async Task<object> StudentsReport3()
        {
            return await _studentRepository.StudentsReport3();
        }

        public async Task<object> StudentsReport4(bool isCoordinator, List<Guid> careers)
        {
            return await _studentRepository.StudentsReport4(isCoordinator, careers);
        }

        public async Task<object> StudentsReport5(bool isCoordinator, List<Guid> careers)
        {
            return await _studentRepository.StudentsReport5(isCoordinator, careers);
        }
        public async Task<object> StudentsReport6(bool isCoordinator, List<Guid> careers)
        {
            return await _studentRepository.StudentsReport6(isCoordinator, careers);
        }
        public async Task<object> StudentsReport7(bool isCoordinator, List<Guid> careers)
        {
            return await _studentRepository.StudentsReport7(isCoordinator, careers);
        }
        public async Task<object> StudentsReport8(List<Guid> careers, Guid? facultyId)
        {
            return await _studentRepository.StudentsReport8(careers, facultyId);
        }

        public async Task<object> StudentsReport9()
        {
            return await _studentRepository.StudentsReport9();
        }

        public async Task<DataTablesStructs.ReturnedData<StudentFilterTemplate>> GetStudentFilterDatatable(DataTablesStructs.SentParameters parameters, Guid? cid, Guid? fid, Guid? pid, string search, ClaimsPrincipal user = null)
        {
            return await _studentRepository.GetStudentFilterDatatable(parameters, cid, fid, pid, search, user);
        }

        public async Task<DataTablesStructs.ReturnedData<StudentRankingByTermTemplate>> GetStudentRankingByTermDataTable(DataTablesStructs.SentParameters parameters, string userId, Guid termId, Guid? careerId, Guid? academicProgramId, Guid? campusId, string search, ClaimsPrincipal user = null)
        {
            return await _studentRepository.GetStudentRankingByTermDataTable(parameters, userId, termId, careerId, academicProgramId, campusId, search, user);
        }
        public async Task<DataTablesStructs.ReturnedData<StudentRankingForCreditsTemplate>> GetStudentRankingForCreditsDataTable(DataTablesStructs.SentParameters parameters, ClaimsPrincipal user, Guid termId, Guid? careerId, Guid? academicProgramId, Guid? campusId, string search)
        {
            return await _studentRepository.GetStudentRankingForCreditsDataTable(parameters, user, termId, careerId, academicProgramId, campusId, search);
        }

        public async Task<DataTablesStructs.ReturnedData<GraduatedsTemplate>> GetGraduatedsDataTable(DataTablesStructs.SentParameters parameters, ClaimsPrincipal user, Guid? admissionTermId = null, Guid? graduationTermId = null, Guid? careerId = null, Guid? academicProgramId = null, string search = null)
        {
            return await _studentRepository.GetGraduatedsDataTable(parameters, user, admissionTermId, graduationTermId, careerId, academicProgramId, search);
        }

        public async Task<DataTablesStructs.ReturnedData<NewStudentTemplate>> GetNewStudentsDataTable(DataTablesStructs.SentParameters parameters, ClaimsPrincipal user, Guid? admissionTermId = null, Guid? careerId = null, Guid? academicProgramId = null, int? status = null, string search = null)
        {
            return await _studentRepository.GetNewStudentsDataTable(parameters, user, admissionTermId, careerId, academicProgramId, status, search);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetCurriculumStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? curriculumId = null)
            => await _studentRepository.GetCurriculumStudentsDatatable(sentParameters, searchValue, curriculumId);

        public async Task<DataTablesStructs.ReturnedData<object>> SearchList(DataTablesStructs.SentParameters sentParameters, bool isCoordinator, List<Guid> careers, string DNI, string name, string code)
        {
            return await _studentRepository.SearchList(sentParameters, isCoordinator, careers, DNI, name, code);
        }

        public async Task<Student> GetWithIncludes(Guid id)
        {
            return await _studentRepository.GetWithIncludes(id);
        }

        public Task<TemplateDataCV> GetStudentProfileDetail(Guid id)
            => _studentRepository.GetStudentProfileDetail(id);

        public Task<StudentStudiesInfoTemplate> GetStudentStudiesDetail(Guid id)
            => _studentRepository.GetStudentStudiesDetail(id);

        public async Task<GraduatedFiltersTemplate> ReportGraduates(bool isCoordinator, List<Guid> careers)
        {
            return await _studentRepository.ReportGraduates(isCoordinator, careers);
        }

        public async Task<int> CountAllGraduated()
        {
            return await _studentRepository.CountAllGraduated();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GraduatedListReport(DataTablesStructs.SentParameters sentParameters, bool isCoordinator, List<Guid> careers, int gradeType, Guid careerParameterId, int year = 0, int admissionYear = 0)
        {
            return await _studentRepository.GraduatedListReport(sentParameters, isCoordinator, careers, gradeType, careerParameterId, year, admissionYear);
        }

        public async Task<CuantityReportTemplate.Table> ReportNumberGraduates(bool isCoordinator, List<Guid> careers, string startDate, string endDate, Guid careerId)
        {
            return await _studentRepository.ReportNumberGraduates(isCoordinator, careers, startDate, endDate, careerId);
        }

        public async Task<object> GlobalList(bool isCoordinator, Guid? careerId, string dni, string nombre, string codigo, int draw, int start, int length)
        {
            return await _studentRepository.GlobalList(isCoordinator, careerId, dni, nombre, codigo, draw, start, length);
        }

        public async Task<CuantityReportTemplate> ReportBachelorTitle(bool isCoordinator, List<Guid> careers)
        {
            return await _studentRepository.ReportBachelorTitle(isCoordinator, careers);
        }

        public async Task<CuantityReportTemplate> ReportBachelorTitleSunedu(bool isCoordinator, List<Guid> careers)
        {
            return await _studentRepository.ReportBachelorTitleSunedu(isCoordinator, careers);
        }

        public async Task<object> GetBachelors(List<Guid> Faculties, List<Guid> Careers, List<Guid> AcademicPrograms, int GradeType)
        {
            return await _studentRepository.GetBachelors(Faculties, Careers, AcademicPrograms, GradeType);
        }

        public async Task<object> GetBachelorsWithOutConfiguration(List<Guid> Faculties, List<Guid> Careers = null, List<Guid> AcademicPrograms = null)
        {
            return await _studentRepository.GetBachelorsWithOutConfiguration(Faculties, Careers, AcademicPrograms);
        }

        public async Task<DataTablesStructs.ReturnedData<StudentInformationDataTableTemplate>> GetStudentInformationDataTable(DataTablesStructs.SentParameters parameters, Guid cid)
        {
            return await _studentRepository.GetStudentInformationDataTable(parameters, cid);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetReportPersonalInformation(DataTablesStructs.SentParameters parameters, Guid? careerId = null, Guid? departmentId = null, Guid? provinceId = null, Guid? districtId = null, int? sex = null, int? schoolType = null, int? universityPreparation = null, Guid? admissionTypeId = null, int? startAge = null, int? endAge = null)
        {
            return await _studentRepository.GetReportPersonalInformationDataTable(parameters, careerId, departmentId, provinceId, districtId, sex, schoolType, universityPreparation, admissionTypeId, startAge, endAge);
        }

        public async Task<StudyRecordTemplate> GetStudyRecord(Guid studentId)
            => await _studentRepository.GetStudyRecord(studentId);

        public async Task<Student> GetStudentProofOfInCome(Guid studentId)
            => await _studentRepository.GetStudentProofOfInCome(studentId);

        public async Task<Student> GetStudentRecordEnrollment(Guid studentId)
            => await _studentRepository.GetStudentRecordEnrollment(studentId);

        public async Task<Student> GetStudentWithCareerAcademicUser(Guid studentId)
            => await _studentRepository.GetStudentWithCareerAcademicUser(studentId);

        public async Task<Student> GetStudentWithCareerAdmissionAcademicUser(Guid studentId)
            => await _studentRepository.GetStudentWithCareerAdmissionAcademicUser(studentId);

        public async Task<IEnumerable<Student>> GetGraduateds()
            => await _studentRepository.GetGraduateds();

        public async Task<HeadBoardCertificateTemplate> GetStudntCertificate(Guid studentId, string university)
            => await _studentRepository.GetStudntCertificate(studentId, university);

        public async Task<IEnumerable<Student>> GetStudentsBySeciontId(Guid sectionId)
            => await _studentRepository.GetStudentsBySeciontId(sectionId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentDatatableAcademicSituation(DataTablesStructs.SentParameters sentParameters, Guid tid, Guid fid, Guid cid, string searchValue = null, int? academicOrder = null, ClaimsPrincipal user = null, Guid? curriculumId = null, int? year = null)
            => await _studentRepository.GetStudentDatatableAcademicSituation(sentParameters, tid, fid, cid, searchValue, academicOrder, user, curriculumId, year);
        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentDatatableAcademicSituationCumulative(DataTablesStructs.SentParameters sentParameters, Guid tid, Guid fid, Guid cid, string searchValue = null, int? academicOrder = null, ClaimsPrincipal user = null)
            => await _studentRepository.GetStudentDatatableAcademicSituationCumulative(sentParameters, tid, fid, cid, searchValue, academicOrder, user);
        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentDatatableAcademicSituationWithOutComent(DataTablesStructs.SentParameters sentParameters, Guid fid, Guid cid, int? academicOrder = null)
            => await _studentRepository.GetStudentDatatableAcademicSituationWithOutComent(sentParameters, fid, cid, academicOrder);
        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentDatatableAcademicSituationDean(DataTablesStructs.SentParameters sentParameters, Guid fid, Guid cid, int? academicOrder = null)
            => await _studentRepository.GetStudentDatatableAcademicSituationDean(sentParameters, fid, cid, academicOrder);
        public async Task<Student> GetStudentHome(string userId)
            => await _studentRepository.GetStudentHome(userId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsWithCreditsDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, int? type = null, string searchValue = null, bool? onlyWithCredits = null, ClaimsPrincipal user = null)
            => await _studentRepository.GetStudentsWithCreditsDataDatatable(sentParameters, termId, facultyId, careerId, type, searchValue, onlyWithCredits, user);

        public async Task<IEnumerable<Student>> GetAllByFacultyOrCareer(Guid facultyId, Guid careerId)
        {
            return await _studentRepository.GetAllByFacultyOrCareer(facultyId, careerId);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsAcademicHistoryDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, Guid? faculty = null, Guid? career = null, string search = null)
            => await _studentRepository.GetStudentsAcademicHistoryDatatable(sentParameters, user, faculty, career, search);
        public async Task<List<AlumniExcelTemplate>> GetStudentToExcelAlumni(Guid? faculty = null, Guid? career = null, ClaimsPrincipal user = null)
            => await _studentRepository.GetStudentToExcelAlumni(faculty, career, user);
        public async Task<DataTablesStructs.ReturnedData<object>> GetSanctionedStudentDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, string userId, Guid? faculty = null, Guid? career = null, string search = null, Guid? term = null)
            => await _studentRepository.GetSanctionedStudentDatatable(sentParameters, user, userId, faculty, career, search, term);
        public async Task<DataTablesStructs.ReturnedData<object>> GetDesertionStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null)
            => await _studentRepository.GetDesertionStudentDatatable(sentParameters, faculty, career, search, user);
        
        public async Task<List<SacntionedExcelTemplate>> GetStudentToExcelSantioned(Guid? term = null, Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null)
            => await _studentRepository.GetStudentToExcelSantioned(term, faculty, career, search, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsUnbeatenDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, string userId, Guid? faculty = null, Guid? career = null, string search = null)
            => await _studentRepository.GetStudentsUnbeatenDatatable(sentParameters, user, userId, faculty, career, search);
        public async Task<List<UnbeatenExcelTemplate>> GetStudentToExcelUnbeaten(Guid? faculty = null, Guid? career = null, ClaimsPrincipal user = null)
            => await _studentRepository.GetStudentToExcelUnbeaten(faculty, career, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetExpelledStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null)
            => await _studentRepository.GetExpelledStudentDatatable(sentParameters, faculty, career, search, user);

        public async Task<List<SacntionedExcelTemplate>> GetStudentToExcelDesertion(Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null)
            => await _studentRepository.GetStudentToExcelDesertion(faculty, career, search, user);

        public async Task<List<SacntionedExcelTemplate>> GetStudentToExcelExpelled(Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null)
            => await _studentRepository.GetStudentToExcelExpelled(faculty, career, search, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetCafeteriaStudentsDatatable(DataTablesStructs.SentParameters sentParameters, bool isCheckedAll, string searchValue = null, Guid? careerId = null, Guid? facultyId = null)
        {
            return await _studentRepository.GetCafeteriaStudentsDatatable(sentParameters, isCheckedAll, searchValue, careerId, facultyId);
        }

        public async Task<List<Student>> GetAllStudentsWithFacultyAndTerm(Guid facultyId, Guid careerId, Guid? termId = null)
            => await _studentRepository.GetAllStudentsWithFacultyAndTerm(facultyId, careerId, termId);

        public IQueryable<Student> GetStudentOrdered(int status, Guid careerId)
            => _studentRepository.GetStudentOrdered(status, careerId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentDatatableEnrolledStudent(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, string userId, Guid termId, Guid facultyId, Guid careerId, int year, int? type = null, Guid? academicProgramId = null, int? cycle = null, Guid? campusId = null, string search = null)
            => await _studentRepository.GetStudentDatatableEnrolledStudent(sentParameters, User, userId, termId, facultyId, careerId, year, type, academicProgramId, cycle, campusId, search);
        
        public async Task<DataTablesStructs.ReturnedData<object>> GetPostPaymentEnrolledStudentDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, Guid termId, Guid? facultyId = null, Guid? careerId = null, int? year = null, int status = 1, string search = null)
            => await _studentRepository.GetPostPaymentEnrolledStudentDatatable(sentParameters, User, termId, facultyId, careerId, year, status, search);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentDatatableEnrollentTutoring(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, Guid termId, Guid? careerId = null, int? cycle = null, string search = null)
            => await _studentRepository.GetStudentDatatableEnrollentTutoring(sentParameters, User, termId, careerId, cycle, search);
        public IQueryable<Student> GetStudentIncludeInformationGeo(string userId)
            => _studentRepository.GetStudentIncludeInformationGeo(userId);

        public async Task<Student> GetStudentIncludeInformationUser(string studentId)
            => await _studentRepository.GetStudentIncludeInformationUser(studentId);

        public IQueryable<Student> GetStudentIncludeInformationGeoCareer(string userId)
            => _studentRepository.GetStudentIncludeInformationGeoCareer(userId);

        public async Task<Student> GetStudentIncludeInformation(string studentId)
            => await _studentRepository.GetStudentIncludeInformation(studentId);

        public async Task<Student> GetStudentIncludeInformationGeoCareer2(Guid StudentId)
            => await _studentRepository.GetStudentIncludeInformationGeoCareer2(StudentId);

        public async Task<object> GetAllStudentWithData(Guid termId)
            => await _studentRepository.GetAllStudentWithData(termId);

        public async Task<object> GetEntrantStudentsEnrolledDatatable(Guid careerId, Guid termId, string search)
            => await _studentRepository.GetEntrantStudentsEnrolledDatatable(careerId, termId, search);

        public async Task<IEnumerable<Student>> GetAllStudentWithDataIn(Guid id)
            => await _studentRepository.GetAllStudentWithDataIn(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetQualifiedStudentsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, string searchValue = null, ClaimsPrincipal user = null)
            => await _studentRepository.GetQualifiedStudentsDatatable(sentParameters, careerId, searchValue, user);

        public async Task<object> GetQualifiedStudentsChart(Guid? careerId = null, ClaimsPrincipal user = null)
            => await _studentRepository.GetQualifiedStudentsChart(careerId, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetBachelorStudentsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, string searchValue = null, ClaimsPrincipal user = null)
            => await _studentRepository.GetBachelorStudentsDatatable(sentParameters, careerId, searchValue, user);

        public async Task<object> GetBachelorStudentsChart(Guid? careerId = null, ClaimsPrincipal user = null)
            => await _studentRepository.GetBachelorStudentsChart(careerId, user);

        public async Task<object> GetGraduatedAndBachelorStudentPieChart(Guid? termId = null, ClaimsPrincipal user = null)
            => await _studentRepository.GetGraduatedAndBachelorStudentPieChart(termId, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentByStatusListDatatable(DataTablesStructs.SentParameters sentParameters, IEnumerable<int> status, Guid? termId = null, ClaimsPrincipal user = null)
            => await _studentRepository.GetStudentByStatusListDatatable(sentParameters, status, termId, user);

        public async Task<object> GetBachelorAndQualifiedStudentPieChart(Guid? termId = null, ClaimsPrincipal user = null)
            => await _studentRepository.GetBachelorAndQualifiedStudentPieChart(termId, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetBachillerInTimeDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null)
        {
            return await _studentRepository.GetBachillerInTimeDatatable(sentParameters, user);
        }

        public async Task<object> GetBachillerInTimeChart(ClaimsPrincipal user = null)
        {
            return await _studentRepository.GetBachillerInTimeChart(user);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetGraduatedStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, Guid? graduationTermId = null, ClaimsPrincipal user = null)
        {
            return await _studentRepository.GetGraduatedStudentDatatable(sentParameters, careerId, graduationTermId, user);
        }

        public async Task<object> GetGraduatedStudentChart(Guid? careerId = null, Guid? graduationTermId = null, ClaimsPrincipal user = null)
        {
            return await _studentRepository.GetGraduatedStudentChart(careerId, graduationTermId, user);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetGraduatedStudentInTimeDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, Guid? graduationTermId = null, ClaimsPrincipal user = null)
        {
            return await _studentRepository.GetGraduatedStudentInTimeDatatable(sentParameters, careerId, graduationTermId, user);
        }

        public async Task<object> GetGraduatedStudentInTimeChart(Guid? careerId = null, Guid? graduationTermId = null, ClaimsPrincipal user = null)
        {
            return await _studentRepository.GetGraduatedStudentInTimeChart(careerId, graduationTermId, user);
        }

        public async Task<object> GetStudentJson(Guid? id = null)
            => await _studentRepository.GetStudentJson(id);

        public async Task<EnrollmentInformationTemplate> GetStudentEnrollmentInformation(Guid studentId)
            => await _studentRepository.GetStudentEnrollmentInformation(studentId);

        public async Task<Student> GetStudentWithDataByUserId(string userId)
            => await _studentRepository.GetStudentWithDataByUserId(userId);

        public async Task<Student> GetStudentByUserName(string username)
            => await _studentRepository.GetStudentByUserName(username);

        public async Task<Student> GetStudentIdString(string studentId)
            => await _studentRepository.GetStudentIdString(studentId);

        public async Task<int> GetStudentsEnrolledCountByTerm(Guid termId, ClaimsPrincipal user = null)
            => await _studentRepository.GetStudentsEnrolledCountByTerm(termId, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetNextEnrollmentStudentsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, Guid? careerId = null, int? academicYear = null, ClaimsPrincipal user = null)
            => await _studentRepository.GetNextEnrollmentStudentsDatatable(sentParameters, facultyId, careerId, academicYear, user);

        public async Task<object> GetStudentAcademicYearsSelectClientSide(Guid? faculty = null, Guid? career = null)
            => await _studentRepository.GetStudentAcademicYearsSelectClientSide(faculty, career);

        public async Task<Student> GetStudentByStudentInformationId(Guid studentInformId)
            => await _studentRepository.GetStudentByStudentInformationId(studentInformId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDeserterStudentReportDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null)
        {
            return await _studentRepository.GetDeserterStudentReportDatatable(sentParameters, user);
        }

        public async Task<object> GetDeserterStudentReportChart(ClaimsPrincipal user = null)
        {
            return await _studentRepository.GetDeserterStudentReportChart(user);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTransferStudentDataTable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, string userId, Guid? faculty = null, Guid? career = null)
            => await _studentRepository.GetTransferStudentDataTable(sentParameters, User, userId, faculty, career);

        public async Task<object> GetEnrolledByFacultyAndTermChart(Guid? termId = null, Guid? facultyId = null, ClaimsPrincipal user = null)
            => await _studentRepository.GetEnrolledByFacultyAndTermChart(termId, facultyId, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrolledByFacultyAndTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? facultyId = null, ClaimsPrincipal user = null)
            => await _studentRepository.GetEnrolledByFacultyAndTermDatatable(sentParameters, termId, facultyId, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentDatatableEnrolledBySectionsStudent(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, string userId, Guid termId, Guid facultyId, Guid careerId, Guid academicProgramId, Guid curriculumId, Guid courseId, Guid sectionId)
        {
            return await _studentRepository.GetStudentDatatableEnrolledBySectionsStudent(sentParameters, User, userId, termId, facultyId, careerId, academicProgramId, curriculumId, courseId, sectionId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetOthersEnrolledCoursesDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId, Guid termId)
            => await _studentRepository.GetOthersEnrolledCoursesDatatable(sentParameters, studentId, termId);

        public async Task<string> GetFacultyByStudentIncludeCareerUser(string userId)
            => await _studentRepository.GetFacultyByStudentIncludeCareerUser(userId);

        public async Task<string> GetStudentFullNameIncludeUserByUserId(string userId)
            => await _studentRepository.GetStudentFullNameIncludeUserByUserId(userId);
        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrollmentDatatableCon(DataTablesStructs.SentParameters sentParameters)
            => await _studentRepository.GetEnrollmentDatatableCon(sentParameters);
        public async Task<Student> GetByIdWithData(Guid id)
            => await _studentRepository.GetByIdWithData(id);
        public async Task<List<Student>> GetStudentWitData()
            => await _studentRepository.GetStudentWitData();
        public async Task<bool> GetStudentToUpdateByCode(string code)
            => await _studentRepository.GetStudentToUpdateByCode(code);
        public async Task<Student> GetWithData(Guid id)
            => await _studentRepository.GetWithData(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GraduatedListReportToAcademicRecord(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, int gradeType, Guid careerId, int year)
            => await _studentRepository.GraduatedListReportToAcademicRecord(sentParameters, user, gradeType, careerId, year);

        public async Task<IEnumerable<EnrolledStudentExcelTemplate>> GetEnrolledStudentsExcelReport(ClaimsPrincipal User, string userId, Guid termId, Guid facultyId, Guid careerId, int? year = null, int? type = null, Guid? academicProgramId = null, Guid? campusId = null)
            => await _studentRepository.GetEnrolledStudentsExcelReport(User, userId, termId, facultyId, careerId, year, type, academicProgramId, campusId);
        public async Task<IEnumerable<EnrolledStudentExcelTemplate>> GetEnrolledTutoringStudentsExcelReport(ClaimsPrincipal User, Guid termId, Guid? careerId = null, int? year = null)
            => await _studentRepository.GetEnrolledTutoringStudentsExcelReport(User, termId, careerId, year);
        public async Task<DataTablesStructs.ReturnedData<SubstituteExamTemplate>> GetStudentsForSubstiteExam(DataTablesStructs.SentParameters sentParameters, Guid termid, Guid sectionId, string search)
        {
            return await _studentRepository.GetStudentsForSubstiteExam(sentParameters, termid, sectionId, search);
        }

        public async Task<List<SubstituteExamTemplate>> GetStudentsForSubstituteExamDataAsync(Guid termid, Guid sectionId, string search = null)
        {
            return await _studentRepository.GetStudentsForSubstituteExamDataAsync(termid, sectionId, search);
        }

        public async Task<IEnumerable<AcademicSituationExcelTemplate>> GetStudentAcademicSituationExcelTemplate(Guid? termId, Guid? facultyId, Guid? careerId, int? academicOrder, ClaimsPrincipal user = null)
            => await _studentRepository.GetStudentAcademicSituationExcelTemplate(termId, facultyId, careerId, academicOrder, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetGlobalListDatatable(DataTablesStructs.SentParameters sentParameters, string dni = null, string name = null, string userName = null, Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null)
            => await _studentRepository.GetGlobalListDatatable(sentParameters, dni, name, userName, termId, facultyId, careerId, academicProgramId);

        public Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeReportGraduatedDatatable(DataTablesStructs.SentParameters sentParameters, string userName = null, string dni = null, string fullName = null, int? studentState = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, Guid? admissionTermId = null, Guid? graduationTermId = null, int? graduationYear = null)
            => _studentRepository.GetJobExchangeReportGraduatedDatatable(sentParameters, userName, dni, fullName, studentState, facultyId, careerId, academicProgramId, admissionTermId, graduationTermId, graduationYear);

        public Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeReportStudentDatatable(DataTablesStructs.SentParameters sentParameters, string userName = null, string dni = null, string fullName = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, Guid? admissionTermId = null)
            => _studentRepository.GetJobExchangeReportStudentDatatable(sentParameters, userName, dni, fullName, facultyId, careerId, academicProgramId, admissionTermId);
        public async Task DownloadGlobalListExcel(IXLWorksheet worksheet, string dni = null, string name = null, string userName = null, Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null)
        {
            await _studentRepository.DownloadGlobalListExcel(worksheet, dni, name, userName, termId, facultyId, careerId, academicProgramId);
        }

        public Task<List<JobExchangeGraduatedDataTemplate>> GetJobExchangeReportGraduatedData(string userName = null, string dni = null, string fullName = null, int? studentState = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, Guid? admissionTermId = null, Guid? graduationTermId = null, int? graduationYear = null)
            => _studentRepository.GetJobExchangeReportGraduatedData(userName ,dni, fullName, studentState, facultyId, careerId, academicProgramId, admissionTermId, graduationTermId, graduationYear);

        public Task<List<JobExchangeStudentDataTemplate>> GetJobExchangeReportStudentData(string userName = null, string dni = null, string fullName = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, Guid? admissionTermId = null)
            => _studentRepository.GetJobExchangeReportStudentData(userName, dni, fullName, facultyId, careerId, academicProgramId, admissionTermId);
        public Task<List<StudentJobExchangeReportTemplate>> ReportGlobalListData(string dni = null, string userName = null, Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null)
            => _studentRepository.ReportGlobalListData(dni, userName, termId, facultyId, careerId, academicProgramId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsByAdmissionTermReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, ClaimsPrincipal user = null, bool? showEnrolled = null, Guid? admissionTypeId = null)
            => await _studentRepository.GetStudentsByAdmissionTermReportDatatable(sentParameters, termId, facultyId, careerId, academicProgramId, user, showEnrolled, admissionTypeId);
        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsByAdmissionTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? careerId = null,Guid? facultyId = null, string search = null)
            => await _studentRepository.GetStudentsByAdmissionTermDatatable(sentParameters, termId, careerId, facultyId, search);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDebtorStudentsReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, ClaimsPrincipal user = null)
   => await _studentRepository.GetDebtorStudentsReportDatatable(sentParameters, termId, facultyId, careerId, academicProgramId, user);

        public async Task<IEnumerable<Student>> GetStudentsByAdmissionTermPdfData(Guid termId, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, ClaimsPrincipal user = null, Guid? admissionTypeId = null)
            => await _studentRepository.GetStudentsByAdmissionTermPdfData(termId, facultyId, careerId, academicProgramId, user, admissionTypeId);

        public async Task<IEnumerable<EnrolledStudentExcelTemplate>> GetStudentsByEquivalenceData(Guid termId, Guid? facultyId = null, Guid? careerId = null)
            => await _studentRepository.GetStudentsByEquivalenceData(termId, facultyId, careerId);

        public async Task<List<DebtStudentTemplate>> GetDebtorStudentsDataPdf(Guid termId, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, ClaimsPrincipal user = null)
          => await _studentRepository.GetDebtorStudentsDataPdf(termId, facultyId, careerId, academicProgramId, user);

        public async Task<IEnumerable<Student>> GetEnrolledStudentBytermId(Guid termId, Guid? facultyId = null, Guid? careerId = null, int? studentAcademicYear = null, int? status = null, Guid? campusId = null)
            => await _studentRepository.GetEnrolledStudentBytermId(termId, facultyId, careerId, studentAcademicYear, status, campusId);

        public async Task<IEnumerable<Student>> GetEnrolledTutoringStudentBytermId(ClaimsPrincipal User, Guid termId, Guid? careerId = null, int? studentAcademicYear = null)
            => await _studentRepository.GetEnrolledTutoringStudentBytermId(User, termId, careerId, studentAcademicYear);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsWithCreditsIncomingDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, int? type = null, string searchValue = null, bool? onlyWithCredits = null, ClaimsPrincipal user = null)
            => await _studentRepository.GetStudentsWithCreditsIncomingDataDatatable(sentParameters, termId, facultyId, careerId, type, searchValue, onlyWithCredits, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsByRecognitionDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, string search = null, ClaimsPrincipal user = null)
            => await _studentRepository.GetStudentsByRecognitionDatatable(sentParameters, termId, facultyId, careerId, search, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsWithExtemporaneousEnrollmentDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, string searchValue = null, ClaimsPrincipal user = null)
            => await _studentRepository.GetStudentsWithExtemporaneousEnrollmentDatatable(sentParameters, termId, facultyId, careerId, searchValue, user);

        public async Task<object> SearchIncomingStudentByTerm(string term, Guid? careerId = null, ClaimsPrincipal user = null)
            => await _studentRepository.SearchIncomingStudentByTerm(term, careerId, user);

        public async Task<List<Student>> GetByDni(string document)
        {
            return await _studentRepository.GetByDni(document);
        }

        public async Task<List<Student>> GetStudentsByDniAndTerm(string document, Guid termId)
            => await _studentRepository.GetStudentsByDniAndTerm(document, termId);

        public async Task<ApplicationUser> BankBatchUserByDocument(string document, int length)
            => await _studentRepository.BankBatchUserByDocument(document, length);

        public IQueryable<Student> GetActiveStudents()
        {
            return _studentRepository.GetActiveStudents();
        }

        public IQueryable<Student> GetStudentWithStudentSectionsByTermId(Guid termId)
        {
            return _studentRepository.GetStudentWithStudentSectionsByTermId(termId);
        }

        public async Task<object> GetTopFacultiesEnrollled(Guid id)
        {
            return await _studentRepository.GetTopFacultiesEnrollled(id);
        }

        public async Task UpdateStudentsCurrentAcademicYearJob(string connectionString)
        {
            await _studentRepository.UpdateStudentsCurrentAcademicYearJob(connectionString);
        }

        public async Task UpdateStudentOrderJob()
        {
            await _studentRepository.UpdateStudentOrderJob();
        }

        public async Task FixCurriculumStudentsJob()
        {
            await _studentRepository.FixCurriculumStudentsJob();
        }

        public async Task<int> UpdateStudentAcademicYearJob()
        {
            return await _studentRepository.UpdateStudentAcademicYearJob();
        }

        public async Task UpdateStudentStatusJob()
        {
            await _studentRepository.UpdateStudentStatusJob();
        }

        public async Task UnbeaterStudentsJob()
        {
            await _studentRepository.UnbeaterStudentsJob();
        }

        public async Task CreateStudentsJob(UserManager<ApplicationUser> userManager, int count, Term term, bool isIntegrated, Guid procedureId_conceptId)
        {
            await _studentRepository.CreateStudentsJob(userManager, count, term, isIntegrated, procedureId_conceptId);
        }

        public async Task<object> SearchEnabledStudentForCourseByTerm(string term, Guid? courseId = null, ClaimsPrincipal user = null)
            => await _studentRepository.SearchEnabledStudentForCourseByTerm(term, courseId, user);

        public async Task<IEnumerable<Student>> GetFacultyStudentsEnrolled(Guid termId)
            => await _studentRepository.GetFacultyStudentsEnrolled(termId);

        public void RemoveRange(List<Student> studentList)
        {
            _studentRepository.RemoveRange(studentList);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrolledStudentLastTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, Guid? careerId = null, string searchValue = null)
            => await _studentRepository.GetEnrolledStudentLastTermDatatable(sentParameters, facultyId, careerId, searchValue);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsByInstitutionalRecordGeneralDatatable(DataTablesStructs.SentParameters sentParameters, Guid id, Guid termId, byte? sisfohClasification = null, Guid? categorizationLevelId = null, Guid? careerId = null)
        {
            return await _studentRepository.GetStudentsByInstitutionalRecordGeneralDatatable(sentParameters, id, termId, sisfohClasification, categorizationLevelId, careerId);
        }
        public async Task<List<StudentsByInstitutionalRecordTemplate>> GetStudentsByInstitutionalRecordGeneralData(Guid id, Guid termId, byte sisfohClasification)
        {
            return await _studentRepository.GetStudentsByInstitutionalRecordGeneralData(id, termId, sisfohClasification);
        }

        public async Task DownloadExcelGraduates(IXLWorksheet worksheet, bool isCoordinator, List<Guid> careers, int gradeType, Guid careerParameterId, int year = 0, int admissionYear = 0)
        {
            await _studentRepository.DownloadExcelGraduates(worksheet, isCoordinator, careers, gradeType, careerParameterId, year, admissionYear);
        }


        public Task<decimal> GetApprovedCreditsByStudentId(Guid studentId) => _studentRepository.GetApprovedCreditsByStudentId(studentId);

        public async Task<DataTablesStructs.ReturnedData<Student>> GetAgreementStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, string academicCoordinatorId = null, string academicRecordStaffId = null)
        {
            return await _studentRepository.GetAgreementStudentsDatatable(sentParameters, searchValue, facultyId, careerId, academicProgramId, academicCoordinatorId, academicRecordStaffId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentDatatableEnrolledExtraCreditStudent(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, string userId, Guid termId, Guid facultyId, Guid careerId, int year, int? type = null, Guid? academicProgramId = null, int? cycle = null)
        {
            return await _studentRepository.GetStudentDatatableEnrolledExtraCreditStudent(sentParameters, User, userId, termId, facultyId, careerId, year, type, academicProgramId, cycle);
        }

        public async Task<List<OriginReportTemplate>> GetOriginStudentReport(Guid termId, ClaimsPrincipal user = null)
            => await _studentRepository.GetOriginStudentReport(termId, user);
        public async Task<DataTablesStructs.ReturnedData<object>> GetOriginStudent(List<Guid> careerId, DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? deparment = null, Guid? province = null, Guid? district = null)
            => await _studentRepository.GetOriginStudent(careerId, sentParameters, termId, deparment, province, district);
        public async Task<List<ReportOriginTemplate>> GetOriginStudentDatatable(List<Guid> careerId, Guid? termId = null, Guid? deparment = null, Guid? province = null, Guid? district = null, ClaimsPrincipal user = null)
            => await _studentRepository.GetOriginStudentDatatable(careerId, termId, deparment, province, district, user);
        public async Task<List<StudentOriginTemplate>> GetOriginStudentReportUniqe(List<Guid> careerId, Guid? termId = null, Guid? deparment = null, Guid? province = null, Guid? district = null, ClaimsPrincipal user = null)
            => await _studentRepository.GetOriginStudentReportUniqe(careerId, termId, deparment, province, district, user);

        public async Task<StudentSexReportTemplate> GetStudentSexReport(Guid termId, ClaimsPrincipal user = null)
            => await _studentRepository.GetStudentSexReport(termId, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentSecondCareerDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, string userId, Guid termId, Guid facultyId, Guid careerId, Guid? academicProgramId = null, int? academicYear = null)
            => await _studentRepository.GetStudentSecondCareerDatatable(sentParameters, User, userId, termId, facultyId, careerId, academicProgramId, academicYear);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsEnrolledMultipleCareersDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, string userId, Guid termId, Guid facultyId, Guid careerId, Guid? academicProgramId = null, int? academicYear = null, string search = null)
            => await _studentRepository.GetStudentsEnrolledMultipleCareersDatatable(sentParameters, User, userId, termId, facultyId, careerId, academicProgramId, academicYear, search);

        public async
        Task<Select2Structs.ResponseParameters> GetStudentsByCareer(Select2Structs.RequestParameters requestParameters, int currentAcademicYear, Guid id, Expression<Func<Student, Select2Structs.Result>> selectPredicate = null, Func<Student, string[]> searchValuePredicate = null, string searchValue = null, int? status = null)
            => await _studentRepository.GetStudentsByCareer(requestParameters, currentAcademicYear, id, selectPredicate, searchValuePredicate, searchValue, status);
        public async Task<Select2Structs.ResponseParameters> GetStudentsByCareerSelect2(Select2Structs.RequestParameters requestParameters, int? currentAcademicYear = null, Guid? id = null, string searchValue = null, int? status = null)
            => await _studentRepository.GetStudentsByCareerSelect2(requestParameters, currentAcademicYear, id, searchValue);

        public async Task<Select2Structs.ResponseParameters> GetStudentsWithPendingCourse(Select2Structs.RequestParameters requestParameters, Guid courseId, string searchValue)
            => await _studentRepository.GetStudentsWithPendingCourse(requestParameters, courseId, searchValue);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsToGraduateDatatable(DataTablesStructs.SentParameters parameters, Guid? termId, Guid? facultyId, Guid? careerId, Guid? curriculumId, string searchValue, ClaimsPrincipal user)
            => await _studentRepository.GetStudentsToGraduateDatatable(parameters, termId, facultyId, careerId, curriculumId, searchValue, user);

        public async Task<IEnumerable<StudentSiriesTemplate>> GetStudentsExcelSiriesReport(ClaimsPrincipal user, Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, Guid? campusId = null, int? year = null)
            => await _studentRepository.GetStudentsExcelSiriesReport(user, termId, facultyId, careerId, campusId, year);
        
        public async Task<IEnumerable<StudentSuneduTemplate>> GetStudentsSuneduReport(ClaimsPrincipal user, Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, Guid? campusId = null, int? year = null)
            => await _studentRepository.GetStudentsSuneduReport(user, termId, facultyId, careerId, campusId, year);

        public async Task<DataTablesStructs.ReturnedData<object>> GetNonEnrolledStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null, int? year = null, bool excludeLastYear = false)
            => await _studentRepository.GetNonEnrolledStudentDatatable(sentParameters, faculty, career, search, user, year, excludeLastYear);

        public async Task<List<NonEnrolledStudentTemplate>> GetNonEnrolledStudentData(Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null, int? year = null, bool excludeLastYear = false)
            => await _studentRepository.GetNonEnrolledStudentData(faculty, career, search, user, year, excludeLastYear);

        public async Task DownloadExcelStudentInformationData(IXLWorksheet worksheet, Guid? careerId = null, Guid? departmentId = null, Guid? provinceId = null, Guid? districtId = null, int? sex = null, int? schoolType = null, int? universityPreparation = null, Guid? admissionTypeId = null, int? startAge = null, int? endAge = null)
        {
            await _studentRepository.DownloadExcelStudentInformationData(worksheet, careerId, departmentId, provinceId, districtId, sex, schoolType, universityPreparation, admissionTypeId, startAge, endAge);
        }

        public async Task DownloadExcelStudentFamilyInformationData(IXLWorksheet worksheet, Guid? careerId = null, Guid? departmentId = null, Guid? provinceId = null, Guid? districtId = null, int? sex = null, int? schoolType = null, int? universityPreparation = null, Guid? admissionTypeId = null, int? startAge = null, int? endAge = null)
        {
            await _studentRepository.DownloadExcelStudentFamilyInformationData(worksheet, careerId, departmentId, provinceId, districtId, sex, schoolType, universityPreparation, admissionTypeId, startAge, endAge);
        }

        public async Task<decimal> GetRequiredApprovedCredits(Guid studentId)
            => await _studentRepository.GetRequiredApprovedCredits(studentId);

        public async Task<decimal> GetElectiveApprovedCredits(Guid studentId)
            => await _studentRepository.GetElectiveApprovedCredits(studentId);

        public async Task<int> CalculateEnrollmentAcademicYear(Guid studentId, Guid termId)
            => await _studentRepository.CalculateEnrollmentAcademicYear(studentId, termId);

        public async Task<List<StudentMultipleCareersTemplate>> GetStudentsEnrolledMultipleCareersData(ClaimsPrincipal User, string userId, Guid termId, Guid facultyId, Guid careerId, Guid? academicProgramId = null, int? academicYear = null)
            => await _studentRepository.GetStudentsEnrolledMultipleCareersData(User, userId, termId, facultyId, careerId, academicProgramId, academicYear);

        public async Task<List<DateTime>> GetStudentsBirthData(Guid termId, Guid? careerId, ClaimsPrincipal user = null)
            => await _studentRepository.GetStudentsBirthData(termId, careerId, user);

        public async Task<object> GetTotalNumberOfStudentsEnrolledByCareerAndTermIdChart(Guid? termId = null, ClaimsPrincipal user = null)
            => await _studentRepository.GetTotalNumberOfStudentsEnrolledByCareerAndTermIdChart(termId, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetReEnrolledStudentsDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId, Guid? careerId, ClaimsPrincipal user, string search = null)
            => await _studentRepository.GetReEnrolledStudentsDatatable(sentParameters, termId, facultyId, careerId, user, search);

        public Task<DataTablesStructs.ReturnedData<object>> GetGraduatedPostulantDatatable(DataTablesStructs.SentParameters sentParameters, int state, Guid? termId = null, ClaimsPrincipal user = null)
            => _studentRepository.GetGraduatedPostulantDatatable(sentParameters, state, termId, user);

        public Task<object> GetGraduatedPostulantChart(int state, Guid? termId = null, ClaimsPrincipal user = null)
            => _studentRepository.GetGraduatedPostulantChart(state, termId, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserLoginStudentsDatatable(DataTablesStructs.SentParameters sentParameters, byte system, byte roleType, string startDate = null, string endDate =null, string search = null)
        => await _studentRepository.GetUserLoginStudentsDatatable(sentParameters, system, roleType, startDate, endDate, search);

        public async Task<DataTablesStructs.ReturnedData<object>> GetInstitutionalWelfareStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId , Guid? facultyId = null, Guid ? careerId = null, Guid? admissionTypeId = null, string search = null)
      => await _studentRepository.GetInstitutionalWelfareStudentDatatable(sentParameters,termId, facultyId, careerId, admissionTypeId, search);

        public async Task<List<StudentJobExchangeExcel>> ExcelStudentsReport1(bool isCoordinator, List<Guid> careers)
        {
            return await _studentRepository.ExcelStudentsReport1(isCoordinator, careers);
        }
        public async Task<List<StudentJobExchangeExcel>> ExcelStudentsReport8(List<Guid> careers, Guid? facultyId)
        {
            return await _studentRepository.ExcelStudentsReport8(careers, facultyId);
        }

        public async Task<List<StudentJobExchangeExcel>> ExcelStudentsReport9(Tuple<bool, List<Guid>> verification)
        {
            return await _studentRepository.ExcelStudentsReport9(verification);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsAcademicSummaryDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, Guid termId, Guid? careerId, bool? unbeaten, string search = null)
            => await _studentRepository.GetStudentsAcademicSummaryDatatable(sentParameters, user, termId, careerId, unbeaten, search);

        public async Task<bool> IsSanctionedStudentValidToStudy(Guid studentId) 
            => await _studentRepository.IsSanctionedStudentValidToStudy(studentId);

        public Task<DataTablesStructs.ReturnedData<object>> GetStudentsWithAverageFinalGrades(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid facultyId, int? academicYear = null, Guid? careerId = null, string search = null, ClaimsPrincipal user = null)
            => _studentRepository.GetStudentsWithAverageFinalGrades(sentParameters, termId, facultyId, academicYear, careerId, search, user);

        public Task<object> GetAllSelectByStatus(int? status = null)
            => _studentRepository.GetAllSelectByStatus(status);

        public Task<DataTablesStructs.ReturnedData<object>> GetAverageTimeToFinishByCareerAndTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid graduationTermId, Guid? careerId = null)
            => _studentRepository.GetAverageTimeToFinishByCareerAndTermDatatable(sentParameters, graduationTermId, careerId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetFutureGraduatedStudentDatatable(DataTablesStructs.SentParameters parameters, Guid? careerId, Guid? curriculumId, string searchValue, ClaimsPrincipal user)
            => await _studentRepository.GetFutureGraduatedStudentDatatable(parameters, careerId, curriculumId, searchValue, user);

        public async Task<decimal> GetEnrolledCreditsByStudentId(Guid studentId, byte? status = null)
            => await _studentRepository.GetEnrolledCreditsByStudentId(studentId, status);

        public async Task<List<StudentAcademicSummaryTemplate>> GetStudentsAcademicSummaryData(ClaimsPrincipal user, Guid termId, Guid? careerId, bool? unbeaten, string search = null)
            => await _studentRepository.GetStudentsAcademicSummaryData(user, termId, careerId, unbeaten, search);

        public async Task<Term> GetLastTermEnrolled(Guid studentId)
            => await _studentRepository.GetLastTermEnrolled(studentId);
        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsManagementDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, string searchValue = null)
            => await _studentRepository.GetStudentsManagementDatatable(sentParameters, facultyId, careerId, academicProgramId, searchValue);

        public async Task<object> GetWithdrawnStudentsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId, string search)
            => await _studentRepository.GetWithdrawnStudentsDatatable(sentParameters, termId, search);

        public Task<StudentGraduatedSurveyInformation> GetStudentGraduatedSurveyInformation(string userId)
            => _studentRepository.GetStudentGraduatedSurveyInformation(userId);

        public Task<DataTablesStructs.ReturnedData<object>> GetStudentByStatusDatatable(DataTablesStructs.SentParameters sentParameters, List<int> status, string searchValue = null, Guid? facultyId = null, Guid? careerId = null, List<Guid> careers = null)
            => _studentRepository.GetStudentByStatusDatatable(sentParameters, status, searchValue,facultyId, careerId, careers);

        public Task<DataTablesStructs.ReturnedData<object>> GetStudentDegreeReportDatatable(DataTablesStructs.SentParameters sentParameters, List<Guid> careers, int? graduatedYear = null, Guid? graduatedTermId = null)
            => _studentRepository.GetStudentDegreeReportDatatable(sentParameters, careers, graduatedYear, graduatedTermId);

        public Task<object> GetStudentDegreeReportChart(List<Guid> careers, int? graduatedYear = null, Guid? graduatedTermId = null)
            => _studentRepository.GetStudentDegreeReportChart(careers, graduatedYear, graduatedTermId);

        public async Task<StudentProcedureResult> StudentCareerTransferRequest(ClaimsPrincipal user, Guid studentId, Guid newCareerId, Guid newCurriculumId)
            => await _studentRepository.StudentCareerTransferRequest(user, studentId, newCareerId, newCurriculumId);

        public Task<DataTablesStructs.ReturnedData<object>> GetStudentDegreeFirstJobReportDatatable(DataTablesStructs.SentParameters sentParameters, List<Guid> careers, int? graduatedYear = null, Guid? graduatedTermId = null)
            => _studentRepository.GetStudentDegreeFirstJobReportDatatable(sentParameters, careers, graduatedYear, graduatedTermId);

        public Task<object> GetStudentDegreeFirstJobReportChart(List<Guid> careers, int? graduatedYear = null, Guid? graduatedTermId = null)
            => _studentRepository.GetStudentDegreeFirstJobReportChart(careers, graduatedYear, graduatedTermId);

        public async Task<StudentProcedureResult> StudentAcademicYearWithdrawalRequest(ClaimsPrincipal user, Guid studentId)
            => await _studentRepository.StudentAcademicYearWithdrawalRequest(user, studentId);

        public async Task<StudentProcedureResult> StudentCourseWithdrawalRequest(ClaimsPrincipal user, Guid studentSectionId)
            => await _studentRepository.StudentCourseWithdrawalRequest(user, studentSectionId);

        public async Task<StudentProcedureResult> ResignStudentRequest(ClaimsPrincipal user, Guid studentId, string reason, string fileUrl)
            => await _studentRepository.ResignStudentRequest(user, studentId, reason, fileUrl);

        public async Task<StudentProcedureResult> ReentryStudentRequest(ClaimsPrincipal user, Guid studentId, string fileUrl)
            => await _studentRepository.ReentryStudentRequest(user, studentId, fileUrl);

        public async Task<StudentProcedureResult> StudentReservationRequest(ClaimsPrincipal user, Guid studentId, string receipt, string fileUrl, string observation)
            => await _studentRepository.StudentReservationRequest(user, studentId, receipt, fileUrl, observation);

        public async Task<StudentProcedureResult> StudentChangeAcademicProgramRequest(ClaimsPrincipal user, Guid studentId, Guid newAcademicProgramId)
            => await _studentRepository.StudentChangeAcademicProgramRequest(user, studentId, newAcademicProgramId);

        public async Task<List<FutureGradutedStudentTemplate>> GetFutureGraduatedStudentsTemplate(Guid? careerId, Guid? curriculumId, ClaimsPrincipal user)
            => await _studentRepository.GetFutureGraduatedStudentsTemplate(careerId, curriculumId, user);

        public async Task<List<StudentFilterTemplate>> GetStudentFilterTemplatesByStatus(List<int> status)
            => await _studentRepository.GetStudentFilterTemplatesByStatus(status);

        public async Task<List<WithdrawnStudentTemplate>> GetWithdrawnStudentsTemplate(Guid? termId)
            => await _studentRepository.GetWithdrawnStudentsTemplate(termId);

        public async Task<List<UserLoginStudentTemplate>> GetUserLoginStudentsTemplate(byte system, byte roleType, string startDate = null, string endDate = null)
            => await _studentRepository.GetUserLoginStudentsTemplate(system, roleType, startDate, endDate);

        public async Task<StudentComprobantInscriptionTemplate> GetStudentComprobantInscriptionTemplate(Guid studentId)
            => await _studentRepository.GetStudentComprobantInscriptionTemplate(studentId);

        public async Task<List<StudentComprobantInscriptionTemplate>> GetStudentsComprobantInscriptionsTemplateByFilters(Guid termId, Guid? careerId, Guid? facultyId)
            => await _studentRepository.GetStudentsComprobantInscriptionsTemplateByFilters(termId, careerId, facultyId);

        public Task<StudentDataTemplate> GetStudentDataByUserId(string userId)
            => _studentRepository.GetStudentDataByUserId(userId);

        public Task<DataTablesStructs.ReturnedData<object>> GetCurrentEnrolledStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId)
            => _studentRepository.GetCurrentEnrolledStudentDatatable(sentParameters, termId);

        public async Task<object> GetAverageGradesByStudentReport(Guid studentId)
            => await _studentRepository.GetAverageGradesByStudentReport(studentId);

        public async Task<StudentProcedureResult> StudentExoneratedCourseRequest(ClaimsPrincipal user, Guid studentId, Guid courseId)
            => await _studentRepository.StudentExoneratedCourseRequest(user, studentId, courseId);

        public async Task<StudentProcedureResult> StudentExtraordinaryEvaluationRequest(ClaimsPrincipal user, Guid studentId, Guid courseId)
            => await _studentRepository.StudentExtraordinaryEvaluationRequest(user, studentId, courseId);

        public Task<int> Count()
            => _studentRepository.Count();

        public async Task<StudentProcedureResult> ExecuteProcedureActivity(ClaimsPrincipal user, UserProcedure userProcedure, StudentUserProcedure studentUserProcedure)
            => await _studentRepository.ExecuteProcedureActivity(user, userProcedure, studentUserProcedure);

        public async Task<StudentProcedureResult> StudentGradeRecoveryRequest(ClaimsPrincipal user, Guid studentSectionId)
            => await _studentRepository.StudentGradeRecoveryRequest(user, studentSectionId);

        public async Task<StudentProcedureResult> StudentSubstituteExamRequest(ClaimsPrincipal user, Guid studentSectionId, string paymentReceipt)
            => await _studentRepository.StudentSubstituteExamRequest(user, studentSectionId, paymentReceipt);

        public async Task<List<EnrolledCourseTemplate>> GetEnrolledCoursesAvailableToSubstitueExam(Guid studentId)
            => await _studentRepository.GetEnrolledCoursesAvailableToSubstitueExam(studentId);

        public async Task<List<EnrolledCourseTemplate>> GetEnrolledCoursesToGradeRecovery(Guid studentId)
            => await _studentRepository.GetEnrolledCoursesToGradeRecovery(studentId);

        public async Task<List<EnrolledCourseTemplate>> GetCoursesAvailableForExoneratedCourse(Guid studentId)
            => await _studentRepository.GetCoursesAvailableForExoneratedCourse(studentId);

        public async Task<List<EnrolledCourseTemplate>> GetCoursesAvailableForExtraordinaryEvaluation(Guid studentId)
            => await _studentRepository.GetCoursesAvailableForExtraordinaryEvaluation(studentId);

        public async Task<int> GetNumberOfApprovedCourses(Guid studentId)
            => await _studentRepository.GetNumberOfApprovedCourses(studentId);

        public Task<object> SearchStudentForDegreeByTerm(string term, int? degreeType = null, Guid? careerId = null, ClaimsPrincipal user = null, bool onlyActiveStudents = false)
            => _studentRepository.SearchStudentForDegreeByTerm(term, degreeType, careerId, user, onlyActiveStudents);
        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrolledStudentPaymentStatusDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, Guid termId, Guid? facultyId = null, Guid? careerId = null, byte? type = null, int status = 1, string search = null)
            => await _studentRepository.GetEnrolledStudentPaymentStatusDatatable(sentParameters, User, termId, facultyId, careerId, type, status, search);

        public async Task<List<StudentPaymentStatus>> GetEnrolledStudentPaymentStatusData(ClaimsPrincipal User, Guid termId, Guid? facultyId = null, Guid? careerId = null, byte? type = null, int status = 1, string search = null)
            => await _studentRepository.GetEnrolledStudentPaymentStatusData(User, termId, facultyId, careerId, type, status, search);
        
        public async Task<List<StudentPaymentStatus>> GetPostPaymentEnrolledStudentData(ClaimsPrincipal User, Guid termId, Guid? facultyId = null, Guid? careerId = null, int? year = null, int status = 1, string search = null)
            => await _studentRepository.GetPostPaymentEnrolledStudentData(User, termId, facultyId, careerId, year, status, search);

        public async Task<StudentProcedureResult> StudentCourseWithdrawalMassiveRequest(ClaimsPrincipal user, List<Guid> studentSectionIds)
            => await _studentRepository.StudentCourseWithdrawalMassiveRequest(user, studentSectionIds);

        public async Task<object> GetSuitableStudentsReportDatatable(Guid? facultyId, Guid? careerId, Guid? programId, int? year, ClaimsPrincipal user = null)
            => await _studentRepository.GetSuitableStudentsReportDatatable(facultyId, careerId, programId, year, user);

        public async Task<object> GetSanctionedStudentsReportDatatable(Guid? facultyId, Guid? careerId, Guid? programId, int? year, ClaimsPrincipal user = null)
            => await _studentRepository.GetSanctionedStudentsReportDatatable(facultyId, careerId, programId, year, user);

        public Task<DataTablesStructs.ReturnedData<object>> GetHomeLoginsDatatable(DataTablesStructs.SentParameters sentParameters, byte? system = null)
            => _studentRepository.GetHomeLoginsDatatable(sentParameters, system);

        public Task<StudentConstancy> GetStudentConstancy(Guid studentId, Guid termId)
            => _studentRepository.GetStudentConstancy(studentId, termId);

        public Task<StudentConstancy> GetStudentLastConstancy(Guid studentId)
            => _studentRepository.GetStudentLastConstancy(studentId);

        public Task<object> GetJobExchangeStudentGraduatedQuantityChartData()
            => _studentRepository.GetJobExchangeStudentGraduatedQuantityChartData();

        public Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentGraduatedQuantityDatatable(DataTablesStructs.SentParameters sentParameters)
            => _studentRepository.GetJobExchangeStudentGraduatedQuantityDatatable(sentParameters);

        public Task<object> GetJobExchangeStudentGraduatedWorkingCareerQuantityChartData(List<int> studentStatus = null, List<Guid> careers = null)
            => _studentRepository.GetJobExchangeStudentGraduatedWorkingCareerQuantityChartData(studentStatus, careers);

        public Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentGraduatedWorkingCareerQuantityDatatable(DataTablesStructs.SentParameters sentParameters, List<int> studentStatus = null, List<Guid> careers = null)
            => _studentRepository.GetJobExchangeStudentGraduatedWorkingCareerQuantityDatatable(sentParameters, studentStatus, careers);

        public Task<object> GetJobExchangeStudentGraduatedWorkingCareerPercentageChartData()
            => _studentRepository.GetJobExchangeStudentGraduatedWorkingCareerPercentageChartData();

        public Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentGraduatedWorkingCareerPercentageDatatable(DataTablesStructs.SentParameters sentParameters)
            => _studentRepository.GetJobExchangeStudentGraduatedWorkingCareerPercentageDatatable(sentParameters);

        public Task<object> GetJobExchangeStudentGraduatedGraduationYearQuantityChartData(int? startYear = null, int? endYear = null)
            => _studentRepository.GetJobExchangeStudentGraduatedGraduationYearQuantityChartData(startYear, endYear);
        public Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentGraduatedGraduationYearQuantityDatatable(DataTablesStructs.SentParameters sentParameters, int? startYear = null, int? endYear = null)
            => _studentRepository.GetJobExchangeStudentGraduatedGraduationYearQuantityDatatable(sentParameters, startYear, endYear);
    }

}
