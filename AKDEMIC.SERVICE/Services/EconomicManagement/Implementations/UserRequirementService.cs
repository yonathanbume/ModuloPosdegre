using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.UserRequirement;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class UserRequirementService : IUserRequirementService
    {
        private readonly IUserRequirementRepository _userRequirementRepository;

        public UserRequirementService(IUserRequirementRepository userRequirementRepository)
        {
            _userRequirementRepository = userRequirementRepository;
        }

        public async Task<int> Count()
        {
            return await _userRequirementRepository.Count();
        }

        public async Task<List<UserRequirement>> GetListByRequirementId(Guid reqid)
            => await _userRequirementRepository.GetListByRequirementId(reqid);
        public async Task<UserRequirement> Get(Guid id)
        {
            return await _userRequirementRepository.Get(id);
        }

        public async Task<DataTablesStructs.ReturnedData<UserRequirement>> GetHigherUITUserRequirementsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _userRequirementRepository.GetHigherUITUserRequirementsDatatable(sentParameters, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<UserRequirement>> GetHigherUITUserRequirementsDatatableByRequirementUser(DataTablesStructs.SentParameters sentParameters, string requirementUserId, string searchValue = null)
        {
            return await _userRequirementRepository.GetHigherUITUserRequirementsDatatableByRequirementUser(sentParameters, requirementUserId, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<UserRequirement>> GetHigherUITUserRequirementsDatatableByRequirementUser(DataTablesStructs.SentParameters sentParameters, string requirementUserId, string roleId, string searchValue = null)
        {
            return await _userRequirementRepository.GetHigherUITUserRequirementsDatatableByRequirementUser(sentParameters, requirementUserId, roleId, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<UserRequirement>> GetHigherUITUserRequirementsDatatableByRole(DataTablesStructs.SentParameters sentParameters, string roleId, string searchValue = null)
        {
            return await _userRequirementRepository.GetHigherUITUserRequirementsDatatableByRole(sentParameters, roleId, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<UserRequirement>> GetLowerUITUserRequirementsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _userRequirementRepository.GetLowerUITUserRequirementsDatatable(sentParameters, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<UserRequirement>> GetLowerUITUserRequirementsDatatableByRequirementUser(DataTablesStructs.SentParameters sentParameters, string requirementUserId, string searchValue = null)
        {
            return await _userRequirementRepository.GetLowerUITUserRequirementsDatatableByRequirementUser(sentParameters, requirementUserId, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<UserRequirement>> GetLowerUITUserRequirementsDatatableByRequirementUser(DataTablesStructs.SentParameters sentParameters, string requirementUserId, string roleId, string searchValue = null)
        {
            return await _userRequirementRepository.GetLowerUITUserRequirementsDatatableByRequirementUser(sentParameters, requirementUserId, roleId, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<UserRequirement>> GetLowerUITUserRequirementsDatatableByRole(DataTablesStructs.SentParameters sentParameters, string roleId, string searchValue = null)
        {
            return await _userRequirementRepository.GetLowerUITUserRequirementsDatatableByRequirementUser(sentParameters, roleId, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<UserRequirement>> GetUserRequirementsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _userRequirementRepository.GetUserRequirementsDatatable(sentParameters, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<UserRequirement>> GetUserRequirementsDatatableByRequirementUser(DataTablesStructs.SentParameters sentParameters, string requirementUserId, string searchValue = null)
        {
            return await _userRequirementRepository.GetUserRequirementsDatatableByRequirementUser(sentParameters, requirementUserId, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<UserRequirement>> GetUserRequirementsDatatableByRequirementUser(DataTablesStructs.SentParameters sentParameters, string requirementUserId, string roleId, string searchValue = null)
        {
            return await _userRequirementRepository.GetUserRequirementsDatatableByRequirementUser(sentParameters, requirementUserId, roleId, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<UserRequirement>> GetUserRequirementsDatatableByRole(DataTablesStructs.SentParameters sentParameters, string roleId, string searchValue = null)
        {
            return await _userRequirementRepository.GetUserRequirementsDatatableByRequirementUser(sentParameters, roleId, searchValue);
        }

        public async Task Insert(UserRequirement userRequirement) =>
            await _userRequirementRepository.Insert(userRequirement);

        public async Task Update(UserRequirement userRequirement) =>
            await _userRequirementRepository.Update(userRequirement);

        public async Task<DataTablesStructs.ReturnedData<object>> GetRequirementDatatable(DataTablesStructs.SentParameters sentParameters, int UserRequirementIndex, Guid dependencyId, int filterValue, string code, int status)
            => await _userRequirementRepository.GetRequirementDatatable(sentParameters, UserRequirementIndex, dependencyId, filterValue, code, status);
        public async Task<List<UserRequirementTemplate>> GetRequirementDatatableToReport(int UserRequirementIndex, Guid? dependencyId = null, int? filterValue = null, string code = null)
         => await _userRequirementRepository.GetRequirementDatatableToReport(UserRequirementIndex, dependencyId, filterValue, code);
        public async Task<UserRequirement> GetUserRequirementById(Guid id)
            => await _userRequirementRepository.GetUserRequirementById(id);
        public async Task AddAsync(UserRequirement userRequirement)
            => await _userRequirementRepository.Add(userRequirement);
        public async Task<object> GetUnitProgram(Guid id)
            => await _userRequirementRepository.GetUnitProgram(id);
        public async Task<object> GetBudgetOffice(Guid id)
            => await _userRequirementRepository.GetBudgetOffice(id);
        public async Task<UserRequirement> GetWithIncludesId(Guid id)
            => await _userRequirementRepository.GetWithIncludesId(id);
        public async Task<string> GetItemById(Guid id)
            => await _userRequirementRepository.GetItemById(id);

        public async Task SaveChangesUserRequirements()
        {
            await _userRequirementRepository.SaveCHanges();
        }
        public async Task<int> GetQuantityItem(Guid id)
            => await _userRequirementRepository.GetQuantityItem(id);
        public async Task<DataTablesStructs.ReturnedData<object>> GetUserRequrimentByOrder(DataTablesStructs.SentParameters sentParameters, Guid orderId)
            => await _userRequirementRepository.GetUserRequrimentByOrder(sentParameters, orderId);

        public Task<UserRequirementOrderDetailTemplate> GetFirstUserRequirementOrderDetail(string supplierRuc, int orderNumber)
            => _userRequirementRepository.GetFirstUserRequirementOrderDetail(supplierRuc, orderNumber);
    }
}
