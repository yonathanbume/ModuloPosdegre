using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.ConceptDistribution;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IConceptDistributionService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetConceptDistributionDatatable(DataTablesStructs.SentParameters sentParameters);
        Task Insert(ConceptDistribution conceptDistribution);
        Task Update(ConceptDistribution conceptDistribution);
        Task DeleteById(Guid id);
        Task<bool> HasPayments(Guid id);
        Task<ConceptDistribution> Get(Guid id);
        Task<IEnumerable<ConceptDistribution>> GetAll();
        Task<ConceptDistribution> GetWithIncludes(Guid id);
        Task<List<ConceptDistributionReportTemplate>> GetConceptDistributionExcel(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetConceptDistributionReportDatatable(DataTablesStructs.SentParameters sentParameters);
    }
}
