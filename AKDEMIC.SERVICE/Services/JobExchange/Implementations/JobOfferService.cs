using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using AKDEMIC.SERVICE.Services.JobExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Implementations
{
    public class JobOfferService : IJobOfferService
    {
        private readonly IJobOfferRepository _jobOfferRepository;

        public JobOfferService(IJobOfferRepository jobOfferRepository)
        {
            _jobOfferRepository = jobOfferRepository;
        }

        public async Task Delete(JobOffer model)
        {
            await _jobOfferRepository.Delete(model);
        }

        public async Task<JobOffer> Get(Guid id)
        {
            return await _jobOfferRepository.Get(id);
        }

        public Task<List<JobOfferTemplate>> GetAllByCompanyId(Guid companyId, int? status = null, DateTime? endDate = null)
            => _jobOfferRepository.GetAllByCompanyId(companyId, status, endDate);

        public IQueryable<JobOffer> GetAllIQueryable(Guid? careerId = null)
        {
            return _jobOfferRepository.GetAllIQueryable(careerId);
        }
        public IQueryable<JobOffer> GetAllIQueryable(List<Guid> careers)
        {
            return _jobOfferRepository.GetAllIQueryable(careers);
        }

        public Task<DataTablesStructs.ReturnedData<object>> GetJobsOffersCreatedByDatatable(DataTablesStructs.SentParameters sentParameters, string userName, string searchValue = null)
            => _jobOfferRepository.GetJobsOffersCreatedByDatatable(sentParameters, userName, searchValue);

        public Task<DataTablesStructs.ReturnedData<object>> GetHomeJobsOffersDatatable(DataTablesStructs.SentParameters sentParameters)
            => _jobOfferRepository.GetHomeJobsOffersDatatable(sentParameters);

        public async Task<object> GetJobsOffersForCompaniesChart(string searchValue = null, string startSearchDate = null, string endSearchDate = null)
            => await _jobOfferRepository.GetJobsOffersForCompaniesChart(searchValue, startSearchDate, endSearchDate);

        public Task<DataTablesStructs.ReturnedData<object>> GetJobsOffersForCompaniesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, string startSearchDate = null, string endSearchDate = null)
            => _jobOfferRepository.GetJobsOffersForCompaniesDatatable(sentParameters,searchValue,startSearchDate, endSearchDate);

        public async Task<IEnumerable<JobOffer>> GetLastJobOffersAvailable(int take, Guid? careerId = null)
            => await _jobOfferRepository.GetLastJobOffersAvailable(take, careerId);

        public async Task<JobOffer> GetWithIncludes(Guid id)
        {
            return await _jobOfferRepository.GetWithIncludes(id);
        }

        public async Task Insert(JobOffer jobOffer)
        {
            await _jobOfferRepository.Insert(jobOffer);
        }

        public async Task Update(JobOffer jobOffer)
        {
            await _jobOfferRepository.Update(jobOffer);
        }

        public Task<object> GetJobExchangeJobOfferCareerReportChart(List<Guid> careers = null)
            => _jobOfferRepository.GetJobExchangeJobOfferCareerReportChart(careers);

        public Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeJobOfferCareerReportDatatable(DataTablesStructs.SentParameters sentParameters, List<Guid> careers = null)
            => _jobOfferRepository.GetJobExchangeJobOfferCareerReportDatatable(sentParameters, careers);
    }
}
