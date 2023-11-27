using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Implementations
{
    public class UserDependencyService : IUserDependencyService
    {
        private readonly IUserDependencyRepository _userDependencyRepository;
        public UserDependencyService(IUserDependencyRepository userDependencyRepository)
        {
            _userDependencyRepository = userDependencyRepository;
        }

        Task<UserDependency> IUserDependencyService.Get(Guid id)
            => _userDependencyRepository.Get(id);

        Task<UserDependency> IUserDependencyService.GetUserDependency(Guid id)
            => _userDependencyRepository.GetUserDependency(id);

        Task<IEnumerable<UserDependency>> IUserDependencyService.GetUserDependencies()
            => _userDependencyRepository.GetUserDependencies();

        Task IUserDependencyService.Delete(UserDependency userDependency)
            => _userDependencyRepository.Delete(userDependency);

        Task IUserDependencyService.DeleteByUser(string userId)
            => _userDependencyRepository.DeleteByUser(userId);

        Task IUserDependencyService.Insert(UserDependency userDependency)
            => _userDependencyRepository.Insert(userDependency);

        Task IUserDependencyService.DeleteRange(IEnumerable<UserDependency> userDependencies)
            => _userDependencyRepository.DeleteRange(userDependencies);

        Task IUserDependencyService.InsertRange(IEnumerable<UserDependency> userDependencies)
            => _userDependencyRepository.InsertRange(userDependencies);

        Task<IEnumerable<UserDependency>> IUserDependencyService.GetAllByFilters(string userId)
            => _userDependencyRepository.GetAllByFilters(userId);

        Task<IEnumerable<UserDependency>> IUserDependencyService.GetUserDependenciesByUser(string userId)
            => _userDependencyRepository.GetUserDependenciesByUser(userId);

        public async Task<List<Guid>> GetIdUserDependenciesAssociat(string userId)
            => await _userDependencyRepository.GetIdUserDependenciesAssociat(userId);

        public async Task<List<UserDependency>> GetDependeciesByUserIdList(string userId)
            => await _userDependencyRepository.GetDependeciesByUserIdList(userId);
        public async Task<object> GetUserDependenciesJsonByUserId(string userId)
            => await _userDependencyRepository.GetUserDependenciesJsonByUserId(userId);
        public async Task<object> GetDependencyUser(string userId)
            => await _userDependencyRepository.GetDependencyUser(userId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetUsersInDependenciesDatatable(DataTablesStructs.SentParameters sentParameters, Guid? dependencyId = null, string searchValue = null)
            => await _userDependencyRepository.GetUsersInDependenciesDatatable(sentParameters,dependencyId,searchValue);
        public async Task<DataTablesStructs.ReturnedData<object>> GetUserDependenciesByDependencyDatatable(DataTablesStructs.SentParameters sentParameters, Guid dependencyId, string searchValue = null)
            => await _userDependencyRepository.GetUserDependenciesByDependencyDatatable(sentParameters, dependencyId, searchValue);
        public void RemoveRange(IEnumerable<UserDependency> userDependencies)
            => _userDependencyRepository.RemoveRange(userDependencies);

        public async Task AddRange(IEnumerable<UserDependency> userDependencies)
            => await _userDependencyRepository.AddRange(userDependencies);

        public Task<List<UserDependency>> GetUserDependencies(string userId)
            => _userDependencyRepository.GetUserDependencies(userId);

        public Task<UserDependency> Add(UserDependency userDependency)
            => _userDependencyRepository.Add(userDependency);
        public async Task<bool> ToggleDirectorDependency(string userId, Guid dependencyId)
            => await _userDependencyRepository.ToggleDirectorDependency(userId, dependencyId);
    }
}