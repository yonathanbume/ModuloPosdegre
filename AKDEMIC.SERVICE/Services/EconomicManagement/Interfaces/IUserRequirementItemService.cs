using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces
{
    public interface IUserRequirementItemService
    {
        Task InsertUserRequirementItem(UserRequirementItem userRequirementItem);
        Task UpdateUserRequirementItem(UserRequirementItem userRequirementItem);
        Task DeleteUserRequirementItem(UserRequirementItem userRequirementItem);
        Task<UserRequirementItem> GetUserRequirementItemById(Guid id);
        Task<IEnumerable<UserRequirementItem>> GetAllUserRequirementItems();
        Task<DataTablesStructs.ReturnedData<object>> GetUserRequirementItmDatatable(DataTablesStructs.SentParameters sentParameters, Guid itemId, string search = null);
        Task AddRangeAsync(List<UserRequirementItem> userRequirementItems);
        Task<object> GetAllUserRequerimentItemsByUserRequerimentId(DataTablesStructs.SentParameters sentParameters, Guid id);
        Task<UserRequirementItem> GetByUserRequrimentId(Guid userRequirementId);
        Task<object> GetByUserRequrimentDetailId(Guid userRequirementId);
        Task Update(UserRequirementItem userRequirementItem);
        Task SaveChanges();
    }
}
