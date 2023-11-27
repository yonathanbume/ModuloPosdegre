using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Career;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Forum;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface ICareerRepository : IRepository<Career>
    {
        Task<List<ModelATemplate>> GetAllAsModelA(string search = null);
        Task<Select2Structs.ResponseParameters> GetCareerSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null, Guid? facultyId = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue);
        Task<IEnumerable<Career>> GetAllData(string search = null);
        Task<DataTablesStructs.ReturnedData<object>> GetEquivalenceDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, ClaimsPrincipal user = null);
        Task<object> GetCareerSelect2ClientSide(Guid? careerId = null, Guid? facultyId = null,bool hasAll = false, string coordinatorId = null, ClaimsPrincipal user = null);
        Task<Tuple<List<Career>, List<Faculty>>> GetCareerFacultiesIdByCoordinator(string academicCoordinatorId);
        Task<Select2Structs.ResponseParameters> GetCareerByUserIdSelect2(Select2Structs.RequestParameters requestParameters, string userId = null, string searchValue = null, Guid? selectedId = null);
        Task<Select2Structs.ResponseParameters> GetCareerByUserCoordinatorIdSelect2(Select2Structs.RequestParameters requestParameters, string userId = null, string searchValue = null, Guid? selectedId = null, Guid? facultyId = null);
        Task<IEnumerable<Select2Structs.Result>> GetCareerSelect2ByAcademicCoordinatorClientSide(string academicCoordinatorId, Guid? selectedId = null);
        Task<IEnumerable<Career>> GetCareerByUserCoordinatorId(string userId, string searchValue = null);
        Task<object> GetSelect2ByFaculty(Guid studentId);
        Task<object> GetCareersToForum();
        Task<List<ForumCareer>> GetForumCareer(ForumTemplate model);
        Task<object> GetCareerSelect2Curriculum(Guid fid, List<Guid> FacultyIds = null);
        Task<object> GetCareerAcademicSecretaryByCareerId(Guid careerId);
        Task<object> GetCareerAcademicCoordinatorByCareerId(Guid careerId);
        Task<object> GetCareerAcademicDepartmentDirectorByCareerId(Guid careerId);
        Task<object> GetCareerAcademicCareerDirectorByCareerId(Guid careerId);
        Task<object> GetAllAsSelect2ClientSide(Guid? facultyId = null, bool includeTitle = false, Guid? careerId = null, string coordinatorId = null, ClaimsPrincipal user = null);
        Task<Guid> GetIdByCoordinatorOrSecretary(string academicCoordinatorId, string academicSecretaryId);
        Task<List<Career>> GetAllByfacultyId(Guid? facultyId = null);
        Task<object> GetEnrollmentShiftDatatableClientSide();
        Task<Career> GetCareerReport(Guid careerId);
        Task<DataTablesStructs.ReturnedData<object>> GetCareersDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, ClaimsPrincipal user, string userId);
        Task<DataTablesStructs.ReturnedData<object>> GetCareersQualityCoordinatorDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetCareers2Datatable(DataTablesStructs.SentParameters sentParameters, Guid termId, ClaimsPrincipal user, string userId, Guid facultyId);
        Task<IEnumerable<Career>> GetAllWithData();
        Task<object> GetCareersJson(string q, ClaimsPrincipal user = null);
        Task<object> GetCareerAndAreaJson();
        Task<object> GetCareersByIdJson(Guid id);
        Task<object> GetCareersByFacultyIdJson(Guid id, bool hasAll = false, ClaimsPrincipal user = null);
        Task<object> GetCareersByFacultyIdWithAllJson(Guid id);
        Task<Career> GetNameByCellExcel(string cell);
        Task<List<Career>> GetCareerFacultyById(Guid id);
        Task<Guid> GetGuidFirst();
        Task<object> GetCareers();
        Task<object> GetAllCareerWithStudentQty();
        Task<object> GetAllCareerWithStudentQty(Guid id, int year);
        Task<DataTablesStructs.ReturnedData<object>> GetEnrolledStudentsByCareer(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, Guid? termId, Guid? campusId = null, bool onlyWithStudents = false);
        Task<IEnumerable<Career>> GettAllData2(string search = null);
        Task<object> GetCareerWithCountTutorAndTutorStudents();
        Task<List<HisotricTemplate>> GetHistory(Guid id);
        Task<List<Career>> GetCareersByClaim(ClaimsPrincipal user);
        Task<object> GetDetailReport(Guid termId, ClaimsPrincipal user);
        Task<List<Career>> GetList(Guid careerId);
        Task<bool> AnyByCode(string code, Guid? id = null);
        Task<bool> AnyAcademicDepartmentByCareerDirector(string userId);
        Task<DataTablesStructs.ReturnedData<object>> GetCareersDatatableByClaim(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, string searchValue, bool? withAcreditationsActive = null);
        Task<object> GetCarrerConditionSelect2(ClaimsPrincipal user);
        Task<DataTablesStructs.ReturnedData<object>> GetNumberOfApprovedStudentsDatatable(DataTablesStructs.SentParameters parameters, Guid? termId, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal? user = null);
        Task<List<Career>> GetFacultiesCareersByDean(string deanId);
        Task<List<Career>> GetFacultiesCareersBySecretary(string secretaryId);
        Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentGraduatedCareerQuantityReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, int? searchStartYear = null, int? searchEndYear = null);
        Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentBachelorQualifiedCareerQuantityReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null);
    }
}