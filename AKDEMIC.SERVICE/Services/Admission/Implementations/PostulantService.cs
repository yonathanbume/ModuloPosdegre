using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Admission.Templates.Postulant;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class PostulantService : IPostulantService
    {
        private readonly IPostulantRepository _postulantRepository;

        public PostulantService(IPostulantRepository postulantRepository)
        {
            _postulantRepository = postulantRepository;
        }

        public async Task AddAsync(Postulant postulant)
            => await _postulantRepository.Add(postulant);

        public async Task AddRangeAsync(List<Postulant> postulants) => await _postulantRepository.AddRange(postulants);

        public async Task InsertPostulant(Postulant postulant) => await _postulantRepository.Insert(postulant);

        public async Task InsertPostulant(Postulant postulant, bool generateCode = false) => await _postulantRepository.Insert(postulant, generateCode);

        public async Task UpdatePostulant(Postulant postulant) =>
            await _postulantRepository.Update(postulant);

        public async Task DeletePostulant(Postulant postulant) =>
            await _postulantRepository.Delete(postulant);

        public async Task<Postulant> GetPostulantById(Guid id) =>
            await _postulantRepository.Get(id);

        public async Task<IEnumerable<Postulant>> GetAllPostulants() =>
            await _postulantRepository.GetAll();

        public async Task<IEnumerable<Postulant>> GetAllByFacultyAndCareerAndTerm(Guid? facultyId = null, Guid? careerId = null, Guid? termId = null, byte? admissionState = null)
            => await _postulantRepository.GetAllByFacultyAndCareerAndTerm(facultyId, careerId, termId, admissionState);

        public async Task<IEnumerable<Postulant>> GetReportByFacultyAndCareerAndTerm(Guid? facultyId = null,
            Guid? careerId = null, Guid? termId = null) =>
            await _postulantRepository.GetReportByFacultyAndCareerAndTerm(facultyId, careerId, termId);

        public async Task<Postulant> GetPostulantByDniAndTerm(string dni, Guid termId)
            => await _postulantRepository.GetPostulantByDniAndTerm(dni, termId);

        public async Task<int> GetStudentCounttByTermAndCareer(Guid admissionTermId, Guid careerId, bool? accepted = null)
            => await _postulantRepository.GetStudentCounttByTermAndCareer(admissionTermId, careerId, accepted);

        public async Task<object> GetPostulantsAdmittedByCurrentAppTermId(Guid? currentApplicationTermId = null)
            => await _postulantRepository.GetPostulantsAdmittedByCurrentAppTermId(currentApplicationTermId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsDatatableAdmitted(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, Guid? careerId = null, Guid? typeId = null, Guid? currentApplicationTermId = null)
            => await _postulantRepository.GetStudentsDatatableAdmitted(sentParameters, facultyId, careerId, typeId, currentApplicationTermId);

        public async Task<List<Postulant>> GetAdmittedPostulantsByCurrentAppTermId(Guid currentApplicationTermId, bool studentNull = false)
            => await _postulantRepository.GetAdmittedPostulantsByCurrentAppTermId(currentApplicationTermId, studentNull);
        public async Task<List<Postulant>> GetAdmittedPostulantsByCurrentAppTermIdOrderByCareer(Guid currentApplicationTermId)
            => await _postulantRepository.GetAdmittedPostulantsByCurrentAppTermIdOrderByCareer(currentApplicationTermId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantByAdmissionStateDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, byte? admissionState = null, Guid? applicationTermId = null, Guid? careerId = null, Guid? admissionTypeId = null, ClaimsPrincipal user = null)
        {
            return await _postulantRepository.GetPostulantByAdmissionStateDatatable(sentParameters, termId, admissionState, applicationTermId, careerId, admissionTypeId, user);
        }

        public async Task<object> GetPostulantByAdmissionStateChart(Guid? termId = null, byte? admissionState = null, Guid? applicationTermId = null, Guid? careerId = null, Guid? admissionTypeId = null, ClaimsPrincipal user = null)
        {
            return await _postulantRepository.GetPostulantByAdmissionStateChart(termId, admissionState, applicationTermId, careerId, admissionTypeId, user);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, string search = null)
            => await _postulantRepository.GetPostulantDatatable(sentParameters, termId, facultyId, careerId, search);

        public async Task<List<Postulant>> GetPostulantIncludeApplication(Term term, Guid careerId)
            => await _postulantRepository.GetPostulantIncludeApplication(term, careerId);

        public async Task<List<Postulant>> GetPostulantsDetail(Guid termId, Guid careerId)
            => await _postulantRepository.GetPostulantsDetail(termId, careerId);

        public async Task<Postulant> GetPostulantWithAdmission(string dni, Guid currentApplicationTermId)
            => await _postulantRepository.GetPostulantWithAdmission(dni, currentApplicationTermId);

        public async Task<object> GetPostulantAcceptedDatatable(DataTablesStructs.SentParameters sentParameters, Guid? applicationTermId = null , ClaimsPrincipal user = null)
            => await _postulantRepository.GetPostulantAcceptedDatatable(sentParameters, applicationTermId, user);

        public async Task<IEnumerable<Postulant>> GetAllPostulantsWithDataInclude(Guid careerId, Guid applicationTernId)
            => await _postulantRepository.GetAllPostulantsWithDataInclude(careerId, applicationTernId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetAcceptedPostulantByResultDatatable(DataTablesStructs.SentParameters sentParameters,Guid? termId = null, Guid? applicationTermId = null, Guid? careerId = null, Guid? admissionTypeId = null, ClaimsPrincipal user = null)
        {
            return await _postulantRepository.GetAcceptedPostulantByResultDatatable(sentParameters, termId, applicationTermId, careerId, admissionTypeId, user);
        }

        public async Task<object> GetAcceptedPostulantByResultChart(Guid? termId = null, Guid ? applicationTermId = null, Guid? careerId = null, Guid? admissionTypeId = null, ClaimsPrincipal user = null)
        {
            return await _postulantRepository.GetAcceptedPostulantByResultChart(termId, applicationTermId, careerId, admissionTypeId, user);
        }

        public async Task<Postulant> GetWithData(Guid id) => await _postulantRepository.GetWithData(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetRegularAdmissionDatatableClientSide(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId, string search)
        {
            return await _postulantRepository.GetRegularAdmissionDatatableClientSide(sentParameters, applicationTermId, search);
        }

        public async Task<object> GetIrregularAdmissionDatatableClientSide(Guid applicationTermId, Guid? careerId = null, Guid? typeId = null)
            => await _postulantRepository.GetIrregularAdmissionDatatableClientSide(applicationTermId, careerId, typeId);

        public async Task<Postulant> GetWithStudentData(Guid id) => await _postulantRepository.GetWithStudentData(id);

        public async Task<object> GetPostulantAcceptedPieChartByApplicationTerm(Guid? applicationTermId = null, ClaimsPrincipal user = null)
            => await _postulantRepository.GetPostulantAcceptedPieChartByApplicationTerm(applicationTermId, user);

        public async Task<bool> GetPostulantExist(Guid currentApplicationTermId, string cell, Guid admissionTypeId, Guid careerId)
            => await _postulantRepository.GetPostulantExist(currentApplicationTermId, cell, admissionTypeId, careerId);

        public async Task<bool> GetPostulantExistAd(Guid currentApplicationTermId, string cell, Guid admissionTypeId)
            => await _postulantRepository.GetPostulantExistAd(currentApplicationTermId, cell, admissionTypeId);
        public async Task<List<Postulant>> GetCareerPostulantRegular(Guid careerId, Guid applicationTernId)
            => await _postulantRepository.GetCareerPostulantRegular(careerId, applicationTernId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantResultsDatatable(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId, Guid? careerId = null, string search = null)
            => await _postulantRepository.GetPostulantResultsDatatable(sentParameters, applicationTermId, careerId, search);

        public async Task<object> GetPostulantResultByDni(Guid? currentApplicationTermId = null, string dni = null)
            => await _postulantRepository.GetPostulantResultByDni(currentApplicationTermId, dni);

        public async Task<string> GetUserWithCodeExist(string userCodePrefix, Guid? applicationTermId = null)
            => await _postulantRepository.GetUserWithCodeExist(userCodePrefix, applicationTermId);
        public async Task<List<Postulant>> GetPostulantApplicationTermId(Guid applicationTermId)
            => await _postulantRepository.GetPostulantApplicationTermId(applicationTermId);

        public async Task<List<PostulantReportTemplate>> GetTopScoresPerCareer(Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, Guid? admissionTypeId = null, Guid? applicationTermId = null, byte? admissionState = null, ClaimsPrincipal user = null, List<Guid> applicationTermsIds = null)
            => await _postulantRepository.GetTopScoresPerCareer(termId,facultyId, careerId, admissionTypeId, applicationTermId,admissionState, user, applicationTermsIds);

        public async Task<IEnumerable<Postulant>> GetAllPostulantsByFilters(Guid? facultyId = null, Guid? careerId = null, Guid? applicationTermId = null, Guid? termId = null, byte? admissionState = null, int? secondaryEducationType = null, Guid? admissionTypeId = null, ClaimsPrincipal user = null, List<Guid> applicationTermsId = null)
            => await _postulantRepository.GetAllPostulantsByFilters(facultyId, careerId, applicationTermId, termId, admissionState,secondaryEducationType, admissionTypeId, user, applicationTermsId);
        public async Task<bool> GetPostulantByDniId(Guid id, string dni)
            => await _postulantRepository.GetPostulantByDniId(id, dni);
        public async Task<bool> GetPostulantByDniIdAdmission(Guid id, string dni)
            => await _postulantRepository.GetPostulantByDniIdAdmission(id, dni);
        public async Task<object> GetPostulantDebts(Guid id, string dni)
            => await _postulantRepository.GetPostulantDebts(id, dni);
        public async Task<decimal> GetCostPostulantById(Guid id)
            => await _postulantRepository.GetCostPostulantById(id);
        public async Task<object> GetPostulantsPayment(Guid id)
            => await _postulantRepository.GetPostulantsPayment(id);
        public async Task<Postulant> GetPostulantWithAdmissionById(Guid id)
            => await _postulantRepository.GetPostulantWithAdmissionById(id);

        public async Task<Tuple<bool, string>> GenerateStudents(UserManager<ApplicationUser> userManager, Guid currentApplicationTermId)
         => await _postulantRepository.GenerateStudents(userManager, currentApplicationTermId);

        public async Task<List<PostulantTemplate>> GetPostulantByAdmissionStateDataExcel(byte? admissionState, Guid? applicationTermId = null, Guid? academicProgramId = null, Guid? admissionTypeId = null)
            => await _postulantRepository.GetPostulantByAdmissionStateDataExcel(admissionState, applicationTermId, academicProgramId, admissionTypeId);
        public async Task<List<Postulant>> GetPostulantAdmission(AdmissionExam admissionExam)
            => await _postulantRepository.GetPostulantAdmission(admissionExam);

        public async Task<List<AdmittedStudentTemplate>> GetAdmittedStudentsExcel(Guid? facultyId = null, Guid? careerId = null, Guid? typeId = null, Guid? currentApplicationTermId = null, ClaimsPrincipal user = null)
            => await _postulantRepository.GetAdmittedStudentsExcel(facultyId, careerId, typeId, currentApplicationTermId, user);
        public async Task<DataTablesStructs.ReturnedData<Postulant>> GetAdmittedApplicants(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, Guid? admissionTypeId = null, Guid? applicationTermId = null, ClaimsPrincipal user = null)
            => await _postulantRepository.GetAdmittedApplicants(sentParameters, careerId, admissionTypeId, applicationTermId, user);
        public async Task<Postulant> GetByStudent(Guid studentId)
            => await _postulantRepository.GetByStudent(studentId);

        public async Task DeletePostulantById(Guid id)
            => await _postulantRepository.DeletePostulantById(id);

        public async Task SaveChanges()
        {
            await _postulantRepository.SaveChanges();
        }

        public async Task<object> GetTopFacultiesPostulants(Guid id)
        {
            return await _postulantRepository.GetTopFacultiesPostulants(id);
        }

        public async Task<int> CountByDateAndBroadcastMedium(DateTime startDate, int broadcastMedium)
        {
            return await _postulantRepository.CountByDateAndBroadcastMedium(startDate, broadcastMedium);
        }

        public async Task<int> CreatePostulants(int count)
        {
            return await _postulantRepository.CreatePostulants(count);
        }

        public async Task<Tuple<int, int>> PostulantPaymentJob(Guid id)
        {
            return await _postulantRepository.PostulantPaymentJob(id);
        }

        public async Task<object> GetAcceptedPostulantByResult(Guid? applicationTermId = null, Guid? academicProgramId = null, Guid? admissionTypeId = null)
        {
            return await _postulantRepository.GetAcceptedPostulantByResult(applicationTermId, academicProgramId, admissionTypeId);
        }

        public async Task<bool> ValidationByVoucher(string voucher)
        {
            return await _postulantRepository.ValidationByVoucher(voucher);
        }

        public async Task<bool> ValidationByDNI(string document)
        {
            return await _postulantRepository.ValidationByDNI(document);
        }

        public Task<DataTablesStructs.ReturnedData<object>> GetNonOrdinaryAdmissionDatatable(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId, Guid? careerId = null, Guid? typeId = null, string search = null)
            => _postulantRepository.GetNonOrdinaryAdmissionDatatable(sentParameters, applicationTermId, careerId, typeId, search);

        public Task<DataTablesStructs.ReturnedData<object>> GetPostulantsDatatable(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId, Guid? careerId = null, Guid? typeId = null, bool? hasDiscapacity = null, bool? hasPhoto = null, string search = null)
            => _postulantRepository.GetPostulantsDatatable(sentParameters, applicationTermId, careerId, typeId, hasDiscapacity, hasPhoto, search);

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantsAdmittedDatatable(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId, Guid? careerId = null, Guid? typeId = null, string search = null)
        {
            return await _postulantRepository.GetPostulantsAdmittedDatatable(sentParameters,applicationTermId,careerId,typeId,search);
        }
        public async Task<List<PostulantResultTemplate>> GetPostulantResultsByDni(string dni, Guid? currentApplicationTermId = null)
        {
            return await _postulantRepository.GetPostulantResultsByDni(dni, currentApplicationTermId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantsByAppTermIdAndAdmissionExamIdDataTable(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId, Guid admissionExamId, string search = null)
        {
            return await _postulantRepository.GetPostulantsByAppTermIdAndAdmissionExamIdDataTable(sentParameters, applicationTermId, admissionExamId, search);
        }
        public async Task<List<PostulantAdmittedTemplate>> GetPostulantAdmittedData(Guid applicationTermId, Guid? careerId = null, Guid? typeId = null, string search = null)
        {
            return await _postulantRepository.GetPostulantAdmittedData(applicationTermId, careerId, typeId, search);
        }
        public async Task<Postulant> GetPostulantByCode(string postulantcode, Guid? applicationTermId = null)
        {
            return await _postulantRepository.GetPostulantByCode(postulantcode, applicationTermId);
        }

        public async Task RecalculateFinalScore(Guid applicationTermId) => await _postulantRepository.RecalculateFinalScore(applicationTermId);

        public async Task<IEnumerable<Postulant>> GetPostulantsByFilter(Guid applicationTermId, bool? hasDiscapacity, bool? hasPhoto, string searchValue = null)
            => await _postulantRepository.GetPostulantsByFilter(applicationTermId, hasDiscapacity, hasPhoto, searchValue);

        public async Task<DataTablesStructs.ReturnedData<object>> GetCareersWithManualApprovalDatatable(Guid applicationTermId)
            => await _postulantRepository.GetCareersWithManualApprovalDatatable(applicationTermId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantsWithManualApprovalDatatable(Guid applicationTermId, Guid careerId, Guid admissionTypeId)
            => await _postulantRepository.GetPostulantsWithManualApprovalDatatable(applicationTermId, careerId, admissionTypeId);

        public async Task<List<Postulant>> GetAllPostulantsWithFolders()
        {
            return await _postulantRepository.GetAllPostulantsWithFolders();
        }

        public async Task<object> GetPostulantByAgeRangeChart(Guid? facultyId, Guid termId, Guid? applicationtermId, byte? admissionState = null, List<Guid> applicationTermsId = null)
        {
            return await _postulantRepository.GetPostulantByAgeRangeChart(facultyId, termId, applicationtermId,admissionState, applicationTermsId);
        }

        public async Task<bool> GetPostulantByDniIdAndActiveApplicationTerms(string dni)
        {
            return await _postulantRepository.GetPostulantByDniIdAndActiveApplicationTerms(dni);
        }

        public async Task ProcessOrdinaryPostulantsResults(Guid applicationtermId)
            => await _postulantRepository.ProcessOrdinaryPostulantsResults(applicationtermId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetSiriesResult(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId, Guid termId, List<Guid> applicationTermsIds = null)
        {
            return await _postulantRepository.GetSiriesResult(sentParameters, applicationTermId, termId, applicationTermsIds);
        }

        public async Task<List<Postulant>> GetSiriesExcelResult(Guid applicationTermId, Guid termId)
        {
            return await _postulantRepository.GetSiriesExcelResult(applicationTermId, termId);
        }

        public async Task<object> GetTotalNumberOfEntrantsOfTheAdmissionExamByCareerAsData(Guid? termId = null, Guid? careerId = null, Guid? admissionTypeId = null, Guid? academicProgramId = null, ClaimsPrincipal user = null)
            => await _postulantRepository.GetTotalNumberOfEntrantsOfTheAdmissionExamByCareerAsData(termId, careerId, admissionTypeId, academicProgramId, user);

        public async Task<object> GetTotalNumberOfPostulantsOfTheAdmissionExamByCareerAsData(Guid? termId = null, Guid? careerId = null, Guid? admissionTypeId = null, Guid? academicProgramId = null, ClaimsPrincipal user = null)
            => await _postulantRepository.GetTotalNumberOfPostulantsOfTheAdmissionExamByCareerAsData(termId, careerId, admissionTypeId, academicProgramId, user);

        public async Task<Postulant> CreatePostulantFromStudent(Guid studentId)
            => await _postulantRepository.CreatePostulantFromStudent(studentId);

        public async Task<Postulant> GetPostulantByDocument(string document, Guid? applicationTermId = null)
            => await _postulantRepository.GetPostulantByDocument(document, applicationTermId);

        public Task<object> GetApplicationTermsActiveByDni(string dni)
            => _postulantRepository.GetApplicationTermsActiveByDni(dni);

        public Task<bool> ExistPostulantOnApplicationTerm(string dni, Guid applicationTermId)
            => _postulantRepository.ExistPostulantOnApplicationTerm(dni, applicationTermId);

        public async Task UpdateRange(IEnumerable<Postulant> postulants)
            => await _postulantRepository.UpdateRange(postulants);

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantPaymentDatatable(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId, string search = null)
            => await _postulantRepository.GetPostulantPaymentDatatable(sentParameters, applicationTermId, search);

        public Task<DataTablesStructs.ReturnedData<object>> GetPostulantAdmittedDatatable(DataTablesStructs.SentParameters sentParameters, Guid? applicationTermId = null, int? proofOfIncomeStatus = null, string searchValue = null)
            => _postulantRepository.GetPostulantAdmittedDatatable(sentParameters, applicationTermId, proofOfIncomeStatus, searchValue);

        public Task<Postulant> Get(Guid id)
            => _postulantRepository.Get(id);
        public async Task<List<Postulant>> GetAdmittedPostulantsByTermId(Guid termId)
            => await _postulantRepository.GetAdmittedPostulantsByTermId(termId);

        public async Task<int> GetPostulantTriesByDocument(string document)
            => await _postulantRepository.GetPostulantTriesByDocument(document);

    }
}