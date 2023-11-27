using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Server;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Server.Implementations
{
    public class GeneralLinkRepository : Repository<GeneralLink> , IGeneralLinkRepository
    {
        public GeneralLinkRepository(AkdemicContext context) : base(context){ }

        public async Task<IEnumerable<GeneralLink>> GetAll(byte? type = null, ClaimsPrincipal user = null)
        {
            var query = _context.GeneralLinks.AsNoTracking();

            if (type != null)
                query = query.Where(x => x.Type == type);

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var rolesId = await _context.UserRoles.Where(x => x.UserId == userId).Select(x => x.RoleId).ToListAsync();

                query = query.Where(x => !x.GeneralLinkRoles.Any() || x.GeneralLinkRoles.Any(y => rolesId.Contains(y.ApplicationRoleId)));
            }


            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, byte? type)
        {
            var query = _context.GeneralLinks
                .OrderByDescending(x=>x.CreatedAt)
                .AsNoTracking();

            if (type.HasValue)
                query = query.Where(x => x.Type == type);

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Description,
                    x.ImageUrl,
                    x.Link,
                    x.Type,
                    roles = x.GeneralLinkRoles.Select(y=> new
                    {
                        text = y.ApplicationRole.Name,
                        id = y.ApplicationRoleId
                    }).ToList()
                })
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }
    }
}
