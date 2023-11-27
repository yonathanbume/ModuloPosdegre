using AKDEMIC.ENTITIES.Models.Generals;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface IUserRoleService
    {
        Task<int> CountByRole(string role);
        //Task<IEnumerable<ApplicationUserRole>> GetByRole(string role);
        Task Insert(ApplicationUserRole appUserRole);
        Task <List<ApplicationUserRole>> GetAllRolesFromUser(string userId);
        void RemoveRange(List<ApplicationUserRole> userRoles);
        Task Delete(ApplicationUserRole role);
        Task<ApplicationUserRole> GetByUserIdAndRoleName(string userId, string roleName);
    }
}
