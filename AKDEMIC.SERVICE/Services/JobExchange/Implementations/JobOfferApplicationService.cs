using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class JobOfferApplicationService : IJobOfferApplicationService
    {
        private readonly IJobOfferApplicationRepository _jobOfferApplicationRepository;

        public JobOfferApplicationService(IJobOfferApplicationRepository jobOfferApplicationRepository)
        {
            _jobOfferApplicationRepository = jobOfferApplicationRepository;
        }

        public Task Delete(JobOfferApplication jobOfferApplication)
            => _jobOfferApplicationRepository.Delete(jobOfferApplication);

        public async Task<IEnumerable<JobOfferApplication>> GetAllByStudent(Guid studentId)
        {
            return await _jobOfferApplicationRepository.GetAllByStudent(studentId);
        }

        public async Task<IEnumerable<JobOfferApplication>> GetAllWithIncludesByStudent(Guid studentId)
        {
            return await _jobOfferApplicationRepository.GetAllWithIncludesByStudent(studentId);
        }

        public Task<List<JobOfferApplicationAgreementReportTemplate>> GetApplicationWithAgreementReportData()
            => _jobOfferApplicationRepository.GetApplicationWithAgreementReportData();

        public async Task<JobOfferApplication> GetByJobOfferAndStudent(Guid jobOfferId, Guid studentId)
        {
            return await _jobOfferApplicationRepository.GetByJobOfferAndStudent(jobOfferId, studentId);
        }

        public async Task<List<JobOfferApplicationTemplate>> GetByJobOfferData(Guid? jobOfferId, DateTime startDate, DateTime endDate, string searchValue = null)
        {
            return await _jobOfferApplicationRepository.GetByJobOfferData(jobOfferId, startDate, endDate, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetByJobOfferDatatable(DataTablesStructs.SentParameters sentParameters, List<Guid> careers, Guid? jobOfferId, DateTime startDate, DateTime endDate, string searchValue = null)
        {
            return await _jobOfferApplicationRepository.GetByJobOfferDatatable(sentParameters,careers, jobOfferId, startDate, endDate, searchValue);
        }

        public Task<List<JobOfferApplication>> GetJobOfferApplications(Guid jobOfferId)
            => _jobOfferApplicationRepository.GetJobOfferApplications(jobOfferId);

        public async Task<object> GetJobOfferApplicationSelect2ClientSide(Guid companyId, Guid? careerId = null)
        {
            return await _jobOfferApplicationRepository.GetJobOfferApplicationSelect2ClientSide(companyId ,careerId);
        }

        public async Task Insert(JobOfferApplication jobOfferApplication)
        {
            await _jobOfferApplicationRepository.Insert(jobOfferApplication);
        }

        public async Task Update(JobOfferApplication jobOfferApplication)
        {
            await _jobOfferApplicationRepository.Update(jobOfferApplication);
        }

        public Task<object> GetJobExchangeStudentWorkTypeReportChart(Guid? facultyId = null, List<Guid> careers = null)
            => _jobOfferApplicationRepository.GetJobExchangeStudentWorkTypeReportChart(facultyId, careers);

        public Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentWorkTypeReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, List<Guid> careers = null)
            => _jobOfferApplicationRepository.GetJobExchangeStudentWorkTypeReportDatatable(sentParameters, facultyId, careers);

        public Task<object> GetJobExchangeJobOfferApplicationCareerReportChart(List<Guid> careers = null)
            => _jobOfferApplicationRepository.GetJobExchangeJobOfferApplicationCareerReportChart(careers);
        public Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeJobOfferApplicationCareerReportDatatable(DataTablesStructs.SentParameters sentParameters, List<Guid> careers = null)
            => _jobOfferApplicationRepository.GetJobExchangeJobOfferApplicationCareerReportDatatable(sentParameters, careers);
    }
}
