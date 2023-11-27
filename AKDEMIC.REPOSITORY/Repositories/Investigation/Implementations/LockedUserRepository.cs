using System;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Investigation.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Investigation.Implementations
{
    public class LockedUserRepository : Repository<LockedUser>, ILockedUserRepository
    {
        public LockedUserRepository(AkdemicContext context) : base(context) { }

        public async Task Lock(string userId, string text)
        {
            var lockedUser = new LockedUser()
            {
                DateTime = DateTime.UtcNow.ToDefaultTimeZone(),
                Description = text,
                Status = true,
                UserId = userId
            };
            await _context.LockedUsers.AddAsync(lockedUser);
            await _context.SaveChangesAsync();
        }

        public async Task Unlock(string userId, string text)
        {
            var lockedUser = new LockedUser()
            {
                DateTime = DateTime.UtcNow.ToDefaultTimeZone(),
                Description = text,
                Status = false,
                UserId = userId
            };
            await _context.LockedUsers.AddAsync(lockedUser);
            await _context.SaveChangesAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> Historic(DataTablesStructs.SentParameters sentParameters, string userId)
        {
            var query = _context.LockedUsers.Where(x => x.UserId == userId).AsNoTracking();

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescending(x => x.DateTime)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    text = x.Description,
                    date = x.DateTime.ToDateTimeFormat(),
                    status = x.Status ? "Bloqueado" : "Desbloqueado"
                }).ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
    }
}
