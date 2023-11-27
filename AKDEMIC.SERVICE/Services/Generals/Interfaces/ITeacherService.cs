using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Teacher;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Education;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.TeacherDedication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface ITeacherService
    {
        Task<int> Count();
        Task<Teacher> GetTeacherWithData(string userId);
        void Remove(Teacher teacher);
        Task DeleteAsync(Teacher teacher);
        Task<Select2Structs.ResponseParameters> GetFacultyTachersByAcademicProgramId(Select2Structs.RequestParameters requestParameters, Guid academicProgramId, string keyname);
        Task<Teacher> GetAsync(string id);
        Task<Teacher> GetIgnoreQueryFilter(string id);
        Task<Teacher> GetWithTeacherDedication(string id);
        Task<IEnumerable<Teacher>> GetAllWithUser();
        Task<IEnumerable<Teacher>> GetAll();
        Task<DataTablesStructs.ReturnedData<object>> GetTeacherDatatableByLevelStudy(DataTablesStructs.SentParameters sentParameters, int? levelStudy, string search = null);
        Task<DataTablesStructs.ReturnedData<object>> GetTeacherDatatableByAgeSex(DataTablesStructs.SentParameters sentParameters ,int ? ageRange = null , int? sex = null, string search = null, ClaimsPrincipal user = null);
        Task<List<Teacher>> GetAllByCareers(List<Guid> careers);
        Task<int> CountByCareers(List<Guid> careers);
        Task AddAsync(Teacher teacher);
        Task<object> GetAllAsSelect2ClientSide(Guid? facultyId = null, string keyWord = null, Guid? careerId = null, string coordinatorId = null, Guid? academicDepartmentId = null);
        Task<TeacherLaborInformationTemplate> GetTeacherLaborTransparencyInformation(string userId);
        Task<DataTablesStructs.ReturnedData<object>> GetTeachingPlanDataTable(DataTablesStructs.SentParameters paginationParameter, Guid termId ,string search);
        Task<DataTablesStructs.ReturnedData<object>> GetOlderTeacherDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
        Task<List<ConditionAndDedicationTemplate>> GetAllByConditionAndDedication(Guid? condition, Guid? dedication, Guid? careerId, Guid? category,string search, string coordinatorId = null, Guid? academicDepartmentId = null, Guid? regimeId = null);
        Task<DataTablesStructs.ReturnedData<ConditionAndDedicationTemplate>> GetAllByConditionAndDedicationDataTable(DataTablesStructs.SentParameters sentParameters,Guid? condition, Guid? dedication, Guid? careerId, Guid? category, string search, string coordinatorId = null, Guid? academicDepartmentId = null, Guid? regimeId = null, int? maxStudeyLevel = null);
        Task<object> CountByAgeRangeAndSex(int? ageRange = null, int? sex = null);
        Task<IEnumerable<TeacherTemplateA>> GetTeachersModelA(Guid? facultyId, string coordinatoriId = null, Guid? academicDepartmentId = null, ClaimsPrincipal user = null);
        Task<IEnumerable<TeacherPlusAssitance>> GetAllPlusAssitance(Guid? careerId = null, string secretaryId = null);
        Task<string> GetTeacherFullNameById(string userId);
        Task<object> GetAllByTermandCareer(Guid termId, Guid? careerId, string coordinatorId = null, byte? status = null, ClaimsPrincipal user = null, Guid? academicDepartmentId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAllByTermandCareer2(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? careerId, string coordinatorId = null, byte? status = null,string searchValue = null, Guid? academicDepartmentId = null, ClaimsPrincipal user = null, byte? sex = null, bool? viewAll = false);
        Task<object> GetAllAsModelC();
        Task<Select2Structs.ResponseParameters> GetTeachersByFacultySelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null, Guid? facultyId = null, bool? isActive = null);
        Task<IEnumerable<Teacher>> GetPerformanceByCareerAndTermAndWeekAndDedication(Guid termId,
            int week, Guid? facultyId = null, Guid? teacherDedicationId = null);

        Task<object> CountPerformanceByCareerAndTermAndWeekAndDedication(Guid termId,
            int? week = null, Guid? facultyId = null, Guid? teacherDedicationId = null);

        Task<object> CountByLevelStudy();
        Task<Select2Structs.ResponseParameters> GetTeachersSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<Teacher, Select2Structs.Result>> selectPredicate = null, Func<Teacher, string[]> searchValuePredicate = null, string searchValue = null, int? status = null);
        Task<Select2Structs.ResponseParameters> GetTeachersByCareer(Select2Structs.RequestParameters requestParameters, Guid id, Expression<Func<Teacher, Select2Structs.Result>> selectPredicate = null, Func<Teacher, string[]> searchValuePredicate = null, string searchValue = null, int? status = null);
        Task<DataTablesStructs.ReturnedData<object>> GetTeachersDatatable(DataTablesStructs.SentParameters sentParameters, Guid? faculty, Guid? careerId, string search = null, Guid? termId = null, Guid? academicDepartment = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetIntranetTeachersDatatable(DataTablesStructs.SentParameters sentParameters,Guid? academicDepartmentId = null, string search = null, ClaimsPrincipal user = null, Guid? termId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetTeacherByDedicationDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? dedicationId = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAllEscalafonReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? academicDepartmentId = null, Guid? conditionId = null, Guid? dedicationId = null, Guid? categoryId = null, Guid? capPositionId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetStudiesDatatableByUser(DataTablesStructs.SentParameters sentParameters, string UserId);
        Task<object> GetTeacherSelect2Report();
        Task<Select2Structs.ResponseParameters> GetTeachersSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null, Guid? careerId = null, Guid? academicDepartmentId = null, ClaimsPrincipal user = null);
        Task<IEnumerable<Select2Structs.Result>> GetTeachersSelect2ClientSide(Guid? facultyId = null, Guid? careerId = null);
        Task<object> GetTeacherSelectByAcademicDepartment(Guid? facultyId = null, Guid? academicDepartmentId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetTeachersByCareersDatatable(DataTablesStructs.SentParameters sentParameters, List<Guid> careers);
        Task<DataTablesStructs.ReturnedData<object>> GetTeachersBirthdayThisMonth(DataTablesStructs.SentParameters sentParameters);
        Task<DataTablesStructs.ReturnedData<object>> GetTeachersContractThisMonth(DataTablesStructs.SentParameters sentParameters);
        Task<DataTablesStructs.ReturnedData<object>> GetTeachersLicenseThisMonth(DataTablesStructs.SentParameters sentParameters);
        Task<TeacherTemplateA> GetAsTemplateAById(string teacherId);
        Task InsertAsync(Teacher teacher);
        Task UpdateAsync(Teacher teacher);
        Task<int> GetTeachersEnrolledCountByTermIdAndLevelStudie(Guid termId, int levelStudy);
        Task<int> GetTeachersEnrolledCountByTermId(Guid termId, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetByCategoryAndTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? categoryId = null, ClaimsPrincipal user = null);
        Task<object> GetByCategoryAndTermChart(Guid? termId = null, Guid? categoryId = null, ClaimsPrincipal user = null);
        Task<object> GetMagisterByAcademicDepartmentChart(Guid? academicDepartmentId = null, ClaimsPrincipal user = null);
        Task<object> GetDoctorsByAcademicDepartmentChart(Guid? academicDepartmentId = null, ClaimsPrincipal user = null);
        Task<object> GetSecondSpecialityByAcademicDepartmentChart(Guid? academicDepartmentId = null, ClaimsPrincipal user = null);      
        Task<DataTablesStructs.ReturnedData<object>> GetTeacherByCountryDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? countryId = null );
        Task<DataTablesStructs.ReturnedData<object>> GetTeacherForeignDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, bool? foreign = null, ClaimsPrincipal user = null);
        Task<object> GetTeacherByCountryChart(Guid? termId = null, Guid? countryId = null);
        Task<object> GetTeacherForeignChart(Guid? termId = null, bool? foreign = null, ClaimsPrincipal user = null);
        Task<int> GetRegisteredTeachersCountByTermId(Guid termId);
        Task<object> GetTeachersJson(string q);
        Task<Teacher> GetByUserId(string userId);
        Task<object> GetTeacherJsonByUserId(string userId);
        Task<object> GetTeachersByIdJson(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetTeachersWithInvestigationProjectDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<object>> GetPercentageTeachersInvestigationProjectDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null);
        Task<object> GetPercentageTeachersInvestigationProjectChart(ClaimsPrincipal user = null);
        Task<object> GetTeacherByDedicationChart(Guid? termId = null, Guid? dedicationId = null, ClaimsPrincipal user = null);
        Task<DataTablesStructs.ReturnedData<Teacher>> GetTeacherByClaimDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, Guid? facultyId = null, string search = null);
        Task<DataTablesStructs.ReturnedData<object>> GetTeachersToAssignCourses(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, string searchValue = null, ClaimsPrincipal user = null, Guid? academicDepartmentId = null);
        Task<object> SearchTeacherByTerm(string term);
        Task<object> SearchTeacherByTerm(string term, List<string> filteredUsers);
        Task<DataTablesStructs.ReturnedData<object>> GetDismissalTeacherDatatable(DataTablesStructs.SentParameters sentParameters, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetTeacherByAgeRange(DataTablesStructs.SentParameters sentParameters, string search, int minAge , int maxAge);
        Task<DataTablesStructs.ReturnedData<object>> GetStudiesByStudyLevelDatatable(DataTablesStructs.SentParameters sentParameters, string search, int ? studyLevel = 0, ClaimsPrincipal user = null, string expeditionDate = null, Guid? countryId = null, Guid? institutionId = null, Guid? termId =null);
        Task<Select2Structs.ResponseParameters> GetTeachersByCareerSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null, int? status = null);
        Task<Select2Structs.ResponseParameters> GetTeachersByAcademicDepartmentSelect2(Select2Structs.RequestParameters requestParameters, Guid? id = null, string searchValue = null, int? status = null);
        Task<DataTablesStructs.ReturnedData<object>> GetTeacherForAsistanceDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId, Guid? academicDepartmentId = null, ClaimsPrincipal user = null, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetTeacherClassesWithoutAttendance(DataTablesStructs.SentParameters parameters, Guid termId, Guid? academicDepartmentId, string search, DateTime endTime, ClaimsPrincipal user);
        Task<List<ScheduleTemplate>> GetTeacherCompleteSchedule(Guid termId, string teacherId, DateTime start, DateTime end);
        Task<List<UserGenericStudiesTemplate>> GetAllUserGenericStudies(int? studyLevel = 0, ClaimsPrincipal user = null, Guid? termId = null);
        Task<List<TeacherReportTemplate>> GetAllTeacherBySexAge(int? ageRange = null, int? sex = null, ClaimsPrincipal user = null);

        Task<DataTablesStructs.ReturnedData<object>> GetTeacherByAcademicDepartmentDatatable(DataTablesStructs.SentParameters sentParameters, Guid? academicDepartmentId = null);
        Task<object> GetTeacherByAcademicDepartmentChart(Guid? academicDepartmentId = null);


        Task<DataTablesStructs.ReturnedData<object>> GetTeacherClassAttendanceDatatable(DataTablesStructs.SentParameters sentParameters,Guid? termId = null , Guid? academicDepartmentId = null , string search = null);
        Task<DataTablesStructs.ReturnedData<object>> GetTeacherContractsDatatable(DataTablesStructs.SentParameters sentParameters, int year, string search = null);
        Task<DataTablesStructs.ReturnedData<TeacherInformationTemplate>> GetQuantityOfAllTeachersByTermIdDatatable(DataTablesStructs.SentParameters parameters, Guid? termId = null, Guid? academicDepartmentId = null, Guid? dedicationId = null, Guid? categoryId = null, Guid? conditionId = null);
        Task<object> GetQuantityOfAllTeachersByTermIdChart(Guid? termId = null, Guid? academicDepartmentId = null, Guid? dedicationId = null, Guid? categoryId = null, Guid? conditionId = null);
        Task<object> GetQuantityOfAllHiredTeachersByTermIdChart(Guid termId);
        Task<object> GetQuantityOfAllTeachersByAcademicDegreeChart(Guid? termId = null, Guid? academicDepartmentId = null);
        Task<object> GetAllTeacherByDedicationChart(Guid? academicDepartmentId = null, Guid? regimeId = null);
        Task<DataTablesStructs.ReturnedData<object>> GetPreProfessionalInternshipSupervisorDatatable(DataTablesStructs.SentParameters parameters, Guid? academicDepartmentId, string search);
        Task<DataTablesStructs.ReturnedData<object>> GetTeachersManagementDatatable(DataTablesStructs.SentParameters sentParameters, Guid? academicDepartmentId = null, string search = null);
        Task<DataTablesStructs.ReturnedData<object>> GetTeachersWithNonTeachingLoadDatatable(DataTablesStructs.SentParameters parameters, Guid termId, string search, Guid? academicDepartmentId = null, bool? viewAll = null, ClaimsPrincipal user = null);
    }
}