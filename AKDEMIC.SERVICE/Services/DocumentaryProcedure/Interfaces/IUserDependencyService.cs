using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces
{
    public interface IUserDependencyService
    {
        Task<UserDependency> Get(Guid id);
        Task<UserDependency> Add(UserDependency userDependency);
        Task<UserDependency> GetUserDependency(Guid id);
        Task<IEnumerable<UserDependency>> GetUserDependencies();
        Task Delete(UserDependency userDependency);
        Task DeleteByUser(string userId);
        Task Insert(UserDependency userDependency);
        Task DeleteRange(IEnumerable<UserDependency> userDependencies);
        void RemoveRange(IEnumerable<UserDependency> userDependencies);
        Task InsertRange(IEnumerable<UserDependency> userDependencies);
        Task AddRange(IEnumerable<UserDependency> userDependencies);
        Task<IEnumerable<UserDependency>> GetAllByFilters(string userId = null);
        Task<IEnumerable<UserDependency>> GetUserDependenciesByUser(string userId);
        Task<List<Guid>> GetIdUserDependenciesAssociat(string userId);
        Task<List<UserDependency>> GetDependeciesByUserIdList(string userId);
        Task<object> GetUserDependenciesJsonByUserId(string userId);
        Task<object> GetDependencyUser(string userId);
        Task<List<UserDependency>> GetUserDependencies(string userId);

        Task<DataTablesStructs.ReturnedData<object>> GetUsersInDependenciesDatatable(DataTablesStructs.SentParameters sentParameters, Guid? dependencyId = null, string searchValue = null);
        Task<DataTablesStructs.ReturnedData<object>> GetUserDependenciesByDependencyDatatable(DataTablesStructs.SentParameters sentParameters, Guid dependencyId, string searchValue = null);
        Task<bool> ToggleDirectorDependency(string userId, Guid dependencyId);
    }
}