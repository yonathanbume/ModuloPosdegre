using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    public class UserRoleRepository : Repository<ApplicationUserRole>, IUserRoleRepository
    {
        public UserRoleRepository(AkdemicContext context) : base(context) { }

        public async Task<int> CountByRole(string role)
        {
            IQueryable<ApplicationUserRole> query = _context.UserRoles.AsQueryable();

            //En caso de bolsa
            if (role == "Todos")
            {
                query = query.Where(x => x.Role.Name == "Alumnos" || x.Role.Name == "Empresa");
            }
            else
            {
                query = query.Where(x => x.Role.Name == role);
            }

            return await query.CountAsync();
        }

        public async Task<List<ApplicationUserRole>> GetAllRolesFromUser(string userId)
        {
            return await _context.UserRoles.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUserRole>> GetByRole(string role)
        {
            IQueryable<ApplicationUserRole> query = _context.UserRoles.AsQueryable();

            //En caso de bolsa
            if (role == "Todos")
            {
                query = query.Where(x => x.Role.Name == "Alumnos" || x.Role.Name == "Empresa");
            }
            else
            {
                query = query.Where(x => x.Role.Name == role);
            }

            return await query.ToListAsync();
        }

        public async Task<ApplicationUserRole> GetByUserIdAndRoleName(string userId, string roleName)
        {
            return await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId == userId && x.Role.Name == roleName);
        }
    }
}
