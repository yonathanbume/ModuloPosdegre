using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Implementations
{
    public class TutoringAnnouncementRepository : Repository<TutoringAnnouncement>, ITutoringAnnouncementRepository
    {
        public TutoringAnnouncementRepository(AkdemicContext context) : base(context)
        {
        }
        public async Task<IEnumerable<TutoringAnnouncement>> GetAllByRoles(string[] roleNames)
        {
            var query = _context.TutoringAnnouncements
                .Include(x => x.TutoringAnnouncementCareers)
                .ThenInclude(x => x.Career)
                .Include(x => x.TutoringAnnouncementRoles)
                .ThenInclude(x => x.Role)
                .OrderByDescending(x => x.DisplayTime)
                .Where(x => x.DisplayTime <= DateTime.UtcNow)
                .AsQueryable();
            if (!roleNames.Any(rn => rn == ConstantHelpers.ROLES.SUPERADMIN))
            {
                query = query
                .Where(x => x.AllRoles || x.TutoringAnnouncementRoles.Any(t => roleNames.Any(r => r == t.Role.Name)));

                var t1 = await query.Where(x => x.AllRoles || x.TutoringAnnouncementRoles.Any(t => roleNames.Any(r => r == t.Role.Name))).ToListAsync();
                
                var mmm = t1;
            }
            return await query.ToListAsync();
        }
        public async Task<IEnumerable<TutoringAnnouncement>> GetAllByRolesAndCareer(string[] roleNames,byte system, List<Guid> careers)
        {
            var query = _context.TutoringAnnouncements
                .Include(x => x.TutoringAnnouncementCareers)
                .ThenInclude(x => x.Career)
                .Include(x => x.TutoringAnnouncementRoles)
                .ThenInclude(x => x.Role)
                .OrderByDescending(x => x.DisplayTime)
                .Where(x => x.System == system && x.DisplayTime <= DateTime.UtcNow && DateTime.UtcNow <= x.EndTime)
                .AsQueryable();

            if (!roleNames.Any(rn => rn == ConstantHelpers.ROLES.SUPERADMIN ||
                rn == ConstantHelpers.ROLES.TUTORING_COORDINADOR_ADMIN ||
                rn == ConstantHelpers.ROLES.INTRANET_ADMIN))
            {
                query = query
                .Where(x => x.AllRoles || x.TutoringAnnouncementRoles.Any(t => roleNames.Any(r => r == t.Role.Name)))
                .Where(q => q.AllCareers || q.TutoringAnnouncementCareers.Any(p=> careers.Any(r=> r == p.Career.Id)));
            }
            return await query.ToListAsync();
        }

        public override async Task DeleteById(Guid tutoringAnnouncementId)
        {
            var announcement = await _context.TutoringAnnouncements
                .Include(x => x.TutoringAnnouncementRoles)
                .Include(x => x.TutoringAnnouncementCareers)
                .Where(x => x.Id == tutoringAnnouncementId)
                .FirstOrDefaultAsync();
            if(announcement.TutoringAnnouncementRoles.Any())
                _context.TutoringAnnouncementRoles.RemoveRange(announcement.TutoringAnnouncementRoles);
            if (announcement.TutoringAnnouncementCareers.Any())
                _context.TutoringAnnouncementCareers.RemoveRange(announcement.TutoringAnnouncementCareers);
            await _context.SaveChangesAsync();
            await base.DeleteById(tutoringAnnouncementId);
        }

        public async Task<TutoringAnnouncement> GetWithCareersAndRolesById(Guid tutoringAnnouncementId)
            => await _context.TutoringAnnouncements
                .Include(x => x.TutoringAnnouncementRoles)
                .Include(x => x.TutoringAnnouncementCareers)
                .Where(x => x.Id == tutoringAnnouncementId)
                .FirstOrDefaultAsync();

        public async Task<DataTablesStructs.ReturnedData<TutoringAnnouncement>> GetTutoringAnnouncementsDatatable(DataTablesStructs.SentParameters sentParameters, byte system, string searchValue = null, bool? published = null, bool? isCoordinatorAdmin = null, string userId = null)
        {
            Expression<Func<TutoringAnnouncement, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Title);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.TutoringAnnouncementRoles.Select(tR => tR.Role.Name).FirstOrDefault());
                    break;
                case "2":
                    orderByPredicate = ((x) => x.TutoringAnnouncementCareers.Select(tc => tc.Career.Name).FirstOrDefault());

                    break;
                default:
                    orderByPredicate = ((x) => x.Title);

                    break;
            }

            var query = _context.TutoringAnnouncements.Where(x=>x.System == system)
                .AsNoTracking();

            if (published.HasValue)
            {
                query = published.Value ? query.Where(x => x.DisplayTime <= DateTime.UtcNow)
                    : query.Where(x => x.DisplayTime > DateTime.UtcNow);
            }

            if (isCoordinatorAdmin.HasValue)
            {
                if (isCoordinatorAdmin.Value)
                {
                    if (!string.IsNullOrEmpty(userId))
                        query = query.Where(x => x.UserId.Contains(userId));
                }
              
            }
                

            if (!string.IsNullOrEmpty(searchValue))
                query = query
                    .Where(x => x.Title.ToUpper().Contains(searchValue)
                            || x.TutoringAnnouncementCareers.Select(tc => tc.Career.Name).FirstOrDefault().ToUpper().Contains(searchValue)
                            || x.TutoringAnnouncementRoles.Select(tR => tR.Role.Name).FirstOrDefault().ToUpper().Contains(searchValue));

            var recordsFiltered = await query.CountAsync();
            query = query
                .AsQueryable();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new TutoringAnnouncement
                {
                    Id = x.Id,
                    AllRoles = x.AllRoles,
                    AllCareers = x.AllCareers,
                    Title = x.Title,
                    Message = x.Message,
                    DisplayTime = x.DisplayTime,
                    EndTime = x.EndTime,
                    UserId = x.UserId,
                    TutoringAnnouncementCareers = x.TutoringAnnouncementCareers.Select(ta => new TutoringAnnouncementCareer
                    {
                        CareerId = ta.CareerId,
                        Career = new ENTITIES.Models.Generals.Career
                        {
                            Name = ta.Career.Name
                        }
                    }).ToList(),
                    TutoringAnnouncementRoles = x.TutoringAnnouncementRoles.Select(tr => new TutoringAnnouncementRole
                    {
                        RoleId = tr.RoleId,
                        Role = new ENTITIES.Models.Generals.ApplicationRole
                        {
                            Name = tr.Role.Name
                        }
                    })
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<TutoringAnnouncement>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
    }
}
