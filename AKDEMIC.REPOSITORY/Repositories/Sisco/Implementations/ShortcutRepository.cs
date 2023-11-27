using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Template;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Sisco.Implementations
{
    public class ShortcutRepository : Repository<Shortcut>, IShortcutRepository
    {
        public ShortcutRepository(AkdemicContext context) : base(context) { }
        public async Task<DataTablesStructs.ReturnedData<ShortcutTemplate>> GetAllShortcutDatatable(DataTablesStructs.SentParameters sentParameters, int type, string title = null)
        {
            Expression<Func<Shortcut, dynamic>> orderByPredicate = null;

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


            var query = _context.Shortcuts
                .AsNoTracking();
            
            if(type == 1)
                query = query.Where(x => x.Type.Contains("ACCESO DIRECTO"));
            if(type == 2)
                query = query.Where(x => x.Type.Contains("ENLACE DE INTERES"));

            var recordsFiltered = await query.CountAsync();

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(q => q.Title.Contains(title));
            }

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new ShortcutTemplate
                {
                    Id = x.Id,
                    Title = x.Title,
                    PublicationDate = $"{x.PublicationDate:dd-MM-yyyy hh:mm:ss tt}",
                    UrlDirection = x.UrlDirection,
                    Type = x.Type
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<ShortcutTemplate>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<EditShortcutTemplate> GetShortcutById(Guid id)
        {
            var shortcut = await _context.Shortcuts.Include(x => x.SubShortcuts).Where(x => x.Id == id).FirstAsync();

            var listSubShort = new List<SubShortcutTemplate>();

            foreach (var item in shortcut.SubShortcuts)
            {
                var subShort = new SubShortcutTemplate()
                {
                    Id = item.Id,
                    PublicationDate = $"{item.PublicationDate:dd-MM-yyyy hh:mm:ss tt}",
                    ShortcutId = item.ShortcutId.HasValue ? item.ShortcutId.Value : Guid.Empty,
                    Title = item.Title,
                    UrlDirection = item.UrlDirection
                };
                listSubShort.Add(subShort);
            }

            var model = await _context.Shortcuts.Include(x => x.SubShortcuts)
                .Where(x => x.Id == id)
                .Select(x => new EditShortcutTemplate
                {
                    Id = x.Id,
                    Title = x.Title,
                    UrlDirection = x.UrlDirection,
                    ListSubShortcut = listSubShort

                }).FirstAsync();

            return model;
        }

    }
}
