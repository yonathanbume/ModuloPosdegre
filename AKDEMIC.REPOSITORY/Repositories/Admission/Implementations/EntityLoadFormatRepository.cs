using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Implementations
{
    public class EntityLoadFormatRepository : Repository<EntityLoadFormat>, IEntityLoadFormatRepository
    {
        public EntityLoadFormatRepository(AkdemicContext context) : base(context)
        {

        }

        public async Task Activate(Guid entityLoadFormatId)
        {
            var formats = await _context.EntityLoadFormats.ToListAsync();
            foreach (var item in formats) item.IsActive = item.Id == entityLoadFormatId;
            await _context.SaveChangesAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> EntityLoadFormatDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            var query = _context.EntityLoadFormats.AsQueryable();
            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Description.Contains(searchValue));
            }

            var recordsFiltered = 0;

            recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    description = x.Description,
                    isActive = x.IsActive
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> EntityLoadSelect2(bool? onlyActive = false)
        {
            var query = _context.EntityLoadFormats.AsNoTracking();

            if (onlyActive.HasValue && onlyActive.Value)
                query = query.Where(x => x.IsActive);

            var result = await query
                .OrderByDescending(x => x.IsActive)
                .ThenBy(x => x.Description)
                .Select(x => new
                {
                    x.Id,
                    text = x.Description
                }).ToListAsync();

            return result;
        }

        public async Task<EntityLoadFormat> GetActive()
        {
            var format = await _context.EntityLoadFormats.FirstOrDefaultAsync(x => x.IsActive);
            return format;
        }
    }
}
