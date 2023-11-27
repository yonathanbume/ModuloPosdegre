using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IExpenditureProvisionService
    {
        Task InsertExpenditureProvision(ExpenditureProvision expenditureProvision);
        Task UpdateExpenditureProvision(ExpenditureProvision expenditureProvision);
        Task DeleteExpenditureProvision(ExpenditureProvision expenditureProvision);
        Task<ExpenditureProvision> GetExpenditureProvisionById(Guid id);
        Task<IEnumerable<ExpenditureProvision>> GetAllExpenditureProvisions();
        Task<DataTablesStructs.ReturnedData<object>> GetExpenditureProvisionDatatable(DataTablesStructs.SentParameters sentParameters, string search);
        Task<object> GetProvisionByDependencyAndStatus(Guid id, int status);
        Task<List<ExpenditureProvision>> GetExpenditureProvisionList(Guid id, int status);
        Task<DataTablesStructs.ReturnedData<object>> GetExpenditureProvisionDatatableDependency(string userId, DataTablesStructs.SentParameters sentParameters, string search);
        IQueryable<ExpenditureProvision> ProvisionsQry(DateTime date, int status);
        Task<List<ExpenditureProvision>> GetExpenditureProvisionStatusList(int status);
        Task<DataTablesStructs.ReturnedData<object>> GetExpenditureProvisionDatatableProvision(DataTablesStructs.SentParameters sentParameters, string search);

    }
}
