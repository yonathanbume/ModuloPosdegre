using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.User;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.SurveyUser;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.SuneduReport;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<bool> Any(string id)
            => _userRepository.Any(id);

        public Task<ApplicationUser> Get(string id)
            => _userRepository.Get(id);

        public Task<IEnumerable<ApplicationUser>> GetAll()
            => _userRepository.GetAll();

        public Task<IEnumerable<ApplicationUser>> GetDependencyUsers()
            => _userRepository.GetDependencyUsers();

        public Task<IEnumerable<ApplicationUser>> GetBySearchValue(string searchValue)
            => _userRepository.GetBySearchValue(searchValue);

        public Task<ApplicationUser> GetByUserName(string userName)
            => _userRepository.GetByUserName(userName);

        public async Task<ApplicationUser> GetByUserNameWithoutSpecialChars(string userName)
            => await _userRepository.GetByUserNameWithoutSpecialChars(userName);

        public async Task<ApplicationUser> GetByBankDocument(string document)
            => await _userRepository.GetByBankDocument(document);

        public Task<Select2Structs.ResponseParameters> GetDependencyUsersSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
            => _userRepository.GetDependencyUsersSelect2(requestParameters, searchValue);

        public Task<Select2Structs.ResponseParameters> GetExternalUsersToInterestGroupSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
            => _userRepository.GetExternalUsersToInterestGroupSelect2(requestParameters, searchValue);

        public Task<Select2Structs.ResponseParameters> GetUsersSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
            => _userRepository.GetUsersSelect2(requestParameters, searchValue);
        public async Task<Select2Structs.ResponseParameters> GetDependencyUsersSelect2ByDependency(Select2Structs.RequestParameters requestParameters, Guid dependency, string searchValue = null)
            => await _userRepository.GetDependencyUsersSelect2ByDependency(requestParameters, dependency, searchValue);

        public Task Delete(ApplicationUser user)
            => _userRepository.Delete(user);

        public Task Insert(ApplicationUser user)
        {
            return _userRepository.Insert(user);
        }

        public Task Update(ApplicationUser user)
            => _userRepository.Update(user);

        public Task<int> CountWorkers()
            => _userRepository.CountWorkers();

        public Task RemoveFromRole(ApplicationUser user, string roleName)
            => _userRepository.RemoveFromRole(user, roleName);

        public Task RemoveFromRole(ApplicationUser user, ApplicationRole role)
            => _userRepository.RemoveFromRole(user, role);

        public Task RemoveFromRoles(ApplicationUser user, IEnumerable<string> roleNames)
            => _userRepository.RemoveFromRoles(user, roleNames);

        public Task RemoveFromRoles(ApplicationUser user, IEnumerable<ApplicationRole> roles)
            => _userRepository.RemoveFromRoles(user, roles);

        public Task<IEnumerable<string>> GetRoles(ApplicationUser user)
            => _userRepository.GetRoles(user);

        public Task<ApplicationUser> GetByEmail(string email)
            => _userRepository.GetByEmail(email);
        public async Task<ApplicationUser> GetByEmailFirst(string email, string id)
            => await _userRepository.GetByEmailFirst(email, id);
        public Task<ApplicationUser> GetReniecUserByDni(string dni)
            => _userRepository.GetReniecUserByDni(dni);

        public async Task<ApplicationUser> GetUserByEmail(string email)
            => await _userRepository.GetUserByEmail(email);

        public Task AddToRole(ApplicationUser user, string roleName)
            => _userRepository.AddToRole(user, roleName);

        public Task AddToRoles(ApplicationUser user, IEnumerable<string> roleNames)
            => _userRepository.AddToRoles(user, roleNames);

        public Task<ApplicationUser> GetUserByClaim(ClaimsPrincipal user)
            => _userRepository.GetUserByClaim(user);

        public string GetUserIdByClaim(ClaimsPrincipal user)
            => _userRepository.GetUserIdByClaim(user);

        public Task<bool> AnyByUserName(string userName, string ignoredId = null)
            => _userRepository.AnyByUserName(userName, ignoredId);

        public Task<bool> AnyByEmail(string email, string ignoredId = null, string dni = null)
            => _userRepository.AnyByEmail(email, ignoredId, dni);

        public Task<bool> AnyByEmailIgnoreQueryFilter(string email, string ignoredId = null, string dni = null)
            => _userRepository.AnyByEmailIgnoreQueryFilter(email, ignoredId, dni);

        public Task<DataTablesStructs.ReturnedData<ApplicationUser>> GetExternalUsersToInterestGroupDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _userRepository.GetExternalUsersToInterestGroupDatatable(sentParameters, searchValue);

        public async Task<DataTablesStructs.ReturnedData<object>> GetSurveyJobExchangeUsersDatatable(DataTablesStructs.SentParameters sentParameters, Guid companyId, int status, string rol, List<Guid> graduationTerms, string searchValue = null, Guid? careerId = null, ClaimsPrincipal user = null)
        {
            return await _userRepository.GetSurveyJobExchangeUsersDatatable(sentParameters, companyId, status, rol, graduationTerms, searchValue, careerId, user);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSurveyIntranetUsersDatatable(DataTablesStructs.SentParameters sentParameters, string rol, List<int> academicYears, bool onlyEnrolled = false, Guid? careerId = null, Guid? facultyId = null, Guid? specialtyId = null, Guid? academicDepartmentId = null)
        {
            return await _userRepository.GetSurveyIntranetUsersDatatable(sentParameters, rol , academicYears, onlyEnrolled ,careerId, facultyId, specialtyId, academicDepartmentId);
        }

        public async Task<int> CountSurveyIntranetUsers(string rol, List<int> academicYears, bool onlyEnrolled = false, Guid? careerId = null, Guid? facultyId = null, Guid? specialtyId = null, Guid? academicDepartmentId = null)
        {
            return await _userRepository.CountSurveyIntranetUsers(rol, academicYears, onlyEnrolled, careerId, facultyId, specialtyId, academicDepartmentId);
        }

        public async Task<IEnumerable<ApplicationUser>> GetSurveyIntranetUsers(string rol, List<int> academicYears, bool onlyEnrolled = false, Guid? careerId = null, Guid? facultyId = null, Guid? specialtyId = null, Guid? academicDepartmentId = null)
        {
            return await _userRepository.GetSurveyIntranetUsers(rol, academicYears, onlyEnrolled, careerId , facultyId, specialtyId, academicDepartmentId);
        }

        public async Task<ApplicationUser> GetUserById(string userId)
            => await _userRepository.GetUserById(userId);
        public async Task<ApplicationUser> GetDeletedUserById(string userId)
            => await _userRepository.GetDeletedUserById(userId);

        public async Task<ApplicationUser> GetUserWithDependecies(string userId)
            => await _userRepository.GetUserWithDependecies(userId);

        public async Task<IEnumerable<Select2Structs.Result>> GetUsersByRolesSelect2ClientSide(IEnumerable<string> roles)
            => await _userRepository.GetUsersByRolesSelect2ClientSide(roles);

        public IQueryable<ApplicationUser> GetAllIQueryable()
        {
            return _userRepository.GetAllIQueryable();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUsersForEvaluationAndInvestigationDatatable(DataTablesStructs.SentParameters sentParameters, IEnumerable<string> roles, byte? role, byte type, Guid? careerId = null, Guid? facultyId = null, string search = null)
        {
            return await _userRepository.GetUsersForEvaluationAndInvestigationDatatable(sentParameters, roles, role, type, careerId, facultyId, search);
        }

        public async Task<bool> AnyUserByEmail(string userId, string email)
        {
            return await _userRepository.AnyUserByEmail(userId, email);
        }

        public async Task<Select2Structs.ResponseParameters> GetUsersForInvestigationAndEvalutionSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
            => await _userRepository.GetUsersForInvestigationAndEvalutionSelect2(requestParameters, searchValue);

        public async Task<int> CountSurveyJobExchangeUsersDatatable(Guid companyId, int status, string rol, List<Guid> graduationTerms, string searchValue = null, Guid? careerId = null, ClaimsPrincipal user = null)
        {
            return await _userRepository.CountSurveyJobExchangeUsersDatatable(companyId, status, rol, graduationTerms, searchValue, careerId, user);
        }

        public async Task<List<SurveyUserTemplate>> GetSurveyJobExchangeUsers(Guid companyId, int status, string rol, List<Guid> graduationTerms, string searchValue = null, Guid? careerId = null, ClaimsPrincipal user = null)
        {
            return await _userRepository.GetSurveyJobExchangeUsers(companyId, status, rol, graduationTerms, searchValue, careerId, user);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUsersBySurveyJobExchangeGeneralDatatable(DataTablesStructs.SentParameters sentParameters, Guid surveyId, Guid careerId, Guid facultyId)
        {
            return await _userRepository.GetUsersBySurveyJobExchangeGeneralDatatable(sentParameters, surveyId, careerId, facultyId);
        }

        public Task<DataTablesStructs.ReturnedData<ApplicationUser>> GetUsersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, string role = null, IEnumerable<string> exceptionRoles = null)
            => _userRepository.GetUsersDatatable(sentParameters, searchValue, role, exceptionRoles);

        public async Task<DataTablesStructs.ReturnedData<ApplicationUser>> GetUsersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, string role = null)
        {
            return await _userRepository.GetUsersDatatable(sentParameters, searchValue, role);
        }

        public async Task<string> GetLastByPrefix(string prefix) => await _userRepository.GetLastByPrefix(prefix);

        public async Task<string> GetUserWithCodeExist(string userCodePrix)
            => await _userRepository.GetUserWithCodeExist(userCodePrix);

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserCashierDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => await _userRepository.GetUserCashierDatatable(sentParameters, search);

        public async Task<ApplicationUser> GetUserByStudent(Guid studentId)
            => await _userRepository.GetUserByStudent(studentId);

        public Task<object> GetOnlyAdministrative(Select2Structs.RequestParameters requestParameters, string searchedValue)
            => _userRepository.GetOnlyAdministrative(requestParameters, searchedValue);
        public async Task<ApplicationUser> GetDependencyUserByUserDependency(Guid? dependencyId = null)
            => await _userRepository.GetDependencyUserByUserDependency(dependencyId);
        public async Task<string> GetNameComplete(string userId)
            => await _userRepository.GetNameComplete(userId);
        public async Task<object> GetUserJson(string term)
            => await _userRepository.GetUserJson(term);

        public async Task<DataTablesStructs.ReturnedData<object>> GetScaleUsersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, int? userType = null)
            => await _userRepository.GetScaleUsersDatatable(sentParameters, searchValue, userType);

        public async Task<Select2Structs.ResponseParameters> GetUsersByRoleNameSelect2(Select2Structs.RequestParameters requestParameters, string roleName = null, string searchValue = null)
            => await _userRepository.GetUsersByRoleNameSelect2(requestParameters, roleName, searchValue);

        public async Task<Select2Structs.ResponseParameters> GetUsersByDependencyIdSelect2(Select2Structs.RequestParameters requestParameters, Guid dependecyId, string searchValue)
            => await _userRepository.GetUsersByDependencyIdSelect2(requestParameters, dependecyId, searchValue);

        public async Task<ApplicationUser> GetWithData(string userId)
            => await _userRepository.GetWithData(userId);

        public async Task<List<string>> GetAllEmails(List<string> userId)
            => await _userRepository.GetAllEmails(userId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetAcademicRecordUsers(DataTablesStructs.SentParameters sentParameters, string search = null)
            => await _userRepository.GetAcademicRecordUsers(sentParameters, search);

        public async Task<object> GetUsersTreasurySelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
            => await _userRepository.GetUsersTreasurySelect2(requestParameters, searchValue);

        public async Task<bool> AnyByDni(string dni, string ignoredId = null)
            => await _userRepository.AnyByDni(dni, ignoredId);

        public async Task<Select2Structs.ResponseParameters> Select2WithOutStudentRole(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await _userRepository.Select2WithOutStudentRole(requestParameters, searchValue);
        }

        public async Task<object> GetUSersSelect2CLientSideToSisco(string term)
            => await _userRepository.GetUSersSelect2CLientSideToSisco(term);

        public async Task UpdateUsersPasswordJob(string connectionString)
        {
            await _userRepository.UpdateUsersPasswordJob(connectionString);
        }

        public async Task UpdateUserFullNameJob()
        {
            await _userRepository.UpdateUserFullNameJob();
        }

        public async Task SaveChanges()
        {
            await _userRepository.SaveChanges();
        }

        public async Task LoadUsersFullNameJob()
        {
            await _userRepository.LoadUsersFullNameJob();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachersAndStudents(DataTablesStructs.SentParameters sentParameters, Guid? careerId, int type, string searchValue = null)
        {
            return await _userRepository.GetTeachersAndStudents(sentParameters, careerId, type, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPreuniversitaryStudentsDatatable(DataTablesStructs.SentParameters sentParameters, Guid preuniversitaryTermId, string searchValue = null)
            => await _userRepository.GetPreuniversitaryStudentsDatatable(sentParameters, preuniversitaryTermId, searchValue);

        public async Task<bool> AnyByDniAndUserName(string dni, string username, string ignoredId = null)
        {
            return await _userRepository.AnyByDniAndUserName(dni, username, ignoredId);
        }

        public async Task<ApplicationUser> GetByFullName(string fullName)
            => await _userRepository.GetByFullName(fullName);

        public async Task<IEnumerable<ApplicationUser>> GetAllByDni(string dni)
            => await _userRepository.GetAllByDni(dni);

        public Task<DataTablesStructs.ReturnedData<object>> GetOlderAdministrativeDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => _userRepository.GetOlderAdministrativeDatatable(sentParameters, search);

        public Task<DataTablesStructs.ReturnedData<object>> GetScaleIgnoreQueryFilterUsersDatatable(DataTablesStructs.SentParameters sentParameters, int? state = null, string searchValue = null, int? userType = null)
            => _userRepository.GetScaleIgnoreQueryFilterUsersDatatable(sentParameters,state, searchValue, userType);

        public Task<DataTablesStructs.ReturnedData<object>> GetScaleContractUsersDatatable(DataTablesStructs.SentParameters sentParameters, Guid? conditionId = null, Guid? dedicationId = null, string searchValue = null)
            => _userRepository.GetScaleContractUsersDatatable(sentParameters, conditionId, dedicationId, searchValue);

        public async Task<string> ShowPasswordHint(string userName, string userWeb)
            => await _userRepository.ShowPasswordHint(userName, userWeb);

        public async Task<ApplicationUser> GetByUserWeb(string userWeb)
            => await _userRepository.GetByUserWeb(userWeb);

        public async Task<object> GetUsersAuthoritySelect2ServerSide(string search)
        {
            return await _userRepository.GetUsersAuthoritySelect2ServerSide(search);
        }
        public async Task<object> GetUsersStudentsSelect2ServerSide(string search)
        {
            return await _userRepository.GetUsersStudentsSelect2ServerSide(search);
        }
        public async Task<object> GetAllUsersSelect2ServerSide(string search)
        {
            return await _userRepository.GetAllUsersSelect2ServerSide(search);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserWithDoctoralDegreeDatatable(DataTablesStructs.SentParameters parameters, string searchValue)
            => await _userRepository.GetUserWithDoctoralDegreeDatatable(parameters, searchValue);

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserWithMasterDegreeDatatable(DataTablesStructs.SentParameters parameters, string searchValue)
            => await _userRepository.GetUserWithMasterDegreeDatatable(parameters, searchValue);

        public async Task<DataTablesStructs.ReturnedData<object>> GetUsersWithTrainingDatatable(DataTablesStructs.SentParameters parameters, string searchValue)
            => await _userRepository.GetUsersWithTrainingDatatable(parameters, searchValue);

        public async Task<bool> AnyWithSameEmail(string userId, string email)
            => await _userRepository.AnyWithSameEmail(userId, email);
        public async Task<ApplicationUser> GetAdminitrativeByUserId(string userId)
            => await _userRepository.GetAdminitrativeByUserId(userId);
        public Task<string> GetPersonalizedEmailPassword(string username)
            => _userRepository.GetPersonalizedEmailPassword(username);

        public Task<List<FavoriteCompany>> GetAllFavoriteCompaniesFromUser(string userId)
            => _userRepository.GetAllFavoriteCompaniesFromUser(userId);
        public Task<ApplicationUser> GetIgnoreQueryFilter(string id)
            => _userRepository.GetIgnoreQueryFilter(id);

        public Task<bool> AnyByUserNameIgnoreQueryFilter(string userName, string ignoredId = null)
            => _userRepository.AnyByUserNameIgnoreQueryFilter(userName, ignoredId);

        public Task<List<AdministrativeSuneduReportTemplate>> GetSuneduReportForAdministrative(Guid termId)
            => _userRepository.GetSuneduReportForAdministrative(termId);

        public Task<List<TeacherSuneduReportTemplate>> GetSuneduReportForTeacher(Guid termId)
            => _userRepository.GetSuneduReportForTeacher(termId);

        public Task<List<TeacherSuneduReportTemplate>> GetSuneduC9ReportForTeacher(Guid termId)
            => _userRepository.GetSuneduC9ReportForTeacher(termId);

        public Task<List<S3TeacherSuneduReportTemplate>> GetSuneduS3ReportForTeacher(Guid termId)
            => _userRepository.GetSuneduS3ReportForTeacher(termId);

        public Task<DataTablesStructs.ReturnedData<object>> GetScaleBenefitDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _userRepository.GetScaleBenefitDatatable(sentParameters,searchValue);

        public Task<List<ApplicationUser>> GetAllByEmail(string email)
            => _userRepository.GetAllByEmail(email);

        public Task<object> SearchByTerm(string term, bool showStudents = false, bool showTeachers = false)
            => _userRepository.SearchByTerm(term, showStudents, showTeachers);

        public Task<IEnumerable<ApplicationUser>> GetAllStudents()
            => _userRepository.GetAllStudents();

        public Task<DataTablesStructs.ReturnedData<object>> GetScaleGeographicalUsersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _userRepository.GetScaleGeographicalUsersDatatable(sentParameters,searchValue);

        public Task<List<UserScaleInformation>> GetScaleGeographicalUsersReport()
            => _userRepository.GetScaleGeographicalUsersReport();

        public Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeUsersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, string rolId = null)
            => _userRepository.GetJobExchangeUsersDatatable(sentParameters, searchValue, rolId);

        public Task<List<string>> GetJobExchangeUsersEmails(string searchValue = null, string rolId = null)
            => _userRepository.GetJobExchangeUsersEmails(searchValue, rolId);

        public async Task<object> GetNonStudentUsersSelect2(string term, List<string> filteredUsers = null)
            => await _userRepository.GetNonStudentUsersSelect2(term, filteredUsers);

        public Task<List<SurveyUserTemplate>> GetSurveyUsersToSend(List<string> users)
            => _userRepository.GetSurveyUsersToSend(users);

        public async Task<DataTablesStructs.ReturnedData<object>> GetUsersManagementDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => await _userRepository.GetUsersManagementDatatable(sentParameters, search);

        public async Task<DataTablesStructs.ReturnedData<object>> GetLockedUsersDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => await _userRepository.GetLockedUsersDatatable(sentParameters, search);

        public Task<ApplicationUser> GetWithGeoLocation(string id)
            => _userRepository.GetWithGeoLocation(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetUsersDatatableByType(DataTablesStructs.SentParameters sentParameters, byte type, string search, List<string> roles = null)
            => await _userRepository.GetUsersDatatableByType(sentParameters, type, search, roles);

        public async Task<List<ApplicationUser>> SearchByEmail(string email)
            => await _userRepository.SearchByEmail(email);

        public Task<ApplicationUser> GetByUserNameIgnoreQueryFilter(string userName)
            => _userRepository.GetByUserNameIgnoreQueryFilter(userName);

        public Task<ApplicationUser> Add(ApplicationUser user)
            => _userRepository.Add(user);

        public Task<Select2Structs.ResponseParameters> GetUsersByTypeSelect2(Select2Structs.RequestParameters requestParameters, int? userType = null, string searchValue = null)
            => _userRepository.GetUsersByTypeSelect2(requestParameters, userType, searchValue);

        public Task<DataTablesStructs.ReturnedData<object>> GetUserDataTable(DataTablesStructs.SentParameters parameters1, string search)
            =>_userRepository.GetUserDataTable(parameters1, search);
    }
}