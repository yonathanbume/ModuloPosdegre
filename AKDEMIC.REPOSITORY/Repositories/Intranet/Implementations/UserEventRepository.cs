using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.UserEvent;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class UserEventRepository : Repository<UserEvent>, IUserEventRepository
    {
        public UserEventRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<ReportEventTemplate> GetUserEventByEventId(Guid eid)
        {
            return await _context.UserEvents.Include(x => x.Event).Where(x => x.EventId == eid)
                .Select(x => new ReportEventTemplate
                {
                    Id = x.EventId,
                    EventDate = x.Event.EventDate.ToLocalDateFormat(),
                    OrganizerName = x.Event.Organizer.FullName,
                    Name = x.Event.Name,
                    Description = x.Event.Description,
                    Place = x.Event.Place,
                    StartEndDate = x.Event.RegistrationStartDate.ToLocalDateFormat() + '-' + x.Event.RegistrationEndDate.ToLocalDateFormat(),
                    GeneralCost = Math.Round(Decimal.Multiply(Convert.ToDecimal(x.Event.UserEvents.Count(c => c.Absent == false)), x.Event.Cost), 0)
                }).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<UserEvent>> GetUserEventsByEventId(Guid eid)
        {
            return await _context.UserEvents.Include(x => x.Event).Where(x => x.EventId == eid).ToListAsync();
        }

        public async Task<List<StudentEnrolledTemplate>> GetRegisteredByEventId(Guid eid)
        {
            var result = await _context.UserEvents
               .Where(x => x.EventId == eid)
               .Select(x => new StudentEnrolledTemplate
               {
                   Name = x.User != null ? x.User.FullName : x.ExternalUser.FullName,
                   Email = x.User != null ? x.User.Email : x.ExternalUser.Email,
                   Dni = x.User != null ? x.User.Dni : x.ExternalUser.DocumentNumber,
                   PhoneNumber = x.User != null ? x.User.PhoneNumber : x.ExternalUser.PhoneNumber
               }).ToListAsync();

            return result;
        }

        public async Task<object> GetAssistanceDetailByEventId(Guid eid)
        {
            var result = await _context.UserEvents
                .Where(x => x.EventId == eid)
                .Select(x => new
                {
                    id = x.Id,
                    fullname = x.User != null ? x.User.FullName : x.ExternalUser.FullName,
                    email = x.User != null ? x.User.Email : x.ExternalUser.Email,
                    assistance = x.Absent
                }).ToListAsync();

            return result;
        }
        public async Task<List<UserEvent>> GetAllUserEvent()
        {
            var LstStudentEvents = await _context.UserEvents.ToListAsync();

            return LstStudentEvents;
        }

        public async Task<object> GetEventUserInscription()
        {
            var eventsuser = await _context.UserEvents.Select(x => new { eventId = x.EventId }).ToListAsync();

            return eventsuser;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<UserEvent> GetByExternalUserAndEventId(Guid eventId, Guid externalUserId)
        {
            var userEvent = await _context.UserEvents
                .Where(x => x.EventId == eventId && x.ExternalUserId == externalUserId)
                .FirstOrDefaultAsync();

            return userEvent;
        }

        public async Task<bool> IsUserSignedToEvent(string userId, Guid eventId)
        {
            return await _context.UserEvents.AnyAsync(x => x.UserId == userId && x.EventId == eventId);
        }

        public async Task<bool> ValidateRole(string userId, Guid eventId)
        {
            var eventRoles = await _context.EventRoles.Where(x => x.EventId == eventId).Select(x => x.RoleId).ToListAsync();
            var userRoles = await _context.UserRoles.Where(x => x.UserId == userId).Select(x => x.RoleId).ToListAsync();

            var isUserAllowed = eventRoles.Any(x => userRoles.Contains(x));

            return isUserAllowed;
        }

        public async Task<bool> ValidateCareer(string userId, Guid eventId)
        {
            //La carrera solo se validará para estudiantes y docentes
            var student = await _context.Students.Where(x => x.UserId == userId).AsNoTracking().FirstOrDefaultAsync();
            var teacher = await _context.Teachers.Where(x => x.UserId == userId).AsNoTracking().FirstOrDefaultAsync();

            var userCareers = new List<Guid>();

            if (student != null) userCareers.Add(student.CareerId);
            if (teacher != null && teacher.CareerId != null) userCareers.Add(teacher.CareerId.Value);

            var eventCareers = await _context.EventCareers.Where(x => x.EventId == eventId).Select(x => x.CareerId).ToListAsync();

            var isUserAllowed = true;

            //Si el usuario, no es docente, ni alumno, no hay necesidad de validar la carrera
            if (student != null  || teacher != null)
            {
                isUserAllowed = eventCareers.Any(x => userCareers.Contains(x));
            }

            return isUserAllowed;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserEventDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, string userId = null)
        {
            Expression<Func<UserEvent, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Event.Name);
                    break;
                case "1":
                    orderByPredicate = (x) => x.Event.Organizer.FullName;
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Event.Place);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Event.EventDate);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.Absent);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.Event.HasCertification);
                    break;

            }

            var query = _context.UserEvents
                .AsNoTracking();

            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(x => x.UserId == userId);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                var searchTrim = searchValue.ToUpper().Trim();
                query = query.Where(x => x.Event.Name.ToUpper().Contains(searchTrim));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    eventName = x.Event.Name,
                    organizer = x.Event.Organizer.FullName,
                    place = x.Event.Place,
                    x.Event.HasCertification,
                    x.Absent,
                    eventDate = x.Event.EventDate.ToLocalDateFormat(),
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

        public async Task<UserEventPDFTemplate> GetCertificateTemplate(Guid id)
        {
            var userEvent = await _context.UserEvents
                .Where(x => x.Id == id)
                .Select(x => new UserEventPDFTemplate
                {
                    EventName = x.Event.Name,
                    Place = x.Event.Place,
                    CertificateTitle = x.Event.EventCertification.Title,
                    CertificateContent = x.Event.EventCertification.Content,
                    EventDate = x.Event.EventDate.ToLocalDateFormat(),
                    FullName = x.User.FullName,
                    UserName = x.User.UserName
                })
                .FirstOrDefaultAsync();

            return userEvent;
        }
    }
}
