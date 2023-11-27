using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class HeritageRepository : Repository<Heritage> , IHeritageRepository
    {
        public HeritageRepository(AkdemicContext context) : base(context) { }

        public async Task<Heritage> Get(Guid catalogItemId, Guid dependencyId)
        {
            var item = await _context.Heritages.Where(x => x.CatalogItemId == catalogItemId && x.DependencyId == dependencyId).FirstOrDefaultAsync();
            return item;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetHeritageDatatable(DataTablesStructs.SentParameters parameters, ClaimsPrincipal user, string search)
        {
            Expression<Func<Heritage, dynamic>> orderByPredicate = null;

            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.CatalogItem.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.CatalogItem.Description;
                    break;
                case "2":
                    orderByPredicate = (x) => x.PecosaCode;
                    break;
                default:
                    orderByPredicate = (x) => x.Quantity;
                    break;
            }

            var query = _context.Heritages
               .AsNoTracking();

            if (user.IsInRole(ConstantHelpers.ROLES.COST_CENTER))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var dependecies = await _context.UserDependencies.Where(x => x.UserId == userId).Select(x => x.DependencyId).ToListAsync();
                query = query.Where(x => dependecies.Contains(x.DependencyId));
            }

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.CatalogItem.Description.ToLower().Contains(search.ToLower()) || x.CatalogItem.Code.ToLower().Contains(search.ToLower()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.DependencyId,
                    x.CatalogItemId,
                    x.CatalogItem.Code,
                    dependency = x.Dependency.Name,
                    x.CatalogItem.Description,
                    x.PecosaCode,
                    x.Quantity
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<Heritage>> GetHeritages(ClaimsPrincipal user)
        {
            var query = _context.Heritages
               .AsNoTracking();

            if (user.IsInRole(ConstantHelpers.ROLES.COST_CENTER))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var dependecies = await _context.UserDependencies.Where(x => x.UserId == userId).Select(x => x.DependencyId).ToListAsync();
                query = query.Where(x => dependecies.Contains(x.DependencyId));
            }

            var data = await query
                .Select(x => new Heritage
                {
                    PecosaCode = x.PecosaCode,
                    CatalogItem = new CatalogItem
                    {
                        Code = x.CatalogItem.Code,
                        Description = x.CatalogItem.Description
                    },
                    Code = x.Code,
                    Quantity = x.Quantity,
                    Ubication = x.Ubication,
                    Dependency = new ENTITIES.Models.DocumentaryProcedure.Dependency
                    {
                        Name = x.Dependency.Name
                    }
                })
                .ToListAsync();

            return data;
        }
    }
}
