using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Admission.Templates.Postulant;
using Microsoft.AspNetCore.Identity;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces
{
    public interface IPostulantRepository : IRepository<Postulant>
    {
        Task<IEnumerable<Postulant>> GetAllByFacultyAndCareerAndTerm(Guid? facultyId = null, Guid? careerId = null, Guid? termId = null, byte? admissionState = null);

        Task<IEnumerable<Postulant>> GetAllPostulantsByFilters(Guid? facultyId = null, Guid? careerId = null, Guid? applicationTermId = null, Guid? termId = null, byte? admissionState = null, int? secondaryEducationType = null, Guid? admissionTypeId = null, ClaimsPrincipal user = null, List<Guid> applicationTermsId = null);

        Task<IEnumerable<Postulant>> GetReportByFacultyAndCareerAndTerm(Guid? facultyId = null,
            Guid? careerId = null, Guid? termId = null);
        Task DeletePostulantById(Guid id);
        Task<Postulant> GetPostulantByDniAndTerm(string dni, Guid termId);
        Task<int> GetStudentCounttByTermAndCareer(Guid admissionTermId, Guid careerId, bool? accepted = null);
        Task<object> GetPostulantsAdmittedByCurrentAppTermId(Guid? currentApplicationTermId = null);
        Task<object> GetPostulantByAdmissionStateChart(Guid? termId = null, byte? admissionState = null, Guid? applicationTermId = null, Guid? careerId = null, Guid? admissionTypeId = null, ClaimsPrincipal user = null);
        Task<object> GetAcceptedPostulantByResultChart(Guid? termId = null, Guid ? applicationTermId = null, Guid? careerId = null, Guid? admissionTypeId = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentsDatatableAdmitted(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, Guid? careerId = null, Guid? typeId = null, Guid? currentApplicationTermId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetPostulantAdmittedDatatable(DataTablesStructs.SentParameters sentParameters, Guid? applicationTermId = null, int? proofOfIncomeStatus = null, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetPostulantByAdmissionStateDatatable(DataTablesStructs.SentParameters sentParameters,Guid? termId = null, byte? admissionState = null, Guid? applicationTermId = null, Guid? careerId = null, Guid? admissionTypeId = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAcceptedPostulantByResultDatatable(DataTablesStructs.SentParameters sentParameters,Guid? termId = null, Guid? applicationTermId = null, Guid? careerId = null, Guid? admissionTypeId = null, ClaimsPrincipal user = null);
        Task<List<Postulant>> GetAdmittedPostulantsByCurrentAppTermId(Guid currentApplicationTermId, bool studentNull = false);
        Task<List<Postulant>> GetAdmittedPostulantsByTermId(Guid termId);
        Task<List<Postulant>> GetAdmittedPostulantsByCurrentAppTermIdOrderByCareer(Guid currentApplicationTermId);

        Task<DataTablesStructs.ReturnedData<object>> GetPostulantDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, string search = null);
        Task<List<Postulant>> GetPostulantIncludeApplication(Term term, Guid careerId);
        Task<List<Postulant>> GetPostulantsDetail(Guid termId, Guid careerId);
        Task<Postulant> GetPostulantWithAdmission(string dni, Guid currentApplicationTermId);
        Task<IEnumerable<Postulant>> GetAllPostulantsWithDataInclude(Guid careerId, Guid applicationTernId);
        Task<Postulant> GetWithData(Guid id);
        Task<object> GetPostulantAcceptedPieChartByApplicationTerm(Guid? applicationTermId = null, ClaimsPrincipal user = null);
        Task<Postulant> GetWithStudentData(Guid id);

        Task<object> GetPostulantAcceptedDatatable(DataTablesStructs.SentParameters sentParameters, Guid? applicationTermId = null, ClaimsPrincipal user = null);

        Task<DataTablesStructs.ReturnedData<object>> GetRegularAdmissionDatatableClientSide(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId, string search);
        Task<object> GetIrregularAdmissionDatatableClientSide(Guid applicationTermId, Guid? careerId = null, Guid? typeId = null);
        Task<bool> GetPostulantExist(Guid currentApplicationTermId, string cell, Guid admissionTypeId, Guid careerId);
        Task<bool> GetPostulantExistAd(Guid currentApplicationTermId, string cell, Guid admissionTypeId);
        Task<List<Postulant>> GetCareerPostulantRegular(Guid careerId, Guid applicationTernId);
        Task<DataTablesStructs.ReturnedData<object>> GetPostulantResultsDatatable(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId, Guid? careerId = null, string search = null);
        Task<object> GetPostulantResultByDni(Guid? currentApplicationTermId = null, string dni = null);
        Task<string> GetUserWithCodeExist(string userCodePrefix, Guid? applicationTermId = null);
        Task<List<PostulantResultTemplate>> GetPostulantResultsByDni(string dni, Guid? currentApplicationTermId = null);
        Task<List<Postulant>> GetPostulantApplicationTermId(Guid applicationTermId);
        Task<List<PostulantReportTemplate>> GetTopScoresPerCareer(Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, Guid? admissionTypeId = null, Guid? applicationTermId = null, byte? admissionState = null, ClaimsPrincipal user = null, List<Guid> applicationTermsIds = null);
        Task<bool> GetPostulantByDniId(Guid id, string dni);
        Task<object> GetApplicationTermsActiveByDni(string dni);
        Task<bool> GetPostulantByDniIdAdmission(Guid id, string dni);
        Task<object> GetPostulantDebts(Guid id, string dni);
        Task<decimal> GetCostPostulantById(Guid id);
        Task<object> GetPostulantsPayment(Guid id);
        Task<Postulant> GetPostulantWithAdmissionById(Guid id);

        Task<Tuple<bool, string>> GenerateStudents(UserManager<ApplicationUser> userManager,Guid currentApplicationTermId);
        Task<List<PostulantTemplate>> GetPostulantByAdmissionStateDataExcel(byte? admissionState, Guid? applicationTermId = null, Guid? academicProgramId = null, Guid? admissionTypeId = null);
        Task<List<Postulant>> GetPostulantAdmission(AdmissionExam admissionExam);

        Task<List<AdmittedStudentTemplate>> GetAdmittedStudentsExcel(Guid? facultyId = null, Guid? careerId = null, Guid? typeId = null, Guid? currentApplicationTermId = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<Postulant>> GetAdmittedApplicants(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, Guid? admissionTypeId = null, Guid? applicationTermId = null, ClaimsPrincipal user = null);
        Task<Postulant> GetByStudent(Guid studentId);
        Task SaveChanges();
        Task<object> GetTopFacultiesPostulants(Guid id);
        Task<int> CountByDateAndBroadcastMedium(DateTime startDate, int broadcastMedium);
        Task<int> CreatePostulants(int count);
        Task<bool> ExistPostulantOnApplicationTerm(string dni, Guid applicationTermId);
        Task<Tuple<int, int>> PostulantPaymentJob(Guid id);
        Task<object> GetAcceptedPostulantByResult(Guid? applicationTermId = null, Guid? academicProgramId = null, Guid? admissionTypeId = null);
        Task<bool> ValidationByVoucher(string voucher);
        Task<bool> ValidationByDNI(string document);
        
        Task<DataTablesStructs.ReturnedData<object>> GetNonOrdinaryAdmissionDatatable(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId, Guid? careerId = null, Guid? typeId = null, string search = null);
        Task<DataTablesStructs.ReturnedData<object>> GetPostulantsDatatable(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId, Guid? careerId = null, Guid? typeId = null, bool? hasDiscapacity = null, bool? hasPhoto = null, string search = null);
        Task<DataTablesStructs.ReturnedData<object>> GetPostulantsAdmittedDatatable(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId, Guid? careerId = null, Guid? typeId = null, string search = null);
        Task<DataTablesStructs.ReturnedData<object>> GetPostulantsByAppTermIdAndAdmissionExamIdDataTable(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId, Guid admissionExamId, string search = null);
        Task<List<PostulantAdmittedTemplate>> GetPostulantAdmittedData(Guid applicationTermId, Guid? careerId = null, Guid? typeId = null, string search = null);
        Task<Postulant> GetPostulantByCode(string postulantcode, Guid? applicationTermId = null);
        Task RecalculateFinalScore(Guid applicationTermId);
        Task<IEnumerable<Postulant>> GetPostulantsByFilter(Guid applicationTermId, bool? hasDiscapacity, bool? hasPhoto, string searchValue = null);

        Task<DataTablesStructs.ReturnedData<object>> GetCareersWithManualApprovalDatatable(Guid applicationTermId);
        Task<DataTablesStructs.ReturnedData<object>> GetPostulantsWithManualApprovalDatatable(Guid applicationTermId, Guid careerId, Guid admissionTypeId);
        Task<List<Postulant>> GetAllPostulantsWithFolders();
        Task<object> GetPostulantByAgeRangeChart(Guid? facultyId, Guid termId, Guid? applicationtermId, byte? admissionState = null, List<Guid> applicationTermsId = null);
        Task<bool> GetPostulantByDniIdAndActiveApplicationTerms(string dni);
        Task ProcessOrdinaryPostulantsResults(Guid applicationtermId);
        Task<DataTablesStructs.ReturnedData<object>> GetSiriesResult(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId, Guid termId, List<Guid> applicationTermsIds = null);

        Task<object> GetTotalNumberOfEntrantsOfTheAdmissionExamByCareerAsData(Guid? termId = null, Guid? careerId = null, Guid? admissionTypeId = null, Guid? academicProgramId = null, ClaimsPrincipal user = null);

        Task<object> GetTotalNumberOfPostulantsOfTheAdmissionExamByCareerAsData(Guid? termId = null, Guid? careerId = null, Guid? admissionTypeId = null, Guid? academicProgramId = null, ClaimsPrincipal user = null);
        Task<List<Postulant>> GetSiriesExcelResult(Guid applicationTermId, Guid termId);
        Task<Postulant> CreatePostulantFromStudent(Guid studentId);

        Task Insert(Postulant postulant, bool generateCode = false);
        Task<Postulant> GetPostulantByDocument(string document, Guid? applicationTermId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetPostulantPaymentDatatable(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId, string search = null);

        Task<int> GetPostulantTriesByDocument(string document);
    }
}