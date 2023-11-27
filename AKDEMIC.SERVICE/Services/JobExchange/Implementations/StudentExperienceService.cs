using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class StudentExperienceService : IStudentExperienceService
    {
        private readonly IStudentExperienceRepository _studentExperienceRepository;

        public StudentExperienceService(IStudentExperienceRepository studentExperienceRepository)
        {
            _studentExperienceRepository = studentExperienceRepository;
        }

        public async Task<bool> ExistStudentExperienceByCompany(Guid companyId, Guid StudentId)
        {
            return await _studentExperienceRepository.ExistStudentExperienceByCompany(companyId, StudentId);
        }

        public async Task<StudentExperience> FirstOrDefaultById(Guid id)
        {
            return await _studentExperienceRepository.FirstOrDefaultById(id);
        }

        public async Task<List<ProfileDetailTemplate.ExperienceDate>> GetAllByStudentTemplate(Guid studentId)
        {
            return await _studentExperienceRepository.GetAllByStudentTemplate(studentId);
        }

        public async Task<StudentExperience> GetLastByStartDate()
        {
            return await _studentExperienceRepository.GetLastByStartDate();
        }

        public async Task<object> GetStudentExperiencePersonalized(Guid studentexperienceId)
        {
            return await _studentExperienceRepository.GetStudentExperiencePersonalized(studentexperienceId);
        }

        public async Task<object> GetStudentExperiencesByCompanyIdSelect2ClientSide(Guid companyId)
        {
            return await _studentExperienceRepository.GetStudentExperiencesByCompanyIdSelect2ClientSide(companyId);
        }

        public async Task<object> GetStudentExperiencesById(Guid id)
        {
            return await _studentExperienceRepository.GetStudentExperiencesById(id);
        }

        public async Task<object> GetStudentExperiencesByStudent(Guid studentId)
        {
            return await _studentExperienceRepository.GetStudentExperiencesByStudent(studentId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentWorkingDatatable(DataTablesStructs.SentParameters sentParameters, Guid? companyId, Guid? careerId, string searchValue = null)
        {
            return await _studentExperienceRepository.GetStudentWorkingDatatable(sentParameters, companyId, careerId, searchValue);
        }

        public async Task<object> GetWorkingBachelorsChart(string startSearchDate = null, string endSearchDate = null)
            => await _studentExperienceRepository.GetWorkingBachelorsChart(startSearchDate, endSearchDate);

        public async Task<StudentExperience> GetYearWorking()
        {
            return await _studentExperienceRepository.GetYearWorking();
        }

        public async Task Insert(StudentExperience studentExperience)
        {
            await _studentExperienceRepository.Insert(studentExperience);
        }

        public async Task Remove(StudentExperience studentExperience)
        {
            await _studentExperienceRepository.Delete(studentExperience);
        }

        public async Task Update(StudentExperience studentExperience)
        {
            await _studentExperienceRepository.Update(studentExperience);
        }

        public Task<object> GetJobExchangeStudentExperienceCareerReportChart(List<Guid> careers = null)
            => _studentExperienceRepository.GetJobExchangeStudentExperienceCareerReportChart(careers);

        public Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentExperienceCareerReportDatatable(DataTablesStructs.SentParameters sentParameters, List<Guid> careers = null)
            => _studentExperienceRepository.GetJobExchangeStudentExperienceCareerReportDatatable(sentParameters, careers);

        public Task<object> GetJobExchangeStudentExperienceSectorReportChart()
            => _studentExperienceRepository.GetJobExchangeStudentExperienceSectorReportChart();
        public Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentExperienceSectorReportDatatable(DataTablesStructs.SentParameters sentParameters)
            => _studentExperienceRepository.GetJobExchangeStudentExperienceSectorReportDatatable(sentParameters);
    }
}