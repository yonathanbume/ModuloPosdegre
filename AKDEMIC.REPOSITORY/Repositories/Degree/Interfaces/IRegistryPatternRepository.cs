using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Degree;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Degree.Templates;
using AKDEMIC.REPOSITORY.Repositories.Degree.Templates.Diploma;
using AKDEMIC.REPOSITORY.Repositories.Degree.Templates.RegistryPattern;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using static AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates.ProfileDetailTemplate;

namespace AKDEMIC.REPOSITORY.Repositories.Degree.Interfaces
{
    public interface IRegistryPatternRepository: IRepository<RegistryPattern>
    {
        Task<RegistryPattern> GetWithIncludes(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetUserProcedureByRegistryPattern(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> UserProceduresGenerateByRegistryPattern(DataTablesStructs.SentParameters sentParameters, Guid? facultyId, Guid? careerId, Expression<Func<RegistryPattern, dynamic>> selectPredicate = null, Func<RegistryPattern, string[]> searchValuePredicate = null, string searchValue = null);
        Task<RegistryPatternTemplate> GetRegistryPatternTemplate(Guid id);
        Task UpdateRegistryPatternTemplate(RegistryPatternTemplate model);
        Task DownloadExcel(IXLWorksheet worksheet, Guid? facultyId, Guid? careerId, Guid? academicProgramId, string dni = null, string searchBookNumber = null, string dateStartFilter = null, string dateEndFilter = null, int ? type = null, int? clasification = null, bool toSunedu = false);
        Task<List<DegreeRelationTemplate>> GetRelationDegreesByProcedure(DateTime startDateTime, DateTime endDateTime, List<Guid> resultsToSearch, string bookCode);
        Task<List<DegreeRelationTemplate>> GetRelationDegreesByConcept(DateTime startDateTime, DateTime endDateTime, List<Guid> resultsToSearch);
        Task<List<RegistryPattern>> GetAllByStudent(Guid studentId);
        Task<RegistryPattern> GetByStudentIdAndGradeType(Guid studentId, int gradeType);
        Task<int> RegistryPattern3MonthAgoCount();
        Task<int> Diploma3MonthAgoCount();
        Task<RegistryPatternConceptStudentTemplate> GetRegistryPatternConceptByStudentId(Guid studentId);
        Task<RegistryPatternProcedureStudentTemplate> GetRegistryPatternProcedureByStudentId(Guid studentId);
        Task<List<DegreeDate>> GetAllByStudentTemplate(Guid studentId);
        Task<DataTablesStructs.ReturnedData<object>> GetRegistryPatternDatatableByConfiguration(DataTablesStructs.SentParameters sentParameters, Guid? facultyId, Guid? careerId, Guid? academicProgramId, string searchBookNumber = null , string dateStartFilter = null , string dateEndFilter = null, string searchValue = null, string searchFullName = null , string searchDNI = null , int? status = null, int? type = null, int? diplomaStatus = null, string searchDiplomaNumber = null, string officeNumber = null, int? clasification = null);
        Task<RegistryPattern> GetRegistryPatternBasedGradeReport(Guid studentId, int gradeType);
        Task<DataTablesStructs.ReturnedData<object>> GetRegistryPatternApprovedDatatableByConfiguration(DataTablesStructs.SentParameters sentParameters, Guid? careerId, string searchBookNumber, string dateStartFilter, string dateEndFilter, string searchValue, int? status);
        Task ApprovedAll(Guid? careerId, string searchBookNumber, string dateStartFilter, string dateEndFilter, string searchValue);

        Task<DataTablesStructs.ReturnedData<object>> GetRelationDegreeDatatable(DataTablesStructs.SentParameters sentParameters, DateTime startDate, DateTime endDate, string bookCode , string searchValue = null);
        Task<DiplomaPdfReportTemplate> GetPdfReport(Guid id);
        Task<List<FormatRegisterTemplate>> GetFormatRegisterTemplate(string searchValue , string searchBookNumber, string dateStartFilter, string dateEndFilter);
        Task<object> GetReportProfessionalTitleByModality(string dateStartFilter, string dateEndFilter, List<Guid> Careers = null);


        Task<DataTablesStructs.ReturnedData<object>> GetDiplomasByDegreeModality(DataTablesStructs.SentParameters sentParameters, string degreeModality, ClaimsPrincipal user = null);
        Task<object> GetDiplomasByDegreeModalityChart(string degreeModality, ClaimsPrincipal user = null);
        Task<List<RegistryPattern>> GetRegistryPatternsListByBookNumber(string bookNumber);
        Task<int> CurrentCountByGradeType(int gradeType);
        Task<object> GetStudentBasicInformation(Guid registryPatternId);
        Task<RegistryPatternBook> GetRegistryPatternBook();
        Task<List<RegistryPatternReportTemplate>> GetRegistryPatternData();

        Task SaveChangesAsync();
    }
}
