using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRoleService(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }
        public async Task<int> CountByRole(string role)
        {
            return await _userRoleRepository.CountByRole(role);
        }
        //public IEnumerable<ApplicationUserRole> GetByRole(string role)
        //{
        //    throw new NotImplementedException();
        //}
        public async Task Insert(ApplicationUserRole appUserRole)
        {
            await _userRoleRepository.Insert(appUserRole);
        }
        public async Task Delete(ApplicationUserRole role)
        {
            await _userRoleRepository.Delete(role);
        }
        public async Task<ApplicationUserRole> GetByUserIdAndRoleName(string userId, string roleName)
        {
            return await _userRoleRepository.GetByUserIdAndRoleName(userId, roleName);
        }

        public Task<List<ApplicationUserRole>> GetAllRolesFromUser(string userId)
            => _userRoleRepository.GetAllRolesFromUser(userId);

        public void RemoveRange(List<ApplicationUserRole> userRoles)
            => _userRoleRepository.RemoveRange(userRoles);
    }
}
