using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class JobOfferCareerService : IJobOfferCareerService
    {
        private readonly IJobOfferCareerRepository _jobOfferCareerRepository;

        public JobOfferCareerService(IJobOfferCareerRepository jobOfferCareerRepository)
        {
            _jobOfferCareerRepository = jobOfferCareerRepository;
        }

        public async Task DeleteRange(IEnumerable<JobOfferCareer> jobOfferCareers)
        {
             await _jobOfferCareerRepository.DeleteRange(jobOfferCareers);
        }

        public async Task<IEnumerable<JobOfferCareer>> GetAllByJobOfferId(Guid jobOfferId)
        {
            return await _jobOfferCareerRepository.GetAllByJobOfferId(jobOfferId);
        }

        public async Task<object> GetJobOfferByCareersAsData(Guid? careerId = null, string startSearchDate = null, string endSearchDate = null, ClaimsPrincipal user = null)
        {
            return await _jobOfferCareerRepository.GetJobOfferByCareersAsData(careerId, startSearchDate, endSearchDate, user);
        }

        public async Task<object> JobOfferReport1(bool isCoordinator, List<Guid> careers)
        {
            return await _jobOfferCareerRepository.JobOfferReport1(isCoordinator,careers);
        }

        public async Task<object> JobOfferReport2(bool isCoordinator, List<Guid> careers)
        {
            return await _jobOfferCareerRepository.JobOfferReport2(isCoordinator, careers);
        }

        public async Task JobOfferReport2Excel(IXLWorksheet worksheet, ClaimsPrincipal user, List<Guid> careers)
        {
             await _jobOfferCareerRepository.JobOfferReport2Excel(worksheet, user, careers);
        }

        public Task<List<JobOfferApplicationExcelReportTemplate>> JobOfferReport2ExcelData(ClaimsPrincipal user, List<Guid> careers)
            => _jobOfferCareerRepository.JobOfferReport2ExcelData(user, careers);
    }
}
