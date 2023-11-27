using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class EventFileRepository : Repository<EventFile>, IEventFileRepository
    {
        public EventFileRepository(AkdemicContext context) : base(context) { }

        public async Task<List<EventFile>> GetAllByEvent(Guid eventId)
            => await _context.EventFiles.Where(x => x.EventId == eventId).ToListAsync();

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid eventId)
        {
            var query = _context.EventFiles.Where(x => x.EventId == eventId).AsNoTracking();

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescending(x=>x.CreatedAt)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                  .Select(x => new 
                  {
                      x.Id,
                      x.UrlFile,
                      x.Name,
                      x.CreatedAt
                  })
                  .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
    }
}
