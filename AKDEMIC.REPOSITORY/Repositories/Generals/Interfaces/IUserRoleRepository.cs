using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface IUserRoleRepository : IRepository<ApplicationUserRole>
    {
        Task<int> CountByRole(string role);
        Task<IEnumerable<ApplicationUserRole>> GetByRole(string role);
        Task<ApplicationUserRole> GetByUserIdAndRoleName(string userId, string roleName);
        Task<List<ApplicationUserRole>> GetAllRolesFromUser(string userId);       
    }
}
