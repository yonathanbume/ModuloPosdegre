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
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    public class UserAnnouncementRepository : Repository<UserAnnouncement>, IUserAnnouncementRepository
    {
        public UserAnnouncementRepository(AkdemicContext context) : base(context)
        {
        }
        public async override Task<UserAnnouncement> Get(Guid id)
        {
            return await _context.UserAnnouncements
                .Where(x => x.Id == id)
                .Include(x => x.User)
                .FirstOrDefaultAsync();
        }
        public async Task<bool> AnyInDates(DateTime startDate, DateTime endDate, Guid? id)
        {
            if(id != null && id.HasValue)
            {
                return await _context.UserAnnouncements.AnyAsync(x => (x.StartDate >= startDate &&
                                                             x.EndDate <= startDate)||
                                                             (x.StartDate >= endDate &&
                                                             x.EndDate <= endDate) && 
                                                             x.Id != id);
            }
            return await _context.UserAnnouncements.AnyAsync(x => (x.StartDate >= startDate &&
                                                             x.EndDate <= startDate)||
                                                             (x.StartDate >= endDate &&
                                                             x.EndDate <= endDate));       
        }

        public async Task<UserAnnouncement> GetActive(string userId)
        {
            return await _context.UserAnnouncements.Where(x =>x.UserId==userId && x.StartDate <= DateTime.UtcNow && x.EndDate >= DateTime.UtcNow).FirstOrDefaultAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataTable(DataTablesStructs.SentParameters sentParameters, string search)
        {
            var query = _context.UserAnnouncements
                .AsQueryable();

            var recordsFiltered = await query.CountAsync();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.User.PaternalSurname.Contains(search) ||x.User.MaternalSurname.Contains(search) ||x.User.Name.Contains(search));

            var data = await query
                .OrderBy(x => x.CreatedAt)
                .Select(x => new
                {
                    id = x.Id,
                    title = x.Title,
                    type = ConstantHelpers.ANNOUNCEMENT.TYPE.VALUES[x.Type],
                    x.UserId,
                    x.User.FullName,
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
    }
}
