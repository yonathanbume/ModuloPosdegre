using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Career;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Forum;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public class CareerService : ICareerService
    {
        private readonly ICareerRepository _careerRepository;

        public CareerService(ICareerRepository careerRepository)
        {
            _careerRepository = careerRepository;
        }

        public async Task Delete(Career career) => await _careerRepository.Delete(career);

        public async Task DeleteById(Guid id) => await _careerRepository.DeleteById(id);

        public async Task<Career> Get(Guid id) => await _careerRepository.Get(id);

        public async Task<IEnumerable<Career>> GetAll() => await _careerRepository.GetAll();
        public async Task<IEnumerable<Career>> GetAllData(string search = null) => await _careerRepository.GetAllData(search);

        public async Task<Select2Structs.ResponseParameters> GetCareerSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null, Guid? facultyId = null, ClaimsPrincipal user = null)
            => await _careerRepository.GetCareerSelect2(requestParameters, searchValue, facultyId, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
            => await _careerRepository.GetDataDatatable(sentParameters, searchValue);

        public async Task Insert(Career area) => await _careerRepository.Insert(area);

        public async Task Update(Career area) => await _careerRepository.Update(area);

        public async Task<object> GetCareerSelect2ClientSide(Guid? careerId = null, Guid? facultyId = null, bool hasAll = false, string coordinatorId = null, ClaimsPrincipal user= null)
            => await _careerRepository.GetCareerSelect2ClientSide(careerId, facultyId, hasAll, coordinatorId,user);
        public async Task<DataTablesStructs.ReturnedData<object>> GetEquivalenceDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, ClaimsPrincipal user = null)
            => await _careerRepository.GetEquivalenceDataDatatable(sentParameters, searchValue, user);

        public async Task<Tuple<List<Career>, List<Faculty>>> GetCareerFacultiesIdByCoordinator(string academicCoordinatorId)
            => await _careerRepository.GetCareerFacultiesIdByCoordinator(academicCoordinatorId);

        public async Task<Select2Structs.ResponseParameters> GetCareerByUserIdSelect2(Select2Structs.RequestParameters requestParameters, string userId = null, string searchValue = null, Guid? selectedId = null)
            => await _careerRepository.GetCareerByUserIdSelect2(requestParameters, userId, searchValue, selectedId);

        public async Task<Select2Structs.ResponseParameters> GetCareerByUserCoordinatorIdSelect2(Select2Structs.RequestParameters requestParameters, string userId = null, string searchValue = null, Guid? selectedId = null, Guid? facultyId = null)
        => await _careerRepository.GetCareerByUserCoordinatorIdSelect2(requestParameters, userId, searchValue, selectedId, facultyId);

        public async Task<IEnumerable<Career>> GetCareerByUserCoordinatorId(string userId, string searchValue = null)
        {
            return await _careerRepository.GetCareerByUserCoordinatorId(userId, searchValue);
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetCareerSelect2ByAcademicCoordinatorClientSide(string academicCoordinatorId, Guid? selectedId = null)
            => await _careerRepository.GetCareerSelect2ByAcademicCoordinatorClientSide(academicCoordinatorId, selectedId);
        public async Task<object> GetSelect2ByFaculty(Guid studentId)
            => await _careerRepository.GetSelect2ByFaculty(studentId);
        public async Task<object> GetCareersToForum()
            => await _careerRepository.GetCareersToForum();
        public async Task<List<ForumCareer>> GetForumCareer(ForumTemplate model)
            => await _careerRepository.GetForumCareer(model);

        public async Task<object> GetCareerSelect2Curriculum(Guid fid, List<Guid> FacultyIds = null)
            => await _careerRepository.GetCareerSelect2Curriculum(fid, FacultyIds);

        public Task<object> GetCareerAcademicSecretaryByCareerId(Guid careerId)
            => _careerRepository.GetCareerAcademicSecretaryByCareerId(careerId);

        public Task<object> GetCareerAcademicCoordinatorByCareerId(Guid careerId)
            => _careerRepository.GetCareerAcademicCoordinatorByCareerId(careerId);

        public Task<List<ModelATemplate>> GetAllAsModelA(string search = null)
            => _careerRepository.GetAllAsModelA(search);

        public Task<object> GetAllAsSelect2ClientSide(Guid? facultyId = null, bool includeTitle = false, Guid? careerId = null, string coordinatorId = null, ClaimsPrincipal user = null)
            => _careerRepository.GetAllAsSelect2ClientSide(facultyId, includeTitle, careerId, coordinatorId, user);

        public async Task<int> Count()
        {
            return await _careerRepository.Count();
        }

        public Task<Guid> GetIdByCoordinatorOrSecretary(string academicCoordinatorId, string academicSecretaryId)
        {
            return _careerRepository.GetIdByCoordinatorOrSecretary(academicCoordinatorId, academicSecretaryId);
        }

        public async Task<List<Career>> GetAllByfacultyId(Guid? facultyId = null)
            => await _careerRepository.GetAllByfacultyId(facultyId);

        public Task<object> GetCareerAcademicDepartmentDirectorByCareerId(Guid careerId)
            => _careerRepository.GetCareerAcademicDepartmentDirectorByCareerId(careerId);

        public Task<object> GetCareerAcademicCareerDirectorByCareerId(Guid careerId)
            => _careerRepository.GetCareerAcademicCareerDirectorByCareerId(careerId);

        public async Task<object> GetEnrollmentShiftDatatableClientSide() => await _careerRepository.GetEnrollmentShiftDatatableClientSide();
        public async Task<Career> GetCareerReport(Guid careerId)
            => await _careerRepository.GetCareerReport(careerId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetCareersDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, ClaimsPrincipal user, string userId)
            => await _careerRepository.GetCareersDatatable(sentParameters, termId, user, userId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetCareers2Datatable(DataTablesStructs.SentParameters sentParameters, Guid termId, ClaimsPrincipal user, string userId, Guid facultyId)
    => await _careerRepository.GetCareers2Datatable(sentParameters, termId, user, userId, facultyId);

        public async Task<IEnumerable<Career>> GetAllWithData()
            => await _careerRepository.GetAllWithData();

        public async Task<object> GetCareersJson(string q, ClaimsPrincipal user = null)
            => await _careerRepository.GetCareersJson(q, user);

        public async Task<object> GetCareerAndAreaJson()
            => await _careerRepository.GetCareerAndAreaJson();

        public async Task<object> GetCareersByIdJson(Guid id)
            => await _careerRepository.GetCareersByIdJson(id);

        public async Task<object> GetCareersByFacultyIdJson(Guid id, bool hasAll = false, ClaimsPrincipal user = null)
            => await _careerRepository.GetCareersByFacultyIdJson(id, hasAll, user);

        public async Task<object> GetCareersByFacultyIdWithAllJson(Guid id)
            => await _careerRepository.GetCareersByFacultyIdWithAllJson(id);

        public async Task<Career> GetNameByCellExcel(string cell)
            => await _careerRepository.GetNameByCellExcel(cell);

        public async Task<List<Career>> GetCareerFacultyById(Guid id)
            => await _careerRepository.GetCareerFacultyById(id);

        public async Task<Guid> GetGuidFirst()
            => await _careerRepository.GetGuidFirst();
        public async Task<object> GetCareers()
            => await _careerRepository.GetCareers();

        public async Task<object> GetAllCareerWithStudentQty()
        {
            return await _careerRepository.GetAllCareerWithStudentQty();
        }
        public async Task<object> GetAllCareerWithStudentQty(Guid id,int year)
        {
            return await _careerRepository.GetAllCareerWithStudentQty(id, year);
        }

        public async Task InsertRange(Career[] listCareer)
        {
            await _careerRepository.InsertRange(listCareer);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrolledStudentsByCareer(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, Guid? termId, Guid? campusId = null, bool onlyWithStudents = false)
        {
            return await _careerRepository.GetEnrolledStudentsByCareer(sentParameters,user, termId, campusId, onlyWithStudents);
        }

        public async Task<IEnumerable<Career>> GettAllData2(string search = null)
            => await _careerRepository.GettAllData2(search);
        public async Task<object> GetCareerWithCountTutorAndTutorStudents()
            => await _careerRepository.GetCareerWithCountTutorAndTutorStudents();

        public async Task<List<HisotricTemplate>> GetHistory(Guid id)
        {
            return await _careerRepository.GetHistory(id);
        }

        public async Task<List<Career>> GetCareersByClaim(ClaimsPrincipal user)
            => await _careerRepository.GetCareersByClaim(user);
        public async Task<object> GetDetailReport(Guid termId, ClaimsPrincipal user)
            => await _careerRepository.GetDetailReport(termId, user);
        public async Task<List<Career>> GetList(Guid careerId)
            => await _careerRepository.GetList(careerId);

        public Task<bool> AnyByCode(string code, Guid? id = null)
            => _careerRepository.AnyByCode(code, id);
        public async Task<DataTablesStructs.ReturnedData<object>> GetCareersDatatableByClaim(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, string searchValue, bool? withAcreditationsActive = null
            )
            => await _careerRepository.GetCareersDatatableByClaim(sentParameters, user, searchValue, withAcreditationsActive);

        public async Task<bool> AnyAcademicDepartmentByCareerDirector(string userId)
            => await _careerRepository.AnyAcademicDepartmentByCareerDirector(userId);

        public async Task<object> GetCarrerConditionSelect2(ClaimsPrincipal user)
        {
            return await _careerRepository.GetCarrerConditionSelect2(user);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetNumberOfApprovedStudentsDatatable(DataTablesStructs.SentParameters parameters, Guid? termId, Guid? facultyId = null, Guid? careerId = null , ClaimsPrincipal? user = null)
            => await _careerRepository.GetNumberOfApprovedStudentsDatatable(parameters, termId, facultyId, careerId, user);

        public Task<DataTablesStructs.ReturnedData<object>> GetCareersQualityCoordinatorDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _careerRepository.GetCareersQualityCoordinatorDatatable(sentParameters, searchValue);

        public Task<List<Career>> GetFacultiesCareersByDean(string deanId)
            => _careerRepository.GetFacultiesCareersByDean(deanId);

        public Task<List<Career>> GetFacultiesCareersBySecretary(string secretaryId)
            => _careerRepository.GetFacultiesCareersBySecretary(secretaryId);

        public Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentGraduatedCareerQuantityReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, int? searchStartYear = null, int? searchEndYear = null)
            => _careerRepository.GetJobExchangeStudentGraduatedCareerQuantityReportDatatable(sentParameters, careerId, searchStartYear, searchEndYear);

        public Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentBachelorQualifiedCareerQuantityReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null)
            => _careerRepository.GetJobExchangeStudentBachelorQualifiedCareerQuantityReportDatatable(sentParameters, careerId);
    }
}