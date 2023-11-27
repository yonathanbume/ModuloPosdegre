using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.JobExchange.Interfaces
{
    public interface IJobOfferService
    {
        IQueryable<JobOffer> GetAllIQueryable(Guid? careerId = null);
        IQueryable<JobOffer> GetAllIQueryable(List<Guid> careers);
        Task Insert(JobOffer jobOffer);
        Task Update(JobOffer jobOffer);
        Task<JobOffer> Get(Guid id);
        Task Delete(JobOffer model);
        Task<JobOffer> GetWithIncludes(Guid id);
        Task<List<JobOfferTemplate>> GetAllByCompanyId(Guid companyId, int? status = null, DateTime? endDate = null);
        Task<IEnumerable<JobOffer>> GetLastJobOffersAvailable(int take, Guid? careerId = null);
        Task<object> GetJobsOffersForCompaniesChart(string searchValue = null,string startSearchDate = null, string endSearchDate = null);
        Task<DataTablesStructs.ReturnedData<object>> GetHomeJobsOffersDatatable(DataTablesStructs.SentParameters sentParameters);
        Task<DataTablesStructs.ReturnedData<object>> GetJobsOffersForCompaniesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, string startSearchDate = null, string endSearchDate = null);
        Task<DataTablesStructs.ReturnedData<object>> GetJobsOffersCreatedByDatatable(DataTablesStructs.SentParameters sentParameters,string userName, string searchValue = null);

        Task<object> GetJobExchangeJobOfferCareerReportChart(List<Guid> careers = null);
        Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeJobOfferCareerReportDatatable(DataTablesStructs.SentParameters sentParameters, List<Guid> careers = null);

    }
}
