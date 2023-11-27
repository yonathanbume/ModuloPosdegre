using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IRequirementService
    {
        Task<int> Count();
        Task<Requirement> Get(Guid id);
        Task<List<Requirement>> GetList(Guid id);
        Task<UserRequirement> GetWithIncludes(Guid id);
        Task<DataTablesStructs.ReturnedData<Requirement>> GetRequirementsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetRequirementOrderDatatable(DataTablesStructs.SentParameters sentParameters,int userRequirementIndex, string searchValue = null, int? filterValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetRequirementReportByDependencyAndOrderTypeDatatable(DataTablesStructs.SentParameters sentParameters, Guid? centerId = null, int? type = null, DateTime? startDate = null, DateTime? endDate = null);
        Task<DataTablesStructs.ReturnedData<object>> GetRequirementReportByFundingSourceAndDependencyDatatable(DataTablesStructs.SentParameters sentParameters);
        Task<object> GetRequirementReportByDependencyAndOrderTypeChart(Guid? centerId = null, int? type = null, DateTime? startDate = null, DateTime? endDate = null);
        Task<object> GetRequirementReportByFundingSourceAndDependencyChart();
        Task Insert(Requirement requirement);
        Task Update(Requirement requirement);
        Task<List<UserRequirement>> GetOrderDetailIncludeOrderUser(Guid id);
        Task<Requirement> GetRequirementWithDataById(Guid id);
        Task<object> GetRequirementDetail(Guid id);
        Task<bool> AnyAsync(string codeNumber);
        Task AddAsync(Requirement requirement);
        Task<bool> FindByCodeNumberAndCodeNumber(string codeNumber, string codeNumber2);
    }
}
