using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.UserRequirement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces
{
    public interface IUserRequirementRepository : IRepository<UserRequirement>
    {
        Task<List<UserRequirement>> GetListByRequirementId(Guid reqid);
        Task<DataTablesStructs.ReturnedData<UserRequirement>> GetHigherUITUserRequirementsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<UserRequirement>> GetHigherUITUserRequirementsDatatableByRequirementUser(DataTablesStructs.SentParameters sentParameters, string requirementUserId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<UserRequirement>> GetHigherUITUserRequirementsDatatableByRequirementUser(DataTablesStructs.SentParameters sentParameters, string requirementUserId, string roleId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<UserRequirement>> GetHigherUITUserRequirementsDatatableByRole(DataTablesStructs.SentParameters sentParameters, string roleId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<UserRequirement>> GetLowerUITUserRequirementsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<UserRequirement>> GetLowerUITUserRequirementsDatatableByRequirementUser(DataTablesStructs.SentParameters sentParameters, string requirementUserId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<UserRequirement>> GetLowerUITUserRequirementsDatatableByRequirementUser(DataTablesStructs.SentParameters sentParameters, string requirementUserId, string roleId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<UserRequirement>> GetLowerUITUserRequirementsDatatableByRole(DataTablesStructs.SentParameters sentParameters, string roleId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<UserRequirement>> GetUserRequirementsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<UserRequirement>> GetUserRequirementsDatatableByRequirementUser(DataTablesStructs.SentParameters sentParameters, string requirementUserId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<UserRequirement>> GetUserRequirementsDatatableByRequirementUser(DataTablesStructs.SentParameters sentParameters, string requirementUserId, string roleId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<UserRequirement>> GetUserRequirementsDatatableByRole(DataTablesStructs.SentParameters sentParameters, string roleId, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetRequirementDatatable(DataTablesStructs.SentParameters sentParameters, int UserRequirementIndex, Guid dependencyId, int filterValue, string code, int status);
        Task<List<UserRequirementTemplate>> GetRequirementDatatableToReport(int UserRequirementIndex, Guid? dependencyId = null, int? filterValue = null, string code = null);
        Task<UserRequirement> GetUserRequirementById(Guid id);
        Task<object> GetUnitProgram(Guid id);
        Task<object> GetBudgetOffice(Guid id);
        Task<UserRequirement> GetWithIncludesId(Guid id);
        Task<UserRequirementOrderDetailTemplate> GetFirstUserRequirementOrderDetail(string supplierRuc, int orderNumber);
        Task<string> GetItemById(Guid id);
        Task SaveCHanges();
        Task<int> GetQuantityItem(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetUserRequrimentByOrder(DataTablesStructs.SentParameters sentParameters, Guid orderId);
    }
}
