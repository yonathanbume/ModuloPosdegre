using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Event;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.EventRole;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(AkdemicContext context) : base(context) { }

        public async Task<List<EventTemplate>> GetEventsSiscoToHome()
        {

            var events = await _context.Events
                .OrderByDescending(x => x.EventDate)
                .Take(9)
                .Select(x => new EventTemplate
                {
                    Id = x.Id,
                    UrlPicture = null,
                    Date = x.EventDate.ToDefaultTimeZone().ToString("dd MMMM yyyy", new CultureInfo("es")),
                    Time = x.EventDate.ToLocalTimeFormat()
                }).ToListAsync();

            return events;
        }

        public async Task<List<EventTemplate>> GetEventsSiscoAllToHome()
        {
            var novelties = await _context.Events
                .Select(x => new EventTemplate
                {
                    Id = x.Id,
                    Name = x.Name,
                    Place = x.Place,
                    Description = x.Description,
                    Date = x.EventDate.ToDefaultTimeZone().ToString("dddd, dd MMMM yyyy", new CultureInfo("es-PE")),
                    Time = x.EventDate.ToLocalTimeFormat(),
                    UrlPicture = null
                }).ToListAsync();

            return novelties;
        }

        #region EVENTS DATA TABLE
        public async Task<DataTablesStructs.ReturnedData<EventDataTableTemplate>> GetEventsDataTable(DataTablesStructs.SentParameters sentParameters)
        {
            Expression<Func<Event, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Description);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.EventType.Name);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Organizer.Name);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.Place);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.EventDate);
                    break;
                case "6":
                    orderByPredicate = ((x) => x.RegistrationStartDate);
                    break;
                case "7":
                    orderByPredicate = ((x) => x.RegistrationEndDate);
                    break;
            }

            var query = _context.Events.AsNoTracking();


            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new EventDataTableTemplate
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Eventtype = x.EventType.Name,
                    Organizer = x.Organizer.FullName,
                    Place = x.Place,
                    Eventdate = x.EventDate.ToLocalDateFormat(),
                    RegistrationStartDate = x.RegistrationStartDate.ToLocalDateFormat(),
                    RegistrationEndDate = x.RegistrationEndDate.ToLocalDateFormat()
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<EventDataTableTemplate>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
        private async Task<DataTablesStructs.ReturnedData<EventDataTableTemplate>> GetEventsDataTable(
             DataTablesStructs.SentParameters sentParameters,
             Expression<Func<EventDataTableTemplate, EventDataTableTemplate>> selectPredicate = null,
             Expression<Func<EventDataTableTemplate, dynamic>> orderByPredicate = null,
             Func<EventDataTableTemplate, string[]> searchValuePredicate = null)
        {
            var query = _context.Events
                 .AsQueryable();



            var result = query
                .Select(x => new EventDataTableTemplate
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Eventtype = x.EventType.Name,
                    Organizer = x.Organizer.Name,
                    Place = x.Place,
                    Eventdate = x.EventDate.ToLocalDateFormat(),
                    RegistrationStartDate = x.RegistrationStartDate.ToLocalDateFormat(),
                    RegistrationEndDate = x.RegistrationEndDate.ToLocalDateFormat()
                });

            return await result.ToDataTables(sentParameters, selectPredicate);
        }
        private Expression<Func<EventDataTableTemplate, dynamic>> GetStudentRankingByTermDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.Name);
                //case "1":
                //    return ((x) => x.Code);
                //case "2":
                //    return ((x) => x.Career);
                //case "3":
                //    return ((x) => x.AcademicProgram);
                //case "4":
                //    return ((x) => x.Intents);
                //case "5":
                //    return ((x) => x.Grade);
                //case "6":
                //    return ((x) => x.Approbed);
                default:
                    return ((x) => x.Name);
            }
        }
        private Func<EventDataTableTemplate, string[]> GetStudentRankingByTermDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.Name  +"",
                //x.position+"",
                //x.weightedAverageGrade+"",
                //x.meritType+"",
                //x.credits+"",
                //x.code +"",
                //x.career+"",
                //x.campus+"",
                //x.academicYear +"",
            };
        }
        #endregion

        public async Task<ReportEventTemplate> GetUserEventByEventId(Guid eid)
        {
            return await _context.Events.Where(x => x.Id == eid)
                .Select (x => new ReportEventTemplate
                {
                    Id = x.Id,
                    EventDate = x.EventDate.ToLocalDateFormat(),
                    OrganizerName = x.Organizer.FullName,
                    Name = x.Name,
                    Description = x.Description,
                    Place = x.Place,
                    StartEndDate = x.RegistrationStartDate.ToLocalDateFormat() + '-' + x.RegistrationEndDate.ToLocalDateFormat(),
                    GeneralCost = Math.Round(Decimal.Multiply(Convert.ToDecimal(x.UserEvents.Count(c => c.Absent == false)), x.Cost), 0)
                }).FirstOrDefaultAsync();
        }

        public async Task<object> GetUserRoles()
        {
            var result = await _context.Roles.Select(x => new
            {
                id = x.Id,
                text = x.Name
            }).ToListAsync();

            return result;
        } 

        public async Task<DataTablesStructs.ReturnedData<object>> GetEvents(DataTablesStructs.SentParameters sentParameters, byte system, Guid eventTypeId, string searchValue = null, string organizerId = null, bool isAdmin = false)
        {
            Expression<Func<Event, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.Organizer.Name); break;
                case "2":
                    orderByPredicate = ((x) => x.EventType.Name); break;
                case "3":
                    orderByPredicate = ((x) => x.Description); break;
                case "4":
                    orderByPredicate = ((x) => x.Place); break;
                case "5":
                    orderByPredicate = ((x) => x.EventDate); break;
                case "6":
                    orderByPredicate = ((x) => x.RegistrationStartDate); break;
                case "7":
                    orderByPredicate = ((x) => x.RegistrationEndDate); break;
            }

            var query = _context.Events.Where(x => x.System == system).AsQueryable();

            if (!isAdmin)
            {
                if (!string.IsNullOrEmpty(organizerId) && !isAdmin)
                {
                    query = query.Where(x => x.OrganizerId == organizerId);
                }
            }


            if (eventTypeId != Guid.Empty)
            {
                query = query.Where(x => x.EventTypeId == eventTypeId);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()) ||
                                    x.Organizer.Name.ToUpper().Contains(searchValue.ToUpper()) ||
                                    x.Description.ToUpper().Contains(searchValue.ToUpper()));
            }


            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    nameEvent = x.Name,
                    description = x.Description,
                    organizer = x.Organizer.FullName,
                    type = x.EventType.Name,
                    place = x.Place,
                    isPublic = x.IsPublic,
                    eventDate = x.EventDate.ToLocalDateTimeFormat(),
                    registrationStartDate = x.RegistrationStartDate.ToLocalDateTimeFormat(),
                    registrationEndDate = x.RegistrationEndDate.ToLocalDateTimeFormat()
                })
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

        public async Task<EventInscriptionTemplate> GetEventInscriptionById(Guid id)
        {
            var result = await _context.Events
            .Where(x => x.Id == id)
            .Select(x => new EventInscriptionTemplate
            {
                Id = x.Id,
                Creator = x.Organizer.FullName,
                RegistrationStartDate = x.RegistrationStartDate.ToLocalDateFormat(),
                RegistrationEndDate = x.RegistrationEndDate.ToLocalDateFormat(),
                EventDate = x.EventDate.ToLocalDateFormat(),
                EventName = x.Name,
                Description = x.Description,
                EventTypeName = x.EventType.Name,    
                PathPicture = x.PathPicture,
                UrlVideo = x.UrlVideo,
                IsPublic = x.IsPublic,
                AllowedCareers = x.EventCareers.Select(x => x.Id).ToList(),
                AllowedRoles = x.EventRoles.Select(x => x.RoleId).ToList()
            }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<EventInscriptionTemplate>> GetTwoNextUpcomingEvents()
        {
            var today = DateTime.UtcNow;

            var events = await _context.Events
                .Where(x => today >= x.RegistrationStartDate && today <= x.RegistrationEndDate)
                .OrderBy(x => x.RegistrationStartDate)
                .Select(x => new EventInscriptionTemplate 
                {
                    Id = x.Id,
                    Creator = x.Organizer.FullName,
                    RegistrationStartDate = x.RegistrationStartDate.ToLocalDateFormat(),
                    RegistrationEndDate = x.RegistrationEndDate.ToLocalDateFormat(),
                    EventDate = x.EventDate.ToLocalDateFormat(),
                    EventName = x.Name,
                    Description = x.Description,
                    EventTypeName = x.EventType.Name,
                    PathPicture = x.PathPicture,
                    UrlVideo = x.UrlVideo
                })
                .Take(2)
                .ToListAsync();

            return events;
        }

        public async Task<Event> GetEventWithIncludeById(Guid id)
        {
            var result = await _context.Events
                .Include(x => x.EventCertification)
                .Where(x => x.Id == id).FirstOrDefaultAsync();

            return result;
        }

        public async Task<EventTemplate> GetEventData(Guid id)
        {
            var result = await _context.Events
                .Where(x => x.Id == id)
                .Select(x => new EventTemplate 
                {
                    Id = x.Id,
                    Date = x.EventDate.ToLocalDateFormat(),
                    Time = x.EventDate.ToLocalTimeFormat(),
                    Description = x.Description,
                    IsPublic = x.IsPublic,
                    Name = x.Name,
                    Place = x.Place,
                    Cost = x.Cost,
                    HasCertification = x.HasCertification,
                    System = x.System,
                    UrlPicture = x.PathPicture,
                    UrlVideo = x.UrlVideo,
                    EventCareers = x.EventCareers
                        .Select(y => new EventCareerTemplate 
                        {
                            Id = y.Id,
                            EventId = y.EventId,
                            CareerId = y.CareerId,
                            CareerName = y.Career.Name,
                            CareerCode = y.Career.Code
                        })
                        .ToList(),
                    EventRoles = x.EventRoles
                        .Select(y => new EventRoleTemplate 
                        {
                            Id = y.Id,
                            EventId = y.EventId,
                            RoleId = y.RoleId,
                            RoleName = y.Role.Name
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
