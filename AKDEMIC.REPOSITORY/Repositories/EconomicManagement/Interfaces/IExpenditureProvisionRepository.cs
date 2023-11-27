using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IExpenditureProvisionRepository : IRepository<ExpenditureProvision>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetExpenditureProvisionDatatable(DataTablesStructs.SentParameters sentParameters, string search);
        Task<object> GetProvisionByDependencyAndStatus(Guid id, int status);
        Task<List<ExpenditureProvision>> GetExpenditureProvisionList(Guid id, int status);
        Task<DataTablesStructs.ReturnedData<object>> GetExpenditureProvisionDatatableDependency(string userId, DataTablesStructs.SentParameters sentParameters, string search);
        IQueryable<ExpenditureProvision> ProvisionsQry(DateTime date, int status);
        Task<List<ExpenditureProvision>> GetExpenditureProvisionStatusList(int status);
        Task<DataTablesStructs.ReturnedData<object>> GetExpenditureProvisionDatatableProvision(DataTablesStructs.SentParameters sentParameters, string search);
    }
}
