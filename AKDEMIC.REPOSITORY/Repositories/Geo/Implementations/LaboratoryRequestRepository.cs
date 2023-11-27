using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Geo;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Geo.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Geo.Templates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Geo.Implementations
{
    public class LaboratoryRequestRepository : Repository<LaboratoyRequest>, ILaboratoryRequestRepository
    {
        public LaboratoryRequestRepository(AkdemicContext context) : base(context) { }
        
        public async Task<ATSTemplate> GetATS(Guid id)
        {
            var model = await _context.LaboratoyRequests
            .Include(x => x.Teacher.User)
            .Select(x => new ATSTemplate
            {
                Id = x.Id,
                Teacher = x.Teacher.User.FullName,
                Date = x.Date.ToDefaultTimeZone().ToShortDateString(),
                StartTime = x.StartTime.ToLocalDateTimeFormat(),
                EndTime = x.EndTime.ToLocalDateTimeFormat(),
            })
            .FirstOrDefaultAsync(x => x.Id == id);

            return model;
        }
        public async Task<object> GetRequestsByUser(string userId)
        {
            var query = _context.LaboratoyRequests
            .Where(x => x.TeacherId == userId)
            .AsQueryable();

            var result = await query.Select(x => new
            {
                id = x.Id,
                dateRequest = $"{x.Date.ToShortDateString()} {x.StartTime.ToLocalDateTimeFormat()} - {x.EndTime.ToLocalDateTimeFormat()}",
                description = x.Description,
                section = $"{x.Section.CourseTerm.Course.Name} - {x.Section.CourseTerm.Course.Code} - {x.Section.Code}",
                date = x.CreatedAt.ToLocalDateTimeFormat(),
                teacher = $"{x.Teacher.User.FullName}",
                string_state = ConstantHelpers.GEO.REQUEST.STATES[x.State],
                state = x.State,
                answered = x.State != ConstantHelpers.GEO.REQUEST.ACCEPTED && x.State != ConstantHelpers.GEO.REQUEST.DENIED,
                denied = x.State == ConstantHelpers.GEO.REQUEST.DENIED,
                createdAt = x.CreatedAt
            }).OrderByDescending(x => x.createdAt.Value).ToListAsync();

            return result;
        }
    }
}
