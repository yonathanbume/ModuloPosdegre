using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Template;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Sisco.Implementations
{
    public class SubShortcutRepository : Repository<SubShortcut>, ISubShortcutRepository
    {
        public SubShortcutRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<SubShortcutTemplate>> GetAllSubShortcutDatatable(DataTablesStructs.SentParameters sentParameters, Guid id)
        {
            Expression<Func<SubShortcut, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Title;
                    break;
                case "1":
                    orderByPredicate = (x) => x.PublicationDate;
                    break;
                case "2":
                    orderByPredicate = (x) => x.UrlDirection;
                    break;
                default:
                    orderByPredicate = (x) => x.PublicationDate;
                    break;
            }


            var query = _context.SubShortcuts
                .AsNoTracking();

            query = query.Where(x => x.ShortcutId == id);

            var recordsFiltered = await query.CountAsync();

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new SubShortcutTemplate
                {
                    Id = x.Id,
                    Title = x.Title,
                    PublicationDate = $"{x.PublicationDate:dd-MM-yyyy hh:mm:ss tt}",
                    UrlDirection = x.UrlDirection,
                    ShortcutId = x.ShortcutId.HasValue ? x.ShortcutId.Value : Guid.Empty
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<SubShortcutTemplate>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<SubShortcut> GetSubShortcutByIdAndShortcutId(Guid ShortcutId, Guid id)
        {
            var subShortcut = await _context.SubShortcuts.Where(x => x.ShortcutId == ShortcutId && x.Id == id).FirstAsync();

            return subShortcut;
        }
    }
}
