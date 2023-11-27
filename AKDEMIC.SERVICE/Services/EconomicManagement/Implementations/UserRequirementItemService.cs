using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.SERVICE.Services.EconomicManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.EconomicManagement.Implementations
{
    public class UserRequirementItemService : IUserRequirementItemService
    {
        private readonly IUserRequirementItemRepository _userRequirementItemRepository;

        public UserRequirementItemService(IUserRequirementItemRepository userRequirementItemRepository)
        {
            _userRequirementItemRepository = userRequirementItemRepository;
        }

        public async Task InsertUserRequirementItem(UserRequirementItem userRequirementItem) =>
            await _userRequirementItemRepository.Insert(userRequirementItem);

        public async Task UpdateUserRequirementItem(UserRequirementItem userRequirementItem) =>
            await _userRequirementItemRepository.Update(userRequirementItem);

        public async Task DeleteUserRequirementItem(UserRequirementItem userRequirementItem) =>
            await _userRequirementItemRepository.Delete(userRequirementItem);

        public async Task<UserRequirementItem> GetUserRequirementItemById(Guid id) =>
            await _userRequirementItemRepository.Get(id);

        public async Task<IEnumerable<UserRequirementItem>> GetAllUserRequirementItems() =>
            await _userRequirementItemRepository.GetAll();
        public async Task<DataTablesStructs.ReturnedData<object>> GetUserRequirementItmDatatable(DataTablesStructs.SentParameters sentParameters, Guid itemId, string search = null)
            => await _userRequirementItemRepository.GetUserRequirementItmDatatable(sentParameters, itemId, search);
        public async Task AddRangeAsync(List<UserRequirementItem> userRequirementItems)
            => await _userRequirementItemRepository.AddRange(userRequirementItems);

        public async Task<object> GetAllUserRequerimentItemsByUserRequerimentId(DataTablesStructs.SentParameters sentParameters, Guid id)
            => await _userRequirementItemRepository.GetAllUserRequerimentItemsByUserRequerimentId(sentParameters, id);
        public async Task<UserRequirementItem> GetByUserRequrimentId(Guid userRequirementId)
            => await _userRequirementItemRepository.GetByUserRequrimentId(userRequirementId);
        public async Task<object> GetByUserRequrimentDetailId(Guid userRequirementId)
            => await _userRequirementItemRepository.GetByUserRequrimentDetailId(userRequirementId);

        public async Task Update(UserRequirementItem userRequirementItem)
        {
            await _userRequirementItemRepository.Update(userRequirementItem);
        }

        public async Task SaveChanges()
        {
            await _userRequirementItemRepository.SaveChanges();
        }
    }
}
