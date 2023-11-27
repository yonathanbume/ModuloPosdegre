using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IRequirementRepository : IRepository<Requirement>
    {
        Task<List<Requirement>> GetList(Guid id);
        Task<UserRequirement> GetWithIncludes(Guid id);
        Task<DataTablesStructs.ReturnedData<Requirement>> GetRequirementsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetRequirementOrderDatatable(DataTablesStructs.SentParameters sentParameters,int userRequirementIndex, string searchValue = null, int? filterValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetRequirementReportByDependencyAndOrderTypeDatatable(DataTablesStructs.SentParameters sentParameters, Guid? centerId = null, int? type = null, DateTime? startDate = null, DateTime? endDate = null);
        Task<DataTablesStructs.ReturnedData<object>> GetRequirementReportByFundingSourceAndDependencyDatatable(DataTablesStructs.SentParameters sentParameters);
        Task<object> GetRequirementReportByDependencyAndOrderTypeChart(Guid? centerId = null, int? type = null, DateTime? startDate = null, DateTime? endDate = null);
        Task<object> GetRequirementReportByFundingSourceAndDependencyChart();
        Task<List<UserRequirement>> GetOrderDetailIncludeOrderUser(Guid id);
        Task<Requirement> GetRequirementWithDataById(Guid id);
        Task<object> GetRequirementDetail(Guid id);
        Task<bool> AnyAsync(string codeNumber);
        Task<bool> FindByCodeNumberAndCodeNumber(string codeNumber, string codeNumber2);
    }
}
