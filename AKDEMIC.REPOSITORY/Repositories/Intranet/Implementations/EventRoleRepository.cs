using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.EventRole;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class EventRoleRepository : Repository<EventRole>, IEventRoleRepository
    {
        public EventRoleRepository(AkdemicContext context) : base(context) { }


        public async Task<List<EventInscriptionTemplate>> GetEventsInscription(IList<string> roles, string userId, bool? isPublic = null, byte? system = null)
        {
            var query = _context.EventRoles.AsNoTracking();

            if (isPublic != null)
                query = query.Where(x => x.Event.IsPublic == isPublic);

            if (system != null)
                query = query.Where(x => x.Event.System == system);

            var events = await query
                    .Where(x => roles.Contains(x.Role.Name) && x.Event.RegistrationEndDate >= DateTime.UtcNow)
                    .Select(x => new EventInscriptionTemplate
                    {
                        EventTypeName = x.Event.EventType.Name,
                        EventTypeColor = x.Event.EventType.Color,
                        EventTypeId = x.Event.EventType.Id,
                        Id = x.Event.Id,
                        EventDate = x.Event.EventDate.ToLocalDateFormat(),
                        EventName = x.Event.Name,
                        Description = x.Event.Description,
                        PathPicture = x.Event.PathPicture,
                        SignedUp = x.Event.UserEvents.Any(ue => ue.UserId == userId)
                    }).ToListAsync();

            return events;
        }

        public async Task<List<EventInscriptionTemplate>> GetPublicEvents(string userId = null, byte? system = null)
        {
            //Si el evento es publico, se ignora roles, por lo cual no va en el filtro,
            //sin embargo el userid del aspnetusers se puede respetar en caso de ser estudiante y esta logeado

            var query = _context.Events.AsNoTracking();

            if (system != null)
                query = query.Where(x => x.System == system);

            var result = await query
                .Where(x => x.IsPublic && x.RegistrationEndDate >= DateTime.UtcNow)
                .Select(x => new EventInscriptionTemplate
                {
                    EventTypeName = x.EventType.Name,
                    EventTypeColor = x.EventType.Color,
                    EventTypeId = x.EventType.Id,
                    Id = x.Id,
                    EventDate = x.EventDate.ToLocalDateFormat(),
                    EventName = x.Name,
                    Description = x.Description,
                    PathPicture = x.PathPicture,
                    SignedUp = x.UserEvents.Any(ue => ue.UserId == userId)
                })
                .ToListAsync();


            return result;
        }

        public async Task<object> GetEventHome(IList<string> roles)
        {
            //var events = _context.EventRoles
            //    .Include(x => x.Role)
            //    .Include(x => x.Event)
            //    .Where(x => roles.Contains(x.Role.Name))
            //    .Take(3)
            //    .ToList()
            //    .Select(x => new
            //    {
            //        name = x.Event.Name,
            //        eventdate = x.Event.EventDate,
            //        createdat = x.Event.CreatedAt,
            //        place = x.Event.Place,
            //        day = ConstantHelpers.WEEKDAY.ENUM_TO_STRING(x.Event.EventDate.DayOfWeek),
            //        monthname = ConstantHelpers.MONTHS.VALUES[x.Event.EventDate.Month]
            //    })
            //    .OrderByDescending(x => x.createdat)
            //    .ToList();

            var events = await _context.EventRoles
                .Where(x => roles.Contains(x.Role.Name))
                .OrderBy(x => x.Event.CreatedAt)
                .Select(x => new
                {
                    name = x.Event.Name,
                    eventdate = x.Event.EventDate,
                    createdat = x.Event.CreatedAt,
                    place = x.Event.Place,
                    day = ConstantHelpers.WEEKDAY.ENUM_TO_STRING(x.Event.EventDate.DayOfWeek),
                    monthname = ConstantHelpers.MONTHS.VALUES[x.Event.EventDate.Month]
                })
                .Take(3)
                .ToListAsync();

            return events;
        }

        public async Task<List<EventRole>> GetAllEventRolesByEventId(Guid id)
        {
            return await _context.EventRoles.Where(x => x.EventId == id).ToListAsync();
        }
    }
}
