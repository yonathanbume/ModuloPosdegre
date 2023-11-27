using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces
{
    public interface IUserDependencyRepository : IRepository<UserDependency>
    {
        Task<IEnumerable<UserDependency>> GetAllByFilters(string userId = null);
        Task<UserDependency> GetUserDependency(Guid id);
        Task<IEnumerable<UserDependency>> GetUserDependencies();
        Task DeleteByUser(string userId);
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