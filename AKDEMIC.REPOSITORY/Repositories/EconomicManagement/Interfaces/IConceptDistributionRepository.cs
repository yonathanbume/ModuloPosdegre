using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.ConceptDistribution;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IConceptDistributionRepository:IRepository<ConceptDistribution>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetConceptDistributionDatatable(DataTablesStructs.SentParameters sentParameters);
        Task<ConceptDistribution> GetWithIncludes(Guid id);
        Task<List<ConceptDistributionReportTemplate>> GetConceptDistributionExcel(Guid id);
        Task<bool> HasPayments(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetConceptDistributionReportDatatable(DataTablesStructs.SentParameters sentParameters);
    }
}
