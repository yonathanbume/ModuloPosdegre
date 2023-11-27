using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    public class BeginningAnnouncementRepository : Repository<BeginningAnnouncement>, IBeginningAnnouncementRepository
    {
        public BeginningAnnouncementRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<bool> AnyInDates(DateTime startDate, DateTime endDate,byte appearence, Guid? id)
        {
            if(id != null && id.HasValue)
            {
                return await _context.BeginningAnnouncements.AnyAsync(x => (x.StartDate >= startDate &&
                                                             x.EndDate <= startDate)||
                                                             (x.StartDate >= endDate &&
                                                             x.EndDate <= endDate) && 
                                                             x.AppearsIn == appearence &&
                                                             x.Id != id);
            }
            return await _context.BeginningAnnouncements.AnyAsync(x => (x.StartDate >= startDate &&
                                                             x.EndDate <= startDate)||
                                                             (x.StartDate >= endDate &&
                                                             x.EndDate <= endDate) &&
                                                             x.AppearsIn == appearence 
                                                             );       
        }

        public async Task<BeginningAnnouncement> GetActive()
        {
            return await _context.BeginningAnnouncements.Where(x => x.StartDate <= DateTime.UtcNow && x.EndDate >= DateTime.UtcNow&& x.AppearsIn==ConstantHelpers.ANNOUNCEMENT.APPEARS_IN.HOME).Include(x=>x.BeginningAnnouncementRoles).ThenInclude(y=>y.Role).FirstOrDefaultAsync();
        }

        public async Task<List<BeginningAnnouncement>> GetBeginningAnnouncements(byte system, ClaimsPrincipal user)
        {
            var query = _context.BeginningAnnouncements.Where(x => x.StartDate.Date <= DateTime.UtcNow.Date && x.EndDate.Date >= DateTime.UtcNow.Date && x.AppearsIn == ConstantHelpers.ANNOUNCEMENT.APPEARS_IN.HOME && (x.System == system || x.System == 0)).Include(x => x.BeginningAnnouncementRoles).ThenInclude(y => y.Role).AsNoTracking();
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var roles = _context.UserRoles.Where(x => x.UserId == userId).Select(x => x.RoleId).ToHashSet();
            query = query.Where(x =>!x.BeginningAnnouncementRoles.Any() || x.BeginningAnnouncementRoles.Any(y => roles.Contains(y.RoleId)));
            return await query.ToListAsync();
        }

        public async Task<List<BeginningAnnouncement>> GetBeginningAnnouncementsForLogin(byte system)
        {
            var query = _context.BeginningAnnouncements.Where(x => x.StartDate.Date <= DateTime.UtcNow.Date && x.EndDate.Date >= DateTime.UtcNow.Date && x.AppearsIn == ConstantHelpers.ANNOUNCEMENT.APPEARS_IN.LOGIN && (x.System == system || x.System == 0)).Include(x => x.BeginningAnnouncementRoles).ThenInclude(y => y.Role).AsNoTracking();
            return await query.ToListAsync();
        }
        public async Task<BeginningAnnouncement> GetActiveForLogin()
        {
            return await _context.BeginningAnnouncements.Where(x => x.StartDate.Date <= DateTime.UtcNow.Date && x.EndDate.Date >= DateTime.UtcNow.Date && x.AppearsIn==ConstantHelpers.ANNOUNCEMENT.APPEARS_IN.LOGIN).FirstOrDefaultAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataTable(DataTablesStructs.SentParameters sentParameters, string search)
        {
            var query = _context.BeginningAnnouncements
                .AsQueryable();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderBy(x => x.CreatedAt)
                .Select(x => new
                {
                    id = x.Id,
                    title = x.Title,
                    type = ConstantHelpers.ANNOUNCEMENT.TYPE.VALUES.ContainsKey(x.Type)?  ConstantHelpers.ANNOUNCEMENT.TYPE.VALUES[x.Type]: "-",
                    system = ConstantHelpers.ANNOUNCEMENT.SYSTEM.VALUES.ContainsKey(x.System) ?  ConstantHelpers.ANNOUNCEMENT.SYSTEM.VALUES[x.System]: "Todos",
                    x.Description,
                    startDate = x.StartDate.ToDateFormat(),
                    endDate = x.EndDate.ToDateFormat(),
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = recordsFiltered,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<BeginningAnnouncementRole>> GetRoles(Guid announcementId)
            => await _context.BeginningAnnouncementRoles.Where(x => x.BeginningAnnouncementId == announcementId).ToListAsync();

        public async Task RemoveRoles(Guid announcementId)
        {
            var roles = await _context.BeginningAnnouncementRoles.Where(x => x.BeginningAnnouncementId == announcementId).ToListAsync();
            _context.RemoveRange(roles);
            await _context.SaveChangesAsync();
        } 
    }
}
