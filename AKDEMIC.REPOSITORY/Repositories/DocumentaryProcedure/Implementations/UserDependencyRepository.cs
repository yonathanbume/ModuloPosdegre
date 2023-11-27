using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations
{
    public class UserDependencyRepository : Repository<UserDependency>, IUserDependencyRepository
    {
        public UserDependencyRepository(AkdemicContext context) : base(context) { }


        async Task<IEnumerable<UserDependency>> IUserDependencyRepository.GetAllByFilters(string userId)
        {
            var query = _context.UserDependencies.AsQueryable();

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(x => x.UserId == userId);

            return await query.ToListAsync();
        }

        async Task<UserDependency> IUserDependencyRepository.GetUserDependency(Guid id)
        {
            return await _context.UserDependencies
                .SelectUserDependency()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        async Task<IEnumerable<UserDependency>> IUserDependencyRepository.GetUserDependencies()
        {
            var query = _context.UserDependencies
                .SelectUserDependency()
                .AsQueryable();

            return await query.ToListAsync();
        }

        async Task IUserDependencyRepository.DeleteByUser(string userId)
        {
            var userDependencies = await _context.UserDependencies
                .Where(x => x.UserId == userId)
                .ToListAsync();

            _context.UserDependencies.RemoveRange(userDependencies);
            await _context.SaveChangesAsync();
        }

        async Task<IEnumerable<UserDependency>> IUserDependencyRepository.GetUserDependenciesByUser(string userId)
        {
            var query = _context.UserDependencies
                .Where(x => x.UserId == userId)
                .SelectUserDependency()
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<List<Guid>> GetIdUserDependenciesAssociat(string userId)
        {

            var userDependencies = await _context.UserDependencies.Where(x => x.UserId == userId).Select(x => x.DependencyId).ToListAsync();
            return userDependencies;
        }

        public async Task<List<UserDependency>> GetDependeciesByUserIdList(string userId)
        {
            var dependencies = await _context.UserDependencies.Include(x => x.Dependency)
                .Where(x => x.UserId == userId).ToListAsync();

            return dependencies;
        }

        public async Task<object> GetUserDependenciesJsonByUserId(string userId)
        {
            var data = await _context.UserDependencies
                .Where(x => x.UserId == userId)
                .Select(x => new
                {
                    id = x.Dependency.Id,
                    text = x.Dependency.Name
                }).ToListAsync();

            var result = new
            {
                items = data
            };

            return result;
        }

        public async Task<object> GetDependencyUser(string userId)
        {
            var userDependencies = await _context.UserDependencies.Include(x => x.Dependency).Where(x => x.UserId == userId)
                    .Select(x => new
                    {
                        id = x.DependencyId,
                        text = x.Dependency.Name
                    }).ToListAsync();

            return userDependencies;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUsersInDependenciesDatatable(DataTablesStructs.SentParameters sentParameters, Guid? dependencyId = null, string searchValue = null)
        {
            Expression<Func<UserDependency, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Dependency.Acronym;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Dependency.Name;
                    break;
                default:
                    orderByPredicate = (x) => x.Dependency.Acronym;
                    break;
            }


            var query = _context.UserDependencies
                .AsNoTracking();

            if (dependencyId != null)
                query = query.Where(x => x.DependencyId == dependencyId);

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.User.Name.ToUpper().Contains(searchValue.ToUpper()) || 
                                        x.User.PaternalSurname.ToUpper().Contains(searchValue.ToUpper()) ||
                                        x.User.MaternalSurname.ToUpper().Contains(searchValue.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.UserId,
                    name = x.User.FullName,
                    email = x.User.Email,
                    phone = x.User.PhoneNumber ?? "--"
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserDependenciesByDependencyDatatable(DataTablesStructs.SentParameters sentParameters, Guid dependencyId, string searchValue)
        {
            Expression<Func<UserDependency, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.Name;
                    break;
                default:
                    orderByPredicate = (x) => x.User.Name;
                    break;
            }


            var query = _context.UserDependencies.Where(x => x.DependencyId == dependencyId).Include(x => x.User).Include(x => x.Dependency).ThenInclude(x => x.User)
                .AsNoTracking();
            var test = _context.UserDependencies.Where(x => x.DependencyId == dependencyId).Include(x => x.User).Include(x => x.Dependency).ThenInclude(x => x.User).ToList();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.User.Name.ToUpper().Contains(searchValue.ToUpper()) ||
                                        x.User.PaternalSurname.ToUpper().Contains(searchValue.ToUpper()) ||
                                        x.User.MaternalSurname.ToUpper().Contains(searchValue.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    dependencyId = x.DependencyId,
                    userId = x.UserId,
                    name = x.User.FullName,
                    isDirector = x.Dependency.UserId != null ? (x.Dependency.UserId == x.UserId ? true : false) : false
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
        public async Task<List<UserDependency>> GetUserDependencies(string userId)
        {
            var query = _context.UserDependencies
                .Include(x => x.Dependency)
                .Where(x => x.UserId == userId).AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<bool> ToggleDirectorDependency(string userId, Guid dependencyId)
        {
            var dependency = await _context.Dependencies
               .Where(x => x.Id == dependencyId)
               .FirstOrDefaultAsync();

            var dependencyAny = dependency.UserId != null;

            if (dependencyAny)
            {
                dependency.UserId = null;
            }
            else
            {
                dependency.UserId = userId;
            }

            await _context.SaveChangesAsync();
            return !dependencyAny;
        }
    }
}