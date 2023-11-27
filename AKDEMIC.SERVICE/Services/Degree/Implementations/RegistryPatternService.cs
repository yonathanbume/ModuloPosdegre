using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Degree;
using AKDEMIC.REPOSITORY.Repositories.Degree.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Degree.Templates;
using AKDEMIC.REPOSITORY.Repositories.Degree.Templates.Diploma;
using AKDEMIC.REPOSITORY.Repositories.Degree.Templates.RegistryPattern;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using AKDEMIC.SERVICE.Services.Degree.Interfaces;
using ClosedXML.Excel;

namespace AKDEMIC.SERVICE.Services.Degree.Implementations
{
    public class RegistryPatternService : IRegistryPatternService
    {
        private readonly IRegistryPatternRepository _registryPatternRepository;

        public RegistryPatternService(IRegistryPatternRepository registryPatternRepository)
        {
            _registryPatternRepository = registryPatternRepository;
        }

        public async Task<RegistryPatternTemplate> GetRegistryPatternTemplate(Guid id)
        {
            return await _registryPatternRepository.GetRegistryPatternTemplate(id);
        }

        public async Task DownloadExcel(IXLWorksheet worksheet, Guid? facultyId, Guid? careerId, Guid? academicProgramId, string dni = null, string searchBookNumber = null, string dateStartFilter = null, string dateEndFilter = null, int? type = null, int? clasification = null, bool toSunedu = false)
        {
            await _registryPatternRepository.DownloadExcel(worksheet, facultyId, careerId, academicProgramId, dni, searchBookNumber, dateStartFilter, dateEndFilter, type,clasification,toSunedu);
        }

        public async Task<List<RegistryPattern>> GetAllByStudent(Guid studentId)
        {
            return await _registryPatternRepository.GetAllByStudent(studentId);
        }

        public async Task<List<DegreeRelationTemplate>> GetRelationDegreesByConcept(DateTime startDateTime, DateTime endDateTime, List<Guid> resultsToSearch)
        {
            return await _registryPatternRepository.GetRelationDegreesByConcept(startDateTime, endDateTime, resultsToSearch);
        }

        public async Task<List<DegreeRelationTemplate>> GetRelationDegreesByProcedure(DateTime startDateTime, DateTime endDateTime, List<Guid> resultsToSearch, string bookCode)
        {
            return await _registryPatternRepository.GetRelationDegreesByProcedure(startDateTime, endDateTime, resultsToSearch, bookCode);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserProcedureByRegistryPattern(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _registryPatternRepository.GetUserProcedureByRegistryPattern(sentParameters, searchValue);
        }

        public async Task<int> RegistryPattern3MonthAgoCount()
        {
            return await _registryPatternRepository.RegistryPattern3MonthAgoCount();
        }
        public async Task<int> Diploma3MonthAgoCount()
        {
            return await _registryPatternRepository.Diploma3MonthAgoCount();
        }

        public async Task UpdateRegistryPatternTemplate(RegistryPatternTemplate model)
        {
            await _registryPatternRepository.UpdateRegistryPatternTemplate(model);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> UserProceduresGenerateByRegistryPattern(DataTablesStructs.SentParameters sentParameters, Guid? facultyId, Guid? careerId, Expression<Func<RegistryPattern, dynamic>> selectPredicate = null, Func<RegistryPattern, string[]> searchValuePredicate = null, string searchValue = null)
        {
            return await _registryPatternRepository.UserProceduresGenerateByRegistryPattern(sentParameters, facultyId, careerId, selectPredicate, searchValuePredicate, searchValue);
        }

        public async Task<RegistryPatternConceptStudentTemplate> GetRegistryPatternConceptByStudentId(Guid studentId)
            => await _registryPatternRepository.GetRegistryPatternConceptByStudentId(studentId);

        public async Task<RegistryPatternProcedureStudentTemplate> GetRegistryPatternProcedureByStudentId(Guid studentId)
            => await _registryPatternRepository.GetRegistryPatternProcedureByStudentId(studentId);

        public async Task<List<ProfileDetailTemplate.DegreeDate>> GetAllByStudentTemplate(Guid studentId)
        {
            return await _registryPatternRepository.GetAllByStudentTemplate(studentId);
        }

        public async Task Insert(RegistryPattern model)
        {
            await _registryPatternRepository.Insert(model);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetRegistryPatternDatatableByConfiguration(DataTablesStructs.SentParameters sentParameters, Guid? facultyId, Guid? careerId, Guid? academicProgramId, string searchBookNumber = null, string dateStartFilter = null, string dateEndFilter = null, string searchValue = null, string searchFullName = null, string searchDNI = null, int? status = null, int? type = null, int? diplomaStatus = null, string searchDiplomaNumber = null, string officeNumber = null, int? clasification = null)
        {
            return await _registryPatternRepository.GetRegistryPatternDatatableByConfiguration(sentParameters, facultyId, careerId, academicProgramId, searchBookNumber, dateStartFilter, dateEndFilter, searchValue, searchFullName, searchDNI, status, type, diplomaStatus, searchDiplomaNumber,officeNumber,clasification);
        }

        public async Task<RegistryPattern> Get(Guid id)
            => await _registryPatternRepository.Get(id);

        public async Task<RegistryPattern> GetWithIncludes(Guid id)
            => await _registryPatternRepository.GetWithIncludes(id);

        public async Task<RegistryPattern> GetRegistryPatternBasedGradeReport(Guid studentId, int gradeType)
        {
            return await _registryPatternRepository.GetRegistryPatternBasedGradeReport(studentId, gradeType);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetRegistryPatternApprovedDatatableByConfiguration(DataTablesStructs.SentParameters sentParameters, Guid? careerId, string searchBookNumber = null, string dateStartFilter = null, string dateEndFilter = null, string searchValue = null, int? status = null)
        {
            return await _registryPatternRepository.GetRegistryPatternApprovedDatatableByConfiguration(sentParameters, careerId, searchBookNumber, dateStartFilter, dateEndFilter, searchValue, status);
        }

        public async Task Update(RegistryPattern model)
        {
            await _registryPatternRepository.Update(model);
        }

        public async Task ApprovedAll(Guid? careerId, string searchBookNumber, string dateStartFilter, string dateEndFilter, string searchValue)
        {
            await _registryPatternRepository.ApprovedAll(careerId, searchBookNumber, dateStartFilter, dateEndFilter, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetRelationDegreeDatatable(DataTablesStructs.SentParameters sentParameters, DateTime startDateTime, DateTime endDateTime, string bookCode, string searchValue = null)
        {
            return await _registryPatternRepository.GetRelationDegreeDatatable(sentParameters, startDateTime, endDateTime, bookCode, searchValue);
        }

        public async Task<DiplomaPdfReportTemplate> GetPdfReport(Guid id)
        {
            return await _registryPatternRepository.GetPdfReport(id);
        }

        public async Task<List<FormatRegisterTemplate>> GetFormatRegisterTemplate(string searchValue , string searchBookNumber, string dateStartFilter, string dateEndFilter)
        {
            return await _registryPatternRepository.GetFormatRegisterTemplate(searchValue, searchBookNumber, dateStartFilter, dateEndFilter);
        }

        public async Task<object> GetReportProfessionalTitleByModality(string dateStartFilter, string dateEndFilter, List<Guid> Careers = null)
        {
            return await _registryPatternRepository.GetReportProfessionalTitleByModality(dateStartFilter, dateEndFilter, Careers);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDiplomasByDegreeModality(DataTablesStructs.SentParameters sentParameters, string degreeModality, ClaimsPrincipal user = null)
            => await _registryPatternRepository.GetDiplomasByDegreeModality(sentParameters , degreeModality, user);

        public async Task<object> GetDiplomasByDegreeModalityChart(string degreeModality, ClaimsPrincipal user = null)
            => await _registryPatternRepository.GetDiplomasByDegreeModalityChart(degreeModality, user);

        public async Task<List<RegistryPattern>> GetRegistryPatternsListByBookNumber(string bookNumber)
        {
            return await _registryPatternRepository.GetRegistryPatternsListByBookNumber(bookNumber);
        }
        public async Task<int> CurrentCountByGradeType(int gradeType)
        {
            return await _registryPatternRepository.CurrentCountByGradeType(gradeType);
        }

        public async Task<object> GetStudentBasicInformation(Guid registryPatternId)
        {
            return await _registryPatternRepository.GetStudentBasicInformation(registryPatternId);
        }

        public async Task<RegistryPatternBook> GetRegistryPatternBook()
            => await _registryPatternRepository.GetRegistryPatternBook();

        public Task<RegistryPattern> GetByStudentIdAndGradeType(Guid studentId, int gradeType)
            => _registryPatternRepository.GetByStudentIdAndGradeType(studentId, gradeType);

        public Task<RegistryPattern> Add(RegistryPattern model)
            => _registryPatternRepository.Add(model);

        public Task<List<RegistryPatternReportTemplate>> GetRegistryPatternData()
            => _registryPatternRepository.GetRegistryPatternData();

        public Task<IEnumerable<RegistryPattern>> GetAll()
            => _registryPatternRepository.GetAll();

        public Task SaveChangesAsync()
            => _registryPatternRepository.SaveChangesAsync();
    }
}
