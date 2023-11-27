using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class EventTypeRepository : Repository<EventType>, IEventTypeRepository
    {
        public EventTypeRepository(AkdemicContext context) : base(context) { }

        public async Task<EventType> GetEventType(Guid eventType)
        {
            var EventType = await _context.EventTypes.Where(x => x.Id == eventType).FirstOrDefaultAsync();

            return EventType;
        }

        public async Task<List<EventType>> EventTypesDelete()
        {     
            var EventTypes = await _context.EventTypes.ToListAsync();

            return EventTypes;
        }

        public async Task<object> GetEventTypes()
        {
            var result = await _context.EventTypes.Select(
             x => new
             {
                 id = x.Id,
                 name = x.Name,
                 color = x.Color
             }).ToListAsync();

            return result;
        }
        public async Task<bool> FindAnyEventType(Guid id)
        {
            var eventTypeAny = false;
            eventTypeAny = await _context.EventTypes.AnyAsync(x => x.Id == id);
            return eventTypeAny;
        }
        public async Task<object> GetAllEventTypesInscription(bool isAll = false)
        {
            var eventTypes = await _context.EventTypes
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                }).ToListAsync();
            if (isAll)
            {
                eventTypes.Insert(0, new { id = Guid.Empty, text = "Todas" });
            }
            return eventTypes;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<EventType, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "1":
                    orderByPredicate = (x) => x.Color;
                    break;
            }

            var query = _context.EventTypes.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Color,
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

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
