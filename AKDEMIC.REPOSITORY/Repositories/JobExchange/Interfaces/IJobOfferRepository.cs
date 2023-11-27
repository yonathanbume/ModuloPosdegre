using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces
{
    public interface IJobOfferRepository: IRepository<JobOffer>
    {
        IQueryable<JobOffer> GetAllIQueryable(Guid? careerId = null);
        IQueryable<JobOffer> GetAllIQueryable(List<Guid> careers);
        Task<JobOffer> GetWithIncludes(Guid id);
        Task<IEnumerable<JobOffer>> GetLastJobOffersAvailable(int take, Guid? careerId = null);
        Task<List<JobOfferTemplate>> GetAllByCompanyId(Guid companyId, int? status = null, DateTime? endDate = null);
        Task<object> GetJobsOffersForCompaniesChart(string searchValue = null, string startSearchDate = null, string endSearchDate = null);
        Task<DataTablesStructs.ReturnedData<object>> GetHomeJobsOffersDatatable(DataTablesStructs.SentParameters sentParameters);
        Task<DataTablesStructs.ReturnedData<object>> GetJobsOffersForCompaniesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, string startSearchDate = null, string endSearchDate = null);
        Task<DataTablesStructs.ReturnedData<object>> GetJobsOffersCreatedByDatatable(DataTablesStructs.SentParameters sentParameters, string userName, string searchValue = null);
        Task<object> GetJobExchangeJobOfferCareerReportChart(List<Guid> careers = null);
        Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeJobOfferCareerReportDatatable(DataTablesStructs.SentParameters sentParameters, List<Guid> careers = null);
    }
}
