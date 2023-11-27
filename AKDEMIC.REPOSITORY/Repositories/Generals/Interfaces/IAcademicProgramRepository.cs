using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.AcademicProgram;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface IAcademicProgramRepository : IRepository<AcademicProgram>
    {
        Task<Select2Structs.ResponseParameters> GetAcademicProgramSelect2(Select2Structs.RequestParameters requestParameters, Guid? selectedId, string searchValue = null);
        Task<Select2Structs.ResponseParameters> GetAcademicProgramByCareerSelect2(Select2Structs.RequestParameters requestParameters, Guid? selectedId, Guid? careerId, string searchValue = null);
        Task<bool> AnyByName(string name, Guid? id = null);
        Task<object> GetAllAsModelA(Guid? facultyId = null, Guid? careerId = null, string coordinatorId = null);
        Task<object> GetAsModelB(Guid? id = null);
        Task<DataTablesStructs.ReturnedData<object>> AcademicProgramsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? curriculumId = null);
        Task<object> GetAcademicProgramsSelect(Guid cid, bool HasAll = false, List<Guid> CareerIds = null);
        Task<IEnumerable<Select2Structs.Result>> GetAcademicProgramByCareerSelect2ClientSide(Guid? careerId = null, Guid? selectedId = null);
        Task<object> GetCareerAcademicProgramsJson(Guid id, bool onlyWithCourses = false);
        Task<IEnumerable<AcademicProgram>> GetAllByCareer(Guid careerId);
        Task<AcademicProgram> GetByCode(string code, Guid? careerId = null);
        Task<object> GetCurriculumAcademicProgramsJson(Guid id);
        Task<object> GetCareerAcademicProgramsByPlan(Guid id);
        Task<AcademicProgramReportTemplate> GetAcademicProgramsReportData(Guid termId, Guid facultyId, Guid careerId);
        Task<DataTablesStructs.ReturnedData<AcademicProgramModelATemplate>> GetAllAsModelADataTable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, Guid? careerId = null, string coordinatorId = null, string search = null,ClaimsPrincipal user = null);
        Task LoadCurriculumAcademicProgramJob();
        Task LoadStudentsAcademicProgramJob();
        Task<object> GetAcademicProgramByCoursesSelect2CliendSide(Guid careerId);
        Task<object> GetAcademicProgramByCampusIdSelect2ClientSide(Guid campusId, Guid? selectedId = null);
        Task<object> GetCareersToForum();
        Task<List<AcademicProgram>> GetAcademicProgramsByCurriculumId(Guid curriculumId);
        Task<DataTablesStructs.ReturnedData<object>> GetEnrolledStudentsByAcademicProgram(DataTablesStructs.SentParameters sentParameters, Guid termId, bool onlyWithStudents = false);
    }
}