using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Teacher;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Education;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.TeacherDedication;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;
        public TeacherService(ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        public async Task<Select2Structs.ResponseParameters> GetFacultyTachersByAcademicProgramId(Select2Structs.RequestParameters requestParameters, Guid academicProgramId, string keyname)
            => await _teacherRepository.GetFacultyTachersByAcademicProgramId(requestParameters, academicProgramId, keyname);

        public async Task<IEnumerable<Teacher>> GetAll()
            => await _teacherRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetTeacherDatatableByLevelStudy(DataTablesStructs.SentParameters sentParameters, int? levelStudy, string search = null)
            => _teacherRepository.GetTeacherDatatableByLevelStudy(sentParameters,levelStudy,search);
        public Task<DataTablesStructs.ReturnedData<object>> GetTeacherDatatableByAgeSex(DataTablesStructs.SentParameters sentParameters, int? ageRange = null, int? sex = null, string search = null, ClaimsPrincipal user = null)
            => _teacherRepository.GetTeacherDatatableByAgeSex(sentParameters, ageRange, sex, search, user);

        public async Task<object> CountByAgeRangeAndSex(int? ageRange = null, int? sex = null) =>
            await _teacherRepository.CountByAgeRangeAndSex(ageRange, sex);

        public async Task<IEnumerable<Teacher>> GetPerformanceByCareerAndTermAndWeekAndDedication(Guid termId, int week,
            Guid? facultyId = null,
            Guid? teacherDedicationId = null) =>
            await _teacherRepository.GetPerformanceByCareerAndTermAndWeekAndDedication(termId, week, facultyId,
                teacherDedicationId);

        public async Task<object> CountPerformanceByCareerAndTermAndWeekAndDedication(Guid termId,
            int? week = null, Guid? facultyId = null, Guid? teacherDedicationId = null) =>
            await _teacherRepository.CountPerformanceByCareerAndTermAndWeekAndDedication(termId, week, facultyId,
                teacherDedicationId);

        public async Task<object> CountByLevelStudy() => await _teacherRepository.CountByLevelStudy();

        public async Task<IEnumerable<Teacher>> GetAllWithUser()
            => await _teacherRepository.GetAllWithUser();

        public async Task<Select2Structs.ResponseParameters> GetTeachersSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<Teacher, Select2Structs.Result>> selectPredicate = null, Func<Teacher, string[]> searchValuePredicate = null, string searchValue = null, int? status = null)
            => await _teacherRepository.GetTeachersSelect2(requestParameters, selectPredicate, searchValuePredicate, searchValue, status);
        public async Task<Select2Structs.ResponseParameters> GetTeachersByCareer(Select2Structs.RequestParameters requestParameters, Guid id, Expression<Func<Teacher, Select2Structs.Result>> selectPredicate = null, Func<Teacher, string[]> searchValuePredicate = null, string searchValue = null, int? status = null)
            => await _teacherRepository.GetTeachersByCareer(requestParameters, id, selectPredicate, searchValuePredicate, searchValue, status);
        public async Task<IEnumerable<TeacherTemplateA>> GetTeachersModelA(Guid? facultyId, string coordinatorId = null, Guid? academicDepartmentId = null, ClaimsPrincipal user = null)
            => await _teacherRepository.GetTeachersModelA(facultyId, coordinatorId, academicDepartmentId,user);

        public async Task<string> GetTeacherFullNameById(string userId)
            => await _teacherRepository.GetTeacherFullNameById(userId);

        public async Task<List<ConditionAndDedicationTemplate>> GetAllByConditionAndDedication(Guid? condition, Guid? dedication, Guid? careerId, Guid? category, string search, string coordinatorId = null, Guid? academicDeaprtmentId = null, Guid? regimeId = null)
            => await _teacherRepository.GetAllByConditionAndDedication(condition, dedication, careerId, category,search, coordinatorId, academicDeaprtmentId, regimeId);
        public async Task<DataTablesStructs.ReturnedData<ConditionAndDedicationTemplate>> GetAllByConditionAndDedicationDataTable(DataTablesStructs.SentParameters sentParameters,Guid? condition, Guid? dedication, Guid? careerId, Guid? category, string search, string coordinatorId = null, Guid? academicDepartmentId = null, Guid? regimeId = null, int? maxStudyLevel = null)
            {
            return await _teacherRepository.GetAllByConditionAndDedicationDataTable(sentParameters,condition, dedication, careerId, category, search, coordinatorId, academicDepartmentId, regimeId, maxStudyLevel);
                }
        public async Task<object> GetAllAsSelect2ClientSide(Guid? facultyId = null, string keyWord = null, Guid? careerId = null, string coordinatorId = null, Guid? academicDepartmentId = null)
            => await _teacherRepository.GetAllAsSelect2ClientSide(facultyId, keyWord, careerId, coordinatorId, academicDepartmentId);

        public async Task AddAsync(Teacher teacher)
            => await _teacherRepository.Add(teacher);

        public async Task<Teacher> GetAsync(string id)
            => await _teacherRepository.Get(id);

        public Task DeleteAsync(Teacher teacher)
            => _teacherRepository.Delete(teacher);

        public async Task<Teacher> GetTeacherWithData(string userId)
            => await _teacherRepository.GetTeacherWithData(userId);
        public async Task<IEnumerable<TeacherPlusAssitance>> GetAllPlusAssitance(Guid? careerId = null, string secretaryId = null)
            => await _teacherRepository.GetAllPlusAssitance(careerId, secretaryId);
        public async Task<object> GetAllAsSelect2ClientSide(Guid? facultyId = null, string keyWord = null)
            => await _teacherRepository.GetAllAsSelect2ClientSide(facultyId, keyWord);

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachersDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId, Guid? careerId, string search = null, Guid? termId = null, Guid? academicDepartment = null, ClaimsPrincipal user = null)
            => await _teacherRepository.GetTeachersDatatable(sentParameters, facultyId, careerId, search, termId, academicDepartment, user);

        public Task<DataTablesStructs.ReturnedData<object>> GetIntranetTeachersDatatable(DataTablesStructs.SentParameters sentParameters, Guid? academicDepartmentId = null, string search = null, ClaimsPrincipal user = null, Guid? termId = null)
            => _teacherRepository.GetIntranetTeachersDatatable(sentParameters, academicDepartmentId, search, user, termId);


        public async Task<List<Teacher>> GetAllByCareers(List<Guid> careers)
        {
            return await _teacherRepository.GetAllByCareers(careers);
        }

        public async Task<int> CountByCareers(List<Guid> careers)
        {
            return await _teacherRepository.CountByCareers(careers);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachersByCareersDatatable(DataTablesStructs.SentParameters sentParameters, List<Guid> careers)
        {
            return await _teacherRepository.GetTeachersByCareersDatatable(sentParameters, careers);
        }
        public async Task<object> GetAllByTermandCareer(Guid termId, Guid? careerId, string coordinatorId = null, byte? status = null, ClaimsPrincipal user = null, Guid? academicDepartmentId = null)
            => await _teacherRepository.GetAllByTermandCareer(termId, careerId, coordinatorId, status, user, academicDepartmentId);

        public async Task<object> GetTeacherSelect2Report()
            => await _teacherRepository.GetTeacherSelect2Report();

        public async Task<Select2Structs.ResponseParameters> GetTeachersSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null, Guid? careerId = null, Guid? academicDepartmentId = null, ClaimsPrincipal user = null)
            => await _teacherRepository.GetTeachersSelect2(requestParameters, searchValue, careerId, academicDepartmentId, user);

        public async Task<IEnumerable<Select2Structs.Result>> GetTeachersSelect2ClientSide(Guid? facultyId = null, Guid? careerId = null)
            => await _teacherRepository.GetTeachersSelect2ClientSide(facultyId, careerId);

        public Task<object> GetTeacherSelectByAcademicDepartment(Guid? facultyId = null, Guid? academicDepartmentId = null)
            => _teacherRepository.GetTeacherSelectByAcademicDepartment(facultyId,academicDepartmentId);
        public Task<TeacherTemplateA> GetAsTemplateAById(string teacherId)
            => _teacherRepository.GetAsTemplateAById(teacherId);

        public Task<object> GetAllAsModelC()
            => _teacherRepository.GetAllAsModelC();

        public Task InsertAsync(Teacher teacher)
            => _teacherRepository.Insert(teacher);

        public Task UpdateAsync(Teacher teacher)
            => _teacherRepository.Update(teacher);

        public Task<Teacher> GetWithTeacherDedication(string id)
            => _teacherRepository.GetWithTeacherDedication(id);

        public async Task<int> GetTeachersEnrolledCountByTermIdAndLevelStudie(Guid termId, int levelStudy)
            => await _teacherRepository.GetTeachersEnrolledCountByTermIdAndLevelStudie(termId, levelStudy);

        public async Task<int> GetTeachersEnrolledCountByTermId(Guid termId, ClaimsPrincipal user = null)
            => await _teacherRepository.GetTeachersEnrolledCountByTermId(termId, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetByCategoryAndTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? categoryId = null, ClaimsPrincipal user = null)
            => await _teacherRepository.GetByCategoryAndTermDatatable(sentParameters, termId, categoryId, user);

        public async Task<object> GetByCategoryAndTermChart(Guid? termId = null, Guid? categoryId = null, ClaimsPrincipal user = null)
            => await _teacherRepository.GetByCategoryAndTermChart(termId, categoryId, user);

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherByCountryDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? countryId = null)
            => await _teacherRepository.GetTeacherByCountryDatatable(sentParameters, termId, countryId);

        public async Task<object> GetTeachersJson(string q)
            => await _teacherRepository.GetTeachersJson(q);

        public async Task<object> GetTeacherJsonByUserId(string userId)
            => await _teacherRepository.GetTeacherJsonByUserId(userId);

        public async Task<object> GetTeachersByIdJson(Guid id)
            => await _teacherRepository.GetTeachersByIdJson(id);

        public async Task<object> GetTeacherByCountryChart(Guid? termId = null, Guid? countryId = null)
            => await _teacherRepository.GetTeacherByCountryChart(termId, countryId);

        public async Task<int> GetRegisteredTeachersCountByTermId(Guid termId)
            => await _teacherRepository.GetRegisteredTeachersCountByTermId(termId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachersWithInvestigationProjectDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null)
            => await _teacherRepository.GetTeachersWithInvestigationProjectDatatable(sentParameters, user);

        public Task<DataTablesStructs.ReturnedData<object>> GetOlderTeacherDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => _teacherRepository.GetOlderTeacherDatatable(sentParameters,search);

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllEscalafonReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? academicDepartmentId = null, Guid? conditionId = null, Guid? dedicationId = null, Guid? categoryId = null, Guid? capPositionId = null)
            => await _teacherRepository.GetAllEscalafonReportDatatable(sentParameters, academicDepartmentId, conditionId, dedicationId, categoryId, capPositionId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherByDedicationDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? dedicationId = null, ClaimsPrincipal user = null)
            => await _teacherRepository.GetTeacherByDedicationDatatable(sentParameters,termId, dedicationId, user);

        public async Task<object> GetTeacherByDedicationChart(Guid? termId = null, Guid? dedicationId = null, ClaimsPrincipal user = null)
            => await _teacherRepository.GetTeacherByDedicationChart(termId, dedicationId, user);

        public async Task<int> Count()
            => await _teacherRepository.Count();

        public async Task<Select2Structs.ResponseParameters> GetTeachersByFacultySelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null, Guid? facultyId = null, bool? isActive = null)
            => await _teacherRepository.GetTeachersByFacultySelect2(requestParameters, searchValue, facultyId, isActive);

        public async Task<DataTablesStructs.ReturnedData<Teacher>> GetTeacherByClaimDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, Guid? facultyId = null, string search = null)
            => await _teacherRepository.GetTeacherByClaimDatatable(sentParameters, user, facultyId, search);

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachingPlanDataTable(DataTablesStructs.SentParameters paginationParameter, Guid termId ,string search)
        {
            return await _teacherRepository.GetTeachingPlanDataTable(paginationParameter, termId,search);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachersToAssignCourses(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, string searchValue = null, ClaimsPrincipal user = null, Guid? academicDepartmentId = null)
            => await _teacherRepository.GetTeachersToAssignCourses(sentParameters, careerId, searchValue, user, academicDepartmentId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllByTermandCareer2(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? careerId, string coordinatorId = null, byte? status = null, string searchValue = null, Guid? academicDepartmentId = null, ClaimsPrincipal user = null, byte? sex = null,bool? viewAll = false)
        {
            return await _teacherRepository.GetAllByTermandCareer2(sentParameters, termId, careerId, coordinatorId, status, searchValue, academicDepartmentId, user, sex, viewAll);
        }

        public async Task<object> SearchTeacherByTerm(string term) => await _teacherRepository.SearchTeacherByTerm(term);

        public async Task<object> SearchTeacherByTerm(string term, List<string> filteredUsers) => await _teacherRepository.SearchTeacherByTerm(term, filteredUsers);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDismissalTeacherDatatable(DataTablesStructs.SentParameters sentParameters, string search)
            => await _teacherRepository.GetDismissalTeacherDatatable(sentParameters, search);

        public async Task<Teacher> GetByUserId(string userId) => await _teacherRepository.GetByUserId(userId);
        public async Task<Select2Structs.ResponseParameters> GetTeachersByCareerSelect2(Select2Structs.RequestParameters requestParameters,  string searchValue = null, int? status = null)
            => await _teacherRepository.GetTeachersByCareerSelect2(requestParameters, searchValue, status);

        public void Remove(Teacher teacher)
            => _teacherRepository.Remove(teacher);

        public Task<DataTablesStructs.ReturnedData<object>> GetTeachersBirthdayThisMonth(DataTablesStructs.SentParameters sentParameters)
            => _teacherRepository.GetTeachersBirthdayThisMonth(sentParameters);
        public Task<DataTablesStructs.ReturnedData<object>> GetTeachersContractThisMonth(DataTablesStructs.SentParameters sentParameters)
    => _teacherRepository.GetTeachersContractThisMonth(sentParameters);
        public Task<DataTablesStructs.ReturnedData<object>> GetTeachersLicenseThisMonth(DataTablesStructs.SentParameters sentParameters)
    => _teacherRepository.GetTeachersLicenseThisMonth(sentParameters);

        public Task<DataTablesStructs.ReturnedData<object>> GetTeacherByAgeRange(DataTablesStructs.SentParameters sentParameters, string search, int minAge, int maxAge)
            => _teacherRepository.GetTeacherByAgeRange(sentParameters,search,minAge,maxAge);

        public Task<DataTablesStructs.ReturnedData<object>> GetTeacherForAsistanceDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId, Guid? academicDepartmentId = null, ClaimsPrincipal user = null, string searchValue = null)
            => _teacherRepository.GetTeacherForAsistanceDatatable(sentParameters,facultyId,academicDepartmentId,user,searchValue);

        public Task<DataTablesStructs.ReturnedData<object>> GetStudiesDatatableByUser(DataTablesStructs.SentParameters sentParameters, string UserId)
            => _teacherRepository.GetStudiesDatatableByUser(sentParameters,UserId);

        public Task<DataTablesStructs.ReturnedData<object>> GetStudiesByStudyLevelDatatable(DataTablesStructs.SentParameters sentParameters, string search, int? studyLevel = 0, ClaimsPrincipal user = null, string expeditionDate = null, Guid? countryId = null, Guid? institutionId = null, Guid? termId = null)
            => _teacherRepository.GetStudiesByStudyLevelDatatable(sentParameters, search, studyLevel, user, expeditionDate, countryId, institutionId, termId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherClassesWithoutAttendance(DataTablesStructs.SentParameters parameters, Guid termId, Guid? academicDepartmentId, string search, DateTime endTime, ClaimsPrincipal user)
            => await _teacherRepository.GetTeacherClassesWithoutAttendance(parameters, termId, academicDepartmentId, search, endTime, user);

        public async Task<List<ScheduleTemplate>> GetTeacherCompleteSchedule(Guid termId, string teacherId, DateTime start, DateTime end)
            => await _teacherRepository.GetTeacherCompleteSchedule(termId, teacherId, start, end);

        public Task<object> GetMagisterByAcademicDepartmentChart(Guid? academicDepartmentId = null, ClaimsPrincipal user = null)
            => _teacherRepository.GetMagisterByAcademicDepartmentChart(academicDepartmentId, user);

        public Task<object> GetDoctorsByAcademicDepartmentChart(Guid? academicDepartmentId = null, ClaimsPrincipal user = null)
            => _teacherRepository.GetDoctorsByAcademicDepartmentChart(academicDepartmentId, user);

        public Task<object> GetSecondSpecialityByAcademicDepartmentChart(Guid? academicDepartmentId = null, ClaimsPrincipal user = null)
            => _teacherRepository.GetSecondSpecialityByAcademicDepartmentChart(academicDepartmentId, user);

        public Task<List<UserGenericStudiesTemplate>> GetAllUserGenericStudies(int? studyLevel = 0, ClaimsPrincipal user = null, Guid? termId = null)
            => _teacherRepository.GetAllUserGenericStudies(studyLevel, user, termId);

        public Task<List<TeacherReportTemplate>> GetAllTeacherBySexAge(int? ageRange = null, int? sex = null, ClaimsPrincipal user = null)
            => _teacherRepository.GetAllTeacherBySexAge(ageRange, sex, user);

        public Task<DataTablesStructs.ReturnedData<object>> GetTeacherClassAttendanceDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? academicDepartmentId = null, string search = null)
            => _teacherRepository.GetTeacherClassAttendanceDatatable(sentParameters,termId,academicDepartmentId,search);

        public Task<DataTablesStructs.ReturnedData<object>> GetTeacherContractsDatatable(DataTablesStructs.SentParameters sentParameters, int year, string search = null)
            => _teacherRepository.GetTeacherContractsDatatable(sentParameters,year,search);

        public Task<Teacher> GetIgnoreQueryFilter(string id)
            => _teacherRepository.GetIgnoreQueryFilter(id);

        public async Task<DataTablesStructs.ReturnedData<TeacherInformationTemplate>> GetQuantityOfAllTeachersByTermIdDatatable(DataTablesStructs.SentParameters parameters, Guid? termId = null, Guid? academicDepartmentId = null, Guid? dedicationId = null, Guid? categoryId = null, Guid? conditionId = null)
            => await _teacherRepository.GetQuantityOfAllTeachersByTermIdDatatable(parameters, termId, academicDepartmentId, dedicationId, categoryId, conditionId);

        public async Task<object> GetQuantityOfAllTeachersByTermIdChart(Guid? termId = null, Guid? academicDepartmentId = null, Guid? dedicationId = null, Guid? categoryId = null, Guid? conditionId = null)
            => await _teacherRepository.GetQuantityOfAllTeachersByTermIdChart(termId, academicDepartmentId, dedicationId, categoryId, conditionId);

        public async Task<object> GetQuantityOfAllHiredTeachersByTermIdChart(Guid termId)
            => await _teacherRepository.GetQuantityOfAllHiredTeachersByTermIdChart(termId);

        public async Task<object> GetQuantityOfAllTeachersByAcademicDegreeChart(Guid? termId = null, Guid? academicDepartmentId = null)
            => await _teacherRepository.GetQuantityOfAllTeachersByAcademicDegreeChart(termId, academicDepartmentId);

        public async Task<object> GetAllTeacherByDedicationChart(Guid? academicDepartmentId = null, Guid? regimeId = null)
            => await _teacherRepository.GetAllTeacherByDedicationChart(academicDepartmentId, regimeId);

        public Task<DataTablesStructs.ReturnedData<object>> GetTeacherByAcademicDepartmentDatatable(DataTablesStructs.SentParameters sentParameters, Guid? academicDepartmentId = null)
            => _teacherRepository.GetTeacherByAcademicDepartmentDatatable(sentParameters, academicDepartmentId);

        public Task<object> GetTeacherByAcademicDepartmentChart(Guid? academicDepartmentId = null)
            => _teacherRepository.GetTeacherByAcademicDepartmentChart(academicDepartmentId);

        public Task<DataTablesStructs.ReturnedData<object>> GetTeacherForeignDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, bool? foreign = null, ClaimsPrincipal user = null)
            => _teacherRepository.GetTeacherForeignDatatable(sentParameters, termId, foreign, user);

        public Task<object> GetTeacherForeignChart(Guid? termId = null, bool? foreign = null, ClaimsPrincipal user = null)
            => _teacherRepository.GetTeacherForeignChart(termId, foreign, user);

        public Task<TeacherLaborInformationTemplate> GetTeacherLaborTransparencyInformation(string userId)
            => _teacherRepository.GetTeacherLaborTransparencyInformation(userId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetPreProfessionalInternshipSupervisorDatatable(DataTablesStructs.SentParameters parameters, Guid? academicDepartmentId, string search)
            => await _teacherRepository.GetPreProfessionalInternshipSupervisorDatatable(parameters, academicDepartmentId, search);

        public Task<Select2Structs.ResponseParameters> GetTeachersByAcademicDepartmentSelect2(Select2Structs.RequestParameters requestParameters, Guid? id = null, string searchValue = null, int? status = null)
            => _teacherRepository.GetTeachersByAcademicDepartmentSelect2(requestParameters, id, searchValue, status);

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachersManagementDatatable(DataTablesStructs.SentParameters sentParameters, Guid? academicDepartmentId = null, string search = null)
            => await _teacherRepository.GetTeachersManagementDatatable(sentParameters, academicDepartmentId, search);

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachersWithNonTeachingLoadDatatable(DataTablesStructs.SentParameters parameters, Guid termId, string search, Guid? academicDeparmentId = null, bool? viewAll = null, ClaimsPrincipal user = null)
            => await _teacherRepository.GetTeachersWithNonTeachingLoadDatatable(parameters, termId, search, academicDeparmentId, viewAll, user);

        public Task<DataTablesStructs.ReturnedData<object>> GetPercentageTeachersInvestigationProjectDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null)
            => _teacherRepository.GetPercentageTeachersInvestigationProjectDatatable(sentParameters, user);

        public Task<object> GetPercentageTeachersInvestigationProjectChart(ClaimsPrincipal user = null)
            => _teacherRepository.GetPercentageTeachersInvestigationProjectChart(user);
    }   
}