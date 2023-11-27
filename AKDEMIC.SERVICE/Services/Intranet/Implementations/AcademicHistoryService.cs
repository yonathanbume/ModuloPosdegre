using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AcademicHistory;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class AcademicHistoryService : IAcademicHistoryService
    {
        private readonly IAcademicHistoryRepository _academicHistoryRepository;

        public async Task<AcademicHistory> GetWithData(Guid id)
            => await _academicHistoryRepository.GetWithData(id);
        public AcademicHistoryService(IAcademicHistoryRepository academicHistoryRepository)
        {
            _academicHistoryRepository = academicHistoryRepository;
        }

        public async Task<int> CountFailedAcademicHistoriesByStudent(Guid studentId)
        {
            return await _academicHistoryRepository.CountFailedAcademicHistoriesByStudent(studentId);
        }

        public async Task<IEnumerable<AcademicHistory>> GetAll()
            => await _academicHistoryRepository.GetAll();

        public async Task<AcademicHistory> Get(Guid id)
        {
            return await _academicHistoryRepository.Get(id);
        }

        public async Task<IEnumerable<AcademicHistory>> GetAcademicHistoriesByStudent(Guid studentId, Guid? termId = null, bool? validated = null, bool? approved = null)
        {
            return await _academicHistoryRepository.GetAcademicHistoriesByStudent(studentId, termId, validated, approved);
        }

        public async Task<IEnumerable<AcademicHistoryReportTemplate>> GetAcademicHistoriesReportAsData(Guid careerId,Guid curriculumId,Guid courseId, Guid termId)
        {
            return await _academicHistoryRepository.GetAcademicHistoriesReportAsData(careerId,curriculumId,courseId, termId);
        }

        public async Task<DataTablesStructs.ReturnedData<AcademicHistoryTemplate>> GetAcademicHistoryDatatable(DataTablesStructs.SentParameters parameters, Guid careerId,Guid curriculumid,Guid courseId, Guid termId, string name)
        {
            return await _academicHistoryRepository.GetAcademicHistoryDatatable(parameters,careerId, curriculumid, courseId, termId, name);
        }

        public async Task<AcademicHistory> GetLastByStudentAndCourse(Guid studentId, Guid courseId)
            => await _academicHistoryRepository.GetLastByStudentAndCourse(studentId, courseId);

        public async Task<IEnumerable<CurriculumCourseTemplate>> GetCurriculumCoursesByStudentId(Guid studentId, bool electiveCourses)
            => await _academicHistoryRepository.GetCurriculumCoursesByStudentId(studentId, electiveCourses);

        public async Task InsertRange(IEnumerable<AcademicHistory> academicHistories)
        {
            await _academicHistoryRepository.InsertRange(academicHistories);
        }

        public async Task Insert(AcademicHistory academicHistory)
            => await _academicHistoryRepository.Insert(academicHistory);

        public async Task<IEnumerable<AcademicHistory>> GetAllWithSectionAndTeacherSections(Guid? studentId = null, Guid? termId = null, bool? validated = null, bool? approved = null)
            => await _academicHistoryRepository.GetAllWithSectionAndTeacherSections(studentId, termId, validated, approved);

        public async Task<List<CertificateTemplate>> GetListCertificateByStudentAndCurriculum(Guid studentId, Guid curriculumId)
            => await _academicHistoryRepository.GetListCertificateByStudentAndCurriculum(studentId, curriculumId);

        public async Task<int> GetCoursesDisapprovedByStudentId(Guid studentId)
            => await _academicHistoryRepository.GetCoursesDisapprovedByStudentId(studentId);

        public async Task<int> GetCoursesRecoveredByStudentId(Guid studentId)
            => await _academicHistoryRepository.GetCoursesRecoveredByStudentId(studentId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsFourth(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, string userId, Guid? faculty = null, Guid? career = null, string search = null)
            => await _academicHistoryRepository.GetStudentsFourth(sentParameters, user, userId, faculty, career, search);

        public async Task<List<FourthExcelTemplate>> GetStudentToExcelFourth(Guid? faculty = null, Guid? career = null, ClaimsPrincipal user = null)
            => await _academicHistoryRepository.GetStudentToExcelFourth(faculty, career, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsThirdDatatable(DataTablesStructs.SentParameters sentParameters, Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null)
            => await _academicHistoryRepository.GetStudentsThirdDatatable(sentParameters, faculty, career, search, user);

        public async Task<List<ThridExcelTemplate>> GetStudentToExcelThird(Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null)
                => await _academicHistoryRepository.GetStudentToExcelThird(faculty, career, search, user);

        public async Task<List<AcademicHistory>> GetByStudentId(Guid studentId)
            => await _academicHistoryRepository.GetByStudentId(studentId);

        public Task<DataTablesStructs.ReturnedData<object>> GetExtraordinaryEvaluationHistoryDatatable(DataTablesStructs.SentParameters sentParameters, Guid? term = null, Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null)
            => _academicHistoryRepository.GetExtraordinaryEvaluationHistoryDatatable(sentParameters, term, faculty, career, search, user);

        public async Task<AcademicHistory> GetApprovedsNyCourseIdAndStudentId(Guid courseId, Guid studentid)
        {
            return await _academicHistoryRepository.GetApprovedsNyCourseIdAndStudentId(courseId, studentid);
        }

        public async Task<bool> AnyDisaprovedAcademicHistories(Guid courseId, Guid studentId)
        {
            return await _academicHistoryRepository.AnyDisaprovedAcademicHistories(courseId, studentId);
        }

        public async Task<byte> GetLowestAcademicYear(Guid studentId, Guid curriculumId)
        {
            return await _academicHistoryRepository.GetLowestAcademicYear(studentId, curriculumId);
        }

        public async Task<int> GetDissaprovedCount(Guid studentId)
        {
            return await _academicHistoryRepository.GetDissaprovedCount(studentId);
        }

        public async Task<string> GetTimesByCourseAndUserId(Guid courseId, string userId)
        {
            return await _academicHistoryRepository.GetTimesByCourseAndUserId(courseId, userId);
        }
        public async Task<List<AcademicHistory>> GetApprovedByStudentId(Guid studentId)
        {
            return await _academicHistoryRepository.GetApprovedByStudentId(studentId);
        }

        public async Task AddRange(List<AcademicHistory> newAcademicHistories)
        {
            await _academicHistoryRepository.AddRange(newAcademicHistories);
        }

        public async Task UpdateAcademicHistoriesJob(string connectionString)
        {
            await _academicHistoryRepository.UpdateAcademicHistoriesJob(connectionString);
        }

        public async Task UpdateAcademicHistoryTryJob(string code)
        {
            await _academicHistoryRepository.UpdateAcademicHistoryTryJob(code);
        }

        public async Task StudentsCurriculumConvalidationJob(Guid careerId)
        {
            await _academicHistoryRepository.StudentsCurriculumConvalidationJob(careerId);
        }

        public async Task StudentsCurriculumConvalidationJob(Guid studentId, Guid prevCurriculumId)
        {
            await _academicHistoryRepository.StudentsCurriculumConvalidationJob(studentId, prevCurriculumId);
        }

        public async Task Update(AcademicHistory academichistory)
        {
            await _academicHistoryRepository.Update(academichistory);
        }

        public async Task<AcademicHistory> GetByStudentIdAndCourseId(Guid studentId, Guid courseId)
        {
            return await _academicHistoryRepository.GetByStudentIdAndCourseId(studentId, courseId);
        }

        public async Task Delete(AcademicHistory academichistory)
        {
            await _academicHistoryRepository.Delete(academichistory);
        }

        public void RemoveRange(IEnumerable<AcademicHistory> academicHistories) => _academicHistoryRepository.RemoveRange(academicHistories);

        public async Task<List<AcademicHistory>> GetAcademicHistoryValidated(Guid studentId, Guid termId)
            => await _academicHistoryRepository.GetAcademicHistoryValidated(studentId, termId);

        public Task<bool> IsTryHigherThan(Guid studentId, int number)
            => _academicHistoryRepository.IsTryHigherThan(studentId,number);

        public async Task<AcademicHistory> GetAcademicHistoryByStudentAndSectionId(Guid studentId, Guid sectionId)
            => await _academicHistoryRepository.GetAcademicHistoryByStudentAndSectionId(studentId, sectionId);
        public async Task<Select2Structs.ResponseParameters> GetTAcademicHistoryByCourseAndTerm(Select2Structs.RequestParameters requestParameters, string searchValue = null, Guid? courseId = null, Guid? termId = null)
            => await _academicHistoryRepository.GetTAcademicHistoryByCourseAndTerm(requestParameters, searchValue, courseId, termId);

        public async Task<AcademicHistory> GetAcademicHistoryBystudentAndCourseId(Guid studentId, Guid courseId, Guid termId)
            => await _academicHistoryRepository.GetAcademicHistoryBystudentAndCourseId(studentId, courseId, termId);

        public async Task<List<AcademicHistoryYearTemplate>> GetAcademicHistoriesByYearRange(int startYear, int endYear)
            => await _academicHistoryRepository.GetAcademicHistoriesByYearRange(startYear, endYear);

        public async Task<List<TermDisapprovedGradeTemplate>> GetDisapprovedAcademicHistoriesByYearRange(int startYear, int endYear)
            => await _academicHistoryRepository.GetDisapprovedAcademicHistoriesByYearRange(startYear, endYear);

        public async Task<List<AcademicHistoryTemplate>> GetAcademicHistoryTemplate(Guid careerId, Guid curriculumId, Guid courseId, Guid termId)
            => await _academicHistoryRepository.GetAcademicHistoryTemplate(careerId, curriculumId, courseId, termId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetCourseRecognitionReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid term, Guid? faculty = null, Guid? career = null, string search = null)
            => await _academicHistoryRepository.GetCourseRecognitionReportDatatable(sentParameters, term, faculty, career, search);
        
        public async Task<List<StudentHistoryTemplate>> GetCourseRecognitionReportData(Guid term, Guid? faculty = null, Guid? career = null, string search = null)
            => await _academicHistoryRepository.GetCourseRecognitionReportData(term, faculty, career, search);

        public async Task<object> GetDisapprovedCoursesByStudentDataDatatable(Guid termid, Guid? facultyId = null, Guid? careerId = null, int? disapprovedCourses = null)
            => await _academicHistoryRepository.GetDisapprovedCoursesByStudentDataDatatable(termid, facultyId, careerId, disapprovedCourses);

        public async Task<List<DisapprovedCoursesByStudentTemplate>> GetDisapprovedCoursesByStudentData(Guid termid, Guid? facultyId = null, Guid? careerId = null, int? disapprovedCourses = null)
            => await _academicHistoryRepository.GetDisapprovedCoursesByStudentData(termid, facultyId, careerId, disapprovedCourses);
        
        public async Task<List<CourseEquivalenceTemplate>> GetCourseEquivalenceDataByStudent(Guid studentId, Guid? termId = null)
            => await _academicHistoryRepository.GetCourseEquivalenceDataByStudent(studentId, termId);
    }
}
